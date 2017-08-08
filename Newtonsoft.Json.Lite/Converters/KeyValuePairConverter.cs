namespace Newtonsoft.Json.Converters
{
    using Newtonsoft.Json;
    using Newtonsoft.Json.Utilities;
    using System;
    using System.Collections.Generic;
    using System.Reflection;

    public class KeyValuePairConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return ((objectType.IsValueType && objectType.IsGenericType) && (objectType.GetGenericTypeDefinition() == typeof(KeyValuePair<,>)));
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            IList<Type> genericArguments = objectType.GetGenericArguments();
            Type type = genericArguments[0];
            Type type2 = genericArguments[1];
            reader.Read();
            reader.Read();
            object obj2 = serializer.Deserialize(reader, type);
            reader.Read();
            reader.Read();
            object obj3 = serializer.Deserialize(reader, type2);
            reader.Read();
            return ReflectionUtils.CreateInstance(objectType, new object[] { obj2, obj3 });
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            Type type = value.GetType();
            PropertyInfo property = type.GetProperty("Key");
            PropertyInfo member = type.GetProperty("Value");
            writer.WriteStartObject();
            writer.WritePropertyName("Key");
            serializer.Serialize(writer, ReflectionUtils.GetMemberValue(property, value));
            writer.WritePropertyName("Value");
            serializer.Serialize(writer, ReflectionUtils.GetMemberValue(member, value));
            writer.WriteEndObject();
        }
    }
}

