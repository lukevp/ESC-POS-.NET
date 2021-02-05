namespace ESCPOS_NET.Emitters
{
    public abstract partial class BaseCommandEmitter<TCommandValues> : ICommandEmitter
    {
        /* Character Commands */
        public virtual byte[] SetStyles(PrintStyle style) => new byte[] { Values.ESC, Values.StyleMode, (byte)style };

        public virtual byte[] LeftAlign() => new byte[] { Values.ESC, Values.Alignment, (byte)Align.Left };

        public virtual byte[] CenterAlign() => new byte[] { Values.ESC, Values.Alignment, (byte)Align.Center };

        public virtual byte[] RightAlign() => new byte[] { Values.ESC, Values.Alignment, (byte)Align.Right };

        public virtual byte[] RightCharacterSpacing(int spaceCount) => new byte[] { Values.ESC, Values.RightCharacterSpacing, (byte)spaceCount };

        public virtual byte[] CodePage(CodePage codePage) => new byte[] { Values.ESC, Values.CodePage, (byte)codePage };
    }
}
