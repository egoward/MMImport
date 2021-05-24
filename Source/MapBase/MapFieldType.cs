using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Edonica.MapBase
{
    /// <summary>
    ///The types we will attempt to store in our database.
    ///(We can't use DBType as that's not in Silverlight)
    /// </summary>
    public enum MapFieldType
    {
        /// <summary>
        /// Null field type (or error)
        /// </summary>
        Null,
        /// <summary>
        /// 32 bit integer
        /// </summary>
        Int32,
        /// <summary>
        /// double precision floating point
        /// </summary>
        Double,
        /// <summary>
        /// String - should have a size
        /// </summary>
        String,
        /// <summary>
        /// Point geometry (x,y)
        /// </summary>
        Point,
        /// <summary>
        /// Line or MultiLine items
        /// </summary>
        Polyline,
        /// <summary>
        /// Region items
        /// </summary>
        Region,
        /// <summary>
        /// Geometry items (of unknown type)
        /// </summary>
        Geometry,

        /// <summary>
        /// Lumps of XML
        /// </summary>
        XML
    }
}
