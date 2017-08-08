namespace Newtonsoft.Json.Serialization
{
    using Newtonsoft.Json;
    using Newtonsoft.Json.Utilities;
    using System;
    using System.Globalization;
    using System.Reflection;

    public class ReflectionValueProvider : IValueProvider
    {
        private readonly MemberInfo _memberInfo;

        public ReflectionValueProvider(MemberInfo memberInfo)
        {
            ValidationUtils.ArgumentNotNull(memberInfo, "memberInfo");
            this._memberInfo = memberInfo;
        }

        public object GetValue(object target)
        {
            object memberValue;
            try
            {
                memberValue = ReflectionUtils.GetMemberValue(this._memberInfo, target);
            }
            catch (Exception exception)
            {
                throw new JsonSerializationException("Error getting value from '{0}' on '{1}'.".FormatWith(CultureInfo.InvariantCulture, new object[] { this._memberInfo.Name, target.GetType() }), exception);
            }
            return memberValue;
        }

        public void SetValue(object target, object value)
        {
            try
            {
                ReflectionUtils.SetMemberValue(this._memberInfo, target, value);
            }
            catch (Exception exception)
            {
                throw new JsonSerializationException("Error setting value to '{0}' on '{1}'.".FormatWith(CultureInfo.InvariantCulture, new object[] { this._memberInfo.Name, target.GetType() }), exception);
            }
        }
    }
}

