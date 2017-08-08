namespace Sunisoft.IrisSkin
{
    using System;
    using System.Drawing;

    public class ScrollBarRes : IDisposable
    {
        private Bitmap[,] x804214fd8fadc2a7;
        private Brush x9646b37d9a66d008;
        private bool xa605cad12563f03c;
        private Bitmap[,] xc088a3c732c3a72e;
        private SkinEngine xdc87e2b99332cd4a;
        private Bitmap[,] xe26e029a72589589;

        public ScrollBarRes(SkinEngine engine)
        {
            this.xdc87e2b99332cd4a = engine;
            this.x89aa76148c30a93b();
        }

        public void Dispose()
        {
            this.xdc87e2b99332cd4a = null;
        }

        private void x89aa76148c30a93b()
        {
            this.x9646b37d9a66d008 = this.xdc87e2b99332cd4a.GetBrush("SKIN2_SCROLLBARCOLOR");
            this.x804214fd8fadc2a7 = new Bitmap[2, 3];
            this.xe26e029a72589589 = new Bitmap[2, 3];
            this.xc088a3c732c3a72e = new Bitmap[2, 5];
            this.x804214fd8fadc2a7[0, 0] = this.xdc87e2b99332cd4a.GetBitmap("SKIN2_SCROLLBARUPBUTTON", 3, 1);
            this.x804214fd8fadc2a7[0, 1] = this.xdc87e2b99332cd4a.GetBitmap("SKIN2_SCROLLBARUPBUTTON", 3, 2);
            this.x804214fd8fadc2a7[0, 2] = this.xdc87e2b99332cd4a.GetBitmap("SKIN2_SCROLLBARUPBUTTON", 3, 3);
            this.xe26e029a72589589[0, 0] = this.xdc87e2b99332cd4a.GetBitmap("SKIN2_SCROLLBARDOWNBUTTON", 3, 1);
            this.xe26e029a72589589[0, 1] = this.xdc87e2b99332cd4a.GetBitmap("SKIN2_SCROLLBARDOWNBUTTON", 3, 2);
            this.xe26e029a72589589[0, 2] = this.xdc87e2b99332cd4a.GetBitmap("SKIN2_SCROLLBARDOWNBUTTON", 3, 3);
            for (int i = 0; i < 3; i++)
            {
                this.x804214fd8fadc2a7[1, i] = (Bitmap) this.x804214fd8fadc2a7[0, i].Clone();
                this.x804214fd8fadc2a7[1, i].RotateFlip(RotateFlipType.Rotate270FlipNone);
                this.xe26e029a72589589[1, i] = (Bitmap) this.xe26e029a72589589[0, i].Clone();
                this.xe26e029a72589589[1, i].RotateFlip(RotateFlipType.Rotate270FlipNone);
            }
            Bitmap image = this.xdc87e2b99332cd4a.GetBitmap("SKIN2_SCROLLBAR");
            try
            {
                if (((image.Height == 0x21) && (this.xdc87e2b99332cd4a.GetColor("SKIN2_SCROLLBARCOLOR") == Color.FromArgb(0xff, 0xcb, 0xcb, 0xcb))) && (image.GetPixel(1, 10) == Color.FromArgb(0xff, 1, 0x39, 0xb2)))
                {
                    this.xa605cad12563f03c = true;
                }
            }
            catch
            {
            }
            int num2 = (image.Height - 6) / 3;
            int width = image.Width;
            int top = 0;
            int bottom = 0;
            for (int j = 0; j < 5; j++)
            {
                switch (j)
                {
                    case 0:
                    case 4:
                        bottom += 3;
                        break;

                    default:
                        if (j == 2)
                        {
                            bottom += num2 + ((image.Height - 6) % 3);
                        }
                        else
                        {
                            bottom += num2;
                        }
                        break;
                }
                this.xc088a3c732c3a72e[0, j] = new Bitmap(width, bottom - top);
                using (Graphics graphics = Graphics.FromImage(this.xc088a3c732c3a72e[0, j]))
                {
                    graphics.DrawImage(image, new Rectangle(0, 0, width, bottom - top), Rectangle.FromLTRB(0, top, width, bottom), GraphicsUnit.Pixel);
                }
                this.xc088a3c732c3a72e[1, j] = (Bitmap) this.xc088a3c732c3a72e[0, j].Clone();
                this.xc088a3c732c3a72e[1, j].RotateFlip(RotateFlipType.Rotate270FlipNone);
                top = bottom;
            }
        }

        public Brush BackColorBrush
        {
            get
            {
                return this.x9646b37d9a66d008;
            }
        }

        public Bitmap[,] DownButton
        {
            get
            {
                return this.xe26e029a72589589;
            }
        }

        public bool IsMacOS
        {
            get
            {
                return this.xa605cad12563f03c;
            }
        }

        public Bitmap[,] Slider
        {
            get
            {
                return this.xc088a3c732c3a72e;
            }
        }

        public Bitmap[,] UpButton
        {
            get
            {
                return this.x804214fd8fadc2a7;
            }
        }
    }
}

