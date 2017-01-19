<?xml version="1.0"?><doc>
<members>
<!-- Discarding badly formed XML document comment for member 'D:libffmpeg.clock_t'. -->
<member name="M:libffmpeg.avutil_version" decl="true" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavcodec\avcodec.h" line="24">
@file
@ingroup libavc
Libavcodec external API header

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
Macro definitions for various function/variable attributes

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
<!-- Discarding badly formed XML document comment for member 'T:libffmpeg.av_intfloat32'. -->
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

@file
Macro definitions for various function/variable attributes

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
Macro definitions for various function/variable attributes

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
<!-- Discarding badly formed XML document comment for member 'T:libffmpeg.AVFrameSideDataType'. -->
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
<!-- Discarding badly formed XML document comment for member 'T:libffmpeg.AVCodecID'. -->
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
<member name="T:libffmpeg.AVIOInterruptCB" decl="false" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavcodec\avcodec.h" line="5308">
@}

@file
Public dictionary API.
@deprecated
 AVDictionary is provided for compatibility with libav. It is both in
 implementation as well as API inefficient. It does not scale and is
 extremely slow with large dictionaries.
 It is recommended that new code uses our tree container from tree.c/h
 where applicable, which uses AVL trees to achieve O(log n) performance.

@file
@ingroup lavf_io
Buffered I/O operations

@file
common internal and external API header

@file
Public dictionary API.
@deprecated
 AVDictionary is provided for compatibility with libav. It is both in
 implementation as well as API inefficient. It does not scale and is
 extremely slow with large dictionaries.
 It is recommended that new code uses our tree container from tree.c/h
 where applicable, which uses AVL trees to achieve O(log n) performance.

@file
@ingroup libavf
Libavformat version macros

FF_API_* defines may be placed below to indicate public API that will be
dropped at a future version bump. The defines themselves are not part of
the public API and may change, break or disappear at any time.

 Callback for checking whether to abort blocking functions.
 AVERROR_EXIT is returned in this case by the interrupted
 function. During blocking operations, callback is called with
 opaque as parameter. If the callback returns 1, the
 blocking operation will be aborted.

 No members can be added to this struct without a major bump, if
 new elements have been added after this struct in AVFormatContext
 or AVIOContext.

</member>
<member name="T:libffmpeg.AVIOContext" decl="false" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavformat\avio.h" line="56">
 Bytestream IO Context.
 New fields can be added to the end with minor version bumps.
 Removal, reordering and changes to existing fields require a major
 version bump.
 sizeof(AVIOContext) must not be used outside libav*.

 @note None of the function pointers in AVIOContext should be called
       directly, they should only be set by the client application
       when implementing custom I/O. Normally these are set to the
       function pointers specified in avio_alloc_context()

</member>
<member name="F:libffmpeg.AVIOContext.av_class" decl="false" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavformat\avio.h" line="69">
 A class for private options.

 If this AVIOContext is created by avio_open2(), av_class is set and
 passes the options down to protocols.

 If this AVIOContext is manually allocated, then av_class may be set by
 the caller.

 warning -- this field can be NULL, be sure to not pass this AVIOContext
 to any av_opt_* functions in that case.

</member>
<member name="F:libffmpeg.AVIOContext.read_pause" decl="false" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavformat\avio.h" line="103">
Pause or resume playback for network streaming protocols - e.g. MMS.

</member>
<member name="F:libffmpeg.AVIOContext.read_seek" decl="false" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavformat\avio.h" line="107">
Seek to a given timestamp in stream with the specified stream_index.
Needed for some network streaming protocols which don't support seeking
to byte position.

</member>
<member name="F:libffmpeg.AVIOContext.seekable" decl="false" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavformat\avio.h" line="114">
A combination of AVIO_SEEKABLE_ flags or 0 when the stream is not seekable.

</member>
<member name="F:libffmpeg.AVIOContext.maxsize" decl="false" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavformat\avio.h" line="119">
max filesize, used to limit allocations
This field is internal to libavformat and access from outside is not allowed.

</member>
<member name="F:libffmpeg.AVIOContext.direct" decl="false" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavformat\avio.h" line="125">
avio_read and avio_write should if possible be satisfied directly
instead of going through a buffer, and avio_seek will always
call the underlying seek function directly.

</member>
<member name="F:libffmpeg.AVIOContext.bytes_read" decl="false" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavformat\avio.h" line="132">
Bytes read statistic
This field is internal to libavformat and access from outside is not allowed.

</member>
<member name="F:libffmpeg.AVIOContext.seek_count" decl="false" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavformat\avio.h" line="138">
seek statistic
This field is internal to libavformat and access from outside is not allowed.

</member>
<member name="F:libffmpeg.AVIOContext.writeout_count" decl="false" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavformat\avio.h" line="144">
writeout statistic
This field is internal to libavformat and access from outside is not allowed.

</member>
<member name="F:libffmpeg.AVIOContext.orig_buffer_size" decl="false" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavformat\avio.h" line="150">
Original buffer size
used internally after probing and ensure seekback to reset the buffer size
This field is internal to libavformat and access from outside is not allowed.

</member>
<member name="M:libffmpeg.avio_find_protocol_name(System.SByte!System.Runtime.CompilerServices.IsSignUnspecifiedByte!System.Runtime.CompilerServices.IsConst*)" decl="true" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavformat\avio.h" line="160">
 Return the name of the protocol that will handle the passed URL.

 NULL is returned if no protocol could be found for the given URL.

 @return Name of the protocol or NULL.

</member>
<member name="M:libffmpeg.avio_check(System.SByte!System.Runtime.CompilerServices.IsSignUnspecifiedByte!System.Runtime.CompilerServices.IsConst*,System.Int32)" decl="true" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavformat\avio.h" line="169">
 Return AVIO_FLAG_* access flags corresponding to the access permissions
 of the resource in url, or a negative value corresponding to an
 AVERROR code in case of failure. The returned access flags are
 masked by the value in flags.

 @note This function is intrinsically unsafe, in the sense that the
 checked resource may change its existence or permission status from
 one call to another. Thus you should not trust the returned value,
 unless you are sure that no other processes are accessing the
 checked resource.

</member>
<member name="M:libffmpeg.avio_alloc_context(System.Byte*,System.Int32,System.Int32,System.Void*,=FUNC:System.Int32(System.Void*,System.Byte*,System.Int32),=FUNC:System.Int32(System.Void*,System.Byte*,System.Int32),=FUNC:System.Int64(System.Void*,System.Int64,System.Int32))" decl="true" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavformat\avio.h" line="183">
 Allocate and initialize an AVIOContext for buffered I/O. It must be later
 freed with av_free().

 @param buffer Memory block for input/output operations via AVIOContext.
        The buffer must be allocated with av_malloc() and friends.
        It may be freed and replaced with a new buffer by libavformat.
        AVIOContext.buffer holds the buffer currently in use,
        which must be later freed with av_free().
 @param buffer_size The buffer size is very important for performance.
        For protocols with fixed blocksize it should be set to this blocksize.
        For others a typical size is a cache page, e.g. 4kb.
 @param write_flag Set to 1 if the buffer should be writable, 0 otherwise.
 @param opaque An opaque pointer to user-specific data.
 @param read_packet  A function for refilling the buffer, may be NULL.
 @param write_packet A function for writing the buffer contents, may be NULL.
        The function may not change the input buffers content.
 @param seek A function for seeking to specified byte position, may be NULL.

 @return Allocated AVIOContext or NULL on failure.

</member>
<member name="M:libffmpeg.avio_put_str(libffmpeg.AVIOContext*,System.SByte!System.Runtime.CompilerServices.IsSignUnspecifiedByte!System.Runtime.CompilerServices.IsConst*)" decl="true" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavformat\avio.h" line="224">
Write a NULL-terminated string.
@return number of bytes written.

</member>
<member name="M:libffmpeg.avio_put_str16le(libffmpeg.AVIOContext*,System.SByte!System.Runtime.CompilerServices.IsSignUnspecifiedByte!System.Runtime.CompilerServices.IsConst*)" decl="true" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavformat\avio.h" line="230">
Convert an UTF-8 string to UTF-16LE and write it.
@return number of bytes written.

</member>
<member name="M:libffmpeg.avio_put_str16be(libffmpeg.AVIOContext*,System.SByte!System.Runtime.CompilerServices.IsSignUnspecifiedByte!System.Runtime.CompilerServices.IsConst*)" decl="true" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavformat\avio.h" line="236">
Convert an UTF-8 string to UTF-16BE and write it.
@return number of bytes written.

</member>
<!-- Discarding badly formed XML document comment for member 'M:libffmpeg.avio_seek(libffmpeg.AVIOContext*,System.Int64,System.Int32)'. -->
<member name="M:libffmpeg.avio_skip(libffmpeg.AVIOContext*,System.Int64)" decl="true" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavformat\avio.h" line="263">
Skip given number of bytes forward
@return new position or AVERROR.

</member>
<member name="M:libffmpeg.avio_tell(libffmpeg.AVIOContext*)" decl="false" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavformat\avio.h" line="269">
ftell() equivalent for AVIOContext.
@return position or AVERROR.

</member>
<member name="M:libffmpeg.avio_size(libffmpeg.AVIOContext*)" decl="true" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavformat\avio.h" line="278">
Get the filesize.
@return filesize or AVERROR

</member>
<member name="M:libffmpeg.avio_feof(libffmpeg.AVIOContext*)" decl="true" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavformat\avio.h" line="284">
feof() equivalent for AVIOContext.
@return non zero if and only if end of file

</member>
<member name="M:libffmpeg.url_feof(libffmpeg.AVIOContext*)" decl="true" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavformat\avio.h" line="290">
@deprecated use avio_feof()

</member>
<member name="M:libffmpeg.avio_printf(libffmpeg.AVIOContext*,System.SByte!System.Runtime.CompilerServices.IsSignUnspecifiedByte!System.Runtime.CompilerServices.IsConst*,BTEllipsis)" decl="true" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavformat\avio.h" line="297">
@warning currently size is limited 
</member>
<member name="M:libffmpeg.avio_flush(libffmpeg.AVIOContext*)" decl="true" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavformat\avio.h" line="300">
 Force flushing of buffered data.

 For write streams, force the buffered data to be immediately written to the output,
 without to wait to fill the internal buffer.

 For read streams, discard all currently buffered data, and advance the
 reported file position to that of the underlying stream. This does not
 read new data, and does not perform any seeks.

</member>
<member name="M:libffmpeg.avio_read(libffmpeg.AVIOContext*,System.Byte*,System.Int32)" decl="true" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavformat\avio.h" line="312">
Read size bytes from AVIOContext into buf.
@return number of bytes read or AVERROR

</member>
<member name="M:libffmpeg.avio_r8(libffmpeg.AVIOContext*)" decl="true" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavformat\avio.h" line="318">
 @name Functions for reading from AVIOContext
 @{

 @note return 0 if EOF, so you cannot use it if EOF handling is
       necessary

</member>
<!-- Discarding badly formed XML document comment for member 'M:libffmpeg.avio_get_str(libffmpeg.AVIOContext*,System.Int32,System.SByte!System.Runtime.CompilerServices.IsSignUnspecifiedByte*,System.Int32)'. -->
<!-- Discarding badly formed XML document comment for member 'M:libffmpeg.avio_get_str16le(libffmpeg.AVIOContext*,System.Int32,System.SByte!System.Runtime.CompilerServices.IsSignUnspecifiedByte*,System.Int32)'. -->
<member name="M:libffmpeg.avio_open(libffmpeg.AVIOContext**,System.SByte!System.Runtime.CompilerServices.IsSignUnspecifiedByte!System.Runtime.CompilerServices.IsConst*,System.Int32)" decl="true" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavformat\avio.h" line="362">
@name URL open modes
The flags argument to avio_open must be one of the following
constants, optionally ORed with other flags.
@{

@}

Use non-blocking mode.
If this flag is set, operations on the context will return
AVERROR(EAGAIN) if they can not be performed immediately.
If this flag is not set, operations on the context will never return
AVERROR(EAGAIN).
Note that this flag does not affect the opening/connecting of the
context. Connecting a protocol will always block if necessary (e.g. on
network protocols) but never hang (e.g. on busy devices).
Warning: non-blocking protocols is work-in-progress; this flag may be
silently ignored.

Use direct mode.
avio_read and avio_write should if possible be satisfied directly
instead of going through a buffer, and avio_seek will always
call the underlying seek function directly.

 Create and initialize a AVIOContext for accessing the
 resource indicated by url.
 @note When the resource indicated by url has been opened in
 read+write mode, the AVIOContext can be used only for writing.

 @param s Used to return the pointer to the created AVIOContext.
 In case of failure the pointed to value is set to NULL.
 @param url resource to access
 @param flags flags which control how the resource indicated by url
 is to be opened
 @return &gt;= 0 in case of success, a negative value corresponding to an
 AVERROR code in case of failure

</member>
<member name="M:libffmpeg.avio_open2(libffmpeg.AVIOContext**,System.SByte!System.Runtime.CompilerServices.IsSignUnspecifiedByte!System.Runtime.CompilerServices.IsConst*,System.Int32,libffmpeg.AVIOInterruptCB!System.Runtime.CompilerServices.IsConst*,libffmpeg.AVDictionary**)" decl="true" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavformat\avio.h" line="413">
 Create and initialize a AVIOContext for accessing the
 resource indicated by url.
 @note When the resource indicated by url has been opened in
 read+write mode, the AVIOContext can be used only for writing.

 @param s Used to return the pointer to the created AVIOContext.
 In case of failure the pointed to value is set to NULL.
 @param url resource to access
 @param flags flags which control how the resource indicated by url
 is to be opened
 @param int_cb an interrupt callback to be used at the protocols level
 @param options  A dictionary filled with protocol-private options. On return
 this parameter will be destroyed and replaced with a dict containing options
 that were not found. May be NULL.
 @return &gt;= 0 in case of success, a negative value corresponding to an
 AVERROR code in case of failure

</member>
<!-- Discarding badly formed XML document comment for member 'M:libffmpeg.avio_close(libffmpeg.AVIOContext*)'. -->
<!-- Discarding badly formed XML document comment for member 'M:libffmpeg.avio_closep(libffmpeg.AVIOContext**)'. -->
<member name="M:libffmpeg.avio_open_dyn_buf(libffmpeg.AVIOContext**)" decl="true" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavformat\avio.h" line="460">
 Open a write only memory stream.

 @param s new IO context
 @return zero if no error.

</member>
<member name="M:libffmpeg.avio_close_dyn_buf(libffmpeg.AVIOContext*,System.Byte**)" decl="true" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavformat\avio.h" line="468">
 Return the written size and a pointer to the buffer. The buffer
 must be freed with av_free().
 Padding of FF_INPUT_BUFFER_PADDING_SIZE is added to the buffer.

 @param s IO context
 @param pbuffer pointer to a byte buffer
 @return the length of the byte buffer

</member>
<member name="M:libffmpeg.avio_enum_protocols(System.Void**,System.Int32)" decl="true" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavformat\avio.h" line="479">
 Iterate through names of available protocols.

 @param opaque A private pointer representing current protocol.
        It must be a pointer to NULL on first iteration and will
        be updated by successive calls to avio_enum_protocols.
 @param output If set to 1, iterate over output protocols,
               otherwise over input protocols.

 @return A static string containing the name of current protocol or NULL

</member>
<member name="M:libffmpeg.avio_pause(libffmpeg.AVIOContext*,System.Int32)" decl="true" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavformat\avio.h" line="492">
 Pause and resume playing - only meaningful if using a network streaming
 protocol (e.g. MMS).

 @param h     IO context from which to call the read_pause function pointer
 @param pause 1 for pause, 0 for resume

</member>
<member name="M:libffmpeg.avio_seek_time(libffmpeg.AVIOContext*,System.Int32,System.Int64,System.Int32)" decl="true" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavformat\avio.h" line="501">
 Seek to a given timestamp relative to some component stream.
 Only meaningful if using a network streaming protocol (e.g. MMS.).

 @param h IO context from which to call the seek function pointers
 @param stream_index The stream index that the timestamp is relative to.
        If stream_index is (-1) the timestamp should be in AV_TIME_BASE
        units from the beginning of the presentation.
        If a stream_index &gt;= 0 is used and the protocol does not support
        seeking based on component streams, the call will fail.
 @param timestamp timestamp in AVStream.time_base units
        or if there is no stream specified then in AV_TIME_BASE units.
 @param flags Optional combination of AVSEEK_FLAG_BACKWARD, AVSEEK_FLAG_BYTE
        and AVSEEK_FLAG_ANY. The protocol may silently ignore
        AVSEEK_FLAG_BACKWARD and AVSEEK_FLAG_ANY, but AVSEEK_FLAG_BYTE will
        fail if used and not supported.
 @return &gt;= 0 on success
 @see AVInputFormat::read_seek

</member>
<member name="M:libffmpeg.avio_read_to_bprint(libffmpeg.AVIOContext*,libffmpeg.AVBPrint*,System.UInt32)" decl="true" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavformat\avio.h" line="526">
 Read contents of h into print buffer, up to max_size bytes, or up to EOF.

 @return 0 for success (max_size bytes read or EOF reached), negative error
 code otherwise

</member>
<!-- Discarding badly formed XML document comment for member 'M:libffmpeg.av_get_packet(libffmpeg.AVIOContext*,libffmpeg.AVPacket*,System.Int32)'. -->
<member name="M:libffmpeg.av_append_packet(libffmpeg.AVIOContext*,libffmpeg.AVPacket*,System.Int32)" decl="true" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavformat\avformat.h" line="364">
 Read data and append it to the current content of the AVPacket.
 If pkt-&gt;size is 0 this is identical to av_get_packet.
 Note that this uses av_grow_packet and thus involves a realloc
 which is inefficient. Thus this function should only be used
 when there is no reasonable way to know (an upper bound of)
 the final size.

 @param s    associated IO context
 @param pkt packet
 @param size amount of data to read
 @return &gt;0 (read size) if OK, AVERROR_xxx otherwise, previous data
         will not be lost even if an error occurs.

</member>
<!-- Discarding badly formed XML document comment for member 'T:libffmpeg.AVFrac'. -->
<member name="T:libffmpeg.AVProbeData" decl="false" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavformat\avformat.h" line="398">
This structure contains the data a format has to probe a file.

</member>
<!-- Discarding badly formed XML document comment for member 'T:libffmpeg.AVOutputFormat'. -->
<member name="F:libffmpeg.AVOutputFormat.long_name" decl="false" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavformat\avformat.h" line="459">
Descriptive name for the format, meant to be more human-readable
than name. You should use the NULL_IF_CONFIG_SMALL() macro
to define it.

</member>
<member name="F:libffmpeg.AVOutputFormat.flags" decl="false" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavformat\avformat.h" line="471">
can use flags: AVFMT_NOFILE, AVFMT_NEEDNUMBER, AVFMT_RAWPICTURE,
AVFMT_GLOBALHEADER, AVFMT_NOTIMESTAMPS, AVFMT_VARIABLE_FPS,
AVFMT_NODIMENSIONS, AVFMT_NOSTREAMS, AVFMT_ALLOW_FLUSH,
AVFMT_TS_NONSTRICT

</member>
<member name="T:libffmpeg.AVCodecTag" decl="true" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavformat\avformat.h" line="479">
List of supported codec_id-codec_tag pairs, ordered by "better
choice first". The arrays are all terminated by AV_CODEC_ID_NONE.

</member>
<member name="F:libffmpeg.AVOutputFormat.priv_data_size" decl="false" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavformat\avformat.h" line="496">
size of private data so that it can be allocated in the wrapper

</member>
<member name="F:libffmpeg.AVOutputFormat.write_packet" decl="false" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavformat\avformat.h" line="502">
Write a packet. If AVFMT_ALLOW_FLUSH is set in flags,
pkt can be NULL in order to flush data buffered in the muxer.
When flushing, return 0 if there still is more data to flush,
or 1 if everything was flushed and there is no more buffered
data.

</member>
<member name="F:libffmpeg.AVOutputFormat.interleave_packet" decl="false" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavformat\avformat.h" line="511">
Currently only used to set pixel format if not YUV420P.

</member>
<member name="F:libffmpeg.AVOutputFormat.query_codec" decl="false" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavformat\avformat.h" line="516">
 Test if the given codec can be stored in this container.

 @return 1 if the codec is supported, 0 if it is not.
         A negative number if unknown.
         MKTAG('A', 'P', 'I', 'C') if the codec is only supported as AV_DISPOSITION_ATTACHED_PIC

</member>
<member name="F:libffmpeg.AVOutputFormat.control_message" decl="false" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavformat\avformat.h" line="527">
Allows sending messages from application to device.

</member>
<member name="F:libffmpeg.AVOutputFormat.write_uncoded_frame" decl="false" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavformat\avformat.h" line="533">
 Write an uncoded AVFrame.

 See av_write_uncoded_frame() for details.

 The library will free *frame afterwards, but the muxer can prevent it
 by setting the pointer to NULL.

</member>
<member name="F:libffmpeg.AVOutputFormat.get_device_list" decl="false" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavformat\avformat.h" line="543">
Returns device list with it properties.
@see avdevice_list_devices() for more details.

</member>
<member name="F:libffmpeg.AVOutputFormat.create_device_capabilities" decl="false" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavformat\avformat.h" line="548">
Initialize device capabilities submodule.
@see avdevice_capabilities_create() for more details.

</member>
<member name="F:libffmpeg.AVOutputFormat.free_device_capabilities" decl="false" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavformat\avformat.h" line="553">
Free device capabilities submodule.
@see avdevice_capabilities_free() for more details.

</member>
<member name="T:libffmpeg.AVInputFormat" decl="false" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavformat\avformat.h" line="560">
@}

