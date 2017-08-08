namespace Newtonsoft.Json.Schema
{
    using System;

    [Flags]
    public enum JsonSchemaType
    {
        Any = 0x7f,
        Array = 0x20,
        Boolean = 8,
        Float = 2,
        Integer = 4,
        None = 0,
        Null = 0x40,
        Object = 0x10,
        String = 1
    }
}

