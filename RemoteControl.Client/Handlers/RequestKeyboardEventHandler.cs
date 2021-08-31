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
    [PacketType(ePacketType.PACKET_KEYBOARD_EVENT_REQUEST)]
    class RequestKeyboardEventHandler : AbstractRequestHandler
    {
        public override void Handle(SocketSession session, ePacketType reqType, object reqObj)
        {
            var req = reqObj as RequestKeyboardEvent;

            DoOutput(string.Format("keyCode:{0},keyValue:{1},keyOperation:{2}",
                    req.KeyCode, req.KeyValue, req.KeyOperation));
            KeyboardOpeUtil.KeyOpe(req.KeyCode, req.KeyOperation);
        }
    }
}