@addtogroup lavf_decoding
@{

</member>
<member name="F:libffmpeg.AVInputFormat.name" decl="false" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavformat\avformat.h" line="569">
A comma separated list of short names for the format. New names
may be appended with a minor bump.

</member>
<member name="F:libffmpeg.AVInputFormat.long_name" decl="false" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavformat\avformat.h" line="575">
Descriptive name for the format, meant to be more human-readable
than name. You should use the NULL_IF_CONFIG_SMALL() macro
to define it.

</member>
<member name="F:libffmpeg.AVInputFormat.flags" decl="false" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavformat\avformat.h" line="582">
Can use flags: AVFMT_NOFILE, AVFMT_NEEDNUMBER, AVFMT_SHOW_IDS,
AVFMT_GENERIC_INDEX, AVFMT_TS_DISCONT, AVFMT_NOBINSEARCH,
AVFMT_NOGENSEARCH, AVFMT_NO_BYTE_SEEK, AVFMT_SEEK_TO_PTS.

</member>
<member name="F:libffmpeg.AVInputFormat.extensions" decl="false" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavformat\avformat.h" line="589">
If extensions are defined, then no probe is done. You should
usually not use extension format guessing because it is not
reliable enough

</member>
<member name="F:libffmpeg.AVInputFormat.mime_type" decl="false" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavformat\avformat.h" line="600">
Comma-separated list of mime types.
It is used check for matching mime types while probing.
@see av_probe_input_format2

</member>
<member name="F:libffmpeg.AVInputFormat.raw_codec_id" decl="false" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavformat\avformat.h" line="616">
Raw demuxers store their codec ID here.

</member>
<member name="F:libffmpeg.AVInputFormat.priv_data_size" decl="false" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavformat\avformat.h" line="621">
Size of private data so that it can be allocated in the wrapper.

</member>
<member name="F:libffmpeg.AVInputFormat.read_probe" decl="false" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavformat\avformat.h" line="626">
Tell if a given file has a chance of being parsed as this format.
The buffer provided is guaranteed to be AVPROBE_PADDING_SIZE bytes
big so you do not have to check for that unless you need more.

</member>
<member name="F:libffmpeg.AVInputFormat.read_header" decl="false" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavformat\avformat.h" line="633">
Read the format header and initialize the AVFormatContext
structure. Return 0 if OK. 'avformat_new_stream' should be
called to create new streams.

</member>
<!-- Discarding badly formed XML document comment for member 'F:libffmpeg.AVInputFormat.read_packet'. -->
<member name="F:libffmpeg.AVInputFormat.read_close" decl="false" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavformat\avformat.h" line="651">
Close the stream. The AVFormatContext and AVStreams are not
freed by this function

</member>
<member name="F:libffmpeg.AVInputFormat.read_seek" decl="false" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavformat\avformat.h" line="657">
Seek to a given timestamp relative to the frames in
stream component stream_index.
@param stream_index Must not be -1.
@param flags Selects which direction should be preferred if no exact
             match is available.
@return &gt;= 0 on success (but not necessarily the new offset)

</member>
<member name="F:libffmpeg.AVInputFormat.read_timestamp" decl="false" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavformat\avformat.h" line="668">
Get the next timestamp in stream[stream_index].time_base units.
@return the timestamp or AV_NOPTS_VALUE if an error occurred

</member>
<member name="F:libffmpeg.AVInputFormat.read_play" decl="false" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavformat\avformat.h" line="675">
Start/resume playing - only meaningful if using a network-based format
(RTSP).

</member>
<member name="F:libffmpeg.AVInputFormat.read_pause" decl="false" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavformat\avformat.h" line="681">
Pause playing - only meaningful if using a network-based format
(RTSP).

</member>
<!-- Discarding badly formed XML document comment for member 'F:libffmpeg.AVInputFormat.read_seek2'. -->
<member name="F:libffmpeg.AVInputFormat.get_device_list" decl="false" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavformat\avformat.h" line="695">
Returns device list with it properties.
@see avdevice_list_devices() for more details.

</member>
<member name="F:libffmpeg.AVInputFormat.create_device_capabilities" decl="false" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavformat\avformat.h" line="701">
Initialize device capabilities submodule.
@see avdevice_capabilities_create() for more details.

</member>
<member name="F:libffmpeg.AVInputFormat.free_device_capabilities" decl="false" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavformat\avformat.h" line="707">
Free device capabilities submodule.
@see avdevice_capabilities_free() for more details.

</member>
<member name="T:libffmpeg.AVStreamParseType" decl="false" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavformat\avformat.h" line="713">
@}

</member>
<member name="T:libffmpeg.AVStream" decl="false" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavformat\avformat.h" line="749">
Track should be used during playback by default.
Useful for subtitle track that should be displayed
even when user did not explicitly ask for subtitles.

The stream is stored in the file as an attached picture/"cover art" (e.g.
APIC frame in ID3v2). The single packet associated with it will be returned
among the first few packets read from the file unless seeking takes place.
It can also be accessed at any time in AVStream.attached_pic.

To specify text track kind (different from subtitles default).

Options for behavior on timestamp wrap detection.

Stream structure.
New fields can be added to the end with minor version bumps.
Removal, reordering and changes to existing fields require a major
version bump.
sizeof(AVStream) must not be used outside libav*.

</member>
<member name="F:libffmpeg.AVStream.id" decl="false" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavformat\avformat.h" line="789">
Format-specific stream ID.
decoding: set by libavformat
encoding: set by the user, replaced by libavformat if left unset

</member>
<member name="F:libffmpeg.AVStream.codec" decl="false" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavformat\avformat.h" line="795">
 Codec context associated with this stream. Allocated and freed by
 libavformat.

 - decoding: The demuxer exports codec information stored in the headers
             here.
 - encoding: The user sets codec information, the muxer writes it to the
             output. Mandatory fields as specified in AVCodecContext
             documentation must be set even if this AVCodecContext is
             not actually used for encoding.

</member>
<member name="T:libffmpeg.AVFrac" decl="true" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavformat\avformat.h" line="810">
@deprecated this field is unused

</member>
<member name="F:libffmpeg.AVStream.time_base" decl="false" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavformat\avformat.h" line="817">
 This is the fundamental unit of time (in seconds) in terms
 of which frame timestamps are represented.

 decoding: set by libavformat
 encoding: May be set by the caller before avformat_write_header() to
           provide a hint to the muxer about the desired timebase. In
           avformat_write_header(), the muxer will overwrite this field
           with the timebase that will actually be used for the timestamps
           written into the file (which may or may not be related to the
           user-provided one, depending on the format).

</member>
<member name="F:libffmpeg.AVStream.start_time" decl="false" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavformat\avformat.h" line="831">
Decoding: pts of the first frame of the stream in presentation order, in stream time base.
Only set this if you are absolutely 100% sure that the value you set
it to really is the pts of the first frame.
This may be undefined (AV_NOPTS_VALUE).
@note The ASF header does NOT contain a correct start_time the ASF
demuxer must NOT set this.

</member>
<member name="F:libffmpeg.AVStream.duration" decl="false" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavformat\avformat.h" line="841">
Decoding: duration of the stream, in stream time base.
If a source file does not specify a duration, but does specify
a bitrate, this value will be estimated from bitrate and file size.

</member>
<member name="F:libffmpeg.AVStream.sample_aspect_ratio" decl="false" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavformat\avformat.h" line="854">
sample aspect ratio (0 if unknown)
- encoding: Set by user.
- decoding: Set by libavformat.

</member>
<member name="F:libffmpeg.AVStream.avg_frame_rate" decl="false" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavformat\avformat.h" line="863">
 Average framerate

 - demuxing: May be set by libavformat when creating the stream or in
             avformat_find_stream_info().
 - muxing: May be set by the caller before avformat_write_header().

</member>
<member name="F:libffmpeg.AVStream.attached_pic" decl="false" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavformat\avformat.h" line="872">
 For streams with AV_DISPOSITION_ATTACHED_PIC disposition, this packet
 will contain the attached picture.

 decoding: set by libavformat, must not be modified by the caller.
 encoding: unused

</member>
<member name="F:libffmpeg.AVStream.side_data" decl="false" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavformat\avformat.h" line="881">
 An array of side data that applies to the whole stream (i.e. the
 container does not allow it to change between packets).

 There may be no overlap between the side data in this array and side data
 in the packets. I.e. a given side data is either exported by the muxer
 (demuxing) / set by the caller (muxing) in this array, then it never
 appears in the packets, or the side data is exported / sent through
 the packets (always in the first packet where the value becomes known or
 changes), then it does not appear in this array.

 - demuxing: Set by libavformat when the stream is created.
 - muxing: May be set by the caller before avformat_write_header().

 Freed by libavformat in avformat_free_context().

 @see av_format_inject_global_side_data()

</member>
<member name="F:libffmpeg.AVStream.nb_side_data" decl="false" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavformat\avformat.h" line="900">
The number of elements in the AVStream.side_data array.

</member>
<member name="F:libffmpeg.AVStream.event_flags" decl="false" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavformat\avformat.h" line="905">
Flags for the user to detect events happening on the stream. Flags must
be cleared by the user once the event has been handled.
A combination of AVSTREAM_EVENT_FLAG_*.

</member>
<member name="F:libffmpeg.AVStream.first_dts" decl="false" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavformat\avformat.h" line="956">
 Timestamp corresponding to the last dts sync point.

 Initialized when AVCodecParserContext.dts_sync_point &gt;= 0 and
 a DTS is received from the underlying container. Otherwise set to
 AV_NOPTS_VALUE by default.

</member>
<member name="F:libffmpeg.AVStream.probe_packets" decl="false" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavformat\avformat.h" line="968">
Number of packets to buffer for codec probing

</member>
<member name="F:libffmpeg.AVStream.codec_info_nb_frames" decl="false" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavformat\avformat.h" line="974">
Number of frames that have been demuxed during av_find_stream_info()

</member>
<member name="T:libffmpeg.AVPacketList" decl="true" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavformat\avformat.h" line="983">
last packet in packet_buffer for this stream when muxing.

</member>
<member name="F:libffmpeg.AVStream.r_frame_rate" decl="false" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavformat\avformat.h" line="996">
 Real base framerate of the stream.
 This is the lowest framerate with which all timestamps can be
 represented accurately (it is the least common multiple of all
 framerates in the stream). Note, this value is just a guess!
 For example, if the time base is 1/90000 and all frames have either
 approximately 3600 or 1800 timer ticks, then r_frame_rate will be 50/1.

 Code outside avformat should access this field using:
 av_stream_get/set_r_frame_rate(stream)

</member>
<member name="F:libffmpeg.AVStream.stream_identifier" decl="false" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavformat\avformat.h" line="1009">
Stream Identifier
This is the MPEG-TS stream identifier +1
0 means unknown

</member>
<member name="F:libffmpeg.AVStream.request_probe" decl="false" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavformat\avformat.h" line="1019">
stream probing state
-1   -&gt; probing finished
 0   -&gt; no probing requested
rest -&gt; perform probing with request_probe being the minimum score to accept.
NOT PART OF PUBLIC API

</member>
<member name="F:libffmpeg.AVStream.skip_to_keyframe" decl="false" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavformat\avformat.h" line="1027">
Indicates that everything up to the next keyframe
should be discarded.

</member>
<member name="F:libffmpeg.AVStream.skip_samples" decl="false" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavformat\avformat.h" line="1033">
Number of samples to skip at the start of the frame decoded from the next packet.

</member>
<member name="F:libffmpeg.AVStream.first_discard_sample" decl="false" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavformat\avformat.h" line="1038">
If not 0, the first audio sample that should be discarded from the stream.
This is broken by design (needs global sample count), but can't be
avoided for broken by design formats such as mp3 with ad-hoc gapless
audio support.

</member>
<member name="F:libffmpeg.AVStream.last_discard_sample" decl="false" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavformat\avformat.h" line="1046">
The sample after last sample that is intended to be discarded after
first_discard_sample. Works on frame boundaries only. Used to prevent
early EOF if the gapless info is broken (considered concatenated mp3s).

</member>
<member name="F:libffmpeg.AVStream.nb_decoded_frames" decl="false" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavformat\avformat.h" line="1053">
Number of internally decoded frames, used internally in libavformat, do not access
its lifetime differs from info which is why it is not in that structure.

</member>
<member name="F:libffmpeg.AVStream.mux_ts_offset" decl="false" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavformat\avformat.h" line="1059">
Timestamp offset added to timestamps before muxing
NOT PART OF PUBLIC API

</member>
<member name="F:libffmpeg.AVStream.pts_wrap_reference" decl="false" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavformat\avformat.h" line="1065">
Internal data to check for wrapping of the time stamp

</member>
<member name="F:libffmpeg.AVStream.pts_wrap_behavior" decl="false" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavformat\avformat.h" line="1070">
 Options for behavior, when a wrap is detected.

 Defined by AV_PTS_WRAP_ values.

 If correction is enabled, there are two possibilities:
 If the first time stamp is near the wrap point, the wrap offset
 will be subtracted, which will create negative time stamps.
 Otherwise the offset will be added.

</member>
<member name="F:libffmpeg.AVStream.update_initial_durations_done" decl="false" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavformat\avformat.h" line="1082">
Internal data to prevent doing update_initial_durations() twice

</member>
<member name="F:libffmpeg.AVStream.pts_reorder_error" decl="false" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavformat\avformat.h" line="1087">
Internal data to generate dts from pts

</member>
<member name="F:libffmpeg.AVStream.last_dts_for_order_check" decl="false" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavformat\avformat.h" line="1093">
Internal data to analyze DTS and detect faulty mpeg streams

</member>
<member name="F:libffmpeg.AVStream.inject_global_side_data" decl="false" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavformat\avformat.h" line="1100">
Internal data to inject global side data

</member>
<member name="F:libffmpeg.AVStream.recommended_encoder_configuration" decl="false" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavformat\avformat.h" line="1105">
String containing paris of key and values describing recommended encoder configuration.
Paris are separated by ','.
Keys are separated from values by '='.

</member>
<member name="F:libffmpeg.AVStream.display_aspect_ratio" decl="false" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavformat\avformat.h" line="1112">
display aspect ratio (0 if unknown)
- encoding: unused
- decoding: Set by libavformat to calculate sample_aspect_ratio internally

</member>
<member name="M:libffmpeg.av_stream_get_end_pts(libffmpeg.AVStream!System.Runtime.CompilerServices.IsConst*)" decl="true" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavformat\avformat.h" line="1126">
 Returns the pts of the last muxed packet + its duration

 the retuned value is undefined when used with a demuxer.

</member>
<member name="T:libffmpeg.AVProgram" decl="false" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavformat\avformat.h" line="1135">
New fields can be added to the end with minor version bumps.
Removal, reordering and changes to existing fields require a major
version bump.
sizeof(AVProgram) must not be used outside libav*.

</member>
<member name="D:libffmpeg.av_format_control_message" decl="false" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavformat\avformat.h" line="1178">
Callback used by devices to communicate with application.

