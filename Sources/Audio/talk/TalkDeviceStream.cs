using System;
using iSpyApplication.Sources.Audio.streams;
using NAudio.Wave;
using NAudio.Wave.SampleProviders;

namespace iSpyApplication.Sources.Audio.talk
{
    class TalkDeviceStream: IAudioSource
    {
        private string _source;
        private volatile bool _isrunning;
        private float _gain;
        private bool _listening;

        private WaveInEvent _waveIn;
        private WaveInProvider _waveProvider;
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


        /// <summary>
        /// State of the audio source.
        /// </summary>
        /// 
        /// <remarks>Current state of audio source object - running or not.</remarks>
        /// 
        public bool IsRunning => _isrunning;

        /// <summary>
        /// Initializes a new instance of the <see cref="LocalDeviceStream"/> class.
        /// </summary>
        /// 
        public TalkDeviceStream() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="LocalDeviceStream"/> class.
        /// </summary>
        /// 
        /// <param name="source">source, which provides audio data.</param>
        /// 
        public TalkDeviceStream(string source)
        {
            _source = source;
        }

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
            if (!IsRunning)
            {
                // check source

                int i = 0, selind = -1;
                for (int n = 0; n < WaveIn.DeviceCount; n++)
                {
                    if (WaveIn.GetCapabilities(n).ProductName == _source)
                        selind = i;
                    i++;
                }
                if (selind == -1)
                {
                    //device no longer connected or not configured
                    if (i > 0)
                        selind = 0;
                    else
                    {
                        //if (AudioSourceError != null)
                        //    AudioSourceError(this, new AudioSourceErrorEventArgs("not connected"));
                        AudioFinished?.Invoke(this, new PlayingFinishedEventArgs(ReasonToFinishPlaying.DeviceLost));
                        return;    
                    }
                    
                }

                _waveIn = new WaveInEvent { BufferMilliseconds = 200, DeviceNumber = selind, WaveFormat = RecordingFormat };
                _waveIn.DataAvailable += WaveInDataAvailable;
                _waveIn.RecordingStopped += WaveInRecordingStopped;

                _waveProvider = new WaveInProvider(_waveIn);
                _sampleChannel = new SampleChannel(_waveProvider);
                
                if (LevelChanged != null)
                {
                    _sampleChannel.PreVolumeMeter += SampleChannelPreVolumeMeter;
                }
                _waveIn.StartRecording();

            }
        }

        void SampleChannelPreVolumeMeter(object sender, StreamVolumeEventArgs e)
        {
            LevelChanged?.Invoke(this, new LevelChangedEventArgs(e.MaxSampleValues));
        }

        void WaveInDataAvailable(object sender, WaveInEventArgs e)
        {
            _isrunning = true;
            if (DataAvailable != null)
            {
                //forces processing of volume level without piping it out
                int l = e.BytesRecorded;
                if (_sampleChannel != null)
                {
                    var sampleBuffer = new float[e.BytesRecorded];
                    l = _sampleChannel.Read(sampleBuffer, 0, e.BytesRecorded);
                }
                if (Listening)
                {
                    WaveOutProvider?.AddSamples(e.Buffer, 0, l);
                }
                
                DataAvailable(this, new DataAvailableEventArgs((byte[])e.Buffer.Clone(), l));
            }
        }

        void WaveInRecordingStopped(object sender, EventArgs e)
        {
            _isrunning = false;
            AudioFinished?.Invoke(this, new PlayingFinishedEventArgs(ReasonToFinishPlaying.StoppedByUser));
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
            if (_sampleChannel != null)
                _sampleChannel.PreVolumeMeter -= SampleChannelPreVolumeMeter;

            if (_waveIn != null)
            {
                // signal to stop
                _waveIn.DataAvailable -= WaveInDataAvailable;
                _waveIn.StopRecording();
                _waveIn.RecordingStopped -= WaveInRecordingStopped;

                if (WaveOutProvider != null)
                {
                    if (WaveOutProvider.BufferedBytes>0) WaveOutProvider.ClearBuffer();
                    WaveOutProvider = null;
                }

                _waveIn.Dispose();
                _waveIn = null;

            }
        }

        public void Restart()
        {
            
        }


        public WaveFormat RecordingFormat { get; set; }
    }
}
