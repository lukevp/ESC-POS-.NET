using System.Collections.Generic;
using ESCP_NET.Emitters.Enums;

namespace ESCP_NET.Emitters.BaseCommandEmitter
{
    public abstract partial class BaseCommandEmitter
    {
        public byte[] PrintBarcode(BarCodeType type, BarcodeModuleWidth moduleWidth, byte[] data)
        {
            var length = 6 + data.Length;
            var (nl, nh) = Util.SplitNumber(length);
            const int s = 0;
            const int c = 0;
            const int v1 = 21;
            const int v2 = 0;

            var command = new List<byte>
            {
                AsciiTable.ESC, AsciiTable.LEFT_PARENTHESIS, AsciiTable.B, (byte) nl, (byte) nh, (byte) type,
                (byte) moduleWidth, s, v1, v2, c
            };
            command.AddRange(data);

            return command.ToArray();
        }
    }
}