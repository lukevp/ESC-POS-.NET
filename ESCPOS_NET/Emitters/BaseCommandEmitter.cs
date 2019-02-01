using System.Collections.Generic;
using System.Linq;

namespace ESCPOS_NET.Emitters
{
    public static class Cmd
    {
        public static readonly byte ESC = 0x1B;
        public static readonly byte GS = 0x1D;
    }
    public static class Ops
    {
        public static readonly byte Initialize = 0x40;
        public static readonly byte EnableDisable = 0x3D;
        public static readonly byte PaperCut = 0x56;
    }
    public static class Chars
    {
        public static readonly byte StyleMode = 0x21;
    }
    public static class Whitespace
    {
        // TODO: tabs?
        public static readonly byte Linefeed = 0x0A;
        public static readonly byte FeedLines = 0x64;
        public static readonly byte FeedLinesReverse = 0x65;
        public static readonly byte FeedDots = 0x4A;
    }

    public static class Functions
    {
        public static readonly byte PaperCutFullCut = 0x00;
        public static readonly byte PaperCutFullCutWithFeed = 0x41;
        public static readonly byte PaperCutPartialCut = 0x01;
        public static readonly byte PaperCutPartialCutWithFeed = 0x42;
    }

    public interface ICommandEmitter
    {
        /* Operational Commands */
        byte[] Initialize { get; }
        byte[] Enable { get; }
        byte[] Disable { get; }

        /* Character Commands */
        byte[] SetStyles(PrintStyle style);

        /* Action Commands */
        byte[] FullCut { get; }
        byte[] PartialCut { get; }
        byte[] FullCutAfterFeed(int lineCount);
        byte[] PartialCutAfterFeed(int lineCount);

        /* Printing Commands */
        byte[] PrintLines(string lines);
        byte[] PrintLine(string line);
        byte[] FeedLines(int lineCount);
        byte[] FeedLinesReverse(int lineCount);
        byte[] FeedDots(int dotCount);

        /* Image Commands */
        /* Status Commands */
        /* Barcode Commands */
    }

    public abstract class BaseCommandEmitter : ICommandEmitter
    {
        /* Operational Commands */
        public byte[] Initialize => new byte[] { Cmd.ESC, Ops.Initialize };
        public byte[] Enable => new byte[] { Cmd.ESC, Ops.EnableDisable, 1 };
        public byte[] Disable => new byte[] { Cmd.ESC, Ops.EnableDisable, 0 };

        /* Character Commands */
        public byte[] SetStyles(PrintStyle style)
        {
            return new byte[] { Cmd.ESC, Chars.StyleMode, (byte)style };
        }


        /* Action Commands */
        public byte[] FullCut => new byte[] { Cmd.GS, Ops.PaperCut, Functions.PaperCutFullCut };
        public byte[] PartialCut => new byte[] { Cmd.GS, Ops.PaperCut, Functions.PaperCutPartialCut };
        public byte[] FullCutAfterFeed(int lineCount)
        {
            return new byte[] { Cmd.GS, Ops.PaperCut, Functions.PaperCutFullCutWithFeed, (byte)lineCount };
        }
        public byte[] PartialCutAfterFeed(int lineCount)
        {
            return new byte[] { Cmd.GS, Ops.PaperCut, Functions.PaperCutPartialCutWithFeed, (byte)lineCount };
        }

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
            return(line.TrimEnd() + "\n").ToCharArray().Select(x => (byte)x).ToArray();
        }

        public byte[] FeedLines(int lineCount)
        {
            return new byte[] { Cmd.ESC, Whitespace.FeedLines, (byte)lineCount };
        }

        public byte[] FeedLinesReverse(int lineCount)
        {
            return new byte[] { Cmd.ESC, Whitespace.FeedLinesReverse, (byte)lineCount };
        }

        public byte[] FeedDots(int dotCount)
        {
            return new byte[] { Cmd.ESC, Whitespace.FeedDots, (byte)dotCount };
        }

        /* Image Commands */
        /* Status Commands */
        /* Barcode Commands */

    }
}
