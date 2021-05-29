using System;
using System.Collections.Generic;
using System.Text;
using Edonica.MapBase;
using OSGeo.OGR;
using Edonica.XMLImport;

namespace MMImportCore.OGR
{
    public class OutputOGRTable : OutputGenericTable
    {
        internal OutputOGR outputProcessor;

        internal OutputOGRTable(OutputOGR ogr, MapTableDefinition table) : base( table )
        {
            outputProcessor = ogr;
        }


        MapFieldType geomTypeInternal;
        wkbGeometryType geomTypeWKB;

        Layer layer = null;

        //Open up handles
        internal void Init( DataSource dataSource, string sourceId )
        {

            int geometryFieldIndex = tableSchema.FindIndex(delegate(MapFieldDefinition d) { return ExportSchema.IsGeometry(d.Type); });
            if (geometryFieldIndex < 0 )
            {
                Log("No geometry fields detected - skipping table");
                return;
            }
            MapFieldDefinition geometryField = tableSchema[ geometryFieldIndex ];

            geomTypeInternal = geometryField.Type;
            switch (geomTypeInternal)
            {
                case MapFieldType.Point: geomTypeWKB = wkbGeometryType.wkbPoint; break;
                case MapFieldType.Polyline: geomTypeWKB = wkbGeometryType.wkbLineString; break;
                case MapFieldType.Region: geomTypeWKB = wkbGeometryType.wkbMultiPolygon; break;
                default: throw new Exception("Unsupported geometry type : " + geomTypeInternal.ToString());

            }

            string layerName = String.Format(outputProcessor.config.LayerName, tableSchema.TableName, sourceId );




            if (outputProcessor.config.AppendToExistingFiles)
            {
                Log("GetLayerByName : " + layerName);
                layer = dataSource.GetLayerByName(layerName);
                if (layer != null)
                    return;
            }

            Log("CreateLayer : " + layerName);
            layer = dataSource.CreateLayer(layerName, null, geomTypeWKB, new string[] { });

            foreach (MapFieldDefinition field in tableSchema)
            {
                switch( field.Type )
                {
                    case MapFieldType.Double:
                        {
                            using( FieldDefn f = new FieldDefn( field.Name, FieldType.OFTReal ) )
                            {
                                layer.CreateField( f , 1 );
                            }
                        }
                        break;
                    case MapFieldType.Int32:
                        {
                            using( FieldDefn f = new FieldDefn( field.Name, FieldType.OFTString ) )
                            {
                                layer.CreateField( f, 1 );
                            }
                        }
                        break;
                    case MapFieldType.String:
                        {
                            using( FieldDefn f = new FieldDefn( field.Name, FieldType.OFTString ) )
                            {
                                f.SetWidth( field.Size );
                                layer.CreateField( f, 1 );
                            }
                        }
                        break;
                    default:
                        {
                            if (!ExportSchema.IsGeometry(field.Type))
                            {
                                Log("Unrecognised field type : " + field.Name + " - " + field.Type);
                            }
                        }
                        break;
                }
            }


        }

        //Clear down handles
        internal void Finish()
        {
            Log("Finish");
            if (layer != null)
            {
                layer.Dispose();
                layer = null;
            }
        }

        internal override void OutputRecord(System.Xml.XmlElement element)
        {
            if (layer == null)
                return; //Skipping - this happens when we didn't have a geometry field (we warned earlier)

            string[] values = new string[tableSchema.Count];
            MapGeometry geometry;
            ReadValues(element, values, out geometry);

            FeatureDefn def = layer.GetLayerDefn();
            using (Feature feature = new Feature(def))
            {
                int numFields = def.GetFieldCount();
                for(int indexInOGR=0;indexInOGR<numFields;indexInOGR++)
                {
                    FieldDefn OGRFieldDef = def.GetFieldDefn(indexInOGR);

                    //This is a bit naff but we have difficulty mapping our names.
                    //KML driver will append 'Name' and 'Description' to the front
                    //SHP Driver will truncate names
                    //So we try by name, then fall back to index (and hope)

                    int indexInSchema = -1;
                    if( !mapNameToLocation.TryGetValue( OGRFieldDef.GetName(), out indexInSchema ) )
                    {
                        if (indexInOGR < tableSchema.Count)
                        {
                            indexInSchema = indexInOGR;
                        }
                    }

                    if (indexInSchema >= 0)
                    {
                        string valueInXML = values[indexInSchema];
                        switch (tableSchema[indexInSchema].Type)
                        {
                            case MapFieldType.String:
                                feature.SetField(indexInOGR, valueInXML.Trim());
                                break;
                            case MapFieldType.Double:
                                {
                                    double d;
                                    if (Double.TryParse(valueInXML, out d))
                                    {
                                        feature.SetField(indexInOGR, d);
                                    }
                                }
                                break;
                            case MapFieldType.Int32:
                                int i;
                                if (Int32.TryParse(valueInXML, out i))
                                {
                                    feature.SetField(indexInOGR, i);
                                }
                                break;
                        }
                    }
                }

                byte[] wkb = geometry.AsWellKnownBinary();
                using (Geometry geom = Geometry.CreateFromWkb(wkb))
                {
                    feature.SetGeometry(geom);
                    layer.CreateFeature(feature);
                }
            }
            
        }


        public override OutputGeneric OutputProcessorGeneric
        {
            get { return outputProcessor; }
        }
    }
}
