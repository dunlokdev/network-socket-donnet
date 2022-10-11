using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;

namespace BaiKTra
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("======= SERVER ======");
            IPEndPoint serverEndPoint = new IPEndPoint(IPAddress.Any, 5000);
            Socket serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            serverSocket.Bind(serverEndPoint);
            serverSocket.Listen(10);

            Console.WriteLine("Dang doi client ket noi ...");
            Socket clientSocket = serverSocket.Accept();

            EndPoint clientEndPoint = clientSocket.RemoteEndPoint;
            Console.WriteLine("IP: " + clientEndPoint.ToString());

            byte[] buff;
            string myStr = "Hello world";
            int byteReceive;
            string str;
            buff = Encoding.ASCII.GetBytes(myStr);
            clientSocket.Send(buff, 0, buff.Length, SocketFlags.None);

            while (true)
            {
                buff = new byte[1024];
                byteReceive = clientSocket.Receive(buff, 0, buff.Length, SocketFlags.None);
                str = Encoding.UTF8.GetString(buff, 0, byteReceive);
                Console.WriteLine("Client said: " + str);
                str = "Ban da gui tin nhan thanh cong !";
                buff = Encoding.ASCII.GetBytes(str);
                clientSocket.Send(buff, 0, buff.Length, SocketFlags.None);
            }

            Console.ReadLine();
            serverSocket.Close();
            clientSocket.Close();
        }
    }
}
