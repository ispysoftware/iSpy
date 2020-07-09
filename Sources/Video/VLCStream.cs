using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;
using LibVLCSharp.Shared;
using iSpyApplication.Controls;
using iSpyApplication.Sources.Audio;
using iSpyApplication.Utilities;
using NAudio.Wave;
using NAudio.Wave.SampleProviders;
using File = System.IO.File;
using System.Diagnostics;
using System.Threading.Tasks;
using static iSpyApplication.Delegates;
using System.Text;

namespace iSpyApplication.Sources.Video
{
    internal class VlcStream : VideoBase, IVideoSource, IAudioSource, ISupportsAudio
    {
        public string Source { get; }
        private CameraWindow _camera;

        public bool IsRunning { get; set; }
        public bool Seekable = false;
        public long Time, Duration;
        private volatile bool _stopping;

        public WaveFormat RecordingFormat
        {
            get
            {
                return new WaveFormat(22050, 16, 1);
            }
            set
            {
                //ignore
            }
        }

        public bool IsAudio { get; }
        private bool _listening;
        private bool _audioInited = false;
        private BufferedWaveProvider _waveProvider = null;
        private SampleChannel _sampleChannel;

        public event NewFrameEventHandler NewFrame;
        public event ErrorHandler ErrorHandler;
        public event PlayingFinishedEventHandler PlayingFinished;
        public event DataAvailableEventHandler DataAvailable;
        public event AudioFinishedEventHandler AudioFinished;
        public event LevelChangedEventHandler LevelChanged;
        public event HasAudioStreamEventHandler HasAudioStream;

        private int _timeoutMilliSeconds;
        private int _connectMilliSeconds = 10000;
        private bool _ignoreAudio;
        private bool _disposed;
        private List<string> _options;
        private Size _size;
        private GCHandle? _imageData = null;
        private MediaPlayer _mediaPlayer = null;
        private bool _connecting = false;

        private LibVLC _libVLC = null;
        private bool _failedLoad = false;
        private static bool _coreInitialized = false;
        private static object _coreLock = new object();
        private DateTime _lastTimeUpdate = DateTime.MinValue;
        private AutoResetEvent _stopped = new AutoResetEvent(false);
        private LibVLC LibVLC
        {
            get
            {
                if (_libVLC != null) return _libVLC;

                if (!_coreInitialized)
                {
                    lock (_coreLock)
                    {
                        if (!_coreInitialized)
                        {
                            try
                            {
                                Core.Initialize(VlcHelper.VLCLocation);
                                _coreInitialized = true;
                            }
                            catch(Exception ex)
                            {
                                Logger.LogException(ex);
                                _failedLoad = true;
                                throw new ApplicationException("VLC (v" + VlcHelper.MinVersion + "+) failed to initialise. Set location in settings.");
                            }
                        }
                    }


                }
                try
                {
                    _libVLC = new LibVLC(new string[]
                    {
                    "--ignore-config",
                    "--no-osd"
                    });
                }
                catch (Exception ex)
                {
                    Logger.LogException(ex, "VLC Setup");
                    _failedLoad = true;
                    throw new ApplicationException("VLC not found (v"+VlcHelper.MinVersion+"+). Set location in settings.");
                }
                _libVLC.Log += _libVLC_Log;
                //GC.KeepAlive(_libVLC);
                return _libVLC;
            }
        }

        private static void _libVLC_Log(object sender, LogEventArgs e)
        {
            Debug.WriteLine("vlc: " + e.Message);
        }

        private MediaPlayer.LibVLCAudioPlayCb _processAudio;
        private MediaPlayer.LibVLCAudioSetupCb _audioSetup;
        private MediaPlayer.LibVLCAudioCleanupCb _cleanupAudio;
        private MediaPlayer.LibVLCAudioDrainCb _drainAudio;
        private MediaPlayer.LibVLCAudioFlushCb _flushAudio;
        private MediaPlayer.LibVLCAudioPauseCb _pauseAudio;
        private MediaPlayer.LibVLCAudioResumeCb _resumeAudio;

        private MediaPlayer.LibVLCVideoFormatCb _videoFormat;
        private MediaPlayer.LibVLCVideoLockCb _lockCB;
        private MediaPlayer.LibVLCVideoUnlockCb _unlockCB;
        private MediaPlayer.LibVLCVideoDisplayCb _displayCB;
        //private MediaPlayer.LibVLCVideoCleanupCb _cleanupVideoCB;
        
        private DateTime _lastFrame = DateTime.UtcNow;
        private ReasonToFinishPlaying _res = ReasonToFinishPlaying.DeviceLost;

