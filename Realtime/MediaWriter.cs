using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using FFmpeg.AutoGen;
using iSpyApplication.Sources.Video;
using iSpyApplication.Utilities;

namespace iSpyApplication.Realtime
{
    internal unsafe class MediaWriter : FFmpegBase
    {
        [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public delegate int AvInterruptCb(void* ctx);

        public delegate int InterruptCallback();
        private readonly byte[] _convOut = new byte[44100];
        private readonly bool _isAudio;
        private bool _abort;
        private byte[] _audioBuffer = new byte[44100];
        private int _audioBufferSizeCurrent;
        private AVCodecContext* _audioCodecContext;
        private AVFrame* _audioFrame, _videoFrame;

        private AVIOInterruptCB_callback_func _aviocb;
        private bool _closing;
        private GCHandle _convHandle;
        private AVFormatContext* _formatContext;
        private int _frameNumber;
        private bool _ignoreAudio;
        private AvInterruptCb _interruptCallback;
        private IntPtr _interruptCallbackAddress;

        private bool _isConstantFramerate;

        private long _lastAudioPts;

        private DateTime _lastPacket;
        //private double _maxLevel = -1;
        private bool _opened;
        private SwsContext* _pConvertContext;
        private SwrContext* _swrContext;
        private AVCodec* _videoCodec;
        private AVCodecContext* _videoCodecContext;
        private AVStream* _videoStream, _audioStream;
        private int _width, _height, _framerate; //_bitRate
        public DateTime CreatedDate;

        public string Filename;
        public int InChannels = 1;
        public int InSampleRate = 22050;
        public bool IsFile = true;
        public bool IsTimelapse;
        private readonly string movflags = ""; //"faststart";
        public int OutChannels = 1;
        public int OutSampleRate = 22050;
        public long SizeBytes;
        private int _crf;
        public int GpuIndex = 0;

        public int Timeout = 5000;

        public HwEncoder Gpu;
        public class HwEncoder
        {
            public string Codec;
            public string Name;

            public HwEncoder(string codec, string name)
            {
                Codec = codec;
                Name = name;
            }
        }

        public static HwEncoder[] Encoders =
        {
            new HwEncoder("h264_nvenc", "nvidia"),
            new HwEncoder("h264_qsv", "quicksync"),
            new HwEncoder("h264_amf", "amd"),
        };

        public MediaWriter(): base("Writer")
        {
            
        }

        public bool Closed => !_opened;

        public int InterruptCb(void* ctx)
        {
            if ((DateTime.UtcNow - _lastPacket).TotalMilliseconds > Timeout || _abort)
            {
                if (!_abort)
                {
                    Logger.LogMessage("Writer Timeout");
                }

                Logger.LogMessage("Aborting Writer");
                _abort = true;
                return 1;
            }
            return 0;
        }

        public int InterruptCb()
        {
            if ((DateTime.UtcNow - _lastPacket).TotalMilliseconds > Timeout || _abort)
            {
                return 1;
            }
            return 0;
        }

        public void Open(string fileName, int width, int height, AVCodecID videoCodec, int framerate,AVCodecID audioCodec, DateTime created, int crf)
        {
            CreatedDate = created;
            Filename = fileName;
            _abort = false;
            _ignoreAudio = false;
            _crf = crf;

            if (videoCodec != AVCodecID.AV_CODEC_ID_NONE)
            {
                IsTimelapse = framerate != 0;

                if (((width & 1) != 0) || ((height & 1) != 0))
                {
                    throw new ArgumentException("Video file resolution must be a multiple of two.");
                }
            }

            int i;
            _lastPacket = DateTime.UtcNow;
            var outputFormat = ffmpeg.av_guess_format(null, fileName, null);
            if (outputFormat == null)
            {
                switch (videoCodec)
                {
                    default:
                    case AVCodecID.AV_CODEC_ID_MPEG1VIDEO:
                        outputFormat = ffmpeg.av_guess_format("mpeg1video", null, null);
                        break;
                }
            }

            _formatContext = ffmpeg.avformat_alloc_context();

            if (_formatContext == null)
            {
                throw new Exception("Cannot allocate format context.");
            }

            _interruptCallback = InterruptCb;
            _interruptCallbackAddress = Marshal.GetFunctionPointerForDelegate(_interruptCallback);

            _aviocb = new AVIOInterruptCB_callback_func
                      {
                          Pointer = _interruptCallbackAddress
                      };
            _formatContext->interrupt_callback.callback = _aviocb;
            _formatContext->interrupt_callback.opaque = null;


            _formatContext->oformat = outputFormat;

            AVDictionary* opts = null;

            if (audioCodec != AVCodecID.AV_CODEC_ID_NONE)
            {
                AddAudioStream(audioCodec);
                OpenAudio();
            }

            if (videoCodec != AVCodecID.AV_CODEC_ID_NONE)
            {
                _width = width;
                _height = height;
                //_bitRate = videoBitRate;
                _framerate = framerate;
                _isConstantFramerate = framerate > 0;

                AddVideoStream(videoCodec);
                OpenVideo();

                if (videoCodec == AVCodecID.AV_CODEC_ID_MPEG1VIDEO)
                {
                    ffmpeg.av_dict_set(&opts, "pkt_size", "1316", 0);
                    ffmpeg.av_dict_set(&opts, "buffer_size", "65535", 0);

                    //ffmpeg.av_dict_set(&opts, "crf", "30", 0);
                }
            }


            if (movflags != "")
                ffmpeg.av_dict_set(&opts, "movflags", movflags, 0);


            if ((outputFormat->flags & ffmpeg.AVFMT_NOFILE) != ffmpeg.AVFMT_NOFILE)
            {
                i = ffmpeg.avio_open2(&_formatContext->pb, fileName, ffmpeg.AVIO_FLAG_WRITE, null, &opts);
                if (i < 0)
                {
                    throw new Exception("Cannot create the video file. (" + i + ")");
                }
            }

            i = ffmpeg.avformat_write_header(_formatContext, null);
            if (i < 0)
            {
                throw new Exception("Cannot write header - check disk space (" + i + ")");
            }

            ffmpeg.av_dict_free(&opts);

            _frameNumber = 0;
            _opened = true;
        }

        public void Close()
        {
            DoClose();
        }

        private void DoClose()
        {
            Program.MutexHelper.Wait();
            if (_formatContext != null)
            {
                if (IsFile)
                {
                    if (_opened)
                    {
                        Flush();
                    }

                    if (_formatContext->pb != null)
                    {
                        ffmpeg.av_write_trailer(_formatContext);
                    }
                }
                if (_formatContext->pb != null)
                {
                    var pinprt = &_formatContext->pb;
                    ffmpeg.avio_closep(pinprt);
                    _formatContext->pb = null;
                }

                _audioBuffer = null;


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

                if (_videoCodecContext != null)
                {
                    ffmpeg.avcodec_close(_videoCodecContext);
                    fixed (AVCodecContext** c = &_videoCodecContext)
                    {
                        ffmpeg.avcodec_free_context(c);
                    }
                }

                if (_audioCodecContext != null)
                {
                    ffmpeg.avcodec_close(_audioCodecContext);
                    fixed (AVCodecContext** c = &_audioCodecContext)
                    {
                        ffmpeg.avcodec_free_context(c);
                    }
                }

                if (_formatContext->streams != null)
                {
                    var j = (int) _formatContext->nb_streams;
                    for (var i = j - 1; i >= 0; i--)
                    {
                        var stream = _formatContext->streams[i];
                        if (stream != null && stream->codec != null)
                        {
                            if (stream->codecpar != null && stream->codecpar->extradata_size > 0)
                                ffmpeg.av_free(stream->codecpar->extradata);

                            stream->discard = AVDiscard.AVDISCARD_ALL;
                            ffmpeg.av_freep(&stream);
                        }
                    }
                }


                _videoStream = null;
                _audioStream = null;

                fixed (AVFormatContext** pinprt = &_formatContext)
                {
                    ffmpeg.av_freep(pinprt);
                }
                _formatContext = null;
            }

            if (_pConvertContext != null)
            {
                ffmpeg.sws_freeContext(_pConvertContext);
                _pConvertContext = null;
            }
            if (_swrContext != null)
            {
                ffmpeg.swr_close(_swrContext);
                _swrContext = null;
            }

            if (IsFile)
            {
                try
                {
                    var fi = new FileInfo(Filename);
                    SizeBytes = fi.Length;
                }
                catch
                {
                    SizeBytes = 0;
                }
            }

            if (_convHandle.IsAllocated)
                _convHandle.Free();
            _opened = false;
        }

        public void WriteAudio(byte[] soundBuffer, int soundBufferSize, int level, DateTime timestamp)
        {
            if (!_opened)
            {
                throw new Exception("An audio file was not opened yet.");
            }
            AddAudioSamples(soundBuffer, soundBufferSize, timestamp);

            if (_isAudio)
            {
                //_frameWritten.Set();
            }
        }

        public void WriteFrame(Bitmap frame)
        {
            WriteFrame(frame, DateTime.MinValue);
        }
        public void WriteFrame(Bitmap frame, DateTime timestamp)
        {
            if (!_opened)
            {
                throw new Exception("A file was not opened yet.");
            }
            if (ffmpeg.avcodec_is_open(_videoCodecContext) <= 0)
                throw new Exception("codec is not open");

            if ((frame.Width != _videoCodecContext->width) || (frame.Height != _videoCodecContext->height))
            {
                throw new Exception("Frame size cannot change during recording.");
            }

            var bitmapData = frame.LockBits(new Rectangle(0, 0, _width, _height), ImageLockMode.ReadOnly,
            frame.PixelFormat == PixelFormat.Format8bppIndexed
                ? PixelFormat.Format8bppIndexed
                : PixelFormat.Format24bppRgb);

            byte*[] srcData = {(byte*) bitmapData.Scan0};
            int[] srcLinesize = {bitmapData.Stride};

            if (_pConvertContext == null)
            {
                var pfmt = AVPixelFormat.AV_PIX_FMT_BGR24;

                if (frame.PixelFormat == PixelFormat.Format8bppIndexed)
                {
                    pfmt = AVPixelFormat.AV_PIX_FMT_GRAY8;
                }

                int w = _videoCodecContext->width;
                int h = _videoCodecContext->height;
                _pConvertContext = ffmpeg.sws_getContext(w, h, pfmt, w, h,_videoCodecContext->pix_fmt, ffmpeg.SWS_FAST_BILINEAR, null, null, null);
            }


            if (Log("SWS_SCALE",
                ffmpeg.sws_scale(_pConvertContext, srcData, srcLinesize, 0, _videoCodecContext->height,
                    _videoFrame->data, _videoFrame->linesize)))
            {
                return;
            }

            frame.UnlockBits(bitmapData);

            if (!_isConstantFramerate)
            {
                var pts = (long)(timestamp - CreatedDate).TotalMilliseconds;
                _videoFrame->pts = pts;
            }
            else
            {
                _videoFrame->pts = _frameNumber;
                _frameNumber++;
            }


            var packet = new AVPacket();
            ffmpeg.av_init_packet(&packet);

            packet.data = null;
            packet.size = 0;

            var ret = ffmpeg.avcodec_send_frame(_videoCodecContext, _videoFrame);
            while (ret >= 0)
            {
                ret = ffmpeg.avcodec_receive_packet(_videoCodecContext, &packet);
                if (ret == 0)
                {
                    if (packet.size > 0)
                    {
                        if (packet.pts != ffmpeg.AV_NOPTS_VALUE)
                            packet.pts = ffmpeg.av_rescale_q(packet.pts, _videoCodecContext->time_base,
                                _videoStream->time_base);
                        if (packet.dts != ffmpeg.AV_NOPTS_VALUE)
                            packet.dts = ffmpeg.av_rescale_q(packet.dts, _videoCodecContext->time_base,
                                _videoStream->time_base);

                        packet.stream_index = _videoStream->index;
                        // write the compressed frame to the media file
                        _lastPacket = DateTime.UtcNow;
                        ret = ffmpeg.av_interleaved_write_frame(_formatContext, &packet);
                    }
                }
            }

            ffmpeg.av_packet_unref(&packet);
            //_frameWritten.Set();
        }

        private void Flush()
        {
            if (_opened)
            {
                if (_videoStream != null && _videoCodecContext != null)
                {
                    _lastPacket = DateTime.UtcNow;
                    var packet = new AVPacket();
                    ffmpeg.av_init_packet(&packet);

                    var ret = ffmpeg.avcodec_send_frame(_videoCodecContext, null);
                    while (ret >= 0)
                    {
                        ret = ffmpeg.avcodec_receive_packet(_videoCodecContext, &packet);
                        if (ret == 0)
                        {
                            if (packet.size > 0)
                            {
                                if (packet.pts != ffmpeg.AV_NOPTS_VALUE)
                                    packet.pts = ffmpeg.av_rescale_q(packet.pts, _videoCodecContext->time_base,
                                        _videoStream->time_base);
                                if (packet.dts != ffmpeg.AV_NOPTS_VALUE)
                                    packet.dts = ffmpeg.av_rescale_q(packet.dts, _videoCodecContext->time_base,
                                        _videoStream->time_base);

                                packet.stream_index = _videoStream->index;
                                // write the compressed frame to the media file
                                ret = ffmpeg.av_interleaved_write_frame(_formatContext, &packet);
                            }
                        }
                    }
                    ffmpeg.av_packet_unref(&packet);
                }
                if (_audioStream != null && _audioCodecContext != null)
                {
                    _lastPacket = DateTime.UtcNow;
                    var packet = new AVPacket();
                    ffmpeg.av_init_packet(&packet);

                    var ret = ffmpeg.avcodec_send_frame(_audioCodecContext, null);
                    while (ret >= 0)
                    {
                        ret = ffmpeg.avcodec_receive_packet(_audioCodecContext, &packet);
                        if (ret == 0)
                        {
                            if (packet.size > 0)
                            {
                                if (packet.pts != ffmpeg.AV_NOPTS_VALUE)
                                    packet.pts = ffmpeg.av_rescale_q(packet.pts, _audioCodecContext->time_base,
                                        _audioStream->time_base);
                                if (packet.dts != ffmpeg.AV_NOPTS_VALUE)
                                    packet.dts = ffmpeg.av_rescale_q(packet.dts, _audioCodecContext->time_base,
                                        _audioStream->time_base);

                                packet.stream_index = _audioStream->index;
                                // write the compressed frame to the media file
                                ret = ffmpeg.av_interleaved_write_frame(_formatContext, &packet);
                            }
                        }
                    }
                    ffmpeg.av_packet_unref(&packet);
                }
            }
        }

        private void AddAudioSamples(byte[] soundBuffer, int soundBufferSize, DateTime timestamp)
        {
            if (_audioStream == null || _audioCodecContext == null || soundBufferSize <= 0 || _ignoreAudio)
                return;

            var srcSize = ffmpeg.av_samples_get_buffer_size(null, InChannels, _audioCodecContext->frame_size,
                AVSampleFormat.AV_SAMPLE_FMT_S16, 0);

            ffmpeg.av_frame_unref(_audioFrame);

            _audioFrame->nb_samples = _audioCodecContext->frame_size;

            var packet = new AVPacket();

            Buffer.BlockCopy(soundBuffer, 0, _audioBuffer, _audioBufferSizeCurrent, soundBufferSize);
            _audioBufferSizeCurrent += soundBufferSize;

            int remaining = _audioBufferSizeCurrent, cursor = 0;
            fixed (byte* p = _audioBuffer)
            {
                var inPointerLocal = p;
                var pts = (long) (timestamp - CreatedDate).TotalMilliseconds;
                while (remaining >= srcSize)
                {
                    ffmpeg.av_init_packet(&packet);

                    var ptr = _convHandle.AddrOfPinnedObject().ToPointer();
                    var convOutPointerLocal = (byte*) ptr;
                    var dstNbSamples =
                        (int)
                            ffmpeg.av_rescale_rnd(
                                ffmpeg.swr_get_delay(_swrContext, _audioCodecContext->sample_rate) +
                                _audioCodecContext->frame_size, _audioCodecContext->sample_rate,
                                _audioCodecContext->sample_rate,
                                AVRounding.AV_ROUND_UP);

                    if (Log("SWR_CONVERT", ffmpeg.swr_convert(_swrContext,
                        &convOutPointerLocal,
                        dstNbSamples,
                        &inPointerLocal,
                        _audioFrame->nb_samples)))
                    {
                        ffmpeg.av_packet_unref(&packet);
                        _ignoreAudio = true;
                        _audioBufferSizeCurrent = 0;
                        return;
                    }


                    _audioFrame->pts = Math.Max(pts, _lastAudioPts + 1);
                    _lastAudioPts = _audioFrame->pts;


                    var dstSamplesSize = ffmpeg.av_samples_get_buffer_size(null, _audioCodecContext->channels,
                        _audioCodecContext->frame_size,
                        _audioCodecContext->sample_fmt, 0);

                    if (Log("FILL_AUDIO",
                        ffmpeg.avcodec_fill_audio_frame(_audioFrame, _audioCodecContext->channels,
                            _audioCodecContext->sample_fmt, convOutPointerLocal, dstSamplesSize, 0)))
                    {
                        ffmpeg.av_packet_unref(&packet);
                        _ignoreAudio = true;
                        _audioBufferSizeCurrent = 0;
                        return;
                    }
                    inPointerLocal += srcSize;

                    var ret = ffmpeg.avcodec_send_frame(_audioCodecContext, _audioFrame);
                    while (ret >= 0)
                    {
                        ret = ffmpeg.avcodec_receive_packet(_audioCodecContext, &packet);
                        if (ret == 0)
                        {
                            if (packet.pts != ffmpeg.AV_NOPTS_VALUE)
                                packet.pts = ffmpeg.av_rescale_q(packet.pts, _audioCodecContext->time_base,
                                    _audioStream->time_base);
                            if (packet.dts != ffmpeg.AV_NOPTS_VALUE)
                                packet.dts = ffmpeg.av_rescale_q(packet.dts, _audioCodecContext->time_base,
                                    _audioStream->time_base);


                            packet.stream_index = _audioStream->index;
                            packet.flags |= ffmpeg.AV_PKT_FLAG_KEY;
                            _lastPacket = DateTime.UtcNow;

                            if (Log("WRITE_AUDIO_FRAME", ffmpeg.av_interleaved_write_frame(_formatContext, &packet)))
                            {
                                ffmpeg.av_packet_unref(&packet);
                                _ignoreAudio = true;
                                return;
                            }

                            
                        }
                    }
                    cursor += srcSize;
                    remaining -= srcSize;
                    ffmpeg.av_packet_unref(&packet);
                    pts++;
                }
            }

            Buffer.BlockCopy(_audioBuffer, cursor, _audioBuffer, 0, remaining);
            _audioBufferSizeCurrent = remaining;
        }

        private void OpenVideo()
        {
            //_maxLevel = -1;
            ffmpeg.avcodec_parameters_from_context(_videoStream->codecpar, _videoCodecContext);

            _videoFrame = ffmpeg.av_frame_alloc();

            _videoFrame->width = _videoCodecContext->width;
            _videoFrame->height = _videoCodecContext->height;
            _videoFrame->format = (int) _videoCodecContext->pix_fmt;

            ffmpeg.av_frame_get_buffer(_videoFrame, 32);
        }

        private void GetVideoCodec(AVCodecID baseCodec)
        {
            if (Gpu != null)
            {
                _videoCodec = ffmpeg.avcodec_find_encoder_by_name(Gpu.Codec);
                if (_videoCodec != null)
                {
                    if (TryOpenVideoCodec(baseCodec, Gpu))
                    {
                        Logger.LogMessage("using " + Gpu.Name + " hardware encoder");
                        return;
                    }

                    switch (Gpu.Name)
                    {
                        case "quicksync":
                            Logger.LogMessage("Install Intel Media Server Studio to use QSV encoder");
                            break;
                        default:
                            Logger.LogMessage("Update graphics driver to use " + Gpu.Name + " encoder");
                            break;
                    }
                }
            }

            
            _videoCodec = ffmpeg.avcodec_find_encoder(baseCodec);

            if (TryOpenVideoCodec(baseCodec, null))
            {
                Logger.LogMessage("using software encoder");
                return;
            }

            Logger.LogMessage("could not open any encoder codec");
            throw new Exception("Failed opening any codec");
        }

        private bool TryOpenVideoCodec(AVCodecID baseCodec, HwEncoder gpu)
        {
            _videoCodecContext = ffmpeg.avcodec_alloc_context3(_videoCodec);

            _videoCodecContext->codec_id = baseCodec;
            _videoCodecContext->codec_type = AVMediaType.AVMEDIA_TYPE_VIDEO;

            _videoCodecContext->width = _width;
            _videoCodecContext->height = _height;

            if (_isConstantFramerate)
            {
                _videoCodecContext->time_base.num = 1;
                _videoCodecContext->time_base.den = _framerate;
            }
            else
            {
                _videoCodecContext->time_base.num = 1;
                switch (baseCodec)
                {
                    default:
                        _videoCodecContext->time_base.den = 1000;
                        break;
                }
            }
            
            //ffmpeg.av_opt_set(_videoCodecContext->priv_data, "tune", "zerolatency", 0);
            _videoCodecContext->pix_fmt = AVPixelFormat.AV_PIX_FMT_YUV420P;
            switch (_videoCodecContext->codec_id)
            {
                case AVCodecID.AV_CODEC_ID_MPEG1VIDEO:
                    _videoCodecContext->bit_rate = 700000;
                    /* frames per second */
                    //_videoCodecContext->gop_size = 12; /* emit one intra frame every ten frames */
                    _videoCodecContext->max_b_frames = 0;
                    //ffmpeg.av_opt_set(_videoCodecContext->priv_data, "crf", "30.0", 0);
                    //ffmpeg.av_opt_set(_videoCodecContext->priv_data, "preset", "slow", 0);
                    ffmpeg.av_opt_set(_videoCodecContext->priv_data, "pkt_size", "1316", 0);
                    break;
                case AVCodecID.AV_CODEC_ID_H264:
                    ffmpeg.av_opt_set(_videoCodecContext->priv_data, "profile", "baseline", ffmpeg.AV_OPT_SEARCH_CHILDREN); //keep as baseline - otherwise rewind doesn't work
                    if (gpu != null)
                    {
                        ffmpeg.av_opt_set_int(_videoCodecContext->priv_data, "hwaccel_device", GpuIndex, ffmpeg.AV_OPT_SEARCH_CHILDREN);
                        ffmpeg.av_opt_set(_videoCodecContext->priv_data, "preset", "fast", ffmpeg.AV_OPT_SEARCH_CHILDREN);
                        _videoCodecContext->bit_rate = 100000000;
                        switch (gpu.Name)
                        {
                            case "amd":
                                ffmpeg.av_opt_set_int(_videoCodecContext->priv_data, "crf", _crf, ffmpeg.AV_OPT_SEARCH_CHILDREN);
                                _videoCodecContext->rc_max_rate = 100000000;
                                _videoCodecContext->rc_min_rate = 50000000;
                                _videoCodecContext->qmin = 18;
                                _videoCodecContext->qmax = 46;
                                _videoCodecContext->max_qdiff = 4;
                                break;
                            case "quicksync":
                                _videoCodecContext->pix_fmt = AVPixelFormat.AV_PIX_FMT_NV12;
                                break;
                        }
                    }
                    else
                        ffmpeg.av_opt_set(_videoCodecContext->priv_data, "preset", "veryfast", ffmpeg.AV_OPT_SEARCH_CHILDREN);

                    ffmpeg.av_opt_set_int(_videoCodecContext->priv_data, "crf", _crf, ffmpeg.AV_OPT_SEARCH_CHILDREN);

                    break;
                case AVCodecID.AV_CODEC_ID_HEVC:
                    ffmpeg.av_opt_set(_videoCodecContext->priv_data, "x265-params", "qp=20", 0);
                    ffmpeg.av_opt_set(_videoCodecContext->priv_data, "preset", "slow", 0);
                    _videoCodecContext->qmin = 16;
                    _videoCodecContext->qmax = 26;
                    _videoCodecContext->max_qdiff = 4;
                    //_videoCodecContext->sample_aspect_ratio.num = _width;
                    //_videoCodecContext->sample_aspect_ratio.den = _height;
                    break;
                default:
                    //_videoCodecContext->bit_rate = _bitRate;
                    break;
            }

            

            if ((_formatContext->oformat->flags & ffmpeg.AVFMT_GLOBALHEADER) == ffmpeg.AVFMT_GLOBALHEADER)
            {
                _videoCodecContext->flags |= ffmpeg.AV_CODEC_FLAG_GLOBAL_HEADER;
            }

            int cdc;
            try
            {
                Program.MutexHelper.Wait();

                cdc = ffmpeg.avcodec_open2(_videoCodecContext, _videoCodec, null);
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
            if (cdc >= 0)
                return true;

            fixed (AVCodecContext** ctx = &_videoCodecContext)
            {
                ffmpeg.avcodec_free_context(ctx);
            }
            _videoCodecContext = null;
            return false;
        }

        private void AddVideoStream(AVCodecID codecId)
        {
            GetVideoCodec(codecId);

            _videoStream = ffmpeg.avformat_new_stream(_formatContext, null);
            if (_videoStream == null)
            {
                throw new Exception("Failed creating new video stream.");
            }

            _videoStream->time_base.num = _videoCodecContext->time_base.num;
            _videoStream->time_base.den = _videoCodecContext->time_base.den;

            if (_videoCodecContext->extradata_size > 0)
            {
                _videoStream->codecpar->extradata_size = _videoCodecContext->extradata_size;
                _videoStream->codecpar->extradata = (byte*)ffmpeg.av_malloc((ulong)_videoCodecContext->extradata_size + ffmpeg.AV_INPUT_BUFFER_PADDING_SIZE);

                var j = 0;
                while (j++ < _videoCodecContext->extradata_size)
                {
                    *(_videoStream->codecpar->extradata + j) = *(_videoCodecContext->extradata + j);
                }
            }
        }

        private void OpenAudio()
        {
            var codec = ffmpeg.avcodec_find_encoder(_audioCodecContext->codec_id);

            if (codec == null)
            {
                throw new Exception("Cannot find audio codec.");
            }

            ffmpeg.av_opt_set(_audioCodecContext->priv_data, "tune", "zerolatency", 0);

            var ret = ffmpeg.avcodec_open2(_audioCodecContext, codec, null);

            if (ret < 0)
            {
                throw new Exception("Cannot open audio codec.");
            }

            ffmpeg.avcodec_parameters_from_context(_audioStream->codecpar, _audioCodecContext);

            _audioFrame = ffmpeg.av_frame_alloc();
        }

        private void AddAudioStream(AVCodecID audioCodec)
        {
            var codec = ffmpeg.avcodec_find_encoder(audioCodec);

            _audioStream = ffmpeg.avformat_new_stream(_formatContext, null);

            if (_audioStream == null)
            {
                throw new Exception("Failed creating new audio stream.");
            }

            _audioCodecContext = ffmpeg.avcodec_alloc_context3(codec);

            _audioCodecContext->codec_id = audioCodec;
            _audioCodecContext->codec_type = AVMediaType.AVMEDIA_TYPE_AUDIO;

            _audioCodecContext->sample_fmt = AVSampleFormat.AV_SAMPLE_FMT_FLTP; //AV_SAMPLE_FMT_S16;

            if (audioCodec == AVCodecID.AV_CODEC_ID_MP3)
                _audioCodecContext->sample_fmt = AVSampleFormat.AV_SAMPLE_FMT_S16P;

            _audioCodecContext->sample_rate = 22050;

            _audioCodecContext->channel_layout = (ulong) ffmpeg.av_get_default_channel_layout(1);
            _audioCodecContext->channels = ffmpeg.av_get_channel_layout_nb_channels(_audioCodecContext->channel_layout);

            _audioStream->time_base.num = _audioCodecContext->time_base.num = 1;
            _audioStream->time_base.den = _audioCodecContext->time_base.den = 1000;


            _audioCodecContext->bits_per_raw_sample = 16;

            if ((_formatContext->oformat->flags & ffmpeg.AVFMT_GLOBALHEADER) == ffmpeg.AVFMT_GLOBALHEADER)
            {
                _audioCodecContext->flags |= ffmpeg.AV_CODEC_FLAG_GLOBAL_HEADER;
            }

            if ((codec->capabilities & ffmpeg.AV_CODEC_CAP_EXPERIMENTAL) != 0)
            {
                _audioCodecContext->strict_std_compliance = -2;
            }

            _swrContext = ffmpeg.swr_alloc_set_opts(null,
                ffmpeg.av_get_default_channel_layout(_audioCodecContext->channels),
                _audioCodecContext->sample_fmt,
                _audioCodecContext->sample_rate,
                ffmpeg.av_get_default_channel_layout(_audioCodecContext->channels),
                AVSampleFormat.AV_SAMPLE_FMT_S16,
                _audioCodecContext->sample_rate,
                0,
                null);
            Throw("SWR_INIT", ffmpeg.swr_init(_swrContext));

            _convHandle = GCHandle.Alloc(_convOut, GCHandleType.Pinned);
        }
    }
}