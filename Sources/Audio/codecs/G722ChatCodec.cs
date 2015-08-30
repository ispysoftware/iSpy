using System;
using NAudio.Codecs;
using NAudio.Wave;

namespace iSpyApplication.Sources.Audio.codecs
{
    class G722ChatCodec : INetworkChatCodec
    {
        private readonly int _bitrate;
        private readonly G722CodecState _encoderState;
        private readonly G722CodecState _decoderState;
        private readonly G722Codec _codec;

        public G722ChatCodec()
        {
            _bitrate = 64000;
            _encoderState = new G722CodecState(_bitrate, G722Flags.None);
            _decoderState = new G722CodecState(_bitrate, G722Flags.None);
            _codec = new G722Codec();
            RecordFormat = new WaveFormat(16000, 1);
        }

        public string Name => "G.722 16kHz";

        public int BitsPerSecond => _bitrate;

        public WaveFormat RecordFormat { get; }

        public byte[] Encode(byte[] data, int offset, int length)
        {
            if (offset != 0)
            {
                throw new ArgumentException("G722 does not yet support non-zero offsets");
            }
            WaveBuffer wb = new WaveBuffer(data);
            int encodedLength = length / 4;
            byte[] outputBuffer = new byte[encodedLength];
            _codec.Encode(_encoderState, outputBuffer, wb.ShortBuffer, length / 2);
            return outputBuffer;
        }

        public byte[] Decode(byte[] data, int offset, int length)
        {
            if (offset != 0)
            {
                throw new ArgumentException("G722 does not yet support non-zero offsets");
            }
            int decodedLength = length * 4;
            byte[] outputBuffer = new byte[decodedLength];
            WaveBuffer wb = new WaveBuffer(outputBuffer);
            _codec.Decode(_decoderState, wb.ShortBuffer, data, length);
            return outputBuffer;
        }

        public void Dispose()
        {
            // nothing to do
        }

        public bool IsAvailable => true;
    }
}
