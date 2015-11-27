using System;
using System.Collections.Concurrent;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;
using iSpy.Video.FFMPEG;
using iSpyApplication.Sources.Audio;
using iSpyApplication.Utilities;
using NAudio.Wave;
using NAudio.Wave.SampleProviders;

namespace iSpyApplication.Sources.Video
{
    public class FfmpegStream : IVideoSource, IAudioSource, ISupportsAudio, IDisposable
    {
        private int _framesReceived;
        private ManualResetEvent _stopEvent;
        private Thread _thread;
        private string _source;
        
        #region Audio
        private float _gain;
        private bool _listening;
        private volatile bool _stopping;

        private BufferedWaveProvider _waveProvider;
        public SampleChannel SampleChannel;

        public BufferedWaveProvider WaveOutProvider { get; set; }
        public VolumeWaveProvider16New VolumeProvider { get; set; }
        #endregion

        private Int64 _lastFrame = DateTime.MinValue.Ticks;

        public DateTime LastFrame
        {
            get { return new DateTime(_lastFrame); }
            set { Interlocked.Exchange(ref _lastFrame, value.Ticks); }
        }
        

        /// <summary>
        /// Initializes a new instance of the <see cref="FfmpegStream"/> class.
        /// </summary>
        /// 
        public FfmpegStream()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FfmpegStream"/> class.
        /// </summary>
        /// 
        /// <param name="source">URL, which provides video stream.</param>
        public FfmpegStream(string source)
        {
            _source = source;
        }

        public IAudioSource OutAudio;

        #region IVideoSource Members

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
        public event PlayingFinishedEventHandler PlayingFinished;

        public event DataAvailableEventHandler DataAvailable;
        public event LevelChangedEventHandler LevelChanged;
        public event AudioFinishedEventHandler AudioFinished;

        public event HasAudioStreamEventHandler HasAudioStream;

        /// <summary>
        /// Video source.
        /// </summary>
        /// 
        /// <remarks>URL, which provides video stream.</remarks>
        /// 
        public string Source
        {
            get { return _source; }
            set { _source = value; }
        }

        /// <summary>
        /// Received bytes count.
        /// </summary>
        /// 
        /// <remarks>Number of bytes the video source provided from the moment of the last
        /// access to the property.
        /// </remarks>
        /// 
        public long BytesReceived => 0;

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
        /// State of the video source.
        /// </summary>
        /// 
        /// <remarks>Current state of video source object - running or not.</remarks>
        /// 
        public bool IsRunning => _thread != null && !_thread.Join(TimeSpan.Zero);

        private readonly object _lock = new object();
        private Thread _eventing;

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
            if (string.IsNullOrEmpty(_source))
                 throw new ArgumentException("Video source is not specified.");

            if (IsRunning) return;

            lock (_lock)
            {
                _stopping = false;
                _framesReceived = 0;

                // create events
                _stopEvent = new ManualResetEvent(false);

                // create and start new thread
                _thread = new Thread(FfmpegListener) {Name = "ffmpeg " + _source, IsBackground = true};
                _thread.Start();
            }
        }

        public double PlaybackRate = 1;

        private VideoFileReader _vfr;

        public string Cookies = "";
        public string UserAgent = "";
        public string Headers = "";
        public int RTSPMode = 0;

        public int AnalyzeDuration = 2000;
        public int Timeout = 8000;

        ReasonToFinishPlaying _reasonToStop = ReasonToFinishPlaying.StoppedByUser;

