using System;
using System.IO;
using System.IO.Ports;
using System.Threading.Tasks;

namespace ESCPOS_NET
{
    public class SerialPrinter : BasePrinter
    {
        private readonly string portName;
        private readonly int baudRate;
        private SerialPort _serialPort;

        public SerialPrinter(string portName, int baudRate)
            : base()
        {
            this.portName = portName;
            this.baudRate = baudRate;
        }

        public override void Connect(bool reconnecting = false)
        {
            _serialPort = new SerialPort(portName, baudRate);
            _serialPort.Open();

            base.Connect(reconnecting);
        }

        protected override int ReadBytesUnderlying(byte[] buffer, int offset, int bufferSize)
        {
            if (this._serialPort.BytesToRead == 0) return 0;
            return this._serialPort.Read(buffer, 0, Math.Min(bufferSize, this._serialPort.BytesToRead));
        }

        protected override void WriteBytesUnderlying(byte[] buffer, int offset, int count)
        {
            this._serialPort.Write(buffer, 0, count);
        }

        protected override void FlushUnderlying()
        {
            // noop
        }

        protected override void OverridableDispose()
        {
            if (_serialPort != null)
            {
                _serialPort?.Close();
                _serialPort?.Dispose();
                Task.Delay(250).Wait(); // Based on MSDN Documentation, should sleep after calling Close or some functionality will not be determinant.
            }
        }
    }
}