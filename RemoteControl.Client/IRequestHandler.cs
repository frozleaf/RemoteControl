using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RemoteControl.Protocals;

namespace RemoteControl.Client
{
    interface IRequestHandler
    {
        void Handle(SocketSession session, ePacketType reqType, object reqObj);
    }
}
