namespace Newtonsoft.Json.Serialization
{
    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;
    using Newtonsoft.Json.Utilities;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Diagnostics.CodeAnalysis;
    using System.Globalization;
    using System.Linq;
    using System.Runtime.InteropServices;
    using System.Runtime.Serialization;
    using System.Security;

    internal class JsonSerializerInternalWriter : JsonSerializerInternalBase
    {
        private JsonSerializerProxy _internalSerializer;
        private List<object> _serializeStack;

        public JsonSerializerInternalWriter(JsonSerializer serializer) : base(serializer)
        {
        }

        private bool CheckForCircularReference(object value, ReferenceLoopHandling? referenceLoopHandling, JsonContract contract)
        {
            if (((value != null) && !(contract is JsonPrimitiveContract)) && (this.SerializeStack.IndexOf(value) != -1))
            {
                switch (referenceLoopHandling.GetValueOrDefault(base.Serializer.ReferenceLoopHandling))
                {
                    case ReferenceLoopHandling.Error:
                        throw new JsonSerializationException("Self referencing loop detected for type '{0}'.".FormatWith(CultureInfo.InvariantCulture, new object[] { value.GetType() }));

                    case ReferenceLoopHandling.Ignore:
                        return false;

                    case ReferenceLoopHandling.Serialize:
                        return true;
                }
                throw new InvalidOperationException("Unexpected ReferenceLoopHandling value: '{0}'".FormatWith(CultureInfo.InvariantCulture, new object[] { base.Serializer.ReferenceLoopHandling }));
            }
            return true;
        }

        private JsonContract GetContractSafe(object value)
        {
            if (value == null)
            {
                return null;
            }
            return base.Serializer.ContractResolver.ResolveContract(value.GetType());
        }

        private JsonSerializerProxy GetInternalSerializer()
        {
            if (this._internalSerializer == null)
            {
                this._internalSerializer = new JsonSerializerProxy(this);
            }
            return this._internalSerializer;
        }

        private string GetPropertyName(DictionaryEntry entry)
        {
            string str;
            if (entry.Key is IConvertible)
            {
                return Convert.ToString(entry.Key, CultureInfo.InvariantCulture);
            }
            if (TryConvertToString(entry.Key, entry.Key.GetType(), out str))
            {
                return str;
            }
            return entry.Key.ToString();
        }

        private void HandleError(JsonWriter writer, int initialDepth)
        {
            base.ClearErrorContext();
            while (writer.Top > initialDepth)
            {
                writer.WriteEnd();
            }
        }

        private bool HasFlag(PreserveReferencesHandling value, PreserveReferencesHandling flag)
        {
            return ((value & flag) == flag);
        }

        private bool HasFlag(TypeNameHandling value, TypeNameHandling flag)
        {
            return ((value & flag) == flag);
        }

        private bool IsSpecified(JsonProperty property, object target)
        {
            return ((property.GetIsSpecified == null) || property.GetIsSpecified(target));
        }

        public void Serialize(JsonWriter jsonWriter, object value)
        {
            if (jsonWriter == null)
            {
                throw new ArgumentNullException("jsonWriter");
            }
            this.SerializeValue(jsonWriter, value, this.GetContractSafe(value), null, null);
        }

        private void SerializeConvertable(JsonWriter writer, JsonConverter converter, object value, JsonContract contract)
        {
            if (this.ShouldWriteReference(value, null, contract))
            {
                this.WriteReference(writer, value);
            }
            else if (this.CheckForCircularReference(value, null, contract))
            {
                this.SerializeStack.Add(value);
                converter.WriteJson(writer, value, this.GetInternalSerializer());
                this.SerializeStack.RemoveAt(this.SerializeStack.Count - 1);
            }
        }

