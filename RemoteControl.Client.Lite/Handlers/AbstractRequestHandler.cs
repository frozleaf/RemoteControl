using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using RemoteControl.Protocals;
using System.Diagnostics;

namespace RemoteControl.Client.Handlers
{
    abstract class AbstractRequestHandler:IRequestHandler
    {
        public Action<object,EventArgs> OnFireQuit;
        public abstract void Handle(Protocals.SocketSession session, Protocals.ePacketType reqType, object reqObj);

        protected Thread RunTaskThread(Action action)
        {
            Thread t = new Thread(()=>action());
            t.IsBackground = true;
            t.Start();
            return t;
        }

        protected Thread RunTaskThread(Action<SocketSession> action, SocketSession session)
        {
            return RunTaskThread(() => action(session));
        }

        protected void DoOutput(string sMsg)
        {
            Console.WriteLine("{0} {1}", DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"), sMsg);
        }

        protected string GetTempPath()
        {
            return Environment.GetEnvironmentVariable("temp");
        }

        protected int FindServerPortByProcessName(string processName)
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
    }
}
