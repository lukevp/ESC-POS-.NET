using System.IO;

namespace ESCPOS_NET
{
    public class MemoryPrinter : BasePrinter
    {
        #region Private Members

        private MemoryStream _ms;

        #endregion

        #region Constructors

        // TODO: default values to their default values in ctor.
        public MemoryPrinter() : base()
        {
            this.InitPrinter();
        }

        #endregion

        #region Private Methods

        protected override void InitPrinter()
        {
            _ms = new MemoryStream();
            _writer = new BinaryWriter(_ms);
        }

        #endregion

        public override void Write(byte[] bytes)
        {
            if (!this._ms.CanWrite)
            {
                this.InitPrinter();
            }

            base.Write(bytes);
        }

        public byte[] GetAllData()
        {
            return _ms.ToArray();
        }

        #region Dispose

        private bool _disposed = false;
        
        protected override void Dispose(bool disposing)
        {
            if (_disposed)
                return;

            if (disposing)
            {
                _ms?.Close();
                _ms?.Dispose();
            }

            base.Dispose(disposing);
        }

        #endregion
    }
}
