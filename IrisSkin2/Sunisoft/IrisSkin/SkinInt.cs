namespace Sunisoft.IrisSkin
{
    using System;

    public class SkinInt : IDisposable
    {
        public int SKIN2_BOTTOMREGIONMAXY;
        public int SKIN2_BOTTOMREGIONMINY;
        public int SKIN2_TITLEBARBUTTONPOSX;
        public int SKIN2_TITLEBARBUTTONPOSY;
        public int SKIN2_TITLEBARCAPTIONTOP;
        public int SKIN2_TITLEBARICONPOSX;
        public int SKIN2_TITLEBARICONPOSY;
        public int SKIN2_TITLEBARREGIONMAXY;
        public int SKIN2_TITLEBARREGIONMINY;
        private SkinEngine xdc87e2b99332cd4a;

        public SkinInt(SkinEngine engine)
        {
            this.xdc87e2b99332cd4a = engine;
            this.xe098eb5fc84e11fd();
        }

        public void Dispose()
        {
            this.xdc87e2b99332cd4a = null;
        }

        private void xe098eb5fc84e11fd()
        {
            this.SKIN2_TITLEBARICONPOSX = this.xdc87e2b99332cd4a.GetInt("SKIN2_TITLEBARICONPOSX");
            this.SKIN2_TITLEBARICONPOSY = this.xdc87e2b99332cd4a.GetInt("SKIN2_TITLEBARICONPOSY");
            this.SKIN2_TITLEBARCAPTIONTOP = this.xdc87e2b99332cd4a.GetInt("SKIN2_TITLEBARCAPTIONTOP");
            this.SKIN2_TITLEBARBUTTONPOSX = this.xdc87e2b99332cd4a.GetInt("SKIN2_TITLEBARBUTTONPOSX");
            this.SKIN2_TITLEBARBUTTONPOSY = this.xdc87e2b99332cd4a.GetInt("SKIN2_TITLEBARBUTTONPOSY");
            this.SKIN2_TITLEBARREGIONMINY = this.xdc87e2b99332cd4a.GetInt("SKIN2_TITLEBARREGIONMINY");
            this.SKIN2_TITLEBARREGIONMAXY = this.xdc87e2b99332cd4a.GetInt("SKIN2_TITLEBARREGIONMAXY");
            this.SKIN2_BOTTOMREGIONMINY = this.xdc87e2b99332cd4a.GetInt("SKIN2_BOTTOMREGIONMINY");
            this.SKIN2_BOTTOMREGIONMAXY = this.xdc87e2b99332cd4a.GetInt("SKIN2_BOTTOMREGIONMAXY");
        }
    }
}

