using System;
using NAudio.Wave;

namespace iSpyApplication.Sources.Audio.talk
{
    class TalkHelperStream: WaveStream
    {
        private readonly byte[] _sourceBuffer;
        private long _position;
        private readonly long _bufferLength;

        public TalkHelperStream(byte[] sourceBuffer, long bufferLength, WaveFormat waveFormat)
        {
            _sourceBuffer = sourceBuffer;
            WaveFormat = waveFormat;
            _bufferLength = bufferLength;
        }

        /// <summary>
        /// The WaveFormat of this stream
        /// </summary>
        public override WaveFormat WaveFormat { get; }

        /// <summary>
        /// The length in bytes of this stream (if supported)
        /// </summary>
        public override long Length => _bufferLength;

        /// <summary>
        /// The current position in this stream
        /// </summary>
        public override long Position
        {
            get
            {
                return _position;
            }
            set
            {
                _position = value;
            }
        }

        /// <summary>
        /// Reads data from the stream
        /// </summary>
        public override int Read(byte[] buffer, int offset, int count)
        {
            var pos = (int) _position;
            int b = (int) _bufferLength - (pos + offset);
            if (b < count) count = b;
            Buffer.BlockCopy(_sourceBuffer, pos + offset, buffer, 0, count);
            _position += count;
            return count;
        }
    }
}
