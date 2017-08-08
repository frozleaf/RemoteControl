namespace Newtonsoft.Json
{
    using System;

    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple=false)]
    public sealed class JsonIgnoreAttribute : Attribute
    {
    }
}

