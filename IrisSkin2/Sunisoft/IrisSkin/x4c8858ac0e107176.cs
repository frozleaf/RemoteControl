namespace Sunisoft.IrisSkin
{
    using System;
    using System.Drawing;

    internal class x4c8858ac0e107176 : xf7df44b87349eabf
    {
        public x4c8858ac0e107176(IntPtr handle, SkinEngine engine) : base(handle, engine)
        {
        }

        protected override void PaintControl()
        {
            base.PaintControl();
            xae4dd1cafd2eb77c.RECT rcButton = new xae4dd1cafd2eb77c.RECT();
            x40255b11ef821fa3.COMBOBOXINFO cbi = new x40255b11ef821fa3.COMBOBOXINFO {
                cbSize = 0x34
            };
            if (x61467fe65a98f20c.GetComboBoxInfo(base.Handle, ref cbi))
            {
                IntPtr windowDC = x61467fe65a98f20c.GetWindowDC(base.Handle);
                using (Graphics graphics = Graphics.FromHwnd(base.Handle))
                {
                    rcButton = cbi.rcButton;
                    Rectangle rect = new Rectangle(rcButton.left, rcButton.top, rcButton.right - rcButton.left, rcButton.bottom - rcButton.top);
                    Brush brush = base.Engine.Res.Brushes.SKIN2_FORMCOLOR;
                    graphics.FillRectangle(brush, rect);
                    using (Pen pen = new Pen(base.Engine.Res.Brushes.SKIN2_CONTROLBORDERCOLOR, 1f))
                    {
                        graphics.DrawLine(pen, rect.Left, rect.Top - 1, rect.Left, rect.Bottom + 1);
                    }
                    int x = ((rect.Width / 2) + rect.Left) - 3;
                    int y = rect.Height / 2;
                    x448fd9ab43628c71.DrawArrowDown(graphics, x, y, this.Enabled);
                }
                x61467fe65a98f20c.ReleaseDC(base.Handle, windowDC);
            }
        }
    }
}

