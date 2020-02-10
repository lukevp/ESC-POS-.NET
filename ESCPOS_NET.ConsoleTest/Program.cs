using ESCPOS_NET.Emitters;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace ESCPOS_NET.ConsoleTest
{
    class Program
    {
        private static BasePrinter printer;
        private static ICommandEmitter e;

        static void Main(string[] args)
        {

            Console.WriteLine("ESCPOS_NET Test Application...");
            Console.WriteLine("1 ) Test Serial Port");
            Console.WriteLine("2 ) Test Network Printer");
            Console.Write("Choice: ");
            string comPort = "";
            string baudRate;
            string ip;
            string networkPort;
            var response = Console.ReadLine();
            var valid = new List<string> { "1", "2" };
            if (!valid.Contains(response)) response = "1";
            int choice = int.Parse(response);

            if (choice == 1)
            {
                if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                {
                    while (!comPort.StartsWith("COM"))
                    { 
                        Console.Write("COM Port (eg. COM5): ");
                        comPort = Console.ReadLine();
                        if (string.IsNullOrWhiteSpace(comPort))
                        {
                            comPort = "COM5";
                        }
                    }
                    Console.Write("Baud Rate (eg. 115200): ");
                    baudRate = Console.ReadLine();
                    if (string.IsNullOrWhiteSpace(baudRate))
                    {
                        baudRate = "115200";
                    }
                    printer = new SerialPrinter(comPort, int.Parse(baudRate));
                }
                else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
                {
                    Console.Write("File / virtual com path (eg. /dev/usb/lp0): ");
                    comPort = Console.ReadLine();
                    if (string.IsNullOrWhiteSpace(comPort))
                    {
                        comPort = "/dev/usb/lp0";
                    }
                    printer = new FilePrinter(comPort);
                }
            }
            else if (choice == 2)
            {
                Console.Write("IP Address (eg. 192.168.1.240): ");
                ip = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(ip))
                {
                    ip = "192.168.1.240";
                }
                Console.Write("TCP Port (eg. 9000): ");
                networkPort = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(networkPort))
                {
                    networkPort = "9000";
                }
                printer = new NetworkPrinter(ip, int.Parse(networkPort));
            }

            bool monitor = false;
            Console.Write("Turn on Live Status Back Monitoring? (y/n): ");
            response = Console.ReadLine().Trim().ToLowerInvariant();
            if (response.Length >= 1 && response[0] == 'y')
            {
                monitor = true;
            }
            
            e = new EPSON();
            List<string> testCases = new List<string>()
            {
                "Printing",
                "Line Spacing",
                "Barcode Styles",
                "Barcode Types",
                "Text Styles",
                "Full Receipt",
                "Images",
                "Legacy Images",
                "Large Byte Arrays",
                "Cash Drawer Pin2",
                "Cash Drawer Pin5"
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

                try
                {
                    choice = Convert.ToInt32(Console.ReadLine());
                    if (choice != 99 && (choice < 1 || choice > testCases.Count))
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

                if (monitor)
                { 
                    printer.StartMonitoring();
                }
                Setup(monitor);
                printer?.Write(e.PrintLine($"== [ Start {testCases[choice - 1]} ] =="));

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
                        printer.Write(Tests.BarcodeTypes(e));
                        break;
                    case 5:
                        printer.Write(Tests.TextStyles(e));
                        break;
                    case 6:
                        printer.Write(Tests.Receipt(e));
                        break;
                    case 7:
                        printer.Write(Tests.Images(e, false));
                        break;
                    case 8:
                        printer.Write(Tests.Images(e, true));
                        break;
                    case 9:
                        try
                        {
                            printer.Write(Tests.TestLargeByteArrays(e));
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine($"Aborting print due to test failure. Exception: {e?.Message}, Stack Trace: {e?.GetBaseException()?.StackTrace}");
                        }
                        break;
                    case 10:
                        printer.Write(Tests.CashDrawerOpenPin2(e));
                        break;
                    case 11:
                        printer.Write(Tests.CashDrawerOpenPin5(e));
                        break;
                    default:
                        Console.WriteLine("Invalid entry.");
                        break;
                }

                Setup(monitor);
                printer?.Write(e.PrintLine($"== [ End {testCases[choice - 1]} ] =="));
                printer?.Write(e.PartialCutAfterFeed(5));

                // TODO: write a sanitation check.
                // TODO: make DPI to inch conversion function
                // TODO: full cuts and reverse feeding not implemented on epson...  should throw exception?
                // TODO: make an input loop that lets you execute each test separately.
                // TODO: also make an automatic runner that runs all tests (command line).
                //Thread.Sleep(1000);
            }
        }

        static void StatusChanged(object sender, EventArgs ps)
        {
            var status = (PrinterStatusEventArgs)ps;
            Console.WriteLine($"Printer Online Status: {status.IsPrinterOnline}");
            Console.WriteLine(JsonConvert.SerializeObject(status));
        }
        private static bool _hasEnabledStatusMonitoring = false;

        static void Setup(bool enableStatusBackMonitoring)
        {
            if (printer != null)
            {
                // Only register status monitoring once.
                if (!_hasEnabledStatusMonitoring)
                {
                    printer.StatusChanged += StatusChanged;
                    _hasEnabledStatusMonitoring = true;
                }
                printer?.Write(e.Initialize());
                printer?.Write(e.Enable());
                if (enableStatusBackMonitoring)
                {
                    printer.Write(e.EnableAutomaticStatusBack());
                }
            }
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
