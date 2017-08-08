namespace Sunisoft.IrisSkin.InternalControls
{
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Drawing.Imaging;
    using System.Windows.Forms;

    [ToolboxItem(false)]
    internal class x5d3356d9dffccb60 : Control
    {
        protected int borderWidth;
        protected int buttonDisabledImageIndex;
        protected int buttonEnabledImageIndex;
        protected System.Drawing.Imaging.ImageAttributes buttonImageAttr;
        protected System.Windows.Forms.ImageList buttonImages;
        protected MouseButtons mouseButton;
        protected bool mouseCapture;
        protected bool mouseOver;
        protected bool popupStyle;

        public x5d3356d9dffccb60()
        {
            this.InternalConstruct(null, -1, -1, null);
        }

        public x5d3356d9dffccb60(System.Windows.Forms.ImageList imageList, int imageIndexEnabled)
        {
            this.InternalConstruct(imageList, imageIndexEnabled, -1, null);
        }

        public x5d3356d9dffccb60(System.Windows.Forms.ImageList imageList, int imageIndexEnabled, int imageIndexDisabled)
        {
            this.InternalConstruct(imageList, imageIndexEnabled, imageIndexDisabled, null);
        }

        public x5d3356d9dffccb60(System.Windows.Forms.ImageList imageList, int imageIndexEnabled, int imageIndexDisabled, System.Drawing.Imaging.ImageAttributes imageAttr)
        {
            this.InternalConstruct(imageList, imageIndexEnabled, imageIndexDisabled, imageAttr);
        }

        public void InternalConstruct(System.Windows.Forms.ImageList imageList, int imageIndexEnabled, int imageIndexDisabled, System.Drawing.Imaging.ImageAttributes imageAttr)
        {
            this.buttonImages = imageList;
            this.buttonEnabledImageIndex = imageIndexEnabled;
            this.buttonDisabledImageIndex = imageIndexDisabled;
            this.buttonImageAttr = imageAttr;
            this.borderWidth = 2;
            this.mouseOver = false;
            this.mouseCapture = false;
            this.popupStyle = true;
            this.mouseButton = MouseButtons.None;
            base.SetStyle(ControlStyles.DoubleBuffer | ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint, true);
            base.SetStyle(ControlStyles.StandardDoubleClick, false);
            base.SetStyle(ControlStyles.Selectable, false);
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            if (!this.mouseCapture)
            {
                this.mouseOver = true;
                this.mouseCapture = true;
                this.mouseButton = e.Button;
                base.Invalidate();
            }
            base.OnMouseDown(e);
        }

        protected override void OnMouseEnter(EventArgs e)
        {
            this.mouseOver = true;
            base.Invalidate();
            base.OnMouseEnter(e);
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            this.mouseOver = false;
            base.Invalidate();
            base.OnMouseLeave(e);
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            bool flag = base.ClientRectangle.Contains(new Point(e.X, e.Y));
            if (flag != this.mouseOver)
            {
                this.mouseOver = flag;
                base.Invalidate();
            }
            base.OnMouseMove(e);
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            if (e.Button == this.mouseButton)
            {
                this.mouseOver = false;
                this.mouseCapture = false;
                base.Invalidate();
            }
            else
            {
                base.Capture = true;
            }
            base.OnMouseUp(e);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            if (this.buttonImages != null)
            {
                if (!base.Enabled)
                {
                    if (this.buttonDisabledImageIndex != -1)
                    {
                        if (this.buttonImageAttr == null)
                        {
                            e.Graphics.DrawImage(this.buttonImages.Images[this.buttonDisabledImageIndex], new Point(1, 1));
                        }
                        else
                        {
                            Image image = this.buttonImages.Images[this.buttonDisabledImageIndex];
                            Point[] destPoints = new Point[3];
                            destPoints[0].X = 1;
                            destPoints[0].Y = 1;
                            destPoints[1].X = destPoints[0].X + image.Width;
                            destPoints[1].Y = destPoints[0].Y;
                            destPoints[2].X = destPoints[0].X;
                            destPoints[2].Y = destPoints[1].Y + image.Height;
                            e.Graphics.DrawImage(this.buttonImages.Images[this.buttonDisabledImageIndex], destPoints, new Rectangle(0, 0, image.Width, image.Height), GraphicsUnit.Pixel, this.buttonImageAttr);
                        }
                    }
                    else if (this.buttonEnabledImageIndex != -1)
                    {
                        ControlPaint.DrawImageDisabled(e.Graphics, this.buttonImages.Images[this.buttonEnabledImageIndex], 1, 1, this.BackColor);
                    }
                }
                else
                {
                    ButtonBorderStyle solid;
                    if (this.buttonImageAttr == null)
                    {
                        e.Graphics.DrawImage(this.buttonImages.Images[this.buttonEnabledImageIndex], (this.mouseOver && this.mouseCapture) ? new Point(2, 2) : new Point(1, 1));
                    }
                    else
                    {
                        Image image2 = this.buttonImages.Images[this.buttonEnabledImageIndex];
                        Point[] pointArray2 = new Point[3];
                        pointArray2[0].X = (this.mouseOver && this.mouseCapture) ? 2 : 1;
                        pointArray2[0].Y = (this.mouseOver && this.mouseCapture) ? 2 : 1;
                        pointArray2[1].X = pointArray2[0].X + image2.Width;
                        pointArray2[1].Y = pointArray2[0].Y;
                        pointArray2[2].X = pointArray2[0].X;
                        pointArray2[2].Y = pointArray2[1].Y + image2.Height;
                        e.Graphics.DrawImage(this.buttonImages.Images[this.buttonEnabledImageIndex], pointArray2, new Rectangle(0, 0, image2.Width, image2.Height), GraphicsUnit.Pixel, this.buttonImageAttr);
                    }
                    if (this.popupStyle)
                    {
                        if (this.mouseOver && base.Enabled)
                        {
                            solid = this.mouseCapture ? ButtonBorderStyle.Inset : ButtonBorderStyle.Outset;
                        }
                        else
                        {
                            solid = ButtonBorderStyle.Solid;
                        }
                    }
                    else if (base.Enabled)
                    {
                        solid = (this.mouseOver && this.mouseCapture) ? ButtonBorderStyle.Inset : ButtonBorderStyle.Outset;
                    }
                    else
                    {
                        solid = ButtonBorderStyle.Solid;
                    }
                    ControlPaint.DrawBorder(e.Graphics, base.ClientRectangle, this.BackColor, this.borderWidth, solid, this.BackColor, this.borderWidth, solid, this.BackColor, this.borderWidth, solid, this.BackColor, this.borderWidth, solid);
                }
            }
            base.OnPaint(e);
        }

        [DefaultValue(2), Category("Appearance")]
        public int BorderWidth
        {
            get
            {
                return this.borderWidth;
            }
            set
            {
                if (this.borderWidth != value)
                {
                    this.borderWidth = value;
                    base.Invalidate();
                }
            }
        }

        [Category("Appearance"), DefaultValue((string) null)]
        public System.Drawing.Imaging.ImageAttributes ImageAttributes
        {
            get
            {
                return this.buttonImageAttr;
            }
            set
            {
                if (this.buttonImageAttr != value)
                {
                    this.buttonImageAttr = value;
                    base.Invalidate();
                }
            }
        }

        [Category("Appearance"), DefaultValue(-1)]
        public int ImageIndexDisabled
        {
            get
            {
                return this.buttonDisabledImageIndex;
            }
            set
            {
                if (this.buttonDisabledImageIndex != value)
                {
                    this.buttonDisabledImageIndex = value;
                    base.Invalidate();
                }
            }
        }

        [DefaultValue(-1), Category("Appearance")]
        public int ImageIndexEnabled
        {
            get
            {
                return this.buttonEnabledImageIndex;
            }
            set
            {
                if (this.buttonEnabledImageIndex != value)
                {
                    this.buttonEnabledImageIndex = value;
                    base.Invalidate();
                }
            }
        }

        [Category("Appearance"), DefaultValue((string) null)]
        public System.Windows.Forms.ImageList ImageList
        {
            get
            {
                return this.buttonImages;
            }
            set
            {
                if (this.buttonImages != value)
                {
                    this.buttonImages = value;
                    base.Invalidate();
                }
            }
        }

        [Category("Appearance"), DefaultValue(true)]
        public bool PopupStyle
        {
            get
            {
                return this.popupStyle;
            }
            set
            {
                if (this.popupStyle != value)
                {
                    this.popupStyle = value;
                    base.Invalidate();
                }
            }
        }
    }
}

