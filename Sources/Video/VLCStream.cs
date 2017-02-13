using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;
using Declarations;
using Declarations.Events;
using Declarations.Media;
using Declarations.Players;
using iSpyApplication.Sources.Audio;
using iSpyApplication.Utilities;
using Implementation;
using NAudio.Wave;
using NAudio.Wave.SampleProviders;
using File = System.IO.File;

namespace iSpyApplication.Sources.Video
{
    public class VlcStream : IVideoSource, IAudioSource, ISupportsAudio, IDisposable
    {
        public int FormatWidth = 320, FormatHeight = 240;
        private readonly string[] _arguments;
        private int _framesReceived;

        private volatile bool _stopRequested, _stopping;

        IMediaPlayerFactory _mFactory;
        IMedia _mMedia;
        IVideoPlayer _mPlayer;

        private Thread _thread;
        private ManualResetEvent _stopEvent;

        #region Audio
        private float _gain;
        private bool _listening;

        private bool _needsSetup = true;

        public int BytePacket = 400;

        private WaveFormat _recordingFormat;
        private BufferedWaveProvider _waveProvider;
        private SampleChannel _sampleChannel;

        public BufferedWaveProvider WaveOutProvider { get; set; }

        #endregion

        private long _lastFrame = DateTime.MinValue.Ticks;

        public DateTime LastFrame
        {
            get { return new DateTime(_lastFrame); }
            set { Interlocked.Exchange(ref _lastFrame, value.Ticks); }
        }

        public int TimeOut = 8000;

        // URL for VLCstream
        private string _source;

        /// <summary>
        /// Initializes a new instance of the <see cref="VlcStream"/> class.
        /// </summary>
        /// 
        public VlcStream()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="VlcStream"/> class.
        /// </summary>
        /// 
        /// <param name="source">URL, which provides VLCstream.</param>
        /// <param name="arguments"></param>
        public VlcStream(string source, string[] arguments)
        {
            _source = source;
            _arguments = arguments;
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

        /// <summary>
        /// Video playing finished event.
        /// </summary>
        /// 
        /// <remarks><para>This event is used to notify clients that the video playing has finished.</para>
        /// </remarks>
        /// 
        public event PlayingFinishedEventHandler PlayingFinished;

        /// <summary>
        /// Video source.
        /// </summary>
        /// 
        /// <remarks>URL, which provides VLCstream.</remarks>
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
        public long BytesReceived
        {
            get
            {
                const long bytes = 0;
                //bytesReceived = 0;
                return bytes;
            }
        }

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
            if (!VlcHelper.VlcInstalled)
                return;

            if (string.IsNullOrEmpty(_source))
                    throw new ArgumentException("Video source is not specified.");

            if (IsRunning) return;

            lock (_lock)
            {
                _stopRequested = false;               

                // create events
                _stopEvent = new ManualResetEvent(false);

                // create and start new thread
                
                _thread = new Thread(WorkerThread) {Name = _source, IsBackground = true};
                _thread.SetApartmentState(ApartmentState.MTA);
                _thread.Start();
            }
        }

        

        private Thread _eventing;
        private readonly object _lock = new object();

