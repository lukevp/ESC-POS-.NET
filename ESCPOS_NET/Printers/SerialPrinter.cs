using ESCPOS_NET.Events;
using System;
using System.IO;
using System.IO.Ports;
using System.Threading.Tasks;

namespace ESCPOS_NET
{
    public class SerialPrinter : BasePrinter
    {
        #region Private Members

        private SerialPort _serialPort;
        private string _portName;
        private int _baudRate;

        #endregion

        #region Constructors

        public SerialPrinter(string portName, int baudRate) : base()
        {
            this._portName = portName;
            this._baudRate = baudRate;
            this.InitPrinter();
        }

        #endregion

        #region Private Members

        protected override void InitPrinter()
        {
            try
            {
                _serialPort = new SerialPort(this._portName, this._baudRate);
                _serialPort.Open();
                _writer = new BinaryWriter(_serialPort.BaseStream);
                _reader = new BinaryReader(_serialPort.BaseStream);
            }
            catch (Exception cE)
            {
                OnStatusChanged(this, new StatusUpdateEventArgs()
                {
                    Message = cE.Message,
                    UpdateType = StatusEventType.Error
                });
            }
        }

        #endregion

        /// <summary>
        /// Override to ensure that the port is open and ready.
        /// </summary>
        /// <param name="bytes"></param>
        public override void Write(byte[] bytes)
        {
            if (!this._serialPort.IsOpen)
            {
                this.InitPrinter();
            }

            base.Write(bytes);

            this._serialPort.Close();
        }

        #region Dispose 

        private bool _disposed = false;

        protected override void Dispose(bool disposing)
        {
            if (_disposed)
                return;

            if (disposing)
            {
                _serialPort?.Close();
                _serialPort?.Dispose();
                Task.Delay(250).Wait(); // Based on MSDN Documentation, should sleep after calling Close or some functionality will not be determinant.
            }

            base.Dispose(disposing);
        }

        #endregion
    }
}