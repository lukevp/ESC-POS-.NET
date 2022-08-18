using ESCPOS_NET.Utilities;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Concurrent;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;

namespace ESCPOS_NET
{
    public abstract partial class BasePrinter : IPrinter, IDisposable
    {
        private bool disposed = false;

        //private volatile bool _isMonitoring;

        private CancellationTokenSource _readCancellationTokenSource;
        private CancellationTokenSource _writeCancellationTokenSource;

        private readonly int _maxBytesPerWrite = 15000; // max byte chunks to write at once.

        public PrinterStatusEventArgs Status { get; private set; } = new PrinterStatusEventArgs();

        public event EventHandler StatusChanged;
        public event EventHandler Disconnected;
        public event EventHandler Connected;

        protected BinaryWriter Writer { get; set; }

        protected BinaryReader Reader { get; set; }

        protected ConcurrentQueue<byte> ReadBuffer { get; set; } = new ConcurrentQueue<byte>();

        protected ConcurrentQueue<byte[]> WriteBuffer { get; set; } = new ConcurrentQueue<byte[]>();

        protected int BytesWrittenSinceLastFlush { get; set; } = 0;

        protected volatile bool IsConnected = true;

        public string PrinterName { get; protected set; }

        protected BasePrinter()
        {
            PrinterName = Guid.NewGuid().ToString();
            Init();
        }
        protected BasePrinter(string printerName)
        {
            if (string.IsNullOrEmpty(printerName))
            {
                printerName = Guid.NewGuid().ToString();
            }
            PrinterName = printerName;
            Init();
        }
        private void Init()
        {
            _readCancellationTokenSource = new CancellationTokenSource();
            _writeCancellationTokenSource = new CancellationTokenSource();
            Logging.Logger?.LogDebug("[{Function}]:[{PrinterName}] Initializing Task Threads...", $"{this}.{MethodBase.GetCurrentMethod().Name}", PrinterName);
            //Task.Factory.StartNew(MonitorPrinterStatusLongRunningTask, _connectivityCancellationTokenSource.Token, TaskCreationOptions.LongRunning, TaskScheduler.Default).ConfigureAwait(false);
            Task.Factory.StartNew(WriteLongRunningTask, _writeCancellationTokenSource.Token, TaskCreationOptions.LongRunning, TaskScheduler.Default).ConfigureAwait(false);
            Task.Factory.StartNew(ReadLongRunningTask, _readCancellationTokenSource.Token, TaskCreationOptions.LongRunning, TaskScheduler.Default).ConfigureAwait(false);
            // TODO: read and status monitoring probably won't work for fileprinter, should let printer types disable this feature.
            Logging.Logger?.LogDebug("[{Function}]:[{PrinterName}] Task Threads started", $"{this}.{MethodBase.GetCurrentMethod().Name}", PrinterName);
        }


        protected void InvokeConnect()
        {
            Task.Run(() => Connected?.Invoke(this, new ConnectionEventArgs() { IsConnected = true }));
        }
        protected void InvokeDisconnect()
        {
            Task.Run(() => Disconnected?.Invoke(this, new ConnectionEventArgs() { IsConnected = false }));
        }

        protected virtual void Reconnect()
        {
            // Implemented in the network printer
        }
        protected virtual async void WriteLongRunningTask()
        {
            while (true)
            {
                if (_writeCancellationTokenSource != null && _writeCancellationTokenSource.IsCancellationRequested)
                {
                    Logging.Logger?.LogDebug("[{Function}]:[{PrinterName}] Write Long-Running Task Cancellation was requested.", $"{this}.{MethodBase.GetCurrentMethod().Name}", PrinterName);
                    break;
                }

                await Task.Delay(100);
                if (!IsConnected)
                {
                    continue;
                }

                try
                {
                    var didDequeue = WriteBuffer.TryDequeue(out var nextBytes);
                    if (didDequeue && nextBytes?.Length > 0)
                    {
                        WriteToBinaryWriter(nextBytes);
                    }
                }
                catch (IOException)
                {
                    // Thrown if the printer times out the socket connection
                    // default is 90 seconds
                    //Logging.Logger?.LogDebug("[{Function}]:[{PrinterName}] Swallowing IOException... sometimes happens with network printers. Should get reconnected automatically.");
                }
                catch
                {
                    // Swallow the exception
                    //Logging.Logger?.LogDebug("[{Function}]:[{PrinterName}] Swallowing generic read exception... sometimes happens with serial port printers.");
                }
            }
        }

        protected virtual async void ReadLongRunningTask()
        {
            while (true)
            {
                if (_readCancellationTokenSource != null && _readCancellationTokenSource.IsCancellationRequested)
                {
                    Logging.Logger?.LogDebug("[{Function}]:[{PrinterName}] Read Long-Running Task Cancellation was requested.", $"{this}.{MethodBase.GetCurrentMethod().Name}", PrinterName);
                    break;
                }

                await Task.Delay(100);

                if (Reader == null) continue;
                if (!IsConnected) continue;

                try
                {
                    // Sometimes the serial port lib will throw an exception and read past the end of the queue if a
                    // status changes while data is being written.  We just ignore these bytes.
                    var b = Reader.BaseStream.ReadByte();
                    if (b >= 0 && b <= 255)
                    {
                        ReadBuffer.Enqueue((byte)b);
                        DataAvailable();
                    }
                }

                catch
                {
                    // Swallow the exception
                    //Logging.Logger?.LogDebug("[{Function}]:[{PrinterName}] Swallowing generic read exception... sometimes happens with serial port printers.", $"{this}.{MethodBase.GetCurrentMethod().Name}", PrinterName);
                }
            }
        }

