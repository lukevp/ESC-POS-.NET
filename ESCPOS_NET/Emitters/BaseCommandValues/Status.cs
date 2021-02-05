namespace ESCPOS_NET.Emitters
{
    public partial class BaseCommandValues : ICommandValues
    {
        public virtual byte AutomaticStatusBack => 0x61;
        public virtual byte AutomaticInkStatusBack => 0x6A;
    }
}
