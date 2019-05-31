using System;
using System.Collections.Generic;
using System.Threading;
using FFmpeg.AutoGen;

namespace iSpyApplication.Sources.Audio
{
    public unsafe class AudioReader : IDisposable
    {
        private const int BUFSIZE = 2000000;
        private const int TIMEOUT = 20;

        public readonly int rate = 22050;
        public readonly int channels = 1;

        private int audio_stream_index = -1;
        private AVFormatContext* fmt_ctx = null;
        private AVCodecContext* _audioCodecContext = null;
        private SwrContext* _swrContext = null;
        private AVStream* _audioStream = null;

        private List<Filter> filtersAudio = new List<Filter>();
        private AVFilterContext* buffersrc_ctx = null;
        private AVFilterContext* buffersink_ctx = null;
        private AVFilterGraph* filter_graph = null;

        public AudioReader()
        {
            //ffmpeg.av_register_all();
            //ffmpeg.avfilter_register_all();
        }

        public AudioReader(int rate, int channels) : this()
        {
            this.rate = rate;
            this.channels = channels;
        }

        public List<Filter> AudioFilters
        {
            get { return filtersAudio; }
        }

        public void AddAudioFilter(string name, string args, string key = "")
        {
            filtersAudio.Add(new Filter(name, args, key));
        }

        private AVFilterGraph* init_filter_graph(AVFormatContext* format, AVCodecContext* codec, int audio_stream_index, AVFilterContext** buffersrc_ctx, AVFilterContext** buffersink_ctx)
        {
            // create graph
            var filter_graph = ffmpeg.avfilter_graph_alloc();

            // add input filter
            var abuffersrc = ffmpeg.avfilter_get_by_name("abuffer");
            var args = string.Format("sample_fmt={0}:channel_layout={1}:sample_rate={2}:time_base={3}/{4}",
                (int)codec->sample_fmt,
                codec->channel_layout,
                codec->sample_rate,
                format->streams[audio_stream_index]->time_base.num,
                format->streams[audio_stream_index]->time_base.den);
            ffmpeg.avfilter_graph_create_filter(buffersrc_ctx, abuffersrc, "IN", args, null, filter_graph);

            // add output filter
            var abuffersink = ffmpeg.avfilter_get_by_name("abuffersink");
            ffmpeg.avfilter_graph_create_filter(buffersink_ctx, abuffersink, "OUT", "", null, filter_graph);

            AVFilterContext* _filter_ctx = null;
            for (var i = 0; i < filtersAudio.Count; i++)
            {
                var filter = ffmpeg.avfilter_get_by_name(filtersAudio[i].name);
                AVFilterContext* filter_ctx;
                ffmpeg.avfilter_graph_create_filter(&filter_ctx, filter, (filtersAudio[i].name + filtersAudio[i].key).ToUpper(), filtersAudio[i].args, null, filter_graph);

                if (i == 0)
                {
                    ffmpeg.avfilter_link(*buffersrc_ctx, 0, filter_ctx, 0);
                }
                if (_filter_ctx != null)
                {
                    ffmpeg.avfilter_link(_filter_ctx, 0, filter_ctx, 0);
                }
                if (i == filtersAudio.Count - 1)
                {
                    ffmpeg.avfilter_link(filter_ctx, 0, *buffersink_ctx, 0);
                }

                _filter_ctx = filter_ctx;
            }
            ffmpeg.avfilter_graph_config(filter_graph, null);

            return filter_graph;
        }

