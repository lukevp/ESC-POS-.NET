using ESCPOS_NET.Emitters.BaseCommandValues;

namespace ESCPOS_NET.Emitters
{
    public abstract partial class BaseCommandEmitter : ICommandEmitter
    {
        /* Operational Commands */
        public virtual byte[] Initialize() => new byte[] { Cmd.ESC, Ops.Initialize };

        public virtual byte[] Enable() => new byte[] { Cmd.ESC, Ops.EnableDisable, 1 };

        public virtual byte[] Disable() => new byte[] { Cmd.ESC, Ops.EnableDisable, 0 };

        public virtual byte[] StandardMode() => new byte[] { Cmd.ESC, Ops.StandardMode };
    }
}
