namespace ESCPOS_NET.Emitters
{
    public partial class BaseCommandValues : ICommandValues
    {
        // TODO: tabs?
        public virtual byte Linefeed => 0x0A;
        public virtual byte FeedLines => 0x64;
        public virtual byte FeedLinesReverse => 0x65;
        public virtual byte FeedDots => 0x4A;
        public virtual byte ResetLineSpacing => 0x32;
        public virtual byte LineSpacingInDots => 0x33;
    }
}
