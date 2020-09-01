using ESCPOS_NET.Emitters;

namespace ESCPOS_NET.ConsoleTest
{
    public static partial class Tests
    {
        public static byte[] CashDrawerOpenPin2(ICommandEmitter e)
        {
            return ByteSplicer.Combine(
                e.CashDrawerOpenPin2(), // Fire this twice ensures it's operation 
                e.CashDrawerOpenPin2());
        }

        public static byte[] CashDrawerOpenPin5(ICommandEmitter e)
        {
            return ByteSplicer.Combine(
                e.CashDrawerOpenPin5(), // Fire this twice ensures it's operation 
                e.CashDrawerOpenPin5());
        }
    }
}