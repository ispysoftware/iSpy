using NAudio.Wave;
using NSpeex;

namespace iSpyApplication.Sources.Audio.codecs
{
    class NarrowBandSpeexCodec : SpeexChatCodec
    {
        public NarrowBandSpeexCodec() : 
            base(BandMode.Narrow, 8000, "Speex Narrow Band")
        {
        
        }
    }

    class WideBandSpeexCodec : SpeexChatCodec
    {
        public WideBandSpeexCodec() : 
            base(BandMode.Wide, 16000, "Speex Wide Band (16kHz)")
        {

        }
    }

    class UltraWideBandSpeexCodec : SpeexChatCodec
    {
        public UltraWideBandSpeexCodec() :
            base(BandMode.UltraWide, 32000, "Speex Ultra Wide Band (32kHz)")
        {

        }
    }

    class SpeexChatCodec : INetworkChatCodec
    {
        private readonly WaveFormat _recordingFormat;
        private readonly SpeexDecoder _decoder;
        private readonly SpeexEncoder _encoder;
        private readonly WaveBuffer _encoderInputBuffer;

        public SpeexChatCodec(BandMode bandMode, int sampleRate, string description)
        {
            _decoder = new SpeexDecoder(bandMode);
            _encoder = new SpeexEncoder(bandMode);
            _recordingFormat = new WaveFormat(sampleRate, 16, 1);
            Name = description;
            _encoderInputBuffer = new WaveBuffer(_recordingFormat.AverageBytesPerSecond); // more than enough
        }

        public string Name { get; }

        public int BitsPerSecond => -1;

        public WaveFormat RecordFormat => _recordingFormat;

        public byte[] Encode(byte[] data, int offset, int length)
        {
            FeedSamplesIntoEncoderInputBuffer(data, offset, length);
            int samplesToEncode = _encoderInputBuffer.ShortBufferCount;
            if (samplesToEncode % _encoder.FrameSize != 0)
            {
                samplesToEncode -= samplesToEncode % _encoder.FrameSize;
            }
            byte[] outputBufferTemp = new byte[length]; // contains more than enough space
            int bytesWritten = _encoder.Encode(_encoderInputBuffer.ShortBuffer, 0, samplesToEncode, outputBufferTemp, 0, length);
            byte[] encoded = new byte[bytesWritten];
            System.Array.Copy(outputBufferTemp, 0, encoded, 0, bytesWritten);
            ShiftLeftoverSamplesDown(samplesToEncode);
            return encoded;
        }

        private void ShiftLeftoverSamplesDown(int samplesEncoded)
        {
            int leftoverSamples = _encoderInputBuffer.ShortBufferCount - samplesEncoded;
            System.Array.Copy(_encoderInputBuffer.ByteBuffer, samplesEncoded * 2, _encoderInputBuffer.ByteBuffer, 0, leftoverSamples * 2);
            _encoderInputBuffer.ShortBufferCount = leftoverSamples;
        }

        private void FeedSamplesIntoEncoderInputBuffer(byte[] data, int offset, int length)
        {
            System.Array.Copy(data, offset, _encoderInputBuffer.ByteBuffer, _encoderInputBuffer.ByteBufferCount, length);
            _encoderInputBuffer.ByteBufferCount += length;
        }

        public byte[] Decode(byte[] data, int offset, int length)
        {
            byte[] outputBufferTemp = new byte[length * 320];
            WaveBuffer wb = new WaveBuffer(outputBufferTemp);
            int samplesDecoded = _decoder.Decode(data, offset, length, wb.ShortBuffer, 0, false);
            int bytesDecoded = samplesDecoded * 2;
            byte[] decoded = new byte[bytesDecoded];
            System.Array.Copy(outputBufferTemp, 0, decoded, 0, bytesDecoded);
            return decoded;
        }

        public void Dispose()
        {
            // nothing to do
        }

        public bool IsAvailable => true;
    }
}
