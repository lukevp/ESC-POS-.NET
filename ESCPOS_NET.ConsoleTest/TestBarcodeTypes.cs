using ESCPOS_NET.Emitters;

namespace ESCPOS_NET.ConsoleTest
{

    public static partial class Tests
    {
        public static byte[][] BarcodeTypes(ICommandEmitter e) => new byte[][] {
            e.SetBarcodeHeightInDots(600),
            e.SetBarWidth(BarWidth.Thinnest),
            e.SetBarLabelPosition(BarLabelPrintPosition.Below),

            e.PrintLine("CODABAR_NW_7:"),
            e.PrintBarcode(BarcodeType.CODABAR_NW_7, "A31117013206375B"),

            e.PrintLine("CODE128:"),
            e.PrintBarcode(BarcodeType.CODE128, "ESC_POS_NET"),
            e.PrintLine(),

            e.PrintLine("CODE128 Type C:"),
            e.PrintBarcode(BarcodeType.CODE128, "123456789101", BarcodeCode.CODE_C),
            e.PrintLine(),

            e.PrintLine("CODE39:"),
            e.PrintBarcode(BarcodeType.CODE39, "*ESC-POS-NET*"),
            e.PrintLine(),

            e.PrintLine("CODE93:"),
            e.PrintBarcode(BarcodeType.CODE93, "*ESC_POS_NET*"),
            e.PrintLine(),

            e.PrintLine("GS1_128:"),
            e.PrintBarcode(BarcodeType.GS1_128, "(01)9501234567890*"),
            e.PrintLine(),

            e.PrintLine("GS1_DATABAR_EXPANDED:"),
            e.PrintBarcode(BarcodeType.GS1_DATABAR_EXPANDED, "0001234567890"),
            e.PrintLine(),

            e.PrintLine("GS1_DATABAR_LIMITED:"),
            e.PrintBarcode(BarcodeType.GS1_DATABAR_LIMITED, "0001234567890"),
            e.PrintLine(),

            e.PrintLine("GS1_DATABAR_OMNIDIRECTIONAL:"),
            e.PrintBarcode(BarcodeType.GS1_DATABAR_OMNIDIRECTIONAL, "0001234567890"),
            e.PrintLine(),

            e.PrintLine("GS1_DATABAR_TRUNCATED:"),
            e.PrintBarcode(BarcodeType.GS1_DATABAR_TRUNCATED, "0001234567890"),
            e.PrintLine(),

            e.PrintLine("ITF:"),
            e.PrintBarcode(BarcodeType.ITF, "1234567895"),
            e.PrintLine(),

            e.PrintLine("JAN13_EAN13:"),
            e.PrintBarcode(BarcodeType.JAN13_EAN13, "5901234123457"),
            e.PrintLine(),

            e.PrintLine("JAN8_EAN8:"),
            e.PrintBarcode(BarcodeType.JAN8_EAN8, "96385074"),
            e.PrintLine(),

            e.PrintLine("UPC_A:"),
            e.PrintBarcode(BarcodeType.UPC_A, "042100005264"),
            e.PrintLine(),

            e.PrintLine("UPC_E:"),
            e.PrintBarcode(BarcodeType.UPC_E, "425261"),
            e.PrintLine()
        };
    }
}
