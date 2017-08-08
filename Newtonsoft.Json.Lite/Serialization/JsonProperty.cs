namespace Newtonsoft.Json.Serialization
{
    using Newtonsoft.Json;
    using System;
    using System.Runtime.CompilerServices;

    public class JsonProperty
    {
        public override string ToString()
        {
            return this.PropertyName;
        }

        public JsonConverter Converter { get; set; }

        public object DefaultValue { get; set; }

        public Newtonsoft.Json.DefaultValueHandling? DefaultValueHandling { get; set; }

        public Predicate<object> GetIsSpecified { get; set; }

        public bool Ignored { get; set; }

        public bool? IsReference { get; set; }

        public JsonConverter MemberConverter { get; set; }

        public Newtonsoft.Json.NullValueHandling? NullValueHandling { get; set; }

        public Newtonsoft.Json.ObjectCreationHandling? ObjectCreationHandling { get; set; }

        public string PropertyName { get; set; }

        public Type PropertyType { get; set; }

        public bool Readable { get; set; }

        public Newtonsoft.Json.ReferenceLoopHandling? ReferenceLoopHandling { get; set; }

        public Newtonsoft.Json.Required Required { get; set; }

        public Action<object, object> SetIsSpecified { get; set; }

        public Predicate<object> ShouldSerialize { get; set; }

        public Newtonsoft.Json.TypeNameHandling? TypeNameHandling { get; set; }

        public string UnderlyingName { get; set; }

        public IValueProvider ValueProvider { get; set; }

        public bool Writable { get; set; }
    }
}

