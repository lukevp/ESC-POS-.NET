using ESCPOS_NET.Emitters.BaseCommandValues;

namespace ESCPOS_NET.Emitters
{
    public abstract partial class BaseCommandEmitter : ICommandEmitter
    {
        /* Cash Drawer Commands */
        public virtual byte[] CashDrawerOpenPin2() => new byte[] { Cmd.ESC, Ops.CashDrawerPulse, 0x00 };

        public virtual byte[] CashDrawerOpenPin5() => new byte[] { Cmd.ESC, Ops.CashDrawerPulse, 0x01 };
    }
}
