// This code is adapted from a sample found at the URL 
// "http://blogs.msdn.com/b/jmanning/archive/2004/12/19/325699.aspx"

using System;
using System.Net;
using System.Net.Sockets;
using System.IO;
using System.Text;
using System.Threading;

namespace TcpEchoServer
{
	public class TcpEchoServer
	{
		public static void Main()
		{
			Console.WriteLine("Starting echo server...");

			int port = 9100;
			TcpListener listener = new TcpListener(IPAddress.Loopback, port);
			listener.Start();
			TcpClient client;

            while (true)
            {
                // Accept multiple connections with a dedicated thread for each connection
                client = listener.AcceptTcpClient();
				ThreadPool.QueueUserWorkItem(TcpClientConnectionHandler, client);
            }
        }

		private static void TcpClientConnectionHandler(object obj)
		{
			var tcp = (TcpClient)obj;
			NetworkStream stream = tcp.GetStream();
			StreamWriter writer = new StreamWriter(stream, Encoding.ASCII) { AutoFlush = true };
			StreamReader reader = new StreamReader(stream, Encoding.ASCII);
			try
			{
				while (true)
				{
					string inputLine = "";
					while (inputLine != null)
					{
						inputLine = reader.ReadLine();
						writer.Write("E");
						Console.WriteLine("Echoing string: " + inputLine);
					}
					Console.WriteLine("Server saw disconnect from client.");
				}
			}
			catch(IOException)
            {
				// connection is closed
            }			
		}
	}
}
