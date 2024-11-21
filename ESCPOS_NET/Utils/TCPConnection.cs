using ESCPOS_NET.Utils;
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
        //public event EventHandler<DataReceivedEventArgs> DataReceived;
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
        /// <summary>
        /// Establish a connection to the server without retry. SocketException will be thrown after the certain period of attempts.
        /// </summary>
        /// <exception cref="System.Net.Sockets.SocketException"></exception>
        public void Connect()
        {
            _client.Connect();
        }
        public void ConnectWithRetries(int timeoutMs)
        {
            _client.ConnectWithRetries(timeoutMs);
        }

        public void Dispose()
        {
            Dispose(true);
        }

        ~TCPConnection()
        {
            Dispose(false);
        }

        private void Dispose(bool disposing)
        {
            try
            {
                _client.Events.DataReceived -= DataReceivedEventHandler;
                _client.Events.Connected -= ConnectedEventHandler;
                _client.Events.Disconnected -= DisconnectedEventHandler;
                _client?.Dispose();
                _client = null;
            }
            catch { }
            try
            {
                WriteStream?.Dispose();
                ReadStream?.Dispose();
                WriteStream = null;
                ReadStream = null;
            }
            catch { }
        }
    }
}
