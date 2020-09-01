using System;
using System.Collections.Generic;

namespace ESCP_NET.Emitters.Extensions._9P
{
    public abstract partial class ESC9PExtension
    {
        public byte[] SetN_216LineSpacing(byte space)
        {
            return new[] {AsciiTable.ESC, AsciiTable.THREE, space};
        }

        public byte[] SetN_72LineSpacing(byte space)
        {
            return new[] {AsciiTable.ESC, AsciiTable.A, space};
        }

        public byte[] Set7_72LineSpacing()
        {
            return new[] {AsciiTable.ESC, AsciiTable.ONE};
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
    }
}