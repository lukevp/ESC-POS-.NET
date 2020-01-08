using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace ESCPOS_NET
{
    public class NetworkPrinter : BasePrinter
    {
        private readonly IPEndPoint _endPoint;
        private Socket _socket;
        private NetworkStream _sockStream;

        public NetworkPrinter(IPEndPoint endPoint) : base()
        {
            _endPoint = endPoint;
            Connect();
        }

        public NetworkPrinter(IPAddress ipAddress, int port) : base()
        {
            _endPoint = new IPEndPoint(ipAddress, port);
            Connect();
        }

        public NetworkPrinter(string ipAddress, int port) : base()
        {
            _endPoint = new IPEndPoint(IPAddress.Parse(ipAddress), port);
            Connect();
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

        ~NetworkPrinter()
        {
            Dispose(false);
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
