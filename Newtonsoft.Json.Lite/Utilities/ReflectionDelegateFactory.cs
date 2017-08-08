namespace Newtonsoft.Json.Utilities
{
    using System;
    using System.Globalization;
    using System.Reflection;

    internal abstract class ReflectionDelegateFactory
    {
        protected ReflectionDelegateFactory()
        {
        }

        public abstract Func<T> CreateDefaultConstructor<T>(Type type);
        public abstract Func<T, object> CreateGet<T>(FieldInfo fieldInfo);
        public Func<T, object> CreateGet<T>(MemberInfo memberInfo)
        {
            PropertyInfo propertyInfo = memberInfo as PropertyInfo;
            if (propertyInfo != null)
            {
                return this.CreateGet<T>(propertyInfo);
            }
            FieldInfo fieldInfo = memberInfo as FieldInfo;
            if (fieldInfo == null)
            {
                throw new Exception("Could not create getter for {0}.".FormatWith(CultureInfo.InvariantCulture, new object[] { memberInfo }));
            }
            return this.CreateGet<T>(fieldInfo);
        }

        public abstract Func<T, object> CreateGet<T>(PropertyInfo propertyInfo);
        public abstract MethodCall<T, object> CreateMethodCall<T>(MethodBase method);
        public abstract Action<T, object> CreateSet<T>(FieldInfo fieldInfo);
        public Action<T, object> CreateSet<T>(MemberInfo memberInfo)
        {
            PropertyInfo propertyInfo = memberInfo as PropertyInfo;
            if (propertyInfo != null)
            {
                return this.CreateSet<T>(propertyInfo);
            }
            FieldInfo fieldInfo = memberInfo as FieldInfo;
            if (fieldInfo == null)
            {
                throw new Exception("Could not create setter for {0}.".FormatWith(CultureInfo.InvariantCulture, new object[] { memberInfo }));
            }
            return this.CreateSet<T>(fieldInfo);
        }

        public abstract Action<T, object> CreateSet<T>(PropertyInfo propertyInfo);
    }
}

