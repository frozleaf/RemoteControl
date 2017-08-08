namespace Newtonsoft.Json.Serialization
{
    using System;

    internal interface IMetadataTypeAttribute
    {
        Type MetadataClassType { get; }
    }
}

