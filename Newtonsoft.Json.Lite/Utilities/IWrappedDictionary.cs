namespace Newtonsoft.Json.Utilities
{
    using System;
    using System.Collections;

    internal interface IWrappedDictionary : IDictionary, ICollection, IEnumerable
    {
        object UnderlyingDictionary { get; }
    }
}

