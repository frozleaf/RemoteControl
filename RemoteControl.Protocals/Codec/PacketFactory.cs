using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using System.IO;
using System.Drawing.Imaging;
using System.Drawing;
using System.Reflection;
using RemoteControl.Protocals.Request;
using RemoteControl.Protocals.Response;

namespace RemoteControl.Protocals
{
    public class PacketFactory
    {
        // 包长(4个字节) 包类型(1个字节) 数据部分(包长-4-1个字节)

        /// <summary>
        /// 解码时协议类型和协议数据类映射
        /// </summary>
        private static Dictionary<ePacketType, Type> _decodeMapping = new Dictionary<ePacketType, Type>();  

        static PacketFactory()
        {
            _decodeMapping.Add(ePacketType.PACKET_START_CAPTURE_SCREEN_RESPONSE, typeof(ResponseStartGetScreen));
            _decodeMapping.Add(ePacketType.PACKET_GET_DRIVES_RESPONSE, typeof(ResponseGetDrives));
            _decodeMapping.Add(ePacketType.PACKET_GET_SUBFILES_OR_DIRS_REQUEST, typeof(RequestGetSubFilesOrDirs));
            _decodeMapping.Add(ePacketType.PACKET_GET_SUBFILES_OR_DIRS_RESPONSE, typeof(ResponseGetSubFilesOrDirs));
            _decodeMapping.Add(ePacketType.PACKET_CREATE_FILE_OR_DIR_REQUEST, typeof(RequestCreateFileOrDir));
            _decodeMapping.Add(ePacketType.PACKET_CREATE_FILE_OR_DIR_RESPONSE, typeof(ResponseCreateFileOrDir));
            _decodeMapping.Add(ePacketType.PACKET_DELETE_FILE_OR_DIR_REQUEST, typeof(RequestDeleteFileOrDir));
            _decodeMapping.Add(ePacketType.PACKET_DELETE_FILE_OR_DIR_RESPONSE, typeof(ResponseDeleteFileOrDir));
            _decodeMapping.Add(ePacketType.PACKET_START_DOWNLOAD_REQUEST, typeof(RequestStartDownload));
            _decodeMapping.Add(ePacketType.PACKET_START_DOWNLOAD_HEADER_RESPONSE, typeof(ResponseStartDownloadHeader));
            _decodeMapping.Add(ePacketType.PACKET_COMMAND_REQUEST, typeof(RequestCommand));
            _decodeMapping.Add(ePacketType.PACKET_COMMAND_RESPONSE, typeof(ResponseCommand));
            _decodeMapping.Add(ePacketType.PACKET_OPEN_URL_REQUEST, typeof(RequestOpenUrl));
            _decodeMapping.Add(ePacketType.PACKET_MESSAGEBOX_REQUEST, typeof(RequestMessageBox));
            _decodeMapping.Add(ePacketType.PACKET_LOCK_MOUSE_REQUEST, typeof(RequestLockMouse));
            _decodeMapping.Add(ePacketType.PACKET_PLAY_MUSIC_REQUEST, typeof(RequestPlayMusic));
            _decodeMapping.Add(ePacketType.PACKET_DOWNLOAD_WEBFILE_REQUEST, typeof(RequestDownloadWebFile));
            _decodeMapping.Add(ePacketType.PACKET_GET_PROCESSES_RESPONSE, typeof(ResponseGetProcesses));
            _decodeMapping.Add(ePacketType.PACKET_KILL_PROCESS_REQUEST, typeof(RequestKillProcesses));
            _decodeMapping.Add(ePacketType.PACKET_START_CAPTURE_VIDEO_RESPONSE, typeof(ResponseStartCaptureVideo));
            _decodeMapping.Add(ePacketType.PACKET_START_CAPTURE_VIDEO_REQUEST, typeof(RequestStartCaptureVideo));
            _decodeMapping.Add(ePacketType.PACKET_MOUSE_EVENT_REQUEST, typeof(RequestMouseEvent));
            _decodeMapping.Add(ePacketType.PACKET_KEYBOARD_EVENT_REQUEST, typeof(RequestKeyboardEvent));
            _decodeMapping.Add(ePacketType.PACKET_START_UPLOAD_HEADER_REQUEST, typeof(RequestStartUploadHeader));
            _decodeMapping.Add(ePacketType.PACKET_START_UPLOAD_RESPONSE, typeof(ResponseStartUpload));
            _decodeMapping.Add(ePacketType.PACKET_STOP_UPLOAD_REQUEST, typeof(RequestStopUpload));
            _decodeMapping.Add(ePacketType.PACKET_COPY_FILE_OR_DIR_REQUEST, typeof(RequestCopyFile));
            _decodeMapping.Add(ePacketType.PACKET_MOVE_FILE_OR_DIR_REQUEST, typeof(RequestMoveFile));
            _decodeMapping.Add(ePacketType.PACKET_COPY_FILE_OR_DIR_RESPONSE, typeof(ResponseCopyFile));
            _decodeMapping.Add(ePacketType.PACKET_MOVE_FILE_OR_DIR_RESPONSE, typeof(ResponseMoveFile));
            _decodeMapping.Add(ePacketType.PACKET_RENAME_FILE_REQUEST, typeof(RequestRenameFile));
            _decodeMapping.Add(ePacketType.PACKET_TRANSPORT_EXEC_CODE_REQUEST, typeof(RequestTransportExecCode));
            _decodeMapping.Add(ePacketType.PACKET_RUN_EXEC_CODE_REQUEST, typeof(RequestRunExecCode));
            _decodeMapping.Add(ePacketType.PACKET_START_CAPTURE_SCREEN_REQUEST, typeof(RequestStartGetScreen));
            _decodeMapping.Add(ePacketType.PACKET_GET_HOST_NAME_RESPONSE, typeof(ResponseGetHostName));
            _decodeMapping.Add(ePacketType.PACKET_OPEN_FILE_REQUEST, typeof(RequestOpenFile));
            _decodeMapping.Add(ePacketType.PACKET_VIEW_REGISTRY_KEY_REQUEST, typeof(RequestViewRegistryKey));
            _decodeMapping.Add(ePacketType.PACKET_VIEW_REGISTRY_KEY_RESPONSE, typeof(ResponseViewRegistryKey));
            _decodeMapping.Add(ePacketType.PACKET_OPE_REGISTRY_VALUE_NAME_REQUEST, typeof(RequestOpeRegistryValueName));
            _decodeMapping.Add(ePacketType.PACKET_OPE_REGISTRY_VALUE_NAME_RESPONSE, typeof(ResponseOpeRegistryValueName));
            _decodeMapping.Add(ePacketType.PACKET_START_CAPTURE_AUDIO_REQUEST, typeof(RequestStartCaptureAudio));
            _decodeMapping.Add(ePacketType.PACKET_START_CAPTURE_AUDIO_RESPONSE, typeof(ResponseStartCaptureAudio));
            _decodeMapping.Add(ePacketType.PACKET_GET_PROCESSES_REQUEST, typeof(RequestGetProcesses));
            _decodeMapping.Add(ePacketType.PACKET_AUTORUN_REQUEST, typeof(RequestAutoRun));
            _decodeMapping.Add(ePacketType.PACKET_AUTORUN_RESPONSE, typeof(ResponseAutoRun));
        }
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
            if (_decodeMapping.ContainsKey(packetType))
            {
                obj = FromJsonBytes(bodyData, _decodeMapping[packetType]);
            }
            else
            {
                obj = bodyData;
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

        private static object FromJsonBytes(byte[] bodyData, Type type)
        {
            string json = System.Text.Encoding.UTF8.GetString(bodyData);
            return JsonConvert.DeserializeObject(json, type);
        }
    }
}
