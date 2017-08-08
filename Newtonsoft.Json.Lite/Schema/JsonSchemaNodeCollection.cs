namespace Newtonsoft.Json.Schema
{
    using System;
    using System.Collections.ObjectModel;

    internal class JsonSchemaNodeCollection : KeyedCollection<string, JsonSchemaNode>
    {
        protected override string GetKeyForItem(JsonSchemaNode item)
        {
            return item.Id;
        }
    }
}

