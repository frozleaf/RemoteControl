namespace Newtonsoft.Json.Linq
{
    using Newtonsoft.Json;
    using Newtonsoft.Json.Utilities;
    using System;

    public class JTokenWriter : JsonWriter
    {
        private JContainer _parent;
        private JContainer _token;
        private JValue _value;

        public JTokenWriter()
        {
        }

        public JTokenWriter(JContainer container)
        {
            ValidationUtils.ArgumentNotNull(container, "container");
            this._token = container;
            this._parent = container;
        }

        private void AddParent(JContainer container)
        {
            if (this._parent == null)
            {
                this._token = container;
            }
            else
            {
                this._parent.Add(container);
            }
            this._parent = container;
        }

        internal void AddValue(JValue value, JsonToken token)
        {
            if (this._parent != null)
            {
                this._parent.Add(value);
                if (this._parent.Type == JTokenType.Property)
                {
                    this._parent = this._parent.Parent;
                }
            }
            else
            {
                this._value = value;
            }
        }

        private void AddValue(object value, JsonToken token)
        {
            this.AddValue(new JValue(value), token);
        }

        public override void Close()
        {
            base.Close();
        }

        public override void Flush()
        {
        }

        private void RemoveParent()
        {
            this._parent = this._parent.Parent;
            if ((this._parent != null) && (this._parent.Type == JTokenType.Property))
            {
                this._parent = this._parent.Parent;
            }
        }

        public override void WriteComment(string text)
        {
            base.WriteComment(text);
            this.AddValue(JValue.CreateComment(text), JsonToken.Comment);
        }

        protected override void WriteEnd(JsonToken token)
        {
            this.RemoveParent();
        }

        public override void WriteNull()
        {
            base.WriteNull();
            this.AddValue((JValue) null, JsonToken.Null);
        }

        public override void WritePropertyName(string name)
        {
            base.WritePropertyName(name);
            this.AddParent(new JProperty(name));
        }

        public override void WriteRaw(string json)
        {
            base.WriteRaw(json);
            this.AddValue((JValue) new JRaw(json), JsonToken.Raw);
        }

        public override void WriteStartArray()
        {
            base.WriteStartArray();
            this.AddParent(new JArray());
        }

        public override void WriteStartConstructor(string name)
        {
            base.WriteStartConstructor(name);
            this.AddParent(new JConstructor(name));
        }

        public override void WriteStartObject()
        {
            base.WriteStartObject();
            this.AddParent(new JObject());
        }

        public override void WriteUndefined()
        {
            base.WriteUndefined();
            this.AddValue((JValue) null, JsonToken.Undefined);
        }

        public override void WriteValue(bool value)
        {
            base.WriteValue(value);
            this.AddValue(value, JsonToken.Boolean);
        }

        public override void WriteValue(byte value)
        {
            base.WriteValue(value);
            this.AddValue(value, JsonToken.Integer);
        }

        public override void WriteValue(char value)
        {
            base.WriteValue(value);
            this.AddValue(value.ToString(), JsonToken.String);
        }

        public override void WriteValue(DateTime value)
        {
            base.WriteValue(value);
            this.AddValue(value, JsonToken.Date);
        }

        public override void WriteValue(DateTimeOffset value)
        {
            base.WriteValue(value);
            this.AddValue(value, JsonToken.Date);
        }

        public override void WriteValue(decimal value)
        {
            base.WriteValue(value);
            this.AddValue(value, JsonToken.Float);
        }

        public override void WriteValue(double value)
        {
            base.WriteValue(value);
            this.AddValue(value, JsonToken.Float);
        }

        public override void WriteValue(short value)
        {
            base.WriteValue(value);
            this.AddValue(value, JsonToken.Integer);
        }

        public override void WriteValue(int value)
        {
            base.WriteValue(value);
            this.AddValue(value, JsonToken.Integer);
        }

        public override void WriteValue(long value)
        {
            base.WriteValue(value);
            this.AddValue(value, JsonToken.Integer);
        }

        [CLSCompliant(false)]
        public override void WriteValue(sbyte value)
        {
            base.WriteValue(value);
            this.AddValue(value, JsonToken.Integer);
        }

        public override void WriteValue(float value)
        {
            base.WriteValue(value);
            this.AddValue(value, JsonToken.Float);
        }

        public override void WriteValue(string value)
        {
            base.WriteValue(value);
            this.AddValue(value ?? string.Empty, JsonToken.String);
        }

        [CLSCompliant(false)]
        public override void WriteValue(ushort value)
        {
            base.WriteValue(value);
            this.AddValue(value, JsonToken.Integer);
        }

        [CLSCompliant(false)]
        public override void WriteValue(uint value)
        {
            base.WriteValue(value);
            this.AddValue(value, JsonToken.Integer);
        }

        [CLSCompliant(false)]
        public override void WriteValue(ulong value)
        {
            base.WriteValue(value);
            this.AddValue(value, JsonToken.Integer);
        }

        public override void WriteValue(byte[] value)
        {
            base.WriteValue(value);
            this.AddValue(value, JsonToken.Bytes);
        }

        public JToken Token
        {
            get
            {
                if (this._token != null)
                {
                    return this._token;
                }
                return this._value;
            }
        }
    }
}

