namespace Sunisoft.IrisSkin
{
    using Sunisoft.IrisSkin.Design;
    using System;
    using System.Collections;
    using System.ComponentModel;
    using System.Drawing.Design;
    using System.Reflection;

    [Editor(typeof(xf4009cb7cc23b91e), typeof(UITypeEditor))]
    public class SkinCollection : CollectionBase
    {
        internal SkinCollection()
        {
        }

        public SkinCollectionItem Add(SkinCollectionItem item)
        {
            base.List.Add(item);
            return item;
        }

        public void Remove(SkinCollectionItem item)
        {
            base.List.Remove(item);
        }

        public void Remove(int index)
        {
            base.List.RemoveAt(index);
        }

        public SkinCollectionItem this[int index]
        {
            get
            {
                return (SkinCollectionItem) base.List[index];
            }
        }
    }
}

