using Newtonsoft.Json.Utilities;
using System;
using System.Globalization;

namespace Newtonsoft.Json.Linq
{
    public class JTokenReader : JsonReader, IJsonLineInfo
    {
        private readonly JToken _root;

        private JToken _parent;

        private JToken _current;

        private bool IsEndElement
        {
            get
            {
                return this._current == this._parent;
            }
        }

        int IJsonLineInfo.LineNumber
        {
            get
            {
                int result;
                if (base.CurrentState == JsonReader.State.Start)
                {
                    result = 0;
                }
                else
                {
                    IJsonLineInfo jsonLineInfo = this.IsEndElement ? null : this._current;
                    if (jsonLineInfo != null)
                    {
                        result = jsonLineInfo.LineNumber;
                    }
                    else
                    {
                        result = 0;
                    }
                }
                return result;
            }
        }

        int IJsonLineInfo.LinePosition
        {
            get
            {
                int result;
                if (base.CurrentState == JsonReader.State.Start)
                {
                    result = 0;
                }
                else
                {
                    IJsonLineInfo jsonLineInfo = this.IsEndElement ? null : this._current;
                    if (jsonLineInfo != null)
                    {
                        result = jsonLineInfo.LinePosition;
                    }
                    else
                    {
                        result = 0;
                    }
                }
                return result;
            }
        }

        public JTokenReader(JToken token)
        {
            ValidationUtils.ArgumentNotNull(token, "token");
            this._root = token;
            this._current = token;
        }

        public override byte[] ReadAsBytes()
        {
            this.Read();
            if (this.TokenType == JsonToken.String)
            {
                string text = (string)this.Value;
                byte[] value = (text.Length == 0) ? new byte[0] : Convert.FromBase64String(text);
                this.SetToken(JsonToken.Bytes, value);
            }
            byte[] result;
            if (this.TokenType == JsonToken.Null)
            {
                result = null;
            }
            else
            {
                if (this.TokenType != JsonToken.Bytes)
                {
                    throw new JsonReaderException("Error reading bytes. Expected bytes but got {0}.".FormatWith(CultureInfo.InvariantCulture, new object[]
					{
						this.TokenType
					}));
                }
                result = (byte[])this.Value;
            }
            return result;
        }

        public override decimal? ReadAsDecimal()
        {
            this.Read();
            decimal? result;
            if (this.TokenType == JsonToken.Null)
            {
                result = null;
            }
            else
            {
                if (this.TokenType != JsonToken.Integer && this.TokenType != JsonToken.Float)
                {
                    throw new JsonReaderException("Error reading decimal. Expected a number but got {0}.".FormatWith(CultureInfo.InvariantCulture, new object[]
					{
						this.TokenType
					}));
                }
                this.SetToken(JsonToken.Float, Convert.ToDecimal(this.Value, CultureInfo.InvariantCulture));
                result = new decimal?((decimal)this.Value);
            }
            return result;
        }

        public override DateTimeOffset? ReadAsDateTimeOffset()
        {
            this.Read();
            DateTimeOffset? result;
            if (this.TokenType == JsonToken.Null)
            {
                result = null;
            }
            else
            {
                if (this.TokenType != JsonToken.Date)
                {
                    throw new JsonReaderException("Error reading date. Expected bytes but got {0}.".FormatWith(CultureInfo.InvariantCulture, new object[]
					{
						this.TokenType
					}));
                }
                this.SetToken(JsonToken.Date, new DateTimeOffset((DateTime)this.Value));
                result = new DateTimeOffset?((DateTimeOffset)this.Value);
            }
            return result;
        }

        public override bool Read()
        {
            bool result;
            if (base.CurrentState != JsonReader.State.Start)
            {
                JContainer jContainer = this._current as JContainer;
                if (jContainer != null && this._parent != jContainer)
                {
                    result = this.ReadInto(jContainer);
                }
                else
                {
                    result = this.ReadOver(this._current);
                }
            }
            else
            {
                this.SetToken(this._current);
                result = true;
            }
            return result;
        }

