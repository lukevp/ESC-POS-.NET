using ESCPOS_NET.Emitters;

namespace ESCPOS_NET.ConsoleTest
{
    public static partial class Tests
    {
        private const string websiteString = "https://github.com/lukevp/ESC-POS-.NET/";
        public static byte[][] TwoDimensionCodes(ICommandEmitter e) => new byte[][] {
            e.PrintLine("PDF417:"),
            e.Print2DCode(TwoDimensionCodeType.PDF417, websiteString),
            e.PrintLine(),

            e.PrintLine("PDF417 (TINY):"),
            e.Print2DCode(TwoDimensionCodeType.PDF417, websiteString, Size2DCode.TINY),
            e.PrintLine(),

            e.PrintLine("PDF417 (LARGE):"),
            e.Print2DCode(TwoDimensionCodeType.PDF417, websiteString, Size2DCode.LARGE),
            e.PrintLine(),

            e.PrintLine("QRCODE MODEL 1:"),
            e.Print2DCode(TwoDimensionCodeType.QRCODE_MODEL1, websiteString),
            e.PrintLine(),

            e.PrintLine("QRCODE MODEL 2:"),
            e.PrintQRCode(websiteString),
            e.PrintLine(),

            e.PrintLine("QRCODE MICRO:"),
            e.Print2DCode(TwoDimensionCodeType.QRCODE_MICRO, "github.com/lukevp"),
            e.PrintLine(),

            e.PrintLine("QRCODE MODEL 1 (TINY):"),
            e.Print2DCode(TwoDimensionCodeType.QRCODE_MODEL1, websiteString, Size2DCode.TINY),
            e.PrintLine(),

            e.PrintLine("QRCODE MODEL 1 (LARGE):"),
            e.Print2DCode(TwoDimensionCodeType.QRCODE_MODEL1, websiteString, Size2DCode.LARGE),
            e.PrintLine()
        };
    }
}
