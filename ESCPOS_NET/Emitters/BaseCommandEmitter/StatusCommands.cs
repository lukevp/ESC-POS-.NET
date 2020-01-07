namespace ESCPOS_NET.Emitters
{
    public abstract partial class BaseCommandEmitter : ICommandEmitter
    {
        /* Status Commands */

        public byte[] EnableAutomaticStatusBack() => new byte[] { Cmd.GS, Status.AutomaticStatusBack, 0xFF };

        public byte[] EnableAutomaticInkStatusBack() => new byte[] { Cmd.GS, Status.AutomaticInkStatusBack, 0xFF };
    }
}
