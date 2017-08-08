namespace Newtonsoft.Json
{
    using System;
    using System.Runtime.CompilerServices;

    [AttributeUsage(AttributeTargets.Parameter | AttributeTargets.Field | AttributeTargets.Property, AllowMultiple=false)]
    public sealed class JsonPropertyAttribute : Attribute
    {
        internal Newtonsoft.Json.DefaultValueHandling? _defaultValueHandling;
        internal bool? _isReference;
        internal Newtonsoft.Json.NullValueHandling? _nullValueHandling;
        internal Newtonsoft.Json.ObjectCreationHandling? _objectCreationHandling;
        internal Newtonsoft.Json.ReferenceLoopHandling? _referenceLoopHandling;
        internal Newtonsoft.Json.TypeNameHandling? _typeNameHandling;

        public JsonPropertyAttribute()
        {
        }

        public JsonPropertyAttribute(string propertyName)
        {
            this.PropertyName = propertyName;
        }

        public Newtonsoft.Json.DefaultValueHandling DefaultValueHandling
        {
            get
            {
                Newtonsoft.Json.DefaultValueHandling? nullable = this._defaultValueHandling;
                return (nullable.HasValue ? nullable.GetValueOrDefault() : Newtonsoft.Json.DefaultValueHandling.Include);
            }
            set
            {
                this._defaultValueHandling = new Newtonsoft.Json.DefaultValueHandling?(value);
            }
        }

        public bool IsReference
        {
            get
            {
                bool? nullable = this._isReference;
                return (nullable.HasValue ? nullable.GetValueOrDefault() : false);
            }
            set
            {
                this._isReference = new bool?(value);
            }
        }

        public Newtonsoft.Json.NullValueHandling NullValueHandling
        {
            get
            {
                Newtonsoft.Json.NullValueHandling? nullable = this._nullValueHandling;
                return (nullable.HasValue ? nullable.GetValueOrDefault() : Newtonsoft.Json.NullValueHandling.Include);
            }
            set
            {
                this._nullValueHandling = new Newtonsoft.Json.NullValueHandling?(value);
            }
        }

        public Newtonsoft.Json.ObjectCreationHandling ObjectCreationHandling
        {
            get
            {
                Newtonsoft.Json.ObjectCreationHandling? nullable = this._objectCreationHandling;
                return (nullable.HasValue ? nullable.GetValueOrDefault() : Newtonsoft.Json.ObjectCreationHandling.Auto);
            }
            set
            {
                this._objectCreationHandling = new Newtonsoft.Json.ObjectCreationHandling?(value);
            }
        }

        public string PropertyName { get; set; }

        public Newtonsoft.Json.ReferenceLoopHandling ReferenceLoopHandling
        {
            get
            {
                Newtonsoft.Json.ReferenceLoopHandling? nullable = this._referenceLoopHandling;
                return (nullable.HasValue ? nullable.GetValueOrDefault() : Newtonsoft.Json.ReferenceLoopHandling.Error);
            }
            set
            {
                this._referenceLoopHandling = new Newtonsoft.Json.ReferenceLoopHandling?(value);
            }
        }

        public Newtonsoft.Json.Required Required { get; set; }

        public Newtonsoft.Json.TypeNameHandling TypeNameHandling
        {
            get
            {
                Newtonsoft.Json.TypeNameHandling? nullable = this._typeNameHandling;
                return (nullable.HasValue ? nullable.GetValueOrDefault() : Newtonsoft.Json.TypeNameHandling.None);
            }
            set
            {
                this._typeNameHandling = new Newtonsoft.Json.TypeNameHandling?(value);
            }
        }
    }
}