        private void SerializeDictionary(JsonWriter writer, IWrappedDictionary values, JsonDictionaryContract contract, JsonProperty member, JsonContract collectionValueContract)
        {
            contract.InvokeOnSerializing(values.UnderlyingDictionary, base.Serializer.Context);
            this.SerializeStack.Add(values.UnderlyingDictionary);
            writer.WriteStartObject();
            bool? isReference = contract.IsReference;
            if (isReference.HasValue ? isReference.GetValueOrDefault() : this.HasFlag(base.Serializer.PreserveReferencesHandling, PreserveReferencesHandling.Objects))
            {
                writer.WritePropertyName("$id");
                writer.WriteValue(base.Serializer.ReferenceResolver.GetReference(this, values.UnderlyingDictionary));
            }
            if (this.ShouldWriteType(TypeNameHandling.Objects, contract, member, collectionValueContract))
            {
                this.WriteTypeProperty(writer, values.UnderlyingDictionary.GetType());
            }
            JsonContract contract2 = base.Serializer.ContractResolver.ResolveContract(contract.DictionaryValueType ?? typeof(object));
            int top = writer.Top;
            IDictionary dictionary = values;
            foreach (DictionaryEntry entry in dictionary)
            {
                string propertyName = this.GetPropertyName(entry);
                propertyName = (contract.PropertyNameResolver != null) ? contract.PropertyNameResolver(propertyName) : propertyName;
                try
                {
                    object obj2 = entry.Value;
                    JsonContract contractSafe = this.GetContractSafe(obj2);
                    if (this.ShouldWriteReference(obj2, null, contractSafe))
                    {
                        writer.WritePropertyName(propertyName);
                        this.WriteReference(writer, obj2);
                    }
                    else
                    {
                        ReferenceLoopHandling? referenceLoopHandling = null;
                        if (this.CheckForCircularReference(obj2, referenceLoopHandling, contract))
                        {
                            writer.WritePropertyName(propertyName);
                            this.SerializeValue(writer, obj2, contractSafe, null, contract2);
                        }
                    }
                }
                catch (Exception exception)
                {
                    if (!base.IsErrorHandled(values.UnderlyingDictionary, contract, propertyName, exception))
                    {
                        throw;
                    }
                    this.HandleError(writer, top);
                }
            }
            writer.WriteEndObject();
            this.SerializeStack.RemoveAt(this.SerializeStack.Count - 1);
            contract.InvokeOnSerialized(values.UnderlyingDictionary, base.Serializer.Context);
        }

        [SecuritySafeCritical, SuppressMessage("Microsoft.Portability", "CA1903:UseOnlyApiFromTargetedFramework", MessageId="System.Security.SecuritySafeCriticalAttribute")]
        private void SerializeISerializable(JsonWriter writer, ISerializable value, JsonISerializableContract contract)
        {
            contract.InvokeOnSerializing(value, base.Serializer.Context);
            this.SerializeStack.Add(value);
            writer.WriteStartObject();
            SerializationInfo info = new SerializationInfo(contract.UnderlyingType, new FormatterConverter());
            value.GetObjectData(info, base.Serializer.Context);
            SerializationInfoEnumerator enumerator = info.GetEnumerator();
            while (enumerator.MoveNext())
            {
                SerializationEntry current = enumerator.Current;
                writer.WritePropertyName(current.Name);
                this.SerializeValue(writer, current.Value, this.GetContractSafe(current.Value), null, null);
            }
            writer.WriteEndObject();
            this.SerializeStack.RemoveAt(this.SerializeStack.Count - 1);
            contract.InvokeOnSerialized(value, base.Serializer.Context);
        }

        private void SerializeList(JsonWriter writer, IWrappedCollection values, JsonArrayContract contract, JsonProperty member, JsonContract collectionValueContract)
        {
            contract.InvokeOnSerializing(values.UnderlyingCollection, base.Serializer.Context);
            this.SerializeStack.Add(values.UnderlyingCollection);
            bool? isReference = contract.IsReference;
            bool flag = isReference.HasValue ? isReference.GetValueOrDefault() : this.HasFlag(base.Serializer.PreserveReferencesHandling, PreserveReferencesHandling.Arrays);
            bool flag2 = this.ShouldWriteType(TypeNameHandling.Arrays, contract, member, collectionValueContract);
            if (flag || flag2)
            {
                writer.WriteStartObject();
                if (flag)
                {
                    writer.WritePropertyName("$id");
                    writer.WriteValue(base.Serializer.ReferenceResolver.GetReference(this, values.UnderlyingCollection));
                }
                if (flag2)
                {
                    this.WriteTypeProperty(writer, values.UnderlyingCollection.GetType());
                }
                writer.WritePropertyName("$values");
            }
            JsonContract contract2 = base.Serializer.ContractResolver.ResolveContract(contract.CollectionItemType ?? typeof(object));
            writer.WriteStartArray();
            int top = writer.Top;
            int keyValue = 0;
            foreach (object obj2 in values)
            {
                try
                {
                    JsonContract contractSafe = this.GetContractSafe(obj2);
                    if (this.ShouldWriteReference(obj2, null, contractSafe))
                    {
                        this.WriteReference(writer, obj2);
                    }
                    else
                    {
                        ReferenceLoopHandling? referenceLoopHandling = null;
                        if (this.CheckForCircularReference(obj2, referenceLoopHandling, contract))
                        {
                            this.SerializeValue(writer, obj2, contractSafe, null, contract2);
                        }
                    }
                }
                catch (Exception exception)
                {
                    if (!base.IsErrorHandled(values.UnderlyingCollection, contract, keyValue, exception))
                    {
                        throw;
                    }
                    this.HandleError(writer, top);
                }
                finally
                {
                    keyValue++;
                }
            }
            writer.WriteEndArray();
            if (flag || flag2)
            {
                writer.WriteEndObject();
            }
            this.SerializeStack.RemoveAt(this.SerializeStack.Count - 1);
            contract.InvokeOnSerialized(values.UnderlyingCollection, base.Serializer.Context);
        }

