namespace ESCPOS_NET.FiscalEmitters.BaseCommandValues
{
    public static class Cmd
    {
        public static readonly byte Preamble = 0x01;
        public static readonly byte Postamble = 0x05;
        public static readonly byte Terminator = 0x03;
        public static readonly byte[] Empty = new byte[] { };

        public static readonly byte Tab = 0x09;
        public static readonly byte Times = 0x2A;
    }
}
