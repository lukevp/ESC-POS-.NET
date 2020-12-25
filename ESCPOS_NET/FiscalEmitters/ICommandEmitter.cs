using System;
using System.Collections.Generic;
using System.Text;

namespace ESCPOS_NET.FiscalEmitters
{
    public interface ICommandEmitter
    {
        /* Action Commands */
        byte[] PaperCut();

        byte[] PaperFeed(int lines = 1);

        /*Fiscal Commands*/
        byte[] OpenReceipt(int opCode = 1, string opPwd = "000000", int posNumber = 0);

        byte[] RegisterItem(string itemDescription, char taxCode, decimal price, decimal quantity = 1, decimal percDiscount = 0, decimal surcharge = 0);

        byte[] RegisterTotal(string totalizationDescription, char? paidMode, decimal? amount);

        byte[] CloseReceipt();

        byte[] PrintText(string text);

        /*Non Fiscal Receipt Commands*/
        byte[] PrintNonFiscalReceipt(string text);
    }
}
