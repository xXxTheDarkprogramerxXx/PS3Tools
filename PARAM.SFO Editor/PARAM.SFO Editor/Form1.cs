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

        //----The Point of this is to simulate ps3 data corrupt for whatever reason
        int errorcount = 0;
        bool errors = false;
        public void errormessage(int errorcount)
        { 
            string message = "The Parameters of the System File Object has errors they have been marked\n\n\r\t Total Errors "+errorcount+"\n\n\r\t";
            MessageBox.Show(message, "Errors Found", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        #endregion << Error Code >>

        PeXploit.PARAM_SFO psfo;

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

        #region <<< Title ID >>>

        public string getTitleID(FileStream path)
        {
            int startHeader = 0xA30;
            int endheader = 0xA40;

            BinaryReader breader = new BinaryReader(path);
            breader.BaseStream.Position = startHeader;
            byte[] itemSection = breader.ReadBytes(endheader);
            string key_1 = ByteArrayToAscii(itemSection, 0,20,false );
            return key_1;
        }

        #endregion <<< Title ID >>>

        #region <<< Table Content >>>

        public string getAccountID(FileStream path)
        {
            //ACCOUNT_ID[edit]
            //Info
            //param_fmt: utf8 - S
            //param_max_len: 0x10(16 bytes)           
            //param_len: 0x10(16 bytes)
            //Tip
            //Used by: Save Data
            //PSN User Account stored as utf8 - S.The string is compared with the user info in XRegistry.sys.The comparison can only return two values, right, or wrong, if the comparison returns right the SaveData is valid.
            //Filled with zeros when the user has not been registered in PSN.

            int startHeader = 0x588;
            int endheader = 0x597;

            BinaryReader breader = new BinaryReader(path);
            breader.BaseStream.Position = startHeader;
            byte[] itemSection = breader.ReadBytes(endheader);
            string key_1 = ByteArrayToAscii(itemSection, 0, 16,false);
            return key_1;
        }

        public string GetAttribute(FileStream path)
        {
            //Info
            //param_fmt: int32
            //param_max_len: 0x4(4 bytes)
            //param_len:      0x4(4 bytes)
            //Tip
            //Used by: HDD Game, PS1 Game, Minis Game, PSP Re-masters Game, PCEngine game, NEOGEO game, Game Data, Save Data
            //Contains a maximum of 32 flags that can be turned on/ off to activate/ deactivate special boot modes and features of the content.

            //Values are stored in "Little Endian" format inside the SFO, to represent the whole tables in a "human readable" format has been needed to convert them to "Big Endian" and then to "Binary".
            int startHeader = 0x153;
            int endheader = 0x157;

            BinaryReader breader = new BinaryReader(path);
            breader.BaseStream.Position = startHeader;
            byte[] itemSection = breader.ReadBytes(endheader);

            //Always change 3rd parameter to apply the size of the field to extract should be param_max_len
            string key_1 = ByteArrayToAscii(itemSection, 0, 4, false);
            return key_1;

        }
        #region <<< GAME DATA >>>
        public string GetGameData1(FileStream path)
        {
            int startHeader = 0x158;
            int endheader = 0x168;

            BinaryReader breader = new BinaryReader(path);
            breader.BaseStream.Position = startHeader;
            byte[] itemSection = breader.ReadBytes(endheader);

            //Always change 3rd parameter to apply the size of the field to extract should be param_max_len
            string key_1 = ByteArrayToAscii(itemSection, 0, 17, false);
            return key_1;
        }

        public string GetGameData2(FileStream path)
        {
            int startHeader = 0x169;
            int endheader = 0x15C;

            BinaryReader breader = new BinaryReader(path);
            breader.BaseStream.Position = startHeader;
            byte[] itemSection = breader.ReadBytes(endheader);

            //Always change 3rd parameter to apply the size of the field to extract should be param_max_len
            string key_1 = ByteArrayToAscii(itemSection, 0, 18, false);
            return key_1;
        }

        #endregion <<< GAME DATA >>>


        #endregion <<< Table Content >>>

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
            //as a test i used The Evil Within save game
            OpenFileDialog thedialog = new OpenFileDialog();
            thedialog.Title = "PARAM.SFO";
            thedialog.Filter = ".SFO|PARAM.SFO";
            thedialog.InitialDirectory = System.Environment.SpecialFolder.MyComputer.ToString();
            if(thedialog.ShowDialog() == DialogResult.OK)
            {
                using (FileStream str = File.OpenRead(thedialog.FileName.ToString()))
                {

                     psfo = new PeXploit.PARAM_SFO(thedialog.FileName.ToString());

                    //Check MAGIC
                     if (psfo != null)
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

                        //TitleID
                        textBox1.Text = psfo.TitleID.ToString();

                        //Account ID
                        getAccountID(str);
                       

                        //GetAttribute
                        string attribute =  psfo.Attribute;

                        #region <<< Attribute String >>>

                        if(attribute == "SD")
                        {
                            textBox2.Text = "Save Data";
                        }
                        else if(attribute == "GD")
                        {
                            textBox2.Text = "Game Data";
                        }
                        else
                        {
                            textBox2.Text = attribute;
                        }

                        #endregion <<< Attribute String >>>

                        //Get Game Data 1 -Progress Level Ext

                        textBox3.Text = GetGameData1(str);

                        textBox4.Text = GetGameData2(str);

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

        private void button3_Click(object sender, EventArgs e)
        {
            RawView raw = new RawView(psfo);
            raw.ShowDialog();
        }   
    }
}
