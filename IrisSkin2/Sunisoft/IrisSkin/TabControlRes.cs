namespace Sunisoft.IrisSkin
{
    using System;
    using System.Drawing;

    public class TabControlRes : IDisposable
    {
        private Pen x4e3bc09d0766bdda;
        private Bitmap[,] x564c6c527905c683;
        private Bitmap x711921dd2712f496;
        private Brush x9646b37d9a66d008;
        private SkinEngine xdc87e2b99332cd4a;

        public TabControlRes(SkinEngine engine)
        {
            this.xdc87e2b99332cd4a = engine;
            this.x03f66216ed8e1afe();
        }

        public void Dispose()
        {
            this.xdc87e2b99332cd4a = null;
        }

        private void x03f66216ed8e1afe()
        {
            this.x9646b37d9a66d008 = this.xdc87e2b99332cd4a.GetBrush("SKIN2_TABCONTROLCOLOR");
            this.x4e3bc09d0766bdda = new Pen(this.xdc87e2b99332cd4a.GetBrush("SKIN2_TABCONTROLBORDERCOLOR"), 1f);
            this.x564c6c527905c683 = new Bitmap[2, 3];
            for (int i = 0; i < 3; i++)
            {
                this.x564c6c527905c683[0, i] = this.xdc87e2b99332cd4a.GetBitmap("SKIN2_TABCONTROL", 6, i + 1);
                this.x564c6c527905c683[1, i] = this.xdc87e2b99332cd4a.GetBitmap("SKIN2_TABCONTROL", 6, i + 4);
            }
            this.x711921dd2712f496 = this.xdc87e2b99332cd4a.GetBitmap("SKIN2_TABCONTROLLINE");
        }

        public Brush BackColorBrush
        {
            get
            {
                return this.x9646b37d9a66d008;
            }
        }

        public Pen BorderPen
        {
            get
            {
                return this.x4e3bc09d0766bdda;
            }
        }

        public Bitmap[,] TabImage
        {
            get
            {
                return this.x564c6c527905c683;
            }
        }

        public Bitmap TabLine
        {
            get
            {
                return this.x711921dd2712f496;
            }
        }
    }
}

