using ESCPOS_NET.Emitters;

namespace ESCPOS_NET.ConsoleTest
{
    public static partial class Tests
    {
        public static byte[][] LineSpacing(ICommandEmitter e) => new byte[][] {
            e.SetLineSpacingInDots(200),
            e.PrintLine("This is the default spacing."),
            e.SetLineSpacingInDots(15),
            e.PrintLine("This has 200 dots of spacing above."),
            e.ResetLineSpacing(),
            e.PrintLine("This has 15 dots of spacing above."),
            e.PrintLine("This has the default spacing.")
        };
    }
}
