using ESCPOS_NET.Emitters.BaseCommandValues;
using ESCPOS_NET.Emitters.Enums;

namespace ESCPOS_NET.Emitters.BaseCommandEmitter
{
    public abstract partial class BaseCommandEmitter : ICommandEmitter
    {
        /* Character Commands */
        public byte[] SetStyles(PrintStyle style)
        {
            return new[] {Cmd.ESC, Chars.StyleMode, (byte) style};
        }

        public byte[] LeftAlign()
        {
            return new[] {Cmd.ESC, Chars.Alignment, (byte) Align.Left};
        }

        public byte[] CenterAlign()
        {
            return new[] {Cmd.ESC, Chars.Alignment, (byte) Align.Center};
        }

        public byte[] RightAlign()
        {
            return new[] {Cmd.ESC, Chars.Alignment, (byte) Align.Right};
        }

        public byte[] RightCharacterSpacing(int spaceCount)
        {
            return new[] {Cmd.ESC, Chars.RightCharacterSpacing, (byte) spaceCount};
        }

        public byte[] CodePage(CodePage codePage)
        {
            return new[] {Cmd.ESC, Chars.CodePage, (byte) codePage};
        }
    }
}