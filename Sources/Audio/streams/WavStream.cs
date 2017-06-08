using System;
using System.Net;
using System.Threading;
using System.Windows.Forms;
using iSpyApplication.Utilities;
using NAudio.Wave;
using NAudio.Wave.SampleProviders;

namespace iSpyApplication.Sources.Audio.streams
{
    public class WavStream : IAudioSource, IDisposable
    {
        private string _source;
        private ManualResetEvent _abort;
        private ReasonToFinishPlaying _res = ReasonToFinishPlaying.DeviceLost;
        private bool _listening;
        public bool IsAudio => true;
        private Thread _thread;
        private BufferedWaveProvider _waveProvider;
        private SampleChannel _sampleChannel;
        public BufferedWaveProvider WaveOutProvider { get; set; }

        private float _gain;

        public WaveFormat RecordingFormat { get; set; }

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
                    WaveOutProvider = new BufferedWaveProvider(RecordingFormat);
                }

                _listening = value;
            }
        }

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
        /// <param name="source">source, which provides audio data.</param>
        /// 
        public WavStream(string source)
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
            if (IsRunning) return;
            // check source
            if (string.IsNullOrEmpty(_source))
                throw new ArgumentException("Audio source is not specified.");

            _waveProvider = new BufferedWaveProvider(RecordingFormat) { DiscardOnBufferOverflow = true, BufferDuration = TimeSpan.FromMilliseconds(500) };
            _sampleChannel = new SampleChannel(_waveProvider);
            _sampleChannel.PreVolumeMeter += SampleChannelPreVolumeMeter;

            _res = ReasonToFinishPlaying.DeviceLost;
            _thread = new Thread(StreamWav)
                      {
                          Name = "WavStream Audio Receiver (" + _source + ")"
                      };
            _thread.Start();
        }

        
        private readonly ConnectionFactory _connectionFactory = new ConnectionFactory();
        private void StreamWav()
        {
            _abort = new ManualResetEvent(false);
            HttpWebRequest request = null;
            try
            {
                using (HttpWebResponse resp = _connectionFactory.GetResponse(_source,"GET","", out request))
                {
                    //1/4 of a second, 16 byte buffer
                    var data = new byte[((RecordingFormat.SampleRate/4)*2)*RecordingFormat.Channels];

                    using (var stream = resp.GetResponseStream())
                    {
                        if (stream == null)
                            throw new Exception("Stream is null");

                        while (!_abort.WaitOne(20) && !MainForm.ShuttingDown)
                        {
                            var da = DataAvailable;
                            if (da != null)
                            {
                                int recbytesize = stream.Read(data, 0, data.Length);
                                if (recbytesize == 0)
                                    throw new Exception("lost stream");


                                if (_sampleChannel == null) continue;
                                _waveProvider.AddSamples(data, 0, recbytesize);

                                var sampleBuffer = new float[recbytesize];
                                int read = _sampleChannel.Read(sampleBuffer, 0, recbytesize);

                                da(this, new DataAvailableEventArgs((byte[])data.Clone(), read));

                                if (Listening)
                                {
                                    WaveOutProvider?.AddSamples(data, 0, read);
                                }
                            }
                            else
                            {
                                break;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _res = ReasonToFinishPlaying.DeviceLost;
                Logger.LogException(ex,"WavStream");
            }

            AudioFinished?.Invoke(this, new PlayingFinishedEventArgs(_res));
            _abort.Close();
        }

        void SampleChannelPreVolumeMeter(object sender, StreamVolumeEventArgs e)
        {
            LevelChanged?.Invoke(this, new LevelChangedEventArgs(e.MaxSampleValues));
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
