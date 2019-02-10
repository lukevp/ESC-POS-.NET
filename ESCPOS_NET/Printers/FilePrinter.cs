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
            _file = File.OpenWrite(filePath);
            _writer = new BinaryWriter(_file);
        }

        ~FilePrinter()
        {
            _file.Close();
        }
    }
}
