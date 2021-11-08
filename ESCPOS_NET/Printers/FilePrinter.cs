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

        public override void Connect(bool reconnecting = false)
        {
            if (createIfNotExists)
            {
                _file = File.Open(filePath, FileMode.OpenOrCreate);
            }
            else
            {
                _file = File.Open(filePath, FileMode.Open);
            }

            base.Connect(reconnecting);
        }

        protected override int ReadBytesUnderlying(byte[] buffer, int offset, int bufferSize)
        {
            return _file.Read(buffer, offset, bufferSize);
        }
        
        protected override void WriteBytesUnderlying(byte[] buffer, int offset, int count)
        {
            _file.Write(buffer, offset, count);
        }

        protected override void FlushUnderlying()
        {
            _file.Flush();
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
