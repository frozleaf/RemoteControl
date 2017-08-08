using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RemoteControl.Protocals
{
    public class ProcessProperty
    {
        public string ProcessName;
        public int PID;
        public string User;
        public int CPURate;
        public float PrivateMemory;
        public int ThreadCount;
        public string ExecutablePath;
        public string CommandLine;
        public string FileDescription;
    }
}
