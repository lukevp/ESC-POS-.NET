using ESCPOS_NET.Emitters;

namespace ESCPOS_NET.ConsoleTest
{

    public static partial class Tests
    {
        public static byte[][] BarcodeStyles(ICommandEmitter e) => new byte[][] {
            //TODO: test all widths and put bar in front in label
            e.PrintLine("Thinnest Width:"),
            e.SetBarcodeHeightInDots(300),
            e.SetBarWidth(BarWidth.Thinnest),
            e.PrintBarcode(BarcodeType.UPC_A, "012345678905"),

            e.PrintLine("Thin Width:"),
            e.SetBarcodeHeightInDots(300),
            e.SetBarWidth(BarWidth.Thin),
            e.PrintBarcode(BarcodeType.UPC_A, "012345678905"),

            e.PrintLine("Default Width:"),
            e.SetBarcodeHeightInDots(300),
            e.SetBarWidth(BarWidth.Default),
            e.PrintBarcode(BarcodeType.UPC_A, "012345678905"),

            e.PrintLine("Thicker Width:"),
            e.SetBarcodeHeightInDots(300),
            e.SetBarWidth(BarWidth.Thick),
            e.PrintBarcode(BarcodeType.UPC_A, "012345678905"),

            e.PrintLine("Thickest Width:"),
            e.SetBarcodeHeightInDots(300),
            e.SetBarWidth(BarWidth.Thickest),
            e.PrintBarcode(BarcodeType.UPC_A, "012345678905"),


            e.PrintLine("Short (50 dots):"),
            e.SetBarcodeHeightInDots(50),
            e.SetBarWidth(BarWidth.Default),
            e.PrintBarcode(BarcodeType.UPC_A, "012345678905"),

            e.PrintLine("Tall (255 dots):"),
            e.SetBarcodeHeightInDots(255),
            e.PrintBarcode(BarcodeType.UPC_A, "012345678905"),

            e.PrintLine("Label Above:"),
            e.SetBarcodeHeightInDots(50),
            e.SetBarLabelPosition(BarLabelPrintPosition.Above),
            e.PrintBarcode(BarcodeType.UPC_A, "012345678905"),

            e.PrintLine("Label Above and Below:"),
            e.SetBarLabelPosition(BarLabelPrintPosition.Both),
            e.PrintBarcode(BarcodeType.UPC_A, "012345678905"),

            e.PrintLine("Label Below:"),
            e.SetBarLabelPosition(BarLabelPrintPosition.Below),
            e.PrintBarcode(BarcodeType.UPC_A, "012345678905"),

            e.PrintLine("Font B Label Below:"),
            e.SetBarLabelFontB(true),
            e.PrintBarcode(BarcodeType.UPC_A, "012345678905")
        };
    }
}
