using Newtonsoft.Json.Utilities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Linq;

namespace Newtonsoft.Json.Linq
{
    public class JObject : JContainer, IDictionary<string, JToken>, ICollection<KeyValuePair<string, JToken>>, IEnumerable<KeyValuePair<string, JToken>>, IEnumerable, INotifyPropertyChanged, ICustomTypeDescriptor, INotifyPropertyChanging
    {
        private class JPropertKeyedCollection : KeyedCollection<string, JToken>
        {
            public new IDictionary<string, JToken> Dictionary
            {
                get
                {
                    return base.Dictionary;
                }
            }

            public JPropertKeyedCollection(IEqualityComparer<string> comparer)
                : base(comparer)
            {
            }

            protected override string GetKeyForItem(JToken item)
            {
                return ((JProperty)item).Name;
            }

            protected override void InsertItem(int index, JToken item)
            {
                if (this.Dictionary == null)
                {
                    base.InsertItem(index, item);
                }
                else
                {
                    string keyForItem = this.GetKeyForItem(item);
                    this.Dictionary[keyForItem] = item;
                    base.Items.Insert(index, item);
                }
            }
        }

        private JObject.JPropertKeyedCollection _properties = new JObject.JPropertKeyedCollection(StringComparer.Ordinal);

        public event PropertyChangedEventHandler PropertyChanged;

        public event PropertyChangingEventHandler PropertyChanging;

        protected override IList<JToken> ChildrenTokens
        {
            get
            {
                return this._properties;
            }
        }

        public override JTokenType Type
        {
            get
            {
                return JTokenType.Object;
            }
        }

        public override JToken this[object key]
        {
            get
            {
                ValidationUtils.ArgumentNotNull(key, "o");
                string text = key as string;
                if (text == null)
                {
                    throw new ArgumentException("Accessed JObject values with invalid key value: {0}. Object property name expected.".FormatWith(CultureInfo.InvariantCulture, new object[]
					{
						MiscellaneousUtils.ToString(key)
					}));
                }
                return this[text];
            }
            set
            {
                ValidationUtils.ArgumentNotNull(key, "o");
                string text = key as string;
                if (text == null)
                {
                    throw new ArgumentException("Set JObject values with invalid key value: {0}. Object property name expected.".FormatWith(CultureInfo.InvariantCulture, new object[]
					{
						MiscellaneousUtils.ToString(key)
					}));
                }
                this[text] = value;
            }
        }

        public JToken this[string propertyName]
        {
            get
            {
                ValidationUtils.ArgumentNotNull(propertyName, "propertyName");
                JProperty jProperty = this.Property(propertyName);
                return (jProperty != null) ? jProperty.Value : null;
            }
            set
            {
                JProperty jProperty = this.Property(propertyName);
                if (jProperty != null)
                {
                    jProperty.Value = value;
                }
                else
                {
                    this.OnPropertyChanging(propertyName);
                    this.Add(new JProperty(propertyName, value));
                    this.OnPropertyChanged(propertyName);
                }
            }
        }

        ICollection<string> IDictionary<string, JToken>.Keys
        {
            get
            {
                return this._properties.Dictionary.Keys;
            }
        }

        ICollection<JToken> IDictionary<string, JToken>.Values
        {
            get
            {
                return this._properties.Dictionary.Values;
            }
        }

        public new int Count
        {
            get
            {
                return this.ChildrenTokens.Count;
            }
        }

        bool ICollection<KeyValuePair<string, JToken>>.IsReadOnly
        {
            get
            {
                return false;
            }
        }

        public JObject()
        {
        }

        public JObject(JObject other)
            : base(other)
        {
        }

        public JObject(params object[] content)
            : this((object)content)
        {
        }

        public JObject(object content)
        {
            this.Add(content);
        }

        internal override bool DeepEquals(JToken node)
        {
            JObject jObject = node as JObject;
            return jObject != null && base.ContentsEqual(jObject);
        }

        internal override void ValidateToken(JToken o, JToken existing)
        {
            ValidationUtils.ArgumentNotNull(o, "o");
            if (o.Type != JTokenType.Property)
            {
                throw new ArgumentException("Can not add {0} to {1}.".FormatWith(CultureInfo.InvariantCulture, new object[]
				{
					o.GetType(),
					base.GetType()
				}));
            }
            JProperty jProperty = (JProperty)o;
            if (existing != null)
            {
                JProperty jProperty2 = (JProperty)existing;
                if (jProperty.Name == jProperty2.Name)
                {
                    return;
                }
            }
            if (this._properties.Dictionary != null && this._properties.Dictionary.TryGetValue(jProperty.Name, out existing))
            {
                throw new ArgumentException("Can not add property {0} to {1}. Property with the same name already exists on object.".FormatWith(CultureInfo.InvariantCulture, new object[]
				{
					jProperty.Name,
					base.GetType()
				}));
            }
        }

