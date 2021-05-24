using System;
using System.Net;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.IO;

namespace Edonica.MapBase
{
    public class MapGeometryWKBReader
    {
        class WKBReaderException : Exception
        {
            public WKBReaderException(string s)
                : base(s)
            {
            }
        }
        public static MapGeometry ReadWKB(byte[] bytes)
        {
            MemoryStream ms = new MemoryStream(bytes);
            BinaryReader reader = new BinaryReader(ms);
            return ReadWKB(reader);
        }

        public static Point ReadPoint(BinaryReader reader)
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
                ret[c] = ReadPoint(reader);
            }
            return ret;

        }

        public static MapGeometry ReadWKB(BinaryReader reader)
        {
            byte byteOrder = reader.ReadByte();
            if (byteOrder != MapGeometry.wkbByteOrder)
                throw new WKBReaderException("Unsupported byte order");
            return ReadWKBWithKnownByteOrder(reader);
        }

        public static MapGeometry ReadWKBWithKnownByteOrder(BinaryReader reader)
        {
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
                            for (int c = 0; c < ret.innerBoundaries.Length; c++)
                            {
                                ret.innerBoundaries[c] = ReadPointArray(reader);
                            }
                        }
                        return ret;
                    }
                case 4:
                    {
                        //Not tested as of 11/11/2007
                        return new MapGeometryMultiPoint(ReadPointArray(reader));
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
