using System;
using System.Drawing;
using System.IO;
using System.Net;
using System.Text;
using System.Threading;
using iSpyApplication.Controls;
using iSpyApplication.Sources.Audio;
using iSpyApplication.Utilities;
using NAudio.Wave;
using NAudio.Wave.SampleProviders;

namespace iSpyApplication.Sources.Video
{
    /// <summary>
    /// iSpyKinect provider stream for video and audio from an iSpyKinect network instance
    /// </summary>
    /// <remarks>
    /// Main Integration Points:
    /// KinectNetworkStream:
    /// CameraWindow - OpenVideoSource
    /// CameraWindow - Enable
    /// CameraWindow - Disable
    /// Camera - Plugin
    /// Camera - RunPlugin
    /// </remarks>
    internal class KinectNetworkStream : VideoBase, IVideoSource, IAudioSource, ISupportsAudio
    {
        private readonly objectsCamera _source;
        private string _login;
        private string _password;
        private IWebProxy _proxy;
        private bool _useSeparateConnectionGroup = true;
        private int _requestTimeout = 10000;
        private const int BufSize = 1024 * 1024*2;
        private const int ReadSize = 1024;
        private bool _usehttp10;
        private ManualResetEvent _abort;
        private ReasonToFinishPlaying _res = ReasonToFinishPlaying.DeviceLost;

        private Thread _thread;
        private string _userAgent = "Mozilla/5.0";

        #region Audio
        private float _gain;
        private bool _listening;

        public int BytePacket = 400;

        private BufferedWaveProvider _waveProvider;
        private SampleChannel _sampleChannel;

        public BufferedWaveProvider WaveOutProvider { get; set; }
        public IAudioSource OutAudio;
        #endregion

        #region Audio Stuff
        public event DataAvailableEventHandler DataAvailable;
        public event LevelChangedEventHandler LevelChanged;
        public event AudioFinishedEventHandler AudioFinished;
        public event HasAudioStreamEventHandler HasAudioStream;

        public float Gain
        {
            get { return _gain; }
            set
            {
                _gain = value;
                if (_sampleChannel != null)
                {
                    _sampleChannel.Volume = value;
                }
            }
        }

        public bool Listening
        {
            get
            {
                if (IsRunning && _listening)
                    return true;
                return false;

            }
            set
            {
                if (RecordingFormat == null)
                {
                    _listening = false;
                    return;
                }

                if (WaveOutProvider != null)
                {
                    if (WaveOutProvider.BufferedBytes>0) WaveOutProvider.ClearBuffer();
                    WaveOutProvider = null;
                }

                if (value)
                {
                    WaveOutProvider = new BufferedWaveProvider(RecordingFormat) { DiscardOnBufferOverflow = true, BufferDuration = TimeSpan.FromMilliseconds(500) };
                }

                _listening = value;
            }
        }

        public WaveFormat RecordingFormat { get; set; }

        #endregion

        #region Alerts Stuff
        // The alert handler lets us pass events from the kinect stream to the camera window for processing
        public event AlertEventHandler AlertHandler;
        #endregion

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

        /// <summary>
        /// Use or not separate connection group.
        /// </summary>
        /// 
        /// <remarks>The property indicates to open web request in separate connection group.</remarks>
        /// 
        public bool SeparateConnectionGroup
        {
            get { return _useSeparateConnectionGroup; }
            set { _useSeparateConnectionGroup = value; }
        }

        /// <summary>
        /// Use or not HTTP Protocol 1.0
        /// </summary>
        /// 
        /// <remarks>The property indicates to open web request using HTTP 1.0 protocol.</remarks>
        /// 
        public bool UseHTTP10
        {
            get { return _usehttp10; }
            set { _usehttp10 = value; }
        }


        /// <summary>
        /// Video source.
        /// </summary>
        /// 
        /// <remarks>URL, which provides iSpyKinect network stream.</remarks>
        /// 
        public string Source
        {
            get { return _source.settings.videosourcestring; }
            set
            {
                _source.settings.videosourcestring = value;
            }
        }

        /// <summary>
        /// Login value.
        /// </summary>
        /// 
        /// <remarks>Login required to access video source (unused).</remarks>
        /// 
        public string Login
        {
            get { return _login; }
            set { _login = value; }
        }

        /// <summary>
        /// Password value.
        /// </summary>
        /// 
        /// <remarks>Password required to access video source (unused).</remarks>
        /// 
        public string Password
        {
            get { return _password; }
            set { _password = value; }
        }

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
        public IWebProxy Proxy
        {
            get { return _proxy; }
            set { _proxy = value; }
        }

