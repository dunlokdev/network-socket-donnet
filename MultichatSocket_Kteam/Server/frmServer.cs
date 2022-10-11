using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading;

namespace Server
{
    public partial class frmServer : Form
    {
        IPEndPoint IP;
        Socket server;
        List<Socket> clientList;
        private const int BUFFER_SIZE = 1024;
        private const int PORT_NUMBER = 9999;

        public frmServer()
        {
            InitializeComponent();
            CheckForIllegalCrossThreadCalls = false;
            Connect();
        }

        /// <summary>
        /// Gửi tin cho tất cả client khi nhấn button send
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSend_Click(object sender, EventArgs e)
        {
            foreach (Socket item in clientList)
            {
                Send(item);
            }
            AddMessage(txbMessage.Text);
        }

        /// <summary>
        /// Kết nối đến server
        /// </summary>
        void Connect()
        {
            clientList = new List<Socket>();
            IP = new IPEndPoint(IPAddress.Any, PORT_NUMBER);
            server = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            server.Bind(IP);
            AddMessage($"Server đã khởi động tại địa chỉ 127.0.0.1, Port: {PORT_NUMBER}");

            Thread listen = new Thread(() =>
            {
                try
                {
                    while (true)
                    {
                        server.Listen(100);
                        Socket client = server.Accept();
                        clientList.Add(client);
                        AddMessage(client.RemoteEndPoint.ToString() + ": Đã kết nối");
                        Thread receive = new Thread(Receive);
                        receive.IsBackground = true;
                        receive.Start(client);
                    }
                }
                catch (Exception)
                {
                    IP = new IPEndPoint(IPAddress.Any, 9999);
                    server = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                }
            });
            listen.IsBackground = true;
            listen.Start();

        }

        /// <summary>
        /// Đóng kết nối hiện thời
        /// </summary>
        void Close()
        {
            server.Close();
        }

        /// <summary>
        /// Gửi tin
        /// </summary>
        void Send(Socket client)
        {
            if (client != null && txbMessage.Text != String.Empty)
                client.Send(Serialize(txbMessage.Text));
        }

        /// <summary>
        /// Nhận tin
        /// </summary>
        void Receive(object obj)
        {
            Socket client = obj as Socket;
            try
            {
                while (true)
                {
                    byte[] data = new byte[1024 * 5000];
                    client.Receive(data);

                    string message = (string)Deserialize(data);
                    foreach (Socket item in clientList)
                    {
                        if (item != null && item != client)
                            item.Send(Serialize(message));
                    }
                    AddMessage($"Client (IP: {client.RemoteEndPoint.ToString()}): " + message);
                }
            }
            catch
            {
                clientList.Remove(client);
                AddMessage(client.RemoteEndPoint.ToString() + ": Đã đóng kết nối!");
                client.Close();
            }
        }

        /// <summary>
        /// Thêm message vào khung chat
        /// </summary>
        /// <param name="message"></param>
        void AddMessage(string message)
        {
            lsvMessage.Items.Add(new ListViewItem() { Text = message });
            txbMessage.Text = "";
        }

        /// <summary>
        /// Phân mảnh
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        byte[] Serialize(object obj)
        {
            MemoryStream stream = new MemoryStream();
            BinaryFormatter formatter = new BinaryFormatter();

            formatter.Serialize(stream, obj);

            return stream.ToArray();
        }

        /// <summary>
        /// Gom mảnh
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        object Deserialize(byte[] data)
        {
            MemoryStream stream = new MemoryStream(data);
            BinaryFormatter formatter = new BinaryFormatter();

            return formatter.Deserialize(stream);
        }

        /// <summary>
        /// Đóng kết nối khi đóng Form
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void frmServer_FormClosed(object sender, FormClosedEventArgs e)
        {
            Close();
        }

    }
}
