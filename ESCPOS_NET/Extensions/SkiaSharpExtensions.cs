using System;

namespace SkiaSharp.Extensions
{
    public static class SkiaSharpExtensions
    {
        public static byte[] ToSingleBitPixelByteArray(this SKBitmap source,
                                                       out int outputWidth,
                                                       out int outputHeight,
                                                       bool rasterFormat = true,
                                                       int? maxWidth = null,
                                                       int? maxHeight = null,
                                                       float threshold = 0.5f)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (threshold < 0f || threshold > 1f)
            {
                throw new ArgumentOutOfRangeException(nameof(threshold), threshold, "Threshold must be between 0 and 1.");
            }

            SKBitmap resizedBitmap = null;
            SKBitmap monochromeBitmap = null;
            SKBitmap transformedBitmap = null;

            try
            {
                resizedBitmap = ResizeIfNeeded(source, maxWidth, maxHeight);

                var workingBitmap = resizedBitmap ?? source;

                monochromeBitmap = ConvertToMonochrome(workingBitmap, threshold);

                if (rasterFormat)
                {
                    outputWidth = monochromeBitmap.Width;
                    outputHeight = monochromeBitmap.Height;

                    return ConvertToSingleBitPixelByteArray(monochromeBitmap);
                }

                transformedBitmap = Rotate90AndFlipHorizontal(monochromeBitmap);

                outputWidth = transformedBitmap.Width;
                outputHeight = transformedBitmap.Height;

                return ConvertToSingleBitPixelByteArray(transformedBitmap);
            }
            finally
            {
                if (transformedBitmap != null)
                {
                    transformedBitmap.Dispose();
                }

                if (monochromeBitmap != null)
                {
                    monochromeBitmap.Dispose();
                }

                if (resizedBitmap != null)
                {
                    resizedBitmap.Dispose();
                }
            }
        }

        private static SKBitmap ResizeIfNeeded(
    SKBitmap source,
    int? maxWidth,
    int? maxHeight)
        {
            if (!maxWidth.HasValue && !maxHeight.HasValue)
            {
                return null;
            }

            if (maxWidth.HasValue && maxWidth.Value <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(maxWidth), maxWidth, "Maximum width must be greater than zero.");
            }

