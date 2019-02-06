using System;
using System.Collections.Generic;
using System.Text;

namespace ESCPOS_NET.Emitters
{
    public interface ICommandEmitter
    {
        /* Operational Commands */
        byte[] Initialize { get; }
        byte[] Enable { get; }
        byte[] Disable { get; }

        /* Character Commands */
        byte[] SetStyles(PrintStyle style);
        byte[] LeftAlign { get; }
        byte[] RightAlign { get; }
        byte[] CenterAlign { get; }

        /* Action Commands */
        byte[] FullCut { get; }
        byte[] PartialCut { get; }
        byte[] FullCutAfterFeed(int lineCount);
        byte[] PartialCutAfterFeed(int lineCount);

        /* Printing Commands */
        byte[] PrintLines(string lines);
        byte[] PrintLine(string line);
        byte[] Print(string line);
        byte[] FeedLines(int lineCount);
        byte[] FeedLinesReverse(int lineCount);
        byte[] FeedDots(int dotCount);

        /* Image Commands */
        /* Status Commands */
        /* Barcode Commands */
        byte[] PrintBarcode(BarcodeType type, string barcode);
        byte[] SetBarcodeHeightInDots(int height);
        byte[] SetBarWidth(BarWidth width);
        byte[] SetBarLabelPosition(BarLabelPrintPosition position);
        byte[] SetBarLabelFontB(bool fontB);
        /* 2D-Code Commands */
    }
}
