namespace Sunisoft.IrisSkin
{
    using Sunisoft.IrisSkin.Design;
    using Sunisoft.IrisSkin.InternalControls;
    using System;
    using System.Collections;
    using System.ComponentModel;
    using System.ComponentModel.Design.Serialization;
    using System.Drawing;
    using System.Drawing.Design;
    using System.IO;
    using System.Runtime.CompilerServices;
    using System.Security.Cryptography;
    using System.Text;
    using System.Windows.Forms;
    using x293b01486f981425;

    [Serializable, ToolboxBitmap(typeof(SkinEngine), "SkinEngineIcon.bmp"), ToolboxItem(true), DesignerSerializer(typeof(x2f6133cd59233dfd), typeof(CodeDomSerializer))]
    public class SkinEngine : Component
    {
        private string __build;
        private bool active;
        private SkinCollection addtionalBuiltInSkins;
        private IntPtr backColorBrush;
        private Hashtable bitmapList;
        private int bottomBorderHeight;
        private Hashtable brushList;
        private bool builtIn;
        internal static x6161963e817c3cff CbtProc;
        private IntPtr controlBorderBrush;
        private static Hashtable dialogList = new Hashtable();
        private Hashtable disabledForm;
        private int disableTag;
        private bool drawFormCaption;
        private bool drawFormIcon;
        internal static DSACryptoServiceProvider DSA = new DSACryptoServiceProvider();
        internal static byte[] DSAHash;
        private bool enable3rdControl;
        internal static SkinEngine Engine;
        private int formCaptionPosX;
        private int formCaptionPosY;
        private int formMinWidth;
        internal static IntPtr Hook = IntPtr.Zero;
        private int leftBorderWidth;
        private Font menuFont;
        private int minTextWidth;
        private Component owner;
        private static System.Random ram = new System.Random();
        private static int random = 0;
        private x7f0ebae1a2d30adf reader;
        private bool realActive;
        private Sunisoft.IrisSkin.Res res;
        private string resSysMenuClose;
        private string resSysMenuMax;
        private string resSysMenuMin;
        private int rightBorderWidth;
        private string serialNumber;
        private bool skinAllForm;
        private bool skinDialogs;
        private string skinFile;
        internal static ArrayList SkinHandleList = new ArrayList();
        private string skinPassword;
        private bool skinScrollBar;
        private Stream skinStream;
        private Stream skinStreamMain;
        private Font titleFont;
        private int titleHeight;
        private static Hashtable windowList = new Hashtable();

        public event SkinChanged CurrentSkinChanged;

