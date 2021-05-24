using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;

namespace Edonica.XMLImport
{
    /// <summary>
    /// Configuration for postgres bulk import
    /// </summary>
    public class ConfigOutputPostgresBulk : ConfigOutputPostgres
    {
        private string bulkOutputDebugDirectory="C:\\temp\\MMBulkLoad\\";

        /// <summary>
        /// As per description
        /// </summary>
        [Category("Debug")]
        [Description("In normal processing, we don't write temporary files, but in DummyRun is set, this directory can be used to set a folder to generate the export files.  The data can then be manually loaded.  Leave blank to avoid generating these files.")]
        [EditorAttribute(typeof(UITypeEditorFileBrowseFolder), typeof(System.Drawing.Design.UITypeEditor))]
        public string BulkOutputDebugDirectory
        {
            get { return bulkOutputDebugDirectory; }
            set { bulkOutputDebugDirectory = value; }
        }


        private string pSQLPathToEXE = "C:\\pgsql\\bin\\psql.exe";
        /// <summary>
        /// As per description
        /// </summary>
        [Category("Bulk Loader")]
        [Description("Location of PSQL executable.")]
        [EditorAttribute(typeof(UITypeEditorFileBrowseFile), typeof(System.Drawing.Design.UITypeEditor))]
        [BrowsableFile( FileFilter="Postgres SQL Tool (psql.exe)|psql.exe||")]
        public string PSQLPathToExe
        {
            get { return pSQLPathToEXE; }
            set { pSQLPathToEXE = value; }
        }

        private string pSQLCommandLineArguments = "-w -h 127.0.0.1 -d MasterMap -U postgres -c \"copy {0} from STDIN\";";
        /// <summary>
        /// As per description
        /// </summary>
        [Category("Bulk Loader")]
        [Description("Arguments passed into PSQSL.")]
        [EditorAttribute(typeof(UITypeEditorEditBigString), typeof(System.Drawing.Design.UITypeEditor))]
        public string PSQLCommandLineArguments
        {
            get { return pSQLCommandLineArguments; }
            set { pSQLCommandLineArguments = value; }
        }

        /// <summary>
        /// Create an instance of the Postgres bulk loader
        /// </summary>
        /// <returns></returns>
        public override OutputGeneric CreateInstance()
        {
            OutputNPGSQL_BulkLoad ret = new OutputNPGSQL_BulkLoad();
            ret.config = this;
            return ret;
        }



    }
}
