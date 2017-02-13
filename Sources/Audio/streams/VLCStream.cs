using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;
using Declarations;
using Declarations.Events;
using Declarations.Media;
using Declarations.Players;
using iSpyApplication.Utilities;
using Implementation;
using NAudio.Wave;
using NAudio.Wave.SampleProviders;

namespace iSpyApplication.Sources.Audio.streams
{
    public class VLCStream : IAudioSource, IDisposable
    {
        private volatile bool _stopRequested, _stopping;
        private readonly string[] _arguments;
        private int _framesReceived;
        private IMediaPlayerFactory _mFactory;
        private IMedia _mMedia;
        private IVideoPlayer _mPlayer;
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

        // URL for VLCstream
        private string _source;

        /// <summary>
        /// Initializes a new instance of the <see cref="VLCStream"/> class.
        /// </summary>
        /// 
        public VLCStream()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="VLCStream"/> class.
        /// </summary>
        /// 
        /// <param name="source">URL, which provides VLCstream.</param>
        /// <param name="arguments"></param>
        public VLCStream(string source, string[] arguments)
        {
            _source = source;
            _arguments = arguments;
        }

        public IAudioSource OutAudio;

        #region IVideoSource Members

        /// <summary>
        /// Video source error event.
        /// </summary>
        /// 
        /// <remarks>This event is used to notify clients about any type of errors occurred in
        /// video source object, for example internal exceptions.</remarks>
        /// 
        //public event VideoSourceErrorEventHandler VideoSourceError;

        /// <summary>
        /// Video playing finished event.
        /// </summary>
        /// 
        /// <remarks><para>This event is used to notify clients that the video playing has finished.</para>
        /// </remarks>
        /// 
        //public event PlayingFinishedEventHandler PlayingFinished;

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


