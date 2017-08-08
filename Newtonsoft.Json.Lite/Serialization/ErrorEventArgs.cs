namespace Newtonsoft.Json.Serialization
{
    using System;
    using System.Runtime.CompilerServices;

    public class ErrorEventArgs : EventArgs
    {
        public ErrorEventArgs(object currentObject, Newtonsoft.Json.Serialization.ErrorContext errorContext)
        {
            this.CurrentObject = currentObject;
            this.ErrorContext = errorContext;
        }

        public object CurrentObject { get; private set; }

        public Newtonsoft.Json.Serialization.ErrorContext ErrorContext { get; private set; }
    }
}

