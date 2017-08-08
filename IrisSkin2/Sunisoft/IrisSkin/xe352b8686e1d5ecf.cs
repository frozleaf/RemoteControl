namespace Sunisoft.IrisSkin
{
    using System;
    using System.Globalization;
    using System.Reflection;
    using System.Windows.Forms;

    internal class xe352b8686e1d5ecf
    {
        private static Assembly xe16f4e54dbd828d7;

        static xe352b8686e1d5ecf()
        {
            try
            {
                xe16f4e54dbd828d7 = Assembly.Load("IrisSkin2_NetAdvantage53Win");
            }
            catch
            {
            }
        }

        public static void Create(Control control, SkinEngine engine)
        {
            try
            {
                if (xe16f4e54dbd828d7 != null)
                {
                    xe16f4e54dbd828d7.CreateInstance("IrisSkinNetAdvantage53Win.Implement3rdControl", true, BindingFlags.CreateInstance, null, new object[] { control, engine }, CultureInfo.CurrentCulture, null);
                }
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception);
            }
        }
    }
}

