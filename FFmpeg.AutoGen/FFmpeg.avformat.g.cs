using System;
using System.Runtime.InteropServices;

namespace FFmpeg.AutoGen
{
    public unsafe partial struct AVBuffer
    {
    }
    
    public unsafe partial struct AVBufferPool
    {
    }
    
    public unsafe partial struct AVBPrint
    {
    }
    
    public unsafe partial struct AVDictionary
    {
    }
    
    public unsafe partial struct AVCodecInternal
    {
    }
    
    public unsafe partial struct AVCodecDefault
    {
    }
    
    public unsafe partial struct MpegEncContext
    {
    }
    
    public unsafe partial struct ReSampleContext
    {
    }
    
    public unsafe partial struct AVResampleContext
    {
    }
    
    public unsafe partial struct AVBSFInternal
    {
    }
    
    public unsafe partial struct AVBSFList
    {
    }
    
    public unsafe partial struct AVIOInterruptCB
    {
        public IntPtr @callback;
        public void* @opaque;
    }
    
    public unsafe partial struct AVIODirEntry
    {
        public sbyte* @name;
        public int @type;
        public int @utf8;
        public long @size;
        public long @modification_timestamp;
        public long @access_timestamp;
        public long @status_change_timestamp;
        public long @user_id;
        public long @group_id;
        public long @filemode;
    }
    
    public unsafe partial struct AVIODirContext
    {
        public URLContext* @url_context;
    }
    
    public unsafe partial struct URLContext
    {
    }
    
    public unsafe partial struct AVIOContext
    {
        public AVClass* @av_class;
        public sbyte* @buffer;
        public int @buffer_size;
        public sbyte* @buf_ptr;
        public sbyte* @buf_end;
        public void* @opaque;
        public IntPtr @read_packet;
        public IntPtr @write_packet;
        public IntPtr @seek;
        public long @pos;
        public int @must_flush;
        public int @eof_reached;
        public int @write_flag;
        public int @max_packet_size;
        public int @checksum;
        public sbyte* @checksum_ptr;
        public IntPtr @update_checksum;
        public int @error;
        public IntPtr @read_pause;
        public IntPtr @read_seek;
        public int @seekable;
        public long @maxsize;
        public int @direct;
        public long @bytes_read;
        public int @seek_count;
        public int @writeout_count;
        public int @orig_buffer_size;
        public int @short_seek_threshold;
        public sbyte* @protocol_whitelist;
        public sbyte* @protocol_blacklist;
        public IntPtr @write_data_type;
        public int @ignore_boundary_point;
        public AVIODataMarkerType @current_type;
        public long @last_time;
    }
    
    public unsafe partial struct AVBPrint
    {
    }
    
    public unsafe partial struct AVFormatContext
    {
    }
    
    public unsafe partial struct AVDeviceInfoList
    {
    }
    
    public unsafe partial struct AVDeviceCapabilitiesQuery
    {
    }
    
    public unsafe partial struct AVFrac
    {
        public long @val;
        public long @num;
        public long @den;
    }
    
    public unsafe partial struct AVCodecTag
    {
    }
    
    public unsafe partial struct AVProbeData
    {
        public sbyte* @filename;
        public sbyte* @buf;
        public int @buf_size;
        public sbyte* @mime_type;
    }
    
    public unsafe partial struct AVOutputFormat
    {
        public sbyte* @name;
        public sbyte* @long_name;
        public sbyte* @mime_type;
        public sbyte* @extensions;
        public AVCodecID @audio_codec;
        public AVCodecID @video_codec;
        public AVCodecID @subtitle_codec;
        public int @flags;
        public AVCodecTag** @codec_tag;
        public AVClass* @priv_class;
        public AVOutputFormat* @next;
        public int @priv_data_size;
        public IntPtr @write_header;
        public IntPtr @write_packet;
        public IntPtr @write_trailer;
        public IntPtr @interleave_packet;
        public IntPtr @query_codec;
        public IntPtr @get_output_timestamp;
        public IntPtr @control_message;
        public IntPtr @write_uncoded_frame;
        public IntPtr @get_device_list;
        public IntPtr @create_device_capabilities;
        public IntPtr @free_device_capabilities;
        public AVCodecID @data_codec;
        public IntPtr @init;
        public IntPtr @deinit;
        public IntPtr @check_bitstream;
    }
    
    public unsafe partial struct AVInputFormat
    {
        public sbyte* @name;
        public sbyte* @long_name;
        public int @flags;
        public sbyte* @extensions;
        public AVCodecTag** @codec_tag;
        public AVClass* @priv_class;
        public sbyte* @mime_type;
        public AVInputFormat* @next;
        public int @raw_codec_id;
        public int @priv_data_size;
        public IntPtr @read_probe;
        public IntPtr @read_header;
        public IntPtr @read_packet;
        public IntPtr @read_close;
        public IntPtr @read_seek;
        public IntPtr @read_timestamp;
        public IntPtr @read_play;
        public IntPtr @read_pause;
        public IntPtr @read_seek2;
        public IntPtr @get_device_list;
        public IntPtr @create_device_capabilities;
        public IntPtr @free_device_capabilities;
    }
    
    public unsafe partial struct AVIndexEntry
    {
        public long @pos;
        public long @timestamp;
        public int @flags;
        public int @size;
        public int @min_distance;
    }
    
    public unsafe partial struct AVStreamInternal
    {
    }
    
