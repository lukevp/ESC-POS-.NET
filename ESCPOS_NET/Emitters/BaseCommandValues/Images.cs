namespace ESCPOS_NET.Emitters
{
    public partial class BaseCommandValues : ICommandValues
    {
        public virtual byte ImageCmdParen => 0x28;
        public virtual byte ImageCmdLegacy => 0x76;
        public virtual byte ImageCmd8 => 0x38;
        public virtual byte ImageCmdL => 0x4C;
    }
}
