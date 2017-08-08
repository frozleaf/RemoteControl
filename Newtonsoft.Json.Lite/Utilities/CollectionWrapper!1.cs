namespace Newtonsoft.Json.Utilities
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Threading;

    internal class CollectionWrapper<T> : ICollection<T>, IEnumerable<T>, IWrappedCollection, IList, ICollection, IEnumerable
    {
        private readonly ICollection<T> _genericCollection;
        private readonly IList _list;
        private object _syncRoot;

        public CollectionWrapper(ICollection<T> list)
        {
            ValidationUtils.ArgumentNotNull(list, "list");
            this._genericCollection = list;
        }

        public CollectionWrapper(IList list)
        {
            ValidationUtils.ArgumentNotNull(list, "list");
            if (list is ICollection<T>)
            {
                this._genericCollection = (ICollection<T>) list;
            }
            else
            {
                this._list = list;
            }
        }

        public virtual void Add(T item)
        {
            if (this._genericCollection != null)
            {
                this._genericCollection.Add(item);
            }
            else
            {
                this._list.Add(item);
            }
        }

        public virtual void Clear()
        {
            if (this._genericCollection != null)
            {
                this._genericCollection.Clear();
            }
            else
            {
                this._list.Clear();
            }
        }

        public virtual bool Contains(T item)
        {
            if (this._genericCollection != null)
            {
                return this._genericCollection.Contains(item);
            }
            return this._list.Contains(item);
        }

        public virtual void CopyTo(T[] array, int arrayIndex)
        {
            if (this._genericCollection != null)
            {
                this._genericCollection.CopyTo(array, arrayIndex);
            }
            else
            {
                this._list.CopyTo(array, arrayIndex);
            }
        }

        public virtual IEnumerator<T> GetEnumerator()
        {
            if (this._genericCollection != null)
            {
                return this._genericCollection.GetEnumerator();
            }
            return this._list.Cast<T>().GetEnumerator();
        }

        private static bool IsCompatibleObject(object value)
        {
            if (!((value is T) || ((value == null) && (!typeof(T).IsValueType || ReflectionUtils.IsNullableType(typeof(T))))))
            {
                return false;
            }
            return true;
        }

        public virtual bool Remove(T item)
        {
            if (this._genericCollection != null)
            {
                return this._genericCollection.Remove(item);
            }
            bool flag = this._list.Contains(item);
            if (flag)
            {
                this._list.Remove(item);
            }
            return flag;
        }

        void ICollection.CopyTo(Array array, int arrayIndex)
        {
            this.CopyTo((T[]) array, arrayIndex);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            if (this._genericCollection != null)
            {
                return this._genericCollection.GetEnumerator();
            }
            return this._list.GetEnumerator();
        }

        int IList.Add(object value)
        {
            CollectionWrapper<T>.VerifyValueType(value);
            this.Add((T) value);
            return (this.Count - 1);
        }

        bool IList.Contains(object value)
        {
            return (CollectionWrapper<T>.IsCompatibleObject(value) && this.Contains((T) value));
        }

        int IList.IndexOf(object value)
        {
            if (this._genericCollection != null)
            {
                throw new Exception("Wrapped ICollection<T> does not support IndexOf.");
            }
            if (CollectionWrapper<T>.IsCompatibleObject(value))
            {
                return this._list.IndexOf((T) value);
            }
            return -1;
        }

        void IList.Insert(int index, object value)
        {
            if (this._genericCollection != null)
            {
                throw new Exception("Wrapped ICollection<T> does not support Insert.");
            }
            CollectionWrapper<T>.VerifyValueType(value);
            this._list.Insert(index, (T) value);
        }

        void IList.Remove(object value)
        {
            if (CollectionWrapper<T>.IsCompatibleObject(value))
            {
                this.Remove((T) value);
            }
        }

        void IList.RemoveAt(int index)
        {
            if (this._genericCollection != null)
            {
                throw new Exception("Wrapped ICollection<T> does not support RemoveAt.");
            }
            this._list.RemoveAt(index);
        }

        private static void VerifyValueType(object value)
        {
            if (!CollectionWrapper<T>.IsCompatibleObject(value))
            {
                throw new ArgumentException("The value '{0}' is not of type '{1}' and cannot be used in this generic collection.".FormatWith(CultureInfo.InvariantCulture, new object[] { value, typeof(T) }), "value");
            }
        }

        public virtual int Count
        {
            get
            {
                if (this._genericCollection != null)
                {
                    return this._genericCollection.Count;
                }
                return this._list.Count;
            }
        }

        public virtual bool IsReadOnly
        {
            get
            {
                if (this._genericCollection != null)
                {
                    return this._genericCollection.IsReadOnly;
                }
                return this._list.IsReadOnly;
            }
        }

        bool ICollection.IsSynchronized
        {
            get
            {
                return false;
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

        bool IList.IsFixedSize
        {
            get
            {
                if (this._genericCollection != null)
                {
                    return this._genericCollection.IsReadOnly;
                }
                return this._list.IsFixedSize;
            }
        }

        object IList.this[int index]
        {
            get
            {
                if (this._genericCollection != null)
                {
                    throw new Exception("Wrapped ICollection<T> does not support indexer.");
                }
                return this._list[index];
            }
            set
            {
                if (this._genericCollection != null)
                {
                    throw new Exception("Wrapped ICollection<T> does not support indexer.");
                }
                CollectionWrapper<T>.VerifyValueType(value);
                this._list[index] = (T) value;
            }
        }

        public object UnderlyingCollection
        {
            get
            {
                if (this._genericCollection != null)
                {
                    return this._genericCollection;
                }
                return this._list;
            }
        }
    }
}

