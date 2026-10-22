using System.IO;
using System.Linq;
using ESCPOS_NET.Emitters;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using Xunit;
using Color = SixLabors.ImageSharp.Color;

namespace ESCPOS_NET.UnitTest.EmittersBased.EPSONTests
{
    public class ImageCommandsTests
    {
        private readonly EPSON _emitter = new EPSON();

        [Fact]
        public void LegacyImage_PacksPixelsAndPadsEachRow()
        {
            using var image = new Image<Rgba32>(9, 2, Color.White.ToPixel<Rgba32>());
            image[0, 0] = Color.Black.ToPixel<Rgba32>();
            image[8, 0] = Color.Black.ToPixel<Rgba32>();
            image[1, 1] = Color.Black.ToPixel<Rgba32>();

            var result = _emitter.PrintImage(Png(image), true, isLegacy: true);

            Assert.Equal(new byte[] { 0x1D, 0x76, 0x30, 0, 2, 0, 2, 0, 0x80, 0x80, 0x40, 0 }, result);
        }

        [Fact]
        public void LegacyImage_CompositesTransparencyOntoWhite()
        {
            using var image = new Image<Rgba32>(8, 1);

            Assert.Equal(new byte[] { 0x1D, 0x76, 0x30, 0, 1, 0, 1, 0, 0 },
                _emitter.PrintImage(Png(image), true, isLegacy: true));
        }

        [Fact]
        public void LegacyImage_ResizesWithOriginalAspectRatio()
        {
            using var image = new Image<Rgba32>(32, 16, Color.Black.ToPixel<Rgba32>());
            var result = _emitter.PrintImage(Png(image), true, isLegacy: true, maxWidth: 16);

            Assert.Equal(new byte[] { 0x1D, 0x76, 0x30, 0, 2, 0, 8, 0 }, result.Take(8));
            Assert.Equal(24, result.Length);
            Assert.All(result.Skip(8), value => Assert.Equal((byte)0xFF, value));
        }

        [Fact]
        public void LegacyImage_EncodesHighDimensionBytes()
        {
            using var image = new Image<Rgba32>(2048, 256, Color.White.ToPixel<Rgba32>());
            var result = _emitter.PrintImage(Png(image), true, isLegacy: true);

            Assert.Equal(new byte[] { 0x1D, 0x76, 0x30, 0, 0, 1, 0, 1 }, result.Take(8));
            Assert.Equal(8 + 256 * 256, result.Length);
        }

        [Theory]
        [InlineData(false, 0x32)]
        [InlineData(true, 0x33)]
        public void BufferedImage_IncludesDensityBufferAndPrintCommands(bool highDensity, byte density)
        {
            using var image = new Image<Rgba32>(8, 1, Color.Black.ToPixel<Rgba32>());
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
            Assert.Throws<UnknownImageFormatException>(() => _emitter.PrintImage(new byte[] { 1, 2, 3 }, true));
        }

        private static byte[] Png(Image<Rgba32> image)
        {
            using var stream = new MemoryStream();
            image.SaveAsPng(stream);
            return stream.ToArray();
        }
    }
}