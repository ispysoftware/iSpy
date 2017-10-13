using System;
using System.Runtime.InteropServices;

namespace FFmpeg.AutoGen
{
    public unsafe struct _iobuf
    {
        public void* @_Placeholder;
    }
    
    /// <summary>Rational number (pair of numerator and denominator).</summary>
    public unsafe struct AVRational
    {
        /// <summary>Numerator</summary>
        public int @num;
        /// <summary>Denominator</summary>
        public int @den;
    }
    
    /// <summary>Describe the class of an AVClass context structure. That is an arbitrary struct of which the first field is a pointer to an AVClass struct (e.g. AVCodecContext, AVFormatContext etc.).</summary>
    public unsafe struct AVClass
    {
        /// <summary>The name of the class; usually it is the same name as the context structure type to which the AVClass is associated.</summary>
        public byte* @class_name;
        /// <summary>A pointer to a function which returns the name of a context instance ctx associated with the class.</summary>
        public AVClass_item_name_func @item_name;
        /// <summary>a pointer to the first option specified in the class if any or NULL</summary>
        public AVOption* @option;
        /// <summary>LIBAVUTIL_VERSION with which this structure was created. This is used to allow fields to be added without requiring major version bumps everywhere.</summary>
        public int @version;
        /// <summary>Offset in the structure where log_level_offset is stored. 0 means there is no such variable</summary>
        public int @log_level_offset_offset;
        /// <summary>Offset in the structure where a pointer to the parent context for logging is stored. For example a decoder could pass its AVCodecContext to eval as such a parent context, which an av_log() implementation could then leverage to display the parent context. The offset can be NULL.</summary>
        public int @parent_log_context_offset;
        /// <summary>Return next AVOptions-enabled child or NULL</summary>
        public AVClass_child_next_func @child_next;
        /// <summary>Return an AVClass corresponding to the next potential AVOptions-enabled child.</summary>
        public AVClass_child_class_next_func @child_class_next;
        /// <summary>Category used for visualization (like color) This is only set if the category is equal for all objects using this class. available since version (51 &lt;&lt; 16 | 56 &lt;&lt; 8 | 100)</summary>
        public AVClassCategory @category;
        /// <summary>Callback to return the category. available since version (51 &lt;&lt; 16 | 59 &lt;&lt; 8 | 100)</summary>
        public AVClass_get_category_func @get_category;
        /// <summary>Callback to return the supported/allowed ranges. available since version (52.12)</summary>
        public AVClass_query_ranges_func @query_ranges;
    }
    
    /// <summary>AVOption</summary>
    public unsafe struct AVOption
    {
        public byte* @name;
        /// <summary>short English help text</summary>
        public byte* @help;
        /// <summary>The offset relative to the context structure where the option value is stored. It should be 0 for named constants.</summary>
        public int @offset;
        public AVOptionType @type;
        public AVOption_default_val @default_val;
        /// <summary>minimum valid value for the option</summary>
        public double @min;
        /// <summary>maximum valid value for the option</summary>
        public double @max;
        public int @flags;
        /// <summary>The logical unit to which the option belongs. Non-constant options and corresponding named constants share the same unit. May be NULL.</summary>
        public byte* @unit;
    }
    
    /// <summary>List of AVOptionRange structs.</summary>
    public unsafe struct AVOptionRanges
    {
        /// <summary>Array of option ranges.</summary>
        public AVOptionRange** @range;
        /// <summary>Number of ranges per component.</summary>
        public int @nb_ranges;
        /// <summary>Number of componentes.</summary>
        public int @nb_components;
    }
    
    public unsafe struct AVFifoBuffer
    {
        public byte* @buffer;
        public byte* @rptr;
        public byte* @wptr;
        public byte* @end;
        public uint @rndx;
        public uint @wndx;
    }
    
    /// <summary>Structure to hold side data for an AVFrame.</summary>
    public unsafe struct AVFrameSideData
    {
        public AVFrameSideDataType @type;
        public byte* @data;
        public int @size;
        public AVDictionary* @metadata;
        public AVBufferRef* @buf;
    }
    
    /// <summary>A reference to a data buffer.</summary>
    public unsafe struct AVBufferRef
    {
        public AVBuffer* @buffer;
        /// <summary>The data buffer. It is considered writable if and only if this is the only reference to the buffer, in which case av_buffer_is_writable() returns 1.</summary>
        public byte* @data;
        /// <summary>Size of data in bytes.</summary>
        public int @size;
    }
    
    /// <summary>This structure describes decoded (raw) audio or video data.</summary>
    public unsafe struct AVFrame
    {
        /// <summary>pointer to the picture/channel planes. This might be different from the first allocated byte</summary>
        public byte_ptrArray8 @data;
        /// <summary>For video, size in bytes of each picture line. For audio, size in bytes of each plane.</summary>
        public int_array8 @linesize;
        /// <summary>pointers to the data planes/channels.</summary>
        public byte** @extended_data;
        /// <summary>width and height of the video frame</summary>
        public int @width;
        /// <summary>width and height of the video frame</summary>
        public int @height;
        /// <summary>number of audio samples (per channel) described by this frame</summary>
        public int @nb_samples;
        /// <summary>format of the frame, -1 if unknown or unset Values correspond to enum AVPixelFormat for video frames, enum AVSampleFormat for audio)</summary>
        public int @format;
        /// <summary>1 -&gt; keyframe, 0-&gt; not</summary>
        public int @key_frame;
        /// <summary>Picture type of the frame.</summary>
        public AVPictureType @pict_type;
        /// <summary>Sample aspect ratio for the video frame, 0/1 if unknown/unspecified.</summary>
        public AVRational @sample_aspect_ratio;
        /// <summary>Presentation timestamp in time_base units (time when frame should be shown to user).</summary>
        public long @pts;
        /// <summary>PTS copied from the AVPacket that was decoded to produce this frame.</summary>
        public long @pkt_pts;
        /// <summary>DTS copied from the AVPacket that triggered returning this frame. (if frame threading isn&apos;t used) This is also the Presentation time of this AVFrame calculated from only AVPacket.dts values without pts values.</summary>
        public long @pkt_dts;
        /// <summary>picture number in bitstream order</summary>
        public int @coded_picture_number;
        /// <summary>picture number in display order</summary>
        public int @display_picture_number;
        /// <summary>quality (between 1 (good) and FF_LAMBDA_MAX (bad))</summary>
        public int @quality;
        /// <summary>for some private data of the user</summary>
        public void* @opaque;
        public ulong_array8 @error;
        /// <summary>When decoding, this signals how much the picture must be delayed. extra_delay = repeat_pict / (2*fps)</summary>
        public int @repeat_pict;
        /// <summary>The content of the picture is interlaced.</summary>
        public int @interlaced_frame;
        /// <summary>If the content is interlaced, is top field displayed first.</summary>
        public int @top_field_first;
        /// <summary>Tell user application that palette has changed from previous frame.</summary>
        public int @palette_has_changed;
        /// <summary>reordered opaque 64 bits (generally an integer or a double precision float PTS but can be anything). The user sets AVCodecContext.reordered_opaque to represent the input at that time, the decoder reorders values as needed and sets AVFrame.reordered_opaque to exactly one of the values provided by the user through AVCodecContext.reordered_opaque</summary>
        public long @reordered_opaque;
        /// <summary>Sample rate of the audio data.</summary>
        public int @sample_rate;
        /// <summary>Channel layout of the audio data.</summary>
        public ulong @channel_layout;
        /// <summary>AVBuffer references backing the data for this frame. If all elements of this array are NULL, then this frame is not reference counted. This array must be filled contiguously -- if buf[i] is non-NULL then buf[j] must also be non-NULL for all j &lt; i.</summary>
        public AVBufferRef_ptrArray8 @buf;
        /// <summary>For planar audio which requires more than AV_NUM_DATA_POINTERS AVBufferRef pointers, this array will hold all the references which cannot fit into AVFrame.buf.</summary>
        public AVBufferRef** @extended_buf;
        /// <summary>Number of elements in extended_buf.</summary>
        public int @nb_extended_buf;
        public AVFrameSideData** @side_data;
        public int @nb_side_data;
        /// <summary>Frame flags, a combination of</summary>
        public int @flags;
        /// <summary>MPEG vs JPEG YUV range. - encoding: Set by user - decoding: Set by libavcodec</summary>
        public AVColorRange @color_range;
        public AVColorPrimaries @color_primaries;
        public AVColorTransferCharacteristic @color_trc;
        /// <summary>YUV colorspace type. - encoding: Set by user - decoding: Set by libavcodec</summary>
        public AVColorSpace @colorspace;
        public AVChromaLocation @chroma_location;
        /// <summary>frame timestamp estimated using various heuristics, in stream time base - encoding: unused - decoding: set by libavcodec, read by user.</summary>
        public long @best_effort_timestamp;
        /// <summary>reordered pos from the last AVPacket that has been input into the decoder - encoding: unused - decoding: Read by user.</summary>
        public long @pkt_pos;
        /// <summary>duration of the corresponding packet, expressed in AVStream-&gt;time_base units, 0 if unknown. - encoding: unused - decoding: Read by user.</summary>
        public long @pkt_duration;
        /// <summary>metadata. - encoding: Set by user. - decoding: Set by libavcodec.</summary>
        public AVDictionary* @metadata;
        /// <summary>decode error flags of the frame, set to a combination of FF_DECODE_ERROR_xxx flags if the decoder produced a frame, but there were errors during the decoding. - encoding: unused - decoding: set by libavcodec, read by user.</summary>
        public int @decode_error_flags;
        /// <summary>number of audio channels, only used for audio. - encoding: unused - decoding: Read by user.</summary>
        public int @channels;
        /// <summary>size of the corresponding packet containing the compressed frame. It is set to a negative value if unknown. - encoding: unused - decoding: set by libavcodec, read by user.</summary>
        public int @pkt_size;
        /// <summary>QP table</summary>
        public sbyte* @qscale_table;
        /// <summary>QP store stride</summary>
        public int @qstride;
        public int @qscale_type;
        public AVBufferRef* @qp_table_buf;
        /// <summary>For hwaccel-format frames, this should be a reference to the AVHWFramesContext describing the frame.</summary>
        public AVBufferRef* @hw_frames_ctx;
        /// <summary>AVBufferRef for free use by the API user. FFmpeg will never check the contents of the buffer ref. FFmpeg calls av_buffer_unref() on it when the frame is unreferenced. av_frame_copy_props() calls create a new reference with av_buffer_ref() for the target frame&apos;s opaque_ref field.</summary>
        public AVBufferRef* @opaque_ref;
    }
    
    public unsafe struct AVDictionaryEntry
    {
        public byte* @key;
        public byte* @value;
    }
    
    /// <summary>A single allowed range of values, or a single allowed value.</summary>
    public unsafe struct AVOptionRange
    {
        public byte* @str;
        /// <summary>Value range. For string ranges this represents the min/max length. For dimensions this represents the min/max pixel count or width/height in multi-component case.</summary>
        public double @value_min;
        /// <summary>Value range. For string ranges this represents the min/max length. For dimensions this represents the min/max pixel count or width/height in multi-component case.</summary>
        public double @value_max;
        /// <summary>Value&apos;s component range. For string this represents the unicode range for chars, 0-127 limits to ASCII.</summary>
        public double @component_min;
        /// <summary>Value&apos;s component range. For string this represents the unicode range for chars, 0-127 limits to ASCII.</summary>
        public double @component_max;
        /// <summary>Range flag. If set to 1 the struct encodes a range, if set to 0 a single value.</summary>
        public int @is_range;
    }
    
    /// <summary>the default value for scalar options</summary>
    public unsafe struct AVOption_default_val
    {
        public long @i64;
        public double @dbl;
        public byte* @str;
        public AVRational @q;
    }
    
    /// <summary>Descriptor that unambiguously describes how the bits of a pixel are stored in the up to 4 data planes of an image. It also stores the subsampling factors and number of components.</summary>
    public unsafe struct AVPixFmtDescriptor
    {
        public byte* @name;
        /// <summary>The number of components each pixel has, (1-4)</summary>
        public byte @nb_components;
        /// <summary>Amount to shift the luma width right to find the chroma width. For YV12 this is 1 for example. chroma_width = AV_CEIL_RSHIFT(luma_width, log2_chroma_w) The note above is needed to ensure rounding up. This value only refers to the chroma components.</summary>
        public byte @log2_chroma_w;
        /// <summary>Amount to shift the luma height right to find the chroma height. For YV12 this is 1 for example. chroma_height= AV_CEIL_RSHIFT(luma_height, log2_chroma_h) The note above is needed to ensure rounding up. This value only refers to the chroma components.</summary>
        public byte @log2_chroma_h;
        /// <summary>Combination of AV_PIX_FMT_FLAG_... flags.</summary>
        public ulong @flags;
        /// <summary>Parameters that describe how pixels are packed. If the format has 1 or 2 components, then luma is 0. If the format has 3 or 4 components: if the RGB flag is set then 0 is red, 1 is green and 2 is blue; otherwise 0 is luma, 1 is chroma-U and 2 is chroma-V.</summary>
        public AVComponentDescriptor_array4 @comp;
        /// <summary>Alternative comma-separated names.</summary>
        public byte* @alias;
    }
    
    public unsafe struct AVComponentDescriptor
    {
        /// <summary>Which of the 4 planes contains the component.</summary>
        public int @plane;
        /// <summary>Number of elements between 2 horizontally consecutive pixels. Elements are bits for bitstream formats, bytes otherwise.</summary>
        public int @step;
        /// <summary>Number of elements before the component of the first pixel. Elements are bits for bitstream formats, bytes otherwise.</summary>
        public int @offset;
        /// <summary>Number of least significant bits that must be shifted away to get the value.</summary>
        public int @shift;
        /// <summary>Number of bits in the component.</summary>
        public int @depth;
        /// <summary>deprecated, use step instead</summary>
        public int @step_minus1;
        /// <summary>deprecated, use depth instead</summary>
        public int @depth_minus1;
        /// <summary>deprecated, use offset instead</summary>
        public int @offset_plus1;
    }
    
    public unsafe struct AVTimecode
    {
        /// <summary>timecode frame start (first base frame number)</summary>
        public int @start;
        /// <summary>flags such as drop frame, +24 hours support, ...</summary>
        public uint @flags;
        /// <summary>frame rate in rational form</summary>
        public AVRational @rate;
        /// <summary>frame per second; must be consistent with the rate field</summary>
        public uint @fps;
    }
    
    /// <summary>This struct aggregates all the (hardware/vendor-specific) &quot;high-level&quot; state, i.e. state that is not tied to a concrete processing configuration. E.g., in an API that supports hardware-accelerated encoding and decoding, this struct will (if possible) wrap the state that is common to both encoding and decoding and from which specific instances of encoders or decoders can be derived.</summary>
    public unsafe struct AVHWDeviceContext
    {
        /// <summary>A class for logging. Set by av_hwdevice_ctx_alloc().</summary>
        public AVClass* @av_class;
        /// <summary>Private data used internally by libavutil. Must not be accessed in any way by the caller.</summary>
        public AVHWDeviceInternal* @internal;
        /// <summary>This field identifies the underlying API used for hardware access.</summary>
        public AVHWDeviceType @type;
        /// <summary>The format-specific data, allocated and freed by libavutil along with this context.</summary>
        public void* @hwctx;
        /// <summary>This field may be set by the caller before calling av_hwdevice_ctx_init().</summary>
        public AVHWDeviceContext_free_func @free;
        /// <summary>Arbitrary user data, to be used e.g. by the free() callback.</summary>
        public void* @user_opaque;
    }
    
