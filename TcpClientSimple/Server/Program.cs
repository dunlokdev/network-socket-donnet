using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;

namespace Server
{
    public class Program
    {
        private const int BUFFER_SIZE = 1024;
        private const int PORT_NUMBER = 8000;
        static ASCIIEncoding encoding = new ASCIIEncoding();
        static void Main(string[] args)
        {
            Console.WriteLine("\n=== SERVER === ");
            try
            {
                IPAddress address = IPAddress.Parse("127.0.0.1");
                TcpListener listener = new TcpListener(address, PORT_NUMBER);

                // 1. Listen
                listener.Start();
                Console.WriteLine($"Server: {listener.LocalEndpoint}");
                Console.WriteLine("Waiting for a connection...");
                Socket socket = listener.AcceptSocket();
                Console.WriteLine($"Connection recived from: {socket.RemoteEndPoint}");

                while (true)
                {
                    // 2. Recived
                    byte[] data = new byte[BUFFER_SIZE];
                    int recv = socket.Receive(data, 0, data.Length, SocketFlags.None);
                    string str = Encoding.ASCII.GetString(data, 0, recv);
                    Console.WriteLine($"Client send: {str}");

                    // 3. Send
                    socket.Send(encoding.GetBytes("Success!"));
                }
                // 4. Close
                socket.Close();
                listener.Stop();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex}");
                return;
            }
        }
    }
}