</member>
<member name="T:libffmpeg.AVDurationEstimationMethod" decl="false" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavformat\avformat.h" line="1185">
The duration of a video can be estimated through various ways, and this enum can be used
to know how the duration was estimated.

</member>
<member name="T:libffmpeg.AVFormatContext" decl="false" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavformat\avformat.h" line="1197">
Format I/O context.
New fields can be added to the end with minor version bumps.
Removal, reordering and changes to existing fields require a major
version bump.
sizeof(AVFormatContext) must not be used outside libav*, use
avformat_alloc_context() to create an AVFormatContext.

</member>
<member name="F:libffmpeg.AVFormatContext.av_class" decl="false" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavformat\avformat.h" line="1206">
A class for logging and @ref avoptions. Set by avformat_alloc_context().
Exports (de)muxer private options if they exist.

</member>
<member name="T:libffmpeg.AVInputFormat" decl="true" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavformat\avformat.h" line="1212">
 The input container format.

 Demuxing only, set by avformat_open_input().

</member>
<member name="T:libffmpeg.AVOutputFormat" decl="true" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavformat\avformat.h" line="1219">
 The output container format.

 Muxing only, must be set by the caller before avformat_write_header().

</member>
<member name="F:libffmpeg.AVFormatContext.priv_data" decl="false" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavformat\avformat.h" line="1226">
 Format private data. This is an AVOptions-enabled struct
 if and only if iformat/oformat.priv_class is not NULL.

 - muxing: set by avformat_write_header()
 - demuxing: set by avformat_open_input()

</member>
<member name="F:libffmpeg.AVFormatContext.pb" decl="false" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavformat\avformat.h" line="1235">
 I/O context.

 - demuxing: either set by the user before avformat_open_input() (then
             the user must close it manually) or set by avformat_open_input().
 - muxing: set by the user before avformat_write_header(). The caller must
           take care of closing / freeing the IO context.

 Do NOT set this field if AVFMT_NOFILE flag is set in
 iformat/oformat.flags. In such a case, the (de)muxer will handle
 I/O in some other way and this field will be NULL.

</member>
<member name="F:libffmpeg.AVFormatContext.ctx_flags" decl="false" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavformat\avformat.h" line="1250">
Flags signalling stream properties. A combination of AVFMTCTX_*.
Set by libavformat.

</member>
<member name="F:libffmpeg.AVFormatContext.nb_streams" decl="false" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavformat\avformat.h" line="1256">
 Number of elements in AVFormatContext.streams.

 Set by avformat_new_stream(), must not be modified by any other code.

</member>
<member name="F:libffmpeg.AVFormatContext.streams" decl="false" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavformat\avformat.h" line="1262">
 A list of all streams in the file. New streams are created with
 avformat_new_stream().

 - demuxing: streams are created by libavformat in avformat_open_input().
             If AVFMTCTX_NOHEADER is set in ctx_flags, then new streams may also
             appear in av_read_frame().
 - muxing: streams are created by the user before avformat_write_header().

 Freed by libavformat in avformat_free_context().

</member>
<member name="F:libffmpeg.AVFormatContext.filename" decl="false" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavformat\avformat.h" line="1275">
 input or output filename

 - demuxing: set by avformat_open_input()
 - muxing: may be set by the caller before avformat_write_header()

</member>
<member name="F:libffmpeg.AVFormatContext.start_time" decl="false" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavformat\avformat.h" line="1283">
 Position of the first frame of the component, in
 AV_TIME_BASE fractional seconds. NEVER set this value directly:
 It is deduced from the AVStream values.

 Demuxing only, set by libavformat.

</member>
<member name="F:libffmpeg.AVFormatContext.duration" decl="false" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavformat\avformat.h" line="1292">
 Duration of the stream, in AV_TIME_BASE fractional
 seconds. Only set this value if you know none of the individual stream
 durations and also do not set any of them. This is deduced from the
 AVStream values if not set.

 Demuxing only, set by libavformat.

</member>
<member name="F:libffmpeg.AVFormatContext.bit_rate" decl="false" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavformat\avformat.h" line="1302">
Total stream bitrate in bit/s, 0 if not
available. Never set it directly if the file_size and the
duration are known as FFmpeg can compute it automatically.

</member>
<member name="F:libffmpeg.AVFormatContext.flags" decl="false" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavformat\avformat.h" line="1312">
Flags modifying the (de)muxer behaviour. A combination of AVFMT_FLAG_*.
Set by the user before avformat_open_input() / avformat_write_header().

</member>
<member name="F:libffmpeg.AVFormatContext.probesize" decl="false" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavformat\avformat.h" line="1327">
 When muxing, try to avoid writing any random/volatile data to the output.
 This includes any random IDs, real-time timestamps/dates, muxer version, etc.

 This flag is mainly intended for testing.

@deprecated deprecated in favor of probesize2

</member>
<member name="F:libffmpeg.AVFormatContext.max_analyze_duration" decl="false" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavformat\avformat.h" line="1344">
@deprecated deprecated in favor of max_analyze_duration2

</member>
<member name="T:libffmpeg.AVCodecID" decl="true" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavformat\avformat.h" line="1356">
Forced video codec_id.
Demuxing: Set by user.

</member>
<member name="T:libffmpeg.AVCodecID" decl="true" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavformat\avformat.h" line="1362">
Forced audio codec_id.
Demuxing: Set by user.

</member>
<member name="T:libffmpeg.AVCodecID" decl="true" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavformat\avformat.h" line="1368">
Forced subtitle codec_id.
Demuxing: Set by user.

</member>
<member name="F:libffmpeg.AVFormatContext.max_index_size" decl="false" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavformat\avformat.h" line="1374">
Maximum amount of memory in bytes to use for the index of each stream.
If the index exceeds this size, entries will be discarded as
needed to maintain a smaller size. This can lead to slower or less
accurate seeking (depends on demuxer).
Demuxers for which a full in-memory index is mandatory will ignore
this.
- muxing: unused
- demuxing: set by user

</member>
<member name="F:libffmpeg.AVFormatContext.max_picture_buffer" decl="false" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavformat\avformat.h" line="1386">
Maximum amount of memory in bytes to use for buffering frames
obtained from realtime capture devices.

</member>
<member name="F:libffmpeg.AVFormatContext.nb_chapters" decl="false" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavformat\avformat.h" line="1392">
Number of chapters in AVChapter array.
When muxing, chapters are normally written in the file header,
so nb_chapters should normally be initialized before write_header
is called. Some muxers (e.g. mov and mkv) can also write chapters
in the trailer.  To write chapters in the trailer, nb_chapters
must be zero when write_header is called and non-zero when
write_trailer is called.
- muxing: set by user
- demuxing: set by libavformat

</member>
<member name="F:libffmpeg.AVFormatContext.metadata" decl="false" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavformat\avformat.h" line="1406">
 Metadata that applies to the whole file.

 - demuxing: set by libavformat in avformat_open_input()
 - muxing: may be set by the caller before avformat_write_header()

 Freed by libavformat in avformat_free_context().

</member>
<member name="F:libffmpeg.AVFormatContext.start_time_realtime" decl="false" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavformat\avformat.h" line="1416">
Start time of the stream in real world time, in microseconds
since the Unix epoch (00:00 1st January 1970). That is, pts=0 in the
stream was captured at this real world time.
- muxing: Set by the caller before avformat_write_header(). If set to
          either 0 or AV_NOPTS_VALUE, then the current wall-time will
          be used.
- demuxing: Set by libavformat. AV_NOPTS_VALUE if unknown. Note that
            the value may become known after some number of frames
            have been received.

</member>
<member name="F:libffmpeg.AVFormatContext.fps_probe_size" decl="false" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavformat\avformat.h" line="1429">
The number of frames used for determining the framerate in
avformat_find_stream_info().
Demuxing only, set by the caller before avformat_find_stream_info().

</member>
<member name="F:libffmpeg.AVFormatContext.error_recognition" decl="false" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavformat\avformat.h" line="1436">
Error recognition; higher values will detect more errors but may
misdetect some more or less valid parts as errors.
Demuxing only, set by the caller before avformat_open_input().

</member>
<member name="F:libffmpeg.AVFormatContext.interrupt_callback" decl="false" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavformat\avformat.h" line="1443">
 Custom interrupt callbacks for the I/O layer.

 demuxing: set by the user before avformat_open_input().
 muxing: set by the user before avformat_write_header()
 (mainly useful for AVFMT_NOFILE formats). The callback
 should also be passed to avio_open2() if it's used to
 open the file.

</member>
<member name="F:libffmpeg.AVFormatContext.debug" decl="false" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavformat\avformat.h" line="1454">
Flags to enable debugging.

</member>
<member name="F:libffmpeg.AVFormatContext.max_interleave_delta" decl="false" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavformat\avformat.h" line="1460">
 Maximum buffering duration for interleaving.

 To ensure all the streams are interleaved correctly,
 av_interleaved_write_frame() will wait until it has at least one packet
 for each stream before actually writing any packets to the output file.
 When some streams are "sparse" (i.e. there are large gaps between
 successive packets), this can result in excessive buffering.

 This field specifies the maximum difference between the timestamps of the
 first and the last packet in the muxing queue, above which libavformat
 will output a packet regardless of whether it has queued a packet for all
 the streams.

 Muxing only, set by the caller before avformat_write_header().

</member>
<member name="F:libffmpeg.AVFormatContext.strict_std_compliance" decl="false" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavformat\avformat.h" line="1478">
Allow non-standard and experimental extension
@see AVCodecContext.strict_std_compliance

</member>
<member name="F:libffmpeg.AVFormatContext.event_flags" decl="false" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavformat\avformat.h" line="1484">
Flags for the user to detect events happening on the file. Flags must
be cleared by the user once the event has been handled.
A combination of AVFMT_EVENT_FLAG_*.

</member>
<member name="F:libffmpeg.AVFormatContext.max_ts_probe" decl="false" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavformat\avformat.h" line="1492">
Maximum number of packets to read while waiting for the first timestamp.
Decoding only.

</member>
<member name="F:libffmpeg.AVFormatContext.avoid_negative_ts" decl="false" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavformat\avformat.h" line="1498">
Avoid negative timestamps during muxing.
Any value of the AVFMT_AVOID_NEG_TS_* constants.
Note, this only works when using av_interleaved_write_frame. (interleave_packet_per_dts is in use)
- muxing: Set by user
- demuxing: unused

</member>
<member name="F:libffmpeg.AVFormatContext.ts_id" decl="false" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavformat\avformat.h" line="1510">
Transport stream id.
This will be moved into demuxer private options. Thus no API/ABI compatibility

</member>
<member name="F:libffmpeg.AVFormatContext.audio_preload" decl="false" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavformat\avformat.h" line="1516">
Audio preload in microseconds.
Note, not all formats support this and unpredictable things may happen if it is used when not supported.
- encoding: Set by user via AVOptions (NO direct access)
- decoding: unused

</member>
<member name="F:libffmpeg.AVFormatContext.max_chunk_duration" decl="false" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavformat\avformat.h" line="1524">
Max chunk time in microseconds.
Note, not all formats support this and unpredictable things may happen if it is used when not supported.
- encoding: Set by user via AVOptions (NO direct access)
- decoding: unused

</member>
<member name="F:libffmpeg.AVFormatContext.max_chunk_size" decl="false" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavformat\avformat.h" line="1532">
Max chunk size in bytes
Note, not all formats support this and unpredictable things may happen if it is used when not supported.
- encoding: Set by user via AVOptions (NO direct access)
- decoding: unused

</member>
<member name="F:libffmpeg.AVFormatContext.use_wallclock_as_timestamps" decl="false" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavformat\avformat.h" line="1540">
forces the use of wallclock timestamps as pts/dts of packets
This has undefined results in the presence of B frames.
- encoding: unused
- decoding: Set by user via AVOptions (NO direct access)

</member>
<member name="F:libffmpeg.AVFormatContext.avio_flags" decl="false" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavformat\avformat.h" line="1548">
avio flags, used to force AVIO_FLAG_DIRECT.
- encoding: unused
- decoding: Set by user via AVOptions (NO direct access)

</member>
<member name="T:libffmpeg.AVDurationEstimationMethod" decl="true" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavformat\avformat.h" line="1555">
The duration field can be estimated through various ways, and this field can be used
to know how the duration was estimated.
- encoding: unused
- decoding: Read by user via AVOptions (NO direct access)

</member>
<member name="F:libffmpeg.AVFormatContext.skip_initial_bytes" decl="false" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavformat\avformat.h" line="1563">
Skip initial bytes when opening stream
- encoding: unused
- decoding: Set by user via AVOptions (NO direct access)

</member>
<member name="F:libffmpeg.AVFormatContext.correct_ts_overflow" decl="false" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavformat\avformat.h" line="1570">
Correct single timestamp overflows
- encoding: unused
- decoding: Set by user via AVOptions (NO direct access)

</member>
<member name="F:libffmpeg.AVFormatContext.seek2any" decl="false" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavformat\avformat.h" line="1577">
Force seeking to any (also non key) frames.
- encoding: unused
- decoding: Set by user via AVOptions (NO direct access)

</member>
<member name="F:libffmpeg.AVFormatContext.flush_packets" decl="false" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavformat\avformat.h" line="1584">
Flush the I/O context after each packet.
- encoding: Set by user via AVOptions (NO direct access)
- decoding: unused

</member>
<member name="F:libffmpeg.AVFormatContext.probe_score" decl="false" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavformat\avformat.h" line="1591">
format probing score.
The maximal score is AVPROBE_SCORE_MAX, its set when the demuxer probes
the format.
- encoding: unused
- decoding: set by avformat, read by user via av_format_get_probe_score() (NO direct access)

</member>
<member name="F:libffmpeg.AVFormatContext.format_probesize" decl="false" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavformat\avformat.h" line="1600">
number of bytes to read maximally to identify format.
- encoding: unused
- decoding: set by user through AVOPtions (NO direct access)

</member>
<member name="F:libffmpeg.AVFormatContext.codec_whitelist" decl="false" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavformat\avformat.h" line="1607">
',' separated list of allowed decoders.
If NULL then all are allowed
- encoding: unused
- decoding: set by user through AVOptions (NO direct access)

</member>
<member name="F:libffmpeg.AVFormatContext.format_whitelist" decl="false" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavformat\avformat.h" line="1615">
',' separated list of allowed demuxers.
If NULL then all are allowed
- encoding: unused
- decoding: set by user through AVOptions (NO direct access)

</member>
<member name="F:libffmpeg.AVFormatContext.internal" decl="false" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavformat\avformat.h" line="1623">
An opaque field for libavformat internal usage.
Must not be accessed in any way by callers.

</member>
<member name="F:libffmpeg.AVFormatContext.io_repositioned" decl="false" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavformat\avformat.h" line="1629">
IO repositioned flag.
This is set by avformat when the underlaying IO context read pointer
is repositioned, for example when doing byte based seeking.
Demuxers can use the flag to detect such changes.

</member>
<member name="F:libffmpeg.AVFormatContext.video_codec" decl="false" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavformat\avformat.h" line="1637">
Forced video codec.
This allows forcing a specific decoder, even when there are multiple with
the same codec_id.
Demuxing: Set by user via av_format_set_video_codec (NO direct access).

</member>
<member name="F:libffmpeg.AVFormatContext.audio_codec" decl="false" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavformat\avformat.h" line="1645">
Forced audio codec.
This allows forcing a specific decoder, even when there are multiple with
the same codec_id.
Demuxing: Set by user via av_format_set_audio_codec (NO direct access).

</member>
<member name="F:libffmpeg.AVFormatContext.subtitle_codec" decl="false" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavformat\avformat.h" line="1653">
Forced subtitle codec.
This allows forcing a specific decoder, even when there are multiple with
the same codec_id.
Demuxing: Set by user via av_format_set_subtitle_codec (NO direct access).

</member>
<member name="F:libffmpeg.AVFormatContext.data_codec" decl="false" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavformat\avformat.h" line="1661">
Forced data codec.
This allows forcing a specific decoder, even when there are multiple with
the same codec_id.
Demuxing: Set by user via av_format_set_data_codec (NO direct access).

</member>
<member name="F:libffmpeg.AVFormatContext.metadata_header_padding" decl="false" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavformat\avformat.h" line="1669">
Number of bytes to be written as padding in a metadata header.
Demuxing: Unused.
Muxing: Set by user via av_format_set_metadata_header_padding.

</member>
<member name="F:libffmpeg.AVFormatContext.opaque" decl="false" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavformat\avformat.h" line="1676">
User data.
This is a place for some private data of the user.
Mostly usable with control_message_cb or any future callbacks in device's context.

</member>
<member name="F:libffmpeg.AVFormatContext.control_message_cb" decl="false" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavformat\avformat.h" line="1683">
Callback used by devices to communicate with application.

</member>
<member name="F:libffmpeg.AVFormatContext.output_ts_offset" decl="false" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavformat\avformat.h" line="1688">
Output timestamp offset, in microseconds.
Muxing: set by user via AVOptions (NO direct access)

</member>
<member name="F:libffmpeg.AVFormatContext.max_analyze_duration2" decl="false" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavformat\avformat.h" line="1694">
Maximum duration (in AV_TIME_BASE units) of the data read
from input in avformat_find_stream_info().
Demuxing only, set by the caller before avformat_find_stream_info()
via AVOptions (NO direct access).
Can be set to 0 to let avformat choose using a heuristic.

</member>
<member name="F:libffmpeg.AVFormatContext.probesize2" decl="false" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavformat\avformat.h" line="1703">
Maximum size of the data read from input for determining
the input container format.
Demuxing only, set by the caller before avformat_open_input()
via AVOptions (NO direct access).

</member>
<member name="F:libffmpeg.AVFormatContext.dump_separator" decl="false" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavformat\avformat.h" line="1711">
dump format separator.
can be ", " or "\n      " or anything else
Code outside libavformat should access this field using AVOptions
(NO direct access).
- muxing: Set by user.
- demuxing: Set by user.

</member>
<member name="T:libffmpeg.AVCodecID" decl="true" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavformat\avformat.h" line="1721">
Forced Data codec_id.
Demuxing: Set by user.

</member>
<member name="M:libffmpeg.av_format_inject_global_side_data(libffmpeg.AVFormatContext*)" decl="true" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavformat\avformat.h" line="1744">
This function will cause global side data to be injected in the next packet
of each stream as well as after any subsequent seek.

