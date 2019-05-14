using System.Collections.Generic;
using System.Text;

namespace ESCPOS_NET.Emitters
{
    public abstract partial class BaseCommandEmitter : ICommandEmitter
    {
        public byte[] PrintQrcode(string qrData, int size)
        {
            var command = new List<byte>();

            int store_len = qrData.Length + 3;
            byte store_pL = (byte)(store_len % 256);
            byte store_pH = (byte)(store_len / 256);

            command.AddRange(SetQrCodeModel());
            command.AddRange(Set2dSize(size));
            command.AddRange(Store2DData(store_pL, store_pH));
            command.AddRange(Encoding.GetEncoding("iso-8859-1").GetBytes(qrData));
            command.AddRange(PrintData());

            return command.ToArray();
        }

        //Function 165 
        private byte[] SetQrCodeModel() => new byte[] { Cmd.GS, QrCode.QrCodeCmd, 0x6B, 0x04, 0x00, 0x31, 0x41, 0x32, 0x00 };

        //Function 167 
        private byte[] Set2dSize(int size) => new byte[] { Cmd.GS, QrCode.QrCodeCmd, 0x6B, 0x03, 0x00, 0x31, 0x43, (byte)size };

        // Function 169
        private byte[] SetErrorCorrectionLevel() => new byte[] { Cmd.GS, QrCode.QrCodeCmd, 0x6B, 0x03, 0x00, 0x31, 0x45, 0x32 }; //TODO: colocar enumerador para nivel de correção

        // Function 180
        private byte[] Store2DData(byte store_pL, byte store_pH) => new byte[] { Cmd.GS, QrCode.QrCodeCmd, 0x6B, store_pL, store_pH, 0x31, 0x50, 0x30 };

        //Function 181
        private byte[] PrintData() => new byte[] { Cmd.GS, QrCode.QrCodeCmd, 0x6B, 0x03, 0x00, 0x31, 0x51, 0x30 };

        // Function 182
        private byte[] Transmit2dData() => new byte[] { Cmd.GS, QrCode.QrCodeCmd, 0x6B, 0x03, 0x00, 0x52, 0x30 };
    }
}
