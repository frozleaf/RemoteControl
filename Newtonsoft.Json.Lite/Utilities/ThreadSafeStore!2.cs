namespace Newtonsoft.Json.Utilities
{
    using System;
    using System.Collections.Generic;

    internal class ThreadSafeStore<TKey, TValue>
    {
        private readonly Func<TKey, TValue> _creator;
        private readonly object _lock;
        private Dictionary<TKey, TValue> _store;

        public ThreadSafeStore(Func<TKey, TValue> creator)
        {
            this._lock = new object();
            if (creator == null)
            {
                throw new ArgumentNullException("creator");
            }
            this._creator = creator;
        }

        private TValue AddValue(TKey key)
        {
            TValue local = this._creator(key);
            lock (this._lock)
            {
                if (this._store == null)
                {
                    this._store = new Dictionary<TKey, TValue>();
                    this._store[key] = local;
                }
                else
                {
                    TValue local2;
                    if (this._store.TryGetValue(key, out local2))
                    {
                        return local2;
                    }
                    Dictionary<TKey, TValue> dictionary = new Dictionary<TKey, TValue>(this._store);
                    dictionary[key] = local;
                    this._store = dictionary;
                }
                return local;
            }
        }

        public TValue Get(TKey key)
        {
            TValue local;
            if (this._store == null)
            {
                return this.AddValue(key);
            }
            if (!this._store.TryGetValue(key, out local))
            {
                return this.AddValue(key);
            }
            return local;
        }
    }
}

