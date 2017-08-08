namespace Newtonsoft.Json.Schema
{
    using System;
    using System.Runtime.CompilerServices;

    [Serializable]
    public class JsonSchemaException : Exception
    {
        public JsonSchemaException()
        {
        }

        public JsonSchemaException(string message) : base(message)
        {
        }

        public JsonSchemaException(string message, Exception innerException) : base(message, innerException)
        {
        }

        internal JsonSchemaException(string message, Exception innerException, int lineNumber, int linePosition) : base(message, innerException)
        {
            this.LineNumber = lineNumber;
            this.LinePosition = linePosition;
        }

        public int LineNumber { get; private set; }

        public int LinePosition { get; private set; }
    }
}

