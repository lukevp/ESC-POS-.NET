namespace ESCPOS_NET.Emitters
{
    public abstract partial class BaseCommandEmitter<TCommandValues> : ICommandEmitter
    {
        public virtual byte[] ResetLineSpacing() => new byte[] { Values.ESC, Values.ResetLineSpacing };

        public virtual byte[] SetLineSpacingInDots(int dots) => new byte[] { Values.ESC, Values.LineSpacingInDots, (byte)dots };
    }
}
