namespace ESCPOS_NET.Emitters
{
    public partial class BaseCommandValues : ICommandValues
    {
        public virtual byte Initialize => 0x40;
        public virtual byte EnableDisable => 0x3D;
        public virtual byte PaperCut => 0x56;
        public virtual byte CashDrawerPulse => 0x70;
    }
}