    /// <summary>This struct describes a set or pool of &quot;hardware&quot; frames (i.e. those with data not located in normal system memory). All the frames in the pool are assumed to be allocated in the same way and interchangeable.</summary>
    public unsafe struct AVHWFramesContext
    {
        /// <summary>A class for logging.</summary>
        public AVClass* @av_class;
        /// <summary>Private data used internally by libavutil. Must not be accessed in any way by the caller.</summary>
        public AVHWFramesInternal* @internal;
        /// <summary>A reference to the parent AVHWDeviceContext. This reference is owned and managed by the enclosing AVHWFramesContext, but the caller may derive additional references from it.</summary>
        public AVBufferRef* @device_ref;
        /// <summary>The parent AVHWDeviceContext. This is simply a pointer to device_ref-&gt;data provided for convenience.</summary>
        public AVHWDeviceContext* @device_ctx;
        /// <summary>The format-specific data, allocated and freed automatically along with this context.</summary>
        public void* @hwctx;
        /// <summary>This field may be set by the caller before calling av_hwframe_ctx_init().</summary>
        public AVHWFramesContext_free_func @free;
        /// <summary>Arbitrary user data, to be used e.g. by the free() callback.</summary>
        public void* @user_opaque;
        /// <summary>A pool from which the frames are allocated by av_hwframe_get_buffer(). This field may be set by the caller before calling av_hwframe_ctx_init(). The buffers returned by calling av_buffer_pool_get() on this pool must have the properties described in the documentation in the corresponding hw type&apos;s header (hwcontext_*.h). The pool will be freed strictly before this struct&apos;s free() callback is invoked.</summary>
        public AVBufferPool* @pool;
        /// <summary>Initial size of the frame pool. If a device type does not support dynamically resizing the pool, then this is also the maximum pool size.</summary>
        public int @initial_pool_size;
        /// <summary>The pixel format identifying the underlying HW surface type.</summary>
        public AVPixelFormat @format;
        /// <summary>The pixel format identifying the actual data layout of the hardware frames.</summary>
        public AVPixelFormat @sw_format;
        /// <summary>The allocated dimensions of the frames in this pool.</summary>
        public int @width;
        /// <summary>The allocated dimensions of the frames in this pool.</summary>
        public int @height;
    }
    
    /// <summary>This struct describes the constraints on hardware frames attached to a given device with a hardware-specific configuration. This is returned by av_hwdevice_get_hwframe_constraints() and must be freed by av_hwframe_constraints_free() after use.</summary>
    public unsafe struct AVHWFramesConstraints
    {
        /// <summary>A list of possible values for format in the hw_frames_ctx, terminated by AV_PIX_FMT_NONE. This member will always be filled.</summary>
        public AVPixelFormat* @valid_hw_formats;
        /// <summary>A list of possible values for sw_format in the hw_frames_ctx, terminated by AV_PIX_FMT_NONE. Can be NULL if this information is not known.</summary>
        public AVPixelFormat* @valid_sw_formats;
        /// <summary>The minimum size of frames in this hw_frames_ctx. (Zero if not known.)</summary>
        public int @min_width;
        public int @min_height;
        /// <summary>The maximum size of frames in this hw_frames_ctx. (INT_MAX if not known / no limit.)</summary>
        public int @max_width;
        public int @max_height;
    }
    
    /// <summary>This struct is allocated as AVHWDeviceContext.hwctx</summary>
    public unsafe struct AVDXVA2DeviceContext
    {
        public IDirect3DDeviceManager9* @devmgr;
    }
    
    public unsafe struct IDirect3DDeviceManager9
    {
        public IDirect3DDeviceManager9Vtbl* @lpVtbl;
    }
    
    public unsafe struct IDirect3DDeviceManager9Vtbl
    {
        public void* @QueryInterface;
        public void* @AddRef;
        public void* @Release;
        public void* @ResetDevice;
        public void* @OpenDeviceHandle;
        public void* @CloseDeviceHandle;
        public void* @TestDevice;
        public void* @LockDevice;
        public void* @UnlockDevice;
        public void* @GetVideoService;
    }
    
    /// <summary>This struct is allocated as AVHWFramesContext.hwctx</summary>
    public unsafe struct AVDXVA2FramesContext
    {
        /// <summary>The surface type (e.g. DXVA2_VideoProcessorRenderTarget or DXVA2_VideoDecoderRenderTarget). Must be set by the caller.</summary>
        public ulong @surface_type;
        /// <summary>The surface pool. When an external pool is not provided by the caller, this will be managed (allocated and filled on init, freed on uninit) by libavutil.</summary>
        public IDirect3DSurface9** @surfaces;
        public int @nb_surfaces;
        /// <summary>Certain drivers require the decoder to be destroyed before the surfaces. To allow internally managed pools to work properly in such cases, this field is provided.</summary>
        public IDirectXVideoDecoder* @decoder_to_release;
    }
    
    public unsafe struct IDirect3DSurface9
    {
        public IDirect3DSurface9Vtbl* @lpVtbl;
    }
    
    public unsafe struct IDirect3DSurface9Vtbl
    {
        public void* @QueryInterface;
        public void* @AddRef;
        public void* @Release;
        public void* @GetDevice;
        public void* @SetPrivateData;
        public void* @GetPrivateData;
        public void* @FreePrivateData;
        public void* @SetPriority;
        public void* @GetPriority;
        public void* @PreLoad;
        public void* @GetType;
        public void* @GetContainer;
        public void* @GetDesc;
        public void* @LockRect;
        public void* @UnlockRect;
        public void* @GetDC;
        public void* @ReleaseDC;
    }
    
    public unsafe struct IDirectXVideoDecoder
    {
        public IDirectXVideoDecoderVtbl* @lpVtbl;
    }
    
    public unsafe struct IDirectXVideoDecoderVtbl
    {
        public void* @QueryInterface;
        public void* @AddRef;
        public void* @Release;
        public void* @GetVideoDecoderService;
        public void* @GetCreationParameters;
        public void* @GetBuffer;
        public void* @ReleaseBuffer;
        public void* @BeginFrame;
        public void* @EndFrame;
        public void* @Execute;
    }
    
    public unsafe struct SwsVector
    {
        /// <summary>pointer to the list of coefficients</summary>
        public double* @coeff;
        /// <summary>number of coefficients in the vector</summary>
        public int @length;
    }
    
    public unsafe struct SwsFilter
    {
        public SwsVector* @lumH;
        public SwsVector* @lumV;
        public SwsVector* @chrH;
        public SwsVector* @chrV;
    }
    
    /// <summary>This struct describes the properties of a single codec described by an AVCodecID.</summary>
    public unsafe struct AVCodecDescriptor
    {
        public AVCodecID @id;
        public AVMediaType @type;
        /// <summary>Name of the codec described by this descriptor. It is non-empty and unique for each codec descriptor. It should contain alphanumeric characters and &apos;_&apos; only.</summary>
        public byte* @name;
        /// <summary>A more descriptive name for this codec. May be NULL.</summary>
        public byte* @long_name;
        /// <summary>Codec properties, a combination of AV_CODEC_PROP_* flags.</summary>
        public int @props;
        /// <summary>MIME type(s) associated with the codec. May be NULL; if not, a NULL-terminated array of MIME types. The first item is always non-NULL and is the preferred MIME type.</summary>
        public byte** @mime_types;
        /// <summary>If non-NULL, an array of profiles recognized for this codec. Terminated with FF_PROFILE_UNKNOWN.</summary>
        public AVProfile* @profiles;
    }
    
    /// <summary>AVProfile.</summary>
    public unsafe struct AVProfile
    {
        public int @profile;
        /// <summary>short name for the profile</summary>
        public byte* @name;
    }
    
    public unsafe struct RcOverride
    {
        public int @start_frame;
        public int @end_frame;
        public int @qscale;
        public float @quality_factor;
    }
    
    /// <summary>Pan Scan area. This specifies the area which should be displayed. Note there may be multiple such areas for one frame.</summary>
    public unsafe struct AVPanScan
    {
        /// <summary>id - encoding: Set by user. - decoding: Set by libavcodec.</summary>
        public int @id;
        /// <summary>width and height in 1/16 pel - encoding: Set by user. - decoding: Set by libavcodec.</summary>
        public int @width;
        public int @height;
        /// <summary>position of the top left corner in 1/16 pel for up to 3 fields/frames - encoding: Set by user. - decoding: Set by libavcodec.</summary>
        public short_arrayOfArray6 @position;
    }
    
    /// <summary>This structure describes the bitrate properties of an encoded bitstream. It roughly corresponds to a subset the VBV parameters for MPEG-2 or HRD parameters for H.264/HEVC.</summary>
    public unsafe struct AVCPBProperties
    {
        /// <summary>Maximum bitrate of the stream, in bits per second. Zero if unknown or unspecified.</summary>
        public int @max_bitrate;
        /// <summary>Minimum bitrate of the stream, in bits per second. Zero if unknown or unspecified.</summary>
        public int @min_bitrate;
        /// <summary>Average bitrate of the stream, in bits per second. Zero if unknown or unspecified.</summary>
        public int @avg_bitrate;
        /// <summary>The size of the buffer to which the ratecontrol is applied, in bits. Zero if unknown or unspecified.</summary>
        public int @buffer_size;
        /// <summary>The delay between the time the packet this structure is associated with is received and the time when it should be decoded, in periods of a 27MHz clock.</summary>
        public ulong @vbv_delay;
    }
    
    public unsafe struct AVPacketSideData
    {
        public byte* @data;
        public int @size;
        public AVPacketSideDataType @type;
    }
    
    /// <summary>This structure stores compressed data. It is typically exported by demuxers and then passed as input to decoders, or received as output from encoders and then passed to muxers.</summary>
    public unsafe struct AVPacket
    {
        /// <summary>A reference to the reference-counted buffer where the packet data is stored. May be NULL, then the packet data is not reference-counted.</summary>
        public AVBufferRef* @buf;
        /// <summary>Presentation timestamp in AVStream-&gt;time_base units; the time at which the decompressed packet will be presented to the user. Can be AV_NOPTS_VALUE if it is not stored in the file. pts MUST be larger or equal to dts as presentation cannot happen before decompression, unless one wants to view hex dumps. Some formats misuse the terms dts and pts/cts to mean something different. Such timestamps must be converted to true pts/dts before they are stored in AVPacket.</summary>
        public long @pts;
        /// <summary>Decompression timestamp in AVStream-&gt;time_base units; the time at which the packet is decompressed. Can be AV_NOPTS_VALUE if it is not stored in the file.</summary>
        public long @dts;
        public byte* @data;
        public int @size;
        public int @stream_index;
        /// <summary>A combination of AV_PKT_FLAG values</summary>
        public int @flags;
        /// <summary>Additional packet data that can be provided by the container. Packet can contain several types of side information.</summary>
        public AVPacketSideData* @side_data;
        public int @side_data_elems;
        /// <summary>Duration of this packet in AVStream-&gt;time_base units, 0 if unknown. Equals next_pts - this_pts in presentation order.</summary>
        public long @duration;
        /// <summary>byte position in stream, -1 if unknown</summary>
        public long @pos;
        public long @convergence_duration;
    }
    
