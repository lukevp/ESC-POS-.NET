using System;
using System.IO;
using System.Numerics;
using System.Linq;
using ESCPOS_NET.Emitters;
using Xunit;
using SkiaSharp;

namespace ESCPOS_NET.UnitTest.EmittersBased.EPSONTests
{
    public class ImageCommandsTests
    {
        private readonly EPSON _emitter = new EPSON();

        [Fact]
        public void LegacyImage_PacksPixelsAndPadsEachRow()
        {
            using var image = Bitmap(9, 2, SKColors.White);
            image.SetPixel(0, 0, SKColors.Black);
            image.SetPixel(8, 0, SKColors.Black);
            image.SetPixel(1, 1, SKColors.Black);

            var result = _emitter.PrintImage(Png(image), true, isLegacy: true);

            Assert.Equal(new byte[] { 0x1D, 0x76, 0x30, 0, 2, 0, 2, 0, 0x80, 0x80, 0x40, 0 }, result);
        }

        [Fact]
        public void LegacyImage_CompositesTransparencyOntoWhite()
        {
            using var image = Bitmap(8, 1, SKColors.Transparent);

            Assert.Equal(new byte[] { 0x1D, 0x76, 0x30, 0, 1, 0, 1, 0, 0 },
                _emitter.PrintImage(Png(image), true, isLegacy: true));
        }

        [Fact]
        public void LegacyImage_ResizesWithOriginalAspectRatio()
        {
            using var image = Bitmap(32, 16, SKColors.Black);
            var result = _emitter.PrintImage(Png(image), true, isLegacy: true, maxWidth: 16);

            Assert.Equal(new byte[] { 0x1D, 0x76, 0x30, 0, 2, 0, 8, 0 }, result.Take(8));
            Assert.Equal(24, result.Length);
            Assert.All(result.Skip(8), value => Assert.Equal((byte)0xFF, value));
        }

        [Fact]
        public void LegacyImage_EncodesHighDimensionBytes()
        {
            using var image = Bitmap(2048, 256, SKColors.White);
            var result = _emitter.PrintImage(Png(image), true, isLegacy: true);

            Assert.Equal(new byte[] { 0x1D, 0x76, 0x30, 0, 0, 1, 0, 1 }, result.Take(8));
            Assert.Equal(8 + 256 * 256, result.Length);
        }

        [Theory]
        [InlineData(false, 0x32)]
        [InlineData(true, 0x33)]
        public void BufferedImage_IncludesDensityBufferAndPrintCommands(bool highDensity, byte density)
        {
            using var image = Bitmap(8, 1, SKColors.Black);
            var result = _emitter.PrintImage(Png(image), highDensity, color: 2);

            Assert.Equal(new byte[] {
                0x1D, 0x28, 0x4C, 4, 0, 0x30, 0x31, density, density,
                0x1D, 0x28, 0x4C, 11, 0, 0x30, 0x70, 0x30, 1, 1, 0x32, 8, 0, 1, 0, 0xFF,
                0x1D, 0x28, 0x4C, 2, 0, 0x30, 0x32
            }, result);
        }

        [Fact]
        public void PrintImage_RejectsInvalidImageData()
        {
            Assert.Throws<ArgumentException>(() => _emitter.PrintImage(new byte[] { 1, 2, 3 }, true));
        }

        [Theory]
        [InlineData("png")]
        [InlineData("jpg")]
        [InlineData("bmp")]
        [InlineData("gif")]
        public void PrintImage_DecodesExistingSampleFormats(string extension)
        {
            var bytes = File.ReadAllBytes(Path.Combine(AppContext.BaseDirectory, "TestImages", "pd-logo-300." + extension));
            var result = _emitter.PrintImage(bytes, true, isLegacy: true, maxWidth: 300);

            int rowBytes = result[4] + (result[5] << 8);
            int height = result[6] + (result[7] << 8);
            Assert.Equal(38, rowBytes);
            Assert.True(height > 0);
            Assert.Equal(8 + rowBytes * height, result.Length);
            Assert.Contains(result.Skip(8), value => value != 0);
        }

        [Fact]
        public void PrintImage_DithersGrayIntoBlackAndWhitePixels()
        {
            using var image = Bitmap(64, 64, new SKColor(128, 128, 128));
            var result = _emitter.PrintImage(Png(image), true, isLegacy: true);
            int blackPixels = result.Skip(8).Sum(value => BitOperations.PopCount((uint)value));

            Assert.InRange(blackPixels, 64 * 64 * 35 / 100, 64 * 64 * 65 / 100);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-2)]
        public void PrintImage_RejectsInvalidMaximumWidths(int maxWidth)
        {
            using var image = Bitmap(8, 1, SKColors.Black);
            Assert.Throws<ArgumentOutOfRangeException>(() => _emitter.PrintImage(Png(image), true, maxWidth: maxWidth));
        }
        private static SKBitmap Bitmap(int width, int height, SKColor color)
        {
            var bitmap = new SKBitmap(width, height);
            bitmap.Erase(color);
            return bitmap;
        }

        private static byte[] Png(SKBitmap bitmap)
        {
            using var image = SKImage.FromBitmap(bitmap);
            using var data = image.Encode(SKEncodedImageFormat.Png, 100);
            return data.ToArray();
        }
    }
}