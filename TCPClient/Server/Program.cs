using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;

namespace Server
{
    internal class Program
    {

        static void Main(string[] args)
        {
            Console.WriteLine("=== SERVER ===");
            // TcpListen cần ip + port
            // Local Host: IPAddress.Loopback ~ 127.0.0.1
            // Any: IPAddress.Any ~ Dia chi IP bat ky
            // Minh tu go IP: IPAddress.Parse("127.0.0.1")
            TcpListener listener = new TcpListener(IPAddress.Parse("127.0.0.1"), 9000);

            // 1. LISTEN
            listener.Start();
            Console.WriteLine("Dang cho Client ket noi ...");
            Socket client = listener.AcceptSocket();


            // 2. RECVICE
            byte[] data = new byte[1024]; 
            int recv = client.Receive(data, 0, data.Length, SocketFlags.None);
            string str = Encoding.UTF8.GetString(data, 0, recv);
            Console.WriteLine($"Client: {str}");

            // 3. SEND
            string input = "Xin chao Client";
            data = Encoding.ASCII.GetBytes(input);
            client.Send(data,0,data.Length, SocketFlags.None);
            // 4. CLOSE
            client.Close();
            listener.Stop();
        }

    }
}
