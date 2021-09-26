﻿using ESCPOS_NET.Emitters;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Linq;
using Microsoft.Extensions.Logging;
using System.Threading;

namespace ESCPOS_NET.ConsoleTest
{
    internal class Program
    {
        private static BasePrinter printer;
        private static ICommandEmitter e;
        /// <summary>
        /// Indicate whether to test with long-lived printer object or create and dispose every time.
        /// </summary>
        private const bool SINGLETON_PRINTER_OBJECT = false;

        static void Main(string[] args)
        {
            Console.WriteLine("Welcome to the ESCPOS_NET Test Application!");
            Console.Write("Would you like to see all debug messages? (y/n): ");
            var response = Console.ReadLine().Trim().ToLowerInvariant();
            var logLevel = LogLevel.Information;
            if (response.Length >= 1 && response[0] == 'y')
            {
                Console.WriteLine("Debugging enabled!");
                logLevel = LogLevel.Trace;
            }
            var factory = LoggerFactory.Create(b => b.AddConsole().SetMinimumLevel(logLevel));
            var logger = factory.CreateLogger<Program>();
            ESCPOS_NET.Logging.Logger = logger;

            Console.WriteLine("1 ) Test Serial Port");
            Console.WriteLine("2 ) Test Network Printer");
            Console.Write("Choice: ");
            string comPort = "";
            string ip = "";
            string networkPort = "";
            Action createPrinter = null;
            response = Console.ReadLine();
            var valid = new List<string> { "1", "2" };
            if (!valid.Contains(response))
            {
                response = "1";
            }

            int choice = int.Parse(response);

            if (choice == 1)
            {
                if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                {
                    while (!comPort.StartsWith("COM"))
                    {
                        Console.Write("COM Port (enter for default COM5): ");
                        comPort = Console.ReadLine();
                        if (string.IsNullOrWhiteSpace(comPort))
                        {
                            comPort = "COM5";
                        }
                    }                    
                    Console.Write("Baud Rate (enter for default 115200): ");                    
                    if (!int.TryParse(Console.ReadLine(), out var baudRate))
                    {
                        baudRate = 115200;
                    }
                    if (SINGLETON_PRINTER_OBJECT)
                        printer = new SerialPrinter(portName: comPort, baudRate: baudRate);
                    else
                        createPrinter = () => { printer = new SerialPrinter(portName: comPort, baudRate: baudRate); };
                }
                else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
                {
                    Console.Write("File / virtual com path (eg. /dev/usb/lp0): ");
                    comPort = Console.ReadLine();
                    if (string.IsNullOrWhiteSpace(comPort))
                    {
                        comPort = "/dev/usb/lp0";
                    }
                    if (SINGLETON_PRINTER_OBJECT)
                        printer = new FilePrinter(filePath: comPort, false);
                    else
                        createPrinter = () => { printer = new FilePrinter(filePath: comPort, false); };
                }
            }
            else if (choice == 2)
            {
                Console.Write("IP Address (eg. 192.168.1.240): ");
                ip = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(ip))
                {
                    ip = "127.0.0.1"; // default to local for using TCPPrintServerTest
                }
                Console.Write("TCP Port (enter for default 9100): ");
                networkPort = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(networkPort))
                {
                    networkPort = "9100";
                }
                if (SINGLETON_PRINTER_OBJECT)
                    printer = new NetworkPrinter(settings: new NetworkPrinterSettings() { ConnectionString = $"{ip}:{networkPort}" });
                else
                    createPrinter = () => { printer = new NetworkPrinter(settings: new NetworkPrinterSettings() { ConnectionString = $"{ip}:{networkPort}" }); };
            }

            bool monitor = false;
            Thread.Sleep(500);
            Console.Write("Turn on Live Status Back Monitoring? (y/n): ");
            response = Console.ReadLine().Trim().ToLowerInvariant();
            if (response.Length >= 1 && response[0] == 'y')
            {
                monitor = true;
            }