        private int open_input(string path)
        {
            try
            {
                int ret;

                var _timeoutMicroSeconds = Math.Max(5000000, TIMEOUT * 1000);

                AVDictionary* options = null;
                if (path.Contains(":"))
                {
                    var prefix = path.ToLower().Substring(0, path.IndexOf(":", StringComparison.Ordinal));
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

                            break;
                        case "rtsp":
                        case "rtmp":
                            ffmpeg.av_dict_set_int(&options, "stimeout", _timeoutMicroSeconds, 0);
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

                fmt_ctx = ffmpeg.avformat_alloc_context();
                fmt_ctx->max_analyze_duration = 0; //0 = auto

                fixed (AVFormatContext** at = &fmt_ctx)
                {
                    ret = ffmpeg.avformat_open_input(at, path, null, &options);
                }

                if (ret < 0)
                {
                    throw new ApplicationException("Failed to open input.");
                }

                ret = ffmpeg.avformat_find_stream_info(fmt_ctx, null);
                if (ret < 0)
                {
                    throw new ApplicationException("Failed to find stream information.");
                }

                AVCodec* dec;
                ret = ffmpeg.av_find_best_stream(fmt_ctx, AVMediaType.AVMEDIA_TYPE_AUDIO, -1, -1, &dec, 0);
                if (ret < 0)
                {
                    throw new ApplicationException("Failed to find an audio stream in input.");
                }

                audio_stream_index = ret;

                _audioCodecContext = fmt_ctx->streams[audio_stream_index]->codec;
                _audioStream = fmt_ctx->streams[audio_stream_index];

                fmt_ctx->flags |= ffmpeg.AVFMT_FLAG_DISCARD_CORRUPT;
                fmt_ctx->flags |= ffmpeg.AVFMT_FLAG_NOBUFFER;

                if (_audioStream != null)
                {
                    var audiocodec = ffmpeg.avcodec_find_decoder(_audioCodecContext->codec_id);
                    if (audiocodec != null)
                    {
                        ffmpeg.av_opt_set_int(_audioCodecContext, "refcounted_frames", 1, 0);

                        /* init the audio decoder */
                        if ((ret = ffmpeg.avcodec_open2(_audioCodecContext, audiocodec, null)) < 0)
                        {
                            throw new ApplicationException("Failed to open audio decoder.");
                        }

                        if (filtersAudio.Count > 0)
                        {
                            fixed (AVFilterContext** bsrc = &buffersrc_ctx)
                            {
                                fixed (AVFilterContext** bsink = &buffersink_ctx)
                                {
                                    // build a filter graph
                                    if ((int)(filter_graph = init_filter_graph(fmt_ctx, _audioCodecContext, audio_stream_index, bsrc, bsink)) == 0)
                                    {
                                        throw new ApplicationException("Failed to create the filter graph.");
                                    }
                                }
                            }
                        }

                        var outlayout = ffmpeg.av_get_default_channel_layout(channels);
                        _audioCodecContext->request_sample_fmt = AVSampleFormat.AV_SAMPLE_FMT_S16;
                        _audioCodecContext->request_channel_layout = (ulong)outlayout;

                        //var chans = 1;
                        //if (_audioCodecContext->channels > 1) //downmix
                        //    chans = 2;

                        _swrContext = ffmpeg.swr_alloc_set_opts(null,
                            outlayout,
                            AVSampleFormat.AV_SAMPLE_FMT_S16,
                            rate,
                            ffmpeg.av_get_default_channel_layout(_audioCodecContext->channels),
                            _audioCodecContext->sample_fmt,
                            _audioCodecContext->sample_rate,
                            0,
                            null);

                        ret = ffmpeg.swr_init(_swrContext);
                        if (ret < 0)
                        {
                            throw new ApplicationException("Failed to create swrContext.");
                        }
                    }
                }

                return ret;
            }
            catch (Exception ex)
            {
                throw ex;
            }

            //return -1;
        }

        public void ReadSamples(string inputAudio, Func<byte[], int, bool> readSampleCallback)
        {
            if (readSampleCallback == null) return;

            const int EAGAIN = 11;

            var brk = false;
            var packet = new AVPacket();

            try
            {
                int ret = open_input(inputAudio);
                if (ret < 0)
                {
                    return;
                }

                byte[] buffer = null, tbuffer = null;

                while (true)
                {
                    ffmpeg.av_init_packet(&packet);

                    if (_audioCodecContext != null && buffer == null)
                    {
                        buffer = new byte[_audioCodecContext->sample_rate * 2];
                        tbuffer = new byte[_audioCodecContext->sample_rate * 2];
                    }

                    ret = ffmpeg.av_read_frame(fmt_ctx, &packet);
                    if (ret < 0)
                    {
                        break;
                    }

                    if ((packet.flags & ffmpeg.AV_PKT_FLAG_CORRUPT) == ffmpeg.AV_PKT_FLAG_CORRUPT)
                    {
                        break;
                    }

                    if (packet.stream_index == audio_stream_index)
                    {
                        var s = 0;
                        fixed (byte** outPtrs = new byte*[32])
                        {
                            fixed (byte* bPtr = &tbuffer[0])
                            {
                                outPtrs[0] = bPtr;

                                AVFrame* _af = null;
                                var af = ffmpeg.av_frame_alloc();
                                var ff = ffmpeg.av_frame_alloc();

                                ffmpeg.avcodec_send_packet(_audioCodecContext, &packet);
                                do
                                {
                                    ret = ffmpeg.avcodec_receive_frame(_audioCodecContext, af);
                                    if (ret == 0)
                                    {
                                        if (filter_graph != null)
                                        {
                                            // add the frame into the filter graph
                                            ffmpeg.av_buffersrc_add_frame(buffersrc_ctx, af);

                                            // get the frame out from the filter graph
                                            ret = ffmpeg.av_buffersink_get_frame(buffersink_ctx, ff);

                                            if (ret == -EAGAIN)
                                                break;

                                            _af = ff;
                                        }
                                        else
                                        {
                                            _af = af;
                                        }

                                        fixed (byte** datptr = _af->data.ToArray())
                                        {
                                            var numSamplesOut = ffmpeg.swr_convert(_swrContext,
                                                outPtrs,
                                                _audioCodecContext->sample_rate,
                                                datptr,
                                                _af->nb_samples);

                                            if (numSamplesOut > 0)
                                            {
                                                var l = numSamplesOut * 2 * channels;
                                                Buffer.BlockCopy(tbuffer, 0, buffer, s, l);
                                                s += l;
                                            }
                                            else
                                            {
                                                ret = numSamplesOut; //(error)
                                            }
                                        }

                                        if (_af->decode_error_flags > 0) break;
                                    }

                                } while (ret == 0);
                                ffmpeg.av_frame_free(&ff);
                                ffmpeg.av_frame_free(&af);

                                if (s > 0)
                                {
                                    var ba = new byte[s];
                                    Buffer.BlockCopy(buffer, 0, ba, 0, s);

                                    if (readSampleCallback(ba, s))
                                    {
                                        brk = true;
                                        break;
                                    }
                                }
                            }
                        }
                    }
                    ffmpeg.av_packet_unref(&packet);

                    if (ret == -EAGAIN)
                    {
                        Thread.Sleep(10);
                    }

                    if (brk) break;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (_audioCodecContext != null)
                {
                    ffmpeg.avcodec_close(_audioCodecContext);
                }

                if (fmt_ctx != null)
                {
                    fixed (AVFormatContext** at = &fmt_ctx)
                    {
                        ffmpeg.avformat_close_input(at);
                    }
                }

                fmt_ctx = null;
                _audioCodecContext = null;
                _audioStream = null;

                if (_swrContext != null)
                {
                    fixed (SwrContext** s = &_swrContext)
                    {
                        ffmpeg.swr_free(s);
                    }

                    _swrContext = null;
                }

                if (filter_graph != null)
                {
                    fixed (AVFilterGraph** f = &filter_graph)
                    {
                        ffmpeg.avfilter_graph_free(f);
                    }

                    filter_graph = null;
                    buffersink_ctx = null;
                    buffersrc_ctx = null;

                    filtersAudio.Clear();
                }
            }
        }

        public void Dispose()
        {
        }

        public class Filter
        {
            public readonly string name = "";
            public readonly string args = "";
            public readonly string key = "";

            public Filter(string name, string args, string key = "")
            {
                this.name = name;
                this.args = args;
                this.key = key;
            }
        }
    }
}