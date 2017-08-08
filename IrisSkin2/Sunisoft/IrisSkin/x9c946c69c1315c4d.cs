namespace Sunisoft.IrisSkin
{
    using System;
    using System.Drawing;
    using System.Windows.Forms;

    internal class x9c946c69c1315c4d : x8917d01b98173f4c
    {
        private uint x1f25abf5fb75e795;
        private Rectangle x26545669838eb36e;
        private Point x2aa5114a5da7d6c8;
        private uint x30cc7819189f11b6;
        private bool x34ce871902cf4acd;
        private Timer x420067493d7ebb36;

        public x9c946c69c1315c4d(Control control, SkinEngine engine) : base(control, engine)
        {
            this.x420067493d7ebb36 = new Timer();
            this.x1f25abf5fb75e795 = 0x115;
            this.x26545669838eb36e = new Rectangle(0, 0, 0, 0);
            this.x2aa5114a5da7d6c8 = new Point(0);
        }

        protected override void AfterWndProc(ref Message m)
        {
            if ((base.CanPaint && base.Engine.SkinScrollBar) && this.CanScroll)
            {
                uint msg = (uint) m.Msg;
                if (msg == 15)
                {
                    x40255b11ef821fa3.PAINTSTRUCT paintstruct;
                    x61467fe65a98f20c.BeginPaint(base.Handle, out paintstruct);
                    this.PaintControl();
                    x61467fe65a98f20c.EndPaint(base.Handle, ref paintstruct);
                }
                else if (msg == 0x84)
                {
                    int num = m.Result.ToInt32();
                    switch (num)
                    {
                        case 7:
                            m.Result = new IntPtr(num + 0x100);
                            break;

                        case 6:
                            m.Result = new IntPtr(num + 0x100);
                            break;
                    }
                }
                base.AfterWndProc(ref m);
            }
        }

        protected override bool BeforeWndProc(ref Message m)
        {
            int num;
            x40255b11ef821fa3.SCROLLINFO scrollinfo;
            x40255b11ef821fa3.SCROLLBARINFO scrollbarinfo;
            int nMin;
            int nPos;
            if (!base.CanPaint)
            {
                return base.BeforeWndProc(ref m);
            }
            if (!base.Engine.SkinScrollBar)
            {
                return base.BeforeWndProc(ref m);
            }
            if (!this.CanScroll)
            {
                return base.BeforeWndProc(ref m);
            }
            switch (((uint) m.Msg))
            {
                case 0x200:
                {
                    if (!this.x34ce871902cf4acd)
                    {
                        if (base.Ctrl is TextBox)
                        {
                            this.PaintControl();
                        }
                        goto Label_0B8F;
                    }
                    int num2 = 0;
                    int num3 = 0;
                    nMin = 0;
                    nPos = 0;
                    scrollbarinfo = new x40255b11ef821fa3.SCROLLBARINFO {
                        cbSize = 60
                    };
                    scrollinfo = new x40255b11ef821fa3.SCROLLINFO {
                        cbSize = 0x1c,
                        fMask = 0x1f
                    };
                    new Rectangle(0, 0, 0, 0);
                    if (this.x1f25abf5fb75e795 != 0x115)
                    {
                        if ((this.x1f25abf5fb75e795 == 0x114) && x61467fe65a98f20c.GetScrollBarInfo(base.Handle, 0xfffffffa, ref scrollbarinfo))
                        {
                            num2 = (scrollbarinfo.rcScrollBar.right - scrollbarinfo.rcScrollBar.left) - (scrollbarinfo.dxyLineButton * 2);
                            if (num2 <= 0)
                            {
                                goto Label_0B8F;
                            }
                            x61467fe65a98f20c.GetScrollInfo(base.Handle, 0, ref scrollinfo);
                            num3 = ((scrollinfo.nMax - scrollinfo.nMin) - ((int) scrollinfo.nPage)) + 1;
                            nMin = ((((Control.MousePosition.X - scrollbarinfo.rcScrollBar.left) - scrollbarinfo.dxyLineButton) * num3) / num2) + scrollinfo.nMin;
                            if (nMin < scrollinfo.nMin)
                            {
                                nMin = scrollinfo.nMin;
                            }
                            if (nMin > num3)
                            {
                                nMin = num3;
                            }
                            int num7 = (((this.x2aa5114a5da7d6c8.X - scrollbarinfo.rcScrollBar.left) - scrollbarinfo.dxyLineButton) * num3) / num2;
                            nPos = scrollinfo.nPos;
                            if (this.x2aa5114a5da7d6c8.X > Control.MousePosition.X)
                            {
                                while (scrollinfo.nPos > nMin)
                                {
                                    x61467fe65a98f20c.SendMessage(base.Handle, 0x114, (uint) 0, (uint) 0);
                                    x61467fe65a98f20c.GetScrollInfo(base.Handle, 0, ref scrollinfo);
                                    if (nPos == scrollinfo.nPos)
                                    {
                                        break;
                                    }
                                    nPos = scrollinfo.nPos;
                                }
                            }
                            if (this.x2aa5114a5da7d6c8.X < Control.MousePosition.X)
                            {
                                while (scrollinfo.nPos < nMin)
                                {
                                    x61467fe65a98f20c.SendMessage(base.Handle, 0x114, (uint) 1, (uint) 0);
                                    x61467fe65a98f20c.GetScrollInfo(base.Handle, 0, ref scrollinfo);
                                    if (nPos == scrollinfo.nPos)
                                    {
                                        break;
                                    }
                                    nPos = scrollinfo.nPos;
                                }
                            }
                        }
                        goto Label_0493;
                    }
                    if (!x61467fe65a98f20c.GetScrollBarInfo(base.Handle, 0xfffffffb, ref scrollbarinfo))
                    {
                        goto Label_0493;
                    }
                    num2 = (scrollbarinfo.rcScrollBar.bottom - scrollbarinfo.rcScrollBar.top) - (scrollbarinfo.dxyLineButton * 2);
                    if (num2 <= 0)
                    {
                        goto Label_0B8F;
                    }
                    x61467fe65a98f20c.GetScrollInfo(base.Handle, 1, ref scrollinfo);
                    num3 = ((scrollinfo.nMax - scrollinfo.nMin) - ((int) scrollinfo.nPage)) + 1;
                    nMin = ((((Control.MousePosition.Y - scrollbarinfo.rcScrollBar.top) - scrollbarinfo.dxyLineButton) * num3) / num2) + scrollinfo.nMin;
                    if (nMin < scrollinfo.nMin)
                    {
                        nMin = scrollinfo.nMin;
                    }
                    if (nMin > num3)
                    {
                        nMin = num3;
                    }
                    int num1 = (((this.x2aa5114a5da7d6c8.Y - scrollbarinfo.rcScrollBar.top) - scrollbarinfo.dxyLineButton) * num3) / num2;
                    nPos = scrollinfo.nPos;
                    if (this.x2aa5114a5da7d6c8.Y > Control.MousePosition.Y)
                    {
                        while (scrollinfo.nPos > nMin)
                        {
                            x61467fe65a98f20c.SendMessage(base.Handle, 0x115, (uint) 0, (uint) 0);
                            x61467fe65a98f20c.GetScrollInfo(base.Handle, 1, ref scrollinfo);
                            if (nPos == scrollinfo.nPos)
                            {
                                break;
                            }
                            nPos = scrollinfo.nPos;
                        }
                    }
                    break;
                }
                case 0x201:
                    if (!this.x34ce871902cf4acd)
                    {
                        goto Label_0B8F;
                    }
                    return false;

                case 0x202:
                    if (this.x34ce871902cf4acd)
                    {
                        this.x6139c418c50df34e();
                    }
                    goto Label_0B8F;

                case 0x20a:
                    num = m.WParam.ToInt32() >> 0x10;
                    if (num > 0)
                    {
                        x61467fe65a98f20c.PostMessage(base.Handle, 0x115, (uint) 2, (uint) 0);
                    }
                    else
                    {
                        x61467fe65a98f20c.PostMessage(base.Handle, 0x115, (uint) 3, (uint) 0);
                    }
                    return false;

                case 0x215:
                    if (m.LParam != base.Handle)
                    {
                        this.x6139c418c50df34e();
                    }
                    goto Label_0B8F;

                case 160:
                    return false;

                case 0xa1:
                {
                    Point point;
                    num = m.WParam.ToInt32();
                    if (num > 0x100)
                    {
                        num -= 0x100;
                    }
                    Rectangle r = new Rectangle(0, 0, 0, 0);
                    switch (num)
                    {
                        case 7:
                            x61467fe65a98f20c.SetCapture(base.Handle);
                            point = new Point(0);
                            scrollbarinfo = new x40255b11ef821fa3.SCROLLBARINFO {
                                cbSize = 60
                            };
                            x61467fe65a98f20c.GetCursorPos(ref point);
                            if (x61467fe65a98f20c.GetScrollBarInfo(base.Handle, 0xfffffffb, ref scrollbarinfo))
                            {
                                r = Rectangle.FromLTRB(scrollbarinfo.rcScrollBar.left, scrollbarinfo.rcScrollBar.top, scrollbarinfo.rcScrollBar.right, scrollbarinfo.rcScrollBar.top + scrollbarinfo.dxyLineButton);
                                if (x448fd9ab43628c71.InRect(point, r))
                                {
                                    x61467fe65a98f20c.PostMessage(base.Handle, 0x115, (uint) 0, (uint) 0);
                                    this.x5319e78a3425ad41(0x115, 0, r);
                                    return false;
                                }
                                r = Rectangle.FromLTRB(scrollbarinfo.rcScrollBar.left, scrollbarinfo.rcScrollBar.top + scrollbarinfo.dxyLineButton, scrollbarinfo.rcScrollBar.right, scrollbarinfo.rcScrollBar.top + scrollbarinfo.xyThumbTop);
                                if (x448fd9ab43628c71.InRect(point, r))
                                {
                                    x61467fe65a98f20c.PostMessage(base.Handle, 0x115, (uint) 2, (uint) 0);
                                    this.x5319e78a3425ad41(0x115, 2, r);
                                    return false;
                                }
                                r = Rectangle.FromLTRB(scrollbarinfo.rcScrollBar.left, scrollbarinfo.rcScrollBar.top + scrollbarinfo.xyThumbBottom, scrollbarinfo.rcScrollBar.right, scrollbarinfo.rcScrollBar.bottom - scrollbarinfo.dxyLineButton);
                                if (x448fd9ab43628c71.InRect(point, r))
                                {
                                    x61467fe65a98f20c.PostMessage(base.Handle, 0x115, (uint) 3, (uint) 0);
                                    this.x5319e78a3425ad41(0x115, 3, r);
                                    return false;
                                }
                                r = Rectangle.FromLTRB(scrollbarinfo.rcScrollBar.left, scrollbarinfo.rcScrollBar.bottom - scrollbarinfo.dxyLineButton, scrollbarinfo.rcScrollBar.right, scrollbarinfo.rcScrollBar.bottom);
                                if (x448fd9ab43628c71.InRect(point, r))
                                {
                                    x61467fe65a98f20c.PostMessage(base.Handle, 0x115, (uint) 1, (uint) 0);
                                    this.x5319e78a3425ad41(0x115, 1, r);
                                    return false;
                                }
                                r = Rectangle.FromLTRB(scrollbarinfo.rcScrollBar.left, scrollbarinfo.rcScrollBar.top + scrollbarinfo.xyThumbTop, scrollbarinfo.rcScrollBar.right, scrollbarinfo.rcScrollBar.top + scrollbarinfo.xyThumbBottom);
                                if (x448fd9ab43628c71.InRect(point, r))
                                {
                                    this.x5319e78a3425ad41(0x115, 5, r);
                                    return false;
                                }
                            }
                            break;

                        case 6:
                            x61467fe65a98f20c.SetCapture(base.Handle);
                            scrollbarinfo = new x40255b11ef821fa3.SCROLLBARINFO {
                                cbSize = 60
                            };
                            point = new Point(0);
                            x61467fe65a98f20c.GetCursorPos(ref point);
                            if (x61467fe65a98f20c.GetScrollBarInfo(base.Handle, 0xfffffffa, ref scrollbarinfo))
                            {
                                r = Rectangle.FromLTRB(scrollbarinfo.rcScrollBar.left, scrollbarinfo.rcScrollBar.top, scrollbarinfo.rcScrollBar.left + scrollbarinfo.dxyLineButton, scrollbarinfo.rcScrollBar.bottom);
                                if (x448fd9ab43628c71.InRect(point, r))
                                {
                                    x61467fe65a98f20c.PostMessage(base.Handle, 0x114, (uint) 0, (uint) 0);
                                    this.x5319e78a3425ad41(0x114, 0, r);
                                    return false;
                                }
                                r = Rectangle.FromLTRB(scrollbarinfo.rcScrollBar.left + scrollbarinfo.dxyLineButton, scrollbarinfo.rcScrollBar.top, scrollbarinfo.rcScrollBar.left + scrollbarinfo.xyThumbTop, scrollbarinfo.rcScrollBar.bottom);
                                if (x448fd9ab43628c71.InRect(point, r))
                                {
                                    x61467fe65a98f20c.PostMessage(base.Handle, 0x114, (uint) 2, (uint) 0);
                                    this.x5319e78a3425ad41(0x114, 2, r);
                                    return false;
                                }
                                r = Rectangle.FromLTRB(scrollbarinfo.rcScrollBar.left + scrollbarinfo.xyThumbBottom, scrollbarinfo.rcScrollBar.top, scrollbarinfo.rcScrollBar.right - scrollbarinfo.dxyLineButton, scrollbarinfo.rcScrollBar.bottom);
                                if (x448fd9ab43628c71.InRect(point, r))
                                {
                                    x61467fe65a98f20c.PostMessage(base.Handle, 0x114, (uint) 3, (uint) 0);
                                    this.x5319e78a3425ad41(0x114, 3, r);
                                    return false;
                                }
                                r = Rectangle.FromLTRB(scrollbarinfo.rcScrollBar.right - scrollbarinfo.dxyLineButton, scrollbarinfo.rcScrollBar.top, scrollbarinfo.rcScrollBar.right, scrollbarinfo.rcScrollBar.bottom);
                                if (x448fd9ab43628c71.InRect(point, r))
                                {
                                    x61467fe65a98f20c.PostMessage(base.Handle, 0x114, (uint) 1, (uint) 0);
                                    this.x5319e78a3425ad41(0x114, 1, r);
                                    return false;
                                }
                                r = Rectangle.FromLTRB(scrollbarinfo.rcScrollBar.left + scrollbarinfo.xyThumbTop, scrollbarinfo.rcScrollBar.top, scrollbarinfo.rcScrollBar.left + scrollbarinfo.xyThumbBottom, scrollbarinfo.rcScrollBar.bottom);
                                if (x448fd9ab43628c71.InRect(point, r))
                                {
                                    this.x5319e78a3425ad41(0x114, 5, r);
                                    return false;
                                }
                            }
                            break;
                    }
                    return false;
                }
                case 0xa2:
                    return false;

                case 0x2b:
                    if (m.WParam != IntPtr.Zero)
                    {
                        this.PaintControl();
                    }
                    goto Label_0B8F;

                case 0x114:
                    num = m.WParam.ToInt32();
                    if ((num & 5L) != 5L)
                    {
                        scrollinfo = new x40255b11ef821fa3.SCROLLINFO {
                            cbSize = 0x1c,
                            fMask = 0x1f
                        };
                        x61467fe65a98f20c.GetScrollInfo(base.Handle, 0, ref scrollinfo);
                        switch (num)
                        {
                            case 0:
                            case 2:
                                if (scrollinfo.nPos <= scrollinfo.nMin)
                                {
                                    return false;
                                }
                                goto Label_0B8F;
                        }
                        if (((num == 1L) || (num == 3L)) && (scrollinfo.nPos > ((scrollinfo.nMax - scrollinfo.nPage) - scrollinfo.nMin)))
                        {
                            return false;
                        }
                    }
                    goto Label_0B8F;

                case 0x115:
                    num = m.WParam.ToInt32();
                    if ((num & 5L) == 5L)
                    {
                        goto Label_0B8F;
                    }
                    scrollinfo = new x40255b11ef821fa3.SCROLLINFO {
                        cbSize = 0x1c,
                        fMask = 0x1f
                    };
                    x61467fe65a98f20c.GetScrollInfo(base.Handle, 1, ref scrollinfo);
                    switch (num)
                    {
                        case 0:
                        case 2:
                            if (scrollinfo.nPos <= scrollinfo.nMin)
                            {
                                return false;
                            }
                            goto Label_0B8F;
                    }
                    if (((num != 1L) && (num != 3L)) || (scrollinfo.nPos <= ((scrollinfo.nMax - scrollinfo.nPage) - scrollinfo.nMin)))
                    {
                        goto Label_0B8F;
                    }
                    return false;

                default:
                    goto Label_0B8F;
            }
            if (this.x2aa5114a5da7d6c8.Y < Control.MousePosition.Y)
            {
                while (scrollinfo.nPos < nMin)
                {
                    x61467fe65a98f20c.SendMessage(base.Handle, 0x115, (uint) 1, (uint) 0);
                    x61467fe65a98f20c.GetScrollInfo(base.Handle, 1, ref scrollinfo);
                    if (nPos == scrollinfo.nPos)
                    {
                        break;
                    }
                    nPos = scrollinfo.nPos;
                }
            }
        Label_0493:
            this.x2aa5114a5da7d6c8 = Control.MousePosition;
            return false;
        Label_0B8F:
            return base.BeforeWndProc(ref m);
        }

        protected override void DoInit()
        {
            base.DoInit();
            this.x420067493d7ebb36.Interval = 100;
            this.x420067493d7ebb36.Stop();
            this.x420067493d7ebb36.Tick += new EventHandler(this.x8605d09b86ce01b1);
        }

        protected override void PaintControl()
        {
            if ((((base.Ctrl.ClientRectangle.Width > 0) && (base.Ctrl.ClientRectangle.Height > 0)) && base.CanPaint) && base.Engine.SkinScrollBar)
            {
                Rectangle rectangle;
                base.PaintControl();
                x40255b11ef821fa3.SCROLLBARINFO psbi = new x40255b11ef821fa3.SCROLLBARINFO {
                    cbSize = 60
                };
                IntPtr windowDC = x61467fe65a98f20c.GetWindowDC(base.Handle);
                xae4dd1cafd2eb77c.RECT lpRect = new xae4dd1cafd2eb77c.RECT();
                x61467fe65a98f20c.GetWindowRect(base.Handle, ref lpRect);
                int num = 0;
                if (x61467fe65a98f20c.GetScrollBarInfo(base.Handle, 0xfffffffb, ref psbi))
                {
                    rectangle = new Rectangle(psbi.rcScrollBar.left - lpRect.left, psbi.rcScrollBar.top - lpRect.top, psbi.rcScrollBar.right - psbi.rcScrollBar.left, psbi.rcScrollBar.bottom - psbi.rcScrollBar.top);
                    if (((rectangle.X >= 0) && (rectangle.Y >= 0)) && ((rectangle.Width > 0) && (rectangle.Height > 0)))
                    {
                        using (Bitmap bitmap = new Bitmap(rectangle.Width, ((rectangle.Height + base.Ctrl.Height) - rectangle.Bottom) - 1))
                        {
                            using (Graphics graphics = Graphics.FromImage(bitmap))
                            {
                                graphics.FillRectangle(base.Engine.Res.Brushes.SKIN2_FORMCOLOR, 0, 0, bitmap.Width, bitmap.Height);
                                this.x3a329aff8c7e9df4(graphics, psbi, new Rectangle(0, 0, rectangle.Width, rectangle.Height), num);
                            }
                            using (Graphics graphics2 = Graphics.FromHdc(windowDC))
                            {
                                graphics2.DrawImageUnscaled(bitmap, rectangle.X, rectangle.Y);
                            }
                        }
                    }
                }
                if (x61467fe65a98f20c.GetScrollBarInfo(base.Handle, 0xfffffffa, ref psbi))
                {
                    rectangle = new Rectangle(psbi.rcScrollBar.left - lpRect.left, psbi.rcScrollBar.top - lpRect.top, psbi.rcScrollBar.right - psbi.rcScrollBar.left, psbi.rcScrollBar.bottom - psbi.rcScrollBar.top);
                    if (((rectangle.X >= 0) && (rectangle.Y >= 0)) && ((rectangle.Width > 0) && (rectangle.Height > 0)))
                    {
                        using (Bitmap bitmap2 = new Bitmap(rectangle.Width, rectangle.Height))
                        {
                            using (Graphics graphics3 = Graphics.FromImage(bitmap2))
                            {
                                this.xb16feaabcbd204bd(graphics3, psbi, new Rectangle(0, 0, rectangle.Width, rectangle.Height), num);
                            }
                            using (Graphics graphics4 = Graphics.FromHdc(windowDC))
                            {
                                graphics4.DrawImageUnscaled(bitmap2, rectangle.X, rectangle.Y);
                            }
                        }
                    }
                }
                x61467fe65a98f20c.ReleaseDC(base.Handle, windowDC);
            }
        }

        private void x3a329aff8c7e9df4(Graphics x4b101060f4767186, x40255b11ef821fa3.SCROLLBARINFO x8d3f74e5f925679c, Rectangle x382bb49d8f914534, int xd5d613d57fc5703b)
        {
            Brush backColorBrush = base.Engine.Res.ScrollBarRes.BackColorBrush;
            x4b101060f4767186.FillRectangle(backColorBrush, x382bb49d8f914534);
            int height = Math.Min(0x10, (x382bb49d8f914534.Height - 1) / 2);
            Bitmap image = base.Engine.Res.ScrollBarRes.UpButton[0, xd5d613d57fc5703b];
            Rectangle srcRect = new Rectangle(0, 0, image.Width, image.Height);
            Rectangle destRect = new Rectangle(x382bb49d8f914534.X, x382bb49d8f914534.Y, x382bb49d8f914534.Width, height);
            x4b101060f4767186.DrawImage(image, destRect, srcRect, GraphicsUnit.Pixel);
            image = base.Engine.Res.ScrollBarRes.DownButton[0, xd5d613d57fc5703b];
            srcRect = new Rectangle(0, 0, image.Width, image.Height);
            destRect = new Rectangle(x382bb49d8f914534.X, x382bb49d8f914534.Bottom - height, x382bb49d8f914534.Width, height);
            x4b101060f4767186.DrawImage(image, destRect, srcRect, GraphicsUnit.Pixel);
            destRect = new Rectangle(x382bb49d8f914534.Left, x382bb49d8f914534.Top + x8d3f74e5f925679c.xyThumbTop, x382bb49d8f914534.Width, x8d3f74e5f925679c.xyThumbBottom - x8d3f74e5f925679c.xyThumbTop);
            if (destRect.Height > 8)
            {
                x448fd9ab43628c71.PaintSlider(base.Engine, x4b101060f4767186, destRect, true);
            }
        }

        private void x5319e78a3425ad41(uint x1f25abf5fb75e795, uint x30cc7819189f11b6, Rectangle xb55b340ae3a3e4e0)
        {
            this.x1f25abf5fb75e795 = x1f25abf5fb75e795;
            this.x30cc7819189f11b6 = x30cc7819189f11b6;
            this.x26545669838eb36e = xb55b340ae3a3e4e0;
            if (x30cc7819189f11b6 == 5)
            {
                this.x34ce871902cf4acd = true;
                this.x2aa5114a5da7d6c8 = Control.MousePosition;
            }
            this.x420067493d7ebb36.Start();
        }

        private void x6139c418c50df34e()
        {
            this.x34ce871902cf4acd = false;
            this.x420067493d7ebb36.Stop();
        }

        private void x8605d09b86ce01b1(object xe0292b9ed559da7d, EventArgs xfbf34718e704c6bc)
        {
            x40255b11ef821fa3.SCROLLBARINFO psbi = new x40255b11ef821fa3.SCROLLBARINFO {
                cbSize = 60
            };
            Point mousePosition = Control.MousePosition;
            Rectangle r = new Rectangle(0, 0, 0, 0);
            if (this.x1f25abf5fb75e795 == 0x115)
            {
                if (x61467fe65a98f20c.GetScrollBarInfo(base.Handle, 0xfffffffb, ref psbi))
                {
                    r = Rectangle.FromLTRB(psbi.rcScrollBar.left, psbi.rcScrollBar.top + psbi.xyThumbTop, psbi.rcScrollBar.right, psbi.rcScrollBar.top + psbi.xyThumbBottom);
                }
            }
            else if ((this.x1f25abf5fb75e795 == 0x114) && x61467fe65a98f20c.GetScrollBarInfo(base.Handle, 0xfffffffa, ref psbi))
            {
                r = Rectangle.FromLTRB(psbi.rcScrollBar.left + psbi.xyThumbTop, psbi.rcScrollBar.top, psbi.rcScrollBar.left + psbi.xyThumbBottom, psbi.rcScrollBar.bottom);
            }
            if (x448fd9ab43628c71.InRect(mousePosition, this.x26545669838eb36e) && !x448fd9ab43628c71.InRect(mousePosition, r))
            {
                x61467fe65a98f20c.PostMessage(base.Handle, this.x1f25abf5fb75e795, this.x30cc7819189f11b6, 0);
            }
        }

        private void xb16feaabcbd204bd(Graphics x4b101060f4767186, x40255b11ef821fa3.SCROLLBARINFO x8d3f74e5f925679c, Rectangle x382bb49d8f914534, int xd5d613d57fc5703b)
        {
            Brush backColorBrush = base.Engine.Res.ScrollBarRes.BackColorBrush;
            x4b101060f4767186.FillRectangle(backColorBrush, x382bb49d8f914534);
            base.Ctrl.PointToClient(Control.MousePosition);
            int width = Math.Min(0x10, (x382bb49d8f914534.Width - 1) / 2);
            Bitmap image = base.Engine.Res.ScrollBarRes.UpButton[1, xd5d613d57fc5703b];
            Rectangle srcRect = new Rectangle(0, 0, image.Width, image.Height);
            Rectangle destRect = new Rectangle(x382bb49d8f914534.X, x382bb49d8f914534.Y, width, x382bb49d8f914534.Height);
            x4b101060f4767186.DrawImage(image, destRect, srcRect, GraphicsUnit.Pixel);
            image = base.Engine.Res.ScrollBarRes.DownButton[1, xd5d613d57fc5703b];
            srcRect = new Rectangle(0, 0, image.Width, image.Height);
            destRect = new Rectangle(x382bb49d8f914534.Right - width, x382bb49d8f914534.Y, width, x382bb49d8f914534.Height);
            x4b101060f4767186.DrawImage(image, destRect, srcRect, GraphicsUnit.Pixel);
            destRect = new Rectangle(x382bb49d8f914534.Left + x8d3f74e5f925679c.xyThumbTop, x382bb49d8f914534.Top, x8d3f74e5f925679c.xyThumbBottom - x8d3f74e5f925679c.xyThumbTop, x382bb49d8f914534.Height);
            if (destRect.Width > 8)
            {
                x448fd9ab43628c71.PaintSlider(base.Engine, x4b101060f4767186, destRect, false);
            }
        }

        protected virtual bool CanScroll
        {
            get
            {
                return true;
            }
        }
    }
}

