using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using System.IO;
using System.Drawing.Imaging;
using System.Drawing;
using RemoteControl.Protocals.Request;
using RemoteControl.Protocals.Response;

namespace RemoteControl.Protocals
{
    public class PacketFactory
    {
        // 包长(4个字节) 包类型(1个字节) 数据部分(包长-4-1个字节)

        public static byte[] EncodeOject(ePacketType packetType, object obj)
        {
            byte[] packet = null;

            byte[] bodyBytes = null;

            if (packetType == ePacketType.PACKET_START_DOWNLOAD_RESPONSE)
            {
                bodyBytes = (byte[])obj;
            }
            else
            {
                if (obj != null)
                {
                    bodyBytes = ToJsonBytes(obj);
                }
            }

            packet = Encode(packetType, bodyBytes);

            return packet;
        }

        public static void DecodeObject(byte[] packetData, out ePacketType packetType, out object obj)
        {
            byte[] bodyData = null;
            Decode(packetData, out packetType, out bodyData);

            obj = null;
            switch (packetType)
            {
                case ePacketType.PACKET_START_CAPTURE_SCREEN_RESPONSE:
                    obj = FromJsonBytes<ResponseStartGetScreen>(bodyData);
                    break;
                case ePacketType.PACKET_START_DOWNLOAD_RESPONSE:
                    obj = bodyData;
                    break;
                case ePacketType.PACKET_GET_DRIVES_RESPONSE:
                    obj = FromJsonBytes<ResponseGetDrives>(bodyData);
                    break;
                case ePacketType.PACKET_GET_SUBFILES_OR_DIRS_REQUEST:
                    obj = FromJsonBytes<RequestGetSubFilesOrDirs>(bodyData);
                    break;
                case ePacketType.PACKET_GET_SUBFILES_OR_DIRS_RESPONSE:
                    obj = FromJsonBytes<ResponseGetSubFilesOrDirs>(bodyData);
                    break;
                case ePacketType.PACKET_CREATE_FILE_OR_DIR_REQUEST:
                    obj = FromJsonBytes<RequestCreateFileOrDir>(bodyData);
                    break;
                case ePacketType.PACKET_CREATE_FILE_OR_DIR_RESPONSE:
                    obj = FromJsonBytes<ResponseCreateFileOrDir>(bodyData);
                    break;
                case ePacketType.PACKET_DELETE_FILE_OR_DIR_REQUEST:
                    obj = FromJsonBytes<RequestDeleteFileOrDir>(bodyData);
                    break;
                case ePacketType.PACKET_DELETE_FILE_OR_DIR_RESPONSE:
                    obj = FromJsonBytes<ResponseDeleteFileOrDir>(bodyData);
                    break;
                case ePacketType.PACKET_START_DOWNLOAD_REQUEST:
                    obj = FromJsonBytes<RequestStartDownload>(bodyData);
                    break;
                case ePacketType.PACKET_START_DOWNLOAD_HEADER_RESPONSE:
                    obj = FromJsonBytes<ResponseStartDownloadHeader>(bodyData);
                    break;
                case ePacketType.PACKET_COMMAND_REQUEST:
                    obj = FromJsonBytes<RequestCommand>(bodyData);
                    break;
                case ePacketType.PACKET_COMMAND_RESPONSE:
                    obj = FromJsonBytes<ResponseCommand>(bodyData);
                    break;
                case ePacketType.PACKET_OPEN_URL_REQUEST:
                    obj = FromJsonBytes<RequestOpenUrl>(bodyData);
                    break;
                case ePacketType.PACKET_MESSAGEBOX_REQUEST:
                    obj = FromJsonBytes<RequestMessageBox>(bodyData);
                    break;
                case ePacketType.PACKET_LOCK_MOUSE_REQUEST:
                    obj = FromJsonBytes<RequestLockMouse>(bodyData);
                    break;
                case ePacketType.PACKET_PLAY_MUSIC_REQUEST:
                    obj = FromJsonBytes<RequestPlayMusic>(bodyData);
                    break;
                case ePacketType.PACKET_DOWNLOAD_WEBFILE_REQUEST:
                    obj = FromJsonBytes<RequestDownloadWebFile>(bodyData);
                    break;
                case ePacketType.PACKET_GET_PROCESSES_RESPONSE:
                    obj = FromJsonBytes<ResponseGetProcesses>(bodyData);
                    break;
                case ePacketType.PACKET_KILL_PROCESS_REQUEST:
                    obj = FromJsonBytes<RequestKillProcesses>(bodyData);
                    break;
                case ePacketType.PACKET_START_CAPTURE_VIDEO_RESPONSE:
                    obj = FromJsonBytes<ResponseStartCaptureVideo>(bodyData);
                    break;
                case ePacketType.PACKET_START_CAPTURE_VIDEO_REQUEST:
                    obj = FromJsonBytes<RequestStartCaptureVideo>(bodyData);
                    break;
                case ePacketType.PACKET_MOUSE_EVENT_REQUEST:
                    obj = FromJsonBytes<RequestMouseEvent>(bodyData);
                    break;
                case ePacketType.PACKET_KEYBOARD_EVENT_REQUEST:
                    obj = FromJsonBytes<RequestKeyboardEvent>(bodyData);
                    break;
                case ePacketType.PACKET_START_UPLOAD_HEADER_REQUEST:
                    obj = FromJsonBytes<RequestStartUploadHeader>(bodyData);
                    break;
                case ePacketType.PACKET_START_UPLOAD_RESPONSE:
                    obj = FromJsonBytes<ResponseStartUpload>(bodyData);
                    break;
                case ePacketType.PACKET_STOP_UPLOAD_REQUEST:
                    obj = FromJsonBytes<RequestStopUpload>(bodyData);
                    break;
                case ePacketType.PACKET_COPY_FILE_OR_DIR_REQUEST:
                    obj = FromJsonBytes<RequestCopyFile>(bodyData);
                    break;
                case ePacketType.PACKET_MOVE_FILE_OR_DIR_REQUEST:
                    obj = FromJsonBytes<RequestMoveFile>(bodyData);
                    break;
                case ePacketType.PACKET_COPY_FILE_OR_DIR_RESPONSE:
                    obj = FromJsonBytes<ResponseCopyFile>(bodyData);
                    break;
                case ePacketType.PACKET_MOVE_FILE_OR_DIR_RESPONSE:
                    obj = FromJsonBytes<ResponseMoveFile>(bodyData);
                    break;
                case ePacketType.PACKET_RENAME_FILE_REQUEST:
                    obj = FromJsonBytes<RequestRenameFile>(bodyData);
                    break;
                case ePacketType.PACKET_TRANSPORT_EXEC_CODE_REQUEST:
                    obj = FromJsonBytes<RequestTransportExecCode>(bodyData);
                    break;
                case ePacketType.PACKET_RUN_EXEC_CODE_REQUEST:
                    obj = FromJsonBytes<RequestRunExecCode>(bodyData);
                    break;
                case ePacketType.PACKET_START_CAPTURE_SCREEN_REQUEST:
                    obj = FromJsonBytes<RequestStartGetScreen>(bodyData);
                    break;
                case ePacketType.PACKET_GET_HOST_NAME_RESPONSE:
                    obj = FromJsonBytes<ResponseGetHostName>(bodyData);
                    break;
            }
        }

        private static byte[] Encode(ePacketType packetType, byte[] bodyData)
        {
            List<byte> result = new List<byte>();

            if(bodyData == null)
            {
                bodyData = new byte[0];
            }
            int packetLength = bodyData.Length + 1 + 4;
            result.AddRange(BitConverter.GetBytes(packetLength));
            result.Add((byte)packetType);
            result.AddRange(bodyData);

            return result.ToArray();
        }

        private static void Decode(byte[] packetData, out ePacketType packetType, out byte[] bodyData)
        {
            int packetLength = BitConverter.ToInt32(packetData, 0);
            packetType = (ePacketType)packetData[4];
            bodyData = new byte[packetLength - 4 - 1];
            for (int i = 0; i < bodyData.Length; i++)
            {
                bodyData[i] = packetData[i + 5];
            }
        }

        private static byte[] ToJsonBytes(object obj)
        {
            string json = JsonConvert.SerializeObject(obj);
            return System.Text.Encoding.UTF8.GetBytes(json);
        }

        private static object FromJsonBytes<T>(byte[] bodyData)
        {
            string json = System.Text.Encoding.UTF8.GetString(bodyData);
            return JsonConvert.DeserializeObject<T>(json);
        }
    }
}
