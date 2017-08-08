namespace Newtonsoft.Json.Serialization
{
    using Newtonsoft.Json;
    using Newtonsoft.Json.Utilities;
    using System;
    using System.Reflection;
    using System.Runtime.CompilerServices;
    using System.Runtime.Serialization;

    public abstract class JsonContract
    {
        internal JsonContract(Type underlyingType)
        {
            ValidationUtils.ArgumentNotNull(underlyingType, "underlyingType");
            this.UnderlyingType = underlyingType;
            this.CreatedType = underlyingType;
        }

        internal void InvokeOnDeserialized(object o, StreamingContext context)
        {
            if (this.OnDeserialized != null)
            {
                this.OnDeserialized.Invoke(o, new object[] { context });
            }
        }

        internal void InvokeOnDeserializing(object o, StreamingContext context)
        {
            if (this.OnDeserializing != null)
            {
                this.OnDeserializing.Invoke(o, new object[] { context });
            }
        }

        internal void InvokeOnError(object o, StreamingContext context, ErrorContext errorContext)
        {
            if (this.OnError != null)
            {
                this.OnError.Invoke(o, new object[] { context, errorContext });
            }
        }

        internal void InvokeOnSerialized(object o, StreamingContext context)
        {
            if (this.OnSerialized != null)
            {
                this.OnSerialized.Invoke(o, new object[] { context });
            }
        }

        internal void InvokeOnSerializing(object o, StreamingContext context)
        {
            if (this.OnSerializing != null)
            {
                this.OnSerializing.Invoke(o, new object[] { context });
            }
        }

        public JsonConverter Converter { get; set; }

        public Type CreatedType { get; set; }

        public Func<object> DefaultCreator { get; set; }

        public bool DefaultCreatorNonPublic { get; set; }

        internal JsonConverter InternalConverter { get; set; }

        public bool? IsReference { get; set; }

        public MethodInfo OnDeserialized { get; set; }

        public MethodInfo OnDeserializing { get; set; }

        public MethodInfo OnError { get; set; }

        public MethodInfo OnSerialized { get; set; }

        public MethodInfo OnSerializing { get; set; }

        public Type UnderlyingType { get; private set; }
    }
}

