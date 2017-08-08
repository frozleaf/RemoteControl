namespace Sunisoft.IrisSkin
{
    using System;

    internal class x735ffd864c9f9835
    {
        public static x5b126f5f998c28e9 Create(IntPtr handle, string name, SkinEngine engine)
        {
            if (name == "BUTTON")
            {
                return new x52106a473347a957(handle, engine);
            }
            if (name == "BUTTON_CHECKBOX")
            {
                return new xdbcc71de92f8e117(handle, engine);
            }
            if (name == "BUTTON_RADIOBUTTON")
            {
                return new xcf289f871d952cfd(handle, engine);
            }
            if ((name == "COMBOBOX") || (name == "COMBOBOXEX32"))
            {
                return new x4c8858ac0e107176(handle, engine);
            }
            if (name == "SYSTABCONTROL32")
            {
                return new x224a55c9c6142e96(handle, engine);
            }
            if (name == "TOOLBARWINDOW32")
            {
                return new x3ee27da79bc28367(handle, engine);
            }
            return null;
        }
    }
}

