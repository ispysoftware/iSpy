<?xml version="1.0"?><doc>
<members>
<member name="F:iSpy.Video.FFMPEG.AudioCodec.MP3" decl="false" source="d:\projects\ispy\ispy\ffmpeg\audiocodec.h" line="21">
<summary>
MPEG-3
</summary>
</member>
<member name="T:iSpy.Video.FFMPEG.AudioCodec" decl="false" source="d:\projects\ispy\ispy\ffmpeg\audiocodec.h" line="15">
<summary>
Enumeration of some audio codecs from FFmpeg library, which are available for writing audio files.
</summary>
</member>
<member name="M:libffmpeg.__local_stdio_printf_options" decl="false" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavcodec\avcodec.h" line="24">
@file
@ingroup libavc
Libavcodec external API header

</member>
<member name="M:libffmpeg.avutil_version" decl="true" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavutil\avutil.h" line="24">
@file
external API header

 @mainpage

 @section ffmpeg_intro Introduction

 This document describes the usage of the different libraries
 provided by FFmpeg.

 @li @ref libavc "libavcodec" encoding/decoding library
 @li @ref lavfi "libavfilter" graph-based frame editing library
 @li @ref libavf "libavformat" I/O and muxing/demuxing library
 @li @ref lavd "libavdevice" special devices muxing/demuxing library
 @li @ref lavu "libavutil" common utility library
 @li @ref lswr "libswresample" audio resampling, format conversion and mixing
 @li @ref lpp  "libpostproc" post processing library
 @li @ref libsws "libswscale" color conversion and scaling library

 @section ffmpeg_versioning Versioning and compatibility

 Each of the FFmpeg libraries contains a version.h header, which defines a
 major, minor and micro version number with the
 <em>LIBRARYNAME_VERSION_{MAJOR,MINOR,MICRO}</em> macros. The major version
 number is incremented with backward incompatible changes - e.g. removing
 parts of the public API, reordering public struct members, etc. The minor
 version number is incremented for backward compatible API changes or major
 new features - e.g. adding a new public function or a new decoder. The micro
 version number is incremented for smaller changes that a calling program
 might still want to check for - e.g. changing behavior in a previously
 unspecified situation.

 FFmpeg guarantees backward API and ABI compatibility for each library as long
 as its major version number is unchanged. This means that no public symbols
 will be removed or renamed. Types and names of the public struct members and
 values of public macros and enums will remain the same (unless they were
 explicitly declared as not part of the public API). Documented behavior will
 not change.

 In other words, any correct program that works with a given FFmpeg snapshot
 should work just as well without any changes with any later snapshot with the
 same major versions. This applies to both rebuilding the program against new
 FFmpeg versions or to replacing the dynamic FFmpeg libraries that a program
 links against.

 However, new public symbols may be added and new members may be appended to
 public structs whose size is not part of public ABI (most public structs in
 FFmpeg). New macros and enum values may be added. Behavior in undocumented
 situations may change slightly (and be documented). All those are accompanied
 by an entry in doc/APIchanges and incrementing either the minor or micro
 version number.

 @defgroup lavu Common utility functions

 @brief
 libavutil contains the code shared across all the other FFmpeg
 libraries

 @note In order to use the functions provided by avutil you must include
 the specific header.

 @{

 @defgroup lavu_crypto Crypto and Hashing

 @{
 @}

 @defgroup lavu_math Maths
 @{

 @}

 @defgroup lavu_string String Manipulation

 @{

 @}

 @defgroup lavu_mem Memory Management

 @{

 @}

 @defgroup lavu_data Data Structures
 @{

 @}

 @defgroup lavu_audio Audio related

 @{

 @}

 @defgroup lavu_error Error Codes

 @{

 @}

 @defgroup lavu_log Logging Facility

 @{

 @}

 @defgroup lavu_misc Other

 @{

 @defgroup lavu_internal Internal

 Not exported functions, for internal usage only

 @{

 @}

 @defgroup preproc_misc Preprocessor String Macros

 @{

 @}

 @defgroup version_utils Library Version Macros

 @{

 @}

@addtogroup lavu_ver
@{

Return the LIBAVUTIL_VERSION_INT constant.

</member>
<member name="M:libffmpeg.avutil_configuration" decl="true" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavutil\avutil.h" line="173">
Return the libavutil build-time configuration.

</member>
<member name="M:libffmpeg.avutil_license" decl="true" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavutil\avutil.h" line="178">
Return the libavutil license.

</member>
<member name="T:libffmpeg.AVMediaType" decl="false" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavutil\avutil.h" line="183">
@}

@addtogroup lavu_media Media Type
@brief Media Type

</member>
<member name="M:libffmpeg.av_get_media_type_string(libffmpeg.AVMediaType)" decl="true" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavutil\avutil.h" line="202">
Return a string describing the media_type enum, NULL if media_type
is unknown.

</member>
<member name="T:libffmpeg.AVPictureType" decl="false" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavutil\avutil.h" line="208">
 @defgroup lavu_const Constants
 @{

 @defgroup lavu_enc Encoding specific

 @note those definition should move to avcodec
 @{

 @}
 @defgroup lavu_time Timestamp specific

 FFmpeg internal timebase and timestamp definitions

 @{

 @brief Undefined timestamp value

 Usually reported by demuxer that work on containers that do not provide
 either pts or dts.

Internal time base represented as integer

Internal time base represented as fractional value

 @}
 @}
 @defgroup lavu_picture Image related

 AVPicture types, pixel formats and basic image planes manipulation.

 @{

</member>
<member name="M:libffmpeg.av_get_picture_type_char(libffmpeg.AVPictureType)" decl="true" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavutil\avutil.h" line="276">
 Return a single letter to describe the given picture type
 pict_type.

 @param[in] pict_type the picture type @return a single character
 representing the picture type, '?' if pict_type is unknown

</member>
<member name="F:libffmpeg.av_reverse" decl="false" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavutil\attributes.h" line="21">
@file
Macro definitions for various function/variable attributes

Disable warnings about deprecated features
This is useful for sections of code kept for backward compatibility and
scheduled for removal.

Mark a variable as used and prevent the compiler from optimizing it
away.  This is useful for variables accessed only from inline
assembler without the compiler being aware.

@file
@ingroup lavu
Utility Preprocessor macros

 @addtogroup preproc_misc Preprocessor String Macros

 String manipulation macros

 @{

@}

 @addtogroup version_utils

 Useful to check and match library version in order to maintain
 backward compatibility.

 @{

@}

@file
@ingroup lavu
Libavutil version macros

 @defgroup lavu_ver Version and Build diagnostics

 Macros and function useful to check at compiletime and at runtime
 which version of libavutil is in use.

 @{

 @}

 @defgroup depr_guards Deprecation guards
 FF_API_* defines may be placed below to indicate public API that will be
 dropped at a future version bump. The defines themselves are not part of
 the public API and may change, break or disappear at any time.

 @{

@}

Reverse the order of the bits of an 8-bits unsigned integer.

</member>
<member name="M:libffmpeg.av_log2(System.UInt32)" decl="true" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavutil\common.h" line="21">
@file
common internal and external API header

</member>
<member name="M:libffmpeg.av_clip_c(System.Int32,System.Int32,System.Int32)" decl="false" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavutil\common.h" line="98">
Clip a signed integer value into the amin-amax range.
@param a value to clip
@param amin minimum value of the clip range
@param amax maximum value of the clip range
@return clipped value

</member>
<member name="M:libffmpeg.av_clip64_c(System.Int64,System.Int64,System.Int64)" decl="false" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavutil\common.h" line="115">
Clip a signed 64bit integer value into the amin-amax range.
@param a value to clip
@param amin minimum value of the clip range
@param amax maximum value of the clip range
@return clipped value

</member>
<member name="M:libffmpeg.av_clip_uint8_c(System.Int32)" decl="false" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavutil\common.h" line="132">
Clip a signed integer value into the 0-255 range.
@param a value to clip
@return clipped value

</member>
<member name="M:libffmpeg.av_clip_int8_c(System.Int32)" decl="false" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavutil\common.h" line="143">
Clip a signed integer value into the -128,127 range.
@param a value to clip
@return clipped value

</member>
<member name="M:libffmpeg.av_clip_uint16_c(System.Int32)" decl="false" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavutil\common.h" line="154">
Clip a signed integer value into the 0-65535 range.
@param a value to clip
@return clipped value

</member>
<member name="M:libffmpeg.av_clip_int16_c(System.Int32)" decl="false" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavutil\common.h" line="165">
Clip a signed integer value into the -32768,32767 range.
@param a value to clip
@return clipped value

</member>
<member name="M:libffmpeg.av_clipl_int32_c(System.Int64)" decl="false" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavutil\common.h" line="176">
Clip a signed 64-bit integer value into the -2147483648,2147483647 range.
@param a value to clip
@return clipped value

</member>
<member name="M:libffmpeg.av_clip_intp2_c(System.Int32,System.Int32)" decl="false" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavutil\common.h" line="187">
Clip a signed integer into the -(2^p),(2^p-1) range.
@param  a value to clip
@param  p bit position to clip at
@return clipped value

</member>
<member name="M:libffmpeg.av_clip_uintp2_c(System.Int32,System.Int32)" decl="false" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavutil\common.h" line="201">
Clip a signed integer to an unsigned power of two range.
@param  a value to clip
@param  p bit position to clip at
@return clipped value

</member>
<member name="M:libffmpeg.av_sat_add32_c(System.Int32,System.Int32)" decl="false" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavutil\common.h" line="213">
 Add two signed 32-bit values with saturation.

 @param  a one value
 @param  b another value
 @return sum with signed saturation

</member>
<member name="M:libffmpeg.av_sat_dadd32_c(System.Int32,System.Int32)" decl="false" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavutil\common.h" line="225">
 Add a doubled value to another value with saturation at both stages.

 @param  a first value
 @param  b value doubled and added to a
 @return sum with signed saturation

</member>
<member name="M:libffmpeg.av_clipf_c(System.Single,System.Single,System.Single)" decl="false" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavutil\common.h" line="237">
Clip a float value into the amin-amax range.
@param a value to clip
@param amin minimum value of the clip range
@param amax maximum value of the clip range
@return clipped value

</member>
<member name="M:libffmpeg.av_clipd_c(System.Double,System.Double,System.Double)" decl="false" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavutil\common.h" line="254">
Clip a double value into the amin-amax range.
@param a value to clip
@param amin minimum value of the clip range
@param amax maximum value of the clip range
@return clipped value

</member>
<member name="M:libffmpeg.av_ceil_log2_c(System.Int32)" decl="false" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavutil\common.h" line="271">
Compute ceil(log2(x)).
 * @param x value used to compute ceil(log2(x))
 * @return computed ceiling of log2(x)

</member>
<member name="M:libffmpeg.av_popcount_c(System.UInt32)" decl="false" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavutil\common.h" line="280">
Count number of bits set to one in x
@param x value to count bits of
@return the number of bits set to one in x

</member>
<member name="M:libffmpeg.av_popcount64_c(System.UInt64)" decl="false" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavutil\common.h" line="294">
Count number of bits set to one in x
@param x value to count bits of
@return the number of bits set to one in x

</member>
<member name="M:libffmpeg.av_strerror(System.Int32,System.SByte!System.Runtime.CompilerServices.IsSignUnspecifiedByte*,System.UInt32)" decl="true" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavutil\common.h" line="307">
 Convert a UTF-8 character (up to 4 bytes) to its 32-bit UCS-4 encoded form.

 @param val      Output value, must be an lvalue of type uint32_t.
 @param GET_BYTE Expression reading one byte from the input.
                 Evaluated up to 7 times (4 for the currently
                 assigned Unicode range).  With a memory buffer
                 input, this could be *ptr++.
 @param ERROR    Expression to be evaluated on invalid input,
                 typically a goto statement.

 @warning ERROR should not contain a loop control statement which
 could interact with the internal while loop, and should force an
 exit from the macro code (e.g. through a goto or a return) in order
 to prevent undefined results.

 Convert a UTF-16 character (2 or 4 bytes) to its 32-bit UCS-4 encoded form.

 @param val       Output value, must be an lvalue of type uint32_t.
 @param GET_16BIT Expression returning two bytes of UTF-16 data converted
                  to native byte order.  Evaluated one or two times.
 @param ERROR     Expression to be evaluated on invalid input,
                  typically a goto statement.

@def PUT_UTF8(val, tmp, PUT_BYTE)
Convert a 32-bit Unicode character to its UTF-8 encoded form (up to 4 bytes long).
@param val is an input-only argument and should be of type uint32_t. It holds
a UCS-4 encoded Unicode character that is to be converted to UTF-8. If
val is given as a function it is executed only once.
@param tmp is a temporary variable and should be of type uint8_t. It
represents an intermediate value during conversion that is to be
output by PUT_BYTE.
@param PUT_BYTE writes the converted UTF-8 bytes to any proper destination.
It could be a function or a statement, and uses tmp as the input byte.
For example, PUT_BYTE could be "*output++ = tmp;" PUT_BYTE will be
executed up to 4 times for values in the valid UTF-8 range and up to
7 times in the general case, depending on the length of the converted
Unicode character.

@def PUT_UTF16(val, tmp, PUT_16BIT)
Convert a 32-bit Unicode character to its UTF-16 encoded form (2 or 4 bytes).
@param val is an input-only argument and should be of type uint32_t. It holds
a UCS-4 encoded Unicode character that is to be converted to UTF-16. If
val is given as a function it is executed only once.
@param tmp is a temporary variable and should be of type uint16_t. It
represents an intermediate value during conversion that is to be
output by PUT_16BIT.
@param PUT_16BIT writes the converted UTF-16 data to any proper destination
in desired endianness. It could be a function or a statement, and uses tmp
as the input byte.  For example, PUT_BYTE could be "*output++ = tmp;"
PUT_BYTE will be executed 1 or 2 times depending on input character.

@file
memory handling functions

@file
error code definitions

 @addtogroup lavu_error

 @{

This is semantically identical to AVERROR_BUG
it has been introduced in Libav after our AVERROR_BUG and with a modified value.

 Put a description of the AVERROR code errnum in errbuf.
 In case of failure the global variable errno is set to indicate the
 error. Even in case of failure av_strerror() will print a generic
 error message indicating the errnum provided to errbuf.

 @param errnum      error code to describe
 @param errbuf      buffer to which description is written
 @param errbuf_size the size in bytes of errbuf
 @return 0 on success, a negative value if a description for errnum
 cannot be found

</member>
<member name="M:libffmpeg.av_make_error_string(System.SByte!System.Runtime.CompilerServices.IsSignUnspecifiedByte*,System.UInt32,System.Int32)" decl="false" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavutil\error.h" line="99">
 Fill the provided buffer with a string containing an error string
 corresponding to the AVERROR code errnum.

 @param errbuf         a buffer
 @param errbuf_size    size in bytes of errbuf
 @param errnum         error code to describe
 @return the buffer in input, filled with the error description
 @see av_strerror()

</member>
<member name="M:libffmpeg.av_malloc(System.UInt32)" decl="true" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavutil\error.h" line="115">
Convenience macro, the return value should be used only directly in
function arguments but never stand-alone.

@}

@addtogroup lavu_mem
@{

Allocate a block of size bytes with alignment suitable for all
memory accesses (including vectors if available on the CPU).
@param size Size in bytes for the memory block to be allocated.
@return Pointer to the allocated block, NULL if the block cannot
be allocated.
@see av_mallocz()

</member>
<member name="M:libffmpeg.av_malloc_array(System.UInt32,System.UInt32)" decl="false" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavutil\mem.h" line="85">
Allocate a block of size * nmemb bytes with av_malloc().
@param nmemb Number of elements
@param size Size of the single element
@return Pointer to the allocated block, NULL if the block cannot
be allocated.
@see av_malloc()

</member>
<member name="M:libffmpeg.av_realloc(System.Void*,System.UInt32)" decl="true" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavutil\mem.h" line="100">
Allocate or reallocate a block of memory.
If ptr is NULL and size &gt; 0, allocate a new block. If
size is zero, free the memory block pointed to by ptr.
@param ptr Pointer to a memory block already allocated with
av_realloc() or NULL.
@param size Size in bytes of the memory block to be allocated or
reallocated.
@return Pointer to a newly-reallocated block or NULL if the block
cannot be reallocated or the function is used to free the memory block.
@warning Pointers originating from the av_malloc() family of functions must
         not be passed to av_realloc(). The former can be implemented using
         memalign() (or other functions), and there is no guarantee that
         pointers from such functions can be passed to realloc() at all.
         The situation is undefined according to POSIX and may crash with
         some libc implementations.
@see av_fast_realloc()

</member>
<member name="M:libffmpeg.av_realloc_f(System.Void*,System.UInt32,System.UInt32)" decl="true" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavutil\mem.h" line="120">
Allocate or reallocate a block of memory.
This function does the same thing as av_realloc, except:
- It takes two arguments and checks the result of the multiplication for
  integer overflow.
- It frees the input block in case of failure, thus avoiding the memory
  leak with the classic "buf = realloc(buf); if (!buf) return -1;".

</member>
<member name="M:libffmpeg.av_reallocp(System.Void*,System.UInt32)" decl="true" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavutil\mem.h" line="130">
Allocate or reallocate a block of memory.
If *ptr is NULL and size &gt; 0, allocate a new block. If
size is zero, free the memory block pointed to by ptr.
@param   ptr Pointer to a pointer to a memory block already allocated
         with av_realloc(), or pointer to a pointer to NULL.
         The pointer is updated on success, or freed on failure.
@param   size Size in bytes for the memory block to be allocated or
         reallocated
@return  Zero on success, an AVERROR error code on failure.
@warning Pointers originating from the av_malloc() family of functions must
         not be passed to av_reallocp(). The former can be implemented using
         memalign() (or other functions), and there is no guarantee that
         pointers from such functions can be passed to realloc() at all.
         The situation is undefined according to POSIX and may crash with
         some libc implementations.

</member>
<member name="M:libffmpeg.av_realloc_array(System.Void*,System.UInt32,System.UInt32)" decl="true" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavutil\mem.h" line="149">
Allocate or reallocate an array.
If ptr is NULL and nmemb &gt; 0, allocate a new block. If
nmemb is zero, free the memory block pointed to by ptr.
@param ptr Pointer to a memory block already allocated with
av_realloc() or NULL.
@param nmemb Number of elements
@param size Size of the single element
@return Pointer to a newly-reallocated block or NULL if the block
cannot be reallocated or the function is used to free the memory block.
@warning Pointers originating from the av_malloc() family of functions must
         not be passed to av_realloc(). The former can be implemented using
         memalign() (or other functions), and there is no guarantee that
         pointers from such functions can be passed to realloc() at all.
         The situation is undefined according to POSIX and may crash with
         some libc implementations.

</member>
<member name="M:libffmpeg.av_reallocp_array(System.Void*,System.UInt32,System.UInt32)" decl="true" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavutil\mem.h" line="168">
Allocate or reallocate an array through a pointer to a pointer.
If *ptr is NULL and nmemb &gt; 0, allocate a new block. If
nmemb is zero, free the memory block pointed to by ptr.
@param ptr Pointer to a pointer to a memory block already allocated
with av_realloc(), or pointer to a pointer to NULL.
The pointer is updated on success, or freed on failure.
@param nmemb Number of elements
@param size Size of the single element
@return Zero on success, an AVERROR error code on failure.
@warning Pointers originating from the av_malloc() family of functions must
         not be passed to av_realloc(). The former can be implemented using
         memalign() (or other functions), and there is no guarantee that
         pointers from such functions can be passed to realloc() at all.
         The situation is undefined according to POSIX and may crash with
         some libc implementations.

</member>
<member name="M:libffmpeg.av_free(System.Void*)" decl="true" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavutil\mem.h" line="187">
Free a memory block which has been allocated with av_malloc(z)() or
av_realloc().
@param ptr Pointer to the memory block which should be freed.
@note ptr = NULL is explicitly allowed.
@note It is recommended that you use av_freep() instead.
@see av_freep()

</member>
<member name="M:libffmpeg.av_mallocz(System.UInt32)" decl="true" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavutil\mem.h" line="197">
Allocate a block of size bytes with alignment suitable for all
memory accesses (including vectors if available on the CPU) and
zero all the bytes of the block.
@param size Size in bytes for the memory block to be allocated.
@return Pointer to the allocated block, NULL if it cannot be allocated.
@see av_malloc()

</member>
<member name="M:libffmpeg.av_calloc(System.UInt32,System.UInt32)" decl="true" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavutil\mem.h" line="207">
Allocate a block of nmemb * size bytes with alignment suitable for all
memory accesses (including vectors if available on the CPU) and
zero all the bytes of the block.
The allocation will fail if nmemb * size is greater than or equal
to INT_MAX.
@param nmemb
@param size
@return Pointer to the allocated block, NULL if it cannot be allocated.

</member>
<member name="M:libffmpeg.av_mallocz_array(System.UInt32,System.UInt32)" decl="false" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavutil\mem.h" line="219">
Allocate a block of size * nmemb bytes with av_mallocz().
@param nmemb Number of elements
@param size Size of the single element
@return Pointer to the allocated block, NULL if the block cannot
be allocated.
@see av_mallocz()
@see av_malloc_array()

</member>
<member name="M:libffmpeg.av_strdup(System.SByte!System.Runtime.CompilerServices.IsSignUnspecifiedByte!System.Runtime.CompilerServices.IsConst*)" decl="true" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavutil\mem.h" line="235">
Duplicate the string s.
@param s string to be duplicated
@return Pointer to a newly-allocated string containing a
copy of s or NULL if the string cannot be allocated.

</member>
<member name="M:libffmpeg.av_strndup(System.SByte!System.Runtime.CompilerServices.IsSignUnspecifiedByte!System.Runtime.CompilerServices.IsConst*,System.UInt32)" decl="true" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavutil\mem.h" line="243">
Duplicate a substring of the string s.
@param s string to be duplicated
@param len the maximum length of the resulting string (not counting the
           terminating byte).
@return Pointer to a newly-allocated string containing a
copy of s or NULL if the string cannot be allocated.

</member>
<member name="M:libffmpeg.av_memdup(System.Void!System.Runtime.CompilerServices.IsConst*,System.UInt32)" decl="true" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavutil\mem.h" line="253">
Duplicate the buffer p.
@param p buffer to be duplicated
@return Pointer to a newly allocated buffer containing a
copy of p or NULL if the buffer cannot be allocated.

</member>
<member name="M:libffmpeg.av_freep(System.Void*)" decl="true" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavutil\mem.h" line="261">
Free a memory block which has been allocated with av_malloc(z)() or
av_realloc() and set the pointer pointing to it to NULL.
@param ptr Pointer to the pointer to the memory block which should
be freed.
@note passing a pointer to a NULL pointer is safe and leads to no action.
@see av_free()

</member>
<member name="M:libffmpeg.av_dynarray_add(System.Void*,System.Int32*,System.Void*)" decl="true" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavutil\mem.h" line="271">
 Add an element to a dynamic array.

 The array to grow is supposed to be an array of pointers to
 structures, and the element to add must be a pointer to an already
 allocated structure.

 The array is reallocated when its size reaches powers of 2.
 Therefore, the amortized cost of adding an element is constant.

 In case of success, the pointer to the array is updated in order to
 point to the new grown array, and the number pointed to by nb_ptr
 is incremented.
 In case of failure, the array is freed, *tab_ptr is set to NULL and
 *nb_ptr is set to 0.

 @param tab_ptr pointer to the array to grow
 @param nb_ptr  pointer to the number of elements in the array
 @param elem    element to add
 @see av_dynarray_add_nofree(), av_dynarray2_add()

</member>
<member name="M:libffmpeg.av_dynarray_add_nofree(System.Void*,System.Int32*,System.Void*)" decl="true" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavutil\mem.h" line="294">
 Add an element to a dynamic array.

 Function has the same functionality as av_dynarray_add(),
 but it doesn't free memory on fails. It returns error code
 instead and leave current buffer untouched.

 @param tab_ptr pointer to the array to grow
 @param nb_ptr  pointer to the number of elements in the array
 @param elem    element to add
 @return &gt;=0 on success, negative otherwise.
 @see av_dynarray_add(), av_dynarray2_add()

</member>
<member name="M:libffmpeg.av_dynarray2_add(System.Void**,System.Int32*,System.UInt32,System.Byte!System.Runtime.CompilerServices.IsConst*)" decl="true" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavutil\mem.h" line="309">
 Add an element of size elem_size to a dynamic array.

 The array is reallocated when its number of elements reaches powers of 2.
 Therefore, the amortized cost of adding an element is constant.

 In case of success, the pointer to the array is updated in order to
 point to the new grown array, and the number pointed to by nb_ptr
 is incremented.
 In case of failure, the array is freed, *tab_ptr is set to NULL and
 *nb_ptr is set to 0.

 @param tab_ptr   pointer to the array to grow
 @param nb_ptr    pointer to the number of elements in the array
 @param elem_size size in bytes of the elements in the array
 @param elem_data pointer to the data of the element to add. If NULL, the space of
                  the new added element is not filled.
 @return          pointer to the data of the element to copy in the new allocated space.
                  If NULL, the new allocated space is left uninitialized."
 @see av_dynarray_add(), av_dynarray_add_nofree()

</member>
<member name="M:libffmpeg.av_size_mult(System.UInt32,System.UInt32,System.UInt32*)" decl="false" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavutil\mem.h" line="333">
Multiply two size_t values checking for overflow.
@return  0 if success, AVERROR(EINVAL) if overflow.

</member>
<member name="M:libffmpeg.av_max_alloc(System.UInt32)" decl="true" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavutil\mem.h" line="348">
Set the maximum size that may me allocated in one block.

</member>
<member name="M:libffmpeg.av_memcpy_backptr(System.Byte*,System.Int32,System.Int32)" decl="true" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavutil\mem.h" line="353">
 deliberately overlapping memcpy implementation
 @param dst destination buffer
 @param back how many bytes back we start (the initial size of the overlapping window), must be &gt; 0
 @param cnt number of bytes to copy, must be &gt;= 0

 cnt &gt; back is valid, this will copy the bytes we just copied,
 thus creating a repeating pattern with a period length of back.

</member>
<member name="M:libffmpeg.av_fast_realloc(System.Void*,System.UInt32*,System.UInt32)" decl="true" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavutil\mem.h" line="364">
 Reallocate the given block if it is not large enough, otherwise do nothing.

 @see av_realloc

</member>
<member name="M:libffmpeg.av_fast_malloc(System.Void*,System.UInt32*,System.UInt32)" decl="true" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavutil\mem.h" line="371">
 Allocate a buffer, reusing the given one if large enough.

 Contrary to av_fast_realloc the current buffer contents might not be
 preserved and on error the old buffer is freed, thus no special
 handling to avoid memleaks is necessary.

 @param ptr pointer to pointer to already allocated buffer, overwritten with pointer to new buffer
 @param size size of the buffer *ptr points to
 @param min_size minimum size of *ptr buffer after returning, *ptr will be NULL and
                 *size 0 if an error occurred.

</member>
<!-- Discarding badly formed XML document comment for member 'T:libffmpeg.AVRational'. -->
<member name="M:libffmpeg.av_make_q(System.Int32,System.Int32)" decl="false" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavutil\rational.h" line="48">
Create a rational.
Useful for compilers that do not support compound literals.
@note  The return value is not reduced.

</member>
<!-- Discarding badly formed XML document comment for member 'M:libffmpeg.av_cmp_q(libffmpeg.AVRational,libffmpeg.AVRational)'. -->
<member name="M:libffmpeg.av_q2d(libffmpeg.AVRational)" decl="false" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavutil\rational.h" line="75">
Convert rational to double.
@param a rational to convert
@return (double) a

</member>
<!-- Discarding badly formed XML document comment for member 'M:libffmpeg.av_reduce(System.Int32*,System.Int32*,System.Int64,System.Int64,System.Int64)'. -->
<member name="M:libffmpeg.av_mul_q(libffmpeg.AVRational,libffmpeg.AVRational)" decl="true" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavutil\rational.h" line="96">
Multiply two rationals.
@param b first rational
@param c second rational
@return b*c

</member>
<member name="M:libffmpeg.av_div_q(libffmpeg.AVRational,libffmpeg.AVRational)" decl="true" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavutil\rational.h" line="104">
Divide one rational by another.
@param b first rational
@param c second rational
@return b/c

</member>
<member name="M:libffmpeg.av_add_q(libffmpeg.AVRational,libffmpeg.AVRational)" decl="true" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavutil\rational.h" line="112">
Add two rationals.
@param b first rational
@param c second rational
@return b+c

</member>
<member name="M:libffmpeg.av_sub_q(libffmpeg.AVRational,libffmpeg.AVRational)" decl="true" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavutil\rational.h" line="120">
Subtract one rational from another.
@param b first rational
@param c second rational
@return b-c

</member>
<member name="M:libffmpeg.av_inv_q(libffmpeg.AVRational)" decl="false" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavutil\rational.h" line="128">
Invert a rational.
@param q value
@return 1 / q

</member>
<member name="M:libffmpeg.av_d2q(System.Double,System.Int32)" decl="true" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavutil\rational.h" line="139">
 Convert a double precision floating point number to a rational.
 inf is expressed as {1,0} or {-1,0} depending on the sign.

 @param d double to convert
 @param max the maximum allowed numerator and denominator
 @return (AVRational) d

</member>
<member name="M:libffmpeg.av_nearer_q(libffmpeg.AVRational,libffmpeg.AVRational,libffmpeg.AVRational)" decl="true" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavutil\rational.h" line="149">
@return 1 if q1 is nearer to q than q2, -1 if q2 is nearer
than q1, 0 if they have the same distance.

</member>
<member name="M:libffmpeg.av_find_nearest_q_idx(libffmpeg.AVRational,libffmpeg.AVRational!System.Runtime.CompilerServices.IsConst*)" decl="true" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavutil\rational.h" line="155">
Find the nearest value in q_list to q.
@param q_list an array of rationals terminated by {0, 0}
@return the index of the nearest value found in the array

</member>
<member name="T:libffmpeg.av_intfloat32" decl="false" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavutil\rational.h" line="162">
@}

</member>
<member name="M:libffmpeg.av_int2float(System.UInt32)" decl="false" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavutil\intfloat.h" line="37">
Reinterpret a 32-bit integer as a float.

</member>
<member name="M:libffmpeg.av_float2int(System.Single)" decl="false" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavutil\intfloat.h" line="47">
Reinterpret a float as a 32-bit integer.

</member>
<member name="M:libffmpeg.av_int2double(System.UInt64)" decl="false" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavutil\intfloat.h" line="57">
Reinterpret a 64-bit integer as a double.

</member>
<member name="M:libffmpeg.av_double2int(System.Double)" decl="false" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavutil\intfloat.h" line="67">
Reinterpret a double as a 64-bit integer.

</member>
<member name="T:libffmpeg.AVRounding" decl="false" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavutil\mathematics.h" line="64">
@addtogroup lavu_math
@{

</member>
<!-- Discarding badly formed XML document comment for member 'M:libffmpeg.av_gcd(System.Int64,System.Int64)'. -->
<member name="M:libffmpeg.av_rescale(System.Int64,System.Int64,System.Int64)" decl="true" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavutil\mathematics.h" line="86">
Rescale a 64-bit integer with rounding to nearest.
A simple a*b/c isn't possible as it can overflow.

</member>
<member name="M:libffmpeg.av_rescale_rnd(System.Int64,System.Int64,System.Int64,libffmpeg.AVRounding)" decl="true" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavutil\mathematics.h" line="92">
 Rescale a 64-bit integer with specified rounding.
 A simple a*b/c isn't possible as it can overflow.

 @return rescaled value a, or if AV_ROUND_PASS_MINMAX is set and a is
         INT64_MIN or INT64_MAX then a is passed through unchanged.

</member>
<member name="M:libffmpeg.av_rescale_q(System.Int64,libffmpeg.AVRational,libffmpeg.AVRational)" decl="true" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavutil\mathematics.h" line="101">
Rescale a 64-bit integer by 2 rational numbers.

</member>
<member name="M:libffmpeg.av_rescale_q_rnd(System.Int64,libffmpeg.AVRational,libffmpeg.AVRational,libffmpeg.AVRounding)" decl="true" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavutil\mathematics.h" line="106">
 Rescale a 64-bit integer by 2 rational numbers with specified rounding.

 @return rescaled value a, or if AV_ROUND_PASS_MINMAX is set and a is
         INT64_MIN or INT64_MAX then a is passed through unchanged.

</member>
<member name="M:libffmpeg.av_compare_ts(System.Int64,libffmpeg.AVRational,System.Int64,libffmpeg.AVRational)" decl="true" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavutil\mathematics.h" line="115">
Compare 2 timestamps each in its own timebases.
The result of the function is undefined if one of the timestamps
is outside the int64_t range when represented in the others timebase.
@return -1 if ts_a is before ts_b, 1 if ts_a is after ts_b or 0 if they represent the same position

</member>
<member name="M:libffmpeg.av_compare_mod(System.UInt64,System.UInt64,System.UInt64)" decl="true" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavutil\mathematics.h" line="123">
 Compare 2 integers modulo mod.
 That is we compare integers a and b for which only the least
 significant log2(mod) bits are known.

 @param mod must be a power of 2
 @return a negative value if a is smaller than b
         a positive value if a is greater than b
         0                if a equals          b

</member>
<member name="M:libffmpeg.av_rescale_delta(libffmpeg.AVRational,System.Int64,libffmpeg.AVRational,System.Int32,System.Int64*,libffmpeg.AVRational)" decl="true" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavutil\mathematics.h" line="135">
 Rescale a timestamp while preserving known durations.

 @param in_ts Input timestamp
 @param in_tb Input timebase
 @param fs_tb Duration and *last timebase
 @param duration duration till the next call
 @param out_tb Output timebase

</member>
<member name="M:libffmpeg.av_add_stable(libffmpeg.AVRational,System.Int64,libffmpeg.AVRational,System.Int64)" decl="true" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavutil\mathematics.h" line="146">
 Add a value to a timestamp.

 This function guarantees that when the same value is repeatly added that
 no accumulation of rounding errors occurs.

 @param ts Input timestamp
 @param ts_tb Input timestamp timebase
 @param inc value to add to ts
 @param inc_tb inc timebase

</member>
<member name="T:libffmpeg.AVClass" decl="false" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavutil\log.h" line="61">
Describe the class of an AVClass context structure. That is an
arbitrary struct of which the first field is a pointer to an
AVClass struct (e.g. AVCodecContext, AVFormatContext etc.).

</member>
<member name="F:libffmpeg.AVClass.class_name" decl="false" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavutil\log.h" line="67">
The name of the class; usually it is the same name as the
context structure type to which the AVClass is associated.

</member>
<member name="F:libffmpeg.AVClass.item_name" decl="false" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavutil\log.h" line="73">
A pointer to a function which returns the name of a context
instance ctx associated with the class.

</member>
<member name="T:libffmpeg.AVOption" decl="true" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavutil\log.h" line="79">
 a pointer to the first option specified in the class if any or NULL

 @see av_set_default_options()

</member>
<member name="F:libffmpeg.AVClass.version" decl="false" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavutil\log.h" line="86">
LIBAVUTIL_VERSION with which this structure was created.
This is used to allow fields to be added without requiring major
version bumps everywhere.

</member>
<member name="F:libffmpeg.AVClass.log_level_offset_offset" decl="false" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavutil\log.h" line="94">
Offset in the structure where log_level_offset is stored.
0 means there is no such variable

</member>
<member name="F:libffmpeg.AVClass.parent_log_context_offset" decl="false" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavutil\log.h" line="100">
Offset in the structure where a pointer to the parent context for
logging is stored. For example a decoder could pass its AVCodecContext
to eval as such a parent context, which an av_log() implementation
could then leverage to display the parent context.
The offset can be NULL.

</member>
<member name="F:libffmpeg.AVClass.child_next" decl="false" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavutil\log.h" line="109">
Return next AVOptions-enabled child or NULL

</member>
<member name="T:libffmpeg.AVClass" decl="true" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavutil\log.h" line="114">
 Return an AVClass corresponding to the next potential
 AVOptions-enabled child.

 The difference between child_next and this is that
 child_next iterates over _already existing_ objects, while
 child_class_next iterates over _all possible_ children.

</member>
<!-- Discarding badly formed XML document comment for member 'F:libffmpeg.AVClass.category'. -->
<!-- Discarding badly formed XML document comment for member 'F:libffmpeg.AVClass.get_category'. -->
<member name="F:libffmpeg.AVClass.query_ranges" decl="false" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavutil\log.h" line="137">
Callback to return the supported/allowed ranges.
available since version (52.12)

</member>
<member name="M:libffmpeg.av_log(System.Void*,System.Int32,System.SByte!System.Runtime.CompilerServices.IsSignUnspecifiedByte!System.Runtime.CompilerServices.IsConst*,BTEllipsis)" decl="true" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavutil\log.h" line="144">
 @addtogroup lavu_log

 @{

 @defgroup lavu_log_constants Logging Constants

 @{

Print no output.

Something went really wrong and we will crash now.

Something went wrong and recovery is not possible.
For example, no header was found for a format which depends
on headers or an illegal combination of parameters is used.

Something went wrong and cannot losslessly be recovered.
However, not all future data is affected.

Something somehow does not look correct. This may or may not
lead to problems. An example would be the use of '-vstrict -2'.

Standard information.

Detailed information.

Stuff which is only useful for libav* developers.

@}

 * Sets additional colors for extended debugging sessions.
 * @code
   av_log(ctx, AV_LOG_DEBUG|AV_LOG_C(134), "Message in purple\n");
   @endcode
 * Requires 256color terminal support. Uses outside debugging is not
 * recommended.

 Send the specified message to the log if the level is less than or equal
 to the current av_log_level. By default, all logging messages are sent to
 stderr. This behavior can be altered by setting a different logging callback
 function.
 @see av_log_set_callback

 @param avcl A pointer to an arbitrary struct of which the first field is a
        pointer to an AVClass struct.
 @param level The importance level of the message expressed using a @ref
        lavu_log_constants "Logging Constant".
 @param fmt The format string (printf-compatible) that specifies how
        subsequent arguments are converted to output.

</member>
<member name="M:libffmpeg.av_vlog(System.Void*,System.Int32,System.SByte!System.Runtime.CompilerServices.IsSignUnspecifiedByte!System.Runtime.CompilerServices.IsConst*,System.SByte!System.Runtime.CompilerServices.IsSignUnspecifiedByte*)" decl="true" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavutil\log.h" line="231">
 Send the specified message to the log if the level is less than or equal
 to the current av_log_level. By default, all logging messages are sent to
 stderr. This behavior can be altered by setting a different logging callback
 function.
 @see av_log_set_callback

 @param avcl A pointer to an arbitrary struct of which the first field is a
        pointer to an AVClass struct.
 @param level The importance level of the message expressed using a @ref
        lavu_log_constants "Logging Constant".
 @param fmt The format string (printf-compatible) that specifies how
        subsequent arguments are converted to output.
 @param vl The arguments referenced by the format string.

</member>
<member name="M:libffmpeg.av_log_get_level" decl="true" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavutil\log.h" line="248">
 Get the current log level

 @see lavu_log_constants

 @return Current log level

</member>
<member name="M:libffmpeg.av_log_set_level(System.Int32)" decl="true" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavutil\log.h" line="257">
 Set the log level

 @see lavu_log_constants

 @param level Logging level

</member>
<member name="M:libffmpeg.av_log_set_callback(=FUNC:System.Void(System.Void*,System.Int32,System.SByte!System.Runtime.CompilerServices.IsSignUnspecifiedByte!System.Runtime.CompilerServices.IsConst*,System.SByte!System.Runtime.CompilerServices.IsSignUnspecifiedByte*))" decl="true" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavutil\log.h" line="266">
 Set the logging callback

 @note The callback must be thread safe, even if the application does not use
       threads itself as some codecs are multithreaded.

 @see av_log_default_callback

 @param callback A logging function with a compatible signature.

</member>
<member name="M:libffmpeg.av_log_default_callback(System.Void*,System.Int32,System.SByte!System.Runtime.CompilerServices.IsSignUnspecifiedByte!System.Runtime.CompilerServices.IsConst*,System.SByte!System.Runtime.CompilerServices.IsSignUnspecifiedByte*)" decl="true" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavutil\log.h" line="278">
 Default logging callback

 It prints the message to stderr, optionally colorizing it.

 @param avcl A pointer to an arbitrary struct of which the first field is a
        pointer to an AVClass struct.
 @param level The importance level of the message expressed using a @ref
        lavu_log_constants "Logging Constant".
 @param fmt The format string (printf-compatible) that specifies how
        subsequent arguments are converted to output.
 @param vl The arguments referenced by the format string.

</member>
<member name="M:libffmpeg.av_default_item_name(System.Void*)" decl="true" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavutil\log.h" line="294">
 Return the context name

 @param  ctx The AVClass context

 @return The AVClass class_name

</member>
<member name="M:libffmpeg.av_log_format_line(System.Void*,System.Int32,System.SByte!System.Runtime.CompilerServices.IsSignUnspecifiedByte!System.Runtime.CompilerServices.IsConst*,System.SByte!System.Runtime.CompilerServices.IsSignUnspecifiedByte*,System.SByte!System.Runtime.CompilerServices.IsSignUnspecifiedByte*,System.Int32,System.Int32*)" decl="true" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavutil\log.h" line="304">
Format a line of log the same way as the default callback.
@param line          buffer to receive the formated line
@param line_size     size of the buffer
@param print_prefix  used to store whether the prefix must be printed;
                     must point to a persistent integer initially set to 1

</member>
<member name="M:libffmpeg.av_log_set_flags(System.Int32)" decl="true" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavutil\log.h" line="314">
av_dlog macros
Useful to print debug messages that shouldn't get compiled in normally.

Skip repeated messages, this requires the user app to use av_log() instead of
(f)printf as the 2 would otherwise interfere and lead to
"Last message repeated x times" messages below (f)printf messages with some
bad luck.
Also to receive the last, "last repeated" line if any, the user app must
call av_log(NULL, AV_LOG_QUIET, "%s", ""); at the end

 Include the log severity in messages originating from codecs.

 Results in messages such as:
 [rawvideo @ 0xDEADBEEF] [error] encode did not produce valid pts

</member>
<member name="F:AV_PIX_FMT_YUV420P9BE" decl="false" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavutil\pixfmt.h" line="156">
The following 12 formats have the disadvantage of needing 1 format for each bit depth.
Notice that each 9/10 bits sample is stored in 16 bits with extra padding.
If you want to support multiple bit depths, then using AV_PIX_FMT_YUV420P16* with the bpp stored separately is better.

</member>
<member name="F:AV_PIX_FMT_YUVA422P_LIBAV" decl="false" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavutil\pixfmt.h" line="189">
duplicated pixel formats for compatibility with libav.
FFmpeg supports these formats since May 8 2012 and Jan 28 2012 (commits f9ca1ac7 and 143a5c55)
Libav added them Oct 12 2012 with incompatible values (commit 6d5600e85)

</member>
<member name="F:AV_PIX_FMT_RGBA64BE_LIBAV" decl="false" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavutil\pixfmt.h" line="224">
duplicated pixel formats for compatibility with libav.
FFmpeg supports these formats since Sat Sep 24 06:01:45 2011 +0200 (commits 9569a3c9f41387a8c7d1ce97d8693520477a66c3)
also see Fri Nov 25 01:38:21 2011 +0100 92afb431621c79155fcb7171d26f137eb1bee028
Libav added them Sun Mar 16 23:05:47 2014 +0100 with incompatible values (commit 1481d24c3a0abf81e1d7a514547bd5305232be30)

</member>
<member name="F:AV_PIX_FMT_GBRAP_LIBAV" decl="false" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavutil\pixfmt.h" line="242">
duplicated pixel formats for compatibility with libav.
FFmpeg supports these formats since May 3 2013 (commit e6d4e687558d08187e7a415a7725e4b1a416f782)
Libav added them Jan 14 2015 with incompatible values (commit 0e6c7dfa650e8b0497bfa7a06394b7a462ddc33a)

</member>
<member name="F:AV_PIX_FMT_QSV" decl="false" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavutil\pixfmt.h" line="250">
HW acceleration through QSV, data[3] contains a pointer to the
mfxFrameSurface1 structure.

</member>
<!-- Discarding badly formed XML document comment for member 'T:libffmpeg.AVPixelFormat'. -->
<member name="T:libffmpeg.AVColorPrimaries" decl="false" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavutil\pixfmt.h" line="448">
Chromaticity coordinates of the source primaries.

</member>
<member name="T:libffmpeg.AVColorTransferCharacteristic" decl="false" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavutil\pixfmt.h" line="466">
Color Transfer Characteristic.

</member>
<member name="T:libffmpeg.AVColorSpace" decl="false" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavutil\pixfmt.h" line="489">
YUV colorspace type.

</member>
<member name="T:libffmpeg.AVColorRange" decl="false" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavutil\pixfmt.h" line="509">
MPEG vs JPEG YUV range.

</member>
<member name="T:libffmpeg.AVChromaLocation" decl="false" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavutil\pixfmt.h" line="519">
 Location of chroma samples.

  X   X      3 4 X      X are luma samples,
             1 2        1-6 are possible chroma positions
  X   X      5 6 X      0 is undefined/unknown position

</member>
<member name="M:libffmpeg.av_x_if_null(System.Void!System.Runtime.CompilerServices.IsConst*,System.Void!System.Runtime.CompilerServices.IsConst*)" decl="false" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavutil\avutil.h" line="298">
Return x default pointer in case p is NULL.

</member>
<member name="M:libffmpeg.av_int_list_length_for_size(System.UInt32,System.Void!System.Runtime.CompilerServices.IsConst*,System.UInt64)" decl="true" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavutil\avutil.h" line="306">
 Compute the length of an integer list.

 @param elsize  size in bytes of each list element (only 1, 2, 4 or 8)
 @param term    list terminator (usually 0 or -1)
 @param list    pointer to the list
 @return  length of the list, in elements, not counting the terminator

</member>
<member name="M:libffmpeg.av_fopen_utf8(System.SByte!System.Runtime.CompilerServices.IsSignUnspecifiedByte!System.Runtime.CompilerServices.IsConst*,System.SByte!System.Runtime.CompilerServices.IsSignUnspecifiedByte!System.Runtime.CompilerServices.IsConst*)" decl="true" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavutil\avutil.h" line="317">
 Compute the length of an integer list.

 @param term  list terminator (usually 0 or -1)
 @param list  pointer to the list
 @return  length of the list, in elements, not counting the terminator

Open a file using a UTF-8 filename.
The API of this function matches POSIX fopen(), errors are returned through
errno.

</member>
<member name="M:libffmpeg.av_get_time_base_q" decl="true" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavutil\avutil.h" line="334">
Return the fractional representation of the internal time base.

</member>
<member name="T:libffmpeg.AVSampleFormat" decl="false" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavutil\avutil.h" line="339">
@}
@}

 @addtogroup lavu_audio
 @{

 @defgroup lavu_sampfmts Audio sample formats

 Audio sample format enumeration and related convenience functions.
 @{


 Audio sample formats

 - The data described by the sample format is always in native-endian order.
   Sample values can be expressed by native C types, hence the lack of a signed
   24-bit sample format even though it is a common raw audio data format.

 - The floating-point formats are based on full volume being in the range
   [-1.0, 1.0]. Any values outside this range are beyond full volume level.

 - The data layout as used in av_samples_fill_arrays() and elsewhere in FFmpeg
   (such as AVFrame in libavcodec) is as follows:

 @par
 For planar sample formats, each audio channel is in a separate data plane,
 and linesize is the buffer size, in bytes, for a single plane. All data
 planes must be the same size. For packed sample formats, only the first data
 plane is used, and samples for each channel are interleaved. In this case,
 linesize is the buffer size, in bytes, for the 1 plane.


</member>
<member name="M:libffmpeg.av_get_sample_fmt_name(libffmpeg.AVSampleFormat)" decl="true" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavutil\samplefmt.h" line="76">
Return the name of sample_fmt, or NULL if sample_fmt is not
recognized.

</member>
<member name="T:libffmpeg.AVSampleFormat" decl="true" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavutil\samplefmt.h" line="82">
Return a sample format corresponding to name, or AV_SAMPLE_FMT_NONE
on error.

</member>
<!-- Discarding badly formed XML document comment for member 'T:libffmpeg.AVSampleFormat'. -->
<member name="T:libffmpeg.AVSampleFormat" decl="true" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavutil\samplefmt.h" line="96">
 * Get the packed alternative form of the given sample format.
 *
 * If the passed sample_fmt is already in packed format, the format returned is
 * the same as the input.
 *
 * @return  the packed alternative form of the given sample format or
            AV_SAMPLE_FMT_NONE on error.

</member>
<member name="T:libffmpeg.AVSampleFormat" decl="true" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavutil\samplefmt.h" line="107">
 * Get the planar alternative form of the given sample format.
 *
 * If the passed sample_fmt is already in planar format, the format returned is
 * the same as the input.
 *
 * @return  the planar alternative form of the given sample format or
            AV_SAMPLE_FMT_NONE on error.

</member>
<member name="M:libffmpeg.av_get_sample_fmt_string(System.SByte!System.Runtime.CompilerServices.IsSignUnspecifiedByte*,System.Int32,libffmpeg.AVSampleFormat)" decl="true" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavutil\samplefmt.h" line="118">
 Generate a string corresponding to the sample format with
 sample_fmt, or a header if sample_fmt is negative.

 @param buf the buffer where to write the string
 @param buf_size the size of buf
 @param sample_fmt the number of the sample format to print the
 corresponding info string, or a negative value to print the
 corresponding header.
 @return the pointer to the filled buffer or NULL if sample_fmt is
 unknown or in case of other errors

</member>
<member name="M:libffmpeg.av_get_bytes_per_sample(libffmpeg.AVSampleFormat)" decl="true" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavutil\samplefmt.h" line="132">
 Return number of bytes per sample.

 @param sample_fmt the sample format
 @return number of bytes per sample or zero if unknown for the given
 sample format

</member>
<member name="M:libffmpeg.av_sample_fmt_is_planar(libffmpeg.AVSampleFormat)" decl="true" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavutil\samplefmt.h" line="141">
 Check if the sample format is planar.

 @param sample_fmt the sample format to inspect
 @return 1 if the sample format is planar, 0 if it is interleaved

</member>
<member name="M:libffmpeg.av_samples_get_buffer_size(System.Int32*,System.Int32,System.Int32,libffmpeg.AVSampleFormat,System.Int32)" decl="true" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavutil\samplefmt.h" line="149">
 Get the required buffer size for the given audio parameters.

 @param[out] linesize calculated linesize, may be NULL
 @param nb_channels   the number of channels
 @param nb_samples    the number of samples in a single channel
 @param sample_fmt    the sample format
 @param align         buffer size alignment (0 = default, 1 = no alignment)
 @return              required buffer size, or negative error code on failure

</member>
<member name="M:libffmpeg.av_samples_fill_arrays(System.Byte**,System.Int32*,System.Byte!System.Runtime.CompilerServices.IsConst*,System.Int32,System.Int32,libffmpeg.AVSampleFormat,System.Int32)" decl="true" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavutil\samplefmt.h" line="162">
 @}

 @defgroup lavu_sampmanip Samples manipulation

 Functions that manipulate audio samples
 @{

 Fill plane data pointers and linesize for samples with sample
 format sample_fmt.

 The audio_data array is filled with the pointers to the samples data planes:
 for planar, set the start point of each channel's data within the buffer,
 for packed, set the start point of the entire buffer only.

 The value pointed to by linesize is set to the aligned size of each
 channel's data buffer for planar layout, or to the aligned size of the
 buffer for all channels for packed layout.

 The buffer in buf must be big enough to contain all the samples
 (use av_samples_get_buffer_size() to compute its minimum size),
 otherwise the audio_data pointers will point to invalid data.

 @see enum AVSampleFormat
 The documentation for AVSampleFormat describes the data layout.

 @param[out] audio_data  array to be filled with the pointer for each channel
 @param[out] linesize    calculated linesize, may be NULL
 @param buf              the pointer to a buffer containing the samples
 @param nb_channels      the number of channels
 @param nb_samples       the number of samples in a single channel
 @param sample_fmt       the sample format
 @param align            buffer size alignment (0 = default, 1 = no alignment)
 @return                 &gt;=0 on success or a negative error code on failure
 @todo return minimum size in bytes required for the buffer in case
 of success at the next bump

</member>
<!-- Discarding badly formed XML document comment for member 'M:libffmpeg.av_samples_alloc(System.Byte**,System.Int32*,System.Int32,System.Int32,libffmpeg.AVSampleFormat,System.Int32)'. -->
<member name="M:libffmpeg.av_samples_alloc_array_and_samples(System.Byte***,System.Int32*,System.Int32,System.Int32,libffmpeg.AVSampleFormat,System.Int32)" decl="true" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavutil\samplefmt.h" line="228">
 Allocate a data pointers array, samples buffer for nb_samples
 samples, and fill data pointers and linesize accordingly.

 This is the same as av_samples_alloc(), but also allocates the data
 pointers array.

 @see av_samples_alloc()

</member>
<member name="M:libffmpeg.av_samples_copy(System.Byte**,System.Byte*!System.Runtime.CompilerServices.IsConst*,System.Int32,System.Int32,System.Int32,System.Int32,libffmpeg.AVSampleFormat)" decl="true" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavutil\samplefmt.h" line="240">
 Copy samples from src to dst.

 @param dst destination array of pointers to data planes
 @param src source array of pointers to data planes
 @param dst_offset offset in samples at which the data will be written to dst
 @param src_offset offset in samples at which the data will be read from src
 @param nb_samples number of samples to be copied
 @param nb_channels number of audio channels
 @param sample_fmt audio sample format

</member>
<member name="M:libffmpeg.av_samples_set_silence(System.Byte**,System.Int32,System.Int32,System.Int32,libffmpeg.AVSampleFormat)" decl="true" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavutil\samplefmt.h" line="255">
 Fill an audio buffer with silence.

 @param audio_data  array of pointers to data planes
 @param offset      offset in samples at which to start filling
 @param nb_samples  number of samples to fill
 @param nb_channels number of audio channels
 @param sample_fmt  audio sample format

</member>
<member name="T:libffmpeg.AVBuffer" decl="true" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavutil\samplefmt.h" line="267">
@}
@}

@file
@ingroup lavu_buffer
refcounted data buffer API

 @defgroup lavu_buffer AVBuffer
 @ingroup lavu_data

 @{
 AVBuffer is an API for reference-counted data buffers.

 There are two core objects in this API -- AVBuffer and AVBufferRef. AVBuffer
 represents the data buffer itself; it is opaque and not meant to be accessed
 by the caller directly, but only through AVBufferRef. However, the caller may
 e.g. compare two AVBuffer pointers to check whether two different references
 are describing the same data buffer. AVBufferRef represents a single
 reference to an AVBuffer and it is the object that may be manipulated by the
 caller directly.

 There are two functions provided for creating a new AVBuffer with a single
 reference -- av_buffer_alloc() to just allocate a new buffer, and
 av_buffer_create() to wrap an existing array in an AVBuffer. From an existing
 reference, additional references may be created with av_buffer_ref().
 Use av_buffer_unref() to free a reference (this will automatically free the
 data once all the references are freed).

 The convention throughout this API and the rest of FFmpeg is such that the
 buffer is considered writable if there exists only one reference to it (and
 it has not been marked as read-only). The av_buffer_is_writable() function is
 provided to check whether this is true and av_buffer_make_writable() will
 automatically create a new writable buffer when necessary.
 Of course nothing prevents the calling code from violating this convention,
 however that is safe only when all the existing references are under its
 control.

 @note Referencing and unreferencing the buffers is thread-safe and thus
 may be done from multiple threads simultaneously without any need for
 additional locking.

 @note Two different references to the same buffer can point to different
 parts of the buffer (i.e. their AVBufferRef.data will not be equal).

A reference counted buffer type. It is opaque and is meant to be used through
references (AVBufferRef).

</member>
<member name="T:libffmpeg.AVBufferRef" decl="false" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavutil\buffer.h" line="75">
 A reference to a data buffer.

 The size of this struct is not a part of the public ABI and it is not meant
 to be allocated directly.

</member>
<member name="F:libffmpeg.AVBufferRef.data" decl="false" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavutil\buffer.h" line="84">
The data buffer. It is considered writable if and only if
this is the only reference to the buffer, in which case
av_buffer_is_writable() returns 1.

</member>
<member name="F:libffmpeg.AVBufferRef.size" decl="false" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavutil\buffer.h" line="90">
Size of data in bytes.

</member>
<member name="M:libffmpeg.av_buffer_alloc(System.Int32)" decl="true" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavutil\buffer.h" line="96">
 Allocate an AVBuffer of the given size using av_malloc().

 @return an AVBufferRef of given size or NULL when out of memory

</member>
<member name="M:libffmpeg.av_buffer_allocz(System.Int32)" decl="true" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavutil\buffer.h" line="103">
Same as av_buffer_alloc(), except the returned buffer will be initialized
to zero.

</member>
<member name="M:libffmpeg.av_buffer_create(System.Byte*,System.Int32,=FUNC:System.Void(System.Void*,System.Byte*),System.Void*,System.Int32)" decl="true" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavutil\buffer.h" line="109">
Always treat the buffer as read-only, even when it has only one
reference.

 Create an AVBuffer from an existing array.

 If this function is successful, data is owned by the AVBuffer. The caller may
 only access data through the returned AVBufferRef and references derived from
 it.
 If this function fails, data is left untouched.
 @param data   data array
 @param size   size of data in bytes
 @param free   a callback for freeing this buffer's data
 @param opaque parameter to be got for processing or passed to free
 @param flags  a combination of AV_BUFFER_FLAG_*

 @return an AVBufferRef referring to data on success, NULL on failure.

</member>
<member name="M:libffmpeg.av_buffer_default_free(System.Void*,System.Byte*)" decl="true" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavutil\buffer.h" line="134">
Default free callback, which calls av_free() on the buffer data.
This function is meant to be passed to av_buffer_create(), not called
directly.

</member>
<member name="M:libffmpeg.av_buffer_ref(libffmpeg.AVBufferRef*)" decl="true" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavutil\buffer.h" line="141">
 Create a new reference to an AVBuffer.

 @return a new AVBufferRef referring to the same AVBuffer as buf or NULL on
 failure.

</member>
<member name="M:libffmpeg.av_buffer_unref(libffmpeg.AVBufferRef**)" decl="true" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavutil\buffer.h" line="149">
 Free a given reference and automatically free the buffer if there are no more
 references to it.

 @param buf the reference to be freed. The pointer is set to NULL on return.

</member>
<member name="M:libffmpeg.av_buffer_is_writable(libffmpeg.AVBufferRef!System.Runtime.CompilerServices.IsConst*)" decl="true" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavutil\buffer.h" line="157">
@return 1 if the caller may write to the data referred to by buf (which is
true if and only if buf is the only reference to the underlying AVBuffer).
Return 0 otherwise.
A positive answer is valid until av_buffer_ref() is called on buf.

</member>
<member name="M:libffmpeg.av_buffer_get_opaque(libffmpeg.AVBufferRef!System.Runtime.CompilerServices.IsConst*)" decl="true" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavutil\buffer.h" line="165">
@return the opaque parameter set by av_buffer_create.

</member>
<member name="M:libffmpeg.av_buffer_make_writable(libffmpeg.AVBufferRef**)" decl="true" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavutil\buffer.h" line="172">
 Create a writable reference from a given buffer reference, avoiding data copy
 if possible.

 @param buf buffer reference to make writable. On success, buf is either left
            untouched, or it is unreferenced and a new writable AVBufferRef is
            written in its place. On failure, buf is left untouched.
 @return 0 on success, a negative AVERROR on failure.

</member>
<member name="M:libffmpeg.av_buffer_realloc(libffmpeg.AVBufferRef**,System.Int32)" decl="true" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavutil\buffer.h" line="183">
 Reallocate a given buffer.

 @param buf  a buffer reference to reallocate. On success, buf will be
             unreferenced and a new reference with the required size will be
             written in its place. On failure buf will be left untouched. *buf
             may be NULL, then a new buffer is allocated.
 @param size required new buffer size.
 @return 0 on success, a negative AVERROR on failure.

 @note the buffer is actually reallocated with av_realloc() only if it was
 initially allocated through av_buffer_realloc(NULL) and there is only one
 reference to it (i.e. the one passed to this function). In all other cases
 a new buffer is allocated and the data is copied.

</member>
<member name="T:libffmpeg.AVBufferPool" decl="true" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavutil\buffer.h" line="200">
@}

 @defgroup lavu_bufferpool AVBufferPool
 @ingroup lavu_data

 @{
 AVBufferPool is an API for a lock-free thread-safe pool of AVBuffers.

 Frequently allocating and freeing large buffers may be slow. AVBufferPool is
 meant to solve this in cases when the caller needs a set of buffers of the
 same size (the most obvious use case being buffers for raw video or audio
 frames).

 At the beginning, the user must call av_buffer_pool_init() to create the
 buffer pool. Then whenever a buffer is needed, call av_buffer_pool_get() to
 get a reference to a new buffer, similar to av_buffer_alloc(). This new
 reference works in all aspects the same way as the one created by
 av_buffer_alloc(). However, when the last reference to this buffer is
 unreferenced, it is returned to the pool instead of being freed and will be
 reused for subsequent av_buffer_pool_get() calls.

 When the caller is done with the pool and no longer needs to allocate any new
 buffers, av_buffer_pool_uninit() must be called to mark the pool as freeable.
 Once all the buffers are released, it will automatically be freed.

 Allocating and releasing buffers with this API is thread-safe as long as
 either the default alloc callback is used, or the user-supplied one is
 thread-safe.

The buffer pool. This structure is opaque and not meant to be accessed
directly. It is allocated with av_buffer_pool_init() and freed with
av_buffer_pool_uninit().

</member>
<member name="M:libffmpeg.av_buffer_pool_init(System.Int32,=FUNC:libffmpeg.AVBufferRef*(System.Int32))" decl="true" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavutil\buffer.h" line="240">
 Allocate and initialize a buffer pool.

 @param size size of each buffer in this pool
 @param alloc a function that will be used to allocate new buffers when the
 pool is empty. May be NULL, then the default allocator will be used
 (av_buffer_alloc()).
 @return newly created buffer pool on success, NULL on error.

</member>
<member name="M:libffmpeg.av_buffer_pool_uninit(libffmpeg.AVBufferPool**)" decl="true" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavutil\buffer.h" line="251">
 Mark the pool as being available for freeing. It will actually be freed only
 once all the allocated buffers associated with the pool are released. Thus it
 is safe to call this function while some of the allocated buffers are still
 in use.

 @param pool pointer to the pool to be freed. It will be set to NULL.
 @see av_buffer_pool_can_uninit()

</member>
<member name="M:libffmpeg.av_buffer_pool_get(libffmpeg.AVBufferPool*)" decl="true" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavutil\buffer.h" line="262">
 Allocate a new AVBuffer, reusing an old buffer from the pool when available.
 This function may be called simultaneously from multiple threads.

 @return a reference to the new buffer on success, NULL on error.

</member>
<!-- Discarding badly formed XML document comment for member 'M:libffmpeg.av_get_cpu_flags'. -->
<member name="M:libffmpeg.av_force_cpu_flags(System.Int32)" decl="true" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavutil\cpu.h" line="77">
Disables cpu detection and forces the specified flags.
-1 is a special case that disables forcing of specific flags.

</member>
<member name="M:libffmpeg.av_set_cpu_flags_mask(System.Int32)" decl="true" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavutil\cpu.h" line="83">
 Set a mask on flags returned by av_get_cpu_flags().
 This function is mainly useful for testing.
 Please use av_force_cpu_flags() and av_get_cpu_flags() instead which are more flexible

 @warning this function is not thread safe.

</member>
<member name="M:libffmpeg.av_parse_cpu_flags(System.SByte!System.Runtime.CompilerServices.IsSignUnspecifiedByte!System.Runtime.CompilerServices.IsConst*)" decl="true" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavutil\cpu.h" line="92">
 Parse CPU flags from a string.

 The returned flags contain the specified flags as well as related unspecified flags.

 This function exists only for compatibility with libav.
 Please use av_parse_cpu_caps() when possible.
 @return a combination of AV_CPU_* flags, negative on error.

</member>
<member name="M:libffmpeg.av_parse_cpu_caps(System.UInt32*,System.SByte!System.Runtime.CompilerServices.IsSignUnspecifiedByte!System.Runtime.CompilerServices.IsConst*)" decl="true" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavutil\cpu.h" line="104">
 Parse CPU caps from a string and update the given AV_CPU_* flags based on that.

 @return negative on error.

</member>
<member name="M:libffmpeg.av_cpu_count" decl="true" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavutil\cpu.h" line="111">
@return the number of logical CPU cores present.

</member>
<member name="T:libffmpeg.AVMatrixEncoding" decl="false" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavutil\channel_layout.h" line="27">
@file
audio channel layout utility functions

@addtogroup lavu_audio
@{

 @defgroup channel_masks Audio channel masks

 A channel layout is a 64-bits integer with a bit set for every channel.
 The number of bits set must be equal to the number of channels.
 The value 0 means that the channel layout is not known.
 @note this data structure is not powerful enough to handle channels
 combinations that have the same channel multiple times, such as
 dual-mono.

 @{

Channel mask value used for AVCodecContext.request_channel_layout
    to indicate that the user requests the channel order of the decoder output
    to be the native codec channel order. 
@}
@defgroup channel_mask_c Audio channel layouts
@{

</member>
<member name="M:libffmpeg.av_get_channel_layout(System.SByte!System.Runtime.CompilerServices.IsSignUnspecifiedByte!System.Runtime.CompilerServices.IsConst*)" decl="true" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavutil\channel_layout.h" line="124">
 Return a channel layout id that matches name, or 0 if no match is found.

 name can be one or several of the following notations,
 separated by '+' or '|':
 - the name of an usual channel layout (mono, stereo, 4.0, quad, 5.0,
   5.0(side), 5.1, 5.1(side), 7.1, 7.1(wide), downmix);
 - the name of a single channel (FL, FR, FC, LFE, BL, BR, FLC, FRC, BC,
   SL, SR, TC, TFL, TFC, TFR, TBL, TBC, TBR, DL, DR);
 - a number of channels, in decimal, optionally followed by 'c', yielding
   the default channel layout for that number of channels (@see
   av_get_default_channel_layout);
 - a channel layout mask, in hexadecimal starting with "0x" (see the
   AV_CH_* macros).

 @warning Starting from the next major bump the trailing character
 'c' to specify a number of channels will be required, while a
 channel layout mask could also be specified as a decimal number
 (if and only if not followed by "c").

 Example: "stereo+FC" = "2c+FC" = "2c+1c" = "0x7"

</member>
<!-- Discarding badly formed XML document comment for member 'M:libffmpeg.av_get_channel_layout_string(System.SByte!System.Runtime.CompilerServices.IsSignUnspecifiedByte*,System.Int32,System.Int32,System.UInt64)'. -->
<member name="M:libffmpeg.av_bprint_channel_layout(libffmpeg.AVBPrint*,System.Int32,System.UInt64)" decl="true" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavutil\channel_layout.h" line="158">
Append a description of a channel layout to a bprint buffer.

</member>
<member name="M:libffmpeg.av_get_channel_layout_nb_channels(System.UInt64)" decl="true" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavutil\channel_layout.h" line="163">
Return the number of channels in the channel layout.

</member>
<member name="M:libffmpeg.av_get_default_channel_layout(System.Int32)" decl="true" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavutil\channel_layout.h" line="168">
Return default channel layout for a given number of channels.

</member>
<member name="M:libffmpeg.av_get_channel_layout_channel_index(System.UInt64,System.UInt64)" decl="true" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavutil\channel_layout.h" line="173">
 Get the index of a channel in channel_layout.

 @param channel a channel layout describing exactly one channel which must be
                present in channel_layout.

 @return index of channel in channel_layout on success, a negative AVERROR
         on error.

</member>
<member name="M:libffmpeg.av_channel_layout_extract_channel(System.UInt64,System.Int32)" decl="true" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavutil\channel_layout.h" line="185">
Get the channel with the given index in channel_layout.

</member>
<member name="M:libffmpeg.av_get_channel_name(System.UInt64)" decl="true" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavutil\channel_layout.h" line="190">
 Get the name of a given channel.

 @return channel name on success, NULL on error.

</member>
<member name="M:libffmpeg.av_get_channel_description(System.UInt64)" decl="true" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavutil\channel_layout.h" line="197">
 Get the description of a given channel.

 @param channel  a channel layout with a single channel
 @return  channel description on success, NULL on error

</member>
<!-- Discarding badly formed XML document comment for member 'M:libffmpeg.av_get_standard_channel_layout(System.UInt32,System.UInt64*,System.SByte!System.Runtime.CompilerServices.IsSignUnspecifiedByte!System.Runtime.CompilerServices.IsConst**)'. -->
<!-- Discarding badly formed XML document comment for member 'T:libffmpeg.AVDictionaryEntry'. -->
<member name="M:libffmpeg.av_dict_get(libffmpeg.AVDictionary!System.Runtime.CompilerServices.IsConst*,System.SByte!System.Runtime.CompilerServices.IsSignUnspecifiedByte!System.Runtime.CompilerServices.IsConst*,libffmpeg.AVDictionaryEntry!System.Runtime.CompilerServices.IsConst*,System.Int32)" decl="true" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavutil\dict.h" line="89">
 Get a dictionary entry with matching key.

 The returned entry key or value must not be changed, or it will
 cause undefined behavior.

 To iterate through all the dictionary entries, you can set the matching key
 to the null string "" and set the AV_DICT_IGNORE_SUFFIX flag.

 @param prev Set to the previous matching element to find the next.
             If set to NULL the first matching element is returned.
 @param key matching key
 @param flags a collection of AV_DICT_* flags controlling how the entry is retrieved
 @return found entry or NULL in case no matching entry was found in the dictionary

</member>
<member name="M:libffmpeg.av_dict_count(libffmpeg.AVDictionary!System.Runtime.CompilerServices.IsConst*)" decl="true" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavutil\dict.h" line="107">
 Get number of entries in dictionary.

 @param m dictionary
 @return  number of entries in dictionary

</member>
<!-- Discarding badly formed XML document comment for member 'M:libffmpeg.av_dict_set(libffmpeg.AVDictionary**,System.SByte!System.Runtime.CompilerServices.IsSignUnspecifiedByte!System.Runtime.CompilerServices.IsConst*,System.SByte!System.Runtime.CompilerServices.IsSignUnspecifiedByte!System.Runtime.CompilerServices.IsConst*,System.Int32)'. -->
<member name="M:libffmpeg.av_dict_set_int(libffmpeg.AVDictionary**,System.SByte!System.Runtime.CompilerServices.IsSignUnspecifiedByte!System.Runtime.CompilerServices.IsConst*,System.Int64,System.Int32)" decl="true" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavutil\dict.h" line="130">
 Convenience wrapper for av_dict_set that converts the value to a string
 and stores it.

 Note: If AV_DICT_DONT_STRDUP_KEY is set, key will be freed on error.

</member>
<member name="M:libffmpeg.av_dict_parse_string(libffmpeg.AVDictionary**,System.SByte!System.Runtime.CompilerServices.IsSignUnspecifiedByte!System.Runtime.CompilerServices.IsConst*,System.SByte!System.Runtime.CompilerServices.IsSignUnspecifiedByte!System.Runtime.CompilerServices.IsConst*,System.SByte!System.Runtime.CompilerServices.IsSignUnspecifiedByte!System.Runtime.CompilerServices.IsConst*,System.Int32)" decl="true" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavutil\dict.h" line="138">
 Parse the key/value pairs list and add the parsed entries to a dictionary.

 In case of failure, all the successfully set entries are stored in
 *pm. You may need to manually free the created dictionary.

 @param key_val_sep  a 0-terminated list of characters used to separate
                     key from value
 @param pairs_sep    a 0-terminated list of characters used to separate
                     two pairs from each other
 @param flags        flags to use when adding to dictionary.
                     AV_DICT_DONT_STRDUP_KEY and AV_DICT_DONT_STRDUP_VAL
                     are ignored since the key/value tokens will always
                     be duplicated.
 @return             0 on success, negative AVERROR code on failure

</member>
<member name="M:libffmpeg.av_dict_copy(libffmpeg.AVDictionary**,libffmpeg.AVDictionary!System.Runtime.CompilerServices.IsConst*,System.Int32)" decl="true" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavutil\dict.h" line="158">
Copy entries from one AVDictionary struct into another.
@param dst pointer to a pointer to a AVDictionary struct. If *dst is NULL,
           this function will allocate a struct for you and put it in *dst
@param src pointer to source AVDictionary struct
@param flags flags to use when setting entries in *dst
@note metadata is read using the AV_DICT_IGNORE_SUFFIX flag

</member>
<member name="M:libffmpeg.av_dict_free(libffmpeg.AVDictionary**)" decl="true" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavutil\dict.h" line="168">
Free all the memory allocated for an AVDictionary struct
and all keys and values.

</member>
<member name="M:libffmpeg.av_dict_get_string(libffmpeg.AVDictionary!System.Runtime.CompilerServices.IsConst*,System.SByte!System.Runtime.CompilerServices.IsSignUnspecifiedByte**,System.SByte!System.Runtime.CompilerServices.IsSignUnspecifiedByte!System.Runtime.CompilerServices.IsConst,System.SByte!System.Runtime.CompilerServices.IsSignUnspecifiedByte!System.Runtime.CompilerServices.IsConst)" decl="true" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavutil\dict.h" line="174">
 Get dictionary entries as a string.

 Create a string containing dictionary's entries.
 Such string may be passed back to av_dict_parse_string().
 @note String is escaped with backslashes ('\').

 @param[in]  m             dictionary
 @param[out] buffer        Pointer to buffer that will be allocated with string containg entries.
                           Buffer must be freed by the caller when is no longer needed.
 @param[in]  key_val_sep   character used to separate key from value
 @param[in]  pairs_sep     character used to separate two pairs from each other
 @return                   &gt;= 0 on success, negative on error
 @warning Separators cannot be neither '\\' nor '\0'. They also cannot be the same.

</member>
<member name="F:AV_FRAME_DATA_PANSCAN" decl="false" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavutil\frame.h" line="49">
The data is the AVPanScan struct defined in libavcodec.

</member>
<member name="F:AV_FRAME_DATA_A53_CC" decl="false" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavutil\frame.h" line="53">
ATSC A53 Part 4 Closed Captions.
A53 CC bitstream is stored as uint8_t in AVFrameSideData.data.
The number of bytes of CC data is AVFrameSideData.size.

</member>
<member name="F:AV_FRAME_DATA_STEREO3D" decl="false" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavutil\frame.h" line="59">
Stereoscopic 3d metadata.
The data is the AVStereo3D struct defined in libavutil/stereo3d.h.

</member>
<member name="F:AV_FRAME_DATA_MATRIXENCODING" decl="false" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavutil\frame.h" line="64">
The data is the AVMatrixEncoding enum defined in libavutil/channel_layout.h.

</member>
<member name="F:AV_FRAME_DATA_DOWNMIX_INFO" decl="false" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavutil\frame.h" line="68">
Metadata relevant to a downmix procedure.
The data is the AVDownmixInfo struct defined in libavutil/downmix_info.h.

</member>
<member name="F:AV_FRAME_DATA_REPLAYGAIN" decl="false" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavutil\frame.h" line="73">
ReplayGain information in the form of the AVReplayGain struct.

</member>
<member name="F:AV_FRAME_DATA_DISPLAYMATRIX" decl="false" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavutil\frame.h" line="77">
 This side data contains a 3x3 transformation matrix describing an affine
 transformation that needs to be applied to the frame for correct
 presentation.

 See libavutil/display.h for a detailed description of the data.

</member>
<member name="F:AV_FRAME_DATA_AFD" decl="false" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavutil\frame.h" line="85">
Active Format Description data consisting of a single byte as specified
in ETSI TS 101 154 using AVActiveFormatDescription enum.

</member>
<member name="F:AV_FRAME_DATA_MOTION_VECTORS" decl="false" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavutil\frame.h" line="90">
Motion vectors exported by some codecs (on demand through the export_mvs
flag set in the libavcodec AVCodecContext flags2 option).
The data is the AVMotionVector struct defined in
libavutil/motion_vector.h.

</member>
<member name="F:AV_FRAME_DATA_SKIP_SAMPLES" decl="false" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavutil\frame.h" line="97">
Recommmends skipping the specified number of samples. This is exported
only if the "skip_manual" AVOption is set in libavcodec.
This has the same format as AV_PKT_DATA_SKIP_SAMPLES.
@code
u32le number of samples to skip from start of this packet
u32le number of samples to skip from end of this packet
u8    reason for start skip
u8    reason for end   skip (0=padding silence, 1=convergence)
@endcode

</member>
<member name="F:AV_FRAME_DATA_AUDIO_SERVICE_TYPE" decl="false" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavutil\frame.h" line="110">
This side data must be associated with an audio frame and corresponds to
enum AVAudioServiceType defined in avcodec.h.

</member>
<member name="T:libffmpeg.AVFrameSideDataType" decl="false" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavutil\dict.h" line="192">
@}

@file
@ingroup lavu_frame
reference-counted frame API

 @defgroup lavu_frame AVFrame
 @ingroup lavu_data

 @{
 AVFrame is an abstraction for reference-counted raw multimedia data.

</member>
<member name="T:libffmpeg.AVFrame" decl="false" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavutil\frame.h" line="134">
 This structure describes decoded (raw) audio or video data.

 AVFrame must be allocated using av_frame_alloc(). Note that this only
 allocates the AVFrame itself, the buffers for the data must be managed
 through other means (see below).
 AVFrame must be freed with av_frame_free().

 AVFrame is typically allocated once and then reused multiple times to hold
 different data (e.g. a single AVFrame to hold frames received from a
 decoder). In such a case, av_frame_unref() will free any references held by
 the frame and reset it to its original clean state before it
 is reused again.

 The data described by an AVFrame is usually reference counted through the
 AVBuffer API. The underlying buffer references are stored in AVFrame.buf /
 AVFrame.extended_buf. An AVFrame is considered to be reference counted if at
 least one reference is set, i.e. if AVFrame.buf[0] != NULL. In such a case,
 every single data plane must be contained in one of the buffers in
 AVFrame.buf or AVFrame.extended_buf.
 There may be a single buffer for all the data, or one separate buffer for
 each plane, or anything in between.

 sizeof(AVFrame) is not a part of the public ABI, so new fields may be added
 to the end with a minor bump.
 Similarly fields that are marked as to be only accessed by
 av_opt_ptr() can be reordered. This allows 2 forks to add fields
 without breaking compatibility with each other.

</member>
<member name="F:libffmpeg.AVFrame.data" decl="false" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavutil\frame.h" line="165">
 pointer to the picture/channel planes.
 This might be different from the first allocated byte

 Some decoders access areas outside 0,0 - width,height, please
 see avcodec_align_dimensions2(). Some filters and swscale can read
 up to 16 bytes beyond the planes, if these filters are to be used,
 then 16 extra bytes must be allocated.

</member>
<member name="F:libffmpeg.AVFrame.linesize" decl="false" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavutil\frame.h" line="176">
 For video, size in bytes of each picture line.
 For audio, size in bytes of each plane.

 For audio, only linesize[0] may be set. For planar audio, each channel
 plane must be the same size.

 For video the linesizes should be multiples of the CPUs alignment
 preference, this is 16 or 32 for modern desktop CPUs.
 Some code requires such alignment other code can be slower without
 correct alignment, for yet other it makes no difference.

 @note The linesize may be larger than the size of usable data -- there
 may be extra padding present for performance reasons.

</member>
<member name="F:libffmpeg.AVFrame.extended_data" decl="false" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavutil\frame.h" line="193">
 pointers to the data planes/channels.

 For video, this should simply point to data[].

 For planar audio, each channel has a separate data pointer, and
 linesize[0] contains the size of each channel buffer.
 For packed audio, there is just one data pointer, and linesize[0]
 contains the total size of the buffer for all channels.

 Note: Both data and extended_data should always be set in a valid frame,
 but for planar audio with more channels that can fit in data,
 extended_data must be used in order to access all channels.

</member>
<member name="F:libffmpeg.AVFrame.width" decl="false" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavutil\frame.h" line="209">
width and height of the video frame

</member>
<member name="F:libffmpeg.AVFrame.nb_samples" decl="false" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavutil\frame.h" line="214">
number of audio samples (per channel) described by this frame

</member>
<member name="F:libffmpeg.AVFrame.format" decl="false" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavutil\frame.h" line="219">
format of the frame, -1 if unknown or unset
Values correspond to enum AVPixelFormat for video frames,
enum AVSampleFormat for audio)

</member>
<member name="F:libffmpeg.AVFrame.key_frame" decl="false" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavutil\frame.h" line="226">
1 -&gt; keyframe, 0-&gt; not

</member>
<member name="T:libffmpeg.AVPictureType" decl="true" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavutil\frame.h" line="231">
Picture type of the frame.

</member>
<member name="F:libffmpeg.AVFrame.sample_aspect_ratio" decl="false" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavutil\frame.h" line="241">
Sample aspect ratio for the video frame, 0/1 if unknown/unspecified.

</member>
<member name="F:libffmpeg.AVFrame.pts" decl="false" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavutil\frame.h" line="246">
Presentation timestamp in time_base units (time when frame should be shown to user).

</member>
<member name="F:libffmpeg.AVFrame.pkt_pts" decl="false" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavutil\frame.h" line="251">
PTS copied from the AVPacket that was decoded to produce this frame.

</member>
<member name="F:libffmpeg.AVFrame.pkt_dts" decl="false" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavutil\frame.h" line="256">
DTS copied from the AVPacket that triggered returning this frame. (if frame threading isn't used)
This is also the Presentation time of this AVFrame calculated from
only AVPacket.dts values without pts values.

</member>
<member name="F:libffmpeg.AVFrame.coded_picture_number" decl="false" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavutil\frame.h" line="263">
picture number in bitstream order

</member>
<member name="F:libffmpeg.AVFrame.display_picture_number" decl="false" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavutil\frame.h" line="267">
picture number in display order

</member>
<member name="F:libffmpeg.AVFrame.quality" decl="false" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavutil\frame.h" line="272">
quality (between 1 (good) and FF_LAMBDA_MAX (bad))

</member>
<member name="F:libffmpeg.AVFrame.qscale_table" decl="false" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavutil\frame.h" line="281">
QP table

</member>
<member name="F:libffmpeg.AVFrame.qstride" decl="false" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavutil\frame.h" line="286">
QP store stride

</member>
<member name="F:libffmpeg.AVFrame.mbskip_table" decl="false" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavutil\frame.h" line="295">
mbskip_table[mb]&gt;=1 if MB didn't change
stride= mb_width = (width+15)&gt;&gt;4

</member>
<!-- Discarding badly formed XML document comment for member 'F:libffmpeg.AVFrame.motion_val'. -->
<member name="F:libffmpeg.AVFrame.mb_type" decl="false" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavutil\frame.h" line="314">
macroblock type table
mb_type_base + mb_width + 2

</member>
<member name="F:libffmpeg.AVFrame.dct_coeff" decl="false" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavutil\frame.h" line="321">
DCT coefficients

</member>
<member name="F:libffmpeg.AVFrame.ref_index" decl="false" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavutil\frame.h" line="327">
motion reference frame index
the order in which these are stored can depend on the codec.

</member>
<member name="F:libffmpeg.AVFrame.opaque" decl="false" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavutil\frame.h" line="335">
for some private data of the user

</member>
<member name="F:libffmpeg.AVFrame.error" decl="false" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavutil\frame.h" line="340">
error

</member>
<member name="F:libffmpeg.AVFrame.repeat_pict" decl="false" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavutil\frame.h" line="350">
When decoding, this signals how much the picture must be delayed.
extra_delay = repeat_pict / (2*fps)

</member>
<member name="F:libffmpeg.AVFrame.interlaced_frame" decl="false" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavutil\frame.h" line="356">
The content of the picture is interlaced.

</member>
<member name="F:libffmpeg.AVFrame.top_field_first" decl="false" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavutil\frame.h" line="361">
If the content is interlaced, is top field displayed first.

</member>
<member name="F:libffmpeg.AVFrame.palette_has_changed" decl="false" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavutil\frame.h" line="366">
Tell user application that palette has changed from previous frame.

</member>
<member name="T:libffmpeg.AVPanScan" decl="true" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavutil\frame.h" line="375">
Pan scan.

</member>
<member name="F:libffmpeg.AVFrame.reordered_opaque" decl="false" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavutil\frame.h" line="382">
reordered opaque 64bit (generally an integer or a double precision float
PTS but can be anything).
The user sets AVCodecContext.reordered_opaque to represent the input at
that time,
the decoder reorders values as needed and sets AVFrame.reordered_opaque
to exactly one of the values provided by the user through AVCodecContext.reordered_opaque
@deprecated in favor of pkt_pts

</member>
<member name="F:libffmpeg.AVFrame.hwaccel_picture_private" decl="false" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavutil\frame.h" line="394">
@deprecated this field is unused

</member>
<member name="F:libffmpeg.AVFrame.motion_subsample_log2" decl="false" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavutil\frame.h" line="404">
log2 of the size of the block which a single vector in motion_val represents:
(4-&gt;16x16, 3-&gt;8x8, 2-&gt; 4x4, 1-&gt; 2x2)

</member>
<member name="F:libffmpeg.AVFrame.sample_rate" decl="false" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavutil\frame.h" line="411">
Sample rate of the audio data.

</member>
<member name="F:libffmpeg.AVFrame.channel_layout" decl="false" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavutil\frame.h" line="416">
Channel layout of the audio data.

</member>
<!-- Discarding badly formed XML document comment for member 'F:libffmpeg.AVFrame.buf'. -->
<member name="F:libffmpeg.AVFrame.extended_buf" decl="false" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavutil\frame.h" line="435">
 For planar audio which requires more than AV_NUM_DATA_POINTERS
 AVBufferRef pointers, this array will hold all the references which
 cannot fit into AVFrame.buf.

 Note that this is different from AVFrame.extended_data, which always
 contains all the pointers. This array only contains the extra pointers,
 which cannot fit into AVFrame.buf.

 This array is always allocated using av_malloc() by whoever constructs
 the frame. It is freed in av_frame_unref().

</member>
<member name="F:libffmpeg.AVFrame.nb_extended_buf" decl="false" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavutil\frame.h" line="448">
Number of elements in extended_buf.

</member>
<member name="F:libffmpeg.AVFrame.flags" decl="false" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavutil\frame.h" line="456">
 @defgroup lavu_frame_flags AV_FRAME_FLAGS
 Flags describing additional frame properties.

 @{

The frame data may be corrupted, e.g. due to decoding errors.

@}

Frame flags, a combination of @ref lavu_frame_flags

</member>
<member name="T:libffmpeg.AVColorRange" decl="true" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavutil\frame.h" line="476">
MPEG vs JPEG YUV range.
It must be accessed using av_frame_get_color_range() and
av_frame_set_color_range().
- encoding: Set by user
- decoding: Set by libavcodec

</member>
<member name="T:libffmpeg.AVColorSpace" decl="true" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavutil\frame.h" line="489">
YUV colorspace type.
It must be accessed using av_frame_get_colorspace() and
av_frame_set_colorspace().
- encoding: Set by user
- decoding: Set by libavcodec

</member>
<member name="F:libffmpeg.AVFrame.best_effort_timestamp" decl="false" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavutil\frame.h" line="500">
frame timestamp estimated using various heuristics, in stream time base
Code outside libavcodec should access this field using:
av_frame_get_best_effort_timestamp(frame)
- encoding: unused
- decoding: set by libavcodec, read by user.

</member>
<member name="F:libffmpeg.AVFrame.pkt_pos" decl="false" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavutil\frame.h" line="509">
reordered pos from the last AVPacket that has been input into the decoder
Code outside libavcodec should access this field using:
av_frame_get_pkt_pos(frame)
- encoding: unused
- decoding: Read by user.

</member>
<member name="F:libffmpeg.AVFrame.pkt_duration" decl="false" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavutil\frame.h" line="518">
duration of the corresponding packet, expressed in
AVStream-&gt;time_base units, 0 if unknown.
Code outside libavcodec should access this field using:
av_frame_get_pkt_duration(frame)
- encoding: unused
- decoding: Read by user.

</member>
<member name="F:libffmpeg.AVFrame.metadata" decl="false" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavutil\frame.h" line="528">
metadata.
Code outside libavcodec should access this field using:
av_frame_get_metadata(frame)
- encoding: Set by user.
- decoding: Set by libavcodec.

</member>
<member name="F:libffmpeg.AVFrame.decode_error_flags" decl="false" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavutil\frame.h" line="537">
decode error flags of the frame, set to a combination of
FF_DECODE_ERROR_xxx flags if the decoder produced a frame, but there
were errors during the decoding.
Code outside libavcodec should access this field using:
av_frame_get_decode_error_flags(frame)
- encoding: unused
- decoding: set by libavcodec, read by user.

</member>
<member name="F:libffmpeg.AVFrame.channels" decl="false" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavutil\frame.h" line="550">
number of audio channels, only used for audio.
Code outside libavcodec should access this field using:
av_frame_get_channels(frame)
- encoding: unused
- decoding: Read by user.

</member>
<member name="F:libffmpeg.AVFrame.pkt_size" decl="false" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavutil\frame.h" line="559">
size of the corresponding packet containing the compressed
frame. It must be accessed using av_frame_get_pkt_size() and
av_frame_set_pkt_size().
It is set to a negative value if unknown.
- encoding: unused
- decoding: set by libavcodec, read by user.

</member>
<member name="F:libffmpeg.AVFrame.qp_table_buf" decl="false" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavutil\frame.h" line="569">
Not to be accessed directly from outside libavutil

</member>
<member name="M:libffmpeg.av_frame_get_best_effort_timestamp(libffmpeg.AVFrame!System.Runtime.CompilerServices.IsConst*)" decl="true" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavutil\frame.h" line="575">
Accessors for some AVFrame fields.
The position of these field in the structure is not part of the ABI,
they should not be accessed directly outside libavcodec.

</member>
<member name="M:libffmpeg.av_get_colorspace_name(libffmpeg.AVColorSpace)" decl="true" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavutil\frame.h" line="606">
Get the name of a colorspace.
@return a static string identifying the colorspace; can be NULL.

</member>
<member name="M:libffmpeg.av_frame_alloc" decl="true" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavutil\frame.h" line="612">
 Allocate an AVFrame and set its fields to default values.  The resulting
 struct must be freed using av_frame_free().

 @return An AVFrame filled with default values or NULL on failure.

 @note this only allocates the AVFrame itself, not the data buffers. Those
 must be allocated through other means, e.g. with av_frame_get_buffer() or
 manually.

</member>
<member name="M:libffmpeg.av_frame_free(libffmpeg.AVFrame**)" decl="true" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavutil\frame.h" line="624">
 Free the frame and any dynamically allocated objects in it,
 e.g. extended_data. If the frame is reference counted, it will be
 unreferenced first.

 @param frame frame to be freed. The pointer will be set to NULL.

</member>
<member name="M:libffmpeg.av_frame_ref(libffmpeg.AVFrame*,libffmpeg.AVFrame!System.Runtime.CompilerServices.IsConst*)" decl="true" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavutil\frame.h" line="633">
 Set up a new reference to the data described by the source frame.

 Copy frame properties from src to dst and create a new reference for each
 AVBufferRef from src.

 If src is not reference counted, new buffers are allocated and the data is
 copied.

 @return 0 on success, a negative AVERROR on error

</member>
<member name="M:libffmpeg.av_frame_clone(libffmpeg.AVFrame!System.Runtime.CompilerServices.IsConst*)" decl="true" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavutil\frame.h" line="646">
 Create a new frame that references the same data as src.

 This is a shortcut for av_frame_alloc()+av_frame_ref().

 @return newly created AVFrame on success, NULL on error.

</member>
<member name="M:libffmpeg.av_frame_unref(libffmpeg.AVFrame*)" decl="true" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavutil\frame.h" line="655">
Unreference all the buffers referenced by frame and reset the frame fields.

</member>
<member name="M:libffmpeg.av_frame_move_ref(libffmpeg.AVFrame*,libffmpeg.AVFrame*)" decl="true" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavutil\frame.h" line="660">
Move everythnig contained in src to dst and reset src.

</member>
<member name="M:libffmpeg.av_frame_get_buffer(libffmpeg.AVFrame*,System.Int32)" decl="true" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavutil\frame.h" line="665">
 Allocate new buffer(s) for audio or video data.

 The following fields must be set on frame before calling this function:
 - format (pixel format for video, sample format for audio)
 - width and height for video
 - nb_samples and channel_layout for audio

 This function will fill AVFrame.data and AVFrame.buf arrays and, if
 necessary, allocate and fill AVFrame.extended_data and AVFrame.extended_buf.
 For planar formats, one buffer will be allocated for each plane.

 @param frame frame in which to store the new buffers.
 @param align required buffer size alignment

 @return 0 on success, a negative AVERROR on error.

</member>
<member name="M:libffmpeg.av_frame_is_writable(libffmpeg.AVFrame*)" decl="true" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavutil\frame.h" line="684">
 Check if the frame data is writable.

 @return A positive value if the frame data is writable (which is true if and
 only if each of the underlying buffers has only one reference, namely the one
 stored in this frame). Return 0 otherwise.

 If 1 is returned the answer is valid until av_buffer_ref() is called on any
 of the underlying AVBufferRefs (e.g. through av_frame_ref() or directly).

 @see av_frame_make_writable(), av_buffer_is_writable()

</member>
<member name="M:libffmpeg.av_frame_make_writable(libffmpeg.AVFrame*)" decl="true" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavutil\frame.h" line="698">
 Ensure that the frame data is writable, avoiding data copy if possible.

 Do nothing if the frame is writable, allocate new buffers and copy the data
 if it is not.

 @return 0 on success, a negative AVERROR on error.

 @see av_frame_is_writable(), av_buffer_is_writable(),
 av_buffer_make_writable()

</member>
<member name="M:libffmpeg.av_frame_copy(libffmpeg.AVFrame*,libffmpeg.AVFrame!System.Runtime.CompilerServices.IsConst*)" decl="true" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavutil\frame.h" line="711">
 Copy the frame data from src to dst.

 This function does not allocate anything, dst must be already initialized and
 allocated with the same parameters as src.

 This function only copies the frame data (i.e. the contents of the data /
 extended data arrays), not any other properties.

 @return &gt;= 0 on success, a negative AVERROR on error.

</member>
<member name="M:libffmpeg.av_frame_copy_props(libffmpeg.AVFrame*,libffmpeg.AVFrame!System.Runtime.CompilerServices.IsConst*)" decl="true" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavutil\frame.h" line="724">
 Copy only "metadata" fields from src to dst.

 Metadata for the purpose of this function are those fields that do not affect
 the data layout in the buffers.  E.g. pts, sample rate (for audio) or sample
 aspect ratio (for video), but not width/height or channel layout.
 Side data is also copied.

</member>
<member name="M:libffmpeg.av_frame_get_plane_buffer(libffmpeg.AVFrame*,System.Int32)" decl="true" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavutil\frame.h" line="734">
 Get the buffer reference a given data plane is stored in.

 @param plane index of the data plane of interest in frame-&gt;extended_data.

 @return the buffer reference that contains the plane or NULL if the input
 frame is not valid.

</member>
<member name="M:libffmpeg.av_frame_new_side_data(libffmpeg.AVFrame*,libffmpeg.AVFrameSideDataType,System.Int32)" decl="true" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavutil\frame.h" line="744">
 Add a new side data to a frame.

 @param frame a frame to which the side data should be added
 @param type type of the added side data
 @param size size of the side data

 @return newly added side data on success, NULL on error

</member>
<member name="M:libffmpeg.av_frame_get_side_data(libffmpeg.AVFrame!System.Runtime.CompilerServices.IsConst*,libffmpeg.AVFrameSideDataType)" decl="true" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavutil\frame.h" line="757">
@return a pointer to the side data of a given type on success, NULL if there
is no side data with such type in this frame.

</member>
<member name="M:libffmpeg.av_frame_remove_side_data(libffmpeg.AVFrame*,libffmpeg.AVFrameSideDataType)" decl="true" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavutil\frame.h" line="764">
If side data of the supplied type exists in the frame, free it and remove it
from the frame.

</member>
<member name="M:libffmpeg.av_frame_side_data_name(libffmpeg.AVFrameSideDataType)" decl="true" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavutil\frame.h" line="770">
@return a string identifying the side data type

</member>
<member name="T:libffmpeg.AVCodecID" decl="false" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavutil\frame.h" line="775">
@}

@file
@ingroup libavc
Libavcodec version macros.

FF_API_* defines may be placed below to indicate public API that will be
dropped at a future version bump. The defines themselves are not part of
the public API and may change, break or disappear at any time.

 @defgroup libavc Encoding/Decoding Library
 @{

 @defgroup lavc_decoding Decoding
 @{
 @}

 @defgroup lavc_encoding Encoding
 @{
 @}

 @defgroup lavc_codec Codecs
 @{
 @defgroup lavc_codec_native Native Codecs
 @{
 @}
 @defgroup lavc_codec_wrappers External library wrappers
 @{
 @}
 @defgroup lavc_codec_hwaccel Hardware Accelerators bridge
 @{
 @}
 @}
 @defgroup lavc_internal Internal
 @{
 @}
 @}


 @defgroup lavc_core Core functions/structures.
 @ingroup libavc

 Basic definitions, functions for querying libavcodec capabilities,
 allocating core structures, etc.
 @{

 Identify the syntax and semantics of the bitstream.
 The principle is roughly:
 Two decoders with the same ID can decode the same streams.
 Two encoders with the same ID can encode compatible streams.
 There may be slight deviations from the principle due to implementation
 details.

 If you add a codec ID to this list, add it so that
 1. no value of a existing codec ID changes (that would break ABI),
 2. Give it a value which when taken as ASCII is recognized uniquely by a human as this specific codec.
    This ensures that 2 forks can independently add AVCodecIDs without producing conflicts.

 After adding new codec IDs, do not forget to add an entry to the codec
 descriptor list and bump libavcodec minor version.

</member>
<member name="T:libffmpeg.AVCodecDescriptor" decl="false" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavcodec\avcodec.h" line="554">
This struct describes the properties of a single codec described by an
AVCodecID.
@see avcodec_descriptor_get()

</member>
<member name="F:libffmpeg.AVCodecDescriptor.name" decl="false" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavcodec\avcodec.h" line="562">
Name of the codec described by this descriptor. It is non-empty and
unique for each codec descriptor. It should contain alphanumeric
characters and '_' only.

</member>
<member name="F:libffmpeg.AVCodecDescriptor.long_name" decl="false" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavcodec\avcodec.h" line="568">
A more descriptive name for this codec. May be NULL.

</member>
<member name="F:libffmpeg.AVCodecDescriptor.props" decl="false" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavcodec\avcodec.h" line="572">
Codec properties, a combination of AV_CODEC_PROP_* flags.

</member>
<member name="F:libffmpeg.AVCodecDescriptor.mime_types" decl="false" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavcodec\avcodec.h" line="577">
MIME type(s) associated with the codec.
May be NULL; if not, a NULL-terminated array of MIME types.
The first item is always non-NULL and is the preferred MIME type.

</member>
<!-- Discarding badly formed XML document comment for member 'T:libffmpeg.Motion_Est_ID'. -->
<member name="T:libffmpeg.AVDiscard" decl="false" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavcodec\avcodec.h" line="656">
@ingroup lavc_decoding

</member>
<member name="T:libffmpeg.RcOverride" decl="false" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavcodec\avcodec.h" line="684">
@ingroup lavc_encoding

</member>
<member name="T:libffmpeg.AVPanScan" decl="false" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavcodec\avcodec.h" line="695">
@deprecated there is no libavcodec-wide limit on the number of B-frames

Allow decoders to produce frames with data planes that are not aligned
to CPU requirements (e.g. due to cropping).

@deprecated use the "gmc" private option of the libxvid encoder

@deprecated use the flag "mv0" in the "mpv_flags" private option of the
mpegvideo encoders

@deprecated passing reference-counted frames to the encoders replaces this
flag

@deprecated edges are not used/required anymore. I.e. this flag is now always
set.

@deprecated use the flag "naq" in the "mpv_flags" private option of the
mpegvideo encoders

Codec uses get_buffer() for allocating buffers and supports custom allocators.
If not set, it might not use get_buffer() at all or use operations that
assume the buffer was allocated by avcodec_default_get_buffer.

 Encoder or decoder requires flushing with NULL input at the end in order to
 give the complete and correct output.

 NOTE: If this flag is not set, the codec is guaranteed to never be fed with
       with NULL data. The user can still send NULL data to the public encode
       or decode function, but libavcodec will not pass it along to the codec
       unless this flag is set.

 Decoders:
 The decoder has a non-zero delay and needs to be fed with avpkt-&gt;data=NULL,
 avpkt-&gt;size=0 at the end to get the delayed data until the decoder no longer
 returns frames.

 Encoders:
 The encoder needs to be fed with NULL data at the end of encoding until the
 encoder no longer returns data.

 NOTE: For encoders implementing the AVCodec.encode2() function, setting this
       flag also means that the encoder must set the pts and duration for
       each output packet. If this flag is not set, the pts and duration will
       be determined by libavcodec from the input frame.

Codec can be fed a final frame with a smaller size.
This can be used to prevent truncation of the last audio samples.

Codec can export data for HW decoding (VDPAU).

Codec can output multiple frames per AVPacket
Normally demuxers return one frame at a time, demuxers which do not do
are connected to a parser to split what they return into proper frames.
This flag is reserved to the very rare category of codecs which have a
bitstream that cannot be split into frames without timeconsuming
operations like full decoding. Demuxers carring such bitstreams thus
may return multiple frames in a packet. This has many disadvantages like
prohibiting stream copy in many cases thus it should only be considered
as a last resort.

Codec is experimental and is thus avoided in favor of non experimental
encoders

Codec should fill in channel configuration and samplerate instead of container

@deprecated no codecs use this capability

Codec supports frame-level multithreading.

Codec supports slice-based (or partition-based) multithreading.

Codec supports changed parameters at any point.

Codec supports avctx-&gt;thread_count == 0 (auto).

Audio encoder supports receiving a different number of samples in each call.

Codec is intra only.

Codec is lossless.

Pan Scan area.
This specifies the area which should be displayed.
Note there may be multiple such areas for one frame.

</member>
<member name="F:libffmpeg.AVPanScan.id" decl="false" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavcodec\avcodec.h" line="923">
id
- encoding: Set by user.
- decoding: Set by libavcodec.

</member>
<member name="F:libffmpeg.AVPanScan.width" decl="false" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavcodec\avcodec.h" line="930">
width and height in 1/16 pel
- encoding: Set by user.
- decoding: Set by libavcodec.

</member>
<member name="F:libffmpeg.AVPanScan.position" decl="false" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavcodec\avcodec.h" line="938">
position of the top left corner in 1/16 pel for up to 3 fields/frames
- encoding: Set by user.
- decoding: Set by libavcodec.

</member>
<!-- Discarding badly formed XML document comment for member 'F:AV_PKT_DATA_PARAM_CHANGE'. -->
<member name="F:AV_PKT_DATA_H263_MB_INFO" decl="false" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavcodec\avcodec.h" line="997">
An AV_PKT_DATA_H263_MB_INFO side data packet contains a number of
structures with info about macroblocks relevant to splitting the
packet into smaller packets on macroblock edges (e.g. as for RFC 2190).
That is, it does not necessarily contain info about all macroblocks,
as long as the distance between macroblocks in the info is smaller
than the target payload size.
Each MB info structure is 12 bytes, and is laid out as follows:
@code
u32le bit offset from the start of the packet
u8    current quantizer at the start of the macroblock
u8    GOB number
u16le macroblock address within the GOB
u8    horizontal MV predictor
u8    vertical MV predictor
u8    horizontal MV predictor for block number 3
u8    vertical MV predictor for block number 3
@endcode

</member>
<member name="F:AV_PKT_DATA_REPLAYGAIN" decl="false" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavcodec\avcodec.h" line="1018">
This side data should be associated with an audio stream and contains
ReplayGain information in form of the AVReplayGain struct.

</member>
<member name="F:AV_PKT_DATA_DISPLAYMATRIX" decl="false" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavcodec\avcodec.h" line="1024">
 This side data contains a 3x3 transformation matrix describing an affine
 transformation that needs to be applied to the decoded video frames for
 correct presentation.

 See libavutil/display.h for a detailed description of the data.

</member>
<member name="F:AV_PKT_DATA_STEREO3D" decl="false" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavcodec\avcodec.h" line="1033">
This side data should be associated with a video stream and contains
Stereoscopic 3D information in form of the AVStereo3D struct.

</member>
<member name="F:AV_PKT_DATA_AUDIO_SERVICE_TYPE" decl="false" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavcodec\avcodec.h" line="1039">
This side data should be associated with an audio stream and corresponds
to enum AVAudioServiceType.

</member>
<member name="F:AV_PKT_DATA_SKIP_SAMPLES" decl="false" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavcodec\avcodec.h" line="1045">
Recommmends skipping the specified number of samples
@code
u32le number of samples to skip from start of this packet
u32le number of samples to skip from end of this packet
u8    reason for start skip
u8    reason for end   skip (0=padding silence, 1=convergence)
@endcode

</member>
<member name="F:AV_PKT_DATA_JP_DUALMONO" decl="false" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavcodec\avcodec.h" line="1056">
An AV_PKT_DATA_JP_DUALMONO side data packet indicates that
the packet may contain "dual mono" audio specific to Japanese DTV
and if it is true, recommends only the selected channel to be used.
@code
u8    selected channels (0=mail/left, 1=sub/right, 2=both)
@endcode

</member>
<member name="F:AV_PKT_DATA_STRINGS_METADATA" decl="false" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavcodec\avcodec.h" line="1066">
A list of zero terminated key/value strings. There is no end marker for
the list, so it is required to rely on the side data size to stop.

</member>
<member name="F:AV_PKT_DATA_SUBTITLE_POSITION" decl="false" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavcodec\avcodec.h" line="1072">
Subtitle event position
@code
u32le x1
u32le y1
u32le x2
u32le y2
@endcode

</member>
<member name="F:AV_PKT_DATA_MATROSKA_BLOCKADDITIONAL" decl="false" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavcodec\avcodec.h" line="1083">
Data found in BlockAdditional element of matroska container. There is
no end marker for the data, so it is required to rely on the side data
size to recognize the end. 8 byte id (as found in BlockAddId) followed
by data.

</member>
<member name="F:AV_PKT_DATA_WEBVTT_IDENTIFIER" decl="false" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavcodec\avcodec.h" line="1091">
The optional first identifier line of a WebVTT cue.

</member>
<member name="F:AV_PKT_DATA_WEBVTT_SETTINGS" decl="false" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavcodec\avcodec.h" line="1096">
The optional settings (rendering instructions) that immediately
follow the timestamp specifier of a WebVTT cue.

</member>
<member name="F:AV_PKT_DATA_METADATA_UPDATE" decl="false" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavcodec\avcodec.h" line="1102">
A list of zero terminated key/value strings. There is no end marker for
the list, so it is required to rely on the side data size to stop. This
side data includes updated metadata which appeared in the stream.

</member>
<member name="T:libffmpeg.AVPacketSideDataType" decl="false" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavcodec\avcodec.h" line="965">
The decoder will keep a reference to the frame and may reuse it later.

 @defgroup lavc_packet AVPacket

 Types and functions for working with AVPacket.
 @{

</member>
<member name="T:libffmpeg.AVPacket" decl="false" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavcodec\avcodec.h" line="1116">
 This structure stores compressed data. It is typically exported by demuxers
 and then passed as input to decoders, or received as output from encoders and
 then passed to muxers.

 For video, it should typically contain one compressed frame. For audio it may
 contain several compressed frames.

 AVPacket is one of the few structs in FFmpeg, whose size is a part of public
 ABI. Thus it may be allocated on stack and no new fields can be added to it
 without libavcodec and libavformat major bump.

 The semantics of data ownership depends on the buf or destruct (deprecated)
 fields. If either is set, the packet data is dynamically allocated and is
 valid indefinitely until av_free_packet() is called (which in turn calls
 av_buffer_unref()/the destruct callback to free the data). If neither is set,
 the packet data is typically backed by some static buffer somewhere and is
 only valid for a limited time (e.g. until the next read call when demuxing).

 The side data is always allocated with av_malloc() and is freed in
 av_free_packet().

</member>
<member name="F:libffmpeg.AVPacket.buf" decl="false" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavcodec\avcodec.h" line="1139">
A reference to the reference-counted buffer where the packet data is
stored.
May be NULL, then the packet data is not reference-counted.

</member>
<member name="F:libffmpeg.AVPacket.pts" decl="false" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavcodec\avcodec.h" line="1145">
Presentation timestamp in AVStream-&gt;time_base units; the time at which
the decompressed packet will be presented to the user.
Can be AV_NOPTS_VALUE if it is not stored in the file.
pts MUST be larger or equal to dts as presentation cannot happen before
decompression, unless one wants to view hex dumps. Some formats misuse
the terms dts and pts/cts to mean something different. Such timestamps
must be converted to true pts/dts before they are stored in AVPacket.

</member>
<member name="F:libffmpeg.AVPacket.dts" decl="false" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavcodec\avcodec.h" line="1155">
Decompression timestamp in AVStream-&gt;time_base units; the time at which
the packet is decompressed.
Can be AV_NOPTS_VALUE if it is not stored in the file.

</member>
<member name="F:libffmpeg.AVPacket.flags" decl="false" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavcodec\avcodec.h" line="1164">
A combination of AV_PKT_FLAG values

</member>
<member name="F:libffmpeg.AVPacket.side_data" decl="false" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavcodec\avcodec.h" line="1168">
Additional packet data that can be provided by the container.
Packet can contain several types of side information.

</member>
<member name="F:libffmpeg.AVPacket.duration" decl="false" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavcodec\avcodec.h" line="1175">
Duration of this packet in AVStream-&gt;time_base units, 0 if unknown.
Equals next_pts - this_pts in presentation order.

</member>
<member name="F:libffmpeg.AVPacket.convergence_duration" decl="false" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavcodec\avcodec.h" line="1188">
 Time difference in AVStream-&gt;time_base units from the pts of this
 packet to the point at which the output from the decoder has converged
 independent from the availability of previous frames. That is, the
 frames are virtually identical no matter if decoding started from
 the very first frame or from this keyframe.
 Is AV_NOPTS_VALUE if unknown.
 This field is not the display duration of the current packet.
 This field has no meaning if the packet does not have AV_PKT_FLAG_KEY
 set.

 The purpose of this field is to allow seeking in streams that have no
 keyframes in the conventional sense. It corresponds to the
 recovery point SEI in H.264 and match_time_delta in NUT. It is also
 essential for some types of subtitle streams to ensure that all
 subtitles are correctly displayed after seeking.

</member>
<member name="T:libffmpeg.AVCodecInternal" decl="true" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavcodec\avcodec.h" line="1216">
@}

</member>
<member name="T:libffmpeg.AVCodecContext" decl="false" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavcodec\avcodec.h" line="1231">
main external API structure.
New fields can be added to the end with minor version bumps.
Removal, reordering and changes to existing fields require a major
version bump.
Please use AVOptions (av_opt* / av_set/get*()) to access these fields from user
applications.
sizeof(AVCodecContext) must not be used outside libav*.

</member>
<member name="F:libffmpeg.AVCodecContext.av_class" decl="false" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavcodec\avcodec.h" line="1241">
information on struct for av_log
- set by avcodec_alloc_context3

</member>
<member name="F:libffmpeg.AVCodecContext.codec_name" decl="false" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavcodec\avcodec.h" line="1251">
@deprecated this field is not used for anything in libavcodec

</member>
<!-- Discarding badly formed XML document comment for member 'F:libffmpeg.AVCodecContext.codec_tag'. -->
<member name="F:libffmpeg.AVCodecContext.stream_codec_tag" decl="false" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavcodec\avcodec.h" line="1275">
@deprecated this field is unused

</member>
<member name="T:libffmpeg.AVCodecInternal" decl="true" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavcodec\avcodec.h" line="1284">
 Private context used for internal data.

 Unlike priv_data, this is not codec-specific. It is used in general
 libavcodec functions.

</member>
<member name="F:libffmpeg.AVCodecContext.opaque" decl="false" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavcodec\avcodec.h" line="1292">
Private data of the user, can be used to carry app specific stuff.
- encoding: Set by user.
- decoding: Set by user.

</member>
<member name="F:libffmpeg.AVCodecContext.bit_rate" decl="false" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavcodec\avcodec.h" line="1299">
the average bitrate
- encoding: Set by user; unused for constant quantizer encoding.
- decoding: Set by libavcodec. 0 or some bitrate if this info is available in the stream.

</member>
<member name="F:libffmpeg.AVCodecContext.bit_rate_tolerance" decl="false" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavcodec\avcodec.h" line="1306">
number of bits the bitstream is allowed to diverge from the reference.
          the reference can be CBR (for CBR pass1) or VBR (for pass2)
- encoding: Set by user; unused for constant quantizer encoding.
- decoding: unused

</member>
<member name="F:libffmpeg.AVCodecContext.global_quality" decl="false" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavcodec\avcodec.h" line="1314">
Global quality for codecs which cannot change it per frame.
This should be proportional to MPEG-1/2/4 qscale.
- encoding: Set by user.
- decoding: unused

</member>
<member name="F:libffmpeg.AVCodecContext.compression_level" decl="false" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavcodec\avcodec.h" line="1322">
- encoding: Set by user.
- decoding: unused

</member>
<member name="F:libffmpeg.AVCodecContext.flags" decl="false" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavcodec\avcodec.h" line="1329">
CODEC_FLAG_*.
- encoding: Set by user.
- decoding: Set by user.

</member>
<member name="F:libffmpeg.AVCodecContext.flags2" decl="false" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavcodec\avcodec.h" line="1336">
CODEC_FLAG2_*
- encoding: Set by user.
- decoding: Set by user.

</member>
<member name="F:libffmpeg.AVCodecContext.extradata" decl="false" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavcodec\avcodec.h" line="1343">
some codecs need / can use extradata like Huffman tables.
mjpeg: Huffman tables
rv10: additional flags
mpeg4: global headers (they can be in the bitstream or here)
The allocated memory should be FF_INPUT_BUFFER_PADDING_SIZE bytes larger
than extradata_size to avoid problems if it is read with the bitstream reader.
The bytewise contents of extradata must not depend on the architecture or CPU endianness.
- encoding: Set/allocated/freed by libavcodec.
- decoding: Set/allocated/freed by user.

</member>
<member name="F:libffmpeg.AVCodecContext.time_base" decl="false" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavcodec\avcodec.h" line="1357">
This is the fundamental unit of time (in seconds) in terms
of which frame timestamps are represented. For fixed-fps content,
timebase should be 1/framerate and timestamp increments should be
identically 1.
This often, but not always is the inverse of the frame rate or field rate
for video.
- encoding: MUST be set by user.
- decoding: the use of this field for decoding is deprecated.
            Use framerate instead.

</member>
<member name="F:libffmpeg.AVCodecContext.ticks_per_frame" decl="false" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavcodec\avcodec.h" line="1370">
 For some codecs, the time base is closer to the field rate than the frame rate.
 Most notably, H.264 and MPEG-2 specify time_base as half of frame duration
 if no telecine is used ...

 Set to time_base ticks per frame. Default 1, e.g., H.264/MPEG-2 set it to 2.

</member>
<member name="F:libffmpeg.AVCodecContext.delay" decl="false" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavcodec\avcodec.h" line="1379">
 Codec delay.

 Encoding: Number of frames delay there will be from the encoder input to
           the decoder output. (we assume the decoder matches the spec)
 Decoding: Number of frames delay in addition to what a standard decoder
           as specified in the spec would produce.

 Video:
   Number of frames the decoded output will be delayed relative to the
   encoded input.

 Audio:
   For encoding, this field is unused (see initial_padding).

   For decoding, this is the number of samples the decoder needs to
   output before the decoder's output is valid. When seeking, you should
   start decoding this many samples prior to your desired seek point.

 - encoding: Set by libavcodec.
 - decoding: Set by libavcodec.

</member>
<member name="F:libffmpeg.AVCodecContext.width" decl="false" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavcodec\avcodec.h" line="1405">
picture width / height.
- encoding: MUST be set by user.
- decoding: May be set by the user before opening the decoder if known e.g.
            from the container. Some decoders will require the dimensions
            to be set by the caller. During decoding, the decoder may
            overwrite those values as required.

</member>
<member name="F:libffmpeg.AVCodecContext.coded_width" decl="false" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavcodec\avcodec.h" line="1415">
Bitstream width / height, may be different from width/height e.g. when
the decoded frame is cropped before being output or lowres is enabled.
- encoding: unused
- decoding: May be set by the user before opening the decoder if known
            e.g. from the container. During decoding, the decoder may
            overwrite those values as required.

</member>
<member name="F:libffmpeg.AVCodecContext.gop_size" decl="false" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavcodec\avcodec.h" line="1429">
the number of pictures in a group of pictures, or 0 for intra_only
- encoding: Set by user.
- decoding: unused

</member>
<member name="T:libffmpeg.AVPixelFormat" decl="true" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavcodec\avcodec.h" line="1436">
Pixel format, see AV_PIX_FMT_xxx.
May be set by the demuxer if known from headers.
May be overridden by the decoder if it knows better.
- encoding: Set by user.
- decoding: Set by user if known, overridden by libavcodec if known

</member>
<member name="F:libffmpeg.AVCodecContext.me_method" decl="false" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavcodec\avcodec.h" line="1445">
Motion estimation algorithm used for video coding.
1 (zero), 2 (full), 3 (log), 4 (phods), 5 (epzs), 6 (x1), 7 (hex),
8 (umh), 9 (iter), 10 (tesa) [7, 8, 10 are x264 specific, 9 is snow specific]
- encoding: MUST be set by user.
- decoding: unused

</member>
<member name="F:libffmpeg.AVCodecContext.draw_horiz_band" decl="false" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavcodec\avcodec.h" line="1454">
If non NULL, 'draw_horiz_band' is called by the libavcodec
decoder to draw a horizontal band. It improves cache usage. Not
all codecs can do that. You must check the codec capabilities
beforehand.
When multithreading is used, it may be called from multiple threads
at the same time; threads might draw different parts of the same AVFrame,
or multiple AVFrames, and there is no guarantee that slices will be drawn
in order.
The function is also used by hardware acceleration APIs.
It is called at least once during frame decoding to pass
the data needed for hardware render.
In that mode instead of pixel data, AVFrame points to
a structure specific to the acceleration API. The application
reads the structure and can change some fields to indicate progress
or mark state.
- encoding: unused
- decoding: Set by user.
@param height the height of the slice
@param y the y position of the slice
@param type 1-&gt;top field, 2-&gt;bottom field, 3-&gt;frame
@param offset offset into the AVFrame.data from which the slice should be read

</member>
<member name="T:libffmpeg.AVPixelFormat" decl="true" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavcodec\avcodec.h" line="1481">
callback to negotiate the pixelFormat
@param fmt is the list of formats which are supported by the codec,
it is terminated by -1 as 0 is a valid format, the formats are ordered by quality.
The first is always the native one.
@note The callback may be called again immediately if initialization for
the selected (hardware-accelerated) pixel format failed.
@warning Behavior is undefined if the callback returns a value not
in the fmt list of formats.
@return the chosen format
- encoding: unused
- decoding: Set by user, if not set the native format will be chosen.

</member>
<member name="F:libffmpeg.AVCodecContext.max_b_frames" decl="false" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavcodec\avcodec.h" line="1496">
maximum number of B-frames between non-B-frames
Note: The output will be delayed by max_b_frames+1 relative to the input.
- encoding: Set by user.
- decoding: unused

</member>
<!-- Discarding badly formed XML document comment for member 'F:libffmpeg.AVCodecContext.b_quant_factor'. -->
<member name="F:libffmpeg.AVCodecContext.rc_strategy" decl="false" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavcodec\avcodec.h" line="1513">
obsolete FIXME remove 
</member>
<member name="F:libffmpeg.AVCodecContext.b_quant_offset" decl="false" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavcodec\avcodec.h" line="1519">
qscale offset between IP and B-frames
- encoding: Set by user.
- decoding: unused

</member>
<member name="F:libffmpeg.AVCodecContext.has_b_frames" decl="false" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavcodec\avcodec.h" line="1526">
Size of the frame reordering buffer in the decoder.
For MPEG-2 it is 1 IPB or 0 low delay IP.
- encoding: Set by libavcodec.
- decoding: Set by libavcodec.

</member>
<member name="F:libffmpeg.AVCodecContext.mpeg_quant" decl="false" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavcodec\avcodec.h" line="1534">
0-&gt; h263 quant 1-&gt; mpeg quant
- encoding: Set by user.
- decoding: unused

</member>
<!-- Discarding badly formed XML document comment for member 'F:libffmpeg.AVCodecContext.i_quant_factor'. -->
<member name="F:libffmpeg.AVCodecContext.i_quant_offset" decl="false" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavcodec\avcodec.h" line="1550">
qscale offset between P and I-frames
- encoding: Set by user.
- decoding: unused

</member>
<member name="F:libffmpeg.AVCodecContext.lumi_masking" decl="false" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavcodec\avcodec.h" line="1557">
luminance masking (0-&gt; disabled)
- encoding: Set by user.
- decoding: unused

</member>
<member name="F:libffmpeg.AVCodecContext.temporal_cplx_masking" decl="false" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavcodec\avcodec.h" line="1564">
temporary complexity masking (0-&gt; disabled)
- encoding: Set by user.
- decoding: unused

</member>
<member name="F:libffmpeg.AVCodecContext.spatial_cplx_masking" decl="false" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavcodec\avcodec.h" line="1571">
spatial complexity masking (0-&gt; disabled)
- encoding: Set by user.
- decoding: unused

</member>
<member name="F:libffmpeg.AVCodecContext.p_masking" decl="false" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavcodec\avcodec.h" line="1578">
p block masking (0-&gt; disabled)
- encoding: Set by user.
- decoding: unused

</member>
<member name="F:libffmpeg.AVCodecContext.dark_masking" decl="false" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavcodec\avcodec.h" line="1585">
darkness masking (0-&gt; disabled)
- encoding: Set by user.
- decoding: unused

</member>
<member name="F:libffmpeg.AVCodecContext.slice_count" decl="false" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavcodec\avcodec.h" line="1592">
slice count
- encoding: Set by libavcodec.
- decoding: Set by user (or 0).

</member>
<member name="F:libffmpeg.AVCodecContext.prediction_method" decl="false" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavcodec\avcodec.h" line="1598">
prediction method (needed for huffyuv)
- encoding: Set by user.
- decoding: unused

</member>
<member name="F:libffmpeg.AVCodecContext.slice_offset" decl="false" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavcodec\avcodec.h" line="1608">
slice offsets in the frame in bytes
- encoding: Set/allocated by libavcodec.
- decoding: Set/allocated by user (or NULL).

</member>
<member name="F:libffmpeg.AVCodecContext.sample_aspect_ratio" decl="false" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavcodec\avcodec.h" line="1615">
sample aspect ratio (0 if unknown)
That is the width of a pixel divided by the height of the pixel.
Numerator and denominator must be relatively prime and smaller than 256 for some video standards.
- encoding: Set by user.
- decoding: Set by libavcodec.

</member>
<member name="F:libffmpeg.AVCodecContext.me_cmp" decl="false" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavcodec\avcodec.h" line="1624">
motion estimation comparison function
- encoding: Set by user.
- decoding: unused

</member>
<member name="F:libffmpeg.AVCodecContext.me_sub_cmp" decl="false" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavcodec\avcodec.h" line="1630">
subpixel motion estimation comparison function
- encoding: Set by user.
- decoding: unused

</member>
<member name="F:libffmpeg.AVCodecContext.mb_cmp" decl="false" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavcodec\avcodec.h" line="1636">
macroblock comparison function (not supported yet)
- encoding: Set by user.
- decoding: unused

</member>
<member name="F:libffmpeg.AVCodecContext.ildct_cmp" decl="false" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavcodec\avcodec.h" line="1642">
interlaced DCT comparison function
- encoding: Set by user.
- decoding: unused

</member>
<!-- Discarding badly formed XML document comment for member 'F:libffmpeg.AVCodecContext.dia_size'. -->
<member name="F:libffmpeg.AVCodecContext.last_predictor_count" decl="false" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavcodec\avcodec.h" line="1672">
amount of previous MV predictors (2a+1 x 2a+1 square)
- encoding: Set by user.
- decoding: unused

</member>
<member name="F:libffmpeg.AVCodecContext.pre_me" decl="false" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavcodec\avcodec.h" line="1679">
prepass for motion estimation
- encoding: Set by user.
- decoding: unused

</member>
<member name="F:libffmpeg.AVCodecContext.me_pre_cmp" decl="false" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavcodec\avcodec.h" line="1686">
motion estimation prepass comparison function
- encoding: Set by user.
- decoding: unused

</member>
<!-- Discarding badly formed XML document comment for member 'F:libffmpeg.AVCodecContext.pre_dia_size'. -->
<member name="F:libffmpeg.AVCodecContext.me_subpel_quality" decl="false" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavcodec\avcodec.h" line="1700">
subpel ME quality
- encoding: Set by user.
- decoding: unused

</member>
<member name="F:libffmpeg.AVCodecContext.dtg_active_format" decl="false" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavcodec\avcodec.h" line="1708">
 DTG active format information (additional aspect ratio
 information only used in DVB MPEG-2 transport streams)
 0 if not set.

 - encoding: unused
 - decoding: Set by decoder.
 @deprecated Deprecated in favor of AVSideData

</member>
<member name="F:libffmpeg.AVCodecContext.me_range" decl="false" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavcodec\avcodec.h" line="1727">
 maximum motion estimation search range in subpel units
 If 0 then no limit.

 - encoding: Set by user.
 - decoding: unused

</member>
<member name="F:libffmpeg.AVCodecContext.intra_quant_bias" decl="false" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavcodec\avcodec.h" line="1736">
intra quantizer bias
- encoding: Set by user.
- decoding: unused

</member>
<member name="F:libffmpeg.AVCodecContext.inter_quant_bias" decl="false" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavcodec\avcodec.h" line="1744">
inter quantizer bias
- encoding: Set by user.
- decoding: unused

</member>
<member name="F:libffmpeg.AVCodecContext.slice_flags" decl="false" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavcodec\avcodec.h" line="1751">
slice flags
- encoding: unused
- decoding: Set by user.

</member>
<member name="F:libffmpeg.AVCodecContext.xvmc_acceleration" decl="false" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavcodec\avcodec.h" line="1762">
XVideo Motion Acceleration
- encoding: forbidden
- decoding: set by decoder
@deprecated XvMC doesn't need it anymore.

</member>
<member name="F:libffmpeg.AVCodecContext.mb_decision" decl="false" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavcodec\avcodec.h" line="1771">
macroblock decision mode
- encoding: Set by user.
- decoding: unused

</member>
<member name="F:libffmpeg.AVCodecContext.intra_matrix" decl="false" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavcodec\avcodec.h" line="1781">
custom intra quantization matrix
- encoding: Set by user, can be NULL.
- decoding: Set by libavcodec.

</member>
<member name="F:libffmpeg.AVCodecContext.inter_matrix" decl="false" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavcodec\avcodec.h" line="1788">
custom inter quantization matrix
- encoding: Set by user, can be NULL.
- decoding: Set by libavcodec.

</member>
<member name="F:libffmpeg.AVCodecContext.scenechange_threshold" decl="false" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavcodec\avcodec.h" line="1795">
scene change detection threshold
0 is default, larger means fewer detected scene changes.
- encoding: Set by user.
- decoding: unused

</member>
<member name="F:libffmpeg.AVCodecContext.noise_reduction" decl="false" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavcodec\avcodec.h" line="1803">
noise reduction strength
- encoding: Set by user.
- decoding: unused

</member>
<member name="F:libffmpeg.AVCodecContext.me_threshold" decl="false" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavcodec\avcodec.h" line="1811">
@deprecated this field is unused

</member>
<member name="F:libffmpeg.AVCodecContext.mb_threshold" decl="false" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavcodec\avcodec.h" line="1817">
@deprecated this field is unused

</member>
<member name="F:libffmpeg.AVCodecContext.intra_dc_precision" decl="false" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavcodec\avcodec.h" line="1824">
precision of the intra DC coefficient - 8
- encoding: Set by user.
- decoding: Set by libavcodec

</member>
<member name="F:libffmpeg.AVCodecContext.skip_top" decl="false" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavcodec\avcodec.h" line="1831">
Number of macroblock rows at the top which are skipped.
- encoding: unused
- decoding: Set by user.

</member>
<member name="F:libffmpeg.AVCodecContext.skip_bottom" decl="false" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavcodec\avcodec.h" line="1838">
Number of macroblock rows at the bottom which are skipped.
- encoding: unused
- decoding: Set by user.

</member>
<member name="F:libffmpeg.AVCodecContext.border_masking" decl="false" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavcodec\avcodec.h" line="1846">
@deprecated use encoder private options instead

</member>
<member name="F:libffmpeg.AVCodecContext.mb_lmin" decl="false" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavcodec\avcodec.h" line="1853">
minimum MB lagrange multipler
- encoding: Set by user.
- decoding: unused

</member>
<member name="F:libffmpeg.AVCodecContext.mb_lmax" decl="false" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavcodec\avcodec.h" line="1860">
maximum MB lagrange multipler
- encoding: Set by user.
- decoding: unused

</member>
<member name="F:libffmpeg.AVCodecContext.me_penalty_compensation" decl="false" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavcodec\avcodec.h" line="1867">

 - encoding: Set by user.
 - decoding: unused

</member>
<member name="F:libffmpeg.AVCodecContext.bidir_refine" decl="false" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavcodec\avcodec.h" line="1874">

 - encoding: Set by user.
 - decoding: unused

</member>
<member name="F:libffmpeg.AVCodecContext.brd_scale" decl="false" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavcodec\avcodec.h" line="1881">

 - encoding: Set by user.
 - decoding: unused

</member>
<member name="F:libffmpeg.AVCodecContext.keyint_min" decl="false" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavcodec\avcodec.h" line="1888">
minimum GOP size
- encoding: Set by user.
- decoding: unused

</member>
<member name="F:libffmpeg.AVCodecContext.refs" decl="false" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavcodec\avcodec.h" line="1895">
number of reference frames
- encoding: Set by user.
- decoding: Set by lavc.

</member>
<member name="F:libffmpeg.AVCodecContext.chromaoffset" decl="false" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavcodec\avcodec.h" line="1902">
chroma qp offset from luma
- encoding: Set by user.
- decoding: unused

</member>
<member name="F:libffmpeg.AVCodecContext.scenechange_factor" decl="false" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavcodec\avcodec.h" line="1910">
Multiplied by qscale for each frame and added to scene_change_score.
- encoding: Set by user.
- decoding: unused

</member>
<member name="F:libffmpeg.AVCodecContext.mv0_threshold" decl="false" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavcodec\avcodec.h" line="1918">

 Note: Value depends upon the compare function used for fullpel ME.
 - encoding: Set by user.
 - decoding: unused

</member>
<member name="F:libffmpeg.AVCodecContext.b_sensitivity" decl="false" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavcodec\avcodec.h" line="1926">
Adjust sensitivity of b_frame_strategy 1.
- encoding: Set by user.
- decoding: unused

</member>
<member name="T:libffmpeg.AVColorPrimaries" decl="true" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavcodec\avcodec.h" line="1933">
Chromaticity coordinates of the source primaries.
- encoding: Set by user
- decoding: Set by libavcodec

</member>
<member name="T:libffmpeg.AVColorTransferCharacteristic" decl="true" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavcodec\avcodec.h" line="1940">
Color Transfer Characteristic.
- encoding: Set by user
- decoding: Set by libavcodec

</member>
<member name="T:libffmpeg.AVColorSpace" decl="true" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavcodec\avcodec.h" line="1947">
YUV colorspace type.
- encoding: Set by user
- decoding: Set by libavcodec

</member>
<member name="T:libffmpeg.AVColorRange" decl="true" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavcodec\avcodec.h" line="1954">
MPEG vs JPEG YUV range.
- encoding: Set by user
- decoding: Set by libavcodec

</member>
<member name="T:libffmpeg.AVChromaLocation" decl="true" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavcodec\avcodec.h" line="1961">
This defines the location of chroma samples.
- encoding: Set by user
- decoding: Set by libavcodec

</member>
<member name="F:libffmpeg.AVCodecContext.slices" decl="false" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavcodec\avcodec.h" line="1968">
Number of slices.
Indicates number of picture subdivisions. Used for parallelized
decoding.
- encoding: Set by user
- decoding: unused

</member>
<member name="T:libffmpeg.AVFieldOrder" decl="true" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavcodec\avcodec.h" line="1977">
Field order
     * - encoding: set by libavcodec
     * - decoding: Set by user.

</member>
<member name="T:libffmpeg.AVSampleFormat" decl="true" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavcodec\avcodec.h" line="1987">
audio sample format
- encoding: Set by user.
- decoding: Set by libavcodec.

</member>
<member name="F:libffmpeg.AVCodecContext.frame_size" decl="false" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavcodec\avcodec.h" line="1995">
 Number of samples per channel in an audio frame.

 - encoding: set by libavcodec in avcodec_open2(). Each submitted frame
   except the last must contain exactly frame_size samples per channel.
   May be 0 when the codec has CODEC_CAP_VARIABLE_FRAME_SIZE set, then the
   frame size is not restricted.
 - decoding: may be set by some decoders to indicate constant frame size

</member>
<member name="F:libffmpeg.AVCodecContext.frame_number" decl="false" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavcodec\avcodec.h" line="2006">
 Frame counter, set by libavcodec.

 - decoding: total number of frames returned from the decoder so far.
 - encoding: total number of frames passed to the encoder so far.

   @note the counter is not incremented if encoding/decoding resulted in
   an error.

</member>
<member name="F:libffmpeg.AVCodecContext.block_align" decl="false" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavcodec\avcodec.h" line="2017">
number of bytes per packet if constant and known or 0
Used by some WAV based audio codecs.

</member>
<member name="F:libffmpeg.AVCodecContext.cutoff" decl="false" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavcodec\avcodec.h" line="2023">
Audio cutoff bandwidth (0 means "automatic")
- encoding: Set by user.
- decoding: unused

</member>
<member name="F:libffmpeg.AVCodecContext.request_channels" decl="false" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavcodec\avcodec.h" line="2031">
Decoder should decode to this many channels if it can (0 for default)
- encoding: unused
- decoding: Set by user.
@deprecated Deprecated in favor of request_channel_layout.

</member>
<member name="F:libffmpeg.AVCodecContext.channel_layout" decl="false" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavcodec\avcodec.h" line="2040">
Audio channel layout.
- encoding: set by user.
- decoding: set by user, may be overwritten by libavcodec.

</member>
<member name="F:libffmpeg.AVCodecContext.request_channel_layout" decl="false" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavcodec\avcodec.h" line="2047">
Request decoder to use this channel layout if it can (0 for default)
- encoding: unused
- decoding: Set by user.

</member>
<member name="T:libffmpeg.AVAudioServiceType" decl="true" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavcodec\avcodec.h" line="2054">
Type of service that the audio stream conveys.
- encoding: Set by user.
- decoding: Set by libavcodec.

</member>
<member name="T:libffmpeg.AVSampleFormat" decl="true" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavcodec\avcodec.h" line="2061">
desired sample format
- encoding: Not used.
- decoding: Set by user.
Decoder will decode to this format if it can.

</member>
<member name="F:libffmpeg.AVCodecContext.get_buffer" decl="false" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavcodec\avcodec.h" line="2070">
 Called at the beginning of each frame to get a buffer for it.

 The function will set AVFrame.data[], AVFrame.linesize[].
 AVFrame.extended_data[] must also be set, but it should be the same as
 AVFrame.data[] except for planar audio with more channels than can fit
 in AVFrame.data[]. In that case, AVFrame.data[] shall still contain as
 many data pointers as it can hold.

 if CODEC_CAP_DR1 is not set then get_buffer() must call
 avcodec_default_get_buffer() instead of providing buffers allocated by
 some other means.

 AVFrame.data[] should be 32- or 16-byte-aligned unless the CPU doesn't
 need it. avcodec_default_get_buffer() aligns the output buffer properly,
 but if get_buffer() is overridden then alignment considerations should
 be taken into account.

 @see avcodec_default_get_buffer()

 Video:

 If pic.reference is set then the frame will be read later by libavcodec.
 avcodec_align_dimensions2() should be used to find the required width and
 height, as they normally need to be rounded up to the next multiple of 16.

 If frame multithreading is used and thread_safe_callbacks is set,
 it may be called from a different thread, but not from more than one at
 once. Does not need to be reentrant.

 @see release_buffer(), reget_buffer()
 @see avcodec_align_dimensions2()

 Audio:

 Decoders request a buffer of a particular size by setting
 AVFrame.nb_samples prior to calling get_buffer(). The decoder may,
 however, utilize only part of the buffer by setting AVFrame.nb_samples
 to a smaller value in the output frame.

 Decoders cannot use the buffer after returning from
 avcodec_decode_audio4(), so they will not call release_buffer(), as it
 is assumed to be released immediately upon return. In some rare cases,
 a decoder may need to call get_buffer() more than once in a single
 call to avcodec_decode_audio4(). In that case, when get_buffer() is
 called again after it has already been called once, the previously
 acquired buffer is assumed to be released at that time and may not be
 reused by the decoder.

 As a convenience, av_samples_get_buffer_size() and
 av_samples_fill_arrays() in libavutil may be used by custom get_buffer()
 functions to find the required data size and to fill data pointers and
 linesize. In AVFrame.linesize, only linesize[0] may be set for audio
 since all planes must be the same size.

 @see av_samples_get_buffer_size(), av_samples_fill_arrays()

 - encoding: unused
 - decoding: Set by libavcodec, user can override.

 @deprecated use get_buffer2()

</member>
<member name="F:libffmpeg.AVCodecContext.release_buffer" decl="false" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavcodec\avcodec.h" line="2135">
 Called to release buffers which were allocated with get_buffer.
 A released buffer can be reused in get_buffer().
 pic.data[*] must be set to NULL.
 May be called from a different thread if frame multithreading is used,
 but not by more than one thread at once, so does not need to be reentrant.
 - encoding: unused
 - decoding: Set by libavcodec, user can override.

 @deprecated custom freeing callbacks should be set from get_buffer2()

</member>
<member name="F:libffmpeg.AVCodecContext.reget_buffer" decl="false" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavcodec\avcodec.h" line="2149">
Called at the beginning of a frame to get cr buffer for it.
Buffer type (size, hints) must be the same. libavcodec won't check it.
libavcodec will pass previous buffer in pic, function should return
same buffer or new buffer with old frame "painted" into it.
If pic.data[0] == NULL must behave like get_buffer().
if CODEC_CAP_DR1 is not set then reget_buffer() must call
avcodec_default_reget_buffer() instead of providing buffers allocated by
some other means.
- encoding: unused
- decoding: Set by libavcodec, user can override.

</member>
<member name="F:libffmpeg.AVCodecContext.get_buffer2" decl="false" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavcodec\avcodec.h" line="2165">
 This callback is called at the beginning of each frame to get data
 buffer(s) for it. There may be one contiguous buffer for all the data or
 there may be a buffer per each data plane or anything in between. What
 this means is, you may set however many entries in buf[] you feel necessary.
 Each buffer must be reference-counted using the AVBuffer API (see description
 of buf[] below).

 The following fields will be set in the frame before this callback is
 called:
 - format
 - width, height (video only)
 - sample_rate, channel_layout, nb_samples (audio only)
 Their values may differ from the corresponding values in
 AVCodecContext. This callback must use the frame values, not the codec
 context values, to calculate the required buffer size.

 This callback must fill the following fields in the frame:
 - data[]
 - linesize[]
 - extended_data:
   * if the data is planar audio with more than 8 channels, then this
     callback must allocate and fill extended_data to contain all pointers
     to all data planes. data[] must hold as many pointers as it can.
     extended_data must be allocated with av_malloc() and will be freed in
     av_frame_unref().
   * otherwise exended_data must point to data
 - buf[] must contain one or more pointers to AVBufferRef structures. Each of
   the frame's data and extended_data pointers must be contained in these. That
   is, one AVBufferRef for each allocated chunk of memory, not necessarily one
   AVBufferRef per data[] entry. See: av_buffer_create(), av_buffer_alloc(),
   and av_buffer_ref().
 - extended_buf and nb_extended_buf must be allocated with av_malloc() by
   this callback and filled with the extra buffers if there are more
   buffers than buf[] can hold. extended_buf will be freed in
   av_frame_unref().

 If CODEC_CAP_DR1 is not set then get_buffer2() must call
 avcodec_default_get_buffer2() instead of providing buffers allocated by
 some other means.

 Each data plane must be aligned to the maximum required by the target
 CPU.

 @see avcodec_default_get_buffer2()

 Video:

 If AV_GET_BUFFER_FLAG_REF is set in flags then the frame may be reused
 (read and/or written to if it is writable) later by libavcodec.

 avcodec_align_dimensions2() should be used to find the required width and
 height, as they normally need to be rounded up to the next multiple of 16.

 Some decoders do not support linesizes changing between frames.

 If frame multithreading is used and thread_safe_callbacks is set,
 this callback may be called from a different thread, but not from more
 than one at once. Does not need to be reentrant.

 @see avcodec_align_dimensions2()

 Audio:

 Decoders request a buffer of a particular size by setting
 AVFrame.nb_samples prior to calling get_buffer2(). The decoder may,
 however, utilize only part of the buffer by setting AVFrame.nb_samples
 to a smaller value in the output frame.

 As a convenience, av_samples_get_buffer_size() and
 av_samples_fill_arrays() in libavutil may be used by custom get_buffer2()
 functions to find the required data size and to fill data pointers and
 linesize. In AVFrame.linesize, only linesize[0] may be set for audio
 since all planes must be the same size.

 @see av_samples_get_buffer_size(), av_samples_fill_arrays()

 - encoding: unused
 - decoding: Set by libavcodec, user can override.

</member>
<member name="F:libffmpeg.AVCodecContext.refcounted_frames" decl="false" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavcodec\avcodec.h" line="2247">
 If non-zero, the decoded audio and video frames returned from
 avcodec_decode_video2() and avcodec_decode_audio4() are reference-counted
 and are valid indefinitely. The caller must free them with
 av_frame_unref() when they are not needed anymore.
 Otherwise, the decoded frames must not be freed by the caller and are
 only valid until the next decode call.

 - encoding: unused
 - decoding: set by the caller before avcodec_open2().

</member>
<member name="F:libffmpeg.AVCodecContext.qmin" decl="false" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavcodec\avcodec.h" line="2264">
minimum quantizer
- encoding: Set by user.
- decoding: unused

</member>
<member name="F:libffmpeg.AVCodecContext.qmax" decl="false" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavcodec\avcodec.h" line="2271">
maximum quantizer
- encoding: Set by user.
- decoding: unused

</member>
<member name="F:libffmpeg.AVCodecContext.max_qdiff" decl="false" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavcodec\avcodec.h" line="2278">
maximum quantizer difference between frames
- encoding: Set by user.
- decoding: unused

</member>
<member name="F:libffmpeg.AVCodecContext.rc_qsquish" decl="false" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavcodec\avcodec.h" line="2286">
@deprecated use encoder private options instead

</member>
<member name="F:libffmpeg.AVCodecContext.rc_buffer_size" decl="false" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavcodec\avcodec.h" line="2298">
decoder bitstream buffer size
- encoding: Set by user.
- decoding: unused

</member>
<member name="F:libffmpeg.AVCodecContext.rc_override_count" decl="false" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavcodec\avcodec.h" line="2305">
ratecontrol override, see RcOverride
- encoding: Allocated/set/freed by user.
- decoding: unused

</member>
<member name="F:libffmpeg.AVCodecContext.rc_eq" decl="false" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavcodec\avcodec.h" line="2314">
@deprecated use encoder private options instead

</member>
<member name="F:libffmpeg.AVCodecContext.rc_max_rate" decl="false" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavcodec\avcodec.h" line="2321">
maximum bitrate
- encoding: Set by user.
- decoding: Set by libavcodec.

</member>
<member name="F:libffmpeg.AVCodecContext.rc_min_rate" decl="false" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavcodec\avcodec.h" line="2328">
minimum bitrate
- encoding: Set by user.
- decoding: unused

</member>
<member name="F:libffmpeg.AVCodecContext.rc_buffer_aggressivity" decl="false" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavcodec\avcodec.h" line="2336">
@deprecated use encoder private options instead

</member>
<!-- Discarding badly formed XML document comment for member 'F:libffmpeg.AVCodecContext.rc_max_available_vbv_use'. -->
<!-- Discarding badly formed XML document comment for member 'F:libffmpeg.AVCodecContext.rc_min_vbv_overflow_use'. -->
<member name="F:libffmpeg.AVCodecContext.rc_initial_buffer_occupancy" decl="false" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavcodec\avcodec.h" line="2360">
Number of bits which should be loaded into the rc buffer before decoding starts.
- encoding: Set by user.
- decoding: unused

</member>
<member name="F:libffmpeg.AVCodecContext.coder_type" decl="false" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavcodec\avcodec.h" line="2374">
coder type
- encoding: Set by user.
- decoding: unused

</member>
<member name="F:libffmpeg.AVCodecContext.context_model" decl="false" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavcodec\avcodec.h" line="2381">
context model
- encoding: Set by user.
- decoding: unused

</member>
<member name="F:libffmpeg.AVCodecContext.lmin" decl="false" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavcodec\avcodec.h" line="2389">
@deprecated use encoder private options instead

</member>
<member name="F:libffmpeg.AVCodecContext.lmax" decl="false" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavcodec\avcodec.h" line="2395">
@deprecated use encoder private options instead

</member>
<member name="F:libffmpeg.AVCodecContext.frame_skip_threshold" decl="false" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavcodec\avcodec.h" line="2402">
frame skip threshold
- encoding: Set by user.
- decoding: unused

</member>
<member name="F:libffmpeg.AVCodecContext.frame_skip_factor" decl="false" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavcodec\avcodec.h" line="2409">
frame skip factor
- encoding: Set by user.
- decoding: unused

</member>
<member name="F:libffmpeg.AVCodecContext.frame_skip_exp" decl="false" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavcodec\avcodec.h" line="2416">
frame skip exponent
- encoding: Set by user.
- decoding: unused

</member>
<member name="F:libffmpeg.AVCodecContext.frame_skip_cmp" decl="false" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavcodec\avcodec.h" line="2423">
frame skip comparison function
- encoding: Set by user.
- decoding: unused

</member>
<member name="F:libffmpeg.AVCodecContext.trellis" decl="false" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavcodec\avcodec.h" line="2430">
trellis RD quantization
- encoding: Set by user.
- decoding: unused

</member>
<member name="F:libffmpeg.AVCodecContext.min_prediction_order" decl="false" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavcodec\avcodec.h" line="2437">
- encoding: Set by user.
- decoding: unused

</member>
<member name="F:libffmpeg.AVCodecContext.max_prediction_order" decl="false" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavcodec\avcodec.h" line="2443">
- encoding: Set by user.
- decoding: unused

</member>
<member name="F:libffmpeg.AVCodecContext.timecode_frame_start" decl="false" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavcodec\avcodec.h" line="2449">
GOP timecode frame start number
- encoding: Set by user, in non drop frame format
- decoding: Set by libavcodec (timecode in the 25 bits format, -1 if unset)

</member>
<member name="F:libffmpeg.AVCodecContext.frame_bits" decl="false" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavcodec\avcodec.h" line="2481">
number of bits used for the previously encoded frame
- encoding: Set by libavcodec.
- decoding: unused

</member>
<member name="F:libffmpeg.AVCodecContext.stats_out" decl="false" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavcodec\avcodec.h" line="2488">
pass1 encoding statistics output buffer
- encoding: Set by libavcodec.
- decoding: unused

</member>
<member name="F:libffmpeg.AVCodecContext.stats_in" decl="false" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavcodec\avcodec.h" line="2495">
pass2 encoding statistics input buffer
Concatenated stuff from stats_out of pass1 should be placed here.
- encoding: Allocated/set/freed by user.
- decoding: unused

</member>
<member name="F:libffmpeg.AVCodecContext.workaround_bugs" decl="false" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavcodec\avcodec.h" line="2503">
Work around bugs in encoders which sometimes cannot be detected automatically.
- encoding: Set by user
- decoding: Set by user

</member>
<member name="F:libffmpeg.AVCodecContext.strict_std_compliance" decl="false" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavcodec\avcodec.h" line="2530">
strictly follow the standard (MPEG4, ...).
- encoding: Set by user.
- decoding: Set by user.
Setting this to STRICT or higher means the encoder and decoder will
generally do stupid things, whereas setting it to unofficial or lower
will mean the encoder might produce output that is not supported by all
spec-compliant decoders. Decoders don't differentiate between normal,
unofficial and experimental (that is, they always try to decode things
when they can) unless they are explicitly asked to behave stupidly
(=strictly conform to the specs)

</member>
<member name="F:libffmpeg.AVCodecContext.error_concealment" decl="false" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavcodec\avcodec.h" line="2549">
error concealment flags
- encoding: unused
- decoding: Set by user.

</member>
<member name="F:libffmpeg.AVCodecContext.debug" decl="false" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavcodec\avcodec.h" line="2559">
debug
- encoding: Set by user.
- decoding: Set by user.

</member>
<member name="F:libffmpeg.AVCodecContext.debug_mv" decl="false" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavcodec\avcodec.h" line="2571">
@deprecated this option does nothing

debug
Code outside libavcodec should access this field using AVOptions
- encoding: Set by user.
- decoding: Set by user.

</member>
<member name="F:libffmpeg.AVCodecContext.err_recognition" decl="false" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavcodec\avcodec.h" line="2606">
Error recognition; may misdetect some more or less valid parts as errors.
- encoding: unused
- decoding: Set by user.

</member>
<member name="F:libffmpeg.AVCodecContext.reordered_opaque" decl="false" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavcodec\avcodec.h" line="2613">
Verify checksums embedded in the bitstream (could be of either encoded or
decoded data, depending on the codec) and print an error message on mismatch.
If AV_EF_EXPLODE is also set, a mismatching checksum will result in the
decoder returning an error.

opaque 64bit number (generally a PTS) that will be reordered and
output in AVFrame.reordered_opaque
- encoding: unused
- decoding: Set by user.

</member>
<member name="T:libffmpeg.AVHWAccel" decl="true" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavcodec\avcodec.h" line="2638">
Hardware accelerator in use
- encoding: unused.
- decoding: Set by libavcodec

</member>
<member name="F:libffmpeg.AVCodecContext.hwaccel_context" decl="false" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavcodec\avcodec.h" line="2645">
Hardware accelerator context.
For some hardware accelerators, a global context needs to be
provided by the user. In that case, this holds display-dependent
data FFmpeg cannot instantiate itself. Please refer to the
FFmpeg HW accelerator documentation to know how to fill this
is. e.g. for VA API, this is a struct vaapi_context.
- encoding: unused
- decoding: Set by user

</member>
<!-- Discarding badly formed XML document comment for member 'F:libffmpeg.AVCodecContext.error'. -->
<member name="F:libffmpeg.AVCodecContext.dct_algo" decl="false" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavcodec\avcodec.h" line="2664">
DCT algorithm, see FF_DCT_* below
- encoding: Set by user.
- decoding: unused

</member>
<member name="F:libffmpeg.AVCodecContext.idct_algo" decl="false" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavcodec\avcodec.h" line="2679">
IDCT algorithm, see FF_IDCT_* below.
- encoding: Set by user.
- decoding: Set by user.

</member>
<member name="F:libffmpeg.AVCodecContext.bits_per_coded_sample" decl="false" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavcodec\avcodec.h" line="2714">
bits per sample/pixel from the demuxer (needed for huffyuv).
- encoding: Set by libavcodec.
- decoding: Set by user.

</member>
<member name="F:libffmpeg.AVCodecContext.bits_per_raw_sample" decl="false" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavcodec\avcodec.h" line="2721">
Bits per sample/pixel of internal libavcodec pixel/sample format.
- encoding: set by user.
- decoding: set by libavcodec.

</member>
<member name="F:libffmpeg.AVCodecContext.lowres" decl="false" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavcodec\avcodec.h" line="2729">
low resolution decoding, 1-&gt; 1/2 size, 2-&gt;1/4 size
- encoding: unused
- decoding: Set by user.
Code outside libavcodec should access this field using:
av_codec_{get,set}_lowres(avctx)

</member>
<member name="F:libffmpeg.AVCodecContext.coded_frame" decl="false" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavcodec\avcodec.h" line="2739">
the picture in the bitstream
- encoding: Set by libavcodec.
- decoding: unused

</member>
<member name="F:libffmpeg.AVCodecContext.thread_count" decl="false" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavcodec\avcodec.h" line="2746">
thread count
is used to decide how many independent tasks should be passed to execute()
- encoding: Set by user.
- decoding: Set by user.

</member>
<member name="F:libffmpeg.AVCodecContext.thread_type" decl="false" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavcodec\avcodec.h" line="2754">
 Which multithreading methods to use.
 Use of FF_THREAD_FRAME will increase decoding delay by one frame per thread,
 so clients which cannot provide future frames should not use it.

 - encoding: Set by user, otherwise the default is used.
 - decoding: Set by user, otherwise the default is used.

</member>
<member name="F:libffmpeg.AVCodecContext.active_thread_type" decl="false" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavcodec\avcodec.h" line="2766">
Which multithreading methods are in use by the codec.
- encoding: Set by libavcodec.
- decoding: Set by libavcodec.

</member>
<member name="F:libffmpeg.AVCodecContext.thread_safe_callbacks" decl="false" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavcodec\avcodec.h" line="2773">
Set by the client if its custom get_buffer() callback can be called
synchronously from another thread, which allows faster multithreaded decoding.
draw_horiz_band() will be called from other threads regardless of this setting.
Ignored if the default get_buffer() is used.
- encoding: Set by user.
- decoding: Set by user.

</member>
<member name="F:libffmpeg.AVCodecContext.execute" decl="false" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavcodec\avcodec.h" line="2783">
The codec may call this to execute several independent things.
It will return only after finishing all tasks.
The user may replace this with some multithreaded implementation,
the default implementation will execute the parts serially.
@param count the number of things to execute
- encoding: Set by libavcodec, user can override.
- decoding: Set by libavcodec, user can override.

</member>
<!-- Discarding badly formed XML document comment for member 'F:libffmpeg.AVCodecContext.execute2'. -->
<member name="F:libffmpeg.AVCodecContext.thread_opaque" decl="false" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavcodec\avcodec.h" line="2815">
@deprecated this field should not be used from outside of lavc

</member>
<member name="F:libffmpeg.AVCodecContext.nsse_weight" decl="false" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavcodec\avcodec.h" line="2822">
noise vs. sse weight for the nsse comparison function
- encoding: Set by user.
- decoding: unused

</member>
<member name="F:libffmpeg.AVCodecContext.profile" decl="false" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavcodec\avcodec.h" line="2829">
profile
- encoding: Set by user.
- decoding: Set by libavcodec.

</member>
<member name="F:libffmpeg.AVCodecContext.level" decl="false" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavcodec\avcodec.h" line="2913">
level
- encoding: Set by user.
- decoding: Set by libavcodec.

</member>
<member name="T:libffmpeg.AVDiscard" decl="true" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavcodec\avcodec.h" line="2921">
Skip loop filtering for selected frames.
- encoding: unused
- decoding: Set by user.

</member>
<member name="T:libffmpeg.AVDiscard" decl="true" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavcodec\avcodec.h" line="2928">
Skip IDCT/dequantization for selected frames.
- encoding: unused
- decoding: Set by user.

</member>
<member name="T:libffmpeg.AVDiscard" decl="true" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavcodec\avcodec.h" line="2935">
Skip decoding for selected frames.
- encoding: unused
- decoding: Set by user.

</member>
<member name="F:libffmpeg.AVCodecContext.subtitle_header" decl="false" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavcodec\avcodec.h" line="2942">
Header containing style information for text subtitles.
For SUBTITLE_ASS subtitle type, it should contain the whole ASS
[Script Info] and [V4+ Styles] section, plus the [Events] line and
the Format line following. It shouldn't include any Dialogue line.
- encoding: Set/allocated/freed by user (before avcodec_open2())
- decoding: Set/allocated/freed by libavcodec (by avcodec_open2())

</member>
<member name="F:libffmpeg.AVCodecContext.error_rate" decl="false" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavcodec\avcodec.h" line="2954">
@deprecated use the 'error_rate' private AVOption of the mpegvideo
encoders

</member>
<member name="F:libffmpeg.AVCodecContext.pkt" decl="false" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavcodec\avcodec.h" line="2963">
@deprecated this field is not supposed to be accessed from outside lavc

</member>
<member name="F:libffmpeg.AVCodecContext.vbv_delay" decl="false" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavcodec\avcodec.h" line="2970">
VBV delay coded in the last frame (in periods of a 27 MHz clock).
Used for compliant TS muxing.
- encoding: Set by libavcodec.
- decoding: unused.

</member>
<member name="F:libffmpeg.AVCodecContext.side_data_only_packets" decl="false" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavcodec\avcodec.h" line="2978">
 Encoding only. Allow encoders to output packets that do not contain any
 encoded data, only side data.

 Some encoders need to output such packets, e.g. to update some stream
 parameters at the end of encoding.

 All callers are strongly recommended to set this option to 1 and update
 their code to deal with such packets, since this behaviour may become
 always enabled in the future (then this option will be deprecated and
 later removed). To avoid ABI issues when this happens, the callers should
 use AVOptions to set this field.

</member>
<member name="F:libffmpeg.AVCodecContext.initial_padding" decl="false" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavcodec\avcodec.h" line="2993">
 Audio only. The number of "priming" samples (padding) inserted by the
 encoder at the beginning of the audio. I.e. this number of leading
 decoded samples must be discarded by the caller to get the original audio
 without leading padding.

 - decoding: unused
 - encoding: Set by libavcodec. The timestamps on the output packets are
             adjusted by the encoder so that they always refer to the
             first sample of the data actually contained in the packet,
             including any added padding.  E.g. if the timebase is
             1/samplerate and the timestamp of the first input sample is
             0, the timestamp of the first output packet will be
             -initial_padding.

</member>
<member name="F:libffmpeg.AVCodecContext.framerate" decl="false" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavcodec\avcodec.h" line="3010">
- decoding: For codecs that store a framerate value in the compressed
            bitstream, the decoder may export it here. { 0, 1} when
            unknown.
- encoding: unused

</member>
<member name="T:libffmpeg.AVPixelFormat" decl="true" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavcodec\avcodec.h" line="3018">
Nominal unaccelerated pixel format, see AV_PIX_FMT_xxx.
- encoding: unused.
- decoding: Set by libavcodec before calling get_format()

</member>
<member name="F:libffmpeg.AVCodecContext.pkt_timebase" decl="false" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavcodec\avcodec.h" line="3025">
Timebase in which pkt_dts/pts and AVPacket.dts/pts are.
Code outside libavcodec should access this field using:
av_codec_{get,set}_pkt_timebase(avctx)
- encoding unused.
- decoding set by user.

</member>
<member name="F:libffmpeg.AVCodecContext.codec_descriptor" decl="false" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavcodec\avcodec.h" line="3034">
AVCodecDescriptor
Code outside libavcodec should access this field using:
av_codec_{get,set}_codec_descriptor(avctx)
- encoding: unused.
- decoding: set by libavcodec.

</member>
<member name="F:libffmpeg.AVCodecContext.pts_correction_num_faulty_pts" decl="false" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavcodec\avcodec.h" line="3054">
Current statistics for PTS correction.
- decoding: maintained and used by libavcodec, not intended to be used by user apps
- encoding: unused

</member>
<member name="F:libffmpeg.AVCodecContext.sub_charenc" decl="false" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavcodec\avcodec.h" line="3064">
Character encoding of the input subtitles file.
- decoding: set by user
- encoding: unused

</member>
<member name="F:libffmpeg.AVCodecContext.sub_charenc_mode" decl="false" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavcodec\avcodec.h" line="3071">
Subtitles character encoding mode. Formats or codecs might be adjusting
this setting (if they are doing the conversion themselves for instance).
- decoding: set by libavcodec
- encoding: unused

</member>
<member name="F:libffmpeg.AVCodecContext.skip_alpha" decl="false" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavcodec\avcodec.h" line="3082">
 Skip processing alpha if supported by codec.
 Note that if the format uses pre-multiplied alpha (common with VP6,
 and recommended due to better video quality/compression)
 the image will look as if alpha-blended onto a black background.
 However for formats that do not use pre-multiplied alpha
 there might be serious artefacts (though e.g. libswscale currently
 assumes pre-multiplied alpha anyway).
 Code outside libavcodec should access this field using AVOptions

 - decoding: set by user
 - encoding: unused

</member>
<member name="F:libffmpeg.AVCodecContext.seek_preroll" decl="false" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavcodec\avcodec.h" line="3097">
Number of samples to skip after a discontinuity
- decoding: unused
- encoding: set by libavcodec

</member>
<member name="F:libffmpeg.AVCodecContext.chroma_intra_matrix" decl="false" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavcodec\avcodec.h" line="3117">
custom intra quantization matrix
Code outside libavcodec should access this field using av_codec_g/set_chroma_intra_matrix()
- encoding: Set by user, can be NULL.
- decoding: unused.

</member>
<member name="F:libffmpeg.AVCodecContext.dump_separator" decl="false" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavcodec\avcodec.h" line="3125">
dump format separator.
can be ", " or "\n      " or anything else
Code outside libavcodec should access this field using AVOptions
(NO direct access).
- encoding: Set by user.
- decoding: Set by user.

</member>
<member name="F:libffmpeg.AVCodecContext.codec_whitelist" decl="false" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavcodec\avcodec.h" line="3135">
',' separated list of allowed decoders.
If NULL then all are allowed
- encoding: unused
- decoding: set by user through AVOPtions (NO direct access)

</member>
<member name="T:libffmpeg.AVProfile" decl="false" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavcodec\avcodec.h" line="3159">
AVProfile.

</member>
<member name="T:libffmpeg.AVCodec" decl="false" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavcodec\avcodec.h" line="3171">
AVCodec.

</member>
<member name="F:libffmpeg.AVCodec.name" decl="false" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavcodec\avcodec.h" line="3175">
Name of the codec implementation.
The name is globally unique among encoders and among decoders (but an
encoder and a decoder can share the same name).
This is the primary way to find a codec from the user perspective.

</member>
<member name="F:libffmpeg.AVCodec.long_name" decl="false" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavcodec\avcodec.h" line="3182">
Descriptive name for the codec, meant to be more human readable than name.
You should use the NULL_IF_CONFIG_SMALL() macro to define it.

</member>
<member name="F:libffmpeg.AVCodec.capabilities" decl="false" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavcodec\avcodec.h" line="3189">
Codec capabilities.
see CODEC_CAP_*

</member>
<member name="F:libffmpeg.AVCodec.init_thread_copy" decl="false" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavcodec\avcodec.h" line="3214">
@name Frame-level threading support functions
@{

If defined, called on thread contexts when they are created.
If the codec allocates writable tables in init(), re-allocate them here.
priv_data will be set to a copy of the original.

</member>
<member name="F:libffmpeg.AVCodec.update_thread_context" decl="false" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavcodec\avcodec.h" line="3224">
 Copy necessary context variables from a previous thread context to the current one.
 If not defined, the next thread will start automatically; otherwise, the codec
 must call ff_thread_finish_setup().

 dst and src will (rarely) point to the same context, in which case memcpy should be skipped.

</member>
<member name="F:libffmpeg.AVCodec.defaults" decl="false" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavcodec\avcodec.h" line="3232">
@} 
Private codec-specific defaults.

</member>
<member name="F:libffmpeg.AVCodec.init_static_data" decl="false" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavcodec\avcodec.h" line="3239">
Initialize codec static data, called from avcodec_register().

</member>
<member name="F:libffmpeg.AVCodec.encode2" decl="false" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavcodec\avcodec.h" line="3247">
 Encode data to an AVPacket.

 @param      avctx          codec context
 @param      avpkt          output AVPacket (may contain a user-provided buffer)
 @param[in]  frame          AVFrame containing the raw data to be encoded
 @param[out] got_packet_ptr encoder sets to 0 or 1 to indicate that a
                            non-empty packet was returned in avpkt.
 @return 0 on success, negative error code on failure

</member>
<member name="F:libffmpeg.AVCodec.flush" decl="false" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavcodec\avcodec.h" line="3261">
Flush buffers.
Will be called when seeking

</member>
<member name="F:libffmpeg.AVCodec.caps_internal" decl="false" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavcodec\avcodec.h" line="3266">
Internal codec capabilities.
See FF_CODEC_CAP_* in internal.h

</member>
<member name="T:libffmpeg.AVHWAccel" decl="false" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavcodec\avcodec.h" line="3277">
@defgroup lavc_hwaccel AVHWAccel
@{

</member>
<member name="F:libffmpeg.AVHWAccel.name" decl="false" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavcodec\avcodec.h" line="3282">
Name of the hardware accelerated codec.
The name is globally unique among encoders and among decoders (but an
encoder and a decoder can share the same name).

</member>
<member name="T:libffmpeg.AVMediaType" decl="true" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavcodec\avcodec.h" line="3289">
 Type of codec implemented by the hardware accelerator.

 See AVMEDIA_TYPE_xxx

</member>
<member name="T:libffmpeg.AVCodecID" decl="true" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavcodec\avcodec.h" line="3296">
 Codec implemented by the hardware accelerator.

 See AV_CODEC_ID_xxx

</member>
<member name="T:libffmpeg.AVPixelFormat" decl="true" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavcodec\avcodec.h" line="3303">
 Supported pixel format.

 Only hardware accelerated formats are supported here.

</member>
<member name="F:libffmpeg.AVHWAccel.capabilities" decl="false" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavcodec\avcodec.h" line="3310">
Hardware accelerated codec capabilities.
see FF_HWACCEL_CODEC_CAP_*

</member>
<member name="F:libffmpeg.AVHWAccel.alloc_frame" decl="false" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavcodec\avcodec.h" line="3325">
Allocate a custom buffer

</member>
<member name="F:libffmpeg.AVHWAccel.start_frame" decl="false" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavcodec\avcodec.h" line="3330">
 Called at the beginning of each frame or field picture.

 Meaningful frame information (codec specific) is guaranteed to
 be parsed at this point. This function is mandatory.

 Note that buf can be NULL along with buf_size set to 0.
 Otherwise, this means the whole frame is available at this point.

 @param avctx the codec context
 @param buf the frame data buffer base
 @param buf_size the size of the frame in bytes
 @return zero if successful, a negative value otherwise

</member>
<member name="F:libffmpeg.AVHWAccel.decode_slice" decl="false" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavcodec\avcodec.h" line="3346">
 Callback for each slice.

 Meaningful slice information (codec specific) is guaranteed to
 be parsed at this point. This function is mandatory.
 The only exception is XvMC, that works on MB level.

 @param avctx the codec context
 @param buf the slice data buffer base
 @param buf_size the size of the slice in bytes
 @return zero if successful, a negative value otherwise

</member>
<member name="F:libffmpeg.AVHWAccel.end_frame" decl="false" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavcodec\avcodec.h" line="3360">
 Called at the end of each frame or field picture.

 The whole picture is parsed at this point and can now be sent
 to the hardware accelerator. This function is mandatory.

 @param avctx the codec context
 @return zero if successful, a negative value otherwise

</member>
<member name="F:libffmpeg.AVHWAccel.frame_priv_data_size" decl="false" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavcodec\avcodec.h" line="3371">
 Size of per-frame hardware accelerator private data.

 Private data is allocated with av_mallocz() before
 AVCodecContext.get_buffer() and deallocated after
 AVCodecContext.release_buffer().

</member>
<member name="F:libffmpeg.AVHWAccel.decode_mb" decl="false" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavcodec\avcodec.h" line="3380">
 Called for every Macroblock in a slice.

 XvMC uses it to replace the ff_mpv_decode_mb().
 Instead of decoding to raw picture, MB parameters are
 stored in an array provided by the video driver.

 @param s the mpeg context

</member>
<member name="F:libffmpeg.AVHWAccel.init" decl="false" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavcodec\avcodec.h" line="3391">
 Initialize the hwaccel private data.

 This will be called from ff_get_format(), after hwaccel and
 hwaccel_context are set and the hwaccel private data in AVCodecInternal
 is allocated.

</member>
<member name="F:libffmpeg.AVHWAccel.uninit" decl="false" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavcodec\avcodec.h" line="3400">
 Uninitialize the hwaccel private data.

 This will be called from get_format() or avcodec_close(), after hwaccel
 and hwaccel_context are already uninitialized.

</member>
<member name="F:libffmpeg.AVHWAccel.priv_data_size" decl="false" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavcodec\avcodec.h" line="3408">
Size of the private data to allocate in
AVCodecInternal.hwaccel_priv_data.

</member>
<member name="T:libffmpeg.AVPicture" decl="false" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavcodec\avcodec.h" line="3415">
Hardware acceleration should be used for decoding even if the codec level
used is unknown or higher than the maximum supported level reported by the
hardware driver.

Hardware acceleration can output YUV pixel formats with a different chroma
sampling than 4:2:0 and/or other than 8 bits per component.

@}

 @defgroup lavc_picture AVPicture

 Functions for working with AVPicture
 @{

 Picture data structure.

 Up to four components can be stored into it, the last component is
 alpha.

</member>
<member name="F:SUBTITLE_TEXT" decl="false" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavcodec\avcodec.h" line="3459">
Plain text, the text field must be set by the decoder and is
authoritative. ass and pict fields may contain approximations.

</member>
<member name="F:SUBTITLE_ASS" decl="false" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavcodec\avcodec.h" line="3465">
Formatted text, the ass field must be set by the decoder and is
authoritative. pict and text fields may contain approximations.

</member>
<member name="T:libffmpeg.AVSubtitleType" decl="false" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavcodec\avcodec.h" line="3450">
@}

</member>
<member name="F:libffmpeg.AVSubtitleRect.pict" decl="false" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavcodec\avcodec.h" line="3481">
data+linesize for the bitmap of this subtitle.
can be set for text/ass as well once they are rendered

</member>
<member name="F:libffmpeg.AVSubtitleRect.ass" decl="false" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavcodec\avcodec.h" line="3490">
0 terminated ASS/SSA compatible event line.
The presentation of this is unaffected by the other values in this
struct.

</member>
<member name="M:libffmpeg.av_codec_next(libffmpeg.AVCodec!System.Runtime.CompilerServices.IsConst*)" decl="true" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavcodec\avcodec.h" line="3509">
If c is NULL, returns the first registered codec,
if c is non-NULL, returns the next registered codec after c,
or NULL if c is the last one.

</member>
<member name="M:libffmpeg.avcodec_version" decl="true" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavcodec\avcodec.h" line="3516">
Return the LIBAVCODEC_VERSION_INT constant.

</member>
<member name="M:libffmpeg.avcodec_configuration" decl="true" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavcodec\avcodec.h" line="3521">
Return the libavcodec build-time configuration.

</member>
<member name="M:libffmpeg.avcodec_license" decl="true" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavcodec\avcodec.h" line="3526">
Return the libavcodec license.

</member>
<member name="M:libffmpeg.avcodec_register(libffmpeg.AVCodec*)" decl="true" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavcodec\avcodec.h" line="3531">
 Register the codec codec and initialize libavcodec.

 @warning either this function or avcodec_register_all() must be called
 before any other libavcodec functions.

 @see avcodec_register_all()

</member>
<member name="M:libffmpeg.avcodec_register_all" decl="true" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavcodec\avcodec.h" line="3541">
 Register all the codecs, parsers and bitstream filters which were enabled at
 configuration time. If you do not call this function you can select exactly
 which formats you want to support, by using the individual registration
 functions.

 @see avcodec_register
 @see av_register_codec_parser
 @see av_register_bitstream_filter

</member>
<member name="M:libffmpeg.avcodec_alloc_context3(libffmpeg.AVCodec!System.Runtime.CompilerServices.IsConst*)" decl="true" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavcodec\avcodec.h" line="3553">
 Allocate an AVCodecContext and set its fields to default values. The
 resulting struct should be freed with avcodec_free_context().

 @param codec if non-NULL, allocate private data and initialize defaults
              for the given codec. It is illegal to then call avcodec_open2()
              with a different codec.
              If NULL, then the codec-specific defaults won't be initialized,
              which may result in suboptimal default settings (this is
              important mainly for encoders, e.g. libx264).

 @return An AVCodecContext filled with default values or NULL on failure.
 @see avcodec_get_context_defaults

</member>
<member name="M:libffmpeg.avcodec_free_context(libffmpeg.AVCodecContext**)" decl="true" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavcodec\avcodec.h" line="3569">
Free the codec context and everything associated with it and write NULL to
the provided pointer.

</member>
<member name="M:libffmpeg.avcodec_get_context_defaults3(libffmpeg.AVCodecContext*,libffmpeg.AVCodec!System.Runtime.CompilerServices.IsConst*)" decl="true" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavcodec\avcodec.h" line="3575">
 Set the fields of the given AVCodecContext to default values corresponding
 to the given codec (defaults may be codec-dependent).

 Do not call this function if a non-NULL codec has been passed
 to avcodec_alloc_context3() that allocated this AVCodecContext.
 If codec is non-NULL, it is illegal to call avcodec_open2() with a
 different codec on this AVCodecContext.

</member>
<member name="M:libffmpeg.avcodec_get_class" decl="true" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavcodec\avcodec.h" line="3586">
 Get the AVClass for AVCodecContext. It can be used in combination with
 AV_OPT_SEARCH_FAKE_OBJ for examining options.

 @see av_opt_find().

</member>
<member name="M:libffmpeg.avcodec_get_frame_class" decl="true" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavcodec\avcodec.h" line="3594">
 Get the AVClass for AVFrame. It can be used in combination with
 AV_OPT_SEARCH_FAKE_OBJ for examining options.

 @see av_opt_find().

</member>
<member name="M:libffmpeg.avcodec_get_subtitle_rect_class" decl="true" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavcodec\avcodec.h" line="3602">
 Get the AVClass for AVSubtitleRect. It can be used in combination with
 AV_OPT_SEARCH_FAKE_OBJ for examining options.

 @see av_opt_find().

</member>
<member name="M:libffmpeg.avcodec_copy_context(libffmpeg.AVCodecContext*,libffmpeg.AVCodecContext!System.Runtime.CompilerServices.IsConst*)" decl="true" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavcodec\avcodec.h" line="3610">
 Copy the settings of the source AVCodecContext into the destination
 AVCodecContext. The resulting destination codec context will be
 unopened, i.e. you are required to call avcodec_open2() before you
 can use this AVCodecContext to decode/encode video/audio data.

 @param dest target codec context, should be initialized with
             avcodec_alloc_context3(NULL), but otherwise uninitialized
 @param src source codec context
 @return AVERROR() on error (e.g. memory allocation error), 0 on success

</member>
<member name="M:libffmpeg.avcodec_alloc_frame" decl="true" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavcodec\avcodec.h" line="3624">
@deprecated use av_frame_alloc()

</member>
<member name="M:libffmpeg.avcodec_get_frame_defaults(libffmpeg.AVFrame*)" decl="true" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavcodec\avcodec.h" line="3630">
 Set the fields of the given AVFrame to default values.

 @param frame The AVFrame of which the fields should be set to default values.

 @deprecated use av_frame_unref()

</member>
<member name="M:libffmpeg.avcodec_free_frame(libffmpeg.AVFrame**)" decl="true" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavcodec\avcodec.h" line="3640">
 Free the frame and any dynamically allocated objects in it,
 e.g. extended_data.

 @param frame frame to be freed. The pointer will be set to NULL.

 @warning this function does NOT free the data buffers themselves
 (it does not know how, since they might have been allocated with
  a custom get_buffer()).

 @deprecated use av_frame_free()

</member>
<!-- Discarding badly formed XML document comment for member 'M:libffmpeg.avcodec_open2(libffmpeg.AVCodecContext*,libffmpeg.AVCodec!System.Runtime.CompilerServices.IsConst*,libffmpeg.AVDictionary**)'. -->
<member name="M:libffmpeg.avcodec_close(libffmpeg.AVCodecContext*)" decl="true" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavcodec\avcodec.h" line="3697">
 Close a given AVCodecContext and free all the data associated with it
 (but not the AVCodecContext itself).

 Calling this function on an AVCodecContext that hasn't been opened will free
 the codec-specific data allocated in avcodec_alloc_context3() /
 avcodec_get_context_defaults3() with a non-NULL codec. Subsequent calls will
 do nothing.

</member>
<member name="M:libffmpeg.avsubtitle_free(libffmpeg.AVSubtitle*)" decl="true" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavcodec\avcodec.h" line="3708">
 Free all allocated data in the given subtitle struct.

 @param sub AVSubtitle to free.

</member>
<member name="M:libffmpeg.av_destruct_packet(libffmpeg.AVPacket*)" decl="true" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavcodec\avcodec.h" line="3715">
@}

@addtogroup lavc_packet
@{

Default packet destructor.
@deprecated use the AVBuffer API instead

</member>
<member name="M:libffmpeg.av_init_packet(libffmpeg.AVPacket*)" decl="true" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavcodec\avcodec.h" line="3733">
 Initialize optional fields of a packet with default values.

 Note, this does not touch the data and size members, which have to be
 initialized separately.

 @param pkt packet

</member>
<member name="M:libffmpeg.av_new_packet(libffmpeg.AVPacket*,System.Int32)" decl="true" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavcodec\avcodec.h" line="3743">
 Allocate the payload of a packet and initialize its fields with
 default values.

 @param pkt packet
 @param size wanted payload size
 @return 0 if OK, AVERROR_xxx otherwise

</member>
<member name="M:libffmpeg.av_shrink_packet(libffmpeg.AVPacket*,System.Int32)" decl="true" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavcodec\avcodec.h" line="3753">
 Reduce packet size, correctly zeroing padding

 @param pkt packet
 @param size new size

</member>
<member name="M:libffmpeg.av_grow_packet(libffmpeg.AVPacket*,System.Int32)" decl="true" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavcodec\avcodec.h" line="3761">
 Increase packet size, correctly zeroing padding

 @param pkt packet
 @param grow_by number of bytes by which to increase the size of the packet

</member>
<member name="M:libffmpeg.av_packet_from_data(libffmpeg.AVPacket*,System.Byte*,System.Int32)" decl="true" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavcodec\avcodec.h" line="3769">
 Initialize a reference-counted packet from av_malloc()ed data.

 @param pkt packet to be initialized. This function will set the data, size,
        buf and destruct fields, all others are left untouched.
 @param data Data allocated by av_malloc() to be used as packet data. If this
        function returns successfully, the data is owned by the underlying AVBuffer.
        The caller may not access the data through other means.
 @param size size of data in bytes, without the padding. I.e. the full buffer
        size is assumed to be size + FF_INPUT_BUFFER_PADDING_SIZE.

 @return 0 on success, a negative AVERROR on error

</member>
<member name="M:libffmpeg.av_dup_packet(libffmpeg.AVPacket*)" decl="true" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavcodec\avcodec.h" line="3784">
@warning This is a hack - the packet memory allocation stuff is broken. The
packet is allocated if it was not really allocated.

</member>
<member name="M:libffmpeg.av_copy_packet(libffmpeg.AVPacket*,libffmpeg.AVPacket!System.Runtime.CompilerServices.IsConst*)" decl="true" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavcodec\avcodec.h" line="3790">
 Copy packet, including contents

 @return 0 on success, negative AVERROR on fail

</member>
<member name="M:libffmpeg.av_copy_packet_side_data(libffmpeg.AVPacket*,libffmpeg.AVPacket!System.Runtime.CompilerServices.IsConst*)" decl="true" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavcodec\avcodec.h" line="3797">
 Copy packet side data

 @return 0 on success, negative AVERROR on fail

</member>
<member name="M:libffmpeg.av_free_packet(libffmpeg.AVPacket*)" decl="true" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavcodec\avcodec.h" line="3804">
 Free a packet.

 @param pkt packet to free

</member>
<member name="M:libffmpeg.av_packet_new_side_data(libffmpeg.AVPacket*,libffmpeg.AVPacketSideDataType,System.Int32)" decl="true" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavcodec\avcodec.h" line="3811">
 Allocate new information of a packet.

 @param pkt packet
 @param type side information type
 @param size side information size
 @return pointer to fresh allocated data or NULL otherwise

</member>
<!-- Discarding badly formed XML document comment for member 'M:libffmpeg.av_packet_shrink_side_data(libffmpeg.AVPacket*,libffmpeg.AVPacketSideDataType,System.Int32)'. -->
<member name="M:libffmpeg.av_packet_get_side_data(libffmpeg.AVPacket*,libffmpeg.AVPacketSideDataType,System.Int32*)" decl="true" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavcodec\avcodec.h" line="3833">
 Get side information from packet.

 @param pkt packet
 @param type desired side information type
 @param size pointer for side information size to store (optional)
 @return pointer to data if present or NULL otherwise

</member>
<member name="M:libffmpeg.av_packet_pack_dictionary(libffmpeg.AVDictionary*,System.Int32*)" decl="true" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavcodec\avcodec.h" line="3848">
 Pack a dictionary for use in side_data.

 @param dict The dictionary to pack.
 @param size pointer to store the size of the returned data
 @return pointer to data if successful, NULL otherwise

</member>
<!-- Discarding badly formed XML document comment for member 'M:libffmpeg.av_packet_unpack_dictionary(System.Byte!System.Runtime.CompilerServices.IsConst*,System.Int32,libffmpeg.AVDictionary**)'. -->
<member name="M:libffmpeg.av_packet_free_side_data(libffmpeg.AVPacket*)" decl="true" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavcodec\avcodec.h" line="3867">
 Convenience function to free all the side data stored.
 All the other fields stay untouched.

 @param pkt packet

</member>
<member name="M:libffmpeg.av_packet_ref(libffmpeg.AVPacket*,libffmpeg.AVPacket!System.Runtime.CompilerServices.IsConst*)" decl="true" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavcodec\avcodec.h" line="3875">
 Setup a new reference to the data described by a given packet

 If src is reference-counted, setup dst as a new reference to the
 buffer in src. Otherwise allocate a new buffer in dst and copy the
 data from src into it.

 All the other fields are copied from src.

 @see av_packet_unref

 @param dst Destination packet
 @param src Source packet

 @return 0 on success, a negative AVERROR on error.

</member>
<member name="M:libffmpeg.av_packet_unref(libffmpeg.AVPacket*)" decl="true" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavcodec\avcodec.h" line="3893">
 Wipe the packet.

 Unreference the buffer referenced by the packet and reset the
 remaining packet fields to their default values.

 @param pkt The packet to be unreferenced.

</member>
<member name="M:libffmpeg.av_packet_move_ref(libffmpeg.AVPacket*,libffmpeg.AVPacket*)" decl="true" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavcodec\avcodec.h" line="3903">
 Move every field in src to dst and reset src.

 @see av_packet_unref

 @param src Source packet, will be reset
 @param dst Destination packet

</member>
<member name="M:libffmpeg.av_packet_copy_props(libffmpeg.AVPacket*,libffmpeg.AVPacket!System.Runtime.CompilerServices.IsConst*)" decl="true" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavcodec\avcodec.h" line="3913">
 Copy only "properties" fields from src to dst.

 Properties for the purpose of this function are all the fields
 beside those related to the packet data (buf, data, size)

 @param dst Destination packet
 @param src Source packet

 @return 0 on success AVERROR on failure.


</member>
<member name="M:libffmpeg.av_packet_rescale_ts(libffmpeg.AVPacket*,libffmpeg.AVRational,libffmpeg.AVRational)" decl="true" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavcodec\avcodec.h" line="3927">
 Convert valid timing fields (timestamps / durations) in a packet from one
 timebase to another. Timestamps with unknown values (AV_NOPTS_VALUE) will be
 ignored.

 @param pkt packet on which the conversion will be performed
 @param tb_src source timebase, in which the timing fields in pkt are
               expressed
 @param tb_dst destination timebase, to which the timing fields will be
               converted

</member>
<member name="M:libffmpeg.avcodec_find_decoder(libffmpeg.AVCodecID)" decl="true" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavcodec\avcodec.h" line="3940">
@}

@addtogroup lavc_decoding
@{

 Find a registered decoder with a matching codec ID.

 @param id AVCodecID of the requested decoder
 @return A decoder if one was found, NULL otherwise.

</member>
<member name="M:libffmpeg.avcodec_find_decoder_by_name(System.SByte!System.Runtime.CompilerServices.IsSignUnspecifiedByte!System.Runtime.CompilerServices.IsConst*)" decl="true" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavcodec\avcodec.h" line="3957">
 Find a registered decoder with the specified name.

 @param name name of the requested decoder
 @return A decoder if one was found, NULL otherwise.

</member>
<member name="M:libffmpeg.avcodec_default_get_buffer2(libffmpeg.AVCodecContext*,libffmpeg.AVFrame*,System.Int32)" decl="true" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavcodec\avcodec.h" line="3971">
The default callback for AVCodecContext.get_buffer2(). It is made public so
it can be called by custom get_buffer2() implementations for decoders without
CODEC_CAP_DR1 set.

</member>
<member name="M:libffmpeg.avcodec_get_edge_width" decl="true" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavcodec\avcodec.h" line="3979">
 Return the amount of padding in pixels which the get_buffer callback must
 provide around the edge of the image for codecs which do not have the
 CODEC_FLAG_EMU_EDGE flag.

 @return Required padding in pixels.

 @deprecated CODEC_FLAG_EMU_EDGE is deprecated, so this function is no longer
 needed

</member>
<member name="M:libffmpeg.avcodec_align_dimensions(libffmpeg.AVCodecContext*,System.Int32*,System.Int32*)" decl="true" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavcodec\avcodec.h" line="3993">
 Modify width and height values so that they will result in a memory
 buffer that is acceptable for the codec if you do not use any horizontal
 padding.

 May only be used if a codec with CODEC_CAP_DR1 has been opened.

</member>
<member name="M:libffmpeg.avcodec_align_dimensions2(libffmpeg.AVCodecContext*,System.Int32*,System.Int32*,System.Int32*)" decl="true" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavcodec\avcodec.h" line="4002">
 Modify width and height values so that they will result in a memory
 buffer that is acceptable for the codec if you also ensure that all
 line sizes are a multiple of the respective linesize_align[i].

 May only be used if a codec with CODEC_CAP_DR1 has been opened.

</member>
<member name="M:libffmpeg.avcodec_enum_to_chroma_pos(System.Int32*,System.Int32*,libffmpeg.AVChromaLocation)" decl="true" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavcodec\avcodec.h" line="4012">
 Converts AVChromaLocation to swscale x/y chroma position.

 The positions represent the chroma (0,0) position in a coordinates system
 with luma (0,0) representing the origin and luma(1,1) representing 256,256

 @param xpos  horizontal chroma sample position
 @param ypos  vertical   chroma sample position

</member>
<member name="T:libffmpeg.AVChromaLocation" decl="true" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavcodec\avcodec.h" line="4023">
 Converts swscale x/y chroma position to AVChromaLocation.

 The positions represent the chroma (0,0) position in a coordinates system
 with luma (0,0) representing the origin and luma(1,1) representing 256,256

 @param xpos  horizontal chroma sample position
 @param ypos  vertical   chroma sample position

</member>
<member name="M:libffmpeg.avcodec_decode_audio3(libffmpeg.AVCodecContext*,System.Int16*,System.Int32*,libffmpeg.AVPacket*)" decl="true" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavcodec\avcodec.h" line="4035">
 Wrapper function which calls avcodec_decode_audio4.

 @deprecated Use avcodec_decode_audio4 instead.

 Decode the audio frame of size avpkt-&gt;size from avpkt-&gt;data into samples.
 Some decoders may support multiple frames in a single AVPacket, such
 decoders would then just decode the first frame. In this case,
 avcodec_decode_audio3 has to be called again with an AVPacket that contains
 the remaining data in order to decode the second frame etc.
 If no frame
 could be outputted, frame_size_ptr is zero. Otherwise, it is the
 decompressed frame size in bytes.

 @warning You must set frame_size_ptr to the allocated size of the
 output buffer before calling avcodec_decode_audio3().

 @warning The input buffer must be FF_INPUT_BUFFER_PADDING_SIZE larger than
 the actual read bytes because some optimized bitstream readers read 32 or 64
 bits at once and could read over the end.

 @warning The end of the input buffer avpkt-&gt;data should be set to 0 to ensure that
 no overreading happens for damaged MPEG streams.

 @warning You must not provide a custom get_buffer() when using
 avcodec_decode_audio3().  Doing so will override it with
 avcodec_default_get_buffer.  Use avcodec_decode_audio4() instead,
 which does allow the application to provide a custom get_buffer().

 @note You might have to align the input buffer avpkt-&gt;data and output buffer
 samples. The alignment requirements depend on the CPU: On some CPUs it isn't
 necessary at all, on others it won't work at all if not aligned and on others
 it will work but it will have an impact on performance.

 In practice, avpkt-&gt;data should have 4 byte alignment at minimum and
 samples should be 16 byte aligned unless the CPU doesn't need it
 (AltiVec and SSE do).

 @note Codecs which have the CODEC_CAP_DELAY capability set have a delay
 between input and output, these need to be fed with avpkt-&gt;data=NULL,
 avpkt-&gt;size=0 at the end to return the remaining frames.

 @param avctx the codec context
 @param[out] samples the output buffer, sample type in avctx-&gt;sample_fmt
                     If the sample format is planar, each channel plane will
                     be the same size, with no padding between channels.
 @param[in,out] frame_size_ptr the output buffer size in bytes
 @param[in] avpkt The input AVPacket containing the input buffer.
            You can create such packet with av_init_packet() and by then setting
            data and size, some decoders might in addition need other fields.
            All decoders are designed to use the least fields possible though.
 @return On error a negative value is returned, otherwise the number of bytes
 used or zero if no frame data was decompressed (used) from the input AVPacket.

</member>
<member name="M:libffmpeg.avcodec_decode_audio4(libffmpeg.AVCodecContext*,libffmpeg.AVFrame*,System.Int32*,libffmpeg.AVPacket!System.Runtime.CompilerServices.IsConst*)" decl="true" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavcodec\avcodec.h" line="4094">
 Decode the audio frame of size avpkt-&gt;size from avpkt-&gt;data into frame.

 Some decoders may support multiple frames in a single AVPacket. Such
 decoders would then just decode the first frame and the return value would be
 less than the packet size. In this case, avcodec_decode_audio4 has to be
 called again with an AVPacket containing the remaining data in order to
 decode the second frame, etc...  Even if no frames are returned, the packet
 needs to be fed to the decoder with remaining data until it is completely
 consumed or an error occurs.

 Some decoders (those marked with CODEC_CAP_DELAY) have a delay between input
 and output. This means that for some packets they will not immediately
 produce decoded output and need to be flushed at the end of decoding to get
 all the decoded data. Flushing is done by calling this function with packets
 with avpkt-&gt;data set to NULL and avpkt-&gt;size set to 0 until it stops
 returning samples. It is safe to flush even those decoders that are not
 marked with CODEC_CAP_DELAY, then no samples will be returned.

 @warning The input buffer, avpkt-&gt;data must be FF_INPUT_BUFFER_PADDING_SIZE
          larger than the actual read bytes because some optimized bitstream
          readers read 32 or 64 bits at once and could read over the end.

 @note The AVCodecContext MUST have been opened with @ref avcodec_open2()
 before packets may be fed to the decoder.

 @param      avctx the codec context
 @param[out] frame The AVFrame in which to store decoded audio samples.
                   The decoder will allocate a buffer for the decoded frame by
                   calling the AVCodecContext.get_buffer2() callback.
                   When AVCodecContext.refcounted_frames is set to 1, the frame is
                   reference counted and the returned reference belongs to the
                   caller. The caller must release the frame using av_frame_unref()
                   when the frame is no longer needed. The caller may safely write
                   to the frame if av_frame_is_writable() returns 1.
                   When AVCodecContext.refcounted_frames is set to 0, the returned
                   reference belongs to the decoder and is valid only until the
                   next call to this function or until closing or flushing the
                   decoder. The caller may not write to it.
 @param[out] got_frame_ptr Zero if no frame could be decoded, otherwise it is
                           non-zero. Note that this field being set to zero
                           does not mean that an error has occurred. For
                           decoders with CODEC_CAP_DELAY set, no given decode
                           call is guaranteed to produce a frame.
 @param[in]  avpkt The input AVPacket containing the input buffer.
                   At least avpkt-&gt;data and avpkt-&gt;size should be set. Some
                   decoders might also require additional fields to be set.
 @return A negative error code is returned if an error occurred during
         decoding, otherwise the number of bytes consumed from the input
         AVPacket is returned.

</member>
<!-- Discarding badly formed XML document comment for member 'M:libffmpeg.avcodec_decode_video2(libffmpeg.AVCodecContext*,libffmpeg.AVFrame*,System.Int32*,libffmpeg.AVPacket!System.Runtime.CompilerServices.IsConst*)'. -->
<member name="M:libffmpeg.avcodec_decode_subtitle2(libffmpeg.AVCodecContext*,libffmpeg.AVSubtitle*,System.Int32*,libffmpeg.AVPacket*)" decl="true" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavcodec\avcodec.h" line="4195">
 Decode a subtitle message.
 Return a negative value on error, otherwise return the number of bytes used.
 If no subtitle could be decompressed, got_sub_ptr is zero.
 Otherwise, the subtitle is stored in *sub.
 Note that CODEC_CAP_DR1 is not available for subtitle codecs. This is for
 simplicity, because the performance difference is expect to be negligible
 and reusing a get_buffer written for video codecs would probably perform badly
 due to a potentially very different allocation pattern.

 Some decoders (those marked with CODEC_CAP_DELAY) have a delay between input
 and output. This means that for some packets they will not immediately
 produce decoded output and need to be flushed at the end of decoding to get
 all the decoded data. Flushing is done by calling this function with packets
 with avpkt-&gt;data set to NULL and avpkt-&gt;size set to 0 until it stops
 returning subtitles. It is safe to flush even those decoders that are not
 marked with CODEC_CAP_DELAY, then no subtitles will be returned.

 @note The AVCodecContext MUST have been opened with @ref avcodec_open2()
 before packets may be fed to the decoder.

 @param avctx the codec context
 @param[out] sub The Preallocated AVSubtitle in which the decoded subtitle will be stored,
                 must be freed with avsubtitle_free if *got_sub_ptr is set.
 @param[in,out] got_sub_ptr Zero if no subtitle could be decompressed, otherwise, it is nonzero.
 @param[in] avpkt The input AVPacket containing the input buffer.

</member>
<member name="T:libffmpeg.AVPictureStructure" decl="false" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavcodec\avcodec.h" line="4226">
@defgroup lavc_parsing Frame parsing
@{

</member>
<member name="F:libffmpeg.AVCodecParserContext.repeat_pict" decl="false" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavcodec\avcodec.h" line="4247">
 This field is used for proper frame duration computation in lavf.
 It signals, how much longer the frame duration of the current frame
 is compared to normal frame duration.

 frame_duration = (1 + repeat_pict) * time_base

 It is used by codecs like H.264 to display telecined material.

</member>
<member name="F:libffmpeg.AVCodecParserContext.offset" decl="false" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavcodec\avcodec.h" line="4274">
Set if the parser has a valid file offset
</member>
<member name="F:libffmpeg.AVCodecParserContext.key_frame" decl="false" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavcodec\avcodec.h" line="4281">
Set by parser to 1 for key frames and 0 for non-key frames.
It is initialized to -1, so if the parser doesn't set this flag,
old-style fallback using AV_PICTURE_TYPE_I picture type as key frames
will be used.

</member>
<member name="F:libffmpeg.AVCodecParserContext.convergence_duration" decl="false" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavcodec\avcodec.h" line="4289">
 Time difference in stream time base units from the pts of this
 packet to the point at which the output from the decoder has converged
 independent from the availability of previous frames. That is, the
 frames are virtually identical no matter if decoding started from
 the very first frame or from this keyframe.
 Is AV_NOPTS_VALUE if unknown.
 This field is not the display duration of the current frame.
 This field has no meaning if the packet does not have AV_PKT_FLAG_KEY
 set.

 The purpose of this field is to allow seeking in streams that have no
 keyframes in the conventional sense. It corresponds to the
 recovery point SEI in H.264 and match_time_delta in NUT. It is also
 essential for some types of subtitle streams to ensure that all
 subtitles are correctly displayed after seeking.

</member>
<!-- Discarding badly formed XML document comment for member 'F:libffmpeg.AVCodecParserContext.dts_sync_point'. -->
<member name="F:libffmpeg.AVCodecParserContext.dts_ref_dts_delta" decl="false" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavcodec\avcodec.h" line="4320">
 Offset of the current timestamp against last timestamp sync point in
 units of AVCodecContext.time_base.

 Set to INT_MIN when dts_sync_point unused. Otherwise, it must
 contain a valid timestamp offset.

 Note that the timestamp of sync point has usually a nonzero
 dts_ref_dts_delta, which refers to the previous sync point. Offset of
 the next frame after timestamp sync point will be usually 1.

 For example, this corresponds to H.264 cpb_removal_delay.

</member>
<member name="F:libffmpeg.AVCodecParserContext.pts_dts_delta" decl="false" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavcodec\avcodec.h" line="4335">
 Presentation delay of current frame in units of AVCodecContext.time_base.

 Set to INT_MIN when dts_sync_point unused. Otherwise, it must
 contain valid non-negative timestamp delta (presentation time of a frame
 must not lie in the past).

 This delay represents the difference between decoding and presentation
 time of the frame.

 For example, this corresponds to H.264 dpb_output_delay.

</member>
<member name="F:libffmpeg.AVCodecParserContext.cur_frame_pos" decl="false" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavcodec\avcodec.h" line="4349">
 Position of the packet in file.

 Analogous to cur_frame_pts/dts

</member>
<member name="F:libffmpeg.AVCodecParserContext.pos" decl="false" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavcodec\avcodec.h" line="4356">
Byte position of currently parsed frame in stream.

</member>
<member name="F:libffmpeg.AVCodecParserContext.last_pos" decl="false" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavcodec\avcodec.h" line="4361">
Previous frame byte position.

</member>
<member name="F:libffmpeg.AVCodecParserContext.duration" decl="false" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavcodec\avcodec.h" line="4366">
Duration of the current frame.
For audio, this is in units of 1 / AVCodecContext.sample_rate.
For all other types, this is in units of AVCodecContext.time_base.

</member>
<member name="T:libffmpeg.AVPictureStructure" decl="true" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavcodec\avcodec.h" line="4375">
 Indicate whether a picture is coded as a frame, top field or bottom field.

 For example, H.264 field_pic_flag equal to 0 corresponds to
 AV_PICTURE_STRUCTURE_FRAME. An H.264 picture with field_pic_flag
 equal to 1 and bottom_field_flag equal to 0 corresponds to
 AV_PICTURE_STRUCTURE_TOP_FIELD.

</member>
<member name="F:libffmpeg.AVCodecParserContext.output_picture_number" decl="false" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavcodec\avcodec.h" line="4385">
 Picture number incremented in presentation or output order.
 This field may be reinitialized at the first picture of a new sequence.

 For example, this corresponds to H.264 PicOrderCnt.

</member>
<member name="F:libffmpeg.AVCodecParserContext.width" decl="false" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavcodec\avcodec.h" line="4393">
Dimensions of the decoded video intended for presentation.

</member>
<member name="F:libffmpeg.AVCodecParserContext.coded_width" decl="false" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavcodec\avcodec.h" line="4399">
Dimensions of the coded video.

</member>
<member name="F:libffmpeg.AVCodecParserContext.format" decl="false" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavcodec\avcodec.h" line="4405">
 The format of the coded data, corresponds to enum AVPixelFormat for video
 and for enum AVSampleFormat for audio.

 Note that a decoder can have considerable freedom in how exactly it
 decodes the data, so the format reported here might be different from the
 one returned by a decoder.

</member>
<!-- Discarding badly formed XML document comment for member 'M:libffmpeg.av_parser_parse2(libffmpeg.AVCodecParserContext*,libffmpeg.AVCodecContext*,System.Byte**,System.Int32*,System.Byte!System.Runtime.CompilerServices.IsConst*,System.Int32,System.Int64,System.Int64,System.Int64)'. -->
<member name="M:libffmpeg.av_parser_change(libffmpeg.AVCodecParserContext*,libffmpeg.AVCodecContext*,System.Byte**,System.Int32*,System.Byte!System.Runtime.CompilerServices.IsConst*,System.Int32,System.Int32)" decl="true" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavcodec\avcodec.h" line="4471">
@return 0 if the output buffer is a subset of the input, 1 if it is allocated and must be freed
@deprecated use AVBitStreamFilter

</member>
<member name="M:libffmpeg.avcodec_find_encoder(libffmpeg.AVCodecID)" decl="true" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavcodec\avcodec.h" line="4481">
@}
@}

@addtogroup lavc_encoding
@{

 Find a registered encoder with a matching codec ID.

 @param id AVCodecID of the requested encoder
 @return An encoder if one was found, NULL otherwise.

</member>
<member name="M:libffmpeg.avcodec_find_encoder_by_name(System.SByte!System.Runtime.CompilerServices.IsSignUnspecifiedByte!System.Runtime.CompilerServices.IsConst*)" decl="true" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavcodec\avcodec.h" line="4499">
 Find a registered encoder with the specified name.

 @param name name of the requested encoder
 @return An encoder if one was found, NULL otherwise.

</member>
<member name="M:libffmpeg.avcodec_encode_audio(libffmpeg.AVCodecContext*,System.Byte*,System.Int32,System.Int16!System.Runtime.CompilerServices.IsConst*)" decl="true" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavcodec\avcodec.h" line="4508">
 Encode an audio frame from samples into buf.

 @deprecated Use avcodec_encode_audio2 instead.

 @note The output buffer should be at least FF_MIN_BUFFER_SIZE bytes large.
 However, for codecs with avctx-&gt;frame_size equal to 0 (e.g. PCM) the user
 will know how much space is needed because it depends on the value passed
 in buf_size as described below. In that case a lower value can be used.

 @param avctx the codec context
 @param[out] buf the output buffer
 @param[in] buf_size the output buffer size
 @param[in] samples the input buffer containing the samples
 The number of samples read from this buffer is frame_size*channels,
 both of which are defined in avctx.
 For codecs which have avctx-&gt;frame_size equal to 0 (e.g. PCM) the number of
 samples read from samples is equal to:
 buf_size * 8 / (avctx-&gt;channels * av_get_bits_per_sample(avctx-&gt;codec_id))
 This also implies that av_get_bits_per_sample() must not return 0 for these
 codecs.
 @return On error a negative value is returned, on success zero or the number
 of bytes used to encode the data read from the input buffer.

</member>
<member name="M:libffmpeg.avcodec_encode_audio2(libffmpeg.AVCodecContext*,libffmpeg.AVPacket*,libffmpeg.AVFrame!System.Runtime.CompilerServices.IsConst*,System.Int32*)" decl="true" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavcodec\avcodec.h" line="4537">
 Encode a frame of audio.

 Takes input samples from frame and writes the next output packet, if
 available, to avpkt. The output packet does not necessarily contain data for
 the most recent frame, as encoders can delay, split, and combine input frames
 internally as needed.

 @param avctx     codec context
 @param avpkt     output AVPacket.
                  The user can supply an output buffer by setting
                  avpkt-&gt;data and avpkt-&gt;size prior to calling the
                  function, but if the size of the user-provided data is not
                  large enough, encoding will fail. If avpkt-&gt;data and
                  avpkt-&gt;size are set, avpkt-&gt;destruct must also be set. All
                  other AVPacket fields will be reset by the encoder using
                  av_init_packet(). If avpkt-&gt;data is NULL, the encoder will
                  allocate it. The encoder will set avpkt-&gt;size to the size
                  of the output packet.

                  If this function fails or produces no output, avpkt will be
                  freed using av_free_packet() (i.e. avpkt-&gt;destruct will be
                  called to free the user supplied buffer).
 @param[in] frame AVFrame containing the raw audio data to be encoded.
                  May be NULL when flushing an encoder that has the
                  CODEC_CAP_DELAY capability set.
                  If CODEC_CAP_VARIABLE_FRAME_SIZE is set, then each frame
                  can have any number of samples.
                  If it is not set, frame-&gt;nb_samples must be equal to
                  avctx-&gt;frame_size for all frames except the last.
                  The final frame may be smaller than avctx-&gt;frame_size.
 @param[out] got_packet_ptr This field is set to 1 by libavcodec if the
                            output packet is non-empty, and to 0 if it is
                            empty. If the function returns an error, the
                            packet can be assumed to be invalid, and the
                            value of got_packet_ptr is undefined and should
                            not be used.
 @return          0 on success, negative error code on failure

</member>
<member name="M:libffmpeg.avcodec_encode_video(libffmpeg.AVCodecContext*,System.Byte*,System.Int32,libffmpeg.AVFrame!System.Runtime.CompilerServices.IsConst*)" decl="true" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavcodec\avcodec.h" line="4580">
 @deprecated use avcodec_encode_video2() instead.

 Encode a video frame from pict into buf.
 The input picture should be
 stored using a specific format, namely avctx.pix_fmt.

 @param avctx the codec context
 @param[out] buf the output buffer for the bitstream of encoded frame
 @param[in] buf_size the size of the output buffer in bytes
 @param[in] pict the input picture to encode
 @return On error a negative value is returned, on success zero or the number
 of bytes used from the output buffer.

</member>
<member name="M:libffmpeg.avcodec_encode_video2(libffmpeg.AVCodecContext*,libffmpeg.AVPacket*,libffmpeg.AVFrame!System.Runtime.CompilerServices.IsConst*,System.Int32*)" decl="true" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavcodec\avcodec.h" line="4599">
 Encode a frame of video.

 Takes input raw video data from frame and writes the next output packet, if
 available, to avpkt. The output packet does not necessarily contain data for
 the most recent frame, as encoders can delay and reorder input frames
 internally as needed.

 @param avctx     codec context
 @param avpkt     output AVPacket.
                  The user can supply an output buffer by setting
                  avpkt-&gt;data and avpkt-&gt;size prior to calling the
                  function, but if the size of the user-provided data is not
                  large enough, encoding will fail. All other AVPacket fields
                  will be reset by the encoder using av_init_packet(). If
                  avpkt-&gt;data is NULL, the encoder will allocate it.
                  The encoder will set avpkt-&gt;size to the size of the
                  output packet. The returned data (if any) belongs to the
                  caller, he is responsible for freeing it.

                  If this function fails or produces no output, avpkt will be
                  freed using av_free_packet() (i.e. avpkt-&gt;destruct will be
                  called to free the user supplied buffer).
 @param[in] frame AVFrame containing the raw video data to be encoded.
                  May be NULL when flushing an encoder that has the
                  CODEC_CAP_DELAY capability set.
 @param[out] got_packet_ptr This field is set to 1 by libavcodec if the
                            output packet is non-empty, and to 0 if it is
                            empty. If the function returns an error, the
                            packet can be assumed to be invalid, and the
                            value of got_packet_ptr is undefined and should
                            not be used.
 @return          0 on success, negative error code on failure

</member>
<member name="T:libffmpeg.ReSampleContext" decl="true" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavcodec\avcodec.h" line="4640">
@}

 @defgroup lavc_resample Audio resampling
 @ingroup libavc
 @deprecated use libswresample instead

 @{

</member>
<member name="M:libffmpeg.av_audio_resample_init(System.Int32,System.Int32,System.Int32,System.Int32,libffmpeg.AVSampleFormat,libffmpeg.AVSampleFormat,System.Int32,System.Int32,System.Int32,System.Double)" decl="true" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavcodec\avcodec.h" line="4657">
 *  Initialize audio resampling context.
 *
 * @param output_channels  number of output channels
 * @param input_channels   number of input channels
 * @param output_rate      output sample rate
 * @param input_rate       input sample rate
 * @param sample_fmt_out   requested output sample format
 * @param sample_fmt_in    input sample format
 * @param filter_length    length of each FIR filter in the filterbank relative to the cutoff frequency
 * @param log2_phase_count log2 of the number of entries in the polyphase filterbank
 * @param linear           if 1 then the used FIR filter will be linearly interpolated
                           between the 2 closest, if 0 the closest will be used
 * @param cutoff           cutoff frequency, 1.0 corresponds to half the output sampling rate
 * @return allocated ReSampleContext, NULL if error occurred

</member>
<member name="M:libffmpeg.audio_resample_close(libffmpeg.ReSampleContext*)" decl="true" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavcodec\avcodec.h" line="4684">
 Free resample context.

 @param s a non-NULL pointer to a resample context previously
          created with av_audio_resample_init()

</member>
<member name="T:libffmpeg.AVResampleContext" decl="true" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavcodec\avcodec.h" line="4694">
 * Initialize an audio resampler.
 * Note, if either rate is not an integer then simply scale both rates up so they are.
 * @param filter_length length of each FIR filter in the filterbank relative to the cutoff freq
 * @param log2_phase_count log2 of the number of entries in the polyphase filterbank
 * @param linear If 1 then the used FIR filter will be linearly interpolated
                 between the 2 closest, if 0 the closest will be used
 * @param cutoff cutoff frequency, 1.0 corresponds to half the output sampling rate

</member>
<member name="M:libffmpeg.av_resample(libffmpeg.AVResampleContext*,System.Int16*,System.Int16*,System.Int32*,System.Int32,System.Int32,System.Int32)" decl="true" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavcodec\avcodec.h" line="4706">
Resample an array of samples using a previously configured context.
@param src an array of unconsumed samples
@param consumed the number of samples of src which have been consumed are returned here
@param src_size the number of unconsumed samples available
@param dst_size the amount of space in samples available in dst
@param update_ctx If this is 0 then the context will not be modified, that way several channels can be resampled with the same context.
@return the number of samples written in dst or -1 if an error occurred

</member>
<member name="M:libffmpeg.av_resample_compensate(libffmpeg.AVResampleContext*,System.Int32,System.Int32)" decl="true" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavcodec\avcodec.h" line="4719">
 Compensate samplerate/timestamp drift. The compensation is done by changing
 the resampler parameters, so no audible clicks or similar distortions occur
 @param compensation_distance distance in output samples over which the compensation should be performed
 @param sample_delta number of output samples which should be output less

 example: av_resample_compensate(c, 10, 500)
 here instead of 510 samples only 500 samples would be output

 note, due to rounding the actual compensation might be slightly different,
 especially if the compensation_distance is large and the in_rate used during init is small

</member>
<member name="M:libffmpeg.avpicture_alloc(libffmpeg.AVPicture*,libffmpeg.AVPixelFormat,System.Int32,System.Int32)" decl="true" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavcodec\avcodec.h" line="4736">
@}

@addtogroup lavc_picture
@{

 Allocate memory for the pixels of a picture and setup the AVPicture
 fields for it.

 Call avpicture_free() to free it.

 @param picture            the picture structure to be filled in
 @param pix_fmt            the pixel format of the picture
 @param width              the width of the picture
 @param height             the height of the picture
 @return zero if successful, a negative error code otherwise

 @see av_image_alloc(), avpicture_fill()

</member>
<member name="M:libffmpeg.avpicture_free(libffmpeg.AVPicture*)" decl="true" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavcodec\avcodec.h" line="4762">
 Free a picture previously allocated by avpicture_alloc().
 The data buffer used by the AVPicture is freed, but the AVPicture structure
 itself is not.

 @param picture the AVPicture to be freed

</member>
<member name="M:libffmpeg.avpicture_fill(libffmpeg.AVPicture*,System.Byte!System.Runtime.CompilerServices.IsConst*,libffmpeg.AVPixelFormat,System.Int32,System.Int32)" decl="true" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavcodec\avcodec.h" line="4771">
 Setup the picture fields based on the specified image parameters
 and the provided image data buffer.

 The picture fields are filled in by using the image data buffer
 pointed to by ptr.

 If ptr is NULL, the function will fill only the picture linesize
 array and return the required size for the image buffer.

 To allocate an image buffer and fill the picture data in one call,
 use avpicture_alloc().

 @param picture       the picture to be filled in
 @param ptr           buffer where the image data is stored, or NULL
 @param pix_fmt       the pixel format of the image
 @param width         the width of the image in pixels
 @param height        the height of the image in pixels
 @return the size in bytes required for src, a negative error code
 in case of failure

 @see av_image_fill_arrays()

</member>
<member name="M:libffmpeg.avpicture_layout(libffmpeg.AVPicture!System.Runtime.CompilerServices.IsConst*,libffmpeg.AVPixelFormat,System.Int32,System.Int32,System.Byte*,System.Int32)" decl="true" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavcodec\avcodec.h" line="4797">
 Copy pixel data from an AVPicture into a buffer.

 avpicture_get_size() can be used to compute the required size for
 the buffer to fill.

 @param src        source picture with filled data
 @param pix_fmt    picture pixel format
 @param width      picture width
 @param height     picture height
 @param dest       destination buffer
 @param dest_size  destination buffer size in bytes
 @return the number of bytes written to dest, or a negative value
 (error code) on error, for example if the destination buffer is not
 big enough

 @see av_image_copy_to_buffer()

</member>
<member name="M:libffmpeg.avpicture_get_size(libffmpeg.AVPixelFormat,System.Int32,System.Int32)" decl="true" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavcodec\avcodec.h" line="4819">
 Calculate the size in bytes that a picture of the given width and height
 would occupy if stored in the given picture format.

 @param pix_fmt    picture pixel format
 @param width      picture width
 @param height     picture height
 @return the computed picture buffer size or a negative error code
 in case of error

 @see av_image_get_buffer_size().

</member>
<member name="M:libffmpeg.avpicture_deinterlace(libffmpeg.AVPicture*,libffmpeg.AVPicture!System.Runtime.CompilerServices.IsConst*,libffmpeg.AVPixelFormat,System.Int32,System.Int32)" decl="true" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavcodec\avcodec.h" line="4834">
  deinterlace - if not supported return -1

 @deprecated - use yadif (in libavfilter) instead

</member>
<member name="M:libffmpeg.av_picture_copy(libffmpeg.AVPicture*,libffmpeg.AVPicture!System.Runtime.CompilerServices.IsConst*,libffmpeg.AVPixelFormat,System.Int32,System.Int32)" decl="true" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavcodec\avcodec.h" line="4843">
Copy image src to dst. Wraps av_image_copy().

</member>
<member name="M:libffmpeg.av_picture_crop(libffmpeg.AVPicture*,libffmpeg.AVPicture!System.Runtime.CompilerServices.IsConst*,libffmpeg.AVPixelFormat,System.Int32,System.Int32)" decl="true" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavcodec\avcodec.h" line="4849">
Crop image top and left side.

</member>
<member name="M:libffmpeg.av_picture_pad(libffmpeg.AVPicture*,libffmpeg.AVPicture!System.Runtime.CompilerServices.IsConst*,System.Int32,System.Int32,libffmpeg.AVPixelFormat,System.Int32,System.Int32,System.Int32,System.Int32,System.Int32*)" decl="true" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavcodec\avcodec.h" line="4855">
Pad image.

</member>
<member name="M:libffmpeg.avcodec_get_chroma_sub_sample(libffmpeg.AVPixelFormat,System.Int32*,System.Int32*)" decl="true" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavcodec\avcodec.h" line="4861">
@}

 @defgroup lavc_misc Utility functions
 @ingroup libavc

 Miscellaneous utility functions related to both encoding and decoding
 (or neither).
 @{

 @defgroup lavc_misc_pixfmt Pixel formats

 Functions for working with pixel formats.
 @{

 Utility function to access log2_chroma_w log2_chroma_h from
 the pixel format AVPixFmtDescriptor.

 This function asserts that pix_fmt is valid. See av_pix_fmt_get_chroma_sub_sample
 for one that returns a failure code and continues in case of invalid
 pix_fmts.

 @param[in]  pix_fmt the pixel format
 @param[out] h_shift store log2_chroma_w
 @param[out] v_shift store log2_chroma_h

 @see av_pix_fmt_get_chroma_sub_sample

</member>
<member name="M:libffmpeg.avcodec_pix_fmt_to_codec_tag(libffmpeg.AVPixelFormat)" decl="true" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavcodec\avcodec.h" line="4898">
Return a value representing the fourCC code associated to the
pixel format pix_fmt, or 0 if no associated fourCC code can be
found.

</member>
<member name="M:libffmpeg.avcodec_get_pix_fmt_loss(libffmpeg.AVPixelFormat,libffmpeg.AVPixelFormat,System.Int32)" decl="true" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavcodec\avcodec.h" line="4905">
@deprecated see av_get_pix_fmt_loss()

</member>
<member name="T:libffmpeg.AVPixelFormat" decl="true" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavcodec\avcodec.h" line="4911">
 Find the best pixel format to convert to given a certain source pixel
 format.  When converting from one pixel format to another, information loss
 may occur.  For example, when converting from RGB24 to GRAY, the color
 information will be lost. Similarly, other losses occur when converting from
 some formats to other formats. avcodec_find_best_pix_fmt_of_2() searches which of
 the given pixel formats should be used to suffer the least amount of loss.
 The pixel formats from which it chooses one, are determined by the
 pix_fmt_list parameter.


 @param[in] pix_fmt_list AV_PIX_FMT_NONE terminated array of pixel formats to choose from
 @param[in] src_pix_fmt source pixel format
 @param[in] has_alpha Whether the source pixel format alpha channel is used.
 @param[out] loss_ptr Combination of flags informing you what kind of losses will occur.
 @return The best pixel format to convert to or -1 if none was found.

</member>
<member name="T:libffmpeg.AVPixelFormat" decl="true" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavcodec\avcodec.h" line="4932">
@deprecated see av_find_best_pix_fmt_of_2()

</member>
<member name="M:libffmpeg.avcodec_set_dimensions(libffmpeg.AVCodecContext*,System.Int32,System.Int32)" decl="true" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavcodec\avcodec.h" line="4951">
@}

@deprecated this function is not supposed to be used from outside of lavc

</member>
<member name="M:libffmpeg.av_get_codec_tag_string(System.SByte!System.Runtime.CompilerServices.IsSignUnspecifiedByte*,System.UInt32,System.UInt32)" decl="true" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavcodec\avcodec.h" line="4963">
 Put a string representing the codec tag codec_tag in buf.

 @param buf       buffer to place codec tag in
 @param buf_size size in bytes of buf
 @param codec_tag codec tag to assign
 @return the length of the string that would have been generated if
 enough space had been available, excluding the trailing null

</member>
<member name="M:libffmpeg.av_get_profile_name(libffmpeg.AVCodec!System.Runtime.CompilerServices.IsConst*,System.Int32)" decl="true" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavcodec\avcodec.h" line="4976">
 Return a name for the specified profile, if available.

 @param codec the codec that is searched for the given profile
 @param profile the profile value for which a name is requested
 @return A name for the profile if found, NULL otherwise.

</member>
<member name="M:libffmpeg.avcodec_fill_audio_frame(libffmpeg.AVFrame*,System.Int32,libffmpeg.AVSampleFormat,System.Byte!System.Runtime.CompilerServices.IsConst*,System.Int32,System.Int32)" decl="true" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavcodec\avcodec.h" line="4989">
 Fill AVFrame audio data and linesize pointers.

 The buffer buf must be a preallocated buffer with a size big enough
 to contain the specified samples amount. The filled AVFrame data
 pointers will point to this buffer.

 AVFrame extended_data channel pointers are allocated if necessary for
 planar audio.

 @param frame       the AVFrame
                    frame-&gt;nb_samples must be set prior to calling the
                    function. This function fills in frame-&gt;data,
                    frame-&gt;extended_data, frame-&gt;linesize[0].
 @param nb_channels channel count
 @param sample_fmt  sample format
 @param buf         buffer to use for frame data
 @param buf_size    size of buffer
 @param align       plane size sample alignment (0 = default)
 @return            &gt;=0 on success, negative error code on failure
 @todo return the size in bytes required to store the samples in
 case of success, at the next libavutil bump

</member>
<member name="M:libffmpeg.avcodec_flush_buffers(libffmpeg.AVCodecContext*)" decl="true" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavcodec\avcodec.h" line="5016">
 Reset the internal decoder state / flush internal buffers. Should be called
 e.g. when seeking or when switching to a different stream.

 @note when refcounted frames are not used (i.e. avctx-&gt;refcounted_frames is 0),
 this invalidates the frames previously returned from the decoder. When
 refcounted frames are used, the decoder just releases any references it might
 keep internally, but the caller's reference remains valid.

</member>
<member name="M:libffmpeg.av_get_bits_per_sample(libffmpeg.AVCodecID)" decl="true" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavcodec\avcodec.h" line="5027">
 Return codec bits per sample.

 @param[in] codec_id the codec
 @return Number of bits per sample or zero if unknown for the given codec.

</member>
<member name="T:libffmpeg.AVCodecID" decl="true" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavcodec\avcodec.h" line="5035">
Return the PCM codec associated with a sample format.
@param be  endianness, 0 for little, 1 for big,
           -1 (or anything else) for native
@return  AV_CODEC_ID_PCM_* or AV_CODEC_ID_NONE

</member>
<member name="M:libffmpeg.av_get_exact_bits_per_sample(libffmpeg.AVCodecID)" decl="true" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavcodec\avcodec.h" line="5043">
 Return codec bits per sample.
 Only return non-zero if the bits per sample is exactly correct, not an
 approximation.

 @param[in] codec_id the codec
 @return Number of bits per sample or zero if unknown for the given codec.

</member>
<member name="M:libffmpeg.av_get_audio_frame_duration(libffmpeg.AVCodecContext*,System.Int32)" decl="true" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavcodec\avcodec.h" line="5053">
 Return audio frame duration.

 @param avctx        codec context
 @param frame_bytes  size of the frame, or 0 if unknown
 @return             frame duration, in samples, if known. 0 if not able to
                     determine.

</member>
<member name="M:libffmpeg.av_register_bitstream_filter(libffmpeg.AVBitStreamFilter*)" decl="true" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavcodec\avcodec.h" line="5083">
 Register a bitstream filter.

 The filter will be accessible to the application code through
 av_bitstream_filter_next() or can be directly initialized with
 av_bitstream_filter_init().

 @see avcodec_register_all()

</member>
<member name="M:libffmpeg.av_bitstream_filter_init(System.SByte!System.Runtime.CompilerServices.IsSignUnspecifiedByte!System.Runtime.CompilerServices.IsConst*)" decl="true" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavcodec\avcodec.h" line="5094">
 Create and initialize a bitstream filter context given a bitstream
 filter name.

 The returned context must be freed with av_bitstream_filter_close().

 @param name    the name of the bitstream filter
 @return a bitstream filter context if a matching filter was found
 and successfully initialized, NULL otherwise

</member>
<member name="M:libffmpeg.av_bitstream_filter_filter(libffmpeg.AVBitStreamFilterContext*,libffmpeg.AVCodecContext*,System.SByte!System.Runtime.CompilerServices.IsSignUnspecifiedByte!System.Runtime.CompilerServices.IsConst*,System.Byte**,System.Int32*,System.Byte!System.Runtime.CompilerServices.IsConst*,System.Int32,System.Int32)" decl="true" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavcodec\avcodec.h" line="5106">
 Filter bitstream.

 This function filters the buffer buf with size buf_size, and places the
 filtered buffer in the buffer pointed to by poutbuf.

 The output buffer must be freed by the caller.

 @param bsfc            bitstream filter context created by av_bitstream_filter_init()
 @param avctx           AVCodecContext accessed by the filter, may be NULL.
                        If specified, this must point to the encoder context of the
                        output stream the packet is sent to.
 @param args            arguments which specify the filter configuration, may be NULL
 @param poutbuf         pointer which is updated to point to the filtered buffer
 @param poutbuf_size    pointer which is updated to the filtered buffer size in bytes
 @param buf             buffer containing the data to filter
 @param buf_size        size in bytes of buf
 @param keyframe        set to non-zero if the buffer to filter corresponds to a key-frame packet data
 @return &gt;= 0 in case of success, or a negative error code in case of failure

 If the return value is positive, an output buffer is allocated and
 is available in *poutbuf, and is distinct from the input buffer.

 If the return value is 0, the output buffer is not allocated and
 should be considered identical to the input buffer, or in case
 *poutbuf was set it points to the input buffer (not necessarily to
 its starting address).

</member>
<member name="M:libffmpeg.av_bitstream_filter_close(libffmpeg.AVBitStreamFilterContext*)" decl="true" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavcodec\avcodec.h" line="5139">
 Release bitstream filter context.

 @param bsf the bitstream filter context created with
 av_bitstream_filter_init(), can be NULL

</member>
<member name="M:libffmpeg.av_bitstream_filter_next(libffmpeg.AVBitStreamFilter!System.Runtime.CompilerServices.IsConst*)" decl="true" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavcodec\avcodec.h" line="5147">
 If f is NULL, return the first registered bitstream filter,
 if f is non-NULL, return the next registered bitstream filter
 after f, or NULL if f is the last one.

 This function can be used to iterate over all registered bitstream
 filters.

</member>
<member name="M:libffmpeg.av_fast_padded_malloc(System.Void*,System.UInt32*,System.UInt32)" decl="true" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavcodec\avcodec.h" line="5159">
 Same behaviour av_fast_malloc but the buffer has additional
 FF_INPUT_BUFFER_PADDING_SIZE at the end which will always be 0.

 In addition the whole buffer will initially and after resizes
 be 0-initialized so that no uninitialized data will ever appear.

</member>
<member name="M:libffmpeg.av_fast_padded_mallocz(System.Void*,System.UInt32*,System.UInt32)" decl="true" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavcodec\avcodec.h" line="5168">
Same behaviour av_fast_padded_malloc except that buffer will always
be 0-initialized after call.

</member>
<member name="M:libffmpeg.av_xiphlacing(System.Byte*,System.UInt32)" decl="true" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavcodec\avcodec.h" line="5174">
 Encode extradata length to a buffer. Used by xiph codecs.

 @param s buffer to write to; must be at least (v/255+1) bytes long
 @param v size of extradata in bytes
 @return number of bytes written to the buffer.

</member>
<member name="M:libffmpeg.av_log_missing_feature(System.Void*,System.SByte!System.Runtime.CompilerServices.IsSignUnspecifiedByte!System.Runtime.CompilerServices.IsConst*,System.Int32)" decl="true" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavcodec\avcodec.h" line="5184">
Log a generic warning message about a missing feature. This function is
intended to be used internally by FFmpeg (libavcodec, libavformat, etc.)
only, and would normally not be used by applications.
@param[in] avc a pointer to an arbitrary struct of which the first field is
a pointer to an AVClass struct
@param[in] feature string containing the name of the missing feature
@param[in] want_sample indicates if samples are wanted which exhibit this feature.
If want_sample is non-zero, additional verbage will be added to the log
message which tells the user how to report samples to the development
mailing list.
@deprecated Use avpriv_report_missing_feature() instead.

</member>
<member name="M:libffmpeg.av_log_ask_for_sample(System.Void*,System.SByte!System.Runtime.CompilerServices.IsSignUnspecifiedByte!System.Runtime.CompilerServices.IsConst*,BTEllipsis)" decl="true" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavcodec\avcodec.h" line="5200">
Log a generic warning message asking for a sample. This function is
intended to be used internally by FFmpeg (libavcodec, libavformat, etc.)
only, and would normally not be used by applications.
@param[in] avc a pointer to an arbitrary struct of which the first field is
a pointer to an AVClass struct
@param[in] msg string containing an optional message, or NULL if no message
@deprecated Use avpriv_request_sample() instead.

</member>
<member name="M:libffmpeg.av_register_hwaccel(libffmpeg.AVHWAccel*)" decl="true" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavcodec\avcodec.h" line="5213">
Register the hardware accelerator hwaccel.

</member>
<member name="M:libffmpeg.av_hwaccel_next(libffmpeg.AVHWAccel!System.Runtime.CompilerServices.IsConst*)" decl="true" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavcodec\avcodec.h" line="5218">
If hwaccel is NULL, returns the first registered hardware accelerator,
if hwaccel is non-NULL, returns the next registered hardware accelerator
after hwaccel, or NULL if hwaccel is the last one.

</member>
<member name="T:libffmpeg.AVLockOp" decl="false" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavcodec\avcodec.h" line="5226">
Lock operation used by lockmgr

</member>
<member name="M:libffmpeg.av_lockmgr_register(=FUNC:System.Int32(System.Void**,libffmpeg.AVLockOp))" decl="true" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavcodec\avcodec.h" line="5236">
 Register a user provided lock manager supporting the operations
 specified by AVLockOp. The "mutex" argument to the function points
 to a (void *) where the lockmgr should store/get a pointer to a user
 allocated mutex. It is NULL upon AV_LOCK_CREATE and equal to the
 value left by the last call for all other ops. If the lock manager is
 unable to perform the op then it should leave the mutex in the same
 state as when it was called and return a non-zero value. However,
 when called with AV_LOCK_DESTROY the mutex will always be assumed to
 have been successfully destroyed. If av_lockmgr_register succeeds
 it will return a non-negative value, if it fails it will return a
 negative value and destroy all mutex and unregister all callbacks.
 av_lockmgr_register is not thread-safe, it must be called from a
 single thread before any calls which make use of locking are used.

 @param cb User defined callback. av_lockmgr_register invokes calls
           to this callback and the previously registered callback.
           The callback will be used to create more than one mutex
           each of which must be backed by its own underlying locking
           mechanism (i.e. do not use a single static object to
           implement your lock manager). If cb is set to NULL the
           lockmgr will be unregistered.

</member>
<member name="T:libffmpeg.AVMediaType" decl="true" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavcodec\avcodec.h" line="5261">
Get the type of the given codec.

</member>
<member name="M:libffmpeg.avcodec_get_name(libffmpeg.AVCodecID)" decl="true" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavcodec\avcodec.h" line="5266">
Get the name of a codec.
@return  a static string identifying the codec; never NULL

</member>
<member name="M:libffmpeg.avcodec_is_open(libffmpeg.AVCodecContext*)" decl="true" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavcodec\avcodec.h" line="5272">
@return a positive value if s is open (i.e. avcodec_open2() was called on it
with no corresponding avcodec_close()), 0 otherwise.

</member>
<member name="M:libffmpeg.av_codec_is_encoder(libffmpeg.AVCodec!System.Runtime.CompilerServices.IsConst*)" decl="true" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavcodec\avcodec.h" line="5278">
@return a non-zero number if codec is an encoder, zero otherwise

</member>
<member name="M:libffmpeg.av_codec_is_decoder(libffmpeg.AVCodec!System.Runtime.CompilerServices.IsConst*)" decl="true" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavcodec\avcodec.h" line="5283">
@return a non-zero number if codec is a decoder, zero otherwise

</member>
<member name="M:libffmpeg.avcodec_descriptor_get(libffmpeg.AVCodecID)" decl="true" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavcodec\avcodec.h" line="5288">
@return descriptor for given codec ID or NULL if no descriptor exists.

</member>
<member name="M:libffmpeg.avcodec_descriptor_next(libffmpeg.AVCodecDescriptor!System.Runtime.CompilerServices.IsConst*)" decl="true" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavcodec\avcodec.h" line="5293">
 Iterate over all codec descriptors known to libavcodec.

 @param prev previous descriptor. NULL to get the first descriptor.

 @return next descriptor or NULL after the last descriptor

</member>
<member name="M:libffmpeg.avcodec_descriptor_get_by_name(System.SByte!System.Runtime.CompilerServices.IsSignUnspecifiedByte!System.Runtime.CompilerServices.IsConst*)" decl="true" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavcodec\avcodec.h" line="5302">
@return codec descriptor with the given name or NULL if no such descriptor
        exists.

</member>
</members>
</doc>