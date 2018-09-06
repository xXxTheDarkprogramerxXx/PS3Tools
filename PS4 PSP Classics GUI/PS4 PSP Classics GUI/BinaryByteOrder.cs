using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace Helpers
{
    public enum ByteOrder { LSB, MSB };

    public static class BinaryByteOrder
    {
        public static UInt64 SwapByteOrder(UInt64 src, int bytelength)
        {
            UInt64[] bitmasks = {
                                  0x00000000000000FF,
                                  0x000000000000FF00,
                                  0x0000000000FF0000,
                                  0x00000000FF000000,
                                  0x000000FF00000000,
                                  0x0000FF0000000000,
                                  0x00FF000000000000,
                                  0xFF00000000000000
                                };

            UInt64 result = 0;
            int bitshift = (bytelength * 8) - 8;

            for (int byteidx = 0; byteidx < bytelength; byteidx++)
            {
                UInt64 curvalue = src & bitmasks[byteidx];

                if (bitshift < 0)
                {
                    result += curvalue >> (-bitshift);
                }
                else
                {
                    result += curvalue << bitshift;
                }

                bitshift -= 16;
            }

            return result;
        }

        public static T ChangeStructOrder<T>(T srcstruct, ByteOrder byteorder) where T : struct
        {
            if (byteorder == ByteOrder.LSB)
                return srcstruct;


            Type structtype = typeof(T);
            TypedReference structref = __makeref(srcstruct);

            FieldInfo[] fields = structtype.GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
            foreach (FieldInfo fi in fields)
            {
                //if (fi.FieldType == typeof(Byte))
                //{
                //    Byte value = Convert.ToByte(fi.GetValue(srcstruct));
                //    //value = value;

                //    // fi.SetValue(result, Convert.ToInt16(value));
                //    fi.SetValueDirect(structref, Convert.ToInt16(value));
                //    continue;
                //}
                if (fi.FieldType == typeof(Int16))
                {
                    UInt64 value = Convert.ToUInt64(fi.GetValue(srcstruct));
                    value = SwapByteOrder(value, 2);

                    // fi.SetValue(result, Convert.ToInt16(value));
                    fi.SetValueDirect(structref, Convert.ToInt16(value));
                    continue;
                }
                if (fi.FieldType == typeof(UInt16))
                {
                    UInt64 value = Convert.ToUInt64(fi.GetValue(srcstruct));
                    value = SwapByteOrder(value, 2);

                    // fi.SetValue(result, Convert.ToUInt16(value));
                    fi.SetValueDirect(structref, Convert.ToUInt16(value));
                    continue;
                }
                if (fi.FieldType == typeof(Int32))
                {
                    UInt64 value = Convert.ToUInt64(fi.GetValue(srcstruct));
                    value = SwapByteOrder(value, 4);

                    // fi.SetValue(result, Convert.ToInt32(value));
                    fi.SetValueDirect(structref, Convert.ToInt32(value));
                    continue;
                }
                if (fi.FieldType == typeof(UInt32))
                {
                    UInt64 value = Convert.ToUInt64(fi.GetValue(srcstruct));
                    value = SwapByteOrder(value, 4);

                    // fi.SetValue(result, Convert.ToUInt32(value));
                    fi.SetValueDirect(structref, Convert.ToUInt32(value));
                    continue;
                }
                if (fi.FieldType == typeof(Int64))
                {
                    UInt64 value = Convert.ToUInt64(fi.GetValue(srcstruct));
                    value = SwapByteOrder(value, 8);

                    // fi.SetValue(result, Convert.ToInt64(value));
                    fi.SetValueDirect(structref, Convert.ToInt64(value));
                    continue;
                }
                if (fi.FieldType == typeof(UInt64))
                {
                    UInt64 value = Convert.ToUInt64(fi.GetValue(srcstruct));
                    value = SwapByteOrder(value, 8);

                    // fi.SetValue(result, value);
                    fi.SetValueDirect(structref, value);
                    continue;
                }
            }

            return srcstruct;
        }
    }
}
