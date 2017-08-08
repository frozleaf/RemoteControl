using Microsoft.Win32;
using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Runtime.CompilerServices;
using System.Windows.Forms;

namespace Sunisoft.IrisSkin.InternalControls
{
    [ToolboxItem(false)]
    internal class x3c41176af7e54b01 : ContainerControl, IMessageFilter
    {
        protected const int LengthGap = 3;

        protected const int BoxExpandUpper = 1;

        protected const int BoxExpandSides = 2;

        protected const int ShadowGap = 4;

        protected const int ShadowYOffset = 4;

        protected const int SeparatorWidth = 15;

        protected const int SubMenuBorderAdjust = 2;

        protected const int MinIndex = 0;

        protected const int RestoreIndex = 1;

        protected const int CloseIndex = 2;

        protected const int ChevronIndex = 3;

        protected const int ButtonLength = 16;

        protected const int ChevronLength = 12;

        protected const int PendantLength = 48;

        protected const int PendantOffset = 3;

        protected readonly uint WM_OPERATEMENU = 1025u;

        protected static ImageList menuImages;

        protected static bool supportsLayered;

        protected int rowWidth;

        protected int rowHeight;

        protected int trackItem;

        protected int breadthGap;

        protected int animateTime;

        protected IntPtr oldFocus;

        protected bool animateFirst;

        protected bool selected;

        protected bool multiLine;

        protected bool mouseOver;

        protected bool manualFocus;

        protected bool drawUpwards;

        protected bool defaultFont;

        protected bool plainAsBlock;

        protected bool dismissTransfer;

        protected bool ignoreMouseMove;

        protected bool expandAllTogether;

        protected bool rememberExpansion;

        protected bool deselectReset;

        protected bool highlightInfrequent;

        protected bool exitLoop;

        protected Form activeChild;

        protected Form mdiContainer;

        protected xc69458cec0f3af75 glyphFading;

        protected xdbfa333b4cd503e0 direction;

        protected x902c4aee45bfd906 popupMenu;

        protected ArrayList drawCommands;

        protected x6fd23f8bad2f3ced animate;

        protected x1f5697535eab37b9 animateStyle;

        protected x5f4b657f68f87baa chevronStartCommand;

        protected xd53b20b7b4b2a08a menuCommands;

        protected x3c41176af7e54b01 childMenu;

        protected x5d3356d9dffccb60 minButton;

        protected x5d3356d9dffccb60 restoreButton;

        protected x5d3356d9dffccb60 closeButton;

        private x26569a56dfbc2c6d xaa7558c320af04eb;

        private x26569a56dfbc2c6d xd42ce0324cbc114a;

        private SkinEngine xcab6a0e662ada486;

        private x5f4b657f68f87baa[] x4dedac25f8166fb5;

        private x5f4b657f68f87baa x54f4e17b11b07b8c = new x5f4b657f68f87baa("-");

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

        public override Font Font
        {
            get
            {
                return base.Font;
            }
            set
            {
                if (value != base.Font)
                {
                    this.defaultFont = (value == SystemInformation.MenuFont);
                    this.x21498b72da0020ba(value);
                    this.x39e008704a72ea56();
                    base.Invalidate();
                }
            }
        }

        public bool PlainAsBlock
        {
            get
            {
                return this.plainAsBlock;
            }
            set
            {
                if (this.plainAsBlock != value)
                {
                    this.plainAsBlock = value;
                    this.x39e008704a72ea56();
                    base.Invalidate();
                }
            }
        }

        public bool MultiLine
        {
            get
            {
                return this.multiLine;
            }
            set
            {
                if (this.multiLine != value)
                {
                    this.multiLine = value;
                    this.x39e008704a72ea56();
                    base.Invalidate();
                }
            }
        }

