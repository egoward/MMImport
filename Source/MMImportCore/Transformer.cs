///////////////////////////////////////////////////
//Copyright Edonica

using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Xml.Xsl;
using System.IO;
using System.Xml.Schema;
using System.IO.Compression;
//using System.Windows.Forms;
using System.Configuration;

namespace Edonica.XMLImport
{
    /// <summary>
    /// Main class that holds everything together.
    /// 
    /// config - tells it what to do
    /// outputSchema - defines database tables that we create
    /// XSL file - defines transform from source XML to outputSchema
    /// outputProcessor - the output
    /// Call BaseInit and BaseShutdown do initialisaion and startup.
    /// </summary>
    public class Transformer
    {
        /// <summary>
        /// Format a string in a sensible international format
        /// </summary>
        static public string dateTimeFormatString = "yyyy/MM/dd-hh:mm:ss";
        /// <summary>
        ///We keep one of these for each input element name
        ///This lets us report back a summary of what nodes were input and what element types were output in accordance.
        /// </summary>
        public class SummaryForInputNode
        {
            /// <summary>
            /// Increment the counter for a given output name
            /// </summary>
            /// <param name="name">name of output element / table</param>
            public void AddOutputCount(string name )
            {

                if (OutputElementCounts.ContainsKey(name))
                    OutputElementCounts[name]++;
                else
                    OutputElementCounts[name] = 1;
            }

            /// <summary>
            /// Dictionary of output element names to counts.
            /// </summary>
            public Dictionary<string, int> OutputElementCounts = new Dictionary<string, int>();
            /// <summary>
            /// Number of items we have ignored (input generates no output)
            /// </summary>
            public int Ignored = 0;
            /// <summary>
            /// Number of items we have failed to process (input throws an exception)
            /// </summary>
            public int Errors = 0;
            /// <summary>
            /// Total number of items 
            /// </summary>
            public int NumItems = 0;

            /// <summary>
            /// Total processing times
            /// </summary>
            public long timeInput, timeTransform, timeOutputParse, timeOutputDo;
        }


        /// <summary>
        /// The configuration we are using.
        /// </summary>
        public ConfigGeneral config;

        /// <summary>
        /// XML document we use to load XML elements in as we process them.
        /// </summary>
        public XmlDocument loaderDocument = new XmlDocument();

        /// <summary>
        /// XSL transformation - translates input to something in the output schema
        /// </summary>
        public XslCompiledTransform transform = new XslCompiledTransform();

        /// <summary>
        /// Dictionary of input XML to output summary
        /// </summary>
        Dictionary<string, SummaryForInputNode> inputSummary = new Dictionary<string, SummaryForInputNode>();

        /// <summary>
        /// Place we write log info to.  This currently ends up in a rich text box.
        /// </summary>
        public TextWriter log;

        /// <summary>
        /// The definition of the output tables and columns within.
        /// This is independent of database / file type.
        /// </summary>
        public ExportSchema outputSchema = null;

        /// <summary>
        /// The actual output processor - this pushes the results into the database or file
        /// </summary>
        public OutputGeneric outputProcessor = null;

        /// <summary>
        /// Delegate type for functions that can report cancellation 
        /// </summary>
        /// <returns></returns>
        public delegate bool CheckCancelFlagFunction();
        /// <summary>
        /// Function that will return true if 'Cancel' has been pressed.
        /// </summary>
        public CheckCancelFlagFunction CheckCancelFlag;

        /// <summary>
        /// Stopwatch used for internal timings
        /// </summary>
        public System.Diagnostics.Stopwatch stopWatch = new System.Diagnostics.Stopwatch();



        /// <summary>
        /// Write something to the log file
        /// </summary>
        /// <param name="s">String to log</param>
        public void Log(string s)
        {
            log.WriteLine(s);
        }

        /// <summary>
        /// Locate the summary node for a given input element name.
        /// </summary>
        /// <param name="s">Element name</param>
        /// <returns></returns>
        public SummaryForInputNode GetSummaryNodeForInputElement(string s)
        {
            SummaryForInputNode ret;
            if (inputSummary.TryGetValue(s, out ret))
            {
                return ret;
            }
            ret = new SummaryForInputNode();
            inputSummary.Add(s, ret);
            return ret;
        }

        /// <summary>
        /// Convert a timespan from a StopWatch
        /// </summary>
        /// <param name="ticks"></param>
        /// <returns></returns>
        public static string FormatTime(long ticks)
        {
            double seconds = ticks / (double)System.Diagnostics.Stopwatch.Frequency;
            return seconds.ToString("0.000") + " seconds";
        }