        static SkinEngine()
        {
            DSA.FromXmlString(string.Intern(x1110bdd110cdcea4._d574bb1a8f3e9cbc("nhjeciafoihfjhofaifghjmgikdhcikhkibicjiiijpifigjlfnjgfekhglkcfclhijlfeammghmpfomnhfngdmnpcdoockobgbpofipifppddgaecnabeebjdlbafccoejckcadnbhdeeodhbfehbmeopcfabkffbbgjphggapgjaghfpmhmbeigokikacjobjjibakkngkmaokioelhollapcmoojmgoanhphncapnbmfoipmoaodpjnkpbpbaimiakmpakkgbonnbmmecknlcjlcddljdlnaeenheeloeflffemmfoidggkkgjkbhaiihblphejgiljnimiejjjljahckhgjknhaljihlikolpffmgjmmdhdnpiknhhbomgioagpofhgpognpjeeadglamecbjejbfeacfdhcdfocodfdjdmdledefdkefgbfdeififpfocggoengafehjdlhoaciaejieeajmahjaeojmbfkbamkdddlmbklhbbmfbimacpmapfnmbnnpaeogalokpbpecjpmnppgngaaonamnebmmlbloccfnjcanadinhdmmodmofeaomemndfgnkfkkbgpnigikpgolghjnnhfneiglliokcjkkjjelaklnhkhmokhmflbnmlkmdmbmkmmjbnekinnkpnhhgohlnoiiepdjlpngcafkjafgabgkhbhjobngfcjimcifddhfkddibeagielgpeahgfeinfkgegpdlgpdchghjhndaibfhilgoiiefjmgmjgfdkhfkkhdblfdillcplaegmecnmhcennclndecocejojaapaahppdopddfamamamadbickbkoacpcicaapcdcgdkomdgodeopkelobfooifhbaggnggoongfbfhfamhcocifmjidpajnmhjgmojdmfkplmkpkdlemklilbmdlimnmpmpkgneonnmmeogllofjcpknjpnmaaoihakmoabmfbljmbgldcllkcambdhjidehpdbhgeogneeiefdklfokcgojjgnjahfghhkfohpjfijjmilidjdhkjmfbkkeikfgpkgfgljfnlodemcilmigcniejnagaocehoofooiefpbemphgdaedkakfbbaeibjcpbmegcebncpdedialdkacecejejeafbehffbofhcfgkdmgcbdhgakhbbbigciidcpigpfjpanjhbekonkkonblgbjlompldpgmjaomoafncamnhncolnjodnapknhpgpopeofadomacndbknkbnmbcekicgopcjkgdgkndckeecjlejlcfljjfgjagbkhgcjogcjfhpimhmidijikigibjfjijiipjfhgkcjnkkheljjllgicmohjmlganighnkfondjfongmoajdpigkpphbamgiapfpagfgbbinbkdeckglcdfcdgfjdadaejgheceoekcfflfmfegdgnfkgndbhdeihnephocgilbniiaejhdljfcckddjkbealcbhlnbolpafmocmmicdnjcknhcbohbioacpoabgplanpmodahokajnbbloibgaacfogclpncnmednlldfncegmjebmaffnhfeoofbofgnnmgeldhnmkhhkbicmiianpipmgjnmnjcnekdklknlclkkjlhjamkjhmnlomaifncmmnhldohhkohibpgjipekppmkgaminaagebgilbihccogjcogadpfhdlfodlefemgmelhdfihkfehbgleiggepghfghlgnhggeimgliodcjhgjjkgakaghkdgokbffllfmlecdmlckmhfbnkbinhbpndbgodanobcepfdlpadcagdjaiaabbdhbedobkcfcncmclbddfckdooaejohejnoeloffhpmfcodgjokgaachbbjhlophdpgilpnibafjooljemck", 0x54204941)));
            DSAHash = Encoding.ASCII.GetBytes("IrisSkin is good !!!");
        }

        public SkinEngine()
        {
            this.active = true;
            this.builtIn = true;
            this.disableTag = 0x270f;
            this.skinAllForm = true;
            this.skinDialogs = true;
            this.skinScrollBar = true;
            this.formCaptionPosX = -1;
            this.formCaptionPosY = -1;
            this.serialNumber = "";
            this.resSysMenuClose = "关闭(&C)";
            this.resSysMenuMax = "最大化/还原(&X)";
            this.resSysMenuMin = "最小化(&N)";
            this.drawFormIcon = true;
            this.drawFormCaption = true;
            this.__build = "2006.10.19";
            this.disabledForm = new Hashtable();
            this.reader = new x7f0ebae1a2d30adf();
            this.backColorBrush = IntPtr.Zero;
            this.bitmapList = new Hashtable();
            this.brushList = new Hashtable();
            this.InternalConstructor();
        }

        public SkinEngine(Component owner)
        {
            this.active = true;
            this.builtIn = true;
            this.disableTag = 0x270f;
            this.skinAllForm = true;
            this.skinDialogs = true;
            this.skinScrollBar = true;
            this.formCaptionPosX = -1;
            this.formCaptionPosY = -1;
            this.serialNumber = "";
            this.resSysMenuClose = "关闭(&C)";
            this.resSysMenuMax = "最大化/还原(&X)";
            this.resSysMenuMin = "最小化(&N)";
            this.drawFormIcon = true;
            this.drawFormCaption = true;
            this.__build = "2006.10.19";
            this.disabledForm = new Hashtable();
            this.reader = new x7f0ebae1a2d30adf();
            this.backColorBrush = IntPtr.Zero;
            this.bitmapList = new Hashtable();
            this.brushList = new Hashtable();
            this.owner = owner;
            this.InternalConstructor();
        }

