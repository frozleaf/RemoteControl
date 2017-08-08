namespace Sunisoft.IrisSkin
{
    using System;
    using System.Drawing;
    using System.Windows.Forms;

    internal class x896bb4cf7b29301a : xbd3f2493841f18a1
    {
        public x896bb4cf7b29301a(Control control, SkinEngine engine) : base(control, engine)
        {
        }

        protected override void DoInit()
        {
            base.DoInit();
            CheckBox ctrl = (CheckBox) base.Ctrl;
            ctrl.CheckedChanged += new EventHandler(this.x1dba4f67a1b5a3fb);
            ctrl.Click += new EventHandler(this.xf0204c2f26f9917d);
            ctrl.Paint += new PaintEventHandler(this.x6d90c2026f9061d8);
        }

        protected override void PaintControl()
        {
            if ((base.Ctrl.ClientRectangle.Width > 0) && (base.Ctrl.ClientRectangle.Height > 0))
            {
                Graphics graphics = Graphics.FromHwnd(base.Ctrl.Handle);
                this.x8bc95f030953f87b(graphics);
                graphics.Dispose();
            }
        }

        private void x1dba4f67a1b5a3fb(object xe0292b9ed559da7d, EventArgs xfbf34718e704c6bc)
        {
            if (base.CanPaint)
            {
                this.PaintControl();
            }
        }

        private void x6d90c2026f9061d8(object xe0292b9ed559da7d, PaintEventArgs xfbf34718e704c6bc)
        {
            if (base.CanPaint)
            {
                this.x8bc95f030953f87b(xfbf34718e704c6bc.Graphics);
            }
        }

        private void x6f37a3496a324cf4(Graphics x4b101060f4767186, int x43abf768b4ac7ff4)
        {
            Rectangle rectangle;
            StringFormat format;
            Brush gray;
            CheckBox ctrl = (CheckBox) base.Ctrl;
            x448fd9ab43628c71.CalcLayoutRect(ctrl.TextAlign, ctrl.ClientRectangle, ctrl.RightToLeft, out rectangle, out format);
            if ((ctrl.RightToLeft & RightToLeft.Yes) != RightToLeft.Yes)
            {
                switch (ctrl.CheckAlign)
                {
                    case ContentAlignment.TopCenter:
                    case ContentAlignment.MiddleCenter:
                    case ContentAlignment.BottomCenter:
                        goto Label_0163;

                    case ContentAlignment.TopRight:
                    case ContentAlignment.MiddleRight:
                    case ContentAlignment.BottomRight:
                        rectangle.Width -= x43abf768b4ac7ff4;
                        goto Label_0163;
                }
            }
            else
            {
                switch (ctrl.CheckAlign)
                {
                    case ContentAlignment.TopCenter:
                    case ContentAlignment.MiddleCenter:
                    case ContentAlignment.BottomCenter:
                        goto Label_0163;

                    case ContentAlignment.TopRight:
                    case ContentAlignment.MiddleRight:
                    case ContentAlignment.BottomRight:
                        rectangle.X += x43abf768b4ac7ff4;
                        rectangle.Width -= x43abf768b4ac7ff4;
                        goto Label_0163;
                }
                rectangle.Width -= x43abf768b4ac7ff4;
                goto Label_0163;
            }
            rectangle.X += x43abf768b4ac7ff4;
            rectangle.Width -= x43abf768b4ac7ff4;
        Label_0163:
            if (base.ctrlMouseState == xb9506a535e31f22a.MouseDown)
            {
                x448fd9ab43628c71.OffsetRect(ref rectangle, 1, 1);
            }
            if (base.Ctrl.Enabled)
            {
                gray = new SolidBrush(base.Ctrl.ForeColor);
            }
            else
            {
                gray = Brushes.Gray;
            }
            x4b101060f4767186.DrawString(ctrl.Text, ctrl.Font, gray, rectangle, format);
            if (ctrl.Focused)
            {
                x4b101060f4767186.DrawRectangle(x448fd9ab43628c71.FocusRectanglePen, rectangle.X, rectangle.Y, rectangle.Width - 1, rectangle.Height - 1);
            }
            if (base.Ctrl.Enabled && (gray != null))
            {
                gray.Dispose();
            }
        }

        private void x8bc95f030953f87b(Graphics x4b101060f4767186)
        {
            CheckBox ctrl = (CheckBox) base.Ctrl;
            int index = 0;
            if (ctrl.CheckState == CheckState.Checked)
            {
                if (ctrl.Enabled)
                {
                    index = 1;
                }
                else
                {
                    index = 4;
                }
            }
            else if (ctrl.CheckState == CheckState.Indeterminate)
            {
                if (ctrl.Enabled)
                {
                    index = 2;
                }
                else
                {
                    index = 5;
                }
            }
            else if (ctrl.Enabled)
            {
                index = 3;
            }
            else
            {
                index = 6;
            }
            Bitmap image = base.Engine.Res.SplitBitmaps.SKIN2_CHECKBOX[index];
            int num2 = Math.Max(image.Width, 13);
            int height = Math.Max(image.Height, 13);
            num2 = Math.Min(num2, 14);
            height = Math.Min(num2, 14);
            using (Brush brush = new SolidBrush(ctrl.BackColor))
            {
                Rectangle destRect = x448fd9ab43628c71.CalcLayoutRect(ctrl.CheckAlign, ctrl.RightToLeft, ctrl.ClientRectangle, num2, height, 0, 0);
                x4b101060f4767186.FillRectangle(brush, ctrl.ClientRectangle);
                Rectangle srcRect = new Rectangle(0, 0, image.Width, image.Height);
                x4b101060f4767186.DrawImage(image, destRect, srcRect, GraphicsUnit.Pixel);
                if (ctrl.Image != null)
                {
                    this.xcea9a0eaf0e83f21(x4b101060f4767186, num2);
                }
                this.x6f37a3496a324cf4(x4b101060f4767186, num2);
            }
        }

        private void xcea9a0eaf0e83f21(Graphics x4b101060f4767186, int x43abf768b4ac7ff4)
        {
            CheckBox ctrl = (CheckBox) base.Ctrl;
            Image image = ctrl.Image;
            if ((image.Height > 0) && (image.Width > 0))
            {
                Rectangle clientRectangle = ctrl.ClientRectangle;
                switch (ctrl.CheckAlign)
                {
                    case ContentAlignment.TopCenter:
                    case ContentAlignment.MiddleCenter:
                    case ContentAlignment.BottomCenter:
                        break;

                    case ContentAlignment.TopRight:
                    case ContentAlignment.MiddleRight:
                    case ContentAlignment.BottomRight:
                        clientRectangle.Width -= x43abf768b4ac7ff4;
                        break;

                    default:
                        clientRectangle.X += x43abf768b4ac7ff4;
                        clientRectangle.Width -= x43abf768b4ac7ff4;
                        break;
                }
                Rectangle rect = x448fd9ab43628c71.CalcLayoutRect(ctrl.ImageAlign, ctrl.RightToLeft, clientRectangle, image.Width, image.Height);
                if (base.ctrlMouseState == xb9506a535e31f22a.MouseDown)
                {
                    x448fd9ab43628c71.OffsetRect(ref rect, 1, 1);
                }
                x4b101060f4767186.DrawImage(image, rect.X, rect.Y, image.Width, image.Height);
            }
        }

        private void xf0204c2f26f9917d(object xe0292b9ed559da7d, EventArgs xfbf34718e704c6bc)
        {
            if (base.CanPaint)
            {
                this.PaintControl();
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

