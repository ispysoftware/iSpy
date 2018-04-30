using System;
using System.Runtime.InteropServices;
using FFmpeg.AutoGen;
using NAudio.Wave;

namespace iSpyApplication.Sources.Audio.talk
{
    internal unsafe class WaveFormatConverter
    {
        private readonly WaveFormat _fromFormat, _toFormat;
        private SwrContext* _swrContext;
        private GCHandle _convHandle;
        readonly byte[] _convOut = new byte[44100];

        public WaveFormatConverter(WaveFormat fromFormat, WaveFormat toFormat)
        {
            _fromFormat = fromFormat;
            _toFormat = toFormat;

            _swrContext = ffmpeg.swr_alloc_set_opts(null,
                ffmpeg.av_get_default_channel_layout(_toFormat.Channels),
                AVSampleFormat.AV_SAMPLE_FMT_S16,
                _toFormat.SampleRate,
                ffmpeg.av_get_default_channel_layout(_fromFormat.Channels),
                AVSampleFormat.AV_SAMPLE_FMT_S16,
                _fromFormat.SampleRate,
                0,
                null);
            ffmpeg.swr_init(_swrContext);

            _convHandle = GCHandle.Alloc(_convOut, GCHandleType.Pinned);
        }

        public void Close()
        {
            if (_convHandle.IsAllocated)
                _convHandle.Free();

            if (_swrContext != null)
            {
                ffmpeg.swr_close(_swrContext);
                _swrContext = null;
            }
        }

        public byte[] Process(byte[] audio, int dataLen)
        {
            fixed (byte* p = audio)
            {
                byte* inPointerLocal = p;
                var ptr = _convHandle.AddrOfPinnedObject().ToPointer();
                byte* convOutPointerLocal = (byte*)ptr;
                int srcNbSamples = dataLen / 2;//16 bit
                int dstNbSamples =
                    (int)
                    ffmpeg.av_rescale_rnd(
                        ffmpeg.swr_get_delay(_swrContext, _fromFormat.SampleRate) + srcNbSamples, _toFormat.SampleRate, _fromFormat.SampleRate, AVRounding.AV_ROUND_UP);

                var samples = ffmpeg.swr_convert(_swrContext, &convOutPointerLocal, dstNbSamples, &inPointerLocal, srcNbSamples);
                if (samples < 0)
                {
                    throw new Exception("Error encoding audio");
                }

                byte[] convertedAudio = new byte[samples * 2];
                Buffer.BlockCopy(_convOut, 0, convertedAudio, 0, samples * 2);
                return convertedAudio;
            }
        }
    }
}
