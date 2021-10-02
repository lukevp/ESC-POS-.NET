using Microsoft.Extensions.Logging;
using SimpleTcp;
using System;
using System.IO;
using System.Threading.Tasks;
using System.Reflection;

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
            IsConnected = true;
            InvokeConnect();
        }
        private void DisconnectedEvent(object sender, ClientDisconnectedEventArgs e)
        {
            IsConnected = false;
            InvokeDisconnect();
            Logging.Logger?.LogWarning("[{Function}]:[{PrinterName}] Network printer connection terminated. Attempting to reconnect. Connection String: {ConnectionString}", $"{this}.{MethodBase.GetCurrentMethod().Name}", PrinterName, _settings.ConnectionString);
            Connect();
        }
        private void AttemptReconnectInfinitely()
        {
            try
            {
                //_tcpConnection.ConnectWithRetries(300000);
                _tcpConnection.ConnectWithRetries(3000);
            }
            catch
            {
                //Logging.Logger?.LogWarning("[{Function}]:[{PrinterName}] Network printer unable to connect after 5 minutes. Attempting to reconnect. Settings: {Settings}", $"{this}.{MethodBase.GetCurrentMethod().Name}", PrinterName, JsonSerializer.Serialize(_settings));
                Task.Run(async () => { await Task.Delay(250); Connect(); });
            }
        }

        private void Connect()
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

            Reader = new BinaryReader(_tcpConnection.ReadStream);
            Writer = new BinaryWriter(_tcpConnection.WriteStream);

            Task.Run(() => { AttemptReconnectInfinitely(); });
        }

        protected override void OverridableDispose()
        {
            _tcpConnection = null;
        }
    }
}
