using RJCP.IO.Ports;
using System;
using System.IO;

namespace ESCPOS_NET
{
    public class SerialPrinter : BasePrinter,  IDisposable
    {
        private SerialPortStream _serialPort;

        public SerialPrinter(string portName, int baudRate) : base()
        {
            _serialPort = new SerialPortStream(portName, baudRate);
            _serialPort.Open();
            _writer = new BinaryWriter(_serialPort);
            _reader = new BinaryReader(_serialPort);
        }

        ~SerialPrinter()
        {
            Dispose();
        }

        public void Dispose()
        {
            _writer.Close();
            _reader.Close();
            _serialPort.Close();
        }
    }
}