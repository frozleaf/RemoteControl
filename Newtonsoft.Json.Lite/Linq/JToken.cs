namespace Newtonsoft.Json.Linq
{
    using Newtonsoft.Json;
    using Newtonsoft.Json.Utilities;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using System.Runtime.CompilerServices;
    using System.Threading;

    public abstract class JToken : IJEnumerable<JToken>, IEnumerable<JToken>, IEnumerable, IJsonLineInfo, ICloneable
    {
        private static JTokenEqualityComparer _equalityComparer;
        private int? _lineNumber;
        private int? _linePosition;
        private JToken _next;
        private JContainer _parent;
        private JToken _previous;

        internal JToken()
        {
        }

        public void AddAfterSelf(object content)
        {
            if (this._parent == null)
            {
                throw new InvalidOperationException("The parent is missing.");
            }
            int num = this._parent.IndexOfItem(this);
            this._parent.AddInternal(num + 1, content);
        }

        public void AddBeforeSelf(object content)
        {
            if (this._parent == null)
            {
                throw new InvalidOperationException("The parent is missing.");
            }
            int index = this._parent.IndexOfItem(this);
            this._parent.AddInternal(index, content);
        }

        public IEnumerable<JToken> AfterSelf()
        {
            if (this.Parent != null)
            {
                for (JToken iteratorVariable0 = this.Next; iteratorVariable0 != null; iteratorVariable0 = iteratorVariable0.Next)
                {
                    yield return iteratorVariable0;
                }
            }
        }

        public IEnumerable<JToken> Ancestors()
        {
            for (JToken iteratorVariable0 = this.Parent; iteratorVariable0 != null; iteratorVariable0 = iteratorVariable0.Parent)
            {
                yield return iteratorVariable0;
            }
        }

        public IEnumerable<JToken> BeforeSelf()
        {
            for (JToken iteratorVariable0 = this.Parent.First; iteratorVariable0 != this; iteratorVariable0 = iteratorVariable0.Next)
            {
                yield return iteratorVariable0;
            }
        }

        public virtual JEnumerable<JToken> Children()
        {
            throw new InvalidOperationException("Cannot access child value on {0}.".FormatWith(CultureInfo.InvariantCulture, new object[] { base.GetType() }));
        }

        public JEnumerable<T> Children<T>() where T: JToken
        {
            return new JEnumerable<T>(this.Children().OfType<T>());
        }

        internal abstract JToken CloneToken();
        public JsonReader CreateReader()
        {
            return new JTokenReader(this);
        }

        public JToken DeepClone()
        {
            return this.CloneToken();
        }

        internal abstract bool DeepEquals(JToken node);
        public static bool DeepEquals(JToken t1, JToken t2)
        {
            return ((t1 == t2) || (((t1 != null) && (t2 != null)) && t1.DeepEquals(t2)));
        }

        private static JValue EnsureValue(JToken value)
        {
            if (value == null)
            {
                throw new ArgumentNullException("value");
            }
            if (value is JProperty)
            {
                value = ((JProperty) value).Value;
            }
            return (value as JValue);
        }

        public static JToken FromObject(object o)
        {
            return FromObjectInternal(o, new JsonSerializer());
        }

        public static JToken FromObject(object o, JsonSerializer jsonSerializer)
        {
            return FromObjectInternal(o, jsonSerializer);
        }

        internal static JToken FromObjectInternal(object o, JsonSerializer jsonSerializer)
        {
            ValidationUtils.ArgumentNotNull(o, "o");
            ValidationUtils.ArgumentNotNull(jsonSerializer, "jsonSerializer");
            using (JTokenWriter writer = new JTokenWriter())
            {
                jsonSerializer.Serialize(writer, o);
                return writer.Token;
            }
        }

        internal abstract int GetDeepHashCode();
        private static string GetType(JToken token)
        {
            ValidationUtils.ArgumentNotNull(token, "token");
            if (token is JProperty)
            {
                token = ((JProperty) token).Value;
            }
            return token.Type.ToString();
        }

        private static bool IsNullable(JToken o)
        {
            return ((o.Type == JTokenType.Undefined) || (o.Type == JTokenType.Null));
        }

        public static JToken Load(JsonReader reader)
        {
            return ReadFrom(reader);
        }

        bool IJsonLineInfo.HasLineInfo()
        {
            return (this._lineNumber.HasValue && this._linePosition.HasValue);
        }

        public static explicit operator bool(JToken value)
        {
            JValue o = EnsureValue(value);
            if (!((o != null) && ValidateBoolean(o, false)))
            {
                throw new ArgumentException("Can not convert {0} to Boolean.".FormatWith(CultureInfo.InvariantCulture, new object[] { GetType(value) }));
            }
            return (bool) o.Value;
        }

        public static explicit operator DateTime(JToken value)
        {
            JValue o = EnsureValue(value);
            if (!((o != null) && ValidateDate(o, false)))
            {
                throw new ArgumentException("Can not convert {0} to DateTime.".FormatWith(CultureInfo.InvariantCulture, new object[] { GetType(value) }));
            }
            return (DateTime) o.Value;
        }

        public static explicit operator DateTimeOffset(JToken value)
        {
            JValue o = EnsureValue(value);
            if (!((o != null) && ValidateDate(o, false)))
            {
                throw new ArgumentException("Can not convert {0} to DateTimeOffset.".FormatWith(CultureInfo.InvariantCulture, new object[] { GetType(value) }));
            }
            return (DateTimeOffset) o.Value;
        }

        public static explicit operator decimal(JToken value)
        {
            JValue o = EnsureValue(value);
            if (!((o != null) && ValidateFloat(o, false)))
            {
                throw new ArgumentException("Can not convert {0} to Decimal.".FormatWith(CultureInfo.InvariantCulture, new object[] { GetType(value) }));
            }
            return Convert.ToDecimal(o.Value, CultureInfo.InvariantCulture);
        }

        public static explicit operator double(JToken value)
        {
            JValue o = EnsureValue(value);
            if (!((o != null) && ValidateFloat(o, false)))
            {
                throw new ArgumentException("Can not convert {0} to Double.".FormatWith(CultureInfo.InvariantCulture, new object[] { GetType(value) }));
            }
            return (double) o.Value;
        }

        public static explicit operator short(JToken value)
        {
            JValue o = EnsureValue(value);
            if (!((o != null) && ValidateInteger(o, false)))
            {
                throw new ArgumentException("Can not convert {0} to Int16.".FormatWith(CultureInfo.InvariantCulture, new object[] { GetType(value) }));
            }
            return Convert.ToInt16(o.Value, CultureInfo.InvariantCulture);
        }

        public static explicit operator int(JToken value)
        {
            JValue o = EnsureValue(value);
            if (!((o != null) && ValidateInteger(o, false)))
            {
                throw new ArgumentException("Can not convert {0} to Int32.".FormatWith(CultureInfo.InvariantCulture, new object[] { GetType(value) }));
            }
            return Convert.ToInt32(o.Value, CultureInfo.InvariantCulture);
        }

        public static explicit operator long(JToken value)
        {
            JValue o = EnsureValue(value);
            if (!((o != null) && ValidateInteger(o, false)))
            {
                throw new ArgumentException("Can not convert {0} to Int64.".FormatWith(CultureInfo.InvariantCulture, new object[] { GetType(value) }));
            }
            return (long) o.Value;
        }

        public static explicit operator bool?(JToken value)
        {
            if (value == null)
            {
                return null;
            }
            JValue o = EnsureValue(value);
            if (!((o != null) && ValidateBoolean(o, true)))
            {
                throw new ArgumentException("Can not convert {0} to Boolean.".FormatWith(CultureInfo.InvariantCulture, new object[] { GetType(value) }));
            }
            return (bool?) o.Value;
        }

        public static explicit operator DateTime?(JToken value)
        {
            if (value == null)
            {
                return null;
            }
            JValue o = EnsureValue(value);
            if (!((o != null) && ValidateDate(o, true)))
            {
                throw new ArgumentException("Can not convert {0} to DateTime.".FormatWith(CultureInfo.InvariantCulture, new object[] { GetType(value) }));
            }
            return (DateTime?) o.Value;
        }

        public static explicit operator DateTimeOffset?(JToken value)
        {
            if (value == null)
            {
                return null;
            }
            JValue o = EnsureValue(value);
            if (!((o != null) && ValidateDate(o, true)))
            {
                throw new ArgumentException("Can not convert {0} to DateTimeOffset.".FormatWith(CultureInfo.InvariantCulture, new object[] { GetType(value) }));
            }
            return (DateTimeOffset?) o.Value;
        }

        public static explicit operator decimal?(JToken value)
        {
            if (value == null)
            {
                return null;
            }
            JValue o = EnsureValue(value);
            if (!((o != null) && ValidateFloat(o, true)))
            {
                throw new ArgumentException("Can not convert {0} to Decimal.".FormatWith(CultureInfo.InvariantCulture, new object[] { GetType(value) }));
            }
            return ((o.Value != null) ? new decimal?(Convert.ToDecimal(o.Value, CultureInfo.InvariantCulture)) : null);
        }

        public static explicit operator double?(JToken value)
        {
            if (value == null)
            {
                return null;
            }
            JValue o = EnsureValue(value);
            if (!((o != null) && ValidateFloat(o, true)))
            {
                throw new ArgumentException("Can not convert {0} to Double.".FormatWith(CultureInfo.InvariantCulture, new object[] { GetType(value) }));
            }
            return (double?) o.Value;
        }

        public static explicit operator short?(JToken value)
        {
            if (value == null)
            {
                return null;
            }
            JValue o = EnsureValue(value);
            if (!((o != null) && ValidateInteger(o, true)))
            {
                throw new ArgumentException("Can not convert {0} to Int16.".FormatWith(CultureInfo.InvariantCulture, new object[] { GetType(value) }));
            }
            return ((o.Value != null) ? new short?(Convert.ToInt16(o.Value, CultureInfo.InvariantCulture)) : null);
        }

        public static explicit operator int?(JToken value)
        {
            if (value == null)
            {
                return null;
            }
            JValue o = EnsureValue(value);
            if (!((o != null) && ValidateInteger(o, true)))
            {
                throw new ArgumentException("Can not convert {0} to Int32.".FormatWith(CultureInfo.InvariantCulture, new object[] { GetType(value) }));
            }
            return ((o.Value != null) ? new int?(Convert.ToInt32(o.Value, CultureInfo.InvariantCulture)) : null);
        }

        public static explicit operator long?(JToken value)
        {
            if (value == null)
            {
                return null;
            }
            JValue o = EnsureValue(value);
            if (!((o != null) && ValidateInteger(o, true)))
            {
                throw new ArgumentException("Can not convert {0} to Int64.".FormatWith(CultureInfo.InvariantCulture, new object[] { GetType(value) }));
            }
            return (long?) o.Value;
        }

        public static explicit operator float?(JToken value)
        {
            if (value == null)
            {
                return null;
            }
            JValue o = EnsureValue(value);
            if (!((o != null) && ValidateFloat(o, true)))
            {
                throw new ArgumentException("Can not convert {0} to Single.".FormatWith(CultureInfo.InvariantCulture, new object[] { GetType(value) }));
            }
            return ((o.Value != null) ? new float?(Convert.ToSingle(o.Value, CultureInfo.InvariantCulture)) : null);
        }

        [CLSCompliant(false)]
        public static explicit operator ushort?(JToken value)
        {
            if (value == null)
            {
                return null;
            }
            JValue o = EnsureValue(value);
            if (!((o != null) && ValidateInteger(o, true)))
            {
                throw new ArgumentException("Can not convert {0} to UInt16.".FormatWith(CultureInfo.InvariantCulture, new object[] { GetType(value) }));
            }
            return ((o.Value != null) ? new ushort?((ushort) Convert.ToInt16(o.Value, CultureInfo.InvariantCulture)) : null);
        }

        [CLSCompliant(false)]
        public static explicit operator uint?(JToken value)
        {
            if (value == null)
            {
                return null;
            }
            JValue o = EnsureValue(value);
            if (!((o != null) && ValidateInteger(o, true)))
            {
                throw new ArgumentException("Can not convert {0} to UInt32.".FormatWith(CultureInfo.InvariantCulture, new object[] { GetType(value) }));
            }
            return (uint?) o.Value;
        }

        [CLSCompliant(false)]
        public static explicit operator ulong?(JToken value)
        {
            if (value == null)
            {
                return null;
            }
            JValue o = EnsureValue(value);
            if (!((o != null) && ValidateInteger(o, true)))
            {
                throw new ArgumentException("Can not convert {0} to UInt64.".FormatWith(CultureInfo.InvariantCulture, new object[] { GetType(value) }));
            }
            return (ulong?) o.Value;
        }

        public static explicit operator float(JToken value)
        {
            JValue o = EnsureValue(value);
            if (!((o != null) && ValidateFloat(o, false)))
            {
                throw new ArgumentException("Can not convert {0} to Single.".FormatWith(CultureInfo.InvariantCulture, new object[] { GetType(value) }));
            }
            return Convert.ToSingle(o.Value, CultureInfo.InvariantCulture);
        }

        public static explicit operator string(JToken value)
        {
            if (value == null)
            {
                return null;
            }
            JValue o = EnsureValue(value);
            if (!((o != null) && ValidateString(o)))
            {
                throw new ArgumentException("Can not convert {0} to String.".FormatWith(CultureInfo.InvariantCulture, new object[] { GetType(value) }));
            }
            return (string) o.Value;
        }

        [CLSCompliant(false)]
        public static explicit operator ushort(JToken value)
        {
            JValue o = EnsureValue(value);
            if (!((o != null) && ValidateInteger(o, false)))
            {
                throw new ArgumentException("Can not convert {0} to UInt16.".FormatWith(CultureInfo.InvariantCulture, new object[] { GetType(value) }));
            }
            return Convert.ToUInt16(o.Value, CultureInfo.InvariantCulture);
        }

        [CLSCompliant(false)]
        public static explicit operator uint(JToken value)
        {
            JValue o = EnsureValue(value);
            if (!((o != null) && ValidateInteger(o, false)))
            {
                throw new ArgumentException("Can not convert {0} to UInt32.".FormatWith(CultureInfo.InvariantCulture, new object[] { GetType(value) }));
            }
            return Convert.ToUInt32(o.Value, CultureInfo.InvariantCulture);
        }

        [CLSCompliant(false)]
        public static explicit operator ulong(JToken value)
        {
            JValue o = EnsureValue(value);
            if (!((o != null) && ValidateInteger(o, false)))
            {
                throw new ArgumentException("Can not convert {0} to UInt64.".FormatWith(CultureInfo.InvariantCulture, new object[] { GetType(value) }));
            }
            return Convert.ToUInt64(o.Value, CultureInfo.InvariantCulture);
        }

        public static explicit operator byte[](JToken value)
        {
            JValue o = EnsureValue(value);
            if (!((o != null) && ValidateBytes(o)))
            {
                throw new ArgumentException("Can not convert {0} to byte array.".FormatWith(CultureInfo.InvariantCulture, new object[] { GetType(value) }));
            }
            return (byte[]) o.Value;
        }

        public static implicit operator JToken(bool value)
        {
            return new JValue(value);
        }

        public static implicit operator JToken(DateTime value)
        {
            return new JValue(value);
        }

        public static implicit operator JToken(DateTimeOffset value)
        {
            return new JValue(value);
        }

        public static implicit operator JToken(decimal value)
        {
            return new JValue(value);
        }

        public static implicit operator JToken(double value)
        {
            return new JValue(value);
        }

        [CLSCompliant(false)]
        public static implicit operator JToken(short value)
        {
            return new JValue((long) value);
        }

        public static implicit operator JToken(int value)
        {
            return new JValue((long) value);
        }

        public static implicit operator JToken(long value)
        {
            return new JValue(value);
        }

        public static implicit operator JToken(bool? value)
        {
            return new JValue(value);
        }

        public static implicit operator JToken(DateTime? value)
        {
            return new JValue(value);
        }

        public static implicit operator JToken(DateTimeOffset? value)
        {
            return new JValue(value);
        }

        public static implicit operator JToken(decimal? value)
        {
            return new JValue(value);
        }

        public static implicit operator JToken(double? value)
        {
            return new JValue(value);
        }

        [CLSCompliant(false)]
        public static implicit operator JToken(short? value)
        {
            return new JValue(value);
        }

        public static implicit operator JToken(int? value)
        {
            return new JValue(value);
        }

        public static implicit operator JToken(long? value)
        {
            return new JValue(value);
        }

        public static implicit operator JToken(float? value)
        {
            return new JValue(value);
        }

        [CLSCompliant(false)]
        public static implicit operator JToken(ushort? value)
        {
            return new JValue(value);
        }

        [CLSCompliant(false)]
        public static implicit operator JToken(uint? value)
        {
            return new JValue(value);
        }

        [CLSCompliant(false)]
        public static implicit operator JToken(ulong? value)
        {
            return new JValue(value);
        }

        public static implicit operator JToken(float value)
        {
            return new JValue((double) value);
        }

        public static implicit operator JToken(string value)
        {
            return new JValue(value);
        }

        [CLSCompliant(false)]
        public static implicit operator JToken(ushort value)
        {
            return new JValue((long) value);
        }

        [CLSCompliant(false)]
        public static implicit operator JToken(uint value)
        {
            return new JValue((long) value);
        }

        [CLSCompliant(false)]
        public static implicit operator JToken(ulong value)
        {
            return new JValue(value);
        }

        public static implicit operator JToken(byte[] value)
        {
            return new JValue(value);
        }

        public static JToken Parse(string json)
        {
            JsonReader reader = new JsonTextReader(new StringReader(json));
            return Load(reader);
        }

        public static JToken ReadFrom(JsonReader reader)
        {
            ValidationUtils.ArgumentNotNull(reader, "reader");
            if ((reader.TokenType == JsonToken.None) && !reader.Read())
            {
                throw new Exception("Error reading JToken from JsonReader.");
            }
            if (reader.TokenType == JsonToken.StartObject)
            {
                return JObject.Load(reader);
            }
            if (reader.TokenType == JsonToken.StartArray)
            {
                return JArray.Load(reader);
            }
            if (reader.TokenType == JsonToken.PropertyName)
            {
                return JProperty.Load(reader);
            }
            if (reader.TokenType == JsonToken.StartConstructor)
            {
                return JConstructor.Load(reader);
            }
            if (JsonReader.IsStartToken(reader.TokenType))
            {
                throw new Exception("Error reading JToken from JsonReader. Unexpected token: {0}".FormatWith(CultureInfo.InvariantCulture, new object[] { reader.TokenType }));
            }
            return new JValue(reader.Value);
        }

        public void Remove()
        {
            if (this._parent == null)
            {
                throw new InvalidOperationException("The parent is missing.");
            }
            this._parent.RemoveItem(this);
        }

        public void Replace(JToken value)
        {
            if (this._parent == null)
            {
                throw new InvalidOperationException("The parent is missing.");
            }
            this._parent.ReplaceItem(this, value);
        }

        public JToken SelectToken(string path)
        {
            return this.SelectToken(path, false);
        }

        public JToken SelectToken(string path, bool errorWhenNoMatch)
        {
            JPath path2 = new JPath(path);
            return path2.Evaluate(this, errorWhenNoMatch);
        }

        internal void SetLineInfo(IJsonLineInfo lineInfo)
        {
            if ((lineInfo != null) && lineInfo.HasLineInfo())
            {
                this.SetLineInfo(lineInfo.LineNumber, lineInfo.LinePosition);
            }
        }

        internal void SetLineInfo(int lineNumber, int linePosition)
        {
            this._lineNumber = new int?(lineNumber);
            this._linePosition = new int?(linePosition);
        }

        IEnumerator<JToken> IEnumerable<JToken>.GetEnumerator()
        {
            return this.Children().GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable<JToken>) this).GetEnumerator();
        }

        object ICloneable.Clone()
        {
            return this.DeepClone();
        }

        public override string ToString()
        {
            return this.ToString(Formatting.Indented, new JsonConverter[0]);
        }

        public string ToString(Formatting formatting, params JsonConverter[] converters)
        {
            using (StringWriter writer = new StringWriter(CultureInfo.InvariantCulture))
            {
                JsonTextWriter writer2 = new JsonTextWriter(writer) {
                    Formatting = formatting
                };
                this.WriteTo(writer2, converters);
                return writer.ToString();
            }
        }

        private static bool ValidateBoolean(JToken o, bool nullable)
        {
            return ((o.Type == JTokenType.Boolean) || (nullable && IsNullable(o)));
        }

        private static bool ValidateBytes(JToken o)
        {
            return ((o.Type == JTokenType.Bytes) || IsNullable(o));
        }

        private static bool ValidateDate(JToken o, bool nullable)
        {
            return ((o.Type == JTokenType.Date) || (nullable && IsNullable(o)));
        }

        private static bool ValidateFloat(JToken o, bool nullable)
        {
            return (((o.Type == JTokenType.Float) || (o.Type == JTokenType.Integer)) || (nullable && IsNullable(o)));
        }

        private static bool ValidateInteger(JToken o, bool nullable)
        {
            return ((o.Type == JTokenType.Integer) || (nullable && IsNullable(o)));
        }

        private static bool ValidateString(JToken o)
        {
            return ((((o.Type == JTokenType.String) || (o.Type == JTokenType.Comment)) || (o.Type == JTokenType.Raw)) || IsNullable(o));
        }

        public virtual T Value<T>(object key)
        {
            JToken token = this[key];
            return token.Convert<JToken, T>();
        }

        public virtual IEnumerable<T> Values<T>()
        {
            throw new InvalidOperationException("Cannot access child value on {0}.".FormatWith(CultureInfo.InvariantCulture, new object[] { base.GetType() }));
        }

        public abstract void WriteTo(JsonWriter writer, params JsonConverter[] converters);

        public static JTokenEqualityComparer EqualityComparer
        {
            get
            {
                if (_equalityComparer == null)
                {
                    _equalityComparer = new JTokenEqualityComparer();
                }
                return _equalityComparer;
            }
        }

        public virtual JToken First
        {
            get
            {
                throw new InvalidOperationException("Cannot access child value on {0}.".FormatWith(CultureInfo.InvariantCulture, new object[] { base.GetType() }));
            }
        }

        public abstract bool HasValues { get; }

        public virtual JToken this[object key]
        {
            get
            {
                throw new InvalidOperationException("Cannot access child value on {0}.".FormatWith(CultureInfo.InvariantCulture, new object[] { base.GetType() }));
            }
            set
            {
                throw new InvalidOperationException("Cannot set child value on {0}.".FormatWith(CultureInfo.InvariantCulture, new object[] { base.GetType() }));
            }
        }

        public virtual JToken Last
        {
            get
            {
                throw new InvalidOperationException("Cannot access child value on {0}.".FormatWith(CultureInfo.InvariantCulture, new object[] { base.GetType() }));
            }
        }

        int IJsonLineInfo.LineNumber
        {
            get
            {
                int? nullable = this._lineNumber;
                return (nullable.HasValue ? nullable.GetValueOrDefault() : 0);
            }
        }

        int IJsonLineInfo.LinePosition
        {
            get
            {
                int? nullable = this._linePosition;
                return (nullable.HasValue ? nullable.GetValueOrDefault() : 0);
            }
        }

        IJEnumerable<JToken> IJEnumerable<JToken>.this[object key]
        {
            get
            {
                return this[key];
            }
        }

        public JToken Next
        {
            get
            {
                return this._next;
            }
            internal set
            {
                this._next = value;
            }
        }

        public JContainer Parent
        {
            [DebuggerStepThrough]
            get
            {
                return this._parent;
            }
            internal set
            {
                this._parent = value;
            }
        }

        public JToken Previous
        {
            get
            {
                return this._previous;
            }
            internal set
            {
                this._previous = value;
            }
        }

        public JToken Root
        {
            get
            {
                JContainer parent = this.Parent;
                if (parent == null)
                {
                    return this;
                }
                while (parent.Parent != null)
                {
                    parent = parent.Parent;
                }
                return parent;
            }
        }

        public abstract JTokenType Type { get; }



    }
}

