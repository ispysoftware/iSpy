using System;
using System.Drawing;
using System.IO;
using System.Net;
using System.Threading;
using iSpyApplication.Utilities;

namespace iSpyApplication.Sources.Video
{
    public class JpegStream : VideoBase, IVideoSource, IDisposable
    {
        // buffer size used to download JPEG image
        private const int BufferSize = 1024*1024;
        // size of portion to read at once
        private const int ReadSize = 1024;
        private readonly int _frameInterval, _requestTimeout;

        /// <summary>
        ///     Login value.
        /// </summary>
        /// <remarks>Login required to access video source.</remarks>
        private readonly string _login, _password, _cookies, _httpUserAgent, _headers;

        private readonly bool _useHttp10;
        // received byte count
        private long _bytesReceived;

        private bool _disposed;
        // login and password for HTTP authentication
        // proxy information
        // received frames count
        private int _framesReceived;
        // URL for JPEG files
        private readonly objectsCamera _source;
        private Thread _thread;


        /// <summary>
        ///     Initializes a new instance of the <see cref="JpegStream" /> class.
        /// </summary>
        /// <param name="source">URL, which provides JPEG files.</param>
        public JpegStream(objectsCamera source) : base(source)
        {
            _source = source;
            var ckies = source.settings.cookies ?? "";
            ckies = ckies.Replace("[USERNAME]", source.settings.login);
            ckies = ckies.Replace("[PASSWORD]", source.settings.password);
            ckies = ckies.Replace("[CHANNEL]", source.settings.ptzchannel);

            var hdrs = source.settings.headers ?? "";
            hdrs = hdrs.Replace("[USERNAME]", source.settings.login);
            hdrs = hdrs.Replace("[PASSWORD]", source.settings.password);
            hdrs = hdrs.Replace("[CHANNEL]", source.settings.ptzchannel);

            _login = source.settings.login;
            _password = source.settings.password;
            _requestTimeout = source.settings.timeout;
            _useHttp10 = source.settings.usehttp10;
            _httpUserAgent = source.settings.useragent;
            _frameInterval = source.settings.frameinterval;
            _cookies = ckies;
            _headers = hdrs;
        }

        /// <summary>
        ///     Use or not separate connection group.
        /// </summary>
        /// <remarks>The property indicates to open web request in separate connection group.</remarks>
        public bool SeparateConnectionGroup { get; set; }

        /// <summary>
        ///     Gets or sets proxy information for the request.
        /// </summary>
        /// <remarks>
        ///     <para>
        ///         The local computer or application config file may specify that a default
        ///         proxy to be used. If the Proxy property is specified, then the proxy settings from the Proxy
        ///         property overridea the local computer or application config file and the instance will use
        ///         the proxy settings specified. If no proxy is specified in a config file
        ///         and the Proxy property is unspecified, the request uses the proxy settings
        ///         inherited from Internet Explorer on the local computer. If there are no proxy settings
        ///         in Internet Explorer, the request is sent directly to the server.
        ///     </para>
        /// </remarks>
        public IWebProxy Proxy { get; set; }

        // Public implementation of Dispose pattern callable by consumers. 
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        ///     New frame event.
        /// </summary>
        /// <remarks>
        ///     <para>Notifies clients about new available frame from video source.</para>
        ///     <para>
        ///         <note>
        ///             Since video source may have multiple clients, each client is responsible for
        ///             making a copy (cloning) of the passed video frame, because the video source disposes its
        ///             own original copy after notifying of clients.
        ///         </note>
        ///     </para>
        /// </remarks>
        public event NewFrameEventHandler NewFrame;

        /// <summary>
        ///     Video playing finished event.
        /// </summary>
        /// <remarks>
        ///     <para>This event is used to notify clients that the video playing has finished.</para>
        /// </remarks>
        public event PlayingFinishedEventHandler PlayingFinished;


        /// <summary>
        ///     Video source.
        /// </summary>
        /// <remarks>URL, which provides JPEG files.</remarks>
        public virtual string Source
        {
            get { return _source.settings.videosourcestring; }
            set { _source.settings.videosourcestring = value; }
        }

        /// <summary>
        ///     Received frames count.
        /// </summary>
        /// <remarks>
        ///     Number of frames the video source provided from the moment of the last
        ///     access to the property.
        /// </remarks>
        public int FramesReceived
        {
            get
            {
                var frames = _framesReceived;
                _framesReceived = 0;
                return frames;
            }
        }

        /// <summary>
        ///     Received bytes count.
        /// </summary>
        /// <remarks>
        ///     Number of bytes the video source provided from the moment of the last
        ///     access to the property.
        /// </remarks>
        public long BytesReceived
        {
            get
            {
                var bytes = _bytesReceived;
                _bytesReceived = 0;
                return bytes;
            }
        }

        /// <summary>
        ///     State of the video source.
        /// </summary>
        /// <remarks>Current state of video source object - running or not.</remarks>
        public bool IsRunning
        {
            get
            {
                if (_thread == null)
                    return false;

                try
                {
                    return !_thread.Join(TimeSpan.Zero);
                }
                catch
                {
                    return true;
                }
            }
        }

