namespace Newtonsoft.Json.Serialization
{
    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;
    using Newtonsoft.Json.Utilities;
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Globalization;
    using System.Linq;
    using System.Reflection;
    using System.Runtime.Serialization;

    internal class JsonSerializerInternalReader : JsonSerializerInternalBase
    {
        private JsonFormatterConverter _formatterConverter;
        private JsonSerializerProxy _internalSerializer;

        public JsonSerializerInternalReader(JsonSerializer serializer) : base(serializer)
        {
        }

        private void CheckedRead(JsonReader reader)
        {
            if (!reader.Read())
            {
                throw new JsonSerializationException("Unexpected end when deserializing object.");
            }
        }

        private object CreateAndPopulateDictionary(JsonReader reader, JsonDictionaryContract contract, string id)
        {
            if ((contract.DefaultCreator == null) || (contract.DefaultCreatorNonPublic && (base.Serializer.ConstructorHandling != ConstructorHandling.AllowNonPublicDefaultConstructor)))
            {
                throw new JsonSerializationException("Unable to find a default constructor to use for type {0}.".FormatWith(CultureInfo.InvariantCulture, new object[] { contract.UnderlyingType }));
            }
            object obj2 = contract.DefaultCreator();
            IWrappedDictionary dictionary = contract.CreateWrapper(obj2);
            this.PopulateDictionary(dictionary, reader, contract, id);
            return dictionary.UnderlyingDictionary;
        }

        private object CreateAndPopulateList(JsonReader reader, string reference, JsonArrayContract contract)
        {
            return CollectionUtils.CreateAndPopulateList(contract.CreatedType, (l, isTemporaryListReference) => {
                if ((reference != null) && isTemporaryListReference)
                {
                    throw new JsonSerializationException("Cannot preserve reference to array or readonly list: {0}".FormatWith(CultureInfo.InvariantCulture, new object[] { contract.UnderlyingType }));
                }
                if ((contract.OnSerializing != null) && isTemporaryListReference)
                {
                    throw new JsonSerializationException("Cannot call OnSerializing on an array or readonly list: {0}".FormatWith(CultureInfo.InvariantCulture, new object[] { contract.UnderlyingType }));
                }
                if ((contract.OnError != null) && isTemporaryListReference)
                {
                    throw new JsonSerializationException("Cannot call OnError on an array or readonly list: {0}".FormatWith(CultureInfo.InvariantCulture, new object[] { contract.UnderlyingType }));
                }
                this.PopulateList(contract.CreateWrapper(l), reader, reference, contract);
            });
        }

        private object CreateAndPopulateObject(JsonReader reader, JsonObjectContract contract, string id)
        {
            object newObject = null;
            if (contract.UnderlyingType.IsInterface || contract.UnderlyingType.IsAbstract)
            {
                throw new JsonSerializationException("Could not create an instance of type {0}. Type is an interface or abstract class and cannot be instantated.".FormatWith(CultureInfo.InvariantCulture, new object[] { contract.UnderlyingType }));
            }
            if (contract.OverrideConstructor != null)
            {
                if (contract.OverrideConstructor.GetParameters().Length > 0)
                {
                    return this.CreateObjectFromNonDefaultConstructor(reader, contract, contract.OverrideConstructor, id);
                }
                newObject = contract.OverrideConstructor.Invoke(null);
            }
            else if ((contract.DefaultCreator != null) && (!contract.DefaultCreatorNonPublic || (base.Serializer.ConstructorHandling == ConstructorHandling.AllowNonPublicDefaultConstructor)))
            {
                newObject = contract.DefaultCreator();
            }
            else if (contract.ParametrizedConstructor != null)
            {
                return this.CreateObjectFromNonDefaultConstructor(reader, contract, contract.ParametrizedConstructor, id);
            }
            if (newObject == null)
            {
                throw new JsonSerializationException("Unable to find a constructor to use for type {0}. A class should either have a default constructor, one constructor with arguments or a constructor marked with the JsonConstructor attribute.".FormatWith(CultureInfo.InvariantCulture, new object[] { contract.UnderlyingType }));
            }
            this.PopulateObject(newObject, reader, contract, id);
            return newObject;
        }

