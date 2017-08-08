namespace Newtonsoft.Json.Linq
{
    using Newtonsoft.Json;
    using Newtonsoft.Json.Utilities;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Globalization;
    using System.IO;
    using System.Reflection;

    public class JArray : JContainer, IList<JToken>, ICollection<JToken>, IEnumerable<JToken>, IEnumerable
    {
        private IList<JToken> _values;

        public JArray()
        {
            this._values = new List<JToken>();
        }

        public JArray(JArray other) : base(other)
        {
            this._values = new List<JToken>();
        }

        public JArray(params object[] content) : this((object)content)
        {
        }

        public JArray(object content)
        {
            this._values = new List<JToken>();
            this.Add(content);
        }

        public void Add(JToken item)
        {
            this.Add(item);
        }

        public void Clear()
        {
            this.ClearItems();
        }

        internal override JToken CloneToken()
        {
            return new JArray(this);
        }

        public bool Contains(JToken item)
        {
            return this.ContainsItem(item);
        }

        internal override bool DeepEquals(JToken node)
        {
            JArray container = node as JArray;
            return ((container != null) && base.ContentsEqual(container));
        }

        public static JArray FromObject(object o)
        {
            return FromObject(o, new JsonSerializer());
        }

        public static JArray FromObject(object o, JsonSerializer jsonSerializer)
        {
            JToken token = JToken.FromObjectInternal(o, jsonSerializer);
            if (token.Type != JTokenType.Array)
            {
                throw new ArgumentException("Object serialized to {0}. JArray instance expected.".FormatWith(CultureInfo.InvariantCulture, new object[] { token.Type }));
            }
            return (JArray) token;
        }

        internal override int GetDeepHashCode()
        {
            return base.ContentsHashCode();
        }

        public int IndexOf(JToken item)
        {
            return base.IndexOfItem(item);
        }

        public void Insert(int index, JToken item)
        {
            this.InsertItem(index, item);
        }

        public static JArray Load(JsonReader reader)
        {
            if ((reader.TokenType == JsonToken.None) && !reader.Read())
            {
                throw new Exception("Error reading JArray from JsonReader.");
            }
            if (reader.TokenType != JsonToken.StartArray)
            {
                throw new Exception("Error reading JArray from JsonReader. Current JsonReader item is not an array: {0}".FormatWith(CultureInfo.InvariantCulture, new object[] { reader.TokenType }));
            }
            JArray array = new JArray();
            array.SetLineInfo(reader as IJsonLineInfo);
            array.ReadTokenFrom(reader);
            return array;
        }

        public static JArray Parse(string json)
        {
            JsonReader reader = new JsonTextReader(new StringReader(json));
            return Load(reader);
        }

        public bool Remove(JToken item)
        {
            return this.RemoveItem(item);
        }

        public void RemoveAt(int index)
        {
            this.RemoveItemAt(index);
        }

        void ICollection<JToken>.CopyTo(JToken[] array, int arrayIndex)
        {
            this.CopyItemsTo(array, arrayIndex);
        }

        public override void WriteTo(JsonWriter writer, params JsonConverter[] converters)
        {
            writer.WriteStartArray();
            foreach (JToken token in this.ChildrenTokens)
            {
                token.WriteTo(writer, converters);
            }
            writer.WriteEndArray();
        }

        protected override IList<JToken> ChildrenTokens
        {
            get
            {
                return this._values;
            }
        }

        public override JToken this[object key]
        {
            get
            {
                ValidationUtils.ArgumentNotNull(key, "o");
                if (!(key is int))
                {
                    throw new ArgumentException("Accessed JArray values with invalid key value: {0}. Array position index expected.".FormatWith(CultureInfo.InvariantCulture, new object[] { MiscellaneousUtils.ToString(key) }));
                }
                return this.GetItem((int) key);
            }
            set
            {
                ValidationUtils.ArgumentNotNull(key, "o");
                if (!(key is int))
                {
                    throw new ArgumentException("Set JArray values with invalid key value: {0}. Array position index expected.".FormatWith(CultureInfo.InvariantCulture, new object[] { MiscellaneousUtils.ToString(key) }));
                }
                this.SetItem((int) key, value);
            }
        }

        public JToken this[int index]
        {
            get
            {
                return this.GetItem(index);
            }
            set
            {
                this.SetItem(index, value);
            }
        }

        bool ICollection<JToken>.IsReadOnly
        {
            get
            {
                return false;
            }
        }

        public override JTokenType Type
        {
            get
            {
                return JTokenType.Array;
            }
        }
    }
}

