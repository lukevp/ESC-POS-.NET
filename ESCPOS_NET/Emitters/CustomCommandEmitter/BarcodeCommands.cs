using ESCPOS_NET.DataValidation;
using ESCPOS_NET.Emitters.BaseCommandValues;
using ESCPOS_NET.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ESCPOS_NET.Emitters
{
    public abstract partial class CustomCommandEmitter
    {
        public override byte[] Print2DCode(TwoDimensionCodeType type, string data, Size2DCode size = Size2DCode.NORMAL, CorrectionLevel2DCode correction = CorrectionLevel2DCode.PERCENT_7)
        {
            DataValidator.Validate2DCode(type, data);
            List<byte> command = new List<byte>();
            byte[] initial = { Cmd.GS, Barcodes.Set2DCode, Barcodes.PrintBarcode };
            switch (type)
            {
                case TwoDimensionCodeType.PDF417:
                    command.AddRange(initial, Barcodes.SetPDF417NumberOfColumns, Barcodes.AutoEnding);
                    command.AddRange(initial, Barcodes.SetPDF417NumberOfRows, Barcodes.AutoEnding);
                    command.AddRange(initial, Barcodes.SetPDF417DotSize, (byte)size);
                    command.AddRange(initial, Barcodes.SetPDF417CorrectionLevel, (byte)correction);

                    // k = (pL + pH * 256) - 3 --> But pH is always 0.
                    int k = data.Length;
                    int l = k + 3;
                    command.AddRange(initial, (byte)l, Barcodes.StorePDF417Data);
                    command.AddRange(data.ToCharArray().Select(x => (byte)x));

                    // Prints stored PDF417
                    command.AddRange(initial, Barcodes.PrintPDF417);
                    break;

                case TwoDimensionCodeType.QRCODE_MODEL1:
                case TwoDimensionCodeType.QRCODE_MODEL2:
                case TwoDimensionCodeType.QRCODE_MICRO:
                    command.AddRange(initial, Barcodes.SelectQRCodeModel, (byte)type, Barcodes.AutoEnding);
                    command.AddRange(initial, Barcodes.SetQRCodeDotSize, (byte)size);
                    command.AddRange(initial, Barcodes.SetQRCodeCorrectionLevel, (byte)correction);
                    int num = data.Length + 3;
                    int pL = num % 256;
                    int pH = num / 256;

                    // NOTE: StoreQRCodeData is using CUSTOM Printer override
                    command.AddRange(initial, (byte)pL, (byte)pH, CustomCommandValues.Barcodes.StoreQRCodeData);
                    command.AddRange(data.ToCharArray().Select(x => (byte)x));

                    // Prints stored QRCode
                    // NOTE: PrintQRCode is using CUSTOM Printer override
                    command.AddRange(initial, CustomCommandValues.Barcodes.PrintQRCode);
                    break;

                default:
                    throw new NotImplementedException($"2D Code '{type}' was not implemented yet.");
            }

            return command.ToArray();
        }
    }
}
