namespace Sunisoft.IrisSkin
{
    using System;
    using System.Drawing;
    using System.Windows.Forms;

    internal class x1d3c48e32d645589 : x2edc3f693fe78d2e
    {
        private xa7ba4c69fc1cad79 x6b0ad9f73c48ad53;
        private BorderStyle xacfbd7a08ba56c78;

        public x1d3c48e32d645589(Control control, SkinEngine engine) : base(control, engine)
        {
            this.x6b0ad9f73c48ad53 = new xa7ba4c69fc1cad79(engine, control);
        }

        protected override void DoInit()
        {
            base.DoInit();
            base.Engine.CurrentSkinChanged += new SkinChanged(this.x11d4d6bec891e7ac);
            ListView ctrl = (ListView) base.Ctrl;
            this.xacfbd7a08ba56c78 = ctrl.BorderStyle;
            if ((this.xacfbd7a08ba56c78 != BorderStyle.None) && base.CanPaint)
            {
                ctrl.BorderStyle = BorderStyle.FixedSingle;
            }
        }

        protected override void PaintControl()
        {
            base.PaintControl();
        }

        private void x11d4d6bec891e7ac(object xe0292b9ed559da7d, SkinChangedEventArgs xfbf34718e704c6bc)
        {
            if (!base.Ctrl.IsDisposed)
            {
                ListView ctrl = (ListView) base.Ctrl;
                if (this.xacfbd7a08ba56c78 != BorderStyle.None)
                {
                    if (base.CanPaint)
                    {
                        ctrl.BorderStyle = BorderStyle.FixedSingle;
                    }
                    else
                    {
                        ctrl.BorderStyle = this.xacfbd7a08ba56c78;
                    }
                }
            }
        }

        protected override int BorderWidth
        {
            get
            {
                return 0;
            }
        }

        protected override bool ChangeBackColor
        {
            get
            {
                return false;
            }
        }

        protected override bool PaintBorder
        {
            get
            {
                ListView ctrl = (ListView) base.Ctrl;
                if (ctrl.BorderStyle == BorderStyle.None)
                {
                    return false;
                }
                return true;
            }
        }

        internal class xa7ba4c69fc1cad79 : NativeWindow
        {
            private Control x246c0c54671f3f3e;
            private StringFormat x5786461d089b10a0;
            private SkinEngine xdc87e2b99332cd4a;

            public xa7ba4c69fc1cad79(SkinEngine engine, Control host)
            {
                this.xdc87e2b99332cd4a = engine;
                this.x246c0c54671f3f3e = host;
                this.x5786461d089b10a0 = new StringFormat();
                this.x5786461d089b10a0.LineAlignment = StringAlignment.Center;
                this.x5786461d089b10a0.FormatFlags = StringFormatFlags.NoWrap;
                if (this.x246c0c54671f3f3e != null)
                {
                    IntPtr handle = x61467fe65a98f20c.SendMessage(host.Handle, 0x101f, (uint) 0, (uint) 0);
                    if (handle != IntPtr.Zero)
                    {
                        base.AssignHandle(handle);
                    }
                }
            }

