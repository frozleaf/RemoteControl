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
    class RequestQuitAppHandler : AbstractRequestHandler
    {
        public override void Handle(SocketSession session, ePacketType reqType, object reqObj)
        {
            if (OnFireQuit != null)
            {
                OnFireQuit(null, null);
            }
        }
    }
}
