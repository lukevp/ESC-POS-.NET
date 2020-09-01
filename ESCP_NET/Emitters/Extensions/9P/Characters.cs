using System;
using ESCP_NET.Emitters.Extensions.Enums;

namespace ESCP_NET.Emitters.Extensions._9P
{
    public abstract partial class ESC9PExtension : BaseCommandEmitter.BaseCommandEmitter
    {
        public byte[] AssignCharacterTable(CharacterTables table, int characterTableNum)
        {
            if (characterTableNum < 0 || characterTableNum > 3) throw new ArgumentOutOfRangeException();
            var (d2, d3) = Util.GetD2D3FromCharacterTable(table);
            return new byte[]
                {AsciiTable.ESC, AsciiTable.LEFT_PARENTHESIS, AsciiTable.t, 3, 0, 1, (byte) characterTableNum, d2, d3};
        }
    }
}