using ESCPOS_NET.Emitters.BaseCommandValues;
using System;

namespace ESCPOS_NET.Emitters
{
    public abstract partial class BaseCommandEmitter : ICommandEmitter
    {
        public virtual byte[] SetLeftMargin(int leftMargin)
        {
            if (leftMargin < 0)
            {
                throw new ArgumentException($"Left margin '{leftMargin}' is lower than the minimum 0.");
            }
            else if (65535 < leftMargin)
            {
                throw new ArgumentException($"Left margin '{leftMargin}' is higher than the maximum 65535.");
            }

            return new byte[] { Cmd.GS, PrintPosition.LeftMargin, (byte)leftMargin, 0x0 };
        }
    }
}
