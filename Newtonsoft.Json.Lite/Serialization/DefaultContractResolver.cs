namespace Newtonsoft.Json.Serialization
{
    using Newtonsoft.Json;
    using Newtonsoft.Json.Converters;
    using Newtonsoft.Json.Linq;
    using Newtonsoft.Json.Utilities;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Diagnostics.CodeAnalysis;
    using System.Globalization;
    using System.Linq;
    using System.Reflection;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;
    using System.Runtime.Serialization;

    public class DefaultContractResolver : IContractResolver
    {
        private Dictionary<ResolverContractKey, JsonContract> _instanceContractCache;
        private readonly bool _sharedCache;
        private static Dictionary<ResolverContractKey, JsonContract> _sharedContractCache;
        private static readonly object _typeContractCacheLock;
        private static readonly IList<JsonConverter> BuiltInConverters;
        internal static readonly IContractResolver Instance = new DefaultContractResolver(true);

        static DefaultContractResolver()
        {
            List<JsonConverter> list = new List<JsonConverter> {
                new EntityKeyMemberConverter(),
                new BinaryConverter(),
                new KeyValuePairConverter(),
                new DataSetConverter(),
                new DataTableConverter(),
            };
            BuiltInConverters = list;
            _typeContractCacheLock = new object();
        }

        public DefaultContractResolver() : this(false)
        {
        }

        public DefaultContractResolver(bool shareCache)
        {
            this.DefaultMembersSearchFlags = BindingFlags.Public | BindingFlags.Instance;
            this._sharedCache = shareCache;
        }

        internal static bool CanConvertToString(Type type)
        {
            TypeConverter converter = ConvertUtils.GetConverter(type);
            return ((((((converter != null) && !(converter is ComponentConverter)) && !(converter is ReferenceConverter)) && (converter.GetType() != typeof(TypeConverter))) && converter.CanConvertTo(typeof(string))) || ((type == typeof(Type)) || type.IsSubclassOf(typeof(Type))));
        }

        protected virtual JsonArrayContract CreateArrayContract(Type objectType)
        {
            JsonArrayContract contract = new JsonArrayContract(objectType);
            this.InitializeContract(contract);
            return contract;
        }

        protected virtual IList<JsonProperty> CreateConstructorParameters(ConstructorInfo constructor, JsonPropertyCollection memberProperties)
        {
            ParameterInfo[] parameters = constructor.GetParameters();
            JsonPropertyCollection propertys = new JsonPropertyCollection(constructor.DeclaringType);
            foreach (ParameterInfo info in parameters)
            {
                JsonProperty closestMatchProperty = memberProperties.GetClosestMatchProperty(info.Name);
                if ((closestMatchProperty != null) && (closestMatchProperty.PropertyType != info.ParameterType))
                {
                    closestMatchProperty = null;
                }
                JsonProperty property = this.CreatePropertyFromConstructorParameter(closestMatchProperty, info);
                if (property != null)
                {
                    propertys.AddProperty(property);
                }
            }
            return propertys;
        }

        protected virtual JsonContract CreateContract(Type objectType)
        {
            Type type = ReflectionUtils.EnsureNotNullableType(objectType);
            if (JsonConvert.IsJsonPrimitiveType(type))
            {
                return this.CreatePrimitiveContract(type);
            }
            if (JsonTypeReflector.GetJsonObjectAttribute(type) == null)
            {
                if (JsonTypeReflector.GetJsonArrayAttribute(type) != null)
                {
                    return this.CreateArrayContract(type);
                }
                if ((type == typeof(JToken)) || type.IsSubclassOf(typeof(JToken)))
                {
                    return this.CreateLinqContract(type);
                }
                if (CollectionUtils.IsDictionaryType(type))
                {
                    return this.CreateDictionaryContract(type);
                }
                if (typeof(IEnumerable).IsAssignableFrom(type))
                {
                    return this.CreateArrayContract(type);
                }
                if (CanConvertToString(type))
                {
                    return this.CreateStringContract(type);
                }
                if (typeof(ISerializable).IsAssignableFrom(type))
                {
                    return this.CreateISerializableContract(type);
                }
            }
            return this.CreateObjectContract(type);
        }

        protected virtual JsonDictionaryContract CreateDictionaryContract(Type objectType)
        {
            JsonDictionaryContract contract = new JsonDictionaryContract(objectType);
            this.InitializeContract(contract);
            contract.PropertyNameResolver = new Func<string, string>(this.ResolvePropertyName);
            return contract;
        }

        protected virtual JsonISerializableContract CreateISerializableContract(Type objectType)
        {
            JsonISerializableContract contract = new JsonISerializableContract(objectType);
            this.InitializeContract(contract);
            ConstructorInfo method = objectType.GetConstructor(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance, null, new Type[] { typeof(SerializationInfo), typeof(StreamingContext) }, null);
            if (method != null)
            {
                MethodCall<object, object> methodCall = JsonTypeReflector.ReflectionDelegateFactory.CreateMethodCall<object>(method);
                contract.ISerializableCreator = args => methodCall(null, args);
            }
            return contract;
        }

        protected virtual JsonLinqContract CreateLinqContract(Type objectType)
        {
            JsonLinqContract contract = new JsonLinqContract(objectType);
            this.InitializeContract(contract);
            return contract;
        }

        protected virtual IValueProvider CreateMemberValueProvider(MemberInfo member)
        {
            if (this.DynamicCodeGeneration)
            {
                return new DynamicValueProvider(member);
            }
            return new ReflectionValueProvider(member);
        }

        protected virtual JsonObjectContract CreateObjectContract(Type objectType)
        {
            ConstructorInfo attributeConstructor;
            JsonObjectContract contract = new JsonObjectContract(objectType);
            this.InitializeContract(contract);
            contract.MemberSerialization = JsonTypeReflector.GetObjectMemberSerialization(objectType);
            contract.Properties.AddRange<JsonProperty>(this.CreateProperties(contract.UnderlyingType, contract.MemberSerialization));
            if (objectType.GetConstructors(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance).Any<ConstructorInfo>(c => c.IsDefined(typeof(JsonConstructorAttribute), true)))
            {
                attributeConstructor = this.GetAttributeConstructor(objectType);
                if (attributeConstructor != null)
                {
                    contract.OverrideConstructor = attributeConstructor;
                    contract.ConstructorParameters.AddRange<JsonProperty>(this.CreateConstructorParameters(attributeConstructor, contract.Properties));
                }
                return contract;
            }
            if ((contract.DefaultCreator == null) || contract.DefaultCreatorNonPublic)
            {
                attributeConstructor = this.GetParametrizedConstructor(objectType);
                if (attributeConstructor != null)
                {
                    contract.ParametrizedConstructor = attributeConstructor;
                    contract.ConstructorParameters.AddRange<JsonProperty>(this.CreateConstructorParameters(attributeConstructor, contract.Properties));
                }
            }
            return contract;
        }

        protected virtual JsonPrimitiveContract CreatePrimitiveContract(Type objectType)
        {
            JsonPrimitiveContract contract = new JsonPrimitiveContract(objectType);
            this.InitializeContract(contract);
            return contract;
        }

        protected virtual IList<JsonProperty> CreateProperties(Type type, MemberSerialization memberSerialization)
        {
            List<MemberInfo> serializableMembers = this.GetSerializableMembers(type);
            if (serializableMembers == null)
            {
                throw new JsonSerializationException("Null collection of seralizable members returned.");
            }
            JsonPropertyCollection propertys = new JsonPropertyCollection(type);
            foreach (MemberInfo info in serializableMembers)
            {
                JsonProperty property = this.CreateProperty(info, memberSerialization);
                if (property != null)
                {
                    propertys.AddProperty(property);
                }
            }
            return propertys;
        }

        protected virtual JsonProperty CreateProperty(MemberInfo member, MemberSerialization memberSerialization)
        {
            bool flag;
            JsonProperty property = new JsonProperty {
                PropertyType = ReflectionUtils.GetMemberUnderlyingType(member),
                ValueProvider = this.CreateMemberValueProvider(member)
            };
            this.SetPropertySettingsFromAttributes(property, member, member.Name, member.DeclaringType, memberSerialization, out flag);
            property.Readable = ReflectionUtils.CanReadMemberValue(member, flag);
            property.Writable = ReflectionUtils.CanSetMemberValue(member, flag);
            property.ShouldSerialize = this.CreateShouldSerializeTest(member);
            this.SetIsSpecifiedActions(property, member);
            return property;
        }

        protected virtual JsonProperty CreatePropertyFromConstructorParameter(JsonProperty matchingMemberProperty, ParameterInfo parameterInfo)
        {
            bool flag;
            JsonProperty property = new JsonProperty {
                PropertyType = parameterInfo.ParameterType
            };
            this.SetPropertySettingsFromAttributes(property, parameterInfo, parameterInfo.Name, parameterInfo.Member.DeclaringType, MemberSerialization.OptOut, out flag);
            property.Readable = false;
            property.Writable = true;
            if (matchingMemberProperty != null)
            {
                property.PropertyName = (property.PropertyName != parameterInfo.Name) ? property.PropertyName : matchingMemberProperty.PropertyName;
                property.Converter = property.Converter ?? matchingMemberProperty.Converter;
                property.MemberConverter = property.MemberConverter ?? matchingMemberProperty.MemberConverter;
                property.DefaultValue = property.DefaultValue ?? matchingMemberProperty.DefaultValue;
                property.Required = (property.Required != Required.Default) ? property.Required : matchingMemberProperty.Required;
                bool? isReference = property.IsReference;
                property.IsReference = isReference.HasValue ? new bool?(isReference.GetValueOrDefault()) : matchingMemberProperty.IsReference;
                NullValueHandling? nullValueHandling = property.NullValueHandling;
                property.NullValueHandling = nullValueHandling.HasValue ? new NullValueHandling?(nullValueHandling.GetValueOrDefault()) : matchingMemberProperty.NullValueHandling;
                DefaultValueHandling? defaultValueHandling = property.DefaultValueHandling;
                property.DefaultValueHandling = defaultValueHandling.HasValue ? new DefaultValueHandling?(defaultValueHandling.GetValueOrDefault()) : matchingMemberProperty.DefaultValueHandling;
                ReferenceLoopHandling? referenceLoopHandling = property.ReferenceLoopHandling;
                property.ReferenceLoopHandling = referenceLoopHandling.HasValue ? new ReferenceLoopHandling?(referenceLoopHandling.GetValueOrDefault()) : matchingMemberProperty.ReferenceLoopHandling;
                ObjectCreationHandling? objectCreationHandling = property.ObjectCreationHandling;
                property.ObjectCreationHandling = objectCreationHandling.HasValue ? new ObjectCreationHandling?(objectCreationHandling.GetValueOrDefault()) : matchingMemberProperty.ObjectCreationHandling;
                TypeNameHandling? typeNameHandling = property.TypeNameHandling;
                property.TypeNameHandling = typeNameHandling.HasValue ? new TypeNameHandling?(typeNameHandling.GetValueOrDefault()) : matchingMemberProperty.TypeNameHandling;
            }
            return property;
        }

        private Predicate<object> CreateShouldSerializeTest(MemberInfo member)
        {
            MethodInfo method = member.DeclaringType.GetMethod("ShouldSerialize" + member.Name, new Type[0]);
            if ((method == null) || (method.ReturnType != typeof(bool)))
            {
                return null;
            }
            MethodCall<object, object> shouldSerializeCall = JsonTypeReflector.ReflectionDelegateFactory.CreateMethodCall<object>(method);
            return o => ((bool) shouldSerializeCall(o, new object[0]));
        }

        protected virtual JsonStringContract CreateStringContract(Type objectType)
        {
            JsonStringContract contract = new JsonStringContract(objectType);
            this.InitializeContract(contract);
            return contract;
        }

        private ConstructorInfo GetAttributeConstructor(Type objectType)
        {
            IList<ConstructorInfo> list = (from c in objectType.GetConstructors(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance)
                where c.IsDefined(typeof(JsonConstructorAttribute), true)
                select c).ToList<ConstructorInfo>();
            if (list.Count > 1)
            {
                throw new Exception("Multiple constructors with the JsonConstructorAttribute.");
            }
            if (list.Count == 1)
            {
                return list[0];
            }
            return null;
        }

        private Dictionary<ResolverContractKey, JsonContract> GetCache()
        {
            if (this._sharedCache)
            {
                return _sharedContractCache;
            }
            return this._instanceContractCache;
        }

        internal static string GetClrTypeFullName(Type type)
        {
            if (!(!type.IsGenericTypeDefinition && type.ContainsGenericParameters))
            {
                return type.FullName;
            }
            return string.Format(CultureInfo.InvariantCulture, "{0}.{1}", new object[] { type.Namespace, type.Name });
        }

        private Func<object> GetDefaultCreator(Type createdType)
        {
            return JsonTypeReflector.ReflectionDelegateFactory.CreateDefaultConstructor<object>(createdType);
        }

        private ConstructorInfo GetParametrizedConstructor(Type objectType)
        {
            IList<ConstructorInfo> constructors = objectType.GetConstructors(BindingFlags.Public | BindingFlags.Instance);
            if (constructors.Count == 1)
            {
                return constructors[0];
            }
            return null;
        }

        protected virtual List<MemberInfo> GetSerializableMembers(Type objectType)
        {
            Type type;
            DataContractAttribute dataContractAttribute = JsonTypeReflector.GetDataContractAttribute(objectType);
            List<MemberInfo> list = (from m in ReflectionUtils.GetFieldsAndProperties(objectType, this.DefaultMembersSearchFlags)
                where !ReflectionUtils.IsIndexedProperty(m)
                select m).ToList<MemberInfo>();
            List<MemberInfo> list2 = (from m in ReflectionUtils.GetFieldsAndProperties(objectType, BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Static | BindingFlags.Instance)
                where !ReflectionUtils.IsIndexedProperty(m)
                select m).ToList<MemberInfo>();
            List<MemberInfo> source = new List<MemberInfo>();
            foreach (MemberInfo info in list2)
            {
                if (this.SerializeCompilerGeneratedMembers || !info.IsDefined(typeof(CompilerGeneratedAttribute), true))
                {
                    if (list.Contains(info))
                    {
                        source.Add(info);
                    }
                    else if (JsonTypeReflector.GetAttribute<JsonPropertyAttribute>((ICustomAttributeProvider) info) != null)
                    {
                        source.Add(info);
                    }
                    else if ((dataContractAttribute != null) && (JsonTypeReflector.GetAttribute<DataMemberAttribute>((ICustomAttributeProvider) info) != null))
                    {
                        source.Add(info);
                    }
                }
            }
            if (objectType.AssignableToTypeName("System.Data.Objects.DataClasses.EntityObject", out type))
            {
                source = source.Where<MemberInfo>(new Func<MemberInfo, bool>(this.ShouldSerializeEntityMember)).ToList<MemberInfo>();
            }
            return source;
        }

        [SuppressMessage("Microsoft.Portability", "CA1903:UseOnlyApiFromTargetedFramework", MessageId="System.Runtime.Serialization.DataContractAttribute.#get_IsReference()")]
        private void InitializeContract(JsonContract contract)
        {
            JsonContainerAttribute jsonContainerAttribute = JsonTypeReflector.GetJsonContainerAttribute(contract.UnderlyingType);
            if (jsonContainerAttribute != null)
            {
                contract.IsReference = jsonContainerAttribute._isReference;
            }
            else
            {
                DataContractAttribute dataContractAttribute = JsonTypeReflector.GetDataContractAttribute(contract.UnderlyingType);
                if ((dataContractAttribute != null) && dataContractAttribute.IsReference)
                {
                    contract.IsReference = true;
                }
            }
            contract.Converter = this.ResolveContractConverter(contract.UnderlyingType);
            contract.InternalConverter = JsonSerializer.GetMatchingConverter(BuiltInConverters, contract.UnderlyingType);
            if (ReflectionUtils.HasDefaultConstructor(contract.CreatedType, true) || contract.CreatedType.IsValueType)
            {
                contract.DefaultCreator = this.GetDefaultCreator(contract.CreatedType);
                contract.DefaultCreatorNonPublic = !contract.CreatedType.IsValueType && (ReflectionUtils.GetDefaultConstructor(contract.CreatedType) == null);
            }
            foreach (MethodInfo info in contract.UnderlyingType.GetMethods(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly))
            {
                if (!info.ContainsGenericParameters)
                {
                    Type prevAttributeType = null;
                    ParameterInfo[] parameters = info.GetParameters();
                    if (IsValidCallback(info, parameters, typeof(OnSerializingAttribute), contract.OnSerializing, ref prevAttributeType))
                    {
                        contract.OnSerializing = info;
                    }
                    if (IsValidCallback(info, parameters, typeof(OnSerializedAttribute), contract.OnSerialized, ref prevAttributeType))
                    {
                        contract.OnSerialized = info;
                    }
                    if (IsValidCallback(info, parameters, typeof(OnDeserializingAttribute), contract.OnDeserializing, ref prevAttributeType))
                    {
                        contract.OnDeserializing = info;
                    }
                    if (IsValidCallback(info, parameters, typeof(OnDeserializedAttribute), contract.OnDeserialized, ref prevAttributeType))
                    {
                        contract.OnDeserialized = info;
                    }
                    if (IsValidCallback(info, parameters, typeof(OnErrorAttribute), contract.OnError, ref prevAttributeType))
                    {
                        contract.OnError = info;
                    }
                }
            }
        }

        private static bool IsValidCallback(MethodInfo method, ParameterInfo[] parameters, Type attributeType, MethodInfo currentCallback, ref Type prevAttributeType)
        {
            if (!method.IsDefined(attributeType, false))
            {
                return false;
            }
            if (currentCallback != null)
            {
                throw new Exception("Invalid attribute. Both '{0}' and '{1}' in type '{2}' have '{3}'.".FormatWith(CultureInfo.InvariantCulture, new object[] { method, currentCallback, GetClrTypeFullName(method.DeclaringType), attributeType }));
            }
            if (prevAttributeType != null)
            {
                throw new Exception("Invalid Callback. Method '{3}' in type '{2}' has both '{0}' and '{1}'.".FormatWith(CultureInfo.InvariantCulture, new object[] { prevAttributeType, attributeType, GetClrTypeFullName(method.DeclaringType), method }));
            }
            if (method.IsVirtual)
            {
                throw new Exception("Virtual Method '{0}' of type '{1}' cannot be marked with '{2}' attribute.".FormatWith(CultureInfo.InvariantCulture, new object[] { method, GetClrTypeFullName(method.DeclaringType), attributeType }));
            }
            if (method.ReturnType != typeof(void))
            {
                throw new Exception("Serialization Callback '{1}' in type '{0}' must return void.".FormatWith(CultureInfo.InvariantCulture, new object[] { GetClrTypeFullName(method.DeclaringType), method }));
            }
            if (attributeType == typeof(OnErrorAttribute))
            {
                if ((((parameters == null) || (parameters.Length != 2)) || (parameters[0].ParameterType != typeof(StreamingContext))) || (parameters[1].ParameterType != typeof(ErrorContext)))
                {
                    throw new Exception("Serialization Error Callback '{1}' in type '{0}' must have two parameters of type '{2}' and '{3}'.".FormatWith(CultureInfo.InvariantCulture, new object[] { GetClrTypeFullName(method.DeclaringType), method, typeof(StreamingContext), typeof(ErrorContext) }));
                }
            }
            else if (((parameters == null) || (parameters.Length != 1)) || (parameters[0].ParameterType != typeof(StreamingContext)))
            {
                throw new Exception("Serialization Callback '{1}' in type '{0}' must have a single parameter of type '{2}'.".FormatWith(CultureInfo.InvariantCulture, new object[] { GetClrTypeFullName(method.DeclaringType), method, typeof(StreamingContext) }));
            }
            prevAttributeType = attributeType;
            return true;
        }

        public virtual JsonContract ResolveContract(Type type)
        {
            JsonContract contract;
            if (type == null)
            {
                throw new ArgumentNullException("type");
            }
            ResolverContractKey key = new ResolverContractKey(base.GetType(), type);
            Dictionary<ResolverContractKey, JsonContract> cache = this.GetCache();
            if ((cache == null) || !cache.TryGetValue(key, out contract))
            {
                contract = this.CreateContract(type);
                lock (_typeContractCacheLock)
                {
                    cache = this.GetCache();
                    Dictionary<ResolverContractKey, JsonContract> dictionary2 = (cache != null) ? new Dictionary<ResolverContractKey, JsonContract>(cache) : new Dictionary<ResolverContractKey, JsonContract>();
                    dictionary2[key] = contract;
                    this.UpdateCache(dictionary2);
                }
            }
            return contract;
        }

        protected virtual JsonConverter ResolveContractConverter(Type objectType)
        {
            return JsonTypeReflector.GetJsonConverter(objectType, objectType);
        }

        protected virtual string ResolvePropertyName(string propertyName)
        {
            return propertyName;
        }

        private void SetIsSpecifiedActions(JsonProperty property, MemberInfo member)
        {
            Func<object, object> specifiedPropertyGet;
            MemberInfo field = member.DeclaringType.GetProperty(member.Name + "Specified");
            if (field == null)
            {
                field = member.DeclaringType.GetField(member.Name + "Specified");
            }
            if ((field != null) && (ReflectionUtils.GetMemberUnderlyingType(field) == typeof(bool)))
            {
                specifiedPropertyGet = JsonTypeReflector.ReflectionDelegateFactory.CreateGet<object>(field);
                property.GetIsSpecified = o => (bool) specifiedPropertyGet(o);
                property.SetIsSpecified = JsonTypeReflector.ReflectionDelegateFactory.CreateSet<object>(field);
            }
        }

        private void SetPropertySettingsFromAttributes(JsonProperty property, ICustomAttributeProvider attributeProvider, string name, Type declaringType, MemberSerialization memberSerialization, out bool allowNonPublicAccess)
        {
            DataMemberAttribute dataMemberAttribute;
            string propertyName;
            if ((JsonTypeReflector.GetDataContractAttribute(declaringType) != null) && (attributeProvider is MemberInfo))
            {
                dataMemberAttribute = JsonTypeReflector.GetDataMemberAttribute((MemberInfo) attributeProvider);
            }
            else
            {
                dataMemberAttribute = null;
            }
            JsonPropertyAttribute attribute = JsonTypeReflector.GetAttribute<JsonPropertyAttribute>(attributeProvider);
            bool flag = JsonTypeReflector.GetAttribute<JsonIgnoreAttribute>(attributeProvider) != null;
            if ((attribute != null) && (attribute.PropertyName != null))
            {
                propertyName = attribute.PropertyName;
            }
            else if ((dataMemberAttribute != null) && (dataMemberAttribute.Name != null))
            {
                propertyName = dataMemberAttribute.Name;
            }
            else
            {
                propertyName = name;
            }
            property.PropertyName = this.ResolvePropertyName(propertyName);
            property.UnderlyingName = name;
            if (attribute != null)
            {
                property.Required = attribute.Required;
            }
            else if (dataMemberAttribute != null)
            {
                property.Required = dataMemberAttribute.IsRequired ? Required.AllowNull : Required.Default;
            }
            else
            {
                property.Required = Required.Default;
            }
            property.Ignored = flag || (((memberSerialization == MemberSerialization.OptIn) && (attribute == null)) && (dataMemberAttribute == null));
            property.Converter = JsonTypeReflector.GetJsonConverter(attributeProvider, property.PropertyType);
            property.MemberConverter = JsonTypeReflector.GetJsonConverter(attributeProvider, property.PropertyType);
            DefaultValueAttribute attribute4 = JsonTypeReflector.GetAttribute<DefaultValueAttribute>(attributeProvider);
            property.DefaultValue = (attribute4 != null) ? attribute4.Value : null;
            property.NullValueHandling = (attribute != null) ? attribute._nullValueHandling : null;
            property.DefaultValueHandling = (attribute != null) ? attribute._defaultValueHandling : null;
            property.ReferenceLoopHandling = (attribute != null) ? attribute._referenceLoopHandling : null;
            property.ObjectCreationHandling = (attribute != null) ? attribute._objectCreationHandling : null;
            property.TypeNameHandling = (attribute != null) ? attribute._typeNameHandling : null;
            property.IsReference = (attribute != null) ? attribute._isReference : null;
            allowNonPublicAccess = false;
            if ((this.DefaultMembersSearchFlags & BindingFlags.NonPublic) == BindingFlags.NonPublic)
            {
                allowNonPublicAccess = true;
            }
            if (attribute != null)
            {
                allowNonPublicAccess = true;
            }
            if (dataMemberAttribute != null)
            {
                allowNonPublicAccess = true;
            }
        }

        private bool ShouldSerializeEntityMember(MemberInfo memberInfo)
        {
            PropertyInfo info = memberInfo as PropertyInfo;
            if ((info != null) && (info.PropertyType.IsGenericType && (info.PropertyType.GetGenericTypeDefinition().FullName == "System.Data.Objects.DataClasses.EntityReference`1")))
            {
                return false;
            }
            return true;
        }

        private void UpdateCache(Dictionary<ResolverContractKey, JsonContract> cache)
        {
            if (this._sharedCache)
            {
                _sharedContractCache = cache;
            }
            else
            {
                this._instanceContractCache = cache;
            }
        }

        public BindingFlags DefaultMembersSearchFlags { get; set; }

        public bool DynamicCodeGeneration
        {
            get
            {
                return JsonTypeReflector.DynamicCodeGeneration;
            }
        }

        public bool SerializeCompilerGeneratedMembers { get; set; }
    }
}

