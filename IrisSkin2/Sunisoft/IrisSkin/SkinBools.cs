namespace Sunisoft.IrisSkin
{
    using System;

    public class SkinBools : IDisposable
    {
        public bool SKIN2_BOTTOMBORDERNEEDREGION;
        public bool SKIN2_BOTTOMBORDERTHREESECT;
        public bool SKIN2_TITLEBARNEEDREGION;
        public bool SKIN2_TITLEFIVESECT;
        private SkinEngine xdc87e2b99332cd4a;

        public SkinBools(SkinEngine engine)
        {
            this.xdc87e2b99332cd4a = engine;
            this.x225b462871340e0d();
        }

        public void Dispose()
        {
            this.xdc87e2b99332cd4a = null;
        }

        private void x225b462871340e0d()
        {
            this.SKIN2_TITLEFIVESECT = this.xdc87e2b99332cd4a.GetBool("SKIN2_TITLEFIVESECT");
            this.SKIN2_BOTTOMBORDERTHREESECT = this.xdc87e2b99332cd4a.GetBool("SKIN2_BOTTOMBORDERTHREESECT");
            this.SKIN2_TITLEBARNEEDREGION = this.xdc87e2b99332cd4a.GetBool("SKIN2_TITLEBARNEEDREGION");
            this.SKIN2_BOTTOMBORDERNEEDREGION = this.xdc87e2b99332cd4a.GetBool("SKIN2_BOTTOMBORDERNEEDREGION");
        }
    }
}

