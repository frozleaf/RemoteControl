using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RemoteControl.Protocals.Request
{
    public class RequestRunExecCode
    {
        public string ID;
        public eExecMode Mode = eExecMode.ExecByPlugin;
        public string FileArguments;
        public bool IsKillMySelf;
    }

    public enum eExecMode
    {
        ExecByPlugin,
        ExecByFile
    }
}
