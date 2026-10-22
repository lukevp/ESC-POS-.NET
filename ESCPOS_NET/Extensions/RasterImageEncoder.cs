using System;
using SkiaSharp;

namespace ESCPOS_NET.Utilities
{
    internal static class RasterImageEncoder
    {
        // Stucki kernel: pixel offsets and weights for this row and the next two.
        private static readonly (int DeltaX, int DeltaY, int Weight)[] StuckiKernel = {
            (1, 0, 8), (2, 0, 4),
            (-2, 1, 2), (-1, 1, 4), (0, 1, 8), (1, 1, 4), (2, 1, 2),
            (-2, 2, 1), (-1, 2, 2), (0, 2, 4), (1, 2, 2), (2, 2, 1)
        };

        internal static byte[] Encode(byte[] encodedImage, int maxWidth, out int width, out int height)
        {
            const float RedLuminanceWeight = 0.2126f;
            const float GreenLuminanceWeight = 0.7152f;
            const float BlueLuminanceWeight = 0.0722f;
            const float BlackLevel = 0f;
            const float WhiteLevel = 255f;
            const float BlackWhiteThreshold = (BlackLevel + WhiteLevel) / 2;
            const float StuckiWeightSum = 42f;
            const int DiffusionRadius = 2;
            const int ErrorRowCount = DiffusionRadius + 1;
            const int ErrorRowPadding = DiffusionRadius * 2;
            const int BitsPerByte = 8;
            const byte MostSignificantBit = 0x80;
            const int UnlimitedWidth = -1;

            if (maxWidth != UnlimitedWidth && maxWidth <= 0)
                throw new ArgumentOutOfRangeException(nameof(maxWidth));

            using var data = SKData.CreateCopy(encodedImage);
            using var codec = SKCodec.Create(data)
                ?? throw new ArgumentException("The image could not be decoded.", nameof(encodedImage));
            using var source = SKBitmap.Decode(codec)
                ?? throw new ArgumentException("The image could not be decoded.", nameof(encodedImage));
            width = maxWidth == UnlimitedWidth ? source.Width : maxWidth;
            height = Math.Max(1, checked((int)Math.Round(source.Height * (double)width / source.Width)));
            if (width > ushort.MaxValue || height > ushort.MaxValue)
                throw new ArgumentException("The image exceeds the ESC/POS raster dimensions.", nameof(encodedImage));

            using var bitmap = new SKBitmap(width, height, SKColorType.Rgba8888, SKAlphaType.Opaque);
            using (var canvas = new SKCanvas(bitmap))
            using (var image = SKImage.FromBitmap(source))
            {
                canvas.Clear(SKColors.White);
                canvas.DrawImage(image, new SKRect(0, 0, width, height), new SKSamplingOptions(SKFilterMode.Linear));
            }

            int bytesPerRow = (width + BitsPerByte - 1) / BitsPerByte;
            var result = new byte[checked(bytesPerRow * height)];
            var pixels = bitmap.Pixels;
            var errors = new float[ErrorRowCount][];
            for (int row = 0; row < ErrorRowCount; row++)
                errors[row] = new float[width + ErrorRowPadding];
            for (int y = 0; y < height; y++)
            {
                var currentErrors = errors[y % ErrorRowCount];
                for (int x = 0; x < width; x++)
                {
                    var pixel = pixels[y * width + x];
                    float gray = Math.Clamp(RedLuminanceWeight * pixel.Red + GreenLuminanceWeight * pixel.Green
                        + BlueLuminanceWeight * pixel.Blue + currentErrors[x + DiffusionRadius], BlackLevel, WhiteLevel);
                    bool black = gray < BlackWhiteThreshold;
                    if (black)
                        result[y * bytesPerRow + x / BitsPerByte] |= (byte)(MostSignificantBit >> (x % BitsPerByte));

                    float error = gray - (black ? BlackLevel : WhiteLevel);
                    foreach (var offset in StuckiKernel)
                        errors[(y + offset.DeltaY) % ErrorRowCount][x + DiffusionRadius + offset.DeltaX] += error * offset.Weight / StuckiWeightSum;
                }
                Array.Clear(currentErrors, 0, currentErrors.Length);
            }
            return result;
        }
    }
}