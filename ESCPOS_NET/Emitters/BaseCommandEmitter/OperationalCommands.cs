using ESCPOS_NET.Emitters.BaseCommandValues;

namespace ESCPOS_NET.Emitters.BaseCommandEmitter
{
    public abstract partial class BaseCommandEmitter : ICommandEmitter
    {
        /* Operational Commands */
        public byte[] Initialize()
        {
            return new[] {Cmd.ESC, Ops.Initialize};
        }

        public byte[] Enable()
        {
            return new byte[] {Cmd.ESC, Ops.EnableDisable, 1};
        }

        public byte[] Disable()
        {
            return new byte[] {Cmd.ESC, Ops.EnableDisable, 0};
        }
    }
}