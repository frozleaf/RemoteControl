namespace Newtonsoft.Json.Serialization
{
    using System;
    using System.Runtime.CompilerServices;

    public class ErrorContext
    {
        internal ErrorContext(object originalObject, object member, Exception error)
        {
            this.OriginalObject = originalObject;
            this.Member = member;
            this.Error = error;
        }

        public Exception Error { get; private set; }

        public bool Handled { get; set; }

        public object Member { get; private set; }

        public object OriginalObject { get; private set; }
    }
}

