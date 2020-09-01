using System.Diagnostics.CodeAnalysis;

namespace ESCPOS_NET.Emitters.Enums
{
    [SuppressMessage("StyleCop.CSharp.SpacingRules", "SA1025:Code should not contain multiple whitespace in a row",
        Justification = "Enums are easier to read if they have whitespace alignment.")]
    public enum Align
    {
        Left = 0,
        Center = 1,
        Right = 2
    }
}