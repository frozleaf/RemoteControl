using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using System.IO;
using System.Drawing.Imaging;
using System.Drawing;
using System.Reflection;
using RemoteControl.Protocals.Codec;
using RemoteControl.Protocals.Request;
using RemoteControl.Protocals.Response;

namespace RemoteControl.Protocals.Codec
{
    public class CodecFactory
    {
        private static CodecFactoryBase _coder = null;

        public static CodecFactoryBase Instance
        {
            get
            {
                if(_coder!=null)
                    return _coder;

                var mappings = GetMappings();
                _coder = new CodecFactoryBase(mappings);

                return _coder;
            }
        }
        private static Dictionary<ePacketType, Type> GetMappings()
        {
            Dictionary<ePacketType, Type> mappings = new Dictionary<ePacketType, Type>();

            mappings.Add(ePacketType.PACKET_START_CAPTURE_SCREEN_RESPONSE, typeof(ResponseStartGetScreen));
            mappings.Add(ePacketType.PACKET_GET_DRIVES_RESPONSE, typeof(ResponseGetDrives));
            mappings.Add(ePacketType.PACKET_GET_SUBFILES_OR_DIRS_REQUEST, typeof(RequestGetSubFilesOrDirs));
            mappings.Add(ePacketType.PACKET_GET_SUBFILES_OR_DIRS_RESPONSE, typeof(ResponseGetSubFilesOrDirs));
            mappings.Add(ePacketType.PACKET_CREATE_FILE_OR_DIR_REQUEST, typeof(RequestCreateFileOrDir));
            mappings.Add(ePacketType.PACKET_CREATE_FILE_OR_DIR_RESPONSE, typeof(ResponseCreateFileOrDir));
            mappings.Add(ePacketType.PACKET_DELETE_FILE_OR_DIR_REQUEST, typeof(RequestDeleteFileOrDir));
            mappings.Add(ePacketType.PACKET_DELETE_FILE_OR_DIR_RESPONSE, typeof(ResponseDeleteFileOrDir));
            mappings.Add(ePacketType.PACKET_START_DOWNLOAD_REQUEST, typeof(RequestStartDownload));
            mappings.Add(ePacketType.PACKET_START_DOWNLOAD_HEADER_RESPONSE, typeof(ResponseStartDownloadHeader));
            mappings.Add(ePacketType.PACKET_COMMAND_REQUEST, typeof(RequestCommand));
            mappings.Add(ePacketType.PACKET_COMMAND_RESPONSE, typeof(ResponseCommand));
            mappings.Add(ePacketType.PACKET_OPEN_URL_REQUEST, typeof(RequestOpenUrl));
            mappings.Add(ePacketType.PACKET_MESSAGEBOX_REQUEST, typeof(RequestMessageBox));
            mappings.Add(ePacketType.PACKET_LOCK_MOUSE_REQUEST, typeof(RequestLockMouse));
            mappings.Add(ePacketType.PACKET_PLAY_MUSIC_REQUEST, typeof(RequestPlayMusic));
            mappings.Add(ePacketType.PACKET_DOWNLOAD_WEBFILE_REQUEST, typeof(RequestDownloadWebFile));
            mappings.Add(ePacketType.PACKET_GET_PROCESSES_RESPONSE, typeof(ResponseGetProcesses));
            mappings.Add(ePacketType.PACKET_KILL_PROCESS_REQUEST, typeof(RequestKillProcesses));
            mappings.Add(ePacketType.PACKET_START_CAPTURE_VIDEO_RESPONSE, typeof(ResponseStartCaptureVideo));
            mappings.Add(ePacketType.PACKET_START_CAPTURE_VIDEO_REQUEST, typeof(RequestStartCaptureVideo));
            mappings.Add(ePacketType.PACKET_MOUSE_EVENT_REQUEST, typeof(RequestMouseEvent));
            mappings.Add(ePacketType.PACKET_KEYBOARD_EVENT_REQUEST, typeof(RequestKeyboardEvent));
            mappings.Add(ePacketType.PACKET_START_UPLOAD_HEADER_REQUEST, typeof(RequestStartUploadHeader));
            mappings.Add(ePacketType.PACKET_START_UPLOAD_RESPONSE, typeof(ResponseStartUpload));
            mappings.Add(ePacketType.PACKET_STOP_UPLOAD_REQUEST, typeof(RequestStopUpload));
            mappings.Add(ePacketType.PACKET_COPY_FILE_OR_DIR_REQUEST, typeof(RequestCopyFile));
            mappings.Add(ePacketType.PACKET_MOVE_FILE_OR_DIR_REQUEST, typeof(RequestMoveFile));
            mappings.Add(ePacketType.PACKET_COPY_FILE_OR_DIR_RESPONSE, typeof(ResponseCopyFile));
            mappings.Add(ePacketType.PACKET_MOVE_FILE_OR_DIR_RESPONSE, typeof(ResponseMoveFile));
            mappings.Add(ePacketType.PACKET_RENAME_FILE_REQUEST, typeof(RequestRenameFile));
            mappings.Add(ePacketType.PACKET_TRANSPORT_EXEC_CODE_REQUEST, typeof(RequestTransportExecCode));
            mappings.Add(ePacketType.PACKET_RUN_EXEC_CODE_REQUEST, typeof(RequestRunExecCode));
            mappings.Add(ePacketType.PACKET_START_CAPTURE_SCREEN_REQUEST, typeof(RequestStartGetScreen));
            mappings.Add(ePacketType.PACKET_GET_HOST_NAME_RESPONSE, typeof(ResponseGetHostName));
            mappings.Add(ePacketType.PACKET_OPEN_FILE_REQUEST, typeof(RequestOpenFile));
            mappings.Add(ePacketType.PACKET_VIEW_REGISTRY_KEY_REQUEST, typeof(RequestViewRegistryKey));
            mappings.Add(ePacketType.PACKET_VIEW_REGISTRY_KEY_RESPONSE, typeof(ResponseViewRegistryKey));
            mappings.Add(ePacketType.PACKET_OPE_REGISTRY_VALUE_NAME_REQUEST, typeof(RequestOpeRegistryValueName));
            mappings.Add(ePacketType.PACKET_OPE_REGISTRY_VALUE_NAME_RESPONSE, typeof(ResponseOpeRegistryValueName));
            mappings.Add(ePacketType.PACKET_START_CAPTURE_AUDIO_REQUEST, typeof(RequestStartCaptureAudio));
            mappings.Add(ePacketType.PACKET_START_CAPTURE_AUDIO_RESPONSE, typeof(ResponseStartCaptureAudio));
            mappings.Add(ePacketType.PACKET_GET_PROCESSES_REQUEST, typeof(RequestGetProcesses));
            mappings.Add(ePacketType.PACKET_AUTORUN_REQUEST, typeof(RequestAutoRun));
            mappings.Add(ePacketType.PACKET_AUTORUN_RESPONSE, typeof(ResponseAutoRun));
            mappings.Add(ePacketType.PACKET_START_DOWNLOAD_RESPONSE, typeof(ResponseStartDownload));

            return mappings;
        }
    }
}
