using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ESCPOS_NET.DataValidation;
using ESCPOS_NET.Emitters.BaseCommandValues;
using ESCPOS_NET.Emitters.Enums;
using ESCPOS_NET.Extensions;

namespace ESCPOS_NET.Emitters.BaseCommandEmitter
{
    public abstract partial class BaseCommandEmitter : ICommandEmitter
    {
        /* Barcode Commands */
        public byte[] PrintBarcode(BarcodeType type, string barcode, BarcodeCode code = BarcodeCode.CODE_B)
        {
            DataValidator.ValidateBarcode(type, code, barcode);

            // For CODE128, prepend the first 2 characters as 0x7B and the CODE type, and escape 0x7B characters.
            if (type == BarcodeType.CODE128)
            {
                if (code == BarcodeCode.CODE_C)
                {
                    var b = Encoding.ASCII.GetBytes(barcode);
                    var ob = new byte[b.Length / 2];
                    for (int i = 0, obc = 0; i < b.Length; i += 2)
                        ob[obc++] = (byte) ((b[i] - '0') * 10 + (b[i + 1] - '0'));

                    barcode = Encoding.ASCII.GetString(ob);
                }

                barcode = barcode.Replace("{", "{{");
                barcode = $"{(char) 0x7B}{(char) code}" + barcode;
            }

            var command = new List<byte> {Cmd.GS, Barcodes.PrintBarcode, (byte) type, (byte) barcode.Length};
            command.AddRange(barcode.ToCharArray().Select(x => (byte) x));
            return command.ToArray();
        }

        public byte[] Print2DCode(TwoDimensionCodeType type, string data, Size2DCode size = Size2DCode.NORMAL,
            CorrectionLevel2DCode correction = CorrectionLevel2DCode.PERCENT_7)
        {
            DataValidator.Validate2DCode(type, data);
            var command = new List<byte>();
            byte[] initial = {Cmd.GS, Barcodes.Set2DCode, Barcodes.PrintBarcode};
            switch (type)
            {
                case TwoDimensionCodeType.PDF417:
                    command.AddRange(initial, Barcodes.SetPDF417NumberOfColumns, Barcodes.AutoEnding);
                    command.AddRange(initial, Barcodes.SetPDF417NumberOfRows, Barcodes.AutoEnding);
                    command.AddRange(initial, Barcodes.SetPDF417DotSize, (byte) size);
                    command.AddRange(initial, Barcodes.SetPDF417CorrectionLevel, (byte) correction);

                    // k = (pL + pH * 256) - 3 --> But pH is always 0.
                    var k = data.Length;
                    var l = k + 3;
                    command.AddRange(initial, (byte) l, Barcodes.StorePDF417Data);
                    command.AddRange(data.ToCharArray().Select(x => (byte) x));

                    // Prints stored PDF417
                    command.AddRange(initial, Barcodes.PrintPDF417);
                    break;

                case TwoDimensionCodeType.QRCODE_MODEL1:
                case TwoDimensionCodeType.QRCODE_MODEL2:
                case TwoDimensionCodeType.QRCODE_MICRO:
                    command.AddRange(initial, Barcodes.SelectQRCodeModel, (byte) type, Barcodes.AutoEnding);
                    command.AddRange(initial, Barcodes.SetQRCodeDotSize, (byte) size);
                    command.AddRange(initial, Barcodes.SetQRCodeCorrectionLevel, (byte) correction);
                    var num = data.Length + 3;
                    var pL = num % 256;
                    var pH = num / 256;
                    command.AddRange(initial, (byte) pL, (byte) pH, Barcodes.StoreQRCodeData);
                    command.AddRange(data.ToCharArray().Select(x => (byte) x));

                    // Prints stored QRCode
                    command.AddRange(initial, Barcodes.PrintQRCode);
                    break;

                default:
                    throw new NotImplementedException($"2D Code '{type}' was not implemented yet.");
            }

            return command.ToArray();
        }

        public byte[] SetBarcodeHeightInDots(int height)
        {
            return new[] {Cmd.GS, Barcodes.SetBarcodeHeightInDots, (byte) height};
        }

        public byte[] SetBarWidth(BarWidth width)
        {
            return new[] {Cmd.GS, Barcodes.SetBarWidth, (byte) width};
        }

        public byte[] SetBarLabelPosition(BarLabelPrintPosition position)
        {
            return new[] {Cmd.GS, Barcodes.SetBarLabelPosition, (byte) position};
        }

        public byte[] SetBarLabelFontB(bool fontB)
        {
            return new[] {Cmd.GS, Barcodes.SetBarLabelFont, (byte) (fontB ? 1 : 0)};
        }
    }
}