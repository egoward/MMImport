using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;

namespace Edonica.XMLImport
{
    /// <summary>
    /// Configuration details that are specifc to Postgres
    /// Used in both the bulk load and insert implementations
    /// </summary>
    public abstract class ConfigOutputPostgres : ConfigOutputGeneric
    {

        private string connectionString="Server=127.0.0.1;Database=MasterMap;User Id=postgres;CommandTimeout=86400;";

        /// <summary>
        /// Connection string passed into NPGSQL library
        /// </summary>
        [Category("(general)")]
        [Description("Connection string to use when connecting to the database")]
        [EditorAttribute(typeof(UITypeEditorEditBigString), typeof(System.Drawing.Design.UITypeEditor))]
        public string ConnectionString
        {
            get { return connectionString; }
            set { connectionString = value; }
        }

        private int srid = 27700;
        /// <summary>
        /// Spatial reference identifier.  We set this attribute on the table after the import.
        /// </summary>
        [Category("Spatial system")]
        [Description("EPSG (European Petroleum Survey Group) code that defined the coordinate space we're bringing the data in with.  To avoid some problems importing WKB, we bring data in with an SRID of -1 then set it when we add the indexes.")]
        public int SRID
        {
            get { return srid; }
            set { srid=value; }
        }

        private bool ignoreErrorsDroppingTables =true;

        /// <summary>
        /// Ignore errors when dropping tables and indexes.  This is handy for clearing the database down from an odd state.
        /// </summary>
        [Description("Don't generate an error when DROP commands fail.")]
        public bool IgnoreErrorsDroppingTables
        {
            get { return ignoreErrorsDroppingTables; }
            set { ignoreErrorsDroppingTables = value; }
        }


        private string tableNamePrefix = "";
        /// <summary>
        /// Name to prefix all the tables with.  Set this to "UPDATE_" prior to importing change only updates.
        /// </summary>
        [Description("Name to prefix all the tables with.  Set this to \"UPDATE_\" prior to importing change only updates.")]
        public string TableNamePrefix
        {
            get { return tableNamePrefix; }
            set { tableNamePrefix = value; }
        }

        private int commandTimeoutOverride = 86400;

        /// <summary>
        /// Change the CommandTimeout on postgres commands.  This is also set in the connection string but 
        /// we've encountered problems where it doesn't appear to be having an effect.
        /// </summary>
        [Description("Number of seconds to wait for a CommandTimeout.  This is also set in the ConnectionString, to be safe, set it in both places.")]
        public int CommandTimeoutOverride
        {
            get { return commandTimeoutOverride; }
            set { commandTimeoutOverride = value; }
        }
	
    }
}
