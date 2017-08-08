namespace Newtonsoft.Json
{
    using Newtonsoft.Json.Serialization;
    using Newtonsoft.Json.Utilities;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Runtime.Serialization;
    using System.Runtime.Serialization.Formatters;
    using System.Threading;

    public class JsonSerializer
    {
        private SerializationBinder _binder = DefaultSerializationBinder.Instance;
        private Newtonsoft.Json.ConstructorHandling _constructorHandling = Newtonsoft.Json.ConstructorHandling.Default;
        private StreamingContext _context = JsonSerializerSettings.DefaultContext;
        private IContractResolver _contractResolver;
        private JsonConverterCollection _converters;
        private Newtonsoft.Json.DefaultValueHandling _defaultValueHandling = Newtonsoft.Json.DefaultValueHandling.Include;
        private Newtonsoft.Json.MissingMemberHandling _missingMemberHandling = Newtonsoft.Json.MissingMemberHandling.Ignore;
        private Newtonsoft.Json.NullValueHandling _nullValueHandling = Newtonsoft.Json.NullValueHandling.Include;
        private Newtonsoft.Json.ObjectCreationHandling _objectCreationHandling = Newtonsoft.Json.ObjectCreationHandling.Auto;
        private Newtonsoft.Json.PreserveReferencesHandling _preserveReferencesHandling = Newtonsoft.Json.PreserveReferencesHandling.None;
        private Newtonsoft.Json.ReferenceLoopHandling _referenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Error;
        private IReferenceResolver _referenceResolver;
        private FormatterAssemblyStyle _typeNameAssemblyFormat;
        private Newtonsoft.Json.TypeNameHandling _typeNameHandling = Newtonsoft.Json.TypeNameHandling.None;

        public event EventHandler<Newtonsoft.Json.Serialization.ErrorEventArgs> Error;

        public static JsonSerializer Create(JsonSerializerSettings settings)
        {
            JsonSerializer serializer = new JsonSerializer();
            if (settings != null)
            {
                if (!CollectionUtils.IsNullOrEmpty<JsonConverter>(settings.Converters))
                {
                    serializer.Converters.AddRange<JsonConverter>(settings.Converters);
                }
                serializer.TypeNameHandling = settings.TypeNameHandling;
                serializer.TypeNameAssemblyFormat = settings.TypeNameAssemblyFormat;
                serializer.PreserveReferencesHandling = settings.PreserveReferencesHandling;
                serializer.ReferenceLoopHandling = settings.ReferenceLoopHandling;
                serializer.MissingMemberHandling = settings.MissingMemberHandling;
                serializer.ObjectCreationHandling = settings.ObjectCreationHandling;
                serializer.NullValueHandling = settings.NullValueHandling;
                serializer.DefaultValueHandling = settings.DefaultValueHandling;
                serializer.ConstructorHandling = settings.ConstructorHandling;
                serializer.Context = settings.Context;
                if (settings.Error != null)
                {
                    serializer.Error += settings.Error;
                }
                if (settings.ContractResolver != null)
                {
                    serializer.ContractResolver = settings.ContractResolver;
                }
                if (settings.ReferenceResolver != null)
                {
                    serializer.ReferenceResolver = settings.ReferenceResolver;
                }
                if (settings.Binder != null)
                {
                    serializer.Binder = settings.Binder;
                }
            }
            return serializer;
        }

        public object Deserialize(JsonReader reader)
        {
            return this.Deserialize(reader, null);
        }

        public T Deserialize<T>(JsonReader reader)
        {
            return (T) this.Deserialize(reader, typeof(T));
        }

        public object Deserialize(JsonReader reader, Type objectType)
        {
            return this.DeserializeInternal(reader, objectType);
        }

        public object Deserialize(TextReader reader, Type objectType)
        {
            return this.Deserialize(new JsonTextReader(reader), objectType);
        }

        internal virtual object DeserializeInternal(JsonReader reader, Type objectType)
        {
            ValidationUtils.ArgumentNotNull(reader, "reader");
            JsonSerializerInternalReader reader2 = new JsonSerializerInternalReader(this);
            return reader2.Deserialize(reader, objectType);
        }

        internal JsonConverter GetMatchingConverter(Type type)
        {
            return GetMatchingConverter(this._converters, type);
        }

        internal static JsonConverter GetMatchingConverter(IList<JsonConverter> converters, Type objectType)
        {
            ValidationUtils.ArgumentNotNull(objectType, "objectType");
            if (converters != null)
            {
                for (int i = 0; i < converters.Count; i++)
                {
                    JsonConverter converter = converters[i];
                    if (converter.CanConvert(objectType))
                    {
                        return converter;
                    }
                }
            }
            return null;
        }

        internal void OnError(Newtonsoft.Json.Serialization.ErrorEventArgs e)
        {
            EventHandler<Newtonsoft.Json.Serialization.ErrorEventArgs> error = this.Error;
            if (error != null)
            {
                error(this, e);
            }
        }

        public void Populate(JsonReader reader, object target)
        {
            this.PopulateInternal(reader, target);
        }

        public void Populate(TextReader reader, object target)
        {
            this.Populate(new JsonTextReader(reader), target);
        }

        internal virtual void PopulateInternal(JsonReader reader, object target)
        {
            ValidationUtils.ArgumentNotNull(reader, "reader");
            ValidationUtils.ArgumentNotNull(target, "target");
            new JsonSerializerInternalReader(this).Populate(reader, target);
        }

        public void Serialize(JsonWriter jsonWriter, object value)
        {
            this.SerializeInternal(jsonWriter, value);
        }

        public void Serialize(TextWriter textWriter, object value)
        {
            this.Serialize(new JsonTextWriter(textWriter), value);
        }

        internal virtual void SerializeInternal(JsonWriter jsonWriter, object value)
        {
            ValidationUtils.ArgumentNotNull(jsonWriter, "jsonWriter");
            new JsonSerializerInternalWriter(this).Serialize(jsonWriter, value);
        }

        public virtual SerializationBinder Binder
        {
            get
            {
                return this._binder;
            }
            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException("value", "Serialization binder cannot be null.");
                }
                this._binder = value;
            }
        }

        public virtual Newtonsoft.Json.ConstructorHandling ConstructorHandling
        {
            get
            {
                return this._constructorHandling;
            }
            set
            {
                if ((value < Newtonsoft.Json.ConstructorHandling.Default) || (value > Newtonsoft.Json.ConstructorHandling.AllowNonPublicDefaultConstructor))
                {
                    throw new ArgumentOutOfRangeException("value");
                }
                this._constructorHandling = value;
            }
        }

        public virtual StreamingContext Context
        {
            get
            {
                return this._context;
            }
            set
            {
                this._context = value;
            }
        }

        public virtual IContractResolver ContractResolver
        {
            get
            {
                if (this._contractResolver == null)
                {
                    this._contractResolver = DefaultContractResolver.Instance;
                }
                return this._contractResolver;
            }
            set
            {
                this._contractResolver = value;
            }
        }

        public virtual JsonConverterCollection Converters
        {
            get
            {
                if (this._converters == null)
                {
                    this._converters = new JsonConverterCollection();
                }
                return this._converters;
            }
        }

        public virtual Newtonsoft.Json.DefaultValueHandling DefaultValueHandling
        {
            get
            {
                return this._defaultValueHandling;
            }
            set
            {
                if ((value < Newtonsoft.Json.DefaultValueHandling.Include) || (value > Newtonsoft.Json.DefaultValueHandling.Ignore))
                {
                    throw new ArgumentOutOfRangeException("value");
                }
                this._defaultValueHandling = value;
            }
        }

        public virtual Newtonsoft.Json.MissingMemberHandling MissingMemberHandling
        {
            get
            {
                return this._missingMemberHandling;
            }
            set
            {
                if ((value < Newtonsoft.Json.MissingMemberHandling.Ignore) || (value > Newtonsoft.Json.MissingMemberHandling.Error))
                {
                    throw new ArgumentOutOfRangeException("value");
                }
                this._missingMemberHandling = value;
            }
        }

        public virtual Newtonsoft.Json.NullValueHandling NullValueHandling
        {
            get
            {
                return this._nullValueHandling;
            }
            set
            {
                if ((value < Newtonsoft.Json.NullValueHandling.Include) || (value > Newtonsoft.Json.NullValueHandling.Ignore))
                {
                    throw new ArgumentOutOfRangeException("value");
                }
                this._nullValueHandling = value;
            }
        }

        public virtual Newtonsoft.Json.ObjectCreationHandling ObjectCreationHandling
        {
            get
            {
                return this._objectCreationHandling;
            }
            set
            {
                if ((value < Newtonsoft.Json.ObjectCreationHandling.Auto) || (value > Newtonsoft.Json.ObjectCreationHandling.Replace))
                {
                    throw new ArgumentOutOfRangeException("value");
                }
                this._objectCreationHandling = value;
            }
        }

        public virtual Newtonsoft.Json.PreserveReferencesHandling PreserveReferencesHandling
        {
            get
            {
                return this._preserveReferencesHandling;
            }
            set
            {
                if ((value < Newtonsoft.Json.PreserveReferencesHandling.None) || (value > Newtonsoft.Json.PreserveReferencesHandling.All))
                {
                    throw new ArgumentOutOfRangeException("value");
                }
                this._preserveReferencesHandling = value;
            }
        }

        public virtual Newtonsoft.Json.ReferenceLoopHandling ReferenceLoopHandling
        {
            get
            {
                return this._referenceLoopHandling;
            }
            set
            {
                if ((value < Newtonsoft.Json.ReferenceLoopHandling.Error) || (value > Newtonsoft.Json.ReferenceLoopHandling.Serialize))
                {
                    throw new ArgumentOutOfRangeException("value");
                }
                this._referenceLoopHandling = value;
            }
        }

        public virtual IReferenceResolver ReferenceResolver
        {
            get
            {
                if (this._referenceResolver == null)
                {
                    this._referenceResolver = new DefaultReferenceResolver();
                }
                return this._referenceResolver;
            }
            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException("value", "Reference resolver cannot be null.");
                }
                this._referenceResolver = value;
            }
        }

        public virtual FormatterAssemblyStyle TypeNameAssemblyFormat
        {
            get
            {
                return this._typeNameAssemblyFormat;
            }
            set
            {
                if ((value < FormatterAssemblyStyle.Simple) || (value > FormatterAssemblyStyle.Full))
                {
                    throw new ArgumentOutOfRangeException("value");
                }
                this._typeNameAssemblyFormat = value;
            }
        }

        public virtual Newtonsoft.Json.TypeNameHandling TypeNameHandling
        {
            get
            {
                return this._typeNameHandling;
            }
            set
            {
                if ((value < Newtonsoft.Json.TypeNameHandling.None) || (value > Newtonsoft.Json.TypeNameHandling.Auto))
                {
                    throw new ArgumentOutOfRangeException("value");
                }
                this._typeNameHandling = value;
            }
        }
    }
}

