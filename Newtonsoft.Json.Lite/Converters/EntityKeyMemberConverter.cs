namespace Newtonsoft.Json.Converters
{
    using Newtonsoft.Json;
    using Newtonsoft.Json.Serialization;
    using Newtonsoft.Json.Utilities;
    using System;
    using System.Globalization;

    public class EntityKeyMemberConverter : JsonConverter
    {
        private const string EntityKeyMemberFullTypeName = "System.Data.EntityKeyMember";

        public override bool CanConvert(Type objectType)
        {
            return objectType.AssignableToTypeName("System.Data.EntityKeyMember");
        }

        private static void ReadAndAssert(JsonReader reader)
        {
            if (!reader.Read())
            {
                throw new JsonSerializationException("Unexpected end.");
            }
        }

        private static void ReadAndAssertProperty(JsonReader reader, string propertyName)
        {
            ReadAndAssert(reader);
            if ((reader.TokenType != JsonToken.PropertyName) || (reader.Value.ToString() != propertyName))
            {
                throw new JsonSerializationException("Expected JSON property '{0}'.".FormatWith(CultureInfo.InvariantCulture, new object[] { propertyName }));
            }
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            IEntityKeyMember wrapper = DynamicWrapper.CreateWrapper<IEntityKeyMember>(Activator.CreateInstance(objectType));
            ReadAndAssertProperty(reader, "Key");
            ReadAndAssert(reader);
            wrapper.Key = reader.Value.ToString();
            ReadAndAssertProperty(reader, "Type");
            ReadAndAssert(reader);
            Type type = Type.GetType(reader.Value.ToString());
            ReadAndAssertProperty(reader, "Value");
            ReadAndAssert(reader);
            wrapper.Value = serializer.Deserialize(reader, type);
            ReadAndAssert(reader);
            return DynamicWrapper.GetUnderlyingObject(wrapper);
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            IEntityKeyMember member = DynamicWrapper.CreateWrapper<IEntityKeyMember>(value);
            Type type = (member.Value != null) ? member.Value.GetType() : null;
            writer.WriteStartObject();
            writer.WritePropertyName("Key");
            writer.WriteValue(member.Key);
            writer.WritePropertyName("Type");
            writer.WriteValue((type != null) ? type.FullName : null);
            writer.WritePropertyName("Value");
            if (type != null)
            {
                string str;
                if (JsonSerializerInternalWriter.TryConvertToString(member.Value, type, out str))
                {
                    writer.WriteValue(str);
                }
                else
                {
                    writer.WriteValue(member.Value);
                }
            }
            else
            {
                writer.WriteNull();
            }
            writer.WriteEndObject();
        }
    }
}

