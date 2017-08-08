namespace Newtonsoft.Json.Schema
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Runtime.CompilerServices;

    public class JsonSchemaResolver
    {
        public JsonSchemaResolver()
        {
            this.LoadedSchemas = new List<JsonSchema>();
        }

        public virtual JsonSchema GetSchema(string id)
        {
            return this.LoadedSchemas.SingleOrDefault<JsonSchema>(s => (s.Id == id));
        }

        public IList<JsonSchema> LoadedSchemas { get; protected set; }
    }
}

