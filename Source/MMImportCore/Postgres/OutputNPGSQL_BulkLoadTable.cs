using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Xml.Xsl;
using System.Data;
using System.IO;
using System.Diagnostics;

using Npgsql;

using Edonica.MapBase;

namespace Edonica.XMLImport
{
    /// <summary>
    /// Class for managing output of data via the postgres bulk load method
    /// </summary>
    public class OutputNPGSQL_BulkLoadTable : OutputGenericTable
    {
        /// <summary>
        /// Bulk import
        /// </summary>
        /// <param name="bulkLoad"></param>
        /// <param name="tableSchema"></param>
        public OutputNPGSQL_BulkLoadTable(OutputNPGSQL_BulkLoad bulkLoad, MapTableDefinition tableSchema)
            : base(tableSchema)
        {
            this.outputProcessor = bulkLoad;
        }

        internal OutputNPGSQL_BulkLoad outputProcessor;
        Process proc;
        bool UseCSV = false;
        StreamWriter writer;

        /// <summary>
        /// Property to get the generic parent output method.
        /// </summary>
        public override OutputGeneric OutputProcessorGeneric
        {
            get { return outputProcessor; }
        }

        long timeRead;
        long timeWrite;
        long timeEncode;

        override internal void OutputRecord(XmlElement element)
        {

            string[] values = new string[tableSchema.Count];

            Stopwatch watchTmp = new Stopwatch();
            watchTmp.Start();

            long start = watchTmp.ElapsedTicks;
            ReadValues(element, values);
            timeRead += watchTmp.ElapsedTicks - start;

            start = watchTmp.ElapsedTicks;

            StringWriter lineWriter = new StringWriter();
            lineWriter.Write(values[0]);
            for (int c = 1; c < values.Length; c++)
            {
                lineWriter.Write(UseCSV ? "," : "\t");
                if (values[c] != null)
                {
                    if (UseCSV)
                    {
                        lineWriter.Write(values[c].Trim());
                    }
                    else
                    {
                        //Encode null values as "\N"
                        string s = values[c].Trim();
                        lineWriter.Write(s == "" ? "\\N" : s);
                    }
                }
            }
            lineWriter.Write("\n");
            timeWrite += watchTmp.ElapsedTicks - start;


            start = watchTmp.ElapsedTicks;
            if (writer != null)
            {
                //There's no 'StandardInputEncoding' and it ends up as the local codepage so we need to encode UTF8 and write the bytes.
                byte[] utf8Bytes = Encoding.UTF8.GetBytes(lineWriter.ToString());
                writer.BaseStream.Write(utf8Bytes, 0, utf8Bytes.Length);
            }
            timeEncode+= watchTmp.ElapsedTicks - start;

        }


        internal void Init()
        {
            string bulkLoadArguments = String.Format(outputProcessor.config.PSQLCommandLineArguments, outputProcessor.config.TableNamePrefix + tableSchema.TableName);
            if (outputProcessor.config.DummyRun)
            {
                string extension = UseCSV ? ".CSV" : ".TXT";
                if (!String.IsNullOrEmpty(outputProcessor.config.BulkOutputDebugDirectory))
                {
                    if(!Directory.Exists( outputProcessor.config.BulkOutputDebugDirectory ) )
                    {
                        Log("Creating directory " + outputProcessor.config.BulkOutputDebugDirectory );
                        Directory.CreateDirectory( outputProcessor.config.BulkOutputDebugDirectory );
                    }
                    string sLogFile = Path.Combine(outputProcessor.config.BulkOutputDebugDirectory, outputProcessor.config.TableNamePrefix + tableSchema.TableName + extension);
                    Log("Writing output to " + sLogFile);
                    writer = new StreamWriter(sLogFile);

                    Log("You could run : TYPE " + sLogFile + " | " + outputProcessor.config.PSQLPathToExe + " " + bulkLoadArguments);
                }
                else
                {   
                    Log("Output will be discarded");
                }
            }
            else
            {
                proc = new Process();
                proc.StartInfo.CreateNoWindow = true;
                proc.StartInfo.FileName = outputProcessor.config.PSQLPathToExe;
                proc.StartInfo.Arguments = bulkLoadArguments;
                proc.StartInfo.UseShellExecute = false;
#if !MONO
                proc.StartInfo.StandardErrorEncoding = System.Text.Encoding.UTF8;
                proc.StartInfo.StandardOutputEncoding = System.Text.Encoding.UTF8;
#endif
                proc.StartInfo.RedirectStandardInput = true;
                proc.StartInfo.RedirectStandardError = true;
                proc.StartInfo.RedirectStandardOutput = true;
                Log("Starting " + proc.StartInfo.FileName + " " + proc.StartInfo.Arguments + "");
                proc.EnableRaisingEvents = true;
                proc.Exited += new EventHandler(proc_Exited);
                proc.OutputDataReceived += new DataReceivedEventHandler(proc_OutputDataReceived);
                proc.ErrorDataReceived += new DataReceivedEventHandler(proc_ErrorDataReceived);
                proc.Start();

                //This is a bit rubbish - output from the application is buffered so we only see chunks of 2K
                //This means that we only get the error message at the end.
                proc.BeginErrorReadLine();
                proc.BeginOutputReadLine();

                writer = proc.StandardInput;
            }

        }


        void proc_Exited(object sender, EventArgs e)
        {
            if (proc == null)
                return;

            try
            {
                if (outputProcessor.config.ShowSummaryInfo || proc.ExitCode != 0 )
                {
                    Log("Bulk loader process has exited with code " + proc.ExitCode);
                }
            }
            catch (Exception ex)
            {
                Log("Bulk loader process has exited - fetching ExitCode raised" + ex.Message);
            }
        }

        void proc_ErrorDataReceived(object sender, DataReceivedEventArgs e)
        {
            if (e.Data != null && e.Data.Length > 0)
            {
                this.outputProcessor.errorsOccured = true;
                Log("STDERR : " + e.Data);
            }
        }

        void proc_OutputDataReceived(object sender, DataReceivedEventArgs e)
        {
            if (e.Data != null && e.Data.Length > 0)
            {
                Log("STDOUT : " + e.Data);
            }
        }

        internal void Finish()
        {
            if (writer != null)
            {
                writer.Close();
                writer.Dispose();
                writer = null;
            }
            if (proc != null)
            {
                if (outputProcessor.config.ShowSummaryInfo)
                {
                    Log(tableSchema.TableName + " - Waiting for bulk load to finish");
                }
                try
                {
                    proc.WaitForExit();
                    Log(tableSchema.TableName + " - Done");
                    proc.Close();
                    proc = null;
                }
                catch (Exception ex)
                {
                    Log("Error stopping bulk loader - " + ex.Message);
                }
            }

            if (outputProcessor.config.ShowSummaryInfo)
            {
                Log("timeRead   : " + Transformer.FormatTime(timeRead));
                Log("timeWrite  : " + Transformer.FormatTime(timeWrite));
                Log("timeEncode : " + Transformer.FormatTime(timeEncode));
            }

        }


    }

}