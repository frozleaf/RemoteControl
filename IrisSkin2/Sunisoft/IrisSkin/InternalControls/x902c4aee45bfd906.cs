using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Drawing.Text;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Windows.Forms;

namespace Sunisoft.IrisSkin.InternalControls
{
    [ToolboxItem(false)]
    internal class x902c4aee45bfd906 : NativeWindow
    {
        internal enum xf398ffaf32ffe055
        {
            Left,
            Right,
            All
        }

        internal enum xd1ffc28f5c092301
        {
            BorderTop,
            BorderLeft,
            BorderBottom,
            BorderRight,
            ImageGapTop,
            ImageGapLeft,
            ImageGapBottom,
            ImageGapRight,
            TextGapLeft,
            TextGapRight,
            SubMenuGapLeft,
            SubMenuWidth,
            SubMenuGapRight,
            SeparatorHeight,
            SeparatorWidth,
            ShortcutGap,
            ShadowWidth,
            ShadowHeight,
            ExtraWidthGap,
            ExtraHeightGap,
            ExtraRightGap,
            ExtraReduce
        }

        internal enum xaa7785f730d8dd15
        {
            Check,
            Radio,
            SubMenu,
            CheckSelected,
            RadioSelected,
            SubMenuSelected,
            Expansion,
            ImageError
        }

        protected static readonly int[,] _position;

        protected static int selectionDelay;

        protected static int expansionDelay;

        protected static int imageWidth;

        protected static int imageHeight;

        protected static int shadowLength;

        protected static int shadowHalf;

        protected static int blendSteps;

        protected static Bitmap shadowCache;

        protected static int shadowCacheWidth;

        protected static int shadowCacheHeight;

        protected static ImageList menuImages;

        protected static bool supportsLayered;

        protected readonly uint WM_DISMISS = 1025u;

        protected readonly uint WM_OPERATE_SUBMENU = 1026u;

        protected System.Windows.Forms.Timer timer;

        protected Font textFont;

        protected int popupItem;

        protected int trackItem;

        protected int borderGap;

        protected int returnDir;

        protected int extraSize;

        protected int excludeOffset;

        protected int animateTime;

        protected bool animateFirst;

        protected bool animateIn;

        protected bool layered;

        protected bool exitLoop;

        protected bool mouseOver;

        protected bool popupDown;

        protected bool popupRight;

        protected bool excludeTop;

        protected bool showInfrequent;

        protected bool rememberExpansion;

        protected bool highlightInfrequent;

        protected Size currentSize;

        protected Point screenPosition;

        protected Point lastMousePosition;

        protected Point currentPoint;

        protected Point leftScreenPosition;

        protected Point aboveScreenPosition;

        protected xdbfa333b4cd503e0 direction;

        protected x902c4aee45bfd906 parentMenu;

        protected x902c4aee45bfd906 childMenu;

        protected x6fd23f8bad2f3ced animateItem;

        protected x1f5697535eab37b9 animateStyle;

        protected ArrayList drawCommands;

        protected x3c41176af7e54b01 parentControl;

        internal RightToLeft x94975a4c4f1d71c4;

        protected x5f4b657f68f87baa returnCommand;

        protected xd53b20b7b4b2a08a menuCommands;

        private x26569a56dfbc2c6d xaa7558c320af04eb;

        private x26569a56dfbc2c6d xd42ce0324cbc114a;

        private SkinEngine xcab6a0e662ada486;

        public event x26569a56dfbc2c6d Selected
        {
            [MethodImpl(MethodImplOptions.Synchronized)]
            add
            {
                this.xaa7558c320af04eb = (x26569a56dfbc2c6d)Delegate.Combine(this.xaa7558c320af04eb, value);
            }
            [MethodImpl(MethodImplOptions.Synchronized)]
            remove
            {
                this.xaa7558c320af04eb = (x26569a56dfbc2c6d)Delegate.Remove(this.xaa7558c320af04eb, value);
            }
        }

        public event x26569a56dfbc2c6d Deselected
        {
            [MethodImpl(MethodImplOptions.Synchronized)]
            add
            {
                this.xd42ce0324cbc114a = (x26569a56dfbc2c6d)Delegate.Combine(this.xd42ce0324cbc114a, value);
            }
            [MethodImpl(MethodImplOptions.Synchronized)]
            remove
            {
                this.xd42ce0324cbc114a = (x26569a56dfbc2c6d)Delegate.Remove(this.xd42ce0324cbc114a, value);
            }
        }

        public SkinEngine Engine
        {
            get
            {
                return this.xcab6a0e662ada486;
            }
            set
            {
                this.xcab6a0e662ada486 = value;
            }
        }

        public xd53b20b7b4b2a08a MenuCommands
        {
            get
            {
                return this.menuCommands;
            }
        }

        public Font Font
        {
            get
            {
                return this.textFont;
            }
            set
            {
                this.textFont = value;
            }
        }

        public bool ShowInfrequent
        {
            get
            {
                return this.showInfrequent;
            }
            set
            {
                this.showInfrequent = value;
            }
        }

        public bool RememberExpansion
        {
            get
            {
                return this.rememberExpansion;
            }
            set
            {
                this.rememberExpansion = value;
            }
        }

        public bool HighlightInfrequent
        {
            get
            {
                return this.highlightInfrequent;
            }
            set
            {
                this.highlightInfrequent = value;
            }
        }

        public x6fd23f8bad2f3ced Animate
        {
            get
            {
                return this.animateItem;
            }
            set
            {
                this.animateItem = value;
            }
        }

        public int AnimateTime
        {
            get
            {
                return this.animateTime;
            }
            set
            {
                this.animateTime = value;
            }
        }

        public x1f5697535eab37b9 AnimateStyle
        {
            get
            {
                return this.animateStyle;
            }
            set
            {
                this.animateStyle = value;
            }
        }

        static x902c4aee45bfd906()
        {
            x902c4aee45bfd906._position = new int[,]
			{
				{
					2,
					1,
					0,
					1,
					2,
					3,
					3,
					5,
					4,
					4,
					2,
					6,
					5,
					5,
					1,
					10,
					4,
					4,
					2,
					2,
					0,
					0
				},
				{
					1,
					0,
					1,
					2,
					2,
					1,
					3,
					4,
					3,
					3,
					2,
					8,
					5,
					5,
					5,
					10,
					0,
					0,
					2,
					2,
					2,
					5
				}
			};
            x902c4aee45bfd906.selectionDelay = 400;
            x902c4aee45bfd906.expansionDelay = 1100;
            x902c4aee45bfd906.imageWidth = 16;
            x902c4aee45bfd906.imageHeight = 16;
            x902c4aee45bfd906.shadowLength = 4;
            x902c4aee45bfd906.shadowHalf = 2;
            x902c4aee45bfd906.blendSteps = 6;
            x902c4aee45bfd906.shadowCache = null;
            x902c4aee45bfd906.shadowCacheWidth = 0;
            x902c4aee45bfd906.shadowCacheHeight = 0;
            x902c4aee45bfd906.menuImages = null;
            x902c4aee45bfd906.supportsLayered = false;
            x902c4aee45bfd906.menuImages = x58dd58a96343fde0.LoadBitmapStrip(Type.GetType("Sunisoft.IrisSkin.SkinEngine"), "Sunisoft.IrisSkin.ImagesPopupMenu.bmp", new Size(16, 16), new Point(0, 0));
            x902c4aee45bfd906.supportsLayered = (OSFeature.Feature.GetVersionPresent(OSFeature.LayeredWindows) != null);
        }

        public x902c4aee45bfd906(SkinEngine engine)
        {
            this.Engine = engine;
            this.drawCommands = new ArrayList();
            this.menuCommands = new xd53b20b7b4b2a08a();
            this.returnDir = 0;
            this.extraSize = 0;
            this.borderGap = 0;
            this.popupItem = -1;
            this.trackItem = -1;
            this.childMenu = null;
            this.exitLoop = false;
            this.popupDown = true;
            this.mouseOver = false;
            this.excludeTop = true;
            this.popupRight = true;
            this.parentMenu = null;
            this.excludeOffset = 0;
            this.parentControl = null;
            this.returnCommand = null;
            this.highlightInfrequent = false;
            this.showInfrequent = false;
            this.rememberExpansion = true;
            this.lastMousePosition = new Point(-1, -1);
            this.direction = xdbfa333b4cd503e0.Horizontal;
            this.textFont = SystemInformation.MenuFont;
            this.animateTime = 100;
            this.animateItem = x6fd23f8bad2f3ced.System;
            this.animateStyle = x1f5697535eab37b9.System;
            this.animateFirst = true;
            this.animateIn = true;
            this.timer = new System.Windows.Forms.Timer();
            this.timer.Interval = x902c4aee45bfd906.selectionDelay;
            this.timer.Tick += new EventHandler(this.x07cc1c177f7b41bd);
        }

        public x5f4b657f68f87baa TrackPopup(Point screenPos)
        {
            return this.TrackPopup(screenPos, false);
        }

        public x5f4b657f68f87baa TrackPopup(Point screenPos, bool selectFirst)
        {
            if (this.menuCommands.VisibleItems())
            {
                this.direction = xdbfa333b4cd503e0.Horizontal;
                this.screenPosition = screenPos;
                this.aboveScreenPosition = screenPos;
                this.leftScreenPosition = screenPos;
                return this.x6e4244e25732db7d(selectFirst);
            }
            return null;
        }

        public x5f4b657f68f87baa TrackPopupTaskBar(Point screenPos)
        {
            if (this.menuCommands.VisibleItems())
            {
                this.direction = xdbfa333b4cd503e0.Horizontal;
                this.screenPosition = screenPos;
                this.aboveScreenPosition = screenPos;
                this.leftScreenPosition = screenPos;
                return this.xf1c4e2176aec0ad6(false);
            }
            return null;
        }

        internal x5f4b657f68f87baa x6192996f26aa9421(Point x0ce73f6cbd7d5515, Point xd682fa060330cf55, xdbfa333b4cd503e0 x23e85093ba3a7d1d, xd53b20b7b4b2a08a x2ac8fd3ce3986cdc, int xb2c6baf52a3ff3eb, bool xacc37ebdd71fcc44, x3c41176af7e54b01 x37f2fc2a7f30f790, bool x0a7391c99fcdc469, ref int x0cd2bb383a46f073)
        {
            this.direction = x23e85093ba3a7d1d;
            this.parentControl = x37f2fc2a7f30f790;
            this.borderGap = xb2c6baf52a3ff3eb;
            this.animateIn = x0a7391c99fcdc469;
            xd53b20b7b4b2a08a xd53b20b7b4b2a08a = this.menuCommands;
            this.menuCommands = x2ac8fd3ce3986cdc;
            this.screenPosition = x0ce73f6cbd7d5515;
            this.aboveScreenPosition = xd682fa060330cf55;
            this.leftScreenPosition = x0ce73f6cbd7d5515;
            x5f4b657f68f87baa result = this.x6e4244e25732db7d(xacc37ebdd71fcc44);
            this.menuCommands = xd53b20b7b4b2a08a;
            this.parentControl = null;
            this.returnDir = x0cd2bb383a46f073;
            return result;
        }

        protected x5f4b657f68f87baa x6e4244e25732db7d(Point x9348d50e682f780a, Point xf361a72b1913ee9f, xd53b20b7b4b2a08a x2ac8fd3ce3986cdc, x902c4aee45bfd906 xb13f2de377f27597, bool xacc37ebdd71fcc44, x3c41176af7e54b01 x37f2fc2a7f30f790, bool xc8609d9b3214b3e7, bool x8e0eb9fe60b28fae, bool x0a7391c99fcdc469, ref int x0cd2bb383a46f073)
        {
            this.direction = xdbfa333b4cd503e0.Horizontal;
            this.parentControl = x37f2fc2a7f30f790;
            this.parentMenu = xb13f2de377f27597;
            this.animateIn = x0a7391c99fcdc469;
            xd53b20b7b4b2a08a xd53b20b7b4b2a08a = this.menuCommands;
            this.menuCommands = x2ac8fd3ce3986cdc;
            this.screenPosition = x9348d50e682f780a;
            this.aboveScreenPosition = x9348d50e682f780a;
            this.leftScreenPosition = xf361a72b1913ee9f;
            this.popupRight = xc8609d9b3214b3e7;
            this.popupDown = x8e0eb9fe60b28fae;
            x5f4b657f68f87baa result = this.x6e4244e25732db7d(xacc37ebdd71fcc44);
            this.menuCommands = xd53b20b7b4b2a08a;
            this.parentControl = null;
            this.parentMenu = null;
            this.returnDir = x0cd2bb383a46f073;
            return result;
        }

