namespace Newtonsoft.Json
{
    using Newtonsoft.Json.Linq;
    using Newtonsoft.Json.Utilities;
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Runtime.CompilerServices;

    public abstract class JsonReader : IDisposable
    {
        private State _currentState = State.Start;
        private JTokenType _currentTypeContext;
        private char _quoteChar;
        private readonly List<JTokenType> _stack = new List<JTokenType>();
        private JsonToken _token;
        private int _top;
        private object _value;
        private Type _valueType;

        protected JsonReader()
        {
            this.CloseInput = true;
            this.Push(JTokenType.None);
        }

        public virtual void Close()
        {
            this._currentState = State.Closed;
            this._token = JsonToken.None;
            this._value = null;
            this._valueType = null;
        }

        protected virtual void Dispose(bool disposing)
        {
            if ((this._currentState != State.Closed) && disposing)
            {
                this.Close();
            }
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
            throw new JsonReaderException("Not a valid close JsonToken: {0}".FormatWith(CultureInfo.InvariantCulture, new object[] { token }));
        }

        internal static bool IsPrimitiveToken(JsonToken token)
        {
            switch (token)
            {
                case JsonToken.Integer:
                case JsonToken.Float:
                case JsonToken.String:
                case JsonToken.Boolean:
                case JsonToken.Null:
                case JsonToken.Undefined:
                case JsonToken.Date:
                case JsonToken.Bytes:
                    return true;
            }
            return false;
        }

        internal static bool IsStartToken(JsonToken token)
        {
            switch (token)
            {
                case JsonToken.None:
                case JsonToken.Comment:
                case JsonToken.Raw:
                case JsonToken.Integer:
                case JsonToken.Float:
                case JsonToken.String:
                case JsonToken.Boolean:
                case JsonToken.Null:
                case JsonToken.Undefined:
                case JsonToken.EndObject:
                case JsonToken.EndArray:
                case JsonToken.EndConstructor:
                case JsonToken.Date:
                case JsonToken.Bytes:
                    return false;

                case JsonToken.StartObject:
                case JsonToken.StartArray:
                case JsonToken.StartConstructor:
                case JsonToken.PropertyName:
                    return true;
            }
            throw MiscellaneousUtils.CreateArgumentOutOfRangeException("token", token, "Unexpected JsonToken value.");
        }

        private JTokenType Peek()
        {
            return this._currentTypeContext;
        }

        private JTokenType Pop()
        {
            JTokenType type = this.Peek();
            this._stack.RemoveAt(this._stack.Count - 1);
            this._top--;
            this._currentTypeContext = this._stack[this._top - 1];
            return type;
        }

        private void Push(JTokenType value)
        {
            this._stack.Add(value);
            this._top++;
            this._currentTypeContext = value;
        }

        public abstract bool Read();
        public abstract byte[] ReadAsBytes();
        public abstract DateTimeOffset? ReadAsDateTimeOffset();
        public abstract decimal? ReadAsDecimal();
        protected void SetStateBasedOnCurrent()
        {
            JTokenType type = this.Peek();
            switch (type)
            {
                case JTokenType.None:
                    this._currentState = State.Finished;
                    break;

                case JTokenType.Object:
                    this._currentState = State.Object;
                    break;

                case JTokenType.Array:
                    this._currentState = State.Array;
                    break;

                case JTokenType.Constructor:
                    this._currentState = State.Constructor;
                    break;

                default:
                    throw new JsonReaderException("While setting the reader state back to current object an unexpected JsonType was encountered: {0}".FormatWith(CultureInfo.InvariantCulture, new object[] { type }));
            }
        }

        protected void SetToken(JsonToken newToken)
        {
            this.SetToken(newToken, null);
        }

        protected virtual void SetToken(JsonToken newToken, object value)
        {
            this._token = newToken;
            switch (newToken)
            {
                case JsonToken.StartObject:
                    this._currentState = State.ObjectStart;
                    this.Push(JTokenType.Object);
                    break;

                case JsonToken.StartArray:
                    this._currentState = State.ArrayStart;
                    this.Push(JTokenType.Array);
                    break;

                case JsonToken.StartConstructor:
                    this._currentState = State.ConstructorStart;
                    this.Push(JTokenType.Constructor);
                    break;

                case JsonToken.PropertyName:
                    this._currentState = State.Property;
                    this.Push(JTokenType.Property);
                    break;

                case JsonToken.Raw:
                case JsonToken.Integer:
                case JsonToken.Float:
                case JsonToken.String:
                case JsonToken.Boolean:
                case JsonToken.Null:
                case JsonToken.Undefined:
                case JsonToken.Date:
                case JsonToken.Bytes:
                    this._currentState = State.PostValue;
                    break;

                case JsonToken.EndObject:
                    this.ValidateEnd(JsonToken.EndObject);
                    this._currentState = State.PostValue;
                    break;

                case JsonToken.EndArray:
                    this.ValidateEnd(JsonToken.EndArray);
                    this._currentState = State.PostValue;
                    break;

                case JsonToken.EndConstructor:
                    this.ValidateEnd(JsonToken.EndConstructor);
                    this._currentState = State.PostValue;
                    break;
            }
            if ((this.Peek() == JTokenType.Property) && (this._currentState == State.PostValue))
            {
                this.Pop();
            }
            if (value != null)
            {
                this._value = value;
                this._valueType = value.GetType();
            }
            else
            {
                this._value = null;
                this._valueType = null;
            }
        }

        public void Skip()
        {
            if (IsStartToken(this.TokenType))
            {
                int depth = this.Depth;
                while (this.Read() && (depth < this.Depth))
                {
                }
            }
        }

        void IDisposable.Dispose()
        {
            this.Dispose(true);
        }

        private void ValidateEnd(JsonToken endToken)
        {
            JTokenType type = this.Pop();
            if (this.GetTypeForCloseToken(endToken) != type)
            {
                throw new JsonReaderException("JsonToken {0} is not valid for closing JsonType {1}.".FormatWith(CultureInfo.InvariantCulture, new object[] { endToken, type }));
            }
        }

        public bool CloseInput { get; set; }

        protected State CurrentState
        {
            get
            {
                return this._currentState;
            }
        }

        public virtual int Depth
        {
            get
            {
                int num = this._top - 1;
                if (IsStartToken(this.TokenType))
                {
                    return (num - 1);
                }
                return num;
            }
        }

        public virtual char QuoteChar
        {
            get
            {
                return this._quoteChar;
            }
            protected internal set
            {
                this._quoteChar = value;
            }
        }

        public virtual JsonToken TokenType
        {
            get
            {
                return this._token;
            }
        }

        public virtual object Value
        {
            get
            {
                return this._value;
            }
        }

        public virtual Type ValueType
        {
            get
            {
                return this._valueType;
            }
        }

        protected enum State
        {
            Start,
            Complete,
            Property,
            ObjectStart,
            Object,
            ArrayStart,
            Array,
            Closed,
            PostValue,
            ConstructorStart,
            Constructor,
            Error,
            Finished
        }
    }
}

