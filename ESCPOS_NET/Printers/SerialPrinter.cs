using RJCP.IO.Ports;
using System.IO;

namespace ESCPOS_NET
{
    public class SerialPrinter : BasePrinter
    {
        private SerialPortStream _serialPort;

        public SerialPrinter(string portName, int baudRate) : base()
        {
            _serialPort = new SerialPortStream(portName, baudRate);
            _serialPort.Open();
            _writer = new BinaryWriter(_serialPort);
        }

        ~SerialPrinter()
        {
            _serialPort.Close();
        }
    }
}