        /// <summary>
        ///     Start video source.
        /// </summary>
        /// <remarks>
        ///     Starts video source and return execution to caller. Video source
        ///     object creates background thread and notifies about new frames with the
        ///     help of <see cref="NewFrame" /> event.
        /// </remarks>
        /// <exception cref="ArgumentException">Video source is not specified.</exception>
        public void Start()
        {
            if (!IsRunning)
            {
                // check source
                if (string.IsNullOrEmpty(_source.settings.videosourcestring))
                    throw new ArgumentException("Video source is not specified.");

                _framesReceived = 0;
                _bytesReceived = 0;

                // create events
                _abort.Reset();
                _res = ReasonToFinishPlaying.DeviceLost;

                // create and start new thread
                _thread = new Thread(WorkerThread) {Name = _source.settings.videosourcestring, IsBackground = true};
                _thread.Start();
            }
        }

        private ReasonToFinishPlaying _res = ReasonToFinishPlaying.DeviceLost;
        private ManualResetEvent _abort = new ManualResetEvent(false);

        public void Restart()
        {
            if (!IsRunning) return;
            _res = ReasonToFinishPlaying.Restart;
            _abort.Set();
        }
        

        public void Stop()
        {
            if (IsRunning)
            {
                _res = ReasonToFinishPlaying.StoppedByUser;
                _abort.Set();
            }
            else
            {
                _res = ReasonToFinishPlaying.StoppedByUser;
                PlayingFinished?.Invoke(this, new PlayingFinishedEventArgs(_res));
            }
        }

        /// <summary>
        ///     Free resource.
        /// </summary>
        private void Free()
        {
            _thread = null;
        }

        // Worker thread
        private void WorkerThread()
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
            var rand = new Random((int) DateTime.UtcNow.Ticks);
            // download start time and duration
            var err = 0;
            ConnectionFactory connectionFactory = new ConnectionFactory();
            while (!_abort.WaitOne(0) && !MainForm.ShuttingDown)
            {
                var total = 0;

                try
                {
                    // set download start time
                    var start = DateTime.UtcNow;
                    var vss = Tokenise(_source.settings.videosourcestring);
                    var url = vss + (vss.IndexOf('?') == -1 ? '?' : '&') + "fake=" + rand.Next();

                    response = connectionFactory.GetResponse(url, _cookies, _headers, _httpUserAgent, _login, _password,
                        "GET", "", "", _useHttp10, out request);

                    // get response stream
                    try
                    {
                        stream = response.GetResponseStream();
                    }
                    catch (NullReferenceException)
                    {
                        throw new Exception("Connection failed");
                    }
                    stream.ReadTimeout = _requestTimeout;

                    // loop
                    while (!_abort.WaitOne(20))
                    {
                        // check total read
                        if (total > BufferSize - ReadSize)
                        {
                            total = 0;
                        }

                        // read next portion from stream
                        int read;
                        if ((read = stream.Read(buffer, total, ReadSize)) == 0)
                            break;

                        total += read;

                        // increment received bytes counter
                        _bytesReceived += read;
                    }

                    
                    // increment frames counter
                    _framesReceived++;

                    // provide new image to clients
                    if (NewFrame != null)
                    {
                        using (var bitmap = (Bitmap) Image.FromStream(new MemoryStream(buffer, 0, total)))
                        {
                            // notify client
                            NewFrame(this, new NewFrameEventArgs(bitmap));
                            // release the image
                        }
                    }


                    // wait for a while ?
                    if (_frameInterval > 0)
                    {
                        // get download duration
                        var span = DateTime.UtcNow.Subtract(start);
                        // milliseconds to sleep
                        var msec = _frameInterval - (int) span.TotalMilliseconds;

                        if ((msec > 0) || _abort.WaitOne(0))
                            break;
                    }
                    err = 0;
                }
                catch (ThreadAbortException)
                {
                    break;
                }
                catch (Exception ex)
                {
                    // provide information to clients
                    Logger.LogException(ex, "JPEG");
                    err++;
                    if (err > 3)
                    {
                        _res = ReasonToFinishPlaying.DeviceLost;
                        break;
                    }
                    //if ( VideoSourceError != null )
                    //{
                    //    VideoSourceError( this, new VideoSourceErrorEventArgs( exception.Message ) );
                    //}
                    // wait for a while before the next try
                    Thread.Sleep(250);
                }
                finally
                {
                    request?.Abort();
                    stream?.Flush();
                    stream?.Close();
                    response?.Close();
                }
            }

            PlayingFinished?.Invoke(this, new PlayingFinishedEventArgs(_res));
            Free();
        }

        // Protected implementation of Dispose pattern. 
        protected virtual void Dispose(bool disposing)
        {
            if (_disposed)
                return;

            // Free any unmanaged objects here. 
            //
            _disposed = true;
        }
    }
}