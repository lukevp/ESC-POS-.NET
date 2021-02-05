namespace ESCPOS_NET.Emitters
{
    public partial class BaseCommandValues : ICommandValues
    {
        public virtual byte ESC => 0x1B;
        public virtual byte GS => 0x1D;
    }
}