    /// <summary>main external API structure. New fields can be added to the end with minor version bumps. Removal, reordering and changes to existing fields require a major version bump. You can use AVOptions (av_opt* / av_set/get*()) to access these fields from user applications. The name string for AVOptions options matches the associated command line parameter name and can be found in libavcodec/options_table.h The AVOption/command line parameter names differ in some cases from the C structure field names for historic reasons or brevity. sizeof(AVCodecContext) must not be used outside libav*.</summary>
    public unsafe struct AVCodecContext
    {
        /// <summary>information on struct for av_log - set by avcodec_alloc_context3</summary>
        public AVClass* @av_class;
        public int @log_level_offset;
        public AVMediaType @codec_type;
        public AVCodec* @codec;
        public byte_array32 @codec_name;
        public AVCodecID @codec_id;
        /// <summary>fourcc (LSB first, so &quot;ABCD&quot; -&gt; (&apos;D&apos;&lt;&lt;24) + (&apos;C&apos;&lt;&lt;16) + (&apos;B&apos;&lt;&lt;8) + &apos;A&apos;). This is used to work around some encoder bugs. A demuxer should set this to what is stored in the field used to identify the codec. If there are multiple such fields in a container then the demuxer should choose the one which maximizes the information about the used codec. If the codec tag field in a container is larger than 32 bits then the demuxer should remap the longer ID to 32 bits with a table or other structure. Alternatively a new extra_codec_tag + size could be added but for this a clear advantage must be demonstrated first. - encoding: Set by user, if not then the default based on codec_id will be used. - decoding: Set by user, will be converted to uppercase by libavcodec during init.</summary>
        public uint @codec_tag;
        public uint @stream_codec_tag;
        public void* @priv_data;
        /// <summary>Private context used for internal data.</summary>
        public AVCodecInternal* @internal;
        /// <summary>Private data of the user, can be used to carry app specific stuff. - encoding: Set by user. - decoding: Set by user.</summary>
        public void* @opaque;
        /// <summary>the average bitrate - encoding: Set by user; unused for constant quantizer encoding. - decoding: Set by user, may be overwritten by libavcodec if this info is available in the stream</summary>
        public long @bit_rate;
        /// <summary>number of bits the bitstream is allowed to diverge from the reference. the reference can be CBR (for CBR pass1) or VBR (for pass2) - encoding: Set by user; unused for constant quantizer encoding. - decoding: unused</summary>
        public int @bit_rate_tolerance;
        /// <summary>Global quality for codecs which cannot change it per frame. This should be proportional to MPEG-1/2/4 qscale. - encoding: Set by user. - decoding: unused</summary>
        public int @global_quality;
        /// <summary>- encoding: Set by user. - decoding: unused</summary>
        public int @compression_level;
        /// <summary>AV_CODEC_FLAG_*. - encoding: Set by user. - decoding: Set by user.</summary>
        public int @flags;
        /// <summary>AV_CODEC_FLAG2_* - encoding: Set by user. - decoding: Set by user.</summary>
        public int @flags2;
        /// <summary>some codecs need / can use extradata like Huffman tables. MJPEG: Huffman tables rv10: additional flags MPEG-4: global headers (they can be in the bitstream or here) The allocated memory should be AV_INPUT_BUFFER_PADDING_SIZE bytes larger than extradata_size to avoid problems if it is read with the bitstream reader. The bytewise contents of extradata must not depend on the architecture or CPU endianness. - encoding: Set/allocated/freed by libavcodec. - decoding: Set/allocated/freed by user.</summary>
        public byte* @extradata;
        public int @extradata_size;
        /// <summary>This is the fundamental unit of time (in seconds) in terms of which frame timestamps are represented. For fixed-fps content, timebase should be 1/framerate and timestamp increments should be identically 1. This often, but not always is the inverse of the frame rate or field rate for video. 1/time_base is not the average frame rate if the frame rate is not constant.</summary>
        public AVRational @time_base;
        /// <summary>For some codecs, the time base is closer to the field rate than the frame rate. Most notably, H.264 and MPEG-2 specify time_base as half of frame duration if no telecine is used ...</summary>
        public int @ticks_per_frame;
        /// <summary>Codec delay.</summary>
        public int @delay;
        /// <summary>picture width / height.</summary>
        public int @width;
        /// <summary>picture width / height.</summary>
        public int @height;
        /// <summary>Bitstream width / height, may be different from width/height e.g. when the decoded frame is cropped before being output or lowres is enabled.</summary>
        public int @coded_width;
        /// <summary>Bitstream width / height, may be different from width/height e.g. when the decoded frame is cropped before being output or lowres is enabled.</summary>
        public int @coded_height;
        /// <summary>the number of pictures in a group of pictures, or 0 for intra_only - encoding: Set by user. - decoding: unused</summary>
        public int @gop_size;
        /// <summary>Pixel format, see AV_PIX_FMT_xxx. May be set by the demuxer if known from headers. May be overridden by the decoder if it knows better.</summary>
        public AVPixelFormat @pix_fmt;
        /// <summary>This option does nothing</summary>
        public int @me_method;
        /// <summary>If non NULL, &apos;draw_horiz_band&apos; is called by the libavcodec decoder to draw a horizontal band. It improves cache usage. Not all codecs can do that. You must check the codec capabilities beforehand. When multithreading is used, it may be called from multiple threads at the same time; threads might draw different parts of the same AVFrame, or multiple AVFrames, and there is no guarantee that slices will be drawn in order. The function is also used by hardware acceleration APIs. It is called at least once during frame decoding to pass the data needed for hardware render. In that mode instead of pixel data, AVFrame points to a structure specific to the acceleration API. The application reads the structure and can change some fields to indicate progress or mark state. - encoding: unused - decoding: Set by user.</summary>
        public AVCodecContext_draw_horiz_band_func @draw_horiz_band;
        /// <summary>callback to negotiate the pixelFormat</summary>
        public AVCodecContext_get_format_func @get_format;
        /// <summary>maximum number of B-frames between non-B-frames Note: The output will be delayed by max_b_frames+1 relative to the input. - encoding: Set by user. - decoding: unused</summary>
        public int @max_b_frames;
        /// <summary>qscale factor between IP and B-frames If &gt; 0 then the last P-frame quantizer will be used (q= lastp_q*factor+offset). If &lt; 0 then normal ratecontrol will be done (q= -normal_q*factor+offset). - encoding: Set by user. - decoding: unused</summary>
        public float @b_quant_factor;
        public int @rc_strategy;
        public int @b_frame_strategy;
        /// <summary>qscale offset between IP and B-frames - encoding: Set by user. - decoding: unused</summary>
        public float @b_quant_offset;
        /// <summary>Size of the frame reordering buffer in the decoder. For MPEG-2 it is 1 IPB or 0 low delay IP. - encoding: Set by libavcodec. - decoding: Set by libavcodec.</summary>
        public int @has_b_frames;
        public int @mpeg_quant;
        /// <summary>qscale factor between P- and I-frames If &gt; 0 then the last P-frame quantizer will be used (q = lastp_q * factor + offset). If &lt; 0 then normal ratecontrol will be done (q= -normal_q*factor+offset). - encoding: Set by user. - decoding: unused</summary>
        public float @i_quant_factor;
        /// <summary>qscale offset between P and I-frames - encoding: Set by user. - decoding: unused</summary>
        public float @i_quant_offset;
        /// <summary>luminance masking (0-&gt; disabled) - encoding: Set by user. - decoding: unused</summary>
        public float @lumi_masking;
        /// <summary>temporary complexity masking (0-&gt; disabled) - encoding: Set by user. - decoding: unused</summary>
        public float @temporal_cplx_masking;
        /// <summary>spatial complexity masking (0-&gt; disabled) - encoding: Set by user. - decoding: unused</summary>
        public float @spatial_cplx_masking;
        /// <summary>p block masking (0-&gt; disabled) - encoding: Set by user. - decoding: unused</summary>
        public float @p_masking;
        /// <summary>darkness masking (0-&gt; disabled) - encoding: Set by user. - decoding: unused</summary>
        public float @dark_masking;
        /// <summary>slice count - encoding: Set by libavcodec. - decoding: Set by user (or 0).</summary>
        public int @slice_count;
        public int @prediction_method;
        /// <summary>slice offsets in the frame in bytes - encoding: Set/allocated by libavcodec. - decoding: Set/allocated by user (or NULL).</summary>
        public int* @slice_offset;
        /// <summary>sample aspect ratio (0 if unknown) That is the width of a pixel divided by the height of the pixel. Numerator and denominator must be relatively prime and smaller than 256 for some video standards. - encoding: Set by user. - decoding: Set by libavcodec.</summary>
        public AVRational @sample_aspect_ratio;
        /// <summary>motion estimation comparison function - encoding: Set by user. - decoding: unused</summary>
        public int @me_cmp;
        /// <summary>subpixel motion estimation comparison function - encoding: Set by user. - decoding: unused</summary>
        public int @me_sub_cmp;
        /// <summary>macroblock comparison function (not supported yet) - encoding: Set by user. - decoding: unused</summary>
        public int @mb_cmp;
        /// <summary>interlaced DCT comparison function - encoding: Set by user. - decoding: unused</summary>
        public int @ildct_cmp;
        /// <summary>ME diamond size &amp; shape - encoding: Set by user. - decoding: unused</summary>
        public int @dia_size;
        /// <summary>amount of previous MV predictors (2a+1 x 2a+1 square) - encoding: Set by user. - decoding: unused</summary>
        public int @last_predictor_count;
        public int @pre_me;
        /// <summary>motion estimation prepass comparison function - encoding: Set by user. - decoding: unused</summary>
        public int @me_pre_cmp;
        /// <summary>ME prepass diamond size &amp; shape - encoding: Set by user. - decoding: unused</summary>
        public int @pre_dia_size;
        /// <summary>subpel ME quality - encoding: Set by user. - decoding: unused</summary>
        public int @me_subpel_quality;
        /// <summary>DTG active format information (additional aspect ratio information only used in DVB MPEG-2 transport streams) 0 if not set.</summary>
        public int @dtg_active_format;
        /// <summary>maximum motion estimation search range in subpel units If 0 then no limit.</summary>
        public int @me_range;
        public int @intra_quant_bias;
        public int @inter_quant_bias;
        /// <summary>slice flags - encoding: unused - decoding: Set by user.</summary>
        public int @slice_flags;
        /// <summary>XVideo Motion Acceleration - encoding: forbidden - decoding: set by decoder</summary>
        public int @xvmc_acceleration;
        /// <summary>macroblock decision mode - encoding: Set by user. - decoding: unused</summary>
        public int @mb_decision;
        /// <summary>custom intra quantization matrix - encoding: Set by user, can be NULL. - decoding: Set by libavcodec.</summary>
        public ushort* @intra_matrix;
        /// <summary>custom inter quantization matrix - encoding: Set by user, can be NULL. - decoding: Set by libavcodec.</summary>
        public ushort* @inter_matrix;
        public int @scenechange_threshold;
        public int @noise_reduction;
        public int @me_threshold;
        public int @mb_threshold;
        /// <summary>precision of the intra DC coefficient - 8 - encoding: Set by user. - decoding: Set by libavcodec</summary>
        public int @intra_dc_precision;
        /// <summary>Number of macroblock rows at the top which are skipped. - encoding: unused - decoding: Set by user.</summary>
        public int @skip_top;
        /// <summary>Number of macroblock rows at the bottom which are skipped. - encoding: unused - decoding: Set by user.</summary>
        public int @skip_bottom;
        public float @border_masking;
        /// <summary>minimum MB Lagrange multiplier - encoding: Set by user. - decoding: unused</summary>
        public int @mb_lmin;
        /// <summary>maximum MB Lagrange multiplier - encoding: Set by user. - decoding: unused</summary>
        public int @mb_lmax;
        public int @me_penalty_compensation;
        /// <summary>- encoding: Set by user. - decoding: unused</summary>
        public int @bidir_refine;
        public int @brd_scale;
        /// <summary>minimum GOP size - encoding: Set by user. - decoding: unused</summary>
        public int @keyint_min;
        /// <summary>number of reference frames - encoding: Set by user. - decoding: Set by lavc.</summary>
        public int @refs;
        public int @chromaoffset;
        /// <summary>Multiplied by qscale for each frame and added to scene_change_score. - encoding: Set by user. - decoding: unused</summary>
        public int @scenechange_factor;
        /// <summary>Note: Value depends upon the compare function used for fullpel ME. - encoding: Set by user. - decoding: unused</summary>
        public int @mv0_threshold;
        public int @b_sensitivity;
        /// <summary>Chromaticity coordinates of the source primaries. - encoding: Set by user - decoding: Set by libavcodec</summary>
        public AVColorPrimaries @color_primaries;
        /// <summary>Color Transfer Characteristic. - encoding: Set by user - decoding: Set by libavcodec</summary>
        public AVColorTransferCharacteristic @color_trc;
        /// <summary>YUV colorspace type. - encoding: Set by user - decoding: Set by libavcodec</summary>
        public AVColorSpace @colorspace;
        /// <summary>MPEG vs JPEG YUV range. - encoding: Set by user - decoding: Set by libavcodec</summary>
        public AVColorRange @color_range;
        /// <summary>This defines the location of chroma samples. - encoding: Set by user - decoding: Set by libavcodec</summary>
        public AVChromaLocation @chroma_sample_location;
        /// <summary>Number of slices. Indicates number of picture subdivisions. Used for parallelized decoding. - encoding: Set by user - decoding: unused</summary>
        public int @slices;
        /// <summary>Field order - encoding: set by libavcodec - decoding: Set by user.</summary>
        public AVFieldOrder @field_order;
        /// <summary>samples per second</summary>
        public int @sample_rate;
        /// <summary>number of audio channels</summary>
        public int @channels;
        /// <summary>sample format</summary>
        public AVSampleFormat @sample_fmt;
        /// <summary>Number of samples per channel in an audio frame.</summary>
        public int @frame_size;
        /// <summary>Frame counter, set by libavcodec.</summary>
        public int @frame_number;
        /// <summary>number of bytes per packet if constant and known or 0 Used by some WAV based audio codecs.</summary>
        public int @block_align;
        /// <summary>Audio cutoff bandwidth (0 means &quot;automatic&quot;) - encoding: Set by user. - decoding: unused</summary>
        public int @cutoff;
        /// <summary>Audio channel layout. - encoding: set by user. - decoding: set by user, may be overwritten by libavcodec.</summary>
        public ulong @channel_layout;
        /// <summary>Request decoder to use this channel layout if it can (0 for default) - encoding: unused - decoding: Set by user.</summary>
        public ulong @request_channel_layout;
        /// <summary>Type of service that the audio stream conveys. - encoding: Set by user. - decoding: Set by libavcodec.</summary>
        public AVAudioServiceType @audio_service_type;
        /// <summary>desired sample format - encoding: Not used. - decoding: Set by user. Decoder will decode to this format if it can.</summary>
        public AVSampleFormat @request_sample_fmt;
        /// <summary>This callback is called at the beginning of each frame to get data buffer(s) for it. There may be one contiguous buffer for all the data or there may be a buffer per each data plane or anything in between. What this means is, you may set however many entries in buf[] you feel necessary. Each buffer must be reference-counted using the AVBuffer API (see description of buf[] below).</summary>
        public AVCodecContext_get_buffer2_func @get_buffer2;
        /// <summary>If non-zero, the decoded audio and video frames returned from avcodec_decode_video2() and avcodec_decode_audio4() are reference-counted and are valid indefinitely. The caller must free them with av_frame_unref() when they are not needed anymore. Otherwise, the decoded frames must not be freed by the caller and are only valid until the next decode call.</summary>
        public int @refcounted_frames;
        /// <summary>amount of qscale change between easy &amp; hard scenes (0.0-1.0)</summary>
        public float @qcompress;
        /// <summary>amount of qscale smoothing over time (0.0-1.0)</summary>
        public float @qblur;
        /// <summary>minimum quantizer - encoding: Set by user. - decoding: unused</summary>
        public int @qmin;
        /// <summary>maximum quantizer - encoding: Set by user. - decoding: unused</summary>
        public int @qmax;
        /// <summary>maximum quantizer difference between frames - encoding: Set by user. - decoding: unused</summary>
        public int @max_qdiff;
        public float @rc_qsquish;
        public float @rc_qmod_amp;
        public int @rc_qmod_freq;
        /// <summary>decoder bitstream buffer size - encoding: Set by user. - decoding: unused</summary>
        public int @rc_buffer_size;
        /// <summary>ratecontrol override, see RcOverride - encoding: Allocated/set/freed by user. - decoding: unused</summary>
        public int @rc_override_count;
        public RcOverride* @rc_override;
        public byte* @rc_eq;
        /// <summary>maximum bitrate - encoding: Set by user. - decoding: Set by user, may be overwritten by libavcodec.</summary>
        public long @rc_max_rate;
        /// <summary>minimum bitrate - encoding: Set by user. - decoding: unused</summary>
        public long @rc_min_rate;
        public float @rc_buffer_aggressivity;
        public float @rc_initial_cplx;
        /// <summary>Ratecontrol attempt to use, at maximum, &lt;value&gt; of what can be used without an underflow. - encoding: Set by user. - decoding: unused.</summary>
        public float @rc_max_available_vbv_use;
        /// <summary>Ratecontrol attempt to use, at least, &lt;value&gt; times the amount needed to prevent a vbv overflow. - encoding: Set by user. - decoding: unused.</summary>
        public float @rc_min_vbv_overflow_use;
        /// <summary>Number of bits which should be loaded into the rc buffer before decoding starts. - encoding: Set by user. - decoding: unused</summary>
        public int @rc_initial_buffer_occupancy;
        public int @coder_type;
        public int @context_model;
        public int @lmin;
        public int @lmax;
        public int @frame_skip_threshold;
        public int @frame_skip_factor;
        public int @frame_skip_exp;
        public int @frame_skip_cmp;
        /// <summary>trellis RD quantization - encoding: Set by user. - decoding: unused</summary>
        public int @trellis;
        public int @min_prediction_order;
        public int @max_prediction_order;
        public long @timecode_frame_start;
        public AVCodecContext_rtp_callback_func @rtp_callback;
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
        /// <summary>pass1 encoding statistics output buffer - encoding: Set by libavcodec. - decoding: unused</summary>
        public byte* @stats_out;
        /// <summary>pass2 encoding statistics input buffer Concatenated stuff from stats_out of pass1 should be placed here. - encoding: Allocated/set/freed by user. - decoding: unused</summary>
        public byte* @stats_in;
        /// <summary>Work around bugs in encoders which sometimes cannot be detected automatically. - encoding: Set by user - decoding: Set by user</summary>
        public int @workaround_bugs;
        /// <summary>strictly follow the standard (MPEG-4, ...). - encoding: Set by user. - decoding: Set by user. Setting this to STRICT or higher means the encoder and decoder will generally do stupid things, whereas setting it to unofficial or lower will mean the encoder might produce output that is not supported by all spec-compliant decoders. Decoders don&apos;t differentiate between normal, unofficial and experimental (that is, they always try to decode things when they can) unless they are explicitly asked to behave stupidly (=strictly conform to the specs)</summary>
        public int @strict_std_compliance;
        /// <summary>error concealment flags - encoding: unused - decoding: Set by user.</summary>
        public int @error_concealment;
        /// <summary>debug - encoding: Set by user. - decoding: Set by user.</summary>
        public int @debug;
        /// <summary>debug - encoding: Set by user. - decoding: Set by user.</summary>
        public int @debug_mv;
        /// <summary>Error recognition; may misdetect some more or less valid parts as errors. - encoding: unused - decoding: Set by user.</summary>
        public int @err_recognition;
        /// <summary>opaque 64-bit number (generally a PTS) that will be reordered and output in AVFrame.reordered_opaque - encoding: unused - decoding: Set by user.</summary>
        public long @reordered_opaque;
        /// <summary>Hardware accelerator in use - encoding: unused. - decoding: Set by libavcodec</summary>
        public AVHWAccel* @hwaccel;
        /// <summary>Hardware accelerator context. For some hardware accelerators, a global context needs to be provided by the user. In that case, this holds display-dependent data FFmpeg cannot instantiate itself. Please refer to the FFmpeg HW accelerator documentation to know how to fill this is. e.g. for VA API, this is a struct vaapi_context. - encoding: unused - decoding: Set by user</summary>
        public void* @hwaccel_context;
        /// <summary>error - encoding: Set by libavcodec if flags &amp; AV_CODEC_FLAG_PSNR. - decoding: unused</summary>
        public ulong_array8 @error;
        /// <summary>DCT algorithm, see FF_DCT_* below - encoding: Set by user. - decoding: unused</summary>
        public int @dct_algo;
        /// <summary>IDCT algorithm, see FF_IDCT_* below. - encoding: Set by user. - decoding: Set by user.</summary>
        public int @idct_algo;
        /// <summary>bits per sample/pixel from the demuxer (needed for huffyuv). - encoding: Set by libavcodec. - decoding: Set by user.</summary>
        public int @bits_per_coded_sample;
        /// <summary>Bits per sample/pixel of internal libavcodec pixel/sample format. - encoding: set by user. - decoding: set by libavcodec.</summary>
        public int @bits_per_raw_sample;
        /// <summary>low resolution decoding, 1-&gt; 1/2 size, 2-&gt;1/4 size - encoding: unused - decoding: Set by user.</summary>
        public int @lowres;
        /// <summary>the picture in the bitstream - encoding: Set by libavcodec. - decoding: unused</summary>
        public AVFrame* @coded_frame;
        /// <summary>thread count is used to decide how many independent tasks should be passed to execute() - encoding: Set by user. - decoding: Set by user.</summary>
        public int @thread_count;
        /// <summary>Which multithreading methods to use. Use of FF_THREAD_FRAME will increase decoding delay by one frame per thread, so clients which cannot provide future frames should not use it.</summary>
        public int @thread_type;
        /// <summary>Which multithreading methods are in use by the codec. - encoding: Set by libavcodec. - decoding: Set by libavcodec.</summary>
        public int @active_thread_type;
        /// <summary>Set by the client if its custom get_buffer() callback can be called synchronously from another thread, which allows faster multithreaded decoding. draw_horiz_band() will be called from other threads regardless of this setting. Ignored if the default get_buffer() is used. - encoding: Set by user. - decoding: Set by user.</summary>
        public int @thread_safe_callbacks;
        /// <summary>The codec may call this to execute several independent things. It will return only after finishing all tasks. The user may replace this with some multithreaded implementation, the default implementation will execute the parts serially.</summary>
        public AVCodecContext_execute_func @execute;
        /// <summary>The codec may call this to execute several independent things. It will return only after finishing all tasks. The user may replace this with some multithreaded implementation, the default implementation will execute the parts serially. Also see avcodec_thread_init and e.g. the --enable-pthread configure option.</summary>
        public AVCodecContext_execute2_func @execute2;
        /// <summary>noise vs. sse weight for the nsse comparison function - encoding: Set by user. - decoding: unused</summary>
        public int @nsse_weight;
        /// <summary>profile - encoding: Set by user. - decoding: Set by libavcodec.</summary>
        public int @profile;
        /// <summary>level - encoding: Set by user. - decoding: Set by libavcodec.</summary>
        public int @level;
        /// <summary>Skip loop filtering for selected frames. - encoding: unused - decoding: Set by user.</summary>
        public AVDiscard @skip_loop_filter;
        /// <summary>Skip IDCT/dequantization for selected frames. - encoding: unused - decoding: Set by user.</summary>
        public AVDiscard @skip_idct;
        /// <summary>Skip decoding for selected frames. - encoding: unused - decoding: Set by user.</summary>
        public AVDiscard @skip_frame;
        /// <summary>Header containing style information for text subtitles. For SUBTITLE_ASS subtitle type, it should contain the whole ASS [Script Info] and [V4+ Styles] section, plus the [Events] line and the Format line following. It shouldn&apos;t include any Dialogue line. - encoding: Set/allocated/freed by user (before avcodec_open2()) - decoding: Set/allocated/freed by libavcodec (by avcodec_open2())</summary>
        public byte* @subtitle_header;
        public int @subtitle_header_size;
        public int @error_rate;
        /// <summary>VBV delay coded in the last frame (in periods of a 27 MHz clock). Used for compliant TS muxing. - encoding: Set by libavcodec. - decoding: unused.</summary>
        public ulong @vbv_delay;
        /// <summary>Encoding only and set by default. Allow encoders to output packets that do not contain any encoded data, only side data.</summary>
        public int @side_data_only_packets;
        /// <summary>Audio only. The number of &quot;priming&quot; samples (padding) inserted by the encoder at the beginning of the audio. I.e. this number of leading decoded samples must be discarded by the caller to get the original audio without leading padding.</summary>
        public int @initial_padding;
        /// <summary>- decoding: For codecs that store a framerate value in the compressed bitstream, the decoder may export it here. { 0, 1} when unknown. - encoding: May be used to signal the framerate of CFR content to an encoder.</summary>
        public AVRational @framerate;
        /// <summary>Nominal unaccelerated pixel format, see AV_PIX_FMT_xxx. - encoding: unused. - decoding: Set by libavcodec before calling get_format()</summary>
        public AVPixelFormat @sw_pix_fmt;
        /// <summary>Timebase in which pkt_dts/pts and AVPacket.dts/pts are. - encoding unused. - decoding set by user.</summary>
        public AVRational @pkt_timebase;
        /// <summary>AVCodecDescriptor - encoding: unused. - decoding: set by libavcodec.</summary>
        public AVCodecDescriptor* @codec_descriptor;
        /// <summary>Current statistics for PTS correction. - decoding: maintained and used by libavcodec, not intended to be used by user apps - encoding: unused</summary>
        public long @pts_correction_num_faulty_pts;
        /// <summary>Number of incorrect PTS values so far</summary>
        public long @pts_correction_num_faulty_dts;
        /// <summary>Number of incorrect DTS values so far</summary>
        public long @pts_correction_last_pts;
        /// <summary>PTS of the last frame</summary>
        public long @pts_correction_last_dts;
        /// <summary>Character encoding of the input subtitles file. - decoding: set by user - encoding: unused</summary>
        public byte* @sub_charenc;
        /// <summary>Subtitles character encoding mode. Formats or codecs might be adjusting this setting (if they are doing the conversion themselves for instance). - decoding: set by libavcodec - encoding: unused</summary>
        public int @sub_charenc_mode;
        /// <summary>Skip processing alpha if supported by codec. Note that if the format uses pre-multiplied alpha (common with VP6, and recommended due to better video quality/compression) the image will look as if alpha-blended onto a black background. However for formats that do not use pre-multiplied alpha there might be serious artefacts (though e.g. libswscale currently assumes pre-multiplied alpha anyway).</summary>
        public int @skip_alpha;
        /// <summary>Number of samples to skip after a discontinuity - decoding: unused - encoding: set by libavcodec</summary>
        public int @seek_preroll;
        /// <summary>custom intra quantization matrix - encoding: Set by user, can be NULL. - decoding: unused.</summary>
        public ushort* @chroma_intra_matrix;
        /// <summary>dump format separator. can be &quot;, &quot; or &quot; &quot; or anything else - encoding: Set by user. - decoding: Set by user.</summary>
        public byte* @dump_separator;
        /// <summary>&apos;,&apos; separated list of allowed decoders. If NULL then all are allowed - encoding: unused - decoding: set by user</summary>
        public byte* @codec_whitelist;
        public uint @properties;
        /// <summary>Additional data associated with the entire coded stream.</summary>
        public AVPacketSideData* @coded_side_data;
        public int @nb_coded_side_data;
        /// <summary>A reference to the AVHWFramesContext describing the input (for encoding) or output (decoding) frames. The reference is set by the caller and afterwards owned (and freed) by libavcodec - it should never be read by the caller after being set.</summary>
        public AVBufferRef* @hw_frames_ctx;
        /// <summary>Control the form of AVSubtitle.rects[N]-&gt;ass - decoding: set by user - encoding: unused</summary>
        public int @sub_text_format;
        /// <summary>Audio only. The amount of padding (in samples) appended by the encoder to the end of the audio. I.e. this number of decoded samples must be discarded by the caller from the end of the stream to get the original audio without any trailing padding.</summary>
        public int @trailing_padding;
        /// <summary>The number of pixels per image to maximally accept.</summary>
        public long @max_pixels;
        /// <summary>A reference to the AVHWDeviceContext describing the device which will be used by a hardware encoder/decoder. The reference is set by the caller and afterwards owned (and freed) by libavcodec.</summary>
        public AVBufferRef* @hw_device_ctx;
        /// <summary>Bit set of AV_HWACCEL_FLAG_* flags, which affect hardware accelerated decoding (if active). - encoding: unused - decoding: Set by user (either before avcodec_open2(), or in the AVCodecContext.get_format callback)</summary>
        public int @hwaccel_flags;
    }
    
