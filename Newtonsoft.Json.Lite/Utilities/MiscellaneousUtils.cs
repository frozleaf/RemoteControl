namespace Newtonsoft.Json.Utilities
{
    using System;
    using System.Globalization;
    using System.Runtime.InteropServices;

    internal static class MiscellaneousUtils
    {
        public static int ByteArrayCompare(byte[] a1, byte[] a2)
        {
            int num = a1.Length.CompareTo(a2.Length);
            if (num != 0)
            {
                return num;
            }
            for (int i = 0; i < a1.Length; i++)
            {
                int num3 = a1[i].CompareTo(a2[i]);
                if (num3 != 0)
                {
                    return num3;
                }
            }
            return 0;
        }

        public static string BytesToHex(byte[] bytes)
        {
            return BytesToHex(bytes, false);
        }

        public static string BytesToHex(byte[] bytes, bool removeDashes)
        {
            string str = BitConverter.ToString(bytes);
            if (removeDashes)
            {
                str = str.Replace("-", "");
            }
            return str;
        }

        public static ArgumentOutOfRangeException CreateArgumentOutOfRangeException(string paramName, object actualValue, string message)
        {
            return new ArgumentOutOfRangeException(paramName, message + Environment.NewLine + "Actual value was {0}.".FormatWith(CultureInfo.InvariantCulture, new object[] { actualValue }));
        }

        public static string GetLocalName(string qualifiedName)
        {
            string str;
            string str2;
            GetQualifiedNameParts(qualifiedName, out str, out str2);
            return str2;
        }

        public static string GetPrefix(string qualifiedName)
        {
            string str;
            string str2;
            GetQualifiedNameParts(qualifiedName, out str, out str2);
            return str;
        }

        public static void GetQualifiedNameParts(string qualifiedName, out string prefix, out string localName)
        {
            int index = qualifiedName.IndexOf(':');
            if (((index == -1) || (index == 0)) || ((qualifiedName.Length - 1) == index))
            {
                prefix = null;
                localName = qualifiedName;
            }
            else
            {
                prefix = qualifiedName.Substring(0, index);
                localName = qualifiedName.Substring(index + 1);
            }
        }

        public static byte[] HexToBytes(string hex)
        {
            string str = hex.Replace("-", string.Empty);
            byte[] buffer = new byte[str.Length / 2];
            int num = 4;
            int index = 0;
            foreach (char ch in str)
            {
                int num3 = (ch - '0') % 0x20;
                if (num3 > 9)
                {
                    num3 -= 7;
                }
                buffer[index] = (byte) (buffer[index] | ((byte) (num3 << num)));
                num ^= 4;
                if (num != 0)
                {
                    index++;
                }
            }
            return buffer;
        }

        public static string ToString(object value)
        {
            if (value == null)
            {
                return "{null}";
            }
            return ((value is string) ? ("\"" + value.ToString() + "\"") : value.ToString());
        }

        public static bool TryAction<T>(Creator<T> creator, out T output)
        {
            ValidationUtils.ArgumentNotNull(creator, "creator");
            try
            {
                output = creator();
                return true;
            }
            catch
            {
                output = default(T);
                return false;
            }
        }

        public static bool ValueEquals(object objA, object objB)
        {
            if ((objA == null) && (objB == null))
            {
                return true;
            }
            if ((objA != null) && (objB == null))
            {
                return false;
            }
            if ((objA == null) && (objB != null))
            {
                return false;
            }
            if (objA.GetType() != objB.GetType())
            {
                if (ConvertUtils.IsInteger(objA) && ConvertUtils.IsInteger(objB))
                {
                    return Convert.ToDecimal(objA, CultureInfo.CurrentCulture).Equals(Convert.ToDecimal(objB, CultureInfo.CurrentCulture));
                }
                return (((((objA is double) || (objA is float)) || (objA is decimal)) && (((objB is double) || (objB is float)) || (objB is decimal))) && MathUtils.ApproxEquals(Convert.ToDouble(objA, CultureInfo.CurrentCulture), Convert.ToDouble(objB, CultureInfo.CurrentCulture)));
            }
            return objA.Equals(objB);
        }
    }
}

