namespace Sunisoft.IrisSkin
{
    using System;
    using System.Windows.Forms;

    internal class xf3f6919ac5d158dc
    {
        public static xbd3f2493841f18a1 Create(Control control, SkinEngine engine)
        {
            if (control is Label)
            {
                return new x2d2ded96c91a7f0f(control, engine);
            }
            if (control is Button)
            {
                return new xead0c12c344036e3(control, engine);
            }
            if (control is CheckBox)
            {
                return new x896bb4cf7b29301a(control, engine);
            }
            if (control is RadioButton)
            {
                return new xfe25d236d92b80ed(control, engine);
            }
            if (control is ScrollBar)
            {
                return new x41d32e77872302c5(control, engine);
            }
            if (control is TextBox)
            {
                return new x3e6caec60640904a(control, engine);
            }
            if (control is MaskedTextBox)
            {
                return new x3e6caec60640904a(control, engine);
            }
            if (control is RichTextBox)
            {
                return new x3e6caec60640904a(control, engine);
            }
            if (control is TrackBar)
            {
                return new x422d5afcba1397d3(control, engine);
            }
            if (control is ProgressBar)
            {
                return new xc3d08931d227ebfe(control, engine);
            }
            if (control is NumericUpDown)
            {
                return new x2ada8799c851d3c1(control, engine);
            }
            if (control is TabPage)
            {
                return new xbd3f2493841f18a1(control, engine);
            }
            if (control is Panel)
            {
                return new x3a157e8c7a942ff8(control, engine);
            }
            if (control is ListView)
            {
                return new x1d3c48e32d645589(control, engine);
            }
            if (control is ComboBox)
            {
                return new xd8e48d4d4b6a016e(control, engine);
            }
            if (control is ListBox)
            {
                return new x467f7b62900802c7(control, engine);
            }
            if (control is TabControl)
            {
                return new x97d171718e5c7e7f(control, engine);
            }
            if (control is DateTimePicker)
            {
                return new x6a7aef305e9ce97d(control, engine);
            }
            if (control is GroupBox)
            {
                return new x77a19d75b4e98e57(control, engine);
            }
            if (control is TreeView)
            {
                return new xc0bf85194d12f6b3(control, engine);
            }
            if (control is DataGrid)
            {
                return new x0e2bf2da454c077f(control, engine);
            }
            if (control is ToolBar)
            {
                return new xac9a4b8f6325d7e3(control, engine);
            }
            if (control is StatusBar)
            {
                return new xba6d4b6ba4628dd2(control, engine);
            }
            if (control is ToolStripContainer)
            {
                return new xf2c3112f07098aa3(control, engine);
            }
            if (control is MenuStrip)
            {
                return new x095234c5c1abb370(control, engine);
            }
            if (control is StatusStrip)
            {
                return new x095234c5c1abb370(control, engine);
            }
            if (control is ToolStrip)
            {
                return new x095234c5c1abb370(control, engine);
            }
            if (engine.Enable3rdControl)
            {
                xe352b8686e1d5ecf.Create(control, engine);
            }
            return new xbd32ddd20be31ef9(control, engine);
        }
    }
}

