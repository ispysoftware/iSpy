using System;
using System.Drawing;
using System.IO;
using System.Net;
using System.Threading;
using iSpyApplication.Utilities;

namespace iSpyApplication.Sources.Video
{
	public class JpegStream: IVideoSource, IDisposable
	{
        // URL for JPEG files
		private string _source;
        // login and password for HTTP authentication
	    // proxy information
	    // received frames count
		private int _framesReceived;
        // received byte count
		private long _bytesReceived;
        // use separate HTTP connection group or use default
	    // prevent cashing or not
	    // frame interval in milliseconds
		private int _frameInterval;
        // timeout value for web request
        private int _requestTimeout = 10000;
        // if we should use basic authentication when connecting to the video source
	    public string Headers = "";

        // buffer size used to download JPEG image
		private const int BufferSize = 1024 * 1024;
        // size of portion to read at once
        private const int ReadSize = 1024;

	    private Thread _thread;
		private ManualResetEvent _stopEvent;

        /// <summary>
        /// New frame event.
        /// </summary>
        /// 
        /// <remarks><para>Notifies clients about new available frame from video source.</para>
        /// 
        /// <para><note>Since video source may have multiple clients, each client is responsible for
        /// making a copy (cloning) of the passed video frame, because the video source disposes its
        /// own original copy after notifying of clients.</note></para>
        /// </remarks>
        /// 
        public event NewFrameEventHandler NewFrame;

        /// <summary>
        /// Video playing finished event.
        /// </summary>
        /// 
        /// <remarks><para>This event is used to notify clients that the video playing has finished.</para>
        /// </remarks>
        /// 
        public event PlayingFinishedEventHandler PlayingFinished;

        public string Cookies { get; set; } = "";

	    /// <summary>
        /// Use or not HTTP Protocol 1.0
        /// </summary>
        /// 
        /// <remarks>The property indicates to open web request using HTTP 1.0 protocol.</remarks>
        /// 
        public bool UseHttp10 { get; set; }

	    public string HttpUserAgent { get; set; } = "Mozilla/5.0";

	    /// <summary>
        /// Use or not separate connection group.
        /// </summary>
        /// 
        /// <remarks>The property indicates to open web request in separate connection group.</remarks>
        /// 
		public bool SeparateConnectionGroup { get; set; }

	    /// <summary>
        /// Use or not caching.
        /// </summary>
        /// 
        /// <remarks>If the property is set to <b>true</b>, then a fake random parameter will be added
        /// to URL to prevent caching. It's required for clients, who are behind proxy server.</remarks>
        /// 
		public bool PreventCaching { get; set; } = true;

	    /// <summary>
        /// Frame interval.
        /// </summary>
        /// 
        /// <remarks>The property sets the interval in milliseconds betwen frames. If the property is
        /// set to 100, then the desired frame rate will be 10 frames per second. Default value is 0 -
        /// get new frames as fast as possible.</remarks>
        /// 
		public int FrameInterval
		{
			get { return _frameInterval; }
			set { _frameInterval = value; }
		}

        /// <summary>
        /// Video source.
        /// </summary>
        /// 
        /// <remarks>URL, which provides JPEG files.</remarks>
        /// 
        public virtual string Source
		{
			get { return _source; }
			set { _source = value; }
		}

        /// <summary>
        /// Login value.
        /// </summary>
        /// 
        /// <remarks>Login required to access video source.</remarks>
        /// 
		public string Login { get; set; }

	    /// <summary>
        /// Password value.
        /// </summary>
        /// 
        /// <remarks>Password required to access video source.</remarks>
        /// 
        public string Password { get; set; }

	    /// <summary>
        /// Gets or sets proxy information for the request.
        /// </summary>
        /// 
        /// <remarks><para>The local computer or application config file may specify that a default
        /// proxy to be used. If the Proxy property is specified, then the proxy settings from the Proxy
        /// property overridea the local computer or application config file and the instance will use
        /// the proxy settings specified. If no proxy is specified in a config file
        /// and the Proxy property is unspecified, the request uses the proxy settings
        /// inherited from Internet Explorer on the local computer. If there are no proxy settings
        /// in Internet Explorer, the request is sent directly to the server.
        /// </para></remarks>
        /// 
        public IWebProxy Proxy { get; set; }

