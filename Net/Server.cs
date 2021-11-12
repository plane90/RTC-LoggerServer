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
            String = 0,
            Binary = 1,
            exit = 2,
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
                    Trace.WriteLine(e.Message);
                }
                //});
            }
        }

        private static void ReceivePacket(Socket clientSocket)
        {
            byte[] packet = new byte[1000 * 1024];
            while (clientSocket.Connected)
            {
                Trace.WriteLine($"Waiting Packet...");
                clientSocket.Receive(packet);  // Blocked
                using MemoryStream ms = new MemoryStream(packet);
                using BinaryReader br = new BinaryReader(ms);
                var dataType = GetDataType(br);
                Trace.WriteLine($"Received Packet type: {dataType}");
                if (dataType == DataType.exit)
                {
                    clientSocket.Close();
                    break;
                }
                Application.Current.Dispatcher.Invoke(() => Delivery(packet, br, dataType));
            }
        }

        private static DataType GetDataType(BinaryReader br)
        {
            var txt = br.ReadString();
            if (txt == DataType.Binary.GetHashCode().ToString())
            {
                return DataType.Binary;
            }
            else if (txt == DataType.String.GetHashCode().ToString())
            {
                return DataType.String;
            }
            return DataType.exit;
        }

        private static void Delivery(byte[] packet, BinaryReader br, DataType dataType)
        {
            switch (dataType)
            {
                case DataType.String:
                    OnLog?.Invoke(GetLogData(br));
                    break;
                case DataType.Binary:
                    OnFrame?.Invoke(packet);
                    break;
            }
        }

        private static LogData GetLogData(BinaryReader br)
        {
            var logData = new LogData();
            logData.Type = (LogType)int.Parse(br.ReadString());
            logData.DateTime = br.ReadString();
            logData.Message = br.ReadString();
            logData.StackTrace = br.ReadString();
            Trace.WriteLine($"Log: [{logData.DateTime}] \"{logData.Message}\"");
            return logData;
        }
    }
}