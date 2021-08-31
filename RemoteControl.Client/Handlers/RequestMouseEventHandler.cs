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
    [PacketType(ePacketType.PACKET_MOUSE_EVENT_REQUEST)]
    class RequestMouseEventHandler : AbstractRequestHandler
    {
        public override void Handle(SocketSession session, ePacketType reqType, object reqObj)
        {
            var req = reqObj as RequestMouseEvent;

            DoOutput(string.Format("button:{0},operation:{1},location:{2},{3}",
                    req.MouseButton,
                    req.MouseOperation,
                    req.MouseLocation.X, req.MouseLocation.Y));

            if (req.MouseOperation == eMouseOperations.MouseDown)
            {
                MouseOpeUtil.MouseDown(req.MouseButton, req.MouseLocation);
            }
            else if (req.MouseOperation == eMouseOperations.MouseUp)
            {
                MouseOpeUtil.MouseUp(req.MouseButton, req.MouseLocation);
            }
            else if (req.MouseOperation == eMouseOperations.MousePress)
            {
                MouseOpeUtil.MousePress(req.MouseButton, req.MouseLocation);
            }
            else if (req.MouseOperation == eMouseOperations.MouseDoubleClick)
            {
                MouseOpeUtil.MouseDoubleClick(req.MouseButton, req.MouseLocation);
            }
            else if (req.MouseOperation == eMouseOperations.MouseMove)
            {
                MouseOpeUtil.MouseMove(req.MouseLocation);
            }
            else
            {
                return;
            }
        }
    }
}
