namespace ESCPOS_NET.Emitters
{
    public partial class BaseCommandValues : ICommandValues
    {
        public virtual byte PaperCutFullCut => 0x00;
        public virtual byte PaperCutFullCutWithFeed => 0x41;
        public virtual byte PaperCutPartialCut => 0x01;
        public virtual byte PaperCutPartialCutWithFeed => 0x42;
    }
}
