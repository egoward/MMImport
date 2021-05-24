using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.IO;
using MMImportCore;

namespace Edonica.XMLImport
{
    /// <summary>
    /// Main application form - consists of a menu and a splitter with general properties on the left and export properties on the right.
    /// A combo box lets you choose the type of the export provider
    /// </summary>
    public partial class FormImportMain : Form
    {
        class OutputTypeHolder
        {
            public OutputTypeHolder(string Name, Type type)
            {
                this.Name = Name;
                this.type = type;
            }
            public string Name;
            public Type type;

            public override string ToString()
            {
                return Name;
            }
        }


        /// <summary>
        /// Initialise the form.
        /// More work is done on the Load event.
        /// </summary>
        public FormImportMain()
        {
            InitializeComponent();

            comboBoxExportTarget.Items.Add(new OutputTypeHolder("OGR - Expot to OGR supported file eg. Shape", typeof(ConfigOutputOGR)));
            comboBoxExportTarget.Items.Add(new OutputTypeHolder("SQL Server 2008", typeof(ConfigOutputMSSQL)));
            comboBoxExportTarget.Items.Add(new OutputTypeHolder("Postgres - INSERT statements", typeof(ConfigOutputPostgresInsert)));
            comboBoxExportTarget.Items.Add(new OutputTypeHolder("Postgres - Bulk Load", typeof(ConfigOutputPostgresBulk)));
        }


        /// <summary>
        /// Main configuration object
        /// </summary>
        public ConfigGeneral config;

        /// <summary>
        /// When the user changes the combo box, we change the type of our output configuration.
        /// </summary>
        private void comboBoxExportTarget_SelectionChangeCommitted(object sender, EventArgs e)
        {
            OutputTypeHolder t = (OutputTypeHolder)comboBoxExportTarget.SelectedItem;
            if (propertyGridOutput.SelectedObject != null && propertyGridOutput.SelectedObject.GetType() == t.type)
            {
                //Don't change if we don't have to!
                return;
            }

            config.OutputConfig = (ConfigOutputGeneric)t.type.GetConstructor(Type.EmptyTypes).Invoke(null);
            //Rebind the property grid to our new object.
            propertyGridOutput.SelectedObject = config.OutputConfig;
        }

        void PopulateFormFromData()
        {
            if (config == null)
            {
                config = new ConfigGeneral();
            }
            propertyGridGeneral.SelectedObject = config;

            if (config.OutputConfig == null)
            {
                //20080526 - Change default output type to OGR / SHP
                //config.OutputConfig = new ConfigOutputPostgresBulk();
                config.OutputConfig = new ConfigOutputOGR();
            }
            for (int c = 0; c < comboBoxExportTarget.Items.Count; c++)
            {
                if (((OutputTypeHolder)comboBoxExportTarget.Items[c]).type == config.OutputConfig.GetType())
                {
                    comboBoxExportTarget.SelectedIndex = c;
                    break;
                }
            }

            propertyGridOutput.SelectedObject = config.OutputConfig;
        }

        string autosaveFilename;

        void LoadDefaultConfig()
        {
            if (File.Exists(autosaveFilename))
            {
                try
                {
                    config = ConfigGeneral.Load(autosaveFilename);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error loading " + autosaveFilename + "\n\n" + ex.ToString());
                }
            }
        }

        private void FormImportParams_Load(object sender, EventArgs e)
        {
            autosaveFilename = Path.Combine(Application.StartupPath, "Default.XMLImport");
            LoadDefaultConfig();
            PopulateFormFromData();
        }

        private void FormImportParams_FormClosing(object sender, FormClosingEventArgs e)
        {
            //DialogResult result = MessageBox.Show(this,"Save settings to\n   " + autosaveFilename + "\n for use next time?","Save settings?", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
            //if (result == DialogResult.Cancel)
            //{
            //    e.Cancel = true;
            //}
            //else if (result == DialogResult.Yes)
            //{
            //}
        }

        private void saveDefaultsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (File.Exists(autosaveFilename))
            {
                if (MessageBox.Show(this, "Overwrite configuration file...\n   " + autosaveFilename + "\n...?\n", "Save settings?", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) != DialogResult.OK)
                {
                    return;
                }
            }
            config.Save(autosaveFilename);
        }

