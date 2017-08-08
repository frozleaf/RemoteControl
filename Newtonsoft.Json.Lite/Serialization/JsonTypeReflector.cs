namespace Newtonsoft.Json.Serialization
{
    using Newtonsoft.Json;
    using Newtonsoft.Json.Utilities;
    using System;
    using System.ComponentModel;
    using System.Globalization;
    using System.Linq;
    using System.Reflection;
    using System.Runtime.Serialization;
    using System.Security.Permissions;

    internal static class JsonTypeReflector
    {
        private static Type _cachedMetadataTypeAttributeType;
        private static bool? _dynamicCodeGeneration;
        public const string ArrayValuesPropertyName = "$values";
        private static readonly ThreadSafeStore<Type, Type> AssociatedMetadataTypesCache = new ThreadSafeStore<Type, Type>(new Func<Type, Type>(JsonTypeReflector.GetAssociateMetadataTypeFromAttribute));
        public const string IdPropertyName = "$id";
        private static readonly ThreadSafeStore<ICustomAttributeProvider, Type> JsonConverterTypeCache = new ThreadSafeStore<ICustomAttributeProvider, Type>(new Func<ICustomAttributeProvider, Type>(JsonTypeReflector.GetJsonConverterTypeFromAttribute));
        private const string MetadataTypeAttributeTypeName = "System.ComponentModel.DataAnnotations.MetadataTypeAttribute, System.ComponentModel.DataAnnotations, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35";
        public const string RefPropertyName = "$ref";
        public const string ShouldSerializePrefix = "ShouldSerialize";
        public const string SpecifiedPostfix = "Specified";
        public const string TypePropertyName = "$type";
        public const string ValuePropertyName = "$value";

        private static Type GetAssociatedMetadataType(Type type)
        {
            return AssociatedMetadataTypesCache.Get(type);
        }

        private static Type GetAssociateMetadataTypeFromAttribute(Type type)
        {
            Type metadataTypeAttributeType = GetMetadataTypeAttributeType();
            if (metadataTypeAttributeType == null)
            {
                return null;
            }
            object realObject = type.GetCustomAttributes(metadataTypeAttributeType, true).SingleOrDefault<object>();
            if (realObject == null)
            {
                return null;
            }
            IMetadataTypeAttribute attribute = DynamicCodeGeneration ? DynamicWrapper.CreateWrapper<IMetadataTypeAttribute>(realObject) : new LateBoundMetadataTypeAttribute(realObject);
            return attribute.MetadataClassType;
        }

        public static T GetAttribute<T>(ICustomAttributeProvider attributeProvider) where T: Attribute
        {
            Type type = attributeProvider as Type;
            if (type != null)
            {
                return GetAttribute<T>(type);
            }
            MemberInfo memberInfo = attributeProvider as MemberInfo;
            if (memberInfo != null)
            {
                return GetAttribute<T>(memberInfo);
            }
            return ReflectionUtils.GetAttribute<T>(attributeProvider, true);
        }

        private static T GetAttribute<T>(MemberInfo memberInfo) where T: Attribute
        {
            T attribute;
            Type associatedMetadataType = GetAssociatedMetadataType(memberInfo.DeclaringType);
            if (associatedMetadataType != null)
            {
                MemberInfo memberInfoFromType = ReflectionUtils.GetMemberInfoFromType(associatedMetadataType, memberInfo);
                if (memberInfoFromType != null)
                {
                    attribute = ReflectionUtils.GetAttribute<T>(memberInfoFromType, true);
                    if (attribute != null)
                    {
                        return attribute;
                    }
                }
            }
            attribute = ReflectionUtils.GetAttribute<T>(memberInfo, true);
            if (attribute != null)
            {
                return attribute;
            }
            foreach (Type type2 in memberInfo.DeclaringType.GetInterfaces())
            {
                MemberInfo attributeProvider = ReflectionUtils.GetMemberInfoFromType(type2, memberInfo);
                if (attributeProvider != null)
                {
                    attribute = ReflectionUtils.GetAttribute<T>(attributeProvider, true);
                    if (attribute != null)
                    {
                        return attribute;
                    }
                }
            }
            return default(T);
        }

        private static T GetAttribute<T>(Type type) where T: Attribute
        {
            T attribute;
            Type associatedMetadataType = GetAssociatedMetadataType(type);
            if (associatedMetadataType != null)
            {
                attribute = ReflectionUtils.GetAttribute<T>(associatedMetadataType, true);
                if (attribute != null)
                {
                    return attribute;
                }
            }
            attribute = ReflectionUtils.GetAttribute<T>(type, true);
            if (attribute != null)
            {
                return attribute;
            }
            foreach (Type type3 in type.GetInterfaces())
            {
                attribute = ReflectionUtils.GetAttribute<T>(type3, true);
                if (attribute != null)
                {
                    return attribute;
                }
            }
            return default(T);
        }

        public static DataContractAttribute GetDataContractAttribute(Type type)
        {
            DataContractAttribute attribute = null;
            for (Type type2 = type; (attribute == null) && (type2 != null); type2 = type2.BaseType)
            {
                attribute = CachedAttributeGetter<DataContractAttribute>.GetAttribute(type2);
            }
            return attribute;
        }

        public static DataMemberAttribute GetDataMemberAttribute(MemberInfo memberInfo)
        {
            if (memberInfo.MemberType == MemberTypes.Field)
            {
                return CachedAttributeGetter<DataMemberAttribute>.GetAttribute(memberInfo);
            }
            PropertyInfo info = (PropertyInfo) memberInfo;
            DataMemberAttribute attribute = CachedAttributeGetter<DataMemberAttribute>.GetAttribute(info);
            if ((attribute == null) && info.IsVirtual())
            {
                for (Type type = info.DeclaringType; (attribute == null) && (type != null); type = type.BaseType)
                {
                    PropertyInfo memberInfoFromType = (PropertyInfo) ReflectionUtils.GetMemberInfoFromType(type, info);
                    if ((memberInfoFromType != null) && memberInfoFromType.IsVirtual())
                    {
                        attribute = CachedAttributeGetter<DataMemberAttribute>.GetAttribute(memberInfoFromType);
                    }
                }
            }
            return attribute;
        }

        public static JsonArrayAttribute GetJsonArrayAttribute(Type type)
        {
            return (GetJsonContainerAttribute(type) as JsonArrayAttribute);
        }

        public static JsonContainerAttribute GetJsonContainerAttribute(Type type)
        {
            return CachedAttributeGetter<JsonContainerAttribute>.GetAttribute(type);
        }

        public static JsonConverter GetJsonConverter(ICustomAttributeProvider attributeProvider, Type targetConvertedType)
        {
            Type jsonConverterType = GetJsonConverterType(attributeProvider);
            if (jsonConverterType != null)
            {
                JsonConverter converter = JsonConverterAttribute.CreateJsonConverterInstance(jsonConverterType);
                if (!converter.CanConvert(targetConvertedType))
                {
                    throw new JsonSerializationException("JsonConverter {0} on {1} is not compatible with member type {2}.".FormatWith(CultureInfo.InvariantCulture, new object[] { converter.GetType().Name, attributeProvider, targetConvertedType.Name }));
                }
                return converter;
            }
            return null;
        }

        private static Type GetJsonConverterType(ICustomAttributeProvider attributeProvider)
        {
            return JsonConverterTypeCache.Get(attributeProvider);
        }

        private static Type GetJsonConverterTypeFromAttribute(ICustomAttributeProvider attributeProvider)
        {
            JsonConverterAttribute attribute = GetAttribute<JsonConverterAttribute>(attributeProvider);
            return ((attribute != null) ? attribute.ConverterType : null);
        }

        public static JsonObjectAttribute GetJsonObjectAttribute(Type type)
        {
            return (GetJsonContainerAttribute(type) as JsonObjectAttribute);
        }

        private static Type GetMetadataTypeAttributeType()
        {
            if (_cachedMetadataTypeAttributeType == null)
            {
                Type type = Type.GetType("System.ComponentModel.DataAnnotations.MetadataTypeAttribute, System.ComponentModel.DataAnnotations, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35");
                if (type == null)
                {
                    return null;
                }
                _cachedMetadataTypeAttributeType = type;
            }
            return _cachedMetadataTypeAttributeType;
        }

        public static MemberSerialization GetObjectMemberSerialization(Type objectType)
        {
            JsonObjectAttribute jsonObjectAttribute = GetJsonObjectAttribute(objectType);
            if (jsonObjectAttribute == null)
            {
                if (GetDataContractAttribute(objectType) != null)
                {
                    return MemberSerialization.OptIn;
                }
                return MemberSerialization.OptOut;
            }
            return jsonObjectAttribute.MemberSerialization;
        }

        public static TypeConverter GetTypeConverter(Type type)
        {
            return TypeDescriptor.GetConverter(type);
        }

        public static bool DynamicCodeGeneration
        {
            get
            {
                if (!_dynamicCodeGeneration.HasValue)
                {
                    try
                    {
                        new ReflectionPermission(ReflectionPermissionFlag.MemberAccess).Demand();
                        new ReflectionPermission(ReflectionPermissionFlag.RestrictedMemberAccess).Demand();
                        new SecurityPermission(SecurityPermissionFlag.UnmanagedCode).Demand();
                        new SecurityPermission(PermissionState.Unrestricted).Demand();
                        _dynamicCodeGeneration = true;
                    }
                    catch (Exception)
                    {
                        _dynamicCodeGeneration = false;
                    }
                }
                return _dynamicCodeGeneration.Value;
            }
        }

        public static Newtonsoft.Json.Utilities.ReflectionDelegateFactory ReflectionDelegateFactory
        {
            get
            {
                if (DynamicCodeGeneration)
                {
                    return DynamicReflectionDelegateFactory.Instance;
                }
                return LateBoundReflectionDelegateFactory.Instance;
            }
        }
    }
}

