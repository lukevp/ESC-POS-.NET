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
        public static readonly byte ReversePrintMode = 0x42;
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
        public static readonly byte AutomaticInkStatusBack = 0x6A;
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
    public static class Images
    {
        public static readonly byte ImageCmdParen = 0x28;
        public static readonly byte ImageCmd8 = 0x38;
        public static readonly byte ImageCmdL = 0x4C;
    }

    public static class QrCode
    {
        public static readonly byte QrCodeCmd = 0x28; // l
        public static readonly byte SetQrCodeModel = 0x41; // Function 165
        public static readonly byte SetQrCodeSize = 0x43; // Function 167
        public static readonly byte SetQrCodeErrorLevel = 0x45; // Function 169
        public static readonly byte QrCodeStoreData = 0x50; // Function 180
        public static readonly byte QrCodeTransmiteSize = 0x52; // Function 182
    }
}
