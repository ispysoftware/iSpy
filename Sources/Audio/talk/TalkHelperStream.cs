using System;
using System.IO;
using NAudio.Wave;

namespace iSpyApplication.Sources.Audio.talk
{
    class TalkHelperStream: WaveStream
    {
        WaveFormat format;
        long position = 0;
        long length;
        private byte[] _buffer;

        public TalkHelperStream(byte[] src, long length, WaveFormat format)
        {
            this.format = format;
            this.length = length;
            _buffer = src;
        }

        public override WaveFormat WaveFormat
        {
            get { return format; }
        }

        public override long Length
        {
            get { return length; }
        }

        public override long Position
        {
            get
            {
                return position;
            }
            set
            {
                position =  value;
            }
        }

        public override int Read(byte[] dest, int offset, int count)
        {
            if (position >= length)
            {
                return 0;
            }
            count = (int)Math.Min(count, length - position);

            Buffer.BlockCopy(_buffer, (int) position, dest, offset, count);
            position += count;
            return count;
        }
    }
}
