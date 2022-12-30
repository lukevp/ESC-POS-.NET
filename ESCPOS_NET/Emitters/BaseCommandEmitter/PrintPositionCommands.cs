using ESCPOS_NET.Emitters.BaseCommandValues;

namespace ESCPOS_NET.Emitters
{
    public abstract partial class BaseCommandEmitter : ICommandEmitter
    {
        public virtual byte[] SetLeftMargin(int leftMargin) => new byte[] { Cmd.GS, PrintPosition.LeftMargin, (byte)leftMargin, 0x0 };
    }
}
