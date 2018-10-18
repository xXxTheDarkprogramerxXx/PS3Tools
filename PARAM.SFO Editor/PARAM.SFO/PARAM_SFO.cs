/* Copyright (c) 2015 - 2018 TheDarkporgramer
*
* This was originally done by Jappi88 (Jappi88 at Gmail dot com) https://github.com/Jappi88
* All modifications have been TheDarkporgramer (save sfo ext ext ) https://github.com/xXxTheDarkprogramerxXx
* 
* This(software Is provided) 'as-is', without any express or implied
* warranty. In no event will the authors be held liable for any damages arising from the use of this software.
*
* Permission is granted to anyone to use this software for any purpose,
* including commercial applications*, and to alter it and redistribute it
* freely, subject to the following restrictions:
*
* 1. The origin of this software must not be misrepresented; you must not
*   claim that you wrote the original software. If you use this software
*   in a product, an acknowledge in the product documentation is required.
*
* 2. Altered source versions must be plainly marked as such, and must not
*    be misrepresented as being the original software.
*
* 3. This notice may not be removed or altered from any source distribution.
*
* *Contact must be made to discuses permission and terms.
*/

using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using PARAM.SFO;
using System.Runtime.InteropServices;
using System.Linq;

namespace Param_SFO
{
    public class PARAM_SFO
    {
        #region << Enums >>
        public enum DataTypes : uint
        {
            PSN_Game =18248,
            GameData = 0x4744,
            SaveData = 0x5344,
            AppPhoto = 0x4150,
            AppMusic = 0x414D,
            AppVideo = 0x4156,
            BroadCastVideo = 0x4256,
            AppleTV = 4154,
            WebTV = 5754,
            CellBE = 0x4342,
            Home = 0x484D,
            StoreFronted = 0x5346,
            HDDGame = 0x4847,
            DiscGame = 0x4447,
            AutoInstallRoot = 0x4152,
            DiscPackage = 0x4450,
            ExtraRoot = 0x5852,
            VideoRoot = 0x5652,
            ThemeRoot = 0x5452,
            DiscMovie = 0x444D,
            Game_Digital_Application = 0x4081AC0,//GD
            PS4_Game_Application_Patch = 28775,
            Additional_Content = 25441,//PSvita PS4
            GameContent=25447,//PSVITA
            Blu_Ray_Disc = 25698,//PS4
            None
        }

        public enum FMT : ushort
        {
            UTF_8 = 0x0004,
            ASCII = 0x0402,
            Utf8Null = 0x0204,
            UINT32 = 0x0404,
        }

        #endregion << Enums >>

        #region << Vars>>
        public List<Table> Tables { get; set; }

        #endregion << Vars>>

        #region << Example Of Calling Functions >>
        //ypu can use this as SFO.Atribute 
        public string Attribute
        {
            get
            {
                if (Tables == null)
                    return "";
                foreach(Table t in Tables)
                {
                    if (t.Name == "ATTRIBUTE")
                        return t.Value;
                }
                return "";
            }
        }

        public DataTypes DataType
        {
            get
            {
                if (Tables == null)
                    return DataTypes.None;
                foreach (Table t in Tables)
                    if (t.Name == "CATEGORY")
                        return ((DataTypes)BitConverter.ToUInt16(Encoding.UTF8.GetBytes(t.Value), 0));
                return DataTypes.None;
            }
        }

        public string APP_VER
        {
            get
            {
                if (Tables == null)
                    return "";
                foreach (Table t in Tables)
                {
                    if (t.Name == "APP_VER")
                        return t.Value;
                }
                return "";
            }
        }

        public string Detail
        {
            get
            {
                if (Tables == null)
                    return "";
                foreach (Table t in Tables)
                    if (t.Name == "DETAIL")
                        return t.Value;
                return "";
            }
        }

        public string ContentID
        {
            get
            {
                if (Tables == null)
                    return "";
                foreach (Table t in Tables)
                    if (t.Name == "CONTENT_ID")
                        return t.Value;
                return "";
            }
        }

        public string TITLEID
        {
            get
            {
                if (Tables == null)
                    return "";
                foreach (Table t in Tables)
                    if (t.Name == "TITLE_ID")
                        return t.Value;
                return "";
            }
        }

        public string TitleID
        {
            get
            {
                string name = TITLEID;
                if (name == "")
                    return "";
                return name.Split('-')[0];
            }
        }

