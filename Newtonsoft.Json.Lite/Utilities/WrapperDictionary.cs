namespace Newtonsoft.Json.Utilities
{
    using System;
    using System.Collections.Generic;

    internal class WrapperDictionary
    {
        private readonly Dictionary<string, Type> _wrapperTypes = new Dictionary<string, Type>();

        private static string GenerateKey(Type interfaceType, Type realObjectType)
        {
            return (interfaceType.Name + "_" + realObjectType.Name);
        }

        public Type GetType(Type interfaceType, Type realObjectType)
        {
            string key = GenerateKey(interfaceType, realObjectType);
            if (this._wrapperTypes.ContainsKey(key))
            {
                return this._wrapperTypes[key];
            }
            return null;
        }

        public void SetType(Type interfaceType, Type realObjectType, Type wrapperType)
        {
            string key = GenerateKey(interfaceType, realObjectType);
            if (this._wrapperTypes.ContainsKey(key))
            {
                this._wrapperTypes[key] = wrapperType;
            }
            else
            {
                this._wrapperTypes.Add(key, wrapperType);
            }
        }
    }
}

