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
    [PacketType(ePacketType.PACKET_RESTART_APP_REQUEST)]
    class RequestRestartAppHandler : AbstractRequestHandler
    {
        public override void Handle(SocketSession session, ePacketType reqType, object reqObj)
        {
            string path = System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName;
            var thread = ProcessUtil.Run(path, "/r", true);
            thread.Join();
            RemoteControlApplication.FireQuitEvent();
        }
    }
}
