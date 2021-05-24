using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Edonica.MapBase;
using System.Xml;
using System.Data.SqlClient;
using System.Data;
using Microsoft.SqlServer.Types;

namespace Edonica.XMLImport
{
    class OutputMSSQLTable : OutputGenericTable
    {
        OutputMSSQL outputProcessor;
        SqlBulkCopy sqlBulkCopy;

        DataTable SchemaTable;
        List<DataRow> RowsToWrite = new List<DataRow>();

        public void Flush()
        {

            if (!outputProcessor.config.DummyRun)
            {
                outputProcessor.Log(String.Format("Flusing {0}.  Rows : {1}", outputProcessor.config.TableNamePrefix + tableSchema.TableName, RowsToWrite.Count));
                sqlBulkCopy.WriteToServer(RowsToWrite.ToArray());
            }
            RowsToWrite.Clear();
        }

        public OutputMSSQLTable(OutputMSSQL processor, MapTableDefinition tableSchema)
            : base(tableSchema)
        {
            outputProcessor = processor;
        }

        public void InitTable(MapTableDefinition table)
        {
            sqlBulkCopy = new SqlBulkCopy(outputProcessor.conn);
            sqlBulkCopy.DestinationTableName = outputProcessor.config.TableNamePrefix + table.TableName;

            SchemaTable = new DataTable( outputProcessor.config.TableNamePrefix +  table.TableName );


            foreach( MapFieldDefinition f in table )
            {
                Type type = typeof(void);
                switch (f.Type )
                {
                    case MapFieldType.Null:
                        type = typeof(void);
                        break;
                    case MapFieldType.Int32:
                        type = typeof(Int32);
                        break;
                    case MapFieldType.Double:
                        type = typeof(Double);
                        break;
                    case MapFieldType.String:
                        type = typeof(String);
                        break;
                    case MapFieldType.Point:
                    case MapFieldType.Polyline:
                    case MapFieldType.Region:
                    case MapFieldType.Geometry:
                        type = outputProcessor.config.ConvertToWGS84 ? typeof( SqlGeography)  : typeof(SqlGeometry );
                        break;
                    case MapFieldType.XML:
                        type = typeof(XmlNode);
                        break;
                }
                SchemaTable.Columns.Add( f.Name,type );
            }
        }




        internal override void OutputRecord(XmlElement element)
        {
            DataRow row = SchemaTable.NewRow();

            for (int p = 0; p < tableSchema.Count; p++)
            {
                object value = null;
                MapFieldDefinition field = tableSchema[p];
                XmlNode node = element[field.Name];

                switch (field.Type)
                {
                    case MapFieldType.Int32:
                        value = Convert.ToInt32(node.InnerText);
                        break;
                    case MapFieldType.Double:
                        value = Convert.ToDouble(node.InnerText);
                        break;
                    case MapFieldType.String:
                        value = node.InnerText.Trim();
                        break;
                    case MapFieldType.Point:
                    case MapFieldType.Polyline:
                    case MapFieldType.Region:
                    case MapFieldType.Geometry:
                        {
                            try
                            {
                                MapGeometry geom = ReadGML.ReadXmlNode(node.FirstChild);
                                TransformGeometry(geom);
                                if (outputProcessor.config.ConvertToWGS84) //? typeof(SqlGeography) : typeof(SqlGeometry);
                                    value = MSSQLHelpers.MakeGeography(geom, 4326);
                                else
                                    value = MSSQLHelpers.MakeGeometry(geom, 27700);
                            }
                            catch (Exception ex)
                            {
                                String Error = String.Format("Error : {0}", ex.Message);
                                outputProcessor.Log(Error);
                            }
                        }
                        break;
                    case MapFieldType.XML:
                        break;
                }
               row[p]=value;

            }
            RowsToWrite.Add(row);

            if (RowsToWrite.Count >= outputProcessor.config.BufferSizeInRows)
            {
                Flush();
            }

        }



        public override OutputGeneric OutputProcessorGeneric
        {
            get { return outputProcessor; }
        }
    }
}
