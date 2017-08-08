namespace Sunisoft.IrisSkin
{
    using System;
    using System.Drawing;
    using System.Drawing.Text;
    using System.Windows.Forms;

    internal class x97d171718e5c7e7f : x8917d01b98173f4c
    {
        public x97d171718e5c7e7f(Control control, SkinEngine engine) : base(control, engine)
        {
            if (this.x0e7ffd184973f9bc)
            {
                uint windowLong = x61467fe65a98f20c.GetWindowLong(base.Handle, -20);
                if ((windowLong & 0x400000) == 0x400000)
                {
                    windowLong -= 0x400000;
                }
                x61467fe65a98f20c.SetWindowLong(base.Handle, -20, windowLong);
            }
        }

        protected override bool BeforeWndProc(ref Message m)
        {
            switch (((uint) m.Msg))
            {
                case 15:
                {
                    x40255b11ef821fa3.PAINTSTRUCT lpPaint = new x40255b11ef821fa3.PAINTSTRUCT();
                    x61467fe65a98f20c.BeginPaint(base.Ctrl.Handle, out lpPaint);
                    this.PaintControl();
                    x61467fe65a98f20c.EndPaint(base.Ctrl.Handle, ref lpPaint);
                    return false;
                }
                case 0x84:
                    if (this.x0e7ffd184973f9bc)
                    {
                        int num = m.LParam.ToInt32();
                        Point p = new Point(num & 0xffff, num >> 0x10);
                        p = base.Ctrl.PointToClient(p);
                        p.X = base.Ctrl.Width - p.X;
                        p = base.Ctrl.PointToScreen(p);
                        num = p.X + (p.Y << 0x10);
                        m.LParam = new IntPtr(num);
                    }
                    break;

                case 0x201:
                    if (this.x0e7ffd184973f9bc)
                    {
                        int num2 = m.LParam.ToInt32();
                        num2 = ((base.Ctrl.Width - num2) & 0xffff) + (num2 & 0xfff0000);
                        m.LParam = new IntPtr(num2);
                    }
                    break;
            }
            return base.BeforeWndProc(ref m);
        }

        protected override void OnCurrentSkinChanged(object sender, SkinChangedEventArgs e)
        {
            base.OnCurrentSkinChanged(sender, e);
            if (!base.CanPaint)
            {
                if (this.x0e7ffd184973f9bc)
                {
                    uint dwNewLong = x61467fe65a98f20c.GetWindowLong(base.Handle, -20) & 0x400000;
                    x61467fe65a98f20c.SetWindowLong(base.Handle, -20, dwNewLong);
                }
            }
            else if (this.x0e7ffd184973f9bc)
            {
                uint windowLong = x61467fe65a98f20c.GetWindowLong(base.Handle, -20);
                if ((windowLong & 0x400000) == 0x400000)
                {
                    windowLong -= 0x400000;
                }
                x61467fe65a98f20c.SetWindowLong(base.Handle, -20, windowLong);
            }
        }

        protected override void PaintControl()
        {
            if (((base.Ctrl.ClientRectangle.Width > 0) && (base.Ctrl.ClientRectangle.Height > 0)) && base.CanPaint)
            {
                using (Bitmap bitmap = new Bitmap(base.Ctrl.Width, base.Ctrl.Height))
                {
                    using (Graphics graphics = Graphics.FromImage(bitmap))
                    {
                        this.x8bc95f030953f87b(graphics);
                    }
                    if (this.x0e7ffd184973f9bc)
                    {
                        bitmap.RotateFlip(RotateFlipType.RotateNoneFlipX);
                    }
                    using (Graphics graphics2 = Graphics.FromImage(bitmap))
                    {
                        this.x3bf72114458d67b7(graphics2);
                    }
                    using (Graphics graphics3 = Graphics.FromHwnd(base.Handle))
                    {
                        graphics3.DrawImageUnscaled(bitmap, 0, 0);
                    }
                }
            }
        }

        private void x3bf72114458d67b7(Graphics x41347a961b838962)
        {
            TabControl ctrl = (TabControl) base.Ctrl;
            StringFormat format = new StringFormat {
                FormatFlags = StringFormatFlags.NoWrap,
                HotkeyPrefix = HotkeyPrefix.None,
                LineAlignment = StringAlignment.Center,
                Alignment = StringAlignment.Center
            };
            int height = base.Engine.Res.TabControlRes.TabLine.Height;
            int width = ctrl.Width;
            for (int i = 0; i < ctrl.TabPages.Count; i++)
            {
                Brush brush;
                Image image = null;
                if ((((ctrl.ImageList != null) && (ctrl.TabPages != null)) && ((ctrl.TabPages.Count > i) && (ctrl.TabPages[i] != null))) && ((ctrl.TabPages[i].ImageIndex >= 0) && (ctrl.ImageList.Images.Count > ctrl.TabPages[i].ImageIndex)))
                {
                    image = ctrl.ImageList.Images[ctrl.TabPages[i].ImageIndex];
                }
                Rectangle tabRect = ctrl.GetTabRect(i);
                int num4 = 1;
                switch (ctrl.Alignment)
                {
                    case TabAlignment.Bottom:
                        tabRect.Y += height;
                        tabRect.Height -= height;
                        if (image == null)
                        {
                            goto Label_03B2;
                        }
                        if (!this.x0e7ffd184973f9bc)
                        {
                            break;
                        }
                        x41347a961b838962.DrawImageUnscaled(image, (width - image.Width) - tabRect.X, tabRect.Y);
                        goto Label_0226;

                    case TabAlignment.Left:
                        tabRect.X += num4;
                        format.FormatFlags = StringFormatFlags.DirectionVertical;
                        tabRect.Width -= height;
                        if (image == null)
                        {
                            goto Label_03B2;
                        }
                        if (!this.x0e7ffd184973f9bc)
                        {
                            goto Label_02B8;
                        }
                        x41347a961b838962.DrawImageUnscaled(image, (width - image.Width) - tabRect.X, (tabRect.Bottom - image.Height) - 3);
                        goto Label_02D8;

                    case TabAlignment.Right:
                        tabRect.X -= num4;
                        tabRect.X += height;
                        tabRect.Width -= height;
                        format.FormatFlags = StringFormatFlags.DirectionVertical;
                        tabRect.Width -= height;
                        if (image == null)
                        {
                            goto Label_03B2;
                        }
                        if (!this.x0e7ffd184973f9bc)
                        {
                            goto Label_036C;
                        }
                        x41347a961b838962.DrawImageUnscaled(image, (width - image.Width) - tabRect.X, tabRect.Y + 3);
                        goto Label_0384;

                    default:
                        tabRect.Y += num4;
                        tabRect.Height -= height;
                        if (image != null)
                        {
                            if (this.x0e7ffd184973f9bc)
                            {
                                x41347a961b838962.DrawImageUnscaled(image, (width - image.Width) - (tabRect.X + 2), tabRect.Y);
                            }
                            else
                            {
                                x41347a961b838962.DrawImageUnscaled(image, tabRect.X + 2, tabRect.Y);
                            }
                            tabRect.X += image.Width + 3;
                            tabRect.Width -= image.Width + 3;
                        }
                        goto Label_03B2;
                }
                x41347a961b838962.DrawImageUnscaled(image, tabRect.X, tabRect.Y);
            Label_0226:
                tabRect.X += image.Width;
                tabRect.Width -= image.Width;
                goto Label_03B2;
            Label_02B8:
                x41347a961b838962.DrawImageUnscaled(image, tabRect.X, (tabRect.Bottom - image.Height) - 3);
            Label_02D8:
                tabRect.Height -= image.Height + 3;
                goto Label_03B2;
            Label_036C:
                x41347a961b838962.DrawImageUnscaled(image, tabRect.X, tabRect.Y + 3);
            Label_0384:
                tabRect.Y += image.Height + 3;
                tabRect.Height -= image.Height + 3;
            Label_03B2:
                if (ctrl.SelectedIndex == i)
                {
                    brush = base.Engine.Res.Brushes.SKIN2_TABCONTROLACTIVEFONTCOLOR;
                }
                else
                {
                    brush = base.Engine.Res.Brushes.SKIN2_TABCONTROLINACTIVEFONTCOLOR;
                }
                if (this.x0e7ffd184973f9bc)
                {
                    tabRect.X = (width - tabRect.X) - tabRect.Width;
                }
                x41347a961b838962.DrawString(ctrl.TabPages[i].Text, ctrl.Font, brush, tabRect, format);
                format.FormatFlags = StringFormatFlags.NoWrap;
            }
        }

        private void x8bc95f030953f87b(Graphics x4b101060f4767186)
        {
            TabControl ctrl = (TabControl) base.Ctrl;
            Brush brush = base.Engine.Res.Brushes.SKIN2_FORMCOLOR;
            x4b101060f4767186.FillRectangle(brush, ctrl.ClientRectangle);
            this.xa7a441e9d4de36b0(x4b101060f4767186);
            for (int i = 0; i < ctrl.TabPages.Count; i++)
            {
                this.xc33f5f7a18a754cb(x4b101060f4767186, i);
            }
        }

        private void xa7a441e9d4de36b0(Graphics x4b101060f4767186)
        {
            Rectangle rectangle;
            Bitmap bitmap2;
            TabControl ctrl = (TabControl) base.Ctrl;
            Bitmap tabLine = base.Engine.Res.TabControlRes.TabLine;
            switch (ctrl.Alignment)
            {
                case TabAlignment.Bottom:
                    rectangle = new Rectangle(0, ctrl.DisplayRectangle.Bottom + 2, ctrl.Width, tabLine.Height);
                    bitmap2 = new Bitmap(ctrl.Width, tabLine.Height);
                    break;

                case TabAlignment.Left:
                    rectangle = new Rectangle((ctrl.DisplayRectangle.X - 2) - tabLine.Height, 0, tabLine.Height, ctrl.Height);
                    bitmap2 = new Bitmap(ctrl.Height, tabLine.Height);
                    break;

                case TabAlignment.Right:
                    rectangle = new Rectangle(ctrl.DisplayRectangle.Right + 2, 0, tabLine.Height, ctrl.Height);
                    bitmap2 = new Bitmap(ctrl.Height, tabLine.Height);
                    break;

                default:
                    rectangle = new Rectangle(0, (ctrl.DisplayRectangle.Y - 2) - tabLine.Height, ctrl.Width, tabLine.Height);
                    bitmap2 = new Bitmap(ctrl.Width, tabLine.Height);
                    break;
            }
            using (Graphics graphics = Graphics.FromImage(bitmap2))
            {
                graphics.DrawImage(tabLine, new Rectangle(0, 0, bitmap2.Width, bitmap2.Height), 0, 0, tabLine.Width, tabLine.Height, GraphicsUnit.Pixel, x448fd9ab43628c71.DrawImageAttrTileX);
            }
            switch (ctrl.Alignment)
            {
                case TabAlignment.Bottom:
                    bitmap2.RotateFlip(RotateFlipType.Rotate180FlipNone);
                    break;

                case TabAlignment.Left:
                    bitmap2.RotateFlip(RotateFlipType.Rotate270FlipNone);
                    break;

                case TabAlignment.Right:
                    bitmap2.RotateFlip(RotateFlipType.Rotate90FlipNone);
                    break;
            }
            x4b101060f4767186.DrawImageUnscaled(bitmap2, rectangle);
            bitmap2.Dispose();
            Pen borderPen = base.Engine.Res.TabControlRes.BorderPen;
            switch (ctrl.Alignment)
            {
                case TabAlignment.Bottom:
                    x4b101060f4767186.DrawLine(borderPen, 0, 0, 0, ctrl.DisplayRectangle.Bottom + 2);
                    x4b101060f4767186.DrawLine(borderPen, 0, 0, ctrl.Width, 0);
                    x4b101060f4767186.DrawLine(borderPen, ctrl.Width - 1, 0, ctrl.Width - 1, ctrl.DisplayRectangle.Bottom + 2);
                    return;

                case TabAlignment.Left:
                    x4b101060f4767186.DrawLine(borderPen, ctrl.DisplayRectangle.X - 2, 0, ctrl.Width, 0);
                    x4b101060f4767186.DrawLine(borderPen, ctrl.DisplayRectangle.X - 2, ctrl.Height - 1, ctrl.Width, ctrl.Height - 1);
                    x4b101060f4767186.DrawLine(borderPen, ctrl.Width - 1, 0, ctrl.Width - 1, ctrl.Height);
                    return;

                case TabAlignment.Right:
                    x4b101060f4767186.DrawLine(borderPen, 0, 0, 0, ctrl.Height);
                    x4b101060f4767186.DrawLine(borderPen, 0, 0, ctrl.DisplayRectangle.Right + 2, 0);
                    x4b101060f4767186.DrawLine(borderPen, 0, ctrl.Height - 1, ctrl.DisplayRectangle.Right + 2, ctrl.Height - 1);
                    return;
            }
            x4b101060f4767186.DrawLine(borderPen, 0, ctrl.DisplayRectangle.Y - 2, 0, ctrl.Height);
            x4b101060f4767186.DrawLine(borderPen, 0, ctrl.Height - 1, ctrl.Width, ctrl.Height - 1);
            x4b101060f4767186.DrawLine(borderPen, ctrl.Width - 1, ctrl.DisplayRectangle.Y - 2, ctrl.Width - 1, ctrl.Height);
        }

        private void xc33f5f7a18a754cb(Graphics x41347a961b838962, int x067d6ddeefb41622)
        {
            Bitmap bitmap2;
            int num3;
            TabControl ctrl = (TabControl) base.Ctrl;
            Rectangle tabRect = ctrl.GetTabRect(x067d6ddeefb41622);
            if (ctrl.SelectedIndex == x067d6ddeefb41622)
            {
                num3 = 0;
            }
            else
            {
                num3 = 1;
            }
            switch (ctrl.Alignment)
            {
                case TabAlignment.Left:
                case TabAlignment.Right:
                    bitmap2 = new Bitmap(tabRect.Height, tabRect.Width);
                    break;

                default:
                    bitmap2 = new Bitmap(tabRect.Width, tabRect.Height);
                    break;
            }
            using (Graphics graphics = Graphics.FromImage(bitmap2))
            {
                Bitmap image = base.Engine.Res.TabControlRes.TabImage[num3, 0];
                Rectangle destRect = new Rectangle(0, 0, image.Width, bitmap2.Height);
                int right = destRect.Right;
                graphics.DrawImage(image, destRect, 0, 0, image.Width, image.Height, GraphicsUnit.Pixel, x448fd9ab43628c71.DrawImageAttrTileY);
                image = base.Engine.Res.TabControlRes.TabImage[num3, 2];
                destRect = new Rectangle(bitmap2.Width - image.Width, 0, image.Width, bitmap2.Height);
                int left = destRect.Left;
                graphics.DrawImage(image, destRect, 0, 0, image.Width, image.Height, GraphicsUnit.Pixel, x448fd9ab43628c71.DrawImageAttrTileY);
                image = base.Engine.Res.TabControlRes.TabImage[num3, 1];
                destRect = Rectangle.FromLTRB(right, 0, left, bitmap2.Height);
                graphics.DrawImage(image, destRect, 0, 0, image.Width, image.Height, GraphicsUnit.Pixel, x448fd9ab43628c71.DrawImageAttrTileY);
                image = base.Engine.Res.TabControlRes.TabLine;
                int height = image.Height;
                Rectangle rectangle2 = new Rectangle(0, 0, image.Width, image.Height);
                destRect = new Rectangle(0, bitmap2.Height - image.Height, bitmap2.Width, image.Height);
                graphics.DrawImage(image, destRect, rectangle2.X, rectangle2.Y, rectangle2.Width, rectangle2.Height, GraphicsUnit.Pixel, x448fd9ab43628c71.DrawImageAttrTileX);
            }
            switch (ctrl.Alignment)
            {
                case TabAlignment.Bottom:
                    bitmap2.RotateFlip(RotateFlipType.Rotate180FlipNone);
                    break;

                case TabAlignment.Left:
                    bitmap2.RotateFlip(RotateFlipType.Rotate270FlipNone);
                    break;

                case TabAlignment.Right:
                    bitmap2.RotateFlip(RotateFlipType.Rotate90FlipNone);
                    break;
            }
            x41347a961b838962.DrawImageUnscaled(bitmap2, tabRect.X, tabRect.Y);
            bitmap2.Dispose();
        }

        protected override bool ChangeBackColor
        {
            get
            {
                return false;
            }
        }

        private bool x0e7ffd184973f9bc
        {
            get
            {
                TabControl ctrl = (TabControl) base.Ctrl;
                return (ctrl.RightToLeftLayout && ((ctrl.RightToLeft & RightToLeft.Yes) == RightToLeft.Yes));
            }
        }
    }
}

