namespace ESCPOS_NET.Emitters.CustomCommandValues
{
    public static class Barcodes
    {
        /// <summary>
        /// CUSTOM implements the Store QR Code Data slightly differently than the base implementation
        /// </summary>
        public static readonly byte[] StoreQRCodeData = { 0x31, 0x50, 0x31 };

        /// <summary>
        /// CUSTOM implements the Print QR Code slightly differently than the base implementation
        /// </summary>
        public static readonly byte[] PrintQRCode = { 0x03, 0x00, 0x31, 0x51, 0x31 };
    }
}
