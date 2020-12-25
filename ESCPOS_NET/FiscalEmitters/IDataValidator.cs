using System;
using System.Collections.Generic;
using System.Text;

namespace ESCPOS_NET.FiscalEmitters
{
    public interface IDataValidator
    {
        void ValidateItemRegistration(ref string itemDescription, char taxCode, decimal price, decimal quantity = 1, decimal percDiscount = 0, decimal surcharge = 0);
        void ValidateReceiptOpening(int opCode = 1, string opPwd = "000000", int posNumber = 0);
        void ValidateTotalRegistration(ref string totalDescription, char? paidMode, decimal? amount);
        void ValidateTextPrinting(ref string text);
    }
}
