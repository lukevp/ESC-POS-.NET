using ESCPOS_NET.Emitters.BaseCommandValues;

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
        byte[] CashDrawerOpenPin2(int pulseOnTimeMs = 120, int pulseOffTimeMs = 240);

        byte[] CashDrawerOpenPin5(int pulseOnTimeMs = 120, int pulseOffTimeMs = 240);

        /* Character Commands */
        byte[] SetStyles(PrintStyle style);

        byte[] LeftAlign();

        byte[] RightAlign();

        byte[] CenterAlign();

        byte[] ReverseMode(bool activate);

        byte[] RightCharacterSpacing(int spaceCount);

        byte[] UpsideDownMode(bool activate);

        byte[] CodePage(CodePage codePage);

        byte[] Color(Color color);

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

        byte[] DisableAutomaticStatusBack();

        byte[] DisableAutomaticInkStatusBack();

        byte[] RequestOnlineStatus();

        byte[] RequestPaperStatus();

        byte[] RequestDrawerStatus();

        byte[] RequestInkStatus();


        /* Barcode Commands */
        byte[] PrintBarcode(BarcodeType type, string barcode, BarcodeCode code = BarcodeCode.CODE_B);

        /* 2D-Code Commands */
        byte[] PrintQRCode(string data, TwoDimensionCodeType type = TwoDimensionCodeType.QRCODE_MODEL2, Size2DCode size = Size2DCode.NORMAL, CorrectionLevel2DCode correction = CorrectionLevel2DCode.PERCENT_7);

        byte[] Print2DCode(TwoDimensionCodeType type, string data, Size2DCode size = Size2DCode.NORMAL, CorrectionLevel2DCode correction = CorrectionLevel2DCode.PERCENT_7);

        byte[] SetBarcodeHeightInDots(int height);

        byte[] SetBarWidth(BarWidth width);

        byte[] SetBarLabelPosition(BarLabelPrintPosition position);

        byte[] SetBarLabelFontB(bool fontB);
    }
}
