using System;
using System.Diagnostics.CodeAnalysis;

namespace ESCPOS_NET.Emitters.Enums
{
    [Flags]
    [SuppressMessage("StyleCop.CSharp.SpacingRules", "SA1025:Code should not contain multiple whitespace in a row",
        Justification = "Enums are easier to read if they have whitespace alignment.")]
    public enum PrintStyle
    {
        None = 0,
        FontB = 1,
        Bold = 1 << 3,
        DoubleHeight = 1 << 4,
        DoubleWidth = 1 << 5,
        Underline = 1 << 7
    }
}