        public SkinEngine(Component owner, Stream skinStream)
        {
            this.active = true;
            this.builtIn = true;
            this.disableTag = 0x270f;
            this.skinAllForm = true;
            this.skinDialogs = true;
            this.skinScrollBar = true;
            this.formCaptionPosX = -1;
            this.formCaptionPosY = -1;
            this.serialNumber = "";
            this.resSysMenuClose = "关闭(&C)";
            this.resSysMenuMax = "最大化/还原(&X)";
            this.resSysMenuMin = "最小化(&N)";
            this.drawFormIcon = true;
            this.drawFormCaption = true;
            this.__build = "2006.10.19";
            this.disabledForm = new Hashtable();
            this.reader = new x7f0ebae1a2d30adf();
            this.backColorBrush = IntPtr.Zero;
            this.bitmapList = new Hashtable();
            this.brushList = new Hashtable();
            this.owner = owner;
            this.InternalConstructor();
            this.SkinStream = skinStream;
        }

        public SkinEngine(Component owner, string skinFile)
        {
            this.active = true;
            this.builtIn = true;
            this.disableTag = 0x270f;
            this.skinAllForm = true;
            this.skinDialogs = true;
            this.skinScrollBar = true;
            this.formCaptionPosX = -1;
            this.formCaptionPosY = -1;
            this.serialNumber = "";
            this.resSysMenuClose = "关闭(&C)";
            this.resSysMenuMax = "最大化/还原(&X)";
            this.resSysMenuMin = "最小化(&N)";
            this.drawFormIcon = true;
            this.drawFormCaption = true;
            this.__build = "2006.10.19";
            this.disabledForm = new Hashtable();
            this.reader = new x7f0ebae1a2d30adf();
            this.backColorBrush = IntPtr.Zero;
            this.bitmapList = new Hashtable();
            this.brushList = new Hashtable();
            this.owner = owner;
            this.InternalConstructor();
            this.SkinFile = skinFile;
        }

        public void AddContextMenu(ContextMenu menu)
        {
            if (menu != null)
            {
                new xa1883d0b59b7005b(this, menu);
            }
        }

        public void AddContextMenuStrip(ContextMenuStrip menu)
        {
            if (menu != null)
            {
                new x095234c5c1abb370(menu, Engine);
            }
        }

        public void AddControl(Control control)
        {
            if (control != null)
            {
                xf3f6919ac5d158dc.Create(control, Engine);
            }
        }

        public void AddForm(Form form)
        {
            if ((form != null) && !form.IsDisposed)
            {
                if (form.Created)
                {
                    SkinHandleList.Add(form.Handle);
                    this.DoAddWnd(form.Handle, true);
                }
                else
                {
                    form.HandleCreated += new EventHandler(this.SkinFormHandleCreated);
                }
            }
        }

        public void ApplyAdditionalBuiltInSkins(int count)
        {
            if (this.AddtionalBuiltInSkins.Count == 0)
            {
                throw new ApplicationException("there is no addtional built in skin");
            }
            if ((count < 0) || (count > this.AddtionalBuiltInSkins.Count))
            {
                throw new ArgumentOutOfRangeException("count", string.Format("count must equal to 0 or greater than 0 and less than {0}", this.AddtionalBuiltInSkins.Count));
            }
            this.reader.SkinPassword = this.AddtionalBuiltInSkins[count].SkinPassword;
            this.SkinStream = this.AddtionalBuiltInSkins[count].SkinSteam;
        }

        public void ApplyMainBuiltInSkin()
        {
            this.reader.SkinPassword = this.skinPassword;
            this.SkinStream = this.SkinStreamMain;
        }

        protected override void Dispose(bool disposing)
        {
            if (Hook != IntPtr.Zero)
            {
                x61467fe65a98f20c.UnhookWindowsHookEx(Hook);
                Hook = IntPtr.Zero;
            }
            base.Dispose(disposing);
        }

        private void DoAddDlg(IntPtr handle, bool isManul)
        {
            if (!dialogList.ContainsKey(handle))
            {
                dialogList.Add(handle, null);
                xb052c904ac95dc43 xbcacdc = new xb052c904ac95dc43(handle, this);
                dialogList[handle] = xbcacdc;
            }
        }

