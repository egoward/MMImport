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
    //TODO: Rename 'MapGeometryRegion' to 'MapGeometryPolygon' (in line with OGC)?

    /// <summary>
    /// MapRegion class - an outer polygon that can have holes in it.
    /// </summary>
    public class MapGeometryRegion : MapGeometry
    {
        /// <summary>
        /// The outer boundary of the polygon
        /// </summary>
        public Point[] outerBoundary;
        /// <summary>
        /// Any holes, or null if there are no holes
        /// </summary>
        public Point[][] innerBoundaries;

        /// <summary>
        /// Convert to WKB (minus byte order)
        /// </summary>
        /// <param name="writer"></param>
        public override void WriteWellKnownBinaryTo(BinaryWriter writer)
        {
            //Type - regular polygon
            writer.Write((Int32)3);
            int numRings = innerBoundaries == null ? 1 : (innerBoundaries.Length + 1);
            writer.Write((Int32)numRings);
            WritePointArray(writer, outerBoundary);
            if (innerBoundaries != null)
            {
                foreach (Point[] inner in innerBoundaries)
                {
                    WritePointArray(writer, inner);
                }

            }
        }

        /// <summary>
        /// Apply an operation to every point in the region.
        /// </summary>
        /// <param name="op"></param>
        public override void ForEachPoint(PointOperation op)
        {
            for (int c = 0; c < outerBoundary.Length; c++)
            {
                op(ref outerBoundary[c]);
            }

            if (innerBoundaries != null)
            {
                foreach (Point[] innerBoundary in innerBoundaries)
                {
                    for (int c = 0; c < innerBoundary.Length; c++)
                    {
                        op(ref innerBoundary[c]);
                    }
                }
            }
        }

        public override Rect Extents
        {
            get
            {
                return ExtentsOfPointArray(outerBoundary);
            }
        }





    }
}