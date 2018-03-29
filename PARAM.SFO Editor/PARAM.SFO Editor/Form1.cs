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
using System.Diagnostics;

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

        private System.Windows.Forms.Timer timer1;

        int i = 0;
        string MainPath;
        bool InitialLoad, CheckBoxBusy = false;

        PeXploit.PARAM_SFO psfo;

        Playstation version;

        public enum Playstation
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

        public void RunTimer()
        {
            timer1 = new System.Windows.Forms.Timer();
            timer1.Tick += new EventHandler(timer1_Tick);
            timer1.Interval = Convert.ToInt32(TimeSpan.FromSeconds(5).TotalMilliseconds); // in miliseconds
            timer1.Start();
            if (backgroundWorker1.IsBusy == false)
            {
                backgroundWorker1.RunWorkerAsync();
            }
        }

        public void Stoptimer()
        {
            timer1.Stop();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if(backgroundWorker1.IsBusy == false && backgroundWorker1.CancellationPending == false)
            {
                backgroundWorker1.RunWorkerAsync();
            }
        }


        private void button1_Click(object sender, EventArgs e)
        {
            gbxSoundFormat.Enabled = false;
            gbxVideo.Enabled = false;
            tbControl.TabPages.Remove(tbPS4);
            tbControl.TabPages.Remove(tbPS3);
            tbControl.TabPages.Add(tbPS4);
            tbControl.TabPages.Add(tbPS3);


            cbxAddon.Items.Clear();
            cbVersion.Items.Clear();
            cbSystemVersion.Items.Clear();
            cbxAppVersion.Items.Clear();

            chbBoot.Enabled = false;
            chbBoot.Text = "Bootable";

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
                        //set initial load too true so we dont do anything unnasasary 
                        InitialLoad = true;

                        List<string> AlreadyAdded = new List<string>();
                        foreach (PeXploit.PARAM_SFO.Table t in psfo.Tables)
                        {
                            if(t.Name == "TITLE_ID")
                            {
                                txtTitleId.Text = t.Value.Trim();
                                txtTitleId.Tag = t.Name;
                                AlreadyAdded.Add(t.Name);
                            }
                            if(t.Name == "CONTENT_ID")
                            {
                                txtContentId.Text = t.Value.Trim();
                                txtContentId.Tag = t.Name;
                                AlreadyAdded.Add(t.Name);
                            }
                            if (t.Name == "TITLE")
                            {
                                txtTitle.Text = t.Value.Trim();
                                txtTitle.Tag = t.Name;
                                AlreadyAdded.Add(t.Name);
                            }
                            if(t.Name == "CATEGORY")
                            {
                                txtCATEGORY.Text =((PeXploit.PARAM_SFO.DataTypes)BitConverter.ToUInt16(Encoding.UTF8.GetBytes(t.Value), 0)).ToString();
                                txtCATEGORY.Tag = t.Name;
                                AlreadyAdded.Add(t.Name);
                            }
                            if(t.Name == "APP_VER")
                            {
                                cbxAppVersion.Items.Add(t.Value.Trim());
                                cbxAppVersion.Tag = t.Name;
                                AlreadyAdded.Add(t.Name);
                                cbxAppVersion.SelectedIndex = 0;
                            }
                            if(t.Name == "VERSION")
                            {
                                cbVersion.Items.Add(t.Value.Trim());
                                cbVersion.Tag = t.Name;
                                AlreadyAdded.Add(t.Name);
                                cbVersion.SelectedIndex = 0;
                            }
                            if(t.Name == "PARENTAL_LEVEL")
                            {
                                if (t.Value == "")
                                {
                                    cbxParent.SelectedIndex = 0;
                                }
                                else
                                {
                                    cbxParent.Tag = t.Name;
                                    cbxParent.SelectedIndex = Convert.ToInt32(t.Value);
                                    AlreadyAdded.Add(t.Name);
                                }
                            }
                            if(t.Name == "PS3_SYSTEM_VER")
                            {
                                cbSystemVersion.Tag = t.Name;
                                //we know its PS3
                                cbSystemVersion.Items.Add(t.Value.ToString());
                                pbLogo.Image = Properties.Resources.images;
                                AlreadyAdded.Add(t.Name);
                                cbSystemVersion.SelectedIndex = 0;
                                version = Playstation.ps3;
                                tbControl.TabPages.Remove(tbPS4);
                            }
                            if(t.Name == "SYSTEM_VER")
                            {
                                cbSystemVersion.Tag = t.Name;
                                cbSystemVersion.Items.Add(t.Value.ToString());
                                pbLogo.Image = Properties.Resources.ps4_logo_white1;
                                AlreadyAdded.Add(t.Name);
                                cbSystemVersion.SelectedIndex = 0;
                                version = Playstation.ps4;
                                tbControl.TabPages.Remove(tbPS3);
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
                            if (t.Name == "BOOTABLE")
                            {
                                chbBoot.Enabled = true;
                                if (t.Value.ToString() == "0")
                                {
                                    chbBoot.Checked = false;
                                }
                                if (t.Value == "1")
                                {
                                    chbBoot.Checked = true;
                                    chbBoot.Text = "Bootable (Mode 1)";
                                }
                                if (t.Value == "2")
                                {
                                    chbBoot.Checked = true;
                                    chbBoot.Text = "Bootable (Mode 2)";
                                }
                            }
                            if (t.Name == "SOUND_FORMAT")
                            {
                                gbxSoundFormat.Enabled = true;
                                #region << PS3 Sound Format >>
                                int Val = 0;
                                int.TryParse(t.Value.Trim(), out Val);
                                switch(Val)
                                {
                                    case 1 :
                                        chbxLPCM2.Checked = true;
                                        break;
                                    default:
                                        break;
                                }
                                #endregion << PS3 Sound Format >>
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
                            Stoptimer();
                            backgroundWorker1.CancelAsync();
                        }

                        RunTimer();
                        btnRaw.Enabled = true;
                        InitialLoad = false;
                        button2.Enabled = true;
                    }
                    else
                    {
                        MessageBox.Show("The file selected isn't a valid SFO","File Error",MessageBoxButtons.OK,MessageBoxIcon.Error);
                    }
               
                }
            }
            else
            {
                tbControl.TabPages.Remove(tbPS4);
                tbControl.TabPages.Remove(tbPS3);
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
            RawView raw = new RawView(psfo,version);
            raw.Show();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            ExtractAllResources();

            tbControl.TabPages.Remove(tbPS4);
            tbControl.TabPages.Remove(tbPS3);
        }

        private string AppCommonPath()
        {
            string returnstring = "";

            returnstring = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\Ps4Tools\";

            return returnstring;
        }


        public void ExtractAllResources()
        {
            if (!Directory.Exists(AppCommonPath()))
            {
                Directory.CreateDirectory(AppCommonPath());
            }
            if (!Directory.Exists(AppCommonPath() + @"\ext\"))
            {
                Directory.CreateDirectory(AppCommonPath() + @"\ext\");
            }
            //We will replace every file each time we call any toolkit to stop issues with different versions ext ext

            ////SCE Files
            //copy byte files

            //ext
            System.IO.File.WriteAllBytes(AppCommonPath() + @"\ext\" + "di.exe", Properties.Resources.di);
            System.IO.File.WriteAllBytes(AppCommonPath() + @"\ext\" + "sc.exe", Properties.Resources.sc);
            System.IO.File.WriteAllBytes(AppCommonPath() + @"\ext\" + "libatrac9.dll", Properties.Resources.libatrac9);
            //orbis
            System.IO.File.WriteAllBytes(AppCommonPath() + "orbis-pub-cmd.exe", Properties.Resources.orbis_pub_cmd);
            System.IO.File.WriteAllBytes(AppCommonPath() + "orbis-pub-rx.dll", Properties.Resources.orbis_pub_prx);

            //copy text files
            System.IO.File.WriteAllText(AppCommonPath() + @"\ext\" + "trp_compare_default.css", Properties.Resources.trp_compare_default);

            //Delete Working Directory and recreate it
            if (Directory.Exists(AppCommonPath() + @"\Working\"))
            {
                DeleteDirectory(AppCommonPath() + @"\Working\");
            }

            Directory.CreateDirectory(AppCommonPath() + @"\Working\");

        }

        public static void DeleteDirectory(string target_dir)
        {
            string[] files = Directory.GetFiles(target_dir);
            string[] dirs = Directory.GetDirectories(target_dir);

            foreach (string file in files)
            {
                File.SetAttributes(file, FileAttributes.Normal);
                File.Delete(file);
            }

            foreach (string dir in dirs)
            {
                DeleteDirectory(dir);
            }

            Directory.Delete(target_dir, false);
        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            if (backgroundWorker1.CancellationPending)
            {
                e.Cancel = true;
            }
            else
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
        }

        private void cbxAddon_SelectedIndexChanged(object sender, EventArgs e)
        {
            foreach (PeXploit.PARAM_SFO.Table item in psfo.Tables)
            {
                if(item.Name == cbxAddon.SelectedItem.ToString().Trim() && item.Name != string.Empty)
                {
                    CheckBoxBusy = true;
                    txtAddonData.Text = item.Value.ToString();
                    CheckBoxBusy = false;
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
            if (e.Cancelled)
            {
                //item has been cancled
                MessageBox.Show("Prog Cancled");
            }
           
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
            //set all other data
            for (int i = 0; i < psfo.Tables.Length; i++)
            {
                if (psfo.Tables[i].Name == cbxAddon.SelectedText.ToString())
                {
                    psfo.Tables[i].Value = txtAddonData.Text.Trim();
                }
            }
        }

        private void chb720_CheckedChanged(object sender, EventArgs e)
        {
            if (InitialLoad == true || CheckBoxBusy == true)
            {
                return;
            }

            CheckBoxBusy = true;

            //first get the value from param table
            int iValue = 0, psfoValue = 0;

            for (int i = 0; i < psfo.Tables.Length; i++)
            {
                if (psfo.Tables[i].Name == "RESOLUTION")
                {
                    int.TryParse(psfo.Tables[i].Value, out psfoValue);
                    iValue = i;
                    break;
                }
            }
            //now we add or subtract
            if (chb720.Checked == true)
            {
                psfo.Tables[iValue].Value = (psfoValue + 4).ToString();
                if (cbxAddon.SelectedItem.ToString() == "RESOLUTION")
                {
                    txtAddonData.Text = psfo.Tables[iValue].Value.ToString();
                    CheckBoxBusy = false;
                }
            }
            else
            {
                psfo.Tables[iValue].Value = (psfoValue - 4).ToString();
                if (cbxAddon.SelectedItem.ToString() == "RESOLUTION")
                {
                    txtAddonData.Text = psfo.Tables[iValue].Value.ToString();
                    CheckBoxBusy = false;
                }
            }
        }

        private void chbx576Wide_CheckedChanged(object sender, EventArgs e)
        {
            if (InitialLoad == true || CheckBoxBusy == true)
            {
                return;
            }

            CheckBoxBusy = true;

            //first get the value from param table
            int iValue = 0, psfoValue = 0;

            for (int i = 0; i < psfo.Tables.Length; i++)
            {
                if (psfo.Tables[i].Name == "RESOLUTION")
                {
                    int.TryParse(psfo.Tables[i].Value, out psfoValue);
                    iValue = i;
                    break;
                }
            }
            //now we add or subtract
            if (chbx576Wide.Checked == true)
            {
                psfo.Tables[iValue].Value = (psfoValue + 32).ToString();
                if (cbxAddon.SelectedItem.ToString() == "RESOLUTION")
                {
                    txtAddonData.Text = psfo.Tables[iValue].Value.ToString();
                    CheckBoxBusy = false;
                }
            }
            else
            {
                psfo.Tables[iValue].Value = (psfoValue - 32).ToString();
                if (cbxAddon.SelectedItem.ToString() == "RESOLUTION")
                {
                    txtAddonData.Text = psfo.Tables[iValue].Value.ToString();
                    CheckBoxBusy = false;
                }
            }
        }

        private void chbx576_CheckedChanged(object sender, EventArgs e)
        {
            if (InitialLoad == true || CheckBoxBusy == true)
            {
                return;
            }

            CheckBoxBusy = true;

            //first get the value from param table
            int iValue = 0, psfoValue = 0;

            for (int i = 0; i < psfo.Tables.Length; i++)
            {
                if (psfo.Tables[i].Name == "RESOLUTION")
                {
                    int.TryParse(psfo.Tables[i].Value, out psfoValue);
                    iValue = i;
                    break;
                }
            }
            //now we add or subtract
            if (chbx576.Checked == true)
            {
                psfo.Tables[iValue].Value = (psfoValue + 2).ToString();
                if (cbxAddon.SelectedItem.ToString() == "RESOLUTION")
                {
                    txtAddonData.Text = psfo.Tables[iValue].Value.ToString();
                    CheckBoxBusy = false;
                }
            }
            else
            {
                psfo.Tables[iValue].Value = (psfoValue - 2).ToString();
                if (cbxAddon.SelectedItem.ToString() == "RESOLUTION")
                {
                    txtAddonData.Text = psfo.Tables[iValue].Value.ToString();
                    CheckBoxBusy = false;
                }
            }
        }

        private void chbx480Wide_CheckedChanged(object sender, EventArgs e)
        {
            if (InitialLoad == true || CheckBoxBusy == true)
            {
                return;
            }

            CheckBoxBusy = true;

            //first get the value from param table
            int iValue = 0, psfoValue = 0;

            for (int i = 0; i < psfo.Tables.Length; i++)
            {
                if (psfo.Tables[i].Name == "RESOLUTION")
                {
                    int.TryParse(psfo.Tables[i].Value, out psfoValue);
                    iValue = i;
                    break;
                }
            }
            //now we add or subtract
            if (chbx480Wide.Checked == true)
            {
                psfo.Tables[iValue].Value = (psfoValue + 16).ToString();
                if (cbxAddon.SelectedItem.ToString() == "RESOLUTION")
                {
                    txtAddonData.Text = psfo.Tables[iValue].Value.ToString();
                    CheckBoxBusy = false;
                }
            }
            else
            {
                psfo.Tables[iValue].Value = (psfoValue - 16).ToString();
                if (cbxAddon.SelectedItem.ToString() == "RESOLUTION")
                {
                    txtAddonData.Text = psfo.Tables[iValue].Value.ToString();
                    CheckBoxBusy = false;
                }
            }
        }

        private void chbx480_CheckedChanged(object sender, EventArgs e)
        {
            if (InitialLoad == true || CheckBoxBusy == true)
            {
                return;
            }

            CheckBoxBusy = true;

            //first get the value from param table
            int iValue = 0, psfoValue = 0;

            for (int i = 0; i < psfo.Tables.Length; i++)
            {
                if (psfo.Tables[i].Name == "RESOLUTION")
                {
                    int.TryParse(psfo.Tables[i].Value, out psfoValue);
                    iValue = i;
                    break;
                }
            }
            //now we add or subtract
            if (chbx480.Checked == true)
            {
                psfo.Tables[iValue].Value = (psfoValue + 1).ToString();
                if (cbxAddon.SelectedItem.ToString() == "RESOLUTION")
                {
                    txtAddonData.Text = psfo.Tables[iValue].Value.ToString();
                    CheckBoxBusy = false;
                }
            }
            else
            {
                psfo.Tables[iValue].Value = (psfoValue - 1).ToString();
                if (cbxAddon.SelectedItem.ToString() == "RESOLUTION")
                {
                    txtAddonData.Text = psfo.Tables[iValue].Value.ToString();
                    CheckBoxBusy = false;
                }
            }
        }

        private void chbx1080_CheckedChanged(object sender, EventArgs e)
        {
            if(InitialLoad == true || CheckBoxBusy == true)
            {
                return;
            }

            CheckBoxBusy = true;

            //first get the value from param table
            int iValue = 0, psfoValue = 0;
             
            for (int i = 0; i < psfo.Tables.Length; i++)
            {
                if (psfo.Tables[i].Name == "RESOLUTION")
                {
                    int.TryParse(psfo.Tables[i].Value, out psfoValue);
                    iValue = i;
                    break;
                }
            }
            //now we add or subtract
            if (chbx1080.Checked == true)
            {
                psfo.Tables[iValue].Value = (psfoValue + 8).ToString();
                if(cbxAddon.SelectedItem.ToString() == "RESOLUTION")
                {
                    txtAddonData.Text = psfo.Tables[iValue].Value.ToString();
                    CheckBoxBusy = false;
                }
            }
            else
            {
                psfo.Tables[iValue].Value = (psfoValue - 8).ToString();
                if (cbxAddon.SelectedItem.ToString() == "RESOLUTION")
                {
                    txtAddonData.Text = psfo.Tables[iValue].Value.ToString();
                    CheckBoxBusy = false;
                }
            }
        }

        private void backgroundWorker1_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            
        }

        private void button2_Click(object sender, EventArgs e)
        {
            SaveFileDialog dlg = new SaveFileDialog();
            dlg.Filter = "PARAM.SFO (PARAM.SFO)|PARAM.SFO";
            dlg.DefaultExt = "SFO";
            dlg.AddExtension = true;
            if(dlg.ShowDialog() == DialogResult.OK)
            {
                //user wants to save in a new location or whatever
                //psfo.SaveFile(psfo,dlg.FileName); //this will be added back as soon as this code is working this is for the initial release of the tool for PS4 sfo saving 
                CreateSFX(psfo, dlg);
            }
        }

        public void CreateSFX(PeXploit.PARAM_SFO psfo, SaveFileDialog dlg)
        {
            string FileHeader;
            if (version == Form1.Playstation.ps4)
            {
                //table items
                FileHeader = CreateSFXHeader();
                string XMLItem = FileHeader + "\n<paramsfo>";//begin the tag
                foreach (var item in psfo.Tables)
                {
                    XMLItem += "\n\t<param key=\"" + item.Name + "\">" + item.Value + "</param>";
                }
                XMLItem += "\n</paramsfo>";//close the tag
                                           //we dont aks user where he wants to save the file 



                System.IO.File.WriteAllText(AppCommonPath() + @"\Working\param.sfx", XMLItem);
                Orbis_CMD("", "sfo_create \"" + AppCommonPath() + @"\Working\" + "param.sfx" + "\" \"" + dlg.FileName + "\"");
                MessageBox.Show("SFO Created", "Done", MessageBoxButtons.OK, MessageBoxIcon.Information);
                System.Diagnostics.Process.Start("explorer.exe", System.IO.Path.GetDirectoryName(dlg.FileName));
                System.IO.File.Delete(AppCommonPath() + @"\Working\param.sfx");//remove the SFX
            

                

            }
            else
            {
                MessageBox.Show("SFO Saving For PS3 may be a bit buggy", "SFO Build", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                //FileHeader = CreateSFXHeader();
                //string XMLItem = FileHeader + "\n<paramsfo>";//begin the tag
                //foreach (var item in psfo.Tables)
                //{
                //    #region << Get Format >>
                //    var frmt = "";
                //    if (item.Indextable.param_data_fmt == PeXploit.PARAM_SFO.FMT.ASCII)
                //    {
                //        frmt = "utf8-S";
                //    }
                //    if (item.Indextable.param_data_fmt == PeXploit.PARAM_SFO.FMT.UINT32)
                //    {
                //        frmt = "int32";
                //    }
                //    if (item.Indextable.param_data_fmt == PeXploit.PARAM_SFO.FMT.UTF_8)
                //    {
                //        frmt = "utf8";
                //    }
                //    #endregion << Get Format >>


                //    XMLItem += "\n\t<param key=\"" + item.Name + "\"" + "fmt=\"" + frmt + "\" max_len=\"" + item.Indextable.param_data_max_len + "\"" + ">" + item.Value + "</param>";
                //}
                //XMLItem += "\n</paramsfo>";//close the tag

                psfo.SaveFile(psfo,dlg.FileName);

                MessageBox.Show("SFO Created", "Done", MessageBoxButtons.OK, MessageBoxIcon.Information);

            }


        }

        public string CreateSFXHeader()
        {
            return "<?xml version=\"1.0\" encoding=\"utf-8\" standalone=\"yes\"?>";
        }

        private void txtTitleId_TextChanged(object sender, EventArgs e)
        {
            for (int i = 0; i < psfo.Tables.Length; i++)
            {
                if(psfo.Tables[i].Name == "TITLE_ID")
                {
                    psfo.Tables[i].Value = txtTitleId.Text.Trim();
                }
            }
        }

        private void txtTitle_TextChanged(object sender, EventArgs e)
        {
            for (int i = 0; i < psfo.Tables.Length; i++)
            {
                if (psfo.Tables[i].Name == "TITLE")
                {
                    psfo.Tables[i].Value = txtTitle.Text.Trim();
                }
            }
        }

        private void cbSystemVersion_TextChanged(object sender, EventArgs e)
        {
            for (int i = 0; i < psfo.Tables.Length; i++)
            {
                if (psfo.Tables[i].Name == "SYSTEM_VER" || psfo.Tables[i].Name == "PS3_SYSTEM_VER")
                {
                    psfo.Tables[i].Value = cbSystemVersion.Text.Trim();
                }
            }
        }

        private void cbxParent_TextChanged(object sender, EventArgs e)
        {
            for (int i = 0; i < psfo.Tables.Length; i++)
            {
                if (psfo.Tables[i].Name == cbxParent.Tag.ToString())
                {
                    psfo.Tables[i].Value = cbxParent.Text.Trim();
                }
            }
        }

        private void cbxParent_SelectedIndexChanged(object sender, EventArgs e)
        {
            for (int i = 0; i < psfo.Tables.Length; i++)
            {
                if (psfo.Tables[i].Name == cbxParent.Tag.ToString())
                {
                    psfo.Tables[i].Value = cbxParent.Text.Trim();
                }
            }
        }

        private void cbVersion_SelectedIndexChanged(object sender, EventArgs e)
        {
            for (int i = 0; i < psfo.Tables.Length; i++)
            {
                if (psfo.Tables[i].Name == cbVersion.Tag.ToString())
                {
                    psfo.Tables[i].Value = cbVersion.Text.Trim();
                }
            }
        }

        private void cbxAppVersion_SelectedIndexChanged(object sender, EventArgs e)
        {
            for (int i = 0; i < psfo.Tables.Length; i++)
            {
                if (psfo.Tables[i].Name == cbxAppVersion.Tag.ToString())
                {
                    psfo.Tables[i].Value = cbxAppVersion.Text.Trim();
                }
            }
        }

        //orbis
        public string Orbis_CMD(string command, string arguments)
        {
            ProcessStartInfo start = new ProcessStartInfo();
            start.FileName = AppCommonPath() + "orbis-pub-cmd.exe " + command;
            start.Arguments = arguments;
            start.UseShellExecute = false;
            start.RedirectStandardOutput = true;
            start.CreateNoWindow = true;
            using (Process process = Process.Start(start))
            {
                process.ErrorDataReceived += Process_ErrorDataReceived;
                using (StreamReader reader = process.StandardOutput)
                {
                    string result = reader.ReadToEnd();
                    return result;
                }
            }
        }

        private void Process_ErrorDataReceived(object sender, DataReceivedEventArgs e)
        {
            MessageBox.Show("Error Creating SFO\n" + e.Data.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }
}
