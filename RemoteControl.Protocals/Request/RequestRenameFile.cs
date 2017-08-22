using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RemoteControl.Protocals
{
    public class RequestRenameFile
    {
        public string SourceFile;
        public string DestinationFileName;
        public string GetDestinationFileName()
        {
            return System.IO.Path.GetFileName(this.DestinationFileName);
        }
    }
}
