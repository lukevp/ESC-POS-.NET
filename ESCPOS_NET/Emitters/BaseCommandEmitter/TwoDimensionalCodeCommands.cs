using ESCPOS_NET.DataValidation;
using ESCPOS_NET.Utilities;
using System.Collections.Generic;
using System.Linq;

namespace ESCPOS_NET.Emitters
{
    public abstract partial class BaseCommandEmitter : ICommandEmitter
    {
        private byte[] Get2DCommandHeader(TwoDimensionalCodeType type, byte FunctionID, int dataLength)
        {
            int storelen = dataLength + 3;
            byte store_pL = (byte)(storelen % 256);
            byte store_pH = (byte)(storelen / 256);
            return new byte[] { Cmd.GS, TwoDimensionalCodes.Paren, TwoDimensionalCodes.K, store_pL, store_pH, (byte)type, FunctionID };

        }
        /* 2D Code Commands */
        public byte[] Print2DCode(TwoDimensionalCodeType type, string codeData)
        {
                Dictionary<TwoDimensionalCodeType, byte> functionCodes
                //DataValidator.Validate2DCode(type, barcode);
                /*
                // For CODE128, prepend the first 2 characters as 0x7B and the CODE type, and escape 0x7B characters.
                if (type == BarcodeType.CODE128)
                {
                    barcode = barcode.Replace("{", "{{");
                    barcode = $"{(char)0x7B}{(char)code}" + barcode;
                }
                */

                var command = Get2DCommandHeader(type, );
            command.AddRange(barcode.ToCharArray().Select(x => (byte)x));
            return command.ToArray();
        }
        public byte[] Print2DCode(TwoDimensionalCodeType type, string codeData, int size)
        {
            return ByteSplicer.Combine(
                Set2DCodeWidth(type, size),
                Print2DCode(type, codeData)
            );
        }
        public byte[] PrintQRCode(string codeData, int size)
        {
            return ByteSplicer.Combine(
                Set2DCodeWidth(TwoDimensionalCodeType.QR_CODE, size),
                Print2DCode(TwoDimensionalCodeType.QR_CODE, codeData)
            );
        }
        public byte[] Print2DCode(TwoDimensionalCodeType type, string codeData, int width, int height)
        {
            return ByteSplicer.Combine(
                Set2DCodeWidth(type, width),
                Set2DCodeHeight(type, height),
                Print2DCode(type, codeData)
            );
        }
        public byte[] Set2DCodeWidth(TwoDimensionalCodeType type, int width) => new byte[] { Cmd.GS, Barcodes.TwoDimensionalCodeParen, Barcodes.TwoDimensionalCodeK, 
        (byte)width };
        public byte[] Set2DCodeHeight(TwoDimensionalCodeType type, int height) => new byte[] { Cmd.GS, Barcodes.SetBarcodeHeightInDots, (byte)height };
    }
}
