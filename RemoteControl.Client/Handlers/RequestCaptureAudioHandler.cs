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

namespace RemoteControl.Client
{
    class RequestCaptureAudioHandler : IRequestHandler
    {
        private WaveIn _waveIn = null;
        private SocketSession _session;

        public void Handle(SocketSession session, ePacketType reqType, object reqObj)
        {
            _session = session;
            if (reqType == ePacketType.PACKET_STOP_CAPTURE_AUDIO_REQUEST)
            {
                StopTest();
                return;
                if(_waveIn!=null)
                {
                    _waveIn.Dispose();
                    _waveIn = null;
                }
            }
            else if (reqType == ePacketType.PACKET_START_CAPTURE_AUDIO_REQUEST)
            {
                RequestStartCaptureAudio req = reqObj as RequestStartCaptureAudio;
                ResponseStartCaptureAudio resp = new ResponseStartCaptureAudio();
                try
                {
                    if (_waveIn != null)
                    {
                        return;
                    }
                    StartTest();
                    return;
                    if (WaveIn.Devices.Length > 0)
                    {
                        _waveIn = new WaveIn(WaveIn.Devices[0], 8000, 16, 1, 400);
                        _waveIn.BufferFull += new BufferFullHandler(m_pWaveIn_BufferFull);
                        _waveIn.Start();
                    }
                    else
                    {
                        throw new Exception("未找到可用的音频设备");
                    }
                }
                catch (Exception ex)
                {
                    resp.Result = false;
                    resp.Message = ex.ToString();
                    resp.Detail = ex.StackTrace.ToString();

                    session.Send(ePacketType.PACKET_START_CAPTURE_AUDIO_RESPONSE, resp);
                }
            }
        }

        void m_pWaveIn_BufferFull(byte[] buffer)
        {
            ResponseStartCaptureAudio resp = new ResponseStartCaptureAudio();
            resp.AudioData = G711.Encode_aLaw(buffer, 0, buffer.Length);
            _session.Send(ePacketType.PACKET_START_CAPTURE_AUDIO_RESPONSE, resp);
        }

        bool _isTestRunning = false;
        void StartTest()
        {
            new Thread(() => {
                _isTestRunning = true;
                using (FileStream fs = File.OpenRead(Utils.GetAppPath() + "\\audio\\futurama.raw"))
                {
                    byte[] buffer = new byte[400];
                    int readedCount = fs.Read(buffer, 0, buffer.Length);
                    long lastSendTime = DateTime.Now.Ticks;
                    while (readedCount > 0)
                    {
                        if (!_isTestRunning)
                            return;

                        m_pWaveIn_BufferFull(buffer);

                        readedCount = fs.Read(buffer, 0, buffer.Length);

                        Thread.Sleep(25);

                        lastSendTime = DateTime.Now.Ticks;
                    }
                }
            }) { IsBackground=true}.Start();
        }

        void StopTest()
        {
            _isTestRunning = false;
        }
    }
}
