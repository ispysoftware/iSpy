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
    
    public unsafe partial struct AVCodecDescriptor
    {
        public AVCodecID @id;
        public AVMediaType @type;
        public sbyte* @name;
        public sbyte* @long_name;
        public int @props;
        public sbyte** @mime_types;
        public AVProfile* @profiles;
    }
    
    public unsafe partial struct AVProfile
    {
    }
    
    public unsafe partial struct RcOverride
    {
        public int @start_frame;
        public int @end_frame;
        public int @qscale;
        public float @quality_factor;
    }
    
    public unsafe partial struct AVPanScan
    {
        public int @id;
        public int @width;
        public int @height;
        public fixed short @position0[2]; public fixed short @position1[2]; public fixed short @position2[2]; 
    }
    
    public unsafe partial struct AVCPBProperties
    {
        public int @max_bitrate;
        public int @min_bitrate;
        public int @avg_bitrate;
        public int @buffer_size;
        public ulong @vbv_delay;
    }
    
    public unsafe partial struct AVPacketSideData
    {
        public sbyte* @data;
        public int @size;
        public AVPacketSideDataType @type;
    }
    
    public unsafe partial struct AVPacket
    {
        public AVBufferRef* @buf;
        public long @pts;
        public long @dts;
        public sbyte* @data;
        public int @size;
        public int @stream_index;
        public int @flags;
        public AVPacketSideData* @side_data;
        public int @side_data_elems;
        public long @duration;
        public long @pos;
        public long @convergence_duration;
    }
    
    public unsafe partial struct AVCodecInternal
    {
    }
    
    public unsafe partial struct AVCodecContext
    {
        public AVClass* @av_class;
        public int @log_level_offset;
        public AVMediaType @codec_type;
        public AVCodec* @codec;
        public fixed sbyte @codec_name[32]; 
        public AVCodecID @codec_id;
        public uint @codec_tag;
        public uint @stream_codec_tag;
        public void* @priv_data;
        public AVCodecInternal* @internal;
        public void* @opaque;
        public long @bit_rate;
        public int @bit_rate_tolerance;
        public int @global_quality;
        public int @compression_level;
        public int @flags;
        public int @flags2;
        public sbyte* @extradata;
        public int @extradata_size;
        public AVRational @time_base;
        public int @ticks_per_frame;
        public int @delay;
        public int @width;
        public int @height;
        public int @coded_width;
        public int @coded_height;
        public int @gop_size;
        public AVPixelFormat @pix_fmt;
        public int @me_method;
        public IntPtr @draw_horiz_band;
        public IntPtr @get_format;
        public int @max_b_frames;
        public float @b_quant_factor;
        public int @rc_strategy;
        public int @b_frame_strategy;
        public float @b_quant_offset;
        public int @has_b_frames;
        public int @mpeg_quant;
        public float @i_quant_factor;
        public float @i_quant_offset;
        public float @lumi_masking;
        public float @temporal_cplx_masking;
        public float @spatial_cplx_masking;
        public float @p_masking;
        public float @dark_masking;
        public int @slice_count;
        public int @prediction_method;
        public int* @slice_offset;
        public AVRational @sample_aspect_ratio;
        public int @me_cmp;
        public int @me_sub_cmp;
        public int @mb_cmp;
        public int @ildct_cmp;
        public int @dia_size;
        public int @last_predictor_count;
        public int @pre_me;
        public int @me_pre_cmp;
        public int @pre_dia_size;
        public int @me_subpel_quality;
        public int @dtg_active_format;
        public int @me_range;
        public int @intra_quant_bias;
        public int @inter_quant_bias;
        public int @slice_flags;
        public int @xvmc_acceleration;
        public int @mb_decision;
        public ushort* @intra_matrix;
        public ushort* @inter_matrix;
        public int @scenechange_threshold;
        public int @noise_reduction;
        public int @me_threshold;
        public int @mb_threshold;
        public int @intra_dc_precision;
        public int @skip_top;
        public int @skip_bottom;
        public float @border_masking;
        public int @mb_lmin;
        public int @mb_lmax;
        public int @me_penalty_compensation;
        public int @bidir_refine;
        public int @brd_scale;
        public int @keyint_min;
        public int @refs;
        public int @chromaoffset;
        public int @scenechange_factor;
        public int @mv0_threshold;
        public int @b_sensitivity;
        public AVColorPrimaries @color_primaries;
        public AVColorTransferCharacteristic @color_trc;
        public AVColorSpace @colorspace;
        public AVColorRange @color_range;
        public AVChromaLocation @chroma_sample_location;
        public int @slices;
        public AVFieldOrder @field_order;
        public int @sample_rate;
        public int @channels;
        public AVSampleFormat @sample_fmt;
        public int @frame_size;
        public int @frame_number;
        public int @block_align;
        public int @cutoff;
        public ulong @channel_layout;
        public ulong @request_channel_layout;
        public AVAudioServiceType @audio_service_type;
        public AVSampleFormat @request_sample_fmt;
        public IntPtr @get_buffer2;
        public int @refcounted_frames;
        public float @qcompress;
        public float @qblur;
        public int @qmin;
        public int @qmax;
        public int @max_qdiff;
        public float @rc_qsquish;
        public float @rc_qmod_amp;
        public int @rc_qmod_freq;
        public int @rc_buffer_size;
        public int @rc_override_count;
        public RcOverride* @rc_override;
        public sbyte* @rc_eq;
        public long @rc_max_rate;
        public long @rc_min_rate;
        public float @rc_buffer_aggressivity;
        public float @rc_initial_cplx;
        public float @rc_max_available_vbv_use;
        public float @rc_min_vbv_overflow_use;
        public int @rc_initial_buffer_occupancy;
        public int @coder_type;
        public int @context_model;
        public int @lmin;
        public int @lmax;
        public int @frame_skip_threshold;
        public int @frame_skip_factor;
        public int @frame_skip_exp;
        public int @frame_skip_cmp;
        public int @trellis;
        public int @min_prediction_order;
        public int @max_prediction_order;
        public long @timecode_frame_start;
        public IntPtr @rtp_callback;
        public int @rtp_payload_size;
        public int @mv_bits;
        public int @header_bits;
        public int @i_tex_bits;
        public int @p_tex_bits;
        public int @i_count;
        public int @p_count;
        public int @skip_count;
        public int @misc_bits;
        public int @frame_bits;
        public sbyte* @stats_out;
        public sbyte* @stats_in;
        public int @workaround_bugs;
        public int @strict_std_compliance;
        public int @error_concealment;
        public int @debug;
        public int @debug_mv;
        public int @err_recognition;
        public long @reordered_opaque;
        public AVHWAccel* @hwaccel;
        public void* @hwaccel_context;
        public fixed ulong @error[8]; 
        public int @dct_algo;
        public int @idct_algo;
        public int @bits_per_coded_sample;
        public int @bits_per_raw_sample;
        public int @lowres;
        public AVFrame* @coded_frame;
        public int @thread_count;
        public int @thread_type;
        public int @active_thread_type;
        public int @thread_safe_callbacks;
        public IntPtr @execute;
        public IntPtr @execute2;
        public int @nsse_weight;
        public int @profile;
        public int @level;
        public AVDiscard @skip_loop_filter;
        public AVDiscard @skip_idct;
        public AVDiscard @skip_frame;
        public sbyte* @subtitle_header;
        public int @subtitle_header_size;
        public int @error_rate;
        public ulong @vbv_delay;
        public int @side_data_only_packets;
        public int @initial_padding;
        public AVRational @framerate;
        public AVPixelFormat @sw_pix_fmt;
        public AVRational @pkt_timebase;
        public AVCodecDescriptor* @codec_descriptor;
        public long @pts_correction_num_faulty_pts;
        public long @pts_correction_num_faulty_dts;
        public long @pts_correction_last_pts;
        public long @pts_correction_last_dts;
        public sbyte* @sub_charenc;
        public int @sub_charenc_mode;
        public int @skip_alpha;
        public int @seek_preroll;
        public ushort* @chroma_intra_matrix;
        public sbyte* @dump_separator;
        public sbyte* @codec_whitelist;
        public uint @properties;
        public AVPacketSideData* @coded_side_data;
        public int @nb_coded_side_data;
        public AVBufferRef* @hw_frames_ctx;
        public int @sub_text_format;
        public int @trailing_padding;
    }
    
    public unsafe partial struct AVHWAccel
    {
    }
    
    public unsafe partial struct AVCodec
    {
    }
    
    public unsafe partial struct AVProfile
    {
        public int @profile;
        public sbyte* @name;
    }
    
    public unsafe partial struct AVCodecDefault
    {
    }
    
    public unsafe partial struct AVSubtitle
    {
    }
    
    public unsafe partial struct AVCodec
    {
        public sbyte* @name;
        public sbyte* @long_name;
        public AVMediaType @type;
        public AVCodecID @id;
        public int @capabilities;
        public AVRational* @supported_framerates;
        public AVPixelFormat* @pix_fmts;
        public int* @supported_samplerates;
        public AVSampleFormat* @sample_fmts;
        public ulong* @channel_layouts;
        public sbyte @max_lowres;
        public AVClass* @priv_class;
        public AVProfile* @profiles;
        public int @priv_data_size;
        public AVCodec* @next;
        public IntPtr @init_thread_copy;
        public IntPtr @update_thread_context;
        public AVCodecDefault* @defaults;
        public IntPtr @init_static_data;
        public IntPtr @init;
        public IntPtr @encode_sub;
        public IntPtr @encode2;
        public IntPtr @decode;
        public IntPtr @close;
        public IntPtr @send_frame;
        public IntPtr @send_packet;
        public IntPtr @receive_frame;
        public IntPtr @receive_packet;
        public IntPtr @flush;
        public int @caps_internal;
    }
    
    public unsafe partial struct MpegEncContext
    {
    }
    
    public unsafe partial struct AVHWAccel
    {
        public sbyte* @name;
        public AVMediaType @type;
        public AVCodecID @id;
        public AVPixelFormat @pix_fmt;
        public int @capabilities;
        public AVHWAccel* @next;
        public IntPtr @alloc_frame;
        public IntPtr @start_frame;
        public IntPtr @decode_slice;
        public IntPtr @end_frame;
        public int @frame_priv_data_size;
        public IntPtr @decode_mb;
        public IntPtr @init;
        public IntPtr @uninit;
        public int @priv_data_size;
    }
    
    public unsafe partial struct AVPicture
    {
        public sbyte* @data0; public sbyte* @data1; public sbyte* @data2; public sbyte* @data3; public sbyte* @data4; public sbyte* @data5; public sbyte* @data6; public sbyte* @data7; 
        public fixed int @linesize[8]; 
    }
    
    public unsafe partial struct AVSubtitleRect
    {
        public int @x;
        public int @y;
        public int @w;
        public int @h;
        public int @nb_colors;
        public AVPicture @pict;
        public sbyte* @data0; public sbyte* @data1; public sbyte* @data2; public sbyte* @data3; 
        public fixed int @linesize[4]; 
        public AVSubtitleType @type;
        public sbyte* @text;
        public sbyte* @ass;
        public int @flags;
    }
    
    public unsafe partial struct AVSubtitle
    {
        public ushort @format;
        public uint @start_display_time;
        public uint @end_display_time;
        public uint @num_rects;
        public AVSubtitleRect** @rects;
        public long @pts;
    }
    
    public unsafe partial struct AVCodecParameters
    {
        public AVMediaType @codec_type;
        public AVCodecID @codec_id;
        public uint @codec_tag;
        public sbyte* @extradata;
        public int @extradata_size;
        public int @format;
        public long @bit_rate;
        public int @bits_per_coded_sample;
        public int @bits_per_raw_sample;
        public int @profile;
        public int @level;
        public int @width;
        public int @height;
        public AVRational @sample_aspect_ratio;
        public AVFieldOrder @field_order;
        public AVColorRange @color_range;
        public AVColorPrimaries @color_primaries;
        public AVColorTransferCharacteristic @color_trc;
        public AVColorSpace @color_space;
        public AVChromaLocation @chroma_location;
        public int @video_delay;
        public ulong @channel_layout;
        public int @channels;
        public int @sample_rate;
        public int @block_align;
        public int @frame_size;
        public int @initial_padding;
        public int @trailing_padding;
        public int @seek_preroll;
    }
    
    public unsafe partial struct AVCodecParserContext
    {
        public void* @priv_data;
        public AVCodecParser* @parser;
        public long @frame_offset;
        public long @cur_offset;
        public long @next_frame_offset;
        public int @pict_type;
        public int @repeat_pict;
        public long @pts;
        public long @dts;
        public long @last_pts;
        public long @last_dts;
        public int @fetch_timestamp;
        public int @cur_frame_start_index;
        public fixed long @cur_frame_offset[4]; 
        public fixed long @cur_frame_pts[4]; 
        public fixed long @cur_frame_dts[4]; 
        public int @flags;
        public long @offset;
        public fixed long @cur_frame_end[4]; 
        public int @key_frame;
        public long @convergence_duration;
        public int @dts_sync_point;
        public int @dts_ref_dts_delta;
        public int @pts_dts_delta;
        public fixed long @cur_frame_pos[4]; 
        public long @pos;
        public long @last_pos;
        public int @duration;
        public AVFieldOrder @field_order;
        public AVPictureStructure @picture_structure;
        public int @output_picture_number;
        public int @width;
        public int @height;
        public int @coded_width;
        public int @coded_height;
        public int @format;
    }
    
    public unsafe partial struct AVCodecParser
    {
    }
    
    public unsafe partial struct AVCodecParser
    {
        public fixed int @codec_ids[5]; 
        public int @priv_data_size;
        public IntPtr @parser_init;
        public IntPtr @parser_parse;
        public IntPtr @parser_close;
        public IntPtr @split;
        public AVCodecParser* @next;
    }
    
    public unsafe partial struct ReSampleContext
    {
    }
    
    public unsafe partial struct AVResampleContext
    {
    }
    
    public unsafe partial struct AVBitStreamFilterContext
    {
        public void* @priv_data;
        public AVBitStreamFilter* @filter;
        public AVCodecParserContext* @parser;
        public AVBitStreamFilterContext* @next;
        public sbyte* @args;
    }
    
    public unsafe partial struct AVBitStreamFilter
    {
    }
    
    public unsafe partial struct AVBSFInternal
    {
    }
    
    public unsafe partial struct AVBSFContext
    {
        public AVClass* @av_class;
        public AVBitStreamFilter* @filter;
        public AVBSFInternal* @internal;
        public void* @priv_data;
        public AVCodecParameters* @par_in;
        public AVCodecParameters* @par_out;
        public AVRational @time_base_in;
        public AVRational @time_base_out;
    }
    
    public unsafe partial struct AVBitStreamFilter
    {
        public sbyte* @name;
        public AVCodecID* @codec_ids;
        public AVClass* @priv_class;
        public int @priv_data_size;
        public IntPtr @init;
        public IntPtr @filter;
        public IntPtr @close;
    }
    
    public unsafe partial struct AVBSFList
    {
    }
    
    public enum AVCodecID : int
    {
        @AV_CODEC_ID_NONE = 0,
        @AV_CODEC_ID_MPEG1VIDEO = 1,
        @AV_CODEC_ID_MPEG2VIDEO = 2,
        @AV_CODEC_ID_MPEG2VIDEO_XVMC = 3,
        @AV_CODEC_ID_H261 = 4,
        @AV_CODEC_ID_H263 = 5,
        @AV_CODEC_ID_RV10 = 6,
        @AV_CODEC_ID_RV20 = 7,
        @AV_CODEC_ID_MJPEG = 8,
        @AV_CODEC_ID_MJPEGB = 9,
        @AV_CODEC_ID_LJPEG = 10,
        @AV_CODEC_ID_SP5X = 11,
        @AV_CODEC_ID_JPEGLS = 12,
        @AV_CODEC_ID_MPEG4 = 13,
        @AV_CODEC_ID_RAWVIDEO = 14,
        @AV_CODEC_ID_MSMPEG4V1 = 15,
        @AV_CODEC_ID_MSMPEG4V2 = 16,
        @AV_CODEC_ID_MSMPEG4V3 = 17,
        @AV_CODEC_ID_WMV1 = 18,
        @AV_CODEC_ID_WMV2 = 19,
        @AV_CODEC_ID_H263P = 20,
        @AV_CODEC_ID_H263I = 21,
        @AV_CODEC_ID_FLV1 = 22,
        @AV_CODEC_ID_SVQ1 = 23,
        @AV_CODEC_ID_SVQ3 = 24,
        @AV_CODEC_ID_DVVIDEO = 25,
        @AV_CODEC_ID_HUFFYUV = 26,
        @AV_CODEC_ID_CYUV = 27,
        @AV_CODEC_ID_H264 = 28,
        @AV_CODEC_ID_INDEO3 = 29,
        @AV_CODEC_ID_VP3 = 30,
        @AV_CODEC_ID_THEORA = 31,
        @AV_CODEC_ID_ASV1 = 32,
        @AV_CODEC_ID_ASV2 = 33,
        @AV_CODEC_ID_FFV1 = 34,
        @AV_CODEC_ID_4XM = 35,
        @AV_CODEC_ID_VCR1 = 36,
        @AV_CODEC_ID_CLJR = 37,
        @AV_CODEC_ID_MDEC = 38,
        @AV_CODEC_ID_ROQ = 39,
        @AV_CODEC_ID_INTERPLAY_VIDEO = 40,
        @AV_CODEC_ID_XAN_WC3 = 41,
        @AV_CODEC_ID_XAN_WC4 = 42,
        @AV_CODEC_ID_RPZA = 43,
        @AV_CODEC_ID_CINEPAK = 44,
        @AV_CODEC_ID_WS_VQA = 45,
        @AV_CODEC_ID_MSRLE = 46,
        @AV_CODEC_ID_MSVIDEO1 = 47,
        @AV_CODEC_ID_IDCIN = 48,
        @AV_CODEC_ID_8BPS = 49,
        @AV_CODEC_ID_SMC = 50,
        @AV_CODEC_ID_FLIC = 51,
        @AV_CODEC_ID_TRUEMOTION1 = 52,
        @AV_CODEC_ID_VMDVIDEO = 53,
        @AV_CODEC_ID_MSZH = 54,
        @AV_CODEC_ID_ZLIB = 55,
        @AV_CODEC_ID_QTRLE = 56,
        @AV_CODEC_ID_TSCC = 57,
        @AV_CODEC_ID_ULTI = 58,
        @AV_CODEC_ID_QDRAW = 59,
        @AV_CODEC_ID_VIXL = 60,
        @AV_CODEC_ID_QPEG = 61,
        @AV_CODEC_ID_PNG = 62,
        @AV_CODEC_ID_PPM = 63,
        @AV_CODEC_ID_PBM = 64,
        @AV_CODEC_ID_PGM = 65,
        @AV_CODEC_ID_PGMYUV = 66,
        @AV_CODEC_ID_PAM = 67,
        @AV_CODEC_ID_FFVHUFF = 68,
        @AV_CODEC_ID_RV30 = 69,
        @AV_CODEC_ID_RV40 = 70,
        @AV_CODEC_ID_VC1 = 71,
        @AV_CODEC_ID_WMV3 = 72,
        @AV_CODEC_ID_LOCO = 73,
        @AV_CODEC_ID_WNV1 = 74,
        @AV_CODEC_ID_AASC = 75,
        @AV_CODEC_ID_INDEO2 = 76,
        @AV_CODEC_ID_FRAPS = 77,
        @AV_CODEC_ID_TRUEMOTION2 = 78,
        @AV_CODEC_ID_BMP = 79,
        @AV_CODEC_ID_CSCD = 80,
        @AV_CODEC_ID_MMVIDEO = 81,
        @AV_CODEC_ID_ZMBV = 82,
        @AV_CODEC_ID_AVS = 83,
        @AV_CODEC_ID_SMACKVIDEO = 84,
        @AV_CODEC_ID_NUV = 85,
        @AV_CODEC_ID_KMVC = 86,
        @AV_CODEC_ID_FLASHSV = 87,
        @AV_CODEC_ID_CAVS = 88,
        @AV_CODEC_ID_JPEG2000 = 89,
        @AV_CODEC_ID_VMNC = 90,
        @AV_CODEC_ID_VP5 = 91,
        @AV_CODEC_ID_VP6 = 92,
        @AV_CODEC_ID_VP6F = 93,
        @AV_CODEC_ID_TARGA = 94,
        @AV_CODEC_ID_DSICINVIDEO = 95,
        @AV_CODEC_ID_TIERTEXSEQVIDEO = 96,
        @AV_CODEC_ID_TIFF = 97,
        @AV_CODEC_ID_GIF = 98,
        @AV_CODEC_ID_DXA = 99,
        @AV_CODEC_ID_DNXHD = 100,
        @AV_CODEC_ID_THP = 101,
        @AV_CODEC_ID_SGI = 102,
        @AV_CODEC_ID_C93 = 103,
        @AV_CODEC_ID_BETHSOFTVID = 104,
        @AV_CODEC_ID_PTX = 105,
        @AV_CODEC_ID_TXD = 106,
        @AV_CODEC_ID_VP6A = 107,
        @AV_CODEC_ID_AMV = 108,
        @AV_CODEC_ID_VB = 109,
        @AV_CODEC_ID_PCX = 110,
        @AV_CODEC_ID_SUNRAST = 111,
        @AV_CODEC_ID_INDEO4 = 112,
        @AV_CODEC_ID_INDEO5 = 113,
        @AV_CODEC_ID_MIMIC = 114,
        @AV_CODEC_ID_RL2 = 115,
        @AV_CODEC_ID_ESCAPE124 = 116,
        @AV_CODEC_ID_DIRAC = 117,
        @AV_CODEC_ID_BFI = 118,
        @AV_CODEC_ID_CMV = 119,
        @AV_CODEC_ID_MOTIONPIXELS = 120,
        @AV_CODEC_ID_TGV = 121,
        @AV_CODEC_ID_TGQ = 122,
        @AV_CODEC_ID_TQI = 123,
        @AV_CODEC_ID_AURA = 124,
        @AV_CODEC_ID_AURA2 = 125,
        @AV_CODEC_ID_V210X = 126,
        @AV_CODEC_ID_TMV = 127,
        @AV_CODEC_ID_V210 = 128,
        @AV_CODEC_ID_DPX = 129,
        @AV_CODEC_ID_MAD = 130,
        @AV_CODEC_ID_FRWU = 131,
        @AV_CODEC_ID_FLASHSV2 = 132,
        @AV_CODEC_ID_CDGRAPHICS = 133,
        @AV_CODEC_ID_R210 = 134,
        @AV_CODEC_ID_ANM = 135,
        @AV_CODEC_ID_BINKVIDEO = 136,
        @AV_CODEC_ID_IFF_ILBM = 137,
        @AV_CODEC_ID_KGV1 = 138,
        @AV_CODEC_ID_YOP = 139,
        @AV_CODEC_ID_VP8 = 140,
        @AV_CODEC_ID_PICTOR = 141,
        @AV_CODEC_ID_ANSI = 142,
        @AV_CODEC_ID_A64_MULTI = 143,
        @AV_CODEC_ID_A64_MULTI5 = 144,
        @AV_CODEC_ID_R10K = 145,
        @AV_CODEC_ID_MXPEG = 146,
        @AV_CODEC_ID_LAGARITH = 147,
        @AV_CODEC_ID_PRORES = 148,
        @AV_CODEC_ID_JV = 149,
        @AV_CODEC_ID_DFA = 150,
        @AV_CODEC_ID_WMV3IMAGE = 151,
        @AV_CODEC_ID_VC1IMAGE = 152,
        @AV_CODEC_ID_UTVIDEO = 153,
        @AV_CODEC_ID_BMV_VIDEO = 154,
        @AV_CODEC_ID_VBLE = 155,
        @AV_CODEC_ID_DXTORY = 156,
        @AV_CODEC_ID_V410 = 157,
        @AV_CODEC_ID_XWD = 158,
        @AV_CODEC_ID_CDXL = 159,
        @AV_CODEC_ID_XBM = 160,
        @AV_CODEC_ID_ZEROCODEC = 161,
        @AV_CODEC_ID_MSS1 = 162,
        @AV_CODEC_ID_MSA1 = 163,
        @AV_CODEC_ID_TSCC2 = 164,
        @AV_CODEC_ID_MTS2 = 165,
        @AV_CODEC_ID_CLLC = 166,
        @AV_CODEC_ID_MSS2 = 167,
        @AV_CODEC_ID_VP9 = 168,
        @AV_CODEC_ID_AIC = 169,
        @AV_CODEC_ID_ESCAPE130 = 170,
        @AV_CODEC_ID_G2M = 171,
        @AV_CODEC_ID_WEBP = 172,
        @AV_CODEC_ID_HNM4_VIDEO = 173,
        @AV_CODEC_ID_HEVC = 174,
        @AV_CODEC_ID_FIC = 175,
        @AV_CODEC_ID_ALIAS_PIX = 176,
        @AV_CODEC_ID_BRENDER_PIX = 177,
        @AV_CODEC_ID_PAF_VIDEO = 178,
        @AV_CODEC_ID_EXR = 179,
        @AV_CODEC_ID_VP7 = 180,
        @AV_CODEC_ID_SANM = 181,
        @AV_CODEC_ID_SGIRLE = 182,
        @AV_CODEC_ID_MVC1 = 183,
        @AV_CODEC_ID_MVC2 = 184,
        @AV_CODEC_ID_HQX = 185,
        @AV_CODEC_ID_TDSC = 186,
        @AV_CODEC_ID_HQ_HQA = 187,
        @AV_CODEC_ID_HAP = 188,
        @AV_CODEC_ID_DDS = 189,
        @AV_CODEC_ID_DXV = 190,
        @AV_CODEC_ID_SCREENPRESSO = 191,
        @AV_CODEC_ID_RSCC = 192,
        @AV_CODEC_ID_Y41P = 32768,
        @AV_CODEC_ID_AVRP = 32769,
        @AV_CODEC_ID_012V = 32770,
        @AV_CODEC_ID_AVUI = 32771,
        @AV_CODEC_ID_AYUV = 32772,
        @AV_CODEC_ID_TARGA_Y216 = 32773,
        @AV_CODEC_ID_V308 = 32774,
        @AV_CODEC_ID_V408 = 32775,
        @AV_CODEC_ID_YUV4 = 32776,
        @AV_CODEC_ID_AVRN = 32777,
        @AV_CODEC_ID_CPIA = 32778,
        @AV_CODEC_ID_XFACE = 32779,
        @AV_CODEC_ID_SNOW = 32780,
        @AV_CODEC_ID_SMVJPEG = 32781,
        @AV_CODEC_ID_APNG = 32782,
        @AV_CODEC_ID_DAALA = 32783,
        @AV_CODEC_ID_CFHD = 32784,
        @AV_CODEC_ID_TRUEMOTION2RT = 32785,
        @AV_CODEC_ID_M101 = 32786,
        @AV_CODEC_ID_MAGICYUV = 32787,
        @AV_CODEC_ID_SHEERVIDEO = 32788,
        @AV_CODEC_ID_YLC = 32789,
        @AV_CODEC_ID_FIRST_AUDIO = 65536,
        @AV_CODEC_ID_PCM_S16LE = 65536,
        @AV_CODEC_ID_PCM_S16BE = 65537,
        @AV_CODEC_ID_PCM_U16LE = 65538,
        @AV_CODEC_ID_PCM_U16BE = 65539,
        @AV_CODEC_ID_PCM_S8 = 65540,
        @AV_CODEC_ID_PCM_U8 = 65541,
        @AV_CODEC_ID_PCM_MULAW = 65542,
        @AV_CODEC_ID_PCM_ALAW = 65543,
        @AV_CODEC_ID_PCM_S32LE = 65544,
        @AV_CODEC_ID_PCM_S32BE = 65545,
        @AV_CODEC_ID_PCM_U32LE = 65546,
        @AV_CODEC_ID_PCM_U32BE = 65547,
        @AV_CODEC_ID_PCM_S24LE = 65548,
        @AV_CODEC_ID_PCM_S24BE = 65549,
        @AV_CODEC_ID_PCM_U24LE = 65550,
        @AV_CODEC_ID_PCM_U24BE = 65551,
        @AV_CODEC_ID_PCM_S24DAUD = 65552,
        @AV_CODEC_ID_PCM_ZORK = 65553,
        @AV_CODEC_ID_PCM_S16LE_PLANAR = 65554,
        @AV_CODEC_ID_PCM_DVD = 65555,
        @AV_CODEC_ID_PCM_F32BE = 65556,
        @AV_CODEC_ID_PCM_F32LE = 65557,
        @AV_CODEC_ID_PCM_F64BE = 65558,
        @AV_CODEC_ID_PCM_F64LE = 65559,
        @AV_CODEC_ID_PCM_BLURAY = 65560,
        @AV_CODEC_ID_PCM_LXF = 65561,
        @AV_CODEC_ID_S302M = 65562,
        @AV_CODEC_ID_PCM_S8_PLANAR = 65563,
        @AV_CODEC_ID_PCM_S24LE_PLANAR = 65564,
        @AV_CODEC_ID_PCM_S32LE_PLANAR = 65565,
        @AV_CODEC_ID_PCM_S16BE_PLANAR = 65566,
        @AV_CODEC_ID_PCM_S64LE = 67584,
        @AV_CODEC_ID_PCM_S64BE = 67585,
        @AV_CODEC_ID_ADPCM_IMA_QT = 69632,
        @AV_CODEC_ID_ADPCM_IMA_WAV = 69633,
        @AV_CODEC_ID_ADPCM_IMA_DK3 = 69634,
        @AV_CODEC_ID_ADPCM_IMA_DK4 = 69635,
        @AV_CODEC_ID_ADPCM_IMA_WS = 69636,
        @AV_CODEC_ID_ADPCM_IMA_SMJPEG = 69637,
        @AV_CODEC_ID_ADPCM_MS = 69638,
        @AV_CODEC_ID_ADPCM_4XM = 69639,
        @AV_CODEC_ID_ADPCM_XA = 69640,
        @AV_CODEC_ID_ADPCM_ADX = 69641,
        @AV_CODEC_ID_ADPCM_EA = 69642,
        @AV_CODEC_ID_ADPCM_G726 = 69643,
        @AV_CODEC_ID_ADPCM_CT = 69644,
        @AV_CODEC_ID_ADPCM_SWF = 69645,
        @AV_CODEC_ID_ADPCM_YAMAHA = 69646,
        @AV_CODEC_ID_ADPCM_SBPRO_4 = 69647,
        @AV_CODEC_ID_ADPCM_SBPRO_3 = 69648,
        @AV_CODEC_ID_ADPCM_SBPRO_2 = 69649,
        @AV_CODEC_ID_ADPCM_THP = 69650,
        @AV_CODEC_ID_ADPCM_IMA_AMV = 69651,
        @AV_CODEC_ID_ADPCM_EA_R1 = 69652,
        @AV_CODEC_ID_ADPCM_EA_R3 = 69653,
        @AV_CODEC_ID_ADPCM_EA_R2 = 69654,
        @AV_CODEC_ID_ADPCM_IMA_EA_SEAD = 69655,
        @AV_CODEC_ID_ADPCM_IMA_EA_EACS = 69656,
        @AV_CODEC_ID_ADPCM_EA_XAS = 69657,
        @AV_CODEC_ID_ADPCM_EA_MAXIS_XA = 69658,
        @AV_CODEC_ID_ADPCM_IMA_ISS = 69659,
        @AV_CODEC_ID_ADPCM_G722 = 69660,
        @AV_CODEC_ID_ADPCM_IMA_APC = 69661,
        @AV_CODEC_ID_ADPCM_VIMA = 69662,
        @AV_CODEC_ID_VIMA = 69662,
        @AV_CODEC_ID_ADPCM_AFC = 71680,
        @AV_CODEC_ID_ADPCM_IMA_OKI = 71681,
        @AV_CODEC_ID_ADPCM_DTK = 71682,
        @AV_CODEC_ID_ADPCM_IMA_RAD = 71683,
        @AV_CODEC_ID_ADPCM_G726LE = 71684,
        @AV_CODEC_ID_ADPCM_THP_LE = 71685,
        @AV_CODEC_ID_ADPCM_PSX = 71686,
        @AV_CODEC_ID_ADPCM_AICA = 71687,
        @AV_CODEC_ID_ADPCM_IMA_DAT4 = 71688,
        @AV_CODEC_ID_ADPCM_MTAF = 71689,
        @AV_CODEC_ID_AMR_NB = 73728,
        @AV_CODEC_ID_AMR_WB = 73729,
        @AV_CODEC_ID_RA_144 = 77824,
        @AV_CODEC_ID_RA_288 = 77825,
        @AV_CODEC_ID_ROQ_DPCM = 81920,
        @AV_CODEC_ID_INTERPLAY_DPCM = 81921,
        @AV_CODEC_ID_XAN_DPCM = 81922,
        @AV_CODEC_ID_SOL_DPCM = 81923,
        @AV_CODEC_ID_SDX2_DPCM = 83968,
        @AV_CODEC_ID_MP2 = 86016,
        @AV_CODEC_ID_MP3 = 86017,
        @AV_CODEC_ID_AAC = 86018,
        @AV_CODEC_ID_AC3 = 86019,
        @AV_CODEC_ID_DTS = 86020,
        @AV_CODEC_ID_VORBIS = 86021,
        @AV_CODEC_ID_DVAUDIO = 86022,
        @AV_CODEC_ID_WMAV1 = 86023,
        @AV_CODEC_ID_WMAV2 = 86024,
        @AV_CODEC_ID_MACE3 = 86025,
        @AV_CODEC_ID_MACE6 = 86026,
        @AV_CODEC_ID_VMDAUDIO = 86027,
        @AV_CODEC_ID_FLAC = 86028,
        @AV_CODEC_ID_MP3ADU = 86029,
        @AV_CODEC_ID_MP3ON4 = 86030,
        @AV_CODEC_ID_SHORTEN = 86031,
        @AV_CODEC_ID_ALAC = 86032,
        @AV_CODEC_ID_WESTWOOD_SND1 = 86033,
        @AV_CODEC_ID_GSM = 86034,
        @AV_CODEC_ID_QDM2 = 86035,
        @AV_CODEC_ID_COOK = 86036,
        @AV_CODEC_ID_TRUESPEECH = 86037,
        @AV_CODEC_ID_TTA = 86038,
        @AV_CODEC_ID_SMACKAUDIO = 86039,
        @AV_CODEC_ID_QCELP = 86040,
        @AV_CODEC_ID_WAVPACK = 86041,
        @AV_CODEC_ID_DSICINAUDIO = 86042,
        @AV_CODEC_ID_IMC = 86043,
        @AV_CODEC_ID_MUSEPACK7 = 86044,
        @AV_CODEC_ID_MLP = 86045,
        @AV_CODEC_ID_GSM_MS = 86046,
        @AV_CODEC_ID_ATRAC3 = 86047,
        @AV_CODEC_ID_VOXWARE = 86048,
        @AV_CODEC_ID_APE = 86049,
        @AV_CODEC_ID_NELLYMOSER = 86050,
        @AV_CODEC_ID_MUSEPACK8 = 86051,
        @AV_CODEC_ID_SPEEX = 86052,
        @AV_CODEC_ID_WMAVOICE = 86053,
        @AV_CODEC_ID_WMAPRO = 86054,
        @AV_CODEC_ID_WMALOSSLESS = 86055,
        @AV_CODEC_ID_ATRAC3P = 86056,
        @AV_CODEC_ID_EAC3 = 86057,
        @AV_CODEC_ID_SIPR = 86058,
        @AV_CODEC_ID_MP1 = 86059,
        @AV_CODEC_ID_TWINVQ = 86060,
        @AV_CODEC_ID_TRUEHD = 86061,
        @AV_CODEC_ID_MP4ALS = 86062,
        @AV_CODEC_ID_ATRAC1 = 86063,
        @AV_CODEC_ID_BINKAUDIO_RDFT = 86064,
        @AV_CODEC_ID_BINKAUDIO_DCT = 86065,
        @AV_CODEC_ID_AAC_LATM = 86066,
        @AV_CODEC_ID_QDMC = 86067,
        @AV_CODEC_ID_CELT = 86068,
        @AV_CODEC_ID_G723_1 = 86069,
        @AV_CODEC_ID_G729 = 86070,
        @AV_CODEC_ID_8SVX_EXP = 86071,
        @AV_CODEC_ID_8SVX_FIB = 86072,
        @AV_CODEC_ID_BMV_AUDIO = 86073,
        @AV_CODEC_ID_RALF = 86074,
        @AV_CODEC_ID_IAC = 86075,
        @AV_CODEC_ID_ILBC = 86076,
        @AV_CODEC_ID_OPUS = 86077,
        @AV_CODEC_ID_COMFORT_NOISE = 86078,
        @AV_CODEC_ID_TAK = 86079,
        @AV_CODEC_ID_METASOUND = 86080,
        @AV_CODEC_ID_PAF_AUDIO = 86081,
        @AV_CODEC_ID_ON2AVC = 86082,
        @AV_CODEC_ID_DSS_SP = 86083,
        @AV_CODEC_ID_FFWAVESYNTH = 88064,
        @AV_CODEC_ID_SONIC = 88065,
        @AV_CODEC_ID_SONIC_LS = 88066,
        @AV_CODEC_ID_EVRC = 88067,
        @AV_CODEC_ID_SMV = 88068,
        @AV_CODEC_ID_DSD_LSBF = 88069,
        @AV_CODEC_ID_DSD_MSBF = 88070,
        @AV_CODEC_ID_DSD_LSBF_PLANAR = 88071,
        @AV_CODEC_ID_DSD_MSBF_PLANAR = 88072,
        @AV_CODEC_ID_4GV = 88073,
        @AV_CODEC_ID_INTERPLAY_ACM = 88074,
        @AV_CODEC_ID_XMA1 = 88075,
        @AV_CODEC_ID_XMA2 = 88076,
        @AV_CODEC_ID_DST = 88077,
        @AV_CODEC_ID_FIRST_SUBTITLE = 94208,
        @AV_CODEC_ID_DVD_SUBTITLE = 94208,
        @AV_CODEC_ID_DVB_SUBTITLE = 94209,
        @AV_CODEC_ID_TEXT = 94210,
        @AV_CODEC_ID_XSUB = 94211,
        @AV_CODEC_ID_SSA = 94212,
        @AV_CODEC_ID_MOV_TEXT = 94213,
        @AV_CODEC_ID_HDMV_PGS_SUBTITLE = 94214,
        @AV_CODEC_ID_DVB_TELETEXT = 94215,
        @AV_CODEC_ID_SRT = 94216,
        @AV_CODEC_ID_MICRODVD = 96256,
        @AV_CODEC_ID_EIA_608 = 96257,
        @AV_CODEC_ID_JACOSUB = 96258,
        @AV_CODEC_ID_SAMI = 96259,
        @AV_CODEC_ID_REALTEXT = 96260,
        @AV_CODEC_ID_STL = 96261,
        @AV_CODEC_ID_SUBVIEWER1 = 96262,
        @AV_CODEC_ID_SUBVIEWER = 96263,
        @AV_CODEC_ID_SUBRIP = 96264,
        @AV_CODEC_ID_WEBVTT = 96265,
        @AV_CODEC_ID_MPL2 = 96266,
        @AV_CODEC_ID_VPLAYER = 96267,
        @AV_CODEC_ID_PJS = 96268,
        @AV_CODEC_ID_ASS = 96269,
        @AV_CODEC_ID_HDMV_TEXT_SUBTITLE = 96270,
        @AV_CODEC_ID_FIRST_UNKNOWN = 98304,
        @AV_CODEC_ID_TTF = 98304,
        @AV_CODEC_ID_SCTE_35 = 98305,
        @AV_CODEC_ID_BINTEXT = 100352,
        @AV_CODEC_ID_XBIN = 100353,
        @AV_CODEC_ID_IDF = 100354,
        @AV_CODEC_ID_OTF = 100355,
        @AV_CODEC_ID_SMPTE_KLV = 100356,
        @AV_CODEC_ID_DVD_NAV = 100357,
        @AV_CODEC_ID_TIMED_ID3 = 100358,
        @AV_CODEC_ID_BIN_DATA = 100359,
        @AV_CODEC_ID_PROBE = 102400,
        @AV_CODEC_ID_MPEG2TS = 131072,
        @AV_CODEC_ID_MPEG4SYSTEMS = 131073,
        @AV_CODEC_ID_FFMETADATA = 135168,
        @AV_CODEC_ID_WRAPPED_AVFRAME = 135169,
    }
    
    public enum Motion_Est_ID : int
    {
        @ME_ZERO = 1,
        @ME_FULL = 2,
        @ME_LOG = 3,
        @ME_PHODS = 4,
        @ME_EPZS = 5,
        @ME_X1 = 6,
        @ME_HEX = 7,
        @ME_UMH = 8,
        @ME_TESA = 9,
        @ME_ITER = 50,
    }
    
    public enum AVDiscard : int
    {
        @AVDISCARD_NONE = -16,
        @AVDISCARD_DEFAULT = 0,
        @AVDISCARD_NONREF = 8,
        @AVDISCARD_BIDIR = 16,
        @AVDISCARD_NONINTRA = 24,
        @AVDISCARD_NONKEY = 32,
        @AVDISCARD_ALL = 48,
    }
    
    public enum AVAudioServiceType : int
    {
        @AV_AUDIO_SERVICE_TYPE_MAIN = 0,
        @AV_AUDIO_SERVICE_TYPE_EFFECTS = 1,
        @AV_AUDIO_SERVICE_TYPE_VISUALLY_IMPAIRED = 2,
        @AV_AUDIO_SERVICE_TYPE_HEARING_IMPAIRED = 3,
        @AV_AUDIO_SERVICE_TYPE_DIALOGUE = 4,
        @AV_AUDIO_SERVICE_TYPE_COMMENTARY = 5,
        @AV_AUDIO_SERVICE_TYPE_EMERGENCY = 6,
        @AV_AUDIO_SERVICE_TYPE_VOICE_OVER = 7,
        @AV_AUDIO_SERVICE_TYPE_KARAOKE = 8,
        @AV_AUDIO_SERVICE_TYPE_NB = 9,
    }
    
    public enum AVPacketSideDataType : int
    {
        @AV_PKT_DATA_PALETTE = 0,
        @AV_PKT_DATA_NEW_EXTRADATA = 1,
        @AV_PKT_DATA_PARAM_CHANGE = 2,
        @AV_PKT_DATA_H263_MB_INFO = 3,
        @AV_PKT_DATA_REPLAYGAIN = 4,
        @AV_PKT_DATA_DISPLAYMATRIX = 5,
        @AV_PKT_DATA_STEREO3D = 6,
        @AV_PKT_DATA_AUDIO_SERVICE_TYPE = 7,
        @AV_PKT_DATA_QUALITY_STATS = 8,
        @AV_PKT_DATA_FALLBACK_TRACK = 9,
        @AV_PKT_DATA_CPB_PROPERTIES = 10,
        @AV_PKT_DATA_SKIP_SAMPLES = 70,
        @AV_PKT_DATA_JP_DUALMONO = 71,
        @AV_PKT_DATA_STRINGS_METADATA = 72,
        @AV_PKT_DATA_SUBTITLE_POSITION = 73,
        @AV_PKT_DATA_MATROSKA_BLOCKADDITIONAL = 74,
        @AV_PKT_DATA_WEBVTT_IDENTIFIER = 75,
        @AV_PKT_DATA_WEBVTT_SETTINGS = 76,
        @AV_PKT_DATA_METADATA_UPDATE = 77,
        @AV_PKT_DATA_MPEGTS_STREAM_ID = 78,
        @AV_PKT_DATA_MASTERING_DISPLAY_METADATA = 79,
    }
    
    public enum AVSideDataParamChangeFlags : int
    {
        @AV_SIDE_DATA_PARAM_CHANGE_CHANNEL_COUNT = 1,
        @AV_SIDE_DATA_PARAM_CHANGE_CHANNEL_LAYOUT = 2,
        @AV_SIDE_DATA_PARAM_CHANGE_SAMPLE_RATE = 4,
        @AV_SIDE_DATA_PARAM_CHANGE_DIMENSIONS = 8,
    }
    
    public enum AVFieldOrder : int
    {
        @AV_FIELD_UNKNOWN = 0,
        @AV_FIELD_PROGRESSIVE = 1,
        @AV_FIELD_TT = 2,
        @AV_FIELD_BB = 3,
        @AV_FIELD_TB = 4,
        @AV_FIELD_BT = 5,
    }
    
    public enum AVSubtitleType : int
    {
        @SUBTITLE_NONE = 0,
        @SUBTITLE_BITMAP = 1,
        @SUBTITLE_TEXT = 2,
        @SUBTITLE_ASS = 3,
    }
    
    public enum AVPictureStructure : int
    {
        @AV_PICTURE_STRUCTURE_UNKNOWN = 0,
        @AV_PICTURE_STRUCTURE_TOP_FIELD = 1,
        @AV_PICTURE_STRUCTURE_BOTTOM_FIELD = 2,
        @AV_PICTURE_STRUCTURE_FRAME = 3,
    }
    
    public enum AVLockOp : int
    {
        @AV_LOCK_CREATE = 0,
        @AV_LOCK_OBTAIN = 1,
        @AV_LOCK_RELEASE = 2,
        @AV_LOCK_DESTROY = 3,
    }
    
    public unsafe static partial class ffmpeg
    {
        public const int LIBAVCODEC_VERSION_MAJOR = 57;
        public const int LIBAVCODEC_VERSION_MINOR = 64;
        public const int LIBAVCODEC_VERSION_MICRO = 100;
        public const bool FF_API_VIMA_DECODER = (LIBAVCODEC_VERSION_MAJOR<58);
        public const bool FF_API_AUDIO_CONVERT = (LIBAVCODEC_VERSION_MAJOR<58);
        public const bool FF_API_AVCODEC_RESAMPLE = FF_API_AUDIO_CONVERT;
        public const bool FF_API_GETCHROMA = (LIBAVCODEC_VERSION_MAJOR<58);
        public const bool FF_API_MISSING_SAMPLE = (LIBAVCODEC_VERSION_MAJOR<58);
        public const bool FF_API_LOWRES = (LIBAVCODEC_VERSION_MAJOR<58);
        public const bool FF_API_CAP_VDPAU = (LIBAVCODEC_VERSION_MAJOR<58);
        public const bool FF_API_BUFS_VDPAU = (LIBAVCODEC_VERSION_MAJOR<58);
        public const bool FF_API_VOXWARE = (LIBAVCODEC_VERSION_MAJOR<58);
        public const bool FF_API_SET_DIMENSIONS = (LIBAVCODEC_VERSION_MAJOR<58);
        public const bool FF_API_DEBUG_MV = (LIBAVCODEC_VERSION_MAJOR<58);
        public const bool FF_API_AC_VLC = (LIBAVCODEC_VERSION_MAJOR<58);
        public const bool FF_API_OLD_MSMPEG4 = (LIBAVCODEC_VERSION_MAJOR<58);
        public const bool FF_API_ASPECT_EXTENDED = (LIBAVCODEC_VERSION_MAJOR<58);
        public const bool FF_API_ARCH_ALPHA = (LIBAVCODEC_VERSION_MAJOR<58);
        public const bool FF_API_ERROR_RATE = (LIBAVCODEC_VERSION_MAJOR<58);
        public const bool FF_API_QSCALE_TYPE = (LIBAVCODEC_VERSION_MAJOR<58);
        public const bool FF_API_MB_TYPE = (LIBAVCODEC_VERSION_MAJOR<58);
        public const bool FF_API_MAX_BFRAMES = (LIBAVCODEC_VERSION_MAJOR<58);
        public const bool FF_API_NEG_LINESIZES = (LIBAVCODEC_VERSION_MAJOR<58);
        public const bool FF_API_EMU_EDGE = (LIBAVCODEC_VERSION_MAJOR<58);
        public const bool FF_API_ARCH_SH4 = (LIBAVCODEC_VERSION_MAJOR<58);
        public const bool FF_API_ARCH_SPARC = (LIBAVCODEC_VERSION_MAJOR<58);
        public const bool FF_API_UNUSED_MEMBERS = (LIBAVCODEC_VERSION_MAJOR<58);
        public const bool FF_API_IDCT_XVIDMMX = (LIBAVCODEC_VERSION_MAJOR<58);
        public const bool FF_API_INPUT_PRESERVED = (LIBAVCODEC_VERSION_MAJOR<58);
        public const bool FF_API_NORMALIZE_AQP = (LIBAVCODEC_VERSION_MAJOR<58);
        public const bool FF_API_GMC = (LIBAVCODEC_VERSION_MAJOR<58);
        public const bool FF_API_MV0 = (LIBAVCODEC_VERSION_MAJOR<58);
        public const bool FF_API_CODEC_NAME = (LIBAVCODEC_VERSION_MAJOR<58);
        public const bool FF_API_AFD = (LIBAVCODEC_VERSION_MAJOR<58);
        public const bool FF_API_VISMV = (LIBAVCODEC_VERSION_MAJOR<58);
        public const bool FF_API_AUDIOENC_DELAY = (LIBAVCODEC_VERSION_MAJOR<58);
        public const bool FF_API_VAAPI_CONTEXT = (LIBAVCODEC_VERSION_MAJOR<58);
        public const bool FF_API_AVCTX_TIMEBASE = (LIBAVCODEC_VERSION_MAJOR<59);
        public const bool FF_API_MPV_OPT = (LIBAVCODEC_VERSION_MAJOR<59);
        public const bool FF_API_STREAM_CODEC_TAG = (LIBAVCODEC_VERSION_MAJOR<59);
        public const bool FF_API_QUANT_BIAS = (LIBAVCODEC_VERSION_MAJOR<59);
        public const bool FF_API_RC_STRATEGY = (LIBAVCODEC_VERSION_MAJOR<59);
        public const bool FF_API_CODED_FRAME = (LIBAVCODEC_VERSION_MAJOR<59);
        public const bool FF_API_MOTION_EST = (LIBAVCODEC_VERSION_MAJOR<59);
        public const bool FF_API_WITHOUT_PREFIX = (LIBAVCODEC_VERSION_MAJOR<59);
        public const bool FF_API_SIDEDATA_ONLY_PKT = (LIBAVCODEC_VERSION_MAJOR<59);
        public const bool FF_API_VDPAU_PROFILE = (LIBAVCODEC_VERSION_MAJOR<59);
        public const bool FF_API_CONVERGENCE_DURATION = (LIBAVCODEC_VERSION_MAJOR<59);
        public const bool FF_API_AVPICTURE = (LIBAVCODEC_VERSION_MAJOR<59);
        public const bool FF_API_AVPACKET_OLD_API = (LIBAVCODEC_VERSION_MAJOR<59);
        public const bool FF_API_RTP_CALLBACK = (LIBAVCODEC_VERSION_MAJOR<59);
        public const bool FF_API_VBV_DELAY = (LIBAVCODEC_VERSION_MAJOR<59);
        public const bool FF_API_CODER_TYPE = (LIBAVCODEC_VERSION_MAJOR<59);
        public const bool FF_API_STAT_BITS = (LIBAVCODEC_VERSION_MAJOR<59);
        public const bool FF_API_PRIVATE_OPT = (LIBAVCODEC_VERSION_MAJOR<59);
        public const bool FF_API_ASS_TIMING = (LIBAVCODEC_VERSION_MAJOR<59);
        public const bool FF_API_OLD_BSF = (LIBAVCODEC_VERSION_MAJOR<59);
        public const bool FF_API_COPY_CONTEXT = (LIBAVCODEC_VERSION_MAJOR<59);
        public const bool FF_API_GET_CONTEXT_DEFAULTS = (LIBAVCODEC_VERSION_MAJOR<59);
        public const bool FF_API_NVENC_OLD_NAME = (LIBAVCODEC_VERSION_MAJOR<59);
        public const int AV_CODEC_PROP_INTRA_ONLY = (1<<0);
        public const int AV_CODEC_PROP_LOSSY = (1<<1);
        public const int AV_CODEC_PROP_LOSSLESS = (1<<2);
        public const int AV_CODEC_PROP_REORDER = (1<<3);
        public const int AV_CODEC_PROP_BITMAP_SUB = (1<<16);
        public const int AV_CODEC_PROP_TEXT_SUB = (1<<17);
        public const int AV_INPUT_BUFFER_PADDING_SIZE = 32;
        public const int AV_INPUT_BUFFER_MIN_SIZE = 16384;
        public const int FF_INPUT_BUFFER_PADDING_SIZE = 32;
        public const int FF_MIN_BUFFER_SIZE = 16384;
        public const int FF_MAX_B_FRAMES = 16;
        public const int AV_CODEC_FLAG_UNALIGNED = (1<<0);
        public const int AV_CODEC_FLAG_QSCALE = (1<<1);
        public const int AV_CODEC_FLAG_4MV = (1<<2);
        public const int AV_CODEC_FLAG_OUTPUT_CORRUPT = (1<<3);
        public const int AV_CODEC_FLAG_QPEL = (1<<4);
        public const int AV_CODEC_FLAG_PASS1 = (1<<9);
        public const int AV_CODEC_FLAG_PASS2 = (1<<10);
        public const int AV_CODEC_FLAG_LOOP_FILTER = (1<<11);
        public const int AV_CODEC_FLAG_GRAY = (1<<13);
        public const int AV_CODEC_FLAG_PSNR = (1<<15);
        public const int AV_CODEC_FLAG_TRUNCATED = (1<<16);
        public const int AV_CODEC_FLAG_INTERLACED_DCT = (1<<18);
        public const int AV_CODEC_FLAG_LOW_DELAY = (1<<19);
        public const int AV_CODEC_FLAG_GLOBAL_HEADER = (1<<22);
        public const int AV_CODEC_FLAG_BITEXACT = (1<<23);
        public const int AV_CODEC_FLAG_AC_PRED = (1<<24);
        public const int AV_CODEC_FLAG_INTERLACED_ME = (1<<29);
        public const uint AV_CODEC_FLAG_CLOSED_GOP = (1U<<31);
        public const int AV_CODEC_FLAG2_FAST = (1<<0);
        public const int AV_CODEC_FLAG2_NO_OUTPUT = (1<<2);
        public const int AV_CODEC_FLAG2_LOCAL_HEADER = (1<<3);
        public const int AV_CODEC_FLAG2_DROP_FRAME_TIMECODE = (1<<13);
        public const int AV_CODEC_FLAG2_CHUNKS = (1<<15);
        public const int AV_CODEC_FLAG2_IGNORE_CROP = (1<<16);
        public const int AV_CODEC_FLAG2_SHOW_ALL = (1<<22);
        public const int AV_CODEC_FLAG2_EXPORT_MVS = (1<<28);
        public const int AV_CODEC_FLAG2_SKIP_MANUAL = (1<<29);
        public const int AV_CODEC_FLAG2_RO_FLUSH_NOOP = (1<<30);
        public const int AV_CODEC_CAP_DRAW_HORIZ_BAND = (1<<0);
        public const int AV_CODEC_CAP_DR1 = (1<<1);
        public const int AV_CODEC_CAP_TRUNCATED = (1<<3);
        public const int AV_CODEC_CAP_DELAY = (1<<5);
        public const int AV_CODEC_CAP_SMALL_LAST_FRAME = (1<<6);
        public const int AV_CODEC_CAP_HWACCEL_VDPAU = (1<<7);
        public const int AV_CODEC_CAP_SUBFRAMES = (1<<8);
        public const int AV_CODEC_CAP_EXPERIMENTAL = (1<<9);
        public const int AV_CODEC_CAP_CHANNEL_CONF = (1<<10);
        public const int AV_CODEC_CAP_FRAME_THREADS = (1<<12);
        public const int AV_CODEC_CAP_SLICE_THREADS = (1<<13);
        public const int AV_CODEC_CAP_PARAM_CHANGE = (1<<14);
        public const int AV_CODEC_CAP_AUTO_THREADS = (1<<15);
        public const int AV_CODEC_CAP_VARIABLE_FRAME_SIZE = (1<<16);
        public const int AV_CODEC_CAP_AVOID_PROBING = (1<<17);
        public const int AV_CODEC_CAP_INTRA_ONLY = 0x40000000;
        public const uint AV_CODEC_CAP_LOSSLESS = 0x80000000;
        public const int CODEC_FLAG_UNALIGNED = AV_CODEC_FLAG_UNALIGNED;
        public const int CODEC_FLAG_QSCALE = AV_CODEC_FLAG_QSCALE;
        public const int CODEC_FLAG_4MV = AV_CODEC_FLAG_4MV;
        public const int CODEC_FLAG_OUTPUT_CORRUPT = AV_CODEC_FLAG_OUTPUT_CORRUPT;
        public const int CODEC_FLAG_QPEL = AV_CODEC_FLAG_QPEL;
        public const int CODEC_FLAG_GMC = 0x0020;
        public const int CODEC_FLAG_MV0 = 0x0040;
        public const int CODEC_FLAG_INPUT_PRESERVED = 0x0100;
        public const int CODEC_FLAG_PASS1 = AV_CODEC_FLAG_PASS1;
        public const int CODEC_FLAG_PASS2 = AV_CODEC_FLAG_PASS2;
        public const int CODEC_FLAG_GRAY = AV_CODEC_FLAG_GRAY;
        public const int CODEC_FLAG_EMU_EDGE = 0x4000;
        public const int CODEC_FLAG_PSNR = AV_CODEC_FLAG_PSNR;
        public const int CODEC_FLAG_TRUNCATED = AV_CODEC_FLAG_TRUNCATED;
        public const int CODEC_FLAG_NORMALIZE_AQP = 0x00020000;
        public const int CODEC_FLAG_INTERLACED_DCT = AV_CODEC_FLAG_INTERLACED_DCT;
        public const int CODEC_FLAG_LOW_DELAY = AV_CODEC_FLAG_LOW_DELAY;
        public const int CODEC_FLAG_GLOBAL_HEADER = AV_CODEC_FLAG_GLOBAL_HEADER;
        public const int CODEC_FLAG_BITEXACT = AV_CODEC_FLAG_BITEXACT;
        public const int CODEC_FLAG_AC_PRED = AV_CODEC_FLAG_AC_PRED;
        public const int CODEC_FLAG_LOOP_FILTER = AV_CODEC_FLAG_LOOP_FILTER;
        public const int CODEC_FLAG_INTERLACED_ME = AV_CODEC_FLAG_INTERLACED_ME;
        public const uint CODEC_FLAG_CLOSED_GOP = AV_CODEC_FLAG_CLOSED_GOP;
        public const int CODEC_FLAG2_FAST = AV_CODEC_FLAG2_FAST;
        public const int CODEC_FLAG2_NO_OUTPUT = AV_CODEC_FLAG2_NO_OUTPUT;
        public const int CODEC_FLAG2_LOCAL_HEADER = AV_CODEC_FLAG2_LOCAL_HEADER;
        public const int CODEC_FLAG2_DROP_FRAME_TIMECODE = AV_CODEC_FLAG2_DROP_FRAME_TIMECODE;
        public const int CODEC_FLAG2_IGNORE_CROP = AV_CODEC_FLAG2_IGNORE_CROP;
        public const int CODEC_FLAG2_CHUNKS = AV_CODEC_FLAG2_CHUNKS;
        public const int CODEC_FLAG2_SHOW_ALL = AV_CODEC_FLAG2_SHOW_ALL;
        public const int CODEC_FLAG2_EXPORT_MVS = AV_CODEC_FLAG2_EXPORT_MVS;
        public const int CODEC_FLAG2_SKIP_MANUAL = AV_CODEC_FLAG2_SKIP_MANUAL;
        public const int CODEC_CAP_DRAW_HORIZ_BAND = AV_CODEC_CAP_DRAW_HORIZ_BAND;
        public const int CODEC_CAP_DR1 = AV_CODEC_CAP_DR1;
        public const int CODEC_CAP_TRUNCATED = AV_CODEC_CAP_TRUNCATED;
        public const int CODEC_CAP_HWACCEL = 0x0010;
        public const int CODEC_CAP_DELAY = AV_CODEC_CAP_DELAY;
        public const int CODEC_CAP_SMALL_LAST_FRAME = AV_CODEC_CAP_SMALL_LAST_FRAME;
        public const int CODEC_CAP_HWACCEL_VDPAU = AV_CODEC_CAP_HWACCEL_VDPAU;
        public const int CODEC_CAP_SUBFRAMES = AV_CODEC_CAP_SUBFRAMES;
        public const int CODEC_CAP_EXPERIMENTAL = AV_CODEC_CAP_EXPERIMENTAL;
        public const int CODEC_CAP_CHANNEL_CONF = AV_CODEC_CAP_CHANNEL_CONF;
        public const int CODEC_CAP_NEG_LINESIZES = 0x0800;
        public const int CODEC_CAP_FRAME_THREADS = AV_CODEC_CAP_FRAME_THREADS;
        public const int CODEC_CAP_SLICE_THREADS = AV_CODEC_CAP_SLICE_THREADS;
        public const int CODEC_CAP_PARAM_CHANGE = AV_CODEC_CAP_PARAM_CHANGE;
        public const int CODEC_CAP_AUTO_THREADS = AV_CODEC_CAP_AUTO_THREADS;
        public const int CODEC_CAP_VARIABLE_FRAME_SIZE = AV_CODEC_CAP_VARIABLE_FRAME_SIZE;
        public const int CODEC_CAP_INTRA_ONLY = AV_CODEC_CAP_INTRA_ONLY;
        public const uint CODEC_CAP_LOSSLESS = AV_CODEC_CAP_LOSSLESS;
        public const int HWACCEL_CODEC_CAP_EXPERIMENTAL = 0x0200;
        public const int MB_TYPE_INTRA4x4 = 0x0001;
        public const int MB_TYPE_INTRA16x16 = 0x0002;
        public const int MB_TYPE_INTRA_PCM = 0x0004;
        public const int MB_TYPE_16x16 = 0x0008;
        public const int MB_TYPE_16x8 = 0x0010;
        public const int MB_TYPE_8x16 = 0x0020;
        public const int MB_TYPE_8x8 = 0x0040;
        public const int MB_TYPE_INTERLACED = 0x0080;
        public const int MB_TYPE_DIRECT2 = 0x0100;
        public const int MB_TYPE_ACPRED = 0x0200;
        public const int MB_TYPE_GMC = 0x0400;
        public const int MB_TYPE_SKIP = 0x0800;
        public const int MB_TYPE_P0L0 = 0x1000;
        public const int MB_TYPE_P1L0 = 0x2000;
        public const int MB_TYPE_P0L1 = 0x4000;
        public const int MB_TYPE_P1L1 = 0x8000;
        public const int MB_TYPE_L0 = (MB_TYPE_P0L0|MB_TYPE_P1L0);
        public const int MB_TYPE_L1 = (MB_TYPE_P0L1|MB_TYPE_P1L1);
        public const int MB_TYPE_L0L1 = (MB_TYPE_L0|MB_TYPE_L1);
        public const int MB_TYPE_QUANT = 0x00010000;
        public const int MB_TYPE_CBP = 0x00020000;
        public const int FF_QSCALE_TYPE_MPEG1 = 0;
        public const int FF_QSCALE_TYPE_MPEG2 = 1;
        public const int FF_QSCALE_TYPE_H264 = 2;
        public const int FF_QSCALE_TYPE_VP56 = 3;
        public const int AV_GET_BUFFER_FLAG_REF = (1<<0);
        public const int AV_PKT_FLAG_KEY = 0x0001;
        public const int AV_PKT_FLAG_CORRUPT = 0x0002;
        public const int AV_PKT_FLAG_DISCARD = 0x0004;
        public const int FF_COMPRESSION_DEFAULT = -1;
        public const int FF_ASPECT_EXTENDED = 15;
        public const int FF_RC_STRATEGY_XVID = 1;
        public const int FF_PRED_LEFT = 0;
        public const int FF_PRED_PLANE = 1;
        public const int FF_PRED_MEDIAN = 2;
        public const int FF_CMP_SAD = 0;
        public const int FF_CMP_SSE = 1;
        public const int FF_CMP_SATD = 2;
        public const int FF_CMP_DCT = 3;
        public const int FF_CMP_PSNR = 4;
        public const int FF_CMP_BIT = 5;
        public const int FF_CMP_RD = 6;
        public const int FF_CMP_ZERO = 7;
        public const int FF_CMP_VSAD = 8;
        public const int FF_CMP_VSSE = 9;
        public const int FF_CMP_NSSE = 10;
        public const int FF_CMP_W53 = 11;
        public const int FF_CMP_W97 = 12;
        public const int FF_CMP_DCTMAX = 13;
        public const int FF_CMP_DCT264 = 14;
        public const int FF_CMP_MEDIAN_SAD = 15;
        public const int FF_CMP_CHROMA = 256;
        public const int FF_DTG_AFD_SAME = 8;
        public const int FF_DTG_AFD_4_3 = 9;
        public const int FF_DTG_AFD_16_9 = 10;
        public const int FF_DTG_AFD_14_9 = 11;
        public const int FF_DTG_AFD_4_3_SP_14_9 = 13;
        public const int FF_DTG_AFD_16_9_SP_14_9 = 14;
        public const int FF_DTG_AFD_SP_4_3 = 15;
        public const int FF_DEFAULT_QUANT_BIAS = 999999;
        public const int SLICE_FLAG_CODED_ORDER = 0x0001;
        public const int SLICE_FLAG_ALLOW_FIELD = 0x0002;
        public const int SLICE_FLAG_ALLOW_PLANE = 0x0004;
        public const int FF_MB_DECISION_SIMPLE = 0;
        public const int FF_MB_DECISION_BITS = 1;
        public const int FF_MB_DECISION_RD = 2;
        public const int FF_CODER_TYPE_VLC = 0;
        public const int FF_CODER_TYPE_AC = 1;
        public const int FF_CODER_TYPE_RAW = 2;
        public const int FF_CODER_TYPE_RLE = 3;
        public const int FF_CODER_TYPE_DEFLATE = 4;
        public const int FF_BUG_AUTODETECT = 1;
        public const int FF_BUG_OLD_MSMPEG4 = 2;
        public const int FF_BUG_XVID_ILACE = 4;
        public const int FF_BUG_UMP4 = 8;
        public const int FF_BUG_NO_PADDING = 16;
        public const int FF_BUG_AMV = 32;
        public const int FF_BUG_AC_VLC = 0;
        public const int FF_BUG_QPEL_CHROMA = 64;
        public const int FF_BUG_STD_QPEL = 128;
        public const int FF_BUG_QPEL_CHROMA2 = 256;
        public const int FF_BUG_DIRECT_BLOCKSIZE = 512;
        public const int FF_BUG_EDGE = 1024;
        public const int FF_BUG_HPEL_CHROMA = 2048;
        public const int FF_BUG_DC_CLIP = 4096;
        public const int FF_BUG_MS = 8192;
        public const int FF_BUG_TRUNCATED = 16384;
        public const int FF_COMPLIANCE_VERY_STRICT = 2;
        public const int FF_COMPLIANCE_STRICT = 1;
        public const int FF_COMPLIANCE_NORMAL = 0;
        public const int FF_COMPLIANCE_UNOFFICIAL = -1;
        public const int FF_COMPLIANCE_EXPERIMENTAL = -2;
        public const int FF_EC_GUESS_MVS = 1;
        public const int FF_EC_DEBLOCK = 2;
        public const int FF_EC_FAVOR_INTER = 256;
        public const int FF_DEBUG_PICT_INFO = 1;
        public const int FF_DEBUG_RC = 2;
        public const int FF_DEBUG_BITSTREAM = 4;
        public const int FF_DEBUG_MB_TYPE = 8;
        public const int FF_DEBUG_QP = 16;
        public const int FF_DEBUG_MV = 32;
        public const int FF_DEBUG_DCT_COEFF = 0x00000040;
        public const int FF_DEBUG_SKIP = 0x00000080;
        public const int FF_DEBUG_STARTCODE = 0x00000100;
        public const int FF_DEBUG_PTS = 0x00000200;
        public const int FF_DEBUG_ER = 0x00000400;
        public const int FF_DEBUG_MMCO = 0x00000800;
        public const int FF_DEBUG_BUGS = 0x00001000;
        public const int FF_DEBUG_VIS_QP = 0x00002000;
        public const int FF_DEBUG_VIS_MB_TYPE = 0x00004000;
        public const int FF_DEBUG_BUFFERS = 0x00008000;
        public const int FF_DEBUG_THREADS = 0x00010000;
        public const int FF_DEBUG_GREEN_MD = 0x00800000;
        public const int FF_DEBUG_NOMC = 0x01000000;
        public const int FF_DEBUG_VIS_MV_P_FOR = 0x00000001;
        public const int FF_DEBUG_VIS_MV_B_FOR = 0x00000002;
        public const int FF_DEBUG_VIS_MV_B_BACK = 0x00000004;
        public const int AV_EF_CRCCHECK = (1<<0);
        public const int AV_EF_BITSTREAM = (1<<1);
        public const int AV_EF_BUFFER = (1<<2);
        public const int AV_EF_EXPLODE = (1<<3);
        public const int AV_EF_IGNORE_ERR = (1<<15);
        public const int AV_EF_CAREFUL = (1<<16);
        public const int AV_EF_COMPLIANT = (1<<17);
        public const int AV_EF_AGGRESSIVE = (1<<18);
        public const int FF_DCT_AUTO = 0;
        public const int FF_DCT_FASTINT = 1;
        public const int FF_DCT_INT = 2;
        public const int FF_DCT_MMX = 3;
        public const int FF_DCT_ALTIVEC = 5;
        public const int FF_DCT_FAAN = 6;
        public const int FF_IDCT_AUTO = 0;
        public const int FF_IDCT_INT = 1;
        public const int FF_IDCT_SIMPLE = 2;
        public const int FF_IDCT_SIMPLEMMX = 3;
        public const int FF_IDCT_ARM = 7;
        public const int FF_IDCT_ALTIVEC = 8;
        public const int FF_IDCT_SH4 = 9;
        public const int FF_IDCT_SIMPLEARM = 10;
        public const int FF_IDCT_IPP = 13;
        public const int FF_IDCT_XVID = 14;
        public const int FF_IDCT_XVIDMMX = 14;
        public const int FF_IDCT_SIMPLEARMV5TE = 16;
        public const int FF_IDCT_SIMPLEARMV6 = 17;
        public const int FF_IDCT_SIMPLEVIS = 18;
        public const int FF_IDCT_FAAN = 20;
        public const int FF_IDCT_SIMPLENEON = 22;
        public const int FF_IDCT_SIMPLEALPHA = 23;
        public const int FF_IDCT_SIMPLEAUTO = 128;
        public const int FF_THREAD_FRAME = 1;
        public const int FF_THREAD_SLICE = 2;
        public const int FF_PROFILE_UNKNOWN = -99;
        public const int FF_PROFILE_RESERVED = -100;
        public const int FF_PROFILE_AAC_MAIN = 0;
        public const int FF_PROFILE_AAC_LOW = 1;
        public const int FF_PROFILE_AAC_SSR = 2;
        public const int FF_PROFILE_AAC_LTP = 3;
        public const int FF_PROFILE_AAC_HE = 4;
        public const int FF_PROFILE_AAC_HE_V2 = 28;
        public const int FF_PROFILE_AAC_LD = 22;
        public const int FF_PROFILE_AAC_ELD = 38;
        public const int FF_PROFILE_MPEG2_AAC_LOW = 128;
        public const int FF_PROFILE_MPEG2_AAC_HE = 131;
        public const int FF_PROFILE_DNXHD = 0;
        public const int FF_PROFILE_DNXHR_LB = 1;
        public const int FF_PROFILE_DNXHR_SQ = 2;
        public const int FF_PROFILE_DNXHR_HQ = 3;
        public const int FF_PROFILE_DNXHR_HQX = 4;
        public const int FF_PROFILE_DNXHR_444 = 5;
        public const int FF_PROFILE_DTS = 20;
        public const int FF_PROFILE_DTS_ES = 30;
        public const int FF_PROFILE_DTS_96_24 = 40;
        public const int FF_PROFILE_DTS_HD_HRA = 50;
        public const int FF_PROFILE_DTS_HD_MA = 60;
        public const int FF_PROFILE_DTS_EXPRESS = 70;
        public const int FF_PROFILE_MPEG2_422 = 0;
        public const int FF_PROFILE_MPEG2_HIGH = 1;
        public const int FF_PROFILE_MPEG2_SS = 2;
        public const int FF_PROFILE_MPEG2_SNR_SCALABLE = 3;
        public const int FF_PROFILE_MPEG2_MAIN = 4;
        public const int FF_PROFILE_MPEG2_SIMPLE = 5;
        public const int FF_PROFILE_H264_CONSTRAINED = (1<<9);
        public const int FF_PROFILE_H264_INTRA = (1<<11);
        public const int FF_PROFILE_H264_BASELINE = 66;
        public const int FF_PROFILE_H264_CONSTRAINED_BASELINE = (66|FF_PROFILE_H264_CONSTRAINED);
        public const int FF_PROFILE_H264_MAIN = 77;
        public const int FF_PROFILE_H264_EXTENDED = 88;
        public const int FF_PROFILE_H264_HIGH = 100;
        public const int FF_PROFILE_H264_HIGH_10 = 110;
        public const int FF_PROFILE_H264_HIGH_10_INTRA = (110|FF_PROFILE_H264_INTRA);
        public const int FF_PROFILE_H264_MULTIVIEW_HIGH = 118;
        public const int FF_PROFILE_H264_HIGH_422 = 122;
        public const int FF_PROFILE_H264_HIGH_422_INTRA = (122|FF_PROFILE_H264_INTRA);
        public const int FF_PROFILE_H264_STEREO_HIGH = 128;
        public const int FF_PROFILE_H264_HIGH_444 = 144;
        public const int FF_PROFILE_H264_HIGH_444_PREDICTIVE = 244;
        public const int FF_PROFILE_H264_HIGH_444_INTRA = (244|FF_PROFILE_H264_INTRA);
        public const int FF_PROFILE_H264_CAVLC_444 = 44;
        public const int FF_PROFILE_VC1_SIMPLE = 0;
        public const int FF_PROFILE_VC1_MAIN = 1;
        public const int FF_PROFILE_VC1_COMPLEX = 2;
        public const int FF_PROFILE_VC1_ADVANCED = 3;
        public const int FF_PROFILE_MPEG4_SIMPLE = 0;
        public const int FF_PROFILE_MPEG4_SIMPLE_SCALABLE = 1;
        public const int FF_PROFILE_MPEG4_CORE = 2;
        public const int FF_PROFILE_MPEG4_MAIN = 3;
        public const int FF_PROFILE_MPEG4_N_BIT = 4;
        public const int FF_PROFILE_MPEG4_SCALABLE_TEXTURE = 5;
        public const int FF_PROFILE_MPEG4_SIMPLE_FACE_ANIMATION = 6;
        public const int FF_PROFILE_MPEG4_BASIC_ANIMATED_TEXTURE = 7;
        public const int FF_PROFILE_MPEG4_HYBRID = 8;
        public const int FF_PROFILE_MPEG4_ADVANCED_REAL_TIME = 9;
        public const int FF_PROFILE_MPEG4_CORE_SCALABLE = 10;
        public const int FF_PROFILE_MPEG4_ADVANCED_CODING = 11;
        public const int FF_PROFILE_MPEG4_ADVANCED_CORE = 12;
        public const int FF_PROFILE_MPEG4_ADVANCED_SCALABLE_TEXTURE = 13;
        public const int FF_PROFILE_MPEG4_SIMPLE_STUDIO = 14;
        public const int FF_PROFILE_MPEG4_ADVANCED_SIMPLE = 15;
        public const int FF_PROFILE_JPEG2000_CSTREAM_RESTRICTION_0 = 1;
        public const int FF_PROFILE_JPEG2000_CSTREAM_RESTRICTION_1 = 2;
        public const int FF_PROFILE_JPEG2000_CSTREAM_NO_RESTRICTION = 32768;
        public const int FF_PROFILE_JPEG2000_DCINEMA_2K = 3;
        public const int FF_PROFILE_JPEG2000_DCINEMA_4K = 4;
        public const int FF_PROFILE_VP9_0 = 0;
        public const int FF_PROFILE_VP9_1 = 1;
        public const int FF_PROFILE_VP9_2 = 2;
        public const int FF_PROFILE_VP9_3 = 3;
        public const int FF_PROFILE_HEVC_MAIN = 1;
        public const int FF_PROFILE_HEVC_MAIN_10 = 2;
        public const int FF_PROFILE_HEVC_MAIN_STILL_PICTURE = 3;
        public const int FF_PROFILE_HEVC_REXT = 4;
        public const int FF_LEVEL_UNKNOWN = -99;
        public const int FF_SUB_CHARENC_MODE_DO_NOTHING = -1;
        public const int FF_SUB_CHARENC_MODE_AUTOMATIC = 0;
        public const int FF_SUB_CHARENC_MODE_PRE_DECODER = 1;
        public const int FF_CODEC_PROPERTY_LOSSLESS = 0x00000001;
        public const int FF_CODEC_PROPERTY_CLOSED_CAPTIONS = 0x00000002;
        public const int FF_SUB_TEXT_FMT_ASS = 0;
        public const int FF_SUB_TEXT_FMT_ASS_WITH_TIMINGS = 1;
        public const int AV_HWACCEL_FLAG_IGNORE_LEVEL = (1<<0);
        public const int AV_HWACCEL_FLAG_ALLOW_HIGH_DEPTH = (1<<1);
        public const int AV_SUBTITLE_FLAG_FORCED = 0x00000001;
        public const int AV_PARSER_PTS_NB = 4;
        public const int PARSER_FLAG_COMPLETE_FRAMES = 0x0001;
        public const int PARSER_FLAG_ONCE = 0x0002;
        public const int PARSER_FLAG_FETCHED_OFFSET = 0x0004;
        public const int PARSER_FLAG_USE_CODEC_TS = 0x1000;
        private const string libavcodec = "avcodec-57";
        
        [DllImport(libavcodec, EntryPoint = "av_codec_get_pkt_timebase", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern AVRational av_codec_get_pkt_timebase(AVCodecContext* @avctx);
        
        [DllImport(libavcodec, EntryPoint = "av_codec_set_pkt_timebase", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern void av_codec_set_pkt_timebase(AVCodecContext* @avctx, AVRational @val);
        
        [DllImport(libavcodec, EntryPoint = "av_codec_get_codec_descriptor", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern AVCodecDescriptor* av_codec_get_codec_descriptor(AVCodecContext* @avctx);
        
        [DllImport(libavcodec, EntryPoint = "av_codec_set_codec_descriptor", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern void av_codec_set_codec_descriptor(AVCodecContext* @avctx, AVCodecDescriptor* @desc);
        
        [DllImport(libavcodec, EntryPoint = "av_codec_get_codec_properties", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern uint av_codec_get_codec_properties(AVCodecContext* @avctx);
        
        [DllImport(libavcodec, EntryPoint = "av_codec_get_lowres", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern int av_codec_get_lowres(AVCodecContext* @avctx);
        
        [DllImport(libavcodec, EntryPoint = "av_codec_set_lowres", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern void av_codec_set_lowres(AVCodecContext* @avctx, int @val);
        
        [DllImport(libavcodec, EntryPoint = "av_codec_get_seek_preroll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern int av_codec_get_seek_preroll(AVCodecContext* @avctx);
        
        [DllImport(libavcodec, EntryPoint = "av_codec_set_seek_preroll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern void av_codec_set_seek_preroll(AVCodecContext* @avctx, int @val);
        
        [DllImport(libavcodec, EntryPoint = "av_codec_get_chroma_intra_matrix", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern ushort* av_codec_get_chroma_intra_matrix(AVCodecContext* @avctx);
        
        [DllImport(libavcodec, EntryPoint = "av_codec_set_chroma_intra_matrix", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern void av_codec_set_chroma_intra_matrix(AVCodecContext* @avctx, ushort* @val);
        
        [DllImport(libavcodec, EntryPoint = "av_codec_get_max_lowres", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern int av_codec_get_max_lowres(AVCodec* @codec);
        
        [DllImport(libavcodec, EntryPoint = "av_codec_next", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern AVCodec* av_codec_next(AVCodec* @c);
        
        [DllImport(libavcodec, EntryPoint = "avcodec_version", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern uint avcodec_version();
        
        [DllImport(libavcodec, EntryPoint = "avcodec_configuration", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        [return: MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof(ConstCharPtrMarshaler))]
        public static extern string avcodec_configuration();
        
        [DllImport(libavcodec, EntryPoint = "avcodec_license", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        [return: MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof(ConstCharPtrMarshaler))]
        public static extern string avcodec_license();
        
        [DllImport(libavcodec, EntryPoint = "avcodec_register", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern void avcodec_register(AVCodec* @codec);
        
        [DllImport(libavcodec, EntryPoint = "avcodec_register_all", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern void avcodec_register_all();
        
        [DllImport(libavcodec, EntryPoint = "avcodec_alloc_context3", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern AVCodecContext* avcodec_alloc_context3(AVCodec* @codec);
        
        [DllImport(libavcodec, EntryPoint = "avcodec_free_context", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern void avcodec_free_context(AVCodecContext** @avctx);
        
        [DllImport(libavcodec, EntryPoint = "avcodec_get_context_defaults3", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern int avcodec_get_context_defaults3(AVCodecContext* @s, AVCodec* @codec);
        
        [DllImport(libavcodec, EntryPoint = "avcodec_get_class", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern AVClass* avcodec_get_class();
        
        [DllImport(libavcodec, EntryPoint = "avcodec_get_frame_class", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern AVClass* avcodec_get_frame_class();
        
        [DllImport(libavcodec, EntryPoint = "avcodec_get_subtitle_rect_class", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern AVClass* avcodec_get_subtitle_rect_class();
        
        [DllImport(libavcodec, EntryPoint = "avcodec_copy_context", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern int avcodec_copy_context(AVCodecContext* @dest, AVCodecContext* @src);
        
        [DllImport(libavcodec, EntryPoint = "avcodec_parameters_alloc", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern AVCodecParameters* avcodec_parameters_alloc();
        
        [DllImport(libavcodec, EntryPoint = "avcodec_parameters_free", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern void avcodec_parameters_free(AVCodecParameters** @par);
        
        [DllImport(libavcodec, EntryPoint = "avcodec_parameters_copy", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern int avcodec_parameters_copy(AVCodecParameters* @dst, AVCodecParameters* @src);
        
        [DllImport(libavcodec, EntryPoint = "avcodec_parameters_from_context", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern int avcodec_parameters_from_context(AVCodecParameters* @par, AVCodecContext* @codec);
        
        [DllImport(libavcodec, EntryPoint = "avcodec_parameters_to_context", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern int avcodec_parameters_to_context(AVCodecContext* @codec, AVCodecParameters* @par);
        
        [DllImport(libavcodec, EntryPoint = "avcodec_open2", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern int avcodec_open2(AVCodecContext* @avctx, AVCodec* @codec, AVDictionary** @options);
        
        [DllImport(libavcodec, EntryPoint = "avcodec_close", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern int avcodec_close(AVCodecContext* @avctx);
        
        [DllImport(libavcodec, EntryPoint = "avsubtitle_free", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern void avsubtitle_free(AVSubtitle* @sub);
        
        [DllImport(libavcodec, EntryPoint = "av_packet_alloc", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern AVPacket* av_packet_alloc();
        
        [DllImport(libavcodec, EntryPoint = "av_packet_clone", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern AVPacket* av_packet_clone(AVPacket* @src);
        
        [DllImport(libavcodec, EntryPoint = "av_packet_free", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern void av_packet_free(AVPacket** @pkt);
        
        [DllImport(libavcodec, EntryPoint = "av_init_packet", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern void av_init_packet(AVPacket* @pkt);
        
        [DllImport(libavcodec, EntryPoint = "av_new_packet", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern int av_new_packet(AVPacket* @pkt, int @size);
        
        [DllImport(libavcodec, EntryPoint = "av_shrink_packet", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern void av_shrink_packet(AVPacket* @pkt, int @size);
        
        [DllImport(libavcodec, EntryPoint = "av_grow_packet", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern int av_grow_packet(AVPacket* @pkt, int @grow_by);
        
        [DllImport(libavcodec, EntryPoint = "av_packet_from_data", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern int av_packet_from_data(AVPacket* @pkt, sbyte* @data, int @size);
        
        [DllImport(libavcodec, EntryPoint = "av_dup_packet", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern int av_dup_packet(AVPacket* @pkt);
        
        [DllImport(libavcodec, EntryPoint = "av_copy_packet", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern int av_copy_packet(AVPacket* @dst, AVPacket* @src);
        
        [DllImport(libavcodec, EntryPoint = "av_copy_packet_side_data", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern int av_copy_packet_side_data(AVPacket* @dst, AVPacket* @src);
        
        [DllImport(libavcodec, EntryPoint = "av_free_packet", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern void av_free_packet(AVPacket* @pkt);
        
        [DllImport(libavcodec, EntryPoint = "av_packet_new_side_data", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern sbyte* av_packet_new_side_data(AVPacket* @pkt, AVPacketSideDataType @type, int @size);
        
        [DllImport(libavcodec, EntryPoint = "av_packet_add_side_data", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern int av_packet_add_side_data(AVPacket* @pkt, AVPacketSideDataType @type, sbyte* @data, ulong @size);
        
        [DllImport(libavcodec, EntryPoint = "av_packet_shrink_side_data", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern int av_packet_shrink_side_data(AVPacket* @pkt, AVPacketSideDataType @type, int @size);
        
        [DllImport(libavcodec, EntryPoint = "av_packet_get_side_data", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern sbyte* av_packet_get_side_data(AVPacket* @pkt, AVPacketSideDataType @type, int* @size);
        
        [DllImport(libavcodec, EntryPoint = "av_packet_merge_side_data", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern int av_packet_merge_side_data(AVPacket* @pkt);
        
        [DllImport(libavcodec, EntryPoint = "av_packet_split_side_data", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern int av_packet_split_side_data(AVPacket* @pkt);
        
        [DllImport(libavcodec, EntryPoint = "av_packet_side_data_name", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        [return: MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof(ConstCharPtrMarshaler))]
        public static extern string av_packet_side_data_name(AVPacketSideDataType @type);
        
        [DllImport(libavcodec, EntryPoint = "av_packet_pack_dictionary", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern sbyte* av_packet_pack_dictionary(AVDictionary* @dict, int* @size);
        
        [DllImport(libavcodec, EntryPoint = "av_packet_unpack_dictionary", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern int av_packet_unpack_dictionary(sbyte* @data, int @size, AVDictionary** @dict);
        
        [DllImport(libavcodec, EntryPoint = "av_packet_free_side_data", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern void av_packet_free_side_data(AVPacket* @pkt);
        
        [DllImport(libavcodec, EntryPoint = "av_packet_ref", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern int av_packet_ref(AVPacket* @dst, AVPacket* @src);
        
        [DllImport(libavcodec, EntryPoint = "av_packet_unref", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern void av_packet_unref(AVPacket* @pkt);
        
        [DllImport(libavcodec, EntryPoint = "av_packet_move_ref", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern void av_packet_move_ref(AVPacket* @dst, AVPacket* @src);
        
        [DllImport(libavcodec, EntryPoint = "av_packet_copy_props", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern int av_packet_copy_props(AVPacket* @dst, AVPacket* @src);
        
        [DllImport(libavcodec, EntryPoint = "av_packet_rescale_ts", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern void av_packet_rescale_ts(AVPacket* @pkt, AVRational @tb_src, AVRational @tb_dst);
        
        [DllImport(libavcodec, EntryPoint = "avcodec_find_decoder", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern AVCodec* avcodec_find_decoder(AVCodecID @id);
        
        [DllImport(libavcodec, EntryPoint = "avcodec_find_decoder_by_name", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern AVCodec* avcodec_find_decoder_by_name([MarshalAs(UnmanagedType.LPStr)] string @name);
        
        [DllImport(libavcodec, EntryPoint = "avcodec_default_get_buffer2", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern int avcodec_default_get_buffer2(AVCodecContext* @s, AVFrame* @frame, int @flags);
        
        [DllImport(libavcodec, EntryPoint = "avcodec_get_edge_width", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern uint avcodec_get_edge_width();
        
        [DllImport(libavcodec, EntryPoint = "avcodec_align_dimensions", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern void avcodec_align_dimensions(AVCodecContext* @s, int* @width, int* @height);
        
        [DllImport(libavcodec, EntryPoint = "avcodec_align_dimensions2", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern void avcodec_align_dimensions2(AVCodecContext* @s, int* @width, int* @height, [MarshalAs(UnmanagedType.LPArray, SizeConst=8)] int[] @linesize_align);
        
        [DllImport(libavcodec, EntryPoint = "avcodec_enum_to_chroma_pos", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern int avcodec_enum_to_chroma_pos(int* @xpos, int* @ypos, AVChromaLocation @pos);
        
        [DllImport(libavcodec, EntryPoint = "avcodec_chroma_pos_to_enum", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern AVChromaLocation avcodec_chroma_pos_to_enum(int @xpos, int @ypos);
        
        [DllImport(libavcodec, EntryPoint = "avcodec_decode_audio4", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern int avcodec_decode_audio4(AVCodecContext* @avctx, AVFrame* @frame, int* @got_frame_ptr, AVPacket* @avpkt);
        
        [DllImport(libavcodec, EntryPoint = "avcodec_decode_video2", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern int avcodec_decode_video2(AVCodecContext* @avctx, AVFrame* @picture, int* @got_picture_ptr, AVPacket* @avpkt);
        
        [DllImport(libavcodec, EntryPoint = "avcodec_decode_subtitle2", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern int avcodec_decode_subtitle2(AVCodecContext* @avctx, AVSubtitle* @sub, int* @got_sub_ptr, AVPacket* @avpkt);
        
        [DllImport(libavcodec, EntryPoint = "avcodec_send_packet", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern int avcodec_send_packet(AVCodecContext* @avctx, AVPacket* @avpkt);
        
        [DllImport(libavcodec, EntryPoint = "avcodec_receive_frame", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern int avcodec_receive_frame(AVCodecContext* @avctx, AVFrame* @frame);
        
        [DllImport(libavcodec, EntryPoint = "avcodec_send_frame", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern int avcodec_send_frame(AVCodecContext* @avctx, AVFrame* @frame);
        
        [DllImport(libavcodec, EntryPoint = "avcodec_receive_packet", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern int avcodec_receive_packet(AVCodecContext* @avctx, AVPacket* @avpkt);
        
        [DllImport(libavcodec, EntryPoint = "av_parser_next", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern AVCodecParser* av_parser_next(AVCodecParser* @c);
        
        [DllImport(libavcodec, EntryPoint = "av_register_codec_parser", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern void av_register_codec_parser(AVCodecParser* @parser);
        
        [DllImport(libavcodec, EntryPoint = "av_parser_init", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern AVCodecParserContext* av_parser_init(int @codec_id);
        
        [DllImport(libavcodec, EntryPoint = "av_parser_parse2", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern int av_parser_parse2(AVCodecParserContext* @s, AVCodecContext* @avctx, sbyte** @poutbuf, int* @poutbuf_size, sbyte* @buf, int @buf_size, long @pts, long @dts, long @pos);
        
        [DllImport(libavcodec, EntryPoint = "av_parser_change", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern int av_parser_change(AVCodecParserContext* @s, AVCodecContext* @avctx, sbyte** @poutbuf, int* @poutbuf_size, sbyte* @buf, int @buf_size, int @keyframe);
        
        [DllImport(libavcodec, EntryPoint = "av_parser_close", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern void av_parser_close(AVCodecParserContext* @s);
        
        [DllImport(libavcodec, EntryPoint = "avcodec_find_encoder", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern AVCodec* avcodec_find_encoder(AVCodecID @id);
        
        [DllImport(libavcodec, EntryPoint = "avcodec_find_encoder_by_name", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern AVCodec* avcodec_find_encoder_by_name([MarshalAs(UnmanagedType.LPStr)] string @name);
        
        [DllImport(libavcodec, EntryPoint = "avcodec_encode_audio2", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern int avcodec_encode_audio2(AVCodecContext* @avctx, AVPacket* @avpkt, AVFrame* @frame, int* @got_packet_ptr);
        
        [DllImport(libavcodec, EntryPoint = "avcodec_encode_video2", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern int avcodec_encode_video2(AVCodecContext* @avctx, AVPacket* @avpkt, AVFrame* @frame, int* @got_packet_ptr);
        
        [DllImport(libavcodec, EntryPoint = "avcodec_encode_subtitle", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern int avcodec_encode_subtitle(AVCodecContext* @avctx, sbyte* @buf, int @buf_size, AVSubtitle* @sub);
        
        [DllImport(libavcodec, EntryPoint = "av_audio_resample_init", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern ReSampleContext* av_audio_resample_init(int @output_channels, int @input_channels, int @output_rate, int @input_rate, AVSampleFormat @sample_fmt_out, AVSampleFormat @sample_fmt_in, int @filter_length, int @log2_phase_count, int @linear, double @cutoff);
        
        [DllImport(libavcodec, EntryPoint = "audio_resample", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern int audio_resample(ReSampleContext* @s, short* @output, short* @input, int @nb_samples);
        
        [DllImport(libavcodec, EntryPoint = "audio_resample_close", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern void audio_resample_close(ReSampleContext* @s);
        
        [DllImport(libavcodec, EntryPoint = "av_resample_init", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern AVResampleContext* av_resample_init(int @out_rate, int @in_rate, int @filter_length, int @log2_phase_count, int @linear, double @cutoff);
        
        [DllImport(libavcodec, EntryPoint = "av_resample", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern int av_resample(AVResampleContext* @c, short* @dst, short* @src, int* @consumed, int @src_size, int @dst_size, int @update_ctx);
        
        [DllImport(libavcodec, EntryPoint = "av_resample_compensate", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern void av_resample_compensate(AVResampleContext* @c, int @sample_delta, int @compensation_distance);
        
        [DllImport(libavcodec, EntryPoint = "av_resample_close", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern void av_resample_close(AVResampleContext* @c);
        
        [DllImport(libavcodec, EntryPoint = "avpicture_alloc", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern int avpicture_alloc(AVPicture* @picture, AVPixelFormat @pix_fmt, int @width, int @height);
        
        [DllImport(libavcodec, EntryPoint = "avpicture_free", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern void avpicture_free(AVPicture* @picture);
        
        [DllImport(libavcodec, EntryPoint = "avpicture_fill", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern int avpicture_fill(AVPicture* @picture, sbyte* @ptr, AVPixelFormat @pix_fmt, int @width, int @height);
        
        [DllImport(libavcodec, EntryPoint = "avpicture_layout", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern int avpicture_layout(AVPicture* @src, AVPixelFormat @pix_fmt, int @width, int @height, sbyte* @dest, int @dest_size);
        
        [DllImport(libavcodec, EntryPoint = "avpicture_get_size", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern int avpicture_get_size(AVPixelFormat @pix_fmt, int @width, int @height);
        
        [DllImport(libavcodec, EntryPoint = "av_picture_copy", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern void av_picture_copy(AVPicture* @dst, AVPicture* @src, AVPixelFormat @pix_fmt, int @width, int @height);
        
        [DllImport(libavcodec, EntryPoint = "av_picture_crop", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern int av_picture_crop(AVPicture* @dst, AVPicture* @src, AVPixelFormat @pix_fmt, int @top_band, int @left_band);
        
        [DllImport(libavcodec, EntryPoint = "av_picture_pad", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern int av_picture_pad(AVPicture* @dst, AVPicture* @src, int @height, int @width, AVPixelFormat @pix_fmt, int @padtop, int @padbottom, int @padleft, int @padright, int* @color);
        
        [DllImport(libavcodec, EntryPoint = "avcodec_get_chroma_sub_sample", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern void avcodec_get_chroma_sub_sample(AVPixelFormat @pix_fmt, int* @h_shift, int* @v_shift);
        
        [DllImport(libavcodec, EntryPoint = "avcodec_pix_fmt_to_codec_tag", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern uint avcodec_pix_fmt_to_codec_tag(AVPixelFormat @pix_fmt);
        
        [DllImport(libavcodec, EntryPoint = "avcodec_get_pix_fmt_loss", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern int avcodec_get_pix_fmt_loss(AVPixelFormat @dst_pix_fmt, AVPixelFormat @src_pix_fmt, int @has_alpha);
        
        [DllImport(libavcodec, EntryPoint = "avcodec_find_best_pix_fmt_of_list", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern AVPixelFormat avcodec_find_best_pix_fmt_of_list(AVPixelFormat* @pix_fmt_list, AVPixelFormat @src_pix_fmt, int @has_alpha, int* @loss_ptr);
        
        [DllImport(libavcodec, EntryPoint = "avcodec_find_best_pix_fmt_of_2", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern AVPixelFormat avcodec_find_best_pix_fmt_of_2(AVPixelFormat @dst_pix_fmt1, AVPixelFormat @dst_pix_fmt2, AVPixelFormat @src_pix_fmt, int @has_alpha, int* @loss_ptr);
        
        [DllImport(libavcodec, EntryPoint = "avcodec_find_best_pix_fmt2", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern AVPixelFormat avcodec_find_best_pix_fmt2(AVPixelFormat @dst_pix_fmt1, AVPixelFormat @dst_pix_fmt2, AVPixelFormat @src_pix_fmt, int @has_alpha, int* @loss_ptr);
        
        [DllImport(libavcodec, EntryPoint = "avcodec_default_get_format", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern AVPixelFormat avcodec_default_get_format(AVCodecContext* @s, AVPixelFormat* @fmt);
        
        [DllImport(libavcodec, EntryPoint = "avcodec_set_dimensions", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern void avcodec_set_dimensions(AVCodecContext* @s, int @width, int @height);
        
        [DllImport(libavcodec, EntryPoint = "av_get_codec_tag_string", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern ulong av_get_codec_tag_string(IntPtr @buf, ulong @buf_size, uint @codec_tag);
        
        [DllImport(libavcodec, EntryPoint = "avcodec_string", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern void avcodec_string(IntPtr @buf, int @buf_size, AVCodecContext* @enc, int @encode);
        
        [DllImport(libavcodec, EntryPoint = "av_get_profile_name", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        [return: MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof(ConstCharPtrMarshaler))]
        public static extern string av_get_profile_name(AVCodec* @codec, int @profile);
        
        [DllImport(libavcodec, EntryPoint = "avcodec_profile_name", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        [return: MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof(ConstCharPtrMarshaler))]
        public static extern string avcodec_profile_name(AVCodecID @codec_id, int @profile);
        
        [DllImport(libavcodec, EntryPoint = "avcodec_default_execute", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern int avcodec_default_execute(AVCodecContext* @c, IntPtr @func, void* @arg, int* @ret, int @count, int @size);
        
        [DllImport(libavcodec, EntryPoint = "avcodec_default_execute2", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern int avcodec_default_execute2(AVCodecContext* @c, IntPtr @func, void* @arg, int* @ret, int @count);
        
        [DllImport(libavcodec, EntryPoint = "avcodec_fill_audio_frame", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern int avcodec_fill_audio_frame(AVFrame* @frame, int @nb_channels, AVSampleFormat @sample_fmt, sbyte* @buf, int @buf_size, int @align);
        
        [DllImport(libavcodec, EntryPoint = "avcodec_flush_buffers", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern void avcodec_flush_buffers(AVCodecContext* @avctx);
        
        [DllImport(libavcodec, EntryPoint = "av_get_bits_per_sample", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern int av_get_bits_per_sample(AVCodecID @codec_id);
        
        [DllImport(libavcodec, EntryPoint = "av_get_pcm_codec", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern AVCodecID av_get_pcm_codec(AVSampleFormat @fmt, int @be);
        
        [DllImport(libavcodec, EntryPoint = "av_get_exact_bits_per_sample", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern int av_get_exact_bits_per_sample(AVCodecID @codec_id);
        
        [DllImport(libavcodec, EntryPoint = "av_get_audio_frame_duration", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern int av_get_audio_frame_duration(AVCodecContext* @avctx, int @frame_bytes);
        
        [DllImport(libavcodec, EntryPoint = "av_get_audio_frame_duration2", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern int av_get_audio_frame_duration2(AVCodecParameters* @par, int @frame_bytes);
        
        [DllImport(libavcodec, EntryPoint = "av_register_bitstream_filter", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern void av_register_bitstream_filter(AVBitStreamFilter* @bsf);
        
        [DllImport(libavcodec, EntryPoint = "av_bitstream_filter_init", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern AVBitStreamFilterContext* av_bitstream_filter_init([MarshalAs(UnmanagedType.LPStr)] string @name);
        
        [DllImport(libavcodec, EntryPoint = "av_bitstream_filter_filter", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern int av_bitstream_filter_filter(AVBitStreamFilterContext* @bsfc, AVCodecContext* @avctx, [MarshalAs(UnmanagedType.LPStr)] string @args, sbyte** @poutbuf, int* @poutbuf_size, sbyte* @buf, int @buf_size, int @keyframe);
        
        [DllImport(libavcodec, EntryPoint = "av_bitstream_filter_close", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern void av_bitstream_filter_close(AVBitStreamFilterContext* @bsf);
        
        [DllImport(libavcodec, EntryPoint = "av_bitstream_filter_next", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern AVBitStreamFilter* av_bitstream_filter_next(AVBitStreamFilter* @f);
        
        [DllImport(libavcodec, EntryPoint = "av_bsf_get_by_name", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern AVBitStreamFilter* av_bsf_get_by_name([MarshalAs(UnmanagedType.LPStr)] string @name);
        
        [DllImport(libavcodec, EntryPoint = "av_bsf_next", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern AVBitStreamFilter* av_bsf_next(void** @opaque);
        
        [DllImport(libavcodec, EntryPoint = "av_bsf_alloc", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern int av_bsf_alloc(AVBitStreamFilter* @filter, AVBSFContext** @ctx);
        
        [DllImport(libavcodec, EntryPoint = "av_bsf_init", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern int av_bsf_init(AVBSFContext* @ctx);
        
        [DllImport(libavcodec, EntryPoint = "av_bsf_send_packet", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern int av_bsf_send_packet(AVBSFContext* @ctx, AVPacket* @pkt);
        
        [DllImport(libavcodec, EntryPoint = "av_bsf_receive_packet", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern int av_bsf_receive_packet(AVBSFContext* @ctx, AVPacket* @pkt);
        
        [DllImport(libavcodec, EntryPoint = "av_bsf_free", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern void av_bsf_free(AVBSFContext** @ctx);
        
        [DllImport(libavcodec, EntryPoint = "av_bsf_get_class", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern AVClass* av_bsf_get_class();
        
        [DllImport(libavcodec, EntryPoint = "av_bsf_list_alloc", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern AVBSFList* av_bsf_list_alloc();
        
        [DllImport(libavcodec, EntryPoint = "av_bsf_list_free", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern void av_bsf_list_free(AVBSFList** @lst);
        
        [DllImport(libavcodec, EntryPoint = "av_bsf_list_append", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern int av_bsf_list_append(AVBSFList* @lst, AVBSFContext* @bsf);
        
        [DllImport(libavcodec, EntryPoint = "av_bsf_list_append2", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern int av_bsf_list_append2(AVBSFList* @lst, [MarshalAs(UnmanagedType.LPStr)] string @bsf_name, AVDictionary** @options);
        
        [DllImport(libavcodec, EntryPoint = "av_bsf_list_finalize", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern int av_bsf_list_finalize(AVBSFList** @lst, AVBSFContext** @bsf);
        
        [DllImport(libavcodec, EntryPoint = "av_bsf_list_parse_str", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern int av_bsf_list_parse_str([MarshalAs(UnmanagedType.LPStr)] string @str, AVBSFContext** @bsf);
        
        [DllImport(libavcodec, EntryPoint = "av_bsf_get_null_filter", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern int av_bsf_get_null_filter(AVBSFContext** @bsf);
        
        [DllImport(libavcodec, EntryPoint = "av_fast_padded_malloc", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern void av_fast_padded_malloc(void* @ptr, uint* @size, ulong @min_size);
        
        [DllImport(libavcodec, EntryPoint = "av_fast_padded_mallocz", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern void av_fast_padded_mallocz(void* @ptr, uint* @size, ulong @min_size);
        
        [DllImport(libavcodec, EntryPoint = "av_xiphlacing", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern uint av_xiphlacing(sbyte* @s, uint @v);
        
        [DllImport(libavcodec, EntryPoint = "av_log_missing_feature", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern void av_log_missing_feature(void* @avc, [MarshalAs(UnmanagedType.LPStr)] string @feature, int @want_sample);
        
        [DllImport(libavcodec, EntryPoint = "av_log_ask_for_sample", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern void av_log_ask_for_sample(void* @avc, [MarshalAs(UnmanagedType.LPStr)] string @msg);
        
        [DllImport(libavcodec, EntryPoint = "av_register_hwaccel", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern void av_register_hwaccel(AVHWAccel* @hwaccel);
        
        [DllImport(libavcodec, EntryPoint = "av_hwaccel_next", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern AVHWAccel* av_hwaccel_next(AVHWAccel* @hwaccel);
        
        [DllImport(libavcodec, EntryPoint = "av_lockmgr_register", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern int av_lockmgr_register(IntPtr @cb);
        
        [DllImport(libavcodec, EntryPoint = "avcodec_get_type", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern AVMediaType avcodec_get_type(AVCodecID @codec_id);
        
        [DllImport(libavcodec, EntryPoint = "avcodec_get_name", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        [return: MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof(ConstCharPtrMarshaler))]
        public static extern string avcodec_get_name(AVCodecID @id);
        
        [DllImport(libavcodec, EntryPoint = "avcodec_is_open", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern int avcodec_is_open(AVCodecContext* @s);
        
        [DllImport(libavcodec, EntryPoint = "av_codec_is_encoder", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern int av_codec_is_encoder(AVCodec* @codec);
        
        [DllImport(libavcodec, EntryPoint = "av_codec_is_decoder", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern int av_codec_is_decoder(AVCodec* @codec);
        
        [DllImport(libavcodec, EntryPoint = "avcodec_descriptor_get", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern AVCodecDescriptor* avcodec_descriptor_get(AVCodecID @id);
        
        [DllImport(libavcodec, EntryPoint = "avcodec_descriptor_next", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern AVCodecDescriptor* avcodec_descriptor_next(AVCodecDescriptor* @prev);
        
        [DllImport(libavcodec, EntryPoint = "avcodec_descriptor_get_by_name", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern AVCodecDescriptor* avcodec_descriptor_get_by_name([MarshalAs(UnmanagedType.LPStr)] string @name);
        
        [DllImport(libavcodec, EntryPoint = "av_cpb_properties_alloc", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern AVCPBProperties* av_cpb_properties_alloc(ulong* @size);
        
    }
}