        protected x5f4b657f68f87baa x6e4244e25732db7d(bool xacc37ebdd71fcc44)
        {
            x5f4b657f68f87baa result;
            try
            {
                this.returnCommand = null;
                this.trackItem = -1;
                this.exitLoop = false;
                this.mouseOver = false;
                this.returnDir = 0;
                bool flag = false;
                this.animateFirst = true;
                this.xb3525a0e7d2fd376();
                x40255b11ef821fa3.MSG x8a41fbc87a3fb = default(x40255b11ef821fa3.MSG);
                if (xacc37ebdd71fcc44)
                {
                    this.x5b2e42a68d835b31();
                }
                x61467fe65a98f20c.SetCursor(x61467fe65a98f20c.LoadCursor(IntPtr.Zero, 32512u));
                bool flag2 = x61467fe65a98f20c.HideCaret(IntPtr.Zero);
                while (!this.exitLoop)
                {
                    if (x61467fe65a98f20c.WaitMessage())
                    {
                        while (!this.exitLoop && x61467fe65a98f20c.PeekMessage(ref x8a41fbc87a3fb, 0, 0u, 0u, 0u))
                        {
                            bool flag3 = false;
                            int num = this.currentSize.Width - x902c4aee45bfd906._position[0, 16];
                            int num2 = this.currentSize.Height - x902c4aee45bfd906._position[0, 17];
                            if (x8a41fbc87a3fb.message == 514u || x8a41fbc87a3fb.message == 520u || x8a41fbc87a3fb.message == 517u || x8a41fbc87a3fb.message == 524u || x8a41fbc87a3fb.message == 162u || x8a41fbc87a3fb.message == 168u || x8a41fbc87a3fb.message == 165u || x8a41fbc87a3fb.message == 172u)
                            {
                                x555516122dcc901e.POINT x0ce73f6cbd7d = this.x4b688849d80162f6(x8a41fbc87a3fb);
                                if (x0ce73f6cbd7d.x >= this.currentPoint.X && x0ce73f6cbd7d.x <= this.currentPoint.X + num && x0ce73f6cbd7d.y >= this.currentPoint.Y && x0ce73f6cbd7d.y <= this.currentPoint.Y + num2)
                                {
                                    this.xd6cfa3f3f3125e82(x0ce73f6cbd7d.x, x0ce73f6cbd7d.y);
                                    flag3 = true;
                                }
                                else
                                {
                                    x902c4aee45bfd906 x902c4aee45bfd = this.x3c6f7570c20e3f7d(x0ce73f6cbd7d, ref x8a41fbc87a3fb);
                                    if (x902c4aee45bfd != null)
                                    {
                                        x902c4aee45bfd.xd6cfa3f3f3125e82(x0ce73f6cbd7d.x, x0ce73f6cbd7d.y);
                                        flag3 = true;
                                    }
                                }
                            }
                            if (x8a41fbc87a3fb.message == 513u || x8a41fbc87a3fb.message == 519u || x8a41fbc87a3fb.message == 516u || x8a41fbc87a3fb.message == 523u || x8a41fbc87a3fb.message == 161u || x8a41fbc87a3fb.message == 167u || x8a41fbc87a3fb.message == 164u || x8a41fbc87a3fb.message == 171u)
                            {
                                x555516122dcc901e.POINT x0ce73f6cbd7d2 = this.x4b688849d80162f6(x8a41fbc87a3fb);
                                if (x0ce73f6cbd7d2.x >= this.currentPoint.X && x0ce73f6cbd7d2.x <= this.currentPoint.X + num && x0ce73f6cbd7d2.y >= this.currentPoint.Y && x0ce73f6cbd7d2.y <= this.currentPoint.Y + num2)
                                {
                                    flag3 = true;
                                }
                                else if (this.x3c6f7570c20e3f7d(x0ce73f6cbd7d2, ref x8a41fbc87a3fb) == null)
                                {
                                    if (this.x277ff37e12aff68b(x0ce73f6cbd7d2, ref x8a41fbc87a3fb))
                                    {
                                        this.parentControl.x2ea07c3ab5d970d4(x0ce73f6cbd7d2);
                                        flag3 = true;
                                    }
                                    else
                                    {
                                        this.exitLoop = true;
                                        flag = true;
                                        if (this.parentControl != null && x8a41fbc87a3fb.hwnd == this.parentControl.Handle)
                                        {
                                            flag = false;
                                        }
                                    }
                                }
                                else
                                {
                                    flag3 = true;
                                }
                            }
                            if (x8a41fbc87a3fb.message == 512u)
                            {
                                x555516122dcc901e.POINT x0ce73f6cbd7d3 = this.x4b688849d80162f6(x8a41fbc87a3fb);
                                if (x0ce73f6cbd7d3.x >= this.currentPoint.X && x0ce73f6cbd7d3.x <= this.currentPoint.X + num && x0ce73f6cbd7d3.y >= this.currentPoint.Y && x0ce73f6cbd7d3.y <= this.currentPoint.Y + num2)
                                {
                                    this.x136735fdfe6d04ea(x0ce73f6cbd7d3.x, x0ce73f6cbd7d3.y);
                                }
                                else
                                {
                                    if (this.mouseOver)
                                    {
                                        this.xa84c74b103039157();
                                    }
                                    x902c4aee45bfd906 x902c4aee45bfd2 = this.x3c6f7570c20e3f7d(x0ce73f6cbd7d3, ref x8a41fbc87a3fb);
                                    if (x902c4aee45bfd2 != null)
                                    {
                                        x902c4aee45bfd2.x136735fdfe6d04ea(x0ce73f6cbd7d3.x, x0ce73f6cbd7d3.y);
                                    }
                                    else if (this.x277ff37e12aff68b(x0ce73f6cbd7d3, ref x8a41fbc87a3fb))
                                    {
                                        this.parentControl.x136735fdfe6d04ea(x0ce73f6cbd7d3);
                                    }
                                }
                                flag3 = true;
                            }
                            if (x8a41fbc87a3fb.message == 32u)
                            {
                                this.x773209141f430902();
                                flag3 = true;
                            }
                            if (x8a41fbc87a3fb.message == 260u)
                            {
                                if ((int)x8a41fbc87a3fb.wParam == 18)
                                {
                                    this.exitLoop = true;
                                }
                                else
                                {
                                    x8a41fbc87a3fb.message = 256u;
                                }
                            }
                            if (x8a41fbc87a3fb.message == 256u)
                            {
                                int num3 = (int)x8a41fbc87a3fb.wParam;
                                if (num3 != 13)
                                {
                                    if (num3 != 27)
                                    {
                                        switch (num3)
                                        {
                                            case 37:
                                                this.x0e37e200024f43ba();
                                                break;
                                            case 38:
                                                this.x5437d760e9a6340b();
                                                break;
                                            case 39:
                                                if (this.xb2aa7fc859b3d5b7())
                                                {
                                                    flag = true;
                                                }
                                                break;
                                            case 40:
                                                this.x5b2e42a68d835b31();
                                                break;
                                            default:
                                                {
                                                    int num4 = this.x65c1cc54cc8b0e75((char)((int)x8a41fbc87a3fb.wParam));
                                                    if (num4 != -1)
                                                    {
                                                        x2cc390e9409b0f3f x2cc390e9409b0f3f = this.drawCommands[num4] as x2cc390e9409b0f3f;
                                                        this.returnCommand = x2cc390e9409b0f3f.MenuCommand;
                                                        this.exitLoop = true;
                                                        flag = true;
                                                    }
                                                    break;
                                                }
                                        }
                                    }
                                    else
                                    {
                                        this.exitLoop = true;
                                    }
                                }
                                else if (this.trackItem != -1)
                                {
                                    x2cc390e9409b0f3f x2cc390e9409b0f3f2 = this.drawCommands[this.trackItem] as x2cc390e9409b0f3f;
                                    if (x2cc390e9409b0f3f2.SubMenu)
                                    {
                                        this.x0760506d6abf77a2(this.trackItem, false);
                                        flag = true;
                                    }
                                    else if (x2cc390e9409b0f3f2.Expansion)
                                    {
                                        this.x057464a452da34b7();
                                    }
                                    else
                                    {
                                        this.returnCommand = x2cc390e9409b0f3f2.MenuCommand;
                                        this.exitLoop = true;
                                    }
                                }
                            }
                            if (x8a41fbc87a3fb.message == 256u || x8a41fbc87a3fb.message == 257u || x8a41fbc87a3fb.message == 260u || x8a41fbc87a3fb.message == 261u)
                            {
                                flag3 = true;
                            }
                            if (flag3)
                            {
                                x40255b11ef821fa3.MSG mSG = default(x40255b11ef821fa3.MSG);
                                x61467fe65a98f20c.GetMessage(ref mSG, IntPtr.Zero, 0u, 0u);
                            }
                            else if (!flag)
                            {
                                if (x61467fe65a98f20c.GetMessage(ref x8a41fbc87a3fb, IntPtr.Zero, 0u, 0u))
                                {
                                    x61467fe65a98f20c.TranslateMessage(ref x8a41fbc87a3fb);
                                    x61467fe65a98f20c.DispatchMessage(ref x8a41fbc87a3fb);
                                }
                            }
                            else
                            {
                                flag = false;
                            }
                        }
                    }
                }
                if (flag2)
                {
                    x61467fe65a98f20c.ShowCaret(IntPtr.Zero);
                }
                this.xeb515a5b77846afe(this.trackItem, -1, false, false);
                this.x4139e66e6dfaac52();
                this.DestroyHandle();
                if (this.parentMenu == null && this.returnCommand != null && this.parentControl == null)
                {
                    this.returnCommand.OnClick(EventArgs.Empty);
                }
                result = this.returnCommand;
            }
            catch (Exception)
            {
                result = null;
            }
            return result;
        }

        protected x5f4b657f68f87baa xf1c4e2176aec0ad6(bool xacc37ebdd71fcc44)
        {
            x5f4b657f68f87baa result;
            try
            {
                this.returnCommand = null;
                this.trackItem = -1;
                this.exitLoop = false;
                this.mouseOver = false;
                this.returnDir = 0;
                bool flag = false;
                this.animateFirst = true;
                this.xa1715c5212ff8a33();
                x40255b11ef821fa3.MSG x8a41fbc87a3fb = default(x40255b11ef821fa3.MSG);
                if (xacc37ebdd71fcc44)
                {
                    this.x5b2e42a68d835b31();
                }
                x61467fe65a98f20c.SetCursor(x61467fe65a98f20c.LoadCursor(IntPtr.Zero, 32512u));
                bool flag2 = x61467fe65a98f20c.HideCaret(IntPtr.Zero);
                while (!this.exitLoop)
                {
                    if (x61467fe65a98f20c.WaitMessage())
                    {
                        while (!this.exitLoop && x61467fe65a98f20c.PeekMessage(ref x8a41fbc87a3fb, 0, 0u, 0u, 0u))
                        {
                            bool flag3 = false;
                            int num = this.currentSize.Width - x902c4aee45bfd906._position[0, 16];
                            int num2 = this.currentSize.Height - x902c4aee45bfd906._position[0, 17];
                            if (x8a41fbc87a3fb.message == 514u || x8a41fbc87a3fb.message == 520u || x8a41fbc87a3fb.message == 517u || x8a41fbc87a3fb.message == 524u || x8a41fbc87a3fb.message == 162u || x8a41fbc87a3fb.message == 168u || x8a41fbc87a3fb.message == 165u || x8a41fbc87a3fb.message == 172u)
                            {
                                x555516122dcc901e.POINT x0ce73f6cbd7d = this.x4b688849d80162f6(x8a41fbc87a3fb);
                                if (x0ce73f6cbd7d.x >= this.currentPoint.X && x0ce73f6cbd7d.x <= this.currentPoint.X + num && x0ce73f6cbd7d.y >= this.currentPoint.Y && x0ce73f6cbd7d.y <= this.currentPoint.Y + num2)
                                {
                                    this.xd6cfa3f3f3125e82(x0ce73f6cbd7d.x, x0ce73f6cbd7d.y);
                                    flag3 = true;
                                }
                                else
                                {
                                    x902c4aee45bfd906 x902c4aee45bfd = this.x3c6f7570c20e3f7d(x0ce73f6cbd7d, ref x8a41fbc87a3fb);
                                    if (x902c4aee45bfd != null)
                                    {
                                        x902c4aee45bfd.xd6cfa3f3f3125e82(x0ce73f6cbd7d.x, x0ce73f6cbd7d.y);
                                        flag3 = true;
                                    }
                                }
                            }
                            if (x8a41fbc87a3fb.message == 513u || x8a41fbc87a3fb.message == 519u || x8a41fbc87a3fb.message == 516u || x8a41fbc87a3fb.message == 523u || x8a41fbc87a3fb.message == 161u || x8a41fbc87a3fb.message == 167u || x8a41fbc87a3fb.message == 164u || x8a41fbc87a3fb.message == 171u)
                            {
                                x555516122dcc901e.POINT x0ce73f6cbd7d2 = this.x4b688849d80162f6(x8a41fbc87a3fb);
                                if (x0ce73f6cbd7d2.x >= this.currentPoint.X && x0ce73f6cbd7d2.x <= this.currentPoint.X + num && x0ce73f6cbd7d2.y >= this.currentPoint.Y && x0ce73f6cbd7d2.y <= this.currentPoint.Y + num2)
                                {
                                    flag3 = true;
                                }
                                else if (this.x3c6f7570c20e3f7d(x0ce73f6cbd7d2, ref x8a41fbc87a3fb) == null)
                                {
                                    if (this.x277ff37e12aff68b(x0ce73f6cbd7d2, ref x8a41fbc87a3fb))
                                    {
                                        this.parentControl.x2ea07c3ab5d970d4(x0ce73f6cbd7d2);
                                        flag3 = true;
                                    }
                                    else
                                    {
                                        this.exitLoop = true;
                                        flag = true;
                                        if (this.parentControl != null && x8a41fbc87a3fb.hwnd == this.parentControl.Handle)
                                        {
                                            flag = false;
                                        }
                                    }
                                }
                                else
                                {
                                    flag3 = true;
                                }
                            }
                            if (x8a41fbc87a3fb.message == 512u)
                            {
                                x555516122dcc901e.POINT x0ce73f6cbd7d3 = this.x4b688849d80162f6(x8a41fbc87a3fb);
                                if (x0ce73f6cbd7d3.x >= this.currentPoint.X && x0ce73f6cbd7d3.x <= this.currentPoint.X + num && x0ce73f6cbd7d3.y >= this.currentPoint.Y && x0ce73f6cbd7d3.y <= this.currentPoint.Y + num2)
                                {
                                    this.x136735fdfe6d04ea(x0ce73f6cbd7d3.x, x0ce73f6cbd7d3.y);
                                }
                                else
                                {
                                    if (this.mouseOver)
                                    {
                                        this.xa84c74b103039157();
                                    }
                                    x902c4aee45bfd906 x902c4aee45bfd2 = this.x3c6f7570c20e3f7d(x0ce73f6cbd7d3, ref x8a41fbc87a3fb);
                                    if (x902c4aee45bfd2 != null)
                                    {
                                        x902c4aee45bfd2.x136735fdfe6d04ea(x0ce73f6cbd7d3.x, x0ce73f6cbd7d3.y);
                                    }
                                    else if (this.x277ff37e12aff68b(x0ce73f6cbd7d3, ref x8a41fbc87a3fb))
                                    {
                                        this.parentControl.x136735fdfe6d04ea(x0ce73f6cbd7d3);
                                    }
                                }
                                flag3 = true;
                            }
                            if (x8a41fbc87a3fb.message == 32u)
                            {
                                this.x773209141f430902();
                                flag3 = true;
                            }
                            if (x8a41fbc87a3fb.message == 260u)
                            {
                                if ((int)x8a41fbc87a3fb.wParam == 18)
                                {
                                    this.exitLoop = true;
                                }
                                else
                                {
                                    x8a41fbc87a3fb.message = 256u;
                                }
                            }
                            if (x8a41fbc87a3fb.message == 256u)
                            {
                                int num3 = (int)x8a41fbc87a3fb.wParam;
                                if (num3 != 13)
                                {
                                    if (num3 != 27)
                                    {
                                        switch (num3)
                                        {
                                            case 37:
                                                this.x0e37e200024f43ba();
                                                break;
                                            case 38:
                                                this.x5437d760e9a6340b();
                                                break;
                                            case 39:
                                                if (this.xb2aa7fc859b3d5b7())
                                                {
                                                    flag = true;
                                                }
                                                break;
                                            case 40:
                                                this.x5b2e42a68d835b31();
                                                break;
                                            default:
                                                {
                                                    int num4 = this.x65c1cc54cc8b0e75((char)((int)x8a41fbc87a3fb.wParam));
                                                    if (num4 != -1)
                                                    {
                                                        x2cc390e9409b0f3f x2cc390e9409b0f3f = this.drawCommands[num4] as x2cc390e9409b0f3f;
                                                        this.returnCommand = x2cc390e9409b0f3f.MenuCommand;
                                                        this.exitLoop = true;
                                                        flag = true;
                                                    }
                                                    break;
                                                }
                                        }
                                    }
                                    else
                                    {
                                        this.exitLoop = true;
                                    }
                                }
                                else if (this.trackItem != -1)
                                {
                                    x2cc390e9409b0f3f x2cc390e9409b0f3f2 = this.drawCommands[this.trackItem] as x2cc390e9409b0f3f;
                                    if (x2cc390e9409b0f3f2.SubMenu)
                                    {
                                        this.x0760506d6abf77a2(this.trackItem, false);
                                        flag = true;
                                    }
                                    else if (x2cc390e9409b0f3f2.Expansion)
                                    {
                                        this.x057464a452da34b7();
                                    }
                                    else
                                    {
                                        this.returnCommand = x2cc390e9409b0f3f2.MenuCommand;
                                        this.exitLoop = true;
                                    }
                                }
                            }
                            if (x8a41fbc87a3fb.message == 256u || x8a41fbc87a3fb.message == 257u || x8a41fbc87a3fb.message == 260u || x8a41fbc87a3fb.message == 261u)
                            {
                                flag3 = true;
                            }
                            if (flag3)
                            {
                                x40255b11ef821fa3.MSG mSG = default(x40255b11ef821fa3.MSG);
                                x61467fe65a98f20c.GetMessage(ref mSG, IntPtr.Zero, 0u, 0u);
                            }
                            else if (!flag)
                            {
                                if (x61467fe65a98f20c.GetMessage(ref x8a41fbc87a3fb, IntPtr.Zero, 0u, 0u))
                                {
                                    x61467fe65a98f20c.TranslateMessage(ref x8a41fbc87a3fb);
                                    x61467fe65a98f20c.DispatchMessage(ref x8a41fbc87a3fb);
                                }
                            }
                            else
                            {
                                flag = false;
                            }
                        }
                    }
                }
                if (flag2)
                {
                    x61467fe65a98f20c.ShowCaret(IntPtr.Zero);
                }
                this.xeb515a5b77846afe(this.trackItem, -1, false, false);
                this.x4139e66e6dfaac52();
                this.DestroyHandle();
                if (this.parentMenu == null && this.returnCommand != null && this.parentControl == null)
                {
                    this.returnCommand.OnClick(EventArgs.Empty);
                }
                result = this.returnCommand;
            }
            catch (Exception)
            {
                result = null;
            }
            return result;
        }