    /// <summary>AVCodec.</summary>
    public unsafe struct AVCodec
    {
        /// <summary>Name of the codec implementation. The name is globally unique among encoders and among decoders (but an encoder and a decoder can share the same name). This is the primary way to find a codec from the user perspective.</summary>
        public byte* @name;
        /// <summary>Descriptive name for the codec, meant to be more human readable than name. You should use the NULL_IF_CONFIG_SMALL() macro to define it.</summary>
        public byte* @long_name;
        public AVMediaType @type;
        public AVCodecID @id;
        /// <summary>Codec capabilities. see AV_CODEC_CAP_*</summary>
        public int @capabilities;
        /// <summary>array of supported framerates, or NULL if any, array is terminated by {0,0}</summary>
        public AVRational* @supported_framerates;
        /// <summary>array of supported pixel formats, or NULL if unknown, array is terminated by -1</summary>
        public AVPixelFormat* @pix_fmts;
        /// <summary>array of supported audio samplerates, or NULL if unknown, array is terminated by 0</summary>
        public int* @supported_samplerates;
        /// <summary>array of supported sample formats, or NULL if unknown, array is terminated by -1</summary>
        public AVSampleFormat* @sample_fmts;
        /// <summary>array of support channel layouts, or NULL if unknown. array is terminated by 0</summary>
        public ulong* @channel_layouts;
        /// <summary>maximum value for lowres supported by the decoder</summary>
        public byte @max_lowres;
        /// <summary>AVClass for the private context</summary>
        public AVClass* @priv_class;
        /// <summary>array of recognized profiles, or NULL if unknown, array is terminated by {FF_PROFILE_UNKNOWN}</summary>
        public AVProfile* @profiles;
        /// <summary>*************************************************************** No fields below this line are part of the public API. They may not be used outside of libavcodec and can be changed and removed at will. New public fields should be added right above. ****************************************************************</summary>
        public int @priv_data_size;
        public AVCodec* @next;
        /// <summary>@{</summary>
        public AVCodec_init_thread_copy_func @init_thread_copy;
        /// <summary>Copy necessary context variables from a previous thread context to the current one. If not defined, the next thread will start automatically; otherwise, the codec must call ff_thread_finish_setup().</summary>
        public AVCodec_update_thread_context_func @update_thread_context;
        /// <summary>Private codec-specific defaults.</summary>
        public AVCodecDefault* @defaults;
        /// <summary>Initialize codec static data, called from avcodec_register().</summary>
        public AVCodec_init_static_data_func @init_static_data;
        public AVCodec_init_func @init;
        public AVCodec_encode_sub_func @encode_sub;
        /// <summary>Encode data to an AVPacket.</summary>
        public AVCodec_encode2_func @encode2;
        public AVCodec_decode_func @decode;
        public AVCodec_close_func @close;
        /// <summary>Decode/encode API with decoupled packet/frame dataflow. The API is the same as the avcodec_ prefixed APIs (avcodec_send_frame() etc.), except that: - never called if the codec is closed or the wrong type, - AVPacket parameter change side data is applied right before calling AVCodec-&gt;send_packet, - if AV_CODEC_CAP_DELAY is not set, drain packets or frames are never sent, - only one drain packet is ever passed down (until the next flush()), - a drain AVPacket is always NULL (no need to check for avpkt-&gt;size).</summary>
        public AVCodec_send_frame_func @send_frame;
        public AVCodec_send_packet_func @send_packet;
        public AVCodec_receive_frame_func @receive_frame;
        public AVCodec_receive_packet_func @receive_packet;
        /// <summary>Flush buffers. Will be called when seeking</summary>
        public AVCodec_flush_func @flush;
        /// <summary>Internal codec capabilities. See FF_CODEC_CAP_* in internal.h</summary>
        public int @caps_internal;
    }
    
    public unsafe struct AVSubtitle
    {
        public ushort @format;
        public uint @start_display_time;
        public uint @end_display_time;
        public uint @num_rects;
        public AVSubtitleRect** @rects;
        /// <summary>Same as packet pts, in AV_TIME_BASE</summary>
        public long @pts;
    }
    
    public unsafe struct AVSubtitleRect
    {
        /// <summary>top left corner of pict, undefined when pict is not set</summary>
        public int @x;
        /// <summary>top left corner of pict, undefined when pict is not set</summary>
        public int @y;
        /// <summary>width of pict, undefined when pict is not set</summary>
        public int @w;
        /// <summary>height of pict, undefined when pict is not set</summary>
        public int @h;
        /// <summary>number of colors in pict, undefined when pict is not set</summary>
        public int @nb_colors;
        public AVPicture @pict;
        /// <summary>data+linesize for the bitmap of this subtitle. Can be set for text/ass as well once they are rendered.</summary>
        public byte_ptrArray4 @data;
        public int_array4 @linesize;
        public AVSubtitleType @type;
        /// <summary>0 terminated plain UTF-8 text</summary>
        public byte* @text;
        /// <summary>0 terminated ASS/SSA compatible event line. The presentation of this is unaffected by the other values in this struct.</summary>
        public byte* @ass;
        public int @flags;
    }
    
    /// <summary>Picture data structure.</summary>
    public unsafe struct AVPicture
    {
        /// <summary>pointers to the image data planes</summary>
        public byte_ptrArray8 @data;
        /// <summary>number of bytes per line</summary>
        public int_array8 @linesize;
    }
    
    /// <summary>@{</summary>
    public unsafe struct AVHWAccel
    {
        /// <summary>Name of the hardware accelerated codec. The name is globally unique among encoders and among decoders (but an encoder and a decoder can share the same name).</summary>
        public byte* @name;
        /// <summary>Type of codec implemented by the hardware accelerator.</summary>
        public AVMediaType @type;
        /// <summary>Codec implemented by the hardware accelerator.</summary>
        public AVCodecID @id;
        /// <summary>Supported pixel format.</summary>
        public AVPixelFormat @pix_fmt;
        /// <summary>Hardware accelerated codec capabilities. see HWACCEL_CODEC_CAP_*</summary>
        public int @capabilities;
        /// <summary>*************************************************************** No fields below this line are part of the public API. They may not be used outside of libavcodec and can be changed and removed at will. New public fields should be added right above. ****************************************************************</summary>
        public AVHWAccel* @next;
        /// <summary>Allocate a custom buffer</summary>
        public AVHWAccel_alloc_frame_func @alloc_frame;
        /// <summary>Called at the beginning of each frame or field picture.</summary>
        public AVHWAccel_start_frame_func @start_frame;
        /// <summary>Callback for each slice.</summary>
        public AVHWAccel_decode_slice_func @decode_slice;
        /// <summary>Called at the end of each frame or field picture.</summary>
        public AVHWAccel_end_frame_func @end_frame;
        /// <summary>Size of per-frame hardware accelerator private data.</summary>
        public int @frame_priv_data_size;
        /// <summary>Called for every Macroblock in a slice.</summary>
        public AVHWAccel_decode_mb_func @decode_mb;
        /// <summary>Initialize the hwaccel private data.</summary>
        public AVHWAccel_init_func @init;
        /// <summary>Uninitialize the hwaccel private data.</summary>
        public AVHWAccel_uninit_func @uninit;
        /// <summary>Size of the private data to allocate in AVCodecInternal.hwaccel_priv_data.</summary>
        public int @priv_data_size;
        /// <summary>Internal hwaccel capabilities.</summary>
        public int @caps_internal;
    }
    