        private void FfmpegListener()
        {
            _reasonToStop = ReasonToFinishPlaying.StoppedByUser;
            _vfr = null;
            bool open = false;
            string errmsg = "";
            _eventing = null;
            _stopping = false;
            try
            {
                Program.FfmpegMutex.WaitOne();
                _vfr = new VideoFileReader();

                //ensure http/https is lower case for string compare in ffmpeg library
                int i = _source.IndexOf("://", StringComparison.Ordinal);
                if (i > -1)
                {
                    _source = _source.Substring(0, i).ToLower() + _source.Substring(i);
                }
                _vfr.Timeout = Timeout;
                _vfr.AnalyzeDuration = AnalyzeDuration;
                _vfr.Cookies = Cookies;
                _vfr.UserAgent = UserAgent;
                _vfr.Headers = Headers;
                _vfr.Flags = -1;
                _vfr.NoBuffer = true;
                _vfr.RTSPMode = RTSPMode;
                _vfr.Open(_source);
                open = true;
            }
            catch (Exception ex)
            {
                Logger.LogExceptionToFile(ex, "FFMPEG");
            }
            finally
            {
                try
                {
                    Program.FfmpegMutex.ReleaseMutex();
                }
                catch (ObjectDisposedException)
                {
                    //can happen on shutdown
                }
            }

            if (_vfr == null || !_vfr.IsOpen || !open)
            {
                ShutDown("Could not open stream" + ": " + _source);
                return;
            }

            bool hasaudio = false;
            

            if (_vfr.Channels > 0)
            {
                hasaudio = true;
                RecordingFormat = new WaveFormat(_vfr.SampleRate, 16, _vfr.Channels);
                _waveProvider = new BufferedWaveProvider(RecordingFormat) { DiscardOnBufferOverflow = true, BufferDuration = TimeSpan.FromMilliseconds(500) };
                SampleChannel = new SampleChannel(_waveProvider);
                SampleChannel.PreVolumeMeter += SampleChannelPreVolumeMeter;   
            }
            

            Duration = _vfr.Duration;

            _videoQueue = new ConcurrentQueue<Bitmap>();
            _audioQueue = new ConcurrentQueue<byte[]>();
            _eventing = new Thread(EventManager) { Name = "ffmpeg eventing", IsBackground = true };
            _eventing.Start();

            try
            {
                while (!_stopEvent.WaitOne(5) && !MainForm.ShuttingDown)
                {
                    var nf = NewFrame;
                    if (nf == null)
                        break;

                    object frame = _vfr.ReadFrame();
                    switch (_vfr.LastFrameType)
                    {
                        case 0:
                            //null packet
                            if ((DateTime.UtcNow - LastFrame).TotalMilliseconds > Timeout)
                                throw new TimeoutException("Timeout reading from video stream");
                            break;
                        case 1:
                            LastFrame = DateTime.UtcNow;
                            if (hasaudio)
                            {
                                var data = frame as byte[];
                                if (data?.Length > 0)
                                {
                                    ProcessAudio(data);
                                }
                            }
                            break;
                        case 2:
                            LastFrame = DateTime.UtcNow;

                            var bmp = frame as Bitmap;
                            if (bmp != null)
                            {
                                if (_videoQueue.Count<20)
                                    _videoQueue.Enqueue(bmp);
                            }
                            break;
                    }
                }
                
            }
            catch (Exception e)
            {
                Logger.LogExceptionToFile(e, "FFMPEG");
                errmsg = e.Message;
            }

            _stopEvent.Set();
            _eventing.Join();

            if (SampleChannel != null)
            {
                SampleChannel.PreVolumeMeter -= SampleChannelPreVolumeMeter;
                SampleChannel = null;
            }

            if (_waveProvider?.BufferedBytes > 0)
                _waveProvider?.ClearBuffer();

            ShutDown(errmsg);
        }

        private void ShutDown(string errmsg)
        {

            bool err=!string.IsNullOrEmpty(errmsg);
            if (err)
            {
                _reasonToStop = ReasonToFinishPlaying.DeviceLost;
            }

            
                try
                {
                    if (_vfr != null && _vfr.IsOpen)
                    {
                        _vfr?.Dispose(); //calls close
                    }
                }
                catch (Exception ex)
                {
                    Logger.LogExceptionToFile(ex, "FFMPEG");
                }

            PlayingFinished?.Invoke(this, new PlayingFinishedEventArgs(_reasonToStop));
            AudioFinished?.Invoke(this, new PlayingFinishedEventArgs(_reasonToStop));


            _stopEvent.Close();
            _stopEvent = null;
            _stopping = false;
        }

