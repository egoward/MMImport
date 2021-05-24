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
    /// Class representing a collection of lines
    /// </summary>
    public class MapGeometryMultiLineString : MapGeometry
    {
        /// <summary>
        /// Array of lines
        /// </summary>
        public MapGeometryPolyline[] lines;
        /// <summary>
        /// Write to WKB (minus byte order)
        /// </summary>
        /// <param name="writer"></param>
        public override void WriteWellKnownBinaryTo(BinaryWriter writer)
        {
            //Type - multi line string
            writer.Write((Int32)5);
            writer.Write((Int32)lines.Length);
            for (int c = 0; c < lines.Length; c++)
            {
                writer.Write(wkbByteOrder);
                lines[c].WriteWellKnownBinaryTo(writer);
            }
        }

        /// <summary>
        /// Apply an operation to every point in the collection
        /// </summary>
        /// <param name="op"></param>
        public override void ForEachPoint(PointOperation op)
        {
            foreach (MapGeometryPolyline line in lines)
            {
                line.ForEachPoint(op);
            }
        }

        public override Rect Extents
        {
            get 
            {
                Rect rect = lines[0].Extents;
                for (int c = 1; c < lines.Length; c++)
                {
                    rect.Union(lines[c].Extents);
                }
                return rect;
            }
        }
    }
}