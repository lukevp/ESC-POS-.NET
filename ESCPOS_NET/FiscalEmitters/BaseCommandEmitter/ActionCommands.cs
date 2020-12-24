using ESCPOS_NET.FiscalEmitters.BaseCommandValues;

namespace ESCPOS_NET.FiscalEmitters
{
    public abstract partial class BaseCommandEmitter : ICommandEmitter
    {
        /* Action Commands */
        public byte[] PaperCut() => CmdWrapper(Functions.PaperCut, Cmd.Empty);
        public byte[] PaperFeed(int lines = 1) => CmdWrapper(Functions.PaperFeed, new byte[] { (byte)lines });
    }
}
