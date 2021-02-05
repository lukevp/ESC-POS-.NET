namespace ESCPOS_NET.Emitters
{
    public abstract partial class BaseCommandEmitter<TCommandValues> : ICommandEmitter
    {
        /* Cash Drawer Commands */
        public virtual byte[] CashDrawerOpenPin2() => new byte[] { Values.ESC, Values.CashDrawerPulse, 0x00 };

        public virtual byte[] CashDrawerOpenPin5() => new byte[] { Values.ESC, Values.CashDrawerPulse, 0x01 };
    }
}
