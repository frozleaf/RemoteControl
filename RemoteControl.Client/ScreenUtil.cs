using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using System.Drawing;
using System.Windows.Forms;

namespace RemoteControl.Client
{
    class ScreenUtil
    {
        /// <summary>
        /// 捕获屏幕
        /// <para>不支持未登陆时截图</para>
        /// </summary>
        /// <returns></returns>
        public static Image CaptureScreen1()
        {
            Image myImage = new Bitmap(Screen.PrimaryScreen.Bounds.Width, Screen.PrimaryScreen.Bounds.Height);
            Graphics g = Graphics.FromImage(myImage);
            g.CopyFromScreen(new Point(0, 0), new Point(0, 0), new Size(Screen.PrimaryScreen.Bounds.Width, Screen.PrimaryScreen.Bounds.Height));
            IntPtr pDC = g.GetHdc();
            g.ReleaseHdc(pDC);

            return myImage;
        }

        [DllImport("Gdi32.dll")]
        public extern static int BitBlt(IntPtr hDC, int x, int y, int nWidth, int nHeight, IntPtr hSrcDC, int xSrc, int ySrc, int dwRop);

        /// <summary>
        /// 捕获屏幕
        /// <para>支持未登陆时截图</para>
        /// </summary>
        /// <returns></returns>
        public static Image CaptureScreen2()
        {
            int width = Screen.PrimaryScreen.Bounds.Width;
            int height = Screen.PrimaryScreen.Bounds.Height;
            Bitmap screenCopy = new Bitmap(width, height);
            using (Graphics gDest = Graphics.FromImage(screenCopy))
            {
                Graphics gSrc = Graphics.FromHwnd(IntPtr.Zero);
                IntPtr hSrcDC = gSrc.GetHdc();
                IntPtr hDC = gDest.GetHdc();
                int retval = BitBlt(hDC, 0, 0, width, height, hSrcDC, 0, 0, (int)(CopyPixelOperation.SourceCopy | CopyPixelOperation.CaptureBlt));
                gDest.ReleaseHdc();
                gSrc.ReleaseHdc();
            }

            return screenCopy;
        }
    }
}
