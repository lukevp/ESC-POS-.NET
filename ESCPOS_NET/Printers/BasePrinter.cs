using System;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;

namespace ESCPOS_NET
{
    public abstract partial class BasePrinter : IDisposable
    {
    	private bool disposed = false;
        private volatile bool _isMonitoring;
        private CancellationTokenSource _cancellationTokenSource;
        private readonly int _maxBytesPerWrite = 15000; // max byte chunks to write at once.
        
        public PrinterStatusEventArgs Status { get; private set; } = null;
        public event EventHandler StatusChanged;
        protected BinaryWriter Writer { get; set; }
        protected BinaryReader Reader { get; set; }
        protected System.Timers.Timer FlushTimer { get; set; }
        protected ConcurrentQueue<byte> ReadBuffer { get; set; } = new ConcurrentQueue<byte>();
        protected int BytesWrittenSinceLastFlush { get; set; } = 0;
        protected virtual bool IsConnected => false;

        protected BasePrinter()
        {
            FlushTimer = new System.Timers.Timer(50);
            FlushTimer.Elapsed += Flush;
            FlushTimer.AutoReset = false;
        }

        protected virtual void Reconnect()
        {
             // Implemented in the network printer
        }

        public virtual void Read()
        {
            while (_isMonitoring)
            {
                try
                {
                    if (_cancellationTokenSource != null && _cancellationTokenSource.IsCancellationRequested)
                    {
                        _cancellationTokenSource.Token.ThrowIfCancellationRequested();
                    }

                    // Sometimes the serial port lib will throw an exception and read past the end of the queue if a
                    // status changes while data is being written.  We just ignore these bytes.
                    var b = Reader.ReadByte();
                    ReadBuffer.Enqueue(b);
                    DataAvailable(false);
                }
                catch (OperationCanceledException ex)
                {
                    _cancellationTokenSource.Dispose();
                    _cancellationTokenSource = null;
                    _isMonitoring = false;
                    Debug.WriteLine($"Read Cancelled Exception: {ex.Message}");
                }
                catch (IOException ex)
                {
                    // Thrown if the printer times out the socket connection 
                    // default is 90 seconds
                    Thread.Sleep(100);
                    _isMonitoring = false;
                    DataAvailable(true);
                    Debug.WriteLine($"Read Exception: {ex.Message}");
                }
                catch (Exception ex)
                {
                    // Swallow the exception
                    Debug.WriteLine($"Read Exception: {ex.Message}");
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
                Writer.Write(bytes, bytePointer, count);
                BytesWrittenSinceLastFlush += count;
                if (BytesWrittenSinceLastFlush >= 200)
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
                FlushTimer.Start();
            }
        }

        protected virtual void Flush(object sender, ElapsedEventArgs e)
        {
            BytesWrittenSinceLastFlush = 0;
            FlushTimer.Stop();
            Writer.Flush();
        }

        public virtual void StartMonitoring()
        {
            if (!_isMonitoring)
            {
                Console.WriteLine(nameof(StartMonitoring));
                ReadBuffer = new ConcurrentQueue<byte>();

                _isMonitoring = true;
                _cancellationTokenSource = new CancellationTokenSource();
                Task.Factory.StartNew(Read, _cancellationTokenSource.Token, TaskCreationOptions.LongRunning, TaskScheduler.Default).ConfigureAwait(false);
            }
        }

        public virtual void StopMonitoring()
        {
            if (_isMonitoring)
            {
                Console.WriteLine(nameof(StopMonitoring));
				_isMonitoring = false;
				ReadBuffer = new ConcurrentQueue<byte>();

                if (_cancellationTokenSource != null)
                {
                    _cancellationTokenSource.Cancel();
                }                    
            }
        }

        public virtual void DataAvailable(bool timeout)
        {
            if (timeout)
            {
                TryUpdatePrinterStatus(null, timeout);
                return;
            }

            if (ReadBuffer.Count() % 4 == 0)
            {
                var bytes = new byte[4];
                for (int i = 0; i < 4; i++)
                {
                    if (!ReadBuffer.TryDequeue(out bytes[i]))
                    {
                        // Ran out of bytes unexpectedly.
                        return; 
                    }
                }

                TryUpdatePrinterStatus(bytes, timeout);

                // TODO: call other update handlers.
            }
        }

        private void TryUpdatePrinterStatus(byte[] bytes, bool timeout)
        {
            if (timeout)
            {
                // try to reconnect
                if (!IsConnected)
                {
                    Reconnect();
                }

                // Test if re-connection worked
                if (!IsConnected)
                {
                    Status = new PrinterStatusEventArgs()
                    {
                        DeviceConnectionTimeout = true,
                    };

                    StatusChanged?.Invoke(this, Status);
                    return;
                }
            }
#if DEBUG
            var bytesToString = string.Empty;
            var index = 0;
            foreach (var b in bytes)
            {
                bytesToString += $"index[{index}], value[{b}]\n";
                index++;
            }

            Debug.WriteLine($"TryUpdatePrinterStatus: \n{bytesToString}");
#endif

            // Check header bits 0, 1 and 7 are 0, and 4 is 1
            if (bytes[0].IsBitNotSet(0) && bytes[0].IsBitNotSet(1) && bytes[0].IsBitSet(4) && bytes[0].IsBitNotSet(7))
            {
                Status = new PrinterStatusEventArgs()
                {
                    // byte[0] == 20 cash drawer closed 
                    // byte[0] == 16 cash drawer open
                    // Note some cash drawers do not close properly.
                    IsCashDrawerOpen = bytes[0].IsBitNotSet(2),
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
                    IsPaperOut = bytes[2].IsBitSet(2) && bytes[2].IsBitSet(3),
                    DeviceConnectionTimeout = timeout
                };
            }

            StatusChanged?.Invoke(this, Status);
        }

        ~BasePrinter()
        {
            Dispose(false);
        }

        public void Dispose()
        {            
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void OverridableDispose() // This method should only be called by the Dispose method.  // It allows synchronous disposing of derived class dependencies with base class disposes.
        {
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposed)
            {
                return;
            }

            if (disposing)
            {
                _cancellationTokenSource?.Cancel();
                FlushTimer?.Stop();
                if (FlushTimer != null)
                {
                    FlushTimer.Elapsed -= Flush;
                }
               
                FlushTimer?.Dispose();
                Reader?.Close();
                Reader?.Dispose();
                Writer?.Close();
                Writer?.Dispose();
                
                OverridableDispose();
            }
            disposed = true;
        }
    }
}