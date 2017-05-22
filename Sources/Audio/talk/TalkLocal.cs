using System;
using NAudio.Wave;

namespace iSpyApplication.Sources.Audio.talk
{
    internal class TalkLocal: ITalkTarget, IDisposable
    {
        private readonly object _obj = new object();
        private bool _bTalking;
        private readonly IAudioSource _audioSource;
        private IWavePlayer _waveOut;
        private BufferedWaveProvider _bwp;

        public TalkLocal(IAudioSource audioSource)
        {
            _audioSource = audioSource;
        }

        public void Start()
        {
            _audioSource.Listening = true;
            _audioSource.DataAvailable -= AudioSourceDataAvailable;
            _audioSource.DataAvailable -= _audioSource_DataAvailablePipe;
            if (_audioSource.WaveOutProvider == null)
            {
                _bwp = new BufferedWaveProvider(_audioSource.RecordingFormat);
                _audioSource.DataAvailable += AudioSourceDataAvailable;

                _waveOut = new DirectSoundOut(100);
                _waveOut.Init(_bwp);
                _waveOut.Play();
                _bTalking = true;
            }
            else
            {
                _waveOut = new DirectSoundOut(100);
                _waveOut.Init(_audioSource.WaveOutProvider);
                _waveOut.Play();
                _bTalking = true;
                _audioSource.DataAvailable += _audioSource_DataAvailablePipe;
            }
        }

        void _audioSource_DataAvailablePipe(object sender, DataAvailableEventArgs eventArgs)
        {
            //event here because it's checked for null
        }

        void AudioSourceDataAvailable(object sender, DataAvailableEventArgs eventArgs)
        {
            _bwp.AddSamples(eventArgs.RawData,0,eventArgs.BytesRecorded);
        }
        
        public void Stop()
        {
            if (_bTalking)
            {
                lock (_obj)
                {
                    if (_bTalking)
                    {
                        _bTalking = false;
                    }
                    TalkStopped?.Invoke(this, EventArgs.Empty);
                }
            }
            if (_audioSource != null)
            {
                _audioSource.Listening = false;
                _audioSource.DataAvailable -= AudioSourceDataAvailable;
                _audioSource.DataAvailable -= _audioSource_DataAvailablePipe;
            }

            if (_waveOut != null)
            {
                _waveOut.Stop();
                _waveOut.Dispose();
            }
        }

        public bool Connected => true;

        public event TalkStoppedEventHandler TalkStopped;

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
                _waveOut?.Dispose();
            }

            // Free any unmanaged objects here. 
            //
            _disposed = true;
        }
    }
}