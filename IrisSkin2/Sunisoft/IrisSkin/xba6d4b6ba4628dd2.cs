namespace Sunisoft.IrisSkin
{
    using System;
    using System.Collections;
    using System.Drawing;
    using System.Windows.Forms;

    internal class xba6d4b6ba4628dd2 : x2edc3f693fe78d2e
    {
        private Hashtable x4d8ef95511cad1be;

        public xba6d4b6ba4628dd2(Control control, SkinEngine engine) : base(control, engine)
        {
            this.x4d8ef95511cad1be = new Hashtable();
        }

        protected override void DoInit()
        {
            base.DoInit();
            StatusBar ctrl = (StatusBar) base.Ctrl;
            ctrl.DrawItem += new StatusBarDrawItemEventHandler(this.xfb5158b9af0961d6);
            this.x62669b25bbf75a19();
        }

        protected override void OnCurrentSkinChanged(object sender, SkinChangedEventArgs e)
        {
            base.OnCurrentSkinChanged(sender, e);
            this.x62669b25bbf75a19();
        }

        protected override void PaintControl()
        {
            if (((base.Ctrl.ClientRectangle.Width > 0) && (base.Ctrl.ClientRectangle.Height > 0)) && base.CanPaint)
            {
                StatusBar ctrl = (StatusBar) base.Ctrl;
                Brush brush = base.Engine.Res.Brushes.SKIN2_FORMCOLOR;
                using (Bitmap bitmap = new Bitmap(ctrl.Width, ctrl.Height))
                {
                    using (Graphics graphics = Graphics.FromImage(bitmap))
                    {
                        Rectangle rect = new Rectangle(0, 0, ctrl.Width, ctrl.Height);
                        graphics.FillRectangle(brush, rect);
                        if (ctrl.SizingGrip)
                        {
                            this.x3e99179c1c1e8d0c(graphics);
                            rect.Width -= 0x11;
                        }
                        if (!ctrl.ShowPanels)
                        {
                            brush = base.Engine.Res.Brushes.SKIN2_CONTROLFONTCOLOR;
                            Font font = ctrl.Font;
                            if (rect.Width > 0)
                            {
                                StringFormat format = new StringFormat();
                                if ((ctrl.RightToLeft & RightToLeft.Yes) == RightToLeft.Yes)
                                {
                                    format.FormatFlags |= StringFormatFlags.DirectionRightToLeft;
                                }
                                format.LineAlignment = StringAlignment.Center;
                                format.FormatFlags |= StringFormatFlags.NoWrap;
                                graphics.DrawString(ctrl.Text, font, brush, rect, format);
                            }
                        }
                        else
                        {
                            int num = 0;
                            int key = 0;
                            foreach (StatusBarPanel panel in ctrl.Panels)
                            {
                                ButtonBorderStyle none;
                                int width = panel.Width;
                                if (this.x4d8ef95511cad1be.ContainsKey(key) && (this.x4d8ef95511cad1be[key] is int))
                                {
                                    width = (int) this.x4d8ef95511cad1be[key];
                                }
                                brush = base.Engine.Res.Brushes.SKIN2_CONTROLBORDERCOLOR;
                                int right = num + width;
                                if (panel.BorderStyle == StatusBarPanelBorderStyle.None)
                                {
                                    none = ButtonBorderStyle.None;
                                }
                                else if (panel.BorderStyle == StatusBarPanelBorderStyle.Raised)
                                {
                                    none = ButtonBorderStyle.Outset;
                                }
                                else
                                {
                                    none = ButtonBorderStyle.Inset;
                                }
                                Rectangle bounds = Rectangle.FromLTRB(num + 1, rect.Top + 1, right, rect.Bottom);
                                ControlPaint.DrawBorder(graphics, bounds, base.Engine.Res.Colors.SKIN2_CONTROLBORDERCOLOR, none);
                                brush = base.Engine.Res.Brushes.SKIN2_CONTROLFONTCOLOR;
                                if (panel.Icon != null)
                                {
                                    graphics.DrawIcon(panel.Icon, bounds.X + 4, bounds.Top + ((bounds.Height - panel.Icon.Height) / 2));
                                    bounds.X += 6 + panel.Icon.Width;
                                    bounds.Width -= 6 + panel.Icon.Width;
                                }
                                if (bounds.Width > 0)
                                {
                                    StringFormat format2 = new StringFormat {
                                        LineAlignment = StringAlignment.Center
                                    };
                                    format2.FormatFlags |= StringFormatFlags.NoWrap;
                                    switch (panel.Alignment)
                                    {
                                        case HorizontalAlignment.Left:
                                            format2.Alignment = StringAlignment.Near;
                                            break;

                                        case HorizontalAlignment.Right:
                                            format2.Alignment = StringAlignment.Far;
                                            break;

                                        case HorizontalAlignment.Center:
                                            format2.Alignment = StringAlignment.Center;
                                            break;
                                    }
                                    graphics.DrawString(panel.Text, ctrl.Font, brush, bounds, format2);
                                }
                                num = right;
                                key++;
                            }
                        }
                    }
                    using (Graphics graphics2 = Graphics.FromHwnd(ctrl.Handle))
                    {
                        graphics2.DrawImageUnscaled(bitmap, 0, 0);
                    }
                }
            }
        }

        protected void x3e99179c1c1e8d0c(Graphics x4b101060f4767186)
        {
            StatusBar ctrl = (StatusBar) base.Ctrl;
            Rectangle rect = Rectangle.FromLTRB(ctrl.Width - 0x10, ctrl.Height - 15, ctrl.Width - 1, ctrl.Height - 1);
            Brush brush = base.Engine.Res.Brushes.SKIN2_FORMCOLOR;
            x4b101060f4767186.FillRectangle(brush, rect);
            Pen pen = new Pen(Color.FromKnownColor(KnownColor.InactiveCaption), 1f);
            x4b101060f4767186.DrawLine(pen, rect.Left + 1, rect.Bottom, rect.Right, rect.Top);
            x4b101060f4767186.DrawLine(pen, rect.Left + 5, rect.Bottom, rect.Right, rect.Top + 4);
            x4b101060f4767186.DrawLine(pen, rect.Left + 9, rect.Bottom, rect.Right, rect.Top + 8);
            x4b101060f4767186.DrawLine(pen, rect.Left + 13, rect.Bottom, rect.Right, rect.Top + 12);
            if ((ctrl.Height - 15) > 3)
            {
                rect = Rectangle.FromLTRB(ctrl.Width - 0x10, 3, ctrl.Width - 1, ctrl.Height - 15);
                x4b101060f4767186.FillRectangle(brush, rect);
            }
            rect = Rectangle.FromLTRB(ctrl.Width - 20, 3, ctrl.Width - 0x10, ctrl.Height - 1);
            x4b101060f4767186.FillRectangle(brush, rect);
            pen.Dispose();
        }

        private void x62669b25bbf75a19()
        {
            StatusBar ctrl = (StatusBar) base.Ctrl;
            if (base.CanPaint)
            {
                foreach (StatusBarPanel panel in ctrl.Panels)
                {
                    panel.Style = StatusBarPanelStyle.OwnerDraw;
                }
            }
            else
            {
                foreach (StatusBarPanel panel2 in ctrl.Panels)
                {
                    panel2.Style = StatusBarPanelStyle.Text;
                }
            }
        }

        private void xfb5158b9af0961d6(object xe0292b9ed559da7d, StatusBarDrawItemEventArgs xedd3aee327ed365e)
        {
            if (this.x4d8ef95511cad1be.ContainsKey(xedd3aee327ed365e.Index))
            {
                this.x4d8ef95511cad1be[xedd3aee327ed365e.Index] = xedd3aee327ed365e.Bounds.Width;
            }
            else
            {
                this.x4d8ef95511cad1be.Add(xedd3aee327ed365e.Index, xedd3aee327ed365e.Bounds.Width);
            }
        }

        protected override bool ChangeBackColor
        {
            get
            {
                return true;
            }
        }
    }
}

