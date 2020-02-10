using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using SixLabors.ImageSharp.Advanced;
using System.Collections.Generic;
using System.Linq;
using ESCPOS_NET.Utilities;
using System;

namespace ESCPOS_NET.Emitters
{
    public abstract partial class BaseCommandEmitter : ICommandEmitter
    {
        private byte[] GetImageHeader(int commandLength)
        {
            byte[] lengths = new byte[4];
            int i = 0;
            while (commandLength > 0)
            {
                lengths[i] = (byte)(commandLength & 0xFF);
                commandLength >>= 8;
                i++;
            }
            if (i >= 3)
            {
                return new byte[] { Cmd.GS, Images.ImageCmd8, Images.ImageCmdL, lengths[0], lengths[1], lengths[2], lengths[3] };
            }
            else
            {
                return new byte[] { Cmd.GS, Images.ImageCmdParen, Images.ImageCmdL, lengths[0], lengths[1] };
            }
        }

        /* Image Commands */
        public byte[] SetImageDensity(bool isHiDPI)
        {
            ByteArrayBuilder builder = new ByteArrayBuilder();
            byte dpiSetting = isHiDPI ? (byte)0x33 : (byte)0x32; // TODO: is this right??
            byte[] baseCommand = new byte[] { 0x30, 0x31, dpiSetting, dpiSetting};
            builder.Append(GetImageHeader(baseCommand.Length));
            builder.Append(baseCommand);
            return builder.ToArray();
        }

        public byte[] BufferImage(byte[] image, int maxWidth = -1, bool isLegacy = false, int color = 1)
        {
            ByteArrayBuilder imageCommand = new ByteArrayBuilder();

            byte colorByte;
            switch (color)
            {
                case 2:
                    colorByte = 0x32;
                    break;
                case 3:
                    colorByte = 0x33;
                    break;
                default:
                    colorByte = 0x31;
                    break;
            }

            using (Image<Rgba32> img = Image.Load(image))
            {
                int width = img.Width;
                int height = img.Height;
                // Use native width/height of image without resizing if maxWidth is default value.
                if (maxWidth != -1)
                {
                    width = maxWidth;
                    // Get closest height that's the same aspect ratio as the width.
                    height = (int)(maxWidth * (Convert.ToDouble(img.Height) / img.Width));
                    img.Mutate(x => x.Resize(width, height));
                }
                byte heightL = (byte)(height);
                byte heightH = (byte)(height >> 8);

                if (isLegacy)
                {
                    // TODO: if there's a remainder, need to add 1?
                    var byteWidth = width / 8;
                    if (width % 8 != 0)
                    {
                        byteWidth += 1;
                    }
                    byte widthL = (byte)byteWidth;
                    byte widthH = (byte)(byteWidth >> 8);
                    imageCommand.Append(new byte[] { Cmd.GS, Images.ImageCmdLegacy, 0x30, 0x00, widthL, widthH, heightL, heightH });
                }
                else
                {
                    byte widthL = (byte)(width);
                    byte widthH = (byte)(width >> 8);
                    imageCommand.Append(new byte[] { 0x30, 0x70, 0x30, 0x01, 0x01, colorByte, widthL, widthH, heightL, heightH });
                }


                // TODO: test making a List<byte> if it's faster than using ByteArrayBuilder.

                double[] ditherErrorsNext = new double[width + 2];
                // Bit pack every 8 horizontal bits into a single byte.
                for (int y = 0; y < img.Height; y++)
                {
                    // Create an array for capturing dither errors
                    double[] ditherErrorsCurrent = ditherErrorsNext;
                    ditherErrorsNext = new double[width + 2];

                    Span<Rgba32> pixelRowSpan = img.GetPixelRowSpan(y);
                    byte buffer = 0x00;
                    int bufferCount = 7;
                    for (int x = 0; x < img.Width; x++)
                    {
                        // Determine if pixel should be colored in.
                        var pixelValue = ((0.30 * pixelRowSpan[x].R) + (0.59 * pixelRowSpan[x].G) + (0.11 * pixelRowSpan[x].B) + (ditherErrorsCurrent[x]));
                        double error = 0;
                        if (pixelValue <= 127)
                        {
                            // Pixel value should be black (0), if it is greater than zero then the error is the difference (pixelvalue - 0)
                            // which is just the pixel value.
                            error = pixelValue;
                            buffer |= (byte)(0x01 << bufferCount);
                        }
                        else
                        {
                            // Pixel should be white, so error is pixelvalue - 255
                            error = pixelValue - 255;
                        }

                        // Propagate dither.
                        //error <<= 1; // error * 32
                        double pixelValue8 = error / 4;
                        double pixelValue4 = error / 8;
                        double pixelValue2 = error / 16;
                        ditherErrorsCurrent[x + 1] += pixelValue8;
                        ditherErrorsCurrent[x + 2] += pixelValue4;

                        if (x - 2 > 0) ditherErrorsNext[x - 2] += pixelValue2;
                        if (x - 1 > 0) ditherErrorsNext[x - 1] += pixelValue4;
                        ditherErrorsNext[x] += pixelValue8;
                        ditherErrorsNext[x + 1] += pixelValue4;
                        ditherErrorsNext[x + 2] += pixelValue2;

                        if (bufferCount == 0)
                        {
                            bufferCount = 8;
                            imageCommand.Append(new byte[] { buffer });
                            buffer = 0x00;
                        }
                        bufferCount -= 1;
                    }
                    // For images not divisible by 8
                    if (bufferCount != 7)
                    {
                        imageCommand.Append(new byte[] { buffer });
                    }
                }
            }
            // Load image to print buffer
            ByteArrayBuilder response = new ByteArrayBuilder();
            byte[] imageCommandBytes = imageCommand.ToArray();
            if (!isLegacy)
            { 
                response.Append(GetImageHeader(imageCommandBytes.Length));
            }
            response.Append(imageCommandBytes);
            return response.ToArray();
        }

        public byte[] WriteImageFromBuffer()
        {
            // Print image that's already buffered.
            ByteArrayBuilder response = new ByteArrayBuilder();
            byte[] printCommandBytes = new byte[] { 0x30, 0x32 };
            response.Append(GetImageHeader(printCommandBytes.Length));
            response.Append(printCommandBytes);
            return response.ToArray();
        }
        public byte[] PrintImage(byte[] image, bool isHiDPI, bool isLegacy = false, int maxWidth = -1, int color = 1)
        {
            if (isLegacy)
            {
                return ByteSplicer.Combine(BufferImage(image, maxWidth, isLegacy));
            }
            else
            { 
                return ByteSplicer.Combine(SetImageDensity(isHiDPI), BufferImage(image, maxWidth, isLegacy, color), WriteImageFromBuffer());
            }
        }
    }
}
