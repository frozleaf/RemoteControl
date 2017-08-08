namespace Newtonsoft.Json
{
    using System;
    using System.Runtime.CompilerServices;

    [Serializable]
    public class JsonReaderException : Exception
    {
        public JsonReaderException()
        {
        }

        public JsonReaderException(string message) : base(message)
        {
        }

        public JsonReaderException(string message, Exception innerException) : base(message, innerException)
        {
        }

        internal JsonReaderException(string message, Exception innerException, int lineNumber, int linePosition) : base(message, innerException)
        {
            this.LineNumber = lineNumber;
            this.LinePosition = linePosition;
        }

        public int LineNumber { get; private set; }

        public int LinePosition { get; private set; }
    }
}

