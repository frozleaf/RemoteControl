using System;
using System.Collections;
using System.Runtime.CompilerServices;

namespace Sunisoft.IrisSkin.InternalControls
{
    internal class x81f0e41c32f52159 : CollectionBase
    {
        private int xa83ef27b7a71ab3f;

        private x66edf89974942dab x18717d558d54a6dc;

        private x66edf89974942dab xebadb072f5851c44;

        private x2f6aff803d60b50c xf12ffa4bc2262d75;

        private x2f6aff803d60b50c xb1acec69632d2193;

        private x2f6aff803d60b50c xf41c8c66e3182d79;

        private x2f6aff803d60b50c xb56f44eae6e354a0;

        public event x66edf89974942dab Clearing
        {
            [MethodImpl(MethodImplOptions.Synchronized)]
            add
            {
                this.x18717d558d54a6dc = (x66edf89974942dab)Delegate.Combine(this.x18717d558d54a6dc, value);
            }
            [MethodImpl(MethodImplOptions.Synchronized)]
            remove
            {
                this.x18717d558d54a6dc = (x66edf89974942dab)Delegate.Remove(this.x18717d558d54a6dc, value);
            }
        }

        public event x66edf89974942dab Cleared
        {
            [MethodImpl(MethodImplOptions.Synchronized)]
            add
            {
                this.xebadb072f5851c44 = (x66edf89974942dab)Delegate.Combine(this.xebadb072f5851c44, value);
            }
            [MethodImpl(MethodImplOptions.Synchronized)]
            remove
            {
                this.xebadb072f5851c44 = (x66edf89974942dab)Delegate.Remove(this.xebadb072f5851c44, value);
            }
        }

        public event x2f6aff803d60b50c Inserting
        {
            [MethodImpl(MethodImplOptions.Synchronized)]
            add
            {
                this.xf12ffa4bc2262d75 = (x2f6aff803d60b50c)Delegate.Combine(this.xf12ffa4bc2262d75, value);
            }
            [MethodImpl(MethodImplOptions.Synchronized)]
            remove
            {
                this.xf12ffa4bc2262d75 = (x2f6aff803d60b50c)Delegate.Remove(this.xf12ffa4bc2262d75, value);
            }
        }

        public event x2f6aff803d60b50c Inserted
        {
            [MethodImpl(MethodImplOptions.Synchronized)]
            add
            {
                this.xb1acec69632d2193 = (x2f6aff803d60b50c)Delegate.Combine(this.xb1acec69632d2193, value);
            }
            [MethodImpl(MethodImplOptions.Synchronized)]
            remove
            {
                this.xb1acec69632d2193 = (x2f6aff803d60b50c)Delegate.Remove(this.xb1acec69632d2193, value);
            }
        }

        public event x2f6aff803d60b50c Removing
        {
            [MethodImpl(MethodImplOptions.Synchronized)]
            add
            {
                this.xf41c8c66e3182d79 = (x2f6aff803d60b50c)Delegate.Combine(this.xf41c8c66e3182d79, value);
            }
            [MethodImpl(MethodImplOptions.Synchronized)]
            remove
            {
                this.xf41c8c66e3182d79 = (x2f6aff803d60b50c)Delegate.Remove(this.xf41c8c66e3182d79, value);
            }
        }

        public event x2f6aff803d60b50c Removed
        {
            [MethodImpl(MethodImplOptions.Synchronized)]
            add
            {
                this.xb56f44eae6e354a0 = (x2f6aff803d60b50c)Delegate.Combine(this.xb56f44eae6e354a0, value);
            }
            [MethodImpl(MethodImplOptions.Synchronized)]
            remove
            {
                this.xb56f44eae6e354a0 = (x2f6aff803d60b50c)Delegate.Remove(this.xb56f44eae6e354a0, value);
            }
        }

        public bool IsSuspended
        {
            get
            {
                return this.xa83ef27b7a71ab3f > 0;
            }
        }

        public x81f0e41c32f52159()
        {
            this.xa83ef27b7a71ab3f = 0;
        }

        public void SuspendEvents()
        {
            this.xa83ef27b7a71ab3f++;
        }

        public void ResumeEvents()
        {
            this.xa83ef27b7a71ab3f--;
        }

        protected override void OnClear()
        {
            if (this.IsSuspended)
            {
                return;
            }
            if (this.x18717d558d54a6dc != null)
            {
                this.x18717d558d54a6dc();
            }
        }

        protected override void OnClearComplete()
        {
            if (this.IsSuspended)
            {
                return;
            }
            if (this.xebadb072f5851c44 != null)
            {
                this.xebadb072f5851c44();
            }
        }

        protected override void OnInsert(int index, object value)
        {
            if (this.IsSuspended)
            {
                return;
            }
            if (this.xf12ffa4bc2262d75 != null)
            {
                this.xf12ffa4bc2262d75(index, value);
            }
        }

        protected override void OnInsertComplete(int index, object value)
        {
            if (this.IsSuspended)
            {
                return;
            }
            if (this.xb1acec69632d2193 != null)
            {
                this.xb1acec69632d2193(index, value);
            }
        }

        protected override void OnRemove(int index, object value)
        {
            if (this.IsSuspended)
            {
                return;
            }
            if (this.xf41c8c66e3182d79 != null)
            {
                this.xf41c8c66e3182d79(index, value);
            }
        }

        protected override void OnRemoveComplete(int index, object value)
        {
            if (this.IsSuspended)
            {
                return;
            }
            if (this.xb56f44eae6e354a0 != null)
            {
                this.xb56f44eae6e354a0(index, value);
            }
        }

        protected int x2ee5ad3d826ed0fe(object xbcea506a33cf9111)
        {
            return base.List.IndexOf(xbcea506a33cf9111);
        }
    }
}
