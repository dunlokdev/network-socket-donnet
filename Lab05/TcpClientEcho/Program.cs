using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TcpClientEcho
{
    internal class Program
    {
        static void Main(string[] args)
        {
            if (args.Length != 3)
            {
                Console.WriteLine("Missing parameters!");
                return;
            }

            int port;
            string message;
            try
            {
                port = int.Parse(args[1]);
                message = args[2];
            }
            catch (Exception)
            {
                Console.WriteLine("Invalid parameters!");
                return;
            }

            TcpClient client;
            try
            {
                client = new TcpClient(args[0], port);
            }
            catch (SocketException e)
            {
                Console.WriteLine("Can't connect to server");
                Console.WriteLine("Error: " + e.Message);
                return;
            }

            NetworkStream stream = client.GetStream();
            Console.WriteLine("Connected successflly to server");

            int i = 1;
            while (true)
            {
                try
                {
                    byte[] buff = Encoding.UTF8.GetBytes(message);
                    stream.Write(buff, 0, buff.Length);

                    buff = new byte[1024];
                    int bytes = stream.Read(buff, 0, buff.Length);
                    message = Encoding.UTF8.GetString(buff, 0, bytes);
                    Console.WriteLine($"#{i}. {message}");

                    Thread.Sleep(1000);
                }
                catch (Exception)
                {
                    Console.WriteLine("Has error when processing this request");
                    break;
                }
                i++;
            }

            stream.Flush();
            stream.Close();
            client.Close();
        }
    }
}
