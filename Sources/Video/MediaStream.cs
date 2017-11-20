using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using FFmpeg.AutoGen;
using iSpyApplication.Controls;
using iSpyApplication.Sources.Audio;
using iSpyApplication.Utilities;
using NAudio.Wave;
using NAudio.Wave.SampleProviders;

namespace iSpyApplication.Sources.Video
{
    internal unsafe class MediaStream : VideoBase, IVideoSource, IAudioSource, ISupportsAudio
    {
        [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public delegate int AvInterruptCb(void* ctx);

        public static WaveFormat OutFormat = new WaveFormat(22050, 16, 1);

        private readonly objectsMicrophone _audiosource;
        private readonly int _timeout;
        private readonly string _cookies = "";
        private readonly string _headers = "";


        private readonly AVInputFormat* _inputFormat;
        private readonly string _options = "";
        private readonly string _modeRTSP = "tcp";
        private readonly objectsCamera _source;

        private readonly string _userAgent = "";
        private bool _abort;
        private AVCodecContext* _audioCodecContext;
        private AVFrame* _audioFrame, _videoFrame;
        private AVIOInterruptCB_callback_func _aviocb;
        private AVCodecContext* _videoCodecContext;
        private bool _disposed;
        private AVFormatContext* _formatContext;
        private AvInterruptCb _interruptCallback;

        private IntPtr _interruptCallbackAddress;
        private DateTime _lastPacket;
        private DateTime _lastVideoFrame;

        private bool _listening;
        private ReasonToFinishPlaying _res = ReasonToFinishPlaying.DeviceLost;

        private volatile bool _starting;
        private SwrContext* _swrContext;
        private Thread _thread;
        private AVStream* _videoStream, _audioStream;

        public MediaStream(CameraWindow source) : base(source)
        {
            _source = source.Camobject;
            _inputFormat = null;
            IsAudio = false;

            _cookies = _source.settings.cookies;
            _timeout = Math.Max(3000, _source.settings.timeout);

            _userAgent = _source.settings.useragent;
            _headers = _source.settings.headers;
            _modeRTSP = Helper.RTSPMode(_source.settings.rtspmode);
        }

        public MediaStream(objectsMicrophone source) : base(null)
        {
            _audiosource = source;
            _inputFormat = null;
            IsAudio = true;
            _timeout = Math.Max(3000, source.settings.timeout);
            _options = source.settings.ffmpeg;
        }

        public bool IsAudio { get; }

        public event DataAvailableEventHandler DataAvailable;
        public event LevelChangedEventHandler LevelChanged;
        public event AudioFinishedEventHandler AudioFinished;

        public WaveFormat RecordingFormat { get; set; }

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
                {
                    WaveOutProvider = new BufferedWaveProvider(OutFormat)
                                      {
                                          DiscardOnBufferOverflow = true,
                                          BufferDuration =
                                              TimeSpan.FromMilliseconds(500)
                                      };
                }
                _listening = value;
            }
        }

        public event HasAudioStreamEventHandler HasAudioStream;

        public event NewFrameEventHandler NewFrame;
        public event PlayingFinishedEventHandler PlayingFinished;

        //public MediaStream(string format, objectsCamera source) : base(source)
        //{
        //    _source = source;
        //    _inputFormat = ffmpeg.av_find_input_format(format);
        //    if (_inputFormat == null)
        //    {
        //        throw new Exception("Can not find input format " + format);
        //    }
        //}

        public string Source
        {
            get
            {
                if (IsAudio)
                    return _audiosource.settings.sourcename;

                return _source.settings.videosourcestring;
            }
        }


        public void Start()
        {
            if (_starting || IsRunning) return;
            _abort = false;
            _res = ReasonToFinishPlaying.DeviceLost;
            _starting = true;
            Task.Factory.StartNew(DoStart);
        }


        public void Restart()
        {
            if (!IsRunning) return;
            _res = ReasonToFinishPlaying.Restart;
            _abort = true;
        }

        public void Stop()
        {
            if (!IsRunning) return;
            _res = ReasonToFinishPlaying.StoppedByUser;
            _abort = true;
        }

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

        // Public implementation of Dispose pattern callable by consumers. 
        public void Dispose()
        {
            Dispose(true);
        }

        public event Delegates.ErrorHandler ErrorHandler;

