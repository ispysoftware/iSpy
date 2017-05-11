using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using FFmpeg.AutoGen;
using iSpyApplication.Sources.Audio;
using iSpyApplication.Utilities;
using NAudio.Wave;
using NAudio.Wave.SampleProviders;

namespace iSpyApplication.Sources.Video
{
    public unsafe class MediaStream : VideoBase, IVideoSource, IAudioSource, ISupportsAudio
    {
        private AVFormatContext* _formatContext;
        private AVCodecContext* _codecContext;
        private AVCodecContext* _audioCodecContext;
        private AVStream* _videoStream, _audioStream;
        private SwrContext* _swrContext;
        public event Delegates.ErrorHandler ErrorHandler;

        private IntPtr _interruptCallbackAddress;
        private AvInterruptCb _interruptCallback;

        [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public delegate int AvInterruptCb(void* ctx);

        public int InterruptCb(void* ctx)
        {
            if ((DateTime.UtcNow - _lastPacket).TotalMilliseconds > _timeout || _stopReadingFrames)
            {
                if (!_stopReadingFrames)
                {
                    _res = ReasonToFinishPlaying.DeviceLost;
                }
                _stopReadingFrames = true;
                return 1;
            }
            return 0;
        }

        private DateTime _lastPacket;
        private bool _stopReadingFrames;
        private Thread _thread;
        private DateTime _lastVideoFrame;
        private ReasonToFinishPlaying _res = ReasonToFinishPlaying.DeviceLost;

        private readonly string _cookies = "";
        private readonly string _userAgent = "";
        private readonly string _headers = "";
        private readonly string RTSPmode = "tcp";
        private readonly string _options = "";
        private readonly objectsCamera _source;
        private readonly objectsMicrophone _audiosource;

        public event NewFrameEventHandler NewFrame;
        public event PlayingFinishedEventHandler PlayingFinished;
        public event DataAvailableEventHandler DataAvailable;
        public event LevelChangedEventHandler LevelChanged;
        public event AudioFinishedEventHandler AudioFinished;


        private readonly AVInputFormat* _inputFormat;

        private readonly int _timeout = 5000;
        private readonly int _analyzeDuration = 2000;

        private volatile bool _starting;
        private readonly bool _modeAudio;

        public MediaStream(objectsCamera source) : base(source)
        {
            _source = source;
            _inputFormat = null;
            _modeAudio = false;

            _cookies = source.settings.cookies;
            _analyzeDuration = source.settings.analyseduration;
            _timeout = source.settings.timeout;
            _userAgent = source.settings.useragent;
            _headers = source.settings.headers;
            RTSPmode = Helper.RTSPMode(source.settings.rtspmode);

        }

        public MediaStream(objectsMicrophone source) : base(null)
        {
            _audiosource = source;
            _inputFormat = null;
            _modeAudio = true;
            _timeout = source.settings.timeout;
            _analyzeDuration = source.settings.analyzeduration;
            _options = source.settings.ffmpeg;
        }

        public MediaStream(string format, objectsCamera source) : base(source)
        {
            _source = source;
            _inputFormat = ffmpeg.av_find_input_format(format);
            if (_inputFormat == null)
            {
                throw new Exception("Can not find input format " + format);
            }
        }

        public string Source
        {
            get
            {
                if (_modeAudio)
                    return _audiosource.settings.sourcename;

                return _source.settings.videosourcestring;
            }
        }

        public void Close()
        {
            Stop();
        }



        public void Start()
        {
            if (_starting || IsRunning) return;
            _stopReadingFrames = false;
            _res = ReasonToFinishPlaying.DeviceLost;
            _starting = true;
            Task.Factory.StartNew(DoStart);

        }

        private void DoStart()
        {
            var vss = Source;
            if (!_modeAudio)
                vss = Tokenise(vss);

            AVDictionary* options = null;
            if (_inputFormat == null)
            {
                ffmpeg.av_dict_set(&options, "analyzeduration", _analyzeDuration.ToString(), 0);



                string prefix = vss.ToLower().Substring(0, vss.IndexOf(":", StringComparison.Ordinal));
                switch (prefix)
                {
                    case "http":
                    case "mmsh":
                    case "mms":
                        ffmpeg.av_dict_set(&options, "timeout", _timeout.ToString(), 0);
                        ffmpeg.av_dict_set(&options, "stimeout", (_timeout * 1000).ToString(), 0);

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
                    case "tcp":
                    case "udp":
                    case "rtp":
                    case "sdp":
                    case "mmst":
                    case "ftp":
                        ffmpeg.av_dict_set(&options, "timeout", _timeout.ToString(), 0);
                        break;
                    case "rtsp":
                    case "rtmp":
                        ffmpeg.av_dict_set(&options, "stimeout", (_timeout * 1000).ToString(), 0);
                        if (_userAgent != "")
                        {
                            ffmpeg.av_dict_set(&options, "user-agent", _userAgent, 0);
                        }
                        break;
                }

                ffmpeg.av_dict_set(&options, "rtsp_transport", RTSPmode, 0);
            }

            ffmpeg.av_dict_set(&options, "rtbufsize", "10000000", 0);

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


            _stopReadingFrames = false;
            try
            {
                Program.FfmpegMutex.WaitOne();
                var pFormatContext = ffmpeg.avformat_alloc_context();
                _lastPacket = DateTime.UtcNow;


                _interruptCallback = InterruptCb;
                _interruptCallbackAddress = Marshal.GetFunctionPointerForDelegate(_interruptCallback);

                pFormatContext->interrupt_callback.callback = _interruptCallbackAddress;
                pFormatContext->interrupt_callback.opaque = null;


                if (ffmpeg.avformat_open_input(&pFormatContext, vss, _inputFormat, &options) != 0)
                {
                    throw new ApplicationException(@"Could not open source");
                }
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
                    Program.FfmpegMutex.ReleaseMutex();
                }
                catch
                {

                }
            }
            _starting = false;
        }


