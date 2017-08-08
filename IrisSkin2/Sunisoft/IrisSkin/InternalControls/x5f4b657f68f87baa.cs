using System;
using System.ComponentModel;
using System.Drawing;
using System.Runtime.CompilerServices;
using System.Windows.Forms;

namespace Sunisoft.IrisSkin.InternalControls
{
    [ToolboxItem(false)]
    internal class x5f4b657f68f87baa : Component
    {
        internal delegate void x5282b9c42ea1abce(x5f4b657f68f87baa item, x5f4b657f68f87baa.x879a106b0501b9dc prop);

        internal enum x879a106b0501b9dc
        {
            Text,
            Enabled,
            ImageIndex,
            ImageList,
            Image,
            Shortcut,
            Checked,
            RadioCheck,
            Break,
            Infrequent,
            Visible,
            Description
        }

        protected bool visible;

        protected bool ibreak;

        protected string text;

        protected string description;

        protected bool enabled;

        protected bool ichecked;

        protected int imageIndex;

        protected bool infrequent;

        protected object tag;

        protected bool radioCheck;

        protected Shortcut shortcut;

        protected ImageList imageList;

        protected Image image;

        protected xd53b20b7b4b2a08a menuItems;

        private EventHandler x98992f4120a73bb9;

        private EventHandler x295cb4a1df7a5add;

        private x26569a56dfbc2c6d x3b2905fe94e52614;

        private x26569a56dfbc2c6d x1b233dbc8c05d73f;

        private x5f4b657f68f87baa.x5282b9c42ea1abce x0ad6cb77c00e4e89;

        private MenuItem x569ad91f8a39fe51;

        internal bool xb2504b89d66feca9;

        internal Form xb7d28b7a6d50662f;

        private bool x49b363db01752634;

        public event EventHandler Click
        {
            [MethodImpl(MethodImplOptions.Synchronized)]
            add
            {
                this.x98992f4120a73bb9 = (EventHandler)Delegate.Combine(this.x98992f4120a73bb9, value);
            }
            [MethodImpl(MethodImplOptions.Synchronized)]
            remove
            {
                this.x98992f4120a73bb9 = (EventHandler)Delegate.Remove(this.x98992f4120a73bb9, value);
            }
        }

        public event EventHandler Update
        {
            [MethodImpl(MethodImplOptions.Synchronized)]
            add
            {
                this.x295cb4a1df7a5add = (EventHandler)Delegate.Combine(this.x295cb4a1df7a5add, value);
            }
            [MethodImpl(MethodImplOptions.Synchronized)]
            remove
            {
                this.x295cb4a1df7a5add = (EventHandler)Delegate.Remove(this.x295cb4a1df7a5add, value);
            }
        }

        public event x26569a56dfbc2c6d PopupStart
        {
            [MethodImpl(MethodImplOptions.Synchronized)]
            add
            {
                this.x3b2905fe94e52614 = (x26569a56dfbc2c6d)Delegate.Combine(this.x3b2905fe94e52614, value);
            }
            [MethodImpl(MethodImplOptions.Synchronized)]
            remove
            {
                this.x3b2905fe94e52614 = (x26569a56dfbc2c6d)Delegate.Remove(this.x3b2905fe94e52614, value);
            }
        }

        public event x26569a56dfbc2c6d PopupEnd
        {
            [MethodImpl(MethodImplOptions.Synchronized)]
            add
            {
                this.x1b233dbc8c05d73f = (x26569a56dfbc2c6d)Delegate.Combine(this.x1b233dbc8c05d73f, value);
            }
            [MethodImpl(MethodImplOptions.Synchronized)]
            remove
            {
                this.x1b233dbc8c05d73f = (x26569a56dfbc2c6d)Delegate.Remove(this.x1b233dbc8c05d73f, value);
            }
        }

        public event x5f4b657f68f87baa.x5282b9c42ea1abce PropertyChanged
        {
            [MethodImpl(MethodImplOptions.Synchronized)]
            add
            {
                this.x0ad6cb77c00e4e89 = (x5f4b657f68f87baa.x5282b9c42ea1abce)Delegate.Combine(this.x0ad6cb77c00e4e89, value);
            }
            [MethodImpl(MethodImplOptions.Synchronized)]
            remove
            {
                this.x0ad6cb77c00e4e89 = (x5f4b657f68f87baa.x5282b9c42ea1abce)Delegate.Remove(this.x0ad6cb77c00e4e89, value);
            }
        }

