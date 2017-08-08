using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RemoteControl.Protocals
{
    public class ResponseGetProcesses : ResponseBase
    {
        public List<ProcessProperty> Processes = new List<ProcessProperty>();
    }
}
