using System;
using System.Net;
using System.Windows;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Edonica.MapBase
{
    /// <summary>
    /// Simple abstraction of a table - derives from a list of field and also have a name.
    /// </summary>
    [XmlRoot("MapTableDefinition")]
    public class MapTableDefinition : List<MapFieldDefinition>
    {
        /// <summary>
        /// The name of the table
        /// </summary>
        public string TableName;

        public int IndexOfField(string name)
        {
            for (int c = 0; c < Count; c++)
            {
                if (String.Compare(this[c].Name, name, StringComparison.CurrentCultureIgnoreCase) == 0)
                    return c;
            }
            return -1;

        }
    }
}