        internal void InternalPropertyChanged(JProperty childProperty)
        {
            this.OnPropertyChanged(childProperty.Name);
            this.OnListChanged(new ListChangedEventArgs(ListChangedType.ItemChanged, base.IndexOfItem(childProperty)));
        }

        internal void InternalPropertyChanging(JProperty childProperty)
        {
            this.OnPropertyChanging(childProperty.Name);
        }

        internal override JToken CloneToken()
        {
            return new JObject(this);
        }

        public IEnumerable<JProperty> Properties()
        {
            return this.ChildrenTokens.Cast<JProperty>();
        }

        public JProperty Property(string name)
        {
            JProperty result;
            if (this._properties.Dictionary == null)
            {
                result = null;
            }
            else if (name == null)
            {
                result = null;
            }
            else
            {
                JToken jToken;
                this._properties.Dictionary.TryGetValue(name, out jToken);
                result = (JProperty)jToken;
            }
            return result;
        }

        public JEnumerable<JToken> PropertyValues()
        {
            return new JEnumerable<JToken>(from p in this.Properties()
                                           select p.Value);
        }

        public new static JObject Load(JsonReader reader)
        {
            ValidationUtils.ArgumentNotNull(reader, "reader");
            if (reader.TokenType == JsonToken.None)
            {
                if (!reader.Read())
                {
                    throw new Exception("Error reading JObject from JsonReader.");
                }
            }
            if (reader.TokenType != JsonToken.StartObject)
            {
                throw new Exception("Error reading JObject from JsonReader. Current JsonReader item is not an object: {0}".FormatWith(CultureInfo.InvariantCulture, new object[]
				{
					reader.TokenType
				}));
            }
            JObject jObject = new JObject();
            jObject.SetLineInfo(reader as IJsonLineInfo);
            jObject.ReadTokenFrom(reader);
            return jObject;
        }

        public new static JObject Parse(string json)
        {
            JsonReader reader = new JsonTextReader(new StringReader(json));
            return JObject.Load(reader);
        }

        public new static JObject FromObject(object o)
        {
            return JObject.FromObject(o, new JsonSerializer());
        }

        public new static JObject FromObject(object o, JsonSerializer jsonSerializer)
        {
            JToken jToken = JToken.FromObjectInternal(o, jsonSerializer);
            if (jToken != null && jToken.Type != JTokenType.Object)
            {
                throw new ArgumentException("Object serialized to {0}. JObject instance expected.".FormatWith(CultureInfo.InvariantCulture, new object[]
				{
					jToken.Type
				}));
            }
            return (JObject)jToken;
        }

        public override void WriteTo(JsonWriter writer, params JsonConverter[] converters)
        {
            writer.WriteStartObject();
            using (IEnumerator<JToken> enumerator = this.ChildrenTokens.GetEnumerator())
            {
                while (enumerator.MoveNext())
                {
                    JProperty jProperty = (JProperty)enumerator.Current;
                    jProperty.WriteTo(writer, converters);
                }
            }
            writer.WriteEndObject();
        }

        public void Add(string propertyName, JToken value)
        {
            this.Add(new JProperty(propertyName, value));
        }

        bool IDictionary<string, JToken>.ContainsKey(string key)
        {
            return this._properties.Dictionary != null && this._properties.Dictionary.ContainsKey(key);
        }

        public bool Remove(string propertyName)
        {
            JProperty jProperty = this.Property(propertyName);
            bool result;
            if (jProperty == null)
            {
                result = false;
            }
            else
            {
                jProperty.Remove();
                result = true;
            }
            return result;
        }

        public bool TryGetValue(string propertyName, out JToken value)
        {
            JProperty jProperty = this.Property(propertyName);
            bool result;
            if (jProperty == null)
            {
                value = null;
                result = false;
            }
            else
            {
                value = jProperty.Value;
                result = true;
            }
            return result;
        }

        void ICollection<KeyValuePair<string, JToken>>.Add(KeyValuePair<string, JToken> item)
        {
            this.Add(new JProperty(item.Key, item.Value));
        }

        void ICollection<KeyValuePair<string, JToken>>.Clear()
        {
            base.RemoveAll();
        }

        bool ICollection<KeyValuePair<string, JToken>>.Contains(KeyValuePair<string, JToken> item)
        {
            JProperty jProperty = this.Property(item.Key);
            return jProperty != null && jProperty.Value == item.Value;
        }