            protected virtual unsafe void PaintHeader()
            {
                Rectangle lpRect = new Rectangle(0, 0, 0, 0);
                x61467fe65a98f20c.GetWindowRect(base.Handle, ref lpRect);
                Point p = new Point(lpRect.X, lpRect.Y);
                p = this.x246c0c54671f3f3e.PointToClient(p);
                lpRect.X = p.X;
                lpRect.Y = p.Y;
                p = new Point(lpRect.Width, lpRect.Height);
                p = this.x246c0c54671f3f3e.PointToClient(p);
                lpRect.Width = p.X - lpRect.X;
                lpRect.Height = p.Y - lpRect.Y;
                ListView view = (ListView) this.x246c0c54671f3f3e;
                IntPtr windowDC = x61467fe65a98f20c.GetWindowDC(base.Handle);
                using (Graphics graphics = Graphics.FromHdc(windowDC))
                {
                    int num2 = 0;
                    Font messageFont = x448fd9ab43628c71.GetMessageFont();
                    for (int i = 0; i < view.Columns.Count; i++)
                    {
                        IntPtr wParam = new IntPtr(i);
                        Rectangle rect = new Rectangle(0, 0, 0, 0);
                        Rectangle* rectanglePtr = &rect;
                        IntPtr lParam = new IntPtr((void*) rectanglePtr);
                        x61467fe65a98f20c.SendMessage(base.Handle, 0x1207, wParam, lParam);
                        if (lParam != IntPtr.Zero)
                        {
                            int num3;
                            int num4;
                            rectanglePtr = (Rectangle*) lParam;
                            rect.Width -= rect.X;
                            rect.Height -= rect.Y;
                            graphics.FillRectangle(this.xdc87e2b99332cd4a.Res.Brushes.SKIN2_FORMCOLOR, rect);
                            ColumnHeader header = view.Columns[i];
                            switch (header.TextAlign)
                            {
                                case HorizontalAlignment.Right:
                                    num3 = (rect.Right - rect.Left) - 6;
                                    num4 = (num3 - ((int) graphics.MeasureString(header.Text, messageFont).Width)) - 2;
                                    if (num4 < 4)
                                    {
                                        num4 = 4;
                                    }
                                    num4 = rect.Left + num4;
                                    this.x5786461d089b10a0.Alignment = StringAlignment.Near;
                                    break;

                                case HorizontalAlignment.Center:
                                    num4 = rect.Left + 4;
                                    this.x5786461d089b10a0.Alignment = StringAlignment.Center;
                                    break;

                                default:
                                    num4 = rect.Left + 4;
                                    this.x5786461d089b10a0.Alignment = StringAlignment.Near;
                                    break;
                            }
                            num3 = (rect.Right - rect.Left) - 2;
                            string s = x448fd9ab43628c71.FormatStringWithWidth(graphics, header.Text, messageFont, num3);
                            graphics.DrawString(s, messageFont, this.xdc87e2b99332cd4a.Res.Brushes.SKIN2_CONTROLFONTCOLOR, Rectangle.FromLTRB(num4, rect.Top, rect.Right - 2, rect.Bottom), this.x5786461d089b10a0);
                            using (Pen pen = new Pen(this.xdc87e2b99332cd4a.Res.Brushes.SKIN2_CONTROLBORDERCOLOR))
                            {
                                if (rect.Right == (num2 + 1))
                                {
                                    graphics.DrawLine(pen, rect.Right - 2, rect.Top, rect.Right - 2, rect.Left);
                                }
                                num2 = rect.Right - 1;
                                graphics.DrawLine(pen, num2, rect.Top, num2, rect.Bottom);
                            }
                        }
                        if (rect.Right < lpRect.Right)
                        {
                            graphics.FillRectangle(this.xdc87e2b99332cd4a.Res.Brushes.SKIN2_FORMCOLOR, Rectangle.FromLTRB(rect.Right, rect.Top, lpRect.Right, rect.Bottom));
                        }
                        using (Pen pen2 = new Pen(this.xdc87e2b99332cd4a.Res.Brushes.SKIN2_CONTROLBORDERCOLOR))
                        {
                            graphics.DrawLine(pen2, lpRect.Left, lpRect.Bottom - 1, rect.Right, lpRect.Bottom - 1);
                        }
                    }
                }
                x61467fe65a98f20c.ReleaseDC(this.x246c0c54671f3f3e.Handle, windowDC);
            }

            protected override void WndProc(ref Message m)
            {
                if (this.xdc87e2b99332cd4a.RealActive && (m.Msg == 15))
                {
                    x40255b11ef821fa3.PAINTSTRUCT paintstruct;
                    x61467fe65a98f20c.BeginPaint(base.Handle, out paintstruct);
                    this.PaintHeader();
                    x61467fe65a98f20c.EndPaint(base.Handle, ref paintstruct);
                }
                base.WndProc(ref m);
                if (this.xdc87e2b99332cd4a.RealActive)
                {
                    switch (((uint) m.Msg))
                    {
                        case 0x1204:
                        case 0x120c:
                            this.PaintHeader();
                            break;
                    }
                }
            }
        }
    }
}

