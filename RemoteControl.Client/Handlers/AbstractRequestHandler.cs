using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using RemoteControl.Protocals;

namespace RemoteControl.Client.Handlers
{
    abstract class AbstractRequestHandler:IRequestHandler
    {
        public abstract void Handle(Protocals.SocketSession session, Protocals.ePacketType reqType, object reqObj);

        protected Thread RunTaskThread(Action action)
        {
            Thread t = new Thread(()=>action());
            t.IsBackground = true;
            t.Start();
            return t;
        }

        protected Thread RunTaskThread(Action<SocketSession> action, SocketSession session)
        {
            return RunTaskThread(() => action(session));
        }
    }
}
