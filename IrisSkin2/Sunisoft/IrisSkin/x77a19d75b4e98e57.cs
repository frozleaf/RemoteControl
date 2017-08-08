namespace Sunisoft.IrisSkin
{
    using System;
    using System.Drawing;
    using System.Drawing.Text;
    using System.Windows.Forms;

    internal class x77a19d75b4e98e57 : xbd3f2493841f18a1
    {
        private const int xfa432a34de1585b8 = 6;

        public x77a19d75b4e98e57(Control control, SkinEngine engine) : base(control, engine)
        {
        }

        protected override void PaintControl()
        {
            if (((base.Ctrl.ClientRectangle.Width > 0) && (base.Ctrl.ClientRectangle.Height > 0)) && base.CanPaint)
            {
                using (Graphics graphics = Graphics.FromHwnd(base.Ctrl.Handle))
                {
                    this.x8bc95f030953f87b(graphics);
                }
            }
        }

        private void x8bc95f030953f87b(Graphics x4b101060f4767186)
        {
            GroupBox ctrl = (GroupBox) base.Ctrl;
            StringFormat format = new StringFormat {
                Alignment = StringAlignment.Near,
                LineAlignment = StringAlignment.Near,
                HotkeyPrefix = HotkeyPrefix.Show
            };
            Rectangle lpRect = new Rectangle(0, 0, 0, 0);
            x61467fe65a98f20c.GetWindowRect(base.Ctrl.Handle, ref lpRect);
            lpRect.Width -= lpRect.X;
            lpRect.Height -= lpRect.Y;
            lpRect.X = 0;
            lpRect.Y = 0;
            Brush brush = base.Engine.Res.Brushes.SKIN2_FORMCOLOR;
            Brush brush2 = base.Engine.Res.Brushes.SKIN2_CONTROLFONTCOLOR;
            x4b101060f4767186.FillRectangle(brush, lpRect);
            using (Pen pen = new Pen(base.Engine.Res.Brushes.SKIN2_CONTROLBORDERCOLOR, 1f))
            {
                if ((ctrl.RightToLeft & RightToLeft.Yes) == RightToLeft.Yes)
                {
                    format.Alignment = StringAlignment.Far;
                    format.FormatFlags |= StringFormatFlags.DirectionRightToLeft;
                }
                else
                {
                    format.Alignment = StringAlignment.Near;
                }
                SizeF ef = x4b101060f4767186.MeasureString(ctrl.Text, ctrl.Font, (int) (ctrl.Width - 12), format);
                if ((ctrl.RightToLeft & RightToLeft.Yes) == RightToLeft.Yes)
                {
                    x4b101060f4767186.DrawLine(pen, (float) lpRect.X, (float) (lpRect.Y + 6), (lpRect.Right - 6) - ef.Width, (float) (lpRect.Y + 6));
                    x4b101060f4767186.DrawLine(pen, (int) ((lpRect.Right - 6) - 1), (int) (lpRect.Y + 6), (int) (lpRect.Width - 1), (int) (lpRect.Y + 6));
                    x4b101060f4767186.DrawString(ctrl.Text, ctrl.Font, brush2, new RectangleF((lpRect.Right - 6) - ef.Width, (float) lpRect.Top, ef.Width, ef.Height), format);
                }
                else
                {
                    x4b101060f4767186.DrawLine(pen, lpRect.X, lpRect.Y + 6, (lpRect.X + 6) + 1, lpRect.Y + 6);
                    x4b101060f4767186.DrawLine(pen, (lpRect.Left + 6) + ef.Width, (float) (lpRect.Y + 6), (float) (lpRect.Width - 1), (float) (lpRect.Y + 6));
                    x4b101060f4767186.DrawString(ctrl.Text, ctrl.Font, brush2, new RectangleF((float) ((lpRect.X + 6) + 1), (float) lpRect.Top, ef.Width, ef.Height), format);
                }
                x4b101060f4767186.DrawLine(pen, lpRect.X, lpRect.Y + 6, lpRect.X, lpRect.Bottom - 1);
                x4b101060f4767186.DrawLine(pen, lpRect.X, lpRect.Bottom - 1, lpRect.Right - 1, lpRect.Bottom - 1);
                x4b101060f4767186.DrawLine(pen, (int) (lpRect.Right - 1), (int) (lpRect.Y + 6), (int) (lpRect.Right - 1), (int) (lpRect.Bottom - 1));
            }
        }

        protected override bool ChangeFontColor
        {
            get
            {
                return true;
            }
        }
    }
}