        public string Title
        {
            get
            {
                if (Tables == null)
                    return "";
                foreach (Table t in Tables)
                    if (t.Name == "TITLE")
                        return t.Value;
                return "";
            }
        }

        public string Category
        {
            get
            {
                if (Tables == null)
                    return "";
                foreach (Table t in Tables)
                    if (t.Name == "CATEGORY")
                        return t.Value;
                return "";
            }
        }

        public enum Playstation
        {
            ps3 = 0,
            psvita = 1,
            ps4 = 2,
            psp = 3,
            unknown = 4,//there will be a time i no longer support the scene this will be for ps5+ most probabbly
        }

        public Playstation PlaystationVersion
        {
            get
            {
                if (Tables == null)
                    return Playstation.unknown;
                foreach (Table t in Tables)
                {
                    if (t.Name == "PS3_SYSTEM_VER")
                        return Playstation.ps3;//this is the unique offset for ps3
                    if (t.Name == "PSP2_SYSTEM_VER")
                    {
                        return Playstation.psvita;//this is the only flag that tells us its a psvita
                    }
                    if (t.Name == "PSP_SYSTEM_VER")
                    {
                        return Playstation.psp;//this is how we know its a psp
                    }
                    if (t.Name == "SYSTEM_VER")//i believe this to be only ps4
                    {
                        return Playstation.ps4;
                    }
                }  
                return Playstation.unknown;
            }
        }


        #endregion << Example Of Calling Functions >>

        #region Param.SFO Struct 

        public struct Header
        {
            public static byte[] Magic = {0, 0x50, 0x53, 0x46};
            public static byte[] version = {01, 01, 0, 0};
            public static uint KeyTableStart = 0;
            public static uint DataTableStart = 0;
            public static uint IndexTableEntries = 0;

            private static byte[] Buffer
            {
                get
                {
                    var header = new byte[20];
                    Array.Copy(Magic, 0, header, 0, 4);
                    Array.Copy(version, 0, header, 4, 4);
                    Array.Copy(BitConverter.GetBytes(KeyTableStart), 0, header, 8, 4);
                    Array.Copy(BitConverter.GetBytes(DataTableStart), 0, header, 12, 4);
                    Array.Copy(BitConverter.GetBytes(IndexTableEntries), 0, header, 16, 4);
                    return header;
                }
            }

            public static void Read(BinaryReader input)
            {
                input.BaseStream.Seek(0, SeekOrigin.Begin);
                input.Read(Magic, 0, 4);
                input.Read(version, 0, 4);
                KeyTableStart = input.ReadUInt32();
                DataTableStart = input.ReadUInt32();
                IndexTableEntries = input.ReadUInt32();
            }
            
        }
        [Serializable]
        public struct Table : IComparable
        {
            public index_table Indextable;
            public string Name;
            public string Value;
            public int index;

            public byte[] NameBuffer
            {
                get
                {
                    var buffer = new byte[Name.Length + 1];
                    Array.Copy(Encoding.UTF8.GetBytes(Name), 0, buffer, 0, Encoding.UTF8.GetBytes(Name).Length);
                    return buffer;
                }
            }

            public byte[] ValueBuffer
            {
                get
                {
                    byte[] buffer;
                    switch (Indextable.param_data_fmt)
                    {
                        case FMT.ASCII:
                            buffer = new byte[Indextable.param_data_max_len];
                            Array.Copy(Encoding.ASCII.GetBytes(Value), 0, buffer, 0, Encoding.UTF8.GetBytes(Value).Length);
                            return buffer;
                        case FMT.UINT32:
                            return BitConverter.GetBytes(uint.Parse(Value));
                        case FMT.UTF_8:
                            buffer = new byte[Indextable.param_data_max_len];
                            Array.Copy(Encoding.UTF8.GetBytes(Value), 0, buffer, 0, Encoding.UTF8.GetBytes(Value).Length);
                            return buffer;
                        case FMT.Utf8Null:
                            buffer = new byte[Indextable.param_data_max_len];
                            Array.Copy(Encoding.UTF8.GetBytes(Value), 0, buffer, 0, Encoding.UTF8.GetBytes(Value).Length);/*write the length of the array*/
                            return buffer;
                        default:
                            return null;
                    }
                }
            }

