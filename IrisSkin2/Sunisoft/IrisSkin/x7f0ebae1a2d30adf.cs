namespace Sunisoft.IrisSkin
{
    using System;
    using System.Collections;
    using System.Drawing;
    using System.Drawing.Imaging;
    using System.IO;
    using System.Runtime.InteropServices;

    internal class x7f0ebae1a2d30adf
    {
        private string x2de0dddab0e399a9;
        private Hashtable x5e15c490aec0e408;
        private bool x6c7711ed04d2ac90;
        private Stream x6d84580748d50f45;
        private string x9fc8c6f4ce2be206;
        private Hashtable xb41dc438649d784b;
        private Hashtable xc41ca09fad45f9ee;

        public x7f0ebae1a2d30adf()
        {
            this.xc41ca09fad45f9ee = new Hashtable();
            this.x5e15c490aec0e408 = new Hashtable();
            this.xb41dc438649d784b = new Hashtable();
        }

        public x7f0ebae1a2d30adf(Stream stream)
        {
            this.xc41ca09fad45f9ee = new Hashtable();
            this.x5e15c490aec0e408 = new Hashtable();
            this.xb41dc438649d784b = new Hashtable();
            this.SkinStream = stream;
        }

        public x7f0ebae1a2d30adf(string skin)
        {
            this.xc41ca09fad45f9ee = new Hashtable();
            this.x5e15c490aec0e408 = new Hashtable();
            this.xb41dc438649d784b = new Hashtable();
            this.SkinFile = skin;
        }

        public Bitmap GetBitmap(string key)
        {
            if (this.Ready && this.xc41ca09fad45f9ee.Contains(key))
            {
                return (Bitmap) Image.FromStream((Stream) this.xc41ca09fad45f9ee[key]);
            }
            return null;
        }

        public Bitmap GetBitmap(string key, int spitCount, int spitIndex)
        {
            if (this.Ready && this.xc41ca09fad45f9ee.Contains(key))
            {
                return x448fd9ab43628c71.SplitBitmap(this.GetBitmap(key), spitCount, spitIndex);
            }
            return null;
        }

        public bool GetBool(string key)
        {
            if (!this.x6c7711ed04d2ac90)
            {
                return false;
            }
            if (!this.xb41dc438649d784b.ContainsKey(key))
            {
                return false;
            }
            return (((int) this.xb41dc438649d784b[key]) == 1);
        }

        public Color GetColor(string key)
        {
            Color color = Color.FromArgb(0xff, 0xff, 0xff);
            if (!this.x6c7711ed04d2ac90)
            {
                return color;
            }
            if (!this.x5e15c490aec0e408.ContainsKey(key))
            {
                return color;
            }
            int sysColor = (int) this.x5e15c490aec0e408[key];
            if ((sysColor & 0xff000000L) == 0xff000000L)
            {
                sysColor = x61467fe65a98f20c.GetSysColor(sysColor & 0xff);
            }
            return Color.FromArgb(sysColor & 0xff, (sysColor & 0xff00) >> 8, (sysColor & 0xff0000) >> 0x10);
        }

        public int GetInt(string key)
        {
            if (this.x5e15c490aec0e408.ContainsKey(key))
            {
                return (int) this.x5e15c490aec0e408[key];
            }
            return 0;
        }

        protected virtual void OnSkinFileChanged(string fileName)
        {
            this.x2fd0a49330761d63(fileName);
            this.x9fc8c6f4ce2be206 = fileName;
        }

        protected virtual void OnSkinStreamChanged(Stream stream)
        {
            this.x2fd0a49330761d63(stream);
            this.x6d84580748d50f45 = stream;
        }

        public void SetBitmap(string key, Bitmap buf)
        {
            if (this.x6c7711ed04d2ac90)
            {
                MemoryStream stream = new MemoryStream();
                buf.Save(stream, ImageFormat.Bmp);
                this.xc41ca09fad45f9ee[key] = stream;
            }
        }

        public void SetBool(string key, bool b)
        {
            if (this.x6c7711ed04d2ac90)
            {
                if (b)
                {
                    this.xb41dc438649d784b[key] = 1;
                }
                else
                {
                    this.xb41dc438649d784b[key] = 0;
                }
            }
        }

        public void SetColor(string key, Color color)
        {
            if (this.x6c7711ed04d2ac90)
            {
                this.x5e15c490aec0e408[key] = color.ToArgb();
            }
        }

        protected virtual void SkinPasswordChanged()
        {
            this.x2fd0a49330761d63(this.x6d84580748d50f45);
        }

        private void x0c4b0fbba1d27e4d()
        {
            this.xc41ca09fad45f9ee.Clear();
            this.x5e15c490aec0e408.Clear();
            this.xb41dc438649d784b.Clear();
        }

        private void x2fd0a49330761d63(Stream x23cda4cfdf81f2cf)
        {
            this.x6c7711ed04d2ac90 = false;
            this.x0c4b0fbba1d27e4d();
            Stream writeStream = new MemoryStream();
            if (x2b13dab2573bcd5e.Unzip(x23cda4cfdf81f2cf, "SKINDATA.SK2", writeStream, this.x2de0dddab0e399a9) == xfba9214ce91902fb.Success)
            {
                BinaryReader reader = new BinaryReader(writeStream);
                writeStream.Seek(0L, SeekOrigin.Begin);
                xe148cfaee7cc82a1 xecfaeecca1 = (xe148cfaee7cc82a1) x49cceb88e1b6126e(reader, typeof(xe148cfaee7cc82a1), Marshal.SizeOf(typeof(xe148cfaee7cc82a1)));
                writeStream.Seek(0L, SeekOrigin.Begin);
                x57f24fc7026c57d0 xffccd = (x57f24fc7026c57d0) x49cceb88e1b6126e(reader, typeof(x57f24fc7026c57d0), Marshal.SizeOf(typeof(x57f24fc7026c57d0)));
                if (xffccd.FirstNode != 0)
                {
                    writeStream.Seek((long) xffccd.FirstNode, SeekOrigin.Begin);
                    x8632e8b92c54e123 xebce = (x8632e8b92c54e123) x49cceb88e1b6126e(reader, typeof(x8632e8b92c54e123), Marshal.SizeOf(typeof(x8632e8b92c54e123)));
                    do
                    {
                        int num;
                        writeStream.Seek((long) xebce.NodeStart, SeekOrigin.Begin);
                        switch (xebce.Type)
                        {
                            case xab0f0a5b63c83169.Stream:
                            {
                                MemoryStream stream = new MemoryStream(reader.ReadBytes((int) xebce.NodeSize), 0, (int) xebce.NodeSize);
                                this.xc41ca09fad45f9ee.Add(xebce.Key, stream);
                                break;
                            }
                            case xab0f0a5b63c83169.Int:
                                num = reader.ReadInt32();
                                this.x5e15c490aec0e408.Add(xebce.Key, num);
                                break;

                            case xab0f0a5b63c83169.Bool:
                                num = reader.ReadByte();
                                this.xb41dc438649d784b.Add(xebce.Key, num);
                                break;
                        }
                        writeStream.Seek((long) xebce.NextNode, SeekOrigin.Begin);
                        xebce = (x8632e8b92c54e123) x49cceb88e1b6126e(reader, typeof(x8632e8b92c54e123), Marshal.SizeOf(typeof(x8632e8b92c54e123)));
                        num = 0;
                    }
                    while (xebce.NextNode != 0);
                    this.x6c7711ed04d2ac90 = true;
                }
            }
        }

        private void x2fd0a49330761d63(string xafe2f3653ee64ebc)
        {
            FileStream stream;
            if (File.Exists(xafe2f3653ee64ebc))
            {
                stream = File.Open(xafe2f3653ee64ebc, FileMode.Open, FileAccess.Read, FileShare.Read);
            }
            else
            {
                stream = File.Open(Path.GetFileName(xafe2f3653ee64ebc), FileMode.Open, FileAccess.Read, FileShare.Read);
            }
            try
            {
                this.x2fd0a49330761d63(stream);
            }
            finally
            {
                stream.Close();
            }
        }

        private static unsafe object x49cceb88e1b6126e(BinaryReader xe134235b3526fa75, Type x43163d22e8cd5a71, int x0ceec69a97f73617)
        {
            object obj2;
            fixed (byte* numRef = xe134235b3526fa75.ReadBytes(x0ceec69a97f73617))
            {
                obj2 = Marshal.PtrToStructure((IntPtr) numRef, x43163d22e8cd5a71);
            }
            return obj2;
        }

        public bool Ready
        {
            get
            {
                return this.x6c7711ed04d2ac90;
            }
        }

        public string SkinFile
        {
            get
            {
                return this.x9fc8c6f4ce2be206;
            }
            set
            {
                this.OnSkinFileChanged(value);
            }
        }

        public string SkinPassword
        {
            get
            {
                return this.x2de0dddab0e399a9;
            }
            set
            {
                if (this.x2de0dddab0e399a9 != value)
                {
                    this.x2de0dddab0e399a9 = value;
                    this.SkinPasswordChanged();
                }
            }
        }

        public Stream SkinStream
        {
            get
            {
                return this.x6d84580748d50f45;
            }
            set
            {
                this.OnSkinStreamChanged(value);
            }
        }
    }
}