        private void loadDefaultsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LoadDefaultConfig();
            PopulateFormFromData();
        }


        private void saveConfigurationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (SaveFileDialog dlg = new SaveFileDialog())
            {
                dlg.Filter = "XML import definition files(*.XMLImport)|*.XMLImport||";
                //dlg.Title = "Select file to open";
                if (dlg.ShowDialog() != DialogResult.OK)
                    return;
                config.Save( dlg.FileName);
                PopulateFormFromData();
            }
        }

        private void loadConfigurationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog dlg = new OpenFileDialog())
            {
                dlg.Filter = "XML import definition files(*.XMLImport)|*.XMLImport||";
                //dlg.Title = "Select file to open";
                if (dlg.ShowDialog() != DialogResult.OK)
                    return;
                config = ConfigGeneral.Load(dlg.FileName);
                PopulateFormFromData();
            }
        }

        private void resetConfigurationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            config = new ConfigGeneral();
            PopulateFormFromData();
        }

        delegate void ImportAction( Transformer prog );

        void DoAction( ImportAction action )
        {
            using (FormWorker f = new FormWorker( Transformer.MakePathAbsolute(config.LogFile) ))
            {
                f.proc = delegate(TextWriter writer)
                {
                    Transformer prog = new Transformer();
                    prog.log = writer;
                    prog.Log("Started at " + DateTime.Now.ToString(Transformer.dateTimeFormatString));
                    prog.config = config;
                    prog.BaseInit();
                    prog.CheckCancelFlag = delegate(){return f.CancellationPending;};
                    action(prog );
                    prog.BaseShutdown();
                    prog.Log("Finished at " + DateTime.Now.ToString(Transformer.dateTimeFormatString));

                };

                f.ShowDialog();
            }
        }

        private void testConfigurationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ImportAction action = delegate(Transformer prog)
            {
                //Do nothing - just testing!
            };
            DoAction(action);
        }

        private void createTablesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ImportAction action = delegate(Transformer prog)
            {
                prog.outputProcessor.CreateTables();
            };
            DoAction(action);
        }

        private void dropTablesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are you sure you want to drop the tables from this database?", "Drop tables", MessageBoxButtons.YesNo) != DialogResult.Yes)
            {
                return;
            }
            ImportAction action = delegate(Transformer prog)
            {
                prog.outputProcessor.DropTables();
            };
            DoAction(action);

        }

        private void createIndexesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ImportAction action = delegate(Transformer prog)
            {
                prog.outputProcessor.CreateIndexes();
            };
            DoAction(action);
        }

        private void dropIndexesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ImportAction action = delegate(Transformer prog)
            {
                prog.outputProcessor.DropIndexes();
            };
            DoAction(action);
        }


        private void importFilesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Title = "Select files to import";
            ofd.Filter = "XML files (*.gz;*.z;*.xml)|*.gz;*.z;*.xml|All Files (*.*)|*.*";
            ofd.Multiselect = true;
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                ImportFiles(ofd.FileNames);
            }
        }

        private void removeDuplicatesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ImportAction action = delegate(Transformer prog)
            {
                prog.outputProcessor.RemoveDuplicates();
            };
            DoAction(action);
        }
        private void queryInfoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ImportAction action = delegate(Transformer prog)
            {
                prog.outputProcessor.QueryInformation();
            };
            DoAction(action);
        }


        void ImportFiles(string[] filenames)
        {
            ImportAction action = delegate(Transformer prog)
            {
                for (int c = 0; c < filenames.Length; c++)
                {
                    string filename = filenames[c];
                    prog.Log("Processing " + filename + "(" + (c+1) + " of " + filenames.Length +")" );
                    prog.outputProcessor.OutputStart( Path.GetFileNameWithoutExtension( filename ) );
                    prog.ProcessFile(filename);
                    prog.outputProcessor.OutputStop();
                    if (prog.CheckCancelFlag != null && prog.CheckCancelFlag())
                    {
                        break;
                    }
                }
            };
            DoAction(action);

        }

        void CollectImportFilesFromPath(string path, List<string> paths)
        {
            foreach (string childPath in Directory.GetDirectories(path))
            {
                CollectImportFilesFromPath(childPath, paths);
            }

            foreach( string fileName in Directory.GetFiles( path ) )
            {
                string extension = Path.GetExtension(fileName).ToLower();
                if (extension == ".z" || extension == ".gz" || extension == ".xml")
                    paths.Add(fileName);
            }
        }

        private void importADirectoryFullOfFilesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog dlg = new FolderBrowserDialog();
            dlg.RootFolder = Environment.SpecialFolder.MyComputer;
            dlg.Description = "Pick a folder to recursively import.  *.XML, *.Z and *.GZ files will be processed";
            if (dlg.ShowDialog() != DialogResult.OK)
                return;

            //string[] files = Directory.GetFiles(dlg.SelectedPath, "*.xml;*.z;*.gz", SearchOption.AllDirectories);


            List<string> paths = new List<string>();
            CollectImportFilesFromPath(dlg.SelectedPath, paths);

            ImportFiles(paths.ToArray());
        }

        private void applyLoadedUpdatesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ImportAction action = delegate(Transformer prog)
            {
                prog.outputProcessor.ApplyUpdates();
            };
            DoAction(action);
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void toolStripMenuItemImportOSTNData_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Title = "Please select OSTN text file";
            ofd.Filter = "CSV text files (*.csv;*.txt)|*.csv;*.txt";
            ofd.FileName = "OSTN02_OSGM02_GB.txt";
            if (ofd.ShowDialog() == DialogResult.Cancel)
                return;

            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Title = "Please select location to save OSTN binary";
            sfd.Filter = "Binary files (*.dat)|*.dat";

            sfd.FileName = Path.Combine( Path.GetDirectoryName(GetType().Module.FullyQualifiedName), "OSTN02.dat" );
            if (sfd.ShowDialog() == DialogResult.Cancel)
                return;

            try
            {
                Edonica.Projection.OSTNDataParser.ConvertCSV(ofd.FileName, sfd.FileName);
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, "Error : \n" + ex.Message, "OSTN data error", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }
            MessageBox.Show(this, "OSTN02 data import complete", "Import Complete", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void reprojectToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (FormReproject f = new FormReproject())
            {
                f.ShowDialog();
            }

        }






    }
}