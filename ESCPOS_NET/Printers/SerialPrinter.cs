using System.IO;
using System.IO.Ports;
using System.Threading.Tasks;

namespace ESCPOS_NET
{
    public class SerialPrinter : BasePrinter
    {
        private readonly SerialPort _serialPort;

        public SerialPrinter(string portName, int baudRate)
            : base()
        {
            _serialPort = new SerialPort(portName, baudRate);
            _serialPort.Open();
            Writer = new BinaryWriter(_serialPort.BaseStream);
            Reader = new BinaryReader(_serialPort.BaseStream);
        }

        protected override void OverridableDispose()
        {
            _serialPort?.Close();
            _serialPort?.Dispose();
            Task.Delay(250).Wait(); // Based on MSDN Documentation, should sleep after calling Close or some functionality will not be determinant.
        }
    }
}