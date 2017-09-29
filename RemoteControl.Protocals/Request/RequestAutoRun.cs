using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RemoteControl.Protocals.Request
{
    public class RequestAutoRun
    {
        public eAutoRunMode AutoRunMode = eAutoRunMode.ByRegistry;
        public OpeType OperationMode = OpeType.New;
    }

    public enum eAutoRunMode
    {
        ByRegistry,
        BySchedualTask
    }
}
