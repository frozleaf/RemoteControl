namespace Newtonsoft.Json
{
    using System;
    using System.Runtime.CompilerServices;

    [AttributeUsage(AttributeTargets.Interface | AttributeTargets.Class, AllowMultiple=false)]
    public abstract class JsonContainerAttribute : Attribute
    {
        internal bool? _isReference;

        protected JsonContainerAttribute()
        {
        }

        protected JsonContainerAttribute(string id)
        {
            this.Id = id;
        }

        public string Description { get; set; }

        public string Id { get; set; }

        public bool IsReference
        {
            get
            {
                bool? nullable = this._isReference;
                return (nullable.HasValue ? nullable.GetValueOrDefault() : false);
            }
            set
            {
                this._isReference = new bool?(value);
            }
        }

        public string Title { get; set; }
    }
}

