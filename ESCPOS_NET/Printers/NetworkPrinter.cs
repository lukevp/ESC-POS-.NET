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

        //protected override void Reconnect()
        //{
        //    if (_settings == null) return;
        //    if (!_settings.ReconnectOnTimeout)
        //    {
        //        Logging.Logger?.LogInformation($"[{PrinterName}] Reconnect: Settings have disabled reconnection, skipping reconnect attempt.");
        //        return;
        //    }

        //    reconnectAttempts += 1;
        //    Logging.Logger?.LogTrace($"[{PrinterName}] Reconnect: Reconnection attempt {reconnectAttempts}.");
        //    // TODO: implement simple TCP
        //    try
        //    {
        //        Writer?.Flush();
        //    }
        //    catch (Exception e)
        //    {
        //        Logging.Logger?.LogDebug(e, $"[{PrinterName}] Reconnect: Issue flushing writer.");
        //    }
        //    try
        //    {
        //        Writer?.Close();
        //    }
        //    catch (Exception e)
        //    {
        //        Logging.Logger?.LogDebug(e, $"[{PrinterName}] Reconnect: Issue closing writer.");
        //    }
        //    try
        //    {
        //        Reader?.Close();
        //    }
        //    catch (Exception e)
        //    {
        //        Logging.Logger?.LogDebug(e, $"[{PrinterName}] Reconnect: Issue closing reader.");
        //    }
        //    try
        //    {
        //        _sockStream?.Close();
        //    }
        //    catch (Exception e)
        //    {
        //        Logging.Logger?.LogDebug(e, $"[{PrinterName}] Reconnect: Issue closing socket stream.");
        //    }
        //    try
        //    {
        //        _socket?.Close();
        //    }
        //    catch (Exception e)
        //    {
        //        Logging.Logger?.LogDebug(e, $"[{PrinterName}] Reconnect: Issue closing socket.");
        //    }
        //    try
        //    {
        //        Logging.Logger?.LogDebug($"[{PrinterName}] Reconnect: Attempting to reconnect...");
        //        Connect();
        //    }
        //    catch (Exception e)
        //    {
        //        Logging.Logger?.LogError(e, $"[{PrinterName}] Reconnect: Failed to reconnect.");
        //        throw;
        //    }
        //}

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
        private void DataReceived(object sender, DataReceivedEventArgs e)
        {

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

            // let's go!
            // Attempt to connect for 5 minutes. If it fails, try again.
            // TODO: adapt binaryreader and binarywriter to the SimpleClient so that when 
            // DataReceived is fired or when write is called it will send it to the client lib (SendAsync)

            //// Need to review the parameters set here
            //Writer = new BinaryWriter(_sockStream, new UTF8Encoding(), true);
            //Reader = new BinaryReader(_sockStream, new UTF8Encoding(), true);
        }

        protected override void OverridableDispose()
        {
            _tcpConnection = null;
        }
    }
}
