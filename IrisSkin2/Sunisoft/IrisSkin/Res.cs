namespace Sunisoft.IrisSkin
{
    using System;

    public class Res : IDisposable
    {
        private SkinInt x2491a4d79a7b1ed9;
        private Sunisoft.IrisSkin.TabControlRes x3bf027fb2999c92f;
        private SkinBools x78ac2e397a838680;
        private Sunisoft.IrisSkin.SplitBitmaps x89e66747b11f0a70;
        private SkinBrushes x91bc7f70f8537c7c;
        private SkinColors xa70c7ccd3278240f;
        private Sunisoft.IrisSkin.ScrollBarRes xbf1147be56545845;
        private SkinEngine xdc87e2b99332cd4a;
        private SkinBitmaps xfcfb5c1949fe6b20;

        public Res(SkinEngine engine)
        {
            this.xdc87e2b99332cd4a = engine;
            this.x30b60dadc625bd97();
        }

        public void Dispose()
        {
            this.xdc87e2b99332cd4a = null;
        }

        private void x30b60dadc625bd97()
        {
            this.xa70c7ccd3278240f = new SkinColors(this.xdc87e2b99332cd4a);
            this.x91bc7f70f8537c7c = new SkinBrushes(this.xdc87e2b99332cd4a);
            this.x2491a4d79a7b1ed9 = new SkinInt(this.xdc87e2b99332cd4a);
            this.x78ac2e397a838680 = new SkinBools(this.xdc87e2b99332cd4a);
            this.xfcfb5c1949fe6b20 = new SkinBitmaps(this.xdc87e2b99332cd4a);
            this.x89e66747b11f0a70 = new Sunisoft.IrisSkin.SplitBitmaps(this.xdc87e2b99332cd4a);
            this.xbf1147be56545845 = new Sunisoft.IrisSkin.ScrollBarRes(this.xdc87e2b99332cd4a);
            this.x3bf027fb2999c92f = new Sunisoft.IrisSkin.TabControlRes(this.xdc87e2b99332cd4a);
        }

        public SkinBitmaps Bitmaps
        {
            get
            {
                return this.xfcfb5c1949fe6b20;
            }
        }

        public SkinBools Bools
        {
            get
            {
                return this.x78ac2e397a838680;
            }
        }

        public SkinBrushes Brushes
        {
            get
            {
                return this.x91bc7f70f8537c7c;
            }
        }

        public SkinColors Colors
        {
            get
            {
                return this.xa70c7ccd3278240f;
            }
        }

        public SkinInt Integers
        {
            get
            {
                return this.x2491a4d79a7b1ed9;
            }
        }

        public Sunisoft.IrisSkin.ScrollBarRes ScrollBarRes
        {
            get
            {
                return this.xbf1147be56545845;
            }
        }

        public Sunisoft.IrisSkin.SplitBitmaps SplitBitmaps
        {
            get
            {
                return this.x89e66747b11f0a70;
            }
        }

        public Sunisoft.IrisSkin.TabControlRes TabControlRes
        {
            get
            {
                return this.x3bf027fb2999c92f;
            }
        }
    }
}

