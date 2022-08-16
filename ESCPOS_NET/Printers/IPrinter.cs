using System;

namespace ESCPOS_NET
{
    public interface IPrinter
    {
        PrinterStatusEventArgs GetStatus();
        void Write(params byte[][] arrays);
        void Write(byte[] bytes);
    }
}