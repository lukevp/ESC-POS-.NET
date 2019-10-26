namespace ESCPOS_NET.Emitters
{
    public enum BarcodeType // All from Function B
    {
        UPC_A                       = 0x41,
        UPC_E                       = 0x42,
        JAN13_EAN13                 = 0x43,
        JAN8_EAN8                   = 0x44,
        CODE39                      = 0x45,
        ITF                         = 0x46,
        CODABAR_NW_7                = 0x47,
        CODE93                      = 0x48,
        CODE128                     = 0x49,
        GS1_128                     = 0x4A,
        GS1_DATABAR_OMNIDIRECTIONAL = 0x4B,
        GS1_DATABAR_TRUNCATED       = 0x4C,
        GS1_DATABAR_LIMITED         = 0x4D,
        GS1_DATABAR_EXPANDED        = 0x4E
    }
}
