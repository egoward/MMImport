using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.IO;

using MMImportCore;
using OSGeo.OGR;
using Edonica.Projection;
using Edonica.MapBase;
using System.Windows;

namespace Edonica.XMLImport
{
    public partial class FormReproject : Form
    {
        public FormReproject()
        {
            InitializeComponent();
        }

        void InitDriverCombo(ComboBox cb)
        {
            OGRHelper.EnsureInitialised();
            int driverCount = Ogr.GetDriverCount();
            for (int c = 0; c < driverCount; c++)
            {
                Driver driver = Ogr.GetDriver(c);
                cb.Items.Add(driver.name);
            }
            cb.SelectedIndex = 0;
        }

        private void FormReproject_Load(object sender, EventArgs e)
        {
            InitDriverCombo(comboBoxSourceMap);
            InitDriverCombo(comboBoxTargetMap);

#if DEBUG
            textBoxSourceA.Text = "C:\\GIS1\\SHP";
            textBoxSourceB.Text = "input";
            textBoxTargetA.Text = "C:\\GIS1\\SHP";
            textBoxTargetB.Text = "output";
#endif

        }

        private void buttonSourceBrowse_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "ESRI Shape Files (*.SHP)|*.SHP|All Files (*.*)|*.*";
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                textBoxSourceA.Text = Path.GetDirectoryName( ofd.FileName );
                textBoxSourceB.Text = Path.GetFileNameWithoutExtension(ofd.FileName);
            }
        }

        private void buttonTargetBrowse_Click(object sender, EventArgs e)
        {
            SaveFileDialog ofd = new SaveFileDialog();
            ofd.Filter = "ESRI Shape Files (*.SHP)|*.SHP|All Files (*.*)|*.*";
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                textBoxTargetA.Text = Path.GetDirectoryName(ofd.FileName);
                textBoxTargetB.Text = Path.GetFileNameWithoutExtension(ofd.FileName);
            }
        }

        public enum TransformMode
        {
            WGS84ToOS1936,
            OS1936ToWGS84
        }


        private void buttonGo_Click(object sender, EventArgs e)
        {
            string sourceDriver = comboBoxSourceMap.Text;
            string sourceMapA = textBoxSourceA.Text;
            string sourceMapB = textBoxSourceB.Text;
            string targetDriver = comboBoxTargetMap.Text;
            string targetMapA = textBoxTargetA.Text;
            string targetMapB = textBoxTargetB.Text;
            TransformMode mode = radioButtonOStoWGS84.Checked ? TransformMode.OS1936ToWGS84 : TransformMode.WGS84ToOS1936;

            MapGeometry.PointOperation transformOp;
            if (mode == TransformMode.OS1936ToWGS84)
            {
                transformOp = delegate(ref Point p)
                {
                    p = LatLongNGR.ConvertOSGB36ToWGS84(p);
                };
            }
            else //mode == TransformMode.WGS84ToOS1936
            {
                transformOp = delegate(ref Point p)
                {
                    p = LatLongNGR.ConvertWGS84ToOSGB36(p);
                };
            };


            FormWorker.WorkerProc proc = delegate(TextWriter writer)
            {
                TransformUtils.TransformGeometry(writer, sourceDriver, sourceMapA, sourceMapB, targetDriver, targetMapA, targetMapB, transformOp);
            };
            
            using (FormWorker formWorker = new FormWorker(null))
            {
                //MapProjector proj = new MapProjector();
                formWorker.proc = proc;
                formWorker.ShowDialog();
            }
        }
    }
}