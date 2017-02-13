using System;
using System.IO;
using System.Net;
using System.Threading;
using System.Windows.Forms;
using iSpyApplication.Utilities;
using NAudio.Wave;
using NAudio.Wave.SampleProviders;

namespace iSpyApplication.Sources.Audio.streams
{
    public class MP3Stream : IAudioSource, IDisposable
    {
        private string _source;
        private ManualResetEvent _stopEvent;
        private bool _listening;

        private Thread _thread;
        public WaveFormat RecordingFormat { get; private set; }
        public BufferedWaveProvider WaveOutProvider { get; set; }
        private SampleChannel _sampleChannel;
        private float _gain;


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
        public MP3Stream(string source)
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
                if (string.IsNullOrEmpty(_source))
                    throw new ArgumentException("Audio source is not specified.");

                _stopEvent = new ManualResetEvent(false);
                _thread = new Thread(StreamMP3)
                {
                    Name = "MP3 Audio Receiver (" + _source + ")"
                };
                _thread.Start();

            }
        }

        private BufferedWaveProvider _bufferedWaveProvider;

        private void StreamMP3()
        {
            HttpWebRequest request = null;
            
            try
            {
                var resp = ConnectionFactory.GetResponse(_source,"GET", out request);
                var buffer = new byte[16384 * 4]; // needs to be big enough to hold a decompressed frame
                IMp3FrameDecompressor decompressor = null;

                using (var responseStream = resp.GetResponseStream())
                {
                    var readFullyStream = new ReadFullyStream(responseStream);
                    while (!_stopEvent.WaitOne(10, false) && !MainForm.ShuttingDown)
                    {
                        if (_bufferedWaveProvider != null &&
                            _bufferedWaveProvider.BufferLength - _bufferedWaveProvider.BufferedBytes <
                            _bufferedWaveProvider.WaveFormat.AverageBytesPerSecond/4)
                        {
                            //Debug.WriteLine("Buffer getting full, taking a break");
                            Thread.Sleep(100);
                        }
                        else
                        {
                            var da = DataAvailable;
                            if (da != null)
                            {
                                Mp3Frame frame;
                                try
                                {
                                    frame = Mp3Frame.LoadFromStream(readFullyStream);
                                }
                                catch (EndOfStreamException)
                                {
                                    // reached the end of the MP3 file / stream
                                    break;
                                }
                                catch (WebException)
                                {
                                    // probably we have aborted download from the GUI thread
                                    break;
                                }
                                if (decompressor == null || _bufferedWaveProvider == null)
                                {
                                    // don't think these details matter too much - just help ACM select the right codec
                                    // however, the buffered provider doesn't know what sample rate it is working at
                                    // until we have a frame
                                    WaveFormat waveFormat = new Mp3WaveFormat(frame.SampleRate,
                                        frame.ChannelMode == ChannelMode.Mono ? 1 : 2, frame.FrameLength, frame.BitRate);

                                    RecordingFormat = new WaveFormat(frame.SampleRate, 16,
                                        frame.ChannelMode == ChannelMode.Mono ? 1 : 2);

                                    decompressor = new AcmMp3FrameDecompressor(waveFormat);
                                    _bufferedWaveProvider = new BufferedWaveProvider(decompressor.OutputFormat)
                                                            {BufferDuration = TimeSpan.FromSeconds(5)};

                                    _sampleChannel = new SampleChannel(_bufferedWaveProvider);
                                    _sampleChannel.PreVolumeMeter += SampleChannelPreVolumeMeter;

                                }

                                int decompressed = decompressor.DecompressFrame(frame, buffer, 0);
                                _bufferedWaveProvider.AddSamples(buffer, 0, decompressed);

                                var sampleBuffer = new float[buffer.Length];
                                int read = _sampleChannel.Read(sampleBuffer, 0, buffer.Length);

                                da(this, new DataAvailableEventArgs((byte[])buffer.Clone(), read));

                                if (Listening)
                                {
                                    WaveOutProvider?.AddSamples(buffer, 0, read);
                                }
                            }
                        }
                        if (_stopEvent.WaitOne(0, false))
                            break;
                    }

                    AudioFinished?.Invoke(this, new PlayingFinishedEventArgs(ReasonToFinishPlaying.StoppedByUser));

                    // was doing this in a finally block, but for some reason
                    // we are hanging on response stream .Dispose so never get there
                    if (decompressor != null)
                    {
                        decompressor.Dispose();
                        decompressor = null;
                    }

                }
            }
            catch (Exception ex)
            {
                var af = AudioFinished;
                af?.Invoke(this, new PlayingFinishedEventArgs(ReasonToFinishPlaying.DeviceLost));

                Logger.LogExceptionToFile(ex,"MP3Stream");
            }
            finally
            {
                try
                {
                    request?.Abort();
                }
                catch { }
                request = null;                
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
                catch { }

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
