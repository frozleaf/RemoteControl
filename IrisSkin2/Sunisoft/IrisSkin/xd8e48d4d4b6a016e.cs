namespace Sunisoft.IrisSkin
{
    using System;
    using System.Drawing;
    using System.Drawing.Drawing2D;
    using System.Windows.Forms;

    internal class xd8e48d4d4b6a016e : x2edc3f693fe78d2e
    {
        private StringFormat x5786461d089b10a0;
        private const int xc2b2889221bc8c5e = 20;

        public xd8e48d4d4b6a016e(Control control, SkinEngine engine) : base(control, engine)
        {
            this.x5786461d089b10a0 = new StringFormat();
            this.x5786461d089b10a0.LineAlignment = StringAlignment.Center;
            this.x5786461d089b10a0.FormatFlags = StringFormatFlags.LineLimit;
        }

        protected override bool BeforeWndProc(ref Message m)
        {
            switch (((uint) m.Msg))
            {
                case 15:
                    x40255b11ef821fa3.PAINTSTRUCT paintstruct;
                    x61467fe65a98f20c.BeginPaint(base.Ctrl.Handle, out paintstruct);
                    this.PaintControl();
                    x61467fe65a98f20c.EndPaint(base.Ctrl.Handle, ref paintstruct);
                    break;

                case 20:
                    return false;
            }
            return base.BeforeWndProc(ref m);
        }

        protected override void PaintControl()
        {
            if ((base.Ctrl.ClientRectangle.Width <= 0) || (base.Ctrl.ClientRectangle.Height <= 0))
            {
                return;
            }
            if (!base.CanPaint)
            {
                return;
            }
            ComboBox ctrl = (ComboBox) base.Ctrl;
            Rectangle rect = new Rectangle(0, 0, ctrl.Width, ctrl.Height);
            if (ctrl.DropDownStyle == ComboBoxStyle.Simple)
            {
                base.PaintControl();
                return;
            }
            IntPtr windowDC = x61467fe65a98f20c.GetWindowDC(base.Ctrl.Handle);
            using (Graphics graphics = Graphics.FromHdc(windowDC))
            {
                if (ctrl.DropDownStyle == ComboBoxStyle.DropDownList)
                {
                    if (ctrl.Enabled)
                    {
                        using (Brush brush = new SolidBrush(base.Ctrl.BackColor))
                        {
                            graphics.FillRectangle(brush, rect);
                            goto Label_0285;
                        }
                    }
                    using (Brush brush2 = new SolidBrush(Color.FromKnownColor(KnownColor.Control)))
                    {
                        graphics.FillRectangle(brush2, rect);
                        goto Label_0285;
                    }
                }
                rect.Height -= 4;
                rect.Width -= 4;
                rect.X += 2;
                rect.Y += 2;
                if (ctrl.Enabled)
                {
                    using (Brush brush3 = new SolidBrush(base.Ctrl.BackColor))
                    {
                        using (Pen pen = new Pen(brush3, 2f))
                        {
                            graphics.DrawRectangle(pen, rect);
                            if ((ctrl.RightToLeft & RightToLeft.Yes) == RightToLeft.Yes)
                            {
                                graphics.DrawLine(pen, (rect.Left + 20) - 3, rect.Top, (rect.Left + 20) - 3, rect.Bottom);
                            }
                            else
                            {
                                graphics.DrawLine(pen, (rect.Right - 20) + 3, rect.Top, (rect.Right - 20) + 3, rect.Bottom);
                            }
                        }
                        goto Label_0285;
                    }
                }
                using (Brush brush4 = new SolidBrush(Color.FromKnownColor(KnownColor.Control)))
                {
                    using (Pen pen2 = new Pen(brush4, 2f))
                    {
                        graphics.DrawRectangle(pen2, rect);
                        if ((ctrl.RightToLeft & RightToLeft.Yes) == RightToLeft.Yes)
                        {
                            graphics.DrawLine(pen2, (rect.Left + 20) - 3, rect.Top, (rect.Left + 20) - 3, rect.Bottom);
                        }
                        else
                        {
                            graphics.DrawLine(pen2, (rect.Right - 20) + 3, rect.Top, (rect.Right - 20) + 3, rect.Bottom);
                        }
                    }
                }
            Label_0285:
                using (Bitmap bitmap = new Bitmap(base.Ctrl.Width, base.Ctrl.Height))
                {
                    using (Graphics graphics2 = Graphics.FromImage(bitmap))
                    {
                        this.x8bc95f030953f87b(graphics2);
                    }
                    graphics.DrawImageUnscaled(bitmap, 0, 0);
                }
                if (ctrl.DropDownStyle == ComboBoxStyle.DropDownList)
                {
                    StringFormat format = new StringFormat {
                        Alignment = StringAlignment.Near
                    };
                    if ((ctrl.RightToLeft & RightToLeft.Yes) == RightToLeft.Yes)
                    {
                        format.FormatFlags = StringFormatFlags.LineLimit | StringFormatFlags.DirectionRightToLeft;
                        rect.X += 20;
                        rect.Width -= 20;
                    }
                    else
                    {
                        format.FormatFlags = StringFormatFlags.LineLimit;
                        rect.X += 2;
                        rect.Width -= 20;
                    }
                    format.LineAlignment = StringAlignment.Center;
                    if (ctrl.Focused && (ctrl.SelectedIndex != -1))
                    {
                        Rectangle rectangle2 = rect;
                        rectangle2.X++;
                        rectangle2.Width -= 3;
                        rectangle2.Y += 3;
                        rectangle2.Height -= 6;
                        using (Brush brush5 = new SolidBrush(Color.FromKnownColor(KnownColor.Highlight)))
                        {
                            graphics.FillRectangle(brush5, rectangle2);
                        }
                        using (Pen pen3 = new Pen(Color.Black))
                        {
                            rectangle2.Width--;
                            rectangle2.Height--;
                            pen3.DashStyle = DashStyle.Dot;
                            graphics.DrawRectangle(pen3, rectangle2);
                        }
                        using (Brush brush6 = new SolidBrush(Color.FromKnownColor(KnownColor.HighlightText)))
                        {
                            graphics.DrawString(ctrl.Text, ctrl.Font, brush6, rect, format);
                            goto Label_04A1;
                        }
                    }
                    using (Brush brush7 = new SolidBrush(ctrl.ForeColor))
                    {
                        graphics.DrawString(ctrl.Text, ctrl.Font, brush7, rect, format);
                    }
                }
            }
        Label_04A1:
            x61467fe65a98f20c.ReleaseDC(base.Handle, windowDC);
        }

        private void x7be4a5b4e2519d4d()
        {
            if ((base.Ctrl.ClientRectangle.Width > 0) && (base.Ctrl.ClientRectangle.Height > 0))
            {
                ComboBox ctrl = (ComboBox) base.Ctrl;
                if (ctrl.DropDownStyle != ComboBoxStyle.Simple)
                {
                    IntPtr windowDC = x61467fe65a98f20c.GetWindowDC(base.Ctrl.Handle);
                    Rectangle rect = new Rectangle(0, 0, ctrl.Width, ctrl.Height);
                    using (Graphics graphics = Graphics.FromHdc(windowDC))
                    {
                        if (ctrl.DropDownStyle == ComboBoxStyle.DropDownList)
                        {
                            if (ctrl.Enabled)
                            {
                                using (Brush brush = new SolidBrush(base.Ctrl.BackColor))
                                {
                                    graphics.FillRectangle(brush, rect);
                                    goto Label_01A4;
                                }
                            }
                            using (Brush brush2 = new SolidBrush(Color.FromKnownColor(KnownColor.Control)))
                            {
                                graphics.FillRectangle(brush2, rect);
                                goto Label_01A4;
                            }
                        }
                        rect.Height -= 4;
                        rect.Width -= 4;
                        rect.X += 2;
                        rect.Y += 2;
                        if (ctrl.Enabled)
                        {
                            using (Brush brush3 = new SolidBrush(base.Ctrl.BackColor))
                            {
                                using (new Pen(brush3, 2f))
                                {
                                    graphics.DrawRectangle(Pens.Red, rect);
                                }
                                goto Label_01A4;
                            }
                        }
                        using (Brush brush4 = new SolidBrush(Color.FromKnownColor(KnownColor.Control)))
                        {
                            using (new Pen(brush4, 2f))
                            {
                                graphics.DrawRectangle(Pens.Red, rect);
                            }
                        }
                    Label_01A4:
                        if (ctrl.DropDownStyle == ComboBoxStyle.DropDownList)
                        {
                            StringFormat format = new StringFormat {
                                Alignment = StringAlignment.Near
                            };
                            if ((ctrl.RightToLeft & RightToLeft.Yes) == RightToLeft.Yes)
                            {
                                format.FormatFlags = StringFormatFlags.LineLimit | StringFormatFlags.DirectionRightToLeft;
                                if (ctrl.DropDownStyle == ComboBoxStyle.DropDownList)
                                {
                                    rect.Width--;
                                }
                            }
                            else
                            {
                                format.FormatFlags = StringFormatFlags.LineLimit;
                                if (ctrl.DropDownStyle == ComboBoxStyle.DropDownList)
                                {
                                    rect.X++;
                                }
                            }
                            format.LineAlignment = StringAlignment.Center;
                            using (Brush brush5 = new SolidBrush(ctrl.ForeColor))
                            {
                                graphics.DrawString(ctrl.Text, ctrl.Font, brush5, rect, format);
                            }
                        }
                    }
                    x61467fe65a98f20c.ReleaseDC(base.Handle, windowDC);
                }
            }
        }

        private void x8bc95f030953f87b(Graphics x4b101060f4767186)
        {
            Rectangle clientRectangle;
            ComboBox ctrl = (ComboBox) base.Ctrl;
            using (Pen pen = new Pen(base.Engine.Res.Brushes.SKIN2_CONTROLBORDERCOLOR, 1f))
            {
                clientRectangle = ctrl.ClientRectangle;
                clientRectangle.Width--;
                clientRectangle.Height--;
                x4b101060f4767186.DrawRectangle(pen, clientRectangle);
            }
            if ((ctrl.RightToLeft & RightToLeft.Yes) == RightToLeft.Yes)
            {
                clientRectangle = new Rectangle(0, 0, 20, ctrl.Height);
            }
            else
            {
                clientRectangle = new Rectangle(ctrl.Width - 20, 0, 20, ctrl.Height);
            }
            clientRectangle.X += 2;
            clientRectangle.Y += 2;
            clientRectangle.Width -= 5;
            clientRectangle.Height -= 5;
            Brush brush = base.Engine.Res.Brushes.SKIN2_FORMCOLOR;
            x4b101060f4767186.FillRectangle(brush, clientRectangle);
            using (Pen pen2 = new Pen(base.Engine.Res.Brushes.SKIN2_CONTROLBORDERCOLOR, 1f))
            {
                x4b101060f4767186.DrawRectangle(pen2, clientRectangle);
            }
            x448fd9ab43628c71.DrawArrowDown(x4b101060f4767186, (clientRectangle.X + 10) - 5, (clientRectangle.Y + (ctrl.Height / 2)) - 3, ctrl.Enabled);
        }

        protected override bool CanScroll
        {
            get
            {
                return false;
            }
        }

        protected override bool ChangeBackColor
        {
            get
            {
                return false;
            }
        }
    }
}

