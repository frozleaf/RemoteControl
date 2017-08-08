namespace Sunisoft.IrisSkin
{
    using System;
    using System.Drawing;
    using System.Windows.Forms;

    internal class xc3d08931d227ebfe : x8917d01b98173f4c
    {
        public xc3d08931d227ebfe(Control control, SkinEngine engine) : base(control, engine)
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
                    m.Result = IntPtr.Zero;
                    return false;
                }
                case 0x402:
                    this.PaintControl();
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
                ProgressBar ctrl = (ProgressBar) base.Ctrl;
                using (Bitmap bitmap = new Bitmap(base.Ctrl.Width, base.Ctrl.Height))
                {
                    using (Graphics graphics = Graphics.FromImage(bitmap))
                    {
                        this.x8bc95f030953f87b(graphics);
                    }
                    using (Graphics graphics2 = Graphics.FromHwnd(base.Handle))
                    {
                        if (this.x0e7ffd184973f9bc)
                        {
                            bitmap.RotateFlip(RotateFlipType.RotateNoneFlipX);
                            graphics2.DrawImageUnscaled(bitmap, 0, 0);
                        }
                        else
                        {
                            graphics2.DrawImageUnscaled(bitmap, 0, 0);
                        }
                    }
                }
            }
        }

        private void x8bc95f030953f87b(Graphics x4b101060f4767186)
        {
            ProgressBar ctrl = (ProgressBar) base.Ctrl;
            Rectangle clientRectangle = ctrl.ClientRectangle;
            int width = ctrl.Width;
            int height = ctrl.Height;
            int num5 = 2;
            Bitmap image = base.Engine.Res.Bitmaps.SKIN2_PROGRESSBAR1;
            int x = image.Width;
            Rectangle srcRect = new Rectangle(0, 0, image.Width, num5);
            Rectangle destRect = srcRect;
            x4b101060f4767186.DrawImage(image, destRect, srcRect, GraphicsUnit.Pixel);
            srcRect = new Rectangle(0, image.Height - num5, image.Width, num5);
            destRect = new Rectangle(0, height - num5, image.Width, num5);
            x4b101060f4767186.DrawImage(image, destRect, srcRect, GraphicsUnit.Pixel);
            srcRect = Rectangle.FromLTRB(0, num5, image.Width, image.Height - num5);
            destRect = new Rectangle(0, num5, image.Width, height - num5);
            x4b101060f4767186.DrawImage(image, destRect, srcRect, GraphicsUnit.Pixel);
            image = base.Engine.Res.Bitmaps.SKIN2_PROGRESSBAR4;
            int right = width - image.Width;
            srcRect = new Rectangle(0, 0, image.Width, num5);
            destRect = new Rectangle(width - image.Width, 0, image.Width, num5);
            x4b101060f4767186.DrawImage(image, destRect, srcRect, GraphicsUnit.Pixel);
            srcRect = new Rectangle(0, image.Height - num5, image.Width, num5);
            destRect = new Rectangle(width - image.Width, height - num5, image.Width, num5);
            x4b101060f4767186.DrawImage(image, destRect, srcRect, GraphicsUnit.Pixel);
            srcRect = Rectangle.FromLTRB(0, num5, image.Width, image.Height - num5);
            destRect = new Rectangle(width - image.Width, num5, image.Width, height - num5);
            x4b101060f4767186.DrawImage(image, destRect, srcRect, GraphicsUnit.Pixel);
            if (ctrl.Maximum != ctrl.Minimum)
            {
                clientRectangle = new Rectangle(x, 0, ((ctrl.Value - ctrl.Minimum) * (right - x)) / (ctrl.Maximum - ctrl.Minimum), ctrl.Height);
                image = base.Engine.Res.Bitmaps.SKIN2_PROGRESSBAR2;
                srcRect = new Rectangle(0, 0, image.Width, num5);
                destRect = Rectangle.FromLTRB(clientRectangle.Left, clientRectangle.Top, clientRectangle.Right, num5);
                x4b101060f4767186.DrawImage(image, destRect, srcRect.X, srcRect.Y, srcRect.Width, srcRect.Height, GraphicsUnit.Pixel, x448fd9ab43628c71.DrawImageAttrTileX);
                srcRect = new Rectangle(0, image.Height - num5, image.Width, num5);
                destRect = Rectangle.FromLTRB(clientRectangle.Left, clientRectangle.Bottom - num5, clientRectangle.Right, clientRectangle.Bottom);
                x4b101060f4767186.DrawImage(image, destRect, srcRect.X, srcRect.Y, srcRect.Width, srcRect.Height, GraphicsUnit.Pixel, x448fd9ab43628c71.DrawImageAttrTileX);
                srcRect = Rectangle.FromLTRB(0, num5, image.Width, image.Height - num5);
                destRect = Rectangle.FromLTRB(clientRectangle.Left, clientRectangle.Top + num5, clientRectangle.Right, clientRectangle.Bottom - num5);
                x4b101060f4767186.DrawImage(image, destRect, srcRect.X, srcRect.Y, srcRect.Width, srcRect.Height, GraphicsUnit.Pixel, x448fd9ab43628c71.DrawImageAttrTileX);
                clientRectangle = Rectangle.FromLTRB(clientRectangle.Right, clientRectangle.Top, right, clientRectangle.Bottom);
                image = base.Engine.Res.Bitmaps.SKIN2_PROGRESSBAR3;
                srcRect = new Rectangle(0, 0, image.Width, num5);
                destRect = Rectangle.FromLTRB(clientRectangle.Left, clientRectangle.Top, clientRectangle.Right, num5);
                x4b101060f4767186.DrawImage(image, destRect, srcRect.X, srcRect.Y, srcRect.Width, srcRect.Height, GraphicsUnit.Pixel, x448fd9ab43628c71.DrawImageAttrTileX);
                srcRect = new Rectangle(0, image.Height - num5, image.Width, num5);
                destRect = Rectangle.FromLTRB(clientRectangle.Left, clientRectangle.Bottom - num5, clientRectangle.Right, clientRectangle.Bottom);
                x4b101060f4767186.DrawImage(image, destRect, srcRect.X, srcRect.Y, srcRect.Width, srcRect.Height, GraphicsUnit.Pixel, x448fd9ab43628c71.DrawImageAttrTileX);
                srcRect = Rectangle.FromLTRB(0, num5, image.Width, image.Height - num5);
                destRect = Rectangle.FromLTRB(clientRectangle.Left, clientRectangle.Top + num5, clientRectangle.Right, clientRectangle.Bottom - num5);
                x4b101060f4767186.DrawImage(image, destRect, srcRect.X, srcRect.Y, srcRect.Width, srcRect.Height, GraphicsUnit.Pixel, x448fd9ab43628c71.DrawImageAttrTileX);
            }
        }

        private bool x0e7ffd184973f9bc
        {
            get
            {
                ProgressBar ctrl = (ProgressBar) base.Ctrl;
                return (ctrl.RightToLeftLayout && ((ctrl.RightToLeft & RightToLeft.Yes) == RightToLeft.Yes));
            }
        }
    }
}

