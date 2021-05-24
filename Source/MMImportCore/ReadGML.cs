///////////////////////////////////////////////////
//Copyright Edonica

using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Collections.ObjectModel;
using System.IO;
using Edonica.GIS;

namespace Edonica.XMLImport
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
            foreach( Point p in points )
            {
                WritePoint( writer, p );
            }
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
        public abstract void ForEachPoint( PointOperation op );

    }

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
            WritePoint( writer, point );
        }

        /// <summary>
        /// Apply an operator to the point (overriden)
        /// </summary>
        /// <param name="op"></param>
        public override void ForEachPoint(MapGeometry.PointOperation op)
        {
            op(ref point);
        }


        /// <summary>
        /// The point in question
        /// </summary>
        public Point point;
    }
    /// <summary>
    /// A simple multipoint line.
    /// </summary>
    public class MapGeometryPolyline : MapGeometry
    {
        /// <summary>
        /// Simple constructor
        /// </summary>
        /// <param name="points"></param>
        public MapGeometryPolyline(Point[] points)
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
            writer.Write((int)2);
            WritePointArray(writer, points);
        }

        /// <summary>
        /// Apply an operator to every point in the line
        /// </summary>
        /// <param name="op"></param>
        public override void ForEachPoint(MapGeometry.PointOperation op)
        {
            for(int c=0;c<points.Length;c++)
            {
                op(ref points[c]);
            }
        }
        /// <summary>
        /// The points
        /// </summary>
        public Point[] points;
    }

    /// <summary>
    /// A multipoint item.
    /// </summary>
    public class MapGeometryMultipoint : MapGeometry
    {
        /// <summary>
        /// Simple constructor
        /// </summary>
        /// <param name="points"></param>
        public MapGeometryMultipoint(Point[] points)
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
        /// <summary>
        /// The points
        /// </summary>
        public Point[] points;
    }


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
                foreach( Point[] inner in innerBoundaries )
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
            for(int c=0;c<outerBoundary.Length;c++)
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


    }

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
    }

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
                region.ForEachPoint( op );
            }
        }


    }


    /// <summary>
    /// Static class (you can't construct one) that contains functions for reading GML, providing it complies with a load of things.
    /// </summary>
    public static class ReadGML
    {
        /// <summary>
        /// Read an XML node as a GML geometry.  None name should be Point, LineString, Polygon, MultiLineString.
        /// Throws an exception if the node can't be parsed
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        public static MapGeometry ReadXmlNode(XmlNode node)
        {
            if (node == null)
                throw new Exception("Null node encountered");

            string localName = node.LocalName;
            if (localName == "Point")
                return ParsePoint(node);
            if (localName == "LineString")
                return new MapGeometryPolyline(ParseCoordinateList(node));
            if (localName == "Polygon")
                return ParsePolygon(node);
            if (localName == "MultiLineString")
                return ParseMultiLineString(node);
            else
                throw new Exception("Unrecognised GML node " + node.LocalName);
        }


        /// <summary>
        /// Parse a single point from a gml:coordinates node.
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        static MapGeometry ParsePoint(XmlNode node)
        {
            Point[] points = ReadCoords(node["gml:coordinates"]);
            if (points.Length != 1)
                throw new Exception("Expected exactly one point in coordinates");
            return new MapGeometryPoint(points[0]);
        }

        /// <summary>
        /// Parse a polygon.  Node should have a 'outerBoundaryIs' child.
        /// Throws an exception if it isn't right.
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        static MapGeometryRegion ParsePolygon(XmlNode node)
        {
            Point[] outer = null;
            List<Point[]> inners = new List<Point[]>();
            foreach (XmlNode child in node.ChildNodes)
            {
                if (child.LocalName == "outerBoundaryIs")
                {
                    outer = ParseCoordinateList(child["gml:LinearRing"]);
                }
                else if (child.LocalName == "innerBoundaryIs")
                {
                    inners.Add(ParseCoordinateList(child["gml:LinearRing"]));
                }
            }
            if (outer == null)
                throw new Exception("No ounerBoundaryIs in polygon node");

            MapGeometryRegion ret = new MapGeometryRegion();
            ret.outerBoundary = outer;
            if (inners.Count != 0)
            {
                ret.innerBoundaries = inners.ToArray();
            }
            return ret;
        }

        /// <summary>
        /// Parse a multi line string.
        /// element must have a lineStringMember child.
        /// Throws an exception if it's not valid.
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        static MapGeometryMultiLineString ParseMultiLineString(XmlNode node)
        {
            List<MapGeometryPolyline> lines = new List<MapGeometryPolyline>();
            foreach (XmlNode child in node.ChildNodes)
            {
                if (child.LocalName == "lineStringMember")
                {
                    lines.Add(new MapGeometryPolyline(ParseCoordinateList(child["gml:LineString"])) );
                }
            }
            if( lines.Count == 0 )
                throw new Exception("No listStringMember(s) found in MultiLineString");

            MapGeometryMultiLineString ret = new MapGeometryMultiLineString();
            ret.lines = lines.ToArray();
            return ret;

        }

        /// <summary>
        /// Read a list of coordinates from a gml:coordinates node.
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        static Point[] ParseCoordinateList(XmlNode node)
        {
            return ReadCoords(node["gml:coordinates"]);
        }

        /// <summary>
        /// Read a list of coordinates from a node.  This is done based on the first text child.
        /// Throws an exception if it's not valid.
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        static Point[] ReadCoords(XmlNode node)
        {
            if (node == null)
                throw new Exception("Unable to locate coordinates");
            //Should do faffling with <X> and <Y> elements.
            string text = node.ChildNodes[0].Value;
            string[] coords = text.Split(new char[]{ ' ', '\t', '\n', '\r'}, StringSplitOptions.RemoveEmptyEntries);
            Point[] ret = new Point[coords.Length];
            for (int c = 0; c < coords.Length; c++)
            {
                ret[c] = ParsePoint(coords[c]);
            }
            return ret;

        }

        /// <summary>
        /// Parse a single point from a string.  Comma is the delimiter.
        /// Throws an exception if there aren't two numbers or if they can't be parsed. 
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        static Point ParsePoint(string s)
        {
            string[] bits = s.Split(',');
            if (bits.Length != 2)
            {
                throw new Exception("Invalid point string encountered : " + s);
            }
            return new Point(Double.Parse(bits[0]), Double.Parse(bits[1]));
        }

    }

    public class MapGeometryWKBReader
    {
        class WKBReaderException : Exception
        {
            public WKBReaderException( string s ) : base( s )
            {
            }
        }
        public static MapGeometry ReadWKB( byte[] bytes )
        {
            MemoryStream ms = new MemoryStream( bytes );
            BinaryReader reader = new BinaryReader( ms );
            return ReadWKB( reader );
        }

        public static Point ReadPoint( BinaryReader reader )
        {
            double x = reader.ReadDouble();
            double y = reader.ReadDouble();
            return new Point(x, y);
        }

        public static Point[] ReadPointArray(BinaryReader reader)
        {
            int count = reader.ReadInt32();
            if (count < 0 || count > 128 * 1024 * 1024)
                throw new WKBReaderException("Invalid number of points in point array");

            Point[] ret = new Point[count];
            for (int c = 0; c < count; c++)
            {
                ret[c] = ReadPoint( reader );
            }
            return ret;

        }

        public static MapGeometry ReadWKB(BinaryReader reader)
        {
            byte byteOrder = reader.ReadByte();
            if( byteOrder != MapGeometry.wkbByteOrder )
                throw new WKBReaderException("Unsupported byte order");

            int objectType = reader.ReadInt32();
            switch (objectType)
            {
                case 1:
                    {
                        Point p = ReadPoint(reader);
                        return new MapGeometryPoint(p);
                    }
                case 2:
                    {
                        return new MapGeometryPolyline(ReadPointArray(reader));
                    }
                case 3:
                    {
                        MapGeometryRegion ret = new MapGeometryRegion();
                        int numRings = reader.ReadInt32();
                        if (numRings < 1 || numRings > 128 * 1024 * 1024)
                            throw new WKBReaderException("Invalid number of rings in region");
                        ret.outerBoundary = ReadPointArray(reader);

                        if (numRings > 1)
                        {
                            ret.innerBoundaries = new Point[numRings - 1][];
                            for(int c=0;c<ret.innerBoundaries.Length;c++)
                            {
                                ret.innerBoundaries[c] = ReadPointArray(reader);
                            }
                        }
                        return ret;
                    }
                case 4:
                    {
                        //Not tested as of 11/11/2007
                        return new MapGeometryMultipoint(ReadPointArray(reader));
                    }
                case 5:
                    {
                        MapGeometryMultiLineString ret = new MapGeometryMultiLineString();
                        int numLines = reader.ReadInt32();
                        if (numLines < 1 || numLines > 128 * 1024 * 1024)
                            throw new WKBReaderException("Invalid number of lines in MapGeometryMultiLineString");
                        ret.lines = new MapGeometryPolyline[numLines];
                        for (int c = 0; c < numLines; c++)
                        {
                            byte objectType2 = reader.ReadByte();
                            int itemType = reader.ReadInt32(); //Will be 2
                            ret.lines[c] = new MapGeometryPolyline(ReadPointArray(reader));
                        }
                        return ret;
                    }
                case 6:
                    {
                        MapGeometryMultiRegion ret = new MapGeometryMultiRegion();
                        int numRegions = reader.ReadInt32();
                        if (numRegions < 1 || numRegions > 128 * 1024 * 1024)
                            throw new WKBReaderException("Invalid number of regions in MapGeometryMultiRegion");
                        ret.regions = new MapGeometryRegion[numRegions];
                        for (int c = 0; c < numRegions; c++)
                        {
                            ret.regions[c] = (MapGeometryRegion)ReadWKB(reader);
                        }
                        return ret;
                    }
                default:
                    throw new WKBReaderException("Unsupported WKB object type : " + objectType);

            }

                

        }
    }
}