        public void Dismiss()
        {
            if (base.Handle != IntPtr.Zero)
            {
                this.timer.Stop();
                if (this.childMenu != null)
                {
                    this.childMenu.Dismiss();
                }
                this.exitLoop = true;
                this.x4139e66e6dfaac52();
                x61467fe65a98f20c.PostMessage(base.Handle, this.WM_DISMISS, 0u, 0u);
            }
        }

        protected void x4139e66e6dfaac52()
        {
            x61467fe65a98f20c.ShowWindow(base.Handle, 0);
        }

        protected void xf1420593eaecc724(bool x789c645a15deb49b)
        {
            this.x9119fce83e4631ad(0);
            x61467fe65a98f20c.ShowWindow(base.Handle, 4);
            int num = this.animateTime / x902c4aee45bfd906.blendSteps;
            for (int i = 0; i < x902c4aee45bfd906.blendSteps; i++)
            {
                byte x6ad505c7ef981b0e = (byte)(63 + 192 / x902c4aee45bfd906.blendSteps * (i + 1));
                DateTime now = DateTime.Now;
                this.x9119fce83e4631ad(x6ad505c7ef981b0e);
                TimeSpan timeSpan = DateTime.Now.Subtract(now);
                if (this.animateTime > 0 && timeSpan.Milliseconds < num)
                {
                    Thread.Sleep(num - timeSpan.Milliseconds);
                }
            }
        }

        protected void xb3525a0e7d2fd376()
        {
            this.layered = x902c4aee45bfd906.supportsLayered;
            Size x001544edc57babc = this.x4e2849ed170f5bc2();
            Point point = this.x6e375ee3ca34b13b(x001544edc57babc);
            if (this.menuCommands.Count == 0)
            {
                x001544edc57babc = new Size(0, 0);
            }
            CreateParams createParams = new CreateParams();
            createParams.Caption = "NativeSkinContextMenu";
            createParams.X = point.X;
            createParams.Y = point.Y;
            createParams.Height = x001544edc57babc.Height;
            createParams.Width = x001544edc57babc.Width;
            createParams.Parent = IntPtr.Zero;
            createParams.Style = -2147483648;
            createParams.ExStyle = 136;
            if (this.layered)
            {
                createParams.ExStyle += 524288;
            }
            this.CreateHandle(createParams);
            if (!this.layered)
            {
                this.x1803d70928e3ae9b(x001544edc57babc);
            }
            this.currentSize = x001544edc57babc;
            this.currentPoint = point;
            bool flag = false;
            if (this.layered)
            {
                this.x9119fce83e4631ad();
                bool flag2 = false;
                switch (this.animateItem)
                {
                    case x6fd23f8bad2f3ced.No:
                        flag2 = false;
                        break;
                    case x6fd23f8bad2f3ced.Yes:
                        flag2 = true;
                        break;
                    case x6fd23f8bad2f3ced.System:
                        {
                            int num = 0;
                            x61467fe65a98f20c.SystemParametersInfoA(4098u, 0u, ref num, 0u);
                            flag2 = (num != 0);
                            break;
                        }
                }
                if (flag2 && this.animateIn)
                {
                    uint num2 = (uint)this.animateStyle;
                    if (this.animateStyle == x1f5697535eab37b9.System)
                    {
                        int num3 = 0;
                        x61467fe65a98f20c.SystemParametersInfoA(4114u, 0u, ref num3, 0u);
                        if (num3 != 0)
                        {
                            num2 = 524288u;
                        }
                        else
                        {
                            num2 = 262149u;
                        }
                    }
                    if ((num2 & 524288u) != 0u)
                    {
                        this.xf1420593eaecc724(true);
                    }
                    else
                    {
                        x61467fe65a98f20c.AnimateWindow(base.Handle, (uint)this.animateTime, num2);
                    }
                    flag = true;
                }
            }
            if (!flag)
            {
                x61467fe65a98f20c.ShowWindow(base.Handle, 4);
            }
        }

        protected void xa1715c5212ff8a33()
        {
            this.layered = x902c4aee45bfd906.supportsLayered;
            Size x001544edc57babc = this.x4e2849ed170f5bc2();
            Point point = this.x1339f634e34e0e03(x001544edc57babc);
            if (this.menuCommands.Count == 0)
            {
                x001544edc57babc = new Size(0, 0);
            }
            CreateParams createParams = new CreateParams();
            createParams.Caption = "NativeSkinContextMenu";
            createParams.X = point.X;
            createParams.Y = point.Y;
            createParams.Height = x001544edc57babc.Height;
            createParams.Width = x001544edc57babc.Width;
            createParams.Parent = IntPtr.Zero;
            createParams.Style = -2147483648;
            createParams.ExStyle = 136;
            if (this.layered)
            {
                createParams.ExStyle += 524288;
            }
            this.CreateHandle(createParams);
            if (!this.layered)
            {
                this.x1803d70928e3ae9b(x001544edc57babc);
            }
            this.currentSize = x001544edc57babc;
            this.currentPoint = point;
            bool flag = false;
            if (this.layered)
            {
                this.x9119fce83e4631ad();
                bool flag2 = false;
                switch (this.animateItem)
                {
                    case x6fd23f8bad2f3ced.No:
                        flag2 = false;
                        break;
                    case x6fd23f8bad2f3ced.Yes:
                        flag2 = true;
                        break;
                    case x6fd23f8bad2f3ced.System:
                        {
                            int num = 0;
                            x61467fe65a98f20c.SystemParametersInfoA(4098u, 0u, ref num, 0u);
                            flag2 = (num != 0);
                            break;
                        }
                }
                if (flag2 && this.animateIn)
                {
                    uint num2 = (uint)this.animateStyle;
                    if (this.animateStyle == x1f5697535eab37b9.System)
                    {
                        int num3 = 0;
                        x61467fe65a98f20c.SystemParametersInfoA(4114u, 0u, ref num3, 0u);
                        if (num3 != 0)
                        {
                            num2 = 524288u;
                        }
                        else
                        {
                            num2 = 262149u;
                        }
                    }
                    if ((num2 & 524288u) != 0u)
                    {
                        this.xf1420593eaecc724(true);
                    }
                    else
                    {
                        x61467fe65a98f20c.AnimateWindow(base.Handle, (uint)this.animateTime, num2);
                    }
                    flag = true;
                }
            }
            if (!flag)
            {
                x61467fe65a98f20c.ShowWindow(base.Handle, 4);
            }
        }

        protected void x9119fce83e4631ad()
        {
            this.x9119fce83e4631ad(this.currentPoint, this.currentSize, 255);
        }

        protected void x9119fce83e4631ad(byte x6ad505c7ef981b0e)
        {
            this.x9119fce83e4631ad(this.currentPoint, this.currentSize, x6ad505c7ef981b0e);
        }

        protected void x9119fce83e4631ad(Point x2f7096dac971d6ec, Size x0ceec69a97f73617, byte x6ad505c7ef981b0e)
        {
            Bitmap bitmap = new Bitmap(x0ceec69a97f73617.Width, x0ceec69a97f73617.Height, PixelFormat.Format32bppArgb);
            using (Graphics graphics = Graphics.FromImage(bitmap))
            {
                Rectangle x3821770539095b = new Rectangle(0, 0, x0ceec69a97f73617.Width, x0ceec69a97f73617.Height);
                this.x4af0f9e5d7ac184a(graphics, x3821770539095b);
                this.x05a10b378cdf8119(graphics);
                IntPtr dC = x61467fe65a98f20c.GetDC(IntPtr.Zero);
                IntPtr intPtr = x31775329b2a4ff52.CreateCompatibleDC(dC);
                IntPtr hbitmap = bitmap.GetHbitmap(Color.FromArgb(0));
                IntPtr hObject = x31775329b2a4ff52.SelectObject(intPtr, hbitmap);
                x555516122dcc901e.SIZE sIZE;
                sIZE.cx = x0ceec69a97f73617.Width;
                sIZE.cy = x0ceec69a97f73617.Height;
                x555516122dcc901e.POINT pOINT;
                pOINT.x = x2f7096dac971d6ec.X;
                pOINT.y = x2f7096dac971d6ec.Y;
                x555516122dcc901e.POINT pOINT2;
                pOINT2.x = 0;
                pOINT2.y = 0;
                x1439a41cfa24189f.BLENDFUNCTION bLENDFUNCTION = default(x1439a41cfa24189f.BLENDFUNCTION);
                bLENDFUNCTION.BlendOp = 0;
                bLENDFUNCTION.BlendFlags = 0;
                bLENDFUNCTION.SourceConstantAlpha = x6ad505c7ef981b0e;
                bLENDFUNCTION.AlphaFormat = 1;
                x61467fe65a98f20c.UpdateLayeredWindow(base.Handle, dC, ref pOINT, ref sIZE, intPtr, ref pOINT2, 0, ref bLENDFUNCTION, 2);
                x31775329b2a4ff52.SelectObject(intPtr, hObject);
                x61467fe65a98f20c.ReleaseDC(IntPtr.Zero, dC);
                x31775329b2a4ff52.DeleteObject(hbitmap);
                x31775329b2a4ff52.DeleteDC(intPtr);
            }
        }

