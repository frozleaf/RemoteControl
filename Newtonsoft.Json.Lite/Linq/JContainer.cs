namespace Newtonsoft.Json.Linq
{
    using Newtonsoft.Json;
    using Newtonsoft.Json.Utilities;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Globalization;
    using System.Linq;
    using System.Runtime.CompilerServices;
    using System.Threading;

    public abstract class JContainer : JToken, IList<JToken>, ICollection<JToken>, IEnumerable<JToken>, ITypedList, IBindingList, IList, ICollection, IEnumerable
    {
        private bool _busy;
        private object _syncRoot;

        public event AddingNewEventHandler AddingNew;

        public event ListChangedEventHandler ListChanged;

        internal JContainer()
        {
        }

        internal JContainer(JContainer other)
        {
            ValidationUtils.ArgumentNotNull(other, "c");
            foreach (JToken token in (IEnumerable<JToken>) other)
            {
                this.Add(token);
            }
        }

        public virtual void Add(object content)
        {
            this.AddInternal(this.ChildrenTokens.Count, content);
        }

        public void AddFirst(object content)
        {
            this.AddInternal(0, content);
        }

        internal void AddInternal(int index, object content)
        {
            if (this.IsMultiContent(content))
            {
                IEnumerable enumerable = (IEnumerable) content;
                int num = index;
                foreach (object obj2 in enumerable)
                {
                    this.AddInternal(num, obj2);
                    num++;
                }
            }
            else
            {
                JToken item = this.CreateFromContent(content);
                this.InsertItem(index, item);
            }
        }

        internal void CheckReentrancy()
        {
            if (this._busy)
            {
                throw new InvalidOperationException("Cannot change {0} during a collection change event.".FormatWith(CultureInfo.InvariantCulture, new object[] { base.GetType() }));
            }
        }

        public override JEnumerable<JToken> Children()
        {
            return new JEnumerable<JToken>(this.ChildrenTokens);
        }

        internal virtual void ClearItems()
        {
            this.CheckReentrancy();
            foreach (JToken token in this.ChildrenTokens)
            {
                token.Parent = null;
                token.Previous = null;
                token.Next = null;
            }
            this.ChildrenTokens.Clear();
            this.OnListChanged(new ListChangedEventArgs(ListChangedType.Reset, -1));
        }

        internal virtual bool ContainsItem(JToken item)
        {
            return (this.IndexOfItem(item) != -1);
        }

        internal bool ContentsEqual(JContainer container)
        {
            JToken first = this.First;
            JToken node = container.First;
            if (first != node)
            {
                while ((first != null) || (node != null))
                {
                    if (((first != null) && (node != null)) && first.DeepEquals(node))
                    {
                        first = (first != this.Last) ? first.Next : null;
                        node = (node != container.Last) ? node.Next : null;
                    }
                    else
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        internal int ContentsHashCode()
        {
            int num = 0;
            foreach (JToken token in this.ChildrenTokens)
            {
                num ^= token.GetDeepHashCode();
            }
            return num;
        }

        internal virtual void CopyItemsTo(Array array, int arrayIndex)
        {
            if (array == null)
            {
                throw new ArgumentNullException("array");
            }
            if (arrayIndex < 0)
            {
                throw new ArgumentOutOfRangeException("arrayIndex", "arrayIndex is less than 0.");
            }
            if (arrayIndex >= array.Length)
            {
                throw new ArgumentException("arrayIndex is equal to or greater than the length of array.");
            }
            if (this.Count > (array.Length - arrayIndex))
            {
                throw new ArgumentException("The number of elements in the source JObject is greater than the available space from arrayIndex to the end of the destination array.");
            }
            int num = 0;
            foreach (JToken token in this.ChildrenTokens)
            {
                array.SetValue(token, (int) (arrayIndex + num));
                num++;
            }
        }

        internal JToken CreateFromContent(object content)
        {
            if (content is JToken)
            {
                return (JToken) content;
            }
            return new JValue(content);
        }

        public JsonWriter CreateWriter()
        {
            return new JTokenWriter(this);
        }

        public IEnumerable<JToken> Descendants()
        {
            foreach (JToken iteratorVariable0 in this.ChildrenTokens)
            {
                yield return iteratorVariable0;
                JContainer iteratorVariable1 = iteratorVariable0 as JContainer;
                if (iteratorVariable1 != null)
                {
                    foreach (JToken iteratorVariable2 in iteratorVariable1.Descendants())
                    {
                        yield return iteratorVariable2;
                    }
                }
            }
        }

        internal JToken EnsureParentToken(JToken item)
        {
            if (item == null)
            {
                return new JValue((JValue)null);
            }
            if (item.Parent != null)
            {
                item = item.CloneToken();
            }
            else
            {
                JContainer parent = this;
                while (parent.Parent != null)
                {
                    parent = parent.Parent;
                }
                if (item == parent)
                {
                    item = item.CloneToken();
                }
            }
            return item;
        }

        private JToken EnsureValue(object value)
        {
            if (value == null)
            {
                return null;
            }
            if (!(value is JToken))
            {
                throw new ArgumentException("Argument is not a JToken.");
            }
            return (JToken) value;
        }

        internal virtual JToken GetItem(int index)
        {
            return this.ChildrenTokens[index];
        }

        internal int IndexOfItem(JToken item)
        {
            return this.ChildrenTokens.IndexOf<JToken>(item, JTokenReferenceEqualityComparer.Instance);
        }

        internal virtual void InsertItem(int index, JToken item)
        {
            if (index > this.ChildrenTokens.Count)
            {
                throw new ArgumentOutOfRangeException("index", "Index must be within the bounds of the List.");
            }
            this.CheckReentrancy();
            item = this.EnsureParentToken(item);
            JToken token = (index == 0) ? null : this.ChildrenTokens[index - 1];
            JToken token2 = (index == this.ChildrenTokens.Count) ? null : this.ChildrenTokens[index];
            this.ValidateToken(item, null);
            item.Parent = this;
            item.Previous = token;
            if (token != null)
            {
                token.Next = item;
            }
            item.Next = token2;
            if (token2 != null)
            {
                token2.Previous = item;
            }
            this.ChildrenTokens.Insert(index, item);
            if (this.ListChanged != null)
            {
                this.OnListChanged(new ListChangedEventArgs(ListChangedType.ItemAdded, index));
            }
        }

        internal bool IsMultiContent(object content)
        {
            return ((((content is IEnumerable) && !(content is string)) && !(content is JToken)) && !(content is byte[]));
        }

        internal static bool IsTokenUnchanged(JToken currentValue, JToken newValue)
        {
            JValue value2 = currentValue as JValue;
            return ((value2 != null) && (((value2.Type == JTokenType.Null) && (newValue == null)) || value2.Equals(newValue)));
        }

        protected virtual void OnAddingNew(AddingNewEventArgs e)
        {
            AddingNewEventHandler addingNew = this.AddingNew;
            if (addingNew != null)
            {
                addingNew(this, e);
            }
        }

        protected virtual void OnListChanged(ListChangedEventArgs e)
        {
            ListChangedEventHandler listChanged = this.ListChanged;
            if (listChanged != null)
            {
                this._busy = true;
                try
                {
                    listChanged(this, e);
                }
                finally
                {
                    this._busy = false;
                }
            }
        }

        internal void ReadContentFrom(JsonReader r)
        {
            JValue value2;
            JProperty property;
            ValidationUtils.ArgumentNotNull(r, "r");
            IJsonLineInfo lineInfo = r as IJsonLineInfo;
            JContainer parent = this;
        Label_0016:
            if ((parent is JProperty) && (((JProperty) parent).Value != null))
            {
                if (parent == this)
                {
                    return;
                }
                parent = parent.Parent;
            }
            switch (r.TokenType)
            {
                case JsonToken.None:
                    goto Label_028A;

                case JsonToken.StartObject:
                {
                    JObject content = new JObject();
                    content.SetLineInfo(lineInfo);
                    parent.Add(content);
                    parent = content;
                    goto Label_028A;
                }
                case JsonToken.StartArray:
                {
                    JArray array = new JArray();
                    array.SetLineInfo(lineInfo);
                    parent.Add(array);
                    parent = array;
                    goto Label_028A;
                }
                case JsonToken.StartConstructor:
                {
                    JConstructor constructor = new JConstructor(r.Value.ToString());
                    constructor.SetLineInfo(constructor);
                    parent.Add(constructor);
                    parent = constructor;
                    goto Label_028A;
                }
                case JsonToken.PropertyName:
                {
                    string name = r.Value.ToString();
                    property = new JProperty(name);
                    property.SetLineInfo(lineInfo);
                    JProperty property2 = ((JObject) parent).Property(name);
                    if (property2 != null)
                    {
                        property2.Replace(property);
                        break;
                    }
                    parent.Add(property);
                    break;
                }
                case JsonToken.Comment:
                    value2 = JValue.CreateComment(r.Value.ToString());
                    value2.SetLineInfo(lineInfo);
                    parent.Add(value2);
                    goto Label_028A;

                case JsonToken.Integer:
                case JsonToken.Float:
                case JsonToken.String:
                case JsonToken.Boolean:
                case JsonToken.Date:
                case JsonToken.Bytes:
                    value2 = new JValue(r.Value);
                    value2.SetLineInfo(lineInfo);
                    parent.Add(value2);
                    goto Label_028A;

                case JsonToken.Null:
                    value2 = new JValue(null, JTokenType.Null);
                    value2.SetLineInfo(lineInfo);
                    parent.Add(value2);
                    goto Label_028A;

                case JsonToken.Undefined:
                    value2 = new JValue(null, JTokenType.Undefined);
                    value2.SetLineInfo(lineInfo);
                    parent.Add(value2);
                    goto Label_028A;

                case JsonToken.EndObject:
                    if (parent != this)
                    {
                        parent = parent.Parent;
                        goto Label_028A;
                    }
                    return;

                case JsonToken.EndArray:
                    if (parent == this)
                    {
                        return;
                    }
                    parent = parent.Parent;
                    goto Label_028A;

                case JsonToken.EndConstructor:
                    if (parent != this)
                    {
                        parent = parent.Parent;
                        goto Label_028A;
                    }
                    return;

                default:
                    throw new InvalidOperationException("The JsonReader should not be on a token of type {0}.".FormatWith(CultureInfo.InvariantCulture, new object[] { r.TokenType }));
            }
            parent = property;
        Label_028A:
            if (r.Read())
            {
                goto Label_0016;
            }
        }

        internal void ReadTokenFrom(JsonReader r)
        {
            int depth = r.Depth;
            if (!r.Read())
            {
                throw new Exception("Error reading {0} from JsonReader.".FormatWith(CultureInfo.InvariantCulture, new object[] { base.GetType().Name }));
            }
            this.ReadContentFrom(r);
            if (r.Depth > depth)
            {
                throw new Exception("Unexpected end of content while loading {0}.".FormatWith(CultureInfo.InvariantCulture, new object[] { base.GetType().Name }));
            }
        }

        public void RemoveAll()
        {
            this.ClearItems();
        }

        internal virtual bool RemoveItem(JToken item)
        {
            int index = this.IndexOfItem(item);
            if (index >= 0)
            {
                this.RemoveItemAt(index);
                return true;
            }
            return false;
        }

        internal virtual void RemoveItemAt(int index)
        {
            if (index < 0)
            {
                throw new ArgumentOutOfRangeException("index", "Index is less than 0.");
            }
            if (index >= this.ChildrenTokens.Count)
            {
                throw new ArgumentOutOfRangeException("index", "Index is equal to or greater than Count.");
            }
            this.CheckReentrancy();
            JToken token = this.ChildrenTokens[index];
            JToken token2 = (index == 0) ? null : this.ChildrenTokens[index - 1];
            JToken token3 = (index == (this.ChildrenTokens.Count - 1)) ? null : this.ChildrenTokens[index + 1];
            if (token2 != null)
            {
                token2.Next = token3;
            }
            if (token3 != null)
            {
                token3.Previous = token2;
            }
            token.Parent = null;
            token.Previous = null;
            token.Next = null;
            this.ChildrenTokens.RemoveAt(index);
            this.OnListChanged(new ListChangedEventArgs(ListChangedType.ItemDeleted, index));
        }

        public void ReplaceAll(object content)
        {
            this.ClearItems();
            this.Add(content);
        }

        internal virtual void ReplaceItem(JToken existing, JToken replacement)
        {
            if ((existing != null) && (existing.Parent == this))
            {
                int index = this.IndexOfItem(existing);
                this.SetItem(index, replacement);
            }
        }

        internal virtual void SetItem(int index, JToken item)
        {
            if (index < 0)
            {
                throw new ArgumentOutOfRangeException("index", "Index is less than 0.");
            }
            if (index >= this.ChildrenTokens.Count)
            {
                throw new ArgumentOutOfRangeException("index", "Index is equal to or greater than Count.");
            }
            JToken currentValue = this.ChildrenTokens[index];
            if (!IsTokenUnchanged(currentValue, item))
            {
                this.CheckReentrancy();
                item = this.EnsureParentToken(item);
                this.ValidateToken(item, currentValue);
                JToken token2 = (index == 0) ? null : this.ChildrenTokens[index - 1];
                JToken token3 = (index == (this.ChildrenTokens.Count - 1)) ? null : this.ChildrenTokens[index + 1];
                item.Parent = this;
                item.Previous = token2;
                if (token2 != null)
                {
                    token2.Next = item;
                }
                item.Next = token3;
                if (token3 != null)
                {
                    token3.Previous = item;
                }
                this.ChildrenTokens[index] = item;
                currentValue.Parent = null;
                currentValue.Previous = null;
                currentValue.Next = null;
                this.OnListChanged(new ListChangedEventArgs(ListChangedType.ItemChanged, index));
            }
        }

        void ICollection<JToken>.Add(JToken item)
        {
            this.Add(item);
        }

        void ICollection<JToken>.Clear()
        {
            this.ClearItems();
        }

        bool ICollection<JToken>.Contains(JToken item)
        {
            return this.ContainsItem(item);
        }

        void ICollection<JToken>.CopyTo(JToken[] array, int arrayIndex)
        {
            this.CopyItemsTo(array, arrayIndex);
        }

        bool ICollection<JToken>.Remove(JToken item)
        {
            return this.RemoveItem(item);
        }

        int IList<JToken>.IndexOf(JToken item)
        {
            return this.IndexOfItem(item);
        }

        void IList<JToken>.Insert(int index, JToken item)
        {
            this.InsertItem(index, item);
        }

        void IList<JToken>.RemoveAt(int index)
        {
            this.RemoveItemAt(index);
        }

        void ICollection.CopyTo(Array array, int index)
        {
            this.CopyItemsTo(array, index);
        }

        int IList.Add(object value)
        {
            this.Add(this.EnsureValue(value));
            return (this.Count - 1);
        }

        void IList.Clear()
        {
            this.ClearItems();
        }

        bool IList.Contains(object value)
        {
            return this.ContainsItem(this.EnsureValue(value));
        }

        int IList.IndexOf(object value)
        {
            return this.IndexOfItem(this.EnsureValue(value));
        }

        void IList.Insert(int index, object value)
        {
            this.InsertItem(index, this.EnsureValue(value));
        }

        void IList.Remove(object value)
        {
            this.RemoveItem(this.EnsureValue(value));
        }

        void IList.RemoveAt(int index)
        {
            this.RemoveItemAt(index);
        }

        void IBindingList.AddIndex(PropertyDescriptor property)
        {
        }

        object IBindingList.AddNew()
        {
            AddingNewEventArgs e = new AddingNewEventArgs();
            this.OnAddingNew(e);
            if (e.NewObject == null)
            {
                throw new Exception("Could not determine new value to add to '{0}'.".FormatWith(CultureInfo.InvariantCulture, new object[] { base.GetType() }));
            }
            if (!(e.NewObject is JToken))
            {
                throw new Exception("New item to be added to collection must be compatible with {0}.".FormatWith(CultureInfo.InvariantCulture, new object[] { typeof(JToken) }));
            }
            JToken newObject = (JToken) e.NewObject;
            this.Add(newObject);
            return newObject;
        }

        void IBindingList.ApplySort(PropertyDescriptor property, ListSortDirection direction)
        {
            throw new NotSupportedException();
        }

        int IBindingList.Find(PropertyDescriptor property, object key)
        {
            throw new NotSupportedException();
        }

        void IBindingList.RemoveIndex(PropertyDescriptor property)
        {
        }

        void IBindingList.RemoveSort()
        {
            throw new NotSupportedException();
        }

        PropertyDescriptorCollection ITypedList.GetItemProperties(PropertyDescriptor[] listAccessors)
        {
            ICustomTypeDescriptor first = this.First as ICustomTypeDescriptor;
            if (first != null)
            {
                return first.GetProperties();
            }
            return null;
        }

        string ITypedList.GetListName(PropertyDescriptor[] listAccessors)
        {
            return string.Empty;
        }

        internal virtual void ValidateToken(JToken o, JToken existing)
        {
            ValidationUtils.ArgumentNotNull(o, "o");
            if (o.Type == JTokenType.Property)
            {
                throw new ArgumentException("Can not add {0} to {1}.".FormatWith(CultureInfo.InvariantCulture, new object[] { o.GetType(), base.GetType() }));
            }
        }

        public override IEnumerable<T> Values<T>()
        {
            return this.ChildrenTokens.Convert<JToken, T>();
        }

        protected abstract IList<JToken> ChildrenTokens { get; }

        public int Count
        {
            get
            {
                return this.ChildrenTokens.Count;
            }
        }

        public override JToken First
        {
            get
            {
                return this.ChildrenTokens.FirstOrDefault<JToken>();
            }
        }

        public override bool HasValues
        {
            get
            {
                return (this.ChildrenTokens.Count > 0);
            }
        }

        public override JToken Last
        {
            get
            {
                return this.ChildrenTokens.LastOrDefault<JToken>();
            }
        }

        bool ICollection<JToken>.IsReadOnly
        {
            get
            {
                return false;
            }
        }

        JToken IList<JToken>.this[int index]
        {
            get
            {
                return this.GetItem(index);
            }
            set
            {
                this.SetItem(index, value);
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
                return false;
            }
        }

        bool IList.IsReadOnly
        {
            get
            {
                return false;
            }
        }

        object IList.this[int index]
        {
            get
            {
                return this.GetItem(index);
            }
            set
            {
                this.SetItem(index, this.EnsureValue(value));
            }
        }

        bool IBindingList.AllowEdit
        {
            get
            {
                return true;
            }
        }

        bool IBindingList.AllowNew
        {
            get
            {
                return true;
            }
        }

        bool IBindingList.AllowRemove
        {
            get
            {
                return true;
            }
        }

        bool IBindingList.IsSorted
        {
            get
            {
                return false;
            }
        }

        ListSortDirection IBindingList.SortDirection
        {
            get
            {
                return ListSortDirection.Ascending;
            }
        }

        PropertyDescriptor IBindingList.SortProperty
        {
            get
            {
                return null;
            }
        }

        bool IBindingList.SupportsChangeNotification
        {
            get
            {
                return true;
            }
        }

        bool IBindingList.SupportsSearching
        {
            get
            {
                return false;
            }
        }

        bool IBindingList.SupportsSorting
        {
            get
            {
                return false;
            }
        }


        private class JTokenReferenceEqualityComparer : IEqualityComparer<JToken>
        {
            public static readonly JContainer.JTokenReferenceEqualityComparer Instance = new JContainer.JTokenReferenceEqualityComparer();

            public bool Equals(JToken x, JToken y)
            {
                return object.ReferenceEquals(x, y);
            }

            public int GetHashCode(JToken obj)
            {
                if (obj == null)
                {
                    return 0;
                }
                return obj.GetHashCode();
            }
        }
    }
}