        /// <summary>
        /// Write information about the elements we have processed for input and what the results were.
        /// </summary>
        /// <param name="writer">TextWriter that receives output</param>
        void DumpInputSummary(TextWriter writer)
        {
            foreach (KeyValuePair<string, SummaryForInputNode> input in inputSummary)
            {
                writer.WriteLine("Input: " + input.Key);
                writer.WriteLine("  Count: " + input.Value.NumItems);
                if (input.Value.Ignored > 0)
                {
                    writer.WriteLine("  Ignored: " + input.Value.Ignored);
                }
                if (input.Value.Errors > 0)
                {
                    writer.WriteLine("  Errors: " + input.Value.Errors);
                }
                if (config.ShowSummary)
                {
                    writer.WriteLine("Timings : ParseIn {0}, Trans {1}, ParseOut {2} SendOut {3}", FormatTime(input.Value.timeInput), FormatTime(input.Value.timeTransform), FormatTime(input.Value.timeOutputParse), FormatTime(input.Value.timeOutputDo) );
                    foreach (KeyValuePair<string, int> output in input.Value.OutputElementCounts)
                    {
                        writer.WriteLine("    Output count: " + output.Key + " - " + output.Value);
                    }
                }

            }
        }


        /// <summary>
        /// Process an input element.
        /// This creates a WorkItem which does the actual work and holds relevant information so we can report failure sensibly.
        /// </summary>
        /// <param name="filename"></param>
        /// <param name="reader"></param>
        /// <param name="outputProcessor"></param>
        public void ProcessNode(string filename, XmlTextReader reader, OutputGeneric outputProcessor)
        {
            WorkItem context = new WorkItem();
            context.reader = reader;
            context.program = this;
            context.outputProcessor = outputProcessor;
            context.filename = filename;
            context.Process();
        }

        /// <summary>
        /// Ensure a path that the user entered is absolute.
        /// We use FileOpen dialogs which change the current working directory isnso we use Application.StartupPath instead.
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string MakePathAbsolute(string input)
        {
            if (String.IsNullOrEmpty(input))
                return null;
            else if (Path.IsPathRooted(input))
                return input;
            else
            {
                string ApplicationPath = System.IO.Path.GetDirectoryName(typeof(Transformer).Assembly.Location);
                return Path.Combine(ApplicationPath, input);
            }
        }


        /// <summary>
        /// Read the schema, load the transform and connect to our output file / database.
        /// </summary>
        public void BaseInit()
        {
            stopWatch.Start();
            string schemaFile = MakePathAbsolute(config.OutputSchema);
            Log("Loading schema : " + schemaFile);
            outputSchema = ExportSchema.ReadSchema(schemaFile, log);

            string transformFile = MakePathAbsolute(config.Transform);
            Log("Loading transform : " + transformFile);
            transform.Load(transformFile);

            Log("Initialising output processor");
            outputProcessor = config.OutputConfig.CreateInstance();
            outputProcessor.Log = this.Log;
            outputProcessor.BaseInit(outputSchema);
            Log("Output processor connected");
        }

        /// <summary>
        /// Close down the connection to the output processor.
        /// </summary>
        public void BaseShutdown()
        {
            Log("Shutting down output processor");
            outputProcessor.BaseShutdown();

            stopWatch.Stop();

            Log("Shutdown complete : (" + FormatTime( stopWatch.ElapsedTicks ) +")");

        }


        /// <summary>
        /// Import a file, transform it and send the result to the output processor.
        /// You must call BaseInit before calling this.
        /// </summary>
        /// <param name="filename">Name of file to process</param>
        public void ProcessFile(string filename)
        {
            long startTicks = stopWatch.ElapsedTicks;

            string extension = Path.GetExtension(filename).ToLower();
            Stream rawStream = new FileStream(filename, FileMode.Open, FileAccess.Read);
            Stream inputStream = null;
            if (extension == ".z" || extension == ".gz")
            {
                Log("Processing compressed file size : " + rawStream.Length);
                inputStream = new GZipStream(rawStream, CompressionMode.Decompress);
            }
            else
            {
                Log("Processing uncompressed file size : " + rawStream.Length);
                inputStream = rawStream;
            }


            System.Diagnostics.Stopwatch watchTmp = new System.Diagnostics.Stopwatch();

            using (XmlTextReader reader = new XmlTextReader(inputStream))
            {
                bool Aborting = false;

                int nextReportedLine = 0;

                while (reader.Read())
                {
                    Aborting = CheckCancelFlag != null && CheckCancelFlag();
                    if( Aborting )
                        break;
                    if (reader.NodeType == XmlNodeType.Element)
                    {
                        if (reader.Name == config.ImportBaseElement)
                        {
                            reader.ReadStartElement();
                            while (reader.Read())
                            {
                                Aborting = CheckCancelFlag != null && CheckCancelFlag();
                                if( Aborting )
                                    break;

                                int lineNumber = reader.LineNumber;
                                if (lineNumber >= nextReportedLine)
                                {
                                    double percentage = 100 * (double)rawStream.Position / (double)rawStream.Length;
                                    log.WriteLine("Line : " + lineNumber + " - " + percentage.ToString("0.0") + "%");
                                    nextReportedLine += 100000;
                                }

                                if (reader.NodeType == XmlNodeType.EndElement)
                                {
                                    break;
                                }
                                else if (reader.NodeType == XmlNodeType.Element)
                                {
                                    ProcessNode(filename, reader, outputProcessor);
                                }
                            }
                        }
                    }
                }
            }
            inputStream.Close();
            rawStream.Close();

            if (config.ShowSummary)
            {
                log.WriteLine("Total time for file : " + FormatTime(stopWatch.ElapsedTicks - startTicks));
            }

            DumpInputSummary(log);

        }

    }

}