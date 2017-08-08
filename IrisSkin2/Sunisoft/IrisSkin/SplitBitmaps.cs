namespace Sunisoft.IrisSkin
{
    using System;
    using System.Drawing;

    public class SplitBitmaps : IDisposable
    {
        private Bitmap[] x3d2e024a784598d5;
        private Bitmap[] x4944f3253ed604fe;
        private Bitmap[] x842bba7c67d596f0;
        private Bitmap[] x842f8dce891d0e49;
        private SkinEngine xdc87e2b99332cd4a;

        public SplitBitmaps(SkinEngine engine)
        {
            this.xdc87e2b99332cd4a = engine;
            this.x25a256ec885d930e();
        }

        public void Dispose()
        {
            this.xdc87e2b99332cd4a = null;
        }

        private void x25a256ec885d930e()
        {
        }

        public Bitmap[] SKIN2_BUTTON
        {
            get
            {
                if (this.x3d2e024a784598d5 == null)
                {
                    this.x3d2e024a784598d5 = new Bitmap[4];
                    for (int i = 1; i < 4; i++)
                    {
                        this.x3d2e024a784598d5[i] = this.xdc87e2b99332cd4a.GetBitmap("SKIN2_BUTTON", 3, i);
                    }
                }
                return this.x3d2e024a784598d5;
            }
        }

        public Bitmap[] SKIN2_CHECKBOX
        {
            get
            {
                if (this.x842bba7c67d596f0 == null)
                {
                    this.x842bba7c67d596f0 = new Bitmap[7];
                    for (int i = 1; i < 7; i++)
                    {
                        this.x842bba7c67d596f0[i] = this.xdc87e2b99332cd4a.GetBitmap("SKIN2_CHECKBOX", 6, i);
                    }
                }
                return this.x842bba7c67d596f0;
            }
        }

        public Bitmap[] SKIN2_RADIOBUTTON
        {
            get
            {
                if (this.x4944f3253ed604fe == null)
                {
                    this.x4944f3253ed604fe = new Bitmap[5];
                    for (int i = 1; i < 5; i++)
                    {
                        this.x4944f3253ed604fe[i] = this.xdc87e2b99332cd4a.GetBitmap("SKIN2_RADIOBUTTON", 4, i);
                    }
                }
                return this.x4944f3253ed604fe;
            }
        }

        public Bitmap[] SKIN2_TITLEBUTTONS
        {
            get
            {
                if (this.x842f8dce891d0e49 == null)
                {
                    this.x842f8dce891d0e49 = new Bitmap[0x13];
                    for (int i = 1; i < 0x13; i++)
                    {
                        this.x842f8dce891d0e49[i] = this.xdc87e2b99332cd4a.GetBitmap("SKIN2_TITLEBUTTONS", 0x12, i);
                    }
                }
                return this.x842f8dce891d0e49;
            }
        }
    }
}

