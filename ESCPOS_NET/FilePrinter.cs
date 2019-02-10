using ESCPOS_NET.Emitters;
using RJCP.IO.Ports;
using System;
using System.IO;

namespace ESCPOS_NET
{
    public class FilePrinter : IPrinter
    {
        private BinaryWriter _file;
        private string path;

        // TODO: default values to their default values in ctor.
        public FilePrinter(string filePath)
        {
            path = filePath;
            _file = new BinaryWriter(File.OpenWrite(filePath));
            Console.WriteLine("Filestream Opened.");
        }

        ~FilePrinter()
        {
            _file.Close();
        }

        public void Write(byte[] bytes)
        {
            _file.Write(bytes);
            _file.Flush();
        }
    }
}
