using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;

namespace ClientSimple
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("======= CLIENT =======");
            byte[] buff = new byte[1024];
            IPEndPoint serverEndPoint = new IPEndPoint(IPAddress.Loopback, 5000);
            Socket serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            Console.WriteLine("Dang doi ket noi den server...");

            try
            {
                serverSocket.Connect(serverEndPoint);
            }
            catch (SocketException ex)
            {
                Console.WriteLine("Khong the ket noi den server! Error: " + ex);
                Console.ReadKey(); 
                return;
            }
            Console.WriteLine("Ket noi den server thanh cong! ");
            int byteReceive;
            string str;
            if (serverSocket.Connected)
            {
                byteReceive = serverSocket.Receive(buff, 0, buff.Length, SocketFlags.None);
                str = Encoding.UTF8.GetString(buff, 0, byteReceive);
                Console.WriteLine("Server gui: " + str);
            }

            // Gui du lieu len server
            while (true)
            {
                Console.Write("Enter your message: ");
                str = Console.ReadLine();
                buff = Encoding.ASCII.GetBytes(str);
                serverSocket.Send(buff, 0, buff.Length, SocketFlags.None);
                buff = new byte[1024];
                byteReceive = serverSocket.Receive(buff, 0, buff.Length, SocketFlags.None);
                str = Encoding.UTF8.GetString(buff, 0, byteReceive);
                Console.WriteLine("Server said: " + str);
            }

            Console.ReadKey();
            serverSocket.Close();
        }
    }
}
