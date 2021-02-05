namespace ESCPOS_NET.Emitters
{
    public partial class BaseCommandValues : ICommandValues
    {
        public virtual byte StyleMode => 0x21;
        public virtual byte Alignment => 0x61;
        public virtual byte ReversePrintMode => 0x42;
        public virtual byte RightCharacterSpacing => 0x20;
        public virtual byte UpsideDownMode => 0x7B;
        public virtual byte CodePage => 0x74;
    }
}
