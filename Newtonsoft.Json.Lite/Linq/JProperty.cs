using Newtonsoft.Json.Utilities;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;

namespace Newtonsoft.Json.Linq
{
    public class JProperty : JContainer
    {
        private readonly List<JToken> _content = new List<JToken>();

        private readonly string _name;

        protected override IList<JToken> ChildrenTokens
        {
            get
            {
                return this._content;
            }
        }

        public string Name
        {
            [DebuggerStepThrough]
            get
            {
                return this._name;
            }
        }

        public new JToken Value
        {
            [DebuggerStepThrough]
            get
            {
                return (this.ChildrenTokens.Count > 0) ? this.ChildrenTokens[0] : null;
            }
            set
            {
                base.CheckReentrancy();
                JToken item = value ?? new JValue((JValue)null);
                if (this.ChildrenTokens.Count == 0)
                {
                    this.InsertItem(0, item);
                }
                else
                {
                    this.SetItem(0, item);
                }
            }
        }

        public override JTokenType Type
        {
            [DebuggerStepThrough]
            get
            {
                return JTokenType.Property;
            }
        }

        public JProperty(JProperty other)
            : base(other)
        {
            this._name = other.Name;
        }

        internal override JToken GetItem(int index)
        {
            if (index != 0)
            {
                throw new ArgumentOutOfRangeException();
            }
            return this.Value;
        }

        internal override void SetItem(int index, JToken item)
        {
            if (index != 0)
            {
                throw new ArgumentOutOfRangeException();
            }
            if (!JContainer.IsTokenUnchanged(this.Value, item))
            {
                if (base.Parent != null)
                {
                    ((JObject)base.Parent).InternalPropertyChanging(this);
                }
                base.SetItem(0, item);
                if (base.Parent != null)
                {
                    ((JObject)base.Parent).InternalPropertyChanged(this);
                }
            }
        }

        internal override bool RemoveItem(JToken item)
        {
            throw new Exception("Cannot add or remove items from {0}.".FormatWith(CultureInfo.InvariantCulture, new object[]
			{
				typeof(JProperty)
			}));
        }

        internal override void RemoveItemAt(int index)
        {
            throw new Exception("Cannot add or remove items from {0}.".FormatWith(CultureInfo.InvariantCulture, new object[]
			{
				typeof(JProperty)
			}));
        }

        internal override void InsertItem(int index, JToken item)
        {
            if (this.Value != null)
            {
                throw new Exception("{0} cannot have multiple values.".FormatWith(CultureInfo.InvariantCulture, new object[]
				{
					typeof(JProperty)
				}));
            }
            base.InsertItem(0, item);
        }

        internal override bool ContainsItem(JToken item)
        {
            return this.Value == item;
        }

        internal override void ClearItems()
        {
            throw new Exception("Cannot add or remove items from {0}.".FormatWith(CultureInfo.InvariantCulture, new object[]
			{
				typeof(JProperty)
			}));
        }

        internal override bool DeepEquals(JToken node)
        {
            JProperty jProperty = node as JProperty;
            return jProperty != null && this._name == jProperty.Name && base.ContentsEqual(jProperty);
        }

        internal override JToken CloneToken()
        {
            return new JProperty(this);
        }

        internal JProperty(string name)
        {
            ValidationUtils.ArgumentNotNull(name, "name");
            this._name = name;
        }

        public JProperty(string name, params object[] content)
            : this(name, (object)content)
        {
        }

        public JProperty(string name, object content)
        {
            ValidationUtils.ArgumentNotNull(name, "name");
            this._name = name;
            this.Value = (base.IsMultiContent(content) ? new JArray(content) : base.CreateFromContent(content));
        }

        public override void WriteTo(JsonWriter writer, params JsonConverter[] converters)
        {
            writer.WritePropertyName(this._name);
            this.Value.WriteTo(writer, converters);
        }

        internal override int GetDeepHashCode()
        {
            return this._name.GetHashCode() ^ ((this.Value != null) ? this.Value.GetDeepHashCode() : 0);
        }

        public new static JProperty Load(JsonReader reader)
        {
            if (reader.TokenType == JsonToken.None)
            {
                if (!reader.Read())
                {
                    throw new Exception("Error reading JProperty from JsonReader.");
                }
            }
            if (reader.TokenType != JsonToken.PropertyName)
            {
                throw new Exception("Error reading JProperty from JsonReader. Current JsonReader item is not a property: {0}".FormatWith(CultureInfo.InvariantCulture, new object[]
				{
					reader.TokenType
				}));
            }
            JProperty jProperty = new JProperty((string)reader.Value);
            jProperty.SetLineInfo(reader as IJsonLineInfo);
            jProperty.ReadTokenFrom(reader);
            return jProperty;
        }
    }
}
