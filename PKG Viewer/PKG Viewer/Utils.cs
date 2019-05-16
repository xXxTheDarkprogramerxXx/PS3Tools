using System;
using System.Linq;
using System.Text;
using System.Drawing;
using System.IO;
using System.Runtime.CompilerServices;
using Microsoft.VisualBasic;
using Microsoft.VisualBasic.CompilerServices;


namespace PKG_Viewer
{
    [StandardModule]
    internal sealed class Utils
    {
        public class Names
        {
            public int Index;

            public ulong Offset;

            public ulong Size;

            public string Name;

            public Names(int m_Index, ulong m_Offset, ulong m_Size, string m_Name)
            {
                this.Index = m_Index;
                this.Offset = m_Offset;
                this.Size = m_Size;
                this.Name = m_Name;
            }
        }

        public static uint ReadUInt32(object stream)
        {
            byte[] array = new byte[4];
            Type arg_4A_1 = null;
            string arg_4A_2 = "Read";
            object[] array2 = new object[]
            {
                array,
                0,
                4
            };
            object[] arg_4A_3 = array2;
            string[] arg_4A_4 = null;
            Type[] arg_4A_5 = null;
            bool[] array3 = new bool[]
            {
                true,
                false,
                false
            };
            NewLateBinding.LateCall(stream, arg_4A_1, arg_4A_2, arg_4A_3, arg_4A_4, arg_4A_5, array3, true);
            if (array3[0])
            {
                array = (byte[])Conversions.ChangeType(RuntimeHelpers.GetObjectValue(array2[0]), typeof(byte[]));
            }
            Array.Reverse(array, 0, 4);
            return BitConverter.ToUInt32(array, 0);
        }

        public static ushort ReadUInt16(object stream)
        {
            byte[] array = new byte[4];
            Type arg_4A_1 = null;
            string arg_4A_2 = "Read";
            object[] array2 = new object[]
            {
                array,
                0,
                2
            };
            object[] arg_4A_3 = array2;
            string[] arg_4A_4 = null;
            Type[] arg_4A_5 = null;
            bool[] array3 = new bool[]
            {
                true,
                false,
                false
            };
            NewLateBinding.LateCall(stream, arg_4A_1, arg_4A_2, arg_4A_3, arg_4A_4, arg_4A_5, array3, true);
            if (array3[0])
            {
                array = (byte[])Conversions.ChangeType(RuntimeHelpers.GetObjectValue(array2[0]), typeof(byte[]));
            }
            Array.Reverse(array, 0, 2);
            return BitConverter.ToUInt16(array, 0);
        }

        public static string ReadASCIIString(object stream, int legth)
        {
            byte[] array = new byte[checked(legth - 1 + 1)];
            Type arg_50_1 = null;
            string arg_50_2 = "Read";
            object[] array2 = new object[]
            {
                array,
                0,
                array.Length
            };
            object[] arg_50_3 = array2;
            string[] arg_50_4 = null;
            Type[] arg_50_5 = null;
            bool[] array3 = new bool[]
            {
                true,
                false,
                false
            };
            NewLateBinding.LateCall(stream, arg_50_1, arg_50_2, arg_50_3, arg_50_4, arg_50_5, array3, true);
            if (array3[0])
            {
                array = (byte[])Conversions.ChangeType(RuntimeHelpers.GetObjectValue(array2[0]), typeof(byte[]));
            }
            return Encoding.ASCII.GetString(array);
        }

        public static byte[] ReadByte(object stream, int legth)
        {
            byte[] array = new byte[checked(legth - 1 + 1)];
            Type arg_50_1 = null;
            string arg_50_2 = "Read";
            object[] array2 = new object[]
            {
                array,
                0,
                array.Length
            };
            object[] arg_50_3 = array2;
            string[] arg_50_4 = null;
            Type[] arg_50_5 = null;
            bool[] array3 = new bool[]
            {
                true,
                false,
                false
            };
            NewLateBinding.LateCall(stream, arg_50_1, arg_50_2, arg_50_3, arg_50_4, arg_50_5, array3, true);
            if (array3[0])
            {
                array = (byte[])Conversions.ChangeType(RuntimeHelpers.GetObjectValue(array2[0]), typeof(byte[]));
            }
            return array;
        }

        public static Image BytesToImage(byte[] ImgBytes)
        {
            Image result = null;
            if (ImgBytes != null)
            {
                MemoryStream stream = new MemoryStream(ImgBytes);
                result = Image.FromStream(stream);
            }
            return result;
        }

        public static bool isLinux()
        {
            return Operators.CompareString(Conversions.ToString(Path.DirectorySeparatorChar), "/", false) == 0 && !MyProject.Computer.Info.OSFullName.Contains("Microsoft Windows");
        }

        public static bool Contain(byte[] a, byte[] b)
        {
            checked
            {
                if (a != null)
                {
                    if (b != null)
                    {
                        if (a.Length > 0 && b.Length > 0)
                        {
                            if (a.Length != b.Length)
                            {
                                return false;
                            }
                            int arg_27_0 = 0;
                            int num = a.Length - 1;
                            for (int i = arg_27_0; i <= num; i++)
                            {
                                if (a[i] != b[i])
                                {
                                    return false;
                                }
                            }
                            return true;
                        }
                    }
                }
                return false;
            }
        }

        public static string HexToString(string hex)
        {
            StringBuilder stringBuilder = new StringBuilder(hex.Length / 2);
            int arg_18_0 = 0;
            checked
            {
                int num = hex.Length - 2;
                for (int i = arg_18_0; i <= num; i += 2)
                {
                    stringBuilder.Append(Strings.Chr((int)Convert.ToByte(hex.Substring(i, 2), 16)));
                }
                return stringBuilder.ToString();
            }
        }
    }
}
