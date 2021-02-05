namespace ESCPOS_NET.Emitters
{
    public abstract partial class BaseCommandEmitter<TCommandValues> : ICommandEmitter
    {
        /* Status Commands */
        public virtual byte[] EnableAutomaticStatusBack() => new byte[] { Values.GS, Values.AutomaticStatusBack, 0xFF };

        public virtual byte[] EnableAutomaticInkStatusBack() => new byte[] { Values.GS, Values.AutomaticInkStatusBack, 0xFF };
    }
}
