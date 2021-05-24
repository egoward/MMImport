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
    /// MapGeometryMultiRegion class - a number of distinct regions
    /// </summary>
    public class MapGeometryMultiRegion : MapGeometry
    {
        /// <summary>
        /// The outer boundary of the polygon
        /// </summary>
        public MapGeometryRegion[] regions;

        /// <summary>
        /// Convert to WKB (minus byte order)
        /// </summary>
        /// <param name="writer"></param>
        public override void WriteWellKnownBinaryTo(BinaryWriter writer)
        {
            //Type - regular polygon
            writer.Write((Int32)6);
            writer.Write((Int32)regions.Length);
            for (int c = 0; c < regions.Length; c++)
            {
                writer.Write(wkbByteOrder);
                regions[c].WriteWellKnownBinaryTo(writer);
            }
        }

        /// <summary>
        /// Apply an operation to every point in the region.
        /// </summary>
        /// <param name="op"></param>
        public override void ForEachPoint(PointOperation op)
        {
            foreach (MapGeometryRegion region in regions)
            {
                region.ForEachPoint(op);
            }
        }

        public override Rect Extents
        {
            get
            {
                Rect rect = regions[0].Extents;
                for (int c = 1; c < regions.Length; c++)
                {
                    rect.Union(regions[c].Extents);
                }
                return rect;
            }
        }

    }

}