        protected void x1803d70928e3ae9b(Size x001544edc57babc2)
        {
            int num = x902c4aee45bfd906._position[0, 17];
            int num2 = x902c4aee45bfd906._position[0, 16];
            Region region = new Region();
            region.MakeInfinite();
            region.Xor(new Rectangle(x001544edc57babc2.Width - num2, 0, num2, num));
            if (this.direction != xdbfa333b4cd503e0.Vertical || this.excludeTop)
            {
                region.Xor(new Rectangle(0, x001544edc57babc2.Height - num, num2, num));
            }
            using (Graphics graphics = Graphics.FromHwnd(base.Handle))
            {
                x61467fe65a98f20c.SetWindowRgn(base.Handle, region.GetHrgn(graphics), false);
            }
        }

        protected Point x1339f634e34e0e03(Size x001544edc57babc2)
        {
            Screen.GetWorkingArea(this.screenPosition);
            Point result = this.screenPosition;
            this.excludeTop = true;
            this.excludeOffset = 0;
            x001544edc57babc2.Width -= x902c4aee45bfd906._position[0, 16];
            result.Y = result.Y - x001544edc57babc2.Height + 3;
            return result;
        }

        protected Point x6e375ee3ca34b13b(Size x001544edc57babc2)
        {
            Rectangle workingArea = Screen.GetWorkingArea(this.screenPosition);
            Point result = this.screenPosition;
            int arg_1A_0 = workingArea.Width;
            int left = workingArea.Left;
            int right = workingArea.Right;
            int arg_32_0 = workingArea.Height;
            int bottom = workingArea.Bottom;
            int top = workingArea.Top;
            this.excludeTop = true;
            this.excludeOffset = 0;
            x001544edc57babc2.Width -= x902c4aee45bfd906._position[0, 16];
            if (this.popupDown)
            {
                if (result.Y + x001544edc57babc2.Height > bottom)
                {
                    if (this.parentControl != null && this.parentMenu == null && this.aboveScreenPosition.Y - x001544edc57babc2.Height > top)
                    {
                        result.Y = this.aboveScreenPosition.Y - x001544edc57babc2.Height;
                        this.popupDown = false;
                        this.excludeTop = false;
                        this.parentControl.xd502a472a14a9c04();
                    }
                    if (result.Y + x001544edc57babc2.Height > bottom)
                    {
                        if (this.parentMenu != null)
                        {
                            this.popupDown = false;
                            if (this.aboveScreenPosition.Y - x001544edc57babc2.Height > top)
                            {
                                result.Y = this.aboveScreenPosition.Y - x001544edc57babc2.Height;
                            }
                            else
                            {
                                result.Y = top;
                            }
                        }
                        else
                        {
                            result.Y = bottom - x001544edc57babc2.Height - 1;
                        }
                    }
                }
            }
            else if (result.Y - x001544edc57babc2.Height < top)
            {
                this.popupDown = true;
                if (result.Y + x001544edc57babc2.Height > bottom)
                {
                    result.Y = bottom - x001544edc57babc2.Height - 1;
                }
            }
            else
            {
                result.Y -= x001544edc57babc2.Height;
            }
            if (this.popupRight)
            {
                if (result.X + x001544edc57babc2.Width > right)
                {
                    if (this.parentMenu != null)
                    {
                        this.popupRight = false;
                        result.X = this.leftScreenPosition.X - x001544edc57babc2.Width;
                        if (result.X < left)
                        {
                            result.X = left;
                        }
                    }
                    else
                    {
                        int num = right - x001544edc57babc2.Width - 1;
                        this.excludeOffset = result.X - num;
                        result.X = num;
                    }
                }
            }
            else
            {
                result.X = this.leftScreenPosition.X;
                if (result.X - x001544edc57babc2.Width < left)
                {
                    this.popupRight = true;
                    if (this.screenPosition.X + x001544edc57babc2.Width > right)
                    {
                        result.X = right - x001544edc57babc2.Width - 1;
                    }
                    else
                    {
                        result.X = this.screenPosition.X;
                    }
                }
                else
                {
                    result.X -= x001544edc57babc2.Width;
                }
            }
            return result;
        }

        protected void x057464a452da34b7()
        {
            this.drawCommands.Clear();
            this.showInfrequent = true;
            if (this.rememberExpansion)
            {
                this.menuCommands.ShowInfrequent = true;
            }
            Size x001544edc57babc = this.x4e2849ed170f5bc2();
            Point point = this.x6e375ee3ca34b13b(x001544edc57babc);
            this.currentPoint = point;
            this.currentSize = x001544edc57babc;
            if (!this.layered)
            {
                this.x1803d70928e3ae9b(x001544edc57babc);
                x61467fe65a98f20c.MoveWindow(base.Handle, point.X, point.Y, x001544edc57babc.Width, x001544edc57babc.Height, true);
                xae4dd1cafd2eb77c.RECT rECT = default(xae4dd1cafd2eb77c.RECT);
                rECT.left = 0;
                rECT.top = 0;
                rECT.right = x001544edc57babc.Width;
                rECT.bottom = x001544edc57babc.Height;
                x61467fe65a98f20c.InvalidateRect(base.Handle, ref rECT, true);
                return;
            }
            this.x9119fce83e4631ad();
            this.x95f5f55ee201fb8f();
        }

        protected Size x4e2849ed170f5bc2()
        {
            this.drawCommands = new ArrayList();
            int num = x902c4aee45bfd906._position[0, 4] + x902c4aee45bfd906.imageHeight + x902c4aee45bfd906._position[0, 6];
            int num2 = x902c4aee45bfd906._position[0, 5] + x902c4aee45bfd906.imageWidth + x902c4aee45bfd906._position[0, 7] + x902c4aee45bfd906._position[0, 8] + x902c4aee45bfd906._position[0, 9] + x902c4aee45bfd906._position[0, 10] + x902c4aee45bfd906._position[0, 11] + x902c4aee45bfd906._position[0, 12];
            int num3 = this.textFont.Height;
            if (num3 < num)
            {
                num3 = num;
            }
            int height = SystemInformation.WorkingArea.Height;
            int num4 = x902c4aee45bfd906._position[0, 1];
            int num5 = x902c4aee45bfd906._position[0, 0];
            int num6 = num5;
            int num7 = num2;
            int num8 = 0;
            int num9 = 0;
            int num10 = 0;
            int num11 = 0;
            bool flag = false;
            bool flag2 = false;
            IntPtr dC = x61467fe65a98f20c.GetDC(IntPtr.Zero);
            ArrayList arrayList = new ArrayList();
            using (Graphics graphics = Graphics.FromHdc(dC))
            {
                if (this.menuCommands.ExtraText.Length > 0)
                {
                    int num12 = (int)graphics.MeasureString(this.menuCommands.ExtraText, this.menuCommands.ExtraFont).Height + 1;
                    this.extraSize = num12 + x902c4aee45bfd906._position[0, 20] + x902c4aee45bfd906._position[0, 18] * 2;
                    num4 += this.extraSize;
                    num8 = this.extraSize;
                }
                foreach (x5f4b657f68f87baa x5f4b657f68f87baa in this.menuCommands)
                {
                    x5f4b657f68f87baa.OnUpdate(EventArgs.Empty);
                    if (x5f4b657f68f87baa.Visible && (x5f4b657f68f87baa.MenuCommands.Count <= 0 || x5f4b657f68f87baa.MenuCommands.VisibleItems()))
                    {
                        if (x5f4b657f68f87baa.Infrequent && !this.showInfrequent)
                        {
                            flag = true;
                        }
                        else
                        {
                            if (x5f4b657f68f87baa.Break)
                            {
                                num10 = 0;
                                num11++;
                                this.xfbe050c2d5b2be39(arrayList, num7);
                                num4 += num7;
                                int num13 = x902c4aee45bfd906._position[0, 14];
                                x2cc390e9409b0f3f value = new x2cc390e9409b0f3f(new Rectangle(num4, 0, num13, 0), false);
                                this.drawCommands.Add(value);
                                num4 += num13;
                                num6 = num5;
                                num8 += num7 + num13;
                                num7 = num2;
                            }
                            int num14;
                            int num15;
                            if (x5f4b657f68f87baa.Text == "-")
                            {
                                num14 = num2;
                                num15 = x902c4aee45bfd906._position[0, 13];
                            }
                            else
                            {
                                num15 = num3;
                                num14 = num2 + (int)graphics.MeasureString(x5f4b657f68f87baa.Text, this.textFont).Width + 1;
                                if (x5f4b657f68f87baa.Shortcut != Shortcut.None)
                                {
                                    SizeF sizeF = graphics.MeasureString(this.x8cf43133d25d686a(x5f4b657f68f87baa.Shortcut), this.textFont);
                                    num14 += x902c4aee45bfd906._position[0, 15] + (int)sizeF.Width + 1;
                                }
                            }
                            if (num6 + num15 >= height)
                            {
                                num10 = 0;
                                num11++;
                                this.xfbe050c2d5b2be39(arrayList, num7);
                                num4 += num7;
                                int num16 = x902c4aee45bfd906._position[0, 14];
                                x2cc390e9409b0f3f x2cc390e9409b0f3f = new x2cc390e9409b0f3f(new Rectangle(num4, num5, num16, 0), false);
                                this.drawCommands.Add(x2cc390e9409b0f3f);
                                num4 += num16;
                                num6 = num5;
                                num8 += num7 + num16;
                                num7 = num2;
                                x2cc390e9409b0f3f.Infrequent = flag2;
                            }
                            Rectangle drawRect = new Rectangle(num4, num6, num14, num15);
                            x2cc390e9409b0f3f x2cc390e9409b0f3f2 = new x2cc390e9409b0f3f(x5f4b657f68f87baa, drawRect, num10, num11);
                            if (flag2 != x5f4b657f68f87baa.Infrequent)
                            {
                                if (x5f4b657f68f87baa.Infrequent)
                                {
                                    x2cc390e9409b0f3f2.TopBorder = true;
                                }
                                else if (this.drawCommands.Count > 0)
                                {
                                    for (int i = this.drawCommands.Count - 1; i >= 0; i--)
                                    {
                                        if (!(this.drawCommands[i] as x2cc390e9409b0f3f).Separator)
                                        {
                                            (this.drawCommands[i] as x2cc390e9409b0f3f).BottomBorder = true;
                                            break;
                                        }
                                    }
                                }
                            }
                            if (!x2cc390e9409b0f3f2.Separator)
                            {
                                flag2 = x5f4b657f68f87baa.Infrequent;
                            }
                            this.drawCommands.Add(x2cc390e9409b0f3f2);
                            arrayList.Add(x2cc390e9409b0f3f2);
                            if (num14 > num7)
                            {
                                num7 = num14;
                            }
                            num6 += num15;
                            if (num6 > num9)
                            {
                                num9 = num6;
                            }
                            num10++;
                        }
                    }
                }
                if (flag)
                {
                    Rectangle drawRect2 = new Rectangle(num4, num6, num2, num);
                    x2cc390e9409b0f3f value2 = new x2cc390e9409b0f3f(drawRect2, true);
                    this.drawCommands.Add(value2);
                    arrayList.Add(value2);
                    num6 += num;
                    if (num6 > num9)
                    {
                        num9 = num6;
                    }
                }
                this.xfbe050c2d5b2be39(arrayList, num7);
            }
            x61467fe65a98f20c.ReleaseDC(IntPtr.Zero, dC);
            int num17 = x902c4aee45bfd906._position[0, 1] + num8 + num7 + x902c4aee45bfd906._position[0, 3];
            int num18 = x902c4aee45bfd906._position[0, 0] + num9 + x902c4aee45bfd906._position[0, 2];
            this.x5796c1a518d308ac(num9);
            int num19 = x902c4aee45bfd906._position[0, 17];
            int num20 = x902c4aee45bfd906._position[0, 16];
            return new Size(num17 + num19, num18 + num20);
        }

        protected Color x898c3842f8dd56e5(Color xfe9df16f7e7c346d, Color x753506715ee62862, int x6ad505c7ef981b0e)
        {
            Color color = Color.FromArgb(255, xfe9df16f7e7c346d);
            Color color2 = Color.FromArgb(255, x753506715ee62862);
            float num = (float)color.R;
            float num2 = (float)color.G;
            float num3 = (float)color.B;
            float num4 = (float)color2.R;
            float num5 = (float)color2.G;
            float num6 = (float)color2.B;
            float num7 = num * (float)x6ad505c7ef981b0e / 255f + num4 * ((float)(255 - x6ad505c7ef981b0e) / 255f);
            float num8 = num2 * (float)x6ad505c7ef981b0e / 255f + num5 * ((float)(255 - x6ad505c7ef981b0e) / 255f);
            float num9 = num3 * (float)x6ad505c7ef981b0e / 255f + num6 * ((float)(255 - x6ad505c7ef981b0e) / 255f);
            byte red = (byte)num7;
            byte green = (byte)num8;
            byte blue = (byte)num9;
            return Color.FromArgb(255, (int)red, (int)green, (int)blue);
        }

        protected void x5796c1a518d308ac(int x3b2a52f70523c25a)
        {
            foreach (x2cc390e9409b0f3f x2cc390e9409b0f3f in this.drawCommands)
            {
                if (x2cc390e9409b0f3f.VerticalSeparator)
                {
                    Rectangle drawRect = x2cc390e9409b0f3f.DrawRect;
                    x2cc390e9409b0f3f.DrawRect = new Rectangle(drawRect.Left, drawRect.Top, drawRect.Width, x3b2a52f70523c25a);
                }
            }
        }

