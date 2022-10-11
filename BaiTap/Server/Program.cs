using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Server
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
            buff = Encoding.ASCII.GetBytes(myStr);
            clientSocket.Send(buff, 0, buff.Length, SocketFlags.None);

            while (true)
            {
                Console.Clear();
                double a, b, kq = 0;

                Console.Write("Nhap so a: ");
                a = int.Parse(Console.ReadLine());
                Console.Write("Nhap so b: ");
                b = int.Parse(Console.ReadLine());
                
                Console.WriteLine("\n\n0. Thoat khoi chuong trinh");
                Console.WriteLine("1. Phep cong");
                Console.WriteLine("2. Phep tru");
                Console.WriteLine("3. Phep nhan");
                Console.WriteLine("4. Phep chia");
                Console.Write("Nhap lua chon: ");

                int choose = int.Parse(Console.ReadLine());
                switch (choose)
                {
                    case 0:
                        Console.WriteLine("Exit");
                        return;
                    case 1:
                        Console.WriteLine("Phep cong");
                        kq = a + b;
                        break;
                    case 2:
                        Console.WriteLine("Phep tru");
                        kq = a - b;
                        break;
                    case 3:
                        Console.WriteLine("Phep nhan");
                        kq = a * b;
                        break;
                    case 4:
                        Console.WriteLine("Phep chia");
                        kq = (double)a / b;
                        break;
                    default:
                        break;
                }
                Console.WriteLine($"Ket qua cua phep toan: {kq}");
                buff = BitConverter.GetBytes(kq);
                clientSocket.Send(buff, 0, buff.Length, SocketFlags.None);
                Console.WriteLine("Press key to continue program...");
                Console.ReadKey();
            }
            Console.ReadLine();
            serverSocket.Close();
            clientSocket.Close();
        }
    }
}
