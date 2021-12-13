﻿using Microsoft.Extensions.Logging;
using SimpleTcp;
using System;
using System.IO;
using System.Threading.Tasks;
using System.Reflection;
using System.Threading;

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
            Connect(true);
        }

        public override void Connect(bool reconnecting = false)
        {

            OverridableDispose();

            // instantiate
            _tcpConnection = new TCPConnection(_settings.ConnectionString);

            // set events
            _tcpConnection.Connected += ConnectedEvent;
            _tcpConnection.Disconnected += DisconnectedEvent;

            _tcpConnection.ConnectWithRetries(3000);

            base.Connect(reconnecting);
        }

        protected override void WriteBytesUnderlying(byte[] buffer, int offset, int count)
        {
            _tcpConnection.WriteStream?.Write(buffer, offset, count);
        }

        protected override int ReadBytesUnderlying(byte[] buffer, int offset, int bufferSize)
        {
            return _tcpConnection.ReadStream?.Read(buffer, offset, bufferSize) ?? 0;
        }

        protected override void FlushUnderlying()
        {
            _tcpConnection.WriteStream?.Flush();
        }

        protected override void OverridableDispose()
        {
            if (_tcpConnection != null)
            {
                _tcpConnection.Connected -= ConnectedEvent;
                _tcpConnection.Disconnected -= DisconnectedEvent;
                _tcpConnection?.Dispose();
                _tcpConnection = null;
            }
        }
    }
}