        protected void xfbe050c2d5b2be39(ArrayList x607ab5024f65c83d, int xc529a6810c5d3b0c)
        {
            foreach (x2cc390e9409b0f3f x2cc390e9409b0f3f in x607ab5024f65c83d)
            {
                Rectangle drawRect = x2cc390e9409b0f3f.DrawRect;
                x2cc390e9409b0f3f.DrawRect = new Rectangle(drawRect.Left, drawRect.Top, xc529a6810c5d3b0c, drawRect.Height);
            }
            x607ab5024f65c83d.Clear();
        }

        protected void x95f5f55ee201fb8f()
        {
            xae4dd1cafd2eb77c.RECT rECT = default(xae4dd1cafd2eb77c.RECT);
            x61467fe65a98f20c.GetWindowRect(base.Handle, ref rECT);
            Rectangle x3821770539095b = new Rectangle(0, 0, rECT.right - rECT.left, rECT.bottom - rECT.top);
            using (Graphics graphics = Graphics.FromHwnd(base.Handle))
            {
                this.x4af0f9e5d7ac184a(graphics, x3821770539095b);
                this.x05a10b378cdf8119(graphics);
            }
        }

        protected string x8cf43133d25d686a(Shortcut xc0de55fd2ca182a4)
        {
            char c = (char)(xc0de55fd2ca182a4 & (Shortcut)65535);
            if (c >= '0' && c <= '9')
            {
                string text = "";
                int num = (int)((long)xc0de55fd2ca182a4 & (long)(-65536));
                if ((num & 65536) != 0)
                {
                    text += "Shift+";
                }
                if ((num & 131072) != 0)
                {
                    text += "Ctrl+";
                }
                if ((num & 262144) != 0)
                {
                    text += "Alt+";
                }
                return text + c;
            }
            return TypeDescriptor.GetConverter(typeof(Keys)).ConvertToString((Keys)xc0de55fd2ca182a4);
        }

        protected bool x5437d760e9a6340b()
        {
            int num = this.trackItem;
            int num2 = num;
            for (int i = 0; i < this.drawCommands.Count; i++)
            {
                num--;
                if (num == num2)
                {
                    return false;
                }
                if (num < 0)
                {
                    num = this.drawCommands.Count - 1;
                }
                x2cc390e9409b0f3f x2cc390e9409b0f3f = this.drawCommands[num] as x2cc390e9409b0f3f;
                if (!x2cc390e9409b0f3f.Separator && x2cc390e9409b0f3f.Enabled && num != this.trackItem)
                {
                    this.xeb515a5b77846afe(this.trackItem, num, false, false);
                    return true;
                }
            }
            return false;
        }

        protected bool x5b2e42a68d835b31()
        {
            int num = this.trackItem;
            for (int i = 0; i < this.drawCommands.Count; i++)
            {
                num++;
                if (num >= this.drawCommands.Count)
                {
                    num = 0;
                }
                x2cc390e9409b0f3f x2cc390e9409b0f3f = this.drawCommands[num] as x2cc390e9409b0f3f;
                if (!x2cc390e9409b0f3f.Separator && x2cc390e9409b0f3f.Enabled && num != this.trackItem)
                {
                    this.xeb515a5b77846afe(this.trackItem, num, false, false);
                    return true;
                }
            }
            return false;
        }

        protected void x0e37e200024f43ba()
        {
            bool flag = this.parentMenu != null || this.parentControl != null;
            bool flag2 = false;
            if (this.trackItem != -1)
            {
                x2cc390e9409b0f3f x2cc390e9409b0f3f = this.drawCommands[this.trackItem] as x2cc390e9409b0f3f;
                int col = x2cc390e9409b0f3f.Col;
                int row = x2cc390e9409b0f3f.Row;
                if (col > 0)
                {
                    int x169940ac3300e3e = -1;
                    int num = -1;
                    int num2 = col - 1;
                    x2cc390e9409b0f3f x2cc390e9409b0f3f2 = null;
                    for (int i = 0; i < this.drawCommands.Count; i++)
                    {
                        x2cc390e9409b0f3f x2cc390e9409b0f3f3 = this.drawCommands[i] as x2cc390e9409b0f3f;
                        if (x2cc390e9409b0f3f3.Col == num2 && x2cc390e9409b0f3f3.Row <= row && x2cc390e9409b0f3f3.Row > num && !x2cc390e9409b0f3f3.Separator && x2cc390e9409b0f3f3.Enabled)
                        {
                            num = x2cc390e9409b0f3f3.Row;
                            x2cc390e9409b0f3f2 = x2cc390e9409b0f3f3;
                            x169940ac3300e3e = i;
                        }
                    }
                    if (x2cc390e9409b0f3f2 != null)
                    {
                        this.xeb515a5b77846afe(this.trackItem, x169940ac3300e3e, false, false);
                    }
                    else
                    {
                        flag2 = true;
                    }
                }
                else
                {
                    flag2 = true;
                }
            }
            else if (this.parentMenu != null)
            {
                if (!this.x5437d760e9a6340b())
                {
                    flag2 = true;
                }
            }
            else
            {
                flag2 = true;
            }
            if (flag && flag2)
            {
                this.returnCommand = null;
                this.timer.Stop();
                this.exitLoop = true;
                if (this.parentMenu == null)
                {
                    this.returnDir = -1;
                }
            }
        }

        protected bool xb2aa7fc859b3d5b7()
        {
            bool flag = this.parentControl != null;
            bool flag2 = false;
            bool result = false;
            if (this.trackItem != -1)
            {
                x2cc390e9409b0f3f x2cc390e9409b0f3f = this.drawCommands[this.trackItem] as x2cc390e9409b0f3f;
                if (x2cc390e9409b0f3f.SubMenu)
                {
                    this.x0760506d6abf77a2(this.trackItem, true);
                    result = true;
                }
                else
                {
                    int col = x2cc390e9409b0f3f.Col;
                    int row = x2cc390e9409b0f3f.Row;
                    int x169940ac3300e3e = -1;
                    int num = -1;
                    int num2 = col + 1;
                    x2cc390e9409b0f3f x2cc390e9409b0f3f2 = null;
                    for (int i = 0; i < this.drawCommands.Count; i++)
                    {
                        x2cc390e9409b0f3f x2cc390e9409b0f3f3 = this.drawCommands[i] as x2cc390e9409b0f3f;
                        if (x2cc390e9409b0f3f3.Col == num2 && x2cc390e9409b0f3f3.Row <= row && x2cc390e9409b0f3f3.Row > num && !x2cc390e9409b0f3f3.Separator && x2cc390e9409b0f3f3.Enabled)
                        {
                            num = x2cc390e9409b0f3f3.Row;
                            x2cc390e9409b0f3f2 = x2cc390e9409b0f3f3;
                            x169940ac3300e3e = i;
                        }
                    }
                    if (x2cc390e9409b0f3f2 != null)
                    {
                        this.xeb515a5b77846afe(this.trackItem, x169940ac3300e3e, false, false);
                    }
                    else
                    {
                        flag2 = true;
                    }
                }
            }
            else if (this.parentMenu != null)
            {
                if (!this.x5b2e42a68d835b31())
                {
                    flag2 = true;
                }
            }
            else
            {
                flag2 = true;
            }
            if (flag && flag2)
            {
                this.returnCommand = null;
                this.timer.Stop();
                this.exitLoop = true;
                this.returnDir = 1;
            }
            return result;
        }

        protected int x65c1cc54cc8b0e75(char xba08ce632055a1d9)
        {
            int i = 0;
            while (i < this.drawCommands.Count)
            {
                x2cc390e9409b0f3f x2cc390e9409b0f3f = this.drawCommands[i] as x2cc390e9409b0f3f;
                if (x2cc390e9409b0f3f.Enabled && xba08ce632055a1d9 == x2cc390e9409b0f3f.Mnemonic)
                {
                    if (x2cc390e9409b0f3f.SubMenu)
                    {
                        if (this.trackItem != i)
                        {
                            this.xeb515a5b77846afe(this.trackItem, i, true, false);
                        }
                        return -1;
                    }
                    return i;
                }
                else
                {
                    i++;
                }
            }
            return -1;
        }

        protected x555516122dcc901e.POINT x4b688849d80162f6(x40255b11ef821fa3.MSG x8a41fbc87a3fb305)
        {
            x555516122dcc901e.POINT result;
            result.x = (int)((short)((int)x8a41fbc87a3fb305.lParam & 65535));
            result.y = (int)((short)((uint)((int)x8a41fbc87a3fb305.lParam & -65536) >> 16));
            if (x8a41fbc87a3fb305.message != 162u && x8a41fbc87a3fb305.message != 168u && x8a41fbc87a3fb305.message != 165u && x8a41fbc87a3fb305.message != 172u && x8a41fbc87a3fb305.message != 161u && x8a41fbc87a3fb305.message != 167u && x8a41fbc87a3fb305.message != 164u && x8a41fbc87a3fb305.message != 171u)
            {
                x61467fe65a98f20c.ClientToScreen(x8a41fbc87a3fb305.hwnd, ref result);
            }
            return result;
        }

        protected bool x277ff37e12aff68b(x555516122dcc901e.POINT x0ce73f6cbd7d5515, ref x40255b11ef821fa3.MSG x8a41fbc87a3fb305)
        {
            if (x8a41fbc87a3fb305.message == 512u && this.parentControl != null)
            {
                xae4dd1cafd2eb77c.RECT rECT = default(xae4dd1cafd2eb77c.RECT);
                x61467fe65a98f20c.GetWindowRect(this.parentControl.Handle, ref rECT);
                if (x0ce73f6cbd7d5515.x >= rECT.left && x0ce73f6cbd7d5515.x <= rECT.right && x0ce73f6cbd7d5515.y >= rECT.top && x0ce73f6cbd7d5515.y <= rECT.bottom)
                {
                    return true;
                }
            }
            return false;
        }

        internal x902c4aee45bfd906 x3c6f7570c20e3f7d(x555516122dcc901e.POINT x0ce73f6cbd7d5515, ref x40255b11ef821fa3.MSG x8a41fbc87a3fb305)
        {
            if (this.parentMenu != null)
            {
                return this.parentMenu.xcb534454d6825379(x0ce73f6cbd7d5515);
            }
            return null;
        }

        protected x902c4aee45bfd906 xcb534454d6825379(x555516122dcc901e.POINT x0ce73f6cbd7d5515)
        {
            xae4dd1cafd2eb77c.RECT rECT = default(xae4dd1cafd2eb77c.RECT);
            x61467fe65a98f20c.GetWindowRect(base.Handle, ref rECT);
            if (x0ce73f6cbd7d5515.x >= rECT.left && x0ce73f6cbd7d5515.x <= rECT.right && x0ce73f6cbd7d5515.y >= rECT.top && x0ce73f6cbd7d5515.y <= rECT.bottom)
            {
                return this;
            }
            if (this.parentMenu != null)
            {
                return this.parentMenu.xcb534454d6825379(x0ce73f6cbd7d5515);
            }
            return null;
        }

        protected void xeb515a5b77846afe(int x5360a38cdebd0e93, int x169940ac3300e3e7, bool xaca68b9c5e07af12, bool xb2ad27288bcb93e7)
        {
            bool flag = false;
            using (Graphics graphics = Graphics.FromHwnd(base.Handle))
            {
                if (x5360a38cdebd0e93 != -1)
                {
                    x2cc390e9409b0f3f x2cc390e9409b0f3f = this.drawCommands[x5360a38cdebd0e93] as x2cc390e9409b0f3f;
                    if (this.layered)
                    {
                        flag = true;
                    }
                    else
                    {
                        this.xb5a5bdae3ba8936b(graphics, this.drawCommands[x5360a38cdebd0e93] as x2cc390e9409b0f3f, false);
                    }
                    if (x2cc390e9409b0f3f.MenuCommand != null)
                    {
                        this.OnDeselected(x2cc390e9409b0f3f.MenuCommand);
                    }
                }
                if (x169940ac3300e3e7 != -1)
                {
                    this.timer.Stop();
                    if (!xb2ad27288bcb93e7 && this.childMenu != null)
                    {
                        this.timer.Interval = x902c4aee45bfd906.selectionDelay;
                        this.timer.Start();
                    }
                    x2cc390e9409b0f3f x2cc390e9409b0f3f2 = this.drawCommands[x169940ac3300e3e7] as x2cc390e9409b0f3f;
                    if (!x2cc390e9409b0f3f2.Separator && x2cc390e9409b0f3f2.Enabled)
                    {
                        if (this.layered)
                        {
                            flag = true;
                        }
                        else
                        {
                            this.xb5a5bdae3ba8936b(graphics, x2cc390e9409b0f3f2, true);
                        }
                        if (!xb2ad27288bcb93e7 && xaca68b9c5e07af12)
                        {
                            if (x2cc390e9409b0f3f2.Expansion)
                            {
                                this.timer.Interval = x902c4aee45bfd906.expansionDelay;
                            }
                            else
                            {
                                this.timer.Interval = x902c4aee45bfd906.selectionDelay;
                            }
                            this.timer.Start();
                        }
                        if (x2cc390e9409b0f3f2.MenuCommand != null)
                        {
                            this.OnSelected(x2cc390e9409b0f3f2.MenuCommand);
                        }
                    }
                    else
                    {
                        x169940ac3300e3e7 = -1;
                    }
                }
                this.trackItem = x169940ac3300e3e7;
                if (this.layered && flag)
                {
                    this.x9119fce83e4631ad();
                }
            }
        }

