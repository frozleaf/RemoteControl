namespace Sunisoft.IrisSkin.InternalControls
{
    using System;
    using System.Drawing;

    internal class x2cc390e9409b0f3f
    {
        protected bool bottomBorder;
        protected bool chevron;
        protected int col;
        protected x5f4b657f68f87baa command;
        protected Rectangle drawRect;
        protected bool enabled;
        protected bool expansion;
        protected bool infrequent;
        protected char mnemonic;
        protected int row;
        protected Rectangle selectRect;
        protected bool separator;
        protected bool subMenu;
        protected bool topBorder;
        protected bool vertSeparator;

        public x2cc390e9409b0f3f(Rectangle drawRect)
        {
            this.row = -1;
            this.col = -1;
            this.mnemonic = '0';
            this.enabled = true;
            this.subMenu = false;
            this.expansion = false;
            this.separator = false;
            this.vertSeparator = false;
            this.topBorder = false;
            this.bottomBorder = false;
            this.infrequent = false;
            this.chevron = true;
            this.drawRect = drawRect;
            this.selectRect = drawRect;
            this.command = null;
        }

        public x2cc390e9409b0f3f(x5f4b657f68f87baa command, Rectangle drawRect)
        {
            this.InternalConstruct(command, drawRect, drawRect, -1, -1);
        }

        public x2cc390e9409b0f3f(Rectangle drawRect, bool expansion)
        {
            this.row = -1;
            this.col = -1;
            this.mnemonic = '0';
            this.enabled = true;
            this.subMenu = false;
            this.expansion = expansion;
            this.separator = !expansion;
            this.vertSeparator = !expansion;
            this.topBorder = false;
            this.bottomBorder = false;
            this.infrequent = false;
            this.chevron = false;
            this.drawRect = drawRect;
            this.selectRect = drawRect;
            this.command = null;
        }

        public x2cc390e9409b0f3f(x5f4b657f68f87baa command, Rectangle drawRect, Rectangle selectRect)
        {
            this.InternalConstruct(command, drawRect, selectRect, -1, -1);
        }

        public x2cc390e9409b0f3f(x5f4b657f68f87baa command, Rectangle drawRect, int row, int col)
        {
            this.InternalConstruct(command, drawRect, drawRect, row, col);
        }

        public void InternalConstruct(x5f4b657f68f87baa command, Rectangle drawRect, Rectangle selectRect, int row, int col)
        {
            this.row = row;
            this.col = col;
            this.enabled = command.Enabled;
            this.expansion = false;
            this.vertSeparator = false;
            this.drawRect = drawRect;
            this.selectRect = selectRect;
            this.command = command;
            this.topBorder = false;
            this.bottomBorder = false;
            this.infrequent = command.Infrequent;
            this.chevron = false;
            this.separator = this.command.Text == "-";
            this.subMenu = this.command.MenuCommands.Count > 0;
            int index = -1;
            if (command.Text != null)
            {
                index = command.Text.IndexOf('&');
            }
            if ((index != -1) && (index < (command.Text.Length - 1)))
            {
                this.mnemonic = char.ToUpper(command.Text[index + 1]);
            }
        }

        public bool BottomBorder
        {
            get
            {
                return this.bottomBorder;
            }
            set
            {
                this.bottomBorder = value;
            }
        }

        public bool Chevron
        {
            get
            {
                return this.chevron;
            }
        }

        public int Col
        {
            get
            {
                return this.col;
            }
        }

        public Rectangle DrawRect
        {
            get
            {
                return this.drawRect;
            }
            set
            {
                this.drawRect = value;
            }
        }

        public bool Enabled
        {
            get
            {
                return this.enabled;
            }
        }

        public bool Expansion
        {
            get
            {
                return this.expansion;
            }
        }

        public bool Infrequent
        {
            get
            {
                return this.infrequent;
            }
            set
            {
                this.infrequent = value;
            }
        }

        public x5f4b657f68f87baa MenuCommand
        {
            get
            {
                return this.command;
            }
        }

        public char Mnemonic
        {
            get
            {
                return this.mnemonic;
            }
        }

        public int Row
        {
            get
            {
                return this.row;
            }
        }

        public Rectangle SelectRect
        {
            get
            {
                return this.selectRect;
            }
            set
            {
                this.selectRect = value;
            }
        }

        public bool Separator
        {
            get
            {
                return this.separator;
            }
        }

        public bool SubMenu
        {
            get
            {
                return this.subMenu;
            }
        }

        public bool TopBorder
        {
            get
            {
                return this.topBorder;
            }
            set
            {
                this.topBorder = value;
            }
        }

        public bool VerticalSeparator
        {
            get
            {
                return this.vertSeparator;
            }
        }
    }
}

