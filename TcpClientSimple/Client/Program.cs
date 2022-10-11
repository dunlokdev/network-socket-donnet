using System;
using System.IO;
using System.Net;
using System.Text;
using System.Net.Sockets;

public class Client
{

    private const int BUFFER_SIZE = 1024;
    private const int PORT_NUMBER = 8000;

    static ASCIIEncoding encoding = new ASCIIEncoding();

    public static void Main()
    {

        Console.WriteLine("\n=== CLIENT === ");
        try
        {
            TcpClient client = new TcpClient();

            // 1. connect
            client.Connect("127.0.0.1", PORT_NUMBER);
            Stream stream = client.GetStream();

            Console.WriteLine("Connected to Dunlok server.");
            while (true)
            {
                Console.Write("Enter your message: ");

                string str = Console.ReadLine();

                // 2. send
                byte[] data = encoding.GetBytes(str);

                stream.Write(data, 0, data.Length);
                // 3. receive
                data = new byte[BUFFER_SIZE];
                int recv = stream.Read(data, 0, BUFFER_SIZE);

                Console.WriteLine(encoding.GetString(data, 0, recv));

            }
            // 4. Close
            stream.Close();
            client.Close();
        }

        catch (Exception ex)
        {
            Console.WriteLine("Error: " + ex);
        }

        Console.Read();
    }
}