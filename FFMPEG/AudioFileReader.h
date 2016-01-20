// iSpy FFMPEG Library
// http://www.ispyconnect.com
//
// Copyright © ispyconnect.com, 2013
#pragma once

using namespace System;
using namespace System::Drawing;
using namespace System::Drawing::Imaging;

namespace iSpy { namespace Video { namespace FFMPEG
{
	ref struct ReaderPrivateData2;

	public ref class AudioFileReader : IDisposable
	{
	public:

		property int SampleRate
		{
			int get( )
			{
				CheckIfAudioFileIsOpen( );
				return m_sampleRate;
			}
		}

		property int Channels
		{
			int get( )
			{
				CheckIfAudioFileIsOpen( );
				return m_channels;
			}
		}

		property int BitsPerSample
		{
			int get( )
			{
				CheckIfAudioFileIsOpen( );
				return m_bitsPerSample;
			}
		}

		property String^ CodecName
		{
			String^ get( )
			{
				CheckIfAudioFileIsOpen( );
				return m_codecName;
			}
		}

		property String^ Cookies
		{
			String^ get()
			{
				return cookies;
			}
			void set(String^ value)	{
				cookies = value;
			}
		}

		property String^ UserAgent
		{
			String^ get()
			{
				return useragent;
			}
			void set(String^ value)	{
				useragent = value;
			}
		}

		property String^ Headers
		{
			String^ get()
			{
				return headers;
			}
			void set(String^ value)	{
				headers = value;
			}
		}

		property bool IsOpen
		{
			bool get ( )
			{
				return ( data != nullptr  && !disposed);
			}
		}

		property int Timeout {
			int get ( )
			{
				return timeout;
			}
			void set(int value)	{
				timeout = value;
			}
		}

		property int AnalyzeDuration {
			int get ( )
			{
				return analyzeduration;
			}
			void set(int value)	{
				analyzeduration = value;
			}
		}

    protected:

        !AudioFileReader( )
        {
            Close( );
        }

	public:

		AudioFileReader( void );

        ~AudioFileReader( )
        {
			if (disposed)
				return;

            this->!AudioFileReader( );
            disposed = true;
        }

		void Open( String^ fileName);

		void Abort();

		array<unsigned char>^ ReadAudioFrame( );

		void Close( );
	private:

		String^ m_codecName;
		int m_sampleRate, m_channels,m_bitsPerSample;
		int timeout;
		int analyzeduration;
		String^ cookies;
		String^ useragent;
		String^ headers;

	private:
		void CheckIfAudioFileIsOpen( )
		{
			if ( data == nullptr )
			{
				throw gcnew System::IO::IOException( "Video file not open." );
			}
		}

        void CheckIfDisposed( )
        {
            if ( disposed )
            {
                throw gcnew System::ObjectDisposedException( "The object was disposed." );
            }
        }

		
	private:
		ReaderPrivateData2^ data;
        bool disposed;
	};

} } }
