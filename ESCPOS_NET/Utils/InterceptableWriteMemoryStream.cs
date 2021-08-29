using System;
using System.IO;

namespace ESCPOS_NET
{
    public class InterceptableWriteMemoryStream : Stream
    {
        private MemoryStream _innerStream = new MemoryStream();
        public override bool CanRead => _innerStream.CanRead;

        public override bool CanSeek => _innerStream.CanSeek;

        public override bool CanWrite => _innerStream.CanWrite;

        public override long Length => _innerStream.Length;

        public override long Position { get => _innerStream.Position; set => _innerStream.Position = value; }

        private Action<byte[]> _writeAction { get; set; }
        public InterceptableWriteMemoryStream(Action<byte[]> writeAction)
        {
            _writeAction = writeAction;   
        }
        public override void Flush()
        {
            _innerStream.Flush();
        }

        public override int Read(byte[] buffer, int offset, int count)
        {
            return _innerStream.Read(buffer, offset, count);
        }

        public override long Seek(long offset, SeekOrigin origin)
        {
            return _innerStream.Seek(offset, origin);
        }

        public override void SetLength(long value)
        {
            _innerStream.SetLength(value);
        }

        public override void Write(byte[] buffer, int offset, int count)
        {
            var writeArray = new byte[count];
            Buffer.BlockCopy(buffer, offset, writeArray, 0, count);
            _writeAction(writeArray);
        }
    }
}
