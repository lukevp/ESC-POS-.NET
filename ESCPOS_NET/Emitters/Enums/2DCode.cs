using System;
using System.Collections.Generic;
using System.Text;

namespace ESCPOS_NET.Emitters
{
    public enum TwoDimensionCodeType
    {
        PDF417 = 0,
        QRCODE_MODEL1 = 49,
        QRCODE_MODEL2,
        QRCODE_MICRO
    }

    public enum Size2DCode
    {
        TINY = 2,
        SMALL,
        NORMAL,
        LARGE,
        EXTRA
    }

    public enum CorrectionLevel2DCode
    {
        Percent7 = 48,
        Percent15,
        Percent25,
        Percent30
    }
}
