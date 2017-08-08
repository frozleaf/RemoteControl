namespace Newtonsoft.Json.Converters
{
    using Newtonsoft.Json;
    using Newtonsoft.Json.Utilities;
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Reflection;
    using System.Runtime.CompilerServices;
    using System.Runtime.Serialization;

    public class StringEnumConverter : JsonConverter
    {
        private readonly Dictionary<Type, BidirectionalDictionary<string, string>> _enumMemberNamesPerType = new Dictionary<Type, BidirectionalDictionary<string, string>>();

        public override bool CanConvert(Type objectType)
        {
            Type type = ReflectionUtils.IsNullableType(objectType) ? Nullable.GetUnderlyingType(objectType) : objectType;
            return type.IsEnum;
        }

        private BidirectionalDictionary<string, string> GetEnumNameMap(Type t)
        {
            BidirectionalDictionary<string, string> dictionary;
            if (!this._enumMemberNamesPerType.TryGetValue(t, out dictionary))
            {
                lock (this._enumMemberNamesPerType)
                {
                    if (this._enumMemberNamesPerType.TryGetValue(t, out dictionary))
                    {
                        return dictionary;
                    }
                    dictionary = new BidirectionalDictionary<string, string>(StringComparer.OrdinalIgnoreCase, StringComparer.OrdinalIgnoreCase);
                    foreach (FieldInfo info in t.GetFields())
                    {
                        string str3;
                        string name = info.Name;
                        string second = (from a in info.GetCustomAttributes(typeof(EnumMemberAttribute), true).Cast<EnumMemberAttribute>() select a.Value).SingleOrDefault<string>() ?? info.Name;
                        if (dictionary.TryGetBySecond(second, out str3))
                        {
                            throw new Exception("Enum name '{0}' already exists on enum '{1}'.".FormatWith(CultureInfo.InvariantCulture, new object[] { second, t.Name }));
                        }
                        dictionary.Add(name, second);
                    }
                    this._enumMemberNamesPerType[t] = dictionary;
                }
            }
            return dictionary;
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            Type t = ReflectionUtils.IsNullableType(objectType) ? Nullable.GetUnderlyingType(objectType) : objectType;
            if (reader.TokenType == JsonToken.Null)
            {
                if (!ReflectionUtils.IsNullableType(objectType))
                {
                    throw new Exception("Cannot convert null value to {0}.".FormatWith(CultureInfo.InvariantCulture, new object[] { objectType }));
                }
                return null;
            }
            if (reader.TokenType == JsonToken.String)
            {
                string str;
                this.GetEnumNameMap(t).TryGetBySecond(reader.Value.ToString(), out str);
                str = str ?? reader.Value.ToString();
                return Enum.Parse(t, str, true);
            }
            if (reader.TokenType != JsonToken.Integer)
            {
                throw new Exception("Unexpected token when parsing enum. Expected String or Integer, got {0}.".FormatWith(CultureInfo.InvariantCulture, new object[] { reader.TokenType }));
            }
            return ConvertUtils.ConvertOrCast(reader.Value, CultureInfo.InvariantCulture, t);
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            if (value == null)
            {
                writer.WriteNull();
            }
            else
            {
                Enum enum2 = (Enum) value;
                string first = enum2.ToString("G");
                if (char.IsNumber(first[0]) || (first[0] == '-'))
                {
                    writer.WriteValue(value);
                }
                else
                {
                    string str2;
                    this.GetEnumNameMap(enum2.GetType()).TryGetByFirst(first, out str2);
                    str2 = str2 ?? first;
                    if (this.CamelCaseText)
                    {
                        str2 = StringUtils.ToCamelCase(str2);
                    }
                    writer.WriteValue(str2);
                }
            }
        }

        public bool CamelCaseText { get; set; }
    }
}

