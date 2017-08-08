using System;
using System.Reflection;

namespace Newtonsoft.Json.Utilities
{
    internal class LateBoundReflectionDelegateFactory : ReflectionDelegateFactory
    {
        public static readonly LateBoundReflectionDelegateFactory Instance = new LateBoundReflectionDelegateFactory();

        public override MethodCall<T, object> CreateMethodCall<T>(MethodBase method)
        {
            ValidationUtils.ArgumentNotNull(method, "method");
            ConstructorInfo c = method as ConstructorInfo;
            MethodCall<T, object> result;
            if (c != null)
            {
                result = ((T o, object[] a) => c.Invoke(a));
            }
            else
            {
                result = ((T o, object[] a) => method.Invoke(o, a));
            }
            return result;
        }

        public override Func<T> CreateDefaultConstructor<T>(Type type)
        {
            ValidationUtils.ArgumentNotNull(type, "type");
            Func<T> result;
            if (type.IsValueType)
            {
                result = (() => (T)((object)ReflectionUtils.CreateInstance(type, new object[0])));
            }
            else
            {
                ConstructorInfo constructorInfo = ReflectionUtils.GetDefaultConstructor(type, true);
                result = (() => (T)((object)constructorInfo.Invoke(null)));
            }
            return result;
        }

        public override Func<T, object> CreateGet<T>(PropertyInfo propertyInfo)
        {
            ValidationUtils.ArgumentNotNull(propertyInfo, "propertyInfo");
            return (T o) => propertyInfo.GetValue(o, null);
        }

        public override Func<T, object> CreateGet<T>(FieldInfo fieldInfo)
        {
            ValidationUtils.ArgumentNotNull(fieldInfo, "fieldInfo");
            return (T o) => fieldInfo.GetValue(o);
        }

        public override Action<T, object> CreateSet<T>(FieldInfo fieldInfo)
        {
            ValidationUtils.ArgumentNotNull(fieldInfo, "fieldInfo");
            return delegate(T o, object v)
            {
                fieldInfo.SetValue(o, v);
            };
        }

        public override Action<T, object> CreateSet<T>(PropertyInfo propertyInfo)
        {
            ValidationUtils.ArgumentNotNull(propertyInfo, "propertyInfo");
            return delegate(T o, object v)
            {
                propertyInfo.SetValue(o, v, null);
            };
        }
    }
}
