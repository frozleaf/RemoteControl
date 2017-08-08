using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RemoteControl.Protocals
{
    public class RequestStartDownload
    {
        public ePathType PathType { get; private set; }
        public string Path;
        public string SavePath;

        public RequestStartDownload()
        {
            this.PathType = ePathType.File;
        }
    }
}
