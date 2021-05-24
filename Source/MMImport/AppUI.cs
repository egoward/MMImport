using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Xml.Xsl;
using System.IO;
using System.Xml.Schema;
using System.IO.Compression;
using System.Windows.Forms;
using System.Configuration;

namespace Edonica.XMLImport
{
    static class AppUI
    {
        [STAThread]
        static void Main(string[] args)
        {
            
            using (FormImportMain f = new FormImportMain())
            {
                f.ShowDialog();
            }
       }

    }
}

