using System;

namespace ESCPOS_NET.Emitters
{
    [Flags]
    public enum PrintStyle
    {
        None         = 0,
        FontB        = 1,
        Bold         = 1 << 3,
        DoubleHeight = 1 << 4,
        DoubleWidth  = 1 << 5,
        Underline    = 1 << 7
    }
}