        protected void x07cc1c177f7b41bd(object xe0292b9ed559da7d, EventArgs xfbf34718e704c6bc)
        {
            this.timer.Stop();
            bool flag = true;
            if (this.childMenu != null)
            {
                if (this.popupItem != this.trackItem)
                {
                    x61467fe65a98f20c.PostMessage(this.childMenu.Handle, this.WM_DISMISS, 0u, 0u);
                }
                else
                {
                    flag = false;
                }
            }
            if (flag && this.trackItem != -1)
            {
                x2cc390e9409b0f3f x2cc390e9409b0f3f = this.drawCommands[this.trackItem] as x2cc390e9409b0f3f;
                if (x2cc390e9409b0f3f.SubMenu)
                {
                    this.x0760506d6abf77a2(this.trackItem, false);
                    return;
                }
                if (x2cc390e9409b0f3f.Expansion)
                {
                    this.x057464a452da34b7();
                }
            }
        }

        protected void x0760506d6abf77a2(int xf624f8e86c5ae5f1, bool xacc37ebdd71fcc44)
        {
            x61467fe65a98f20c.PostMessage(base.Handle, this.WM_OPERATE_SUBMENU, (uint)xf624f8e86c5ae5f1, xacc37ebdd71fcc44 ? 1u : 0u);
        }

        protected void xbcf8eb07b7e76a62(ref Message x6088325dec1baa2a)
        {
            int index = (int)x6088325dec1baa2a.WParam;
            bool xacc37ebdd71fcc = x6088325dec1baa2a.LParam != IntPtr.Zero;
            this.popupItem = index;
            this.childMenu = new x902c4aee45bfd906(this.Engine);
            x2cc390e9409b0f3f x2cc390e9409b0f3f = this.drawCommands[index] as x2cc390e9409b0f3f;
            x555516122dcc901e.POINT pOINT;
            pOINT.x = x2cc390e9409b0f3f.DrawRect.Right;
            pOINT.y = x2cc390e9409b0f3f.DrawRect.Top;
            x61467fe65a98f20c.ClientToScreen(base.Handle, ref pOINT);
            x555516122dcc901e.POINT pOINT2;
            pOINT2.x = x2cc390e9409b0f3f.DrawRect.Left;
            pOINT2.y = x2cc390e9409b0f3f.DrawRect.Top;
            x61467fe65a98f20c.ClientToScreen(base.Handle, ref pOINT2);
            this.childMenu.Font = this.Font;
            this.childMenu.Animate = this.Animate;
            this.childMenu.AnimateStyle = this.AnimateStyle;
            this.childMenu.AnimateTime = this.AnimateTime;
            int num = 0;
            this.childMenu.RememberExpansion = this.rememberExpansion;
            this.childMenu.showInfrequent = x2cc390e9409b0f3f.MenuCommand.MenuCommands.ShowInfrequent;
            this.childMenu.HighlightInfrequent = this.highlightInfrequent;
            x2cc390e9409b0f3f.MenuCommand.OnPopupStart();
            this.returnCommand = this.childMenu.x6e4244e25732db7d(new Point(pOINT.x, pOINT.y), new Point(pOINT2.x, pOINT2.y), x2cc390e9409b0f3f.MenuCommand.MenuCommands, this, xacc37ebdd71fcc, this.parentControl, this.popupRight, this.popupDown, this.animateFirst, ref num);
            x2cc390e9409b0f3f.MenuCommand.OnPopupEnd();
            this.childMenu = null;
            this.animateFirst = false;
            if (this.returnCommand != null || num != 0)
            {
                this.timer.Stop();
                this.exitLoop = true;
                this.returnDir = num;
            }
        }

        public virtual void OnSelected(x5f4b657f68f87baa mc)
        {
            if (this.parentControl != null)
            {
                this.parentControl.OnSelected(mc);
                return;
            }
            if (this.xaa7558c320af04eb != null)
            {
                this.xaa7558c320af04eb(mc);
                return;
            }
            if (this.parentMenu != null)
            {
                this.parentMenu.OnSelected(mc);
            }
        }

        public virtual void OnDeselected(x5f4b657f68f87baa mc)
        {
            if (this.parentControl != null)
            {
                this.parentControl.OnDeselected(mc);
                return;
            }
            if (this.xd42ce0324cbc114a != null)
            {
                this.xd42ce0324cbc114a(mc);
                return;
            }
            if (this.parentMenu != null)
            {
                this.parentMenu.OnDeselected(mc);
            }
        }

        protected void x038e1503810e39c7(ref Message x6088325dec1baa2a)
        {
            x40255b11ef821fa3.PAINTSTRUCT pAINTSTRUCT = default(x40255b11ef821fa3.PAINTSTRUCT);
            IntPtr hdc = x61467fe65a98f20c.BeginPaint(x6088325dec1baa2a.HWnd, out pAINTSTRUCT);
            xae4dd1cafd2eb77c.RECT rECT = default(xae4dd1cafd2eb77c.RECT);
            x61467fe65a98f20c.GetWindowRect(base.Handle, ref rECT);
            Rectangle x3821770539095b = new Rectangle(0, 0, rECT.right - rECT.left, rECT.bottom - rECT.top);
            using (Graphics graphics = Graphics.FromHdc(hdc))
            {
                Bitmap image = new Bitmap(x3821770539095b.Width, x3821770539095b.Height);
                using (Graphics graphics2 = Graphics.FromImage(image))
                {
                    this.x4af0f9e5d7ac184a(graphics2, x3821770539095b);
                    this.x05a10b378cdf8119(graphics2);
                }
                graphics.DrawImageUnscaled(image, 0, 0);
            }
            x61467fe65a98f20c.EndPaint(x6088325dec1baa2a.HWnd, ref pAINTSTRUCT);
        }

        protected void xa68b9c37c0ff7fc5(ref Message x6088325dec1baa2a)
        {
            this.timer.Stop();
            this.exitLoop = true;
        }

        protected void x76a103c0a87eb01c()
        {
            this.timer.Stop();
            if (this.popupItem != this.trackItem)
            {
                this.xeb515a5b77846afe(this.trackItem, this.popupItem, false, true);
            }
            if (this.parentMenu != null)
            {
                this.parentMenu.x76a103c0a87eb01c();
            }
        }

        protected void x136735fdfe6d04ea(int x75cf7df8c59ffa4d, int xc13ed6de98262a2d)
        {
            x75cf7df8c59ffa4d -= this.currentPoint.X;
            xc13ed6de98262a2d -= this.currentPoint.Y;
            if (this.parentMenu != null)
            {
                this.parentMenu.x76a103c0a87eb01c();
            }
            this.mouseOver = true;
            Point point = new Point(x75cf7df8c59ffa4d, xc13ed6de98262a2d);
            if (this.lastMousePosition != point)
            {
                for (int i = 0; i < this.drawCommands.Count; i++)
                {
                    x2cc390e9409b0f3f x2cc390e9409b0f3f = this.drawCommands[i] as x2cc390e9409b0f3f;
                    if (x2cc390e9409b0f3f.DrawRect.Contains(point) && this.trackItem != i)
                    {
                        this.xeb515a5b77846afe(this.trackItem, i, true, false);
                    }
                }
                this.lastMousePosition = point;
            }
        }

        protected void xa84c74b103039157()
        {
            if (this.trackItem != -1 && this.childMenu == null)
            {
                this.xeb515a5b77846afe(this.trackItem, -1, false, false);
            }
            this.mouseOver = false;
            this.lastMousePosition = new Point(-1, -1);
        }

        protected void xd6cfa3f3f3125e82(int x75cf7df8c59ffa4d, int xc13ed6de98262a2d)
        {
            x75cf7df8c59ffa4d -= this.currentPoint.X;
            xc13ed6de98262a2d -= this.currentPoint.Y;
            Point pt = new Point(x75cf7df8c59ffa4d, xc13ed6de98262a2d);
            for (int i = 0; i < this.drawCommands.Count; i++)
            {
                x2cc390e9409b0f3f x2cc390e9409b0f3f = this.drawCommands[i] as x2cc390e9409b0f3f;
                if (x2cc390e9409b0f3f.DrawRect.Contains(pt) && this.trackItem != i)
                {
                    this.xeb515a5b77846afe(this.trackItem, i, false, false);
                }
            }
            if (this.trackItem != -1)
            {
                x2cc390e9409b0f3f x2cc390e9409b0f3f2 = this.drawCommands[this.trackItem] as x2cc390e9409b0f3f;
                if (x2cc390e9409b0f3f2.SubMenu)
                {
                    if (this.popupItem != this.trackItem)
                    {
                        if (this.childMenu != null)
                        {
                            x61467fe65a98f20c.PostMessage(this.childMenu.Handle, this.WM_DISMISS, 0u, 0u);
                        }
                        this.x0760506d6abf77a2(this.trackItem, false);
                        return;
                    }
                }
                else
                {
                    if (x2cc390e9409b0f3f2.Expansion)
                    {
                        this.x057464a452da34b7();
                        return;
                    }
                    if (this.childMenu != null)
                    {
                        x61467fe65a98f20c.PostMessage(this.childMenu.Handle, this.WM_DISMISS, 0u, 0u);
                    }
                    this.returnCommand = x2cc390e9409b0f3f2.MenuCommand;
                    this.timer.Stop();
                    this.exitLoop = true;
                }
            }
        }

        protected void x20a824d899eb6cd8(ref Message x6088325dec1baa2a)
        {
            x6088325dec1baa2a.Result = (IntPtr)3;
        }

        protected void x773209141f430902()
        {
            x61467fe65a98f20c.SetCursor(x61467fe65a98f20c.LoadCursor(IntPtr.Zero, 32512u));
        }

        protected void xb97e8a8020dddaac()
        {
            if (this.childMenu != null)
            {
                x61467fe65a98f20c.PostMessage(this.childMenu.Handle, this.WM_DISMISS, 0u, 0u);
            }
            this.returnCommand = null;
            this.timer.Stop();
            this.exitLoop = true;
            this.x4139e66e6dfaac52();
            this.DestroyHandle();
        }

        protected bool xddec9c21ef58186c(ref Message x6088325dec1baa2a)
        {
            x555516122dcc901e.POINT pOINT;
            pOINT.x = (int)((short)((int)x6088325dec1baa2a.LParam & 65535));
            pOINT.y = (int)((short)((uint)((int)x6088325dec1baa2a.LParam & -65536) >> 16));
            x555516122dcc901e.POINT pOINT2;
            pOINT2.x = this.currentSize.Width - x902c4aee45bfd906._position[0, 16];
            pOINT2.y = this.currentSize.Height - x902c4aee45bfd906._position[0, 17];
            x61467fe65a98f20c.ClientToScreen(base.Handle, ref pOINT2);
            if (pOINT.x > pOINT2.x || pOINT.y > pOINT2.y)
            {
                x6088325dec1baa2a.Result = (IntPtr)(-1);
                return true;
            }
            return false;
        }

        protected override void WndProc(ref Message m)
        {
            if ((long)m.Msg == (long)((ulong)this.WM_DISMISS))
            {
                this.xb97e8a8020dddaac();
                return;
            }
            if ((long)m.Msg == (long)((ulong)this.WM_OPERATE_SUBMENU))
            {
                this.xbcf8eb07b7e76a62(ref m);
                return;
            }
            uint msg = (uint)m.Msg;
            if (msg <= 28u)
            {
                if (msg == 15u)
                {
                    this.x038e1503810e39c7(ref m);
                    return;
                }
                if (msg == 28u)
                {
                    this.xa68b9c37c0ff7fc5(ref m);
                    return;
                }
            }
            else
            {
                switch (msg)
                {
                    case 32u:
                        this.x773209141f430902();
                        return;
                    case 33u:
                        this.x20a824d899eb6cd8(ref m);
                        return;
                    default:
                        if (msg == 132u)
                        {
                            if (!this.xddec9c21ef58186c(ref m))
                            {
                                base.WndProc(ref m);
                                return;
                            }
                            return;
                        }
                        break;
                }
            }
            base.WndProc(ref m);
        }

        protected unsafe static Bitmap x7ca9c34ba153ef81(int x9b0739496f8b5475, int x4d5aabc7a55b12ba)
        {
            if (x902c4aee45bfd906.shadowCacheWidth == x9b0739496f8b5475 && x902c4aee45bfd906.shadowCacheHeight == x4d5aabc7a55b12ba && x902c4aee45bfd906.shadowCache != null)
            {
                return x902c4aee45bfd906.shadowCache;
            }
            if (x902c4aee45bfd906.shadowCache != null)
            {
                x902c4aee45bfd906.shadowCache.Dispose();
            }
            Bitmap bitmap = new Bitmap(x9b0739496f8b5475, x4d5aabc7a55b12ba, PixelFormat.Format32bppArgb);
            BitmapData bitmapData = bitmap.LockBits(new Rectangle(0, 0, x9b0739496f8b5475, x4d5aabc7a55b12ba), ImageLockMode.WriteOnly, PixelFormat.Format32bppArgb);
            uint* ptr = (uint*)((void*)bitmapData.Scan0);
            for (int i = 0; i < x4d5aabc7a55b12ba; i++)
            {
                int num = bitmapData.Stride * i / 4;
                int num2 = 64 * (x4d5aabc7a55b12ba - i) / (x4d5aabc7a55b12ba + 1);
                for (int j = 0; j < x9b0739496f8b5475; j++)
                {
                    int num3 = num2 * (x9b0739496f8b5475 - j) / (x9b0739496f8b5475 + 1);
                    ptr[num + j] = (uint)((uint)num3 << 24);
                }
            }
            bitmap.UnlockBits(bitmapData);
            x902c4aee45bfd906.shadowCache = bitmap;
            x902c4aee45bfd906.shadowCacheWidth = x9b0739496f8b5475;
            x902c4aee45bfd906.shadowCacheHeight = x4d5aabc7a55b12ba;
            return x902c4aee45bfd906.shadowCache;
        }

