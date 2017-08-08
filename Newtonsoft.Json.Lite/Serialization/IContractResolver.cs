namespace Newtonsoft.Json.Serialization
{
    using System;

    public interface IContractResolver
    {
        JsonContract ResolveContract(Type type);
    }
}