        public void Restart()
        {
            if (!IsRunning) return;
            _res = ReasonToFinishPlaying.Restart;
            _stopReadingFrames = true;
        }

        public void Stop()
        {
            if (!IsRunning) return;
            _res = ReasonToFinishPlaying.StoppedByUser;
            _stopReadingFrames = true;
        }

        private void SetupFormat()
        {

            if (ffmpeg.avformat_find_stream_info(_formatContext, null) != 0)
            {
                throw new ApplicationException(@"Could not find stream info");
            }


            _formatContext->flags |= ffmpeg.AVFMT_FLAG_DISCARD_CORRUPT;
            _formatContext->flags |= ffmpeg.AVFMT_FLAG_NOBUFFER;

            for (int i = 0; i < _formatContext->nb_streams; i++)
            {
                if (_formatContext->streams[i]->codec->codec_type == AVMediaType.AVMEDIA_TYPE_VIDEO)
                {
                    // get the pointer to the codec context for the video stream
                    _codecContext = _formatContext->streams[i]->codec;
                    _codecContext->workaround_bugs = 1;
                    _videoStream = _formatContext->streams[i];
                    break;
                }
            }

            if (_videoStream != null)
            {
                _codecContext->flags2 |= ffmpeg.CODEC_FLAG2_FAST | ffmpeg.CODEC_FLAG2_CHUNKS |
                                         ffmpeg.CODEC_FLAG_LOW_DELAY;

                var codec = ffmpeg.avcodec_find_decoder(_codecContext->codec_id);
                if (codec == null)
                {
                    throw new ApplicationException("Cannot find a codec to decode the video stream.");
                }

                if ((codec->capabilities & ffmpeg.AV_CODEC_CAP_TRUNCATED) == ffmpeg.AV_CODEC_CAP_TRUNCATED)
                {
                    _codecContext->flags |= ffmpeg.AV_CODEC_FLAG_TRUNCATED;
                }

                if (ffmpeg.avcodec_open2(_codecContext, codec, null) < 0)
                {
                    throw new ApplicationException("Cannot open the video codec.");
                }

                _lastVideoFrame = DateTime.UtcNow;

                AVHWAccel* hwaccel = null;

                while (true)
                {
                    hwaccel = ffmpeg.av_hwaccel_next(hwaccel);
                    if (hwaccel == null)
                        break;
                    if (hwaccel->id == _codecContext->codec_id && hwaccel->pix_fmt == _codecContext->pix_fmt)
                    {
                        Logger.LogMessage("USing HW decoder");
                        _codecContext->hwaccel = hwaccel;
                        break;
                    }

                }
            }

            _lastPacket = DateTime.UtcNow;

            for (int i = 0; i < _formatContext->nb_streams; i++)
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
                AVCodec* audiocodec = ffmpeg.avcodec_find_decoder(_audioCodecContext->codec_id);
                if (audiocodec != null)
                {
                    _audioCodecContext->refcounted_frames = 0;

                    if (ffmpeg.avcodec_open2(_audioCodecContext, audiocodec, null) == 0)
                    {
                        _audioCodecContext->request_sample_fmt = AVSampleFormat.AV_SAMPLE_FMT_S16;

                        int chans = 1;
                        if (_audioCodecContext->channels > 1) //downmix
                            chans = 2;

                        _swrContext = ffmpeg.swr_alloc_set_opts(null,
                                ffmpeg.av_get_default_channel_layout(chans),
                                AVSampleFormat.AV_SAMPLE_FMT_S16,
                                _audioCodecContext->sample_rate,
                                ffmpeg.av_get_default_channel_layout(_audioCodecContext->channels),
                                _audioCodecContext->sample_fmt,
                                _audioCodecContext->sample_rate,
                                0,
                                null);
                        ffmpeg.swr_init(_swrContext);

                        RecordingFormat = new WaveFormat(_audioCodecContext->sample_rate, 16, _audioCodecContext->channels);
                    }
                }
            }