    /// <summary>This struct describes the properties of an encoded stream.</summary>
    public unsafe struct AVCodecParameters
    {
        /// <summary>General type of the encoded data.</summary>
        public AVMediaType @codec_type;
        /// <summary>Specific type of the encoded data (the codec used).</summary>
        public AVCodecID @codec_id;
        /// <summary>Additional information about the codec (corresponds to the AVI FOURCC).</summary>
        public uint @codec_tag;
        /// <summary>Extra binary data needed for initializing the decoder, codec-dependent.</summary>
        public byte* @extradata;
        /// <summary>Size of the extradata content in bytes.</summary>
        public int @extradata_size;
        /// <summary>- video: the pixel format, the value corresponds to enum AVPixelFormat. - audio: the sample format, the value corresponds to enum AVSampleFormat.</summary>
        public int @format;
        /// <summary>The average bitrate of the encoded data (in bits per second).</summary>
        public long @bit_rate;
        /// <summary>The number of bits per sample in the codedwords.</summary>
        public int @bits_per_coded_sample;
        /// <summary>This is the number of valid bits in each output sample. If the sample format has more bits, the least significant bits are additional padding bits, which are always 0. Use right shifts to reduce the sample to its actual size. For example, audio formats with 24 bit samples will have bits_per_raw_sample set to 24, and format set to AV_SAMPLE_FMT_S32. To get the original sample use &quot;(int32_t)sample &gt;&gt; 8&quot;.&quot;</summary>
        public int @bits_per_raw_sample;
        /// <summary>Codec-specific bitstream restrictions that the stream conforms to.</summary>
        public int @profile;
        public int @level;
        /// <summary>Video only. The dimensions of the video frame in pixels.</summary>
        public int @width;
        public int @height;
        /// <summary>Video only. The aspect ratio (width / height) which a single pixel should have when displayed.</summary>
        public AVRational @sample_aspect_ratio;
        /// <summary>Video only. The order of the fields in interlaced video.</summary>
        public AVFieldOrder @field_order;
        /// <summary>Video only. Additional colorspace characteristics.</summary>
        public AVColorRange @color_range;
        public AVColorPrimaries @color_primaries;
        public AVColorTransferCharacteristic @color_trc;
        public AVColorSpace @color_space;
        public AVChromaLocation @chroma_location;
        /// <summary>Video only. Number of delayed frames.</summary>
        public int @video_delay;
        /// <summary>Audio only. The channel layout bitmask. May be 0 if the channel layout is unknown or unspecified, otherwise the number of bits set must be equal to the channels field.</summary>
        public ulong @channel_layout;
        /// <summary>Audio only. The number of audio channels.</summary>
        public int @channels;
        /// <summary>Audio only. The number of audio samples per second.</summary>
        public int @sample_rate;
        /// <summary>Audio only. The number of bytes per coded audio frame, required by some formats.</summary>
        public int @block_align;
        /// <summary>Audio only. Audio frame size, if known. Required by some formats to be static.</summary>
        public int @frame_size;
        /// <summary>Audio only. The amount of padding (in samples) inserted by the encoder at the beginning of the audio. I.e. this number of leading decoded samples must be discarded by the caller to get the original audio without leading padding.</summary>
        public int @initial_padding;
        /// <summary>Audio only. The amount of padding (in samples) appended by the encoder to the end of the audio. I.e. this number of decoded samples must be discarded by the caller from the end of the stream to get the original audio without any trailing padding.</summary>
        public int @trailing_padding;
        /// <summary>Audio only. Number of samples to skip after a discontinuity.</summary>
        public int @seek_preroll;
    }
    
    public unsafe struct AVCodecParserContext
    {
        public void* @priv_data;
        public AVCodecParser* @parser;
        public long @frame_offset;
        public long @cur_offset;
        public long @next_frame_offset;
        public int @pict_type;
        /// <summary>This field is used for proper frame duration computation in lavf. It signals, how much longer the frame duration of the current frame is compared to normal frame duration.</summary>
        public int @repeat_pict;
        public long @pts;
        public long @dts;
        public long @last_pts;
        public long @last_dts;
        public int @fetch_timestamp;
        public int @cur_frame_start_index;
        public long_array4 @cur_frame_offset;
        public long_array4 @cur_frame_pts;
        public long_array4 @cur_frame_dts;
        public int @flags;
        /// <summary>byte offset from starting packet start</summary>
        public long @offset;
        public long_array4 @cur_frame_end;
        /// <summary>Set by parser to 1 for key frames and 0 for non-key frames. It is initialized to -1, so if the parser doesn&apos;t set this flag, old-style fallback using AV_PICTURE_TYPE_I picture type as key frames will be used.</summary>
        public int @key_frame;
        public long @convergence_duration;
        /// <summary>Synchronization point for start of timestamp generation.</summary>
        public int @dts_sync_point;
        /// <summary>Offset of the current timestamp against last timestamp sync point in units of AVCodecContext.time_base.</summary>
        public int @dts_ref_dts_delta;
        /// <summary>Presentation delay of current frame in units of AVCodecContext.time_base.</summary>
        public int @pts_dts_delta;
        /// <summary>Position of the packet in file.</summary>
        public long_array4 @cur_frame_pos;
        /// <summary>Byte position of currently parsed frame in stream.</summary>
        public long @pos;
        /// <summary>Previous frame byte position.</summary>
        public long @last_pos;
        /// <summary>Duration of the current frame. For audio, this is in units of 1 / AVCodecContext.sample_rate. For all other types, this is in units of AVCodecContext.time_base.</summary>
        public int @duration;
        public AVFieldOrder @field_order;
        /// <summary>Indicate whether a picture is coded as a frame, top field or bottom field.</summary>
        public AVPictureStructure @picture_structure;
        /// <summary>Picture number incremented in presentation or output order. This field may be reinitialized at the first picture of a new sequence.</summary>
        public int @output_picture_number;
        /// <summary>Dimensions of the decoded video intended for presentation.</summary>
        public int @width;
        public int @height;
        /// <summary>Dimensions of the coded video.</summary>
        public int @coded_width;
        public int @coded_height;
        /// <summary>The format of the coded data, corresponds to enum AVPixelFormat for video and for enum AVSampleFormat for audio.</summary>
        public int @format;
    }
    
    public unsafe struct AVCodecParser
    {
        public int_array5 @codec_ids;
        public int @priv_data_size;
        public AVCodecParser_parser_init_func @parser_init;
        public AVCodecParser_parser_parse_func @parser_parse;
        public AVCodecParser_parser_close_func @parser_close;
        public AVCodecParser_split_func @split;
        public AVCodecParser* @next;
    }
    
    /// <summary>The bitstream filter state.</summary>
    public unsafe struct AVBSFContext
    {
        /// <summary>A class for logging and AVOptions</summary>
        public AVClass* @av_class;
        /// <summary>The bitstream filter this context is an instance of.</summary>
        public AVBitStreamFilter* @filter;
        /// <summary>Opaque libavcodec internal data. Must not be touched by the caller in any way.</summary>
        public AVBSFInternal* @internal;
        /// <summary>Opaque filter-specific private data. If filter-&gt;priv_class is non-NULL, this is an AVOptions-enabled struct.</summary>
        public void* @priv_data;
        /// <summary>Parameters of the input stream. This field is allocated in av_bsf_alloc(), it needs to be filled by the caller before av_bsf_init().</summary>
        public AVCodecParameters* @par_in;
        /// <summary>Parameters of the output stream. This field is allocated in av_bsf_alloc(), it is set by the filter in av_bsf_init().</summary>
        public AVCodecParameters* @par_out;
        /// <summary>The timebase used for the timestamps of the input packets. Set by the caller before av_bsf_init().</summary>
        public AVRational @time_base_in;
        /// <summary>The timebase used for the timestamps of the output packets. Set by the filter in av_bsf_init().</summary>
        public AVRational @time_base_out;
    }
    
    public unsafe struct AVBitStreamFilter
    {
        public byte* @name;
        /// <summary>A list of codec ids supported by the filter, terminated by AV_CODEC_ID_NONE. May be NULL, in that case the bitstream filter works with any codec id.</summary>
        public AVCodecID* @codec_ids;
        /// <summary>A class for the private data, used to declare bitstream filter private AVOptions. This field is NULL for bitstream filters that do not declare any options.</summary>
        public AVClass* @priv_class;
        /// <summary>*************************************************************** No fields below this line are part of the public API. They may not be used outside of libavcodec and can be changed and removed at will. New public fields should be added right above. ****************************************************************</summary>
        public int @priv_data_size;
        public AVBitStreamFilter_init_func @init;
        public AVBitStreamFilter_filter_func @filter;
        public AVBitStreamFilter_close_func @close;
    }
    
    public unsafe struct AVBitStreamFilterContext
    {
        public void* @priv_data;
        public AVBitStreamFilter* @filter;
        public AVCodecParserContext* @parser;
        public AVBitStreamFilterContext* @next;
        /// <summary>Internal default arguments, used if NULL is passed to av_bitstream_filter_filter(). Not for access by library users.</summary>
        public byte* @args;
    }
    
    /// <summary>This structure is used to provides the necessary configurations and data to the Direct3D11 FFmpeg HWAccel implementation.</summary>
    public unsafe struct AVD3D11VAContext
    {
        /// <summary>D3D11 decoder object</summary>
        public ID3D11VideoDecoder* @decoder;
        /// <summary>D3D11 VideoContext</summary>
        public ID3D11VideoContext* @video_context;
        /// <summary>D3D11 configuration used to create the decoder</summary>
        public D3D11_VIDEO_DECODER_CONFIG* @cfg;
        /// <summary>The number of surface in the surface array</summary>
        public uint @surface_count;
        /// <summary>The array of Direct3D surfaces used to create the decoder</summary>
        public ID3D11VideoDecoderOutputView** @surface;
        /// <summary>A bit field configuring the workarounds needed for using the decoder</summary>
        public ulong @workaround;
        /// <summary>Private to the FFmpeg AVHWAccel implementation</summary>
        public uint @report_id;
        /// <summary>Mutex to access video_context</summary>
        public void* @context_mutex;
    }
    
    public unsafe struct ID3D11VideoDecoder
    {
        public ID3D11VideoDecoderVtbl* @lpVtbl;
    }
    
    public unsafe struct ID3D11VideoDecoderVtbl
    {
        public void* @QueryInterface;
        public void* @AddRef;
        public void* @Release;
        public void* @GetDevice;
        public void* @GetPrivateData;
        public void* @SetPrivateData;
        public void* @SetPrivateDataInterface;
        public void* @GetCreationParameters;
        public void* @GetDriverHandle;
    }
    
    public unsafe struct ID3D11VideoContext
    {
        public ID3D11VideoContextVtbl* @lpVtbl;
    }
    
    public unsafe struct ID3D11VideoContextVtbl
    {
        public void* @QueryInterface;
        public void* @AddRef;
        public void* @Release;
        public void* @GetDevice;
        public void* @GetPrivateData;
        public void* @SetPrivateData;
        public void* @SetPrivateDataInterface;
        public void* @GetDecoderBuffer;
        public void* @ReleaseDecoderBuffer;
        public void* @DecoderBeginFrame;
        public void* @DecoderEndFrame;
        public void* @SubmitDecoderBuffers;
        public void* @DecoderExtension;
        public void* @VideoProcessorSetOutputTargetRect;
        public void* @VideoProcessorSetOutputBackgroundColor;
        public void* @VideoProcessorSetOutputColorSpace;
        public void* @VideoProcessorSetOutputAlphaFillMode;
        public void* @VideoProcessorSetOutputConstriction;
        public void* @VideoProcessorSetOutputStereoMode;
        public void* @VideoProcessorSetOutputExtension;
        public void* @VideoProcessorGetOutputTargetRect;
        public void* @VideoProcessorGetOutputBackgroundColor;
        public void* @VideoProcessorGetOutputColorSpace;
        public void* @VideoProcessorGetOutputAlphaFillMode;
        public void* @VideoProcessorGetOutputConstriction;
        public void* @VideoProcessorGetOutputStereoMode;
        public void* @VideoProcessorGetOutputExtension;
        public void* @VideoProcessorSetStreamFrameFormat;
        public void* @VideoProcessorSetStreamColorSpace;
        public void* @VideoProcessorSetStreamOutputRate;
        public void* @VideoProcessorSetStreamSourceRect;
        public void* @VideoProcessorSetStreamDestRect;
        public void* @VideoProcessorSetStreamAlpha;
        public void* @VideoProcessorSetStreamPalette;
        public void* @VideoProcessorSetStreamPixelAspectRatio;
        public void* @VideoProcessorSetStreamLumaKey;
        public void* @VideoProcessorSetStreamStereoFormat;
        public void* @VideoProcessorSetStreamAutoProcessingMode;
        public void* @VideoProcessorSetStreamFilter;
        public void* @VideoProcessorSetStreamExtension;
        public void* @VideoProcessorGetStreamFrameFormat;
        public void* @VideoProcessorGetStreamColorSpace;
        public void* @VideoProcessorGetStreamOutputRate;
        public void* @VideoProcessorGetStreamSourceRect;
        public void* @VideoProcessorGetStreamDestRect;
        public void* @VideoProcessorGetStreamAlpha;
        public void* @VideoProcessorGetStreamPalette;
        public void* @VideoProcessorGetStreamPixelAspectRatio;
        public void* @VideoProcessorGetStreamLumaKey;
        public void* @VideoProcessorGetStreamStereoFormat;
        public void* @VideoProcessorGetStreamAutoProcessingMode;
        public void* @VideoProcessorGetStreamFilter;
        public void* @VideoProcessorGetStreamExtension;
        public void* @VideoProcessorBlt;
        public void* @NegotiateCryptoSessionKeyExchange;
        public void* @EncryptionBlt;
        public void* @DecryptionBlt;
        public void* @StartSessionKeyRefresh;
        public void* @FinishSessionKeyRefresh;
        public void* @GetEncryptionBltKey;
        public void* @NegotiateAuthenticatedChannelKeyExchange;
        public void* @QueryAuthenticatedChannel;
        public void* @ConfigureAuthenticatedChannel;
        public void* @VideoProcessorSetStreamRotation;
        public void* @VideoProcessorGetStreamRotation;
    }
    
    public unsafe struct D3D11_VIDEO_DECODER_CONFIG
    {
        public _GUID @guidConfigBitstreamEncryption;
        public _GUID @guidConfigMBcontrolEncryption;
        public _GUID @guidConfigResidDiffEncryption;
        public uint @ConfigBitstreamRaw;
        public uint @ConfigMBcontrolRasterOrder;
        public uint @ConfigResidDiffHost;
        public uint @ConfigSpatialResid8;
        public uint @ConfigResid8Subtraction;
        public uint @ConfigSpatialHost8or9Clipping;
        public uint @ConfigSpatialResidInterleaved;
        public uint @ConfigIntraResidUnsigned;
        public uint @ConfigResidDiffAccelerator;
        public uint @ConfigHostInverseScan;
        public uint @ConfigSpecificIDCT;
        public uint @Config4GroupedCoefs;
        public ushort @ConfigMinRenderTargetBuffCount;
        public ushort @ConfigDecoderSpecific;
    }
    
    public unsafe struct _GUID
    {
        public ulong @Data1;
        public ushort @Data2;
        public ushort @Data3;
        public byte_array8 @Data4;
    }
    
    public unsafe struct ID3D11VideoDecoderOutputView
    {
        public ID3D11VideoDecoderOutputViewVtbl* @lpVtbl;
    }
    
    public unsafe struct ID3D11VideoDecoderOutputViewVtbl
    {
        public void* @QueryInterface;
        public void* @AddRef;
        public void* @Release;
        public void* @GetDevice;
        public void* @GetPrivateData;
        public void* @SetPrivateData;
        public void* @SetPrivateDataInterface;
        public void* @GetResource;
        public void* @GetDesc;
    }
    
    /// <summary>This structure contains the data a format has to probe a file.</summary>
    public unsafe struct AVProbeData
    {
        public byte* @filename;
        /// <summary>Buffer must have AVPROBE_PADDING_SIZE of extra allocated bytes filled with zero.</summary>
        public byte* @buf;
        /// <summary>Size of buf except extra allocated bytes</summary>
        public int @buf_size;
        /// <summary>mime_type, when known.</summary>
        public byte* @mime_type;
    }
    
    public unsafe struct AVIndexEntry
    {
        public long @pos;
        /// <summary>Timestamp in AVStream.time_base units, preferably the time from which on correctly decoded frames are available when seeking to this entry. That means preferable PTS on keyframe based formats. But demuxers can choose to store a different timestamp, if it is more convenient for the implementation or nothing better is known</summary>
        public long @timestamp;
        /// <summary>Flag is used to indicate which frame should be discarded after decoding.</summary>
        public int @flags2_size30;
        /// <summary>Minimum distance between this and the previous keyframe, used to avoid unneeded searching.</summary>
        public int @min_distance;
    }
    