    public unsafe partial struct AVStream
    {
        public int @index;
        public int @id;
        public AVCodecContext* @codec;
        public void* @priv_data;
        public AVFrac @pts;
        public AVRational @time_base;
        public long @start_time;
        public long @duration;
        public long @nb_frames;
        public int @disposition;
        public AVDiscard @discard;
        public AVRational @sample_aspect_ratio;
        public AVDictionary* @metadata;
        public AVRational @avg_frame_rate;
        public AVPacket @attached_pic;
        public AVPacketSideData* @side_data;
        public int @nb_side_data;
        public int @event_flags;
        public info* @info;
        public int @pts_wrap_bits;
        public long @first_dts;
        public long @cur_dts;
        public long @last_IP_pts;
        public int @last_IP_duration;
        public int @probe_packets;
        public int @codec_info_nb_frames;
        public AVStreamParseType @need_parsing;
        public AVCodecParserContext* @parser;
        public AVPacketList* @last_in_packet_buffer;
        public AVProbeData @probe_data;
        public fixed long @pts_buffer[17]; 
        public AVIndexEntry* @index_entries;
        public int @nb_index_entries;
        public uint @index_entries_allocated_size;
        public AVRational @r_frame_rate;
        public int @stream_identifier;
        public long @interleaver_chunk_size;
        public long @interleaver_chunk_duration;
        public int @request_probe;
        public int @skip_to_keyframe;
        public int @skip_samples;
        public long @start_skip_samples;
        public long @first_discard_sample;
        public long @last_discard_sample;
        public int @nb_decoded_frames;
        public long @mux_ts_offset;
        public long @pts_wrap_reference;
        public int @pts_wrap_behavior;
        public int @update_initial_durations_done;
        public fixed long @pts_reorder_error[17]; 
        public fixed sbyte @pts_reorder_error_count[17]; 
        public long @last_dts_for_order_check;
        public sbyte @dts_ordered;
        public sbyte @dts_misordered;
        public int @inject_global_side_data;
        public sbyte* @recommended_encoder_configuration;
        public AVRational @display_aspect_ratio;
        public FFFrac* @priv_pts;
        public AVStreamInternal* @internal;
        public AVCodecParameters* @codecpar;
    }
    
    public unsafe partial struct FFFrac
    {
    }
    
    public unsafe partial struct AVPacketList
    {
    }
    
    public unsafe partial struct info
    {
        public long @last_dts;
        public long @duration_gcd;
        public int @duration_count;
        public long @rfps_duration_sum;
        public IntPtr @duration_error;
        public long @codec_info_duration;
        public long @codec_info_duration_fields;
        public int @found_decoder;
        public long @last_duration;
        public long @fps_first_dts;
        public int @fps_first_dts_idx;
        public long @fps_last_dts;
        public int @fps_last_dts_idx;
    }
    
    public unsafe partial struct AVProgram
    {
        public int @id;
        public int @flags;
        public AVDiscard @discard;
        public uint* @stream_index;
        public uint @nb_stream_indexes;
        public AVDictionary* @metadata;
        public int @program_num;
        public int @pmt_pid;
        public int @pcr_pid;
        public long @start_time;
        public long @end_time;
        public long @pts_wrap_reference;
        public int @pts_wrap_behavior;
    }
    
    public unsafe partial struct AVChapter
    {
        public int @id;
        public AVRational @time_base;
        public long @start;
        public long @end;
        public AVDictionary* @metadata;
    }
    
    public unsafe partial struct AVFormatInternal
    {
    }
    
    public unsafe partial struct AVFormatContext
    {
        public AVClass* @av_class;
        public AVInputFormat* @iformat;
        public AVOutputFormat* @oformat;
        public void* @priv_data;
        public AVIOContext* @pb;
        public int @ctx_flags;
        public uint @nb_streams;
        public AVStream** @streams;
        public fixed sbyte @filename[1024]; 
        public long @start_time;
        public long @duration;
        public long @bit_rate;
        public uint @packet_size;
        public int @max_delay;
        public int @flags;
        public long @probesize;
        public long @max_analyze_duration;
        public sbyte* @key;
        public int @keylen;
        public uint @nb_programs;
        public AVProgram** @programs;
        public AVCodecID @video_codec_id;
        public AVCodecID @audio_codec_id;
        public AVCodecID @subtitle_codec_id;
        public uint @max_index_size;
        public uint @max_picture_buffer;
        public uint @nb_chapters;
        public AVChapter** @chapters;
        public AVDictionary* @metadata;
        public long @start_time_realtime;
        public int @fps_probe_size;
        public int @error_recognition;
        public AVIOInterruptCB @interrupt_callback;
        public int @debug;
        public long @max_interleave_delta;
        public int @strict_std_compliance;
        public int @event_flags;
        public int @max_ts_probe;
        public int @avoid_negative_ts;
        public int @ts_id;
        public int @audio_preload;
        public int @max_chunk_duration;
        public int @max_chunk_size;
        public int @use_wallclock_as_timestamps;
        public int @avio_flags;
        public AVDurationEstimationMethod @duration_estimation_method;
        public long @skip_initial_bytes;
        public uint @correct_ts_overflow;
        public int @seek2any;
        public int @flush_packets;
        public int @probe_score;
        public int @format_probesize;
        public sbyte* @codec_whitelist;
        public sbyte* @format_whitelist;
        public AVFormatInternal* @internal;
        public int @io_repositioned;
        public AVCodec* @video_codec;
        public AVCodec* @audio_codec;
        public AVCodec* @subtitle_codec;
        public AVCodec* @data_codec;
        public int @metadata_header_padding;
        public void* @opaque;
        public IntPtr @control_message_cb;
        public long @output_ts_offset;
        public sbyte* @dump_separator;
        public AVCodecID @data_codec_id;
        public IntPtr @open_cb;
        public sbyte* @protocol_whitelist;
        public IntPtr @io_open;
        public IntPtr @io_close;
        public sbyte* @protocol_blacklist;
    }
    
    public unsafe partial struct AVPacketList
    {
        public AVPacket @pkt;
        public AVPacketList* @next;
    }
    
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public unsafe delegate int av_format_control_message(AVFormatContext* @s, int @type, void* @data, ulong @data_size);
    
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public unsafe delegate int AVOpenCallback(AVFormatContext* @s, AVIOContext** @pb, [MarshalAs(UnmanagedType.LPStr)] string @url, int @flags, AVIOInterruptCB* @int_cb, AVDictionary** @options);
    
    public enum AVIODirEntryType : int
    {
        @AVIO_ENTRY_UNKNOWN = 0,
        @AVIO_ENTRY_BLOCK_DEVICE = 1,
        @AVIO_ENTRY_CHARACTER_DEVICE = 2,
        @AVIO_ENTRY_DIRECTORY = 3,
        @AVIO_ENTRY_NAMED_PIPE = 4,
        @AVIO_ENTRY_SYMBOLIC_LINK = 5,
        @AVIO_ENTRY_SOCKET = 6,
        @AVIO_ENTRY_FILE = 7,
        @AVIO_ENTRY_SERVER = 8,
        @AVIO_ENTRY_SHARE = 9,
        @AVIO_ENTRY_WORKGROUP = 10,
    }
    
