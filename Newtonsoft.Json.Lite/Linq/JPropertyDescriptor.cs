namespace Newtonsoft.Json.Linq
{
    using Newtonsoft.Json.Utilities;
    using System;
    using System.ComponentModel;

    public class JPropertyDescriptor : PropertyDescriptor
    {
        private readonly Type _propertyType;

        public JPropertyDescriptor(string name, Type propertyType) : base(name, null)
        {
            ValidationUtils.ArgumentNotNull(name, "name");
            ValidationUtils.ArgumentNotNull(propertyType, "propertyType");
            this._propertyType = propertyType;
        }

        public override bool CanResetValue(object component)
        {
            return false;
        }

        private static JObject CastInstance(object instance)
        {
            return (JObject) instance;
        }

        public override object GetValue(object component)
        {
            return CastInstance(component)[this.Name];
        }

        public override void ResetValue(object component)
        {
        }

        public override void SetValue(object component, object value)
        {
            JToken token = (value is JToken) ? ((JToken) value) : new JValue(value);
            CastInstance(component)[this.Name] = token;
        }

        public override bool ShouldSerializeValue(object component)
        {
            return false;
        }

        public override Type ComponentType
        {
            get
            {
                return typeof(JObject);
            }
        }

        public override bool IsReadOnly
        {
            get
            {
                return false;
            }
        }

        protected override int NameHashCode
        {
            get
            {
                return base.NameHashCode;
            }
        }

        public override Type PropertyType
        {
            get
            {
                return this._propertyType;
            }
        }
    }
}

