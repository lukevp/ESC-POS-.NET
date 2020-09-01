using ESCP_NET.Emitters.Extensions.Enums;

namespace ESCP_NET.Emitters.Extensions.P2
{
    public abstract partial class ESCP2Extension
    {
        public byte[] SetUnit(DefinedUnits unit)
        {
            _definedUnit = (int) unit;
            return new byte[] {AsciiTable.ESC, AsciiTable.LEFT_PARENTHESIS, AsciiTable.U, 1, 0, (byte) unit};
        }

        public byte[] SetN_180LineSpacing(byte space)
        {
            return new[] {AsciiTable.ESC, AsciiTable.THREE, space};
        }

        public byte[] SetN_360LineSpacing(byte space)
        {
            return new[] {AsciiTable.ESC, AsciiTable.PLUS, space};
        }

        public byte[] SetN_60LineSpacing(byte space)
        {
            return new[] {AsciiTable.ESC, AsciiTable.A, space};
        }
    }
}