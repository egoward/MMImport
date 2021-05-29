using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Xml.Xsl;
using System.Data;
using Npgsql;
using System.IO;
using Edonica.MapBase;

namespace Edonica.XMLImport
{
    /// <summary>
    /// Class containing general Postgres code.  This is used by the bulk loader and postgres
    /// </summary>
    public abstract class OutputProcessorNPGSQL_Base : OutputGeneric
    {
        //public static GisSharpBlog.NetTopologySuite.IO.WKTWriter wktWriter = new GisSharpBlog.NetTopologySuite.IO.WKTWriter();


        //NpgsqlConnection conn;
        /*
    Server: Address/Name of Postgresql Server; 
    Port: Port to connect to; 
    Protocol: Protocol version to use, instead of automatic; Integer 2 or 3; 
    Database: Database name. Defaults to user name if not specified; 
    User Id: User name; 
    Password: Password for clear text authentication; 
    SSL: True or False. Controls whether to attempt a secure connection. Default = False; 
    Pooling: True or False. Controls whether connection pooling is used. Default = True; 
    MinPoolSize: Min size of connection pool; 
    MaxPoolSize: Max size of connection pool; 
    Encoding: Encoding to be used; Can be ASCII or UNICODE. Default is ASCII. Use UNICODE if you are having problems with accents. 
    Timeout: Time to wait for connection open in seconds. Default is 15. 
    CommandTimeout: Time to wait for command to finish execution before throw an exception. In seconds. Default is 20. 
    Sslmode: Mode for ssl connection control. Can be Prefer, Require, Allow or Disable. Default is Disable. Check user manual for explanation of values. 
    ConnectionLifeTime: Time to wait before closing unused connections in the pool in seconds. Default is 15. 
    SyncNotification: Specifies if Npgsql should use synchronous notifications. 
         */
        //public string ConnectionString;// = Settings.Default.NPGSQLConnectionString;// "Server=localhost;Database=Scratch;User Id=postgres";

        //public bool IgnoreDropFailures = true;
        //public bool DropTables = true;
        //public bool CreateTables = true;

        internal IDbConnection conn;

        internal ExportSchema schema;

        //void Log(string s)
        //{
        //    ReportWarning(s);
        //}

        //static GeometryMunger munger = new GeometryMunger();