    /// <summary>Stream structure. New fields can be added to the end with minor version bumps. Removal, reordering and changes to existing fields require a major version bump. sizeof(AVStream) must not be used outside libav*.</summary>
    public unsafe struct AVStream
    {
        /// <summary>stream index in AVFormatContext</summary>
        public int @index;
        /// <summary>Format-specific stream ID. decoding: set by libavformat encoding: set by the user, replaced by libavformat if left unset</summary>
        public int @id;
        public AVCodecContext* @codec;
        public void* @priv_data;
        public AVFrac @pts;
        /// <summary>This is the fundamental unit of time (in seconds) in terms of which frame timestamps are represented.</summary>
        public AVRational @time_base;
        /// <summary>Decoding: pts of the first frame of the stream in presentation order, in stream time base. Only set this if you are absolutely 100% sure that the value you set it to really is the pts of the first frame. This may be undefined (AV_NOPTS_VALUE).</summary>
        public long @start_time;
        /// <summary>Decoding: duration of the stream, in stream time base. If a source file does not specify a duration, but does specify a bitrate, this value will be estimated from bitrate and file size.</summary>
        public long @duration;
        /// <summary>number of frames in this stream if known or 0</summary>
        public long @nb_frames;
        /// <summary>AV_DISPOSITION_* bit field</summary>
        public int @disposition;
        /// <summary>Selects which packets can be discarded at will and do not need to be demuxed.</summary>
        public AVDiscard @discard;
        /// <summary>sample aspect ratio (0 if unknown) - encoding: Set by user. - decoding: Set by libavformat.</summary>
        public AVRational @sample_aspect_ratio;
        public AVDictionary* @metadata;
        /// <summary>Average framerate</summary>
        public AVRational @avg_frame_rate;
        /// <summary>For streams with AV_DISPOSITION_ATTACHED_PIC disposition, this packet will contain the attached picture.</summary>
        public AVPacket @attached_pic;
        /// <summary>An array of side data that applies to the whole stream (i.e. the container does not allow it to change between packets).</summary>
        public AVPacketSideData* @side_data;
        /// <summary>The number of elements in the AVStream.side_data array.</summary>
        public int @nb_side_data;
        /// <summary>Flags for the user to detect events happening on the stream. Flags must be cleared by the user once the event has been handled. A combination of AVSTREAM_EVENT_FLAG_*.</summary>
        public int @event_flags;
        public AVStream_info* @info;
        /// <summary>number of bits in pts (used for wrapping control)</summary>
        public int @pts_wrap_bits;
        /// <summary>Timestamp corresponding to the last dts sync point.</summary>
        public long @first_dts;
        public long @cur_dts;
        public long @last_IP_pts;
        public int @last_IP_duration;
        /// <summary>Number of packets to buffer for codec probing</summary>
        public int @probe_packets;
        /// <summary>Number of frames that have been demuxed during avformat_find_stream_info()</summary>
        public int @codec_info_nb_frames;
        public AVStreamParseType @need_parsing;
        public AVCodecParserContext* @parser;
        /// <summary>last packet in packet_buffer for this stream when muxing.</summary>
        public AVPacketList* @last_in_packet_buffer;
        public AVProbeData @probe_data;
        public long_array17 @pts_buffer;
        /// <summary>Only used if the format does not support seeking natively.</summary>
        public AVIndexEntry* @index_entries;
        public int @nb_index_entries;
        public uint @index_entries_allocated_size;
        /// <summary>Real base framerate of the stream. This is the lowest framerate with which all timestamps can be represented accurately (it is the least common multiple of all framerates in the stream). Note, this value is just a guess! For example, if the time base is 1/90000 and all frames have either approximately 3600 or 1800 timer ticks, then r_frame_rate will be 50/1.</summary>
        public AVRational @r_frame_rate;
        /// <summary>Stream Identifier This is the MPEG-TS stream identifier +1 0 means unknown</summary>
        public int @stream_identifier;
        public long @interleaver_chunk_size;
        public long @interleaver_chunk_duration;
        /// <summary>stream probing state -1 -&gt; probing finished 0 -&gt; no probing requested rest -&gt; perform probing with request_probe being the minimum score to accept. NOT PART OF PUBLIC API</summary>
        public int @request_probe;
        /// <summary>Indicates that everything up to the next keyframe should be discarded.</summary>
        public int @skip_to_keyframe;
        /// <summary>Number of samples to skip at the start of the frame decoded from the next packet.</summary>
        public int @skip_samples;
        /// <summary>If not 0, the number of samples that should be skipped from the start of the stream (the samples are removed from packets with pts==0, which also assumes negative timestamps do not happen). Intended for use with formats such as mp3 with ad-hoc gapless audio support.</summary>
        public long @start_skip_samples;
        /// <summary>If not 0, the first audio sample that should be discarded from the stream. This is broken by design (needs global sample count), but can&apos;t be avoided for broken by design formats such as mp3 with ad-hoc gapless audio support.</summary>
        public long @first_discard_sample;
        /// <summary>The sample after last sample that is intended to be discarded after first_discard_sample. Works on frame boundaries only. Used to prevent early EOF if the gapless info is broken (considered concatenated mp3s).</summary>
        public long @last_discard_sample;
        /// <summary>Number of internally decoded frames, used internally in libavformat, do not access its lifetime differs from info which is why it is not in that structure.</summary>
        public int @nb_decoded_frames;
        /// <summary>Timestamp offset added to timestamps before muxing NOT PART OF PUBLIC API</summary>
        public long @mux_ts_offset;
        /// <summary>Internal data to check for wrapping of the time stamp</summary>
        public long @pts_wrap_reference;
        /// <summary>Options for behavior, when a wrap is detected.</summary>
        public int @pts_wrap_behavior;
        /// <summary>Internal data to prevent doing update_initial_durations() twice</summary>
        public int @update_initial_durations_done;
        /// <summary>Internal data to generate dts from pts</summary>
        public long_array17 @pts_reorder_error;
        public byte_array17 @pts_reorder_error_count;
        /// <summary>Internal data to analyze DTS and detect faulty mpeg streams</summary>
        public long @last_dts_for_order_check;
        public byte @dts_ordered;
        public byte @dts_misordered;
        /// <summary>Internal data to inject global side data</summary>
        public int @inject_global_side_data;
        /// <summary>String containing paris of key and values describing recommended encoder configuration. Paris are separated by &apos;,&apos;. Keys are separated from values by &apos;=&apos;.</summary>
        public byte* @recommended_encoder_configuration;
        /// <summary>display aspect ratio (0 if unknown) - encoding: unused - decoding: Set by libavformat to calculate sample_aspect_ratio internally</summary>
        public AVRational @display_aspect_ratio;
        public FFFrac* @priv_pts;
        /// <summary>An opaque field for libavformat internal usage. Must not be accessed in any way by callers.</summary>
        public AVStreamInternal* @internal;
        public AVCodecParameters* @codecpar;
    }
    
    /// <summary>The exact value of the fractional number is: &apos;val + num / den&apos;. num is assumed to be 0 &lt;= num &lt; den.</summary>
    public unsafe struct AVFrac
    {
        public long @val;
        public long @num;
        public long @den;
    }
    
    public unsafe struct AVStream_info
    {
        public long @last_dts;
        public long @duration_gcd;
        public int @duration_count;
        public long @rfps_duration_sum;
        public double_arrayOfArray798* @duration_error;
        public long @codec_info_duration;
        public long @codec_info_duration_fields;
        /// <summary>0 -&gt; decoder has not been searched for yet. &gt;0 -&gt; decoder found &lt;0 -&gt; decoder with codec_id == -found_decoder has not been found</summary>
        public int @found_decoder;
        public long @last_duration;
        /// <summary>Those are used for average framerate estimation.</summary>
        public long @fps_first_dts;
        public int @fps_first_dts_idx;
        public long @fps_last_dts;
        public int @fps_last_dts_idx;
    }
    
    public unsafe struct AVPacketList
    {
        public AVPacket @pkt;
        public AVPacketList* @next;
    }
    
    /// <summary>New fields can be added to the end with minor version bumps. Removal, reordering and changes to existing fields require a major version bump. sizeof(AVProgram) must not be used outside libav*.</summary>
    public unsafe struct AVProgram
    {
        public int @id;
        public int @flags;
        /// <summary>selects which program to discard and which to feed to the caller</summary>
        public AVDiscard @discard;
        public uint* @stream_index;
        public uint @nb_stream_indexes;
        public AVDictionary* @metadata;
        public int @program_num;
        public int @pmt_pid;
        public int @pcr_pid;
        /// <summary>*************************************************************** All fields below this line are not part of the public API. They may not be used outside of libavformat and can be changed and removed at will. New public fields should be added right above. ****************************************************************</summary>
        public long @start_time;
        public long @end_time;
        /// <summary>reference dts for wrap detection</summary>
        public long @pts_wrap_reference;
        /// <summary>behavior on wrap detection</summary>
        public int @pts_wrap_behavior;
    }
    
    public unsafe struct AVChapter
    {
        /// <summary>unique ID to identify the chapter</summary>
        public int @id;
        /// <summary>time base in which the start/end timestamps are specified</summary>
        public AVRational @time_base;
        /// <summary>chapter start/end time in time_base units</summary>
        public long @start;
        /// <summary>chapter start/end time in time_base units</summary>
        public long @end;
        public AVDictionary* @metadata;
    }
    
    /// <summary>@{</summary>
    public unsafe struct AVOutputFormat
    {
        public byte* @name;
        /// <summary>Descriptive name for the format, meant to be more human-readable than name. You should use the NULL_IF_CONFIG_SMALL() macro to define it.</summary>
        public byte* @long_name;
        public byte* @mime_type;
        /// <summary>comma-separated filename extensions</summary>
        public byte* @extensions;
        /// <summary>default audio codec</summary>
        public AVCodecID @audio_codec;
        /// <summary>default video codec</summary>
        public AVCodecID @video_codec;
        /// <summary>default subtitle codec</summary>
        public AVCodecID @subtitle_codec;
        /// <summary>can use flags: AVFMT_NOFILE, AVFMT_NEEDNUMBER, AVFMT_GLOBALHEADER, AVFMT_NOTIMESTAMPS, AVFMT_VARIABLE_FPS, AVFMT_NODIMENSIONS, AVFMT_NOSTREAMS, AVFMT_ALLOW_FLUSH, AVFMT_TS_NONSTRICT, AVFMT_TS_NEGATIVE</summary>
        public int @flags;
        /// <summary>List of supported codec_id-codec_tag pairs, ordered by &quot;better choice first&quot;. The arrays are all terminated by AV_CODEC_ID_NONE.</summary>
        public AVCodecTag** @codec_tag;
        /// <summary>AVClass for the private context</summary>
        public AVClass* @priv_class;
        /// <summary>*************************************************************** No fields below this line are part of the public API. They may not be used outside of libavformat and can be changed and removed at will. New public fields should be added right above. ****************************************************************</summary>
        public AVOutputFormat* @next;
        /// <summary>size of private data so that it can be allocated in the wrapper</summary>
        public int @priv_data_size;
        public AVOutputFormat_write_header_func @write_header;
        /// <summary>Write a packet. If AVFMT_ALLOW_FLUSH is set in flags, pkt can be NULL in order to flush data buffered in the muxer. When flushing, return 0 if there still is more data to flush, or 1 if everything was flushed and there is no more buffered data.</summary>
        public AVOutputFormat_write_packet_func @write_packet;
        public AVOutputFormat_write_trailer_func @write_trailer;
        /// <summary>Currently only used to set pixel format if not YUV420P.</summary>
        public AVOutputFormat_interleave_packet_func @interleave_packet;
        /// <summary>Test if the given codec can be stored in this container.</summary>
        public AVOutputFormat_query_codec_func @query_codec;
        public AVOutputFormat_get_output_timestamp_func @get_output_timestamp;
        /// <summary>Allows sending messages from application to device.</summary>
        public AVOutputFormat_control_message_func @control_message;
        /// <summary>Write an uncoded AVFrame.</summary>
        public AVOutputFormat_write_uncoded_frame_func @write_uncoded_frame;
        /// <summary>Returns device list with it properties.</summary>
        public AVOutputFormat_get_device_list_func @get_device_list;
        /// <summary>Initialize device capabilities submodule.</summary>
        public AVOutputFormat_create_device_capabilities_func @create_device_capabilities;
        /// <summary>Free device capabilities submodule.</summary>
        public AVOutputFormat_free_device_capabilities_func @free_device_capabilities;
        /// <summary>default data codec</summary>
        public AVCodecID @data_codec;
        /// <summary>Initialize format. May allocate data here, and set any AVFormatContext or AVStream parameters that need to be set before packets are sent. This method must not write output.</summary>
        public AVOutputFormat_init_func @init;
        /// <summary>Deinitialize format. If present, this is called whenever the muxer is being destroyed, regardless of whether or not the header has been written.</summary>
        public AVOutputFormat_deinit_func @deinit;
        /// <summary>Set up any necessary bitstream filtering and extract any extra data needed for the global header. Return 0 if more packets from this stream must be checked; 1 if not.</summary>
        public AVOutputFormat_check_bitstream_func @check_bitstream;
    }
    
