///////////////////////////////////////////////////
//Copyright Edonica

using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing.Design;
using System.ComponentModel;
using System.Windows.Forms;

namespace Edonica.XMLImport
{
    class BrowsableFileAttribute : Attribute
    {
        private string fileFilter;

        public string FileFilter
        {
            get { return fileFilter; }
            set { fileFilter = value; }
        }
    }
    class UITypeEditorFileBrowseFile : UITypeEditor
    {
        public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
        {
            return UITypeEditorEditStyle.Modal;
        }
        public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value)
        {
            string Filter="All Files (*.*)|*.*||";
            string Title = "Select a file";
            foreach (object o in context.PropertyDescriptor.Attributes)
            {
                if (o as BrowsableFileAttribute != null)
                {
                    Filter = (o as BrowsableFileAttribute).FileFilter;
                }
                if (o as DescriptionAttribute != null)
                {
                    Title = (o as DescriptionAttribute).Description;
                }
            }
            OpenFileDialog dlg = new OpenFileDialog();
            if (value!=null)
            {
                dlg.FileName = value.ToString();
            }
            dlg.Filter = Filter;
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                return dlg.FileName;
            }
            else
            {
                return value;
            }
        }
    }

    class UITypeEditorFileBrowseFolder : UITypeEditor
    {
        public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
        {
            return UITypeEditorEditStyle.Modal;
        }
        public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value)
        {
            FolderBrowserDialog dlg = new FolderBrowserDialog();
            dlg.RootFolder = Environment.SpecialFolder.MyComputer;
            foreach (object o in context.PropertyDescriptor.Attributes)
            {
                if (o as DescriptionAttribute != null)
                {
                    dlg.Description = (o as DescriptionAttribute).Description;
                }
            }
            if (value != null)
            {
                dlg.SelectedPath = value.ToString();
            }
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                return dlg.SelectedPath;
            }
            else
            {
                return value;
            }
        }
    }

    class UITypeEditorEditBigString : UITypeEditor
    {
        public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
        {
            return UITypeEditorEditStyle.Modal;
        }
        public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value)
        {
            using (FormEditBigString f = new FormEditBigString())
            {
                f.Text = context.PropertyDescriptor.Name;
                foreach (object o in context.PropertyDescriptor.Attributes)
                {
                    if (o as DescriptionAttribute != null)
                    {
                        f.Text = (o as DescriptionAttribute).Description;
                    }
                }

                f.EditText = value ==null ? "" : value.ToString();

                if (f.ShowDialog() == DialogResult.OK)
                {
                    return f.EditText;
                }
                else
                {
                    return value;
                }
            }
                
        }
    }


}
