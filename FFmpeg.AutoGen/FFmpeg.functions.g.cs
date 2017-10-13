using System;
using System.Runtime.InteropServices;

namespace FFmpeg.AutoGen
{
    public unsafe static partial class ffmpeg
    {
        [Obsolete("")]
        [DllImport("avcodec-57", EntryPoint = "audio_resample", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern int audio_resample(ReSampleContext* @s, short* @output, short* @input, int @nb_samples);
        
        /// <summary>Free resample context.</summary>
        /// <param name="s">a non-NULL pointer to a resample context previously created with av_audio_resample_init()</param>
        [Obsolete("")]
        [DllImport("avcodec-57", EntryPoint = "audio_resample_close", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern void audio_resample_close(ReSampleContext* @s);
        
        /// <summary>Initialize audio resampling context.</summary>
        /// <param name="output_channels">number of output channels</param>
        /// <param name="input_channels">number of input channels</param>
        /// <param name="output_rate">output sample rate</param>
        /// <param name="input_rate">input sample rate</param>
        /// <param name="sample_fmt_out">requested output sample format</param>
        /// <param name="sample_fmt_in">input sample format</param>
        /// <param name="filter_length">length of each FIR filter in the filterbank relative to the cutoff frequency</param>
        /// <param name="log2_phase_count">log2 of the number of entries in the polyphase filterbank</param>
        /// <param name="linear">if 1 then the used FIR filter will be linearly interpolated between the 2 closest, if 0 the closest will be used</param>
        /// <param name="cutoff">cutoff frequency, 1.0 corresponds to half the output sampling rate</param>
        [Obsolete("")]
        [DllImport("avcodec-57", EntryPoint = "av_audio_resample_init", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern ReSampleContext* av_audio_resample_init(int @output_channels, int @input_channels, int @output_rate, int @input_rate, AVSampleFormat @sample_fmt_out, AVSampleFormat @sample_fmt_in, int @filter_length, int @log2_phase_count, int @linear, double @cutoff);
        
        /// <summary>Release bitstream filter context.</summary>
        /// <param name="bsf">the bitstream filter context created with av_bitstream_filter_init(), can be NULL</param>
        [Obsolete("")]
        [DllImport("avcodec-57", EntryPoint = "av_bitstream_filter_close", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern void av_bitstream_filter_close(AVBitStreamFilterContext* @bsf);
        
        /// <summary>Filter bitstream.</summary>
        /// <param name="bsfc">bitstream filter context created by av_bitstream_filter_init()</param>
        /// <param name="avctx">AVCodecContext accessed by the filter, may be NULL. If specified, this must point to the encoder context of the output stream the packet is sent to.</param>
        /// <param name="args">arguments which specify the filter configuration, may be NULL</param>
        /// <param name="poutbuf">pointer which is updated to point to the filtered buffer</param>
        /// <param name="poutbuf_size">pointer which is updated to the filtered buffer size in bytes</param>
        /// <param name="buf">buffer containing the data to filter</param>
        /// <param name="buf_size">size in bytes of buf</param>
        /// <param name="keyframe">set to non-zero if the buffer to filter corresponds to a key-frame packet data</param>
        [Obsolete("")]
        [DllImport("avcodec-57", EntryPoint = "av_bitstream_filter_filter", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern int av_bitstream_filter_filter(AVBitStreamFilterContext* @bsfc, AVCodecContext* @avctx, [MarshalAs(UnmanagedType.LPStr)] string @args, byte** @poutbuf, int* @poutbuf_size, byte* @buf, int @buf_size, int @keyframe);
        
        /// <summary>Create and initialize a bitstream filter context given a bitstream filter name.</summary>
        /// <param name="name">the name of the bitstream filter</param>
        [Obsolete("")]
        [DllImport("avcodec-57", EntryPoint = "av_bitstream_filter_init", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern AVBitStreamFilterContext* av_bitstream_filter_init([MarshalAs(UnmanagedType.LPStr)] string @name);
        
        /// <summary>If f is NULL, return the first registered bitstream filter, if f is non-NULL, return the next registered bitstream filter after f, or NULL if f is the last one.</summary>
        [Obsolete("")]
        [DllImport("avcodec-57", EntryPoint = "av_bitstream_filter_next", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern AVBitStreamFilter* av_bitstream_filter_next(AVBitStreamFilter* @f);
        
        /// <summary>Allocate a context for a given bitstream filter. The caller must fill in the context parameters as described in the documentation and then call av_bsf_init() before sending any data to the filter.</summary>
        /// <param name="filter">the filter for which to allocate an instance.</param>
        /// <param name="ctx">a pointer into which the pointer to the newly-allocated context will be written. It must be freed with av_bsf_free() after the filtering is done.</param>
        [DllImport("avcodec-57", EntryPoint = "av_bsf_alloc", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern int av_bsf_alloc(AVBitStreamFilter* @filter, AVBSFContext** @ctx);
        
        /// <summary>Free a bitstream filter context and everything associated with it; write NULL into the supplied pointer.</summary>
        [DllImport("avcodec-57", EntryPoint = "av_bsf_free", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern void av_bsf_free(AVBSFContext** @ctx);
        
        /// <summary>Returns a bitstream filter with the specified name or NULL if no such bitstream filter exists.</summary>
        [DllImport("avcodec-57", EntryPoint = "av_bsf_get_by_name", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern AVBitStreamFilter* av_bsf_get_by_name([MarshalAs(UnmanagedType.LPStr)] string @name);
        
        /// <summary>Get the AVClass for AVBSFContext. It can be used in combination with AV_OPT_SEARCH_FAKE_OBJ for examining options.</summary>
        [DllImport("avcodec-57", EntryPoint = "av_bsf_get_class", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern AVClass* av_bsf_get_class();
        
        /// <summary>Get null/pass-through bitstream filter.</summary>
        /// <param name="bsf">Pointer to be set to new instance of pass-through bitstream filter</param>
        [DllImport("avcodec-57", EntryPoint = "av_bsf_get_null_filter", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern int av_bsf_get_null_filter(AVBSFContext** @bsf);
        
        /// <summary>Prepare the filter for use, after all the parameters and options have been set.</summary>
        [DllImport("avcodec-57", EntryPoint = "av_bsf_init", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern int av_bsf_init(AVBSFContext* @ctx);
        
        /// <summary>Allocate empty list of bitstream filters. The list must be later freed by av_bsf_list_free() or finalized by av_bsf_list_finalize().</summary>
        [DllImport("avcodec-57", EntryPoint = "av_bsf_list_alloc", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern AVBSFList* av_bsf_list_alloc();
        
        /// <summary>Append bitstream filter to the list of bitstream filters.</summary>
        /// <param name="lst">List to append to</param>
        /// <param name="bsf">Filter context to be appended</param>
        [DllImport("avcodec-57", EntryPoint = "av_bsf_list_append", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern int av_bsf_list_append(AVBSFList* @lst, AVBSFContext* @bsf);
        
        /// <summary>Construct new bitstream filter context given it&apos;s name and options and append it to the list of bitstream filters.</summary>
        /// <param name="lst">List to append to</param>
        /// <param name="bsf_name">Name of the bitstream filter</param>
        /// <param name="options">Options for the bitstream filter, can be set to NULL</param>
        [DllImport("avcodec-57", EntryPoint = "av_bsf_list_append2", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern int av_bsf_list_append2(AVBSFList* @lst, [MarshalAs(UnmanagedType.LPStr)] string @bsf_name, AVDictionary** @options);
        
        /// <summary>Finalize list of bitstream filters.</summary>
        /// <param name="lst">Filter list structure to be transformed</param>
        /// <param name="bsf">Pointer to be set to newly created</param>
        [DllImport("avcodec-57", EntryPoint = "av_bsf_list_finalize", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern int av_bsf_list_finalize(AVBSFList** @lst, AVBSFContext** @bsf);
        
        /// <summary>Free list of bitstream filters.</summary>
        /// <param name="lst">Pointer to pointer returned by av_bsf_list_alloc()</param>
        [DllImport("avcodec-57", EntryPoint = "av_bsf_list_free", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern void av_bsf_list_free(AVBSFList** @lst);
        
        /// <summary>Parse string describing list of bitstream filters and create single Resulting allocated by av_bsf_alloc().</summary>
        /// <param name="str">String describing chain of bitstream filters in format `bsf1[=opt1=val1:opt2=val2][,bsf2]`</param>
        /// <param name="bsf">Pointer to be set to newly created</param>
        [DllImport("avcodec-57", EntryPoint = "av_bsf_list_parse_str", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern int av_bsf_list_parse_str([MarshalAs(UnmanagedType.LPStr)] string @str, AVBSFContext** @bsf);
        
        /// <summary>Iterate over all registered bitstream filters.</summary>
        /// <param name="opaque">a pointer where libavcodec will store the iteration state. Must point to NULL to start the iteration.</param>
        [DllImport("avcodec-57", EntryPoint = "av_bsf_next", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern AVBitStreamFilter* av_bsf_next(void** @opaque);
        
        /// <summary>Retrieve a filtered packet.</summary>
        /// <param name="pkt">this struct will be filled with the contents of the filtered packet. It is owned by the caller and must be freed using av_packet_unref() when it is no longer needed. This parameter should be &quot;clean&quot; (i.e. freshly allocated with av_packet_alloc() or unreffed with av_packet_unref()) when this function is called. If this function returns successfully, the contents of pkt will be completely overwritten by the returned data. On failure, pkt is not touched.</param>
        [DllImport("avcodec-57", EntryPoint = "av_bsf_receive_packet", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern int av_bsf_receive_packet(AVBSFContext* @ctx, AVPacket* @pkt);
        
        /// <summary>Submit a packet for filtering.</summary>
        /// <param name="pkt">the packet to filter. pkt must contain some payload (i.e data or side data must be present in pkt). The bitstream filter will take ownership of the packet and reset the contents of pkt. pkt is not touched if an error occurs. This parameter may be NULL, which signals the end of the stream (i.e. no more packets will be sent). That will cause the filter to output any packets it may have buffered internally.</param>
        [DllImport("avcodec-57", EntryPoint = "av_bsf_send_packet", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern int av_bsf_send_packet(AVBSFContext* @ctx, AVPacket* @pkt);
        
        [DllImport("avcodec-57", EntryPoint = "av_codec_get_chroma_intra_matrix", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern ushort* av_codec_get_chroma_intra_matrix(AVCodecContext* @avctx);
        
        [DllImport("avcodec-57", EntryPoint = "av_codec_get_codec_descriptor", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern AVCodecDescriptor* av_codec_get_codec_descriptor(AVCodecContext* @avctx);
        
        [DllImport("avcodec-57", EntryPoint = "av_codec_get_codec_properties", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern uint av_codec_get_codec_properties(AVCodecContext* @avctx);
        
        [DllImport("avcodec-57", EntryPoint = "av_codec_get_lowres", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern int av_codec_get_lowres(AVCodecContext* @avctx);
        
        [DllImport("avcodec-57", EntryPoint = "av_codec_get_max_lowres", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern int av_codec_get_max_lowres(AVCodec* @codec);
        
        [DllImport("avcodec-57", EntryPoint = "av_codec_get_pkt_timebase", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern AVRational av_codec_get_pkt_timebase(AVCodecContext* @avctx);
        
        [DllImport("avcodec-57", EntryPoint = "av_codec_get_seek_preroll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern int av_codec_get_seek_preroll(AVCodecContext* @avctx);
        
        /// <summary>Returns a non-zero number if codec is a decoder, zero otherwise</summary>
        [DllImport("avcodec-57", EntryPoint = "av_codec_is_decoder", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern int av_codec_is_decoder(AVCodec* @codec);
        
        /// <summary>Returns a non-zero number if codec is an encoder, zero otherwise</summary>
        [DllImport("avcodec-57", EntryPoint = "av_codec_is_encoder", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern int av_codec_is_encoder(AVCodec* @codec);
        
        /// <summary>If c is NULL, returns the first registered codec, if c is non-NULL, returns the next registered codec after c, or NULL if c is the last one.</summary>
        [DllImport("avcodec-57", EntryPoint = "av_codec_next", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern AVCodec* av_codec_next(AVCodec* @c);
        
        [DllImport("avcodec-57", EntryPoint = "av_codec_set_chroma_intra_matrix", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern void av_codec_set_chroma_intra_matrix(AVCodecContext* @avctx, ushort* @val);
        
        [DllImport("avcodec-57", EntryPoint = "av_codec_set_codec_descriptor", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern void av_codec_set_codec_descriptor(AVCodecContext* @avctx, AVCodecDescriptor* @desc);
        
        [DllImport("avcodec-57", EntryPoint = "av_codec_set_lowres", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern void av_codec_set_lowres(AVCodecContext* @avctx, int @val);
        
        [DllImport("avcodec-57", EntryPoint = "av_codec_set_pkt_timebase", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern void av_codec_set_pkt_timebase(AVCodecContext* @avctx, AVRational @val);
        
        [DllImport("avcodec-57", EntryPoint = "av_codec_set_seek_preroll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern void av_codec_set_seek_preroll(AVCodecContext* @avctx, int @val);
        
        /// <summary>Copy packet, including contents</summary>
        [DllImport("avcodec-57", EntryPoint = "av_copy_packet", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern int av_copy_packet(AVPacket* @dst, AVPacket* @src);
        
        /// <summary>Copy packet side data</summary>
        [DllImport("avcodec-57", EntryPoint = "av_copy_packet_side_data", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern int av_copy_packet_side_data(AVPacket* @dst, AVPacket* @src);
        
        /// <summary>Allocate a CPB properties structure and initialize its fields to default values.</summary>
        /// <param name="size">if non-NULL, the size of the allocated struct will be written here. This is useful for embedding it in side data.</param>
        [DllImport("avcodec-57", EntryPoint = "av_cpb_properties_alloc", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern AVCPBProperties* av_cpb_properties_alloc(ulong* @size);
        
        /// <summary>Allocate an AVD3D11VAContext.</summary>
        [DllImport("avcodec-57", EntryPoint = "av_d3d11va_alloc_context", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern AVD3D11VAContext* av_d3d11va_alloc_context();
        
        [Obsolete("Use av_packet_ref")]
        [DllImport("avcodec-57", EntryPoint = "av_dup_packet", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern int av_dup_packet(AVPacket* @pkt);
        
        /// <summary>Same behaviour av_fast_malloc but the buffer has additional AV_INPUT_BUFFER_PADDING_SIZE at the end which will always be 0.</summary>
        [DllImport("avcodec-57", EntryPoint = "av_fast_padded_malloc", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern void av_fast_padded_malloc(void* @ptr, uint* @size, ulong @min_size);
        
        /// <summary>Same behaviour av_fast_padded_malloc except that buffer will always be 0-initialized after call.</summary>
        [DllImport("avcodec-57", EntryPoint = "av_fast_padded_mallocz", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern void av_fast_padded_mallocz(void* @ptr, uint* @size, ulong @min_size);
        
        /// <summary>Free a packet.</summary>
        /// <param name="pkt">packet to free</param>
        [Obsolete("Use av_packet_unref")]
        [DllImport("avcodec-57", EntryPoint = "av_free_packet", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern void av_free_packet(AVPacket* @pkt);
        
        /// <summary>Return audio frame duration.</summary>
        /// <param name="avctx">codec context</param>
        /// <param name="frame_bytes">size of the frame, or 0 if unknown</param>
        [DllImport("avcodec-57", EntryPoint = "av_get_audio_frame_duration", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern int av_get_audio_frame_duration(AVCodecContext* @avctx, int @frame_bytes);
        
        /// <summary>This function is the same as av_get_audio_frame_duration(), except it works with AVCodecParameters instead of an AVCodecContext.</summary>
        [DllImport("avcodec-57", EntryPoint = "av_get_audio_frame_duration2", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern int av_get_audio_frame_duration2(AVCodecParameters* @par, int @frame_bytes);
        
        /// <summary>Return codec bits per sample.</summary>
        /// <param name="codec_id">the codec</param>
        [DllImport("avcodec-57", EntryPoint = "av_get_bits_per_sample", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern int av_get_bits_per_sample(AVCodecID @codec_id);
        
        /// <summary>Put a string representing the codec tag codec_tag in buf.</summary>
        /// <param name="buf">buffer to place codec tag in</param>
        /// <param name="buf_size">size in bytes of buf</param>
        /// <param name="codec_tag">codec tag to assign</param>
        [Obsolete("see av_fourcc_make_string() and av_fourcc2str().")]
        [DllImport("avcodec-57", EntryPoint = "av_get_codec_tag_string", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern ulong av_get_codec_tag_string(byte* @buf, ulong @buf_size, uint @codec_tag);
        
        /// <summary>Return codec bits per sample. Only return non-zero if the bits per sample is exactly correct, not an approximation.</summary>
        /// <param name="codec_id">the codec</param>
        [DllImport("avcodec-57", EntryPoint = "av_get_exact_bits_per_sample", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern int av_get_exact_bits_per_sample(AVCodecID @codec_id);
        
        /// <summary>Return the PCM codec associated with a sample format.</summary>
        /// <param name="be">endianness, 0 for little, 1 for big, -1 (or anything else) for native</param>
        [DllImport("avcodec-57", EntryPoint = "av_get_pcm_codec", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern AVCodecID av_get_pcm_codec(AVSampleFormat @fmt, int @be);
        
        /// <summary>Return a name for the specified profile, if available.</summary>
        /// <param name="codec">the codec that is searched for the given profile</param>
        /// <param name="profile">the profile value for which a name is requested</param>
        [DllImport("avcodec-57", EntryPoint = "av_get_profile_name", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        [return: MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof(ConstCharPtrMarshaler))]
        public static extern string av_get_profile_name(AVCodec* @codec, int @profile);
        
        /// <summary>Increase packet size, correctly zeroing padding</summary>
        /// <param name="pkt">packet</param>
        /// <param name="grow_by">number of bytes by which to increase the size of the packet</param>
        [DllImport("avcodec-57", EntryPoint = "av_grow_packet", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern int av_grow_packet(AVPacket* @pkt, int @grow_by);
        
        /// <summary>If hwaccel is NULL, returns the first registered hardware accelerator, if hwaccel is non-NULL, returns the next registered hardware accelerator after hwaccel, or NULL if hwaccel is the last one.</summary>
        [DllImport("avcodec-57", EntryPoint = "av_hwaccel_next", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern AVHWAccel* av_hwaccel_next(AVHWAccel* @hwaccel);
        
        /// <summary>Initialize optional fields of a packet with default values.</summary>
        /// <param name="pkt">packet</param>
        [DllImport("avcodec-57", EntryPoint = "av_init_packet", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern void av_init_packet(AVPacket* @pkt);
        
        /// <summary>Register a user provided lock manager supporting the operations specified by AVLockOp. The &quot;mutex&quot; argument to the function points to a (void *) where the lockmgr should store/get a pointer to a user allocated mutex. It is NULL upon AV_LOCK_CREATE and equal to the value left by the last call for all other ops. If the lock manager is unable to perform the op then it should leave the mutex in the same state as when it was called and return a non-zero value. However, when called with AV_LOCK_DESTROY the mutex will always be assumed to have been successfully destroyed. If av_lockmgr_register succeeds it will return a non-negative value, if it fails it will return a negative value and destroy all mutex and unregister all callbacks. av_lockmgr_register is not thread-safe, it must be called from a single thread before any calls which make use of locking are used.</summary>
        /// <param name="cb">User defined callback. av_lockmgr_register invokes calls to this callback and the previously registered callback. The callback will be used to create more than one mutex each of which must be backed by its own underlying locking mechanism (i.e. do not use a single static object to implement your lock manager). If cb is set to NULL the lockmgr will be unregistered.</param>
        [DllImport("avcodec-57", EntryPoint = "av_lockmgr_register", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern int av_lockmgr_register(av_lockmgr_register_cb_func @cb);
        
        /// <summary>Log a generic warning message asking for a sample. This function is intended to be used internally by FFmpeg (libavcodec, libavformat, etc.) only, and would normally not be used by applications.</summary>
        /// <param name="avc">a pointer to an arbitrary struct of which the first field is a pointer to an AVClass struct</param>
        /// <param name="msg">string containing an optional message, or NULL if no message</param>
        [Obsolete("Use avpriv_request_sample() instead.")]
        [DllImport("avcodec-57", EntryPoint = "av_log_ask_for_sample", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern void av_log_ask_for_sample(void* @avc, [MarshalAs(UnmanagedType.LPStr)] string @msg);
        
        /// <summary>Log a generic warning message about a missing feature. This function is intended to be used internally by FFmpeg (libavcodec, libavformat, etc.) only, and would normally not be used by applications.</summary>
        /// <param name="avc">a pointer to an arbitrary struct of which the first field is a pointer to an AVClass struct</param>
        /// <param name="feature">string containing the name of the missing feature</param>
        /// <param name="want_sample">indicates if samples are wanted which exhibit this feature. If want_sample is non-zero, additional verbiage will be added to the log message which tells the user how to report samples to the development mailing list.</param>
        [Obsolete("Use avpriv_report_missing_feature() instead.")]
        [DllImport("avcodec-57", EntryPoint = "av_log_missing_feature", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern void av_log_missing_feature(void* @avc, [MarshalAs(UnmanagedType.LPStr)] string @feature, int @want_sample);
        
        /// <summary>Allocate the payload of a packet and initialize its fields with default values.</summary>
        /// <param name="pkt">packet</param>
        /// <param name="size">wanted payload size</param>
        [DllImport("avcodec-57", EntryPoint = "av_new_packet", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern int av_new_packet(AVPacket* @pkt, int @size);
        
        /// <summary>Wrap an existing array as a packet side data.</summary>
        /// <param name="pkt">packet</param>
        /// <param name="type">side information type</param>
        /// <param name="data">the side data array. It must be allocated with the av_malloc() family of functions. The ownership of the data is transferred to pkt.</param>
        /// <param name="size">side information size</param>
        [DllImport("avcodec-57", EntryPoint = "av_packet_add_side_data", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern int av_packet_add_side_data(AVPacket* @pkt, AVPacketSideDataType @type, byte* @data, ulong @size);
        
        /// <summary>Allocate an AVPacket and set its fields to default values. The resulting struct must be freed using av_packet_free().</summary>
        [DllImport("avcodec-57", EntryPoint = "av_packet_alloc", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern AVPacket* av_packet_alloc();
        
        /// <summary>Create a new packet that references the same data as src.</summary>
        [DllImport("avcodec-57", EntryPoint = "av_packet_clone", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern AVPacket* av_packet_clone(AVPacket* @src);
        
        /// <summary>Copy only &quot;properties&quot; fields from src to dst.</summary>
        /// <param name="dst">Destination packet</param>
        /// <param name="src">Source packet</param>
        [DllImport("avcodec-57", EntryPoint = "av_packet_copy_props", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern int av_packet_copy_props(AVPacket* @dst, AVPacket* @src);
        
        /// <summary>Free the packet, if the packet is reference counted, it will be unreferenced first.</summary>
        [DllImport("avcodec-57", EntryPoint = "av_packet_free", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern void av_packet_free(AVPacket** @pkt);
        
        /// <summary>Convenience function to free all the side data stored. All the other fields stay untouched.</summary>
        /// <param name="pkt">packet</param>
        [DllImport("avcodec-57", EntryPoint = "av_packet_free_side_data", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern void av_packet_free_side_data(AVPacket* @pkt);
        
        /// <summary>Initialize a reference-counted packet from av_malloc()ed data.</summary>
        /// <param name="pkt">packet to be initialized. This function will set the data, size, buf and destruct fields, all others are left untouched.</param>
        /// <param name="data">Data allocated by av_malloc() to be used as packet data. If this function returns successfully, the data is owned by the underlying AVBuffer. The caller may not access the data through other means.</param>
        /// <param name="size">size of data in bytes, without the padding. I.e. the full buffer size is assumed to be size + AV_INPUT_BUFFER_PADDING_SIZE.</param>
        [DllImport("avcodec-57", EntryPoint = "av_packet_from_data", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern int av_packet_from_data(AVPacket* @pkt, byte* @data, int @size);
        
        /// <summary>Get side information from packet.</summary>
        /// <param name="pkt">packet</param>
        /// <param name="type">desired side information type</param>
        /// <param name="size">pointer for side information size to store (optional)</param>
        [DllImport("avcodec-57", EntryPoint = "av_packet_get_side_data", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern byte* av_packet_get_side_data(AVPacket* @pkt, AVPacketSideDataType @type, int* @size);
        
        [Obsolete("")]
        [DllImport("avcodec-57", EntryPoint = "av_packet_merge_side_data", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern int av_packet_merge_side_data(AVPacket* @pkt);
        
        /// <summary>Move every field in src to dst and reset src.</summary>
        /// <param name="dst">Destination packet</param>
        /// <param name="src">Source packet, will be reset</param>
        [DllImport("avcodec-57", EntryPoint = "av_packet_move_ref", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern void av_packet_move_ref(AVPacket* @dst, AVPacket* @src);
        
        /// <summary>Allocate new information of a packet.</summary>
        /// <param name="pkt">packet</param>
        /// <param name="type">side information type</param>
        /// <param name="size">side information size</param>
        [DllImport("avcodec-57", EntryPoint = "av_packet_new_side_data", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern byte* av_packet_new_side_data(AVPacket* @pkt, AVPacketSideDataType @type, int @size);
        
        /// <summary>Pack a dictionary for use in side_data.</summary>
        /// <param name="dict">The dictionary to pack.</param>
        /// <param name="size">pointer to store the size of the returned data</param>
        [DllImport("avcodec-57", EntryPoint = "av_packet_pack_dictionary", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern byte* av_packet_pack_dictionary(AVDictionary* @dict, int* @size);
        
        /// <summary>Setup a new reference to the data described by a given packet</summary>
        /// <param name="dst">Destination packet</param>
        /// <param name="src">Source packet</param>
        [DllImport("avcodec-57", EntryPoint = "av_packet_ref", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern int av_packet_ref(AVPacket* @dst, AVPacket* @src);
        
        /// <summary>Convert valid timing fields (timestamps / durations) in a packet from one timebase to another. Timestamps with unknown values (AV_NOPTS_VALUE) will be ignored.</summary>
        /// <param name="pkt">packet on which the conversion will be performed</param>
        /// <param name="tb_src">source timebase, in which the timing fields in pkt are expressed</param>
        /// <param name="tb_dst">destination timebase, to which the timing fields will be converted</param>
        [DllImport("avcodec-57", EntryPoint = "av_packet_rescale_ts", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern void av_packet_rescale_ts(AVPacket* @pkt, AVRational @tb_src, AVRational @tb_dst);
        
        /// <summary>Shrink the already allocated side data buffer</summary>
        /// <param name="pkt">packet</param>
        /// <param name="type">side information type</param>
        /// <param name="size">new side information size</param>
        [DllImport("avcodec-57", EntryPoint = "av_packet_shrink_side_data", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern int av_packet_shrink_side_data(AVPacket* @pkt, AVPacketSideDataType @type, int @size);
        
        [DllImport("avcodec-57", EntryPoint = "av_packet_side_data_name", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        [return: MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof(ConstCharPtrMarshaler))]
        public static extern string av_packet_side_data_name(AVPacketSideDataType @type);
        
        [Obsolete("")]
        [DllImport("avcodec-57", EntryPoint = "av_packet_split_side_data", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern int av_packet_split_side_data(AVPacket* @pkt);
        
        /// <summary>Unpack a dictionary from side_data.</summary>
        /// <param name="data">data from side_data</param>
        /// <param name="size">size of the data</param>
        /// <param name="dict">the metadata storage dictionary</param>
        [DllImport("avcodec-57", EntryPoint = "av_packet_unpack_dictionary", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern int av_packet_unpack_dictionary(byte* @data, int @size, AVDictionary** @dict);
        
        /// <summary>Wipe the packet.</summary>
        /// <param name="pkt">The packet to be unreferenced.</param>
        [DllImport("avcodec-57", EntryPoint = "av_packet_unref", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern void av_packet_unref(AVPacket* @pkt);
        
        /// <summary>Returns 0 if the output buffer is a subset of the input, 1 if it is allocated and must be freed use AVBitStreamFilter</summary>
        [DllImport("avcodec-57", EntryPoint = "av_parser_change", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern int av_parser_change(AVCodecParserContext* @s, AVCodecContext* @avctx, byte** @poutbuf, int* @poutbuf_size, byte* @buf, int @buf_size, int @keyframe);
        
        [DllImport("avcodec-57", EntryPoint = "av_parser_close", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern void av_parser_close(AVCodecParserContext* @s);
        
        [DllImport("avcodec-57", EntryPoint = "av_parser_init", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern AVCodecParserContext* av_parser_init(int @codec_id);
        
        [DllImport("avcodec-57", EntryPoint = "av_parser_next", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern AVCodecParser* av_parser_next(AVCodecParser* @c);
        
        /// <summary>Parse a packet.</summary>
        /// <param name="s">parser context.</param>
        /// <param name="avctx">codec context.</param>
        /// <param name="poutbuf">set to pointer to parsed buffer or NULL if not yet finished.</param>
        /// <param name="poutbuf_size">set to size of parsed buffer or zero if not yet finished.</param>
        /// <param name="buf">input buffer.</param>
        /// <param name="buf_size">buffer size in bytes without the padding. I.e. the full buffer size is assumed to be buf_size + AV_INPUT_BUFFER_PADDING_SIZE. To signal EOF, this should be 0 (so that the last frame can be output).</param>
        /// <param name="pts">input presentation timestamp.</param>
        /// <param name="dts">input decoding timestamp.</param>
        /// <param name="pos">input byte position in stream.</param>
        [DllImport("avcodec-57", EntryPoint = "av_parser_parse2", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern int av_parser_parse2(AVCodecParserContext* @s, AVCodecContext* @avctx, byte** @poutbuf, int* @poutbuf_size, byte* @buf, int @buf_size, long @pts, long @dts, long @pos);
        
        [Obsolete("av_image_copy() instead.")]
        [DllImport("avcodec-57", EntryPoint = "av_picture_copy", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern void av_picture_copy(AVPicture* @dst, AVPicture* @src, AVPixelFormat @pix_fmt, int @width, int @height);
        
        [Obsolete("unused")]
        [DllImport("avcodec-57", EntryPoint = "av_picture_crop", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern int av_picture_crop(AVPicture* @dst, AVPicture* @src, AVPixelFormat @pix_fmt, int @top_band, int @left_band);
        
        [Obsolete("unused")]
        [DllImport("avcodec-57", EntryPoint = "av_picture_pad", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern int av_picture_pad(AVPicture* @dst, AVPicture* @src, int @height, int @width, AVPixelFormat @pix_fmt, int @padtop, int @padbottom, int @padleft, int @padright, int* @color);
        
        /// <summary>Register a bitstream filter.</summary>
        [Obsolete("")]
        [DllImport("avcodec-57", EntryPoint = "av_register_bitstream_filter", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern void av_register_bitstream_filter(AVBitStreamFilter* @bsf);
        
        [DllImport("avcodec-57", EntryPoint = "av_register_codec_parser", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern void av_register_codec_parser(AVCodecParser* @parser);
        
        /// <summary>Register the hardware accelerator hwaccel.</summary>
        [DllImport("avcodec-57", EntryPoint = "av_register_hwaccel", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern void av_register_hwaccel(AVHWAccel* @hwaccel);
        
        /// <summary>Resample an array of samples using a previously configured context.</summary>
        /// <param name="src">an array of unconsumed samples</param>
        /// <param name="consumed">the number of samples of src which have been consumed are returned here</param>
        /// <param name="src_size">the number of unconsumed samples available</param>
        /// <param name="dst_size">the amount of space in samples available in dst</param>
        /// <param name="update_ctx">If this is 0 then the context will not be modified, that way several channels can be resampled with the same context.</param>
        [Obsolete("")]
        [DllImport("avcodec-57", EntryPoint = "av_resample", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern int av_resample(AVResampleContext* @c, short* @dst, short* @src, int* @consumed, int @src_size, int @dst_size, int @update_ctx);
        
        [Obsolete("")]
        [DllImport("avcodec-57", EntryPoint = "av_resample_close", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern void av_resample_close(AVResampleContext* @c);
        
        /// <summary>Compensate samplerate/timestamp drift. The compensation is done by changing the resampler parameters, so no audible clicks or similar distortions occur</summary>
        /// <param name="sample_delta">number of output samples which should be output less</param>
        /// <param name="compensation_distance">distance in output samples over which the compensation should be performed</param>
        [Obsolete("")]
        [DllImport("avcodec-57", EntryPoint = "av_resample_compensate", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern void av_resample_compensate(AVResampleContext* @c, int @sample_delta, int @compensation_distance);
        
        /// <summary>Initialize an audio resampler. Note, if either rate is not an integer then simply scale both rates up so they are.</summary>
        /// <param name="filter_length">length of each FIR filter in the filterbank relative to the cutoff freq</param>
        /// <param name="log2_phase_count">log2 of the number of entries in the polyphase filterbank</param>
        /// <param name="linear">If 1 then the used FIR filter will be linearly interpolated between the 2 closest, if 0 the closest will be used</param>
        /// <param name="cutoff">cutoff frequency, 1.0 corresponds to half the output sampling rate</param>
        [Obsolete("")]
        [DllImport("avcodec-57", EntryPoint = "av_resample_init", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern AVResampleContext* av_resample_init(int @out_rate, int @in_rate, int @filter_length, int @log2_phase_count, int @linear, double @cutoff);
        
        /// <summary>Reduce packet size, correctly zeroing padding</summary>
        /// <param name="pkt">packet</param>
        /// <param name="size">new size</param>
        [DllImport("avcodec-57", EntryPoint = "av_shrink_packet", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern void av_shrink_packet(AVPacket* @pkt, int @size);
        
        /// <summary>Encode extradata length to a buffer. Used by xiph codecs.</summary>
        /// <param name="s">buffer to write to; must be at least (v/255+1) bytes long</param>
        /// <param name="v">size of extradata in bytes</param>
        [DllImport("avcodec-57", EntryPoint = "av_xiphlacing", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern uint av_xiphlacing(byte* @s, uint @v);
        
        /// <summary>Modify width and height values so that they will result in a memory buffer that is acceptable for the codec if you do not use any horizontal padding.</summary>
        [DllImport("avcodec-57", EntryPoint = "avcodec_align_dimensions", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern void avcodec_align_dimensions(AVCodecContext* @s, int* @width, int* @height);
        
        /// <summary>Modify width and height values so that they will result in a memory buffer that is acceptable for the codec if you also ensure that all line sizes are a multiple of the respective linesize_align[i].</summary>
        [DllImport("avcodec-57", EntryPoint = "avcodec_align_dimensions2", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern void avcodec_align_dimensions2(AVCodecContext* @s, int* @width, int* @height, ref int_array8 @linesize_align);
        
        /// <summary>Allocate an AVCodecContext and set its fields to default values. The resulting struct should be freed with avcodec_free_context().</summary>
        /// <param name="codec">if non-NULL, allocate private data and initialize defaults for the given codec. It is illegal to then call avcodec_open2() with a different codec. If NULL, then the codec-specific defaults won&apos;t be initialized, which may result in suboptimal default settings (this is important mainly for encoders, e.g. libx264).</param>
        [DllImport("avcodec-57", EntryPoint = "avcodec_alloc_context3", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern AVCodecContext* avcodec_alloc_context3(AVCodec* @codec);
        
        /// <summary>Converts swscale x/y chroma position to AVChromaLocation.</summary>
        /// <param name="xpos">horizontal chroma sample position</param>
        /// <param name="ypos">vertical   chroma sample position</param>
        [DllImport("avcodec-57", EntryPoint = "avcodec_chroma_pos_to_enum", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern AVChromaLocation avcodec_chroma_pos_to_enum(int @xpos, int @ypos);
        
        /// <summary>Close a given AVCodecContext and free all the data associated with it (but not the AVCodecContext itself).</summary>
        [DllImport("avcodec-57", EntryPoint = "avcodec_close", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern int avcodec_close(AVCodecContext* @avctx);
        
        /// <summary>Return the libavcodec build-time configuration.</summary>
        [DllImport("avcodec-57", EntryPoint = "avcodec_configuration", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        [return: MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof(ConstCharPtrMarshaler))]
        public static extern string avcodec_configuration();
        
        /// <summary>Copy the settings of the source AVCodecContext into the destination AVCodecContext. The resulting destination codec context will be unopened, i.e. you are required to call avcodec_open2() before you can use this AVCodecContext to decode/encode video/audio data.</summary>
        /// <param name="dest">target codec context, should be initialized with avcodec_alloc_context3(NULL), but otherwise uninitialized</param>
        /// <param name="src">source codec context</param>
        [Obsolete("The semantics of this function are ill-defined and it should not be used. If you need to transfer the stream parameters from one codec context to another, use an intermediate AVCodecParameters instance and the avcodec_parameters_from_context() / avcodec_parameters_to_context() functions.")]
        [DllImport("avcodec-57", EntryPoint = "avcodec_copy_context", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern int avcodec_copy_context(AVCodecContext* @dest, AVCodecContext* @src);
        
        /// <summary>Decode the audio frame of size avpkt-&gt;size from avpkt-&gt;data into frame.</summary>
        /// <param name="avctx">the codec context</param>
        /// <param name="frame">The AVFrame in which to store decoded audio samples. The decoder will allocate a buffer for the decoded frame by calling the AVCodecContext.get_buffer2() callback. When AVCodecContext.refcounted_frames is set to 1, the frame is reference counted and the returned reference belongs to the caller. The caller must release the frame using av_frame_unref() when the frame is no longer needed. The caller may safely write to the frame if av_frame_is_writable() returns 1. When AVCodecContext.refcounted_frames is set to 0, the returned reference belongs to the decoder and is valid only until the next call to this function or until closing or flushing the decoder. The caller may not write to it.</param>
        /// <param name="got_frame_ptr">Zero if no frame could be decoded, otherwise it is non-zero. Note that this field being set to zero does not mean that an error has occurred. For decoders with AV_CODEC_CAP_DELAY set, no given decode call is guaranteed to produce a frame.</param>
        /// <param name="avpkt">The input AVPacket containing the input buffer. At least avpkt-&gt;data and avpkt-&gt;size should be set. Some decoders might also require additional fields to be set.</param>
        [Obsolete("Use avcodec_send_packet() and avcodec_receive_frame().")]
        [DllImport("avcodec-57", EntryPoint = "avcodec_decode_audio4", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern int avcodec_decode_audio4(AVCodecContext* @avctx, AVFrame* @frame, int* @got_frame_ptr, AVPacket* @avpkt);
        
        /// <summary>Decode a subtitle message. Return a negative value on error, otherwise return the number of bytes used. If no subtitle could be decompressed, got_sub_ptr is zero. Otherwise, the subtitle is stored in *sub. Note that AV_CODEC_CAP_DR1 is not available for subtitle codecs. This is for simplicity, because the performance difference is expect to be negligible and reusing a get_buffer written for video codecs would probably perform badly due to a potentially very different allocation pattern.</summary>
        /// <param name="avctx">the codec context</param>
        /// <param name="sub">The Preallocated AVSubtitle in which the decoded subtitle will be stored, must be freed with avsubtitle_free if *got_sub_ptr is set.</param>
        /// <param name="got_sub_ptr">Zero if no subtitle could be decompressed, otherwise, it is nonzero.</param>
        /// <param name="avpkt">The input AVPacket containing the input buffer.</param>
        [DllImport("avcodec-57", EntryPoint = "avcodec_decode_subtitle2", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern int avcodec_decode_subtitle2(AVCodecContext* @avctx, AVSubtitle* @sub, int* @got_sub_ptr, AVPacket* @avpkt);
        
        /// <summary>Decode the video frame of size avpkt-&gt;size from avpkt-&gt;data into picture. Some decoders may support multiple frames in a single AVPacket, such decoders would then just decode the first frame.</summary>
        /// <param name="avctx">the codec context</param>
        /// <param name="picture">The AVFrame in which the decoded video frame will be stored. Use av_frame_alloc() to get an AVFrame. The codec will allocate memory for the actual bitmap by calling the AVCodecContext.get_buffer2() callback. When AVCodecContext.refcounted_frames is set to 1, the frame is reference counted and the returned reference belongs to the caller. The caller must release the frame using av_frame_unref() when the frame is no longer needed. The caller may safely write to the frame if av_frame_is_writable() returns 1. When AVCodecContext.refcounted_frames is set to 0, the returned reference belongs to the decoder and is valid only until the next call to this function or until closing or flushing the decoder. The caller may not write to it.</param>
        /// <param name="got_picture_ptr">Zero if no frame could be decompressed, otherwise, it is nonzero.</param>
        /// <param name="avpkt">The input AVPacket containing the input buffer. You can create such packet with av_init_packet() and by then setting data and size, some decoders might in addition need other fields like flags &amp;AV _PKT_FLAG_KEY. All decoders are designed to use the least fields possible.</param>
        [Obsolete("Use avcodec_send_packet() and avcodec_receive_frame().")]
        [DllImport("avcodec-57", EntryPoint = "avcodec_decode_video2", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern int avcodec_decode_video2(AVCodecContext* @avctx, AVFrame* @picture, int* @got_picture_ptr, AVPacket* @avpkt);
        
        [DllImport("avcodec-57", EntryPoint = "avcodec_default_execute", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern int avcodec_default_execute(AVCodecContext* @c, avcodec_default_execute_func_func @func, void* @arg, int* @ret, int @count, int @size);
        
        [DllImport("avcodec-57", EntryPoint = "avcodec_default_execute2", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern int avcodec_default_execute2(AVCodecContext* @c, avcodec_default_execute2_func_func @func, void* @arg, int* @ret, int @count);
        
        /// <summary>The default callback for AVCodecContext.get_buffer2(). It is made public so it can be called by custom get_buffer2() implementations for decoders without AV_CODEC_CAP_DR1 set.</summary>
        [DllImport("avcodec-57", EntryPoint = "avcodec_default_get_buffer2", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern int avcodec_default_get_buffer2(AVCodecContext* @s, AVFrame* @frame, int @flags);
        
        [DllImport("avcodec-57", EntryPoint = "avcodec_default_get_format", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern AVPixelFormat avcodec_default_get_format(AVCodecContext* @s, AVPixelFormat* @fmt);
        
        /// <summary>Returns descriptor for given codec ID or NULL if no descriptor exists.</summary>
        [DllImport("avcodec-57", EntryPoint = "avcodec_descriptor_get", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern AVCodecDescriptor* avcodec_descriptor_get(AVCodecID @id);
        
        /// <summary>Returns codec descriptor with the given name or NULL if no such descriptor exists.</summary>
        [DllImport("avcodec-57", EntryPoint = "avcodec_descriptor_get_by_name", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern AVCodecDescriptor* avcodec_descriptor_get_by_name([MarshalAs(UnmanagedType.LPStr)] string @name);
        
        /// <summary>Iterate over all codec descriptors known to libavcodec.</summary>
        /// <param name="prev">previous descriptor. NULL to get the first descriptor.</param>
        [DllImport("avcodec-57", EntryPoint = "avcodec_descriptor_next", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern AVCodecDescriptor* avcodec_descriptor_next(AVCodecDescriptor* @prev);
        
        /// <summary>Encode a frame of audio.</summary>
        /// <param name="avctx">codec context</param>
        /// <param name="avpkt">output AVPacket. The user can supply an output buffer by setting avpkt-&gt;data and avpkt-&gt;size prior to calling the function, but if the size of the user-provided data is not large enough, encoding will fail. If avpkt-&gt;data and avpkt-&gt;size are set, avpkt-&gt;destruct must also be set. All other AVPacket fields will be reset by the encoder using av_init_packet(). If avpkt-&gt;data is NULL, the encoder will allocate it. The encoder will set avpkt-&gt;size to the size of the output packet.</param>
        /// <param name="frame">AVFrame containing the raw audio data to be encoded. May be NULL when flushing an encoder that has the AV_CODEC_CAP_DELAY capability set. If AV_CODEC_CAP_VARIABLE_FRAME_SIZE is set, then each frame can have any number of samples. If it is not set, frame-&gt;nb_samples must be equal to avctx-&gt;frame_size for all frames except the last. The final frame may be smaller than avctx-&gt;frame_size.</param>
        /// <param name="got_packet_ptr">This field is set to 1 by libavcodec if the output packet is non-empty, and to 0 if it is empty. If the function returns an error, the packet can be assumed to be invalid, and the value of got_packet_ptr is undefined and should not be used.</param>
        [Obsolete("use avcodec_send_frame()/avcodec_receive_packet() instead")]
        [DllImport("avcodec-57", EntryPoint = "avcodec_encode_audio2", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern int avcodec_encode_audio2(AVCodecContext* @avctx, AVPacket* @avpkt, AVFrame* @frame, int* @got_packet_ptr);
        
        [DllImport("avcodec-57", EntryPoint = "avcodec_encode_subtitle", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern int avcodec_encode_subtitle(AVCodecContext* @avctx, byte* @buf, int @buf_size, AVSubtitle* @sub);
        
        /// <summary>Encode a frame of video.</summary>
        /// <param name="avctx">codec context</param>
        /// <param name="avpkt">output AVPacket. The user can supply an output buffer by setting avpkt-&gt;data and avpkt-&gt;size prior to calling the function, but if the size of the user-provided data is not large enough, encoding will fail. All other AVPacket fields will be reset by the encoder using av_init_packet(). If avpkt-&gt;data is NULL, the encoder will allocate it. The encoder will set avpkt-&gt;size to the size of the output packet. The returned data (if any) belongs to the caller, he is responsible for freeing it.</param>
        /// <param name="frame">AVFrame containing the raw video data to be encoded. May be NULL when flushing an encoder that has the AV_CODEC_CAP_DELAY capability set.</param>
        /// <param name="got_packet_ptr">This field is set to 1 by libavcodec if the output packet is non-empty, and to 0 if it is empty. If the function returns an error, the packet can be assumed to be invalid, and the value of got_packet_ptr is undefined and should not be used.</param>
        [Obsolete("use avcodec_send_frame()/avcodec_receive_packet() instead")]
        [DllImport("avcodec-57", EntryPoint = "avcodec_encode_video2", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern int avcodec_encode_video2(AVCodecContext* @avctx, AVPacket* @avpkt, AVFrame* @frame, int* @got_packet_ptr);
        
        /// <summary>Converts AVChromaLocation to swscale x/y chroma position.</summary>
        /// <param name="xpos">horizontal chroma sample position</param>
        /// <param name="ypos">vertical   chroma sample position</param>
        [DllImport("avcodec-57", EntryPoint = "avcodec_enum_to_chroma_pos", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern int avcodec_enum_to_chroma_pos(int* @xpos, int* @ypos, AVChromaLocation @pos);
        
        /// <summary>Fill AVFrame audio data and linesize pointers.</summary>
        /// <param name="frame">the AVFrame frame-&gt;nb_samples must be set prior to calling the function. This function fills in frame-&gt;data, frame-&gt;extended_data, frame-&gt;linesize[0].</param>
        /// <param name="nb_channels">channel count</param>
        /// <param name="sample_fmt">sample format</param>
        /// <param name="buf">buffer to use for frame data</param>
        /// <param name="buf_size">size of buffer</param>
        /// <param name="align">plane size sample alignment (0 = default)</param>
        [DllImport("avcodec-57", EntryPoint = "avcodec_fill_audio_frame", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern int avcodec_fill_audio_frame(AVFrame* @frame, int @nb_channels, AVSampleFormat @sample_fmt, byte* @buf, int @buf_size, int @align);
        
        [DllImport("avcodec-57", EntryPoint = "avcodec_find_best_pix_fmt_of_2", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern AVPixelFormat avcodec_find_best_pix_fmt_of_2(AVPixelFormat @dst_pix_fmt1, AVPixelFormat @dst_pix_fmt2, AVPixelFormat @src_pix_fmt, int @has_alpha, int* @loss_ptr);
        
        /// <summary>Find the best pixel format to convert to given a certain source pixel format. When converting from one pixel format to another, information loss may occur. For example, when converting from RGB24 to GRAY, the color information will be lost. Similarly, other losses occur when converting from some formats to other formats. avcodec_find_best_pix_fmt_of_2() searches which of the given pixel formats should be used to suffer the least amount of loss. The pixel formats from which it chooses one, are determined by the pix_fmt_list parameter.</summary>
        /// <param name="pix_fmt_list">AV_PIX_FMT_NONE terminated array of pixel formats to choose from</param>
        /// <param name="src_pix_fmt">source pixel format</param>
        /// <param name="has_alpha">Whether the source pixel format alpha channel is used.</param>
        /// <param name="loss_ptr">Combination of flags informing you what kind of losses will occur.</param>
        [DllImport("avcodec-57", EntryPoint = "avcodec_find_best_pix_fmt_of_list", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern AVPixelFormat avcodec_find_best_pix_fmt_of_list(AVPixelFormat* @pix_fmt_list, AVPixelFormat @src_pix_fmt, int @has_alpha, int* @loss_ptr);
        
        [Obsolete("")]
        [DllImport("avcodec-57", EntryPoint = "avcodec_find_best_pix_fmt2", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern AVPixelFormat avcodec_find_best_pix_fmt2(AVPixelFormat @dst_pix_fmt1, AVPixelFormat @dst_pix_fmt2, AVPixelFormat @src_pix_fmt, int @has_alpha, int* @loss_ptr);
        
        /// <summary>Find a registered decoder with a matching codec ID.</summary>
        /// <param name="id">AVCodecID of the requested decoder</param>
        [DllImport("avcodec-57", EntryPoint = "avcodec_find_decoder", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern AVCodec* avcodec_find_decoder(AVCodecID @id);
        
        /// <summary>Find a registered decoder with the specified name.</summary>
        /// <param name="name">name of the requested decoder</param>
        [DllImport("avcodec-57", EntryPoint = "avcodec_find_decoder_by_name", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern AVCodec* avcodec_find_decoder_by_name([MarshalAs(UnmanagedType.LPStr)] string @name);
        
        /// <summary>Find a registered encoder with a matching codec ID.</summary>
        /// <param name="id">AVCodecID of the requested encoder</param>
        [DllImport("avcodec-57", EntryPoint = "avcodec_find_encoder", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern AVCodec* avcodec_find_encoder(AVCodecID @id);
        
        /// <summary>Find a registered encoder with the specified name.</summary>
        /// <param name="name">name of the requested encoder</param>
        [DllImport("avcodec-57", EntryPoint = "avcodec_find_encoder_by_name", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern AVCodec* avcodec_find_encoder_by_name([MarshalAs(UnmanagedType.LPStr)] string @name);
        
        /// <summary>Reset the internal decoder state / flush internal buffers. Should be called e.g. when seeking or when switching to a different stream.</summary>
        [DllImport("avcodec-57", EntryPoint = "avcodec_flush_buffers", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern void avcodec_flush_buffers(AVCodecContext* @avctx);
        
        /// <summary>Free the codec context and everything associated with it and write NULL to the provided pointer.</summary>
        [DllImport("avcodec-57", EntryPoint = "avcodec_free_context", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern void avcodec_free_context(AVCodecContext** @avctx);
        
        /// <summary>Utility function to access log2_chroma_w log2_chroma_h from the pixel format AVPixFmtDescriptor.</summary>
        /// <param name="pix_fmt">the pixel format</param>
        /// <param name="h_shift">store log2_chroma_w</param>
        /// <param name="v_shift">store log2_chroma_h</param>
        [DllImport("avcodec-57", EntryPoint = "avcodec_get_chroma_sub_sample", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern void avcodec_get_chroma_sub_sample(AVPixelFormat @pix_fmt, int* @h_shift, int* @v_shift);
        
        /// <summary>Get the AVClass for AVCodecContext. It can be used in combination with AV_OPT_SEARCH_FAKE_OBJ for examining options.</summary>
        [DllImport("avcodec-57", EntryPoint = "avcodec_get_class", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern AVClass* avcodec_get_class();
        
        [DllImport("avcodec-57", EntryPoint = "avcodec_get_context_defaults3", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern int avcodec_get_context_defaults3(AVCodecContext* @s, AVCodec* @codec);
        
        /// <summary>Return the amount of padding in pixels which the get_buffer callback must provide around the edge of the image for codecs which do not have the CODEC_FLAG_EMU_EDGE flag.</summary>
        [Obsolete("CODEC_FLAG_EMU_EDGE is deprecated, so this function is no longer needed")]
        [DllImport("avcodec-57", EntryPoint = "avcodec_get_edge_width", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern uint avcodec_get_edge_width();
        
        /// <summary>Get the AVClass for AVFrame. It can be used in combination with AV_OPT_SEARCH_FAKE_OBJ for examining options.</summary>
        [DllImport("avcodec-57", EntryPoint = "avcodec_get_frame_class", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern AVClass* avcodec_get_frame_class();
        
        /// <summary>Get the name of a codec.</summary>
        [DllImport("avcodec-57", EntryPoint = "avcodec_get_name", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        [return: MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof(ConstCharPtrMarshaler))]
        public static extern string avcodec_get_name(AVCodecID @id);
        
        [DllImport("avcodec-57", EntryPoint = "avcodec_get_pix_fmt_loss", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern int avcodec_get_pix_fmt_loss(AVPixelFormat @dst_pix_fmt, AVPixelFormat @src_pix_fmt, int @has_alpha);
        
        /// <summary>Get the AVClass for AVSubtitleRect. It can be used in combination with AV_OPT_SEARCH_FAKE_OBJ for examining options.</summary>
        [DllImport("avcodec-57", EntryPoint = "avcodec_get_subtitle_rect_class", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern AVClass* avcodec_get_subtitle_rect_class();
        
        /// <summary>Get the type of the given codec.</summary>
        [DllImport("avcodec-57", EntryPoint = "avcodec_get_type", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern AVMediaType avcodec_get_type(AVCodecID @codec_id);
        
        /// <summary>Returns a positive value if s is open (i.e. avcodec_open2() was called on it with no corresponding avcodec_close()), 0 otherwise.</summary>
        [DllImport("avcodec-57", EntryPoint = "avcodec_is_open", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern int avcodec_is_open(AVCodecContext* @s);
        
        /// <summary>Return the libavcodec license.</summary>
        [DllImport("avcodec-57", EntryPoint = "avcodec_license", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        [return: MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof(ConstCharPtrMarshaler))]
        public static extern string avcodec_license();
        
        /// <summary>Initialize the AVCodecContext to use the given AVCodec. Prior to using this function the context has to be allocated with avcodec_alloc_context3().</summary>
        /// <param name="avctx">The context to initialize.</param>
        /// <param name="codec">The codec to open this context for. If a non-NULL codec has been previously passed to avcodec_alloc_context3() or for this context, then this parameter MUST be either NULL or equal to the previously passed codec.</param>
        /// <param name="options">A dictionary filled with AVCodecContext and codec-private options. On return this object will be filled with options that were not found.</param>
        [DllImport("avcodec-57", EntryPoint = "avcodec_open2", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern int avcodec_open2(AVCodecContext* @avctx, AVCodec* @codec, AVDictionary** @options);
        
        /// <summary>Allocate a new AVCodecParameters and set its fields to default values (unknown/invalid/0). The returned struct must be freed with avcodec_parameters_free().</summary>
        [DllImport("avcodec-57", EntryPoint = "avcodec_parameters_alloc", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern AVCodecParameters* avcodec_parameters_alloc();
        
        /// <summary>Copy the contents of src to dst. Any allocated fields in dst are freed and replaced with newly allocated duplicates of the corresponding fields in src.</summary>
        [DllImport("avcodec-57", EntryPoint = "avcodec_parameters_copy", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern int avcodec_parameters_copy(AVCodecParameters* @dst, AVCodecParameters* @src);
        
        /// <summary>Free an AVCodecParameters instance and everything associated with it and write NULL to the supplied pointer.</summary>
        [DllImport("avcodec-57", EntryPoint = "avcodec_parameters_free", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern void avcodec_parameters_free(AVCodecParameters** @par);
        
        /// <summary>Fill the parameters struct based on the values from the supplied codec context. Any allocated fields in par are freed and replaced with duplicates of the corresponding fields in codec.</summary>
        [DllImport("avcodec-57", EntryPoint = "avcodec_parameters_from_context", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern int avcodec_parameters_from_context(AVCodecParameters* @par, AVCodecContext* @codec);
        
        /// <summary>Fill the codec context based on the values from the supplied codec parameters. Any allocated fields in codec that have a corresponding field in par are freed and replaced with duplicates of the corresponding field in par. Fields in codec that do not have a counterpart in par are not touched.</summary>
        [DllImport("avcodec-57", EntryPoint = "avcodec_parameters_to_context", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern int avcodec_parameters_to_context(AVCodecContext* @codec, AVCodecParameters* @par);
        
        /// <summary>Return a value representing the fourCC code associated to the pixel format pix_fmt, or 0 if no associated fourCC code can be found.</summary>
        [DllImport("avcodec-57", EntryPoint = "avcodec_pix_fmt_to_codec_tag", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern uint avcodec_pix_fmt_to_codec_tag(AVPixelFormat @pix_fmt);
        
        /// <summary>Return a name for the specified profile, if available.</summary>
        /// <param name="codec_id">the ID of the codec to which the requested profile belongs</param>
        /// <param name="profile">the profile value for which a name is requested</param>
        [DllImport("avcodec-57", EntryPoint = "avcodec_profile_name", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        [return: MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof(ConstCharPtrMarshaler))]
        public static extern string avcodec_profile_name(AVCodecID @codec_id, int @profile);
        
        /// <summary>Return decoded output data from a decoder.</summary>
        /// <param name="avctx">codec context</param>
        /// <param name="frame">This will be set to a reference-counted video or audio frame (depending on the decoder type) allocated by the decoder. Note that the function will always call av_frame_unref(frame) before doing anything else.</param>
        [DllImport("avcodec-57", EntryPoint = "avcodec_receive_frame", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern int avcodec_receive_frame(AVCodecContext* @avctx, AVFrame* @frame);
        
        /// <summary>Read encoded data from the encoder.</summary>
        /// <param name="avctx">codec context</param>
        /// <param name="avpkt">This will be set to a reference-counted packet allocated by the encoder. Note that the function will always call av_frame_unref(frame) before doing anything else.</param>
        [DllImport("avcodec-57", EntryPoint = "avcodec_receive_packet", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern int avcodec_receive_packet(AVCodecContext* @avctx, AVPacket* @avpkt);
        
        /// <summary>Register the codec codec and initialize libavcodec.</summary>
        [DllImport("avcodec-57", EntryPoint = "avcodec_register", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern void avcodec_register(AVCodec* @codec);
        
        /// <summary>Register all the codecs, parsers and bitstream filters which were enabled at configuration time. If you do not call this function you can select exactly which formats you want to support, by using the individual registration functions.</summary>
        [DllImport("avcodec-57", EntryPoint = "avcodec_register_all", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern void avcodec_register_all();
        
        /// <summary>Supply a raw video or audio frame to the encoder. Use avcodec_receive_packet() to retrieve buffered output packets.</summary>
        /// <param name="avctx">codec context</param>
        /// <param name="frame">AVFrame containing the raw audio or video frame to be encoded. Ownership of the frame remains with the caller, and the encoder will not write to the frame. The encoder may create a reference to the frame data (or copy it if the frame is not reference-counted). It can be NULL, in which case it is considered a flush packet.  This signals the end of the stream. If the encoder still has packets buffered, it will return them after this call. Once flushing mode has been entered, additional flush packets are ignored, and sending frames will return AVERROR_EOF.</param>
        [DllImport("avcodec-57", EntryPoint = "avcodec_send_frame", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern int avcodec_send_frame(AVCodecContext* @avctx, AVFrame* @frame);
        
        /// <summary>Supply raw packet data as input to a decoder.</summary>
        /// <param name="avctx">codec context</param>
        /// <param name="avpkt">The input AVPacket. Usually, this will be a single video frame, or several complete audio frames. Ownership of the packet remains with the caller, and the decoder will not write to the packet. The decoder may create a reference to the packet data (or copy it if the packet is not reference-counted). Unlike with older APIs, the packet is always fully consumed, and if it contains multiple frames (e.g. some audio codecs), will require you to call avcodec_receive_frame() multiple times afterwards before you can send a new packet. It can be NULL (or an AVPacket with data set to NULL and size set to 0); in this case, it is considered a flush packet, which signals the end of the stream. Sending the first flush packet will return success. Subsequent ones are unnecessary and will return AVERROR_EOF. If the decoder still has frames buffered, it will return them after sending a flush packet.</param>
        [DllImport("avcodec-57", EntryPoint = "avcodec_send_packet", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern int avcodec_send_packet(AVCodecContext* @avctx, AVPacket* @avpkt);
        
        [Obsolete("this function is not supposed to be used from outside of lavc")]
        [DllImport("avcodec-57", EntryPoint = "avcodec_set_dimensions", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern void avcodec_set_dimensions(AVCodecContext* @s, int @width, int @height);
        
        [DllImport("avcodec-57", EntryPoint = "avcodec_string", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern void avcodec_string(byte* @buf, int @buf_size, AVCodecContext* @enc, int @encode);
        
        /// <summary>Return the LIBAVCODEC_VERSION_INT constant.</summary>
        [DllImport("avcodec-57", EntryPoint = "avcodec_version", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern uint avcodec_version();
        
        [Obsolete("unused")]
        [DllImport("avcodec-57", EntryPoint = "avpicture_alloc", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern int avpicture_alloc(AVPicture* @picture, AVPixelFormat @pix_fmt, int @width, int @height);
        
        [Obsolete("use av_image_fill_arrays() instead.")]
        [DllImport("avcodec-57", EntryPoint = "avpicture_fill", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern int avpicture_fill(AVPicture* @picture, byte* @ptr, AVPixelFormat @pix_fmt, int @width, int @height);
        
        [Obsolete("unused")]
        [DllImport("avcodec-57", EntryPoint = "avpicture_free", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern void avpicture_free(AVPicture* @picture);
        
        [Obsolete("use av_image_get_buffer_size() instead.")]
        [DllImport("avcodec-57", EntryPoint = "avpicture_get_size", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern int avpicture_get_size(AVPixelFormat @pix_fmt, int @width, int @height);
        
        [Obsolete("use av_image_copy_to_buffer() instead.")]
        [DllImport("avcodec-57", EntryPoint = "avpicture_layout", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern int avpicture_layout(AVPicture* @src, AVPixelFormat @pix_fmt, int @width, int @height, byte* @dest, int @dest_size);
        
        /// <summary>Free all allocated data in the given subtitle struct.</summary>
        /// <param name="sub">AVSubtitle to free.</param>
        [DllImport("avcodec-57", EntryPoint = "avsubtitle_free", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern void avsubtitle_free(AVSubtitle* @sub);
        
        /// <summary>Audio input devices iterator.</summary>
        [DllImport("avdevice-57", EntryPoint = "av_input_audio_device_next", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern AVInputFormat* av_input_audio_device_next(AVInputFormat* @d);
        
        /// <summary>Video input devices iterator.</summary>
        [DllImport("avdevice-57", EntryPoint = "av_input_video_device_next", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern AVInputFormat* av_input_video_device_next(AVInputFormat* @d);
        
        /// <summary>Audio output devices iterator.</summary>
        [DllImport("avdevice-57", EntryPoint = "av_output_audio_device_next", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern AVOutputFormat* av_output_audio_device_next(AVOutputFormat* @d);
        
        /// <summary>Video output devices iterator.</summary>
        [DllImport("avdevice-57", EntryPoint = "av_output_video_device_next", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern AVOutputFormat* av_output_video_device_next(AVOutputFormat* @d);
        
        /// <summary>Send control message from application to device.</summary>
        /// <param name="s">device context.</param>
        /// <param name="type">message type.</param>
        /// <param name="data">message data. Exact type depends on message type.</param>
        /// <param name="data_size">size of message data.</param>
        [DllImport("avdevice-57", EntryPoint = "avdevice_app_to_dev_control_message", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern int avdevice_app_to_dev_control_message(AVFormatContext* @s, AVAppToDevMessageType @type, void* @data, ulong @data_size);
        
        /// <summary>Initialize capabilities probing API based on AVOption API.</summary>
        /// <param name="caps">Device capabilities data. Pointer to a NULL pointer must be passed.</param>
        /// <param name="s">Context of the device.</param>
        /// <param name="device_options">An AVDictionary filled with device-private options. On return this parameter will be destroyed and replaced with a dict containing options that were not found. May be NULL. The same options must be passed later to avformat_write_header() for output devices or avformat_open_input() for input devices, or at any other place that affects device-private options.</param>
        [DllImport("avdevice-57", EntryPoint = "avdevice_capabilities_create", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern int avdevice_capabilities_create(AVDeviceCapabilitiesQuery** @caps, AVFormatContext* @s, AVDictionary** @device_options);
        
        /// <summary>Free resources created by avdevice_capabilities_create()</summary>
        /// <param name="caps">Device capabilities data to be freed.</param>
        /// <param name="s">Context of the device.</param>
        [DllImport("avdevice-57", EntryPoint = "avdevice_capabilities_free", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern void avdevice_capabilities_free(AVDeviceCapabilitiesQuery** @caps, AVFormatContext* @s);
        
        /// <summary>Return the libavdevice build-time configuration.</summary>
        [DllImport("avdevice-57", EntryPoint = "avdevice_configuration", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        [return: MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof(ConstCharPtrMarshaler))]
        public static extern string avdevice_configuration();
        
        /// <summary>Send control message from device to application.</summary>
        /// <param name="s">device context.</param>
        /// <param name="type">message type.</param>
        /// <param name="data">message data. Can be NULL.</param>
        /// <param name="data_size">size of message data.</param>
        [DllImport("avdevice-57", EntryPoint = "avdevice_dev_to_app_control_message", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern int avdevice_dev_to_app_control_message(AVFormatContext* @s, AVDevToAppMessageType @type, void* @data, ulong @data_size);
        
        /// <summary>Convenient function to free result of avdevice_list_devices().</summary>
        [DllImport("avdevice-57", EntryPoint = "avdevice_free_list_devices", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern void avdevice_free_list_devices(AVDeviceInfoList** @device_list);
        
        /// <summary>Return the libavdevice license.</summary>
        [DllImport("avdevice-57", EntryPoint = "avdevice_license", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        [return: MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof(ConstCharPtrMarshaler))]
        public static extern string avdevice_license();
        
        /// <summary>List devices.</summary>
        /// <param name="s">device context.</param>
        /// <param name="device_list">list of autodetected devices.</param>
        [DllImport("avdevice-57", EntryPoint = "avdevice_list_devices", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern int avdevice_list_devices(AVFormatContext* @s, AVDeviceInfoList** @device_list);
        
        /// <summary>List devices.</summary>
        /// <param name="device">device format. May be NULL if device name is set.</param>
        /// <param name="device_name">device name. May be NULL if device format is set.</param>
        /// <param name="device_options">An AVDictionary filled with device-private options. May be NULL. The same options must be passed later to avformat_write_header() for output devices or avformat_open_input() for input devices, or at any other place that affects device-private options.</param>
        /// <param name="device_list">list of autodetected devices</param>
        [DllImport("avdevice-57", EntryPoint = "avdevice_list_input_sources", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern int avdevice_list_input_sources(AVInputFormat* @device, [MarshalAs(UnmanagedType.LPStr)] string @device_name, AVDictionary* @device_options, AVDeviceInfoList** @device_list);
        
        [DllImport("avdevice-57", EntryPoint = "avdevice_list_output_sinks", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern int avdevice_list_output_sinks(AVOutputFormat* @device, [MarshalAs(UnmanagedType.LPStr)] string @device_name, AVDictionary* @device_options, AVDeviceInfoList** @device_list);
        
        /// <summary>Initialize libavdevice and register all the input and output devices.</summary>
        [DllImport("avdevice-57", EntryPoint = "avdevice_register_all", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern void avdevice_register_all();
        
        /// <summary>Return the LIBAVDEVICE_VERSION_INT constant.</summary>
        [DllImport("avdevice-57", EntryPoint = "avdevice_version", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern uint avdevice_version();
        
        /// <summary>Create an AVABufferSinkParams structure.</summary>
        [DllImport("avfilter-6", EntryPoint = "av_abuffersink_params_alloc", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern AVABufferSinkParams* av_abuffersink_params_alloc();
        
        [DllImport("avfilter-6", EntryPoint = "av_buffersink_get_channel_layout", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern ulong av_buffersink_get_channel_layout(AVFilterContext* @ctx);
        
        [DllImport("avfilter-6", EntryPoint = "av_buffersink_get_channels", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern int av_buffersink_get_channels(AVFilterContext* @ctx);
        
        [DllImport("avfilter-6", EntryPoint = "av_buffersink_get_format", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern int av_buffersink_get_format(AVFilterContext* @ctx);
        
        /// <summary>Get a frame with filtered data from sink and put it in frame.</summary>
        /// <param name="ctx">pointer to a context of a buffersink or abuffersink AVFilter.</param>
        /// <param name="frame">pointer to an allocated frame that will be filled with data. The data must be freed using av_frame_unref() / av_frame_free()</param>
        [DllImport("avfilter-6", EntryPoint = "av_buffersink_get_frame", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern int av_buffersink_get_frame(AVFilterContext* @ctx, AVFrame* @frame);
        
        /// <summary>Get a frame with filtered data from sink and put it in frame.</summary>
        /// <param name="ctx">pointer to a buffersink or abuffersink filter context.</param>
        /// <param name="frame">pointer to an allocated frame that will be filled with data. The data must be freed using av_frame_unref() / av_frame_free()</param>
        /// <param name="flags">a combination of AV_BUFFERSINK_FLAG_* flags</param>
        [DllImport("avfilter-6", EntryPoint = "av_buffersink_get_frame_flags", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern int av_buffersink_get_frame_flags(AVFilterContext* @ctx, AVFrame* @frame, int @flags);
        
        [DllImport("avfilter-6", EntryPoint = "av_buffersink_get_frame_rate", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern AVRational av_buffersink_get_frame_rate(AVFilterContext* @ctx);
        
        [DllImport("avfilter-6", EntryPoint = "av_buffersink_get_h", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern int av_buffersink_get_h(AVFilterContext* @ctx);
        
        [DllImport("avfilter-6", EntryPoint = "av_buffersink_get_hw_frames_ctx", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern AVBufferRef* av_buffersink_get_hw_frames_ctx(AVFilterContext* @ctx);
        
        [DllImport("avfilter-6", EntryPoint = "av_buffersink_get_sample_aspect_ratio", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern AVRational av_buffersink_get_sample_aspect_ratio(AVFilterContext* @ctx);
        
        [DllImport("avfilter-6", EntryPoint = "av_buffersink_get_sample_rate", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern int av_buffersink_get_sample_rate(AVFilterContext* @ctx);
        
        /// <summary>Same as av_buffersink_get_frame(), but with the ability to specify the number of samples read. This function is less efficient than av_buffersink_get_frame(), because it copies the data around.</summary>
        /// <param name="ctx">pointer to a context of the abuffersink AVFilter.</param>
        /// <param name="frame">pointer to an allocated frame that will be filled with data. The data must be freed using av_frame_unref() / av_frame_free() frame will contain exactly nb_samples audio samples, except at the end of stream, when it can contain less than nb_samples.</param>
        [DllImport("avfilter-6", EntryPoint = "av_buffersink_get_samples", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern int av_buffersink_get_samples(AVFilterContext* @ctx, AVFrame* @frame, int @nb_samples);
        
        [DllImport("avfilter-6", EntryPoint = "av_buffersink_get_time_base", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern AVRational av_buffersink_get_time_base(AVFilterContext* @ctx);
        
        /// <summary>Get the properties of the stream @{</summary>
        [DllImport("avfilter-6", EntryPoint = "av_buffersink_get_type", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern AVMediaType av_buffersink_get_type(AVFilterContext* @ctx);
        
        [DllImport("avfilter-6", EntryPoint = "av_buffersink_get_w", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern int av_buffersink_get_w(AVFilterContext* @ctx);
        
        /// <summary>Create an AVBufferSinkParams structure.</summary>
        [DllImport("avfilter-6", EntryPoint = "av_buffersink_params_alloc", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern AVBufferSinkParams* av_buffersink_params_alloc();
        
        /// <summary>Set the frame size for an audio buffer sink.</summary>
        [DllImport("avfilter-6", EntryPoint = "av_buffersink_set_frame_size", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern void av_buffersink_set_frame_size(AVFilterContext* @ctx, uint @frame_size);
        
        /// <summary>Add a frame to the buffer source.</summary>
        /// <param name="ctx">an instance of the buffersrc filter</param>
        /// <param name="frame">frame to be added. If the frame is reference counted, this function will take ownership of the reference(s) and reset the frame. Otherwise the frame data will be copied. If this function returns an error, the input frame is not touched.</param>
        [DllImport("avfilter-6", EntryPoint = "av_buffersrc_add_frame", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern int av_buffersrc_add_frame(AVFilterContext* @ctx, AVFrame* @frame);
        
        /// <summary>Add a frame to the buffer source.</summary>
        /// <param name="buffer_src">pointer to a buffer source context</param>
        /// <param name="frame">a frame, or NULL to mark EOF</param>
        /// <param name="flags">a combination of AV_BUFFERSRC_FLAG_*</param>
        [DllImport("avfilter-6", EntryPoint = "av_buffersrc_add_frame_flags", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern int av_buffersrc_add_frame_flags(AVFilterContext* @buffer_src, AVFrame* @frame, int @flags);
        
        /// <summary>Get the number of failed requests.</summary>
        [DllImport("avfilter-6", EntryPoint = "av_buffersrc_get_nb_failed_requests", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern uint av_buffersrc_get_nb_failed_requests(AVFilterContext* @buffer_src);
        
        /// <summary>Allocate a new AVBufferSrcParameters instance. It should be freed by the caller with av_free().</summary>
        [DllImport("avfilter-6", EntryPoint = "av_buffersrc_parameters_alloc", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern AVBufferSrcParameters* av_buffersrc_parameters_alloc();
        
        /// <summary>Initialize the buffersrc or abuffersrc filter with the provided parameters. This function may be called multiple times, the later calls override the previous ones. Some of the parameters may also be set through AVOptions, then whatever method is used last takes precedence.</summary>
        /// <param name="ctx">an instance of the buffersrc or abuffersrc filter</param>
        /// <param name="param">the stream parameters. The frames later passed to this filter must conform to those parameters. All the allocated fields in param remain owned by the caller, libavfilter will make internal copies or references when necessary.</param>
        [DllImport("avfilter-6", EntryPoint = "av_buffersrc_parameters_set", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern int av_buffersrc_parameters_set(AVFilterContext* @ctx, AVBufferSrcParameters* @param);
        
        /// <summary>Add a frame to the buffer source.</summary>
        /// <param name="ctx">an instance of the buffersrc filter</param>
        /// <param name="frame">frame to be added. If the frame is reference counted, this function will make a new reference to it. Otherwise the frame data will be copied.</param>
        [DllImport("avfilter-6", EntryPoint = "av_buffersrc_write_frame", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern int av_buffersrc_write_frame(AVFilterContext* @ctx, AVFrame* @frame);
        
        /// <summary>If filter is NULL, returns a pointer to the first registered filter pointer, if filter is non-NULL, returns the next pointer after filter. If the returned pointer points to NULL, the last registered filter was already reached.</summary>
        [Obsolete("use avfilter_next()")]
        [DllImport("avfilter-6", EntryPoint = "av_filter_next", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern AVFilter** av_filter_next(AVFilter** @filter);
        
        /// <summary>Negotiate the media format, dimensions, etc of all inputs to a filter.</summary>
        /// <param name="filter">the filter to negotiate the properties for its inputs</param>
        [DllImport("avfilter-6", EntryPoint = "avfilter_config_links", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern int avfilter_config_links(AVFilterContext* @filter);
        
        /// <summary>Return the libavfilter build-time configuration.</summary>
        [DllImport("avfilter-6", EntryPoint = "avfilter_configuration", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        [return: MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof(ConstCharPtrMarshaler))]
        public static extern string avfilter_configuration();
        
        /// <summary>Free a filter context. This will also remove the filter from its filtergraph&apos;s list of filters.</summary>
        /// <param name="filter">the filter to free</param>
        [DllImport("avfilter-6", EntryPoint = "avfilter_free", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern void avfilter_free(AVFilterContext* @filter);
        
        [DllImport("avfilter-6", EntryPoint = "avfilter_get_by_name", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern AVFilter* avfilter_get_by_name([MarshalAs(UnmanagedType.LPStr)] string @name);
        
        /// <summary>Returns AVClass for AVFilterContext.</summary>
        [DllImport("avfilter-6", EntryPoint = "avfilter_get_class", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern AVClass* avfilter_get_class();
        
        /// <summary>Add an existing filter instance to a filter graph.</summary>
        /// <param name="graphctx">the filter graph</param>
        /// <param name="filter">the filter to be added</param>
        [Obsolete("use avfilter_graph_alloc_filter() to allocate a filter in a filter graph")]
        [DllImport("avfilter-6", EntryPoint = "avfilter_graph_add_filter", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern int avfilter_graph_add_filter(AVFilterGraph* @graphctx, AVFilterContext* @filter);
        
        /// <summary>Allocate a filter graph.</summary>
        [DllImport("avfilter-6", EntryPoint = "avfilter_graph_alloc", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern AVFilterGraph* avfilter_graph_alloc();
        
        /// <summary>Create a new filter instance in a filter graph.</summary>
        /// <param name="graph">graph in which the new filter will be used</param>
        /// <param name="filter">the filter to create an instance of</param>
        /// <param name="name">Name to give to the new instance (will be copied to AVFilterContext.name). This may be used by the caller to identify different filters, libavfilter itself assigns no semantics to this parameter. May be NULL.</param>
        [DllImport("avfilter-6", EntryPoint = "avfilter_graph_alloc_filter", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern AVFilterContext* avfilter_graph_alloc_filter(AVFilterGraph* @graph, AVFilter* @filter, [MarshalAs(UnmanagedType.LPStr)] string @name);
        
        /// <summary>Check validity and configure all the links and formats in the graph.</summary>
        /// <param name="graphctx">the filter graph</param>
        /// <param name="log_ctx">context used for logging</param>
        [DllImport("avfilter-6", EntryPoint = "avfilter_graph_config", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern int avfilter_graph_config(AVFilterGraph* @graphctx, void* @log_ctx);
        
        /// <summary>Create and add a filter instance into an existing graph. The filter instance is created from the filter filt and inited with the parameters args and opaque.</summary>
        /// <param name="name">the instance name to give to the created filter instance</param>
        /// <param name="graph_ctx">the filter graph</param>
        [DllImport("avfilter-6", EntryPoint = "avfilter_graph_create_filter", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern int avfilter_graph_create_filter(AVFilterContext** @filt_ctx, AVFilter* @filt, [MarshalAs(UnmanagedType.LPStr)] string @name, [MarshalAs(UnmanagedType.LPStr)] string @args, void* @opaque, AVFilterGraph* @graph_ctx);
        
        /// <summary>Dump a graph into a human-readable string representation.</summary>
        /// <param name="graph">the graph to dump</param>
        /// <param name="options">formatting options; currently ignored</param>
        [DllImport("avfilter-6", EntryPoint = "avfilter_graph_dump", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern byte* avfilter_graph_dump(AVFilterGraph* @graph, [MarshalAs(UnmanagedType.LPStr)] string @options);
        
        /// <summary>Free a graph, destroy its links, and set *graph to NULL. If *graph is NULL, do nothing.</summary>
        [DllImport("avfilter-6", EntryPoint = "avfilter_graph_free", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern void avfilter_graph_free(AVFilterGraph** @graph);
        
        /// <summary>Get a filter instance identified by instance name from graph.</summary>
        /// <param name="graph">filter graph to search through.</param>
        /// <param name="name">filter instance name (should be unique in the graph).</param>
        [DllImport("avfilter-6", EntryPoint = "avfilter_graph_get_filter", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern AVFilterContext* avfilter_graph_get_filter(AVFilterGraph* @graph, [MarshalAs(UnmanagedType.LPStr)] string @name);
        
        /// <summary>Add a graph described by a string to a graph.</summary>
        /// <param name="graph">the filter graph where to link the parsed graph context</param>
        /// <param name="filters">string to be parsed</param>
        /// <param name="inputs">linked list to the inputs of the graph</param>
        /// <param name="outputs">linked list to the outputs of the graph</param>
        [DllImport("avfilter-6", EntryPoint = "avfilter_graph_parse", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern int avfilter_graph_parse(AVFilterGraph* @graph, [MarshalAs(UnmanagedType.LPStr)] string @filters, AVFilterInOut* @inputs, AVFilterInOut* @outputs, void* @log_ctx);
        
        /// <summary>Add a graph described by a string to a graph.</summary>
        /// <param name="graph">the filter graph where to link the parsed graph context</param>
        /// <param name="filters">string to be parsed</param>
        /// <param name="inputs">pointer to a linked list to the inputs of the graph, may be NULL. If non-NULL, *inputs is updated to contain the list of open inputs after the parsing, should be freed with avfilter_inout_free().</param>
        /// <param name="outputs">pointer to a linked list to the outputs of the graph, may be NULL. If non-NULL, *outputs is updated to contain the list of open outputs after the parsing, should be freed with avfilter_inout_free().</param>
        [DllImport("avfilter-6", EntryPoint = "avfilter_graph_parse_ptr", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern int avfilter_graph_parse_ptr(AVFilterGraph* @graph, [MarshalAs(UnmanagedType.LPStr)] string @filters, AVFilterInOut** @inputs, AVFilterInOut** @outputs, void* @log_ctx);
        
        /// <summary>Add a graph described by a string to a graph.</summary>
        /// <param name="graph">the filter graph where to link the parsed graph context</param>
        /// <param name="filters">string to be parsed</param>
        /// <param name="inputs">a linked list of all free (unlinked) inputs of the parsed graph will be returned here. It is to be freed by the caller using avfilter_inout_free().</param>
        /// <param name="outputs">a linked list of all free (unlinked) outputs of the parsed graph will be returned here. It is to be freed by the caller using avfilter_inout_free().</param>
        [DllImport("avfilter-6", EntryPoint = "avfilter_graph_parse2", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern int avfilter_graph_parse2(AVFilterGraph* @graph, [MarshalAs(UnmanagedType.LPStr)] string @filters, AVFilterInOut** @inputs, AVFilterInOut** @outputs);
        
        /// <summary>Queue a command for one or more filter instances.</summary>
        /// <param name="graph">the filter graph</param>
        /// <param name="target">the filter(s) to which the command should be sent &quot;all&quot; sends to all filters otherwise it can be a filter or filter instance name which will send the command to all matching filters.</param>
        /// <param name="cmd">the command to sent, for handling simplicity all commands must be alphanumeric only</param>
        /// <param name="arg">the argument for the command</param>
        /// <param name="ts">time at which the command should be sent to the filter</param>
        [DllImport("avfilter-6", EntryPoint = "avfilter_graph_queue_command", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern int avfilter_graph_queue_command(AVFilterGraph* @graph, [MarshalAs(UnmanagedType.LPStr)] string @target, [MarshalAs(UnmanagedType.LPStr)] string @cmd, [MarshalAs(UnmanagedType.LPStr)] string @arg, int @flags, double @ts);
        
        /// <summary>Request a frame on the oldest sink link.</summary>
        [DllImport("avfilter-6", EntryPoint = "avfilter_graph_request_oldest", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern int avfilter_graph_request_oldest(AVFilterGraph* @graph);
        
        /// <summary>Send a command to one or more filter instances.</summary>
        /// <param name="graph">the filter graph</param>
        /// <param name="target">the filter(s) to which the command should be sent &quot;all&quot; sends to all filters otherwise it can be a filter or filter instance name which will send the command to all matching filters.</param>
        /// <param name="cmd">the command to send, for handling simplicity all commands must be alphanumeric only</param>
        /// <param name="arg">the argument for the command</param>
        /// <param name="res">a buffer with size res_size where the filter(s) can return a response.</param>
        [DllImport("avfilter-6", EntryPoint = "avfilter_graph_send_command", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern int avfilter_graph_send_command(AVFilterGraph* @graph, [MarshalAs(UnmanagedType.LPStr)] string @target, [MarshalAs(UnmanagedType.LPStr)] string @cmd, [MarshalAs(UnmanagedType.LPStr)] string @arg, byte* @res, int @res_len, int @flags);
        
        /// <summary>Enable or disable automatic format conversion inside the graph.</summary>
        /// <param name="flags">any of the AVFILTER_AUTO_CONVERT_* constants</param>
        [DllImport("avfilter-6", EntryPoint = "avfilter_graph_set_auto_convert", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern void avfilter_graph_set_auto_convert(AVFilterGraph* @graph, uint @flags);
        
        /// <summary>Initialize a filter with the supplied dictionary of options.</summary>
        /// <param name="ctx">uninitialized filter context to initialize</param>
        /// <param name="options">An AVDictionary filled with options for this filter. On return this parameter will be destroyed and replaced with a dict containing options that were not found. This dictionary must be freed by the caller. May be NULL, then this function is equivalent to avfilter_init_str() with the second parameter set to NULL.</param>
        [DllImport("avfilter-6", EntryPoint = "avfilter_init_dict", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern int avfilter_init_dict(AVFilterContext* @ctx, AVDictionary** @options);
        
        /// <summary>Initialize a filter.</summary>
        /// <param name="filter">the filter to initialize</param>
        /// <param name="args">A string of parameters to use when initializing the filter. The format and meaning of this string varies by filter.</param>
        /// <param name="opaque">Any extra non-string data needed by the filter. The meaning of this parameter varies by filter.</param>
        [Obsolete("")]
        [DllImport("avfilter-6", EntryPoint = "avfilter_init_filter", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern int avfilter_init_filter(AVFilterContext* @filter, [MarshalAs(UnmanagedType.LPStr)] string @args, void* @opaque);
        
        /// <summary>Initialize a filter with the supplied parameters.</summary>
        /// <param name="ctx">uninitialized filter context to initialize</param>
        /// <param name="args">Options to initialize the filter with. This must be a &apos;:&apos;-separated list of options in the &apos;key=value&apos; form. May be NULL if the options have been set directly using the AVOptions API or there are no options that need to be set.</param>
        [DllImport("avfilter-6", EntryPoint = "avfilter_init_str", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern int avfilter_init_str(AVFilterContext* @ctx, [MarshalAs(UnmanagedType.LPStr)] string @args);
        
        /// <summary>Allocate a single AVFilterInOut entry. Must be freed with avfilter_inout_free().</summary>
        [DllImport("avfilter-6", EntryPoint = "avfilter_inout_alloc", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern AVFilterInOut* avfilter_inout_alloc();
        
        /// <summary>Free the supplied list of AVFilterInOut and set *inout to NULL. If *inout is NULL, do nothing.</summary>
        [DllImport("avfilter-6", EntryPoint = "avfilter_inout_free", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern void avfilter_inout_free(AVFilterInOut** @inout);
        
        /// <summary>Insert a filter in the middle of an existing link.</summary>
        /// <param name="link">the link into which the filter should be inserted</param>
        /// <param name="filt">the filter to be inserted</param>
        /// <param name="filt_srcpad_idx">the input pad on the filter to connect</param>
        /// <param name="filt_dstpad_idx">the output pad on the filter to connect</param>
        [DllImport("avfilter-6", EntryPoint = "avfilter_insert_filter", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern int avfilter_insert_filter(AVFilterLink* @link, AVFilterContext* @filt, uint @filt_srcpad_idx, uint @filt_dstpad_idx);
        
        /// <summary>Return the libavfilter license.</summary>
        [DllImport("avfilter-6", EntryPoint = "avfilter_license", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        [return: MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof(ConstCharPtrMarshaler))]
        public static extern string avfilter_license();
        
        /// <summary>Link two filters together.</summary>
        /// <param name="src">the source filter</param>
        /// <param name="srcpad">index of the output pad on the source filter</param>
        /// <param name="dst">the destination filter</param>
        /// <param name="dstpad">index of the input pad on the destination filter</param>
        [DllImport("avfilter-6", EntryPoint = "avfilter_link", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern int avfilter_link(AVFilterContext* @src, uint @srcpad, AVFilterContext* @dst, uint @dstpad);
        
        /// <summary>Free the link in *link, and set its pointer to NULL.</summary>
        [DllImport("avfilter-6", EntryPoint = "avfilter_link_free", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern void avfilter_link_free(AVFilterLink** @link);
        
        /// <summary>Get the number of channels of a link.</summary>
        [DllImport("avfilter-6", EntryPoint = "avfilter_link_get_channels", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern int avfilter_link_get_channels(AVFilterLink* @link);
        
        /// <summary>Set the closed field of a link.</summary>
        [Obsolete("applications are not supposed to mess with links, they should close the sinks.")]
        [DllImport("avfilter-6", EntryPoint = "avfilter_link_set_closed", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern void avfilter_link_set_closed(AVFilterLink* @link, int @closed);
        
        /// <summary>Iterate over all registered filters.</summary>
        [DllImport("avfilter-6", EntryPoint = "avfilter_next", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern AVFilter* avfilter_next(AVFilter* @prev);
        
        /// <summary>Create a filter instance.</summary>
        /// <param name="filter_ctx">put here a pointer to the created filter context on success, NULL on failure</param>
        /// <param name="filter">the filter to create an instance of</param>
        /// <param name="inst_name">Name to give to the new instance. Can be NULL for none.</param>
        [Obsolete("use avfilter_graph_alloc_filter() instead")]
        [DllImport("avfilter-6", EntryPoint = "avfilter_open", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern int avfilter_open(AVFilterContext** @filter_ctx, AVFilter* @filter, [MarshalAs(UnmanagedType.LPStr)] string @inst_name);
        
        /// <summary>Get the number of elements in a NULL-terminated array of AVFilterPads (e.g. AVFilter.inputs/outputs).</summary>
        [DllImport("avfilter-6", EntryPoint = "avfilter_pad_count", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern int avfilter_pad_count(AVFilterPad* @pads);
        
        /// <summary>Get the name of an AVFilterPad.</summary>
        /// <param name="pads">an array of AVFilterPads</param>
        /// <param name="pad_idx">index of the pad in the array it; is the caller&apos;s responsibility to ensure the index is valid</param>
        [DllImport("avfilter-6", EntryPoint = "avfilter_pad_get_name", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        [return: MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof(ConstCharPtrMarshaler))]
        public static extern string avfilter_pad_get_name(AVFilterPad* @pads, int @pad_idx);
        
        /// <summary>Get the type of an AVFilterPad.</summary>
        /// <param name="pads">an array of AVFilterPads</param>
        /// <param name="pad_idx">index of the pad in the array; it is the caller&apos;s responsibility to ensure the index is valid</param>
        [DllImport("avfilter-6", EntryPoint = "avfilter_pad_get_type", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern AVMediaType avfilter_pad_get_type(AVFilterPad* @pads, int @pad_idx);
        
        /// <summary>Make the filter instance process a command. It is recommended to use avfilter_graph_send_command().</summary>
        [DllImport("avfilter-6", EntryPoint = "avfilter_process_command", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern int avfilter_process_command(AVFilterContext* @filter, [MarshalAs(UnmanagedType.LPStr)] string @cmd, [MarshalAs(UnmanagedType.LPStr)] string @arg, byte* @res, int @res_len, int @flags);
        
        /// <summary>Register a filter. This is only needed if you plan to use avfilter_get_by_name later to lookup the AVFilter structure by name. A filter can still by instantiated with avfilter_graph_alloc_filter even if it is not registered.</summary>
        /// <param name="filter">the filter to register</param>
        [DllImport("avfilter-6", EntryPoint = "avfilter_register", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern int avfilter_register(AVFilter* @filter);
        
        /// <summary>Initialize the filter system. Register all builtin filters.</summary>
        [DllImport("avfilter-6", EntryPoint = "avfilter_register_all", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern void avfilter_register_all();
        
        /// <summary>Uninitialize the filter system. Unregister all filters.</summary>
        [Obsolete("")]
        [DllImport("avfilter-6", EntryPoint = "avfilter_uninit", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern void avfilter_uninit();
        
        /// <summary>Return the LIBAVFILTER_VERSION_INT constant.</summary>
        [DllImport("avfilter-6", EntryPoint = "avfilter_version", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern uint avfilter_version();
        
        /// <summary>Add an index entry into a sorted list. Update the entry if the list already contains it.</summary>
        /// <param name="timestamp">timestamp in the time base of the given stream</param>
        [DllImport("avformat-57", EntryPoint = "av_add_index_entry", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern int av_add_index_entry(AVStream* @st, long @pos, long @timestamp, int @size, int @distance, int @flags);
        
        /// <summary>Read data and append it to the current content of the AVPacket. If pkt-&gt;size is 0 this is identical to av_get_packet. Note that this uses av_grow_packet and thus involves a realloc which is inefficient. Thus this function should only be used when there is no reasonable way to know (an upper bound of) the final size.</summary>
        /// <param name="s">associated IO context</param>
        /// <param name="pkt">packet</param>
        /// <param name="size">amount of data to read</param>
        [DllImport("avformat-57", EntryPoint = "av_append_packet", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern int av_append_packet(AVIOContext* @s, AVPacket* @pkt, int @size);
        
        [Obsolete("")]
        [DllImport("avformat-57", EntryPoint = "av_apply_bitstream_filters", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern int av_apply_bitstream_filters(AVCodecContext* @codec, AVPacket* @pkt, AVBitStreamFilterContext* @bsfc);
        
        /// <summary>Get the AVCodecID for the given codec tag tag. If no codec id is found returns AV_CODEC_ID_NONE.</summary>
        /// <param name="tags">list of supported codec_id-codec_tag pairs, as stored in AVInputFormat.codec_tag and AVOutputFormat.codec_tag</param>
        /// <param name="tag">codec tag to match to a codec ID</param>
        [DllImport("avformat-57", EntryPoint = "av_codec_get_id", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern AVCodecID av_codec_get_id(AVCodecTag** @tags, uint @tag);
        
        /// <summary>Get the codec tag for the given codec id id. If no codec tag is found returns 0.</summary>
        /// <param name="tags">list of supported codec_id-codec_tag pairs, as stored in AVInputFormat.codec_tag and AVOutputFormat.codec_tag</param>
        /// <param name="id">codec ID to match to a codec tag</param>
        [DllImport("avformat-57", EntryPoint = "av_codec_get_tag", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern uint av_codec_get_tag(AVCodecTag** @tags, AVCodecID @id);
        
        /// <summary>Get the codec tag for the given codec id.</summary>
        /// <param name="tags">list of supported codec_id - codec_tag pairs, as stored in AVInputFormat.codec_tag and AVOutputFormat.codec_tag</param>
        /// <param name="id">codec id that should be searched for in the list</param>
        /// <param name="tag">A pointer to the found tag</param>
        [DllImport("avformat-57", EntryPoint = "av_codec_get_tag2", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern int av_codec_get_tag2(AVCodecTag** @tags, AVCodecID @id, uint* @tag);
        
        [Obsolete("")]
        [DllImport("avformat-57", EntryPoint = "av_demuxer_open", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern int av_demuxer_open(AVFormatContext* @ic);
        
        /// <summary>Print detailed information about the input or output format, such as duration, bitrate, streams, container, programs, metadata, side data, codec and time base.</summary>
        /// <param name="ic">the context to analyze</param>
        /// <param name="index">index of the stream to dump information about</param>
        /// <param name="url">the URL to print, such as source or destination file</param>
        /// <param name="is_output">Select whether the specified context is an input(0) or output(1)</param>
        [DllImport("avformat-57", EntryPoint = "av_dump_format", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern void av_dump_format(AVFormatContext* @ic, int @index, [MarshalAs(UnmanagedType.LPStr)] string @url, int @is_output);
        
        /// <summary>Check whether filename actually is a numbered sequence generator.</summary>
        /// <param name="filename">possible numbered sequence string</param>
        [DllImport("avformat-57", EntryPoint = "av_filename_number_test", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern int av_filename_number_test([MarshalAs(UnmanagedType.LPStr)] string @filename);
        
        /// <summary>Find the &quot;best&quot; stream in the file. The best stream is determined according to various heuristics as the most likely to be what the user expects. If the decoder parameter is non-NULL, av_find_best_stream will find the default decoder for the stream&apos;s codec; streams for which no decoder can be found are ignored.</summary>
        /// <param name="ic">media file handle</param>
        /// <param name="type">stream type: video, audio, subtitles, etc.</param>
        /// <param name="wanted_stream_nb">user-requested stream number, or -1 for automatic selection</param>
        /// <param name="related_stream">try to find a stream related (eg. in the same program) to this one, or -1 if none</param>
        /// <param name="decoder_ret">if non-NULL, returns the decoder for the selected stream</param>
        /// <param name="flags">flags; none are currently defined</param>
        [DllImport("avformat-57", EntryPoint = "av_find_best_stream", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern int av_find_best_stream(AVFormatContext* @ic, AVMediaType @type, int @wanted_stream_nb, int @related_stream, AVCodec** @decoder_ret, int @flags);
        
        [DllImport("avformat-57", EntryPoint = "av_find_default_stream_index", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern int av_find_default_stream_index(AVFormatContext* @s);
        
        /// <summary>Find AVInputFormat based on the short name of the input format.</summary>
        [DllImport("avformat-57", EntryPoint = "av_find_input_format", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern AVInputFormat* av_find_input_format([MarshalAs(UnmanagedType.LPStr)] string @short_name);
        
        /// <summary>Find the programs which belong to a given stream.</summary>
        /// <param name="ic">media file handle</param>
        /// <param name="last">the last found program, the search will start after this program, or from the beginning if it is NULL</param>
        /// <param name="s">stream index</param>
        [DllImport("avformat-57", EntryPoint = "av_find_program_from_stream", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern AVProgram* av_find_program_from_stream(AVFormatContext* @ic, AVProgram* @last, int @s);
        
        /// <summary>Returns the method used to set ctx-&gt;duration.</summary>
        [DllImport("avformat-57", EntryPoint = "av_fmt_ctx_get_duration_estimation_method", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern AVDurationEstimationMethod av_fmt_ctx_get_duration_estimation_method(AVFormatContext* @ctx);
        
        [DllImport("avformat-57", EntryPoint = "av_format_get_audio_codec", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern AVCodec* av_format_get_audio_codec(AVFormatContext* @s);
        
        [DllImport("avformat-57", EntryPoint = "av_format_get_control_message_cb", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern av_format_get_control_message_cb_func av_format_get_control_message_cb(AVFormatContext* @s);
        
        [DllImport("avformat-57", EntryPoint = "av_format_get_data_codec", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern AVCodec* av_format_get_data_codec(AVFormatContext* @s);
        
        [DllImport("avformat-57", EntryPoint = "av_format_get_metadata_header_padding", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern int av_format_get_metadata_header_padding(AVFormatContext* @s);
        
        [DllImport("avformat-57", EntryPoint = "av_format_get_opaque", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern void* av_format_get_opaque(AVFormatContext* @s);
        
        [Obsolete("")]
        [DllImport("avformat-57", EntryPoint = "av_format_get_open_cb", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern av_format_get_open_cb_func av_format_get_open_cb(AVFormatContext* @s);
        
        /// <summary>Accessors for some AVFormatContext fields. These used to be provided for ABI compatibility, and do not need to be used anymore.</summary>
        [DllImport("avformat-57", EntryPoint = "av_format_get_probe_score", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern int av_format_get_probe_score(AVFormatContext* @s);
        
        [DllImport("avformat-57", EntryPoint = "av_format_get_subtitle_codec", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern AVCodec* av_format_get_subtitle_codec(AVFormatContext* @s);
        
        [DllImport("avformat-57", EntryPoint = "av_format_get_video_codec", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern AVCodec* av_format_get_video_codec(AVFormatContext* @s);
        
        /// <summary>This function will cause global side data to be injected in the next packet of each stream as well as after any subsequent seek.</summary>
        [DllImport("avformat-57", EntryPoint = "av_format_inject_global_side_data", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern void av_format_inject_global_side_data(AVFormatContext* @s);
        
        [DllImport("avformat-57", EntryPoint = "av_format_set_audio_codec", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern void av_format_set_audio_codec(AVFormatContext* @s, AVCodec* @c);
        
        [DllImport("avformat-57", EntryPoint = "av_format_set_control_message_cb", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern void av_format_set_control_message_cb(AVFormatContext* @s, av_format_set_control_message_cb_callback_func @callback);
        
        [DllImport("avformat-57", EntryPoint = "av_format_set_data_codec", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern void av_format_set_data_codec(AVFormatContext* @s, AVCodec* @c);
        
        [DllImport("avformat-57", EntryPoint = "av_format_set_metadata_header_padding", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern void av_format_set_metadata_header_padding(AVFormatContext* @s, int @c);
        
        [DllImport("avformat-57", EntryPoint = "av_format_set_opaque", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern void av_format_set_opaque(AVFormatContext* @s, void* @opaque);
        
        [Obsolete("")]
        [DllImport("avformat-57", EntryPoint = "av_format_set_open_cb", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern void av_format_set_open_cb(AVFormatContext* @s, av_format_set_open_cb_callback_func @callback);
        
        [DllImport("avformat-57", EntryPoint = "av_format_set_subtitle_codec", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern void av_format_set_subtitle_codec(AVFormatContext* @s, AVCodec* @c);
        
        [DllImport("avformat-57", EntryPoint = "av_format_set_video_codec", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern void av_format_set_video_codec(AVFormatContext* @s, AVCodec* @c);
        
        [DllImport("avformat-57", EntryPoint = "av_get_frame_filename", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern int av_get_frame_filename(byte* @buf, int @buf_size, [MarshalAs(UnmanagedType.LPStr)] string @path, int @number);
        
        /// <summary>Return in &apos;buf&apos; the path with &apos;%d&apos; replaced by a number.</summary>
        /// <param name="buf">destination buffer</param>
        /// <param name="buf_size">destination buffer size</param>
        /// <param name="path">numbered sequence string</param>
        /// <param name="number">frame number</param>
        /// <param name="flags">AV_FRAME_FILENAME_FLAGS_*</param>
        [DllImport("avformat-57", EntryPoint = "av_get_frame_filename2", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern int av_get_frame_filename2(byte* @buf, int @buf_size, [MarshalAs(UnmanagedType.LPStr)] string @path, int @number, int @flags);
        
        /// <summary>Get timing information for the data currently output. The exact meaning of &quot;currently output&quot; depends on the format. It is mostly relevant for devices that have an internal buffer and/or work in real time.</summary>
        /// <param name="s">media file handle</param>
        /// <param name="stream">stream in the media file</param>
        /// <param name="dts">DTS of the last packet output for the stream, in stream time_base units</param>
        /// <param name="wall">absolute time when that packet whas output, in microsecond</param>
        [DllImport("avformat-57", EntryPoint = "av_get_output_timestamp", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern int av_get_output_timestamp(AVFormatContext* @s, int @stream, long* @dts, long* @wall);
        
        /// <summary>Allocate and read the payload of a packet and initialize its fields with default values.</summary>
        /// <param name="s">associated IO context</param>
        /// <param name="pkt">packet</param>
        /// <param name="size">desired payload size</param>
        [DllImport("avformat-57", EntryPoint = "av_get_packet", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern int av_get_packet(AVIOContext* @s, AVPacket* @pkt, int @size);
        
        /// <summary>Guess the codec ID based upon muxer and filename.</summary>
        [DllImport("avformat-57", EntryPoint = "av_guess_codec", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern AVCodecID av_guess_codec(AVOutputFormat* @fmt, [MarshalAs(UnmanagedType.LPStr)] string @short_name, [MarshalAs(UnmanagedType.LPStr)] string @filename, [MarshalAs(UnmanagedType.LPStr)] string @mime_type, AVMediaType @type);
        
        /// <summary>Return the output format in the list of registered output formats which best matches the provided parameters, or return NULL if there is no match.</summary>
        /// <param name="short_name">if non-NULL checks if short_name matches with the names of the registered formats</param>
        /// <param name="filename">if non-NULL checks if filename terminates with the extensions of the registered formats</param>
        /// <param name="mime_type">if non-NULL checks if mime_type matches with the MIME type of the registered formats</param>
        [DllImport("avformat-57", EntryPoint = "av_guess_format", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern AVOutputFormat* av_guess_format([MarshalAs(UnmanagedType.LPStr)] string @short_name, [MarshalAs(UnmanagedType.LPStr)] string @filename, [MarshalAs(UnmanagedType.LPStr)] string @mime_type);
        
        /// <summary>Guess the frame rate, based on both the container and codec information.</summary>
        /// <param name="ctx">the format context which the stream is part of</param>
        /// <param name="stream">the stream which the frame is part of</param>
        /// <param name="frame">the frame for which the frame rate should be determined, may be NULL</param>
        [DllImport("avformat-57", EntryPoint = "av_guess_frame_rate", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern AVRational av_guess_frame_rate(AVFormatContext* @ctx, AVStream* @stream, AVFrame* @frame);
        
        /// <summary>Guess the sample aspect ratio of a frame, based on both the stream and the frame aspect ratio.</summary>
        /// <param name="format">the format context which the stream is part of</param>
        /// <param name="stream">the stream which the frame is part of</param>
        /// <param name="frame">the frame with the aspect ratio to be determined</param>
        [DllImport("avformat-57", EntryPoint = "av_guess_sample_aspect_ratio", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern AVRational av_guess_sample_aspect_ratio(AVFormatContext* @format, AVStream* @stream, AVFrame* @frame);
        
        /// <summary>Send a nice hexadecimal dump of a buffer to the specified file stream.</summary>
        /// <param name="f">The file stream pointer where the dump should be sent to.</param>
        /// <param name="buf">buffer</param>
        /// <param name="size">buffer size</param>
        [DllImport("avformat-57", EntryPoint = "av_hex_dump", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern void av_hex_dump(_iobuf* @f, byte* @buf, int @size);
        
        /// <summary>Send a nice hexadecimal dump of a buffer to the log.</summary>
        /// <param name="avcl">A pointer to an arbitrary struct of which the first field is a pointer to an AVClass struct.</param>
        /// <param name="level">The importance level of the message, lower values signifying higher importance.</param>
        /// <param name="buf">buffer</param>
        /// <param name="size">buffer size</param>
        [DllImport("avformat-57", EntryPoint = "av_hex_dump_log", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern void av_hex_dump_log(void* @avcl, int @level, byte* @buf, int @size);
        
        /// <summary>If f is NULL, returns the first registered input format, if f is non-NULL, returns the next registered input format after f or NULL if f is the last one.</summary>
        [DllImport("avformat-57", EntryPoint = "av_iformat_next", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern AVInputFormat* av_iformat_next(AVInputFormat* @f);
        
        /// <summary>Get the index for a specific timestamp.</summary>
        /// <param name="st">stream that the timestamp belongs to</param>
        /// <param name="timestamp">timestamp to retrieve the index for</param>
        /// <param name="flags">if AVSEEK_FLAG_BACKWARD then the returned index will correspond to the timestamp which is &lt; = the requested one, if backward is 0, then it will be &gt;= if AVSEEK_FLAG_ANY seek to any frame, only keyframes otherwise</param>
        [DllImport("avformat-57", EntryPoint = "av_index_search_timestamp", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern int av_index_search_timestamp(AVStream* @st, long @timestamp, int @flags);
        
        /// <summary>Write a packet to an output media file ensuring correct interleaving.</summary>
        /// <param name="s">media file handle</param>
        /// <param name="pkt">The packet containing the data to be written.  If the packet is reference-counted, this function will take ownership of this reference and unreference it later when it sees fit. The caller must not access the data through this reference after this function returns. If the packet is not reference-counted, libavformat will make a copy.  This parameter can be NULL (at any time, not just at the end), to flush the interleaving queues.  Packet&apos;s</param>
        [DllImport("avformat-57", EntryPoint = "av_interleaved_write_frame", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern int av_interleaved_write_frame(AVFormatContext* @s, AVPacket* @pkt);
        
        /// <summary>Write an uncoded frame to an output media file.</summary>
        [DllImport("avformat-57", EntryPoint = "av_interleaved_write_uncoded_frame", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern int av_interleaved_write_uncoded_frame(AVFormatContext* @s, int @stream_index, AVFrame* @frame);
        
        /// <summary>Return a positive value if the given filename has one of the given extensions, 0 otherwise.</summary>
        /// <param name="filename">file name to check against the given extensions</param>
        /// <param name="extensions">a comma-separated list of filename extensions</param>
        [DllImport("avformat-57", EntryPoint = "av_match_ext", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern int av_match_ext([MarshalAs(UnmanagedType.LPStr)] string @filename, [MarshalAs(UnmanagedType.LPStr)] string @extensions);
        
        [DllImport("avformat-57", EntryPoint = "av_new_program", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern AVProgram* av_new_program(AVFormatContext* @s, int @id);
        
        /// <summary>If f is NULL, returns the first registered output format, if f is non-NULL, returns the next registered output format after f or NULL if f is the last one.</summary>
        [DllImport("avformat-57", EntryPoint = "av_oformat_next", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern AVOutputFormat* av_oformat_next(AVOutputFormat* @f);
        
        /// <summary>Send a nice dump of a packet to the log.</summary>
        /// <param name="avcl">A pointer to an arbitrary struct of which the first field is a pointer to an AVClass struct.</param>
        /// <param name="level">The importance level of the message, lower values signifying higher importance.</param>
        /// <param name="pkt">packet to dump</param>
        /// <param name="dump_payload">True if the payload must be displayed, too.</param>
        /// <param name="st">AVStream that the packet belongs to</param>
        [DllImport("avformat-57", EntryPoint = "av_pkt_dump_log2", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern void av_pkt_dump_log2(void* @avcl, int @level, AVPacket* @pkt, int @dump_payload, AVStream* @st);
        
        /// <summary>Send a nice dump of a packet to the specified file stream.</summary>
        /// <param name="f">The file stream pointer where the dump should be sent to.</param>
        /// <param name="pkt">packet to dump</param>
        /// <param name="dump_payload">True if the payload must be displayed, too.</param>
        /// <param name="st">AVStream that the packet belongs to</param>
        [DllImport("avformat-57", EntryPoint = "av_pkt_dump2", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern void av_pkt_dump2(_iobuf* @f, AVPacket* @pkt, int @dump_payload, AVStream* @st);
        
        /// <summary>Like av_probe_input_buffer2() but returns 0 on success</summary>
        [DllImport("avformat-57", EntryPoint = "av_probe_input_buffer", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern int av_probe_input_buffer(AVIOContext* @pb, AVInputFormat** @fmt, [MarshalAs(UnmanagedType.LPStr)] string @url, void* @logctx, uint @offset, uint @max_probe_size);
        
        /// <summary>Probe a bytestream to determine the input format. Each time a probe returns with a score that is too low, the probe buffer size is increased and another attempt is made. When the maximum probe size is reached, the input format with the highest score is returned.</summary>
        /// <param name="pb">the bytestream to probe</param>
        /// <param name="fmt">the input format is put here</param>
        /// <param name="url">the url of the stream</param>
        /// <param name="logctx">the log context</param>
        /// <param name="offset">the offset within the bytestream to probe from</param>
        /// <param name="max_probe_size">the maximum probe buffer size (zero for default)</param>
        [DllImport("avformat-57", EntryPoint = "av_probe_input_buffer2", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern int av_probe_input_buffer2(AVIOContext* @pb, AVInputFormat** @fmt, [MarshalAs(UnmanagedType.LPStr)] string @url, void* @logctx, uint @offset, uint @max_probe_size);
        
        /// <summary>Guess the file format.</summary>
        /// <param name="pd">data to be probed</param>
        /// <param name="is_opened">Whether the file is already opened; determines whether demuxers with or without AVFMT_NOFILE are probed.</param>
        [DllImport("avformat-57", EntryPoint = "av_probe_input_format", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern AVInputFormat* av_probe_input_format(AVProbeData* @pd, int @is_opened);
        
        /// <summary>Guess the file format.</summary>
        /// <param name="pd">data to be probed</param>
        /// <param name="is_opened">Whether the file is already opened; determines whether demuxers with or without AVFMT_NOFILE are probed.</param>
        /// <param name="score_max">A probe score larger that this is required to accept a detection, the variable is set to the actual detection score afterwards. If the score is &lt; = AVPROBE_SCORE_MAX / 4 it is recommended to retry with a larger probe buffer.</param>
        [DllImport("avformat-57", EntryPoint = "av_probe_input_format2", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern AVInputFormat* av_probe_input_format2(AVProbeData* @pd, int @is_opened, int* @score_max);
        
        /// <summary>Guess the file format.</summary>
        /// <param name="is_opened">Whether the file is already opened; determines whether demuxers with or without AVFMT_NOFILE are probed.</param>
        /// <param name="score_ret">The score of the best detection.</param>
        [DllImport("avformat-57", EntryPoint = "av_probe_input_format3", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern AVInputFormat* av_probe_input_format3(AVProbeData* @pd, int @is_opened, int* @score_ret);
        
        [DllImport("avformat-57", EntryPoint = "av_program_add_stream_index", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern void av_program_add_stream_index(AVFormatContext* @ac, int @progid, uint @idx);
        
        /// <summary>Return the next frame of a stream. This function returns what is stored in the file, and does not validate that what is there are valid frames for the decoder. It will split what is stored in the file into frames and return one for each call. It will not omit invalid data between valid frames so as to give the decoder the maximum information possible for decoding.</summary>
        [DllImport("avformat-57", EntryPoint = "av_read_frame", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern int av_read_frame(AVFormatContext* @s, AVPacket* @pkt);
        
        /// <summary>Pause a network-based stream (e.g. RTSP stream).</summary>
        [DllImport("avformat-57", EntryPoint = "av_read_pause", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern int av_read_pause(AVFormatContext* @s);
        
        /// <summary>Start playing a network-based stream (e.g. RTSP stream) at the current position.</summary>
        [DllImport("avformat-57", EntryPoint = "av_read_play", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern int av_read_play(AVFormatContext* @s);
        
        /// <summary>Initialize libavformat and register all the muxers, demuxers and protocols. If you do not call this function, then you can select exactly which formats you want to support.</summary>
        [DllImport("avformat-57", EntryPoint = "av_register_all", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern void av_register_all();
        
        [DllImport("avformat-57", EntryPoint = "av_register_input_format", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern void av_register_input_format(AVInputFormat* @format);
        
        [DllImport("avformat-57", EntryPoint = "av_register_output_format", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern void av_register_output_format(AVOutputFormat* @format);
        
        /// <summary>Generate an SDP for an RTP session.</summary>
        /// <param name="ac">array of AVFormatContexts describing the RTP streams. If the array is composed by only one context, such context can contain multiple AVStreams (one AVStream per RTP stream). Otherwise, all the contexts in the array (an AVCodecContext per RTP stream) must contain only one AVStream.</param>
        /// <param name="n_files">number of AVCodecContexts contained in ac</param>
        /// <param name="buf">buffer where the SDP will be stored (must be allocated by the caller)</param>
        /// <param name="size">the size of the buffer</param>
        [DllImport("avformat-57", EntryPoint = "av_sdp_create", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern int av_sdp_create(AVFormatContext*[] @ac, int @n_files, byte* @buf, int @size);
        
        /// <summary>Seek to the keyframe at timestamp. &apos;timestamp&apos; in &apos;stream_index&apos;.</summary>
        /// <param name="s">media file handle</param>
        /// <param name="stream_index">If stream_index is (-1), a default stream is selected, and timestamp is automatically converted from AV_TIME_BASE units to the stream specific time_base.</param>
        /// <param name="timestamp">Timestamp in AVStream.time_base units or, if no stream is specified, in AV_TIME_BASE units.</param>
        /// <param name="flags">flags which select direction and seeking mode</param>
        [DllImport("avformat-57", EntryPoint = "av_seek_frame", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern int av_seek_frame(AVFormatContext* @s, int @stream_index, long @timestamp, int @flags);
        
        /// <summary>Wrap an existing array as stream side data.</summary>
        /// <param name="st">stream</param>
        /// <param name="type">side information type</param>
        /// <param name="data">the side data array. It must be allocated with the av_malloc() family of functions. The ownership of the data is transferred to st.</param>
        /// <param name="size">side information size</param>
        [DllImport("avformat-57", EntryPoint = "av_stream_add_side_data", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern int av_stream_add_side_data(AVStream* @st, AVPacketSideDataType @type, byte* @data, ulong @size);
        
        /// <summary>Get the internal codec timebase from a stream.</summary>
        /// <param name="st">input stream to extract the timebase from</param>
        [DllImport("avformat-57", EntryPoint = "av_stream_get_codec_timebase", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern AVRational av_stream_get_codec_timebase(AVStream* @st);
        
        /// <summary>Returns the pts of the last muxed packet + its duration</summary>
        [DllImport("avformat-57", EntryPoint = "av_stream_get_end_pts", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern long av_stream_get_end_pts(AVStream* @st);
        
        [DllImport("avformat-57", EntryPoint = "av_stream_get_parser", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern AVCodecParserContext* av_stream_get_parser(AVStream* @s);
        
        [DllImport("avformat-57", EntryPoint = "av_stream_get_r_frame_rate", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern AVRational av_stream_get_r_frame_rate(AVStream* @s);
        
        [DllImport("avformat-57", EntryPoint = "av_stream_get_recommended_encoder_configuration", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern byte* av_stream_get_recommended_encoder_configuration(AVStream* @s);
        
        [DllImport("avformat-57", EntryPoint = "av_stream_get_side_data", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern byte* av_stream_get_side_data(AVStream* @stream, AVPacketSideDataType @type, int* @size);
        
        /// <summary>Allocate new information from stream.</summary>
        /// <param name="stream">stream</param>
        /// <param name="type">desired side information type</param>
        /// <param name="size">side information size</param>
        [DllImport("avformat-57", EntryPoint = "av_stream_new_side_data", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern byte* av_stream_new_side_data(AVStream* @stream, AVPacketSideDataType @type, int @size);
        
        [DllImport("avformat-57", EntryPoint = "av_stream_set_r_frame_rate", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern void av_stream_set_r_frame_rate(AVStream* @s, AVRational @r);
        
        [DllImport("avformat-57", EntryPoint = "av_stream_set_recommended_encoder_configuration", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern void av_stream_set_recommended_encoder_configuration(AVStream* @s, byte* @configuration);
        
        /// <summary>Split a URL string into components.</summary>
        /// <param name="proto">the buffer for the protocol</param>
        /// <param name="proto_size">the size of the proto buffer</param>
        /// <param name="authorization">the buffer for the authorization</param>
        /// <param name="authorization_size">the size of the authorization buffer</param>
        /// <param name="hostname">the buffer for the host name</param>
        /// <param name="hostname_size">the size of the hostname buffer</param>
        /// <param name="port_ptr">a pointer to store the port number in</param>
        /// <param name="path">the buffer for the path</param>
        /// <param name="path_size">the size of the path buffer</param>
        /// <param name="url">the URL to split</param>
        [DllImport("avformat-57", EntryPoint = "av_url_split", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern void av_url_split(byte* @proto, int @proto_size, byte* @authorization, int @authorization_size, byte* @hostname, int @hostname_size, int* @port_ptr, byte* @path, int @path_size, [MarshalAs(UnmanagedType.LPStr)] string @url);
        
        /// <summary>Write a packet to an output media file.</summary>
        /// <param name="s">media file handle</param>
        /// <param name="pkt">The packet containing the data to be written. Note that unlike av_interleaved_write_frame(), this function does not take ownership of the packet passed to it (though some muxers may make an internal reference to the input packet).  This parameter can be NULL (at any time, not just at the end), in order to immediately flush data buffered within the muxer, for muxers that buffer up data internally before writing it to the output.  Packet&apos;s</param>
        [DllImport("avformat-57", EntryPoint = "av_write_frame", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern int av_write_frame(AVFormatContext* @s, AVPacket* @pkt);
        
        /// <summary>Write the stream trailer to an output media file and free the file private data.</summary>
        /// <param name="s">media file handle</param>
        [DllImport("avformat-57", EntryPoint = "av_write_trailer", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern int av_write_trailer(AVFormatContext* @s);
        
        /// <summary>Write an uncoded frame to an output media file.</summary>
        [DllImport("avformat-57", EntryPoint = "av_write_uncoded_frame", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern int av_write_uncoded_frame(AVFormatContext* @s, int @stream_index, AVFrame* @frame);
        
        /// <summary>Test whether a muxer supports uncoded frame.</summary>
        [DllImport("avformat-57", EntryPoint = "av_write_uncoded_frame_query", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern int av_write_uncoded_frame_query(AVFormatContext* @s, int @stream_index);
        
        /// <summary>Allocate an AVFormatContext. avformat_free_context() can be used to free the context and everything allocated by the framework within it.</summary>
        [DllImport("avformat-57", EntryPoint = "avformat_alloc_context", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern AVFormatContext* avformat_alloc_context();
        
        /// <summary>Allocate an AVFormatContext for an output format. avformat_free_context() can be used to free the context and everything allocated by the framework within it.</summary>
        /// <param name="oformat">format to use for allocating the context, if NULL format_name and filename are used instead</param>
        /// <param name="format_name">the name of output format to use for allocating the context, if NULL filename is used instead</param>
        /// <param name="filename">the name of the filename to use for allocating the context, may be NULL</param>
        [DllImport("avformat-57", EntryPoint = "avformat_alloc_output_context2", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern int avformat_alloc_output_context2(AVFormatContext** @ctx, AVOutputFormat* @oformat, [MarshalAs(UnmanagedType.LPStr)] string @format_name, [MarshalAs(UnmanagedType.LPStr)] string @filename);
        
        /// <summary>Close an opened input AVFormatContext. Free it and all its contents and set *s to NULL.</summary>
        [DllImport("avformat-57", EntryPoint = "avformat_close_input", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern void avformat_close_input(AVFormatContext** @s);
        
        /// <summary>Return the libavformat build-time configuration.</summary>
        [DllImport("avformat-57", EntryPoint = "avformat_configuration", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        [return: MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof(ConstCharPtrMarshaler))]
        public static extern string avformat_configuration();
        
        /// <summary>Read packets of a media file to get stream information. This is useful for file formats with no headers such as MPEG. This function also computes the real framerate in case of MPEG-2 repeat frame mode. The logical file position is not changed by this function; examined packets may be buffered for later processing.</summary>
        /// <param name="ic">media file handle</param>
        /// <param name="options">If non-NULL, an ic.nb_streams long array of pointers to dictionaries, where i-th member contains options for codec corresponding to i-th stream. On return each dictionary will be filled with options that were not found.</param>
        [DllImport("avformat-57", EntryPoint = "avformat_find_stream_info", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern int avformat_find_stream_info(AVFormatContext* @ic, AVDictionary** @options);
        
        /// <summary>Discard all internally buffered data. This can be useful when dealing with discontinuities in the byte stream. Generally works only with formats that can resync. This includes headerless formats like MPEG-TS/TS but should also work with NUT, Ogg and in a limited way AVI for example.</summary>
        /// <param name="s">media file handle</param>
        [DllImport("avformat-57", EntryPoint = "avformat_flush", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern int avformat_flush(AVFormatContext* @s);
        
        /// <summary>Free an AVFormatContext and all its streams.</summary>
        /// <param name="s">context to free</param>
        [DllImport("avformat-57", EntryPoint = "avformat_free_context", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern void avformat_free_context(AVFormatContext* @s);
        
        /// <summary>Get the AVClass for AVFormatContext. It can be used in combination with AV_OPT_SEARCH_FAKE_OBJ for examining options.</summary>
        [DllImport("avformat-57", EntryPoint = "avformat_get_class", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern AVClass* avformat_get_class();
        
        /// <summary>Returns the table mapping MOV FourCCs for audio to AVCodecID.</summary>
        [DllImport("avformat-57", EntryPoint = "avformat_get_mov_audio_tags", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern AVCodecTag* avformat_get_mov_audio_tags();
        
        /// <summary>Returns the table mapping MOV FourCCs for video to libavcodec AVCodecID.</summary>
        [DllImport("avformat-57", EntryPoint = "avformat_get_mov_video_tags", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern AVCodecTag* avformat_get_mov_video_tags();
        
        /// <summary>Returns the table mapping RIFF FourCCs for audio to AVCodecID.</summary>
        [DllImport("avformat-57", EntryPoint = "avformat_get_riff_audio_tags", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern AVCodecTag* avformat_get_riff_audio_tags();
        
        /// <summary>@{ Get the tables mapping RIFF FourCCs to libavcodec AVCodecIDs. The tables are meant to be passed to av_codec_get_id()/av_codec_get_tag() as in the following code:</summary>
        [DllImport("avformat-57", EntryPoint = "avformat_get_riff_video_tags", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern AVCodecTag* avformat_get_riff_video_tags();
        
        /// <summary>Allocate the stream private data and initialize the codec, but do not write the header. May optionally be used before avformat_write_header to initialize stream parameters before actually writing the header. If using this function, do not pass the same options to avformat_write_header.</summary>
        /// <param name="s">Media file handle, must be allocated with avformat_alloc_context(). Its oformat field must be set to the desired output format; Its pb field must be set to an already opened AVIOContext.</param>
        /// <param name="options">An AVDictionary filled with AVFormatContext and muxer-private options. On return this parameter will be destroyed and replaced with a dict containing options that were not found. May be NULL.</param>
        [DllImport("avformat-57", EntryPoint = "avformat_init_output", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern int avformat_init_output(AVFormatContext* @s, AVDictionary** @options);
        
        /// <summary>Return the libavformat license.</summary>
        [DllImport("avformat-57", EntryPoint = "avformat_license", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        [return: MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof(ConstCharPtrMarshaler))]
        public static extern string avformat_license();
        
        /// <summary>Check if the stream st contained in s is matched by the stream specifier spec.</summary>
        [DllImport("avformat-57", EntryPoint = "avformat_match_stream_specifier", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern int avformat_match_stream_specifier(AVFormatContext* @s, AVStream* @st, [MarshalAs(UnmanagedType.LPStr)] string @spec);
        
        /// <summary>Undo the initialization done by avformat_network_init.</summary>
        [DllImport("avformat-57", EntryPoint = "avformat_network_deinit", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern int avformat_network_deinit();
        
        /// <summary>Do global initialization of network components. This is optional, but recommended, since it avoids the overhead of implicitly doing the setup for each session.</summary>
        [DllImport("avformat-57", EntryPoint = "avformat_network_init", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern int avformat_network_init();
        
        /// <summary>Add a new stream to a media file.</summary>
        /// <param name="s">media file handle</param>
        /// <param name="c">If non-NULL, the AVCodecContext corresponding to the new stream will be initialized to use this codec. This is needed for e.g. codec-specific defaults to be set, so codec should be provided if it is known.</param>
        [DllImport("avformat-57", EntryPoint = "avformat_new_stream", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern AVStream* avformat_new_stream(AVFormatContext* @s, AVCodec* @c);
        
        /// <summary>Open an input stream and read the header. The codecs are not opened. The stream must be closed with avformat_close_input().</summary>
        /// <param name="ps">Pointer to user-supplied AVFormatContext (allocated by avformat_alloc_context). May be a pointer to NULL, in which case an AVFormatContext is allocated by this function and written into ps. Note that a user-supplied AVFormatContext will be freed on failure.</param>
        /// <param name="url">URL of the stream to open.</param>
        /// <param name="fmt">If non-NULL, this parameter forces a specific input format. Otherwise the format is autodetected.</param>
        /// <param name="options">A dictionary filled with AVFormatContext and demuxer-private options. On return this parameter will be destroyed and replaced with a dict containing options that were not found. May be NULL.</param>
        [DllImport("avformat-57", EntryPoint = "avformat_open_input", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern int avformat_open_input(AVFormatContext** @ps, [MarshalAs(UnmanagedType.LPStr)] string @url, AVInputFormat* @fmt, AVDictionary** @options);
        
        /// <summary>Test if the given container can store a codec.</summary>
        /// <param name="ofmt">container to check for compatibility</param>
        /// <param name="codec_id">codec to potentially store in container</param>
        /// <param name="std_compliance">standards compliance level, one of FF_COMPLIANCE_*</param>
        [DllImport("avformat-57", EntryPoint = "avformat_query_codec", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern int avformat_query_codec(AVOutputFormat* @ofmt, AVCodecID @codec_id, int @std_compliance);
        
        [DllImport("avformat-57", EntryPoint = "avformat_queue_attached_pictures", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern int avformat_queue_attached_pictures(AVFormatContext* @s);
        
        /// <summary>Seek to timestamp ts. Seeking will be done so that the point from which all active streams can be presented successfully will be closest to ts and within min/max_ts. Active streams are all streams that have AVStream.discard &lt; AVDISCARD_ALL.</summary>
        /// <param name="s">media file handle</param>
        /// <param name="stream_index">index of the stream which is used as time base reference</param>
        /// <param name="min_ts">smallest acceptable timestamp</param>
        /// <param name="ts">target timestamp</param>
        /// <param name="max_ts">largest acceptable timestamp</param>
        /// <param name="flags">flags</param>
        [DllImport("avformat-57", EntryPoint = "avformat_seek_file", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern int avformat_seek_file(AVFormatContext* @s, int @stream_index, long @min_ts, long @ts, long @max_ts, int @flags);
        
        /// <summary>Transfer internal timing information from one stream to another.</summary>
        /// <param name="ofmt">target output format for ost</param>
        /// <param name="ost">output stream which needs timings copy and adjustments</param>
        /// <param name="ist">reference input stream to copy timings from</param>
        /// <param name="copy_tb">define from where the stream codec timebase needs to be imported</param>
        [DllImport("avformat-57", EntryPoint = "avformat_transfer_internal_stream_timing_info", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern int avformat_transfer_internal_stream_timing_info(AVOutputFormat* @ofmt, AVStream* @ost, AVStream* @ist, AVTimebaseSource @copy_tb);
        
        /// <summary>Return the LIBAVFORMAT_VERSION_INT constant.</summary>
        [DllImport("avformat-57", EntryPoint = "avformat_version", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern uint avformat_version();
        
        /// <summary>Allocate the stream private data and write the stream header to an output media file.</summary>
        /// <param name="s">Media file handle, must be allocated with avformat_alloc_context(). Its oformat field must be set to the desired output format; Its pb field must be set to an already opened AVIOContext.</param>
        /// <param name="options">An AVDictionary filled with AVFormatContext and muxer-private options. On return this parameter will be destroyed and replaced with a dict containing options that were not found. May be NULL.</param>
        [DllImport("avformat-57", EntryPoint = "avformat_write_header", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern int avformat_write_header(AVFormatContext* @s, AVDictionary** @options);
        
        /// <summary>Accept and allocate a client context on a server context.</summary>
        /// <param name="s">the server context</param>
        /// <param name="c">the client context, must be unallocated</param>
        [DllImport("avformat-57", EntryPoint = "avio_accept", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern int avio_accept(AVIOContext* @s, AVIOContext** @c);
        
        /// <summary>Allocate and initialize an AVIOContext for buffered I/O. It must be later freed with av_free().</summary>
        /// <param name="buffer">Memory block for input/output operations via AVIOContext. The buffer must be allocated with av_malloc() and friends. It may be freed and replaced with a new buffer by libavformat. AVIOContext.buffer holds the buffer currently in use, which must be later freed with av_free().</param>
        /// <param name="buffer_size">The buffer size is very important for performance. For protocols with fixed blocksize it should be set to this blocksize. For others a typical size is a cache page, e.g. 4kb.</param>
        /// <param name="write_flag">Set to 1 if the buffer should be writable, 0 otherwise.</param>
        /// <param name="opaque">An opaque pointer to user-specific data.</param>
        /// <param name="read_packet">A function for refilling the buffer, may be NULL.</param>
        /// <param name="write_packet">A function for writing the buffer contents, may be NULL. The function may not change the input buffers content.</param>
        /// <param name="seek">A function for seeking to specified byte position, may be NULL.</param>
        [DllImport("avformat-57", EntryPoint = "avio_alloc_context", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern AVIOContext* avio_alloc_context(byte* @buffer, int @buffer_size, int @write_flag, void* @opaque, avio_alloc_context_read_packet_func @read_packet, avio_alloc_context_write_packet_func @write_packet, avio_alloc_context_seek_func @seek);
        
        /// <summary>Return AVIO_FLAG_* access flags corresponding to the access permissions of the resource in url, or a negative value corresponding to an AVERROR code in case of failure. The returned access flags are masked by the value in flags.</summary>
        [DllImport("avformat-57", EntryPoint = "avio_check", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern int avio_check([MarshalAs(UnmanagedType.LPStr)] string @url, int @flags);
        
        /// <summary>Close the resource accessed by the AVIOContext s and free it. This function can only be used if s was opened by avio_open().</summary>
        [DllImport("avformat-57", EntryPoint = "avio_close", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern int avio_close(AVIOContext* @s);
        
        /// <summary>Close directory.</summary>
        /// <param name="s">directory read context.</param>
        [DllImport("avformat-57", EntryPoint = "avio_close_dir", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern int avio_close_dir(AVIODirContext** @s);
        
        /// <summary>Return the written size and a pointer to the buffer. The buffer must be freed with av_free(). Padding of AV_INPUT_BUFFER_PADDING_SIZE is added to the buffer.</summary>
        /// <param name="s">IO context</param>
        /// <param name="pbuffer">pointer to a byte buffer</param>
        [DllImport("avformat-57", EntryPoint = "avio_close_dyn_buf", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern int avio_close_dyn_buf(AVIOContext* @s, byte** @pbuffer);
        
        /// <summary>Close the resource accessed by the AVIOContext *s, free it and set the pointer pointing to it to NULL. This function can only be used if s was opened by avio_open().</summary>
        [DllImport("avformat-57", EntryPoint = "avio_closep", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern int avio_closep(AVIOContext** @s);
        
        /// <summary>Iterate through names of available protocols.</summary>
        /// <param name="opaque">A private pointer representing current protocol. It must be a pointer to NULL on first iteration and will be updated by successive calls to avio_enum_protocols.</param>
        /// <param name="output">If set to 1, iterate over output protocols, otherwise over input protocols.</param>
        [DllImport("avformat-57", EntryPoint = "avio_enum_protocols", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        [return: MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof(ConstCharPtrMarshaler))]
        public static extern string avio_enum_protocols(void** @opaque, int @output);
        
        /// <summary>feof() equivalent for AVIOContext.</summary>
        [DllImport("avformat-57", EntryPoint = "avio_feof", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern int avio_feof(AVIOContext* @s);
        
        /// <summary>Return the name of the protocol that will handle the passed URL.</summary>
        [DllImport("avformat-57", EntryPoint = "avio_find_protocol_name", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        [return: MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof(ConstCharPtrMarshaler))]
        public static extern string avio_find_protocol_name([MarshalAs(UnmanagedType.LPStr)] string @url);
        
        /// <summary>Force flushing of buffered data.</summary>
        [DllImport("avformat-57", EntryPoint = "avio_flush", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern void avio_flush(AVIOContext* @s);
        
        /// <summary>Free entry allocated by avio_read_dir().</summary>
        /// <param name="entry">entry to be freed.</param>
        [DllImport("avformat-57", EntryPoint = "avio_free_directory_entry", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern void avio_free_directory_entry(AVIODirEntry** @entry);
        
        /// <summary>Return the written size and a pointer to the buffer. The AVIOContext stream is left intact. The buffer must NOT be freed. No padding is added to the buffer.</summary>
        /// <param name="s">IO context</param>
        /// <param name="pbuffer">pointer to a byte buffer</param>
        [DllImport("avformat-57", EntryPoint = "avio_get_dyn_buf", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern int avio_get_dyn_buf(AVIOContext* @s, byte** @pbuffer);
        
        /// <summary>Read a string from pb into buf. The reading will terminate when either a NULL character was encountered, maxlen bytes have been read, or nothing more can be read from pb. The result is guaranteed to be NULL-terminated, it will be truncated if buf is too small. Note that the string is not interpreted or validated in any way, it might get truncated in the middle of a sequence for multi-byte encodings.</summary>
        [DllImport("avformat-57", EntryPoint = "avio_get_str", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern int avio_get_str(AVIOContext* @pb, int @maxlen, byte* @buf, int @buflen);
        
        [DllImport("avformat-57", EntryPoint = "avio_get_str16be", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern int avio_get_str16be(AVIOContext* @pb, int @maxlen, byte* @buf, int @buflen);
        
        /// <summary>Read a UTF-16 string from pb and convert it to UTF-8. The reading will terminate when either a null or invalid character was encountered or maxlen bytes have been read.</summary>
        [DllImport("avformat-57", EntryPoint = "avio_get_str16le", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern int avio_get_str16le(AVIOContext* @pb, int @maxlen, byte* @buf, int @buflen);
        
        /// <summary>Perform one step of the protocol handshake to accept a new client. This function must be called on a client returned by avio_accept() before using it as a read/write context. It is separate from avio_accept() because it may block. A step of the handshake is defined by places where the application may decide to change the proceedings. For example, on a protocol with a request header and a reply header, each one can constitute a step because the application may use the parameters from the request to change parameters in the reply; or each individual chunk of the request can constitute a step. If the handshake is already finished, avio_handshake() does nothing and returns 0 immediately.</summary>
        /// <param name="c">the client context to perform the handshake on</param>
        [DllImport("avformat-57", EntryPoint = "avio_handshake", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern int avio_handshake(AVIOContext* @c);
        
        /// <summary>Create and initialize a AVIOContext for accessing the resource indicated by url.</summary>
        /// <param name="s">Used to return the pointer to the created AVIOContext. In case of failure the pointed to value is set to NULL.</param>
        /// <param name="url">resource to access</param>
        /// <param name="flags">flags which control how the resource indicated by url is to be opened</param>
        [DllImport("avformat-57", EntryPoint = "avio_open", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern int avio_open(AVIOContext** @s, [MarshalAs(UnmanagedType.LPStr)] string @url, int @flags);
        
        /// <summary>Open directory for reading.</summary>
        /// <param name="s">directory read context. Pointer to a NULL pointer must be passed.</param>
        /// <param name="url">directory to be listed.</param>
        /// <param name="options">A dictionary filled with protocol-private options. On return this parameter will be destroyed and replaced with a dictionary containing options that were not found. May be NULL.</param>
        [DllImport("avformat-57", EntryPoint = "avio_open_dir", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern int avio_open_dir(AVIODirContext** @s, [MarshalAs(UnmanagedType.LPStr)] string @url, AVDictionary** @options);
        
        /// <summary>Open a write only memory stream.</summary>
        /// <param name="s">new IO context</param>
        [DllImport("avformat-57", EntryPoint = "avio_open_dyn_buf", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern int avio_open_dyn_buf(AVIOContext** @s);
        
        /// <summary>Create and initialize a AVIOContext for accessing the resource indicated by url.</summary>
        /// <param name="s">Used to return the pointer to the created AVIOContext. In case of failure the pointed to value is set to NULL.</param>
        /// <param name="url">resource to access</param>
        /// <param name="flags">flags which control how the resource indicated by url is to be opened</param>
        /// <param name="int_cb">an interrupt callback to be used at the protocols level</param>
        /// <param name="options">A dictionary filled with protocol-private options. On return this parameter will be destroyed and replaced with a dict containing options that were not found. May be NULL.</param>
        [DllImport("avformat-57", EntryPoint = "avio_open2", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern int avio_open2(AVIOContext** @s, [MarshalAs(UnmanagedType.LPStr)] string @url, int @flags, AVIOInterruptCB* @int_cb, AVDictionary** @options);
        
        /// <summary>Pause and resume playing - only meaningful if using a network streaming protocol (e.g. MMS).</summary>
        /// <param name="h">IO context from which to call the read_pause function pointer</param>
        /// <param name="pause">1 for pause, 0 for resume</param>
        [DllImport("avformat-57", EntryPoint = "avio_pause", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern int avio_pause(AVIOContext* @h, int @pause);
        
        [DllImport("avformat-57", EntryPoint = "avio_printf", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern int avio_printf(AVIOContext* @s, [MarshalAs(UnmanagedType.LPStr)] string @fmt);
        
        /// <summary>Write a NULL-terminated string.</summary>
        [DllImport("avformat-57", EntryPoint = "avio_put_str", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern int avio_put_str(AVIOContext* @s, [MarshalAs(UnmanagedType.LPStr)] string @str);
        
        /// <summary>Convert an UTF-8 string to UTF-16BE and write it.</summary>
        /// <param name="s">the AVIOContext</param>
        /// <param name="str">NULL-terminated UTF-8 string</param>
        [DllImport("avformat-57", EntryPoint = "avio_put_str16be", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern int avio_put_str16be(AVIOContext* @s, [MarshalAs(UnmanagedType.LPStr)] string @str);
        
        /// <summary>Convert an UTF-8 string to UTF-16LE and write it.</summary>
        /// <param name="s">the AVIOContext</param>
        /// <param name="str">NULL-terminated UTF-8 string</param>
        [DllImport("avformat-57", EntryPoint = "avio_put_str16le", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern int avio_put_str16le(AVIOContext* @s, [MarshalAs(UnmanagedType.LPStr)] string @str);
        
        /// <summary>@{</summary>
        [DllImport("avformat-57", EntryPoint = "avio_r8", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern int avio_r8(AVIOContext* @s);
        
        [DllImport("avformat-57", EntryPoint = "avio_rb16", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern uint avio_rb16(AVIOContext* @s);
        
        [DllImport("avformat-57", EntryPoint = "avio_rb24", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern uint avio_rb24(AVIOContext* @s);
        
        [DllImport("avformat-57", EntryPoint = "avio_rb32", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern uint avio_rb32(AVIOContext* @s);
        
        [DllImport("avformat-57", EntryPoint = "avio_rb64", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern ulong avio_rb64(AVIOContext* @s);
        
        /// <summary>Read size bytes from AVIOContext into buf.</summary>
        [DllImport("avformat-57", EntryPoint = "avio_read", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern int avio_read(AVIOContext* @s, byte* @buf, int @size);
        
        /// <summary>Get next directory entry.</summary>
        /// <param name="s">directory read context.</param>
        /// <param name="next">next entry or NULL when no more entries.</param>
        [DllImport("avformat-57", EntryPoint = "avio_read_dir", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern int avio_read_dir(AVIODirContext* @s, AVIODirEntry** @next);
        
        /// <summary>Read contents of h into print buffer, up to max_size bytes, or up to EOF.</summary>
        [DllImport("avformat-57", EntryPoint = "avio_read_to_bprint", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern int avio_read_to_bprint(AVIOContext* @h, AVBPrint* @pb, ulong @max_size);
        
        [DllImport("avformat-57", EntryPoint = "avio_rl16", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern uint avio_rl16(AVIOContext* @s);
        
        [DllImport("avformat-57", EntryPoint = "avio_rl24", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern uint avio_rl24(AVIOContext* @s);
        
        [DllImport("avformat-57", EntryPoint = "avio_rl32", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern uint avio_rl32(AVIOContext* @s);
        
        [DllImport("avformat-57", EntryPoint = "avio_rl64", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern ulong avio_rl64(AVIOContext* @s);
        
        /// <summary>fseek() equivalent for AVIOContext.</summary>
        [DllImport("avformat-57", EntryPoint = "avio_seek", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern long avio_seek(AVIOContext* @s, long @offset, int @whence);
        
        /// <summary>Seek to a given timestamp relative to some component stream. Only meaningful if using a network streaming protocol (e.g. MMS.).</summary>
        /// <param name="h">IO context from which to call the seek function pointers</param>
        /// <param name="stream_index">The stream index that the timestamp is relative to. If stream_index is (-1) the timestamp should be in AV_TIME_BASE units from the beginning of the presentation. If a stream_index &gt;= 0 is used and the protocol does not support seeking based on component streams, the call will fail.</param>
        /// <param name="timestamp">timestamp in AVStream.time_base units or if there is no stream specified then in AV_TIME_BASE units.</param>
        /// <param name="flags">Optional combination of AVSEEK_FLAG_BACKWARD, AVSEEK_FLAG_BYTE and AVSEEK_FLAG_ANY. The protocol may silently ignore AVSEEK_FLAG_BACKWARD and AVSEEK_FLAG_ANY, but AVSEEK_FLAG_BYTE will fail if used and not supported.</param>
        [DllImport("avformat-57", EntryPoint = "avio_seek_time", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern long avio_seek_time(AVIOContext* @h, int @stream_index, long @timestamp, int @flags);
        
        /// <summary>Get the filesize.</summary>
        [DllImport("avformat-57", EntryPoint = "avio_size", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern long avio_size(AVIOContext* @s);
        
        /// <summary>Skip given number of bytes forward</summary>
        [DllImport("avformat-57", EntryPoint = "avio_skip", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern long avio_skip(AVIOContext* @s, long @offset);
        
        [DllImport("avformat-57", EntryPoint = "avio_w8", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern void avio_w8(AVIOContext* @s, int @b);
        
        [DllImport("avformat-57", EntryPoint = "avio_wb16", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern void avio_wb16(AVIOContext* @s, uint @val);
        
        [DllImport("avformat-57", EntryPoint = "avio_wb24", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern void avio_wb24(AVIOContext* @s, uint @val);
        
        [DllImport("avformat-57", EntryPoint = "avio_wb32", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern void avio_wb32(AVIOContext* @s, uint @val);
        
        [DllImport("avformat-57", EntryPoint = "avio_wb64", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern void avio_wb64(AVIOContext* @s, ulong @val);
        
        [DllImport("avformat-57", EntryPoint = "avio_wl16", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern void avio_wl16(AVIOContext* @s, uint @val);
        
        [DllImport("avformat-57", EntryPoint = "avio_wl24", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern void avio_wl24(AVIOContext* @s, uint @val);
        
        [DllImport("avformat-57", EntryPoint = "avio_wl32", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern void avio_wl32(AVIOContext* @s, uint @val);
        
        [DllImport("avformat-57", EntryPoint = "avio_wl64", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern void avio_wl64(AVIOContext* @s, ulong @val);
        
        [DllImport("avformat-57", EntryPoint = "avio_write", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern void avio_write(AVIOContext* @s, byte* @buf, int @size);
        
        /// <summary>Mark the written bytestream as a specific type.</summary>
        /// <param name="time">the stream time the current bytestream pos corresponds to (in AV_TIME_BASE units), or AV_NOPTS_VALUE if unknown or not applicable</param>
        /// <param name="type">the kind of data written starting at the current pos</param>
        [DllImport("avformat-57", EntryPoint = "avio_write_marker", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern void avio_write_marker(AVIOContext* @s, long @time, AVIODataMarkerType @type);
        
        /// <summary>Delete a resource.</summary>
        /// <param name="url">resource to be deleted.</param>
        [DllImport("avformat-57", EntryPoint = "avpriv_io_delete", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern int avpriv_io_delete([MarshalAs(UnmanagedType.LPStr)] string @url);
        
        /// <summary>Move or rename a resource.</summary>
        /// <param name="url_src">url to resource to be moved</param>
        /// <param name="url_dst">new url to resource if the operation succeeded</param>
        [DllImport("avformat-57", EntryPoint = "avpriv_io_move", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern int avpriv_io_move([MarshalAs(UnmanagedType.LPStr)] string @url_src, [MarshalAs(UnmanagedType.LPStr)] string @url_dst);
        
        [Obsolete("use avio_feof()")]
        [DllImport("avformat-57", EntryPoint = "url_feof", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern int url_feof(AVIOContext* @s);
        
        /// <summary>Add two rationals.</summary>
        /// <param name="b">First rational</param>
        /// <param name="c">Second rational</param>
        [DllImport("avutil-55", EntryPoint = "av_add_q", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern AVRational av_add_q(AVRational @b, AVRational @c);
        
        /// <summary>Add a value to a timestamp.</summary>
        /// <param name="ts_tb">Input timestamp time base</param>
        /// <param name="ts">Input timestamp</param>
        /// <param name="inc_tb">Time base of `inc`</param>
        /// <param name="inc">Value to be added</param>
        [DllImport("avutil-55", EntryPoint = "av_add_stable", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern long av_add_stable(AVRational @ts_tb, long @ts, AVRational @inc_tb, long @inc);
        
        /// <summary>Allocate an AVAudioFifo.</summary>
        /// <param name="sample_fmt">sample format</param>
        /// <param name="channels">number of channels</param>
        /// <param name="nb_samples">initial allocation size, in samples</param>
        [DllImport("avutil-55", EntryPoint = "av_audio_fifo_alloc", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern AVAudioFifo* av_audio_fifo_alloc(AVSampleFormat @sample_fmt, int @channels, int @nb_samples);
        
        /// <summary>Drain data from an AVAudioFifo.</summary>
        /// <param name="af">AVAudioFifo to drain</param>
        /// <param name="nb_samples">number of samples to drain</param>
        [DllImport("avutil-55", EntryPoint = "av_audio_fifo_drain", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern int av_audio_fifo_drain(AVAudioFifo* @af, int @nb_samples);
        
        /// <summary>Free an AVAudioFifo.</summary>
        /// <param name="af">AVAudioFifo to free</param>
        [DllImport("avutil-55", EntryPoint = "av_audio_fifo_free", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern void av_audio_fifo_free(AVAudioFifo* @af);
        
        /// <summary>Peek data from an AVAudioFifo.</summary>
        /// <param name="af">AVAudioFifo to read from</param>
        /// <param name="data">audio data plane pointers</param>
        /// <param name="nb_samples">number of samples to peek</param>
        [DllImport("avutil-55", EntryPoint = "av_audio_fifo_peek", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern int av_audio_fifo_peek(AVAudioFifo* @af, void** @data, int @nb_samples);
        
        /// <summary>Peek data from an AVAudioFifo.</summary>
        /// <param name="af">AVAudioFifo to read from</param>
        /// <param name="data">audio data plane pointers</param>
        /// <param name="nb_samples">number of samples to peek</param>
        /// <param name="offset">offset from current read position</param>
        [DllImport("avutil-55", EntryPoint = "av_audio_fifo_peek_at", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern int av_audio_fifo_peek_at(AVAudioFifo* @af, void** @data, int @nb_samples, int @offset);
        
        /// <summary>Read data from an AVAudioFifo.</summary>
        /// <param name="af">AVAudioFifo to read from</param>
        /// <param name="data">audio data plane pointers</param>
        /// <param name="nb_samples">number of samples to read</param>
        [DllImport("avutil-55", EntryPoint = "av_audio_fifo_read", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern int av_audio_fifo_read(AVAudioFifo* @af, void** @data, int @nb_samples);
        
        /// <summary>Reallocate an AVAudioFifo.</summary>
        /// <param name="af">AVAudioFifo to reallocate</param>
        /// <param name="nb_samples">new allocation size, in samples</param>
        [DllImport("avutil-55", EntryPoint = "av_audio_fifo_realloc", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern int av_audio_fifo_realloc(AVAudioFifo* @af, int @nb_samples);
        
        /// <summary>Reset the AVAudioFifo buffer.</summary>
        /// <param name="af">AVAudioFifo to reset</param>
        [DllImport("avutil-55", EntryPoint = "av_audio_fifo_reset", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern void av_audio_fifo_reset(AVAudioFifo* @af);
        
        /// <summary>Get the current number of samples in the AVAudioFifo available for reading.</summary>
        /// <param name="af">the AVAudioFifo to query</param>
        [DllImport("avutil-55", EntryPoint = "av_audio_fifo_size", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern int av_audio_fifo_size(AVAudioFifo* @af);
        
        /// <summary>Get the current number of samples in the AVAudioFifo available for writing.</summary>
        /// <param name="af">the AVAudioFifo to query</param>
        [DllImport("avutil-55", EntryPoint = "av_audio_fifo_space", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern int av_audio_fifo_space(AVAudioFifo* @af);
        
        /// <summary>Write data to an AVAudioFifo.</summary>
        /// <param name="af">AVAudioFifo to write to</param>
        /// <param name="data">audio data plane pointers</param>
        /// <param name="nb_samples">number of samples to write</param>
        [DllImport("avutil-55", EntryPoint = "av_audio_fifo_write", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern int av_audio_fifo_write(AVAudioFifo* @af, void** @data, int @nb_samples);
        
        /// <summary>Append a description of a channel layout to a bprint buffer.</summary>
        [DllImport("avutil-55", EntryPoint = "av_bprint_channel_layout", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern void av_bprint_channel_layout(AVBPrint* @bp, int @nb_channels, ulong @channel_layout);
        
        /// <summary>Allocate an AVBuffer of the given size using av_malloc().</summary>
        [DllImport("avutil-55", EntryPoint = "av_buffer_alloc", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern AVBufferRef* av_buffer_alloc(int @size);
        
        /// <summary>Same as av_buffer_alloc(), except the returned buffer will be initialized to zero.</summary>
        [DllImport("avutil-55", EntryPoint = "av_buffer_allocz", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern AVBufferRef* av_buffer_allocz(int @size);
        
        /// <summary>Create an AVBuffer from an existing array.</summary>
        /// <param name="data">data array</param>
        /// <param name="size">size of data in bytes</param>
        /// <param name="free">a callback for freeing this buffer&apos;s data</param>
        /// <param name="opaque">parameter to be got for processing or passed to free</param>
        /// <param name="flags">a combination of AV_BUFFER_FLAG_*</param>
        [DllImport("avutil-55", EntryPoint = "av_buffer_create", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern AVBufferRef* av_buffer_create(byte* @data, int @size, av_buffer_create_free_func @free, void* @opaque, int @flags);
        
        /// <summary>Default free callback, which calls av_free() on the buffer data. This function is meant to be passed to av_buffer_create(), not called directly.</summary>
        [DllImport("avutil-55", EntryPoint = "av_buffer_default_free", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern void av_buffer_default_free(void* @opaque, byte* @data);
        
        /// <summary>Returns the opaque parameter set by av_buffer_create.</summary>
        [DllImport("avutil-55", EntryPoint = "av_buffer_get_opaque", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern void* av_buffer_get_opaque(AVBufferRef* @buf);
        
        [DllImport("avutil-55", EntryPoint = "av_buffer_get_ref_count", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern int av_buffer_get_ref_count(AVBufferRef* @buf);
        
        /// <summary>Returns 1 if the caller may write to the data referred to by buf (which is true if and only if buf is the only reference to the underlying AVBuffer). Return 0 otherwise. A positive answer is valid until av_buffer_ref() is called on buf.</summary>
        [DllImport("avutil-55", EntryPoint = "av_buffer_is_writable", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern int av_buffer_is_writable(AVBufferRef* @buf);
        
        /// <summary>Create a writable reference from a given buffer reference, avoiding data copy if possible.</summary>
        /// <param name="buf">buffer reference to make writable. On success, buf is either left untouched, or it is unreferenced and a new writable AVBufferRef is written in its place. On failure, buf is left untouched.</param>
        [DllImport("avutil-55", EntryPoint = "av_buffer_make_writable", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern int av_buffer_make_writable(AVBufferRef** @buf);
        
        /// <summary>Allocate a new AVBuffer, reusing an old buffer from the pool when available. This function may be called simultaneously from multiple threads.</summary>
        [DllImport("avutil-55", EntryPoint = "av_buffer_pool_get", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern AVBufferRef* av_buffer_pool_get(AVBufferPool* @pool);
        
        /// <summary>Allocate and initialize a buffer pool.</summary>
        /// <param name="size">size of each buffer in this pool</param>
        /// <param name="alloc">a function that will be used to allocate new buffers when the pool is empty. May be NULL, then the default allocator will be used (av_buffer_alloc()).</param>
        [DllImport("avutil-55", EntryPoint = "av_buffer_pool_init", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern AVBufferPool* av_buffer_pool_init(int @size, av_buffer_pool_init_alloc_func @alloc);
        
        /// <summary>Allocate and initialize a buffer pool with a more complex allocator.</summary>
        /// <param name="size">size of each buffer in this pool</param>
        /// <param name="opaque">arbitrary user data used by the allocator</param>
        /// <param name="alloc">a function that will be used to allocate new buffers when the pool is empty.</param>
        /// <param name="pool_free">a function that will be called immediately before the pool is freed. I.e. after av_buffer_pool_uninit() is called by the caller and all the frames are returned to the pool and freed. It is intended to uninitialize the user opaque data.</param>
        [DllImport("avutil-55", EntryPoint = "av_buffer_pool_init2", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern AVBufferPool* av_buffer_pool_init2(int @size, void* @opaque, av_buffer_pool_init2_alloc_func @alloc, av_buffer_pool_init2_pool_free_func @pool_free);
        
        /// <summary>Mark the pool as being available for freeing. It will actually be freed only once all the allocated buffers associated with the pool are released. Thus it is safe to call this function while some of the allocated buffers are still in use.</summary>
        /// <param name="pool">pointer to the pool to be freed. It will be set to NULL.</param>
        [DllImport("avutil-55", EntryPoint = "av_buffer_pool_uninit", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern void av_buffer_pool_uninit(AVBufferPool** @pool);
        
        /// <summary>Reallocate a given buffer.</summary>
        /// <param name="buf">a buffer reference to reallocate. On success, buf will be unreferenced and a new reference with the required size will be written in its place. On failure buf will be left untouched. *buf may be NULL, then a new buffer is allocated.</param>
        /// <param name="size">required new buffer size.</param>
        [DllImport("avutil-55", EntryPoint = "av_buffer_realloc", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern int av_buffer_realloc(AVBufferRef** @buf, int @size);
        
        /// <summary>Create a new reference to an AVBuffer.</summary>
        [DllImport("avutil-55", EntryPoint = "av_buffer_ref", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern AVBufferRef* av_buffer_ref(AVBufferRef* @buf);
        
        /// <summary>Free a given reference and automatically free the buffer if there are no more references to it.</summary>
        /// <param name="buf">the reference to be freed. The pointer is set to NULL on return.</param>
        [DllImport("avutil-55", EntryPoint = "av_buffer_unref", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern void av_buffer_unref(AVBufferRef** @buf);
        
        /// <summary>Non-inlined equivalent of av_mallocz_array().</summary>
        [DllImport("avutil-55", EntryPoint = "av_calloc", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern void* av_calloc(ulong @nmemb, ulong @size);
        
        /// <summary>Get the channel with the given index in channel_layout.</summary>
        [DllImport("avutil-55", EntryPoint = "av_channel_layout_extract_channel", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern ulong av_channel_layout_extract_channel(ulong @channel_layout, int @index);
        
        /// <summary>Returns the name for provided chroma location or NULL if unknown.</summary>
        [DllImport("avutil-55", EntryPoint = "av_chroma_location_name", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        [return: MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof(ConstCharPtrMarshaler))]
        public static extern string av_chroma_location_name(AVChromaLocation @location);
        
        /// <summary>Returns the name for provided color primaries or NULL if unknown.</summary>
        [DllImport("avutil-55", EntryPoint = "av_color_primaries_name", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        [return: MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof(ConstCharPtrMarshaler))]
        public static extern string av_color_primaries_name(AVColorPrimaries @primaries);
        
        /// <summary>Returns the name for provided color range or NULL if unknown.</summary>
        [DllImport("avutil-55", EntryPoint = "av_color_range_name", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        [return: MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof(ConstCharPtrMarshaler))]
        public static extern string av_color_range_name(AVColorRange @range);
        
        /// <summary>Returns the name for provided color space or NULL if unknown.</summary>
        [DllImport("avutil-55", EntryPoint = "av_color_space_name", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        [return: MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof(ConstCharPtrMarshaler))]
        public static extern string av_color_space_name(AVColorSpace @space);
        
        /// <summary>Returns the name for provided color transfer or NULL if unknown.</summary>
        [DllImport("avutil-55", EntryPoint = "av_color_transfer_name", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        [return: MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof(ConstCharPtrMarshaler))]
        public static extern string av_color_transfer_name(AVColorTransferCharacteristic @transfer);
        
        /// <summary>Compare the remainders of two integer operands divided by a common divisor.</summary>
        /// <param name="mod">Divisor; must be a power of 2</param>
        [DllImport("avutil-55", EntryPoint = "av_compare_mod", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern long av_compare_mod(ulong @a, ulong @b, ulong @mod);
        
        /// <summary>Compare two timestamps each in its own time base.</summary>
        [DllImport("avutil-55", EntryPoint = "av_compare_ts", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern int av_compare_ts(long @ts_a, AVRational @tb_a, long @ts_b, AVRational @tb_b);
        
        /// <summary>Returns the number of logical CPU cores present.</summary>
        [DllImport("avutil-55", EntryPoint = "av_cpu_count", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern int av_cpu_count();
        
        /// <summary>Convert a double precision floating point number to a rational.</summary>
        /// <param name="d">`double` to convert</param>
        /// <param name="max">Maximum allowed numerator and denominator</param>
        [DllImport("avutil-55", EntryPoint = "av_d2q", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern AVRational av_d2q(double @d, int @max);
        
        [DllImport("avutil-55", EntryPoint = "av_default_get_category", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern AVClassCategory av_default_get_category(void* @ptr);
        
        /// <summary>Return the context name</summary>
        /// <param name="ctx">The AVClass context</param>
        [DllImport("avutil-55", EntryPoint = "av_default_item_name", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        [return: MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof(ConstCharPtrMarshaler))]
        public static extern string av_default_item_name(void* @ctx);
        
        /// <summary>Copy entries from one AVDictionary struct into another.</summary>
        /// <param name="dst">pointer to a pointer to a AVDictionary struct. If *dst is NULL, this function will allocate a struct for you and put it in *dst</param>
        /// <param name="src">pointer to source AVDictionary struct</param>
        /// <param name="flags">flags to use when setting entries in *dst</param>
        [DllImport("avutil-55", EntryPoint = "av_dict_copy", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern int av_dict_copy(AVDictionary** @dst, AVDictionary* @src, int @flags);
        
        /// <summary>Get number of entries in dictionary.</summary>
        /// <param name="m">dictionary</param>
        [DllImport("avutil-55", EntryPoint = "av_dict_count", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern int av_dict_count(AVDictionary* @m);
        
        /// <summary>Free all the memory allocated for an AVDictionary struct and all keys and values.</summary>
        [DllImport("avutil-55", EntryPoint = "av_dict_free", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern void av_dict_free(AVDictionary** @m);
        
        /// <summary>Get a dictionary entry with matching key.</summary>
        /// <param name="key">matching key</param>
        /// <param name="prev">Set to the previous matching element to find the next. If set to NULL the first matching element is returned.</param>
        /// <param name="flags">a collection of AV_DICT_* flags controlling how the entry is retrieved</param>
        [DllImport("avutil-55", EntryPoint = "av_dict_get", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern AVDictionaryEntry* av_dict_get(AVDictionary* @m, [MarshalAs(UnmanagedType.LPStr)] string @key, AVDictionaryEntry* @prev, int @flags);
        
        /// <summary>Get dictionary entries as a string.</summary>
        /// <param name="m">dictionary</param>
        /// <param name="buffer">Pointer to buffer that will be allocated with string containg entries. Buffer must be freed by the caller when is no longer needed.</param>
        /// <param name="key_val_sep">character used to separate key from value</param>
        /// <param name="pairs_sep">character used to separate two pairs from each other</param>
        [DllImport("avutil-55", EntryPoint = "av_dict_get_string", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern int av_dict_get_string(AVDictionary* @m, byte** @buffer, byte @key_val_sep, byte @pairs_sep);
        
        /// <summary>Parse the key/value pairs list and add the parsed entries to a dictionary.</summary>
        /// <param name="key_val_sep">a 0-terminated list of characters used to separate key from value</param>
        /// <param name="pairs_sep">a 0-terminated list of characters used to separate two pairs from each other</param>
        /// <param name="flags">flags to use when adding to dictionary. AV_DICT_DONT_STRDUP_KEY and AV_DICT_DONT_STRDUP_VAL are ignored since the key/value tokens will always be duplicated.</param>
        [DllImport("avutil-55", EntryPoint = "av_dict_parse_string", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern int av_dict_parse_string(AVDictionary** @pm, [MarshalAs(UnmanagedType.LPStr)] string @str, [MarshalAs(UnmanagedType.LPStr)] string @key_val_sep, [MarshalAs(UnmanagedType.LPStr)] string @pairs_sep, int @flags);
        
        /// <summary>Set the given entry in *pm, overwriting an existing entry.</summary>
        /// <param name="pm">pointer to a pointer to a dictionary struct. If *pm is NULL a dictionary struct is allocated and put in *pm.</param>
        /// <param name="key">entry key to add to *pm (will either be av_strduped or added as a new key depending on flags)</param>
        /// <param name="value">entry value to add to *pm (will be av_strduped or added as a new key depending on flags). Passing a NULL value will cause an existing entry to be deleted.</param>
        [DllImport("avutil-55", EntryPoint = "av_dict_set", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern int av_dict_set(AVDictionary** @pm, [MarshalAs(UnmanagedType.LPStr)] string @key, [MarshalAs(UnmanagedType.LPStr)] string @value, int @flags);
        
        /// <summary>Convenience wrapper for av_dict_set that converts the value to a string and stores it.</summary>
        [DllImport("avutil-55", EntryPoint = "av_dict_set_int", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern int av_dict_set_int(AVDictionary** @pm, [MarshalAs(UnmanagedType.LPStr)] string @key, long @value, int @flags);
        
        /// <summary>Divide one rational by another.</summary>
        /// <param name="b">First rational</param>
        /// <param name="c">Second rational</param>
        [DllImport("avutil-55", EntryPoint = "av_div_q", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern AVRational av_div_q(AVRational @b, AVRational @c);
        
        /// <summary>Add the pointer to an element to a dynamic array.</summary>
        /// <param name="tab_ptr">Pointer to the array to grow</param>
        /// <param name="nb_ptr">Pointer to the number of elements in the array</param>
        /// <param name="elem">Element to add</param>
        [DllImport("avutil-55", EntryPoint = "av_dynarray_add", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern void av_dynarray_add(void* @tab_ptr, int* @nb_ptr, void* @elem);
        
        /// <summary>Add an element to a dynamic array.</summary>
        [DllImport("avutil-55", EntryPoint = "av_dynarray_add_nofree", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern int av_dynarray_add_nofree(void* @tab_ptr, int* @nb_ptr, void* @elem);
        
        /// <summary>Add an element of size `elem_size` to a dynamic array.</summary>
        /// <param name="tab_ptr">Pointer to the array to grow</param>
        /// <param name="nb_ptr">Pointer to the number of elements in the array</param>
        /// <param name="elem_size">Size in bytes of an element in the array</param>
        /// <param name="elem_data">Pointer to the data of the element to add. If `NULL`, the space of the newly added element is allocated but left uninitialized.</param>
        [DllImport("avutil-55", EntryPoint = "av_dynarray2_add", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern void* av_dynarray2_add(void** @tab_ptr, int* @nb_ptr, ulong @elem_size, byte* @elem_data);
        
        /// <summary>Allocate a buffer, reusing the given one if large enough.</summary>
        /// <param name="ptr">Pointer to pointer to an already allocated buffer. `*ptr` will be overwritten with pointer to new buffer on success or `NULL` on failure</param>
        /// <param name="size">Pointer to current size of buffer `*ptr`. `*size` is changed to `min_size` in case of success or 0 in case of failure</param>
        /// <param name="min_size">New size of buffer `*ptr`</param>
        [DllImport("avutil-55", EntryPoint = "av_fast_malloc", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern void av_fast_malloc(void* @ptr, uint* @size, ulong @min_size);
        
        /// <summary>Allocate and clear a buffer, reusing the given one if large enough.</summary>
        /// <param name="ptr">Pointer to pointer to an already allocated buffer. `*ptr` will be overwritten with pointer to new buffer on success or `NULL` on failure</param>
        /// <param name="size">Pointer to current size of buffer `*ptr`. `*size` is changed to `min_size` in case of success or 0 in case of failure</param>
        /// <param name="min_size">New size of buffer `*ptr`</param>
        [DllImport("avutil-55", EntryPoint = "av_fast_mallocz", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern void av_fast_mallocz(void* @ptr, uint* @size, ulong @min_size);
        
        /// <summary>Reallocate the given buffer if it is not large enough, otherwise do nothing.</summary>
        /// <param name="ptr">Already allocated buffer, or `NULL`</param>
        /// <param name="size">Pointer to current size of buffer `ptr`. `*size` is changed to `min_size` in case of success or 0 in case of failure</param>
        /// <param name="min_size">New size of buffer `ptr`</param>
        [DllImport("avutil-55", EntryPoint = "av_fast_realloc", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern void* av_fast_realloc(void* @ptr, uint* @size, ulong @min_size);
        
        /// <summary>Initialize an AVFifoBuffer.</summary>
        /// <param name="size">of FIFO</param>
        [DllImport("avutil-55", EntryPoint = "av_fifo_alloc", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern AVFifoBuffer* av_fifo_alloc(uint @size);
        
        /// <summary>Initialize an AVFifoBuffer.</summary>
        /// <param name="nmemb">number of elements</param>
        /// <param name="size">size of the single element</param>
        [DllImport("avutil-55", EntryPoint = "av_fifo_alloc_array", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern AVFifoBuffer* av_fifo_alloc_array(ulong @nmemb, ulong @size);
        
        /// <summary>Read and discard the specified amount of data from an AVFifoBuffer.</summary>
        /// <param name="f">AVFifoBuffer to read from</param>
        /// <param name="size">amount of data to read in bytes</param>
        [DllImport("avutil-55", EntryPoint = "av_fifo_drain", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern void av_fifo_drain(AVFifoBuffer* @f, int @size);
        
        /// <summary>Free an AVFifoBuffer.</summary>
        /// <param name="f">AVFifoBuffer to free</param>
        [DllImport("avutil-55", EntryPoint = "av_fifo_free", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern void av_fifo_free(AVFifoBuffer* @f);
        
        /// <summary>Free an AVFifoBuffer and reset pointer to NULL.</summary>
        /// <param name="f">AVFifoBuffer to free</param>
        [DllImport("avutil-55", EntryPoint = "av_fifo_freep", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern void av_fifo_freep(AVFifoBuffer** @f);
        
        /// <summary>Feed data from an AVFifoBuffer to a user-supplied callback. Similar as av_fifo_gereric_read but without discarding data.</summary>
        /// <param name="f">AVFifoBuffer to read from</param>
        /// <param name="dest">data destination</param>
        /// <param name="buf_size">number of bytes to read</param>
        /// <param name="func">generic read function</param>
        [DllImport("avutil-55", EntryPoint = "av_fifo_generic_peek", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern int av_fifo_generic_peek(AVFifoBuffer* @f, void* @dest, int @buf_size, av_fifo_generic_peek_func_func @func);
        
        /// <summary>Feed data at specific position from an AVFifoBuffer to a user-supplied callback. Similar as av_fifo_gereric_read but without discarding data.</summary>
        /// <param name="f">AVFifoBuffer to read from</param>
        /// <param name="dest">data destination</param>
        /// <param name="offset">offset from current read position</param>
        /// <param name="buf_size">number of bytes to read</param>
        /// <param name="func">generic read function</param>
        [DllImport("avutil-55", EntryPoint = "av_fifo_generic_peek_at", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern int av_fifo_generic_peek_at(AVFifoBuffer* @f, void* @dest, int @offset, int @buf_size, av_fifo_generic_peek_at_func_func @func);
        
        /// <summary>Feed data from an AVFifoBuffer to a user-supplied callback.</summary>
        /// <param name="f">AVFifoBuffer to read from</param>
        /// <param name="dest">data destination</param>
        /// <param name="buf_size">number of bytes to read</param>
        /// <param name="func">generic read function</param>
        [DllImport("avutil-55", EntryPoint = "av_fifo_generic_read", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern int av_fifo_generic_read(AVFifoBuffer* @f, void* @dest, int @buf_size, av_fifo_generic_read_func_func @func);
        
        /// <summary>Feed data from a user-supplied callback to an AVFifoBuffer.</summary>
        /// <param name="f">AVFifoBuffer to write to</param>
        /// <param name="src">data source; non-const since it may be used as a modifiable context by the function defined in func</param>
        /// <param name="size">number of bytes to write</param>
        /// <param name="func">generic write function; the first parameter is src, the second is dest_buf, the third is dest_buf_size. func must return the number of bytes written to dest_buf, or &lt; = 0 to indicate no more data available to write. If func is NULL, src is interpreted as a simple byte array for source data.</param>
        [DllImport("avutil-55", EntryPoint = "av_fifo_generic_write", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern int av_fifo_generic_write(AVFifoBuffer* @f, void* @src, int @size, av_fifo_generic_write_func_func @func);
        
        /// <summary>Enlarge an AVFifoBuffer. In case of reallocation failure, the old FIFO is kept unchanged. The new fifo size may be larger than the requested size.</summary>
        /// <param name="f">AVFifoBuffer to resize</param>
        /// <param name="additional_space">the amount of space in bytes to allocate in addition to av_fifo_size()</param>
        [DllImport("avutil-55", EntryPoint = "av_fifo_grow", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern int av_fifo_grow(AVFifoBuffer* @f, uint @additional_space);
        
        /// <summary>Resize an AVFifoBuffer. In case of reallocation failure, the old FIFO is kept unchanged.</summary>
        /// <param name="f">AVFifoBuffer to resize</param>
        /// <param name="size">new AVFifoBuffer size in bytes</param>
        [DllImport("avutil-55", EntryPoint = "av_fifo_realloc2", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern int av_fifo_realloc2(AVFifoBuffer* @f, uint @size);
        
        /// <summary>Reset the AVFifoBuffer to the state right after av_fifo_alloc, in particular it is emptied.</summary>
        /// <param name="f">AVFifoBuffer to reset</param>
        [DllImport("avutil-55", EntryPoint = "av_fifo_reset", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern void av_fifo_reset(AVFifoBuffer* @f);
        
        /// <summary>Return the amount of data in bytes in the AVFifoBuffer, that is the amount of data you can read from it.</summary>
        /// <param name="f">AVFifoBuffer to read from</param>
        [DllImport("avutil-55", EntryPoint = "av_fifo_size", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern int av_fifo_size(AVFifoBuffer* @f);
        
        /// <summary>Return the amount of space in bytes in the AVFifoBuffer, that is the amount of data you can write into it.</summary>
        /// <param name="f">AVFifoBuffer to write into</param>
        [DllImport("avutil-55", EntryPoint = "av_fifo_space", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern int av_fifo_space(AVFifoBuffer* @f);
        
        /// <summary>Compute what kind of losses will occur when converting from one specific pixel format to another. When converting from one pixel format to another, information loss may occur. For example, when converting from RGB24 to GRAY, the color information will be lost. Similarly, other losses occur when converting from some formats to other formats. These losses can involve loss of chroma, but also loss of resolution, loss of color depth, loss due to the color space conversion, loss of the alpha bits or loss due to color quantization. av_get_fix_fmt_loss() informs you about the various types of losses which will occur when converting from one pixel format to another.</summary>
        /// <param name="src_pix_fmt">source pixel format</param>
        /// <param name="has_alpha">Whether the source pixel format alpha channel is used.</param>
        [DllImport("avutil-55", EntryPoint = "av_find_best_pix_fmt_of_2", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern AVPixelFormat av_find_best_pix_fmt_of_2(AVPixelFormat @dst_pix_fmt1, AVPixelFormat @dst_pix_fmt2, AVPixelFormat @src_pix_fmt, int @has_alpha, int* @loss_ptr);
        
        /// <summary>Find the value in a list of rationals nearest a given reference rational.</summary>
        /// <param name="q">Reference rational</param>
        /// <param name="q_list">Array of rationals terminated by `{0, 0}`</param>
        [DllImport("avutil-55", EntryPoint = "av_find_nearest_q_idx", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern int av_find_nearest_q_idx(AVRational @q, AVRational* @q_list);
        
        /// <summary>Open a file using a UTF-8 filename. The API of this function matches POSIX fopen(), errors are returned through errno.</summary>
        [DllImport("avutil-55", EntryPoint = "av_fopen_utf8", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern _iobuf* av_fopen_utf8([MarshalAs(UnmanagedType.LPStr)] string @path, [MarshalAs(UnmanagedType.LPStr)] string @mode);
        
        /// <summary>Disables cpu detection and forces the specified flags. -1 is a special case that disables forcing of specific flags.</summary>
        [DllImport("avutil-55", EntryPoint = "av_force_cpu_flags", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern void av_force_cpu_flags(int @flags);
        
        /// <summary>Fill the provided buffer with a string containing a FourCC (four-character code) representation.</summary>
        /// <param name="buf">a buffer with size in bytes of at least AV_FOURCC_MAX_STRING_SIZE</param>
        /// <param name="fourcc">the fourcc to represent</param>
        [DllImport("avutil-55", EntryPoint = "av_fourcc_make_string", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern byte* av_fourcc_make_string(byte* @buf, uint @fourcc);
        
        /// <summary>Allocate an AVFrame and set its fields to default values. The resulting struct must be freed using av_frame_free().</summary>
        [DllImport("avutil-55", EntryPoint = "av_frame_alloc", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern AVFrame* av_frame_alloc();
        
        /// <summary>Create a new frame that references the same data as src.</summary>
        [DllImport("avutil-55", EntryPoint = "av_frame_clone", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern AVFrame* av_frame_clone(AVFrame* @src);
        
        /// <summary>Copy the frame data from src to dst.</summary>
        [DllImport("avutil-55", EntryPoint = "av_frame_copy", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern int av_frame_copy(AVFrame* @dst, AVFrame* @src);
        
        /// <summary>Copy only &quot;metadata&quot; fields from src to dst.</summary>
        [DllImport("avutil-55", EntryPoint = "av_frame_copy_props", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern int av_frame_copy_props(AVFrame* @dst, AVFrame* @src);
        
        /// <summary>Free the frame and any dynamically allocated objects in it, e.g. extended_data. If the frame is reference counted, it will be unreferenced first.</summary>
        /// <param name="frame">frame to be freed. The pointer will be set to NULL.</param>
        [DllImport("avutil-55", EntryPoint = "av_frame_free", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern void av_frame_free(AVFrame** @frame);
        
        /// <summary>Accessors for some AVFrame fields. These used to be provided for ABI compatibility, and do not need to be used anymore.</summary>
        [DllImport("avutil-55", EntryPoint = "av_frame_get_best_effort_timestamp", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern long av_frame_get_best_effort_timestamp(AVFrame* @frame);
        
        /// <summary>Allocate new buffer(s) for audio or video data.</summary>
        /// <param name="frame">frame in which to store the new buffers.</param>
        /// <param name="align">required buffer size alignment</param>
        [DllImport("avutil-55", EntryPoint = "av_frame_get_buffer", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern int av_frame_get_buffer(AVFrame* @frame, int @align);
        
        [DllImport("avutil-55", EntryPoint = "av_frame_get_channel_layout", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern long av_frame_get_channel_layout(AVFrame* @frame);
        
        [DllImport("avutil-55", EntryPoint = "av_frame_get_channels", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern int av_frame_get_channels(AVFrame* @frame);
        
        [DllImport("avutil-55", EntryPoint = "av_frame_get_color_range", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern AVColorRange av_frame_get_color_range(AVFrame* @frame);
        
        [DllImport("avutil-55", EntryPoint = "av_frame_get_colorspace", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern AVColorSpace av_frame_get_colorspace(AVFrame* @frame);
        
        [DllImport("avutil-55", EntryPoint = "av_frame_get_decode_error_flags", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern int av_frame_get_decode_error_flags(AVFrame* @frame);
        
        [DllImport("avutil-55", EntryPoint = "av_frame_get_metadata", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern AVDictionary* av_frame_get_metadata(AVFrame* @frame);
        
        [DllImport("avutil-55", EntryPoint = "av_frame_get_pkt_duration", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern long av_frame_get_pkt_duration(AVFrame* @frame);
        
        [DllImport("avutil-55", EntryPoint = "av_frame_get_pkt_pos", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern long av_frame_get_pkt_pos(AVFrame* @frame);
        
        [DllImport("avutil-55", EntryPoint = "av_frame_get_pkt_size", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern int av_frame_get_pkt_size(AVFrame* @frame);
        
        /// <summary>Get the buffer reference a given data plane is stored in.</summary>
        /// <param name="plane">index of the data plane of interest in frame-&gt;extended_data.</param>
        [DllImport("avutil-55", EntryPoint = "av_frame_get_plane_buffer", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern AVBufferRef* av_frame_get_plane_buffer(AVFrame* @frame, int @plane);
        
        [DllImport("avutil-55", EntryPoint = "av_frame_get_qp_table", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern sbyte* av_frame_get_qp_table(AVFrame* @f, int* @stride, int* @type);
        
        [DllImport("avutil-55", EntryPoint = "av_frame_get_sample_rate", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern int av_frame_get_sample_rate(AVFrame* @frame);
        
        /// <summary>Returns a pointer to the side data of a given type on success, NULL if there is no side data with such type in this frame.</summary>
        [DllImport("avutil-55", EntryPoint = "av_frame_get_side_data", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern AVFrameSideData* av_frame_get_side_data(AVFrame* @frame, AVFrameSideDataType @type);
        
        /// <summary>Check if the frame data is writable.</summary>
        [DllImport("avutil-55", EntryPoint = "av_frame_is_writable", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern int av_frame_is_writable(AVFrame* @frame);
        
        /// <summary>Ensure that the frame data is writable, avoiding data copy if possible.</summary>
        [DllImport("avutil-55", EntryPoint = "av_frame_make_writable", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern int av_frame_make_writable(AVFrame* @frame);
        
        /// <summary>Move everything contained in src to dst and reset src.</summary>
        [DllImport("avutil-55", EntryPoint = "av_frame_move_ref", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern void av_frame_move_ref(AVFrame* @dst, AVFrame* @src);
        
        /// <summary>Add a new side data to a frame.</summary>
        /// <param name="frame">a frame to which the side data should be added</param>
        /// <param name="type">type of the added side data</param>
        /// <param name="size">size of the side data</param>
        [DllImport("avutil-55", EntryPoint = "av_frame_new_side_data", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern AVFrameSideData* av_frame_new_side_data(AVFrame* @frame, AVFrameSideDataType @type, int @size);
        
        /// <summary>Set up a new reference to the data described by the source frame.</summary>
        [DllImport("avutil-55", EntryPoint = "av_frame_ref", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern int av_frame_ref(AVFrame* @dst, AVFrame* @src);
        
        /// <summary>If side data of the supplied type exists in the frame, free it and remove it from the frame.</summary>
        [DllImport("avutil-55", EntryPoint = "av_frame_remove_side_data", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern void av_frame_remove_side_data(AVFrame* @frame, AVFrameSideDataType @type);
        
        [DllImport("avutil-55", EntryPoint = "av_frame_set_best_effort_timestamp", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern void av_frame_set_best_effort_timestamp(AVFrame* @frame, long @val);
        
        [DllImport("avutil-55", EntryPoint = "av_frame_set_channel_layout", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern void av_frame_set_channel_layout(AVFrame* @frame, long @val);
        
        [DllImport("avutil-55", EntryPoint = "av_frame_set_channels", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern void av_frame_set_channels(AVFrame* @frame, int @val);
        
        [DllImport("avutil-55", EntryPoint = "av_frame_set_color_range", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern void av_frame_set_color_range(AVFrame* @frame, AVColorRange @val);
        
        [DllImport("avutil-55", EntryPoint = "av_frame_set_colorspace", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern void av_frame_set_colorspace(AVFrame* @frame, AVColorSpace @val);
        
        [DllImport("avutil-55", EntryPoint = "av_frame_set_decode_error_flags", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern void av_frame_set_decode_error_flags(AVFrame* @frame, int @val);
        
        [DllImport("avutil-55", EntryPoint = "av_frame_set_metadata", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern void av_frame_set_metadata(AVFrame* @frame, AVDictionary* @val);
        
        [DllImport("avutil-55", EntryPoint = "av_frame_set_pkt_duration", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern void av_frame_set_pkt_duration(AVFrame* @frame, long @val);
        
        [DllImport("avutil-55", EntryPoint = "av_frame_set_pkt_pos", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern void av_frame_set_pkt_pos(AVFrame* @frame, long @val);
        
        [DllImport("avutil-55", EntryPoint = "av_frame_set_pkt_size", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern void av_frame_set_pkt_size(AVFrame* @frame, int @val);
        
        [DllImport("avutil-55", EntryPoint = "av_frame_set_qp_table", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern int av_frame_set_qp_table(AVFrame* @f, AVBufferRef* @buf, int @stride, int @type);
        
        [DllImport("avutil-55", EntryPoint = "av_frame_set_sample_rate", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern void av_frame_set_sample_rate(AVFrame* @frame, int @val);
        
        /// <summary>Returns a string identifying the side data type</summary>
        [DllImport("avutil-55", EntryPoint = "av_frame_side_data_name", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        [return: MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof(ConstCharPtrMarshaler))]
        public static extern string av_frame_side_data_name(AVFrameSideDataType @type);
        
        /// <summary>Unreference all the buffers referenced by frame and reset the frame fields.</summary>
        [DllImport("avutil-55", EntryPoint = "av_frame_unref", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern void av_frame_unref(AVFrame* @frame);
        
        /// <summary>Free a memory block which has been allocated with a function of av_malloc() or av_realloc() family.</summary>
        /// <param name="ptr">Pointer to the memory block which should be freed.</param>
        [DllImport("avutil-55", EntryPoint = "av_free", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern void av_free(void* @ptr);
        
        /// <summary>Free a memory block which has been allocated with a function of av_malloc() or av_realloc() family, and set the pointer pointing to it to `NULL`.</summary>
        /// <param name="ptr">Pointer to the pointer to the memory block which should be freed</param>
        [DllImport("avutil-55", EntryPoint = "av_freep", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern void av_freep(void* @ptr);
        
        /// <summary>Compute the greatest common divisor of two integer operands.</summary>
        [DllImport("avutil-55", EntryPoint = "av_gcd", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern long av_gcd(long @a, long @b);
        
        /// <summary>Return the planar&lt;-&gt;packed alternative form of the given sample format, or AV_SAMPLE_FMT_NONE on error. If the passed sample_fmt is already in the requested planar/packed format, the format returned is the same as the input.</summary>
        [DllImport("avutil-55", EntryPoint = "av_get_alt_sample_fmt", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern AVSampleFormat av_get_alt_sample_fmt(AVSampleFormat @sample_fmt, int @planar);
        
        /// <summary>Return the number of bits per pixel used by the pixel format described by pixdesc. Note that this is not the same as the number of bits per sample.</summary>
        [DllImport("avutil-55", EntryPoint = "av_get_bits_per_pixel", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern int av_get_bits_per_pixel(AVPixFmtDescriptor* @pixdesc);
        
        /// <summary>Return number of bytes per sample.</summary>
        /// <param name="sample_fmt">the sample format</param>
        [DllImport("avutil-55", EntryPoint = "av_get_bytes_per_sample", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern int av_get_bytes_per_sample(AVSampleFormat @sample_fmt);
        
        /// <summary>Get the description of a given channel.</summary>
        /// <param name="channel">a channel layout with a single channel</param>
        [DllImport("avutil-55", EntryPoint = "av_get_channel_description", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        [return: MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof(ConstCharPtrMarshaler))]
        public static extern string av_get_channel_description(ulong @channel);
        
        /// <summary>Return a channel layout id that matches name, or 0 if no match is found.</summary>
        [DllImport("avutil-55", EntryPoint = "av_get_channel_layout", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern ulong av_get_channel_layout([MarshalAs(UnmanagedType.LPStr)] string @name);
        
        /// <summary>Get the index of a channel in channel_layout.</summary>
        /// <param name="channel">a channel layout describing exactly one channel which must be present in channel_layout.</param>
        [DllImport("avutil-55", EntryPoint = "av_get_channel_layout_channel_index", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern int av_get_channel_layout_channel_index(ulong @channel_layout, ulong @channel);
        
        /// <summary>Return the number of channels in the channel layout.</summary>
        [DllImport("avutil-55", EntryPoint = "av_get_channel_layout_nb_channels", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern int av_get_channel_layout_nb_channels(ulong @channel_layout);
        
        /// <summary>Return a description of a channel layout. If nb_channels is &lt;= 0, it is guessed from the channel_layout.</summary>
        /// <param name="buf">put here the string containing the channel layout</param>
        /// <param name="buf_size">size in bytes of the buffer</param>
        [DllImport("avutil-55", EntryPoint = "av_get_channel_layout_string", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern void av_get_channel_layout_string(byte* @buf, int @buf_size, int @nb_channels, ulong @channel_layout);
        
        /// <summary>Get the name of a given channel.</summary>
        [DllImport("avutil-55", EntryPoint = "av_get_channel_name", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        [return: MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof(ConstCharPtrMarshaler))]
        public static extern string av_get_channel_name(ulong @channel);
        
        /// <summary>Get the name of a colorspace.</summary>
        [DllImport("avutil-55", EntryPoint = "av_get_colorspace_name", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        [return: MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof(ConstCharPtrMarshaler))]
        public static extern string av_get_colorspace_name(AVColorSpace @val);
        
        /// <summary>Return the flags which specify extensions supported by the CPU. The returned value is affected by av_force_cpu_flags() if that was used before. So av_get_cpu_flags() can easily be used in an application to detect the enabled cpu flags.</summary>
        [DllImport("avutil-55", EntryPoint = "av_get_cpu_flags", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern int av_get_cpu_flags();
        
        /// <summary>Return default channel layout for a given number of channels.</summary>
        [DllImport("avutil-55", EntryPoint = "av_get_default_channel_layout", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern long av_get_default_channel_layout(int @nb_channels);
        
        /// <summary>Return a channel layout and the number of channels based on the specified name.</summary>
        /// <param name="name">channel layout specification string</param>
        /// <param name="channel_layout">parsed channel layout (0 if unknown)</param>
        /// <param name="nb_channels">number of channels</param>
        [DllImport("avutil-55", EntryPoint = "av_get_extended_channel_layout", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern int av_get_extended_channel_layout([MarshalAs(UnmanagedType.LPStr)] string @name, ulong* @channel_layout, int* @nb_channels);
        
        /// <summary>Return a string describing the media_type enum, NULL if media_type is unknown.</summary>
        [DllImport("avutil-55", EntryPoint = "av_get_media_type_string", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        [return: MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof(ConstCharPtrMarshaler))]
        public static extern string av_get_media_type_string(AVMediaType @media_type);
        
        /// <summary>Get the packed alternative form of the given sample format.</summary>
        [DllImport("avutil-55", EntryPoint = "av_get_packed_sample_fmt", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern AVSampleFormat av_get_packed_sample_fmt(AVSampleFormat @sample_fmt);
        
        /// <summary>Return the number of bits per pixel for the pixel format described by pixdesc, including any padding or unused bits.</summary>
        [DllImport("avutil-55", EntryPoint = "av_get_padded_bits_per_pixel", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern int av_get_padded_bits_per_pixel(AVPixFmtDescriptor* @pixdesc);
        
        /// <summary>Return a single letter to describe the given picture type pict_type.</summary>
        /// <param name="pict_type">the picture type</param>
        [DllImport("avutil-55", EntryPoint = "av_get_picture_type_char", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern byte av_get_picture_type_char(AVPictureType @pict_type);
        
        /// <summary>Return the pixel format corresponding to name.</summary>
        [DllImport("avutil-55", EntryPoint = "av_get_pix_fmt", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern AVPixelFormat av_get_pix_fmt([MarshalAs(UnmanagedType.LPStr)] string @name);
        
        /// <summary>Compute what kind of losses will occur when converting from one specific pixel format to another. When converting from one pixel format to another, information loss may occur. For example, when converting from RGB24 to GRAY, the color information will be lost. Similarly, other losses occur when converting from some formats to other formats. These losses can involve loss of chroma, but also loss of resolution, loss of color depth, loss due to the color space conversion, loss of the alpha bits or loss due to color quantization. av_get_fix_fmt_loss() informs you about the various types of losses which will occur when converting from one pixel format to another.</summary>
        /// <param name="dst_pix_fmt">destination pixel format</param>
        /// <param name="src_pix_fmt">source pixel format</param>
        /// <param name="has_alpha">Whether the source pixel format alpha channel is used.</param>
        [DllImport("avutil-55", EntryPoint = "av_get_pix_fmt_loss", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern int av_get_pix_fmt_loss(AVPixelFormat @dst_pix_fmt, AVPixelFormat @src_pix_fmt, int @has_alpha);
        
        /// <summary>Return the short name for a pixel format, NULL in case pix_fmt is unknown.</summary>
        [DllImport("avutil-55", EntryPoint = "av_get_pix_fmt_name", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        [return: MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof(ConstCharPtrMarshaler))]
        public static extern string av_get_pix_fmt_name(AVPixelFormat @pix_fmt);
        
        /// <summary>Print in buf the string corresponding to the pixel format with number pix_fmt, or a header if pix_fmt is negative.</summary>
        /// <param name="buf">the buffer where to write the string</param>
        /// <param name="buf_size">the size of buf</param>
        /// <param name="pix_fmt">the number of the pixel format to print the corresponding info string, or a negative value to print the corresponding header.</param>
        [DllImport("avutil-55", EntryPoint = "av_get_pix_fmt_string", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern byte* av_get_pix_fmt_string(byte* @buf, int @buf_size, AVPixelFormat @pix_fmt);
        
        /// <summary>Get the planar alternative form of the given sample format.</summary>
        [DllImport("avutil-55", EntryPoint = "av_get_planar_sample_fmt", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern AVSampleFormat av_get_planar_sample_fmt(AVSampleFormat @sample_fmt);
        
        /// <summary>Return a sample format corresponding to name, or AV_SAMPLE_FMT_NONE on error.</summary>
        [DllImport("avutil-55", EntryPoint = "av_get_sample_fmt", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern AVSampleFormat av_get_sample_fmt([MarshalAs(UnmanagedType.LPStr)] string @name);
        
        /// <summary>Return the name of sample_fmt, or NULL if sample_fmt is not recognized.</summary>
        [DllImport("avutil-55", EntryPoint = "av_get_sample_fmt_name", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        [return: MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof(ConstCharPtrMarshaler))]
        public static extern string av_get_sample_fmt_name(AVSampleFormat @sample_fmt);
        
        /// <summary>Generate a string corresponding to the sample format with sample_fmt, or a header if sample_fmt is negative.</summary>
        /// <param name="buf">the buffer where to write the string</param>
        /// <param name="buf_size">the size of buf</param>
        /// <param name="sample_fmt">the number of the sample format to print the corresponding info string, or a negative value to print the corresponding header.</param>
        [DllImport("avutil-55", EntryPoint = "av_get_sample_fmt_string", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern byte* av_get_sample_fmt_string(byte* @buf, int @buf_size, AVSampleFormat @sample_fmt);
        
        /// <summary>Get the value and name of a standard channel layout.</summary>
        /// <param name="index">index in an internal list, starting at 0</param>
        /// <param name="layout">channel layout mask</param>
        /// <param name="name">name of the layout</param>
        [DllImport("avutil-55", EntryPoint = "av_get_standard_channel_layout", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern int av_get_standard_channel_layout(uint @index, ulong* @layout, byte** @name);
        
        /// <summary>Return the fractional representation of the internal time base.</summary>
        [DllImport("avutil-55", EntryPoint = "av_get_time_base_q", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern AVRational av_get_time_base_q();
        
        /// <summary>Allocate an AVHWDeviceContext for a given hardware type.</summary>
        /// <param name="type">the type of the hardware device to allocate.</param>
        [DllImport("avutil-55", EntryPoint = "av_hwdevice_ctx_alloc", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern AVBufferRef* av_hwdevice_ctx_alloc(AVHWDeviceType @type);
        
        /// <summary>Open a device of the specified type and create an AVHWDeviceContext for it.</summary>
        /// <param name="device_ctx">On success, a reference to the newly-created device context will be written here. The reference is owned by the caller and must be released with av_buffer_unref() when no longer needed. On failure, NULL will be written to this pointer.</param>
        /// <param name="type">The type of the device to create.</param>
        /// <param name="device">A type-specific string identifying the device to open.</param>
        /// <param name="opts">A dictionary of additional (type-specific) options to use in opening the device. The dictionary remains owned by the caller.</param>
        /// <param name="flags">currently unused</param>
        [DllImport("avutil-55", EntryPoint = "av_hwdevice_ctx_create", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern int av_hwdevice_ctx_create(AVBufferRef** @device_ctx, AVHWDeviceType @type, [MarshalAs(UnmanagedType.LPStr)] string @device, AVDictionary* @opts, int @flags);
        
        /// <summary>Finalize the device context before use. This function must be called after the context is filled with all the required information and before it is used in any way.</summary>
        /// <param name="ref">a reference to the AVHWDeviceContext</param>
        [DllImport("avutil-55", EntryPoint = "av_hwdevice_ctx_init", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern int av_hwdevice_ctx_init(AVBufferRef* @ref);
        
        /// <summary>Get the constraints on HW frames given a device and the HW-specific configuration to be used with that device. If no HW-specific configuration is provided, returns the maximum possible capabilities of the device.</summary>
        /// <param name="hwconfig">a filled HW-specific configuration structure, or NULL to return the maximum possible capabilities of the device.</param>
        [DllImport("avutil-55", EntryPoint = "av_hwdevice_get_hwframe_constraints", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern AVHWFramesConstraints* av_hwdevice_get_hwframe_constraints(AVBufferRef* @ref, void* @hwconfig);
        
        /// <summary>Allocate a HW-specific configuration structure for a given HW device. After use, the user must free all members as required by the specific hardware structure being used, then free the structure itself with av_free().</summary>
        /// <param name="device_ctx">a reference to the associated AVHWDeviceContext.</param>
        [DllImport("avutil-55", EntryPoint = "av_hwdevice_hwconfig_alloc", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern void* av_hwdevice_hwconfig_alloc(AVBufferRef* @device_ctx);
        
        /// <summary>Free an AVHWFrameConstraints structure.</summary>
        /// <param name="constraints">The (filled or unfilled) AVHWFrameConstraints structure.</param>
        [DllImport("avutil-55", EntryPoint = "av_hwframe_constraints_free", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern void av_hwframe_constraints_free(AVHWFramesConstraints** @constraints);
        
        /// <summary>Allocate an AVHWFramesContext tied to a given device context.</summary>
        /// <param name="device_ctx">a reference to a AVHWDeviceContext. This function will make a new reference for internal use, the one passed to the function remains owned by the caller.</param>
        [DllImport("avutil-55", EntryPoint = "av_hwframe_ctx_alloc", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern AVBufferRef* av_hwframe_ctx_alloc(AVBufferRef* @device_ctx);
        
        /// <summary>Create and initialise an AVHWFramesContext as a mapping of another existing AVHWFramesContext on a different device.</summary>
        /// <param name="derived_frame_ctx">On success, a reference to the newly created AVHWFramesContext.</param>
        /// <param name="derived_device_ctx">A reference to the device to create the new AVHWFramesContext on.</param>
        /// <param name="source_frame_ctx">A reference to an existing AVHWFramesContext which will be mapped to the derived context.</param>
        /// <param name="flags">Currently unused; should be set to zero.</param>
        [DllImport("avutil-55", EntryPoint = "av_hwframe_ctx_create_derived", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern int av_hwframe_ctx_create_derived(AVBufferRef** @derived_frame_ctx, AVPixelFormat @format, AVBufferRef* @derived_device_ctx, AVBufferRef* @source_frame_ctx, int @flags);
        
        /// <summary>Finalize the context before use. This function must be called after the context is filled with all the required information and before it is attached to any frames.</summary>
        /// <param name="ref">a reference to the AVHWFramesContext</param>
        [DllImport("avutil-55", EntryPoint = "av_hwframe_ctx_init", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern int av_hwframe_ctx_init(AVBufferRef* @ref);
        
        /// <summary>Allocate a new frame attached to the given AVHWFramesContext.</summary>
        /// <param name="hwframe_ctx">a reference to an AVHWFramesContext</param>
        /// <param name="frame">an empty (freshly allocated or unreffed) frame to be filled with newly allocated buffers.</param>
        /// <param name="flags">currently unused, should be set to zero</param>
        [DllImport("avutil-55", EntryPoint = "av_hwframe_get_buffer", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern int av_hwframe_get_buffer(AVBufferRef* @hwframe_ctx, AVFrame* @frame, int @flags);
        
        /// <summary>Map a hardware frame.</summary>
        /// <param name="dst">Destination frame, to contain the mapping.</param>
        /// <param name="src">Source frame, to be mapped.</param>
        /// <param name="flags">Some combination of AV_HWFRAME_MAP_* flags.</param>
        [DllImport("avutil-55", EntryPoint = "av_hwframe_map", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern int av_hwframe_map(AVFrame* @dst, AVFrame* @src, int @flags);
        
        /// <summary>Copy data to or from a hw surface. At least one of dst/src must have an AVHWFramesContext attached.</summary>
        /// <param name="dst">the destination frame. dst is not touched on failure.</param>
        /// <param name="src">the source frame.</param>
        /// <param name="flags">currently unused, should be set to zero</param>
        [DllImport("avutil-55", EntryPoint = "av_hwframe_transfer_data", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern int av_hwframe_transfer_data(AVFrame* @dst, AVFrame* @src, int @flags);
        
        /// <summary>Get a list of possible source or target formats usable in av_hwframe_transfer_data().</summary>
        /// <param name="hwframe_ctx">the frame context to obtain the information for</param>
        /// <param name="dir">the direction of the transfer</param>
        /// <param name="formats">the pointer to the output format list will be written here. The list is terminated with AV_PIX_FMT_NONE and must be freed by the caller when no longer needed using av_free(). If this function returns successfully, the format list will have at least one item (not counting the terminator). On failure, the contents of this pointer are unspecified.</param>
        /// <param name="flags">currently unused, should be set to zero</param>
        [DllImport("avutil-55", EntryPoint = "av_hwframe_transfer_get_formats", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern int av_hwframe_transfer_get_formats(AVBufferRef* @hwframe_ctx, AVHWFrameTransferDirection @dir, AVPixelFormat** @formats, int @flags);
        
        /// <summary>Allocate an image with size w and h and pixel format pix_fmt, and fill pointers and linesizes accordingly. The allocated image buffer has to be freed by using av_freep(&amp;pointers[0]).</summary>
        /// <param name="align">the value to use for buffer size alignment</param>
        [DllImport("avutil-55", EntryPoint = "av_image_alloc", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern int av_image_alloc(ref byte_ptrArray4 @pointers, ref int_array4 @linesizes, int @w, int @h, AVPixelFormat @pix_fmt, int @align);
        
        /// <summary>Check if the given sample aspect ratio of an image is valid.</summary>
        /// <param name="w">width of the image</param>
        /// <param name="h">height of the image</param>
        /// <param name="sar">sample aspect ratio of the image</param>
        [DllImport("avutil-55", EntryPoint = "av_image_check_sar", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern int av_image_check_sar(uint @w, uint @h, AVRational @sar);
        
        /// <summary>Check if the given dimension of an image is valid, meaning that all bytes of the image can be addressed with a signed int.</summary>
        /// <param name="w">the width of the picture</param>
        /// <param name="h">the height of the picture</param>
        /// <param name="log_offset">the offset to sum to the log level for logging with log_ctx</param>
        /// <param name="log_ctx">the parent logging context, it may be NULL</param>
        [DllImport("avutil-55", EntryPoint = "av_image_check_size", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern int av_image_check_size(uint @w, uint @h, int @log_offset, void* @log_ctx);
        
        /// <summary>Check if the given dimension of an image is valid, meaning that all bytes of a plane of an image with the specified pix_fmt can be addressed with a signed int.</summary>
        /// <param name="w">the width of the picture</param>
        /// <param name="h">the height of the picture</param>
        /// <param name="max_pixels">the maximum number of pixels the user wants to accept</param>
        /// <param name="pix_fmt">the pixel format, can be AV_PIX_FMT_NONE if unknown.</param>
        /// <param name="log_offset">the offset to sum to the log level for logging with log_ctx</param>
        /// <param name="log_ctx">the parent logging context, it may be NULL</param>
        [DllImport("avutil-55", EntryPoint = "av_image_check_size2", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern int av_image_check_size2(uint @w, uint @h, long @max_pixels, AVPixelFormat @pix_fmt, int @log_offset, void* @log_ctx);
        
        /// <summary>Copy image in src_data to dst_data.</summary>
        /// <param name="dst_linesizes">linesizes for the image in dst_data</param>
        /// <param name="src_linesizes">linesizes for the image in src_data</param>
        [DllImport("avutil-55", EntryPoint = "av_image_copy", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern void av_image_copy(ref byte_ptrArray4 @dst_data, ref int_array4 @dst_linesizes, ref byte_ptrArray4 @src_data, int_array4 @src_linesizes, AVPixelFormat @pix_fmt, int @width, int @height);
        
        /// <summary>Copy image plane from src to dst. That is, copy &quot;height&quot; number of lines of &quot;bytewidth&quot; bytes each. The first byte of each successive line is separated by *_linesize bytes.</summary>
        /// <param name="dst_linesize">linesize for the image plane in dst</param>
        /// <param name="src_linesize">linesize for the image plane in src</param>
        [DllImport("avutil-55", EntryPoint = "av_image_copy_plane", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern void av_image_copy_plane(byte* @dst, int @dst_linesize, byte* @src, int @src_linesize, int @bytewidth, int @height);
        
        /// <summary>Copy image data from an image into a buffer.</summary>
        /// <param name="dst">a buffer into which picture data will be copied</param>
        /// <param name="dst_size">the size in bytes of dst</param>
        /// <param name="src_data">pointers containing the source image data</param>
        /// <param name="pix_fmt">the pixel format of the source image</param>
        /// <param name="width">the width of the source image in pixels</param>
        /// <param name="height">the height of the source image in pixels</param>
        /// <param name="align">the assumed linesize alignment for dst</param>
        [DllImport("avutil-55", EntryPoint = "av_image_copy_to_buffer", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern int av_image_copy_to_buffer(byte* @dst, int @dst_size, byte_ptrArray4 @src_data, int_array4 @src_linesize, AVPixelFormat @pix_fmt, int @width, int @height, int @align);
        
        /// <summary>Copy image data located in uncacheable (e.g. GPU mapped) memory. Where available, this function will use special functionality for reading from such memory, which may result in greatly improved performance compared to plain av_image_copy().</summary>
        [DllImport("avutil-55", EntryPoint = "av_image_copy_uc_from", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern void av_image_copy_uc_from(ref byte_ptrArray4 @dst_data, long_array4 @dst_linesizes, ref byte_ptrArray4 @src_data, long_array4 @src_linesizes, AVPixelFormat @pix_fmt, int @width, int @height);
        
        /// <summary>Setup the data pointers and linesizes based on the specified image parameters and the provided array.</summary>
        /// <param name="dst_data">data pointers to be filled in</param>
        /// <param name="src">buffer which will contain or contains the actual image data, can be NULL</param>
        /// <param name="pix_fmt">the pixel format of the image</param>
        /// <param name="width">the width of the image in pixels</param>
        /// <param name="height">the height of the image in pixels</param>
        /// <param name="align">the value used in src for linesize alignment</param>
        [DllImport("avutil-55", EntryPoint = "av_image_fill_arrays", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern int av_image_fill_arrays(ref byte_ptrArray4 @dst_data, ref int_array4 @dst_linesize, byte* @src, AVPixelFormat @pix_fmt, int @width, int @height, int @align);
        
        /// <summary>Fill plane linesizes for an image with pixel format pix_fmt and width width.</summary>
        /// <param name="linesizes">array to be filled with the linesize for each plane</param>
        [DllImport("avutil-55", EntryPoint = "av_image_fill_linesizes", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern int av_image_fill_linesizes(ref int_array4 @linesizes, AVPixelFormat @pix_fmt, int @width);
        
        /// <summary>Compute the max pixel step for each plane of an image with a format described by pixdesc.</summary>
        /// <param name="max_pixsteps">an array which is filled with the max pixel step for each plane. Since a plane may contain different pixel components, the computed max_pixsteps[plane] is relative to the component in the plane with the max pixel step.</param>
        /// <param name="max_pixstep_comps">an array which is filled with the component for each plane which has the max pixel step. May be NULL.</param>
        [DllImport("avutil-55", EntryPoint = "av_image_fill_max_pixsteps", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern void av_image_fill_max_pixsteps(ref int_array4 @max_pixsteps, ref int_array4 @max_pixstep_comps, AVPixFmtDescriptor* @pixdesc);
        
        /// <summary>Fill plane data pointers for an image with pixel format pix_fmt and height height.</summary>
        /// <param name="data">pointers array to be filled with the pointer for each image plane</param>
        /// <param name="ptr">the pointer to a buffer which will contain the image</param>
        /// <param name="linesizes">the array containing the linesize for each plane, should be filled by av_image_fill_linesizes()</param>
        [DllImport("avutil-55", EntryPoint = "av_image_fill_pointers", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern int av_image_fill_pointers(ref byte_ptrArray4 @data, AVPixelFormat @pix_fmt, int @height, byte* @ptr, int_array4 @linesizes);
        
        /// <summary>Return the size in bytes of the amount of data required to store an image with the given parameters.</summary>
        /// <param name="align">the assumed linesize alignment</param>
        [DllImport("avutil-55", EntryPoint = "av_image_get_buffer_size", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern int av_image_get_buffer_size(AVPixelFormat @pix_fmt, int @width, int @height, int @align);
        
        /// <summary>Compute the size of an image line with format pix_fmt and width width for the plane plane.</summary>
        [DllImport("avutil-55", EntryPoint = "av_image_get_linesize", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern int av_image_get_linesize(AVPixelFormat @pix_fmt, int @width, int @plane);
        
        /// <summary>Compute the length of an integer list.</summary>
        /// <param name="elsize">size in bytes of each list element (only 1, 2, 4 or 8)</param>
        /// <param name="list">pointer to the list</param>
        /// <param name="term">list terminator (usually 0 or -1)</param>
        [DllImport("avutil-55", EntryPoint = "av_int_list_length_for_size", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern uint av_int_list_length_for_size(uint @elsize, void* @list, ulong @term);
        
        /// <summary>Send the specified message to the log if the level is less than or equal to the current av_log_level. By default, all logging messages are sent to stderr. This behavior can be altered by setting a different logging callback function.</summary>
        /// <param name="avcl">A pointer to an arbitrary struct of which the first field is a pointer to an AVClass struct or NULL if general log.</param>
        /// <param name="level">The importance level of the message expressed using a</param>
        /// <param name="fmt">The format string (printf-compatible) that specifies how subsequent arguments are converted to output.</param>
        [DllImport("avutil-55", EntryPoint = "av_log", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern void av_log(void* @avcl, int @level, [MarshalAs(UnmanagedType.LPStr)] string @fmt);
        
        /// <summary>Default logging callback</summary>
        /// <param name="avcl">A pointer to an arbitrary struct of which the first field is a pointer to an AVClass struct.</param>
        /// <param name="level">The importance level of the message expressed using a</param>
        /// <param name="fmt">The format string (printf-compatible) that specifies how subsequent arguments are converted to output.</param>
        /// <param name="vl">The arguments referenced by the format string.</param>
        [DllImport("avutil-55", EntryPoint = "av_log_default_callback", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern void av_log_default_callback(void* @avcl, int @level, [MarshalAs(UnmanagedType.LPStr)] string @fmt, byte* @vl);
        
        /// <summary>Format a line of log the same way as the default callback.</summary>
        /// <param name="line">buffer to receive the formatted line</param>
        /// <param name="line_size">size of the buffer</param>
        /// <param name="print_prefix">used to store whether the prefix must be printed; must point to a persistent integer initially set to 1</param>
        [DllImport("avutil-55", EntryPoint = "av_log_format_line", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern void av_log_format_line(void* @ptr, int @level, [MarshalAs(UnmanagedType.LPStr)] string @fmt, byte* @vl, byte* @line, int @line_size, int* @print_prefix);
        
        /// <summary>Format a line of log the same way as the default callback.</summary>
        /// <param name="line">buffer to receive the formatted line; may be NULL if line_size is 0</param>
        /// <param name="line_size">size of the buffer; at most line_size-1 characters will be written to the buffer, plus one null terminator</param>
        /// <param name="print_prefix">used to store whether the prefix must be printed; must point to a persistent integer initially set to 1</param>
        [DllImport("avutil-55", EntryPoint = "av_log_format_line2", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern int av_log_format_line2(void* @ptr, int @level, [MarshalAs(UnmanagedType.LPStr)] string @fmt, byte* @vl, byte* @line, int @line_size, int* @print_prefix);
        
        [DllImport("avutil-55", EntryPoint = "av_log_get_flags", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern int av_log_get_flags();
        
        /// <summary>Get the current log level</summary>
        [DllImport("avutil-55", EntryPoint = "av_log_get_level", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern int av_log_get_level();
        
        /// <summary>Set the logging callback</summary>
        /// <param name="callback">A logging function with a compatible signature.</param>
        [DllImport("avutil-55", EntryPoint = "av_log_set_callback", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern void av_log_set_callback(av_log_set_callback_callback_func @callback);
        
        [DllImport("avutil-55", EntryPoint = "av_log_set_flags", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern void av_log_set_flags(int @arg);
        
        /// <summary>Set the log level</summary>
        /// <param name="level">Logging level</param>
        [DllImport("avutil-55", EntryPoint = "av_log_set_level", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern void av_log_set_level(int @level);
        
        [DllImport("avutil-55", EntryPoint = "av_log2", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern int av_log2(uint @v);
        
        [DllImport("avutil-55", EntryPoint = "av_log2_16bit", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern int av_log2_16bit(uint @v);
        
        /// <summary>Allocate a memory block with alignment suitable for all memory accesses (including vectors if available on the CPU).</summary>
        /// <param name="size">Size in bytes for the memory block to be allocated</param>
        [DllImport("avutil-55", EntryPoint = "av_malloc", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern void* av_malloc(ulong @size);
        
        /// <summary>Allocate a memory block with alignment suitable for all memory accesses (including vectors if available on the CPU) and zero all the bytes of the block.</summary>
        /// <param name="size">Size in bytes for the memory block to be allocated</param>
        [DllImport("avutil-55", EntryPoint = "av_mallocz", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern void* av_mallocz(ulong @size);
        
        /// <summary>Set the maximum size that may be allocated in one block.</summary>
        /// <param name="max">Value to be set as the new maximum size</param>
        [DllImport("avutil-55", EntryPoint = "av_max_alloc", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern void av_max_alloc(ulong @max);
        
        /// <summary>Overlapping memcpy() implementation.</summary>
        /// <param name="dst">Destination buffer</param>
        /// <param name="back">Number of bytes back to start copying (i.e. the initial size of the overlapping window); must be &gt; 0</param>
        /// <param name="cnt">Number of bytes to copy; must be &gt;= 0</param>
        [DllImport("avutil-55", EntryPoint = "av_memcpy_backptr", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern void av_memcpy_backptr(byte* @dst, int @back, int @cnt);
        
        /// <summary>Duplicate a buffer with av_malloc().</summary>
        /// <param name="p">Buffer to be duplicated</param>
        /// <param name="size">Size in bytes of the buffer copied</param>
        [DllImport("avutil-55", EntryPoint = "av_memdup", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern void* av_memdup(void* @p, ulong @size);
        
        /// <summary>Multiply two rationals.</summary>
        /// <param name="b">First rational</param>
        /// <param name="c">Second rational</param>
        [DllImport("avutil-55", EntryPoint = "av_mul_q", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern AVRational av_mul_q(AVRational @b, AVRational @c);
        
        /// <summary>Find which of the two rationals is closer to another rational.</summary>
        /// <param name="q">Rational to be compared against</param>
        [DllImport("avutil-55", EntryPoint = "av_nearer_q", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern int av_nearer_q(AVRational @q, AVRational @q1, AVRational @q2);
        
        /// <summary>Iterate over potential AVOptions-enabled children of parent.</summary>
        /// <param name="prev">result of a previous call to this function or NULL</param>
        [DllImport("avutil-55", EntryPoint = "av_opt_child_class_next", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern AVClass* av_opt_child_class_next(AVClass* @parent, AVClass* @prev);
        
        /// <summary>Iterate over AVOptions-enabled children of obj.</summary>
        /// <param name="prev">result of a previous call to this function or NULL</param>
        [DllImport("avutil-55", EntryPoint = "av_opt_child_next", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern void* av_opt_child_next(void* @obj, void* @prev);
        
        /// <summary>Copy options from src object into dest object.</summary>
        /// <param name="dest">Object to copy from</param>
        /// <param name="src">Object to copy into</param>
        [DllImport("avutil-55", EntryPoint = "av_opt_copy", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern int av_opt_copy(void* @dest, void* @src);
        
        [DllImport("avutil-55", EntryPoint = "av_opt_eval_double", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern int av_opt_eval_double(void* @obj, AVOption* @o, [MarshalAs(UnmanagedType.LPStr)] string @val, double* @double_out);
        
        /// <summary>@{ This group of functions can be used to evaluate option strings and get numbers out of them. They do the same thing as av_opt_set(), except the result is written into the caller-supplied pointer.</summary>
        /// <param name="obj">a struct whose first element is a pointer to AVClass.</param>
        /// <param name="o">an option for which the string is to be evaluated.</param>
        /// <param name="val">string to be evaluated.</param>
        [DllImport("avutil-55", EntryPoint = "av_opt_eval_flags", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern int av_opt_eval_flags(void* @obj, AVOption* @o, [MarshalAs(UnmanagedType.LPStr)] string @val, int* @flags_out);
        
        [DllImport("avutil-55", EntryPoint = "av_opt_eval_float", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern int av_opt_eval_float(void* @obj, AVOption* @o, [MarshalAs(UnmanagedType.LPStr)] string @val, float* @float_out);
        
        [DllImport("avutil-55", EntryPoint = "av_opt_eval_int", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern int av_opt_eval_int(void* @obj, AVOption* @o, [MarshalAs(UnmanagedType.LPStr)] string @val, int* @int_out);
        
        [DllImport("avutil-55", EntryPoint = "av_opt_eval_int64", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern int av_opt_eval_int64(void* @obj, AVOption* @o, [MarshalAs(UnmanagedType.LPStr)] string @val, long* @int64_out);
        
        [DllImport("avutil-55", EntryPoint = "av_opt_eval_q", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern int av_opt_eval_q(void* @obj, AVOption* @o, [MarshalAs(UnmanagedType.LPStr)] string @val, AVRational* @q_out);
        
        /// <summary>Look for an option in an object. Consider only options which have all the specified flags set.</summary>
        /// <param name="obj">A pointer to a struct whose first element is a pointer to an AVClass. Alternatively a double pointer to an AVClass, if AV_OPT_SEARCH_FAKE_OBJ search flag is set.</param>
        /// <param name="name">The name of the option to look for.</param>
        /// <param name="unit">When searching for named constants, name of the unit it belongs to.</param>
        /// <param name="opt_flags">Find only options with all the specified flags set (AV_OPT_FLAG).</param>
        /// <param name="search_flags">A combination of AV_OPT_SEARCH_*.</param>
        [DllImport("avutil-55", EntryPoint = "av_opt_find", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern AVOption* av_opt_find(void* @obj, [MarshalAs(UnmanagedType.LPStr)] string @name, [MarshalAs(UnmanagedType.LPStr)] string @unit, int @opt_flags, int @search_flags);
        
        /// <summary>Look for an option in an object. Consider only options which have all the specified flags set.</summary>
        /// <param name="obj">A pointer to a struct whose first element is a pointer to an AVClass. Alternatively a double pointer to an AVClass, if AV_OPT_SEARCH_FAKE_OBJ search flag is set.</param>
        /// <param name="name">The name of the option to look for.</param>
        /// <param name="unit">When searching for named constants, name of the unit it belongs to.</param>
        /// <param name="opt_flags">Find only options with all the specified flags set (AV_OPT_FLAG).</param>
        /// <param name="search_flags">A combination of AV_OPT_SEARCH_*.</param>
        /// <param name="target_obj">if non-NULL, an object to which the option belongs will be written here. It may be different from obj if AV_OPT_SEARCH_CHILDREN is present in search_flags. This parameter is ignored if search_flags contain AV_OPT_SEARCH_FAKE_OBJ.</param>
        [DllImport("avutil-55", EntryPoint = "av_opt_find2", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern AVOption* av_opt_find2(void* @obj, [MarshalAs(UnmanagedType.LPStr)] string @name, [MarshalAs(UnmanagedType.LPStr)] string @unit, int @opt_flags, int @search_flags, void** @target_obj);
        
        /// <summary>Check whether a particular flag is set in a flags field.</summary>
        /// <param name="field_name">the name of the flag field option</param>
        /// <param name="flag_name">the name of the flag to check</param>
        [DllImport("avutil-55", EntryPoint = "av_opt_flag_is_set", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern int av_opt_flag_is_set(void* @obj, [MarshalAs(UnmanagedType.LPStr)] string @field_name, [MarshalAs(UnmanagedType.LPStr)] string @flag_name);
        
        /// <summary>Free all allocated objects in obj.</summary>
        [DllImport("avutil-55", EntryPoint = "av_opt_free", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern void av_opt_free(void* @obj);
        
        /// <summary>Free an AVOptionRanges struct and set it to NULL.</summary>
        [DllImport("avutil-55", EntryPoint = "av_opt_freep_ranges", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern void av_opt_freep_ranges(AVOptionRanges** @ranges);
        
        /// <summary>@{ Those functions get a value of the option with the given name from an object.</summary>
        /// <param name="obj">a struct whose first element is a pointer to an AVClass.</param>
        /// <param name="name">name of the option to get.</param>
        /// <param name="search_flags">flags passed to av_opt_find2. I.e. if AV_OPT_SEARCH_CHILDREN is passed here, then the option may be found in a child of obj.</param>
        /// <param name="out_val">value of the option will be written here</param>
        [DllImport("avutil-55", EntryPoint = "av_opt_get", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern int av_opt_get(void* @obj, [MarshalAs(UnmanagedType.LPStr)] string @name, int @search_flags, byte** @out_val);
        
        [DllImport("avutil-55", EntryPoint = "av_opt_get_channel_layout", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern int av_opt_get_channel_layout(void* @obj, [MarshalAs(UnmanagedType.LPStr)] string @name, int @search_flags, long* @ch_layout);
        
        /// <param name="out_val">The returned dictionary is a copy of the actual value and must be freed with av_dict_free() by the caller</param>
        [DllImport("avutil-55", EntryPoint = "av_opt_get_dict_val", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern int av_opt_get_dict_val(void* @obj, [MarshalAs(UnmanagedType.LPStr)] string @name, int @search_flags, AVDictionary** @out_val);
        
        [DllImport("avutil-55", EntryPoint = "av_opt_get_double", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern int av_opt_get_double(void* @obj, [MarshalAs(UnmanagedType.LPStr)] string @name, int @search_flags, double* @out_val);
        
        [DllImport("avutil-55", EntryPoint = "av_opt_get_image_size", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern int av_opt_get_image_size(void* @obj, [MarshalAs(UnmanagedType.LPStr)] string @name, int @search_flags, int* @w_out, int* @h_out);
        
        [DllImport("avutil-55", EntryPoint = "av_opt_get_int", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern int av_opt_get_int(void* @obj, [MarshalAs(UnmanagedType.LPStr)] string @name, int @search_flags, long* @out_val);
        
        /// <summary>Extract a key-value pair from the beginning of a string.</summary>
        /// <param name="ropts">pointer to the options string, will be updated to point to the rest of the string (one of the pairs_sep or the final NUL)</param>
        /// <param name="key_val_sep">a 0-terminated list of characters used to separate key from value, for example &apos;=&apos;</param>
        /// <param name="pairs_sep">a 0-terminated list of characters used to separate two pairs from each other, for example &apos;:&apos; or &apos;,&apos;</param>
        /// <param name="flags">flags; see the AV_OPT_FLAG_* values below</param>
        /// <param name="rkey">parsed key; must be freed using av_free()</param>
        /// <param name="rval">parsed value; must be freed using av_free()</param>
        [DllImport("avutil-55", EntryPoint = "av_opt_get_key_value", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern int av_opt_get_key_value(byte** @ropts, [MarshalAs(UnmanagedType.LPStr)] string @key_val_sep, [MarshalAs(UnmanagedType.LPStr)] string @pairs_sep, uint @flags, byte** @rkey, byte** @rval);
        
        [DllImport("avutil-55", EntryPoint = "av_opt_get_pixel_fmt", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern int av_opt_get_pixel_fmt(void* @obj, [MarshalAs(UnmanagedType.LPStr)] string @name, int @search_flags, AVPixelFormat* @out_fmt);
        
        [DllImport("avutil-55", EntryPoint = "av_opt_get_q", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern int av_opt_get_q(void* @obj, [MarshalAs(UnmanagedType.LPStr)] string @name, int @search_flags, AVRational* @out_val);
        
        [DllImport("avutil-55", EntryPoint = "av_opt_get_sample_fmt", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern int av_opt_get_sample_fmt(void* @obj, [MarshalAs(UnmanagedType.LPStr)] string @name, int @search_flags, AVSampleFormat* @out_fmt);
        
        [DllImport("avutil-55", EntryPoint = "av_opt_get_video_rate", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern int av_opt_get_video_rate(void* @obj, [MarshalAs(UnmanagedType.LPStr)] string @name, int @search_flags, AVRational* @out_val);
        
        /// <summary>Check if given option is set to its default value.</summary>
        /// <param name="obj">AVClass object to check option on</param>
        /// <param name="o">option to be checked</param>
        [DllImport("avutil-55", EntryPoint = "av_opt_is_set_to_default", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern int av_opt_is_set_to_default(void* @obj, AVOption* @o);
        
        /// <summary>Check if given option is set to its default value.</summary>
        /// <param name="obj">AVClass object to check option on</param>
        /// <param name="name">option name</param>
        /// <param name="search_flags">combination of AV_OPT_SEARCH_*</param>
        [DllImport("avutil-55", EntryPoint = "av_opt_is_set_to_default_by_name", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern int av_opt_is_set_to_default_by_name(void* @obj, [MarshalAs(UnmanagedType.LPStr)] string @name, int @search_flags);
        
        /// <summary>Iterate over all AVOptions belonging to obj.</summary>
        /// <param name="obj">an AVOptions-enabled struct or a double pointer to an AVClass describing it.</param>
        /// <param name="prev">result of the previous call to av_opt_next() on this object or NULL</param>
        [DllImport("avutil-55", EntryPoint = "av_opt_next", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern AVOption* av_opt_next(void* @obj, AVOption* @prev);
        
        /// <summary>@}</summary>
        [DllImport("avutil-55", EntryPoint = "av_opt_ptr", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern void* av_opt_ptr(AVClass* @avclass, void* @obj, [MarshalAs(UnmanagedType.LPStr)] string @name);
        
        /// <summary>Get a list of allowed ranges for the given option.</summary>
        /// <param name="flags">is a bitmask of flags, undefined flags should not be set and should be ignored AV_OPT_SEARCH_FAKE_OBJ indicates that the obj is a double pointer to a AVClass instead of a full instance AV_OPT_MULTI_COMPONENT_RANGE indicates that function may return more than one component,</param>
        [DllImport("avutil-55", EntryPoint = "av_opt_query_ranges", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern int av_opt_query_ranges(AVOptionRanges** @p0, void* @obj, [MarshalAs(UnmanagedType.LPStr)] string @key, int @flags);
        
        /// <summary>Get a default list of allowed ranges for the given option.</summary>
        /// <param name="flags">is a bitmask of flags, undefined flags should not be set and should be ignored AV_OPT_SEARCH_FAKE_OBJ indicates that the obj is a double pointer to a AVClass instead of a full instance AV_OPT_MULTI_COMPONENT_RANGE indicates that function may return more than one component,</param>
        [DllImport("avutil-55", EntryPoint = "av_opt_query_ranges_default", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern int av_opt_query_ranges_default(AVOptionRanges** @p0, void* @obj, [MarshalAs(UnmanagedType.LPStr)] string @key, int @flags);
        
        /// <summary>Serialize object&apos;s options.</summary>
        /// <param name="obj">AVClass object to serialize</param>
        /// <param name="opt_flags">serialize options with all the specified flags set (AV_OPT_FLAG)</param>
        /// <param name="flags">combination of AV_OPT_SERIALIZE_* flags</param>
        /// <param name="buffer">Pointer to buffer that will be allocated with string containg serialized options. Buffer must be freed by the caller when is no longer needed.</param>
        /// <param name="key_val_sep">character used to separate key from value</param>
        /// <param name="pairs_sep">character used to separate two pairs from each other</param>
        [DllImport("avutil-55", EntryPoint = "av_opt_serialize", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern int av_opt_serialize(void* @obj, int @opt_flags, int @flags, byte** @buffer, byte @key_val_sep, byte @pairs_sep);
        
        /// <summary>@{ Those functions set the field of obj with the given name to value.</summary>
        /// <param name="obj">A struct whose first element is a pointer to an AVClass.</param>
        /// <param name="name">the name of the field to set</param>
        /// <param name="val">The value to set. In case of av_opt_set() if the field is not of a string type, then the given string is parsed. SI postfixes and some named scalars are supported. If the field is of a numeric type, it has to be a numeric or named scalar. Behavior with more than one scalar and +- infix operators is undefined. If the field is of a flags type, it has to be a sequence of numeric scalars or named flags separated by &apos;+&apos; or &apos;-&apos;. Prefixing a flag with &apos;+&apos; causes it to be set without affecting the other flags; similarly, &apos;-&apos; unsets a flag.</param>
        /// <param name="search_flags">flags passed to av_opt_find2. I.e. if AV_OPT_SEARCH_CHILDREN is passed here, then the option may be set on a child of obj.</param>
        [DllImport("avutil-55", EntryPoint = "av_opt_set", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern int av_opt_set(void* @obj, [MarshalAs(UnmanagedType.LPStr)] string @name, [MarshalAs(UnmanagedType.LPStr)] string @val, int @search_flags);
        
        [DllImport("avutil-55", EntryPoint = "av_opt_set_bin", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern int av_opt_set_bin(void* @obj, [MarshalAs(UnmanagedType.LPStr)] string @name, byte* @val, int @size, int @search_flags);
        
        [DllImport("avutil-55", EntryPoint = "av_opt_set_channel_layout", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern int av_opt_set_channel_layout(void* @obj, [MarshalAs(UnmanagedType.LPStr)] string @name, long @ch_layout, int @search_flags);
        
        /// <summary>Set the values of all AVOption fields to their default values.</summary>
        /// <param name="s">an AVOption-enabled struct (its first member must be a pointer to AVClass)</param>
        [DllImport("avutil-55", EntryPoint = "av_opt_set_defaults", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern void av_opt_set_defaults(void* @s);
        
        /// <summary>Set the values of all AVOption fields to their default values. Only these AVOption fields for which (opt-&gt;flags &amp; mask) == flags will have their default applied to s.</summary>
        /// <param name="s">an AVOption-enabled struct (its first member must be a pointer to AVClass)</param>
        /// <param name="mask">combination of AV_OPT_FLAG_*</param>
        /// <param name="flags">combination of AV_OPT_FLAG_*</param>
        [DllImport("avutil-55", EntryPoint = "av_opt_set_defaults2", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern void av_opt_set_defaults2(void* @s, int @mask, int @flags);
        
        /// <summary>Set all the options from a given dictionary on an object.</summary>
        /// <param name="obj">a struct whose first element is a pointer to AVClass</param>
        /// <param name="options">options to process. This dictionary will be freed and replaced by a new one containing all options not found in obj. Of course this new dictionary needs to be freed by caller with av_dict_free().</param>
        [DllImport("avutil-55", EntryPoint = "av_opt_set_dict", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern int av_opt_set_dict(void* @obj, AVDictionary** @options);
        
        [DllImport("avutil-55", EntryPoint = "av_opt_set_dict_val", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern int av_opt_set_dict_val(void* @obj, [MarshalAs(UnmanagedType.LPStr)] string @name, AVDictionary* @val, int @search_flags);
        
        /// <summary>Set all the options from a given dictionary on an object.</summary>
        /// <param name="obj">a struct whose first element is a pointer to AVClass</param>
        /// <param name="options">options to process. This dictionary will be freed and replaced by a new one containing all options not found in obj. Of course this new dictionary needs to be freed by caller with av_dict_free().</param>
        /// <param name="search_flags">A combination of AV_OPT_SEARCH_*.</param>
        [DllImport("avutil-55", EntryPoint = "av_opt_set_dict2", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern int av_opt_set_dict2(void* @obj, AVDictionary** @options, int @search_flags);
        
        [DllImport("avutil-55", EntryPoint = "av_opt_set_double", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern int av_opt_set_double(void* @obj, [MarshalAs(UnmanagedType.LPStr)] string @name, double @val, int @search_flags);
        
        /// <summary>Parse the key-value pairs list in opts. For each key=value pair found, set the value of the corresponding option in ctx.</summary>
        /// <param name="ctx">the AVClass object to set options on</param>
        /// <param name="opts">the options string, key-value pairs separated by a delimiter</param>
        /// <param name="shorthand">a NULL-terminated array of options names for shorthand notation: if the first field in opts has no key part, the key is taken from the first element of shorthand; then again for the second, etc., until either opts is finished, shorthand is finished or a named option is found; after that, all options must be named</param>
        /// <param name="key_val_sep">a 0-terminated list of characters used to separate key from value, for example &apos;=&apos;</param>
        /// <param name="pairs_sep">a 0-terminated list of characters used to separate two pairs from each other, for example &apos;:&apos; or &apos;,&apos;</param>
        [DllImport("avutil-55", EntryPoint = "av_opt_set_from_string", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern int av_opt_set_from_string(void* @ctx, [MarshalAs(UnmanagedType.LPStr)] string @opts, byte** @shorthand, [MarshalAs(UnmanagedType.LPStr)] string @key_val_sep, [MarshalAs(UnmanagedType.LPStr)] string @pairs_sep);
        
        [DllImport("avutil-55", EntryPoint = "av_opt_set_image_size", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern int av_opt_set_image_size(void* @obj, [MarshalAs(UnmanagedType.LPStr)] string @name, int @w, int @h, int @search_flags);
        
        [DllImport("avutil-55", EntryPoint = "av_opt_set_int", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern int av_opt_set_int(void* @obj, [MarshalAs(UnmanagedType.LPStr)] string @name, long @val, int @search_flags);
        
        [DllImport("avutil-55", EntryPoint = "av_opt_set_pixel_fmt", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern int av_opt_set_pixel_fmt(void* @obj, [MarshalAs(UnmanagedType.LPStr)] string @name, AVPixelFormat @fmt, int @search_flags);
        
        [DllImport("avutil-55", EntryPoint = "av_opt_set_q", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern int av_opt_set_q(void* @obj, [MarshalAs(UnmanagedType.LPStr)] string @name, AVRational @val, int @search_flags);
        
        [DllImport("avutil-55", EntryPoint = "av_opt_set_sample_fmt", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern int av_opt_set_sample_fmt(void* @obj, [MarshalAs(UnmanagedType.LPStr)] string @name, AVSampleFormat @fmt, int @search_flags);
        
        [DllImport("avutil-55", EntryPoint = "av_opt_set_video_rate", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern int av_opt_set_video_rate(void* @obj, [MarshalAs(UnmanagedType.LPStr)] string @name, AVRational @val, int @search_flags);
        
        /// <summary>Show the obj options.</summary>
        /// <param name="av_log_obj">log context to use for showing the options</param>
        /// <param name="req_flags">requested flags for the options to show. Show only the options for which it is opt-&gt;flags &amp; req_flags.</param>
        /// <param name="rej_flags">rejected flags for the options to show. Show only the options for which it is !(opt-&gt;flags &amp; req_flags).</param>
        [DllImport("avutil-55", EntryPoint = "av_opt_show2", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern int av_opt_show2(void* @obj, void* @av_log_obj, int @req_flags, int @rej_flags);
        
        /// <summary>Parse CPU caps from a string and update the given AV_CPU_* flags based on that.</summary>
        [DllImport("avutil-55", EntryPoint = "av_parse_cpu_caps", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern int av_parse_cpu_caps(uint* @flags, [MarshalAs(UnmanagedType.LPStr)] string @s);
        
        /// <summary>Parse CPU flags from a string.</summary>
        [Obsolete("")]
        [DllImport("avutil-55", EntryPoint = "av_parse_cpu_flags", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern int av_parse_cpu_flags([MarshalAs(UnmanagedType.LPStr)] string @s);
        
        /// <summary>Returns number of planes in pix_fmt, a negative AVERROR if pix_fmt is not a valid pixel format.</summary>
        [DllImport("avutil-55", EntryPoint = "av_pix_fmt_count_planes", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern int av_pix_fmt_count_planes(AVPixelFormat @pix_fmt);
        
        /// <summary>Returns a pixel format descriptor for provided pixel format or NULL if this pixel format is unknown.</summary>
        [DllImport("avutil-55", EntryPoint = "av_pix_fmt_desc_get", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern AVPixFmtDescriptor* av_pix_fmt_desc_get(AVPixelFormat @pix_fmt);
        
        /// <summary>Returns an AVPixelFormat id described by desc, or AV_PIX_FMT_NONE if desc is not a valid pointer to a pixel format descriptor.</summary>
        [DllImport("avutil-55", EntryPoint = "av_pix_fmt_desc_get_id", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern AVPixelFormat av_pix_fmt_desc_get_id(AVPixFmtDescriptor* @desc);
        
        /// <summary>Iterate over all pixel format descriptors known to libavutil.</summary>
        /// <param name="prev">previous descriptor. NULL to get the first descriptor.</param>
        [DllImport("avutil-55", EntryPoint = "av_pix_fmt_desc_next", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern AVPixFmtDescriptor* av_pix_fmt_desc_next(AVPixFmtDescriptor* @prev);
        
        /// <summary>Utility function to access log2_chroma_w log2_chroma_h from the pixel format AVPixFmtDescriptor.</summary>
        /// <param name="pix_fmt">the pixel format</param>
        /// <param name="h_shift">store log2_chroma_w (horizontal/width shift)</param>
        /// <param name="v_shift">store log2_chroma_h (vertical/height shift)</param>
        [DllImport("avutil-55", EntryPoint = "av_pix_fmt_get_chroma_sub_sample", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern int av_pix_fmt_get_chroma_sub_sample(AVPixelFormat @pix_fmt, int* @h_shift, int* @v_shift);
        
        /// <summary>Utility function to swap the endianness of a pixel format.</summary>
        /// <param name="pix_fmt">the pixel format</param>
        [DllImport("avutil-55", EntryPoint = "av_pix_fmt_swap_endianness", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern AVPixelFormat av_pix_fmt_swap_endianness(AVPixelFormat @pix_fmt);
        
        /// <summary>Convert an AVRational to a IEEE 32-bit `float` expressed in fixed-point format.</summary>
        /// <param name="q">Rational to be converted</param>
        [DllImport("avutil-55", EntryPoint = "av_q2intfloat", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern uint av_q2intfloat(AVRational @q);
        
        /// <summary>Read a line from an image, and write the values of the pixel format component c to dst.</summary>
        /// <param name="data">the array containing the pointers to the planes of the image</param>
        /// <param name="linesize">the array containing the linesizes of the image</param>
        /// <param name="desc">the pixel format descriptor for the image</param>
        /// <param name="x">the horizontal coordinate of the first pixel to read</param>
        /// <param name="y">the vertical coordinate of the first pixel to read</param>
        /// <param name="w">the width of the line to read, that is the number of values to write to dst</param>
        /// <param name="read_pal_component">if not zero and the format is a paletted format writes the values corresponding to the palette component c in data[1] to dst, rather than the palette indexes in data[0]. The behavior is undefined if the format is not paletted.</param>
        [DllImport("avutil-55", EntryPoint = "av_read_image_line", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern void av_read_image_line(ushort* @dst, ref byte_ptrArray4 @data, int_array4 @linesize, AVPixFmtDescriptor* @desc, int @x, int @y, int @c, int @w, int @read_pal_component);
        
        /// <summary>Allocate, reallocate, or free a block of memory.</summary>
        /// <param name="ptr">Pointer to a memory block already allocated with av_realloc() or `NULL`</param>
        /// <param name="size">Size in bytes of the memory block to be allocated or reallocated</param>
        [DllImport("avutil-55", EntryPoint = "av_realloc", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern void* av_realloc(void* @ptr, ulong @size);
        
        /// <summary>Allocate, reallocate, or free an array.</summary>
        /// <param name="ptr">Pointer to a memory block already allocated with av_realloc() or `NULL`</param>
        /// <param name="nmemb">Number of elements in the array</param>
        /// <param name="size">Size of the single element of the array</param>
        [DllImport("avutil-55", EntryPoint = "av_realloc_array", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern void* av_realloc_array(void* @ptr, ulong @nmemb, ulong @size);
        
        /// <summary>Allocate, reallocate, or free a block of memory.</summary>
        [DllImport("avutil-55", EntryPoint = "av_realloc_f", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern void* av_realloc_f(void* @ptr, ulong @nelem, ulong @elsize);
        
        /// <summary>Allocate, reallocate, or free a block of memory through a pointer to a pointer.</summary>
        /// <param name="ptr">Pointer to a pointer to a memory block already allocated with av_realloc(), or a pointer to `NULL`. The pointer is updated on success, or freed on failure.</param>
        /// <param name="size">Size in bytes for the memory block to be allocated or reallocated</param>
        [DllImport("avutil-55", EntryPoint = "av_reallocp", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern int av_reallocp(void* @ptr, ulong @size);
        
        /// <summary>Allocate, reallocate, or free an array through a pointer to a pointer.</summary>
        /// <param name="ptr">Pointer to a pointer to a memory block already allocated with av_realloc(), or a pointer to `NULL`. The pointer is updated on success, or freed on failure.</param>
        /// <param name="nmemb">Number of elements</param>
        /// <param name="size">Size of the single element</param>
        [DllImport("avutil-55", EntryPoint = "av_reallocp_array", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern int av_reallocp_array(void* @ptr, ulong @nmemb, ulong @size);
        
        /// <summary>Reduce a fraction.</summary>
        /// <param name="dst_num">Destination numerator</param>
        /// <param name="dst_den">Destination denominator</param>
        /// <param name="num">Source numerator</param>
        /// <param name="den">Source denominator</param>
        /// <param name="max">Maximum allowed values for `dst_num` &amp; `dst_den`</param>
        [DllImport("avutil-55", EntryPoint = "av_reduce", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern int av_reduce(int* @dst_num, int* @dst_den, long @num, long @den, long @max);
        
        /// <summary>Rescale a 64-bit integer with rounding to nearest.</summary>
        [DllImport("avutil-55", EntryPoint = "av_rescale", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern long av_rescale(long @a, long @b, long @c);
        
        /// <summary>Rescale a timestamp while preserving known durations.</summary>
        /// <param name="in_tb">Input time base</param>
        /// <param name="in_ts">Input timestamp</param>
        /// <param name="fs_tb">Duration time base; typically this is finer-grained (greater) than `in_tb` and `out_tb`</param>
        /// <param name="duration">Duration till the next call to this function (i.e. duration of the current packet/frame)</param>
        /// <param name="last">Pointer to a timestamp expressed in terms of `fs_tb`, acting as a state variable</param>
        /// <param name="out_tb">Output timebase</param>
        [DllImport("avutil-55", EntryPoint = "av_rescale_delta", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern long av_rescale_delta(AVRational @in_tb, long @in_ts, AVRational @fs_tb, int @duration, long* @last, AVRational @out_tb);
        
        /// <summary>Rescale a 64-bit integer by 2 rational numbers.</summary>
        [DllImport("avutil-55", EntryPoint = "av_rescale_q", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern long av_rescale_q(long @a, AVRational @bq, AVRational @cq);
        
        /// <summary>Rescale a 64-bit integer by 2 rational numbers with specified rounding.</summary>
        [DllImport("avutil-55", EntryPoint = "av_rescale_q_rnd", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern long av_rescale_q_rnd(long @a, AVRational @bq, AVRational @cq, AVRounding @rnd);
        
        /// <summary>Rescale a 64-bit integer with specified rounding.</summary>
        [DllImport("avutil-55", EntryPoint = "av_rescale_rnd", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern long av_rescale_rnd(long @a, long @b, long @c, AVRounding @rnd);
        
        /// <summary>Check if the sample format is planar.</summary>
        /// <param name="sample_fmt">the sample format to inspect</param>
        [DllImport("avutil-55", EntryPoint = "av_sample_fmt_is_planar", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern int av_sample_fmt_is_planar(AVSampleFormat @sample_fmt);
        
        /// <summary>Allocate a samples buffer for nb_samples samples, and fill data pointers and linesize accordingly. The allocated samples buffer can be freed by using av_freep(&amp;audio_data[0]) Allocated data will be initialized to silence.</summary>
        /// <param name="audio_data">array to be filled with the pointer for each channel</param>
        /// <param name="linesize">aligned size for audio buffer(s), may be NULL</param>
        /// <param name="nb_channels">number of audio channels</param>
        /// <param name="nb_samples">number of samples per channel</param>
        /// <param name="align">buffer size alignment (0 = default, 1 = no alignment)</param>
        [DllImport("avutil-55", EntryPoint = "av_samples_alloc", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern int av_samples_alloc(byte** @audio_data, int* @linesize, int @nb_channels, int @nb_samples, AVSampleFormat @sample_fmt, int @align);
        
        /// <summary>Allocate a data pointers array, samples buffer for nb_samples samples, and fill data pointers and linesize accordingly.</summary>
        [DllImport("avutil-55", EntryPoint = "av_samples_alloc_array_and_samples", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern int av_samples_alloc_array_and_samples(byte*** @audio_data, int* @linesize, int @nb_channels, int @nb_samples, AVSampleFormat @sample_fmt, int @align);
        
        /// <summary>Copy samples from src to dst.</summary>
        /// <param name="dst">destination array of pointers to data planes</param>
        /// <param name="src">source array of pointers to data planes</param>
        /// <param name="dst_offset">offset in samples at which the data will be written to dst</param>
        /// <param name="src_offset">offset in samples at which the data will be read from src</param>
        /// <param name="nb_samples">number of samples to be copied</param>
        /// <param name="nb_channels">number of audio channels</param>
        /// <param name="sample_fmt">audio sample format</param>
        [DllImport("avutil-55", EntryPoint = "av_samples_copy", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern int av_samples_copy(byte** @dst, byte** @src, int @dst_offset, int @src_offset, int @nb_samples, int @nb_channels, AVSampleFormat @sample_fmt);
        
        /// <summary>Fill plane data pointers and linesize for samples with sample format sample_fmt.</summary>
        /// <param name="audio_data">array to be filled with the pointer for each channel</param>
        /// <param name="linesize">calculated linesize, may be NULL</param>
        /// <param name="buf">the pointer to a buffer containing the samples</param>
        /// <param name="nb_channels">the number of channels</param>
        /// <param name="nb_samples">the number of samples in a single channel</param>
        /// <param name="sample_fmt">the sample format</param>
        /// <param name="align">buffer size alignment (0 = default, 1 = no alignment)</param>
        [DllImport("avutil-55", EntryPoint = "av_samples_fill_arrays", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern int av_samples_fill_arrays(byte** @audio_data, int* @linesize, byte* @buf, int @nb_channels, int @nb_samples, AVSampleFormat @sample_fmt, int @align);
        
        /// <summary>Get the required buffer size for the given audio parameters.</summary>
        /// <param name="linesize">calculated linesize, may be NULL</param>
        /// <param name="nb_channels">the number of channels</param>
        /// <param name="nb_samples">the number of samples in a single channel</param>
        /// <param name="sample_fmt">the sample format</param>
        /// <param name="align">buffer size alignment (0 = default, 1 = no alignment)</param>
        [DllImport("avutil-55", EntryPoint = "av_samples_get_buffer_size", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern int av_samples_get_buffer_size(int* @linesize, int @nb_channels, int @nb_samples, AVSampleFormat @sample_fmt, int @align);
        
        /// <summary>Fill an audio buffer with silence.</summary>
        /// <param name="audio_data">array of pointers to data planes</param>
        /// <param name="offset">offset in samples at which to start filling</param>
        /// <param name="nb_samples">number of samples to fill</param>
        /// <param name="nb_channels">number of audio channels</param>
        /// <param name="sample_fmt">audio sample format</param>
        [DllImport("avutil-55", EntryPoint = "av_samples_set_silence", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern int av_samples_set_silence(byte** @audio_data, int @offset, int @nb_samples, int @nb_channels, AVSampleFormat @sample_fmt);
        
        /// <summary>Set a mask on flags returned by av_get_cpu_flags(). This function is mainly useful for testing. Please use av_force_cpu_flags() and av_get_cpu_flags() instead which are more flexible</summary>
        [Obsolete("")]
        [DllImport("avutil-55", EntryPoint = "av_set_cpu_flags_mask", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern void av_set_cpu_flags_mask(int @mask);
        
        /// <summary>Parse the key/value pairs list in opts. For each key/value pair found, stores the value in the field in ctx that is named like the key. ctx must be an AVClass context, storing is done using AVOptions.</summary>
        /// <param name="opts">options string to parse, may be NULL</param>
        /// <param name="key_val_sep">a 0-terminated list of characters used to separate key from value</param>
        /// <param name="pairs_sep">a 0-terminated list of characters used to separate two pairs from each other</param>
        [DllImport("avutil-55", EntryPoint = "av_set_options_string", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern int av_set_options_string(void* @ctx, [MarshalAs(UnmanagedType.LPStr)] string @opts, [MarshalAs(UnmanagedType.LPStr)] string @key_val_sep, [MarshalAs(UnmanagedType.LPStr)] string @pairs_sep);
        
        /// <summary>Duplicate a string.</summary>
        /// <param name="s">String to be duplicated</param>
        [DllImport("avutil-55", EntryPoint = "av_strdup", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern byte* av_strdup([MarshalAs(UnmanagedType.LPStr)] string @s);
        
        /// <summary>Put a description of the AVERROR code errnum in errbuf. In case of failure the global variable errno is set to indicate the error. Even in case of failure av_strerror() will print a generic error message indicating the errnum provided to errbuf.</summary>
        /// <param name="errnum">error code to describe</param>
        /// <param name="errbuf">buffer to which description is written</param>
        /// <param name="errbuf_size">the size in bytes of errbuf</param>
        [DllImport("avutil-55", EntryPoint = "av_strerror", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern int av_strerror(int @errnum, byte* @errbuf, ulong @errbuf_size);
        
        /// <summary>Duplicate a substring of a string.</summary>
        /// <param name="s">String to be duplicated</param>
        /// <param name="len">Maximum length of the resulting string (not counting the terminating byte)</param>
        [DllImport("avutil-55", EntryPoint = "av_strndup", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern byte* av_strndup([MarshalAs(UnmanagedType.LPStr)] string @s, ulong @len);
        
        /// <summary>Subtract one rational from another.</summary>
        /// <param name="b">First rational</param>
        /// <param name="c">Second rational</param>
        [DllImport("avutil-55", EntryPoint = "av_sub_q", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern AVRational av_sub_q(AVRational @b, AVRational @c);
        
        /// <summary>Adjust frame number for NTSC drop frame time code.</summary>
        /// <param name="framenum">frame number to adjust</param>
        /// <param name="fps">frame per second, 30 or 60</param>
        [DllImport("avutil-55", EntryPoint = "av_timecode_adjust_ntsc_framenum2", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern int av_timecode_adjust_ntsc_framenum2(int @framenum, int @fps);
        
        /// <summary>Check if the timecode feature is available for the given frame rate</summary>
        [DllImport("avutil-55", EntryPoint = "av_timecode_check_frame_rate", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern int av_timecode_check_frame_rate(AVRational @rate);
        
        /// <summary>Convert frame number to SMPTE 12M binary representation.</summary>
        /// <param name="tc">timecode data correctly initialized</param>
        /// <param name="framenum">frame number</param>
        [DllImport("avutil-55", EntryPoint = "av_timecode_get_smpte_from_framenum", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern uint av_timecode_get_smpte_from_framenum(AVTimecode* @tc, int @framenum);
        
        /// <summary>Init a timecode struct with the passed parameters.</summary>
        /// <param name="tc">pointer to an allocated AVTimecode</param>
        /// <param name="rate">frame rate in rational form</param>
        /// <param name="flags">miscellaneous flags such as drop frame, +24 hours, ... (see AVTimecodeFlag)</param>
        /// <param name="frame_start">the first frame number</param>
        /// <param name="log_ctx">a pointer to an arbitrary struct of which the first field is a pointer to an AVClass struct (used for av_log)</param>
        [DllImport("avutil-55", EntryPoint = "av_timecode_init", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern int av_timecode_init(AVTimecode* @tc, AVRational @rate, int @flags, int @frame_start, void* @log_ctx);
        
        /// <summary>Parse timecode representation (hh:mm:ss[:;.]ff).</summary>
        /// <param name="tc">pointer to an allocated AVTimecode</param>
        /// <param name="rate">frame rate in rational form</param>
        /// <param name="str">timecode string which will determine the frame start</param>
        /// <param name="log_ctx">a pointer to an arbitrary struct of which the first field is a pointer to an AVClass struct (used for av_log).</param>
        [DllImport("avutil-55", EntryPoint = "av_timecode_init_from_string", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern int av_timecode_init_from_string(AVTimecode* @tc, AVRational @rate, [MarshalAs(UnmanagedType.LPStr)] string @str, void* @log_ctx);
        
        /// <summary>Get the timecode string from the 25-bit timecode format (MPEG GOP format).</summary>
        /// <param name="buf">destination buffer, must be at least AV_TIMECODE_STR_SIZE long</param>
        /// <param name="tc25bit">the 25-bits timecode</param>
        [DllImport("avutil-55", EntryPoint = "av_timecode_make_mpeg_tc_string", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern byte* av_timecode_make_mpeg_tc_string(byte* @buf, uint @tc25bit);
        
        /// <summary>Get the timecode string from the SMPTE timecode format.</summary>
        /// <param name="buf">destination buffer, must be at least AV_TIMECODE_STR_SIZE long</param>
        /// <param name="tcsmpte">the 32-bit SMPTE timecode</param>
        /// <param name="prevent_df">prevent the use of a drop flag when it is known the DF bit is arbitrary</param>
        [DllImport("avutil-55", EntryPoint = "av_timecode_make_smpte_tc_string", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern byte* av_timecode_make_smpte_tc_string(byte* @buf, uint @tcsmpte, int @prevent_df);
        
        /// <summary>Load timecode string in buf.</summary>
        /// <param name="tc">timecode data correctly initialized</param>
        /// <param name="buf">destination buffer, must be at least AV_TIMECODE_STR_SIZE long</param>
        /// <param name="framenum">frame number</param>
        [DllImport("avutil-55", EntryPoint = "av_timecode_make_string", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern byte* av_timecode_make_string(AVTimecode* @tc, byte* @buf, int @framenum);
        
        /// <summary>Return an informative version string. This usually is the actual release version number or a git commit description. This string has no fixed format and can change any time. It should never be parsed by code.</summary>
        [DllImport("avutil-55", EntryPoint = "av_version_info", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        [return: MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof(ConstCharPtrMarshaler))]
        public static extern string av_version_info();
        
        /// <summary>Send the specified message to the log if the level is less than or equal to the current av_log_level. By default, all logging messages are sent to stderr. This behavior can be altered by setting a different logging callback function.</summary>
        /// <param name="avcl">A pointer to an arbitrary struct of which the first field is a pointer to an AVClass struct.</param>
        /// <param name="level">The importance level of the message expressed using a</param>
        /// <param name="fmt">The format string (printf-compatible) that specifies how subsequent arguments are converted to output.</param>
        /// <param name="vl">The arguments referenced by the format string.</param>
        [DllImport("avutil-55", EntryPoint = "av_vlog", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern void av_vlog(void* @avcl, int @level, [MarshalAs(UnmanagedType.LPStr)] string @fmt, byte* @vl);
        
        /// <summary>Write the values from src to the pixel format component c of an image line.</summary>
        /// <param name="src">array containing the values to write</param>
        /// <param name="data">the array containing the pointers to the planes of the image to write into. It is supposed to be zeroed.</param>
        /// <param name="linesize">the array containing the linesizes of the image</param>
        /// <param name="desc">the pixel format descriptor for the image</param>
        /// <param name="x">the horizontal coordinate of the first pixel to write</param>
        /// <param name="y">the vertical coordinate of the first pixel to write</param>
        /// <param name="w">the width of the line to write, that is the number of values to write to the image line</param>
        [DllImport("avutil-55", EntryPoint = "av_write_image_line", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern void av_write_image_line(ushort* @src, ref byte_ptrArray4 @data, int_array4 @linesize, AVPixFmtDescriptor* @desc, int @x, int @y, int @c, int @w);
        
        [DllImport("avutil-55", EntryPoint = "avpriv_frame_get_metadatap", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern AVDictionary** avpriv_frame_get_metadatap(AVFrame* @frame);
        
        /// <summary>Return the libavutil build-time configuration.</summary>
        [DllImport("avutil-55", EntryPoint = "avutil_configuration", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        [return: MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof(ConstCharPtrMarshaler))]
        public static extern string avutil_configuration();
        
        /// <summary>Return the libavutil license.</summary>
        [DllImport("avutil-55", EntryPoint = "avutil_license", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        [return: MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof(ConstCharPtrMarshaler))]
        public static extern string avutil_license();
        
        /// <summary>Return the LIBAVUTIL_VERSION_INT constant.</summary>
        [DllImport("avutil-55", EntryPoint = "avutil_version", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern uint avutil_version();
        
        /// <summary>Return the libpostproc build-time configuration.</summary>
        [DllImport("postproc-54", EntryPoint = "postproc_configuration", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        [return: MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof(ConstCharPtrMarshaler))]
        public static extern string postproc_configuration();
        
        /// <summary>Return the libpostproc license.</summary>
        [DllImport("postproc-54", EntryPoint = "postproc_license", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        [return: MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof(ConstCharPtrMarshaler))]
        public static extern string postproc_license();
        
        /// <summary>Return the LIBPOSTPROC_VERSION_INT constant.</summary>
        [DllImport("postproc-54", EntryPoint = "postproc_version", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern uint postproc_version();
        
        [DllImport("postproc-54", EntryPoint = "pp_free_context", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern void pp_free_context(void* @ppContext);
        
        [DllImport("postproc-54", EntryPoint = "pp_free_mode", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern void pp_free_mode(void* @mode);
        
        [DllImport("postproc-54", EntryPoint = "pp_get_context", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern void* pp_get_context(int @width, int @height, int @flags);
        
        /// <summary>Return a pp_mode or NULL if an error occurred.</summary>
        /// <param name="name">the string after &quot;-pp&quot; on the command line</param>
        /// <param name="quality">a number from 0 to PP_QUALITY_MAX</param>
        [DllImport("postproc-54", EntryPoint = "pp_get_mode_by_name_and_quality", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern void* pp_get_mode_by_name_and_quality([MarshalAs(UnmanagedType.LPStr)] string @name, int @quality);
        
        [DllImport("postproc-54", EntryPoint = "pp_postprocess", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern void pp_postprocess(ref byte_ptrArray3 @src, int_array3 @srcStride, ref byte_ptrArray3 @dst, int_array3 @dstStride, int @horizontalSize, int @verticalSize, sbyte* @QP_store, int @QP_stride, void* @mode, void* @ppContext, int @pict_type);
        
        /// <summary>Allocate SwrContext.</summary>
        [DllImport("swresample-2", EntryPoint = "swr_alloc", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern SwrContext* swr_alloc();
        
        /// <summary>Allocate SwrContext if needed and set/reset common parameters.</summary>
        /// <param name="s">existing Swr context if available, or NULL if not</param>
        /// <param name="out_ch_layout">output channel layout (AV_CH_LAYOUT_*)</param>
        /// <param name="out_sample_fmt">output sample format (AV_SAMPLE_FMT_*).</param>
        /// <param name="out_sample_rate">output sample rate (frequency in Hz)</param>
        /// <param name="in_ch_layout">input channel layout (AV_CH_LAYOUT_*)</param>
        /// <param name="in_sample_fmt">input sample format (AV_SAMPLE_FMT_*).</param>
        /// <param name="in_sample_rate">input sample rate (frequency in Hz)</param>
        /// <param name="log_offset">logging level offset</param>
        /// <param name="log_ctx">parent logging context, can be NULL</param>
        [DllImport("swresample-2", EntryPoint = "swr_alloc_set_opts", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern SwrContext* swr_alloc_set_opts(SwrContext* @s, long @out_ch_layout, AVSampleFormat @out_sample_fmt, int @out_sample_rate, long @in_ch_layout, AVSampleFormat @in_sample_fmt, int @in_sample_rate, int @log_offset, void* @log_ctx);
        
        /// <summary>Generate a channel mixing matrix.</summary>
        /// <param name="in_layout">input channel layout</param>
        /// <param name="out_layout">output channel layout</param>
        /// <param name="center_mix_level">mix level for the center channel</param>
        /// <param name="surround_mix_level">mix level for the surround channel(s)</param>
        /// <param name="lfe_mix_level">mix level for the low-frequency effects channel</param>
        /// <param name="rematrix_maxval">if 1.0, coefficients will be normalized to prevent overflow. if INT_MAX, coefficients will not be normalized.</param>
        /// <param name="matrix">mixing coefficients; matrix[i + stride * o] is the weight of input channel i in output channel o.</param>
        /// <param name="stride">distance between adjacent input channels in the matrix array</param>
        /// <param name="matrix_encoding">matrixed stereo downmix mode (e.g. dplii)</param>
        /// <param name="log_ctx">parent logging context, can be NULL</param>
        [DllImport("swresample-2", EntryPoint = "swr_build_matrix", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern int swr_build_matrix(ulong @in_layout, ulong @out_layout, double @center_mix_level, double @surround_mix_level, double @lfe_mix_level, double @rematrix_maxval, double @rematrix_volume, double* @matrix, int @stride, AVMatrixEncoding @matrix_encoding, void* @log_ctx);
        
        /// <summary>Closes the context so that swr_is_initialized() returns 0.</summary>
        /// <param name="s">Swr context to be closed</param>
        [DllImport("swresample-2", EntryPoint = "swr_close", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern void swr_close(SwrContext* @s);
        
        /// <summary>Configure or reconfigure the SwrContext using the information provided by the AVFrames.</summary>
        /// <param name="swr">audio resample context</param>
        [DllImport("swresample-2", EntryPoint = "swr_config_frame", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern int swr_config_frame(SwrContext* @swr, AVFrame* @out, AVFrame* @in);
        
        /// <summary>Convert audio.</summary>
        /// <param name="s">allocated Swr context, with parameters set</param>
        /// <param name="out">output buffers, only the first one need be set in case of packed audio</param>
        /// <param name="out_count">amount of space available for output in samples per channel</param>
        /// <param name="in">input buffers, only the first one need to be set in case of packed audio</param>
        /// <param name="in_count">number of input samples available in one channel</param>
        [DllImport("swresample-2", EntryPoint = "swr_convert", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern int swr_convert(SwrContext* @s, byte** @out, int @out_count, byte** @in, int @in_count);
        
        /// <summary>Convert the samples in the input AVFrame and write them to the output AVFrame.</summary>
        /// <param name="swr">audio resample context</param>
        /// <param name="output">output AVFrame</param>
        /// <param name="input">input AVFrame</param>
        [DllImport("swresample-2", EntryPoint = "swr_convert_frame", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern int swr_convert_frame(SwrContext* @swr, AVFrame* @output, AVFrame* @input);
        
        /// <summary>Drops the specified number of output samples.</summary>
        /// <param name="s">allocated Swr context</param>
        /// <param name="count">number of samples to be dropped</param>
        [DllImport("swresample-2", EntryPoint = "swr_drop_output", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern int swr_drop_output(SwrContext* @s, int @count);
        
        /// <summary>Free the given SwrContext and set the pointer to NULL.</summary>
        /// <param name="s">a pointer to a pointer to Swr context</param>
        [DllImport("swresample-2", EntryPoint = "swr_free", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern void swr_free(SwrContext** @s);
        
        /// <summary>Get the AVClass for SwrContext. It can be used in combination with AV_OPT_SEARCH_FAKE_OBJ for examining options.</summary>
        [DllImport("swresample-2", EntryPoint = "swr_get_class", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern AVClass* swr_get_class();
        
        /// <summary>Gets the delay the next input sample will experience relative to the next output sample.</summary>
        /// <param name="s">swr context</param>
        /// <param name="base">timebase in which the returned delay will be:</param>
        [DllImport("swresample-2", EntryPoint = "swr_get_delay", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern long swr_get_delay(SwrContext* @s, long @base);
        
        /// <summary>Find an upper bound on the number of samples that the next swr_convert call will output, if called with in_samples of input samples. This depends on the internal state, and anything changing the internal state (like further swr_convert() calls) will may change the number of samples swr_get_out_samples() returns for the same number of input samples.</summary>
        /// <param name="in_samples">number of input samples.</param>
        [DllImport("swresample-2", EntryPoint = "swr_get_out_samples", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern int swr_get_out_samples(SwrContext* @s, int @in_samples);
        
        /// <summary>Initialize context after user parameters have been set.</summary>
        /// <param name="s">Swr context to initialize</param>
        [DllImport("swresample-2", EntryPoint = "swr_init", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern int swr_init(SwrContext* @s);
        
        /// <summary>Injects the specified number of silence samples.</summary>
        /// <param name="s">allocated Swr context</param>
        /// <param name="count">number of samples to be dropped</param>
        [DllImport("swresample-2", EntryPoint = "swr_inject_silence", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern int swr_inject_silence(SwrContext* @s, int @count);
        
        /// <summary>Check whether an swr context has been initialized or not.</summary>
        /// <param name="s">Swr context to check</param>
        [DllImport("swresample-2", EntryPoint = "swr_is_initialized", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern int swr_is_initialized(SwrContext* @s);
        
        /// <summary>Convert the next timestamp from input to output timestamps are in 1/(in_sample_rate * out_sample_rate) units.</summary>
        [DllImport("swresample-2", EntryPoint = "swr_next_pts", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern long swr_next_pts(SwrContext* @s, long @pts);
        
        /// <summary>Set a customized input channel mapping.</summary>
        /// <param name="s">allocated Swr context, not yet initialized</param>
        /// <param name="channel_map">customized input channel mapping (array of channel indexes, -1 for a muted channel)</param>
        [DllImport("swresample-2", EntryPoint = "swr_set_channel_mapping", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern int swr_set_channel_mapping(SwrContext* @s, int* @channel_map);
        
        /// <summary>Activate resampling compensation (&quot;soft&quot; compensation). This function is internally called when needed in swr_next_pts().</summary>
        /// <param name="s">allocated Swr context. If it is not initialized, or SWR_FLAG_RESAMPLE is not set, swr_init() is called with the flag set.</param>
        /// <param name="sample_delta">delta in PTS per sample</param>
        /// <param name="compensation_distance">number of samples to compensate for</param>
        [DllImport("swresample-2", EntryPoint = "swr_set_compensation", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern int swr_set_compensation(SwrContext* @s, int @sample_delta, int @compensation_distance);
        
        /// <summary>Set a customized remix matrix.</summary>
        /// <param name="s">allocated Swr context, not yet initialized</param>
        /// <param name="matrix">remix coefficients; matrix[i + stride * o] is the weight of input channel i in output channel o</param>
        /// <param name="stride">offset between lines of the matrix</param>
        [DllImport("swresample-2", EntryPoint = "swr_set_matrix", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern int swr_set_matrix(SwrContext* @s, double* @matrix, int @stride);
        
        /// <summary>Return the swr build-time configuration.</summary>
        [DllImport("swresample-2", EntryPoint = "swresample_configuration", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        [return: MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof(ConstCharPtrMarshaler))]
        public static extern string swresample_configuration();
        
        /// <summary>Return the swr license.</summary>
        [DllImport("swresample-2", EntryPoint = "swresample_license", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        [return: MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof(ConstCharPtrMarshaler))]
        public static extern string swresample_license();
        
        /// <summary>Return the</summary>
        [DllImport("swresample-2", EntryPoint = "swresample_version", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern uint swresample_version();
        
        [Obsolete("")]
        [DllImport("swscale-4", EntryPoint = "sws_addVec", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern void sws_addVec(SwsVector* @a, SwsVector* @b);
        
        /// <summary>Allocate an empty SwsContext. This must be filled and passed to sws_init_context(). For filling see AVOptions, options.c and sws_setColorspaceDetails().</summary>
        [DllImport("swscale-4", EntryPoint = "sws_alloc_context", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern SwsContext* sws_alloc_context();
        
        /// <summary>Allocate and return an uninitialized vector with length coefficients.</summary>
        [DllImport("swscale-4", EntryPoint = "sws_allocVec", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern SwsVector* sws_allocVec(int @length);
        
        [Obsolete("")]
        [DllImport("swscale-4", EntryPoint = "sws_cloneVec", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern SwsVector* sws_cloneVec(SwsVector* @a);
        
        /// <summary>Convert an 8-bit paletted frame into a frame with a color depth of 24 bits.</summary>
        /// <param name="src">source frame buffer</param>
        /// <param name="dst">destination frame buffer</param>
        /// <param name="num_pixels">number of pixels to convert</param>
        /// <param name="palette">array with [256] entries, which must match color arrangement (RGB or BGR) of src</param>
        [DllImport("swscale-4", EntryPoint = "sws_convertPalette8ToPacked24", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern void sws_convertPalette8ToPacked24(byte* @src, byte* @dst, int @num_pixels, byte* @palette);
        
        /// <summary>Convert an 8-bit paletted frame into a frame with a color depth of 32 bits.</summary>
        /// <param name="src">source frame buffer</param>
        /// <param name="dst">destination frame buffer</param>
        /// <param name="num_pixels">number of pixels to convert</param>
        /// <param name="palette">array with [256] entries, which must match color arrangement (RGB or BGR) of src</param>
        [DllImport("swscale-4", EntryPoint = "sws_convertPalette8ToPacked32", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern void sws_convertPalette8ToPacked32(byte* @src, byte* @dst, int @num_pixels, byte* @palette);
        
        [Obsolete("")]
        [DllImport("swscale-4", EntryPoint = "sws_convVec", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern void sws_convVec(SwsVector* @a, SwsVector* @b);
        
        /// <summary>Free the swscaler context swsContext. If swsContext is NULL, then does nothing.</summary>
        [DllImport("swscale-4", EntryPoint = "sws_freeContext", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern void sws_freeContext(SwsContext* @swsContext);
        
        [DllImport("swscale-4", EntryPoint = "sws_freeFilter", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern void sws_freeFilter(SwsFilter* @filter);
        
        [DllImport("swscale-4", EntryPoint = "sws_freeVec", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern void sws_freeVec(SwsVector* @a);
        
        /// <summary>Get the AVClass for swsContext. It can be used in combination with AV_OPT_SEARCH_FAKE_OBJ for examining options.</summary>
        [DllImport("swscale-4", EntryPoint = "sws_get_class", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern AVClass* sws_get_class();
        
        /// <summary>Check if context can be reused, otherwise reallocate a new one.</summary>
        [DllImport("swscale-4", EntryPoint = "sws_getCachedContext", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern SwsContext* sws_getCachedContext(SwsContext* @context, int @srcW, int @srcH, AVPixelFormat @srcFormat, int @dstW, int @dstH, AVPixelFormat @dstFormat, int @flags, SwsFilter* @srcFilter, SwsFilter* @dstFilter, double* @param);
        
        /// <summary>Return a pointer to yuv&lt;-&gt;rgb coefficients for the given colorspace suitable for sws_setColorspaceDetails().</summary>
        /// <param name="colorspace">One of the SWS_CS_* macros. If invalid, SWS_CS_DEFAULT is used.</param>
        [DllImport("swscale-4", EntryPoint = "sws_getCoefficients", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern int* sws_getCoefficients(int @colorspace);
        
        /// <summary>Returns -1 if not supported</summary>
        [DllImport("swscale-4", EntryPoint = "sws_getColorspaceDetails", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern int sws_getColorspaceDetails(SwsContext* @c, int** @inv_table, int* @srcRange, int** @table, int* @dstRange, int* @brightness, int* @contrast, int* @saturation);
        
        [Obsolete("")]
        [DllImport("swscale-4", EntryPoint = "sws_getConstVec", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern SwsVector* sws_getConstVec(double @c, int @length);
        
        /// <summary>Allocate and return an SwsContext. You need it to perform scaling/conversion operations using sws_scale().</summary>
        /// <param name="srcW">the width of the source image</param>
        /// <param name="srcH">the height of the source image</param>
        /// <param name="srcFormat">the source image format</param>
        /// <param name="dstW">the width of the destination image</param>
        /// <param name="dstH">the height of the destination image</param>
        /// <param name="dstFormat">the destination image format</param>
        /// <param name="flags">specify which algorithm and options to use for rescaling</param>
        /// <param name="param">extra parameters to tune the used scaler For SWS_BICUBIC param[0] and [1] tune the shape of the basis function, param[0] tunes f(1) and param[1] f´(1) For SWS_GAUSS param[0] tunes the exponent and thus cutoff frequency For SWS_LANCZOS param[0] tunes the width of the window function</param>
        [DllImport("swscale-4", EntryPoint = "sws_getContext", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern SwsContext* sws_getContext(int @srcW, int @srcH, AVPixelFormat @srcFormat, int @dstW, int @dstH, AVPixelFormat @dstFormat, int @flags, SwsFilter* @srcFilter, SwsFilter* @dstFilter, double* @param);
        
        [DllImport("swscale-4", EntryPoint = "sws_getDefaultFilter", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern SwsFilter* sws_getDefaultFilter(float @lumaGBlur, float @chromaGBlur, float @lumaSharpen, float @chromaSharpen, float @chromaHShift, float @chromaVShift, int @verbose);
        
        /// <summary>Return a normalized Gaussian curve used to filter stuff quality = 3 is high quality, lower is lower quality.</summary>
        [DllImport("swscale-4", EntryPoint = "sws_getGaussianVec", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern SwsVector* sws_getGaussianVec(double @variance, double @quality);
        
        [Obsolete("")]
        [DllImport("swscale-4", EntryPoint = "sws_getIdentityVec", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern SwsVector* sws_getIdentityVec();
        
        /// <summary>Initialize the swscaler context sws_context.</summary>
        [DllImport("swscale-4", EntryPoint = "sws_init_context", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern int sws_init_context(SwsContext* @sws_context, SwsFilter* @srcFilter, SwsFilter* @dstFilter);
        
        /// <summary>Returns a positive value if an endianness conversion for pix_fmt is supported, 0 otherwise.</summary>
        /// <param name="pix_fmt">the pixel format</param>
        [DllImport("swscale-4", EntryPoint = "sws_isSupportedEndiannessConversion", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern int sws_isSupportedEndiannessConversion(AVPixelFormat @pix_fmt);
        
        /// <summary>Return a positive value if pix_fmt is a supported input format, 0 otherwise.</summary>
        [DllImport("swscale-4", EntryPoint = "sws_isSupportedInput", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern int sws_isSupportedInput(AVPixelFormat @pix_fmt);
        
        /// <summary>Return a positive value if pix_fmt is a supported output format, 0 otherwise.</summary>
        [DllImport("swscale-4", EntryPoint = "sws_isSupportedOutput", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern int sws_isSupportedOutput(AVPixelFormat @pix_fmt);
        
        /// <summary>Scale all the coefficients of a so that their sum equals height.</summary>
        [DllImport("swscale-4", EntryPoint = "sws_normalizeVec", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern void sws_normalizeVec(SwsVector* @a, double @height);
        
        [Obsolete("")]
        [DllImport("swscale-4", EntryPoint = "sws_printVec2", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern void sws_printVec2(SwsVector* @a, AVClass* @log_ctx, int @log_level);
        
        /// <summary>Scale the image slice in srcSlice and put the resulting scaled slice in the image in dst. A slice is a sequence of consecutive rows in an image.</summary>
        /// <param name="c">the scaling context previously created with sws_getContext()</param>
        /// <param name="srcSlice">the array containing the pointers to the planes of the source slice</param>
        /// <param name="srcStride">the array containing the strides for each plane of the source image</param>
        /// <param name="srcSliceY">the position in the source image of the slice to process, that is the number (counted starting from zero) in the image of the first row of the slice</param>
        /// <param name="srcSliceH">the height of the source slice, that is the number of rows in the slice</param>
        /// <param name="dst">the array containing the pointers to the planes of the destination image</param>
        /// <param name="dstStride">the array containing the strides for each plane of the destination image</param>
        [DllImport("swscale-4", EntryPoint = "sws_scale", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern int sws_scale(SwsContext* @c, byte*[] @srcSlice, int[] @srcStride, int @srcSliceY, int @srcSliceH, byte*[] @dst, int[] @dstStride);
        
        /// <summary>Scale all the coefficients of a by the scalar value.</summary>
        [DllImport("swscale-4", EntryPoint = "sws_scaleVec", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern void sws_scaleVec(SwsVector* @a, double @scalar);
        
        /// <summary>Returns -1 if not supported</summary>
        /// <param name="inv_table">the yuv2rgb coefficients describing the input yuv space, normally ff_yuv2rgb_coeffs[x]</param>
        /// <param name="srcRange">flag indicating the while-black range of the input (1=jpeg / 0=mpeg)</param>
        /// <param name="table">the yuv2rgb coefficients describing the output yuv space, normally ff_yuv2rgb_coeffs[x]</param>
        /// <param name="dstRange">flag indicating the while-black range of the output (1=jpeg / 0=mpeg)</param>
        /// <param name="brightness">16.16 fixed point brightness correction</param>
        /// <param name="contrast">16.16 fixed point contrast correction</param>
        /// <param name="saturation">16.16 fixed point saturation correction</param>
        [DllImport("swscale-4", EntryPoint = "sws_setColorspaceDetails", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern int sws_setColorspaceDetails(SwsContext* @c, int_array4 @inv_table, int @srcRange, int_array4 @table, int @dstRange, int @brightness, int @contrast, int @saturation);
        
        [Obsolete("")]
        [DllImport("swscale-4", EntryPoint = "sws_shiftVec", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern void sws_shiftVec(SwsVector* @a, int @shift);
        
        [Obsolete("")]
        [DllImport("swscale-4", EntryPoint = "sws_subVec", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern void sws_subVec(SwsVector* @a, SwsVector* @b);
        
        /// <summary>Return the libswscale build-time configuration.</summary>
        [DllImport("swscale-4", EntryPoint = "swscale_configuration", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        [return: MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof(ConstCharPtrMarshaler))]
        public static extern string swscale_configuration();
        
        /// <summary>Return the libswscale license.</summary>
        [DllImport("swscale-4", EntryPoint = "swscale_license", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        [return: MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof(ConstCharPtrMarshaler))]
        public static extern string swscale_license();
        
        /// <summary>Color conversion and scaling library.</summary>
        [DllImport("swscale-4", EntryPoint = "swscale_version", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern uint swscale_version();
        
    }
}
