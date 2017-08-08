namespace Newtonsoft.Json.Serialization
{
    using System;

    public interface IValueProvider
    {
        object GetValue(object target);
        void SetValue(object target, object value);
    }
}

