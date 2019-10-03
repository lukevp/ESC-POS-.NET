using System.Collections.Generic;
using System.Linq;

namespace ESCPOS_NET.Emitters
{

    public abstract partial class BaseCommandEmitter : ICommandEmitter
    {
        /* Printing Commands */
        public byte[] Print(string data)
        {
            List<byte> outputBytes = new List<byte>();
            // Fix OSX or Windows-style newlines
            data = data.Replace("\r\n", "\n");
            data = data.Replace("\r", "\n");

            // TODO: Sanitize...
            return data.ToCharArray().Select(x => (byte)x).ToArray();
        }

        public byte[] PrintLine(string line)
        {
            if (line == null)
            {
                return Print("\n");
            }
            return Print(line.Replace("\r", "").Replace("\n", "") + "\n");
        }

        public byte[] FeedLines(int lineCount) => new byte[] { Cmd.ESC, Whitespace.FeedLines, (byte)lineCount };
        public byte[] FeedLinesReverse(int lineCount) => new byte[] { Cmd.ESC, Whitespace.FeedLinesReverse, (byte)lineCount };
        public byte[] FeedDots(int dotCount) => new byte[] { Cmd.ESC, Whitespace.FeedDots, (byte)dotCount };
        public byte[] ReverseMode(bool enable) => new byte[] { Cmd.GS, Chars.ReversePrintMode, enable ? (byte)0x01 : (byte)0x00 };
        public byte[] UpsideDownMode(bool enable) => new byte[] { Cmd.ESC, Chars.UpsideDownMode, enable ? (byte)0x01 : (byte)0x00 };
    }
}
