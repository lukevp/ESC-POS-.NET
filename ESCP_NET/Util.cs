using System;
using System.Linq;
using ESCP_NET.Emitters.Extensions.Enums;

namespace ESCP_NET
{
    public static class Util
    {
        public static (byte d2, byte d3) GetD2D3FromCharacterTable(CharacterTables table)
        {
            return table switch
            {
                CharacterTables.Italic => (0, 0),
                CharacterTables.PC437_US => (1, 0),
                CharacterTables.PC437_GREEK => (1, 16),
                CharacterTables.PC932_JAPANESE => (2, 0),
                CharacterTables.PC850_MULTILANGUAL => (3, 0),
                CharacterTables.PC851_GREEK => (4, 0),
                CharacterTables.PC853_TURKISH => (5, 0),
                CharacterTables.PC855_CYRYLLIC => (6, 0),
                CharacterTables.PC860_PORTUGAL => (7, 0),
                CharacterTables.PC863_CANADA_FRENCH => (8, 0),
                CharacterTables.PC865_NORWAY => (9, 0),
                CharacterTables.PC852_EAST_EUROPE => (10, 0),
                CharacterTables.PC857_TURKISH => (11, 0),
                CharacterTables.PC862_HEBREW => (12, 0),
                CharacterTables.PC864_ARABIC => (13, 0),
                CharacterTables.PC_AR864 => (13, 32),
                CharacterTables.PC866_RUSSIAN => (14, 0),
                CharacterTables.BULGARIAN_ASCII => (14, 16),
                CharacterTables.PC866_LATVIAN => (14, 32),
                CharacterTables.PC869_GREEK => (15, 0),
                CharacterTables.USSR_GOST => (16, 0),
                CharacterTables.ECMA_94_1 => (17, 0),
                CharacterTables.KU42 => (18, 0),
                CharacterTables.TIS11 => (19, 0),
                CharacterTables.TIS18 => (20, 0),
                CharacterTables.TIS17 => (21, 0),
                CharacterTables.TIS13 => (22, 0),
                CharacterTables.TIS16 => (23, 0),
                CharacterTables.PC861_ICELAND => (24, 0),
                CharacterTables.BRASCII => (25, 0),
                CharacterTables.Abicomp => (26, 0),
                CharacterTables.MAZOWIA => (27, 0),
                CharacterTables.Code_MJK => (28, 0),
                CharacterTables.ISO8859_7 => (29, 7),
                CharacterTables.ISO8859_1 => (29, 16),
                CharacterTables.TSM_WIN => (30, 0),
                CharacterTables.ISO_Latin_1T => (31, 0),
                CharacterTables.Bulgaria => (32, 0),
                CharacterTables.Hebrew_7 => (33, 0),
                CharacterTables.Hebrew_8 => (34, 0),
                CharacterTables.Roman_8 => (35, 0),
                CharacterTables.PC774_LITHUANIA => (36, 0),
                CharacterTables.Estonia => (37, 0),
                CharacterTables.ISCII => (38, 0),
                CharacterTables.PC_ISCII => (39, 0),
                CharacterTables.PC_APTEC => (40, 0),
                CharacterTables.PC708 => (41, 0),
                CharacterTables.PC720 => (42, 0),
                CharacterTables.OCR_B => (112, 0),
                CharacterTables.ISO_Latin_1 => (127, 1),
                CharacterTables.ISO_8859_2 => (127, 2),
                CharacterTables.ISO_Latin_7 => (127, 7),
                _ => throw new ArgumentOutOfRangeException()
            };
        }

        public static int GetFractionPart(double number)
        {
            var converted = number.ToString();
            var fraction = converted.Split('.').Last();
            var result = int.Parse(fraction);
            return result;
        }

        public static (int lower, int higher) SplitNumber(int number)
        {
            var higher = number / 256;
            var lower = GetFractionPart(number);
            return (lower, higher);
        }

        public static (int lower, int higher) SplitNumber(double number)
        {
            var higher = (int) (number / 256);
            var lower = GetFractionPart(number);
            return (lower, higher);
        }
    }
}