</member>
<member name="T:libffmpeg.AVDurationEstimationMethod" decl="true" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavformat\avformat.h" line="1750">
 Returns the method used to set ctx-&gt;duration.

 @return AVFMT_DURATION_FROM_PTS, AVFMT_DURATION_FROM_STREAM, or AVFMT_DURATION_FROM_BITRATE.

</member>
<member name="M:libffmpeg.avformat_version" decl="true" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavformat\avformat.h" line="1763">
 @defgroup lavf_core Core functions
 @ingroup libavf

 Functions for querying libavformat capabilities, allocating core structures,
 etc.
 @{

Return the LIBAVFORMAT_VERSION_INT constant.

</member>
<member name="M:libffmpeg.avformat_configuration" decl="true" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavformat\avformat.h" line="1777">
Return the libavformat build-time configuration.

</member>
<member name="M:libffmpeg.avformat_license" decl="true" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavformat\avformat.h" line="1782">
Return the libavformat license.

</member>
<member name="M:libffmpeg.av_register_all" decl="true" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavformat\avformat.h" line="1787">
 Initialize libavformat and register all the muxers, demuxers and
 protocols. If you do not call this function, then you can select
 exactly which formats you want to support.

 @see av_register_input_format()
 @see av_register_output_format()

</member>
<member name="M:libffmpeg.avformat_network_init" decl="true" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavformat\avformat.h" line="1800">
 Do global initialization of network components. This is optional,
 but recommended, since it avoids the overhead of implicitly
 doing the setup for each session.

 Calling this function will become mandatory if using network
 protocols at some major version bump.

</member>
<member name="M:libffmpeg.avformat_network_deinit" decl="true" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavformat\avformat.h" line="1810">
Undo the initialization done by avformat_network_init.

</member>
<member name="M:libffmpeg.av_iformat_next(libffmpeg.AVInputFormat!System.Runtime.CompilerServices.IsConst*)" decl="true" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavformat\avformat.h" line="1815">
If f is NULL, returns the first registered input format,
if f is non-NULL, returns the next registered input format after f
or NULL if f is the last one.

</member>
<member name="M:libffmpeg.av_oformat_next(libffmpeg.AVOutputFormat!System.Runtime.CompilerServices.IsConst*)" decl="true" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavformat\avformat.h" line="1822">
If f is NULL, returns the first registered output format,
if f is non-NULL, returns the next registered output format after f
or NULL if f is the last one.

</member>
<member name="M:libffmpeg.avformat_alloc_context" decl="true" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavformat\avformat.h" line="1829">
Allocate an AVFormatContext.
avformat_free_context() can be used to free the context and everything
allocated by the framework within it.

</member>
<member name="M:libffmpeg.avformat_free_context(libffmpeg.AVFormatContext*)" decl="true" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavformat\avformat.h" line="1836">
Free an AVFormatContext and all its streams.
@param s context to free

</member>
<member name="M:libffmpeg.avformat_get_class" decl="true" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavformat\avformat.h" line="1842">
 Get the AVClass for AVFormatContext. It can be used in combination with
 AV_OPT_SEARCH_FAKE_OBJ for examining options.

 @see av_opt_find().

</member>
<member name="M:libffmpeg.avformat_new_stream(libffmpeg.AVFormatContext*,libffmpeg.AVCodec!System.Runtime.CompilerServices.IsConst*)" decl="true" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavformat\avformat.h" line="1850">
 Add a new stream to a media file.

 When demuxing, it is called by the demuxer in read_header(). If the
 flag AVFMTCTX_NOHEADER is set in s.ctx_flags, then it may also
 be called in read_packet().

 When muxing, should be called by the user before avformat_write_header().

 User is required to call avcodec_close() and avformat_free_context() to
 clean up the allocation by avformat_new_stream().

 @param s media file handle
 @param c If non-NULL, the AVCodecContext corresponding to the new stream
 will be initialized to use this codec. This is needed for e.g. codec-specific
 defaults to be set, so codec should be provided if it is known.

 @return newly created stream or NULL on error.

</member>
<member name="M:libffmpeg.av_stream_get_side_data(libffmpeg.AVStream*,libffmpeg.AVPacketSideDataType,System.Int32*)" decl="true" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavformat\avformat.h" line="1871">
 Get side information from stream.

 @param stream stream
 @param type desired side information type
 @param size pointer for side information size to store (optional)
 @return pointer to data if present or NULL otherwise

</member>
<member name="M:libffmpeg.avformat_alloc_output_context2(libffmpeg.AVFormatContext**,libffmpeg.AVOutputFormat*,System.SByte!System.Runtime.CompilerServices.IsSignUnspecifiedByte!System.Runtime.CompilerServices.IsConst*,System.SByte!System.Runtime.CompilerServices.IsSignUnspecifiedByte!System.Runtime.CompilerServices.IsConst*)" decl="true" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavformat\avformat.h" line="1884">
@}

 Allocate an AVFormatContext for an output format.
 avformat_free_context() can be used to free the context and
 everything allocated by the framework within it.

 @param *ctx is set to the created format context, or to NULL in
 case of failure
 @param oformat format to use for allocating the context, if NULL
 format_name and filename are used instead
 @param format_name the name of output format to use for allocating the
 context, if NULL filename is used instead
 @param filename the name of the filename to use for allocating the
 context, may be NULL
 @return &gt;= 0 in case of success, a negative AVERROR code in case of
 failure

</member>
<member name="M:libffmpeg.av_find_input_format(System.SByte!System.Runtime.CompilerServices.IsSignUnspecifiedByte!System.Runtime.CompilerServices.IsConst*)" decl="true" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavformat\avformat.h" line="1908">
@addtogroup lavf_decoding
@{

Find AVInputFormat based on the short name of the input format.

</member>
<member name="M:libffmpeg.av_probe_input_format(libffmpeg.AVProbeData*,System.Int32)" decl="true" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavformat\avformat.h" line="1918">
 Guess the file format.

 @param pd        data to be probed
 @param is_opened Whether the file is already opened; determines whether
                  demuxers with or without AVFMT_NOFILE are probed.

</member>
<!-- Discarding badly formed XML document comment for member 'M:libffmpeg.av_probe_input_format2(libffmpeg.AVProbeData*,System.Int32,System.Int32*)'. -->
<member name="M:libffmpeg.av_probe_input_format3(libffmpeg.AVProbeData*,System.Int32,System.Int32*)" decl="true" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavformat\avformat.h" line="1941">
 Guess the file format.

 @param is_opened Whether the file is already opened; determines whether
                  demuxers with or without AVFMT_NOFILE are probed.
 @param score_ret The score of the best detection.

</member>
<member name="M:libffmpeg.av_probe_input_buffer2(libffmpeg.AVIOContext*,libffmpeg.AVInputFormat**,System.SByte!System.Runtime.CompilerServices.IsSignUnspecifiedByte!System.Runtime.CompilerServices.IsConst*,System.Void*,System.UInt32,System.UInt32)" decl="true" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavformat\avformat.h" line="1950">
 Probe a bytestream to determine the input format. Each time a probe returns
 with a score that is too low, the probe buffer size is increased and another
 attempt is made. When the maximum probe size is reached, the input format
 with the highest score is returned.

 @param pb the bytestream to probe
 @param fmt the input format is put here
 @param filename the filename of the stream
 @param logctx the log context
 @param offset the offset within the bytestream to probe from
 @param max_probe_size the maximum probe buffer size (zero for default)
 @return the score in case of success, a negative value corresponding to an
         the maximal score is AVPROBE_SCORE_MAX
 AVERROR code otherwise

</member>
<member name="M:libffmpeg.av_probe_input_buffer(libffmpeg.AVIOContext*,libffmpeg.AVInputFormat**,System.SByte!System.Runtime.CompilerServices.IsSignUnspecifiedByte!System.Runtime.CompilerServices.IsConst*,System.Void*,System.UInt32,System.UInt32)" decl="true" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavformat\avformat.h" line="1970">
Like av_probe_input_buffer2() but returns 0 on success

</member>
<member name="M:libffmpeg.avformat_open_input(libffmpeg.AVFormatContext**,System.SByte!System.Runtime.CompilerServices.IsSignUnspecifiedByte!System.Runtime.CompilerServices.IsConst*,libffmpeg.AVInputFormat*,libffmpeg.AVDictionary**)" decl="true" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavformat\avformat.h" line="1977">
 Open an input stream and read the header. The codecs are not opened.
 The stream must be closed with avformat_close_input().

 @param ps Pointer to user-supplied AVFormatContext (allocated by avformat_alloc_context).
           May be a pointer to NULL, in which case an AVFormatContext is allocated by this
           function and written into ps.
           Note that a user-supplied AVFormatContext will be freed on failure.
 @param filename Name of the stream to open.
 @param fmt If non-NULL, this parameter forces a specific input format.
            Otherwise the format is autodetected.
 @param options  A dictionary filled with AVFormatContext and demuxer-private options.
                 On return this parameter will be destroyed and replaced with a dict containing
                 options that were not found. May be NULL.

 @return 0 on success, a negative AVERROR on failure.

 @note If you want to use custom IO, preallocate the format context and set its pb field.

</member>
<member name="M:libffmpeg.avformat_find_stream_info(libffmpeg.AVFormatContext*,libffmpeg.AVDictionary**)" decl="true" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavformat\avformat.h" line="2001">
 Read packets of a media file to get stream information. This
 is useful for file formats with no headers such as MPEG. This
 function also computes the real framerate in case of MPEG-2 repeat
 frame mode.
 The logical file position is not changed by this function;
 examined packets may be buffered for later processing.

 @param ic media file handle
 @param options  If non-NULL, an ic.nb_streams long array of pointers to
                 dictionaries, where i-th member contains options for
                 codec corresponding to i-th stream.
                 On return each dictionary will be filled with options that were not found.
 @return &gt;=0 if OK, AVERROR_xxx on error

 @note this function isn't guaranteed to open all the codecs, so
       options being non-empty at return is a perfectly normal behavior.

 @todo Let the user decide somehow what information is needed so that
       we do not waste time getting stuff the user does not need.

</member>
<member name="M:libffmpeg.av_find_program_from_stream(libffmpeg.AVFormatContext*,libffmpeg.AVProgram*,System.Int32)" decl="true" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavformat\avformat.h" line="2024">
 Find the programs which belong to a given stream.

 @param ic    media file handle
 @param last  the last found program, the search will start after this
              program, or from the beginning if it is NULL
 @param s     stream index
 @return the next program which belongs to s, NULL if no program is found or
         the last program is not among the programs of ic.

</member>
<member name="M:libffmpeg.av_find_best_stream(libffmpeg.AVFormatContext*,libffmpeg.AVMediaType,System.Int32,System.Int32,libffmpeg.AVCodec**,System.Int32)" decl="true" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavformat\avformat.h" line="2036">
 Find the "best" stream in the file.
 The best stream is determined according to various heuristics as the most
 likely to be what the user expects.
 If the decoder parameter is non-NULL, av_find_best_stream will find the
 default decoder for the stream's codec; streams for which no decoder can
 be found are ignored.

 @param ic                media file handle
 @param type              stream type: video, audio, subtitles, etc.
 @param wanted_stream_nb  user-requested stream number,
                          or -1 for automatic selection
 @param related_stream    try to find a stream related (eg. in the same
                          program) to this one, or -1 if none
 @param decoder_ret       if non-NULL, returns the decoder for the
                          selected stream
 @param flags             flags; none are currently defined
 @return  the non-negative stream number in case of success,
          AVERROR_STREAM_NOT_FOUND if no stream with the requested type
          could be found,
          AVERROR_DECODER_NOT_FOUND if streams were found but no decoder
 @note  If av_find_best_stream returns successfully and decoder_ret is not
        NULL, then *decoder_ret is guaranteed to be set to a valid AVCodec.

</member>
<!-- Discarding badly formed XML document comment for member 'M:libffmpeg.av_read_frame(libffmpeg.AVFormatContext*,libffmpeg.AVPacket*)'. -->
<member name="M:libffmpeg.av_seek_frame(libffmpeg.AVFormatContext*,System.Int32,System.Int64,System.Int32)" decl="true" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavformat\avformat.h" line="2093">
 Seek to the keyframe at timestamp.
 'timestamp' in 'stream_index'.

 @param s media file handle
 @param stream_index If stream_index is (-1), a default
 stream is selected, and timestamp is automatically converted
 from AV_TIME_BASE units to the stream specific time_base.
 @param timestamp Timestamp in AVStream.time_base units
        or, if no stream is specified, in AV_TIME_BASE units.
 @param flags flags which select direction and seeking mode
 @return &gt;= 0 on success

</member>
<!-- Discarding badly formed XML document comment for member 'M:libffmpeg.avformat_seek_file(libffmpeg.AVFormatContext*,System.Int32,System.Int64,System.Int64,System.Int64,System.Int32)'. -->
<member name="M:libffmpeg.avformat_flush(libffmpeg.AVFormatContext*)" decl="true" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavformat\avformat.h" line="2139">
 Discard all internally buffered data. This can be useful when dealing with
 discontinuities in the byte stream. Generally works only with formats that
 can resync. This includes headerless formats like MPEG-TS/TS but should also
 work with NUT, Ogg and in a limited way AVI for example.

 The set of streams, the detected duration, stream parameters and codecs do
 not change when calling this function. If you want a complete reset, it's
 better to open a new AVFormatContext.

 This does not flush the AVIOContext (s-&gt;pb). If necessary, call
 avio_flush(s-&gt;pb) before calling this function.

 @param s media file handle
 @return &gt;=0 on success, error code otherwise

</member>
<member name="M:libffmpeg.av_read_play(libffmpeg.AVFormatContext*)" decl="true" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavformat\avformat.h" line="2157">
Start playing a network-based stream (e.g. RTSP stream) at the
current position.

</member>
<member name="M:libffmpeg.av_read_pause(libffmpeg.AVFormatContext*)" decl="true" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavformat\avformat.h" line="2163">
 Pause a network-based stream (e.g. RTSP stream).

 Use av_read_play() to resume it.

</member>
<member name="M:libffmpeg.avformat_close_input(libffmpeg.AVFormatContext**)" decl="true" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavformat\avformat.h" line="2170">
Close an opened input AVFormatContext. Free it and all its contents
and set *s to NULL.

</member>
<member name="M:libffmpeg.avformat_write_header(libffmpeg.AVFormatContext*,libffmpeg.AVDictionary**)" decl="true" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavformat\avformat.h" line="2175">
@}

