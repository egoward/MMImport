using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using Edonica.XMLImport;
using OSGeo.OGR;
using System.IO;
using System.Windows.Forms;
using Edonica.MapBase;

namespace MMImportCore.OGR
{
    class OutputOGR : OutputGeneric
    {
        public ConfigOutputOGR config;
        DataSource dataSource = null;


        public override void BaseShutdown()
        {
        }

        public override void DropTables()
        {
            throw new Exception("OGR Output does not support this method.");
        }

        public override void CreateTables()
        {
            throw new Exception("OGR Output does not support this method.");
        }

        public override void DropIndexes()
        {
            throw new Exception("OGR Output does not support this method.");
        }

        public override void CreateIndexes()
        {
            throw new Exception("OGR Output does not support this method.");
        }
        public override void BaseInit(ExportSchema schema)
        {
            base.BaseInit(schema);
            foreach (MapTableDefinition table in schema.tables.Values)
            {
                Tables.Add(table.TableName, new OutputOGRTable(this, table));
            }
        }
        public override void OutputStart( string sourceID )
        {
            OGRHelper.EnsureInitialised();
            Log("Starting output for " + sourceID);

            Driver driver = Ogr.GetDriverByName(config.DriverName);
            if (driver == null)
                throw new Exception("Unable to locate driver " + config.DriverName);

            //We don't really know how the data sources work but it makes life a lot better
            //if we can create folders automatically.
            OGRDataSourceType DataSourceType = config.DataSourceType;
            if (DataSourceType == OGRDataSourceType.Auto)
            {
                switch (config.DriverName)
                {
                    case "ESRI Shapefile":
                    case "MapInfo File":
                        DataSourceType = OGRDataSourceType.Folder;
                        break;
                    case "CSV":
                    case "GML":
                    case "KML":
                        DataSourceType = OGRDataSourceType.File;
                        break;
                    default:
                        DataSourceType = OGRDataSourceType.Other;
                        break;
                }
            }

            string dataSourceName = config.DataSourceName;
            if (DataSourceType == OGRDataSourceType.Folder)
            {
                Log("Ensuring folder exists : " + dataSourceName);
                if (Path.IsPathRooted(dataSourceName) && !Directory.Exists(dataSourceName))
                {
                    try
                    {
                        Log("Attempting to create directory : " + dataSourceName);
                        Directory.CreateDirectory(dataSourceName);
                        Log("Directory created");
                    }
                    catch (Exception)
                    {
                        Log("Failed to create directory");
                    }
                }

                if (config.OutputEachInputFileToItsOwnDirectory)
                {
                    dataSourceName = dataSourceName + "\\" + sourceID;
                }
            }
            else if (DataSourceType == OGRDataSourceType.File)
            {
                string folder = Path.GetDirectoryName(dataSourceName);
                if (!Directory.Exists(folder))
                {
                    try
                    {
                        Log("Attempting to create directory : " + folder);
                        Directory.CreateDirectory(folder);
                        Log("Directory created");
                    }
                    catch (Exception)
                    {
                        Log("Failed to create directory");
                    }
                }
                if (config.OutputEachInputFileToItsOwnDirectory)
                {
                    dataSourceName = Path.ChangeExtension(dataSourceName, sourceID + Path.GetExtension(dataSourceName));
                }
            }
            else
            {
                //We don't know what the connection is, just try to do it.
            }

            Log("CreateDataSource : " + dataSourceName);
            dataSource = driver.CreateDataSource(dataSourceName , new string[] { });

            foreach (OutputOGRTable table in Tables.Values)
            {
                table.Init( dataSource, sourceID );
            }
        }

        public override void OutputStop()
        {
            Log("Finishing output");
            foreach (OutputOGRTable table in Tables.Values)
            {
                table.Finish();
            }

            if (dataSource != null)
            {
                dataSource.Dispose();
                dataSource = null;
            }

        }

        public override void RemoveDuplicates()
        {
            throw new Exception("OGR Output does not support this method.");
        }

        public override void QueryInformation()
        {
            Log("No information to report from OGR driver");
        }

        public override void ApplyUpdates()
        {
            throw new Exception("OGR Output does not support this method.");
        }

        public override ConfigOutputGeneric GeneralConfig
        {
            get { return config; }
        }
    }
}
