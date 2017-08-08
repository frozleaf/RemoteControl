namespace Newtonsoft.Json
{
    using System;

    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Constructor, AllowMultiple=false)]
    public sealed class JsonConstructorAttribute : Attribute
    {
    }
}