        private object CreateISerializable(JsonReader reader, JsonISerializableContract contract, string id)
        {
            Type underlyingType = contract.UnderlyingType;
            SerializationInfo info = new SerializationInfo(contract.UnderlyingType, this.GetFormatterConverter());
            bool flag = false;
            do
            {
                switch (reader.TokenType)
                {
                    case JsonToken.PropertyName:
                    {
                        string name = reader.Value.ToString();
                        if (!reader.Read())
                        {
                            throw new JsonSerializationException("Unexpected end when setting {0}'s value.".FormatWith(CultureInfo.InvariantCulture, new object[] { name }));
                        }
                        info.AddValue(name, JToken.ReadFrom(reader));
                        break;
                    }
                    case JsonToken.Comment:
                        break;

                    case JsonToken.EndObject:
                        flag = true;
                        break;

                    default:
                        throw new JsonSerializationException("Unexpected token when deserializing object: " + reader.TokenType);
                }
            }
            while (!flag && reader.Read());
            if (contract.ISerializableCreator == null)
            {
                throw new JsonSerializationException("ISerializable type '{0}' does not have a valid constructor.".FormatWith(CultureInfo.InvariantCulture, new object[] { underlyingType }));
            }
            object obj2 = contract.ISerializableCreator(new object[] { info, base.Serializer.Context });
            if (id != null)
            {
                base.Serializer.ReferenceResolver.AddReference(this, id, obj2);
            }
            contract.InvokeOnDeserializing(obj2, base.Serializer.Context);
            contract.InvokeOnDeserialized(obj2, base.Serializer.Context);
            return obj2;
        }

        private JToken CreateJObject(JsonReader reader)
        {
            ValidationUtils.ArgumentNotNull(reader, "reader");
            using (JTokenWriter writer = new JTokenWriter())
            {
                writer.WriteStartObject();
                if (reader.TokenType == JsonToken.PropertyName)
                {
                    writer.WriteToken(reader, reader.Depth - 1);
                }
                else
                {
                    writer.WriteEndObject();
                }
                return writer.Token;
            }
        }

        private JToken CreateJToken(JsonReader reader, JsonContract contract)
        {
            ValidationUtils.ArgumentNotNull(reader, "reader");
            if ((contract != null) && (contract.UnderlyingType == typeof(JRaw)))
            {
                return JRaw.Create(reader);
            }
            using (JTokenWriter writer = new JTokenWriter())
            {
                writer.WriteToken(reader);
                return writer.Token;
            }
        }

        private object CreateList(JsonReader reader, Type objectType, JsonContract contract, JsonProperty member, object existingValue, string reference)
        {
            if (this.HasDefinedType(objectType))
            {
                JsonArrayContract contract2 = this.EnsureArrayContract(objectType, contract);
                if (existingValue == null)
                {
                    return this.CreateAndPopulateList(reader, reference, contract2);
                }
                return this.PopulateList(contract2.CreateWrapper(existingValue), reader, reference, contract2);
            }
            return this.CreateJToken(reader, contract);
        }

