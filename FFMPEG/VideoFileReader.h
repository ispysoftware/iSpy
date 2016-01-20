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
	ref struct ReaderPrivateData;

	public ref class VideoFileReader : IDisposable
	{
	public:
		property int SampleRate
		{
			int get( )
			{
				CheckIfVideoFileIsOpen( );
				return m_sampleRate;
			}
		}
		property int Channels
		{
			int get( )
			{
				CheckIfVideoFileIsOpen( );
				return m_channels;
			}
		}
		property int BitsPerSample
		{
			int get( )
			{
				CheckIfVideoFileIsOpen( );
				return m_bitsPerSample;
			}
		}
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
		property int FrameRate
		{
			int get( )
			{
				CheckIfVideoFileIsOpen( );
				return m_frameRate;
			}
		}

		property Int64 Duration {
			Int64 get(){
				CheckIfVideoFileIsOpen( );
				return m_duration;
			}
		}

		property Int64 VideoTime {
			Int64 get(){
				CheckIfVideoFileIsOpen( );
				return m_time;
			}
		}

		property Int64 AudioTime {
			Int64 get(){
				CheckIfVideoFileIsOpen( );
				return m_audiotime;
			}
		}
		property Int64 FrameCount
		{
			Int64 get( )
			{
				CheckIfVideoFileIsOpen( );
				return m_framesCount;
			}
		}

		property String^ CodecName
		{
			String^ get( )
			{
				CheckIfVideoFileIsOpen( );
				return m_codecName;
			}
		}

		property int LastFrameType
		{
			int get( )
			{
				return m_lastFrameType;
			}
		}

		property String^ AudioCodecName
		{
			String^ get( )
			{
				CheckIfVideoFileIsOpen( );
				return m_audiocodecName;
			}
		}

		property String^ Cookies
		{
			String^ get( )
			{
				return cookies;
			}
			void set(String^ value)	{
				cookies = value;
			}
		}

		property String^ UserAgent
		{
			String^ get( )
			{
				return useragent;
			}
			void set(String^ value)	{
				useragent = value;
			}
		}

		property String^ Headers
		{
			String^ get( )
			{
				return headers;
			}
			void set(String^ value)	{
				headers = value;
			}
		}
		property int RTSPMode
		{
			int get( )
			{
				return rtspmode;
			}
			void set(int value)	{
				rtspmode = value;
			}
		}

		property bool IsOpen
		{
			bool get ( )
			{
				return ( data != nullptr && !disposed);
			}
		}

		property bool NoBuffer {
			bool get ( )
			{
				return nobuffer;
			}
			void set(bool value)	{
				nobuffer = value;
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

		property int Flags {
			int get ( )
			{
				return flags;
			}
			void set(int value)	{
				flags = value;
			}
		}

    protected:
 
        !VideoFileReader( )
        {
            this->Close( );
        }

	public:

		VideoFileReader( void );

        ~VideoFileReader( )
        {
			if (disposed)
				return;
            this->!VideoFileReader( );
            disposed = true;
        }

		void Open( String^ fileName);//

		void Seek( int timeInSeconds );

		void Abort();

		Object^ ReadFrame( );

		void Close( );

	private:

		int m_width;
		int m_height;
		int	m_frameRate;
		int m_lastFrameType;
		int timeout;
		int analyzeduration;
		int flags;
		int rtspmode;
		bool nobuffer;
		
		String^ m_codecName;
		String^ m_audiocodecName;
		String^ cookies;
		String^ useragent;
		String^ headers;

		Int64 m_framesCount;
		Int64 m_duration;
		Int64 m_time;
		Int64 m_audiotime;

		int m_sampleRate, m_channels,m_bitsPerSample;

	private:
		void CheckIfVideoFileIsOpen( )
		{
			if ( data == nullptr )
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

		
		

	private:
		ReaderPrivateData^ data;
        bool disposed;
	};

} } }
