using ESCPOS_NET.Emitters;

namespace ESCPOS_NET.ConsoleTest
{

    public static partial class Tests
    {
        public static byte[][] TextStyles(ICommandEmitter e) => new byte[][] {
            e.SetStyles(PrintStyle.None),
            e.Print("Default: The quick brown fox jumped over the lazy dogs.\n"),
            e.SetStyles(PrintStyle.FontB),
            e.Print("Font B: The quick brown fox jumped over the lazy dogs.\n"),
            e.SetStyles(PrintStyle.Bold),
            e.Print("Bold: The quick brown fox jumped over the lazy dogs.\n"),
            e.SetStyles(PrintStyle.Underline),
            e.Print("Underline: The quick brown fox jumped over the lazy dogs.\n"),
            e.SetStyles(PrintStyle.DoubleWidth),
            e.Print("DoubleWidth: The quick brown fox jumped over the lazy dogs.\n"),
            e.SetStyles(PrintStyle.DoubleHeight),
            e.Print("DoubleHeight: The quick brown fox jumped over the lazy dogs.\n"),
            e.SetStyles(PrintStyle.FontB | PrintStyle.DoubleHeight | PrintStyle.DoubleWidth | PrintStyle.Underline | PrintStyle.Bold),
            e.Print("All Styles: The quick brown fox jumped over the lazy dogs.\n"),
            e.SetStyles(PrintStyle.None),
            e.ReverseMode(true),
            e.PrintLine("REVERSE MODE: The quick brown fox jumped over the lazy dogs."),
            e.SetStyles(PrintStyle.FontB | PrintStyle.DoubleHeight | PrintStyle.DoubleWidth),
            e.PrintLine("REVERSE MODE: The quick brown fox jumped over the lazy dogs."),
            e.SetStyles(PrintStyle.None),
                e.ReverseMode(false),
            e.SetStyles(PrintStyle.None),
            e.RightCharacterSpacing(5),
            e.PrintLine("Right space 5: The quick brown fox jumped over the lazy dogs."),
            e.RightCharacterSpacing(0),
            e.SetStyles(PrintStyle.None),
            e.UpsideDownMode(true),
            e.PrintLine("Upside Down Mode: The quick brown fox jumped over the lazy dogs."),
            e.UpsideDownMode(false)
        };
    }
}
