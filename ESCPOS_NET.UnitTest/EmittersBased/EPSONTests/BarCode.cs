using ESCPOS_NET.Emitters;
using ESCPOS_NET.Utilities;
using Xunit;

namespace ESCPOS_NET.UnitTest.EmittersBased.EPSONTests
{
    public class BarCode
    {
        private const string WEBSITE_STRING = "https://github.com/lukevp/ESC-POS-.NET/";
        private const string SHORTER_WEBSITE_STRING = "https://github.com/";

        [Theory]
        [InlineData(TwoDimensionCodeType.QRCODE_MODEL1, Size2DCode.EXTRA, CorrectionLevel2DCode.PERCENT_15)]
        [InlineData(TwoDimensionCodeType.QRCODE_MODEL1, Size2DCode.LARGE, CorrectionLevel2DCode.PERCENT_15)]
        [InlineData(TwoDimensionCodeType.QRCODE_MODEL1, Size2DCode.NORMAL, CorrectionLevel2DCode.PERCENT_15)]
        [InlineData(TwoDimensionCodeType.QRCODE_MODEL1, Size2DCode.SMALL, CorrectionLevel2DCode.PERCENT_15)]
        [InlineData(TwoDimensionCodeType.QRCODE_MODEL1, Size2DCode.TINY, CorrectionLevel2DCode.PERCENT_15)]
        [InlineData(TwoDimensionCodeType.QRCODE_MODEL1, Size2DCode.EXTRA, CorrectionLevel2DCode.PERCENT_25)]
        [InlineData(TwoDimensionCodeType.QRCODE_MODEL1, Size2DCode.LARGE, CorrectionLevel2DCode.PERCENT_25)]
        [InlineData(TwoDimensionCodeType.QRCODE_MODEL1, Size2DCode.NORMAL, CorrectionLevel2DCode.PERCENT_25)]
        [InlineData(TwoDimensionCodeType.QRCODE_MODEL1, Size2DCode.SMALL, CorrectionLevel2DCode.PERCENT_25)]
        [InlineData(TwoDimensionCodeType.QRCODE_MODEL1, Size2DCode.TINY, CorrectionLevel2DCode.PERCENT_25)]
        [InlineData(TwoDimensionCodeType.QRCODE_MODEL1, Size2DCode.EXTRA, CorrectionLevel2DCode.PERCENT_30)]
        [InlineData(TwoDimensionCodeType.QRCODE_MODEL1, Size2DCode.LARGE, CorrectionLevel2DCode.PERCENT_30)]
        [InlineData(TwoDimensionCodeType.QRCODE_MODEL1, Size2DCode.NORMAL, CorrectionLevel2DCode.PERCENT_30)]
        [InlineData(TwoDimensionCodeType.QRCODE_MODEL1, Size2DCode.SMALL, CorrectionLevel2DCode.PERCENT_30)]
        [InlineData(TwoDimensionCodeType.QRCODE_MODEL1, Size2DCode.TINY, CorrectionLevel2DCode.PERCENT_30)]
        [InlineData(TwoDimensionCodeType.QRCODE_MODEL1, Size2DCode.EXTRA, CorrectionLevel2DCode.PERCENT_7)]
        [InlineData(TwoDimensionCodeType.QRCODE_MODEL1, Size2DCode.LARGE, CorrectionLevel2DCode.PERCENT_7)]
        [InlineData(TwoDimensionCodeType.QRCODE_MODEL1, Size2DCode.NORMAL, CorrectionLevel2DCode.PERCENT_7)]
        [InlineData(TwoDimensionCodeType.QRCODE_MODEL1, Size2DCode.SMALL, CorrectionLevel2DCode.PERCENT_7)]
        [InlineData(TwoDimensionCodeType.QRCODE_MODEL1, Size2DCode.TINY, CorrectionLevel2DCode.PERCENT_7)]
        [InlineData(TwoDimensionCodeType.QRCODE_MODEL2, Size2DCode.EXTRA, CorrectionLevel2DCode.PERCENT_15)]
        [InlineData(TwoDimensionCodeType.QRCODE_MODEL2, Size2DCode.LARGE, CorrectionLevel2DCode.PERCENT_15)]
        [InlineData(TwoDimensionCodeType.QRCODE_MODEL2, Size2DCode.NORMAL, CorrectionLevel2DCode.PERCENT_15)]
        [InlineData(TwoDimensionCodeType.QRCODE_MODEL2, Size2DCode.SMALL, CorrectionLevel2DCode.PERCENT_15)]
        [InlineData(TwoDimensionCodeType.QRCODE_MODEL2, Size2DCode.TINY, CorrectionLevel2DCode.PERCENT_15)]
        [InlineData(TwoDimensionCodeType.QRCODE_MODEL2, Size2DCode.EXTRA, CorrectionLevel2DCode.PERCENT_25)]
        [InlineData(TwoDimensionCodeType.QRCODE_MODEL2, Size2DCode.LARGE, CorrectionLevel2DCode.PERCENT_25)]
        [InlineData(TwoDimensionCodeType.QRCODE_MODEL2, Size2DCode.NORMAL, CorrectionLevel2DCode.PERCENT_25)]
        [InlineData(TwoDimensionCodeType.QRCODE_MODEL2, Size2DCode.SMALL, CorrectionLevel2DCode.PERCENT_25)]
        [InlineData(TwoDimensionCodeType.QRCODE_MODEL2, Size2DCode.TINY, CorrectionLevel2DCode.PERCENT_25)]
        [InlineData(TwoDimensionCodeType.QRCODE_MODEL2, Size2DCode.EXTRA, CorrectionLevel2DCode.PERCENT_30)]
        [InlineData(TwoDimensionCodeType.QRCODE_MODEL2, Size2DCode.LARGE, CorrectionLevel2DCode.PERCENT_30)]
        [InlineData(TwoDimensionCodeType.QRCODE_MODEL2, Size2DCode.NORMAL, CorrectionLevel2DCode.PERCENT_30)]
        [InlineData(TwoDimensionCodeType.QRCODE_MODEL2, Size2DCode.SMALL, CorrectionLevel2DCode.PERCENT_30)]
        [InlineData(TwoDimensionCodeType.QRCODE_MODEL2, Size2DCode.TINY, CorrectionLevel2DCode.PERCENT_30)]
        [InlineData(TwoDimensionCodeType.QRCODE_MODEL2, Size2DCode.EXTRA, CorrectionLevel2DCode.PERCENT_7)]
        [InlineData(TwoDimensionCodeType.QRCODE_MODEL2, Size2DCode.LARGE, CorrectionLevel2DCode.PERCENT_7)]
        [InlineData(TwoDimensionCodeType.QRCODE_MODEL2, Size2DCode.NORMAL, CorrectionLevel2DCode.PERCENT_7)]
        [InlineData(TwoDimensionCodeType.QRCODE_MODEL2, Size2DCode.SMALL, CorrectionLevel2DCode.PERCENT_7)]
        [InlineData(TwoDimensionCodeType.QRCODE_MODEL2, Size2DCode.TINY, CorrectionLevel2DCode.PERCENT_7)]
        [InlineData(TwoDimensionCodeType.QRCODE_MICRO, Size2DCode.EXTRA, CorrectionLevel2DCode.PERCENT_15)]
        [InlineData(TwoDimensionCodeType.QRCODE_MICRO, Size2DCode.LARGE, CorrectionLevel2DCode.PERCENT_15)]
        [InlineData(TwoDimensionCodeType.QRCODE_MICRO, Size2DCode.NORMAL, CorrectionLevel2DCode.PERCENT_15)]
        [InlineData(TwoDimensionCodeType.QRCODE_MICRO, Size2DCode.SMALL, CorrectionLevel2DCode.PERCENT_15)]
        [InlineData(TwoDimensionCodeType.QRCODE_MICRO, Size2DCode.TINY, CorrectionLevel2DCode.PERCENT_15)]
        [InlineData(TwoDimensionCodeType.QRCODE_MICRO, Size2DCode.EXTRA, CorrectionLevel2DCode.PERCENT_25)]
        [InlineData(TwoDimensionCodeType.QRCODE_MICRO, Size2DCode.LARGE, CorrectionLevel2DCode.PERCENT_25)]
        [InlineData(TwoDimensionCodeType.QRCODE_MICRO, Size2DCode.NORMAL, CorrectionLevel2DCode.PERCENT_25)]
        [InlineData(TwoDimensionCodeType.QRCODE_MICRO, Size2DCode.SMALL, CorrectionLevel2DCode.PERCENT_25)]
        [InlineData(TwoDimensionCodeType.QRCODE_MICRO, Size2DCode.TINY, CorrectionLevel2DCode.PERCENT_25)]
        [InlineData(TwoDimensionCodeType.QRCODE_MICRO, Size2DCode.EXTRA, CorrectionLevel2DCode.PERCENT_30)]
        [InlineData(TwoDimensionCodeType.QRCODE_MICRO, Size2DCode.LARGE, CorrectionLevel2DCode.PERCENT_30)]
        [InlineData(TwoDimensionCodeType.QRCODE_MICRO, Size2DCode.NORMAL, CorrectionLevel2DCode.PERCENT_30)]
        [InlineData(TwoDimensionCodeType.QRCODE_MICRO, Size2DCode.SMALL, CorrectionLevel2DCode.PERCENT_30)]
        [InlineData(TwoDimensionCodeType.QRCODE_MICRO, Size2DCode.TINY, CorrectionLevel2DCode.PERCENT_30)]
        [InlineData(TwoDimensionCodeType.QRCODE_MICRO, Size2DCode.EXTRA, CorrectionLevel2DCode.PERCENT_7)]
        [InlineData(TwoDimensionCodeType.QRCODE_MICRO, Size2DCode.LARGE, CorrectionLevel2DCode.PERCENT_7)]
        [InlineData(TwoDimensionCodeType.QRCODE_MICRO, Size2DCode.NORMAL, CorrectionLevel2DCode.PERCENT_7)]
        [InlineData(TwoDimensionCodeType.QRCODE_MICRO, Size2DCode.SMALL, CorrectionLevel2DCode.PERCENT_7)]
        [InlineData(TwoDimensionCodeType.QRCODE_MICRO, Size2DCode.TINY, CorrectionLevel2DCode.PERCENT_7)]
        public void PrintQRCode_Success(TwoDimensionCodeType codeType, Size2DCode size2DCode, CorrectionLevel2DCode correctionLevel2DCode)
        {
            string data = codeType == TwoDimensionCodeType.QRCODE_MICRO ? SHORTER_WEBSITE_STRING : WEBSITE_STRING;
            var begining = new byte[] { 29, 40, 107, 4, 0, 49, 65, (byte)codeType, 0, 29, 40, 107, 3, 0, 49, 67, (byte)size2DCode, 29, 40, 107, 3, 0, 49, 69, (byte)correctionLevel2DCode, 29, 40, 107, (byte)(data.Length + 3), 0, 49, 80, 48, 104, 116, 116, 112, 115, 58, 47, 47, 103, 105, 116, 104, 117, 98, 46, 99, 111, 109, 47 };
            var ending = new byte[] { 29, 40, 107, 3, 0, 49, 81, 48 };
            
            byte[] result;
            if (codeType == TwoDimensionCodeType.QRCODE_MICRO)
            {
                result = ByteSplicer.Combine(begining, ending);
            }
            else
            {
                var longerStr = new byte[] { 108, 117, 107, 101, 118, 112, 47, 69, 83, 67, 45, 80, 79, 83, 45, 46, 78, 69, 84, 47, };
                result = ByteSplicer.Combine(begining, longerStr, ending);
            }
            
            Assert.Equal(result, new EPSON().Print2DCode(codeType, data, size2DCode, correctionLevel2DCode));
        }
    }
}

