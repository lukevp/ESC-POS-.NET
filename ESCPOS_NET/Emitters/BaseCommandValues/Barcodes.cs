namespace ESCPOS_NET.Emitters
{
    public partial class BaseCommandValues : ICommandValues
    {
        public virtual byte PrintBarcode =>  0x6B;
        public virtual byte SetBarcodeHeightInDots => 0x68;
        public virtual byte SetBarWidth => 0x77;
        public virtual byte SetBarLabelPosition => 0x48;
        public virtual byte SetBarLabelFont => 0x66;

        public virtual byte Set2DCode => 0x28;
        public virtual byte AutoEnding => 0x00;
        public virtual byte[] SetPDF417NumberOfColumns => new byte[] { 0x03, 0x00, 0x30, 0x41 };
        public virtual byte[] SetPDF417NumberOfRows => new byte[] { 0x03, 0x00, 0x30, 0x42 };
        public virtual byte[] SetPDF417DotSize => new byte[] { 0x03, 0x00, 0x30, 0x43 };
        public virtual byte[] SetPDF417CorrectionLevel => new byte[] { 0x04, 0x00, 0x30, 0x45, 0x30 };
        public virtual byte[] StorePDF417Data => new byte[] { 0x00, 0x30, 0x50, 0x30 };
        public virtual byte[] PrintPDF417 => new byte[] { 0x03, 0x00, 0x30, 0x51, 0x30 };

        public virtual byte[] SelectQRCodeModel => new byte[] { 0x04, 0x00, 0x31, 0x41 };
        public virtual byte[] SetQRCodeDotSize => new byte[] { 0x03, 0x00, 0x31, 0x43 };
        public virtual byte[] SetQRCodeCorrectionLevel => new byte[] { 0x03, 0x00, 0x31, 0x45 };
        public virtual byte[] StoreQRCodeData => new byte[] { 0x31, 0x50, 0x30 };
        public virtual byte[] PrintQRCode => new byte[] { 0x03, 0x00, 0x31, 0x51, 0x30 };
    }
}
