using System;
using System.IO;
using System.IO.Ports;
using System.Threading.Tasks;

namespace ESCPOS_NET
{
    public class SerialPrinter : BasePrinter,  IDisposable
    {
        private SerialPort _serialPort { get; set; }
        public SerialPrinter(string portName, int baudRate) : base()
        {
            _serialPort = new SerialPort(portName, baudRate);
            _serialPort.Open();
            _writer = new BinaryWriter(_serialPort.BaseStream);
            _reader = new BinaryReader(_serialPort.BaseStream);
        }

        ~SerialPrinter()
        {
            Dispose();
        }

        protected override void OverridableDispose()
        {
            _serialPort?.Close();
            _serialPort?.Dispose();
            Task.Delay(250).Wait(); // Based on MSDN Documentation, should sleep after calling Close or some functionality will not be determinant.
        }
    }
}