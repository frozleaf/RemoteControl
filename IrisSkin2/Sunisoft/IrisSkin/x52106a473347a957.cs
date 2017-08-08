namespace Sunisoft.IrisSkin
{
    using System;
    using System.Drawing;
    using System.Windows.Forms;

    internal class x52106a473347a957 : x5b126f5f998c28e9
    {
        private int x01b557925841ae51;
        private Timer x420067493d7ebb36;
        private bool x5e45bc5ff748c78c;

        public x52106a473347a957(IntPtr handle, SkinEngine engine) : base(handle, engine)
        {
            this.x420067493d7ebb36 = new Timer();
            this.x420067493d7ebb36.Interval = 0x8a;
            this.x420067493d7ebb36.Tick += new EventHandler(this.x0f7fdae17d8cfcb5);
            this.x420067493d7ebb36.Start();
        }

        protected override bool BeforeWndProc(ref Message m)
        {
            switch (((uint) m.Msg))
            {
                case 0xf3:
                    this.x01b557925841ae51 = m.WParam.ToInt32();
                    return false;

                case 0x101:
                    if ((this.x01b557925841ae51 == 1) && (m.WParam.ToInt32() == 0x20))
                    {
                        x61467fe65a98f20c.PostMessage(x61467fe65a98f20c.GetParent(base.Handle), 0x111, new IntPtr(x61467fe65a98f20c.GetDlgCtrlID(base.Handle)), base.Handle);
                    }
                    break;

                case 0x202:
                    if (this.x01b557925841ae51 == 1)
                    {
                        x61467fe65a98f20c.PostMessage(x61467fe65a98f20c.GetParent(base.Handle), 0x111, new IntPtr(x61467fe65a98f20c.GetDlgCtrlID(base.Handle)), base.Handle);
                    }
                    break;
            }
            return base.BeforeWndProc(ref m);
        }

        protected override void PaintControl()
        {
            if (this.CanPaint && ((this.Width > 0) && (this.Height > 0)))
            {
                using (Graphics graphics = Graphics.FromHwnd(base.Handle))
                {
                    this.x8bc95f030953f87b(graphics);
                }
            }
        }

        private void x0f7fdae17d8cfcb5(object xe0292b9ed559da7d, EventArgs xfbf34718e704c6bc)
        {
            if (!this.x5e45bc5ff748c78c)
            {
                this.PaintControl();
                this.x5e45bc5ff748c78c = true;
            }
            xb9506a535e31f22a none = xb9506a535e31f22a.None;
            Point lpPoint = new Point(0, 0);
            xae4dd1cafd2eb77c.RECT lpRect = new xae4dd1cafd2eb77c.RECT();
            if (x61467fe65a98f20c.GetCursorPos(ref lpPoint))
            {
                x61467fe65a98f20c.GetWindowRect(base.Handle, ref lpRect);
                if (x448fd9ab43628c71.InRect(lpPoint, new Rectangle(lpRect.left, lpRect.top, lpRect.right - lpRect.left, lpRect.bottom - lpRect.top)))
                {
                    none = xb9506a535e31f22a.MouseIn;
                }
                else
                {
                    none = xb9506a535e31f22a.None;
                }
                if (base.ctrlMouseState != none)
                {
                    base.ctrlMouseState = none;
                    this.PaintControl();
                }
            }
        }

        private void x6b515e4773b2f185(Graphics x4b101060f4767186, Bitmap x5ce6bc2fb6fe458d)
        {
            x4b101060f4767186.DrawImage(x5ce6bc2fb6fe458d, this.ClientRectangle, this.ClientRectangle, GraphicsUnit.Pixel);
        }

        private void x6b515e4773b2f185(Graphics x4b101060f4767186, Bitmap x5ce6bc2fb6fe458d, string x1c2743f354837549, Font x26094932cf7a9139, Rectangle x59dcf7372713ec60)
        {
            Brush gray = null;
            x4b101060f4767186.DrawImage(x5ce6bc2fb6fe458d, this.ClientRectangle, this.ClientRectangle, GraphicsUnit.Pixel);
            if ((x1c2743f354837549 != null) && (x1c2743f354837549 != ""))
            {
                if (!this.Enabled)
                {
                    gray = Brushes.Gray;
                }
                else
                {
                    gray = new SolidBrush(this.ForeColor);
                }
                if (base.ctrlMouseState == xb9506a535e31f22a.MouseDown)
                {
                    x448fd9ab43628c71.OffsetRect(ref x59dcf7372713ec60, 1, 1);
                }
                x4b101060f4767186.DrawString(x1c2743f354837549, x26094932cf7a9139, gray, x59dcf7372713ec60);
            }
            if ((gray != null) && this.Enabled)
            {
                gray.Dispose();
            }
        }

        private void x6f37a3496a324cf4(Graphics x4b101060f4767186, string xb41faee6912a2313)
        {
            Rectangle rectangle;
            StringFormat format;
            Brush gray;
            x448fd9ab43628c71.CalcLayoutRect(ContentAlignment.MiddleCenter, this.ClientRectangle, this.RightToLeft, out rectangle, out format);
            if (this.x01b557925841ae51 == 1)
            {
                x448fd9ab43628c71.OffsetRect(ref rectangle, this.x01b557925841ae51, this.x01b557925841ae51);
            }
            if (this.Enabled)
            {
                gray = base.Engine.Res.Brushes.SKIN2_BUTTONFONTCOLOR;
            }
            else
            {
                gray = Brushes.Gray;
            }
            x4b101060f4767186.DrawString(xb41faee6912a2313, this.Font, gray, rectangle, format);
        }

        private void x8bc95f030953f87b(Graphics x4b101060f4767186)
        {
            int ctrlMouseState = (int) base.ctrlMouseState;
            if (this.x01b557925841ae51 == 1)
            {
                ctrlMouseState = 3;
            }
            if (!this.Enabled)
            {
                ctrlMouseState = 1;
            }
            Bitmap src = base.Engine.Res.SplitBitmaps.SKIN2_BUTTON[ctrlMouseState];
            using (Bitmap bitmap2 = new Bitmap(this.Width, this.Height))
            {
                using (Graphics graphics = Graphics.FromImage(bitmap2))
                {
                    Brush brush = base.Engine.Res.Brushes.SKIN2_FORMCOLOR;
                    graphics.FillRectangle(brush, this.ClientRectangle);
                    x448fd9ab43628c71.SplitDraw(src, graphics, this.ClientRectangle);
                    string text = this.Text;
                    if ((text != null) && (text != ""))
                    {
                        this.x6f37a3496a324cf4(graphics, text);
                    }
                    if (this.Focused)
                    {
                        Rectangle rect = Rectangle.FromLTRB(this.ClientRectangle.Left + 4, this.ClientRectangle.Top + 4, this.ClientRectangle.Right - 5, this.ClientRectangle.Bottom - 5);
                        if (base.ctrlMouseState == xb9506a535e31f22a.MouseDown)
                        {
                            x448fd9ab43628c71.OffsetRect(ref rect, 1, 1);
                        }
                        graphics.DrawRectangle(x448fd9ab43628c71.FocusRectanglePen, rect);
                    }
                    this.x6b515e4773b2f185(x4b101060f4767186, bitmap2);
                }
            }
        }
    }
}

