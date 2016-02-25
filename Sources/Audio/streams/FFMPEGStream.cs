using System;
using System.Threading;
using System.Windows.Forms;
using iSpyApplication.Utilities;
using NAudio.Wave;
using NAudio.Wave.SampleProviders;
using AudioFileReader = iSpy.Video.FFMPEG.AudioFileReader;

namespace iSpyApplication.Sources.Audio.streams
{
    class FfmpegAudioStream: IAudioSource, IDisposable
    {
        private string _source;
        private bool _listening;
        private float _gain;
        private ManualResetEvent _stopEvent;
        private AudioFileReader _afr;
        private Int64 _lastFrame = DateTime.MinValue.Ticks;

        public DateTime LastFrame
        {
            get { return new DateTime(_lastFrame); }
            set { Interlocked.Exchange(ref _lastFrame, value.Ticks); }
        }

        private Thread _thread;

        private BufferedWaveProvider _waveProvider;
        private SampleChannel _sampleChannel;

        public BufferedWaveProvider WaveOutProvider { get; set; }

        /// <summary>
        /// New frame event.
        /// </summary>
        /// 
        /// <remarks><para>Notifies clients about new available frame from audio source.</para>
        /// 
        /// <para><note>Since audio source may have multiple clients, each client is responsible for
        /// making a copy (cloning) of the passed audio frame, because the audio source disposes its
        /// own original copy after notifying of clients.</note></para>
        /// </remarks>
        /// 
        public event DataAvailableEventHandler DataAvailable;

        /// <summary>
        /// New frame event.
        /// </summary>
        /// 
        /// <remarks><para>Notifies clients about new available frame from audio source.</para>
        /// 
        /// <para><note>Since audio source may have multiple clients, each client is responsible for
        /// making a copy (cloning) of the passed audio frame, because the audio source disposes its
        /// own original copy after notifying of clients.</note></para>
        /// </remarks>
        /// 
        public event LevelChangedEventHandler LevelChanged;

        /// <summary>
        /// audio source error event.
        /// </summary>
        /// 
        /// <remarks>This event is used to notify clients about any type of errors occurred in
        /// audio source object, for example internal exceptions.</remarks>
        /// 
        //public event AudioSourceErrorEventHandler AudioSourceError;

        /// <summary>
        /// audio playing finished event.
        /// </summary>
        /// 
        /// <remarks><para>This event is used to notify clients that the audio playing has finished.</para>
        /// </remarks>
        /// 
        public event AudioFinishedEventHandler AudioFinished;

        /// <summary>
        /// audio source.
        /// </summary>
        /// 
        /// <remarks>URL, which provides JPEG files.</remarks>
        /// 
        public virtual string Source
        {
            get { return _source; }
            set { _source = value; }
        }

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

        private bool IsFileSource => _source != null && _source.IndexOf("://", StringComparison.Ordinal) == -1;


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
        /// Initializes a new instance of the <see cref="LocalDeviceStream"/> class.
        /// </summary>
        /// 
        public FfmpegAudioStream() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="LocalDeviceStream"/> class.
        /// </summary>
        /// 
        /// <param name="source">source, which provides audio data.</param>
        /// 
        public FfmpegAudioStream(string source)
        {
            _source = source;
        }

        private readonly object _lock = new object();

        /// <summary>
        /// Start audio source.
        /// </summary>
        /// 
        /// <remarks>Starts audio source and return execution to caller. audio source
        /// object creates background thread and notifies about new frames with the
        /// help of <see cref="DataAvailable"/> event.</remarks>
        /// 
        /// <exception cref="ArgumentException">audio source is not specified.</exception>
        /// 
        public void Start()
        {
            
            if (string.IsNullOrEmpty(_source))
                throw new ArgumentException("Audio source is not specified.");

            if (IsRunning)
                return;

            lock (_lock)
            {

                _stopEvent = new ManualResetEvent(false);

                _thread = new Thread(FfmpegListener)
                          {
                              Name = "FFMPEG Audio Receiver (" + _source + ")"
                          };
                _thread.Start();
            }


        }

        public string Cookies = "";
        public int Timeout = 8000;
        public int AnalyseDuration = 2000;
        public string Headers = "";
        public string UserAgent = "";

        //public bool NoBuffer;
        ReasonToFinishPlaying _reasonToStop = ReasonToFinishPlaying.StoppedByUser;

