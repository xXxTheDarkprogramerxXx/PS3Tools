using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Runtime.InteropServices;

namespace Helpers
{
    public class StructWriter : BinaryWriter
    {
        private readonly ByteOrder _byteOrder;

        public StructWriter(ByteOrder byteorder, Stream output)
            : base(output)
        {
            _byteOrder = byteorder;
        }

        public StructWriter(ByteOrder byteorder, Stream output, Encoding encoding)
            : base(output, encoding)
        {
            _byteOrder = byteorder;
        }

        public void WriteStruct<T>(T srcstruct) where T : struct
        {
            // Make sure the struct is in the correct byte-order
            srcstruct = BinaryByteOrder.ChangeStructOrder<T>(srcstruct, _byteOrder);

            byte[] buffer = new byte[Marshal.SizeOf(srcstruct)];
            GCHandle handle = GCHandle.Alloc(buffer, GCHandleType.Pinned);
            try
            {
                Marshal.StructureToPtr(srcstruct, handle.AddrOfPinnedObject(), false);
                base.Write(buffer);
            }
            finally
            {
                handle.Free();
            }
        }

        public void WriteStructs<T>(IEnumerable<T> srcstructs) where T : struct
        {
            foreach (T srcstruct in srcstructs)
            {
                WriteStruct<T>(srcstruct);
            }
        }

        public void WriteString(StringType stringtype, string srcstring)
        {
            byte[] buffer = Encoding.UTF8.GetBytes(srcstring);

            if (stringtype == StringType.PascalString)
            {
                base.Write((byte)buffer.Length);
            }

            base.Write(buffer);

            if (stringtype == StringType.NullTerminated)
            {
                base.Write((byte)0);
            }
        }

        public void WriteStrings(StringType stringtype, IEnumerable<string> srcstrings)
        {
            foreach (string str in srcstrings)
            {
                WriteString(stringtype, str);
            }
        }
    }
}
