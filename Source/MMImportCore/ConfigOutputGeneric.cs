using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;

namespace Edonica.XMLImport
{
    /// <summary>
    /// Generic output configuraiton.  This gets bound to a property grid.
    /// Contained within a 'ConfigGeneral' object.
    /// </summary>
    public abstract class ConfigOutputGeneric
    {

        private bool dummyRun;

        /// <summary>
        /// Is this a dummy run?
        /// </summary>
        [Category("Debug")]
        [Description("If true, import will not actually occur but we will go through all the steps.")]
        public bool DummyRun
        {
            get { return dummyRun; }
            set { dummyRun = value; }
        }

        /// <summary>
        /// Overriden by a concrete implementation - creates an instance of the object that does the work.
        /// </summary>
        /// <returns></returns>
        public abstract OutputGeneric CreateInstance();


        private bool showSummaryInfo;

        /// <summary>
        /// Should we dump extra information about the output?
        /// </summary>
        [Category("Debug")]
        [Description("Show summary information")]
        public bool ShowSummaryInfo
        {
            get { return showSummaryInfo; }
            set { showSummaryInfo = value; }
        }

        private bool convertToWGS84 = false;
        /// <summary>
        /// Write coordinates in WGS84 (GPS) coordinates rather than OS grid references
        /// </summary>
        [Category("Spatial system")]
        [Description("If true, OS grid references will be converted into WGS84 coordinates before writing to the database.  WGS84 data should have an SRD of 4326")]
        public bool ConvertToWGS84
        {
            get { return convertToWGS84; }
            set { convertToWGS84 = value; }
        }

	
    }
}
