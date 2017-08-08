namespace Newtonsoft.Json.Utilities
{
    using System;
    using System.Collections;

    internal interface IWrappedCollection : IList, ICollection, IEnumerable
    {
        object UnderlyingCollection { get; }
    }
}

