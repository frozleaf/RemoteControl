namespace Sunisoft.IrisSkin
{
    using System;
    using System.Drawing;
    using System.Text;
    using System.Windows.Forms;

    internal class x5b126f5f998c28e9 : NativeWindow
    {
        protected xb9506a535e31f22a ctrlMouseState = xb9506a535e31f22a.None;
        protected SkinEngine Engine;

        public x5b126f5f998c28e9(IntPtr handle, SkinEngine engine)
        {
            this.Engine = engine;
            base.AssignHandle(handle);
        }

        protected virtual void AfterWndProc(ref Message m)
        {
            switch (((uint) m.Msg))
            {
                case 0x200:
                    if (this.ctrlMouseState != xb9506a535e31f22a.MouseDown)
                    {
                        this.ctrlMouseState = xb9506a535e31f22a.MouseIn;
                    }
                    this.PaintControl();
                    return;

                case 0x201:
                    this.ctrlMouseState = xb9506a535e31f22a.MouseDown;
                    this.PaintControl();
                    return;

                case 0x202:
                    this.ctrlMouseState = xb9506a535e31f22a.None;
                    this.PaintControl();
                    return;

                case 0x2a3:
                    this.ctrlMouseState = xb9506a535e31f22a.None;
                    this.PaintControl();
                    return;

                case 10:
                    this.PaintControl();
                    return;

                case 11:
                    break;

                case 12:
                case 15:
                    x40255b11ef821fa3.PAINTSTRUCT paintstruct;
                    x61467fe65a98f20c.BeginPaint(base.Handle, out paintstruct);
                    this.PaintControl();
                    x61467fe65a98f20c.EndPaint(base.Handle, ref paintstruct);
                    break;

                default:
                    return;
            }
        }

        protected virtual bool BeforeWndProc(ref Message m)
        {
            return true;
        }

        protected virtual void PaintControl()
        {
        }

        protected override void WndProc(ref Message m)
        {
            if (this.CanPaint)
            {
                if (this.BeforeWndProc(ref m))
                {
                    base.WndProc(ref m);
                }
                this.AfterWndProc(ref m);
            }
            else
            {
                base.WndProc(ref m);
            }
        }

        protected virtual bool CanPaint
        {
            get
            {
                return this.Engine.RealActive;
            }
        }

        protected virtual Rectangle ClientRectangle
        {
            get
            {
                Rectangle lpRect = new Rectangle(0, 0, 0, 0);
                x61467fe65a98f20c.GetClientRect(base.Handle, ref lpRect);
                return lpRect;
            }
        }

        protected virtual bool Enabled
        {
            get
            {
                return ((x61467fe65a98f20c.GetWindowLong(base.Handle, -16) & 0x8000000) != 0x8000000);
            }
        }

        protected virtual bool Focused
        {
            get
            {
                return (x61467fe65a98f20c.GetFocus() == base.Handle);
            }
        }

        protected virtual System.Drawing.Font Font
        {
            get
            {
                return x448fd9ab43628c71.GetMessageFont();
            }
        }

        protected virtual Color ForeColor
        {
            get
            {
                return Color.FromKnownColor(KnownColor.ControlText);
            }
        }

        protected virtual int Height
        {
            get
            {
                xae4dd1cafd2eb77c.RECT lpRect = new xae4dd1cafd2eb77c.RECT();
                x61467fe65a98f20c.GetClientRect(base.Handle, ref lpRect);
                return lpRect.bottom;
            }
        }

        protected virtual System.Windows.Forms.RightToLeft RightToLeft
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

        protected virtual string Text
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

        protected virtual int Width
        {
            get
            {
                xae4dd1cafd2eb77c.RECT lpRect = new xae4dd1cafd2eb77c.RECT();
                x61467fe65a98f20c.GetClientRect(base.Handle, ref lpRect);
                return lpRect.right;
            }
        }
    }
}

