using System;
using System.Collections;
using System.Collections.Concurrent;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;

namespace ESCPOS_NET
{
    public abstract partial class BasePrinter : IDisposable
    {
        public PrinterStatus Status = null;
        public event EventHandler StatusChanged;
        protected BinaryWriter _writer;
        protected BinaryReader _reader;
        protected System.Timers.Timer _flushTimer;
        protected Thread _readThread;
        protected ConcurrentQueue<byte> _readBuffer = new ConcurrentQueue<byte>();
        protected int _bytesWrittenSinceLastFlush = 0;
        private readonly int _maxBytesPerWrite = 15000; // max byte chunks to write at once.

        public BasePrinter()
        {
            _flushTimer = new System.Timers.Timer(50);
            _flushTimer.Elapsed += Flush;
            _flushTimer.AutoReset = false;
        }
        public virtual void Read()
        {
            while (true)
            {
                try
                {
                    if (_monitoring)
                    {
                        // Sometimes the serial port lib will throw an exception and read past the end of the queue if a
                        // status changes while data is being written.  We just ignore these bytes.
                        var b = _reader.ReadByte();
                        _readBuffer.Enqueue(b);
                        DataAvailable();
                    }
                }
                catch
                {
                    Thread.Sleep(100);
                }
            }
        }

        public virtual void Write(byte[] bytes)
        {
            int bytePointer = 0;
            int bytesLeft = bytes.Length;
            bool hasFlushed = false;
            while (bytesLeft > 0)
            {
                int count = Math.Min(_maxBytesPerWrite, bytesLeft);
                _writer.Write(bytes, bytePointer, count);
                _bytesWrittenSinceLastFlush += count;
                if (_bytesWrittenSinceLastFlush >= 200)
                {
                    // Immediately trigger a flush before proceeding so the output buffer will not be delayed.
                    hasFlushed = true;
                    Flush(null, null);
                }
                bytePointer += count;
                bytesLeft -= count;
            }
            if (!hasFlushed)
            {
                _flushTimer.Start();
            }
        }

        protected virtual void Flush(object sender, ElapsedEventArgs e)
        {
            _bytesWrittenSinceLastFlush = 0;
            _flushTimer.Stop();
            _writer.Flush();
        }

        public virtual void StartMonitoring()
        {
            if (_readThread == null)
            {
                _readThread = new Thread(new ThreadStart(Read));
                _readThread.Start();
            }
            _readBuffer = new ConcurrentQueue<byte>();
            _monitoring = true;
        }

        public virtual void StopMonitoring()
        {
            _monitoring = false;
            _readBuffer = new ConcurrentQueue<byte>();
        }

        public virtual void DataAvailable()
        {
            if (_readBuffer.Count() % 4 == 0)
            {
                var bytes = new byte[4];
                for (int i = 0; i < 4; i++)
                {
                    if (!_readBuffer.TryDequeue(out bytes[i])) return; // Ran out of bytes unexpectedly.
                }

                TryUpdatePrinterStatus(bytes);
                // TODO: call other update handlers.
            }
        }

        private bool _monitoring = false;

        private void TryUpdatePrinterStatus(byte[] bytes)
        {
            // Check header bits 0, 1 and 7 are 0, and 4 is 1
            if (bytes[0].IsBitNotSet(0) && bytes[0].IsBitNotSet(1) && bytes[0].IsBitSet(4) && bytes[0].IsBitNotSet(7))
            {
                Status = new PrinterStatus()
                {
                    IsCashDrawerOpen = bytes[0].IsBitNotSet(2), // TODO: verify that this state is correct and is not inverted.
                    IsPrinterOnline = bytes[0].IsBitNotSet(3),
                    IsCoverOpen = bytes[0].IsBitSet(5),
                    IsPaperCurrentlyFeeding = bytes[0].IsBitSet(6),
                    IsWaitingForOnlineRecovery = bytes[1].IsBitSet(0),
                    IsPaperFeedButtonPushed = bytes[1].IsBitSet(1),
                    DidRecoverableNonAutocutterErrorOccur = bytes[1].IsBitSet(2),
                    DidAutocutterErrorOccur = bytes[1].IsBitSet(3),
                    DidUnrecoverableErrorOccur = bytes[1].IsBitSet(5),
                    DidRecoverableErrorOccur = bytes[1].IsBitSet(6),
                    IsPaperLow = bytes[2].IsBitSet(0) && bytes[2].IsBitSet(1),
                    IsPaperOut = bytes[2].IsBitSet(2) && bytes[2].IsBitSet(3)
                };
            }
            StatusChanged?.Invoke(this, Status);
        }

        // ~~~START~~~ IDisposable
        // Flag: Has Dispose already been called?
        private bool disposed = false;

        protected virtual void OverridableDispose() // This method should only be called by the Dispose method.  // It allows synchronous disposing of derived class dependencies with base class disposes.
        {
        }
        public virtual void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        protected virtual void Dispose(bool disposing)
        {
            if (disposed)
                return;
            if (disposing)
            {
                _readThread?.Abort();
                _writer?.Close();
                _writer?.Dispose();
                _reader?.Close();
                _reader?.Dispose();
                OverridableDispose();
            }
            disposed = true;
        }
        // ~~~END~~~ IDisposable
        private void TryUpdateInkStatus()
        {
            throw new NotImplementedException();
        }
    }
}