        public VlcStream(CameraWindow source) : base(source)
        {
            _camera = source;
            Source = source.Camobject.settings.videosourcestring.Trim();
            _timeoutMilliSeconds = Math.Max(5000, source.Camobject.settings.timeout);
            _connectMilliSeconds = Math.Max(_timeoutMilliSeconds, 15000);
            _ignoreAudio = source.Camobject.settings.ignoreaudio;
            _options = source.Camobject.settings.vlcargs.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries).ToList();
            if (source.Camobject.settings.resize)
            {
                if (source.Camobject.settings.resizeWidth > 0 && source.Camobject.settings.resizeHeight > 0)
                {
                    _options.Add(":canvas-width=" + source.Camobject.settings.resizeWidth);
                    _options.Add(":canvas-height=" + source.Camobject.settings.resizeHeight);
                }
            }

            _options.Add(":avcodec-hw=none");
        }

        public VlcStream(VolumeLevel source) : base(null)
        {
            Source = source.Micobject.settings.sourcename;
            _timeoutMilliSeconds = Math.Max(5000, source.Micobject.settings.timeout);
            _connectMilliSeconds = Math.Max(_timeoutMilliSeconds, 10000);
            _options = source.Micobject.settings.vlcargs.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries).ToList();
            IsAudio = true;
        }

        public void Restart()
        {
            Debug.WriteLine("RESTART");
            if (IsRunning)
            {
                Stop(ReasonToFinishPlaying.Restart);
            }
            else
            {
                Start();
            }
        }

        public void Seek(float pc)
        {
            if (_mediaPlayer != null && _mediaPlayer.IsSeekable)
            {
                _mediaPlayer.Position = pc;
            }
        }
        public void Start()
        {
            if (_failedLoad || string.IsNullOrEmpty(VlcHelper.VLCLocation))
            {
                throw new ApplicationException("VLC not found. Set location in settings.");
            }

            if (IsRunning) return;
            IsRunning = true;
            Task.Run(DoStart);
        }

        private void DoStart() {

            if (_stopping) return;
            try
            {
                _connecting = true;
                Init();
            }
            catch (Exception ex)
            {
                Logger.LogException(ex, "VLCStream");
                ErrorHandler?.Invoke("Invalid Source (" + Source + ")");
                _res = ReasonToFinishPlaying.VideoSourceError;
                //Cleanup();
            }
        }

        public void Stop()
        {
            Stop(ReasonToFinishPlaying.StoppedByUser);
        }
        public void Stop(ReasonToFinishPlaying res)
        {

            if (_stopping)
                return;

            lock (this)
            {
                if (_stopping)
                    return;

                _stopping = true;
                _res = res;
                
                var mp = _mediaPlayer;
                if (mp!=null && mp?.NativeReference != IntPtr.Zero && mp?.IsPlaying == true)
                {
                    Debug.WriteLine("stop (" + DateTime.Now.Millisecond + ")");
                    mp?.Pause();
                    mp?.Stop();
                    _stopped.WaitOne(2000);
                }

            }
        }
        

        public void Tick()
        {
            if (!IsRunning)
                return;

            var ms = _connecting ? _connectMilliSeconds : _timeoutMilliSeconds;
            if ((DateTime.UtcNow - _lastTimeUpdate).TotalMilliseconds > ms)
            {
                Debug.WriteLine("TIMEOUT");
                _lastTimeUpdate = DateTime.MaxValue;
                Stop(ReasonToFinishPlaying.DeviceLost);
            }
        }

        #region Vlc audio callbacks
        private void FlushAudio(IntPtr data, long pts) { }
        private void CleanupAudio(IntPtr opaque) { }
        private void ResumeAudio(IntPtr data, long pts) { }
        private void PauseAudio(IntPtr data, long pts) { }
        private void DrainAudio(IntPtr data) { }

        private int AudioSetup(ref IntPtr opaque, ref IntPtr format, ref uint rate, ref uint channels)
        {
            Debug.WriteLine("AUDIO SETUP");
            channels = (uint)RecordingFormat.Channels;
            rate = (uint)RecordingFormat.SampleRate;
            return 0;

        }
        private void ProcessAudio(IntPtr data, IntPtr samples, uint count, long pts)
        {
            if (!IsRunning || _ignoreAudio) return;
            _lastTimeUpdate = DateTime.UtcNow;
            _connecting = false;
            var da = DataAvailable;
            int bytes = (int)count * 2;//(16 bit, 1 channel)
            if (HasAudioStream != null)
            {
                HasAudioStream?.Invoke(this, EventArgs.Empty);
                HasAudioStream = null;
            }
            if (da != null)
            {
                var buf = new byte[bytes];
                Marshal.Copy(samples, buf, 0, bytes);
                if (!_audioInited)
                {
                    _audioInited = true;
                    _waveProvider = new BufferedWaveProvider(RecordingFormat)
                    {
                        DiscardOnBufferOverflow = true,
                        BufferDuration = TimeSpan.FromMilliseconds(200)
                    };
                    _sampleChannel = new SampleChannel(_waveProvider);

                    _sampleChannel.PreVolumeMeter += SampleChannelPreVolumeMeter;
                }

                _waveProvider.AddSamples(buf, 0, bytes);

                var sampleBuffer = new float[bytes];
                var read = _sampleChannel.Read(sampleBuffer, 0, bytes);

                da(this, new DataAvailableEventArgs(buf, bytes));

                if (Listening) WaveOutProvider?.AddSamples(buf, 0, bytes);
            }
        }

        public BufferedWaveProvider WaveOutProvider { get; set; }

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
                    WaveOutProvider = new BufferedWaveProvider(RecordingFormat)
                    {
                        DiscardOnBufferOverflow = true,
                        BufferDuration =
                                              TimeSpan.FromMilliseconds(500)
                    };
                _listening = value;
            }
        }

        
        #endregion

        #region Vlc video callbacks
        private void DisplayVideo(IntPtr userdata, IntPtr picture)
        {
            if (!IsRunning || IsAudio) return;
            _lastFrame = DateTime.UtcNow;
            _connecting = false;
            if (ShouldEmitFrame)
            {
                var l = _size.Width * _size.Height * 4;
                GC.AddMemoryPressure(l);
                using (var mat = new Bitmap(_size.Width, _size.Height, _size.Width*4,
                                    PixelFormat.Format32bppArgb,userdata))
                {
                    var nfe = new NewFrameEventArgs(mat);
                    NewFrame.Invoke(this, nfe);
                }
                GC.RemoveMemoryPressure(l);
                if (Seekable)
                {
                    Time = _mediaPlayer.Time;
                    Duration = _mediaPlayer.Length;
                }
            }
        }
        private IntPtr LockVideo(IntPtr userdata, IntPtr planes)
        {
            Marshal.WriteIntPtr(planes, userdata);
            return userdata;
        }

        private void UnlockVideo(IntPtr opaque, IntPtr picture, IntPtr planes)
        {
        }

        private void CleanupVideo(ref IntPtr opaque)
        {

        }

        private uint GetAlignedDimension(uint dimension, uint mod)
        {
            var modResult = dimension % mod;
            if (modResult == 0)
            {
                return dimension;
            }

            return dimension + mod - (dimension % mod);
        }
        /// <summary>
        /// Converts a 4CC string representation to its UInt32 equivalent
        /// </summary>
        /// <param name="fourCCString">The 4CC string</param>
        /// <returns>The UInt32 representation of the 4cc</returns>
        static void ToFourCC(string fourCCString, IntPtr destination)
        {
            if (fourCCString.Length != 4)
            {
                throw new ArgumentException("4CC codes must be 4 characters long", nameof(fourCCString));
            }

            var bytes = Encoding.ASCII.GetBytes(fourCCString);

            for (var i = 0; i < 4; i++)
            {
                Marshal.WriteByte(destination, i, bytes[i]);
            }
        }

        /// <summary>
        /// Called by vlc when the video format is needed. This method allocats the picture buffers for vlc and tells it to set the chroma to RV32
        /// </summary>
        /// <param name="userdata">The user data that will be given to the <see cref="LockVideo"/> callback. It contains the pointer to the buffer</param>
        /// <param name="chroma">The chroma</param>
        /// <param name="width">The visible width</param>
        /// <param name="height">The visible height</param>
        /// <param name="pitches">The buffer width</param>
        /// <param name="lines">The buffer height</param>
        /// <returns>The number of buffers allocated</returns>
        private uint VideoFormat(ref IntPtr userdata, IntPtr chroma, ref uint width, ref uint height, ref uint pitches, ref uint lines)
        {
            Debug.WriteLine("VideoFormat");
            ToFourCC("RV32", chroma);

            //Correct video width and height according to TrackInfo
            _size = new Size((int)width, (int)height);
            var md = _mediaPlayer.Media;
            foreach (MediaTrack track in md.Tracks)
            {
                if (track.TrackType == TrackType.Video)
                {
                    var trackInfo = track.Data;
                    if (trackInfo.Video.Width > 0 && trackInfo.Video.Height > 0)
                    {
                        width = trackInfo.Video.Width;
                        height = trackInfo.Video.Height;
                        _size = new Size((int)width, (int)height);
                        if (trackInfo.Video.SarDen != 0)
                        {
                            width = width * trackInfo.Video.SarNum / trackInfo.Video.SarDen;
                        }
                    }

                    break;
                }
            }

            pitches = this.GetAlignedDimension((uint)(width * 32) / 8, 32);
            lines = this.GetAlignedDimension(height, 32);

            var b = new byte[width * height * 32];
            if (_imageData != null)
                _imageData?.Free();
            _imageData = GCHandle.Alloc(b, GCHandleType.Pinned);
            userdata = ((GCHandle)_imageData).AddrOfPinnedObject();
            return 1;
        }
        #endregion
        public void Dispose()
        {
            Dispose(true);
        }

        protected virtual void Dispose(bool disposing)
        {
            Debug.WriteLine("DISPOSE");
            if (_disposed)
                return;

            if (disposing)
            {
                _mediaPlayer?.Dispose();
                _libVLC?.Dispose();
                _imageData?.Free();
                _stopped?.Close();
            }

            // Free any unmanaged objects here. 
            //
            _disposed = true;
        }


        private void Init()
        {
            _mediaPlayer?.Dispose();

            _videoFormat = VideoFormat;
            _lockCB = LockVideo;
            _unlockCB = UnlockVideo;
            _displayCB = DisplayVideo;
            //_cleanupVideoCB = CleanupVideo;

            _audioSetup = AudioSetup;
            _processAudio = ProcessAudio;
            _cleanupAudio = CleanupAudio;
            _pauseAudio = PauseAudio;
            _resumeAudio = ResumeAudio;
            _flushAudio = FlushAudio;
            _drainAudio = DrainAudio;
            string overrideURL = null;

            if (_camera != null)
            {
                switch (_camera.Camobject.settings.sourceindex)
                {
                    case 9:
                        var od = _camera.ONVIFDevice;
                        if (od != null)
                        {
                            var ep = od.StreamEndpoint;
                            if (ep != null)
                            {
                                var u = ep.Uri.Uri;
                                overrideURL = u;
                            }
                        }
                        break;
                }
            }

            FromType ftype = FromType.FromLocation;
            Seekable = false;
           

            var murl = overrideURL ?? Source;
            if (string.IsNullOrEmpty(murl))
            {
                throw new Exception("Video source is empty");
            }

            try
            {
                var p = Path.GetFullPath(overrideURL ?? Source);
                Seekable = !string.IsNullOrEmpty(p);
                if (Seekable)
                    ftype = FromType.FromPath;
            }
            catch (Exception)
            {
                Seekable = false;
            }


            using (var media = new Media(LibVLC, murl, ftype))
            {

                
                Duration = Time = 0;

                foreach (var opt in _options)
                {
                    media.AddOption(opt);
                }

                _mediaPlayer = new MediaPlayer(media);
                _mediaPlayer.SetVideoFormatCallbacks(_videoFormat, null);//  _cleanupVideoCB);
                _mediaPlayer.SetVideoCallbacks(_lockCB, _unlockCB, _displayCB);
                _mediaPlayer.TimeChanged += _mediaPlayer_TimeChanged;
                _mediaPlayer.EnableHardwareDecoding = false;

                
                _mediaPlayer.SetAudioFormatCallback(_audioSetup, _cleanupAudio);
                _mediaPlayer.SetAudioCallbacks(_processAudio, _pauseAudio, _resumeAudio, _flushAudio, _drainAudio);


                _mediaPlayer.EncounteredError += (sender, e) =>
                {
                    ErrorHandler?.Invoke("VLC Error");
                    _res = ReasonToFinishPlaying.VideoSourceError;
                };

                _mediaPlayer.EndReached += (sender, e) =>
                {
                    _res = ReasonToFinishPlaying.VideoSourceError;
                };

                _mediaPlayer.Stopped += (sender, e) =>
                {
                    Logger.LogMessage("VLC stopped");
                    Task.Run(() =>
                    {
                        Debug.WriteLine("CLEANUP");
                        IsRunning = false;
                        _stopping = false;

                        
                        _stopped?.Set();

                        PlayingFinished?.Invoke(this, new PlayingFinishedEventArgs(_res));
                        AudioFinished?.Invoke(this, new PlayingFinishedEventArgs(_res));
                    });
                };
            }
            _lastTimeUpdate = DateTime.UtcNow;
            _mediaPlayer.Play();
        }

        private void _mediaPlayer_TimeChanged(object sender, MediaPlayerTimeChangedEventArgs e)
        {
            _lastTimeUpdate = DateTime.UtcNow;
        }

       
        private void SampleChannelPreVolumeMeter(object sender, StreamVolumeEventArgs e)
        {
            LevelChanged?.Invoke(this, new LevelChangedEventArgs(e.MaxSampleValues));
        }
    }
}
