using DiscUtils.Iso9660;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Resources;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using System.IO.Compression;
using System.Net;
using System.Text.RegularExpressions;
using System.Security.AccessControl;
using System.Security.Principal;

namespace PS4_PS2_Classis_GUI
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }


        XmlDataDocument xmldoc = null;
        string xmlcontentid = "";


        //items needed
        string PS2ID;
        string[] elfs;
        private string[][] Apps;
        private string[][] Auths;
        private StringComparison ignore = StringComparison.InvariantCultureIgnoreCase;
        private List<string> Fws;

        bool BusyCoping = false;

        private void Form1_Load(object sender, EventArgs e)
        {
            //img button stuff

            //extract all resources for the current program
            ExtractAllResources();
            //Load the GP4 after extracted
            LoadGp4();

            //quickly read sfo 
            string RunningPath = AppDomain.CurrentDomain.BaseDirectory;
            PS2ClassicsSfo.SFO sfo = new PS2ClassicsSfo.SFO(AppCommonPath() + @"\PS2Emu\" + "param.sfo");

            //all we want to change is the Content ID which will rename the package 
            txtContentID.Text = sfo.ContentID.ToString().Trim().Substring(7, 9);

            //Load AuthDB we need to resign all the self files
            //LoadAuthDB();//not using this right now either
        }

        /// <summary>
        /// load Auth this method comes from CFW Prophet 
        /// Follow him on Tiwtter @cfwprophet or visit his github https://github.com/cfwprpht
        /// </summary>
        public void LoadAuthDB()
        {
            if (!File.Exists(AppCommonPath() + "authinfo.txt"))
                MessageBox.Show("Can not find authinfo.txt.");
            else
            {
                Fws = new List<string>();
                List<string[]> _Apps = new List<string[]>();
                List<string[]> _Auths = new List<string[]>();
                List<string> _apps = new List<string>();
                List<string> _auths = new List<string>();
                bool app, auth, fw;
                app = auth = fw = false;
                foreach (string line in File.ReadAllLines(AppCommonPath() + "authinfo.txt"))
                {
                    if (!string.IsNullOrEmpty(line))
                    {
                        if (line.Contains("[FW="))
                        {
                            if (Fws.Count > 0 && app) { MessageBox.Show("DataBase Inconsistent !"); break; }

                            string Is = @"=";
                            string Ie = "]";

                            int start = line.ToString().IndexOf(Is) + Is.Length;
                            int end = line.ToString().IndexOf(Ie, start);
                            if (end > start)
                                Fws.Add(line.ToString().Substring(start, end - start));
                            fw = true;
                            if (Fws.Count > 1)
                            {
                                _Apps.Add(_apps.ToArray());
                                _Auths.Add(_auths.ToArray());
                                _apps = new List<string>();
                                _auths = new List<string>();
                            }

                        }
                        else if (line.Contains("[Name="))
                        {
                            if (!fw)
                            {
                                if (!auth) { MessageBox.Show("DataBase Inconsistent !"); break; }
                            }

                            string Is = @"=";
                            string Ie = "]";

                            int start = line.ToString().IndexOf(Is) + Is.Length;
                            int end = line.ToString().IndexOf(Ie, start);
                            if (end > start)
                                _apps.Add(line.ToString().Substring(start, end - start));
                            auth = fw = false;
                            app = true;
                        }
                        else if (line.Contains("[Auth="))
                        {
                            if (!app)
                            {
                                if (fw) { MessageBox.Show("DataBase Inconsistent !"); break; }
                            }
                            string Is = @"=";
                            string Ie = "]";

                            int start = line.ToString().IndexOf(Is) + Is.Length;
                            int end = line.ToString().IndexOf(Ie, start);
                            if (end > start)
                                _auths.Add(line.ToString().Substring(start, end - start));
                            app = false;
                            auth = true;
                        }
                    }
                }

                _Apps.Add(_apps.ToArray());
                _Auths.Add(_auths.ToArray());

                Apps = _Apps.ToArray();
                Auths = _Auths.ToArray();
                //will need to clean this up a bit later
            }
        }

        /// <summary>
        /// Load the XML Project File for PS4 PKG's/ISO's
        /// in this example i will show how to mine the content id
        /// </summary>
        public void LoadGp4()
        {
            //create new XML Document 
            xmldoc = new XmlDataDocument();
            //nodelist 
            XmlNodeList xmlnode;
            //setup the resource file to be extarcted
            string RunningPath = AppDomain.CurrentDomain.BaseDirectory;
            //load the xml file from the base directory
            xmldoc.Load(AppCommonPath() + @"\PS2Emu\" + "PS2Classics.gp4");
            //now load the nodes
            xmlnode = xmldoc.GetElementsByTagName("volume");//volume is inside the xml
            //loop to get all info from the node list
            foreach (XmlNode xn in xmlnode)
            {
                XmlNode xNode = xn.SelectSingleNode("package");
                if (xNode != null)
                {
                    //we found the info we are looking for
                    xmlcontentid = xNode.Attributes[0].Value.ToString();//fetch the attribute
                }
            }

        }

        /// <summary>
        /// Save the GP4 so we can build the PKG Via Command Prompt
        /// </summary>
        public void SaveGp4()
        {
            //create new XML Document 
            xmldoc = new XmlDataDocument();
            //nodelist 
            XmlNodeList xmlnode;
            //setup the resource file to be extarcted
            string RunningPath = AppDomain.CurrentDomain.BaseDirectory;
            //load the xml file from the base directory
            xmldoc.Load(AppCommonPath() + @"\PS2Emu\" + "PS2Classics.gp4");
            //now load the nodes
            xmlnode = xmldoc.GetElementsByTagName("volume");//volume is inside the xml
            //loop to get all info from the node list
            foreach (XmlNode xn in xmlnode)
            {
                XmlNode xNode = xn.SelectSingleNode("package");
                if (xNode != null)
                {
                    //we found the info we are looking for
                    xNode.Attributes[0].Value = xmlcontentid;//set the attribute
                }
            }
            ////Uncomment this if you want to use the current datetime
            //xmlnode = xmldoc.GetElementsByTagName("volume_ts");
            //foreach (XmlNode item in xmlnode)
            //{
            //    item.InnerText = DateTime.Now.ToString("YYYY-MM-DD HH:mm:ss");//2018-03-21 15:37:08
            //}
            
            xmldoc.Save(AppCommonPath() + @"\PS2Emu\" + "PS2Classics.gp4");

        }

        #region << orbis_pub_cmd >>

        /*
         *orbis-pub-cmd.exe gp4_proj_create --volume_type pkg_ps4_app --storage_type bd50 --content_id IV0002-NPXS29038_00-SIMPLESHOOTINGAM --passcode GvE6xCpZxd96scOUGuLPbuLp8O800B0s simple_shooting_game.gp4 
         */

        #region << Update PS4 Project >>



        #endregion << Update PS4 Project >>

        #region << --volume_type >>

        public enum Volume_Type_PKG
        {
            //Project For a Package
            [Description("Project for an application package")]
            pkg_ps4_app,
            [Description("Project for a patch package")]
            pkg_ps4_patch,
            [Description("Project for a remaster package")]
            pkg_ps4_remaster,
            [Description("Project for an additional content package (with extra data)")]
            pkg_ps4_ac_data,
            [Description("Project for an additional content package (without extra data)")]
            pkg_ps4_ac_nodata,
            [Description("Project for a system software theme package")]
            pkg_ps4_theme

        }

        public enum Volume_Type_ISO
        {
            //Project For a Package
            [Description("Project for an ISO image file (BD, Max 25 GB)")]
            bd25,
            [Description("Project for an ISO image file (BD, Max 50 GB)")]
            bd50,
            [Description("Project for an ISO image file (BD, Max 50GB + BD, Max 25GB)")]
            bd50_25,
            [Description("Project for an ISO image file (BD, Max 50GB + BD, Max 50GB)")]
            bd50_50
        }

        //examplecode
        //Example: --volume_type pkg_ps4_app

        #endregion << --volume_type>>

        #region << _volume_ts TimeStamp>>

        /// <summary>
        /// This will return current timestamp as YYYY-MM-DD hh:mm:ss
        /// </summary>
        /// <returns>--volume_ts "2014-01-01 12:34:56"</returns>

        public string GetCurrentTimeStamp()
        {
            return "--volume_ts \"" + DateTime.Now.ToString("YYYY-MM-DD hh:mm:ss") + "\"";
        }

        //examplecode
        //Example: --volume_ts "2014-01-01 12:34:56"
        #endregion << _volume_ts TimeStamp>>

        #region << Enum List>>
        
       

        #endregion << Enum List>>

        #region << --content_id content_id>>
        /// <summary>
        /// this will set the game id
        /// </summary>
        /// <param name="GameID"></param>
        /// <returns>--content_id " + "UP9000-" + GameID + "_00-SLUS209090000001"</returns>
        public string SetContentID(string GameID)
        {
            //set the game id
            return "--content_id " + "UP9000-" + GameID + "_00-SLUS209090000001";
        }

        //examplecode
        //Example: --content_id UP9000-CRST00001_00-SLUS209090000001
        #endregion << --content_id content_id>>

        #region <<--passcode passcode >>
        public string SetPasscode(string passcode)
        {
            return "--passcode " + passcode;
        }

        public string Use_Default_Passcode()
        {
            return "--passcode ng8II8vax3iXZU7sfI3ugo8XlebJ731o";//default 
        }

        //Example: --passcode GvE6xCpZxd96scOUGuLPbuLp8O800B0s
        #endregion <<--passcode passcode >>


        #region <<--storage_type Storage Type >>

        public enum Storage_Type_Application
        {
            //Project For a Package
            [Description("Digital and BD, Max 25 GB")]
            bd25,
            [Description("Digital and BD, Max 50 GB")]
            bd50,
            [Description("Digital and 2 BDs, Max 50GB+25GB (2 images)")]
            bd50_25,
            [Description("Digital and 2 BDs, Max 50GB+50GB (2 images)")]
            bd50_50,
            [Description("Digital only, Max 50 GB")]
            digital50
        }

        public enum Storage_Type_Patch
        {
            //Project For a Package
            [Description("Digital only, Max 25 GB")]
            digital25
        }

        public enum Storage_Type_Remaster
        {
            //Project For a Package
            [Description("Digital and BD, Max 25 GB")]
            bd25,
            [Description("Digital and BD, Max 50 GB")]
            bd50,
            [Description("Digital only, Max 25 GB")]
            digital25,
            [Description("Digital only, Max 50 GB")]
            digital50
        }

        #endregion <<--storage_type Storage Type >>

        #region << --app_type app_type >>

        public enum App_Type
        {
            //Project For a Package
            [Description("Paid Standalone Full App")]
            full,
            [Description("Upgradable App")]
            upgradable,
            [Description("Demo App")]
            demo,
            [Description("Freemium App")]
            freemium,
        }

        #endregion << --app_type app_type >>

        //we will work with an existing XML

        public string Orbis_CMD(string command, string arguments)
        {
            ProcessStartInfo start = new ProcessStartInfo();
            start.FileName = AppCommonPath() + "orbis-pub-cmd.exe " + command;
            start.Arguments = arguments ;
            start.UseShellExecute = false;
            start.RedirectStandardOutput = true;
            start.CreateNoWindow = true;
            using (Process process = Process.Start(start))
            {
                process.ErrorDataReceived += Process_ErrorDataReceived;
                using (StreamReader reader = process.StandardOutput)
                {
                    string result = reader.ReadToEnd();
                    if(result.Contains("already converted from elf file to self file"))
                    {
                        DialogResult dlr =  MessageBox.Show("Already Converted From Elf Error Found.... will be using Orbis-pub-gen for this pkg\n\n Simply Click Build and select the save folder","Error with an alternative",MessageBoxButtons.OKCancel,MessageBoxIcon.Question);
                        if(dlr == DialogResult.OK)
                        {
                            //this will open up the GP4 Project inside the Utility
                            Orbis_Pub__GenCMD("", AppCommonPath() + @"\PS2Emu\" + "PS2Classics.gp4");

                        }
                    }
                    else if (result.Contains("[Error]"))
                    {
                        MessageBox.Show(result);
                    }
                    return result;
                }
            }
        }


        public string Orbis_Pub__GenCMD(string command, string arguments)
        {
            ProcessStartInfo start = new ProcessStartInfo();
            start.FileName = AppCommonPath() + "orbis-pub-gen.exe " + command;
            start.Arguments = arguments;
            start.UseShellExecute = false;
            start.RedirectStandardOutput = false;
            start.CreateNoWindow = false;
            using (Process process = Process.Start(start))
            {
                process.WaitForExit();
            }
            return "";
        }

        private void Process_ErrorDataReceived(object sender, DataReceivedEventArgs e)
        {


            if(e.Data.ToString().Trim() == "")
            {
                MessageBox.Show(e.Data.ToString());
            }
        }

        /// <summary>
        /// Fake Sign and spoof authentication informations. //CFW Prophets stuff not uing right now
        /// </summary>
        /// <param name="deci">The TitleIDs decimal value as a hex string.</param>
        private bool FakeSign(string deci)
        {
            string _elfs = string.Empty;
            int count = 0;
            bool err = false;

            ProcessStartInfo run = new ProcessStartInfo();
            Process call = new Process();
            call.ErrorDataReceived += Ps2_ErrorHandler;
            run.FileName = AppCommonPath() + "make_fself.exe";
            run.UseShellExecute = false;
            run.CreateNoWindow = run.RedirectStandardError = true;

            foreach (string elf in Apps[2])
            {
                //need to make sure name matches 
                if(!elf.Contains("prx"))
                {

                }
                string auth = Auths[2][count];
                auth = deci + auth.Substring(4, auth.Length - 4);

                _elfs = string.Empty;
                foreach (string found in elfs) { if (found.Contains(elf)) _elfs = found; }
                if (_elfs == string.Empty) { MessageBox.Show("Couldn't find: " + elf); return false; }

                run.Arguments = "--paid " + auth.Substring(0, 16).EndianSwapp() + " --auth-info " + auth + " " + _elfs + " " + _elfs.Replace(".elf", "fself").Replace(".prx", "fself");
                MessageBox.Show(run.Arguments);
                call.StartInfo = run;

                try { call.Start(); }
                catch (Exception io) { MessageBox.Show(io.ToString()); err = true; break; }

                call.BeginErrorReadLine();
                call.WaitForExit();
                count++;
            }

            if (err) return false;
            return true;
        }

        /// <summary>
        /// Error Event Handler for the make_fself.py and orbis-pub-cmd-ps2.exe Process.
        /// </summary>
        /// <param name="sendingProcess">The Process which triggered this Event.</param>
        /// <param name="outLine">The Received Data Event Arguments.</param>
        private static void Ps2_ErrorHandler(object sendingProcess, DataReceivedEventArgs outLine) {
            MessageBox.Show(outLine.Data);
        }

        #endregion << orbis_pub_cmd >>

        #region << Extract Needed Resources >>

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
            if (!Directory.Exists(AppCommonPath() + @"\PS2Emu\"))
            {
                Directory.CreateDirectory(AppCommonPath() + @"\PS2Emu\");
            }

            //copy byte files
            System.IO.File.WriteAllBytes(AppCommonPath() + @"\PS2Emu\" + "PS2Classics.gp4", Properties.Resources.PS2Classics);
            System.IO.File.WriteAllBytes(AppCommonPath() + @"\PS2Emu\" + "param.sfo", Properties.Resources.param);
            System.IO.File.WriteAllBytes(AppCommonPath() + "orbis-pub-cmd.exe", Properties.Resources.orbis_pub_cmd);
            System.IO.File.WriteAllBytes(AppCommonPath() + "PS2.zip", Properties.Resources.PS2);

            System.IO.File.WriteAllBytes(AppCommonPath() + "ext.zip", Properties.Resources.ext);

            //copy images for the save process
            Properties.Resources.icon0.Save(AppCommonPath() + @"\PS2Emu\" + "icon0.png");
            Properties.Resources.icon0.Save(AppCommonPath() + @"\PS2Emu\" + "pic0.png");
            Properties.Resources.icon0.Save(AppCommonPath() + @"\PS2Emu\" + "pic1.png");

            //copy text files
            System.IO.File.WriteAllText(AppCommonPath() + @"\PS2Emu\" +  "sfo.xml", Properties.Resources.sfo);

            //extarct zip
            if(Directory.Exists(AppCommonPath() + @"\PS2\"))
            {
                DeleteDirectory(AppCommonPath() + @"\PS2\");
            }
            ZipFile.ExtractToDirectory(AppCommonPath() + "PS2.zip", AppCommonPath());


            if (Directory.Exists(AppCommonPath() + @"\ext\"))
            {
                DeleteDirectory(AppCommonPath() + @"\ext\");
            }
            ZipFile.ExtractToDirectory(AppCommonPath() + "ext.zip", AppCommonPath());

            File.Delete(AppCommonPath() + "ext.zip");
            File.Delete((AppCommonPath() + "PS2.zip"));
        }


        #endregion << Extract Needed Resources >>

        /// <summary>
        /// This function will clean out a directory and then delete the directory
        /// </summary>
        /// <param name="target_dir">Supply the directory you want cleaned</param>
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

        /// <summary>
        /// This Button is used to Load an ISO into our Program (Check PS2 File Header)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnISO_Click(object sender, EventArgs e)
        {
            //Open File Dialog For ISO Files
            OpenFileDialog thedialog = new OpenFileDialog();
            thedialog.Title = "Select ISO";
            thedialog.Filter = "Image File|*.iso";
            thedialog.InitialDirectory = Environment.SpecialFolder.MyComputer.ToString();
            if (thedialog.ShowDialog() == DialogResult.OK)
            {
                //set the path and the text on the gui
                string isopath = thedialog.FileName;
                txtPath.Text = isopath;

                //now using the file stream we can read the CNF file
                using (FileStream isoStream = File.OpenRead(isopath))
                {
                    //use disk utils to read iso quickly
                    CDReader cd = new CDReader(isoStream, true);
                    //look for the spesific file
                    Stream fileStream = cd.OpenFile(@"SYSTEM.CNF", FileMode.Open);
                    // Use fileStream...
                    TextReader tr = new StreamReader(fileStream);
                    string fullstring = tr.ReadToEnd();//read string to end this will read all the info we need
                    
                    //mine for info
                    string Is = @"\";
                    string Ie = ";";
                    
                    //mine the start and end of the string
                    int start = fullstring.ToString().IndexOf(Is) + Is.Length;
                    int end = fullstring.ToString().IndexOf(Ie, start);
                    if (end > start)
                    {
                        string PS2Id = fullstring.ToString().Substring(start, end - start);

                        if (PS2Id != string.Empty)
                        {
                            PS2ID = PS2Id.Replace(".", ""); 
                            lblPS2ID.Text = "PS2 ID : " + PS2Id.Replace(".", "");
                        }
                        else
                        {
                            MessageBox.Show("Could not load PS2 ID");
                        }
                    }
                    else
                    {
                        DialogResult dlr =  MessageBox.Show("Could not load PS2 ID\n\n wpuld you like to submit an issue ?","Error Reporting",MessageBoxButtons.YesNo,MessageBoxIcon.Question,MessageBoxDefaultButton.Button2);
                        if(dlr == DialogResult.Yes)
                        {
                            //load github issue page
                            Process.Start(@"https://github.com/xXxTheDarkprogramerxXx/PS3Tools/issues");
                        }

                    }
                }
            }
        }

        private void contextMenuStrip1_Opening(object sender, CancelEventArgs e)
        {

        }

        private void restoreBackgroundToolStripMenuItem_Click(object sender, EventArgs e)
        {
            
        }

        private void txtTitleId_TextChanged(object sender, EventArgs e)
        {
            //set the name of the package on the GUI as well to show a bit of information
            lblContentName.Text = txtTitleId.Text.Trim();
        }

        #region << Progress Bar Copy Files >>

        public delegate void IntDelegate(int Int);

        public event IntDelegate FileCopyProgress;
        public void CopyFileWithProgress(string source, string destination)
        {
            var webClient = new WebClient();
            webClient.DownloadProgressChanged += DownloadProgress;
            webClient.DownloadFile(new Uri(source), destination);
        }

        private void DownloadProgress(object sender, DownloadProgressChangedEventArgs e)
        {
            if (FileCopyProgress != null)
            {
                FileCopyProgress(e.ProgressPercentage);
                progressBar1.Invoke(new Action(() => progressBar1.Value = e.ProgressPercentage));
            }

        }

        #endregion << Progress Bar Copy Files >>

        /// <summary>
        /// Check if ELFs of the base are Decrypted.
        /// </summary>
        /// <param name="path">Path to the template folder.</param>
        private bool IsElfDecrypted()
        {
            byte[] magic = new byte[4] { 0x7F, 0x45, 0x4C, 0x46, };

            foreach (string elf in elfs)
            {
                using (BinaryReader binReader = new BinaryReader(new FileStream(elf, FileMode.Open, FileAccess.Read)))
                {
                    byte[] fmagic = new byte[4];
                    binReader.Read(fmagic, 0, 4);
                    if (!magic.Contains(fmagic)) return false;
                    binReader.Close();
                }
            }
            return true;
        }


        private void UpdateString(string txt)
        {
            lblTask.Invoke(new Action(() => lblTask.Text = txt));
        }
        FolderBrowserDialog tempkeeper = null;
        /// <summary>
        /// This Button is when the user selects to convert the file to PS4 PKG
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnConvert_Click(object sender, EventArgs e)
        {
            //moving code over
            ExtractAllResources();//extarct all resources when we need it

            FolderBrowserDialog saveFileDialog1 = new FolderBrowserDialog();
            //saveFileDialog1.Filter = "PS4 PKG|*.pkg";
            //saveFileDialog1.Title = "Save an PS4 PKG File";
            //saveFileDialog1.ov
            if (DialogResult.OK != saveFileDialog1.ShowDialog())
            {
                return;
            }

            tempkeeper = saveFileDialog1;
            try
            {
                if (backgroundWorker1.IsBusy == false)
                {
                    backgroundWorker1.RunWorkerAsync();
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
           

        }


        /// <summary>
        /// Get the string of a byte converted decimal value.
        /// </summary>
        /// <param name="titleId">The TitleID decimal to convert.</param>
        /// <returns>A string, representing the convertet decimal byte value.</returns>
        private string GetDecimalBytes(string titleId)
        {
            byte[] titleIdBytes = Convert.ToDecimal(titleId).GetBytes();
            return BitConverter.ToString(titleIdBytes).Substring(0, 5).Replace("-", "");
        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                FolderBrowserDialog saveFileDialog1 = tempkeeper;

                progressBar1.Invoke(new Action(() => progressBar1.Visible = true));
                UpdateString("Creating Working Area");

                if (!Directory.Exists(AppCommonPath() + @"\Working\"))
                {
                    Directory.CreateDirectory(AppCommonPath() + @"\Working\");
                }

                UpdateString("Getting needed files");
                //first we need to build the new SFO 
                File.Copy(AppCommonPath() + @"\PS2Emu\" + "sfo.xml", AppCommonPath() + @"\Working\" + "sfo.xml", true);

                //now we need to prase it and change it 

                UpdateString("Gathering GP4 Info");
                //create new XML Document 
                xmldoc = new XmlDataDocument();
                //nodelist 
                XmlNodeList xmlnode;
                //setup the resource file to be extarcted
                string RunningPath = AppDomain.CurrentDomain.BaseDirectory;
                //load the xml file from the base directory
                xmldoc.Load(AppCommonPath() + @"\Working\" + "sfo.xml");
                //now load the nodes
                xmlnode = xmldoc.GetElementsByTagName("paramsfo");//volume is inside the xml
                                                                  //loop to get all info from the node list
                foreach (XmlNode xn in xmlnode)
                {
                    XmlNode xNode = xn.SelectSingleNode("CONTENT_ID");
                    XmlNodeList nodes = xmldoc.SelectNodes("//param[@key='CONTENT_ID']");
                    if (nodes != null)
                    {
                        xmlcontentid = "UP9000-" + txtContentID.Text.Trim() + "_00-" + PS2ID.Replace("_", "") + "0000001";
                        nodes[0].InnerText = xmlcontentid;
                    }
                    nodes = xmldoc.SelectNodes("//param[@key='TITLE']");
                    if (nodes != null)
                    {
                        nodes[0].InnerText = txtTitleId.Text.Trim();
                    }
                    nodes = xmldoc.SelectNodes("//param[@key='TITLE_ID']");
                    if (nodes != null)
                    {
                        nodes[0].InnerText = txtContentID.Text.Trim();
                    }
                    for (int i = 1; i < 7; i++)
                    {
                        //fix the enter key issue i have found in some in
                        nodes = xmldoc.SelectNodes("//param[@key='SERVICE_ID_ADDCONT_ADD_" + i + "']");
                        if (nodes != null)
                        {
                            nodes[0].InnerText = string.Empty;
                        }
                    }
                }
                //save this into the working folder
                xmldoc.Save(AppCommonPath() + @"\Working\" + "sfo.xml");


                UpdateString("Creating GP4 Project");

                SaveGp4();


                UpdateString("Creating SFO File");
                //now call orbis and create sfo
                Orbis_CMD("", "sfo_create \"" + AppCommonPath() + @"\Working\" + "sfo.xml" + "\" \"" + AppCommonPath() + @"\Working\" + "param.sfo" + "\"");

                //move SFO to main directory with locations of new images 

                UpdateString("Moving SFO File");
                File.Copy(AppCommonPath() + @"\Working\" + "param.sfo", AppCommonPath() + @"\PS2\sce_sys\param.sfo", true);
                //now move ISO

                //save images
                if (pictureBox1.Image != null)
                    pictureBox1.Image.Save(AppCommonPath() + @"PS2\sce_sys\icon0.png", System.Drawing.Imaging.ImageFormat.Png);


                if (pictureBox2.Image != null)
                    pictureBox2.Image.Save(AppCommonPath() + @"PS2\sce_sys\pic1.png", System.Drawing.Imaging.ImageFormat.Png);

                UpdateString("Moving ISO File This May Take Some Time");
                BusyCoping = true;
                new System.Threading.Thread(new System.Threading.ThreadStart(delegate
                {

                //clean up the blank file
                File.Delete(AppCommonPath() + @"\PS2\image\disc01.iso");
                //CopyFileWithProgress(txtPath.Text.Trim(), AppCommonPath() + @"\PS2\image\disc01.iso");
                File.Copy(txtPath.Text.Trim(), AppCommonPath() + @"\PS2\image\disc01.iso", true);
                    BusyCoping = false;
                })).Start();

                while (BusyCoping == true)
                {
                    Thread.Sleep(TimeSpan.FromSeconds(5));//sleep for 5 seconds
                }
                ////not using this right now
                #region << Not Using This Right Now >>
                // Set elfs path.//this is from CFWProphet THANSK BRO 
                //elfs = new string[2] {
                //    //AppCommonPath() + @"\PS2\eboot.elf",//pre patched
                //    //AppCommonPath() + @"\PS2\ps2-emu-compiler.elf",//pre patched
                //    AppCommonPath() + @"\PS2\sce_module\libSceFios2.prx",
                //    AppCommonPath() + @"\PS2\sce_module\libc.prx",
                //};

                //if (IsElfDecrypted() == false)
                //{
                //    return;
                //}

                //if (!FakeSign(GetDecimalBytes(PS2ID.Replace("_", "").Substring(4, 5))))
                //{
                //    MessageBox.Show("Error Signing Fake Selfs");
                //    return;
                //}
                #endregion << Not Using This Right Now >>
                //now create pkg 

                UpdateString("Creating PS4 PKG");
                BusyCoping = true;
                new System.Threading.Thread(new System.Threading.ThreadStart(delegate
                {
                   Orbis_CMD("", "img_create --oformat pkg \"" + AppCommonPath() + @"\PS2Emu\" + "PS2Classics.gp4\" \"" + saveFileDialog1.SelectedPath + "\"");
                //orbis_pub_cmd.exe img_create --skip_digest --oformat pkg C:\Users\3deEchelon\AppData\Roaming\Ps4Tools\PS2Emu\PS2Classics.gp4 C:\Users\3deEchelon\AppData\Roaming\Ps4Tools\PS2Emu\
                BusyCoping = false;
                })).Start();

                while (BusyCoping == true)
                {
                    Thread.Sleep(TimeSpan.FromSeconds(5));//sleep for 5 seconds
                }

                UpdateString("Done Opening Location");
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            FolderBrowserDialog saveFileDialog1 = tempkeeper;

            MessageBox.Show("Convert completed");
            Process.Start(saveFileDialog1.SelectedPath);


            //now we delete the working directory
            DeleteDirectory(AppCommonPath() + @"\Working\");
            DeleteDirectory(AppCommonPath() + @"\PS2\");
            DeleteDirectory(AppCommonPath() + @"\PS2Emu\");

        }

        private void changeBacgroToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.Title = "Select Image";
            dlg.Filter = "Image File|*.png";
            dlg.InitialDirectory = Environment.SpecialFolder.MyComputer.ToString();
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                string fileName;
                fileName = dlg.FileName;
                pictureBox2.Load(fileName);
            }
        }

        private void changeImageToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.Title = "Select Image";
            dlg.Filter = "Image File|*.png";
            dlg.InitialDirectory = Environment.SpecialFolder.MyComputer.ToString();
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                string fileName;
                fileName = dlg.FileName;
                pictureBox1.Load(fileName);
            }
        }

        private void restoreBackgroundToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            pictureBox2.Image = Properties.Resources.pic1;
        }

        private void resotreIconToolStripMenuItem_Click(object sender, EventArgs e)
        {
            pictureBox1.Image = Properties.Resources.pic0;
        }

        private void logAnIssueToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DialogResult dlr = MessageBox.Show("Would you like to submit an issue ?", "Error Reporting", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);
            if (dlr == DialogResult.Yes)
            {
                //load github issue page
                Process.Start(@"https://github.com/xXxTheDarkprogramerxXx/PS3Tools/issues");
            }
        }

        private void creditsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Credits cred = new Credits();
            cred.ShowDialog();
        }
    }

    public static class Extensionclass
    {
        public static string EndianSwapp(this string source)
        {
            string reversed = string.Empty;
            for (int i = source.Length; i > 0; i -= 2)
            {
                if (i < 2) reversed += source.Substring(i - i, 1);
                else reversed += source.Substring(i - 2, 2);
            }
            return reversed;
        }

        public static byte[] GetBytes(this decimal dec)
        {
            //Load four 32 bit integers from the Decimal.GetBits function
            Int32[] bits = decimal.GetBits(dec);

            //Create a temporary list to hold the bytes
            List<byte> bytes = new List<byte>();

            //iterate each 32 bit integer
            foreach (Int32 i in bits) bytes.AddRange(BitConverter.GetBytes(i)); //add the bytes of the current 32bit integer to the bytes list

            //return the bytes list as an array
            return bytes.ToArray();
        }

        /// <summary>
        /// Checks a Array for existens of a other Array.
        /// </summary>
        /// <typeparam name="T">The Type of the array to use and the value to add.</typeparam>
        /// <param name="source">The source Array.</param>
        /// <param name="range">The Array to check for existens.</param>
        /// <returns>True if the source Array contains the Array to check for, else false.</returns>
        public static bool Contains<T>(this T[] source, T[] range)
        {
            if (source == null) throw new FormatException("Null Refernce", new Exception("The Array to check for a value existens is not Initialized."));
            for (int i = 0; i < source.Length; i++)
            {
                if (source[i].Equals(range[i]))
                {
                    if ((source.Length - i) >= range.Length)
                    {
                        int match = 1;
                        for (int j = 1; j < range.Length; j++)
                        {
                            if (source[i + j].Equals(range[j])) match++;
                            else { i += j; break; }
                        }
                        if (match == range.Length) return true;
                    }
                    else break;
                }
            }
            return false;
        }
    }



}
    
