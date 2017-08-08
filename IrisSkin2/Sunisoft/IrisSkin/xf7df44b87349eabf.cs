namespace Sunisoft.IrisSkin
{
    using System;

    internal class xf7df44b87349eabf : x5b126f5f998c28e9
    {
        public xf7df44b87349eabf(IntPtr handle, SkinEngine engine) : base(handle, engine)
        {
        }

        protected override void PaintControl()
        {
            base.PaintControl();
            xae4dd1cafd2eb77c.RECT lpRect = new xae4dd1cafd2eb77c.RECT();
            x61467fe65a98f20c.GetWindowRect(base.Handle, ref lpRect);
            x448fd9ab43628c71.OffsetRect(ref lpRect, -lpRect.left, -lpRect.top);
            IntPtr windowDC = x61467fe65a98f20c.GetWindowDC(base.Handle);
            x61467fe65a98f20c.FrameRect(windowDC, ref lpRect, base.Engine.ControlBorderBrush);
            x61467fe65a98f20c.ReleaseDC(base.Handle, windowDC);
        }
    }
}

