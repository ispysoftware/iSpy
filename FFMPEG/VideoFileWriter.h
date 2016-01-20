// iSpy FFMPEG Library
// http://www.ispyconnect.com
//
// Copyright © ispyconnect.com, 2013

#pragma once

using namespace System;
using namespace System::Drawing;
using namespace System::Drawing::Imaging;
using namespace System::Threading;

#include "VideoCodec.h"
#include "AudioCodec.h"

namespace iSpy { namespace Video { namespace FFMPEG
{
	ref struct WriterPrivateData;
    public ref class VideoFileWriter : IDisposable
	{
	public:
		property int Width
		{
			int get( )
			{
				CheckIfVideoFileIsOpen( );
				return m_width;
			}
		}
		property int Height
		{
			int get( )
			{
				CheckIfVideoFileIsOpen( );
				return m_height;
			}
		}
		property int BitRate
		{
			int get( )
			{
				CheckIfVideoFileIsOpen( );
				return m_bitRate;
			}
		}
		property VideoCodec Codec
		{
			VideoCodec get( )
			{
				CheckIfVideoFileIsOpen( );
				return m_codec;
			}
		}
		property AudioCodec ACodec
		{
			AudioCodec get( )
			{
				CheckIfVideoFileIsOpen( );
				return m_audiocodec;
			}
		}
		property bool IsOpen
		{
			bool get ( )
			{
				return ( data != nullptr && !disposed );
			}
		}


		

    protected:
        !VideoFileWriter( )
        {
            this->Close( );
        }

	public:
		
		VideoFileWriter( void );
        ~VideoFileWriter( )
        {
			if (disposed)
				return;

            this->!VideoFileWriter( );
            disposed = true;
        }
		bool Open( String^ fileName, int width, int height );
		bool Open( String^ fileName, int width, int height, VideoCodec codec );
		bool Open( String^ fileName, int width, int height, VideoCodec codec, int bitRate, int frameRate);
		bool Open( String^ fileName, int width, int height, VideoCodec codec, int bitRate, AudioCodec audioCodec, int frameRate, int bitrate, int samplerate, int channels );
		void WriteVideoFrame( Bitmap^ frame, int64_t pts);
		void WriteAudio(BYTE* soundBuffer, int soundBufferSize, int64_t pts);
		void AddAudioSamples( WriterPrivateData^ data,  BYTE* soundBuffer, int soundBufferSize, int64_t pts);

		void Close( );

	private:

		int m_width;
		int m_height;
		int m_bitRate;
		int m_frameNumber;
		VideoCodec m_codec;
		AudioCodec m_audiocodec;

	private:
		
		void CheckIfVideoFileIsOpen( )
		{
			if ( !data )
			{
				throw gcnew System::IO::IOException( "Video file is not open, so can not access its properties." );
			}
		}
        void CheckIfDisposed( )
        {
            if ( disposed )
            {
                throw gcnew System::ObjectDisposedException( "The object was already disposed." );
            }
        }
		
		
		void Flush( );

	private:
		WriterPrivateData^ data;
        bool disposed;
	};

} } }
