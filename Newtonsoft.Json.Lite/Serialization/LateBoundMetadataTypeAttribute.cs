namespace Newtonsoft.Json.Serialization
{
    using Newtonsoft.Json.Utilities;
    using System;
    using System.Reflection;

    internal class LateBoundMetadataTypeAttribute : IMetadataTypeAttribute
    {
        private readonly object _attribute;
        private static PropertyInfo _metadataClassTypeProperty;

        public LateBoundMetadataTypeAttribute(object attribute)
        {
            this._attribute = attribute;
        }

        public Type MetadataClassType
        {
            get
            {
                if (_metadataClassTypeProperty == null)
                {
                    _metadataClassTypeProperty = this._attribute.GetType().GetProperty("MetadataClassType");
                }
                return (Type) ReflectionUtils.GetMemberValue(_metadataClassTypeProperty, this._attribute);
            }
        }
    }
}

