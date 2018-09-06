/*
 * SFO
 *  SFO / PSF reader
 * 
 * http://hitmen.c02.at/files/yapspd/psp_doc/chap26.html
 * 
 * 
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Diagnostics;
using System.Runtime.InteropServices;
using Helpers;

namespace PS2ClassicsSfo
{
    public enum SFOType { Binary, Text, Int }

    public enum Category
    {
        Unknown = 0,
        WLanGame = 1,           // WG
        MemoryStickSave = 2,    // MS
        MemoryStickGame = 4,    // MG
        UmdGame = 8,            // UG
        UmdVideo = 16,          // UV
        UmdAudio = 32,          // UA
        UmdCleaningDisc = 64,   // UC
        DiscGame = 128,         // DG
        DiscMovie = 256,        // DM
        HardDiskGame = 512,     // HD
        GameData = 1024,        // GD
        DiscPackage = 2048,     // DP
        InstallPackage = 4096,  // IP
        GameSaveData = 8192,    // SD
        PS4GameApplication = 25703,
        PS4GameApplicationPatch = 28775,
        PS4AdditionalContent = 25441
    }

    [Flags]
    public enum VideoResolution
    {
        vr480 = 1,
        vr576 = 2,

        vr720 = 4,
        vr1080 = 8,

        vr480_16x9 = 16,
        vr576_16x9 = 32,
    }

    [Flags]
    public enum AudioFormat
    {
        LPCM2 = 1,
        LPCM5_1 = 4,
        LPCM7_1 = 16,
        DolbyDigital = 258,
        DTS = 514,
    }

    [DebuggerDisplay("{ToString()}")]
    public class SFOValue : IComparable
    {
        private string _keyName;
        private SFOType _valueType;
        private int _valueInt;
        private long _valueLong;
        private string _valueString;
        private byte[] _valueBinary;

        public SFOValue(string keyname, object newvalue)
        {
            if (newvalue is int)
            {
                _valueType = SFOType.Int;
                _keyName = keyname;
                _valueInt = (int)newvalue;
            }
            else if (newvalue is Int64)
            {
                _valueType = SFOType.Int;
                _keyName = keyname;
                _valueLong = (Int64)newvalue;
            }
            else
            if (newvalue is string)
            {
                _valueType = SFOType.Text;
                _keyName = keyname;
                _valueString = (string)newvalue;
            }
            else
            // if (newvalue is Array)
            {
                _valueType = SFOType.Binary;
                _keyName = keyname;
                _valueBinary = (byte[])newvalue;
            }
        }

        public string KeyName
        {
            get { return _keyName; }
        }

        public SFOType ValueType
        {
            get { return _valueType; }
        }

        public int ValueInt
        {
            get { return _valueInt; }
        }

        public string ValueString
        {
            get { return _valueString; }
        }

        public byte[] ValueBinary
        {
            get { return _valueBinary; }
        }

        public override string ToString()
        {
            switch (_valueType)
            {
                case SFOType.Binary:
                    {
                        return string.Format("{0} : {1} : '{2}'.", _keyName, "binary", _valueBinary);
                    }
                case SFOType.Int:
                    {
                        return string.Format("{0} : {1} : '{2}'.", _keyName, "int", _valueInt);
                    }
                case SFOType.Text:
                    {
                        return string.Format("{0} : {1} : '{2}'.", _keyName, "text", _valueString);
                    }
            }

            return base.ToString();
        }

        public int CompareTo(object obj)
        {
            string objkeyname = null;
            SFOValue objvalue = obj as SFOValue;
            if (objvalue != null)
            {
                objkeyname = objvalue._keyName;
            }

            return _keyName.CompareTo(objkeyname);
        }
    }

    public class SFO
    {
        #region SFO File Structs

        [StructLayout(LayoutKind.Sequential)]
        struct SFO_HEADER
        { 
            public byte magic;           // 00
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
            public char[] signature;    // PSF
            public byte FileVersionHigh; // 01
            public byte FileVersionLow;  // 01
            public short Unknown1;
            public uint Start_of_Variable_Name_Table; // Offset...
            public uint Start_of_Variable_Data_Table; // Offset...
            public uint NumberOfVariables; // Variables count
        }

        private enum DATA_TYPE : byte
        {
            BinaryData = 0,
            Utf8Text = 2,
            Si32Integer = 4
        }

        [StructLayout(LayoutKind.Sequential)]
        struct INDEX_TABLE_ENTRY
        {
            public ushort KeyNameOffset;
            public byte Unknown;
            public DATA_TYPE DataType;
            public uint ValueDataSize;
            public uint ValueDataSizePlusPadding;
            public uint DataValueOffset;
        }

        #endregion SFO File Structs

        private List<SFOValue> _values = new List<SFOValue>();

        public SFO(bool addrequiredvalues)
        {
            if (addrequiredvalues)
            {
                SetValue("ATTRIBUTE", 0);
                SetValue("LICENSE", "Library programs ©Sony Computer Entertainment Inc. Licensed for play on the PLAYSTATION®3 Computer Entertainment System or authorized PLAYSTATION®3 format systems. For full terms and conditions see the user's manual. This product is authorized and produced under license from Sony Computer Entertainment Inc. Use is subject to the copyright laws and the terms and conditions of the user's license.");
            }
        }

        public SFO(string filename)
        {
            using (Stream fs = File.OpenRead(filename))
            {
                using (StructReader sr = new StructReader(ByteOrder.LSB, fs))
                {
                    SFO_HEADER sfoheader = sr.ReadStruct<SFO_HEADER>();

                    if (
                        (sfoheader.magic != 0) ||
                        (sfoheader.signature[0] != 'P') ||
                        (sfoheader.signature[1] != 'S') ||
                        (sfoheader.signature[2] != 'F')
                       )
                    {
                        throw new Exception("Unknown file format!");
                    }

                    
                    // Read variable information...
                    INDEX_TABLE_ENTRY[] indexes = sr.ReadStructs<INDEX_TABLE_ENTRY>((int)sfoheader.NumberOfVariables);

                    
                    // Read variable names...
                    fs.Seek(sfoheader.Start_of_Variable_Name_Table, SeekOrigin.Begin);

                    string[] variablenames = sr.ReadStrings(StringType.NullTerminated, indexes.Length);

                    
                    // Read variable data...
                    for (int idx = 0; idx < indexes.Length; idx++)
                    {
                        INDEX_TABLE_ENTRY index = indexes[idx];
                        string indexname = variablenames[idx];

                        fs.Seek(sfoheader.Start_of_Variable_Data_Table + index.DataValueOffset, SeekOrigin.Begin);

                        switch (index.DataType)
                        {
                            case DATA_TYPE.BinaryData:
                                {
                                    byte[] valuedata = sr.ReadBytes((int)index.ValueDataSize);

                                    SFOValue newvalue = new SFOValue(indexname, valuedata);
                                    _values.Add(newvalue);

                                    break;
                                }
                            case DATA_TYPE.Si32Integer:
                                {
                                    Int64 intvalue = sr.ReadInt64();

                                    SFOValue newvalue = new SFOValue(indexname, intvalue);
                                    _values.Add(newvalue);

                                    break;
                                }
                            case DATA_TYPE.Utf8Text:
                                {
                                    string str = sr.ReadString(StringType.PlainText, (int)index.ValueDataSize - 1);

                                    SFOValue newvalue = new SFOValue(indexname, str);
                                    _values.Add(newvalue);

                                    break;
                                }
                            default:

                                string str1 = sr.ReadString(StringType.PlainText, (int)index.ValueDataSize - 1);

                                SFOValue newvalue1 = new SFOValue(indexname, str1);
                                _values.Add(newvalue1);


                                break;
                        }
                    }
                }
            }
        }

        public void SaveFile(string filename)
        {
            // Sort the values before we save them to the sfo file
            _values.Sort();

            using (FileStream stream = File.Create(filename))
            {
                using (StructWriter sw = new StructWriter(ByteOrder.LSB, stream))
                {
                    INDEX_TABLE_ENTRY[] indexes = new INDEX_TABLE_ENTRY[_values.Count];
                    string[] variablenames = new string[_values.Count];

                    int curkeynameoffset = 0;
                    uint curvalueoffset = 0;
                    for (int idx = 0; idx < _values.Count; idx++)
                    {
                        SFOValue value = _values[idx];

                        DATA_TYPE datatype = DATA_TYPE.BinaryData;
                        uint datasize = 0;
                        switch (value.ValueType)
                        {
                            case SFOType.Binary:
                                {
                                    datatype = DATA_TYPE.BinaryData;
                                    datasize = (uint)value.ValueBinary.Length;
                                    break;
                                }
                            case SFOType.Int:
                                {
                                    datatype = DATA_TYPE.Si32Integer;
                                    datasize = 4;
                                    break;
                                }
                            case SFOType.Text:
                                {
                                    datatype = DATA_TYPE.Utf8Text;
                                    datasize = (uint)Encoding.UTF8.GetBytes(value.ValueString).Length + 1;
                                    break;
                                }
                            default:
                                {
                                    throw new Exception("Unknown SFOType!");
                                }
                        }


                        indexes[idx].KeyNameOffset = (ushort)curkeynameoffset;
                        indexes[idx].Unknown = 4;
                        indexes[idx].DataType = datatype;
                        indexes[idx].ValueDataSize = datasize;
                        indexes[idx].ValueDataSizePlusPadding = GetPaddingSize(value.KeyName, datasize);
                        indexes[idx].DataValueOffset = curvalueoffset;

                        variablenames[idx] = value.KeyName;

                        curkeynameoffset += value.KeyName.Length + 1;
                        curvalueoffset += indexes[idx].ValueDataSizePlusPadding;
                    }

                    
                    SFO_HEADER sfoheader = new SFO_HEADER();
                    sfoheader.magic = 0;
                    sfoheader.signature = new char[] { 'P', 'S', 'F' };
                    sfoheader.FileVersionHigh = 1;
                    sfoheader.FileVersionLow = 1;
                    sfoheader.Unknown1 = 0;
                    sfoheader.Start_of_Variable_Name_Table = PadOffset(Marshal.SizeOf(sfoheader) + (indexes.Length * Marshal.SizeOf(typeof(INDEX_TABLE_ENTRY))));
                    sfoheader.Start_of_Variable_Data_Table = PadOffset(sfoheader.Start_of_Variable_Name_Table + curkeynameoffset);
                    sfoheader.NumberOfVariables = (uint)_values.Count;

                    sw.WriteStruct(sfoheader);

                    
                    // Write variable information...
                    sw.WriteStructs<INDEX_TABLE_ENTRY>(indexes);

                    WritePadBytes(sw, sw.BaseStream.Position, sfoheader.Start_of_Variable_Name_Table);

                    // Write variable names...
                    sw.WriteStrings(StringType.NullTerminated, variablenames);

                    WritePadBytes(sw, sw.BaseStream.Position, sfoheader.Start_of_Variable_Data_Table);

                    // Write variable data...
                    for (int idx = 0; idx < _values.Count; idx++)
                    {
                        SFOValue value = _values[idx];

                        switch (value.ValueType)
                        {
                            case SFOType.Binary:
                                {
                                    sw.Write(value.ValueBinary);
                                    break;
                                }
                            case SFOType.Int:
                                {
                                    sw.Write((Int32)value.ValueInt);
                                    break;
                                }
                            case SFOType.Text:
                                {
                                    sw.WriteString(StringType.NullTerminated, value.ValueString);
                                    break;
                                }
                        }

                        long pos = sw.BaseStream.Position;

                        WritePadBytes(sw, indexes[idx].ValueDataSize, indexes[idx].ValueDataSizePlusPadding);
                    }
                }
            }
        }

        private void WritePadBytes(StructWriter sw, long curlen, long wantedlen)
        {
            long padlength = wantedlen - curlen;
            if (padlength <= 0)
                return;


            byte[] buffer = new byte[padlength];
            for (int padidx = 0; padidx < buffer.Length; padidx++)
            {
                buffer[padidx] = 0;
            }

            sw.Write(buffer);
        }

        private uint GetPaddingSize(string keyname, uint datasize)
        {
            uint knownlength = 0;
            switch (keyname.ToUpper())
            {
                case "LICENSE":
                    {
                        knownlength = 512;
                        break;
                    }
                case "TITLE":
                    {
                        knownlength = 128;
                        break;
                    }
                case "TITLE_ID":
                    {
                        knownlength = 16;
                        break;
                    }
            }

            if (knownlength > 0)
            {
                if (datasize > knownlength)
                    throw new Exception(string.Format("{0} too long. Max length = {1}.", keyname, knownlength));
                return knownlength;
            }

            
            if (datasize <= 4)
                return 4;

            if (datasize <= 8)
                return 8;

            if (datasize <= 16)
                return 16;

            // for (int curbit = 2; curbit < 12; curbit++)
            for (int curbit = 7; curbit < 12; curbit++)
            {
                int cursize = 1 << curbit;

                if (datasize <= cursize)
                    return (uint)cursize;
            }

            return PadOffset(datasize);
        }

        private uint PadOffset(long offset)
        {
            return PadOffset((uint)offset);
        }

        private uint PadOffset(int offset)
        {
            return PadOffset((uint)offset);
        }

        private uint PadOffset(uint offset)
        {
            // Pad the value to 4 bytes
            return (uint)((offset + 3) / 4) * 4;
        }

        #region Property Helpers

        private string GetValueString(string keyname, string defaultvalue)
        {
            var entries = from v in _values
                          where v.KeyName == keyname
                          select v;

            if (entries.Count() == 1)
            {
                return entries.First().ValueString;
            }

            return defaultvalue;
        }

        private int GetValueInt(string keyname, int defaultvalue)
        {
            var entries = from v in _values
                          where v.KeyName == keyname
                          select v;

            if (entries.Count() == 1)
            {
                return entries.First().ValueInt;
            }

            return defaultvalue;
        }

        private void SetValue(string keyname, object newvaluedata)
        {
            var entries = from v in _values
                          where v.KeyName == keyname
                          select v;

            List<SFOValue> values = entries.ToList();
            foreach (SFOValue value in values)
            {
                _values.Remove(value);
            }

            SFOValue newvalue = new SFOValue(keyname, newvaluedata);
            _values.Add(newvalue);
        }

        #endregion Property Helpers

        #region Properties

        public List<SFOValue> Values
        {
            get { return _values; }
        }

        public string ApplicationVersion
        {
            get { return GetValueString("APP_VER", ""); }
            set { SetValue("APP_VER", value); }
        }

        public bool Bootable
        {
            get { return (GetValueInt("BOOTABLE", 0) == 1); }
            set
            {
                int newvalue = value ? 1 : 0;
                SetValue("BOOTABLE", newvalue);
            }
        }

        public Set<VideoResolution> VideoResolution
        {
            get
            {
                return GetValueInt("RESOLUTION", 0);
            }
            set
            {
                SetValue("RESOLUTION", (int)value);
            }
        }

        public Set<AudioFormat> AudioFormat
        {
            get
            {
                return GetValueInt("SOUND_FORMAT", 0);
            }
            set
            {
                SetValue("SOUND_FORMAT", (int)value);
            }
        }

        public Category Category
        {
            get
            {
                switch (GetValueString("CATEGORY", ""))
                {
                    case "WG": return Category.WLanGame;
                    case "MS": return Category.MemoryStickSave;
                    case "MG": return Category.MemoryStickGame;
                    case "UG": return Category.UmdGame;
                    case "UV": return Category.UmdVideo;
                    case "UA": return Category.UmdAudio;
                    case "UC": return Category.UmdCleaningDisc;
                    case "DG": return Category.DiscGame;
                    case "DM": return Category.DiscMovie;
                    // case "HD": return Category.HardDiskGame;
                    case "HG": return Category.HardDiskGame; // HD ?
                    case "GD": return Category.GameData;
                    case "DP": return Category.DiscPackage;
                    case "IP": return Category.InstallPackage;
                    case "SD": return Category.GameSaveData;
                }

                return Category.Unknown;
            }
            set
            {
                switch (value)
                {
                    case Category.WLanGame:
                        {
                            SetValue("CATEGORY", "WG");
                            break;
                        }
                    case Category.MemoryStickSave:
                        {
                            SetValue("CATEGORY", "MS");
                            break;
                        }
                    case Category.MemoryStickGame:
                        {
                            SetValue("CATEGORY", "MG");
                            break;
                        }
                    case Category.UmdGame:
                        {
                            SetValue("CATEGORY", "UG");
                            break;
                        }
                    case Category.UmdVideo:
                        {
                            SetValue("CATEGORY", "UV");
                            break;
                        }
                    case Category.UmdAudio:
                        {
                            SetValue("CATEGORY", "UA");
                            break;
                        }
                    case Category.UmdCleaningDisc:
                        {
                            SetValue("CATEGORY", "UC");
                            break;
                        }
                    case Category.DiscGame:
                        {
                            SetValue("CATEGORY", "DG");
                            break;
                        }
                    case Category.DiscMovie:
                        {
                            SetValue("CATEGORY", "DM");
                            break;
                        }
                    case Category.HardDiskGame:
                        {
                            SetValue("CATEGORY", "HG"); // HD ?
                            break;
                        }
                    case Category.GameData:
                        {
                            SetValue("CATEGORY", "GD");
                            break;
                        }
                    case Category.DiscPackage:
                        {
                            SetValue("CATEGORY", "DP");
                            break;
                        }
                    case Category.InstallPackage:
                        {
                            SetValue("CATEGORY", "IP");
                            break;
                        }
                    case Category.GameSaveData:
                        {
                            SetValue("CATEGORY", "SD");
                            break;
                        }
                    default:
                        throw new Exception("Unknown category");
                }
            }
        }

        public string License
        {
            get { return GetValueString("LICENSE", ""); }
            set { SetValue("LICENSE", value); }
        }

        public int ParentalLevel
        {
            get { return GetValueInt("PARENTAL_LEVEL", 0); }
            set { SetValue("PARENTAL_LEVEL", value); }
        }

        public string SystemVersion
        {
            get { return GetValueString("SYSTEM_VER", ""); }
            set
            {
                // 01.9300
                //if (!string.IsNullOrEmpty(value))
                //{
                //    // Make sure the major version is 2 chars long
                //    if (value.IndexOf('.') == 1)
                //        value = "0" + value;

                //    // Make sure the total length of the version is 7 chars. That is 2 for the major and 4 for the minor version.
                //    value = value.PadRight(7, '0');
                //}
                SetValue("SYSTEM_VER", value);
            }
        }

        public string Title
        {
            get { return GetValueString("TITLE", ""); }
            set { SetValue("TITLE", value); }
        }

        /*
        public string TitleJapanese
        {
            get { return GetValueString("TITLE_00", ""); }
            set { SetValue("TITLE_00", value); }
        }

        public string TitleFrench
        {
            get { return GetValueString("TITLE_02", ""); }
            set { SetValue("TITLE_02", value); }
        }

        public string TitleSpanish
        {
            get { return GetValueString("TITLE_03", ""); }
            set { SetValue("TITLE_03", value); }
        }

        public string TitleGerman
        {
            get { return GetValueString("TITLE_04", ""); }
            set { SetValue("TITLE_04", value); }
        }

        public string TitleItalian
        {
            get { return GetValueString("TITLE_05", ""); }
            set { SetValue("TITLE_05", value); }
        }

        public string TitleDutch
        {
            get { return GetValueString("TITLE_06", ""); }
            set { SetValue("TITLE_06", value); }
        }

        public string TitlePortuguese
        {
            get { return GetValueString("TITLE_07", ""); }
            set { SetValue("TITLE_07", value); }
        }

        public string TitleRussian
        {
            get { return GetValueString("TITLE_08", ""); }
            set { SetValue("TITLE_08", value); }
        }
        */

        public string TitleId
        {
            get { return GetValueString("TITLE_ID", ""); }
            set
            {
                string[] parts = value.Split('-', '_');

                if (parts.Length == 4)
                {
                    SetValue("TITLE_ID", parts[1]);
                }
                else
                {
                    SetValue("TITLE_ID", value);
                }
            }
        }

        public string Version
        {
            get { return GetValueString("VERSION", ""); }
            set { SetValue("VERSION", value); }
        }

        public string ContentID
        {
            get { return GetValueString("CONTENT_ID", ""); }
            set { SetValue("CONTENT_ID", value); }
        }

        #endregion Properties
    }
}
