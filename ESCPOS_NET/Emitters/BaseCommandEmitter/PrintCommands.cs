using System.Linq;
using System.Text;
using ESCPOS_NET.Emitters.BaseCommandValues;

namespace ESCPOS_NET.Emitters
{
    public abstract partial class BaseCommandEmitter : ICommandEmitter
    {
        private static bool encodingProviderInitialized = false;

        /* Printing Commands */
        public virtual byte[] Print(string data)
        {
            // Fix OSX or Windows-style newlines
            data = data.Replace("\r\n", "\n");
            data = data.Replace("\r", "\n");

            // TODO: Sanitize...
            return data.ToCharArray().Select(x => (byte)x).ToArray();
        }

        /// <summary>
        /// Prints the string, converting the bytes to given code page. Note that Printer needs to be set to that same
        /// code page also.
        /// </summary>
        /// <param name="data">Data to print</param>
        /// <param name="codepage">Codepage number which the string will be converted to.</param>
        /// <returns></returns>
        public virtual byte[] Print(string data, int codepage)
        {
            // Fix OSX or Windows-style newlines
            data = data.Replace("\r\n", "\n");
            data = data.Replace("\r", "\n");

            if (!encodingProviderInitialized)
            {
                Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
                encodingProviderInitialized = true;
            }

            return System.Text.Encoding.GetEncoding(codepage).GetBytes(data);
        }

        public virtual byte[] PrintLine(string line)
        {
            if (line == null)
            {
                return Print("\n");
            }

            return Print(line.Replace("\r", string.Empty).Replace("\n", string.Empty) + "\n");
        }

        /// <summary>
        /// Prints line, converting the bytes to given code page. Note that Printer needs to be set to that same
        /// code page also.
        /// </summary>
        /// <param name="line">Line to print</param>
        /// <param name="codepage">Codepage number which the string will be converted to.</param>
        /// <returns></returns>
        public virtual byte[] PrintLine(string line, int codepage)
        {
            if (line == null)
            {
                return Print("\n");
            }

            return Print(line.Replace("\r", string.Empty).Replace("\n", string.Empty) + "\n", codepage);
        }

        public virtual byte[] FeedLines(int lineCount) => new byte[] { Cmd.ESC, Whitespace.FeedLines, (byte)lineCount };

        public virtual byte[] FeedLinesReverse(int lineCount) => new byte[] { Cmd.ESC, Whitespace.FeedLinesReverse, (byte)lineCount };

        public virtual byte[] FeedDots(int dotCount) => new byte[] { Cmd.ESC, Whitespace.FeedDots, (byte)dotCount };

        public virtual byte[] ReverseMode(bool enable) => new byte[] { Cmd.GS, Chars.ReversePrintMode, enable ? (byte)0x01 : (byte)0x00 };

        public virtual byte[] UpsideDownMode(bool enable) => new byte[] { Cmd.ESC, Chars.UpsideDownMode, enable ? (byte)0x01 : (byte)0x00 };
    }
}
