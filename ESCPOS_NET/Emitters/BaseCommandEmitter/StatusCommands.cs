using System.Collections.Generic;
using System.Linq;

namespace ESCPOS_NET.Emitters
{
    public abstract partial class BaseCommandEmitter : ICommandEmitter
    {
        /* Status Commands */

        // Default this to ASB all statuses (Drawer Kick-Out Connector, Online, Error, Roll Paper Sensor, and Panel Switch).
        // All 0s are unused bytes.
        public byte[] EnableAutomaticStatusBack() => new byte[] { Cmd.GS, Status.AutomaticStatusBack, 0b11110010 };
    }
}
