using System;
using iSpyApplication.Utilities;
using NAudio.Wave;
using NAudio.Wave.SampleProviders;

namespace iSpyApplication.Sources.Audio.streams
{
    class LocalDeviceStream: IAudioSource
    {
        private string _source;
        private volatile bool _started;
        private float _gain;
        private bool _listening;

        private WaveInEvent _waveIn;
        private WaveInProvider _waveProvider;
        private SampleChannel _sampleChannel;
        public bool IsAudio => true;

        private ReasonToFinishPlaying _res = ReasonToFinishPlaying.DeviceLost;

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
                    WaveOutProvider = new BufferedWaveProvider(RecordingFormat) { DiscardOnBufferOverflow = true, BufferDuration = TimeSpan.FromMilliseconds(500)};
                }
                
                _listening = value;
            }
        }


        /// <summary>
        /// State of the audio source.
        /// </summary>
        /// 
        /// <remarks>Current state of audio source object - running or not.</remarks>
        /// 
        public bool IsRunning => _started;

        /// <summary>
        /// Initializes a new instance of the <see cref="LocalDeviceStream"/> class.
        /// </summary>
        /// 
        public LocalDeviceStream() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="LocalDeviceStream"/> class.
        /// </summary>
        /// 
        /// <param name="source">source, which provides audio data.</param>
        /// 
        public LocalDeviceStream(string source)
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


            if (_started) return;
            
            // check source
            lock (_lock)
            {
                if (_started)
                    return;

                int i = 0, selind = -1;
                for (var n = 0; n < WaveIn.DeviceCount; n++)
                {
                    if (WaveIn.GetCapabilities(n).ProductName == _source)
                        selind = i;
                    i++;
                }
                if (selind == -1)
                {
                    AudioFinished?.Invoke(this, new PlayingFinishedEventArgs(ReasonToFinishPlaying.DeviceLost));
                    return;
                }
                _started = true;
                _res = ReasonToFinishPlaying.DeviceLost;
                _waveIn = new WaveInEvent
                          {
                              BufferMilliseconds = 200,
                              DeviceNumber = selind,
                              WaveFormat = RecordingFormat
                          };
                _waveIn.DataAvailable += WaveInDataAvailable;
                _waveIn.RecordingStopped += WaveInRecordingStopped;

                _waveProvider = new WaveInProvider(_waveIn);
                _sampleChannel = new SampleChannel(_waveProvider);
                _sampleChannel.PreVolumeMeter += SampleChannelPreVolumeMeter;
                _waveIn.StartRecording();
            }
        }

        void SampleChannelPreVolumeMeter(object sender, StreamVolumeEventArgs e)
        {
            LevelChanged?.Invoke(this, new LevelChangedEventArgs(e.MaxSampleValues));
        }

        void WaveInDataAvailable(object sender, WaveInEventArgs e)
        {
            if (_waveIn == null) return;
            try { 
                var da = DataAvailable;
                if (da == null) return;
                var sc = _sampleChannel;
                if (sc == null) return;
                var sampleBuffer = new float[e.BytesRecorded];
                int read = sc.Read(sampleBuffer, 0, e.BytesRecorded);

                da(this, new DataAvailableEventArgs((byte[])e.Buffer.Clone(), read));

                if (Listening)
                {
                    WaveOutProvider?.AddSamples(e.Buffer, 0, read);
                }
            }
            catch (Exception ex)
            {
                var af = AudioFinished;
                af?.Invoke(this, new PlayingFinishedEventArgs(ReasonToFinishPlaying.DeviceLost));

                Logger.LogException(ex, "AudioDevice");
            }
        }


        void WaveInRecordingStopped(object sender, StoppedEventArgs e)
        {
            _started = false;
            if (e.Exception!=null && e.Exception.Message.IndexOf("NoDriver", StringComparison.Ordinal)!=-1)
                _res = ReasonToFinishPlaying.DeviceLost;
            AudioFinished?.Invoke(this, new PlayingFinishedEventArgs(_res));
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
            _res = ReasonToFinishPlaying.StoppedByUser;
            if (_waveIn == null)
            {
                AudioFinished?.Invoke(this, new PlayingFinishedEventArgs(_res));
            }
            else
                StopSource();
        }

        private void StopSource()
        {
            var wi = _waveIn;
            if (wi == null)
                return;
            // signal to stop
            var sc = _sampleChannel;
            if (sc != null)
                sc.PreVolumeMeter -= SampleChannelPreVolumeMeter;
            wi.DataAvailable -= WaveInDataAvailable;
            try { wi.StopRecording(); }
            catch (Exception ex)
            {
                Logger.LogException(ex, "Device");
            }
            wi.RecordingStopped -= WaveInRecordingStopped;
            if (WaveOutProvider?.BufferedBytes > 0) WaveOutProvider?.ClearBuffer();

            wi.Dispose();
            _waveIn = null;
            _started = false;
        }

        public void Restart()
        {
            _res = ReasonToFinishPlaying.Restart;
            StopSource();
        }


        public WaveFormat RecordingFormat { get; set; }
    }
}
