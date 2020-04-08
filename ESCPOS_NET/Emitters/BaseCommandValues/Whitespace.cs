namespace ESCPOS_NET.Emitters.BaseCommandValues
{
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
}
