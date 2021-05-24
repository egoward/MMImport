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
    /// A multipoint item.
    /// </summary>
    public class MapGeometryMultiPoint : MapGeometry
    {
        /// <summary>
        /// Simple constructor
        /// </summary>
        /// <param name="points"></param>
        public MapGeometryMultiPoint(Point[] points)
        {
            this.points = points;
        }
        /// <summary>
        /// Convert to WKB (minus byte order)
        /// </summary>
        /// <param name="writer"></param>
        public override void WriteWellKnownBinaryTo(BinaryWriter writer)
        {
            //Type - line string
            writer.Write((int)4);
            WritePointArray(writer, points);
        }

        /// <summary>
        /// Apply an operator to every point in the line
        /// </summary>
        /// <param name="op"></param>
        public override void ForEachPoint(MapGeometry.PointOperation op)
        {
            for (int c = 0; c < points.Length; c++)
            {
                op(ref points[c]);
            }
        }

        public override Rect Extents
        {
            get { return ExtentsOfPointArray(points); }
        }

        /// <summary>
        /// The points
        /// </summary>
        public Point[] points;
    }


}