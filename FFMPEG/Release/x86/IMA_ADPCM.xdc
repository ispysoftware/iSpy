<?xml version="1.0"?><doc>
<members>
<member name="T:iSpy.Video.FFMPEG.IMA_ADPCM" decl="false" source="d:\projects\ispy\ispy\ffmpeg\ima_adpcm.h" line="47">
@defgroup ima_adpcm Audio Codec - IMA ADPCM
@{

@brief A class which implements the IMA ADPCM audio coding algorithm.

Typically, IMA ADPCM data is stored as blocks of data with the #PredictedValue
and #StepIndex values held in a header to each block. When decoding, these values
should be writen to this class to initialise the decoding of a block. When encoding,
these values should be writen out to a block's header prior to encoding the samples
in the block.

Note, when IMA ADPCM data is stored in Microsoft WAV files, the #PredictedValue
found in the block's header is used as the first sample for that block. The first ADPCM
value in the block's data then represents the second sample.

@version 2006-05-20
	- Changed code to use standard typedefs, e.g. replaced uint8 with uint8_t, and made use of size_t.

</member>
<member name="M:iSpy.Video.FFMPEG.IMA_ADPCM.EncodeInit(System.Int16,System.Int16)" decl="true" source="d:\projects\ispy\ispy\ffmpeg\ima_adpcm.h" line="71">
	Initialise the #PredictedValue and #StepIndex members to the
	optimum values for encoding an audio stream whoes first two PCM samples have the
	values given. Use of this method at the start of audio stream encoding gives
	improved accuracy over a naive initialisation which sets #PredictedValue
	and #StepIndex to predetermined constant values.

	@param sample1 The first PCM sample in the audio stream.
	@param sample2 The second PCM sample in the audio stream.

</member>
<member name="M:iSpy.Video.FFMPEG.IMA_ADPCM.Encode(System.Int16)" decl="true" source="d:\projects\ispy\ispy\ffmpeg\ima_adpcm.h" line="83">
	Encode a single linear PCM sample as an ADPCM value.

	@param pcm16 The PCM value to encode.

	@return The 4 least significan bits represent the encoded ADPCM value.
			Other bits are zero.

	@post #PredictedValue and #StepIndex are updated ready
		  for encoding the next sample.

</member>
<member name="M:iSpy.Video.FFMPEG.IMA_ADPCM.Decode(System.UInt32)" decl="true" source="d:\projects\ispy\ispy\ffmpeg\ima_adpcm.h" line="96">
	Decode a single ADPCM value into a 16bit linear PCM value.

	@param adpcm The 4 least significan bits represent ADPCM value to encode.
				 Other bits are ignored.

	@return The decoded 16 bit PCM value sign extended to an int.

	@post #PredictedValue and #StepIndex are updated ready
		  for decoding the next sample.

</member>
<!-- Discarding badly formed XML document comment for member 'M:iSpy.Video.FFMPEG.IMA_ADPCM.Encode(System.Byte*,System.Int32,System.Int16!System.Runtime.CompilerServices.IsConst*,System.UInt32)'. -->
<!-- Discarding badly formed XML document comment for member 'M:iSpy.Video.FFMPEG.IMA_ADPCM.Decode(System.Int16*,System.Byte!System.Runtime.CompilerServices.IsConst*,System.Int32,System.UInt32)'. -->
<member name="F:iSpy.Video.FFMPEG.IMA_ADPCM.PredictedValue" decl="false" source="d:\projects\ispy\ispy\ffmpeg\ima_adpcm.h" line="160">
The predicted value of the next sample.
Typically, this value is read from the header, or written to the header, of a block
of ADPCM values.

</member>
<member name="F:iSpy.Video.FFMPEG.IMA_ADPCM.StepIndex" decl="false" source="d:\projects\ispy\ispy\ffmpeg\ima_adpcm.h" line="167">
The step index used for the next ADPCM value
Typically, this value is read from the header, or written to the header, of a block
of ADPCM values.

</member>
</members>
</doc>