namespace Newtonsoft.Json.Serialization
{
    using Newtonsoft.Json.Utilities;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Reflection;
    using System.Runtime.CompilerServices;

    public class JsonDictionaryContract : JsonContract
    {
        private readonly Type _genericCollectionDefinitionType;
        private MethodCall<object, object> _genericWrapperCreator;
        private Type _genericWrapperType;

        public JsonDictionaryContract(Type underlyingType) : base(underlyingType)
        {
            Type type;
            Type type2;
            if (ReflectionUtils.ImplementsGenericDefinition(underlyingType, typeof(IDictionary<,>), out this._genericCollectionDefinitionType))
            {
                type = this._genericCollectionDefinitionType.GetGenericArguments()[0];
                type2 = this._genericCollectionDefinitionType.GetGenericArguments()[1];
            }
            else
            {
                ReflectionUtils.GetDictionaryKeyValueTypes(base.UnderlyingType, out type, out type2);
            }
            this.DictionaryKeyType = type;
            this.DictionaryValueType = type2;
            if (this.IsTypeGenericDictionaryInterface(base.UnderlyingType))
            {
                base.CreatedType = ReflectionUtils.MakeGenericType(typeof(Dictionary<,>), new Type[] { type, type2 });
            }
        }

        internal IWrappedDictionary CreateWrapper(object dictionary)
        {
            if (dictionary is IDictionary)
            {
                return new DictionaryWrapper<object, object>((IDictionary) dictionary);
            }
            if (this._genericWrapperType == null)
            {
                this._genericWrapperType = ReflectionUtils.MakeGenericType(typeof(DictionaryWrapper<,>), new Type[] { this.DictionaryKeyType, this.DictionaryValueType });
                ConstructorInfo constructor = this._genericWrapperType.GetConstructor(new Type[] { this._genericCollectionDefinitionType });
                this._genericWrapperCreator = JsonTypeReflector.ReflectionDelegateFactory.CreateMethodCall<object>(constructor);
            }
            return (IWrappedDictionary) this._genericWrapperCreator(null, new object[] { dictionary });
        }

        private bool IsTypeGenericDictionaryInterface(Type type)
        {
            if (!type.IsGenericType)
            {
                return false;
            }
            return (type.GetGenericTypeDefinition() == typeof(IDictionary<,>));
        }

        internal Type DictionaryKeyType { get; private set; }

        internal Type DictionaryValueType { get; private set; }

        public Func<string, string> PropertyNameResolver { get; set; }
    }
}

