using System;
using System.IO;
using System.Threading;
using iSpyApplication.Utilities;
using NAudio.Wave;
using NAudio.Wave.SampleProviders;

namespace iSpyApplication.Sources.Audio.streams
{
    class DirectStream: IAudioSource, IDisposable
    {
        private Stream _stream;
        private float _gain;
        private bool _listening;
        private ManualResetEvent _abort = new ManualResetEvent(false);
        private ReasonToFinishPlaying _res = ReasonToFinishPlaying.DeviceLost;

        private Thread _thread;

        private BufferedWaveProvider _waveProvider;
        private SampleChannel _sampleChannel;

        public int PacketSize = 882;
        public int Interval = 40;
        public bool IsAudio => true;


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
        /// <remarks></remarks>
        /// 
        public virtual string Source => "direct stream";

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
        public DirectStream() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="LocalDeviceStream"/> class.
        /// </summary>
        /// 
        /// <param name="stream">source, which provides audio data.</param>
        /// 
        public DirectStream(Stream stream)
        {
            _stream = stream;
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
                if (_stream == null)
                    throw new ArgumentException("Audio source is not specified.");

                _waveProvider = new BufferedWaveProvider(RecordingFormat);
                _sampleChannel = new SampleChannel(_waveProvider);
                _sampleChannel.PreVolumeMeter += SampleChannelPreVolumeMeter;

                _abort.Reset();
                _thread = new Thread(DirectStreamListener)
                                          {
                                              Name = "DirectStream Audio Receiver"
                                          };
                _thread.Start();

            }
        }

        void SampleChannelPreVolumeMeter(object sender, StreamVolumeEventArgs e)
        {
            LevelChanged?.Invoke(this, new LevelChangedEventArgs(e.MaxSampleValues));
        }


        private void DirectStreamListener()
        {
            _abort.Reset();
            try
            {
                var data = new byte[PacketSize];
                if (_stream != null)
                {
                    while (!_abort.WaitOne(0) && !MainForm.ShuttingDown)
                    {
                        var da = DataAvailable;
                        if (da != null)
                        {
                            int recbytesize = _stream.Read(data, 0, PacketSize);
                            if (recbytesize > 0)
                            {
                                if (_sampleChannel != null)
                                {
                                    _waveProvider.AddSamples(data, 0, recbytesize);

                                    var sampleBuffer = new float[recbytesize];
                                    int read = _sampleChannel.Read(sampleBuffer, 0, recbytesize);

                                    da(this, new DataAvailableEventArgs((byte[])data.Clone(), read));
                                    
                                    if (Listening)
                                    {
                                        WaveOutProvider?.AddSamples(data, 0, read);
                                    }
                                }
                                
                            }
                            else
                            {
                                break;
                            }

                            if (_abort.WaitOne(Interval, false))
                                break;
                        }

                        
                    }
                }
            }
            catch (Exception e)
            {
                _res = ReasonToFinishPlaying.DeviceLost;
                Logger.LogException(e,"Direct");
            }
            
            _stream?.Close();
            _stream = null;


            if (_sampleChannel != null)
                _sampleChannel.PreVolumeMeter -= SampleChannelPreVolumeMeter;

            if (_waveProvider != null && _waveProvider.BufferedBytes > 0)
                _waveProvider.ClearBuffer();

            if (WaveOutProvider?.BufferedBytes > 0) WaveOutProvider.ClearBuffer();

            AudioFinished?.Invoke(this, new PlayingFinishedEventArgs(_res));
            _abort.Close();
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
                AudioFinished?.Invoke(this, new PlayingFinishedEventArgs(_res));
            }
        }

        public void Restart()
        {
            if (!IsRunning) return;
            _res = ReasonToFinishPlaying.Restart;
            _abort?.Set();
        }


        public WaveFormat RecordingFormat { get; set; }

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
                _abort?.Close();
            }

            // Free any unmanaged objects here. 
            //
            _disposed = true;
        }
    }
}
