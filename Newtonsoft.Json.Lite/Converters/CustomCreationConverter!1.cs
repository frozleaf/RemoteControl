namespace Newtonsoft.Json.Converters
{
    using Newtonsoft.Json;
    using System;

    public abstract class CustomCreationConverter<T> : JsonConverter
    {
        protected CustomCreationConverter()
        {
        }

        public override bool CanConvert(Type objectType)
        {
            return typeof(T).IsAssignableFrom(objectType);
        }

        public abstract T Create(Type objectType);
        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Null)
            {
                return null;
            }
            T target = this.Create(objectType);
            if (target == null)
            {
                throw new JsonSerializationException("No object created.");
            }
            serializer.Populate(reader, target);
            return target;
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotSupportedException("CustomCreationConverter should only be used while deserializing.");
        }

        public override bool CanWrite
        {
            get
            {
                return false;
            }
        }
    }
}

