namespace Sunisoft.IrisSkin
{
    using System;
    using System.Drawing;
    using System.Windows.Forms;

    internal class x0e2bf2da454c077f : xbd3f2493841f18a1
    {
        public x0e2bf2da454c077f(Control control, SkinEngine engine) : base(control, engine)
        {
        }

        protected override void PaintControl()
        {
            if (((base.Ctrl.Width > 0) && (base.Ctrl.Height > 0)) && base.CanPaint)
            {
                base.PaintControl();
                IntPtr windowDC = x61467fe65a98f20c.GetWindowDC(base.Ctrl.Handle);
                using (Graphics graphics = Graphics.FromHdc(windowDC))
                {
                    using (Bitmap bitmap = new Bitmap(base.Ctrl.Width, base.Ctrl.Height))
                    {
                        using (Graphics graphics2 = Graphics.FromImage(bitmap))
                        {
                            this.x8bc95f030953f87b(graphics2);
                            graphics.DrawImageUnscaled(bitmap, 0, 0);
                        }
                    }
                    x61467fe65a98f20c.ReleaseDC(base.Ctrl.Handle, windowDC);
                }
            }
        }

        private void x8bc95f030953f87b(Graphics x4b101060f4767186)
        {
            if (this.PaintBorder)
            {
                Pen pen;
                Brush brush = base.Engine.Res.Brushes.SKIN2_CONTROLBORDERCOLOR;
                Rectangle lpRect = new Rectangle(0, 0, 0, 0);
                using (pen = new Pen(brush, 1f))
                {
                    x61467fe65a98f20c.GetWindowRect(base.Ctrl.Handle, ref lpRect);
                    lpRect.Width -= lpRect.X + 1;
                    lpRect.Height -= lpRect.Y + 1;
                    lpRect.X = 0;
                    lpRect.Y = 0;
                    x4b101060f4767186.DrawRectangle(pen, lpRect);
                }
                if (this.BorderWidth > 0)
                {
                    if (this.EnableState)
                    {
                        pen = new Pen(base.Ctrl.BackColor, (float) this.BorderWidth);
                    }
                    else
                    {
                        pen = new Pen(Color.FromKnownColor(KnownColor.Control), (float) this.BorderWidth);
                    }
                    lpRect.X += this.BorderWidth;
                    lpRect.Y += this.BorderWidth;
                    lpRect.Width -= this.BorderWidth + 1;
                    lpRect.Height -= this.BorderWidth + 1;
                    x4b101060f4767186.DrawLine(pen, lpRect.Left, 1, lpRect.Left, lpRect.Bottom);
                    x4b101060f4767186.DrawLine(pen, lpRect.Left, lpRect.Top, lpRect.Right, lpRect.Top);
                    pen.Dispose();
                }
            }
        }

        protected int BorderWidth
        {
            get
            {
                return 1;
            }
        }

        protected override bool ChangeBackColor
        {
            get
            {
                return false;
            }
        }

        protected virtual bool EnableState
        {
            get
            {
                return base.Ctrl.Enabled;
            }
        }

        protected virtual bool PaintBorder
        {
            get
            {
                return true;
            }
        }
    }
}

