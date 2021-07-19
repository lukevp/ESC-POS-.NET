using Microsoft.Extensions.Logging;
using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

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
        private Socket _socket;
        private NetworkStream _sockStream;
        private int reconnectAttempts = 0;
        private volatile bool _isConnecting = false;

        protected override bool IsConnected
        {
            get
            {
                if (_socket == null) return false;
                if (_isConnecting) return false;
                if (!_socket.Connected) return false;

                try
                {
                    return !(_socket.Poll(1, SelectMode.SelectRead) && _socket.Available == 0);
                }
                catch (Exception e)
                {
                    Logging.Logger?.LogDebug(e, "IsConnected returning false due to connection issue.");
                    return false;
                }
            }
        }

        public NetworkPrinter(NetworkPrinterSettings settings) : base(settings.PrinterName)
        {
            _settings = settings;
            Connect();
        }

        public NetworkPrinter(IPEndPoint endPoint, bool reconnectOnTimeout)
            : base()
        {
            _settings = new NetworkPrinterSettings()
            {
                EndPoint = endPoint,
                ReconnectOnTimeout = reconnectOnTimeout,
            };
            Connect();
        }

        public NetworkPrinter(IPAddress ipAddress, int port, bool reconnectOnTimeout)
            : base()
        {
            var endPoint = new IPEndPoint(ipAddress, port);
            _settings = new NetworkPrinterSettings()
            {
                EndPoint = endPoint,
                ReconnectOnTimeout = reconnectOnTimeout,
            };
            Connect();
        }

        public NetworkPrinter(string ipAddress, int port, bool reconnectOnTimeout)
            : base()
        {
            var endPoint = new IPEndPoint(IPAddress.Parse(ipAddress), port);
            _settings = new NetworkPrinterSettings()
            {
                EndPoint = endPoint,
                ReconnectOnTimeout = reconnectOnTimeout,
            };
            Connect();
        }

        protected override void Reconnect()
        {
            if (_settings == null) return;
            if (!_settings.ReconnectOnTimeout)
            {
                Logging.Logger?.LogInformation($"[{PrinterName}] Reconnect: Settings have disabled reconnection, skipping reconnect attempt.");
                return;
            }

            reconnectAttempts += 1;
            Logging.Logger?.LogTrace($"[{PrinterName}] Reconnect: Reconnection attempt {reconnectAttempts}.");
            // TODO: implement simple TCP
            try
            {
                Writer?.Flush();
            }
            catch (Exception e)
            {
                Logging.Logger?.LogDebug(e, $"[{PrinterName}] Reconnect: Issue flushing writer.");
            }
            try
            {
                Writer?.Close();
            }
            catch (Exception e)
            {
                Logging.Logger?.LogDebug(e, $"[{PrinterName}] Reconnect: Issue closing writer.");
            }
            try
            {
                Reader?.Close();
            }
            catch (Exception e)
            {
                Logging.Logger?.LogDebug(e, $"[{PrinterName}] Reconnect: Issue closing reader.");
            }
            try
            {
                _sockStream?.Close();
            }
            catch (Exception e)
            {
                Logging.Logger?.LogDebug(e, $"[{PrinterName}] Reconnect: Issue closing socket stream.");
            }
            try
            {
                _socket?.Close();
            }
            catch (Exception e)
            {
                Logging.Logger?.LogDebug(e, $"[{PrinterName}] Reconnect: Issue closing socket.");
            }
            try
            {
                Logging.Logger?.LogDebug($"[{PrinterName}] Reconnect: Attempting to reconnect...");
                Connect();
            }
            catch (Exception e)
            {
                Logging.Logger?.LogError(e, $"[{PrinterName}] Reconnect: Failed to reconnect.");
                throw;
            }
        }

        private void Connect()
        {

            // Allow cooperative threads to check volatile bool to see if we are currently processing the socket
            // and it's not readable (for example, the IsConnected property).
            if (_isConnecting) return;
            _isConnecting = true;


            _socket = new Socket(_settings.EndPoint.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
            _socket.SendTimeout = (int)(_settings.SendTimeoutMs ?? 3000);
            _socket.ReceiveTimeout = (int)(_settings.ReceiveTimeoutMs ?? 750);
            _socket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.KeepAlive, true);


            var socketArgs = new SocketAsyncEventArgs();

            IAsyncResult result = _socket.BeginConnect(_settings.EndPoint, null, null);

            bool success = result.AsyncWaitHandle.WaitOne((int)(_settings.ConnectTimeoutMs ?? 10000), true);

            if (_socket.Connected)
            {
                _socket.EndConnect(result);
                _sockStream = new NetworkStream(_socket);

                // Need to review the parameters set here
                Writer = new BinaryWriter(_sockStream, new UTF8Encoding(), true);
                Reader = new BinaryReader(_sockStream, new UTF8Encoding(), true);

                Logging.Logger?.LogDebug($"[{PrinterName}] Connect: Successfully connected.");

                reconnectAttempts = 0;
                _isConnecting = false;
                // StartMonitoring();
            }
            else
            {
                _socket.Close();
                if (_settings.ReconnectOnTimeout && (_settings.MaxReconnectAttempts == null && reconnectAttempts < 100) || reconnectAttempts < _settings.MaxReconnectAttempts)
                {
                    _isConnecting = false;
                    Thread.Sleep(250);
                    Reconnect();
                }
            }
        }

        protected override void OverridableDispose()
        {
            _sockStream?.Close();
            _sockStream?.Dispose();
            _socket?.Close();
            _socket?.Dispose();
        }
    }
}
