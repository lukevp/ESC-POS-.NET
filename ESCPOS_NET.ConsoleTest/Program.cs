using ESCPOS_NET.Emitters;
using System;
using System.Threading;

namespace ESCPOS_NET.ConsoleTest
{
    class Program
    {
        private static SerialPrinter sp;
        private static ICommandEmitter emitter;


        static void Main(string[] args)
        {
            sp = new SerialPrinter("COM20", 115200);
            emitter = new EPSON_TM_T20II();

            Setup();
            //TestStyles();
            //TestLineFeeds();
            //TestCutter();
            //TestMultiLineWrite();
            // TODO: write a sanitation check.
            // TODO: full cuts and reverse feeding not implemented on epson...  should throw exception?

            Thread.Sleep(5000);
        }

        static void Setup()
        {
            sp.Write(emitter.Initialize);
            sp.Write(emitter.Enable);
        }

        static void TestMultiLineWrite()
        {
            sp.Write(emitter.PrintLines("This is a test\r\nOf multiline\rprinting with different\n line separators.\n\n"));
        }

        static void TestStyles()
        {
            sp.Write(emitter.SetStyles(PrintStyle.None));
            sp.Write(emitter.PrintLines("Default: The quick brown fox jumped over the lazy dogs.\n"));
            sp.Write(emitter.SetStyles(PrintStyle.FontB));
            sp.Write(emitter.PrintLines("Font B: The quick brown fox jumped over the lazy dogs.\n"));
            sp.Write(emitter.SetStyles(PrintStyle.Bold));
            sp.Write(emitter.PrintLines("Bold: The quick brown fox jumped over the lazy dogs.\n"));
            sp.Write(emitter.SetStyles(PrintStyle.Underline));
            sp.Write(emitter.PrintLines("Underline: The quick brown fox jumped over the lazy dogs.\n"));
            sp.Write(emitter.SetStyles(PrintStyle.DoubleWidth));
            sp.Write(emitter.PrintLines("DoubleWidth: The quick brown fox jumped over the lazy dogs.\n"));
            sp.Write(emitter.SetStyles(PrintStyle.DoubleHeight));
            sp.Write(emitter.PrintLines("DoubleHeight: The quick brown fox jumped over the lazy dogs.\n"));
            sp.Write(emitter.SetStyles(PrintStyle.FontB | PrintStyle.DoubleHeight | PrintStyle.DoubleWidth | PrintStyle.Underline | PrintStyle.Bold));
            sp.Write(emitter.PrintLines("All Styles: The quick brown fox jumped over the lazy dogs.\n"));
            sp.Write(emitter.SetStyles(PrintStyle.None));
        }
        static void TestLineFeeds()
        {
            sp.Write(emitter.PrintLine("Feeding 1000 dots."));
            sp.Write(emitter.FeedDots(1000));
            sp.Write(emitter.PrintLine("Feeding 3 lines."));
            sp.Write(emitter.FeedLines(3));
            sp.Write(emitter.PrintLine("Done Feeding."));
            sp.Write(emitter.PrintLine("Reverse Feeding 6 lines."));
            sp.Write(emitter.FeedLinesReverse(6));
            sp.Write(emitter.PrintLine("Done Reverse Feeding."));
        }

        static void TestCutter()
        {
            sp.Write(emitter.PrintLine("Performing Full Cut (no feed)."));
            sp.Write(emitter.FullCut);
            sp.Write(emitter.PrintLine("Performing Partial Cut (no feed)."));
            sp.Write(emitter.PartialCut);
            sp.Write(emitter.PrintLine("Performing Full Cut (1000 dot feed)."));
            sp.Write(emitter.FullCutAfterFeed(1000));
            sp.Write(emitter.PrintLine("Performing Partial Cut (1000 dot feed)."));
            sp.Write(emitter.PartialCutAfterFeed(1000));
        }
    }
}