@addtogroup lavf_encoding
@{

 Allocate the stream private data and write the stream header to
 an output media file.

 @param s Media file handle, must be allocated with avformat_alloc_context().
          Its oformat field must be set to the desired output format;
          Its pb field must be set to an already opened AVIOContext.
 @param options  An AVDictionary filled with AVFormatContext and muxer-private options.
                 On return this parameter will be destroyed and replaced with a dict containing
                 options that were not found. May be NULL.

 @return 0 on success, negative AVERROR on failure.

 @see av_opt_find, av_dict_set, avio_open, av_oformat_next.

</member>
<!-- Discarding badly formed XML document comment for member 'M:libffmpeg.av_write_frame(libffmpeg.AVFormatContext*,libffmpeg.AVPacket*)'. -->
<!-- Discarding badly formed XML document comment for member 'M:libffmpeg.av_interleaved_write_frame(libffmpeg.AVFormatContext*,libffmpeg.AVPacket*)'. -->
<member name="M:libffmpeg.av_write_uncoded_frame(libffmpeg.AVFormatContext*,System.Int32,libffmpeg.AVFrame*)" decl="true" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavformat\avformat.h" line="2272">
 Write a uncoded frame to an output media file.

 The frame must be correctly interleaved according to the container
 specification; if not, then av_interleaved_write_frame() must be used.

 See av_interleaved_write_frame() for details.

</member>
<member name="M:libffmpeg.av_interleaved_write_uncoded_frame(libffmpeg.AVFormatContext*,System.Int32,libffmpeg.AVFrame*)" decl="true" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavformat\avformat.h" line="2283">
 Write a uncoded frame to an output media file.

 If the muxer supports it, this function allows to write an AVFrame
 structure directly, without encoding it into a packet.
 It is mostly useful for devices and similar special muxers that use raw
 video or PCM data and will not serialize it into a byte stream.

 To test whether it is possible to use it with a given muxer and stream,
 use av_write_uncoded_frame_query().

 The caller gives up ownership of the frame and must not access it
 afterwards.

 @return  &gt;=0 for success, a negative code on error

</member>
<!-- Discarding badly formed XML document comment for member 'M:libffmpeg.av_write_uncoded_frame_query(libffmpeg.AVFormatContext*,System.Int32)'. -->
<member name="M:libffmpeg.av_write_trailer(libffmpeg.AVFormatContext*)" decl="true" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavformat\avformat.h" line="2310">
 Write the stream trailer to an output media file and free the
 file private data.

 May only be called after a successful call to avformat_write_header.

 @param s media file handle
 @return 0 if OK, AVERROR_xxx on error

</member>
<member name="M:libffmpeg.av_guess_format(System.SByte!System.Runtime.CompilerServices.IsSignUnspecifiedByte!System.Runtime.CompilerServices.IsConst*,System.SByte!System.Runtime.CompilerServices.IsSignUnspecifiedByte!System.Runtime.CompilerServices.IsConst*,System.SByte!System.Runtime.CompilerServices.IsSignUnspecifiedByte!System.Runtime.CompilerServices.IsConst*)" decl="true" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavformat\avformat.h" line="2321">
 Return the output format in the list of registered output formats
 which best matches the provided parameters, or return NULL if
 there is no match.

 @param short_name if non-NULL checks if short_name matches with the
 names of the registered formats
 @param filename if non-NULL checks if filename terminates with the
 extensions of the registered formats
 @param mime_type if non-NULL checks if mime_type matches with the
 MIME type of the registered formats

</member>
<member name="T:libffmpeg.AVCodecID" decl="true" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavformat\avformat.h" line="2337">
Guess the codec ID based upon muxer and filename.

</member>
<member name="M:libffmpeg.av_get_output_timestamp(libffmpeg.AVFormatContext*,System.Int32,System.Int64*,System.Int64*)" decl="true" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavformat\avformat.h" line="2344">
Get timing information for the data currently output.
The exact meaning of "currently output" depends on the format.
It is mostly relevant for devices that have an internal buffer and/or
work in real time.
@param s          media file handle
@param stream     stream in the media file
@param[out] dts   DTS of the last packet output for the stream, in stream
                  time_base units
@param[out] wall  absolute time when that packet whas output,
                  in microsecond
@return  0 if OK, AVERROR(ENOSYS) if the format does not support it
Note: some formats or devices may not allow to measure dts and wall
atomically.

</member>
<member name="M:libffmpeg.av_hex_dump(_iobuf*,System.Byte!System.Runtime.CompilerServices.IsConst*,System.Int32)" decl="true" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavformat\avformat.h" line="2363">
@}

 @defgroup lavf_misc Utility functions
 @ingroup libavf
 @{

 Miscellaneous utility functions related to both muxing and demuxing
 (or neither).

 Send a nice hexadecimal dump of a buffer to the specified file stream.

 @param f The file stream pointer where the dump should be sent to.
 @param buf buffer
 @param size buffer size

 @see av_hex_dump_log, av_pkt_dump2, av_pkt_dump_log2

</member>
<member name="M:libffmpeg.av_hex_dump_log(System.Void*,System.Int32,System.Byte!System.Runtime.CompilerServices.IsConst*,System.Int32)" decl="true" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavformat\avformat.h" line="2388">
 Send a nice hexadecimal dump of a buffer to the log.

 @param avcl A pointer to an arbitrary struct of which the first field is a
 pointer to an AVClass struct.
 @param level The importance level of the message, lower values signifying
 higher importance.
 @param buf buffer
 @param size buffer size

 @see av_hex_dump, av_pkt_dump2, av_pkt_dump_log2

</member>
<member name="M:libffmpeg.av_pkt_dump2(_iobuf*,libffmpeg.AVPacket!System.Runtime.CompilerServices.IsConst*,System.Int32,libffmpeg.AVStream!System.Runtime.CompilerServices.IsConst*)" decl="true" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavformat\avformat.h" line="2402">
 Send a nice dump of a packet to the specified file stream.

 @param f The file stream pointer where the dump should be sent to.
 @param pkt packet to dump
 @param dump_payload True if the payload must be displayed, too.
 @param st AVStream that the packet belongs to

</member>
<member name="M:libffmpeg.av_pkt_dump_log2(System.Void*,System.Int32,libffmpeg.AVPacket!System.Runtime.CompilerServices.IsConst*,System.Int32,libffmpeg.AVStream!System.Runtime.CompilerServices.IsConst*)" decl="true" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavformat\avformat.h" line="2413">
 Send a nice dump of a packet to the log.

 @param avcl A pointer to an arbitrary struct of which the first field is a
 pointer to an AVClass struct.
 @param level The importance level of the message, lower values signifying
 higher importance.
 @param pkt packet to dump
 @param dump_payload True if the payload must be displayed, too.
 @param st AVStream that the packet belongs to

</member>
<member name="T:libffmpeg.AVCodecID" decl="true" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavformat\avformat.h" line="2427">
 Get the AVCodecID for the given codec tag tag.
 If no codec id is found returns AV_CODEC_ID_NONE.

 @param tags list of supported codec_id-codec_tag pairs, as stored
 in AVInputFormat.codec_tag and AVOutputFormat.codec_tag
 @param tag  codec tag to match to a codec ID

</member>
<member name="M:libffmpeg.av_codec_get_tag(libffmpeg.AVCodecTag!System.Runtime.CompilerServices.IsConst*!System.Runtime.CompilerServices.IsConst*,libffmpeg.AVCodecID)" decl="true" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavformat\avformat.h" line="2437">
 Get the codec tag for the given codec id id.
 If no codec tag is found returns 0.

 @param tags list of supported codec_id-codec_tag pairs, as stored
 in AVInputFormat.codec_tag and AVOutputFormat.codec_tag
 @param id   codec ID to match to a codec tag

</member>
<member name="M:libffmpeg.av_codec_get_tag2(libffmpeg.AVCodecTag!System.Runtime.CompilerServices.IsConst*!System.Runtime.CompilerServices.IsConst*,libffmpeg.AVCodecID,System.UInt32*)" decl="true" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavformat\avformat.h" line="2447">
 Get the codec tag for the given codec id.

 @param tags list of supported codec_id - codec_tag pairs, as stored
 in AVInputFormat.codec_tag and AVOutputFormat.codec_tag
 @param id codec id that should be searched for in the list
 @param tag A pointer to the found tag
 @return 0 if id was not found in tags, &gt; 0 if it was found

</member>
<!-- Discarding badly formed XML document comment for member 'M:libffmpeg.av_index_search_timestamp(libffmpeg.AVStream*,System.Int64,System.Int32)'. -->
<member name="M:libffmpeg.av_add_index_entry(libffmpeg.AVStream*,System.Int64,System.Int64,System.Int32,System.Int32,System.Int32)" decl="true" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavformat\avformat.h" line="2474">
 Add an index entry into a sorted list. Update the entry if the list
 already contains it.

 @param timestamp timestamp in the time base of the given stream

</member>
<member name="M:libffmpeg.av_url_split(System.SByte!System.Runtime.CompilerServices.IsSignUnspecifiedByte*,System.Int32,System.SByte!System.Runtime.CompilerServices.IsSignUnspecifiedByte*,System.Int32,System.SByte!System.Runtime.CompilerServices.IsSignUnspecifiedByte*,System.Int32,System.Int32*,System.SByte!System.Runtime.CompilerServices.IsSignUnspecifiedByte*,System.Int32,System.SByte!System.Runtime.CompilerServices.IsSignUnspecifiedByte!System.Runtime.CompilerServices.IsConst*)" decl="true" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavformat\avformat.h" line="2484">
 Split a URL string into components.

 The pointers to buffers for storing individual components may be null,
 in order to ignore that component. Buffers for components not found are
 set to empty strings. If the port is not found, it is set to a negative
 value.

 @param proto the buffer for the protocol
 @param proto_size the size of the proto buffer
 @param authorization the buffer for the authorization
 @param authorization_size the size of the authorization buffer
 @param hostname the buffer for the host name
 @param hostname_size the size of the hostname buffer
 @param port_ptr a pointer to store the port number in
 @param path the buffer for the path
 @param path_size the size of the path buffer
 @param url the URL to split

</member>
<member name="M:libffmpeg.av_dump_format(libffmpeg.AVFormatContext*,System.Int32,System.SByte!System.Runtime.CompilerServices.IsSignUnspecifiedByte!System.Runtime.CompilerServices.IsConst*,System.Int32)" decl="true" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavformat\avformat.h" line="2511">
 Print detailed information about the input or output format, such as
 duration, bitrate, streams, container, programs, metadata, side data,
 codec and time base.

 @param ic        the context to analyze
 @param index     index of the stream to dump information about
 @param url       the URL to print, such as source or destination file
 @param is_output Select whether the specified context is an input(0) or output(1)

</member>
<member name="M:libffmpeg.av_get_frame_filename(System.SByte!System.Runtime.CompilerServices.IsSignUnspecifiedByte*,System.Int32,System.SByte!System.Runtime.CompilerServices.IsSignUnspecifiedByte!System.Runtime.CompilerServices.IsConst*,System.Int32)" decl="true" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavformat\avformat.h" line="2526">
 Return in 'buf' the path with '%d' replaced by a number.

 Also handles the '%0nd' format where 'n' is the total number
 of digits and '%%'.

 @param buf destination buffer
 @param buf_size destination buffer size
 @param path numbered sequence string
 @param number frame number
 @return 0 if OK, -1 on format error

</member>
<member name="M:libffmpeg.av_filename_number_test(System.SByte!System.Runtime.CompilerServices.IsSignUnspecifiedByte!System.Runtime.CompilerServices.IsConst*)" decl="true" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavformat\avformat.h" line="2541">
 Check whether filename actually is a numbered sequence generator.

 @param filename possible numbered sequence string
 @return 1 if a valid numbered sequence string, 0 otherwise

</member>
<member name="M:libffmpeg.av_sdp_create(libffmpeg.AVFormatContext**,System.Int32,System.SByte!System.Runtime.CompilerServices.IsSignUnspecifiedByte*,System.Int32)" decl="true" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavformat\avformat.h" line="2549">
 Generate an SDP for an RTP session.

 Note, this overwrites the id values of AVStreams in the muxer contexts
 for getting unique dynamic payload types.

 @param ac array of AVFormatContexts describing the RTP streams. If the
           array is composed by only one context, such context can contain
           multiple AVStreams (one AVStream per RTP stream). Otherwise,
           all the contexts in the array (an AVCodecContext per RTP stream)
           must contain only one AVStream.
 @param n_files number of AVCodecContexts contained in ac
 @param buf buffer where the SDP will be stored (must be allocated by
            the caller)
 @param size the size of the buffer
 @return 0 if OK, AVERROR_xxx on error

</member>
<member name="M:libffmpeg.av_match_ext(System.SByte!System.Runtime.CompilerServices.IsSignUnspecifiedByte!System.Runtime.CompilerServices.IsConst*,System.SByte!System.Runtime.CompilerServices.IsSignUnspecifiedByte!System.Runtime.CompilerServices.IsConst*)" decl="true" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavformat\avformat.h" line="2568">
 Return a positive value if the given filename has one of the given
 extensions, 0 otherwise.

 @param filename   file name to check against the given extensions
 @param extensions a comma-separated list of filename extensions

</member>
<member name="M:libffmpeg.avformat_query_codec(libffmpeg.AVOutputFormat!System.Runtime.CompilerServices.IsConst*,libffmpeg.AVCodecID,System.Int32)" decl="true" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavformat\avformat.h" line="2577">
 Test if the given container can store a codec.

 @param ofmt           container to check for compatibility
 @param codec_id       codec to potentially store in container
 @param std_compliance standards compliance level, one of FF_COMPLIANCE_*

 @return 1 if codec with ID codec_id can be stored in ofmt, 0 if it cannot.
         A negative number if this information is not available.

</member>
<member name="T:libffmpeg.AVCodecTag" decl="true" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavformat\avformat.h" line="2590">
@defgroup riff_fourcc RIFF FourCCs
@{
Get the tables mapping RIFF FourCCs to libavcodec AVCodecIDs. The tables are
meant to be passed to av_codec_get_id()/av_codec_get_tag() as in the
following code:
@code
uint32_t tag = MKTAG('H', '2', '6', '4');
const struct AVCodecTag *table[] = { avformat_get_riff_video_tags(), 0 };
enum AVCodecID id = av_codec_get_id(table, tag);
@endcode

@return the table mapping RIFF FourCCs for video to libavcodec AVCodecID.

</member>
<member name="T:libffmpeg.AVCodecTag" decl="true" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavformat\avformat.h" line="2606">
@return the table mapping RIFF FourCCs for audio to AVCodecID.

</member>
<member name="T:libffmpeg.AVCodecTag" decl="true" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavformat\avformat.h" line="2610">
@return the table mapping MOV FourCCs for video to libavcodec AVCodecID.

</member>
<member name="T:libffmpeg.AVCodecTag" decl="true" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavformat\avformat.h" line="2614">
@return the table mapping MOV FourCCs for audio to AVCodecID.

</member>
<member name="M:libffmpeg.av_guess_sample_aspect_ratio(libffmpeg.AVFormatContext*,libffmpeg.AVStream*,libffmpeg.AVFrame*)" decl="true" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavformat\avformat.h" line="2619">
@}

 Guess the sample aspect ratio of a frame, based on both the stream and the
 frame aspect ratio.

 Since the frame aspect ratio is set by the codec but the stream aspect ratio
 is set by the demuxer, these two may not be equal. This function tries to
 return the value that you should use if you would like to display the frame.

 Basic logic is to use the stream aspect ratio if it is set to something sane
 otherwise use the frame aspect ratio. This way a container setting, which is
 usually easy to modify can override the coded value in the frames.

 @param format the format context which the stream is part of
 @param stream the stream which the frame is part of
 @param frame the frame with the aspect ratio to be determined
 @return the guessed (valid) sample_aspect_ratio, 0/1 if no idea

</member>
<member name="M:libffmpeg.av_guess_frame_rate(libffmpeg.AVFormatContext*,libffmpeg.AVStream*,libffmpeg.AVFrame*)" decl="true" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavformat\avformat.h" line="2642">
 Guess the frame rate, based on both the container and codec information.

 @param ctx the format context which the stream is part of
 @param stream the stream which the frame is part of
 @param frame the frame for which the frame rate should be determined, may be NULL
 @return the guessed (valid) frame rate, 0/1 if no idea

</member>
<member name="M:libffmpeg.avformat_match_stream_specifier(libffmpeg.AVFormatContext*,libffmpeg.AVStream*,System.SByte!System.Runtime.CompilerServices.IsSignUnspecifiedByte!System.Runtime.CompilerServices.IsConst*)" decl="true" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavformat\avformat.h" line="2652">
 Check if the stream st contained in s is matched by the stream specifier
 spec.

 See the "stream specifiers" chapter in the documentation for the syntax
 of spec.

 @return  &gt;0 if st is matched by spec;
          0  if st is not matched by spec;
          AVERROR code if spec is invalid

 @note  A stream specifier can match several streams in the format.

</member>
<member name="M:libffmpeg.swscale_version" decl="true" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavformat\avformat.h" line="2671">
@}

@file
@ingroup libsws
external API header

@file
swscale version macros

FF_API_* defines may be placed below to indicate public API that will be
dropped at a future version bump. The defines themselves are not part of
the public API and may change, break or disappear at any time.

 @defgroup libsws Color conversion and scaling
 @{

 Return the LIBSWSCALE_VERSION_INT constant.

</member>
<member name="M:libffmpeg.swscale_configuration" decl="true" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libswscale\swscale.h" line="45">
Return the libswscale build-time configuration.

</member>
<member name="M:libffmpeg.swscale_license" decl="true" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libswscale\swscale.h" line="50">
Return the libswscale license.

</member>
<!-- Discarding badly formed XML document comment for member 'M:libffmpeg.sws_getCoefficients(System.Int32)'. -->
<member name="M:libffmpeg.sws_isSupportedInput(libffmpeg.AVPixelFormat)" decl="true" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libswscale\swscale.h" line="137">
Return a positive value if pix_fmt is a supported input format, 0
otherwise.

</member>
<member name="M:libffmpeg.sws_isSupportedOutput(libffmpeg.AVPixelFormat)" decl="true" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libswscale\swscale.h" line="143">
Return a positive value if pix_fmt is a supported output format, 0
otherwise.

</member>
<member name="M:libffmpeg.sws_isSupportedEndiannessConversion(libffmpeg.AVPixelFormat)" decl="true" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libswscale\swscale.h" line="149">
@param[in]  pix_fmt the pixel format
@return a positive value if an endianness conversion for pix_fmt is
supported, 0 otherwise.

</member>
<member name="T:libffmpeg.SwsContext" decl="true" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libswscale\swscale.h" line="156">
Allocate an empty SwsContext. This must be filled and passed to
sws_init_context(). For filling see AVOptions, options.c and
sws_setColorspaceDetails().

</member>
<member name="M:libffmpeg.sws_init_context(libffmpeg.SwsContext*,libffmpeg.SwsFilter*,libffmpeg.SwsFilter*)" decl="true" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libswscale\swscale.h" line="163">
 Initialize the swscaler context sws_context.

 @return zero or positive value on success, a negative value on
 error

</member>
<member name="M:libffmpeg.sws_freeContext(libffmpeg.SwsContext*)" decl="true" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libswscale\swscale.h" line="171">
Free the swscaler context swsContext.
If swsContext is NULL, then does nothing.

</member>
<member name="T:libffmpeg.SwsContext" decl="true" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libswscale\swscale.h" line="177">
 Allocate and return an SwsContext. You need it to perform
 scaling/conversion operations using sws_scale().

 @param srcW the width of the source image
 @param srcH the height of the source image
 @param srcFormat the source image format
 @param dstW the width of the destination image
 @param dstH the height of the destination image
 @param dstFormat the destination image format
 @param flags specify which algorithm and options to use for rescaling
 @return a pointer to an allocated context, or NULL in case of error
 @note this function is to be removed after a saner alternative is
       written

</member>
<member name="M:libffmpeg.sws_scale(libffmpeg.SwsContext*,System.Byte!System.Runtime.CompilerServices.IsConst*!System.Runtime.CompilerServices.IsConst*,System.Int32!System.Runtime.CompilerServices.IsConst*,System.Int32,System.Int32,System.Byte*!System.Runtime.CompilerServices.IsConst*,System.Int32!System.Runtime.CompilerServices.IsConst*)" decl="true" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libswscale\swscale.h" line="197">
 Scale the image slice in srcSlice and put the resulting scaled
 slice in the image in dst. A slice is a sequence of consecutive
 rows in an image.

 Slices have to be provided in sequential order, either in
 top-bottom or bottom-top order. If slices are provided in
 non-sequential order the behavior of the function is undefined.

 @param c         the scaling context previously created with
                  sws_getContext()
 @param srcSlice  the array containing the pointers to the planes of
                  the source slice
 @param srcStride the array containing the strides for each plane of
                  the source image
 @param srcSliceY the position in the source image of the slice to
                  process, that is the number (counted starting from
                  zero) in the image of the first row of the slice
 @param srcSliceH the height of the source slice, that is the number
                  of rows in the slice
 @param dst       the array containing the pointers to the planes of
                  the destination image
 @param dstStride the array containing the strides for each plane of
                  the destination image
 @return          the height of the output slice

</member>
<member name="M:libffmpeg.sws_setColorspaceDetails(libffmpeg.SwsContext*,System.Int32!System.Runtime.CompilerServices.IsConst*,System.Int32,System.Int32!System.Runtime.CompilerServices.IsConst*,System.Int32,System.Int32,System.Int32,System.Int32)" decl="true" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libswscale\swscale.h" line="227">
@param dstRange flag indicating the while-black range of the output (1=jpeg / 0=mpeg)
@param srcRange flag indicating the while-black range of the input (1=jpeg / 0=mpeg)
@param table the yuv2rgb coefficients describing the output yuv space, normally ff_yuv2rgb_coeffs[x]
@param inv_table the yuv2rgb coefficients describing the input yuv space, normally ff_yuv2rgb_coeffs[x]
@param brightness 16.16 fixed point brightness correction
@param contrast 16.16 fixed point contrast correction
@param saturation 16.16 fixed point saturation correction
@return -1 if not supported

</member>
<member name="M:libffmpeg.sws_getColorspaceDetails(libffmpeg.SwsContext*,System.Int32**,System.Int32*,System.Int32**,System.Int32*,System.Int32*,System.Int32*,System.Int32*)" decl="true" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libswscale\swscale.h" line="241">
@return -1 if not supported

</member>
<member name="M:libffmpeg.sws_allocVec(System.Int32)" decl="true" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libswscale\swscale.h" line="248">
Allocate and return an uninitialized vector with length coefficients.

</member>
<member name="M:libffmpeg.sws_getGaussianVec(System.Double,System.Double)" decl="true" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libswscale\swscale.h" line="253">
Return a normalized Gaussian curve used to filter stuff
quality = 3 is high quality, lower is lower quality.

</member>
<member name="M:libffmpeg.sws_getConstVec(System.Double,System.Int32)" decl="true" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libswscale\swscale.h" line="259">
Allocate and return a vector with length coefficients, all
with the same value c.

</member>
<member name="M:libffmpeg.sws_getIdentityVec" decl="true" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libswscale\swscale.h" line="265">
Allocate and return a vector with just one coefficient, with
value 1.0.

</member>
<member name="M:libffmpeg.sws_scaleVec(libffmpeg.SwsVector*,System.Double)" decl="true" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libswscale\swscale.h" line="271">
Scale all the coefficients of a by the scalar value.

</member>
<member name="M:libffmpeg.sws_normalizeVec(libffmpeg.SwsVector*,System.Double)" decl="true" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libswscale\swscale.h" line="276">
Scale all the coefficients of a so that their sum equals height.

</member>
<member name="M:libffmpeg.sws_cloneVec(libffmpeg.SwsVector*)" decl="true" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libswscale\swscale.h" line="285">
Allocate and return a clone of the vector a, that is a vector
with the same coefficients as a.

</member>
<!-- Discarding badly formed XML document comment for member 'M:libffmpeg.sws_printVec2(libffmpeg.SwsVector*,libffmpeg.AVClass*,System.Int32)'. -->
<member name="T:libffmpeg.SwsContext" decl="true" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libswscale\swscale.h" line="305">
 Check if context can be reused, otherwise reallocate a new one.

 If context is NULL, just calls sws_getContext() to get a new
 context. Otherwise, checks if the parameters are the ones already
 saved in context. If that is the case, returns the current
 context. Otherwise, frees context and gets a new context with
 the new parameters.

 Be warned that srcFilter and dstFilter are not checked, they
 are assumed to remain the same.

</member>
<member name="M:libffmpeg.sws_convertPalette8ToPacked32(System.Byte!System.Runtime.CompilerServices.IsConst*,System.Byte*,System.Int32,System.Byte!System.Runtime.CompilerServices.IsConst*)" decl="true" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libswscale\swscale.h" line="323">
 Convert an 8-bit paletted frame into a frame with a color depth of 32 bits.

 The output frame will have the same packed format as the palette.

 @param src        source frame buffer
 @param dst        destination frame buffer
 @param num_pixels number of pixels to convert
 @param palette    array with [256] entries, which must match color arrangement (RGB or BGR) of src

</member>
<member name="M:libffmpeg.sws_convertPalette8ToPacked24(System.Byte!System.Runtime.CompilerServices.IsConst*,System.Byte*,System.Int32,System.Byte!System.Runtime.CompilerServices.IsConst*)" decl="true" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libswscale\swscale.h" line="335">
 Convert an 8-bit paletted frame into a frame with a color depth of 24 bits.

 With the palette format "ABCD", the destination frame ends up with the format "ABC".

 @param src        source frame buffer
 @param dst        destination frame buffer
 @param num_pixels number of pixels to convert
 @param palette    array with [256] entries, which must match color arrangement (RGB or BGR) of src

</member>
<member name="M:libffmpeg.sws_get_class" decl="true" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libswscale\swscale.h" line="347">
 Get the AVClass for swsContext. It can be used in combination with
 AV_OPT_SEARCH_FAKE_OBJ for examining options.

 @see av_opt_find().

</member>
<!-- Discarding badly formed XML document comment for member 'T:libffmpeg.SwrDitherType'. -->
<member name="T:libffmpeg.SwrEngine" decl="false" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libswresample\swresample.h" line="162">
Resampling Engines 
</member>
<member name="T:libffmpeg.SwrFilterType" decl="false" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libswresample\swresample.h" line="169">
Resampling Filter Types 
</member>
<member name="T:libffmpeg.SwrContext" decl="true" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libswresample\swresample.h" line="176">
@}

