using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;
using Newtonsoft.Json.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;

namespace Newtonsoft.Json.Schema
{
    public class JsonSchemaGenerator
    {
        private class TypeSchema
        {
            public Type Type
            {
                get;
                private set;
            }

            public JsonSchema Schema
            {
                get;
                private set;
            }

            public TypeSchema(Type type, JsonSchema schema)
            {
                ValidationUtils.ArgumentNotNull(type, "type");
                ValidationUtils.ArgumentNotNull(schema, "schema");
                this.Type = type;
                this.Schema = schema;
            }
        }

        private IContractResolver _contractResolver;

        private JsonSchemaResolver _resolver;

        private IList<JsonSchemaGenerator.TypeSchema> _stack = new List<JsonSchemaGenerator.TypeSchema>();

        private JsonSchema _currentSchema;

        public UndefinedSchemaIdHandling UndefinedSchemaIdHandling
        {
            get;
            set;
        }

        public IContractResolver ContractResolver
        {
            get
            {
                IContractResolver result;
                if (this._contractResolver == null)
                {
                    result = DefaultContractResolver.Instance;
                }
                else
                {
                    result = this._contractResolver;
                }
                return result;
            }
            set
            {
                this._contractResolver = value;
            }
        }

        private JsonSchema CurrentSchema
        {
            get
            {
                return this._currentSchema;
            }
        }

        private void Push(JsonSchemaGenerator.TypeSchema typeSchema)
        {
            this._currentSchema = typeSchema.Schema;
            this._stack.Add(typeSchema);
            this._resolver.LoadedSchemas.Add(typeSchema.Schema);
        }

        private JsonSchemaGenerator.TypeSchema Pop()
        {
            JsonSchemaGenerator.TypeSchema result = this._stack[this._stack.Count - 1];
            this._stack.RemoveAt(this._stack.Count - 1);
            JsonSchemaGenerator.TypeSchema typeSchema = this._stack.LastOrDefault<JsonSchemaGenerator.TypeSchema>();
            if (typeSchema != null)
            {
                this._currentSchema = typeSchema.Schema;
            }
            else
            {
                this._currentSchema = null;
            }
            return result;
        }

        public JsonSchema Generate(Type type)
        {
            return this.Generate(type, new JsonSchemaResolver(), false);
        }

        public JsonSchema Generate(Type type, JsonSchemaResolver resolver)
        {
            return this.Generate(type, resolver, false);
        }

        public JsonSchema Generate(Type type, bool rootSchemaNullable)
        {
            return this.Generate(type, new JsonSchemaResolver(), rootSchemaNullable);
        }

        public JsonSchema Generate(Type type, JsonSchemaResolver resolver, bool rootSchemaNullable)
        {
            ValidationUtils.ArgumentNotNull(type, "type");
            ValidationUtils.ArgumentNotNull(resolver, "resolver");
            this._resolver = resolver;
            return this.GenerateInternal(type, (!rootSchemaNullable) ? Required.Always : Required.Default, false);
        }

        private string GetTitle(Type type)
        {
            JsonContainerAttribute jsonContainerAttribute = JsonTypeReflector.GetJsonContainerAttribute(type);
            string result;
            if (jsonContainerAttribute != null && !string.IsNullOrEmpty(jsonContainerAttribute.Title))
            {
                result = jsonContainerAttribute.Title;
            }
            else
            {
                result = null;
            }
            return result;
        }

        private string GetDescription(Type type)
        {
            JsonContainerAttribute jsonContainerAttribute = JsonTypeReflector.GetJsonContainerAttribute(type);
            string result;
            if (jsonContainerAttribute != null && !string.IsNullOrEmpty(jsonContainerAttribute.Description))
            {
                result = jsonContainerAttribute.Description;
            }
            else
            {
                DescriptionAttribute attribute = ReflectionUtils.GetAttribute<DescriptionAttribute>(type);
                if (attribute != null)
                {
                    result = attribute.Description;
                }
                else
                {
                    result = null;
                }
            }
            return result;
        }

        private string GetTypeId(Type type, bool explicitOnly)
        {
            JsonContainerAttribute jsonContainerAttribute = JsonTypeReflector.GetJsonContainerAttribute(type);
            string result;
            if (jsonContainerAttribute != null && !string.IsNullOrEmpty(jsonContainerAttribute.Id))
            {
                result = jsonContainerAttribute.Id;
            }
            else if (explicitOnly)
            {
                result = null;
            }
            else
            {
                switch (this.UndefinedSchemaIdHandling)
                {
                    case UndefinedSchemaIdHandling.UseTypeName:
                        result = type.FullName;
                        break;
                    case UndefinedSchemaIdHandling.UseAssemblyQualifiedName:
                        result = type.AssemblyQualifiedName;
                        break;
                    default:
                        result = null;
                        break;
                }
            }
            return result;
        }

