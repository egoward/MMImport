///////////////////////////////////////////////////
//Copyright Edonica

using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Collections.ObjectModel;
using System.IO;
using System.Windows;

namespace Edonica.MapBase
{
    /// <summary>
    /// MapGeometry representing a point on the map
    /// </summary>
    public class MapGeometryPoint : MapGeometry
    {
        /// <summary>
        /// Simple constructor
        /// </summary>
        /// <param name="p"></param>
        public MapGeometryPoint(Point p)
        {
            this.point = p;
        }
        /// <summary>
        /// Convert to WKB (minus byte order)
        /// </summary>
        /// <param name="writer"></param>
        public override void WriteWellKnownBinaryTo(BinaryWriter writer)
        {
            //Type - point
            writer.Write((int)1);
            WritePoint(writer, point);
        }

        /// <summary>
        /// Apply an operator to the point (overriden)
        /// </summary>
        /// <param name="op"></param>
        public override void ForEachPoint(MapGeometry.PointOperation op)
        {
            op(ref point);
        }
        /// Get the extents of the item
        /// </summary>
        public override Rect Extents
        {
            get { return new Rect(point, new Point()); }
        }



        /// <summary>
        /// The point in question
        /// </summary>
        public Point point;
    }
}