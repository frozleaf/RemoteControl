namespace Newtonsoft.Json.Utilities
{
    using Newtonsoft.Json.Serialization;
    using System;
    using System.ComponentModel;
    using System.Data.SqlTypes;
    using System.Globalization;
    using System.Reflection;
    using System.Runtime.InteropServices;

    internal static class ConvertUtils
    {
        private static readonly ThreadSafeStore<TypeConvertKey, Func<object, object>> CastConverters = new ThreadSafeStore<TypeConvertKey, Func<object, object>>(new Func<TypeConvertKey, Func<object, object>>(ConvertUtils.CreateCastConverter));

        public static bool CanConvertType(Type initialType, Type targetType, bool allowTypeNameToString)
        {
            ValidationUtils.ArgumentNotNull(initialType, "initialType");
            ValidationUtils.ArgumentNotNull(targetType, "targetType");
            if (ReflectionUtils.IsNullableType(targetType))
            {
                targetType = Nullable.GetUnderlyingType(targetType);
            }
            if (targetType == initialType)
            {
                return true;
            }
            if (typeof(IConvertible).IsAssignableFrom(initialType) && typeof(IConvertible).IsAssignableFrom(targetType))
            {
                return true;
            }
            if ((initialType == typeof(DateTime)) && (targetType == typeof(DateTimeOffset)))
            {
                return true;
            }
            if ((initialType == typeof(Guid)) && ((targetType == typeof(Guid)) || (targetType == typeof(string))))
            {
                return true;
            }
            if ((initialType == typeof(Type)) && (targetType == typeof(string)))
            {
                return true;
            }
            TypeConverter converter = GetConverter(initialType);
            if ((((converter != null) && !IsComponentConverter(converter)) && converter.CanConvertTo(targetType)) && (allowTypeNameToString || (converter.GetType() != typeof(TypeConverter))))
            {
                return true;
            }
            TypeConverter converter2 = GetConverter(targetType);
            return ((((converter2 != null) && !IsComponentConverter(converter2)) && converter2.CanConvertFrom(initialType)) || ((initialType == typeof(DBNull)) && ReflectionUtils.IsNullable(targetType)));
        }

        public static T Convert<T>(object initialValue)
        {
            return Convert<T>(initialValue, CultureInfo.CurrentCulture);
        }

        public static T Convert<T>(object initialValue, CultureInfo culture)
        {
            return (T) Convert(initialValue, culture, typeof(T));
        }

        public static object Convert(object initialValue, CultureInfo culture, Type targetType)
        {
            if (initialValue == null)
            {
                throw new ArgumentNullException("initialValue");
            }
            if (ReflectionUtils.IsNullableType(targetType))
            {
                targetType = Nullable.GetUnderlyingType(targetType);
            }
            Type t = initialValue.GetType();
            if (targetType == t)
            {
                return initialValue;
            }
            if ((initialValue is string) && typeof(Type).IsAssignableFrom(targetType))
            {
                return Type.GetType((string) initialValue, true);
            }
            if ((targetType.IsInterface || targetType.IsGenericTypeDefinition) || targetType.IsAbstract)
            {
                throw new ArgumentException("Target type {0} is not a value type or a non-abstract class.".FormatWith(CultureInfo.InvariantCulture, new object[] { targetType }), "targetType");
            }
            if ((initialValue is IConvertible) && typeof(IConvertible).IsAssignableFrom(targetType))
            {
                if (targetType.IsEnum)
                {
                    if (initialValue is string)
                    {
                        return Enum.Parse(targetType, initialValue.ToString(), true);
                    }
                    if (IsInteger(initialValue))
                    {
                        return Enum.ToObject(targetType, initialValue);
                    }
                }
                return System.Convert.ChangeType(initialValue, targetType, culture);
            }
            if ((initialValue is DateTime) && (targetType == typeof(DateTimeOffset)))
            {
                return new DateTimeOffset((DateTime) initialValue);
            }
            if (initialValue is string)
            {
                if (targetType == typeof(Guid))
                {
                    return new Guid((string) initialValue);
                }
                if (targetType == typeof(Uri))
                {
                    return new Uri((string) initialValue);
                }
                if (targetType == typeof(TimeSpan))
                {
                    return TimeSpan.Parse((string) initialValue);
                }
            }
            TypeConverter converter = GetConverter(t);
            if ((converter != null) && converter.CanConvertTo(targetType))
            {
                return converter.ConvertTo(null, culture, initialValue, targetType);
            }
            TypeConverter converter2 = GetConverter(targetType);
            if ((converter2 != null) && converter2.CanConvertFrom(t))
            {
                return converter2.ConvertFrom(null, culture, initialValue);
            }
            if (initialValue == DBNull.Value)
            {
                if (!ReflectionUtils.IsNullable(targetType))
                {
                    throw new Exception("Can not convert null {0} into non-nullable {1}.".FormatWith(CultureInfo.InvariantCulture, new object[] { t, targetType }));
                }
                return EnsureTypeAssignable(null, t, targetType);
            }
            if (!(initialValue is INullable))
            {
                throw new Exception("Can not convert from {0} to {1}.".FormatWith(CultureInfo.InvariantCulture, new object[] { t, targetType }));
            }
            return EnsureTypeAssignable(ToValue((INullable) initialValue), t, targetType);
        }

        public static T ConvertOrCast<T>(object initialValue)
        {
            return ConvertOrCast<T>(initialValue, CultureInfo.CurrentCulture);
        }

        public static T ConvertOrCast<T>(object initialValue, CultureInfo culture)
        {
            return (T) ConvertOrCast(initialValue, culture, typeof(T));
        }

        public static object ConvertOrCast(object initialValue, CultureInfo culture, Type targetType)
        {
            object obj2;
            if (targetType == typeof(object))
            {
                return initialValue;
            }
            if ((initialValue == null) && ReflectionUtils.IsNullable(targetType))
            {
                return null;
            }
            if (TryConvert(initialValue, culture, targetType, out obj2))
            {
                return obj2;
            }
            return EnsureTypeAssignable(initialValue, ReflectionUtils.GetObjectType(initialValue), targetType);
        }

        private static Func<object, object> CreateCastConverter(TypeConvertKey t)
        {
            MethodInfo method = t.TargetType.GetMethod("op_Implicit", new Type[] { t.InitialType });
            if (method == null)
            {
                method = t.TargetType.GetMethod("op_Explicit", new Type[] { t.InitialType });
            }
            if (method == null)
            {
                return null;
            }
            MethodCall<object, object> call = JsonTypeReflector.ReflectionDelegateFactory.CreateMethodCall<object>(method);
            return o => call(null, new object[] { o });
        }

        private static object EnsureTypeAssignable(object value, Type initialType, Type targetType)
        {
            Type c = (value != null) ? value.GetType() : null;
            if (value != null)
            {
                if (targetType.IsAssignableFrom(c))
                {
                    return value;
                }
                Func<object, object> func = CastConverters.Get(new TypeConvertKey(c, targetType));
                if (func != null)
                {
                    return func(value);
                }
            }
            else if (ReflectionUtils.IsNullable(targetType))
            {
                return null;
            }
            throw new Exception("Could not cast or convert from {0} to {1}.".FormatWith(CultureInfo.InvariantCulture, new object[] { (initialType != null) ? initialType.ToString() : "{null}", targetType }));
        }

        internal static TypeConverter GetConverter(Type t)
        {
            return JsonTypeReflector.GetTypeConverter(t);
        }

        private static bool IsComponentConverter(TypeConverter converter)
        {
            return (converter is ComponentConverter);
        }

        public static bool IsInteger(object value)
        {
            switch (System.Convert.GetTypeCode(value))
            {
                case TypeCode.SByte:
                case TypeCode.Byte:
                case TypeCode.Int16:
                case TypeCode.UInt16:
                case TypeCode.Int32:
                case TypeCode.UInt32:
                case TypeCode.Int64:
                case TypeCode.UInt64:
                    return true;
            }
            return false;
        }

        public static object ToValue(INullable nullableValue)
        {
            if (nullableValue == null)
            {
                return null;
            }
            if (nullableValue is SqlInt32)
            {
                return ToValue((SqlInt32) nullableValue);
            }
            if (nullableValue is SqlInt64)
            {
                return ToValue((SqlInt64) nullableValue);
            }
            if (nullableValue is SqlBoolean)
            {
                return ToValue((SqlBoolean) nullableValue);
            }
            if (nullableValue is SqlString)
            {
                return ToValue((SqlString) nullableValue);
            }
            if (!(nullableValue is SqlDateTime))
            {
                throw new Exception("Unsupported INullable type: {0}".FormatWith(CultureInfo.InvariantCulture, new object[] { nullableValue.GetType() }));
            }
            return ToValue((SqlDateTime) nullableValue);
        }

        public static bool TryConvert<T>(object initialValue, out T convertedValue)
        {
            return TryConvert<T>(initialValue, CultureInfo.CurrentCulture, out convertedValue);
        }

        public static bool TryConvert<T>(object initialValue, CultureInfo culture, out T convertedValue)
        {
            return MiscellaneousUtils.TryAction<T>(delegate {
                object obj2;
                TryConvert(initialValue, CultureInfo.CurrentCulture, typeof(T), out obj2);
                return (T) obj2;
            }, out convertedValue);
        }

        public static bool TryConvert(object initialValue, CultureInfo culture, Type targetType, out object convertedValue)
        {
            return MiscellaneousUtils.TryAction<object>(() => Convert(initialValue, culture, targetType), out convertedValue);
        }

        public static bool TryConvertOrCast<T>(object initialValue, out T convertedValue)
        {
            return TryConvertOrCast<T>(initialValue, CultureInfo.CurrentCulture, out convertedValue);
        }

        public static bool TryConvertOrCast<T>(object initialValue, CultureInfo culture, out T convertedValue)
        {
            return MiscellaneousUtils.TryAction<T>(delegate {
                object obj2;
                TryConvertOrCast(initialValue, CultureInfo.CurrentCulture, typeof(T), out obj2);
                return (T) obj2;
            }, out convertedValue);
        }

        public static bool TryConvertOrCast(object initialValue, CultureInfo culture, Type targetType, out object convertedValue)
        {
            return MiscellaneousUtils.TryAction<object>(() => ConvertOrCast(initialValue, culture, targetType), out convertedValue);
        }

        [StructLayout(LayoutKind.Sequential)]
        internal struct TypeConvertKey : IEquatable<ConvertUtils.TypeConvertKey>
        {
            private readonly Type _initialType;
            private readonly Type _targetType;
            public Type InitialType
            {
                get
                {
                    return this._initialType;
                }
            }
            public Type TargetType
            {
                get
                {
                    return this._targetType;
                }
            }
            public TypeConvertKey(Type initialType, Type targetType)
            {
                this._initialType = initialType;
                this._targetType = targetType;
            }

            public override int GetHashCode()
            {
                return (this._initialType.GetHashCode() ^ this._targetType.GetHashCode());
            }

            public override bool Equals(object obj)
            {
                return ((obj is ConvertUtils.TypeConvertKey) && this.Equals((ConvertUtils.TypeConvertKey) obj));
            }

            public bool Equals(ConvertUtils.TypeConvertKey other)
            {
                return ((this._initialType == other._initialType) && (this._targetType == other._targetType));
            }
        }
    }
}

