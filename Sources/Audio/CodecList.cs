using System.Collections.Generic;
using iSpyApplication.Sources.Audio.codecs;

namespace iSpyApplication.Sources.Audio
{
    static class CodecList
    {
        private static List<INetworkChatCodec> _codecs;
        public static List<INetworkChatCodec> Codecs
        {
            get
            {
                if (_codecs != null)
                    return _codecs;
                _codecs = new List<INetworkChatCodec>
                              {
                                  new AcmALawChatCodec(),
                                  new G722ChatCodec(),
                                  new Gsm610ChatCodec(),
                                  new MicrosoftAdpcmChatCodec(),
                                  new MuLawChatCodec(),
                                  new NarrowBandSpeexCodec(),
                                  new TrueSpeechChatCodec(),
                                  new UltraWideBandSpeexCodec(),
                                  new WideBandSpeexCodec(),
                                  new UncompressedPcmChatCodec()
                              };

                return _codecs;
            }
        }
    }
}
