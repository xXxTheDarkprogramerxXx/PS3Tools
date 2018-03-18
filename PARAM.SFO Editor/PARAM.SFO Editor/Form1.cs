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
using System.Threading;

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



        int i = 0;
        string MainPath;

        PeXploit.PARAM_SFO psfo;

        Playstation version;

        enum Playstation
        {
            ps3 = 0,
            ps4 = 2
        }

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
            cbxAddon.Items.Clear();
            cbVersion.Items.Clear();
            cbSystemVersion.Items.Clear();
            cbxAppVersion.Items.Clear();

            OpenFileDialog thedialog = new OpenFileDialog();
            thedialog.Title = "PARAM.SFO";
            thedialog.Filter = ".SFO|*.SFO";
            thedialog.InitialDirectory = System.Environment.SpecialFolder.MyComputer.ToString();
            if(thedialog.ShowDialog() == DialogResult.OK)
            {
                using (FileStream str = File.OpenRead(thedialog.FileName.ToString()))
                {

                     psfo = new PeXploit.PARAM_SFO(thedialog.FileName.ToString());

                    MainPath = System.IO.Path.GetDirectoryName(thedialog.FileName.ToString());

                    //Check MAGIC
                     if (psfo != null)
                    {
                        
                        List<string> AlreadyAdded = new List<string>();
                        foreach (PeXploit.PARAM_SFO.Table t in psfo.Tables)
                        {
                            if(t.Name == "TITLE_ID")
                            {
                                txtTitleId.Text = t.Value.Trim();
                                AlreadyAdded.Add(t.Name);
                            }
                            if(t.Name == "CONTENT_ID")
                            {
                                txtContentId.Text = t.Value.Trim();
                                AlreadyAdded.Add(t.Name);
                            }
                            if (t.Name == "TITLE")
                            {
                                

                                txtTitle.Text = t.Value.Trim(); 
                                AlreadyAdded.Add(t.Name);
                            }
                            if(t.Name == "CATEGORY")
                            {
                                txtCATEGORY.Text =((PeXploit.PARAM_SFO.DataTypes)BitConverter.ToUInt16(Encoding.UTF8.GetBytes(t.Value), 0)).ToString();
                                AlreadyAdded.Add(t.Name);
                            }
                            if(t.Name == "APP_VER")
                            {
                                cbxAppVersion.Items.Add(t.Value.Trim());
                                AlreadyAdded.Add(t.Name);
                                cbxAppVersion.SelectedIndex = 0;
                            }
                            if(t.Name == "VERSION")
                            {
                                cbVersion.Items.Add(t.Value.Trim());
                                AlreadyAdded.Add(t.Name);
                                cbVersion.SelectedIndex = 0;
                            }
                            if(t.Name == "PARENTAL_LEVEL")
                            {
                                cbxParent.SelectedIndex = Convert.ToInt32(t.Value);
                                AlreadyAdded.Add(t.Name);
                            }
                            if(t.Name == "PS3_SYSTEM_VER")
                            {
                                //we know its PS3
                                cbSystemVersion.Items.Add(t.Value.ToString());
                                pbLogo.Image = Properties.Resources.images;
                                AlreadyAdded.Add(t.Name);
                                cbSystemVersion.SelectedIndex = 0;
                                version = Playstation.ps3;
                            }
                            if(t.Name == "SYSTEM_VER")
                            {
                                cbSystemVersion.Items.Add(t.Value.ToString());
                                pbLogo.Image = Properties.Resources.ps4_logo_white1;
                                AlreadyAdded.Add(t.Name);
                                cbSystemVersion.SelectedIndex = 0;
                                version = Playstation.ps4;
                            }
                            if (t.Name == "RESOLUTION")
                            {
                                gbxVideo.Enabled = true;
                                #region << PS3 Resolution >>
                                int Val = 0;
                                int.TryParse(t.Value.Trim(), out Val);
                                switch(Val)
                                {
                                    case 1:
                                        chbx480.Checked = true;
                                        break;
                                    case 2:
                                        chbx576.Checked = true;
                                        break;
                                    case 3:
                                        chbx480.Checked = true;
                                        chbx576.Checked = true;
                                        break;
                                    case 4:
                                        chb720.Checked = true;
                                        break;
                                    case 5:
                                        chb720.Checked = true;
                                        chbx480.Checked = true;
                                        break;
                                    case 6:
                                        chb720.Checked = true;
                                        chbx576.Checked = true;
                                        break;
                                    case 7:
                                        chb720.Checked = true;
                                        chbx576.Checked = true;
                                        chbx480.Checked = true;
                                        break;
                                    case 8:
                                        chbx1080.Checked = true;
                                        break;
                                    case 9:
                                        chbx1080.Checked = true;
                                        chbx480.Checked = true;
                                        break;
                                    case 10:
                                        chbx1080.Checked = true;
                                        chbx576.Checked = true;
                                        break;
                                    case 11:
                                        chbx1080.Checked = true;
                                        chbx576.Checked = true;
                                        chbx480.Checked = true;
                                        break;
                                    case 12:
                                        chbx1080.Checked = true;
                                        chb720.Checked = true;
                                        break;
                                    case 13:
                                        chbx1080.Checked = true;
                                        chb720.Checked = true;
                                        chbx480.Checked = true;
                                        break;
                                    case 14:
                                        chbx1080.Checked = true;
                                        chb720.Checked = true;
                                        chbx576.Checked = true;
                                        break;
                                    case 15:
                                        chbx1080.Checked = true;
                                        chb720.Checked = true;
                                        chbx576.Checked = true;
                                        chbx480.Checked = true;
                                        break;
                                    case 16:
                                        chbx480Wide.Checked = true;
                                        break;
                                    case 17:
                                        chbx480Wide.Checked = true;
                                        chbx480.Checked = true;
                                        break;
                                    case 18:
                                        chbx480Wide.Checked = true;
                                        chbx576.Checked = true;
                                        break;
                                    case 19:
                                        chbx480.Checked = true;
                                        chbx480Wide.Checked = true;
                                        chbx576.Checked = true;
                                        break;
                                    case 20:
                                        chbx480Wide.Checked = true;
                                        chb720.Checked = true;
                                        break;
                                    case 21:
                                        chbx480.Checked = true;
                                        chbx480Wide.Checked = true;
                                        chb720.Checked = true;
                                        break;
                                    case 22:
                                        chbx480Wide.Checked = true;
                                        chbx576.Checked = true;
                                        chb720.Checked = true;
                                        break;
                                    case 23:
                                        chbx480Wide.Checked = true;
                                        chbx480.Checked = true;
                                        chbx576.Checked = true;
                                        chb720.Checked = true;
                                        break;
                                    case 24:
                                        chbx1080.Checked = true;
                                        chbx480Wide.Checked = true;
                                        break;
                                    case 25:
                                        chbx1080.Checked = true;
                                        chbx480.Checked = true;
                                        chbx480Wide.Checked = true;
                                        break;
                                    case 26:
                                        chbx1080.Checked = true;
                                        chbx576.Checked = true;
                                        chbx480Wide.Checked = true;
                                        break;
                                    case 27:
                                        chbx1080.Checked = true;
                                        chbx576.Checked = true;
                                        chbx480.Checked = true;
                                        chbx480Wide.Checked = true;
                                        break;
                                    case 28:
                                        chbx1080.Checked = true;
                                        chb720.Checked = true;
                                        chbx480Wide.Checked = true;
                                        break;
                                    case 29:
                                        chbx1080.Checked = true;
                                        chb720.Checked = true;
                                        chbx480.Checked = true;
                                        chbx480Wide.Checked = true;
                                        break;
                                    case 30:
                                        chbx1080.Checked = true;
                                        chb720.Checked = true;
                                        chbx576.Checked = true;
                                        chbx480Wide.Checked = true;
                                        break;
                                    case 31:
                                        chbx1080.Checked = true;
                                        chb720.Checked = true;
                                        chbx480.Checked = true;
                                        chbx480Wide.Checked = true;
                                        chbx576.Checked = true;
                                        break;
                                    case 32:
                                        chbx576Wide.Checked = true;
                                        break;
                                    case 33:
                                        chbx576Wide.Checked = true;
                                        chbx480.Checked = true;
                                        break;
                                    case 34:
                                        chbx576Wide.Checked = true;
                                        chbx576.Checked = true;
                                        break;
                                    case 35:
                                        chbx576Wide.Checked = true;
                                        chbx576.Checked = true;
                                        chbx480.Checked = true;
                                        break;
                                    case 36:
                                        chbx576Wide.Checked = true;
                                        chb720.Checked = true;
                                        break;
                                    case 37:
                                        chb720.Checked = true;
                                        chbx576Wide.Checked = true;
                                        chbx480.Checked = true;
                                        break;
                                    case 38:
                                        chb720.Checked = true;
                                        chbx576.Checked = true;
                                        chbx576Wide.Checked = true;
                                        break;
                                    case 39:
                                        chb720.Checked = true;
                                        chbx576Wide.Checked = true;
                                        chbx576.Checked = true;
                                        chbx480.Checked = true;
                                        break;
                                    case 40 :
                                        chbx576Wide.Checked = true;
                                        chbx1080.Checked = true;
                                        break;
                                    case 41:
                                        chbx1080.Checked = true;
                                        chbx576Wide.Checked = true;
                                        chbx480.Checked = true;
                                        break;
                                    case 42:
                                        chbx1080.Checked = true;
                                        chbx576Wide.Checked = true;
                                        chbx576.Checked = true;
                                        break;
                                    case 43:
                                        chbx1080.Checked = true;
                                        chbx576Wide.Checked = true;
                                        chbx576.Checked = true;
                                        chbx480.Checked = true;
                                        break;
                                    case 44:                                  
                                        chbx1080.Checked = true;
                                        chbx576Wide.Checked = true;
                                        chb720.Checked = true;
                                        break;
                                    case 45:
                                        chbx1080.Checked = true;
                                        chbx576Wide.Checked = true;
                                        chb720.Checked = true;
                                        chbx480.Checked = true;
                                        break;
                                    case 46:
                                        chbx1080.Checked = true;
                                        chbx576Wide.Checked = true;
                                        chb720.Checked = true;
                                        chbx576.Checked = true;
                                        break;
                                    case 47:
                                        chbx1080.Checked = true;
                                        chbx576Wide.Checked = true;
                                        chb720.Checked = true;
                                        chbx576.Checked = true;
                                        chbx480.Checked = true;
                                        break;
                                    case 48:
                                        chbx480Wide.Checked = true;
                                        chbx576Wide.Checked = true;
                                        break;
                                    case 49:
                                        chbx480Wide.Checked = true;
                                        chbx576Wide.Checked = true;
                                        chbx480.Checked = true;
                                        break;
                                    case 50:
                                        chbx480Wide.Checked = true;
                                        chbx576Wide.Checked = true;
                                        chbx576.Checked = true;
                                        break;
                                    case 51:
                                        chbx480Wide.Checked = true;
                                        chbx576Wide.Checked = true;
                                        chbx576.Checked = true;
                                        chbx480.Checked = true;
                                        break;
                                    case 52:
                                        chbx480Wide.Checked = true;
                                        chbx576Wide.Checked = true;
                                        chb720.Checked = true;
                                        break;
                                    case 53:
                                        chbx480Wide.Checked = true;
                                        chbx576Wide.Checked = true;
                                        chb720.Checked = true;
                                        chbx480.Checked = true;
                                        break;
                                    case 54:
                                        chbx480Wide.Checked = true;
                                        chbx576Wide.Checked = true;
                                        chb720.Checked = true;
                                        chbx576.Checked = true;
                                        break;
                                    case 55:
                                        chbx480Wide.Checked = true;
                                        chbx576Wide.Checked = true;
                                        chb720.Checked = true;
                                        chbx576.Checked = true;
                                        chbx480.Checked = true;
                                        break;
                                    case 56:
                                        chbx480Wide.Checked = true;
                                        chbx576Wide.Checked = true;
                                        chbx1080.Checked = true;
                                        break;
                                    case 57:
                                        chbx480Wide.Checked = true;
                                        chbx576Wide.Checked = true;
                                        chbx1080.Checked = true;
                                        chbx480.Checked = true;
                                        break;
                                    case 58:
                                        chbx480Wide.Checked = true;
                                        chbx576Wide.Checked = true;
                                        chbx1080.Checked = true;
                                        chbx576.Checked = true;
                                        break;
                                    case 59:
                                        chbx480Wide.Checked = true;
                                        chbx576Wide.Checked = true;
                                        chbx1080.Checked = true;
                                        chbx576.Checked = true;
                                        chbx480.Checked = true;
                                        break;
                                    case 60:
                                        chbx480Wide.Checked = true;
                                        chbx576Wide.Checked = true;
                                        chbx1080.Checked = true;
                                        chb720.Checked = true;
                                        break;
                                    case 61:
                                        chbx480Wide.Checked = true;
                                        chbx576Wide.Checked = true;
                                        chbx1080.Checked = true;
                                        chb720.Checked = true;
                                        chbx480.Checked = true;
                                        break;
                                    case 62:
                                        chbx480Wide.Checked = true;
                                        chbx576Wide.Checked = true;
                                        chbx1080.Checked = true;
                                        chb720.Checked = true;
                                        chbx576.Checked = true;
                                        break;
                                    case 63:
                                        chbx480Wide.Checked = true;
                                        chbx576Wide.Checked = true;
                                        chbx1080.Checked = true;
                                        chb720.Checked = true;
                                        chbx576.Checked = true;
                                        chbx480.Checked = true;
                                        break;
                                    default:
                                        break;
                                }
                                #endregion << PS3 Resolution >>
                            }

                            if(!AlreadyAdded.Contains(t.Name))
                            {
                                cbxAddon.Items.Add(t.Name);
                            }


                        }

                        //after loading we need to spesify some things
                        cbxAddon.SelectedIndex = 0;
                        if(backgroundWorker1.IsBusy == true)
                        {
                            backgroundWorker1.CancelAsync();
                        }
                         while(backgroundWorker1.IsBusy == true)
                         {
                             Thread.Sleep(100);
                         }
                        backgroundWorker1.RunWorkerAsync();
                        btnRaw.Enabled = true;
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

        }

        private void Uncheck_Resolution_All()
        {
            chb720.Checked = false;
            chbx1080.Checked = false;
            chbx480.Checked = false;
            chbx480Wide.Checked = false;
            chbx576.Checked = false;
            chbx576Wide.Checked = false;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            RawView raw = new RawView(psfo);
            raw.Show();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            while (i == 0)
            {
                #region << PS3 >>
                if (version == Playstation.ps3)
                {
                    
                    if (Directory.Exists(MainPath + @"\C00\") && i == 0)
                    {
                        pbLogoAndBackground.ImageLocation = MainPath + @"\C00\ICON0.PNG";
                        i = 1;
                        Thread.Sleep(1500);
                    }
                    else
                    {
                        pbLogoAndBackground.ImageLocation = MainPath + @"\ICON0.PNG";
                        i = 1;
                        Thread.Sleep(1500);
                    }
                }
                #endregion << PS3 >>
                else
                {
                    pbLogoAndBackground.ImageLocation = MainPath + @"\ICON0.PNG";
                    i = 1;
                    Thread.Sleep(1500);
                }
            }
            while (i == 1)
            {
                try
                {
                    #region << PS3 >>
                    if (version == Playstation.ps3)
                    {
                        if (Directory.Exists(MainPath + @"\C00\") && i == 1)
                        {
                            pbLogoAndBackground.ImageLocation = MainPath + @"\C00\PIC1.PNG";
                            i = 0;
                            Thread.Sleep(1500);
                        }
                        else
                        {
                            pbLogoAndBackground.ImageLocation = MainPath + @"\PIC1.PNG";
                            i = 0;
                            Thread.Sleep(1500);
                        }
                    }
                    #endregion << PS3 >>

                    else
                    {
                        Random rnd = new Random();
                        int ran = rnd.Next(1, 3);
                        if (ran == 1)
                        {
                            pbLogoAndBackground.ImageLocation = MainPath + @"\PIC1.PNG";
                        }
                        else
                        {
                            if (File.Exists(MainPath + @"\PIC0.PNG"))
                            {
                                pbLogoAndBackground.ImageLocation = MainPath + @"\PIC0.PNG";
                            }
                        }
                        i = 0;
                        Thread.Sleep(1500);
                    }
                }
                catch (Exception ee)
                {
                    string test = ee.Message;
                }
            }

        }

        private void cbxAddon_SelectedIndexChanged(object sender, EventArgs e)
        {
            foreach (PeXploit.PARAM_SFO.Table item in psfo.Tables)
            {
                if(item.Name == cbxAddon.SelectedItem.ToString().Trim())
                {
                    txtAddonData.Text = item.Value.ToString();
                }
            }
        }

        private void txtAddonData_Leave(object sender, EventArgs e)
        {
            //on leave save the info to the table
            for (int i = 0; i < psfo.Tables.Length; i++)
            {
                if(psfo.Tables[i].Name == cbxAddon.SelectedItem.ToString().Trim())
                {
                    psfo.Tables[i].Value = txtAddonData.Text.Trim();
                }
            }
        }

        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            backgroundWorker1.RunWorkerAsync();
        }

        private void txtAddonData_TextChanged(object sender, EventArgs e)
        {
            if(cbxAddon.SelectedItem.ToString() == "RESOLUTION")
            {
                Uncheck_Resolution_All();
                #region << PS3 Resolution >>
                int Val = 0;
                int.TryParse(txtAddonData.Text.Trim(), out Val);
                switch (Val)
                {
                    case 1:
                        chbx480.Checked = true;
                        break;
                    case 2:
                        chbx576.Checked = true;
                        break;
                    case 3:
                        chbx480.Checked = true;
                        chbx576.Checked = true;
                        break;
                    case 4:
                        chb720.Checked = true;
                        break;
                    case 5:
                        chb720.Checked = true;
                        chbx480.Checked = true;
                        break;
                    case 6:
                        chb720.Checked = true;
                        chbx576.Checked = true;
                        break;
                    case 7:
                        chb720.Checked = true;
                        chbx576.Checked = true;
                        chbx480.Checked = true;
                        break;
                    case 8:
                        chbx1080.Checked = true;
                        break;
                    case 9:
                        chbx1080.Checked = true;
                        chbx480.Checked = true;
                        break;
                    case 10:
                        chbx1080.Checked = true;
                        chbx576.Checked = true;
                        break;
                    case 11:
                        chbx1080.Checked = true;
                        chbx576.Checked = true;
                        chbx480.Checked = true;
                        break;
                    case 12:
                        chbx1080.Checked = true;
                        chb720.Checked = true;
                        break;
                    case 13:
                        chbx1080.Checked = true;
                        chb720.Checked = true;
                        chbx480.Checked = true;
                        break;
                    case 14:
                        chbx1080.Checked = true;
                        chb720.Checked = true;
                        chbx576.Checked = true;
                        break;
                    case 15:
                        chbx1080.Checked = true;
                        chb720.Checked = true;
                        chbx576.Checked = true;
                        chbx480.Checked = true;
                        break;
                    case 16:
                        chbx480Wide.Checked = true;
                        break;
                    case 17:
                        chbx480Wide.Checked = true;
                        chbx480.Checked = true;
                        break;
                    case 18:
                        chbx480Wide.Checked = true;
                        chbx576.Checked = true;
                        break;
                    case 19:
                        chbx480.Checked = true;
                        chbx480Wide.Checked = true;
                        chbx576.Checked = true;
                        break;
                    case 20:
                        chbx480Wide.Checked = true;
                        chb720.Checked = true;
                        break;
                    case 21:
                        chbx480.Checked = true;
                        chbx480Wide.Checked = true;
                        chb720.Checked = true;
                        break;
                    case 22:
                        chbx480Wide.Checked = true;
                        chbx576.Checked = true;
                        chb720.Checked = true;
                        break;
                    case 23:
                        chbx480Wide.Checked = true;
                        chbx480.Checked = true;
                        chbx576.Checked = true;
                        chb720.Checked = true;
                        break;
                    case 24:
                        chbx1080.Checked = true;
                        chbx480Wide.Checked = true;
                        break;
                    case 25:
                        chbx1080.Checked = true;
                        chbx480.Checked = true;
                        chbx480Wide.Checked = true;
                        break;
                    case 26:
                        chbx1080.Checked = true;
                        chbx576.Checked = true;
                        chbx480Wide.Checked = true;
                        break;
                    case 27:
                        chbx1080.Checked = true;
                        chbx576.Checked = true;
                        chbx480.Checked = true;
                        chbx480Wide.Checked = true;
                        break;
                    case 28:
                        chbx1080.Checked = true;
                        chb720.Checked = true;
                        chbx480Wide.Checked = true;
                        break;
                    case 29:
                        chbx1080.Checked = true;
                        chb720.Checked = true;
                        chbx480.Checked = true;
                        chbx480Wide.Checked = true;
                        break;
                    case 30:
                        chbx1080.Checked = true;
                        chb720.Checked = true;
                        chbx576.Checked = true;
                        chbx480Wide.Checked = true;
                        break;
                    case 31:
                        chbx1080.Checked = true;
                        chb720.Checked = true;
                        chbx480.Checked = true;
                        chbx480Wide.Checked = true;
                        chbx576.Checked = true;
                        break;
                    case 32:
                        chbx576Wide.Checked = true;
                        break;
                    case 33:
                        chbx576Wide.Checked = true;
                        chbx480.Checked = true;
                        break;
                    case 34:
                        chbx576Wide.Checked = true;
                        chbx576.Checked = true;
                        break;
                    case 35:
                        chbx576Wide.Checked = true;
                        chbx576.Checked = true;
                        chbx480.Checked = true;
                        break;
                    case 36:
                        chbx576Wide.Checked = true;
                        chb720.Checked = true;
                        break;
                    case 37:
                        chb720.Checked = true;
                        chbx576Wide.Checked = true;
                        chbx480.Checked = true;
                        break;
                    case 38:
                        chb720.Checked = true;
                        chbx576.Checked = true;
                        chbx576Wide.Checked = true;
                        break;
                    case 39:
                        chb720.Checked = true;
                        chbx576Wide.Checked = true;
                        chbx576.Checked = true;
                        chbx480.Checked = true;
                        break;
                    case 40:
                        chbx576Wide.Checked = true;
                        chbx1080.Checked = true;
                        break;
                    case 41:
                        chbx1080.Checked = true;
                        chbx576Wide.Checked = true;
                        chbx480.Checked = true;
                        break;
                    case 42:
                        chbx1080.Checked = true;
                        chbx576Wide.Checked = true;
                        chbx576.Checked = true;
                        break;
                    case 43:
                        chbx1080.Checked = true;
                        chbx576Wide.Checked = true;
                        chbx576.Checked = true;
                        chbx480.Checked = true;
                        break;
                    case 44:
                        chbx1080.Checked = true;
                        chbx576Wide.Checked = true;
                        chb720.Checked = true;
                        break;
                    case 45:
                        chbx1080.Checked = true;
                        chbx576Wide.Checked = true;
                        chb720.Checked = true;
                        chbx480.Checked = true;
                        break;
                    case 46:
                        chbx1080.Checked = true;
                        chbx576Wide.Checked = true;
                        chb720.Checked = true;
                        chbx576.Checked = true;
                        break;
                    case 47:
                        chbx1080.Checked = true;
                        chbx576Wide.Checked = true;
                        chb720.Checked = true;
                        chbx576.Checked = true;
                        chbx480.Checked = true;
                        break;
                    case 48:
                        chbx480Wide.Checked = true;
                        chbx576Wide.Checked = true;
                        break;
                    case 49:
                        chbx480Wide.Checked = true;
                        chbx576Wide.Checked = true;
                        chbx480.Checked = true;
                        break;
                    case 50:
                        chbx480Wide.Checked = true;
                        chbx576Wide.Checked = true;
                        chbx576.Checked = true;
                        break;
                    case 51:
                        chbx480Wide.Checked = true;
                        chbx576Wide.Checked = true;
                        chbx576.Checked = true;
                        chbx480.Checked = true;
                        break;
                    case 52:
                        chbx480Wide.Checked = true;
                        chbx576Wide.Checked = true;
                        chb720.Checked = true;
                        break;
                    case 53:
                        chbx480Wide.Checked = true;
                        chbx576Wide.Checked = true;
                        chb720.Checked = true;
                        chbx480.Checked = true;
                        break;
                    case 54:
                        chbx480Wide.Checked = true;
                        chbx576Wide.Checked = true;
                        chb720.Checked = true;
                        chbx576.Checked = true;
                        break;
                    case 55:
                        chbx480Wide.Checked = true;
                        chbx576Wide.Checked = true;
                        chb720.Checked = true;
                        chbx576.Checked = true;
                        chbx480.Checked = true;
                        break;
                    case 56:
                        chbx480Wide.Checked = true;
                        chbx576Wide.Checked = true;
                        chbx1080.Checked = true;
                        break;
                    case 57:
                        chbx480Wide.Checked = true;
                        chbx576Wide.Checked = true;
                        chbx1080.Checked = true;
                        chbx480.Checked = true;
                        break;
                    case 58:
                        chbx480Wide.Checked = true;
                        chbx576Wide.Checked = true;
                        chbx1080.Checked = true;
                        chbx576.Checked = true;
                        break;
                    case 59:
                        chbx480Wide.Checked = true;
                        chbx576Wide.Checked = true;
                        chbx1080.Checked = true;
                        chbx576.Checked = true;
                        chbx480.Checked = true;
                        break;
                    case 60:
                        chbx480Wide.Checked = true;
                        chbx576Wide.Checked = true;
                        chbx1080.Checked = true;
                        chb720.Checked = true;
                        break;
                    case 61:
                        chbx480Wide.Checked = true;
                        chbx576Wide.Checked = true;
                        chbx1080.Checked = true;
                        chb720.Checked = true;
                        chbx480.Checked = true;
                        break;
                    case 62:
                        chbx480Wide.Checked = true;
                        chbx576Wide.Checked = true;
                        chbx1080.Checked = true;
                        chb720.Checked = true;
                        chbx576.Checked = true;
                        break;
                    case 63:
                        chbx480Wide.Checked = true;
                        chbx576Wide.Checked = true;
                        chbx1080.Checked = true;
                        chb720.Checked = true;
                        chbx576.Checked = true;
                        chbx480.Checked = true;
                        break;
                    default:
                        break;
                }
                #endregion << PS3 Resolution >>
            }
        }
    }
}
