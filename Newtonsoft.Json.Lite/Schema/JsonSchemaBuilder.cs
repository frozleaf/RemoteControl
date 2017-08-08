namespace Newtonsoft.Json.Schema
{
    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;
    using Newtonsoft.Json.Utilities;
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;

    internal class JsonSchemaBuilder
    {
        private JsonSchema _currentSchema;
        private JsonReader _reader;
        private readonly JsonSchemaResolver _resolver;
        private readonly IList<JsonSchema> _stack = new List<JsonSchema>();

        public JsonSchemaBuilder(JsonSchemaResolver resolver)
        {
            this._resolver = resolver;
        }

        private JsonSchema BuildSchema()
        {
            if (this._reader.TokenType != JsonToken.StartObject)
            {
                throw new Exception("Expected StartObject while parsing schema object, got {0}.".FormatWith(CultureInfo.InvariantCulture, new object[] { this._reader.TokenType }));
            }
            this._reader.Read();
            if (this._reader.TokenType == JsonToken.EndObject)
            {
                this.Push(new JsonSchema());
                return this.Pop();
            }
            string propertyName = Convert.ToString(this._reader.Value, CultureInfo.InvariantCulture);
            this._reader.Read();
            if (propertyName == "$ref")
            {
                string id = (string) this._reader.Value;
                while (this._reader.Read() && (this._reader.TokenType != JsonToken.EndObject))
                {
                    if (this._reader.TokenType == JsonToken.StartObject)
                    {
                        throw new Exception("Found StartObject within the schema reference with the Id '{0}'".FormatWith(CultureInfo.InvariantCulture, new object[] { id }));
                    }
                }
                JsonSchema schema = this._resolver.GetSchema(id);
                if (schema == null)
                {
                    throw new Exception("Could not resolve schema reference for Id '{0}'.".FormatWith(CultureInfo.InvariantCulture, new object[] { id }));
                }
                return schema;
            }
            this.Push(new JsonSchema());
            this.ProcessSchemaProperty(propertyName);
            while (this._reader.Read() && (this._reader.TokenType != JsonToken.EndObject))
            {
                propertyName = Convert.ToString(this._reader.Value, CultureInfo.InvariantCulture);
                this._reader.Read();
                this.ProcessSchemaProperty(propertyName);
            }
            return this.Pop();
        }

        internal static string MapType(JsonSchemaType type)
        {
            return JsonSchemaConstants.JsonSchemaTypeMapping.Single<KeyValuePair<string, JsonSchemaType>>(kv => (((JsonSchemaType) kv.Value) == type)).Key;
        }

        internal static JsonSchemaType MapType(string type)
        {
            JsonSchemaType type2;
            if (!JsonSchemaConstants.JsonSchemaTypeMapping.TryGetValue(type, out type2))
            {
                throw new Exception("Invalid JSON schema type: {0}".FormatWith(CultureInfo.InvariantCulture, new object[] { type }));
            }
            return type2;
        }

        internal JsonSchema Parse(JsonReader reader)
        {
            this._reader = reader;
            if (reader.TokenType == JsonToken.None)
            {
                this._reader.Read();
            }
            return this.BuildSchema();
        }

        private JsonSchema Pop()
        {
            JsonSchema schema = this._currentSchema;
            this._stack.RemoveAt(this._stack.Count - 1);
            this._currentSchema = this._stack.LastOrDefault<JsonSchema>();
            return schema;
        }

        private void ProcessAdditionalProperties()
        {
            if (this._reader.TokenType == JsonToken.Boolean)
            {
                this.CurrentSchema.AllowAdditionalProperties = (bool) this._reader.Value;
            }
            else
            {
                this.CurrentSchema.AdditionalProperties = this.BuildSchema();
            }
        }

        private void ProcessDefault()
        {
            this.CurrentSchema.Default = JToken.ReadFrom(this._reader);
        }

        private void ProcessEnum()
        {
            if (this._reader.TokenType != JsonToken.StartArray)
            {
                throw new Exception("Expected StartArray token while parsing enum values, got {0}.".FormatWith(CultureInfo.InvariantCulture, new object[] { this._reader.TokenType }));
            }
            this.CurrentSchema.Enum = new List<JToken>();
            while (this._reader.Read() && (this._reader.TokenType != JsonToken.EndArray))
            {
                JToken item = JToken.ReadFrom(this._reader);
                this.CurrentSchema.Enum.Add(item);
            }
        }

        private void ProcessExtends()
        {
            this.CurrentSchema.Extends = this.BuildSchema();
        }

        private void ProcessIdentity()
        {
            this.CurrentSchema.Identity = new List<string>();
            JsonToken tokenType = this._reader.TokenType;
            if (tokenType != JsonToken.StartArray)
            {
                if (tokenType != JsonToken.String)
                {
                    throw new Exception("Expected array or JSON property name string token, got {0}.".FormatWith(CultureInfo.InvariantCulture, new object[] { this._reader.TokenType }));
                }
                this.CurrentSchema.Identity.Add(this._reader.Value.ToString());
            }
            else
            {
                while (this._reader.Read() && (this._reader.TokenType != JsonToken.EndArray))
                {
                    if (this._reader.TokenType != JsonToken.String)
                    {
                        throw new Exception("Exception JSON property name string token, got {0}.".FormatWith(CultureInfo.InvariantCulture, new object[] { this._reader.TokenType }));
                    }
                    this.CurrentSchema.Identity.Add(this._reader.Value.ToString());
                }
            }
        }

        private void ProcessItems()
        {
            this.CurrentSchema.Items = new List<JsonSchema>();
            switch (this._reader.TokenType)
            {
                case JsonToken.StartObject:
                    this.CurrentSchema.Items.Add(this.BuildSchema());
                    break;

                case JsonToken.StartArray:
                    while (this._reader.Read() && (this._reader.TokenType != JsonToken.EndArray))
                    {
                        this.CurrentSchema.Items.Add(this.BuildSchema());
                    }
                    break;

                default:
                    throw new Exception("Expected array or JSON schema object token, got {0}.".FormatWith(CultureInfo.InvariantCulture, new object[] { this._reader.TokenType }));
            }
        }

        private void ProcessOptions()
        {
            this.CurrentSchema.Options = new Dictionary<JToken, string>(new JTokenEqualityComparer());
            if (this._reader.TokenType != JsonToken.StartArray)
            {
                throw new Exception("Expected array token, got {0}.".FormatWith(CultureInfo.InvariantCulture, new object[] { this._reader.TokenType }));
            }
            while (this._reader.Read() && (this._reader.TokenType != JsonToken.EndArray))
            {
                if (this._reader.TokenType != JsonToken.StartObject)
                {
                    throw new Exception("Expect object token, got {0}.".FormatWith(CultureInfo.InvariantCulture, new object[] { this._reader.TokenType }));
                }
                string str = null;
                JToken key = null;
                while (this._reader.Read() && (this._reader.TokenType != JsonToken.EndObject))
                {
                    string str2 = Convert.ToString(this._reader.Value, CultureInfo.InvariantCulture);
                    this._reader.Read();
                    switch (str2)
                    {
                        case "value":
                        {
                            key = JToken.ReadFrom(this._reader);
                            continue;
                        }
                        case "label":
                        {
                            str = (string) this._reader.Value;
                            continue;
                        }
                    }
                    throw new Exception("Unexpected property in JSON schema option: {0}.".FormatWith(CultureInfo.InvariantCulture, new object[] { str2 }));
                }
                if (key == null)
                {
                    throw new Exception("No value specified for JSON schema option.");
                }
                if (this.CurrentSchema.Options.ContainsKey(key))
                {
                    throw new Exception("Duplicate value in JSON schema option collection: {0}".FormatWith(CultureInfo.InvariantCulture, new object[] { key }));
                }
                this.CurrentSchema.Options.Add(key, str);
            }
        }

        private void ProcessPatternProperties()
        {
            Dictionary<string, JsonSchema> dictionary = new Dictionary<string, JsonSchema>();
            if (this._reader.TokenType != JsonToken.StartObject)
            {
                throw new Exception("Expected start object token.");
            }
            while (this._reader.Read() && (this._reader.TokenType != JsonToken.EndObject))
            {
                string key = Convert.ToString(this._reader.Value, CultureInfo.InvariantCulture);
                this._reader.Read();
                if (dictionary.ContainsKey(key))
                {
                    throw new Exception("Property {0} has already been defined in schema.".FormatWith(CultureInfo.InvariantCulture, new object[] { key }));
                }
                dictionary.Add(key, this.BuildSchema());
            }
            this.CurrentSchema.PatternProperties = dictionary;
        }

        private void ProcessProperties()
        {
            IDictionary<string, JsonSchema> dictionary = new Dictionary<string, JsonSchema>();
            if (this._reader.TokenType != JsonToken.StartObject)
            {
                throw new Exception("Expected StartObject token while parsing schema properties, got {0}.".FormatWith(CultureInfo.InvariantCulture, new object[] { this._reader.TokenType }));
            }
            while (this._reader.Read() && (this._reader.TokenType != JsonToken.EndObject))
            {
                string key = Convert.ToString(this._reader.Value, CultureInfo.InvariantCulture);
                this._reader.Read();
                if (dictionary.ContainsKey(key))
                {
                    throw new Exception("Property {0} has already been defined in schema.".FormatWith(CultureInfo.InvariantCulture, new object[] { key }));
                }
                dictionary.Add(key, this.BuildSchema());
            }
            this.CurrentSchema.Properties = dictionary;
        }

        private void ProcessSchemaProperty(string propertyName)
        {
            switch (propertyName)
            {
                case "type":
                    this.CurrentSchema.Type = this.ProcessType();
                    break;

                case "id":
                    this.CurrentSchema.Id = (string) this._reader.Value;
                    break;

                case "title":
                    this.CurrentSchema.Title = (string) this._reader.Value;
                    break;

                case "description":
                    this.CurrentSchema.Description = (string) this._reader.Value;
                    break;

                case "properties":
                    this.ProcessProperties();
                    break;

                case "items":
                    this.ProcessItems();
                    break;

                case "additionalProperties":
                    this.ProcessAdditionalProperties();
                    break;

                case "patternProperties":
                    this.ProcessPatternProperties();
                    break;

                case "required":
                    this.CurrentSchema.Required = new bool?((bool) this._reader.Value);
                    break;

                case "requires":
                    this.CurrentSchema.Requires = (string) this._reader.Value;
                    break;

                case "identity":
                    this.ProcessIdentity();
                    break;

                case "minimum":
                    this.CurrentSchema.Minimum = new double?(Convert.ToDouble(this._reader.Value, CultureInfo.InvariantCulture));
                    break;

                case "maximum":
                    this.CurrentSchema.Maximum = new double?(Convert.ToDouble(this._reader.Value, CultureInfo.InvariantCulture));
                    break;

                case "exclusiveMinimum":
                    this.CurrentSchema.ExclusiveMinimum = new bool?((bool) this._reader.Value);
                    break;

                case "exclusiveMaximum":
                    this.CurrentSchema.ExclusiveMaximum = new bool?((bool) this._reader.Value);
                    break;

                case "maxLength":
                    this.CurrentSchema.MaximumLength = new int?(Convert.ToInt32(this._reader.Value, CultureInfo.InvariantCulture));
                    break;

                case "minLength":
                    this.CurrentSchema.MinimumLength = new int?(Convert.ToInt32(this._reader.Value, CultureInfo.InvariantCulture));
                    break;

                case "maxItems":
                    this.CurrentSchema.MaximumItems = new int?(Convert.ToInt32(this._reader.Value, CultureInfo.InvariantCulture));
                    break;

                case "minItems":
                    this.CurrentSchema.MinimumItems = new int?(Convert.ToInt32(this._reader.Value, CultureInfo.InvariantCulture));
                    break;

                case "divisibleBy":
                    this.CurrentSchema.DivisibleBy = new double?(Convert.ToDouble(this._reader.Value, CultureInfo.InvariantCulture));
                    break;

                case "disallow":
                    this.CurrentSchema.Disallow = this.ProcessType();
                    break;

                case "default":
                    this.ProcessDefault();
                    break;

                case "hidden":
                    this.CurrentSchema.Hidden = new bool?((bool) this._reader.Value);
                    break;

                case "readonly":
                    this.CurrentSchema.ReadOnly = new bool?((bool) this._reader.Value);
                    break;

                case "format":
                    this.CurrentSchema.Format = (string) this._reader.Value;
                    break;

                case "pattern":
                    this.CurrentSchema.Pattern = (string) this._reader.Value;
                    break;

                case "options":
                    this.ProcessOptions();
                    break;

                case "enum":
                    this.ProcessEnum();
                    break;

                case "extends":
                    this.ProcessExtends();
                    break;

                default:
                    this._reader.Skip();
                    break;
            }
        }

        private JsonSchemaType? ProcessType()
        {
            switch (this._reader.TokenType)
            {
                case JsonToken.StartArray:
                {
                    JsonSchemaType? nullable = new JsonSchemaType?(JsonSchemaType.None);
                    while (this._reader.Read() && (this._reader.TokenType != JsonToken.EndArray))
                    {
                        if (this._reader.TokenType != JsonToken.String)
                        {
                            throw new Exception("Exception JSON schema type string token, got {0}.".FormatWith(CultureInfo.InvariantCulture, new object[] { this._reader.TokenType }));
                        }
                        JsonSchemaType? nullable3 = nullable;
                        JsonSchemaType type = MapType(this._reader.Value.ToString());
                        nullable = nullable3.HasValue ? new JsonSchemaType?(((JsonSchemaType) nullable3.GetValueOrDefault()) | type) : null;
                    }
                    return nullable;
                }
                case JsonToken.String:
                    return new JsonSchemaType?(MapType(this._reader.Value.ToString()));
            }
            throw new Exception("Expected array or JSON schema type string token, got {0}.".FormatWith(CultureInfo.InvariantCulture, new object[] { this._reader.TokenType }));
        }

        private void Push(JsonSchema value)
        {
            this._currentSchema = value;
            this._stack.Add(value);
            this._resolver.LoadedSchemas.Add(value);
        }

        private JsonSchema CurrentSchema
        {
            get
            {
                return this._currentSchema;
            }
        }
    }
}

