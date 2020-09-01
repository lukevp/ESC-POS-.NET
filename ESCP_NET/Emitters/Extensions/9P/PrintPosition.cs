using System;
using ESCP_NET.Emitters.Extensions.Enums;

namespace ESCP_NET.Emitters.Extensions._9P
{
    public abstract partial class ESC9PExtension
    {
        public byte[] HorizontalVerticalSkip(int many, SkipType type)
        {
            if (many < 0 || many > 127) throw new ArgumentOutOfRangeException("Expected range: 0 <= many <= 127");
            return new[] {AsciiTable.ESC, AsciiTable.f, (byte) many, (byte) type};
        }
    }
}