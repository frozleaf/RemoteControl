namespace Newtonsoft.Json.Schema
{
    using Newtonsoft.Json.Utilities;
    using System;

    public class ValidationEventArgs : EventArgs
    {
        private readonly JsonSchemaException _ex;

        internal ValidationEventArgs(JsonSchemaException ex)
        {
            ValidationUtils.ArgumentNotNull(ex, "ex");
            this._ex = ex;
        }

        public JsonSchemaException Exception
        {
            get
            {
                return this._ex;
            }
        }

        public string Message
        {
            get
            {
                return this._ex.Message;
            }
        }
    }
}

