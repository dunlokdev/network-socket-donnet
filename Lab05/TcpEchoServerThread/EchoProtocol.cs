using Microsoft.Build.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TcpEchoServerThread
{
    public class EchoProtocol : IProtocol
    {
        public const int BUFSIZE = 32;
        private Socket socket;
        private ILogger logger;

        public EchoProtocol(Socket cltnSock, ILogger logger)
        {
            this.socket = cltnSock;
            this.logger = logger;
        }

        public void handleClient()
        {
            ArrayList entry = new ArrayList();
            entry.Add($"Client address port: ${socket.RemoteEndPoint}");
            entry.Add($"Thread: {Thread.CurrentThread.GetHashCode()}");
            try
            {
                int recvMsgSize;
                int totalBytesEchoed = 0;
                byte[] recvBuffer = new byte[BUFSIZE];
                try
                {
                    while ((recvMsgSize = socket.Receive(recvBuffer, 0, recvBuffer.Length, SocketFlags.None)) > 0) ;
                    {
                        socket.Send(recvBuffer, 0, recvMsgSize, SocketFlags.None);
                        totalBytesEchoed += recvMsgSize;
                    }
                }
                catch (SocketException se)
                {
                    entry.Add(se.ErrorCode + ":" + se.Message);
                }
                entry.Add($"Client finshed ! Echoed: {totalBytesEchoed} bytes.");
            }
            catch (SocketException se)
            {
                entry.Add(se.ErrorCode + ":" + se.Message);
            }
            socket.Close();
            logger.writeEntry(entry);
        }
    }
}
