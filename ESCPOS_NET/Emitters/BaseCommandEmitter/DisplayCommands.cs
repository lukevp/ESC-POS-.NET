using ESCPOS_NET.Emitters.BaseCommandValues;

namespace ESCPOS_NET.Emitters
{
    public abstract partial class BaseCommandEmitter : ICommandEmitter
    {
        /* Display Commands */
        public virtual byte[] Clear() => new byte[] { Display.CLR };
    }
}
