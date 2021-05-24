using System;
using System.Collections.Generic;
using System.Text;
using OSGeo.OGR;
using System.IO;
using Edonica.XMLImport;
using Edonica.MapBase;

namespace MMImportCore
{
    public enum KnownProjection
    {
        WGS84,
        OSGB1936
    };
    public class TransformUtils
    {
        public static void TransformGeometry(TextWriter writer, string sourceDriver, string sourceMapA, string sourceMapB, string targetDriver, string targetMapA, string targetMapB, MapGeometry.PointOperation transformOp)
        {
            OGRHelper.EnsureInitialised();

            writer.WriteLine("Getting source driver");
            Driver driverSource = Ogr.GetDriverByName(sourceDriver);

            if (driverSource == null)
                throw new Exception("Unable to locate GDAL driver [" + sourceDriver + "]");

            writer.WriteLine("Opening map " + sourceMapA);
            using (DataSource dsSource = driverSource.Open(sourceMapA, 0))
            {

                if (dsSource == null)
                    throw new Exception("Unable to locate data source [" + sourceMapA + "]");
                //if (dsSource.GetLayerCount() != 1)
                //{
                //    throw new Exception("Expected a single layer in source map");
                //}
                writer.WriteLine("Inspecting source");
                using (Layer layerSource = dsSource.GetLayerByName(sourceMapB))
                {
                    if (layerSource == null)
                        throw new Exception("Unable to locate layer [" + sourceMapB + "]");

                    FeatureDefn defSource = layerSource.GetLayerDefn();
                    wkbGeometryType geomType = defSource.GetGeomType();

                    writer.WriteLine("Getting target driver");
                    Driver driverTarget = Ogr.GetDriverByName(targetDriver);
                    writer.WriteLine("Creating target data source");
                    using (DataSource dsTarget = driverTarget.CreateDataSource(targetMapA, new string[] { }))
                    {
                        if (dsTarget == null)
                            throw new Exception("Unable to locate GDAL driver [" + targetMapA + "]");

                        writer.WriteLine("Creating target layer " + targetMapB);
                        using (Layer layerTarget = dsTarget.CreateLayer(targetMapB, null, geomType, new string[] { }))
                        {
                            for (int f = 0; f < defSource.GetFieldCount(); f++)
                            {
                                FieldDefn sourceDef = defSource.GetFieldDefn(f);
                                writer.WriteLine("Adding field " + sourceDef.GetName());
                                FieldDefn targetDef = new FieldDefn(sourceDef.GetName(), sourceDef.GetFieldType());
                                if (sourceDef.GetFieldType() == FieldType.OFTString)
                                {
                                    targetDef.SetWidth(sourceDef.GetWidth());
                                }
                                layerTarget.CreateField(targetDef, 1);
                            }


                            int counter = 0;
                            while (true)
                            {
                                using (Feature sourceFeature = layerSource.GetNextFeature())
                                {
                                    if (sourceFeature == null)
                                        break;

                                    using (Feature targetFeature = new Feature(layerTarget.GetLayerDefn()))
                                    {
                                        targetFeature.SetFrom(sourceFeature, 1);

                                        Geometry ogrGeometry = sourceFeature.GetGeometryRef();

                                        //16Mb is big enough, you would think.
                                        byte[] wkbSource = new byte[16 * 1024 * 1024];
                                        ogrGeometry.ExportToWkb(wkbSource, wkbByteOrder.wkbNDR);
                                        MapGeometry geom = MapGeometryWKBReader.ReadWKB(wkbSource);
                                        geom.ForEachPoint(transformOp);
                                        byte[] wkbTransformed = geom.AsWellKnownBinary();
                                        using (Geometry geomTransformed = Geometry.CreateFromWkb(wkbTransformed))
                                        {
                                            targetFeature.SetGeometry(geomTransformed);
                                        }
                                        layerTarget.CreateFeature(targetFeature);
                                    }
                                    counter++;

                                    if (counter % 1000 == 0)
                                    {
                                        writer.WriteLine("Copying item " + counter);
                                    }
                                }

                            }


                        }

                    }

                    writer.WriteLine("All done");
                }
            }
        }
    }
}
