using ESCPOS_NET.Emitters;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Linq;


namespace ESCPOS_NET.ConsoleTest
{
    internal class Program
    {
        private static BasePrinter printer;
        private static ICommandEmitter emitter;

        static void Main(string[] args)
        {

            Console.WriteLine("ESCPOS_NET Test Application...");

            Console.WriteLine($"{(int)PrinterTypeOption.EPSON} {PrinterTypeOption.EPSON} Printer");
            Console.WriteLine($"{(int)PrinterTypeOption.CUSTOM} {PrinterTypeOption.CUSTOM} Printer");
            Console.Write("Choice: ");
            var response = Console.ReadLine();
            var valid = new List<string> { ((int)PrinterTypeOption.EPSON).ToString(), ((int)PrinterTypeOption.CUSTOM).ToString() };
            if (!valid.Contains(response))
            {
                response = ((int)PrinterTypeOption.EPSON).ToString();
            }
            int choice = int.Parse(response);
            switch((PrinterTypeOption)choice)
            {
                case PrinterTypeOption.EPSON:
                    emitter = new EPSON();
                    break;
                case PrinterTypeOption.CUSTOM:
                    emitter = new CUSTOM();
                    break;
            }

            Console.WriteLine("1 ) Test Serial Port");
            Console.WriteLine("2 ) Test Network Printer");
            Console.Write("Choice: ");
            string comPort = "";
            string baudRate;
            string ip;
            string networkPort;
            response = Console.ReadLine();
            valid = new List<string> { "1", "2" };
            if (!valid.Contains(response))
            {
                response = "1";
            }

            choice = int.Parse(response);

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
                    printer = new SerialPrinter(portName: comPort, baudRate: int.Parse(baudRate));
                }
                else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
                {
                    Console.Write("File / virtual com path (eg. /dev/usb/lp0): ");
                    comPort = Console.ReadLine();
                    if (string.IsNullOrWhiteSpace(comPort))
                    {
                        comPort = "/dev/usb/lp0";
                    }
                    printer = new FilePrinter(filePath: comPort, false);
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
                printer = new NetworkPrinter(ipAddress: ip, port: int.Parse(networkPort), reconnectOnTimeout: true);
            }

            bool monitor = false;
            Console.Write("Turn on Live Status Back Monitoring? (y/n): ");
            response = Console.ReadLine().Trim().ToLowerInvariant();
            if (response.Length >= 1 && response[0] == 'y')
            {
                monitor = true;
            }

