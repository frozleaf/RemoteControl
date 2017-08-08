namespace Newtonsoft.Json
{
    using Newtonsoft.Json.Schema;
    using System;

    public abstract class JsonConverter
    {
        protected JsonConverter()
        {
        }

        public abstract bool CanConvert(Type objectType);
        public virtual JsonSchema GetSchema()
        {
            return null;
        }

        public abstract object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer);
        public abstract void WriteJson(JsonWriter writer, object value, JsonSerializer serializer);

        public virtual bool CanRead
        {
            get
            {
                return true;
            }
        }

        public virtual bool CanWrite
        {
            get
            {
                return true;
            }
        }
    }
}

