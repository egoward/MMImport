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
    class OutputNPGSQL_InsertStatementsTable : OutputGenericTable
    {
        public OutputNPGSQL_InsertStatementsTable(OutputNPGSQL_InsertStatements processor, MapTableDefinition tableSchema)
            : base(tableSchema)
        {
            outputProcessor = processor;
        }

        OutputNPGSQL_InsertStatements outputProcessor;

        public override OutputGeneric OutputProcessorGeneric
        {
            get { return outputProcessor; }
        }

        public void InitTable(MapTableDefinition table)
        {
            List<string> fieldNames = table.ConvertAll<string>(delegate(MapFieldDefinition f) { return f.Name; });
            int paramcount = 0;
            List<string> paramNames = table.ConvertAll<string>(delegate(MapFieldDefinition f) { paramcount++; return ":" + f.Name; });
            //List<string> paramNames = table.ConvertAll<string>(delegate(OutputField f) { return "?"; });

            string commandText = "INSERT INTO " + outputProcessor.config.TableNamePrefix + table.TableName + " (" + OutputProcessorNPGSQL_Base.MakeCSVLine(fieldNames) + " ) VALUES (" + OutputProcessorNPGSQL_Base.MakeCSVLine(paramNames) + ")";

            command = outputProcessor.CreateCommand();
            command.CommandText = commandText;

            paramcount = 0;
            List<IDbDataParameter> TheParams = new List<IDbDataParameter>();
            foreach (MapFieldDefinition f in table)
            {
                paramcount++;
                IDbDataParameter param = command.CreateParameter();
                param.ParameterName = f.Name;
                TheParams.Add(param);

                command.Parameters.Add(param);
            }


            parameters = TheParams.ToArray();
        }


        public IDbCommand command;
        public IDataParameter[] parameters;
        internal override void OutputRecord(XmlElement element)
        {
            for (int p = 0; p < tableSchema.Count; p++)
            {
                object value = null;
                MapFieldDefinition field = tableSchema[p];
                MapFieldType type = field.Type;
                DbType dbType = DbType.String;
                if (type != MapFieldType.XML)
                {

                    //XmlNodeList nodes = doc.DocumentElement.GetElementsByTagName(field.Name);
                    //if (nodes.Count == 1)
                    XmlNode node = element[field.Name];
                    if (node != null)
                    {
                        //XmlNode node = nodes[0];

                        if (type == MapFieldType.Int32)
                        {
                            dbType = DbType.Int32;
                            if (!String.IsNullOrEmpty(node.InnerText))
                            {
                                value = Convert.ToInt32(node.InnerText);
                            }
                        }
                        else if (type == MapFieldType.Double)
                        {
                            dbType = DbType.Double;
                            if (!String.IsNullOrEmpty(node.InnerText))
                            {
                                value = Convert.ToDouble(node.InnerText);
                            }
                        }
                        else if (type == MapFieldType.String)
                        {
                            dbType = DbType.String;
                            value = Convert.ToString(node.InnerText);
                        }
                        else if (ExportSchema.IsGeometry(type))
                        {
                            dbType = DbType.String;

                            MapGeometry geom = ReadGML.ReadXmlNode(node.FirstChild);
                            TransformGeometry(geom);
                            value = geom.AsWellKnownBinary();

                        }

                    }
                    else
                    {
                        throw new Exception("Unable to locate field " + field.Name);
                    }

                }

                parameters[p].Value = value;
                parameters[p].DbType = dbType;
            }
            //parameters[2].Value = DBNull.Value;
            command.ExecuteNonQuery();
        }
    }
}