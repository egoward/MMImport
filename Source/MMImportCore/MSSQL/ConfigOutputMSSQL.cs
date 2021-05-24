using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Edonica.XMLImport;
using System.ComponentModel;
using System.Data.SqlClient;

namespace Edonica.XMLImport
{
    /// <summary>
    /// Configuration options for outputting to SQL Server
    /// </summary>
    public class ConfigOutputMSSQL : ConfigOutputGeneric
    {
        public ConfigOutputMSSQL()
        {
            ServerName = Environment.MachineName + "\\SQLExpress";
            DatabaseName = "MasterMap";
            ConnectionStringTemplate = "Data Source={0};Initial Catalog={1};Integrated Security=True;Application Name=\"MMImport (bulk load)\"";
            TableNamePrefix = "";
            //SRID = 27700;
            BufferSizeInRows = 10000;

            SpatialIndexXMIN = 0;
            SpatialIndexYMIN = 0;
            SpatialIndexXMAX = 900000;
            SpatialIndexYMAX = 1100000;

            CellsPerObject = 16;
            GridDensityLevel1 = GridDensity.MEDIUM;
            GridDensityLevel2 = GridDensity.MEDIUM;
            GridDensityLevel3 = GridDensity.MEDIUM;
            GridDensityLevel4 = GridDensity.MEDIUM;

        }

        [Category("Connection")]
        [Description("Server machine name, eg. 'SQLServer01'")]
        //[EditorAttribute(typeof(OGRHelper.UITypeEditorOGRDataSource), typeof(System.Drawing.Design.UITypeEditor))]
        public string ServerName {get;set;}
        [Category("Connection")]
        [Description("Name of database on that server, eg 'MasterMap'")]
        public string DatabaseName { get; set; }

        [Category("Processing")]
        [Description("Number of seconds before a command timeout is registered.")]
        public int CommandTimeoutOverride { get; set; }

        [Category("Processing")]
        [Description("Name to prefix all the tables with.  Set this to \"UPDATE_\" prior to importing change only updates.")]
        public string TableNamePrefix { get; set; }


        [Category("Processing")]
        [Description("How many rows to buffer before performing a bulk load.")]
        public int BufferSizeInRows { get; set; }

        ///// <summary>
        ///// Spatial reference identifier.  We set this attribute on the table after the import.
        ///// </summary>
        //[Category("Spatial system")]
        //[Description("EPSG (European Petroleum Survey Group) code that defined the coordinate space we're bringing the data in with.  To avoid some problems importing WKB, we bring data in with an SRID of -1 then set it when we add the indexes.")]
        //public int SRID {get;set;}



        public override OutputGeneric CreateInstance()
        {
            OutputMSSQL ret = new OutputMSSQL();
            ret.config = this;
            return ret;
        }

        [Category("Connection")]
        public string ConnectionStringTemplate { get; set; }

        [Category("Connection")]
        public string ConnectionString
        {
            get
            {
                return String.Format(ConnectionStringTemplate, ServerName, DatabaseName);
                //SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();
                //builder.DataSource = ServerName;
                //builder.InitialCatalog = DatabaseName;
                //builder.IntegratedSecurity = true;
                //builder.ApplicationName = "MMImport (bulk load)";
                //return builder.ConnectionString;
            }
        }


        [Category("Spatial Parameters - Index")]
        [Description("These extents are only used when we are using OS grid references")]
        public double SpatialIndexXMIN { get; set; }
        [Category("Spatial Parameters - Index")]
        [Description("These extents are only used when we are using OS grid references")]
        public double SpatialIndexYMIN { get; set; }
        [Category("Spatial Parameters - Index")]
        [Description("These extents are only used when we are using OS grid references")]
        public double SpatialIndexXMAX { get; set; }
        [Category("Spatial Parameters - Index")]
        [Description("These extents are only used when we are using OS grid references")]
        public double SpatialIndexYMAX { get; set; }


        [Category("Spatial Parameters - Index")]
        public GridDensity GridDensityLevel1 { get; set; }
        [Category("Spatial Parameters - Index")]
        public GridDensity GridDensityLevel2 { get; set; }
        [Category("Spatial Parameters - Index")]
        public GridDensity GridDensityLevel3 { get; set; }
        [Category("Spatial Parameters - Index")]
        public GridDensity GridDensityLevel4 { get; set; }

        public enum GridDensity
        {
            LOW,
            MEDIUM,
            HIGH
        }

        [Category("Spatial Parameters - Index")]
        public int CellsPerObject { get; set; }
    }
}