        private object CreateObject(JsonReader reader, Type objectType, JsonContract contract, JsonProperty member, object existingValue)
        {
            this.CheckedRead(reader);
            string reference = null;
            if (reader.TokenType == JsonToken.PropertyName)
            {
                bool flag;
                do
                {
                    string a = reader.Value.ToString();
                    if (string.Equals(a, "$ref", StringComparison.Ordinal))
                    {
                        this.CheckedRead(reader);
                        if (reader.TokenType != JsonToken.String)
                        {
                            throw new JsonSerializationException("JSON reference {0} property must have a string value.".FormatWith(CultureInfo.InvariantCulture, new object[] { "$ref" }));
                        }
                        string str3 = reader.Value.ToString();
                        this.CheckedRead(reader);
                        if (reader.TokenType == JsonToken.PropertyName)
                        {
                            throw new JsonSerializationException("Additional content found in JSON reference object. A JSON reference object should only have a {0} property.".FormatWith(CultureInfo.InvariantCulture, new object[] { "$ref" }));
                        }
                        return base.Serializer.ReferenceResolver.ResolveReference(this, str3);
                    }
                    if (string.Equals(a, "$type", StringComparison.Ordinal))
                    {
                        this.CheckedRead(reader);
                        string fullyQualifiedTypeName = reader.Value.ToString();
                        this.CheckedRead(reader);
                        TypeNameHandling? nullable = (member != null) ? member.TypeNameHandling : null;
                        if (((TypeNameHandling) (nullable.HasValue ? nullable.GetValueOrDefault() : base.Serializer.TypeNameHandling)) != TypeNameHandling.None)
                        {
                            string str5;
                            string str6;
                            Type type;
                            ReflectionUtils.SplitFullyQualifiedTypeName(fullyQualifiedTypeName, out str5, out str6);
                            try
                            {
                                type = base.Serializer.Binder.BindToType(str6, str5);
                            }
                            catch (Exception exception)
                            {
                                throw new JsonSerializationException("Error resolving type specified in JSON '{0}'.".FormatWith(CultureInfo.InvariantCulture, new object[] { fullyQualifiedTypeName }), exception);
                            }
                            if (type == null)
                            {
                                throw new JsonSerializationException("Type specified in JSON '{0}' was not resolved.".FormatWith(CultureInfo.InvariantCulture, new object[] { fullyQualifiedTypeName }));
                            }
                            if (!((objectType == null) || objectType.IsAssignableFrom(type)))
                            {
                                throw new JsonSerializationException("Type specified in JSON '{0}' is not compatible with '{1}'.".FormatWith(CultureInfo.InvariantCulture, new object[] { type.AssemblyQualifiedName, objectType.AssemblyQualifiedName }));
                            }
                            objectType = type;
                            contract = this.GetContractSafe(type);
                        }
                        flag = true;
                    }
                    else if (string.Equals(a, "$id", StringComparison.Ordinal))
                    {
                        this.CheckedRead(reader);
                        reference = reader.Value.ToString();
                        this.CheckedRead(reader);
                        flag = true;
                    }
                    else
                    {
                        if (string.Equals(a, "$values", StringComparison.Ordinal))
                        {
                            this.CheckedRead(reader);
                            object obj2 = this.CreateList(reader, objectType, contract, member, existingValue, reference);
                            this.CheckedRead(reader);
                            return obj2;
                        }
                        flag = false;
                    }
                }
                while (flag && (reader.TokenType == JsonToken.PropertyName));
            }
            if (!this.HasDefinedType(objectType))
            {
                return this.CreateJObject(reader);
            }
            if (contract == null)
            {
                throw new JsonSerializationException("Could not resolve type '{0}' to a JsonContract.".FormatWith(CultureInfo.InvariantCulture, new object[] { objectType }));
            }
            JsonDictionaryContract contract2 = contract as JsonDictionaryContract;
            if (contract2 != null)
            {
                if (existingValue == null)
                {
                    return this.CreateAndPopulateDictionary(reader, contract2, reference);
                }
                return this.PopulateDictionary(contract2.CreateWrapper(existingValue), reader, contract2, reference);
            }
            JsonObjectContract contract3 = contract as JsonObjectContract;
            if (contract3 != null)
            {
                if (existingValue == null)
                {
                    return this.CreateAndPopulateObject(reader, contract3, reference);
                }
                return this.PopulateObject(existingValue, reader, contract3, reference);
            }
            JsonPrimitiveContract contract4 = contract as JsonPrimitiveContract;
            if ((contract4 != null) && ((reader.TokenType == JsonToken.PropertyName) && string.Equals(reader.Value.ToString(), "$value", StringComparison.Ordinal)))
            {
                this.CheckedRead(reader);
                object obj3 = this.CreateValueInternal(reader, objectType, contract4, member, existingValue);
                this.CheckedRead(reader);
                return obj3;
            }
            JsonISerializableContract contract5 = contract as JsonISerializableContract;
            if (contract5 == null)
            {
                throw new JsonSerializationException("Cannot deserialize JSON object into type '{0}'.".FormatWith(CultureInfo.InvariantCulture, new object[] { objectType }));
            }
            return this.CreateISerializable(reader, contract5, reference);
        }

        private object CreateObjectFromNonDefaultConstructor(JsonReader reader, JsonObjectContract contract, ConstructorInfo constructorInfo, string id)
        {
            ValidationUtils.ArgumentNotNull(constructorInfo, "constructorInfo");
            Type underlyingType = contract.UnderlyingType;
            IDictionary<JsonProperty, object> dictionary = this.ResolvePropertyAndConstructorValues(contract, reader, underlyingType);
            IDictionary<ParameterInfo, object> source = constructorInfo.GetParameters().ToDictionary<ParameterInfo, ParameterInfo, object>(p => p, p => null);
            IDictionary<JsonProperty, object> dictionary3 = new Dictionary<JsonProperty, object>();
            foreach (KeyValuePair<JsonProperty, object> pair in dictionary)
            {
                ParameterInfo key = source.ForgivingCaseSensitiveFind<KeyValuePair<ParameterInfo, object>>(kv => kv.Key.Name, pair.Key.UnderlyingName).Key;
                if (key != null)
                {
                    source[key] = pair.Value;
                }
                else
                {
                    dictionary3.Add(pair);
                }
            }
            object obj2 = constructorInfo.Invoke(source.Values.ToArray<object>());
            if (id != null)
            {
                base.Serializer.ReferenceResolver.AddReference(this, id, obj2);
            }
            contract.InvokeOnDeserializing(obj2, base.Serializer.Context);
            foreach (KeyValuePair<JsonProperty, object> pair2 in dictionary3)
            {
                JsonProperty property = pair2.Key;
                object obj3 = pair2.Value;
                if (this.ShouldSetPropertyValue(pair2.Key, pair2.Value))
                {
                    property.ValueProvider.SetValue(obj2, obj3);
                }
            }
            contract.InvokeOnDeserialized(obj2, base.Serializer.Context);
            return obj2;
        }

