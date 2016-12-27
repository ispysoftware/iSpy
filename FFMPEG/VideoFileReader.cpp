// iSpy FFMPEG Library
// http://www.ispyconnect.com
//
// Copyright © ispyconnect.com, 2013
#include "StdAfx.h"
#include "stdint.h"
#include "VideoFileReader.h"
#include <exception>
#include <msclr\marshal.h>
#include <vector>
#include <sstream>
#include <string>
 
using namespace System;
using namespace msclr::interop;
using namespace System::Runtime::InteropServices;
using namespace System::Drawing::Imaging;
using namespace System::Runtime::InteropServices;

 
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
			#include "libavutil\opt.h"
			#include "libavutil\avstring.h"
			#include "libavutil\time.h"
        }
}

namespace iSpy { namespace Video { namespace FFMPEG
{
	#undef PixelFormat

    ref struct ReaderPrivateData
    {
        internal:
                libffmpeg::AVFormatContext*             FormatContext;
				libffmpeg::AVCodecContext*              AudioCodecContext;
                libffmpeg::AVCodecContext*              CodecContext;
                libffmpeg::SwsContext*					swsContext;
                libffmpeg::SwrContext*                  swrContext;
 
                libffmpeg::AVStream*                    VideoStream;
                libffmpeg::AVStream*                    AudioStream;
                bool                                    AudioNeedsConvert;
				int										RequestSeek;
				int64_t									LastPacket; 
                int										BytesRemaining;
				gcroot<ReaderPrivateData^>*				hdl;
				int										Timeout;
				bool									Abort;
				bool									Opened;
 
                ReaderPrivateData( )
                {
                        FormatContext     = NULL;
						AudioCodecContext = NULL;
						CodecContext      = NULL;
                        swsContext		  = NULL;
						swrContext		  = NULL;
 
						VideoStream       = NULL;
                        AudioStream       = NULL;
                        AudioNeedsConvert = false;
						RequestSeek		  = -1; 
                        BytesRemaining	  = 0;
						LastPacket        = 0;
						hdl				  = NULL;
						Timeout			  = 5000;
						Abort			  = false;
						Opened			  = false;
                }
        };

        VideoFileReader::VideoFileReader( void ) :
        data( nullptr ), disposed( false )
        {       
                /*libffmpeg::av_register_all( );
                libffmpeg::avcodec_register_all();
                libffmpeg::avformat_network_init();*/
				timeout = 5000;
				analyzeduration = 2000;
				cookies = "";
				useragent = "";
				headers = "";
				flags = -1;
				nobuffer = true;
        }


		static int interrupt_cb(void *ctx) 
		{ 
			gcroot<ReaderPrivateData^>* pointer = (gcroot<ReaderPrivateData^>*)(ctx);
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
		
	#pragma managed(push, off)

		static bool startsWith(const char *str,const char *pre)
		{
			size_t lenpre = strlen(pre),
					lenstr = strlen(str);
			return lenstr < lenpre ? false : strncmp(pre, str, lenpre) == 0;
		}
	

		static libffmpeg::AVFormatContext* open_file( char* fileName, int timeout, int analyzeduration, char* cookies, char* useragent, char* headers, int flags, int rtspmode, void* opaque)
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

				if(strlen(cookies) > 0)	{
					libffmpeg::av_dict_set(&options, "cookies", (const char*) cookies, 0);
				}
				
				if(strlen(headers) > 0)	{
					libffmpeg::av_dict_set(&options, "headers", (const char*) headers, 0);
				}
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

				if(strlen(useragent) > 0)	{
					libffmpeg::av_dict_set(&options, "user-agent", (const char*) useragent, 0);
				}
			}
			
			switch(rtspmode)	{
				case 1:
					libffmpeg::av_dict_set(&options,"rtsp_transport", "tcp",0);
					break;
				default:
				case 2:
					libffmpeg::av_dict_set(&options,"rtsp_transport", "udp",0);
					break;
				case 3:
					libffmpeg::av_dict_set(&options,"rtsp_transport", "udp_multicast",0);
					break;
				case 4:
					libffmpeg::av_dict_set(&options,"rtsp_transport", "http",0);
					break;
			}

