namespace Newtonsoft.Json
{
    using System;

    [AttributeUsage(AttributeTargets.Interface | AttributeTargets.Struct | AttributeTargets.Class, AllowMultiple=false)]
    public sealed class JsonObjectAttribute : JsonContainerAttribute
    {
        private Newtonsoft.Json.MemberSerialization _memberSerialization;

        public JsonObjectAttribute()
        {
            this._memberSerialization = Newtonsoft.Json.MemberSerialization.OptOut;
        }

        public JsonObjectAttribute(Newtonsoft.Json.MemberSerialization memberSerialization)
        {
            this._memberSerialization = Newtonsoft.Json.MemberSerialization.OptOut;
            this.MemberSerialization = memberSerialization;
        }

        public JsonObjectAttribute(string id) : base(id)
        {
            this._memberSerialization = Newtonsoft.Json.MemberSerialization.OptOut;
        }

        public Newtonsoft.Json.MemberSerialization MemberSerialization
        {
            get
            {
                return this._memberSerialization;
            }
            set
            {
                this._memberSerialization = value;
            }
        }
    }
}

