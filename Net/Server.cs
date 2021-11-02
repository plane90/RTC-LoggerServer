using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;
using System.Windows.Threading;

namespace RTC_LoggerServer.Net
{

    static class Server
    {
        public static Socket socket;

        public static void RunServer()
        {
            try
            {
                socket = new Socket(
                    AddressFamily.InterNetwork,
                    SocketType.Stream,
                    ProtocolType.Tcp
                    );

                IPAddress addr = IPAddress.Any; // 0.0.0.0
                IPEndPoint iep = new IPEndPoint(addr, 10040);   // 종단점(ip, port)

                socket.Bind(iep); // ip, port 바인딩
                socket.Listen(1000); // 백로그: 보류 중인 연결 큐의 길이

                while (true)
                {
                    var connectedSocket = socket.Accept();   // Blocked at sock.Accept()
                    _ = Task.Factory.StartNew(() =>
                    {
                        ReceivePacket(connectedSocket);
                    });
                }

            }
            catch (SocketException e)
            {
                Trace.WriteLine(e.Message);
            }
            finally
            {
                socket.Dispose();
            }
        }

        private static void ReceivePacket(Socket connectedSock)
        {
            try
            {
                byte[] packet = new byte[8 * 1024];
                IPEndPoint iep = connectedSock.RemoteEndPoint as IPEndPoint;
                //Dispatcher.Invoke(() => LBClientIp.Text = $"{iep}");
                while (true)
                {
                    connectedSock.Receive(packet);  // Blocked
                    //Dispatcher.Invoke(() => TBLoggerWin.Text += $"Packet Received {packet.Length}\n");
                    using MemoryStream ms = new MemoryStream(packet);
                    using BinaryReader br = new BinaryReader(ms);
                    //Dispatcher.Invoke(() => TBLoggerUnity.Text += $"{br.ReadString()}");

                    if (br.ReadString() == "exit")
                    {
                        socket.Dispose();
                    }
                }
            }
            catch (SocketException e)
            {
                Trace.WriteLine(e);
            }
            finally
            {
                Trace.WriteLine("close");
                connectedSock.Close();
            }
        }
    }
}