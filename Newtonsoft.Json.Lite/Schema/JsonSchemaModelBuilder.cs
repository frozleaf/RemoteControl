namespace Newtonsoft.Json.Schema
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    internal class JsonSchemaModelBuilder
    {
        private JsonSchemaNode _node;
        private Dictionary<JsonSchemaNode, JsonSchemaModel> _nodeModels = new Dictionary<JsonSchemaNode, JsonSchemaModel>();
        private JsonSchemaNodeCollection _nodes = new JsonSchemaNodeCollection();

        public void AddAdditionalProperties(JsonSchemaNode parentNode, JsonSchema schema)
        {
            parentNode.AdditionalProperties = this.AddSchema(parentNode.AdditionalProperties, schema);
        }

        public void AddItem(JsonSchemaNode parentNode, int index, JsonSchema schema)
        {
            JsonSchemaNode existingNode = (parentNode.Items.Count > index) ? parentNode.Items[index] : null;
            JsonSchemaNode item = this.AddSchema(existingNode, schema);
            if (parentNode.Items.Count <= index)
            {
                parentNode.Items.Add(item);
            }
            else
            {
                parentNode.Items[index] = item;
            }
        }

        public void AddProperties(IDictionary<string, JsonSchema> source, IDictionary<string, JsonSchemaNode> target)
        {
            if (source != null)
            {
                foreach (KeyValuePair<string, JsonSchema> pair in source)
                {
                    this.AddProperty(target, pair.Key, pair.Value);
                }
            }
        }

        public void AddProperty(IDictionary<string, JsonSchemaNode> target, string propertyName, JsonSchema schema)
        {
            JsonSchemaNode node;
            target.TryGetValue(propertyName, out node);
            target[propertyName] = this.AddSchema(node, schema);
        }

        public JsonSchemaNode AddSchema(JsonSchemaNode existingNode, JsonSchema schema)
        {
            string id;
            if (existingNode != null)
            {
                if (existingNode.Schemas.Contains(schema))
                {
                    return existingNode;
                }
                id = JsonSchemaNode.GetId(existingNode.Schemas.Union<JsonSchema>(new JsonSchema[] { schema }));
            }
            else
            {
                id = JsonSchemaNode.GetId(new JsonSchema[] { schema });
            }
            if (this._nodes.Contains(id))
            {
                return this._nodes[id];
            }
            JsonSchemaNode item = (existingNode != null) ? existingNode.Combine(schema) : new JsonSchemaNode(schema);
            this._nodes.Add(item);
            this.AddProperties(schema.Properties, item.Properties);
            this.AddProperties(schema.PatternProperties, item.PatternProperties);
            if (schema.Items != null)
            {
                for (int i = 0; i < schema.Items.Count; i++)
                {
                    this.AddItem(item, i, schema.Items[i]);
                }
            }
            if (schema.AdditionalProperties != null)
            {
                this.AddAdditionalProperties(item, schema.AdditionalProperties);
            }
            if (schema.Extends != null)
            {
                item = this.AddSchema(item, schema.Extends);
            }
            return item;
        }

        public JsonSchemaModel Build(JsonSchema schema)
        {
            this._nodes = new JsonSchemaNodeCollection();
            this._node = this.AddSchema(null, schema);
            this._nodeModels = new Dictionary<JsonSchemaNode, JsonSchemaModel>();
            return this.BuildNodeModel(this._node);
        }

        private JsonSchemaModel BuildNodeModel(JsonSchemaNode node)
        {
            JsonSchemaModel model;
            if (!this._nodeModels.TryGetValue(node, out model))
            {
                model = JsonSchemaModel.Create(node.Schemas);
                this._nodeModels[node] = model;
                foreach (KeyValuePair<string, JsonSchemaNode> pair in node.Properties)
                {
                    if (model.Properties == null)
                    {
                        model.Properties = new Dictionary<string, JsonSchemaModel>();
                    }
                    model.Properties[pair.Key] = this.BuildNodeModel(pair.Value);
                }
                foreach (KeyValuePair<string, JsonSchemaNode> pair in node.PatternProperties)
                {
                    if (model.PatternProperties == null)
                    {
                        model.PatternProperties = new Dictionary<string, JsonSchemaModel>();
                    }
                    model.PatternProperties[pair.Key] = this.BuildNodeModel(pair.Value);
                }
                for (int i = 0; i < node.Items.Count; i++)
                {
                    if (model.Items == null)
                    {
                        model.Items = new List<JsonSchemaModel>();
                    }
                    model.Items.Add(this.BuildNodeModel(node.Items[i]));
                }
                if (node.AdditionalProperties != null)
                {
                    model.AdditionalProperties = this.BuildNodeModel(node.AdditionalProperties);
                }
            }
            return model;
        }
    }
}

