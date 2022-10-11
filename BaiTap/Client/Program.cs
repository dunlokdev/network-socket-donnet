using System;
using System.Collections.Generic;
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
                buff = new byte[1024];
                byteReceive = serverSocket.Receive(buff, 0, buff.Length, SocketFlags.None);
                double kq = BitConverter.ToDouble(buff, 0);
                Console.WriteLine("Ket qua cua phep toan: " + kq);
            }

            Console.ReadKey();
            serverSocket.Close();
        }
    }
}