        private void SerializeObject(JsonWriter writer, object value, JsonObjectContract contract, JsonProperty member, JsonContract collectionValueContract)
        {
            contract.InvokeOnSerializing(value, base.Serializer.Context);
            this.SerializeStack.Add(value);
            writer.WriteStartObject();
            bool? isReference = contract.IsReference;
            if (isReference.HasValue ? isReference.GetValueOrDefault() : this.HasFlag(base.Serializer.PreserveReferencesHandling, PreserveReferencesHandling.Objects))
            {
                writer.WritePropertyName("$id");
                writer.WriteValue(base.Serializer.ReferenceResolver.GetReference(this, value));
            }
            if (this.ShouldWriteType(TypeNameHandling.Objects, contract, member, collectionValueContract))
            {
                this.WriteTypeProperty(writer, contract.UnderlyingType);
            }
            int top = writer.Top;
            foreach (JsonProperty property in contract.Properties)
            {
                try
                {
                    if (((!property.Ignored && property.Readable) && this.ShouldSerialize(property, value)) && this.IsSpecified(property, value))
                    {
                        object obj2 = property.ValueProvider.GetValue(value);
                        JsonContract contractSafe = this.GetContractSafe(obj2);
                        this.WriteMemberInfoProperty(writer, obj2, property, contractSafe);
                    }
                }
                catch (Exception exception)
                {
                    if (!base.IsErrorHandled(value, contract, property.PropertyName, exception))
                    {
                        throw;
                    }
                    this.HandleError(writer, top);
                }
            }
            writer.WriteEndObject();
            this.SerializeStack.RemoveAt(this.SerializeStack.Count - 1);
            contract.InvokeOnSerialized(value, base.Serializer.Context);
        }

        private void SerializePrimitive(JsonWriter writer, object value, JsonPrimitiveContract contract, JsonProperty member, JsonContract collectionValueContract)
        {
            if ((contract.UnderlyingType == typeof(byte[])) && this.ShouldWriteType(TypeNameHandling.Objects, contract, member, collectionValueContract))
            {
                writer.WriteStartObject();
                this.WriteTypeProperty(writer, contract.CreatedType);
                writer.WritePropertyName("$value");
                writer.WriteValue(value);
                writer.WriteEndObject();
            }
            else
            {
                writer.WriteValue(value);
            }
        }

        private void SerializeString(JsonWriter writer, object value, JsonStringContract contract)
        {
            string str;
            contract.InvokeOnSerializing(value, base.Serializer.Context);
            TryConvertToString(value, contract.UnderlyingType, out str);
            writer.WriteValue(str);
            contract.InvokeOnSerialized(value, base.Serializer.Context);
        }

        private void SerializeValue(JsonWriter writer, object value, JsonContract valueContract, JsonProperty member, JsonContract collectionValueContract)
        {
            JsonConverter converter = (member != null) ? member.Converter : null;
            if (value == null)
            {
                writer.WriteNull();
            }
            else if ((((converter != null) || ((converter = valueContract.Converter) != null)) || (((converter = base.Serializer.GetMatchingConverter(valueContract.UnderlyingType)) != null) || ((converter = valueContract.InternalConverter) != null))) && converter.CanWrite)
            {
                this.SerializeConvertable(writer, converter, value, valueContract);
            }
            else if (valueContract is JsonPrimitiveContract)
            {
                this.SerializePrimitive(writer, value, (JsonPrimitiveContract) valueContract, member, collectionValueContract);
            }
            else if (valueContract is JsonStringContract)
            {
                this.SerializeString(writer, value, (JsonStringContract) valueContract);
            }
            else if (valueContract is JsonObjectContract)
            {
                this.SerializeObject(writer, value, (JsonObjectContract) valueContract, member, collectionValueContract);
            }
            else if (valueContract is JsonDictionaryContract)
            {
                JsonDictionaryContract contract = (JsonDictionaryContract) valueContract;
                this.SerializeDictionary(writer, contract.CreateWrapper(value), contract, member, collectionValueContract);
            }
            else if (valueContract is JsonArrayContract)
            {
                JsonArrayContract contract2 = (JsonArrayContract) valueContract;
                this.SerializeList(writer, contract2.CreateWrapper(value), contract2, member, collectionValueContract);
            }
            else if (valueContract is JsonLinqContract)
            {
                ((JToken) value).WriteTo(writer, (base.Serializer.Converters != null) ? base.Serializer.Converters.ToArray<JsonConverter>() : null);
            }
            else if (valueContract is JsonISerializableContract)
            {
                this.SerializeISerializable(writer, (ISerializable) value, (JsonISerializableContract) valueContract);
            }
        }

