using ESCPOS_NET.Emitters.BaseCommandValues;

namespace ESCPOS_NET.Emitters.BaseCommandEmitter
{
    public abstract partial class BaseCommandEmitter : ICommandEmitter
    {
        /* Display Commands */
        public byte[] Clear()
        {
            return new[] {Display.CLR};
        }
    }
}