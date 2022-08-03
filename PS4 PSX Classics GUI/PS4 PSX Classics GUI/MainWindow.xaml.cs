using DiscUtils.Iso9660;
using Microsoft.Win32;
using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Xml;
using System.Deployment.Application;
using System.Threading;
using System.ComponentModel;
using System.Text.RegularExpressions;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Threading;
using System.Runtime.InteropServices;
using System.IO.Compression;
using System.Text;
using HtmlAgilityPack;
using System.Net;

namespace PS4_PSX_Classics_GUI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }


        #region << Vars' >>

        //NTSC-Ch
        //NTSC-U
        //NTSC-J
        //PAL_Unk
        public enum PS2Region
        {
            PAL,
            NTSC_J,
            NTSC_U,
            Unknown
        }

        public PS2Region CurrentRegion = PS2Region.Unknown;//just by default can change later


        private static byte[] pvd_buf = new byte[0x800];
        private static byte[] root_buf = new byte[0x10000];
        private static byte[] game_buf = new byte[0x10000];
        private static byte[] sig_buf = new byte[33];

        Vlc.DotNet.Forms.VlcControl tempvlc;

        private readonly BackgroundWorker backgroundWorker1 = new BackgroundWorker();
        private readonly BackgroundWorker bgWorkerSS = new BackgroundWorker();

        private readonly BackgroundWorker bgWorkerVLC = new BackgroundWorker();
        System.Windows.Forms.FolderBrowserDialog tempkeeper = null;

        public static bool bgwClose;
        public static IWavePlayer waveOutDevice = new WaveOut();
        XmlDataDocument xmldoc = null;
        public static string xmlcontentid { get; set; }


        //items needed
        string PS2ID;
        public static List<string> pspfiles = new List<string>();
        public static List<string> PS2CutomLua = new List<string>();
        public static List<string> PS2TitleId = new List<string>();

        //advanced window
        Advanced advanced = new Advanced();

        //Bools
        bool BusyCoping = false;
        bool AddCustomPS2Config = false;
        string CustomConfigLocation = string.Empty;


        #region <<File Types and Instancase >>

        FileType.FileTypes type = FileType.FileTypes.Unknown;

        #endregion << File Tyoes and Instances >>

        #endregion << Vars' >>

        #region << Events >>

        #region << XMB Events >>
        /*
            Most of these events arn't working or are not being used i got the idea from this GUI from a ps4 themse editor on wololo
        */
        private void Grid_MouseDown(object sender, MouseButtonEventArgs e)
        {
            this.Main_Grid.Focus();
        }

        private void Home_background_Drop(object sender, DragEventArgs e)
        {

        }

        private void Home_Miscellaneousicon(object sender, DragEventArgs e)
        {

        }

        private void Icon_Drop(object sender, DragEventArgs e)
        {

        }

        #endregion << XMB Events >>


        #region << Menu Items >>

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                SoundClass.PlayPS4Sound(SoundClass.Sound.PS4_Info_Pannel_Sound);
                SettingsWindow settings = new SettingsWindow();
                settings.ShowDialog();
            }
            catch (Exception ex)
            {

            }
        }

        private void MenuItem_Click_1(object sender, RoutedEventArgs e)
        {
            //CustomMessageBox("This is a test", "Result", PS4_MessageBoxButton.OK, MessageBoxImage.Error);
            PS4_MessageBoxResult dlr = CustomMessageBox("Would you like to submit an issue ?", "Error Reporting", PS4_MessageBoxButton.YesNo, MessageBoxImage.Information);
            if (dlr == PS4_MessageBoxResult.Yes)
            {
                //load github issue page
                Process.Start(@"https://github.com/xXxTheDarkprogramerxXx/PS3Tools/issues");
            }
        }

        private void MenuItem_Click_2(object sender, RoutedEventArgs e)
        {
            //open spesific file 
            //config-emu-ps4.txt
            System.Windows.Forms.OpenFileDialog dlg = new System.Windows.Forms.OpenFileDialog();
            dlg.Title = "Select Config";
            dlg.Filter = "config-emu-ps4.txt|config-emu-ps4.txt";
            dlg.InitialDirectory = Environment.SpecialFolder.MyComputer.ToString();
            if (dlg.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                AddCustomPS2Config = true;
                CustomConfigLocation = dlg.FileName.ToString().Trim();
                System.Windows.Forms.MessageBox.Show("Config will be added to this project");
            }
        }

        private void MenuItem_Click_3(object sender, RoutedEventArgs e)
        {

        }

        private void MenuItem_Click_4(object sender, RoutedEventArgs e)
        {
            var ps4credits = new MessageBox(@"A Special thanks as always to flatz for his awesome work 
Thanks to cfwprophet and VVildCard777 for dealing with all my questions and their help
Special thanks to zordon605 for PS2 Multi Iso Info
And a very special thanks to DefaultDNB for his help", "Credits", PS4_MessageBoxButton.OK, SoundClass.Sound.PS4_Info_Pannel_Sound);
            ps4credits.ShowDialog();
        }

        #endregion << Menu Items >>

        private void Process_ErrorDataReceived(object sender, DataReceivedEventArgs e)
        {
            if (e.Data.ToString().Trim() == "")
            {
                System.Windows.Forms.MessageBox.Show(e.Data.ToString());
            }
        }


        /// <summary>
        /// This Should Be Used to make the key down of the xmb happen swap icons ext ext
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Page_PreviewKeyDown(object sender, KeyEventArgs e)
        {

            //Key key = e.Key;
            //switch (key)
            //{
            //    case Key.Left:
            //        if (this.menunumber == 1)
            //        {
            //            this.selectbox--;
            //            if (this.selectbox < 0)
            //            {
            //                this.selectbox = 0;
            //            }
            //            this.Select(this.selectbox);
            //            return;
            //        }
            //        break;
            //    case Key.Up:
            //        if (this.menunumber == 1)
            //        {
            //            this.homescreen.Visibility = Visibility.Hidden;
            //            this.functionscreen.Visibility = Visibility.Visible;
            //            this.Function_icon_load();
            //            this.menunumber--;
            //            return;
            //        }
            //        if (this.menunumber == 2)
            //        {
            //            this.Miscellaneousscreen.Visibility = Visibility.Hidden;
            //            this.homescreen.Visibility = Visibility.Visible;
            //            this.menunumber--;
            //            return;
            //        }
            //        break;
            //    case Key.Right:
            //        if (this.menunumber == 1)
            //        {
            //            this.selectbox++;
            //            if (this.selectbox > 11)
            //            {
            //                this.selectbox = 11;
            //            }
            //            this.Select(this.selectbox);
            //            return;
            //        }
            //        break;
            //    case Key.Down:
            //        if (this.menunumber == 0)
            //        {
            //            this.functionscreen.Visibility = Visibility.Hidden;
            //            this.homescreen.Visibility = Visibility.Visible;
            //            this.Home_Miscellaneousicon_load();
            //            this.menunumber++;
            //            return;
            //        }
            //        if (this.menunumber == 1)
            //        {
            //            this.homescreen.Visibility = Visibility.Hidden;
            //            this.Miscellaneousscreen.Visibility = Visibility.Visible;
            //            this.Functionarea_load();
            //            this.menunumber++;
            //        }
            //        break;
            //    default:
            //        {
            //            int arg_72_0 = key - Key.LeftShift;
            //            return;
            //        }

        }


        /// <summary>
        /// Converts Byte Array to Image
        /// </summary>
        /// <param name="array"></param>
        /// <returns></returns>
        public BitmapImage ToImage(byte[] array)
        {
            using (var ms = new System.IO.MemoryStream(array))
            {
                var image = new BitmapImage();
                image.BeginInit();
                image.CacheOption = BitmapCacheOption.OnLoad; // here
                image.StreamSource = ms;
                image.EndInit();
                return image;
            }
        }

        public static byte[] ReadFully(Stream input)
        {
            byte[] buffer = new byte[16 * 1024];
            using (MemoryStream ms = new MemoryStream())
            {
                int read;
                while ((read = input.Read(buffer, 0, buffer.Length)) > 0)
                {
                    ms.Write(buffer, 0, read);
                }
                return ms.ToArray();
            }
        }
        static public List<int> SearchBytePattern(byte[] pattern, byte[] bytes)
        {
            List<int> positions = new List<int>();
            int patternLength = pattern.Length;
            int totalLength = bytes.Length;
            byte firstMatchByte = pattern[0];
            for (int i = 0; i < totalLength; i++)
            {
                if (firstMatchByte == bytes[i] && totalLength - i >= patternLength)
                {
                    byte[] match = new byte[patternLength];
                    Array.Copy(bytes, i, match, 0, patternLength);
                    if (match.SequenceEqual<byte>(pattern))
                    {
                        positions.Add(i);
                        i += patternLength - 1;
                    }
                }
            }
            return positions;
        }

        string getPSXId(string file)
        {
            FileStream fin;
            try
            {
                fin = new FileStream(file, FileMode.Open);
                fin.Position = (32768);
                byte[] buffer = new byte[4096];

                while (fin.Read(buffer, 0, buffer.Length) != -1)
                {
                    String buffered = System.Text.Encoding.UTF8.GetString(buffer);

                    if (buffered.Contains("BOOT = cdrom:\\"))
                    {
                        String tmp = "";
                        int lidx = buffered.LastIndexOf("BOOT = cdrom:\\") + 14;
                        for (int i = 0; i < 11; i++)
                        {
                            tmp += buffered[(lidx + i)];
                        }
                        fin.Close();
                        return tmp;
                    }

                }
                fin.Close();
            }
            
            catch (Exception e)
            {
                // TODO Auto-generated catch block
            }

            return null;

        }

        string findGameName(string file)
        {
            // 
            FileStream fin;
            try
            {
                fin = new FileStream(file, FileMode.Open);
                fin.Position = (32768);
                byte[] buffer = new byte[4096];

                while (fin.Read(buffer, 0, buffer.Length) != -1)
                {
                    String buffered = System.Text.Encoding.UTF8.GetString(buffer);

                    if (buffered.Contains("THERE IS NO"))
                    {
                        String tmp = "";
                        int lidx = buffered.LastIndexOf("THERE IS NO");
                        for (int i = 0; i < buffered.LastIndexOf("DATA ON THIS MEMORY CARD"); i++)
                        {
                            tmp += buffered[(lidx + i)];
                        }
                        fin.Close();
                        return tmp;
                    }

                }
                fin.Close();
            }

            catch (Exception e)
            {
                // TODO Auto-generated catch block
            }

            return null;
        }

        private bool RemoteFileExists(string url)
        {
            try
            {
                //Creating the HttpWebRequest
                HttpWebRequest request = WebRequest.Create(url) as HttpWebRequest;
                //Setting the Request method HEAD, you can also use GET too.
                request.Method = "HEAD";
                //Getting the Web Response.
                HttpWebResponse response = request.GetResponse() as HttpWebResponse;
                //Returns TRUE if the Status code == 200
                response.Close();
                return (response.StatusCode == HttpStatusCode.OK);
            }
            catch
            {
                //Any exception will returns false.
                return false;
            }
        }

        /// <summary>
        /// This Loads the PSP Iso Info 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSelectISO_Click(object sender, RoutedEventArgs e)
        {
            //set defualt values
            txtPath.IsEnabled = true;
            lblPSXID.Visibility = Visibility.Visible;
            Icon0.Visibility = Visibility.Visible;
            try
            {
                if (Util.SoundClass.atr3vlc.IsPlaying)
                {
                    Util.SoundClass.atr3vlc.Stop();
                }
                if (tempvlc.IsPlaying)
                {
                    tempvlc.Stop();
                }
            }
            catch
            {

            }
            if (Properties.Settings.Default.EnablePMF == false)
            {
                WindowsFormsHost.Visibility = Visibility.Collapsed;
            }
            UpdateInfo("Checking if there is already a mutli iso screen open and closing it");

            //close open form 
            //this closes any open form
            if (Application.Current.Windows.OfType<MultipleISO_s>().Count() == 1)
                Application.Current.Windows.OfType<MultipleISO_s>().First().Close();
            MultipleISO_s multi = new MultipleISO_s();
            //UpdateInfo("Open File Dialog");
            //Open File Dialog For ISO Files
            OpenFileDialog thedialog = new OpenFileDialog();
            thedialog.Title = "Select PSX Game";
            thedialog.Filter = "PSX Image File (*.cue,*.bin,*.BIN,*.CUE)|*.bin;*.cue;*.BIN;*.CUE;";
            //"Plain text files (*.csv;*.txt)|*.csv;*.txt";
            thedialog.Multiselect = false;//psp emu only supports 1.8Gig so we might as well only allow one iso/pbp/cso file
            thedialog.InitialDirectory = Environment.SpecialFolder.MyComputer.ToString();

            if (thedialog.ShowDialog() == true)
            {
                PS2_Tools.BinCue.CueFile file = new PS2_Tools.BinCue.CueFile(thedialog.FileName);
                //load some info from the binfile 
                if (!File.Exists(file.BinFileName))
                {

                }
                string PS2Id = getPSXId(file.BinFileName);
                if (PS2Id != string.Empty)
                {
                    PS2ID = PS2Id.Replace(".", "");
                    lblPSXID.Content = "PSX ID : " + PS2Id.Replace(".", "");


                    txtContentID.Text = PS2ID.Replace("_", "");
                    string url = "https://psxdatacenter.com/jlist.html";//japan
                    switch(PS2ID.Replace("_", "").Substring(2,1))
                    {
                        case "U":
                            CurrentRegion = PS2Region.NTSC_U;
                            url = "https://psxdatacenter.com/ulist.html";//us
                            break;
                        case "E":
                            CurrentRegion = PS2Region.PAL;
                            url = "https://psxdatacenter.com/plist.html";//pal
                            break;
                        default:
                            CurrentRegion = PS2Region.NTSC_J;
                            break;
                    }


                    var web = new HtmlWeb();
                    var doc = web.Load(url);
                    var item = doc.DocumentNode.SelectNodes("//*[contains(., '"+ PS2ID.Replace("_","-") + "')]");
                    for (int i = 0; i < item.Count; i++)
                    {
                        if(item[i].Name == "tr")
                        {
                            //select <td> having child node <a>
                            var td1 = item[i].SelectSingleNode("./td[a]"); //or using index: ./td[1]
                            var link = td1.FirstChild.Attributes["href"].Value;
                            var title = td1.InnerText;
                            var name = item[i].SelectSingleNode(".//td[@class='col3']/text()").InnerText.Trim().Replace("&nbsp;","");
                            //select <td> not having child node <a>
                            var publisher = item[i].SelectSingleNode("./td[not(a)]") //using index: ./td[2]
                                                .InnerText;

                            txtTitle.Text = name;

                            string urlimg = "http://psxdatacenter.com/" + link.Replace(@"games/", "images/covers/").Replace(".html",".jpg");
                            urlimg = RemoteFileExists(urlimg) == true ? urlimg : "http://psxdatacenter.com/" + link.Replace(@"games/", "images/covers/").Replace(".html", ".png");

                            BitmapImage image = new BitmapImage();
                            image.BeginInit();
                            image.UriSource = new Uri(urlimg);
                            image.EndInit();
                            Icon0.Source = image;
                            Icon0.Stretch = Stretch.Fill;
                            break;
                        }
                    }
                }
                else
                {
                    System.Windows.MessageBox.Show("Could not load PSX ID");
                }






                type = FileType.FileTypes.ISO;




                txtPath.Text = thedialog.FileName;





            }
        }


        /// <summary>
        /// This method makes it possible to change the text for the pkg
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtContentID_Copy_TextChanged(object sender, TextChangedEventArgs e)
        {
            selecttitle.Content = txtTitle.Text.Trim();
        }

        /// <summary>
        /// Load Method
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {


                #region << Quick Sound/Video Extract >>

                //we no longer want to play the ps4 music to give the system more of the ps classic feel
                //System.IO.File.WriteAllBytes(AppCommonPath() + "PS4.mp3", Properties.Resources.ps4BGM);
                System.IO.File.WriteAllBytes(AppCommonPath() + "PSX.mp4", Properties.Resources.psonelogo);

                System.IO.File.WriteAllBytes(AppCommonPath() + "PSX_Emu.zip", Properties.Resources.EMU);

                if (!Directory.Exists(AppCommonPath() + @"\PSXEmu\"))
                {
                    UpdateInfo("Created Directory" + AppCommonPath() + @"\PSXEmu\");
                    Directory.CreateDirectory(AppCommonPath() + @"\PSXEmu\");
                }
                System.IO.File.WriteAllBytes(AppCommonPath() + @"\PSXEmu\" + "param.sfo", Properties.Resources.param);

                if (!Directory.Exists(AppCommonPath() + @"\PSX_Emu\"))
                {
                    UpdateInfo("Created Directory" + AppCommonPath() + @"\PSX_Emu\");
                    ZipFile.ExtractToDirectory(AppCommonPath() + "PSX_Emu.zip", AppCommonPath() + @"\PSX_Emu\");
                }
                File.Delete(AppCommonPath() + "PSX_Emu.zip");

                #endregion <<Quick Sound/Video Extract >>


                #region << Background Workers >>

                bgWorkerSS.DoWork += bgWorkerSS_DoWork;
                bgWorkerSS.WorkerSupportsCancellation = true;
                bgWorkerVLC.DoWork += BgWorkerVLC_DoWork;


                backgroundWorker1.DoWork += BackgroundWorker1_DoWork;
                backgroundWorker1.RunWorkerCompleted += backgroundWorker1_RunWorkerCompleted;
                bgWorkerVLC.RunWorkerCompleted += BgWorkerVLC_RunWorkerCompleted;

                #endregion << Background Workers >>

                #region << Boot Screen Settings >>

                if (Properties.Settings.Default.EnableBootScreen == true)
                {
                    //show bootlogo the good old ps2 classic logo and sound :P
                    this.Hide();

                    VideoScreen PSPlogo = new VideoScreen();
                    PSPlogo.ShowDialog();

                    this.Show();
                }

                #endregion << Boot Screen Settings >>

                #region << Gui Music >>

                ////no longer using uncomment if you want it back
                //if (Properties.Settings.Default.EnableGuiMusic == true)
                //{
                //    btnMutePlaySound.Background = ImageBrushFromBitmap(Properties.Resources.icon_sound_mute);
                //    SoundClass.PlayPS4Sound(SoundClass.Sound.PS4_Music);
                //}

                #endregion << Gui Music >>

                #region << Version Numbering >>

                Version v = Assembly.GetExecutingAssembly().GetName().Version;
                //Check to see if we are ClickOnce Deployed.
                //i.e. the executing code was installed via ClickOnce
                if (ApplicationDeployment.IsNetworkDeployed)
                {
                    //Collect the ClickOnce Current Version
                    v = ApplicationDeployment.CurrentDeployment.CurrentVersion;
                }

                //Show the version in a simple manner
                this.Title = string.Format("PSX Classics GUI Version : {0}", v);


                #endregion << Version Numbering >>

                #region << Advanced Window >>

                // i removed this as its no longer needed
                if (Properties.Settings.Default.EnableAdvancedMode == true)
                {
                    advanced.Show();
                }


                #endregion << Advanced Window >>

                #region << Begin Maintaining the form and getting everything ready >>

                //quickly read sfo 
                UpdateInfo("Reading Custom SFO");
                PS2ClassicsSfo.SFO sfo = new PS2ClassicsSfo.SFO(AppCommonPath() + @"\PSXEmu\" + "param.sfo");

                UpdateInfo("Setting Content ID");
                //all we want to change is the Content ID which will rename the package 
                txtContentID.Text = sfo.ContentID.ToString().Trim().Substring(7, 9);


                #endregion << Begin Maintaining the form and getting everything ready >>

                #region << PMF and AT3 >>
                bgWorkerVLC.RunWorkerAsync();
                #endregion << PMF and AT3 >>
            }
            catch (Exception ex)
            {

                System.Windows.Forms.MessageBox.Show(ex.Message);
            }
        }

        private void BgWorkerVLC_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {

        }

        private void BgWorkerVLC_DoWork(object sender, DoWorkEventArgs e)
        {
            #region << PMF and AT3 >>

            if (Directory.Exists(Path.Combine(AppCommonPath(), "libvlc", IntPtr.Size == 4 ? "win-x86" : "win-x64")))
            {
                var vlcLibDirectory = new DirectoryInfo(Path.Combine(AppCommonPath(), "libvlc", IntPtr.Size == 4 ? "win-x86" : "win-x64"));

                //for testing do this 
                //var vlcLibDirectory = new DirectoryInfo(AppCommonPath());
                System.Windows.Application.Current.Dispatcher.Invoke(
    DispatcherPriority.Normal,
    (ThreadStart)delegate
    {

        tempvlc = new Vlc.DotNet.Forms.VlcControl();
        tempvlc.BeginInit();
        tempvlc.VlcLibDirectory = vlcLibDirectory;// /*libvlc's directory*/;
                                                  //this.MyControl.VlcMediaplayerOptions = new[] { "-vv" };

        string[] options =
        new string[] { "input-repeat=3" };
        var mediaOptions = new string[] { "input-repeat=-1" };
        tempvlc.VlcMediaplayerOptions = options;
        this.WindowsFormsHost.Child = tempvlc;
        this.WindowsFormsHost.Visibility = Visibility.Collapsed;


        //this.WindowsFormsHost.Child = tempvlc;

        tempvlc.EndInit();

        Util.SoundClass.atr3vlc = new Vlc.DotNet.Forms.VlcControl();
        Util.SoundClass.atr3vlc.BeginInit();
        Util.SoundClass.atr3vlc.VlcLibDirectory = vlcLibDirectory;// /*libvlc's directory*/;
                                                                  //this.MyControl.VlcMediaplayerOptions = new[] { "-vv" };
        Util.SoundClass.atr3vlc.EndInit();

    });

            }
            #endregion << PMF and AT3 >>
        }

        /// <summary>
        /// Background Worker For Progres Bar Screen
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void bgWorkerSS_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                Application.Current.Dispatcher.Invoke((Action)delegate
                {
                    // your code

                    Busy Busy = new Busy(bgWorkerSS);
                    Busy.ShowDialog();
                    Busy.Focus();
                });
            }
            catch (Exception ex)
            {

            }
        }

        /// <summary>
        /// Heavy Lifter Bacground Worker this Worker Does Everything
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BackgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {

                System.Windows.Forms.FolderBrowserDialog saveFileDialog1 = tempkeeper;


                UpdateString("Creating Working Area");
                UpdateInfo("Creating Working Area : " + AppCommonPath() + @"\Working\");
                if (!Directory.Exists(AppCommonPath() + @"\Working\"))
                {
                    Directory.CreateDirectory(AppCommonPath() + @"\Working\");
                }

                UpdateString("Getting needed files");
                UpdateInfo("Getting needed files");

                //first we need to build the new SFO
                UpdateInfo("Copying " + AppCommonPath() + @"\PSXEmu\" + "sfo.xml to " + AppCommonPath() + @"\Working\" + "sfo.xml");
                File.Copy(AppCommonPath() + @"\PSXEmu\" + "sfo.xml", AppCommonPath() + @"\Working\" + "sfo.xml", true);

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

                        if (pspfiles.Count > 1)
                        {
                            System.Windows.Application.Current.Dispatcher.Invoke(
                       DispatcherPriority.Normal,
                       (ThreadStart)delegate
                       {
                           xmlcontentid = "UP9000-" + txtContentID.Text.Trim() + "_00-" + txtContentID.Text.Trim() + "0000001";//make this the same no ps2 id required
                       });
                            nodes[0].InnerText = xmlcontentid;
                        }
                        else
                        {
                            System.Windows.Application.Current.Dispatcher.Invoke(
                        DispatcherPriority.Normal,
                        (ThreadStart)delegate
                        {
                            xmlcontentid = "UP9000-" + txtContentID.Text.Trim() + "_00-" + PS2ID.Replace("_", "") + "0000001";
                        });
                            nodes[0].InnerText = xmlcontentid;
                        }
                        //});
                    }
                    nodes = xmldoc.SelectNodes("//param[@key='TITLE']");
                    if (nodes != null)
                    {
                        System.Windows.Application.Current.Dispatcher.Invoke(
                        DispatcherPriority.Normal,
                        (ThreadStart)delegate
                        {
                            nodes[0].InnerText = txtTitle.Text.Trim();
                        });
                    }
                    nodes = xmldoc.SelectNodes("//param[@key='TITLE_ID']");
                    if (nodes != null)
                    {
                        System.Windows.Application.Current.Dispatcher.Invoke(
                        DispatcherPriority.Normal,
                        (ThreadStart)delegate
                        {
                            nodes[0].InnerText = txtContentID.Text.Trim();
                        });
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

                #region << Orbis >>

                //we will just modify the orginal file its way way easier
                //also no need for SCE tools then
                //now call orbis and create sfo
                //Orbis_CMD("", "sfo_create \"" + AppCommonPath() + @"\Working\" + "sfo.xml" + "\" \"" + AppCommonPath() + @"\Working\" + "param.sfo" + "\"");

                //move SFO to main directory with locations of new images 

                //UpdateString("Moving SFO File");
                //File.Copy(AppCommonPath() + @"\Working\" + "param.sfo", AppCommonPath() + @"\PSP\sce_sys\param.sfo", true);


                #endregion << Orbis >>

                #region << PSFO Editor>>

                //Create the ContentID UP9000-

                System.Windows.Application.Current.Dispatcher.Invoke(DispatcherPriority.Normal, (ThreadStart)delegate
                {
                    xmlcontentid = "UP9000-" + txtContentID.Text.Trim() + "_00-" + PS2ID.Replace("_", "") + "0000001";
                });

                //Load The SFO From the existing File Location
                Param_SFO.PARAM_SFO psfo = new Param_SFO.PARAM_SFO(AppCommonPath() + @"\PSP\sce_sys\param.sfo");

                //Set Item Info
                for (int i = 0; i < psfo.Tables.Count; i++)
                {
                    if (psfo.Tables[i].Name == "CONTENT_ID")
                    {
                        var tempitem = psfo.Tables[i];
                        tempitem.Value = xmlcontentid.Trim();
                        psfo.Tables[i] = tempitem;
                    }
                    if (psfo.Tables[i].Name == "TITLE")
                    {
                        string title = "";
                        System.Windows.Application.Current.Dispatcher.Invoke(
                       DispatcherPriority.Normal,
                       (ThreadStart)delegate
                       {
                           title = txtTitle.Text.Trim();
                       });
                        var tempitem = psfo.Tables[i];
                        tempitem.Value = title.Trim();
                        psfo.Tables[i] = tempitem;
                    }

                    if (psfo.Tables[i].Name == "TITLE_ID")
                    {
                        string title = "";
                        System.Windows.Application.Current.Dispatcher.Invoke(
                       DispatcherPriority.Normal,
                       (ThreadStart)delegate
                       {
                           title = txtContentID.Text.Trim();
                       });
                        var tempitem = psfo.Tables[i];
                        tempitem.Value = title.Trim();
                        psfo.Tables[i] = tempitem;
                    }
                }

                //Save SFO
                psfo.SaveSFO(psfo, AppCommonPath() + @"\PSP\sce_sys\param.sfo");//save the sfo over the existing one

                #endregion << PSFO Editor>>

                #region << Save Images>>

                System.Windows.Application.Current.Dispatcher.Invoke(
                        DispatcherPriority.Normal,
                        (ThreadStart)delegate
                        {
                            BitmapSource sr = Icon0.Source as BitmapSource;
                            Bitmap converted = GetBitmap(sr);
                            converted = ResizeImage(converted, 512, 512);//converts the image to the correct size
                            converted = ConvertTo24bpp(converted);//converts image to 24bpp
                            converted.Save(AppCommonPath() + @"PSP\sce_sys\icon0.png", System.Drawing.Imaging.ImageFormat.Png);
                        });

                System.Windows.Application.Current.Dispatcher.Invoke(
                        DispatcherPriority.Normal,
                        (ThreadStart)delegate
                        {
                            //ImageBrush b = ;
                            ImageBrush b = (ImageBrush)BackgroundImage.Background;
                            BitmapSource src = (BitmapSource)b.ImageSource;

                            Bitmap converted = GetBitmap(src);
                            converted = ResizeImage(converted, 1920, 1080);//converts the image to the correct size
                            converted = ConvertTo24bpp(converted);//converts image to 24bpp
                            converted.Save(AppCommonPath() + @"PSP\sce_sys\pic1.png", System.Drawing.Imaging.ImageFormat.Png);
                        });


                #endregion << Save Images>>

                #region << Custom PSP Confog >>

                UpdateString("Moving Custom PSP Config");
                //now we need to check the config file
                if (pspfiles.Count > 1)
                {
                    //modify config file 
                    var textfile = File.ReadAllText(AppCommonPath() + @"PSP\config-emu-ps4.txt");
                    if (textfile.Contains("--max-disc-num="))
                    {
                        //read the nesasary info
                        string Is = @"--max-disc-num=";

                        int start = textfile.ToString().IndexOf(Is) + Is.Length;
                        int end = start + 1;//cause we know its one char more
                        if (end > start)
                        {
                            string texttoreplace = textfile.ToString().Substring(start, end - start);
                            textfile = textfile.Replace(Is + texttoreplace, @"--max-disc-num=" + pspfiles.Count);
                        }
                    }

                    textfile = textfile.Replace(@"#--path-patches=""/app0/patches""", @"--path-patches=""/app0/patches""");//add patches
                    textfile = textfile.Replace(@"#--path-featuredata=""/app0/patches""", @"--path-featuredata=""/app0/patches""");//add featuredata
                    textfile = textfile.Replace(@"#--path-toolingscript=""/app0/patches""", @"--path-toolingscript=""/app0/patches""");//#--path-toolingscript=""/app0/patches"""
                    File.WriteAllText(AppCommonPath() + @"PSP\config-emu-ps4.txt", textfile);
                }

                #endregion << Custom PSP Confog >>

                #region << Enable Mysis Patches >>

                //these patches where orginally done by Mysis ... a c# version was made by Pink1
                //if (Properties.Settings.Default.EnableMysisPatch == true)
                //{
                //    if(type == FileType.FileTypes.PBP)
                //    {
                //        pbp.WritePBPFiles()
                //    }
                //}
                //this will be added after initial release and when ppspp is converted correctly
                #endregion << Enable Mysis Patches >>

                #region << Move ISO >>

                UpdateString("Moving ISO File This May Take Some Time");
                BusyCoping = true;
                new System.Threading.Thread(new System.Threading.ThreadStart(delegate
                {

                    if (MainWindow.pspfiles.Count > 1)

                        for (int i = 0; i < MainWindow.pspfiles.Count; i++)
                        {
                            {

                                UpdateString("Moving ISO File " + (i + 1) + "/" + MainWindow.pspfiles.Count + " This May Take Some Time");
                                File.Copy(pspfiles[i].ToString().Trim(), AppCommonPath() + @"\PS2\image\disc" + String.Format("{0:D2}", i + 1) + ".iso", true);

                            }
                        }
                    else
                    {
                        //we need to handle iso creation from PBP


                        if (type == FileType.FileTypes.ISO)
                        {
                            //clean up the blank file
                            File.Delete(AppCommonPath() + @"\PSP\DATA\USER_L0.IMG");
                            string currentimage = "";
                            currentimage = pspfiles[0].ToString().Trim();//first and only item
                            File.Copy(currentimage, AppCommonPath() + @"\PSP\DATA\USER_L0.IMG", true);

                            //BusyCoping = false;
                        }
                    }

                    //check if file is encrypted inside the system 
                    //if it is decrypt it
                    bool encrypted = false;

                    byte[] EBOOT;
                    byte[] EBOOTDec;
                    using (FileStream isoStream = File.OpenRead(AppCommonPath() + @"\PSP\DATA\USER_L0.IMG"))
                    {
                        UpdateString("Eboot is encrypted\nDecrypting....");


                        //use disk utils to read iso quickly
                        CDReader cd = new CDReader(isoStream, true);
                        //look for the spesific file
                        Stream fileStream = cd.OpenFile("\\PSP_GAME\\SYSDIR\\EBOOT.BIN", FileMode.Open);
                        // Use fileStream...
                        EBOOT = new byte[fileStream.Length];
                        EBOOTDec = new byte[fileStream.Length];
                        fileStream.Read(EBOOT, 0, (int)fileStream.Length);
                        //File.WriteAllBytes(AppCommonPath() + @"\eboot.bin", buffer);
                        fileStream.Position = 0;
                        using (BinaryReader binaryReader = new BinaryReader(fileStream))
                        {
                            Byte[] FileHeader = binaryReader.ReadBytes(4);

                            if (FileHeader.SequenceEqual(new byte[4] { 0x7E, 0x50, 0x53, 0x50 }))
                            {
                                //file is encrypted decrypt it
                                encrypted = true;

                                //new uses C# no more writing disgusting files




                            }
                        }

                        //always clean up
                        //File.Delete(AppCommonPath() + @"\eboot.bin");
                    }
                    if (encrypted == true)
                    {
                        UpdateString("Extracting disc and recreating....");
                        string Label = "";




                        using (FileStream ISOStream = File.Open(AppCommonPath() + @"\PSP\DATA\USER_L0.IMG", FileMode.Open))
                        {
                            CDReader Reader = new CDReader(ISOStream, true, true);
                            //BinarryTools.ExtractDirectory(Reader.Root, AppCommonPath() + "\\" + Path.GetFileNameWithoutExtension(AppCommonPath() + @"\PSP\DATA\USER_L0.IMG") + "\\", "");


                            Label = Reader.VolumeLabel;

                            Reader.Dispose();
                        }

                        long num = EBOOT.Length;
                        if (num > 512320L)
                        {
                            num = 512320L;
                        }


                        byte[] query = (byte[])BinarryTools.ReadRomData(EBOOT, 0L, (int)num);


                        var offset = BinarryTools.FindOffset(AppCommonPath() + @"\PSP\data\USER_L0.IMG", query);
                        byte[] value = (byte[])BinarryTools.ReadRomData(EBOOTDec, 0L, (int)EBOOT.Length);


                        //now replace
                        FileStream fileStream = new FileStream(AppCommonPath() + @"\PSP\data\USER_L0.IMG", FileMode.Open, FileAccess.Write, FileShare.Write);
                        fileStream.Seek(Convert.ToInt64(offset), SeekOrigin.Begin);
                        fileStream.Write(value, 0, value.Length);
                        fileStream.Close();
                        //File.Delete(AppCommonPath() + @"\eboot.bin");
                        //File.Delete(AppCommonPath() + @"\eboot.bin.dec");//delete and cleanup
                        //now replace file 
                        //Replace BOOT.BIN with Eboot.BIn
                        //File.Copy(AppCommonPath() + @"\eboot.bin.dec", AppCommonPath() + "\\" + Path.GetFileNameWithoutExtension(AppCommonPath() + @"\PSP\DATA\USER_L0.IMG") + @"\PSP_GAME\SYSDIR\EBOOT.BIN", true);
                        //File.Copy(AppCommonPath() + @"\eboot.bin.dec", AppCommonPath() + "\\" + Path.GetFileNameWithoutExtension(AppCommonPath() + @"\PSP\DATA\USER_L0.IMG") + @"\PSP_GAME\SYSDIR\BOOT.BIN", true);
                        //File.Delete(AppCommonPath() + @"\eboot.bin.dec");//delete and cleanup
                        //re-create the iso
                        //BinarryTools.CreateIsoImage(AppCommonPath() + "\\" + Path.GetFileNameWithoutExtension(AppCommonPath() + @"\PSP\DATA\USER_L0.IMG"), AppCommonPath() + @"\PSP\DATA\USER_L0.IMG", Label);



                        //PSP_Tools.UMD.ISO iso = new PSP_Tools.UMD.ISO();
                        //iso.PSPTitle = Label ?? psfo.Title;

                        //iso.CreateISO(AppCommonPath() + "\\" + Path.GetFileNameWithoutExtension(AppCommonPath() + @"\PSP\DATA\USER_L0.IMG"), AppCommonPath() + @"\PSP\DATA\USER_L0.IMG");


                        //PSP_Tools.UMD.Sign.UMDSIGN()



                        //while (iso.Status == PSP_Tools.UMD.ISO.ISOStatus.Busy)
                        //{
                        //    //sleep the thread
                        //    DoEvents();
                        //}
                        //if (iso.Status == PSP_Tools.UMD.ISO.ISOStatus.Completed)
                        {
                            //DeleteDirectory(AppCommonPath() + "\\" + Path.GetFileNameWithoutExtension(AppCommonPath() + @"\PSP\DATA\USER_L0.IMG"));
                        }



                    }


                    BusyCoping = false;
                })).Start();

                while (BusyCoping == true)
                {
                    DoEvents();
                }
                #endregion << Move ISO >>

                #region << Patch NP Title File >>
                if (File.Exists(AppCommonPath() + @"\PSP\sce_sys\nptitle.dat"))
                {
                    //updatenptitledata
                    UpdateString("Patching NP Title");
                    // Original Byte string to find and Replace "43 55 53 41 30 35 32 38 39 5F 30 30"
                    Stream FileStream = new FileStream(AppCommonPath() + @"\PSP\sce_sys\nptitle.dat", FileMode.Open, FileAccess.Read, FileShare.Read);

                    //Read NPTitle ID
                    FileStream.Seek(16, SeekOrigin.Begin);
                    byte[] array = new byte[9];
                    FileStream.Read(array, 0, array.Length);
                    //Close the stream
                    FileStream.Close();

                    //get the current stream value
                    var currentstr = Encoding.ASCII.GetString(array);
                    string contentid = currentstr;
                    System.Windows.Application.Current.Dispatcher.Invoke(
                            DispatcherPriority.Normal,
                            (ThreadStart)delegate
                            {
                                contentid = txtContentID.Text;
                            });
                    //encode to bytes the new content id
                    var bytes = Encoding.ASCII.GetBytes(contentid);

                    //read current bytes from file
                    byte[] file = File.ReadAllBytes(AppCommonPath() + @"\PSP\sce_sys\nptitle.dat");

                    //and replace
                    int x, j, iMax = file.Length - array.Length;
                    for (x = 0; x <= iMax; x++)
                    {
                        for (j = 0; j < array.Length; j++)
                            if (file[x + j] != array[j]) break;
                        if (j == array.Length) break;
                    }
                    if (x <= iMax)
                    {
                        for (j = 0; j < array.Length; j++)
                            file[x + j] = bytes[j];
                        File.WriteAllBytes(AppCommonPath() + @"\PSP\sce_sys\nptitle.dat", file);
                    }
                }

                #endregion << Patch NP Title File >>

                UpdateString("Creating PS4 PKG");
                BusyCoping = true;
                new System.Threading.Thread(new System.Threading.ThreadStart(delegate
                {
                    Orbis_CMD("", "img_create --oformat pkg \"" + AppCommonPath() + @"\PSXEmu\" + "PSPClassics.gp4\" \"" + saveFileDialog1.SelectedPath + "\"");
                    BusyCoping = false;
                })).Start();

                while (BusyCoping == true)
                {
                    DoEvents();
                }

                UpdateString("Done Opening Location");
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.Message);
            }
        }

        public static string ByteArrayToString(byte[] ba)
        {
            System.Text.StringBuilder hex = new System.Text.StringBuilder(ba.Length * 2);
            foreach (byte b in ba)
                hex.AppendFormat("{0:x2}", b);
            return hex.ToString();
        }

        /// <summary>
        /// When Background Worker Is Done Open folder
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {

            System.Windows.Forms.FolderBrowserDialog saveFileDialog1 = tempkeeper;

            //MessageBox.Show("Convert completed");
            //no messagebox instead play the bootlogo sound 

            SoundClass.PlayPS4Sound(SoundClass.Sound.Notification);
            OpenCloseWaitScreen(false);
            Process.Start(saveFileDialog1.SelectedPath);


            //now we delete the working directory
            DeleteDirectory(AppCommonPath() + @"\Working\");
            DeleteDirectory(AppCommonPath() + @"\PSP\");
            DeleteDirectory(AppCommonPath() + @"\PSXEmu\");

            //Delete Some FIles that are no longer required
            File.Delete(AppCommonPath() + @"\pkg.exe");
            File.Delete(AppCommonPath() + @"\orbis-pub-cmd.exe");


            //reset some vales 
            AddCustomPS2Config = false;
            CustomConfigLocation = string.Empty;

            UpdateInfo("Ready");
        }

        private void MenuItem_Click_5(object sender, RoutedEventArgs e)
        {
            //SoundClass.PlayPS4Sound(SoundClass.Sound.Navigation);
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            SoundClass.PS4BGMDevice.Stop();
            SoundClass.PlayPS4Sound(SoundClass.Sound.Shutdown);

            try
            {
                if (Util.SoundClass.atr3vlc.IsPlaying)
                {
                    Util.SoundClass.atr3vlc.Stop();
                    //Util.SoundClass.atr3vlc.Dispose();
                }
                if (tempvlc.IsPlaying)
                {
                    tempvlc.Stop();
                    // tempvlc.Dispose();
                }
            }
            catch (Exception ex)
            {
                var temp = ex;
            }

            new Thread(() =>
            {
                Thread.CurrentThread.IsBackground = true;
                /* run your code here */
                Thread.Sleep(TimeSpan.FromSeconds(3));
                Environment.Exit(0);
            }).Start();
        }

        /// <summary>
        /// Menu Item Click Change Icon0
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MenuItem_Click_6(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.Title = "Select Image";
            dlg.Filter = "Image File|*.bmp;*.jpg;*.jpeg;*.png";
            dlg.InitialDirectory = Environment.SpecialFolder.MyComputer.ToString();
            if (dlg.ShowDialog() == true)
            {
                string fileName;
                fileName = dlg.FileName;

                BitmapImage image = new BitmapImage();
                image.BeginInit();
                image.UriSource = new Uri(fileName);
                image.EndInit();
                Icon0.Source = image;
                Icon0.Stretch = Stretch.Fill;
            }
        }

        private void MenuItem_Click_7(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.Title = "Select Image";
            dlg.Filter = "Image File|*.bmp;*.jpg;*.jpeg;*.png";
            dlg.InitialDirectory = Environment.SpecialFolder.MyComputer.ToString();
            if (dlg.ShowDialog() == true)
            {
                string fileName;
                fileName = dlg.FileName;
                ImageBrush imgB = new ImageBrush();

                BitmapImage btpImg = new BitmapImage();
                btpImg.BeginInit();
                btpImg.UriSource = new Uri(fileName);
                btpImg.EndInit();
                imgB.ImageSource = btpImg;
                BackgroundImage.Background = imgB;
            }

        }

        private void btnConvert_Click(object sender, RoutedEventArgs e)
        {
            if (pspfiles.Count == 0)
            {
                return;
            }

            UpdateInfo("Extarcting Resources Started");
            //extract all resources for the current program
            ExtractAllResources();


            UpdateInfo("Converting ISO(s) to PKG ");

            CheckString();

            //moving code over
            ExtractAllResources();//extarct all resources when we need it

            UpdateInfo("Save File Dialog");

            System.Windows.Forms.FolderBrowserDialog saveFileDialog1 = new System.Windows.Forms.FolderBrowserDialog();
            //saveFileDialog1.Filter = "PS4 PKG|*.pkg";
            //saveFileDialog1.Title = "Save an PS4 PKG File";
            //saveFileDialog1.ov
            if (System.Windows.Forms.DialogResult.OK != saveFileDialog1.ShowDialog())
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
            catch (Exception ex)
            {
                OpenCloseWaitScreen(false);
                System.Windows.MessageBox.Show(ex.Message);
            }


        }

        private void btnStart_Click(object sender, RoutedEventArgs e)
        {
            if (File.Exists(txtPath.Text))
            {
                string proc = AppCommonPath() + @"\PSX_Emu\emuhawk.exe --chromeless --fullscreen """ + txtPath.Text + @"""";

                Process.Start(proc);
            }

        }

        private void btnMutePlaySound_Click(object sender, RoutedEventArgs e)
        {
            if (Properties.Settings.Default.EnableGuiMusic == true)
            {
                btnMutePlaySound.Background = ImageBrushFromBitmap(Properties.Resources.icon_sound_on);
                Properties.Settings.Default.EnableGuiMusic = false;
                SoundClass.PS4BGMDevice.Stop();
            }
            else
            {
                btnMutePlaySound.Background = ImageBrushFromBitmap(Properties.Resources.icon_sound_mute);
                Properties.Settings.Default.EnableGuiMusic = true;
                SoundClass.PlayPS4Sound(SoundClass.Sound.PS4_Music);
            }
            Properties.Settings.Default.Save();
        }

        #endregion << Events >>

        #region << Methods >>


        public bool doesStringMatch()
        {
            if (pspfiles.Count == 0)
            {
                return false;
            }

            string txt = string.Empty;
            if (pspfiles.Count > 1)
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
                if (System.Windows.Forms.DialogResult.OK == System.Windows.Forms.MessageBox.Show("Content ID for this package is in the incorect format\n\nWould you like to edit this?", "Pakcage Content ID", System.Windows.Forms.MessageBoxButtons.YesNo, System.Windows.Forms.MessageBoxIcon.Error))
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
            xmldoc.Load(AppCommonPath() + @"\PSXEmu\" + "PS2Classics.gp4");
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
            try
            {

                //create new XML Document 
                xmldoc = new XmlDataDocument();
                //nodelist 
                XmlNodeList xmlnode;
                //setup the resource file to be extarcted
                string RunningPath = AppDomain.CurrentDomain.BaseDirectory;
                //load the xml file from the base directory
                xmldoc.Load(AppCommonPath() + @"PSXEmu\" + "PSPClassics.gp4");
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

                xmldoc.Save(AppCommonPath() + @"\PSXEmu\" + "PSPClassics.gp4");

                //im cheating here a bit 

                //line builder
                //string tempval = @"    <file targ_path=""data/GAME.iso"" orig_path=""..\PSP\image\disc01.iso""" + @" />";
                //string builder = string.Empty;

                //for (int i = 0; i < MainWindow.pspfiles.Count; i++)
                //{
                //    builder += tempval.Replace("disc01.iso", "disc0" + (i + 1) + ".iso") + "\n";
                //}

                //var alllines = File.ReadAllText(AppCommonPath() + @"\PSXEmu\" + "PS2Classics.gp4");

                //alllines = alllines.Replace(tempval, builder.Remove(builder.Length - 1, 1));

                //File.WriteAllText(AppCommonPath() + @"\PSXEmu\" + "PS2Classics.gp4", alllines);

                #region << Configs >>
                //if (pspfiles.Count > 1)
                //{
                //    //line builder
                //    tempval = @"    <file targ_path=""patches/SLES-50366_cli.conf"" orig_path=""..\PS2\patches\SLES-50366_cli.conf""" + @" />";
                //    builder = string.Empty;

                //    for (int i = 0; i < MainWindow.PS2CutomLua.Count; i++)
                //    {
                //        builder += tempval.Replace("SLES-50366_cli.conf", PS2TitleId[i].ToString() /*Game Name Here*/+ "_cli.conf") + "\n";
                //    }

                //    alllines = File.ReadAllText(AppCommonPath() + @"\PSXEmu\" + "PS2Classics.gp4");

                //    alllines = alllines.Replace("@addps2patchHere", builder.Remove(builder.Length - 1, 1));

                //    File.WriteAllText(AppCommonPath() + @"\PSXEmu\" + "PS2Classics.gp4", alllines);


                //    tempval = @"    <file targ_path=""lua_include/SLUS-20071_config.lua"" orig_path=""..\PS2\lua_include\SLUS-20071_config.lua""" + @" />";
                //    builder = string.Empty;

                //    for (int i = 0; i < MainWindow.PS2CutomLua.Count; i++)
                //    {
                //        builder += tempval.Replace("SLUS-20071_config.lua", PS2TitleId[i].ToString().Replace(".","") /*Game Name Here*/+ "_config.lua") + "\n";
                //    }

                //    alllines = File.ReadAllText(AppCommonPath() + @"\PSXEmu\" + "PS2Classics.gp4");

                //    alllines = alllines.Replace("@addps2luhere", builder.Remove(builder.Length - 1, 1));

                //    File.WriteAllText(AppCommonPath() + @"\PSXEmu\" + "PS2Classics.gp4", alllines);

                //}
                //else
                //{
                //    alllines = alllines.Replace("@addps2patchHere", "");//remove the string
                //    alllines = alllines.Replace("@addps2luhere", "");//remove the string
                //    File.WriteAllText(AppCommonPath() + @"\PSXEmu\" + "PS2Classics.gp4", alllines);
                //}
                #endregion << Configs >>

            }
            catch (Exception ex)
            {
                var file = File.ReadAllText(AppCommonPath() + @"PSXEmu\" + "PSPClassics.gp4").Replace("UP9000-CUSA32644_00-NPUG801350000000", xmlcontentid);
                File.WriteAllText(AppCommonPath() + @"PSXEmu\" + "PSPClassics.gp4", file);
            }
        }


        public void ExtractAllResources()
        {
            UpdateInfo("Checking Directory Paths");
            if (!Directory.Exists(AppCommonPath()))
            {
                UpdateInfo("Created Directory" + AppCommonPath());
                Directory.CreateDirectory(AppCommonPath());
            }
            if (!Directory.Exists(AppCommonPath() + @"\PSXEmu\"))
            {
                UpdateInfo("Created Directory" + AppCommonPath() + @"\PSXEmu\");
                Directory.CreateDirectory(AppCommonPath() + @"\PSXEmu\");
            }

            UpdateInfo("Writing All Binary Files to Temp Path....");

            //copy byte files
            System.IO.File.WriteAllBytes(AppCommonPath() + @"\PSXEmu\" + "PSPClassics.gp4", Properties.Resources.psphd1);
            UpdateInfo("Writing Binary File to Temp Path " + "\n Written : " + AppCommonPath() + @"\PSXEmu\" + "PSPClassics.gp4");
            System.IO.File.WriteAllBytes(AppCommonPath() + @"\PSXEmu\" + "param.sfo", Properties.Resources.param);
            UpdateInfo("Writing Binary File to Temp Path " + "\n Written : " + AppCommonPath() + @"\PSXEmu\" + "param.sfo");
            System.IO.File.WriteAllBytes(AppCommonPath() + "orbis-pub-cmd.exe", Properties.Resources.orbis_pub_cmd);
            UpdateInfo("Writing Binary File to Temp Path " + "\n Written : " + AppCommonPath() + "orbis-pub-cmd.exe");

            System.IO.File.WriteAllBytes(AppCommonPath() + "PSP.zip", Properties.Resources.psphd);
            UpdateInfo("Writing Binary File to Temp Path " + "\n Written : " + AppCommonPath() + "PSP.zip");
            System.IO.File.WriteAllBytes(AppCommonPath() + "ext.zip", Properties.Resources.ext);
            UpdateInfo("Writing Binary File to Temp Path " + "\n Written : " + AppCommonPath() + "ext.zip");


            System.IO.File.WriteAllBytes(AppCommonPath() + "psppkg.zip", Properties.Resources.psppkg);
            UpdateInfo("Writing Binary File to Temp Path " + "\n Written : " + AppCommonPath() + "psppkg.zip");

            UpdateInfo("Writing Image Files to Temp Path...");

            //copy images for the save process
            Properties.Resources.icon0.Save(AppCommonPath() + @"\PSXEmu\" + "icon0.png");
            UpdateInfo("Writing Image File to Temp Path " + "\n Written : " + AppCommonPath() + @"\PSXEmu\" + "icon0.png");
            Properties.Resources.icon0.Save(AppCommonPath() + @"\PSXEmu\" + "pic0.png");
            UpdateInfo("Writing Image File to Temp Path " + "\n Written : " + AppCommonPath() + @"\PSXEmu\" + "pic0.png");
            Properties.Resources.icon0.Save(AppCommonPath() + @"\PSXEmu\" + "pic1.png");
            UpdateInfo("Writing Image File to Temp Path " + "\n Written : " + AppCommonPath() + @"\PSXEmu\" + "pic1.png");

            UpdateInfo("Writing Text Files to Temp Path...");
            //copy text files
            System.IO.File.WriteAllText(AppCommonPath() + @"\PSXEmu\" + "sfo.xml", Properties.Resources.sfo);
            UpdateInfo("Writing Image File to Temp Path " + "\n Written : " + AppCommonPath() + @"\PSXEmu\" + "sfo.xml");


            UpdateInfo("Extracting Zip(s)");
            //extarct zip
            if (Directory.Exists(AppCommonPath() + @"\PSP\"))
            {
                DeleteDirectory(AppCommonPath() + @"\PSP\");
            }
            ZipFile.ExtractToDirectory(AppCommonPath() + "PSP.zip", AppCommonPath() + @"\PSP\");


            if (Directory.Exists(AppCommonPath() + @"\ext\"))
            {
                DeleteDirectory(AppCommonPath() + @"\ext\");
            }
            if (File.Exists(AppCommonPath() + @"\pkg.exe"))
            {
                File.Delete(AppCommonPath() + @"\pkg.exe");
            }
            //ZipFile.ExtractToDirectory(AppCommonPath() + "ext.zip", AppCommonPath());

            ZipFile.ExtractToDirectory(AppCommonPath() + "psppkg.zip", AppCommonPath());

            File.Delete(AppCommonPath() + "ext.zip");
            File.Delete((AppCommonPath() + "PSP.zip"));
            File.Delete((AppCommonPath() + "psppkg.zip"));
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

            if (!Directory.Exists(returnstring))
            {
                Directory.CreateDirectory(returnstring);
            }
            return returnstring;
        }

        //If you get 'dllimport unknown'-, then add 'using System.Runtime.InteropServices;'
        [DllImport("gdi32.dll", EntryPoint = "DeleteObject")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool DeleteObject([In] IntPtr hObject);

        public ImageSource ImageSourceForBitmap(Bitmap bmp)
        {
            var handle = bmp.GetHbitmap();
            try
            {
                return System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(handle, IntPtr.Zero, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());
            }
            finally { DeleteObject(handle); }
        }


        public ImageBrush ImageBrushFromBitmap(Bitmap bmp)
        {
            var bitmapSource = System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(bmp.GetHbitmap(),
                                  IntPtr.Zero,
                                  Int32Rect.Empty,
                                  BitmapSizeOptions.FromEmptyOptions());
            return new ImageBrush(bitmapSource);
        }


        Bitmap GetBitmap(BitmapSource source)
        {
            Bitmap bmp = new Bitmap(
              source.PixelWidth,
              source.PixelHeight, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
            System.Drawing.Imaging.BitmapData data = bmp.LockBits(
              new System.Drawing.Rectangle(System.Drawing.Point.Empty, bmp.Size),
              System.Drawing.Imaging.ImageLockMode.WriteOnly,
              System.Drawing.Imaging.PixelFormat.Format32bppPArgb);
            source.CopyPixels(
              Int32Rect.Empty,
              data.Scan0,
              data.Height * data.Stride,
              data.Stride);
            bmp.UnlockBits(data);
            return bmp;
        }

        /// <summary>
        /// Resize the image to the specified width and height.
        /// </summary>
        /// <param name="image">The image to resize.</param>
        /// <param name="width">The width to resize to.</param>
        /// <param name="height">The height to resize to.</param>
        /// <returns>The resized image.</returns>
        public static Bitmap ResizeImage(System.Drawing.Image image, int width, int height)
        {
            var destRect = new System.Drawing.Rectangle(0, 0, width, height);
            var destImage = new Bitmap(width, height);

            destImage.SetResolution(image.HorizontalResolution, image.VerticalResolution);

            using (var graphics = Graphics.FromImage(destImage))
            {
                graphics.CompositingMode = CompositingMode.SourceCopy;
                graphics.CompositingQuality = CompositingQuality.HighQuality;
                graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                graphics.SmoothingMode = SmoothingMode.HighQuality;
                graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;

                using (var wrapMode = new System.Drawing.Imaging.ImageAttributes())
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
                gr.DrawImage(img, new System.Drawing.Rectangle(0, 0, img.Width, img.Height));
            return bmp;
        }


        public static void DoEvents()
        {
            Application.Current.Dispatcher.Invoke(DispatcherPriority.Background,
                                                  new Action(delegate { }));
        }


        /// <summary>
        /// This luanches our custom Messagebox class for the ps4 ui
        /// </summary>
        /// <param name="Text">Test will diplsay in the middle</param>
        /// <param name="Caption">Caption of the fomr</param>
        /// <param name="Messagebuttons">Give it some buttons</param>
        /// <param name="MessageImages">the image will be related to a sound</param>
        /// <returns></returns>
        public PS4_MessageBoxResult CustomMessageBox(string Text, string Caption, PS4_MessageBoxButton Messagebuttons, MessageBoxImage MessageImages)
        {
            MessageBox ps4messagebox = new MessageBox(Text, Caption, Messagebuttons, SoundClass.Sound.Error);
            ps4messagebox.ShowDialog();
            PS4_MessageBoxResult result = MessageBox.ReturnResult;
            return result;
        }


        /// <summary>
        /// updates the advanced window
        /// </summary>
        /// <param name="info"></param>
        public void UpdateInfo(string info)
        {
            if (Properties.Settings.Default.EnableAdvancedMode == true)
            {
                advanced.Dispatcher.Invoke(new Action(() => advanced.LabelText += info + Environment.NewLine));
            }
        }

        private void UpdateString(string txt)
        {
            UpdateInfo(txt);
            OpenCloseWaitScreen(true);
            Busy.INFO = txt;
            //lblTask.Invoke(new Action(() => lblTask.Text = txt));
        }



        #region << Orbis >>
        public string Orbis_CMD(string command, string arguments)
        {
            ProcessStartInfo start = new ProcessStartInfo();
            start.FileName = AppCommonPath() + "pkg.exe " + command;
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
                    if (result.Contains("already converted from elf file to self file"))
                    {
                        System.Windows.Forms.DialogResult dlr = System.Windows.Forms.MessageBox.Show("Already Converted From Elf Error Found.... will be using Orbis-pub-gen for this pkg\n\n Simply Click Build and select the save folder", "Error with an alternative", System.Windows.Forms.MessageBoxButtons.OKCancel, System.Windows.Forms.MessageBoxIcon.Question);
                        if (dlr == System.Windows.Forms.DialogResult.OK)
                        {
                            //this will open up the GP4 Project inside the Utility
                            Orbis_Pub__GenCMD("", AppCommonPath() + @"\PSXEmu\" + "PS2Classics.gp4");

                        }
                    }
                    else if (result.Contains("[Error]"))
                    {
                        System.Windows.Forms.MessageBox.Show(result);
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

        #endregion << Orbis >>

        #endregion << Methods >>
    }

    public static class BinarryTools
    {
        /// <summary>
        /// Finds the first occurrence of <paramref name="pattern"/> in a stream
        /// </summary>
        /// <param name="s">The input stream</param>
        /// <param name="pattern">The pattern</param>
        /// <returns>The index of the first occurrence, or -1 if the pattern has not been found</returns>
        public static long IndexOf(Stream s, byte[] pattern)
        {
            // Prepare the bad character array is done once in a separate step
            var badCharacters = MakeBadCharArray(pattern);

            // We now repeatedly read the stream into a buffer and apply the Boyer-Moore-Horspool algorithm on the buffer until we get a match
            var buffer = new byte[Math.Max(2 * pattern.Length, 4096)];
            long offset = 0; // keep track of the offset in the input stream
            while (true)
            {
                int dataLength;
                if (offset == 0)
                {
                    // the first time we fill the whole buffer
                    dataLength = s.Read(buffer, 0, buffer.Length);
                }
                else
                {
                    // Later, copy the last pattern.Length bytes from the previous buffer to the start and fill up from the stream
                    // This is important so we can also find matches which are partly in the old buffer
                    Array.Copy(buffer, buffer.Length - pattern.Length, buffer, 0, pattern.Length);
                    dataLength = s.Read(buffer, pattern.Length, buffer.Length - pattern.Length) + pattern.Length;
                }

                var index = IndexOf(buffer, dataLength, pattern, badCharacters);
                if (index >= 0)
                    return offset + index; // found!
                if (dataLength < buffer.Length)
                    break;
                offset += dataLength - pattern.Length;
            }

            return -1;
        }

        // --- Boyer-Moore-Horspool algorithm ---
        // (Slightly modified code from
        // https://stackoverflow.com/questions/16252518/boyer-moore-horspool-algorithm-for-all-matches-find-byte-array-inside-byte-arra)
        // Prepare the bad character array is done once in a separate step:
        private static int[] MakeBadCharArray(byte[] pattern)
        {
            var badCharacters = new int[256];

            for (long i = 0; i < 256; ++i)
                badCharacters[i] = pattern.Length;

            for (var i = 0; i < pattern.Length - 1; ++i)
                badCharacters[pattern[i]] = pattern.Length - 1 - i;

            return badCharacters;
        }

        // Core of the BMH algorithm
        private static int IndexOf(byte[] value, int valueLength, byte[] pattern, int[] badCharacters)
        {
            int index = 0;

            while (index <= valueLength - pattern.Length)
            {
                for (var i = pattern.Length - 1; value[index + i] == pattern[i]; --i)
                {
                    if (i == 0)
                        return index;
                }

                index += badCharacters[value[index + pattern.Length - 1]];
            }

            return -1;
        }

        public static List<long> GetPatternPositions(string path, byte[] pattern)
        {
            using (FileStream stream = new FileStream(path, FileMode.Open))
            {
                List<long> searchResults = new List<long>(); //The results as offsets within the file
                int patternPosition = 0; //Track of how much of the array has been matched
                long filePosition = 0;
                long bufferSize = Math.Min(stream.Length, 100000);

                byte[] buffer = new byte[bufferSize];
                int readCount = 0;

                while ((readCount = stream.Read(buffer, 0, buffer.Length)) > 0)
                {
                    for (int i = 0; i < readCount; i++)
                    {
                        byte currentByte = buffer[i];

                        if (currentByte == pattern[0])
                            patternPosition = 0;

                        if (currentByte == pattern[patternPosition])
                        {
                            patternPosition++;
                            if (patternPosition == pattern.Length)
                            {
                                searchResults.Add(filePosition + 1 - pattern.Length);
                                patternPosition = 0;
                            }
                        }
                        filePosition++;
                    }
                }

                return searchResults;
            }
        }

        public static void ExtractDirectory(DiscUtils.DiscDirectoryInfo Dinfo, string RootPath, string PathinISO)
        {
            if (!string.IsNullOrWhiteSpace(PathinISO))
            {
                PathinISO += "\\" + Dinfo.Name;
            }
            RootPath += "\\" + Dinfo.Name;
            AppendDirectory(RootPath);
            foreach (DiscUtils.DiscDirectoryInfo dinfo in Dinfo.GetDirectories())
            {
                ExtractDirectory(dinfo, RootPath, PathinISO);
            }
            foreach (DiscUtils.DiscFileInfo finfo in Dinfo.GetFiles())
            {
                using (Stream FileStr = finfo.OpenRead())
                {
                    using (FileStream Fs = File.Create(RootPath + "\\" + finfo.Name)) // Here you can Set the BufferSize Also e.g. File.Create(RootPath + "\\" + finfo.Name, 4 * 1024)
                    {
                        FileStr.CopyTo(Fs, 4 * 1024); // Buffer Size is 4 * 1024 but you can modify it in your code as per your need
                    }
                }
            }
        }
        static void AppendDirectory(string path)
        {
            try
            {
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
            }
            catch (DirectoryNotFoundException Ex)
            {
                AppendDirectory(Path.GetDirectoryName(path));
            }
            catch (PathTooLongException Exx)
            {
                AppendDirectory(Path.GetDirectoryName(path));
            }
        }


        public static object ReadRomData(byte[] file, long offset, int lenght)
        {
            long num = offset;
            checked
            {
                byte[] array = new byte[lenght - 1 + 1];
                using (BinaryReader binaryReader = new BinaryReader(new MemoryStream(file)))
                {
                    long length = binaryReader.BaseStream.Length;
                    int num2 = 0;
                    binaryReader.BaseStream.Seek(num, SeekOrigin.Begin);
                    while (num < length & num2 < lenght)
                    {
                        array[num2] = binaryReader.ReadByte();
                        num += 1L;
                        num2++;
                    }
                }
                return array;
            }
        }

        public static object FindOffset(string filename, byte[] query)
        {
            using (BinaryReader binaryReader = new BinaryReader(File.Open(filename, FileMode.Open)))
            {
                double num = (double)binaryReader.BaseStream.Length;
                if ((double)query.Length <= num)
                {
                    byte[] array = binaryReader.ReadBytes(query.Length);
                    bool flag = false;
                    double num3;
                    checked
                    {
                        int num2 = query.Length - 1;
                        for (int i = 0; i <= num2; i++)
                        {
                            if (array[i] != query[i])
                            {
                                flag = false;
                                break;
                            }
                            flag = true;
                        }
                        if (flag)
                        {
                            return 0;
                        }
                        num3 = (double)query.Length;
                    }
                    double num4 = num - 1.0;
                    for (double num5 = num3; num5 <= num4; num5 += 1.0)
                    {
                        checked
                        {
                            Array.Copy(array, 1, array, 0, array.Length - 1);
                            array[array.Length - 1] = binaryReader.ReadByte();
                            int num6 = query.Length - 1;
                            for (int j = 0; j <= num6; j++)
                            {
                                if (array[j] != query[j])
                                {
                                    flag = false;
                                    break;
                                }
                                flag = true;
                            }
                        }
                        if (flag)
                        {
                            return num5 - (double)(checked(query.Length - 1));
                        }
                    }
                }
            }
            return -1;
        }
        public static string CreateIsoImage(string sourceDrive, string targetIso, string volumeName)
        {
            try
            {
                var srcFiles = Directory.GetFiles(sourceDrive, "*", SearchOption.AllDirectories);
                var iso = new CDBuilder
                {
                    UseJoliet = true,
                    VolumeIdentifier = volumeName
                };

                foreach (var file in srcFiles)
                {
                    var fi = new FileInfo(file);
                    if (fi.Directory.Name == sourceDrive)
                    {
                        iso.AddFile($"{fi.Name}", fi.FullName);
                        continue;
                    }
                    var srcDir = fi.Directory.FullName.Replace(sourceDrive, "").TrimEnd('\\');
                    iso.AddDirectory(srcDir);
                    iso.AddFile($"{srcDir}\\{fi.Name}", fi.FullName);
                }

                iso.Build(targetIso);
                return "Success";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }


        public static byte[] ReplaceInByteArray(byte[] OriginalArray, byte[] Find,
    byte[] Replace)
        {
            byte[] ReturnValue = OriginalArray;

            if (System.Array.BinarySearch(ReturnValue, Find) > -1)
            {
                byte[] NewReturnValue;
                int lFoundPosition;
                int lCurrentPosition;
                int lCurrentOriginalPosition;
                while (FindInByteArray(ReturnValue, Find) > -1)
                {
                    NewReturnValue = new byte[ReturnValue.Length + Replace.Length -
                    Find.Length];
                    lFoundPosition = FindInByteArray(ReturnValue, Find);
                    lCurrentPosition = 0;
                    lCurrentOriginalPosition = 0;

                    for (int x = 0; x < lFoundPosition; x++)
                    {
                        NewReturnValue[x] = ReturnValue[x];
                        lCurrentPosition++;
                        lCurrentOriginalPosition++;
                    }

                    for (int y = 0; y < Replace.Length; y++)
                    {
                        NewReturnValue[lCurrentPosition] = Replace[y];
                        lCurrentPosition++;
                    }

                    lCurrentOriginalPosition = lCurrentOriginalPosition + Find.Length;

                    while (lCurrentPosition < NewReturnValue.Length)
                    {
                        NewReturnValue[lCurrentPosition] =
                        ReturnValue[lCurrentOriginalPosition];
                        lCurrentPosition++;
                        lCurrentOriginalPosition++;
                    }

                    ReturnValue = NewReturnValue;
                }

            }
            return ReturnValue;
        }

        private static int FindInByteArray(byte[] Haystack, byte[] Needle)
        {
            int lFoundPosition = -1;
            int lMayHaveFoundIt = -1;
            int lMiniCounter = 0;

            for (int lCounter = 0; lCounter < Haystack.Length; lCounter++)
            {
                if (Haystack[lCounter] == Needle[lMiniCounter])
                {
                    if (lMiniCounter == 0)
                        lMayHaveFoundIt = lCounter;
                    if (lMiniCounter == Needle.Length - 1)
                    {
                        return lMayHaveFoundIt;
                    }
                    lMiniCounter++;
                }
                else
                {
                    lMayHaveFoundIt = -1;
                    lMiniCounter = 0;
                }
            }

            return lFoundPosition;
        }

    }


}
