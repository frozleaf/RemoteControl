namespace Sunisoft.IrisSkin
{
    using System;
    using System.Windows.Forms;

    internal class x3e6caec60640904a : x2edc3f693fe78d2e
    {
        public x3e6caec60640904a(Control control, SkinEngine engine) : base(control, engine)
        {
        }

        protected override void DoInit()
        {
            Control parent = base.Ctrl.Parent;
            if ((((!(parent is NumericUpDown) && !(parent is ListView)) && (!(parent is DataGrid) && !(parent is ListBox))) && (!(parent is PropertyGrid) && (parent.CompanyName != "Infragistics, Inc. (www.infragistics.com)"))) && !(parent is DataGridView))
            {
                base.DoInit();
            }
        }

        protected override int BorderWidth
        {
            get
            {
                return 1;
            }
        }

        protected override bool ChangeBackColor
        {
            get
            {
                return false;
            }
        }
    }
}

