using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RemoteControl.Protocals
{
    public class RequestStartUploadHeader
    {
        public ePathType PathType { get; private set; }
        public string From;
        public string To;
        public string Id;

        public RequestStartUploadHeader()
        {
            this.PathType = ePathType.File;
        }
    }
}
