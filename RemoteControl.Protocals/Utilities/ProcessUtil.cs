using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Diagnostics;
using System.Management;

namespace RemoteControl.Protocals.Utilities
{
    /// <summary>
    /// 进程工具类
    /// </summary>
    public class ProcessUtil
    {
        public static Thread Run(string appFileName, string arguments, bool hideWindow)
        {
            return Run(appFileName, arguments, hideWindow, false);
        }

        public static Thread Run(string appFileName, string arguments, bool hideWindow, bool useShellExecute)
        {
            var t = new Thread(() =>
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
            }) { IsBackground = true };
            t.Start();

            return t;
        }

        public static List<ProcessProperty> GetProcessProperyList()
        {
            List<ProcessProperty> list = new List<ProcessProperty>();

            var pros = System.Diagnostics.Process.GetProcesses();
            for (int i = 0; i < pros.Length; i++)
            {
                Process process = pros[i];
                ProcessProperty property = new ProcessProperty();
                property.ProcessName = process.ProcessName;
                property.PID = process.Id;
                property.ThreadCount = process.Threads.Count;
                property.PrivateMemory = GetProcessPrivateMeory(property.ProcessName);
                string executablePath, commandLine;
                GetProcessExcutablePath(property.PID, out executablePath, out commandLine);
                property.CommandLine = commandLine;
                if (process.MainWindowHandle == IntPtr.Zero)
                {
                    property.ExecutablePath = executablePath;
                }
                else
                {
                    property.ExecutablePath = process.MainModule.FileName;
                }
                property.FileDescription = System.IO.Path.GetFileName(property.ExecutablePath);

                list.Add(property);
            }

            return list;
        }

        private static float GetProcessPrivateMeory(string processName)
        {
            PerformanceCounter pc = new PerformanceCounter("Process", "Working Set - Private", processName);

            return pc.NextValue();
        }

        private static void GetProcessExcutablePath(int processId, out string executablePath, out string commandLine)
        {
            /* Win32_Process的结构见：
             * https://msdn.microsoft.com/en-us/library/aa394372(v=vs.85).aspx
             */

            executablePath = string.Empty;
            commandLine = string.Empty;
            string wmiQueryString = "SELECT ProcessId, ExecutablePath, CommandLine FROM Win32_Process WHERE ProcessId = " + processId;
            using (var searcher = new ManagementObjectSearcher(wmiQueryString))
            {
                using (var results = searcher.Get())
                {
                    ManagementObject mo = results.Cast<ManagementObject>().FirstOrDefault();
                    if (mo != null)
                    {
                        executablePath = (string)mo["ExecutablePath"];
                        commandLine = (string)mo["CommandLine"];
                    }
                }
            }
        }

        public static bool KillProcess(string processNameInLower)
        {
            var pros = Process.GetProcesses();
            for (int i = 0; i < pros.Length; i++)
            {
                Process p = pros[i];
                if (p.ProcessName.ToLower().Contains(processNameInLower))
                {
                    try
                    {
                        p.Kill();
                        return true;
                    }
                    catch
                    {
                        return false;
                    }
                }
            }

            return false;
        }
    }
}
