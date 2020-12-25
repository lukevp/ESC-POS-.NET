using System;
using System.Collections.Generic;
using System.Text;

namespace ESCPOS_NET.FiscalEmitters.BaseCommandValues
{
    public static class Fiscal
    {
        public static readonly byte OpenReceipt = 0x30;
        public static readonly byte ItemRegistration = 0x31;
        public static readonly byte TotalRegistration = 0x35;
        public static readonly byte PrintText = 0x36;
        public static readonly byte CloseReceipt = 0x38;
    }
}
