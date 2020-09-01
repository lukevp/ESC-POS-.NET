using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace ESC_NET.Printers
{
    public class NetworkPrinter : BasePrinter
    {
        private readonly IPEndPoint _endPoint;

        // flag which allows an attempt to reconnect on timeout.
        private readonly bool _reconnectOnTimeout;
        private Socket _socket;
        private NetworkStream _sockStream;

        public NetworkPrinter(IPEndPoint endPoint, bool reconnectOnTimeout)
        {
            _reconnectOnTimeout = reconnectOnTimeout;
            _endPoint = endPoint;
            Connect();
        }

        public NetworkPrinter(IPAddress ipAddress, int port, bool reconnectOnTimeout)
        {
            _reconnectOnTimeout = reconnectOnTimeout;
            _endPoint = new IPEndPoint(ipAddress, port);
            Connect();
        }

        public NetworkPrinter(string ipAddress, int port, bool reconnectOnTimeout)
        {
            _reconnectOnTimeout = reconnectOnTimeout;
            _endPoint = new IPEndPoint(IPAddress.Parse(ipAddress), port);
            Connect();
        }

        protected override bool IsConnected =>
            !(_socket.Poll(1000, SelectMode.SelectRead) && _socket.Available == 0 || !_socket.Connected);

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
                Debug.WriteLine($"Failed to reconnect: {ex.Message}");
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