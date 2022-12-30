using ESCPOS_NET.Emitters;

namespace ESCPOS_NET.ConsoleTest
{
    public static partial class Tests
    {
        public static byte[][] Position(ICommandEmitter e) => new byte[][] {
            e.SetLeftMargin(0),
            e.PrintLine("Left Margin: This is 0 left margin."),
            e.SetLeftMargin(10),
            e.PrintLine("Left Margin: This is 10 left margin."),
            e.SetLeftMargin(20),
            e.PrintLine("Left Margin: This is 20 left margin."),
            e.SetLeftMargin(30),
            e.PrintLine("Left Margin: This is 30 left margin."),
            e.SetLeftMargin(0),
            e.PrintLine("Left Margin: This is 0 left margin."),
        };
    }
}
