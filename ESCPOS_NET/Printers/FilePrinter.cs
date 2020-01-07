using System;
using System.IO;

namespace ESCPOS_NET
{
    public class FilePrinter : BasePrinter
    {
        private readonly string _path;
        private FileStream _file;

        // TODO: default values to their default values in ctor.
        public FilePrinter(string filePath) : base()
        {
            _path = filePath;
            _file = File.Open(filePath, FileMode.Open);
            _writer = new BinaryWriter(_file);
            _reader = new BinaryReader(_file);
        }

        ~FilePrinter()
        {
            Dispose();
        }

        protected override void OverridableDispose()
        {
            _file?.Close();
            _file?.Dispose();
        }
    }
}