        public MenuItem AttachedMenuItem
        {
            get
            {
                return this.x569ad91f8a39fe51;
            }
            set
            {
                this.x569ad91f8a39fe51 = value;
            }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public xd53b20b7b4b2a08a MenuCommands
        {
            get
            {
                return this.menuItems;
            }
        }

        [DefaultValue("MenuItem"), Localizable(true)]
        public string Text
        {
            get
            {
                if (this.x569ad91f8a39fe51 != null)
                {
                    return this.x569ad91f8a39fe51.Text;
                }
                return this.text;
            }
            set
            {
                if (this.text != value)
                {
                    this.text = value;
                    this.OnPropertyChanged(x5f4b657f68f87baa.x879a106b0501b9dc.Text);
                }
            }
        }

        [DefaultValue(true)]
        public bool Enabled
        {
            get
            {
                if (this.x569ad91f8a39fe51 != null)
                {
                    return this.x569ad91f8a39fe51.Enabled;
                }
                return this.enabled;
            }
            set
            {
                if (this.enabled != value)
                {
                    this.enabled = value;
                    this.OnPropertyChanged(x5f4b657f68f87baa.x879a106b0501b9dc.Enabled);
                }
            }
        }

        [DefaultValue(-1)]
        public int ImageIndex
        {
            get
            {
                return this.imageIndex;
            }
            set
            {
                if (this.imageIndex != value)
                {
                    this.imageIndex = value;
                    this.OnPropertyChanged(x5f4b657f68f87baa.x879a106b0501b9dc.ImageIndex);
                }
            }
        }

        [DefaultValue(null)]
        public ImageList ImageList
        {
            get
            {
                return this.imageList;
            }
            set
            {
                if (this.imageList != value)
                {
                    this.imageList = value;
                    this.OnPropertyChanged(x5f4b657f68f87baa.x879a106b0501b9dc.ImageList);
                }
            }
        }

        [DefaultValue(null)]
        public Image Image
        {
            get
            {
                return this.image;
            }
            set
            {
                if (this.image != value)
                {
                    this.image = value;
                    this.OnPropertyChanged(x5f4b657f68f87baa.x879a106b0501b9dc.Image);
                }
            }
        }

        [DefaultValue(typeof(Shortcut), "None")]
        public Shortcut Shortcut
        {
            get
            {
                if (this.x569ad91f8a39fe51 != null)
                {
                    return this.x569ad91f8a39fe51.Shortcut;
                }
                return this.shortcut;
            }
            set
            {
                if (this.shortcut != value)
                {
                    this.shortcut = value;
                    this.OnPropertyChanged(x5f4b657f68f87baa.x879a106b0501b9dc.Shortcut);
                }
            }
        }

        [DefaultValue(false)]
        public bool Checked
        {
            get
            {
                if (this.x569ad91f8a39fe51 != null)
                {
                    return this.x569ad91f8a39fe51.Checked;
                }
                return this.ichecked;
            }
            set
            {
                if (this.ichecked != value)
                {
                    this.ichecked = value;
                    this.OnPropertyChanged(x5f4b657f68f87baa.x879a106b0501b9dc.Checked);
                }
            }
        }

        [DefaultValue(false)]
        public bool RadioCheck
        {
            get
            {
                if (this.x569ad91f8a39fe51 != null)
                {
                    return this.x569ad91f8a39fe51.RadioCheck;
                }
                return this.radioCheck;
            }
            set
            {
                if (this.radioCheck != value)
                {
                    this.radioCheck = value;
                    this.OnPropertyChanged(x5f4b657f68f87baa.x879a106b0501b9dc.RadioCheck);
                }
            }
        }

        [DefaultValue(false)]
        public bool Break
        {
            get
            {
                if (this.x569ad91f8a39fe51 != null)
                {
                    return this.x569ad91f8a39fe51.Break;
                }
                return this.ibreak;
            }
            set
            {
                if (this.ibreak != value)
                {
                    this.ibreak = value;
                    this.OnPropertyChanged(x5f4b657f68f87baa.x879a106b0501b9dc.Break);
                }
            }
        }

        [DefaultValue(false)]
        public bool MdiList
        {
            get
            {
                if (this.x569ad91f8a39fe51 != null)
                {
                    return this.x569ad91f8a39fe51.MdiList;
                }
                return this.x49b363db01752634;
            }
            set
            {
                if (this.x49b363db01752634 != value)
                {
                    this.x49b363db01752634 = value;
                }
            }
        }

        [DefaultValue(false)]
        public bool Infrequent
        {
            get
            {
                return this.infrequent;
            }
            set
            {
                if (this.infrequent != value)
                {
                    this.infrequent = value;
                    this.OnPropertyChanged(x5f4b657f68f87baa.x879a106b0501b9dc.Infrequent);
                }
            }
        }

        [DefaultValue(true)]
        public bool Visible
        {
            get
            {
                if (this.x569ad91f8a39fe51 != null)
                {
                    return this.x569ad91f8a39fe51.Visible;
                }
                return this.visible;
            }
            set
            {
                if (this.visible != value)
                {
                    this.visible = value;
                    this.OnPropertyChanged(x5f4b657f68f87baa.x879a106b0501b9dc.Visible);
                }
            }
        }

        [Browsable(false)]
        public bool IsParent
        {
            get
            {
                return this.menuItems.Count > 0;
            }
        }

        [DefaultValue(""), Localizable(true)]
        public string Description
        {
            get
            {
                return this.description;
            }
            set
            {
                this.description = value;
            }
        }

        [DefaultValue(null)]
        public object Tag
        {
            get
            {
                return this.tag;
            }
            set
            {
                this.tag = value;
            }
        }

        public x5f4b657f68f87baa()
        {
            this.InternalConstruct("MenuItem", null, -1, Shortcut.None, null);
        }

        public x5f4b657f68f87baa(string text)
        {
            this.InternalConstruct(text, null, -1, Shortcut.None, null);
        }

        public x5f4b657f68f87baa(string text, EventHandler clickHandler)
        {
            this.InternalConstruct(text, null, -1, Shortcut.None, clickHandler);
        }

        public x5f4b657f68f87baa(string text, Shortcut shortcut)
        {
            this.InternalConstruct(text, null, -1, shortcut, null);
        }

        public x5f4b657f68f87baa(string text, Shortcut shortcut, EventHandler clickHandler)
        {
            this.InternalConstruct(text, null, -1, shortcut, clickHandler);
        }

        public x5f4b657f68f87baa(string text, ImageList imageList, int imageIndex)
        {
            this.InternalConstruct(text, imageList, imageIndex, Shortcut.None, null);
        }

        public x5f4b657f68f87baa(string text, ImageList imageList, int imageIndex, Shortcut shortcut)
        {
            this.InternalConstruct(text, imageList, imageIndex, shortcut, null);
        }

        public x5f4b657f68f87baa(string text, ImageList imageList, int imageIndex, EventHandler clickHandler)
        {
            this.InternalConstruct(text, imageList, imageIndex, Shortcut.None, clickHandler);
        }

        public x5f4b657f68f87baa(string text, ImageList imageList, int imageIndex, Shortcut shortcut, EventHandler clickHandler)
        {
            this.InternalConstruct(text, imageList, imageIndex, shortcut, clickHandler);
        }

        protected void InternalConstruct(string text, ImageList imageList, int imageIndex, Shortcut shortcut, EventHandler clickHandler)
        {
            this.text = text;
            this.imageList = imageList;
            this.imageIndex = imageIndex;
            this.shortcut = shortcut;
            this.description = text;
            if (clickHandler != null)
            {
                this.x98992f4120a73bb9 = (EventHandler)Delegate.Combine(this.x98992f4120a73bb9, clickHandler);
            }
            this.enabled = true;
            this.ichecked = false;
            this.radioCheck = false;
            this.ibreak = false;
            this.tag = null;
            this.visible = true;
            this.infrequent = false;
            this.image = null;
            this.menuItems = new xd53b20b7b4b2a08a();
        }

        public virtual void OnPropertyChanged(x5f4b657f68f87baa.x879a106b0501b9dc prop)
        {
            if (this.x0ad6cb77c00e4e89 != null)
            {
                this.x0ad6cb77c00e4e89(this, prop);
            }
        }

        public void PerformClick()
        {
            if (this.x569ad91f8a39fe51 != null)
            {
                this.x569ad91f8a39fe51.PerformClick();
                return;
            }
            this.OnUpdate(EventArgs.Empty);
            this.OnClick(EventArgs.Empty);
        }

        public virtual void OnClick(EventArgs e)
        {
            if (this.x98992f4120a73bb9 != null)
            {
                this.x98992f4120a73bb9(this, e);
            }
        }

        public virtual void OnUpdate(EventArgs e)
        {
            if (this.x295cb4a1df7a5add != null)
            {
                this.x295cb4a1df7a5add(this, e);
            }
        }

        public virtual void OnPopupStart()
        {
            if (this.x3b2905fe94e52614 != null)
            {
                this.x3b2905fe94e52614(this);
            }
        }

        public virtual void OnPopupEnd()
        {
            if (this.x1b233dbc8c05d73f != null)
            {
                this.x1b233dbc8c05d73f(this);
            }
        }
    }
}
