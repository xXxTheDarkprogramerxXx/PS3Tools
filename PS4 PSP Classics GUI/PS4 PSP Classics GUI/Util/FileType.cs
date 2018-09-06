using DiscUtils.Iso9660;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PS4_PSP_Classics_GUI
{
    class FileType
    {
        public enum FileTypes
        {
            PBP,
            ISO,
            CSO,
            Unknown
        }

        public static FileTypes LoadFileInfo(string path)
        {
            Stream FileStream = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read);

            //else we check the rest
            FileStream.Seek( 0L, SeekOrigin.Begin);
            byte[] array = new byte[4];
            FileStream.Read(array, 0, array.Length);
            if (BitConverter.ToUInt32(array, 0) == 1346523136u)//PBP
            {
                //this is a PBP File
                return FileTypes.PBP;
            }
            

            if(BitConverter.ToUInt32(array,0) == 1330858307)//CSO
            {
                return FileTypes.CSO;//Compressed ISO
            }

           
            try
            {
                CDReader cd = new CDReader(FileStream, true);
                if (cd.FriendlyName != "")
                {
                    return FileTypes.ISO;
                }
            }
            catch (Exception ex)
            {
                //we dont want to actaully use this error just means its not an ISO might be a cso since its the only one of these that are breaking
            }
            finally
            {
                //out of my experiences always use a finally 
                //ESPECIALLY IN THREADS
            }

           

            return FileTypes.Unknown;
        }
    }
}
