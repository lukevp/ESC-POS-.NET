namespace ESCPOS_NET.Emitters
{
    public partial class CustomCommandValues
    {
        /// <summary>
        /// CUSTOM implements the Store QR Code Data slightly differently than the base implementation
        /// </summary>
        public override byte[] StoreQRCodeData => new byte[] { 0x31, 0x50, 0x31 };

        /// <summary>
        /// CUSTOM implements the Print QR Code slightly differently than the base implementation
        /// </summary>
        public override byte[] PrintQRCode => new byte[] { 0x03, 0x00, 0x31, 0x51, 0x31 };
    }
}
