﻿using System;
using System.Linq;
using ESCPOS_NET.Emitters.BaseCommandValues;

namespace ESCPOS_NET.Emitters
{
    public abstract partial class BaseCommandEmitter : ICommandEmitter
    {
        /* Printing Commands */
        public virtual byte[] Print(string data)
        {
            if (data == null)
                throw new ArgumentNullException(nameof(data));

            // Fix OSX or Windows-style newlines
            data = data.Replace("\r\n", "\n");
            data = data.Replace("\r", "\n");

            // TODO: Sanitize...
            return data.ToCharArray().Select(x => (byte)x).ToArray();
        }

        public virtual byte[] PrintLine(string line)
        {
            if (line == null)
            {
                return Print("\n");
            }

            return Print(line.Replace("\r", string.Empty).Replace("\n", string.Empty) + "\n");
        }

        public virtual byte[] FeedLines(int lineCount) => new byte[] { Cmd.ESC, Whitespace.FeedLines, (byte)lineCount };

        public virtual byte[] FeedLinesReverse(int lineCount) => new byte[] { Cmd.ESC, Whitespace.FeedLinesReverse, (byte)lineCount };

        public virtual byte[] FeedDots(int dotCount) => new byte[] { Cmd.ESC, Whitespace.FeedDots, (byte)dotCount };

        public virtual byte[] ReverseMode(bool enable) => new byte[] { Cmd.GS, Chars.ReversePrintMode, enable ? (byte)0x01 : (byte)0x00 };

        public virtual byte[] UpsideDownMode(bool enable) => new byte[] { Cmd.ESC, Chars.UpsideDownMode, enable ? (byte)0x01 : (byte)0x00 };
    }
}
