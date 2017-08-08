namespace Newtonsoft.Json.Serialization
{
    using Newtonsoft.Json;
    using Newtonsoft.Json.Utilities;
    using System;
    using System.Globalization;
    using System.Reflection;

    public class DynamicValueProvider : IValueProvider
    {
        private Func<object, object> _getter;
        private readonly MemberInfo _memberInfo;
        private Action<object, object> _setter;

        public DynamicValueProvider(MemberInfo memberInfo)
        {
            ValidationUtils.ArgumentNotNull(memberInfo, "memberInfo");
            this._memberInfo = memberInfo;
        }

        public object GetValue(object target)
        {
            object obj2;
            try
            {
                if (this._getter == null)
                {
                    this._getter = DynamicReflectionDelegateFactory.Instance.CreateGet<object>(this._memberInfo);
                }
                obj2 = this._getter(target);
            }
            catch (Exception exception)
            {
                throw new JsonSerializationException("Error getting value from '{0}' on '{1}'.".FormatWith(CultureInfo.InvariantCulture, new object[] { this._memberInfo.Name, target.GetType() }), exception);
            }
            return obj2;
        }

        public void SetValue(object target, object value)
        {
            try
            {
                if (this._setter == null)
                {
                    this._setter = DynamicReflectionDelegateFactory.Instance.CreateSet<object>(this._memberInfo);
                }
                if (value == null)
                {
                    if (!ReflectionUtils.IsNullable(ReflectionUtils.GetMemberUnderlyingType(this._memberInfo)))
                    {
                        throw new Exception("Incompatible value. Cannot set {0} to null.".FormatWith(CultureInfo.InvariantCulture, new object[] { this._memberInfo }));
                    }
                }
                else if (!ReflectionUtils.GetMemberUnderlyingType(this._memberInfo).IsAssignableFrom(value.GetType()))
                {
                    throw new Exception("Incompatible value. Cannot set {0} to type {1}.".FormatWith(CultureInfo.InvariantCulture, new object[] { this._memberInfo, value.GetType() }));
                }
                this._setter(target, value);
            }
            catch (Exception exception)
            {
                throw new JsonSerializationException("Error setting value to '{0}' on '{1}'.".FormatWith(CultureInfo.InvariantCulture, new object[] { this._memberInfo.Name, target.GetType() }), exception);
            }
        }
    }
}

