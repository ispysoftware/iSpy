// iSpy FFMPEG Library
// http://www.ispyconnect.com
//
// Copyright © ispyconnect.com, 2013
#include "StdAfx.h"
#include "AudioCodec.h"

namespace libffmpeg
{
	extern "C"
	{
		#pragma warning(disable:4635) 
		#pragma warning(disable:4244) 
		#include "libavcodec\avcodec.h"
	}
}

int audio_codecs[] =
{
	libffmpeg::AV_CODEC_ID_MP3,
	libffmpeg::AV_CODEC_ID_AAC,
	libffmpeg::AV_CODEC_ID_MP4ALS,
	libffmpeg::AV_CODEC_ID_WMAV1
};


int AUDIO_CODECS_COUNT ( sizeof( audio_codecs ) / sizeof( libffmpeg::AVCodecID ) );