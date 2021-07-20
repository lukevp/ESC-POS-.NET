using Microsoft.Extensions.Logging;
using SimpleTcp;
using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ESCPOS_NET
{
    public class NetworkPrinterSettings
    {
        public IPEndPoint EndPoint;
        public bool ReconnectOnTimeout;
        public uint? ReceiveTimeoutMs;
        public uint? SendTimeoutMs;
        public uint? ConnectTimeoutMs;
        public uint? ReconnectTimeoutMs;
        public uint? MaxReconnectAttempts;
        public string PrinterName;
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
            _isConnected = true;
        }
        private void Disconnected(object sender, ClientDisconnectedEventArgs e)
        {
            _isConnected = false;
            // Invoke reconnect attempt in a background thread so we don't block the event handler thread.
            Task.Run(() => { AttemptReconnectInfinitely(); });

        }
        private void AttemptReconnectInfinitely()
        {
            try
            {
                _tcpConnection.ConnectWithRetries(60000);
            }
            catch (TimeoutException)
            {
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
