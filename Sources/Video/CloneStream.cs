using System.Drawing;
using iSpyApplication.Sources.Audio;
using NAudio.Wave;

namespace iSpyApplication.Sources.Video
{
    public class CloneStream : IVideoSource, IAudioSource
    {
        private readonly IVideoSource _source;
        private bool _disposed;

        public CloneStream()
        {
            _source = null;
        }


        public CloneStream(IVideoSource source)
        {
            _source = source;
        }

        public int FrameInterval { get; set; }

        #region IVideoSource Members

        public event NewFrameEventHandler NewFrame;

        public event PlayingFinishedEventHandler PlayingFinished;

        public event DataAvailableEventHandler DataAvailable;
        public event LevelChangedEventHandler LevelChanged;
        public event AudioFinishedEventHandler AudioFinished;

        public virtual string Source => _source==null?"None":_source.Source;

        public WaveFormat RecordingFormat { get; } = null;
        public BufferedWaveProvider WaveOutProvider { get; set; }

        public bool Listening { get; set; }


        public bool IsRunning => (_source != null && _source.IsRunning);


        public void Start()
        {
            if (_source == null)
                return;
            _source.NewFrame -= SourceNewFrame;
            _source.NewFrame += SourceNewFrame;
        }


        void SourceNewFrame(object sender, NewFrameEventArgs eventArgs)
        {
            var bm = (Bitmap)eventArgs.Frame.Clone();
            NewFrame?.Invoke(this, new NewFrameEventArgs(bm));
            bm.Dispose();
        }


        public void Stop()
        {
            if (_source != null)
                _source.NewFrame -= SourceNewFrame;
        }

        public void Restart()
        {
            //ignore
        }

        #endregion

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
