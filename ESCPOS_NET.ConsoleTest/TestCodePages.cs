using ESCPOS_NET.Emitters;
using System.Collections.Generic;

namespace ESCPOS_NET.ConsoleTest
{
    public static partial class Tests
    {
        public static byte[][] CodePages(ICommandEmitter e, CodePage codePage) {
            List<byte[]> test = new List<byte[]>();
            test.Add(e.LeftAlign());
            test.Add(e.PrintLine("Empty space = space or non-printable character."));
            test.Add(e.PrintLine("Each row represents the first hex digit."));
            test.Add(e.PrintLine("Each column represents the second hex digit."));
            test.Add(e.PrintLine("For example, row 7, column A, represents"));
            test.Add(e.PrintLine("the character with hex 0x7A."));
            test.Add(e.PrintLine());
            test.Add(e.PrintLine("Character Table for Code Page:"));
            test.Add(e.PrintLine(codePage + "  (Page " + ((int)codePage) + ")"));
            test.Add(e.PrintLine("=========================="));
            test.Add(e.PrintLine());
            test.Add(e.PrintLine("   0 1 2 3 4 5 6 7 8 9 A B C D E F "));
            // Set CodePage to test codepage.
            test.Add(e.CodePage(codePage));
            for (int d1 = 0; d1 < 0x10; d1++)
            {

                var upperDigit = d1.ToString("x1").ToUpperInvariant();
                test.Add(e.Print(upperDigit + "  "));
                for (int d2 = 0; d2 < 0x10; d2++)
                {
                    var digit = d1 * 0x10 + d2;
                    if (digit <= 0x20) digit = 0x20;
                    test.Add(e.Print((char)digit + " "));
                }
                test.Add(e.PrintLine());
            }
            // Set CodePage back to default
            test.Add(e.CodePage(CodePage.PC437_USA_STANDARD_EUROPE_DEFAULT));
            test.Add(e.PrintLine());
            return test.ToArray();
        }
    }
}
