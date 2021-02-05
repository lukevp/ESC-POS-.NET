namespace ESCPOS_NET.Emitters
{
    public abstract partial class BaseCommandEmitter<TCommandValues> : ICommandEmitter
    {
        /* Action Commands */
        public virtual byte[] FullCut() => new byte[] { Values.GS, Values.PaperCut, Values.PaperCutFullCut };

        public virtual byte[] PartialCut() => new byte[] { Values.GS, Values.PaperCut, Values.PaperCutPartialCut };

        public virtual byte[] FullCutAfterFeed(int lineCount) => new byte[] { Values.GS, Values.PaperCut, Values.PaperCutFullCutWithFeed, (byte)lineCount };

        public virtual byte[] PartialCutAfterFeed(int lineCount) => new byte[] { Values.GS, Values.PaperCut, Values.PaperCutPartialCutWithFeed, (byte)lineCount };
    }
}
