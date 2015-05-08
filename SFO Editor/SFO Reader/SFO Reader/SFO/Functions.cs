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
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Threading;
using System.Windows.Forms;

namespace PeXploit
{
    internal static class Functions
    {
        public static string[] StaticKeys
        {
            get
            {
                return new[]
                {
                    "syscon_manager_key=D413B89663E1FE9F75143D3BB4565274",
                    "keygen_key=6B1ACEA246B745FD8F93763B920594CD53483B82",
                    "savegame_param_sfo_key=0C08000E090504040D010F000406020209060D03",
                    "trophy_param_sfo_key=5D5B647917024E9BB8D330486B996E795D7F4392",
                    "tropsys_dat_key=B080C40FF358643689281736A6BF15892CFEA436",
                    "tropusr_dat_key=8711EFF406913F0937F115FAB23DE1A9897A789A",
                    "troptrns_dat_key=91EE81555ACC1C4FB5AAE5462CFE1C62A4AF36A5",
                    "tropconf_sfm_key=E2ED33C71C444EEBC1E23D635AD8E82F4ECA4E94",
                    "fallback_disc_hash_key=D1C1E10B9C547E689B805DCD9710CE8D"
                };
            }
        }


        public static UInt16 SwapByteOrder(this UInt16 value)
        {
            return (UInt16) ((value & 0xFFU) << 8 | (value & 0xFF00U) >> 8);
        }

        public static UInt32 SwapByteOrder(this UInt32 value)
        {
            return (value & 0x000000FFU) << 24 | (value & 0x0000FF00U) << 8 |
                   (value & 0x00FF0000U) >> 8 | (value & 0xFF000000U) >> 24;
        }

        public static UInt64 SwapByteOrder(this UInt64 value)
        {
            return
                ((value & 0xff00000000000000L) >> 56) |
                ((value & 0x00ff000000000000L) >> 40) |
                ((value & 0x0000ff0000000000L) >> 24) |
                ((value & 0x000000ff00000000L) >> 8) |
                ((value & 0x00000000ff000000L) << 8) |
                ((value & 0x0000000000ff0000L) << 24) |
                ((value & 0x000000000000ff00L) << 40) |
                ((value & 0x00000000000000ffL) << 56);
        }

        public static bool CompareBytes(byte[] a, byte[] b)
        {
            if (a.Length != b.Length)
                return false;
            for (int i = 0; i < a.Length; i++)
            {
                if (a[i] != b[i])
                    return false;
            }
            return true;
        }

        public static byte[] DecryptWithPortability(byte[] iv, byte[] data, int data_size)
        {
            var x = new AesCryptoServiceProvider();
            x.Mode = CipherMode.CBC;
            x.Padding = PaddingMode.Zeros;
            byte[] key = GetStaticKey("syscon_manager_key");
            if (iv.Length != 16)
                Array.Resize(ref iv, 16);
            return x.CreateDecryptor(key, iv).TransformFinalBlock(data, 0, data_size);
        }

        public static byte[] EncryptWithPortability(byte[] iv, byte[] data, int data_size)
        {
            var x = new AesCryptoServiceProvider();
            x.Mode = CipherMode.CBC;
            x.Padding = PaddingMode.Zeros;
            byte[] key = GetStaticKey("syscon_manager_key");
            if (iv.Length != 16)
                Array.Resize(ref iv, 16);
            return x.CreateEncryptor(key, iv).TransformFinalBlock(data, 0, data_size);
        }

        public static byte[] StringToByteArray(this string hex)
        {
            if ((hex.Length%2) != 0) hex = hex.PadLeft(hex.Length + 1, '0');
            return Enumerable.Range(0, hex.Length)
                .Where(x => x%2 == 0)
                .Select(x => Convert.ToByte(hex.Substring(x, 2), 16))
                .ToArray();
        }

        public static byte[] GetStaticKey(string name)
        {
            foreach (string line in StaticKeys)
            {
                string x = line.Split('=')[0];
                if (x.ToLower() == name.ToLower())
                {
                    string value = line.Split('=')[1];
                    return StringToByteArray(value);
                }
            }
            return null;
        }

        private static SecureFileInfo[] xDownloadAldosGameConfig()
        {
            try
            {
                string text = new WebClient().DownloadString("http://ps3tools.aldostools.org/games.conf");
                if (text == null || text.Length < 100)
                    return new SecureFileInfo[] {};
                return ReadConfigFromtext(text);
            }
            catch
            {
                return new SecureFileInfo[] {};
            }
        }

