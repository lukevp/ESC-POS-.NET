using System.IO;

namespace ESCPOS_NET
{
    public class MemoryPrinter : BasePrinter
    {
        private MemoryStream _ms;

        // TODO: default values to their default values in ctor.
        public MemoryPrinter() : base()
        {
            _ms = new MemoryStream();
            _writer = new BinaryWriter(_ms);
        }

        ~MemoryPrinter()
        {
            Dispose();
        }

        public byte[] GetAllData()
        {
            return _ms.ToArray();
        }

        protected override void OverridableDispose()
        {
            _ms?.Close();
            _ms?.Dispose();
        }
    }
}
