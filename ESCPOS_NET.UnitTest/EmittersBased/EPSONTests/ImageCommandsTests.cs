using ESCPOS_NET.Emitters;
using SixLabors.Fonts;
using SixLabors.ImageSharp;
using Xunit;

namespace ESCPOS_NET.UnitTest.EmittersBased.EPSONTests
{
    public class ImageCommandsTests
    {
        [Fact]
        public void RasterisesText_Success()
        {
            var epson = new EPSON();
            FontCollection collection = new FontCollection();
            FontFamily family = collection.Install("c://windows/fonts/arial.ttf");

            byte[] rawImage = epson.PrintRasterizedText("Hello world!", family, 24);
            var renderedImage = Image.Load(rawImage);

            // compare to a fixture?
        }
    }
}
