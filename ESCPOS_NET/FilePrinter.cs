using ESCPOS_NET.Emitters;
using RJCP.IO.Ports;
using System;
using System.IO;
using System.Timers;

namespace ESCPOS_NET
{
    public class FilePrinter : IPrinter
    {
        private BinaryWriter _file;
        private string path;
        private Timer _timer;
        private int _bytesWritten = 0;

        // TODO: default values to their default values in ctor.
        public FilePrinter(string filePath)
        {
            path = filePath;
            _file = new BinaryWriter(File.OpenWrite(filePath));
            _timer = new Timer(20);
            _timer.Elapsed += Flush;
            _timer.AutoReset = false;
        }

        ~FilePrinter()
        {
            _file.Close();
        }

        public void Write(byte[] bytes)
        {
            _timer.Stop();
            _file.Write(bytes);
            _bytesWritten += bytes.Length;
            if (_bytesWritten >= 200)
            {
                // Immediately trigger a flush before proceeding so the output buffer will not be delayed.
                Flush(null, null);
            }
            else
            { 
                _timer.Start();
            }
        }

        private void Flush(object sender, ElapsedEventArgs e)
        {
            _bytesWritten = 0;
            _timer.Stop();
            _file.Flush();
        }
    }
}
