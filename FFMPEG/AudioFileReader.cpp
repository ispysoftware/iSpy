// iSpy FFMPEG Library
// http://www.ispyconnect.com
//
// Copyright © ispyconnect.com, 2013
#include "StdAfx.h"
#include "stdint.h"
#include "AudioFileReader.h"
#include <exception>
#include <msclr\marshal.h>
#include <vector>
#include <sstream>
#include <string>

using namespace System;
using namespace msclr::interop;
using namespace System::Runtime::InteropServices;

namespace libffmpeg
{
	extern "C"
	{
		#pragma warning(disable:4635) 
		#pragma warning(disable:4244) 

		#include "libavformat\avformat.h"
		#include "libavcodec\avcodec.h"
		#include "libswresample\swresample.h"
		#include "libavutil\time.h"
	}
}
namespace iSpy { namespace Video { namespace FFMPEG
{
		ref struct ReaderPrivateData2
		{
		internal:
			libffmpeg::AVFormatContext*		FormatContext;
			libffmpeg::AVStream*			AudioStream;
			libffmpeg::AVCodecContext*		AudioCodecContext;
			libffmpeg::SwrContext*			swrContext;
			int64_t							LastPacket; 
			gcroot<ReaderPrivateData2^>*	hdl;
			int								Timeout;
			bool							Abort;
			bool							Opened;

			ReaderPrivateData2( )
			{
				FormatContext     = NULL;
				AudioStream       = NULL;
				AudioCodecContext      = NULL;
				swrContext		  = NULL;
				LastPacket        = 0;
				Timeout			  = 5000;
				Abort			  = false;
				Opened			  = false;
			}
		};


		AudioFileReader::AudioFileReader( void ) :
			data( nullptr ), disposed( false )
		{	
			/*libffmpeg::av_register_all( );
			libffmpeg::avcodec_register_all();
			libffmpeg::avformat_network_init();*/
		}
		
		static int interrupt_cb(void *ctx) 
		{ 
			gcroot<ReaderPrivateData2^>* pointer = (gcroot<ReaderPrivateData2^>*)(ctx);
			int64_t o = GetTickCount() - (*pointer)->LastPacket;
			int timeout = (*pointer)->Timeout;

			//_RPT1( 0, "interval: %d\n", o );	
			//timeout after 5 seconds of no activity
			if (o > timeout || (*pointer)->Abort)	{
				//OutputDebugStringW(L"interrupted");
				return 1;
			}

			return 0;
		}

		#pragma managed(push, off)

		static bool startsWith(const char *str,const char *pre)
		{
			size_t lenpre = strlen(pre),
					lenstr = strlen(str);
			return lenstr < lenpre ? false : strncmp(pre, str, lenpre) == 0;
		}	

		static libffmpeg::AVFormatContext* open_file(char* fileName, int timeout, int analyzeduration, char* cookies, char* useragent, char* headers, void* opaque)
		{
			libffmpeg::AVDictionary *options = NULL;

			char buffer [33];
			_itoa_s (analyzeduration*1000,buffer,10);
			
			libffmpeg::av_dict_set(&options, "analyzeduration", (const char*) buffer, 0);

			if (startsWith(fileName,"http") || startsWith(fileName,"ftp") || startsWith(fileName,"mmsh") || startsWith(fileName,"mms:"))	{
				//milliseconds...
				_itoa_s (timeout,buffer,10);

				libffmpeg::av_dict_set(&options, "timeout", (const char*) buffer, 0);
				libffmpeg::av_dict_set(&options, "stimeout", (const char*) buffer, 0);

				if (strlen(cookies) > 0)	{
					libffmpeg::av_dict_set(&options, "cookies", (const char*)cookies, 0);
				}

				if (strlen(useragent) > 0)	{
					libffmpeg::av_dict_set(&options, "user-agent", (const char*)useragent, 0);
				}

				if (strlen(headers) > 0)	{
					libffmpeg::av_dict_set(&options, "headers", (const char*)headers, 0);
				}

				//libffmpeg::av_dict_set(&options, "auth_type", "digest", 0);
				

				//future: add http specific flags:
				//multiple_requests
				//post_data
				//seekable
				//chunked_post
				//mime_type
			}
			if (startsWith(fileName,"tcp") || startsWith(fileName,"udp") || startsWith(fileName,"rtp") || startsWith(fileName,"sdp")  || startsWith(fileName,"ftp") || startsWith(fileName,"mmst"))	{
				//microseconds...
				_itoa_s (timeout*1000,buffer,10);
				libffmpeg::av_dict_set(&options, "timeout", (const char*) buffer, 0);
			}
			if (startsWith(fileName,"rtsp") || startsWith(fileName,"rtmp"))	{
				//stimeout instead of timeout
				_itoa_s (timeout*1000,buffer,10);
				libffmpeg::av_dict_set(&options, "stimeout", (const char*) buffer, 0);
			}

			libffmpeg::AVFormatContext* formatContext = libffmpeg::avformat_alloc_context( );

			formatContext->interrupt_callback.callback = interrupt_cb;
			formatContext->interrupt_callback.opaque = opaque;

			//formatContext->timestamp = GetTickCount();
			
			int ret=0;
			ret = libffmpeg::avformat_open_input( &formatContext, fileName, NULL, &options );
			if ( ret !=0 )
			{
				/*char* error = (char*)malloc(1024);
				wchar_t error2[1024];
				if (ret < 0)
				{
				   libffmpeg::av_strerror(ret, error, 1024);
				}*/
				return nullptr;
			}

			formatContext->flags|=AVFMT_FLAG_DISCARD_CORRUPT;

			return formatContext;
		}

