using ESCPOS_NET.Emitters.BaseCommandValues;

namespace ESCPOS_NET.Emitters
{
    public abstract partial class BaseCommandEmitter : ICommandEmitter
    {
        /* Character Commands */
        /// <summary>
        /// Select print mode(s)
        /// </summary>
        /// <param name="style"></param>
        /// <returns></returns>
        public virtual byte[] SetStyles(PrintStyle style) => new byte[] { Cmd.ESC, Chars.StyleMode, (byte)style };

        /// <summary>
        /// Select character size. Please reference <see cref="https://reference.epson-biz.com/modules/ref_escpos/index.php?content_id=34"/>
        /// Value from 0 to 7, 0 for normal size.
        /// </summary>
        /// <param name="widthMagnification">Enlargement in horizontal direction.</param>
        /// <param name="heightMagnification">Enlargement in vertical direction.</param>
        /// <returns></returns>
        public virtual byte[] SetSize(byte widthMagnification, byte heightMagnification)
        {
            if (widthMagnification > 7) widthMagnification = 7;
            if (heightMagnification > 7) heightMagnification = 7;

            return [Cmd.GS, Chars.SizeMode, (byte)(widthMagnification * 0x10 + heightMagnification)];
        }

        /// <summary>
        /// Select character size. Please reference <see cref="https://reference.epson-biz.com/modules/ref_escpos/index.php?content_id=34"/>
        /// Set both width and height magnification in same value.
        /// </summary>
        /// <param name="magnification">Enlargement in both horizontal and vertical direction.</param>
        /// <returns></returns>
        public virtual byte[] SetSize(byte magnification) => SetSize(magnification, magnification);

        public virtual byte[] LeftAlign() => new byte[] { Cmd.ESC, Chars.Alignment, (byte)Align.Left };

        public virtual byte[] CenterAlign() => new byte[] { Cmd.ESC, Chars.Alignment, (byte)Align.Center };

        public virtual byte[] RightAlign() => new byte[] { Cmd.ESC, Chars.Alignment, (byte)Align.Right };

        public virtual byte[] LeftAlignAlt() => new byte[] { Cmd.ESC, Chars.Alignment, (byte)Align.LeftAlt };

        public virtual byte[] CenterAlignAlt() => new byte[] { Cmd.ESC, Chars.Alignment, (byte)Align.CenterAlt };

        public virtual byte[] RightAlignAlt() => new byte[] { Cmd.ESC, Chars.Alignment, (byte)Align.RightAlt };

        public virtual byte[] RightCharacterSpacing(int spaceCount) => new byte[] { Cmd.ESC, Chars.RightCharacterSpacing, (byte)spaceCount };

        public virtual byte[] CodePage(CodePage codePage) => new byte[] { Cmd.ESC, Chars.CodePage, (byte)codePage };

        public virtual byte[] Color(Color color) => new byte[] { Cmd.ESC, Chars.Color, (byte)color };
    }
}
