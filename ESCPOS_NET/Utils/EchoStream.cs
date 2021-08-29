
using System;
using System.IO;
using System.Threading.Tasks;
using System.Threading;
using System.Collections.Concurrent;

namespace ESCPOS_NET.Utils
{

    public class EchoStream : Stream
    {
        public override bool CanTimeout { get; } = true;
        public override int ReadTimeout { get; set; } = Timeout.Infinite;
        public override int WriteTimeout { get; set; } = Timeout.Infinite;
        public override bool CanRead { get; } = true;
        public override bool CanSeek { get; } = false;
        public override bool CanWrite { get; } = true;

        public bool CopyBufferOnWrite { get; set; } = false;

        private readonly object _lock = new object();

        // Default underlying mechanism for BlockingCollection is ConcurrentQueue<T>, which is what we want
        private readonly BlockingCollection<byte[]> _Buffers;
        private int _maxQueueDepth = 10;

        private byte[] m_buffer = null;
        private int m_offset = 0;
        private int m_count = 0;

        private bool m_Closed = false;
        public override void Close()
        {
            m_Closed = true;

            // release any waiting writes
            _Buffers.CompleteAdding();
        }

        public bool DataAvailable
        {
            get
            {
                return _Buffers.Count > 0;
            }
        }

        private long _Length = 0L;
        public override long Length
        {
            get
            {
                return _Length;
            }
        }

        private long _Position = 0L;
        public override long Position
        {
            get
            {
                return _Position;
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public EchoStream() : this(10)
        {
        }

        public EchoStream(int maxQueueDepth)
        {
            _maxQueueDepth = maxQueueDepth;
            _Buffers = new BlockingCollection<byte[]>(_maxQueueDepth);
        }

        // we override the xxxxAsync functions because the default base class shares state between ReadAsync and WriteAsync, which causes a hang if both are called at once
        public new Task WriteAsync(byte[] buffer, int offset, int count)
        {
            return Task.Run(() => Write(buffer, offset, count));
        }

        // we override the xxxxAsync functions because the default base class shares state between ReadAsync and WriteAsync, which causes a hang if both are called at once
        public new Task<int> ReadAsync(byte[] buffer, int offset, int count)
        {
            return Task.Run(() =>
            {
                return Read(buffer, offset, count);
            });
        }

        public override void Write(byte[] buffer, int offset, int count)
        {
            if (m_Closed || buffer.Length - offset < count || count <= 0)
                return;

            byte[] newBuffer;
            if (!CopyBufferOnWrite && offset == 0 && count == buffer.Length)
                newBuffer = buffer;
            else
            {
                newBuffer = new byte[count];
                System.Buffer.BlockCopy(buffer, offset, newBuffer, 0, count);
            }
            if (!_Buffers.TryAdd(newBuffer, WriteTimeout))
                throw new TimeoutException("EchoStream Write() Timeout");

            _Length += count;
        }

        public override int Read(byte[] buffer, int offset, int count)
        {
            if (count == 0)
                return 0;
            lock (_lock)
            {
                if (m_count == 0 && _Buffers.Count == 0)
                {
                    if (m_Closed)
                        return -1;

                    if (_Buffers.TryTake(out m_buffer, ReadTimeout))
                    {
                        m_offset = 0;
                        m_count = m_buffer.Length;
                    }
                    else
                        return m_Closed ? -1 : 0;
                }

                int returnBytes = 0;
                while (count > 0)
                {
                    if (m_count == 0)
                    {
                        if (_Buffers.TryTake(out m_buffer, 0))
                        {
                            m_offset = 0;
                            m_count = m_buffer.Length;
                        }
                        else
                            break;
                    }

                    var bytesToCopy = (count < m_count) ? count : m_count;
                    System.Buffer.BlockCopy(m_buffer, m_offset, buffer, offset, bytesToCopy);
                    m_offset += bytesToCopy;
                    m_count -= bytesToCopy;
                    offset += bytesToCopy;
                    count -= bytesToCopy;

                    returnBytes += bytesToCopy;
                }

                _Position += returnBytes;

                return returnBytes;
            }
        }

        public override int ReadByte()
        {
            byte[] returnValue = new byte[1];
            return (Read(returnValue, 0, 1) <= 0 ? -1 : (int)returnValue[0]);
        }

        public override void Flush()
        {
        }

        public override long Seek(long offset, SeekOrigin origin)
        {
            throw new NotImplementedException();
        }

        public override void SetLength(long value)
        {
            throw new NotImplementedException();
        }
    }
}
