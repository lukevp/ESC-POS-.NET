using System;
using System.Collections.Generic;
using ESCP_NET.Emitters.Extensions.Enums;

namespace ESCP_NET.Emitters.Extensions.P
{
    public abstract class ESCPExtension : BaseCommandEmitter.BaseCommandEmitter
    {
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

        public byte[] SetVerticalTabsInVFUChannel(int m, params byte[] tabs)
        {
            if (tabs.Length > 16) throw new ArgumentOutOfRangeException("Too many tabs, 16 max");
            if (m < 0 || m > 7) throw new ArgumentOutOfRangeException("Expected range: 0 ≤ m ≤ 7");

            var command = new List<byte> {AsciiTable.ESC, AsciiTable.b, (byte) m};
            command.AddRange(tabs);
            command.Add(AsciiTable.NUL);

            return command.ToArray();
        }

        public byte[] SelectVerticalTabChannel(int m)
        {
            if (m < 0 || m > 7) throw new ArgumentOutOfRangeException("Expected range: 0 ≤ m ≤ 7");

            return new[] {AsciiTable.ESC, AsciiTable.SLASH, (byte) m, AsciiTable.NUL};
        }

        public byte[] SelectJustification(Justification justification)
        {
            return new[] {AsciiTable.ESC, AsciiTable.a, (byte) justification};
        }
    }
}