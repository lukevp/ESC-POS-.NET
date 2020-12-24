using ESCPOS_NET.FiscalEmitters.BaseCommandValues;
using ESCPOS_NET.Utilities;
using System;
using System.Linq;

namespace ESCPOS_NET.FiscalEmitters
{
    public abstract partial class BaseCommandEmitter : ICommandEmitter
    {
        public byte[] PrintNonFiscalReceipt(string text)
        {
            var open = CmdWrapper(NonFiscal.OpenReceipt, Cmd.Empty);
            var close = CmdWrapper(NonFiscal.CloseReceipt, Cmd.Empty);
            var bab = new ByteArrayBuilder();
            do
            {
                int chunkSize = Math.Min(48, text.Length);
                var str = text.Substring(0, chunkSize);

                // try to find a line break before the chunk size
                var idx = str.IndexOf('\n');
                var jmp = 0;
                if (idx > 0)
                {
                    str = str.Substring(0, idx);
                    jmp = 1;
                }

                bab.Append(CmdWrapper(NonFiscal.PrintText, str.ToCharArray().Select(x => (byte)x)));

                text = text.Substring(jmp + str.Length - 1);

                // Last Chunck
                if (text.Length <= 48)
                {
                    bab.Append(CmdWrapper(NonFiscal.PrintText, text.ToCharArray().Select(x => (byte)x)));
                }
            } while (text.Length > 48);

            return ByteSplicer.Combine(open, bab.ToArray(), close);
        }
    }
}
