using ESCPOS_NET.Emitters.BaseCommandValues;

namespace ESCPOS_NET.Emitters.BaseCommandEmitter
{
    public abstract partial class BaseCommandEmitter : ICommandEmitter
    {
        /* Cash Drawer Commands */
        public byte[] CashDrawerOpenPin2()
        {
            return new byte[] {Cmd.ESC, Ops.CashDrawerPulse, 0x00};
        }

        public byte[] CashDrawerOpenPin5()
        {
            return new byte[] {Cmd.ESC, Ops.CashDrawerPulse, 0x01};
        }
    }
}