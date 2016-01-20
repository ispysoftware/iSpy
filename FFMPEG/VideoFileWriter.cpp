// iSpy FFMPEG Library
// http://www.ispyconnect.com
//
// Copyright © ispyconnect.com, 2013
#include "StdAfx.h"
#include "stdint.h"
#include "VideoFileWriter.h"
#include <vcclr.h>
#include <sstream>
#include <string>


#define MAX_AUDIO_PACKET_SIZE (128 * 1024)
using System::Runtime::InteropServices::Marshal;


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
		#include "libavutil\opt.h"

	}

}

namespace iSpy { namespace Video { namespace FFMPEG
{
#undef PixelFormat

#pragma region Some private FFmpeg related stuff hidden out of header file

static void write_video_frame( WriterPrivateData^ data );
static void open_video( WriterPrivateData^ data );
static void open_audio( WriterPrivateData^ data );
static void add_video_stream( WriterPrivateData^ data, int width, int height, int bitRate,
							  enum libffmpeg::AVCodecID codec_id, enum libffmpeg::AVPixelFormat pixelFormat, int framerate );
static void add_audio_stream( WriterPrivateData^ data, enum libffmpeg::AVCodecID codec_id);

ref struct WriterPrivateData
{
internal:
	
	libffmpeg::AVFormatContext*		FormatContext;
	libffmpeg::AVStream*			VideoStream;
	libffmpeg::AVStream*			AudioStream;
	libffmpeg::AVFrame*				VideoFrame;
	libffmpeg::AVFrame*				AudioFrame;

	struct libffmpeg::SwsContext*	ConvertContext;
	

	uint8_t*	AudioBuffer;

	int	AudioInputSampleSize;
	int AudioBufferSizeCurrent;
	int AudioBufferSize;
	int FrameNumber;
	bool IsConstantFramerate;
	bool Opened;
	bool Abort;
	gcroot<WriterPrivateData^>*				hdl;
	int										Timeout;
	int64_t									LastPacket; 
	int64_t									LastVideoPTS;
	int64_t									LastAudioPTS;
	int SampleRate;
	int AudioBitRate;
	int Channels;

	WriterPrivateData( )
	{
		FormatContext     = NULL;
		VideoStream       = NULL;
		AudioStream		  = NULL;
		VideoFrame        = NULL;
		AudioFrame		  = NULL;
		ConvertContext	  = NULL;
		LastPacket        = 0;
		Timeout			  = 5000;
		Abort			  = false;
		hdl				  = NULL;
		LastVideoPTS	  = -1000000;
		LastAudioPTS	  = -1000000;


		AudioInputSampleSize = NULL;
		AudioBufferSize = 1024 * 1024 * 4;
		AudioBuffer = new uint8_t[AudioBufferSize];
		AudioBufferSizeCurrent = 0;
		FrameNumber = 0;
		IsConstantFramerate = false;
		Opened = false;
	}
};
#pragma endregion

VideoFileWriter::VideoFileWriter( void ) :
    data( nullptr ), disposed( false )
{
	
}

static int interrupt_cb2(void *ctx) 
{ 
	gcroot<WriterPrivateData^>* pointer = (gcroot<WriterPrivateData^>*)(ctx);
	int64_t o = GetTickCount() - (*pointer)->LastPacket;
	int timeout = (*pointer)->Timeout;
	bool abort =  (*pointer)->Abort;
	////_RPT1( 0, "interval: %d\n", o );	
	//timeout after 5 seconds of no activity
	if (o > timeout || abort)	{
		//OutputDebugStringW(L"interrupted");
		return 1;
	}

	return 0;
} 

int log_averror(int errcode)
    {
            char *errbuf = (char *) calloc(AV_ERROR_MAX_STRING_SIZE, sizeof(char));
            libffmpeg::av_strerror(errcode, errbuf, AV_ERROR_MAX_STRING_SIZE);
            _RPT1( 0, "error: %s\n", errbuf );
            delete [] errbuf;
            return -1;
    }

bool VideoFileWriter::Open( String^ fileName, int width, int height)
{
	return Open( fileName, width, height, VideoCodec::Default );
}

bool VideoFileWriter::Open( String^ fileName, int width, int height, VideoCodec codec )
{
	return Open( fileName, width, height, codec,  1600000, 0 );
}

bool VideoFileWriter::Open( String^ fileName, int width, int height, VideoCodec codec, int bitRate, int framerate )
{
	return Open( fileName, width, height, codec,  bitRate, AudioCodec::None, framerate,0,0,0 );
}

char* ManagedStringToUnmanagedUTF8Char(String^ str)
{
    pin_ptr<const wchar_t> wch = PtrToStringChars(str);
    int nBytes = ::WideCharToMultiByte(CP_UTF8, NULL, wch, -1, NULL, 0, NULL, NULL);
    char* lpszBuffer = new char[nBytes];
    ZeroMemory(lpszBuffer, (nBytes) * sizeof(char)); 
    nBytes = ::WideCharToMultiByte(CP_UTF8, NULL, wch, -1, lpszBuffer, nBytes, NULL, NULL);
    return lpszBuffer;
}

bool VideoFileWriter::Open( String^ fileName, int width, int height, VideoCodec codec, int bitRate, AudioCodec audioCodec, int framerate, int AudioBitRate, int SampleRate, int Channels)
{
    CheckIfDisposed( );
	 char *errbuf = (char *) calloc(AV_ERROR_MAX_STRING_SIZE, sizeof(char));

	data = gcnew WriterPrivateData( );
	bool success = false;

	if ( ( ( width & 1 ) != 0 ) || ( ( height & 1 ) != 0 ) )
	{
		//Console::WriteLine("Video file resolution must be a multiple of two." );
		throw gcnew ArgumentException( "Video file resolution must be a multiple of two." );
	}
	if ( ( (int) codec < -1 ) || ( (int) codec >= CODECS_COUNT ) )
	{
		//Console::WriteLine("Invalid video codec is specified." );
		throw gcnew ArgumentException( "Invalid video codec is specified." );
	}

	m_width  = width;
	m_height = height;
	m_codec  = codec;
	m_audiocodec = audioCodec;
	m_bitRate = bitRate;
	
	char *nativeFileName= ManagedStringToUnmanagedUTF8Char(fileName);
	int i=0;
	try
	{
		libffmpeg::AVOutputFormat* outputFormat = libffmpeg::av_guess_format( NULL, nativeFileName, NULL );

		if ( !outputFormat )
		{
			outputFormat = libffmpeg::av_guess_format( "mp4", NULL, NULL );

			if ( !outputFormat )
			{
				//Console::WriteLine("Cannot find suitable output format." );
				throw gcnew Exception( "Cannot find suitable output format." );
			}
		}
		data->LastPacket = GetTickCount();
		data->hdl = new gcroot<WriterPrivateData^>(data);

		data->FormatContext = libffmpeg::avformat_alloc_context( );
		data->FormatContext->interrupt_callback.callback = interrupt_cb2;
		data->FormatContext->interrupt_callback.opaque = (void*)data->hdl;

		if ( !data->FormatContext )
		{
			//Console::WriteLine("Cannot allocate format context." );
			throw gcnew Exception( "Cannot allocate format context." );
		}
		data->FormatContext->oformat = outputFormat;

		add_video_stream( data, width, height, m_bitRate,
			( codec == VideoCodec::Default ) ? outputFormat->video_codec : (libffmpeg::AVCodecID) video_codecs[(int) codec],
			( codec == VideoCodec::Default ) ? libffmpeg::PIX_FMT_YUV420P : (libffmpeg::AVPixelFormat) pixel_formats[(int) codec], framerate );

		if (m_audiocodec!=AudioCodec::None)	{
			data->SampleRate=SampleRate;
			data->AudioBitRate = AudioBitRate;
			data->Channels = Channels;
			add_audio_stream(data,  (libffmpeg::AVCodecID) audio_codecs[(int)audioCodec]);
			data->AudioFrame=libffmpeg::av_frame_alloc();
		}

		open_video( data );

		if (audioCodec!=AudioCodec::None)
			open_audio( data );

		if ( !( outputFormat->flags & AVFMT_NOFILE ) )
		{
			i=libffmpeg::avio_open2( &data->FormatContext->pb, nativeFileName, AVIO_FLAG_WRITE,&data->FormatContext->interrupt_callback,NULL );
			if ( i < 0 )
			{
				//Console::WriteLine("Cannot open the video file." );
				throw gcnew System::IO::IOException( "Cannot create the video file. ("+i+")" );
			}
		}

		i = libffmpeg::avformat_write_header( data->FormatContext, NULL );
		if ( i < 0 )
		{
			//Console::WriteLine("Cannot write header - check disk space" );
			throw gcnew System::IO::IOException( "Cannot write header - check disk space ("+i+")" );
		}

		success = true;
		data->Opened = true;

	}
	finally
	{
		
		delete nativeFileName;
		if ( !success )
		{
			Close();
		}
	}
	return success;
}

void VideoFileWriter::Close()
{
	if (data!=nullptr && !disposed)
	{
		//_RPT1( 0, "FFMPEG:Closing: %d", 0 );
		int t=0;
		

		if ( data->FormatContext )
		{	
			if (data->Opened){
				Flush();
			}

			if ( data->FormatContext->pb != NULL )
			{
				libffmpeg::av_write_trailer( data->FormatContext );
				pin_ptr<libffmpeg::AVIOContext*> pinprt = &(data->FormatContext->pb);
				libffmpeg::avio_closep(pinprt);
				data->FormatContext->pb = NULL;
			}

			if ( data->AudioBuffer )
			{
				delete[] data->AudioBuffer;
				data->AudioBuffer = NULL;
			}		
							
			if ( data->VideoFrame )
			{
				libffmpeg::avpicture_free((libffmpeg::AVPicture *)data->VideoFrame);
				pin_ptr<libffmpeg::AVFrame*> pinprt = &(data->VideoFrame);
				libffmpeg::av_frame_free( pinprt);
				data->VideoFrame = NULL;
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
					if (stream && stream->codec && stream->codec->codec) {
						stream->discard = libffmpeg::AVDISCARD_ALL;
						libffmpeg::avcodec_close(stream->codec);
						libffmpeg::av_freep(&stream);
					}
				}
			}
			data->VideoStream = NULL;
			data->AudioStream = NULL;
			
			pin_ptr<libffmpeg::AVFormatContext*> pinprt = &(data->FormatContext);
			libffmpeg::av_freep( pinprt );		

			data->FormatContext = NULL;

		}


		if ( data->ConvertContext != NULL )
		{
			libffmpeg::sws_freeContext( data->ConvertContext );
			data->ConvertContext = NULL;
		}

		if (data->hdl)	{
			delete data->hdl;
			data->hdl = NULL;
		}

		delete data;
		//_RPT1( 0, "%d", 10 );
		data = nullptr;
	}
}


void VideoFileWriter::WriteAudio(BYTE* soundBuffer, int soundBufferSize, int64_t pts)
{
	CheckIfDisposed( );

	if ( data == nullptr || !data->Opened)
	{
		throw gcnew System::IO::IOException( "An audio file was not opened yet." );
	}
	if (pts<=data->LastAudioPTS)
	{
		//non sequential PTS
		throw gcnew ArgumentException("Non sequential audio PTS ("+pts+", last: "+data->LastAudioPTS+")");
		data->LastAudioPTS = pts;
	}
	AddAudioSamples(data, soundBuffer, soundBufferSize, pts);
}

void VideoFileWriter::WriteVideoFrame( Bitmap^ frame, int64_t pts)
{
    CheckIfDisposed( );

	if ( data == nullptr || !data->Opened)
	{
		throw gcnew System::IO::IOException( "A video file was not opened yet." );
	}
	if (pts<=data->LastVideoPTS)
	{
		//non sequential PTS
		throw gcnew ArgumentException("Non sequential video PTS ("+pts+", last: "+data->LastVideoPTS+")");
		data->LastVideoPTS = pts;
	}
	libffmpeg::AVCodecContext* codecContext = data->VideoStream->codec;

	if (!libffmpeg::avcodec_is_open(codecContext))
		throw gcnew System::IO::IOException("codec is not open");

	if ( ( frame->PixelFormat != PixelFormat::Format24bppRgb ) &&
	     ( frame->PixelFormat != PixelFormat::Format32bppArgb ) &&
		 ( frame->PixelFormat != PixelFormat::Format32bppPArgb ) &&
	 	 ( frame->PixelFormat != PixelFormat::Format32bppRgb ) &&
		 ( frame->PixelFormat != PixelFormat::Format8bppIndexed ) )
	{
		throw gcnew ArgumentException( "The provided bitmap must be 24 or 32 bpp color image or 8 bpp grayscale image." );
	}

	if ( ( frame->Width != m_width ) || ( frame->Height != m_height ) )
	{
		throw gcnew ArgumentException( "Bitmap size must be of the same as video size, which was specified on opening video file." );
	}

	BitmapData^ bitmapData = frame->LockBits( System::Drawing::Rectangle( 0, 0, m_width, m_height ),
		ImageLockMode::ReadOnly,
		( frame->PixelFormat == PixelFormat::Format8bppIndexed ) ? PixelFormat::Format8bppIndexed : PixelFormat::Format24bppRgb );

	uint8_t* ptr = reinterpret_cast<uint8_t*>( static_cast<void*>( bitmapData->Scan0 ) );

	uint8_t* srcData[4] = { ptr, NULL, NULL, NULL };
	int srcLinesize[4] = { bitmapData->Stride, 0, 0, 0 };

	int h = 0;
	
	if (!data->ConvertContext)	{
		libffmpeg::AVPixelFormat pfmt = libffmpeg::PIX_FMT_BGR24;

		if (frame->PixelFormat == PixelFormat::Format8bppIndexed)
		{
			pfmt = libffmpeg::AV_PIX_FMT_GRAY8;
		}

		data->ConvertContext = libffmpeg::sws_getCachedContext(data->ConvertContext, codecContext->width, codecContext->height, pfmt, codecContext->width, codecContext->height, codecContext->pix_fmt, SWS_FAST_BILINEAR, NULL, NULL, NULL);

	}
	h = libffmpeg::sws_scale(data->ConvertContext, srcData, srcLinesize, 0, m_height, data->VideoFrame->data, data->VideoFrame->linesize);

	if (h <= 0)
	{
		delete bitmapData;
		throw gcnew System::IO::IOException("Error scaling image");

	}
	frame->UnlockBits(bitmapData);

	if (!data->IsConstantFramerate)
	{
		data->VideoFrame->pts = pts;
	}
	else	{
		data->VideoFrame->pts = data->FrameNumber;
	}
	data->FrameNumber++;
	write_video_frame( data );
	delete bitmapData;
	bitmapData = nullptr;
}

void VideoFileWriter::Flush(void)
{
	if ( data != nullptr && data->Opened)
	{
		if (data->VideoStream && data->VideoStream->codec) {
			int out_size = 0;
			int ret = 0;

			while (libffmpeg::avcodec_is_open(data->VideoStream->codec)) {
				data->LastPacket = GetTickCount();
				libffmpeg::AVPacket packet = { 0 };
				libffmpeg::av_init_packet(&packet);

				int got_packet;
				ret = libffmpeg::avcodec_encode_video2( data->VideoStream->codec, &packet, NULL, &got_packet);
				if (ret<0 || !got_packet){
					libffmpeg::av_free_packet(&packet);
					break;
				}

				if (packet.size )	{
					if (packet.pts != AV_NOPTS_VALUE)
						packet.pts = av_rescale_q(packet.pts, data->VideoStream->codec->time_base, data->VideoStream->time_base);
					if (packet.dts != AV_NOPTS_VALUE)
						packet.dts = av_rescale_q(packet.dts, data->VideoStream->codec->time_base, data->VideoStream->time_base);

					packet.stream_index = data->VideoStream->index;
					// write the compressed frame to the media file

					ret = libffmpeg::av_interleaved_write_frame( data->FormatContext, &packet );
					if (ret < 0){
						libffmpeg::av_free_packet(&packet);
						break;
					}
				}

				libffmpeg::av_free_packet(&packet);
			}
			//libffmpeg::avcodec_flush_buffers(data->VideoStream->codec);
			
		}
		if (data->AudioStream && data->AudioStream->codec)	{
			int out_size = 0;
			int ret = 0;
			
			while (libffmpeg::avcodec_is_open(data->AudioStream->codec)) {
				data->LastPacket = GetTickCount();
				libffmpeg::AVPacket packet = { 0 };
				libffmpeg::av_init_packet(&packet);

				int got_packet;
  
    			ret = libffmpeg::avcodec_encode_audio2(data->AudioStream->codec, &packet, NULL, &got_packet);
				
				if (ret<0 || !got_packet)	{				
					libffmpeg::av_free_packet(&packet);
					break;
				}

				if (packet.size )	{
					if (packet.pts != AV_NOPTS_VALUE)
						packet.pts = av_rescale_q(packet.pts, data->AudioStream->codec->time_base, data->AudioStream->time_base);
					if (packet.dts != AV_NOPTS_VALUE)
						packet.dts = av_rescale_q(packet.dts, data->AudioStream->codec->time_base, data->AudioStream->time_base);

					packet.stream_index = data->AudioStream->index;
					packet.flags |= AV_PKT_FLAG_KEY;

					ret = libffmpeg::av_interleaved_write_frame( data->FormatContext, &packet );

					if (ret < 0){
						libffmpeg::av_free_packet(&packet);
						break;
					}
				}
				libffmpeg::av_free_packet(&packet);
			}
			//libffmpeg::avcodec_flush_buffers(data->AudioStream->codec);
			
		}
		
	}
}


void VideoFileWriter::AddAudioSamples( WriterPrivateData^ data,  BYTE* soundBuffer, int soundBufferSize, int64_t pts)
{
		if (!data->AudioStream || !data->AudioStream->codec || soundBufferSize<=0)
			 return;
		
		libffmpeg::AVCodecContext* codecContext = data->AudioStream->codec;

    	memcpy(data->AudioBuffer + data->AudioBufferSizeCurrent,  soundBuffer, soundBufferSize );
    	data->AudioBufferSizeCurrent += soundBufferSize;
    	
    	int nCurrentSize = data->AudioBufferSizeCurrent;
      	
    	int got_packet, ret;
    	int size = libffmpeg::av_samples_get_buffer_size(NULL, codecContext->channels,
    	                                          data->AudioInputSampleSize,
    	                                          codecContext->sample_fmt, 0);

        libffmpeg::av_frame_unref(data->AudioFrame);	

		data->AudioFrame->nb_samples = data->AudioInputSampleSize;
		
		libffmpeg::AVPacket packet = { 0 };

		uint8_t* pSoundBuffer = NULL;
		uint8_t* AudioBufferPlanar = NULL;
		
		if (data->Channels==1 || codecContext->sample_fmt!=libffmpeg::AV_SAMPLE_FMT_S16P)	{
			pSoundBuffer = data->AudioBuffer;
		}
		else
		{
			if (codecContext->sample_fmt==libffmpeg::AV_SAMPLE_FMT_S16P)	{
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
		
		/*if (!data->IsConstantFramerate)
		{
			if (!(codecContext->codec->capabilities & CODEC_CAP_DELAY)) {
				data->AudioFrame->pts = pts;
			}
		}*/
		

		while( nCurrentSize >= size)	{	
			libffmpeg::av_init_packet(&packet);					
    		ret = libffmpeg::avcodec_fill_audio_frame(data->AudioFrame, codecContext->channels, codecContext->sample_fmt, pSoundBuffer, size, 0);
    		if (ret<0)
    		{
				delete AudioBufferPlanar;
				libffmpeg::av_free_packet(&packet);
    			throw gcnew System::IO::IOException("error filling audio");
    		}
					
    		ret = libffmpeg::avcodec_encode_audio2(codecContext, &packet, data->AudioFrame, &got_packet);

			if (ret<0)	{
				delete AudioBufferPlanar;
				libffmpeg::av_free_packet(&packet);
				throw gcnew Exception( "Error while writing audio frame ("+ret+")" );
			}

			if (got_packet && packet.size )	{
				/*if (!(codecContext->codec->capabilities & CODEC_CAP_DELAY)) {
					packet.pts = data->AudioFrame->pts;
					libffmpeg::AVRational t = { 1, codecContext->sample_rate };
					packet.duration = av_rescale_q(data->AudioFrame->nb_samples,
												   t,
												   codecContext->time_base);
				}
				packet.dts = packet.pts;
*/
				if (packet.pts != AV_NOPTS_VALUE)
					packet.pts = av_rescale_q(packet.pts, codecContext->time_base, data->AudioStream->time_base);
				if (packet.dts != AV_NOPTS_VALUE)
					packet.dts = av_rescale_q(packet.dts, codecContext->time_base, data->AudioStream->time_base);

				packet.stream_index = data->AudioStream->index;
				packet.flags |= AV_PKT_FLAG_KEY;
				data->LastPacket = GetTickCount();
				if (libffmpeg::av_interleaved_write_frame(data->FormatContext, &packet) != 0)	{
					delete AudioBufferPlanar;
					libffmpeg::av_free_packet(&packet);
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

#pragma region Private methods
void write_video_frame( WriterPrivateData^ data )
{
	libffmpeg::AVCodecContext* codecContext = data->VideoStream->codec;


	int ret = 0;
	libffmpeg::AVPacket packet = { 0 };
	libffmpeg::av_init_packet(&packet);
	
	if ( data->FormatContext->oformat->flags & AVFMT_RAWPICTURE )
	{
        packet.flags        |= AV_PKT_FLAG_KEY;
        packet.stream_index  = data->VideoStream->index;
        packet.data          = data->VideoFrame->data[0];
        packet.size          = sizeof(libffmpeg::AVPicture);
        ret = libffmpeg::av_interleaved_write_frame(data->FormatContext, &packet);
	}
	else
	{
		
		int got_packet;
		packet.data = NULL;
		packet.size = 0;

		ret = libffmpeg::avcodec_encode_video2( codecContext, &packet, data->VideoFrame, &got_packet);
		if (ret<0) {
			libffmpeg::av_free_packet(&packet);
			throw gcnew Exception( "Error while writing video frame ("+ret+")" );
		}

		if (got_packet && packet.size )	{
			if (packet.pts != AV_NOPTS_VALUE)
                packet.pts = av_rescale_q(packet.pts, codecContext->time_base, data->VideoStream->time_base);
            if (packet.dts != AV_NOPTS_VALUE)
                packet.dts = av_rescale_q(packet.dts, codecContext->time_base, data->VideoStream->time_base);

			packet.stream_index = data->VideoStream->index;
			// write the compressed frame to the media file
			data->LastPacket = GetTickCount();
			ret = libffmpeg::av_interleaved_write_frame( data->FormatContext, &packet );
		}
	}
	libffmpeg::av_free_packet(&packet);

	if ( ret != 0 )
	{
		throw gcnew Exception( "Error while writing video frame ("+ret+")" );
	}
}


static libffmpeg::AVFrame* alloc_picture( enum libffmpeg::AVPixelFormat pix_fmt, int width, int height  )
{
	libffmpeg::AVFrame *picture = libffmpeg::av_frame_alloc();
    if (!picture || libffmpeg::avpicture_alloc((libffmpeg::AVPicture *)picture, pix_fmt, width, height) < 0) {
        libffmpeg::avpicture_free((libffmpeg::AVPicture *)picture);
		return NULL;
	}

	picture->width = width;
	picture->height = height;
	picture->format = pix_fmt;
    return picture;
}


void add_video_stream( WriterPrivateData^ data,  int width, int height, int bitRate,
					  enum libffmpeg::AVCodecID codecId, enum libffmpeg::AVPixelFormat pixelFormat, int framerate )
{
	libffmpeg::AVCodec *codec = libffmpeg::avcodec_find_encoder(codecId);
	libffmpeg::AVCodecContext* codecContext;
	// create new stream
	data->VideoStream = libffmpeg::avformat_new_stream( data->FormatContext, codec );
	if ( !data->VideoStream )
	{
		throw gcnew Exception( "Failed creating new video stream." );
	}


	codecContext = data->VideoStream->codec;

	codecContext->codec_id = codecId;
	codecContext->codec_type = libffmpeg::AVMEDIA_TYPE_VIDEO;

	//
	codecContext->width = width;
	codecContext->height = height;

	codecContext->time_base.num = 1;
	codecContext->time_base.den = 1000;

	if (framerate>0)	{ //avi - fixed fps
		codecContext->time_base.den = framerate;
		data->IsConstantFramerate = true;
	}
	
	codecContext->pix_fmt = pixelFormat;


	switch (codecContext->codec_id)	{
		case libffmpeg::AV_CODEC_ID_MPEG1VIDEO:
			codecContext->mb_decision = 2;
		break;
		case libffmpeg::AV_CODEC_ID_H264:
			codecContext->profile = FF_PROFILE_H264_CONSTRAINED_BASELINE;

			codecContext->coder_type = 1;
			codecContext->flags |= CODEC_FLAG_LOOP_FILTER;
			codecContext->scenechange_threshold = 40;
			codecContext->gop_size = 40;
			codecContext->max_b_frames = 0;
			codecContext->max_qdiff = 4;
			codecContext->me_method = 7;
			codecContext->me_range = 16;
			codecContext->me_cmp |= 1;
			codecContext->me_subpel_quality = 6;
			codecContext->qmin = 10;
			codecContext->qmax = 51;
			codecContext->qcompress = 0.6f;
			codecContext->keyint_min = 2;
			codecContext->trellis = 0;
			codecContext->level = 13;
			codecContext->refs = 1;
			break;
		case libffmpeg::AV_CODEC_ID_H265:
			codecContext->sample_aspect_ratio.num = width;
			codecContext->sample_aspect_ratio.den = height;
			break;
		default:
			codecContext->bit_rate = bitRate;
			break;

	}
	
	if( data->FormatContext->oformat->flags & AVFMT_GLOBALHEADER )
	{
		codecContext->flags |= CODEC_FLAG_GLOBAL_HEADER;
	}


	
}

void open_video( WriterPrivateData^ data )
{
	libffmpeg::AVCodecContext* codecContext = data->VideoStream->codec;
	libffmpeg::AVCodec* codec = libffmpeg::avcodec_find_encoder( codecContext->codec_id );

	if ( !codec )
	{
		throw gcnew Exception( "Cannot find video codec." );
	}
	libffmpeg::av_opt_set(codecContext->priv_data, "tune", "zerolatency", 0);

	libffmpeg::AVDictionary *param = 0;
	if (codecContext->codec_id == libffmpeg::AV_CODEC_ID_H265)	{
		libffmpeg::av_dict_set(&param, "x265-params", "qp=20", 0);
		libffmpeg::av_dict_set(&param, "preset", "ultrafast", 0);
		libffmpeg::av_dict_set(&param, "tune", "zero-latency", 0);
		libffmpeg::av_dict_set(&param, "qmin", "0", 0);
		libffmpeg::av_dict_set(&param, "qmax", "69", 0);
		libffmpeg::av_dict_set(&param, "qdiff", "4", 0);
	}

	int i = libffmpeg::avcodec_open2(codecContext, codec, &param);

	if ( i < 0 )
	{
		throw gcnew Exception( "Cannot open video codec ("+i+")" );
	}

	data->VideoFrame = alloc_picture( codecContext->pix_fmt, codecContext->width, codecContext->height );

	if ( !data->VideoFrame )
	{
		throw gcnew Exception( "Cannot allocate video picture." );
	}
}

void add_audio_stream( WriterPrivateData^ data,  enum libffmpeg::AVCodecID codec_id)
{
	  libffmpeg::AVCodec *codec = libffmpeg::avcodec_find_encoder(codec_id);
	  libffmpeg::AVCodecContext *codecContex;

	  data->AudioStream = libffmpeg::avformat_new_stream(data->FormatContext, codec);

	  if ( !data->AudioStream )
	  {
			throw gcnew Exception( "Failed creating new audio stream." );
	  }

	  // Codec.
	  codecContex = data->AudioStream->codec;
	  codecContex->codec_id = codec_id;
	  codecContex->codec_type = libffmpeg::AVMEDIA_TYPE_AUDIO;


	  codecContex->sample_fmt  = libffmpeg::AV_SAMPLE_FMT_S16;

	  if (codec_id==libffmpeg::AV_CODEC_ID_MP3)
		codecContex->sample_fmt  = libffmpeg::AV_SAMPLE_FMT_S16P;

	  // Set format
	  codecContex->sample_rate = data->SampleRate;

	  codecContex->channel_layout = libffmpeg::av_get_default_channel_layout(data->Channels);
	  codecContex->channels = libffmpeg::av_get_channel_layout_nb_channels(codecContex->channel_layout);
	  

	  codecContex->time_base.num = 1;
	  codecContex->time_base.den = 1000;// codecContex->sample_rate;
	  

	  codecContex->bits_per_raw_sample = 16;
	  
	  if( data->FormatContext->oformat->flags & AVFMT_GLOBALHEADER)
	  {
		codecContex->flags |= CODEC_FLAG_GLOBAL_HEADER;
	  }

	  if ((codec->capabilities & CODEC_CAP_EXPERIMENTAL) != 0) {
            codecContex->strict_std_compliance = -2;//libffmpeg::FF_COMPLIANCE_EXPERIMENTAL;
      }
}


void open_audio( WriterPrivateData^ data )
{
	libffmpeg::AVCodecContext* codecContext = data->AudioStream->codec;
	libffmpeg::AVCodec* codec = avcodec_find_encoder( codecContext->codec_id );

	if ( !codec )
	{
		throw gcnew Exception( "Cannot find audio codec." );
	}


   // Open it.
	int ret = libffmpeg::avcodec_open2(codecContext, codec, NULL);
	if (ret < 0) 
	{
		throw gcnew Exception( "Cannot open audio codec." );
	}

	if (codecContext->frame_size <= 1) 
	{
	// Ugly hack for PCM codecs (will be removed ASAP with new PCM
	// support to compute the input frame size in samples. 
		data->AudioInputSampleSize = data->AudioBufferSize / codecContext->channels;
		switch (codecContext->codec_id) 
		{
			case libffmpeg::CODEC_ID_PCM_S16LE:
			case libffmpeg::CODEC_ID_PCM_S16BE:
			case libffmpeg::CODEC_ID_PCM_U16LE:
			case libffmpeg::CODEC_ID_PCM_U16BE:
			data->AudioInputSampleSize >>= 1;
			break;
			default:
			break;
		}
		codecContext->frame_size = data->AudioInputSampleSize;
	} 
	else 
	{
		data-> AudioInputSampleSize = codecContext->frame_size;
	}
}

#pragma endregion
		
} } }

