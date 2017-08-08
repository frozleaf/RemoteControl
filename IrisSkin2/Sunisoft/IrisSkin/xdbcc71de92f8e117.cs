namespace Sunisoft.IrisSkin
{
    using System;
    using System.Drawing;

    internal class xdbcc71de92f8e117 : x5b126f5f998c28e9
    {
        public xdbcc71de92f8e117(IntPtr handle, SkinEngine engine) : base(handle, engine)
        {
        }

        protected override void PaintControl()
        {
            int index = 1;
            switch (((uint) ((int) x61467fe65a98f20c.SendMessage(base.Handle, 240, IntPtr.Zero, IntPtr.Zero))))
            {
                case 1:
                    if (this.Enabled)
                    {
                        index = 1;
                    }
                    else
                    {
                        index = 4;
                    }
                    break;

                case 2:
                    if (this.Enabled)
                    {
                        index = 2;
                    }
                    else
                    {
                        index = 5;
                    }
                    break;

                default:
                    if (this.Enabled)
                    {
                        index = 3;
                    }
                    else
                    {
                        index = 6;
                    }
                    break;
            }
            Bitmap image = base.Engine.Res.SplitBitmaps.SKIN2_CHECKBOX[index];
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
                    graphics.FillRectangle(base.Engine.Res.Brushes.SKIN2_FORMCOLOR, 0, 0, width, height);
                    rectangle = new Rectangle(0, 0, width, height);
                    rectangle2 = new Rectangle(0, 0, image.Width, image.Height);
                    graphics.DrawImage(image, rectangle, rectangle2, GraphicsUnit.Pixel);
                }
                IntPtr windowDC = x61467fe65a98f20c.GetWindowDC(base.Handle);
                using (Graphics graphics2 = Graphics.FromHdc(windowDC))
                {
                    rectangle2 = new Rectangle(0, 0, width, height);
                    rectangle = rectangle2;
                    graphics2.DrawImage(bitmap2, rectangle, rectangle2, GraphicsUnit.Pixel);
                }
                x61467fe65a98f20c.ReleaseDC(base.Handle, windowDC);
            }
        }
    }
}

