/*
This program is distributed under the terms of the 'MIT license'. The text
of this licence follows...

Copyright (c) 2005 J.D.Medhurst (a.k.a. Tixy)

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in
all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.  IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
THE SOFTWARE.
*/

/**
@file

@brief Implementation of the IMA ADPCM audio coding algorithm
*/
#include "stdAfx.h"
#include "stdint.h"
#include "IMA_ADPCM.h"


extern "C"
{
		// disable warnings about badly formed documentation from FFmpeg, which don't need at all
		#pragma warning(disable:4635) 
		// disable warning about conversion int64 to int32
		#pragma warning(disable:4244) 

}


namespace iSpy { 
	namespace Video { 
		namespace FFMPEG
		{
			static const uint16_t IMA_ADPCMStepTable[89] =
			{
					7,	  8,	9,	 10,   11,	 12,   13,	 14,
				   16,	 17,   19,	 21,   23,	 25,   28,	 31,
				   34,	 37,   41,	 45,   50,	 55,   60,	 66,
				   73,	 80,   88,	 97,  107,	118,  130,	143,
				  157,	173,  190,	209,  230,	253,  279,	307,
				  337,	371,  408,	449,  494,	544,  598,	658,
				  724,	796,  876,	963, 1060, 1166, 1282, 1411,
				 1552, 1707, 1878, 2066, 2272, 2499, 2749, 3024,
				 3327, 3660, 4026, 4428, 4871, 5358, 5894, 6484,
				 7132, 7845, 8630, 9493,10442,11487,12635,13899,
				15289,16818,18500,20350,22385,24623,27086,29794,
				32767
			};


			static const int IMA_ADPCMIndexTable[8] =
			{
				-1, -1, -1, -1, 2, 4, 6, 8,
			};


			void IMA_ADPCM::EncodeInit(int16_t sample1, int16_t sample2)
			{
				PredictedValue = sample1;
				int delta = sample2-sample1;
				if(delta<0)
					delta = - delta;
				if(delta>32767)
					delta = 32767;
				int stepIndex = 0;
				while(IMA_ADPCMStepTable[stepIndex]<(unsigned)delta)
					stepIndex++;
				StepIndex = stepIndex;
			}


			
			unsigned IMA_ADPCM::Encode(int16_t pcm16)
			{


				int pred = PredictedValue;
				int stepIndex = StepIndex;

				int delta = pcm16-pred;
				unsigned value;
				if(delta>=0)
					value = 0;
				else
					{
					value = 8;
					delta = -delta;
					}

				int step = IMA_ADPCMStepTable[stepIndex];
				int diff = step>>3;
				if(delta>step)
					{
					value |= 4;
					delta -= step;
					diff += step;
					}
				step >>= 1;
				if(delta>step)
					{
					value |= 2;
					delta -= step;
					diff += step;
					}
				step >>= 1;
				if(delta>step)
					{
					value |= 1;
					diff += step;
					}

				if(value&8)
					pred -= diff;
				else
					pred += diff;
				if(pred<-0x8000)
					pred = -0x8000;
				else if(pred>0x7fff)
					pred = 0x7fff;
				PredictedValue = pred;

				stepIndex += IMA_ADPCMIndexTable[value&7];
				if(stepIndex<0)
					stepIndex = 0;
				else if(stepIndex>88)
					stepIndex = 88;
				StepIndex = stepIndex;

				return value;
			}


			int IMA_ADPCM::Decode(unsigned adpcm)
			{
				int stepIndex = StepIndex;
				int step = IMA_ADPCMStepTable[stepIndex];

				stepIndex += IMA_ADPCMIndexTable[adpcm&7];
				if(stepIndex<0)
					stepIndex = 0;
				else if(stepIndex>88)
					stepIndex = 88;
				StepIndex = stepIndex;

				int diff = step>>3;
				if(adpcm&4)
					diff += step;
				if(adpcm&2)
					diff += step>>1;
				if(adpcm&1)
					diff += step>>2;

				int pred = PredictedValue;
				if(adpcm&8)
					pred -= diff;
				else
					pred += diff;
				if(pred<-0x8000)
					pred = -0x8000;
				else if(pred>0x7fff)
					pred = 0x7fff;
				PredictedValue = pred;

				return pred;
			}


			unsigned IMA_ADPCM::Encode(uint8_t* dst, int dstOffset, const int16_t* src, unsigned srcSize)
			{
				// use given bit offset
				dst += dstOffset>>3;
				unsigned bitOffset = dstOffset&4;

				// make sure srcSize represents a whole number of samples
				srcSize &= ~1;

				// calculate end of input buffer
				const int16_t* end = (const int16_t*)((const uint8_t*)src+srcSize);

				while(src<end)
					{
					// encode a pcm value from input buffer
					unsigned adpcm = Encode(*src++);

					// pick which nibble to write adpcm value to...
					if(!bitOffset)
						*dst = adpcm;		// write adpcm value to low nibble
					else
						{
						unsigned b = *dst;		// get byte from ouput
						b &= 0x0f;			// clear bits of high nibble
						b |= adpcm<<4;		// or adpcm value into the high nibble
						*dst++ = (uint8_t)b;	// write value back to output and move on to next byte
						}

					// toggle which nibble in byte to write to next
					bitOffset ^= 4;
					}

				// return number bits written to dst
				return srcSize*2;
			}


			unsigned IMA_ADPCM::Decode(int16_t* dst, const uint8_t* src, int srcOffset, unsigned srcSize)
			{
				// use given bit offset
				src += srcOffset>>3;

				// calculate pointers to iterate output buffer
				int16_t* out = dst;
				int16_t* end = out+(srcSize>>2);

				while(out<end)
					{
					// get byte from src
					unsigned adpcm = *src;

					// pick which nibble holds a adpcm value...
					if(srcOffset&4)
						{
						adpcm >>= 4;  // use high nibble of byte
						++src;		  // move on a byte for next sample
						}

					*out++ = Decode(adpcm);  // decode value and store it

					// toggle which nibble in byte to write to next
					srcOffset ^= 4;
					}

				// return number of bytes written to dst
				return out-dst;
			}


			unsigned IMA_ADPCM::EncodeFoscam(unsigned char * raw, int len, unsigned char * encoded)
			{
				short * pcm = (short *)raw;
				int cur_sample;
				int i;
				int delta;
				int sb;
				int code;
				len >>= 1;
	
				for (i = 0;i < len;i ++)
				{
					cur_sample = pcm[i]; 
					delta = cur_sample - PredictedValue; 
					if (delta < 0)
					{
						delta = -delta;
						sb = 8;	
					}
					else 
					{
						sb = 0;
					}	
					code = 4 * delta / IMA_ADPCMStepTable[StepIndex];	
					if (code>7) 
						code=7;	
		
					delta = (IMA_ADPCMStepTable[StepIndex] * code) / 4 + IMA_ADPCMStepTable[StepIndex] / 8;
					if (sb) 
						delta = -delta;
					PredictedValue += delta;	
					if (PredictedValue > 32767)
						PredictedValue = 32767;
					else if (PredictedValue < -32768)
						PredictedValue = -32768;
					//* pre_sample = cur_sample;
		
					StepIndex += IMA_ADPCMIndexTable[code];
					if (StepIndex < 0) 
						StepIndex = 0;
					else if (StepIndex > 88) 
						StepIndex = 88;
		
					if (i & 0x01)
						encoded[i >> 1] |= code | sb;
					else
						encoded[i >> 1] = (code | sb) << 4;
				}
				return len/2;
			}
		}
	}
}
