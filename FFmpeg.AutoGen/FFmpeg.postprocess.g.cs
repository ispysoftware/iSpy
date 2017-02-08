using System;
using System.Runtime.InteropServices;

namespace FFmpeg.AutoGen
{
    public unsafe static partial class ffmpeg
    {
        public const int LIBPOSTPROC_VERSION_MAJOR = 54;
        public const int LIBPOSTPROC_VERSION_MINOR = 1;
        public const int LIBPOSTPROC_VERSION_MICRO = 100;
        public const bool FF_API_QP_TYPE = (LIBPOSTPROC_VERSION_MAJOR<55);
        public const int PP_QUALITY_MAX = 6;
        public const uint PP_CPU_CAPS_MMX = 0x80000000;
        public const int PP_CPU_CAPS_MMX2 = 0x20000000;
        public const int PP_CPU_CAPS_3DNOW = 0x40000000;
        public const int PP_CPU_CAPS_ALTIVEC = 0x10000000;
        public const int PP_CPU_CAPS_AUTO = 0x00080000;
        public const int PP_FORMAT = 0x00000008;
        public const int PP_FORMAT_420 = (0x00000011|PP_FORMAT);
        public const int PP_FORMAT_422 = (0x00000001|PP_FORMAT);
        public const int PP_FORMAT_411 = (0x00000002|PP_FORMAT);
        public const int PP_FORMAT_444 = (0x00000000|PP_FORMAT);
        public const int PP_FORMAT_440 = (0x00000010|PP_FORMAT);
        public const int PP_PICT_TYPE_QP2 = 0x00000010;
        private const string libpostproc = "postproc-54";
        
        [DllImport(libpostproc, EntryPoint = "postproc_version", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern uint postproc_version();
        
        [DllImport(libpostproc, EntryPoint = "postproc_configuration", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        [return: MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof(ConstCharPtrMarshaler))]
        public static extern string postproc_configuration();
        
        [DllImport(libpostproc, EntryPoint = "postproc_license", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        [return: MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof(ConstCharPtrMarshaler))]
        public static extern string postproc_license();
        
        [DllImport(libpostproc, EntryPoint = "pp_postprocess", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern void pp_postprocess([MarshalAs(UnmanagedType.LPArray, SizeConst=3)] sbyte*[] @src, [MarshalAs(UnmanagedType.LPArray, SizeConst=3)] int[] @srcStride, [MarshalAs(UnmanagedType.LPArray, SizeConst=3)] sbyte*[] @dst, [MarshalAs(UnmanagedType.LPArray, SizeConst=3)] int[] @dstStride, int @horizontalSize, int @verticalSize, sbyte* @QP_store, int @QP_stride, void* @mode, void* @ppContext, int @pict_type);
        
        [DllImport(libpostproc, EntryPoint = "pp_get_mode_by_name_and_quality", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern void* pp_get_mode_by_name_and_quality([MarshalAs(UnmanagedType.LPStr)] string @name, int @quality);
        
        [DllImport(libpostproc, EntryPoint = "pp_free_mode", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern void pp_free_mode(void* @mode);
        
        [DllImport(libpostproc, EntryPoint = "pp_get_context", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern void* pp_get_context(int @width, int @height, int @flags);
        
        [DllImport(libpostproc, EntryPoint = "pp_free_context", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern void pp_free_context(void* @ppContext);
        
    }
}
