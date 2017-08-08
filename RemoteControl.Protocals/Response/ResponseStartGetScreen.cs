using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Drawing.Imaging;

namespace RemoteControl.Protocals
{
    public class ResponseStartGetScreen : ResponseBase
    {
        public byte[] ImageData;

        public Image GetImage()
        {
            return ByteArray2Image(this.ImageData);
        }

        public void SetImage(Image image, ImageFormat imageFormat)
        {
            this.ImageData = Image2ByteArray(image, imageFormat);
        }
    }
}
