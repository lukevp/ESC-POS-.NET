namespace ESCPOS_NET.Emitters
{
    public abstract partial class BaseCommandEmitter : ICommandEmitter
    {
        /* Operational Commands */
        public byte[] Initialize() => new byte[] { Cmd.ESC, Ops.Initialize };
        public byte[] Enable() => new byte[] { Cmd.ESC, Ops.EnableDisable, 1 };
        public byte[] Disable() => new byte[] { Cmd.ESC, Ops.EnableDisable, 0 };
    }
}