        private object CreateValueInternal(JsonReader reader, Type objectType, JsonContract contract, JsonProperty member, object existingValue)
        {
            if (contract is JsonLinqContract)
            {
                return this.CreateJToken(reader, contract);
            }
        Label_001F:
            switch (reader.TokenType)
            {
                case JsonToken.StartObject:
                    return this.CreateObject(reader, objectType, contract, member, existingValue);

                case JsonToken.StartArray:
                    return this.CreateList(reader, objectType, contract, member, existingValue, null);

                case JsonToken.StartConstructor:
                case JsonToken.EndConstructor:
                    return reader.Value.ToString();

                case JsonToken.Comment:
                    if (!reader.Read())
                    {
                        throw new JsonSerializationException("Unexpected end when deserializing object.");
                    }
                    goto Label_001F;

                case JsonToken.Raw:
                    return new JRaw((string) reader.Value);

                case JsonToken.Integer:
                case JsonToken.Float:
                case JsonToken.Boolean:
                case JsonToken.Date:
                case JsonToken.Bytes:
                    return this.EnsureType(reader.Value, CultureInfo.InvariantCulture, objectType);

                case JsonToken.String:
                    if ((!string.IsNullOrEmpty((string) reader.Value) || (objectType == null)) || !ReflectionUtils.IsNullableType(objectType))
                    {
                        if (objectType == typeof(byte[]))
                        {
                            return Convert.FromBase64String((string) reader.Value);
                        }
                        return this.EnsureType(reader.Value, CultureInfo.InvariantCulture, objectType);
                    }
                    return null;

                case JsonToken.Null:
                case JsonToken.Undefined:
                    if (objectType != typeof(DBNull))
                    {
                        return this.EnsureType(reader.Value, CultureInfo.InvariantCulture, objectType);
                    }
                    return DBNull.Value;
            }
            throw new JsonSerializationException("Unexpected token while deserializing object: " + reader.TokenType);
        }

        private object CreateValueNonProperty(JsonReader reader, Type objectType, JsonContract contract)
        {
            JsonConverter converter = this.GetConverter(contract, null);
            if ((converter != null) && converter.CanRead)
            {
                return converter.ReadJson(reader, objectType, null, this.GetInternalSerializer());
            }
            return this.CreateValueInternal(reader, objectType, contract, null, null);
        }

        private object CreateValueProperty(JsonReader reader, JsonProperty property, object target, bool gottenCurrentValue, object currentValue)
        {
            JsonContract contractSafe = this.GetContractSafe(property.PropertyType, currentValue);
            Type propertyType = property.PropertyType;
            JsonConverter converter = this.GetConverter(contractSafe, property.MemberConverter);
            if ((converter != null) && converter.CanRead)
            {
                if ((!gottenCurrentValue && (target != null)) && property.Readable)
                {
                    currentValue = property.ValueProvider.GetValue(target);
                }
                return converter.ReadJson(reader, propertyType, currentValue, this.GetInternalSerializer());
            }
            return this.CreateValueInternal(reader, propertyType, contractSafe, property, currentValue);
        }

        public object Deserialize(JsonReader reader, Type objectType)
        {
            if (reader == null)
            {
                throw new ArgumentNullException("reader");
            }
            if (!((reader.TokenType != JsonToken.None) || this.ReadForType(reader, objectType, null)))
            {
                return null;
            }
            return this.CreateValueNonProperty(reader, objectType, this.GetContractSafe(objectType));
        }