        public static SecureFileInfo[] ReadConfigFromtext(string inputtext)
        {
            var files = new List<SecureFileInfo>();
            using (var sr = new StringReader(inputtext))
            {
                string line;
                string s = line = sr.ReadLine();
                while (s != null && (sr.Peek() > -1 && !s.Equals("; -- UNPROTECTED GAMES --")))
                    Application.DoEvents();

                string s1 = line = sr.ReadLine();
                while (s1 != null && (sr.Peek() > -1 && s1.StartsWith(";")))
                    files.Add(new SecureFileInfo(line.Replace(";", ""), "", "", "", false));

                while (sr.Peek() > -1)
                {
                    string s2 = line = sr.ReadLine();
                    while (s2 != null && (sr.Peek() > -1 && s2.StartsWith(";")))
                    {
                        if (line != null)
                        {
                            string name = line.Replace(";", "");
                            string s3 = line = sr.ReadLine();
                            if (s3 != null && s3.StartsWith("["))
                            {
                                string id = line;
                                string readLine = sr.ReadLine();
                                if (readLine != null)
                                {
                                    string diskhashkey = readLine.Split('=')[1];
                                    string secureid = readLine.Split('=')[1];
                                    files.Add(new SecureFileInfo(name, id, secureid, diskhashkey,
                                        (!string.IsNullOrEmpty(secureid) && secureid.Length == 32)));
                                }
                            }
                        }
                    }
                }
                sr.Close();
            }
            return files.ToArray();
        }

        public static SecureFileInfo[] DownloadAldosGameConfig()
        {
            SecureFileInfo[] x = {};
            var t = new Thread(() => x = xDownloadAldosGameConfig());
            t.Start();
            while (t.ThreadState != ThreadState.Stopped)
                Application.DoEvents();
            return x;
        }

        public static byte[] GetHMACSHA1(byte[] key, byte[] data, int start, int length)
        {
            return new HMACSHA1(key).ComputeHash(data, start, length);
        }

        public static byte[] CalculateFileHMACSha1(string file, byte[] key)
        {
            byte[] hash;
            using (var fs = new FileStream(file, FileMode.Open))
            {
                hash = new HMACSHA1(key).ComputeHash(fs);
                fs.Close();
            }
            return hash;
        }

        public static byte[] CalculateFileHMACSha1(Stream input, byte[] key)
        {
            return new HMACSHA1(key).ComputeHash(input);
        }

        public static byte[] Decrypt(byte[] key, byte[] input, int length)
        {
            Array.Resize(ref key, 16);
            Aes x1 = Aes.Create();
            x1.Key = key;
            x1.BlockSize = 128;
            x1.Mode = CipherMode.ECB;
            x1.Padding = PaddingMode.Zeros;
            Aes x2 = Aes.Create();
            x2.Key = key;
            x1.BlockSize = 128;
            x2.Mode = CipherMode.ECB;
            x2.Padding = PaddingMode.Zeros;
            int nums = (length/16);
            var output = new byte[length];
            for (int i = 0; i < nums; i++)
            {
                var blockdata = new byte[16];
                Array.Copy(input, (i*16), blockdata, 0, 16);
                int offset = (i*16);
                var buffer = new byte[16];
                Array.Copy(BitConverter.GetBytes(SwapByteOrder((ulong) i)), 0, buffer, 0, 8);
                buffer = x1.CreateEncryptor().TransformFinalBlock(buffer, 0, buffer.Length);
                blockdata = x2.CreateDecryptor().TransformFinalBlock(blockdata, 0, blockdata.Length);
                for (int j = 0; j < 16; j++)
                    blockdata[j] ^= buffer[j];
                Array.Copy(blockdata, 0, output, (i*16), 16);
            }
            return output;
        }

        public static byte[] Encypt(byte[] key, byte[] input, int length)
        {
            Array.Resize(ref key, 16);
            Aes x1 = Aes.Create();
            x1.Key = key;
            x1.BlockSize = 128;
            x1.Mode = CipherMode.ECB;
            x1.Padding = PaddingMode.Zeros;
            Aes x2 = Aes.Create();
            x2.Key = key;
            x1.BlockSize = 128;
            x2.Mode = CipherMode.ECB;
            x2.Padding = PaddingMode.Zeros;
            int nums = (length/16);
            var output = new byte[length];
            for (int i = 0; i < nums; i++)
            {
                var blockdata = new byte[16];
                Array.Copy(input, (i*16), blockdata, 0, 16);
                int offset = (i*16);
                var buffer = new byte[16];
                Array.Copy(BitConverter.GetBytes(SwapByteOrder((ulong) i)), 0, buffer, 0, 8);
                buffer = x1.CreateEncryptor().TransformFinalBlock(buffer, 0, buffer.Length);
                for (int j = 0; j < 16; j++)
                    blockdata[j] ^= buffer[j];
                blockdata = x2.CreateEncryptor().TransformFinalBlock(blockdata, 0, blockdata.Length);
                Array.Copy(blockdata, 0, output, (i*16), 16);
            }
            return output;
        }
    }
}