        public int InterruptCb(void* ctx)
        {
            if ((DateTime.UtcNow - _lastPacket).TotalMilliseconds > _timeout || _abort)
            {
                if (!_abort)
                {
                    _res = ReasonToFinishPlaying.DeviceLost;
                }
                _abort = true;
                return 1;
            }
            return 0;
        }

        public void Close()
        {
            Stop();
        }

        private void DoStart()
        {
            var vss = Source;
            if (!IsAudio)
                vss = Tokenise(vss);

            AVDictionary* options = null;
            if (_inputFormat == null)
            {
                var prefix = vss.ToLower().Substring(0, vss.IndexOf(":", StringComparison.Ordinal));
                switch (prefix)
                {
                    case "https":
                    case "http":
                    case "mmsh":
                    case "mms":
                        ffmpeg.av_dict_set_int(&options, "timeout", _timeout, 0);
                        ffmpeg.av_dict_set_int(&options, "stimeout", _timeout * 1000, 0);
                        if (_cookies != "")
                        {
                            ffmpeg.av_dict_set(&options, "cookies", _cookies, 0);
                        }

                        if (_headers != "")
                        {
                            ffmpeg.av_dict_set(&options, "headers", _headers, 0);
                        }
                        if (_userAgent != "")
                        {
                            ffmpeg.av_dict_set(&options, "user-agent", _userAgent, 0);
                        }
                        break;
                    default:
                        ffmpeg.av_dict_set_int(&options, "timeout", _timeout, 0);
                        break;
                    case "rtsp":
                    case "rtmp":
                        ffmpeg.av_dict_set_int(&options, "stimeout", _timeout * 1000, 0);
                        if (_userAgent != "")
                        {
                            ffmpeg.av_dict_set(&options, "user-agent", _userAgent, 0);
                        }
                        break;
                }

                ffmpeg.av_dict_set(&options, "rtsp_transport", _modeRTSP, 0);
            }

            ffmpeg.av_dict_set_int(&options, "rtbufsize", 10000000, 0);

            var lo = _options.Split(Environment.NewLine.ToCharArray());
            foreach (var nv in lo)
            {
                if (!string.IsNullOrEmpty(nv))
                {
                    var i = nv.IndexOf('=');
                    if (i > -1)
                    {
                        var n = nv.Substring(0, i).Trim();
                        var v = nv.Substring(i + 1).Trim();
                        if (!string.IsNullOrEmpty(n) && !string.IsNullOrEmpty(v))
                        {
                            int j;
                            if (int.TryParse(v, out j))
                            {
                                ffmpeg.av_dict_set_int(&options, n, j, 0);
                            }
                            else
                            {
                                ffmpeg.av_dict_set(&options, n, v, 0);
                            }
                        }
                    }
                }
            }


            _abort = false;
            try
            {
                Program.MutexHelper.Wait();
                var pFormatContext = ffmpeg.avformat_alloc_context();
                _lastPacket = DateTime.UtcNow;


                _interruptCallback = InterruptCb;
                _interruptCallbackAddress = Marshal.GetFunctionPointerForDelegate(_interruptCallback);

                _aviocb = new AVIOInterruptCB_callback_func
                          {
                              Pointer = _interruptCallbackAddress
                          };
                pFormatContext->interrupt_callback.callback = _aviocb;
                pFormatContext->interrupt_callback.opaque = null;
                pFormatContext->max_analyze_duration = 0;

                Throw("OPEN_INPUT", ffmpeg.avformat_open_input(&pFormatContext, vss, _inputFormat, &options));               
                _formatContext = pFormatContext;


                SetupFormat();
            }
            catch (Exception ex)
            {
                ErrorHandler?.Invoke(ex.Message);
                _res = ReasonToFinishPlaying.VideoSourceError;
                PlayingFinished?.Invoke(this, new PlayingFinishedEventArgs(_res));
                AudioFinished?.Invoke(this, new PlayingFinishedEventArgs(_res));
            }
            finally
            {
                try
                {
                    Program.MutexHelper.Release();
                }
                catch
                {
                }
            }
            _starting = false;
        }