            public int CompareTo(object obj)
            {
                throw new NotImplementedException();
            }
        }
        [Serializable]
        public struct index_table
        {
            public FMT param_data_fmt; /* param_data data type */
            public uint param_data_len; /* param_data used bytes */
            public uint param_data_max_len; /* param_data total reserved bytes */
            public uint param_data_offset; /* param_data offset (relative to start offset of data_table) */
            public ushort param_key_offset; /* param_key offset (relative to start offset of key_table) */
            

            private byte[] Buffer
            {
                get
                {
                    var data = new byte[16];
                    Array.Copy(BitConverter.GetBytes(param_key_offset), 0, data, 0, 2);
                    Array.Copy(BitConverter.GetBytes(((ushort) param_data_fmt)), 0, data, 2, 2);
                    Array.Copy(BitConverter.GetBytes(param_data_len), 0, data, 4, 4);
                    Array.Copy(BitConverter.GetBytes(param_data_max_len), 0, data, 8, 4);
                    Array.Copy(BitConverter.GetBytes(param_data_offset), 0, data, 12, 4);
                    return data;
                }
            }

            public void Read(BinaryReader input)
            {
                param_key_offset = input.ReadUInt16();
                param_data_fmt = (FMT) input.ReadUInt16();
                param_data_len = input.ReadUInt32();
                param_data_max_len = input.ReadUInt32();
                param_data_offset = input.ReadUInt32();
            }
        }

        [Serializable]
        private enum DATA_TYPE : byte
        {
            BinaryData = 0,
            Utf8Text = 2,
            Si32Integer = 4
        }

        #endregion Param.SFO Struct

        #region << Methods >>


        public PARAM_SFO()
        {

        }

        public PARAM_SFO(string filepath)
        {
            Init(new FileStream(filepath, FileMode.Open, FileAccess.Read, FileShare.Read));
        }

        public PARAM_SFO(byte[] inputdata)
        {
            Init(new MemoryStream(inputdata));
        }

        public PARAM_SFO(Stream input)
        {
            Init(input);
        }


        /// <summary>
        /// This is the SaveSFO Function for PS3/PS4/PSVita/And PSP no longer needed for Sony's CMD
        /// </summary>
        /// <param name="psfo">SFO That has been opened</param>
        /// <param name="filename">Save Location</param>
        public void SaveSFO(PARAM_SFO psfo, string filename)
        {
            //we start by opening the stream to the file
            using (var stream = File.Open(filename, FileMode.Create, FileAccess.Write, FileShare.Read))
            {
                if (!stream.CanSeek)
                    throw new ArgumentException("Stream must be seekable");//throw this error we cant seek the stream

                var utf8 = new UTF8Encoding(false);//encoding
                using (var writer = new BinaryWriter(stream, utf8, true))//start binary reader
                {

                    #region << Header Info (DevWiki) >>
                    /*
                     Header	
                     *  0x00	0x04	magic	PSF	
                        0x04	0x04	version	01 01 00 00	1.01
                        0x08	0x04	key_table_start	24 00 00 00	Absolute start offset of key_table = 0x24
                        0x0C	0x04	data_table_start	30 00 00 00	Absolute start offset of data_table = 0x30
                        0x10	0x04	tables_entries	01 00 00 00	Number of entries in index_table, key_table, and data_table = 1
                     */

                    #endregion <<Header Info >>

                    //so lets start writing the info
                    writer.Write(Header.Magic);//write magic "\0PSF" 
                    writer.Write(Header.version);//write version info this is mayjor and minor (01 01 00 00	1.01)
                    Header.KeyTableStart = 0x14 + Header.IndexTableEntries * 0x10;/*we can write all this lovely info from the tables back*/
                    writer.Write(Header.KeyTableStart);
                    
                    Header.DataTableStart = Convert.ToUInt32(Header.KeyTableStart + Tables.Sum(i => i.Name.Length + 1));//needs to be Uint
                    if (Header.DataTableStart % 4 != 0)
                        Header.DataTableStart = (Header.DataTableStart / 4 + 1) * 4;
                    writer.Write(Header.DataTableStart);
                    Header.IndexTableEntries = Convert.ToUInt32(Tables.Count);
                    writer.Write(Header.IndexTableEntries);

                    int lastKeyOffset = Convert.ToInt32(Header.KeyTableStart);
                    int lastValueOffset = Convert.ToInt32(Header.DataTableStart);
                    for (var i = 0; i < Tables.Count; i++)
                    {
                        var entry = Tables[i];

                        writer.BaseStream.Seek(0x14 + i * 0x10, SeekOrigin.Begin);
                        writer.Write((ushort)(lastKeyOffset - Header.KeyTableStart));


                        writer.Write((ushort)entry.Indextable.param_data_fmt);

                        writer.Write(entry.Indextable.param_data_len);
                        writer.Write(entry.Indextable.param_data_max_len);
                        writer.Write(lastValueOffset - Header.DataTableStart);

                        writer.BaseStream.Seek(lastKeyOffset, SeekOrigin.Begin);
                        writer.Write(utf8.GetBytes(entry.Name));
                        writer.Write((byte)0);
                        lastKeyOffset = (int)writer.BaseStream.Position;

                        writer.BaseStream.Seek(lastValueOffset, SeekOrigin.Begin);
                        writer.Write(entry.ValueBuffer);
                        lastValueOffset = (int)writer.BaseStream.Position;
                    }

                    //I'm doing this to just rewrite the first item (Some Cleanup will be needed)
                    //Or maybe not as when I checked this gives a 1 - 1 match with how the Sony tool works
                    //we need to rewrite that first item (PS4/PS3/PSV should be APP-VER)
                    lastKeyOffset = Convert.ToInt32(Header.KeyTableStart);
                    lastValueOffset = Convert.ToInt32(Header.DataTableStart);

                    var tableentry = Tables[0];
                   
                    writer.BaseStream.Seek(lastKeyOffset, SeekOrigin.Begin);
                    writer.Write(utf8.GetBytes(tableentry.Name));
                    writer.Write((byte)0);
                    lastKeyOffset = (int)writer.BaseStream.Position;

                }
            }

        }

