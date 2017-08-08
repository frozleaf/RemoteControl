namespace Sunisoft.IrisSkin.InternalControls
{
    using System;
    using System.Drawing;
    using System.Reflection;
    using System.Windows.Forms;

    internal class x58dd58a96343fde0
    {
        public static Bitmap LoadBitmap(System.Type assemblyType, string imageName)
        {
            return x3a15a33740cd7ff9(assemblyType, imageName, false, new Point(0, 0));
        }

        public static Bitmap LoadBitmap(System.Type assemblyType, string imageName, Point transparentPixel)
        {
            return x3a15a33740cd7ff9(assemblyType, imageName, true, transparentPixel);
        }

        public static ImageList LoadBitmapStrip(System.Type assemblyType, string imageName, Size imageSize)
        {
            return xd1601e45c3cc2056(assemblyType, imageName, imageSize, false, new Point(0, 0));
        }

        public static ImageList LoadBitmapStrip(System.Type assemblyType, string imageName, Size imageSize, Point transparentPixel)
        {
            return xd1601e45c3cc2056(assemblyType, imageName, imageSize, true, transparentPixel);
        }

        public static Cursor LoadCursor(System.Type assemblyType, string cursorName)
        {
            return new Cursor(Assembly.GetAssembly(assemblyType).GetManifestResourceStream(cursorName));
        }

        public static Icon LoadIcon(System.Type assemblyType, string iconName)
        {
            return new Icon(Assembly.GetAssembly(assemblyType).GetManifestResourceStream(iconName));
        }

        public static Icon LoadIcon(System.Type assemblyType, string iconName, Size iconSize)
        {
            return new Icon(LoadIcon(assemblyType, iconName), iconSize);
        }

        protected static Bitmap x3a15a33740cd7ff9(System.Type xeaa6eb5f09df2be8, string xe11224e032957d06, bool x4cf1847391b87d80, Point xc8f338823ae9b184)
        {
            Bitmap bitmap = new Bitmap(Assembly.GetAssembly(xeaa6eb5f09df2be8).GetManifestResourceStream(xe11224e032957d06));
            if (x4cf1847391b87d80)
            {
                Color pixel = bitmap.GetPixel(xc8f338823ae9b184.X, xc8f338823ae9b184.Y);
                bitmap.MakeTransparent(pixel);
            }
            return bitmap;
        }

        protected static ImageList xd1601e45c3cc2056(System.Type xeaa6eb5f09df2be8, string xe11224e032957d06, Size x95dac044246123ac, bool x4cf1847391b87d80, Point xc8f338823ae9b184)
        {
            ImageList list = new ImageList {
                ImageSize = x95dac044246123ac
            };
            Bitmap bitmap = new Bitmap(Assembly.GetAssembly(xeaa6eb5f09df2be8).GetManifestResourceStream(xe11224e032957d06));
            if (x4cf1847391b87d80)
            {
                Color pixel = bitmap.GetPixel(xc8f338823ae9b184.X, xc8f338823ae9b184.Y);
                bitmap.MakeTransparent(pixel);
            }
            list.Images.AddStrip(bitmap);
            return list;
        }
    }
}

