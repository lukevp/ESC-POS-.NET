using System.IO;

namespace ESCPOS_NET
{
    public class FilePrinter : BasePrinter
    {
        private FileStream _file;
        private bool createIfNotExists;
        private string filePath;

        // TODO: default values to their default values in ctor.
        public FilePrinter(string filePath, bool createIfNotExists = false)
            : base()
        {
            this.createIfNotExists = createIfNotExists;
            this.filePath = filePath;
        }

        public override void Connect()
        {
            if (createIfNotExists)
            {
                _file = File.Open(filePath, FileMode.OpenOrCreate);
            }
            else
            {
                _file = File.Open(filePath, FileMode.Open);
            }
            Writer = new BinaryWriter(_file);
            Reader = new BinaryReader(_file);

            base.Connect();
        }

        ~FilePrinter()
        {
            Dispose(false);
        }

        protected override void OverridableDispose()
        {
            _file?.Close();
            _file?.Dispose();
        }
    }
}
