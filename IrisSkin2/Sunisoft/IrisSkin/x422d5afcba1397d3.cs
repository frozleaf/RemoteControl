namespace Sunisoft.IrisSkin
{
    using System;
    using System.Drawing;
    using System.Windows.Forms;

    internal class x422d5afcba1397d3 : x8917d01b98173f4c
    {
        public x422d5afcba1397d3(Control control, SkinEngine engine) : base(control, engine)
        {
        }

        protected override bool BeforeWndProc(ref Message m)
        {
            if (m.HWnd != base.Ctrl.Handle)
            {
                return true;
            }
            switch (((uint) m.Msg))
            {
                case 8:
                    this.PaintControl();
                    return false;

                case 15:
                {
                    x40255b11ef821fa3.PAINTSTRUCT lpPaint = new x40255b11ef821fa3.PAINTSTRUCT();
                    x61467fe65a98f20c.BeginPaint(base.Ctrl.Handle, out lpPaint);
                    x61467fe65a98f20c.EndPaint(base.Ctrl.Handle, ref lpPaint);
                    m.Result = IntPtr.Zero;
                    return false;
                }
                case 20:
                    m.Result = new IntPtr(1);
                    this.PaintControl();
                    return false;
            }
            return base.BeforeWndProc(ref m);
        }

        protected override void DoInit()
        {
            base.DoInit();
            TrackBar ctrl = (TrackBar) base.Ctrl;
            ctrl.ValueChanged += new EventHandler(this.xf51466e2b7549167);
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
                    using (Graphics graphics2 = Graphics.FromHwnd(base.Handle))
                    {
                        if (this.x0e7ffd184973f9bc)
                        {
                            bitmap.RotateFlip(RotateFlipType.RotateNoneFlipX);
                        }
                        graphics2.DrawImageUnscaled(bitmap, 0, 0);
                    }
                }
            }
        }

        private void x86a0b6ee5f4b48be(Graphics x4b101060f4767186, Pen x90279591611601bc, float xc941868c59399d3e, float xaf9a0436a70689de, float x10aaa7cdfa38f254, float xca09b6c2b5b18485, float x0b5a48bb98ab819f)
        {
            while (x10aaa7cdfa38f254 < xca09b6c2b5b18485)
            {
                x4b101060f4767186.DrawLine(x90279591611601bc, x10aaa7cdfa38f254, xc941868c59399d3e, x10aaa7cdfa38f254, xaf9a0436a70689de);
                x10aaa7cdfa38f254 += x0b5a48bb98ab819f;
                if (x10aaa7cdfa38f254 >= xca09b6c2b5b18485)
                {
                    x4b101060f4767186.DrawLine(x90279591611601bc, xca09b6c2b5b18485, xc941868c59399d3e, xca09b6c2b5b18485, xaf9a0436a70689de);
                }
            }
        }

        private void x8bc95f030953f87b(Graphics x4b101060f4767186)
        {
            int width;
            int height;
            int num7;
            if (!base.CanPaint)
            {
                return;
            }
            TrackBar ctrl = (TrackBar) base.Ctrl;
            int num = 10;
            int x = 6;
            int num3 = 7;
            int num4 = 15;
            int num8 = ctrl.Value;
            if ((ctrl.RightToLeft & RightToLeft.Yes) == RightToLeft.Yes)
            {
                num8 = ctrl.Maximum - ctrl.Value;
            }
            if (ctrl.Orientation == Orientation.Horizontal)
            {
                width = ctrl.Width;
                height = ctrl.Height;
            }
            else
            {
                width = ctrl.Height;
                height = ctrl.Width;
            }
            Bitmap image = new Bitmap(width, height);
            Rectangle rect = new Rectangle(0, 0, width, height);
            Graphics graphics = Graphics.FromImage(image);
            Brush brush = base.Engine.Res.Brushes.SKIN2_FORMCOLOR;
            graphics.FillRectangle(brush, rect);
            Bitmap bitmap = base.Engine.Res.Bitmaps.SKIN2_TRACKBAR;
            switch (ctrl.TickStyle)
            {
                case TickStyle.None:
                    num7 = num4 / 2;
                    break;

                case TickStyle.TopLeft:
                    num7 = num + (num4 / 2);
                    break;

                case TickStyle.BottomRight:
                    num7 = num4 / 2;
                    break;

                default:
                    num7 = num + (num4 / 2);
                    break;
            }
            rect = new Rectangle(x, num7, width - (2 * x), bitmap.Height);
            Rectangle destRect = new Rectangle(rect.Left, rect.Top, bitmap.Width / 3, bitmap.Height);
            Rectangle srcRect = new Rectangle(0, 0, bitmap.Width / 3, bitmap.Height);
            graphics.DrawImage(bitmap, destRect, srcRect, GraphicsUnit.Pixel);
            destRect = Rectangle.FromLTRB(rect.Right - (bitmap.Width / 3), rect.Top, rect.Right, rect.Bottom);
            srcRect = new Rectangle(bitmap.Width - (bitmap.Width / 3), 0, bitmap.Width / 3, bitmap.Height);
            graphics.DrawImage(bitmap, destRect, srcRect, GraphicsUnit.Pixel);
            destRect = Rectangle.FromLTRB(rect.Left + (bitmap.Width / 3), rect.Top, rect.Right - (bitmap.Width / 3), rect.Bottom);
            srcRect = Rectangle.FromLTRB(bitmap.Width / 3, 0, bitmap.Width - (bitmap.Width / 3), bitmap.Height);
            graphics.DrawImage(bitmap, destRect, srcRect, GraphicsUnit.Pixel);
            switch (ctrl.TickStyle)
            {
                case TickStyle.None:
                    bitmap = base.Engine.Res.Bitmaps.SKIN2_TRACKBARSLIDER;
                    num7 = 0;
                    break;

                case TickStyle.TopLeft:
                    bitmap = base.Engine.Res.Bitmaps.SKIN2_TRACKBARSLIDER_180;
                    num7 = num;
                    break;

                case TickStyle.BottomRight:
                    bitmap = base.Engine.Res.Bitmaps.SKIN2_TRACKBARSLIDER;
                    num7 = 0;
                    break;

                default:
                    bitmap = base.Engine.Res.Bitmaps.SKIN2_TRACKBARSLIDER;
                    num7 = num;
                    break;
            }
            if (ctrl.Maximum == ctrl.Minimum)
            {
                goto Label_04D0;
            }
            destRect = new Rectangle(((((int) ((((width - (2 * x)) - (2 * num3)) * (num8 - ctrl.Minimum)) / ((float) (ctrl.Maximum - ctrl.Minimum)))) - (bitmap.Width / 2)) + x) + num3, num7, bitmap.Width, bitmap.Height);
            srcRect = new Rectangle(0, 0, bitmap.Width, bitmap.Height);
            graphics.DrawImage(bitmap, destRect, srcRect, GraphicsUnit.Pixel);
            Pen pen = new Pen(Color.Black, 1f);
            float num9 = x + num3;
            float num10 = (width - x) - num3;
            float num11 = ((num10 - num9) * ctrl.TickFrequency) / ((float) (ctrl.Maximum - ctrl.Minimum));
            if (num11 > 0f)
            {
                switch (ctrl.TickStyle)
                {
                    case TickStyle.None:
                        goto Label_04C9;

                    case TickStyle.TopLeft:
                        num7 = num - 4;
                        if (ctrl.Orientation != Orientation.Vertical)
                        {
                            this.x86a0b6ee5f4b48be(graphics, pen, (float) num7, (float) (num7 + 3), num9, num10, num11);
                        }
                        else
                        {
                            this.xa5b17a677db5a5eb(graphics, pen, (float) num7, (float) (num7 + 3), num9, num10, num11);
                        }
                        goto Label_04C9;

                    case TickStyle.BottomRight:
                        num7 = num4 + 7;
                        if (ctrl.Orientation != Orientation.Vertical)
                        {
                            this.x86a0b6ee5f4b48be(graphics, pen, (float) num7, (float) (num7 + 3), num9, num10, num11);
                        }
                        else
                        {
                            this.xa5b17a677db5a5eb(graphics, pen, (float) num7, (float) (num7 + 3), num9, num10, num11);
                        }
                        goto Label_04C9;
                }
                num7 = num - 4;
                if (ctrl.Orientation == Orientation.Vertical)
                {
                    this.xa5b17a677db5a5eb(graphics, pen, (float) num7, (float) (num7 + 3), num9, num10, num11);
                }
                else
                {
                    this.x86a0b6ee5f4b48be(graphics, pen, (float) num7, (float) (num7 + 3), num9, num10, num11);
                }
                num7 = (num + num4) + 7;
                if (ctrl.Orientation == Orientation.Vertical)
                {
                    this.xa5b17a677db5a5eb(graphics, pen, (float) num7, (float) (num7 + 3), num9, num10, num11);
                }
                else
                {
                    this.x86a0b6ee5f4b48be(graphics, pen, (float) num7, (float) (num7 + 3), num9, num10, num11);
                }
            }
        Label_04C9:
            pen.Dispose();
        Label_04D0:
            if (ctrl.Focused)
            {
                rect = Rectangle.FromLTRB(ctrl.ClientRectangle.X + 5, ctrl.ClientRectangle.Y, ctrl.ClientRectangle.Right - 5, ctrl.ClientRectangle.Bottom - 5);
                graphics.DrawRectangle(x448fd9ab43628c71.FocusRectanglePen, rect);
            }
            if (ctrl.Orientation == Orientation.Vertical)
            {
                image.RotateFlip(RotateFlipType.Rotate270FlipNone);
            }
            x4b101060f4767186.DrawImage(image, 0, 0);
            graphics.Dispose();
            image.Dispose();
        }

        private void xa5b17a677db5a5eb(Graphics x4b101060f4767186, Pen x90279591611601bc, float xc941868c59399d3e, float xaf9a0436a70689de, float x10aaa7cdfa38f254, float xca09b6c2b5b18485, float x0b5a48bb98ab819f)
        {
            while (xca09b6c2b5b18485 > x10aaa7cdfa38f254)
            {
                x4b101060f4767186.DrawLine(x90279591611601bc, xca09b6c2b5b18485, xc941868c59399d3e, xca09b6c2b5b18485, xaf9a0436a70689de);
                xca09b6c2b5b18485 -= x0b5a48bb98ab819f;
                if (xca09b6c2b5b18485 <= x10aaa7cdfa38f254)
                {
                    x4b101060f4767186.DrawLine(x90279591611601bc, x10aaa7cdfa38f254, xc941868c59399d3e, x10aaa7cdfa38f254, xaf9a0436a70689de);
                }
            }
        }

        private void xf51466e2b7549167(object xe0292b9ed559da7d, EventArgs xfbf34718e704c6bc)
        {
            this.PaintControl();
        }

        private bool x0e7ffd184973f9bc
        {
            get
            {
                TrackBar ctrl = (TrackBar) base.Ctrl;
                return (ctrl.RightToLeftLayout && ((ctrl.RightToLeft & RightToLeft.Yes) == RightToLeft.Yes));
            }
        }
    }
}

