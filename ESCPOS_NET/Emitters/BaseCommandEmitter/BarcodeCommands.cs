using ESCPOS_NET.DataValidation;
using System.Collections.Generic;
using System.Linq;

namespace ESCPOS_NET.Emitters
{
    public abstract partial class BaseCommandEmitter : ICommandEmitter
    {

        /* Barcode Commands */
        public byte[] PrintBarcode(BarcodeType type, string barcode, BarcodeCode code = BarcodeCode.CODE_B)
        {
            DataValidator.ValidateBarcode(type, barcode);
            
            // For CODE128, prepend the first 2 characters as 0x7B and the CODE type, and escape 0x7B characters.
            if (type == BarcodeType.CODE128)
            {
                barcode = barcode.Replace("{", "{{");
                barcode = $"{(char)0x7B}{(char)code}" + barcode;
            }

            var command = new List<byte> { Cmd.GS, Barcodes.PrintBarcode, (byte)type, (byte)barcode.Length };
            command.AddRange(barcode.ToCharArray().Select(x => (byte)x));
            return command.ToArray();
        }
        public byte[] SetBarcodeHeightInDots(int height) => new byte[] { Cmd.GS, Barcodes.SetBarcodeHeightInDots, (byte)height };
        public byte[] SetBarWidth(BarWidth width) => new byte[] { Cmd.GS, Barcodes.SetBarWidth, (byte)width };
        public byte[] SetBarLabelPosition(BarLabelPrintPosition position) => new byte[] { Cmd.GS, Barcodes.SetBarLabelPosition, (byte)position };
        public byte[] SetBarLabelFontB(bool fontB) => new byte[] { Cmd.GS, Barcodes.SetBarLabelFont, (byte)(fontB ? 1 : 0) };
    }
}
