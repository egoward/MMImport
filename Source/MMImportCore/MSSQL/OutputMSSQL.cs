using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using Edonica.MapBase;

namespace Edonica.XMLImport
{
    class OutputMSSQL : OutputGeneric
    {
        public SqlConnection conn;
        public ConfigOutputMSSQL config;
        public ExportSchema schema;



        public override void BaseInit(ExportSchema schema)
        {
            base.BaseInit(schema);

            Log("Connecting to database : ");
            Log(config.ConnectionString);

            this.schema = schema;
            conn = new SqlConnection(config.ConnectionString);
            conn.Open();
            Log("Connected OK");

            if (config.DummyRun)
            {
                Log("**** DUMMY RUN - NO CHANGES WILL BE MADE ****");
            }
        }

        public override void BaseShutdown()
        {
            if (config.DummyRun)
            {
                Log("**** DUMMY RUN - SQL WAS NOT RUN ****");
            }

            conn.Close();
            conn.Dispose();
        }

        /// <summary>
        /// Create all our database commands here and override the timeout value.
        /// </summary>
        /// <returns></returns>
        public SqlCommand CreateCommand()
        {
            SqlCommand command = conn.CreateCommand();
            command.CommandTimeout = config.CommandTimeoutOverride;
            return command;
        }

        void TryRunSQL(string s, bool returnIsInteresting)
        {
            try
            {
                RunSQL(s, returnIsInteresting);
            }
            catch (Exception ex)
            {
                Log(ex.Message);
            }
        }

        void RunSQL(string sql, bool returnIsInteresting)
        {
            Log("SQL:" + sql);

            if (GeneralConfig.DummyRun)
            {
                //Do nothing
            }
            else
            {
                using (SqlCommand command = CreateCommand())
                {
                    command.CommandText = sql;
                    int i = command.ExecuteNonQuery();
                    if (returnIsInteresting)
                    {
                        Log("  returned : " + i);
                    }
                }
            }
        }

        object RunSQLScalar(string sql)
        {

            using (SqlCommand command = CreateCommand())
            {
                Log("-- Running SQL:");
                Log(sql);
                command.CommandText = sql;
                return command.ExecuteScalar();
            }
        }

        /// <summary>
        /// Convert an enumeration of strings into a CSV list.
        /// </summary>
        /// <param name="e"></param>
        /// <returns></returns>
        public static string MakeCSVLine(IEnumerable<string> e)
        {
            bool first = true;
            string ret = "";
            foreach (string s in e)
            {
                if (first)
                    first = false;
                else
                    ret = ret + ",";
                ret += s;
            }
            return ret;
        }

        internal string FormatFieldForSQLCreate(MapFieldDefinition field)
        {
            string ret = field.Name + " ";
            switch (field.Type)
            {
                case MapFieldType.Double:
                    ret += "float";
                    break;
                case MapFieldType.Int32:
                    ret += "int";
                    break;
                case MapFieldType.Point:
                case MapFieldType.Polyline:
                case MapFieldType.Region:
                case MapFieldType.Geometry:
                    {
                        string geometryType = config.ConvertToWGS84 ? "geography" : "geometry";
                        ret += geometryType;
                    }
                    break;
                case MapFieldType.String:
                    {
                        if (field.Size == 0)
                            ret += "ntext";
                        else
                            ret += "nvarchar(" + field.Size.ToString() + ")";
                    }
                    break;
                case MapFieldType.XML:
                    ret += "xml";
                    break;
            }
            return ret;
        }


        /// <summary>
        /// Called to create tables initially, then again when we remove duplicates
        /// </summary>
        /// <param name="table"></param>
        void BaseCreateTable(MapTableDefinition table)
        {
           string commandText = "CREATE TABLE " + config.TableNamePrefix + table.TableName + " (" +
                MakeCSVLine( table.ConvertAll( x => FormatFieldForSQLCreate( x ) ) ) + ")";

            RunSQL(commandText,false);
        }

        /// <summary>
        /// Create the defined tables and add the geometry column
        /// </summary>
        public override void CreateTables()
        {
            foreach (MapTableDefinition table in schema.tables.Values)
            {
                Log("Creating table " + config.TableNamePrefix + table.TableName);
                BaseCreateTable(table);
            }
        }

        public override void DropTables()
        {
            foreach (MapTableDefinition table in schema.tables.Values)
            {
                TryRunSQL("DROP TABLE " + config.TableNamePrefix + table.TableName, false);
            }
        }

        public override void DropIndexes()
        {
            foreach (MapTableDefinition table in schema.tables.Values)
            {
                string tableName = config.TableNamePrefix + table.TableName;

                foreach (MapFieldDefinition field in table.Where(f => ExportSchema.IsGeometry(f.Type)))
                {
                    TryRunSQL("DROP INDEX IX_" + tableName + "_" + field.Name + " ON " + tableName, false);
                }
                TryRunSQL("ALTER TABLE " + tableName + " DROP CONSTRAINT PK_" + tableName,false);
                TryRunSQL("ALTER TABLE " + tableName + " ALTER COLUMN " + FormatFieldForSQLCreate(table[0]), false);

                List<MapFieldDefinition> geometricFields = table.FindAll(delegate(MapFieldDefinition f) { return ExportSchema.IsGeometry(f.Type); });
            }
        }