        private bool ShouldSerialize(JsonProperty property, object target)
        {
            return ((property.ShouldSerialize == null) || property.ShouldSerialize(target));
        }

        private bool ShouldWriteReference(object value, JsonProperty property, JsonContract contract)
        {
            if (value == null)
            {
                return false;
            }
            if (contract is JsonPrimitiveContract)
            {
                return false;
            }
            bool? isReference = null;
            if (property != null)
            {
                isReference = property.IsReference;
            }
            if (!isReference.HasValue)
            {
                isReference = contract.IsReference;
            }
            if (!isReference.HasValue)
            {
                if (contract is JsonArrayContract)
                {
                    isReference = new bool?(this.HasFlag(base.Serializer.PreserveReferencesHandling, PreserveReferencesHandling.Arrays));
                }
                else
                {
                    isReference = new bool?(this.HasFlag(base.Serializer.PreserveReferencesHandling, PreserveReferencesHandling.Objects));
                }
            }
            if (!isReference.Value)
            {
                return false;
            }
            return base.Serializer.ReferenceResolver.IsReferenced(this, value);
        }

        private bool ShouldWriteType(TypeNameHandling typeNameHandlingFlag, JsonContract contract, JsonProperty member, JsonContract collectionValueContract)
        {
            TypeNameHandling? typeNameHandling = (member != null) ? member.TypeNameHandling : null;
            if (this.HasFlag(typeNameHandling.HasValue ? typeNameHandling.GetValueOrDefault() : base.Serializer.TypeNameHandling, typeNameHandlingFlag))
            {
                return true;
            }
            if (member != null)
            {
                typeNameHandling = member.TypeNameHandling;
                if ((((TypeNameHandling) (typeNameHandling.HasValue ? typeNameHandling.GetValueOrDefault() : base.Serializer.TypeNameHandling)) == TypeNameHandling.Auto) && (contract.UnderlyingType != member.PropertyType))
                {
                    return true;
                }
            }
            else if ((collectionValueContract != null) && ((base.Serializer.TypeNameHandling == TypeNameHandling.Auto) && (contract.UnderlyingType != collectionValueContract.UnderlyingType)))
            {
                return true;
            }
            return false;
        }

        internal static bool TryConvertToString(object value, Type type, out string s)
        {
            TypeConverter converter = ConvertUtils.GetConverter(type);
            if ((((converter != null) && !(converter is ComponentConverter)) && (converter.GetType() != typeof(TypeConverter))) && converter.CanConvertTo(typeof(string)))
            {
                s = converter.ConvertToInvariantString(value);
                return true;
            }
            if (value is Type)
            {
                s = ((Type) value).AssemblyQualifiedName;
                return true;
            }
            s = null;
            return false;
        }

        private void WriteMemberInfoProperty(JsonWriter writer, object memberValue, JsonProperty property, JsonContract contract)
        {
            string propertyName = property.PropertyName;
            object defaultValue = property.DefaultValue;
            if (((((NullValueHandling) property.NullValueHandling.GetValueOrDefault(base.Serializer.NullValueHandling)) != NullValueHandling.Ignore) || (memberValue != null)) && ((((DefaultValueHandling) property.DefaultValueHandling.GetValueOrDefault(base.Serializer.DefaultValueHandling)) != DefaultValueHandling.Ignore) || !MiscellaneousUtils.ValueEquals(memberValue, defaultValue)))
            {
                if (this.ShouldWriteReference(memberValue, property, contract))
                {
                    writer.WritePropertyName(propertyName);
                    this.WriteReference(writer, memberValue);
                }
                else if (this.CheckForCircularReference(memberValue, property.ReferenceLoopHandling, contract))
                {
                    if ((memberValue == null) && (property.Required == Required.Always))
                    {
                        throw new JsonSerializationException("Cannot write a null value for property '{0}'. Property requires a value.".FormatWith(CultureInfo.InvariantCulture, new object[] { property.PropertyName }));
                    }
                    writer.WritePropertyName(propertyName);
                    this.SerializeValue(writer, memberValue, contract, property, null);
                }
            }
        }

        private void WriteReference(JsonWriter writer, object value)
        {
            writer.WriteStartObject();
            writer.WritePropertyName("$ref");
            writer.WriteValue(base.Serializer.ReferenceResolver.GetReference(this, value));
            writer.WriteEndObject();
        }

        private void WriteTypeProperty(JsonWriter writer, Type type)
        {
            writer.WritePropertyName("$type");
            writer.WriteValue(ReflectionUtils.GetTypeName(type, base.Serializer.TypeNameAssemblyFormat));
        }

        private List<object> SerializeStack
        {
            get
            {
                if (this._serializeStack == null)
                {
                    this._serializeStack = new List<object>();
                }
                return this._serializeStack;
            }
        }
    }
}