        private void DoAddWnd(IntPtr handle, bool isManual)
        {
            Control control = Control.FromHandle(handle);
            if (((((control is Form) && !windowList.ContainsKey(handle)) && (control.CompanyName != "9Rays.Net")) && (control.GetType().FullName != "XPTable.Editors.DropDownContainer")) && !(control is x4fef14ebf3863c7f))
            {
                windowList.Add(handle, null);
                xa427f1b2281f554b xafbfb = new xa427f1b2281f554b(handle, this);
                windowList[handle] = xafbfb;
            }
        }

        private static unsafe IntPtr FnHookProc(int nCode, IntPtr wParam, IntPtr lParam)
        {
            if (Engine != null)
            {
                switch (nCode)
                {
                    case 3:
                    {
                        StringBuilder lpClassName = new StringBuilder(260);
                        x61467fe65a98f20c.GetClassName(wParam, lpClassName, 260);
                        if ((lpClassName.Length != 9) || (lpClassName.ToString().ToUpper() != "SCROLLBAR"))
                        {
                            break;
                        }
                        x40255b11ef821fa3.CBT_CREATEWND* cbt_createwndPtr = (x40255b11ef821fa3.CBT_CREATEWND*) lParam;
                        x40255b11ef821fa3.CREATESTRUCT* lpcs = (x40255b11ef821fa3.CREATESTRUCT*) cbt_createwndPtr->lpcs;
                        if ((((lpcs->style & 0x10L) != 0x10L) || !Engine.RealActive) || (lpcs->style > 0x56020014L))
                        {
                            break;
                        }
                        return new IntPtr(1);
                    }
                    case 5:
                    {
                        Control control = Control.FromHandle(wParam);
                        if (control == null)
                        {
                            if (Engine.SkinDialogs)
                            {
                                StringBuilder builder = new StringBuilder(260);
                                x61467fe65a98f20c.GetClassName(wParam, builder, 260);
                                if ((builder.Length == 6) && (builder.ToString() == "#32770"))
                                {
                                    SkinHandleList.Add(wParam);
                                    Engine.DoAddDlg(wParam, true);
                                }
                            }
                            break;
                        }
                        if (!Engine.SkinAllForm)
                        {
                            if (((Engine.Owner is Form) && (control.GetType() == Engine.Owner.GetType())) && !SkinHandleList.Contains(wParam))
                            {
                                SkinHandleList.Add(wParam);
                                Engine.DoAddWnd(wParam, false);
                            }
                            break;
                        }
                        if (!SkinHandleList.Contains(wParam) && (control is Form))
                        {
                            SkinHandleList.Add(wParam);
                            Engine.DoAddWnd(wParam, false);
                        }
                        break;
                    }
                }
            }
            return x61467fe65a98f20c.CallNextHookEx(Hook, nCode, wParam, lParam);
        }

        public Bitmap GetBitmap(string key)
        {
            if (this.bitmapList.ContainsKey(key))
            {
                return (Bitmap) this.bitmapList[key];
            }
            Bitmap bitmap = this.reader.GetBitmap(key);
            if (bitmap != null)
            {
                bitmap.MakeTransparent(this.GetColor("SKIN2_TRANSCOLOR"));
            }
            this.bitmapList.Add(key, bitmap);
            return bitmap;
        }

        public Bitmap GetBitmap(string key, int spitCount, int spitIndex)
        {
            string str = string.Format("{0}{1}OF{2}", key, spitIndex, spitCount);
            if (this.bitmapList.ContainsKey(str))
            {
                return (Bitmap) this.bitmapList[str];
            }
            Bitmap bitmap = this.reader.GetBitmap(key, spitCount, spitIndex);
            if (bitmap != null)
            {
                bitmap.MakeTransparent(this.GetColor("SKIN2_TRANSCOLOR"));
            }
            this.bitmapList.Add(str, bitmap);
            return bitmap;
        }

        public bool GetBool(string key)
        {
            return this.reader.GetBool(key);
        }

        public Brush GetBrush(string key)
        {
            if (this.brushList.ContainsKey(key))
            {
                return (Brush) this.brushList[key];
            }
            Brush brush = new SolidBrush(this.reader.GetColor(key));
            this.brushList.Add(key, brush);
            return brush;
        }

        public Color GetColor(string key)
        {
            return this.reader.GetColor(key);
        }

