namespace Sunisoft.IrisSkin
{
    using System;
    using System.Drawing;
    using System.Windows.Forms;

    internal class x60982e1b4eef090a : Control
    {
        private ScrollBar x246c0c54671f3f3e;
        private bool x8875b8c88ca272fe;
        private SkinEngine xdc87e2b99332cd4a;

        public x60982e1b4eef090a(ScrollBar host, SkinEngine engine, bool hScroll)
        {
            this.x246c0c54671f3f3e = host;
            this.xdc87e2b99332cd4a = engine;
            this.x8875b8c88ca272fe = hScroll;
            if (host.Parent != null)
            {
                if (host.Parent is TableLayoutPanel)
                {
                    if (host.Parent.Parent != null)
                    {
                        host.Parent.Parent.SuspendLayout();
                        host.Parent.Parent.Controls.Add(this);
                        host.Parent.Parent.ResumeLayout();
                    }
                }
                else
                {
                    host.Parent.SuspendLayout();
                    host.Parent.Controls.Add(this);
                    host.Parent.ResumeLayout();
                }
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            this.x8bc95f030953f87b(e.Graphics);
        }

        public void Repaint()
        {
            using (Graphics graphics = Graphics.FromHwnd(base.Handle))
            {
                this.x8bc95f030953f87b(graphics);
            }
        }

        protected override void WndProc(ref Message m)
        {
            if (this.x246c0c54671f3f3e != null)
            {
                switch (((uint) m.Msg))
                {
                    case 0x200:
                    case 0x201:
                    case 0x204:
                    case 0x20a:
                        x61467fe65a98f20c.PostMessage(this.x246c0c54671f3f3e.Handle, (uint) m.Msg, m.WParam, m.LParam);
                        return;
                }
            }
            base.WndProc(ref m);
        }

        private void x3a329aff8c7e9df4(Graphics x4b101060f4767186, ScrollBar x2ee8392f53a01b93, Rectangle x382bb49d8f914534)
        {
            Brush brush = this.xdc87e2b99332cd4a.Res.Brushes.SKIN2_SCROLLBARCOLOR;
            x4b101060f4767186.FillRectangle(brush, x382bb49d8f914534);
            int height = 0;
            int num2 = 0;
            height = Math.Min(0x10, base.Height / 2);
            Bitmap image = this.xdc87e2b99332cd4a.Res.ScrollBarRes.UpButton[0, num2];
            Rectangle srcRect = new Rectangle(0, 0, image.Width, image.Height);
            Rectangle destRect = new Rectangle(x382bb49d8f914534.X, x382bb49d8f914534.Y, x382bb49d8f914534.Width, height);
            x4b101060f4767186.DrawImage(image, destRect, srcRect, GraphicsUnit.Pixel);
            image = this.xdc87e2b99332cd4a.Res.ScrollBarRes.DownButton[0, num2];
            srcRect = new Rectangle(0, 0, image.Width, image.Height);
            destRect = new Rectangle(x382bb49d8f914534.X, x382bb49d8f914534.Bottom - height, x382bb49d8f914534.Width, height);
            x4b101060f4767186.DrawImage(image, destRect, srcRect, GraphicsUnit.Pixel);
            int num3 = x2ee8392f53a01b93.Value;
            int largeChange = x2ee8392f53a01b93.LargeChange;
            if (largeChange == 0)
            {
                largeChange = 1;
            }
            int num5 = x382bb49d8f914534.Height - (height * 2);
            int num6 = (num5 * largeChange) / ((x2ee8392f53a01b93.Maximum - x2ee8392f53a01b93.Minimum) + 1);
            if ((num6 < 10) && (num5 >= 10))
            {
                num6 = 10;
            }
            int num7 = 0;
            if (largeChange == ((x2ee8392f53a01b93.Maximum - x2ee8392f53a01b93.Minimum) + 1))
            {
                num7 = 0;
            }
            else
            {
                num7 = ((num5 - num6) * (num3 - x2ee8392f53a01b93.Minimum)) / (((x2ee8392f53a01b93.Maximum - x2ee8392f53a01b93.Minimum) + 1) - largeChange);
            }
            if (num7 < 0)
            {
                num7 = 0;
            }
            if (num7 > (num5 - num6))
            {
                num7 = num5 - num6;
            }
            if (num5 >= 10)
            {
                destRect = new Rectangle(x382bb49d8f914534.Left, (x382bb49d8f914534.Top + height) + num7, x382bb49d8f914534.Width, num6);
                x448fd9ab43628c71.PaintSlider(this.xdc87e2b99332cd4a, x4b101060f4767186, destRect, true);
            }
        }

        private void x8bc95f030953f87b(Graphics x4b101060f4767186)
        {
            if ((base.ClientRectangle.Width > 0) && (base.ClientRectangle.Height > 0))
            {
                if (this.x8875b8c88ca272fe)
                {
                    using (Bitmap bitmap = new Bitmap(base.Width, base.Height))
                    {
                        using (Graphics graphics = Graphics.FromImage(bitmap))
                        {
                            this.xb16feaabcbd204bd(graphics, this.x246c0c54671f3f3e, base.ClientRectangle);
                        }
                        x4b101060f4767186.DrawImageUnscaled(bitmap, 0, 0);
                        return;
                    }
                }
                using (Bitmap bitmap2 = new Bitmap(base.Width, base.Height))
                {
                    using (Graphics.FromImage(bitmap2))
                    {
                        this.x3a329aff8c7e9df4(x4b101060f4767186, this.x246c0c54671f3f3e, base.ClientRectangle);
                    }
                    x4b101060f4767186.DrawImageUnscaled(bitmap2, 0, 0);
                }
            }
        }

        private void xb16feaabcbd204bd(Graphics x4b101060f4767186, ScrollBar x2ee8392f53a01b93, Rectangle x382bb49d8f914534)
        {
            Brush backColorBrush = this.xdc87e2b99332cd4a.Res.ScrollBarRes.BackColorBrush;
            x4b101060f4767186.FillRectangle(backColorBrush, x382bb49d8f914534);
            int width = 0;
            int num2 = 0;
            width = Math.Min(0x10, base.Width / 2);
            Bitmap image = this.xdc87e2b99332cd4a.Res.ScrollBarRes.UpButton[1, num2];
            Rectangle srcRect = new Rectangle(0, 0, image.Width, image.Height);
            Rectangle destRect = new Rectangle(x382bb49d8f914534.X, x382bb49d8f914534.Y, width, x382bb49d8f914534.Height);
            x4b101060f4767186.DrawImage(image, destRect, srcRect, GraphicsUnit.Pixel);
            image = this.xdc87e2b99332cd4a.Res.ScrollBarRes.DownButton[1, num2];
            srcRect = new Rectangle(0, 0, image.Width, image.Height);
            destRect = new Rectangle(x382bb49d8f914534.Right - width, x382bb49d8f914534.Y, width, x382bb49d8f914534.Height);
            x4b101060f4767186.DrawImage(image, destRect, srcRect, GraphicsUnit.Pixel);
            int num3 = x2ee8392f53a01b93.Value;
            int largeChange = x2ee8392f53a01b93.LargeChange;
            if (largeChange == 0)
            {
                largeChange = 1;
            }
            int num5 = x382bb49d8f914534.Width - (width * 2);
            int num6 = (num5 * largeChange) / ((x2ee8392f53a01b93.Maximum - x2ee8392f53a01b93.Minimum) + 1);
            if ((num6 < 10) && (num5 >= 10))
            {
                num6 = 10;
            }
            int num7 = 0;
            if (largeChange == ((x2ee8392f53a01b93.Maximum - x2ee8392f53a01b93.Minimum) + 1))
            {
                num7 = 0;
            }
            else
            {
                num7 = ((num5 - num6) * (num3 - x2ee8392f53a01b93.Minimum)) / (((x2ee8392f53a01b93.Maximum - x2ee8392f53a01b93.Minimum) + 1) - largeChange);
            }
            if (num7 < 0)
            {
                num7 = 0;
            }
            if (num7 > (num5 - num6))
            {
                num7 = num5 - num6;
            }
            if (num5 >= 10)
            {
                if ((this.x246c0c54671f3f3e.RightToLeft & RightToLeft.Yes) == RightToLeft.Yes)
                {
                    destRect = new Rectangle(((x382bb49d8f914534.Right - width) - num7) - num6, x382bb49d8f914534.Top, num6, x382bb49d8f914534.Height);
                }
                else
                {
                    destRect = new Rectangle((x382bb49d8f914534.Left + width) + num7, x382bb49d8f914534.Top, num6, x382bb49d8f914534.Height);
                }
                x448fd9ab43628c71.PaintSlider(this.xdc87e2b99332cd4a, x4b101060f4767186, destRect, false);
            }
        }
    }
}

