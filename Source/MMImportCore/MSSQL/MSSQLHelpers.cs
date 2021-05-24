using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

using Microsoft.SqlServer.Types;

using Edonica.MapBase;

namespace Edonica.XMLImport
{
    public static class MSSQLHelpers
    {

        public static void SQLGeomSinkAddLine(MapItemSink g, IEnumerable<Point> points)
        {
            IEnumerator<Point> i = points.GetEnumerator();

            if (!i.MoveNext())
                throw new Exception("No points in collection");
            Point first = i.Current;
            g.BeginFigure(first.X, first.Y, null, null);

            while (i.MoveNext())
            {
                Point current = i.Current;
                g.AddLine( current.X, current.Y,null,null);
            }

            g.EndFigure();
        }

        public static SqlGeometry MakeGeometry( MapGeometry geom , int SRID)
        {
            SqlGeometryBuilder builder = new SqlGeometryBuilder();
            builder.SetSrid(SRID);
            SQLGeomSinkAddMapGeometry(new MapItemSinkGeometry(builder), geom);
            return builder.ConstructedGeometry;
        }

        public static SqlGeography MakeGeography(MapGeometry geog, int SRID)
        {
            SqlGeographyBuilder builder = new SqlGeographyBuilder();
            builder.SetSrid(SRID);
            SQLGeomSinkAddMapGeometry(new MapItemSinkGeography(builder), geog);
            return builder.ConstructedGeography;
        }


        public static void SQLGeomSinkAddMapGeometry(MapItemSink g, MapGeometry geomIn)
        {
            
            if (geomIn is MapGeometryPoint)
            {
                MapGeometryPoint i = geomIn as MapGeometryPoint;
                g.BeginItem(OpenGisGeometryType.Point);
                g.BeginFigure(i.point.X, i.point.Y,null,null);
                g.EndFigure();
                g.EndItem();
            }
            else if (geomIn is MapGeometryPolyline)
            {
                MapGeometryPolyline i = geomIn as MapGeometryPolyline;
                g.BeginItem(OpenGisGeometryType.LineString);
                SQLGeomSinkAddLine( g, i.points );
                g.EndItem();
            }
            else if (geomIn is MapGeometryRegion)
            {
                MapGeometryRegion i = geomIn as MapGeometryRegion;
                g.BeginItem(OpenGisGeometryType.Polygon);
                SQLGeomSinkAddLine(g, GeomUtils.TidyLinearRing( i.outerBoundary,false ));
                //SQLGeomSinkAddLine(g, i.outerBoundary.Reverse(), true);
                if (i.innerBoundaries != null)
                {
                    foreach (Point[] o in i.innerBoundaries)
                    {
                        SQLGeomSinkAddLine(g, GeomUtils.TidyLinearRing( o,true ) );
                    }
                }
                g.EndItem();
            }
            else if (geomIn is MapGeometryMultiPoint)
            {
                MapGeometryMultiPoint i = geomIn as MapGeometryMultiPoint;
                g.BeginItem(OpenGisGeometryType.MultiPoint);
                foreach (Point p in i.points)
                {
                    g.BeginFigure(p.X, p.Y, null, null);
                    g.EndFigure();
                }
                g.EndItem();
            }
            else if (geomIn is MapGeometryMultiLineString)
            {
                MapGeometryMultiLineString i = geomIn as MapGeometryMultiLineString;
                g.BeginItem(OpenGisGeometryType.MultiLineString);
                foreach (MapGeometryPolyline o in i.lines)
                {
                    SQLGeomSinkAddLine(g, o.points );
                }
                g.EndItem();
            }
            else if (geomIn is MapGeometryMultiRegion)
            {
                MapGeometryMultiRegion i = geomIn as MapGeometryMultiRegion;
                foreach (MapGeometryRegion r in i.regions)
                {
                    SQLGeomSinkAddMapGeometry(g, r);
                }
            }
        }

        public abstract class MapItemSink
        {
            public abstract void AddLine(double x, double y, double? z, double? m);
            public abstract void BeginFigure(double x, double y, double? z, double? m);
            public abstract void BeginItem(OpenGisGeometryType type);
            public abstract void EndFigure();
            public abstract void EndItem();
            public abstract void SetSrid(int srid);
        }

        public class MapItemSinkGeometry : MapItemSink
        {
            IGeometrySink110 sink;
            public MapItemSinkGeometry(IGeometrySink110 sink)
            {
                this.sink = sink;
            }
            public override void AddLine(double x, double y, double? z, double? m)
            {
                sink.AddLine(x, y, z, m);
            }
            public override void BeginFigure(double x, double y, double? z, double? m)
            {
                sink.BeginFigure(x, y, z, m);
            }
            public override void BeginItem(OpenGisGeometryType type)
            {
                sink.BeginGeometry(type);
            }
            public override void EndFigure()
            {
                sink.EndFigure();
            }
            public override void EndItem()
            {
                sink.EndGeometry();
            }
            public override void SetSrid(int srid)
            {
                sink.SetSrid(srid);
            }
        }
        public class MapItemSinkGeography : MapItemSink
        {
            IGeographySink110 sink;
            public MapItemSinkGeography(IGeographySink110 sink)
            {
                this.sink = sink;
            }
            public override void AddLine(double x, double y, double? z, double? m)
            {
                sink.AddLine(x, y, z, m);
            }
            public override void BeginFigure(double x, double y, double? z, double? m)
            {
                sink.BeginFigure(x, y, z, m);
            }
            public override void BeginItem(OpenGisGeometryType type)
            {
                sink.BeginGeography((OpenGisGeographyType)type);
            }
            public override void EndFigure()
            {
                sink.EndFigure();
            }
            public override void EndItem()
            {
                sink.EndGeography();
            }
            public override void SetSrid(int srid)
            {
                sink.SetSrid(srid);
            }
        }

    }
}
