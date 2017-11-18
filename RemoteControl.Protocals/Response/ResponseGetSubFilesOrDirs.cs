using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RemoteControl.Protocals
{
    public class ResponseGetSubFilesOrDirs:ResponseBase
    {
        public List<DirectoryProperty> dirs;
        public List<FileProperty> files;
    }

    public class DirectoryProperty
    {
        public string DirPath;
        public DateTime CreationTime;
        public DateTime LastWriteTime;
        public DateTime LastAccessTime;
    }

    public class FileProperty
    {
        public string FilePath;
        public long Size;
        public DateTime CreationTime;
        public DateTime LastWriteTime;
        public DateTime LastAccessTime;
    }
}
