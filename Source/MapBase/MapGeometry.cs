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
    /// Base class for geometry items in a random coordinate space
    /// </summary>
    public abstract class MapGeometry
    {
        //Intel x86 type byte order
        internal static byte wkbByteOrder = 1;

        /// <summary>
        /// Convert the type to binary
        /// </summary>
        /// <returns></returns>
        public byte[] AsWellKnownBinary()
        {
            using (MemoryStream ms = new MemoryStream())
            {
                using (BinaryWriter bw = new BinaryWriter(ms))
                {
                    //Byte order...
                    bw.Write(wkbByteOrder);
                    WriteWellKnownBinaryTo(bw);
                    return ms.ToArray();
                }
            }
        }

        /// <summary>
        /// Write a point item in WKB form
        /// </summary>
        /// <param name="writer"></param>
        /// <param name="point"></param>
        public static void WritePoint(BinaryWriter writer, Point point)
        {
            writer.Write((double)point.X);
            writer.Write((double)point.Y);
        }
        /// <summary>
        /// Write a list of points in WKB form
        /// </summary>
        /// <param name="writer"></param>
        /// <param name="points"></param>
        public static void WritePointArray(BinaryWriter writer, Point[] points)
        {
            writer.Write((UInt32)points.Length);
            foreach (Point p in points)
            {
                WritePoint(writer, p);
            }
        }

        public static Rect ExtentsOfPointArray(Point[] points)
        {
            if (points.Length == 1)
                return new Rect(points[0], new Point());
            Rect rect = new Rect(points[0], points[1]);
            for (int c = 2; c < points.Length; c++)
            {
                rect.Union(points[c]);
            }
            return rect;
        }


        /// <summary>
        /// Overwridden to write the geometry.
        /// Note that the byte order will already have been written
        /// </summary>
        /// <param name="writer"></param>
        public abstract void WriteWellKnownBinaryTo(BinaryWriter writer);

        /// <summary>
        /// Delegate definition for a function that applies to a single point.
        /// </summary>
        /// <param name="p">The point</param>
        public delegate void PointOperation(ref Point p);

        /// <summary>
        /// Apply an operation to every opoint in the geometry.
        /// </summary>
        /// <param name="op"></param>
        public abstract void ForEachPoint(PointOperation op);

        public abstract Rect Extents { get; }


    }
}