using ESCPOS_NET.Emitters.BaseCommandValues;

namespace ESCPOS_NET.Emitters
{
    public abstract partial class BaseCommandEmitter : ICommandEmitter
    {
        /* Cash Drawer Commands */
        public virtual byte[] CashDrawerOpenPin2(int pulseOnTimeMs = 120, int pulseOffTimeMs = 240) {
            return new byte[] { Cmd.ESC, Ops.CashDrawerPulse, 0x00, (byte) (pulseOnTimeMs / 2), (byte) (pulseOffTimeMs / 2) };
        }

        public virtual byte[] CashDrawerOpenPin5(int pulseOnTimeMs = 120, int pulseOffTimeMs = 240) {
            return new byte[] { Cmd.ESC, Ops.CashDrawerPulse, 0x01, (byte) (pulseOnTimeMs / 2), (byte) (pulseOffTimeMs / 2) };
        }
    }
}
