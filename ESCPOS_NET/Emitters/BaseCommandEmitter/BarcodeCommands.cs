using ESCPOS_NET.DataValidation;
using ESCPOS_NET.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ESCPOS_NET.Emitters
{
    public abstract partial class BaseCommandEmitter<TCommandValues> : ICommandEmitter
    {
        /* Barcode Commands */
        public virtual byte[] PrintBarcode(BarcodeType type, string barcode, BarcodeCode code = BarcodeCode.CODE_B)
        {
            DataValidator.ValidateBarcode(type, code, barcode);
            return BarcodeBytes(type, barcode, code);
        }

        protected virtual byte[] BarcodeBytes(BarcodeType type, string barcode, BarcodeCode code)
        {
            // For CODE128, prepend the first 2 characters as 0x7B and the CODE type, and escape 0x7B characters.
            if (type == BarcodeType.CODE128)
            {
                if (code == BarcodeCode.CODE_C)
                {
                    byte[] b = Encoding.ASCII.GetBytes(barcode);
                    byte[] ob = new byte[b.Length / 2];
                    for (int i = 0, obc = 0; i < b.Length; i += 2)
                    {
                        ob[obc++] = (byte)(((b[i] - '0') * 10) + (b[i + 1] - '0'));
                    }

                    barcode = Encoding.ASCII.GetString(ob);
                }

                barcode = barcode.Replace("{", "{{");
                barcode = $"{(char)0x7B}{(char)code}" + barcode;
            }

            var command = new List<byte> { Values.GS, Values.PrintBarcode, (byte)type, (byte)barcode.Length };
            command.AddRange(barcode.ToCharArray().Select(x => (byte)x));
            return command.ToArray();
        }

        public virtual byte[] PrintQRCode(string data, TwoDimensionCodeType type = TwoDimensionCodeType.QRCODE_MODEL2, Size2DCode size = Size2DCode.NORMAL, CorrectionLevel2DCode correction = CorrectionLevel2DCode.PERCENT_7)
        {
            if (type == TwoDimensionCodeType.PDF417)
            {
                throw new ArgumentException($"{nameof(TwoDimensionCodeType.PDF417)} is not a valid QRCode type. Please use {nameof(Print2DCode)} method", nameof(type));
            }

            return Print2DCode(type, data, size, correction);
        }

        public virtual byte[] Print2DCode(TwoDimensionCodeType type, string data, Size2DCode size = Size2DCode.NORMAL, CorrectionLevel2DCode correction = CorrectionLevel2DCode.PERCENT_7)
        {
            DataValidator.Validate2DCode(type, data);
            return TwoDimensionCodeBytes(type, data, size, correction);
        }

        protected virtual byte[] TwoDimensionCodeBytes(TwoDimensionCodeType type, string data, Size2DCode size, CorrectionLevel2DCode correction)
        {
            List<byte> command = new List<byte>();
            byte[] initial = { Values.GS, Values.Set2DCode, Values.PrintBarcode };
            switch (type)
            {
                case TwoDimensionCodeType.PDF417:
                    command.AddRange(initial, Values.SetPDF417NumberOfColumns, Values.AutoEnding);
                    command.AddRange(initial, Values.SetPDF417NumberOfRows, Values.AutoEnding);
                    command.AddRange(initial, Values.SetPDF417DotSize, (byte)size);
                    command.AddRange(initial, Values.SetPDF417CorrectionLevel, (byte)correction);

                    // k = (pL + pH * 256) - 3 --> But pH is always 0.
                    int k = data.Length;
                    int l = k + 3;
                    command.AddRange(initial, (byte)l, Values.StorePDF417Data);
                    command.AddRange(data.ToCharArray().Select(x => (byte)x));

                    // Prints stored PDF417
                    command.AddRange(initial, Values.PrintPDF417);
                    break;

                case TwoDimensionCodeType.QRCODE_MODEL1:
                case TwoDimensionCodeType.QRCODE_MODEL2:
                case TwoDimensionCodeType.QRCODE_MICRO:
                    command.AddRange(initial, Values.SelectQRCodeModel, (byte)type, Values.AutoEnding);
                    command.AddRange(initial, Values.SetQRCodeDotSize, (byte)size);
                    command.AddRange(initial, Values.SetQRCodeCorrectionLevel, (byte)correction);
                    int num = data.Length + 3;
                    int pL = num % 256;
                    int pH = num / 256;
                    command.AddRange(initial, (byte)pL, (byte)pH, Values.StoreQRCodeData);
                    command.AddRange(data.ToCharArray().Select(x => (byte)x));

                    // Prints stored QRCode
                    command.AddRange(initial, Values.PrintQRCode);
                    break;

                default:
                    throw new NotImplementedException($"2D Code '{type}' was not implemented yet.");
            }

            return command.ToArray();
        }

        public virtual byte[] SetBarcodeHeightInDots(int height) => new byte[] { Values.GS, Values.SetBarcodeHeightInDots, (byte)height };

        public virtual byte[] SetBarWidth(BarWidth width) => new byte[] { Values.GS, Values.SetBarWidth, (byte)width };

        public virtual byte[] SetBarLabelPosition(BarLabelPrintPosition position) => new byte[] { Values.GS, Values.SetBarLabelPosition, (byte)position };

        public virtual byte[] SetBarLabelFontB(bool fontB) => new byte[] { Values.GS, Values.SetBarLabelFont, (byte)(fontB ? 1 : 0) };
    }
}
