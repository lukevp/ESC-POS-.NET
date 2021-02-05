namespace ESCPOS_NET.Emitters
{
    public abstract partial class BaseCommandEmitter<TCommandValues> : ICommandEmitter
    {
        /* Operational Commands */
        public virtual byte[] Initialize() => new byte[] { Values.ESC, Values.Initialize };

        public virtual byte[] Enable() => new byte[] { Values.ESC, Values.EnableDisable, 1 };

        public virtual byte[] Disable() => new byte[] { Values.ESC, Values.EnableDisable, 0 };
    }
}