The libswresample context. Unlike libavcodec and libavformat, this structure
is opaque. This means that if you would like to set options, you must use
the @ref avoptions API and cannot directly set values to members of the
structure.

</member>
<member name="M:libffmpeg.swr_get_class" decl="true" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libswresample\swresample.h" line="188">
 Get the AVClass for SwrContext. It can be used in combination with
 AV_OPT_SEARCH_FAKE_OBJ for examining options.

 @see av_opt_find().
 @return the AVClass of SwrContext

</member>
<member name="T:libffmpeg.SwrContext" decl="true" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libswresample\swresample.h" line="197">
@name SwrContext constructor functions
@{

 Allocate SwrContext.

 If you use this function you will need to set the parameters (manually or
 with swr_alloc_set_opts()) before calling swr_init().

 @see swr_alloc_set_opts(), swr_init(), swr_free()
 @return NULL on error, allocated context otherwise

</member>
<member name="M:libffmpeg.swr_init(libffmpeg.SwrContext*)" decl="true" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libswresample\swresample.h" line="213">
 Initialize context after user parameters have been set.
 @note The context must be configured using the AVOption API.

 @see av_opt_set_int()
 @see av_opt_set_dict()

 @param[in,out]   s Swr context to initialize
 @return AVERROR error code in case of failure.

</member>
<member name="M:libffmpeg.swr_is_initialized(libffmpeg.SwrContext*)" decl="true" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libswresample\swresample.h" line="225">
 Check whether an swr context has been initialized or not.

 @param[in]       s Swr context to check
 @see swr_init()
 @return positive if it has been initialized, 0 if not initialized

</member>
<member name="T:libffmpeg.SwrContext" decl="true" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libswresample\swresample.h" line="234">
 Allocate SwrContext if needed and set/reset common parameters.

 This function does not require s to be allocated with swr_alloc(). On the
 other hand, swr_alloc() can use swr_alloc_set_opts() to set the parameters
 on the allocated context.

 @param s               existing Swr context if available, or NULL if not
 @param out_ch_layout   output channel layout (AV_CH_LAYOUT_*)
 @param out_sample_fmt  output sample format (AV_SAMPLE_FMT_*).
 @param out_sample_rate output sample rate (frequency in Hz)
 @param in_ch_layout    input channel layout (AV_CH_LAYOUT_*)
 @param in_sample_fmt   input sample format (AV_SAMPLE_FMT_*).
 @param in_sample_rate  input sample rate (frequency in Hz)
 @param log_offset      logging level offset
 @param log_ctx         parent logging context, can be NULL

 @see swr_init(), swr_free()
 @return NULL on error, allocated context otherwise

</member>
<member name="M:libffmpeg.swr_free(libffmpeg.SwrContext**)" decl="true" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libswresample\swresample.h" line="259">
 @}

 @name SwrContext destructor functions
 @{

 Free the given SwrContext and set the pointer to NULL.

 @param[in] s a pointer to a pointer to Swr context

</member>
<member name="M:libffmpeg.swr_close(libffmpeg.SwrContext*)" decl="true" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libswresample\swresample.h" line="273">
 Closes the context so that swr_is_initialized() returns 0.

 The context can be brought back to life by running swr_init(),
 swr_init() can also be used without swr_close().
 This function is mainly provided for simplifying the usecase
 where one tries to support libavresample and libswresample.

 @param[in,out] s Swr context to be closed

</member>
<member name="M:libffmpeg.swr_convert(libffmpeg.SwrContext*,System.Byte**,System.Int32,System.Byte!System.Runtime.CompilerServices.IsConst**,System.Int32)" decl="true" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libswresample\swresample.h" line="285">
 @}

 @name Core conversion functions
 @{

Convert audio.
 *
 * in and in_count can be set to 0 to flush the last few samples out at the
 * end.
 *
 * If more input is provided than output space then the input will be buffered.
 * You can avoid this buffering by providing more output space than input.
 * Conversion will run directly without copying whenever possible.
 *
 * @param s         allocated Swr context, with parameters set
 * @param out       output buffers, only the first one need be set in case of packed audio
 * @param out_count amount of space available for output in samples per channel
 * @param in        input buffers, only the first one need to be set in case of packed audio
 * @param in_count  number of input samples available in one channel
 *
 * @return number of samples output per channel, negative value on error

</member>
<!-- Discarding badly formed XML document comment for member 'M:libffmpeg.swr_next_pts(libffmpeg.SwrContext*,System.Int64)'. -->
<member name="M:libffmpeg.swr_set_compensation(libffmpeg.SwrContext*,System.Int32,System.Int32)" decl="true" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libswresample\swresample.h" line="331">
 @}

 @name Low-level option setting functions
 These functons provide a means to set low-level options that is not possible
 with the AVOption API.
 @{

 Activate resampling compensation ("soft" compensation). This function is
 internally called when needed in swr_next_pts().

 @param[in,out] s             allocated Swr context. If it is not initialized,
                              or SWR_FLAG_RESAMPLE is not set, swr_init() is
                              called with the flag set.
 @param[in]     sample_delta  delta in PTS per sample
 @param[in]     compensation_distance number of samples to compensate for
 @return    &gt;= 0 on success, AVERROR error codes if:
            @li @c s is NULL,
            @li @c compensation_distance is less than 0,
            @li @c compensation_distance is 0 but sample_delta is not,
            @li compensation unsupported by resampler, or
            @li swr_init() fails when called.

</member>
<member name="M:libffmpeg.swr_set_channel_mapping(libffmpeg.SwrContext*,System.Int32!System.Runtime.CompilerServices.IsConst*)" decl="true" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libswresample\swresample.h" line="358">
 Set a customized input channel mapping.

 @param[in,out] s           allocated Swr context, not yet initialized
 @param[in]     channel_map customized input channel mapping (array of channel
                            indexes, -1 for a muted channel)
 @return &gt;= 0 on success, or AVERROR error code in case of failure.

</member>
<member name="M:libffmpeg.swr_set_matrix(libffmpeg.SwrContext*,System.Double!System.Runtime.CompilerServices.IsConst*,System.Int32)" decl="true" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libswresample\swresample.h" line="368">
 Set a customized remix matrix.

 @param s       allocated Swr context, not yet initialized
 @param matrix  remix coefficients; matrix[i + stride * o] is
                the weight of input channel i in output channel o
 @param stride  offset between lines of the matrix
 @return  &gt;= 0 on success, or AVERROR error code in case of failure.

</member>
<member name="M:libffmpeg.swr_drop_output(libffmpeg.SwrContext*,System.Int32)" decl="true" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libswresample\swresample.h" line="379">
 @}

 @name Sample handling functions
 @{

 Drops the specified number of output samples.

 This function, along with swr_inject_silence(), is called by swr_next_pts()
 if needed for "hard" compensation.

 @param s     allocated Swr context
 @param count number of samples to be dropped

 @return &gt;= 0 on success, or a negative AVERROR code on failure

</member>
<member name="M:libffmpeg.swr_inject_silence(libffmpeg.SwrContext*,System.Int32)" decl="true" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libswresample\swresample.h" line="399">
 Injects the specified number of silence samples.

 This function, along with swr_drop_output(), is called by swr_next_pts()
 if needed for "hard" compensation.

 @param s     allocated Swr context
 @param count number of samples to be dropped

 @return &gt;= 0 on success, or a negative AVERROR code on failure

</member>
<member name="M:libffmpeg.swr_get_delay(libffmpeg.SwrContext*,System.Int64)" decl="true" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libswresample\swresample.h" line="412">
 Gets the delay the next input sample will experience relative to the next output sample.

 Swresample can buffer data if more input has been provided than available
 output space, also converting between sample rates needs a delay.
 This function returns the sum of all such delays.
 The exact delay is not necessarily an integer value in either input or
 output sample rate. Especially when downsampling by a large value, the
 output sample rate may be a poor choice to represent the delay, similarly
 for upsampling and the input sample rate.

 @param s     swr context
 @param base  timebase in which the returned delay will be:
              @li if it's set to 1 the returned delay is in seconds
              @li if it's set to 1000 the returned delay is in milliseconds
              @li if it's set to the input sample rate then the returned
                  delay is in input samples
              @li if it's set to the output sample rate then the returned
                  delay is in output samples
              @li if it's the least common multiple of in_sample_rate and
                  out_sample_rate then an exact rounding-free delay will be
                  returned
 @returns     the delay in 1 / @c base units.

</member>
<member name="M:libffmpeg.swresample_version" decl="true" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libswresample\swresample.h" line="438">
 @}

 @name Configuration accessors
 @{

 Return the @ref LIBSWRESAMPLE_VERSION_INT constant.

 This is useful to check if the build-time libswresample has the same version
 as the run-time one.

 @returns     the unsigned int-typed version

</member>
<member name="M:libffmpeg.swresample_configuration" decl="true" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libswresample\swresample.h" line="455">
 Return the swr build-time configuration.

 @returns     the build-time @c ./configure flags

</member>
<member name="M:libffmpeg.swresample_license" decl="true" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libswresample\swresample.h" line="462">
 Return the swr license.

 @returns     the license of libswresample, determined at build-time

</member>
<member name="M:libffmpeg.swr_convert_frame(libffmpeg.SwrContext*,libffmpeg.AVFrame*,libffmpeg.AVFrame!System.Runtime.CompilerServices.IsConst*)" decl="true" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libswresample\swresample.h" line="469">
 @}

 @name AVFrame based API
 @{

 Convert the samples in the input AVFrame and write them to the output AVFrame.

 Input and output AVFrames must have channel_layout, sample_rate and format set.

 If the output AVFrame does not have the data pointers allocated the nb_samples
 field will be set using av_frame_get_buffer()
 is called to allocate the frame.

 The output AVFrame can be NULL or have fewer allocated samples than required.
 In this case, any remaining samples not written to the output will be added
 to an internal FIFO buffer, to be returned at the next call to this function
 or to swr_convert().

 If converting sample rate, there may be data remaining in the internal
 resampling delay buffer. swr_get_delay() tells the number of
 remaining samples. To get this data as output, call this function or
 swr_convert() with NULL input.

 If the SwrContext configuration does not match the output and
 input AVFrame settings the conversion does not take place and depending on
 which AVFrame is not matching AVERROR_OUTPUT_CHANGED, AVERROR_INPUT_CHANGED
 or the result of a bitwise-OR of them is returned.

 @see swr_delay()
 @see swr_convert()
 @see swr_get_delay()

 @param swr             audio resample context
 @param output          output AVFrame
 @param input           input AVFrame
 @return                0 on success, AVERROR on failure or nonmatching
                        configuration.

</member>
<member name="M:libffmpeg.swr_config_frame(libffmpeg.SwrContext*,libffmpeg.AVFrame!System.Runtime.CompilerServices.IsConst*,libffmpeg.AVFrame!System.Runtime.CompilerServices.IsConst*)" decl="true" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libswresample\swresample.h" line="513">
 Configure or reconfigure the SwrContext using the information
 provided by the AVFrames.

 The original resampling context is reset even on failure.
 The function calls swr_close() internally if the context is open.

 @see swr_close();

 @param swr             audio resample context
 @param output          output AVFrame
 @param input           input AVFrame
 @return                0 on success, AVERROR on failure.

</member>
<!-- Discarding badly formed XML document comment for member 'T:libffmpeg.AVOptionType'. -->
<member name="T:libffmpeg.AVOption" decl="false" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavutil\opt.h" line="252">
AVOption

</member>
<member name="F:libffmpeg.AVOption.help" decl="false" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavutil\opt.h" line="258">
short English help text
@todo What about other languages?

</member>
<member name="F:libffmpeg.AVOption.offset" decl="false" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavutil\opt.h" line="264">
The offset relative to the context structure where the option
value is stored. It should be 0 for named constants.

</member>
<member name="F:libffmpeg.AVOption.unit" decl="false" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavutil\opt.h" line="293">
The option is inteded for exporting values to the caller.

The option may not be set through the AVOptions API, only read.
This flag only makes sense when AV_OPT_FLAG_EXPORT is also set.

The logical unit to which the option belongs. Non-constant
options and corresponding named constants share the same
unit. May be NULL.

</member>
<member name="T:libffmpeg.AVOptionRange" decl="false" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavutil\opt.h" line="313">
A single allowed range of values, or a single allowed value.

</member>
<member name="F:libffmpeg.AVOptionRange.value_min" decl="false" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavutil\opt.h" line="318">
Value range.
For string ranges this represents the min/max length.
For dimensions this represents the min/max pixel count or width/height in multi-component case.

</member>
<member name="F:libffmpeg.AVOptionRange.component_min" decl="false" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavutil\opt.h" line="324">
Value's component range.
For string this represents the unicode range for chars, 0-127 limits to ASCII.

</member>
<member name="F:libffmpeg.AVOptionRange.is_range" decl="false" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavutil\opt.h" line="329">
Range flag.
If set to 1 the struct encodes a range, if set to 0 a single value.

</member>
<member name="T:libffmpeg.AVOptionRanges" decl="false" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavutil\opt.h" line="336">
List of AVOptionRange structs.

</member>
<!-- Discarding badly formed XML document comment for member 'F:libffmpeg.AVOptionRanges.range'. -->
<member name="F:libffmpeg.AVOptionRanges.nb_ranges" decl="false" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavutil\opt.h" line="371">
Number of ranges per component.

</member>
<member name="F:libffmpeg.AVOptionRanges.nb_components" decl="false" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavutil\opt.h" line="375">
Number of componentes.

</member>
<member name="M:libffmpeg.av_set_string3(System.Void*,System.SByte!System.Runtime.CompilerServices.IsSignUnspecifiedByte!System.Runtime.CompilerServices.IsConst*,System.SByte!System.Runtime.CompilerServices.IsSignUnspecifiedByte!System.Runtime.CompilerServices.IsConst*,System.Int32,libffmpeg.AVOption!System.Runtime.CompilerServices.IsConst**)" decl="true" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavutil\opt.h" line="383">
 Set the field of obj with the given name to value.

 @param[in] obj A struct whose first element is a pointer to an
 AVClass.
 @param[in] name the name of the field to set
 @param[in] val The value to set. If the field is not of a string
 type, then the given string is parsed.
 SI postfixes and some named scalars are supported.
 If the field is of a numeric type, it has to be a numeric or named
 scalar. Behavior with more than one scalar and +- infix operators
 is undefined.
 If the field is of a flags type, it has to be a sequence of numeric
 scalars or named flags separated by '+' or '-'. Prefixing a flag
 with '+' causes it to be set without affecting the other flags;
 similarly, '-' unsets a flag.
 @param[out] o_out if non-NULL put here a pointer to the AVOption
 found
 @param alloc this parameter is currently ignored
 @return 0 if the value has been set, or an AVERROR code in case of
 error:
 AVERROR_OPTION_NOT_FOUND if no matching option exists
 AVERROR(ERANGE) if the value is out of range
 AVERROR(EINVAL) if the value is not valid
 @deprecated use av_opt_set()

</member>
<!-- Discarding badly formed XML document comment for member 'M:libffmpeg.av_opt_show2(System.Void*,System.Void*,System.Int32,System.Int32)'. -->
<member name="M:libffmpeg.av_opt_set_defaults(System.Void*)" decl="true" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavutil\opt.h" line="434">
 Set the values of all AVOption fields to their default values.

 @param s an AVOption-enabled struct (its first member must be a pointer to AVClass)

</member>
<member name="M:libffmpeg.av_set_options_string(System.Void*,System.SByte!System.Runtime.CompilerServices.IsSignUnspecifiedByte!System.Runtime.CompilerServices.IsConst*,System.SByte!System.Runtime.CompilerServices.IsSignUnspecifiedByte!System.Runtime.CompilerServices.IsConst*,System.SByte!System.Runtime.CompilerServices.IsSignUnspecifiedByte!System.Runtime.CompilerServices.IsConst*)" decl="true" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavutil\opt.h" line="446">
 Parse the key/value pairs list in opts. For each key/value pair
 found, stores the value in the field in ctx that is named like the
 key. ctx must be an AVClass context, storing is done using
 AVOptions.

 @param opts options string to parse, may be NULL
 @param key_val_sep a 0-terminated list of characters used to
 separate key from value
 @param pairs_sep a 0-terminated list of characters used to separate
 two pairs from each other
 @return the number of successfully set key/value pairs, or a negative
 value corresponding to an AVERROR code in case of error:
 AVERROR(EINVAL) if opts cannot be parsed,
 the error code issued by av_opt_set() if a key/value pair
 cannot be set

</member>
<member name="M:libffmpeg.av_opt_set_from_string(System.Void*,System.SByte!System.Runtime.CompilerServices.IsSignUnspecifiedByte!System.Runtime.CompilerServices.IsConst*,System.SByte!System.Runtime.CompilerServices.IsSignUnspecifiedByte!System.Runtime.CompilerServices.IsConst*!System.Runtime.CompilerServices.IsConst*,System.SByte!System.Runtime.CompilerServices.IsSignUnspecifiedByte!System.Runtime.CompilerServices.IsConst*,System.SByte!System.Runtime.CompilerServices.IsSignUnspecifiedByte!System.Runtime.CompilerServices.IsConst*)" decl="true" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavutil\opt.h" line="466">
 Parse the key-value pairs list in opts. For each key=value pair found,
 set the value of the corresponding option in ctx.

 @param ctx          the AVClass object to set options on
 @param opts         the options string, key-value pairs separated by a
                     delimiter
 @param shorthand    a NULL-terminated array of options names for shorthand
                     notation: if the first field in opts has no key part,
                     the key is taken from the first element of shorthand;
                     then again for the second, etc., until either opts is
                     finished, shorthand is finished or a named option is
                     found; after that, all options must be named
 @param key_val_sep  a 0-terminated list of characters used to separate
                     key from value, for example '='
 @param pairs_sep    a 0-terminated list of characters used to separate
                     two pairs from each other, for example ':' or ','
 @return  the number of successfully set key=value pairs, or a negative
          value corresponding to an AVERROR code in case of error:
          AVERROR(EINVAL) if opts cannot be parsed,
          the error code issued by av_set_string3() if a key/value pair
          cannot be set

 Options names must use only the following characters: a-z A-Z 0-9 - . / _
 Separators must use characters distinct from option names and from each
 other.

</member>
<member name="M:libffmpeg.av_opt_free(System.Void*)" decl="true" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavutil\opt.h" line="496">
Free all allocated objects in obj.

</member>
<member name="M:libffmpeg.av_opt_flag_is_set(System.Void*,System.SByte!System.Runtime.CompilerServices.IsSignUnspecifiedByte!System.Runtime.CompilerServices.IsConst*,System.SByte!System.Runtime.CompilerServices.IsSignUnspecifiedByte!System.Runtime.CompilerServices.IsConst*)" decl="true" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavutil\opt.h" line="501">
 Check whether a particular flag is set in a flags field.

 @param field_name the name of the flag field option
 @param flag_name the name of the flag to check
 @return non-zero if the flag is set, zero if the flag isn't set,
         isn't of the right type, or the flags field doesn't exist.

</member>
<member name="M:libffmpeg.av_opt_set_dict(System.Void*,libffmpeg.AVDictionary**)" decl="true" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavutil\opt.h" line="511">
 Set all the options from a given dictionary on an object.

 @param obj a struct whose first element is a pointer to AVClass
 @param options options to process. This dictionary will be freed and replaced
                by a new one containing all options not found in obj.
                Of course this new dictionary needs to be freed by caller
                with av_dict_free().

 @return 0 on success, a negative AVERROR if some option was found in obj,
         but could not be set.

 @see av_dict_copy()

</member>
<member name="M:libffmpeg.av_opt_set_dict2(System.Void*,libffmpeg.AVDictionary**,System.Int32)" decl="true" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavutil\opt.h" line="528">
 Set all the options from a given dictionary on an object.

 @param obj a struct whose first element is a pointer to AVClass
 @param options options to process. This dictionary will be freed and replaced
                by a new one containing all options not found in obj.
                Of course this new dictionary needs to be freed by caller
                with av_dict_free().
 @param search_flags A combination of AV_OPT_SEARCH_*.

 @return 0 on success, a negative AVERROR if some option was found in obj,
         but could not be set.

 @see av_dict_copy()

</member>
<member name="M:libffmpeg.av_opt_get_key_value(System.SByte!System.Runtime.CompilerServices.IsSignUnspecifiedByte!System.Runtime.CompilerServices.IsConst**,System.SByte!System.Runtime.CompilerServices.IsSignUnspecifiedByte!System.Runtime.CompilerServices.IsConst*,System.SByte!System.Runtime.CompilerServices.IsSignUnspecifiedByte!System.Runtime.CompilerServices.IsConst*,System.UInt32,System.SByte!System.Runtime.CompilerServices.IsSignUnspecifiedByte**,System.SByte!System.Runtime.CompilerServices.IsSignUnspecifiedByte**)" decl="true" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavutil\opt.h" line="545">
 Extract a key-value pair from the beginning of a string.

 @param ropts        pointer to the options string, will be updated to
                     point to the rest of the string (one of the pairs_sep
                     or the final NUL)
 @param key_val_sep  a 0-terminated list of characters used to separate
                     key from value, for example '='
 @param pairs_sep    a 0-terminated list of characters used to separate
                     two pairs from each other, for example ':' or ','
 @param flags        flags; see the AV_OPT_FLAG_* values below
 @param rkey         parsed key; must be freed using av_free()
 @param rval         parsed value; must be freed using av_free()

 @return  &gt;=0 for success, or a negative value corresponding to an
          AVERROR code in case of error; in particular:
          AVERROR(EINVAL) if no key is present


</member>
<member name="F:AV_OPT_FLAG_IMPLICIT_KEY" decl="false" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavutil\opt.h" line="571">
Accept to parse a value without a key; the key will then be returned
as NULL.

</member>
<member name="M:libffmpeg.av_opt_eval_flags(System.Void*,libffmpeg.AVOption!System.Runtime.CompilerServices.IsConst*,System.SByte!System.Runtime.CompilerServices.IsSignUnspecifiedByte!System.Runtime.CompilerServices.IsConst*,System.Int32*)" decl="true" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavutil\opt.h" line="578">
 @defgroup opt_eval_funcs Evaluating option strings
 @{
 This group of functions can be used to evaluate option strings
 and get numbers out of them. They do the same thing as av_opt_set(),
 except the result is written into the caller-supplied pointer.

 @param obj a struct whose first element is a pointer to AVClass.
 @param o an option for which the string is to be evaluated.
 @param val string to be evaluated.
 @param *_out value of the string will be written here.

 @return 0 on success, a negative number on failure.

</member>
<member name="M:libffmpeg.av_opt_find(System.Void*,System.SByte!System.Runtime.CompilerServices.IsSignUnspecifiedByte!System.Runtime.CompilerServices.IsConst*,System.SByte!System.Runtime.CompilerServices.IsSignUnspecifiedByte!System.Runtime.CompilerServices.IsConst*,System.Int32,System.Int32)" decl="true" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavutil\opt.h" line="598">
@}

The obj passed to av_opt_find() is fake -- only a double pointer to AVClass
instead of a required pointer to a struct containing AVClass. This is
useful for searching for options without needing to allocate the corresponding
object.

Allows av_opt_query_ranges and av_opt_query_ranges_default to return more than
one component for certain option types.
@see AVOptionRanges for details.

 Look for an option in an object. Consider only options which
 have all the specified flags set.

 @param[in] obj A pointer to a struct whose first element is a
                pointer to an AVClass.
                Alternatively a double pointer to an AVClass, if
                AV_OPT_SEARCH_FAKE_OBJ search flag is set.
 @param[in] name The name of the option to look for.
 @param[in] unit When searching for named constants, name of the unit
                 it belongs to.
 @param opt_flags Find only options with all the specified flags set (AV_OPT_FLAG).
 @param search_flags A combination of AV_OPT_SEARCH_*.

 @return A pointer to the option found, or NULL if no option
         was found.

 @note Options found with AV_OPT_SEARCH_CHILDREN flag may not be settable
 directly with av_opt_set(). Use special calls which take an options
 AVDictionary (e.g. avformat_open_input()) to set options found with this
 flag.

</member>
<member name="M:libffmpeg.av_opt_find2(System.Void*,System.SByte!System.Runtime.CompilerServices.IsSignUnspecifiedByte!System.Runtime.CompilerServices.IsConst*,System.SByte!System.Runtime.CompilerServices.IsSignUnspecifiedByte!System.Runtime.CompilerServices.IsConst*,System.Int32,System.Int32,System.Void**)" decl="true" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavutil\opt.h" line="644">
 Look for an option in an object. Consider only options which
 have all the specified flags set.

 @param[in] obj A pointer to a struct whose first element is a
                pointer to an AVClass.
                Alternatively a double pointer to an AVClass, if
                AV_OPT_SEARCH_FAKE_OBJ search flag is set.
 @param[in] name The name of the option to look for.
 @param[in] unit When searching for named constants, name of the unit
                 it belongs to.
 @param opt_flags Find only options with all the specified flags set (AV_OPT_FLAG).
 @param search_flags A combination of AV_OPT_SEARCH_*.
 @param[out] target_obj if non-NULL, an object to which the option belongs will be
 written here. It may be different from obj if AV_OPT_SEARCH_CHILDREN is present
 in search_flags. This parameter is ignored if search_flags contain
 AV_OPT_SEARCH_FAKE_OBJ.

 @return A pointer to the option found, or NULL if no option
         was found.

</member>
<member name="M:libffmpeg.av_opt_next(System.Void*,libffmpeg.AVOption!System.Runtime.CompilerServices.IsConst*)" decl="true" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavutil\opt.h" line="668">
 Iterate over all AVOptions belonging to obj.

 @param obj an AVOptions-enabled struct or a double pointer to an
            AVClass describing it.
 @param prev result of the previous call to av_opt_next() on this object
             or NULL
 @return next AVOption or NULL

</member>
<member name="M:libffmpeg.av_opt_child_next(System.Void*,System.Void*)" decl="true" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavutil\opt.h" line="679">
 Iterate over AVOptions-enabled children of obj.

 @param prev result of a previous call to this function or NULL
 @return next AVOptions-enabled child or NULL

</member>
<member name="M:libffmpeg.av_opt_child_class_next(libffmpeg.AVClass!System.Runtime.CompilerServices.IsConst*,libffmpeg.AVClass!System.Runtime.CompilerServices.IsConst*)" decl="true" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavutil\opt.h" line="687">
 Iterate over potential AVOptions-enabled children of parent.

 @param prev result of a previous call to this function or NULL
 @return AVClass corresponding to next potential child or NULL

</member>
<member name="M:libffmpeg.av_opt_set(System.Void*,System.SByte!System.Runtime.CompilerServices.IsSignUnspecifiedByte!System.Runtime.CompilerServices.IsConst*,System.SByte!System.Runtime.CompilerServices.IsSignUnspecifiedByte!System.Runtime.CompilerServices.IsConst*,System.Int32)" decl="true" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavutil\opt.h" line="695">
 @defgroup opt_set_funcs Option setting functions
 @{
 Those functions set the field of obj with the given name to value.

 @param[in] obj A struct whose first element is a pointer to an AVClass.
 @param[in] name the name of the field to set
 @param[in] val The value to set. In case of av_opt_set() if the field is not
 of a string type, then the given string is parsed.
 SI postfixes and some named scalars are supported.
 If the field is of a numeric type, it has to be a numeric or named
 scalar. Behavior with more than one scalar and +- infix operators
 is undefined.
 If the field is of a flags type, it has to be a sequence of numeric
 scalars or named flags separated by '+' or '-'. Prefixing a flag
 with '+' causes it to be set without affecting the other flags;
 similarly, '-' unsets a flag.
 @param search_flags flags passed to av_opt_find2. I.e. if AV_OPT_SEARCH_CHILDREN
 is passed here, then the option may be set on a child of obj.

 @return 0 if the value has been set, or an AVERROR code in case of
 error:
 AVERROR_OPTION_NOT_FOUND if no matching option exists
 AVERROR(ERANGE) if the value is out of range
 AVERROR(EINVAL) if the value is not valid

</member>
<member name="M:libffmpeg.av_opt_set_dict_val(System.Void*,System.SByte!System.Runtime.CompilerServices.IsSignUnspecifiedByte!System.Runtime.CompilerServices.IsConst*,libffmpeg.AVDictionary!System.Runtime.CompilerServices.IsConst*,System.Int32)" decl="true" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavutil\opt.h" line="731">
@note Any old dictionary present is discarded and replaced with a copy of the new one. The
caller still owns val is and responsible for freeing it.

</member>
<member name="M:libffmpeg.av_opt_get(System.Void*,System.SByte!System.Runtime.CompilerServices.IsSignUnspecifiedByte!System.Runtime.CompilerServices.IsConst*,System.Int32,System.Byte**)" decl="true" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavutil\opt.h" line="737">
 Set a binary option to an integer list.

 @param obj    AVClass object to set options on
 @param name   name of the binary option
 @param val    pointer to an integer list (must have the correct type with
               regard to the contents of the list)
 @param term   list terminator (usually 0 or -1)
 @param flags  search flags

@}

 @defgroup opt_get_funcs Option getting functions
 @{
 Those functions get a value of the option with the given name from an object.

 @param[in] obj a struct whose first element is a pointer to an AVClass.
 @param[in] name name of the option to get.
 @param[in] search_flags flags passed to av_opt_find2. I.e. if AV_OPT_SEARCH_CHILDREN
 is passed here, then the option may be found in a child of obj.
 @param[out] out_val value of the option will be written here
 @return &gt;=0 on success, a negative error code otherwise

@note the returned string will be av_malloc()ed and must be av_free()ed by the caller

</member>
<member name="M:libffmpeg.av_opt_get_dict_val(System.Void*,System.SByte!System.Runtime.CompilerServices.IsSignUnspecifiedByte!System.Runtime.CompilerServices.IsConst*,System.Int32,libffmpeg.AVDictionary**)" decl="true" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavutil\opt.h" line="781">
@param[out] out_val The returned dictionary is a copy of the actual value and must
be freed with av_dict_free() by the caller

</member>
<member name="M:libffmpeg.av_opt_ptr(libffmpeg.AVClass!System.Runtime.CompilerServices.IsConst*,System.Void*,System.SByte!System.Runtime.CompilerServices.IsSignUnspecifiedByte!System.Runtime.CompilerServices.IsConst*)" decl="true" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavutil\opt.h" line="786">
@}

 Gets a pointer to the requested field in a struct.
 This function allows accessing a struct even when its fields are moved or
 renamed since the application making the access has been compiled,

 @returns a pointer to the field, it can be cast to the correct type and read
          or written to.

</member>
<member name="M:libffmpeg.av_opt_freep_ranges(libffmpeg.AVOptionRanges**)" decl="true" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavutil\opt.h" line="799">
Free an AVOptionRanges struct and set it to NULL.

</member>
<member name="M:libffmpeg.av_opt_query_ranges(libffmpeg.AVOptionRanges**,System.Void*,System.SByte!System.Runtime.CompilerServices.IsSignUnspecifiedByte!System.Runtime.CompilerServices.IsConst*,System.Int32)" decl="true" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavutil\opt.h" line="804">
 Get a list of allowed ranges for the given option.

 The returned list may depend on other fields in obj like for example profile.

 @param flags is a bitmask of flags, undefined flags should not be set and should be ignored
              AV_OPT_SEARCH_FAKE_OBJ indicates that the obj is a double pointer to a AVClass instead of a full instance
              AV_OPT_MULTI_COMPONENT_RANGE indicates that function may return more than one component, @see AVOptionRanges

 The result must be freed with av_opt_freep_ranges.

 @return number of compontents returned on success, a negative errro code otherwise

</member>
<member name="M:libffmpeg.av_opt_copy(System.Void*,System.Void*)" decl="true" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavutil\opt.h" line="819">
 Copy options from src object into dest object.

 Options that require memory allocation (e.g. string or binary) are malloc'ed in dest object.
 Original memory allocated for such options is freed unless both src and dest options points to the same memory.

 @param dest Object to copy from
 @param src  Object to copy into
 @return 0 on success, negative on error

</member>
<member name="M:libffmpeg.av_opt_query_ranges_default(libffmpeg.AVOptionRanges**,System.Void*,System.SByte!System.Runtime.CompilerServices.IsSignUnspecifiedByte!System.Runtime.CompilerServices.IsConst*,System.Int32)" decl="true" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavutil\opt.h" line="831">
 Get a default list of allowed ranges for the given option.

 This list is constructed without using the AVClass.query_ranges() callback
 and can be used as fallback from within the callback.

 @param flags is a bitmask of flags, undefined flags should not be set and should be ignored
              AV_OPT_SEARCH_FAKE_OBJ indicates that the obj is a double pointer to a AVClass instead of a full instance
              AV_OPT_MULTI_COMPONENT_RANGE indicates that function may return more than one component, @see AVOptionRanges

 The result must be freed with av_opt_free_ranges.

 @return number of compontents returned on success, a negative errro code otherwise

</member>
<!-- Discarding badly formed XML document comment for member 'M:libffmpeg.av_opt_is_set_to_default(System.Void*,libffmpeg.AVOption!System.Runtime.CompilerServices.IsConst*)'. -->
<!-- Discarding badly formed XML document comment for member 'M:libffmpeg.av_opt_is_set_to_default_by_name(System.Void*,System.SByte!System.Runtime.CompilerServices.IsSignUnspecifiedByte!System.Runtime.CompilerServices.IsConst*,System.Int32)'. -->
<member name="M:libffmpeg.av_opt_serialize(System.Void*,System.Int32,System.Int32,System.SByte!System.Runtime.CompilerServices.IsSignUnspecifiedByte**,System.SByte!System.Runtime.CompilerServices.IsSignUnspecifiedByte!System.Runtime.CompilerServices.IsConst,System.SByte!System.Runtime.CompilerServices.IsSignUnspecifiedByte!System.Runtime.CompilerServices.IsConst)" decl="true" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavutil\opt.h" line="877">
 Serialize object's options.

 Create a string containing object's serialized options.
 Such string may be passed back to av_opt_set_from_string() in order to restore option values.
 A key/value or pairs separator occurring in the serialized value or
 name string are escaped through the av_escape() function.

 @param[in]  obj           AVClass object to serialize
 @param[in]  opt_flags     serialize options with all the specified flags set (AV_OPT_FLAG)
 @param[in]  flags         combination of AV_OPT_SERIALIZE_* flags
 @param[out] buffer        Pointer to buffer that will be allocated with string containg serialized options.
                           Buffer must be freed by the caller when is no longer needed.
 @param[in]  key_val_sep   character used to separate key from value
 @param[in]  pairs_sep     character used to separate two pairs from each other
 @return                   &gt;= 0 on success, negative on error
 @warning Separators cannot be neither '\\' nor '\0'. They also cannot be the same.

</member>
<member name="M:libffmpeg.av_strstart(System.SByte!System.Runtime.CompilerServices.IsSignUnspecifiedByte!System.Runtime.CompilerServices.IsConst*,System.SByte!System.Runtime.CompilerServices.IsSignUnspecifiedByte!System.Runtime.CompilerServices.IsConst*,System.SByte!System.Runtime.CompilerServices.IsSignUnspecifiedByte!System.Runtime.CompilerServices.IsConst**)" decl="true" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavutil\opt.h" line="897">
@}

