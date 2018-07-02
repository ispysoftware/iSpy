using System;
using System.Drawing;
using System.IO;
using System.Net;
using System.Text;
using System.Threading;
using iSpyApplication.Controls;
using iSpyApplication.Utilities;

namespace iSpyApplication.Sources.Video
{
    /// <summary>
    ///     MJPEG video source.
    /// </summary>
    /// <remarks>
    ///     <para>
    ///         The video source downloads JPEG images from the specified URL, which represents
    ///         MJPEG stream.
    ///     </para>
    ///     <para>Sample usage:</para>
    ///     <code>
    /// // create MJPEG video source
    /// MJPEGStream stream = new MJPEGStream2( "some url" );
    /// // set event handlers
    /// stream.NewFrame += new NewFrameEventHandler( video_NewFrame );
    /// // start the video source
    /// stream.Start( );
    /// // ...
    /// </code>
    ///     <para>
    ///         <note>
    ///             Some cameras produce HTTP header, which does not conform strictly to
    ///             standard, what leads to .NET exception. To avoid this exception the <b>useUnsafeHeaderParsing</b>
    ///             configuration option of <b>httpWebRequest</b> should be set, what may be done using application
    ///             configuration file.
    ///         </note>
    ///     </para>
    ///     <code>
    /// &lt;configuration&gt;
    /// 	&lt;system.net&gt;
    /// 		&lt;settings&gt;
    /// 			&lt;httpWebRequest useUnsafeHeaderParsing="true" /&gt;
    /// 		&lt;/settings&gt;
    /// 	&lt;/system.net&gt;
    /// &lt;/configuration&gt;
    /// </code>
    /// </remarks>
    internal class MJPEGStream : VideoBase, IVideoSource
    {
        // if we should use basic authentication when connecting to the video source

        // buffer size used to download MJPEG stream
        private const int BufSize = 1024*1024;
        // size of portion to read at once
        private const int ReadSize = 1024;
        private ManualResetEvent _abort;
        private readonly string _cookies;

        private readonly string _decodeKey;

        private readonly string _headers;
        private readonly string _httpUserAgent;

        private readonly string _login;
        private readonly string _password;
        // use separate HTTP connection group or use default
        // timeout value for web request
        private readonly int _requestTimeout;
        // URL for MJPEG stream
        private readonly objectsCamera _source;
        private readonly bool _useHttp10;
        // received byte count
        private long _bytesReceived;

        private bool _disposed;

        // login and password for HTTP authentication
        // proxy information
        // received frames count
        private int _framesReceived;
        private ReasonToFinishPlaying _res = ReasonToFinishPlaying.DeviceLost;

        private Thread _thread;


        /// <summary>
        ///     Initializes a new instance of the <see cref="MJPEGStream" /> class.
        /// </summary>
        /// <param name="source">URL, which provides MJPEG stream.</param>
        public MJPEGStream(CameraWindow source) : base(source)
        {
            _source = source.Camobject;

            var ckies = _source.settings.cookies ?? "";
            ckies = ckies.Replace("[USERNAME]", _source.settings.login);
            ckies = ckies.Replace("[PASSWORD]", _source.settings.password);
            ckies = ckies.Replace("[CHANNEL]", _source.settings.ptzchannel);

            var hdrs = _source.settings.headers ?? "";
            hdrs = hdrs.Replace("[USERNAME]", _source.settings.login);
            hdrs = hdrs.Replace("[PASSWORD]", _source.settings.password);
            hdrs = hdrs.Replace("[CHANNEL]", _source.settings.ptzchannel);


            _login = _source.settings.login;
            _password = _source.settings.password;
            _requestTimeout = _source.settings.timeout;
            _httpUserAgent = _source.settings.useragent;
            _decodeKey = _source.decodekey;
            _useHttp10 = _source.settings.usehttp10;
            _cookies = ckies;
            _headers = hdrs;
        }

        /// <summary>
        ///     Gets or sets proxy information for the request.
        /// </summary>
        /// <remarks>
        ///     <para>
        ///         The local computer or application config file may specify that a default
        ///         proxy to be used. If the Proxy property is specified, then the proxy settings from the Proxy
        ///         property override the local computer or application config file and the instance will use
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
        /// <remarks>URL, which provides MJPEG stream.</remarks>
        public string Source
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
                _res = ReasonToFinishPlaying.DeviceLost;

