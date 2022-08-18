using System;
using System.Net.Sockets;
using System.Threading.Tasks;
using ESCPOS_NET.Emitters;
using ESCPOS_NET.Printers;

namespace ESCPOS_NET
{
    // An ImmediatePrinter is a printer that connects to a printer,
    // writes/reads data, and then immediately disconnects.
    // It doesn't queue writes or reads, or keep a connection open to the printer persistently.
    // Therefore, it should expose async methods because the processing may take some time.
    public interface IImmediatePrinter
    {
        Task<bool> GetOnlineStatus(ICommandEmitter emitter);
        Task WriteAsync(params byte[][] arrays);
        Task WriteAsync(byte[] bytes);
    }
}