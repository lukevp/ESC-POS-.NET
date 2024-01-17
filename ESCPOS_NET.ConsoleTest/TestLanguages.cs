using ESCPOS_NET.Emitters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ESCPOS_NET.ConsoleTest
{
    public static partial class Tests
    {
        public static byte[][] Languages(ICommandEmitter e)
        {
            List<byte[]> test = new List<byte[]>();

            // Icelandic code page.
            test.Add(e.CodePage(CodePage.PC861_ICELANDIC));
            test.Add(e.PrintLine("Þetta er próf þÞðÐýÝáúíóéÉÚÍæÆöÖ", 861));

            //TODO: Add more tests for other special code pages here.

            // Set CodePage back to default
            test.Add(e.CodePage(CodePage.PC437_USA_STANDARD_EUROPE_DEFAULT));
            test.Add(e.PrintLine());


            

            return test.ToArray();

        }
    }
}
