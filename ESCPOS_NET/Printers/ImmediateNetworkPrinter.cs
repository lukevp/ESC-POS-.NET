using Microsoft.Extensions.Logging;
using SimpleTcp;
using System;
using System.IO;
using System.Threading.Tasks;
using System.Reflection;
using System.Net.Sockets;
using System.Threading;
using ESCPOS_NET.Utils;
using ESCPOS_NET.Utilities;
using ESCPOS_NET.Emitters;
using ESCPOS_NET.Printers;

namespace ESCPOS_NET
{
    public class ImmediateNetworkPrinterSettings
    {
        // Connection string is of the form printer_name:port or ip:port
        public string ConnectionString { get; set; }
        public uint? ReceiveTimeoutMs { get; set; }
        public uint? SendTimeoutMs { get; set; }
        public uint? ConnectTimeoutMs { get; set; }
        public string PrinterName { get; set; }
    }

    public class ImmediateNetworkPrinter : IImmediatePrinter
    {
        private readonly ImmediateNetworkPrinterSettings _settings;


        public ImmediateNetworkPrinter(ImmediateNetworkPrinterSettings settings)
        {
            _settings = settings;
            _settings.ReceiveTimeoutMs ??= 3000;
            _settings.ConnectTimeoutMs ??= 5000;
            _settings.SendTimeoutMs ??= 3000;
        }

        private async Task<TcpClient> ConnectAsync()
        {
            TcpClient client = new TcpClient();

            // ConnectionString is of the form ip:port, or hostname:port, or hostname.
            var hostnameOrIp = _settings.ConnectionString.Split(':')[0];
            var port = _settings.ConnectionString.Contains(":") ? int.Parse(_settings.ConnectionString.Split(':')[1]) : 9100;

            try
            {
                await CancelableTaskRunnerWithTimeout.RunTask(client.ConnectAsync(hostnameOrIp, port), (int)(_settings.ConnectTimeoutMs ?? 0), CancellationToken.None);
            }
            catch (Exception e)
            {
                Logging.Logger?.LogError(e, "[{Function}]:[{PrinterName}] Unable to connect to printer.", $"{this}.{MethodBase.GetCurrentMethod().Name}", _settings.PrinterName);
                client.Close();
                throw;
            }

            return client;
        }

        public async Task<bool> GetOnlineStatus(ICommandEmitter emitter)
        {
            return await GetOnlineStatus(emitter, null);
        }

        public async Task<bool> GetOnlineStatus(ICommandEmitter emitter, TcpClient client)
        {
            // Connect to printer, write bytes, and return when complete.
            var didCreateClient = false;
            if (client == null)
            {
                client = await ConnectAsync();
                didCreateClient = true;
            }
            var stream = client.GetStream();

            await WriteAsync(ByteSplicer.Combine(
                emitter.Initialize(),
                emitter.Enable(),
                emitter.RequestOnlineStatus()), client);

            var response = new byte[1];
            await CancelableTaskRunnerWithTimeout.RunTask(stream.ReadAsync(response, 0, 1), (int)(_settings.ReceiveTimeoutMs ?? 0), CancellationToken.None);

            if (didCreateClient)
            {
                client.Close();
            }
            return (response[0] & 0x08) == 0;
        }


        public async Task WriteAsync(byte[] bytes)
        {
            await WriteAsync(bytes, default(TcpClient));

        }

        public async Task WriteAsync(params byte[][] arrays)
        {
            await WriteAsync(ByteSplicer.Combine(arrays), default(TcpClient));
        }

        public async Task WriteAsync(byte[] bytes, TcpClient client)
        {
            // We have 2 ways to call this, one with the client supplied and one without.
            // If the client is sent in, we assume we shouldn't close it, so that it can be reused.
            // If we create it in this method, it should be closed.
            var didCreateClient = false;
            if (client == null)
            {
                client = await ConnectAsync();
                didCreateClient = true;
            }
            var stream = client.GetStream();

            await CancelableTaskRunnerWithTimeout.RunTask(stream.WriteAsync(bytes, 0, bytes.Length), (int)(_settings.SendTimeoutMs ?? 0), CancellationToken.None);

            if (didCreateClient)
            {
                client.Close();
            }
        }

    }
}