        private JsonArrayContract EnsureArrayContract(Type objectType, JsonContract contract)
        {
            if (contract == null)
            {
                throw new JsonSerializationException("Could not resolve type '{0}' to a JsonContract.".FormatWith(CultureInfo.InvariantCulture, new object[] { objectType }));
            }
            JsonArrayContract contract2 = contract as JsonArrayContract;
            if (contract2 == null)
            {
                throw new JsonSerializationException("Cannot deserialize JSON array into type '{0}'.".FormatWith(CultureInfo.InvariantCulture, new object[] { objectType }));
            }
            return contract2;
        }

        private object EnsureType(object value, CultureInfo culture, Type targetType)
        {
            if ((targetType != null) && (ReflectionUtils.GetObjectType(value) != targetType))
            {
                try
                {
                    return ConvertUtils.ConvertOrCast(value, culture, targetType);
                }
                catch (Exception exception)
                {
                    throw new JsonSerializationException("Error converting value {0} to type '{1}'.".FormatWith(CultureInfo.InvariantCulture, new object[] { this.FormatValueForPrint(value), targetType }), exception);
                }
            }
            return value;
        }

        private string FormatValueForPrint(object value)
        {
            if (value == null)
            {
                return "{null}";
            }
            if (value is string)
            {
                return ("\"" + value + "\"");
            }
            return value.ToString();
        }

        private JsonContract GetContractSafe(Type type)
        {
            if (type == null)
            {
                return null;
            }
            return base.Serializer.ContractResolver.ResolveContract(type);
        }

        private JsonContract GetContractSafe(Type type, object value)
        {
            if (value == null)
            {
                return this.GetContractSafe(type);
            }
            return base.Serializer.ContractResolver.ResolveContract(value.GetType());
        }

        private JsonConverter GetConverter(JsonContract contract, JsonConverter memberConverter)
        {
            JsonConverter internalConverter = null;
            if (memberConverter != null)
            {
                return memberConverter;
            }
            if (contract != null)
            {
                if (contract.Converter != null)
                {
                    return contract.Converter;
                }
                JsonConverter matchingConverter = base.Serializer.GetMatchingConverter(contract.UnderlyingType);
                if (matchingConverter != null)
                {
                    return matchingConverter;
                }
                if (contract.InternalConverter != null)
                {
                    internalConverter = contract.InternalConverter;
                }
            }
            return internalConverter;
        }

        private JsonFormatterConverter GetFormatterConverter()
        {
            if (this._formatterConverter == null)
            {
                this._formatterConverter = new JsonFormatterConverter(this.GetInternalSerializer());
            }
            return this._formatterConverter;
        }

        private JsonSerializerProxy GetInternalSerializer()
        {
            if (this._internalSerializer == null)
            {
                this._internalSerializer = new JsonSerializerProxy(this);
            }
            return this._internalSerializer;
        }

        private void HandleError(JsonReader reader, int initialDepth)
        {
            base.ClearErrorContext();
            reader.Skip();
            while (reader.Depth > (initialDepth + 1))
            {
                reader.Read();
            }
        }

        private bool HasDefinedType(Type type)
        {
            return (((type != null) && (type != typeof(object))) && !typeof(JToken).IsAssignableFrom(type));
        }

        public void Populate(JsonReader reader, object target)
        {
            ValidationUtils.ArgumentNotNull(target, "target");
            Type type = target.GetType();
            JsonContract contract = base.Serializer.ContractResolver.ResolveContract(type);
            if (reader.TokenType == JsonToken.None)
            {
                reader.Read();
            }
            if (reader.TokenType == JsonToken.StartArray)
            {
                if (!(contract is JsonArrayContract))
                {
                    throw new JsonSerializationException("Cannot populate JSON array onto type '{0}'.".FormatWith(CultureInfo.InvariantCulture, new object[] { type }));
                }
                this.PopulateList(CollectionUtils.CreateCollectionWrapper(target), reader, null, (JsonArrayContract) contract);
            }
            else
            {
                if (reader.TokenType != JsonToken.StartObject)
                {
                    throw new JsonSerializationException("Unexpected initial token '{0}' when populating object. Expected JSON object or array.".FormatWith(CultureInfo.InvariantCulture, new object[] { reader.TokenType }));
                }
                this.CheckedRead(reader);
                string id = null;
                if ((reader.TokenType == JsonToken.PropertyName) && string.Equals(reader.Value.ToString(), "$id", StringComparison.Ordinal))
                {
                    this.CheckedRead(reader);
                    id = reader.Value.ToString();
                    this.CheckedRead(reader);
                }
                if (!(contract is JsonDictionaryContract))
                {
                    if (!(contract is JsonObjectContract))
                    {
                        throw new JsonSerializationException("Cannot populate JSON object onto type '{0}'.".FormatWith(CultureInfo.InvariantCulture, new object[] { type }));
                    }
                    this.PopulateObject(target, reader, (JsonObjectContract) contract, id);
                }
                else
                {
                    this.PopulateDictionary(CollectionUtils.CreateDictionaryWrapper(target), reader, (JsonDictionaryContract) contract, id);
                }
            }
        }

