using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RemoteControl.Server
{
    class ListViewItemFileOrDirTag
    {
        public string Path;
        public bool IsFile = false;

        public string FileOrDirName
        {
            get
            {
                if (string.IsNullOrEmpty(this.Path))
                    return string.Empty;

                if (this.IsFile)
                {
                    return System.IO.Path.GetFileName(this.Path);
                }
                return System.IO.Path.GetDirectoryName(this.Path);
            }
        }
    }
}
