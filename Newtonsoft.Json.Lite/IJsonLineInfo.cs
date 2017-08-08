namespace Newtonsoft.Json
{
    using System;

    public interface IJsonLineInfo
    {
        bool HasLineInfo();

        int LineNumber { get; }

        int LinePosition { get; }
    }
}

