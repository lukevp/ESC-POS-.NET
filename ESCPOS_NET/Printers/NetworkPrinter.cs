using ESCPOS_NET.Events;
using System;
using System.IO;
using System.Net;
using System.Net.Sockets;

namespace ESCPOS_NET
{
    public class NetworkPrinter : BasePrinter
    {
        #region Private Members

        private readonly IPEndPoint _endPoint;
        private Socket _socket;
        private NetworkStream _sockStream;

        #endregion

        #region Constructors

        public NetworkPrinter(IPEndPoint endPoint) : base()
        {
            this._endPoint = endPoint;
            this.Connect();
        }

        public NetworkPrinter(IPAddress ipAddress, int port) : base()
        {
            this._endPoint = new IPEndPoint(ipAddress, port);
            this.Connect();
        }

        public NetworkPrinter(string ipAddress, int port) : base()
        {
            if (IPAddress.TryParse(ipAddress, out IPAddress address))
            {
                this._endPoint = new IPEndPoint(address, port);
                this.Connect();
            }
            else
            {
                OnStatusChanged(this, new StatusUpdateEventArgs()
                {
                    Message = "Invalid IP Address",
                    UpdateType = StatusEventType.Error
                });
            }
        }

        #endregion

        #region Private Methods

        private void Connect()
        {
            try
            {
                this._socket = new Socket(_endPoint.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
                this._socket.Connect(_endPoint);
                this._sockStream = new NetworkStream(_socket);
                this._writer = new BinaryWriter(_sockStream);
                this._reader = new BinaryReader(_sockStream);
            }
            catch (Exception ex)
            {
                OnStatusChanged(this, new StatusUpdateEventArgs()
                {
                    Message = ex.Message,
                    UpdateType = StatusEventType.Error
                });
            }
        }

        #endregion

        #region Dispose

        private bool _disposed = false;

        protected override void Dispose(bool disposing)
        {
            if (_disposed)
                return;

            if (disposing)
            {
                this._sockStream?.Close();
                this._sockStream?.Dispose();
                this._socket?.Close();
                this._socket?.Dispose();
            }

            base.Dispose(disposing);
        }

        #endregion
    }
}
