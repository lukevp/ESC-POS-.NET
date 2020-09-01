namespace ESCP_NET.Emitters.BaseCommandEmitter
{
    public abstract partial class BaseCommandEmitter
    {
        public byte[] CarriageReturn()
        {
            return new[] {AsciiTable.CR};
        }

        public byte[] LineFeed()
        {
            return new[] {AsciiTable.LF};
        }

        public byte[] FormFeed()
        {
            return new[] {AsciiTable.FF};
        }

        public byte[] SetAbsoluteHorizontalPrintPosition(int position)
        {
            var (low, high) = Util.SplitNumber(position);

            return new[] {AsciiTable.ESC, AsciiTable.DOLLAR, (byte) low, (byte) high};
        }

        public byte[] SetRelativeHorizontalPrintPosition(int position)
        {
            var (low, high) = Util.SplitNumber(position);

            return new[] {AsciiTable.ESC, AsciiTable.SLASH, (byte) low, (byte) high};
        }

        public byte[] AdvancePrintPositionVertically(byte position)
        {
            return new[] {AsciiTable.ESC, AsciiTable.J, position};
        }

        public byte[] TabHorizontally()
        {
            return new[] {AsciiTable.TAB};
        }

        public byte[] TabVertically()
        {
            return new[] {AsciiTable.VT};
        }

        public byte[] Backspace()
        {
            return new[] {AsciiTable.BS};
        }
    }
}