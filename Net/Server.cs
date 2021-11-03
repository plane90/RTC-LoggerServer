using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;
using System.Text;
using System;

namespace RTC_LoggerServer.Net
{

    static class Server
    {
        public static Socket socket;

        public static void RunServer()
        {
            using Socket socket = new Socket(
                AddressFamily.InterNetwork,
                SocketType.Stream,
                ProtocolType.Tcp);

            IPEndPoint iep = new IPEndPoint(IPAddress.Any, 10004);

            socket.Bind(iep);
            socket.Listen(1000);

            while (true)
            {
                Trace.WriteLine("Waiting Connection... \n");
                var clientSock = socket.Accept();
                Trace.WriteLine("Connected \n");
                _ = Task.Run(() =>
                {
                    try
                    {
                        ReceivePacket(clientSock);
                    }
                    catch (Exception e)
                    {
                        Trace.WriteLine(e.Message);
                    }
                });
            }
        }

        private static void ReceivePacket(Socket clientSocket)
        {
            byte[] packet = new byte[8 * 1024];
            while (clientSocket.Connected)
            {
                var length = clientSocket.Receive(packet);  // Blocked
                using MemoryStream ms = new MemoryStream(packet);
                using BinaryReader br = new BinaryReader(ms);
                Trace.WriteLine($"\"{br.ReadString()}\" from Client IEP {clientSocket.RemoteEndPoint}, Packet Length: {length}");

                if (br.ReadString() == "exit")
                {
                    clientSocket.Close();
                }
            }
            clientSocket.Close();
        }
    }
}