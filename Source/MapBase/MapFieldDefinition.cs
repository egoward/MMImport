using System;
using System.Net;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Xml;
using System.Xml.Serialization;

namespace Edonica.MapBase
{
    /// <summary>
    /// Simple abstraction of a database field
    /// </summary>
    public class MapFieldDefinition
    {
        /// <summary>
        /// Name of the field
        /// </summary>
        [XmlAttribute]
        public string Name { get; set; }
        /// <summary>
        /// Type of the field
        /// </summary>
        [XmlAttribute]
        public MapFieldType Type { get; set; }
        /// <summary>
        /// If type is String, this should contain the maximum length of the string in characters
        /// </summary>
        [XmlAttribute]
        public int Size { get; set; }

        public override string ToString()
        {
            if (Size == 0)
            {
                return String.Format("{0} {1}", Type.ToString(), Name);
            }
            else
            {
                return String.Format("{0}({1}) {2}", Type.ToString(), Size, Name);
            }
        }
#if false
        //Thought we needed this to replace a lacking XmlSerialization
        XElement AsXElement
        {
            get
            {
                XElement ret = new XElement("Field");
                ret.Add( new XAttribute("Name", Name) );
                ret.Add(new XAttribute("Type", Type));
                if (Size > 0)
                    ret.Add(new XAttribute("Size", Size));
                return ret;
            }
            set
            {
                Name = value.Attribute("Name").Value;
                Type = (MapFieldType)Enum.Parse(typeof(MapFieldType), value.Attribute("Type").Value);
                XAttribute sizeAttribute = value.Attribute("Size");
                if (sizeAttribute != null)
                    Size = Int32.Parse(sizeAttribute.Value);
            }
        }
         #endif

    }

}
