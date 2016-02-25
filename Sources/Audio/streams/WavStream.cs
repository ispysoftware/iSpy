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
        private ManualResetEvent _stopEvent;
        private bool _listening;

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

            _stopEvent = new ManualResetEvent(false);
            _thread = new Thread(StreamWav)
                      {
                          Name = "WavStream Audio Receiver (" + _source + ")"
                      };
            _thread.Start();
        }

        

        private void StreamWav()
        {
            var res = ReasonToFinishPlaying.StoppedByUser;
            HttpWebRequest request = null;
            try
            {
                using (HttpWebResponse resp = ConnectionFactory.GetResponse(_source,"GET", out request))
                {
                    //1/4 of a second, 16 byte buffer
                    var data = new byte[((RecordingFormat.SampleRate/4)*2)*RecordingFormat.Channels];

                    using (var stream = resp.GetResponseStream())
                    {
                        if (stream == null)
                            throw new Exception("Stream is null");

                        while (!_stopEvent.WaitOne(10, false) && !MainForm.ShuttingDown)
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
                res = ReasonToFinishPlaying.DeviceLost;
                Logger.LogExceptionToFile(ex,"WavStream");
            }
            finally
            {
                AudioFinished?.Invoke(this, new PlayingFinishedEventArgs(res) );
            }
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
            if (IsRunning)
            {
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

                Free();
            }
        }

        /// <summary>
        /// Free resource.
        /// </summary>
        /// 
        private void Free()
        {
            _thread = null;

            // release events
            if (_stopEvent != null)
            {
                _stopEvent.Close();
                _stopEvent.Dispose();
            }
            _stopEvent = null;
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
                _stopEvent?.Close();
            }

            // Free any unmanaged objects here. 
            //
            _disposed = true;
        }

    }
}
