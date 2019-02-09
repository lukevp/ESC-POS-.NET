using ESCPOS_NET.Emitters;
using System;
using UtilityClasses;

namespace ESCPOS_NET.ConsoleTest
{
    public static partial class Tests
    {
        public static byte[] LineSpacing(ICommandEmitter e) =>
            new ByteArrayBuilder()
                .Append(e.SetLineSpacingInDots(200))
                .Append(e.PrintLine("This is the default spacing."))
                .Append(e.SetLineSpacingInDots(15))
                .Append(e.PrintLine("This has 200 dots of spacing above."))
                .Append(e.ResetLineSpacing())
                .Append(e.PrintLine("This has 15 dots of spacing above."))
                .Append(e.PrintLine("This has the default spacing.")).ToArray();
    }
}