        public int GetInt(string key)
        {
            return this.reader.GetInt(key);
        }

        private void InternalConstructor()
        {
            this.addtionalBuiltInSkins = new SkinCollection();
            if (!IsDesignModel)
            {
                if (Engine == null)
                {
                    Engine = this;
                    if (Hook == IntPtr.Zero)
                    {
                        CbtProc = new x6161963e817c3cff(SkinEngine.FnHookProc);
                        Hook = x61467fe65a98f20c.SetWindowsHookEx(5, CbtProc, 0, AppDomain.GetCurrentThreadId());
                        Application.ApplicationExit += new EventHandler(SkinEngine.OnApplicationExit);
                    }
                }
                else if (this.owner is Form)
                {
                    Form owner = (Form) this.owner;
                    if (owner.Created)
                    {
                        SkinHandleList.Add(owner.Handle);
                        this.DoAddWnd(owner.Handle, true);
                    }
                    else
                    {
                        owner.HandleCreated += new EventHandler(this.SkinFormHandleCreated);
                    }
                }
            }
        }

        protected virtual void OnActiveChanged()
        {
            if (!IsDesignModel)
            {
                if (this.reader != null)
                {
                    this.realActive = this.reader.Ready & this.active;
                }
                else
                {
                    this.realActive = false;
                }
                this.OnCurrentSkinChanged();
            }
        }

        private static void OnApplicationExit(object sender, EventArgs e)
        {
            Engine.Dispose(false);
        }

        protected virtual void OnBuiltInChanged(bool builtIn)
        {
            if (builtIn)
            {
                this.SetBuiltInSkinFile(this.SkinFile);
                this.builtIn = true;
            }
            else
            {
                this.builtIn = false;
                if (IsDesignModel)
                {
                    this.SkinStreamMain = null;
                }
            }
        }

        protected virtual void OnCurrentSkinChanged()
        {
            this.bitmapList = new Hashtable();
            this.brushList = new Hashtable();
            if (this.RealActive)
            {
                this.PrepareResources();
                Bitmap image = this.Res.Bitmaps.SKIN2_TITLEBAR1;
                this.titleHeight = image.Height;
                image = this.Res.Bitmaps.SKIN2_FORMLEFTBORDER;
                this.leftBorderWidth = image.Width;
                image = this.Res.Bitmaps.SKIN2_FORMRIGHTBORDER;
                this.rightBorderWidth = image.Width;
                image = this.Res.Bitmaps.SKIN2_FORMBOTTOMBORDER1;
                this.bottomBorderHeight = image.Height;
                this.formMinWidth = 0;
                this.formMinWidth += this.leftBorderWidth + this.rightBorderWidth;
                if (this.Res.Bools.SKIN2_TITLEFIVESECT)
                {
                    Font captionFont;
                    image = this.Res.Bitmaps.SKIN2_TITLEBAR1;
                    this.formMinWidth += image.Width;
                    image = this.Res.Bitmaps.SKIN2_TITLEBAR3;
                    this.formMinWidth += image.Width;
                    if (this.TitleFont == null)
                    {
                        captionFont = x448fd9ab43628c71.GetCaptionFont();
                    }
                    else
                    {
                        captionFont = this.TitleFont;
                    }
                    using (Graphics graphics = Graphics.FromImage(image))
                    {
                        this.minTextWidth = graphics.MeasureString("W...", captionFont).ToSize().Width;
                    }
                    this.formMinWidth += (this.minTextWidth + this.Res.Integers.SKIN2_TITLEBARICONPOSX) + 40;
                    this.formMinWidth = Math.Max(this.formMinWidth, 100);
                }
                else
                {
                    image = this.Res.Bitmaps.SKIN2_TITLEBAR1;
                    this.formMinWidth = image.Width;
                    image = this.Res.Bitmaps.SKIN2_TITLEBAR3;
                    this.formMinWidth += image.Width;
                    this.formMinWidth = Math.Max(this.formMinWidth, 100);
                }
            }
            if (this.CurrentSkinChanged != null)
            {
                this.CurrentSkinChanged(this, new SkinChangedEventArgs(this.RealActive));
            }
        }

