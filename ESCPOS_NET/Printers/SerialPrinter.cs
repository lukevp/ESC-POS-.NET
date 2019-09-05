using System;
using System.IO;
using System.IO.Ports;
using System.Timers;

namespace ESCPOS_NET
{
    public class SerialPrinter : BasePrinter,  IDisposable
    {
        private SerialPort _serialPort;

        public SerialPrinter(string portName, int baudRate) : base()
        {
            _serialPort = new SerialPort(portName, baudRate);
            _serialPort.Open();
        }

        public override void Read() {
            while (true) {
                try {
                    // Sometimes the serial port lib will throw an exception and read past the end of the queue if a
                    // status changes while data is being written.  We just ignore these bytes.
                    var b = _serialPort.ReadByte();
                    if (0 <= b && b <= 255) {
                        _readBuffer.Enqueue((byte)b);
                        DataAvailable();
                    }
                }
                catch { }
            }
        }

        public override void Write(byte[] bytes) {
            _writeTimer.Stop();
            _serialPort.Write(bytes, 0, bytes.Length);
            _bytesWritten += bytes.Length;
            if (_bytesWritten >= 200) {
                // Immediately trigger a flush before proceeding so the output buffer will not be delayed.
                Flush(null, null);
            }
            else {
                _writeTimer.Start();
            }
        }

        protected override void Flush(object sender, ElapsedEventArgs e) {
            _bytesWritten = 0;
            _writeTimer.Stop();
            //_serialPort.Flush(); // Not available in SerialPort implementation
        }

        ~SerialPrinter()
        {
            Dispose();
        }

        public void Dispose()
        {
            _serialPort.Close();
        }
    }
}