        void ICollection<KeyValuePair<string, JToken>>.CopyTo(KeyValuePair<string, JToken>[] array, int arrayIndex)
        {
            if (array == null)
            {
                throw new ArgumentNullException("array");
            }
            if (arrayIndex < 0)
            {
                throw new ArgumentOutOfRangeException("arrayIndex", "arrayIndex is less than 0.");
            }
            if (arrayIndex >= array.Length)
            {
                throw new ArgumentException("arrayIndex is equal to or greater than the length of array.");
            }
            if (this.Count > array.Length - arrayIndex)
            {
                throw new ArgumentException("The number of elements in the source JObject is greater than the available space from arrayIndex to the end of the destination array.");
            }
            int num = 0;
            using (IEnumerator<JToken> enumerator = this.ChildrenTokens.GetEnumerator())
            {
                while (enumerator.MoveNext())
                {
                    JProperty jProperty = (JProperty)enumerator.Current;
                    array[arrayIndex + num] = new KeyValuePair<string, JToken>(jProperty.Name, jProperty.Value);
                    num++;
                }
            }
        }

        bool ICollection<KeyValuePair<string, JToken>>.Remove(KeyValuePair<string, JToken> item)
        {
            bool result;
            if (!((ICollection<KeyValuePair<string, JToken>>)this).Contains(item))
            {
                result = false;
            }
            else
            {
                ((IDictionary<string, JToken>)this).Remove(item.Key);
                result = true;
            }
            return result;
        }

        internal override int GetDeepHashCode()
        {
            return base.ContentsHashCode();
        }

        public IEnumerator<KeyValuePair<string, JToken>> GetEnumerator()
        {
            using (IEnumerator<JToken> enumerator = this.ChildrenTokens.GetEnumerator())
            {
                while (enumerator.MoveNext())
                {
                    JProperty jProperty = (JProperty)enumerator.Current;
                    yield return new KeyValuePair<string, JToken>(jProperty.Name, jProperty.Value);
                }
            }
            yield break;
        }

        protected virtual void OnPropertyChanged(string propertyName)
        {
            if (this.PropertyChanged != null)
            {
                this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        protected virtual void OnPropertyChanging(string propertyName)
        {
            if (this.PropertyChanging != null)
            {
                this.PropertyChanging(this, new PropertyChangingEventArgs(propertyName));
            }
        }

        PropertyDescriptorCollection ICustomTypeDescriptor.GetProperties()
        {
            return ((ICustomTypeDescriptor)this).GetProperties(null);
        }

        private static Type GetTokenPropertyType(JToken token)
        {
            Type result;
            if (token is JValue)
            {
                JValue jValue = (JValue)token;
                result = ((jValue.Value != null) ? jValue.Value.GetType() : typeof(object));
            }
            else
            {
                result = token.GetType();
            }
            return result;
        }

        PropertyDescriptorCollection ICustomTypeDescriptor.GetProperties(Attribute[] attributes)
        {
            PropertyDescriptorCollection propertyDescriptorCollection = new PropertyDescriptorCollection(null);
            foreach (KeyValuePair<string, JToken> current in this)
            {
                propertyDescriptorCollection.Add(new JPropertyDescriptor(current.Key, JObject.GetTokenPropertyType(current.Value)));
            }
            return propertyDescriptorCollection;
        }

        AttributeCollection ICustomTypeDescriptor.GetAttributes()
        {
            return AttributeCollection.Empty;
        }

        string ICustomTypeDescriptor.GetClassName()
        {
            return null;
        }

        string ICustomTypeDescriptor.GetComponentName()
        {
            return null;
        }

        TypeConverter ICustomTypeDescriptor.GetConverter()
        {
            return new TypeConverter();
        }

        EventDescriptor ICustomTypeDescriptor.GetDefaultEvent()
        {
            return null;
        }

        PropertyDescriptor ICustomTypeDescriptor.GetDefaultProperty()
        {
            return null;
        }

        object ICustomTypeDescriptor.GetEditor(Type editorBaseType)
        {
            return null;
        }

        EventDescriptorCollection ICustomTypeDescriptor.GetEvents(Attribute[] attributes)
        {
            return EventDescriptorCollection.Empty;
        }

        EventDescriptorCollection ICustomTypeDescriptor.GetEvents()
        {
            return EventDescriptorCollection.Empty;
        }

        object ICustomTypeDescriptor.GetPropertyOwner(PropertyDescriptor pd)
        {
            return null;
        }
    }
}
