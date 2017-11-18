using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RemoteControl.Protocals
{
    public class ResponseGetDrives : ResponseBase
    {
        public List<string> drives;
    }
}
