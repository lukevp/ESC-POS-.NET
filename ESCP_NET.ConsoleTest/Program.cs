using System;
using ESC_NET;
using ESC_NET.Printers;
using ESCP_NET.Emitters.Enums;
using ESCP_NET.Emitters.Extensions.Enums;
using ESCPOS_NET;

namespace ESCP_NET.ConsoleTest
{
    class Program
    {
        static void Main(string[] args)
        {
            FilePrinter printer = new FilePrinter("/dev/usb/lp0");
            var p = new ESCP();
            
            printer.Write(ByteSplicer.Combine(
                p.PrintLine("Hello"),
                p.SetPrintStyle(PrintStyle.Bold),
                p.Beep(),
                p.PrintLine("sadasd"),
                p.SelectJustification(Justification.Center),
                p.SetPrintStyle(PrintStyle.None),
                p.Select15Cpi(),
                p.SelectPrintQuality(PrintQuality.LQ_NLQ),
                p.PrintLine("The end"),
                p.SelectJustification(Justification.Left)
            ));
            Console.ReadKey();
        }
    }
}