﻿using System.IO;

namespace ESC_NET.Printers
{
    public class MemoryPrinter : BasePrinter
    {
        private readonly MemoryStream _ms;

        // TODO: default values to their default values in ctor.
        public MemoryPrinter()
        {
            _ms = new MemoryStream();
            Writer = new BinaryWriter(_ms);
        }

        ~MemoryPrinter()
        {
            Dispose(false);
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