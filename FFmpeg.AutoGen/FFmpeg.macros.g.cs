using System;
using System.Runtime.InteropServices;

namespace FFmpeg.AutoGen
{
    public unsafe static partial class ffmpeg
    {
        /// <summary>0x602</summary>
        public const int _WIN32_WINNT = 0x602;
        // public static attribute_deprecated = __declspec(deprecated);
        // public static av_always_inline = __forceinline;
        /// <summary>(1 &lt;&lt; 0)</summary>
        public const int AV_BUFFER_FLAG_READONLY = 0x1 << 0x0;
        /// <summary>0x2</summary>
        public const int AV_BUFFERSINK_FLAG_NO_REQUEST = 0x2;
        /// <summary>0x1</summary>
        public const int AV_BUFFERSINK_FLAG_PEEK = 0x1;
        // public static av_ceil_log2 = av_ceil_log2_c;
        /// <summary>0x00000100</summary>
        public const int AV_CH_BACK_CENTER = 0x100;
        /// <summary>0x00000010</summary>
        public const int AV_CH_BACK_LEFT = 0x10;
        /// <summary>0x00000020</summary>
        public const int AV_CH_BACK_RIGHT = 0x20;
        /// <summary>0x00000004</summary>
        public const int AV_CH_FRONT_CENTER = 0x4;
        /// <summary>0x00000001</summary>
        public const int AV_CH_FRONT_LEFT = 0x1;
        /// <summary>0x00000040</summary>
        public const int AV_CH_FRONT_LEFT_OF_CENTER = 0x40;
        /// <summary>0x00000002</summary>
        public const int AV_CH_FRONT_RIGHT = 0x2;
        /// <summary>0x00000080</summary>
        public const int AV_CH_FRONT_RIGHT_OF_CENTER = 0x80;
        /// <summary>(AV_CH_LAYOUT_STEREO|AV_CH_BACK_CENTER)</summary>
        public const int AV_CH_LAYOUT_2_1 = AV_CH_LAYOUT_STEREO | AV_CH_BACK_CENTER;
        /// <summary>(AV_CH_LAYOUT_STEREO|AV_CH_SIDE_LEFT|AV_CH_SIDE_RIGHT)</summary>
        public const int AV_CH_LAYOUT_2_2 = AV_CH_LAYOUT_STEREO | AV_CH_SIDE_LEFT | AV_CH_SIDE_RIGHT;
        /// <summary>(AV_CH_LAYOUT_STEREO|AV_CH_LOW_FREQUENCY)</summary>
        public const int AV_CH_LAYOUT_2POINT1 = AV_CH_LAYOUT_STEREO | AV_CH_LOW_FREQUENCY;
        /// <summary>(AV_CH_LAYOUT_SURROUND|AV_CH_LOW_FREQUENCY)</summary>
        public const int AV_CH_LAYOUT_3POINT1 = AV_CH_LAYOUT_SURROUND | AV_CH_LOW_FREQUENCY;
        /// <summary>(AV_CH_LAYOUT_SURROUND|AV_CH_BACK_CENTER)</summary>
        public const int AV_CH_LAYOUT_4POINT0 = AV_CH_LAYOUT_SURROUND | AV_CH_BACK_CENTER;
        /// <summary>(AV_CH_LAYOUT_4POINT0|AV_CH_LOW_FREQUENCY)</summary>
        public const int AV_CH_LAYOUT_4POINT1 = AV_CH_LAYOUT_4POINT0 | AV_CH_LOW_FREQUENCY;
        /// <summary>(AV_CH_LAYOUT_SURROUND|AV_CH_SIDE_LEFT|AV_CH_SIDE_RIGHT)</summary>
        public const int AV_CH_LAYOUT_5POINT0 = AV_CH_LAYOUT_SURROUND | AV_CH_SIDE_LEFT | AV_CH_SIDE_RIGHT;
        /// <summary>(AV_CH_LAYOUT_SURROUND|AV_CH_BACK_LEFT|AV_CH_BACK_RIGHT)</summary>
        public const int AV_CH_LAYOUT_5POINT0_BACK = AV_CH_LAYOUT_SURROUND | AV_CH_BACK_LEFT | AV_CH_BACK_RIGHT;
        /// <summary>(AV_CH_LAYOUT_5POINT0|AV_CH_LOW_FREQUENCY)</summary>
        public const int AV_CH_LAYOUT_5POINT1 = AV_CH_LAYOUT_5POINT0 | AV_CH_LOW_FREQUENCY;
        /// <summary>(AV_CH_LAYOUT_5POINT0_BACK|AV_CH_LOW_FREQUENCY)</summary>
        public const int AV_CH_LAYOUT_5POINT1_BACK = AV_CH_LAYOUT_5POINT0_BACK | AV_CH_LOW_FREQUENCY;
        /// <summary>(AV_CH_LAYOUT_5POINT0|AV_CH_BACK_CENTER)</summary>
        public const int AV_CH_LAYOUT_6POINT0 = AV_CH_LAYOUT_5POINT0 | AV_CH_BACK_CENTER;
        /// <summary>(AV_CH_LAYOUT_2_2|AV_CH_FRONT_LEFT_OF_CENTER|AV_CH_FRONT_RIGHT_OF_CENTER)</summary>
        public const int AV_CH_LAYOUT_6POINT0_FRONT = AV_CH_LAYOUT_2_2 | AV_CH_FRONT_LEFT_OF_CENTER | AV_CH_FRONT_RIGHT_OF_CENTER;
        /// <summary>(AV_CH_LAYOUT_5POINT1|AV_CH_BACK_CENTER)</summary>
        public const int AV_CH_LAYOUT_6POINT1 = AV_CH_LAYOUT_5POINT1 | AV_CH_BACK_CENTER;
        /// <summary>(AV_CH_LAYOUT_5POINT1_BACK|AV_CH_BACK_CENTER)</summary>
        public const int AV_CH_LAYOUT_6POINT1_BACK = AV_CH_LAYOUT_5POINT1_BACK | AV_CH_BACK_CENTER;
        /// <summary>(AV_CH_LAYOUT_6POINT0_FRONT|AV_CH_LOW_FREQUENCY)</summary>
        public const int AV_CH_LAYOUT_6POINT1_FRONT = AV_CH_LAYOUT_6POINT0_FRONT | AV_CH_LOW_FREQUENCY;
        /// <summary>(AV_CH_LAYOUT_5POINT0|AV_CH_BACK_LEFT|AV_CH_BACK_RIGHT)</summary>
        public const int AV_CH_LAYOUT_7POINT0 = AV_CH_LAYOUT_5POINT0 | AV_CH_BACK_LEFT | AV_CH_BACK_RIGHT;
        /// <summary>(AV_CH_LAYOUT_5POINT0|AV_CH_FRONT_LEFT_OF_CENTER|AV_CH_FRONT_RIGHT_OF_CENTER)</summary>
        public const int AV_CH_LAYOUT_7POINT0_FRONT = AV_CH_LAYOUT_5POINT0 | AV_CH_FRONT_LEFT_OF_CENTER | AV_CH_FRONT_RIGHT_OF_CENTER;
        /// <summary>(AV_CH_LAYOUT_5POINT1|AV_CH_BACK_LEFT|AV_CH_BACK_RIGHT)</summary>
        public const int AV_CH_LAYOUT_7POINT1 = AV_CH_LAYOUT_5POINT1 | AV_CH_BACK_LEFT | AV_CH_BACK_RIGHT;
        /// <summary>(AV_CH_LAYOUT_5POINT1|AV_CH_FRONT_LEFT_OF_CENTER|AV_CH_FRONT_RIGHT_OF_CENTER)</summary>
        public const int AV_CH_LAYOUT_7POINT1_WIDE = AV_CH_LAYOUT_5POINT1 | AV_CH_FRONT_LEFT_OF_CENTER | AV_CH_FRONT_RIGHT_OF_CENTER;
        /// <summary>(AV_CH_LAYOUT_5POINT1_BACK|AV_CH_FRONT_LEFT_OF_CENTER|AV_CH_FRONT_RIGHT_OF_CENTER)</summary>
        public const int AV_CH_LAYOUT_7POINT1_WIDE_BACK = AV_CH_LAYOUT_5POINT1_BACK | AV_CH_FRONT_LEFT_OF_CENTER | AV_CH_FRONT_RIGHT_OF_CENTER;
        /// <summary>(AV_CH_LAYOUT_OCTAGONAL|AV_CH_WIDE_LEFT|AV_CH_WIDE_RIGHT|AV_CH_TOP_BACK_LEFT|AV_CH_TOP_BACK_RIGHT|AV_CH_TOP_BACK_CENTER|AV_CH_TOP_FRONT_CENTER|AV_CH_TOP_FRONT_LEFT|AV_CH_TOP_FRONT_RIGHT)</summary>
        public const ulong AV_CH_LAYOUT_HEXADECAGONAL = (ulong)(AV_CH_LAYOUT_OCTAGONAL) | AV_CH_WIDE_LEFT | (ulong)(AV_CH_WIDE_RIGHT) | (ulong)(AV_CH_TOP_BACK_LEFT) | (ulong)(AV_CH_TOP_BACK_RIGHT) | (ulong)(AV_CH_TOP_BACK_CENTER) | (ulong)(AV_CH_TOP_FRONT_CENTER) | (ulong)(AV_CH_TOP_FRONT_LEFT) | (ulong)(AV_CH_TOP_FRONT_RIGHT);
        /// <summary>(AV_CH_LAYOUT_5POINT0_BACK|AV_CH_BACK_CENTER)</summary>
        public const int AV_CH_LAYOUT_HEXAGONAL = AV_CH_LAYOUT_5POINT0_BACK | AV_CH_BACK_CENTER;
        /// <summary>(AV_CH_FRONT_CENTER)</summary>
        public const int AV_CH_LAYOUT_MONO = AV_CH_FRONT_CENTER;
        /// <summary>0x8000000000000000ULL</summary>
        public const ulong AV_CH_LAYOUT_NATIVE = 0x8000000000000000ul;
        /// <summary>(AV_CH_LAYOUT_5POINT0|AV_CH_BACK_LEFT|AV_CH_BACK_CENTER|AV_CH_BACK_RIGHT)</summary>
        public const int AV_CH_LAYOUT_OCTAGONAL = AV_CH_LAYOUT_5POINT0 | AV_CH_BACK_LEFT | AV_CH_BACK_CENTER | AV_CH_BACK_RIGHT;
        /// <summary>(AV_CH_LAYOUT_STEREO|AV_CH_BACK_LEFT|AV_CH_BACK_RIGHT)</summary>
        public const int AV_CH_LAYOUT_QUAD = AV_CH_LAYOUT_STEREO | AV_CH_BACK_LEFT | AV_CH_BACK_RIGHT;
        /// <summary>(AV_CH_FRONT_LEFT|AV_CH_FRONT_RIGHT)</summary>
        public const int AV_CH_LAYOUT_STEREO = AV_CH_FRONT_LEFT | AV_CH_FRONT_RIGHT;
        /// <summary>(AV_CH_STEREO_LEFT|AV_CH_STEREO_RIGHT)</summary>
        public const int AV_CH_LAYOUT_STEREO_DOWNMIX = AV_CH_STEREO_LEFT | AV_CH_STEREO_RIGHT;
        /// <summary>(AV_CH_LAYOUT_STEREO|AV_CH_FRONT_CENTER)</summary>
        public const int AV_CH_LAYOUT_SURROUND = AV_CH_LAYOUT_STEREO | AV_CH_FRONT_CENTER;
        /// <summary>0x00000008</summary>
        public const int AV_CH_LOW_FREQUENCY = 0x8;
        /// <summary>0x0000000800000000ULL</summary>
        public const ulong AV_CH_LOW_FREQUENCY_2 = 0x800000000ul;
        /// <summary>0x00000200</summary>
        public const int AV_CH_SIDE_LEFT = 0x200;
        /// <summary>0x00000400</summary>
        public const int AV_CH_SIDE_RIGHT = 0x400;
        /// <summary>0x20000000</summary>
        public const int AV_CH_STEREO_LEFT = 0x20000000;
        /// <summary>0x40000000</summary>
        public const int AV_CH_STEREO_RIGHT = 0x40000000;
        /// <summary>0x0000000200000000ULL</summary>
        public const ulong AV_CH_SURROUND_DIRECT_LEFT = 0x200000000ul;
        /// <summary>0x0000000400000000ULL</summary>
        public const ulong AV_CH_SURROUND_DIRECT_RIGHT = 0x400000000ul;
        /// <summary>0x00010000</summary>
        public const int AV_CH_TOP_BACK_CENTER = 0x10000;
        /// <summary>0x00008000</summary>
        public const int AV_CH_TOP_BACK_LEFT = 0x8000;
        /// <summary>0x00020000</summary>
        public const int AV_CH_TOP_BACK_RIGHT = 0x20000;
        /// <summary>0x00000800</summary>
        public const int AV_CH_TOP_CENTER = 0x800;
        /// <summary>0x00002000</summary>
        public const int AV_CH_TOP_FRONT_CENTER = 0x2000;
        /// <summary>0x00001000</summary>
        public const int AV_CH_TOP_FRONT_LEFT = 0x1000;
        /// <summary>0x00004000</summary>
        public const int AV_CH_TOP_FRONT_RIGHT = 0x4000;
        /// <summary>0x0000000080000000ULL</summary>
        public const ulong AV_CH_WIDE_LEFT = 0x80000000ul;
        /// <summary>0x0000000100000000ULL</summary>
        public const ulong AV_CH_WIDE_RIGHT = 0x100000000ul;
        // public static av_clip = av_clip_c;
        // public static av_clip_int16 = av_clip_int16_c;
        // public static av_clip_int8 = av_clip_int8_c;
        // public static av_clip_intp2 = av_clip_intp2_c;
        // public static av_clip_uint16 = av_clip_uint16_c;
        // public static av_clip_uint8 = av_clip_uint8_c;
        // public static av_clip_uintp2 = av_clip_uintp2_c;
        // public static av_clip64 = av_clip64_c;
        // public static av_clipd = av_clipd_c;
        // public static av_clipf = av_clipf_c;
        // public static av_clipl_int32 = av_clipl_int32_c;
        /// <summary>(1 &lt;&lt; 15)</summary>
        public const int AV_CODEC_CAP_AUTO_THREADS = 0x1 << 0xf;
        /// <summary>(1 &lt;&lt; 17)</summary>
        public const int AV_CODEC_CAP_AVOID_PROBING = 0x1 << 0x11;
        /// <summary>(1 &lt;&lt; 10)</summary>
        public const int AV_CODEC_CAP_CHANNEL_CONF = 0x1 << 0xa;
        /// <summary>(1 &lt;&lt;  5)</summary>
        public const int AV_CODEC_CAP_DELAY = 0x1 << 0x5;
        /// <summary>(1 &lt;&lt;  1)</summary>
        public const int AV_CODEC_CAP_DR1 = 0x1 << 0x1;
        /// <summary>(1 &lt;&lt;  0)</summary>
        public const int AV_CODEC_CAP_DRAW_HORIZ_BAND = 0x1 << 0x0;
        /// <summary>(1 &lt;&lt;  9)</summary>
        public const int AV_CODEC_CAP_EXPERIMENTAL = 0x1 << 0x9;
        /// <summary>(1 &lt;&lt; 12)</summary>
        public const int AV_CODEC_CAP_FRAME_THREADS = 0x1 << 0xc;
        /// <summary>(1 &lt;&lt;  7)</summary>
        public const int AV_CODEC_CAP_HWACCEL_VDPAU = 0x1 << 0x7;
        /// <summary>0x40000000</summary>
        public const int AV_CODEC_CAP_INTRA_ONLY = 0x40000000;
        /// <summary>0x80000000</summary>
        public const uint AV_CODEC_CAP_LOSSLESS = 0x80000000u;
        /// <summary>(1 &lt;&lt; 14)</summary>
        public const int AV_CODEC_CAP_PARAM_CHANGE = 0x1 << 0xe;
        /// <summary>(1 &lt;&lt; 13)</summary>
        public const int AV_CODEC_CAP_SLICE_THREADS = 0x1 << 0xd;
        /// <summary>(1 &lt;&lt;  6)</summary>
        public const int AV_CODEC_CAP_SMALL_LAST_FRAME = 0x1 << 0x6;
        /// <summary>(1 &lt;&lt;  8)</summary>
        public const int AV_CODEC_CAP_SUBFRAMES = 0x1 << 0x8;
        /// <summary>(1 &lt;&lt;  3)</summary>
        public const int AV_CODEC_CAP_TRUNCATED = 0x1 << 0x3;
        /// <summary>(1 &lt;&lt; 16)</summary>
        public const int AV_CODEC_CAP_VARIABLE_FRAME_SIZE = 0x1 << 0x10;
        /// <summary>(1 &lt;&lt;  2)</summary>
        public const int AV_CODEC_FLAG_4MV = 0x1 << 0x2;
        /// <summary>(1 &lt;&lt; 24)</summary>
        public const int AV_CODEC_FLAG_AC_PRED = 0x1 << 0x18;
        /// <summary>(1 &lt;&lt; 23)</summary>
        public const int AV_CODEC_FLAG_BITEXACT = 0x1 << 0x17;
        /// <summary>(1U &lt;&lt; 31)</summary>
        public const uint AV_CODEC_FLAG_CLOSED_GOP = 0x1u << 0x1f;
        /// <summary>(1 &lt;&lt; 22)</summary>
        public const int AV_CODEC_FLAG_GLOBAL_HEADER = 0x1 << 0x16;
        /// <summary>(1 &lt;&lt; 13)</summary>
        public const int AV_CODEC_FLAG_GRAY = 0x1 << 0xd;
        /// <summary>(1 &lt;&lt; 18)</summary>
        public const int AV_CODEC_FLAG_INTERLACED_DCT = 0x1 << 0x12;
        /// <summary>(1 &lt;&lt; 29)</summary>
        public const int AV_CODEC_FLAG_INTERLACED_ME = 0x1 << 0x1d;
        /// <summary>(1 &lt;&lt; 11)</summary>
        public const int AV_CODEC_FLAG_LOOP_FILTER = 0x1 << 0xb;
        /// <summary>(1 &lt;&lt; 19)</summary>
        public const int AV_CODEC_FLAG_LOW_DELAY = 0x1 << 0x13;
        /// <summary>(1 &lt;&lt;  3)</summary>
        public const int AV_CODEC_FLAG_OUTPUT_CORRUPT = 0x1 << 0x3;
        /// <summary>(1 &lt;&lt;  9)</summary>
        public const int AV_CODEC_FLAG_PASS1 = 0x1 << 0x9;
        /// <summary>(1 &lt;&lt; 10)</summary>
        public const int AV_CODEC_FLAG_PASS2 = 0x1 << 0xa;
        /// <summary>(1 &lt;&lt; 15)</summary>
        public const int AV_CODEC_FLAG_PSNR = 0x1 << 0xf;
        /// <summary>(1 &lt;&lt;  4)</summary>
        public const int AV_CODEC_FLAG_QPEL = 0x1 << 0x4;
        /// <summary>(1 &lt;&lt;  1)</summary>
        public const int AV_CODEC_FLAG_QSCALE = 0x1 << 0x1;
        /// <summary>(1 &lt;&lt; 16)</summary>
        public const int AV_CODEC_FLAG_TRUNCATED = 0x1 << 0x10;
        /// <summary>(1 &lt;&lt;  0)</summary>
        public const int AV_CODEC_FLAG_UNALIGNED = 0x1 << 0x0;
        /// <summary>(1 &lt;&lt; 15)</summary>
        public const int AV_CODEC_FLAG2_CHUNKS = 0x1 << 0xf;
        /// <summary>(1 &lt;&lt; 13)</summary>
        public const int AV_CODEC_FLAG2_DROP_FRAME_TIMECODE = 0x1 << 0xd;
        /// <summary>(1 &lt;&lt; 28)</summary>
        public const int AV_CODEC_FLAG2_EXPORT_MVS = 0x1 << 0x1c;
        /// <summary>(1 &lt;&lt;  0)</summary>
        public const int AV_CODEC_FLAG2_FAST = 0x1 << 0x0;
        /// <summary>(1 &lt;&lt; 16)</summary>
        public const int AV_CODEC_FLAG2_IGNORE_CROP = 0x1 << 0x10;
        /// <summary>(1 &lt;&lt;  3)</summary>
        public const int AV_CODEC_FLAG2_LOCAL_HEADER = 0x1 << 0x3;
        /// <summary>(1 &lt;&lt;  2)</summary>
        public const int AV_CODEC_FLAG2_NO_OUTPUT = 0x1 << 0x2;
        /// <summary>(1 &lt;&lt; 30)</summary>
        public const int AV_CODEC_FLAG2_RO_FLUSH_NOOP = 0x1 << 0x1e;
        /// <summary>(1 &lt;&lt; 22)</summary>
        public const int AV_CODEC_FLAG2_SHOW_ALL = 0x1 << 0x16;
        /// <summary>(1 &lt;&lt; 29)</summary>
        public const int AV_CODEC_FLAG2_SKIP_MANUAL = 0x1 << 0x1d;
        // public static AV_CODEC_ID_H265 = AV_CODEC_ID_HEVC;
        // public static AV_CODEC_ID_IFF_BYTERUN1 = AV_CODEC_ID_IFF_ILBM;
        /// <summary>(1 &lt;&lt; 16)</summary>
        public const int AV_CODEC_PROP_BITMAP_SUB = 0x1 << 0x10;
        /// <summary>(1 &lt;&lt; 0)</summary>
        public const int AV_CODEC_PROP_INTRA_ONLY = 0x1 << 0x0;
        /// <summary>(1 &lt;&lt; 2)</summary>
        public const int AV_CODEC_PROP_LOSSLESS = 0x1 << 0x2;
        /// <summary>(1 &lt;&lt; 1)</summary>
        public const int AV_CODEC_PROP_LOSSY = 0x1 << 0x1;
        /// <summary>(1 &lt;&lt; 3)</summary>
        public const int AV_CODEC_PROP_REORDER = 0x1 << 0x3;
        /// <summary>(1 &lt;&lt; 17)</summary>
        public const int AV_CODEC_PROP_TEXT_SUB = 0x1 << 0x11;
        /// <summary>0x0004</summary>
        public const int AV_CPU_FLAG_3DNOW = 0x4;
        /// <summary>0x0020</summary>
        public const int AV_CPU_FLAG_3DNOWEXT = 0x20;
        /// <summary>0x80000</summary>
        public const int AV_CPU_FLAG_AESNI = 0x80000;
        /// <summary>0x0001</summary>
        public const int AV_CPU_FLAG_ALTIVEC = 0x1;
        /// <summary>(1 &lt;&lt; 0)</summary>
        public const int AV_CPU_FLAG_ARMV5TE = 0x1 << 0x0;
        /// <summary>(1 &lt;&lt; 1)</summary>
        public const int AV_CPU_FLAG_ARMV6 = 0x1 << 0x1;
        /// <summary>(1 &lt;&lt; 2)</summary>
        public const int AV_CPU_FLAG_ARMV6T2 = 0x1 << 0x2;
        /// <summary>(1 &lt;&lt; 6)</summary>
        public const int AV_CPU_FLAG_ARMV8 = 0x1 << 0x6;
        /// <summary>0x10000000</summary>
        public const int AV_CPU_FLAG_ATOM = 0x10000000;
        /// <summary>0x4000</summary>
        public const int AV_CPU_FLAG_AVX = 0x4000;
        /// <summary>0x8000</summary>
        public const int AV_CPU_FLAG_AVX2 = 0x8000;
        /// <summary>0x8000000</summary>
        public const int AV_CPU_FLAG_AVXSLOW = 0x8000000;
        /// <summary>0x20000</summary>
        public const int AV_CPU_FLAG_BMI1 = 0x20000;
        /// <summary>0x40000</summary>
        public const int AV_CPU_FLAG_BMI2 = 0x40000;
        /// <summary>0x1000</summary>
        public const int AV_CPU_FLAG_CMOV = 0x1000;
        /// <summary>0x10000</summary>
        public const int AV_CPU_FLAG_FMA3 = 0x10000;
        /// <summary>0x0800</summary>
        public const int AV_CPU_FLAG_FMA4 = 0x800;
        /// <summary>0x80000000</summary>
        public const uint AV_CPU_FLAG_FORCE = 0x80000000u;
        /// <summary>0x0001</summary>
        public const int AV_CPU_FLAG_MMX = 0x1;
        /// <summary>0x0002</summary>
        public const int AV_CPU_FLAG_MMX2 = 0x2;
        /// <summary>0x0002</summary>
        public const int AV_CPU_FLAG_MMXEXT = 0x2;
        /// <summary>(1 &lt;&lt; 5)</summary>
        public const int AV_CPU_FLAG_NEON = 0x1 << 0x5;
        /// <summary>0x0004</summary>
        public const int AV_CPU_FLAG_POWER8 = 0x4;
        /// <summary>(1 &lt;&lt;16)</summary>
        public const int AV_CPU_FLAG_SETEND = 0x1 << 0x10;
        /// <summary>0x0008</summary>
        public const int AV_CPU_FLAG_SSE = 0x8;
        /// <summary>0x0010</summary>
        public const int AV_CPU_FLAG_SSE2 = 0x10;
        /// <summary>0x40000000</summary>
        public const int AV_CPU_FLAG_SSE2SLOW = 0x40000000;
        /// <summary>0x0040</summary>
        public const int AV_CPU_FLAG_SSE3 = 0x40;
        /// <summary>0x20000000</summary>
        public const int AV_CPU_FLAG_SSE3SLOW = 0x20000000;
        /// <summary>0x0100</summary>
        public const int AV_CPU_FLAG_SSE4 = 0x100;
        /// <summary>0x0200</summary>
        public const int AV_CPU_FLAG_SSE42 = 0x200;
        /// <summary>0x0080</summary>
        public const int AV_CPU_FLAG_SSSE3 = 0x80;
        /// <summary>0x4000000</summary>
        public const int AV_CPU_FLAG_SSSE3SLOW = 0x4000000;
        /// <summary>(1 &lt;&lt; 3)</summary>
        public const int AV_CPU_FLAG_VFP = 0x1 << 0x3;
        /// <summary>(1 &lt;&lt; 7)</summary>
        public const int AV_CPU_FLAG_VFP_VM = 0x1 << 0x7;
        /// <summary>(1 &lt;&lt; 4)</summary>
        public const int AV_CPU_FLAG_VFPV3 = 0x1 << 0x4;
        /// <summary>0x0002</summary>
        public const int AV_CPU_FLAG_VSX = 0x2;
        /// <summary>0x0400</summary>
        public const int AV_CPU_FLAG_XOP = 0x400;
        /// <summary>32</summary>
        public const int AV_DICT_APPEND = 0x20;
        /// <summary>16</summary>
        public const int AV_DICT_DONT_OVERWRITE = 0x10;
        /// <summary>4</summary>
        public const int AV_DICT_DONT_STRDUP_KEY = 0x4;
        /// <summary>8</summary>
        public const int AV_DICT_DONT_STRDUP_VAL = 0x8;
        /// <summary>2</summary>
        public const int AV_DICT_IGNORE_SUFFIX = 0x2;
        /// <summary>1</summary>
        public const int AV_DICT_MATCH_CASE = 0x1;
        /// <summary>64</summary>
        public const int AV_DICT_MULTIKEY = 0x40;
        /// <summary>0x0400</summary>
        public const int AV_DISPOSITION_ATTACHED_PIC = 0x400;
        /// <summary>0x10000</summary>
        public const int AV_DISPOSITION_CAPTIONS = 0x10000;
        /// <summary>0x0200</summary>
        public const int AV_DISPOSITION_CLEAN_EFFECTS = 0x200;
        /// <summary>0x0008</summary>
        public const int AV_DISPOSITION_COMMENT = 0x8;
        /// <summary>0x0001</summary>
        public const int AV_DISPOSITION_DEFAULT = 0x1;
        /// <summary>0x20000</summary>
        public const int AV_DISPOSITION_DESCRIPTIONS = 0x20000;
        /// <summary>0x0002</summary>
        public const int AV_DISPOSITION_DUB = 0x2;
        /// <summary>0x0040</summary>
        public const int AV_DISPOSITION_FORCED = 0x40;
        /// <summary>0x0080</summary>
        public const int AV_DISPOSITION_HEARING_IMPAIRED = 0x80;
        /// <summary>0x0020</summary>
        public const int AV_DISPOSITION_KARAOKE = 0x20;
        /// <summary>0x0010</summary>
        public const int AV_DISPOSITION_LYRICS = 0x10;
        /// <summary>0x40000</summary>
        public const int AV_DISPOSITION_METADATA = 0x40000;
        /// <summary>0x0004</summary>
        public const int AV_DISPOSITION_ORIGINAL = 0x4;
        /// <summary>0x0800</summary>
        public const int AV_DISPOSITION_TIMED_THUMBNAILS = 0x800;
        /// <summary>0x0100</summary>
        public const int AV_DISPOSITION_VISUAL_IMPAIRED = 0x100;
        /// <summary>(1&lt;&lt;18)</summary>
        public const int AV_EF_AGGRESSIVE = 0x1 << 0x12;
        /// <summary>(1&lt;&lt;1)</summary>
        public const int AV_EF_BITSTREAM = 0x1 << 0x1;
        /// <summary>(1&lt;&lt;2)</summary>
        public const int AV_EF_BUFFER = 0x1 << 0x2;
        /// <summary>(1&lt;&lt;16)</summary>
        public const int AV_EF_CAREFUL = 0x1 << 0x10;
        /// <summary>(1&lt;&lt;17)</summary>
        public const int AV_EF_COMPLIANT = 0x1 << 0x11;
        /// <summary>(1&lt;&lt;0)</summary>
        public const int AV_EF_CRCCHECK = 0x1 << 0x0;
        /// <summary>(1&lt;&lt;3)</summary>
        public const int AV_EF_EXPLODE = 0x1 << 0x3;
        /// <summary>(1&lt;&lt;15)</summary>
        public const int AV_EF_IGNORE_ERR = 0x1 << 0xf;
        /// <summary>64</summary>
        public const int AV_ERROR_MAX_STRING_SIZE = 0x40;
        // public static av_extern_inline = inline;
        /// <summary>32</summary>
        public const int AV_FOURCC_MAX_STRING_SIZE = 0x20;
        /// <summary>1</summary>
        public const int AV_FRAME_FILENAME_FLAGS_MULTIPLE = 0x1;
        /// <summary>(1 &lt;&lt; 0)</summary>
        public const int AV_FRAME_FLAG_CORRUPT = 0x1 << 0x0;
        /// <summary>(1 &lt;&lt; 2)</summary>
        public const int AV_FRAME_FLAG_DISCARD = 0x1 << 0x2;
        /// <summary>(1 &lt;&lt; 0)</summary>
        public const int AV_GET_BUFFER_FLAG_REF = 0x1 << 0x0;
        /// <summary>0</summary>
        public const int AV_HAVE_BIGENDIAN = 0x0;
        /// <summary>1</summary>
        public const int AV_HAVE_FAST_UNALIGNED = 0x1;
        /// <summary>(1 &lt;&lt; 1)</summary>
        public const int AV_HWACCEL_FLAG_ALLOW_HIGH_DEPTH = 0x1 << 0x1;
        /// <summary>(1 &lt;&lt; 0)</summary>
        public const int AV_HWACCEL_FLAG_IGNORE_LEVEL = 0x1 << 0x0;
        /// <summary>16384</summary>
        public const int AV_INPUT_BUFFER_MIN_SIZE = 0x4000;
        /// <summary>32</summary>
        public const int AV_INPUT_BUFFER_PADDING_SIZE = 0x20;
        /// <summary>48</summary>
        public const int AV_LOG_DEBUG = 0x30;
        /// <summary>16</summary>
        public const int AV_LOG_ERROR = 0x10;
        /// <summary>8</summary>
        public const int AV_LOG_FATAL = 0x8;
        /// <summary>32</summary>
        public const int AV_LOG_INFO = 0x20;
        /// <summary>(AV_LOG_TRACE - AV_LOG_QUIET)</summary>
        public const int AV_LOG_MAX_OFFSET = AV_LOG_TRACE - AV_LOG_QUIET;
        /// <summary>0</summary>
        public const int AV_LOG_PANIC = 0x0;
        /// <summary>2</summary>
        public const int AV_LOG_PRINT_LEVEL = 0x2;
        /// <summary>-8</summary>
        public const int AV_LOG_QUIET = -0x8;
        /// <summary>1</summary>
        public const int AV_LOG_SKIP_REPEATED = 0x1;
        /// <summary>56</summary>
        public const int AV_LOG_TRACE = 0x38;
        /// <summary>40</summary>
        public const int AV_LOG_VERBOSE = 0x28;
        /// <summary>24</summary>
        public const int AV_LOG_WARNING = 0x18;
        // public static av_mod_uintp2 = av_mod_uintp2_c;
        // public static av_noinline = __declspec(noinline);
        /// <summary>((int64_t)UINT64_C(0x8000000000000000))</summary>
        public static long AV_NOPTS_VALUE = (long)(UINT64_C(0x8000000000000000ul));
        /// <summary>8</summary>
        public const int AV_NUM_DATA_POINTERS = 0x8;
        /// <summary>(1 &lt;&lt; 2)</summary>
        public const int AV_OPT_ALLOW_NULL = 0x1 << 0x2;
        /// <summary>8</summary>
        public const int AV_OPT_FLAG_AUDIO_PARAM = 0x8;
        /// <summary>2</summary>
        public const int AV_OPT_FLAG_DECODING_PARAM = 0x2;
        /// <summary>1</summary>
        public const int AV_OPT_FLAG_ENCODING_PARAM = 0x1;
        /// <summary>64</summary>
        public const int AV_OPT_FLAG_EXPORT = 0x40;
        /// <summary>(1&lt;&lt;16)</summary>
        public const int AV_OPT_FLAG_FILTERING_PARAM = 0x1 << 0x10;
        /// <summary>4</summary>
        public const int AV_OPT_FLAG_METADATA = 0x4;
        /// <summary>128</summary>
        public const int AV_OPT_FLAG_READONLY = 0x80;
        /// <summary>32</summary>
        public const int AV_OPT_FLAG_SUBTITLE_PARAM = 0x20;
        /// <summary>16</summary>
        public const int AV_OPT_FLAG_VIDEO_PARAM = 0x10;
        /// <summary>(1 &lt;&lt; 12)</summary>
        public const int AV_OPT_MULTI_COMPONENT_RANGE = 0x1 << 0xc;
        /// <summary>(1 &lt;&lt; 0)</summary>
        public const int AV_OPT_SEARCH_CHILDREN = 0x1 << 0x0;
        /// <summary>(1 &lt;&lt; 1)</summary>
        public const int AV_OPT_SEARCH_FAKE_OBJ = 0x1 << 0x1;
        /// <summary>0x00000002</summary>
        public const int AV_OPT_SERIALIZE_OPT_FLAGS_EXACT = 0x2;
        /// <summary>0x00000001</summary>
        public const int AV_OPT_SERIALIZE_SKIP_DEFAULTS = 0x1;
        // public static av_parity = av_parity_c;
        /// <summary>4</summary>
        public const int AV_PARSER_PTS_NB = 0x4;
        // public static AV_PIX_FMT_0BGR32 = AV_PIX_FMT_NE(0BGR, RGB0);
        // public static AV_PIX_FMT_0RGB32 = AV_PIX_FMT_NE(0RGB, BGR0);
        // public static AV_PIX_FMT_AYUV64 = AV_PIX_FMT_NE(AYUV64BE, AYUV64LE);
        // public static AV_PIX_FMT_BAYER_BGGR16 = AV_PIX_FMT_NE(BAYER_BGGR16BE,    BAYER_BGGR16LE);
        // public static AV_PIX_FMT_BAYER_GBRG16 = AV_PIX_FMT_NE(BAYER_GBRG16BE,    BAYER_GBRG16LE);
        // public static AV_PIX_FMT_BAYER_GRBG16 = AV_PIX_FMT_NE(BAYER_GRBG16BE,    BAYER_GRBG16LE);
        // public static AV_PIX_FMT_BAYER_RGGB16 = AV_PIX_FMT_NE(BAYER_RGGB16BE,    BAYER_RGGB16LE);
        // public static AV_PIX_FMT_BGR32 = AV_PIX_FMT_NE(ABGR, RGBA);
        // public static AV_PIX_FMT_BGR32_1 = AV_PIX_FMT_NE(BGRA, ARGB);
        // public static AV_PIX_FMT_BGR444 = AV_PIX_FMT_NE(BGR444BE, BGR444LE);
        // public static AV_PIX_FMT_BGR48 = AV_PIX_FMT_NE(BGR48BE,  BGR48LE);
        // public static AV_PIX_FMT_BGR555 = AV_PIX_FMT_NE(BGR555BE, BGR555LE);
        // public static AV_PIX_FMT_BGR565 = AV_PIX_FMT_NE(BGR565BE, BGR565LE);
        // public static AV_PIX_FMT_BGRA64 = AV_PIX_FMT_NE(BGRA64BE, BGRA64LE);
        /// <summary>0x1 &lt;&lt; 0x7</summary>
        public const int AV_PIX_FMT_FLAG_ALPHA = 0x1 << 0x7;
        /// <summary>0x1 &lt;&lt; 0x8</summary>
        public const int AV_PIX_FMT_FLAG_BAYER = 0x1 << 0x8;
        /// <summary>0x1 &lt;&lt; 0x0</summary>
        public const int AV_PIX_FMT_FLAG_BE = 0x1 << 0x0;
        /// <summary>0x1 &lt;&lt; 0x2</summary>
        public const int AV_PIX_FMT_FLAG_BITSTREAM = 0x1 << 0x2;
        /// <summary>0x1 &lt;&lt; 0x3</summary>
        public const int AV_PIX_FMT_FLAG_HWACCEL = 0x1 << 0x3;
        /// <summary>0x1 &lt;&lt; 0x1</summary>
        public const int AV_PIX_FMT_FLAG_PAL = 0x1 << 0x1;
        /// <summary>0x1 &lt;&lt; 0x4</summary>
        public const int AV_PIX_FMT_FLAG_PLANAR = 0x1 << 0x4;
        /// <summary>0x1 &lt;&lt; 0x6</summary>
        public const int AV_PIX_FMT_FLAG_PSEUDOPAL = 0x1 << 0x6;
        /// <summary>0x1 &lt;&lt; 0x5</summary>
        public const int AV_PIX_FMT_FLAG_RGB = 0x1 << 0x5;
        // public static AV_PIX_FMT_GBRAP10 = AV_PIX_FMT_NE(GBRAP10BE,   GBRAP10LE);
        // public static AV_PIX_FMT_GBRAP12 = AV_PIX_FMT_NE(GBRAP12BE,   GBRAP12LE);
        // public static AV_PIX_FMT_GBRAP16 = AV_PIX_FMT_NE(GBRAP16BE,   GBRAP16LE);
        // public static AV_PIX_FMT_GBRP10 = AV_PIX_FMT_NE(GBRP10BE,    GBRP10LE);
        // public static AV_PIX_FMT_GBRP12 = AV_PIX_FMT_NE(GBRP12BE,    GBRP12LE);
        // public static AV_PIX_FMT_GBRP14 = AV_PIX_FMT_NE(GBRP14BE,    GBRP14LE);
        // public static AV_PIX_FMT_GBRP16 = AV_PIX_FMT_NE(GBRP16BE,    GBRP16LE);
        // public static AV_PIX_FMT_GBRP9 = AV_PIX_FMT_NE(GBRP9BE ,    GBRP9LE);
        // public static AV_PIX_FMT_GRAY10 = AV_PIX_FMT_NE(GRAY10BE, GRAY10LE);
        // public static AV_PIX_FMT_GRAY12 = AV_PIX_FMT_NE(GRAY12BE, GRAY12LE);
        // public static AV_PIX_FMT_GRAY16 = AV_PIX_FMT_NE(GRAY16BE, GRAY16LE);
        // public static AV_PIX_FMT_NV20 = AV_PIX_FMT_NE(NV20BE,  NV20LE);
        // public static AV_PIX_FMT_P010 = AV_PIX_FMT_NE(P010BE,  P010LE);
        // public static AV_PIX_FMT_P016 = AV_PIX_FMT_NE(P016BE,  P016LE);
        // public static AV_PIX_FMT_RGB32 = AV_PIX_FMT_NE(ARGB, BGRA);
        // public static AV_PIX_FMT_RGB32_1 = AV_PIX_FMT_NE(RGBA, ABGR);
        // public static AV_PIX_FMT_RGB444 = AV_PIX_FMT_NE(RGB444BE, RGB444LE);
        // public static AV_PIX_FMT_RGB48 = AV_PIX_FMT_NE(RGB48BE,  RGB48LE);
        // public static AV_PIX_FMT_RGB555 = AV_PIX_FMT_NE(RGB555BE, RGB555LE);
        // public static AV_PIX_FMT_RGB565 = AV_PIX_FMT_NE(RGB565BE, RGB565LE);
        // public static AV_PIX_FMT_RGBA64 = AV_PIX_FMT_NE(RGBA64BE, RGBA64LE);
        // public static AV_PIX_FMT_XYZ12 = AV_PIX_FMT_NE(XYZ12BE, XYZ12LE);
        // public static AV_PIX_FMT_YA16 = AV_PIX_FMT_NE(YA16BE,   YA16LE);
        // public static AV_PIX_FMT_YUV420P10 = AV_PIX_FMT_NE(YUV420P10BE, YUV420P10LE);
        // public static AV_PIX_FMT_YUV420P12 = AV_PIX_FMT_NE(YUV420P12BE, YUV420P12LE);
        // public static AV_PIX_FMT_YUV420P14 = AV_PIX_FMT_NE(YUV420P14BE, YUV420P14LE);
        // public static AV_PIX_FMT_YUV420P16 = AV_PIX_FMT_NE(YUV420P16BE, YUV420P16LE);
        // public static AV_PIX_FMT_YUV420P9 = AV_PIX_FMT_NE(YUV420P9BE , YUV420P9LE);
        // public static AV_PIX_FMT_YUV422P10 = AV_PIX_FMT_NE(YUV422P10BE, YUV422P10LE);
        // public static AV_PIX_FMT_YUV422P12 = AV_PIX_FMT_NE(YUV422P12BE, YUV422P12LE);
        // public static AV_PIX_FMT_YUV422P14 = AV_PIX_FMT_NE(YUV422P14BE, YUV422P14LE);
        // public static AV_PIX_FMT_YUV422P16 = AV_PIX_FMT_NE(YUV422P16BE, YUV422P16LE);
        // public static AV_PIX_FMT_YUV422P9 = AV_PIX_FMT_NE(YUV422P9BE , YUV422P9LE);
        // public static AV_PIX_FMT_YUV440P10 = AV_PIX_FMT_NE(YUV440P10BE, YUV440P10LE);
        // public static AV_PIX_FMT_YUV440P12 = AV_PIX_FMT_NE(YUV440P12BE, YUV440P12LE);
        // public static AV_PIX_FMT_YUV444P10 = AV_PIX_FMT_NE(YUV444P10BE, YUV444P10LE);
        // public static AV_PIX_FMT_YUV444P12 = AV_PIX_FMT_NE(YUV444P12BE, YUV444P12LE);
        // public static AV_PIX_FMT_YUV444P14 = AV_PIX_FMT_NE(YUV444P14BE, YUV444P14LE);
        // public static AV_PIX_FMT_YUV444P16 = AV_PIX_FMT_NE(YUV444P16BE, YUV444P16LE);
        // public static AV_PIX_FMT_YUV444P9 = AV_PIX_FMT_NE(YUV444P9BE , YUV444P9LE);
        // public static AV_PIX_FMT_YUVA420P10 = AV_PIX_FMT_NE(YUVA420P10BE, YUVA420P10LE);
        // public static AV_PIX_FMT_YUVA420P16 = AV_PIX_FMT_NE(YUVA420P16BE, YUVA420P16LE);
        // public static AV_PIX_FMT_YUVA420P9 = AV_PIX_FMT_NE(YUVA420P9BE , YUVA420P9LE);
        // public static AV_PIX_FMT_YUVA422P10 = AV_PIX_FMT_NE(YUVA422P10BE, YUVA422P10LE);
        // public static AV_PIX_FMT_YUVA422P16 = AV_PIX_FMT_NE(YUVA422P16BE, YUVA422P16LE);
        // public static AV_PIX_FMT_YUVA422P9 = AV_PIX_FMT_NE(YUVA422P9BE , YUVA422P9LE);
        // public static AV_PIX_FMT_YUVA444P10 = AV_PIX_FMT_NE(YUVA444P10BE, YUVA444P10LE);
        // public static AV_PIX_FMT_YUVA444P16 = AV_PIX_FMT_NE(YUVA444P16BE, YUVA444P16LE);
        // public static AV_PIX_FMT_YUVA444P9 = AV_PIX_FMT_NE(YUVA444P9BE , YUVA444P9LE);
        // public static AV_PKT_DATA_QUALITY_FACTOR = AV_PKT_DATA_QUALITY_STATS;
        /// <summary>0x0002</summary>
        public const int AV_PKT_FLAG_CORRUPT = 0x2;
        /// <summary>0x0004</summary>
        public const int AV_PKT_FLAG_DISCARD = 0x4;
        /// <summary>0x0001</summary>
        public const int AV_PKT_FLAG_KEY = 0x1;
        // public static av_popcount = av_popcount_c;
        // public static av_popcount64 = av_popcount64_c;
        /// <summary>1</summary>
        public const int AV_PROGRAM_RUNNING = 0x1;
        /// <summary>1</summary>
        public const int AV_PTS_WRAP_ADD_OFFSET = 0x1;
        /// <summary>0</summary>
        public const int AV_PTS_WRAP_IGNORE = 0x0;
        /// <summary>-1</summary>
        public const int AV_PTS_WRAP_SUB_OFFSET = -0x1;
        // public static av_sat_add32 = av_sat_add32_c;
        // public static av_sat_dadd32 = av_sat_dadd32_c;
        /// <summary>0x00000001</summary>
        public const int AV_SUBTITLE_FLAG_FORCED = 0x1;
        /// <summary>1000000</summary>
        public const int AV_TIME_BASE = 0xf4240;
        // public static AV_TIME_BASE_Q = (AVRational){1, AV_TIME_BASE};
        /// <summary>0x10</summary>
        public const int AV_TIMECODE_STR_SIZE = 0x10;
        // public static av_unused = __attribute__((unused));
        // public static av_used = __attribute__((used));
        // public static AVCOL_SPC_YCGCO = AVCOL_SPC_YCOCG;
        /// <summary>FFERRTAG(0xF8,&apos;B&apos;,&apos;S&apos;,&apos;F&apos;)</summary>
        public static int AVERROR_BSF_NOT_FOUND = FFERRTAG(0x8,'B','S','F');
        /// <summary>FFERRTAG( &apos;B&apos;,&apos;U&apos;,&apos;F&apos;,&apos;S&apos;)</summary>
        public static int AVERROR_BUFFER_TOO_SMALL = FFERRTAG('B','U','F','S');
        /// <summary>FFERRTAG( &apos;B&apos;,&apos;U&apos;,&apos;G&apos;,&apos;!&apos;)</summary>
        public static int AVERROR_BUG = FFERRTAG('B','U','G','!');
        /// <summary>FFERRTAG( &apos;B&apos;,&apos;U&apos;,&apos;G&apos;,&apos; &apos;)</summary>
        public static int AVERROR_BUG2 = FFERRTAG('B','U','G',' ');
        /// <summary>FFERRTAG(0xF8,&apos;D&apos;,&apos;E&apos;,&apos;C&apos;)</summary>
        public static int AVERROR_DECODER_NOT_FOUND = FFERRTAG(0x8,'D','E','C');
        /// <summary>FFERRTAG(0xF8,&apos;D&apos;,&apos;E&apos;,&apos;M&apos;)</summary>
        public static int AVERROR_DEMUXER_NOT_FOUND = FFERRTAG(0x8,'D','E','M');
        /// <summary>FFERRTAG(0xF8,&apos;E&apos;,&apos;N&apos;,&apos;C&apos;)</summary>
        public static int AVERROR_ENCODER_NOT_FOUND = FFERRTAG(0x8,'E','N','C');
        /// <summary>FFERRTAG( &apos;E&apos;,&apos;O&apos;,&apos;F&apos;,&apos; &apos;)</summary>
        public static int AVERROR_EOF = FFERRTAG('E','O','F',' ');
        /// <summary>FFERRTAG( &apos;E&apos;,&apos;X&apos;,&apos;I&apos;,&apos;T&apos;)</summary>
        public static int AVERROR_EXIT = FFERRTAG('E','X','I','T');
        /// <summary>(-0x2bb2afa8)</summary>
        public const int AVERROR_EXPERIMENTAL = -0x2bb2aa8;
        /// <summary>FFERRTAG( &apos;E&apos;,&apos;X&apos;,&apos;T&apos;,&apos; &apos;)</summary>
        public static int AVERROR_EXTERNAL = FFERRTAG('E','X','T',' ');
        /// <summary>FFERRTAG(0xF8,&apos;F&apos;,&apos;I&apos;,&apos;L&apos;)</summary>
        public static int AVERROR_FILTER_NOT_FOUND = FFERRTAG(0x8,'F','I','L');
        /// <summary>FFERRTAG(0xF8,&apos;4&apos;,&apos;0&apos;,&apos;0&apos;)</summary>
        public static int AVERROR_HTTP_BAD_REQUEST = FFERRTAG(0x8,'4','0','0');
        /// <summary>FFERRTAG(0xF8,&apos;4&apos;,&apos;0&apos;,&apos;3&apos;)</summary>
        public static int AVERROR_HTTP_FORBIDDEN = FFERRTAG(0x8,'4','0','3');
        /// <summary>FFERRTAG(0xF8,&apos;4&apos;,&apos;0&apos;,&apos;4&apos;)</summary>
        public static int AVERROR_HTTP_NOT_FOUND = FFERRTAG(0x8,'4','0','4');
        /// <summary>FFERRTAG(0xF8,&apos;4&apos;,&apos;X&apos;,&apos;X&apos;)</summary>
        public static int AVERROR_HTTP_OTHER_4XX = FFERRTAG(0x8,'4','X','X');
        /// <summary>FFERRTAG(0xF8,&apos;5&apos;,&apos;X&apos;,&apos;X&apos;)</summary>
        public static int AVERROR_HTTP_SERVER_ERROR = FFERRTAG(0x8,'5','X','X');
        /// <summary>FFERRTAG(0xF8,&apos;4&apos;,&apos;0&apos;,&apos;1&apos;)</summary>
        public static int AVERROR_HTTP_UNAUTHORIZED = FFERRTAG(0x8,'4','0','1');
        /// <summary>(-0x636e6701)</summary>
        public const int AVERROR_INPUT_CHANGED = -0x636e6701;
        /// <summary>FFERRTAG( &apos;I&apos;,&apos;N&apos;,&apos;D&apos;,&apos;A&apos;)</summary>
        public static int AVERROR_INVALIDDATA = FFERRTAG('I','N','D','A');
        /// <summary>FFERRTAG(0xF8,&apos;M&apos;,&apos;U&apos;,&apos;X&apos;)</summary>
        public static int AVERROR_MUXER_NOT_FOUND = FFERRTAG(0x8,'M','U','X');
        /// <summary>FFERRTAG(0xF8,&apos;O&apos;,&apos;P&apos;,&apos;T&apos;)</summary>
        public static int AVERROR_OPTION_NOT_FOUND = FFERRTAG(0x8,'O','P','T');
        /// <summary>(-0x636e6702)</summary>
        public const int AVERROR_OUTPUT_CHANGED = -0x636e6702;
        /// <summary>FFERRTAG( &apos;P&apos;,&apos;A&apos;,&apos;W&apos;,&apos;E&apos;)</summary>
        public static int AVERROR_PATCHWELCOME = FFERRTAG('P','A','W','E');
        /// <summary>FFERRTAG(0xF8,&apos;P&apos;,&apos;R&apos;,&apos;O&apos;)</summary>
        public static int AVERROR_PROTOCOL_NOT_FOUND = FFERRTAG(0x8,'P','R','O');
        /// <summary>FFERRTAG(0xF8,&apos;S&apos;,&apos;T&apos;,&apos;R&apos;)</summary>
        public static int AVERROR_STREAM_NOT_FOUND = FFERRTAG(0x8,'S','T','R');
        /// <summary>FFERRTAG( &apos;U&apos;,&apos;N&apos;,&apos;K&apos;,&apos;N&apos;)</summary>
        public static int AVERROR_UNKNOWN = FFERRTAG('U','N','K','N');
        /// <summary>0x2</summary>
        public const int AVFILTER_CMD_FLAG_FAST = 0x2;
        /// <summary>0x1</summary>
        public const int AVFILTER_CMD_FLAG_ONE = 0x1;
        /// <summary>0x1 &lt;&lt; 0x0</summary>
        public const int AVFILTER_FLAG_DYNAMIC_INPUTS = 0x1 << 0x0;
        /// <summary>0x1 &lt;&lt; 0x1</summary>
        public const int AVFILTER_FLAG_DYNAMIC_OUTPUTS = 0x1 << 0x1;
        /// <summary>0x1 &lt;&lt; 0x2</summary>
        public const int AVFILTER_FLAG_SLICE_THREADS = 0x1 << 0x2;
        /// <summary>AVFILTER_FLAG_SUPPORT_TIMELINE_GENERIC | AVFILTER_FLAG_SUPPORT_TIMELINE_INTERNAL</summary>
        public const int AVFILTER_FLAG_SUPPORT_TIMELINE = AVFILTER_FLAG_SUPPORT_TIMELINE_GENERIC | AVFILTER_FLAG_SUPPORT_TIMELINE_INTERNAL;
        /// <summary>0x1 &lt;&lt; 0x10</summary>
        public const int AVFILTER_FLAG_SUPPORT_TIMELINE_GENERIC = 0x1 << 0x10;
        /// <summary>0x1 &lt;&lt; 0x11</summary>
        public const int AVFILTER_FLAG_SUPPORT_TIMELINE_INTERNAL = 0x1 << 0x11;
        /// <summary>0x1 &lt;&lt; 0x0</summary>
        public const int AVFILTER_THREAD_SLICE = 0x1 << 0x0;
        /// <summary>0x10000</summary>
        public const int AVFMT_ALLOW_FLUSH = 0x10000;
        /// <summary>-1</summary>
        public const int AVFMT_AVOID_NEG_TS_AUTO = -0x1;
        /// <summary>1</summary>
        public const int AVFMT_AVOID_NEG_TS_MAKE_NON_NEGATIVE = 0x1;
        /// <summary>2</summary>
        public const int AVFMT_AVOID_NEG_TS_MAKE_ZERO = 0x2;
        /// <summary>0x0001</summary>
        public const int AVFMT_EVENT_FLAG_METADATA_UPDATED = 0x1;
        /// <summary>0x200000</summary>
        public const int AVFMT_FLAG_AUTO_BSF = 0x200000;
        /// <summary>0x0400</summary>
        public const int AVFMT_FLAG_BITEXACT = 0x400;
        /// <summary>0x0080</summary>
        public const int AVFMT_FLAG_CUSTOM_IO = 0x80;
        /// <summary>0x0100</summary>
        public const int AVFMT_FLAG_DISCARD_CORRUPT = 0x100;
        /// <summary>0x80000</summary>
        public const int AVFMT_FLAG_FAST_SEEK = 0x80000;
        /// <summary>0x0200</summary>
        public const int AVFMT_FLAG_FLUSH_PACKETS = 0x200;
        /// <summary>0x0001</summary>
        public const int AVFMT_FLAG_GENPTS = 0x1;
        /// <summary>0x0008</summary>
        public const int AVFMT_FLAG_IGNDTS = 0x8;
        /// <summary>0x0002</summary>
        public const int AVFMT_FLAG_IGNIDX = 0x2;
        /// <summary>0x40000</summary>
        public const int AVFMT_FLAG_KEEP_SIDE_DATA = 0x40000;
        /// <summary>0x8000</summary>
        public const int AVFMT_FLAG_MP4A_LATM = 0x8000;
        /// <summary>0x0040</summary>
        public const int AVFMT_FLAG_NOBUFFER = 0x40;
        /// <summary>0x0010</summary>
        public const int AVFMT_FLAG_NOFILLIN = 0x10;
        /// <summary>0x0004</summary>
        public const int AVFMT_FLAG_NONBLOCK = 0x4;
        /// <summary>0x0020</summary>
        public const int AVFMT_FLAG_NOPARSE = 0x20;
        /// <summary>0x20000</summary>
        public const int AVFMT_FLAG_PRIV_OPT = 0x20000;
        /// <summary>0x100000</summary>
        public const int AVFMT_FLAG_SHORTEST = 0x100000;
        /// <summary>0x10000</summary>
        public const int AVFMT_FLAG_SORT_DTS = 0x10000;
        /// <summary>0x0100</summary>
        public const int AVFMT_GENERIC_INDEX = 0x100;
        /// <summary>0x0040</summary>
        public const int AVFMT_GLOBALHEADER = 0x40;
        /// <summary>0x0002</summary>
        public const int AVFMT_NEEDNUMBER = 0x2;
        /// <summary>0x8000</summary>
        public const int AVFMT_NO_BYTE_SEEK = 0x8000;
        /// <summary>0x2000</summary>
        public const int AVFMT_NOBINSEARCH = 0x2000;
        /// <summary>0x0800</summary>
        public const int AVFMT_NODIMENSIONS = 0x800;
        /// <summary>0x0001</summary>
        public const int AVFMT_NOFILE = 0x1;
        /// <summary>0x4000</summary>
        public const int AVFMT_NOGENSEARCH = 0x4000;
        /// <summary>0x1000</summary>
        public const int AVFMT_NOSTREAMS = 0x1000;
        /// <summary>0x0080</summary>
        public const int AVFMT_NOTIMESTAMPS = 0x80;
        /// <summary>0x0020</summary>
        public const int AVFMT_RAWPICTURE = 0x20;
        /// <summary>0x4000000</summary>
        public const int AVFMT_SEEK_TO_PTS = 0x4000000;
        /// <summary>0x0008</summary>
        public const int AVFMT_SHOW_IDS = 0x8;
        /// <summary>0x0200</summary>
        public const int AVFMT_TS_DISCONT = 0x200;
        /// <summary>0x40000</summary>
        public const int AVFMT_TS_NEGATIVE = 0x40000;
        /// <summary>0x20000</summary>
        public const int AVFMT_TS_NONSTRICT = 0x20000;
        /// <summary>0x0400</summary>
        public const int AVFMT_VARIABLE_FPS = 0x400;
        /// <summary>0x0001</summary>
        public const int AVFMTCTX_NOHEADER = 0x1;
        /// <summary>0x0002</summary>
        public const int AVINDEX_DISCARD_FRAME = 0x2;
        /// <summary>0x0001</summary>
        public const int AVINDEX_KEYFRAME = 0x1;
        /// <summary>0x8000</summary>
        public const int AVIO_FLAG_DIRECT = 0x8000;
        /// <summary>8</summary>
        public const int AVIO_FLAG_NONBLOCK = 0x8;
        /// <summary>1</summary>
        public const int AVIO_FLAG_READ = 0x1;
        /// <summary>(AVIO_FLAG_READ|AVIO_FLAG_WRITE)</summary>
        public const int AVIO_FLAG_READ_WRITE = AVIO_FLAG_READ | AVIO_FLAG_WRITE;
        /// <summary>2</summary>
        public const int AVIO_FLAG_WRITE = 0x2;
        /// <summary>(1 &lt;&lt; 0)</summary>
        public const int AVIO_SEEKABLE_NORMAL = 0x1 << 0x0;
        /// <summary>(1 &lt;&lt; 1)</summary>
        public const int AVIO_SEEKABLE_TIME = 0x1 << 0x1;
        /// <summary>256</summary>
        public const int AVPALETTE_COUNT = 0x100;
        /// <summary>1024</summary>
        public const int AVPALETTE_SIZE = 0x400;
        /// <summary>32</summary>
        public const int AVPROBE_PADDING_SIZE = 0x20;
        /// <summary>50</summary>
        public const int AVPROBE_SCORE_EXTENSION = 0x32;
        /// <summary>100</summary>
        public const int AVPROBE_SCORE_MAX = 0x64;
        /// <summary>75</summary>
        public const int AVPROBE_SCORE_MIME = 0x4b;
        /// <summary>(AVPROBE_SCORE_MAX/4)</summary>
        public const int AVPROBE_SCORE_RETRY = AVPROBE_SCORE_MAX / 0x4;
        /// <summary>(AVPROBE_SCORE_MAX/4-1)</summary>
        public const int AVPROBE_SCORE_STREAM_RETRY = AVPROBE_SCORE_MAX / 0x4 - 0x1;
        /// <summary>4</summary>
        public const int AVSEEK_FLAG_ANY = 0x4;
        /// <summary>1</summary>
        public const int AVSEEK_FLAG_BACKWARD = 0x1;
        /// <summary>2</summary>
        public const int AVSEEK_FLAG_BYTE = 0x2;
        /// <summary>8</summary>
        public const int AVSEEK_FLAG_FRAME = 0x8;
        /// <summary>0x20000</summary>
        public const int AVSEEK_FORCE = 0x20000;
        /// <summary>0x10000</summary>
        public const int AVSEEK_SIZE = 0x10000;
        /// <summary>0x0001</summary>
        public const int AVSTREAM_EVENT_FLAG_METADATA_UPDATED = 0x1;
        /// <summary>1</summary>
        public const int AVSTREAM_INIT_IN_INIT_OUTPUT = 0x1;
        /// <summary>0</summary>
        public const int AVSTREAM_INIT_IN_WRITE_HEADER = 0x0;
        /// <summary>AV_CODEC_CAP_AUTO_THREADS</summary>
        public const int CODEC_CAP_AUTO_THREADS = AV_CODEC_CAP_AUTO_THREADS;
        /// <summary>AV_CODEC_CAP_CHANNEL_CONF</summary>
        public const int CODEC_CAP_CHANNEL_CONF = AV_CODEC_CAP_CHANNEL_CONF;
        /// <summary>AV_CODEC_CAP_DELAY</summary>
        public const int CODEC_CAP_DELAY = AV_CODEC_CAP_DELAY;
        /// <summary>AV_CODEC_CAP_DR1</summary>
        public const int CODEC_CAP_DR1 = AV_CODEC_CAP_DR1;
        /// <summary>AV_CODEC_CAP_DRAW_HORIZ_BAND</summary>
        public const int CODEC_CAP_DRAW_HORIZ_BAND = AV_CODEC_CAP_DRAW_HORIZ_BAND;
        /// <summary>AV_CODEC_CAP_EXPERIMENTAL</summary>
        public const int CODEC_CAP_EXPERIMENTAL = AV_CODEC_CAP_EXPERIMENTAL;
        /// <summary>AV_CODEC_CAP_FRAME_THREADS</summary>
        public const int CODEC_CAP_FRAME_THREADS = AV_CODEC_CAP_FRAME_THREADS;
        /// <summary>0x0010</summary>
        public const int CODEC_CAP_HWACCEL = 0x10;
        /// <summary>AV_CODEC_CAP_HWACCEL_VDPAU</summary>
        public const int CODEC_CAP_HWACCEL_VDPAU = AV_CODEC_CAP_HWACCEL_VDPAU;
        /// <summary>AV_CODEC_CAP_INTRA_ONLY</summary>
        public const int CODEC_CAP_INTRA_ONLY = AV_CODEC_CAP_INTRA_ONLY;
        /// <summary>AV_CODEC_CAP_LOSSLESS</summary>
        public const uint CODEC_CAP_LOSSLESS = AV_CODEC_CAP_LOSSLESS;
        /// <summary>0x0800</summary>
        public const int CODEC_CAP_NEG_LINESIZES = 0x800;
        /// <summary>AV_CODEC_CAP_PARAM_CHANGE</summary>
        public const int CODEC_CAP_PARAM_CHANGE = AV_CODEC_CAP_PARAM_CHANGE;
        /// <summary>AV_CODEC_CAP_SLICE_THREADS</summary>
        public const int CODEC_CAP_SLICE_THREADS = AV_CODEC_CAP_SLICE_THREADS;
        /// <summary>AV_CODEC_CAP_SMALL_LAST_FRAME</summary>
        public const int CODEC_CAP_SMALL_LAST_FRAME = AV_CODEC_CAP_SMALL_LAST_FRAME;
        /// <summary>AV_CODEC_CAP_SUBFRAMES</summary>
        public const int CODEC_CAP_SUBFRAMES = AV_CODEC_CAP_SUBFRAMES;
        /// <summary>AV_CODEC_CAP_TRUNCATED</summary>
        public const int CODEC_CAP_TRUNCATED = AV_CODEC_CAP_TRUNCATED;
        /// <summary>AV_CODEC_CAP_VARIABLE_FRAME_SIZE</summary>
        public const int CODEC_CAP_VARIABLE_FRAME_SIZE = AV_CODEC_CAP_VARIABLE_FRAME_SIZE;
        /// <summary>AV_CODEC_FLAG_4MV</summary>
        public const int CODEC_FLAG_4MV = AV_CODEC_FLAG_4MV;
        /// <summary>AV_CODEC_FLAG_AC_PRED</summary>
        public const int CODEC_FLAG_AC_PRED = AV_CODEC_FLAG_AC_PRED;
        /// <summary>AV_CODEC_FLAG_BITEXACT</summary>
        public const int CODEC_FLAG_BITEXACT = AV_CODEC_FLAG_BITEXACT;
        /// <summary>AV_CODEC_FLAG_CLOSED_GOP</summary>
        public const uint CODEC_FLAG_CLOSED_GOP = AV_CODEC_FLAG_CLOSED_GOP;
        /// <summary>0x4000</summary>
        public const int CODEC_FLAG_EMU_EDGE = 0x4000;
        /// <summary>AV_CODEC_FLAG_GLOBAL_HEADER</summary>
        public const int CODEC_FLAG_GLOBAL_HEADER = AV_CODEC_FLAG_GLOBAL_HEADER;
        /// <summary>0x0020</summary>
        public const int CODEC_FLAG_GMC = 0x20;
        /// <summary>AV_CODEC_FLAG_GRAY</summary>
        public const int CODEC_FLAG_GRAY = AV_CODEC_FLAG_GRAY;
        /// <summary>0x0100</summary>
        public const int CODEC_FLAG_INPUT_PRESERVED = 0x100;
        /// <summary>AV_CODEC_FLAG_INTERLACED_DCT</summary>
        public const int CODEC_FLAG_INTERLACED_DCT = AV_CODEC_FLAG_INTERLACED_DCT;
        /// <summary>AV_CODEC_FLAG_INTERLACED_ME</summary>
        public const int CODEC_FLAG_INTERLACED_ME = AV_CODEC_FLAG_INTERLACED_ME;
        /// <summary>AV_CODEC_FLAG_LOOP_FILTER</summary>
        public const int CODEC_FLAG_LOOP_FILTER = AV_CODEC_FLAG_LOOP_FILTER;
        /// <summary>AV_CODEC_FLAG_LOW_DELAY</summary>
        public const int CODEC_FLAG_LOW_DELAY = AV_CODEC_FLAG_LOW_DELAY;
        /// <summary>0x0040</summary>
        public const int CODEC_FLAG_MV0 = 0x40;
        /// <summary>0x00020000</summary>
        public const int CODEC_FLAG_NORMALIZE_AQP = 0x20000;
        /// <summary>AV_CODEC_FLAG_OUTPUT_CORRUPT</summary>
        public const int CODEC_FLAG_OUTPUT_CORRUPT = AV_CODEC_FLAG_OUTPUT_CORRUPT;
        /// <summary>AV_CODEC_FLAG_PASS1</summary>
        public const int CODEC_FLAG_PASS1 = AV_CODEC_FLAG_PASS1;
        /// <summary>AV_CODEC_FLAG_PASS2</summary>
        public const int CODEC_FLAG_PASS2 = AV_CODEC_FLAG_PASS2;
        /// <summary>AV_CODEC_FLAG_PSNR</summary>
        public const int CODEC_FLAG_PSNR = AV_CODEC_FLAG_PSNR;
        /// <summary>AV_CODEC_FLAG_QPEL</summary>
        public const int CODEC_FLAG_QPEL = AV_CODEC_FLAG_QPEL;
        /// <summary>AV_CODEC_FLAG_QSCALE</summary>
        public const int CODEC_FLAG_QSCALE = AV_CODEC_FLAG_QSCALE;
        /// <summary>AV_CODEC_FLAG_TRUNCATED</summary>
        public const int CODEC_FLAG_TRUNCATED = AV_CODEC_FLAG_TRUNCATED;
        /// <summary>AV_CODEC_FLAG_UNALIGNED</summary>
        public const int CODEC_FLAG_UNALIGNED = AV_CODEC_FLAG_UNALIGNED;
        /// <summary>AV_CODEC_FLAG2_CHUNKS</summary>
        public const int CODEC_FLAG2_CHUNKS = AV_CODEC_FLAG2_CHUNKS;
        /// <summary>AV_CODEC_FLAG2_DROP_FRAME_TIMECODE</summary>
        public const int CODEC_FLAG2_DROP_FRAME_TIMECODE = AV_CODEC_FLAG2_DROP_FRAME_TIMECODE;
        /// <summary>AV_CODEC_FLAG2_EXPORT_MVS</summary>
        public const int CODEC_FLAG2_EXPORT_MVS = AV_CODEC_FLAG2_EXPORT_MVS;
        /// <summary>AV_CODEC_FLAG2_FAST</summary>
        public const int CODEC_FLAG2_FAST = AV_CODEC_FLAG2_FAST;
        /// <summary>AV_CODEC_FLAG2_IGNORE_CROP</summary>
        public const int CODEC_FLAG2_IGNORE_CROP = AV_CODEC_FLAG2_IGNORE_CROP;
        /// <summary>AV_CODEC_FLAG2_LOCAL_HEADER</summary>
        public const int CODEC_FLAG2_LOCAL_HEADER = AV_CODEC_FLAG2_LOCAL_HEADER;
        /// <summary>AV_CODEC_FLAG2_NO_OUTPUT</summary>
        public const int CODEC_FLAG2_NO_OUTPUT = AV_CODEC_FLAG2_NO_OUTPUT;
        /// <summary>AV_CODEC_FLAG2_SHOW_ALL</summary>
        public const int CODEC_FLAG2_SHOW_ALL = AV_CODEC_FLAG2_SHOW_ALL;
        /// <summary>AV_CODEC_FLAG2_SKIP_MANUAL</summary>
        public const int CODEC_FLAG2_SKIP_MANUAL = AV_CODEC_FLAG2_SKIP_MANUAL;
        /// <summary>(LIBAVCODEC_VERSION_MAJOR &lt; 58)</summary>
        public const bool FF_API_AC_VLC = LIBAVCODEC_VERSION_MAJOR < 0x3a;
        /// <summary>(LIBAVCODEC_VERSION_MAJOR &lt; 58)</summary>
        public const bool FF_API_AFD = LIBAVCODEC_VERSION_MAJOR < 0x3a;
        /// <summary>(LIBAVCODEC_VERSION_MAJOR &lt; 58)</summary>
        public const bool FF_API_ARCH_ALPHA = LIBAVCODEC_VERSION_MAJOR < 0x3a;
        /// <summary>(LIBAVCODEC_VERSION_MAJOR &lt; 58)</summary>
        public const bool FF_API_ARCH_SH4 = LIBAVCODEC_VERSION_MAJOR < 0x3a;
        /// <summary>(LIBAVCODEC_VERSION_MAJOR &lt; 58)</summary>
        public const bool FF_API_ARCH_SPARC = LIBAVCODEC_VERSION_MAJOR < 0x3a;
        /// <summary>(LIBAVCODEC_VERSION_MAJOR &lt; 58)</summary>
        public const bool FF_API_ASPECT_EXTENDED = LIBAVCODEC_VERSION_MAJOR < 0x3a;
        /// <summary>(LIBAVCODEC_VERSION_MAJOR &lt; 59)</summary>
        public const bool FF_API_ASS_TIMING = LIBAVCODEC_VERSION_MAJOR < 0x3b;
        /// <summary>(LIBAVCODEC_VERSION_MAJOR &lt; 58)</summary>
        public const bool FF_API_AUDIO_CONVERT = LIBAVCODEC_VERSION_MAJOR < 0x3a;
        /// <summary>(LIBAVCODEC_VERSION_MAJOR &lt; 58)</summary>
        public const bool FF_API_AUDIOENC_DELAY = LIBAVCODEC_VERSION_MAJOR < 0x3a;
        /// <summary>FF_API_AUDIO_CONVERT</summary>
        public const bool FF_API_AVCODEC_RESAMPLE = FF_API_AUDIO_CONVERT;
        /// <summary>(LIBAVCODEC_VERSION_MAJOR &lt; 59)</summary>
        public const bool FF_API_AVCTX_TIMEBASE = LIBAVCODEC_VERSION_MAJOR < 0x3b;
        /// <summary>LIBAVFILTER_VERSION_MAJOR &lt; 0x7</summary>
        public const bool FF_API_AVFILTER_INIT_FILTER = LIBAVFILTER_VERSION_MAJOR < 0x7;
        /// <summary>LIBAVFILTER_VERSION_MAJOR &lt; 0x7</summary>
        public const bool FF_API_AVFILTER_OPEN = LIBAVFILTER_VERSION_MAJOR < 0x7;
        /// <summary>(LIBAVCODEC_VERSION_MAJOR &lt; 59)</summary>
        public const bool FF_API_AVPACKET_OLD_API = LIBAVCODEC_VERSION_MAJOR < 0x3b;
        /// <summary>(LIBAVCODEC_VERSION_MAJOR &lt; 59)</summary>
        public const bool FF_API_AVPICTURE = LIBAVCODEC_VERSION_MAJOR < 0x3b;
        /// <summary>(LIBAVCODEC_VERSION_MAJOR &lt; 58)</summary>
        public const bool FF_API_BUFS_VDPAU = LIBAVCODEC_VERSION_MAJOR < 0x3a;
        /// <summary>(LIBAVCODEC_VERSION_MAJOR &lt; 58)</summary>
        public const bool FF_API_CAP_VDPAU = LIBAVCODEC_VERSION_MAJOR < 0x3a;
        /// <summary>(LIBAVCODEC_VERSION_MAJOR &lt; 58)</summary>
        public const bool FF_API_CODEC_NAME = LIBAVCODEC_VERSION_MAJOR < 0x3a;
        /// <summary>(LIBAVCODEC_VERSION_MAJOR &lt; 59)</summary>
        public const bool FF_API_CODED_FRAME = LIBAVCODEC_VERSION_MAJOR < 0x3b;
        /// <summary>(LIBAVCODEC_VERSION_MAJOR &lt; 59)</summary>
        public const bool FF_API_CODER_TYPE = LIBAVCODEC_VERSION_MAJOR < 0x3b;
        /// <summary>(LIBAVFORMAT_VERSION_MAJOR &lt; 58)</summary>
        public const bool FF_API_COMPUTE_PKT_FIELDS2 = LIBAVFORMAT_VERSION_MAJOR < 0x3a;
        /// <summary>(LIBAVCODEC_VERSION_MAJOR &lt; 59)</summary>
        public const bool FF_API_CONVERGENCE_DURATION = LIBAVCODEC_VERSION_MAJOR < 0x3b;
        /// <summary>(LIBAVCODEC_VERSION_MAJOR &lt; 59)</summary>
        public const bool FF_API_COPY_CONTEXT = LIBAVCODEC_VERSION_MAJOR < 0x3b;
        /// <summary>(LIBAVUTIL_VERSION_MAJOR &lt; 56)</summary>
        public const bool FF_API_CRC_BIG_TABLE = LIBAVUTIL_VERSION_MAJOR < 0x38;
        /// <summary>(LIBAVCODEC_VERSION_MAJOR &lt; 58)</summary>
        public const bool FF_API_DEBUG_MV = LIBAVCODEC_VERSION_MAJOR < 0x3a;
        /// <summary>(LIBAVUTIL_VERSION_MAJOR &lt; 56)</summary>
        public const bool FF_API_DLOG = LIBAVUTIL_VERSION_MAJOR < 0x38;
        /// <summary>(LIBAVCODEC_VERSION_MAJOR &lt; 58)</summary>
        public const bool FF_API_EMU_EDGE = LIBAVCODEC_VERSION_MAJOR < 0x3a;
        /// <summary>(LIBAVUTIL_VERSION_MAJOR &lt; 56)</summary>
        public const bool FF_API_ERROR_FRAME = LIBAVUTIL_VERSION_MAJOR < 0x38;
        /// <summary>(LIBAVCODEC_VERSION_MAJOR &lt; 58)</summary>
        public const bool FF_API_ERROR_RATE = LIBAVCODEC_VERSION_MAJOR < 0x3a;
        /// <summary>(LIBAVUTIL_VERSION_MAJOR &lt; 56)</summary>
        public const bool FF_API_FRAME_QP = LIBAVUTIL_VERSION_MAJOR < 0x38;
        /// <summary>(LIBAVCODEC_VERSION_MAJOR &lt; 59)</summary>
        public const bool FF_API_GET_CONTEXT_DEFAULTS = LIBAVCODEC_VERSION_MAJOR < 0x3b;
        /// <summary>(LIBAVCODEC_VERSION_MAJOR &lt; 58)</summary>
        public const bool FF_API_GETCHROMA = LIBAVCODEC_VERSION_MAJOR < 0x3a;
        /// <summary>(LIBAVCODEC_VERSION_MAJOR &lt; 58)</summary>
        public const bool FF_API_GMC = LIBAVCODEC_VERSION_MAJOR < 0x3a;
        /// <summary>(LIBAVFORMAT_VERSION_MAJOR &lt; 58)</summary>
        public const bool FF_API_HLS_WRAP = LIBAVFORMAT_VERSION_MAJOR < 0x3a;
        /// <summary>(LIBAVFORMAT_VERSION_MAJOR &lt; 58)</summary>
        public const bool FF_API_HTTP_USER_AGENT = LIBAVFORMAT_VERSION_MAJOR < 0x3a;
        /// <summary>(LIBAVCODEC_VERSION_MAJOR &lt; 58)</summary>
        public const bool FF_API_IDCT_XVIDMMX = LIBAVCODEC_VERSION_MAJOR < 0x3a;
        /// <summary>(LIBAVCODEC_VERSION_MAJOR &lt; 58)</summary>
        public const bool FF_API_INPUT_PRESERVED = LIBAVCODEC_VERSION_MAJOR < 0x3a;
        /// <summary>(LIBAVFORMAT_VERSION_MAJOR &lt; 58)</summary>
        public const bool FF_API_LAVF_AVCTX = LIBAVFORMAT_VERSION_MAJOR < 0x3a;
        /// <summary>(LIBAVFORMAT_VERSION_MAJOR &lt; 58)</summary>
        public const bool FF_API_LAVF_BITEXACT = LIBAVFORMAT_VERSION_MAJOR < 0x3a;
        /// <summary>(LIBAVFORMAT_VERSION_MAJOR &lt; 58)</summary>
        public const bool FF_API_LAVF_CODEC_TB = LIBAVFORMAT_VERSION_MAJOR < 0x3a;
        /// <summary>(LIBAVFORMAT_VERSION_MAJOR &lt; 58)</summary>
        public const bool FF_API_LAVF_FMT_RAWPICTURE = LIBAVFORMAT_VERSION_MAJOR < 0x3a;
        /// <summary>(LIBAVFORMAT_VERSION_MAJOR &lt; 58)</summary>
        public const bool FF_API_LAVF_FRAC = LIBAVFORMAT_VERSION_MAJOR < 0x3a;
        /// <summary>(LIBAVFORMAT_VERSION_MAJOR &lt; 58)</summary>
        public const bool FF_API_LAVF_KEEPSIDE_FLAG = LIBAVFORMAT_VERSION_MAJOR < 0x3a;
        /// <summary>(LIBAVFORMAT_VERSION_MAJOR &lt; 58)</summary>
        public const bool FF_API_LAVF_MERGE_SD = LIBAVFORMAT_VERSION_MAJOR < 0x3a;
        /// <summary>LIBAVFILTER_VERSION_MAJOR &lt; 0x7</summary>
        public const bool FF_API_LAVR_OPTS = LIBAVFILTER_VERSION_MAJOR < 0x7;
        /// <summary>(LIBAVCODEC_VERSION_MAJOR &lt; 58)</summary>
        public const bool FF_API_LOWRES = LIBAVCODEC_VERSION_MAJOR < 0x3a;
        /// <summary>(LIBAVCODEC_VERSION_MAJOR &lt; 58)</summary>
        public const bool FF_API_MAX_BFRAMES = LIBAVCODEC_VERSION_MAJOR < 0x3a;
        /// <summary>(LIBAVCODEC_VERSION_MAJOR &lt; 58)</summary>
        public const bool FF_API_MB_TYPE = LIBAVCODEC_VERSION_MAJOR < 0x3a;
        /// <summary>(LIBAVCODEC_VERSION_MAJOR &lt; 58)</summary>
        public const bool FF_API_MERGE_SD = LIBAVCODEC_VERSION_MAJOR < 0x3a;
        /// <summary>(LIBAVCODEC_VERSION_MAJOR &lt; 59)</summary>
        public const bool FF_API_MERGE_SD_API = LIBAVCODEC_VERSION_MAJOR < 0x3b;
        /// <summary>(LIBAVCODEC_VERSION_MAJOR &lt; 58)</summary>
        public const bool FF_API_MISSING_SAMPLE = LIBAVCODEC_VERSION_MAJOR < 0x3a;
        /// <summary>(LIBAVCODEC_VERSION_MAJOR &lt; 59)</summary>
        public const bool FF_API_MOTION_EST = LIBAVCODEC_VERSION_MAJOR < 0x3b;
        /// <summary>(LIBAVCODEC_VERSION_MAJOR &lt; 59)</summary>
        public const bool FF_API_MPV_OPT = LIBAVCODEC_VERSION_MAJOR < 0x3b;
        /// <summary>(LIBAVCODEC_VERSION_MAJOR &lt; 58)</summary>
        public const bool FF_API_MV0 = LIBAVCODEC_VERSION_MAJOR < 0x3a;
        /// <summary>(LIBAVCODEC_VERSION_MAJOR &lt; 58)</summary>
        public const bool FF_API_NEG_LINESIZES = LIBAVCODEC_VERSION_MAJOR < 0x3a;
        /// <summary>LIBAVFILTER_VERSION_MAJOR &lt; 0x7</summary>
        public const bool FF_API_NOCONST_GET_NAME = LIBAVFILTER_VERSION_MAJOR < 0x7;
        /// <summary>(LIBAVFORMAT_VERSION_MAJOR &lt; 58)</summary>
        public const bool FF_API_NOCONST_GET_SIDE_DATA = LIBAVFORMAT_VERSION_MAJOR < 0x3a;
        /// <summary>(LIBAVCODEC_VERSION_MAJOR &lt; 58)</summary>
        public const bool FF_API_NORMALIZE_AQP = LIBAVCODEC_VERSION_MAJOR < 0x3a;
        /// <summary>(LIBAVCODEC_VERSION_MAJOR &lt; 59)</summary>
        public const bool FF_API_NVENC_OLD_NAME = LIBAVCODEC_VERSION_MAJOR < 0x3b;
        /// <summary>(LIBAVCODEC_VERSION_MAJOR &lt; 59)</summary>
        public const bool FF_API_OLD_BSF = LIBAVCODEC_VERSION_MAJOR < 0x3b;
        /// <summary>LIBAVFILTER_VERSION_MAJOR &lt; 0x7</summary>
        public const bool FF_API_OLD_FILTER_OPTS = LIBAVFILTER_VERSION_MAJOR < 0x7;
        /// <summary>LIBAVFILTER_VERSION_MAJOR &lt; 0x7</summary>
        public const bool FF_API_OLD_FILTER_OPTS_ERROR = LIBAVFILTER_VERSION_MAJOR < 0x7;
        /// <summary>LIBAVFILTER_VERSION_MAJOR &lt; 0x7</summary>
        public const bool FF_API_OLD_FILTER_REGISTER = LIBAVFILTER_VERSION_MAJOR < 0x7;
        /// <summary>(LIBAVCODEC_VERSION_MAJOR &lt; 58)</summary>
        public const bool FF_API_OLD_MSMPEG4 = LIBAVCODEC_VERSION_MAJOR < 0x3a;
        /// <summary>(LIBAVFORMAT_VERSION_MAJOR &lt; 58)</summary>
        public const bool FF_API_OLD_OPEN_CALLBACKS = LIBAVFORMAT_VERSION_MAJOR < 0x3a;
        /// <summary>(LIBAVFORMAT_VERSION_MAJOR &lt; 58)</summary>
        public const bool FF_API_OLD_ROTATE_API = LIBAVFORMAT_VERSION_MAJOR < 0x3a;
        /// <summary>(LIBAVUTIL_VERSION_MAJOR &lt; 56)</summary>
        public const bool FF_API_OPT_TYPE_METADATA = LIBAVUTIL_VERSION_MAJOR < 0x38;
        /// <summary>(LIBAVUTIL_VERSION_MAJOR &lt; 56)</summary>
        public const bool FF_API_PKT_PTS = LIBAVUTIL_VERSION_MAJOR < 0x38;
        /// <summary>(LIBAVUTIL_VERSION_MAJOR &lt; 56)</summary>
        public const bool FF_API_PLUS1_MINUS1 = LIBAVUTIL_VERSION_MAJOR < 0x38;
        /// <summary>(LIBAVCODEC_VERSION_MAJOR &lt; 59)</summary>
        public const bool FF_API_PRIVATE_OPT = LIBAVCODEC_VERSION_MAJOR < 0x3b;
        /// <summary>LIBPOSTPROC_VERSION_MAJOR &lt; 0x37</summary>
        public const bool FF_API_QP_TYPE = LIBPOSTPROC_VERSION_MAJOR < 0x37;
        /// <summary>(LIBAVCODEC_VERSION_MAJOR &lt; 58)</summary>
        public const bool FF_API_QSCALE_TYPE = LIBAVCODEC_VERSION_MAJOR < 0x3a;
        /// <summary>(LIBAVCODEC_VERSION_MAJOR &lt; 59)</summary>
        public const bool FF_API_QUANT_BIAS = LIBAVCODEC_VERSION_MAJOR < 0x3b;
        /// <summary>1</summary>
        public const int FF_API_R_FRAME_RATE = 0x1;
        /// <summary>(LIBAVCODEC_VERSION_MAJOR &lt; 59)</summary>
        public const bool FF_API_RC_STRATEGY = LIBAVCODEC_VERSION_MAJOR < 0x3b;
        /// <summary>(LIBAVCODEC_VERSION_MAJOR &lt; 59)</summary>
        public const bool FF_API_RTP_CALLBACK = LIBAVCODEC_VERSION_MAJOR < 0x3b;
        /// <summary>(LIBAVCODEC_VERSION_MAJOR &lt; 58)</summary>
        public const bool FF_API_SET_DIMENSIONS = LIBAVCODEC_VERSION_MAJOR < 0x3a;
        /// <summary>(LIBAVCODEC_VERSION_MAJOR &lt; 59)</summary>
        public const bool FF_API_SIDEDATA_ONLY_PKT = LIBAVCODEC_VERSION_MAJOR < 0x3b;
        /// <summary>(LIBAVCODEC_VERSION_MAJOR &lt; 59)</summary>
        public const bool FF_API_STAT_BITS = LIBAVCODEC_VERSION_MAJOR < 0x3b;
        /// <summary>(LIBAVCODEC_VERSION_MAJOR &lt; 59)</summary>
        public const bool FF_API_STREAM_CODEC_TAG = LIBAVCODEC_VERSION_MAJOR < 0x3b;
        /// <summary>(LIBAVCODEC_VERSION_MAJOR &lt; 59)</summary>
        public const bool FF_API_STRUCT_VAAPI_CONTEXT = LIBAVCODEC_VERSION_MAJOR < 0x3b;
        /// <summary>LIBSWSCALE_VERSION_MAJOR &lt; 0x6</summary>
        public const bool FF_API_SWS_VECTOR = LIBSWSCALE_VERSION_MAJOR < 0x6;
        /// <summary>(LIBAVCODEC_VERSION_MAJOR &lt; 59)</summary>
        public const bool FF_API_TAG_STRING = LIBAVCODEC_VERSION_MAJOR < 0x3b;
        /// <summary>(LIBAVCODEC_VERSION_MAJOR &lt; 58)</summary>
        public const bool FF_API_UNUSED_MEMBERS = LIBAVCODEC_VERSION_MAJOR < 0x3a;
        /// <summary>(LIBAVFORMAT_VERSION_MAJOR &lt; 58)</summary>
        public const bool FF_API_URL_FEOF = LIBAVFORMAT_VERSION_MAJOR < 0x3a;
        /// <summary>(LIBAVUTIL_VERSION_MAJOR &lt; 56)</summary>
        public const bool FF_API_VAAPI = LIBAVUTIL_VERSION_MAJOR < 0x38;
        /// <summary>(LIBAVCODEC_VERSION_MAJOR &lt; 58)</summary>
        public const bool FF_API_VAAPI_CONTEXT = LIBAVCODEC_VERSION_MAJOR < 0x3a;
        /// <summary>(LIBAVCODEC_VERSION_MAJOR &lt; 59)</summary>
        public const bool FF_API_VBV_DELAY = LIBAVCODEC_VERSION_MAJOR < 0x3b;
        /// <summary>(LIBAVUTIL_VERSION_MAJOR &lt; 56)</summary>
        public const bool FF_API_VDPAU = LIBAVUTIL_VERSION_MAJOR < 0x38;
        /// <summary>(LIBAVCODEC_VERSION_MAJOR &lt; 59)</summary>
        public const bool FF_API_VDPAU_PROFILE = LIBAVCODEC_VERSION_MAJOR < 0x3b;
        /// <summary>(LIBAVCODEC_VERSION_MAJOR &lt; 58)</summary>
        public const bool FF_API_VIMA_DECODER = LIBAVCODEC_VERSION_MAJOR < 0x3a;
        /// <summary>(LIBAVCODEC_VERSION_MAJOR &lt; 58)</summary>
        public const bool FF_API_VISMV = LIBAVCODEC_VERSION_MAJOR < 0x3a;
        /// <summary>(LIBAVCODEC_VERSION_MAJOR &lt; 58)</summary>
        public const bool FF_API_VOXWARE = LIBAVCODEC_VERSION_MAJOR < 0x3a;
        /// <summary>(LIBAVCODEC_VERSION_MAJOR &lt; 59)</summary>
        public const bool FF_API_WITHOUT_PREFIX = LIBAVCODEC_VERSION_MAJOR < 0x3b;
        /// <summary>(LIBAVUTIL_VERSION_MAJOR &lt; 56)</summary>
        public const bool FF_API_XVMC = LIBAVUTIL_VERSION_MAJOR < 0x38;
        /// <summary>15</summary>
        public const int FF_ASPECT_EXTENDED = 0xf;
        /// <summary>0</summary>
        public const int FF_BUG_AC_VLC = 0x0;
        /// <summary>32</summary>
        public const int FF_BUG_AMV = 0x20;
        /// <summary>1</summary>
        public const int FF_BUG_AUTODETECT = 0x1;
        /// <summary>4096</summary>
        public const int FF_BUG_DC_CLIP = 0x1000;
        /// <summary>512</summary>
        public const int FF_BUG_DIRECT_BLOCKSIZE = 0x200;
        /// <summary>1024</summary>
        public const int FF_BUG_EDGE = 0x400;
        /// <summary>2048</summary>
        public const int FF_BUG_HPEL_CHROMA = 0x800;
        /// <summary>32768</summary>
        public const int FF_BUG_IEDGE = 0x8000;
        /// <summary>8192</summary>
        public const int FF_BUG_MS = 0x2000;
        /// <summary>16</summary>
        public const int FF_BUG_NO_PADDING = 0x10;
        /// <summary>2</summary>
        public const int FF_BUG_OLD_MSMPEG4 = 0x2;
        /// <summary>64</summary>
        public const int FF_BUG_QPEL_CHROMA = 0x40;
        /// <summary>256</summary>
        public const int FF_BUG_QPEL_CHROMA2 = 0x100;
        /// <summary>128</summary>
        public const int FF_BUG_STD_QPEL = 0x80;
        /// <summary>16384</summary>
        public const int FF_BUG_TRUNCATED = 0x4000;
        /// <summary>8</summary>
        public const int FF_BUG_UMP4 = 0x8;
        /// <summary>4</summary>
        public const int FF_BUG_XVID_ILACE = 0x4;
        // public static FF_CEIL_RSHIFT = AV_CEIL_RSHIFT;
        /// <summary>5</summary>
        public const int FF_CMP_BIT = 0x5;
        /// <summary>256</summary>
        public const int FF_CMP_CHROMA = 0x100;
        /// <summary>3</summary>
        public const int FF_CMP_DCT = 0x3;
        /// <summary>14</summary>
        public const int FF_CMP_DCT264 = 0xe;
        /// <summary>13</summary>
        public const int FF_CMP_DCTMAX = 0xd;
        /// <summary>15</summary>
        public const int FF_CMP_MEDIAN_SAD = 0xf;
        /// <summary>10</summary>
        public const int FF_CMP_NSSE = 0xa;
        /// <summary>4</summary>
        public const int FF_CMP_PSNR = 0x4;
        /// <summary>6</summary>
        public const int FF_CMP_RD = 0x6;
        /// <summary>0</summary>
        public const int FF_CMP_SAD = 0x0;
        /// <summary>2</summary>
        public const int FF_CMP_SATD = 0x2;
        /// <summary>1</summary>
        public const int FF_CMP_SSE = 0x1;
        /// <summary>8</summary>
        public const int FF_CMP_VSAD = 0x8;
        /// <summary>9</summary>
        public const int FF_CMP_VSSE = 0x9;
        /// <summary>11</summary>
        public const int FF_CMP_W53 = 0xb;
        /// <summary>12</summary>
        public const int FF_CMP_W97 = 0xc;
        /// <summary>7</summary>
        public const int FF_CMP_ZERO = 0x7;
        /// <summary>0x00000002</summary>
        public const int FF_CODEC_PROPERTY_CLOSED_CAPTIONS = 0x2;
        /// <summary>0x00000001</summary>
        public const int FF_CODEC_PROPERTY_LOSSLESS = 0x1;
        /// <summary>1</summary>
        public const int FF_CODER_TYPE_AC = 0x1;
        /// <summary>4</summary>
        public const int FF_CODER_TYPE_DEFLATE = 0x4;
        /// <summary>2</summary>
        public const int FF_CODER_TYPE_RAW = 0x2;
        /// <summary>3</summary>
        public const int FF_CODER_TYPE_RLE = 0x3;
        /// <summary>0</summary>
        public const int FF_CODER_TYPE_VLC = 0x0;
        /// <summary>-2</summary>
        public const int FF_COMPLIANCE_EXPERIMENTAL = -0x2;
        /// <summary>0</summary>
        public const int FF_COMPLIANCE_NORMAL = 0x0;
        /// <summary>1</summary>
        public const int FF_COMPLIANCE_STRICT = 0x1;
        /// <summary>-1</summary>
        public const int FF_COMPLIANCE_UNOFFICIAL = -0x1;
        /// <summary>2</summary>
        public const int FF_COMPLIANCE_VERY_STRICT = 0x2;
        /// <summary>-1</summary>
        public const int FF_COMPRESSION_DEFAULT = -0x1;
        /// <summary>5</summary>
        public const int FF_DCT_ALTIVEC = 0x5;
        /// <summary>0</summary>
        public const int FF_DCT_AUTO = 0x0;
        /// <summary>6</summary>
        public const int FF_DCT_FAAN = 0x6;
        /// <summary>1</summary>
        public const int FF_DCT_FASTINT = 0x1;
        /// <summary>2</summary>
        public const int FF_DCT_INT = 0x2;
        /// <summary>3</summary>
        public const int FF_DCT_MMX = 0x3;
        /// <summary>4</summary>
        public const int FF_DEBUG_BITSTREAM = 0x4;
        /// <summary>0x00008000</summary>
        public const int FF_DEBUG_BUFFERS = 0x8000;
        /// <summary>0x00001000</summary>
        public const int FF_DEBUG_BUGS = 0x1000;
        /// <summary>0x00000040</summary>
        public const int FF_DEBUG_DCT_COEFF = 0x40;
        /// <summary>0x00000400</summary>
        public const int FF_DEBUG_ER = 0x400;
        /// <summary>0x00800000</summary>
        public const int FF_DEBUG_GREEN_MD = 0x800000;
        /// <summary>8</summary>
        public const int FF_DEBUG_MB_TYPE = 0x8;
        /// <summary>0x00000800</summary>
        public const int FF_DEBUG_MMCO = 0x800;
        /// <summary>32</summary>
        public const int FF_DEBUG_MV = 0x20;
        /// <summary>0x01000000</summary>
        public const int FF_DEBUG_NOMC = 0x1000000;
        /// <summary>1</summary>
        public const int FF_DEBUG_PICT_INFO = 0x1;
        /// <summary>0x00000200</summary>
        public const int FF_DEBUG_PTS = 0x200;
        /// <summary>16</summary>
        public const int FF_DEBUG_QP = 0x10;
        /// <summary>2</summary>
        public const int FF_DEBUG_RC = 0x2;
        /// <summary>0x00000080</summary>
        public const int FF_DEBUG_SKIP = 0x80;
        /// <summary>0x00000100</summary>
        public const int FF_DEBUG_STARTCODE = 0x100;
        /// <summary>0x00010000</summary>
        public const int FF_DEBUG_THREADS = 0x10000;
        /// <summary>0x00004000</summary>
        public const int FF_DEBUG_VIS_MB_TYPE = 0x4000;
        /// <summary>0x00000004</summary>
        public const int FF_DEBUG_VIS_MV_B_BACK = 0x4;
        /// <summary>0x00000002</summary>
        public const int FF_DEBUG_VIS_MV_B_FOR = 0x2;
        /// <summary>0x00000001</summary>
        public const int FF_DEBUG_VIS_MV_P_FOR = 0x1;
        /// <summary>0x00002000</summary>
        public const int FF_DEBUG_VIS_QP = 0x2000;
        /// <summary>1</summary>
        public const int FF_DECODE_ERROR_INVALID_BITSTREAM = 0x1;
        /// <summary>2</summary>
        public const int FF_DECODE_ERROR_MISSING_REFERENCE = 0x2;
        /// <summary>999999</summary>
        public const int FF_DEFAULT_QUANT_BIAS = 0xf423f;
        /// <summary>11</summary>
        public const int FF_DTG_AFD_14_9 = 0xb;
        /// <summary>10</summary>
        public const int FF_DTG_AFD_16_9 = 0xa;
        /// <summary>14</summary>
        public const int FF_DTG_AFD_16_9_SP_14_9 = 0xe;
        /// <summary>9</summary>
        public const int FF_DTG_AFD_4_3 = 0x9;
        /// <summary>13</summary>
        public const int FF_DTG_AFD_4_3_SP_14_9 = 0xd;
        /// <summary>8</summary>
        public const int FF_DTG_AFD_SAME = 0x8;
        /// <summary>15</summary>
        public const int FF_DTG_AFD_SP_4_3 = 0xf;
        /// <summary>0x2</summary>
        public const int FF_DXVA2_WORKAROUND_INTEL_CLEARVIDEO = 0x2;
        /// <summary>0x1</summary>
        public const int FF_DXVA2_WORKAROUND_SCALING_LIST_ZIGZAG = 0x1;
        /// <summary>2</summary>
        public const int FF_EC_DEBLOCK = 0x2;
        /// <summary>256</summary>
        public const int FF_EC_FAVOR_INTER = 0x100;
        /// <summary>1</summary>
        public const int FF_EC_GUESS_MVS = 0x1;
        /// <summary>0x0001</summary>
        public const int FF_FDEBUG_TS = 0x1;
        /// <summary>8</summary>
        public const int FF_IDCT_ALTIVEC = 0x8;
        /// <summary>7</summary>
        public const int FF_IDCT_ARM = 0x7;
        /// <summary>0</summary>
        public const int FF_IDCT_AUTO = 0x0;
        /// <summary>20</summary>
        public const int FF_IDCT_FAAN = 0x14;
        /// <summary>1</summary>
        public const int FF_IDCT_INT = 0x1;
        /// <summary>13</summary>
        public const int FF_IDCT_IPP = 0xd;
        /// <summary>9</summary>
        public const int FF_IDCT_SH4 = 0x9;
        /// <summary>2</summary>
        public const int FF_IDCT_SIMPLE = 0x2;
        /// <summary>23</summary>
        public const int FF_IDCT_SIMPLEALPHA = 0x17;
        /// <summary>10</summary>
        public const int FF_IDCT_SIMPLEARM = 0xa;
        /// <summary>16</summary>
        public const int FF_IDCT_SIMPLEARMV5TE = 0x10;
        /// <summary>17</summary>
        public const int FF_IDCT_SIMPLEARMV6 = 0x11;
        /// <summary>128</summary>
        public const int FF_IDCT_SIMPLEAUTO = 0x80;
        /// <summary>3</summary>
        public const int FF_IDCT_SIMPLEMMX = 0x3;
        /// <summary>22</summary>
        public const int FF_IDCT_SIMPLENEON = 0x16;
        /// <summary>18</summary>
        public const int FF_IDCT_SIMPLEVIS = 0x12;
        /// <summary>14</summary>
        public const int FF_IDCT_XVID = 0xe;
        /// <summary>14</summary>
        public const int FF_IDCT_XVIDMMX = 0xe;
        /// <summary>32</summary>
        public const int FF_INPUT_BUFFER_PADDING_SIZE = 0x20;
        /// <summary>(256*128-1)</summary>
        public const int FF_LAMBDA_MAX = 0x100 * 0x80 - 0x1;
        /// <summary>(1&lt;&lt;FF_LAMBDA_SHIFT)</summary>
        public const int FF_LAMBDA_SCALE = 0x1 << FF_LAMBDA_SHIFT;
        /// <summary>7</summary>
        public const int FF_LAMBDA_SHIFT = 0x7;
        /// <summary>-99</summary>
        public const int FF_LEVEL_UNKNOWN = -0x63;
        /// <summary>0x8</summary>
        public const int FF_LOSS_ALPHA = 0x8;
        /// <summary>0x20</summary>
        public const int FF_LOSS_CHROMA = 0x20;
        /// <summary>0x10</summary>
        public const int FF_LOSS_COLORQUANT = 0x10;
        /// <summary>0x4</summary>
        public const int FF_LOSS_COLORSPACE = 0x4;
        /// <summary>0x2</summary>
        public const int FF_LOSS_DEPTH = 0x2;
        /// <summary>0x1</summary>
        public const int FF_LOSS_RESOLUTION = 0x1;
        /// <summary>16</summary>
        public const int FF_MAX_B_FRAMES = 0x10;
        /// <summary>1</summary>
        public const int FF_MB_DECISION_BITS = 0x1;
        /// <summary>2</summary>
        public const int FF_MB_DECISION_RD = 0x2;
        /// <summary>0</summary>
        public const int FF_MB_DECISION_SIMPLE = 0x0;
        /// <summary>16384</summary>
        public const int FF_MIN_BUFFER_SIZE = 0x4000;
        /// <summary>0</summary>
        public const int FF_PRED_LEFT = 0x0;
        /// <summary>2</summary>
        public const int FF_PRED_MEDIAN = 0x2;
        /// <summary>1</summary>
        public const int FF_PRED_PLANE = 0x1;
        /// <summary>38</summary>
        public const int FF_PROFILE_AAC_ELD = 0x26;
        /// <summary>4</summary>
        public const int FF_PROFILE_AAC_HE = 0x4;
        /// <summary>28</summary>
        public const int FF_PROFILE_AAC_HE_V2 = 0x1c;
        /// <summary>22</summary>
        public const int FF_PROFILE_AAC_LD = 0x16;
        /// <summary>1</summary>
        public const int FF_PROFILE_AAC_LOW = 0x1;
        /// <summary>3</summary>
        public const int FF_PROFILE_AAC_LTP = 0x3;
        /// <summary>0</summary>
        public const int FF_PROFILE_AAC_MAIN = 0x0;
        /// <summary>2</summary>
        public const int FF_PROFILE_AAC_SSR = 0x2;
        /// <summary>0</summary>
        public const int FF_PROFILE_DNXHD = 0x0;
        /// <summary>5</summary>
        public const int FF_PROFILE_DNXHR_444 = 0x5;
        /// <summary>3</summary>
        public const int FF_PROFILE_DNXHR_HQ = 0x3;
        /// <summary>4</summary>
        public const int FF_PROFILE_DNXHR_HQX = 0x4;
        /// <summary>1</summary>
        public const int FF_PROFILE_DNXHR_LB = 0x1;
        /// <summary>2</summary>
        public const int FF_PROFILE_DNXHR_SQ = 0x2;
        /// <summary>20</summary>
        public const int FF_PROFILE_DTS = 0x14;
        /// <summary>40</summary>
        public const int FF_PROFILE_DTS_96_24 = 0x28;
        /// <summary>30</summary>
        public const int FF_PROFILE_DTS_ES = 0x1e;
        /// <summary>70</summary>
        public const int FF_PROFILE_DTS_EXPRESS = 0x46;
        /// <summary>50</summary>
        public const int FF_PROFILE_DTS_HD_HRA = 0x32;
        /// <summary>60</summary>
        public const int FF_PROFILE_DTS_HD_MA = 0x3c;
        /// <summary>66</summary>
        public const int FF_PROFILE_H264_BASELINE = 0x42;
        /// <summary>44</summary>
        public const int FF_PROFILE_H264_CAVLC_444 = 0x2c;
        /// <summary>(1&lt;&lt;9)</summary>
        public const int FF_PROFILE_H264_CONSTRAINED = 0x1 << 0x9;
        /// <summary>(66|FF_PROFILE_H264_CONSTRAINED)</summary>
        public const int FF_PROFILE_H264_CONSTRAINED_BASELINE = 0x42 | FF_PROFILE_H264_CONSTRAINED;
        /// <summary>88</summary>
        public const int FF_PROFILE_H264_EXTENDED = 0x58;
        /// <summary>100</summary>
        public const int FF_PROFILE_H264_HIGH = 0x64;
        /// <summary>110</summary>
        public const int FF_PROFILE_H264_HIGH_10 = 0x6e;
        /// <summary>(110|FF_PROFILE_H264_INTRA)</summary>
        public const int FF_PROFILE_H264_HIGH_10_INTRA = 0x6e | FF_PROFILE_H264_INTRA;
        /// <summary>122</summary>
        public const int FF_PROFILE_H264_HIGH_422 = 0x7a;
        /// <summary>(122|FF_PROFILE_H264_INTRA)</summary>
        public const int FF_PROFILE_H264_HIGH_422_INTRA = 0x7a | FF_PROFILE_H264_INTRA;
        /// <summary>144</summary>
        public const int FF_PROFILE_H264_HIGH_444 = 0x90;
        /// <summary>(244|FF_PROFILE_H264_INTRA)</summary>
        public const int FF_PROFILE_H264_HIGH_444_INTRA = 0xf4 | FF_PROFILE_H264_INTRA;
        /// <summary>244</summary>
        public const int FF_PROFILE_H264_HIGH_444_PREDICTIVE = 0xf4;
        /// <summary>(1&lt;&lt;11)</summary>
        public const int FF_PROFILE_H264_INTRA = 0x1 << 0xb;
        /// <summary>77</summary>
        public const int FF_PROFILE_H264_MAIN = 0x4d;
        /// <summary>118</summary>
        public const int FF_PROFILE_H264_MULTIVIEW_HIGH = 0x76;
        /// <summary>128</summary>
        public const int FF_PROFILE_H264_STEREO_HIGH = 0x80;
        /// <summary>1</summary>
        public const int FF_PROFILE_HEVC_MAIN = 0x1;
        /// <summary>2</summary>
        public const int FF_PROFILE_HEVC_MAIN_10 = 0x2;
        /// <summary>3</summary>
        public const int FF_PROFILE_HEVC_MAIN_STILL_PICTURE = 0x3;
        /// <summary>4</summary>
        public const int FF_PROFILE_HEVC_REXT = 0x4;
        /// <summary>32768</summary>
        public const int FF_PROFILE_JPEG2000_CSTREAM_NO_RESTRICTION = 0x8000;
        /// <summary>1</summary>
        public const int FF_PROFILE_JPEG2000_CSTREAM_RESTRICTION_0 = 0x1;
        /// <summary>2</summary>
        public const int FF_PROFILE_JPEG2000_CSTREAM_RESTRICTION_1 = 0x2;
        /// <summary>3</summary>
        public const int FF_PROFILE_JPEG2000_DCINEMA_2K = 0x3;
        /// <summary>4</summary>
        public const int FF_PROFILE_JPEG2000_DCINEMA_4K = 0x4;
        /// <summary>0</summary>
        public const int FF_PROFILE_MPEG2_422 = 0x0;
        /// <summary>131</summary>
        public const int FF_PROFILE_MPEG2_AAC_HE = 0x83;
        /// <summary>128</summary>
        public const int FF_PROFILE_MPEG2_AAC_LOW = 0x80;
        /// <summary>1</summary>
        public const int FF_PROFILE_MPEG2_HIGH = 0x1;
        /// <summary>4</summary>
        public const int FF_PROFILE_MPEG2_MAIN = 0x4;
        /// <summary>5</summary>
        public const int FF_PROFILE_MPEG2_SIMPLE = 0x5;
        /// <summary>3</summary>
        public const int FF_PROFILE_MPEG2_SNR_SCALABLE = 0x3;
        /// <summary>2</summary>
        public const int FF_PROFILE_MPEG2_SS = 0x2;
        /// <summary>11</summary>
        public const int FF_PROFILE_MPEG4_ADVANCED_CODING = 0xb;
        /// <summary>12</summary>
        public const int FF_PROFILE_MPEG4_ADVANCED_CORE = 0xc;
        /// <summary>9</summary>
        public const int FF_PROFILE_MPEG4_ADVANCED_REAL_TIME = 0x9;
        /// <summary>13</summary>
        public const int FF_PROFILE_MPEG4_ADVANCED_SCALABLE_TEXTURE = 0xd;
        /// <summary>15</summary>
        public const int FF_PROFILE_MPEG4_ADVANCED_SIMPLE = 0xf;
        /// <summary>7</summary>
        public const int FF_PROFILE_MPEG4_BASIC_ANIMATED_TEXTURE = 0x7;
        /// <summary>2</summary>
        public const int FF_PROFILE_MPEG4_CORE = 0x2;
        /// <summary>10</summary>
        public const int FF_PROFILE_MPEG4_CORE_SCALABLE = 0xa;
        /// <summary>8</summary>
        public const int FF_PROFILE_MPEG4_HYBRID = 0x8;
        /// <summary>3</summary>
        public const int FF_PROFILE_MPEG4_MAIN = 0x3;
        /// <summary>4</summary>
        public const int FF_PROFILE_MPEG4_N_BIT = 0x4;
        /// <summary>5</summary>
        public const int FF_PROFILE_MPEG4_SCALABLE_TEXTURE = 0x5;
        /// <summary>0</summary>
        public const int FF_PROFILE_MPEG4_SIMPLE = 0x0;
        /// <summary>6</summary>
        public const int FF_PROFILE_MPEG4_SIMPLE_FACE_ANIMATION = 0x6;
        /// <summary>1</summary>
        public const int FF_PROFILE_MPEG4_SIMPLE_SCALABLE = 0x1;
        /// <summary>14</summary>
        public const int FF_PROFILE_MPEG4_SIMPLE_STUDIO = 0xe;
        /// <summary>-100</summary>
        public const int FF_PROFILE_RESERVED = -0x64;
        /// <summary>-99</summary>
        public const int FF_PROFILE_UNKNOWN = -0x63;
        /// <summary>3</summary>
        public const int FF_PROFILE_VC1_ADVANCED = 0x3;
        /// <summary>2</summary>
        public const int FF_PROFILE_VC1_COMPLEX = 0x2;
        /// <summary>1</summary>
        public const int FF_PROFILE_VC1_MAIN = 0x1;
        /// <summary>0</summary>
        public const int FF_PROFILE_VC1_SIMPLE = 0x0;
        /// <summary>0</summary>
        public const int FF_PROFILE_VP9_0 = 0x0;
        /// <summary>1</summary>
        public const int FF_PROFILE_VP9_1 = 0x1;
        /// <summary>2</summary>
        public const int FF_PROFILE_VP9_2 = 0x2;
        /// <summary>3</summary>
        public const int FF_PROFILE_VP9_3 = 0x3;
        /// <summary>118</summary>
        public const int FF_QP2LAMBDA = 0x76;
        /// <summary>2</summary>
        public const int FF_QSCALE_TYPE_H264 = 0x2;
        /// <summary>0</summary>
        public const int FF_QSCALE_TYPE_MPEG1 = 0x0;
        /// <summary>1</summary>
        public const int FF_QSCALE_TYPE_MPEG2 = 0x1;
        /// <summary>3</summary>
        public const int FF_QSCALE_TYPE_VP56 = 0x3;
        /// <summary>FF_LAMBDA_SCALE</summary>
        public const int FF_QUALITY_SCALE = FF_LAMBDA_SCALE;
        /// <summary>1</summary>
        public const int FF_RC_STRATEGY_XVID = 0x1;
        /// <summary>0</summary>
        public const int FF_SUB_CHARENC_MODE_AUTOMATIC = 0x0;
        /// <summary>-1</summary>
        public const int FF_SUB_CHARENC_MODE_DO_NOTHING = -0x1;
        /// <summary>1</summary>
        public const int FF_SUB_CHARENC_MODE_PRE_DECODER = 0x1;
        /// <summary>0</summary>
        public const int FF_SUB_TEXT_FMT_ASS = 0x0;
        /// <summary>1</summary>
        public const int FF_SUB_TEXT_FMT_ASS_WITH_TIMINGS = 0x1;
        /// <summary>1</summary>
        public const int FF_THREAD_FRAME = 0x1;
        /// <summary>2</summary>
        public const int FF_THREAD_SLICE = 0x2;
        /// <summary>0x0200</summary>
        public const int HWACCEL_CODEC_CAP_EXPERIMENTAL = 0x200;
        // public static LIBAVCODEC_BUILD = LIBAVCODEC_VERSION_INT;
        /// <summary>&quot;Lavc&quot; AV_STRINGIFY(LIBAVCODEC_VERSION)</summary>
        public const string LIBAVCODEC_IDENT = "Lavc";
        // public static LIBAVCODEC_VERSION = AV_VERSION(LIBAVCODEC_VERSION_MAJOR,    LIBAVCODEC_VERSION_MINOR,    LIBAVCODEC_VERSION_MICRO);
        // public static LIBAVCODEC_VERSION_INT = AV_VERSION_INT(LIBAVCODEC_VERSION_MAJOR, LIBAVCODEC_VERSION_MINOR, LIBAVCODEC_VERSION_MICRO);
        /// <summary>57</summary>
        public const int LIBAVCODEC_VERSION_MAJOR = 0x39;
        /// <summary>100</summary>
        public const int LIBAVCODEC_VERSION_MICRO = 0x64;
        /// <summary>89</summary>
        public const int LIBAVCODEC_VERSION_MINOR = 0x59;
        // public static LIBAVDEVICE_BUILD = LIBAVDEVICE_VERSION_INT;
        /// <summary>&quot;Lavd&quot; AV_STRINGIFY(LIBAVDEVICE_VERSION)</summary>
        public const string LIBAVDEVICE_IDENT = "Lavd";
        // public static LIBAVDEVICE_VERSION = AV_VERSION(LIBAVDEVICE_VERSION_MAJOR, LIBAVDEVICE_VERSION_MINOR, LIBAVDEVICE_VERSION_MICRO);
        // public static LIBAVDEVICE_VERSION_INT = AV_VERSION_INT(LIBAVDEVICE_VERSION_MAJOR, LIBAVDEVICE_VERSION_MINOR, LIBAVDEVICE_VERSION_MICRO);
        /// <summary>57</summary>
        public const int LIBAVDEVICE_VERSION_MAJOR = 0x39;
        /// <summary>100</summary>
        public const int LIBAVDEVICE_VERSION_MICRO = 0x64;
        /// <summary>6</summary>
        public const int LIBAVDEVICE_VERSION_MINOR = 0x6;
        // public static LIBAVFILTER_BUILD = LIBAVFILTER_VERSION_INT;
        /// <summary>&quot;Lavfi&quot;</summary>
        public const string LIBAVFILTER_IDENT = "Lavfi";
        // public static LIBAVFILTER_VERSION = AV_VERSION(LIBAVFILTER_VERSION_MAJOR,   LIBAVFILTER_VERSION_MINOR,   LIBAVFILTER_VERSION_MICRO);
        // public static LIBAVFILTER_VERSION_INT = AV_VERSION_INT(LIBAVFILTER_VERSION_MAJOR, LIBAVFILTER_VERSION_MINOR, LIBAVFILTER_VERSION_MICRO);
        /// <summary>0x6</summary>
        public const int LIBAVFILTER_VERSION_MAJOR = 0x6;
        /// <summary>0x64</summary>
        public const int LIBAVFILTER_VERSION_MICRO = 0x64;
        /// <summary>0x52</summary>
        public const int LIBAVFILTER_VERSION_MINOR = 0x52;
        // public static LIBAVFORMAT_BUILD = LIBAVFORMAT_VERSION_INT;
        /// <summary>&quot;Lavf&quot; AV_STRINGIFY(LIBAVFORMAT_VERSION)</summary>
        public const string LIBAVFORMAT_IDENT = "Lavf";
        // public static LIBAVFORMAT_VERSION = AV_VERSION(LIBAVFORMAT_VERSION_MAJOR,   LIBAVFORMAT_VERSION_MINOR,   LIBAVFORMAT_VERSION_MICRO);
        // public static LIBAVFORMAT_VERSION_INT = AV_VERSION_INT(LIBAVFORMAT_VERSION_MAJOR, LIBAVFORMAT_VERSION_MINOR, LIBAVFORMAT_VERSION_MICRO);
        /// <summary>57</summary>
        public const int LIBAVFORMAT_VERSION_MAJOR = 0x39;
        /// <summary>100</summary>
        public const int LIBAVFORMAT_VERSION_MICRO = 0x64;
        /// <summary>71</summary>
        public const int LIBAVFORMAT_VERSION_MINOR = 0x47;
        // public static LIBAVUTIL_BUILD = LIBAVUTIL_VERSION_INT;
        /// <summary>&quot;Lavu&quot; AV_STRINGIFY(LIBAVUTIL_VERSION)</summary>
        public const string LIBAVUTIL_IDENT = "Lavu";
        // public static LIBAVUTIL_VERSION = AV_VERSION(LIBAVUTIL_VERSION_MAJOR,     LIBAVUTIL_VERSION_MINOR,     LIBAVUTIL_VERSION_MICRO);
        // public static LIBAVUTIL_VERSION_INT = AV_VERSION_INT(LIBAVUTIL_VERSION_MAJOR, LIBAVUTIL_VERSION_MINOR, LIBAVUTIL_VERSION_MICRO);
        /// <summary>55</summary>
        public const int LIBAVUTIL_VERSION_MAJOR = 0x37;
        /// <summary>100</summary>
        public const int LIBAVUTIL_VERSION_MICRO = 0x64;
        /// <summary>58</summary>
        public const int LIBAVUTIL_VERSION_MINOR = 0x3a;
        // public static LIBPOSTPROC_BUILD = LIBPOSTPROC_VERSION_INT;
        /// <summary>&quot;postproc&quot;</summary>
        public const string LIBPOSTPROC_IDENT = "postproc";
        // public static LIBPOSTPROC_VERSION = AV_VERSION(LIBPOSTPROC_VERSION_MAJOR, LIBPOSTPROC_VERSION_MINOR, LIBPOSTPROC_VERSION_MICRO);
        // public static LIBPOSTPROC_VERSION_INT = AV_VERSION_INT(LIBPOSTPROC_VERSION_MAJOR, LIBPOSTPROC_VERSION_MINOR, LIBPOSTPROC_VERSION_MICRO);
        /// <summary>0x36</summary>
        public const int LIBPOSTPROC_VERSION_MAJOR = 0x36;
        /// <summary>0x64</summary>
        public const int LIBPOSTPROC_VERSION_MICRO = 0x64;
        /// <summary>0x5</summary>
        public const int LIBPOSTPROC_VERSION_MINOR = 0x5;
        // public static LIBSWRESAMPLE_BUILD = LIBSWRESAMPLE_VERSION_INT;
        /// <summary>&quot;SwR&quot;</summary>
        public const string LIBSWRESAMPLE_IDENT = "SwR";
        // public static LIBSWRESAMPLE_VERSION = AV_VERSION(LIBSWRESAMPLE_VERSION_MAJOR, LIBSWRESAMPLE_VERSION_MINOR, LIBSWRESAMPLE_VERSION_MICRO);
        // public static LIBSWRESAMPLE_VERSION_INT = AV_VERSION_INT(LIBSWRESAMPLE_VERSION_MAJOR, LIBSWRESAMPLE_VERSION_MINOR, LIBSWRESAMPLE_VERSION_MICRO);
        /// <summary>0x2</summary>
        public const int LIBSWRESAMPLE_VERSION_MAJOR = 0x2;
        /// <summary>0x64</summary>
        public const int LIBSWRESAMPLE_VERSION_MICRO = 0x64;
        /// <summary>0x7</summary>
        public const int LIBSWRESAMPLE_VERSION_MINOR = 0x7;
        // public static LIBSWSCALE_BUILD = LIBSWSCALE_VERSION_INT;
        /// <summary>&quot;SwS&quot;</summary>
        public const string LIBSWSCALE_IDENT = "SwS";
        // public static LIBSWSCALE_VERSION = AV_VERSION(LIBSWSCALE_VERSION_MAJOR, LIBSWSCALE_VERSION_MINOR, LIBSWSCALE_VERSION_MICRO);
        // public static LIBSWSCALE_VERSION_INT = AV_VERSION_INT(LIBSWSCALE_VERSION_MAJOR, LIBSWSCALE_VERSION_MINOR, LIBSWSCALE_VERSION_MICRO);
        /// <summary>0x4</summary>
        public const int LIBSWSCALE_VERSION_MAJOR = 0x4;
        /// <summary>0x64</summary>
        public const int LIBSWSCALE_VERSION_MICRO = 0x64;
        /// <summary>0x6</summary>
        public const int LIBSWSCALE_VERSION_MINOR = 0x6;
        /// <summary>2.7182818284590452354</summary>
        public const double M_E = 2.71828182845905d;
        /// <summary>2.30258509299404568402</summary>
        public const double M_LN10 = 2.30258509299405d;
        /// <summary>0.69314718055994530942</summary>
        public const double M_LN2 = 0.693147180559945d;
        /// <summary>3.32192809488736234787</summary>
        public const double M_LOG2_10 = 3.32192809488736d;
        /// <summary>1.61803398874989484820</summary>
        public const double M_PHI = 1.61803398874989d;
        /// <summary>3.14159265358979323846</summary>
        public const double M_PI = 3.14159265358979d;
        /// <summary>1.57079632679489661923</summary>
        public const double M_PI_2 = 1.5707963267949d;
        /// <summary>0.70710678118654752440</summary>
        public const double M_SQRT1_2 = 0.707106781186548d;
        /// <summary>1.41421356237309504880</summary>
        public const double M_SQRT2 = 1.4142135623731d;
        /// <summary>16</summary>
        public const int MAX_REORDER_DELAY = 0x10;
        /// <summary>(30*12+30+3+6)</summary>
        public const int MAX_STD_TIMEBASES = 0x1e * 0xc + 0x1e + 0x3 + 0x6;
        /// <summary>0x0008</summary>
        public const int MB_TYPE_16x16 = 0x8;
        /// <summary>0x0010</summary>
        public const int MB_TYPE_16x8 = 0x10;
        /// <summary>0x0020</summary>
        public const int MB_TYPE_8x16 = 0x20;
        /// <summary>0x0040</summary>
        public const int MB_TYPE_8x8 = 0x40;
        /// <summary>0x0200</summary>
        public const int MB_TYPE_ACPRED = 0x200;
        /// <summary>0x00020000</summary>
        public const int MB_TYPE_CBP = 0x20000;
        /// <summary>0x0100</summary>
        public const int MB_TYPE_DIRECT2 = 0x100;
        /// <summary>0x0400</summary>
        public const int MB_TYPE_GMC = 0x400;
        /// <summary>0x0080</summary>
        public const int MB_TYPE_INTERLACED = 0x80;
        /// <summary>0x0004</summary>
        public const int MB_TYPE_INTRA_PCM = 0x4;
        /// <summary>0x0002</summary>
        public const int MB_TYPE_INTRA16x16 = 0x2;
        /// <summary>0x0001</summary>
        public const int MB_TYPE_INTRA4x4 = 0x1;
        /// <summary>(MB_TYPE_P0L0 | MB_TYPE_P1L0)</summary>
        public const int MB_TYPE_L0 = MB_TYPE_P0L0 | MB_TYPE_P1L0;
        /// <summary>(MB_TYPE_L0   | MB_TYPE_L1)</summary>
        public const int MB_TYPE_L0L1 = MB_TYPE_L0 | MB_TYPE_L1;
        /// <summary>(MB_TYPE_P0L1 | MB_TYPE_P1L1)</summary>
        public const int MB_TYPE_L1 = MB_TYPE_P0L1 | MB_TYPE_P1L1;
        /// <summary>0x1000</summary>
        public const int MB_TYPE_P0L0 = 0x1000;
        /// <summary>0x4000</summary>
        public const int MB_TYPE_P0L1 = 0x4000;
        /// <summary>0x2000</summary>
        public const int MB_TYPE_P1L0 = 0x2000;
        /// <summary>0x8000</summary>
        public const int MB_TYPE_P1L1 = 0x8000;
        /// <summary>0x00010000</summary>
        public const int MB_TYPE_QUANT = 0x10000;
        /// <summary>0x0800</summary>
        public const int MB_TYPE_SKIP = 0x800;
        /// <summary>0x0001</summary>
        public const int PARSER_FLAG_COMPLETE_FRAMES = 0x1;
        /// <summary>0x0004</summary>
        public const int PARSER_FLAG_FETCHED_OFFSET = 0x4;
        /// <summary>0x0002</summary>
        public const int PARSER_FLAG_ONCE = 0x2;
        /// <summary>0x1000</summary>
        public const int PARSER_FLAG_USE_CODEC_TS = 0x1000;
        /// <summary>0x40000000</summary>
        public const int PP_CPU_CAPS_3DNOW = 0x40000000;
        /// <summary>0x10000000</summary>
        public const int PP_CPU_CAPS_ALTIVEC = 0x10000000;
        /// <summary>0x80000</summary>
        public const int PP_CPU_CAPS_AUTO = 0x80000;
        /// <summary>0x80000000u</summary>
        public const uint PP_CPU_CAPS_MMX = 0x80000000u;
        /// <summary>0x20000000</summary>
        public const int PP_CPU_CAPS_MMX2 = 0x20000000;
        /// <summary>0x8</summary>
        public const int PP_FORMAT = 0x8;
        /// <summary>0x2 | PP_FORMAT</summary>
        public const int PP_FORMAT_411 = 0x2 | PP_FORMAT;
        /// <summary>0x11 | PP_FORMAT</summary>
        public const int PP_FORMAT_420 = 0x11 | PP_FORMAT;
        /// <summary>0x1 | PP_FORMAT</summary>
        public const int PP_FORMAT_422 = 0x1 | PP_FORMAT;
        /// <summary>0x10 | PP_FORMAT</summary>
        public const int PP_FORMAT_440 = 0x10 | PP_FORMAT;
        /// <summary>0x0 | PP_FORMAT</summary>
        public const int PP_FORMAT_444 = 0x0 | PP_FORMAT;
        /// <summary>0x10</summary>
        public const int PP_PICT_TYPE_QP2 = 0x10;
        /// <summary>0x6</summary>
        public const int PP_QUALITY_MAX = 0x6;
        // public static QP_STORE_T = int8_t;
        /// <summary>0x0002</summary>
        public const int SLICE_FLAG_ALLOW_FIELD = 0x2;
        /// <summary>0x0004</summary>
        public const int SLICE_FLAG_ALLOW_PLANE = 0x4;
        /// <summary>0x0001</summary>
        public const int SLICE_FLAG_CODED_ORDER = 0x1;
        /// <summary>0x1</summary>
        public const int SWR_FLAG_RESAMPLE = 0x1;
        /// <summary>0x40000</summary>
        public const int SWS_ACCURATE_RND = 0x40000;
        /// <summary>0x20</summary>
        public const int SWS_AREA = 0x20;
        /// <summary>0x4</summary>
        public const int SWS_BICUBIC = 0x4;
        /// <summary>0x40</summary>
        public const int SWS_BICUBLIN = 0x40;
        /// <summary>0x2</summary>
        public const int SWS_BILINEAR = 0x2;
        /// <summary>0x80000</summary>
        public const int SWS_BITEXACT = 0x80000;
        /// <summary>0x9</summary>
        public const int SWS_CS_BT2020 = 0x9;
        /// <summary>0x5</summary>
        public const int SWS_CS_DEFAULT = 0x5;
        /// <summary>0x4</summary>
        public const int SWS_CS_FCC = 0x4;
        /// <summary>0x5</summary>
        public const int SWS_CS_ITU601 = 0x5;
        /// <summary>0x5</summary>
        public const int SWS_CS_ITU624 = 0x5;
        /// <summary>0x1</summary>
        public const int SWS_CS_ITU709 = 0x1;
        /// <summary>0x5</summary>
        public const int SWS_CS_SMPTE170M = 0x5;
        /// <summary>0x7</summary>
        public const int SWS_CS_SMPTE240M = 0x7;
        /// <summary>0x8000</summary>
        public const int SWS_DIRECT_BGR = 0x8000;
        /// <summary>0x800000</summary>
        public const int SWS_ERROR_DIFFUSION = 0x800000;
        /// <summary>0x1</summary>
        public const int SWS_FAST_BILINEAR = 0x1;
        /// <summary>0x4000</summary>
        public const int SWS_FULL_CHR_H_INP = 0x4000;
        /// <summary>0x2000</summary>
        public const int SWS_FULL_CHR_H_INT = 0x2000;
        /// <summary>0x80</summary>
        public const int SWS_GAUSS = 0x80;
        /// <summary>0x200</summary>
        public const int SWS_LANCZOS = 0x200;
        /// <summary>0.002d</summary>
        public const double SWS_MAX_REDUCE_CUTOFF = 0.002d;
        /// <summary>0x1e240</summary>
        public const int SWS_PARAM_DEFAULT = 0x1e240;
        /// <summary>0x10</summary>
        public const int SWS_POINT = 0x10;
        /// <summary>0x1000</summary>
        public const int SWS_PRINT_INFO = 0x1000;
        /// <summary>0x100</summary>
        public const int SWS_SINC = 0x100;
        /// <summary>0x400</summary>
        public const int SWS_SPLINE = 0x400;
        /// <summary>0x30000</summary>
        public const int SWS_SRC_V_CHR_DROP_MASK = 0x30000;
        /// <summary>0x10</summary>
        public const int SWS_SRC_V_CHR_DROP_SHIFT = 0x10;
        /// <summary>0x8</summary>
        public const int SWS_X = 0x8;
    }
}