			libffmpeg::AVFormatContext* formatContext = libffmpeg::avformat_alloc_context( );
			formatContext->interrupt_callback.callback = interrupt_cb;
			formatContext->interrupt_callback.opaque = opaque;
			
			int ret=0;
			ret = libffmpeg::avformat_open_input( &formatContext, fileName, NULL, &options );
			if ( ret !=0 )
			{
				//char* error = (char*)malloc(1024);
				//wchar_t error2[1024];
				//if (ret < 0)
				//{
				//   libffmpeg::av_strerror(ret, error, 1024);
				//  "an error with code -10024 was returned"... is not much use :(
				//}

				//return NULL;
				//libffmpeg::av_free(options);
				return nullptr;
			}

			if (flags>-1)
				formatContext->flags|=flags;
			else
				formatContext->flags|=AVFMT_FLAG_DISCARD_CORRUPT;

			return formatContext;

		}

		#pragma managed(pop)

		void VideoFileReader::Abort()	{
			if (data!=nullptr)
				data->Abort = true;
		}
        void VideoFileReader::Open(String^ fileName)
        {
                CheckIfDisposed( );
 
                data = gcnew ReaderPrivateData( );
				data->Timeout = timeout;
				data->Abort = false;

                bool success = false;
 
                IntPtr ptr = Marshal::StringToHGlobalAnsi( fileName );
                char* nativeFileName = reinterpret_cast<char*>( static_cast<void*>( ptr ) );

				IntPtr ptr2 = Marshal::StringToHGlobalAnsi( cookies );
                char* nativeCookies = reinterpret_cast<char*>( static_cast<void*>( ptr2 ) );

				IntPtr ptr3 = Marshal::StringToHGlobalAnsi( useragent );
                char* nativeuseragent = reinterpret_cast<char*>( static_cast<void*>( ptr3 ) );

				IntPtr ptr4 = Marshal::StringToHGlobalAnsi( headers );
                char* nativeheaders = reinterpret_cast<char*>( static_cast<void*>( ptr4 ) );

				
                try
                {
						data->LastPacket = GetTickCount();
						data->hdl = new gcroot<ReaderPrivateData^>(data);
						data->FormatContext = open_file( nativeFileName, timeout, analyzeduration, nativeCookies, nativeuseragent, nativeheaders, flags, rtspmode, (void*)data->hdl);
						
						if (data->FormatContext==nullptr)
							throw gcnew System::IO::IOException("Cannot open the stream. (" + fileName + ")");

						data->LastPacket = GetTickCount();

						//this is leaking when nobuffer is specified!
						int i = libffmpeg::avformat_find_stream_info( data->FormatContext, NULL );
                        if ( i < 0 )
                        {
                                throw gcnew Exception( "Cannot find stream information. ("+i+")" );
                        }

						if (nobuffer)
							data->FormatContext->flags|=AVFMT_FLAG_NOBUFFER;


						data->LastPacket = GetTickCount();

						for ( unsigned int i = 0; i < data->FormatContext->nb_streams; i++ )
						{
								if( data->FormatContext->streams[i]->codec->codec_type == libffmpeg::AVMEDIA_TYPE_VIDEO )
								{
										// get the pointer to the codec context for the video stream
										data->CodecContext = data->FormatContext->streams[i]->codec;
										data->CodecContext->workaround_bugs = 1;
										data->VideoStream  = data->FormatContext->streams[i];
										break;
								}
						}
						if (data->VideoStream == NULL )
						{
							throw gcnew Exception( "Cannot find the video stream." );
						}

						//this fixes issues with rtsp streams!! woot.
						data->CodecContext->flags2 |= CODEC_FLAG2_FAST | CODEC_FLAG2_CHUNKS | CODEC_FLAG_LOW_DELAY;  // Enable faster H264 decode.
						// Enable motion vector search (potentially slow), strong deblocking filter
						// for damaged macroblocks, and set our error detection sensitivity.
						
						libffmpeg::AVCodec* codec = libffmpeg::avcodec_find_decoder( data->CodecContext->codec_id );
						if ( codec == NULL )
						{
								throw gcnew Exception( "Cannot find a codec to decode the video stream." );
						}
						

						i = libffmpeg::avcodec_open2( data->CodecContext, codec, NULL );
						if ( i < 0 )
						{
								throw gcnew Exception( "Cannot open the video codec. ("+i+")" );
						}
						//throw gcnew VideoException( "Cannot find video stream in the specified file." );
						
						data->LastPacket = GetTickCount();
							
						m_width  = data->CodecContext->width;
						m_height = data->CodecContext->height;
						if (data->VideoStream->r_frame_rate.den==0)
								m_frameRate = 25;
						else
								m_frameRate = data->VideoStream->r_frame_rate.num / data->VideoStream->r_frame_rate.den;
 
						if (m_frameRate==0)
								m_frameRate = 25;
 
						m_codecName = gcnew String( data->CodecContext->codec->name );
						m_framesCount = data->VideoStream->nb_frames;
						m_duration = (data->VideoStream->duration*1000)*libffmpeg::av_q2d(data->VideoStream->time_base);

						
						for ( unsigned int i = 0; i < data->FormatContext->nb_streams; i++ )
						{
								if( data->FormatContext->streams[i]->codec->codec_type == libffmpeg::AVMEDIA_TYPE_AUDIO )
								{
										data->AudioCodecContext = data->FormatContext->streams[i]->codec;
										data->AudioStream  = data->FormatContext->streams[i];
										break;
								}
						}

						if ( data->AudioStream != NULL )
						{
							libffmpeg::AVCodec* audiocodec = libffmpeg::avcodec_find_decoder( data->AudioCodecContext->codec_id );
							if ( audiocodec != NULL )
							{
								data->AudioCodecContext->refcounted_frames = 0;

								if ( libffmpeg::avcodec_open2( data->AudioCodecContext, audiocodec, NULL ) == 0 )
								{
									data->AudioCodecContext->request_sample_fmt  = libffmpeg::AV_SAMPLE_FMT_S16;
 
									m_audiocodecName = gcnew String( data->AudioCodecContext->codec->name );
 
									m_sampleRate = data->AudioCodecContext->sample_rate;
									m_bitsPerSample = 16;
 
									int chans = 1;
									if (data->AudioCodecContext->channels>1) //downmix
											chans = 2;
 
									m_channels = chans;
 
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
								}
							}
						}
						
						//need to set hwcontext before we can start doing this - render to dxva
						/*libffmpeg::AVHWAccel *hwaccel = NULL;

						while ((hwaccel = libffmpeg::av_hwaccel_next(hwaccel))){
							if (hwaccel->id == data->CodecContext->codec->id)
							{
								if (hwaccel->pix_fmt == data->CodecContext->pix_fmt)
								{
									data->CodecContext->hwaccel = hwaccel;
									data->CodecContext->hwaccel_context = ?;
									break;
								}
							}
								

						}*/

						data->LastPacket = GetTickCount();
 
                        success = true;
						data->Opened = true;
                }
                finally
                {
                        Marshal::FreeHGlobal( ptr );
						Marshal::FreeHGlobal( ptr2 );
						Marshal::FreeHGlobal( ptr3 );
						Marshal::FreeHGlobal( ptr4 );
 
                        if ( !success )
                        {
                                Close( );
                        }
                }
        }
 
