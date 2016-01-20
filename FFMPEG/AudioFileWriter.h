// iSpy FFMPEG Library
// http://www.ispyconnect.com
//
// Copyright © ispyconnect.com, 2013
#pragma once
using namespace System;
#include "AudioCodec.h"
namespace iSpy { namespace Video { namespace FFMPEG
{
	ref struct AudioWriterPrivateData;
    public ref class AudioFileWriter : IDisposable
	{
	public:
		property AudioCodec ACodec
		{
			AudioCodec get( )
			{
				CheckIfAudioFileIsOpen( );
				return m_audiocodec;
			}
		}
		property bool IsOpen
		{
			bool get ( )
			{
				return ( data != nullptr && !disposed);
			}
		}

    protected:
        !AudioFileWriter( )
        {
            this->Close( );
        }

	public:
		AudioFileWriter( void );
        ~AudioFileWriter( )
        {
			if (disposed)
				return;
            this->!AudioFileWriter( );
            disposed = true;
        }
		void Open( String^ fileName);
		void Open( String^ fileName, AudioCodec audioCodec, int bitrate, int samplerate, int channels );
		void WriteAudio(BYTE* soundBuffer, int soundBufferSize);
		void Close( );
		
	private:
		AudioCodec m_audiocodec;
	private:
		void CheckIfAudioFileIsOpen( )
		{
			if ( data == nullptr )
			{
				throw gcnew System::IO::IOException( "Audio file is not open." );
			}
		}
        void CheckIfDisposed( )
        {
            if ( disposed )
            {
                throw gcnew System::ObjectDisposedException( "The object was disposed." );
            }
        }
		
		void Flush( );

	private:
		AudioWriterPrivateData^ data;
        bool disposed;
	};

} } }