        private void SetupFormat()
        {
            if (ffmpeg.avformat_find_stream_info(_formatContext, null) != 0)
            {
                throw new ApplicationException(@"Could not find stream info");
            }


            _formatContext->flags |= ffmpeg.AVFMT_FLAG_DISCARD_CORRUPT;
            _formatContext->flags |= ffmpeg.AVFMT_FLAG_NOBUFFER;

            for (var i = 0; i < _formatContext->nb_streams; i++)
            {
                if (_formatContext->streams[i]->codec->codec_type == AVMediaType.AVMEDIA_TYPE_VIDEO)
                {
                    // get the pointer to the codec context for the video stream
                    _videoCodecContext = _formatContext->streams[i]->codec;
                    _videoCodecContext->workaround_bugs = 1;
                    _videoStream = _formatContext->streams[i];
                    break;
                }
            }

            if (_videoStream != null)
            {
                _videoCodecContext->flags2 |= ffmpeg.CODEC_FLAG2_FAST | ffmpeg.CODEC_FLAG_LOW_DELAY;

                var codec = ffmpeg.avcodec_find_decoder(_videoCodecContext->codec_id);
                if (codec == null)
                {
                    throw new ApplicationException("Cannot find a codec to decode the video stream.");
                }
                _videoCodecContext->refcounted_frames = 1;

                if ((codec->capabilities & ffmpeg.AV_CODEC_CAP_TRUNCATED) == ffmpeg.AV_CODEC_CAP_TRUNCATED)
                {
                    _videoCodecContext->flags |= ffmpeg.AV_CODEC_FLAG_TRUNCATED;
                }

                _lastVideoFrame = DateTime.UtcNow;

                AVHWAccel* hwaccel = null;

                while (true)
                {
                    hwaccel = ffmpeg.av_hwaccel_next(hwaccel);
                    if (hwaccel == null)
                        break;
                    if (hwaccel->id == _videoCodecContext->codec_id && hwaccel->pix_fmt == _videoCodecContext->pix_fmt)
                    {
                        Logger.LogMessage("Using HW decoder");
                        _videoCodecContext->hwaccel = hwaccel;
                        break;
                    }
                }

                Throw("OPEN2", ffmpeg.avcodec_open2(_videoCodecContext, codec, null));

                _videoFrame = ffmpeg.av_frame_alloc();
            }

            _lastPacket = DateTime.UtcNow;

            for (var i = 0; i < _formatContext->nb_streams; i++)
            {
                if (_formatContext->streams[i]->codec->codec_type == AVMediaType.AVMEDIA_TYPE_AUDIO)
                {
                    _audioCodecContext = _formatContext->streams[i]->codec;
                    _audioStream = _formatContext->streams[i];
                    break;
                }
            }

            if (_audioStream != null)
            {
                var audiocodec = ffmpeg.avcodec_find_decoder(_audioCodecContext->codec_id);
                if (audiocodec != null)
                {
                    _audioCodecContext->refcounted_frames = 1;

                    Throw("OPEN2 audio", ffmpeg.avcodec_open2(_audioCodecContext, audiocodec, null));

                    var outlayout = ffmpeg.av_get_default_channel_layout(OutFormat.Channels);
                    _audioCodecContext->request_sample_fmt = AVSampleFormat.AV_SAMPLE_FMT_S16;
                    _audioCodecContext->request_channel_layout = (ulong)outlayout;


                    //var chans = 1;
                    //if (_audioCodecContext->channels > 1) //downmix
                    //    chans = 2;

                    _swrContext = ffmpeg.swr_alloc_set_opts(null,
                        outlayout,
                        AVSampleFormat.AV_SAMPLE_FMT_S16,
                        OutFormat.SampleRate,
                        ffmpeg.av_get_default_channel_layout(_audioCodecContext->channels),
                        _audioCodecContext->sample_fmt,
                        _audioCodecContext->sample_rate,
                        0,
                        null);

                    Throw("SWR_INIT", ffmpeg.swr_init(_swrContext));
                    _audioFrame = ffmpeg.av_frame_alloc();
                }
            }

            if (_videoStream == null && _audioStream == null)
            {
                throw new ApplicationException("Cannot find any streams.");
            }

            _lastPacket = DateTime.UtcNow;

            _thread = new Thread(ReadFrames) {Name = Source, IsBackground = true};
            _thread.Start();
        }


