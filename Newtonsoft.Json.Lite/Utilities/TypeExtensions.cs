namespace Newtonsoft.Json.Utilities
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using System.Reflection;
    using System.Runtime.CompilerServices;
    using System.Threading;

    internal static class TypeExtensions
    {
        public static IEnumerable<Type> AllInterfaces(this Type target)
        {
            foreach (Type iteratorVariable0 in target.GetInterfaces())
            {
                yield return iteratorVariable0;
                foreach (Type iteratorVariable1 in iteratorVariable0.AllInterfaces())
                {
                    yield return iteratorVariable1;
                }
            }
        }

        public static IEnumerable<MethodInfo> AllMethods(this Type target)
        {
            List<Type> list = target.AllInterfaces().ToList<Type>();
            list.Add(target);
            return (from type in list
                from method in type.GetMethods()
                select method);
        }

        public static MethodInfo GetGenericMethod(this Type type, string name, params Type[] parameterTypes)
        {
            IEnumerable<MethodInfo> enumerable = from method in type.GetMethods()
                where method.Name == name
                select method;
            foreach (MethodInfo info in enumerable)
            {
                if (info.HasParameters(parameterTypes))
                {
                    return info;
                }
            }
            return null;
        }

        public static bool HasParameters(this MethodInfo method, params Type[] parameterTypes)
        {
            Type[] typeArray = (from parameter in method.GetParameters() select parameter.ParameterType).ToArray<Type>();
            if (typeArray.Length != parameterTypes.Length)
            {
                return false;
            }
            for (int i = 0; i < typeArray.Length; i++)
            {
                if (typeArray[i].ToString() != parameterTypes[i].ToString())
                {
                    return false;
                }
            }
            return true;
        }

    }
}