	    /// <summary>
        /// Received frames count.
        /// </summary>
        /// 
        /// <remarks>Number of frames the video source provided from the moment of the last
        /// access to the property.
        /// </remarks>
        /// 
        public int FramesReceived
		{
			get
			{
				int frames = _framesReceived;
				_framesReceived = 0;
				return frames;
			}
		}

        /// <summary>
        /// Received bytes count.
        /// </summary>
        /// 
        /// <remarks>Number of bytes the video source provided from the moment of the last
        /// access to the property.
        /// </remarks>
        /// 
        public long BytesReceived
		{
			get
			{
				long bytes = _bytesReceived;
				_bytesReceived = 0;
				return bytes;
			}
		}

        /// <summary>
        /// Request timeout value.
        /// </summary>
        /// 
        /// <remarks><para>The property sets timeout value in milliseconds for web requests.</para>
        /// 
        /// <para>Default value is set <b>10000</b> milliseconds.</para></remarks>
        /// 
        public int RequestTimeout
        {
            get { return _requestTimeout; }
            set { _requestTimeout = value; }
        }

        /// <summary>
        /// State of the video source.
        /// </summary>
        /// 
        /// <remarks>Current state of video source object - running or not.</remarks>
        /// 
        public bool IsRunning => _thread != null && !_thread.Join(TimeSpan.Zero);
		
        /// <summary>
        /// Force using of basic authentication when connecting to the video source.
        /// </summary>
        /// 
        /// <remarks><para>For some IP cameras (TrendNET IP cameras, for example) using standard .NET's authentication via credentials
        /// does not seem to be working (seems like camera does not request for authentication, but expects corresponding headers to be
        /// present on connection request). So this property allows to force basic authentication by adding required HTTP headers when
        /// request is sent.</para>
        /// 
        /// <para>Default value is set to <see langword="false"/>.</para>
        /// </remarks>
        /// 
        public bool ForceBasicAuthentication { get; set; }

	    /// <summary>
        /// Initializes a new instance of the <see cref="JpegStream"/> class.
        /// </summary>
        /// 
        public JpegStream( ) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="JpegStream"/> class.
        /// </summary>
        /// 
        /// <param name="source">URL, which provides JPEG files.</param>
        /// 
        public JpegStream( string source )
        {
            _source = source;
        }

        /// <summary>
        /// Start video source.
        /// </summary>
        /// 
        /// <remarks>Starts video source and return execution to caller. Video source
        /// object creates background thread and notifies about new frames with the
        /// help of <see cref="NewFrame"/> event.</remarks>
        /// 
        /// <exception cref="ArgumentException">Video source is not specified.</exception>
        /// 
        public void Start( )
		{
			if ( !IsRunning )
			{
                // check source
                if ( string.IsNullOrEmpty(_source) )
                    throw new ArgumentException( "Video source is not specified." );

				_framesReceived = 0;
				_bytesReceived = 0;

				// create events
				_stopEvent = new ManualResetEvent( false );

                // create and start new thread
				_thread = new Thread( WorkerThread ) {Name = _source, IsBackground = true};
			    _thread.Start( );
			}
		}

        /// <summary>
        /// Signal video source to stop its work.
        /// </summary>
        /// 
        /// <remarks>Signals video source to stop its background thread, stop to
        /// provide new frames and free resources.</remarks>
        /// 
        public void SignalToStop( )
		{
			// stop thread
			if ( _thread != null )
			{
				// signal to stop
				_stopEvent.Set( );
			}
		}

        /// <summary>
        /// Wait for video source has stopped.
        /// </summary>
        /// 
        /// <remarks>Waits for source stopping after it was signalled to stop using
        /// <see cref="SignalToStop"/> method.</remarks>
        /// 
        public void WaitForStop( )
		{
            if (IsRunning)
            {
				// wait for thread stop
                _stopEvent.Set();
                _thread.Join(MainForm.ThreadKillDelay);
                if (_thread != null && !_thread.Join(TimeSpan.Zero))
                    _thread.Abort();
				Free( );
			}
		}

