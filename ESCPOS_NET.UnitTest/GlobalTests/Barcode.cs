using ESCPOS_NET.Emitters;
using System;
using System.Reflection;
using Xunit;

namespace ESCPOS_NET.UnitTest.GlobalTests
{
    public class Barcode
    {
        private const string WEBSITE_STRING = "https://github.com/lukevp/ESC-POS-.NET/";
        private const string SHORTER_WEBSITE_STRING = "https://github.com/";

        [Theory]
        [InlineData("EPSON", TwoDimensionCodeType.QRCODE_MODEL1)]
        [InlineData("EPSON", TwoDimensionCodeType.QRCODE_MODEL2)]
        [InlineData("EPSON", TwoDimensionCodeType.QRCODE_MICRO)]
        [InlineData("EPSON", null)]
        public void PrintQRCode_Success(string emitter, TwoDimensionCodeType? codeType)
        {
            var type = Assembly.LoadFrom("ESCPOS_NET").GetType($"ESCPOS_NET.Emitters.{emitter}", true);
            ICommandEmitter e = (ICommandEmitter)Activator.CreateInstance(type);

            byte[] bytes;
            if (codeType is TwoDimensionCodeType codeTypeValue)
            {
                string data = codeTypeValue == TwoDimensionCodeType.QRCODE_MICRO ? SHORTER_WEBSITE_STRING : WEBSITE_STRING;
                bytes = e.PrintQRCode(data, codeTypeValue);
            }
            else
                bytes = e.PrintQRCode(WEBSITE_STRING);

            Assert.True(bytes.Length > 0);
        }

        [Theory]
        [InlineData("EPSON", TwoDimensionCodeType.PDF417)]
        public void PrintQRCode_Failure(string emitter, TwoDimensionCodeType codeType)
        {
            var type = Assembly.LoadFrom("ESCPOS_NET").GetType($"ESCPOS_NET.Emitters.{emitter}", true);
            ICommandEmitter e = (ICommandEmitter)Activator.CreateInstance(type);
            Assert.Throws<ArgumentException>(() => e.PrintQRCode(WEBSITE_STRING, codeType));
        }
    }
}
