using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RemoteControl.Protocals;

namespace RemoteControl.Client.Handlers
{
    interface IRequestHandler
    {
        void Handle(SocketSession session, ePacketType reqType, object reqObj);
    }
}
