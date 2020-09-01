using ESCP_NET.Emitters.Enums;

namespace ESCP_NET.Emitters.BaseCommandEmitter
{
    public abstract partial class BaseCommandEmitter
    {
        public byte[] ControlPaperLoadingEjecting(ControlPaper control)
        {
            return new[] {AsciiTable.ESC, AsciiTable.EM, (byte) control};
        }

        public byte[] SetPrintingMode(PrintingMode mode)
        {
            return new[] {AsciiTable.ESC, AsciiTable.U, (byte) mode};
        }

        public byte[] Beep()
        {
            return new[] {AsciiTable.BEL};
        }
    }
}