        private bool ReadOver(JToken t)
        {
            bool result;
            if (t == this._root)
            {
                result = this.ReadToEnd();
            }
            else
            {
                JToken next = t.Next;
                if (next == null || next == t || t == t.Parent.Last)
                {
                    if (t.Parent == null)
                    {
                        result = this.ReadToEnd();
                    }
                    else
                    {
                        result = this.SetEnd(t.Parent);
                    }
                }
                else
                {
                    this._current = next;
                    this.SetToken(this._current);
                    result = true;
                }
            }
            return result;
        }

        private bool ReadToEnd()
        {
            return false;
        }

        private JsonToken? GetEndToken(JContainer c)
        {
            JsonToken? result;
            switch (c.Type)
            {
                case JTokenType.Object:
                    result = new JsonToken?(JsonToken.EndObject);
                    break;
                case JTokenType.Array:
                    result = new JsonToken?(JsonToken.EndArray);
                    break;
                case JTokenType.Constructor:
                    result = new JsonToken?(JsonToken.EndConstructor);
                    break;
                case JTokenType.Property:
                    result = null;
                    break;
                default:
                    throw MiscellaneousUtils.CreateArgumentOutOfRangeException("Type", c.Type, "Unexpected JContainer type.");
            }
            return result;
        }

        private bool ReadInto(JContainer c)
        {
            JToken first = c.First;
            bool result;
            if (first == null)
            {
                result = this.SetEnd(c);
            }
            else
            {
                this.SetToken(first);
                this._current = first;
                this._parent = c;
                result = true;
            }
            return result;
        }

        private bool SetEnd(JContainer c)
        {
            JsonToken? endToken = this.GetEndToken(c);
            bool result;
            if (endToken.HasValue)
            {
                base.SetToken(endToken.Value);
                this._current = c;
                this._parent = c;
                result = true;
            }
            else
            {
                result = this.ReadOver(c);
            }
            return result;
        }

        private void SetToken(JToken token)
        {
            switch (token.Type)
            {
                case JTokenType.Object:
                    base.SetToken(JsonToken.StartObject);
                    break;
                case JTokenType.Array:
                    base.SetToken(JsonToken.StartArray);
                    break;
                case JTokenType.Constructor:
                    base.SetToken(JsonToken.StartConstructor);
                    break;
                case JTokenType.Property:
                    this.SetToken(JsonToken.PropertyName, ((JProperty)token).Name);
                    break;
                case JTokenType.Comment:
                    this.SetToken(JsonToken.Comment, ((JValue)token).Value);
                    break;
                case JTokenType.Integer:
                    this.SetToken(JsonToken.Integer, ((JValue)token).Value);
                    break;
                case JTokenType.Float:
                    this.SetToken(JsonToken.Float, ((JValue)token).Value);
                    break;
                case JTokenType.String:
                    this.SetToken(JsonToken.String, ((JValue)token).Value);
                    break;
                case JTokenType.Boolean:
                    this.SetToken(JsonToken.Boolean, ((JValue)token).Value);
                    break;
                case JTokenType.Null:
                    this.SetToken(JsonToken.Null, ((JValue)token).Value);
                    break;
                case JTokenType.Undefined:
                    this.SetToken(JsonToken.Undefined, ((JValue)token).Value);
                    break;
                case JTokenType.Date:
                    this.SetToken(JsonToken.Date, ((JValue)token).Value);
                    break;
                case JTokenType.Raw:
                    this.SetToken(JsonToken.Raw, ((JValue)token).Value);
                    break;
                case JTokenType.Bytes:
                    this.SetToken(JsonToken.Bytes, ((JValue)token).Value);
                    break;
                default:
                    throw MiscellaneousUtils.CreateArgumentOutOfRangeException("Type", token.Type, "Unexpected JTokenType.");
            }
        }

        bool IJsonLineInfo.HasLineInfo()
        {
            bool result;
            if (base.CurrentState == JsonReader.State.Start)
            {
                result = false;
            }
            else
            {
                IJsonLineInfo jsonLineInfo = this.IsEndElement ? null : this._current;
                result = (jsonLineInfo != null && jsonLineInfo.HasLineInfo());
            }
            return result;
        }
    }
}
