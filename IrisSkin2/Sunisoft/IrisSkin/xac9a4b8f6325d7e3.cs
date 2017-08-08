namespace Sunisoft.IrisSkin
{
    using System;
    using System.Collections;
    using System.Drawing;
    using System.Drawing.Drawing2D;
    using System.Drawing.Text;
    using System.Windows.Forms;

    internal class xac9a4b8f6325d7e3 : x8917d01b98173f4c
    {
        private const int x4aac5d4d05a8fbe8 = 2;
        private Hashtable x6d04a76446187b96;
        private const int x7a64d9f3c21462d5 = 13;
        private static StringFormat xae3b2752a89e7464 = new StringFormat();
        private const int xaeefdaeb2a720f7d = 2;

        static xac9a4b8f6325d7e3()
        {
            xae3b2752a89e7464.Alignment = StringAlignment.Center;
            xae3b2752a89e7464.LineAlignment = StringAlignment.Center;
            xae3b2752a89e7464.HotkeyPrefix = HotkeyPrefix.Show;
        }

        public xac9a4b8f6325d7e3(Control control, SkinEngine engine) : base(control, engine)
        {
            this.x6d04a76446187b96 = new Hashtable();
        }

        protected override bool BeforeWndProc(ref Message m)
        {
            switch (((uint) m.Msg))
            {
                case 5:
                    this.PaintControl();
                    break;

                case 15:
                    x40255b11ef821fa3.PAINTSTRUCT paintstruct;
                    x61467fe65a98f20c.BeginPaint(base.Ctrl.Handle, out paintstruct);
                    this.PaintControl();
                    x61467fe65a98f20c.EndPaint(base.Ctrl.Handle, ref paintstruct);
                    break;
            }
            return base.BeforeWndProc(ref m);
        }

        protected override void DoInit()
        {
            base.DoInit();
            ToolBar ctrl = (ToolBar) base.Ctrl;
            ctrl.MouseMove += new MouseEventHandler(this.x92735afcf18b3c70);
            ctrl.ButtonDropDown += new ToolBarButtonClickEventHandler(this.x7f302ba563726f8f);
        }

        protected override void PaintControl()
        {
            if (((base.Ctrl.ClientRectangle.Width > 0) && (base.Ctrl.ClientRectangle.Height > 0)) && base.CanPaint)
            {
                ToolBar ctrl = (ToolBar) base.Ctrl;
                IntPtr windowDC = x61467fe65a98f20c.GetWindowDC(base.Handle);
                Bitmap image = base.Engine.Res.Bitmaps.SKIN2_TOOLBAR;
                using (Graphics graphics = Graphics.FromHwnd(base.Handle))
                {
                    using (Bitmap bitmap2 = new Bitmap(ctrl.Width, ctrl.Height))
                    {
                        using (Graphics graphics2 = Graphics.FromImage(bitmap2))
                        {
                            if (image != null)
                            {
                                graphics2.DrawImage(image, new Rectangle(0, 0, ctrl.Width, ctrl.Height), 0, 0, image.Width, image.Height, GraphicsUnit.Pixel, x448fd9ab43628c71.DrawImageAttrTileY);
                            }
                            else
                            {
                                using (LinearGradientBrush brush = new LinearGradientBrush(new Rectangle(0, 0, ctrl.Width, ctrl.Height), base.Engine.Res.Colors.SKIN2_TOOLBARSTARTCOLOR, base.Engine.Res.Colors.SKIN2_TOOLBARENDCOLOR, LinearGradientMode.Vertical))
                                {
                                    graphics2.FillRectangle(brush, 0, 0, ctrl.Width, ctrl.Height);
                                }
                            }
                            foreach (ToolBarButton button in ctrl.Buttons)
                            {
                                if (!button.Visible)
                                {
                                    continue;
                                }
                                Rectangle rectangle = button.Rectangle;
                                if (button.Style == ToolBarButtonStyle.Separator)
                                {
                                    this.xa8d12864a9092939(graphics2, rectangle, button);
                                    continue;
                                }
                                if (button.Style == ToolBarButtonStyle.ToggleButton)
                                {
                                    if (button.Pushed)
                                    {
                                        this.xa8337c38d02b5c00(graphics2, rectangle, button);
                                    }
                                    else if (base.ctrlMouseState == xb9506a535e31f22a.MouseIn)
                                    {
                                        if (x448fd9ab43628c71.InRect(ctrl.PointToClient(Control.MousePosition), rectangle))
                                        {
                                            this.x721082aab66b93c4(graphics2, rectangle, button);
                                        }
                                        else
                                        {
                                            this.xb30ec7cfdf3e5c19(graphics2, rectangle, button);
                                        }
                                    }
                                    else if (base.ctrlMouseState == xb9506a535e31f22a.MouseDown)
                                    {
                                        if (x448fd9ab43628c71.InRect(ctrl.PointToClient(Control.MousePosition), rectangle))
                                        {
                                            this.xa8337c38d02b5c00(graphics2, rectangle, button);
                                        }
                                        else
                                        {
                                            this.xb30ec7cfdf3e5c19(graphics2, rectangle, button);
                                        }
                                    }
                                    else
                                    {
                                        this.xb30ec7cfdf3e5c19(graphics2, rectangle, button);
                                    }
                                    continue;
                                }
                                switch (base.ctrlMouseState)
                                {
                                    case xb9506a535e31f22a.MouseIn:
                                    {
                                        if (!x448fd9ab43628c71.InRect(ctrl.PointToClient(Control.MousePosition), rectangle))
                                        {
                                            goto Label_02A3;
                                        }
                                        this.x721082aab66b93c4(graphics2, rectangle, button);
                                        continue;
                                    }
                                    case xb9506a535e31f22a.MouseDown:
                                    {
                                        if (!x448fd9ab43628c71.InRect(ctrl.PointToClient(Control.MousePosition), rectangle))
                                        {
                                            break;
                                        }
                                        this.xa8337c38d02b5c00(graphics2, rectangle, button);
                                        continue;
                                    }
                                    default:
                                        goto Label_02B1;
                                }
                                this.xb30ec7cfdf3e5c19(graphics2, rectangle, button);
                                continue;
                            Label_02A3:
                                this.xb30ec7cfdf3e5c19(graphics2, rectangle, button);
                                continue;
                            Label_02B1:
                                this.xb30ec7cfdf3e5c19(graphics2, rectangle, button);
                            }
                        }
                        graphics.DrawImageUnscaled(bitmap2, 0, 0);
                    }
                    x61467fe65a98f20c.ReleaseDC(base.Handle, windowDC);
                }
            }
        }

        private void x721082aab66b93c4(Graphics x4b101060f4767186, Rectangle xb55b340ae3a3e4e0, ToolBarButton x128517d7ded59312)
        {
            if (x128517d7ded59312.Style == ToolBarButtonStyle.DropDownButton)
            {
                this.xc05dfbfc3f35bad9(x4b101060f4767186, xb55b340ae3a3e4e0, base.Engine.Res.Brushes.SKIN2_TOOLBARONCOLOR, base.Engine.Res.Brushes.SKIN2_TOOLBARDOWNCOLOR, true, true, true, x128517d7ded59312);
            }
            else
            {
                this.xc05dfbfc3f35bad9(x4b101060f4767186, xb55b340ae3a3e4e0, base.Engine.Res.Brushes.SKIN2_TOOLBARONCOLOR, base.Engine.Res.Brushes.SKIN2_TOOLBARDOWNCOLOR, true, false, true, x128517d7ded59312);
            }
        }

        private void x7f302ba563726f8f(object xe0292b9ed559da7d, ToolBarButtonClickEventArgs xfbf34718e704c6bc)
        {
            if ((xfbf34718e704c6bc.Button.DropDownMenu != null) && !this.x6d04a76446187b96.ContainsKey(xfbf34718e704c6bc.Button.DropDownMenu.Handle))
            {
                xa1883d0b59b7005b xadbbb = new xa1883d0b59b7005b(base.Engine, xfbf34718e704c6bc.Button.DropDownMenu);
                this.x6d04a76446187b96.Add(xfbf34718e704c6bc.Button.DropDownMenu.Handle, xadbbb);
            }
        }

        private void x92735afcf18b3c70(object xe0292b9ed559da7d, MouseEventArgs xfbf34718e704c6bc)
        {
            this.PaintControl();
        }

        private void xa8337c38d02b5c00(Graphics x4b101060f4767186, Rectangle xb55b340ae3a3e4e0, ToolBarButton x128517d7ded59312)
        {
            if (x128517d7ded59312.Style == ToolBarButtonStyle.DropDownButton)
            {
                this.xc05dfbfc3f35bad9(x4b101060f4767186, xb55b340ae3a3e4e0, base.Engine.Res.Brushes.SKIN2_TOOLBARDOWNCOLOR, base.Engine.Res.Brushes.SKIN2_TOOLBARDOWNCOLOR, true, true, true, x128517d7ded59312);
            }
            else
            {
                this.xc05dfbfc3f35bad9(x4b101060f4767186, xb55b340ae3a3e4e0, base.Engine.Res.Brushes.SKIN2_TOOLBARDOWNCOLOR, base.Engine.Res.Brushes.SKIN2_TOOLBARDOWNCOLOR, true, false, true, x128517d7ded59312);
            }
        }

        private void xa8d12864a9092939(Graphics x4b101060f4767186, Rectangle xb55b340ae3a3e4e0, ToolBarButton x128517d7ded59312)
        {
            Point point = new Point(xb55b340ae3a3e4e0.X + (xb55b340ae3a3e4e0.Width / 2), xb55b340ae3a3e4e0.Top + 1);
            Point point2 = new Point(xb55b340ae3a3e4e0.X + (xb55b340ae3a3e4e0.Width / 2), xb55b340ae3a3e4e0.Bottom - 1);
            using (Pen pen = new Pen(Color.FromKnownColor(KnownColor.ControlDark), 1f))
            {
                x4b101060f4767186.DrawLine(pen, point, point2);
            }
        }

        private void xb30ec7cfdf3e5c19(Graphics x4b101060f4767186, Rectangle xb55b340ae3a3e4e0, ToolBarButton x128517d7ded59312)
        {
            if (x128517d7ded59312.Style == ToolBarButtonStyle.DropDownButton)
            {
                this.xc05dfbfc3f35bad9(x4b101060f4767186, xb55b340ae3a3e4e0, null, null, false, true, false, x128517d7ded59312);
            }
            else
            {
                this.xc05dfbfc3f35bad9(x4b101060f4767186, xb55b340ae3a3e4e0, null, null, false, false, false, x128517d7ded59312);
            }
        }

        private void xc05dfbfc3f35bad9(Graphics x4b101060f4767186, Rectangle xb55b340ae3a3e4e0, Brush x4a36d7ee5781ce75, Brush x35feaa6e77973c81, bool x96554d34fd74b02c, bool xb223465a379c4c80, bool x5afff747d2706456, ToolBarButton x128517d7ded59312)
        {
            ToolBar ctrl = (ToolBar) base.Ctrl;
            Image image = null;
            int width = 0;
            int height = 0;
            if (ctrl.ImageList != null)
            {
                width = ctrl.ImageList.ImageSize.Width;
                height = ctrl.ImageList.ImageSize.Height;
                if ((ctrl.ImageList.Images.Count >= (x128517d7ded59312.ImageIndex + 1)) && (x128517d7ded59312.ImageIndex != -1))
                {
                    image = ctrl.ImageList.Images[x128517d7ded59312.ImageIndex];
                }
            }
            if (x96554d34fd74b02c && x128517d7ded59312.Enabled)
            {
                x4b101060f4767186.FillRectangle(x4a36d7ee5781ce75, xb55b340ae3a3e4e0);
            }
            if (x128517d7ded59312.PartialPush)
            {
                x4b101060f4767186.FillRectangle(Brushes.WhiteSmoke, xb55b340ae3a3e4e0);
            }
            if (x5afff747d2706456 && x128517d7ded59312.Enabled)
            {
                using (Pen pen = new Pen(base.Engine.Res.Colors.SKIN2_TOOLBARBORDERCOLOR))
                {
                    x4b101060f4767186.DrawRectangle(pen, xb55b340ae3a3e4e0.X, xb55b340ae3a3e4e0.Y, xb55b340ae3a3e4e0.Width - 2, xb55b340ae3a3e4e0.Height - 2);
                }
            }
            if (image != null)
            {
                if (ctrl.TextAlign == ToolBarTextAlign.Right)
                {
                    if (x128517d7ded59312.Enabled)
                    {
                        x4b101060f4767186.DrawImageUnscaled(image, 2 + xb55b340ae3a3e4e0.X, xb55b340ae3a3e4e0.Y + ((xb55b340ae3a3e4e0.Height - image.Height) / 2));
                    }
                    else if (x128517d7ded59312.PartialPush)
                    {
                        ControlPaint.DrawImageDisabled(x4b101060f4767186, image, 2 + xb55b340ae3a3e4e0.X, xb55b340ae3a3e4e0.Y + ((xb55b340ae3a3e4e0.Height - image.Height) / 2), Color.WhiteSmoke);
                    }
                    else
                    {
                        ControlPaint.DrawImageDisabled(x4b101060f4767186, image, 2 + xb55b340ae3a3e4e0.X, xb55b340ae3a3e4e0.Y + ((xb55b340ae3a3e4e0.Height - image.Height) / 2), Color.Gray);
                    }
                }
                else if (x128517d7ded59312.Enabled)
                {
                    x4b101060f4767186.DrawImageUnscaled(image, xb55b340ae3a3e4e0.X + ((xb55b340ae3a3e4e0.Width - image.Width) / 2), 2 + xb55b340ae3a3e4e0.Y);
                }
                else if (x128517d7ded59312.PartialPush)
                {
                    ControlPaint.DrawImageDisabled(x4b101060f4767186, image, xb55b340ae3a3e4e0.X + ((xb55b340ae3a3e4e0.Width - image.Width) / 2), 2 + xb55b340ae3a3e4e0.Y, Color.WhiteSmoke);
                }
                else
                {
                    ControlPaint.DrawImageDisabled(x4b101060f4767186, image, xb55b340ae3a3e4e0.X + ((xb55b340ae3a3e4e0.Width - image.Width) / 2), 2 + xb55b340ae3a3e4e0.Y, Color.Gray);
                }
            }
            if ((x128517d7ded59312.Text != null) && (x128517d7ded59312.Text != ""))
            {
                Rectangle rectangle;
                if (ctrl.TextAlign == ToolBarTextAlign.Right)
                {
                    if (x128517d7ded59312.Style != ToolBarButtonStyle.DropDownButton)
                    {
                        rectangle = Rectangle.FromLTRB(((xb55b340ae3a3e4e0.X + width) + 2) + 1, xb55b340ae3a3e4e0.Y, xb55b340ae3a3e4e0.Right - 1, xb55b340ae3a3e4e0.Bottom);
                    }
                    else
                    {
                        rectangle = Rectangle.FromLTRB(((xb55b340ae3a3e4e0.X + width) + 2) + 1, xb55b340ae3a3e4e0.Y, (xb55b340ae3a3e4e0.Right - 1) - 13, xb55b340ae3a3e4e0.Bottom);
                    }
                }
                else if (x128517d7ded59312.Style != ToolBarButtonStyle.DropDownButton)
                {
                    rectangle = Rectangle.FromLTRB(xb55b340ae3a3e4e0.X, ((xb55b340ae3a3e4e0.Y + height) + 2) + 1, xb55b340ae3a3e4e0.Right, xb55b340ae3a3e4e0.Bottom);
                }
                else
                {
                    rectangle = Rectangle.FromLTRB(xb55b340ae3a3e4e0.X, ((xb55b340ae3a3e4e0.Y + height) + 2) + 1, (xb55b340ae3a3e4e0.Right - 1) - 13, xb55b340ae3a3e4e0.Bottom);
                }
                if (x128517d7ded59312.Enabled && !x128517d7ded59312.PartialPush)
                {
                    x4b101060f4767186.DrawString(x128517d7ded59312.Text, ctrl.Font, Brushes.Black, rectangle, xae3b2752a89e7464);
                }
                else
                {
                    x4b101060f4767186.DrawString(x128517d7ded59312.Text, ctrl.Font, Brushes.Gray, rectangle, xae3b2752a89e7464);
                }
            }
            if (xb223465a379c4c80)
            {
                Rectangle rectangle2 = Rectangle.FromLTRB(xb55b340ae3a3e4e0.Right - 13, xb55b340ae3a3e4e0.Y, xb55b340ae3a3e4e0.Right, xb55b340ae3a3e4e0.Bottom);
                if (x96554d34fd74b02c && x128517d7ded59312.Enabled)
                {
                    x4b101060f4767186.FillRectangle(x35feaa6e77973c81, rectangle2.X, rectangle2.Y + 1, rectangle2.Width - 1, rectangle2.Height - 2);
                }
                Brush brush = base.Engine.Res.Brushes.SKIN2_TOOLBARBORDERCOLOR;
                if (x96554d34fd74b02c && x128517d7ded59312.Enabled)
                {
                    using (Pen pen2 = new Pen(brush, 1f))
                    {
                        x4b101060f4767186.DrawRectangle(pen2, rectangle2.X, rectangle2.Y, rectangle2.Width - 2, rectangle2.Height - 2);
                    }
                }
                x448fd9ab43628c71.DrawArrowDown(x4b101060f4767186, (rectangle2.X + (rectangle2.Width / 2)) - 4, (rectangle2.Y + (rectangle2.Height / 2)) - 2, x128517d7ded59312.Enabled);
            }
        }
    }
}

