using System;
using System.Linq;
using ESCP_NET.Emitters.Enums;

namespace ESCP_NET.Emitters.BaseCommandEmitter
{
    public abstract partial class BaseCommandEmitter
    {
        public byte[] Print(string data)
        {
            // Fix OSX or Windows-style newlines
            data = data.Replace("\r\n", "\n");
            data = data.Replace("\r", "\n");

            // TODO: Sanitize...
            return data.ToCharArray().Select(x => (byte) x).ToArray();
        }

        public byte[] PrintLine(string line)
        {
            if (line == null) return Print("\n");

            return Print(line.Replace("\r", string.Empty).Replace("\n", string.Empty) + "\n");
        }

        public byte[] SelectCharacterTable(CharacterTable table)
        {
            return new[] {AsciiTable.ESC, AsciiTable.t, (byte) table};
        }

        public byte[] SelectAnInternationalCharacterSet(InternationalCharacterSet set)
        {
            return new[] {AsciiTable.ESC, AsciiTable.R, (byte) set};
        }

        // User defined characters is missing
        public byte[] CopyRomToRam(int amount)
        {
            return new[] {AsciiTable.ESC, AsciiTable.COLON, AsciiTable.NUL, (byte) amount};
        }

        public byte[] SelectUserDefinedSet(UserSets set)
        {
            return new[] {AsciiTable.ESC, AsciiTable.PRECENT, (byte) set};
        }

        public byte[] SelectTypeface(Typeface typeface)
        {
            return new[] {AsciiTable.ESC, AsciiTable.k, (byte) typeface};
        }

        public byte[] SetIntercharacterSpace(int space)
        {
            if (space < 0 || space > 127) throw new ArgumentOutOfRangeException("Expected 0 <= n <= 127");
            return new[] {AsciiTable.ESC, AsciiTable.SPACE, (byte) space};
        }

        public byte[] SetPrintStyle(PrintStyle style)
        {
            return new[] {AsciiTable.ESC, AsciiTable.EXCLAMATION_MARK, (byte) style};
        }

        public byte[] SelectScriptPrintingType(SupSuberScript script)
        {
            if (script == SupSuberScript.None) return new[] {AsciiTable.ESC, AsciiTable.T};
            return new[] {AsciiTable.ESC, AsciiTable.S, (byte) script};
        }

        public byte[] SelectCharacterStyle(CharacterStyle style)
        {
            return new[] {AsciiTable.ESC, AsciiTable.q, (byte) style};
        }

        public byte[] SelectPrintQuality(PrintQuality quality)
        {
            return new[] {AsciiTable.ESC, AsciiTable.x, (byte) quality};
        }

        public byte[] Select10Cpi()
        {
            return new[] {AsciiTable.ESC, AsciiTable.P};
        }

        public byte[] Select12Cpi()
        {
            return new[] {AsciiTable.ESC, AsciiTable.M};
        }

        public byte[] Select15Cpi()
        {
            return new[] {AsciiTable.ESC, AsciiTable.g};
        }
    }
}