    /// <summary>Format I/O context. New fields can be added to the end with minor version bumps. Removal, reordering and changes to existing fields require a major version bump. sizeof(AVFormatContext) must not be used outside libav*, use avformat_alloc_context() to create an AVFormatContext.</summary>
    public unsafe struct AVFormatContext
    {
        /// <summary>A class for logging and Exports (de)muxer private options if they exist.</summary>
        public AVClass* @av_class;
        /// <summary>The input container format.</summary>
        public AVInputFormat* @iformat;
        /// <summary>The output container format.</summary>
        public AVOutputFormat* @oformat;
        /// <summary>Format private data. This is an AVOptions-enabled struct if and only if iformat/oformat.priv_class is not NULL.</summary>
        public void* @priv_data;
        /// <summary>I/O context.</summary>
        public AVIOContext* @pb;
        /// <summary>Flags signalling stream properties. A combination of AVFMTCTX_*. Set by libavformat.</summary>
        public int @ctx_flags;
        /// <summary>Number of elements in AVFormatContext.streams.</summary>
        public uint @nb_streams;
        /// <summary>A list of all streams in the file. New streams are created with avformat_new_stream().</summary>
        public AVStream** @streams;
        /// <summary>input or output filename</summary>
        public byte_array1024 @filename;
        /// <summary>Position of the first frame of the component, in AV_TIME_BASE fractional seconds. NEVER set this value directly: It is deduced from the AVStream values.</summary>
        public long @start_time;
        /// <summary>Duration of the stream, in AV_TIME_BASE fractional seconds. Only set this value if you know none of the individual stream durations and also do not set any of them. This is deduced from the AVStream values if not set.</summary>
        public long @duration;
        /// <summary>Total stream bitrate in bit/s, 0 if not available. Never set it directly if the file_size and the duration are known as FFmpeg can compute it automatically.</summary>
        public long @bit_rate;
        public uint @packet_size;
        public int @max_delay;
        /// <summary>Flags modifying the (de)muxer behaviour. A combination of AVFMT_FLAG_*. Set by the user before avformat_open_input() / avformat_write_header().</summary>
        public int @flags;
        /// <summary>Maximum size of the data read from input for determining the input container format. Demuxing only, set by the caller before avformat_open_input().</summary>
        public long @probesize;
        /// <summary>Maximum duration (in AV_TIME_BASE units) of the data read from input in avformat_find_stream_info(). Demuxing only, set by the caller before avformat_find_stream_info(). Can be set to 0 to let avformat choose using a heuristic.</summary>
        public long @max_analyze_duration;
        public byte* @key;
        public int @keylen;
        public uint @nb_programs;
        public AVProgram** @programs;
        /// <summary>Forced video codec_id. Demuxing: Set by user.</summary>
        public AVCodecID @video_codec_id;
        /// <summary>Forced audio codec_id. Demuxing: Set by user.</summary>
        public AVCodecID @audio_codec_id;
        /// <summary>Forced subtitle codec_id. Demuxing: Set by user.</summary>
        public AVCodecID @subtitle_codec_id;
        /// <summary>Maximum amount of memory in bytes to use for the index of each stream. If the index exceeds this size, entries will be discarded as needed to maintain a smaller size. This can lead to slower or less accurate seeking (depends on demuxer). Demuxers for which a full in-memory index is mandatory will ignore this. - muxing: unused - demuxing: set by user</summary>
        public uint @max_index_size;
        /// <summary>Maximum amount of memory in bytes to use for buffering frames obtained from realtime capture devices.</summary>
        public uint @max_picture_buffer;
        /// <summary>Number of chapters in AVChapter array. When muxing, chapters are normally written in the file header, so nb_chapters should normally be initialized before write_header is called. Some muxers (e.g. mov and mkv) can also write chapters in the trailer. To write chapters in the trailer, nb_chapters must be zero when write_header is called and non-zero when write_trailer is called. - muxing: set by user - demuxing: set by libavformat</summary>
        public uint @nb_chapters;
        public AVChapter** @chapters;
        /// <summary>Metadata that applies to the whole file.</summary>
        public AVDictionary* @metadata;
        /// <summary>Start time of the stream in real world time, in microseconds since the Unix epoch (00:00 1st January 1970). That is, pts=0 in the stream was captured at this real world time. - muxing: Set by the caller before avformat_write_header(). If set to either 0 or AV_NOPTS_VALUE, then the current wall-time will be used. - demuxing: Set by libavformat. AV_NOPTS_VALUE if unknown. Note that the value may become known after some number of frames have been received.</summary>
        public long @start_time_realtime;
        /// <summary>The number of frames used for determining the framerate in avformat_find_stream_info(). Demuxing only, set by the caller before avformat_find_stream_info().</summary>
        public int @fps_probe_size;
        /// <summary>Error recognition; higher values will detect more errors but may misdetect some more or less valid parts as errors. Demuxing only, set by the caller before avformat_open_input().</summary>
        public int @error_recognition;
        /// <summary>Custom interrupt callbacks for the I/O layer.</summary>
        public AVIOInterruptCB @interrupt_callback;
        /// <summary>Flags to enable debugging.</summary>
        public int @debug;
        /// <summary>Maximum buffering duration for interleaving.</summary>
        public long @max_interleave_delta;
        /// <summary>Allow non-standard and experimental extension</summary>
        public int @strict_std_compliance;
        /// <summary>Flags for the user to detect events happening on the file. Flags must be cleared by the user once the event has been handled. A combination of AVFMT_EVENT_FLAG_*.</summary>
        public int @event_flags;
        /// <summary>Maximum number of packets to read while waiting for the first timestamp. Decoding only.</summary>
        public int @max_ts_probe;
        /// <summary>Avoid negative timestamps during muxing. Any value of the AVFMT_AVOID_NEG_TS_* constants. Note, this only works when using av_interleaved_write_frame. (interleave_packet_per_dts is in use) - muxing: Set by user - demuxing: unused</summary>
        public int @avoid_negative_ts;
        /// <summary>Transport stream id. This will be moved into demuxer private options. Thus no API/ABI compatibility</summary>
        public int @ts_id;
        /// <summary>Audio preload in microseconds. Note, not all formats support this and unpredictable things may happen if it is used when not supported. - encoding: Set by user - decoding: unused</summary>
        public int @audio_preload;
        /// <summary>Max chunk time in microseconds. Note, not all formats support this and unpredictable things may happen if it is used when not supported. - encoding: Set by user - decoding: unused</summary>
        public int @max_chunk_duration;
        /// <summary>Max chunk size in bytes Note, not all formats support this and unpredictable things may happen if it is used when not supported. - encoding: Set by user - decoding: unused</summary>
        public int @max_chunk_size;
        /// <summary>forces the use of wallclock timestamps as pts/dts of packets This has undefined results in the presence of B frames. - encoding: unused - decoding: Set by user</summary>
        public int @use_wallclock_as_timestamps;
        /// <summary>avio flags, used to force AVIO_FLAG_DIRECT. - encoding: unused - decoding: Set by user</summary>
        public int @avio_flags;
        /// <summary>The duration field can be estimated through various ways, and this field can be used to know how the duration was estimated. - encoding: unused - decoding: Read by user</summary>
        public AVDurationEstimationMethod @duration_estimation_method;
        /// <summary>Skip initial bytes when opening stream - encoding: unused - decoding: Set by user</summary>
        public long @skip_initial_bytes;
        /// <summary>Correct single timestamp overflows - encoding: unused - decoding: Set by user</summary>
        public uint @correct_ts_overflow;
        /// <summary>Force seeking to any (also non key) frames. - encoding: unused - decoding: Set by user</summary>
        public int @seek2any;
        /// <summary>Flush the I/O context after each packet. - encoding: Set by user - decoding: unused</summary>
        public int @flush_packets;
        /// <summary>format probing score. The maximal score is AVPROBE_SCORE_MAX, its set when the demuxer probes the format. - encoding: unused - decoding: set by avformat, read by user</summary>
        public int @probe_score;
        /// <summary>number of bytes to read maximally to identify format. - encoding: unused - decoding: set by user</summary>
        public int @format_probesize;
        /// <summary>&apos;,&apos; separated list of allowed decoders. If NULL then all are allowed - encoding: unused - decoding: set by user</summary>
        public byte* @codec_whitelist;
        /// <summary>&apos;,&apos; separated list of allowed demuxers. If NULL then all are allowed - encoding: unused - decoding: set by user</summary>
        public byte* @format_whitelist;
        /// <summary>An opaque field for libavformat internal usage. Must not be accessed in any way by callers.</summary>
        public AVFormatInternal* @internal;
        /// <summary>IO repositioned flag. This is set by avformat when the underlaying IO context read pointer is repositioned, for example when doing byte based seeking. Demuxers can use the flag to detect such changes.</summary>
        public int @io_repositioned;
        /// <summary>Forced video codec. This allows forcing a specific decoder, even when there are multiple with the same codec_id. Demuxing: Set by user</summary>
        public AVCodec* @video_codec;
        /// <summary>Forced audio codec. This allows forcing a specific decoder, even when there are multiple with the same codec_id. Demuxing: Set by user</summary>
        public AVCodec* @audio_codec;
        /// <summary>Forced subtitle codec. This allows forcing a specific decoder, even when there are multiple with the same codec_id. Demuxing: Set by user</summary>
        public AVCodec* @subtitle_codec;
        /// <summary>Forced data codec. This allows forcing a specific decoder, even when there are multiple with the same codec_id. Demuxing: Set by user</summary>
        public AVCodec* @data_codec;
        /// <summary>Number of bytes to be written as padding in a metadata header. Demuxing: Unused. Muxing: Set by user via av_format_set_metadata_header_padding.</summary>
        public int @metadata_header_padding;
        /// <summary>User data. This is a place for some private data of the user.</summary>
        public void* @opaque;
        /// <summary>Callback used by devices to communicate with application.</summary>
        public AVFormatContext_control_message_cb_func @control_message_cb;
        /// <summary>Output timestamp offset, in microseconds. Muxing: set by user</summary>
        public long @output_ts_offset;
        /// <summary>dump format separator. can be &quot;, &quot; or &quot; &quot; or anything else - muxing: Set by user. - demuxing: Set by user.</summary>
        public byte* @dump_separator;
        /// <summary>Forced Data codec_id. Demuxing: Set by user.</summary>
        public AVCodecID @data_codec_id;
        /// <summary>Called to open further IO contexts when needed for demuxing.</summary>
        public AVFormatContext_open_cb_func @open_cb;
        /// <summary>&apos;,&apos; separated list of allowed protocols. - encoding: unused - decoding: set by user</summary>
        public byte* @protocol_whitelist;
        public AVFormatContext_io_open_func @io_open;
        /// <summary>A callback for closing the streams opened with AVFormatContext.io_open().</summary>
        public AVFormatContext_io_close_func @io_close;
        /// <summary>&apos;,&apos; separated list of disallowed protocols. - encoding: unused - decoding: set by user</summary>
        public byte* @protocol_blacklist;
        /// <summary>The maximum number of streams. - encoding: unused - decoding: set by user</summary>
        public int @max_streams;
    }
    
    /// <summary>@{</summary>
    public unsafe struct AVInputFormat
    {
        /// <summary>A comma separated list of short names for the format. New names may be appended with a minor bump.</summary>
        public byte* @name;
        /// <summary>Descriptive name for the format, meant to be more human-readable than name. You should use the NULL_IF_CONFIG_SMALL() macro to define it.</summary>
        public byte* @long_name;
        /// <summary>Can use flags: AVFMT_NOFILE, AVFMT_NEEDNUMBER, AVFMT_SHOW_IDS, AVFMT_GENERIC_INDEX, AVFMT_TS_DISCONT, AVFMT_NOBINSEARCH, AVFMT_NOGENSEARCH, AVFMT_NO_BYTE_SEEK, AVFMT_SEEK_TO_PTS.</summary>
        public int @flags;
        /// <summary>If extensions are defined, then no probe is done. You should usually not use extension format guessing because it is not reliable enough</summary>
        public byte* @extensions;
        public AVCodecTag** @codec_tag;
        /// <summary>AVClass for the private context</summary>
        public AVClass* @priv_class;
        /// <summary>Comma-separated list of mime types. It is used check for matching mime types while probing.</summary>
        public byte* @mime_type;
        /// <summary>*************************************************************** No fields below this line are part of the public API. They may not be used outside of libavformat and can be changed and removed at will. New public fields should be added right above. ****************************************************************</summary>
        public AVInputFormat* @next;
        /// <summary>Raw demuxers store their codec ID here.</summary>
        public int @raw_codec_id;
        /// <summary>Size of private data so that it can be allocated in the wrapper.</summary>
        public int @priv_data_size;
        /// <summary>Tell if a given file has a chance of being parsed as this format. The buffer provided is guaranteed to be AVPROBE_PADDING_SIZE bytes big so you do not have to check for that unless you need more.</summary>
        public AVInputFormat_read_probe_func @read_probe;
        /// <summary>Read the format header and initialize the AVFormatContext structure. Return 0 if OK. &apos;avformat_new_stream&apos; should be called to create new streams.</summary>
        public AVInputFormat_read_header_func @read_header;
        /// <summary>Read one packet and put it in &apos;pkt&apos;. pts and flags are also set. &apos;avformat_new_stream&apos; can be called only if the flag AVFMTCTX_NOHEADER is used and only in the calling thread (not in a background thread).</summary>
        public AVInputFormat_read_packet_func @read_packet;
        /// <summary>Close the stream. The AVFormatContext and AVStreams are not freed by this function</summary>
        public AVInputFormat_read_close_func @read_close;
        /// <summary>Seek to a given timestamp relative to the frames in stream component stream_index.</summary>
        public AVInputFormat_read_seek_func @read_seek;
        /// <summary>Get the next timestamp in stream[stream_index].time_base units.</summary>
        public AVInputFormat_read_timestamp_func @read_timestamp;
        /// <summary>Start/resume playing - only meaningful if using a network-based format (RTSP).</summary>
        public AVInputFormat_read_play_func @read_play;
        /// <summary>Pause playing - only meaningful if using a network-based format (RTSP).</summary>
        public AVInputFormat_read_pause_func @read_pause;
        /// <summary>Seek to timestamp ts. Seeking will be done so that the point from which all active streams can be presented successfully will be closest to ts and within min/max_ts. Active streams are all streams that have AVStream.discard &lt; AVDISCARD_ALL.</summary>
        public AVInputFormat_read_seek2_func @read_seek2;
        /// <summary>Returns device list with it properties.</summary>
        public AVInputFormat_get_device_list_func @get_device_list;
        /// <summary>Initialize device capabilities submodule.</summary>
        public AVInputFormat_create_device_capabilities_func @create_device_capabilities;
        /// <summary>Free device capabilities submodule.</summary>
        public AVInputFormat_free_device_capabilities_func @free_device_capabilities;
    }
    
    /// <summary>List of devices.</summary>
    public unsafe struct AVDeviceInfoList
    {
        /// <summary>list of autodetected devices</summary>
        public AVDeviceInfo** @devices;
        /// <summary>number of autodetected devices</summary>
        public int @nb_devices;
        /// <summary>index of default device or -1 if no default</summary>
        public int @default_device;
    }
    
    /// <summary>Structure describes device capabilities.</summary>
    public unsafe struct AVDeviceCapabilitiesQuery
    {
        public AVClass* @av_class;
        public AVFormatContext* @device_context;
        public AVCodecID @codec;
        public AVSampleFormat @sample_format;
        public AVPixelFormat @pixel_format;
        public int @sample_rate;
        public int @channels;
        public long @channel_layout;
        public int @window_width;
        public int @window_height;
        public int @frame_width;
        public int @frame_height;
        public AVRational @fps;
    }
    
    /// <summary>Bytestream IO Context. New fields can be added to the end with minor version bumps. Removal, reordering and changes to existing fields require a major version bump. sizeof(AVIOContext) must not be used outside libav*.</summary>
    public unsafe struct AVIOContext
    {
        /// <summary>A class for private options.</summary>
        public AVClass* @av_class;
        /// <summary>Start of the buffer.</summary>
        public byte* @buffer;
        /// <summary>Maximum buffer size</summary>
        public int @buffer_size;
        /// <summary>Current position in the buffer</summary>
        public byte* @buf_ptr;
        /// <summary>End of the data, may be less than buffer+buffer_size if the read function returned less data than requested, e.g. for streams where no more data has been received yet.</summary>
        public byte* @buf_end;
        /// <summary>A private pointer, passed to the read/write/seek/... functions.</summary>
        public void* @opaque;
        public AVIOContext_read_packet_func @read_packet;
        public AVIOContext_write_packet_func @write_packet;
        public AVIOContext_seek_func @seek;
        /// <summary>position in the file of the current buffer</summary>
        public long @pos;
        /// <summary>true if the next seek should flush</summary>
        public int @must_flush;
        /// <summary>true if eof reached</summary>
        public int @eof_reached;
        /// <summary>true if open for writing</summary>
        public int @write_flag;
        public int @max_packet_size;
        public ulong @checksum;
        public byte* @checksum_ptr;
        public AVIOContext_update_checksum_func @update_checksum;
        /// <summary>contains the error code or 0 if no error happened</summary>
        public int @error;
        /// <summary>Pause or resume playback for network streaming protocols - e.g. MMS.</summary>
        public AVIOContext_read_pause_func @read_pause;
        /// <summary>Seek to a given timestamp in stream with the specified stream_index. Needed for some network streaming protocols which don&apos;t support seeking to byte position.</summary>
        public AVIOContext_read_seek_func @read_seek;
        /// <summary>A combination of AVIO_SEEKABLE_ flags or 0 when the stream is not seekable.</summary>
        public int @seekable;
        /// <summary>max filesize, used to limit allocations This field is internal to libavformat and access from outside is not allowed.</summary>
        public long @maxsize;
        /// <summary>avio_read and avio_write should if possible be satisfied directly instead of going through a buffer, and avio_seek will always call the underlying seek function directly.</summary>
        public int @direct;
        /// <summary>Bytes read statistic This field is internal to libavformat and access from outside is not allowed.</summary>
        public long @bytes_read;
        /// <summary>seek statistic This field is internal to libavformat and access from outside is not allowed.</summary>
        public int @seek_count;
        /// <summary>writeout statistic This field is internal to libavformat and access from outside is not allowed.</summary>
        public int @writeout_count;
        /// <summary>Original buffer size used internally after probing and ensure seekback to reset the buffer size This field is internal to libavformat and access from outside is not allowed.</summary>
        public int @orig_buffer_size;
        /// <summary>Threshold to favor readahead over seek. This is current internal only, do not use from outside.</summary>
        public int @short_seek_threshold;
        /// <summary>&apos;,&apos; separated list of allowed protocols.</summary>
        public byte* @protocol_whitelist;
        /// <summary>&apos;,&apos; separated list of disallowed protocols.</summary>
        public byte* @protocol_blacklist;
        /// <summary>A callback that is used instead of write_packet.</summary>
        public AVIOContext_write_data_type_func @write_data_type;
        /// <summary>If set, don&apos;t call write_data_type separately for AVIO_DATA_MARKER_BOUNDARY_POINT, but ignore them and treat them as AVIO_DATA_MARKER_UNKNOWN (to avoid needlessly small chunks of data returned from the callback).</summary>
        public int @ignore_boundary_point;
        /// <summary>Internal, not meant to be used from outside of AVIOContext.</summary>
        public AVIODataMarkerType @current_type;
        public long @last_time;
        /// <summary>A callback that is used instead of short_seek_threshold. This is current internal only, do not use from outside.</summary>
        public AVIOContext_short_seek_get_func @short_seek_get;
    }
    
