using ESCPOS_NET.Emitters;
using System;
using System.Threading;

namespace ESCPOS_NET.ConsoleTest
{
    class Program
    {
        private static SerialPrinter sp;
        private static ICommandEmitter e;


        static void Main(string[] args)
        {
            sp = new SerialPrinter("COM20", 115200);
            e = new EPSON_TM_T20II();

            Setup();
            //TestStyles();
            //TestLineFeeds();
            //TestCutter();
            //TestMultiLineWrite();
            TestReceipt();
            // TODO: write a sanitation check.
            // TODO: full cuts and reverse feeding not implemented on epson...  should throw exception?

            Thread.Sleep(5000);
        }

        static void Setup()
        {
            sp.Write(e.Initialize);
            sp.Write(e.Enable);
        }

        static void TestMultiLineWrite()
        {
            sp.Write(e.PrintLines("This is a test\r\nOf multiline\rprinting with different\n line separators.\n\n"));
        }

        static void TestStyles()
        {
            sp.Write(e.SetStyles(PrintStyle.None));
            sp.Write(e.PrintLines("Default: The quick brown fox jumped over the lazy dogs.\n"));
            sp.Write(e.SetStyles(PrintStyle.FontB));
            sp.Write(e.PrintLines("Font B: The quick brown fox jumped over the lazy dogs.\n"));
            sp.Write(e.SetStyles(PrintStyle.Bold));
            sp.Write(e.PrintLines("Bold: The quick brown fox jumped over the lazy dogs.\n"));
            sp.Write(e.SetStyles(PrintStyle.Underline));
            sp.Write(e.PrintLines("Underline: The quick brown fox jumped over the lazy dogs.\n"));
            sp.Write(e.SetStyles(PrintStyle.DoubleWidth));
            sp.Write(e.PrintLines("DoubleWidth: The quick brown fox jumped over the lazy dogs.\n"));
            sp.Write(e.SetStyles(PrintStyle.DoubleHeight));
            sp.Write(e.PrintLines("DoubleHeight: The quick brown fox jumped over the lazy dogs.\n"));
            sp.Write(e.SetStyles(PrintStyle.FontB | PrintStyle.DoubleHeight | PrintStyle.DoubleWidth | PrintStyle.Underline | PrintStyle.Bold));
            sp.Write(e.PrintLines("All Styles: The quick brown fox jumped over the lazy dogs.\n"));
            sp.Write(e.SetStyles(PrintStyle.None));
        }
        static void TestLineFeeds()
        {
            sp.Write(e.PrintLine("Feeding 1000 dots."));
            sp.Write(e.FeedDots(1000));
            sp.Write(e.PrintLine("Feeding 3 lines."));
            sp.Write(e.FeedLines(3));
            sp.Write(e.PrintLine("Done Feeding."));
            sp.Write(e.PrintLine("Reverse Feeding 6 lines."));
            sp.Write(e.FeedLinesReverse(6));
            sp.Write(e.PrintLine("Done Reverse Feeding."));
        }

        static void TestCutter()
        {
            sp.Write(e.PrintLine("Performing Full Cut (no feed)."));
            sp.Write(e.FullCut);
            sp.Write(e.PrintLine("Performing Partial Cut (no feed)."));
            sp.Write(e.PartialCut);
            sp.Write(e.PrintLine("Performing Full Cut (1000 dot feed)."));
            sp.Write(e.FullCutAfterFeed(1000));
            sp.Write(e.PrintLine("Performing Partial Cut (1000 dot feed)."));
            sp.Write(e.PartialCutAfterFeed(1000));
        }

        static void TestReceipt()
        {
            // TODO: write 3 and barcode
            sp.Write(
                e.FeedDots(2000),
                e.CenterAlign,
                e.PrintLine("3"),
                e.SetBarcodeHeightInDots(360),
                e.SetBarWidth(BarWidth.Regular),
                e.SetBarLabelPosition(BarLabelPrintPosition.None),
                e.PrintBarcode(BarcodeType.ITF, "0123456789"),
                e.PrintLine(""),
                e.PrintLine("B&H PHOTO & VIDEO"),
                e.PrintLine("420 NINTH AVE."),
                e.PrintLine("NEW YORK, NY 10001"),
                e.PrintLine("(212) 502-6380 - (800)947-9975"),
                e.SetStyles(PrintStyle.Underline),
                e.PrintLine("www.bhphotovideo.com"),
                e.SetStyles(PrintStyle.None),
                e.PrintLine(""),
                e.LeftAlign,
                e.PrintLine("Order: 123456789        Date: 02/01/19"),
                e.PrintLine(""),
                e.PrintLine(""),
                e.SetStyles(PrintStyle.FontB),
                e.PrintLine("1   TRITON LOW-NOISE IN-LINE MICROPHONE PREAMP"),
                e.PrintLine("    TRFETHEAD/FETHEAD                        89.95         89.95"),
                e.PrintLine("----------------------------------------------------------------"),
                e.RightAlign,
                e.PrintLine("SUBTOTAL         89.95"),
                e.PrintLine("Total Order:         89.95"),
                e.PrintLine("Total Payment:              "),
                e.PrintLine(""),
                e.LeftAlign,
                e.SetStyles(PrintStyle.Bold | PrintStyle.FontB),
                e.PrintLine("SOLD TO:                        SHIP TO:"),
                e.SetStyles(PrintStyle.FontB),
                e.PrintLine("  LUKE PAIREEPINART               LUKE PAIREEPINART"),
                e.PrintLine("  123 FAKE ST.                    123 FAKE ST."),
                e.PrintLine("  DECATUR, IL 12345               DECATUR, IL 12345"),
                e.PrintLine("  (123)456-7890                   (123)456-7890"),
                e.PrintLine("  CUST: 87654321"),
                e.FullCutAfterFeed(1000)
            );
        }
    }
}
