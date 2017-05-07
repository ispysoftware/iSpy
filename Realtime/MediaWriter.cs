﻿using System;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.Globalization;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using FFmpeg.AutoGen;
using iSpyApplication.Utilities;

namespace iSpyApplication.Realtime
{
    public unsafe class MediaWriter
    {
        private AVCodec* _videoCodec;
        private AVFormatContext* _formatContext;
        private AVCodecContext* _videoCodecContext;
        private AVCodecContext* _audioCodecContext;
        private AVStream* _videoStream, _audioStream;
        private AVFrame* _audioFrame, _videoFrame;
        private SwsContext* _swsContext;
        private SwrContext* _swrContext;
        private AVPixelFormat _avPixelFormat = AVPixelFormat.AV_PIX_FMT_YUV420P;
        private const string Extension = ".mp4";

        private bool _isConstantFramerate;
        private int _width, _height, _framerate;//_bitRate
        private string movflags = "";//"faststart";
        private int _audioBufferSizeCurrent;
        private long _lastAudioPts;
        private byte[] _audioBuffer = new byte[44100];
        public DateTime CreatedDate;
        public bool IsStreaming = false;

        private static bool Hwnvidia = true;
        private static bool _hwqsv = true;

        private IntPtr _interruptCallbackAddress;
        private AvInterruptCb _interruptCallback;

