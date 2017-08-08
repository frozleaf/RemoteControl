namespace Newtonsoft.Json.Utilities
{
    using System;
    using System.Collections;

    internal interface IWrappedList : IList, ICollection, IEnumerable
    {
        object UnderlyingList { get; }
    }
}

