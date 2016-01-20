// iSpy FFMPEG Library
// http://www.ispyconnect.com
//
// Copyright © ispyconnect.com, 2013
#include "StdAfx.h"
#include "VideoCodec.h"

namespace libffmpeg
{
	extern "C"
	{
		#pragma warning(disable:4635) 
		#pragma warning(disable:4244) 
		#include "libavcodec\avcodec.h"
	}
}

int video_codecs[] =
{
	libffmpeg::AV_CODEC_ID_MPEG4,
	libffmpeg::AV_CODEC_ID_WMV1,
	libffmpeg::AV_CODEC_ID_WMV2,
	libffmpeg::AV_CODEC_ID_MSMPEG4V2,
	libffmpeg::AV_CODEC_ID_MSMPEG4V3,
	libffmpeg::AV_CODEC_ID_H263P,
	libffmpeg::AV_CODEC_ID_FLV1,
	libffmpeg::AV_CODEC_ID_MPEG2VIDEO,
	libffmpeg::AV_CODEC_ID_RAWVIDEO,
	libffmpeg::AV_CODEC_ID_H264,
	libffmpeg::AV_CODEC_ID_MJPEG,
	libffmpeg::AV_CODEC_ID_H265
};

int pixel_formats[] =
{
	libffmpeg::PIX_FMT_YUV420P,
	libffmpeg::PIX_FMT_YUV420P,
	libffmpeg::PIX_FMT_YUV420P,
	libffmpeg::PIX_FMT_YUV420P,
	libffmpeg::PIX_FMT_YUV420P,
	libffmpeg::PIX_FMT_YUV420P,
	libffmpeg::PIX_FMT_YUV420P,
	libffmpeg::PIX_FMT_YUV420P,
	libffmpeg::PIX_FMT_BGR24,
	libffmpeg::PIX_FMT_YUV420P,
	libffmpeg::PIX_FMT_YUV420P,
	libffmpeg::PIX_FMT_YUV420P,
};

int CODECS_COUNT ( sizeof( video_codecs ) / sizeof( libffmpeg::AVCodecID ) );