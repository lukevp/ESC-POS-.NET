namespace ESCPOS_NET.Emitters.BaseCommandValues
{
    public static class Ops
    {
        public static readonly byte Initialize = 0x40;
        public static readonly byte EnableDisable = 0x3D;
        public static readonly byte PaperCut = 0x56;
        public static readonly byte CashDrawerPulse = 0x70;
        public static readonly byte StandardMode = 0x1B;
    }
}
