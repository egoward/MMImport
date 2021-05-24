using System;
using System.Collections.Generic;
using System.Text;
using OSGeo.OGR;
using System.Drawing.Design;
using System.Windows.Forms.Design;
using System.ComponentModel;
using System.Windows.Forms;

namespace MMImportCore
{
    public static class OGRHelper
    {
        public static bool Initialised = false;

        public static void EnsureInitialised()
        {
            if (!Initialised)
            {
                Ogr.RegisterAll();
                Initialised = true;
            }
        }

        public class UITypeEditorOGRDataSource : UITypeEditor
        {
            public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
            {
                return UITypeEditorEditStyle.DropDown;
            }


            ListBox lb;
            string sValue;

            IWindowsFormsEditorService edSvc = null;
            public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value)
            {
                OGRHelper.EnsureInitialised();

                sValue = value == null ? "" : value.ToString();

                edSvc = (IWindowsFormsEditorService)provider.GetService(typeof(IWindowsFormsEditorService));

                int numDrivers = Ogr.GetDriverCount();

                lb = new ListBox();
                for(int c=0;c<numDrivers;c++)
                {
                    string name = Ogr.GetDriver(c).GetName();
                    int idx = lb.Items.Add(name);
                    if (sValue == name)
                        lb.SelectedIndex = idx;
                }
                lb.DoubleClick += new EventHandler(ListSelected);
                lb.Click += new EventHandler(ListSelected);
                edSvc.DropDownControl(lb);
                lb.Dispose();
                return sValue;

            }
            void ListSelected(object sender, EventArgs e)
            {
                if (edSvc != null)
                {
                    sValue = (string)lb.SelectedItem;
                    edSvc.CloseDropDown();
                }

            }
        }

    }
}