            if (maxHeight.HasValue && maxHeight.Value <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(maxHeight), maxHeight, "Maximum height must be greater than zero.");
            }

            var maximumWidth = maxWidth ?? source.Width;
            var maximumHeight = maxHeight ?? source.Height;

            var widthScale = (double)maximumWidth / source.Width;
            var heightScale = (double)maximumHeight / source.Height;

            var scale = Math.Min(widthScale, heightScale);

            // Do not enlarge the original image.
            scale = Math.Min(scale, 1d);

            var targetWidth = Math.Max(1, (int)Math.Round(source.Width * scale));

            var targetHeight = Math.Max(1, (int)Math.Round(source.Height * scale));

            if (targetWidth == source.Width && targetHeight == source.Height)
            {
                return null;
            }

            var imageInfo = new SKImageInfo(targetWidth, targetHeight, SKColorType.Bgra8888, SKAlphaType.Premul);

            var sampling = new SKSamplingOptions(SKFilterMode.Linear, SKMipmapMode.None);

            var resized = source.Resize(imageInfo, sampling);

            if (resized == null)
            {
                throw new InvalidOperationException("Unable to resize the image.");
            }

            return resized;
        }

        private static SKBitmap ConvertToMonochrome(SKBitmap source, float threshold)
        {
            var width = source.Width;
            var height = source.Height;
            var luminanceBuffer = new float[width * height];

            CopyLuminanceToBuffer(source, luminanceBuffer);

            ApplyStuckiDithering(luminanceBuffer, width, height, threshold * 255f);

            var result = new SKBitmap(width, height, SKColorType.Bgra8888, SKAlphaType.Opaque);

            for (var y = 0; y < height; y++)
            {
                var rowOffset = y * width;

                for (var x = 0; x < width; x++)
                {
                    var value = luminanceBuffer[rowOffset + x];

                    result.SetPixel(x, y, value < 128f ? SKColors.Black : SKColors.White);
                }
            }

            return result;
        }

        private static void CopyLuminanceToBuffer(SKBitmap source, float[] luminanceBuffer)
        {
            var width = source.Width;
            var height = source.Height;

            for (var y = 0; y < height; y++)
            {
                var rowOffset = y * width;

                for (var x = 0; x < width; x++)
                {
                    var color = source.GetPixel(x, y);

                    // Composite the source color over a white background.
                    var alpha = color.Alpha / 255f;
                    var inverseAlpha = 1f - alpha;

                    var red = (color.Red * alpha) + (255f * inverseAlpha);

                    var green = (color.Green * alpha) + (255f * inverseAlpha);

                    var blue = (color.Blue * alpha) + (255f * inverseAlpha);

                    // Calculate luminance using Rec. 709 coefficients.
                    luminanceBuffer[rowOffset + x] = (0.2126f * red) + (0.7152f * green) + (0.0722f * blue);
                }
            }
        }

        private static void ApplyStuckiDithering(float[] buffer, int width, int height, float threshold)
        {
            for (var y = 0; y < height; y++)
            {
                for (var x = 0; x < width; x++)
                {
                    var index = (y * width) + x;

                    // Compatible replacement for Math.Clamp.
                    var oldValue = Math.Max(0f, Math.Min(255f, buffer[index]));

                    var newValue = oldValue < threshold ? 0f : 255f;

                    buffer[index] = newValue;

                    var error = oldValue - newValue;

                    // Stucki error-diffusion kernel:
                    //
                    //           X   8   4
                    //   2   4   8   4   2
                    //   1   2   4   2   1
                    //
                    // Divisor: 42

                    DistributeError(buffer, width, height, x + 1, y, error, 8f / 42f);

                    DistributeError(buffer, width, height, x + 2, y, error, 4f / 42f);

                    DistributeError(buffer, width, height, x - 2, y + 1, error, 2f / 42f);

                    DistributeError(buffer, width, height, x - 1, y + 1, error, 4f / 42f);

                    DistributeError(buffer, width, height, x, y + 1, error, 8f / 42f);

                    DistributeError(buffer, width, height, x + 1, y + 1, error, 4f / 42f);

                    DistributeError(buffer, width, height, x + 2, y + 1, error, 2f / 42f);

                    DistributeError(buffer, width, height, x - 2, y + 2, error, 1f / 42f);

                    DistributeError(buffer, width, height, x - 1, y + 2, error, 2f / 42f);

                    DistributeError(buffer, width, height, x, y + 2, error, 4f / 42f);

                    DistributeError(buffer, width, height, x + 1, y + 2, error, 2f / 42f);

                    DistributeError(buffer, width, height, x + 2, y + 2, error, 1f / 42f);
                }
            }
        }

        private static void DistributeError(float[] buffer, int width, int height, int x, int y, float error, float factor)
        {
            if (x < 0 || x >= width || y < 0 || y >= height)
            {
                return;
            }

            var index = (y * width) + x;

            buffer[index] += error * factor;
        }

        private static SKBitmap Rotate90AndFlipHorizontal(SKBitmap source)
        {
            var result = new SKBitmap(source.Height, source.Width, SKColorType.Bgra8888, SKAlphaType.Opaque);

            for (var y = 0; y < source.Height; y++)
            {
                for (var x = 0; x < source.Width; x++)
                {
                    // Rotate 90 degrees clockwise.
                    var rotatedX = source.Height - 1 - y;
                    var rotatedY = x;

                    // Flip horizontally after rotating.
                    var destinationX = result.Width - 1 - rotatedX;

                    result.SetPixel(destinationX, rotatedY, source.GetPixel(x, y));
                }
            }

            return result;
        }

        private static byte[] ConvertToSingleBitPixelByteArray(SKBitmap bitmap)
        {
            // Each row must contain a whole number of bytes.
            var bytesPerRow = (bitmap.Width + 7) / 8;
            var result = new byte[bytesPerRow * bitmap.Height];

            for (var y = 0; y < bitmap.Height; y++)
            {
                var rowOffset = y * bytesPerRow;

                for (var x = 0; x < bitmap.Width; x++)
                {
                    var pixel = bitmap.GetPixel(x, y);

                    if (!IsBlack(pixel))
                    {
                        continue;
                    }

                    var byteIndex = rowOffset + (x / 8);

                    // Store the first pixel in the most significant bit.
                    var bitIndex = 7 - (x % 8);

                    result[byteIndex] |= (byte)(1 << bitIndex);
                }
            }

            return result;
        }

        private static bool IsBlack(SKColor color)
        {
            return color.Red == 0 &&
                   color.Green == 0 &&
                   color.Blue == 0;
        }
    }
}