        protected void x4af0f9e5d7ac184a(Graphics x4b101060f4767186, Rectangle x3821770539095b42)
        {
            Rectangle rectangle = new Rectangle(0, 0, x3821770539095b42.Width - 1 - x902c4aee45bfd906._position[0, 16], x3821770539095b42.Height - 1 - x902c4aee45bfd906._position[0, 17]);
            int width = x902c4aee45bfd906._position[0, 5] + x902c4aee45bfd906.imageWidth + x902c4aee45bfd906._position[0, 7];
            int x = x902c4aee45bfd906._position[0, 1];
            int num = x902c4aee45bfd906._position[0, 0];
            int height = rectangle.Height - num - x902c4aee45bfd906._position[0, 2] - 1;
            Brush brush = this.Engine.Res.Brushes.SKIN2_MENUITEMCOLOR;
            x4b101060f4767186.FillRectangle(brush, rectangle);
            brush = this.Engine.Res.Brushes.SKIN2_LEFTBARSTARTCOLOR;
            using (Pen pen = new Pen(ControlPaint.Dark(this.Engine.Res.Colors.SKIN2_MENUITEMCOLOR)))
            {
                x4b101060f4767186.DrawRectangle(pen, rectangle);
                if (this.borderGap > 0)
                {
                    if (this.direction == xdbfa333b4cd503e0.Horizontal)
                    {
                        if (this.excludeTop)
                        {
                            x4b101060f4767186.FillRectangle(Brushes.White, rectangle.Left + 1 + this.excludeOffset, rectangle.Top, this.borderGap - 1, 1);
                            x4b101060f4767186.FillRectangle(brush, rectangle.Left + 1 + this.excludeOffset, rectangle.Top, this.borderGap - 1, 1);
                        }
                        else
                        {
                            x4b101060f4767186.FillRectangle(Brushes.White, rectangle.Left + 1 + this.excludeOffset, rectangle.Bottom, this.borderGap - 1, 1);
                            x4b101060f4767186.FillRectangle(brush, rectangle.Left + 1 + this.excludeOffset, rectangle.Bottom, this.borderGap - 1, 1);
                        }
                    }
                    else if (this.excludeTop)
                    {
                        x4b101060f4767186.FillRectangle(Brushes.White, rectangle.Left, rectangle.Top + 1 + this.excludeOffset, 1, this.borderGap - 1);
                        x4b101060f4767186.FillRectangle(brush, rectangle.Left, rectangle.Top + 1 + this.excludeOffset, 1, this.borderGap - 1);
                    }
                    else if (this.popupDown)
                    {
                        x4b101060f4767186.FillRectangle(Brushes.White, rectangle.Left, rectangle.Bottom - 1 - this.excludeOffset, 1, this.borderGap - 1);
                        x4b101060f4767186.FillRectangle(brush, rectangle.Left, rectangle.Bottom - 1 - this.excludeOffset, 1, this.borderGap - 1);
                    }
                    else
                    {
                        x4b101060f4767186.FillRectangle(Brushes.White, rectangle.Left, rectangle.Bottom - this.borderGap + 1, 1, this.borderGap - 1);
                        x4b101060f4767186.FillRectangle(brush, rectangle.Left, rectangle.Bottom - this.borderGap + 1, 1, this.borderGap - 1);
                    }
                }
            }
            Rectangle rect = new Rectangle(x, num, width, height);
            x4b101060f4767186.FillRectangle(Brushes.White, rect);
            x4b101060f4767186.FillRectangle(brush, rect);
            foreach (x2cc390e9409b0f3f x2cc390e9409b0f3f in this.drawCommands)
            {
                if (x2cc390e9409b0f3f.Separator && x2cc390e9409b0f3f.VerticalSeparator)
                {
                    rect.X = x2cc390e9409b0f3f.DrawRect.Right;
                    x4b101060f4767186.FillRectangle(Brushes.White, rect);
                    x4b101060f4767186.FillRectangle(brush, rect);
                }
            }
            int num2 = rectangle.Right + 1;
            int num3 = rectangle.Top + x902c4aee45bfd906._position[0, 17];
            int num4 = rectangle.Bottom + 1;
            int num5 = rectangle.Left + x902c4aee45bfd906._position[0, 16];
            int num6 = rectangle.Left + this.excludeOffset;
            int num7 = rectangle.Left + this.excludeOffset + this.borderGap;
            if (this.borderGap > 0 && !this.excludeTop && this.direction == xdbfa333b4cd503e0.Horizontal)
            {
                int width2 = x3821770539095b42.Width;
                if (num6 >= num5)
                {
                    this.xcea282a46787878f(x4b101060f4767186, num5, num4, num6 - num5, x902c4aee45bfd906._position[0, 17], x902c4aee45bfd906.xf398ffaf32ffe055.Left);
                }
                if (num7 <= width2)
                {
                    this.xcea282a46787878f(x4b101060f4767186, num7, num4, width2 - num7, x902c4aee45bfd906._position[0, 17], x902c4aee45bfd906.xf398ffaf32ffe055.Right);
                }
            }
            else
            {
                if (this.direction == xdbfa333b4cd503e0.Vertical && !this.excludeTop)
                {
                    num5 = 0;
                }
                this.xcea282a46787878f(x4b101060f4767186, num5, num4, num2, x902c4aee45bfd906._position[0, 17], x902c4aee45bfd906.xf398ffaf32ffe055.All);
            }
            this.xe75f29bc258e83bf(x4b101060f4767186, num2, num3, x902c4aee45bfd906._position[0, 16], num4 - num3 - 1);
            if (this.menuCommands.ExtraText.Length > 0)
            {
                this.xe145f7beb24da7f5(x4b101060f4767186, rectangle);
            }
        }

        internal void xb5a5bdae3ba8936b(Graphics x4b101060f4767186, x2cc390e9409b0f3f xd2a8bb4342ab4ef6, bool x15a0329046fb799f)
        {
            Rectangle drawRect = xd2a8bb4342ab4ef6.DrawRect;
            x5f4b657f68f87baa menuCommand = xd2a8bb4342ab4ef6.MenuCommand;
            Color arg_23_0 = this.Engine.Res.Colors.SKIN2_SELECTEDMENUCOLOR;
            Color sKIN2_SELECTEDMENUBORDERCOLOR = this.Engine.Res.Colors.SKIN2_SELECTEDMENUBORDERCOLOR;
            Color sKIN2_SELECTEDMENUCOLOR = this.Engine.Res.Colors.SKIN2_SELECTEDMENUCOLOR;
            int num = x902c4aee45bfd906._position[0, 8];
            int num2 = x902c4aee45bfd906._position[0, 5];
            int num3 = x902c4aee45bfd906._position[0, 7];
            int num4 = drawRect.Left + num2;
            int num5 = num2 + x902c4aee45bfd906.imageWidth + num3;
            int num6 = drawRect.Right - x902c4aee45bfd906._position[0, 12] - x902c4aee45bfd906._position[0, 11];
            int num7 = num6 - x902c4aee45bfd906._position[0, 10] - x902c4aee45bfd906._position[0, 9];
            Brush brush;
            if (xd2a8bb4342ab4ef6.Expansion)
            {
                Rectangle rectangle = drawRect;
                rectangle.X += num5;
                rectangle.Width -= num5;
                int x = rectangle.Left + (rectangle.Width - x902c4aee45bfd906.imageHeight) / 2;
                int y = rectangle.Top + (rectangle.Height - x902c4aee45bfd906.imageHeight) / 2;
                if (x15a0329046fb799f)
                {
                    Rectangle rect = new Rectangle(drawRect.Left + 1, drawRect.Top, drawRect.Width - 3, drawRect.Height - 1);
                    using (Pen pen = new Pen(sKIN2_SELECTEDMENUBORDERCOLOR))
                    {
                        using (SolidBrush solidBrush = new SolidBrush(Color.White))
                        {
                            x4b101060f4767186.FillRectangle(solidBrush, rect);
                        }
                        using (SolidBrush solidBrush2 = new SolidBrush(sKIN2_SELECTEDMENUCOLOR))
                        {
                            x4b101060f4767186.FillRectangle(solidBrush2, rect);
                            x4b101060f4767186.DrawRectangle(pen, rect);
                        }
                        goto IL_29C;
                    }
                }
                brush = this.Engine.Res.Brushes.SKIN2_MENUITEMCOLOR;
                x4b101060f4767186.FillRectangle(brush, new Rectangle(drawRect.Left + 1, drawRect.Top, drawRect.Width - 1, drawRect.Height));
                x4b101060f4767186.FillRectangle(Brushes.White, new Rectangle(drawRect.Left, drawRect.Top, num5, drawRect.Height));
                brush = this.Engine.Res.Brushes.SKIN2_LEFTBARSTARTCOLOR;
                x4b101060f4767186.FillRectangle(brush, new Rectangle(drawRect.Left, drawRect.Top, num5, drawRect.Height));
            IL_29C:
                x4b101060f4767186.DrawImage(x902c4aee45bfd906.menuImages.Images[6], x, y);
                return;
            }
            if (xd2a8bb4342ab4ef6.Separator)
            {
                if (xd2a8bb4342ab4ef6.VerticalSeparator)
                {
                    using (Pen pen2 = new Pen(ControlPaint.Dark(this.Engine.Res.Colors.SKIN2_MENUITEMCOLOR)))
                    {
                        x4b101060f4767186.DrawLine(pen2, drawRect.Left, drawRect.Top, drawRect.Left, drawRect.Bottom);
                        return;
                    }
                }
                Rectangle rect2 = new Rectangle(drawRect.Left, drawRect.Top, num5, drawRect.Height);
                x4b101060f4767186.FillRectangle(Brushes.White, rect2);
                if (this.Engine.Res.Colors.SKIN2_LEFTBARSTARTCOLOR != this.xcab6a0e662ada486.Res.Colors.SKIN2_LEFTBARENDCOLOR)
                {
                    using (LinearGradientBrush linearGradientBrush = new LinearGradientBrush(rect2, this.xcab6a0e662ada486.Res.Colors.SKIN2_LEFTBARSTARTCOLOR, this.xcab6a0e662ada486.Res.Colors.SKIN2_LEFTBARENDCOLOR, LinearGradientMode.Horizontal))
                    {
                        x4b101060f4767186.FillRectangle(linearGradientBrush, rect2);
                        goto IL_3E7;
                    }
                }
                brush = this.Engine.Res.Brushes.SKIN2_LEFTBARSTARTCOLOR;
                x4b101060f4767186.FillRectangle(brush, rect2);
            IL_3E7:
                using (Pen pen3 = new Pen(Color.FromArgb(75, this.Engine.Res.Colors.SKIN2_MENUITEMFONTCOLOR)))
                {
                    x4b101060f4767186.DrawLine(pen3, drawRect.Left + num5 + num, drawRect.Top + 2, drawRect.Right, drawRect.Top + 2);
                    return;
                }
            }
            int num8 = drawRect.Left + num5 + num;
            if (x15a0329046fb799f)
            {
                Rectangle rect3 = new Rectangle(drawRect.Left + 1, drawRect.Top, drawRect.Width - 3, drawRect.Height - 1);
                using (Pen pen4 = new Pen(sKIN2_SELECTEDMENUBORDERCOLOR))
                {
                    using (SolidBrush solidBrush3 = new SolidBrush(Color.White))
                    {
                        x4b101060f4767186.FillRectangle(solidBrush3, rect3);
                    }
                    using (SolidBrush solidBrush4 = new SolidBrush(sKIN2_SELECTEDMENUCOLOR))
                    {
                        x4b101060f4767186.FillRectangle(solidBrush4, rect3);
                        x4b101060f4767186.DrawRectangle(pen4, rect3);
                    }
                    goto IL_69E;
                }
            }
            brush = this.Engine.Res.Brushes.SKIN2_MENUITEMCOLOR;
            x4b101060f4767186.FillRectangle(brush, new Rectangle(drawRect.Left + 1, drawRect.Top, drawRect.Width - 1, drawRect.Height));
            if (xd2a8bb4342ab4ef6.Infrequent && this.highlightInfrequent)
            {
                x4b101060f4767186.FillRectangle(Brushes.White, new Rectangle(num8, drawRect.Top, drawRect.Right - num8 - num, drawRect.Height));
                using (Brush brush2 = new SolidBrush(x448fd9ab43628c71.CalculateColor(this.Engine.Res.Colors.SKIN2_MENUITEMCOLOR, Color.White, 150)))
                {
                    x4b101060f4767186.FillRectangle(brush2, new Rectangle(num8, drawRect.Top, drawRect.Right - num8 - num, drawRect.Height));
                }
            }
            Rectangle rect4 = new Rectangle(drawRect.Left, drawRect.Top, num5, drawRect.Height);
            x4b101060f4767186.FillRectangle(Brushes.White, rect4);
            if (this.Engine.Res.Colors.SKIN2_LEFTBARSTARTCOLOR != this.Engine.Res.Colors.SKIN2_LEFTBARENDCOLOR)
            {
                using (LinearGradientBrush linearGradientBrush2 = new LinearGradientBrush(rect4, this.xcab6a0e662ada486.Res.Colors.SKIN2_LEFTBARSTARTCOLOR, this.xcab6a0e662ada486.Res.Colors.SKIN2_LEFTBARENDCOLOR, LinearGradientMode.Horizontal))
                {
                    x4b101060f4767186.FillRectangle(linearGradientBrush2, rect4);
                    goto IL_69E;
                }
            }
            brush = this.Engine.Res.Brushes.SKIN2_LEFTBARSTARTCOLOR;
            x4b101060f4767186.FillRectangle(brush, rect4);
        IL_69E:
            Rectangle rectangle2 = new Rectangle(num8, drawRect.Top, num7 - num8, drawRect.Height);
            StringFormat stringFormat = new StringFormat();
            stringFormat.FormatFlags = (StringFormatFlags.NoWrap | StringFormatFlags.NoClip);
            if (this.parentControl != null && (this.parentControl.RightToLeft & RightToLeft.Yes) == RightToLeft.Yes)
            {
                stringFormat.FormatFlags |= StringFormatFlags.DirectionRightToLeft;
            }
            if ((this.x94975a4c4f1d71c4 & RightToLeft.Yes) == RightToLeft.Yes)
            {
                stringFormat.FormatFlags |= StringFormatFlags.DirectionRightToLeft;
            }
            stringFormat.Alignment = StringAlignment.Near;
            stringFormat.LineAlignment = StringAlignment.Center;
            stringFormat.HotkeyPrefix = HotkeyPrefix.Show;
            SolidBrush brush3;
            if (x15a0329046fb799f)
            {
                brush3 = new SolidBrush(this.Engine.Res.Colors.SKIN2_SELECTEDMENUFONTCOLOR);
            }
            else if (menuCommand.Enabled)
            {
                brush3 = new SolidBrush(this.Engine.Res.Colors.SKIN2_MENUITEMFONTCOLOR);
            }
            else
            {
                brush3 = new SolidBrush(SystemColors.GrayText);
            }
            Rectangle rectangle3 = rectangle2;
            rectangle3.Offset(1, 1);
            x4b101060f4767186.DrawString(menuCommand.Text, this.textFont, brush3, rectangle2, stringFormat);
            if (menuCommand.Shortcut != Shortcut.None)
            {
                stringFormat.Alignment = StringAlignment.Far;
                if (menuCommand.Enabled)
                {
                    x4b101060f4767186.DrawString(this.x8cf43133d25d686a(menuCommand.Shortcut), this.textFont, brush3, rectangle2, stringFormat);
                }
                else
                {
                    x4b101060f4767186.DrawString(this.x8cf43133d25d686a(menuCommand.Shortcut), this.textFont, brush3, rectangle2, stringFormat);
                }
            }
            int num9 = drawRect.Top + (drawRect.Height - x902c4aee45bfd906.imageHeight) / 2;
            Image image = null;
            if (menuCommand.Checked)
            {
                Pen pen5;
                Brush brush4;
                if (menuCommand.Enabled)
                {
                    pen5 = new Pen(sKIN2_SELECTEDMENUBORDERCOLOR);
                    brush4 = new SolidBrush(Color.FromArgb(20, this.Engine.Res.Colors.SKIN2_SELECTEDMENUBORDERCOLOR));
                }
                else
                {
                    pen5 = new Pen(SystemColors.GrayText);
                    brush4 = new SolidBrush(Color.FromArgb(20, SystemColors.GrayText));
                }
                Rectangle rect5 = new Rectangle(num4 - 1, num9 - 1, x902c4aee45bfd906.imageHeight + 2, x902c4aee45bfd906.imageWidth + 2);
                x4b101060f4767186.FillRectangle(brush4, rect5);
                x4b101060f4767186.DrawRectangle(pen5, rect5);
                pen5.Dispose();
                brush4.Dispose();
                if (menuCommand.RadioCheck)
                {
                    image = x902c4aee45bfd906.menuImages.Images[1];
                }
                else
                {
                    image = this.Engine.Res.Bitmaps.SKIN2_CHECKEDMENUICON;
                }
            }
            else
            {
                try
                {
                    if (menuCommand.Image != null)
                    {
                        image = menuCommand.Image;
                    }
                    else if (menuCommand.ImageList != null && menuCommand.ImageIndex >= 0)
                    {
                        image = menuCommand.ImageList.Images[menuCommand.ImageIndex];
                    }
                }
                catch (Exception)
                {
                    image = x902c4aee45bfd906.menuImages.Images[7];
                }
            }
            if (image != null)
            {
                if (menuCommand.Enabled)
                {
                    if (x15a0329046fb799f && !menuCommand.Checked)
                    {
                        Bitmap bitmap = new Bitmap((Bitmap)image);
                        Color color = Color.FromArgb(154, 156, 146);
                        Color right = Color.FromArgb(0, 0, 0, 0);
                        for (int i = 0; i < image.Width; i++)
                        {
                            for (int j = 0; j < image.Height; j++)
                            {
                                if (bitmap.GetPixel(i, j) != right)
                                {
                                    bitmap.SetPixel(i, j, color);
                                }
                            }
                        }
                        x4b101060f4767186.DrawImage(bitmap, num4 + 1, num9 + 1);
                        x4b101060f4767186.DrawImage(image, num4 - 1, num9 - 1);
                    }
                    else
                    {
                        Bitmap bitmap2 = new Bitmap(image);
                        Color right2 = Color.FromArgb(0, 0, 0, 0);
                        for (int k = 0; k < image.Width; k++)
                        {
                            for (int l = 0; l < image.Height; l++)
                            {
                                Color pixel = bitmap2.GetPixel(k, l);
                                if (pixel != right2)
                                {
                                    Color color2 = Color.FromArgb((int)(pixel.R + 76 - (pixel.R + 32) / 64 * 19), (int)(pixel.G + 76 - (pixel.G + 32) / 64 * 19), (int)(pixel.B + 76 - (pixel.B + 32) / 64 * 19));
                                    bitmap2.SetPixel(k, l, color2);
                                }
                            }
                        }
                        x4b101060f4767186.DrawImage(bitmap2, num4, num9);
                    }
                }
                else
                {
                    ControlPaint.DrawImageDisabled(x4b101060f4767186, image, num4, num9, SystemColors.HighlightText);
                }
            }
            if (xd2a8bb4342ab4ef6.SubMenu)
            {
                if (menuCommand.Enabled)
                {
                    int index = 2;
                    x4b101060f4767186.DrawImage(x902c4aee45bfd906.menuImages.Images[index], num6, num9);
                    return;
                }
                ControlPaint.DrawImageDisabled(x4b101060f4767186, x902c4aee45bfd906.menuImages.Images[2], num6, num9, this.Engine.Res.Colors.SKIN2_SELECTEDMENUFONTCOLOR);
            }
        }

