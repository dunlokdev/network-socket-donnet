using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Client
{
    public partial class frmClient : Form
    {
        IPEndPoint IP;
        Socket client;

        public frmClient()
        {
            InitializeComponent();
            CheckForIllegalCrossThreadCalls = false;
            Connect();
        }

        /// <summary>
        /// Gửi tin khi nhấn button send
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSend_Click(object sender, EventArgs e)
        {
            Send();
            AddMessage(txbMessage.Text);
        }

        /// <summary>
        /// Kết nối đến server
        /// </summary>
        void Connect()
        {
            IP = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 9999);
            client = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            try
            {
                client.Connect(IP);
            }
            catch (Exception)
            {
                MessageBox.Show("Không thể kết nối đến server!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            Thread listen = new Thread(Receive);
            listen.IsBackground = true;
            listen.Start();
        }

        /// <summary>
        /// Đóng kết nối hiện thời
        /// </summary>
        void Close()
        {
            client.Close();
        }

        /// <summary>
        /// Gửi tin
        /// </summary>
        void Send()
        {
            if (txbMessage.Text != String.Empty)
                client.Send(Serialize(txbMessage.Text));
        }

        /// <summary>
        /// Nhận tin
        /// </summary>
        void Receive()
        {
            try
            {
                while (true)
                {
                    byte[] data = new byte[1024 * 5000];
                    client.Receive(data);

                    string message = (String)Deserialize(data);

                    AddMessage($"(IP {client.RemoteEndPoint.ToString()})" + message);
                }
            }
            catch
            {
                Close();
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
            MemoryStream newValue = new MemoryStream();
            BinaryFormatter formatter = new BinaryFormatter();

            formatter.Serialize(newValue, obj);

            return newValue.ToArray();
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
        private void frmClient_FormClosed(object sender, FormClosedEventArgs e)
        {
            Close();
        }
    }
}
