using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace Edonica.XMLImport
{
    /// <summary>
    /// Background worker form
    /// </summary>
    public partial class FormWorker : Form
    {
        /// <summary>
        /// Construct a new form
        /// </summary>
        public FormWorker( string logFile )
        {
            InitializeComponent();
            this.logFile = logFile;
        }

        string logFile;

        /// <summary>
        /// The procedure we will run in the background.
        /// </summary>
        public WorkerProc proc;

        /// <summary>
        /// The signature for a background procedure.
        /// </summary>
        /// <param name="writer">TextWriter that writes to the textbox</param>
        public delegate void WorkerProc( TextWriter writer );


        class LogWriter : StringWriter
        {
            StreamWriter logFileStream = null;

            public LogWriter(RichTextBox richTextBox, string logFile )
            {
                this.richTextBox = richTextBox;
                AddEventDel = new AddEventDelegate(WriteLineInUIThread);

                if( logFile != null )
                {
                    logFileStream = new StreamWriter(logFile,true,Encoding.UTF8);
                }
            }

            RichTextBox richTextBox;
            public override void WriteLine(string value)
            {
                if (logFileStream != null)
                {
                    logFileStream.WriteLine(value);
                }
                richTextBox.BeginInvoke(AddEventDel, new object[] { value });
            }

            public delegate void AddEventDelegate(string s);
            AddEventDelegate AddEventDel;


            void WriteLineInUIThread(string value)
            {
                richTextBox.AppendText(value+"\n");
                //richTextBox.Select(richTextBox.TextLength, 0);
                //richTextBox.ScrollToCaret();
            }

            protected override void Dispose(bool disposing)
            {
                base.Dispose(disposing);

                if (logFileStream != null)
                {
                    logFileStream.WriteLine();
                    logFileStream.WriteLine();
                    logFileStream.Dispose();
                    logFileStream = null;
                }
            }

        }

        private void backgroundWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            using (LogWriter writer = new LogWriter(richTextBox, logFile))
            {
                try
                {
                    proc(writer);
                }
                catch (Exception ex)
                {
                    writer.WriteLine("\nERROR:\n" + ex.Message + "\n\nFull details:\n" + ex.ToString());
                }
            }
        }

        private void FormWorker_Load(object sender, EventArgs e)
        {
            backgroundWorker.RunWorkerAsync();
        }

        private void backgroundWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            buttonClose.Enabled = true;
            buttonClose.Text = "Close";
        }

        private void buttonClose_Click(object sender, EventArgs e)
        {
            if (backgroundWorker.IsBusy)
            {
                DialogResult result = MessageBox.Show("Are you sure you want to abort?", "Abort?", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (result == DialogResult.Yes)
                {
                    backgroundWorker.CancelAsync();
                    buttonClose.Enabled = false;
                }
            }
            else
            {
                Close();
            }
        }
        /// <summary>
        /// Has the user pressed the cancel button?
        /// </summary>
        public bool CancellationPending
        {
            get { return backgroundWorker.CancellationPending; }
        }


    }
}