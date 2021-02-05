using System.Linq;

namespace ESCPOS_NET.Emitters
{
    public abstract partial class BaseCommandEmitter<TCommandValues> : ICommandEmitter
    {
        /* Printing Commands */
        public virtual byte[] Print(string data)
        {
            // Fix OSX or Windows-style newlines
            data = data.Replace("\r\n", "\n");
            data = data.Replace("\r", "\n");

            // TODO: Sanitize...
            return data.ToCharArray().Select(x => (byte)x).ToArray();
        }

        public virtual byte[] PrintLine(string line)
        {
            if (line == null)
            {
                return Print("\n");
            }

            return Print(line.Replace("\r", string.Empty).Replace("\n", string.Empty) + "\n");
        }

        public virtual byte[] FeedLines(int lineCount) => new byte[] { Values.ESC, Values.FeedLines, (byte)lineCount };

        public virtual byte[] FeedLinesReverse(int lineCount) => new byte[] { Values.ESC, Values.FeedLinesReverse, (byte)lineCount };

        public virtual byte[] FeedDots(int dotCount) => new byte[] { Values.ESC, Values.FeedDots, (byte)dotCount };

        public virtual byte[] ReverseMode(bool enable) => new byte[] { Values.GS, Values.ReversePrintMode, enable ? (byte)0x01 : (byte)0x00 };

        public virtual byte[] UpsideDownMode(bool enable) => new byte[] { Values.ESC, Values.UpsideDownMode, enable ? (byte)0x01 : (byte)0x00 };
    }
}
