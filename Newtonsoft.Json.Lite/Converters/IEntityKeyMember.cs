namespace Newtonsoft.Json.Converters
{
    using System;

    internal interface IEntityKeyMember
    {
        string Key { get; set; }

        object Value { get; set; }
    }
}