            var testCases = new Dictionary<Option, string>()
            {
                { Option.Printing, "Printing" },
                { Option.LineSpacing, "Line Spacing" },
                { Option.BarcodeStyles, "Barcode Styles" },
                { Option.BarcodeTypes, "Barcode Types" },
                { Option.TwoDimensionCodes, "2D Codes" },
                { Option.TextStyles, "Text Styles" },
                { Option.FullReceipt, "Full Receipt" },
                { Option.CodePages, "Code Pages (Euro, Katakana, Etc)" },
                { Option.Images, "Images" },
                { Option.LegacyImages, "Legacy Images" },
                { Option.LargeByteArrays, "Large Byte Arrays" },
                { Option.CashDrawerPin2, "Cash Drawer Pin2" },
                { Option.CashDrawerPin5, "Cash Drawer Pin5" },
                { Option.Exit, "Exit" }

            };
            while (true)
            {
                foreach (var item in testCases)
                {
                    Console.WriteLine($"{(int)item.Key} : {item.Value}");
                }
                Console.Write("Execute Test: ");

                if (!int.TryParse(Console.ReadLine(), out choice) || !Enum.IsDefined(typeof(Option), choice))
                {
                    Console.WriteLine("Invalid entry. Please try again.");
                    continue;
                }

                var enumChoice = (Option)choice;
                if (enumChoice == Option.Exit)
                {
                    return;
                }

                Console.Clear();

                if (monitor)
                {
                    printer.StartMonitoring();
                }
                Setup(monitor);

                printer?.Write(emitter.PrintLine($"== [ Start {testCases[enumChoice]} ] =="));

                switch (enumChoice)
                {
                    case Option.Printing:
                        printer.Write(Tests.Printing(emitter));
                        break;
                    case Option.LineSpacing:
                        printer.Write(Tests.LineSpacing(emitter));
                        break;
                    case Option.BarcodeStyles:
                        printer.Write(Tests.BarcodeStyles(emitter));
                        break;
                    case Option.BarcodeTypes:
                        printer.Write(Tests.BarcodeTypes(emitter));
                        break;
                    case Option.TwoDimensionCodes:
                        printer.Write(Tests.TwoDimensionCodes(emitter));
                        break;
                    case Option.TextStyles:
                        printer.Write(Tests.TextStyles(emitter));
                        break;
                    case Option.FullReceipt:
                        printer.Write(Tests.Receipt(emitter));
                        break;
                    case Option.CodePages:
                        var codePage = CodePage.PC437_USA_STANDARD_EUROPE_DEFAULT;
                        Console.WriteLine("To run this test, you must select the index of a code page to print.");
                        Console.WriteLine("The default CodePage is typically CodePage 0 (USA/International).");
                        Console.WriteLine("Press enter to see the list of Code Pages.");
                        Console.ReadLine();
                        List<object> codePages = new List<object>();
                        foreach (var value in Enum.GetValues(typeof(CodePage)))
                        {
                            codePages.Add(value);
                        }
                        for(int i = 0; i < codePages.Count; i++)
                        {
                            Console.WriteLine(i.ToString() + ": " + codePages[i] + "  (Page " + ((int)codePages[i]) + ")");
                        }
                        Console.Write("Index for test Code Page (NOT Page #): ");
                        var page = Console.ReadLine();
                        try
                        {
                            codePage = (CodePage)codePages[int.Parse(page)];
                        }
                        catch
                        {
                            Console.WriteLine("Invalid code page selected, defaulting to CodePage 0.");
                        }

                        printer.Write(Tests.CodePages(e, codePage));
                        break;
                    case Option.Images:
                        printer.Write(Tests.Images(emitter, false));
                        break;
                    case Option.LegacyImages:
                        printer.Write(Tests.Images(emitter, true));
                        break;
                    case Option.LargeByteArrays:
                        try
                        {
                            printer.Write(Tests.TestLargeByteArrays(emitter));
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine($"Aborting print due to test failure. Exception: {e?.Message}, Stack Trace: {e?.GetBaseException()?.StackTrace}");
                        }
                        break;
                    case Option.CashDrawerPin2:
                        printer.Write(Tests.CashDrawerOpenPin2(emitter));
                        break;
                    case Option.CashDrawerPin5:
                        printer.Write(Tests.CashDrawerOpenPin5(emitter));
                        break;
                    default:
                        Console.WriteLine("Invalid entry.");
                        break;
                }

                Setup(monitor);
                printer?.Write(emitter.PrintLine($"== [ End {testCases[enumChoice]} ] =="));
                printer?.Write(emitter.PartialCutAfterFeed(5));

                // TODO: also make an automatic runner that runs all tests (command line).
            }
        }

        public enum PrinterTypeOption
        {
            EPSON = 1,
            CUSTOM = 2,
        }

        public enum Option
        {
            Printing = 1,
            LineSpacing,
            BarcodeStyles,
            BarcodeTypes,
            TwoDimensionCodes,
            TextStyles,
            FullReceipt,
            CodePages,
            Images,
            LegacyImages,
            LargeByteArrays,
            CashDrawerPin2,
            CashDrawerPin5,
            Exit = 99
        }

        private static void StatusChanged(object sender, EventArgs ps)
        {
            var status = (PrinterStatusEventArgs)ps;
            Console.WriteLine($"Printer Online Status: {status.IsCoverOpen}");
            Console.WriteLine(JsonConvert.SerializeObject(status));
        }
        private static bool _hasEnabledStatusMonitoring = false;

        private static void Setup(bool enableStatusBackMonitoring)
        {
            if (printer != null)
            {
                // Only register status monitoring once.
                if (!_hasEnabledStatusMonitoring)
                {
                    printer.StatusChanged += StatusChanged;
                    _hasEnabledStatusMonitoring = true;
                }
                printer?.Write(emitter.Initialize());
                printer?.Write(emitter.Enable());
                if (enableStatusBackMonitoring)
                {
                    printer.Write(emitter.EnableAutomaticStatusBack());
                }
            }
        }
    }
}