        internal string FormatFieldForSQLCreate(MapFieldDefinition field)
        {
            string ret = field.Name + " ";
            switch (field.Type)
            {
                case MapFieldType.Double:
                    ret += "double precision";
                    break;
                case MapFieldType.Int32:
                    ret += "integer";
                    break;
                //case OutputType.Point:
                //    ret += "POINT";
                //    break;
                //case OutputType.Polyline:
                //    ret += "GEOMETRY";  //Should this be line[]?
                //    break;
                //case OutputType.Region:
                //    ret += "GEOMETRY"; //Should this be polygon[]?
                //    break;
                case MapFieldType.String:
                    {
                        if (field.Size == 0)
                            ret += "text";
                        else
                            ret += "varchar(" + field.Size.ToString() + ")";
                    }
                    break;
                case MapFieldType.XML:
                    ret += "text";
                    break;
            }
            return ret;
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
        /// <summary>
        /// Initialise the postgres connection
        /// </summary>
        /// <param name="schema"></param>
        public override void BaseInit(ExportSchema schema)
        {
            base.BaseInit(schema);

            Log("Connecting to database : ");
            Log(PostgresConfig.ConnectionString);

            this.schema = schema;
            conn = new NpgsqlConnection( PostgresConfig.ConnectionString );
            conn.Open();
            Log("Connected OK");

            if (PostgresConfig.DummyRun)
            {
                Log("**** DUMMY RUN - SQL WILL NOT BE RUN ****");
            }

        }

        /// <summary>
        /// Drop the tables
        /// </summary>
        public override void DropTables()
        {
            foreach (MapTableDefinition table in schema.tables.Values)
            {
                TryRunSQL("DROP TABLE " + PostgresConfig.TableNamePrefix + table.TableName,false);
            }
        }



        /// <summary>
        /// Called to create tables initially, then again when we remove duplicates
        /// </summary>
        /// <param name="table"></param>
        void BaseCreateTable(MapTableDefinition table)
        {
            List<MapFieldDefinition> nonGeometric = table.FindAll(delegate(MapFieldDefinition f) { return !ExportSchema.IsGeometry(f.Type); });
            List<String> fieldCreateList = nonGeometric.ConvertAll<string>(delegate(MapFieldDefinition f) { return FormatFieldForSQLCreate(f); });
            string commandText = "CREATE TABLE " + PostgresConfig.TableNamePrefix + table.TableName + " (" +
                MakeCSVLine(fieldCreateList) + ")";
            RunSQL(commandText, false);
            List<MapFieldDefinition> geometric = table.FindAll(delegate(MapFieldDefinition f) { return ExportSchema.IsGeometry(f.Type); });
            foreach (MapFieldDefinition field in geometric)
            {
                string nameForColumnType = field.Type == MapFieldType.Point ? "POINT" : "GEOMETRY";
                //Not quite sure why we do a 'ToLower'
                RunSQL("SELECT * FROM AddGeometryColumn('','" + PostgresConfig.TableNamePrefix.ToLower() + table.TableName.ToLower() + "','" + field.Name.ToLower() + "',-1,'" + nameForColumnType + "', 2);", false);
            }

        }


        /// <summary>
        /// Create the defined tables and add the geometry column
        /// </summary>
        public override void CreateTables()
        {
            foreach (MapTableDefinition table in schema.tables.Values)
            {
                Log("Creating table " + PostgresConfig.TableNamePrefix + table.TableName);
                BaseCreateTable(table);
            }
        }

        /// <summary>
        /// Remove any duplicates.  This is done by renaming the table and doing a 'SELECT DISTINCT'
        /// </summary>
        public override void RemoveDuplicates()
        {
            Log("Removing duplicates");
            foreach (MapTableDefinition table in schema.tables.Values)
            {
                string name = PostgresConfig.TableNamePrefix + table.TableName;
                string dupes = PostgresConfig.TableNamePrefix + table.TableName + "_WithDuplicates";
                string uniqueid = table[0].Name;


                //Move our table out of the way.
                RunSQL("ALTER TABLE " + name + " RENAME TO " + dupes + ";", false);

                //Recreate it
                BaseCreateTable(table);

                //Put the duplicates in
                RunSQL("INSERT INTO " + name + " (SELECT DISTINCT ON (" + uniqueid + ") * FROM " + dupes + ")",true);

                //Remove the old table.
                RunSQL("DROP TABLE " + dupes + ";",false);


                //This doesn't work...
                //RunSQL("ALTER TABLE " + name + " RENAME TO " + dupes + ";", false);
                //RunSQL("SELECT DISTINCT ON (" + table[0].Name + ") * INTO " + name + " FROM " + dupes + ";", true);
                //RunSQL("DROP TABLE " + dupes + ";", false);


            }
        }

        /// <summary>
        /// Just go look at the counts of records
        /// Would be nice to get info about indexes
        /// </summary>
        public override void QueryInformation()
        {
            Log("Fetching info");
            foreach (MapTableDefinition table in schema.tables.Values)
            {
                try
                {
                    object o = RunSQLScalar("SELECT COUNT(*) FROM " + PostgresConfig.TableNamePrefix + table.TableName);
                    Log("Result : " + o.ToString());
                }
                catch( Exception ex )
                {
                    Log("Error : " + ex.Message);
                }

            }
        }

        object RunSQLScalar( string sql )
        {

            using (IDbCommand command = CreateCommand())
            {
                Log("-- Running SQL:");
                Log(sql);
                command.CommandText = sql;
                return command.ExecuteScalar();
            }
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
                if (!PostgresConfig.IgnoreErrorsDroppingTables)
                {
                    throw;
                }
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
                using (IDbCommand command = CreateCommand())
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

        /// <summary>
        /// Do a VACUUM, set the SRID and add the index.
        /// Note that you can't do any more importing after the SRID is set.
        /// </summary>
        public override void CreateIndexes()
        {
            foreach (MapTableDefinition table in schema.tables.Values)
            {
                string tableName = PostgresConfig.TableNamePrefix + table.TableName;
                RunSQL("VACUUM ANALYZE " + tableName,false);
                List<MapFieldDefinition> geometricFields = table.FindAll(delegate(MapFieldDefinition f) { return ExportSchema.IsGeometry(f.Type); });
                foreach (MapFieldDefinition field in geometricFields)
                {
                    RunSQL("SELECT updateGeometrySRID('" + tableName.ToLower() + "', '" + field.Name.ToLower() + "', " + PostgresConfig.SRID + ");",false);
                    RunSQL("CREATE INDEX IDX_" + tableName + "_" + field.Name + " ON " + tableName + " USING GIST ( " + field.Name + " GIST_GEOMETRY_OPS);",false);
                }

            }
        }

        /// <summary>
        /// Drop the indexes and set the SRID back to -1 so we can do more importing.
        /// </summary>
        public override void DropIndexes()
        {
            foreach (MapTableDefinition table in schema.tables.Values)
            {
                string tableName = PostgresConfig.TableNamePrefix + table.TableName;
                List<MapFieldDefinition> geometricFields = table.FindAll(delegate(MapFieldDefinition f) { return ExportSchema.IsGeometry(f.Type); });
                foreach (MapFieldDefinition field in geometricFields)
                {
                    //Set the geometry ID to -1.  This means "We don't know" which is what WellKnownBinary comes in as.
                    TryRunSQL("SELECT updateGeometrySRID('" + tableName.ToLower() + "', '" + field.Name.ToLower() + "',-1);",false);
                    TryRunSQL("DROP INDEX IDX_" + tableName + "_" + field.Name + ";",false);
                }
            }
        }

        /// <summary>
        /// Apply updates to the dataset
        /// </summary>
        public override void ApplyUpdates()
        {
            string departedTableBaseName = "DepartedFeature";

            string departedFeatureTable = PostgresConfig.TableNamePrefix + departedTableBaseName;
            string departedPrimaryKey = schema.tables[departedTableBaseName][0].Name;

            foreach (MapTableDefinition table in schema.tables.Values)
            {
                string tableName = table.TableName;
                string updateTable = PostgresConfig.TableNamePrefix + table.TableName;
                string primaryKey = table[0].Name;

                if (tableName != departedTableBaseName)
                {
                    TryRunSQL("DELETE FROM " + tableName + " WHERE " + tableName + "." + primaryKey + " IN (SELECT " + departedPrimaryKey + " FROM " + departedFeatureTable + ")",true);
                    TryRunSQL("DELETE FROM " + tableName + " WHERE " + tableName + "." + primaryKey + " IN (SELECT " + primaryKey + " FROM " + updateTable + ")",true);
                    TryRunSQL("INSERT INTO " + tableName + " SELECT * FROM " + updateTable ,true);
                }
            }
        }

        /// <summary>
        /// Close the connection
        /// </summary>
        public override void BaseShutdown()
        {
            if (PostgresConfig.DummyRun)
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
        public IDbCommand CreateCommand()
        {
            IDbCommand command = conn.CreateCommand();
            command.CommandTimeout = PostgresConfig.CommandTimeoutOverride;
            return command;
        }

        /// <summary>
        /// Get our config
        /// </summary>
        public abstract ConfigOutputPostgres PostgresConfig { get; }
    }
}