namespace Sunisoft.IrisSkin
{
    using Sunisoft.IrisSkin.Design;
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Drawing.Design;
    using System.IO;

    [Serializable, TypeConverter(typeof(TypeConverter)), ToolboxBitmap(typeof(SkinEngine), "ItemIcon.bmp"), ToolboxItem(false)]
    public class SkinCollectionItem : Component
    {
        [NonSerialized]
        private x7f0ebae1a2d30adf reader;
        private string skinFile;
        private MemoryStream skinStream;

        public SkinCollectionItem()
        {
            this.reader = new x7f0ebae1a2d30adf();
        }

        public SkinCollectionItem(string fileName)
        {
            this.reader = new x7f0ebae1a2d30adf();
            this.SkinFile = fileName;
        }

        protected void OnSkinFileChanged(string file)
        {
            if ((file == null) || (file == ""))
            {
                this.skinStream = null;
            }
            else
            {
                try
                {
                    this.reader.SkinFile = file;
                    FileStream input = File.Open(file, FileMode.Open, FileAccess.Read, FileShare.Read);
                    BinaryReader reader = new BinaryReader(input);
                    this.skinStream = new MemoryStream(reader.ReadBytes((int) input.Length));
                    reader.Close();
                    this.skinFile = file;
                }
                catch
                {
                }
            }
        }

        [Category("Skins"), Editor(typeof(x1cc8dd3ebd3495cd), typeof(UITypeEditor)), Description("Specifies the skin file(.sui file). "), DesignOnly(true)]
        public string SkinFile
        {
            get
            {
                return this.skinFile;
            }
            set
            {
                if (this.skinFile != value)
                {
                    this.OnSkinFileChanged(value);
                }
            }
        }

        [Description("Specifies the password of the skinFile"), Category("Skins"), DefaultValue("")]
        public string SkinPassword
        {
            get
            {
                return this.reader.SkinPassword;
            }
            set
            {
                this.reader.SkinPassword = value;
            }
        }

        [Browsable(false)]
        public MemoryStream SkinSteam
        {
            get
            {
                return this.skinStream;
            }
            set
            {
                this.skinStream = value;
            }
        }
    }
}

