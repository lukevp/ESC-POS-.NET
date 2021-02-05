namespace ESCPOS_NET.Emitters
{
    public abstract partial class BaseCommandEmitter<TCommandValues> : ICommandEmitter
    {
        /* Display Commands */
        public virtual byte[] Clear() => new byte[] { Values.CLR };
    }
}
