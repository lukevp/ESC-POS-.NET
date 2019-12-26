using System.Collections.Generic;
using System.Linq;

namespace ESCPOS_NET.Emitters
{
    public abstract partial class BaseCommandEmitter : ICommandEmitter
    {
        /* Display Commands */
        public byte[] Clear() => new byte[] { Display.CLR };
    }
}
