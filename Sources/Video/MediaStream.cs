using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using FFmpeg.AutoGen;
using iSpyApplication.Controls;
using iSpyApplication.Sources.Audio;
using iSpyApplication.Utilities;
using NAudio.Wave;
using NAudio.Wave.SampleProviders;
using System.Runtime.ExceptionServices;

namespace iSpyApplication.Sources.Video
{
    internal unsafe class MediaStream : VideoBase, IVideoSource, IAudioSource, ISupportsAudio
    {
        [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public delegate int AvInterruptCb(void* ctx);

        private const int BUFSIZE = 2000000;


        private const int AV_CODEC_HW_CONFIG_METHOD_HW_DEVICE_CTX = 0x01;
        private const int CODEC_FLAG_EMU_EDGE = 16384;

        public static WaveFormat OutFormat = new WaveFormat(22050, 16, 1);
        private readonly objectsMicrophone _audiosource;
        private readonly string _cookies = "";
        private readonly string _headers = "";
        private Size _finalSize;

        private readonly AVInputFormat* _inputFormat;
        private readonly string _modeRTSP = "udp";
        private readonly string _options = "";
        private readonly objectsCamera _source;

        private readonly string _userAgent = "";
        private bool _abort;
        private AVCodecContext* _audioCodecContext;
        private AVIOInterruptCB_callback_func _aviocb;
        private AvInterruptCb _interruptCallback;

        private bool _disposed;
        private AVFormatContext* _formatContext;

        private readonly bool _useGPU = false;
        private AVPixelFormat _hwPixFmt;
        private AVBufferRef* _hwDeviceCtx;
        private AVCodecContext_get_format _getFormatCallback;

        private IntPtr _interruptCallbackAddress;
        private DateTime _lastPacket;
        private DateTime _lastVideoFrame;

        private bool _listening;
        private ReasonToFinishPlaying _res = ReasonToFinishPlaying.DeviceLost;

        private volatile bool _starting;
        private SwrContext* _swrContext;
        private Thread _thread;
        private int _timeoutMicroSeconds;
        private bool _ignoreAudio;
        private AVCodecContext* _videoCodecContext;
        private AVStream* _videoStream, _audioStream;

        
        private SwsContext* pConvertContext = null;

        private IntPtr pConvertedFrameBuffer = IntPtr.Zero;
        private SampleChannel sampleChannel;

        public MediaStream(CameraWindow source) : base(source)
        {
            _source = source.Camobject;
            _inputFormat = null;
            IsAudio = false;

            _cookies = _source.settings.cookies;
            _timeoutMicroSeconds = Math.Max(5000000, _source.settings.timeout * 1000);

            _userAgent = _source.settings.useragent;
            _headers = _source.settings.headers;
            _modeRTSP = Helper.RTSPMode(_source.settings.rtspmode);
            _useGPU = _source.settings.useGPU;
            _ignoreAudio = _source.settings.ignoreaudio;
        }

        public MediaStream(objectsMicrophone source) : base(null)
        {
            _audiosource = source;
            _inputFormat = null;
            IsAudio = true;
            _timeoutMicroSeconds = Math.Max(5000000, source.settings.timeout * 1000);
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
                    WaveOutProvider = new BufferedWaveProvider(OutFormat)
                                      {
                                          DiscardOnBufferOverflow = true,
                                          BufferDuration =
                                              TimeSpan.FromMilliseconds(500)
                                      };
                _listening = value;
            }
        }

        public event HasAudioStreamEventHandler HasAudioStream;

        public event NewFrameEventHandler NewFrame;
        public event PlayingFinishedEventHandler PlayingFinished;

        public string Source
        {
            get
            {
                if (IsAudio)
                    return _audiosource.settings.sourcename;

                return _cw.Source;
            }
        }

        public string SourceName
        {
            get
            {
                if (IsAudio)
                    return _audiosource.name;
                return _source.name;
            }
        }


        public void Start()
        {
            if (_starting || IsRunning) return;
            _starting = true;
            _abort = false;
            _res = ReasonToFinishPlaying.DeviceLost;

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
            //don't check abort here as breaks teardown of rtsp streams
            if ((DateTime.UtcNow - _lastPacket).TotalMilliseconds * 1000 > _timeoutMicroSeconds)
            {
                if (!_abort) _res = ReasonToFinishPlaying.DeviceLost;
                _abort = true;
                return 1;
            }

            return 0;
        }