        public xdbfa333b4cd503e0 Direction
        {
            get
            {
                return this.direction;
            }
            set
            {
                if (this.direction != value)
                {
                    this.direction = value;
                    this.x39e008704a72ea56();
                    base.Invalidate();
                }
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

        public bool ExpandAllTogether
        {
            get
            {
                return this.expandAllTogether;
            }
            set
            {
                this.expandAllTogether = value;
            }
        }

        public bool DeselectReset
        {
            get
            {
                return this.deselectReset;
            }
            set
            {
                this.deselectReset = value;
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

        public override DockStyle Dock
        {
            get
            {
                return base.Dock;
            }
            set
            {
                base.Dock = value;
                switch (value)
                {
                    case DockStyle.None:
                        this.direction = xdbfa333b4cd503e0.Horizontal;
                        break;
                    case DockStyle.Top:
                    case DockStyle.Bottom:
                        base.Height = 0;
                        this.direction = xdbfa333b4cd503e0.Horizontal;
                        break;
                    case DockStyle.Left:
                    case DockStyle.Right:
                        base.Width = 0;
                        this.direction = xdbfa333b4cd503e0.Vertical;
                        break;
                }
                this.x39e008704a72ea56();
                base.Invalidate();
            }
        }

        public x6fd23f8bad2f3ced Animate
        {
            get
            {
                return this.animate;
            }
            set
            {
                this.animate = value;
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

        public xc69458cec0f3af75 GlyphFading
        {
            get
            {
                return this.glyphFading;
            }
            set
            {
                this.glyphFading = value;
            }
        }

        public Form MdiContainer
        {
            get
            {
                return this.mdiContainer;
            }
            set
            {
                if (this.mdiContainer != value)
                {
                    if (this.mdiContainer != null)
                    {
                        this.mdiContainer.MdiChildActivate -= new EventHandler(this.x23daf02978257d54);
                        this.xb73b905145ee3f9f();
                    }
                    this.mdiContainer = value;
                    if (this.mdiContainer != null)
                    {
                        this.x9954cbb39675570d();
                        this.mdiContainer.MdiChildActivate += new EventHandler(this.x23daf02978257d54);
                        this.x23daf02978257d54(null, null);
                    }
                }
            }
        }

        static x3c41176af7e54b01()
        {
            x3c41176af7e54b01.menuImages = x58dd58a96343fde0.LoadBitmapStrip(Type.GetType("Sunisoft.IrisSkin.SkinEngine"), "Sunisoft.IrisSkin.ImagesMenuControl.bmp", new Size(16, 16), new Point(0, 0));
            x3c41176af7e54b01.supportsLayered = (OSFeature.Feature.GetVersionPresent(OSFeature.LayeredWindows) != null);
        }

        public x3c41176af7e54b01(SkinEngine engine)
        {
            this.x54f4e17b11b07b8c.xb2504b89d66feca9 = true;
            this.Engine = engine;
            this.trackItem = -1;
            this.oldFocus = IntPtr.Zero;
            this.minButton = null;
            this.popupMenu = null;
            this.activeChild = null;
            this.closeButton = null;
            this.mdiContainer = null;
            this.restoreButton = null;
            this.chevronStartCommand = null;
            this.animateFirst = true;
            this.exitLoop = false;
            this.selected = false;
            this.multiLine = false;
            this.mouseOver = false;
            this.defaultFont = true;
            this.manualFocus = false;
            this.drawUpwards = false;
            this.plainAsBlock = false;
            this.ignoreMouseMove = false;
            this.deselectReset = true;
            this.expandAllTogether = true;
            this.rememberExpansion = true;
            this.highlightInfrequent = true;
            this.dismissTransfer = false;
            this.direction = xdbfa333b4cd503e0.Horizontal;
            this.menuCommands = new xd53b20b7b4b2a08a();
            this.glyphFading = xc69458cec0f3af75.Default;
            this.Dock = DockStyle.Top;
            this.Cursor = Cursors.Arrow;
            this.animateTime = 100;
            this.animate = x6fd23f8bad2f3ced.System;
            this.animateStyle = x1f5697535eab37b9.System;
            base.SetStyle(ControlStyles.UserPaint | ControlStyles.AllPaintingInWmPaint | ControlStyles.DoubleBuffer, true);
            base.SetStyle(ControlStyles.Selectable, false);
            this.menuCommands.Cleared += new x66edf89974942dab(this.x4d43bed1b4aede85);
            this.menuCommands.Inserted += new x2f6aff803d60b50c(this.x6b08695ba39cf4ca);
            this.menuCommands.Removed += new x2f6aff803d60b50c(this.x5cb0c63a7cf35b53);
            SystemEvents.UserPreferenceChanged += new UserPreferenceChangedEventHandler(this.x288bea726e7cd27e);
            if (this.Engine.MenuFont == null)
            {
                this.x21498b72da0020ba(SystemInformation.MenuFont);
            }
            else
            {
                this.x21498b72da0020ba(this.Engine.MenuFont);
            }
            base.TabStop = false;
            base.Height = this.rowHeight;
            Application.AddMessageFilter(this);
            this.Engine.CurrentSkinChanged += new SkinChanged(this.x273cbe3cfa28ea94);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                SystemEvents.UserPreferenceChanged -= new UserPreferenceChangedEventHandler(this.x288bea726e7cd27e);
                if (this.mdiContainer != null)
                {
                    this.mdiContainer.MdiChildActivate -= new EventHandler(this.x23daf02978257d54);
                }
                if (this.activeChild != null)
                {
                    this.activeChild.SizeChanged -= new EventHandler(this.xfbce0f1d6df984cd);
                }
            }
            base.Dispose(disposing);
        }

        private bool x8b3f8f5d533e5ee1()
        {
            return this.BackColor != SystemColors.Control;
        }

        protected void x21498b72da0020ba(Font x978809f7734a0f16)
        {
            base.Font = x978809f7734a0f16;
            this.breadthGap = this.Font.Height / 3 + 1;
            this.rowWidth = (this.rowHeight = this.Font.Height + this.breadthGap * 2 + 1);
        }

        public virtual void OnSelected(x5f4b657f68f87baa mc)
        {
            if (this.xaa7558c320af04eb != null)
            {
                this.xaa7558c320af04eb(mc);
            }
        }

        public virtual void OnDeselected(x5f4b657f68f87baa mc)
        {
            if (this.xd42ce0324cbc114a != null)
            {
                this.xd42ce0324cbc114a(mc);
            }
        }

        protected void x4d43bed1b4aede85()
        {
            this.xcb97d93f0d5ce4a8();
            this.x51ce8d251f940283();
            this.x39e008704a72ea56();
            base.Invalidate();
        }

        protected void x6b08695ba39cf4ca(int xc0c4c459c6ccbd00, object xbcea506a33cf9111)
        {
            x5f4b657f68f87baa x5f4b657f68f87baa = xbcea506a33cf9111 as x5f4b657f68f87baa;
            x5f4b657f68f87baa.PropertyChanged += new x5f4b657f68f87baa.x5282b9c42ea1abce(this.x2b1f4f4697e5ae02);
            this.xcb97d93f0d5ce4a8();
            this.x51ce8d251f940283();
            this.x39e008704a72ea56();
            base.Invalidate();
        }

        protected void x5cb0c63a7cf35b53(int xc0c4c459c6ccbd00, object xbcea506a33cf9111)
        {
            this.xcb97d93f0d5ce4a8();
            this.x51ce8d251f940283();
            this.x39e008704a72ea56();
            base.Invalidate();
        }

        protected void x2b1f4f4697e5ae02(x5f4b657f68f87baa xccb63ca5f63dc470, x5f4b657f68f87baa.x879a106b0501b9dc x5ca6b6e12a4d9043)
        {
            this.x39e008704a72ea56();
            base.Invalidate();
        }

        private void x1996f82940a97595()
        {
            string arg = "";
            bool @checked = false;
            if (this.mdiContainer != null)
            {
                this.x4dedac25f8166fb5 = new x5f4b657f68f87baa[this.mdiContainer.MdiChildren.Length];
                for (int i = 0; i < this.mdiContainer.MdiChildren.Length; i++)
                {
                    arg = "";
                    @checked = false;
                    try
                    {
                        arg = this.mdiContainer.MdiChildren[i].Text;
                        @checked = (this.mdiContainer.MdiChildren[i] == this.mdiContainer.ActiveMdiChild);
                    }
                    catch
                    {
                    }
                    this.x4dedac25f8166fb5[i] = new x5f4b657f68f87baa(string.Format("&{0} {1}", i + 1, arg), new EventHandler(this.x43d97dcf909b3bd3));
                    this.x4dedac25f8166fb5[i].Checked = @checked;
                    this.x4dedac25f8166fb5[i].xb7d28b7a6d50662f = this.mdiContainer.MdiChildren[i];
                    this.x4dedac25f8166fb5[i].xb2504b89d66feca9 = true;
                }
            }
            ArrayList arrayList = new ArrayList();
            IEnumerator enumerator = this.MenuCommands.GetEnumerator();
            while (enumerator.MoveNext())
            {
                x5f4b657f68f87baa x5f4b657f68f87baa = (x5f4b657f68f87baa)enumerator.Current;
                if (x5f4b657f68f87baa.MdiList)
                {
                    this.xa76c42f3062c6f11(x5f4b657f68f87baa);
                }
                if (x5f4b657f68f87baa.MenuCommands.Count > 0)
                {
                    arrayList.Add(x5f4b657f68f87baa);
                }
            }
            goto IL_1D4;
        IL_157:
            x5f4b657f68f87baa x5f4b657f68f87baa2 = (x5f4b657f68f87baa)arrayList[0];
            arrayList.RemoveAt(0);
            foreach (x5f4b657f68f87baa x5f4b657f68f87baa3 in x5f4b657f68f87baa2.MenuCommands)
            {
                if (x5f4b657f68f87baa3.MdiList)
                {
                    this.xa76c42f3062c6f11(x5f4b657f68f87baa3);
                }
                if (x5f4b657f68f87baa3.MenuCommands.Count > 0)
                {
                    arrayList.Add(x5f4b657f68f87baa3);
                }
            }
        IL_1D4:
            if (arrayList.Count <= 0)
            {
                return;
            }
            goto IL_157;
        }

        private void xa76c42f3062c6f11(x5f4b657f68f87baa x61b060a94340c4fc)
        {
            for (int i = x61b060a94340c4fc.MenuCommands.Count - 1; i >= 0; i--)
            {
                if (x61b060a94340c4fc.MenuCommands[i].xb2504b89d66feca9)
                {
                    x61b060a94340c4fc.MenuCommands.RemoveAt(i);
                }
            }
            if (x61b060a94340c4fc.MenuCommands.Count > 0)
            {
                x61b060a94340c4fc.MenuCommands.Add(this.x54f4e17b11b07b8c);
            }
            x5f4b657f68f87baa[] array = this.x4dedac25f8166fb5;
            for (int j = 0; j < array.Length; j++)
            {
                x5f4b657f68f87baa value = array[j];
                x61b060a94340c4fc.MenuCommands.Add(value);
            }
        }

        private void x43d97dcf909b3bd3(object xe0292b9ed559da7d, EventArgs xfbf34718e704c6bc)
        {
            if (xe0292b9ed559da7d is x5f4b657f68f87baa)
            {
                try
                {
                    ((x5f4b657f68f87baa)xe0292b9ed559da7d).xb7d28b7a6d50662f.Activate();
                }
                catch
                {
                }
            }
        }

        protected void x23daf02978257d54(object xe0292b9ed559da7d, EventArgs xfbf34718e704c6bc)
        {
            if (this.activeChild != null)
            {
                this.activeChild.SizeChanged -= new EventHandler(this.xfbce0f1d6df984cd);
            }
            this.activeChild = this.mdiContainer.ActiveMdiChild;
            if (this.activeChild != null)
            {
                this.activeChild.SizeChanged += new EventHandler(this.xfbce0f1d6df984cd);
            }
            this.x1996f82940a97595();
            this.x39e008704a72ea56();
            base.Invalidate();
            if (this.MenuCommands.Count != 0)
            {
                base.Visible = true;
                return;
            }
            if (this.activeChild == null)
            {
                base.Visible = false;
                return;
            }
            if (this.activeChild.WindowState == FormWindowState.Maximized)
            {
                base.Visible = true;
                return;
            }
            base.Visible = false;
        }

        protected void xfbce0f1d6df984cd(object xe0292b9ed559da7d, EventArgs xfbf34718e704c6bc)
        {
            if (this.childMenu != null)
            {
                foreach (x5f4b657f68f87baa value in this.childMenu.MenuCommands)
                {
                    if (this.MenuCommands.Contains(value))
                    {
                        this.MenuCommands.Remove(value);
                    }
                }
                if (this.childMenu.Tag is Form)
                {
                    Form form = (Form)this.childMenu.Tag;
                    if (!form.IsDisposed)
                    {
                        form.Controls.Add(this.childMenu);
                    }
                }
                this.childMenu = null;
            }
            if (this.activeChild.WindowState == FormWindowState.Maximized)
            {
                foreach (Control control in this.activeChild.Controls)
                {
                    if (control is x3c41176af7e54b01)
                    {
                        this.childMenu = (x3c41176af7e54b01)control;
                        this.childMenu.Tag = this.activeChild;
                        this.activeChild.Controls.Remove(control);
                        break;
                    }
                    this.childMenu = null;
                }
                if (this.childMenu != null)
                {
                    foreach (x5f4b657f68f87baa value2 in this.childMenu.MenuCommands)
                    {
                        this.MenuCommands.Add(value2);
                    }
                }
                this.x39e008704a72ea56();
                base.Invalidate();
            }
            if (this.MenuCommands.Count != 0)
            {
                base.Visible = true;
                return;
            }
            if (this.activeChild == null)
            {
                base.Visible = false;
                return;
            }
            if (this.activeChild.WindowState == FormWindowState.Maximized)
            {
                base.Visible = true;
                return;
            }
            base.Visible = false;
        }

        protected void x199464e739d66b0b(object xe0292b9ed559da7d, EventArgs xfbf34718e704c6bc)
        {
            if (this.activeChild != null)
            {
                this.activeChild.WindowState = FormWindowState.Minimized;
                this.x39e008704a72ea56();
                base.Invalidate();
            }
        }

        protected void x6b3bc391561e756b(object xe0292b9ed559da7d, EventArgs xfbf34718e704c6bc)
        {
            if (this.activeChild != null)
            {
                this.activeChild.WindowState = FormWindowState.Normal;
                this.x39e008704a72ea56();
                base.Invalidate();
            }
        }

        protected void x0b912a2b95b630ba(object xe0292b9ed559da7d, EventArgs xfbf34718e704c6bc)
        {
            if (this.activeChild != null)
            {
                this.activeChild.Close();
                this.x39e008704a72ea56();
                base.Invalidate();
            }
        }

        protected void x9954cbb39675570d()
        {
            this.minButton = new x5d3356d9dffccb60(x3c41176af7e54b01.menuImages, 0);
            this.restoreButton = new x5d3356d9dffccb60(x3c41176af7e54b01.menuImages, 1);
            this.closeButton = new x5d3356d9dffccb60(x3c41176af7e54b01.menuImages, 2);
            this.minButton.BackColor = this.Engine.Res.Colors.SKIN2_MENUBARSTARTCOLOR;
            this.restoreButton.BackColor = this.Engine.Res.Colors.SKIN2_MENUBARSTARTCOLOR;
            this.closeButton.BackColor = this.Engine.Res.Colors.SKIN2_MENUBARSTARTCOLOR;
            this.minButton.Size = new Size(16, 16);
            this.restoreButton.Size = new Size(16, 16);
            this.closeButton.Size = new Size(16, 16);
            this.minButton.Location = new Point(-16, -16);
            this.restoreButton.Location = new Point(-16, -16);
            this.closeButton.Location = new Point(-16, -16);
            this.minButton.Click += new EventHandler(this.x199464e739d66b0b);
            this.restoreButton.Click += new EventHandler(this.x6b3bc391561e756b);
            this.closeButton.Click += new EventHandler(this.x0b912a2b95b630ba);
            base.Controls.AddRange(new Control[]
			{
				this.minButton,
				this.restoreButton,
				this.closeButton
			});
        }

        protected void xb73b905145ee3f9f()
        {
            this.minButton.Click -= new EventHandler(this.x199464e739d66b0b);
            this.restoreButton.Click -= new EventHandler(this.x6b3bc391561e756b);
            this.closeButton.Click -= new EventHandler(this.x0b912a2b95b630ba);
            x448fd9ab43628c71.RemoveControl(base.Controls, this.minButton);
            x448fd9ab43628c71.RemoveControl(base.Controls, this.restoreButton);
            x448fd9ab43628c71.RemoveControl(base.Controls, this.closeButton);
            this.minButton.Dispose();
            this.restoreButton.Dispose();
            this.closeButton.Dispose();
            this.minButton = null;
            this.restoreButton = null;
            this.closeButton = null;
        }

        protected override void OnEnabledChanged(EventArgs e)
        {
            base.OnEnabledChanged(e);
            if (!base.Enabled && this.selected)
            {
                if (this.popupMenu != null)
                {
                    this.popupMenu.Dismiss();
                    this.popupMenu = null;
                }
                this.xcb97d93f0d5ce4a8();
                this.drawUpwards = false;
                this.xa23d095e6e52c5f3();
            }
            this.x51ce8d251f940283();
            base.Invalidate();
        }

        internal void x2ea07c3ab5d970d4(x555516122dcc901e.POINT x0ce73f6cbd7d5515)
        {
            x61467fe65a98f20c.ScreenToClient(base.Handle, ref x0ce73f6cbd7d5515);
            this.xbcc99f3d02a70fe7(x0ce73f6cbd7d5515.x, x0ce73f6cbd7d5515.y);
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            this.xbcc99f3d02a70fe7(e.X, e.Y);
            base.OnMouseDown(e);
        }

        protected void xbcc99f3d02a70fe7(int x75cf7df8c59ffa4d, int xc13ed6de98262a2d)
        {
            Point pt = new Point(x75cf7df8c59ffa4d, xc13ed6de98262a2d);
            int i = 0;
            while (i < this.drawCommands.Count)
            {
                x2cc390e9409b0f3f x2cc390e9409b0f3f = this.drawCommands[i] as x2cc390e9409b0f3f;
                if (x2cc390e9409b0f3f.SelectRect.Contains(pt) && x2cc390e9409b0f3f.Enabled)
                {
                    if (x2cc390e9409b0f3f.MenuCommand != null && x2cc390e9409b0f3f.MenuCommand.MenuCommands.Count == 0)
                    {
                        x2cc390e9409b0f3f.MenuCommand.PerformClick();
                    }
                    if (this.selected)
                    {
                        if (this.trackItem == i && this.popupMenu != null)
                        {
                            this.popupMenu.Dismiss();
                            this.popupMenu = null;
                            return;
                        }
                        break;
                    }
                    else
                    {
                        this.selected = true;
                        this.drawUpwards = false;
                        if (this.trackItem != i)
                        {
                            this.trackItem = this.xa7cbddf1ec7a27e7(this.trackItem, i);
                        }
                        else
                        {
                            this.x2cc390e9409b0f3f(this.trackItem, true);
                        }
                        if (x2cc390e9409b0f3f.Chevron || x2cc390e9409b0f3f.MenuCommand.MenuCommands.Count > 0)
                        {
                            x61467fe65a98f20c.PostMessage(base.Handle, this.WM_OPERATEMENU, 1u, 0u);
                            return;
                        }
                        break;
                    }
                }
                else
                {
                    i++;
                }
            }
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            if (this.trackItem != -1 && this.selected && this.popupMenu == null)
            {
                this.xcb97d93f0d5ce4a8();
                this.drawUpwards = false;
                this.x2cc390e9409b0f3f(this.trackItem, true);
                this.xa23d095e6e52c5f3();
            }
            base.OnMouseUp(e);
        }

        internal void x136735fdfe6d04ea(x555516122dcc901e.POINT x0ce73f6cbd7d5515)
        {
            x61467fe65a98f20c.ScreenToClient(base.Handle, ref x0ce73f6cbd7d5515);
            this.xdd922e6d558c59d4(x0ce73f6cbd7d5515.x, x0ce73f6cbd7d5515.y);
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            if (this.ignoreMouseMove)
            {
                this.ignoreMouseMove = false;
            }
            else
            {
                this.xdd922e6d558c59d4(e.X, e.Y);
            }
            base.OnMouseMove(e);
        }

        protected void xdd922e6d558c59d4(int x75cf7df8c59ffa4d, int xc13ed6de98262a2d)
        {
            if (this.ignoreMouseMove)
            {
                this.ignoreMouseMove = false;
                return;
            }
            if (!this.mouseOver)
            {
                x40255b11ef821fa3.TRACKMOUSEEVENTS tRACKMOUSEEVENTS = default(x40255b11ef821fa3.TRACKMOUSEEVENTS);
                tRACKMOUSEEVENTS.cbSize = 16u;
                tRACKMOUSEEVENTS.dwFlags = 2u;
                tRACKMOUSEEVENTS.hWnd = base.Handle;
                tRACKMOUSEEVENTS.dwHoverTime = 0u;
                x61467fe65a98f20c.TrackMouseEvent(ref tRACKMOUSEEVENTS);
                this.mouseOver = true;
            }
            Form form = base.FindForm();
            if (form != null && form.ContainsFocus)
            {
                Point pt = new Point(x75cf7df8c59ffa4d, xc13ed6de98262a2d);
                int i = 0;
                while (i < this.drawCommands.Count)
                {
                    x2cc390e9409b0f3f x2cc390e9409b0f3f = this.drawCommands[i] as x2cc390e9409b0f3f;
                    if (x2cc390e9409b0f3f.SelectRect.Contains(pt) && x2cc390e9409b0f3f.Enabled)
                    {
                        if (this.trackItem == i)
                        {
                            break;
                        }
                        if (!this.selected)
                        {
                            this.trackItem = this.xa7cbddf1ec7a27e7(this.trackItem, i);
                            break;
                        }
                        if (this.popupMenu != null)
                        {
                            this.dismissTransfer = true;
                            this.popupMenu.Dismiss();
                            this.drawUpwards = false;
                        }
                        this.trackItem = this.xa7cbddf1ec7a27e7(this.trackItem, i);
                        if (x2cc390e9409b0f3f.Chevron || x2cc390e9409b0f3f.MenuCommand.MenuCommands.Count > 0)
                        {
                            x61467fe65a98f20c.PostMessage(base.Handle, this.WM_OPERATEMENU, 1u, 0u);
                            break;
                        }
                        break;
                    }
                    else
                    {
                        i++;
                    }
                }
                if (!this.selected && i == this.drawCommands.Count && !this.manualFocus)
                {
                    this.trackItem = this.xa7cbddf1ec7a27e7(this.trackItem, -1);
                }
            }
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            this.mouseOver = false;
            if (!this.manualFocus && this.trackItem != -1 && !this.selected)
            {
                this.trackItem = this.xa7cbddf1ec7a27e7(this.trackItem, -1);
            }
            base.OnMouseLeave(e);
        }

        protected override void OnResize(EventArgs e)
        {
            this.x39e008704a72ea56();
            base.Invalidate();
            base.OnResize(e);
        }

        internal void xd502a472a14a9c04()
        {
            if (this.trackItem != -1 && this.selected)
            {
                this.drawUpwards = true;
                this.x2cc390e9409b0f3f(this.trackItem, true);
            }
        }

        protected void xcb97d93f0d5ce4a8()
        {
            this.animateFirst = true;
            this.selected = false;
            if (this.deselectReset)
            {
                this.xe8febcf8320c4113(this.menuCommands, false);
            }
        }

        internal void x39e008704a72ea56()
        {
            try
            {
                int num;
                if (this.direction == xdbfa333b4cd503e0.Horizontal)
                {
                    num = base.Width;
                }
                else
                {
                    num = base.Height;
                }
                if (num > 0)
                {
                    int num2 = 0;
                    int num3 = 0;
                    this.drawCommands = new ArrayList();
                    int num4 = 6;
                    int arg_37_0 = base.Height;
                    int num5 = 0;
                    num -= num4 + 12;
                    bool flag = num2 == 0 && this.activeChild != null;
                    if (flag && !this.multiLine && this.activeChild.WindowState != FormWindowState.Maximized)
                    {
                        flag = false;
                    }
                    int num6 = 0;
                    int num7 = 0;
                    int num8 = 0;
                    int num9 = 0;
                    if (flag && this.minButton != null)
                    {
                        num -= 55;
                        bool popupStyle = true;
                        int borderWidth = 1;
                        this.minButton.PopupStyle = popupStyle;
                        this.restoreButton.PopupStyle = popupStyle;
                        this.closeButton.PopupStyle = popupStyle;
                        this.minButton.BorderWidth = borderWidth;
                        this.restoreButton.BorderWidth = borderWidth;
                        this.closeButton.BorderWidth = borderWidth;
                        if (this.direction == xdbfa333b4cd503e0.Horizontal)
                        {
                            num6 = base.Width - 3 - 16;
                            num7 = 3;
                            num8 = -16;
                        }
                        else
                        {
                            num6 = 3;
                            num7 = base.Height - 3 - 16;
                            num9 = -16;
                        }
                    }
                    this.chevronStartCommand = null;
                    using (Graphics graphics = base.CreateGraphics())
                    {
                        int num10 = 0;
                        foreach (x5f4b657f68f87baa x5f4b657f68f87baa in this.menuCommands)
                        {
                            x5f4b657f68f87baa.OnUpdate(EventArgs.Empty);
                            if (x5f4b657f68f87baa.Visible)
                            {
                                int num11;
                                if (x5f4b657f68f87baa.Text == "-")
                                {
                                    num11 = 15;
                                }
                                else
                                {
                                    num11 = num4 + (int)graphics.MeasureString(x5f4b657f68f87baa.Text, this.Font).Width + 1;
                                }
                                Rectangle rectangle;
                                if (this.direction == xdbfa333b4cd503e0.Horizontal)
                                {
                                    rectangle = new Rectangle(num5, this.rowHeight * num2, num11, this.rowHeight);
                                }
                                else
                                {
                                    rectangle = new Rectangle(this.rowWidth * num2, num5, this.rowWidth, num11);
                                }
                                num5 += num11;
                                num3++;
                                if (num5 > num && num3 > 1)
                                {
                                    if (this.multiLine)
                                    {
                                        num2++;
                                        num3 = 1;
                                        num5 = num11;
                                        if (this.direction == xdbfa333b4cd503e0.Horizontal)
                                        {
                                            rectangle.X = 0;
                                            rectangle.Y += this.rowHeight;
                                        }
                                        else
                                        {
                                            rectangle.X += this.rowWidth;
                                            rectangle.Y = 0;
                                        }
                                        if (flag && num2 == 1)
                                        {
                                            num += 51;
                                        }
                                    }
                                    else
                                    {
                                        if (num10 <= this.trackItem)
                                        {
                                            this.x51ce8d251f940283();
                                        }
                                        this.chevronStartCommand = x5f4b657f68f87baa;
                                        if (this.direction == xdbfa333b4cd503e0.Horizontal)
                                        {
                                            rectangle.Y = 0;
                                            rectangle.Width = num4 + 12;
                                            rectangle.X = base.Width - rectangle.Width;
                                            rectangle.Height = this.rowHeight;
                                            num6 -= rectangle.Width;
                                        }
                                        else
                                        {
                                            rectangle.X = 0;
                                            rectangle.Height = num4 + 12;
                                            rectangle.Y = base.Height - (num4 + 12);
                                            rectangle.Width = this.rowWidth;
                                            num7 -= rectangle.Height;
                                        }
                                        if ((this.RightToLeft & RightToLeft.Yes) == RightToLeft.Yes)
                                        {
                                            this.drawCommands.Add(new x2cc390e9409b0f3f(new Rectangle(num - rectangle.Right, rectangle.Y, rectangle.Width, rectangle.Height)));
                                            break;
                                        }
                                        this.drawCommands.Add(new x2cc390e9409b0f3f(rectangle));
                                        break;
                                    }
                                }
                                Rectangle selectRect = rectangle;
                                if (this.direction == xdbfa333b4cd503e0.Horizontal)
                                {
                                    selectRect.Height -= 5;
                                }
                                else
                                {
                                    selectRect.Width -= this.breadthGap;
                                }
                                if ((this.RightToLeft & RightToLeft.Yes) == RightToLeft.Yes)
                                {
                                    this.drawCommands.Add(new x2cc390e9409b0f3f(x5f4b657f68f87baa, new Rectangle(num - rectangle.Right, rectangle.Y, rectangle.Width, rectangle.Height), new Rectangle(num - selectRect.Right, selectRect.Y, selectRect.Width, selectRect.Height)));
                                }
                                else
                                {
                                    this.drawCommands.Add(new x2cc390e9409b0f3f(x5f4b657f68f87baa, rectangle, selectRect));
                                }
                                num10++;
                            }
                        }
                    }
                    if (flag && this.minButton != null)
                    {
                        if (this.activeChild.WindowState == FormWindowState.Maximized)
                        {
                            if (!this.minButton.Visible)
                            {
                                this.minButton.Show();
                                this.restoreButton.Show();
                                this.closeButton.Show();
                            }
                            this.minButton.Enabled = this.activeChild.MinimizeBox;
                            this.closeButton.Location = new Point(num6, num7);
                            num6 += num8;
                            num7 += num9;
                            this.restoreButton.Location = new Point(num6, num7);
                            num6 += num8;
                            num7 += num9;
                            this.minButton.Location = new Point(num6, num7);
                        }
                        else if (this.minButton.Visible)
                        {
                            this.minButton.Hide();
                            this.restoreButton.Hide();
                            this.closeButton.Hide();
                        }
                    }
                    else if (this.minButton != null && this.minButton.Visible)
                    {
                        this.minButton.Hide();
                        this.restoreButton.Hide();
                        this.closeButton.Hide();
                    }
                    if (this.direction == xdbfa333b4cd503e0.Horizontal)
                    {
                        int num12 = (num2 + 1) * this.rowHeight;
                        if (base.Height != num12)
                        {
                            base.Height = num12;
                        }
                    }
                    else
                    {
                        int num13 = (num2 + 1) * this.rowWidth;
                        if (base.Width != num13)
                        {
                            base.Width = num13;
                        }
                    }
                }
            }
            catch
            {
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            if (base.Width == 0 && base.Height == 0)
            {
                return;
            }
            Rectangle rectangle = new Rectangle(0, 0, base.Width, base.Height);
            Bitmap sKIN2_MENUBAR = this.Engine.Res.Bitmaps.SKIN2_MENUBAR;
            using (Bitmap bitmap = new Bitmap(base.Width, base.Height))
            {
                using (Graphics graphics = Graphics.FromImage(bitmap))
                {
                    if (sKIN2_MENUBAR != null)
                    {
                        graphics.DrawImage(sKIN2_MENUBAR, rectangle, 0, 0, sKIN2_MENUBAR.Width, sKIN2_MENUBAR.Height, GraphicsUnit.Pixel, x448fd9ab43628c71.DrawImageAttrTileY);
                    }
                    else
                    {
                        if (this.Engine.Res.Colors.SKIN2_MENUBARSTARTCOLOR != this.Engine.Res.Colors.SKIN2_MENUBARENDCOLOR)
                        {
                            using (LinearGradientBrush linearGradientBrush = new LinearGradientBrush(rectangle, this.Engine.Res.Colors.SKIN2_MENUBARSTARTCOLOR, this.Engine.Res.Colors.SKIN2_MENUBARENDCOLOR, LinearGradientMode.Vertical))
                            {
                                graphics.FillRectangle(linearGradientBrush, rectangle);
                                goto IL_110;
                            }
                        }
                        graphics.FillRectangle(this.Engine.Res.Brushes.SKIN2_MENUBARSTARTCOLOR, rectangle);
                    }
                IL_110: ;
                }
                e.Graphics.DrawImageUnscaled(bitmap, 0, 0);
            }
            this.x05a10b378cdf8119(e.Graphics);
            base.OnPaint(e);
        }

        protected void x2cc390e9409b0f3f(int x22f50445643d053e, bool x4b9cb2187c13edda)
        {
            using (Graphics graphics = base.CreateGraphics())
            {
                this.xb5a5bdae3ba8936b(graphics, this.drawCommands[x22f50445643d053e] as x2cc390e9409b0f3f, x4b9cb2187c13edda);
            }
        }

        internal void xb5a5bdae3ba8936b(Graphics x4b101060f4767186, x2cc390e9409b0f3f xd2a8bb4342ab4ef6, bool x4b9cb2187c13edda)
        {
            Rectangle drawRect = xd2a8bb4342ab4ef6.DrawRect;
            x5f4b657f68f87baa menuCommand = xd2a8bb4342ab4ef6.MenuCommand;
            Rectangle rectangle = drawRect;
            rectangle.Width += 4;
            rectangle.Height += 4;
            if (!xd2a8bb4342ab4ef6.Separator)
            {
                Rectangle rectangle2;
                if (xd2a8bb4342ab4ef6.Chevron)
                {
                    rectangle2 = new Rectangle(drawRect.Left + 3, drawRect.Top + 1, drawRect.Width - 6, drawRect.Height - 2);
                }
                else
                {
                    rectangle2 = new Rectangle(drawRect.Left + 3, drawRect.Top + 3, drawRect.Width - 6, drawRect.Height - 6);
                }
                if (xd2a8bb4342ab4ef6.Enabled && x4b9cb2187c13edda)
                {
                    Rectangle rect;
                    if (this.direction == xdbfa333b4cd503e0.Horizontal)
                    {
                        rect = new Rectangle(rectangle2.Left - 2, rectangle2.Top - 1, rectangle2.Width + 4, rectangle2.Height + 1);
                    }
                    else if (!xd2a8bb4342ab4ef6.Chevron)
                    {
                        rect = new Rectangle(rectangle2.Left, rectangle2.Top - 2, rectangle2.Width - 2, rectangle2.Height + 4);
                    }
                    else
                    {
                        rect = rectangle2;
                    }
                    if (this.selected)
                    {
                        Brush brush = this.Engine.Res.Brushes.SKIN2_TOPSELECTEDMENUCOLOR;
                        x4b101060f4767186.FillRectangle(brush, rect);
                        Color color = Color.FromArgb(64, 0, 0, 0);
                        Color color2 = Color.FromArgb(48, 0, 0, 0);
                        Color color3 = Color.FromArgb(0, 0, 0, 0);
                        int num = rect.Right + 1;
                        int bottom = rect.Bottom;
                        if (this.drawUpwards && this.direction == xdbfa333b4cd503e0.Horizontal)
                        {
                            using (Pen pen = new Pen(this.Engine.Res.Colors.SKIN2_TOPSELECTEDMENUBORDERCOLOR))
                            {
                                x4b101060f4767186.DrawRectangle(pen, rect);
                            }
                            if (xd2a8bb4342ab4ef6.SubMenu)
                            {
                                int top = rect.Top;
                                int arg_1DE_0 = rect.Left;
                                int num2 = rect.Bottom + 1;
                                int num3 = rect.Left + 4;
                                int num4 = rect.Width + 1;
                                int num5 = 4;
                                Brush brush2;
                                Brush brush3;
                                Brush brush4;
                                Brush brush5;
                                if (x3c41176af7e54b01.supportsLayered)
                                {
                                    brush2 = new LinearGradientBrush(new Point(num, 9999), new Point(num + 4, 9999), color2, color3);
                                    brush3 = new LinearGradientBrush(new Point(num3 + 4, num2 - 4), new Point(num3, num2 + num5), color, color3);
                                    brush4 = new LinearGradientBrush(new Point(num3 + num4 - 4 - 2, num2 - 4 - 2), new Point(num3 + num4, num2 + num5), color, color3);
                                    brush5 = new LinearGradientBrush(new Point(9999, num2), new Point(9999, num2 + num5), color2, color3);
                                }
                                else
                                {
                                    brush2 = new SolidBrush(SystemColors.ControlDark);
                                    brush3 = brush2;
                                    brush5 = brush2;
                                    brush4 = brush2;
                                }
                                x4b101060f4767186.FillRectangle(brush2, new Rectangle(num, top, 4, bottom - top + 1));
                                x4b101060f4767186.FillRectangle(brush3, num3, num2, 4, num5);
                                x4b101060f4767186.FillRectangle(brush4, num3 + num4 - 4, num2, 4, num5);
                                x4b101060f4767186.FillRectangle(brush5, num3 + 4, num2, num4 - 8, num5);
                                if (x3c41176af7e54b01.supportsLayered)
                                {
                                    brush2.Dispose();
                                    brush3.Dispose();
                                    brush5.Dispose();
                                    brush4.Dispose();
                                }
                                else
                                {
                                    brush2.Dispose();
                                }
                            }
                        }
                        else
                        {
                            using (Pen pen2 = new Pen(this.Engine.Res.Brushes.SKIN2_TOPSELECTEDMENUBORDERCOLOR))
                            {
                                x4b101060f4767186.DrawRectangle(pen2, rect);
                            }
                            if (xd2a8bb4342ab4ef6.SubMenu)
                            {
                                if (this.direction == xdbfa333b4cd503e0.Horizontal)
                                {
                                    x4b101060f4767186.DrawLine(Pens.White, rect.Left, rect.Bottom, rect.Right, rect.Bottom);
                                    using (Pen pen3 = new Pen(this.Engine.Res.Brushes.SKIN2_TOPSELECTEDMENUFONTCOLOR))
                                    {
                                        x4b101060f4767186.DrawLine(pen3, rect.Left, rect.Bottom, rect.Right, rect.Bottom);
                                    }
                                    int num6 = rect.Top + 4;
                                    Brush brush6;
                                    if (x3c41176af7e54b01.supportsLayered)
                                    {
                                        using (LinearGradientBrush linearGradientBrush = new LinearGradientBrush(new Point(num - 4, num6 + 4), new Point(num + 4, num6), color, color3))
                                        {
                                            x4b101060f4767186.FillRectangle(linearGradientBrush, new Rectangle(num, num6, 4, 4));
                                            num6 += 4;
                                        }
                                        brush6 = new LinearGradientBrush(new Point(num, 9999), new Point(num + 4, 9999), color2, color3);
                                    }
                                    else
                                    {
                                        brush6 = new SolidBrush(SystemColors.ControlDark);
                                    }
                                    x4b101060f4767186.FillRectangle(brush6, new Rectangle(num, num6, 4, bottom - num6));
                                    brush6.Dispose();
                                }
                                else
                                {
                                    x4b101060f4767186.DrawLine(Pens.White, rect.Right, rect.Top, rect.Right, rect.Bottom);
                                    using (Pen pen4 = new Pen(this.Engine.Res.Brushes.SKIN2_TOPSELECTEDMENUFONTCOLOR))
                                    {
                                        x4b101060f4767186.DrawLine(pen4, rect.Right, rect.Top, rect.Right, rect.Bottom);
                                    }
                                    int num7 = rect.Left + 4;
                                    Brush brush7;
                                    if (x3c41176af7e54b01.supportsLayered)
                                    {
                                        using (LinearGradientBrush linearGradientBrush2 = new LinearGradientBrush(new Point(num7 + 4, bottom + 1 - 4), new Point(num7, bottom + 1 + 4), color, color3))
                                        {
                                            x4b101060f4767186.FillRectangle(linearGradientBrush2, new Rectangle(num7, bottom + 1, 4, 4));
                                            num7 += 4;
                                        }
                                        brush7 = new LinearGradientBrush(new Point(9999, bottom + 1), new Point(9999, bottom + 1 + 4), color2, color3);
                                    }
                                    else
                                    {
                                        brush7 = new SolidBrush(SystemColors.ControlDark);
                                    }
                                    x4b101060f4767186.FillRectangle(brush7, new Rectangle(num7, bottom + 1, bottom - num7 - 4, 4));
                                    brush7.Dispose();
                                }
                            }
                        }
                    }
                    else
                    {
                        x4b101060f4767186.FillRectangle(this.Engine.Res.Brushes.SKIN2_TOPSELECTEDMENUCOLOR, rect);
                        using (Pen pen5 = new Pen(this.Engine.Res.Colors.SKIN2_TOPSELECTEDMENUBORDERCOLOR))
                        {
                            x4b101060f4767186.DrawRectangle(pen5, rect);
                        }
                    }
                }
                if (xd2a8bb4342ab4ef6.Chevron)
                {
                    int num8 = drawRect.Top;
                    int num9 = drawRect.X + (drawRect.Width - 12) / 2;
                    if (this.selected)
                    {
                        num9++;
                        num8++;
                    }
                    x4b101060f4767186.DrawImage(x3c41176af7e54b01.menuImages.Images[3], num9, num8);
                    return;
                }
                StringFormat stringFormat = new StringFormat();
                stringFormat.FormatFlags = (StringFormatFlags.NoWrap | StringFormatFlags.NoClip);
                stringFormat.Alignment = StringAlignment.Center;
                stringFormat.LineAlignment = StringAlignment.Center;
                stringFormat.HotkeyPrefix = HotkeyPrefix.Show;
                if (this.direction == xdbfa333b4cd503e0.Vertical)
                {
                    stringFormat.FormatFlags |= StringFormatFlags.DirectionVertical;
                }
                if (xd2a8bb4342ab4ef6.Enabled && base.Enabled)
                {
                    Brush brush;
                    if (this.selected && x4b9cb2187c13edda)
                    {
                        brush = this.Engine.Res.Brushes.SKIN2_TOPSELECTEDMENUFONTCOLOR;
                        x4b101060f4767186.DrawString(menuCommand.Text, this.Font, brush, rectangle2, stringFormat);
                        return;
                    }
                    if (x4b9cb2187c13edda)
                    {
                        brush = this.Engine.Res.Brushes.SKIN2_TOPSELECTEDMENUFONTCOLOR;
                        x4b101060f4767186.DrawString(menuCommand.Text, this.Font, brush, rectangle2, stringFormat);
                        return;
                    }
                    brush = this.Engine.Res.Brushes.SKIN2_TOPMENUFONTCOLOR;
                    x4b101060f4767186.DrawString(menuCommand.Text, this.Font, brush, rectangle2, stringFormat);
                    return;
                }
                else
                {
                    Rectangle r = rectangle2;
                    r.Offset(1, 1);
                    x4b101060f4767186.DrawString(menuCommand.Text, this.Font, Brushes.White, r, stringFormat);
                    using (SolidBrush solidBrush = new SolidBrush(SystemColors.GrayText))
                    {
                        x4b101060f4767186.DrawString(menuCommand.Text, this.Font, solidBrush, rectangle2, stringFormat);
                    }
                }
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

        protected int xa7cbddf1ec7a27e7(int x5360a38cdebd0e93, int x169940ac3300e3e7)
        {
            using (Graphics graphics = base.CreateGraphics())
            {
                if (x5360a38cdebd0e93 != -1)
                {
                    x2cc390e9409b0f3f x2cc390e9409b0f3f = this.drawCommands[x5360a38cdebd0e93] as x2cc390e9409b0f3f;
                    this.xb5a5bdae3ba8936b(graphics, this.drawCommands[x5360a38cdebd0e93] as x2cc390e9409b0f3f, false);
                    if (x2cc390e9409b0f3f.MenuCommand != null)
                    {
                        this.OnDeselected(x2cc390e9409b0f3f.MenuCommand);
                    }
                }
                this.trackItem = x169940ac3300e3e7;
                if (this.trackItem != -1)
                {
                    x2cc390e9409b0f3f x2cc390e9409b0f3f2 = this.drawCommands[this.trackItem] as x2cc390e9409b0f3f;
                    this.xb5a5bdae3ba8936b(graphics, this.drawCommands[this.trackItem] as x2cc390e9409b0f3f, true);
                    if (x2cc390e9409b0f3f2.MenuCommand != null)
                    {
                        this.OnSelected(x2cc390e9409b0f3f2.MenuCommand);
                    }
                }
                base.Invalidate();
            }
            return this.trackItem;
        }

        protected void x51ce8d251f940283()
        {
            if (this.trackItem != -1)
            {
                x2cc390e9409b0f3f x2cc390e9409b0f3f = this.drawCommands[this.trackItem] as x2cc390e9409b0f3f;
                if (x2cc390e9409b0f3f.MenuCommand != null)
                {
                    this.OnDeselected(x2cc390e9409b0f3f.MenuCommand);
                }
                this.trackItem = -1;
            }
        }

        internal void x0760506d6abf77a2(x2cc390e9409b0f3f xd2a8bb4342ab4ef6, bool xacc37ebdd71fcc44, bool xec106df5fe2fbc8e)
        {
            if (base.IsDisposed)
            {
                return;
            }
            Rectangle drawRect = xd2a8bb4342ab4ef6.DrawRect;
            Point x0ce73f6cbd7d;
            if (this.direction == xdbfa333b4cd503e0.Horizontal)
            {
                x0ce73f6cbd7d = base.PointToScreen(new Point(xd2a8bb4342ab4ef6.DrawRect.Left + 1, drawRect.Bottom - 3 - 2));
            }
            else
            {
                x0ce73f6cbd7d = base.PointToScreen(new Point(xd2a8bb4342ab4ef6.DrawRect.Right - this.breadthGap, drawRect.Top + 2 - 1));
            }
            Point xd682fa060330cf;
            if (this.direction == xdbfa333b4cd503e0.Horizontal)
            {
                xd682fa060330cf = base.PointToScreen(new Point(xd2a8bb4342ab4ef6.DrawRect.Left + 1, drawRect.Top + this.breadthGap + 3 - 1));
            }
            else
            {
                xd682fa060330cf = base.PointToScreen(new Point(xd2a8bb4342ab4ef6.DrawRect.Right - this.breadthGap, drawRect.Bottom + 3 + 1));
            }
            int xb2c6baf52a3ff3eb;
            if (this.direction == xdbfa333b4cd503e0.Horizontal)
            {
                xb2c6baf52a3ff3eb = xd2a8bb4342ab4ef6.DrawRect.Width - 2;
            }
            else
            {
                xb2c6baf52a3ff3eb = xd2a8bb4342ab4ef6.DrawRect.Height - 2;
            }
            this.popupMenu = new x902c4aee45bfd906(this.Engine);
            int num = 0;
            this.popupMenu.RememberExpansion = this.rememberExpansion;
            this.popupMenu.HighlightInfrequent = this.highlightInfrequent;
            this.popupMenu.Font = base.Font;
            this.popupMenu.Animate = this.animate;
            this.popupMenu.AnimateStyle = this.animateStyle;
            this.popupMenu.AnimateTime = this.animateTime;
            x5f4b657f68f87baa x5f4b657f68f87baa2;
            if (xd2a8bb4342ab4ef6.Chevron)
            {
                xd53b20b7b4b2a08a xd53b20b7b4b2a08a = new xd53b20b7b4b2a08a();
                bool flag = false;
                foreach (x5f4b657f68f87baa x5f4b657f68f87baa in this.menuCommands)
                {
                    if (!flag && x5f4b657f68f87baa == this.chevronStartCommand)
                    {
                        flag = true;
                    }
                    if (flag)
                    {
                        xd53b20b7b4b2a08a.Add(x5f4b657f68f87baa);
                    }
                }
                x5f4b657f68f87baa2 = this.popupMenu.x6192996f26aa9421(x0ce73f6cbd7d, xd682fa060330cf, this.direction, xd53b20b7b4b2a08a, xb2c6baf52a3ff3eb, xacc37ebdd71fcc44, this, this.animateFirst, ref num);
            }
            else
            {
                xd2a8bb4342ab4ef6.MenuCommand.OnPopupStart();
                this.popupMenu.ShowInfrequent = xd2a8bb4342ab4ef6.MenuCommand.MenuCommands.ShowInfrequent;
                x5f4b657f68f87baa2 = this.popupMenu.x6192996f26aa9421(x0ce73f6cbd7d, xd682fa060330cf, this.direction, xd2a8bb4342ab4ef6.MenuCommand.MenuCommands, xb2c6baf52a3ff3eb, xacc37ebdd71fcc44, this, this.animateFirst, ref num);
            }
            this.animateFirst = false;
            if (this.expandAllTogether && this.x35a6958370d758dd(this.menuCommands))
            {
                this.xe8febcf8320c4113(this.menuCommands, true);
            }
            if (num == 0)
            {
                this.mouseOver = false;
                if (!this.dismissTransfer)
                {
                    this.xcb97d93f0d5ce4a8();
                    this.drawUpwards = false;
                    if (!base.IsDisposed)
                    {
                        if (xec106df5fe2fbc8e)
                        {
                            this.trackItem = this.xa7cbddf1ec7a27e7(this.trackItem, -1);
                        }
                        else if (this.trackItem != -1)
                        {
                            this.x2cc390e9409b0f3f(this.trackItem, true);
                        }
                    }
                }
                else
                {
                    this.dismissTransfer = false;
                }
            }
            if (!xd2a8bb4342ab4ef6.Chevron)
            {
                xd2a8bb4342ab4ef6.MenuCommand.OnPopupEnd();
            }
            Application.DoEvents();
            this.popupMenu = null;
            if (num != 0)
            {
                if (num < 0)
                {
                    this.x58049cd54e5ca4e3(true);
                }
                else
                {
                    this.x14014e142c6b152c(true);
                }
                this.ignoreMouseMove = true;
                return;
            }
            if (x5f4b657f68f87baa2 != null)
            {
                if (this.manualFocus)
                {
                    this.xa23d095e6e52c5f3();
                }
                x5f4b657f68f87baa2.OnClick(EventArgs.Empty);
            }
        }

        protected bool x35a6958370d758dd(xd53b20b7b4b2a08a x20e3b25d6d144908)
        {
            foreach (x5f4b657f68f87baa x5f4b657f68f87baa in x20e3b25d6d144908)
            {
                if (x5f4b657f68f87baa.MenuCommands.ShowInfrequent)
                {
                    bool result = true;
                    return result;
                }
                if (this.x35a6958370d758dd(x5f4b657f68f87baa.MenuCommands))
                {
                    bool result = true;
                    return result;
                }
            }
            return false;
        }

        protected void xe8febcf8320c4113(xd53b20b7b4b2a08a x20e3b25d6d144908, bool x789c645a15deb49b)
        {
            foreach (x5f4b657f68f87baa x5f4b657f68f87baa in x20e3b25d6d144908)
            {
                x5f4b657f68f87baa.MenuCommands.ShowInfrequent = x789c645a15deb49b;
                this.xe8febcf8320c4113(x5f4b657f68f87baa.MenuCommands, x789c645a15deb49b);
            }
        }

        protected void x60cdc457bc0610f8()
        {
            if (!this.manualFocus)
            {
                this.manualFocus = true;
                this.animateFirst = true;
                Form form = base.FindForm();
                form.Deactivate += new EventHandler(this.x92abb6992cd92749);
                bool flag = x61467fe65a98f20c.HideCaret(IntPtr.Zero);
                x40255b11ef821fa3.MSG mSG = default(x40255b11ef821fa3.MSG);
                this.exitLoop = false;
                while (!this.exitLoop)
                {
                    if (x61467fe65a98f20c.WaitMessage())
                    {
                        while (!this.exitLoop && x61467fe65a98f20c.PeekMessage(ref mSG, 0, 0u, 0u, 0u))
                        {
                            if (x61467fe65a98f20c.GetMessage(ref mSG, IntPtr.Zero, 0u, 0u) && !this.x58b2784e55b43ddd(ref mSG))
                            {
                                x61467fe65a98f20c.TranslateMessage(ref mSG);
                                x61467fe65a98f20c.DispatchMessage(ref mSG);
                            }
                        }
                    }
                }
                form.Deactivate -= new EventHandler(this.x92abb6992cd92749);
                if (flag)
                {
                    x61467fe65a98f20c.ShowCaret(IntPtr.Zero);
                }
                this.manualFocus = false;
            }
        }

        protected void xa23d095e6e52c5f3()
        {
            if (this.manualFocus)
            {
                this.exitLoop = true;
            }
            if (this.trackItem != -1)
            {
                this.trackItem = this.xa7cbddf1ec7a27e7(this.trackItem, -1);
            }
        }

        protected void x92abb6992cd92749(object xe0292b9ed559da7d, EventArgs xfbf34718e704c6bc)
        {
            this.xa23d095e6e52c5f3();
        }

        protected void x58049cd54e5ca4e3(bool x651d82357ffa4398)
        {
            if (this.popupMenu == null)
            {
                int num = this.trackItem;
                int num2 = num;
                int i = 0;
                while (i < this.drawCommands.Count)
                {
                    num--;
                    if (num == num2)
                    {
                        return;
                    }
                    if (num < 0)
                    {
                        num = this.drawCommands.Count - 1;
                    }
                    x2cc390e9409b0f3f x2cc390e9409b0f3f = this.drawCommands[num] as x2cc390e9409b0f3f;
                    if (!x2cc390e9409b0f3f.Separator && (x2cc390e9409b0f3f.Chevron || x2cc390e9409b0f3f.MenuCommand.Enabled) && num != this.trackItem)
                    {
                        this.trackItem = this.xa7cbddf1ec7a27e7(this.trackItem, num);
                        if (this.selected && (x2cc390e9409b0f3f.Chevron || x2cc390e9409b0f3f.MenuCommand.MenuCommands.Count > 0))
                        {
                            x61467fe65a98f20c.PostMessage(base.Handle, this.WM_OPERATEMENU, 0u, 1u);
                            return;
                        }
                        break;
                    }
                    else
                    {
                        i++;
                    }
                }
            }
        }

        protected void x14014e142c6b152c(bool x651d82357ffa4398)
        {
            if (this.popupMenu == null)
            {
                int num = this.trackItem;
                int i = 0;
                while (i < this.drawCommands.Count)
                {
                    num++;
                    if (num >= this.drawCommands.Count)
                    {
                        num = 0;
                    }
                    x2cc390e9409b0f3f x2cc390e9409b0f3f = this.drawCommands[num] as x2cc390e9409b0f3f;
                    if (!x2cc390e9409b0f3f.Separator && (x2cc390e9409b0f3f.Chevron || x2cc390e9409b0f3f.MenuCommand.Enabled) && num != this.trackItem)
                    {
                        this.trackItem = this.xa7cbddf1ec7a27e7(this.trackItem, num);
                        if (this.selected && (x2cc390e9409b0f3f.Chevron || x2cc390e9409b0f3f.MenuCommand.MenuCommands.Count > 0))
                        {
                            x61467fe65a98f20c.PostMessage(base.Handle, this.WM_OPERATEMENU, 0u, 1u);
                            return;
                        }
                        break;
                    }
                    else
                    {
                        i++;
                    }
                }
            }
        }

        protected void x830edbe776f7cc9f()
        {
            if (this.popupMenu == null && this.trackItem != -1)
            {
                if (!this.selected)
                {
                    x2cc390e9409b0f3f x2cc390e9409b0f3f = this.drawCommands[this.trackItem] as x2cc390e9409b0f3f;
                    if (!x2cc390e9409b0f3f.Chevron && x2cc390e9409b0f3f.MenuCommand.MenuCommands.Count < 0)
                    {
                        x2cc390e9409b0f3f.MenuCommand.OnClick(EventArgs.Empty);
                        int x22f50445643d053e = this.trackItem;
                        this.x51ce8d251f940283();
                        this.x2cc390e9409b0f3f(x22f50445643d053e, false);
                        this.xa23d095e6e52c5f3();
                        return;
                    }
                    this.selected = true;
                    this.drawUpwards = false;
                    this.x2cc390e9409b0f3f(this.trackItem, true);
                    if (x2cc390e9409b0f3f.Chevron || x2cc390e9409b0f3f.MenuCommand.MenuCommands.Count > 0)
                    {
                        x61467fe65a98f20c.PostMessage(base.Handle, this.WM_OPERATEMENU, 0u, 1u);
                        return;
                    }
                }
                else
                {
                    x2cc390e9409b0f3f x2cc390e9409b0f3f2 = this.drawCommands[this.trackItem] as x2cc390e9409b0f3f;
                    x2cc390e9409b0f3f2.MenuCommand.OnClick(EventArgs.Empty);
                    int x22f50445643d053e2 = this.trackItem;
                    this.x51ce8d251f940283();
                    this.xcb97d93f0d5ce4a8();
                    this.x2cc390e9409b0f3f(x22f50445643d053e2, false);
                    this.xa23d095e6e52c5f3();
                }
            }
        }

        protected void x34de1c54143aa5c7()
        {
            if (this.popupMenu == null && this.trackItem != -1 && !this.selected)
            {
                x2cc390e9409b0f3f x2cc390e9409b0f3f = this.drawCommands[this.trackItem] as x2cc390e9409b0f3f;
                if (x2cc390e9409b0f3f.Chevron || x2cc390e9409b0f3f.MenuCommand.MenuCommands.Count >= 0)
                {
                    this.selected = true;
                    this.drawUpwards = false;
                    this.x2cc390e9409b0f3f(this.trackItem, true);
                    if (x2cc390e9409b0f3f.Chevron || x2cc390e9409b0f3f.MenuCommand.MenuCommands.Count > 0)
                    {
                        x61467fe65a98f20c.PostMessage(base.Handle, this.WM_OPERATEMENU, 0u, 1u);
                    }
                }
            }
        }

        protected bool x65c1cc54cc8b0e75(char xba08ce632055a1d9)
        {
            if (!this.selected)
            {
                int i = 0;
                while (i < this.drawCommands.Count)
                {
                    x2cc390e9409b0f3f x2cc390e9409b0f3f = this.drawCommands[i] as x2cc390e9409b0f3f;
                    if (x2cc390e9409b0f3f.MenuCommand != null && x2cc390e9409b0f3f.MenuCommand.Enabled && x2cc390e9409b0f3f.MenuCommand.Visible && xba08ce632055a1d9 == x2cc390e9409b0f3f.Mnemonic)
                    {
                        this.selected = true;
                        this.drawUpwards = false;
                        if (this.trackItem != i)
                        {
                            this.trackItem = this.xa7cbddf1ec7a27e7(this.trackItem, i);
                        }
                        else
                        {
                            this.x2cc390e9409b0f3f(this.trackItem, true);
                        }
                        if (x2cc390e9409b0f3f.Chevron || x2cc390e9409b0f3f.MenuCommand.MenuCommands.Count >= 0)
                        {
                            if (x2cc390e9409b0f3f.Chevron || x2cc390e9409b0f3f.MenuCommand.MenuCommands.Count > 0)
                            {
                                x61467fe65a98f20c.PostMessage(base.Handle, this.WM_OPERATEMENU, 0u, 1u);
                            }
                            return true;
                        }
                        x2cc390e9409b0f3f.MenuCommand.OnClick(EventArgs.Empty);
                        int x22f50445643d053e = this.trackItem;
                        this.x51ce8d251f940283();
                        this.xcb97d93f0d5ce4a8();
                        this.x2cc390e9409b0f3f(x22f50445643d053e, false);
                        this.xa23d095e6e52c5f3();
                        return false;
                    }
                    else
                    {
                        i++;
                    }
                }
            }
            return false;
        }

        protected void x288bea726e7cd27e(object xe0292b9ed559da7d, UserPreferenceChangedEventArgs xfbf34718e704c6bc)
        {
            if (this.defaultFont)
            {
                if (this.Engine.MenuFont == null)
                {
                    this.x21498b72da0020ba(SystemInformation.MenuFont);
                }
                else
                {
                    this.x21498b72da0020ba(this.Engine.MenuFont);
                }
                this.x39e008704a72ea56();
                base.Invalidate();
            }
        }

        protected override void OnSystemColorsChanged(EventArgs e)
        {
            this.x39e008704a72ea56();
            base.Invalidate();
            base.OnSystemColorsChanged(e);
        }

        public bool PreFilterMessage(ref Message msg)
        {
            Form form = base.FindForm();
            if (form != null && form == Form.ActiveForm && form.ContainsFocus)
            {
                uint msg2 = (uint)msg.Msg;
                if (msg2 != 256u)
                {
                    switch (msg2)
                    {
                        case 260u:
                            if (base.Enabled && (int)msg.WParam != 18)
                            {
                                Shortcut x10364107371ea04e = (Shortcut)262144 + (int)msg.WParam;
                                if (this.x4d7a264852c9d2f3(x10364107371ea04e, this.menuCommands))
                                {
                                    return true;
                                }
                                if (this.x65c1cc54cc8b0e75((char)((int)msg.WParam)))
                                {
                                    if (!this.manualFocus)
                                    {
                                        this.x60cdc457bc0610f8();
                                    }
                                    return true;
                                }
                            }
                            break;
                        case 261u:
                            if (base.Enabled && (int)msg.WParam == 18)
                            {
                                if (this.drawCommands.Count > 0)
                                {
                                    if (this.trackItem == -1)
                                    {
                                        for (int i = 0; i < this.drawCommands.Count; i++)
                                        {
                                            x2cc390e9409b0f3f x2cc390e9409b0f3f = this.drawCommands[i] as x2cc390e9409b0f3f;
                                            if (!x2cc390e9409b0f3f.Separator && (x2cc390e9409b0f3f.Chevron || x2cc390e9409b0f3f.MenuCommand.Enabled))
                                            {
                                                this.trackItem = this.xa7cbddf1ec7a27e7(-1, i);
                                                break;
                                            }
                                        }
                                    }
                                    this.x60cdc457bc0610f8();
                                }
                                return true;
                            }
                            break;
                    }
                }
                else if (base.Enabled)
                {
                    ushort keyState = x61467fe65a98f20c.GetKeyState(16);
                    ushort keyState2 = x61467fe65a98f20c.GetKeyState(17);
                    int num = (int)msg.WParam;
                    if ((keyState & 32768) != 0)
                    {
                        num += 65536;
                    }
                    if ((keyState2 & 32768) != 0)
                    {
                        num += 131072;
                    }
                    Shortcut x10364107371ea04e2 = (Shortcut)num;
                    return this.x4d7a264852c9d2f3(x10364107371ea04e2, this.menuCommands);
                }
            }
            return false;
        }

        protected bool x58b2784e55b43ddd(ref x40255b11ef821fa3.MSG x8a41fbc87a3fb305)
        {
            bool result = false;
            uint message = x8a41fbc87a3fb305.message;
            if (message <= 261u)
            {
                if (message <= 164u)
                {
                    if (message != 161u && message != 164u)
                    {
                        return result;
                    }
                }
                else if (message != 167u)
                {
                    switch (message)
                    {
                        case 256u:
                            {
                                ushort keyState = x61467fe65a98f20c.GetKeyState(16);
                                ushort keyState2 = x61467fe65a98f20c.GetKeyState(17);
                                int num = (int)x8a41fbc87a3fb305.wParam;
                                int num2 = num;
                                if ((keyState & 32768) != 0)
                                {
                                    num2 += 65536;
                                }
                                if ((keyState2 & 32768) != 0)
                                {
                                    num2 += 131072;
                                }
                                if (num2 == 27)
                                {
                                    if (this.trackItem != -1 && this.popupMenu == null)
                                    {
                                        this.trackItem = this.xa7cbddf1ec7a27e7(this.trackItem, -1);
                                    }
                                    this.xa23d095e6e52c5f3();
                                    result = true;
                                    return result;
                                }
                                if (num2 == 37)
                                {
                                    if (this.direction == xdbfa333b4cd503e0.Horizontal)
                                    {
                                        this.x58049cd54e5ca4e3(false);
                                    }
                                    if (this.selected)
                                    {
                                        this.ignoreMouseMove = true;
                                    }
                                    result = true;
                                    return result;
                                }
                                if (num2 == 39)
                                {
                                    if (this.direction == xdbfa333b4cd503e0.Horizontal)
                                    {
                                        this.x14014e142c6b152c(false);
                                    }
                                    else
                                    {
                                        this.x34de1c54143aa5c7();
                                    }
                                    if (this.selected)
                                    {
                                        this.ignoreMouseMove = true;
                                    }
                                    result = true;
                                    return result;
                                }
                                if (num2 == 13)
                                {
                                    this.x830edbe776f7cc9f();
                                    result = true;
                                    return result;
                                }
                                if (num2 == 40)
                                {
                                    if (this.direction == xdbfa333b4cd503e0.Horizontal)
                                    {
                                        this.x34de1c54143aa5c7();
                                    }
                                    else
                                    {
                                        this.x14014e142c6b152c(false);
                                    }
                                    result = true;
                                    return result;
                                }
                                if (num2 == 38)
                                {
                                    this.x58049cd54e5ca4e3(false);
                                    result = true;
                                    return result;
                                }
                                Shortcut x10364107371ea04e = (Shortcut)num2;
                                if (!this.x4d7a264852c9d2f3(x10364107371ea04e, this.menuCommands))
                                {
                                    this.x65c1cc54cc8b0e75((char)((int)x8a41fbc87a3fb305.wParam));
                                    if (this.selected)
                                    {
                                        this.ignoreMouseMove = true;
                                    }
                                }
                                else
                                {
                                    this.xa23d095e6e52c5f3();
                                }
                                result = true;
                                return result;
                            }
                        case 257u:
                            result = true;
                            return result;
                        case 258u:
                        case 259u:
                            return result;
                        case 260u:
                            if ((int)x8a41fbc87a3fb305.wParam != 18)
                            {
                                Shortcut x10364107371ea04e2 = (Shortcut)262144 + (int)x8a41fbc87a3fb305.wParam;
                                if (!this.x4d7a264852c9d2f3(x10364107371ea04e2, this.menuCommands))
                                {
                                    this.x65c1cc54cc8b0e75((char)((int)x8a41fbc87a3fb305.wParam));
                                    if (this.selected)
                                    {
                                        this.ignoreMouseMove = true;
                                    }
                                }
                                else
                                {
                                    this.xa23d095e6e52c5f3();
                                }
                                result = true;
                                return result;
                            }
                            return result;
                        case 261u:
                            if ((int)x8a41fbc87a3fb305.wParam == 18)
                            {
                                if (this.trackItem != -1 && this.popupMenu == null)
                                {
                                    this.trackItem = this.xa7cbddf1ec7a27e7(this.trackItem, -1);
                                }
                                this.xa23d095e6e52c5f3();
                                result = true;
                                return result;
                            }
                            return result;
                        default:
                            return result;
                    }
                }
            }
            else if (message <= 516u)
            {
                if (message != 513u && message != 516u)
                {
                    return result;
                }
            }
            else if (message != 519u && message != 523u)
            {
                return result;
            }
            Point pt = new Point((int)x8a41fbc87a3fb305.lParam & 65535, (int)((uint)((int)x8a41fbc87a3fb305.lParam & -65536) >> 16));
            if (!base.ClientRectangle.Contains(pt))
            {
                this.xa23d095e6e52c5f3();
            }
            return result;
        }

        protected bool x4d7a264852c9d2f3(Shortcut x10364107371ea04e, xd53b20b7b4b2a08a x20e3b25d6d144908)
        {
            foreach (x5f4b657f68f87baa x5f4b657f68f87baa in x20e3b25d6d144908)
            {
                if (x5f4b657f68f87baa.Enabled && x5f4b657f68f87baa.Shortcut == x10364107371ea04e)
                {
                    x5f4b657f68f87baa.OnClick(EventArgs.Empty);
                    bool result = true;
                    return result;
                }
                if (x5f4b657f68f87baa.MenuCommands.Count > 0 && this.x4d7a264852c9d2f3(x10364107371ea04e, x5f4b657f68f87baa.MenuCommands))
                {
                    bool result = true;
                    return result;
                }
            }
            return false;
        }

        protected void xa9f9739090793818(ref Message x6088325dec1baa2a)
        {
            if (this.trackItem != -1)
            {
                x2cc390e9409b0f3f xd2a8bb4342ab4ef = this.drawCommands[this.trackItem] as x2cc390e9409b0f3f;
                this.x0760506d6abf77a2(xd2a8bb4342ab4ef, x6088325dec1baa2a.LParam != IntPtr.Zero, x6088325dec1baa2a.WParam != IntPtr.Zero);
            }
        }

        protected void x92bcd6bad6f29e72(ref Message x6088325dec1baa2a)
        {
            x6088325dec1baa2a.Result = (IntPtr)4L;
        }

        protected override void WndProc(ref Message m)
        {
            if ((long)m.Msg == (long)((ulong)this.WM_OPERATEMENU))
            {
                this.xa9f9739090793818(ref m);
            }
            else
            {
                uint msg = (uint)m.Msg;
                if (msg == 135u)
                {
                    this.x92bcd6bad6f29e72(ref m);
                    return;
                }
            }
            base.WndProc(ref m);
        }

        private void x273cbe3cfa28ea94(object xe0292b9ed559da7d, SkinChangedEventArgs xfbf34718e704c6bc)
        {
            if (this.Engine.RealActive)
            {
                if (this.minButton != null)
                {
                    this.minButton.BackColor = this.Engine.Res.Colors.SKIN2_MENUBARSTARTCOLOR;
                }
                if (this.restoreButton != null)
                {
                    this.restoreButton.BackColor = this.Engine.Res.Colors.SKIN2_MENUBARSTARTCOLOR;
                }
                if (this.closeButton != null)
                {
                    this.closeButton.BackColor = this.Engine.Res.Colors.SKIN2_MENUBARSTARTCOLOR;
                }
            }
        }
    }
}
