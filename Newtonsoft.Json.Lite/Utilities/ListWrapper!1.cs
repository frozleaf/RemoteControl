namespace Newtonsoft.Json.Utilities
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Reflection;

    internal class ListWrapper<T> : CollectionWrapper<T>, IList<T>, ICollection<T>, IEnumerable<T>, IWrappedList, IList, ICollection, IEnumerable
    {
        private readonly IList<T> _genericList;

        public ListWrapper(IList<T> list) : base(list)
        {
            ValidationUtils.ArgumentNotNull(list, "list");
            this._genericList = list;
        }

        public ListWrapper(IList list) : base(list)
        {
            ValidationUtils.ArgumentNotNull(list, "list");
            if (list is IList<T>)
            {
                this._genericList = (IList<T>) list;
            }
        }

        public override void Add(T item)
        {
            if (this._genericList != null)
            {
                this._genericList.Add(item);
            }
            else
            {
                base.Add(item);
            }
        }

        public override void Clear()
        {
            if (this._genericList != null)
            {
                this._genericList.Clear();
            }
            else
            {
                base.Clear();
            }
        }

        public override bool Contains(T item)
        {
            if (this._genericList != null)
            {
                return this._genericList.Contains(item);
            }
            return base.Contains(item);
        }

        public override void CopyTo(T[] array, int arrayIndex)
        {
            if (this._genericList != null)
            {
                this._genericList.CopyTo(array, arrayIndex);
            }
            else
            {
                base.CopyTo(array, arrayIndex);
            }
        }

        public override IEnumerator<T> GetEnumerator()
        {
            if (this._genericList != null)
            {
                return this._genericList.GetEnumerator();
            }
            return base.GetEnumerator();
        }

        public int IndexOf(T item)
        {
            if (this._genericList != null)
            {
                return this._genericList.IndexOf(item);
            }
            return this.IndexOf(item);
        }

        public void Insert(int index, T item)
        {
            if (this._genericList != null)
            {
                this._genericList.Insert(index, item);
            }
            else
            {
                this.Insert(index, item);
            }
        }

        public override bool Remove(T item)
        {
            if (this._genericList != null)
            {
                return this._genericList.Remove(item);
            }
            bool flag = base.Contains(item);
            if (flag)
            {
                base.Remove(item);
            }
            return flag;
        }

        public void RemoveAt(int index)
        {
            if (this._genericList != null)
            {
                this._genericList.RemoveAt(index);
            }
            else
            {
                this.RemoveAt(index);
            }
        }

        public override int Count
        {
            get
            {
                if (this._genericList != null)
                {
                    return this._genericList.Count;
                }
                return base.Count;
            }
        }

        public override bool IsReadOnly
        {
            get
            {
                if (this._genericList != null)
                {
                    return this._genericList.IsReadOnly;
                }
                return base.IsReadOnly;
            }
        }

        public T this[int index]
        {
            get
            {
                if (this._genericList != null)
                {
                    return this._genericList[index];
                }
                return (T) this[index];
            }
            set
            {
                if (this._genericList != null)
                {
                    this._genericList[index] = value;
                }
                else
                {
                    this[index] = value;
                }
            }
        }

        public object UnderlyingList
        {
            get
            {
                if (this._genericList != null)
                {
                    return this._genericList;
                }
                return base.UnderlyingCollection;
            }
        }
    }
}