		#pragma managed(pop)


		void AudioFileReader::Open( String^ fileName)
		{
			CheckIfDisposed( );
			
			data = gcnew ReaderPrivateData2( );
			data->Timeout = timeout;
			data->Abort = false;

			bool success = false;

			// convert specified managed String to unmanaged string
			IntPtr ptr = Marshal::StringToHGlobalAnsi( fileName );
			char* nativeFileName = reinterpret_cast<char*>( static_cast<void*>( ptr ) );

			IntPtr ptr2 = Marshal::StringToHGlobalAnsi(cookies);
			char* nativeCookies = reinterpret_cast<char*>(static_cast<void*>(ptr2));

			IntPtr ptr3 = Marshal::StringToHGlobalAnsi(useragent);
			char* nativeuseragent = reinterpret_cast<char*>(static_cast<void*>(ptr3));

			IntPtr ptr4 = Marshal::StringToHGlobalAnsi(headers);
			char* nativeheaders = reinterpret_cast<char*>(static_cast<void*>(ptr4));


			try
			{
				// open the specified audio file
				data->LastPacket = GetTickCount();
				data->hdl = new gcroot<ReaderPrivateData2^>(data);
				data->FormatContext = open_file(nativeFileName, timeout, analyzeduration, nativeCookies, nativeuseragent, nativeheaders, (void*)data->hdl);
				data->LastPacket = GetTickCount();
				if ( data->FormatContext == nullptr )
				{
					throw gcnew System::IO::IOException( "Cannot open the audio file." );
				}

				// retrieve stream information
				if ( libffmpeg::avformat_find_stream_info( data->FormatContext, NULL ) < 0 )
				{
					throw gcnew Exception( "Cannot find stream information." );
				}

				data->FormatContext->flags|=AVFMT_FLAG_NOBUFFER;

				data->LastPacket = GetTickCount();
				// search for the first audio stream
				for ( unsigned int i = 0; i < data->FormatContext->nb_streams; i++ )
				{
					if( data->FormatContext->streams[i]->codec->codec_type == libffmpeg::AVMEDIA_TYPE_AUDIO )
					{
						// get the pointer to the codec context for the audio stream
						data->AudioCodecContext = data->FormatContext->streams[i]->codec;
						data->AudioStream  = data->FormatContext->streams[i];
						break;
					}
				}
				if ( data->AudioStream == NULL )
				{
					throw gcnew Exception( "Cannot find audio stream in the specified file." );
				}

				// find decoder for the audio stream
				libffmpeg::AVCodec* codec = libffmpeg::avcodec_find_decoder( data->AudioCodecContext->codec_id );
				if ( codec == NULL )
				{
					throw gcnew Exception( "Cannot find codec to decode the audio stream." );
				}

				// open the codec
				if ( libffmpeg::avcodec_open2( data->AudioCodecContext, codec, NULL ) < 0 )
				{
					throw gcnew Exception( "Cannot open audio codec." );
				}
				data->LastPacket = GetTickCount();
				if (data->AudioCodecContext->time_base.num > 1000 && data->AudioCodecContext->time_base.den == 1)
					data->AudioCodecContext->time_base.den = 1000;

				data->AudioCodecContext->request_sample_fmt  = libffmpeg::AV_SAMPLE_FMT_S16;

				libffmpeg::AVSampleFormat sfmt=data->AudioCodecContext->sample_fmt;

				// allocate audio frame

				m_codecName = gcnew String( data->AudioCodecContext->codec->name );

				int chans = 1;
				if (data->AudioCodecContext->channels>1) //downmix
					chans = 2;

				m_channels = chans;

				m_sampleRate = data->AudioCodecContext->sample_rate;//data->CodecContext->sample_rate;
				m_bitsPerSample = 16;
				

				data->swrContext = libffmpeg::swr_alloc_set_opts(NULL,
														libffmpeg::av_get_default_channel_layout(chans),
														libffmpeg::AV_SAMPLE_FMT_S16,
														data->AudioCodecContext->sample_rate,
														libffmpeg::av_get_default_channel_layout(data->AudioCodecContext->channels),
														data->AudioCodecContext->sample_fmt,
														data->AudioCodecContext->sample_rate,
														0,
														NULL);
				libffmpeg::swr_init(data->swrContext);

				data->LastPacket = GetTickCount();
				success = true;
				data->Opened = true;
			}
			finally
			{
				System::Runtime::InteropServices::Marshal::FreeHGlobal( ptr );

				if ( !success )
				{
					Close( );
				}
			}
		}

