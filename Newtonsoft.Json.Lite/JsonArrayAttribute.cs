namespace Newtonsoft.Json
{
    using System;

    [AttributeUsage(AttributeTargets.Interface | AttributeTargets.Class, AllowMultiple=false)]
    public sealed class JsonArrayAttribute : JsonContainerAttribute
    {
        private bool _allowNullItems;

        public JsonArrayAttribute()
        {
        }

        public JsonArrayAttribute(bool allowNullItems)
        {
            this._allowNullItems = allowNullItems;
        }

        public JsonArrayAttribute(string id) : base(id)
        {
        }

        public bool AllowNullItems
        {
            get
            {
                return this._allowNullItems;
            }
            set
            {
                this._allowNullItems = value;
            }
        }
    }
}

