using System.Collections.Generic;
using System.Linq;

namespace ESCPOS_NET.Emitters
{
    public abstract partial class BaseCommandEmitter : ICommandEmitter
    {
        /* Action Commands */
        public byte[] FullCut() => new byte[] { Cmd.GS, Ops.PaperCut, Functions.PaperCutFullCut };
        public byte[] PartialCut() => new byte[] { Cmd.GS, Ops.PaperCut, Functions.PaperCutPartialCut };
        public byte[] FullCutAfterFeed(int lineCount) => new byte[] { Cmd.GS, Ops.PaperCut, Functions.PaperCutFullCutWithFeed, (byte)lineCount };
        public byte[] PartialCutAfterFeed(int lineCount) => new byte[] { Cmd.GS, Ops.PaperCut, Functions.PaperCutPartialCutWithFeed, (byte)lineCount };
    }
}
