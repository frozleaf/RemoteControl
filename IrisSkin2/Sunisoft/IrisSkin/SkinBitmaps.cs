namespace Sunisoft.IrisSkin
{
    using System;
    using System.Drawing;

    public class SkinBitmaps : IDisposable
    {
        private Bitmap x048862aee2fbb7b6;
        private Bitmap x079230832986b773;
        private Bitmap x0f026a1d86e2cfd4;
        private Bitmap x114f6663353c562b;
        private Bitmap x1c886665a6136e76;
        private Bitmap x24802ac43d9b7af5;
        private Bitmap x3c2a21d8d15d9bb1;
        private Bitmap x3d2e024a784598d5;
        private Bitmap x4021106c9f5f9916;
        private Bitmap x421416c7bdb1c8c0;
        private Bitmap x4944f3253ed604fe;
        private Bitmap x4d29203e30529d37;
        private Bitmap x4fa1c7e9a378ce7d;
        private Bitmap x4fb141915a72323d;
        private Bitmap x53cf9ae365fc6750;
        private Bitmap x5b254bbb9048a52a;
        private Bitmap x5c533cd8e6e5c871;
        private Bitmap x625a780534399c56;
        private Bitmap x6b5200b8cb0badbb;
        private Bitmap x73408b9bae28f1c0;
        private Bitmap x842bba7c67d596f0;
        private Bitmap x842f8dce891d0e49;
        private Bitmap x8ca5df93d87e7601;
        private Bitmap x8d596e2df0e45028;
        private Bitmap x8e6b08364c633852;
        private Bitmap x95993ff743c4d716;
        private Bitmap x9c3e20a6f8d01e7a;
        private Bitmap xaca54acdfac1b158;
        private Bitmap xb593b0b67850f120;
        private Bitmap xd17a79780ea48cc0;
        private SkinEngine xdc87e2b99332cd4a;
        private Bitmap xdfda3855394bd512;
        private Bitmap xe317167b65b20e4f;
        private Bitmap xeb68386b8c78c07a;
        private Bitmap xf15b36cdcfff527e;
        private Bitmap xf33008181c845518;

        public SkinBitmaps(SkinEngine engine)
        {
            this.xdc87e2b99332cd4a = engine;
        }

        public void Dispose()
        {
            this.xdc87e2b99332cd4a = null;
        }

        public Bitmap SKIN2_BUTTON
        {
            get
            {
                if (this.x3d2e024a784598d5 == null)
                {
                    this.x3d2e024a784598d5 = this.xdc87e2b99332cd4a.GetBitmap("SKIN2_BUTTON");
                }
                return this.x3d2e024a784598d5;
            }
        }

        public Bitmap SKIN2_CHECKBOX
        {
            get
            {
                if (this.x842bba7c67d596f0 == null)
                {
                    this.x842bba7c67d596f0 = this.xdc87e2b99332cd4a.GetBitmap("SKIN2_CHECKBOX");
                }
                return this.x842bba7c67d596f0;
            }
        }

        public Bitmap SKIN2_CHECKBOXLIST
        {
            get
            {
                if (this.x4d29203e30529d37 == null)
                {
                    this.x4d29203e30529d37 = this.xdc87e2b99332cd4a.GetBitmap("SKIN2_CHECKBOXLIST");
                }
                return this.x4d29203e30529d37;
            }
        }

        public Bitmap SKIN2_CHECKEDMENUICON
        {
            get
            {
                if (this.xdfda3855394bd512 == null)
                {
                    this.xdfda3855394bd512 = this.xdc87e2b99332cd4a.GetBitmap("SKIN2_CHECKEDMENUICON");
                }
                return this.xdfda3855394bd512;
            }
        }

        public Bitmap SKIN2_FORMBOTTOMBORDER1
        {
            get
            {
                if (this.x5b254bbb9048a52a == null)
                {
                    this.x5b254bbb9048a52a = this.xdc87e2b99332cd4a.GetBitmap("SKIN2_FORMBOTTOMBORDER1");
                }
                return this.x5b254bbb9048a52a;
            }
        }

        public Bitmap SKIN2_FORMBOTTOMBORDER2
        {
            get
            {
                if (this.x4021106c9f5f9916 == null)
                {
                    this.x4021106c9f5f9916 = this.xdc87e2b99332cd4a.GetBitmap("SKIN2_FORMBOTTOMBORDER2");
                }
                return this.x4021106c9f5f9916;
            }
        }

        public Bitmap SKIN2_FORMBOTTOMBORDER3
        {
            get
            {
                if (this.x73408b9bae28f1c0 == null)
                {
                    this.x73408b9bae28f1c0 = this.xdc87e2b99332cd4a.GetBitmap("SKIN2_FORMBOTTOMBORDER3");
                }
                return this.x73408b9bae28f1c0;
            }
        }

        public Bitmap SKIN2_FORMLEFTBORDER
        {
            get
            {
                if (this.x5c533cd8e6e5c871 == null)
                {
                    this.x5c533cd8e6e5c871 = this.xdc87e2b99332cd4a.GetBitmap("SKIN2_FORMLEFTBORDER");
                }
                return this.x5c533cd8e6e5c871;
            }
        }

        public Bitmap SKIN2_FORMRIGHTBORDER
        {
            get
            {
                if (this.x0f026a1d86e2cfd4 == null)
                {
                    this.x0f026a1d86e2cfd4 = this.xdc87e2b99332cd4a.GetBitmap("SKIN2_FORMRIGHTBORDER");
                }
                return this.x0f026a1d86e2cfd4;
            }
        }

        public Bitmap SKIN2_MENUBAR
        {
            get
            {
                if (this.xaca54acdfac1b158 == null)
                {
                    this.xaca54acdfac1b158 = this.xdc87e2b99332cd4a.GetBitmap("SKIN2_MENUBAR");
                }
                return this.xaca54acdfac1b158;
            }
        }

        public Bitmap SKIN2_MINIMIZEDTITLE
        {
            get
            {
                if (this.x079230832986b773 == null)
                {
                    this.x079230832986b773 = this.xdc87e2b99332cd4a.GetBitmap("SKIN2_MINIMIZEDTITLE");
                }
                return this.x079230832986b773;
            }
        }

        public Bitmap SKIN2_PROGRESSBAR1
        {
            get
            {
                if (this.x4fa1c7e9a378ce7d == null)
                {
                    this.x4fa1c7e9a378ce7d = this.xdc87e2b99332cd4a.GetBitmap("SKIN2_PROGRESSBAR1");
                }
                return this.x4fa1c7e9a378ce7d;
            }
        }

        public Bitmap SKIN2_PROGRESSBAR2
        {
            get
            {
                if (this.x95993ff743c4d716 == null)
                {
                    this.x95993ff743c4d716 = this.xdc87e2b99332cd4a.GetBitmap("SKIN2_PROGRESSBAR2");
                }
                return this.x95993ff743c4d716;
            }
        }

        public Bitmap SKIN2_PROGRESSBAR3
        {
            get
            {
                if (this.x4fb141915a72323d == null)
                {
                    this.x4fb141915a72323d = this.xdc87e2b99332cd4a.GetBitmap("SKIN2_PROGRESSBAR3");
                }
                return this.x4fb141915a72323d;
            }
        }

        public Bitmap SKIN2_PROGRESSBAR4
        {
            get
            {
                if (this.x8ca5df93d87e7601 == null)
                {
                    this.x8ca5df93d87e7601 = this.xdc87e2b99332cd4a.GetBitmap("SKIN2_PROGRESSBAR4");
                }
                return this.x8ca5df93d87e7601;
            }
        }

        public Bitmap SKIN2_RADIOBUTTON
        {
            get
            {
                if (this.x4944f3253ed604fe == null)
                {
                    this.x4944f3253ed604fe = this.xdc87e2b99332cd4a.GetBitmap("SKIN2_RADIOBUTTON");
                }
                return this.x4944f3253ed604fe;
            }
        }

        public Bitmap SKIN2_SCROLLBAR
        {
            get
            {
                if (this.xe317167b65b20e4f == null)
                {
                    this.xe317167b65b20e4f = this.xdc87e2b99332cd4a.GetBitmap("SKIN2_SCROLLBAR");
                }
                return this.xe317167b65b20e4f;
            }
        }

        public Bitmap SKIN2_SCROLLBARDOWNBUTTON
        {
            get
            {
                if (this.x9c3e20a6f8d01e7a == null)
                {
                    this.x9c3e20a6f8d01e7a = this.xdc87e2b99332cd4a.GetBitmap("SKIN2_SCROLLBARDOWNBUTTON");
                }
                return this.x9c3e20a6f8d01e7a;
            }
        }

        public Bitmap SKIN2_SCROLLBARUPBUTTON
        {
            get
            {
                if (this.xf15b36cdcfff527e == null)
                {
                    this.xf15b36cdcfff527e = this.xdc87e2b99332cd4a.GetBitmap("SKIN2_SCROLLBARUPBUTTON");
                }
                return this.xf15b36cdcfff527e;
            }
        }

        public Bitmap SKIN2_SCROLLBUTTON
        {
            get
            {
                if (this.x53cf9ae365fc6750 == null)
                {
                    this.x53cf9ae365fc6750 = this.xdc87e2b99332cd4a.GetBitmap("SKIN2_SCROLLBUTTON");
                }
                return this.x53cf9ae365fc6750;
            }
        }

        public Bitmap SKIN2_SIDECHANNELBAR
        {
            get
            {
                if (this.x625a780534399c56 == null)
                {
                    this.x625a780534399c56 = this.xdc87e2b99332cd4a.GetBitmap("SKIN2_SIDECHANNELBAR");
                }
                return this.x625a780534399c56;
            }
        }

        public Bitmap SKIN2_SIDECHANNELTITLE
        {
            get
            {
                if (this.xf33008181c845518 == null)
                {
                    this.xf33008181c845518 = this.xdc87e2b99332cd4a.GetBitmap("SKIN2_SIDECHANNELTITLE");
                }
                return this.xf33008181c845518;
            }
        }

        public Bitmap SKIN2_TABCONTROL
        {
            get
            {
                if (this.x048862aee2fbb7b6 == null)
                {
                    this.x048862aee2fbb7b6 = this.xdc87e2b99332cd4a.GetBitmap("SKIN2_TABCONTROL");
                }
                return this.x048862aee2fbb7b6;
            }
        }

        public Bitmap SKIN2_TABCONTROLLINE
        {
            get
            {
                if (this.x24802ac43d9b7af5 == null)
                {
                    this.x24802ac43d9b7af5 = this.xdc87e2b99332cd4a.GetBitmap("SKIN2_TABCONTROLLINE");
                }
                return this.x24802ac43d9b7af5;
            }
        }

        public Bitmap SKIN2_TITLEBAR1
        {
            get
            {
                if (this.x3c2a21d8d15d9bb1 == null)
                {
                    this.x3c2a21d8d15d9bb1 = this.xdc87e2b99332cd4a.GetBitmap("SKIN2_TITLEBAR1");
                }
                return this.x3c2a21d8d15d9bb1;
            }
        }

        public Bitmap SKIN2_TITLEBAR2
        {
            get
            {
                if (this.x6b5200b8cb0badbb == null)
                {
                    this.x6b5200b8cb0badbb = this.xdc87e2b99332cd4a.GetBitmap("SKIN2_TITLEBAR2");
                }
                return this.x6b5200b8cb0badbb;
            }
        }

        public Bitmap SKIN2_TITLEBAR3
        {
            get
            {
                if (this.x114f6663353c562b == null)
                {
                    this.x114f6663353c562b = this.xdc87e2b99332cd4a.GetBitmap("SKIN2_TITLEBAR3");
                }
                return this.x114f6663353c562b;
            }
        }

        public Bitmap SKIN2_TITLEBAR4
        {
            get
            {
                if (this.xb593b0b67850f120 == null)
                {
                    this.xb593b0b67850f120 = this.xdc87e2b99332cd4a.GetBitmap("SKIN2_TITLEBAR4");
                }
                return this.xb593b0b67850f120;
            }
        }

        public Bitmap SKIN2_TITLEBAR5
        {
            get
            {
                if (this.x421416c7bdb1c8c0 == null)
                {
                    this.x421416c7bdb1c8c0 = this.xdc87e2b99332cd4a.GetBitmap("SKIN2_TITLEBAR5");
                }
                return this.x421416c7bdb1c8c0;
            }
        }

        public Bitmap SKIN2_TITLEBUTTONS
        {
            get
            {
                if (this.x842f8dce891d0e49 == null)
                {
                    this.x842f8dce891d0e49 = this.xdc87e2b99332cd4a.GetBitmap("SKIN2_TITLEBUTTONS");
                }
                return this.x842f8dce891d0e49;
            }
        }

        public Bitmap SKIN2_TOOLBAR
        {
            get
            {
                if (this.xd17a79780ea48cc0 == null)
                {
                    this.xd17a79780ea48cc0 = this.xdc87e2b99332cd4a.GetBitmap("SKIN2_TOOLBAR");
                }
                return this.xd17a79780ea48cc0;
            }
        }

        public Bitmap SKIN2_TRACKBAR
        {
            get
            {
                if (this.x8e6b08364c633852 == null)
                {
                    this.x8e6b08364c633852 = this.xdc87e2b99332cd4a.GetBitmap("SKIN2_TRACKBAR");
                }
                return this.x8e6b08364c633852;
            }
        }

        public Bitmap SKIN2_TRACKBARSLIDER
        {
            get
            {
                if (this.xeb68386b8c78c07a == null)
                {
                    this.xeb68386b8c78c07a = this.xdc87e2b99332cd4a.GetBitmap("SKIN2_TRACKBARSLIDER");
                }
                return this.xeb68386b8c78c07a;
            }
        }

        public Bitmap SKIN2_TRACKBARSLIDER_180
        {
            get
            {
                if (this.x8d596e2df0e45028 == null)
                {
                    this.x8d596e2df0e45028 = this.xdc87e2b99332cd4a.GetBitmap("SKIN2_TRACKBARSLIDER");
                    if (this.x8d596e2df0e45028 != null)
                    {
                        this.x8d596e2df0e45028.RotateFlip(RotateFlipType.Rotate180FlipNone);
                    }
                }
                return this.x8d596e2df0e45028;
            }
        }

        public Bitmap SKIN2_TRACKBARVSLIDER
        {
            get
            {
                if (this.x1c886665a6136e76 == null)
                {
                    this.x1c886665a6136e76 = this.xdc87e2b99332cd4a.GetBitmap("SKIN2_TRACKBARVSLIDER");
                }
                return this.x1c886665a6136e76;
            }
        }
    }
}

