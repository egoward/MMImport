///////////////////////////////////////////////////
//Copyright Edonica

using System;
using System.Collections.Generic;
using System.Xml;
using System.Windows;

namespace Edonica.MapBase
{
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

}
