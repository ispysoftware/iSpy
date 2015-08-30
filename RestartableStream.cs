using System;
using System.Collections.Generic;
using System.IO;

namespace iSpyApplication
{
    public sealed class RestartableReadStream : Stream
    {
        private readonly Stream _inner;
        private List<byte[]> _buffers;
        private bool _buffering;
        private int? _currentBuffer;
        private int? _currentBufferPosition;
        public RestartableReadStream(Stream inner)
        {
            if (!inner.CanRead) throw new NotSupportedException(); //Don't know what else is being expected of us
            if (inner.CanSeek) throw new NotSupportedException(); //Just use the underlying streams ability to seek, no need for this class
            _inner = inner;
            _buffering = true;
            _buffers = new List<byte[]>();
        }

        public void StopBuffering()
        {
            _buffering = false;
            if (!_currentBuffer.HasValue)
            {
                //We aren't currently using the buffers
                _buffers = null;
                _currentBufferPosition = null;
            }
        }

        public void Restart()
        {
            if (!_buffering) throw new NotSupportedException();  //Buffering got turned off already
            if (_buffers.Count == 0) return;
            _currentBuffer = 0;
            _currentBufferPosition = 0;
        }

        public override void Flush()
        {
            _inner.Flush();
        }

        public override long Seek(long offset, SeekOrigin origin)
        {
            return 0L;
        }

        public override void SetLength(long value)
        {
            
        }

        public override int Read(byte[] buffer, int offset, int count)
        {
            if (_currentBuffer.HasValue)
            {
                //Try to satisfy the read request from the current buffer
                byte[] rbuffer = _buffers[_currentBuffer.Value];
                if (_currentBufferPosition != null)
                {
                    int roffset = _currentBufferPosition.Value;
                    if ((rbuffer.Length - roffset) <= count)
                    {
                        //Just give them what we have in the current buffer (exhausting it)
                        count = (rbuffer.Length - roffset);
                        for (int i = 0; i < count; i++)
                        {
                            buffer[offset + i] = rbuffer[roffset + i];
                        }

                        _currentBuffer++;
                        if (_currentBuffer.Value == _buffers.Count)
                        {
                            //We've stopped reading from the buffers
                            if (!_buffering)
                                _buffers = null;
                            _currentBuffer = null;
                            _currentBufferPosition = null;
                        }
                        return count;
                    }
                    else
                    {
                        for (int i = 0; i < count; i++)
                        {
                            buffer[offset + i] = rbuffer[roffset + i];
                        }
                        _currentBufferPosition += count;
                        return count;
                    }
                }
            }
            //If we reach here, we're currently using the inner stream. But may be buffering the results
            int ncount = _inner.Read(buffer, offset, count);
            if (_buffering)
            {
                byte[] rbuffer = new byte[ncount];
                for (int i = 0; i < ncount; i++)
                {
                    rbuffer[i] = buffer[offset + i];
                }
                _buffers.Add(rbuffer);
            }
            return ncount;
        }

        public override void Write(byte[] buffer, int offset, int count)
        {
            _inner.Write(buffer,offset,count);
        }

        public override bool CanRead => true;

        public override bool CanSeek => false;

        public override bool CanWrite => true;

        public override long Length => 0L;
        
        public override long Position { get; set; }
    }
}
