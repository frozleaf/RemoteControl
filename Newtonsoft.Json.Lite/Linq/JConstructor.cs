namespace Newtonsoft.Json.Linq
{
    using Newtonsoft.Json;
    using Newtonsoft.Json.Utilities;
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Reflection;

    public class JConstructor : JContainer
    {
        private string _name;
        private IList<JToken> _values;

        public JConstructor()
        {
            this._values = new List<JToken>();
        }

        public JConstructor(JConstructor other) : base(other)
        {
            this._values = new List<JToken>();
            this._name = other.Name;
        }

        public JConstructor(string name)
        {
            this._values = new List<JToken>();
            ValidationUtils.ArgumentNotNullOrEmpty(name, "name");
            this._name = name;
        }

        public JConstructor(string name, params object[] content) : this(name, (object)content)
        {
        }

        public JConstructor(string name, object content) : this(name)
        {
            this.Add(content);
        }

        internal override JToken CloneToken()
        {
            return new JConstructor(this);
        }

        internal override bool DeepEquals(JToken node)
        {
            JConstructor container = node as JConstructor;
            return (((container != null) && (this._name == container.Name)) && base.ContentsEqual(container));
        }

        internal override int GetDeepHashCode()
        {
            return (this._name.GetHashCode() ^ base.ContentsHashCode());
        }

        public static JConstructor Load(JsonReader reader)
        {
            if ((reader.TokenType == JsonToken.None) && !reader.Read())
            {
                throw new Exception("Error reading JConstructor from JsonReader.");
            }
            if (reader.TokenType != JsonToken.StartConstructor)
            {
                throw new Exception("Error reading JConstructor from JsonReader. Current JsonReader item is not a constructor: {0}".FormatWith(CultureInfo.InvariantCulture, new object[] { reader.TokenType }));
            }
            JConstructor constructor = new JConstructor((string) reader.Value);
            constructor.SetLineInfo(reader as IJsonLineInfo);
            constructor.ReadTokenFrom(reader);
            return constructor;
        }

        public override void WriteTo(JsonWriter writer, params JsonConverter[] converters)
        {
            writer.WriteStartConstructor(this._name);
            foreach (JToken token in this.Children())
            {
                token.WriteTo(writer, converters);
            }
            writer.WriteEndConstructor();
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
                    throw new ArgumentException("Accessed JConstructor values with invalid key value: {0}. Argument position index expected.".FormatWith(CultureInfo.InvariantCulture, new object[] { MiscellaneousUtils.ToString(key) }));
                }
                return this.GetItem((int) key);
            }
            set
            {
                ValidationUtils.ArgumentNotNull(key, "o");
                if (!(key is int))
                {
                    throw new ArgumentException("Set JConstructor values with invalid key value: {0}. Argument position index expected.".FormatWith(CultureInfo.InvariantCulture, new object[] { MiscellaneousUtils.ToString(key) }));
                }
                this.SetItem((int) key, value);
            }
        }

        public string Name
        {
            get
            {
                return this._name;
            }
            set
            {
                this._name = value;
            }
        }

        public override JTokenType Type
        {
            get
            {
                return JTokenType.Constructor;
            }
        }
    }
}

