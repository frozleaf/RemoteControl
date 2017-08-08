namespace Newtonsoft.Json.Linq
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Reflection;

    public interface IJEnumerable<T> : IEnumerable<T>, IEnumerable where T: JToken
    {
        IJEnumerable<JToken> this[object key] { get; }
    }
}

