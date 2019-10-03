using System;
using System.IO;
using System.IO.Ports;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ESCPOS_NET
{

    // This adapter class is used to expose a Stream type that behaves properly with constant reads on the thread.
    // By default, a read on the serial port past the end causes issues, so you are expected to use BytesToRead property
    // to manage this.  Using this adapter abstracts this away and the ReadStream will have bytes written to it as they are available.
    public class AsyncReadAdapter
    {
        private SerialPort _basePort { get; set; }
        public AsyncReadAdapter(SerialPort basePort)
        {
            _basePort = basePort;
            _basePort.DataReceived += _basePort_DataReceived;
            ReadStream = new MemoryStream();
        }

        private void _basePort_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            SerialPort sp = (SerialPort)sender;

            // Cache the bytes to read property so that it doesn't get modified while in the loop and offset the position reset.
            int bytes = sp.BytesToRead;
            for (int i = 0; i < bytes; i++)
            {
                ReadStream.WriteByte((byte)sp.ReadByte());
            }
            ReadStream.Position = ReadStream.Position - bytes;
        }

        public MemoryStream ReadStream { get; private set; }
    }
    public class SerialPrinter : BasePrinter,  IDisposable
    {
        private SerialPort _serialPort { get; set; }
        private AsyncReadAdapter _asyncReadAdapter { get; set; }
        public SerialPrinter(string portName, int baudRate) : base()
        {
            _serialPort = new SerialPort(portName, baudRate);
            _serialPort.Open();
            //_serialPort.ReadExisting();
            _asyncReadAdapter = new AsyncReadAdapter(_serialPort);
            _writer = new BinaryWriter(_serialPort.BaseStream);
            _reader = new BinaryReader(_asyncReadAdapter.ReadStream);
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