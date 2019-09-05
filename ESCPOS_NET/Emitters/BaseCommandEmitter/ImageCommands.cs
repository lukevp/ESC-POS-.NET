using ESCPOS_NET.Utilities;
using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;

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

        public byte[] BufferImage(byte[] image, int maxWidth = -1, int color = 1)
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

            using (var originalImg = Image.FromStream(new MemoryStream(image))) {
                var img = (Bitmap) originalImg;
                int width = img.Width;
                int height = img.Height;
                // Use native width/height of image without resizing if maxWidth is default value.
                if (maxWidth != -1) {
                    width = maxWidth;
                    // Get closest height that's the same aspect ratio as the width.
                    height = (int)(maxWidth * (Convert.ToDouble(img.Height) / img.Width));
                    img = ResizeImage(img, width, height);
                }
                byte widthL = (byte)(width);
                byte widthH = (byte)(width >> 8);
                byte heightL = (byte)(height);
                byte heightH = (byte)(height >> 8);

                imageCommand.Append(new byte[] { 0x30, 0x70, 0x30, 0x01, 0x01, colorByte, widthL, widthH, heightL, heightH });
                // TODO: test making a List<byte> if it's faster than using ByteArrayBuilder.

                // Bit pack every 8 horizontal bits into a single byte.
                for (int y = 0; y < img.Height; y++) {
                    byte buffer = 0x00;
                    int bufferCount = 7;
                    for (int x = 0; x < img.Width; x++) {
                        // Determine if pixel should be colored in.
                        var pixel = img.GetPixel(x, y);
                        if ((0.30 * pixel.R) + (0.59 * pixel.G) + (0.11 * pixel.B) <= 127) {
                            buffer |= (byte)(0x01 << bufferCount);
                        }
                        if (bufferCount == 0) {
                            bufferCount = 8;
                            imageCommand.Append(new byte[] { buffer });
                            buffer = 0x00;
                        }
                        bufferCount -= 1;
                    }
                    // For images not divisible by 8
                    if (bufferCount != 7) {
                        imageCommand.Append(new byte[] { buffer });
                    }
                }
            }
            // Load image to print buffer
            byte[] imageCommandBytes = imageCommand.ToArray();
            ByteArrayBuilder response = new ByteArrayBuilder();
            response.Append(GetImageHeader(imageCommandBytes.Length));
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

        public byte[] PrintImage(byte[] image, bool isHiDPI, int maxWidth = -1, int color = 1)
        {
            return ByteSplicer.Combine(SetImageDensity(isHiDPI), BufferImage(image, maxWidth, color), WriteImageFromBuffer());
        }

        /// <summary>
        /// Resize the image to the specified width and height.
        /// See https://stackoverflow.com/a/24199315/518491
        /// </summary>
        /// <param name="image">The image to resize.</param>
        /// <param name="width">The width to resize to.</param>
        /// <param name="height">The height to resize to.</param>
        /// <returns>The resized image.</returns>
        public static Bitmap ResizeImage(Image image, int width, int height) {
            var destRect = new Rectangle(0, 0, width, height);
            var destImage = new Bitmap(width, height);

            destImage.SetResolution(image.HorizontalResolution, image.VerticalResolution);

            using (var graphics = Graphics.FromImage(destImage)) {
                graphics.CompositingMode = CompositingMode.SourceCopy;
                graphics.CompositingQuality = CompositingQuality.HighQuality;
                graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                graphics.SmoothingMode = SmoothingMode.HighQuality;
                graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;

                using (var wrapMode = new ImageAttributes()) {
                    wrapMode.SetWrapMode(WrapMode.TileFlipXY);
                    graphics.DrawImage(image, destRect, 0, 0, image.Width, image.Height, GraphicsUnit.Pixel, wrapMode);
                }
            }

            return destImage;
        }
    }
}
