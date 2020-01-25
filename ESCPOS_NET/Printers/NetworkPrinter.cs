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

        private bool _IsBinary = true;

        #endregion

        #region Constructors

        public NetworkPrinter(IPEndPoint endPoint, bool isBinary) : base()
        {
            this._endPoint = endPoint;
            this._IsBinary = isBinary;
            this.InitPrinter();
        }

        public NetworkPrinter(IPAddress ipAddress, int port, bool isBinary) : base()
        {
            this._endPoint = new IPEndPoint(ipAddress, port);
            this._IsBinary = isBinary;
            this.InitPrinter();
        }

        public NetworkPrinter(string ipAddress, int port, bool isBinary) : base()
        {
            if (IPAddress.TryParse(ipAddress, out IPAddress address))
            {
                this._endPoint = new IPEndPoint(address, port);
                this._IsBinary = isBinary;
                this.InitPrinter();
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

        /// <summary>
        /// Override to ensure that the socket is connected.
        /// </summary>
        /// <param name="bytes"></param>
        public override void Write(byte[] bytes)
        {
            if (!this._socket.Connected)
            {
                this.InitPrinter();
            }

            if (this._IsBinary)
            {
                base.Write(bytes);
            }
            else
            {
                this._socket.Send(bytes);
            }            
        }


        #region Private Methods

        protected override void InitPrinter()
        {
            try
            {
                this._socket = new Socket(_endPoint.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
                this._socket.Connect(_endPoint);
                this._sockStream = new NetworkStream(_socket, true);
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
