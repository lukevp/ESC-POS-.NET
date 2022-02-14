using ESCPOS_NET.Emitters.BaseCommandValues;
using ESCPOS_NET.Utilities;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;

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
        public virtual byte[] SetImageDensity(bool isHiDPI)
        {
            ByteArrayBuilder builder = new ByteArrayBuilder();
            byte dpiSetting = isHiDPI ? (byte)0x33 : (byte)0x32; // TODO: is this right??
            byte[] baseCommand = new byte[] { 0x30, 0x31, dpiSetting, dpiSetting };
            builder.Append(GetImageHeader(baseCommand.Length));
            builder.Append(baseCommand);
            return builder.ToArray();
        }

        public virtual byte[] BufferImage(byte[] image, int maxWidth = -1, bool isLegacy = false, int color = 1)
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

            int width;
            int height;
            byte[] imageData;
            using (var img = Image.Load<Rgba32>(image))
            {
                imageData = img.ToSingleBitPixelByteArray(maxWidth: maxWidth == -1 ? (int?)null : maxWidth);
                height = img.Height;
                width = img.Width;
            }

            byte heightL = (byte)height;
            byte heightH = (byte)(height >> 8);

            if (isLegacy)
            {
                var byteWidth = (width + 7 & -8) / 8;
                byte widthL = (byte)byteWidth;
                byte widthH = (byte)(byteWidth >> 8);
                imageCommand.Append(new byte[] { Cmd.GS, Images.ImageCmdLegacy, 0x30, 0x00, widthL, widthH, heightL, heightH });
            }
            else
            {
                byte widthL = (byte)width;
                byte widthH = (byte)(width >> 8);
                imageCommand.Append(new byte[] { 0x30, 0x70, 0x30, 0x01, 0x01, colorByte, widthL, widthH, heightL, heightH });
            }

            imageCommand.Append(imageData);

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

        public virtual byte[] WriteImageFromBuffer()
        {
            // Print image that's already buffered.
            ByteArrayBuilder response = new ByteArrayBuilder();
            byte[] printCommandBytes = new byte[] { 0x30, 0x32 };
            response.Append(GetImageHeader(printCommandBytes.Length));
            response.Append(printCommandBytes);
            return response.ToArray();
        }

        public virtual byte[] PrintImage(byte[] image, bool isHiDPI, bool isLegacy = false, int maxWidth = -1, int color = 1)
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
