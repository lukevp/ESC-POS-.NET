namespace ESCPOS_NET.Emitters
{
    public abstract partial class BaseCommandEmitter : ICommandEmitter
    {
        /* Character Commands */
        public byte[] SetStyles(PrintStyle style) => new byte[] { Cmd.ESC, Chars.StyleMode, (byte)style };
        public byte[] LeftAlign() => new byte[] { Cmd.ESC, Chars.Alignment, (byte)Align.Left };
        public byte[] CenterAlign() => new byte[] { Cmd.ESC, Chars.Alignment, (byte)Align.Center };
        public byte[] RightAlign() => new byte[] { Cmd.ESC, Chars.Alignment, (byte)Align.Right };
        public byte[] RightCharacterSpacing(int spaceCount) => new byte[] { Cmd.ESC, Chars.RightCharacterSpacing, (byte)spaceCount };
    }
}
