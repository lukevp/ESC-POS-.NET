using ESCPOS_NET.Emitters.BaseCommandValues;

namespace ESCPOS_NET.Emitters.BaseCommandEmitter
{
    public abstract partial class BaseCommandEmitter : ICommandEmitter
    {
        public byte[] ResetLineSpacing()
        {
            return new[] {Cmd.ESC, Whitespace.ResetLineSpacing};
        }

        public byte[] SetLineSpacingInDots(int dots)
        {
            return new[] {Cmd.ESC, Whitespace.LineSpacingInDots, (byte) dots};
        }
    }
}