		void AudioFileReader::Abort()	{
			if (data!=nullptr)
				data->Abort = true;
		}

		void AudioFileReader::Close(  )
		{
			if ( data != nullptr && !disposed)
            { 
				data->Abort = true;
				if ( data->FormatContext != NULL )
                {
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
						  }
						}
					}
					pin_ptr<libffmpeg::AVFormatContext*> pinprt = &(data->FormatContext);
                    libffmpeg::avformat_close_input( pinprt );
					data->FormatContext = NULL;
                }

				data->AudioStream       = NULL;
				data->AudioCodecContext = NULL;

				if ( data->swrContext != NULL )
                {
					pin_ptr<libffmpeg::SwrContext*> pinprt = &(data->swrContext);
                    libffmpeg::swr_free(pinprt);
					data->swrContext = NULL;
                }
												
				
				if (data->hdl)	{
					delete data->hdl;
					data->hdl = NULL;
				}

				delete data;
				data=nullptr;
            }
		}

		template <typename T>
		inline void free_vector(std::vector<T>& to_clear)
		{
			std::vector<T> v;
			v.swap(to_clear);
		}


		array<unsigned char>^ AudioFileReader::ReadAudioFrame(  )
		{
		   CheckIfDisposed( );

			if ( data == nullptr || !data->Opened)
			{
				throw gcnew System::IO::IOException( "Cannot read audio frames since audio file is not open." );
			}

			libffmpeg::AVPacket packet = {0};	
			libffmpeg::av_init_packet( &packet );
			libffmpeg::AVFrame *frame=libffmpeg::av_frame_alloc();
			libffmpeg::av_frame_unref(frame);

			do	
			{
			  if (libffmpeg::av_read_frame(data->FormatContext, &packet) < 0)	{
					//usually end of file
					break;
			  }
			  libffmpeg::AVPacket packet_temp = packet;
			  data->LastPacket = GetTickCount();

			  if(data->AudioStream!=NULL && packet.stream_index == data->AudioStream->index)
			  {
					int s = 0;
                    int sz=packet_temp.size;
					array<unsigned char>^ managedBuf = gcnew array<unsigned char>(192000*2);
					std::vector<unsigned char> buffer(192000*2);
					unsigned char* outPtrs[32] = { NULL };
					outPtrs[0] = &buffer[0];
					bool b = false;

					do  {
						int got_frame = 0;                      
						int in_used = libffmpeg::avcodec_decode_audio4(data->AudioCodecContext, frame, &got_frame, &packet_temp);
 
						if (in_used < 0 || got_frame == 0)
						{
							b=true;
							break;
						}

						int numSamplesOut = libffmpeg::swr_convert(data->swrContext,
								outPtrs,
								data->AudioCodecContext->sample_rate,
								(const unsigned char**)frame->data,
								frame->nb_samples);
 
						System::IntPtr iptr = System::IntPtr( &buffer[0] );
						System::Runtime::InteropServices::Marshal::Copy( iptr, managedBuf, s, s + (numSamplesOut*2*m_channels) );
						// *2 as it's always 16 bit audio coming out
						s+=numSamplesOut*2*m_channels;
 
						packet_temp.data += in_used;
						packet_temp.size -= in_used;        
					} while (packet_temp.size > 0);
							
					free_vector(buffer);
							
					if (b)
					{
						delete managedBuf;
						break;
					}

					libffmpeg::av_free_packet(&packet);
					libffmpeg::av_frame_free(&frame);
					Array::Resize(managedBuf, s);
					return managedBuf;	
			  }
			  libffmpeg::av_free_packet(&packet);
			} while(!data->Abort);

			if (!data->Abort)	{
				//break has been called
				libffmpeg::av_free_packet(&packet);
			}
			libffmpeg::av_frame_free(&frame);
			return nullptr;	
		}




} } }