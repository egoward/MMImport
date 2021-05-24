using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Xml.Xsl;
using System.Data;
using Npgsql;
using System.IO;
using System.Diagnostics;
using Edonica.MapBase;


namespace Edonica.XMLImport
{
    /// <summary>
    /// Bulk loader
    /// </summary>
    public class OutputNPGSQL_BulkLoad : OutputProcessorNPGSQL_Base
    {
        /// <summary>
        /// Our configuration
        /// </summary>
        public ConfigOutputPostgresBulk config;

        /// <summary>
        /// Override to provide general config
        /// </summary>
        public override ConfigOutputGeneric GeneralConfig
        {
            get { return config; }
        }

        /// <summary>
        /// Override to provide Postgres config
        /// </summary>
        public override ConfigOutputPostgres PostgresConfig
        {
            get { return config; }
        }


        /// <summary>
        /// Override to initialise this.
        /// Note that we just create our structure, we do more stuff on OutputStart
        /// </summary>
        /// <param name="schema"></param>
        public override void BaseInit(ExportSchema schema)
        {
            if (!File.Exists(config.PSQLPathToExe))
            {
                throw new Exception("Unable to find file [" + config.PSQLPathToExe + "] - Please check PSQLPathToExe setting.  This executable is normally located in ...\\Postgres\\bin\\ folder");
            }

            base.BaseInit(schema);

            //Make insert statements;
            foreach (MapTableDefinition table in schema.tables.Values)
            {
                Tables.Add(table.TableName, new OutputNPGSQL_BulkLoadTable( this, table ));
            }
        }

        /// <summary>
        /// Shut down code (just calls base)
        /// </summary>
        public override void BaseShutdown()
        {
            base.BaseShutdown();

            if (errorsOccured)
            {
                Log("");
                Log("*******************************************************");
                Log("PSQL process wrote to STDERR - check for errors");
                Log("*******************************************************");
                Log("");
            }
        }

        /// <summary>
        /// Flag to track if errors have occured
        /// </summary>
        public bool errorsOccured=false;

        /// <summary>
        /// Start exporting data - passes request to tables
        /// </summary>
        public override void OutputStart( string sourceID )
        {
            foreach (OutputNPGSQL_BulkLoadTable table in Tables.Values)
            {
                table.Init();
            }
            Log("Note that output from bulk loader processes is buffered and may only appear at the end");

        }

        /// <summary>
        /// Finish exporting data - passes request to tables
        /// </summary>
        public override void OutputStop()
        {
            foreach (OutputNPGSQL_BulkLoadTable table in Tables.Values)
            {
                table.Finish();
            }
        }







    }
}