using ESCPOS_NET.Emitters;
using RJCP.IO.Ports;

namespace ESCPOS_NET
{
    public class SerialPrinter
    {
        private SerialPortStream _serialPort;
        // TODO: default values to their default values in ctor.
        public SerialPrinter(string portName, int baudRate)
        {
            _serialPort = new SerialPortStream(portName, baudRate);
            _serialPort.Open();
        }

         ~SerialPrinter()
        {
            _serialPort.Close();
        }


        public void Write(params object[] bytearrays)
        {
            foreach (var obj in bytearrays)
            {
                byte[] bytes = (byte[])obj;
                _serialPort.Write(bytes, 0, bytes.Length);
            }
        }
    }
}
