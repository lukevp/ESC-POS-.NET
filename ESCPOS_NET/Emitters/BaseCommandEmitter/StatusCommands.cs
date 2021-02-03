using ESCPOS_NET.Emitters.BaseCommandValues;

namespace ESCPOS_NET.Emitters
{
    public abstract partial class BaseCommandEmitter : ICommandEmitter
    {
        /* Status Commands */
        public virtual byte[] EnableAutomaticStatusBack() => new byte[] { Cmd.GS, Status.AutomaticStatusBack, 0xFF };

        public virtual byte[] EnableAutomaticInkStatusBack() => new byte[] { Cmd.GS, Status.AutomaticInkStatusBack, 0xFF };
    }
}
