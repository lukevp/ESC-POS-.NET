using ESCPOS_NET.Emitters;
using ESCPOS_NET.Utilities;

namespace ESCPOS_NET.ConsoleTest
{
    public static partial class Tests
    {
        public static byte[][] Printing(ICommandEmitter e) => new byte[][] {
            e.Print("Multiline Test: Windows...\r\nOSX...\rUnix...\n"),
            //TODO: sanitize test.
            e.PrintLine("Feeding 250 dots."),
            e.FeedDots(250),
            e.PrintLine("Feeding 3 lines."),
            e.FeedLines(3),
            e.PrintLine("Done Feeding."),
            e.PrintLine("Reverse Feeding 6 lines."),
            e.FeedLinesReverse(6),
            e.PrintLine("Done Reverse Feeding.")
        };
    }
}