            if (_videoStream == null && _audioStream == null)
            {
                throw new ApplicationException("Cannot find any streams.");
            }

            _lastPacket = DateTime.UtcNow;

            _thread = new Thread(ReadFrames) { Name = Source, IsBackground = true };
            _thread.Start();
        }

        private void ReadFrames()
        {
            AVFrame* pConvertedFrame = null;
            sbyte* pConvertedFrameBuffer = null;
            SwsContext* pConvertContext = null;

            BufferedWaveProvider waveProvider = null;
            SampleChannel sampleChannel = null;

            bool audioInited = false;
            bool videoInited = false;
            var packet = new AVPacket();

            do
            {
                ffmpeg.av_init_packet(&packet);

                AVFrame* frame = ffmpeg.av_frame_alloc();
                ffmpeg.av_frame_unref(frame);

                if (ffmpeg.av_read_frame(_formatContext, &packet) < 0)
                {
                    _stopReadingFrames = true;
                    _res = ReasonToFinishPlaying.VideoSourceError;
                    break;
                }

                if ((packet.flags & ffmpeg.AV_PKT_FLAG_CORRUPT) == ffmpeg.AV_PKT_FLAG_CORRUPT)
                {
                    break;
                }

                AVPacket packetTemp = packet;
                var nf = NewFrame;
                var da = DataAvailable;

                _lastPacket = DateTime.UtcNow;
                if (_audioStream != null && packetTemp.stream_index == _audioStream->index)
                {
                    if (HasAudioStream != null)
                    {
                        HasAudioStream?.Invoke(this, EventArgs.Empty);
                        HasAudioStream = null;
                    }
                    if (da != null)
                    {

                        int s = 0;
                        var buffer = new sbyte[_audioCodecContext->sample_rate * 2];
                        var tbuffer = new sbyte[_audioCodecContext->sample_rate * 2];
                        bool b = false;

                        fixed (sbyte** outPtrs = new sbyte*[32])
                        {
                            fixed (sbyte* bPtr = &tbuffer[0])
                            {
                                outPtrs[0] = bPtr;
                                do
                                {
                                    int gotFrame = 0;
                                    int inUsed = ffmpeg.avcodec_decode_audio4(_audioCodecContext, frame, &gotFrame,
                                        &packetTemp);

                                    if (inUsed < 0 || gotFrame == 0)
                                    {
                                        b = true;
                                        break;
                                    }

                                    int numSamplesOut = ffmpeg.swr_convert(_swrContext,
                                        outPtrs,
                                        _audioCodecContext->sample_rate,
                                        &frame->data0,
                                        frame->nb_samples);

                                    var l = numSamplesOut * 2 * _audioCodecContext->channels;
                                    Buffer.BlockCopy(tbuffer, 0, buffer, s, l);
                                    s += l;


                                    packetTemp.data += inUsed;
                                    packetTemp.size -= inUsed;
                                } while (packetTemp.size > 0);
                            }
                        }

                        if (b)
                        {
                            break;
                        }

                        ffmpeg.av_free_packet(&packet);
                        ffmpeg.av_frame_free(&frame);


                        if (!audioInited)
                        {
                            audioInited = true;
                            RecordingFormat = new WaveFormat(_audioCodecContext->sample_rate, 16,
                                _audioCodecContext->channels);
                            waveProvider = new BufferedWaveProvider(RecordingFormat)
                            {
                                DiscardOnBufferOverflow = true,
                                BufferDuration =
                                                    TimeSpan.FromMilliseconds(500)
                            };
                            sampleChannel = new SampleChannel(waveProvider);

                            sampleChannel.PreVolumeMeter += SampleChannelPreVolumeMeter;
                        }

                        byte[] ba = new byte[s];
                        Buffer.BlockCopy(buffer, 0, ba, 0, s);


                        waveProvider.AddSamples(ba, 0, s);

                        var sampleBuffer = new float[s];
                        int read = sampleChannel.Read(sampleBuffer, 0, s);


                        da(this, new DataAvailableEventArgs(ba, read));


                        if (Listening)
                        {
                            WaveOutProvider?.AddSamples(ba, 0, read);
                        }
                    }
                }

                if (nf != null && _videoStream != null && packet.stream_index == _videoStream->index)
                {
                    int frameFinished = 0;
                    //decode video frame

                    int ret = ffmpeg.avcodec_decode_video2(_codecContext, frame, &frameFinished, &packetTemp);
                    if (ret < 0)
                    {
                        ffmpeg.av_free_packet(&packet);
                        ffmpeg.av_frame_free(&frame);
                        break;
                    }

                    if (frameFinished == 1)
                    {
                        if (!videoInited)
                        {
                            videoInited = true;
                            pConvertedFrame = ffmpeg.av_frame_alloc();
                            var convertedFrameBufferSize = ffmpeg.avpicture_get_size(AVPixelFormat.AV_PIX_FMT_BGR24,
                                _codecContext->width, _codecContext->height);

                            pConvertedFrameBuffer = (sbyte*)ffmpeg.av_malloc((ulong)convertedFrameBufferSize);

                            ffmpeg.avpicture_fill((AVPicture*)pConvertedFrame, pConvertedFrameBuffer,
                                AVPixelFormat.AV_PIX_FMT_BGR24, _codecContext->width, _codecContext->height);

                            pConvertContext = ffmpeg.sws_getContext(_codecContext->width, _codecContext->height,
                                _codecContext->pix_fmt, _codecContext->width, _codecContext->height,
                                AVPixelFormat.AV_PIX_FMT_BGR24, ffmpeg.SWS_FAST_BILINEAR, null, null, null);
                        }
                        var src = &frame->data0;
                        var dst = &pConvertedFrame->data0;
                        var srcStride = frame->linesize;
                        var dstStride = pConvertedFrame->linesize;
                        ffmpeg.sws_scale(pConvertContext, src, srcStride, 0, _codecContext->height, dst, dstStride);

                        var convertedFrameAddress = pConvertedFrame->data0;
                        if (convertedFrameAddress != null)
                        {
                            var imageBufferPtr = new IntPtr(convertedFrameAddress);

                            var linesize = dstStride[0];

                            if (frame->decode_error_flags > 0)
                            {
                                ffmpeg.av_free_packet(&packet);
                                ffmpeg.av_frame_free(&frame);
                                break;
                            }

                            using (
                                var mat = new Bitmap(_codecContext->width, _codecContext->height, linesize,
                                    PixelFormat.Format24bppRgb, imageBufferPtr))
                            {
                                var nfe = new NewFrameEventArgs((Bitmap)mat.Clone());
                                nf.Invoke(this, nfe);
                            }

                            _lastVideoFrame = DateTime.UtcNow;
                        }
                    }
                }

                if (_videoStream != null)
                {
                    if ((DateTime.UtcNow - _lastVideoFrame).TotalMilliseconds > _timeout)
                    {
                        _res = ReasonToFinishPlaying.DeviceLost;
                        _stopReadingFrames = true;
                    }
                }

                ffmpeg.av_free_packet(&packet);
                ffmpeg.av_frame_free(&frame);
            } while (!_stopReadingFrames && !MainForm.ShuttingDown);


            try
            {
                Program.FfmpegMutex.WaitOne();

                if (pConvertedFrame != null)
                    ffmpeg.av_free(pConvertedFrame);

                if (pConvertedFrameBuffer != null)
                    ffmpeg.av_free(pConvertedFrameBuffer);

                if (_formatContext != null)
                {
                    if (_formatContext->streams != null)
                    {
                        int j = (int)_formatContext->nb_streams;
                        for (var i = j - 1; i >= 0; i--)
                        {
                            AVStream* stream = _formatContext->streams[i];

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

                _videoStream = null;
                _audioStream = null;
                _audioCodecContext = null;
                _codecContext = null;

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
                    Program.FfmpegMutex.ReleaseMutex();
                }
                catch
                {

                }
            }

            PlayingFinished?.Invoke(this, new PlayingFinishedEventArgs(_res));
            AudioFinished?.Invoke(this, new PlayingFinishedEventArgs(_res));

        }

        public event HasAudioStreamEventHandler HasAudioStream;

        public WaveFormat RecordingFormat { get; set; }

        public BufferedWaveProvider WaveOutProvider { get; set; }

        private bool _listening;
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

        public int FramesReceived { get; }
        public long BytesReceived { get; }

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

        void SampleChannelPreVolumeMeter(object sender, StreamVolumeEventArgs e)
        {
            LevelChanged?.Invoke(this, new LevelChangedEventArgs(e.MaxSampleValues));
        }
    }
}