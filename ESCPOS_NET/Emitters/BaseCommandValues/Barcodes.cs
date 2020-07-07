namespace ESCPOS_NET.Emitters.BaseCommandValues
{
    public static class Barcodes
    {
        public static readonly byte PrintBarcode = 0x6B;
        public static readonly byte SetBarcodeHeightInDots = 0x68;
        public static readonly byte SetBarWidth = 0x77;
        public static readonly byte SetBarLabelPosition = 0x48;
        public static readonly byte SetBarLabelFont = 0x66;

        public static readonly byte Set2DCode = 0x28;
        public static readonly byte AutoEnding = 0x00;
        public static readonly byte[] SetPDF417NumberOfColumns = { 0x03, 0x00, 0x30, 0x41 };
        public static readonly byte[] SetPDF417NumberOfRows = { 0x03, 0x00, 0x30, 0x42 };
        public static readonly byte[] SetPDF417DotSize = { 0x03, 0x00, 0x30, 0x43 };
        public static readonly byte[] SetPDF417CorrectionLevel = { 0x04, 0x00, 0x30, 0x45, 0x30 };
        public static readonly byte[] StorePDF417Data = { 0x00, 0x30, 0x50, 0x30 };
        public static readonly byte[] PrintPDF417 = { 0x03, 0x00, 0x30, 0x51, 0x30 };

        public static readonly byte[] SelectQRCodeModel = { 0x04, 0x00, 0x31, 0x41 };
        public static readonly byte[] SetQRCodeDotSize = { 0x03, 0x00, 0x31, 0x43 };
        public static readonly byte[] SetQRCodeCorrectionLevel = { 0x03, 0x00, 0x31, 0x45 };
        public static readonly byte[] StoreQRCodeData = { 0x31, 0x50, 0x30 };
        public static readonly byte[] PrintQRCode = { 0x03, 0x00, 0x31, 0x51, 0x30 };
    }
}
