// iSpy FFMPEG Library
// http://www.ispyconnect.com
//
// Copyright © ispyconnect.com, 2013
#pragma once

using namespace System;

extern int audio_codecs[];

extern int AUDIO_CODECS_COUNT;

namespace iSpy { namespace Video { namespace FFMPEG
{
	/// <summary>
	/// Enumeration of some audio codecs from FFmpeg library, which are available for writing audio files.
	/// </summary>
	public enum class AudioCodec
	{
		None = -1,
		/// <summary>
		/// MPEG-3
		/// </summary>
		MP3 = 0,
		AAC,
		M4A,
		WMAV1
	};

} } }