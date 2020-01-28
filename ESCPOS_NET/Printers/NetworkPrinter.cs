using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace ESCPOS_NET
{
    public class NetworkPrinter : BasePrinter
    {
        // flag which allows an attempt to reconnect on timeout.
        private readonly bool _reconnectOnTimeout;
        private readonly IPEndPoint _endPoint;
        private Socket _socket;
        private NetworkStream _sockStream;

        protected override bool IsConnected => !((_socket.Poll(1000, SelectMode.SelectRead) && (_socket.Available == 0)) || !_socket.Connected);

        public NetworkPrinter(IPEndPoint endPoint, bool reconnectOnTimeout) : base()
        {
            _reconnectOnTimeout = reconnectOnTimeout;
            _endPoint = endPoint;
            Connect();
        }

        public NetworkPrinter(IPAddress ipAddress, int port, bool reconnectOnTimeout) : base()
        {
            _reconnectOnTimeout = reconnectOnTimeout;
            _endPoint = new IPEndPoint(ipAddress, port);
            Connect();
        }

        public NetworkPrinter(string ipAddress, int port, bool reconnectOnTimeout) : base()
        {
            _reconnectOnTimeout = reconnectOnTimeout;
            _endPoint = new IPEndPoint(IPAddress.Parse(ipAddress), port);
            Connect();
        }

        protected override void Reconnect()
        {
            try
            {
                if (_reconnectOnTimeout)
                {
                    Console.WriteLine("Trying to reconnect...");
                    StopMonitoring();
                    Writer?.Flush();
                    Writer?.Close();
                    Reader?.Close();

                    _sockStream?.Close();
                    _socket?.Close();

                    Connect();
                    Console.WriteLine("Reconnected!");
                    StartMonitoring();
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Failed to reconnect: {ex.Message}");
                throw;
            }
        }

        private void Connect()
        {
            _socket = new Socket(_endPoint.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
            _socket.Connect(_endPoint);
            _sockStream = new NetworkStream(_socket);

            // Need to review the paramaters set here
            Writer = new BinaryWriter(_sockStream, new UTF8Encoding(), true);
            Reader = new BinaryReader(_sockStream, new UTF8Encoding(), true);
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