        /// <summary>
        /// User agent to specify in HTTP request header.
        /// </summary>
        /// 
        /// <remarks><para>Some IP cameras check what is the requesting user agent and depending
        /// on it they provide video in different formats or do not provide it at all. The property
        /// sets the value of user agent string, which is sent to camera in request header.
        /// </para>
        /// 
        /// <para>Default value is set to "Mozilla/5.0". If the value is set to <see langword="null"/>,
        /// the user agent string is not sent in request header.</para>
        /// </remarks>
        /// 
        public string HttpUserAgent
        {
            get { return _userAgent; }
            set { _userAgent = value; }
        }

        /// <summary>
        /// Request timeout value.
        /// </summary>
        /// 
        /// <remarks>The property sets timeout value in milliseconds for web requests.
        /// Default value is 10000 milliseconds.</remarks>
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
        /// Initializes a new instance of the <see cref="KinectNetworkStream"/> class.
        /// </summary>
        /// 
        /// <param name="source">URL, which provides MJPEG stream.</param>
        /// 
        public KinectNetworkStream(CameraWindow source): base(source)
        {
            _source = source.Camobject;
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
        public void Start()
        {
            if (!IsRunning)
            {
                // check source
                if (string.IsNullOrEmpty(_source.settings.videosourcestring))
                    throw new ArgumentException("Video source is not specified.");

                _res = ReasonToFinishPlaying.DeviceLost;

                // create and start new thread
                _thread = new Thread(WorkerThread) {Name = _source.settings.videosourcestring, IsBackground = true};
                _thread.Start();
            }
        }

        /// <summary>
        /// Signal video source to stop its work.
        /// </summary>
        /// 
        /// <remarks>Signals video source to stop its background thread, stop to
        /// provide new frames and free resources.</remarks>
        /// 
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
            _abort = new ManualResetEvent(false);
            // buffer to read stream
            var buffer = new byte[BufSize];
            var encoding = new ASCIIEncoding();
            while (!_abort.WaitOne(0) && !MainForm.ShuttingDown)
            {
                HttpWebRequest request = null;
                WebResponse response = null;
                Stream stream = null;

                try
                {
                    // create request
                    request = (HttpWebRequest)WebRequest.Create(_source.settings.videosourcestring);
                    // set user agent
                    if (_userAgent != null)
                    {
                        request.UserAgent = _userAgent;
                    }

                    // set proxy
                    if (_proxy != null)
                    {
                        request.Proxy = _proxy;
                    }

                    if (_usehttp10)
                        request.ProtocolVersion = HttpVersion.Version10;

                    // set timeout value for the request
                    request.Timeout = request.ServicePoint.ConnectionLeaseTimeout = request.ServicePoint.MaxIdleTime = _requestTimeout;
                    request.AllowAutoRedirect = true;

                    // set login and password
                    if ((_login != null) && (_password != null) && (_login != string.Empty))
                        request.Credentials = new NetworkCredential(_login, _password);
                    // set connection group name
                    if (_useSeparateConnectionGroup)
                        request.ConnectionGroupName = GetHashCode().ToString();
                    // get response
                    response = request.GetResponse();

                    // get response stream
                    stream = response.GetResponseStream();
                    stream.ReadTimeout = _requestTimeout;

                    byte[] boundary = encoding.GetBytes("--myboundary");
                    byte[] sep = encoding.GetBytes("\r\n\r\n");

                    // loop

                    int startPacket = -1;
                    int endPacket = -1;
                    int ttl = 0;

                    bool hasaudio = false;                   

                    while (!_abort.WaitOne(0) && !MainForm.ShuttingDown)
                    {

                        int read;
                        if ((read = stream.Read(buffer, ttl, ReadSize)) == 0)
                            throw new ApplicationException();
                        
                        ttl += read;

                        if (startPacket==-1)
                        {
                            startPacket = ByteArrayUtils.Find(buffer, boundary, 0, ttl);
                        }
                        else
                        {
                            if (endPacket == -1)
                            {
                                endPacket = ByteArrayUtils.Find(buffer, boundary, startPacket + boundary.Length, ttl-(startPacket + boundary.Length));
                            }    
                        }

                        var nf = NewFrame;
                        
                        
                        if (startPacket>-1 && endPacket>startPacket)
                        {
                            int br = ByteArrayUtils.Find(buffer, sep, startPacket, 100);

                            if (br != -1)
                            {
                                var arr = new byte[br];
                                Array.Copy(buffer, startPacket, arr, 0, br - startPacket);
                                string s = Encoding.ASCII.GetString(arr);
                                int k = s.IndexOf("Content-type: ", StringComparison.Ordinal);
                                if (k!=-1)
                                {
                                    s = s.Substring(k+14);
                                    s = s.Substring(0,s.IndexOf("\r\n", StringComparison.Ordinal));
                                    s = s.Trim();
                                }
                                switch (s)
                                {
                                    case "image/jpeg":
                                        try
                                        {
                                            if (ShouldEmitFrame)
                                            {
                                                using (var ms = new MemoryStream(buffer, br + 4, endPacket - br - 8))
                                                {
                                                    using (var bmp = (Bitmap) Image.FromStream(ms))
                                                    {
                                                        var dae = new NewFrameEventArgs(bmp);
                                                        nf.Invoke(this, dae);
                                                    }
                                                }
                                            }
                                        }
                                        catch (Exception ex)
                                        {
                                            //sometimes corrupted packets come through...
                                            Logger.LogException(ex,"KinectNetwork");
                                        }


                                        break;
                                    case "audio/raw":
                                        if (!hasaudio)
                                        {
                                            hasaudio = true;
                                            //fixed 16khz 1 channel format
                                            RecordingFormat = new WaveFormat(16000, 16, 1);

                                            _waveProvider = new BufferedWaveProvider(RecordingFormat) { DiscardOnBufferOverflow = true, BufferDuration = TimeSpan.FromMilliseconds(500) };

                                            _sampleChannel = new SampleChannel(_waveProvider);
                                            _sampleChannel.PreVolumeMeter +=SampleChannelPreVolumeMeter;
                                            if (HasAudioStream != null)
                                            {
                                                HasAudioStream(this, EventArgs.Empty);
                                                HasAudioStream = null;
                                            }
                                        }

                                        var da = DataAvailable;
                                        if (da != null)
                                        {
                                            int l = endPacket - br - 8;
                                            var data = new byte[l];
                                            int d;
                                            using (var ms = new MemoryStream(buffer, br+4, l))
                                            {
                                                d = ms.Read(data, 0, l);
                                            }
                                            if (d > 0)
                                            {
                                                _waveProvider.AddSamples(data, 0, data.Length);

                                                if (Listening)
                                                {
                                                    WaveOutProvider.AddSamples(data, 0, data.Length);
                                                }

                                                //forces processing of volume level without piping it out
                                                var sampleBuffer = new float[data.Length];
                                                int r = _sampleChannel.Read(sampleBuffer, 0, data.Length);

                                                da(this, new DataAvailableEventArgs((byte[]) data.Clone(),r));
                                            }
                                        }

                                        break;
                                    case "alert/text":
                                        // code to handle alert notifications goes here
                                        if (AlertHandler != null)
                                        {
                                            int dl = endPacket - br - 8;
                                            var data2 = new byte[dl];
                                            using (var ms = new MemoryStream(buffer, br + 4, dl))
                                            {
                                                ms.Read(data2, 0, dl);
                                            }
                                            string alerttype = Encoding.ASCII.GetString(data2);
                                            AlertHandler(this, new AlertEventArgs(alerttype));
                                        }
                                        break;
                                }
                            }

                            ttl -= endPacket;
                            Array.Copy(buffer, endPacket, buffer, 0, ttl);
                            startPacket = -1;
                            endPacket = -1;
                        }
                    }
                }
                catch (ApplicationException)
                {
                    // do nothing for Application Exception, which we raised on our own
                    // wait for a while before the next try
                    Thread.Sleep(250);
                }
                catch (ThreadAbortException)
                {
                    break;
                }
                catch (Exception ex)
                {
                    // provide information to clients
                    Logger.LogException(ex, "KinectNetwork");
                    _res = ReasonToFinishPlaying.DeviceLost;
                    _abort.WaitOne(250);
                    break;
                    // wait for a while before the next try
                    
                }
                finally
                {
                    request?.Abort();
                    stream?.Flush();
                    stream?.Close();
                    response?.Close();
                }
            }

            if (_waveProvider?.BufferedBytes > 0)
                _waveProvider.ClearBuffer();

            Listening = false;

            PlayingFinished?.Invoke(this, new PlayingFinishedEventArgs(_res));
            _abort.Close();
        }

        void SampleChannelPreVolumeMeter(object sender, StreamVolumeEventArgs e)
        {
            LevelChanged?.Invoke(this, new LevelChangedEventArgs(e.MaxSampleValues));
        }

        private bool _disposed;
        // Public implementation of Dispose pattern callable by consumers. 
        public void Dispose()
        {
            Dispose(true);
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
