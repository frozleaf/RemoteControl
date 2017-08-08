namespace Newtonsoft.Json.Linq
{
    using Newtonsoft.Json;
    using Newtonsoft.Json.Utilities;
    using System;
    using System.Globalization;

    public class JValue : JToken, IEquatable<JValue>, IFormattable, IComparable, IComparable<JValue>
    {
        private object _value;
        private JTokenType _valueType;

        public JValue(JValue other) : this(other.Value, other.Type)
        {
        }

        public JValue(bool value) : this(value, JTokenType.Boolean)
        {
        }

        public JValue(DateTime value) : this(value, JTokenType.Date)
        {
        }

        public JValue(double value) : this(value, JTokenType.Float)
        {
        }

        public JValue(long value) : this(value, JTokenType.Integer)
        {
        }

        public JValue(object value) : this(value, GetValueType(null, value))
        {
        }

        public JValue(string value) : this(value, JTokenType.String)
        {
        }

        [CLSCompliant(false)]
        public JValue(ulong value) : this(value, JTokenType.Integer)
        {
        }

        internal JValue(object value, JTokenType type)
        {
            this._value = value;
            this._valueType = type;
        }

        internal override JToken CloneToken()
        {
            return new JValue(this);
        }

        private static int Compare(JTokenType valueType, object objA, object objB)
        {
            if ((objA == null) && (objB == null))
            {
                return 0;
            }
            if ((objA != null) && (objB == null))
            {
                return 1;
            }
            if ((objA == null) && (objB != null))
            {
                return -1;
            }
            switch (valueType)
            {
                case JTokenType.Comment:
                case JTokenType.String:
                case JTokenType.Raw:
                {
                    string str = Convert.ToString(objA, CultureInfo.InvariantCulture);
                    string strB = Convert.ToString(objB, CultureInfo.InvariantCulture);
                    return str.CompareTo(strB);
                }
                case JTokenType.Integer:
                    if ((((objA is ulong) || (objB is ulong)) || (objA is decimal)) || (objB is decimal))
                    {
                        return Convert.ToDecimal(objA, CultureInfo.InvariantCulture).CompareTo(Convert.ToDecimal(objB, CultureInfo.InvariantCulture));
                    }
                    if ((((objA is float) || (objB is float)) || (objA is double)) || (objB is double))
                    {
                        return CompareFloat(objA, objB);
                    }
                    return Convert.ToInt64(objA, CultureInfo.InvariantCulture).CompareTo(Convert.ToInt64(objB, CultureInfo.InvariantCulture));

                case JTokenType.Float:
                    return CompareFloat(objA, objB);

                case JTokenType.Boolean:
                {
                    bool flag = Convert.ToBoolean(objA, CultureInfo.InvariantCulture);
                    bool flag2 = Convert.ToBoolean(objB, CultureInfo.InvariantCulture);
                    return flag.CompareTo(flag2);
                }
                case JTokenType.Date:
                {
                    if (!(objA is DateTime))
                    {
                        if (!(objB is DateTimeOffset))
                        {
                            throw new ArgumentException("Object must be of type DateTimeOffset.");
                        }
                        DateTimeOffset offset = (DateTimeOffset) objA;
                        DateTimeOffset other = (DateTimeOffset) objB;
                        return offset.CompareTo(other);
                    }
                    DateTime time = Convert.ToDateTime(objA, CultureInfo.InvariantCulture);
                    DateTime time2 = Convert.ToDateTime(objB, CultureInfo.InvariantCulture);
                    return time.CompareTo(time2);
                }
                case JTokenType.Bytes:
                {
                    if (!(objB is byte[]))
                    {
                        throw new ArgumentException("Object must be of type byte[].");
                    }
                    byte[] buffer = objA as byte[];
                    byte[] buffer2 = objB as byte[];
                    if (buffer == null)
                    {
                        return -1;
                    }
                    if (buffer2 == null)
                    {
                        return 1;
                    }
                    return MiscellaneousUtils.ByteArrayCompare(buffer, buffer2);
                }
            }
            throw MiscellaneousUtils.CreateArgumentOutOfRangeException("valueType", valueType, "Unexpected value type: {0}".FormatWith(CultureInfo.InvariantCulture, new object[] { valueType }));
        }

        private static int CompareFloat(object objA, object objB)
        {
            double num = Convert.ToDouble(objA, CultureInfo.InvariantCulture);
            double num2 = Convert.ToDouble(objB, CultureInfo.InvariantCulture);
            if (MathUtils.ApproxEquals(num, num2))
            {
                return 0;
            }
            return num.CompareTo(num2);
        }

        public int CompareTo(JValue obj)
        {
            if (obj == null)
            {
                return 1;
            }
            return Compare(this._valueType, this._value, obj._value);
        }

        public static JValue CreateComment(string value)
        {
            return new JValue(value, JTokenType.Comment);
        }

        public static JValue CreateString(string value)
        {
            return new JValue(value, JTokenType.String);
        }

        internal override bool DeepEquals(JToken node)
        {
            JValue value2 = node as JValue;
            if (value2 == null)
            {
                return false;
            }
            return ValuesEquals(this, value2);
        }

        public bool Equals(JValue other)
        {
            if (other == null)
            {
                return false;
            }
            return ValuesEquals(this, other);
        }

        public override bool Equals(object obj)
        {
            if (obj == null)
            {
                return false;
            }
            JValue other = obj as JValue;
            if (other != null)
            {
                return this.Equals(other);
            }
            return base.Equals(obj);
        }

        internal override int GetDeepHashCode()
        {
            int num = (this._value != null) ? this._value.GetHashCode() : 0;
            return (this._valueType.GetHashCode() ^ num);
        }

        public override int GetHashCode()
        {
            if (this._value == null)
            {
                return 0;
            }
            return this._value.GetHashCode();
        }

        private static JTokenType GetStringValueType(JTokenType? current)
        {
            if (current.HasValue)
            {
                switch (current.Value)
                {
                    case JTokenType.Comment:
                    case JTokenType.String:
                    case JTokenType.Raw:
                        return current.Value;
                }
            }
            return JTokenType.String;
        }

        private static JTokenType GetValueType(JTokenType? current, object value)
        {
            if (value == null)
            {
                return JTokenType.Null;
            }
            if (value == DBNull.Value)
            {
                return JTokenType.Null;
            }
            if (value is string)
            {
                return GetStringValueType(current);
            }
            if (((((value is long) || (value is int)) || ((value is short) || (value is sbyte))) || (((value is ulong) || (value is uint)) || (value is ushort))) || (value is byte))
            {
                return JTokenType.Integer;
            }
            if (value is Enum)
            {
                return JTokenType.Integer;
            }
            if (((value is double) || (value is float)) || (value is decimal))
            {
                return JTokenType.Float;
            }
            if (value is DateTime)
            {
                return JTokenType.Date;
            }
            if (value is DateTimeOffset)
            {
                return JTokenType.Date;
            }
            if (value is byte[])
            {
                return JTokenType.Bytes;
            }
            if (!(value is bool))
            {
                throw new ArgumentException("Could not determine JSON object type for type {0}.".FormatWith(CultureInfo.InvariantCulture, new object[] { value.GetType() }));
            }
            return JTokenType.Boolean;
        }

        int IComparable.CompareTo(object obj)
        {
            if (obj == null)
            {
                return 1;
            }
            object objB = (obj is JValue) ? ((JValue) obj).Value : obj;
            return Compare(this._valueType, this._value, objB);
        }

        public override string ToString()
        {
            if (this._value == null)
            {
                return string.Empty;
            }
            return this._value.ToString();
        }

        public string ToString(IFormatProvider formatProvider)
        {
            return this.ToString(null, formatProvider);
        }

        public string ToString(string format)
        {
            return this.ToString(format, CultureInfo.CurrentCulture);
        }

        public string ToString(string format, IFormatProvider formatProvider)
        {
            if (this._value == null)
            {
                return string.Empty;
            }
            IFormattable formattable = this._value as IFormattable;
            if (formattable != null)
            {
                return formattable.ToString(format, formatProvider);
            }
            return this._value.ToString();
        }

        private static bool ValuesEquals(JValue v1, JValue v2)
        {
            return ((v1 == v2) || ((v1._valueType == v2._valueType) && (Compare(v1._valueType, v1._value, v2._value) == 0)));
        }

        public override void WriteTo(JsonWriter writer, params JsonConverter[] converters)
        {
            JsonConverter converter;
            switch (this._valueType)
            {
                case JTokenType.Null:
                    writer.WriteNull();
                    return;

                case JTokenType.Undefined:
                    writer.WriteUndefined();
                    return;

                case JTokenType.Raw:
                    writer.WriteRawValue((this._value != null) ? this._value.ToString() : null);
                    return;

                case JTokenType.Comment:
                    writer.WriteComment(this._value.ToString());
                    return;
            }
            if ((this._value != null) && ((converter = JsonSerializer.GetMatchingConverter(converters, this._value.GetType())) != null))
            {
                converter.WriteJson(writer, this._value, new JsonSerializer());
            }
            else
            {
                switch (this._valueType)
                {
                    case JTokenType.Integer:
                        writer.WriteValue(Convert.ToInt64(this._value, CultureInfo.InvariantCulture));
                        return;

                    case JTokenType.Float:
                        writer.WriteValue(Convert.ToDouble(this._value, CultureInfo.InvariantCulture));
                        return;

                    case JTokenType.String:
                        writer.WriteValue((this._value != null) ? this._value.ToString() : null);
                        return;

                    case JTokenType.Boolean:
                        writer.WriteValue(Convert.ToBoolean(this._value, CultureInfo.InvariantCulture));
                        return;

                    case JTokenType.Date:
                        if (!(this._value is DateTimeOffset))
                        {
                            writer.WriteValue(Convert.ToDateTime(this._value, CultureInfo.InvariantCulture));
                            return;
                        }
                        writer.WriteValue((DateTimeOffset) this._value);
                        return;

                    case JTokenType.Bytes:
                        writer.WriteValue((byte[]) this._value);
                        return;
                }
                throw MiscellaneousUtils.CreateArgumentOutOfRangeException("TokenType", this._valueType, "Unexpected token type.");
            }
        }

        public override bool HasValues
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
                return this._valueType;
            }
        }

        public object Value
        {
            get
            {
                return this._value;
            }
            set
            {
                System.Type type = (this._value != null) ? this._value.GetType() : null;
                System.Type type2 = (value != null) ? value.GetType() : null;
                if (type != type2)
                {
                    this._valueType = GetValueType(new JTokenType?(this._valueType), value);
                }
                this._value = value;
            }
        }
    }
}

