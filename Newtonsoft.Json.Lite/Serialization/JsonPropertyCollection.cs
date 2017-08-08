namespace Newtonsoft.Json.Serialization
{
    using Newtonsoft.Json;
    using Newtonsoft.Json.Utilities;
    using System;
    using System.Collections.ObjectModel;
    using System.Globalization;

    public class JsonPropertyCollection : KeyedCollection<string, JsonProperty>
    {
        private readonly Type _type;

        public JsonPropertyCollection(Type type)
        {
            ValidationUtils.ArgumentNotNull(type, "type");
            this._type = type;
        }

        public void AddProperty(JsonProperty property)
        {
            if (base.Contains(property.PropertyName))
            {
                if (property.Ignored)
                {
                    return;
                }
                JsonProperty item = base[property.PropertyName];
                if (!item.Ignored)
                {
                    throw new JsonSerializationException("A member with the name '{0}' already exists on '{1}'. Use the JsonPropertyAttribute to specify another name.".FormatWith(CultureInfo.InvariantCulture, new object[] { property.PropertyName, this._type }));
                }
                base.Remove(item);
            }
            base.Add(property);
        }

        public JsonProperty GetClosestMatchProperty(string propertyName)
        {
            JsonProperty property = this.GetProperty(propertyName, StringComparison.Ordinal);
            if (property == null)
            {
                property = this.GetProperty(propertyName, StringComparison.OrdinalIgnoreCase);
            }
            return property;
        }

        protected override string GetKeyForItem(JsonProperty item)
        {
            return item.PropertyName;
        }

        public JsonProperty GetProperty(string propertyName, StringComparison comparisonType)
        {
            foreach (JsonProperty property in this)
            {
                if (string.Equals(propertyName, property.PropertyName, comparisonType))
                {
                    return property;
                }
            }
            return null;
        }
    }
}