    public enum AVIODataMarkerType : int
    {
        @AVIO_DATA_MARKER_HEADER = 0,
        @AVIO_DATA_MARKER_SYNC_POINT = 1,
        @AVIO_DATA_MARKER_BOUNDARY_POINT = 2,
        @AVIO_DATA_MARKER_UNKNOWN = 3,
        @AVIO_DATA_MARKER_TRAILER = 4,
    }
    
    public enum AVStreamParseType : int
    {
        @AVSTREAM_PARSE_NONE = 0,
        @AVSTREAM_PARSE_FULL = 1,
        @AVSTREAM_PARSE_HEADERS = 2,
        @AVSTREAM_PARSE_TIMESTAMPS = 3,
        @AVSTREAM_PARSE_FULL_ONCE = 4,
        @AVSTREAM_PARSE_FULL_RAW = 1463898624,
    }
    
    public enum AVDurationEstimationMethod : int
    {
        @AVFMT_DURATION_FROM_PTS = 0,
        @AVFMT_DURATION_FROM_STREAM = 1,
        @AVFMT_DURATION_FROM_BITRATE = 2,
    }
    
    public enum AVTimebaseSource : int
    {
        @AVFMT_TBCF_AUTO = -1,
        @AVFMT_TBCF_DECODER = 0,
        @AVFMT_TBCF_DEMUXER = 1,
        @AVFMT_TBCF_R_FRAMERATE = 2,
    }
    
    public unsafe static partial class ffmpeg
    {
        public const int LIBAVFORMAT_VERSION_MAJOR = 57;
        public const int LIBAVFORMAT_VERSION_MINOR = 56;
        public const int LIBAVFORMAT_VERSION_MICRO = 100;
        public const bool FF_API_LAVF_BITEXACT = (LIBAVFORMAT_VERSION_MAJOR<58);
        public const bool FF_API_LAVF_FRAC = (LIBAVFORMAT_VERSION_MAJOR<58);
        public const bool FF_API_LAVF_CODEC_TB = (LIBAVFORMAT_VERSION_MAJOR<58);
        public const bool FF_API_URL_FEOF = (LIBAVFORMAT_VERSION_MAJOR<58);
        public const bool FF_API_LAVF_FMT_RAWPICTURE = (LIBAVFORMAT_VERSION_MAJOR<58);
        public const bool FF_API_COMPUTE_PKT_FIELDS2 = (LIBAVFORMAT_VERSION_MAJOR<58);
        public const bool FF_API_OLD_OPEN_CALLBACKS = (LIBAVFORMAT_VERSION_MAJOR<58);
        public const bool FF_API_LAVF_AVCTX = (LIBAVFORMAT_VERSION_MAJOR<58);
        public const bool FF_API_NOCONST_GET_SIDE_DATA = (LIBAVFORMAT_VERSION_MAJOR<58);
        public const bool FF_API_HTTP_USER_AGENT = (LIBAVFORMAT_VERSION_MAJOR<58);
        public const int FF_API_R_FRAME_RATE = 1;
        public const int AVIO_SEEKABLE_NORMAL = 0x0001;
        public const int AVSEEK_SIZE = 0x10000;
        public const int AVSEEK_FORCE = 0x20000;
        public const int AVIO_FLAG_READ = 1;
        public const int AVIO_FLAG_WRITE = 2;
        public const int AVIO_FLAG_READ_WRITE = (AVIO_FLAG_READ|AVIO_FLAG_WRITE);
        public const int AVIO_FLAG_NONBLOCK = 8;
        public const int AVIO_FLAG_DIRECT = 0x8000;
        public const int AVPROBE_SCORE_RETRY = (AVPROBE_SCORE_MAX/4);
        public const int AVPROBE_SCORE_STREAM_RETRY = (AVPROBE_SCORE_MAX/4-1);
        public const int AVPROBE_SCORE_EXTENSION = 50;
        public const int AVPROBE_SCORE_MIME = 75;
        public const int AVPROBE_SCORE_MAX = 100;
        public const int AVPROBE_PADDING_SIZE = 32;
        public const int AVFMT_NOFILE = 0x0001;
        public const int AVFMT_NEEDNUMBER = 0x0002;
        public const int AVFMT_SHOW_IDS = 0x0008;
        public const int AVFMT_RAWPICTURE = 0x0020;
        public const int AVFMT_GLOBALHEADER = 0x0040;
        public const int AVFMT_NOTIMESTAMPS = 0x0080;
        public const int AVFMT_GENERIC_INDEX = 0x0100;
        public const int AVFMT_TS_DISCONT = 0x0200;
        public const int AVFMT_VARIABLE_FPS = 0x0400;
        public const int AVFMT_NODIMENSIONS = 0x0800;
        public const int AVFMT_NOSTREAMS = 0x1000;
        public const int AVFMT_NOBINSEARCH = 0x2000;
        public const int AVFMT_NOGENSEARCH = 0x4000;
        public const int AVFMT_NO_BYTE_SEEK = 0x8000;
        public const int AVFMT_ALLOW_FLUSH = 0x10000;
        public const int AVFMT_TS_NONSTRICT = 0x20000;
        public const int AVFMT_TS_NEGATIVE = 0x40000;
        public const int AVFMT_SEEK_TO_PTS = 0x4000000;
        public const int AVINDEX_KEYFRAME = 0x0001;
        public const int AVINDEX_DISCARD_FRAME = 0x0002;
        public const int AV_DISPOSITION_DEFAULT = 0x0001;
        public const int AV_DISPOSITION_DUB = 0x0002;
        public const int AV_DISPOSITION_ORIGINAL = 0x0004;
        public const int AV_DISPOSITION_COMMENT = 0x0008;
        public const int AV_DISPOSITION_LYRICS = 0x0010;
        public const int AV_DISPOSITION_KARAOKE = 0x0020;
        public const int AV_DISPOSITION_FORCED = 0x0040;
        public const int AV_DISPOSITION_HEARING_IMPAIRED = 0x0080;
        public const int AV_DISPOSITION_VISUAL_IMPAIRED = 0x0100;
        public const int AV_DISPOSITION_CLEAN_EFFECTS = 0x0200;
        public const int AV_DISPOSITION_ATTACHED_PIC = 0x0400;
        public const int AV_DISPOSITION_TIMED_THUMBNAILS = 0x0800;
        public const int AV_DISPOSITION_CAPTIONS = 0x10000;
        public const int AV_DISPOSITION_DESCRIPTIONS = 0x20000;
        public const int AV_DISPOSITION_METADATA = 0x40000;
        public const int AV_PTS_WRAP_IGNORE = 0;
        public const int AV_PTS_WRAP_ADD_OFFSET = 1;
        public const int AV_PTS_WRAP_SUB_OFFSET = -1;
        public const int AVSTREAM_EVENT_FLAG_METADATA_UPDATED = 0x0001;
        public const int MAX_STD_TIMEBASES = (30*12+30+3+6);
        public const int MAX_REORDER_DELAY = 16;
        public const int AV_PROGRAM_RUNNING = 1;
        public const int AVFMTCTX_NOHEADER = 0x0001;
        public const int AVFMT_FLAG_GENPTS = 0x0001;
        public const int AVFMT_FLAG_IGNIDX = 0x0002;
        public const int AVFMT_FLAG_NONBLOCK = 0x0004;
        public const int AVFMT_FLAG_IGNDTS = 0x0008;
        public const int AVFMT_FLAG_NOFILLIN = 0x0010;
        public const int AVFMT_FLAG_NOPARSE = 0x0020;
        public const int AVFMT_FLAG_NOBUFFER = 0x0040;
        public const int AVFMT_FLAG_CUSTOM_IO = 0x0080;
        public const int AVFMT_FLAG_DISCARD_CORRUPT = 0x0100;
        public const int AVFMT_FLAG_FLUSH_PACKETS = 0x0200;
        public const int AVFMT_FLAG_BITEXACT = 0x0400;
        public const int AVFMT_FLAG_MP4A_LATM = 0x8000;
        public const int AVFMT_FLAG_SORT_DTS = 0x10000;
        public const int AVFMT_FLAG_PRIV_OPT = 0x20000;
        public const int AVFMT_FLAG_KEEP_SIDE_DATA = 0x40000;
        public const int AVFMT_FLAG_FAST_SEEK = 0x80000;
        public const int AVFMT_FLAG_SHORTEST = 0x100000;
        public const int AVFMT_FLAG_AUTO_BSF = 0x200000;
        public const int FF_FDEBUG_TS = 0x0001;
        public const int AVFMT_EVENT_FLAG_METADATA_UPDATED = 0x0001;
        public const int AVFMT_AVOID_NEG_TS_AUTO = -1;
        public const int AVFMT_AVOID_NEG_TS_MAKE_NON_NEGATIVE = 1;
        public const int AVFMT_AVOID_NEG_TS_MAKE_ZERO = 2;
        public const int AVSEEK_FLAG_BACKWARD = 1;
        public const int AVSEEK_FLAG_BYTE = 2;
        public const int AVSEEK_FLAG_ANY = 4;
        public const int AVSEEK_FLAG_FRAME = 8;
        public const int AVSTREAM_INIT_IN_WRITE_HEADER = 0;
        public const int AVSTREAM_INIT_IN_INIT_OUTPUT = 1;
        public const int AV_FRAME_FILENAME_FLAGS_MULTIPLE = 1;
        private const string libavformat = "avformat-57";
        