        /// <summary>
        /// Stop video source.
        /// </summary>
        /// 
        /// <remarks><para>Stops video source aborting its thread.</para>
        /// 
        /// <para><note>Since the method aborts background thread, its usage is highly not preferred
        /// and should be done only if there are no other options. The correct way of stopping camera
        /// is <see cref="SignalToStop">signaling it stop</see> and then
        /// <see cref="WaitForStop">waiting</see> for background thread's completion.</note></para>
        /// </remarks>
        /// 
        public void Stop( )
		{
			if ( IsRunning )
			{
				WaitForStop( );
			}
		}

        /// <summary>
        /// Free resource.
        /// </summary>
        /// 
		private void Free( )
		{
			_thread = null;

			// release events
			_stopEvent.Close( );
			_stopEvent = null;
		}

        // Worker thread
        private void WorkerThread( )
		{
            // buffer to read stream
			var buffer = new byte[BufferSize];
            // HTTP web request
			HttpWebRequest request = null;
            // web responce
			WebResponse response = null;
            // stream for JPEG downloading
			Stream stream = null;
            // random generator to add fake parameter for cache preventing
			var rand = new Random( (int) DateTime.UtcNow.Ticks );
            // download start time and duration
            var res = ReasonToFinishPlaying.StoppedByUser;
            int err = 0;

            while (!_stopEvent.WaitOne(0, false) && !MainForm.ShuttingDown)
			{
			    int	total = 0;

			    try
				{
                    // set download start time
					DateTime start = DateTime.UtcNow;

                    response = ConnectionFactory.GetResponse(_source + ((_source.IndexOf('?') == -1) ? '?' : '&') + "fake=" + rand.Next(), Cookies, Headers, HttpUserAgent, Proxy, UseHttp10, SeparateConnectionGroup, RequestTimeout, Login, Password,false, out request);

					// get response stream
                    stream = response.GetResponseStream( );
                    stream.ReadTimeout = _requestTimeout;

					// loop
					while ( !_stopEvent.WaitOne( 0, false ) )
					{
						// check total read
						if ( total > BufferSize - ReadSize )
						{
							total = 0;
						}

						// read next portion from stream
					    int	read;
					    if ( ( read = stream.Read( buffer, total, ReadSize ) ) == 0 )
							break;

						total += read;

						// increment received bytes counter
						_bytesReceived += read;
					}

					if ( !_stopEvent.WaitOne( 0, false ) )
					{
						// increment frames counter
						_framesReceived++;

						// provide new image to clients
						if ( NewFrame != null )
						{
						    using (var bitmap = (Bitmap) Image.FromStream(new MemoryStream(buffer, 0, total)))
						    {
						        // notify client
						        NewFrame(this, new NewFrameEventArgs(bitmap));
						        // release the image
						    }
						}
					}

					// wait for a while ?
					if ( _frameInterval > 0 )
					{
						// get download duration
						var span = DateTime.UtcNow.Subtract( start );
						// milliseconds to sleep
						int msec = _frameInterval - (int) span.TotalMilliseconds;

                        if ((msec > 0) && (_stopEvent.WaitOne(msec, false)))
                            break;
					}
				    err = 0;
				}
                catch ( ThreadAbortException )
                {
                    break;
                }
                catch ( Exception ex )
				{
                    // provide information to clients
                    MainForm.LogExceptionToFile(ex,"JPEG");
				    err++;
                    if (err>3)
                    {
                        res = ReasonToFinishPlaying.DeviceLost;
                        break;
                    }
                    //if ( VideoSourceError != null )
                    //{
                    //    VideoSourceError( this, new VideoSourceErrorEventArgs( exception.Message ) );
                    //}
                    // wait for a while before the next try
                    Thread.Sleep( 250 );
                }
				finally
				{
                    request?.Abort();
                    stream?.Flush();
                    stream?.Close();
                    response?.Close();
                }

				// need to stop ?
				if ( _stopEvent.WaitOne( 0, false ) )
					break;
			}

            PlayingFinished?.Invoke( this, new PlayingFinishedEventArgs(res ));
		}

        private bool _disposed;
        // Public implementation of Dispose pattern callable by consumers. 
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        // Protected implementation of Dispose pattern. 
        protected virtual void Dispose(bool disposing)
        {
            if (_disposed)
                return;

            if (disposing)
            {
                _stopEvent?.Close();
            }

            // Free any unmanaged objects here. 
            //
            _disposed = true;
        }
    }
}
