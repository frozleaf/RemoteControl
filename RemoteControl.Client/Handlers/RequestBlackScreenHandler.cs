using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RemoteControl.Protocals;
using Microsoft.Win32;
using RemoteControl.Audio;
using RemoteControl.Audio.Codecs;
using System.Threading;
using System.IO;
using RemoteControl.Protocals.Response;
using RemoteControl.Protocals.Utilities;

namespace RemoteControl.Client.Handlers
{
    class RequestBlackScreenHandler : AbstractRequestHandler
    {
        private bool _isRunning = false;
        public override void Handle(SocketSession session, ePacketType reqType, object reqObj)
        {
            if (reqType == ePacketType.PAKCET_BLACK_SCREEN_REQUEST)
            {
                // 开始
                if (_isRunning)
                    return;
                RunTaskThread(StartBlackScreen, session);
            }
            else if (reqType == ePacketType.PAKCET_UN_BLACK_SCREEN_REQUEST)
            {
                // 停止
                _isRunning = false;
                RunTaskThread(StartUnBlackScreen, session);
            }
        }

        private void StartBlackScreen(SocketSession session)
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

        private void StartUnBlackScreen(SocketSession session)
        {
            try
            {
                ProcessUtil.KillProcess("blackscreen");

                Win32API.ShowWindow(Win32API.FindWindow(Win32API.Shell_TrayWnd_Name, null), Win32API.SW_SHOW);
            }
            catch (Exception ex)
            {

            }
        }
    }
}
