using ESCPOS_NET.Emitters.BaseCommandValues;

namespace ESCPOS_NET.Emitters.BaseCommandEmitter
{
    public abstract partial class BaseCommandEmitter : ICommandEmitter
    {
        /* Action Commands */
        public byte[] FullCut()
        {
            return new[] {Cmd.GS, Ops.PaperCut, Functions.PaperCutFullCut};
        }

        public byte[] PartialCut()
        {
            return new[] {Cmd.GS, Ops.PaperCut, Functions.PaperCutPartialCut};
        }

        public byte[] FullCutAfterFeed(int lineCount)
        {
            return new[] {Cmd.GS, Ops.PaperCut, Functions.PaperCutFullCutWithFeed, (byte) lineCount};
        }

        public byte[] PartialCutAfterFeed(int lineCount)
        {
            return new[] {Cmd.GS, Ops.PaperCut, Functions.PaperCutPartialCutWithFeed, (byte) lineCount};
        }
    }
}