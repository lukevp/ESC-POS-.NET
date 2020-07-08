namespace ESCPOS_NET.Emitters
{
    public enum TwoDimensionCodeType
    {
        PDF417 = 0,
        QRCODE_MODEL1 = 49,
        QRCODE_MODEL2,
        QRCODE_MICRO,
    }

    public enum Size2DCode
    {
        TINY = 2,
        SMALL,
        NORMAL,
        LARGE,
        EXTRA,
    }

    public enum CorrectionLevel2DCode
    {
        PERCENT_7 = 48,
        PERCENT_15,
        PERCENT_25,
        PERCENT_30,
    }
}
