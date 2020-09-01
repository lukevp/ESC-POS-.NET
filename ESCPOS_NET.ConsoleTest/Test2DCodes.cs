using ESCPOS_NET.Emitters;
using ESCPOS_NET.Emitters.Enums;

namespace ESCPOS_NET.ConsoleTest
{
    public static partial class Tests
    {
        private const string websiteString = "https://github.com/lukevp/ESC-POS-.NET/";

        public static byte[] TwoDimensionCodes(ICommandEmitter e)
        {
            return ByteSplicer.Combine(
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
                e.Print2DCode(TwoDimensionCodeType.QRCODE_MODEL2, websiteString),
                e.PrintLine(),
                e.PrintLine("QRCODE MICRO:"),
                e.Print2DCode(TwoDimensionCodeType.QRCODE_MICRO, websiteString),
                e.PrintLine(),
                e.PrintLine("QRCODE MODEL 1 (TINY):"),
                e.Print2DCode(TwoDimensionCodeType.QRCODE_MODEL1, websiteString, Size2DCode.TINY),
                e.PrintLine(),
                e.PrintLine("QRCODE MODEL 1 (LARGE):"),
                e.Print2DCode(TwoDimensionCodeType.QRCODE_MODEL1, websiteString, Size2DCode.LARGE),
                e.PrintLine()
            );
        }
    }
}