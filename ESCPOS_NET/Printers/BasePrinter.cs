using System;
using System.Collections;
using System.Collections.Concurrent;
using System.IO;
using System.Linq;
using System.Threading;
using System.Timers;

namespace ESCPOS_NET
{
    public abstract partial class BasePrinter
    {
        public PrinterStatus Status = null;
        public event EventHandler StatusChanged;
        protected BinaryWriter _writer;
        protected BinaryReader _reader;
        protected System.Timers.Timer _writeTimer;
        protected Thread _readThread;
        protected ConcurrentQueue<byte> _readBuffer = new ConcurrentQueue<byte>();
        protected int _bytesWritten = 0;

        public BasePrinter()
        {
            _writeTimer = new System.Timers.Timer(20);
            _writeTimer.Elapsed += Flush;
            _writeTimer.AutoReset = false;
        }

        public virtual void Read()
        {
            while (true)
            {
                try
                {
                    // Sometimes the serial port lib will throw an exception and read past the end of the queue if a
                    // status changes while data is being written.  We just ignore these bytes.
                    var b = _reader.ReadByte();
                    _readBuffer.Enqueue(b);
                    DataAvailable();
                }
                catch { }
            }
        }

        public virtual void Write(byte[] bytes)
        {
            _writeTimer.Stop();
            _writer.Write(bytes);
            _bytesWritten += bytes.Length;
            if (_bytesWritten >= 200)
            {
                // Immediately trigger a flush before proceeding so the output buffer will not be delayed.
                Flush(null, null);
            }
            else
            {
                _writeTimer.Start();
            }
        }

        protected virtual void Flush(object sender, ElapsedEventArgs e)
        {
            _bytesWritten = 0;
            _writeTimer.Stop();
            _writer.Flush();
        }

        public virtual void StartMonitoring()
        {
            _readBuffer = new ConcurrentQueue<byte>();
            _readThread = new Thread(new ThreadStart(Read));
            _readThread.Start();
        }

        public virtual void StopMonitoring()
        {
            _readThread.Abort();
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

        private void TryUpdateInkStatus()
        {
            throw new NotImplementedException();
        }
    }
}