            e = new EPSON();
            var testCases = new Dictionary<Option, string>()
            {
                { Option.SingleLinePrinting, "Single Line Printing" },
                { Option.MultiLinePrinting, "Multi-line Printing" },
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

                if (!SINGLETON_PRINTER_OBJECT)
                {
                    createPrinter();
                }

                var enumChoice = (Option)choice;
                if (enumChoice == Option.Exit)
                {
                    return;
                }

                Console.Clear();

                if (monitor)
                {
                    printer.WriteTest(e.Initialize());
                    printer.WriteTest(e.Enable());
                    printer.WriteTest(e.EnableAutomaticStatusBack());
                }
                Setup(monitor);

                printer?.WriteTest(e.PrintLine($"== [ Start {testCases[enumChoice]} ] =="));

                switch (enumChoice)
                {
                    case Option.SingleLinePrinting:
                        printer.WriteTest(Tests.SingleLinePrinting(e));
                        break;
                    case Option.MultiLinePrinting:
                        printer.WriteTest(Tests.MultiLinePrinting(e));
                        break;
                    case Option.LineSpacing:
                        printer.WriteTest(Tests.LineSpacing(e));
                        break;
                    case Option.BarcodeStyles:
                        printer.WriteTest(Tests.BarcodeStyles(e));
                        break;
                    case Option.BarcodeTypes:
                        printer.WriteTest(Tests.BarcodeTypes(e));
                        break;
                    case Option.TwoDimensionCodes:
                        printer.WriteTest(Tests.TwoDimensionCodes(e));
                        break;
                    case Option.TextStyles:
                        printer.WriteTest(Tests.TextStyles(e));
                        break;
                    case Option.FullReceipt:
                        printer.WriteTest(Tests.Receipt(e));
                        break;
                    case Option.Images:
                        printer.WriteTest(Tests.Images(e, false));
                        break;
                    case Option.LegacyImages:
                        printer.WriteTest(Tests.Images(e, true));
                        break;
                    case Option.LargeByteArrays:
                        try
                        {
                            printer.WriteTest(Tests.TestLargeByteArrays(e));
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine($"Aborting print due to test failure. Exception: {e?.Message}, Stack Trace: {e?.GetBaseException()?.StackTrace}");
                        }
                        break;
                    case Option.CashDrawerPin2:
                        printer.WriteTest(Tests.CashDrawerOpenPin2(e));
                        break;
                    case Option.CashDrawerPin5:
                        printer.WriteTest(Tests.CashDrawerOpenPin5(e));
                        break;
                    default:
                        Console.WriteLine("Invalid entry.");
                        break;
                }

                Setup(monitor);
                printer?.WriteTest(e.PrintLine($"== [ End {testCases[enumChoice]} ] =="));
                printer?.WriteTest(e.PartialCutAfterFeed(5));
                if (!SINGLETON_PRINTER_OBJECT)
                    printer?.Dispose();
                // TODO: also make an automatic runner that runs all tests (command line).
            }
        }

        public enum Option
        {
            SingleLinePrinting = 1,
            MultiLinePrinting,
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
            if (status == null) { Console.WriteLine("Status was null - unable to read status from printer."); return; }
            Console.WriteLine($"Printer Online Status: {status.IsPrinterOnline}");
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
                printer?.Write(e.Initialize());
                printer?.Write(e.Enable());
                if (enableStatusBackMonitoring)
                {
                    printer.Write(e.EnableAutomaticStatusBack());
                }
            }
        }
    }

    internal static class TestExtensions
    {
        /// <summary>
        /// Wrapper exception function for ease of switching between Write and WriteAsync function
        /// </summary>
        /// <param name="printer"></param>
        /// <param name="arrays"></param>
        internal static void WriteTest(this BasePrinter printer, params byte[][] arrays)
        {
            // Switch to use this if need to test with obsolated Write function
            //printer.Write(arrays);
            printer.WriteAsync(arrays).Wait();
        }
    }
}
