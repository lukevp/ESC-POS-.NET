﻿using ESCPOS_NET.Events;
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
        // Event Handler
        public delegate void StatusUpdate(object sender, StatusUpdateEventArgs e);
        public virtual event StatusUpdate StatusChanged;

        // Binary Streams
        protected BinaryWriter _writer;
        protected BinaryReader _reader;

        // Timers and Threads
        protected System.Timers.Timer _flushTimer;
        protected Thread _readThread;
        protected ConcurrentQueue<byte> _readBuffer = new ConcurrentQueue<byte>();
        protected int _bytesWrittenSinceLastFlush = 0;
        private readonly int _maxBytesPerWrite = 15000; // max byte chunks to write at once.

        #region Constructor

        public BasePrinter()
        {
            _flushTimer = new System.Timers.Timer(50);
            _flushTimer.Elapsed += Flush;
            _flushTimer.AutoReset = false;
        }

        #endregion

        /// <summary>
        /// Read
        /// </summary>
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
                catch(Exception ex)
                {
                    if (this.StatusChanged != null)
                    {
                        StatusChanged(this, new StatusUpdateEventArgs()
                        {
                            Message = ex.Message,
                            UpdateType = StatusEventType.Error
                        });
                    }

                    Thread.Sleep(100);
                }
            }
        }

        /// <summary>
        /// Write the Bytes depending on the printer type.
        /// </summary>
        /// <param name="bytes"></param>
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

        #region Monitoring

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
                StatusUpdateEventArgs e = new StatusUpdateEventArgs()
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
                    IsPaperOut = bytes[2].IsBitSet(2) && bytes[2].IsBitSet(3),
                    UpdateType = StatusEventType.Info,
                    Message = string.Empty
                };

                StatusChanged?.Invoke(this, e);
            }
        }

        #endregion

        #region Dispose

        // Updated to correctly implement Dispose pattern.
        // Flag: Has Dispose already been called?
        private bool _disposed = false;

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (_disposed)
                return;
            if (disposing)
            {
                _readThread?.Abort();
                _writer?.Close();
                _writer?.Dispose();
                _reader?.Close();
                _reader?.Dispose();
            }
            _disposed = true;
        }

        ~BasePrinter()
        {
            Dispose(false);
        }

        #endregion

        #region Events

        // Added to implement status change event model for more telemetry.
        protected virtual void OnStatusChanged(object sender, StatusUpdateEventArgs e)
        {
            StatusChanged?.Invoke(sender, e);
        }

        #endregion
    }
}