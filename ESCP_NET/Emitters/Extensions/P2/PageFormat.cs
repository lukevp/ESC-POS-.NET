namespace ESCP_NET.Emitters.Extensions.P2
{
    public abstract partial class ESCP2Extension
    {
        private int _definedUnit = 1;

        public byte[] SetPageLengthInDefinedUnit(int pageLength)
        {
            var toSplit = pageLength * (1 / (double) _definedUnit);
            var (low, high) = Util.SplitNumber(toSplit);
            return new byte[]
                {AsciiTable.ESC, AsciiTable.LEFT_PARENTHESIS, AsciiTable.C, 2, 0, (byte) low, (byte) high};
        }

        public byte[] SetPageFormat(int topMargin, int bottomMargin)
        {
            var t = topMargin * (1 / (double) _definedUnit);
            var b = bottomMargin * (1 / (double) _definedUnit);
            var (tlow, thigh) = Util.SplitNumber(t);
            var (blow, bhigh) = Util.SplitNumber(b);
            return new byte[]
            {
                AsciiTable.ESC, AsciiTable.LEFT_PARENTHESIS, AsciiTable.c, 4, 0, (byte) tlow, (byte) thigh, (byte) blow,
                (byte) bhigh
            };
        }
    }
}