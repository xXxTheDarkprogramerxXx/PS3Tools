using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace PARAM.SFO_Editor
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
       
        #region << Error Code >>

        //----The Point of this is to simulate ps3 datacorrupt for whatever reason
        int errorcount = 0;
        bool errors = false;
        public void errormessage(int errorcount)
        { 
            string message = "The Paramaters of the System File Object has errors they have been marked\n\n\r\t Total Errors "+errorcount+"\n\n\r\t";
            MessageBox.Show(message, "Errors Found", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        #endregion << Error Code >>

        #region << Header >>
        //Magic
        public bool checkMagic (FileStream path)
        {
            //MAGIC
            int startHeader = 0x0;
            int endheader = 0x04;

            BinaryReader breader = new BinaryReader(path);
            breader.BaseStream.Position = startHeader;
            byte[] itemSection = breader.ReadBytes(endheader);
            string Magic = ByteArrayToAscii(itemSection, 0, itemSection.Length, false);
            if (Magic != "PSF")
            {
                return false;
            }
            else return true;
            
        }
        //Version
        public string getVersion (FileStream path)
        {
            //Version
            int startHeader = 0x04;
            int endheader = 0x08;

            BinaryReader breader = new BinaryReader(path);
            breader.BaseStream.Position = startHeader;
            byte[] itemSection = breader.ReadBytes(endheader);
            string Version = BitConverter.ToString(itemSection,0,2);
            Version = Version.Replace("-", ".");
            return Version;
        }
        //Key_table_start
        public string getKeyTableStart(FileStream path)
        {
            int startHeader = 0x08;
            int endheader = 0x0C;

            BinaryReader breader = new BinaryReader(path);
            breader.BaseStream.Position = startHeader;
            byte[] itemSection = breader.ReadBytes(endheader);
            string key_table_start = BitConverter.ToString(itemSection,0,1);
            return key_table_start;

        }
        //data_table_start
        public string getDataTableStart(FileStream path)
        {
            int startHeader = 0x0C;
            int endheader = 0x10;

            BinaryReader breader = new BinaryReader(path);
            breader.BaseStream.Position = startHeader;
            byte[] itemSection = breader.ReadBytes(endheader);
            string date_table_start = BitConverter.ToString(itemSection, 0, 1);
            return date_table_start;
        }
        //table_entries
        public string getTableEntries(FileStream path)
        {
            int startHeader = 0x10;
            int endheader = 0x14;

            BinaryReader breader = new BinaryReader(path);
            breader.BaseStream.Position = startHeader;
            byte[] itemSection = breader.ReadBytes(endheader);
            string table_entries = BitConverter.ToString(itemSection, 0, 1);
            return table_entries;
        }
        #endregion << Header >>

        #region << index_table >>
        public string getkey_1_offset(FileStream path)
        {
            int startHeader = 0x14;
            int endheader = 0x16;

            BinaryReader breader = new BinaryReader(path);
            breader.BaseStream.Position = startHeader;
            byte[] itemSection = breader.ReadBytes(endheader);
            string key_1_offset = BitConverter.ToString(itemSection, 0, 2);
            return key_1_offset;

        }

        private string getdata_1_fmt(FileStream path)
        {
            int startHeader = 0x16;
            int endheader = 0x18;

            BinaryReader breader = new BinaryReader(path);
            breader.BaseStream.Position = startHeader;
            byte[] itemSection = breader.ReadBytes(endheader);
            string data_1_fmt = BitConverter.ToString(itemSection, 0, 2);
            if (data_1_fmt == "04-02")
            {
                data_1_fmt = "UTF8";
            }
            return data_1_fmt;
        }

        private string getdata_1_len(FileStream path)
        {
            int startHeader = 0x18;
            int endheader = 0x1C;

            BinaryReader breader = new BinaryReader(path);
            breader.BaseStream.Position = startHeader;
            byte[] itemSection = breader.ReadBytes(endheader);
            string getdata_1_len = BitConverter.ToString(itemSection, 0, 1);
            return getdata_1_len;
        }

        private string getdata_1_max_len(FileStream path)
        {
            int startHeader = 0x1C;
            int endheader = 0x20;

            BinaryReader breader = new BinaryReader(path);
            breader.BaseStream.Position = startHeader;
            byte[] itemSection = breader.ReadBytes(endheader);
            string data_1_max_len = BitConverter.ToString(itemSection, 0, 1);
            return data_1_max_len;
        }

        private string getdata_1_offset(FileStream path)
        {
            int startHeader = 0x20;
            int endheader = 0x24;

            BinaryReader breader = new BinaryReader(path);
            breader.BaseStream.Position = startHeader;
            byte[] itemSection = breader.ReadBytes(endheader);
            string data_1_max_len = BitConverter.ToString(itemSection, 0, 1);
            if(data_1_max_len != "00")
            {
                errors = true;
                errorcount++;
                MessageBox.Show("Data offset missmatch", "DataCorrupt", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            return data_1_max_len;
        }

        #endregion << index_table >>

        #region << key_table >>

        public string getkey_1(FileStream path)
        {
            int startHeader = 0x24;
            int endheader = 0x2D;

            BinaryReader breader = new BinaryReader(path);
            breader.BaseStream.Position = startHeader;
            byte[] itemSection = breader.ReadBytes(endheader);
            string key_1 = BitConverter.ToString(itemSection, 0, 9);
            return key_1;
        }

        #endregion << key_table >>

        public static string ByteArrayToHexString(byte[] ByteArray)
        {
            string HexString = "";
            for (int i = 0; i < ByteArray.Length; ++i)
                HexString += ByteArray[i].ToString("X2"); // +" ";
            return HexString;
        }

        public static string HexStringToAscii(string HexString, bool cleanEndOfString)
        {
            try
            {
                string StrValue = "";
                // While there's still something to convert in the hex string
                while (HexString.Length > 0)
                {
                    // Use ToChar() to convert each ASCII value (two hex digits) to the actual character
                    StrValue += System.Convert.ToChar(System.Convert.ToUInt32(HexString.Substring(0, 2), 16)).ToString();

                    // Remove from the hex object the converted value
                    HexString = HexString.Substring(2, HexString.Length - 2);
                }
                // Clean String
                if (cleanEndOfString)
                    StrValue = StrValue.Replace("\0", "");

                return StrValue;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public static string ByteArrayToAscii(byte[] ByteArray, int startPos, int length, bool cleanEndOfString)
        {
            byte[] byteArrayPhrase = new byte[length];
            Array.Copy(ByteArray, startPos, byteArrayPhrase, 0, byteArrayPhrase.Length);
            string hexPhrase = ByteArrayToHexString(byteArrayPhrase);
            return HexStringToAscii(hexPhrase, true);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog thedialog = new OpenFileDialog();
            thedialog.Title = "PARAM.SFO";
            thedialog.Filter = ".SFO|PARAM.SFO";
            thedialog.InitialDirectory = System.Environment.SpecialFolder.MyComputer.ToString();
            if(thedialog.ShowDialog() == DialogResult.OK)
            {
                using (FileStream str = File.OpenRead(thedialog.FileName.ToString()))
                {
                    //Check MAGIC
                    if(checkMagic(str) != false)
                    { 
                         //Version 
                        cbVersion.Items.Add(getVersion(str).ToString());
                        cbVersion.SelectedIndex = 0;
                        //header
                        lblKTS.Text = getKeyTableStart(str);
                        lblDTS.Text = getDataTableStart(str);
                        lblTE.Text = getTableEntries(str);
                        //Index_Table
                        lblkey_1_offset.Text = getkey_1_offset(str);
                        lbldata_1_fmt.Text = getdata_1_fmt(str);
                        lbldata_1_len.Text = getdata_1_len(str);
                        lbldata_1_max_len.Text = getdata_1_max_len(str);
                        lbldata_1_offset.Text = getdata_1_offset(str);
                        //key_table
                        lblkey_1.Text = getkey_1(str);

                    }
                    else
                    {
                        MessageBox.Show("The file selected isn't a valid SFO","File Error",MessageBoxButtons.OK,MessageBoxIcon.Error);
                    }
               
                }
            }
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if(checkBox1.Checked == true)
            {
                gbAdvanced.Visible = true;
            }
            else
            {
                gbAdvanced.Visible = false;
            }
        }

       
    }
}
