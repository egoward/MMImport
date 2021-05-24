using System;
using System.Collections.Generic;
using System.Text;
using System.IO;


namespace Edonica.Projection
{
    /// <summary>
    ///Parse data as obtained from here...
    ///  http://www.ordnancesurvey.co.uk/oswebsite/gps/osnetfreeservices/furtherinfo/questdeveloper.html
    ///Data is read in CSV format and written to a binary file for quick access
    /// </summary>
    public class OSTNDataParser
    {
        internal struct Sample
        {
            public Sample(UInt16 x, UInt16 y, UInt16 z)
            {
                this.x = x;
                this.y = y;
                this.z = z;
            }

            public static Sample Invalid
            {
                get
                {
                    return new Sample(UInt16.MaxValue, UInt16.MaxValue, UInt16.MaxValue);
                }
            }
            public UInt16 x;
            public UInt16 y;
            public UInt16 z;
        }


        static UInt16 GetSampleValue(double v, int offset)
        {
            double dToStore = (v - offset) * 1000;
            if( dToStore < UInt16.MinValue || dToStore > UInt16.MaxValue )
                throw new Exception("Unable to store value with UInt16");

            UInt16 uiToStore = (UInt16)Math.Round(dToStore);

            if (Math.Abs(dToStore - uiToStore) > 0.000001)
                throw new Exception("Unable to store value with UInt16");

            return uiToStore;
        }


        /// <summary>
        /// Convert a CSV file to binary
        /// </summary>
        /// <param name="inputFile">Path to CSV file, eg. "OSTN02_OSGM02_GB.txt"</param>
        /// <param name="outputFile">Path to binary translation file, eg "OSTN02.DAT"</param>
        public static void ConvertCSV(string inputFile, string outputFile)
        {

            int SampleCountX = 700;
            int SampleCountY = 1250;
            int Interval = 1000;

            Sample[,] samples = new Sample[SampleCountX, SampleCountY];

            for (int y = 0; y < SampleCountY; y++)
            {
                for (int x = 0; x < SampleCountX; x++)
                {
                    samples[x, y] = Sample.Invalid;
                }
            }

            int OffsetX = 60;
            int OffsetY = -100;
            int OffsetZ = 20;

            using (StreamReader reader = File.OpenText(inputFile))
            {
                while (!reader.EndOfStream)
                {
                    string line = reader.ReadLine();
                    string[] bits = line.Split(new char[]{','});

                    int sampleIndex = Int32.Parse(bits[0]);
                    int x = Int32.Parse(bits[1]);
                    int y = Int32.Parse(bits[2]);
                    double shiftX = Double.Parse(bits[3]);
                    double shiftY = Double.Parse(bits[4]);
                    double shiftZ = Double.Parse(bits[5]);
                    string datum = bits[6];

                    if (datum != "0")
                    {
                        x /= Interval;
                        y /= Interval;

                        samples[x, y] = new Sample(
                            GetSampleValue(shiftX, OffsetX),
                            GetSampleValue(shiftY, OffsetY),
                            GetSampleValue(shiftZ, OffsetZ));

                    }


                }
            }


            using (FileStream fstream = new FileStream(outputFile, FileMode.Create))
            {
                using (BinaryWriter writer = new BinaryWriter(fstream))
                {
                    writer.Write(new char[] { 'E', 'D', 'O', 'S', 'T', 'N', '0', '2' });
                    writer.Write((Int32)1); //Version

                    writer.Write((Int32)SampleCountX);
                    writer.Write((Int32)SampleCountY);
                    int entrySize = System.Runtime.InteropServices.Marshal.SizeOf(typeof(Sample));
                    writer.Write(entrySize);

                    writer.Write((Int32)OffsetX);
                    writer.Write((Int32)OffsetY);
                    writer.Write((Int32)OffsetZ);

                    for (int y = 0; y < SampleCountY; y++)
                    {
                        for (int x = 0; x < SampleCountX; x++)
                        {
                            writer.Write((UInt16)samples[x, y].x);
                            writer.Write((UInt16)samples[x, y].y);
                            writer.Write((UInt16)samples[x, y].z);
                        }
                    }


                }
            }


        }



    }
}
