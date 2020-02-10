namespace ESCPOS_NET.Emitters
{
    public abstract partial class BaseCommandEmitter : ICommandEmitter
    {
        public byte[] ResetLineSpacing() => new byte[] { Cmd.ESC, Whitespace.ResetLineSpacing };
        public byte[] SetLineSpacingInDots(int dots) => new byte[] { Cmd.ESC, Whitespace.LineSpacingInDots, (byte)dots };
    }
}