        public override void CreateIndexes()
        {
            foreach (MapTableDefinition table in schema.tables.Values)
            {
                string tableName = config.TableNamePrefix + table.TableName;
                RunSQL("ALTER TABLE "+tableName+" ALTER COLUMN "+FormatFieldForSQLCreate(table[0]) +" NOT NULL ",false);
                RunSQL("ALTER TABLE " + tableName + " ADD CONSTRAINT PK_" + tableName + " PRIMARY KEY (" + table[0].Name + ")", false);

                foreach (MapFieldDefinition field in table.Where(f => ExportSchema.IsGeometry(f.Type)))
                {
                    string spatialIndexSQL = "CREATE SPATIAL INDEX IX_" + tableName + "_" + field.Name+ " ON "+tableName+"("+field.Name+")";
                    if( !config.ConvertToWGS84 )
                    {
                        spatialIndexSQL+=
                            String.Format(" WITH ( BOUNDING_BOX = ({0},{1},{2},{3}), CELLS_PER_OBJECT={4}, GRIDS=({5},{6},{7},{8}) )",
                                config.SpatialIndexXMIN, config.SpatialIndexYMIN, config.SpatialIndexXMAX, config.SpatialIndexYMAX,
                                config.CellsPerObject,
                                config.GridDensityLevel1,config.GridDensityLevel2,config.GridDensityLevel3,config.GridDensityLevel4
                                );

                        //CELLS_PER_OBJECT = 16

                    }
                    RunSQL(spatialIndexSQL,false);

  
//                    CREATE SPATIAL INDEX SPATIAL_TopographicPoint ON dbo.TopographicPoint(Geom) USING GEOMETRY_GRID   
//	 WITH( BOUNDING_BOX  = ( xmin  = 0.0, ymin  = 0.0, xmax  = 0.0, ymax  = 0.0), GRIDS  = ( LEVEL_1  = MEDIUM, LEVEL_2  = MEDIUM, LEVEL_3  = MEDIUM, LEVEL_4  = MEDIUM), CELLS_PER_OBJECT  = 16, STATISTICS_NORECOMPUTE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)

                }
            }
        }

        public override void OutputStart(string sourceID)
        {
            //Make insert statements;
            foreach (MapTableDefinition table in schema.tables.Values)
            {
                OutputMSSQLTable holder = new OutputMSSQLTable(this, table);
                holder.InitTable(table);
                Tables.Add(table.TableName, holder);
            }
        }

        public override void OutputStop()
        {
            foreach (OutputGenericTable t in Tables.Values)
            {
                ((OutputMSSQLTable)t).Flush();
            }
            Tables.Clear();
        }

        public override void RemoveDuplicates()
        {
            Log("Removing duplicates");
            foreach (MapTableDefinition table in schema.tables.Values)
            {
                string tableName = config.TableNamePrefix + table.TableName;
                string uniqueid = table[0].Name;

                string sql = "WITH numbered AS "+
                    "( SELECT " + uniqueid + ", row_number() OVER ( PARTITION BY " + uniqueid + " ORDER BY " + uniqueid + " ) AS nr FROM " + tableName + " )" +
                     "DELETE FROM numbered WHERE nr > 1";

                RunSQL(sql, true);
            }
        }

        public override void QueryInformation()
        {
            Log("Fetching info");
            foreach (MapTableDefinition table in schema.tables.Values)
            {
                try
                {
                    object o = RunSQLScalar("SELECT COUNT(*) FROM " + config.TableNamePrefix + table.TableName);
                    Log("Result : " + o.ToString());
                }
                catch (Exception ex)
                {
                    Log("Error : " + ex.Message);
                }

            }
        }

        public override void ApplyUpdates()
        {
            string departedTableBaseName = "DepartedFeature";

            string departedFeatureTable = config.TableNamePrefix + departedTableBaseName;
            string departedPrimaryKey = schema.tables[departedTableBaseName][0].Name;

            foreach (MapTableDefinition table in schema.tables.Values)
            {
                string tableName = table.TableName;
                string updateTable = config.TableNamePrefix + table.TableName;
                string primaryKey = table[0].Name;

                if (tableName != departedTableBaseName)
                {
                    Log("Deleting old items marked for deletion");
                    TryRunSQL("DELETE FROM " + tableName + " WHERE " + tableName + "." + primaryKey + " IN (SELECT " + departedPrimaryKey + " FROM " + departedFeatureTable + ")", true);
                    Log("Deleting old items that have been updated");
                    TryRunSQL("DELETE FROM " + tableName + " WHERE " + tableName + "." + primaryKey + " IN (SELECT " + primaryKey + " FROM " + updateTable + ")", true);
                    Log("Adding the remainder");
                    TryRunSQL("INSERT INTO " + tableName + " SELECT * FROM " + updateTable, true);
                }
            }
        }

        public override ConfigOutputGeneric GeneralConfig
        {
            get { return config; }
        }
    }
}