        protected virtual void OnMainSkinStreamChanged(Stream stream)
        {
            if (stream == null)
            {
                this.realActive = false;
            }
            else
            {
                this.reader.SkinStream = stream;
                this.realActive = this.reader.Ready & this.active;
            }
            this.skinStreamMain = stream;
            this.OnCurrentSkinChanged();
        }

        protected virtual void OnSkinAllFormChanged()
        {
        }

        protected virtual void OnSkinFileChanged(string fileName)
        {
            if (this.BuiltIn && IsDesignModel)
            {
                this.SetBuiltInSkinFile(fileName);
            }
            else
            {
                this.SetSkinFile(fileName);
            }
        }

        protected virtual void OnSkinStreamChanged(Stream skinStream)
        {
            if (skinStream == null)
            {
                this.realActive = false;
            }
            else
            {
                this.reader.SkinStream = skinStream;
                this.realActive = this.reader.Ready & this.active;
            }
            this.OnCurrentSkinChanged();
        }

        private void PrepareResources()
        {
            if (this.res != null)
            {
                this.res.Dispose();
            }
            this.res = null;
            this.res = new Sunisoft.IrisSkin.Res(this);
            if (this.backColorBrush != IntPtr.Zero)
            {
                x31775329b2a4ff52.DeleteObject(this.backColorBrush);
            }
            x1439a41cfa24189f.LOGBRUSH brush = new x1439a41cfa24189f.LOGBRUSH();
            uint @int = (uint) this.GetInt("SKIN2_FORMCOLOR");
            if ((@int & 0xff000000) == 0xff000000)
            {
                @int = (uint) x61467fe65a98f20c.GetSysColor(((int) @int) & 0xff);
            }
            brush.lbColor = @int;
            brush.lbStyle = 0;
            this.backColorBrush = x31775329b2a4ff52.CreateBrushIndirect(ref brush);
            if (this.controlBorderBrush != IntPtr.Zero)
            {
                x31775329b2a4ff52.DeleteObject(this.controlBorderBrush);
            }
            brush = new x1439a41cfa24189f.LOGBRUSH();
            @int = (uint) this.GetInt("SKIN2_CONTROLBORDERCOLOR");
            if ((@int & 0xff000000) == 0xff000000)
            {
                @int = (uint) x61467fe65a98f20c.GetSysColor(((int) @int) & 0xff);
            }
            brush.lbColor = @int;
            brush.lbStyle = 0;
            this.controlBorderBrush = x31775329b2a4ff52.CreateBrushIndirect(ref brush);
        }

        public void RemoveForm(Form form, bool includeControls)
        {
            if ((form != null) && !form.IsDisposed)
            {
                IntPtr handle = form.Handle;
                if (windowList.Contains(handle) && (windowList[handle] is xd24df615efe9450e))
                {
                    ((xd24df615efe9450e) windowList[handle]).x52b190e626f65140(includeControls);
                    windowList.Remove(handle);
                }
            }
        }

        public void ResetMainMenu(Form form)
        {
            if ((form != null) && !form.IsDisposed)
            {
                x61467fe65a98f20c.SendMessage(form.Handle, 0x20d3, (uint) 0x20d3, (uint) 0);
            }
        }

        public void SetBrush(string key, Brush b)
        {
            this.brushList.Add(key, b);
        }

        private void SetBuiltInSkinFile(string file)
        {
            if ((file == null) || (file == ""))
            {
                this.skinStreamMain = null;
                this.realActive = false;
                this.skinFile = file;
            }
            else
            {
                try
                {
                    this.Reader.SkinFile = file;
                    FileStream input = File.Open(file, FileMode.Open, FileAccess.Read, FileShare.Read);
                    BinaryReader reader = new BinaryReader(input);
                    this.skinStreamMain = new MemoryStream(reader.ReadBytes((int) input.Length));
                    reader.Close();
                    this.skinFile = file;
                    this.realActive = this.Reader.Ready & this.active;
                }
                catch
                {
                }
            }
        }

        private void SetSkinFile(string file)
        {
            try
            {
                this.Reader.SkinFile = file;
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.Message);
            }
            this.skinFile = file;
            this.realActive = this.Reader.Ready & this.active;
            this.OnCurrentSkinChanged();
        }