        protected void x05a10b378cdf8119(Graphics x4b101060f4767186)
        {
            for (int i = 0; i < this.drawCommands.Count; i++)
            {
                x2cc390e9409b0f3f xd2a8bb4342ab4ef = this.drawCommands[i] as x2cc390e9409b0f3f;
                this.xb5a5bdae3ba8936b(x4b101060f4767186, xd2a8bb4342ab4ef, i == this.trackItem);
            }
        }

        protected void xe75f29bc258e83bf(Graphics x4b101060f4767186, int xa447fc54e41dfe06, int xc941868c59399d3e, int x9b0739496f8b5475, int x4d5aabc7a55b12ba)
        {
            if (this.layered)
            {
                Color color = Color.FromArgb(64, 0, 0, 0);
                Color color2 = Color.FromArgb(48, 0, 0, 0);
                Color color3 = Color.FromArgb(0, 0, 0, 0);
                if (x4d5aabc7a55b12ba >= x902c4aee45bfd906.shadowLength)
                {
                    using (LinearGradientBrush linearGradientBrush = new LinearGradientBrush(new Point(xa447fc54e41dfe06 - x902c4aee45bfd906.shadowLength, xc941868c59399d3e + x902c4aee45bfd906.shadowLength), new Point(xa447fc54e41dfe06 + x902c4aee45bfd906.shadowLength, xc941868c59399d3e), color, color3))
                    {
                        x4b101060f4767186.FillRectangle(linearGradientBrush, xa447fc54e41dfe06, xc941868c59399d3e, x902c4aee45bfd906.shadowLength, x902c4aee45bfd906.shadowLength);
                        xc941868c59399d3e += x902c4aee45bfd906.shadowLength;
                        x4d5aabc7a55b12ba -= x902c4aee45bfd906.shadowLength;
                    }
                }
                using (LinearGradientBrush linearGradientBrush2 = new LinearGradientBrush(new Point(xa447fc54e41dfe06, 0), new Point(xa447fc54e41dfe06 + x9b0739496f8b5475, 0), color2, color3))
                {
                    x4b101060f4767186.FillRectangle(linearGradientBrush2, xa447fc54e41dfe06, xc941868c59399d3e, x9b0739496f8b5475, x4d5aabc7a55b12ba + 1);
                    return;
                }
            }
            using (SolidBrush solidBrush = new SolidBrush(ControlPaint.Dark(this.Engine.Res.Colors.SKIN2_MENUITEMCOLOR)))
            {
                x4b101060f4767186.FillRectangle(solidBrush, xa447fc54e41dfe06, xc941868c59399d3e, x9b0739496f8b5475, x4d5aabc7a55b12ba + 1);
            }
        }

        protected void xcea282a46787878f(Graphics x4b101060f4767186, int xa447fc54e41dfe06, int xc941868c59399d3e, int x9b0739496f8b5475, int x4d5aabc7a55b12ba, x902c4aee45bfd906.xf398ffaf32ffe055 x9c9eac3a36336680)
        {
            if (this.layered)
            {
                Color color = Color.FromArgb(64, 0, 0, 0);
                Color color2 = Color.FromArgb(48, 0, 0, 0);
                Color color3 = Color.FromArgb(0, 0, 0, 0);
                if (x9c9eac3a36336680 != x902c4aee45bfd906.xf398ffaf32ffe055.Right && x9b0739496f8b5475 >= x902c4aee45bfd906.shadowLength)
                {
                    using (LinearGradientBrush linearGradientBrush = new LinearGradientBrush(new Point(xa447fc54e41dfe06 + x902c4aee45bfd906.shadowLength, xc941868c59399d3e - x902c4aee45bfd906.shadowLength), new Point(xa447fc54e41dfe06, xc941868c59399d3e + x4d5aabc7a55b12ba), color, color3))
                    {
                        x4b101060f4767186.FillRectangle(linearGradientBrush, xa447fc54e41dfe06, xc941868c59399d3e, x902c4aee45bfd906.shadowLength, x4d5aabc7a55b12ba);
                        xa447fc54e41dfe06 += x902c4aee45bfd906.shadowLength;
                        x9b0739496f8b5475 -= x902c4aee45bfd906.shadowLength;
                    }
                }
                if (x9c9eac3a36336680 != x902c4aee45bfd906.xf398ffaf32ffe055.Left && x9b0739496f8b5475 >= x902c4aee45bfd906.shadowLength)
                {
                    try
                    {
                        x4b101060f4767186.DrawImageUnscaled(x902c4aee45bfd906.x7ca9c34ba153ef81(x902c4aee45bfd906.shadowLength, x4d5aabc7a55b12ba), xa447fc54e41dfe06 + x9b0739496f8b5475 - x902c4aee45bfd906.shadowLength, xc941868c59399d3e);
                    }
                    catch
                    {
                    }
                    x9b0739496f8b5475 -= x902c4aee45bfd906.shadowLength;
                }
                using (LinearGradientBrush linearGradientBrush2 = new LinearGradientBrush(new Point(9999, xc941868c59399d3e), new Point(9999, xc941868c59399d3e + x4d5aabc7a55b12ba), color2, color3))
                {
                    x4b101060f4767186.FillRectangle(linearGradientBrush2, xa447fc54e41dfe06, xc941868c59399d3e, x9b0739496f8b5475, x4d5aabc7a55b12ba);
                    return;
                }
            }
            using (SolidBrush solidBrush = new SolidBrush(ControlPaint.Dark(this.Engine.Res.Colors.SKIN2_MENUITEMCOLOR)))
            {
                x4b101060f4767186.FillRectangle(solidBrush, xa447fc54e41dfe06, xc941868c59399d3e, x9b0739496f8b5475, x4d5aabc7a55b12ba);
            }
        }

        protected void xe145f7beb24da7f5(Graphics x4b101060f4767186, Rectangle x04a925412b1bb508)
        {
            Rectangle rectangle = new Rectangle(x04a925412b1bb508.Left, x04a925412b1bb508.Top, this.extraSize - x902c4aee45bfd906._position[0, 20], x04a925412b1bb508.Height);
            bool flag = true;
            Brush brush;
            if (this.menuCommands.ExtraBackBrush != null)
            {
                brush = this.menuCommands.ExtraBackBrush;
                flag = false;
                rectangle.Width++;
            }
            else
            {
                brush = new SolidBrush(this.menuCommands.ExtraBackColor);
            }
            x4b101060f4767186.FillRectangle(brush, rectangle);
            if (flag)
            {
                brush.Dispose();
            }
            rectangle.X += x902c4aee45bfd906._position[0, 18];
            rectangle.Y += x902c4aee45bfd906._position[0, 19];
            rectangle.Width -= x902c4aee45bfd906._position[0, 18] * 2;
            rectangle.Height -= x902c4aee45bfd906._position[0, 19] * 2;
            StringFormat stringFormat = new StringFormat();
            stringFormat.FormatFlags = (StringFormatFlags.DirectionVertical | StringFormatFlags.NoWrap | StringFormatFlags.NoClip);
            stringFormat.Alignment = StringAlignment.Near;
            stringFormat.LineAlignment = StringAlignment.Center;
            bool flag2 = true;
            Brush brush2;
            if (this.menuCommands.ExtraTextBrush != null)
            {
                brush2 = this.menuCommands.ExtraTextBrush;
                flag2 = false;
            }
            else
            {
                brush2 = new SolidBrush(this.menuCommands.ExtraTextColor);
            }
            x957e371151765ec5.DrawReverseString(x4b101060f4767186, this.menuCommands.ExtraText, this.menuCommands.ExtraFont, rectangle, brush2, stringFormat);
            if (flag2)
            {
                brush2.Dispose();
            }
        }
    }
}
