using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.IO;
using System.Xml.Schema;
using Edonica.MapBase;

/////////////////////////////////////////////
//Classes for defining database / map schema
//Base class is 'OutputSchema' which contains a list of tables
//Parsed via the System.Xml.Schema.XmlSchema reader.

namespace Edonica.XMLImport
{



    /// <summary>
    /// Object that represents the output schema.
    /// This is a list of named tables.
    /// </summary>
    public class ExportSchema
    {
        /// <summary>
        /// Dictionary of named tables.
        /// </summary>
        public Dictionary<string, MapTableDefinition> tables = new Dictionary<string, MapTableDefinition>();

        /// <summary>
        /// Is a type geometry
        /// </summary>
        /// <param name="outputType">Field type</param>
        /// <returns>True if type if geometric, otherwise false</returns>
        static public bool IsGeometry(MapFieldType outputType)
        {
            return outputType == MapFieldType.Point || outputType == MapFieldType.Polyline || outputType == MapFieldType.Region;
        }


        /// <summary>
        /// Read a schema file.  Schema is read via System.Xml.Schema.XmlSchema
        /// </summary>
        /// <param name="schemaFile">Filename</param>
        /// <param name="log">Location of log file</param>
        /// <returns>The parsed schema</returns>
        public static ExportSchema ReadSchema(string schemaFile, TextWriter log)
        {
            XmlSchema schema;

            ExportSchema ret = new ExportSchema(); ;
            using (XmlTextReader schemaReader = new XmlTextReader(schemaFile))
            {
                schema = XmlSchema.Read(schemaReader, delegate (object o, ValidationEventArgs args) {log.WriteLine(args.ToString() ); } );
            }
            XmlSchemaSet schemaSet = new XmlSchemaSet();
            schemaSet.Add(schema);
            schemaSet.Compile();


            foreach (XmlSchemaElement element in schema.Elements.Values)
            {
                MapTableDefinition table = new MapTableDefinition();
                table.TableName = element.Name;

                XmlSchemaComplexType complexType = element.ElementSchemaType as XmlSchemaComplexType;
                //We don't need any attributes!

                // Get the sequence particle of the complex type.
                XmlSchemaSequence sequence = complexType.ContentTypeParticle as XmlSchemaSequence;

                // Iterate over each XmlSchemaElement in the Items collection.
                foreach (XmlSchemaElement childElement in sequence.Items)
                {
                    MapFieldDefinition field = new MapFieldDefinition();
                    field.Name = childElement.Name;
                    XmlSchemaType schemaType = childElement.ElementSchemaType;
                    XmlTypeCode typecode = schemaType.TypeCode;
                    if (typecode == XmlTypeCode.Int)
                    {
                        field.Type = MapFieldType.Int32;
                    }
                    else if (typecode == XmlTypeCode.Double)
                    {
                        field.Type = MapFieldType.Double;
                    }
                    else if (typecode == XmlTypeCode.String)
                    {
                        XmlSchemaSimpleType simpleType = schemaType as XmlSchemaSimpleType;
                        XmlSchemaSimpleTypeRestriction restriction = simpleType.Content as XmlSchemaSimpleTypeRestriction;
                        if (restriction != null)
                        {
                            foreach (XmlSchemaObject facet in restriction.Facets)
                            {
                                XmlSchemaMaxLengthFacet maxLength = facet as XmlSchemaMaxLengthFacet;
                                if (maxLength != null)
                                {
                                    field.Size = Int32.Parse(maxLength.Value);
                                }
                            }
                        }
                        field.Type = MapFieldType.String;
                    }
                    else if (schemaType.Name == "GMLStub_Point")
                    {
                        field.Type = MapFieldType.Point;
                    }
                    else if (schemaType.Name == "GMLStub_Polyline")
                    {
                        field.Type = MapFieldType.Polyline;
                    }
                    else if (schemaType.Name == "GMLStub_Region")
                    {
                        field.Type = MapFieldType.Region;
                    }
                    else
                    {
                        throw new Exception("Unsupported type : " + typecode);
                    }
                    table.Add(field);
                }
                ret.tables.Add(table.TableName, table);
            }
            return ret;
        }
    }

}
