namespace Newtonsoft.Json
{
    using System;

    [Serializable]
    public class JsonSerializationException : Exception
    {
        public JsonSerializationException()
        {
        }

        public JsonSerializationException(string message) : base(message)
        {
        }

        public JsonSerializationException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}

