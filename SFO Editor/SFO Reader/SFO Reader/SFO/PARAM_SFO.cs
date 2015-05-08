/* Copyright (c) 2013 - 2014 Jappi88 (Jappi88 at Gmail dot com)
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

namespace PeXploit
{
    public class PARAM_SFO
    {
        public enum DataTypes : uint
        {
            PSNGame =18248,
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
            None
        }

        public enum FMT : ushort
        {
            UTF_8 = 0x0400,
            ASCII = 0x0402,
            UINT32 = 0x0404
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

        public Table[] Tables { get; private set; }

        public DataTypes DataType
        {
            get
            {
                if (Tables == null)
                    return DataTypes.None;
                foreach (Table t in Tables)
                    if (t.Name == "CATEGORY")
                        return ((DataTypes) BitConverter.ToUInt16(Encoding.UTF8.GetBytes(t.Value), 0));
                return DataTypes.None;
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

        private string ReadValue(BinaryReader br, index_table table)
        {
            br.BaseStream.Position = ((Header.DataTableStart) + table.param_data_offset);
            switch (table.param_data_fmt)
            {
                case FMT.ASCII:
                    return Encoding.ASCII.GetString(br.ReadBytes((int) table.param_data_max_len)).Replace("\0", "");
                case FMT.UINT32:
                    return br.ReadUInt32().ToString();
                case FMT.UTF_8:
                    return Encoding.UTF8.GetString(br.ReadBytes((int) table.param_data_max_len)).Replace("\0", "");
                default:
                    return null;
            }
        }

        private string ReadName(BinaryReader br, index_table table)
        {
            br.BaseStream.Position = (Header.KeyTableStart + table.param_key_offset);
            string name = "";
            while (((byte) br.PeekChar()) != 0)
                name += br.ReadChar();
            br.BaseStream.Position++;
            return name;
        }


        private void Init(Stream input)
        {
            using (var br = new BinaryReader(input))
            {
                Header.Read(br);
                if (!Functions.CompareBytes(Header.Magic, new byte[] {0, 0x50, 0x53, 0x46}))
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
                Tables = xtables.ToArray();
                br.Close();
            }
        }

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

        public struct Table
        {
            public index_table Indextable;
            public string Name;
            public string Value;
            public int index;

            private byte[] NameBuffer
            {
                get
                {
                    var buffer = new byte[Name.Length + 1];
                    Array.Copy(Encoding.UTF8.GetBytes(Name), 0, buffer, 0, Name.Length);
                    return buffer;
                }
            }

            private byte[] ValueBuffer
            {
                get
                {
                    byte[] buffer;
                    switch (Indextable.param_data_fmt)
                    {
                        case FMT.ASCII:
                            buffer = new byte[Indextable.param_data_max_len];
                            Array.Copy(Encoding.ASCII.GetBytes(Value), 0, buffer, 0, Value.Length);
                            return buffer;
                        case FMT.UINT32:
                            return BitConverter.GetBytes(uint.Parse(Value));
                        case FMT.UTF_8:
                            buffer = new byte[Indextable.param_data_max_len];
                            Array.Copy(Encoding.UTF8.GetBytes(Value), 0, buffer, 0, Value.Length);
                            return buffer;
                        default:
                            return null;
                    }
                }
            }
        }

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
                    Array.Copy(BitConverter.GetBytes(((ushort) param_data_fmt).SwapByteOrder()), 0, data, 2, 2);
                    Array.Copy(BitConverter.GetBytes(param_data_len), 0, data, 4, 4);
                    Array.Copy(BitConverter.GetBytes(param_data_max_len), 0, data, 8, 4);
                    Array.Copy(BitConverter.GetBytes(param_data_offset), 0, data, 12, 4);
                    return data;
                }
            }

            public void Read(BinaryReader input)
            {
                param_key_offset = input.ReadUInt16();
                param_data_fmt = (FMT) input.ReadUInt16().SwapByteOrder();
                param_data_len = input.ReadUInt32();
                param_data_max_len = input.ReadUInt32();
                param_data_offset = input.ReadUInt32();
            }
        }
    }
}