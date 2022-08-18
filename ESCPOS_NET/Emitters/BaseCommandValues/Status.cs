namespace ESCPOS_NET.Emitters.BaseCommandValues
{
    public static class Status
    {
        public static readonly byte AutomaticStatusBack = 0x61;
        public static readonly byte AutomaticInkStatusBack = 0x6A;
        public static readonly byte RequestStatus = 0x72;
        public static readonly byte RealtimeStatus = 0x04;
    }
}
