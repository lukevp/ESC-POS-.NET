using System;
using System.IO;
using System.Timers;

namespace ESCPOS_NET
{
    public abstract class BasePrinter
    {
        protected BinaryWriter _writer;
        protected BinaryReader _reader;
        protected Timer _timer;
        protected int _bytesWritten = 0;

        public BasePrinter()
        {
            _timer = new Timer(20);
            _timer.Elapsed += Flush;
            _timer.AutoReset = false;
        }
        public virtual void Write(byte[] bytes)
        {
            _timer.Stop();
            _writer.Write(bytes);
            _bytesWritten += bytes.Length;
            if (_bytesWritten >= 200)
            {
                // Immediately trigger a flush before proceeding so the output buffer will not be delayed.
                Flush(null, null);
            }
            else
            {
                _timer.Start();
            }
        }

        public virtual byte[] Read()
        {
            // TODO: make this read until the stream is empty (non-blocking?) or with a timeout perhaps?
            // TODO: make a method that can watch the stream and read / throw events.
            return _reader.ReadBytes(8);
        }

        protected virtual void Flush(object sender, ElapsedEventArgs e)
        {
            _bytesWritten = 0;
            _timer.Stop();
            _writer.Flush();
        }
    }
}