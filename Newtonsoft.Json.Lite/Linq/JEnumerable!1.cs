namespace Newtonsoft.Json.Linq
{
    using Newtonsoft.Json.Utilities;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using System.Runtime.InteropServices;

    [StructLayout(LayoutKind.Sequential)]
    public struct JEnumerable<T> : IJEnumerable<T>, IEnumerable<T>, IEnumerable where T: JToken
    {
        public static readonly JEnumerable<T> Empty;
        private IEnumerable<T> _enumerable;
        public JEnumerable(IEnumerable<T> enumerable)
        {
            ValidationUtils.ArgumentNotNull(enumerable, "enumerable");
            this._enumerable = enumerable;
        }

        public IEnumerator<T> GetEnumerator()
        {
            return this._enumerable.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        public IJEnumerable<JToken> this[object key]
        {
            get
            {
                return new JEnumerable<JToken>(this._enumerable.Values<T, JToken>(key));
            }
        }
        public override bool Equals(object obj)
        {
            return ((obj is JEnumerable<T>) && this._enumerable.Equals(((JEnumerable<T>) obj)._enumerable));
        }

        public override int GetHashCode()
        {
            return this._enumerable.GetHashCode();
        }

        static JEnumerable()
        {
            JEnumerable<T>.Empty = new JEnumerable<T>(Enumerable.Empty<T>());
        }
    }
}

