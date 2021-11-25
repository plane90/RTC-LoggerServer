using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;
using System.Text;
using System;
using System.Windows.Threading;
using System.Windows;

namespace RTC_LoggerServer.Net
{


    static class Server
    {
        public enum DataType
        {
            Log = 0,
            Frame = 1,
            exit = 2,
            keep = 3,
        }
        public enum LogType
        {
            Error = 0,
            Assert = 1,
            Warning = 2,
            Log = 3,
            Exception = 4
        }
        public class LogData
        {
            public LogType Type { get; set; }
            public string DateTime { get; set; }
            public string Message { get; set; }
            public string StackTrace { get; set; } = "";
        }

        public static event Action<LogData> OnLog;
        public static event Action<byte[]> OnFrame;
        private const int HEADER_SIZE = 10;
        private const int CONTENT_SIZE_LOG = 8 * 1024;

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
                //Trace.WriteLine("Connected \n");
                //_ = Task.Run(() =>
                //{
                try
                {
                    ReceivePacket(clientSock);
                }
                catch (Exception e)
                {
                    Trace.WriteLine($"{e.Message}, {e.StackTrace}");
                }
                //});
            }
        }

        private static void ReceivePacket(Socket clientSocket)
        {
            while (clientSocket.Connected)
            {
                Trace.WriteLine($"Waiting Packet...");

                byte[] hPacket = Receive(clientSocket, HEADER_SIZE);
                using MemoryStream ms = new MemoryStream(hPacket);
                using BinaryReader br = new BinaryReader(ms);

                var dataType = GetDataType(br);
                Trace.WriteLine($"Received Packet type: {dataType}");
                if (dataType == DataType.Log)
                {
                    byte[] cPacket = Receive(clientSocket, CONTENT_SIZE_LOG);
                    Application.Current.Dispatcher.Invoke(() => SendMessage(cPacket, dataType));
                }
                if (dataType == DataType.Frame)
                {
                    var length = int.Parse(br.ReadString());
                    byte[] cPacket = Receive(clientSocket, length);
                    Application.Current.Dispatcher.Invoke(() => SendMessage(cPacket, dataType));
                }
                if (dataType == DataType.exit)
                {
                    clientSocket.Close();
                    break;
                }
                if (dataType == DataType.keep)
                {
                    continue;
                }
            }
        }

        private static byte[] Receive(Socket clientSocket, int length)
        {
            byte[] packet = new byte[length];
            var received = 0;
            while (received < length)
            {
                received += clientSocket.Receive(packet,
                    received,
                    length - received,
                    SocketFlags.None);
            }
            Trace.WriteLine($"Received All Packet");
            return packet;
        }

        private static DataType GetDataType(BinaryReader br)
        {
            var txt = br.ReadString();
            if (txt == DataType.Frame.GetHashCode().ToString())
            {
                return DataType.Frame;
            }
            else if (txt == DataType.Log.GetHashCode().ToString())
            {
                return DataType.Log;
            }
            else if (txt == DataType.exit.GetHashCode().ToString())
            {
                return DataType.exit;
            }
            return DataType.keep;
        }

        private static void SendMessage(byte[] packet, DataType dataType)
        {
            switch (dataType)
            {
                case DataType.Log:
                    OnLog?.Invoke(GetLogData(packet));
                    break;
                case DataType.Frame:
                    OnFrame?.Invoke(packet);
                    break;
            }
        }

        private static LogData GetLogData(byte[] packet)
        {
            using MemoryStream ms = new MemoryStream(packet);
            using BinaryReader br = new BinaryReader(ms);
            var logData = new LogData();
            logData.Type = (LogType)int.Parse(br.ReadString());
            logData.DateTime = br.ReadString();
            logData.Message = br.ReadString();
            logData.StackTrace = br.ReadString();
            return logData;
        }

        //private static byte[] GetEncodedFrame(BinaryReader br)
        //{
        //    var length = int.Parse(br.ReadString());
        //    byte[] encodedFrame = new byte[length];
        //    br.Read(encodedFrame);

        //    return encodedFrame;
        //}
    }
}