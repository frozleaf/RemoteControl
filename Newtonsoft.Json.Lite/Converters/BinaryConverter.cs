namespace Newtonsoft.Json.Converters
{
    using Newtonsoft.Json;
    using Newtonsoft.Json.Utilities;
    using System;
    using System.Data.SqlTypes;
    using System.Globalization;

    public class BinaryConverter : JsonConverter
    {
        private const string BinaryTypeName = "System.Data.Linq.Binary";

        public override bool CanConvert(Type objectType)
        {
            return (objectType.AssignableToTypeName("System.Data.Linq.Binary") || ((objectType == typeof(SqlBinary)) || (objectType == typeof(SqlBinary?))));
        }

        private byte[] GetByteArray(object value)
        {
            if (value.GetType().AssignableToTypeName("System.Data.Linq.Binary"))
            {
                return DynamicWrapper.CreateWrapper<IBinary>(value).ToArray();
            }
            if (!(value is SqlBinary))
            {
                throw new Exception("Unexpected value type when writing binary: {0}".FormatWith(CultureInfo.InvariantCulture, new object[] { value.GetType() }));
            }
            SqlBinary binary2 = (SqlBinary) value;
            return binary2.Value;
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            Type type = ReflectionUtils.IsNullableType(objectType) ? Nullable.GetUnderlyingType(objectType) : objectType;
            if (reader.TokenType == JsonToken.Null)
            {
                if (!ReflectionUtils.IsNullable(objectType))
                {
                    throw new Exception("Cannot convert null value to {0}.".FormatWith(CultureInfo.InvariantCulture, new object[] { objectType }));
                }
                return null;
            }
            if (reader.TokenType != JsonToken.String)
            {
                throw new Exception("Unexpected token parsing binary. Expected String, got {0}.".FormatWith(CultureInfo.InvariantCulture, new object[] { reader.TokenType }));
            }
            byte[] buffer = Convert.FromBase64String(reader.Value.ToString());
            if (type.AssignableToTypeName("System.Data.Linq.Binary"))
            {
                return Activator.CreateInstance(type, new object[] { buffer });
            }
            if (type != typeof(SqlBinary))
            {
                throw new Exception("Unexpected object type when writing binary: {0}".FormatWith(CultureInfo.InvariantCulture, new object[] { objectType }));
            }
            return new SqlBinary(buffer);
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            if (value == null)
            {
                writer.WriteNull();
            }
            else
            {
                byte[] byteArray = this.GetByteArray(value);
                writer.WriteValue(byteArray);
            }
        }
    }
}