@file
Macro definitions for various function/variable attributes

@addtogroup lavu_string
@{

 Return non-zero if pfx is a prefix of str. If it is, *ptr is set to
 the address of the first character in str after the prefix.

 @param str input string
 @param pfx prefix to test
 @param ptr updated if the prefix is matched inside str
 @return non-zero if the prefix matches, zero otherwise

</member>
<member name="M:libffmpeg.av_stristart(System.SByte!System.Runtime.CompilerServices.IsSignUnspecifiedByte!System.Runtime.CompilerServices.IsConst*,System.SByte!System.Runtime.CompilerServices.IsSignUnspecifiedByte!System.Runtime.CompilerServices.IsConst*,System.SByte!System.Runtime.CompilerServices.IsSignUnspecifiedByte!System.Runtime.CompilerServices.IsConst**)" decl="true" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavutil\avstring.h" line="44">
 Return non-zero if pfx is a prefix of str independent of case. If
 it is, *ptr is set to the address of the first character in str
 after the prefix.

 @param str input string
 @param pfx prefix to test
 @param ptr updated if the prefix is matched inside str
 @return non-zero if the prefix matches, zero otherwise

</member>
<member name="M:libffmpeg.av_stristr(System.SByte!System.Runtime.CompilerServices.IsSignUnspecifiedByte!System.Runtime.CompilerServices.IsConst*,System.SByte!System.Runtime.CompilerServices.IsSignUnspecifiedByte!System.Runtime.CompilerServices.IsConst*)" decl="true" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavutil\avstring.h" line="56">
 Locate the first case-independent occurrence in the string haystack
 of the string needle.  A zero-length string needle is considered to
 match at the start of haystack.

 This function is a case-insensitive version of the standard strstr().

 @param haystack string to search in
 @param needle   string to search for
 @return         pointer to the located match within haystack
                 or a null pointer if no match

</member>
<member name="M:libffmpeg.av_strnstr(System.SByte!System.Runtime.CompilerServices.IsSignUnspecifiedByte!System.Runtime.CompilerServices.IsConst*,System.SByte!System.Runtime.CompilerServices.IsSignUnspecifiedByte!System.Runtime.CompilerServices.IsConst*,System.UInt32)" decl="true" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavutil\avstring.h" line="70">
 Locate the first occurrence of the string needle in the string haystack
 where not more than hay_length characters are searched. A zero-length
 string needle is considered to match at the start of haystack.

 This function is a length-limited version of the standard strstr().

 @param haystack   string to search in
 @param needle     string to search for
 @param hay_length length of string to search in
 @return           pointer to the located match within haystack
                   or a null pointer if no match

</member>
<member name="M:libffmpeg.av_strlcpy(System.SByte!System.Runtime.CompilerServices.IsSignUnspecifiedByte*,System.SByte!System.Runtime.CompilerServices.IsSignUnspecifiedByte!System.Runtime.CompilerServices.IsConst*,System.UInt32)" decl="true" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavutil\avstring.h" line="85">
 Copy the string src to dst, but no more than size - 1 bytes, and
 null-terminate dst.

 This function is the same as BSD strlcpy().

 @param dst destination buffer
 @param src source string
 @param size size of destination buffer
 @return the length of src

 @warning since the return value is the length of src, src absolutely
 _must_ be a properly 0-terminated string, otherwise this will read beyond
 the end of the buffer and possibly crash.

</member>
<!-- Discarding badly formed XML document comment for member 'M:libffmpeg.av_strlcat(System.SByte!System.Runtime.CompilerServices.IsSignUnspecifiedByte*,System.SByte!System.Runtime.CompilerServices.IsSignUnspecifiedByte!System.Runtime.CompilerServices.IsConst*,System.UInt32)'. -->
<member name="M:libffmpeg.av_strlcatf(System.SByte!System.Runtime.CompilerServices.IsSignUnspecifiedByte*,System.UInt32,System.SByte!System.Runtime.CompilerServices.IsSignUnspecifiedByte!System.Runtime.CompilerServices.IsConst*,BTEllipsis)" decl="true" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavutil\avstring.h" line="120">
Append output to a string, according to a format. Never write out of
the destination buffer, and always put a terminating 0 within
the buffer.
@param dst destination buffer (string to which the output is
 appended)
@param size total size of the destination buffer
@param fmt printf-compatible format string, specifying how the
 following parameters are used
@return the length of the string that would have been generated
 if enough space had been available

</member>
<member name="M:libffmpeg.av_strnlen(System.SByte!System.Runtime.CompilerServices.IsSignUnspecifiedByte!System.Runtime.CompilerServices.IsConst*,System.UInt32)" decl="false" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavutil\avstring.h" line="134">
 Get the count of continuous non zero chars starting from the beginning.

 @param len maximum number of characters to check in the string, that
            is the maximum value which is returned by the function

</member>
<member name="M:libffmpeg.av_asprintf(System.SByte!System.Runtime.CompilerServices.IsSignUnspecifiedByte!System.Runtime.CompilerServices.IsConst*,BTEllipsis)" decl="true" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavutil\avstring.h" line="148">
Print arguments following specified format into a large enough auto
allocated buffer. It is similar to GNU asprintf().
@param fmt printf-compatible format string, specifying how the
           following parameters are used.
@return the allocated string
@note You have to free the string yourself with av_free().

</member>
<member name="M:libffmpeg.av_d2str(System.Double)" decl="true" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavutil\avstring.h" line="158">
Convert a number to a av_malloced string.

</member>
<member name="M:libffmpeg.av_get_token(System.SByte!System.Runtime.CompilerServices.IsSignUnspecifiedByte!System.Runtime.CompilerServices.IsConst**,System.SByte!System.Runtime.CompilerServices.IsSignUnspecifiedByte!System.Runtime.CompilerServices.IsConst*)" decl="true" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavutil\avstring.h" line="163">
 Unescape the given string until a non escaped terminating char,
 and return the token corresponding to the unescaped string.

 The normal \ and ' escaping is supported. Leading and trailing
 whitespaces are removed, unless they are escaped with '\' or are
 enclosed between ''.

 @param buf the buffer to parse, buf will be updated to point to the
 terminating char
 @param term a 0-terminated list of terminating chars
 @return the malloced unescaped string, which must be av_freed by
 the user, NULL in case of allocation failure

</member>
<member name="M:libffmpeg.av_strtok(System.SByte!System.Runtime.CompilerServices.IsSignUnspecifiedByte*,System.SByte!System.Runtime.CompilerServices.IsSignUnspecifiedByte!System.Runtime.CompilerServices.IsConst*,System.SByte!System.Runtime.CompilerServices.IsSignUnspecifiedByte**)" decl="true" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavutil\avstring.h" line="179">
 Split the string into several tokens which can be accessed by
 successive calls to av_strtok().

 A token is defined as a sequence of characters not belonging to the
 set specified in delim.

 On the first call to av_strtok(), s should point to the string to
 parse, and the value of saveptr is ignored. In subsequent calls, s
 should be NULL, and saveptr should be unchanged since the previous
 call.

 This function is similar to strtok_r() defined in POSIX.1.

 @param s the string to parse, may be NULL
 @param delim 0-terminated list of token delimiters, must be non-NULL
 @param saveptr user-provided pointer which points to stored
 information necessary for av_strtok() to continue scanning the same
 string. saveptr is updated to point to the next character after the
 first delimiter found, or to NULL if the string was terminated
 @return the found token, or NULL when no token is found

</member>
<member name="M:libffmpeg.av_isdigit(System.Int32)" decl="true" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavutil\avstring.h" line="203">
Locale-independent conversion of ASCII isdigit.

</member>
<member name="M:libffmpeg.av_isgraph(System.Int32)" decl="true" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavutil\avstring.h" line="208">
Locale-independent conversion of ASCII isgraph.

</member>
<member name="M:libffmpeg.av_isspace(System.Int32)" decl="true" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavutil\avstring.h" line="213">
Locale-independent conversion of ASCII isspace.

</member>
<member name="M:libffmpeg.av_toupper(System.Int32)" decl="false" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavutil\avstring.h" line="218">
Locale-independent conversion of ASCII characters to uppercase.

</member>
<member name="M:libffmpeg.av_tolower(System.Int32)" decl="false" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavutil\avstring.h" line="228">
Locale-independent conversion of ASCII characters to lowercase.

</member>
<member name="M:libffmpeg.av_isxdigit(System.Int32)" decl="true" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavutil\avstring.h" line="238">
Locale-independent conversion of ASCII isxdigit.

</member>
<member name="M:libffmpeg.av_strcasecmp(System.SByte!System.Runtime.CompilerServices.IsSignUnspecifiedByte!System.Runtime.CompilerServices.IsConst*,System.SByte!System.Runtime.CompilerServices.IsSignUnspecifiedByte!System.Runtime.CompilerServices.IsConst*)" decl="true" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavutil\avstring.h" line="243">
Locale-independent case-insensitive compare.
@note This means only ASCII-range characters are case-insensitive

</member>
<member name="M:libffmpeg.av_strncasecmp(System.SByte!System.Runtime.CompilerServices.IsSignUnspecifiedByte!System.Runtime.CompilerServices.IsConst*,System.SByte!System.Runtime.CompilerServices.IsSignUnspecifiedByte!System.Runtime.CompilerServices.IsConst*,System.UInt32)" decl="true" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavutil\avstring.h" line="249">
Locale-independent case-insensitive compare.
@note This means only ASCII-range characters are case-insensitive

</member>
<member name="M:libffmpeg.av_basename(System.SByte!System.Runtime.CompilerServices.IsSignUnspecifiedByte!System.Runtime.CompilerServices.IsConst*)" decl="true" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavutil\avstring.h" line="256">
Thread safe basename.
@param path the path, on DOS both \ and / are considered separators.
@return pointer to the basename substring.

</member>
<member name="M:libffmpeg.av_dirname(System.SByte!System.Runtime.CompilerServices.IsSignUnspecifiedByte*)" decl="true" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavutil\avstring.h" line="263">
Thread safe dirname.
@param path the path, on DOS both \ and / are considered separators.
@return the path with the separator replaced by the string terminator or ".".
@note the function may change the input string.

</member>
<member name="M:libffmpeg.av_match_name(System.SByte!System.Runtime.CompilerServices.IsSignUnspecifiedByte!System.Runtime.CompilerServices.IsConst*,System.SByte!System.Runtime.CompilerServices.IsSignUnspecifiedByte!System.Runtime.CompilerServices.IsConst*)" decl="true" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavutil\avstring.h" line="271">
Match instances of a name in a comma-separated list of names.
@param name  Name to look for.
@param names List of names.
@return 1 on match, 0 otherwise.

</member>
<member name="M:libffmpeg.av_escape(System.SByte!System.Runtime.CompilerServices.IsSignUnspecifiedByte**,System.SByte!System.Runtime.CompilerServices.IsSignUnspecifiedByte!System.Runtime.CompilerServices.IsConst*,System.SByte!System.Runtime.CompilerServices.IsSignUnspecifiedByte!System.Runtime.CompilerServices.IsConst*,libffmpeg.AVEscapeMode,System.Int32)" decl="true" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavutil\avstring.h" line="285">
 Consider spaces special and escape them even in the middle of the
 string.

 This is equivalent to adding the whitespace characters to the special
 characters lists, except it is guaranteed to use the exact same list
 of whitespace characters as the rest of libavutil.

Escape only specified special characters.
Without this flag, escape also any characters that may be considered
special by av_get_token(), such as the single quote.

 Escape string in src, and put the escaped string in an allocated
 string in *dst, which must be freed with av_free().

 @param dst           pointer where an allocated string is put
 @param src           string to escape, must be non-NULL
 @param special_chars string containing the special characters which
                      need to be escaped, can be NULL
 @param mode          escape mode to employ, see AV_ESCAPE_MODE_* macros.
                      Any unknown value for mode will be considered equivalent to
                      AV_ESCAPE_MODE_BACKSLASH, but this behaviour can change without
                      notice.
 @param flags         flags which control how to escape, see AV_ESCAPE_FLAG_ macros
 @return the length of the allocated string, or a negative error code in case of error
 @see av_bprint_escape()

</member>
<member name="M:libffmpeg.av_utf8_decode(System.Int32*,System.Byte!System.Runtime.CompilerServices.IsConst**,System.Byte!System.Runtime.CompilerServices.IsConst*,System.UInt32)" decl="true" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavutil\avstring.h" line="329">
 Read and decode a single UTF-8 code point (character) from the
 buffer in *buf, and update *buf to point to the next byte to
 decode.

 In case of an invalid byte sequence, the pointer will be updated to
 the next byte after the invalid sequence and the function will
 return an error code.

 Depending on the specified flags, the function will also fail in
 case the decoded code point does not belong to a valid range.

 @note For speed-relevant code a carefully implemented use of
 GET_UTF8() may be preferred.

 @param codep   pointer used to return the parsed code in case of success.
                The value in *codep is set even in case the range check fails.
 @param bufp    pointer to the address the first byte of the sequence
                to decode, updated by the function to point to the
                byte next after the decoded sequence
 @param buf_end pointer to the end of the buffer, points to the next
                byte past the last in the buffer. This is used to
                avoid buffer overreads (in case of an unfinished
                UTF-8 sequence towards the end of the buffer).
 @param flags   a collection of AV_UTF8_FLAG_* flags
 @return &gt;= 0 in case a sequence was successfully read, a negative
 value in case of invalid sequence

</member>
<member name="M:libffmpeg.av_match_list(System.SByte!System.Runtime.CompilerServices.IsSignUnspecifiedByte!System.Runtime.CompilerServices.IsConst*,System.SByte!System.Runtime.CompilerServices.IsSignUnspecifiedByte!System.Runtime.CompilerServices.IsConst*,System.SByte!System.Runtime.CompilerServices.IsSignUnspecifiedByte)" decl="true" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavutil\avstring.h" line="360">
Check if a name is in a list.
@returns 0 if not found, or the 1 based index where it has been found in the
           list.

</member>
<member name="M:libffmpeg.av_gettime" decl="true" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavutil\avstring.h" line="367">
@}

Get the current time in microseconds.

</member>
<member name="M:libffmpeg.av_gettime_relative" decl="true" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavutil\time.h" line="31">
Get the current time in microseconds since some unspecified starting point.
On platforms that support it, the time comes from a monotonic clock
This property makes this time source ideal for measuring relative time.
The returned values may not be monotonic on platforms where a monotonic
clock is not available.

</member>
<member name="M:libffmpeg.av_gettime_relative_is_monotonic" decl="true" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavutil\time.h" line="40">
Indicates with a boolean result if the av_gettime_relative() time source
is monotonic.

</member>
<member name="M:libffmpeg.av_usleep(System.UInt32)" decl="true" source="d:\projects\ispy\ispy\ffmpeg\ffmpeg\include\libavutil\time.h" line="46">
 Sleep for a period of time.  Although the duration is expressed in
 microseconds, the actual delay may be rounded to the precision of the
 system timer.

 @param  usec Number of microseconds to sleep.
 @return zero on success or (negative) error code.

</member>
</members>
</doc>