        private void WorkerThread()
        {
            bool file = false;
            try
            {
                if (File.Exists(_source))
                {
                    file = true;
                }
            }
            catch
            {
                // ignored
            }

            if (_mFactory == null)
            {
                var args = new List<string>
                    {
                        "-I", 
                        "dumy",  
		                "--ignore-config", 
                        "--no-osd",
                        "--disable-screensaver",
		                "--plugin-path=./plugins"
                    };
                if (file)
                    args.Add("--file-caching=3000");
                try
                {
                    var l2 = args.ToList();
                    l2.AddRange(_arguments);

                    l2 = l2.Distinct().ToList();
                    _mFactory = new MediaPlayerFactory(l2.ToArray());
                }
                catch (Exception ex)
                {
                    Logger.LogExceptionToFile(ex,"VLC Stream");
                    Logger.LogMessageToFile("VLC arguments are: " + string.Join(",", args.ToArray()), "VLC Stream");
                    Logger.LogMessageToFile("Using default VLC configuration.", "VLC Stream");
                    _mFactory = new MediaPlayerFactory(args.ToArray());
                }
                GC.KeepAlive(_mFactory);
            }

            _mMedia = file ? _mFactory.CreateMedia<IMediaFromFile>(_source) : _mFactory.CreateMedia<IMedia>(_source);

            _mMedia.Events.DurationChanged += EventsDurationChanged;
            _mMedia.Events.StateChanged += EventsStateChanged;

            if (_mPlayer != null)
            {
                try
                {
                    _mPlayer?.Dispose();
                }
                catch
                {
                    // ignored
                }
                _mPlayer = null;
            }


            _mPlayer = _mFactory.CreatePlayer<IVideoPlayer>();
            _mPlayer.Events.TimeChanged += EventsTimeChanged;

            var fc = new Func<SoundFormat, SoundFormat>(SoundFormatCallback);
            _mPlayer.CustomAudioRenderer.SetFormatCallback(fc);
            var ac = new AudioCallbacks { SoundCallback = SoundCallback };

            _mPlayer.CustomAudioRenderer.SetCallbacks(ac);
            _mPlayer.CustomAudioRenderer.SetExceptionHandler(Handler);


            _mPlayer.CustomRenderer.SetCallback(FrameCallback);
            _mPlayer.CustomRenderer.SetExceptionHandler(Handler);
            GC.KeepAlive(_mPlayer);

            _needsSetup = true;
            _stopping = false;
            _mPlayer.CustomRenderer.SetFormat(new BitmapFormat(FormatWidth, FormatHeight, ChromaType.RV24));
            _mPlayer.Open(_mMedia);
            _mMedia.Parse(true);

            _mPlayer.Delay = 0;

            _framesReceived = 0;
            Duration = Time = 0;
            LastFrame = DateTime.MinValue;


            //check if file source (isseekable in _mPlayer is not reliable)
            Seekable = false;
            try
            {
                var p = Path.GetFullPath(_mMedia.Input);
                Seekable = !string.IsNullOrEmpty(p);
            }
            catch (Exception)
            {
                Seekable = false;
            }
            _mPlayer.WindowHandle = IntPtr.Zero;

            _videoQueue = new ConcurrentQueue<Bitmap>();
            _audioQueue = new ConcurrentQueue<byte[]>();
            _eventing = new Thread(EventManager) { Name = "vlc eventing", IsBackground = true };
            _eventing.Start();
            
            _mPlayer.Play();

            _stopEvent.WaitOne();

            if (_eventing!=null && !_eventing.Join(0))
                _eventing.Join();

            if (!Seekable && !_stopRequested)
            {
                PlayingFinished?.Invoke(this, new PlayingFinishedEventArgs(ReasonToFinishPlaying.DeviceLost));
                AudioFinished?.Invoke(this, new PlayingFinishedEventArgs(ReasonToFinishPlaying.DeviceLost));
            }
            else
            {
                PlayingFinished?.Invoke(this, new PlayingFinishedEventArgs(ReasonToFinishPlaying.StoppedByUser));
                AudioFinished?.Invoke(this, new PlayingFinishedEventArgs(ReasonToFinishPlaying.StoppedByUser));
            }

            DisposePlayer();

            _stopEvent?.Close();
            _stopEvent = null;
        }

        void DisposePlayer()
        {
            try
            {
                if (_sampleChannel != null)
                {
                    _sampleChannel.PreVolumeMeter -= SampleChannelPreVolumeMeter;
                    _sampleChannel = null;
                }

                _mMedia.Events.DurationChanged -= EventsDurationChanged;
                _mMedia.Events.StateChanged -= EventsStateChanged;

                _mPlayer.Stop();

                _mMedia.Dispose();
                _mMedia = null;

                if (_waveProvider?.BufferedBytes > 0)
                {
                    try
                    {
                        _waveProvider?.ClearBuffer();
                    }
                    catch
                    {
                        // ignored
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.LogExceptionToFile(ex,"VLC");
            }
            _waveProvider = null;

            Listening = false;
        }

        static void Handler(Exception ex)
        {
            Logger.LogExceptionToFile(ex, "VLC Stream");

        }

        void EventsStateChanged(object sender, MediaStateChange e)
        {
            switch (e.NewState)
            {
                case MediaState.Ended:
                case MediaState.Stopped:
                case MediaState.Error:
                    if (_stopEvent != null && !_stopping)
                    {
                        _stopping = true;
                        try
                        {
                            _stopEvent?.Set();
                        }
                        catch
                        {
                            // ignored
                        }
                    }
                    break;
            }

        }

        public void CheckTimestamp()
        {
            //some feeds keep returning frames even when the connection is lost
            //this detects that by comparing timestamps from the eventstimechanged event
            //and signals an error if more than 8 seconds ago
            if (LastFrame > DateTime.MinValue && (Helper.Now - LastFrame).TotalMilliseconds > TimeOut)
            {
                Stop(false);
            }
        }

        public void Stop()
        {
            Stop(true);
        }

        /// <summary>
        /// Stop video source.
        /// </summary>
        /// 
        public void Stop(bool requested)
        {
            if (!IsRunning || _stopping) return;
            // wait for thread stop
            _stopping = true;
            _stopRequested = requested;

            try
            {
                _stopEvent?.Set();
            }
            catch
            {
                // ignored
            }

            while (_thread != null && !_thread.Join(0))
                Application.DoEvents();
        }


        public bool Seekable;

        public long Time, Duration;

        void EventsDurationChanged(object sender, MediaDurationChange e)
        {
            Duration = e.NewDuration;
        }


        void EventsTimeChanged(object sender, MediaPlayerTimeChanged e)
        {
            Time = e.NewTime;
            LastFrame = Helper.Now;
        }

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
                return IsRunning && _listening;
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
                    if (WaveOutProvider.BufferedBytes > 0) WaveOutProvider.ClearBuffer();
                    WaveOutProvider = null;
                }


                if (value)
                {
                    WaveOutProvider = new BufferedWaveProvider(RecordingFormat) { DiscardOnBufferOverflow = true, BufferDuration = TimeSpan.FromMilliseconds(500) };
                }

                _listening = value;
            }
        }

