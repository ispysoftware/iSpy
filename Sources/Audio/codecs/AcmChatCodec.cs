using NAudio;
using NAudio.Wave;
using NAudio.Wave.Compression;

namespace iSpyApplication.Sources.Audio.codecs
{
    /// <summary>
    /// useful base class for deriving any chat codecs that will use ACM for decode and encode
    /// </summary>
    abstract class AcmChatCodec : INetworkChatCodec
    {
        private readonly WaveFormat _encodeFormat;
        private AcmStream _encodeStream;
        private AcmStream _decodeStream;
        private int _decodeSourceBytesLeftovers;
        private int _encodeSourceBytesLeftovers;

        protected AcmChatCodec(WaveFormat recordFormat, WaveFormat encodeFormat)
        {
            RecordFormat = recordFormat;
            _encodeFormat = encodeFormat;
        }

        public WaveFormat RecordFormat { get; }

        public byte[] Encode(byte[] data, int offset, int length)
        {
            if (_encodeStream == null)
            {
                _encodeStream = new AcmStream(RecordFormat, _encodeFormat);
            }
            return Convert(_encodeStream, data, offset, length, ref _encodeSourceBytesLeftovers);
        }

        public byte[] Decode(byte[] data, int offset, int length)
        {
            if (_decodeStream == null)
            {
                _decodeStream = new AcmStream(_encodeFormat, RecordFormat);
            }
            return Convert(_decodeStream, data, offset, length, ref _decodeSourceBytesLeftovers);
        }

        private static byte[] Convert(AcmStream conversionStream, byte[] data, int offset, int length, ref int sourceBytesLeftovers)
        {
            int bytesInSourceBuffer = length + sourceBytesLeftovers;
            System.Array.Copy(data, offset, conversionStream.SourceBuffer, sourceBytesLeftovers, length);
            int sourceBytesConverted;
            int bytesConverted = conversionStream.Convert(bytesInSourceBuffer, out sourceBytesConverted);
            sourceBytesLeftovers = bytesInSourceBuffer - sourceBytesConverted;
            if (sourceBytesLeftovers > 0)
            {
                // shift the leftovers down
                System.Array.Copy(conversionStream.SourceBuffer, sourceBytesConverted, conversionStream.SourceBuffer, 0, sourceBytesLeftovers);
            }
            byte[] encoded = new byte[bytesConverted];
            System.Array.Copy(conversionStream.DestBuffer, 0, encoded, 0, bytesConverted);
            return encoded;
        }

        public abstract string Name { get; }

        public int BitsPerSecond => _encodeFormat.AverageBytesPerSecond * 8;

        public void Dispose()
        {
            if (_encodeStream != null)
            {
                _encodeStream.Dispose();
                _encodeStream = null;
            }
            if (_decodeStream != null)
            {
                _decodeStream.Dispose();
                _decodeStream = null;
            }
        }

        public bool IsAvailable
        {
            get
            {
                // determine if this codec is installed on this PC
                bool available = true;
                try
                {
                    using (new AcmStream(RecordFormat, _encodeFormat))
                    { }
                    using (new AcmStream(_encodeFormat, RecordFormat))
                    { }
                }
                catch (MmException)
                {
                    available = false;
                }
                return available;
            }
        }
    }
}