        void VideoFileReader::Close(  )
        {
            if ( data != nullptr && !disposed)
            { 
				//_RPT1( 0, "FFMPEG:Closing: %d", 0 );
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
							//_RPT1( 0, "%d", 1 );
							libffmpeg::avcodec_close(stream->codec);
							//_RPT1( 0, "%d", 2 );
						  }
						}
					}
					pin_ptr<libffmpeg::AVFormatContext*> pinprt = &(data->FormatContext);
                    libffmpeg::avformat_close_input( pinprt );
					//_RPT1( 0, "%d", 3 );
					data->FormatContext = NULL;
                }

				data->VideoStream       = NULL;
                data->AudioStream       = NULL;
				data->AudioCodecContext = NULL;
				data->CodecContext = NULL;

				if ( data->swrContext != NULL )
                {
						pin_ptr<libffmpeg::SwrContext*> pinprt = &(data->swrContext);
                        libffmpeg::swr_free(pinprt);
						//_RPT1( 0, "%d", 4 );
						data->swrContext = NULL;
                }

                if ( data->swsContext != NULL )
                {
                        libffmpeg::sws_freeContext( data->swsContext );
						//_RPT1( 0, "%d", 5 );
						data->swsContext = NULL;
                }

				if (data->hdl)	{
					delete data->hdl;
					//_RPT1( 0, "%d", 6 );
					data->hdl = NULL;
				}

				delete data;
				//_RPT1( 0, "%d\n", 7 );
				data=nullptr;
            }

        }
 
        void VideoFileReader::Seek(int timeInSeconds)   {
                if (data==nullptr)
                        return;
				data->RequestSeek = timeInSeconds;
                
        }

		template <typename T>
        inline void free_vector(std::vector<T>& to_clear)
        {
                std::vector<T> v;
                v.swap(to_clear);
        }
 

 
        Object^ VideoFileReader::ReadFrame(  )
        {
                CheckIfDisposed( );
 
                if ( data == nullptr || !data->Opened)
                {
                        throw gcnew System::IO::IOException( "Cannot read ffmpeg stream" );
                }
 
                libffmpeg::AVPacket packet = {0};
				libffmpeg::av_init_packet(&packet);

				libffmpeg::AVFrame* frame = libffmpeg::av_frame_alloc();
				libffmpeg::av_frame_unref(frame);

                do
                {
					if (data->RequestSeek>-1)	{
						int64_t timestamp = AV_TIME_BASE * static_cast<int64_t>(data->RequestSeek);
						data->RequestSeek = -1;

						if (timestamp < 0)
						{
								timestamp = 0;
						}
 
 
						int ret = libffmpeg::av_seek_frame(data->FormatContext, -1, timestamp, 0);
						if (ret >= 0)
						{
							if(data->VideoStream!=NULL)
								libffmpeg::avcodec_flush_buffers(data->CodecContext);
							if(data->AudioStream!=NULL)
								libffmpeg::avcodec_flush_buffers(data->AudioCodecContext);
						}
					}

                    if (libffmpeg::av_read_frame(data->FormatContext, &packet) < 0) {
						break;
                    }

					if (packet.flags & AV_PKT_FLAG_CORRUPT)
					{
						break;
					}

					// Make a shallow copy of packet so we can slide packet.data as frames are
					// decoded from the packet; otherwise av_free_packet() will corrupt memory.
					libffmpeg::AVPacket packet_temp = packet;

					////_RPT1( 0, "got stream %d\n", packet.stream_index );				
					data->LastPacket = GetTickCount();
                    if(data->AudioStream!=NULL && packet_temp.stream_index == data->AudioStream->index)
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

							m_audiotime = (frame->pkt_dts - data->AudioStream->start_time) *1000 * libffmpeg::av_q2d(data->AudioStream->time_base);
                            libffmpeg::av_free_packet(&packet);
                            libffmpeg::av_frame_free(&frame);
                            Array::Resize(managedBuf, s);
							m_lastFrameType = 1;
                            return managedBuf;
                    }
					
					if(data->VideoStream!=NULL && packet.stream_index == data->VideoStream->index)
					{
						int frameFinished = 0;
						//decode video frame

						int ret = libffmpeg::avcodec_decode_video2(data->CodecContext, frame, &frameFinished, &packet_temp);
						if (ret<0)
						{
							break;
						}

						if(frameFinished)
						{
							Bitmap^ bitmap = gcnew Bitmap( data->CodecContext->width, data->CodecContext->height, PixelFormat::Format24bppRgb );
 
							// lock the bitmap
										
							BitmapData^ bitmapData = bitmap->LockBits( System::Drawing::Rectangle( 0, 0, data->CodecContext->width, data->CodecContext->height ), ImageLockMode::ReadOnly, PixelFormat::Format24bppRgb );
 
							uint8_t* ptr = reinterpret_cast<uint8_t*>( static_cast<void*>( bitmapData->Scan0 ) );
 
							uint8_t* srcData[4] = { ptr, NULL, NULL, NULL };
							int srcLinesize[4] = { bitmapData->Stride, 0, 0, 0 };

							switch (data->CodecContext->pix_fmt)	{
							case libffmpeg::AV_PIX_FMT_YUVJ420P:
								data->CodecContext->pix_fmt = libffmpeg::AV_PIX_FMT_YUV420P;
								data->CodecContext->color_range = libffmpeg::AVCOL_RANGE_JPEG;
								break;
							case libffmpeg::AV_PIX_FMT_YUVJ422P:
								data->CodecContext->pix_fmt = libffmpeg::AV_PIX_FMT_YUV422P;
								data->CodecContext->color_range = libffmpeg::AVCOL_RANGE_JPEG;
								break;
							case libffmpeg::AV_PIX_FMT_YUVJ444P:
								data->CodecContext->pix_fmt = libffmpeg::AV_PIX_FMT_YUV444P;
								data->CodecContext->color_range = libffmpeg::AVCOL_RANGE_JPEG;
								break;
							case libffmpeg::AV_PIX_FMT_YUVJ440P:
								data->CodecContext->pix_fmt = libffmpeg::AV_PIX_FMT_YUV440P;
								data->CodecContext->color_range = libffmpeg::AVCOL_RANGE_JPEG;
								break;
							case libffmpeg::AV_PIX_FMT_YUVJ411P:
								data->CodecContext->pix_fmt = libffmpeg::AV_PIX_FMT_YUV411P;
								data->CodecContext->color_range = libffmpeg::AVCOL_RANGE_JPEG;
								break;

							}
							data->swsContext = libffmpeg::sws_getCachedContext(data->swsContext, data->CodecContext->width, data->CodecContext->height, data->CodecContext->pix_fmt,
								data->CodecContext->width, data->CodecContext->height, libffmpeg::AV_PIX_FMT_BGR24, SWS_FAST_BILINEAR, NULL, NULL, NULL);
 							
							libffmpeg::sws_scale( data->swsContext, frame->data, frame->linesize, 0, data->CodecContext->height, srcData, srcLinesize );
 
							bitmap->UnlockBits( bitmapData );
		
							m_time = (frame->pkt_dts - data->VideoStream->start_time) * 1000 * libffmpeg::av_q2d(data->VideoStream->time_base);
										
							if (frame->decode_error_flags>0)	{
								break;
							}
							libffmpeg::av_free_packet(&packet);
							libffmpeg::av_frame_free(&frame);
 
							if (data->VideoStream->avg_frame_rate.den>0)
									m_frameRate =  data->VideoStream->avg_frame_rate.num / data->VideoStream->avg_frame_rate.den;

							m_lastFrameType = 2;

							return bitmap;
						}
					}
						
					libffmpeg::av_free_packet(&packet);
                } while(!data->Abort);

				if (!data->Abort)	{
					//break has been called
					libffmpeg::av_free_packet(&packet);
				}
				libffmpeg::av_frame_free(&frame);
				m_lastFrameType = 0;
                return nullptr;
 
        }
 
 
} } }