    /// <summary>Callback for checking whether to abort blocking functions. AVERROR_EXIT is returned in this case by the interrupted function. During blocking operations, callback is called with opaque as parameter. If the callback returns 1, the blocking operation will be aborted.</summary>
    public unsafe struct AVIOInterruptCB
    {
        public AVIOInterruptCB_callback_func @callback;
        public void* @opaque;
    }
    
    /// <summary>Describes single entry of the directory.</summary>
    public unsafe struct AVIODirEntry
    {
        /// <summary>Filename</summary>
        public byte* @name;
        /// <summary>Type of the entry</summary>
        public int @type;
        /// <summary>Set to 1 when name is encoded with UTF-8, 0 otherwise. Name can be encoded with UTF-8 even though 0 is set.</summary>
        public int @utf8;
        /// <summary>File size in bytes, -1 if unknown.</summary>
        public long @size;
        /// <summary>Time of last modification in microseconds since unix epoch, -1 if unknown.</summary>
        public long @modification_timestamp;
        /// <summary>Time of last access in microseconds since unix epoch, -1 if unknown.</summary>
        public long @access_timestamp;
        /// <summary>Time of last status change in microseconds since unix epoch, -1 if unknown.</summary>
        public long @status_change_timestamp;
        /// <summary>User ID of owner, -1 if unknown.</summary>
        public long @user_id;
        /// <summary>Group ID of owner, -1 if unknown.</summary>
        public long @group_id;
        /// <summary>Unix file mode, -1 if unknown.</summary>
        public long @filemode;
    }
    
    public unsafe struct AVIODirContext
    {
        public URLContext* @url_context;
    }
    
    /// <summary>An instance of a filter</summary>
    public unsafe struct AVFilterContext
    {
        /// <summary>needed for av_log() and filters common options</summary>
        public AVClass* @av_class;
        /// <summary>the AVFilter of which this is an instance</summary>
        public AVFilter* @filter;
        /// <summary>name of this filter instance</summary>
        public byte* @name;
        /// <summary>array of input pads</summary>
        public AVFilterPad* @input_pads;
        /// <summary>array of pointers to input links</summary>
        public AVFilterLink** @inputs;
        /// <summary>number of input pads</summary>
        public uint @nb_inputs;
        /// <summary>array of output pads</summary>
        public AVFilterPad* @output_pads;
        /// <summary>array of pointers to output links</summary>
        public AVFilterLink** @outputs;
        /// <summary>number of output pads</summary>
        public uint @nb_outputs;
        /// <summary>private data for use by the filter</summary>
        public void* @priv;
        /// <summary>filtergraph this filter belongs to</summary>
        public AVFilterGraph* @graph;
        /// <summary>Type of multithreading being allowed/used. A combination of AVFILTER_THREAD_* flags.</summary>
        public int @thread_type;
        /// <summary>An opaque struct for libavfilter internal use.</summary>
        public AVFilterInternal* @internal;
        public AVFilterCommand* @command_queue;
        /// <summary>enable expression string</summary>
        public byte* @enable_str;
        /// <summary>parsed expression (AVExpr*)</summary>
        public void* @enable;
        /// <summary>variable values for the enable expression</summary>
        public double* @var_values;
        /// <summary>the enabled state from the last expression evaluation</summary>
        public int @is_disabled;
        /// <summary>For filters which will create hardware frames, sets the device the filter should create them in. All other filters will ignore this field: in particular, a filter which consumes or processes hardware frames will instead use the hw_frames_ctx field in AVFilterLink to carry the hardware context information.</summary>
        public AVBufferRef* @hw_device_ctx;
        /// <summary>Max number of threads allowed in this filter instance. If &lt;= 0, its value is ignored. Overrides global number of threads set per filter graph.</summary>
        public int @nb_threads;
        /// <summary>Ready status of the filter. A non-0 value means that the filter needs activating; a higher value suggests a more urgent activation.</summary>
        public uint @ready;
    }
    
    /// <summary>Filter definition. This defines the pads a filter contains, and all the callback functions used to interact with the filter.</summary>
    public unsafe struct AVFilter
    {
        /// <summary>Filter name. Must be non-NULL and unique among filters.</summary>
        public byte* @name;
        /// <summary>A description of the filter. May be NULL.</summary>
        public byte* @description;
        /// <summary>List of inputs, terminated by a zeroed element.</summary>
        public AVFilterPad* @inputs;
        /// <summary>List of outputs, terminated by a zeroed element.</summary>
        public AVFilterPad* @outputs;
        /// <summary>A class for the private data, used to declare filter private AVOptions. This field is NULL for filters that do not declare any options.</summary>
        public AVClass* @priv_class;
        /// <summary>A combination of AVFILTER_FLAG_*</summary>
        public int @flags;
        /// <summary>Filter initialization function.</summary>
        public AVFilter_init_func @init;
        /// <summary>Should be set instead of want to pass a dictionary of AVOptions to nested contexts that are allocated during init.</summary>
        public AVFilter_init_dict_func @init_dict;
        /// <summary>Filter uninitialization function.</summary>
        public AVFilter_uninit_func @uninit;
        /// <summary>Query formats supported by the filter on its inputs and outputs.</summary>
        public AVFilter_query_formats_func @query_formats;
        /// <summary>size of private data to allocate for the filter</summary>
        public int @priv_size;
        /// <summary>Additional flags for avfilter internal use only.</summary>
        public int @flags_internal;
        /// <summary>Used by the filter registration system. Must not be touched by any other code.</summary>
        public AVFilter* @next;
        /// <summary>Make the filter instance process a command.</summary>
        public AVFilter_process_command_func @process_command;
        /// <summary>Filter initialization function, alternative to the init() callback. Args contains the user-supplied parameters, opaque is used for providing binary data.</summary>
        public AVFilter_init_opaque_func @init_opaque;
        /// <summary>Filter activation function.</summary>
        public AVFilter_activate_func @activate;
    }
    
    /// <summary>A link between two filters. This contains pointers to the source and destination filters between which this link exists, and the indexes of the pads involved. In addition, this link also contains the parameters which have been negotiated and agreed upon between the filter, such as image dimensions, format, etc.</summary>
    public unsafe struct AVFilterLink
    {
        /// <summary>source filter</summary>
        public AVFilterContext* @src;
        /// <summary>output pad on the source filter</summary>
        public AVFilterPad* @srcpad;
        /// <summary>dest filter</summary>
        public AVFilterContext* @dst;
        /// <summary>input pad on the dest filter</summary>
        public AVFilterPad* @dstpad;
        /// <summary>filter media type</summary>
        public AVMediaType @type;
        /// <summary>agreed upon image width</summary>
        public int @w;
        /// <summary>agreed upon image height</summary>
        public int @h;
        /// <summary>agreed upon sample aspect ratio</summary>
        public AVRational @sample_aspect_ratio;
        /// <summary>channel layout of current buffer (see libavutil/channel_layout.h)</summary>
        public ulong @channel_layout;
        /// <summary>samples per second</summary>
        public int @sample_rate;
        /// <summary>agreed upon media format</summary>
        public int @format;
        /// <summary>Define the time base used by the PTS of the frames/samples which will pass through this link. During the configuration stage, each filter is supposed to change only the output timebase, while the timebase of the input link is assumed to be an unchangeable property.</summary>
        public AVRational @time_base;
        /// <summary>*************************************************************** All fields below this line are not part of the public API. They may not be used outside of libavfilter and can be changed and removed at will. New public fields should be added right above. ****************************************************************</summary>
        public AVFilterFormats* @in_formats;
        public AVFilterFormats* @out_formats;
        /// <summary>Lists of channel layouts and sample rates used for automatic negotiation.</summary>
        public AVFilterFormats* @in_samplerates;
        public AVFilterFormats* @out_samplerates;
        public AVFilterChannelLayouts* @in_channel_layouts;
        public AVFilterChannelLayouts* @out_channel_layouts;
        /// <summary>Audio only, the destination filter sets this to a non-zero value to request that buffers with the given number of samples should be sent to it. AVFilterPad.needs_fifo must also be set on the corresponding input pad. Last buffer before EOF will be padded with silence.</summary>
        public int @request_samples;
        public AVFilterLink_init_state @init_state;
        /// <summary>Graph the filter belongs to.</summary>
        public AVFilterGraph* @graph;
        /// <summary>Current timestamp of the link, as defined by the most recent frame(s), in link time_base units.</summary>
        public long @current_pts;
        /// <summary>Current timestamp of the link, as defined by the most recent frame(s), in AV_TIME_BASE units.</summary>
        public long @current_pts_us;
        /// <summary>Index in the age array.</summary>
        public int @age_index;
        /// <summary>Frame rate of the stream on the link, or 1/0 if unknown or variable; if left to 0/0, will be automatically copied from the first input of the source filter if it exists.</summary>
        public AVRational @frame_rate;
        /// <summary>Buffer partially filled with samples to achieve a fixed/minimum size.</summary>
        public AVFrame* @partial_buf;
        /// <summary>Size of the partial buffer to allocate. Must be between min_samples and max_samples.</summary>
        public int @partial_buf_size;
        /// <summary>Minimum number of samples to filter at once. If filter_frame() is called with fewer samples, it will accumulate them in partial_buf. This field and the related ones must not be changed after filtering has started. If 0, all related fields are ignored.</summary>
        public int @min_samples;
        /// <summary>Maximum number of samples to filter at once. If filter_frame() is called with more samples, it will split them.</summary>
        public int @max_samples;
        /// <summary>Number of channels.</summary>
        public int @channels;
        /// <summary>Link processing flags.</summary>
        public uint @flags;
        /// <summary>Number of past frames sent through the link.</summary>
        public long @frame_count_in;
        /// <summary>Number of past frames sent through the link.</summary>
        public long @frame_count_out;
        /// <summary>A pointer to a FFFramePool struct.</summary>
        public void* @frame_pool;
        /// <summary>True if a frame is currently wanted on the output of this filter. Set when ff_request_frame() is called by the output, cleared when a frame is filtered.</summary>
        public int @frame_wanted_out;
        /// <summary>For hwaccel pixel formats, this should be a reference to the AVHWFramesContext describing the frames.</summary>
        public AVBufferRef* @hw_frames_ctx;
        /// <summary>Internal structure members. The fields below this limit are internal for libavfilter&apos;s use and must in no way be accessed by applications.</summary>
        public byte_array61440 @reserved;
    }
    
    public unsafe struct AVFilterGraph
    {
        public AVClass* @av_class;
        public AVFilterContext** @filters;
        public uint @nb_filters;
        /// <summary>sws options to use for the auto-inserted scale filters</summary>
        public byte* @scale_sws_opts;
        /// <summary>libavresample options to use for the auto-inserted resample filters</summary>
        public byte* @resample_lavr_opts;
        /// <summary>Type of multithreading allowed for filters in this graph. A combination of AVFILTER_THREAD_* flags.</summary>
        public int @thread_type;
        /// <summary>Maximum number of threads used by filters in this graph. May be set by the caller before adding any filters to the filtergraph. Zero (the default) means that the number of threads is determined automatically.</summary>
        public int @nb_threads;
        /// <summary>Opaque object for libavfilter internal use.</summary>
        public AVFilterGraphInternal* @internal;
        /// <summary>Opaque user data. May be set by the caller to an arbitrary value, e.g. to be used from callbacks like Libavfilter will not touch this field in any way.</summary>
        public void* @opaque;
        /// <summary>This callback may be set by the caller immediately after allocating the graph and before adding any filters to it, to provide a custom multithreading implementation.</summary>
        public AVFilterGraph_execute_func @execute;
        /// <summary>swr options to use for the auto-inserted aresample filters, Access ONLY through AVOptions</summary>
        public byte* @aresample_swr_opts;
        /// <summary>Private fields</summary>
        public AVFilterLink** @sink_links;
        public int @sink_links_count;
        public uint @disable_auto_convert;
    }
    
    /// <summary>A linked-list of the inputs/outputs of the filter chain.</summary>
    public unsafe struct AVFilterInOut
    {
        /// <summary>unique name for this input/output in the list</summary>
        public byte* @name;
        /// <summary>filter context associated to this input/output</summary>
        public AVFilterContext* @filter_ctx;
        /// <summary>index of the filt_ctx pad to use for linking</summary>
        public int @pad_idx;
        /// <summary>next input/input in the list, NULL if this is the last</summary>
        public AVFilterInOut* @next;
    }
    
    /// <summary>This structure contains the parameters describing the frames that will be passed to this filter.</summary>
    public unsafe struct AVBufferSrcParameters
    {
        /// <summary>video: the pixel format, value corresponds to enum AVPixelFormat audio: the sample format, value corresponds to enum AVSampleFormat</summary>
        public int @format;
        /// <summary>The timebase to be used for the timestamps on the input frames.</summary>
        public AVRational @time_base;
        /// <summary>Video only, the display dimensions of the input frames.</summary>
        public int @width;
        /// <summary>Video only, the display dimensions of the input frames.</summary>
        public int @height;
        /// <summary>Video only, the sample (pixel) aspect ratio.</summary>
        public AVRational @sample_aspect_ratio;
        /// <summary>Video only, the frame rate of the input video. This field must only be set to a non-zero value if input stream has a known constant framerate and should be left at its initial value if the framerate is variable or unknown.</summary>
        public AVRational @frame_rate;
        /// <summary>Video with a hwaccel pixel format only. This should be a reference to an AVHWFramesContext instance describing the input frames.</summary>
        public AVBufferRef* @hw_frames_ctx;
        /// <summary>Audio only, the audio sampling rate in samples per secon.</summary>
        public int @sample_rate;
        /// <summary>Audio only, the audio channel layout</summary>
        public ulong @channel_layout;
    }
    
    /// <summary>Struct to use for initializing a buffersink context.</summary>
    public unsafe struct AVBufferSinkParams
    {
        /// <summary>list of allowed pixel formats, terminated by AV_PIX_FMT_NONE</summary>
        public AVPixelFormat* @pixel_fmts;
    }
    
    /// <summary>Struct to use for initializing an abuffersink context.</summary>
    public unsafe struct AVABufferSinkParams
    {
        /// <summary>list of allowed sample formats, terminated by AV_SAMPLE_FMT_NONE</summary>
        public AVSampleFormat* @sample_fmts;
        /// <summary>list of allowed channel layouts, terminated by -1</summary>
        public long* @channel_layouts;
        /// <summary>list of allowed channel counts, terminated by -1</summary>
        public int* @channel_counts;
        /// <summary>if not 0, accept any channel count or layout</summary>
        public int @all_channel_counts;
        /// <summary>list of allowed sample rates, terminated by -1</summary>
        public int* @sample_rates;
    }
    
    /// <summary>Structure describes basic parameters of the device.</summary>
    public unsafe struct AVDeviceInfo
    {
        /// <summary>device name, format depends on device</summary>
        public byte* @device_name;
        /// <summary>human friendly name</summary>
        public byte* @device_description;
    }
    
    public unsafe struct AVDeviceRect
    {
        /// <summary>x coordinate of top left corner</summary>
        public int @x;
        /// <summary>y coordinate of top left corner</summary>
        public int @y;
        /// <summary>width</summary>
        public int @width;
        /// <summary>height</summary>
        public int @height;
    }
    
}
