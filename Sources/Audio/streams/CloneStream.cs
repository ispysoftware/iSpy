using System;
using iSpyUniversal.Objects;
using NAudio.Wave;
using NAudio.Wave.SampleProviders;

namespace iSpyUniversal.Sources.Audio.streams
{
    public class CloneStream : IAudioSource
    {
        private long _bytesReceived;
        private int _framesReceived;
        private readonly VolumeLevel _source;

        public CloneStream()
        {
            _source = null;
        }


        public CloneStream(VolumeLevel source)
        {
            _source = source;
        }

        public event PlayingFinishedEventHandler PlayingFinished;

        public long BytesReceived
        {
            get
            {
                long bytes = _bytesReceived;
                _bytesReceived = 0;
                return bytes;
            }
        }


        public event DataAvailableEventHandler AudioAvailable;
        public event LevelChangedEventHandler LevelChanged;
        public event AudioFinishedEventHandler AudioFinished;
        public event Delegates.ErrorHandler ErrorHandler;

        public virtual string Source => _source == null ? "None" : _source.ObjectName;

        public WaveFormat RecordingFormat
        {
            get { return _source.Source?.RecordingFormat; }
        }

        public BufferedWaveProvider WaveOutProvider { get; set; }

        public bool Listening { get; set; }


        public int FramesReceived
        {
            get
            {
                int frames = _framesReceived;
                _framesReceived = 0;
                return frames;
            }
        }


        public bool IsRunning => (_source != null && _source.IsEnabled);

        void SampleChannelPreVolumeMeter(object sender, StreamVolumeEventArgs e)
        {
            LevelChanged?.Invoke(this, new LevelChangedEventArgs(e.MaxSampleValues));
        }

        public void Start()
        {
            if (_source == null)
                return;
            _source.NewFrameHandler -= SourceDataAvailable;
            _source.NewFrameHandler += SourceDataAvailable;
            _source.LevelChanged -= LevelChanged;
            _source.LevelChanged += LevelChanged;
        }


        void SourceDataAvailable(object sender, DataAvailableEventArgs eventArgs)
        {
            try
            {
                AudioAvailable?.Invoke(this, eventArgs);
            }
            catch (Exception ex)
            {
                ErrorHandler?.Invoke(ex.Message);
            }
        }


        public void Stop(ReasonToFinishPlaying reason)
        {
            if (_source != null)
            {
                _source.NewFrameHandler -= SourceDataAvailable;
                _source.LevelChanged -= LevelChanged;
            }
        }
    }
}
