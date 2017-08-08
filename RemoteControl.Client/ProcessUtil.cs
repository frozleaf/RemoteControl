using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using RemoteControl.Protocals;
using System.Management;
using System.IO;

namespace RemoteControl.Client
{
    public class ProcessUtil
    {
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
    }
}
