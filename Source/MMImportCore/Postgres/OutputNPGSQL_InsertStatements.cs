using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Xml.Xsl;
using System.Data;

using Npgsql;

using Edonica.MapBase;

namespace Edonica.XMLImport
{
    class OutputNPGSQL_InsertStatements : OutputProcessorNPGSQL_Base
    {
        public override void BaseInit(ExportSchema schema)
        {
            base.BaseInit(schema);
        }
        public ConfigOutputPostgresInsert config;

        public override ConfigOutputGeneric GeneralConfig
        {
            get { return config; }
        }
        public override ConfigOutputPostgres PostgresConfig
        {
            get { return config; }
        }

        public override void OutputStart(string sourceID )
        {
            //Make insert statements;
            foreach (MapTableDefinition table in schema.tables.Values)
            {
                OutputNPGSQL_InsertStatementsTable holder = new OutputNPGSQL_InsertStatementsTable(this, table);

                holder.InitTable(table);

                Tables.Add(table.TableName, holder);
            }
        }

        public override void OutputStop()
        {
            //Let the database commands look after themselves.
        }

        //Dictionary<string, OutputNPGSQL_InsertStatementsTable> tables = new Dictionary<string, OutputNPGSQL_InsertStatementsTable>();

        //public override IDictionary<string, OutputGenericTable> Tables
        //{
        //    get
        //    {
        //        return (IDictionary<string, OutputGenericTable>)tables;
        //    }
        //}


    }
}