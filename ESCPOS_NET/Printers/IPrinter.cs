using System;

namespace ESCPOS_NET
{
    public interface IPrinter
    {
        PrinterStatusEventArgs GetStatus();
        void Write(params byte[][] arrays);
        void Write(byte[] bytes);
        event EventHandler StatusChanged;
        event EventHandler Disconnected;
        event EventHandler Connected;
        //event EventHandler WriteFailed;
        //event EventHandler Idle;
        //event EventHandler IdleDisconnected; is this useful? to know that it disconnected because of idle? probably better to have this as info in disconnected event object instead.
    }
}