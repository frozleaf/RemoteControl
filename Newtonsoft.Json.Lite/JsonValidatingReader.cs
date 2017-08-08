using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Schema;
using Newtonsoft.Json.Utilities;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace Newtonsoft.Json
{
    public class JsonValidatingReader : JsonReader, IJsonLineInfo
    {
        private class SchemaScope
        {
            private readonly JTokenType _tokenType;

            private readonly IList<JsonSchemaModel> _schemas;

            private readonly Dictionary<string, bool> _requiredProperties;

            public string CurrentPropertyName
            {
                get;
                set;
            }

            public int ArrayItemCount
            {
                get;
                set;
            }

            public IList<JsonSchemaModel> Schemas
            {
                get
                {
                    return this._schemas;
                }
            }

            public Dictionary<string, bool> RequiredProperties
            {
                get
                {
                    return this._requiredProperties;
                }
            }

            public JTokenType TokenType
            {
                get
                {
                    return this._tokenType;
                }
            }

            public SchemaScope(JTokenType tokenType, IList<JsonSchemaModel> schemas)
            {
                this._tokenType = tokenType;
                this._schemas = schemas;
                this._requiredProperties = schemas.SelectMany(new Func<JsonSchemaModel, IEnumerable<string>>(this.GetRequiredProperties)).Distinct<string>().ToDictionary((string p) => p, (string p) => false);
            }

            private IEnumerable<string> GetRequiredProperties(JsonSchemaModel schema)
            {
                IEnumerable<string> result;
                if (schema == null || schema.Properties == null)
                {
                    result = Enumerable.Empty<string>();
                }
                else
                {
                    result = from p in schema.Properties
                             where p.Value.Required
                             select p.Key;
                }
                return result;
            }
        }

        private readonly JsonReader _reader;

        private readonly Stack<JsonValidatingReader.SchemaScope> _stack;

        private JsonSchema _schema;

        private JsonSchemaModel _model;

        private JsonValidatingReader.SchemaScope _currentScope;

        public event ValidationEventHandler ValidationEventHandler;

        public override object Value
        {
            get
            {
                return this._reader.Value;
            }
        }

        public override int Depth
        {
            get
            {
                return this._reader.Depth;
            }
        }

        public override char QuoteChar
        {
            get
            {
                return this._reader.QuoteChar;
            }
            protected internal set
            {
            }
        }

        public override JsonToken TokenType
        {
            get
            {
                return this._reader.TokenType;
            }
        }

        public override Type ValueType
        {
            get
            {
                return this._reader.ValueType;
            }
        }

        private IEnumerable<JsonSchemaModel> CurrentSchemas
        {
            get
            {
                return this._currentScope.Schemas;
            }
        }

        private IEnumerable<JsonSchemaModel> CurrentMemberSchemas
        {
            get
            {
                IEnumerable<JsonSchemaModel> result;
                if (this._currentScope == null)
                {
                    result = new List<JsonSchemaModel>(new JsonSchemaModel[]
					{
						this._model
					});
                }
                else if (this._currentScope.Schemas == null || this._currentScope.Schemas.Count == 0)
                {
                    result = Enumerable.Empty<JsonSchemaModel>();
                }
                else
                {
                    switch (this._currentScope.TokenType)
                    {
                        case JTokenType.None:
                            result = this._currentScope.Schemas;
                            break;
                        case JTokenType.Object:
                            {
                                if (this._currentScope.CurrentPropertyName == null)
                                {
                                    throw new Exception("CurrentPropertyName has not been set on scope.");
                                }
                                IList<JsonSchemaModel> list = new List<JsonSchemaModel>();
                                foreach (JsonSchemaModel current in this.CurrentSchemas)
                                {
                                    JsonSchemaModel item;
                                    if (current.Properties != null && current.Properties.TryGetValue(this._currentScope.CurrentPropertyName, out item))
                                    {
                                        list.Add(item);
                                    }
                                    if (current.PatternProperties != null)
                                    {
                                        foreach (KeyValuePair<string, JsonSchemaModel> current2 in current.PatternProperties)
                                        {
                                            if (Regex.IsMatch(this._currentScope.CurrentPropertyName, current2.Key))
                                            {
                                                list.Add(current2.Value);
                                            }
                                        }
                                    }
                                    if (list.Count == 0 && current.AllowAdditionalProperties && current.AdditionalProperties != null)
                                    {
                                        list.Add(current.AdditionalProperties);
                                    }
                                }
                                result = list;
                                break;
                            }
                        case JTokenType.Array:
                            {
                                IList<JsonSchemaModel> list = new List<JsonSchemaModel>();
                                foreach (JsonSchemaModel current in this.CurrentSchemas)
                                {
                                    if (!CollectionUtils.IsNullOrEmpty<JsonSchemaModel>(current.Items))
                                    {
                                        if (current.Items.Count == 1)
                                        {
                                            list.Add(current.Items[0]);
                                        }
                                        if (current.Items.Count > this._currentScope.ArrayItemCount - 1)
                                        {
                                            list.Add(current.Items[this._currentScope.ArrayItemCount - 1]);
                                        }
                                    }
                                    if (current.AllowAdditionalProperties && current.AdditionalProperties != null)
                                    {
                                        list.Add(current.AdditionalProperties);
                                    }
                                }
                                result = list;
                                break;
                            }
                        case JTokenType.Constructor:
                            result = Enumerable.Empty<JsonSchemaModel>();
                            break;
                        default:
                            throw new ArgumentOutOfRangeException("TokenType", "Unexpected token type: {0}".FormatWith(CultureInfo.InvariantCulture, new object[]
						{
							this._currentScope.TokenType
						}));
                    }
                }
                return result;
            }
        }

        public JsonSchema Schema
        {
            get
            {
                return this._schema;
            }
            set
            {
                if (this.TokenType != JsonToken.None)
                {
                    throw new Exception("Cannot change schema while validating JSON.");
                }
                this._schema = value;
                this._model = null;
            }
        }

        public JsonReader Reader
        {
            get
            {
                return this._reader;
            }
        }

        int IJsonLineInfo.LineNumber
        {
            get
            {
                IJsonLineInfo jsonLineInfo = this._reader as IJsonLineInfo;
                return (jsonLineInfo != null) ? jsonLineInfo.LineNumber : 0;
            }
        }

        int IJsonLineInfo.LinePosition
        {
            get
            {
                IJsonLineInfo jsonLineInfo = this._reader as IJsonLineInfo;
                return (jsonLineInfo != null) ? jsonLineInfo.LinePosition : 0;
            }
        }

        private void Push(JsonValidatingReader.SchemaScope scope)
        {
            this._stack.Push(scope);
            this._currentScope = scope;
        }

        private JsonValidatingReader.SchemaScope Pop()
        {
            JsonValidatingReader.SchemaScope result = this._stack.Pop();
            this._currentScope = ((this._stack.Count != 0) ? this._stack.Peek() : null);
            return result;
        }

        private void RaiseError(string message, JsonSchemaModel schema)
        {
            string message2 = ((IJsonLineInfo)this).HasLineInfo() ? (message + " Line {0}, position {1}.".FormatWith(CultureInfo.InvariantCulture, new object[]
			{
				((IJsonLineInfo)this).LineNumber,
				((IJsonLineInfo)this).LinePosition
			})) : message;
            this.OnValidationEvent(new JsonSchemaException(message2, null, ((IJsonLineInfo)this).LineNumber, ((IJsonLineInfo)this).LinePosition));
        }

        private void OnValidationEvent(JsonSchemaException exception)
        {
            ValidationEventHandler validationEventHandler = this.ValidationEventHandler;
            if (validationEventHandler != null)
            {
                validationEventHandler(this, new ValidationEventArgs(exception));
                return;
            }
            throw exception;
        }

        public JsonValidatingReader(JsonReader reader)
        {
            ValidationUtils.ArgumentNotNull(reader, "reader");
            this._reader = reader;
            this._stack = new Stack<JsonValidatingReader.SchemaScope>();
        }

        private void ValidateInEnumAndNotDisallowed(JsonSchemaModel schema)
        {
            if (schema != null)
            {
                JToken jToken = new JValue(this._reader.Value);
                if (schema.Enum != null)
                {
                    StringWriter stringWriter = new StringWriter(CultureInfo.InvariantCulture);
                    jToken.WriteTo(new JsonTextWriter(stringWriter), new JsonConverter[0]);
                    if (!schema.Enum.ContainsValue(jToken, new JTokenEqualityComparer()))
                    {
                        this.RaiseError("Value {0} is not defined in enum.".FormatWith(CultureInfo.InvariantCulture, new object[]
						{
							stringWriter.ToString()
						}), schema);
                    }
                }
                JsonSchemaType? currentNodeSchemaType = this.GetCurrentNodeSchemaType();
                if (currentNodeSchemaType.HasValue)
                {
                    if (JsonSchemaGenerator.HasFlag(new JsonSchemaType?(schema.Disallow), currentNodeSchemaType.Value))
                    {
                        this.RaiseError("Type {0} is disallowed.".FormatWith(CultureInfo.InvariantCulture, new object[]
						{
							currentNodeSchemaType
						}), schema);
                    }
                }
            }
        }

        private JsonSchemaType? GetCurrentNodeSchemaType()
        {
            JsonSchemaType? result;
            switch (this._reader.TokenType)
            {
                case JsonToken.StartObject:
                    result = new JsonSchemaType?(JsonSchemaType.Object);
                    return result;
                case JsonToken.StartArray:
                    result = new JsonSchemaType?(JsonSchemaType.Array);
                    return result;
                case JsonToken.Integer:
                    result = new JsonSchemaType?(JsonSchemaType.Integer);
                    return result;
                case JsonToken.Float:
                    result = new JsonSchemaType?(JsonSchemaType.Float);
                    return result;
                case JsonToken.String:
                    result = new JsonSchemaType?(JsonSchemaType.String);
                    return result;
                case JsonToken.Boolean:
                    result = new JsonSchemaType?(JsonSchemaType.Boolean);
                    return result;
                case JsonToken.Null:
                    result = new JsonSchemaType?(JsonSchemaType.Null);
                    return result;
            }
            result = null;
            return result;
        }

        public override byte[] ReadAsBytes()
        {
            byte[] result = this._reader.ReadAsBytes();
            this.ValidateCurrentToken();
            return result;
        }

        public override decimal? ReadAsDecimal()
        {
            decimal? result = this._reader.ReadAsDecimal();
            this.ValidateCurrentToken();
            return result;
        }

        public override DateTimeOffset? ReadAsDateTimeOffset()
        {
            DateTimeOffset? result = this._reader.ReadAsDateTimeOffset();
            this.ValidateCurrentToken();
            return result;
        }

        public override bool Read()
        {
            bool result;
            if (!this._reader.Read())
            {
                result = false;
            }
            else if (this._reader.TokenType == JsonToken.Comment)
            {
                result = true;
            }
            else
            {
                this.ValidateCurrentToken();
                result = true;
            }
            return result;
        }

        private void ValidateCurrentToken()
        {
            if (this._model == null)
            {
                JsonSchemaModelBuilder jsonSchemaModelBuilder = new JsonSchemaModelBuilder();
                this._model = jsonSchemaModelBuilder.Build(this._schema);
            }
            switch (this._reader.TokenType)
            {
                case JsonToken.StartObject:
                    {
                        this.ProcessValue();
                        IList<JsonSchemaModel> schemas = this.CurrentMemberSchemas.Where(new Func<JsonSchemaModel, bool>(this.ValidateObject)).ToList<JsonSchemaModel>();
                        this.Push(new JsonValidatingReader.SchemaScope(JTokenType.Object, schemas));
                        return;
                    }
                case JsonToken.StartArray:
                    {
                        this.ProcessValue();
                        IList<JsonSchemaModel> schemas2 = this.CurrentMemberSchemas.Where(new Func<JsonSchemaModel, bool>(this.ValidateArray)).ToList<JsonSchemaModel>();
                        this.Push(new JsonValidatingReader.SchemaScope(JTokenType.Array, schemas2));
                        return;
                    }
                case JsonToken.StartConstructor:
                    this.Push(new JsonValidatingReader.SchemaScope(JTokenType.Constructor, null));
                    return;
                case JsonToken.PropertyName:
                    foreach (JsonSchemaModel current in this.CurrentSchemas)
                    {
                        this.ValidatePropertyName(current);
                    }
                    return;
                case JsonToken.Raw:
                    return;
                case JsonToken.Integer:
                    this.ProcessValue();
                    foreach (JsonSchemaModel current in this.CurrentMemberSchemas)
                    {
                        this.ValidateInteger(current);
                    }
                    return;
                case JsonToken.Float:
                    this.ProcessValue();
                    foreach (JsonSchemaModel current in this.CurrentMemberSchemas)
                    {
                        this.ValidateFloat(current);
                    }
                    return;
                case JsonToken.String:
                    this.ProcessValue();
                    foreach (JsonSchemaModel current in this.CurrentMemberSchemas)
                    {
                        this.ValidateString(current);
                    }
                    return;
                case JsonToken.Boolean:
                    this.ProcessValue();
                    foreach (JsonSchemaModel current in this.CurrentMemberSchemas)
                    {
                        this.ValidateBoolean(current);
                    }
                    return;
                case JsonToken.Null:
                    this.ProcessValue();
                    foreach (JsonSchemaModel current in this.CurrentMemberSchemas)
                    {
                        this.ValidateNull(current);
                    }
                    return;
                case JsonToken.Undefined:
                    return;
                case JsonToken.EndObject:
                    foreach (JsonSchemaModel current in this.CurrentSchemas)
                    {
                        this.ValidateEndObject(current);
                    }
                    this.Pop();
                    return;
                case JsonToken.EndArray:
                    foreach (JsonSchemaModel current in this.CurrentSchemas)
                    {
                        this.ValidateEndArray(current);
                    }
                    this.Pop();
                    return;
                case JsonToken.EndConstructor:
                    this.Pop();
                    return;
                case JsonToken.Date:
                    return;
            }
            throw new ArgumentOutOfRangeException();
        }

        private void ValidateEndObject(JsonSchemaModel schema)
        {
            if (schema != null)
            {
                Dictionary<string, bool> requiredProperties = this._currentScope.RequiredProperties;
                if (requiredProperties != null)
                {
                    List<string> list = (from kv in requiredProperties
                                         where !kv.Value
                                         select kv.Key).ToList<string>();
                    if (list.Count > 0)
                    {
                        this.RaiseError("Required properties are missing from object: {0}.".FormatWith(CultureInfo.InvariantCulture, new object[]
						{
							string.Join(", ", list.ToArray())
						}), schema);
                    }
                }
            }
        }

        private void ValidateEndArray(JsonSchemaModel schema)
        {
            if (schema != null)
            {
                int arrayItemCount = this._currentScope.ArrayItemCount;
                if (schema.MaximumItems.HasValue && arrayItemCount > schema.MaximumItems)
                {
                    this.RaiseError("Array item count {0} exceeds maximum count of {1}.".FormatWith(CultureInfo.InvariantCulture, new object[]
					{
						arrayItemCount,
						schema.MaximumItems
					}), schema);
                }
                if (schema.MinimumItems.HasValue && arrayItemCount < schema.MinimumItems)
                {
                    this.RaiseError("Array item count {0} is less than minimum count of {1}.".FormatWith(CultureInfo.InvariantCulture, new object[]
					{
						arrayItemCount,
						schema.MinimumItems
					}), schema);
                }
            }
        }

        private void ValidateNull(JsonSchemaModel schema)
        {
            if (schema != null)
            {
                if (this.TestType(schema, JsonSchemaType.Null))
                {
                    this.ValidateInEnumAndNotDisallowed(schema);
                }
            }
        }

        private void ValidateBoolean(JsonSchemaModel schema)
        {
            if (schema != null)
            {
                if (this.TestType(schema, JsonSchemaType.Boolean))
                {
                    this.ValidateInEnumAndNotDisallowed(schema);
                }
            }
        }

        private void ValidateString(JsonSchemaModel schema)
        {
            if (schema != null)
            {
                if (this.TestType(schema, JsonSchemaType.String))
                {
                    this.ValidateInEnumAndNotDisallowed(schema);
                    string text = this._reader.Value.ToString();
                    if (schema.MaximumLength.HasValue && text.Length > schema.MaximumLength)
                    {
                        this.RaiseError("String '{0}' exceeds maximum length of {1}.".FormatWith(CultureInfo.InvariantCulture, new object[]
						{
							text,
							schema.MaximumLength
						}), schema);
                    }
                    if (schema.MinimumLength.HasValue && text.Length < schema.MinimumLength)
                    {
                        this.RaiseError("String '{0}' is less than minimum length of {1}.".FormatWith(CultureInfo.InvariantCulture, new object[]
						{
							text,
							schema.MinimumLength
						}), schema);
                    }
                    if (schema.Patterns != null)
                    {
                        foreach (string current in schema.Patterns)
                        {
                            if (!Regex.IsMatch(text, current))
                            {
                                this.RaiseError("String '{0}' does not match regex pattern '{1}'.".FormatWith(CultureInfo.InvariantCulture, new object[]
								{
									text,
									current
								}), schema);
                            }
                        }
                    }
                }
            }
        }

        private void ValidateInteger(JsonSchemaModel schema)
        {
            if (schema != null)
            {
                if (this.TestType(schema, JsonSchemaType.Integer))
                {
                    this.ValidateInEnumAndNotDisallowed(schema);
                    long num = Convert.ToInt64(this._reader.Value, CultureInfo.InvariantCulture);
                    if (schema.Maximum.HasValue)
                    {
                        double num2 = (double)num;
                        double? num3 = schema.Maximum;
                        if (num2 > num3.GetValueOrDefault() && num3.HasValue)
                        {
                            this.RaiseError("Integer {0} exceeds maximum value of {1}.".FormatWith(CultureInfo.InvariantCulture, new object[]
							{
								num,
								schema.Maximum
							}), schema);
                        }
                        bool arg_E6_0;
                        if (schema.ExclusiveMaximum)
                        {
                            num2 = (double)num;
                            num3 = schema.Maximum;
                            arg_E6_0 = (num2 != num3.GetValueOrDefault() || !num3.HasValue);
                        }
                        else
                        {
                            arg_E6_0 = true;
                        }
                        if (!arg_E6_0)
                        {
                            this.RaiseError("Integer {0} equals maximum value of {1} and exclusive maximum is true.".FormatWith(CultureInfo.InvariantCulture, new object[]
							{
								num,
								schema.Maximum
							}), schema);
                        }
                    }
                    if (schema.Minimum.HasValue)
                    {
                        double num2 = (double)num;
                        double? num3 = schema.Minimum;
                        if (num2 < num3.GetValueOrDefault() && num3.HasValue)
                        {
                            this.RaiseError("Integer {0} is less than minimum value of {1}.".FormatWith(CultureInfo.InvariantCulture, new object[]
							{
								num,
								schema.Minimum
							}), schema);
                        }
                        bool arg_1CB_0;
                        if (schema.ExclusiveMinimum)
                        {
                            num2 = (double)num;
                            num3 = schema.Minimum;
                            arg_1CB_0 = (num2 != num3.GetValueOrDefault() || !num3.HasValue);
                        }
                        else
                        {
                            arg_1CB_0 = true;
                        }
                        if (!arg_1CB_0)
                        {
                            this.RaiseError("Integer {0} equals minimum value of {1} and exclusive minimum is true.".FormatWith(CultureInfo.InvariantCulture, new object[]
							{
								num,
								schema.Minimum
							}), schema);
                        }
                    }
                    if (schema.DivisibleBy.HasValue && !JsonValidatingReader.IsZero((double)num % schema.DivisibleBy.Value))
                    {
                        this.RaiseError("Integer {0} is not evenly divisible by {1}.".FormatWith(CultureInfo.InvariantCulture, new object[]
						{
							JsonConvert.ToString(num),
							schema.DivisibleBy
						}), schema);
                    }
                }
            }
        }

        private void ProcessValue()
        {
            if (this._currentScope != null && this._currentScope.TokenType == JTokenType.Array)
            {
                this._currentScope.ArrayItemCount++;
                foreach (JsonSchemaModel current in this.CurrentSchemas)
                {
                    if (current != null && current.Items != null && current.Items.Count > 1 && this._currentScope.ArrayItemCount >= current.Items.Count)
                    {
                        this.RaiseError("Index {0} has not been defined and the schema does not allow additional items.".FormatWith(CultureInfo.InvariantCulture, new object[]
						{
							this._currentScope.ArrayItemCount
						}), current);
                    }
                }
            }
        }

        private void ValidateFloat(JsonSchemaModel schema)
        {
            if (schema != null)
            {
                if (this.TestType(schema, JsonSchemaType.Float))
                {
                    this.ValidateInEnumAndNotDisallowed(schema);
                    double num = Convert.ToDouble(this._reader.Value, CultureInfo.InvariantCulture);
                    if (schema.Maximum.HasValue)
                    {
                        double num2 = num;
                        double? num3 = schema.Maximum;
                        if (num2 > num3.GetValueOrDefault() && num3.HasValue)
                        {
                            this.RaiseError("Float {0} exceeds maximum value of {1}.".FormatWith(CultureInfo.InvariantCulture, new object[]
							{
								JsonConvert.ToString(num),
								schema.Maximum
							}), schema);
                        }
                        bool arg_E4_0;
                        if (schema.ExclusiveMaximum)
                        {
                            num2 = num;
                            num3 = schema.Maximum;
                            arg_E4_0 = (num2 != num3.GetValueOrDefault() || !num3.HasValue);
                        }
                        else
                        {
                            arg_E4_0 = true;
                        }
                        if (!arg_E4_0)
                        {
                            this.RaiseError("Float {0} equals maximum value of {1} and exclusive maximum is true.".FormatWith(CultureInfo.InvariantCulture, new object[]
							{
								JsonConvert.ToString(num),
								schema.Maximum
							}), schema);
                        }
                    }
                    if (schema.Minimum.HasValue)
                    {
                        double num2 = num;
                        double? num3 = schema.Minimum;
                        if (num2 < num3.GetValueOrDefault() && num3.HasValue)
                        {
                            this.RaiseError("Float {0} is less than minimum value of {1}.".FormatWith(CultureInfo.InvariantCulture, new object[]
							{
								JsonConvert.ToString(num),
								schema.Minimum
							}), schema);
                        }
                        bool arg_1C7_0;
                        if (schema.ExclusiveMinimum)
                        {
                            num2 = num;
                            num3 = schema.Minimum;
                            arg_1C7_0 = (num2 != num3.GetValueOrDefault() || !num3.HasValue);
                        }
                        else
                        {
                            arg_1C7_0 = true;
                        }
                        if (!arg_1C7_0)
                        {
                            this.RaiseError("Float {0} equals minimum value of {1} and exclusive minimum is true.".FormatWith(CultureInfo.InvariantCulture, new object[]
							{
								JsonConvert.ToString(num),
								schema.Minimum
							}), schema);
                        }
                    }
                    if (schema.DivisibleBy.HasValue && !JsonValidatingReader.IsZero(num % schema.DivisibleBy.Value))
                    {
                        this.RaiseError("Float {0} is not evenly divisible by {1}.".FormatWith(CultureInfo.InvariantCulture, new object[]
						{
							JsonConvert.ToString(num),
							schema.DivisibleBy
						}), schema);
                    }
                }
            }
        }

        private static bool IsZero(double value)
        {
            double num = 2.2204460492503131E-16;
            return Math.Abs(value) < 10.0 * num;
        }

        private void ValidatePropertyName(JsonSchemaModel schema)
        {
            if (schema != null)
            {
                string text = Convert.ToString(this._reader.Value, CultureInfo.InvariantCulture);
                if (this._currentScope.RequiredProperties.ContainsKey(text))
                {
                    this._currentScope.RequiredProperties[text] = true;
                }
                if (!schema.AllowAdditionalProperties)
                {
                    if (!this.IsPropertyDefinied(schema, text))
                    {
                        this.RaiseError("Property '{0}' has not been defined and the schema does not allow additional properties.".FormatWith(CultureInfo.InvariantCulture, new object[]
						{
							text
						}), schema);
                    }
                }
                this._currentScope.CurrentPropertyName = text;
            }
        }

        private bool IsPropertyDefinied(JsonSchemaModel schema, string propertyName)
        {
            bool result;
            if (schema.Properties != null && schema.Properties.ContainsKey(propertyName))
            {
                result = true;
            }
            else
            {
                if (schema.PatternProperties != null)
                {
                    foreach (string current in schema.PatternProperties.Keys)
                    {
                        if (Regex.IsMatch(propertyName, current))
                        {
                            result = true;
                            return result;
                        }
                    }
                }
                result = false;
            }
            return result;
        }

        private bool ValidateArray(JsonSchemaModel schema)
        {
            return schema == null || this.TestType(schema, JsonSchemaType.Array);
        }

        private bool ValidateObject(JsonSchemaModel schema)
        {
            return schema == null || this.TestType(schema, JsonSchemaType.Object);
        }

        private bool TestType(JsonSchemaModel currentSchema, JsonSchemaType currentType)
        {
            bool result;
            if (!JsonSchemaGenerator.HasFlag(new JsonSchemaType?(currentSchema.Type), currentType))
            {
                this.RaiseError("Invalid type. Expected {0} but got {1}.".FormatWith(CultureInfo.InvariantCulture, new object[]
				{
					currentSchema.Type,
					currentType
				}), currentSchema);
                result = false;
            }
            else
            {
                result = true;
            }
            return result;
        }

        bool IJsonLineInfo.HasLineInfo()
        {
            IJsonLineInfo jsonLineInfo = this._reader as IJsonLineInfo;
            return jsonLineInfo != null && jsonLineInfo.HasLineInfo();
        }
    }
}
