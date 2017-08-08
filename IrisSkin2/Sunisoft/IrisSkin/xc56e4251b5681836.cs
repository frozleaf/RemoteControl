namespace Sunisoft.IrisSkin
{
    using System;
    using System.Drawing;
    using System.Windows.Forms;

    internal class xc56e4251b5681836 : x8917d01b98173f4c
    {
        private x867b4abdb49f9449 x6092b63e9c2b4ce6;
        private x867b4abdb49f9449 xa1f39a0e58651210;

        public xc56e4251b5681836(Control control, SkinEngine engine) : base(control, engine)
        {
            this.xf149df02f1aa3d56();
            if (control is CheckedListBox)
            {
                this.x641297c2f10f25b0.Visible = false;
            }
            control.VisibleChanged += new EventHandler(this.xbad904bdcc347055);
            control.SizeChanged += new EventHandler(this.xe8a173d72f8f3729);
            control.LocationChanged += new EventHandler(this.x9fca9353a2d2087c);
            control.Resize += new EventHandler(this.xb82f209246987e7a);
            control.EnabledChanged += new EventHandler(this.xc68edf742e725d77);
        }

        protected override void AfterWndProc(ref Message m)
        {
            if ((base.CanPaint && base.Engine.SkinScrollBar) && this.CanScroll)
            {
                if (m.Msg == 15)
                {
                    x40255b11ef821fa3.PAINTSTRUCT paintstruct;
                    x61467fe65a98f20c.BeginPaint(base.Handle, out paintstruct);
                    this.PaintControl();
                    x61467fe65a98f20c.EndPaint(base.Handle, ref paintstruct);
                }
                base.AfterWndProc(ref m);
            }
        }

        protected override bool BeforeWndProc(ref Message m)
        {
            if (base.CanPaint)
            {
                if (!base.Engine.SkinScrollBar)
                {
                    return base.BeforeWndProc(ref m);
                }
                if (!this.CanScroll)
                {
                    return base.BeforeWndProc(ref m);
                }
                uint msg = (uint) m.Msg;
                switch (msg)
                {
                    case 0x114:
                    case 0x115:
                        this.PaintControl();
                        break;

                    default:
                        if ((msg == 0x200) && (base.Ctrl is TextBox))
                        {
                            this.PaintControl();
                        }
                        break;
                }
            }
            return base.BeforeWndProc(ref m);
        }

        protected override void OnCurrentSkinChanged(object sender, SkinChangedEventArgs e)
        {
            base.OnCurrentSkinChanged(sender, e);
            this.xf149df02f1aa3d56();
        }

        protected override void PaintControl()
        {
            base.PaintControl();
            if ((((base.Ctrl.ClientRectangle.Width > 0) && (base.Ctrl.ClientRectangle.Height > 0)) && base.CanPaint) && base.Engine.SkinScrollBar)
            {
                Rectangle rectangle;
                x40255b11ef821fa3.SCROLLBARINFO psbi = new x40255b11ef821fa3.SCROLLBARINFO {
                    cbSize = 60
                };
                IntPtr windowDC = x61467fe65a98f20c.GetWindowDC(base.Handle);
                xae4dd1cafd2eb77c.RECT lpRect = new xae4dd1cafd2eb77c.RECT();
                x61467fe65a98f20c.GetWindowRect(base.Handle, ref lpRect);
                int left = 0;
                if (x61467fe65a98f20c.GetScrollBarInfo(base.Handle, 0xfffffffb, ref psbi))
                {
                    rectangle = new Rectangle(psbi.rcScrollBar.left - lpRect.left, psbi.rcScrollBar.top - lpRect.top, psbi.rcScrollBar.right - psbi.rcScrollBar.left, psbi.rcScrollBar.bottom - psbi.rcScrollBar.top);
                    if (((rectangle.X >= 0) && (rectangle.Y >= 0)) && ((rectangle.Width > 0) && (rectangle.Height > 0)))
                    {
                        rectangle.X += base.Ctrl.Left;
                        rectangle.Y += base.Ctrl.Top;
                        if ((base.Ctrl.RightToLeft & RightToLeft.Yes) == RightToLeft.Yes)
                        {
                            left = rectangle.Left;
                        }
                        else
                        {
                            left = rectangle.Right;
                        }
                        if (base.Ctrl.Parent is TableLayoutPanel)
                        {
                            rectangle.X += base.Ctrl.Parent.Left;
                            rectangle.Y += base.Ctrl.Parent.Top;
                        }
                        if (this.x641297c2f10f25b0.Bounds != rectangle)
                        {
                            this.x641297c2f10f25b0.Bounds = rectangle;
                        }
                        this.x641297c2f10f25b0.Repaint(psbi);
                    }
                    else if (this.xa1f39a0e58651210 != null)
                    {
                        this.x641297c2f10f25b0.Visible = false;
                    }
                }
                else if (this.xa1f39a0e58651210 != null)
                {
                    this.x641297c2f10f25b0.Visible = false;
                }
                if (x61467fe65a98f20c.GetScrollBarInfo(base.Handle, 0xfffffffa, ref psbi))
                {
                    rectangle = new Rectangle(psbi.rcScrollBar.left - lpRect.left, psbi.rcScrollBar.top - lpRect.top, psbi.rcScrollBar.right - psbi.rcScrollBar.left, psbi.rcScrollBar.bottom - psbi.rcScrollBar.top);
                    if (((rectangle.X >= 0) && (rectangle.Y >= 0)) && ((rectangle.Width > 0) && (rectangle.Height > 0)))
                    {
                        rectangle.X += base.Ctrl.Left;
                        rectangle.Y += base.Ctrl.Top;
                        if (left != 0)
                        {
                            if ((base.Ctrl.RightToLeft & RightToLeft.Yes) == RightToLeft.Yes)
                            {
                                rectangle = Rectangle.FromLTRB(left, rectangle.Top, rectangle.Right, rectangle.Bottom);
                            }
                            else
                            {
                                rectangle = Rectangle.FromLTRB(rectangle.Left, rectangle.Top, left, rectangle.Bottom);
                            }
                        }
                        if (base.Ctrl.Parent is TableLayoutPanel)
                        {
                            rectangle.X += base.Ctrl.Parent.Left;
                            rectangle.Y += base.Ctrl.Parent.Top;
                        }
                        if (this.x17ac55c3881d8810.Bounds != rectangle)
                        {
                            this.x17ac55c3881d8810.Bounds = rectangle;
                        }
                        this.x17ac55c3881d8810.Repaint(psbi);
                    }
                    else if (this.x6092b63e9c2b4ce6 != null)
                    {
                        this.x17ac55c3881d8810.Visible = false;
                    }
                }
                else if (this.x6092b63e9c2b4ce6 != null)
                {
                    this.x17ac55c3881d8810.Visible = false;
                }
                x61467fe65a98f20c.ReleaseDC(base.Handle, windowDC);
            }
        }

        private void x9fca9353a2d2087c(object xe0292b9ed559da7d, EventArgs xfbf34718e704c6bc)
        {
            this.xf149df02f1aa3d56();
        }

        private void xb82f209246987e7a(object xe0292b9ed559da7d, EventArgs xfbf34718e704c6bc)
        {
            this.xf149df02f1aa3d56();
        }

        private void xbad904bdcc347055(object xe0292b9ed559da7d, EventArgs xfbf34718e704c6bc)
        {
            this.xf149df02f1aa3d56();
        }

        private void xc68edf742e725d77(object xe0292b9ed559da7d, EventArgs xfbf34718e704c6bc)
        {
            this.PaintControl();
        }

        private void xdf8f78b46a8ed400(object xe0292b9ed559da7d, EventArgs xfbf34718e704c6bc)
        {
            this.PaintControl();
        }

        private void xe8a173d72f8f3729(object xe0292b9ed559da7d, EventArgs xfbf34718e704c6bc)
        {
            this.xf149df02f1aa3d56();
            this.PaintControl();
        }

        private void xf149df02f1aa3d56()
        {
            if (this.x6092b63e9c2b4ce6 != null)
            {
                this.x17ac55c3881d8810.Visible = ((base.CanPaint & base.Ctrl.Visible) & base.Engine.SkinScrollBar) & this.CanScroll;
            }
            if (this.xa1f39a0e58651210 != null)
            {
                this.x641297c2f10f25b0.Visible = ((base.CanPaint & base.Ctrl.Visible) & base.Engine.SkinScrollBar) & this.CanScroll;
            }
        }

        protected virtual bool CanScroll
        {
            get
            {
                return true;
            }
        }

        private x867b4abdb49f9449 x17ac55c3881d8810
        {
            get
            {
                if (this.x6092b63e9c2b4ce6 == null)
                {
                    this.x6092b63e9c2b4ce6 = new x867b4abdb49f9449(base.Ctrl, base.Engine, true);
                }
                return this.x6092b63e9c2b4ce6;
            }
        }

        private x867b4abdb49f9449 x641297c2f10f25b0
        {
            get
            {
                if (this.xa1f39a0e58651210 == null)
                {
                    this.xa1f39a0e58651210 = new x867b4abdb49f9449(base.Ctrl, base.Engine, false);
                }
                return this.xa1f39a0e58651210;
            }
        }
    }
}