        private object PopulateDictionary(IWrappedDictionary dictionary, JsonReader reader, JsonDictionaryContract contract, string id)
        {
            if (id != null)
            {
                base.Serializer.ReferenceResolver.AddReference(this, id, dictionary.UnderlyingDictionary);
            }
            contract.InvokeOnDeserializing(dictionary.UnderlyingDictionary, base.Serializer.Context);
            int depth = reader.Depth;
            while (true)
            {
                switch (reader.TokenType)
                {
                    case JsonToken.PropertyName:
                        object obj2;
                        Exception exception;
                        try
                        {
                            obj2 = this.EnsureType(reader.Value, CultureInfo.InvariantCulture, contract.DictionaryKeyType);
                        }
                        catch (Exception exception1)
                        {
                            exception = exception1;
                            throw new JsonSerializationException("Could not convert string '{0}' to dictionary key type '{1}'. Create a TypeConverter to convert from the string to the key type object.".FormatWith(CultureInfo.InvariantCulture, new object[] { reader.Value, contract.DictionaryKeyType }), exception);
                        }
                        if (!this.ReadForType(reader, contract.DictionaryValueType, null))
                        {
                            throw new JsonSerializationException("Unexpected end when deserializing object.");
                        }
                        try
                        {
                            dictionary[obj2] = this.CreateValueNonProperty(reader, contract.DictionaryValueType, this.GetContractSafe(contract.DictionaryValueType));
                        }
                        catch (Exception exception2)
                        {
                            exception = exception2;
                            if (!base.IsErrorHandled(dictionary, contract, obj2, exception))
                            {
                                throw;
                            }
                            this.HandleError(reader, depth);
                        }
                        break;

                    case JsonToken.Comment:
                        break;

                    case JsonToken.EndObject:
                        contract.InvokeOnDeserialized(dictionary.UnderlyingDictionary, base.Serializer.Context);
                        return dictionary.UnderlyingDictionary;

                    default:
                        throw new JsonSerializationException("Unexpected token when deserializing object: " + reader.TokenType);
                }
                if (!reader.Read())
                {
                    throw new JsonSerializationException("Unexpected end when deserializing object.");
                }
            }
        }

        private object PopulateList(IWrappedCollection wrappedList, JsonReader reader, string reference, JsonArrayContract contract)
        {
            object underlyingCollection = wrappedList.UnderlyingCollection;
            if (wrappedList.IsFixedSize)
            {
                reader.Skip();
                return wrappedList.UnderlyingCollection;
            }
            if (reference != null)
            {
                base.Serializer.ReferenceResolver.AddReference(this, reference, underlyingCollection);
            }
            contract.InvokeOnDeserializing(underlyingCollection, base.Serializer.Context);
            int depth = reader.Depth;
            while (this.ReadForTypeArrayHack(reader, contract.CollectionItemType))
            {
                switch (reader.TokenType)
                {
                    case JsonToken.Comment:
                        break;

                    case JsonToken.EndArray:
                        contract.InvokeOnDeserialized(underlyingCollection, base.Serializer.Context);
                        return wrappedList.UnderlyingCollection;

                    default:
                        try
                        {
                            object obj3 = this.CreateValueNonProperty(reader, contract.CollectionItemType, this.GetContractSafe(contract.CollectionItemType));
                            wrappedList.Add(obj3);
                        }
                        catch (Exception exception)
                        {
                            if (!base.IsErrorHandled(underlyingCollection, contract, wrappedList.Count, exception))
                            {
                                throw;
                            }
                            this.HandleError(reader, depth);
                        }
                        break;
                }
            }
            throw new JsonSerializationException("Unexpected end when deserializing array.");
        }

