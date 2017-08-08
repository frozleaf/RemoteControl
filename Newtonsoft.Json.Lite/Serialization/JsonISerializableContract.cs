namespace Newtonsoft.Json.Serialization
{
    using System;
    using System.Runtime.CompilerServices;

    public class JsonISerializableContract : JsonContract
    {
        public JsonISerializableContract(Type underlyingType) : base(underlyingType)
        {
        }

        public ObjectConstructor<object> ISerializableCreator { get; set; }
    }
}

