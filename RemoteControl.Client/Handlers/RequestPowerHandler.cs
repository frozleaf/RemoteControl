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
    [PacketType(ePacketType.PACKET_SHUTDOWN_REQUEST)]
    [PacketType(ePacketType.PACKET_REBOOT_REQUEST)]
    [PacketType(ePacketType.PACKET_SLEEP_REQUEST)]
    [PacketType(ePacketType.PACKET_HIBERNATE_REQUEST)]
    [PacketType(ePacketType.PACKET_LOCK_REQUEST)]
    class RequestPowerHandler : AbstractRequestHandler
    {
        public override void Handle(SocketSession session, ePacketType reqType, object reqObj)
        {
            if (reqType == ePacketType.PACKET_SHUTDOWN_REQUEST)
            {
                ProcessUtil.Run("shutdown.exe", "-s -t 0", true);
            }
            else if (reqType == ePacketType.PACKET_REBOOT_REQUEST)
            {
                ProcessUtil.Run("shutdown.exe", "-r -t 0", true);
            }
            else if (reqType == ePacketType.PACKET_SLEEP_REQUEST)
            {
                ProcessUtil.Run("rundll32.exe", "powrprof.dll,SetSuspendState 0,1,0", true);
            }
            else if (reqType == ePacketType.PACKET_HIBERNATE_REQUEST)
            {
                ProcessUtil.Run("rundll32.exe", "powrProf.dll,SetSuspendState", true);
            }
            else if (reqType == ePacketType.PACKET_LOCK_REQUEST)
            {
                ProcessUtil.Run("rundll32.exe", "user32.dll,LockWorkStation", true);
            }
        }
    }
}
