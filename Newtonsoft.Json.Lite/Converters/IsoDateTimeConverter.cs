namespace Newtonsoft.Json.Converters
{
    using Newtonsoft.Json;
    using Newtonsoft.Json.Utilities;
    using System;
    using System.Globalization;

    public class IsoDateTimeConverter : DateTimeConverterBase
    {
        private CultureInfo _culture;
        private string _dateTimeFormat;
        private System.Globalization.DateTimeStyles _dateTimeStyles = System.Globalization.DateTimeStyles.RoundtripKind;
        private const string DefaultDateTimeFormat = "yyyy'-'MM'-'dd'T'HH':'mm':'ss.FFFFFFFK";

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            bool flag = ReflectionUtils.IsNullableType(objectType);
            Type type = flag ? Nullable.GetUnderlyingType(objectType) : objectType;
            if (reader.TokenType == JsonToken.Null)
            {
                if (!ReflectionUtils.IsNullableType(objectType))
                {
                    throw new Exception("Cannot convert null value to {0}.".FormatWith(CultureInfo.InvariantCulture, new object[] { objectType }));
                }
                return null;
            }
            if (reader.TokenType != JsonToken.String)
            {
                throw new Exception("Unexpected token parsing date. Expected String, got {0}.".FormatWith(CultureInfo.InvariantCulture, new object[] { reader.TokenType }));
            }
            string str = reader.Value.ToString();
            if (string.IsNullOrEmpty(str) && flag)
            {
                return null;
            }
            if (type == typeof(DateTimeOffset))
            {
                if (!string.IsNullOrEmpty(this._dateTimeFormat))
                {
                    return DateTimeOffset.ParseExact(str, this._dateTimeFormat, this.Culture, this._dateTimeStyles);
                }
                return DateTimeOffset.Parse(str, this.Culture, this._dateTimeStyles);
            }
            if (!string.IsNullOrEmpty(this._dateTimeFormat))
            {
                return DateTime.ParseExact(str, this._dateTimeFormat, this.Culture, this._dateTimeStyles);
            }
            return DateTime.Parse(str, this.Culture, this._dateTimeStyles);
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            string str;
            if (value is DateTime)
            {
                DateTime time = (DateTime) value;
                if (((this._dateTimeStyles & System.Globalization.DateTimeStyles.AdjustToUniversal) == System.Globalization.DateTimeStyles.AdjustToUniversal) || ((this._dateTimeStyles & System.Globalization.DateTimeStyles.AssumeUniversal) == System.Globalization.DateTimeStyles.AssumeUniversal))
                {
                    time = time.ToUniversalTime();
                }
                str = time.ToString(this._dateTimeFormat ?? "yyyy'-'MM'-'dd'T'HH':'mm':'ss.FFFFFFFK", this.Culture);
            }
            else
            {
                if (!(value is DateTimeOffset))
                {
                    throw new Exception("Unexpected value when converting date. Expected DateTime or DateTimeOffset, got {0}.".FormatWith(CultureInfo.InvariantCulture, new object[] { ReflectionUtils.GetObjectType(value) }));
                }
                DateTimeOffset offset = (DateTimeOffset) value;
                if (((this._dateTimeStyles & System.Globalization.DateTimeStyles.AdjustToUniversal) == System.Globalization.DateTimeStyles.AdjustToUniversal) || ((this._dateTimeStyles & System.Globalization.DateTimeStyles.AssumeUniversal) == System.Globalization.DateTimeStyles.AssumeUniversal))
                {
                    offset = offset.ToUniversalTime();
                }
                str = offset.ToString(this._dateTimeFormat ?? "yyyy'-'MM'-'dd'T'HH':'mm':'ss.FFFFFFFK", this.Culture);
            }
            writer.WriteValue(str);
        }

        public CultureInfo Culture
        {
            get
            {
                return (this._culture ?? CultureInfo.CurrentCulture);
            }
            set
            {
                this._culture = value;
            }
        }

        public string DateTimeFormat
        {
            get
            {
                return (this._dateTimeFormat ?? string.Empty);
            }
            set
            {
                this._dateTimeFormat = StringUtils.NullEmptyString(value);
            }
        }

        public System.Globalization.DateTimeStyles DateTimeStyles
        {
            get
            {
                return this._dateTimeStyles;
            }
            set
            {
                this._dateTimeStyles = value;
            }
        }
    }
}