        private object PopulateObject(object newObject, JsonReader reader, JsonObjectContract contract, string id)
        {
            contract.InvokeOnDeserializing(newObject, base.Serializer.Context);
            Dictionary<JsonProperty, RequiredValue> requiredProperties = (from m in contract.Properties
                where m.Required != Required.Default
                select m).ToDictionary<JsonProperty, JsonProperty, RequiredValue>(m => m, m => RequiredValue.None);
            if (id != null)
            {
                base.Serializer.ReferenceResolver.AddReference(this, id, newObject);
            }
            int depth = reader.Depth;
            while (true)
            {
                switch (reader.TokenType)
                {
                    case JsonToken.PropertyName:
                    {
                        string propertyName = reader.Value.ToString();
                        JsonProperty closestMatchProperty = contract.Properties.GetClosestMatchProperty(propertyName);
                        if (closestMatchProperty == null)
                        {
                            if (base.Serializer.MissingMemberHandling == MissingMemberHandling.Error)
                            {
                                throw new JsonSerializationException("Could not find member '{0}' on object of type '{1}'".FormatWith(CultureInfo.InvariantCulture, new object[] { propertyName, contract.UnderlyingType.Name }));
                            }
                            reader.Skip();
                            break;
                        }
                        if (!this.ReadForType(reader, closestMatchProperty.PropertyType, closestMatchProperty.Converter))
                        {
                            throw new JsonSerializationException("Unexpected end when setting {0}'s value.".FormatWith(CultureInfo.InvariantCulture, new object[] { propertyName }));
                        }
                        this.SetRequiredProperty(reader, closestMatchProperty, requiredProperties);
                        try
                        {
                            this.SetPropertyValue(closestMatchProperty, reader, newObject);
                        }
                        catch (Exception exception)
                        {
                            if (!base.IsErrorHandled(newObject, contract, propertyName, exception))
                            {
                                throw;
                            }
                            this.HandleError(reader, depth);
                        }
                        break;
                    }
                    case JsonToken.Comment:
                        break;

                    case JsonToken.EndObject:
                        foreach (KeyValuePair<JsonProperty, RequiredValue> pair in requiredProperties)
                        {
                            if (((RequiredValue) pair.Value) == RequiredValue.None)
                            {
                                throw new JsonSerializationException("Required property '{0}' not found in JSON.".FormatWith(CultureInfo.InvariantCulture, new object[] { pair.Key.PropertyName }));
                            }
                            if ((pair.Key.Required == Required.Always) && (((RequiredValue) pair.Value) == RequiredValue.Null))
                            {
                                throw new JsonSerializationException("Required property '{0}' expects a value but got null.".FormatWith(CultureInfo.InvariantCulture, new object[] { pair.Key.PropertyName }));
                            }
                        }
                        contract.InvokeOnDeserialized(newObject, base.Serializer.Context);
                        return newObject;

                    default:
                        throw new JsonSerializationException("Unexpected token when deserializing object: " + reader.TokenType);
                }
                if (!reader.Read())
                {
                    throw new JsonSerializationException("Unexpected end when deserializing object.");
                }
            }
        }

        private bool ReadForType(JsonReader reader, Type t, JsonConverter propertyConverter)
        {
            if (this.GetConverter(this.GetContractSafe(t), propertyConverter) != null)
            {
                return reader.Read();
            }
            if (t == typeof(byte[]))
            {
                reader.ReadAsBytes();
                return true;
            }
            if ((t == typeof(decimal)) || (t == typeof(decimal?)))
            {
                reader.ReadAsDecimal();
                return true;
            }
            if ((t == typeof(DateTimeOffset)) || (t == typeof(DateTimeOffset?)))
            {
                reader.ReadAsDateTimeOffset();
                return true;
            }
            do
            {
                if (!reader.Read())
                {
                    return false;
                }
            }
            while (reader.TokenType == JsonToken.Comment);
            return true;
        }

        private bool ReadForTypeArrayHack(JsonReader reader, Type t)
        {
            try
            {
                return this.ReadForType(reader, t, null);
            }
            catch (JsonReaderException)
            {
                if (reader.TokenType != JsonToken.EndArray)
                {
                    throw;
                }
                return true;
            }
        }

