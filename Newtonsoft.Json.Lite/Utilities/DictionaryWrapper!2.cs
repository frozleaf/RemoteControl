using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace Newtonsoft.Json.Utilities
{
    internal class DictionaryWrapper<TKey, TValue> : IDictionary<TKey, TValue>, ICollection<KeyValuePair<TKey, TValue>>, IEnumerable<KeyValuePair<TKey, TValue>>, IWrappedDictionary, IDictionary, ICollection, IEnumerable
    {
        private struct DictionaryEnumerator<TEnumeratorKey, TEnumeratorValue> : IDictionaryEnumerator, IEnumerator
        {
            private readonly IEnumerator<KeyValuePair<TEnumeratorKey, TEnumeratorValue>> _e;

            public DictionaryEntry Entry
            {
                get
                {
                    return (DictionaryEntry)this.Current;
                }
            }

            public object Key
            {
                get
                {
                    return this.Entry.Key;
                }
            }

            public object Value
            {
                get
                {
                    return this.Entry.Value;
                }
            }

            public object Current
            {
                get
                {
                    KeyValuePair<TEnumeratorKey, TEnumeratorValue> current = this._e.Current;
                    object arg_31_0 = current.Key;
                    current = this._e.Current;
                    return new DictionaryEntry(arg_31_0, current.Value);
                }
            }

            public DictionaryEnumerator(IEnumerator<KeyValuePair<TEnumeratorKey, TEnumeratorValue>> e)
            {
                ValidationUtils.ArgumentNotNull(e, "e");
                this._e = e;
            }

            public bool MoveNext()
            {
                return this._e.MoveNext();
            }

            public void Reset()
            {
                this._e.Reset();
            }
        }

        private readonly IDictionary _dictionary;

        private readonly IDictionary<TKey, TValue> _genericDictionary;

        private object _syncRoot;

        public ICollection<TKey> Keys
        {
            get
            {
                ICollection<TKey> result;
                if (this._genericDictionary != null)
                {
                    result = this._genericDictionary.Keys;
                }
                else
                {
                    result = this._dictionary.Keys.Cast<TKey>().ToList<TKey>();
                }
                return result;
            }
        }

        public ICollection<TValue> Values
        {
            get
            {
                ICollection<TValue> result;
                if (this._genericDictionary != null)
                {
                    result = this._genericDictionary.Values;
                }
                else
                {
                    result = this._dictionary.Values.Cast<TValue>().ToList<TValue>();
                }
                return result;
            }
        }

        public TValue this[TKey key]
        {
            get
            {
                TValue result;
                if (this._genericDictionary != null)
                {
                    result = this._genericDictionary[key];
                }
                else
                {
                    result = (TValue)((object)this._dictionary[key]);
                }
                return result;
            }
            set
            {
                if (this._genericDictionary != null)
                {
                    this._genericDictionary[key] = value;
                }
                else
                {
                    this._dictionary[key] = value;
                }
            }
        }

        public int Count
        {
            get
            {
                int count;
                if (this._genericDictionary != null)
                {
                    count = this._genericDictionary.Count;
                }
                else
                {
                    count = this._dictionary.Count;
                }
                return count;
            }
        }

        public bool IsReadOnly
        {
            get
            {
                bool isReadOnly;
                if (this._genericDictionary != null)
                {
                    isReadOnly = this._genericDictionary.IsReadOnly;
                }
                else
                {
                    isReadOnly = this._dictionary.IsReadOnly;
                }
                return isReadOnly;
            }
        }

        bool IDictionary.IsFixedSize
        {
            get
            {
                return this._genericDictionary == null && this._dictionary.IsFixedSize;
            }
        }

        ICollection IDictionary.Keys
        {
            get
            {
                ICollection result;
                if (this._genericDictionary != null)
                {
                    result = this._genericDictionary.Keys.ToList<TKey>();
                }
                else
                {
                    result = this._dictionary.Keys;
                }
                return result;
            }
        }

        ICollection IDictionary.Values
        {
            get
            {
                ICollection result;
                if (this._genericDictionary != null)
                {
                    result = this._genericDictionary.Values.ToList<TValue>();
                }
                else
                {
                    result = this._dictionary.Values;
                }
                return result;
            }
        }

        object IDictionary.this[object key]
        {
            get
            {
                object result;
                if (this._genericDictionary != null)
                {
                    result = this._genericDictionary[(TKey)((object)key)];
                }
                else
                {
                    result = this._dictionary[key];
                }
                return result;
            }
            set
            {
                if (this._genericDictionary != null)
                {
                    this._genericDictionary[(TKey)((object)key)] = (TValue)((object)value);
                }
                else
                {
                    this._dictionary[key] = value;
                }
            }
        }

        bool ICollection.IsSynchronized
        {
            get
            {
                return this._genericDictionary == null && this._dictionary.IsSynchronized;
            }
        }

        object ICollection.SyncRoot
        {
            get
            {
                if (this._syncRoot == null)
                {
                    Interlocked.CompareExchange(ref this._syncRoot, new object(), null);
                }
                return this._syncRoot;
            }
        }

        public object UnderlyingDictionary
        {
            get
            {
                object result;
                if (this._genericDictionary != null)
                {
                    result = this._genericDictionary;
                }
                else
                {
                    result = this._dictionary;
                }
                return result;
            }
        }

        public DictionaryWrapper(IDictionary dictionary)
        {
            ValidationUtils.ArgumentNotNull(dictionary, "dictionary");
            this._dictionary = dictionary;
        }

        public DictionaryWrapper(IDictionary<TKey, TValue> dictionary)
        {
            ValidationUtils.ArgumentNotNull(dictionary, "dictionary");
            this._genericDictionary = dictionary;
        }

        public void Add(TKey key, TValue value)
        {
            if (this._genericDictionary != null)
            {
                this._genericDictionary.Add(key, value);
            }
            else
            {
                this._dictionary.Add(key, value);
            }
        }

        public bool ContainsKey(TKey key)
        {
            bool result;
            if (this._genericDictionary != null)
            {
                result = this._genericDictionary.ContainsKey(key);
            }
            else
            {
                result = this._dictionary.Contains(key);
            }
            return result;
        }

        public bool Remove(TKey key)
        {
            bool result;
            if (this._genericDictionary != null)
            {
                result = this._genericDictionary.Remove(key);
            }
            else if (this._dictionary.Contains(key))
            {
                this._dictionary.Remove(key);
                result = true;
            }
            else
            {
                result = false;
            }
            return result;
        }

        public bool TryGetValue(TKey key, out TValue value)
        {
            bool result;
            if (this._genericDictionary != null)
            {
                result = this._genericDictionary.TryGetValue(key, out value);
            }
            else if (!this._dictionary.Contains(key))
            {
                value = default(TValue);
                result = false;
            }
            else
            {
                value = (TValue)((object)this._dictionary[key]);
                result = true;
            }
            return result;
        }

        public void Add(KeyValuePair<TKey, TValue> item)
        {
            if (this._genericDictionary != null)
            {
                this._genericDictionary.Add(item);
            }
            else
            {
                ((IList)this._dictionary).Add(item);
            }
        }

        public void Clear()
        {
            if (this._genericDictionary != null)
            {
                this._genericDictionary.Clear();
            }
            else
            {
                this._dictionary.Clear();
            }
        }

        public bool Contains(KeyValuePair<TKey, TValue> item)
        {
            bool result;
            if (this._genericDictionary != null)
            {
                result = this._genericDictionary.Contains(item);
            }
            else
            {
                result = ((IList)this._dictionary).Contains(item);
            }
            return result;
        }

        public void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex)
        {
            if (this._genericDictionary != null)
            {
                this._genericDictionary.CopyTo(array, arrayIndex);
            }
            else
            {
                foreach (DictionaryEntry dictionaryEntry in this._dictionary)
                {
                    array[arrayIndex++] = new KeyValuePair<TKey, TValue>((TKey)((object)dictionaryEntry.Key), (TValue)((object)dictionaryEntry.Value));
                }
            }
        }

        public bool Remove(KeyValuePair<TKey, TValue> item)
        {
            bool result;
            if (this._genericDictionary != null)
            {
                result = this._genericDictionary.Remove(item);
            }
            else if (this._dictionary.Contains(item.Key))
            {
                object objA = this._dictionary[item.Key];
                if (object.Equals(objA, item.Value))
                {
                    this._dictionary.Remove(item.Key);
                    result = true;
                }
                else
                {
                    result = false;
                }
            }
            else
            {
                result = true;
            }
            return result;
        }

        public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
        {
            IEnumerator<KeyValuePair<TKey, TValue>> enumerator;
            if (this._genericDictionary != null)
            {
                enumerator = this._genericDictionary.GetEnumerator();
            }
            else
            {
                enumerator = (from DictionaryEntry de in this._dictionary
                              select new KeyValuePair<TKey, TValue>((TKey)((object)de.Key), (TValue)((object)de.Value))).GetEnumerator();
            }
            return enumerator;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        void IDictionary.Add(object key, object value)
        {
            if (this._genericDictionary != null)
            {
                this._genericDictionary.Add((TKey)((object)key), (TValue)((object)value));
            }
            else
            {
                this._dictionary.Add(key, value);
            }
        }

        bool IDictionary.Contains(object key)
        {
            bool result;
            if (this._genericDictionary != null)
            {
                result = this._genericDictionary.ContainsKey((TKey)((object)key));
            }
            else
            {
                result = this._dictionary.Contains(key);
            }
            return result;
        }

        IDictionaryEnumerator IDictionary.GetEnumerator()
        {
            IDictionaryEnumerator result;
            if (this._genericDictionary != null)
            {
                result = new DictionaryWrapper<TKey, TValue>.DictionaryEnumerator<TKey, TValue>(this._genericDictionary.GetEnumerator());
            }
            else
            {
                result = this._dictionary.GetEnumerator();
            }
            return result;
        }

        public void Remove(object key)
        {
            if (this._genericDictionary != null)
            {
                this._genericDictionary.Remove((TKey)((object)key));
            }
            else
            {
                this._dictionary.Remove(key);
            }
        }

        void ICollection.CopyTo(Array array, int index)
        {
            if (this._genericDictionary != null)
            {
                this._genericDictionary.CopyTo((KeyValuePair<TKey, TValue>[])array, index);
            }
            else
            {
                this._dictionary.CopyTo(array, index);
            }
        }
    }
}