        [DllImport(libavformat, EntryPoint = "avio_find_protocol_name", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        [return: MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof(ConstCharPtrMarshaler))]
        public static extern string avio_find_protocol_name([MarshalAs(UnmanagedType.LPStr)] string @url);
        
        [DllImport(libavformat, EntryPoint = "avio_check", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern int avio_check([MarshalAs(UnmanagedType.LPStr)] string @url, int @flags);
        
        [DllImport(libavformat, EntryPoint = "avpriv_io_move", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern int avpriv_io_move([MarshalAs(UnmanagedType.LPStr)] string @url_src, [MarshalAs(UnmanagedType.LPStr)] string @url_dst);
        
        [DllImport(libavformat, EntryPoint = "avpriv_io_delete", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern int avpriv_io_delete([MarshalAs(UnmanagedType.LPStr)] string @url);
        
        [DllImport(libavformat, EntryPoint = "avio_open_dir", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern int avio_open_dir(AVIODirContext** @s, [MarshalAs(UnmanagedType.LPStr)] string @url, AVDictionary** @options);
        
        [DllImport(libavformat, EntryPoint = "avio_read_dir", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern int avio_read_dir(AVIODirContext* @s, AVIODirEntry** @next);
        
        [DllImport(libavformat, EntryPoint = "avio_close_dir", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern int avio_close_dir(AVIODirContext** @s);
        
        [DllImport(libavformat, EntryPoint = "avio_free_directory_entry", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern void avio_free_directory_entry(AVIODirEntry** @entry);
        
        [DllImport(libavformat, EntryPoint = "avio_alloc_context", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern AVIOContext* avio_alloc_context(sbyte* @buffer, int @buffer_size, int @write_flag, void* @opaque, IntPtr @read_packet, IntPtr @write_packet, IntPtr @seek);
        
        [DllImport(libavformat, EntryPoint = "avio_w8", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern void avio_w8(AVIOContext* @s, int @b);
        
        [DllImport(libavformat, EntryPoint = "avio_write", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern void avio_write(AVIOContext* @s, sbyte* @buf, int @size);
        
        [DllImport(libavformat, EntryPoint = "avio_wl64", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern void avio_wl64(AVIOContext* @s, ulong @val);
        
        [DllImport(libavformat, EntryPoint = "avio_wb64", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern void avio_wb64(AVIOContext* @s, ulong @val);
        
        [DllImport(libavformat, EntryPoint = "avio_wl32", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern void avio_wl32(AVIOContext* @s, uint @val);
        
        [DllImport(libavformat, EntryPoint = "avio_wb32", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern void avio_wb32(AVIOContext* @s, uint @val);
        
        [DllImport(libavformat, EntryPoint = "avio_wl24", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern void avio_wl24(AVIOContext* @s, uint @val);
        
        [DllImport(libavformat, EntryPoint = "avio_wb24", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern void avio_wb24(AVIOContext* @s, uint @val);
        
        [DllImport(libavformat, EntryPoint = "avio_wl16", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern void avio_wl16(AVIOContext* @s, uint @val);
        
        [DllImport(libavformat, EntryPoint = "avio_wb16", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern void avio_wb16(AVIOContext* @s, uint @val);
        
        [DllImport(libavformat, EntryPoint = "avio_put_str", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern int avio_put_str(AVIOContext* @s, [MarshalAs(UnmanagedType.LPStr)] string @str);
        
        [DllImport(libavformat, EntryPoint = "avio_put_str16le", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern int avio_put_str16le(AVIOContext* @s, [MarshalAs(UnmanagedType.LPStr)] string @str);
        
        [DllImport(libavformat, EntryPoint = "avio_put_str16be", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern int avio_put_str16be(AVIOContext* @s, [MarshalAs(UnmanagedType.LPStr)] string @str);
        
        [DllImport(libavformat, EntryPoint = "avio_write_marker", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern void avio_write_marker(AVIOContext* @s, long @time, AVIODataMarkerType @type);
        
        [DllImport(libavformat, EntryPoint = "avio_seek", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern long avio_seek(AVIOContext* @s, long @offset, int @whence);
        
        [DllImport(libavformat, EntryPoint = "avio_skip", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern long avio_skip(AVIOContext* @s, long @offset);
        
        [DllImport(libavformat, EntryPoint = "avio_size", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern long avio_size(AVIOContext* @s);
        
        [DllImport(libavformat, EntryPoint = "avio_feof", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern int avio_feof(AVIOContext* @s);
        
        [DllImport(libavformat, EntryPoint = "url_feof", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern int url_feof(AVIOContext* @s);
        
        [DllImport(libavformat, EntryPoint = "avio_printf", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern int avio_printf(AVIOContext* @s, [MarshalAs(UnmanagedType.LPStr)] string @fmt);
        
        [DllImport(libavformat, EntryPoint = "avio_flush", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern void avio_flush(AVIOContext* @s);
        
        [DllImport(libavformat, EntryPoint = "avio_read", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern int avio_read(AVIOContext* @s, sbyte* @buf, int @size);
        
        [DllImport(libavformat, EntryPoint = "avio_r8", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern int avio_r8(AVIOContext* @s);
        
        [DllImport(libavformat, EntryPoint = "avio_rl16", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern uint avio_rl16(AVIOContext* @s);
        
        [DllImport(libavformat, EntryPoint = "avio_rl24", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern uint avio_rl24(AVIOContext* @s);
        
        [DllImport(libavformat, EntryPoint = "avio_rl32", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern uint avio_rl32(AVIOContext* @s);
        
        [DllImport(libavformat, EntryPoint = "avio_rl64", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern ulong avio_rl64(AVIOContext* @s);
        
        [DllImport(libavformat, EntryPoint = "avio_rb16", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern uint avio_rb16(AVIOContext* @s);
        
        [DllImport(libavformat, EntryPoint = "avio_rb24", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern uint avio_rb24(AVIOContext* @s);
        
        [DllImport(libavformat, EntryPoint = "avio_rb32", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern uint avio_rb32(AVIOContext* @s);
        
        [DllImport(libavformat, EntryPoint = "avio_rb64", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern ulong avio_rb64(AVIOContext* @s);
        
        [DllImport(libavformat, EntryPoint = "avio_get_str", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern int avio_get_str(AVIOContext* @pb, int @maxlen, IntPtr @buf, int @buflen);
        
        [DllImport(libavformat, EntryPoint = "avio_get_str16le", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern int avio_get_str16le(AVIOContext* @pb, int @maxlen, IntPtr @buf, int @buflen);
        
        [DllImport(libavformat, EntryPoint = "avio_get_str16be", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern int avio_get_str16be(AVIOContext* @pb, int @maxlen, IntPtr @buf, int @buflen);
        
        [DllImport(libavformat, EntryPoint = "avio_open", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern int avio_open(AVIOContext** @s, [MarshalAs(UnmanagedType.LPStr)] string @url, int @flags);
        
        [DllImport(libavformat, EntryPoint = "avio_open2", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern int avio_open2(AVIOContext** @s, [MarshalAs(UnmanagedType.LPStr)] string @url, int @flags, AVIOInterruptCB* @int_cb, AVDictionary** @options);
        
        [DllImport(libavformat, EntryPoint = "avio_close", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern int avio_close(AVIOContext* @s);
        
        [DllImport(libavformat, EntryPoint = "avio_closep", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern int avio_closep(AVIOContext** @s);
        
        [DllImport(libavformat, EntryPoint = "avio_open_dyn_buf", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern int avio_open_dyn_buf(AVIOContext** @s);
        
        [DllImport(libavformat, EntryPoint = "avio_close_dyn_buf", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern int avio_close_dyn_buf(AVIOContext* @s, sbyte** @pbuffer);
        
        [DllImport(libavformat, EntryPoint = "avio_enum_protocols", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        [return: MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof(ConstCharPtrMarshaler))]
        public static extern string avio_enum_protocols(void** @opaque, int @output);
        
        [DllImport(libavformat, EntryPoint = "avio_pause", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern int avio_pause(AVIOContext* @h, int @pause);
        
        [DllImport(libavformat, EntryPoint = "avio_seek_time", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern long avio_seek_time(AVIOContext* @h, int @stream_index, long @timestamp, int @flags);
        
        [DllImport(libavformat, EntryPoint = "avio_read_to_bprint", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern int avio_read_to_bprint(AVIOContext* @h, AVBPrint* @pb, ulong @max_size);
        
        [DllImport(libavformat, EntryPoint = "avio_accept", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern int avio_accept(AVIOContext* @s, AVIOContext** @c);
        
        [DllImport(libavformat, EntryPoint = "avio_handshake", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern int avio_handshake(AVIOContext* @c);
        
        [DllImport(libavformat, EntryPoint = "av_get_packet", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern int av_get_packet(AVIOContext* @s, AVPacket* @pkt, int @size);
        
        [DllImport(libavformat, EntryPoint = "av_append_packet", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern int av_append_packet(AVIOContext* @s, AVPacket* @pkt, int @size);
        
        [DllImport(libavformat, EntryPoint = "av_stream_get_r_frame_rate", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern AVRational av_stream_get_r_frame_rate(AVStream* @s);
        
        [DllImport(libavformat, EntryPoint = "av_stream_set_r_frame_rate", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern void av_stream_set_r_frame_rate(AVStream* @s, AVRational @r);
        
        [DllImport(libavformat, EntryPoint = "av_stream_get_parser", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern AVCodecParserContext* av_stream_get_parser(AVStream* @s);
        
        [DllImport(libavformat, EntryPoint = "av_stream_get_recommended_encoder_configuration", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern sbyte* av_stream_get_recommended_encoder_configuration(AVStream* @s);
        
        [DllImport(libavformat, EntryPoint = "av_stream_set_recommended_encoder_configuration", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern void av_stream_set_recommended_encoder_configuration(AVStream* @s, IntPtr @configuration);
        
        [DllImport(libavformat, EntryPoint = "av_stream_get_end_pts", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern long av_stream_get_end_pts(AVStream* @st);
        
        [DllImport(libavformat, EntryPoint = "av_format_get_probe_score", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern int av_format_get_probe_score(AVFormatContext* @s);
        
        [DllImport(libavformat, EntryPoint = "av_format_get_video_codec", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern AVCodec* av_format_get_video_codec(AVFormatContext* @s);
        
        [DllImport(libavformat, EntryPoint = "av_format_set_video_codec", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern void av_format_set_video_codec(AVFormatContext* @s, AVCodec* @c);
        
        [DllImport(libavformat, EntryPoint = "av_format_get_audio_codec", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern AVCodec* av_format_get_audio_codec(AVFormatContext* @s);
        
        [DllImport(libavformat, EntryPoint = "av_format_set_audio_codec", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern void av_format_set_audio_codec(AVFormatContext* @s, AVCodec* @c);
        
        [DllImport(libavformat, EntryPoint = "av_format_get_subtitle_codec", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern AVCodec* av_format_get_subtitle_codec(AVFormatContext* @s);
        
        [DllImport(libavformat, EntryPoint = "av_format_set_subtitle_codec", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern void av_format_set_subtitle_codec(AVFormatContext* @s, AVCodec* @c);
        
        [DllImport(libavformat, EntryPoint = "av_format_get_data_codec", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern AVCodec* av_format_get_data_codec(AVFormatContext* @s);
        
        [DllImport(libavformat, EntryPoint = "av_format_set_data_codec", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern void av_format_set_data_codec(AVFormatContext* @s, AVCodec* @c);
        
        [DllImport(libavformat, EntryPoint = "av_format_get_metadata_header_padding", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern int av_format_get_metadata_header_padding(AVFormatContext* @s);
        
        [DllImport(libavformat, EntryPoint = "av_format_set_metadata_header_padding", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern void av_format_set_metadata_header_padding(AVFormatContext* @s, int @c);
        
        [DllImport(libavformat, EntryPoint = "av_format_get_opaque", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern void* av_format_get_opaque(AVFormatContext* @s);
        
        [DllImport(libavformat, EntryPoint = "av_format_set_opaque", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern void av_format_set_opaque(AVFormatContext* @s, void* @opaque);
        
        [DllImport(libavformat, EntryPoint = "av_format_get_control_message_cb", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern av_format_control_message av_format_get_control_message_cb(AVFormatContext* @s);
        
        [DllImport(libavformat, EntryPoint = "av_format_set_control_message_cb", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern void av_format_set_control_message_cb(AVFormatContext* @s, av_format_control_message @callback);
        
        [DllImport(libavformat, EntryPoint = "av_format_get_open_cb", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern AVOpenCallback av_format_get_open_cb(AVFormatContext* @s);
        
        [DllImport(libavformat, EntryPoint = "av_format_set_open_cb", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern void av_format_set_open_cb(AVFormatContext* @s, AVOpenCallback @callback);
        
        [DllImport(libavformat, EntryPoint = "av_format_inject_global_side_data", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern void av_format_inject_global_side_data(AVFormatContext* @s);
        
        [DllImport(libavformat, EntryPoint = "av_fmt_ctx_get_duration_estimation_method", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern AVDurationEstimationMethod av_fmt_ctx_get_duration_estimation_method(AVFormatContext* @ctx);
        
        [DllImport(libavformat, EntryPoint = "avformat_version", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern uint avformat_version();
        
        [DllImport(libavformat, EntryPoint = "avformat_configuration", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        [return: MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof(ConstCharPtrMarshaler))]
        public static extern string avformat_configuration();
        
        [DllImport(libavformat, EntryPoint = "avformat_license", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        [return: MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof(ConstCharPtrMarshaler))]
        public static extern string avformat_license();
        
        [DllImport(libavformat, EntryPoint = "av_register_all", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern void av_register_all();
        
        [DllImport(libavformat, EntryPoint = "av_register_input_format", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern void av_register_input_format(AVInputFormat* @format);
        
        [DllImport(libavformat, EntryPoint = "av_register_output_format", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern void av_register_output_format(AVOutputFormat* @format);
        
        [DllImport(libavformat, EntryPoint = "avformat_network_init", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern int avformat_network_init();
        
        [DllImport(libavformat, EntryPoint = "avformat_network_deinit", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern int avformat_network_deinit();
        
        [DllImport(libavformat, EntryPoint = "av_iformat_next", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern AVInputFormat* av_iformat_next(AVInputFormat* @f);
        
        [DllImport(libavformat, EntryPoint = "av_oformat_next", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern AVOutputFormat* av_oformat_next(AVOutputFormat* @f);
        
        [DllImport(libavformat, EntryPoint = "avformat_alloc_context", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern AVFormatContext* avformat_alloc_context();
        
        [DllImport(libavformat, EntryPoint = "avformat_free_context", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern void avformat_free_context(AVFormatContext* @s);
        
        [DllImport(libavformat, EntryPoint = "avformat_get_class", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern AVClass* avformat_get_class();
        
        [DllImport(libavformat, EntryPoint = "avformat_new_stream", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern AVStream* avformat_new_stream(AVFormatContext* @s, AVCodec* @c);
        
        [DllImport(libavformat, EntryPoint = "av_stream_new_side_data", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern sbyte* av_stream_new_side_data(AVStream* @stream, AVPacketSideDataType @type, int @size);
        
        [DllImport(libavformat, EntryPoint = "av_stream_get_side_data", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern sbyte* av_stream_get_side_data(AVStream* @stream, AVPacketSideDataType @type, int* @size);
        
        [DllImport(libavformat, EntryPoint = "av_new_program", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern AVProgram* av_new_program(AVFormatContext* @s, int @id);
        
        [DllImport(libavformat, EntryPoint = "avformat_alloc_output_context2", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern int avformat_alloc_output_context2(AVFormatContext** @ctx, AVOutputFormat* @oformat, [MarshalAs(UnmanagedType.LPStr)] string @format_name, [MarshalAs(UnmanagedType.LPStr)] string @filename);
        
        [DllImport(libavformat, EntryPoint = "av_find_input_format", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern AVInputFormat* av_find_input_format([MarshalAs(UnmanagedType.LPStr)] string @short_name);
        
        [DllImport(libavformat, EntryPoint = "av_probe_input_format", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern AVInputFormat* av_probe_input_format(AVProbeData* @pd, int @is_opened);
        
        [DllImport(libavformat, EntryPoint = "av_probe_input_format2", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern AVInputFormat* av_probe_input_format2(AVProbeData* @pd, int @is_opened, int* @score_max);
        
        [DllImport(libavformat, EntryPoint = "av_probe_input_format3", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern AVInputFormat* av_probe_input_format3(AVProbeData* @pd, int @is_opened, int* @score_ret);
        
        [DllImport(libavformat, EntryPoint = "av_probe_input_buffer2", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern int av_probe_input_buffer2(AVIOContext* @pb, AVInputFormat** @fmt, [MarshalAs(UnmanagedType.LPStr)] string @url, void* @logctx, uint @offset, uint @max_probe_size);
        
        [DllImport(libavformat, EntryPoint = "av_probe_input_buffer", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern int av_probe_input_buffer(AVIOContext* @pb, AVInputFormat** @fmt, [MarshalAs(UnmanagedType.LPStr)] string @url, void* @logctx, uint @offset, uint @max_probe_size);
        
        [DllImport(libavformat, EntryPoint = "avformat_open_input", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern int avformat_open_input(AVFormatContext** @ps, [MarshalAs(UnmanagedType.LPStr)] string @url, AVInputFormat* @fmt, AVDictionary** @options);
        
        [DllImport(libavformat, EntryPoint = "av_demuxer_open", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern int av_demuxer_open(AVFormatContext* @ic);
        
        [DllImport(libavformat, EntryPoint = "avformat_find_stream_info", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern int avformat_find_stream_info(AVFormatContext* @ic, AVDictionary** @options);
        
        [DllImport(libavformat, EntryPoint = "av_find_program_from_stream", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern AVProgram* av_find_program_from_stream(AVFormatContext* @ic, AVProgram* @last, int @s);
        
        [DllImport(libavformat, EntryPoint = "av_program_add_stream_index", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern void av_program_add_stream_index(AVFormatContext* @ac, int @progid, uint @idx);
        
        [DllImport(libavformat, EntryPoint = "av_find_best_stream", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern int av_find_best_stream(AVFormatContext* @ic, AVMediaType @type, int @wanted_stream_nb, int @related_stream, AVCodec** @decoder_ret, int @flags);
        
        [DllImport(libavformat, EntryPoint = "av_read_frame", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern int av_read_frame(AVFormatContext* @s, AVPacket* @pkt);
        
        [DllImport(libavformat, EntryPoint = "av_seek_frame", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern int av_seek_frame(AVFormatContext* @s, int @stream_index, long @timestamp, int @flags);
        
        [DllImport(libavformat, EntryPoint = "avformat_seek_file", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern int avformat_seek_file(AVFormatContext* @s, int @stream_index, long @min_ts, long @ts, long @max_ts, int @flags);
        
        [DllImport(libavformat, EntryPoint = "avformat_flush", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern int avformat_flush(AVFormatContext* @s);
        
        [DllImport(libavformat, EntryPoint = "av_read_play", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern int av_read_play(AVFormatContext* @s);
        
        [DllImport(libavformat, EntryPoint = "av_read_pause", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern int av_read_pause(AVFormatContext* @s);
        
        [DllImport(libavformat, EntryPoint = "avformat_close_input", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern void avformat_close_input(AVFormatContext** @s);
        
        [DllImport(libavformat, EntryPoint = "avformat_write_header", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern int avformat_write_header(AVFormatContext* @s, AVDictionary** @options);
        
        [DllImport(libavformat, EntryPoint = "avformat_init_output", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern int avformat_init_output(AVFormatContext* @s, AVDictionary** @options);
        
        [DllImport(libavformat, EntryPoint = "av_write_frame", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern int av_write_frame(AVFormatContext* @s, AVPacket* @pkt);
        
        [DllImport(libavformat, EntryPoint = "av_interleaved_write_frame", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern int av_interleaved_write_frame(AVFormatContext* @s, AVPacket* @pkt);
        
        [DllImport(libavformat, EntryPoint = "av_write_uncoded_frame", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern int av_write_uncoded_frame(AVFormatContext* @s, int @stream_index, AVFrame* @frame);
        
        [DllImport(libavformat, EntryPoint = "av_interleaved_write_uncoded_frame", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern int av_interleaved_write_uncoded_frame(AVFormatContext* @s, int @stream_index, AVFrame* @frame);
        
        [DllImport(libavformat, EntryPoint = "av_write_uncoded_frame_query", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern int av_write_uncoded_frame_query(AVFormatContext* @s, int @stream_index);
        
        [DllImport(libavformat, EntryPoint = "av_write_trailer", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern int av_write_trailer(AVFormatContext* @s);
        
        [DllImport(libavformat, EntryPoint = "av_guess_format", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern AVOutputFormat* av_guess_format([MarshalAs(UnmanagedType.LPStr)] string @short_name, [MarshalAs(UnmanagedType.LPStr)] string @filename, [MarshalAs(UnmanagedType.LPStr)] string @mime_type);
        
        [DllImport(libavformat, EntryPoint = "av_guess_codec", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern AVCodecID av_guess_codec(AVOutputFormat* @fmt, [MarshalAs(UnmanagedType.LPStr)] string @short_name, [MarshalAs(UnmanagedType.LPStr)] string @filename, [MarshalAs(UnmanagedType.LPStr)] string @mime_type, AVMediaType @type);
        
        [DllImport(libavformat, EntryPoint = "av_get_output_timestamp", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern int av_get_output_timestamp(AVFormatContext* @s, int @stream, long* @dts, long* @wall);
        
        [DllImport(libavformat, EntryPoint = "av_hex_dump", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern void av_hex_dump(_iobuf* @f, sbyte* @buf, int @size);
        
        [DllImport(libavformat, EntryPoint = "av_hex_dump_log", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern void av_hex_dump_log(void* @avcl, int @level, sbyte* @buf, int @size);
        
        [DllImport(libavformat, EntryPoint = "av_pkt_dump2", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern void av_pkt_dump2(_iobuf* @f, AVPacket* @pkt, int @dump_payload, AVStream* @st);
        
        [DllImport(libavformat, EntryPoint = "av_pkt_dump_log2", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern void av_pkt_dump_log2(void* @avcl, int @level, AVPacket* @pkt, int @dump_payload, AVStream* @st);
        
        [DllImport(libavformat, EntryPoint = "av_codec_get_id", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern AVCodecID av_codec_get_id(AVCodecTag** @tags, uint @tag);
        
        [DllImport(libavformat, EntryPoint = "av_codec_get_tag", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern uint av_codec_get_tag(AVCodecTag** @tags, AVCodecID @id);
        
        [DllImport(libavformat, EntryPoint = "av_codec_get_tag2", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern int av_codec_get_tag2(AVCodecTag** @tags, AVCodecID @id, uint* @tag);
        
        [DllImport(libavformat, EntryPoint = "av_find_default_stream_index", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern int av_find_default_stream_index(AVFormatContext* @s);
        
        [DllImport(libavformat, EntryPoint = "av_index_search_timestamp", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern int av_index_search_timestamp(AVStream* @st, long @timestamp, int @flags);
        
        [DllImport(libavformat, EntryPoint = "av_add_index_entry", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern int av_add_index_entry(AVStream* @st, long @pos, long @timestamp, int @size, int @distance, int @flags);
        
        [DllImport(libavformat, EntryPoint = "av_url_split", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern void av_url_split(IntPtr @proto, int @proto_size, IntPtr @authorization, int @authorization_size, IntPtr @hostname, int @hostname_size, int* @port_ptr, IntPtr @path, int @path_size, [MarshalAs(UnmanagedType.LPStr)] string @url);
        
        [DllImport(libavformat, EntryPoint = "av_dump_format", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern void av_dump_format(AVFormatContext* @ic, int @index, [MarshalAs(UnmanagedType.LPStr)] string @url, int @is_output);
        
        [DllImport(libavformat, EntryPoint = "av_get_frame_filename2", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern int av_get_frame_filename2(IntPtr @buf, int @buf_size, [MarshalAs(UnmanagedType.LPStr)] string @path, int @number, int @flags);
        
        [DllImport(libavformat, EntryPoint = "av_get_frame_filename", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern int av_get_frame_filename(IntPtr @buf, int @buf_size, [MarshalAs(UnmanagedType.LPStr)] string @path, int @number);
        
        [DllImport(libavformat, EntryPoint = "av_filename_number_test", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern int av_filename_number_test([MarshalAs(UnmanagedType.LPStr)] string @filename);
        
        [DllImport(libavformat, EntryPoint = "av_sdp_create", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern int av_sdp_create(AVFormatContext** @ac, int @n_files, IntPtr @buf, int @size);
        
        [DllImport(libavformat, EntryPoint = "av_match_ext", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern int av_match_ext([MarshalAs(UnmanagedType.LPStr)] string @filename, [MarshalAs(UnmanagedType.LPStr)] string @extensions);
        
        [DllImport(libavformat, EntryPoint = "avformat_query_codec", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern int avformat_query_codec(AVOutputFormat* @ofmt, AVCodecID @codec_id, int @std_compliance);
        
        [DllImport(libavformat, EntryPoint = "avformat_get_riff_video_tags", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern AVCodecTag* avformat_get_riff_video_tags();
        
        [DllImport(libavformat, EntryPoint = "avformat_get_riff_audio_tags", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern AVCodecTag* avformat_get_riff_audio_tags();
        
        [DllImport(libavformat, EntryPoint = "avformat_get_mov_video_tags", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern AVCodecTag* avformat_get_mov_video_tags();
        
        [DllImport(libavformat, EntryPoint = "avformat_get_mov_audio_tags", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern AVCodecTag* avformat_get_mov_audio_tags();
        
        [DllImport(libavformat, EntryPoint = "av_guess_sample_aspect_ratio", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern AVRational av_guess_sample_aspect_ratio(AVFormatContext* @format, AVStream* @stream, AVFrame* @frame);
        
        [DllImport(libavformat, EntryPoint = "av_guess_frame_rate", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern AVRational av_guess_frame_rate(AVFormatContext* @ctx, AVStream* @stream, AVFrame* @frame);
        
        [DllImport(libavformat, EntryPoint = "avformat_match_stream_specifier", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern int avformat_match_stream_specifier(AVFormatContext* @s, AVStream* @st, [MarshalAs(UnmanagedType.LPStr)] string @spec);
        
        [DllImport(libavformat, EntryPoint = "avformat_queue_attached_pictures", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern int avformat_queue_attached_pictures(AVFormatContext* @s);
        
        [DllImport(libavformat, EntryPoint = "av_apply_bitstream_filters", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern int av_apply_bitstream_filters(AVCodecContext* @codec, AVPacket* @pkt, AVBitStreamFilterContext* @bsfc);
        
        [DllImport(libavformat, EntryPoint = "avformat_transfer_internal_stream_timing_info", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern int avformat_transfer_internal_stream_timing_info(AVOutputFormat* @ofmt, AVStream* @ost, AVStream* @ist, AVTimebaseSource @copy_tb);
        
        [DllImport(libavformat, EntryPoint = "av_stream_get_codec_timebase", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern AVRational av_stream_get_codec_timebase(AVStream* @st);
        
    }
}