        void ProcessAudio(byte[] data)
        {
            if (HasAudioStream != null)
            {
                HasAudioStream?.Invoke(this, EventArgs.Empty);
                HasAudioStream = null;
            }
            try
            {
                if (DataAvailable != null)
                {
                    _audioQueue.Enqueue(data);
                }
            }
            catch (NullReferenceException)
            {
                //DataAvailable can be removed at any time
            }
            catch (Exception ex)
            {
                Logger.LogExceptionToFile(ex, "FFMPEG");
            }
        }

        void SampleChannelPreVolumeMeter(object sender, StreamVolumeEventArgs e)
        {
            LevelChanged?.Invoke(this, new LevelChangedEventArgs(e.MaxSampleValues));
        }

        public bool Seekable;

        public long Time
        {
            get
            {
                if (_vfr.IsOpen)
                    return _vfr.VideoTime;
                return 0;
            }
        }
        public long Duration;

        #region Audio Stuff
        public float Gain
        {
            get { return _gain; }
            set
            {
                _gain = value;
                if (SampleChannel != null)
                {
                    SampleChannel.Volume = value;
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
                    if (WaveOutProvider?.BufferedBytes>0) WaveOutProvider?.ClearBuffer();
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

        /// <summary>
        /// Calls Stop
        /// </summary>
        public void SignalToStop()
        {
            Stop();
        }

        /// <summary>
        /// Calls Stop
        /// </summary>
        public void WaitForStop()
        {
            Stop();
        }

        /// <summary>
        /// Stop video source.
        /// </summary>
        /// 
        public void Stop()
        {
            if (!IsRunning || _stopping) return;
            // wait for thread stop
            _stopping = true;
            try
            {
                _stopEvent?.Set();
                while(_thread!=null && !_thread.Join(0))
                    Application.DoEvents();
            }
            catch (Exception ex)
            {
                Logger.LogExceptionToFile(ex,"FFMPEG");
            }
        }
        #endregion


        private ConcurrentQueue<Bitmap> _videoQueue;
        private ConcurrentQueue<byte[]> _audioQueue;

        private void EventManager()
        {
            Bitmap frame;

            while (!_stopEvent.WaitOne(5, false) && !MainForm.ShuttingDown)
            {
                try
                {
                    if (_videoQueue.TryDequeue(out frame))
                    {
                        if (frame != null)
                        {
                            NewFrame?.Invoke(this, new NewFrameEventArgs(frame));
                            frame.Dispose();
                        }
                    }


                    byte[] audio;
                    if (!_audioQueue.TryDequeue(out audio)) continue;

                    var da = DataAvailable;
                    da?.Invoke(this, new DataAvailableEventArgs(audio));

                    var sampleBuffer = new float[audio.Length];
                    int read = SampleChannel.Read(sampleBuffer, 0, audio.Length);

                    _waveProvider?.AddSamples(audio, 0, read);

                    if (WaveOutProvider != null && Listening)
                    {
                        WaveOutProvider?.AddSamples(audio, 0, read);
                    }
                }
                catch (Exception ex)
                {
                    Logger.LogExceptionToFile(ex, "FFMPEG");
                }
            }
            try
            {
                while (_videoQueue != null && _videoQueue.TryDequeue(out frame))
                {
                    frame?.Dispose();
                }
            }
            catch (Exception ex)
            {
                Logger.LogExceptionToFile(ex, "FFMPEG");
            }
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
                try
                {
                    _stopEvent?.Close();
                    _vfr?.Close();
                }
                catch (Exception ex)
                {
                    Logger.LogExceptionToFile(ex, "FFMPEG");
                }
            }

            // Free any unmanaged objects here. 
            //
            _disposed = true;
        }

    }
}