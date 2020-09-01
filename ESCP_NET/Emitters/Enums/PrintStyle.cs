using System.Diagnostics.CodeAnalysis;

namespace ESCP_NET.Emitters.Enums
{
    [SuppressMessage("StyleCop.CSharp.SpacingRules", "SA1025:Code should not contain multiple whitespace in a row",
        Justification = "Enums are easier to read if they have whitespace alignment.")]
    public enum PrintStyle
    {
        None = 0,
        FontB = 1,
        Proportional = 2,
        Condensed = 4,
        Bold = 8,
        DoubleHeight = 16,
        DoubleWidth = 32,
        Underline = 128
    }
}