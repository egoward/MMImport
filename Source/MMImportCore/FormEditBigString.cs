using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Edonica.XMLImport
{
    /// <summary>
    /// Helper class used by the property grid so we can edit connection strings
    /// </summary>
    public partial class FormEditBigString : Form
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public FormEditBigString()
        {
            InitializeComponent();
        }

        internal string EditText
        {
            get { return textBox.Text; }
            set { textBox.Text = value; }
        }

        private void buttonOK_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
            Close();
        }

        private void FormEditBigString_Load(object sender, EventArgs e)
        {
            textBox.Focus();
        }

        //public static DialogResult EditString( string title, ref string s )
        //{
        //    using (FormEditBigString f = new FormEditBigString())
        //    {
        //        f.Text = title;
        //        if (s != null)
        //        {
        //            f.textBox.Text = s;
        //        }
        //        f.ShowDialog();
        //        if (f.DialogResult == DialogResult.OK)
        //        {
        //            s = f.textBox.Text;
        //        }
        //        return f.DialogResult;
        //    }
        //}
    }
}