using System;
using System.Collections.Generic;
using System.Text;

namespace Edonica.XMLImport
{
    /// <summary>
    /// Configuration object for postgres import via SQL statements
    /// </summary>
    public class ConfigOutputPostgresInsert : ConfigOutputPostgres
    {
        /// <summary>
        /// Create an instance of the Postgres SQL INSERT statement class
        /// </summary>
        /// <returns></returns>
        public override OutputGeneric CreateInstance()
        {
            OutputNPGSQL_InsertStatements ret = new OutputNPGSQL_InsertStatements();
            ret.config = this;
            return ret;
        }
    }
}
