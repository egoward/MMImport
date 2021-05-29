using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Windows;

using Edonica.Projection;
using MMImportCore;

using Edonica.MapBase;


namespace Edonica.XMLImport
{
    class AppCmdLine
    {
        public class CommandLineException : Exception
        {
            public CommandLineException( string s ) : base(s ) {}
        }

        public static string defaultConfigFile = "Default.XMLImport";

        public static string ExecutableName
        {
            get
            {
                System.Reflection.Assembly asm = System.Reflection.Assembly.GetExecutingAssembly();
                return Path.GetFileNameWithoutExtension(asm.Location);
            }
        }

        public static void DumpCommandLine( string error )
        {
            string commandLineMessage = 
                "Command line:\n" +
                "  " + ExecutableName + " [/ConfigFile:<Filename>] Command [Arguments]\n" +
                "Default config file : " + defaultConfigFile + "\n" + 
                "Available commands:\n" +
                "  CONFIG_CREATE         Create a new configuration file\n" +
                "  CONFIG_TEST           Test the configuration / database connection\n" +
                "  QUERY                 Query the database and get summary info\n" +
                "  TABLE_CREATE          Create tables in database\n" +
                "  TABLE_DROP            Drop tables in database\n" +
                "  IMPORT File1, File2...Import some files into the database.  (eg. *.gz)\n" +
                "  REMOVE_DUPLICATES     Remove any duplicates from the database\n" +
                "  INDEX_CREATE          Add indexes to the database (will stop adds)\n" +
                "  INDEX_DROP            Remove indexes from database (will allow adds)\n" +
                "  UPDATE_APPLY          Apply updates to the database\n" +
                "  TRANSFORM             Transform geometry in an existing map using OSTN02\n" +
                " eg.\n" + 
                " To create a new configuration file\n" + 
                "  " + ExecutableName + " /ConfigFile:MySettings.xml CONFIG_CREATE\n" +
                " To edit the configuration\n" + 
                "  vi CONFIG_CREATE\n" +
                " To test the configuration\n" + 
                "  " + ExecutableName + " /ConfigFile:MySettings.xml CONFIG_TEST\n" +
                " To create database tables\n" +
                "  " + ExecutableName + " /ConfigFile:MySettings.xml TABLE_CREATE\n"+
                " To import 'gz' files in this directory:\n" + 
                "  " + ExecutableName + " /ConfigFile:MySettings.xml IMPORT *.gz\n" +
                " For help on transforming coordinates:\n" + 
                "  " + ExecutableName + " TRANSFORM HELP\n";

            Console.WriteLine( commandLineMessage );

            Console.WriteLine("ERROR : " + error);
        }

        delegate void ImportAction( Transformer prog );

        static void DoAction( ConfigGeneral config, ImportAction action )
        {
            try
            {
                Transformer prog = new Transformer();
                prog.log = Console.Out;
                prog.Log("Started at " + DateTime.Now.ToString(Transformer.dateTimeFormatString));
                prog.config = config;
                prog.BaseInit();
                prog.CheckCancelFlag = delegate() { return false; };    //No cancel method
                action(prog);
                prog.BaseShutdown();
                prog.Log("Finished at " + DateTime.Now.ToString(Transformer.dateTimeFormatString));
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                Console.WriteLine();
                Console.WriteLine("Please check your configuration.  If valid, please send this error, ");
                Console.WriteLine("your configuration and the associated data to Edonica.");
                Console.WriteLine();
                Console.WriteLine("Error:\n" + ex.Message);
            }
        }


        static string GetMandatoryValue(Dictionary<string, string> keys, string key)
        {
            string ret = GetOptionalValue(keys, key,null);
            if (string.IsNullOrEmpty(ret))
                throw new CommandLineException("Switch not specified : " + key);
            return ret;
        }
        static string GetOptionalValue(Dictionary<string, string> keys, string key, string defaultValue)
        {
            string ret;
            if (!keys.TryGetValue(key.ToUpper(), out ret))
            {
                return defaultValue;
            }
            return ret;
        }
        static object TryReadEnum(Type t ,string s)
        {
            try
            {
                return Enum.Parse(t, s, true);
            }
            catch( Exception  )
            {
                string[] names = Enum.GetNames( t );
                string error = "Invalid parameter : [" + s + "] - possible values : " + names[0];
                for(int c=1;c<names.Length;c++)
                {
                    error+=","+  names[c];
                }
                throw new CommandLineException(error);
            }
        }

