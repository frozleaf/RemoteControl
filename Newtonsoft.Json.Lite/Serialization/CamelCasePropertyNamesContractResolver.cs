namespace Newtonsoft.Json.Serialization
{
    using Newtonsoft.Json.Utilities;
    using System;

    public class CamelCasePropertyNamesContractResolver : DefaultContractResolver
    {
        public CamelCasePropertyNamesContractResolver() : base(true)
        {
        }

        protected override string ResolvePropertyName(string propertyName)
        {
            return StringUtils.ToCamelCase(propertyName);
        }
    }
}

