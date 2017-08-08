namespace Newtonsoft.Json
{
    using System;

    public enum WriteState
    {
        Error,
        Closed,
        Object,
        Array,
        Constructor,
        Property,
        Start
    }
}