        public static void Main(string[] args)
        {
            string configFile = defaultConfigFile;
            List<string> commands = new List<string>();
            Dictionary<string,string> switches = new Dictionary<string,string>();
            foreach (string arg in args)
            {
                if (arg.StartsWith("/"))
                {
                    string switchArg = arg.Substring(1);
                    int idx = switchArg.IndexOf(':');
                    if (idx < 0)
                    {
                        switches.Add(switchArg, "");
                    }
                    else
                    {
                        string key = switchArg.Substring(0, idx);
                        string val = switchArg.Substring(idx+1);
                        switches.Add(key.ToUpper(), val);
                    }
                    //if (switchArg.StartsWith("ConfigFile:", StringComparison.CurrentCultureIgnoreCase))
                    //{
                    //    configFile = switchArg.Substring("ConfigFile:".Length);
                    //}

                }
                else
                {
                    commands.Add(arg);
                }
            }

            if (switches.ContainsKey("CONFIGFILE"))
            {
                configFile = switches["CONFIGFILE"];
            }



#if DEBUG
            double x = 651307.003;
            double y = 313255.686;
            //double x = 522827.614;//651409.903;
            //double y = 186129.406;//313177.270;
            //double x = 522827.614;
            //double y = 186129.406;
            //Point SimpleTransform1 = LatLongNGR.ConvertETRS89GridToOSGB36(new Point(x, y));
            Point p = LatLongNGR.ConvertOSGB36ToWGS84(new Point(x, y));
            //Point p = LatLongNGR.ConvertETRS89GridToWGS84(new Point(x, y));
            
            //Console.WriteLine(p.X);
            //Console.WriteLine(p.Y);
#endif
            

            if (commands.Count == 0)
            {
                DumpCommandLine("ERROR : No command specified");
                return;
            }

            string command = commands[0].ToUpper();
            if (command == "CONFIG_CREATE")
            {
                ConfigGeneral cfg = new ConfigGeneral();
                cfg.OutputConfig = new ConfigOutputOGR();
                Console.WriteLine("Writing default configuration to " + configFile);
                cfg.Save(configFile);
                Console.WriteLine("Done.");
            }
            else if (command == "TRANSFORM")
            {
                TransformMap(switches);
                return;
            }
            else
            {
                if (!File.Exists(configFile))
                {
                    Console.WriteLine("Configuration file " + configFile + " not found");
                    Console.WriteLine("try using CONFIG_CREATE first to create a config file");
                    return;
                }
                ConfigGeneral config = ConfigGeneral.Load(configFile);
                switch (command.ToUpper())
                {
                    case "CONFIG_TEST":
                        {
                            DoAction(config, delegate(Transformer prog)
                                {
                                });
                        }
                        break;
                    case "QUERY":
                        {
                            DoAction(config, delegate(Transformer prog)
                                {
                                    prog.outputProcessor.QueryInformation();
                                });
                        }
                        break;
                    case "TABLE_CREATE":
                        {
                            DoAction(config, delegate(Transformer prog)
                                {
                                    prog.outputProcessor.CreateTables();
                                });
                        }
                        break;
                    case "TABLE_DROP":
                        {
                            DoAction(config, delegate(Transformer prog)
                                {
                                    prog.outputProcessor.DropTables();
                                });
                        }
                        break;
                    case "IMPORT":
                        {
                            if (commands.Count <= 1)
                            {
                                DumpCommandLine("No files specified to import");
                                return;
                            }
                            DoAction(config, delegate(Transformer prog)
                                {
                                    for (int c = 1; c < args.Length; c++)
                                    {
                                        string directory = Path.GetDirectoryName(commands[1]);
                                        string extension = Path.GetFileName(commands[1]);

                                        Console.WriteLine("Processing [" + directory + "] - [" + extension + "]");

                                        foreach (string file in Directory.GetFiles(directory, extension))
                                        {
                                            Console.WriteLine(file);
                                            prog.ProcessFile(file);
                                        }
                                    }
                                });
                        }
                        break;
                    case "REMOVE_DUPLICATES":
                        {
                            DoAction(config, delegate(Transformer prog)
                                {
                                    prog.outputProcessor.RemoveDuplicates();
                                });
                        }
                        break;
                    case "INDEX_CREATE":
                        {
                            DoAction(config, delegate(Transformer prog)
                                {
                                    prog.outputProcessor.CreateIndexes();
                                });
                        }
                        break;
                    case "INDEX_DROP":
                        {
                            DoAction(config, delegate(Transformer prog)
                                {
                                    prog.outputProcessor.DropIndexes();
                                });
                        }
                        break;
                    case "UPDATE_APPLY":
                        {
                            DoAction(config, delegate(Transformer prog)
                                {
                                    prog.outputProcessor.ApplyUpdates();
                                });
                        }
                        break;
                    default:
                        {
                            DumpCommandLine("Unknown command : " + command);
                            return;
                        }
                }
            }
        }