        public WaveFormat RecordingFormat
        {
            get { return _recordingFormat; }
            set
            {
                _recordingFormat = value;
            }
        }

        private int _realChannels;
        private SoundFormat SoundFormatCallback(SoundFormat sf)
        {
            if (!_needsSetup) return sf;
            _needsSetup = false;

            int chan = _realChannels = sf.Channels;
            if (chan > 1)
                chan = 2;//downmix
            _recordingFormat = new WaveFormat(sf.Rate, 16, chan);
            _waveProvider = new BufferedWaveProvider(RecordingFormat);
            _sampleChannel = new SampleChannel(_waveProvider);
            _sampleChannel.PreVolumeMeter += SampleChannelPreVolumeMeter;

            if (HasAudioStream == null) return sf;
            HasAudioStream?.Invoke(this, EventArgs.Empty);
            HasAudioStream = null;

            return sf;
        }

        void SampleChannelPreVolumeMeter(object sender, StreamVolumeEventArgs e)
        {
            var lc = LevelChanged;
            lc?.Invoke(this, new LevelChangedEventArgs(e.MaxSampleValues));
        }

        private void SoundCallback(Sound soundData)
        {
            var da = DataAvailable;
            if (da == null || _needsSetup) return;

            try
            {
                var l = (int)soundData.SamplesSize;
                var data = new byte[l];
                Marshal.Copy(soundData.SamplesData, data, 0, l);

                if (_realChannels > 2)
                {
                    //resample audio to 2 channels
                    data = ToStereo(data, _realChannels);
                }

                _audioQueue.Enqueue(data);
                
            }
            catch (NullReferenceException)
            {
                //DataAvailable can be removed at any time
            }
            catch (Exception ex)
            {
                Logger.LogExceptionToFile(ex, "VLC Audio");
            }
        }

        private static byte[] ToStereo(byte[] input, int fromChannels)
        {
            double ratio = fromChannels / 2d;
            int newLen = Convert.ToInt32(input.Length / ratio);
            var output = new byte[newLen];
            int outputIndex = 0;
            for (var n = 0; n < input.Length; n += (fromChannels * 2))
            {
                // copy in the first 16 bit sample
                output[outputIndex++] = input[n];
                output[outputIndex++] = input[n + 1];
                output[outputIndex++] = input[n + 2];
                output[outputIndex++] = input[n + 3];
            }
            return output;
        }
        #endregion

        private void FrameCallback(Bitmap frame)
        {
            var nf = NewFrame;
            if (nf== null || _stopping)
            {
                frame.Dispose();
                return;
            }
            _framesReceived++;

            _videoQueue.Enqueue(frame);            
        }

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



        #endregion

        public void Seek(float percentage)
        {
            if (_mPlayer != null && _mPlayer.IsSeekable)
            {
                _mPlayer.Position = percentage;
            }
        }



        private ConcurrentQueue<Bitmap> _videoQueue;
        private ConcurrentQueue<byte[]> _audioQueue;

        private void EventManager()
        {
            Bitmap frame;

            while (_stopEvent !=null && !_stopEvent.WaitOne(5, false) && !MainForm.ShuttingDown)
            {
                try
                {
                    var da = DataAvailable;
                    var nf = NewFrame;

                    if (_videoQueue.TryDequeue(out frame))
                    {
                        if (frame != null)
                        {
                            using (var b = (Bitmap) frame.Clone())
                            {
                                //new frame
                                nf?.Invoke(this, new NewFrameEventArgs(b));
                            }
                        }
                    }


                    byte[] audio;
                    if (!_audioQueue.TryDequeue(out audio)) continue;
                    da?.Invoke(this, new DataAvailableEventArgs(audio));

                    var sampleBuffer = new float[audio.Length];
                    _sampleChannel.Read(sampleBuffer, 0, audio.Length);

                    _waveProvider.AddSamples(audio, 0, audio.Length);

                    if (WaveOutProvider != null && Listening)
                    {
                        WaveOutProvider.AddSamples(audio, 0, audio.Length);
                    }
                }
                catch (Exception ex)
                {
                    Logger.LogExceptionToFile(ex,"VLC");
                }
            }
            try
            {
                while (_videoQueue != null && _videoQueue.TryDequeue(out frame))
                {
                    frame?.Dispose();
                }
            }
            catch
            {
                // ignored
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
                    _mFactory?.Dispose();
                }
                catch
                {
                    // ignored
                }
            }

            // Free any unmanaged objects here. 
            //
            _disposed = true;
        }
    }
}