        private JsonSchema GenerateInternal(Type type, Required valueRequired, bool required)
        {
            ValidationUtils.ArgumentNotNull(type, "type");
            string typeId = this.GetTypeId(type, false);
            string typeId2 = this.GetTypeId(type, true);
            JsonSchema result;
            if (!string.IsNullOrEmpty(typeId))
            {
                JsonSchema schema = this._resolver.GetSchema(typeId);
                if (schema != null)
                {
                    if (valueRequired != Required.Always && !JsonSchemaGenerator.HasFlag(schema.Type, JsonSchemaType.Null))
                    {
                        schema.Type |= JsonSchemaType.Null;
                    }
                    if (required && schema.Required != true)
                    {
                        schema.Required = new bool?(true);
                    }
                    result = schema;
                    return result;
                }
            }
            if (this._stack.Any((JsonSchemaGenerator.TypeSchema tc) => tc.Type == type))
            {
                throw new Exception("Unresolved circular reference for type '{0}'. Explicitly define an Id for the type using a JsonObject/JsonArray attribute or automatically generate a type Id using the UndefinedSchemaIdHandling property.".FormatWith(CultureInfo.InvariantCulture, new object[]
				{
					type
				}));
            }
            JsonContract jsonContract = this.ContractResolver.ResolveContract(type);
            JsonConverter jsonConverter;
            if ((jsonConverter = jsonContract.Converter) != null || (jsonConverter = jsonContract.InternalConverter) != null)
            {
                JsonSchema schema2 = jsonConverter.GetSchema();
                if (schema2 != null)
                {
                    result = schema2;
                    return result;
                }
            }
            this.Push(new JsonSchemaGenerator.TypeSchema(type, new JsonSchema()));
            if (typeId2 != null)
            {
                this.CurrentSchema.Id = typeId2;
            }
            if (required)
            {
                this.CurrentSchema.Required = new bool?(true);
            }
            this.CurrentSchema.Title = this.GetTitle(type);
            this.CurrentSchema.Description = this.GetDescription(type);
            if (jsonConverter != null)
            {
                this.CurrentSchema.Type = new JsonSchemaType?(JsonSchemaType.Any);
            }
            else if (jsonContract is JsonDictionaryContract)
            {
                this.CurrentSchema.Type = new JsonSchemaType?(this.AddNullType(JsonSchemaType.Object, valueRequired));
                Type type2;
                Type type3;
                ReflectionUtils.GetDictionaryKeyValueTypes(type, out type2, out type3);
                if (type2 != null)
                {
                    if (typeof(IConvertible).IsAssignableFrom(type2))
                    {
                        this.CurrentSchema.AdditionalProperties = this.GenerateInternal(type3, Required.Default, false);
                    }
                }
            }
            else if (jsonContract is JsonArrayContract)
            {
                this.CurrentSchema.Type = new JsonSchemaType?(this.AddNullType(JsonSchemaType.Array, valueRequired));
                this.CurrentSchema.Id = this.GetTypeId(type, false);
                JsonArrayAttribute jsonArrayAttribute = JsonTypeReflector.GetJsonContainerAttribute(type) as JsonArrayAttribute;
                bool flag = jsonArrayAttribute == null || jsonArrayAttribute.AllowNullItems;
                Type collectionItemType = ReflectionUtils.GetCollectionItemType(type);
                if (collectionItemType != null)
                {
                    this.CurrentSchema.Items = new List<JsonSchema>();
                    this.CurrentSchema.Items.Add(this.GenerateInternal(collectionItemType, (!flag) ? Required.Always : Required.Default, false));
                }
            }
            else if (jsonContract is JsonPrimitiveContract)
            {
                this.CurrentSchema.Type = new JsonSchemaType?(this.GetJsonSchemaType(type, valueRequired));
                if (this.CurrentSchema.Type == JsonSchemaType.Integer && type.IsEnum && !type.IsDefined(typeof(FlagsAttribute), true))
                {
                    this.CurrentSchema.Enum = new List<JToken>();
                    this.CurrentSchema.Options = new Dictionary<JToken, string>();
                    EnumValues<long> namesAndValues = EnumUtils.GetNamesAndValues<long>(type);
                    foreach (EnumValue<long> current in namesAndValues)
                    {
                        JToken jToken = JToken.FromObject(current.Value);
                        this.CurrentSchema.Enum.Add(jToken);
                        this.CurrentSchema.Options.Add(jToken, current.Name);
                    }
                }
            }
            else if (jsonContract is JsonObjectContract)
            {
                this.CurrentSchema.Type = new JsonSchemaType?(this.AddNullType(JsonSchemaType.Object, valueRequired));
                this.CurrentSchema.Id = this.GetTypeId(type, false);
                this.GenerateObjectSchema(type, (JsonObjectContract)jsonContract);
            }
            else if (jsonContract is JsonISerializableContract)
            {
                this.CurrentSchema.Type = new JsonSchemaType?(this.AddNullType(JsonSchemaType.Object, valueRequired));
                this.CurrentSchema.Id = this.GetTypeId(type, false);
                this.GenerateISerializableContract(type, (JsonISerializableContract)jsonContract);
            }
            else if (jsonContract is JsonStringContract)
            {
                JsonSchemaType value = (!ReflectionUtils.IsNullable(jsonContract.UnderlyingType)) ? JsonSchemaType.String : this.AddNullType(JsonSchemaType.String, valueRequired);
                this.CurrentSchema.Type = new JsonSchemaType?(value);
            }
            else
            {
                if (!(jsonContract is JsonLinqContract))
                {
                    throw new Exception("Unexpected contract type: {0}".FormatWith(CultureInfo.InvariantCulture, new object[]
					{
						jsonContract
					}));
                }
                this.CurrentSchema.Type = new JsonSchemaType?(JsonSchemaType.Any);
            }
            result = this.Pop().Schema;
            return result;
        }

