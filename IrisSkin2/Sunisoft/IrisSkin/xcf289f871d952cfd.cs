namespace Sunisoft.IrisSkin
{
    using System;
    using System.Drawing;
    using System.Windows.Forms;

    internal class xcf289f871d952cfd : x5b126f5f998c28e9
    {
        public xcf289f871d952cfd(IntPtr handle, SkinEngine engine) : base(handle, engine)
        {
        }

        protected override void AfterWndProc(ref Message m)
        {
            x40255b11ef821fa3.PAINTSTRUCT paintstruct;
            uint msg = (uint) m.Msg;
            if (msg <= 15)
            {
                if ((msg != 12) && (msg != 15))
                {
                    return;
                }
            }
            else
            {
                switch (msg)
                {
                    case 0x201:
                    case 0x202:
                        this.PaintControl();
                        return;

                    case 0x203:
                    case 20:
                        goto Label_0035;
                }
                return;
            }
        Label_0035:
            x61467fe65a98f20c.BeginPaint(base.Handle, out paintstruct);
            this.PaintControl();
            x61467fe65a98f20c.EndPaint(base.Handle, ref paintstruct);
        }

        protected override void PaintControl()
        {
            int num;
            if ((x61467fe65a98f20c.SendMessage(base.Handle, 240, IntPtr.Zero, IntPtr.Zero).ToInt32() & 1L) == 1L)
            {
                if (this.Enabled)
                {
                    num = 1;
                }
                else
                {
                    num = 3;
                }
            }
            else if (this.Enabled)
            {
                num = 2;
            }
            else
            {
                num = 4;
            }
            Bitmap image = base.Engine.Res.SplitBitmaps.SKIN2_RADIOBUTTON[num];
            int width = image.Width;
            int height = image.Height;
            if (width < 14)
            {
                width = 14;
            }
            else if (width > 15)
            {
                width = 15;
            }
            if (height < 14)
            {
                height = 14;
            }
            else if (height > 15)
            {
                height = 15;
            }
            using (Bitmap bitmap2 = new Bitmap(width, height))
            {
                Rectangle rectangle;
                Rectangle rectangle2;
                using (Graphics graphics = Graphics.FromImage(bitmap2))
                {
                    rectangle = new Rectangle(0, 0, image.Width, image.Height);
                    rectangle2 = new Rectangle(0, 0, width, height);
                    graphics.FillRectangle(base.Engine.Res.Brushes.SKIN2_FORMCOLOR, 0, 0, width, height);
                    graphics.DrawImage(image, rectangle2, rectangle, GraphicsUnit.Pixel);
                }
                IntPtr windowDC = x61467fe65a98f20c.GetWindowDC(base.Handle);
                using (Graphics graphics2 = Graphics.FromHdc(windowDC))
                {
                    rectangle = new Rectangle(0, 0, width, height);
                    rectangle2 = rectangle;
                    graphics2.DrawImage(bitmap2, rectangle2, rectangle, GraphicsUnit.Pixel);
                }
                x61467fe65a98f20c.ReleaseDC(base.Handle, windowDC);
            }
        }
    }
}