        public virtual void Write(params byte[][] arrays)
        {
            Write(ByteSplicer.Combine(arrays));
        }

        public virtual void Write(byte[] bytes)
        {
            WriteBuffer.Enqueue(bytes);
        }

        protected virtual void WriteToBinaryWriter(byte[] bytes)
        {

            if (!IsConnected)
            {
                Logging.Logger?.LogInformation("[{Function}]:[{PrinterName}] Attempted to write but printer isn't connected. Attempting to reconnect...", $"{this}.{MethodBase.GetCurrentMethod().Name}", PrinterName);
                Reconnect();
            }

            if (!IsConnected)
            {
                Logging.Logger?.LogError("[{Function}]:[{PrinterName}] Unrecoverable connectivity error writing to printer.", $"{this}.{MethodBase.GetCurrentMethod().Name}", PrinterName);
                throw new IOException("Unrecoverable connectivity error writing to printer.");
            }

            int bytePointer = 0;
            int bytesLeft = bytes.Length;
            bool hasFlushed = false;
            while (bytesLeft > 0)
            {

                int count = Math.Min(_maxBytesPerWrite, bytesLeft);
                try
                {
                    Writer.Write(bytes, bytePointer, count);
                }
                catch (IOException e)
                {
                    Reconnect();
                    if (!IsConnected)
                    {
                        Logging.Logger?.LogError(e, "[{Function}]:[{PrinterName}] Unrecoverable connectivity error writing to printer.", $"{this}.{MethodBase.GetCurrentMethod().Name}", PrinterName);
                    }
                    Writer.Write(bytes, bytePointer, count);
                }
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
                Task.Run(async () => { await Task.Delay(50); Flush(null, null); });
            }
        }

        public virtual void Flush(object sender, ElapsedEventArgs e)
        {
            try
            {

                BytesWrittenSinceLastFlush = 0;
                Writer.Flush();
            }
            catch (Exception ex)
            {
                Logging.Logger?.LogError(ex, "[{Function}]:[{PrinterName}] Flush threw exception.", $"{this}.{MethodBase.GetCurrentMethod().Name}", PrinterName);
            }
        }

        public virtual void DataAvailable()
        {
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

                TryUpdatePrinterStatus(bytes);

                // TODO: call other update handlers.
            }
        }

        private void TryUpdatePrinterStatus(byte[] bytes)
        {
            var bytesToString = BitConverter.ToString(bytes);

            Logging.Logger?.LogDebug("[{Function}]:[{PrinterName}] TryUpdatePrinterStatus: Received flag values {bytesToString}", $"{this}.{MethodBase.GetCurrentMethod().Name}", PrinterName, bytesToString);

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
                };
            }

            if (StatusChanged != null)
            {
                Logging.Logger?.LogDebug("[{Function}]:[{PrinterName}] Invoking Status Changed Event Handler...", $"{this}.{MethodBase.GetCurrentMethod().Name}", PrinterName);
                StatusChanged?.Invoke(this, Status);
            }
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
                try
                {
                    _readCancellationTokenSource?.Cancel();
                }
                catch (Exception e)
                {
                    Logging.Logger?.LogDebug(e, "[{Function}]:[{PrinterName}] Dispose Issue during cancellation token cancellation call.", $"{this}.{MethodBase.GetCurrentMethod().Name}", PrinterName);
                }
                try
                {
                    Reader?.Close();
                }
                catch (Exception e)
                {
                    Logging.Logger?.LogDebug(e, "[{Function}]:[{PrinterName}] Dispose Issue closing reader.", $"{this}.{MethodBase.GetCurrentMethod().Name}", PrinterName);
                }
                try
                {
                    Reader?.Dispose();
                }
                catch (Exception e)
                {
                    Logging.Logger?.LogDebug(e, "[{Function}]:[{PrinterName}] Dispose Issue disposing reader.", $"{this}.{MethodBase.GetCurrentMethod().Name}", PrinterName);
                }
                try
                {
                    Writer?.Close();
                }
                catch (Exception e)
                {
                    Logging.Logger?.LogDebug(e, "[{Function}]:[{PrinterName}] Dispose Issue closing writer.", $"{this}.{MethodBase.GetCurrentMethod().Name}", PrinterName);
                }
                try
                {
                    Writer?.Dispose();
                }
                catch (Exception e)
                {
                    Logging.Logger?.LogDebug(e, "[{Function}]:[{PrinterName}] Dispose Issue disposing writer.", $"{this}.{MethodBase.GetCurrentMethod().Name}", PrinterName);
                }
                try
                {
                    OverridableDispose();
                }
                catch (Exception e)
                {
                    Logging.Logger?.LogDebug(e, "[{Function}]:[{PrinterName}] Dispose Issue during overridable dispose.", $"{this}.{MethodBase.GetCurrentMethod().Name}", PrinterName);
                }
            }

            disposed = true;
        }

        public PrinterStatusEventArgs GetStatus()
        {
            throw new NotImplementedException();
        }
    }
}