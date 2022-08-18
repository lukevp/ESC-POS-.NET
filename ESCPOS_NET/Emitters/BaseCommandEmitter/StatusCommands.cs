using ESCPOS_NET.Emitters.BaseCommandValues;

namespace ESCPOS_NET.Emitters
{
    public abstract partial class BaseCommandEmitter : ICommandEmitter
    {
        /* Status Commands */
        public virtual byte[] EnableAutomaticStatusBack() => new byte[] { Cmd.GS, Status.AutomaticStatusBack, 0xFF };

        public virtual byte[] EnableAutomaticInkStatusBack() => new byte[] { Cmd.GS, Status.AutomaticInkStatusBack, 0xFF };
        public virtual byte[] DisableAutomaticStatusBack() => new byte[] { Cmd.GS, Status.AutomaticStatusBack, 0x00 };

        public virtual byte[] DisableAutomaticInkStatusBack() => new byte[] { Cmd.GS, Status.AutomaticInkStatusBack, 0x00 };
        public virtual byte[] RequestOnlineStatus() => new byte[] { Cmd.DLE, Status.RealtimeStatus, 0x01 };
        public virtual byte[] RequestPaperStatus() => new byte[] { Cmd.GS, Status.RequestStatus, 0x01 };
        public virtual byte[] RequestDrawerStatus() => new byte[] { Cmd.GS, Status.RequestStatus, 0x02 };
        public virtual byte[] RequestInkStatus() => new byte[] { Cmd.GS, Status.RequestStatus, 0x04 };

    }
}
