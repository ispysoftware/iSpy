// iSpy FFMPEG Library
// http://www.ispyconnect.com
//
// Copyright © ispyconnect.com, 2013
#include "StdAfx.h"
#include "stdint.h"
#include "AudioFileWriter.h"
#include <vcclr.h>
#include <string>

#define MAX_AUDIO_PACKET_SIZE (128 * 1024)

namespace libffmpeg
{
	extern "C"
	{
		#pragma warning(disable:4635) 
		#pragma warning(disable:4244) 

		#include "libavformat\avformat.h"
		#include "libavformat\avio.h"
		#include "libavcodec\avcodec.h"
		#include "libswscale\swscale.h"
		#include "libswresample\swresample.h"
	}
}

namespace iSpy { namespace Video { namespace FFMPEG
{
#pragma region Some private FFmpeg related stuff hidden out of header file

static void open_audio( AudioWriterPrivateData^ data );
static void add_audio_sample( AudioWriterPrivateData^ data, BYTE* soundBuffer, int soundBufferSize);

ref struct AudioWriterPrivateData
{
internal:
	libffmpeg::AVFormatContext*		FormatContext;
	libffmpeg::AVStream*			AudioStream;
	uint8_t*						AudioBuffer;
	libffmpeg::AVFrame*				AudioFrame;

	int AudioBufferSizeCurrent;
	int AudioBufferSize;

	int	AudioInputSampleSize;
	int BitRate;
	int SampleRate;
	int Channels;
	double audio_pts;
	bool NeedsFlush;

	AudioWriterPrivateData( )
	{
		FormatContext     = NULL;
		AudioStream		  = NULL;
		AudioFrame		  = NULL;
		AudioInputSampleSize = NULL;
		AudioBufferSize = 1024 * 1024 * 4;
		AudioBuffer = new uint8_t[AudioBufferSize];
		AudioBufferSizeCurrent = 0;
		audio_pts = 0;
		NeedsFlush = false;
	}
};
#pragma endregion
//static void avlog_cb(void *, int level, const char * szFmt, va_list varg) {
//    //do nothing...
//	std::string str(szFmt);
//	Console::WriteLine(gcnew String(str.c_str()));
//}

AudioFileWriter::AudioFileWriter( void ) :
    data( nullptr ), disposed( false )
{
	//libffmpeg::av_register_all( );
	//libffmpeg::avcodec_register_all();
}

void AudioFileWriter::Open( String^ fileName)
{
	Open( fileName, AudioCodec::MP3,64000,22050,1 );
}

char* ManagedStringToUnmanagedUTF8Char2(String^ str)
{
    pin_ptr<const wchar_t> wch = PtrToStringChars(str);
    int nBytes = ::WideCharToMultiByte(CP_UTF8, NULL, wch, -1, NULL, 0, NULL, NULL);
    char* lpszBuffer = new char[nBytes];
    ZeroMemory(lpszBuffer, (nBytes) * sizeof(char)); 
    nBytes = ::WideCharToMultiByte(CP_UTF8, NULL, wch, -1, lpszBuffer, nBytes, NULL, NULL);
    return lpszBuffer;
}

void AudioFileWriter::Open( String^ fileName, AudioCodec audioCodec, int BitRate, int SampleRate, int Channels)
{
    CheckIfDisposed( );
	Close( );
	

	data = gcnew AudioWriterPrivateData( );
	data->BitRate = BitRate;
	data->SampleRate = SampleRate;
	data->Channels = Channels;

	bool success = false;

	m_audiocodec  = audioCodec;
	
	// convert specified managed String to unmanaged string
	char *nativeFileName= ManagedStringToUnmanagedUTF8Char2(fileName);
	//libffmpeg::av_log_set_callback(avlog_cb);
	try
	{
		libffmpeg::AVOutputFormat *fmt;
		libffmpeg::AVFormatContext *oc;
		libffmpeg::AVCodecContext *c;

		libffmpeg::avformat_alloc_output_context2(&oc, NULL, "mp3", nativeFileName);
		if (!oc)
		{
			throw gcnew Exception( "Cannot find suitable output format." );
		}
		fmt = oc->oformat;
		libffmpeg::AVCodec* codec = libffmpeg::avcodec_find_encoder(fmt->audio_codec);

		data->AudioStream = libffmpeg::avformat_new_stream(oc, codec);
		if(!data->AudioStream)
			throw gcnew Exception( "Cannot find suitable output format." );

		c =data->AudioStream->codec;
		c->codec_id = fmt->audio_codec;
		c->codec_type = libffmpeg::AVMEDIA_TYPE_AUDIO;
		c->sample_fmt = libffmpeg::AV_SAMPLE_FMT_S16P;

		//c->bit_rate = data->BitRate;
		c->sample_rate = data->SampleRate;
		
		c->channel_layout = libffmpeg::av_get_default_channel_layout(data->Channels);
		c->channels = libffmpeg::av_get_channel_layout_nb_channels(c->channel_layout);

		
        c->time_base.num = 1;
		c->time_base.den =  data->SampleRate; //90000
		c->bits_per_raw_sample = 16;

		// Some formats want stream headers to be separate.
		if( oc->oformat->flags & AVFMT_GLOBALHEADER)
		{
			c->flags |= CODEC_FLAG_GLOBAL_HEADER;
		}
		if ((codec->capabilities & CODEC_CAP_EXPERIMENTAL) != 0) {
            c->strict_std_compliance = -2;//libffmpeg::FF_COMPLIANCE_EXPERIMENTAL;
        }

		data->FormatContext = oc;
		libffmpeg::av_dump_format(data->FormatContext, 0, nativeFileName, 1);
		//

		int ret = libffmpeg::avcodec_open2(c, codec, NULL);
		
		if (ret < 0) {
			throw gcnew Exception( "Could not open audio codec");
	    }

		if (c->codec->capabilities & CODEC_CAP_VARIABLE_FRAME_SIZE)
	        data->AudioInputSampleSize = 10000;
	    else
		{
	        data->AudioInputSampleSize = c->frame_size;	
		}

		if (c->frame_size <= 1) 
		  {
			// Ugly hack for PCM codecs (will be removed ASAP with new PCM
			// support to compute the input frame size in samples. 
			data->AudioInputSampleSize = data->AudioBufferSize / c->channels;
			switch (c->codec_id) 
			{
			  case libffmpeg::AV_CODEC_ID_PCM_S16LE:
			  case libffmpeg::AV_CODEC_ID_PCM_S16BE:
			  case libffmpeg::AV_CODEC_ID_PCM_U16LE:
			  case libffmpeg::AV_CODEC_ID_PCM_U16BE:
				data->AudioInputSampleSize >>= 1;
				break;
			  default:
				break;
			}
			c->frame_size = data->AudioInputSampleSize;
		  } 
		  else 
		  {
		   data-> AudioInputSampleSize = c->frame_size;
		  }
		
		
		if (!(fmt->flags & AVFMT_NOFILE)) {
			if (libffmpeg::avio_open(&oc->pb, nativeFileName, AVIO_FLAG_WRITE) < 0) {
				throw gcnew System::IO::IOException( "Cannot open the audio file for writing." );
			}
		}
 
		/* Write the stream header, if any. */
		if (libffmpeg::avformat_write_header(oc, NULL) < 0) {
			throw gcnew System::IO::IOException( "Error writing header." );
		}

		data->AudioFrame=libffmpeg::av_frame_alloc();
		data->NeedsFlush = true;
		success = true;
	}
	finally
	{
		delete nativeFileName;

		if ( !success )
		{
			Close( );
		}
	}
}

void AudioFileWriter::Close( )
{
	if ( data != nullptr && !disposed)
	{
		if ( data->FormatContext )
		{
			if (data->NeedsFlush)	{
				if (libffmpeg::avcodec_is_open(data->AudioStream->codec))
					libffmpeg::avcodec_flush_buffers(data->AudioStream->codec);
			}
			
			if ( data->FormatContext->pb != NULL )
			{
				libffmpeg::av_write_trailer( data->FormatContext );
				libffmpeg::avio_close( data->FormatContext->pb );
				data->FormatContext->pb = NULL;
			}

			if ( data->AudioBuffer )
			{
				delete[] data->AudioBuffer;
				data->AudioBuffer = NULL;
			}		

			if ( data->AudioFrame )
			{
				pin_ptr<libffmpeg::AVFrame*> pinprt = &(data->AudioFrame);
				libffmpeg::av_frame_free( pinprt );
				data->AudioFrame = NULL;
			}

			if (data->FormatContext->streams) {
				for (int i = data->FormatContext->nb_streams - 1; i >= 0; --i) {
					libffmpeg::AVStream* stream = data->FormatContext->streams[i];

					// The conditions for calling avcodec_close():
					// 1. AVStream is alive.
					// 2. AVCodecContext in AVStream is alive.
					// 3. AVCodec in AVCodecContext is alive.
					//
					// Closing a codec context without prior avcodec_open2() will result in
					// a crash in FFmpeg.
					if (stream && stream->codec && stream->codec->codec) {
						stream->discard = libffmpeg::AVDISCARD_ALL;
						libffmpeg::avcodec_close(stream->codec);
						libffmpeg::av_freep(&stream);
					}
				}
			}

			data->AudioStream = NULL;
			
			pin_ptr<libffmpeg::AVFormatContext*> pinprt = &(data->FormatContext);
			libffmpeg::av_freep( pinprt );	

			data->FormatContext = NULL;			
		}

		delete data;
		data = nullptr;
	}
}


void AudioFileWriter::WriteAudio(BYTE* soundBuffer, int soundBufferSize)
{
	CheckIfDisposed( );

	if ( data == nullptr )
	{
		throw gcnew System::IO::IOException( "An audio file was not opened yet." );
	}

	add_audio_sample(data, soundBuffer, soundBufferSize);
}

void AudioFileWriter::Flush(void)
{
	if ( data != nullptr )
	{
		//shouldn't need to flush audio...
	}
}


#pragma region Private methods
    void add_audio_sample( AudioWriterPrivateData^ data, BYTE* soundBuffer, int soundBufferSize)
    {
    	libffmpeg::AVCodecContext* c = data->AudioStream->codec;

    	memcpy(data->AudioBuffer + data->AudioBufferSizeCurrent,  soundBuffer, soundBufferSize );
    	data->AudioBufferSizeCurrent += soundBufferSize;
    	
    	int nCurrentSize    = data->AudioBufferSizeCurrent;
    
    	
    	
    	int got_packet, ret;
    	int size = libffmpeg::av_samples_get_buffer_size(NULL, c->channels,
    	                                          data->AudioInputSampleSize,
    	                                          c->sample_fmt, 0);

		
        libffmpeg::av_frame_unref(data->AudioFrame);

    	data->AudioFrame->nb_samples = data->AudioInputSampleSize;
		
		libffmpeg::AVPacket packet = { 0 };
	
		uint8_t* pSoundBuffer;
		uint8_t* AudioBufferPlanar = NULL;
		
		if (data->Channels==1 || c->sample_fmt!=libffmpeg::AV_SAMPLE_FMT_S16P)	{
			pSoundBuffer = data->AudioBuffer;
		}
		else
		{
			if (c->sample_fmt==libffmpeg::AV_SAMPLE_FMT_S16P)	{
				//convert to planar
				AudioBufferPlanar = new uint8_t[nCurrentSize];
				int offset = size/data->Channels;

				int j=0;
				int p=0;
				while (p+size<=nCurrentSize)	{
					for(int i=0;i<size;i+=4)	{
						AudioBufferPlanar[j] = data->AudioBuffer[p+i];
						AudioBufferPlanar[j+1] = data->AudioBuffer[p+i+1];
						AudioBufferPlanar[j+offset] = data->AudioBuffer[p+i+2];
						AudioBufferPlanar[j+offset+1] = data->AudioBuffer[p+i+3];
						j+=2;
					}
					j=j+offset;
					p=p+size;
				}
				pSoundBuffer = (uint8_t*) AudioBufferPlanar;	
			}
			else
				pSoundBuffer = data->AudioBuffer;
		}

		while( nCurrentSize >= size)	{					
    		ret = libffmpeg::avcodec_fill_audio_frame(data->AudioFrame, c->channels, c->sample_fmt, pSoundBuffer, size, 0);

    		if (ret<0)
    		{
				delete AudioBufferPlanar;
    			throw gcnew System::IO::IOException("error filling audio");
    		}
   			libffmpeg::av_init_packet(&packet);
    
    		ret = libffmpeg::avcodec_encode_audio2(c, &packet, data->AudioFrame, &got_packet);
    
    		if (ret<0)	{
				libffmpeg::av_free_packet(&packet);
				delete AudioBufferPlanar;
				throw gcnew System::IO::IOException("error encoding audio");
			}

    		if (packet.size && got_packet)	{
    			packet.stream_index = data->AudioStream->index;
    
    			if (packet.pts != AV_NOPTS_VALUE)
					packet.pts = libffmpeg::av_rescale_q(packet.pts, c->time_base, data->AudioStream->time_base);
				if (packet.dts != AV_NOPTS_VALUE)
					packet.dts = av_rescale_q(packet.dts, c->time_base, data->AudioStream->time_base);
   
    			packet.flags |= AV_PKT_FLAG_KEY;
    
    			if (libffmpeg::av_interleaved_write_frame(data->FormatContext, &packet) != 0)	{
					libffmpeg::av_free_packet(&packet);
					delete AudioBufferPlanar;
					throw gcnew System::IO::IOException("unable to write audio frame.");
				}

				libffmpeg::av_free_packet(&packet);
				pSoundBuffer += size;
				
    		}
			nCurrentSize -= size;  
			
    		
    	}

		delete AudioBufferPlanar;

		memcpy(data->AudioBuffer, data->AudioBuffer + data->AudioBufferSizeCurrent - nCurrentSize, nCurrentSize);	
		data->AudioBufferSizeCurrent = nCurrentSize;
    }

#pragma endregion
		
} } }

