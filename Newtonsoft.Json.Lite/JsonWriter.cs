namespace Newtonsoft.Json
{
    using Newtonsoft.Json.Linq;
    using Newtonsoft.Json.Utilities;
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Runtime.CompilerServices;

    public abstract class JsonWriter : IDisposable
    {
        private State _currentState;
        private Newtonsoft.Json.Formatting _formatting;
        private readonly List<JTokenType> _stack = new List<JTokenType>(8);
        private int _top;
        private static readonly State[][] stateArray;

        static JsonWriter()
        {
            State[][] stateArray = new State[8][];
            stateArray[0] = new State[] { State.Error, State.Error, State.Error, State.Error, State.Error, State.Error, State.Error, State.Error, State.Error, State.Error };
            stateArray[1] = new State[] { State.ObjectStart, State.ObjectStart, State.Error, State.Error, State.ObjectStart, State.ObjectStart, State.ObjectStart, State.ObjectStart, State.Error, State.Error };
            stateArray[2] = new State[] { State.ArrayStart, State.ArrayStart, State.Error, State.Error, State.ArrayStart, State.ArrayStart, State.ArrayStart, State.ArrayStart, State.Error, State.Error };
            stateArray[3] = new State[] { State.ConstructorStart, State.ConstructorStart, State.Error, State.Error, State.ConstructorStart, State.ConstructorStart, State.ConstructorStart, State.ConstructorStart, State.Error, State.Error };
            stateArray[4] = new State[] { State.Property, State.Error, State.Property, State.Property, State.Error, State.Error, State.Error, State.Error, State.Error, State.Error };
            State[] stateArray2 = new State[10];
            stateArray2[1] = State.Property;
            stateArray2[2] = State.ObjectStart;
            stateArray2[3] = State.Object;
            stateArray2[4] = State.ArrayStart;
            stateArray2[5] = State.Array;
            stateArray2[6] = State.Constructor;
            stateArray2[7] = State.Constructor;
            stateArray2[8] = State.Error;
            stateArray2[9] = State.Error;
            stateArray[5] = stateArray2;
            stateArray2 = new State[10];
            stateArray2[1] = State.Property;
            stateArray2[2] = State.ObjectStart;
            stateArray2[3] = State.Object;
            stateArray2[4] = State.ArrayStart;
            stateArray2[5] = State.Array;
            stateArray2[6] = State.Constructor;
            stateArray2[7] = State.Constructor;
            stateArray2[8] = State.Error;
            stateArray2[9] = State.Error;
            stateArray[6] = stateArray2;
            stateArray2 = new State[10];
            stateArray2[1] = State.Object;
            stateArray2[2] = State.Error;
            stateArray2[3] = State.Error;
            stateArray2[4] = State.Array;
            stateArray2[5] = State.Array;
            stateArray2[6] = State.Constructor;
            stateArray2[7] = State.Constructor;
            stateArray2[8] = State.Error;
            stateArray2[9] = State.Error;
            stateArray[7] = stateArray2;
            JsonWriter.stateArray = stateArray;
        }

        protected JsonWriter()
        {
            this._stack.Add(JTokenType.None);
            this._currentState = State.Start;
            this._formatting = Newtonsoft.Json.Formatting.None;
            this.CloseOutput = true;
        }

        internal void AutoComplete(JsonToken tokenBeingWritten)
        {
            int num;
            switch (tokenBeingWritten)
            {
                case JsonToken.Integer:
                case JsonToken.Float:
                case JsonToken.String:
                case JsonToken.Boolean:
                case JsonToken.Null:
                case JsonToken.Undefined:
                case JsonToken.Date:
                case JsonToken.Bytes:
                    num = 7;
                    break;

                default:
                    num = (int) tokenBeingWritten;
                    break;
            }
            State state = stateArray[num][(int) this._currentState];
            if (state == State.Error)
            {
                throw new JsonWriterException("Token {0} in state {1} would result in an invalid JavaScript object.".FormatWith(CultureInfo.InvariantCulture, new object[] { tokenBeingWritten.ToString(), this._currentState.ToString() }));
            }
            if ((((this._currentState == State.Object) || (this._currentState == State.Array)) || (this._currentState == State.Constructor)) && (tokenBeingWritten != JsonToken.Comment))
            {
                this.WriteValueDelimiter();
            }
            else if ((this._currentState == State.Property) && (this._formatting == Newtonsoft.Json.Formatting.Indented))
            {
                this.WriteIndentSpace();
            }
            Newtonsoft.Json.WriteState writeState = this.WriteState;
            if ((((tokenBeingWritten == JsonToken.PropertyName) && (writeState != Newtonsoft.Json.WriteState.Start)) || (writeState == Newtonsoft.Json.WriteState.Array)) || (writeState == Newtonsoft.Json.WriteState.Constructor))
            {
                this.WriteIndent();
            }
            this._currentState = state;
        }

        private void AutoCompleteAll()
        {
            while (this._top > 0)
            {
                this.WriteEnd();
            }
        }

        private void AutoCompleteClose(JsonToken tokenBeingClosed)
        {
            int num2;
            int num = 0;
            for (num2 = 0; num2 < this._top; num2++)
            {
                int num3 = this._top - num2;
                if (((JTokenType) this._stack[num3]) == this.GetTypeForCloseToken(tokenBeingClosed))
                {
                    num = num2 + 1;
                    break;
                }
            }
            if (num == 0)
            {
                throw new JsonWriterException("No token to close.");
            }
            for (num2 = 0; num2 < num; num2++)
            {
                JsonToken closeTokenForType = this.GetCloseTokenForType(this.Pop());
                if ((this._currentState != State.ObjectStart) && (this._currentState != State.ArrayStart))
                {
                    this.WriteIndent();
                }
                this.WriteEnd(closeTokenForType);
            }
            JTokenType type = this.Peek();
            switch (type)
            {
                case JTokenType.None:
                    this._currentState = State.Start;
                    break;

                case JTokenType.Object:
                    this._currentState = State.Object;
                    break;

                case JTokenType.Array:
                    this._currentState = State.Array;
                    break;

                case JTokenType.Constructor:
                    this._currentState = State.Array;
                    break;

                default:
                    throw new JsonWriterException("Unknown JsonType: " + type);
            }
        }

        public virtual void Close()
        {
            this.AutoCompleteAll();
        }

        private void Dispose(bool disposing)
        {
            if (this.WriteState != Newtonsoft.Json.WriteState.Closed)
            {
                this.Close();
            }
        }

        public abstract void Flush();
        private JsonToken GetCloseTokenForType(JTokenType type)
        {
            switch (type)
            {
                case JTokenType.Object:
                    return JsonToken.EndObject;

                case JTokenType.Array:
                    return JsonToken.EndArray;

                case JTokenType.Constructor:
                    return JsonToken.EndConstructor;
            }
            throw new JsonWriterException("No close token for type: " + type);
        }

        private JTokenType GetTypeForCloseToken(JsonToken token)
        {
            switch (token)
            {
                case JsonToken.EndObject:
                    return JTokenType.Object;

                case JsonToken.EndArray:
                    return JTokenType.Array;

                case JsonToken.EndConstructor:
                    return JTokenType.Constructor;
            }
            throw new JsonWriterException("No type for token: " + token);
        }

        private bool IsEndToken(JsonToken token)
        {
            switch (token)
            {
                case JsonToken.EndObject:
                case JsonToken.EndArray:
                case JsonToken.EndConstructor:
                    return true;
            }
            return false;
        }

        private bool IsStartToken(JsonToken token)
        {
            switch (token)
            {
                case JsonToken.StartObject:
                case JsonToken.StartArray:
                case JsonToken.StartConstructor:
                    return true;
            }
            return false;
        }

        private JTokenType Peek()
        {
            return this._stack[this._top];
        }

        private JTokenType Pop()
        {
            JTokenType type = this.Peek();
            this._top--;
            return type;
        }

        private void Push(JTokenType value)
        {
            this._top++;
            if (this._stack.Count <= this._top)
            {
                this._stack.Add(value);
            }
            else
            {
                this._stack[this._top] = value;
            }
        }

        void IDisposable.Dispose()
        {
            this.Dispose(true);
        }

        public virtual void WriteComment(string text)
        {
            this.AutoComplete(JsonToken.Comment);
        }

        private void WriteConstructorDate(JsonReader reader)
        {
            if (!reader.Read())
            {
                throw new Exception("Unexpected end while reading date constructor.");
            }
            if (reader.TokenType != JsonToken.Integer)
            {
                throw new Exception("Unexpected token while reading date constructor. Expected Integer, got " + reader.TokenType);
            }
            long javaScriptTicks = (long) reader.Value;
            DateTime time = JsonConvert.ConvertJavaScriptTicksToDateTime(javaScriptTicks);
            if (!reader.Read())
            {
                throw new Exception("Unexpected end while reading date constructor.");
            }
            if (reader.TokenType != JsonToken.EndConstructor)
            {
                throw new Exception("Unexpected token while reading date constructor. Expected EndConstructor, got " + reader.TokenType);
            }
            this.WriteValue(time);
        }

        public void WriteEnd()
        {
            this.WriteEnd(this.Peek());
        }

        protected virtual void WriteEnd(JsonToken token)
        {
        }

        private void WriteEnd(JTokenType type)
        {
            switch (type)
            {
                case JTokenType.Object:
                    this.WriteEndObject();
                    break;

                case JTokenType.Array:
                    this.WriteEndArray();
                    break;

                case JTokenType.Constructor:
                    this.WriteEndConstructor();
                    break;

                default:
                    throw new JsonWriterException("Unexpected type when writing end: " + type);
            }
        }

        public void WriteEndArray()
        {
            this.AutoCompleteClose(JsonToken.EndArray);
        }

        public void WriteEndConstructor()
        {
            this.AutoCompleteClose(JsonToken.EndConstructor);
        }

        public void WriteEndObject()
        {
            this.AutoCompleteClose(JsonToken.EndObject);
        }

        protected virtual void WriteIndent()
        {
        }

        protected virtual void WriteIndentSpace()
        {
        }

        public virtual void WriteNull()
        {
            this.AutoComplete(JsonToken.Null);
        }

        public virtual void WritePropertyName(string name)
        {
            this.AutoComplete(JsonToken.PropertyName);
        }

        public virtual void WriteRaw(string json)
        {
        }

        public virtual void WriteRawValue(string json)
        {
            this.AutoComplete(JsonToken.Undefined);
            this.WriteRaw(json);
        }

        public virtual void WriteStartArray()
        {
            this.AutoComplete(JsonToken.StartArray);
            this.Push(JTokenType.Array);
        }

        public virtual void WriteStartConstructor(string name)
        {
            this.AutoComplete(JsonToken.StartConstructor);
            this.Push(JTokenType.Constructor);
        }

        public virtual void WriteStartObject()
        {
            this.AutoComplete(JsonToken.StartObject);
            this.Push(JTokenType.Object);
        }

        public void WriteToken(JsonReader reader)
        {
            int depth;
            ValidationUtils.ArgumentNotNull(reader, "reader");
            if (reader.TokenType == JsonToken.None)
            {
                depth = -1;
            }
            else if (!this.IsStartToken(reader.TokenType))
            {
                depth = reader.Depth + 1;
            }
            else
            {
                depth = reader.Depth;
            }
            this.WriteToken(reader, depth);
        }

        internal void WriteToken(JsonReader reader, int initialDepth)
        {
            do
            {
                switch (reader.TokenType)
                {
                    case JsonToken.None:
                        break;

                    case JsonToken.StartObject:
                        this.WriteStartObject();
                        break;

                    case JsonToken.StartArray:
                        this.WriteStartArray();
                        break;

                    case JsonToken.StartConstructor:
                        if (string.Compare(reader.Value.ToString(), "Date", StringComparison.Ordinal) == 0)
                        {
                            this.WriteConstructorDate(reader);
                        }
                        else
                        {
                            this.WriteStartConstructor(reader.Value.ToString());
                        }
                        break;

                    case JsonToken.PropertyName:
                        this.WritePropertyName(reader.Value.ToString());
                        break;

                    case JsonToken.Comment:
                        this.WriteComment(reader.Value.ToString());
                        break;

                    case JsonToken.Raw:
                        this.WriteRawValue((string) reader.Value);
                        break;

                    case JsonToken.Integer:
                        this.WriteValue((long) reader.Value);
                        break;

                    case JsonToken.Float:
                        this.WriteValue((double) reader.Value);
                        break;

                    case JsonToken.String:
                        this.WriteValue(reader.Value.ToString());
                        break;

                    case JsonToken.Boolean:
                        this.WriteValue((bool) reader.Value);
                        break;

                    case JsonToken.Null:
                        this.WriteNull();
                        break;

                    case JsonToken.Undefined:
                        this.WriteUndefined();
                        break;

                    case JsonToken.EndObject:
                        this.WriteEndObject();
                        break;

                    case JsonToken.EndArray:
                        this.WriteEndArray();
                        break;

                    case JsonToken.EndConstructor:
                        this.WriteEndConstructor();
                        break;

                    case JsonToken.Date:
                        this.WriteValue((DateTime) reader.Value);
                        break;

                    case JsonToken.Bytes:
                        this.WriteValue((byte[]) reader.Value);
                        break;

                    default:
                        throw MiscellaneousUtils.CreateArgumentOutOfRangeException("TokenType", reader.TokenType, "Unexpected token type.");
                }
            }
            while (((initialDepth - 1) < (reader.Depth - (this.IsEndToken(reader.TokenType) ? 1 : 0))) && reader.Read());
        }

        public virtual void WriteUndefined()
        {
            this.AutoComplete(JsonToken.Undefined);
        }

        public virtual void WriteValue(bool value)
        {
            this.AutoComplete(JsonToken.Boolean);
        }

        public virtual void WriteValue(byte value)
        {
            this.AutoComplete(JsonToken.Integer);
        }

        public virtual void WriteValue(char value)
        {
            this.AutoComplete(JsonToken.String);
        }

        public virtual void WriteValue(DateTime value)
        {
            this.AutoComplete(JsonToken.Date);
        }

        public virtual void WriteValue(DateTimeOffset value)
        {
            this.AutoComplete(JsonToken.Date);
        }

        public virtual void WriteValue(decimal value)
        {
            this.AutoComplete(JsonToken.Float);
        }

        public virtual void WriteValue(double value)
        {
            this.AutoComplete(JsonToken.Float);
        }

        public virtual void WriteValue(short value)
        {
            this.AutoComplete(JsonToken.Integer);
        }

        public virtual void WriteValue(int value)
        {
            this.AutoComplete(JsonToken.Integer);
        }

        public virtual void WriteValue(long value)
        {
            this.AutoComplete(JsonToken.Integer);
        }

        public virtual void WriteValue(bool? value)
        {
            if (!value.HasValue)
            {
                this.WriteNull();
            }
            else
            {
                this.WriteValue(value.Value);
            }
        }

        public virtual void WriteValue(byte? value)
        {
            byte? nullable = value;
            int? nullable2 = nullable.HasValue ? new int?(nullable.GetValueOrDefault()) : null;
            if (!nullable2.HasValue)
            {
                this.WriteNull();
            }
            else
            {
                this.WriteValue(value.Value);
            }
        }

        public virtual void WriteValue(char? value)
        {
            char? nullable = value;
            int? nullable2 = nullable.HasValue ? new int?(nullable.GetValueOrDefault()) : null;
            if (!nullable2.HasValue)
            {
                this.WriteNull();
            }
            else
            {
                this.WriteValue(value.Value);
            }
        }

        public virtual void WriteValue(DateTime? value)
        {
            if (!value.HasValue)
            {
                this.WriteNull();
            }
            else
            {
                this.WriteValue(value.Value);
            }
        }

        public virtual void WriteValue(DateTimeOffset? value)
        {
            if (!value.HasValue)
            {
                this.WriteNull();
            }
            else
            {
                this.WriteValue(value.Value);
            }
        }

        public virtual void WriteValue(decimal? value)
        {
            if (!value.HasValue)
            {
                this.WriteNull();
            }
            else
            {
                this.WriteValue(value.Value);
            }
        }

        public virtual void WriteValue(double? value)
        {
            if (!value.HasValue)
            {
                this.WriteNull();
            }
            else
            {
                this.WriteValue(value.Value);
            }
        }

        public virtual void WriteValue(short? value)
        {
            short? nullable = value;
            int? nullable2 = nullable.HasValue ? new int?(nullable.GetValueOrDefault()) : null;
            if (!nullable2.HasValue)
            {
                this.WriteNull();
            }
            else
            {
                this.WriteValue(value.Value);
            }
        }

        public virtual void WriteValue(int? value)
        {
            if (!value.HasValue)
            {
                this.WriteNull();
            }
            else
            {
                this.WriteValue(value.Value);
            }
        }

        public virtual void WriteValue(long? value)
        {
            if (!value.HasValue)
            {
                this.WriteNull();
            }
            else
            {
                this.WriteValue(value.Value);
            }
        }

        [CLSCompliant(false)]
        public virtual void WriteValue(sbyte? value)
        {
            sbyte? nullable = value;
            int? nullable2 = nullable.HasValue ? new int?(nullable.GetValueOrDefault()) : null;
            if (!nullable2.HasValue)
            {
                this.WriteNull();
            }
            else
            {
                this.WriteValue(value.Value);
            }
        }

        public virtual void WriteValue(float? value)
        {
            if (!value.HasValue)
            {
                this.WriteNull();
            }
            else
            {
                this.WriteValue(value.Value);
            }
        }

        [CLSCompliant(false)]
        public virtual void WriteValue(ushort? value)
        {
            ushort? nullable = value;
            int? nullable2 = nullable.HasValue ? new int?(nullable.GetValueOrDefault()) : null;
            if (!nullable2.HasValue)
            {
                this.WriteNull();
            }
            else
            {
                this.WriteValue(value.Value);
            }
        }

        [CLSCompliant(false)]
        public virtual void WriteValue(uint? value)
        {
            if (!value.HasValue)
            {
                this.WriteNull();
            }
            else
            {
                this.WriteValue(value.Value);
            }
        }

        [CLSCompliant(false)]
        public virtual void WriteValue(ulong? value)
        {
            if (!value.HasValue)
            {
                this.WriteNull();
            }
            else
            {
                this.WriteValue(value.Value);
            }
        }

        public virtual void WriteValue(object value)
        {
            if (value == null)
            {
                this.WriteNull();
            }
            else
            {
                if (value is IConvertible)
                {
                    IConvertible convertible = value as IConvertible;
                    switch (convertible.GetTypeCode())
                    {
                        case TypeCode.DBNull:
                            this.WriteNull();
                            return;

                        case TypeCode.Boolean:
                            this.WriteValue(convertible.ToBoolean(CultureInfo.InvariantCulture));
                            return;

                        case TypeCode.Char:
                            this.WriteValue(convertible.ToChar(CultureInfo.InvariantCulture));
                            return;

                        case TypeCode.SByte:
                            this.WriteValue(convertible.ToSByte(CultureInfo.InvariantCulture));
                            return;

                        case TypeCode.Byte:
                            this.WriteValue(convertible.ToByte(CultureInfo.InvariantCulture));
                            return;

                        case TypeCode.Int16:
                            this.WriteValue(convertible.ToInt16(CultureInfo.InvariantCulture));
                            return;

                        case TypeCode.UInt16:
                            this.WriteValue(convertible.ToUInt16(CultureInfo.InvariantCulture));
                            return;

                        case TypeCode.Int32:
                            this.WriteValue(convertible.ToInt32(CultureInfo.InvariantCulture));
                            return;

                        case TypeCode.UInt32:
                            this.WriteValue(convertible.ToUInt32(CultureInfo.InvariantCulture));
                            return;

                        case TypeCode.Int64:
                            this.WriteValue(convertible.ToInt64(CultureInfo.InvariantCulture));
                            return;

                        case TypeCode.UInt64:
                            this.WriteValue(convertible.ToUInt64(CultureInfo.InvariantCulture));
                            return;

                        case TypeCode.Single:
                            this.WriteValue(convertible.ToSingle(CultureInfo.InvariantCulture));
                            return;

                        case TypeCode.Double:
                            this.WriteValue(convertible.ToDouble(CultureInfo.InvariantCulture));
                            return;

                        case TypeCode.Decimal:
                            this.WriteValue(convertible.ToDecimal(CultureInfo.InvariantCulture));
                            return;

                        case TypeCode.DateTime:
                            this.WriteValue(convertible.ToDateTime(CultureInfo.InvariantCulture));
                            return;

                        case TypeCode.String:
                            this.WriteValue(convertible.ToString(CultureInfo.InvariantCulture));
                            return;
                    }
                }
                else
                {
                    if (value is DateTimeOffset)
                    {
                        this.WriteValue((DateTimeOffset) value);
                        return;
                    }
                    if (value is byte[])
                    {
                        this.WriteValue((byte[]) value);
                        return;
                    }
                }
                throw new ArgumentException("Unsupported type: {0}. Use the JsonSerializer class to get the object's JSON representation.".FormatWith(CultureInfo.InvariantCulture, new object[] { value.GetType() }));
            }
        }

        [CLSCompliant(false)]
        public virtual void WriteValue(sbyte value)
        {
            this.AutoComplete(JsonToken.Integer);
        }

        public virtual void WriteValue(float value)
        {
            this.AutoComplete(JsonToken.Float);
        }

        public virtual void WriteValue(string value)
        {
            this.AutoComplete(JsonToken.String);
        }

        [CLSCompliant(false)]
        public virtual void WriteValue(ushort value)
        {
            this.AutoComplete(JsonToken.Integer);
        }

        [CLSCompliant(false)]
        public virtual void WriteValue(uint value)
        {
            this.AutoComplete(JsonToken.Integer);
        }

        [CLSCompliant(false)]
        public virtual void WriteValue(ulong value)
        {
            this.AutoComplete(JsonToken.Integer);
        }

        public virtual void WriteValue(byte[] value)
        {
            if (value == null)
            {
                this.WriteNull();
            }
            else
            {
                this.AutoComplete(JsonToken.Bytes);
            }
        }

        protected virtual void WriteValueDelimiter()
        {
        }

        public virtual void WriteWhitespace(string ws)
        {
            if ((ws != null) && !StringUtils.IsWhiteSpace(ws))
            {
                throw new JsonWriterException("Only white space characters should be used.");
            }
        }

        public bool CloseOutput { get; set; }

        public Newtonsoft.Json.Formatting Formatting
        {
            get
            {
                return this._formatting;
            }
            set
            {
                this._formatting = value;
            }
        }

        protected internal int Top
        {
            get
            {
                return this._top;
            }
        }

        public Newtonsoft.Json.WriteState WriteState
        {
            get
            {
                switch (this._currentState)
                {
                    case State.Start:
                        return Newtonsoft.Json.WriteState.Start;

                    case State.Property:
                        return Newtonsoft.Json.WriteState.Property;

                    case State.ObjectStart:
                    case State.Object:
                        return Newtonsoft.Json.WriteState.Object;

                    case State.ArrayStart:
                    case State.Array:
                        return Newtonsoft.Json.WriteState.Array;

                    case State.ConstructorStart:
                    case State.Constructor:
                        return Newtonsoft.Json.WriteState.Constructor;

                    case State.Closed:
                        return Newtonsoft.Json.WriteState.Closed;

                    case State.Error:
                        return Newtonsoft.Json.WriteState.Error;
                }
                throw new JsonWriterException("Invalid state: " + this._currentState);
            }
        }

        private enum State
        {
            Start,
            Property,
            ObjectStart,
            Object,
            ArrayStart,
            Array,
            ConstructorStart,
            Constructor,
            Bytes,
            Closed,
            Error
        }
    }
}