        private IDictionary<JsonProperty, object> ResolvePropertyAndConstructorValues(JsonObjectContract contract, JsonReader reader, Type objectType)
        {
            IDictionary<JsonProperty, object> dictionary = new Dictionary<JsonProperty, object>();
            bool flag = false;
            do
            {
                switch (reader.TokenType)
                {
                    case JsonToken.PropertyName:
                    {
                        string propertyName = reader.Value.ToString();
                        JsonProperty property = contract.ConstructorParameters.GetClosestMatchProperty(propertyName) ?? contract.Properties.GetClosestMatchProperty(propertyName);
                        if (property == null)
                        {
                            if (!reader.Read())
                            {
                                throw new JsonSerializationException("Unexpected end when setting {0}'s value.".FormatWith(CultureInfo.InvariantCulture, new object[] { propertyName }));
                            }
                            if (base.Serializer.MissingMemberHandling == MissingMemberHandling.Error)
                            {
                                throw new JsonSerializationException("Could not find member '{0}' on object of type '{1}'".FormatWith(CultureInfo.InvariantCulture, new object[] { propertyName, objectType.Name }));
                            }
                            reader.Skip();
                            break;
                        }
                        if (!this.ReadForType(reader, property.PropertyType, property.Converter))
                        {
                            throw new JsonSerializationException("Unexpected end when setting {0}'s value.".FormatWith(CultureInfo.InvariantCulture, new object[] { propertyName }));
                        }
                        if (!property.Ignored)
                        {
                            dictionary[property] = this.CreateValueProperty(reader, property, null, true, null);
                        }
                        else
                        {
                            reader.Skip();
                        }
                        break;
                    }
                    case JsonToken.Comment:
                        break;

                    case JsonToken.EndObject:
                        flag = true;
                        break;

                    default:
                        throw new JsonSerializationException("Unexpected token when deserializing object: " + reader.TokenType);
                }
            }
            while (!flag && reader.Read());
            return dictionary;
        }

        private void SetPropertyValue(JsonProperty property, JsonReader reader, object target)
        {
            if (property.Ignored)
            {
                reader.Skip();
            }
            else
            {
                object obj2 = null;
                bool flag = false;
                bool gottenCurrentValue = false;
                ObjectCreationHandling valueOrDefault = property.ObjectCreationHandling.GetValueOrDefault(base.Serializer.ObjectCreationHandling);
                if ((((valueOrDefault == ObjectCreationHandling.Auto) || (valueOrDefault == ObjectCreationHandling.Reuse)) && ((reader.TokenType == JsonToken.StartArray) || (reader.TokenType == JsonToken.StartObject))) && property.Readable)
                {
                    obj2 = property.ValueProvider.GetValue(target);
                    gottenCurrentValue = true;
                    flag = (((obj2 != null) && !property.PropertyType.IsArray) && !ReflectionUtils.InheritsGenericDefinition(property.PropertyType, typeof(ReadOnlyCollection<>))) && !property.PropertyType.IsValueType;
                }
                if (!(property.Writable || flag))
                {
                    reader.Skip();
                }
                else if ((((NullValueHandling) property.NullValueHandling.GetValueOrDefault(base.Serializer.NullValueHandling)) == NullValueHandling.Ignore) && (reader.TokenType == JsonToken.Null))
                {
                    reader.Skip();
                }
                else if (((((DefaultValueHandling) property.DefaultValueHandling.GetValueOrDefault(base.Serializer.DefaultValueHandling)) == DefaultValueHandling.Ignore) && JsonReader.IsPrimitiveToken(reader.TokenType)) && object.Equals(reader.Value, property.DefaultValue))
                {
                    reader.Skip();
                }
                else
                {
                    object currentValue = flag ? obj2 : null;
                    object obj4 = this.CreateValueProperty(reader, property, target, gottenCurrentValue, currentValue);
                    if ((!flag || (obj4 != obj2)) && this.ShouldSetPropertyValue(property, obj4))
                    {
                        property.ValueProvider.SetValue(target, obj4);
                        if (property.SetIsSpecified != null)
                        {
                            property.SetIsSpecified(target, true);
                        }
                    }
                }
            }
        }

        private void SetRequiredProperty(JsonReader reader, JsonProperty property, Dictionary<JsonProperty, RequiredValue> requiredProperties)
        {
            if (property != null)
            {
                requiredProperties[property] = ((reader.TokenType == JsonToken.Null) || (reader.TokenType == JsonToken.Undefined)) ? RequiredValue.Null : RequiredValue.Value;
            }
        }

        private bool ShouldSetPropertyValue(JsonProperty property, object value)
        {
            if ((((NullValueHandling) property.NullValueHandling.GetValueOrDefault(base.Serializer.NullValueHandling)) == NullValueHandling.Ignore) && (value == null))
            {
                return false;
            }
            if ((((DefaultValueHandling) property.DefaultValueHandling.GetValueOrDefault(base.Serializer.DefaultValueHandling)) == DefaultValueHandling.Ignore) && MiscellaneousUtils.ValueEquals(value, property.DefaultValue))
            {
                return false;
            }
            if (!property.Writable)
            {
                return false;
            }
            return true;
        }

        internal enum RequiredValue
        {
            None,
            Null,
            Value
        }
    }
}

