namespace Sunisoft.IrisSkin
{
    using System;
    using System.Drawing;
    using System.Windows.Forms;

    internal class x6a7aef305e9ce97d : x2edc3f693fe78d2e
    {
        private const int xc2b2889221bc8c5e = 0x15;

        public x6a7aef305e9ce97d(Control control, SkinEngine engine) : base(control, engine)
        {
        }

        protected override void PaintControl()
        {
            if (((base.Ctrl.ClientRectangle.Width > 0) && (base.Ctrl.ClientRectangle.Height > 0)) && base.CanPaint)
            {
                base.PaintControl();
                DateTimePicker ctrl = (DateTimePicker) base.Ctrl;
                if (!ctrl.ShowUpDown)
                {
                    using (Bitmap bitmap = new Bitmap(base.Ctrl.Width, base.Ctrl.Height))
                    {
                        using (Graphics graphics = Graphics.FromImage(bitmap))
                        {
                            this.x8bc95f030953f87b(graphics);
                        }
                        IntPtr windowDC = x61467fe65a98f20c.GetWindowDC(base.Ctrl.Handle);
                        using (Graphics graphics2 = Graphics.FromHdc(windowDC))
                        {
                            graphics2.DrawImageUnscaled(bitmap, 0, 0);
                        }
                        x61467fe65a98f20c.ReleaseDC(base.Ctrl.Handle, windowDC);
                    }
                }
            }
        }

        private void x373a1ceeebad6a15(object xe0292b9ed559da7d, EventArgs xfbf34718e704c6bc)
        {
            this.PaintControl();
        }

        private void x8bc95f030953f87b(Graphics x4b101060f4767186)
        {
            DateTimePicker ctrl = (DateTimePicker) base.Ctrl;
            Rectangle rect = new Rectangle(ctrl.Width - 0x15, 0, 0x15, ctrl.Height);
            Brush brush = base.Engine.Res.Brushes.SKIN2_FORMCOLOR;
            rect.X += 2;
            rect.Y += 2;
            rect.Width -= 5;
            rect.Height -= 5;
            x4b101060f4767186.FillRectangle(brush, rect);
            using (Pen pen = new Pen(base.Engine.Res.Brushes.SKIN2_CONTROLBORDERCOLOR, 1f))
            {
                x4b101060f4767186.DrawRectangle(pen, rect);
            }
            x448fd9ab43628c71.DrawArrowDown(x4b101060f4767186, (rect.X + 10) - 5, (rect.Y + (ctrl.Height / 2)) - 3, ctrl.Enabled);
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

