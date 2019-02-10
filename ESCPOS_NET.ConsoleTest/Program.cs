using ESCPOS_NET.Emitters;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Threading;

namespace ESCPOS_NET.ConsoleTest
{
    class Program
    {
        private static BasePrinter printer;
        private static ICommandEmitter e;

        static void Main(string[] args)
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                printer = new SerialPrinter("COM20", 115200);
            }
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            {
                printer = new FilePrinter("/dev/usb/lp0");
            }
            e = new EPSON_TM_T20II();
            List<string> testCases = new List<string>()
            {
                "Printing",
                "Line Spacing",
                "Barcode Styles",
                "Text Styles",
                "Full Receipt"
            };
            while (true)
            {
                int i = 0;
                foreach (var item in testCases)
                {
                    i += 1;
                    Console.WriteLine($"{i} : {item}");
                }
                Console.WriteLine("99 : Exit");
                Console.Write("Execute Test: ");
                int choice;

                try
                {
                    choice = Convert.ToInt32(Console.ReadLine());
                    if (choice != 99 && (choice < 1 || choice > testCases.Count ))
                    {
                        throw new InvalidOperationException();
                    }
                }
                catch
                {
                    Console.WriteLine("Invalid entry. Please try again.");
                    continue;
                }

                if (choice == 99) return;

                Console.Clear();

                Setup();

                printer.Write(e.PrintLine($"== [ Start {testCases[choice - 1]} ] =="));

                switch (choice)
                {
                    case 1:
                        printer.Write(Tests.Printing(e));
                        break;
                    case 2:
                        printer.Write(Tests.LineSpacing(e));
                        break;
                    case 3:
                        printer.Write(Tests.BarcodeStyles(e));
                        break;
                    case 4:
                        printer.Write(Tests.TextStyles(e));
                        break;
                    case 5:
                        printer.Write(Tests.Receipt(e));
                        break;
                    default:
                        Console.WriteLine("Invalid entry.");
                        break;
                }

                Setup();
                printer.Write(e.PrintLine($"== [ End {testCases[choice - 1]} ] =="));
                printer.Write(e.PartialCutAfterFeed(5));

                //TestCutter();
                //TestMultiLineWrite();
                //TestHEBReceipt();
                // TODO: write a sanitation check.
                // TODO: make DPI to inch convesion function
                // TODO: full cuts and reverse feeding not implemented on epson...  should throw exception?
                // TODO: make an input loop that lets you execute each test separately.
                // TODO: also make an automatic runner that runs all tests (command line).
                //Thread.Sleep(1000);
            }
        }

        static void Setup()
        {
            printer.Write(e.Initialize());
            printer.Write(e.Enable());
        }

        /*
        static void TestCutter()
        {
            sp.Write(e.PrintLine("Performing Full Cut (no feed)."));
            sp.Write(e.FullCut());
            sp.Write(e.PrintLine("Performing Partial Cut (no feed)."));
            sp.Write(e.PartialCut());
            sp.Write(e.PrintLine("Performing Full Cut (1000 dot feed)."));
            sp.Write(e.FullCutAfterFeed(1000));
            sp.Write(e.PrintLine("Performing Partial Cut (1000 dot feed)."));
            sp.Write(e.PartialCutAfterFeed(1000));
        }

        static void TestBarcodeTypes()
        {
            sp.Write(
              e.PrintLine("UPC_A: 012345678905 "),
                e.SetBarLabelFontB(false),
                e.SetBarLabelPosition(BarLabelPrintPosition.Below),
                e.PrintBarcode(BarcodeType.UPC_A, "012345678905")
                );
            /*
             * 
                e.PrintBarcode(BarcodeType.CODE128, "10945500020119184400014"),
                /*
            e.PrintBarcode(BarcodeType., "041220096138"),
             *         UPC_A                       = 0x41,
        UPC_E                       = 0x42,
        JAN13_EAN13                 = 0x43,
        JAN8_EAN8                   = 0x44,
        CODE39                      = 0x45,
        ITF                         = 0x46,
        CODABAR_NW_7                = 0x47,
        CODE93                      = 0x48,
        CODE128                     = 0x49,
        GS1_128                     = 0x4A,
        GS1_DATABAR_OMNIDIRECTIONAL = 0x4B,
        GS1_DATABAR_TRUNCATED       = 0x4C,
        GS1_DATABAR_LIMITED         = 0x4D,
        GS1_DATABAR_EXPANDED        = 0x4E


            */
        /*
    }


    static void TestHEBReceipt()
    {
        sp.Write(
            //e.FeedDots(2000),
            // TODO: logo
            e.CenterAlign(),
            //e.PrintLine("BHONEYWELLv"),
            //e.SetBarcodeHeightInDots(360),
            //e.SetBarWidth(BarWidth.Regular),
            //e.SetBarLabelPosition(BarLabelPrintPosition.None),
            e.PrintBarcode(BarcodeType.JAN13_EAN13, "041220096138"),
            /*
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
            e.PrintLine("  CUST: 87654321"),*//*
        e.FullCutAfterFeed(1000)
        );
    }
   */
    }
}
