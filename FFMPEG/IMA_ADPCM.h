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

extern "C"
{
		// disable warnings about badly formed documentation from FFmpeg, which don't need at all
		#pragma warning(disable:4635) 
		// disable warning about conversion int64 to int32
		#pragma warning(disable:4244) 
}

namespace iSpy { namespace Video { namespace FFMPEG
{
#ifndef IMA_ADPCM_H
#define IMA_ADPCM_H

/**
@defgroup ima_adpcm Audio Codec - IMA ADPCM
@{
*/

/**
@brief A class which implements the IMA ADPCM audio coding algorithm.

Typically, IMA ADPCM data is stored as blocks of data with the #PredictedValue
and #StepIndex values held in a header to each block. When decoding, these values
should be writen to this class to initialise the decoding of a block. When encoding,
these values should be writen out to a block's header prior to encoding the samples
in the block.

Note, when IMA ADPCM data is stored in Microsoft WAV files, the #PredictedValue
found in the block's header is used as the first sample for that block. The first ADPCM
value in the block's data then represents the second sample.

@version 2006-05-20
	- Changed code to use standard typedefs, e.g. replaced uint8 with uint8_t, and made use of size_t.
*/
public ref class IMA_ADPCM
	{
public:
	/**
	Initialise the #PredictedValue and #StepIndex members to the
	optimum values for encoding an audio stream whoes first two PCM samples have the
	values given. Use of this method at the start of audio stream encoding gives
	improved accuracy over a naive initialisation which sets #PredictedValue
	and #StepIndex to predetermined constant values.

	@param sample1 The first PCM sample in the audio stream.
	@param sample2 The second PCM sample in the audio stream.
	*/
	void EncodeInit(int16_t sample1,int16_t sample2);
	unsigned EncodeFoscam(unsigned char * raw, int len, unsigned char * encoded);
	/**
	Encode a single linear PCM sample as an ADPCM value.

	@param pcm16 The PCM value to encode.

	@return The 4 least significan bits represent the encoded ADPCM value.
			Other bits are zero.

	@post #PredictedValue and #StepIndex are updated ready
		  for encoding the next sample.
	*/
	unsigned Encode(int16_t pcm16);

	/**
	Decode a single ADPCM value into a 16bit linear PCM value.

	@param adpcm The 4 least significan bits represent ADPCM value to encode.
				 Other bits are ignored.

	@return The decoded 16 bit PCM value sign extended to an int.

	@post #PredictedValue and #StepIndex are updated ready
		  for decoding the next sample.
	*/
	int Decode(unsigned adpcm);

	/**
	Encode a buffer of 16 bit uniform PCM values into ADPCM values.

	Two ADPCM values are stored in each byte. The value stored in bits 0-3
	corresponds to the sample preceding that stored in bits 4-7.
	Note, if the last encoded ADPCM value is stored in bits 0-3, then bits 4-7
	will be cleared to zero.

	@param dst		 Pointer to location to store ADPCM values.
	@param dstOffset Offset from \a dst, in number-of-bits, at which the decoded values
					 will be stored. I.e. the least significant bit of the first ADPCM
					 value will be stored in byte
					 @code	 dst[dstOffset>>3]	 @endcode
					 at bit position
					 @code	 dstOffset&7		 @endcode
					 Where the bit 0 is the least significant bit in a byte
					 and bit 7 is the most significant bit.
					 The value of \a dstOffset must be a multiple of 4.
	@param src		 Pointer to the buffer of PCM values to be converted.
	@param srcSize	 The size, in bytes, of the buffer at \a src.
					 Must be a multiple of 2.

	@return 		 The number of bits which were stored at dst.
	*/
	
	unsigned Encode(uint8_t* dst, int dstOffset, const int16_t* src, unsigned srcSize);

	/**
	Decode a buffer of ADPCM values into 16 bit uniform PCM values.

	Two ADPCM values are stored in each byte. The value stored in bits 0-3
	corresponds to the sample preceding that stored in bits 4-7.

	@param dst		 Pointer to location to store PCM values.
	@param src		 Pointer to the buffer of ADPCM values to be converted.
	@param srcOffset Offset from \a src, in number-of-bits, from which the ADPCM values
					 will be read. I.e. the least significant bit of the first ADPCM
					 value will be read from byte
					 @code	 src[srcOffset>>3]	 @endcode
					 at bit position
					 @code	 srcOffset&7		 @endcode
					 Where the bit 0 is the least significant bit in a byte
					 and bit 7 is the most significant bit.
					 The value of \a srcOffset must be a multiple of 4.
	@param srcSize	 The number of bits to be read from the buffer at \a src.
					 Must be a multiple of the size of 4.

	@return 		 The number of bytes which were stored at dst.
	*/
	unsigned Decode(int16_t* dst, const uint8_t* src, int srcOffset, unsigned srcSize);
public:
	/**
	The predicted value of the next sample.
	Typically, this value is read from the header, or written to the header, of a block
	of ADPCM values.
	*/
	int16_t PredictedValue;

	/**
	The step index used for the next ADPCM value
	Typically, this value is read from the header, or written to the header, of a block
	of ADPCM values.
	*/
	uint8_t StepIndex;
	};

/** @} */ // End of group

#endif
}}}
