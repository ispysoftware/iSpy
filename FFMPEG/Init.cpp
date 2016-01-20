#include "StdAfx.h"
#include "Init.h"

namespace libffmpeg
{
    extern "C"
        {
			#pragma warning(disable:4635) 
			#pragma warning(disable:4244) 
 
			#include "libavformat\avformat.h"
			#include "libavcodec\avcodec.h"
        }
}

namespace iSpy { 
	namespace Video { 
		namespace FFMPEG {
				void Init::Initialise()	{
					libffmpeg::av_register_all( );
					libffmpeg::avcodec_register_all();
					libffmpeg::avformat_network_init();
				}
				void Init::DeInitialise()	{
					libffmpeg::avformat_network_deinit();
				}		
		}
	}
}