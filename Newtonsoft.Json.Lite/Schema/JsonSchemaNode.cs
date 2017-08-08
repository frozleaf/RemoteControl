namespace Newtonsoft.Json.Schema
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Runtime.CompilerServices;

    internal class JsonSchemaNode
    {
        public JsonSchemaNode(JsonSchema schema)
        {
            this.Schemas = new ReadOnlyCollection<JsonSchema>(new JsonSchema[] { schema });
            this.Properties = new Dictionary<string, JsonSchemaNode>();
            this.PatternProperties = new Dictionary<string, JsonSchemaNode>();
            this.Items = new List<JsonSchemaNode>();
            this.Id = GetId(this.Schemas);
        }

        private JsonSchemaNode(JsonSchemaNode source, JsonSchema schema)
        {
            this.Schemas = new ReadOnlyCollection<JsonSchema>(source.Schemas.Union<JsonSchema>(new JsonSchema[] { schema }).ToList<JsonSchema>());
            this.Properties = new Dictionary<string, JsonSchemaNode>(source.Properties);
            this.PatternProperties = new Dictionary<string, JsonSchemaNode>(source.PatternProperties);
            this.Items = new List<JsonSchemaNode>(source.Items);
            this.AdditionalProperties = source.AdditionalProperties;
            this.Id = GetId(this.Schemas);
        }

        public JsonSchemaNode Combine(JsonSchema schema)
        {
            return new JsonSchemaNode(this, schema);
        }

        public static string GetId(IEnumerable<JsonSchema> schemata)
        {
            return string.Join("-", (from s in schemata select s.InternalId).OrderBy<string, string>(id => id, StringComparer.Ordinal).ToArray<string>());
        }

        public JsonSchemaNode AdditionalProperties { get; set; }

        public string Id { get; private set; }

        public List<JsonSchemaNode> Items { get; private set; }

        public Dictionary<string, JsonSchemaNode> PatternProperties { get; private set; }

        public Dictionary<string, JsonSchemaNode> Properties { get; private set; }

        public ReadOnlyCollection<JsonSchema> Schemas { get; private set; }
    }
}

