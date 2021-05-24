///////////////////////////////////////////////////
//Copyright Edonica

using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Xml.Xsl;
using System.IO;
using System.Xml.Schema;

using Edonica.Projection;
using Edonica.MapBase;
using System.Windows;

namespace Edonica.XMLImport
{
    /// <summary>
    /// Generic output processor.
    /// </summary>
    public abstract class OutputGeneric
    {
        /// <summary>
        /// Called before we do anything
        /// </summary>
        /// <param name="schema"></param>
        public virtual void BaseInit(ExportSchema schema)
        {
            if (GeneralConfig.ConvertToWGS84)
            {
                Log("Validating WGS84 conversion");
                if (!LatLongNGR.OSTNDataPresent)
                {
                    throw new Exception("OSTN data file not present.  OSGB1936 -> WGS84 conversion will not be possible.  Please ensure an OSTN02.DAT file exists in the same directory as the application.");
                }
            }
        }
        /// <summary>
        /// Called after we've done everything.
        /// </summary>
        public abstract void BaseShutdown();
        /// <summary>
        /// Stub for application implementation
        /// </summary>
        public abstract void DropTables();
        /// <summary>
        /// Stub for application implementation
        /// </summary>
        public abstract void CreateTables();
        /// <summary>
        /// Stub for application implementation
        /// </summary>
        public abstract void DropIndexes();
        /// <summary>
        /// Stub for application implementation
        /// </summary>
        public abstract void CreateIndexes();
        /// <summary>
        /// Stub for application implementation
        /// </summary>
        public abstract void OutputStart( string sourceID );
        /// <summary>
        /// Stub for application implementation
        /// </summary>
        public abstract void OutputStop();
        /// <summary>
        /// Stub for application implementation
        /// </summary>
        public abstract void RemoveDuplicates();
        /// <summary>
        /// Stub for application implementation
        /// </summary>
        public abstract void QueryInformation();
        /// <summary>
        /// Stub for application implementation
        /// </summary>
        public abstract void ApplyUpdates();

        /// <summary>
        /// Genenric type for logging functions
        /// </summary>
        /// <param name="s"></param>
        public delegate void Logger(string s);

        /// <summary>
        /// Callback for when we want to write to the log
        /// </summary>
        public Logger Log=null;

        /// <summary>
        /// Dictionary mapping from table name to table object within this output source.
        /// </summary>
        public Dictionary<string, OutputGenericTable> Tables = new Dictionary<string, OutputGenericTable>();


        /// <summary>
        /// Otuput a record - this will look at the element name, find the appropriate output table and call OutputRecord on that.
        /// </summary>
        /// <param name="element"></param>
        public virtual void OutputRecord(XmlElement element)
        {
            OutputGenericTable tableHolder;
            string nodeName = element.Name;
            if (Tables.TryGetValue(nodeName, out tableHolder))
            {
                tableHolder.OutputRecord(element);
            }
            else
            {
                throw new Exception("Unrecognised output table node " + nodeName);
            }
        }

        /// <summary>
        /// Perform any required processing.
        /// This is where OSGB1936 -> WGS84 conversion occurs if necessary.
        /// </summary>
        /// <param name="geom"></param>
        public virtual void TransformGeometry(MapGeometry geom)
        {
            if (GeneralConfig.ConvertToWGS84)
            {
                MapGeometry.PointOperation op = delegate(ref Point p)
                {
                    p = LatLongNGR.ConvertOSGB36ToWGS84(p);
                };
                geom.ForEachPoint( op );
            }
        }

        /// <summary>
        /// Get hold of the configuration object
        /// </summary>
        public abstract ConfigOutputGeneric GeneralConfig { get; }
    }

}