        private void SkinFormHandleCreated(object sender, EventArgs e)
        {
            if (sender is Form)
            {
                Form form = (Form) sender;
                SkinHandleList.Add(form.Handle);
                this.DoAddWnd(form.Handle, true);
            }
        }

        [Browsable(false), DefaultValue("2006.10.19")]
        public string __Build
        {
            get
            {
                return this.__build;
            }
        }

        [Category("Behavior"), DefaultValue(true), Description("If the skin engine skin all .NET forms")]
        public bool Active
        {
            get
            {
                return this.active;
            }
            set
            {
                this.active = value;
                this.OnActiveChanged();
            }
        }

        [Category("Skins"), Description("Build more skins into the EXE file"), DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public SkinCollection AddtionalBuiltInSkins
        {
            get
            {
                return this.addtionalBuiltInSkins;
            }
        }

        internal IntPtr BackColorBrush
        {
            get
            {
                return this.backColorBrush;
            }
        }

        internal int BottomBorderHeight
        {
            get
            {
                return this.bottomBorderHeight;
            }
        }

        [DefaultValue(true), Description("Specifies whether build the skin file into the EXE file."), Category("Skins")]
        public bool BuiltIn
        {
            get
            {
                return this.builtIn;
            }
            set
            {
                if (this.builtIn != value)
                {
                    this.OnBuiltInChanged(value);
                }
            }
        }

        internal IntPtr ControlBorderBrush
        {
            get
            {
                return this.controlBorderBrush;
            }
        }

        internal Hashtable DisabledForm
        {
            get
            {
                return this.disabledForm;
            }
        }

        [DefaultValue(0x270f), Category("Behavior"), Description("If you do not want skin the control, Set Control's Tag property to this")]
        public int DisableTag
        {
            get
            {
                return this.disableTag;
            }
            set
            {
                this.disableTag = value;
            }
        }

        [Browsable(false), DefaultValue(true)]
        public bool DrawFormCaption
        {
            get
            {
                return this.drawFormCaption;
            }
            set
            {
                this.drawFormCaption = value;
            }
        }

        [DefaultValue(true), Browsable(false)]
        public bool DrawFormIcon
        {
            get
            {
                return this.drawFormIcon;
            }
            set
            {
                this.drawFormIcon = value;
            }
        }

        [Category("Behavior"), Description("Enable/Disable skin 3rd controls"), DefaultValue(false)]
        public bool Enable3rdControl
        {
            get
            {
                return this.enable3rdControl;
            }
            set
            {
                this.enable3rdControl = value;
            }
        }

        [Description("Force to specifies the position of the caption on title bar. In general, the position is an auto-position defined in a skin"), Category("Behavior"), DefaultValue(-1)]
        public int FormCaptionPosX
        {
            get
            {
                return this.formCaptionPosX;
            }
            set
            {
                this.formCaptionPosX = value;
            }
        }

        [Category("Behavior"), DefaultValue(-1), Description("Force to specifies the position of the caption on title bar. In general, the position is an auto-position defined in a skin")]
        public int FormCaptionPosY
        {
            get
            {
                return this.formCaptionPosY;
            }
            set
            {
                this.formCaptionPosY = value;
            }
        }

        internal int FormMinWidth
        {
            get
            {
                return this.formMinWidth;
            }
        }

        private static bool IsDesignModel
        {
            get
            {
                string fileName = Path.GetFileName(Application.ExecutablePath);
                if ((fileName == null) || (fileName.Length != 10))
                {
                    return false;
                }
                if (fileName.ToUpper() != "DEVENV.EXE")
                {
                    return false;
                }
                return true;
            }
        }

        internal int LeftBorderWidth
        {
            get
            {
                return this.leftBorderWidth;
            }
        }

        [Category("Font"), Description("Specifies the font of the main menu on the skinned forms"), DefaultValue((string) null)]
        public Font MenuFont
        {
            get
            {
                return this.menuFont;
            }
            set
            {
                this.menuFont = value;
            }
        }

        internal int MinTextWidth
        {
            get
            {
                return this.minTextWidth;
            }
        }

        [Browsable(false)]
        public Component Owner
        {
            get
            {
                return this.owner;
            }
        }

        internal static int Random
        {
            get
            {
                if (SkinEngine.random == 0)
                {
                    SkinEngine.random = 0xc350;
                }
                else
                {
                    SkinEngine.random = ram.Next(0xc351);
                }
                int random = SkinEngine.random;
                SkinEngine.random++;
                return random;
            }
        }

        internal x7f0ebae1a2d30adf Reader
        {
            get
            {
                return this.reader;
            }
        }

        internal bool RealActive
        {
            get
            {
                return this.realActive;
            }
        }

        [Browsable(false)]
        public Sunisoft.IrisSkin.Res Res
        {
            get
            {
                return this.res;
            }
        }

        [Category("Misc"), DefaultValue("关闭(&C)"), Description("Specifies the text for the close menu item in the system menu")]
        public string ResSysMenuClose
        {
            get
            {
                return this.resSysMenuClose;
            }
            set
            {
                this.resSysMenuClose = value;
            }
        }

        [Category("Misc"), DefaultValue("最大化/还原(&X)"), Description("Specifies the text for the maximize/restore menu item in the system menu")]
        public string ResSysMenuMax
        {
            get
            {
                return this.resSysMenuMax;
            }
            set
            {
                this.resSysMenuMax = value;
            }
        }

        [Category("Misc"), DefaultValue("最小化(&N)"), Description("Specifies the text for the minimize menu item in the system menu")]
        public string ResSysMenuMin
        {
            get
            {
                return this.resSysMenuMin;
            }
            set
            {
                this.resSysMenuMin = value;
            }
        }

        internal int RightBorderWidth
        {
            get
            {
                return this.rightBorderWidth;
            }
        }

        [DefaultValue(" "), Description("Input the serial number hear to built the skin without registry infomation"), Category("Misc")]
        public string SerialNumber
        {
            get
            {
                return this.serialNumber;
            }
            set
            {
                this.serialNumber = value;
            }
        }

        [Category("Behavior"), Description("If the skin engine skin all .NET forms"), DefaultValue(true)]
        public bool SkinAllForm
        {
            get
            {
                return this.skinAllForm;
            }
            set
            {
                this.skinAllForm = value;
                this.OnSkinAllFormChanged();
            }
        }

        [DefaultValue(true), Description("If the skin engine skin the dialogs"), Category("Behavior")]
        public bool SkinDialogs
        {
            get
            {
                return this.skinDialogs;
            }
            set
            {
                this.skinDialogs = value;
            }
        }

        [Editor(typeof(x1cc8dd3ebd3495cd), typeof(UITypeEditor)), Description("Specifies the skin file(.sui file). "), Category("Skins")]
        public string SkinFile
        {
            get
            {
                return this.skinFile;
            }
            set
            {
                this.OnSkinFileChanged(value);
            }
        }

        [DefaultValue((string) null), Description("Specifies the password of the skinFile"), Category("Skins")]
        public string SkinPassword
        {
            get
            {
                return this.skinPassword;
            }
            set
            {
                this.reader.SkinPassword = value;
                this.skinPassword = value;
            }
        }

        [Description("If the skin engine skin the Scroll bar of a control"), Category("Behavior"), DefaultValue(true)]
        public bool SkinScrollBar
        {
            get
            {
                return this.skinScrollBar;
            }
            set
            {
                this.skinScrollBar = value;
            }
        }

        [Browsable(false), DefaultValue((string) null)]
        public Stream SkinStream
        {
            get
            {
                return this.skinStream;
            }
            set
            {
                this.OnSkinStreamChanged(value);
            }
        }

        [DefaultValue((string) null), Browsable(false)]
        public Stream SkinStreamMain
        {
            get
            {
                return this.skinStreamMain;
            }
            set
            {
                this.OnMainSkinStreamChanged(value);
            }
        }

        [Category("Font"), DefaultValue((string) null), Description("Specifies the title font of the skinned forms")]
        public Font TitleFont
        {
            get
            {
                return this.titleFont;
            }
            set
            {
                this.titleFont = value;
            }
        }

        internal int TitleHeight
        {
            get
            {
                return this.titleHeight;
            }
        }

        [Description("Current version of IrisSkin"), Category("Version")]
        public string Version
        {
            get
            {
                return "3.2";
            }
        }
    }
}

