namespace Newtonsoft.Json.Utilities
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Reflection;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;
    using System.Runtime.Serialization.Formatters;
    using System.Text;

    internal static class ReflectionUtils
    {
        public static bool AssignableToTypeName(this Type type, string fullTypeName)
        {
            Type type2;
            return type.AssignableToTypeName(fullTypeName, out type2);
        }

        public static bool AssignableToTypeName(this Type type, string fullTypeName, out Type match)
        {
            for (Type type2 = type; type2 != null; type2 = type2.BaseType)
            {
                if (string.Equals(type2.FullName, fullTypeName, StringComparison.Ordinal))
                {
                    match = type2;
                    return true;
                }
            }
            foreach (Type type3 in type.GetInterfaces())
            {
                if (string.Equals(type3.Name, fullTypeName, StringComparison.Ordinal))
                {
                    match = type;
                    return true;
                }
            }
            match = null;
            return false;
        }

        public static bool CanReadMemberValue(MemberInfo member, bool nonPublic)
        {
            MemberTypes memberType = member.MemberType;
            if (memberType != MemberTypes.Field)
            {
                if (memberType != MemberTypes.Property)
                {
                    return false;
                }
            }
            else
            {
                FieldInfo info = (FieldInfo) member;
                return (nonPublic || info.IsPublic);
            }
            PropertyInfo info2 = (PropertyInfo) member;
            if (!info2.CanRead)
            {
                return false;
            }
            return (nonPublic || (info2.GetGetMethod(nonPublic) != null));
        }

        public static bool CanSetMemberValue(MemberInfo member, bool nonPublic)
        {
            MemberTypes memberType = member.MemberType;
            if (memberType != MemberTypes.Field)
            {
                if (memberType != MemberTypes.Property)
                {
                    return false;
                }
            }
            else
            {
                FieldInfo info = (FieldInfo) member;
                if (!info.IsInitOnly)
                {
                    if (nonPublic)
                    {
                        return true;
                    }
                    if (info.IsPublic)
                    {
                        return true;
                    }
                }
                return false;
            }
            PropertyInfo info2 = (PropertyInfo) member;
            if (!info2.CanWrite)
            {
                return false;
            }
            return (nonPublic || (info2.GetSetMethod(nonPublic) != null));
        }

        public static object CreateGeneric(Type genericTypeDefinition, IList<Type> innerTypes, params object[] args)
        {
            return CreateGeneric(genericTypeDefinition, innerTypes, (t, a) => CreateInstance(t, a.ToArray<object>()), args);
        }

        public static object CreateGeneric(Type genericTypeDefinition, Type innerType, params object[] args)
        {
            return CreateGeneric(genericTypeDefinition, new Type[] { innerType }, args);
        }

        public static object CreateGeneric(Type genericTypeDefinition, IList<Type> innerTypes, Func<Type, IList<object>, object> instanceCreator, params object[] args)
        {
            ValidationUtils.ArgumentNotNull(genericTypeDefinition, "genericTypeDefinition");
            ValidationUtils.ArgumentNotNullOrEmpty<Type>(innerTypes, "innerTypes");
            ValidationUtils.ArgumentNotNull(instanceCreator, "createInstance");
            Type type = MakeGenericType(genericTypeDefinition, innerTypes.ToArray<Type>());
            return instanceCreator(type, args);
        }

        public static object CreateInstance(Type type, params object[] args)
        {
            ValidationUtils.ArgumentNotNull(type, "type");
            return Activator.CreateInstance(type, args);
        }

        public static object CreateUnitializedValue(Type type)
        {
            ValidationUtils.ArgumentNotNull(type, "type");
            if (type.IsGenericTypeDefinition)
            {
                throw new ArgumentException("Type {0} is a generic type definition and cannot be instantiated.".FormatWith(CultureInfo.InvariantCulture, new object[] { type }), "type");
            }
            if ((type.IsClass || type.IsInterface) || (type == typeof(void)))
            {
                return null;
            }
            if (!type.IsValueType)
            {
                throw new ArgumentException("Type {0} cannot be instantiated.".FormatWith(CultureInfo.InvariantCulture, new object[] { type }), "type");
            }
            return Activator.CreateInstance(type);
        }

        public static Type EnsureNotNullableType(Type t)
        {
            return (IsNullableType(t) ? Nullable.GetUnderlyingType(t) : t);
        }

        private static int? GetAssemblyDelimiterIndex(string fullyQualifiedTypeName)
        {
            int num = 0;
            for (int i = 0; i < fullyQualifiedTypeName.Length; i++)
            {
                switch (fullyQualifiedTypeName[i])
                {
                    case '[':
                        num++;
                        break;

                    case ']':
                        num--;
                        break;

                    case ',':
                        if (num == 0)
                        {
                            return new int?(i);
                        }
                        break;
                }
            }
            return null;
        }

        public static T GetAttribute<T>(ICustomAttributeProvider attributeProvider) where T: Attribute
        {
            return GetAttribute<T>(attributeProvider, true);
        }

        public static T GetAttribute<T>(ICustomAttributeProvider attributeProvider, bool inherit) where T: Attribute
        {
            return CollectionUtils.GetSingleItem<T>(GetAttributes<T>(attributeProvider, inherit), true);
        }

        public static T[] GetAttributes<T>(ICustomAttributeProvider attributeProvider, bool inherit) where T: Attribute
        {
            ValidationUtils.ArgumentNotNull(attributeProvider, "attributeProvider");
            if (attributeProvider is Type)
            {
                return (T[]) ((Type) attributeProvider).GetCustomAttributes(typeof(T), inherit);
            }
            if (attributeProvider is Assembly)
            {
                return (T[]) Attribute.GetCustomAttributes((Assembly) attributeProvider, typeof(T), inherit);
            }
            if (attributeProvider is MemberInfo)
            {
                return (T[]) Attribute.GetCustomAttributes((MemberInfo) attributeProvider, typeof(T), inherit);
            }
            if (attributeProvider is Module)
            {
                return (T[]) Attribute.GetCustomAttributes((Module) attributeProvider, typeof(T), inherit);
            }
            if (attributeProvider is ParameterInfo)
            {
                return (T[]) Attribute.GetCustomAttributes((ParameterInfo) attributeProvider, typeof(T), inherit);
            }
            return (T[]) attributeProvider.GetCustomAttributes(typeof(T), inherit);
        }

        private static void GetChildPrivateFields(IList<MemberInfo> initialFields, Type targetType, BindingFlags bindingAttr)
        {
            if ((bindingAttr & BindingFlags.NonPublic) != BindingFlags.Default)
            {
                BindingFlags flags = bindingAttr.RemoveFlag(BindingFlags.Public);
                while ((targetType = targetType.BaseType) != null)
                {
                    IEnumerable<MemberInfo> collection = (from f in targetType.GetFields(flags)
                        where f.IsPrivate
                        select f).Cast<MemberInfo>();
                    initialFields.AddRange<MemberInfo>(collection);
                }
            }
        }

        private static void GetChildPrivateProperties(IList<PropertyInfo> initialProperties, Type targetType, BindingFlags bindingAttr)
        {
            if ((bindingAttr & BindingFlags.NonPublic) != BindingFlags.Default)
            {
                BindingFlags flags = bindingAttr.RemoveFlag(BindingFlags.Public);
                while ((targetType = targetType.BaseType) != null)
                {
                    foreach (PropertyInfo info in targetType.GetProperties(flags))
                    {
                        PropertyInfo nonPublicProperty = info;
                        int index = initialProperties.IndexOf<PropertyInfo>(p => p.Name == nonPublicProperty.Name);
                        if (index == -1)
                        {
                            initialProperties.Add(nonPublicProperty);
                        }
                        else
                        {
                            initialProperties[index] = nonPublicProperty;
                        }
                    }
                }
            }
        }

        public static Type GetCollectionItemType(Type type)
        {
            Type type2;
            ValidationUtils.ArgumentNotNull(type, "type");
            if (type.IsArray)
            {
                return type.GetElementType();
            }
            if (ImplementsGenericDefinition(type, typeof(IEnumerable<>), out type2))
            {
                if (type2.IsGenericTypeDefinition)
                {
                    throw new Exception("Type {0} is not a collection.".FormatWith(CultureInfo.InvariantCulture, new object[] { type }));
                }
                return type2.GetGenericArguments()[0];
            }
            if (!typeof(IEnumerable).IsAssignableFrom(type))
            {
                throw new Exception("Type {0} is not a collection.".FormatWith(CultureInfo.InvariantCulture, new object[] { type }));
            }
            return null;
        }

        public static ConstructorInfo GetDefaultConstructor(Type t)
        {
            return GetDefaultConstructor(t, false);
        }

        public static ConstructorInfo GetDefaultConstructor(Type t, bool nonPublic)
        {
            BindingFlags @public = BindingFlags.Public;
            if (nonPublic)
            {
                @public |= BindingFlags.NonPublic;
            }
            return t.GetConstructor(@public | BindingFlags.Instance, null, new Type[0], null);
        }

        public static Type GetDictionaryKeyType(Type dictionaryType)
        {
            Type type;
            Type type2;
            GetDictionaryKeyValueTypes(dictionaryType, out type, out type2);
            return type;
        }

        public static void GetDictionaryKeyValueTypes(Type dictionaryType, out Type keyType, out Type valueType)
        {
            Type type;
            ValidationUtils.ArgumentNotNull(dictionaryType, "type");
            if (ImplementsGenericDefinition(dictionaryType, typeof(IDictionary<,>), out type))
            {
                if (type.IsGenericTypeDefinition)
                {
                    throw new Exception("Type {0} is not a dictionary.".FormatWith(CultureInfo.InvariantCulture, new object[] { dictionaryType }));
                }
                Type[] genericArguments = type.GetGenericArguments();
                keyType = genericArguments[0];
                valueType = genericArguments[1];
            }
            else
            {
                if (!typeof(IDictionary).IsAssignableFrom(dictionaryType))
                {
                    throw new Exception("Type {0} is not a dictionary.".FormatWith(CultureInfo.InvariantCulture, new object[] { dictionaryType }));
                }
                keyType = null;
                valueType = null;
            }
        }

        public static Type GetDictionaryValueType(Type dictionaryType)
        {
            Type type;
            Type type2;
            GetDictionaryKeyValueTypes(dictionaryType, out type, out type2);
            return type2;
        }

        public static IEnumerable<FieldInfo> GetFields(Type targetType, BindingFlags bindingAttr)
        {
            ValidationUtils.ArgumentNotNull(targetType, "targetType");
            List<MemberInfo> initialFields = new List<MemberInfo>(targetType.GetFields(bindingAttr));
            GetChildPrivateFields(initialFields, targetType, bindingAttr);
            return initialFields.Cast<FieldInfo>();
        }

        public static List<MemberInfo> GetFieldsAndProperties<T>(BindingFlags bindingAttr)
        {
            return GetFieldsAndProperties(typeof(T), bindingAttr);
        }

        public static List<MemberInfo> GetFieldsAndProperties(Type type, BindingFlags bindingAttr)
        {
            Func<MemberInfo, bool> predicate = null;
            List<MemberInfo> initial = new List<MemberInfo>();
            initial.AddRange(GetFields(type, bindingAttr));
            initial.AddRange(GetProperties(type, bindingAttr));
            List<MemberInfo> list2 = new List<MemberInfo>(initial.Count);
            var enumerable = from m in initial
                group m by m.Name into g
                select new { 
                    Count = g.Count<MemberInfo>(),
                    Members = g.Cast<MemberInfo>()
                };
            foreach (var type2 in enumerable)
            {
                if (type2.Count == 1)
                {
                    list2.Add(type2.Members.First<MemberInfo>());
                }
                else
                {
                    if (predicate == null)
                    {
                        predicate = m => !IsOverridenGenericMember(m, bindingAttr) || (m.Name == "Item");
                    }
                    IEnumerable<MemberInfo> collection = type2.Members.Where<MemberInfo>(predicate);
                    list2.AddRange(collection);
                }
            }
            return list2;
        }

        public static MemberInfo GetMemberInfoFromType(Type targetType, MemberInfo memberInfo)
        {
            BindingFlags bindingAttr = BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Static | BindingFlags.Instance;
            if (memberInfo.MemberType == MemberTypes.Property)
            {
                PropertyInfo info = (PropertyInfo) memberInfo;
                Type[] types = (from p in info.GetIndexParameters() select p.ParameterType).ToArray<Type>();
                return targetType.GetProperty(info.Name, bindingAttr, null, info.PropertyType, types, null);
            }
            return targetType.GetMember(memberInfo.Name, memberInfo.MemberType, bindingAttr).SingleOrDefault<MemberInfo>();
        }

        public static Type GetMemberUnderlyingType(MemberInfo member)
        {
            ValidationUtils.ArgumentNotNull(member, "member");
            switch (member.MemberType)
            {
                case MemberTypes.Event:
                    return ((EventInfo) member).EventHandlerType;

                case MemberTypes.Field:
                    return ((FieldInfo) member).FieldType;

                case MemberTypes.Property:
                    return ((PropertyInfo) member).PropertyType;
            }
            throw new ArgumentException("MemberInfo must be of type FieldInfo, PropertyInfo or EventInfo", "member");
        }

        public static object GetMemberValue(MemberInfo member, object target)
        {
            ValidationUtils.ArgumentNotNull(member, "member");
            ValidationUtils.ArgumentNotNull(target, "target");
            switch (member.MemberType)
            {
                case MemberTypes.Field:
                    return ((FieldInfo) member).GetValue(target);

                case MemberTypes.Property:
                    try
                    {
                        return ((PropertyInfo) member).GetValue(target, null);
                    }
                    catch (TargetParameterCountException exception)
                    {
                        throw new ArgumentException("MemberInfo '{0}' has index parameters".FormatWith(CultureInfo.InvariantCulture, new object[] { member.Name }), exception);
                    }
                    break;
            }
            throw new ArgumentException("MemberInfo '{0}' is not of type FieldInfo or PropertyInfo".FormatWith(CultureInfo.InvariantCulture, new object[] { CultureInfo.InvariantCulture, member.Name }), "member");
        }

        public static string GetNameAndAssessmblyName(Type t)
        {
            ValidationUtils.ArgumentNotNull(t, "t");
            return (t.FullName + ", " + t.Assembly.GetName().Name);
        }

        public static Type GetObjectType(object v)
        {
            return ((v != null) ? v.GetType() : null);
        }

        public static IEnumerable<PropertyInfo> GetProperties(Type targetType, BindingFlags bindingAttr)
        {
            ValidationUtils.ArgumentNotNull(targetType, "targetType");
            List<PropertyInfo> initialProperties = new List<PropertyInfo>(targetType.GetProperties(bindingAttr));
            GetChildPrivateProperties(initialProperties, targetType, bindingAttr);
            for (int i = 0; i < initialProperties.Count; i++)
            {
                PropertyInfo memberInfo = initialProperties[i];
                if (memberInfo.DeclaringType != targetType)
                {
                    PropertyInfo memberInfoFromType = (PropertyInfo) GetMemberInfoFromType(memberInfo.DeclaringType, memberInfo);
                    initialProperties[i] = memberInfoFromType;
                }
            }
            return initialProperties;
        }

        private static string GetSimpleTypeName(Type type)
        {
            string str = type.FullName + ", " + type.Assembly.GetName().Name;
            if (!(type.IsGenericType && !type.IsGenericTypeDefinition))
            {
                return str;
            }
            StringBuilder builder = new StringBuilder();
            bool flag = false;
            bool flag2 = false;
            for (int i = 0; i < str.Length; i++)
            {
                char ch = str[i];
                switch (ch)
                {
                    case '[':
                    {
                        flag = false;
                        flag2 = false;
                        builder.Append(ch);
                        continue;
                    }
                    case ']':
                    {
                        flag = false;
                        flag2 = false;
                        builder.Append(ch);
                        continue;
                    }
                    case ',':
                    {
                        if (!flag)
                        {
                            flag = true;
                            builder.Append(ch);
                        }
                        else
                        {
                            flag2 = true;
                        }
                        continue;
                    }
                }
                if (!flag2)
                {
                    builder.Append(ch);
                }
            }
            return builder.ToString();
        }

        public static string GetTypeName(Type t, FormatterAssemblyStyle assemblyFormat)
        {
            switch (assemblyFormat)
            {
                case FormatterAssemblyStyle.Simple:
                    return GetSimpleTypeName(t);

                case FormatterAssemblyStyle.Full:
                    return t.AssemblyQualifiedName;
            }
            throw new ArgumentOutOfRangeException();
        }

        public static bool HasDefaultConstructor(Type t)
        {
            return HasDefaultConstructor(t, false);
        }

        public static bool HasDefaultConstructor(Type t, bool nonPublic)
        {
            ValidationUtils.ArgumentNotNull(t, "t");
            return (t.IsValueType || (GetDefaultConstructor(t, nonPublic) != null));
        }

        public static bool ImplementsGenericDefinition(Type type, Type genericInterfaceDefinition)
        {
            Type type2;
            return ImplementsGenericDefinition(type, genericInterfaceDefinition, out type2);
        }

        public static bool ImplementsGenericDefinition(Type type, Type genericInterfaceDefinition, out Type implementingType)
        {
            Type genericTypeDefinition;
            ValidationUtils.ArgumentNotNull(type, "type");
            ValidationUtils.ArgumentNotNull(genericInterfaceDefinition, "genericInterfaceDefinition");
            if (!(genericInterfaceDefinition.IsInterface && genericInterfaceDefinition.IsGenericTypeDefinition))
            {
                throw new ArgumentNullException("'{0}' is not a generic interface definition.".FormatWith(CultureInfo.InvariantCulture, new object[] { genericInterfaceDefinition }));
            }
            if (type.IsInterface && type.IsGenericType)
            {
                genericTypeDefinition = type.GetGenericTypeDefinition();
                if (genericInterfaceDefinition == genericTypeDefinition)
                {
                    implementingType = type;
                    return true;
                }
            }
            foreach (Type type3 in type.GetInterfaces())
            {
                if (type3.IsGenericType)
                {
                    genericTypeDefinition = type3.GetGenericTypeDefinition();
                    if (genericInterfaceDefinition == genericTypeDefinition)
                    {
                        implementingType = type3;
                        return true;
                    }
                }
            }
            implementingType = null;
            return false;
        }

        public static bool InheritsGenericDefinition(Type type, Type genericClassDefinition)
        {
            Type type2;
            return InheritsGenericDefinition(type, genericClassDefinition, out type2);
        }

        public static bool InheritsGenericDefinition(Type type, Type genericClassDefinition, out Type implementingType)
        {
            ValidationUtils.ArgumentNotNull(type, "type");
            ValidationUtils.ArgumentNotNull(genericClassDefinition, "genericClassDefinition");
            if (!(genericClassDefinition.IsClass && genericClassDefinition.IsGenericTypeDefinition))
            {
                throw new ArgumentNullException("'{0}' is not a generic class definition.".FormatWith(CultureInfo.InvariantCulture, new object[] { genericClassDefinition }));
            }
            return InheritsGenericDefinitionInternal(type, genericClassDefinition, out implementingType);
        }

        private static bool InheritsGenericDefinitionInternal(Type currentType, Type genericClassDefinition, out Type implementingType)
        {
            if (currentType.IsGenericType)
            {
                Type genericTypeDefinition = currentType.GetGenericTypeDefinition();
                if (genericClassDefinition == genericTypeDefinition)
                {
                    implementingType = currentType;
                    return true;
                }
            }
            if (currentType.BaseType == null)
            {
                implementingType = null;
                return false;
            }
            return InheritsGenericDefinitionInternal(currentType.BaseType, genericClassDefinition, out implementingType);
        }

        public static bool IsCompatibleValue(object value, Type type)
        {
            if (value == null)
            {
                return IsNullable(type);
            }
            return type.IsAssignableFrom(value.GetType());
        }

        public static bool IsIndexedProperty(MemberInfo member)
        {
            ValidationUtils.ArgumentNotNull(member, "member");
            PropertyInfo property = member as PropertyInfo;
            return ((property != null) && IsIndexedProperty(property));
        }

        public static bool IsIndexedProperty(PropertyInfo property)
        {
            ValidationUtils.ArgumentNotNull(property, "property");
            return (property.GetIndexParameters().Length > 0);
        }

        public static bool IsInstantiatableType(Type t)
        {
            ValidationUtils.ArgumentNotNull(t, "t");
            if (((t.IsAbstract || t.IsInterface) || (t.IsArray || t.IsGenericTypeDefinition)) || (t == typeof(void)))
            {
                return false;
            }
            if (!HasDefaultConstructor(t))
            {
                return false;
            }
            return true;
        }

        public static bool IsNullable(Type t)
        {
            ValidationUtils.ArgumentNotNull(t, "t");
            if (t.IsValueType)
            {
                return IsNullableType(t);
            }
            return true;
        }

        public static bool IsNullableType(Type t)
        {
            ValidationUtils.ArgumentNotNull(t, "t");
            return (t.IsGenericType && (t.GetGenericTypeDefinition() == typeof(Nullable<>)));
        }

        private static bool IsOverridenGenericMember(MemberInfo memberInfo, BindingFlags bindingAttr)
        {
            if ((memberInfo.MemberType != MemberTypes.Field) && (memberInfo.MemberType != MemberTypes.Property))
            {
                throw new ArgumentException("Member must be a field or property.");
            }
            Type declaringType = memberInfo.DeclaringType;
            if (!declaringType.IsGenericType)
            {
                return false;
            }
            Type genericTypeDefinition = declaringType.GetGenericTypeDefinition();
            if (genericTypeDefinition == null)
            {
                return false;
            }
            MemberInfo[] member = genericTypeDefinition.GetMember(memberInfo.Name, bindingAttr);
            if (member.Length == 0)
            {
                return false;
            }
            if (!GetMemberUnderlyingType(member[0]).IsGenericParameter)
            {
                return false;
            }
            return true;
        }

        public static bool IsPropertyIndexed(PropertyInfo property)
        {
            ValidationUtils.ArgumentNotNull(property, "property");
            return !CollectionUtils.IsNullOrEmpty<ParameterInfo>(property.GetIndexParameters());
        }

        public static bool IsUnitializedValue(object value)
        {
            if (value == null)
            {
                return true;
            }
            object obj2 = CreateUnitializedValue(value.GetType());
            return value.Equals(obj2);
        }

        public static bool IsVirtual(this PropertyInfo propertyInfo)
        {
            ValidationUtils.ArgumentNotNull(propertyInfo, "propertyInfo");
            MethodInfo getMethod = propertyInfo.GetGetMethod();
            if ((getMethod != null) && getMethod.IsVirtual)
            {
                return true;
            }
            getMethod = propertyInfo.GetSetMethod();
            return ((getMethod != null) && getMethod.IsVirtual);
        }

        public static bool ItemsUnitializedValue<T>(IList<T> list)
        {
            int num;
            ValidationUtils.ArgumentNotNull(list, "list");
            Type collectionItemType = GetCollectionItemType(list.GetType());
            if (collectionItemType.IsValueType)
            {
                object obj2 = CreateUnitializedValue(collectionItemType);
                for (num = 0; num < list.Count; num++)
                {
                    T local = list[num];
                    if (!local.Equals(obj2))
                    {
                        return false;
                    }
                }
            }
            else
            {
                if (!collectionItemType.IsClass)
                {
                    throw new Exception("Type {0} is neither a ValueType or a Class.".FormatWith(CultureInfo.InvariantCulture, new object[] { collectionItemType }));
                }
                for (num = 0; num < list.Count; num++)
                {
                    object obj3 = list[num];
                    if (obj3 != null)
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        public static Type MakeGenericType(Type genericTypeDefinition, params Type[] innerTypes)
        {
            ValidationUtils.ArgumentNotNull(genericTypeDefinition, "genericTypeDefinition");
            ValidationUtils.ArgumentNotNullOrEmpty<Type>(innerTypes, "innerTypes");
            ValidationUtils.ArgumentConditionTrue(genericTypeDefinition.IsGenericTypeDefinition, "genericTypeDefinition", "Type {0} is not a generic type definition.".FormatWith(CultureInfo.InvariantCulture, new object[] { genericTypeDefinition }));
            return genericTypeDefinition.MakeGenericType(innerTypes);
        }

        public static BindingFlags RemoveFlag(this BindingFlags bindingAttr, BindingFlags flag)
        {
            return (((bindingAttr & flag) == flag) ? (bindingAttr ^ flag) : bindingAttr);
        }

        public static void SetMemberValue(MemberInfo member, object target, object value)
        {
            ValidationUtils.ArgumentNotNull(member, "member");
            ValidationUtils.ArgumentNotNull(target, "target");
            MemberTypes memberType = member.MemberType;
            if (memberType != MemberTypes.Field)
            {
                if (memberType != MemberTypes.Property)
                {
                    throw new ArgumentException("MemberInfo '{0}' must be of type FieldInfo or PropertyInfo".FormatWith(CultureInfo.InvariantCulture, new object[] { member.Name }), "member");
                }
                ((PropertyInfo) member).SetValue(target, value, null);
            }
            else
            {
                ((FieldInfo) member).SetValue(target, value);
            }
        }

        public static void SplitFullyQualifiedTypeName(string fullyQualifiedTypeName, out string typeName, out string assemblyName)
        {
            int? assemblyDelimiterIndex = GetAssemblyDelimiterIndex(fullyQualifiedTypeName);
            if (assemblyDelimiterIndex.HasValue)
            {
                typeName = fullyQualifiedTypeName.Substring(0, assemblyDelimiterIndex.Value).Trim();
                assemblyName = fullyQualifiedTypeName.Substring(assemblyDelimiterIndex.Value + 1, (fullyQualifiedTypeName.Length - assemblyDelimiterIndex.Value) - 1).Trim();
            }
            else
            {
                typeName = fullyQualifiedTypeName;
                assemblyName = null;
            }
        }
    }
}

