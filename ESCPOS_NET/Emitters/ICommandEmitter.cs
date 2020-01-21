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

        /* Cash Drawer Commands */
        byte[] CashDrawerOpenPin2();
        byte[] CashDrawerOpenPin5();

        /* Character Commands */
        byte[] SetStyles(PrintStyle style);
        byte[] LeftAlign();
        byte[] RightAlign();
        byte[] CenterAlign();
        byte[] ReverseMode(bool activate);
        byte[] RightCharacterSpacing(int spaceCount);
        byte[] UpsideDownMode(bool activate);

        /* Action Commands */
        byte[] FullCut();
        byte[] PartialCut();
        byte[] FullCutAfterFeed(int lineCount);
        byte[] PartialCutAfterFeed(int lineCount);


        /* Image Commands */
        byte[] SetImageDensity(bool isHiDPI);
        byte[] BufferImage(byte[] image, int maxWidth, bool isLegacy = false, int color = 1);
        byte[] WriteImageFromBuffer();
        byte[] PrintImage(byte[] image, bool isHiDPI, bool isLegacy = false, int maxWidth = -1, int color = 1);
        /* Status Commands */
        byte[] EnableAutomaticStatusBack();
        byte[] EnableAutomaticInkStatusBack();

        /* Barcode Commands */
        byte[] PrintBarcode(BarcodeType type, string barcode, BarcodeCode code = BarcodeCode.CODE_B);
        byte[] SetBarcodeHeightInDots(int height);
        byte[] SetBarWidth(BarWidth width);
        byte[] SetBarLabelPosition(BarLabelPrintPosition position);
        byte[] SetBarLabelFontB(bool fontB);
        /* 2D-Code Commands */
    }
}
