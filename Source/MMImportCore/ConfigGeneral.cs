using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using System.Xml;
using System.Xml.Serialization;
using MMImportCore;

namespace Edonica.XMLImport
{
    /// <summary>
    /// Configuration object.  This is bound to a property grid and saved with an XML serialiser
    /// </summary>
    [XmlInclude(typeof(ConfigOutputPostgresBulk) )]
    [XmlInclude(typeof(ConfigOutputPostgresInsert))]
    [XmlInclude(typeof(ConfigOutputOGR))]
    [XmlInclude(typeof(ConfigOutputMSSQL))]
    public class ConfigGeneral
    {
        //BrowsableAttribute

        /// <summary>
        /// An appropriately initialised serializer object
        /// </summary>
        static public XmlSerializer serializer = new XmlSerializer(typeof(ConfigGeneral)/*, new Type[] { typeof(ConfigOutputPostgresBulk), typeof(ConfigOutputPostgresInsert) }*/);

        /// <summary>
        /// Save config to filename
        /// </summary>
        /// <param name="filename"></param>
        public void Save(string filename)
        {
            using (XmlTextWriter writer = new XmlTextWriter(filename, Encoding.UTF8))
            {
                writer.Formatting = Formatting.Indented;
                serializer.Serialize(writer, this);
            }
        }

        /// <summary>
        /// Load a configuration file.
        /// </summary>
        /// <param name="filename"></param>
        /// <returns></returns>
        public static ConfigGeneral Load(string filename)
        {
            using (XmlTextReader reader = new XmlTextReader(filename))
            {
                return (ConfigGeneral)serializer.Deserialize(reader) as ConfigGeneral;
            }
        }



        private string outputSchema = "..\\..\\..\\..\\Transforms\\Topo\\OSMMTopography.xsd";

        /// <summary>
        /// Filename containing the schema
        /// </summary>
        [Category("(General)")]
        [BrowsableFile( FileFilter="XML Schema Files(*.xml;*.xsd)|*.xml;*.xsd||" )]
        [Description("The XML schema that defines the output structure.  Tables/Files are created in accordance with this schema")]
        [EditorAttribute(typeof(UITypeEditorFileBrowseFile), typeof(System.Drawing.Design.UITypeEditor))]
        public string OutputSchema
        {
            get { return outputSchema; }
            set { outputSchema = value; }
        }

        private string transform = "..\\..\\..\\..\\Transforms\\Topo\\OSMMTopography.xslt";
        /// <summary>
        /// Filename for the XSL transform
        /// </summary>
        [Category("(General)")]
        [BrowsableFile(FileFilter = "XSL Transform Files(*.xsl;*.xslt)|*.xsl;*.xslt||")]
        [Description("The XSL schema that is applied to every appropriate node in the document.")]
        [EditorAttribute(typeof(UITypeEditorFileBrowseFile), typeof(System.Drawing.Design.UITypeEditor))]
        public string Transform
        {
            get { return transform; }
            set { transform = value; }
        }

        private string importBaseElement = "osgb:FeatureCollection";
        /// <summary>
        /// Base element used for the import
        /// </summary>
        [Description("The XML element that contains all the child elements we wish to import.")]
        public string ImportBaseElement
        {
            get { return importBaseElement; }
            set { importBaseElement = value; }
        }
	
        private bool showIgnoredElements=true;
        /// <summary>
        /// Report ignored data
        /// </summary>
        [Category("Debug")]
        [Description("Set to true if you want XML elements that were not imported to be listed")]
        public bool ShowIgnoredElements
        {
            get { return showIgnoredElements; }
            set { showIgnoredElements = value; }
        }

        private int debugLine = -1;
        /// <summary>
        /// Debuging info (or -1 for no debug
        /// </summary>
        [Category("Debug")]
        [Description("Set to a non zero value if you want to debug a specific line in the XML file.  Other data will not be imported.")]
        public int DebugLine
        {
            get { return debugLine; }
            set { debugLine = value; }
        }

        private string traceOutputDirectory;

        /// <summary>
        /// Very detailed tracing of every element.  Will thrash filesystem.
        /// </summary>
        [Category("Debug")]
        [Description("If set, a debug of every item that passes through the importer is written to the folder.")]
        [EditorAttribute(typeof(UITypeEditorFileBrowseFolder), typeof(System.Drawing.Design.UITypeEditor))]
        public string TraceOutputDirectory
        {
            get { return traceOutputDirectory; }
            set { traceOutputDirectory = value; }
        }


        private ConfigOutputGeneric outputConfig;

        /// <summary>
        /// Configuration object for our output processor
        /// </summary>
        [Browsable(false)]
        public ConfigOutputGeneric OutputConfig
        {
            get { return outputConfig; }
            set { outputConfig = value; }
        }

        private bool showSummary=true;

        /// <summary>
        /// Enable timings to be displayed
        /// </summary>
        [Category("Debug")]
        [Description("If true, after each file is imported, a breakdown of performance counts and time spent at each step will be displayed")]
        public bool ShowSummary
        {
            get { return showSummary; }
            set { showSummary = value; }
        }



        string logFile = "Log.txt";
        /// <summary>
        /// Log file
        /// </summary>
        [Category("Debug")]
        [BrowsableFile(FileFilter = "Text Files(*.txt;*.log)|*.txt;*.log||")]
        [EditorAttribute(typeof(UITypeEditorFileBrowseFile), typeof(System.Drawing.Design.UITypeEditor))]
        [Description("Appends all messages logged in the console to a file.  File will be appended to.  Relative paths will be relative to executable.")]
        public string LogFile
        {
            get { return logFile; }
            set { logFile = value; }
        }
	

    }
}
