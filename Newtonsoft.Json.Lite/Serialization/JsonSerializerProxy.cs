namespace Newtonsoft.Json.Serialization
{
    using Newtonsoft.Json;
    using Newtonsoft.Json.Utilities;
    using System;
    using System.Runtime.Serialization;
    using System.Runtime.Serialization.Formatters;

    internal class JsonSerializerProxy : JsonSerializer
    {
        private readonly JsonSerializer _serializer;
        private readonly JsonSerializerInternalReader _serializerReader;
        private readonly JsonSerializerInternalWriter _serializerWriter;

        public event EventHandler<ErrorEventArgs> Error
        {
            add
            {
                this._serializer.Error += value;
            }
            remove
            {
                this._serializer.Error -= value;
            }
        }

        public JsonSerializerProxy(JsonSerializerInternalReader serializerReader)
        {
            ValidationUtils.ArgumentNotNull(serializerReader, "serializerReader");
            this._serializerReader = serializerReader;
            this._serializer = serializerReader.Serializer;
        }

        public JsonSerializerProxy(JsonSerializerInternalWriter serializerWriter)
        {
            ValidationUtils.ArgumentNotNull(serializerWriter, "serializerWriter");
            this._serializerWriter = serializerWriter;
            this._serializer = serializerWriter.Serializer;
        }

        internal override object DeserializeInternal(JsonReader reader, Type objectType)
        {
            if (this._serializerReader != null)
            {
                return this._serializerReader.Deserialize(reader, objectType);
            }
            return this._serializer.Deserialize(reader, objectType);
        }

        internal JsonSerializerInternalBase GetInternalSerializer()
        {
            if (this._serializerReader != null)
            {
                return this._serializerReader;
            }
            return this._serializerWriter;
        }

        internal override void PopulateInternal(JsonReader reader, object target)
        {
            if (this._serializerReader != null)
            {
                this._serializerReader.Populate(reader, target);
            }
            else
            {
                this._serializer.Populate(reader, target);
            }
        }

        internal override void SerializeInternal(JsonWriter jsonWriter, object value)
        {
            if (this._serializerWriter != null)
            {
                this._serializerWriter.Serialize(jsonWriter, value);
            }
            else
            {
                this._serializer.Serialize(jsonWriter, value);
            }
        }

        public override SerializationBinder Binder
        {
            get
            {
                return this._serializer.Binder;
            }
            set
            {
                this._serializer.Binder = value;
            }
        }

        public override Newtonsoft.Json.ConstructorHandling ConstructorHandling
        {
            get
            {
                return this._serializer.ConstructorHandling;
            }
            set
            {
                this._serializer.ConstructorHandling = value;
            }
        }

        public override StreamingContext Context
        {
            get
            {
                return this._serializer.Context;
            }
            set
            {
                this._serializer.Context = value;
            }
        }

        public override IContractResolver ContractResolver
        {
            get
            {
                return this._serializer.ContractResolver;
            }
            set
            {
                this._serializer.ContractResolver = value;
            }
        }

        public override JsonConverterCollection Converters
        {
            get
            {
                return this._serializer.Converters;
            }
        }

        public override Newtonsoft.Json.DefaultValueHandling DefaultValueHandling
        {
            get
            {
                return this._serializer.DefaultValueHandling;
            }
            set
            {
                this._serializer.DefaultValueHandling = value;
            }
        }

        public override Newtonsoft.Json.MissingMemberHandling MissingMemberHandling
        {
            get
            {
                return this._serializer.MissingMemberHandling;
            }
            set
            {
                this._serializer.MissingMemberHandling = value;
            }
        }

        public override Newtonsoft.Json.NullValueHandling NullValueHandling
        {
            get
            {
                return this._serializer.NullValueHandling;
            }
            set
            {
                this._serializer.NullValueHandling = value;
            }
        }

        public override Newtonsoft.Json.ObjectCreationHandling ObjectCreationHandling
        {
            get
            {
                return this._serializer.ObjectCreationHandling;
            }
            set
            {
                this._serializer.ObjectCreationHandling = value;
            }
        }

        public override Newtonsoft.Json.PreserveReferencesHandling PreserveReferencesHandling
        {
            get
            {
                return this._serializer.PreserveReferencesHandling;
            }
            set
            {
                this._serializer.PreserveReferencesHandling = value;
            }
        }

        public override Newtonsoft.Json.ReferenceLoopHandling ReferenceLoopHandling
        {
            get
            {
                return this._serializer.ReferenceLoopHandling;
            }
            set
            {
                this._serializer.ReferenceLoopHandling = value;
            }
        }

        public override IReferenceResolver ReferenceResolver
        {
            get
            {
                return this._serializer.ReferenceResolver;
            }
            set
            {
                this._serializer.ReferenceResolver = value;
            }
        }

        public override FormatterAssemblyStyle TypeNameAssemblyFormat
        {
            get
            {
                return this._serializer.TypeNameAssemblyFormat;
            }
            set
            {
                this._serializer.TypeNameAssemblyFormat = value;
            }
        }

        public override Newtonsoft.Json.TypeNameHandling TypeNameHandling
        {
            get
            {
                return this._serializer.TypeNameHandling;
            }
            set
            {
                this._serializer.TypeNameHandling = value;
            }
        }
    }
}

