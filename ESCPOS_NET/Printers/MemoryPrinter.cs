using System.IO;

namespace ESCPOS_NET
{
    public class MemoryPrinter : BasePrinter
    {
        private readonly MemoryStream _ms;

        // TODO: default values to their default values in ctor.
        public MemoryPrinter()
            : base()
        {
            _ms = new MemoryStream();
        }

        ~MemoryPrinter()
        {
            Dispose(false);
        }

        public byte[] GetAllData()
        {
            return _ms.ToArray();
        }

        protected override int ReadBytesUnderlying(byte[] buffer, int offset, int bufferSize)
        {
            return 0;
        }

        protected override void WriteBytesUnderlying(byte[] buffer, int offset, int count)
        {
            _ms.Write(buffer, offset, count);
        }

        protected override void FlushUnderlying()
        {
            _ms.Flush();
        }

        protected override void OverridableDispose()
        {
            _ms?.Close();
            _ms?.Dispose();
        }
    }
}
