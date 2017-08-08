namespace Sunisoft.IrisSkin
{
    using System;
    using System.Drawing;
    using System.Text;
    using System.Windows.Forms;

    internal class xb052c904ac95dc43 : xd24df615efe9450e
    {
        private bool x66bf3eedd0ff9957;
        private bool xa8a1d8725b684fc0;
        private bool xc670990170293979;
        private bool xdfdb4d7af566dc92;

        public xb052c904ac95dc43(IntPtr handle, SkinEngine engine) : base(handle, engine)
        {
            this.x66bf3eedd0ff9957 = true;
            this.xc670990170293979 = true;
            int windowTextLengthA = x61467fe65a98f20c.GetWindowTextLengthA(base.Handle);
            if (windowTextLengthA == 8)
            {
                StringBuilder lpString = new StringBuilder(windowTextLengthA, windowTextLengthA + 1);
                x61467fe65a98f20c.GetWindowText(base.Handle, lpString, windowTextLengthA);
                if (lpString.ToString(0, 4) == "打印预览")
                {
                    this.xc670990170293979 = false;
                }
            }
        }

        protected override void AfterWndProc(ref Message m)
        {
            base.AfterWndProc(ref m);
            uint msg = (uint) m.Msg;
            if (msg == 5)
            {
                xae4dd1cafd2eb77c.RECT lpRect = new xae4dd1cafd2eb77c.RECT();
                if (((x61467fe65a98f20c.GetWindowRect(base.Handle, ref lpRect) && !this.xdfdb4d7af566dc92) && (!this.x66bf3eedd0ff9957 && !base.Initializing)) && (this.BorderStyle == FormBorderStyle.FixedDialog))
                {
                    this.xdfdb4d7af566dc92 = true;
                    this.ApplySkin();
                    x61467fe65a98f20c.MoveWindow(base.Handle, lpRect.left, lpRect.top, lpRect.right - lpRect.left, (((((lpRect.bottom - lpRect.top) + this.TitleHeight) + this.MenuHeight) - SystemInformation.CaptionHeight) + base.Engine.BottomBorderHeight) - 4, true);
                }
                this.RegionWindow();
            }
            else if ((msg == 15) && !this.xa8a1d8725b684fc0)
            {
                this.RegionWindow();
            }
        }

        protected override void ApplySkin()
        {
            if (this.CanPaint)
            {
                int cx = 0;
                int cy = 0;
                int num3 = 4;
                int num4 = 4;
                int num5 = 4;
                num3 = Math.Max(base.Engine.LeftBorderWidth, base.cxBorder);
                num4 = Math.Max(base.Engine.RightBorderWidth, base.cxBorder);
                num5 = Math.Max(base.Engine.BottomBorderHeight, base.cyBorder);
                cy = ((this.ClientRectangle.Height + base.Engine.TitleHeight) + this.MenuHeight) + num5;
                cx = (this.ClientRectangle.Width + num3) + num4;
                this.x66bf3eedd0ff9957 = true;
                x61467fe65a98f20c.SetWindowPos(base.Handle, IntPtr.Zero, 0, 0, cx, cy, 0x16);
                this.x66bf3eedd0ff9957 = false;
            }
        }

        protected override bool BeforeWndProc(ref Message m)
        {
            switch (((uint) m.Msg))
            {
                case 5:
                    if (!this.xc670990170293979)
                    {
                        break;
                    }
                    return false;

                case 0x135:
                case 0x138:
                    m.Result = base.Engine.BackColorBrush;
                    x31775329b2a4ff52.SetBkMode(m.WParam, 1);
                    return false;
            }
            return base.BeforeWndProc(ref m);
        }

        protected override void DoSysMenu()
        {
        }

        protected override void DoSysMenu(Point p)
        {
        }

        protected override Point PointToScreen(Point point)
        {
            x61467fe65a98f20c.ClientToScreen(base.Handle, ref point);
            return point;
        }

        protected override void RegionWindow()
        {
            base.RegionWindow();
            this.xa8a1d8725b684fc0 = true;
        }

        protected override void SkinControls()
        {
            IntPtr topWindow = x61467fe65a98f20c.GetTopWindow(base.Handle);
            ulong windowLong = 0L;
            while (topWindow != IntPtr.Zero)
            {
                x5b126f5f998c28e9 xbffce;
                if (base.ControlTable.ContainsKey(topWindow))
                {
                    topWindow = x61467fe65a98f20c.GetWindow(topWindow, 2);
                    continue;
                }
                StringBuilder lpClassName = new StringBuilder(260);
                windowLong = x61467fe65a98f20c.GetWindowLong(topWindow, -16);
                x61467fe65a98f20c.GetClassName(topWindow, lpClassName, 260);
                if ((windowLong & ((ulong) 0x40000000L)) != 0x40000000L)
                {
                    topWindow = x61467fe65a98f20c.GetWindow(topWindow, 2);
                    continue;
                }
                string name = lpClassName.ToString().ToUpper();
                switch (name)
                {
                    case "COMBOBOX":
                    {
                        if ((windowLong & ((ulong) 3L)) != 1L)
                        {
                            break;
                        }
                        topWindow = x61467fe65a98f20c.GetWindow(topWindow, 2);
                        continue;
                    }
                    case "COMBOBOXEX32":
                    {
                        xbffce = x735ffd864c9f9835.Create(x61467fe65a98f20c.GetTopWindow(topWindow), name, base.Engine);
                        base.ControlTable.Add(topWindow, xbffce);
                        continue;
                    }
                    case "BUTTON":
                        if ((windowLong & ((ulong) 7L)) == 7L)
                        {
                            topWindow = x61467fe65a98f20c.GetWindow(topWindow, 2);
                            continue;
                        }
                        if ((((windowLong & ((ulong) 3L)) == 3L) || ((windowLong & ((ulong) 2L)) == 2L)) || ((windowLong & ((ulong) 5L)) == 5L))
                        {
                            name = name + "_CHECKBOX";
                        }
                        else if (((windowLong & ((ulong) 9L)) == 9L) || ((windowLong & ((ulong) 4L)) == 4L))
                        {
                            name = name + "_RADIOBUTTON";
                        }
                        break;
                }
                xbffce = x735ffd864c9f9835.Create(topWindow, name, base.Engine);
                base.ControlTable.Add(topWindow, xbffce);
            }
        }

        protected override FormBorderStyle BorderStyle
        {
            get
            {
                if ((x61467fe65a98f20c.GetWindowLong(base.Handle, -16) & 0x40000) == 0x40000)
                {
                    return FormBorderStyle.Sizable;
                }
                return FormBorderStyle.FixedDialog;
            }
        }

        protected override Rectangle ClientRectangle
        {
            get
            {
                Rectangle lpRect = new Rectangle(0, 0, 0, 0);
                x61467fe65a98f20c.GetClientRect(base.Handle, ref lpRect);
                return lpRect;
            }
        }

        protected override bool ControlBox
        {
            get
            {
                return true;
            }
        }

        protected override int Height
        {
            get
            {
                xae4dd1cafd2eb77c.RECT lpRect = new xae4dd1cafd2eb77c.RECT();
                x61467fe65a98f20c.GetWindowRect(base.Handle, ref lpRect);
                return (lpRect.bottom - lpRect.top);
            }
        }

        protected override bool HelpButton
        {
            get
            {
                return false;
            }
        }

        protected override System.Drawing.Icon Icon
        {
            get
            {
                return null;
            }
        }

        protected override bool IsDisposed
        {
            get
            {
                return false;
            }
        }

        protected override bool IsMdiChild
        {
            get
            {
                return false;
            }
        }

        protected override bool IsMdiContainer
        {
            get
            {
                return false;
            }
        }

        protected override bool MaximizeBox
        {
            get
            {
                ulong windowLong = x61467fe65a98f20c.GetWindowLong(base.Handle, -16);
                return ((windowLong & ((ulong) 0x10000L)) == 0x10000L);
            }
        }

        protected override bool MinimizeBox
        {
            get
            {
                ulong windowLong = x61467fe65a98f20c.GetWindowLong(base.Handle, -16);
                return ((windowLong & ((ulong) 0x20000L)) == 0x20000L);
            }
        }

        protected override System.Drawing.Region Region
        {
            get
            {
                return null;
            }
            set
            {
                IntPtr windowDC = x61467fe65a98f20c.GetWindowDC(base.Handle);
                using (Graphics graphics = Graphics.FromHdc(windowDC))
                {
                    if (value != null)
                    {
                        IntPtr hrgn = value.GetHrgn(graphics);
                        x61467fe65a98f20c.SetWindowRgn(base.Handle, hrgn, true);
                        x31775329b2a4ff52.DeleteObject(hrgn);
                    }
                    else
                    {
                        x61467fe65a98f20c.SetWindowRgn(base.Handle, IntPtr.Zero, true);
                    }
                }
                x61467fe65a98f20c.ReleaseDC(base.Handle, windowDC);
            }
        }

        protected override System.Windows.Forms.RightToLeft RightToLeft
        {
            get
            {
                if ((x61467fe65a98f20c.GetWindowLong(base.Handle, -20) & 0x1000) != 0x1000)
                {
                    return System.Windows.Forms.RightToLeft.No;
                }
                return System.Windows.Forms.RightToLeft.Yes;
            }
        }

        protected override bool RightToLeftLayout
        {
            get
            {
                return false;
            }
        }

        protected override object Tag
        {
            get
            {
                return new object();
            }
        }

        protected override string Text
        {
            get
            {
                int windowTextLengthA = x61467fe65a98f20c.GetWindowTextLengthA(base.Handle);
                if (windowTextLengthA > 0)
                {
                    windowTextLengthA++;
                    StringBuilder lpString = new StringBuilder(windowTextLengthA);
                    x61467fe65a98f20c.GetWindowText(base.Handle, lpString, windowTextLengthA);
                    if (lpString != null)
                    {
                        return lpString.ToString();
                    }
                }
                return "";
            }
        }

        protected override int Width
        {
            get
            {
                xae4dd1cafd2eb77c.RECT lpRect = new xae4dd1cafd2eb77c.RECT();
                x61467fe65a98f20c.GetWindowRect(base.Handle, ref lpRect);
                return (lpRect.right - lpRect.left);
            }
        }
    }
}

