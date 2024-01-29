using ESCPOS_NET.Emitters.BaseCommandValues;
using System;

namespace ESCPOS_NET.Emitters
{
    public abstract partial class BaseCommandEmitter : ICommandEmitter
    {
        public int Columns { get; set; } = 42;

        /* Character Commands */
        public virtual byte[] SetStyles(PrintStyle style) => new byte[] { Cmd.ESC, Chars.StyleMode, (byte)style };

        public virtual byte[] LeftAlign() => new byte[] { Cmd.ESC, Chars.Alignment, (byte)Align.Left };

        public virtual byte[] CenterAlign() => new byte[] { Cmd.ESC, Chars.Alignment, (byte)Align.Center };

        public virtual byte[] RightAlign() => new byte[] { Cmd.ESC, Chars.Alignment, (byte)Align.Right };

        public virtual byte[] LeftAlignAlt() => new byte[] { Cmd.ESC, Chars.Alignment, (byte)Align.LeftAlt };

        public virtual byte[] CenterAlignAlt() => new byte[] { Cmd.ESC, Chars.Alignment, (byte)Align.CenterAlt };

        public virtual byte[] RightAlignAlt() => new byte[] { Cmd.ESC, Chars.Alignment, (byte)Align.RightAlt };

        public virtual byte[] RightCharacterSpacing(int spaceCount) => new byte[] { Cmd.ESC, Chars.RightCharacterSpacing, (byte)spaceCount };

        public virtual byte[] CodePage(CodePage codePage) => new byte[] { Cmd.ESC, Chars.CodePage, (byte)codePage };

        public virtual byte[] Color(Color color) => new byte[] { Cmd.ESC, Chars.Color, (byte)color };

        public virtual byte[] HorizontalLine() => Print("".PadLeft(Columns, '-'));

        public virtual byte[] HorizontalLine(char c) => Print("".PadLeft(Columns, c));

        /// <summary>
        /// Based on printer's column count provides a byte array for 1 full line where both texts fit, first one aligned to the left and the other to the right.
        /// If total length of both texts is greater than printer's column count, Left Aligned text will get truncated to fit both and a whitespace will be added in between
        /// </summary>
        /// <param name="leftAlignedText">Text that will be aligned to the Left</param>
        /// <param name="rightAlignedText">Text that will be aligned to the Right</param>
        /// <returns>Byte array that contains a full line with one text aligned to the left and the other to the right.</returns>
        /// <exception cref="ArgumentException"><paramref name="rightAlignedText"/> length is greater than column count.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="leftAlignedText"/> or <paramref name="rightAlignedText"/> is null.</exception>
        public virtual byte[] SameLineLeftAndRightAlign(string leftAlignedText, string rightAlignedText)
        {
            if (leftAlignedText is null)
            {
                throw new ArgumentNullException(nameof(leftAlignedText));
            }

            if (rightAlignedText is null)
            {
                throw new ArgumentNullException(nameof(rightAlignedText));
            }

            int totalLength = leftAlignedText.Length + rightAlignedText.Length;
            if (rightAlignedText.Length > Columns)
            {
                throw new ArgumentException($"Length of Right-Aligned text ({rightAlignedText.Length}) surpass the printer's column count ({Columns}).");
            }

            if (rightAlignedText.Length >= Columns - 1)
            {
                if (rightAlignedText.Length < Columns)
                {
                    rightAlignedText = $" {rightAlignedText}";
                }
                return Print(rightAlignedText);
            }

            string result;
            if (totalLength >= Columns)
            {
                result = $"{leftAlignedText.Substring(0, Columns - (rightAlignedText.Length + 1))} {rightAlignedText}";
            }
            else
            {
                int padCount = Columns - totalLength;
                result = $"{leftAlignedText}{rightAlignedText.PadLeft(padCount + rightAlignedText.Length, ' ')}";
            }
            return Print(result);
        }
    }
}
