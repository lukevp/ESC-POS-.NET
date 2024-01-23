using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using ESCPOS_NET.Emitters.BaseCommandValues;

namespace ESCPOS_NET.Emitters
{
    public abstract partial class BaseCommandEmitter : ICommandEmitter
    {
        public Encoding Encoding { get; set; }

        /* Printing Commands */
        public virtual byte[] Print(string data)
        {
            // Fix OSX or Windows-style newlines
            data = Regex.Replace(data, @"\r\n|\r", "\n");

            if (Encoding is null)
            {
                // TODO: Sanitize...
                return data.ToCharArray().Select(x => (byte)x).ToArray();
            }

            return Encoding.GetBytes(data);
        }

        public virtual byte[] PrintLine(string line)
        {
            if (line == null)
            {
                return Print("\n");
            }

            return Print(line.Replace("\r", string.Empty).Replace("\n", string.Empty) + "\n");
        }

        public virtual byte[] FeedLines(int lineCount) => new byte[] { Cmd.ESC, Whitespace.FeedLines, (byte)lineCount };

        public virtual byte[] FeedLinesReverse(int lineCount) => new byte[] { Cmd.ESC, Whitespace.FeedLinesReverse, (byte)lineCount };

        public virtual byte[] FeedDots(int dotCount) => new byte[] { Cmd.ESC, Whitespace.FeedDots, (byte)dotCount };

        public virtual byte[] ReverseMode(bool enable) => new byte[] { Cmd.GS, Chars.ReversePrintMode, enable ? (byte)0x01 : (byte)0x00 };

        public virtual byte[] UpsideDownMode(bool enable) => new byte[] { Cmd.ESC, Chars.UpsideDownMode, enable ? (byte)0x01 : (byte)0x00 };
    }
}