        private static void TransformMap(Dictionary<string, string> switches)
        {
            string transformCommandLine =
                "Transforms coordinates between OSGB1936 and WGS84 (aka ETRS89,LatLong,GPS)\nUses Ordnance Survey OSTN02 transformation\n" +
                "  " + ExecutableName + " TRANSFORM \n" +
                "  [/SourceDriver:<Driver>]    default \"ESRI Shapefile\"\n" +
                "  [/SourceFolder:<Folder>]    default is working directory\n" +
                "  /SourceLayer:<Filename>     eg. \"Map1.SHP\"\n" +
                "  [/TargetDriver<Driver>]     default \"ESRI Shapefile\"\n" +
                "  [/TargetFolder:<Folder>]    default is working directory\n" +
                "  /TargetLayer:<Filename>     eg. \"Map1.SHP\"\n" +
                "  /ProjFrom:<WGS84|OSGB1936>  eg. WGS84\n" +
                "  /ProjTo:<WGS84|OSGB1936>    eg. OSGB1936\n" +
                "\n" +
                "  MMImportCommandLine TRANSFORM /SourceFolder:c:\\Temp\\WGS84 /SourceLayer:Map1 /TargetFolder:c:\\Temp\\OSGB1936 /TargetLayer:Map1 /ProjFrom:WGS84 /ProjTo:OSGB1936\n" +
                "\n" +
                "  MMImportCommandLine transform /SourceLayer:Map2 /TargetLayer:Map2OS /ProjFrom:WGS84 /ProjTo:OSGB1936\n" +
                "\n" +
                "Note: Uses GDAL internally as an abstraction layer.\n"+
                "Remove trailing backslashes from folder names and extensions from filenames.";


            try
            {
                string sourceDriver = GetOptionalValue(switches, "SourceDriver", "ESRI Shapefile");
                string sourceFolder = GetOptionalValue(switches, "SourceFolder", Environment.CurrentDirectory);
                string sourceLayer = GetMandatoryValue(switches, "SourceLayer");
                string targetDriver = GetOptionalValue(switches, "TargetDriver", "ESRI Shapefile");
                string targetFolder = GetOptionalValue(switches, "TargetFolder",Environment.CurrentDirectory);
                string targetLayer = GetMandatoryValue(switches, "TargetLayer");

                //GDAL can't handle folders with trailing back slashes for some reason.
                if (sourceFolder.EndsWith("\\")) sourceFolder = sourceFolder.Substring(0, sourceFolder.Length - 1);
                if (targetFolder.EndsWith("\\")) targetFolder = targetFolder.Substring(0, targetFolder.Length - 1);
                if (sourceLayer.EndsWith(".SHP", StringComparison.CurrentCultureIgnoreCase))
                    sourceLayer = sourceLayer.Substring(0, sourceLayer.Length - 4);
                if (targetLayer.EndsWith(".SHP", StringComparison.CurrentCultureIgnoreCase))
                    targetLayer = targetLayer.Substring(0, targetLayer.Length - 4);

                KnownProjection projFrom = (KnownProjection)TryReadEnum(typeof(KnownProjection), GetMandatoryValue(switches, "ProjFrom"));
                KnownProjection projTo = (KnownProjection)TryReadEnum(typeof(KnownProjection), GetMandatoryValue(switches, "ProjTo"));

                if (String.IsNullOrEmpty(sourceLayer))
                {
                    Console.WriteLine(transformCommandLine);
                    return;
                }


                MapGeometry.PointOperation transformOp = null;
                if (projFrom == KnownProjection.OSGB1936 && projTo == KnownProjection.WGS84)
                {
                    transformOp = delegate(ref Point p)
                    {
                        p = LatLongNGR.ConvertOSGB36ToWGS84(p);
                    };
                }
                else if (projFrom == KnownProjection.WGS84 && projTo == KnownProjection.OSGB1936)
                {
                    transformOp = delegate(ref Point p)
                    {
                        p = LatLongNGR.ConvertWGS84ToOSGB36(p);
                    };
                }
                else if (projFrom == projTo)
                {
                    transformOp = delegate(ref Point p)
                    {
                        //Do nothing
                    };
                }
                else
                {
                    throw new CommandLineException("Supported projections : OSGB1936->WGS84, WGS84->OSGB1936");
                }

                TransformUtils.TransformGeometry(Console.Out, sourceDriver, sourceFolder, sourceLayer, targetDriver, targetFolder, targetLayer, transformOp);

            }
            catch (CommandLineException ex)
            {
                Console.WriteLine(transformCommandLine);
                Console.WriteLine();
                Console.WriteLine(ex.Message);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                Console.WriteLine("----------------------------------");
                Console.WriteLine(ex.Message);
            }
        }
    }
}

/*
namespace System.Drawing.Design
{
    class UITypeEditor
    {
    }

}
*/

namespace Edonica.XMLImport
{
    class BrowsableFileAttribute : Attribute
    {
        public string FileFilter { set { } get { return ""; } }
    }

    class UITypeEditorEditBigString : System.Drawing.Design.UITypeEditor
    {
    }

    class UITypeEditorFileBrowseFolder : System.Drawing.Design.UITypeEditor
    {
    }

    class UITypeEditorFileBrowseFile : System.Drawing.Design.UITypeEditor
    {
    }
}