        [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public delegate int AvInterruptCb(void* ctx);

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

        // ReSharper disable once PrivateFieldCanBeConvertedToLocalVariable
        private DateTime _lastPacket;
        private bool _abort;
        private bool _opened;
        private int _frameNumber;
        private double _maxLevel = -1;
        private bool _isVideo;

        public int Timeout = 5000;

        public delegate int InterruptCallback();

        public string Filename;
        private readonly StringBuilder _alertData = new StringBuilder();

        public string AlertData
        {
            get { return Helper.GetLevelDataPoints(_alertData); }
        }
        public double MaxAlarm, TriggerLevel, TriggerLevelMax;
        public long SizeBytes;
        public Bitmap MaxFrame;
        public bool IsTimelapse;

        public int Duration
        {
            get { return (int)(_recordingEndTime - _recordingStartTime).TotalSeconds; }
        }
        private DateTime _recordingStartTime;
        private DateTime _recordingEndTime;

        public bool Closed => !_opened;

        public int InterruptCb()
        {
            if ((DateTime.UtcNow - _lastPacket).TotalMilliseconds > Timeout || _abort)
            {
                return 1;
            }
            return 0;
        }

        public MediaWriter(string fileName, AVCodecID audioCodec)
        {
            Open(fileName, -1,-1,AVCodecID.AV_CODEC_ID_NONE, 0, audioCodec);
        }

        public MediaWriter(string fileName, int width, int height, AVCodecID videoCodec)
        {
            Open(fileName, width, height, videoCodec, 0, AVCodecID.AV_CODEC_ID_NONE);
        }

        public MediaWriter(string fileName, int width, int height, AVCodecID videoCodec, int framerate, AVCodecID audioCodec)
        {
            Open(fileName, width, height, videoCodec, framerate, audioCodec);
        }

        private void Open(string fileName, int width, int height, AVCodecID videoCodec, int framerate, AVCodecID audioCodec)
        {
            CreatedDate = DateTime.UtcNow;
            Filename = fileName;
            _abort = false;

            if (videoCodec != AVCodecID.AV_CODEC_ID_NONE)
            {
                IsTimelapse = framerate != 0;

                if (((width & 1) != 0) || ((height & 1) != 0))
                {
                    throw new ArgumentException("Video file resolution must be a multiple of two.");
                }
            }
            else
            {
                _isVideo = false;
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

            _formatContext->interrupt_callback.callback = _interruptCallbackAddress;
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
                _isConstantFramerate = framerate>0;

                AddVideoStream(videoCodec);
                OpenVideo();

                if (videoCodec == AVCodecID.AV_CODEC_ID_MPEG1VIDEO)
                {
                    ffmpeg.av_dict_set(&opts, "pkt_size", "1316", 0);
                    ffmpeg.av_dict_set(&opts, "buffer_size", "65535", 0);

                    //ffmpeg.av_dict_set(&opts, "crf", "30", 0);
                }
            }
            

            if (movflags!="")
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
            _recordingStartTime = DateTime.UtcNow;
            _opened = true;
        }


        public void Close()
        {
            
            try
            {
                Program.FfmpegMutex.WaitOne();
                _recordingEndTime = DateTime.UtcNow;
                if (_formatContext != null)
                {
                    if (!IsStreaming)
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
                        var pinprt = &(_formatContext->pb);
                        ffmpeg.avio_closep(pinprt);
                        _formatContext->pb = null;
                    }

                    _audioBuffer = null;


                    if (_videoFrame != null)
                    {
                        ffmpeg.avpicture_free((AVPicture*) _videoFrame);
                        fixed (AVFrame** pinprt = &(_videoFrame))
                        {
                            ffmpeg.av_frame_free(pinprt);
                            _videoFrame = null;
                        }
                    }

                    if (_audioFrame != null)
                    {
                        fixed (AVFrame** pinprt = &(_audioFrame))
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
                        int j = (int) _formatContext->nb_streams;
                        for (var i = j - 1; i >= 0; i--)
                        {
                            AVStream* stream = _formatContext->streams[i];
                            if (stream != null && stream->codec != null && stream->codec->codec != null)
                            {
                                stream->discard = AVDiscard.AVDISCARD_ALL;
                                ffmpeg.av_freep(&stream);
                            }
                        }
                    }


                    _videoStream = null;
                    _audioStream = null;

                    fixed (AVFormatContext** pinprt = &(_formatContext))
                    {
                        ffmpeg.av_freep(pinprt);
                    }
                    _formatContext = null;

                }

                if (_swsContext != null)
                {
                    ffmpeg.sws_freeContext(_swsContext);
                    _swsContext = null;
                }
                if (_swrContext != null)
                {
                    ffmpeg.swr_close(_swrContext);
                    _swrContext = null;
                }


                try
                {
                    FileInfo fi = new FileInfo(Filename);
                    SizeBytes = fi.Length;
                }
                catch
                {
                    SizeBytes = 0;
                }


                _opened = false;
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
        }
        
        public void WriteAudio(byte[] soundBuffer, int soundBufferSize, int level, long msOffset)
        {
            if (!_opened)
            {
                throw new Exception("An audio file was not opened yet.");
            }
            AddAudioSamples(soundBuffer, soundBufferSize, msOffset);

            if (!_isVideo)
            {
                _alertData.Append(string.Format(CultureInfo.InvariantCulture, "{0:0.000},", Math.Min(level, 100)));
                if (level>MaxAlarm)
                    MaxAlarm = level;
            }
        }

        public void WriteFrame(Bitmap frame, long msOffset)
        {
            WriteFrame(frame,0, msOffset);
        }

        public void WriteFrame(Bitmap frame, int level, long msOffset)
        {
            if (!_opened)
            {
                throw new Exception("An audio file was not opened yet.");
            }
            if (ffmpeg.avcodec_is_open(_videoCodecContext)<=0)
                throw new Exception("codec is not open");

            if ((frame.Width != _videoCodecContext->width) || (frame.Height != _videoCodecContext->height))
            {
                throw new Exception("Bitmap size must be of the same as video size, which was specified on opening video file.");
            }

            BitmapData bitmapData = frame.LockBits(new Rectangle(0, 0, _width,_height), ImageLockMode.ReadOnly,
        (frame.PixelFormat == PixelFormat.Format8bppIndexed) ? PixelFormat.Format8bppIndexed : PixelFormat.Format24bppRgb);

            var ptr = (sbyte*)bitmapData.Scan0;
            int srcLinesize = bitmapData.Stride;

            if (_swsContext==null)
            {
                AVPixelFormat pfmt = AVPixelFormat.AV_PIX_FMT_BGR24;

                if (frame.PixelFormat == PixelFormat.Format8bppIndexed)
                {
                    pfmt = AVPixelFormat.AV_PIX_FMT_GRAY8;
                }

                _swsContext = ffmpeg.sws_getCachedContext(_swsContext, _videoCodecContext->width, _videoCodecContext->height, pfmt, _videoCodecContext->width, _videoCodecContext->height, _videoCodecContext->pix_fmt, ffmpeg.SWS_FAST_BILINEAR, null, null, null);
            }
            int h = ffmpeg.sws_scale(_swsContext, &ptr, &srcLinesize, 0, _videoCodecContext->height, &_videoFrame->data0, _videoFrame->linesize);

            frame.UnlockBits(bitmapData);

            if (h <= 0)
            {
                throw new Exception("Error scaling image");

            }
            

            if (!_isConstantFramerate)
            {
                var pts = msOffset;
                _videoFrame->pts = pts;
            }
            else
            {
                _videoFrame->pts = _frameNumber;
            }
            _frameNumber++;


            int ret;
            AVPacket packet = new AVPacket();
            ffmpeg.av_init_packet(&packet);

            if ((_formatContext->oformat->flags & ffmpeg.AVFMT_RAWPICTURE)==ffmpeg.AVFMT_RAWPICTURE)
            {
                packet.flags |= ffmpeg.AV_PKT_FLAG_KEY;
                packet.stream_index = _videoStream->index;
                packet.data = _videoFrame->data0;
                packet.size = sizeof(AVPicture);
                ret = ffmpeg.av_interleaved_write_frame(_formatContext, &packet);
            }
            else
            {

                int gotPacket;
                packet.data = null;
                packet.size = 0;

                ret = ffmpeg.avcodec_encode_video2(_videoCodecContext, &packet, _videoFrame, &gotPacket);
                if (ret < 0)
                {
                    ffmpeg.av_free_packet(&packet);
                    throw new Exception("Error while writing video frame (" + ret + ")");
                }

                if (gotPacket>0 && packet.size>0)
                {
                    if ((ulong)packet.pts != ffmpeg.AV_NOPTS_VALUE)
                        packet.pts = ffmpeg.av_rescale_q(packet.pts, _videoCodecContext->time_base, _videoStream->time_base);
                    if ((ulong)packet.dts != ffmpeg.AV_NOPTS_VALUE)
                        packet.dts = ffmpeg.av_rescale_q(packet.dts, _videoCodecContext->time_base, _videoStream->time_base);

                    packet.stream_index = _videoStream->index;
                    // write the compressed frame to the media file
                    _lastPacket = DateTime.UtcNow;
                    ret = ffmpeg.av_write_frame(_formatContext, &packet);
                }
            }
            ffmpeg.av_free_packet(&packet);

            _alertData.Append(string.Format(CultureInfo.InvariantCulture, "{0:0.000},", Math.Min(level,100)));

            if (level > _maxLevel)
            {
                MaxAlarm = level;
                MaxFrame?.Dispose();
                MaxFrame = (Bitmap)frame.Clone();
                _maxLevel = level;
            }

            if (ret != 0)
            {
                throw new Exception("Error while writing video frame (" + ret + ")");
            }

        }

        private void Flush()
        {
            if (_opened)
            {
                if (_videoStream!=null && _videoCodecContext!=null)
                {
                    while (ffmpeg.avcodec_is_open(_videoCodecContext)>0)
                    {
                        _lastPacket = DateTime.UtcNow;
                        AVPacket packet = new AVPacket();
                        ffmpeg.av_init_packet(&packet);

                        int gotPacket;
                        var ret = ffmpeg.avcodec_encode_video2(_videoCodecContext, &packet, null, &gotPacket);
                        if (ret < 0 || gotPacket<=0)
                        {
                            ffmpeg.av_free_packet(&packet);
                            break;
                        }

                        if (packet.size>0)
                        {
                            if ((ulong)packet.pts != ffmpeg.AV_NOPTS_VALUE)
                                packet.pts = ffmpeg.av_rescale_q(packet.pts, _videoCodecContext->time_base, _videoStream->time_base);
                            if ((ulong)packet.dts != ffmpeg.AV_NOPTS_VALUE)
                                packet.dts = ffmpeg.av_rescale_q(packet.dts, _videoCodecContext->time_base, _videoStream->time_base);

                            packet.stream_index = _videoStream->index;
                            // write the compressed frame to the media file

                            ret = ffmpeg.av_interleaved_write_frame(_formatContext, &packet);
                            if (ret < 0)
                            {
                                ffmpeg.av_free_packet(&packet);
                                break;
                            }
                        }

                        ffmpeg.av_free_packet(&packet);
                    }
                    //ffmpeg.avcodec_flush_buffers(_videoCodecContext);

                }
                if (_audioStream!=null && _audioCodecContext!=null)
                {
                    while (ffmpeg.avcodec_is_open(_audioCodecContext)>0)
                    {
                        _lastPacket = DateTime.UtcNow;
                        AVPacket packet = new AVPacket();
                        ffmpeg.av_init_packet(&packet);

                        int gotPacket;

                        var ret = ffmpeg.avcodec_encode_audio2(_audioCodecContext, &packet, null, &gotPacket);

                        if (ret < 0 || gotPacket<=0)
                        {
                            ffmpeg.av_free_packet(&packet);
                            break;
                        }

                        if (packet.size>0)
                        {
                            if ((ulong)packet.pts != ffmpeg.AV_NOPTS_VALUE)
                                packet.pts = ffmpeg.av_rescale_q(packet.pts, _audioCodecContext->time_base, _audioStream->time_base);
                            if ((ulong)packet.dts != ffmpeg.AV_NOPTS_VALUE)
                                packet.dts = ffmpeg.av_rescale_q(packet.dts, _audioCodecContext->time_base, _audioStream->time_base);

                            packet.stream_index = _audioStream->index;
                            packet.flags |= ffmpeg.AV_PKT_FLAG_KEY;

                            ret = ffmpeg.av_interleaved_write_frame(_formatContext, &packet);

                            if (ret < 0)
                            {
                                ffmpeg.av_free_packet(&packet);
                                break;
                            }
                        }
                        ffmpeg.av_free_packet(&packet);
                    }
                    //ffmpeg.avcodec_flush_buffers(_audioCodecContext);

                }

            }
        }

        private void AddAudioSamples(byte[] soundBuffer, int soundBufferSize, long msOffset)
        {
            if (_audioStream==null || _audioCodecContext==null || soundBufferSize <= 0)
                return;

            int size = ffmpeg.av_samples_get_buffer_size(null, _audioCodecContext->channels,
                                                      _audioCodecContext->frame_size,
                                                      AVSampleFormat.AV_SAMPLE_FMT_S16, 0);

            ffmpeg.av_frame_unref(_audioFrame);

            _audioFrame->nb_samples = _audioCodecContext->frame_size;

            AVPacket packet = new AVPacket();           

            Buffer.BlockCopy(soundBuffer, 0, _audioBuffer, _audioBufferSizeCurrent, soundBufferSize);
            _audioBufferSizeCurrent += soundBufferSize;

            int remaining = _audioBufferSizeCurrent, cursor = 0;

            fixed (byte* p = _audioBuffer)
            {
                sbyte* inPointerLocal = (sbyte*)p;
                var pts = msOffset;
                while (remaining >= size)
                {
                    ffmpeg.av_init_packet(&packet);
                    int ret;
                    sbyte[] convOut = new sbyte[10000];
                    
                    fixed (sbyte* convOutPointer = convOut)
                    {
                        sbyte* convOutPointerLocal = convOutPointer;
                        int dstNbSamples =
                            (int)
                                ffmpeg.av_rescale_rnd(
                                    ffmpeg.swr_get_delay(_swrContext, _audioCodecContext->sample_rate) +
                                    _audioFrame->nb_samples, _audioCodecContext->sample_rate,
                                    _audioCodecContext->sample_rate,
                                    AVRounding.AV_ROUND_UP);

                        ret = ffmpeg.swr_convert(_swrContext,
                            &convOutPointerLocal,
                            dstNbSamples,
                            &inPointerLocal,
                            _audioFrame->nb_samples);

                        if (ret < 0)
                        {
                            throw new Exception("Error while converting audio format (" + ret + ")");
                        }

                        _audioFrame->nb_samples = dstNbSamples;

                        _audioFrame->pts = pts;

                        //if (_lastAudioPts > pts)
                        //{
                        //    _audioFrame->pts = _lastAudioPts+1;
                        //}
                        //_lastAudioPts = _audioFrame->pts;

                        var dstSamplesSize = ffmpeg.av_samples_get_buffer_size(null, _audioCodecContext->channels,
                            _audioFrame->nb_samples,
                            _audioCodecContext->sample_fmt, 0);

                        ret = ffmpeg.avcodec_fill_audio_frame(_audioFrame, _audioCodecContext->channels,
                            _audioCodecContext->sample_fmt, convOutPointer, dstSamplesSize, 0);
                        inPointerLocal += size;
                    }

                    if (ret < 0)
                    {
                        ffmpeg.av_free_packet(&packet);
                        throw new Exception("error filling audio");
                    }

                    int gotPacket;

                    ret = ffmpeg.avcodec_encode_audio2(_audioCodecContext, &packet, _audioFrame, &gotPacket);
                    pts++;
                    if (ret < 0)
                    {
                        ffmpeg.av_free_packet(&packet);
                        throw new Exception("Error while encoding audio frame (" + ret + ")");
                    }

                    if (gotPacket > 0 && packet.size > 0)
                    {
                        if ((ulong) packet.pts != ffmpeg.AV_NOPTS_VALUE)
                            packet.pts = ffmpeg.av_rescale_q(packet.pts, _audioCodecContext->time_base, _audioStream->time_base);
                        if ((ulong) packet.dts != ffmpeg.AV_NOPTS_VALUE)
                            packet.dts = ffmpeg.av_rescale_q(packet.dts, _audioCodecContext->time_base, _audioStream->time_base);

                        packet.stream_index = _audioStream->index;
                        packet.flags |= ffmpeg.AV_PKT_FLAG_KEY;
                        _lastPacket = DateTime.UtcNow;
                        
                        if (ffmpeg.av_interleaved_write_frame(_formatContext, &packet) != 0)
                        {
                            ffmpeg.av_free_packet(&packet);
                            throw new Exception("unable to write audio frame.");
                        }
                        ffmpeg.av_free_packet(&packet);
                    }

                    cursor += size;
                    remaining -= size;
                }
            }

            Buffer.BlockCopy(_audioBuffer, cursor, _audioBuffer, 0, remaining);
            _audioBufferSizeCurrent = remaining;

        }

        void OpenVideo( )
        {
            _isVideo = true;
            _maxLevel = -1;
            
            
            

            ffmpeg.avcodec_parameters_from_context(_videoStream->codecpar, _videoCodecContext);
            
            _videoFrame = ffmpeg.av_frame_alloc();
            if (ffmpeg.avpicture_alloc((AVPicture*)_videoFrame, _videoCodecContext->pix_fmt, _videoCodecContext->width, _videoCodecContext->height) < 0)
            {
                ffmpeg.avpicture_free((AVPicture*)_videoFrame);
                throw new Exception("Cannot allocate video picture.");
            }

            _videoFrame->width = _videoCodecContext->width;
            _videoFrame->height = _videoCodecContext->height;
            _videoFrame->format = (int)_videoCodecContext->pix_fmt;
        }

        private void GetVideoCodec(AVCodecID baseCodec)
        {
            if (baseCodec == AVCodecID.AV_CODEC_ID_H264)
            {
                if (Hwnvidia && MainForm.Conf.GPU.nVidia)
                {
                    _avPixelFormat = AVPixelFormat.AV_PIX_FMT_YUV420P;
                    _videoCodec = ffmpeg.avcodec_find_encoder_by_name("nvenc_h264");
                    if (_videoCodec != null)
                    {
                        if (TryOpenVideoCodec(baseCodec))
                        {
                            Logger.LogMessage("using Nvidia hardware encoder");
                            return;
                        }
                    }
                }

                if (_hwqsv && MainForm.Conf.GPU.QuickSync)
                {
                    _avPixelFormat = AVPixelFormat.AV_PIX_FMT_NV12;
                    _videoCodec = ffmpeg.avcodec_find_encoder_by_name("h264_qsv");
                    if (_videoCodec != null)
                    {
                        if (TryOpenVideoCodec(baseCodec))
                        {
                            Logger.LogMessage("using Intel QSV hardware encoder");
                            return;
                        }
                        Logger.LogMessage("Install Intel Media Server Studio and restart iSpy to use QSV");
                        _hwqsv = false;
                    }
                }
            }

            _avPixelFormat = AVPixelFormat.AV_PIX_FMT_YUV420P;
            _videoCodec = ffmpeg.avcodec_find_encoder(baseCodec);

            if (TryOpenVideoCodec(baseCodec))
            {
                Logger.LogMessage("using software encoder");
                return;
            }

            Logger.LogMessage("could not open any encoder codec");
            throw new Exception("Failed opening any codec");
        }

        private bool TryOpenVideoCodec(AVCodecID baseCodec)
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

            _videoCodecContext->pix_fmt = _avPixelFormat;
            
            //ffmpeg.av_opt_set(_videoCodecContext->priv_data, "tune", "zerolatency", 0);

            switch (_videoCodecContext->codec_id)
            {
                case AVCodecID.AV_CODEC_ID_MPEG1VIDEO:
                    _videoCodecContext->bit_rate = 700000;
                    /* frames per second */
                    //_videoCodecContext->gop_size = 12; /* emit one intra frame every ten frames */
                    _videoCodecContext->max_b_frames = 0;
                    //ffmpeg.av_opt_set(_videoCodecContext->priv_data, "crf", "30.0", 0);
                    //ffmpeg.av_opt_set(_videoCodecContext->priv_data, "preset", "slow", 0);
                    ffmpeg.av_opt_set(_videoCodecContext->priv_data, "pkt_size", "1316",0);
                    break;
                case AVCodecID.AV_CODEC_ID_H264:
                    ffmpeg.av_opt_set(_videoCodecContext->priv_data, "profile", "main", 0);
                    ffmpeg.av_opt_set(_videoCodecContext->priv_data, "preset", "slow", 0);
                    _videoCodecContext->qmin = 16;
                    _videoCodecContext->qmax = 26;
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
                _videoCodecContext->flags |= ffmpeg.CODEC_FLAG_GLOBAL_HEADER;
            }

            int cdc;
            try
            {
                Program.FfmpegMutex.WaitOne();

                cdc = ffmpeg.avcodec_open2(_videoCodecContext, _videoCodec, null);
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
            if (cdc >= 0)
                return true;

            fixed (AVCodecContext** ctx = &_videoCodecContext)
            {
                ffmpeg.avcodec_free_context(ctx);
            }
            _videoCodecContext = null;
            return false;
        }
        void AddVideoStream(AVCodecID codecId)
        {
            GetVideoCodec(codecId);

            _videoStream = ffmpeg.avformat_new_stream(_formatContext, null);
            if (_videoStream == null)
            {
                throw new Exception("Failed creating new video stream.");
            }

            _videoStream->time_base.num = _videoCodecContext->time_base.num;
            _videoStream->time_base.den = _videoCodecContext->time_base.den;
        }

        void OpenAudio()
        {
            var codec = ffmpeg.avcodec_find_encoder(_audioCodecContext->codec_id);

            if (codec == null)
            {
                throw new Exception("Cannot find audio codec.");
            }

            ffmpeg.av_opt_set(_audioCodecContext->priv_data, "tune", "zerolatency", 0);

            int ret = ffmpeg.avcodec_open2(_audioCodecContext, codec, null);

            if (ret < 0)
            {
                throw new Exception("Cannot open audio codec.");
            }

            ffmpeg.avcodec_parameters_from_context(_audioStream->codecpar, _audioCodecContext);

            _audioFrame = ffmpeg.av_frame_alloc();           
        }

        void AddAudioStream(AVCodecID audioCodec)
        {
            var codec = ffmpeg.avcodec_find_encoder(audioCodec);

            _audioStream = ffmpeg.avformat_new_stream(_formatContext, null);

	        if ( _audioStream == null )
	        {
			    throw new Exception( "Failed creating new audio stream." );
	        }

            _audioCodecContext = ffmpeg.avcodec_alloc_context3(codec);

            _audioCodecContext->codec_id = audioCodec;
            _audioCodecContext->codec_type = AVMediaType.AVMEDIA_TYPE_AUDIO;

            _audioCodecContext->sample_fmt = AVSampleFormat.AV_SAMPLE_FMT_FLTP;//AV_SAMPLE_FMT_S16;

            if (audioCodec == AVCodecID.AV_CODEC_ID_MP3)
                _audioCodecContext->sample_fmt = AVSampleFormat.AV_SAMPLE_FMT_S16P;

            _audioCodecContext->sample_rate = 22050;

            _audioCodecContext->channel_layout = (ulong)ffmpeg.av_get_default_channel_layout(1);
            _audioCodecContext->channels = ffmpeg.av_get_channel_layout_nb_channels(_audioCodecContext->channel_layout);

            _audioStream->time_base.num = _audioCodecContext->time_base.num = 1;
            _audioStream->time_base.den = _audioCodecContext->time_base.den = 1000;


            _audioCodecContext->bits_per_raw_sample = 16;

            if ((_formatContext->oformat->flags & ffmpeg.AVFMT_GLOBALHEADER)== ffmpeg.AVFMT_GLOBALHEADER)
            {
                _audioCodecContext->flags |= ffmpeg.CODEC_FLAG_GLOBAL_HEADER;
            }

            if ((codec->capabilities & ffmpeg.CODEC_CAP_EXPERIMENTAL) != 0)
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
            int ret = ffmpeg.swr_init(_swrContext);
            if (ret<0)
                throw new Exception("Failed to initialise audio conversion context.");
        }

    }
}
