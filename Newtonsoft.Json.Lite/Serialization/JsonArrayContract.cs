namespace Newtonsoft.Json.Serialization
{
    using Newtonsoft.Json.Utilities;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using System.Runtime.CompilerServices;

    public class JsonArrayContract : JsonContract
    {
        private readonly Type _genericCollectionDefinitionType;
        private MethodCall<object, object> _genericWrapperCreator;
        private Type _genericWrapperType;
        private readonly bool _isCollectionItemTypeNullableType;

        public JsonArrayContract(Type underlyingType) : base(underlyingType)
        {
            if (ReflectionUtils.ImplementsGenericDefinition(underlyingType, typeof(ICollection<>), out this._genericCollectionDefinitionType))
            {
                this.CollectionItemType = this._genericCollectionDefinitionType.GetGenericArguments()[0];
            }
            else
            {
                this.CollectionItemType = ReflectionUtils.GetCollectionItemType(base.UnderlyingType);
            }
            if (this.CollectionItemType != null)
            {
                this._isCollectionItemTypeNullableType = ReflectionUtils.IsNullableType(this.CollectionItemType);
            }
            if (this.IsTypeGenericCollectionInterface(base.UnderlyingType))
            {
                base.CreatedType = ReflectionUtils.MakeGenericType(typeof(List<>), new Type[] { this.CollectionItemType });
            }
        }

        internal IWrappedCollection CreateWrapper(object list)
        {
            if (((list is IList) && ((this.CollectionItemType == null) || !this._isCollectionItemTypeNullableType)) || base.UnderlyingType.IsArray)
            {
                return new CollectionWrapper<object>((IList) list);
            }
            if (this._genericCollectionDefinitionType != null)
            {
                this.EnsureGenericWrapperCreator();
                return (IWrappedCollection) this._genericWrapperCreator(null, new object[] { list });
            }
            IList list2 = ((IEnumerable) list).Cast<object>().ToList<object>();
            if (this.CollectionItemType != null)
            {
                Array array = Array.CreateInstance(this.CollectionItemType, list2.Count);
                for (int i = 0; i < list2.Count; i++)
                {
                    array.SetValue(list2[i], i);
                }
                list2 = array;
            }
            return new CollectionWrapper<object>(list2);
        }

        private void EnsureGenericWrapperCreator()
        {
            if (this._genericWrapperType == null)
            {
                this._genericWrapperType = ReflectionUtils.MakeGenericType(typeof(CollectionWrapper<>), new Type[] { this.CollectionItemType });
                Type type = ReflectionUtils.InheritsGenericDefinition(this._genericCollectionDefinitionType, typeof(List<>)) ? ReflectionUtils.MakeGenericType(typeof(ICollection<>), new Type[] { this.CollectionItemType }) : this._genericCollectionDefinitionType;
                ConstructorInfo constructor = this._genericWrapperType.GetConstructor(new Type[] { type });
                this._genericWrapperCreator = JsonTypeReflector.ReflectionDelegateFactory.CreateMethodCall<object>(constructor);
            }
        }

        private bool IsTypeGenericCollectionInterface(Type type)
        {
            if (!type.IsGenericType)
            {
                return false;
            }
            Type genericTypeDefinition = type.GetGenericTypeDefinition();
            return (((genericTypeDefinition == typeof(IList<>)) || (genericTypeDefinition == typeof(ICollection<>))) || (genericTypeDefinition == typeof(IEnumerable<>)));
        }

        internal Type CollectionItemType { get; private set; }
    }
}