        public void Start()
        {
            if (!IsRunning)
            {
                if (!VlcHelper.VlcInstalled)
                    return;

                _stopRequested = false;

                if (string.IsNullOrEmpty(_source))
                    throw new ArgumentException("Audio source is not specified.");

                // create events
                _stopEvent = new ManualResetEvent(false);

                // create and start new thread
                _thread = new Thread(WorkerThread) { Name = _source };
                _thread.Start();
            }
        }


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
		                "--plugin-path=./plugins",
                        "--novideo"
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
                    Logger.LogExceptionToFile(ex, "VLC Audio");
                    Logger.LogMessageToFile("VLC arguments are: " + String.Join(",", args.ToArray()), "VLC Audio");
                    Logger.LogMessageToFile("Using default VLC configuration.", "VLC Audio");
                    _mFactory = new MediaPlayerFactory(args.ToArray());
                }
                GC.KeepAlive(_mFactory);
            }

            _mMedia = file ? _mFactory.CreateMedia<IMediaFromFile>(_source) : _mFactory.CreateMedia<IMedia>(_source);

            _mMedia.Events.DurationChanged += EventsDurationChanged;
            _mMedia.Events.StateChanged += EventsStateChanged;

            
            try
            {
                _mPlayer?.Dispose();
            }
            catch
            {
                // ignored
            }
            _mPlayer = null;
            


            _mPlayer = _mFactory.CreatePlayer<IVideoPlayer>();
            _mPlayer.Events.TimeChanged += EventsTimeChanged;

            var fc = new Func<SoundFormat, SoundFormat>(SoundFormatCallback);
            _mPlayer.CustomAudioRenderer.SetFormatCallback(fc);
            var ac = new AudioCallbacks { SoundCallback = SoundCallback };
            _mPlayer.CustomAudioRenderer.SetCallbacks(ac);
            _mPlayer.CustomAudioRenderer.SetExceptionHandler(Handler);
            GC.KeepAlive(_mPlayer);

            _needsSetup = true;
            _stopping = false;

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
            _mPlayer.Play();

            _stopEvent.WaitOne();

            if (!Seekable && !_stopRequested)
            {
                AudioFinished?.Invoke(this, new PlayingFinishedEventArgs(ReasonToFinishPlaying.DeviceLost));
            }
            else
            {
                AudioFinished?.Invoke(this, new PlayingFinishedEventArgs(ReasonToFinishPlaying.StoppedByUser));
            }

            DisposePlayer();
            _stopEvent?.Close();
            _stopEvent = null;
        }

        void Handler(Exception ex)
        {
            Logger.LogExceptionToFile(ex, "VLC Audio");
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
                            _stopEvent.Set();
                        }
                        catch
                        {
                            // ignored
                        }
                    }
                    break;
            }
        }

        private long _lastFrame = DateTime.MinValue.Ticks;

        public DateTime LastFrame
        {
            get { return new DateTime(_lastFrame); }
            set { Interlocked.Exchange(ref _lastFrame, value.Ticks); }
        }

        public int TimeOut = 8000;

        public void CheckTimestamp()
        {
            //some feeds keep returning frames even when the connection is lost
            //this detects that by comparing timestamps from the eventstimechanged event
            //and signals an error if more than 8 seconds ago
            var q = LastFrame > DateTime.MinValue && (Helper.Now - LastFrame).TotalMilliseconds > TimeOut;

            if (!q || _stopping) return;
            if (_stopEvent != null)
            {

                Stop();
            }
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
            int chan = _realChannels = sf.Channels;
            if (chan > 1)
                chan = 2;//downmix
            _recordingFormat = new WaveFormat(sf.Rate, 16, chan);
            _waveProvider = new BufferedWaveProvider(RecordingFormat);
            _sampleChannel = new SampleChannel(_waveProvider);

            _sampleChannel.PreVolumeMeter += SampleChannelPreVolumeMeter;
            _needsSetup = false;
            if (HasAudioStream != null)
            {
                HasAudioStream(this, EventArgs.Empty);
                HasAudioStream = null;
            }

            return sf;
        }

        void SampleChannelPreVolumeMeter(object sender, StreamVolumeEventArgs e)
        {
            if (e?.MaxSampleValues != null)
                LevelChanged?.Invoke(this, new LevelChangedEventArgs(e.MaxSampleValues));
        }

        private void SoundCallback(Sound soundData)
        {
            var da = DataAvailable;
            if (da == null || _needsSetup) return;

            try
            {
                var data = new byte[soundData.SamplesSize];
                Marshal.Copy(soundData.SamplesData, data, 0, (int)soundData.SamplesSize);

                if (_realChannels > 2)
                {
                    //resample audio to 2 channels
                    data = ToStereo(data, _realChannels);
                }

                _waveProvider?.AddSamples(data, 0, data.Length);

                //forces processing of volume level without piping it out
                var sampleBuffer = new float[data.Length];
                int read = _sampleChannel.Read(sampleBuffer, 0, data.Length);

                da(this, new DataAvailableEventArgs((byte[])data.Clone(),read));

                if (Listening)
                {
                    WaveOutProvider?.AddSamples(data, 0, read);
                }
            }
            catch
            {
                //can fail at shutdown
            }
        }

        private static byte[] ToStereo(byte[] input, int fromChannels)
        {
            var ratio = fromChannels / 2d;
            var newLen = Convert.ToInt32(input.Length / ratio);
            var output = new byte[newLen];
            var outputIndex = 0;
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
            if (IsRunning)
            {
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
        }

       
        #endregion

        public void Seek(float percentage)
        {
            if (_mPlayer.IsSeekable)
            {
                _mPlayer.Position = percentage;
            }
        }


        private void DisposePlayer()
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
                Logger.LogExceptionToFile(ex, "VLC");
            }
            _waveProvider = null;

            Listening = false;
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
                _mFactory?.Dispose();
                _stopEvent?.Close();
            }

            // Free any unmanaged objects here. 
            //
            _disposed = true;
        }
    }
}