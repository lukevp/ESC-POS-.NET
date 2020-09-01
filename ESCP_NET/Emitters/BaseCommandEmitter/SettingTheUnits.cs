using System;
using System.Collections.Generic;

namespace ESCP_NET.Emitters.BaseCommandEmitter
{
    public abstract partial class BaseCommandEmitter
    {
        public byte[] Select1_8LineSpacing()
        {
            return new[] {AsciiTable.ESC, AsciiTable.ZERO};
        }

        public byte[] Select1_6LineSpacing()
        {
            return new[] {AsciiTable.ESC, AsciiTable.TWO};
        }

        public byte[] SetHorizontalTabs(params byte[] tabs)
        {
            if (tabs.Length > 32) throw new ArgumentOutOfRangeException("Too many tabs");
            var command = new List<byte> {AsciiTable.ESC, AsciiTable.D};
            command.AddRange(tabs);
            command.Add(AsciiTable.NUL);
            return command.ToArray();
        }

        public byte[] SetVerticalTabs(params byte[] tabs)
        {
            if (tabs.Length > 16) throw new ArgumentOutOfRangeException("Too many tabs");
            var command = new List<byte> {AsciiTable.ESC, AsciiTable.B};
            command.AddRange(tabs);
            command.Add(AsciiTable.NUL);
            return command.ToArray();
        }
    }
}