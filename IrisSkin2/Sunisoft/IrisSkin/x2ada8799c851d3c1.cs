namespace Sunisoft.IrisSkin
{
    using System;
    using System.Drawing;
    using System.Runtime.InteropServices;
    using System.Windows.Forms;

    internal class x2ada8799c851d3c1 : x2edc3f693fe78d2e
    {
        private int x01b557925841ae51;
        private Control x256c9720723a7c14;

        public x2ada8799c851d3c1(Control control, SkinEngine engine) : base(control, engine)
        {
        }

        protected override void DoInit()
        {
            base.DoInit();
            if (base.Ctrl.Controls.Count > 0)
            {
                this.x256c9720723a7c14 = base.Ctrl.Controls[0];
                this.x256c9720723a7c14.Paint += new PaintEventHandler(this.x36ba227d26d9f4d3);
            }
            base.Ctrl.MouseDown += new MouseEventHandler(this.x2c376bc5808866ab);
            base.Ctrl.MouseUp += new MouseEventHandler(this.x311c106b70013575);
        }

        private void x2c376bc5808866ab(object xe0292b9ed559da7d, MouseEventArgs xfbf34718e704c6bc)
        {
            if (xfbf34718e704c6bc.Button == MouseButtons.Left)
            {
                Rectangle rectangle;
                Rectangle rectangle2;
                this.x9c2d0a8c2d93d7c6(out rectangle, out rectangle2);
                Point p = new Point(xfbf34718e704c6bc.X, xfbf34718e704c6bc.Y);
                if (x448fd9ab43628c71.InRect(p, rectangle))
                {
                    this.x01b557925841ae51 = -1;
                }
                else if (x448fd9ab43628c71.InRect(p, rectangle2))
                {
                    this.x01b557925841ae51 = 1;
                }
                else
                {
                    this.x01b557925841ae51 = 0;
                }
            }
        }

        private void x311c106b70013575(object xe0292b9ed559da7d, MouseEventArgs xfbf34718e704c6bc)
        {
            this.x01b557925841ae51 = 0;
        }

        private void x36ba227d26d9f4d3(object xe0292b9ed559da7d, PaintEventArgs xfbf34718e704c6bc)
        {
            if (base.CanPaint)
            {
                Brush brush;
                Graphics graphics = xfbf34718e704c6bc.Graphics;
                int width = this.x256c9720723a7c14.Width;
                int height = this.x256c9720723a7c14.Height;
                Rectangle rect = new Rectangle(0, 0, width, height);
                Bitmap image = new Bitmap(width, height);
                Rectangle rectangle2 = new Rectangle(0, 0, this.x256c9720723a7c14.Width, this.x256c9720723a7c14.Height / 2);
                Rectangle rectangle3 = new Rectangle(0, this.x256c9720723a7c14.Height / 2, this.x256c9720723a7c14.Width, this.x256c9720723a7c14.Height / 2);
                Graphics g = Graphics.FromImage(image);
                if (this.x01b557925841ae51 == -1)
                {
                    brush = base.Engine.Res.Brushes.SKIN2_TOOLBARDOWNCOLOR;
                }
                else
                {
                    brush = base.Engine.Res.Brushes.SKIN2_FORMCOLOR;
                }
                g.FillRectangle(brush, rectangle2);
                if (this.x01b557925841ae51 == 1)
                {
                    brush = base.Engine.Res.Brushes.SKIN2_TOOLBARDOWNCOLOR;
                }
                else
                {
                    brush = base.Engine.Res.Brushes.SKIN2_FORMCOLOR;
                }
                g.FillRectangle(brush, rectangle3);
                int x = (((rectangle2.Right - rectangle2.Left) - 6) / 2) + rectangle2.Left;
                int y = (((rectangle2.Bottom - rectangle2.Top) + 3) / 2) + rectangle2.Top;
                if (this.x01b557925841ae51 == -1)
                {
                    x++;
                    y++;
                }
                x448fd9ab43628c71.DrawArrowUp(g, x, y, base.Ctrl.Enabled);
                x = (((rectangle3.Right - rectangle3.Left) - 6) / 2) + rectangle3.Left;
                y = (((rectangle3.Bottom - rectangle3.Top) - 3) / 2) + rectangle3.Top;
                if (this.x01b557925841ae51 == 1)
                {
                    x++;
                    y++;
                }
                x448fd9ab43628c71.DrawArrowDown(g, x, y, base.Ctrl.Enabled);
                graphics.DrawImageUnscaled(image, rect);
                g.Dispose();
                image.Dispose();
            }
        }

        private void x6e1c0299f4a4ec84(object xe0292b9ed559da7d, MouseEventArgs xfbf34718e704c6bc)
        {
            this.x01b557925841ae51 = 0;
        }

        private void x9c2d0a8c2d93d7c6(out Rectangle x704d5d75fd559aed, out Rectangle x68123086ccad0951)
        {
            int x = 0;
            x704d5d75fd559aed = new Rectangle(x, 0, this.x256c9720723a7c14.Width, this.x256c9720723a7c14.Height / 2);
            x68123086ccad0951 = new Rectangle(x, this.x256c9720723a7c14.Height / 2, this.x256c9720723a7c14.Width, this.x256c9720723a7c14.Height / 2);
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