        private void FfmpegListener()
        {
            _reasonToStop = ReasonToFinishPlaying.StoppedByUser;
            _afr = null;
            bool open = false;
            string errmsg = "";
            
            try
            {
                Program.FfmpegMutex.WaitOne();
                _afr = new AudioFileReader();
                int i = _source.IndexOf("://", StringComparison.Ordinal);
                if (i>-1)
                {
                    _source = _source.Substring(0, i).ToLower() + _source.Substring(i);
                }
                _afr.Timeout = Timeout;
                _afr.AnalyzeDuration = AnalyseDuration;
                _afr.Headers = Headers;
                _afr.Cookies = Cookies;
                _afr.UserAgent = UserAgent;
                _afr.Open(_source);
                open = true;
            }
            catch (Exception ex)
            {
                Logger.LogExceptionToFile(ex,"FFMPEG");
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

            if (_afr == null || !_afr.IsOpen || !open)
            {
                ShutDown("Could not open audio stream" + ": " + _source);
                return;
            }


            RecordingFormat = new WaveFormat(_afr.SampleRate, 16, _afr.Channels);
            _waveProvider = new BufferedWaveProvider(RecordingFormat) { DiscardOnBufferOverflow = true, BufferDuration = TimeSpan.FromMilliseconds(500) };
            
            _sampleChannel = new SampleChannel(_waveProvider);
            _sampleChannel.PreVolumeMeter += SampleChannelPreVolumeMeter;

            LastFrame = DateTime.UtcNow;

            try
            {
                while (!_stopEvent.WaitOne(10, false) && !MainForm.ShuttingDown)
                {
                    byte[] data = _afr.ReadAudioFrame();
                    if (data!=null && data.Length > 0)
                    {
                        LastFrame = DateTime.UtcNow;
                        var da = DataAvailable;
                        if (da != null)
                        {
                            //forces processing of volume level without piping it out
                            _waveProvider.AddSamples(data, 0, data.Length);

                            var sampleBuffer = new float[data.Length];
                            int read = _sampleChannel.Read(sampleBuffer, 0, data.Length);
                            
                            da(this, new DataAvailableEventArgs((byte[])data.Clone(),read));

                            if (Listening)
                            {
                                WaveOutProvider?.AddSamples(data, 0, read);
                            }
                            
                        }
                        
                        if (_stopEvent.WaitOne(30, false))
                            break;

                    }
                    else
                    {
                        if ((DateTime.UtcNow - LastFrame).TotalMilliseconds > Timeout)
                        {
                            throw new Exception("Audio source timeout");
                        }
                        if (_stopEvent.WaitOne(30, false))
                            break;
                    }
                    
                }
                
            }
            catch (Exception e)
            {
                Logger.LogExceptionToFile(e,"FFMPEG");
                errmsg = e.Message;
            }

            if (_sampleChannel != null)
            {
                _sampleChannel.PreVolumeMeter -= SampleChannelPreVolumeMeter;
                _sampleChannel = null;
            }

            if (_waveProvider?.BufferedBytes > 0)
                _waveProvider.ClearBuffer();

            if (WaveOutProvider?.BufferedBytes > 0) WaveOutProvider?.ClearBuffer();

            ShutDown(errmsg);
        }

        private void ShutDown(string errmsg)
        {
            bool err = !string.IsNullOrEmpty(errmsg);
            if (err)
            {
                
                _reasonToStop = ReasonToFinishPlaying.DeviceLost;
            }

            if (IsFileSource && !err)
                _reasonToStop = ReasonToFinishPlaying.EndOfStreamReached;

            if (_afr != null && _afr.IsOpen)
            {
                try
                {
                    _afr.Dispose(); //calls close
                }
                catch (Exception ex)
                {
                    Logger.LogExceptionToFile(ex, "FFMPEG");
                }
            }

            AudioFinished?.Invoke(this, new PlayingFinishedEventArgs(_reasonToStop));
        }


        void SampleChannelPreVolumeMeter(object sender, StreamVolumeEventArgs e)
        {
            LevelChanged?.Invoke(this, new LevelChangedEventArgs(e.MaxSampleValues));
        }

        /// <summary>
        /// Stop audio source.
        /// </summary>
        /// 
        /// <remarks><para>Stops audio source.</para>
        /// </remarks>
        /// 
        public void Stop()
        {
            if (!IsRunning) return;
            // wait for thread stop
            _stopEvent.Set();
            try
            {
                while (_thread != null && !_thread.Join(0))
                    Application.DoEvents();
            }
            catch
            {
                // ignored
            }

            _stopEvent?.Close();
            _stopEvent = null;
        }

        public WaveFormat RecordingFormat { get; set; }

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
                _afr?.Close();
            }

            // Free any unmanaged objects here. 
            //
            _disposed = true;
        }
    }
}
