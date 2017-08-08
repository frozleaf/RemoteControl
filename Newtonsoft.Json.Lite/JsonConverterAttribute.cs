namespace Newtonsoft.Json
{
    using Newtonsoft.Json.Utilities;
    using System;
    using System.Globalization;

    [AttributeUsage(AttributeTargets.Parameter | AttributeTargets.Interface | AttributeTargets.Field | AttributeTargets.Property | AttributeTargets.Enum | AttributeTargets.Struct | AttributeTargets.Class, AllowMultiple=false)]
    public sealed class JsonConverterAttribute : Attribute
    {
        private readonly Type _converterType;

        public JsonConverterAttribute(Type converterType)
        {
            if (converterType == null)
            {
                throw new ArgumentNullException("converterType");
            }
            this._converterType = converterType;
        }

        internal static JsonConverter CreateJsonConverterInstance(Type converterType)
        {
            JsonConverter converter;
            try
            {
                converter = (JsonConverter) Activator.CreateInstance(converterType);
            }
            catch (Exception exception)
            {
                throw new Exception("Error creating {0}".FormatWith(CultureInfo.InvariantCulture, new object[] { converterType }), exception);
            }
            return converter;
        }

        public Type ConverterType
        {
            get
            {
                return this._converterType;
            }
        }
    }
}

