using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace Edonica.Projection
{
    internal class GeoCorrectionReader
    {

        internal class CorrectionRecord
        {
            public double ShiftX;
            public double ShiftY;
            public double ShiftH;
        }

        public bool IsInitialised = false;
        public bool HasOSTNData = false;

        FileStream fIn = null;
        BinaryReader reader = null;

        int GridSizeX, GridSizeY, EntrySize;
        int OffsetX, OffsetY, OffsetH;
        long DataBase;

        public string GetOSTNFileName()
        {
            string sPath = System.IO.Path.GetDirectoryName(typeof(GeoCorrectionReader).Module.FullyQualifiedName);
            return Path.Combine(sPath, "OSTN02.DAT");
        }

        public void EnsureInitialised()
        {
            if (IsInitialised)
                return;

            string sFilename = GetOSTNFileName();

            if (File.Exists(sFilename))
            {
                Open(sFilename);
                HasOSTNData = true;
            }
            else
            {
                HasOSTNData = false;
            }
            IsInitialised = true;

        }

        public void Open(string sFilename)
        {
            fIn = new FileStream(sFilename, FileMode.Open);
            reader = new BinaryReader(fIn);

            char[] SignatureExpected = new char[] { 'E', 'D', 'O', 'S', 'T', 'N', '0', '2' };

            char[] SignatureFound = reader.ReadChars(8);
            for (int c = 0; c < 8; c++)
            {
                if (SignatureExpected[c] != SignatureFound[c])
                {
                    throw new Exception("GeoCorrectionReader - invalid file header");
                }
            }

            int iVersion = reader.ReadInt32();
            if (iVersion != 1)
            {
                throw new Exception("GeoCorrectionReader - data file version mismatch");
            }

            GridSizeX = reader.ReadInt32();
            GridSizeY = reader.ReadInt32();
            EntrySize = reader.ReadInt32();      //4 bytes - size of entries = 3*2 bytes=6 bytes

            OffsetX = reader.ReadInt32();
            OffsetY = reader.ReadInt32();
            OffsetH = reader.ReadInt32();

            DataBase = reader.BaseStream.Position;

        }

        double Translate(ushort val, int BaseVal)
        {
            return val / 1000.0 + BaseVal;
        }


        public bool GetRecord(int x, int y, out CorrectionRecord record)
        {
            if (x < 0 || y < 0 || x >= GridSizeX || y >= GridSizeY)
            {
                record = new CorrectionRecord();
                return false;
            }

            long Offset = DataBase + y * EntrySize * GridSizeX + x * EntrySize;

            fIn.Seek(Offset, SeekOrigin.Begin);

            ushort usx = reader.ReadUInt16();
            ushort usy = reader.ReadUInt16();
            ushort ush = reader.ReadUInt16();
            if (usx == ushort.MaxValue && usy == ushort.MaxValue && ush == ushort.MaxValue)
            {
                record = new CorrectionRecord();
                return false;
            }

            record = new CorrectionRecord();
            record.ShiftX = Translate(usx, OffsetX);
            record.ShiftY = Translate(usy, OffsetY);
            record.ShiftH = Translate(ush, OffsetH);
            return true;
        }
    }
}