        private void ReadFrames()
        {
            var pConvertedFrameBuffer = IntPtr.Zero;
            SwsContext* pConvertContext = null;

            var audioInited = false;
            var videoInited = false;
            byte[] buffer = null, tbuffer = null;
            var dstData = new byte_ptrArray4();
            var dstLinesize = new int_array4();
            BufferedWaveProvider waveProvider = null;
            SampleChannel sampleChannel = null;
            var packet = new AVPacket();

            do
            {
                ffmpeg.av_init_packet(&packet);
                if (_audioCodecContext != null && buffer == null)
                {
                    buffer = new byte[_audioCodecContext->sample_rate*2];
                    tbuffer = new byte[_audioCodecContext->sample_rate*2];
                }

                if (Log("AV_READ_FRAME", ffmpeg.av_read_frame(_formatContext, &packet)))
                    break;


                if ((packet.flags & ffmpeg.AV_PKT_FLAG_CORRUPT) == ffmpeg.AV_PKT_FLAG_CORRUPT)
                {
                    break;
                }

                var nf = NewFrame;
                var da = DataAvailable;

                _lastPacket = DateTime.UtcNow;

                int ret;
                if (_audioStream != null && packet.stream_index == _audioStream->index && _audioCodecContext!=null)
                {
                    if (HasAudioStream != null)
                    {
                        HasAudioStream?.Invoke(this, EventArgs.Empty);
                        HasAudioStream = null;
                    }
                    if (da != null)
                    {
                        var s = 0;
                        fixed (byte** outPtrs = new byte*[32])
                        {
                            fixed (byte* bPtr = &tbuffer[0])
                            {
                                outPtrs[0] = bPtr;
                                ffmpeg.avcodec_send_packet(_audioCodecContext, &packet);
                                do
                                {
                                    ret = ffmpeg.avcodec_receive_frame(_audioCodecContext, _audioFrame);
                                    if (ret == 0)
                                    {
                                        var dat = _audioFrame->data[0];
                                        var numSamplesOut = ffmpeg.swr_convert(_swrContext,
                                            outPtrs,
                                            _audioCodecContext->sample_rate,
                                            &dat,
                                            _audioFrame->nb_samples);

                                        var l = numSamplesOut*2*_audioCodecContext->channels;
                                        Buffer.BlockCopy(tbuffer, 0, buffer, s, l);
                                        s += l;
                                    }

                                    if (_audioFrame->decode_error_flags > 0)
                                    {
                                        break;
                                    }
                                } while (ret == 0);

                                if (s > 0)
                                {
                                    var ba = new byte[s];
                                    Buffer.BlockCopy(buffer, 0, ba, 0, s);

                                    if (!audioInited)
                                    {
                                        audioInited = true;
                                        RecordingFormat = new WaveFormat(_audioCodecContext->sample_rate, 16,
                                            _audioCodecContext->channels);

                                        waveProvider = new BufferedWaveProvider(RecordingFormat)
                                                       {
                                                           DiscardOnBufferOverflow = true,
                                                           BufferDuration = TimeSpan.FromMilliseconds(200)
                                                       };
                                        sampleChannel = new SampleChannel(waveProvider);

                                        sampleChannel.PreVolumeMeter += SampleChannelPreVolumeMeter;
                                    }


                                    waveProvider.AddSamples(ba, 0, s);

                                    var sampleBuffer = new float[s];
                                    var read = sampleChannel.Read(sampleBuffer, 0, s);


                                    da(this, new DataAvailableEventArgs(ba, read));


                                    if (Listening)
                                    {
                                        WaveOutProvider?.AddSamples(ba, 0, read);
                                    }
                                }
                            }
                        }
                    }
                }

                if (nf != null && _videoStream != null && packet.stream_index == _videoStream->index && _videoCodecContext != null)
                {
                    ffmpeg.avcodec_send_packet(_videoCodecContext, &packet);
                    do
                    {
                        ret = ffmpeg.avcodec_receive_frame(_videoCodecContext, _videoFrame);
                        if (ret == 0 && EmitFrame)
                        {
                            if (!videoInited)
                            {
                                videoInited = true;
                                var convertedFrameBufferSize =
                                    ffmpeg.av_image_get_buffer_size(AVPixelFormat.AV_PIX_FMT_BGR24, _videoCodecContext->width,
                                        _videoCodecContext->height, 1);

                                pConvertedFrameBuffer = Marshal.AllocHGlobal(convertedFrameBufferSize);

                                ffmpeg.av_image_fill_arrays(ref dstData, ref dstLinesize, (byte*) pConvertedFrameBuffer,
                                    AVPixelFormat.AV_PIX_FMT_BGR24, _videoCodecContext->width, _videoCodecContext->height, 1);


                                pConvertContext = ffmpeg.sws_getContext(_videoCodecContext->width, _videoCodecContext->height,
                                    _videoCodecContext->pix_fmt, _videoCodecContext->width, _videoCodecContext->height,
                                    AVPixelFormat.AV_PIX_FMT_BGR24, ffmpeg.SWS_FAST_BILINEAR, null, null, null);
                            }

                            Log("SWS_SCALE",
                                ffmpeg.sws_scale(pConvertContext, _videoFrame->data, _videoFrame->linesize, 0,
                                    _videoCodecContext->height, dstData, dstLinesize));

                            if (_videoFrame->decode_error_flags > 0)
                            {
                                break;
                            }

                            using (
                                var mat = new Bitmap(_videoCodecContext->width, _videoCodecContext->height, dstLinesize[0],
                                    PixelFormat.Format24bppRgb, pConvertedFrameBuffer))
                            {
                                var nfe = new NewFrameEventArgs(mat);
                                nf.Invoke(this, nfe);
                            }

                            _lastVideoFrame = DateTime.UtcNow;
                        }
                    } while (ret == 0);
                }

                if (nf != null && _videoStream != null)
                {
                    if ((DateTime.UtcNow - _lastVideoFrame).TotalMilliseconds > _timeout)
                    {
                        _res = ReasonToFinishPlaying.DeviceLost;
                        _abort = true;
                    }
                }

                ffmpeg.av_packet_unref(&packet);
            } while (!_abort && !MainForm.ShuttingDown);

            NewFrame?.Invoke(this, new NewFrameEventArgs(null));

            try
            {
                Program.MutexHelper.Wait();

                if (pConvertedFrameBuffer != IntPtr.Zero)
                    Marshal.FreeHGlobal(pConvertedFrameBuffer);

                if (_formatContext != null)
                {
                    if (_formatContext->streams != null)
                    {
                        var j = (int) _formatContext->nb_streams;
                        for (var i = j - 1; i >= 0; i--)
                        {
                            var stream = _formatContext->streams[i];

                            if (stream != null && stream->codec != null && stream->codec->codec != null)
                            {
                                stream->discard = AVDiscard.AVDISCARD_ALL;
                                ffmpeg.avcodec_close(stream->codec);
                            }
                        }
                    }
                    fixed (AVFormatContext** f = &_formatContext)
                    {
                        ffmpeg.avformat_close_input(f);
                    }
                    _formatContext = null;
                }

                if (_videoFrame != null)
                {
                    fixed (AVFrame** pinprt = &_videoFrame)
                    {
                        ffmpeg.av_frame_free(pinprt);
                        _videoFrame = null;
                    }
                }

                if (_audioFrame != null)
                {
                    fixed (AVFrame** pinprt = &_audioFrame)
                    {
                        ffmpeg.av_frame_free(pinprt);
                        _audioFrame = null;
                    }
                }

                _videoStream = null;
                _audioStream = null;
                _audioCodecContext = null;
                _videoCodecContext = null;

                if (_swrContext != null)
                {
                    fixed (SwrContext** s = &_swrContext)
                    {
                        ffmpeg.swr_free(s);
                    }
                    _swrContext = null;
                }

                if (pConvertContext != null)
                {
                    ffmpeg.sws_freeContext(pConvertContext);
                }

                if (sampleChannel != null)
                {
                    sampleChannel.PreVolumeMeter -= SampleChannelPreVolumeMeter;
                    sampleChannel = null;
                }
            }
            catch (Exception ex)
            {
                Logger.LogException(ex, "Media Stream (close)");
            }
            finally
            {
                try
                {
                    Program.MutexHelper.Release();
                }
                catch
                {
                }
            }

            PlayingFinished?.Invoke(this, new PlayingFinishedEventArgs(_res));
            AudioFinished?.Invoke(this, new PlayingFinishedEventArgs(_res));

        }

        private void SampleChannelPreVolumeMeter(object sender, StreamVolumeEventArgs e)
        {
            LevelChanged?.Invoke(this, new LevelChangedEventArgs(e.MaxSampleValues));
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