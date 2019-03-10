using System.IO;
using System.Timers;

namespace ESCPOS_NET
{
    public abstract class BasePrinter
    {
        protected BinaryWriter _writer;
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
        protected virtual void Flush(object sender, ElapsedEventArgs e)
        {
            _bytesWritten = 0;
            _timer.Stop();
            _writer.Flush();
        }
    }
}