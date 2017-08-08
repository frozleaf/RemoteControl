using Newtonsoft.Json.Utilities;
using System;
using System.Globalization;
using System.IO;

namespace Newtonsoft.Json
{
    public class JsonTextReader : JsonReader, IJsonLineInfo
    {
        private enum ReadType
        {
            Read,
            ReadAsBytes,
            ReadAsDecimal,
            ReadAsDateTimeOffset
        }

        private const int LineFeedValue = 10;

        private const int CarriageReturnValue = 13;

        private readonly TextReader _reader;

        private readonly StringBuffer _buffer;

        private char? _lastChar;

        private int _currentLinePosition;

        private int _currentLineNumber;

        private bool _end;

        private JsonTextReader.ReadType _readType;

        public int LineNumber
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
                    result = this._currentLineNumber;
                }
                return result;
            }
        }

        public int LinePosition
        {
            get
            {
                return this._currentLinePosition;
            }
        }

        public JsonTextReader(TextReader reader)
        {
            if (reader == null)
            {
                throw new ArgumentNullException("reader");
            }
            this._reader = reader;
            this._buffer = new StringBuffer(4096);
            this._currentLineNumber = 1;
        }

        private void ParseString(char quote)
        {
            this.ReadStringIntoBuffer(quote);
            if (this._readType == JsonTextReader.ReadType.ReadAsBytes)
            {
                byte[] value;
                if (this._buffer.Position == 0)
                {
                    value = new byte[0];
                }
                else
                {
                    value = Convert.FromBase64CharArray(this._buffer.GetInternalBuffer(), 0, this._buffer.Position);
                    this._buffer.Position = 0;
                }
                this.SetToken(JsonToken.Bytes, value);
            }
            else
            {
                string text = this._buffer.ToString();
                this._buffer.Position = 0;
                if (text.StartsWith("/Date(", StringComparison.Ordinal) && text.EndsWith(")/", StringComparison.Ordinal))
                {
                    this.ParseDate(text);
                }
                else
                {
                    this.SetToken(JsonToken.String, text);
                    this.QuoteChar = quote;
                }
            }
        }

        private void ReadStringIntoBuffer(char quote)
        {
            char c;
            while (true)
            {
                c = this.MoveNext();
                char c2 = c;
                if (c2 <= '"')
                {
                    if (c2 != '\0')
                    {
                        if (c2 != '"')
                        {
                            goto IL_2FD;
                        }
                        goto IL_2DC;
                    }
                    else
                    {
                        if (this._end)
                        {
                            break;
                        }
                        this._buffer.Append('\0');
                    }
                }
                else
                {
                    if (c2 == '\'')
                    {
                        goto IL_2DC;
                    }
                    if (c2 != '\\')
                    {
                        goto IL_2FD;
                    }
                    if ((c = this.MoveNext()) == '\0' && this._end)
                    {
                        goto IL_29B;
                    }
                    c2 = c;
                    if (c2 <= '\\')
                    {
                        if (c2 <= '\'')
                        {
                            if (c2 != '"' && c2 != '\'')
                            {
                                goto Block_12;
                            }
                        }
                        else if (c2 != '/')
                        {
                            if (c2 != '\\')
                            {
                                goto Block_14;
                            }
                            this._buffer.Append('\\');
                            continue;
                        }
                        this._buffer.Append(c);
                    }
                    else if (c2 <= 'f')
                    {
                        if (c2 != 'b')
                        {
                            if (c2 != 'f')
                            {
                                goto Block_17;
                            }
                            this._buffer.Append('\f');
                        }
                        else
                        {
                            this._buffer.Append('\b');
                        }
                    }
                    else
                    {
                        if (c2 != 'n')
                        {
                            switch (c2)
                            {
                                case 'r':
                                    this._buffer.Append('\r');
                                    continue;
                                case 't':
                                    this._buffer.Append('\t');
                                    continue;
                                case 'u':
                                    {
                                        char[] array = new char[4];
                                        for (int i = 0; i < array.Length; i++)
                                        {
                                            if ((c = this.MoveNext()) == '\0' && this._end)
                                            {
                                                goto IL_1E1;
                                            }
                                            array[i] = c;
                                        }
                                        char value = Convert.ToChar(int.Parse(new string(array), NumberStyles.HexNumber, NumberFormatInfo.InvariantInfo));
                                        this._buffer.Append(value);
                                        continue;
                                    }
                            }
                            goto Block_19;
                        }
                        this._buffer.Append('\n');
                    }
                }
                continue;
            IL_2DC:
                if (c == quote)
                {
                    return;
                }
                this._buffer.Append(c);
                continue;
            IL_2FD:
                this._buffer.Append(c);
            }
            throw this.CreateJsonReaderException("Unterminated string. Expected delimiter: {0}. Line {1}, position {2}.", new object[]
			{
				quote,
				this._currentLineNumber,
				this._currentLinePosition
			});
        Block_12:
        Block_14:
        Block_17:
        Block_19:
            goto IL_250;
        IL_1E1:
            throw this.CreateJsonReaderException("Unexpected end while parsing unicode character. Line {0}, position {1}.", new object[]
			{
				this._currentLineNumber,
				this._currentLinePosition
			});
        IL_250:
            throw this.CreateJsonReaderException("Bad JSON escape sequence: {0}. Line {1}, position {2}.", new object[]
			{
				"\\" + c,
				this._currentLineNumber,
				this._currentLinePosition
			});
        IL_29B:
            throw this.CreateJsonReaderException("Unterminated string. Expected delimiter: {0}. Line {1}, position {2}.", new object[]
			{
				quote,
				this._currentLineNumber,
				this._currentLinePosition
			});
        }

        private JsonReaderException CreateJsonReaderException(string format, params object[] args)
        {
            string message = format.FormatWith(CultureInfo.InvariantCulture, args);
            return new JsonReaderException(message, null, this._currentLineNumber, this._currentLinePosition);
        }

        private TimeSpan ReadOffset(string offsetText)
        {
            bool flag = offsetText[0] == '-';
            int num = int.Parse(offsetText.Substring(1, 2), NumberStyles.Integer, CultureInfo.InvariantCulture);
            int num2 = 0;
            if (offsetText.Length >= 5)
            {
                num2 = int.Parse(offsetText.Substring(3, 2), NumberStyles.Integer, CultureInfo.InvariantCulture);
            }
            TimeSpan result = TimeSpan.FromHours((double)num) + TimeSpan.FromMinutes((double)num2);
            if (flag)
            {
                result = result.Negate();
            }
            return result;
        }

        private void ParseDate(string text)
        {
            string text2 = text.Substring(6, text.Length - 8);
            DateTimeKind dateTimeKind = DateTimeKind.Utc;
            int num = text2.IndexOf('+', 1);
            if (num == -1)
            {
                num = text2.IndexOf('-', 1);
            }
            TimeSpan timeSpan = TimeSpan.Zero;
            if (num != -1)
            {
                dateTimeKind = DateTimeKind.Local;
                timeSpan = this.ReadOffset(text2.Substring(num));
                text2 = text2.Substring(0, num);
            }
            long javaScriptTicks = long.Parse(text2, NumberStyles.Integer, CultureInfo.InvariantCulture);
            DateTime dateTime = JsonConvert.ConvertJavaScriptTicksToDateTime(javaScriptTicks);
            if (this._readType == JsonTextReader.ReadType.ReadAsDateTimeOffset)
            {
                this.SetToken(JsonToken.Date, new DateTimeOffset(dateTime.Add(timeSpan).Ticks, timeSpan));
            }
            else
            {
                DateTime dateTime2;
                switch (dateTimeKind)
                {
                    case DateTimeKind.Unspecified:
                        dateTime2 = DateTime.SpecifyKind(dateTime.ToLocalTime(), DateTimeKind.Unspecified);
                        goto IL_EC;
                    case DateTimeKind.Local:
                        dateTime2 = dateTime.ToLocalTime();
                        goto IL_EC;
                }
                dateTime2 = dateTime;
            IL_EC:
                this.SetToken(JsonToken.Date, dateTime2);
            }
        }

        private char MoveNext()
        {
            int num = this._reader.Read();
            int num2 = num;
            char result;
            if (num2 != -1)
            {
                if (num2 != 10)
                {
                    if (num2 != 13)
                    {
                        this._currentLinePosition++;
                    }
                    else
                    {
                        if (this._reader.Peek() == 10)
                        {
                            this._reader.Read();
                        }
                        this._currentLineNumber++;
                        this._currentLinePosition = 0;
                    }
                }
                else
                {
                    this._currentLineNumber++;
                    this._currentLinePosition = 0;
                }
                result = (char)num;
            }
            else
            {
                this._end = true;
                result = '\0';
            }
            return result;
        }

        private bool HasNext()
        {
            return this._reader.Peek() != -1;
        }

        private int PeekNext()
        {
            return this._reader.Peek();
        }

        public override bool Read()
        {
            this._readType = JsonTextReader.ReadType.Read;
            return this.ReadInternal();
        }

        public override byte[] ReadAsBytes()
        {
            this._readType = JsonTextReader.ReadType.ReadAsBytes;
            while (this.ReadInternal())
            {
                if (this.TokenType != JsonToken.Comment)
                {
                    byte[] result;
                    if (this.TokenType == JsonToken.Null)
                    {
                        result = null;
                    }
                    else
                    {
                        if (this.TokenType != JsonToken.Bytes)
                        {
                            throw this.CreateJsonReaderException("Unexpected token when reading bytes: {0}. Line {1}, position {2}.", new object[]
							{
								this.TokenType,
								this._currentLineNumber,
								this._currentLinePosition
							});
                        }
                        result = (byte[])this.Value;
                    }
                    return result;
                }
            }
            throw this.CreateJsonReaderException("Unexpected end when reading bytes: Line {0}, position {1}.", new object[]
			{
				this._currentLineNumber,
				this._currentLinePosition
			});
        }

        public override decimal? ReadAsDecimal()
        {
            this._readType = JsonTextReader.ReadType.ReadAsDecimal;
            while (this.ReadInternal())
            {
                if (this.TokenType != JsonToken.Comment)
                {
                    decimal? result;
                    if (this.TokenType == JsonToken.Null)
                    {
                        result = null;
                    }
                    else if (this.TokenType == JsonToken.Float)
                    {
                        result = (decimal?)this.Value;
                    }
                    else
                    {
                        decimal num;
                        if (this.TokenType != JsonToken.String || !decimal.TryParse((string)this.Value, NumberStyles.Number, CultureInfo.InvariantCulture, out num))
                        {
                            throw this.CreateJsonReaderException("Unexpected token when reading decimal: {0}. Line {1}, position {2}.", new object[]
							{
								this.TokenType,
								this._currentLineNumber,
								this._currentLinePosition
							});
                        }
                        this.SetToken(JsonToken.Float, num);
                        result = new decimal?(num);
                    }
                    return result;
                }
            }
            throw this.CreateJsonReaderException("Unexpected end when reading decimal: Line {0}, position {1}.", new object[]
			{
				this._currentLineNumber,
				this._currentLinePosition
			});
        }

        public override DateTimeOffset? ReadAsDateTimeOffset()
        {
            this._readType = JsonTextReader.ReadType.ReadAsDateTimeOffset;
            while (this.ReadInternal())
            {
                if (this.TokenType != JsonToken.Comment)
                {
                    DateTimeOffset? result;
                    if (this.TokenType == JsonToken.Null)
                    {
                        result = null;
                    }
                    else
                    {
                        if (this.TokenType != JsonToken.Date)
                        {
                            throw this.CreateJsonReaderException("Unexpected token when reading date: {0}. Line {1}, position {2}.", new object[]
							{
								this.TokenType,
								this._currentLineNumber,
								this._currentLinePosition
							});
                        }
                        result = new DateTimeOffset?((DateTimeOffset)this.Value);
                    }
                    return result;
                }
            }
            throw this.CreateJsonReaderException("Unexpected end when reading date: Line {0}, position {1}.", new object[]
			{
				this._currentLineNumber,
				this._currentLinePosition
			});
        }

        private bool ReadInternal()
        {
            char c;
            while (true)
            {
                char? lastChar = this._lastChar;
                if ((lastChar.HasValue ? new int?((int)lastChar.GetValueOrDefault()) : null).HasValue)
                {
                    c = this._lastChar.Value;
                    this._lastChar = null;
                }
                else
                {
                    c = this.MoveNext();
                }
                if (c == '\0' && this._end)
                {
                    break;
                }
                switch (base.CurrentState)
                {
                    case JsonReader.State.Start:
                    case JsonReader.State.Property:
                    case JsonReader.State.ArrayStart:
                    case JsonReader.State.Array:
                    case JsonReader.State.ConstructorStart:
                    case JsonReader.State.Constructor:
                        goto IL_C1;
                    case JsonReader.State.Complete:
                        continue;
                    case JsonReader.State.ObjectStart:
                    case JsonReader.State.Object:
                        goto IL_CD;
                    case JsonReader.State.Closed:
                        continue;
                    case JsonReader.State.PostValue:
                        if (this.ParsePostValue(c))
                        {
                            goto Block_6;
                        }
                        continue;
                    case JsonReader.State.Error:
                        continue;
                }
                goto Block_5;
            }
            bool result = false;
            return result;
        Block_5:
            throw this.CreateJsonReaderException("Unexpected state: {0}. Line {1}, position {2}.", new object[]
			{
				base.CurrentState,
				this._currentLineNumber,
				this._currentLinePosition
			});
        IL_C1:
            result = this.ParseValue(c);
            return result;
        IL_CD:
            result = this.ParseObject(c);
            return result;
        Block_6:
            result = true;
            return result;
        }

        private bool ParsePostValue(char currentChar)
        {
            while (true)
            {
                char c = currentChar;
                if (c <= ')')
                {
                    switch (c)
                    {
                        case '\t':
                        case '\n':
                        case '\r':
                            break;
                        case '\v':
                        case '\f':
                            goto IL_97;
                        default:
                            if (c != ' ')
                            {
                                if (c != ')')
                                {
                                    goto IL_97;
                                }
                                goto IL_6F;
                            }
                            break;
                    }
                }
                else if (c <= '/')
                {
                    if (c == ',')
                    {
                        goto IL_8A;
                    }
                    if (c != '/')
                    {
                        goto IL_97;
                    }
                    goto IL_7F;
                }
                else
                {
                    if (c == ']')
                    {
                        goto IL_5F;
                    }
                    if (c != '}')
                    {
                        goto IL_97;
                    }
                    break;
                }
            IL_E4:
                if ((currentChar = this.MoveNext()) == '\0' && this._end)
                {
                    goto Block_11;
                }
                continue;
            IL_97:
                if (char.IsWhiteSpace(currentChar))
                {
                    goto IL_E4;
                }
                goto IL_A8;
            }
            base.SetToken(JsonToken.EndObject);
            bool result = true;
            return result;
        IL_5F:
            base.SetToken(JsonToken.EndArray);
            result = true;
            return result;
        IL_6F:
            base.SetToken(JsonToken.EndConstructor);
            result = true;
            return result;
        IL_7F:
            this.ParseComment();
            result = true;
            return result;
        IL_8A:
            base.SetStateBasedOnCurrent();
            result = false;
            return result;
        IL_A8:
            throw this.CreateJsonReaderException("After parsing a value an unexpected character was encountered: {0}. Line {1}, position {2}.", new object[]
			{
				currentChar,
				this._currentLineNumber,
				this._currentLinePosition
			});
        Block_11:
            result = false;
            return result;
        }

        private bool ParseObject(char currentChar)
        {
            while (true)
            {
                char c = currentChar;
                if (c <= ' ')
                {
                    switch (c)
                    {
                        case '\t':
                        case '\n':
                        case '\r':
                            break;
                        case '\v':
                        case '\f':
                            goto IL_53;
                        default:
                            if (c != ' ')
                            {
                                goto IL_53;
                            }
                            break;
                    }
                }
                else
                {
                    if (c == '/')
                    {
                        goto IL_46;
                    }
                    if (c != '}')
                    {
                        goto IL_53;
                    }
                    break;
                }
            IL_71:
                if ((currentChar = this.MoveNext()) == '\0' && this._end)
                {
                    goto Block_7;
                }
                continue;
            IL_53:
                if (char.IsWhiteSpace(currentChar))
                {
                    goto IL_71;
                }
                goto IL_64;
            }
            base.SetToken(JsonToken.EndObject);
            bool result = true;
            return result;
        IL_46:
            this.ParseComment();
            result = true;
            return result;
        IL_64:
            result = this.ParseProperty(currentChar);
            return result;
        Block_7:
            result = false;
            return result;
        }

        private bool ParseProperty(char firstChar)
        {
            char c = firstChar;
            char c2;
            if (this.ValidIdentifierChar(c))
            {
                c2 = '\0';
                c = this.ParseUnquotedProperty(c);
            }
            else
            {
                if (c != '"' && c != '\'')
                {
                    throw this.CreateJsonReaderException("Invalid property identifier character: {0}. Line {1}, position {2}.", new object[]
					{
						c,
						this._currentLineNumber,
						this._currentLinePosition
					});
                }
                c2 = c;
                this.ReadStringIntoBuffer(c2);
                c = this.MoveNext();
            }
            if (c != ':')
            {
                c = this.MoveNext();
                this.EatWhitespace(c, false, out c);
                if (c != ':')
                {
                    throw this.CreateJsonReaderException("Invalid character after parsing property name. Expected ':' but got: {0}. Line {1}, position {2}.", new object[]
					{
						c,
						this._currentLineNumber,
						this._currentLinePosition
					});
                }
            }
            this.SetToken(JsonToken.PropertyName, this._buffer.ToString());
            this.QuoteChar = c2;
            this._buffer.Position = 0;
            return true;
        }

        private bool ValidIdentifierChar(char value)
        {
            return char.IsLetterOrDigit(value) || value == '_' || value == '$';
        }

        private char ParseUnquotedProperty(char firstChar)
        {
            this._buffer.Append(firstChar);
            char c;
            while ((c = this.MoveNext()) != '\0' || !this._end)
            {
                if (char.IsWhiteSpace(c) || c == ':')
                {
                    return c;
                }
                if (!this.ValidIdentifierChar(c))
                {
                    throw this.CreateJsonReaderException("Invalid JavaScript property identifier character: {0}. Line {1}, position {2}.", new object[]
					{
						c,
						this._currentLineNumber,
						this._currentLinePosition
					});
                }
                this._buffer.Append(c);
            }
            throw this.CreateJsonReaderException("Unexpected end when parsing unquoted property name. Line {0}, position {1}.", new object[]
			{
				this._currentLineNumber,
				this._currentLinePosition
			});
        }

        private bool ParseValue(char currentChar)
        {
            while (true)
            {
                char c = currentChar;
                if (c <= 'N')
                {
                    if (c <= '"')
                    {
                        switch (c)
                        {
                            case '\t':
                            case '\n':
                            case '\r':
                                break;
                            case '\v':
                            case '\f':
                                goto IL_284;
                            default:
                                switch (c)
                                {
                                    case ' ':
                                        break;
                                    case '!':
                                        goto IL_284;
                                    case '"':
                                        goto IL_DB;
                                    default:
                                        goto IL_284;
                                }
                                break;
                        }
                    }
                    else
                    {
                        switch (c)
                        {
                            case '\'':
                                goto IL_DB;
                            case '(':
                            case '*':
                            case '+':
                            case '.':
                                goto IL_284;
                            case ')':
                                goto IL_272;
                            case ',':
                                goto IL_262;
                            case '-':
                                goto IL_1DF;
                            case '/':
                                goto IL_208;
                            default:
                                if (c == 'I')
                                {
                                    goto IL_1D1;
                                }
                                if (c != 'N')
                                {
                                    goto IL_284;
                                }
                                goto IL_1C3;
                        }
                    }
                }
                else if (c <= 'f')
                {
                    switch (c)
                    {
                        case '[':
                            goto IL_233;
                        case '\\':
                            goto IL_284;
                        case ']':
                            goto IL_252;
                        default:
                            if (c != 'f')
                            {
                                goto IL_284;
                            }
                            goto IL_F8;
                    }
                }
                else
                {
                    if (c == 'n')
                    {
                        goto IL_106;
                    }
                    switch (c)
                    {
                        case 't':
                            goto IL_EA;
                        case 'u':
                            goto IL_216;
                        default:
                            switch (c)
                            {
                                case '{':
                                    goto IL_224;
                                case '|':
                                    goto IL_284;
                                case '}':
                                    goto IL_242;
                                default:
                                    goto IL_284;
                            }
                            break;
                    }
                }
            IL_2FF:
                if ((currentChar = this.MoveNext()) == '\0' && this._end)
                {
                    goto Block_22;
                }
                continue;
            IL_284:
                if (char.IsWhiteSpace(currentChar))
                {
                    goto IL_2FF;
                }
                goto IL_295;
            }
        IL_DB:
            this.ParseString(currentChar);
            bool result = true;
            return result;
        IL_EA:
            this.ParseTrue();
            result = true;
            return result;
        IL_F8:
            this.ParseFalse();
            result = true;
            return result;
        IL_106:
            if (this.HasNext())
            {
                char c2 = (char)this.PeekNext();
                if (c2 == 'u')
                {
                    this.ParseNull();
                }
                else
                {
                    if (c2 != 'e')
                    {
                        throw this.CreateJsonReaderException("Unexpected character encountered while parsing value: {0}. Line {1}, position {2}.", new object[]
						{
							currentChar,
							this._currentLineNumber,
							this._currentLinePosition
						});
                    }
                    this.ParseConstructor();
                }
                result = true;
                return result;
            }
            throw this.CreateJsonReaderException("Unexpected end. Line {0}, position {1}.", new object[]
			{
				this._currentLineNumber,
				this._currentLinePosition
			});
        IL_1C3:
            this.ParseNumberNaN();
            result = true;
            return result;
        IL_1D1:
            this.ParseNumberPositiveInfinity();
            result = true;
            return result;
        IL_1DF:
            if (this.PeekNext() == 73)
            {
                this.ParseNumberNegativeInfinity();
            }
            else
            {
                this.ParseNumber(currentChar);
            }
            result = true;
            return result;
        IL_208:
            this.ParseComment();
            result = true;
            return result;
        IL_216:
            this.ParseUndefined();
            result = true;
            return result;
        IL_224:
            base.SetToken(JsonToken.StartObject);
            result = true;
            return result;
        IL_233:
            base.SetToken(JsonToken.StartArray);
            result = true;
            return result;
        IL_242:
            base.SetToken(JsonToken.EndObject);
            result = true;
            return result;
        IL_252:
            base.SetToken(JsonToken.EndArray);
            result = true;
            return result;
        IL_262:
            base.SetToken(JsonToken.Undefined);
            result = true;
            return result;
        IL_272:
            base.SetToken(JsonToken.EndConstructor);
            result = true;
            return result;
        IL_295:
            if (char.IsNumber(currentChar) || currentChar == '-' || currentChar == '.')
            {
                this.ParseNumber(currentChar);
                result = true;
                return result;
            }
            throw this.CreateJsonReaderException("Unexpected character encountered while parsing value: {0}. Line {1}, position {2}.", new object[]
			{
				currentChar,
				this._currentLineNumber,
				this._currentLinePosition
			});
        Block_22:
            result = false;
            return result;
        }

        private bool EatWhitespace(char initialChar, bool oneOrMore, out char finalChar)
        {
            bool flag = false;
            char c = initialChar;
            while (c == ' ' || char.IsWhiteSpace(c))
            {
                flag = true;
                c = this.MoveNext();
            }
            finalChar = c;
            return !oneOrMore || flag;
        }

        private void ParseConstructor()
        {
            if (this.MatchValue('n', "new", true))
            {
                char c = this.MoveNext();
                if (this.EatWhitespace(c, true, out c))
                {
                    while (char.IsLetter(c))
                    {
                        this._buffer.Append(c);
                        c = this.MoveNext();
                    }
                    this.EatWhitespace(c, false, out c);
                    if (c != '(')
                    {
                        throw this.CreateJsonReaderException("Unexpected character while parsing constructor: {0}. Line {1}, position {2}.", new object[]
						{
							c,
							this._currentLineNumber,
							this._currentLinePosition
						});
                    }
                    string value = this._buffer.ToString();
                    this._buffer.Position = 0;
                    this.SetToken(JsonToken.StartConstructor, value);
                }
            }
        }

        private void ParseNumber(char firstChar)
        {
            char c = firstChar;
            bool flag = false;
            do
            {
                if (this.IsSeperator(c))
                {
                    flag = true;
                    this._lastChar = new char?(c);
                }
                else
                {
                    this._buffer.Append(c);
                }
            }
            while (!flag && ((c = this.MoveNext()) != '\0' || !this._end));
            string text = this._buffer.ToString();
            bool flag2 = firstChar == '0' && !text.StartsWith("0.", StringComparison.OrdinalIgnoreCase);
            object value2;
            JsonToken newToken;
            if (this._readType == JsonTextReader.ReadType.ReadAsDecimal)
            {
                if (flag2)
                {
                    long value = text.StartsWith("0x", StringComparison.OrdinalIgnoreCase) ? Convert.ToInt64(text, 16) : Convert.ToInt64(text, 8);
                    value2 = Convert.ToDecimal(value);
                }
                else
                {
                    value2 = decimal.Parse(text, NumberStyles.AllowLeadingWhite | NumberStyles.AllowTrailingWhite | NumberStyles.AllowLeadingSign | NumberStyles.AllowTrailingSign | NumberStyles.AllowDecimalPoint | NumberStyles.AllowThousands | NumberStyles.AllowExponent, CultureInfo.InvariantCulture);
                }
                newToken = JsonToken.Float;
            }
            else if (flag2)
            {
                value2 = (text.StartsWith("0x", StringComparison.OrdinalIgnoreCase) ? Convert.ToInt64(text, 16) : Convert.ToInt64(text, 8));
                newToken = JsonToken.Integer;
            }
            else if (text.IndexOf(".", StringComparison.OrdinalIgnoreCase) != -1 || text.IndexOf("e", StringComparison.OrdinalIgnoreCase) != -1)
            {
                value2 = Convert.ToDouble(text, CultureInfo.InvariantCulture);
                newToken = JsonToken.Float;
            }
            else
            {
                try
                {
                    value2 = Convert.ToInt64(text, CultureInfo.InvariantCulture);
                }
                catch (OverflowException innerException)
                {
                    throw new JsonReaderException("JSON integer {0} is too large or small for an Int64.".FormatWith(CultureInfo.InvariantCulture, new object[]
					{
						text
					}), innerException);
                }
                newToken = JsonToken.Integer;
            }
            this._buffer.Position = 0;
            this.SetToken(newToken, value2);
        }

        private void ParseComment()
        {
            char c = this.MoveNext();
            if (c == '*')
            {
                while ((c = this.MoveNext()) != '\0' || !this._end)
                {
                    if (c == '*')
                    {
                        if ((c = this.MoveNext()) != '\0' || !this._end)
                        {
                            if (c == '/')
                            {
                                break;
                            }
                            this._buffer.Append('*');
                            this._buffer.Append(c);
                        }
                    }
                    else
                    {
                        this._buffer.Append(c);
                    }
                }
                this.SetToken(JsonToken.Comment, this._buffer.ToString());
                this._buffer.Position = 0;
                return;
            }
            throw this.CreateJsonReaderException("Error parsing comment. Expected: *. Line {0}, position {1}.", new object[]
			{
				this._currentLineNumber,
				this._currentLinePosition
			});
        }

        private bool MatchValue(char firstChar, string value)
        {
            char ch = firstChar;
            int num = 0;
            do
            {
                if (ch != value[num])
                {
                    break;
                }
                num++;
            }
            while ((num < value.Length) && (((ch = this.MoveNext()) != '\0') || !this._end));
            return (num == value.Length);
        }

        private bool MatchValue(char firstChar, string value, bool noTrailingNonSeperatorCharacters)
        {
            bool flag = this.MatchValue(firstChar, value);
            bool result;
            if (!noTrailingNonSeperatorCharacters)
            {
                result = flag;
            }
            else
            {
                int num = this.PeekNext();
                char c = (num != -1) ? ((char)num) : '\0';
                bool flag2 = flag && (c == '\0' || this.IsSeperator(c));
                result = flag2;
            }
            return result;
        }

        private bool IsSeperator(char c)
        {
            bool result;
            if (c <= ')')
            {
                switch (c)
                {
                    case '\t':
                    case '\n':
                    case '\r':
                        break;
                    case '\v':
                    case '\f':
                        goto IL_92;
                    default:
                        if (c != ' ')
                        {
                            if (c != ')')
                            {
                                goto IL_92;
                            }
                            if (base.CurrentState == JsonReader.State.Constructor || base.CurrentState == JsonReader.State.ConstructorStart)
                            {
                                result = true;
                                return result;
                            }
                            goto IL_A5;
                        }
                        break;
                }
                result = true;
                return result;
            }
            if (c <= '/')
            {
                if (c != ',')
                {
                    if (c != '/')
                    {
                        goto IL_92;
                    }
                    result = (this.HasNext() && this.PeekNext() == 42);
                    return result;
                }
            }
            else if (c != ']' && c != '}')
            {
                goto IL_92;
            }
            result = true;
            return result;
        IL_92:
            if (char.IsWhiteSpace(c))
            {
                result = true;
                return result;
            }
        IL_A5:
            result = false;
            return result;
        }

        private void ParseTrue()
        {
            if (this.MatchValue('t', JsonConvert.True, true))
            {
                this.SetToken(JsonToken.Boolean, true);
                return;
            }
            throw this.CreateJsonReaderException("Error parsing boolean value. Line {0}, position {1}.", new object[]
			{
				this._currentLineNumber,
				this._currentLinePosition
			});
        }

        private void ParseNull()
        {
            if (this.MatchValue('n', JsonConvert.Null, true))
            {
                base.SetToken(JsonToken.Null);
                return;
            }
            throw this.CreateJsonReaderException("Error parsing null value. Line {0}, position {1}.", new object[]
			{
				this._currentLineNumber,
				this._currentLinePosition
			});
        }

        private void ParseUndefined()
        {
            if (this.MatchValue('u', JsonConvert.Undefined, true))
            {
                base.SetToken(JsonToken.Undefined);
                return;
            }
            throw this.CreateJsonReaderException("Error parsing undefined value. Line {0}, position {1}.", new object[]
			{
				this._currentLineNumber,
				this._currentLinePosition
			});
        }

        private void ParseFalse()
        {
            if (this.MatchValue('f', JsonConvert.False, true))
            {
                this.SetToken(JsonToken.Boolean, false);
                return;
            }
            throw this.CreateJsonReaderException("Error parsing boolean value. Line {0}, position {1}.", new object[]
			{
				this._currentLineNumber,
				this._currentLinePosition
			});
        }

        private void ParseNumberNegativeInfinity()
        {
            if (this.MatchValue('-', JsonConvert.NegativeInfinity, true))
            {
                this.SetToken(JsonToken.Float, double.NegativeInfinity);
                return;
            }
            throw this.CreateJsonReaderException("Error parsing negative infinity value. Line {0}, position {1}.", new object[]
			{
				this._currentLineNumber,
				this._currentLinePosition
			});
        }

        private void ParseNumberPositiveInfinity()
        {
            if (this.MatchValue('I', JsonConvert.PositiveInfinity, true))
            {
                this.SetToken(JsonToken.Float, double.PositiveInfinity);
                return;
            }
            throw this.CreateJsonReaderException("Error parsing positive infinity value. Line {0}, position {1}.", new object[]
			{
				this._currentLineNumber,
				this._currentLinePosition
			});
        }

        private void ParseNumberNaN()
        {
            if (this.MatchValue('N', JsonConvert.NaN, true))
            {
                this.SetToken(JsonToken.Float, double.NaN);
                return;
            }
            throw this.CreateJsonReaderException("Error parsing NaN value. Line {0}, position {1}.", new object[]
			{
				this._currentLineNumber,
				this._currentLinePosition
			});
        }

        public override void Close()
        {
            base.Close();
            if (base.CloseInput && this._reader != null)
            {
                this._reader.Close();
            }
            if (this._buffer != null)
            {
                this._buffer.Clear();
            }
        }

        public bool HasLineInfo()
        {
            return true;
        }
    }
}
