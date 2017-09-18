using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Drawing.Imaging;

namespace RemoteControl.Protocals
{
    public class ResponseStartCaptureAudio : ResponseBase
    {
        public byte[] AudioData;
    }
}
