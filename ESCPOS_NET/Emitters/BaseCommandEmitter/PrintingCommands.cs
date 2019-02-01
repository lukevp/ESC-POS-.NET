using System.Collections.Generic;
using System.Linq;

namespace ESCPOS_NET.Emitters
{

    public abstract partial class BaseCommandEmitter : ICommandEmitter
    {
        /* Printing Commands */
        public byte[] PrintLines(string lines)
        {

            List<byte> outputBytes = new List<byte>();
            // Fix OSX or Windows-style newlines
            lines = lines.Replace("\r\n", "\n");
            lines = lines.Replace("\r", "\n");

            // TODO: Sanitize...

            return lines.ToCharArray().Select(x => (byte)x).ToArray();
        }

        public byte[] PrintLine(string line)
        {
            // TODO: Sanitize...
            return(line.Replace("\r", "").Replace("\n", "") + "\n").ToCharArray().Select(x => (byte)x).ToArray();
        }

        public byte[] Print(string line)
        {
            // TODO: Sanitize...
            return (line.Replace("\r", "").Replace("\n", "")).ToCharArray().Select(x => (byte)x).ToArray();
        }

        public byte[] FeedLines(int lineCount) => new byte[] { Cmd.ESC, Whitespace.FeedLines, (byte)lineCount };
        public byte[] FeedLinesReverse(int lineCount) => new byte[] { Cmd.ESC, Whitespace.FeedLinesReverse, (byte)lineCount };
        public byte[] FeedDots(int dotCount) => new byte[] { Cmd.ESC, Whitespace.FeedDots, (byte)dotCount };
    }
}
