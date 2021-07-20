using Microsoft.Extensions.Logging;
using SimpleTcp;
using System;
using System.IO;
using System.Net;
using System.Text.Json;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ESCPOS_NET
{
    public class NetworkPrinterSettings
    {
        public IPEndPoint EndPoint { get; set; }
        //public bool ReconnectOnTimeout { get; set; }
        //public uint? ReceiveTimeoutMs { get; set; }
        //public uint? SendTimeoutMs { get; set; }
        //public uint? ConnectTimeoutMs { get; set; }
        //public uint? ReconnectTimeoutMs { get; set; }
        //public uint? MaxReconnectAttempts { get; set; }
        public string PrinterName { get; set; }
    }
    public class NetworkPrinter : BasePrinter
    {
        private readonly NetworkPrinterSettings _settings;
        private TCPConnection _tcpConnection;

        private bool _isConnected = false;
        protected override bool IsConnected
        {
            get
            {
                return _isConnected;
            }
        }

        public NetworkPrinter(NetworkPrinterSettings settings) : base(settings.PrinterName)
        {
            _settings = settings;
            Task.Run(() => Connect());
        }

        private void Connected(object sender, ClientConnectedEventArgs e)
        {
            Logging.Logger?.LogInformation("[{PrinterName}] [{Function}]: Connected successfully to network printer! Settings: {Settings}", PrinterName, "Connected", JsonSerializer.Serialize(_settings));
            _isConnected = true;
        }
        private void Disconnected(object sender, ClientDisconnectedEventArgs e)
        {
            _isConnected = false;
            Logging.Logger?.LogWarning("[{PrinterName}] [{Function}]: Network printer connection terminated. Attempting to reconnect. Settings: {Settings}", PrinterName, "Disconnected", JsonSerializer.Serialize(_settings));
            //    Logging.Logger?.LogTrace($"[{PrinterName}] Reconnect: Reconnection attempt {reconnectAttempts}.");
            // Invoke reconnect attempt in a background thread so we don't block the event handler thread.
            Task.Run(() => { AttemptReconnectInfinitely(); });

        }
        private void AttemptReconnectInfinitely()
        {
            try
            {
                //_tcpConnection.ConnectWithRetries(300000);
                _tcpConnection.ConnectWithRetries(3000);
            }
            catch (TimeoutException)
            {
                Logging.Logger?.LogWarning("[{PrinterName}] [{Function}]: Network printer unable to connect after 5 minutes. Attempting to reconnect. Settings: {Settings}", PrinterName, "AttemptReconnectInfinitely", JsonSerializer.Serialize(_settings));

                Task.Run(async () => { await Task.Delay(250); AttemptReconnectInfinitely(); });
            }
        }

        private void Connect()
        {

            // instantiate
            _tcpConnection = new TCPConnection(_settings.EndPoint.ToString());

            // set events
            _tcpConnection.Connected += Connected;
            _tcpConnection.Disconnected += Disconnected;

            Reader = new BinaryReader(_tcpConnection.ReadStream);
            Writer = new BinaryWriter(_tcpConnection.WriteStream);

            AttemptReconnectInfinitely();

        }

        protected override void OverridableDispose()
        {
            _tcpConnection = null;
        }
    }
}
