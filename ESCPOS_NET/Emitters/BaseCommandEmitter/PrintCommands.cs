using System.Linq;
using ESCPOS_NET.Emitters.BaseCommandValues;

namespace ESCPOS_NET.Emitters.BaseCommandEmitter
{
    public abstract partial class BaseCommandEmitter : ICommandEmitter
    {
        /* Printing Commands */
        public byte[] Print(string data)
        {
            // Fix OSX or Windows-style newlines
            data = data.Replace("\r\n", "\n");
            data = data.Replace("\r", "\n");

            // TODO: Sanitize...
            return data.ToCharArray().Select(x => (byte) x).ToArray();
        }

        public byte[] PrintLine(string line)
        {
            if (line == null) return Print("\n");

            return Print(line.Replace("\r", string.Empty).Replace("\n", string.Empty) + "\n");
        }

        public byte[] FeedLines(int lineCount)
        {
            return new[] {Cmd.ESC, Whitespace.FeedLines, (byte) lineCount};
        }

        public byte[] FeedLinesReverse(int lineCount)
        {
            return new[] {Cmd.ESC, Whitespace.FeedLinesReverse, (byte) lineCount};
        }

        public byte[] FeedDots(int dotCount)
        {
            return new[] {Cmd.ESC, Whitespace.FeedDots, (byte) dotCount};
        }

        public byte[] ReverseMode(bool enable)
        {
            return new[] {Cmd.GS, Chars.ReversePrintMode, enable ? (byte) 0x01 : (byte) 0x00};
        }

        public byte[] UpsideDownMode(bool enable)
        {
            return new[] {Cmd.ESC, Chars.UpsideDownMode, enable ? (byte) 0x01 : (byte) 0x00};
        }
    }
}