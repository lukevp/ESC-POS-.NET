using ESCPOS_NET.Emitters.BaseCommandValues;

namespace ESCPOS_NET.Emitters.BaseCommandEmitter
{
    public abstract partial class BaseCommandEmitter : ICommandEmitter
    {
        /* Status Commands */
        public byte[] EnableAutomaticStatusBack()
        {
            return new byte[] {Cmd.GS, Status.AutomaticStatusBack, 0xFF};
        }

        public byte[] EnableAutomaticInkStatusBack()
        {
            return new byte[] {Cmd.GS, Status.AutomaticInkStatusBack, 0xFF};
        }
    }
}