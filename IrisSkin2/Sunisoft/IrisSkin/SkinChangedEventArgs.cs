namespace Sunisoft.IrisSkin
{
    using System;

    public class SkinChangedEventArgs : EventArgs
    {
        private bool x9315ecd224bb4564;

        public SkinChangedEventArgs(bool realActive)
        {
            this.x9315ecd224bb4564 = realActive;
        }

        public bool RealActive
        {
            get
            {
                return this.x9315ecd224bb4564;
            }
            set
            {
                this.x9315ecd224bb4564 = value;
            }
        }
    }
}

