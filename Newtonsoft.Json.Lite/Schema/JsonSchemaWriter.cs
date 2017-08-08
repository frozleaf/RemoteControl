namespace Newtonsoft.Json.Schema
{
    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;
    using Newtonsoft.Json.Utilities;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    internal class JsonSchemaWriter
    {
        private readonly JsonSchemaResolver _resolver;
        private readonly JsonWriter _writer;

        public JsonSchemaWriter(JsonWriter writer, JsonSchemaResolver resolver)
        {
            ValidationUtils.ArgumentNotNull(writer, "writer");
            this._writer = writer;
            this._resolver = resolver;
        }

        private void ReferenceOrWriteSchema(JsonSchema schema)
        {
            if ((schema.Id != null) && (this._resolver.GetSchema(schema.Id) != null))
            {
                this._writer.WriteStartObject();
                this._writer.WritePropertyName("$ref");
                this._writer.WriteValue(schema.Id);
                this._writer.WriteEndObject();
            }
            else
            {
                this.WriteSchema(schema);
            }
        }

        private void WriteItems(JsonSchema schema)
        {
            if (!CollectionUtils.IsNullOrEmpty<JsonSchema>(schema.Items))
            {
                this._writer.WritePropertyName("items");
                if (schema.Items.Count == 1)
                {
                    this.ReferenceOrWriteSchema(schema.Items[0]);
                }
                else
                {
                    this._writer.WriteStartArray();
                    foreach (JsonSchema schema2 in schema.Items)
                    {
                        this.ReferenceOrWriteSchema(schema2);
                    }
                    this._writer.WriteEndArray();
                }
            }
        }

        private void WritePropertyIfNotNull(JsonWriter writer, string propertyName, object value)
        {
            if (value != null)
            {
                writer.WritePropertyName(propertyName);
                writer.WriteValue(value);
            }
        }

        public void WriteSchema(JsonSchema schema)
        {
            ValidationUtils.ArgumentNotNull(schema, "schema");
            if (!this._resolver.LoadedSchemas.Contains(schema))
            {
                this._resolver.LoadedSchemas.Add(schema);
            }
            this._writer.WriteStartObject();
            this.WritePropertyIfNotNull(this._writer, "id", schema.Id);
            this.WritePropertyIfNotNull(this._writer, "title", schema.Title);
            this.WritePropertyIfNotNull(this._writer, "description", schema.Description);
            this.WritePropertyIfNotNull(this._writer, "required", schema.Required);
            this.WritePropertyIfNotNull(this._writer, "readonly", schema.ReadOnly);
            this.WritePropertyIfNotNull(this._writer, "hidden", schema.Hidden);
            this.WritePropertyIfNotNull(this._writer, "transient", schema.Transient);
            if (schema.Type.HasValue)
            {
                this.WriteType("type", this._writer, schema.Type.Value);
            }
            if (!schema.AllowAdditionalProperties)
            {
                this._writer.WritePropertyName("additionalProperties");
                this._writer.WriteValue(schema.AllowAdditionalProperties);
            }
            else if (schema.AdditionalProperties != null)
            {
                this._writer.WritePropertyName("additionalProperties");
                this.ReferenceOrWriteSchema(schema.AdditionalProperties);
            }
            this.WriteSchemaDictionaryIfNotNull(this._writer, "properties", schema.Properties);
            this.WriteSchemaDictionaryIfNotNull(this._writer, "patternProperties", schema.PatternProperties);
            this.WriteItems(schema);
            this.WritePropertyIfNotNull(this._writer, "minimum", schema.Minimum);
            this.WritePropertyIfNotNull(this._writer, "maximum", schema.Maximum);
            this.WritePropertyIfNotNull(this._writer, "exclusiveMinimum", schema.ExclusiveMinimum);
            this.WritePropertyIfNotNull(this._writer, "exclusiveMaximum", schema.ExclusiveMaximum);
            this.WritePropertyIfNotNull(this._writer, "minLength", schema.MinimumLength);
            this.WritePropertyIfNotNull(this._writer, "maxLength", schema.MaximumLength);
            this.WritePropertyIfNotNull(this._writer, "minItems", schema.MinimumItems);
            this.WritePropertyIfNotNull(this._writer, "maxItems", schema.MaximumItems);
            this.WritePropertyIfNotNull(this._writer, "divisibleBy", schema.DivisibleBy);
            this.WritePropertyIfNotNull(this._writer, "format", schema.Format);
            this.WritePropertyIfNotNull(this._writer, "pattern", schema.Pattern);
            if (schema.Enum != null)
            {
                this._writer.WritePropertyName("enum");
                this._writer.WriteStartArray();
                foreach (JToken token in schema.Enum)
                {
                    token.WriteTo(this._writer, new JsonConverter[0]);
                }
                this._writer.WriteEndArray();
            }
            if (schema.Default != null)
            {
                this._writer.WritePropertyName("default");
                schema.Default.WriteTo(this._writer, new JsonConverter[0]);
            }
            if (schema.Options != null)
            {
                this._writer.WritePropertyName("options");
                this._writer.WriteStartArray();
                foreach (KeyValuePair<JToken, string> pair in schema.Options)
                {
                    this._writer.WriteStartObject();
                    this._writer.WritePropertyName("value");
                    pair.Key.WriteTo(this._writer, new JsonConverter[0]);
                    if (pair.Value != null)
                    {
                        this._writer.WritePropertyName("label");
                        this._writer.WriteValue(pair.Value);
                    }
                    this._writer.WriteEndObject();
                }
                this._writer.WriteEndArray();
            }
            if (schema.Disallow.HasValue)
            {
                this.WriteType("disallow", this._writer, schema.Disallow.Value);
            }
            if (schema.Extends != null)
            {
                this._writer.WritePropertyName("extends");
                this.ReferenceOrWriteSchema(schema.Extends);
            }
            this._writer.WriteEndObject();
        }

        private void WriteSchemaDictionaryIfNotNull(JsonWriter writer, string propertyName, IDictionary<string, JsonSchema> properties)
        {
            if (properties != null)
            {
                writer.WritePropertyName(propertyName);
                writer.WriteStartObject();
                foreach (KeyValuePair<string, JsonSchema> pair in properties)
                {
                    writer.WritePropertyName(pair.Key);
                    this.ReferenceOrWriteSchema(pair.Value);
                }
                writer.WriteEndObject();
            }
        }

        private void WriteType(string propertyName, JsonWriter writer, JsonSchemaType type)
        {
            IList<JsonSchemaType> list;
            if (Enum.IsDefined(typeof(JsonSchemaType), type))
            {
                List<JsonSchemaType> list2 = new List<JsonSchemaType> {
                    type
                };
                list = list2;
            }
            else
            {
                list = (from v in EnumUtils.GetFlagsValues<JsonSchemaType>(type)
                    where v != JsonSchemaType.None
                    select v).ToList<JsonSchemaType>();
            }
            if (list.Count != 0)
            {
                writer.WritePropertyName(propertyName);
                if (list.Count == 1)
                {
                    writer.WriteValue(JsonSchemaBuilder.MapType(list[0]));
                }
                else
                {
                    writer.WriteStartArray();
                    foreach (JsonSchemaType type2 in list)
                    {
                        writer.WriteValue(JsonSchemaBuilder.MapType(type2));
                    }
                    writer.WriteEndArray();
                }
            }
        }
    }
}

