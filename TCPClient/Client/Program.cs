using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Client
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("=== CLIENT ===");
            // 1. Connect
            TcpClient client = new TcpClient();
            client.Connect(IPAddress.Loopback, 9000);
            Stream stream = client.GetStream();

            // 2. Send
            byte[] data = new byte[1024];
            Console.Write("Nhap tin nhan: ");
            string input = Console.ReadLine(); 
            data = Encoding.UTF8.GetBytes(input);
            stream.Write(data, 0, data.Length);
            // 3. Recive
            data = new byte[1024];
            int recv = stream.Read(data, 0, data.Length);
            string strRecv = Encoding.ASCII.GetString(data, 0, recv);
            Console.WriteLine($"Server: {strRecv} ");

            // 4. Close
            stream.Close();
            client.Close();
        }
    }
}
