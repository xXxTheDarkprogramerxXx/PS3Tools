using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Runtime.InteropServices;
using System.Reflection;

namespace Helpers
{
    public enum StringType { PlainText, NullTerminated, PascalString }

    public class StructReader : BinaryReader
    {
        private readonly ByteOrder _byteOrder;

        public StructReader(ByteOrder byteorder, Stream input)
            : base(input)
        {
            _byteOrder = byteorder;
        }

        public StructReader(ByteOrder byteorder, Stream input, Encoding encoding)
            : base(input, encoding)
        {
            _byteOrder = byteorder;
        }

        public T ReadStruct<T>() where T : struct
        {
            T result;
            Type structtype = typeof(T);

            byte[] buffer = this.ReadBytes(Marshal.SizeOf(structtype));
            GCHandle handle = GCHandle.Alloc(buffer, GCHandleType.Pinned);
            try
            {
                result = (T)Marshal.PtrToStructure(handle.AddrOfPinnedObject(), structtype);

                result = BinaryByteOrder.ChangeStructOrder<T>(result, _byteOrder);
            }
            finally
            {
                handle.Free();
            }

            return result;
        }

        public T[] ReadStructs<T>(int count) where T : struct
        {
            T[] result = new T[count];
            for (int idx = 0; idx < result.Length; idx++)
            {
                result[idx] = ReadStruct<T>();
            }

            return result;
        }

        #region String Handling Routines

        public string[] ReadStrings(StringType stringtype, int count)
        {
            string[] result = new string[count];
            
            for (int idx = 0; idx < result.Length; idx++)
            {
                result[idx] = ReadString(stringtype);
            }

            return result;
        }

        public new string ReadString()
        {
            return ReadString(StringType.PascalString);
        }

        public new string ReadString(StringType stringtype)
        {
            return ReadString(this, stringtype, -1);
        }

        public string ReadString(StringType stringtype, int stringlength)
        {
            return ReadString(this, stringtype, stringlength);
        }

        public static string ReadString(BinaryReader br, StringType stringtype)
        {
            return ReadString(br, stringtype, -1);
        }

        public static string ReadString(BinaryReader br, StringType stringtype, int stringlength)
        {
            if (stringtype == StringType.PascalString)
            {
                stringlength = br.ReadByte();
            }

            if ((stringtype == StringType.PlainText) && (stringlength < 0))
            {
                throw new Exception(string.Format("Invalid string length '{0}'.", stringlength));
            }


            List<byte> bytes = new List<byte>();
            while (true)
            {
                if (stringtype == StringType.PascalString || stringtype == StringType.PlainText)
                {
                    if (stringlength >= 0 && bytes.Count == stringlength)
                        break;
                }

                byte ch = br.ReadByte();
                if ((stringtype == StringType.NullTerminated) && (ch == 0x00))
                    break;

                bytes.Add(ch);
            }

            return Encoding.UTF8.GetString(bytes.ToArray());
        }

        #endregion String Handling Routines
    }
}
