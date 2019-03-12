using System;
using System.Collections.Generic;
using System.Text;

namespace ESCPOS_NET.Emitters
{
    public interface ICommandEmitter
    {
        /* Print Commands */
        byte[] PrintLine(string line = null);
        byte[] Print(string line);
        byte[] FeedLines(int lineCount);
        byte[] FeedLinesReverse(int lineCount);
        byte[] FeedDots(int dotCount);

        /* Line Spacing Commands */
        byte[] ResetLineSpacing();
        byte[] SetLineSpacingInDots(int dots);

        /* Operational Commands */
        byte[] Initialize();
        byte[] Enable();
        byte[] Disable();

        /* Character Commands */
        byte[] SetStyles(PrintStyle style);
        byte[] LeftAlign();
        byte[] RightAlign();
        byte[] CenterAlign();

        /* Action Commands */
        byte[] FullCut();
        byte[] PartialCut();
        byte[] FullCutAfterFeed(int lineCount);
        byte[] PartialCutAfterFeed(int lineCount);


        /* Image Commands */

        /* Status Commands */
        byte[] EnableAutomaticStatusBack();

        /* Barcode Commands */
        byte[] PrintBarcode(BarcodeType type, string barcode);
        byte[] SetBarcodeHeightInDots(int height);
        byte[] SetBarWidth(BarWidth width);
        byte[] SetBarLabelPosition(BarLabelPrintPosition position);
        byte[] SetBarLabelFontB(bool fontB);
        /* 2D-Code Commands */
    }
}
