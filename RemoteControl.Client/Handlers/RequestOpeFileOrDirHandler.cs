using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using RemoteControl.Protocals;
using Microsoft.Win32;
using RemoteControl.Protocals.Response;
using RemoteControl.Protocals.Request;
using System.Windows.Forms;
using Microsoft.VisualBasic.Devices;
using RemoteControl.Protocals.Utilities;

namespace RemoteControl.Client.Handlers
{
    [PacketType(ePacketType.PACKET_CREATE_FILE_OR_DIR_REQUEST)]
    [PacketType(ePacketType.PACKET_DELETE_FILE_OR_DIR_REQUEST)]
    [PacketType(ePacketType.PACKET_COPY_FILE_OR_DIR_REQUEST)]
    [PacketType(ePacketType.PACKET_MOVE_FILE_OR_DIR_REQUEST)]
    [PacketType(ePacketType.PACKET_RENAME_FILE_REQUEST)]
    class RequestOpeFileOrDirHandler : AbstractRequestHandler
    {
        public override void Handle(SocketSession session, ePacketType reqType, object reqObj)
        {
            switch (reqType)
            {
                case ePacketType.PACKET_CREATE_FILE_OR_DIR_REQUEST:
                    Create(session, reqType, reqObj);
                    break;
                case ePacketType.PACKET_DELETE_FILE_OR_DIR_REQUEST:
                    Delete(session, reqType, reqObj);
                    break;
                case ePacketType.PACKET_COPY_FILE_OR_DIR_REQUEST:
                    Copy(session, reqType, reqObj);
                    break;
                case ePacketType.PACKET_MOVE_FILE_OR_DIR_REQUEST:
                    Move(session, reqType, reqObj);
                    break;
                case ePacketType.PACKET_RENAME_FILE_REQUEST:
                    Rename(session, reqType, reqObj);
                    break;
            }
        }

        private void Rename(SocketSession session, ePacketType reqType, object reqObj)
        {
            var req = reqObj as RequestRenameFile;
            try
            {
                Computer c = new Computer();
                c.FileSystem.RenameFile(req.SourceFile, req.DestinationFileName);
            }
            catch (Exception ex)
            {
            }
        }

        private void Move(SocketSession session, ePacketType reqType, object reqObj)
        {
            var req = reqObj as RequestMoveFile;
            RunTaskThread(() => CopyFile(session, req.SourceFile, req.DestinationFile, true));
        }

        private void Copy(SocketSession session, ePacketType reqType, object reqObj)
        {
            var req = reqObj as RequestCopyFile;
            RunTaskThread(() => CopyFile(session, req.SourceFile, req.DestinationFile, false));
        }

        private void CopyFile(SocketSession session, string sourceFile, string destFile, bool isDeleteSourceFile)
        {
            ResponseBase resp = null;
            if (isDeleteSourceFile)
            {
                resp = new ResponseMoveFile() { SourceFile = sourceFile, DestinationFile = destFile };
            }
            else
            {
                resp = new ResponseCopyFile() { SourceFile = sourceFile, DestinationFile = destFile };
            }
            try
            {
                string dir = System.IO.Path.GetDirectoryName(destFile);
                string fileName = System.IO.Path.GetFileNameWithoutExtension(destFile);
                string ext = System.IO.Path.GetExtension(destFile);
                string newDestFile = destFile;
                for (int i = 0; ; i++)
                {
                    if (System.IO.File.Exists(newDestFile))
                    {
                        newDestFile = dir + "\\" + fileName + " - 副本" + (i == 0 ? "" : " (" + i + ")") + ext;
                    }
                    else
                    {
                        break;
                    }
                }
                System.IO.File.Copy(sourceFile, newDestFile);
                if (isDeleteSourceFile)
                {
                    System.IO.File.Delete(sourceFile);
                }
            }
            catch (Exception ex)
            {
                resp.Result = false;
                resp.Message = (isDeleteSourceFile ? "移动" : "复制") + "失败," + ex.Message;
                resp.Detail = ex.StackTrace;
            }
            if (isDeleteSourceFile)
            {
                session.Send(ePacketType.PACKET_MOVE_FILE_OR_DIR_RESPONSE, resp);
            }
            else
            {
                session.Send(ePacketType.PACKET_COPY_FILE_OR_DIR_RESPONSE, resp);
            }
        }

        private void Delete(SocketSession session, ePacketType reqType, object reqObj)
        {
            var req = reqObj as RequestDeleteFileOrDir;
            var resp = new ResponseDeleteFileOrDir();
            try
            {
                resp.Path = req.Path;
                resp.PathType = req.PathType;
                if (req.PathType == ePathType.File)
                {
                    System.IO.File.Delete(req.Path);
                }
                else
                {
                    System.IO.Directory.Delete(req.Path);
                }
            }
            catch (Exception ex)
            {
                resp.Result = false;
                resp.Message = ex.ToString();
                resp.Detail = ex.StackTrace.ToString();
            }

            session.Send(ePacketType.PACKET_DELETE_FILE_OR_DIR_RESPONSE, resp);
        }

        private void Create(SocketSession session, ePacketType reqType, object reqObj)
        {
            var req = reqObj as RequestCreateFileOrDir;
            var resp = new ResponseCreateFileOrDir();
            try
            {
                resp.Path = req.Path;
                resp.PathType = req.PathType;
                if (req.PathType == ePathType.File)
                {
                    if (!System.IO.File.Exists(req.Path))
                    {
                        System.IO.File.Create(req.Path).Close();
                    }
                }
                else
                {
                    if (!System.IO.Directory.Exists(req.Path))
                    {
                        System.IO.Directory.CreateDirectory(req.Path);
                    }
                }
            }
            catch (Exception ex)
            {
                resp.Result = false;
                resp.Message = ex.ToString();
                resp.Detail = ex.StackTrace.ToString();
            }

            session.Send(ePacketType.PACKET_CREATE_FILE_OR_DIR_RESPONSE, resp);
        }
    }
}
