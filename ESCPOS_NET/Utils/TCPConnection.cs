﻿using ESCPOS_NET.Utils;
using SimpleTcp;
using System;
using System.IO;

namespace ESCPOS_NET
{
    public class TCPConnection : IDisposable
    {
        public Stream ReadStream { get; private set; } = new EchoStream();
        public Stream WriteStream { get; private set; }
        public event EventHandler<ClientConnectedEventArgs> Connected;
        public event EventHandler<ClientDisconnectedEventArgs> Disconnected;
        public bool IsConnected => _client?.IsConnected ?? false;
        private SimpleTcpClient _client;
        private bool disposedValue;

        public TCPConnection(string destination)
        {
            _client = new SimpleTcpClient(destination);
            _client.Events.Connected += ConnectedEventHandler;
            _client.Events.Disconnected += DisconnectedEventHandler;
            _client.Events.DataReceived += DataReceivedEventHandler;
            _client.Keepalive = new SimpleTcpKeepaliveSettings() { EnableTcpKeepAlives = true, TcpKeepAliveInterval = 1, TcpKeepAliveTime = 1, TcpKeepAliveRetryCount = 3 };
            ReadStream.ReadTimeout = 1500;
            WriteStream = new InterceptableWriteMemoryStream(bytes => _client.Send(bytes));
        }

        private void ConnectedEventHandler(object sender, ClientConnectedEventArgs e)
        {
            Connected?.Invoke(sender, e);
        }

        private void DisconnectedEventHandler(object sender, ClientDisconnectedEventArgs e)
        {
            Disconnected?.Invoke(sender, e);
        }

        private void DataReceivedEventHandler(object sender, DataReceivedEventArgs e)
        {
            ReadStream.Write(e.Data, 0, e.Data.Length);
        }

        public void ConnectWithRetries(int timeoutMs)
        {
            _client.ConnectWithRetries(timeoutMs);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    _client.Events.DataReceived -= DataReceivedEventHandler;
                    _client.Events.Connected -= ConnectedEventHandler;
                    _client.Events.Disconnected -= DisconnectedEventHandler;
                    _client.Disconnect();
                    _client?.Dispose();
                }

                disposedValue = true;
            }
        }

        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}
