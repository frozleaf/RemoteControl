using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.IO;
using System.Drawing.Imaging;

namespace RemoteControl.Protocals
{
    public class ResponseBase
    {
        public bool Result = true;
        public string Message = "成功";
        public string Detail = "";

        protected Image ByteArray2Image(byte[] data)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                ms.Write(data, 0, data.Length);

                return Image.FromStream(ms);
            }
        }

        protected byte[] Image2ByteArray(Image image, ImageFormat imageFormat)
        {
            using (var ms = new MemoryStream())
            {
                image.Save(ms, imageFormat);
                return ms.GetBuffer();
            }
        }
    }
}
