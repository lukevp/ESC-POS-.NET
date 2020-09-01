using System;

namespace ESCP_NET.Emitters.BaseCommandEmitter
{
    public abstract partial class BaseCommandEmitter
    {
        public byte[] SetPageLengthInLines(int lines)
        {
            if (lines < 1 || lines > 127) throw new ArgumentOutOfRangeException("Expected 1 <= n <= 127");
            return new[] {AsciiTable.ESC, AsciiTable.C, (byte) lines};
        }

        public byte[] SetPageLengthInInches(int inches)
        {
            if (inches < 1 || inches > 22) throw new ArgumentOutOfRangeException("Expected 1 <= n <= 22");
            return new[] {AsciiTable.ESC, AsciiTable.C, AsciiTable.NUL, (byte) inches};
        }

        public byte[] SetBottomMargin(int margin)
        {
            if (margin < 1 || margin > 127) throw new ArgumentOutOfRangeException("Expected 1 <= n <= 127");
            return new[] {AsciiTable.ESC, AsciiTable.N, (byte) margin};
        }

        public byte[] CancelBottomMargin()
        {
            return new[] {AsciiTable.ESC, AsciiTable.O};
        }

        public byte[] SetRightMargin(int margin)
        {
            if (margin < 1 || margin > 255) throw new ArgumentOutOfRangeException("Expected 1 <= n <= 255");
            return new[] {AsciiTable.ESC, AsciiTable.Q, (byte) margin};
        }

        public byte[] SetLeftMargin(int margin)
        {
            if (margin < 1 || margin > 255) throw new ArgumentOutOfRangeException("Expected 1 <= n <= 255");
            return new[] {AsciiTable.ESC, AsciiTable.l, (byte) margin};
        }
    }
}