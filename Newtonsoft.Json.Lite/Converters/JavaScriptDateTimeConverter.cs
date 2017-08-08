namespace Newtonsoft.Json.Converters
{
    using Newtonsoft.Json;
    using Newtonsoft.Json.Utilities;
    using System;
    using System.Globalization;

    public class JavaScriptDateTimeConverter : DateTimeConverterBase
    {
        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            Type type = ReflectionUtils.IsNullableType(objectType) ? Nullable.GetUnderlyingType(objectType) : objectType;
            if (reader.TokenType == JsonToken.Null)
            {
                if (!ReflectionUtils.IsNullableType(objectType))
                {
                    throw new Exception("Cannot convert null value to {0}.".FormatWith(CultureInfo.InvariantCulture, new object[] { objectType }));
                }
                return null;
            }
            if ((reader.TokenType != JsonToken.StartConstructor) || (string.Compare(reader.Value.ToString(), "Date", StringComparison.Ordinal) != 0))
            {
                throw new Exception("Unexpected token or value when parsing date. Token: {0}, Value: {1}".FormatWith(CultureInfo.InvariantCulture, new object[] { reader.TokenType, reader.Value }));
            }
            reader.Read();
            if (reader.TokenType != JsonToken.Integer)
            {
                throw new Exception("Unexpected token parsing date. Expected Integer, got {0}.".FormatWith(CultureInfo.InvariantCulture, new object[] { reader.TokenType }));
            }
            long javaScriptTicks = (long) reader.Value;
            DateTime dateTime = JsonConvert.ConvertJavaScriptTicksToDateTime(javaScriptTicks);
            reader.Read();
            if (reader.TokenType != JsonToken.EndConstructor)
            {
                throw new Exception("Unexpected token parsing date. Expected EndConstructor, got {0}.".FormatWith(CultureInfo.InvariantCulture, new object[] { reader.TokenType }));
            }
            if (type == typeof(DateTimeOffset))
            {
                return new DateTimeOffset(dateTime);
            }
            return dateTime;
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            long num;
            if (value is DateTime)
            {
                num = JsonConvert.ConvertDateTimeToJavaScriptTicks(((DateTime) value).ToUniversalTime());
            }
            else
            {
                if (!(value is DateTimeOffset))
                {
                    throw new Exception("Expected date object value.");
                }
                DateTimeOffset offset = (DateTimeOffset) value;
                num = JsonConvert.ConvertDateTimeToJavaScriptTicks(offset.ToUniversalTime().UtcDateTime);
            }
            writer.WriteStartConstructor("Date");
            writer.WriteValue(num);
            writer.WriteEndConstructor();
        }
    }
}