        private JsonSchemaType AddNullType(JsonSchemaType type, Required valueRequired)
        {
            JsonSchemaType result;
            if (valueRequired != Required.Always)
            {
                result = (type | JsonSchemaType.Null);
            }
            else
            {
                result = type;
            }
            return result;
        }

        private void GenerateObjectSchema(Type type, JsonObjectContract contract)
        {
            this.CurrentSchema.Properties = new Dictionary<string, JsonSchema>();
            foreach (JsonProperty current in contract.Properties)
            {
                if (!current.Ignored)
                {
                    bool flag = current.NullValueHandling == NullValueHandling.Ignore || current.DefaultValueHandling == DefaultValueHandling.Ignore || current.ShouldSerialize != null || current.GetIsSpecified != null;
                    JsonSchema jsonSchema = this.GenerateInternal(current.PropertyType, current.Required, !flag);
                    if (current.DefaultValue != null)
                    {
                        jsonSchema.Default = JToken.FromObject(current.DefaultValue);
                    }
                    this.CurrentSchema.Properties.Add(current.PropertyName, jsonSchema);
                }
            }
            if (type.IsSealed)
            {
                this.CurrentSchema.AllowAdditionalProperties = false;
            }
        }

        private void GenerateISerializableContract(Type type, JsonISerializableContract contract)
        {
            this.CurrentSchema.AllowAdditionalProperties = true;
        }

        internal static bool HasFlag(JsonSchemaType? value, JsonSchemaType flag)
        {
            return !value.HasValue || (value & flag) == flag;
        }

        private JsonSchemaType GetJsonSchemaType(Type type, Required valueRequired)
        {
            JsonSchemaType jsonSchemaType = JsonSchemaType.None;
            if (valueRequired != Required.Always && ReflectionUtils.IsNullable(type))
            {
                jsonSchemaType = JsonSchemaType.Null;
                if (ReflectionUtils.IsNullableType(type))
                {
                    type = Nullable.GetUnderlyingType(type);
                }
            }
            TypeCode typeCode = Type.GetTypeCode(type);
            switch (typeCode)
            {
                case TypeCode.Empty:
                case TypeCode.Object:
                    {
                        JsonSchemaType result = jsonSchemaType | JsonSchemaType.String;
                        return result;
                    }
                case TypeCode.DBNull:
                    {
                        JsonSchemaType result = jsonSchemaType | JsonSchemaType.Null;
                        return result;
                    }
                case TypeCode.Boolean:
                    {
                        JsonSchemaType result = jsonSchemaType | JsonSchemaType.Boolean;
                        return result;
                    }
                case TypeCode.Char:
                    {
                        JsonSchemaType result = jsonSchemaType | JsonSchemaType.String;
                        return result;
                    }
                case TypeCode.SByte:
                case TypeCode.Byte:
                case TypeCode.Int16:
                case TypeCode.UInt16:
                case TypeCode.Int32:
                case TypeCode.UInt32:
                case TypeCode.Int64:
                case TypeCode.UInt64:
                    {
                        JsonSchemaType result = jsonSchemaType | JsonSchemaType.Integer;
                        return result;
                    }
                case TypeCode.Single:
                case TypeCode.Double:
                case TypeCode.Decimal:
                    {
                        JsonSchemaType result = jsonSchemaType | JsonSchemaType.Float;
                        return result;
                    }
                case TypeCode.DateTime:
                    {
                        JsonSchemaType result = jsonSchemaType | JsonSchemaType.String;
                        return result;
                    }
                case TypeCode.String:
                    {
                        JsonSchemaType result = jsonSchemaType | JsonSchemaType.String;
                        return result;
                    }
            }
            throw new Exception("Unexpected type code '{0}' for type '{1}'.".FormatWith(CultureInfo.InvariantCulture, new object[]
			{
				typeCode,
				type
			}));
        }
    }
}
