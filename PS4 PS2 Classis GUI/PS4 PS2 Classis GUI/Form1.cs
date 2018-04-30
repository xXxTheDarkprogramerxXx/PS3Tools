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
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Deployment.Application;
using NAudio.Wave;

namespace PS4_PS2_Classis_GUI
{
    public partial class Form1 : Form
    {
        
        public Form1()
        {
            InitializeComponent();
        }
        public static bool bgwClose;
        public static IWavePlayer waveOutDevice = new WaveOut();
        XmlDataDocument xmldoc = null;
        public string xmlcontentid { get; set; }


        //items needed
        string PS2ID;
        public static List<string> isoFiles;
        //advanced window
        Advanced advanced = new Advanced();

        //Bools
        bool BusyCoping = false;
        bool AddCustomPS2Config = false;
        string CustomConfigLocation = string.Empty;


        private void Form1_Load(object sender, EventArgs e)
        {
            //quickly write the media

            System.IO.File.WriteAllBytes(AppCommonPath() + "PS4.mp3", Properties.Resources.ps4BGM);


            if (Properties.Settings.Default.EnableBootScreen == true)
            {
                //show bootlogo the good old ps2 classic logo and sound :P
                this.Hide();

                BootLogo logo = new BootLogo();
                logo.ShowDialog();

                this.Show();
            }
            if(Properties.Settings.Default.EnableGuiMusic == true)
            {
                
                AudioFileReader audioFileReader = new AudioFileReader(AppCommonPath() + "PS4.mp3");

                waveOutDevice.Init(audioFileReader);
                waveOutDevice.Play();
            }
            Version v = Assembly.GetExecutingAssembly().GetName().Version;
            //Check to see if we are ClickOnce Deployed.
            //i.e. the executing code was installed via ClickOnce
            if (ApplicationDeployment.IsNetworkDeployed)
            {
                //Collect the ClickOnce Current Version
                v = ApplicationDeployment.CurrentDeployment.CurrentVersion;
            }
            //Show the version in a simple manner
            this.Text = string.Format("PS2 Classic GUI Version : {0}", v);

            //img button stuff
            if (Properties.Settings.Default.EnableAdvancedMode == true)
            {
                advanced.Show();
            }

            UpdateInfo("Extarcting Resources Started");
            //extract all resources for the current program
            ExtractAllResources();
            UpdateInfo("Loading Custom GP4 Project");
            //Load the GP4 after extracted
            LoadGp4();

            //quickly read sfo 
            UpdateInfo("Reading Custom SFO");
            PS2ClassicsSfo.SFO sfo = new PS2ClassicsSfo.SFO(AppCommonPath() + @"\PS2Emu\" + "param.sfo");

            UpdateInfo("Setting Content ID");
            //all we want to change is the Content ID which will rename the package 
            txtContentID.Text = sfo.ContentID.ToString().Trim().Substring(7, 9);

            //Load AuthDB we need to resign all the self files
            //LoadAuthDB();//not using this right now either
        }

        /// <summary>
        /// updates the advanced window
        /// </summary>
        /// <param name="info"></param>
        public void UpdateInfo(string info)
        {
            if(Properties.Settings.Default.EnableAdvancedMode == true)
            {
                advanced.Invoke(new Action(() => advanced.LabelText += info + Environment.NewLine));
            }
        }


        /// <summary>
        /// load Auth this method comes from CFW Prophet 
        /// Follow him on Tiwtter @cfwprophet or visit his github https://github.com/cfwprpht
        /// </summary>
        
        //public void LoadAuthDB()
        //{
        //    if (!File.Exists(AppCommonPath() + "authinfo.txt"))
        //        MessageBox.Show("Can not find authinfo.txt.");
        //    else
        //    {
        //        Fws = new List<string>();
        //        List<string[]> _Apps = new List<string[]>();
        //        List<string[]> _Auths = new List<string[]>();
        //        List<string> _apps = new List<string>();
        //        List<string> _auths = new List<string>();
        //        bool app, auth, fw;
        //        app = auth = fw = false;
        //        foreach (string line in File.ReadAllLines(AppCommonPath() + "authinfo.txt"))
        //        {
        //            if (!string.IsNullOrEmpty(line))
        //            {
        //                if (line.Contains("[FW="))
        //                {
        //                    if (Fws.Count > 0 && app) { MessageBox.Show("DataBase Inconsistent !"); break; }

        //                    string Is = @"=";
        //                    string Ie = "]";

        //                    int start = line.ToString().IndexOf(Is) + Is.Length;
        //                    int end = line.ToString().IndexOf(Ie, start);
        //                    if (end > start)
        //                        Fws.Add(line.ToString().Substring(start, end - start));
        //                    fw = true;
        //                    if (Fws.Count > 1)
        //                    {
        //                        _Apps.Add(_apps.ToArray());
        //                        _Auths.Add(_auths.ToArray());
        //                        _apps = new List<string>();
        //                        _auths = new List<string>();
        //                    }

        //                }
        //                else if (line.Contains("[Name="))
        //                {
        //                    if (!fw)
        //                    {
        //                        if (!auth) { MessageBox.Show("DataBase Inconsistent !"); break; }
        //                    }

        //                    string Is = @"=";
        //                    string Ie = "]";

        //                    int start = line.ToString().IndexOf(Is) + Is.Length;
        //                    int end = line.ToString().IndexOf(Ie, start);
        //                    if (end > start)
        //                        _apps.Add(line.ToString().Substring(start, end - start));
        //                    auth = fw = false;
        //                    app = true;
        //                }
        //                else if (line.Contains("[Auth="))
        //                {
        //                    if (!app)
        //                    {
        //                        if (fw) { MessageBox.Show("DataBase Inconsistent !"); break; }
        //                    }
        //                    string Is = @"=";
        //                    string Ie = "]";

        //                    int start = line.ToString().IndexOf(Is) + Is.Length;
        //                    int end = line.ToString().IndexOf(Ie, start);
        //                    if (end > start)
        //                        _auths.Add(line.ToString().Substring(start, end - start));
        //                    app = false;
        //                    auth = true;
        //                }
        //            }
        //        }

        //        _Apps.Add(_apps.ToArray());
        //        _Auths.Add(_auths.ToArray());

        //        Apps = _Apps.ToArray();
        //        Auths = _Auths.ToArray();
        //        //will need to clean this up a bit later
        //    }
        //}

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

            //im cheating here a bit 

            //line builder
            string tempval = @"    <file targ_path=""image/disc01.iso"" orig_path=""..\PS2\image\disc01.iso""" + @" />";
            string builder = string.Empty;

            for (int i = 0; i < Form1.isoFiles.Count; i++)
            {
                builder += tempval.Replace("disc01.iso", "disc0" + (i + 1) + ".iso") + "\n";
            }

            var alllines = File.ReadAllText(AppCommonPath() + @"\PS2Emu\" + "PS2Classics.gp4");
          
            alllines = alllines.Replace(tempval, builder.Remove(builder.Length -1,1));

            File.WriteAllText(AppCommonPath() + @"\PS2Emu\" + "PS2Classics.gp4", alllines);
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
        //private bool FakeSign(string deci)
        //{
        //    string _elfs = string.Empty;
        //    int count = 0;
        //    bool err = false;

        //    ProcessStartInfo run = new ProcessStartInfo();
        //    Process call = new Process();
        //    call.ErrorDataReceived += Ps2_ErrorHandler;
        //    run.FileName = AppCommonPath() + "make_fself.exe";
        //    run.UseShellExecute = false;
        //    run.CreateNoWindow = run.RedirectStandardError = true;

        //    foreach (string elf in Apps[2])
        //    {
        //        //need to make sure name matches 
        //        if(!elf.Contains("prx"))
        //        {

        //        }
        //        string auth = Auths[2][count];
        //        auth = deci + auth.Substring(4, auth.Length - 4);

        //        _elfs = string.Empty;
        //        foreach (string found in elfs) { if (found.Contains(elf)) _elfs = found; }
        //        if (_elfs == string.Empty) { MessageBox.Show("Couldn't find: " + elf); return false; }

        //        run.Arguments = "--paid " + auth.Substring(0, 16).EndianSwapp() + " --auth-info " + auth + " " + _elfs + " " + _elfs.Replace(".elf", "fself").Replace(".prx", "fself");
        //        MessageBox.Show(run.Arguments);
        //        call.StartInfo = run;

        //        try { call.Start(); }
        //        catch (Exception io) { MessageBox.Show(io.ToString()); err = true; break; }

        //        call.BeginErrorReadLine();
        //        call.WaitForExit();
        //        count++;
        //    }

        //    if (err) return false;
        //    return true;
        //}

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

        public static string AppCommonPath()
        {
            string returnstring = "";
            if (Properties.Settings.Default.OverwriteTemp == true && Properties.Settings.Default.TempPath != string.Empty)
            {
                returnstring = Properties.Settings.Default.TempPath + @"\Ps4Tools\";
            }
            else
            {
                returnstring = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\Ps4Tools\";
            }
            return returnstring;
        }

        public void ExtractAllResources()
        {
            UpdateInfo("Checking Directory Paths");
            if (!Directory.Exists(AppCommonPath()))
            {
                UpdateInfo("Created Directory" + AppCommonPath());
                Directory.CreateDirectory(AppCommonPath());
            }
            if (!Directory.Exists(AppCommonPath() + @"\PS2Emu\"))
            {
                UpdateInfo("Created Directory" + AppCommonPath() + @"\PS2Emu\");
                Directory.CreateDirectory(AppCommonPath() + @"\PS2Emu\");
            }

            UpdateInfo("Writing All Binary Files to Temp Path....");

            //copy byte files
            System.IO.File.WriteAllBytes(AppCommonPath() + @"\PS2Emu\" + "PS2Classics.gp4", Properties.Resources.PS2Classics);
            UpdateInfo("Writing Binary File to Temp Path " +"\n Written : "+ AppCommonPath() + @"\PS2Emu\" + "PS2Classics.gp4");
            System.IO.File.WriteAllBytes(AppCommonPath() + @"\PS2Emu\" + "param.sfo", Properties.Resources.param);
            UpdateInfo("Writing Binary File to Temp Path " + "\n Written : " + AppCommonPath() + @"\PS2Emu\" + "param.sfo");
            System.IO.File.WriteAllBytes(AppCommonPath() + "orbis-pub-cmd.exe", Properties.Resources.orbis_pub_cmd);
            UpdateInfo("Writing Binary File to Temp Path " + "\n Written : " + AppCommonPath() + "orbis-pub-cmd.exe");
            System.IO.File.WriteAllBytes(AppCommonPath() + "PS2.zip", Properties.Resources.PS2);
            UpdateInfo("Writing Binary File to Temp Path " + "\n Written : " + AppCommonPath() + "PS2.zip");
            System.IO.File.WriteAllBytes(AppCommonPath() + "ext.zip", Properties.Resources.ext);
            UpdateInfo("Writing Binary File to Temp Path " + "\n Written : " + AppCommonPath() + "ext.zip");

            UpdateInfo("Writing Image Files to Temp Path...");

            //copy images for the save process
            Properties.Resources.icon0.Save(AppCommonPath() + @"\PS2Emu\" + "icon0.png");
            UpdateInfo("Writing Image File to Temp Path " + "\n Written : " + AppCommonPath() + @"\PS2Emu\" + "icon0.png");
            Properties.Resources.icon0.Save(AppCommonPath() + @"\PS2Emu\" + "pic0.png");
            UpdateInfo("Writing Image File to Temp Path " + "\n Written : " + AppCommonPath() + @"\PS2Emu\" + "pic0.png");
            Properties.Resources.icon0.Save(AppCommonPath() + @"\PS2Emu\" + "pic1.png");
            UpdateInfo("Writing Image File to Temp Path " + "\n Written : " + AppCommonPath() + @"\PS2Emu\" + "pic1.png");

            UpdateInfo("Writing Text Files to Temp Path...");
            //copy text files
            System.IO.File.WriteAllText(AppCommonPath() + @"\PS2Emu\" +  "sfo.xml", Properties.Resources.sfo);
            UpdateInfo("Writing Image File to Temp Path " + "\n Written : " + AppCommonPath() + @"\PS2Emu\" + "sfo.xml");


            UpdateInfo("Extracting Zip(s)");
            //extarct zip
            if (Directory.Exists(AppCommonPath() + @"\PS2\"))
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
            //set defualt values
            txtPath.Enabled = true;
            lblPS2ID.Visible = true;

            UpdateInfo("Checking if there is already a mutli iso screen open and closing it");

            //close open form 
            //this closes any open form
            if (Application.OpenForms.OfType<MultipleISO_s>().Count() == 1)
                Application.OpenForms.OfType<MultipleISO_s>().First().Close();
            MultipleISO_s multi = new MultipleISO_s();
            UpdateInfo("Open File Dialog");
            //Open File Dialog For ISO Files
            OpenFileDialog thedialog = new OpenFileDialog();
            thedialog.Title = "Select ISO";
            thedialog.Filter = "Image File|*.iso";
            thedialog.Multiselect = true;
            thedialog.InitialDirectory = Environment.SpecialFolder.MyComputer.ToString();
            if (thedialog.ShowDialog() == DialogResult.OK)
            {
                UpdateInfo("adding iso files to list");
                isoFiles = thedialog.FileNames.ToList<string>();//tada now we know how many iso's there is 
                if(isoFiles.Count > 7)
                {
                    MessageBox.Show("Maximum amount of ISO's allowed by the PS4 in a classic is 7");
                    isoFiles.Clear();
                    return;
                }
                if (isoFiles.Count == 1)
                {
                    UpdateInfo("Standard ISO Method");
                    #region << For Single File >>
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

                                if (Properties.Settings.Default.EnablePS2IDReplace == true)
                                {
                                    txtContentID.Text = PS2ID.Replace("_", "");
                                }
                            }
                            else
                            {
                                MessageBox.Show("Could not load PS2 ID");
                            }
                        }
                        else
                        {
                            DialogResult dlr = MessageBox.Show("Could not load PS2 ID\n\n wpuld you like to submit an issue ?", "Error Reporting", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);
                            if (dlr == DialogResult.Yes)
                            {
                                //load github issue page
                                Process.Start(@"https://github.com/xXxTheDarkprogramerxXx/PS3Tools/issues");
                            }

                        }
                    }

                    #endregion << For Single File >>
                }
                else if (isoFiles.Count > 1)
                {
                    UpdateInfo("Multi ISO Method");
                    txtPath.Enabled = false;
                    lblPS2ID.Visible = false;
                    UpdateInfo("Opening Mutli ISO Screen");
                    multi.Show();
                    multi.SetDesktopLocation(this.Location.X + this.Size.Width, this.Location.Y);
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

        //public delegate void IntDelegate(int Int);

        //public event IntDelegate FileCopyProgress;
        //public void CopyFileWithProgress(string source, string destination)
        //{
        //    var webClient = new WebClient();
        //    webClient.DownloadProgressChanged += DownloadProgress;
        //    webClient.DownloadFile(new Uri(source), destination);
        //}

        //private void DownloadProgress(object sender, DownloadProgressChangedEventArgs e)
        //{
        //    if (FileCopyProgress != null)
        //    {
        //        FileCopyProgress(e.ProgressPercentage);
        //        progressBar1.Invoke(new Action(() => progressBar1.Value = e.ProgressPercentage));
        //    }

        //}

        #endregion << Progress Bar Copy Files >>

        /// <summary>
        /// Check if ELFs of the base are Decrypted.
        /// </summary>
        /// <param name="path">Path to the template folder.</param>
        //private bool IsElfDecrypted()
        //{
        //    byte[] magic = new byte[4] { 0x7F, 0x45, 0x4C, 0x46, };

        //    foreach (string elf in elfs)
        //    {
        //        using (BinaryReader binReader = new BinaryReader(new FileStream(elf, FileMode.Open, FileAccess.Read)))
        //        {
        //            byte[] fmagic = new byte[4];
        //            binReader.Read(fmagic, 0, 4);
        //            if (!magic.Contains(fmagic)) return false;
        //            binReader.Close();
        //        }
        //    }
        //    return true;
        //}


        private void UpdateString(string txt)
        {
            UpdateInfo(txt);
            OpenCloseWaitScreen(true);
            Busy.INFO = txt;
            Application.DoEvents();
            //lblTask.Invoke(new Action(() => lblTask.Text = txt));
        }

        FolderBrowserDialog tempkeeper = null;

        public bool doesStringMatch()
        {
            if(isoFiles.Count == 0)
            {
                return false;
            }

            string txt = string.Empty;
            if (isoFiles.Count > 1)
            {
                txt = "UP9000-" + txtContentID.Text.Trim() + "_00-" + txtContentID.Text.Trim() + "0000001";//make this the same no ps2 id required
                
            }
            else
            {
                txt = "UP9000-" + txtContentID.Text.Trim() + "_00-" + PS2ID.Replace("_", "") + "0000001";
            }

            string re1 = "((?:[a-z][a-z]*[0-9]+[a-z0-9]*))";    // Alphanum 1
            string re2 = "(-)"; // Any Single Character 1
            string re3 = "([a-z])"; // Any Single Word Character (Not Whitespace) 1
            string re4 = "([a-z])"; // Any Single Word Character (Not Whitespace) 2
            string re5 = "([a-z])"; // Any Single Word Character (Not Whitespace) 3
            string re6 = "([a-z])"; // Any Single Word Character (Not Whitespace) 4
            string re7 = "(\\d)";   // Any Single Digit 1
            string re8 = "(\\d)";   // Any Single Digit 2
            string re9 = "(\\d)";   // Any Single Digit 3
            string re10 = "(\\d)";  // Any Single Digit 4
            string re11 = "(\\d)";  // Any Single Digit 5
            string re12 = "(_)";    // Any Single Character 2
            string re13 = "(\\d+)"; // Integer Number 1
            string re14 = "(-)";    // Any Single Character 3
            string re15 = "((?:[a-z][a-z]*[0-9]+[a-z0-9]*))";   // Alphanum 2

            Regex r = new Regex(re1 + re2 + re3 + re4 + re5 + re6 + re7 + re8 + re9 + re10 + re11 + re12 + re13 + re14 + re15, RegexOptions.IgnoreCase | RegexOptions.Singleline);
            Match m = r.Match(txt);
            if (m.Success)
            {
                String alphanum1 = m.Groups[1].ToString();
                String c1 = m.Groups[2].ToString();
                String w1 = m.Groups[3].ToString();
                String w2 = m.Groups[4].ToString();
                String w3 = m.Groups[5].ToString();
                String w4 = m.Groups[6].ToString();
                String d1 = m.Groups[7].ToString();
                String d2 = m.Groups[8].ToString();
                String d3 = m.Groups[9].ToString();
                String d4 = m.Groups[10].ToString();
                String d5 = m.Groups[11].ToString();
                String c2 = m.Groups[12].ToString();
                String int1 = m.Groups[13].ToString();
                String c3 = m.Groups[14].ToString();
                String alphanum2 = m.Groups[15].ToString();
                Console.Write("(" + alphanum1.ToString() + ")" + "(" + c1.ToString() + ")" + "(" + w1.ToString() + ")" + "(" + w2.ToString() + ")" + "(" + w3.ToString() + ")" + "(" + w4.ToString() + ")" + "(" + d1.ToString() + ")" + "(" + d2.ToString() + ")" + "(" + d3.ToString() + ")" + "(" + d4.ToString() + ")" + "(" + d5.ToString() + ")" + "(" + c2.ToString() + ")" + "(" + int1.ToString() + ")" + "(" + c3.ToString() + ")" + "(" + alphanum2.ToString() + ")" + "\n");
                return true;
            }
            return false;
        }


        public bool CheckString()
        {
            if (doesStringMatch() == false)
            {
                if (DialogResult.Yes == MessageBox.Show("Content ID for this package is in the incorect format\n\nWould you like to edit this?", "Pakcage Content ID", MessageBoxButtons.YesNo, MessageBoxIcon.Error))
                {
                    //check the string
                    CheckString();
                }
                else
                {
                    return false;
                }
            }
            return true;
        }

        private void OpenCloseWaitScreen(bool open)
        {
            if (open)
            {
                //Info Screen Wait screen
                Busy.INFO = "Loading Data";
                bgwClose = false;
                if (!bgWorkerSS.IsBusy)
                {
                    bgWorkerSS.RunWorkerAsync();
                }
            }
            else
            {
                //Wait screen/ info Screen
                bgwClose = true;
                bgWorkerSS.CancelAsync();
                bgWorkerSS.Dispose();
            }
        }

        /// <summary>
        /// This Button is when the user selects to convert the file to PS4 PKG
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnConvert_Click(object sender, EventArgs e)
        {

            UpdateInfo("Converting ISO(s) to PKG ");

            CheckString();

            //moving code over
            ExtractAllResources();//extarct all resources when we need it

            UpdateInfo("Save File Dialog");

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
                OpenCloseWaitScreen(true);
                if (backgroundWorker1.IsBusy == false)
                {
                    backgroundWorker1.RunWorkerAsync();
                }
            }
            catch(Exception ex)
            {
                OpenCloseWaitScreen(false);
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


                UpdateString("Creating Working Area");
                UpdateInfo("Creating Working Area : " + AppCommonPath() + @"\Working\");
                if (!Directory.Exists(AppCommonPath() + @"\Working\"))
                {
                    Directory.CreateDirectory(AppCommonPath() + @"\Working\");
                }

                UpdateString("Getting needed files");
                UpdateInfo("Getting needed files");

                //first we need to build the new SFO
                UpdateInfo("Copying "+AppCommonPath() + @"\PS2Emu\" + "sfo.xml to " + AppCommonPath() + @"\Working\" + "sfo.xml");
                File.Copy(AppCommonPath() + @"\PS2Emu\" + "sfo.xml", AppCommonPath() + @"\Working\" + "sfo.xml", true);

                //now we need to prase it and change it 

                UpdateString("Gathering GP4 Info");
                UpdateInfo("Gathering GP4 Info");
                //create new XML Document 
                xmldoc = new XmlDataDocument();
                //nodelist 
                XmlNodeList xmlnode;
                //load the xml file from the base directory
                UpdateInfo("Loading SFO as xml");
                xmldoc.Load(AppCommonPath() + @"\Working\" + "sfo.xml");
                //now load the nodes
                xmlnode = xmldoc.GetElementsByTagName("paramsfo");//volume is inside the xml
                UpdateInfo("Update Content ID and other ifno in SFO");                                           //loop to get all info from the node list
                foreach (XmlNode xn in xmlnode)
                {
                    XmlNode xNode = xn.SelectSingleNode("CONTENT_ID");
                    XmlNodeList nodes = xmldoc.SelectNodes("//param[@key='CONTENT_ID']");
                    if (nodes != null)
                    {
                        if (isoFiles.Count > 1)
                        {
                            xmlcontentid = "UP9000-" + txtContentID.Text.Trim() + "_00-" + txtContentID.Text.Trim() + "0000001";//make this the same no ps2 id required
                            nodes[0].InnerText = xmlcontentid;
                        }
                        else
                        {
                            xmlcontentid = "UP9000-" + txtContentID.Text.Trim() + "_00-" + PS2ID.Replace("_", "") + "0000001";
                            nodes[0].InnerText = xmlcontentid;
                        }
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
                {
                    Bitmap converted = (Bitmap)pictureBox1.Image;
                    converted = ResizeImage(converted, 512, 512);//converts the image to the correct size
                    converted = ConvertTo24bpp(converted);//converts image to 24bpp
                    converted.Save(AppCommonPath() + @"PS2\sce_sys\icon0.png", System.Drawing.Imaging.ImageFormat.Png);
                }


                if (pictureBox2.Image != null)
                {
                    Bitmap converted = (Bitmap)pictureBox2.Image;
                    converted = ResizeImage(converted, 1920, 1080);//converts the image to the correct size
                    converted = ConvertTo24bpp(converted);//converts image to 24bpp
                    converted.Save(AppCommonPath() + @"PS2\sce_sys\pic1.png", System.Drawing.Imaging.ImageFormat.Png);
                }

                //add custom config
                if (AddCustomPS2Config == true)
                {
                    //move custom ps2 classics config
                    UpdateString("Copying Custom config");
                    File.Copy(CustomConfigLocation, AppCommonPath() + @"PS2\config-emu-ps4.txt", true);//overwrite the file
                }

                //now we need to check the config file
                if(isoFiles.Count > 1)
                {
                    //modify config file 
                    var textfile = File.ReadAllText(AppCommonPath() + @"PS2\config-emu-ps4.txt");
                    if (textfile.Contains("--max-disc-num="))
                    {
                        //read the nesasary info
                        string Is = @"--max-disc-num=";

                        int start = textfile.ToString().IndexOf(Is) + Is.Length;
                        int end = start + 1;//cause we know its one char more
                        if (end > start)
                        {
                            string texttoreplace =textfile.ToString().Substring(start, end - start);
                           textfile = textfile.Replace(Is+texttoreplace, @"--max-disc-num=" + isoFiles.Count);
                        }
                    }
                    File.WriteAllText(AppCommonPath() + @"PS2\config-emu-ps4.txt", textfile);
                }

                //move iso
                UpdateString("Moving ISO File This May Take Some Time");
                BusyCoping = true;
                new System.Threading.Thread(new System.Threading.ThreadStart(delegate
                {
                    for (int i = 0; i < Form1.isoFiles.Count; i++)
                    {
                        if (Form1.isoFiles.Count > 1)
                        {
                            UpdateString("Moving ISO File " + (i + 1) + "/" + Form1.isoFiles.Count + " This May Take Some Time");
                            //CopyFileWithProgress(txtPath.Text.Trim(), AppCommonPath() + @"\PS2\image\disc01.iso");
                            File.Copy(isoFiles[i].ToString().Trim(), AppCommonPath() + @"\PS2\image\disc" + String.Format("{0:D2}", i + 1) + ".iso", true);
                        }
                        else
                        {
                            //clean up the blank file
                            File.Delete(AppCommonPath() + @"\PS2\image\disc01.iso");
                            //CopyFileWithProgress(txtPath.Text.Trim(), AppCommonPath() + @"\PS2\image\disc01.iso");
                            File.Copy(txtPath.Text.Trim(), AppCommonPath() + @"\PS2\image\disc"+String.Format("{0:D2}", i+1)+ ".iso", true);

                            BusyCoping = false;
                        }
                    }

                    BusyCoping = false;
                })).Start();

                while (BusyCoping == true)
                {
                    Application.DoEvents();
                    //Thread.Sleep(TimeSpan.FromSeconds(5));//sleep for 5 seconds
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
                    Application.DoEvents();
                    //Thread.Sleep(TimeSpan.FromSeconds(5));//sleep for 5 seconds
                }

                UpdateString("Done Opening Location");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        /// <summary>
        /// Resize the image to the specified width and height.
        /// </summary>
        /// <param name="image">The image to resize.</param>
        /// <param name="width">The width to resize to.</param>
        /// <param name="height">The height to resize to.</param>
        /// <returns>The resized image.</returns>
        public static Bitmap ResizeImage(Image image, int width, int height)
        {
            var destRect = new Rectangle(0, 0, width, height);
            var destImage = new Bitmap(width, height);

            destImage.SetResolution(image.HorizontalResolution, image.VerticalResolution);

            using (var graphics = Graphics.FromImage(destImage))
            {
                graphics.CompositingMode = CompositingMode.SourceCopy;
                graphics.CompositingQuality = CompositingQuality.HighQuality;
                graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                graphics.SmoothingMode = SmoothingMode.HighQuality;
                graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;

                using (var wrapMode = new ImageAttributes())
                {
                    wrapMode.SetWrapMode(WrapMode.TileFlipXY);
                    graphics.DrawImage(image, destRect, 0, 0, image.Width, image.Height, GraphicsUnit.Pixel, wrapMode);
                }
            }

            return destImage;
        }


        /// <summary>
        /// this convers images to 24bbp
        /// </summary>
        /// <param name="img"></param>
        /// <returns></returns>
        public static Bitmap ConvertTo24bpp(Bitmap img)
        {
            var bmp = new Bitmap(img.Width, img.Height, System.Drawing.Imaging.PixelFormat.Format24bppRgb);
            using (var gr = Graphics.FromImage(bmp))
                gr.DrawImage(img, new Rectangle(0, 0, img.Width, img.Height));
            return bmp;
        }

        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
           
            FolderBrowserDialog saveFileDialog1 = tempkeeper;

            //MessageBox.Show("Convert completed");
            //no messagebox instead play the bootlogo sound 

            System.IO.Stream str = Properties.Resources.ps4_notification;
            System.Media.SoundPlayer snd = new System.Media.SoundPlayer(str);
            snd.PlaySync();
            OpenCloseWaitScreen(false);
            Process.Start(saveFileDialog1.SelectedPath);


            //now we delete the working directory
            DeleteDirectory(AppCommonPath() + @"\Working\");
            DeleteDirectory(AppCommonPath() + @"\PS2\");
            DeleteDirectory(AppCommonPath() + @"\PS2Emu\");

            //reset some vales 
            AddCustomPS2Config = false;
            CustomConfigLocation = string.Empty;
           
            UpdateInfo("Ready");
        }

        private void changeBacgroToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.Title = "Select Image";
            dlg.Filter = "Image File|*.bmp;*.jpg;*.jpeg;*.png";
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
            dlg.Filter = "Image File|*.bmp;*.jpg;*.jpeg;*.png";
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

        private void openGP4ProjectToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //open the ps2 classics file ... not really needed but hey its nice to easily just go back
        }

        private void saveGP4ProjectToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //save the ps2 classics file
        }

        private void addCustomPs2ConfigToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //open spesific file 
            //config-emu-ps4.txt
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.Title = "Select Config";
            dlg.Filter = "config-emu-ps4.txt|config-emu-ps4.txt";
            dlg.InitialDirectory = Environment.SpecialFolder.MyComputer.ToString();
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                AddCustomPS2Config = true;
                CustomConfigLocation = dlg.FileName.ToString().Trim();
                MessageBox.Show("Config will be added to this project");
            }
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void txtPath_TextChanged(object sender, EventArgs e)
        {

        }

        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            Settings set = new Settings();
            set.ShowDialog();
        }

        private void addCustomULASToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void bgWorkerSS_DoWork(object sender, DoWorkEventArgs e)
        {
            Busy Busy = new Busy(bgWorkerSS);
            Busy.ShowDialog();
            Busy.Focus();
        }

        private void toolStripMenuItem2_Click(object sender, EventArgs e)
        {
            if(waveOutDevice!= null)
            {

            }
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
    
