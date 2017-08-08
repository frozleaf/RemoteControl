namespace Newtonsoft.Json.Utilities
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Reflection;
    using System.Runtime.InteropServices;

    internal static class EnumUtils
    {
        public static IList<T> GetFlagsValues<T>(T value) where T: struct
        {
            Type type = typeof(T);
            if (!type.IsDefined(typeof(FlagsAttribute), false))
            {
                throw new Exception("Enum type {0} is not a set of flags.".FormatWith(CultureInfo.InvariantCulture, new object[] { type }));
            }
            Type underlyingType = Enum.GetUnderlyingType(value.GetType());
            ulong num = Convert.ToUInt64(value, CultureInfo.InvariantCulture);
            EnumValues<ulong> namesAndValues = GetNamesAndValues<T>();
            IList<T> list = new List<T>();
            foreach (EnumValue<ulong> value2 in namesAndValues)
            {
                if (((num & value2.Value) == value2.Value) && (value2.Value != 0L))
                {
                    list.Add((T) Convert.ChangeType(value2.Value, underlyingType, CultureInfo.CurrentCulture));
                }
            }
            if ((list.Count == 0) && (namesAndValues.SingleOrDefault<EnumValue<ulong>>(v => (v.Value == 0L)) != null))
            {
                list.Add(default(T));
            }
            return list;
        }

        public static TEnumType GetMaximumValue<TEnumType>(Type enumType) where TEnumType: IConvertible, IComparable<TEnumType>
        {
            if (enumType == null)
            {
                throw new ArgumentNullException("enumType");
            }
            Type underlyingType = Enum.GetUnderlyingType(enumType);
            if (!typeof(TEnumType).IsAssignableFrom(underlyingType))
            {
                throw new ArgumentException(string.Format(CultureInfo.InvariantCulture, "TEnumType is not assignable from the enum's underlying type of {0}.", new object[] { underlyingType.Name }));
            }
            ulong num = 0L;
            IList<object> values = GetValues(enumType);
            if (enumType.IsDefined(typeof(FlagsAttribute), false))
            {
                foreach (TEnumType local in values)
                {
                    num |= local.ToUInt64(CultureInfo.InvariantCulture);
                }
            }
            else
            {
                foreach (TEnumType local in values)
                {
                    ulong num2 = local.ToUInt64(CultureInfo.InvariantCulture);
                    if (num.CompareTo(num2) == -1)
                    {
                        num = num2;
                    }
                }
            }
            return (TEnumType) Convert.ChangeType(num, typeof(TEnumType), CultureInfo.InvariantCulture);
        }

        public static IList<string> GetNames<T>()
        {
            return GetNames(typeof(T));
        }

        public static IList<string> GetNames(Type enumType)
        {
            if (!enumType.IsEnum)
            {
                throw new ArgumentException("Type '" + enumType.Name + "' is not an enum.");
            }
            List<string> list = new List<string>();
            IEnumerable<FieldInfo> enumerable = from field in enumType.GetFields()
                where field.IsLiteral
                select field;
            foreach (FieldInfo info in enumerable)
            {
                list.Add(info.Name);
            }
            return list;
        }

        public static EnumValues<ulong> GetNamesAndValues<T>() where T: struct
        {
            return GetNamesAndValues<ulong>(typeof(T));
        }

        public static EnumValues<TUnderlyingType> GetNamesAndValues<TEnum, TUnderlyingType>() where TEnum: struct where TUnderlyingType: struct
        {
            return GetNamesAndValues<TUnderlyingType>(typeof(TEnum));
        }

        public static EnumValues<TUnderlyingType> GetNamesAndValues<TUnderlyingType>(Type enumType) where TUnderlyingType: struct
        {
            if (enumType == null)
            {
                throw new ArgumentNullException("enumType");
            }
            ValidationUtils.ArgumentTypeIsEnum(enumType, "enumType");
            IList<object> list = GetValues(enumType);
            IList<string> names = GetNames(enumType);
            EnumValues<TUnderlyingType> values = new EnumValues<TUnderlyingType>();
            for (int i = 0; i < list.Count; i++)
            {
                try
                {
                    values.Add(new EnumValue<TUnderlyingType>(names[i], (TUnderlyingType) Convert.ChangeType(list[i], typeof(TUnderlyingType), CultureInfo.CurrentCulture)));
                }
                catch (OverflowException exception)
                {
                    throw new Exception(string.Format(CultureInfo.InvariantCulture, "Value from enum with the underlying type of {0} cannot be added to dictionary with a value type of {1}. Value was too large: {2}", new object[] { Enum.GetUnderlyingType(enumType), typeof(TUnderlyingType), Convert.ToUInt64(list[i], CultureInfo.InvariantCulture) }), exception);
                }
            }
            return values;
        }

        public static IList<T> GetValues<T>()
        {
            return GetValues(typeof(T)).Cast<T>().ToList<T>();
        }

        public static IList<object> GetValues(Type enumType)
        {
            if (!enumType.IsEnum)
            {
                throw new ArgumentException("Type '" + enumType.Name + "' is not an enum.");
            }
            List<object> list = new List<object>();
            IEnumerable<FieldInfo> enumerable = from field in enumType.GetFields()
                where field.IsLiteral
                select field;
            foreach (FieldInfo info in enumerable)
            {
                object item = info.GetValue(enumType);
                list.Add(item);
            }
            return list;
        }

        public static T Parse<T>(string enumMemberName) where T: struct
        {
            return Parse<T>(enumMemberName, false);
        }

        public static T Parse<T>(string enumMemberName, bool ignoreCase) where T: struct
        {
            ValidationUtils.ArgumentTypeIsEnum(typeof(T), "T");
            return (T) Enum.Parse(typeof(T), enumMemberName, ignoreCase);
        }

        public static bool TryParse<T>(string enumMemberName, bool ignoreCase, out T value) where T: struct
        {
            ValidationUtils.ArgumentTypeIsEnum(typeof(T), "T");
            return MiscellaneousUtils.TryAction<T>(() => Parse<T>(enumMemberName, ignoreCase), out value);
        }
    }
}

