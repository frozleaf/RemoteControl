namespace Newtonsoft.Json.Utilities
{
    using System;
    using System.Collections.ObjectModel;

    internal class EnumValues<T> : KeyedCollection<string, EnumValue<T>> where T: struct
    {
        protected override string GetKeyForItem(EnumValue<T> item)
        {
            return item.Name;
        }
    }
}