        private string ReadValue(BinaryReader br, index_table table)
        {
            br.BaseStream.Position = ((Header.DataTableStart) + table.param_data_offset);
            switch (table.param_data_fmt)
            {
                case FMT.ASCII:
                    //return Encoding.GetEncoding(1252).GetString(br.ReadBytes((int) table.param_data_max_len)).Replace("\0", "");
                    return Encoding.UTF8.GetString(br.ReadBytes((int)table.param_data_max_len)).Replace("\0", "");
                case FMT.UINT32:
                    return br.ReadUInt32().ToString();
                case FMT.UTF_8:
                    return Encoding.UTF8.GetString(br.ReadBytes((int)table.param_data_max_len)).Replace("\0", "");
                case FMT.Utf8Null:
                    return Encoding.UTF8.GetString(br.ReadBytes((int)table.param_data_max_len)).Replace("\0", "");
                default:
                    return null;
            }
        }

        private string ReadValueSpecialChars(BinaryReader br, index_table table)
        {
            br.BaseStream.Position = ((Header.DataTableStart) + table.param_data_offset);
            switch (table.param_data_fmt)
            {
                case FMT.ASCII:
                    return Encoding.UTF8.GetString(br.ReadBytes((int)table.param_data_max_len)).Replace("\0", "");
                case FMT.UINT32:
                    return br.ReadUInt32().ToString();
                case FMT.UTF_8:
                    return Encoding.UTF8.GetString(br.ReadBytes((int)table.param_data_max_len)).Replace("\0", "");
                default:
                    return null;
            }
        }

        private string ReadName(BinaryReader br, index_table table)
        {
            br.BaseStream.Position = (Header.KeyTableStart + table.param_key_offset);
            string name = "";
            while (((byte)br.PeekChar()) != 0)
                name += br.ReadChar();
            br.BaseStream.Position++;
            return name;
        }

        /// <summary>
        /// Start Reading the Parameter file
        /// </summary>
        /// <param name="input">Input Stream</param>
        private void Init(Stream input)
        {
            using (var br = new BinaryReader(input))
            {
                Header.Read(br);
                if (!Functions.CompareBytes(Header.Magic, new byte[] { 0, 0x50, 0x53, 0x46 }))
                    throw new Exception("Invalid PARAM.SFO Header Magic");
                var tables = new List<index_table>();
                for (int i = 0; i < Header.IndexTableEntries; i++)
                {
                    var t = new index_table();
                    t.Read(br);
                    tables.Add(t);
                }
                var xtables = new List<Table>();
                int count = 0;
                foreach (index_table t in tables)
                {
                    var x = new Table();
                    x.index = count;
                    x.Indextable = t;
                    x.Name = ReadName(br, t);
                    x.Value = ReadValue(br, t);
                    count++;
                    xtables.Add(x);
                }
                Tables = xtables;
                br.Close();
            }
        }


        #endregion << Methods >>

    }
}