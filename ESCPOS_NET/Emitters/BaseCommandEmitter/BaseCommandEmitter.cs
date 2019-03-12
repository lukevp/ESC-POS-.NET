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
        public static readonly byte Alignment = 0x61;
    }
    public static class Whitespace
    {
        // TODO: tabs?
        public static readonly byte Linefeed = 0x0A;
        public static readonly byte FeedLines = 0x64;
        public static readonly byte FeedLinesReverse = 0x65;
        public static readonly byte FeedDots = 0x4A;
        public static readonly byte ResetLineSpacing = 0x32;
        public static readonly byte LineSpacingInDots = 0x33;
    }
    public static class Status
    {
        public static readonly byte AutomaticStatusBack = 0x61;
    }

    public static class Functions
    {
        public static readonly byte PaperCutFullCut = 0x00;
        public static readonly byte PaperCutFullCutWithFeed = 0x41;
        public static readonly byte PaperCutPartialCut = 0x01;
        public static readonly byte PaperCutPartialCutWithFeed = 0x42;
    }
    public static class Barcodes
    {
        public static readonly byte PrintBarcode = 0x6B;
        public static readonly byte SetBarcodeHeightInDots = 0x68;
        public static readonly byte SetBarWidth = 0x77;
        public static readonly byte SetBarLabelPosition = 0x48;
        public static readonly byte SetBarLabelFont = 0x66;
    }
}
