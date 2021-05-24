///////////////////////////////////////////////////
//Copyright Edonica

using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.IO;

namespace Edonica.XMLImport
{
    //Class for managing the processing of a single XML node.
    //Used so we can debug / determine details on the transformation.

    class WorkItem
    {
        //Parameters passed in...
        public string filename;
        public Transformer program;
        public XmlTextReader reader;
        public OutputGeneric outputProcessor;

        //Where we are in the input
        public int lineNumberBefore;
        public int lineNumberAfter;

        //The XML node we parsed
        public XmlNode nodeIn;

        //Are we debugging this work item?
        bool debug;

        //The input summary node
        Transformer.SummaryForInputNode inputSummary;

        //The output from the transform
        string outputString;

        //The parsed output from the transform
        XmlDocument docOut;

        //The output element we are actually processing
        XmlElement valueNode;


        //Debug functions...
        static string sConsoleSep = "==========================================";
        void DumpXMLNode(string name, XmlNode node)
        {
            if (node == null)
            {
                program.Log(name + " : null");
            }
            else
            {
                program.Log(name);
                program.Log(sConsoleSep );
                using (StringWriter stringWriter = new StringWriter())
                {
                    using (XmlTextWriter xmlWriter = new XmlTextWriter(stringWriter))
                    {
                        xmlWriter.Formatting = Formatting.Indented;
                        node.WriteTo(xmlWriter);
                    }
                    program.Log(stringWriter.ToString());
                }
                program.Log("");
                program.Log(sConsoleSep);
            }
        }
        //Write debug info about this WorkItem.
        void DumpDebug( TextWriter writer )
        {
            writer.WriteLine("Work item " + filename + " - " + lineNumberBefore.ToString() + " to " + lineNumberAfter.ToString());
            DumpXMLNode("Input XML", nodeIn );
            if (outputString != null)
            {
                writer.WriteLine("Output string:\n{0}\n{1}\n{2}\n", sConsoleSep, outputString, sConsoleSep);
            }
            else
            {
                writer.WriteLine("Output string : null");
            }


            DumpXMLNode("XML document output", docOut);
            DumpXMLNode("Value node", valueNode);

        }

        public void ReadInput()
        {
            long start = program.stopWatch.ElapsedTicks;

            lineNumberBefore = reader.LineNumber;
            nodeIn = program.loaderDocument.ReadNode(reader);
            lineNumberAfter = reader.LineNumber;


            if (lineNumberBefore <= program.config.DebugLine && program.config.DebugLine <= lineNumberAfter)
            {
                program.Log("Debugging element at line " + lineNumberAfter);
                debug = true;
            }
            inputSummary = program.GetSummaryNodeForInputElement(nodeIn.Name);
            inputSummary.NumItems++;
            inputSummary.timeInput += program.stopWatch.ElapsedTicks - start;

        }

        public void DoTransform()
        {
            long start = program.stopWatch.ElapsedTicks;

            StringWriter outputStringWriter = new StringWriter();
            using (XmlWriter outputXmlWriter = new XmlTextWriter(outputStringWriter))
            {
                program.transform.Transform(nodeIn, outputXmlWriter);
                outputXmlWriter.Flush();
            }
            outputString = outputStringWriter.ToString();
            inputSummary.timeTransform += program.stopWatch.ElapsedTicks - start;

        }

        public void DoOutput()
        {

            if (outputString.Trim() == "")
            {
                //program.Log("No output for " + nodeIn.Name + " at " + lineNumber.ToString());
                inputSummary.Ignored++;
                if (program.config.ShowIgnoredElements)
                {
                    program.Log("Ignoring element at line : " + lineNumberBefore);
                    DumpDebug(program.log);
                }
            }
            else
            {
                try
                {
                    long start = program.stopWatch.ElapsedTicks;
                    this.docOut = new XmlDocument();
                    docOut.LoadXml(outputString);
                    XmlElement rootElement = docOut.DocumentElement;
                    if (rootElement.Name != "TransformOutput")
                        throw new Exception("Root element was \"" +rootElement.Name+ "\" expected \"TransformOutput\"");

                    inputSummary.timeOutputParse += program.stopWatch.ElapsedTicks - start;
                    start = program.stopWatch.ElapsedTicks;
                    foreach (XmlNode node in rootElement.ChildNodes)
                    {
                        valueNode = node as XmlElement;
                        if (valueNode == null)
                            continue;
                        if (valueNode.Name == "TransformLog")
                        {
                            program.Log("LOG line " + lineNumberAfter.ToString() + " : " + valueNode.InnerXml.Trim());
                        }
                        else
                        {
                            outputProcessor.OutputRecord(valueNode);
                            if (debug)
                            {
                                program.Log("Transform " + nodeIn.Name + " to " + docOut.DocumentElement.Name + " at " + lineNumberAfter.ToString());
                            }
                            inputSummary.AddOutputCount( valueNode.Name );

                        }
                    }
                    inputSummary.timeOutputDo += program.stopWatch.ElapsedTicks - start;


                }
                catch (Exception ex)
                {
                    inputSummary.Errors++;
                    program.Log("Error processing output for " + nodeIn.Name + " at " + lineNumberAfter.ToString() + " : " + ex.Message);
                    this.DumpDebug(program.log);

                }
            }
        }


        public void Process()
        {
            ReadInput();
            //Skip data that we're not debugging.
            if (program.config.DebugLine > 0 && !debug)
                return;


            DoTransform();

            DoOutput();

            if (debug)
            {
                DumpDebug(program.log);
            }
            
            if (!String.IsNullOrEmpty(program.config.TraceOutputDirectory))
            {
                if (!String.IsNullOrEmpty(program.config.TraceOutputDirectory))
                {
                    using( TextWriter writer = File.CreateText(Path.Combine(program.config.TraceOutputDirectory, lineNumberBefore.ToString() + ".XML")) )
                    {
                        DumpDebug(writer);
                    }
                }
            }
        }
    }

}
