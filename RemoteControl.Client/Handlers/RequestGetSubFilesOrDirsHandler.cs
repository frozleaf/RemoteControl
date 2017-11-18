using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using RemoteControl.Protocals;
using Microsoft.Win32;
using RemoteControl.Protocals.Response;
using RemoteControl.Protocals.Request;
using System.Windows.Forms;
using RemoteControl.Protocals.Utilities;

namespace RemoteControl.Client.Handlers
{
    class RequestGetSubFilesOrDirsHandler : IRequestHandler
    {
        public void Handle(SocketSession session, ePacketType reqType, object reqObj)
        {
            var req = reqObj as RequestGetSubFilesOrDirs;
            var resp = new ResponseGetSubFilesOrDirs();
            try
            {
                resp.dirs = new List<Protocals.DirectoryProperty>();
                var dirs = System.IO.Directory.GetDirectories(req.parentDir).ToList();
                foreach (var item in dirs)
                {
                    DirectoryInfo di = new DirectoryInfo(item);
                    resp.dirs.Add(new DirectoryProperty()
                    {
                        DirPath = item,
                        CreationTime = di.CreationTime,
                        LastWriteTime = di.LastWriteTime,
                        LastAccessTime = di.LastAccessTime
                    });
                }

                resp.files = new List<FileProperty>();
                var files = System.IO.Directory.GetFiles(req.parentDir).ToList();
                foreach (var item in files)
                {
                    FileInfo fi = new FileInfo(item);
                    resp.files.Add(new FileProperty()
                    {
                        FilePath = item,
                        Size = fi.Length,
                        CreationTime = fi.CreationTime,
                        LastWriteTime = fi.LastWriteTime,
                        LastAccessTime = fi.LastAccessTime
                    });
                }
            }
            catch (Exception ex)
            {
                resp.Result = false;
                resp.Message = ex.ToString();
                resp.Detail = ex.StackTrace.ToString();
            }

            session.Send(ePacketType.PACKET_GET_SUBFILES_OR_DIRS_RESPONSE, resp);
        }
    }
}