        public AVPixelFormat GetPixelFormat(AVCodecContext* ctx, AVPixelFormat* pix_fmts)
        {
            for (var p = pix_fmts; *p != AVPixelFormat.AV_PIX_FMT_NONE; p++)
            {
                if (*pix_fmts == _hwPixFmt)
                {
                    return _hwPixFmt;
                }
                pix_fmts++;
            }
            return _videoCodecContext->pix_fmt;
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

            if (string.IsNullOrEmpty(vss))
            {
                ErrorHandler?.Invoke("Source not found");
                _res = ReasonToFinishPlaying.VideoSourceError;
                CleanUp();
                _starting = false;
                return;
            }

            AVDictionary* options = null;
            if (_inputFormat == null)
            {
                var prefix = vss.ToLower().Substring(0, vss.IndexOf(":", StringComparison.Ordinal));
                ffmpeg.av_dict_set_int(&options, "rw_timeout", _timeoutMicroSeconds, 0);
                ffmpeg.av_dict_set_int(&options, "tcp_nodelay", 1, 0);
                switch (prefix)
                {
                    case "https":
                    case "http":
                    case "mmsh":
                    case "mms":
                        ffmpeg.av_dict_set_int(&options, "timeout", _timeoutMicroSeconds, 0);
                        ffmpeg.av_dict_set_int(&options, "stimeout", _timeoutMicroSeconds, 0);

                        if (!string.IsNullOrEmpty(_cookies)) ffmpeg.av_dict_set(&options, "cookies", _cookies, 0);
                        if (!string.IsNullOrEmpty(_headers)) ffmpeg.av_dict_set(&options, "headers", _headers, 0);
                        if (!string.IsNullOrEmpty(_userAgent))
                            ffmpeg.av_dict_set(&options, "user_agent", _userAgent, 0);
                        break;
                    case "ws":
                    case "rtsp":
                    case "rtmp":
                        ffmpeg.av_dict_set_int(&options, "stimeout", _timeoutMicroSeconds, 0);
                        if (!string.IsNullOrEmpty(_userAgent))
                            ffmpeg.av_dict_set(&options, "user_agent", _userAgent, 0);
                        if (!string.IsNullOrEmpty(_modeRTSP))
                        {
                            ffmpeg.av_dict_set(&options, "rtsp_transport", _modeRTSP, 0);
                        }
                        ffmpeg.av_dict_set(&options, "rtsp_flags", "prefer_tcp", 0);
                        break;
                    default:
                        ffmpeg.av_dict_set_int(&options, "timeout", _timeoutMicroSeconds, 0);
                        break;

                    case "tcp":
                        ffmpeg.av_dict_set_int(&options, "timeout", _timeoutMicroSeconds, 0);
                        break;
                    case "udp":
                        ffmpeg.av_dict_set_int(&options, "timeout", _timeoutMicroSeconds, 0);
                        break;
                }
                ffmpeg.av_dict_set_int(&options, "buffer_size", BUFSIZE, 0);
            }
            //ffmpeg.av_dict_set_int(&options, "rtbufsize", BUFSIZE, 0);

            var lo = _options.Split(Environment.NewLine.ToCharArray());
            foreach (var nv in lo)
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
                                ffmpeg.av_dict_set_int(&options, n, j, 0);
                            else
                                ffmpeg.av_dict_set(&options, n, v, 0);
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
                pFormatContext->max_analyze_duration = 0; //0 = auto

                var t = _timeoutMicroSeconds;
                _timeoutMicroSeconds = Math.Max(_timeoutMicroSeconds, 15000000);

                Throw("OPEN_INPUT", ffmpeg.avformat_open_input(&pFormatContext, vss, _inputFormat, &options));
                _formatContext = pFormatContext;

                SetupFormat();

                _timeoutMicroSeconds = t;
            }
            catch (Exception ex)
            {
                ErrorHandler?.Invoke(ex.Message);
                _res = ReasonToFinishPlaying.VideoSourceError;
                CleanUp();
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

        private void LogMessage(string msg)
        {
            Logger.LogMessage(SourceName + ": "+msg);
        }

        private void SetupHardwareDecoding(AVCodec* codec)
        {
            AVHWDeviceType hwtype;

            for (int i = 0; ; i++)
            {
                AVCodecHWConfig* config = ffmpeg.avcodec_get_hw_config(codec, i);
                if (config == null)
                {
                    LogMessage("Hardware decoder not supported for this codec.");
                    return;
                }

                if ((config->methods & AV_CODEC_HW_CONFIG_METHOD_HW_DEVICE_CTX) == AV_CODEC_HW_CONFIG_METHOD_HW_DEVICE_CTX)
                {
                    _hwPixFmt = config->pix_fmt;
                    hwtype = config->device_type;
                    break;
                }
            }

            ffmpeg.avcodec_parameters_to_context(_videoCodecContext, _videoStream->codecpar);
            _getFormatCallback = GetPixelFormat;
            _videoCodecContext->get_format = _getFormatCallback;

            AVBufferRef* hwDeviceCtx = null;
            if (ffmpeg.av_hwdevice_ctx_create(&hwDeviceCtx, hwtype, null, null, 0) < 0)
            {
                LogMessage("Failed to create specified HW device.");
                _hwDeviceCtx = null;
                return;
            }

            _videoCodecContext->hw_device_ctx = ffmpeg.av_buffer_ref(hwDeviceCtx);
            _hwDeviceCtx = hwDeviceCtx;
            LogMessage("Using hardware decoder: " + hwtype);
        }

        private void SetupFormat()
        {
            if (ffmpeg.avformat_find_stream_info(_formatContext, null) != 0)
                throw new ApplicationException("Could not find stream info");


            _formatContext->flags |= ffmpeg.AVFMT_FLAG_DISCARD_CORRUPT;
            _formatContext->flags |= ffmpeg.AVFMT_FLAG_NOBUFFER;


            for (var i = 0; i < _formatContext->nb_streams; i++)
                if (_formatContext->streams[i]->codec->codec_type == AVMediaType.AVMEDIA_TYPE_VIDEO)
                {
                    // get the pointer to the codec context for the video stream
                    _videoCodecContext = _formatContext->streams[i]->codec;
                    _videoStream = _formatContext->streams[i];
                    break;
                }

            if (_videoStream != null)
            {
                var codec = ffmpeg.avcodec_find_decoder(_videoCodecContext->codec_id);

                if (codec == null) throw new ApplicationException("Cannot find a codec to decode the video stream.");

                _lastVideoFrame = DateTime.UtcNow;

                ffmpeg.av_opt_set_int(_videoCodecContext, "refcounted_frames", 1, 0);
                if (_useGPU)
                {
                    SetupHardwareDecoding(codec);
                }

                _videoCodecContext->workaround_bugs = 1;
                _videoCodecContext->flags2 |= ffmpeg.AV_CODEC_FLAG2_FAST | ffmpeg.AV_CODEC_FLAG_LOW_DELAY;// | ffmpeg.AV_CODEC_FLAG2_CHUNKS;

                if ((codec->capabilities & ffmpeg.AV_CODEC_CAP_DR1) != 0)
                    _videoCodecContext->flags |= CODEC_FLAG_EMU_EDGE;
                if ((codec->capabilities & ffmpeg.AV_CODEC_CAP_TRUNCATED) == ffmpeg.AV_CODEC_CAP_TRUNCATED)
                    _videoCodecContext->flags |= ffmpeg.AV_CODEC_FLAG_TRUNCATED;


                Throw("OPEN2", ffmpeg.avcodec_open2(_videoCodecContext, codec, null));
            }

            _lastPacket = DateTime.UtcNow;

            for (var i = 0; i < _formatContext->nb_streams; i++)
                if (_formatContext->streams[i]->codec->codec_type == AVMediaType.AVMEDIA_TYPE_AUDIO)
                {
                    _audioCodecContext = _formatContext->streams[i]->codec;
                    _audioStream = _formatContext->streams[i];
                    break;
                }

            if (_audioStream != null)
            {
                var audiocodec = ffmpeg.avcodec_find_decoder(_audioCodecContext->codec_id);
                if (audiocodec != null)
                {
                    ffmpeg.av_opt_set_int(_audioCodecContext, "refcounted_frames", 1, 0);
                    Throw("OPEN2 audio", ffmpeg.avcodec_open2(_audioCodecContext, audiocodec, null));

                    var outlayout = ffmpeg.av_get_default_channel_layout(OutFormat.Channels);
                    _audioCodecContext->request_sample_fmt = AVSampleFormat.AV_SAMPLE_FMT_S16;
                    _audioCodecContext->request_channel_layout = (ulong) outlayout;

                }
            }

            if (_videoStream == null && _audioStream == null)
                throw new ApplicationException("Cannot find any streams.");

            _lastPacket = DateTime.UtcNow;
            if (_abort) throw new Exception("Connect aborted");

            _thread = new Thread(ReadFrames) {Name = Source, IsBackground = false};
            _thread.Start();
        }

        private void initSWR()
        {
            _swrContext = ffmpeg.swr_alloc_set_opts(null,
                        ffmpeg.av_get_default_channel_layout(OutFormat.Channels),
                        AVSampleFormat.AV_SAMPLE_FMT_S16,
                        OutFormat.SampleRate,
                        ffmpeg.av_get_default_channel_layout(_audioCodecContext->channels),
                        _audioCodecContext->sample_fmt,
                        _audioCodecContext->sample_rate,
                        0,
                        null);

            Throw("SWR_INIT", ffmpeg.swr_init(_swrContext));
        }

        [HandleProcessCorruptedStateExceptions]
        private void ReadFrames()
        {
            pConvertedFrameBuffer = IntPtr.Zero;
            pConvertContext = null;

            var audioInited = false;
            var videoInited = false;
            byte[] buffer = null, tbuffer = null;
            var dstData = new byte_ptrArray4();
            var dstLinesize = new int_array4();
            BufferedWaveProvider waveProvider = null;
            sampleChannel = null;
            var packet = new AVPacket();

            do
            {
                ffmpeg.av_init_packet(&packet);
                if (_audioCodecContext != null && buffer == null)
                {
                    buffer = new byte[_audioCodecContext->sample_rate * 2];
                    tbuffer = new byte[_audioCodecContext->sample_rate * 2];
                }

                if (Log("AV_READ_FRAME", ffmpeg.av_read_frame(_formatContext, &packet))) break;


                if ((packet.flags & ffmpeg.AV_PKT_FLAG_CORRUPT) == ffmpeg.AV_PKT_FLAG_CORRUPT) break;

                var nf = NewFrame;
                var da = DataAvailable;

                _lastPacket = DateTime.UtcNow;

                var ret = -11; //EAGAIN
                if (_audioStream != null && packet.stream_index == _audioStream->index && _audioCodecContext != null && !_ignoreAudio)
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
                                var af = ffmpeg.av_frame_alloc();
                                ffmpeg.avcodec_send_packet(_audioCodecContext, &packet);
                                do
                                {
                                    ret = ffmpeg.avcodec_receive_frame(_audioCodecContext, af);
                                    
                                    if (ret == 0)
                                    {
                                        int numSamplesOut = 0;
                                        try
                                        {
                                            if (_swrContext == null)
                                            {
                                                //need to do this here as send_packet can change channel layout and throw an exception below
                                                initSWR();
                                            }
                                            var dat = af->data[0];
                                        
                                            numSamplesOut = ffmpeg.swr_convert(_swrContext,
                                                outPtrs,
                                                _audioCodecContext->sample_rate,
                                                &dat,
                                                af->nb_samples);
                                        }
                                        catch (Exception ex)
                                        {
                                            Logger.LogException(ex, "MediaStream - Audio Read");
                                            _ignoreAudio = true;
                                            break;
                                        }

                                        if (numSamplesOut > 0)
                                        {
                                            var l = numSamplesOut * 2 * OutFormat.Channels;
                                            Buffer.BlockCopy(tbuffer, 0, buffer, s, l);
                                            s += l;
                                        }
                                        else
                                        {
                                            ret = numSamplesOut; //(error)
                                        }
                                        
                                    }
                                    if (af->decode_error_flags > 0) break;
                                } while (ret == 0);
                                ffmpeg.av_frame_free(&af);
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


                                    da(this, new DataAvailableEventArgs(ba, s));


                                    if (Listening) WaveOutProvider?.AddSamples(ba, 0, read);
                                }
                            }
                        }
                    }
                }

                if (nf != null && _videoStream != null && packet.stream_index == _videoStream->index &&
                    _videoCodecContext != null)
                {
                    
                    var ef = ShouldEmitFrame;
                    ffmpeg.avcodec_send_packet(_videoCodecContext, &packet);
                    do
                    {
                        var vf = ffmpeg.av_frame_alloc();
                        ret = ffmpeg.avcodec_receive_frame(_videoCodecContext, vf);
                        if (ret == 0 && ef)
                        {
                            AVPixelFormat srcFmt;
                            if (_hwDeviceCtx != null)
                            {
                                srcFmt = AVPixelFormat.AV_PIX_FMT_NV12;
                                var output = ffmpeg.av_frame_alloc();
                                ffmpeg.av_hwframe_transfer_data(output, vf, 0);
                                ffmpeg.av_frame_copy_props(output, vf);
                                ffmpeg.av_frame_free(&vf);
                                vf = output;
                            }
                            else
                            {
                                srcFmt = (AVPixelFormat)vf->format;
                            }

                            if (!videoInited)
                            {
                                videoInited = true;

                                _finalSize = Helper.CalcResizeSize(_source.settings.resize, new Size(_videoCodecContext->width, _videoCodecContext->height), new Size(_source.settings.resizeWidth, _source.settings.resizeHeight));

                                var convertedFrameBufferSize = ffmpeg.av_image_get_buffer_size(AVPixelFormat.AV_PIX_FMT_BGR24, _finalSize.Width, _finalSize.Height, 1);
                                pConvertedFrameBuffer = Marshal.AllocHGlobal(convertedFrameBufferSize);
                                ffmpeg.av_image_fill_arrays(ref dstData, ref dstLinesize, (byte*)pConvertedFrameBuffer, AVPixelFormat.AV_PIX_FMT_BGR24, _finalSize.Width, _finalSize.Height, 1);
                                pConvertContext = ffmpeg.sws_getContext(_videoCodecContext->width, _videoCodecContext->height, NormalizePixelFormat(srcFmt), _finalSize.Width, _finalSize.Height, AVPixelFormat.AV_PIX_FMT_BGR24, ffmpeg.SWS_FAST_BILINEAR, null, null, null);

                            }

                            Log("SWS_SCALE", ffmpeg.sws_scale(pConvertContext, vf->data, vf->linesize, 0, _videoCodecContext->height, dstData, dstLinesize));


                            if (vf->decode_error_flags > 0)
                            {
                                ffmpeg.av_frame_free(&vf);
                                break;
                            }

                            using (
                                var mat = new Bitmap(_finalSize.Width, _finalSize.Height, dstLinesize[0],
                                    PixelFormat.Format24bppRgb, pConvertedFrameBuffer))
                            {
                                var nfe = new NewFrameEventArgs(mat);
                                nf.Invoke(this, nfe);
                            }

                            _lastVideoFrame = DateTime.UtcNow;
                            ffmpeg.av_frame_free(&vf);
                            break;
                        }
                        ffmpeg.av_frame_free(&vf);
                    } while (ret == 0);
                }

                if (nf != null && _videoStream != null)
                    if ((DateTime.UtcNow - _lastVideoFrame).TotalMilliseconds * 1000 > _timeoutMicroSeconds)
                    {
                        _res = ReasonToFinishPlaying.DeviceLost;
                        _abort = true;
                    }

                ffmpeg.av_packet_unref(&packet);
                if (ret == -11)
                    Thread.Sleep(10);
            } while (!_abort && !MainForm.ShuttingDown);

            NewFrame?.Invoke(this, new NewFrameEventArgs(null));

            CleanUp();
        }

        private static AVPixelFormat NormalizePixelFormat(AVPixelFormat fmt)
        {
            switch (fmt)
            {
                case AVPixelFormat.AV_PIX_FMT_YUVJ411P: return AVPixelFormat.AV_PIX_FMT_YUV411P;
                case AVPixelFormat.AV_PIX_FMT_YUVJ420P: return AVPixelFormat.AV_PIX_FMT_YUV420P;
                case AVPixelFormat.AV_PIX_FMT_YUVJ422P: return AVPixelFormat.AV_PIX_FMT_YUV422P;
                case AVPixelFormat.AV_PIX_FMT_YUVJ440P: return AVPixelFormat.AV_PIX_FMT_YUV440P;
                case AVPixelFormat.AV_PIX_FMT_YUVJ444P: return AVPixelFormat.AV_PIX_FMT_YUV444P;
                default: return fmt;
            }
        }

        private void CleanUp()
        {
            try
            {
                Program.MutexHelper.Wait();             

                if (pConvertedFrameBuffer != IntPtr.Zero)
                {
                    Marshal.FreeHGlobal(pConvertedFrameBuffer);
                    pConvertedFrameBuffer = IntPtr.Zero;
                }

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

                if (_hwDeviceCtx != null)
                {
                    var f = _hwDeviceCtx;
                    ffmpeg.av_buffer_unref(&f);
                    _hwDeviceCtx = null;
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
                    pConvertContext = null;
                }

                if (sampleChannel != null)
                {
                    sampleChannel.PreVolumeMeter -= SampleChannelPreVolumeMeter;
                    sampleChannel = null;
                }
                
            }
            catch (Exception ex)
            {
                Logger.LogException(ex, SourceName+ ": Media Stream (close)");
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