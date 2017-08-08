namespace Newtonsoft.Json
{
    using Newtonsoft.Json.Converters;
    using Newtonsoft.Json.Utilities;
    using System;
    using System.Globalization;
    using System.IO;
    using System.Text;
    using System.Xml;
    using System.Xml.Linq;

    public static class JsonConvert
    {
        public static readonly string False = "false";
        internal static readonly long InitialJavaScriptDateTicks = 0x89f7ff5f7b58000L;
        public static readonly string NaN = "NaN";
        public static readonly string NegativeInfinity = "-Infinity";
        public static readonly string Null = "null";
        public static readonly string PositiveInfinity = "Infinity";
        public static readonly string True = "true";
        public static readonly string Undefined = "undefined";

        internal static long ConvertDateTimeToJavaScriptTicks(DateTime dateTime)
        {
            return ConvertDateTimeToJavaScriptTicks(dateTime, true);
        }

        internal static long ConvertDateTimeToJavaScriptTicks(DateTime dateTime, bool convertToUtc)
        {
            long universialTicks = convertToUtc ? ToUniversalTicks(dateTime) : dateTime.Ticks;
            return UniversialTicksToJavaScriptTicks(universialTicks);
        }

        internal static long ConvertDateTimeToJavaScriptTicks(DateTime dateTime, TimeSpan offset)
        {
            return UniversialTicksToJavaScriptTicks(ToUniversalTicks(dateTime, offset));
        }

        internal static DateTime ConvertJavaScriptTicksToDateTime(long javaScriptTicks)
        {
            return new DateTime((javaScriptTicks * 0x2710L) + InitialJavaScriptDateTicks, DateTimeKind.Utc);
        }

        public static T DeserializeAnonymousType<T>(string value, T anonymousTypeObject)
        {
            return DeserializeObject<T>(value);
        }

        public static object DeserializeObject(string value)
        {
            return DeserializeObject(value, null, (JsonSerializerSettings) null);
        }

        public static T DeserializeObject<T>(string value)
        {
            return DeserializeObject<T>(value, (JsonSerializerSettings) null);
        }

        public static object DeserializeObject(string value, JsonSerializerSettings settings)
        {
            return DeserializeObject(value, null, settings);
        }

        public static object DeserializeObject(string value, Type type)
        {
            return DeserializeObject(value, type, (JsonSerializerSettings) null);
        }

        public static T DeserializeObject<T>(string value, params JsonConverter[] converters)
        {
            return (T) DeserializeObject(value, typeof(T), converters);
        }

        public static T DeserializeObject<T>(string value, JsonSerializerSettings settings)
        {
            return (T) DeserializeObject(value, typeof(T), settings);
        }

        public static object DeserializeObject(string value, Type type, params JsonConverter[] converters)
        {
            JsonSerializerSettings settings = ((converters != null) && (converters.Length > 0)) ? new JsonSerializerSettings() : null;
            return DeserializeObject(value, type, settings);
        }

        public static object DeserializeObject(string value, Type type, JsonSerializerSettings settings)
        {
            object obj2;
            StringReader reader = new StringReader(value);
            JsonSerializer serializer = JsonSerializer.Create(settings);
            using (JsonReader reader2 = new JsonTextReader(reader))
            {
                obj2 = serializer.Deserialize(reader2, type);
                if (reader2.Read() && (reader2.TokenType != JsonToken.Comment))
                {
                    throw new JsonSerializationException("Additional text found in JSON string after finishing deserializing object.");
                }
            }
            return obj2;
        }

        private static string EnsureDecimalPlace(string text)
        {
            if (text.IndexOf('.') != -1)
            {
                return text;
            }
            return (text + ".0");
        }

        private static string EnsureDecimalPlace(double value, string text)
        {
            if (((double.IsNaN(value) || double.IsInfinity(value)) || (text.IndexOf('.') != -1)) || (text.IndexOf('E') != -1))
            {
                return text;
            }
            return (text + ".0");
        }

        private static TimeSpan GetUtcOffset(DateTime dateTime)
        {
            return TimeZone.CurrentTimeZone.GetUtcOffset(dateTime);
        }

        internal static bool IsJsonPrimitive(object value)
        {
            if (value == null)
            {
                return true;
            }
            IConvertible convertible = value as IConvertible;
            if (convertible != null)
            {
                return IsJsonPrimitiveTypeCode(convertible.GetTypeCode());
            }
            return ((value is DateTimeOffset) || (value is byte[]));
        }

        internal static bool IsJsonPrimitiveType(Type type)
        {
            if (ReflectionUtils.IsNullableType(type))
            {
                type = Nullable.GetUnderlyingType(type);
            }
            return ((type == typeof(DateTimeOffset)) || ((type == typeof(byte[])) || IsJsonPrimitiveTypeCode(Type.GetTypeCode(type))));
        }

        private static bool IsJsonPrimitiveTypeCode(TypeCode typeCode)
        {
            switch (typeCode)
            {
                case TypeCode.DBNull:
                case TypeCode.Boolean:
                case TypeCode.Char:
                case TypeCode.SByte:
                case TypeCode.Byte:
                case TypeCode.Int16:
                case TypeCode.UInt16:
                case TypeCode.Int32:
                case TypeCode.UInt32:
                case TypeCode.Int64:
                case TypeCode.UInt64:
                case TypeCode.Single:
                case TypeCode.Double:
                case TypeCode.Decimal:
                case TypeCode.DateTime:
                case TypeCode.String:
                    return true;
            }
            return false;
        }

        public static void PopulateObject(string value, object target)
        {
            PopulateObject(value, target, null);
        }

        public static void PopulateObject(string value, object target, JsonSerializerSettings settings)
        {
            StringReader reader = new StringReader(value);
            JsonSerializer serializer = JsonSerializer.Create(settings);
            using (JsonReader reader2 = new JsonTextReader(reader))
            {
                serializer.Populate(reader2, target);
                if (reader2.Read() && (reader2.TokenType != JsonToken.Comment))
                {
                    throw new JsonSerializationException("Additional text found in JSON string after finishing deserializing object.");
                }
            }
        }

        public static string SerializeObject(object value)
        {
            return SerializeObject(value, Newtonsoft.Json.Formatting.None, (JsonSerializerSettings) null);
        }

        public static string SerializeObject(object value, Newtonsoft.Json.Formatting formatting)
        {
            return SerializeObject(value, formatting, (JsonSerializerSettings) null);
        }

        public static string SerializeObject(object value, params JsonConverter[] converters)
        {
            return SerializeObject(value, Newtonsoft.Json.Formatting.None, converters);
        }

        public static string SerializeObject(object value, Newtonsoft.Json.Formatting formatting, params JsonConverter[] converters)
        {
            JsonSerializerSettings settings = ((converters != null) && (converters.Length > 0)) ? new JsonSerializerSettings() : null;
            return SerializeObject(value, formatting, settings);
        }

        public static string SerializeObject(object value, Newtonsoft.Json.Formatting formatting, JsonSerializerSettings settings)
        {
            JsonSerializer serializer = JsonSerializer.Create(settings);
            StringBuilder sb = new StringBuilder(0x80);
            StringWriter textWriter = new StringWriter(sb, CultureInfo.InvariantCulture);
            using (JsonTextWriter writer2 = new JsonTextWriter(textWriter))
            {
                writer2.Formatting = formatting;
                serializer.Serialize(writer2, value);
            }
            return textWriter.ToString();
        }

        public static string ToString(bool value)
        {
            return (value ? True : False);
        }

        public static string ToString(byte value)
        {
            return value.ToString(null, CultureInfo.InvariantCulture);
        }

        public static string ToString(char value)
        {
            return ToString(char.ToString(value));
        }

        public static string ToString(DateTime value)
        {
            using (StringWriter writer = StringUtils.CreateStringWriter(0x40))
            {
                WriteDateTimeString(writer, value, GetUtcOffset(value), value.Kind);
                return writer.ToString();
            }
        }

        public static string ToString(DateTimeOffset value)
        {
            using (StringWriter writer = StringUtils.CreateStringWriter(0x40))
            {
                WriteDateTimeString(writer, value.UtcDateTime, value.Offset, DateTimeKind.Local);
                return writer.ToString();
            }
        }

        public static string ToString(decimal value)
        {
            return EnsureDecimalPlace(value.ToString(null, CultureInfo.InvariantCulture));
        }

        public static string ToString(double value)
        {
            return EnsureDecimalPlace(value, value.ToString("R", CultureInfo.InvariantCulture));
        }

        public static string ToString(Enum value)
        {
            return value.ToString("D");
        }

        public static string ToString(Guid value)
        {
            return ('"' + value.ToString("D", CultureInfo.InvariantCulture) + '"');
        }

        public static string ToString(short value)
        {
            return value.ToString(null, CultureInfo.InvariantCulture);
        }

        public static string ToString(int value)
        {
            return value.ToString(null, CultureInfo.InvariantCulture);
        }

        public static string ToString(long value)
        {
            return value.ToString(null, CultureInfo.InvariantCulture);
        }

        public static string ToString(object value)
        {
            if (value == null)
            {
                return Null;
            }
            IConvertible convertible = value as IConvertible;
            if (convertible != null)
            {
                switch (convertible.GetTypeCode())
                {
                    case TypeCode.DBNull:
                        return Null;

                    case TypeCode.Boolean:
                        return ToString(convertible.ToBoolean(CultureInfo.InvariantCulture));

                    case TypeCode.Char:
                        return ToString(convertible.ToChar(CultureInfo.InvariantCulture));

                    case TypeCode.SByte:
                        return ToString(convertible.ToSByte(CultureInfo.InvariantCulture));

                    case TypeCode.Byte:
                        return ToString(convertible.ToByte(CultureInfo.InvariantCulture));

                    case TypeCode.Int16:
                        return ToString(convertible.ToInt16(CultureInfo.InvariantCulture));

                    case TypeCode.UInt16:
                        return ToString(convertible.ToUInt16(CultureInfo.InvariantCulture));

                    case TypeCode.Int32:
                        return ToString(convertible.ToInt32(CultureInfo.InvariantCulture));

                    case TypeCode.UInt32:
                        return ToString(convertible.ToUInt32(CultureInfo.InvariantCulture));

                    case TypeCode.Int64:
                        return ToString(convertible.ToInt64(CultureInfo.InvariantCulture));

                    case TypeCode.UInt64:
                        return ToString(convertible.ToUInt64(CultureInfo.InvariantCulture));

                    case TypeCode.Single:
                        return ToString(convertible.ToSingle(CultureInfo.InvariantCulture));

                    case TypeCode.Double:
                        return ToString(convertible.ToDouble(CultureInfo.InvariantCulture));

                    case TypeCode.Decimal:
                        return ToString(convertible.ToDecimal(CultureInfo.InvariantCulture));

                    case TypeCode.DateTime:
                        return ToString(convertible.ToDateTime(CultureInfo.InvariantCulture));

                    case TypeCode.String:
                        return ToString(convertible.ToString(CultureInfo.InvariantCulture));
                }
            }
            else if (value is DateTimeOffset)
            {
                return ToString((DateTimeOffset) value);
            }
            throw new ArgumentException("Unsupported type: {0}. Use the JsonSerializer class to get the object's JSON representation.".FormatWith(CultureInfo.InvariantCulture, new object[] { value.GetType() }));
        }

        [CLSCompliant(false)]
        public static string ToString(sbyte value)
        {
            return value.ToString(null, CultureInfo.InvariantCulture);
        }

        public static string ToString(float value)
        {
            return EnsureDecimalPlace((double) value, value.ToString("R", CultureInfo.InvariantCulture));
        }

        public static string ToString(string value)
        {
            return ToString(value, '"');
        }

        [CLSCompliant(false)]
        public static string ToString(ushort value)
        {
            return value.ToString(null, CultureInfo.InvariantCulture);
        }

        [CLSCompliant(false)]
        public static string ToString(uint value)
        {
            return value.ToString(null, CultureInfo.InvariantCulture);
        }

        [CLSCompliant(false)]
        public static string ToString(ulong value)
        {
            return value.ToString(null, CultureInfo.InvariantCulture);
        }

        public static string ToString(string value, char delimter)
        {
            return JavaScriptUtils.ToEscapedJavaScriptString(value, delimter, true);
        }

        private static long ToUniversalTicks(DateTime dateTime)
        {
            if (dateTime.Kind == DateTimeKind.Utc)
            {
                return dateTime.Ticks;
            }
            return ToUniversalTicks(dateTime, GetUtcOffset(dateTime));
        }

        private static long ToUniversalTicks(DateTime dateTime, TimeSpan offset)
        {
            if (dateTime.Kind == DateTimeKind.Utc)
            {
                return dateTime.Ticks;
            }
            long num = dateTime.Ticks - offset.Ticks;
            if (num > 0x2bca2875f4373fffL)
            {
                return 0x2bca2875f4373fffL;
            }
            if (num < 0L)
            {
                return 0L;
            }
            return num;
        }

        private static long UniversialTicksToJavaScriptTicks(long universialTicks)
        {
            return ((universialTicks - InitialJavaScriptDateTicks) / 0x2710L);
        }

        internal static void WriteDateTimeString(TextWriter writer, DateTime value)
        {
            WriteDateTimeString(writer, value, GetUtcOffset(value), value.Kind);
        }

        internal static void WriteDateTimeString(TextWriter writer, DateTime value, TimeSpan offset, DateTimeKind kind)
        {
            long num = ConvertDateTimeToJavaScriptTicks(value, offset);
            writer.Write("\"\\/Date(");
            writer.Write(num);
            switch (kind)
            {
                case DateTimeKind.Unspecified:
                case DateTimeKind.Local:
                {
                    writer.Write((offset.Ticks >= 0L) ? "+" : "-");
                    int num2 = Math.Abs(offset.Hours);
                    if (num2 < 10)
                    {
                        writer.Write(0);
                    }
                    writer.Write(num2);
                    int num3 = Math.Abs(offset.Minutes);
                    if (num3 < 10)
                    {
                        writer.Write(0);
                    }
                    writer.Write(num3);
                    break;
                }
            }
            writer.Write(")\\/\"");
        }
    }
}

