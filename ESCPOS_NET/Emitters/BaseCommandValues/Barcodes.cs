namespace ESCPOS_NET.Emitters.BaseCommandValues
{
    public static class Barcodes
    {
        public static readonly byte PrintBarcode = 0x6B;
        public static readonly byte SetBarcodeHeightInDots = 0x68;
        public static readonly byte SetBarWidth = 0x77;
        public static readonly byte SetBarLabelPosition = 0x48;
        public static readonly byte SetBarLabelFont = 0x66;
    }
}
