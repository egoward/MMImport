using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Xml.Xsl;
using System.IO;
using System.Diagnostics;
using Edonica.MapBase;

namespace Edonica.XMLImport
{
    /// <summary>
    /// Generic output processor for a single table.
    /// The 'OutputGeneric' class contains a list of these
    /// This might connect to an individual file or a table within a database
    /// </summary>
    public abstract class OutputGenericTable
    {
        /// <summary>
        /// Constructor - attaches to the output processor and the schema
        /// </summary>
        /// <param name="tableSchema"></param>
        public OutputGenericTable(MapTableDefinition tableSchema)
        {
            this.tableSchema = tableSchema;
            //this.outputProcessor = outputProcessor;
            for (int c = 0; c < tableSchema.Count; c++)
            {
                mapNameToLocation.Add(tableSchema[c].Name, c);
            }
        }

        /// <summary>
        /// Dictionary that gets us from a column name to the location in our list of fields.
        /// </summary>
        public Dictionary<string, int> mapNameToLocation = new Dictionary<string, int>();

        /// <summary>
        /// Overriden by a concrete implementation to actually do the output.
        /// </summary>
        /// <param name="element"></param>
        internal abstract void OutputRecord(XmlElement element);

        /// <summary>
        /// Generic output processor
        /// </summary>
        public abstract OutputGeneric OutputProcessorGeneric { get; }

        /// <summary>
        /// The table in the schema that this corresponds to.
        /// </summary>
        public MapTableDefinition tableSchema;
        /// <summary>
        /// Convenience logging function so we know where we are.
        /// </summary>
        /// <param name="s"></param>
        public void Log(string s)
        {
            OutputProcessorGeneric.Log(tableSchema.TableName + " : " + s);
        }
        /// <summary>
        /// Locate a text child for a node and return the contents
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        string GetStringFromNode(XmlNode node)
        {
            foreach (XmlNode child in node.ChildNodes)
            {
                if (child.NodeType == XmlNodeType.Text)
                    return child.Value;
            }
            return "";
        }

        static byte HexIntToChar(int i)
        {
            if (i < 10)
                return (byte)('0' + i);
            else
                return (byte)(('A' -10) + i);
        }


        /// <summary>
        /// Convert some bytes into hex.  This is used for loading well known binary via text files.
        /// </summary>
        /// <param name="bytes"></param>
        /// <returns></returns>
        public static string BytesToString(byte[] bytes)
        {
            byte[] hexBytes = new byte[bytes.Length * 2];
            for (int c = 0; c < bytes.Length; c++)
            {
                byte b = bytes[c];
                int iLow = b & 0xF;
                int iHigh = b >> 4;

                hexBytes[c * 2] = HexIntToChar(iHigh);
                hexBytes[c * 2 + 1] = HexIntToChar(iLow);
            }

            return System.Text.ASCIIEncoding.ASCII.GetString(hexBytes);
        }

        public void ReadValues(XmlElement rootElement, string[] values)
        {
            MapGeometry junk;
            ReadValues(rootElement, values, out junk);
        }

        /// <summary>
        ///Read values in from our XML node into a list of strings.
        /// rootElement is an output from the XSL transform
        /// 'values' parameter must be initialised to an array of the appropriate size.
        /// </summary>
        /// <param name="rootElement"></param>
        /// <param name="values"></param>
        /// <returns></returns>
        public void ReadValues(XmlElement rootElement, string[] values, out MapGeometry geom)
        {
            geom = null;
            foreach (XmlNode node in rootElement.ChildNodes)
            {
                XmlElement valueNode = node as XmlElement;
                if (valueNode == null)
                    continue;
                string name = valueNode.LocalName;
                int fieldIndex = -1;
                if (!mapNameToLocation.TryGetValue(name, out fieldIndex))
                {
                    Log("Unrecognised element : " + name);
                    continue;
                }
                MapFieldType outputType = tableSchema[fieldIndex].Type;
                string value = "";

                if (outputType == MapFieldType.Int32)
                {
                    value = GetStringFromNode(node);
                }
                else if (outputType == MapFieldType.Double)
                {
                    value = GetStringFromNode(node);
                }
                else if (outputType == MapFieldType.String)
                {
                    //Quotes?!
                    value = GetStringFromNode(node);
                    //value = node.InnerText;
                }
                else if (ExportSchema.IsGeometry(outputType))
                {
                    geom = ReadGML.ReadXmlNode(node.FirstChild);
                    TransformGeometry(geom);
                    byte[] wkb = geom.AsWellKnownBinary();
                    value = BytesToString(wkb);
                }
                values[fieldIndex] = value;
            }

        }

        /// <summary>
        /// Perform any required transformations for this table.
        /// By default this is defered to the output processor.
        /// </summary>
        /// <param name="geom"></param>
        public void TransformGeometry(MapGeometry geom)
        {
            OutputProcessorGeneric.TransformGeometry(geom);
        }

    }
}