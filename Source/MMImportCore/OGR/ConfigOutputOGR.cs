using System;
using System.Collections.Generic;
using System.Text;
using Edonica.XMLImport;
using MMImportCore.OGR;
using System.ComponentModel;

namespace MMImportCore
{
    public class ConfigOutputOGR : ConfigOutputGeneric
    {
        public ConfigOutputOGR()
        {
            DriverName = "ESRI Shapefile";
            DataSourceName = @"C:\temp\MasterMapOut";
            LayerName = "MM_{0}";
            OutputEachInputFileToItsOwnDirectory = false;
            DataSourceType = OGRDataSourceType.Auto;
        }


        [Description("OGR driver to use.")]
        [EditorAttribute(typeof(OGRHelper.UITypeEditorOGRDataSource), typeof(System.Drawing.Design.UITypeEditor))]
        [Category("OGR")]
        public string DriverName {get;set;}

        [Description("For a file based driver, this is the output directory.  For a non file based output, this might be a database connection")]
        [EditorAttribute(typeof(UITypeEditorFileBrowseFolder), typeof(System.Drawing.Design.UITypeEditor))]
        [Category("OGR")]
        public string DataSourceName {get;set;}

        [Category("OGR")]
        [Description("Output layer / file name.  {0} = source table name, {1} = source file name")]
        public string LayerName {get;set;}

        [Description("Type of data source type (so we know what to do with file/directory creation)")]
        [Category("OGR")]
        public OGRDataSourceType DataSourceType { get; set; }


        [Description("Output each input tile to it's own directory?  This option will append the input filename to the output datasource, whereby each tile will end up in it's own directory")]
        [Category("Options")]
        public bool OutputEachInputFileToItsOwnDirectory {get;set;}

        [Description("Append to existing files?")]
        [Category("Options")]
        public bool AppendToExistingFiles {get;set;}


        public override OutputGeneric CreateInstance()
        {
            OutputOGR output = new OutputOGR();
            output.config = this;
            return output;
        }
    }

    public enum OGRDataSourceType
    {
        Auto,
        Folder,
        File,
        Other
    }
}
