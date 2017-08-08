namespace Sunisoft.IrisSkin
{
    using System;
    using System.Drawing;
    using System.Windows.Forms;

    internal class x867b4abdb49f9449 : Control
    {
        private Control x246c0c54671f3f3e;
        private bool x71ee84dfc52c8c16;
        private SkinEngine xdc87e2b99332cd4a;

        public x867b4abdb49f9449(Control host, SkinEngine engine, bool hScroll)
        {
            this.x246c0c54671f3f3e = host;
            this.xdc87e2b99332cd4a = engine;
            this.x71ee84dfc52c8c16 = hScroll;
            if (host.Parent != null)
            {
                if ((host.Parent is TableLayoutPanel) || (host.Parent is SplitContainer))
                {
                    if (host.Parent.Parent != null)
                    {
                        host.Parent.Parent.Controls.Add(this);
                    }
                }
                else
                {
                    host.Parent.Controls.Add(this);
                }
            }
            base.Bounds = host.Bounds;
            base.BringToFront();
        }

        public void Repaint(x40255b11ef821fa3.SCROLLBARINFO info)
        {
            if ((base.Width > 0) && (base.Height > 0))
            {
                using (Bitmap bitmap = new Bitmap(base.Width, base.Height))
                {
                    int num2 = 0;
                    using (Graphics graphics = Graphics.FromImage(bitmap))
                    {
                        Bitmap bitmap2;
                        Rectangle rectangle2;
                        int num;
                        Brush backColorBrush = this.xdc87e2b99332cd4a.Res.ScrollBarRes.BackColorBrush;
                        Rectangle rect = new Rectangle(0, 0, base.Width, base.Height);
                        graphics.FillRectangle(backColorBrush, rect);
                        Rectangle rectangle3 = new Rectangle(0, 0, base.Width, base.Height);
                        if (this.x71ee84dfc52c8c16)
                        {
                            rectangle3.Width = info.rcScrollBar.right - info.rcScrollBar.left;
                            if ((this.x246c0c54671f3f3e.RightToLeft & RightToLeft.Yes) == RightToLeft.Yes)
                            {
                                rectangle3.X += base.PointToClient(new Point(info.rcScrollBar.left, 0)).X;
                            }
                            num = Math.Min(0x10, rectangle3.Width / 2);
                            bitmap2 = this.xdc87e2b99332cd4a.Res.ScrollBarRes.UpButton[1, num2];
                            rectangle2 = new Rectangle(0, 0, bitmap2.Width, bitmap2.Height);
                            rect = new Rectangle(rectangle3.X, rectangle3.Y, num, rectangle3.Height);
                            graphics.DrawImage(bitmap2, rect, rectangle2, GraphicsUnit.Pixel);
                            bitmap2 = this.xdc87e2b99332cd4a.Res.ScrollBarRes.DownButton[1, num2];
                            rectangle2 = new Rectangle(0, 0, bitmap2.Width, bitmap2.Height);
                            rect = new Rectangle(rectangle3.Right - num, rectangle3.Y, num, rectangle3.Height);
                            graphics.DrawImage(bitmap2, rect, rectangle2, GraphicsUnit.Pixel);
                            if ((((info.rgstate[0] & 0x8000) != 0x8000) && ((info.rgstate[0] & 1) != 1)) && ((info.rcScrollBar.right - info.rcScrollBar.left) > 0x26))
                            {
                                rect = new Rectangle(info.xyThumbTop + rectangle3.Left, rectangle3.Top, info.xyThumbBottom - info.xyThumbTop, rectangle3.Bottom);
                                x448fd9ab43628c71.PaintSlider(this.xdc87e2b99332cd4a, graphics, rect, false);
                            }
                        }
                        else
                        {
                            num = Math.Min(0x10, base.Height / 2);
                            bitmap2 = this.xdc87e2b99332cd4a.Res.ScrollBarRes.UpButton[0, num2];
                            rectangle2 = new Rectangle(0, 0, bitmap2.Width, bitmap2.Height);
                            rect = new Rectangle(rectangle3.X, rectangle3.Y, rectangle3.Width, num);
                            graphics.DrawImage(bitmap2, rect, rectangle2, GraphicsUnit.Pixel);
                            bitmap2 = this.xdc87e2b99332cd4a.Res.ScrollBarRes.DownButton[0, num2];
                            rectangle2 = new Rectangle(0, 0, bitmap2.Width, bitmap2.Height);
                            rect = new Rectangle(rectangle3.X, rectangle3.Bottom - num, rectangle3.Width, num);
                            graphics.DrawImage(bitmap2, rect, rectangle2, GraphicsUnit.Pixel);
                            if ((((info.rgstate[0] & 0x8000) != 0x8000) && ((info.rgstate[0] & 1) != 1)) && ((info.rcScrollBar.bottom - info.rcScrollBar.top) > 0x26))
                            {
                                rect = new Rectangle(rectangle3.Left, info.xyThumbTop, rectangle3.Right, info.xyThumbBottom - info.xyThumbTop);
                                x448fd9ab43628c71.PaintSlider(this.xdc87e2b99332cd4a, graphics, rect, true);
                            }
                        }
                    }
                    using (Graphics graphics2 = Graphics.FromHwnd(base.Handle))
                    {
                        graphics2.DrawImageUnscaled(bitmap, 0, 0);
                    }
                }
            }
        }

        protected override void WndProc(ref Message m)
        {
            if (this.x246c0c54671f3f3e != null)
            {
                int messagePos;
                uint num2;
                switch (((uint) m.Msg))
                {
                    case 0x201:
                        messagePos = x61467fe65a98f20c.GetMessagePos();
                        if (!this.x71ee84dfc52c8c16)
                        {
                            num2 = 7;
                        }
                        else
                        {
                            num2 = 6;
                        }
                        x61467fe65a98f20c.ReleaseCapture();
                        x61467fe65a98f20c.SendMessage(this.x246c0c54671f3f3e.Handle, 0xa1, new IntPtr((long) num2), new IntPtr(messagePos));
                        return;

                    case 0x202:
                        messagePos = x61467fe65a98f20c.GetMessagePos();
                        if (!this.x71ee84dfc52c8c16)
                        {
                            num2 = 7;
                        }
                        else
                        {
                            num2 = 6;
                        }
                        x61467fe65a98f20c.ReleaseCapture();
                        x61467fe65a98f20c.SendMessage(this.x246c0c54671f3f3e.Handle, 0xa2, new IntPtr((long) num2), new IntPtr(messagePos));
                        return;

                    case 0x20a:
                        x61467fe65a98f20c.PostMessage(this.x246c0c54671f3f3e.Handle, (uint) m.Msg, m.WParam, m.LParam);
                        return;
                }
            }
            base.WndProc(ref m);
            switch (((uint) m.Msg))
            {
                case 15:
                case 20:
                    x40255b11ef821fa3.PAINTSTRUCT paintstruct;
                    x61467fe65a98f20c.BeginPaint(base.Handle, out paintstruct);
                    this.x8bc95f030953f87b();
                    x61467fe65a98f20c.EndPaint(base.Handle, ref paintstruct);
                    break;
            }
        }

        protected void x8bc95f030953f87b()
        {
            Rectangle rectangle;
            x40255b11ef821fa3.SCROLLBARINFO psbi = new x40255b11ef821fa3.SCROLLBARINFO {
                cbSize = 60
            };
            xae4dd1cafd2eb77c.RECT lpRect = new xae4dd1cafd2eb77c.RECT();
            x61467fe65a98f20c.GetWindowRect(this.x246c0c54671f3f3e.Handle, ref lpRect);
            if (this.x71ee84dfc52c8c16)
            {
                if (x61467fe65a98f20c.GetScrollBarInfo(this.x246c0c54671f3f3e.Handle, 0xfffffffa, ref psbi))
                {
                    rectangle = new Rectangle(psbi.rcScrollBar.left - lpRect.left, psbi.rcScrollBar.top - lpRect.top, psbi.rcScrollBar.right - psbi.rcScrollBar.left, psbi.rcScrollBar.bottom - psbi.rcScrollBar.top);
                    if (((rectangle.X >= 0) && (rectangle.Y >= 0)) && ((rectangle.Width > 0) && (rectangle.Height > 0)))
                    {
                        this.Repaint(psbi);
                    }
                }
            }
            else if (x61467fe65a98f20c.GetScrollBarInfo(base.Handle, 0xfffffffb, ref psbi))
            {
                rectangle = new Rectangle(psbi.rcScrollBar.left - lpRect.left, psbi.rcScrollBar.top - lpRect.top, psbi.rcScrollBar.right - psbi.rcScrollBar.left, psbi.rcScrollBar.bottom - psbi.rcScrollBar.top);
                if (((rectangle.X >= 0) && (rectangle.Y >= 0)) && ((rectangle.Width > 0) && (rectangle.Height > 0)))
                {
                    this.Repaint(psbi);
                }
            }
        }
    }
}

