namespace Newtonsoft.Json.Schema
{
    using Newtonsoft.Json.Linq;
    using Newtonsoft.Json.Utilities;
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;

    internal class JsonSchemaModel
    {
        public JsonSchemaModel()
        {
            this.Type = JsonSchemaType.Any;
            this.AllowAdditionalProperties = true;
            this.Required = false;
        }

        private static void Combine(JsonSchemaModel model, JsonSchema schema)
        {
            bool? nullable;
            model.Required = model.Required ? true : ((nullable = schema.Required).HasValue ? nullable.GetValueOrDefault() : false);
            JsonSchemaType? type = schema.Type;
            model.Type &= (JsonSchemaType) (type.HasValue ? type.GetValueOrDefault() : JsonSchemaType.Any);
            model.MinimumLength = MathUtils.Max(model.MinimumLength, schema.MinimumLength);
            model.MaximumLength = MathUtils.Min(model.MaximumLength, schema.MaximumLength);
            model.DivisibleBy = MathUtils.Max(model.DivisibleBy, schema.DivisibleBy);
            model.Minimum = MathUtils.Max(model.Minimum, schema.Minimum);
            model.Maximum = MathUtils.Max(model.Maximum, schema.Maximum);
            model.ExclusiveMinimum = model.ExclusiveMinimum ? true : ((nullable = schema.ExclusiveMinimum).HasValue ? nullable.GetValueOrDefault() : false);
            model.ExclusiveMaximum = model.ExclusiveMaximum ? true : ((nullable = schema.ExclusiveMaximum).HasValue ? nullable.GetValueOrDefault() : false);
            model.MinimumItems = MathUtils.Max(model.MinimumItems, schema.MinimumItems);
            model.MaximumItems = MathUtils.Min(model.MaximumItems, schema.MaximumItems);
            model.AllowAdditionalProperties = model.AllowAdditionalProperties && schema.AllowAdditionalProperties;
            if (schema.Enum != null)
            {
                if (model.Enum == null)
                {
                    model.Enum = new List<JToken>();
                }
                model.Enum.AddRangeDistinct<JToken>(schema.Enum, new JTokenEqualityComparer());
            }
            type = schema.Disallow;
            model.Disallow |= (JsonSchemaType) (type.HasValue ? type.GetValueOrDefault() : JsonSchemaType.None);
            if (schema.Pattern != null)
            {
                if (model.Patterns == null)
                {
                    model.Patterns = new List<string>();
                }
                model.Patterns.AddDistinct<string>(schema.Pattern);
            }
        }

        public static JsonSchemaModel Create(IList<JsonSchema> schemata)
        {
            JsonSchemaModel model = new JsonSchemaModel();
            foreach (JsonSchema schema in schemata)
            {
                Combine(model, schema);
            }
            return model;
        }

        public JsonSchemaModel AdditionalProperties { get; set; }

        public bool AllowAdditionalProperties { get; set; }

        public JsonSchemaType Disallow { get; set; }

        public double? DivisibleBy { get; set; }

        public IList<JToken> Enum { get; set; }

        public bool ExclusiveMaximum { get; set; }

        public bool ExclusiveMinimum { get; set; }

        public IList<JsonSchemaModel> Items { get; set; }

        public double? Maximum { get; set; }

        public int? MaximumItems { get; set; }

        public int? MaximumLength { get; set; }

        public double? Minimum { get; set; }

        public int? MinimumItems { get; set; }

        public int? MinimumLength { get; set; }

        public IDictionary<string, JsonSchemaModel> PatternProperties { get; set; }

        public IList<string> Patterns { get; set; }

        public IDictionary<string, JsonSchemaModel> Properties { get; set; }

        public bool Required { get; set; }

        public JsonSchemaType Type { get; set; }
    }
}

