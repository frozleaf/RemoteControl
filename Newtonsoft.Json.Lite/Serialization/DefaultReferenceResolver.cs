namespace Newtonsoft.Json.Serialization
{
    using Newtonsoft.Json.Utilities;
    using System;
    using System.Globalization;

    internal class DefaultReferenceResolver : IReferenceResolver
    {
        private int _referenceCount;

        public void AddReference(object context, string reference, object value)
        {
            this.GetMappings(context).Add(reference, value);
        }

        private BidirectionalDictionary<string, object> GetMappings(object context)
        {
            JsonSerializerInternalBase internalSerializer;
            if (context is JsonSerializerInternalBase)
            {
                internalSerializer = (JsonSerializerInternalBase) context;
            }
            else
            {
                if (!(context is JsonSerializerProxy))
                {
                    throw new Exception("The DefaultReferenceResolver can only be used internally.");
                }
                internalSerializer = ((JsonSerializerProxy) context).GetInternalSerializer();
            }
            return internalSerializer.DefaultReferenceMappings;
        }

        public string GetReference(object context, object value)
        {
            string str;
            BidirectionalDictionary<string, object> mappings = this.GetMappings(context);
            if (!mappings.TryGetBySecond(value, out str))
            {
                this._referenceCount++;
                str = this._referenceCount.ToString(CultureInfo.InvariantCulture);
                mappings.Add(str, value);
            }
            return str;
        }

        public bool IsReferenced(object context, object value)
        {
            string str;
            return this.GetMappings(context).TryGetBySecond(value, out str);
        }

        public object ResolveReference(object context, string reference)
        {
            object obj2;
            this.GetMappings(context).TryGetByFirst(reference, out obj2);
            return obj2;
        }
    }
}