                // create and start new thread
                _thread = new Thread(WorkerThread) {Name = _source.settings.videosourcestring, IsBackground = true};
                _thread.Start();
            }
        }

        public void Stop()
        {
            if (IsRunning)
            {
                _res = ReasonToFinishPlaying.StoppedByUser;
                _abort?.Set();
            }
            else
            {
                _res = ReasonToFinishPlaying.StoppedByUser;
                PlayingFinished?.Invoke(this, new PlayingFinishedEventArgs(_res));
            }
        }

        public void Restart()
        {
            if (!IsRunning)
                return;
            _res = ReasonToFinishPlaying.Restart;
            _abort?.Set();
        }


        // Worker thread
        private void WorkerThread()
        {
            // buffer to read stream
            var buffer = new byte[BufSize];
            // JPEG magic number
            var jpegMagic = new byte[] {0xFF, 0xD8, 0xFF};
            _abort = new ManualResetEvent(false);

            var encoding = new ASCIIEncoding();

            while (!_abort.WaitOne(0) && !MainForm.ShuttingDown)
            {
                // HTTP web request
                HttpWebRequest request = null;
                // web response
                WebResponse response = null;
                // stream for MJPEG downloading
                Stream stream = null;
                // boundary between images (string and binary versions)
                string boudaryStr = null;
                // length of boundary
                // flag signaling if boundary was checked or not
                var boundaryIsChecked = false;
                // read amounts and positions
                int todo = 0, total = 0, pos = 0, align = 1;
                var start = 0;

                var connectionFactory = new ConnectionFactory();
                // align
                //  1 = searching for image start
                //  2 = searching for image end

                try
                {
                    // create request
                    // get response
                    var vss = Tokenise(_source.settings.videosourcestring);

                    response = connectionFactory.GetResponse(vss, _cookies, _headers, _httpUserAgent, _login, _password,
                        "GET", "", "", _useHttp10, out request);
                    if (response == null)
                        throw new Exception("Connection failed");
                    // check content type
                    var contentType = response.ContentType;
                    var contentTypeArray = contentType.Split('/');

                    // "application/octet-stream"
                    int boundaryLen;
                    byte[] boundary;
                    if ((contentTypeArray[0] == "application") && (contentTypeArray[1] == "octet-stream"))
                    {
                        boundaryLen = 0;
                        boundary = new byte[0];
                    }
                    else if ((contentTypeArray[0] == "multipart") && contentType.Contains("mixed"))
                    {
                        // get boundary
                        var boundaryIndex = contentType.IndexOf("boundary", 0, StringComparison.Ordinal);
                        if (boundaryIndex != -1)
                        {
                            boundaryIndex = contentType.IndexOf("=", boundaryIndex + 8, StringComparison.Ordinal);
                        }

                        if (boundaryIndex == -1)
                        {
                            // try same scenario as with octet-stream, i.e. without boundaries
                            boundaryLen = 0;
                            boundary = new byte[0];
                        }
                        else
                        {
                            boudaryStr = contentType.Substring(boundaryIndex + 1);
                            // remove spaces and double quotes, which may be added by some IP cameras
                            boudaryStr = boudaryStr.Trim(' ', '"');

                            boundary = encoding.GetBytes(boudaryStr);
                            boundaryLen = boundary.Length;
                            boundaryIsChecked = false;
                        }
                    }
                    else
                    {
                        throw new Exception("Invalid content type.");
                    }

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
                    while (!_abort.WaitOne(0) && !MainForm.ShuttingDown)
                    {
                        // check total read
                        if (total > BufSize - ReadSize)
                        {
                            total = pos = todo = 0;
                        }

                        // read next portion from stream
                        int read;
                        if ((read = stream.Read(buffer, total, ReadSize)) == 0)
                            throw new ApplicationException();

                        total += read;
                        todo += read;

                        // increment received bytes counter
                        _bytesReceived += read;

                        // do we need to check boundary ?
                        if ((boundaryLen != 0) && !boundaryIsChecked)
                        {
                            // some IP cameras, like AirLink, claim that boundary is "myboundary",
                            // when it is really "--myboundary". this needs to be corrected.

                            pos = ByteArrayUtils.Find(buffer, boundary, 0, todo);
                            // continue reading if boudary was not found
                            if (pos == -1)
                                continue;

                            for (var i = pos - 1; i >= 0; i--)
                            {
                                var ch = buffer[i];

                                if ((ch == (byte) '\n') || (ch == (byte) '\r'))
                                {
                                    break;
                                }

                                boudaryStr = (char) ch + boudaryStr;
                            }

                            boundary = encoding.GetBytes(boudaryStr);
                            boundaryLen = boundary.Length;
                            boundaryIsChecked = true;
                        }

                        // search for image start
                        if ((align == 1) && (todo >= jpegMagic.Length))
                        {
                            start = ByteArrayUtils.Find(buffer, jpegMagic, pos, todo);
                            if (start != -1)
                            {
                                // found JPEG start
                                pos = start + jpegMagic.Length;
                                todo = total - pos;
                                align = 2;
                            }
                            else
                            {
                                // delimiter not found
                                todo = jpegMagic.Length - 1;
                                pos = total - todo;
                            }
                        }

                        var decode = !string.IsNullOrEmpty(_decodeKey);

                        // search for image end ( boundaryLen can be 0, so need extra check )
                        while ((align == 2) && (todo != 0) && (todo >= boundaryLen))
                        {
                            var stop = ByteArrayUtils.Find(buffer,
                                boundaryLen != 0 ? boundary : jpegMagic,
                                pos, todo);

                            if (stop != -1)
                            {
                                // increment frames counter
                                _framesReceived++;
                                var nf = NewFrame;
                                // image at stop
                                if (nf != null)
                                {
                                    if (ShouldEmitFrame)
                                    {
                                        if (decode)
                                        {
                                            var marker = Encoding.ASCII.GetBytes(_decodeKey);

                                            using (
                                                var ms = new MemoryStream(buffer, start + jpegMagic.Length,
                                                    jpegMagic.Length + marker.Length))
                                            {
                                                var key = new byte[marker.Length];
                                                ms.Read(key, 0, marker.Length);

                                                if (!ByteArrayUtils.UnsafeCompare(marker, key))
                                                {
                                                    throw new Exception(
                                                        "Image Decode Failed - Check the decode key matches the encode key on ispy server");
                                                }
                                            }


                                            using (
                                                var ms = new MemoryStream(buffer, start + marker.Length,
                                                    stop - start - marker.Length))
                                            {
                                                ms.Seek(0, SeekOrigin.Begin);
                                                ms.WriteByte(jpegMagic[0]);
                                                ms.WriteByte(jpegMagic[1]);
                                                ms.WriteByte(jpegMagic[2]);
                                                ms.Seek(0, SeekOrigin.Begin);

                                                using (var bmp = (Bitmap) Image.FromStream(ms))
                                                {
                                                    var da = new NewFrameEventArgs(bmp);
                                                    nf.Invoke(this, da);
                                                }
                                            }
                                        }
                                        else
                                        {
                                            using (var ms = new MemoryStream(buffer, start, stop - start))
                                            {
                                                using (var bmp = (Bitmap) Image.FromStream(ms))
                                                {
                                                    var da = new NewFrameEventArgs(bmp);
                                                    nf.Invoke(this, da);
                                                }
                                            }
                                        }
                                    }
                                }

                                // shift array
                                pos = stop + boundaryLen;
                                todo = total - pos;
                                Array.Copy(buffer, pos, buffer, 0, todo);

                                total = todo;
                                pos = 0;
                                align = 1;
                            }
                            else
                            {
                                // boundary not found
                                if (boundaryLen != 0)
                                {
                                    todo = boundaryLen - 1;
                                    pos = total - todo;
                                }
                                else
                                {
                                    todo = 0;
                                    pos = total;
                                }
                            }
                        }
                    }
                }
                catch (ApplicationException)
                {
                    // do nothing for Application Exception, which we raised on our own
                    // wait for a while before the next try
                    _abort.WaitOne(250);
                }
                catch (ThreadAbortException)
                {
                    break;
                }
                catch (Exception ex)
                {
                    // provide information to clients
                    Logger.LogException(ex, "MJPEG");
                    _res = ReasonToFinishPlaying.DeviceLost;
                    break;
                    // wait for a while before the next try
                    //Thread.Sleep(250);
                }
                finally
                {
                    // abort request
                    request?.Abort();
                    stream?.Flush();
                    stream?.Close();
                    response?.Close();
                }
            }

            PlayingFinished?.Invoke(this, new PlayingFinishedEventArgs(_res));
            _abort.Close();
        }

        // Protected implementation of Dispose pattern. 
        protected virtual void Dispose(bool disposing)
        {
            if (_disposed)
                return;

            if (disposing)
            {
                
            }

            // Free any unmanaged objects here. 
            //
            _disposed = true;
        }
    }
}