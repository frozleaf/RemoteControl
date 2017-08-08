using System;
using System.Drawing;
using System.Windows.Forms;

namespace Sunisoft.IrisSkin.InternalControls
{
    internal class xd53b20b7b4b2a08a : x81f0e41c32f52159
    {
        protected string extraText;

        protected Font extraFont;

        protected Color extraTextColor;

        protected Brush extraTextBrush;

        protected Color extraBackColor;

        protected Brush extraBackBrush;

        protected bool showInfrequent;

        public x5f4b657f68f87baa this[int index]
        {
            get
            {
                return base.List[index] as x5f4b657f68f87baa;
            }
        }

        public x5f4b657f68f87baa this[string text]
        {
            get
            {
                foreach (x5f4b657f68f87baa x5f4b657f68f87baa in base.List)
                {
                    if (x5f4b657f68f87baa.Text == text)
                    {
                        return x5f4b657f68f87baa;
                    }
                }
                return null;
            }
        }

        public string ExtraText
        {
            get
            {
                return this.extraText;
            }
            set
            {
                this.extraText = value;
            }
        }

        public Font ExtraFont
        {
            get
            {
                return this.extraFont;
            }
            set
            {
                this.extraFont = value;
            }
        }

        public Color ExtraTextColor
        {
            get
            {
                return this.extraTextColor;
            }
            set
            {
                this.extraTextColor = value;
            }
        }

        public Brush ExtraTextBrush
        {
            get
            {
                return this.extraTextBrush;
            }
            set
            {
                this.extraTextBrush = value;
            }
        }

        public Color ExtraBackColor
        {
            get
            {
                return this.extraBackColor;
            }
            set
            {
                this.extraBackColor = value;
            }
        }

        public Brush ExtraBackBrush
        {
            get
            {
                return this.extraBackBrush;
            }
            set
            {
                this.extraBackBrush = value;
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

        public xd53b20b7b4b2a08a()
        {
            this.extraText = "";
            this.extraFont = SystemInformation.MenuFont;
            this.extraTextColor = SystemColors.ActiveCaptionText;
            this.extraTextBrush = null;
            this.extraBackColor = SystemColors.ActiveCaption;
            this.extraBackBrush = null;
            this.showInfrequent = false;
        }

        public x5f4b657f68f87baa Add(x5f4b657f68f87baa value)
        {
            base.List.Add(value);
            return value;
        }

        public void AddRange(x5f4b657f68f87baa[] values)
        {
            for (int i = 0; i < values.Length; i++)
            {
                x5f4b657f68f87baa value = values[i];
                this.Add(value);
            }
        }

        public void Remove(x5f4b657f68f87baa value)
        {
            base.List.Remove(value);
        }

        public void Insert(int index, x5f4b657f68f87baa value)
        {
            base.List.Insert(index, value);
        }

        public bool Contains(x5f4b657f68f87baa value)
        {
            return base.List.Contains(value);
        }

        public int IndexOf(x5f4b657f68f87baa value)
        {
            return base.List.IndexOf(value);
        }

        public bool VisibleItems()
        {
            foreach (x5f4b657f68f87baa x5f4b657f68f87baa in base.List)
            {
                if (x5f4b657f68f87baa.Visible && x5f4b657f68f87baa.Text != "-" && (x5f4b657f68f87baa.MenuCommands.Count <= 0 || x5f4b657f68f87baa.MenuCommands.VisibleItems()))
                {
                    return true;
                }
            }
            return false;
        }
    }
}
