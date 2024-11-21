using Microsoft.Extensions.Logging;
using SimpleTcp;
using System;
using System.IO;
using System.Threading.Tasks;
using System.Reflection;
using System.Net.Sockets;

namespace ESCPOS_NET
{
    public class NetworkPrinterSettings
    {
        // Connection string is of the form printer_name:port or ip:port
        public string ConnectionString { get; set; }
        public EventHandler ConnectedHandler { get; set; }
        public EventHandler DisconnectedHandler { get; set; }
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

        protected override bool IsConnected => _tcpConnection?.IsConnected??false;

        public NetworkPrinter(NetworkPrinterSettings settings) : base(settings.PrinterName)
        {
            _settings = settings;
            if (settings.ConnectedHandler != null)
            {
                Connected += settings.ConnectedHandler;
            }
            if (settings.DisconnectedHandler != null)
            {
                Disconnected += settings.DisconnectedHandler;
            }
            Connect();
        }

        private void ConnectedEvent(object sender, ClientConnectedEventArgs e)
        {
            Logging.Logger?.LogInformation("[{Function}]:[{PrinterName}] Connected successfully to network printer! Connection String: {ConnectionString}", $"{this}.{MethodBase.GetCurrentMethod().Name}", PrinterName, _settings.ConnectionString);
            // Close previously created reader and writer if any (for handling reconnect after connection lose (in the future))
            Reader?.Close();
            Writer?.Close();
            Reader = new BinaryReader(_tcpConnection.ReadStream);
            Writer = new BinaryWriter(_tcpConnection.WriteStream);
            InvokeConnect();
        }
        private void DisconnectedEvent(object sender, ClientDisconnectedEventArgs e)
        {
            InvokeDisconnect();
            Logging.Logger?.LogWarning("[{Function}]:[{PrinterName}] Network printer connection terminated. Attempting to reconnect. Connection String: {ConnectionString}", $"{this}.{MethodBase.GetCurrentMethod().Name}", PrinterName, _settings.ConnectionString);
            Connect();
        }
        private async ValueTask AttemptReconnectInfinitely()
        {
            try
            {
                //_tcpConnection.ConnectWithRetries(300000);
                _tcpConnection.ConnectWithRetries(3000);
            }
            catch
            {
                //Logging.Logger?.LogWarning("[{Function}]:[{PrinterName}] Network printer unable to connect after 5 minutes. Attempting to reconnect. Settings: {Settings}", $"{this}.{MethodBase.GetCurrentMethod().Name}", PrinterName, JsonSerializer.Serialize(_settings));
                await Task.Delay(250);
                CreateTcpConnection();
                await AttemptReconnectInfinitely();
            }
        }

        private void Connect()
        {
            CreateTcpConnection();
            try
            {
                _tcpConnection.Connect();
            }
            catch (SocketException)
            {
                if (!IsConnected)
                {
                    Task.Run(async () => { await AttemptReconnectInfinitely(); });
                }
            }
        }

        private void CreateTcpConnection()
        {
            if (_tcpConnection != null)
            {
                _tcpConnection.Connected -= ConnectedEvent;
                _tcpConnection.Disconnected -= DisconnectedEvent;
            }

            // instantiate
            _tcpConnection = new TCPConnection(_settings.ConnectionString);

            // set events
            _tcpConnection.Connected += ConnectedEvent;
            _tcpConnection.Disconnected += DisconnectedEvent;
        }

        protected override void OverridableDispose()
        {
            // Dispose to close tcp connection when the printer object is disposed otherwise
            // the tcp connection is held until garbage collected
            _tcpConnection?.Dispose();
            _tcpConnection = null;
        }

        ~NetworkPrinter()
        {
            Dispose(true);
        }
    }
}
