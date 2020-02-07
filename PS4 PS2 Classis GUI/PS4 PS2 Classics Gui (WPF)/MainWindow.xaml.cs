#region << Usinings >>

using DiscUtils.Iso9660;
using Microsoft.Win32;
using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Xml;
using System.Deployment;
using System.Deployment.Application;
using System.IO.Compression;
using System.Threading;
using System.ComponentModel;
using System.Text.RegularExpressions;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Threading;
using System.Runtime.InteropServices;
using System.Net;
using System.Security.Principal;
using LibOrbisPkg.GP4;
using LibOrbisPkg.PKG;
using System.Windows.Media.Animation;

#endregion << Usinings >>

namespace PS4_PS2_Classics_Gui__WPF_
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

        public static string VersionNum = "";

        private readonly BackgroundWorker backgroundWorker1 = new BackgroundWorker();
        private readonly BackgroundWorker bgWorkerSS = new BackgroundWorker();
        System.Windows.Forms.FolderBrowserDialog tempkeeper = null;

        public static bool bgwClose;
        public static IWavePlayer waveOutDevice = new WaveOut();
        XmlDataDocument xmldoc = null;
        public static string xmlcontentid { get; set; }


        //items needed
        string PS2ID;
        /// <summary>
        /// This is used for kozarovv patches
        /// </summary>
        string OriginalPS2ID;
        public static List<string> isoFiles = new List<string>();
        public static List<string> PS2CutomLua = new List<string>();
        public static List<string> PS2TitleId = new List<string>();

        //advanced window
        Advanced advanced = new Advanced();

        //Bools
        bool BusyCoping = false;
        bool AddCustomPS2Config = false;
        string CustomConfigLocation = string.Empty;

        //URL String For Custom Patches
        String Url = "https://github.com/kozarovv/PS2-Configs";

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
            CustomMessageBox(@"A Special thanks as always to flatz for his awesome work 
Thanks to cfwprophet and VVildCard777 for dealing with all my questions and their help
Special thanks to zordon605 for PS2 Multi Iso Info", "Credits", PS4_MessageBoxButton.OK, MessageBoxImage.Information);
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


        /// <summary>
        /// This Loads the PS2 Iso Info 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSelectISO_Click(object sender, RoutedEventArgs e)
        {
           

            //set defualt values
            txtPath.IsEnabled = true;
            lblPS2ID.Visibility = Visibility.Visible;

            //UpdateInfo("Checking if there is already a mutli iso screen open and closing it");

            //close open form 
            //this closes any open form
            if (Application.Current.Windows.OfType<MultipleISO_s>().Count() == 1)
                Application.Current.Windows.OfType<MultipleISO_s>().First().Close();
            MultipleISO_s multi = new MultipleISO_s();
            //UpdateInfo("Open File Dialog");
            //Open File Dialog For ISO Files
            OpenFileDialog thedialog = new OpenFileDialog();
            thedialog.Title = "Select ISO";
            thedialog.Filter = "Image File|*.iso;*.cue";
            thedialog.Multiselect = true;
            thedialog.InitialDirectory = Environment.SpecialFolder.MyComputer.ToString();
            if (thedialog.ShowDialog() == true)
            {
                //clear the config file as well
                PS2CutomLua.Clear();

                //UpdateInfo("adding iso files to list");
                isoFiles = thedialog.FileNames.ToList<string>();//tada now we know how many iso's there is 
                if (isoFiles.Count > 7)
                {

                    System.Windows.MessageBox.Show("Maximum amount of ISO's allowed by the PS4 in a classic is 7");
                    isoFiles.Clear();
                    return;
                }
                if (isoFiles.Count == 1)
                {
                    //UpdateInfo("Standard ISO Method");
                    #region << For Single File >>
                    //set the path and the text on the gui
                    string isopath = thedialog.FileName;
                    txtPath.Text = isopath;
                    txtPath.Focus();
                    txtPath.CaretIndex = txtPath.Text.Length;

                    if (System.IO.Path.GetExtension(isopath).ToUpper() == ".ISO")
                    {

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
                                    OriginalPS2ID = PS2Id;
                                    PS2ID = PS2Id.Replace(".", "");
                                    lblPS2ID.Content = "PS2 ID : " + PS2Id.Replace(".", "");

                                    if (Properties.Settings.Default.EnablePS2IDReplace == true)
                                    {
                                        txtContentID.Text = PS2ID.Replace("_", "");
                                    }

                                    #region << PS2 Tools >>
                                    try
                                    {
                                        var ps2item = PS2_Tools.PS2_Content.GetPS2Item(PS2ID.Replace("_", "-"));

                                        BitmapImage image = new BitmapImage();
                                        image.BeginInit();
                                        image.UriSource = new Uri(ps2item.Picture);
                                        image.EndInit();
                                        Icon0.Source = image;
                                        Icon0.Stretch = Stretch.Fill;

                                        txtTitle.Text = ps2item.PS2_Title;
                                    }
                                    catch(Exception ex)
                                    {
                                        //ps2 tools through an error
                                    }
                                    #endregion << PS2 Tools >>

                                }
                                else
                                {
                                    System.Windows.MessageBox.Show("Could not load PS2 ID");
                                }
                            }
                            else
                            {
                                MessageBoxResult msrlt = System.Windows.MessageBox.Show("Could not load PS2 ID\n\n wpuld you like to submit an issue ?", "Error Reporting", MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.Yes);
                                if (msrlt == MessageBoxResult.Yes)
                                {
                                    //load github issue page
                                    Process.Start(@"https://github.com/xXxTheDarkprogramerxXx/PS3Tools/issues");
                                }

                            }
                        }
                    }
                    else if(System.IO.Path.GetExtension(isopath).ToUpper() == ".CUE")//cue file
                    {
                        PS2_Tools.BinCue.CueFile file = new PS2_Tools.BinCue.CueFile(isopath);
                        //load some info from the binfile 
                        if (!File.Exists(file.BinFileName))
                        {

                        }

                        using (var fs = new FileStream(file.BinFileName, FileMode.Open, FileAccess.Read))
                        {
                            byte[] temp = new byte[fs.Length];
                            int bytesRead = fs.Read(temp, 0, temp.Length);
                            // buffer now contains the entire contents of the file
                           // byte[] temp = new byte[10];

                            byte[] cdrom = new byte[] { 0x42, 0x4F, 0x4F, 0x54, 0x32, 0x20, 0x3D, 0x20, 0x63, 0x64, 0x72, 0x6F, 0x6D, 0x30, 0x3A, 0x5C };

                            List<int> positions = SearchBytePattern(cdrom, temp);
                            //found it 
                            if (positions.Count > 0)
                            {
                                // as a separate buffer
                                byte[] copy = new byte[30];
                                Buffer.BlockCopy(temp, positions[0], copy, 0, copy.Length);
                                string fullstring = Encoding.ASCII.GetString(copy);

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
                                        OriginalPS2ID = PS2Id;
                                        PS2ID = PS2Id.Replace(".", "");
                                        lblPS2ID.Content = "PS2 ID : " + PS2Id.Replace(".", "");

                                        if (Properties.Settings.Default.EnablePS2IDReplace == true)
                                        {
                                            txtContentID.Text = PS2ID.Replace("_", "");
                                        }
                                        #region << PS2 Tools >>
                                        try
                                        {
                                            var ps2item = PS2_Tools.PS2_Content.GetPS2Item(PS2ID.Replace("_", "-"));

                                            BitmapImage image = new BitmapImage();
                                            image.BeginInit();
                                            image.UriSource = new Uri(ps2item.Picture);
                                            image.EndInit();
                                            Icon0.Source = image;
                                            Icon0.Stretch = Stretch.Fill;

                                            txtTitle.Text = ps2item.PS2_Title;
                                        }
                                        catch (Exception ex)
                                        {
                                            //ps2 tools through an error
                                        }
                                        #endregion << PS2 Tools >>

                                    }
                                    else
                                    {
                                        System.Windows.MessageBox.Show("Could not load PS2 ID");
                                    }

                                }
                            }
                        }
                    }

                    #endregion << For Single File >>
                }
                else if (isoFiles.Count > 1)
                {           
                    //UpdateInfo("Multi ISO Method");
                    txtPath.IsEnabled = false;
                    lblPS2ID.Visibility = Visibility.Hidden;
                    //UpdateInfo("Opening Mutli ISO Screen");
                    multi.Show();
                    //multi.SetDesktopLocation(this.Location.X + this.Size.Width, this.Location.Y);
                }
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
            if(txtTitle.Text.Trim().ToLower() == "dovahkiin")
            {
                SoundClass.PlayPS4Sound(SoundClass.Sound.sk);
                BitmapImage image = new BitmapImage();
                image.BeginInit();
                image.UriSource = new Uri(@"https://www.mobygames.com/images/covers/l/473949-the-elder-scrolls-v-skyrim-special-edition-playstation-4-manual.jpg");
                image.EndInit();
                Icon0.Source = image;
                Icon0.Stretch = Stretch.Fill;
            }
            if (txtTitle.Text.Trim().ToLower() == "pete")
            {
                SoundClass.PlayPS4Sound(SoundClass.Sound.pete);
                BitmapImage image = new BitmapImage();
                image.BeginInit();
                image.UriSource = new Uri(@"https://d3fa68hw0m2vcc.cloudfront.net/ccc/185449833.jpeg");
                image.EndInit();
                Icon0.Source = image;
                Icon0.Stretch = Stretch.Fill;
            }
            if (txtTitle.Text.Trim().ToLower() == "_snow")
            {
                SoundClass.PlayPS4Sound(SoundClass.Sound.LQ);
                var timer = new DispatcherTimer { Interval = TimeSpan.FromMilliseconds(10) };
                timer.Tick += (s, arg) => Snow();
                timer.Start();
            }
        }

        private static bool IsAdministrator()
        {
            WindowsIdentity identity = WindowsIdentity.GetCurrent();
            WindowsPrincipal principal = new WindowsPrincipal(identity);
            return principal.IsInRole(WindowsBuiltInRole.Administrator);
        }

        readonly Random _random = new Random((int)DateTime.Now.Ticks);
        private void Snow()
        {
            var xAmount = _random.Next(-500, (int)Main_Grid.ActualWidth - 100);
            var yAmount = -100;
            var s = _random.Next(5, 15) * 0.1;
            var rotateAmount = _random.Next(0, 270);

            RotateTransform rotateTransform = new RotateTransform(rotateAmount);
            ScaleTransform scaleTransform = new ScaleTransform(s, s);
            TranslateTransform translateTransform = new TranslateTransform(xAmount, yAmount);

            var flake = new SnowFlakes
            {
                RenderTransform = new TransformGroup
                {
                    Children = new TransformCollection { rotateTransform, scaleTransform, translateTransform }
                },

                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Top,
            };
            Main_Grid.Children.Add(flake);

            Duration duration = new Duration(TimeSpan.FromSeconds(_random.Next(1, 4)));

            xAmount += _random.Next(100, 500);
            var xAnimation = GenerateAnimation(xAmount, duration, flake, "RenderTransform.Children[2].X");

            yAmount += (int)(Main_Grid.ActualHeight + 100 + 100);
            var yAnimation = GenerateAnimation(yAmount, duration, flake, "RenderTransform.Children[2].Y");

            rotateAmount += _random.Next(90, 360);
            var rotateAnimation = GenerateAnimation(rotateAmount, duration, flake, "RenderTransform.Children[0].Angle");

            Storyboard story = new Storyboard();
            story.Completed += (sender, e) => Main_Grid.Children.Remove(flake);
            story.Children.Add(xAnimation);
            story.Children.Add(yAnimation);
            story.Children.Add(rotateAnimation);
            flake.Loaded += (sender, args) => story.Begin();

        }

        private static DoubleAnimation GenerateAnimation(int x, Duration duration, SnowFlakes flake, string propertyPath)
        {
            DoubleAnimation animation = new DoubleAnimation
            {
                To = x,
                Duration = duration
            };
            Storyboard.SetTarget(animation, flake);
            Storyboard.SetTargetProperty(animation, new PropertyPath(propertyPath));
            return animation;
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

                #region << App Center >>

                /*Got to love MS this app center is amazing logs debugs and error logs*/



                #endregion << App Center >>

                #region << Version Numbering >>

                Version v = Assembly.GetExecutingAssembly().GetName().Version;
                //Check to see if we are ClickOnce Deployed.
                //i.e. the executing code was installed via ClickOnce
                if (ApplicationDeployment.IsNetworkDeployed)
                {
                    //Collect the ClickOnce Current Version
                    v = ApplicationDeployment.CurrentDeployment.CurrentVersion;
                }
                if (VersionNum == "")
                {
                    VersionNum = v.ToString();
                }
                //Show the version in a simple manner
                this.Title = string.Format("PS2 Classic GUI Version : {0}", v);


                #endregion << Version Numbering >>

                #region << Admin Access is Required for some of our features >>
                if (Properties.Settings.Default.FontInstalled == false)
                {
                    if (IsAdministrator() == false && !Debugger.IsAttached)
                    {
                        /*Tests font*/

                        string fontName = "PS4Icon";
                        float fontSize = 12;

                        using (Font fontTester = new Font(
                               fontName,
                               fontSize,
                               System.Drawing.FontStyle.Regular,
                               GraphicsUnit.Pixel))
                        {
                            if (fontTester.Name == fontName)
                            {
                                // Font exists
                            }
                            else
                            {
                                // Font doesn't exist
                                this.Hide();

                                // Restart program and run as admin
                                var exeName = System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName;
                                ProcessStartInfo startInfo = new ProcessStartInfo(exeName, VersionNum);
                                startInfo.Verb = "runas";
                                System.Diagnostics.Process.Start(startInfo);
                                Application.Current.Shutdown();

                                return;
                            }
                        }

                       
                    }

                }

                #endregion << Admin Access is Required for some of our features  >>

                #region << First Time Settings >>

                if (Properties.Settings.Default.FirstTime == true)
                {
                    /*Fist Time User Boots Up Ask User if he wants to use a custom path*/
                   var ps4message =  new MessageBox(@"Do you want to use a custom path to run the GUI from this is mainly ideal
if you are using an SSD","Initialization",PS4_MessageBoxButton.YesNo,SoundClass.Sound.Notification);
                    ps4message.ShowDialog();
                    if (PS4_MessageBoxResult.Yes == MessageBox.ReturnResult)
                    {
                        /**Show New Screen to ask where to save*/
                        using (var dialog = new System.Windows.Forms.FolderBrowserDialog())
                        {
                            System.Windows.Forms.DialogResult result = dialog.ShowDialog();
                            if (result == System.Windows.Forms.DialogResult.OK)
                            {
                                Properties.Settings.Default.TempPath = dialog.SelectedPath.ToString();
                            }
                            Properties.Settings.Default.OverwriteTemp = true;
                        }
                        Properties.Settings.Default.Save();//save the settings
                    }

                    var ps4askforhelp = new MessageBox(@"Would you like to know how to use PS2 Classics ?", "PS2 Classics", PS4_MessageBoxButton.YesNo, SoundClass.Sound.Options);
                    ps4askforhelp.ShowDialog();
                    if (PS4_MessageBoxResult.Yes == MessageBox.ReturnResult)
                    {
                        HowToUse.HowToUse howtouse = new HowToUse.HowToUse();
                        howtouse.ShowDialog();
                    }
                }
                #endregion << FirsTime >>

                #region << Font Installer >>

                if(Properties.Settings.Default.FontInstalled == false)
                {
                    if (!File.Exists(System.AppDomain.CurrentDomain.BaseDirectory + "PS4Icon.ttf"))
                    {
                        System.IO.File.WriteAllBytes(System.AppDomain.CurrentDomain.BaseDirectory + "PS4Icon.ttf", Properties.Resources.PS4Icon);
                    }
                    /*Install the font*/
                    FontInstaller.RegisterFont("PS4Icon.ttf");
                    if (File.Exists(System.AppDomain.CurrentDomain.BaseDirectory + "PS4Icon.ttf"))
                    {
                        //sony icon file so we need to delete this
                        File.Delete(System.AppDomain.CurrentDomain.BaseDirectory + "PS4Icon.ttf");
                    }

                }
                Properties.Settings.Default.FontInstalled = true;
                Properties.Settings.Default.Save();

                #endregion << Font Installer >>

                #region << Background Workers >>

                //System.Windows.Forms.MessageBox.Show("Creating Background Workers");
                bgWorkerSS.DoWork += bgWorkerSS_DoWork;
                bgWorkerSS.WorkerSupportsCancellation = true;

                backgroundWorker1.DoWork += BackgroundWorker1_DoWork;
                backgroundWorker1.RunWorkerCompleted += backgroundWorker1_RunWorkerCompleted;


                #endregion << Background Workers >>

                #region << Quick Sound/Video Extract >>


                //System.Windows.Forms.MessageBox.Show("extracting files");
                //System.IO.File.WriteAllBytes(AppCommonPath() + "PS4.mp3", Properties.Resources.ps4BGM);
                System.IO.File.WriteAllBytes(AppCommonPath() + "PS2.mp4", Properties.Resources.PS2_Logo);

                if (!Directory.Exists(AppCommonPath() + @"\PS2Emu\"))
                {
                    UpdateInfo("Created Directory" + AppCommonPath() + @"\PS2Emu\");
                    Directory.CreateDirectory(AppCommonPath() + @"\PS2Emu\");
                }
                System.IO.File.WriteAllBytes(AppCommonPath() + @"\PS2Emu\" + "param.sfo", Properties.Resources.param);
                #endregion <<Quick Sound/Video Extract >>

                #region << Boot Screen Settings >>

                if (Properties.Settings.Default.EnableBootScreen == true)
                {
                    //show bootlogo the good old ps2 classic logo and sound :P
                    this.Hide();

                    VideoScreen PS2logo = new VideoScreen();
                    PS2logo.ShowDialog();

                    this.Show();
                }

                #endregion << Boot Screen Settings >>

                #region << Gui Music >>

                if (Properties.Settings.Default.EnableGuiMusic == true)
                {
                    btnMutePlaySound.Background = ImageBrushFromBitmap(Properties.Resources.icon_sound_mute);
                    SoundClass.PlayPS4Sound(SoundClass.Sound.PS4_Music);
                }

                if(DateTime.Now.Day == 25 && DateTime.Now.Month == 12)
                {
                    SoundClass.PlayPS4Sound(SoundClass.Sound.LQ);
                    var timer = new DispatcherTimer { Interval = TimeSpan.FromMilliseconds(10) };
                    timer.Tick += (s, arg) => Snow();
                    timer.Start();
                }
               
                #endregion << Gui Music >>

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
                PS2ClassicsSfo.SFO sfo = new PS2ClassicsSfo.SFO(AppCommonPath() + @"\PS2Emu\" + "param.sfo");

                UpdateInfo("Setting Content ID");
                //all we want to change is the Content ID which will rename the package 
                txtContentID.Text = sfo.ContentID.ToString().Trim().Substring(7, 9);


                #endregion << Begin Maintaining the form and getting everything ready >>

                #region << Disable First Time >>

                Properties.Settings.Default.FirstTime = false;
                Properties.Settings.Default.FontInstalled = true;
                Properties.Settings.Default.Save();

                #endregion << Disable First Time >>
            }
            catch (Exception ex)
            {

                System.Windows.Forms.MessageBox.Show(ex.Message);
            }
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
                Application.Current.Dispatcher.Invoke((Action)delegate {
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
                UpdateInfo("Copying " + AppCommonPath() + @"\PS2Emu\" + "sfo.xml to " + AppCommonPath() + @"\Working\" + "sfo.xml");
                File.Copy(AppCommonPath() + @"\PS2Emu\" + "sfo.xml", AppCommonPath() + @"\Working\" + "sfo.xml", true);

                //now we need to prase it and change it 

                UpdateString("Gathering GP4 Info");
                UpdateInfo("Gathering GP4 Info");
                //create new XML Document 
                xmldoc = new XmlDataDocument();
                //nodelist 
                XmlNodeList xmlnode;

                #region << SFO Handeling >>

                #region << OLD CODE >>

                ////load the xml file from the base directory
                //UpdateInfo("Loading SFO as xml");
                //xmldoc.Load(AppCommonPath() + @"\Working\" + "sfo.xml");

                ////now load the nodes
                //xmlnode = xmldoc.GetElementsByTagName("paramsfo");//volume is inside the xml
                //UpdateInfo("Update Content ID and other ifno in SFO");                                           //loop to get all info from the node list
                //foreach (XmlNode xn in xmlnode)
                //{
                //    XmlNode xNode = xn.SelectSingleNode("CONTENT_ID");
                //    XmlNodeList nodes = xmldoc.SelectNodes("//param[@key='CONTENT_ID']");
                //    if (nodes != null)
                //    {

                //        if (isoFiles.Count > 1)
                //        {
                //            System.Windows.Application.Current.Dispatcher.Invoke(
                //       DispatcherPriority.Normal,
                //       (ThreadStart)delegate
                //       {
                //           xmlcontentid = "UP9000-" + txtContentID.Text.Trim() + "_00-" + txtContentID.Text.Trim() + "0000001";//make this the same no ps2 id required
                //       });
                //            nodes[0].InnerText = xmlcontentid;
                //        }
                //        else
                //        {
                //            System.Windows.Application.Current.Dispatcher.Invoke(
                //        DispatcherPriority.Normal,
                //        (ThreadStart)delegate
                //        {
                //            xmlcontentid = "UP9000-" + txtContentID.Text.Trim() + "_00-" + PS2ID.Replace("_", "") + "0000001";
                //        });
                //            nodes[0].InnerText = xmlcontentid;
                //        }
                //        //});
                //    }
                //    nodes = xmldoc.SelectNodes("//param[@key='TITLE']");
                //    if (nodes != null)
                //    {
                //        System.Windows.Application.Current.Dispatcher.Invoke(
                //        DispatcherPriority.Normal,
                //        (ThreadStart)delegate
                //        {
                //            nodes[0].InnerText = txtTitle.Text.Trim();
                //        });
                //    }
                //    nodes = xmldoc.SelectNodes("//param[@key='TITLE_ID']");
                //    if (nodes != null)
                //    {
                //        System.Windows.Application.Current.Dispatcher.Invoke(
                //        DispatcherPriority.Normal,
                //        (ThreadStart)delegate
                //        {
                //            nodes[0].InnerText = txtContentID.Text.Trim();
                //        });
                //    }
                //    for (int i = 1; i < 7; i++)
                //    {
                //        //fix the enter key issue i have found in some in
                //        nodes = xmldoc.SelectNodes("//param[@key='SERVICE_ID_ADDCONT_ADD_" + i + "']");
                //        if (nodes != null)
                //        {
                //            nodes[0].InnerText = string.Empty;
                //        }
                //    }
                //}
                ////save this into the working folder
                //xmldoc.Save(AppCommonPath() + @"\Working\" + "sfo.xml");

                #endregion << OLD CODE >>

                #region << PARAM.SFO EDITOR CODE >>

                Param_SFO.PARAM_SFO sfo = new Param_SFO.PARAM_SFO(Properties.Resources.param);//load param directly from resources

                #region << Set Content ID >>

                if (isoFiles.Count > 1)
                {
                    System.Windows.Application.Current.Dispatcher.Invoke(
               DispatcherPriority.Normal,
               (ThreadStart)delegate
               {
                   for (int i = 0; i < sfo.Tables.Count; i++)
                   {
                       if (sfo.Tables[i].Name == "CONTENT_ID")
                       {
                           var tempitem = sfo.Tables[i];
                           tempitem.Value = "UP9000-" + txtContentID.Text.Trim() + "_00-" + txtContentID.Text.Trim() + "0000001";//make this the same no ps2 id required
                           sfo.Tables[i] = tempitem;
                       }
                   }
               });
                }
                else
                {
                    System.Windows.Application.Current.Dispatcher.Invoke(
                DispatcherPriority.Normal,
                (ThreadStart)delegate
                {

                    xmlcontentid = "UP9000-" + txtContentID.Text.Trim() + "_00-" + PS2ID.Replace("_", "") + "0000001";
                    for (int i = 0; i < sfo.Tables.Count; i++)
                    {
                        if (sfo.Tables[i].Name == "CONTENT_ID")
                        {
                            var tempitem = sfo.Tables[i];
                            tempitem.Value = xmlcontentid;
                            sfo.Tables[i] = tempitem;
                        }
                    }
                });

                }

                #endregion << Set Content ID >>

                #region << Set Title >>

                System.Windows.Application.Current.Dispatcher.Invoke(
                       DispatcherPriority.Normal,
                       (ThreadStart)delegate
                       {
                           for (int i = 0; i < sfo.Tables.Count; i++)
                           {
                               if (sfo.Tables[i].Name == "TITLE")
                               {
                                   var tempitem = sfo.Tables[i];
                                   tempitem.Value = txtTitle.Text.Trim();
                                   sfo.Tables[i] = tempitem;
                               }
                           }

                       });

                #endregion << Set Title >>

                #region << Title ID >>

                System.Windows.Application.Current.Dispatcher.Invoke(
                DispatcherPriority.Normal,
                (ThreadStart)delegate
                {
                    for (int i = 0; i < sfo.Tables.Count; i++)
                    {
                        if (sfo.Tables[i].Name == "TITLE_ID")
                        {
                            var tempitem = sfo.Tables[i];
                            tempitem.Value = txtContentID.Text.Trim();
                            sfo.Tables[i] = tempitem;
                        }
                    }
                });

                #endregion << Title Id >>

                //change all the items we need changed

                #endregion << PARAM.SFO EDITOR CODE >>

                #endregion << SFO Handeling >>

                UpdateString("Creating GP4 Project");

                UpdateString("Looking for Custom PS2 LUA And Config from kozarovv");
                if (Properties.Settings.Default.EnableCustomConfigFetching == true)
                {
                    try
                    {
                        //here we get some patches from our friend https://twitter.com/kozarovv
                        SearchGithubForCorrespondingPatches(OriginalPS2ID);
                    }
                    catch (Exception ex)
                    {

                    }
                }

                SaveGp4();


                UpdateString("Creating SFO File");
                //We no longer want to call any orbis functions so we are moving this to param.sfo items

                //now call orbis and create sfo
                //Orbis_CMD("", "sfo_create \"" + AppCommonPath() + @"\Working\" + "sfo.xml" + "\" \"" + AppCommonPath() + @"\Working\" + "param.sfo" + "\"");

                sfo.SaveSFO(sfo, AppCommonPath() + @"\Working\" + "param.sfo");

                //move SFO to main directory with locations of new images 

                UpdateString("Moving SFO File");
                File.Copy(AppCommonPath() + @"\Working\" + "param.sfo", AppCommonPath() + @"\PS2\sce_sys\param.sfo", true);
                //now move ISO

                //save images

                System.Windows.Application.Current.Dispatcher.Invoke(
                        DispatcherPriority.Normal,
                        (ThreadStart)delegate
                        {
                            BitmapSource sr = Icon0.Source as BitmapSource;
                            Bitmap converted = GetBitmap(sr);
                            converted = ResizeImage(converted, 512, 512);//converts the image to the correct size
                            converted = ConvertTo24bpp(converted);//converts image to 24bpp
                            converted.Save(AppCommonPath() + @"PS2\sce_sys\icon0.png", System.Drawing.Imaging.ImageFormat.Png);
                        });

                System.Windows.Application.Current.Dispatcher.Invoke(
                        DispatcherPriority.Normal,
                        (ThreadStart)delegate
                        {
                            ImageBrush b = (ImageBrush)BackgroundImage.Background;
                            BitmapSource src = (BitmapSource)b.ImageSource;

                            Bitmap converted = GetBitmap(src);
                            converted = ResizeImage(converted, 1920, 1080);//converts the image to the correct size
                            converted = ConvertTo24bpp(converted);//converts image to 24bpp
                            converted.Save(AppCommonPath() + @"PS2\sce_sys\pic1.png", System.Drawing.Imaging.ImageFormat.Png);
                        });
                UpdateString("Moving Custom PS2 Config");
                //add custom config
                if (AddCustomPS2Config == true && CustomConfigLocation != string.Empty)
                {
                    //move custom ps2 classics config
                    UpdateString("Copying Custom config");
                    File.Copy(CustomConfigLocation, AppCommonPath() + @"PS2\config-emu-ps4.txt", true);//overwrite the file
                }


                UpdateString("Creating Custom PS2 LUA And Config");
                if (PS2CutomLua.Count > 1)
                {
                    //this function no longer is in use we use koz patches now
                    //for (int i = 0; i < PS2CutomLua.Count; i++)
                    //{
                    //    //we write all text to a file
                    //    if (!Directory.Exists(AppCommonPath() + @"PS2\patches\"))
                    //    {
                    //        Directory.CreateDirectory(AppCommonPath() + @"PS2\patches\");
                    //    }

                    //    File.WriteAllText(AppCommonPath() + @"PS2\patches\"+PS2TitleId[i].ToString()+ "_cli.conf",PS2CutomLua[i].ToString());
                    //}
                    //for (int i = 0; i < PS2CutomLua.Count; i++)
                    //{
                    //    //we write all text to a file
                    //    if (!Directory.Exists(AppCommonPath() + @"PS2\lua_include\"))
                    //    {
                    //        Directory.CreateDirectory(AppCommonPath() + @"PS2\lua_include\");
                    //    }

                    //    File.WriteAllText(AppCommonPath() + @"PS2\lua_include\" + PS2TitleId[i].ToString().Replace(".","") + "_config.lua", "apiRequest(0.1)"/*write custom data here for now just plain and simple stuff*/);
                    //}
                }
                //now we need to check the config file
                if (isoFiles.Count > 1)
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
                            string texttoreplace = textfile.ToString().Substring(start, end - start);
                            textfile = textfile.Replace(Is + texttoreplace, @"--max-disc-num=" + isoFiles.Count);
                        }
                    }

                    textfile = textfile.Replace(@"#--path-patches=""/app0/patches""", @"--path-patches=""/app0/patches""");//add patches
                    textfile = textfile.Replace(@"#--path-featuredata=""/app0/patches""", @"--path-featuredata=""/app0/patches""");//add featuredata
                    textfile = textfile.Replace(@"#--path-toolingscript=""/app0/patches""", @"--path-toolingscript=""/app0/patches""");//#--path-toolingscript=""/app0/patches"""
                    File.WriteAllText(AppCommonPath() + @"PS2\config-emu-ps4.txt", textfile);
                }

                //move iso
                UpdateString("Moving ISO File This May Take Some Time");
                BusyCoping = true;
                new System.Threading.Thread(new System.Threading.ThreadStart(delegate
                {
                    for (int i = 0; i < MainWindow.isoFiles.Count; i++)
                    {
                        if (MainWindow.isoFiles.Count > 1)
                        {
                            //we need to do our check here and below because if it is a cue file it needs to be changed
                            if (System.IO.Path.GetExtension(MainWindow.isoFiles[i].ToString()).ToUpper() == ".ISO")
                            {
                                UpdateString("Moving ISO File " + (i + 1) + "/" + MainWindow.isoFiles.Count + " This May Take Some Time");

                                //CopyFileWithProgress(txtPath.Text.Trim(), AppCommonPath() + @"\PS2\image\disc01.iso");
                                File.Copy(isoFiles[i].ToString().Trim(), AppCommonPath() + @"\PS2\image\disc" + String.Format("{0:D2}", i + 1) + ".iso", true);
                            }
                            else//its a cue file
                            {
                                UpdateString("Creating ISO File from Cue/Bin " + (i + 1) + "/" + MainWindow.isoFiles.Count + " This May Take Some Time");

                                //CopyFileWithProgress(txtPath.Text.Trim(), AppCommonPath() + @"\PS2\image\disc01.iso");
                                //File.Copy(isoFiles[i].ToString().Trim(), AppCommonPath() + @"\PS2\image\disc" + String.Format("{0:D2}", i + 1) + ".iso", true);
                                if (File.Exists(AppCommonPath() + @"\PS2\image\disc" + String.Format("{0:D2}", i + 1) + ".iso"))
                                {
                                    File.Delete(AppCommonPath() + @"\PS2\image\disc" + String.Format("{0:D2}", i + 1) + ".iso");
                                }
                                PS2_Tools.Backups.Bin_Cue.Convert_To_ISO(MainWindow.isoFiles[i].ToString(), AppCommonPath() + @"\PS2\image\disc" + String.Format("{0:D2}", i + 1));
                            }

                        }
                        else
                        {
                            //clean up the blank file
                            File.Delete(AppCommonPath() + @"\PS2\image\disc01.iso");
                            //CopyFileWithProgress(txtPath.Text.Trim(), AppCommonPath() + @"\PS2\image\disc01.iso");
                            string currentimage = "";
                            txtPath.Dispatcher.Invoke(
    DispatcherPriority.Normal,
    (ThreadStart)delegate { currentimage = txtPath.Text.Trim(); });
                            if (System.IO.Path.GetExtension(MainWindow.isoFiles[i].ToString()).ToUpper() == ".ISO")
                            {
                                File.Copy(currentimage, AppCommonPath() + @"\PS2\image\disc" + String.Format("{0:D2}", i + 1) + ".iso", true);
                            }
                            else
                            {
                                UpdateString("Creating ISO File from Bin/Cue This May Take Some Time");
                                PS2_Tools.Backups.Bin_Cue.Convert_To_ISO(currentimage, AppCommonPath() + @"\PS2\image\disc" + String.Format("{0:D2}", i + 1));
                            }
                            BusyCoping = false;

                        }
                    }

                    BusyCoping = false;
                })).Start();

                while (BusyCoping == true)
                {
                    DoEvents();
                }

                //now create pkg 

                UpdateString("Creating PS4 PKG");
                BusyCoping = true;
                //System.Windows.Application.Current.Dispatcher.Invoke(
                //        DispatcherPriority.Normal,
                //        (ThreadStart)delegate
                //        {
                //LibOrbisPkg.GP4.Gp4Project project = LibOrbisPkg.GP4.Gp4Project.ReadFrom(new FileStream(AppCommonPath() + @"\PS2Emu\" + "PS2Classics.gp4", FileMode.Open));
                //LibOrbisPkg.PKG.PkgProperties props = LibOrbisPkg.PKG.PkgProperties.FromGp4(project, AppCommonPath() + @"\PS2\");
                //LibOrbisPkg.PFS.PfsProperties pfsprops = LibOrbisPkg.PFS.PfsProperties.MakeInnerPFSProps(props);
                //LibOrbisPkg.PKG.PkgBuilder builder = new LibOrbisPkg.PKG.PkgBuilder(props);
                //LibOrbisPkg.PFS.PfsBuilder pfsbuilder = new LibOrbisPkg.PFS.PfsBuilder(pfsprops);
                //builder.BuildPkg(pfsbuilder.CalculatePfsSize());
                //LibOrbisPkg.PKG.PkgWriter writer = new LibOrbisPkg.PKG.PkgWriter();
                //builder.Write(saveFileDialog1.SelectedPath + @"\" + props.ContentId + ".pkg");
                if (Properties.Settings.Default.UseLibOrbisPkg == false)
                {
                    Orbis_CMD("", "img_create --oformat pkg \"" + AppCommonPath() + @"\PS2Emu\" + "PS2Classics.gp4\" \"" + saveFileDialog1.SelectedPath + "\"");
                    //orbis_pub_cmd.exe img_create --skip_digest --oformat pkg C:\Users\3deEchelon\AppData\Roaming\Ps4Tools\PS2Emu\PS2Classics.gp4 C:\Users\3deEchelon\AppData\Roaming\Ps4Tools\PS2Emu\
                }
                else

                {

                    var proj = AppCommonPath() + @"\PS2Emu\" + "PS2Classics.gp4";

                    var project = Gp4Project.ReadFrom(File.OpenRead(proj));
                    var props = PkgProperties.FromGp4(project, System.IO.Path.GetDirectoryName(proj));
                    var outputPath = saveFileDialog1.SelectedPath;
                    try
                    {
                        new PkgBuilder(props).Write(System.IO.Path.Combine(
                          outputPath,
                          project.volume.Package.ContentId.ToString() + ".pkg"));
                    }
                    catch (Exception ex)
                    {

                    }
                }
                BusyCoping = false;
                // });

                while (BusyCoping == true)
                {
                    //Thread.Sleep(TimeSpan.FromSeconds(5));//sleep for 5 seconds
                    DoEvents();
                }

                UpdateString("Done Opening Location");
            }
            catch (Exception ex)
            {
                //System.Windows.Forms.MessageBox.Show(ex.Message);
                CustomMessageBox(ex.Message, "Error", PS4_MessageBoxButton.OK, MessageBoxImage.Error);
            }
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
            DeleteDirectory(AppCommonPath() + @"\PS2\");
            DeleteDirectory(AppCommonPath() + @"\PS2Emu\");

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

            new Thread(() =>
            {
                Thread.CurrentThread.IsBackground = true;
                /* run your code here */
                Thread.Sleep(TimeSpan.FromSeconds(2));
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
            if (isoFiles.Count == 0)
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
            CustomMessageBox("PS4 Emulation is currently not Possible :P we will have to wait and see what AlexAltea comes up with", "PS4 Emulation", PS4_MessageBoxButton.OK, MessageBoxImage.Information);
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

        public class GithubRepoFolder
        {
            public string FolderName { get; set; }
            public string FolderMessage { get; set; }
            public string FolderAge { get; set; }

            public string FolderUrl { get; set; }
        }

        public List<GithubRepoFolder> RepoFolders = new List<GithubRepoFolder>();

        public void SearchGithubForCorrespondingPatches(string PS2TitleId)
        {
            /*Code from this */
            string HTMLCodeofsite;
            using (WebClient client = new WebClient())
            {
                System.Net.ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Tls | System.Net.SecurityProtocolType.Tls11 | System.Net.SecurityProtocolType.Tls12;
                client.Headers.Add("user-agent", "Only a test!");
                string urlfrom = Url.Replace("../", "/").Replace("//", "/").Replace("\\", "/");
                if (urlfrom.Contains("https:/"))
                    urlfrom = urlfrom.Replace("https:/", "https://");
                if (urlfrom.Contains("http:/"))
                    urlfrom = urlfrom.Replace("http:/", "http://");
                Uri urltodownload = new Uri(urlfrom);
                HTMLCodeofsite = client.DownloadString(urltodownload);



            }

            //HTMLCodeofsite = webBrowser1.DocumentText;



            if (HTMLCodeofsite != string.Empty)
            {
                //we found the code now we mine 
                /*now we read line by line*/
                string[] array = HTMLCodeofsite.Split("\r\n".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);

                foreach (String item in array)
                {
                    GithubRepoFolder folderrepo = new GithubRepoFolder();
                    if (item.Contains(@"href") && item.Contains(@"kozarovv/PS2-Configs/tree/master/"))
                    {
                        /*try and get the title*/
                        try
                        {
                            int starrstring = item.IndexOf(@"title=""");
                            int endstring = item.IndexOf(@"id=");
                            string substring = item.Substring(starrstring, endstring - starrstring);
                            folderrepo.FolderName = substring.Replace("title=", "").Replace("\"", "");

                        }
                        catch
                        {

                        }
                        /*try and get url*/
                        try
                        {
                            int starrstring = item.IndexOf(@"href=""");
                            int endstring = item.IndexOf(@""">" + folderrepo.FolderName.Replace(" ", ""));
                            string substring = item.Substring(starrstring, endstring - starrstring);
                            folderrepo.FolderUrl = substring.Replace(@"href=""", "").Replace("\"", "");

                            /*when url found add to list*/
                            if (!RepoFolders.Contains(folderrepo))
                            {
                                RepoFolders.Add(folderrepo);
                            }
                        }
                        catch
                        {

                        }
                    }
                }

                /*after looping is done we get those pages and just look for a possible patch*/
                for (int i = 0; i < RepoFolders.Count; i++)
                {


                    using (WebClient client = new WebClient())
                    {
                        System.Net.ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Tls | System.Net.SecurityProtocolType.Tls11 | System.Net.SecurityProtocolType.Tls12;
                        client.Headers.Add("user-agent", "Only a test!");
                        string urlfrom = "https:/github.com/" + RepoFolders[i].FolderUrl;
                        if (urlfrom.Contains("https:/"))
                            urlfrom = urlfrom.Replace("https:/", "https://");
                        if (urlfrom.Contains("http:/"))
                            urlfrom = urlfrom.Replace("http:/", "http://");
                        Uri urltodownload = new Uri(urlfrom);
                        HTMLCodeofsite = client.DownloadString(urltodownload);
                        //once we have the code try and search for the id inside the page 
                        if (HTMLCodeofsite.Contains(PS2TitleId))
                        {
                            UpdateString("Found custom " + RepoFolders[i].FolderName + "\nTrying to download");
                            //winner winner chicken dinner
                            //open that page and see if we can get the dam raw data from there 
                            //find the tag location with the ps2id
                            string[] arrayofcode = HTMLCodeofsite.Split("\r\n".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);

                            foreach (String item in arrayofcode)
                            {
                                if (item.Contains(@"href") && item.Contains(PS2TitleId))
                                {
                                    /*download htmlstring*/
                                    /*try and get url*/
                                    try
                                    {
                                        string ifoundit = item;
                                        int starrstring = item.IndexOf(@"href=""");
                                        int endstring = item.IndexOf(@""">" + PS2TitleId.Replace(" ", ""));
                                        string substring = item.Substring(starrstring, endstring - starrstring);
                                        string FileUrl = substring.Replace(@"href=""", "").Replace("\"", "");
                                        System.Net.ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Tls | System.Net.SecurityProtocolType.Tls11 | System.Net.SecurityProtocolType.Tls12;
                                        client.Headers.Add("user-agent", "Only a test!");
                                        urlfrom = "https:/github.com/" + FileUrl;
                                        if (urlfrom.Contains("https:/"))
                                            urlfrom = urlfrom.Replace("https:/", "https://");
                                        if (urlfrom.Contains("http:/"))
                                            urlfrom = urlfrom.Replace("http:/", "http://");
                                        Uri urltodownloadfile = new Uri(urlfrom);
                                        string filetodownloadhtml = client.DownloadString(urltodownloadfile);

                                        //now we need to find the raw. file location
                                        string[] arrayofcodetogetraw = filetodownloadhtml.Split("\r\n".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                                        foreach (String items in arrayofcodetogetraw)
                                        {
                                            if (items.Contains(@"raw-url"))
                                            {
                                                starrstring = items.IndexOf(@"href=""");
                                                endstring = items.IndexOf(@">Raw</a>");
                                                substring = items.Substring(starrstring, endstring - starrstring);
                                                FileUrl = substring.Replace(@"href=""", "").Replace("\"", "");
                                                string rawurl = items;
                                                urlfrom = "https:/github.com/" + FileUrl;
                                                if (urlfrom.Contains("https:/"))
                                                    urlfrom = urlfrom.Replace("https:/", "https://");
                                                if (urlfrom.Contains("http:/"))
                                                    urlfrom = urlfrom.Replace("http:/", "http://");
                                                urltodownloadfile = new Uri(urlfrom);
                                                string ActualRawFile = client.DownloadString(urltodownloadfile);

                                                //now we need to decide how to save these items
                                                //speaking to kozarovv right now checking into github cause i want to format my pc

                                                if (!Directory.Exists(AppCommonPath() + @"PS2\lua_include\"))
                                                {
                                                    Directory.CreateDirectory(AppCommonPath() + @"PS2\lua_include\");
                                                }

                                                File.WriteAllText(AppCommonPath() + @"\PS2\lua_include\" + PS2TitleId.Replace(".", "") + @"_config.lua", ActualRawFile);

                                                AddCustomPS2Config = true;
                                                PS2CutomLua.Add(PS2TitleId.Replace(".", ""));

                                            }

                                        }
                                    }
                                    catch
                                    {

                                    }
                                }
                            }
                        }
                    }
                }
            }

        }

        public bool doesStringMatch()
        {
            if (isoFiles.Count == 0)
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

            for (int i = 0; i < MainWindow.isoFiles.Count; i++)
            {
                builder += tempval.Replace("disc01.iso", "disc0" + (i + 1) + ".iso") + "\n";
            }

            var alllines = File.ReadAllText(AppCommonPath() + @"\PS2Emu\" + "PS2Classics.gp4");

            alllines = alllines.Replace(tempval, builder.Remove(builder.Length - 1, 1));

            File.WriteAllText(AppCommonPath() + @"\PS2Emu\" + "PS2Classics.gp4", alllines);

            #region << Configs >>
            if (isoFiles.Count > 1)
            {
                //line builder
                tempval = @"    <file targ_path=""patches/SLES-50366_cli.conf"" orig_path=""..\PS2\patches\SLES-50366_cli.conf""" + @" />";
                builder = string.Empty;

                for (int i = 0; i < MainWindow.PS2CutomLua.Count; i++)
                {
                    builder += tempval.Replace("SLES-50366_cli.conf", PS2TitleId[i].ToString() /*Game Name Here*/+ "_cli.conf") + "\n";
                }

                alllines = File.ReadAllText(AppCommonPath() + @"\PS2Emu\" + "PS2Classics.gp4");

                alllines = alllines.Replace("@addps2patchHere", builder.Remove(builder.Length - 1, 1));

                File.WriteAllText(AppCommonPath() + @"\PS2Emu\" + "PS2Classics.gp4", alllines);


                tempval = @"    <file targ_path=""lua_include/SLUS-20071_config.lua"" orig_path=""..\PS2\lua_include\SLUS-20071_config.lua""" + @" />";
                builder = string.Empty;

                for (int i = 0; i < MainWindow.PS2CutomLua.Count; i++)
                {
                    builder += tempval.Replace("SLUS-20071_config.lua", PS2TitleId[i].ToString().Replace(".","") /*Game Name Here*/+ "_config.lua") + "\n";
                }

                alllines = File.ReadAllText(AppCommonPath() + @"\PS2Emu\" + "PS2Classics.gp4");

                alllines = alllines.Replace("@addps2luhere", builder.Remove(builder.Length - 1, 1));

                File.WriteAllText(AppCommonPath() + @"\PS2Emu\" + "PS2Classics.gp4", alllines);

            }
            else
            {
                alllines = alllines.Replace("@addps2patchHere", "");//remove the string
                alllines = alllines.Replace("@addps2luhere", "");//remove the string
                File.WriteAllText(AppCommonPath() + @"\PS2Emu\" + "PS2Classics.gp4", alllines);
            }
            #endregion << Configs >>
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
            if (Properties.Settings.Default.UseSpesifcEmu != "Jax and Daxter")
                System.IO.File.WriteAllBytes(AppCommonPath() + @"\PS2Emu\" + "PS2Classics.gp4", Properties.Resources.PS2Classics);
            else
                System.IO.File.WriteAllBytes(AppCommonPath() + @"\PS2Emu\" + "PS2Classics.gp4", Properties.Resources.JaxAndDax);
            UpdateInfo("Writing Binary File to Temp Path " + "\n Written : " + AppCommonPath() + @"\PS2Emu\" + "PS2Classics.gp4");
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
            System.IO.File.WriteAllText(AppCommonPath() + @"\PS2Emu\" + "sfo.xml", Properties.Resources.sfo);
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

            //we need to clean up the PS2 Folder if settings have changed
            if(Properties.Settings.Default.UseSpesifcEmu == "Jax and Daxter")
            {
                //user changed to Jax and Daxter
                
                DeleteDirectory(AppCommonPath() + @"\PS2\");
                if (!File.Exists(AppCommonPath() + "Jax and Daxter.zip"))
                {
                    CustomMessageBox("Please redownload Jax and Daxter from Settings it seems the zip file is missing", "Error", PS4_MessageBoxButton.OK, MessageBoxImage.Error);
                    System.Windows.Application.Current.Shutdown();
                }
                if(Directory.Exists(AppCommonPath() + "\\Jax and Daxter"))
                {
                    DeleteDirectory(AppCommonPath() + "\\Jax and Daxter");
                }
                ZipFile.ExtractToDirectory(AppCommonPath() + "Jax and Daxter.zip", AppCommonPath() + "\\Jax and Daxter");
                Directory.Move(AppCommonPath() + "\\Jax and Daxter", AppCommonPath() + @"\PS2\");
                if(!Directory.Exists(AppCommonPath() + @"\PS2\image"))
                {
                    Directory.CreateDirectory(AppCommonPath() + @"\PS2\image");
                }
            }
        }

        public static void DeleteDirectory(string target_dir)
        {
            try
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
            catch(Exception ex)
            {
                //we dont log anything here it should be okay
            }
        }

        public static string AppCommonPath()
        {
            string returnstring = "";
            if (Properties.Settings.Default.OverwriteTemp == true && Properties.Settings.Default.TempPath != string.Empty)
            {
                returnstring = Properties.Settings.Default.TempPath + @"\Ps4Tools\";
                if (!Directory.Exists(returnstring))
                {
                    Directory.CreateDirectory(returnstring);
                }
            }
            else
            {
                returnstring = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\Ps4Tools\";
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
                    if (result.Contains("already converted from elf file to self file"))
                    {
                        System.Windows.Forms.DialogResult dlr = System.Windows.Forms.MessageBox.Show("Already Converted From Elf Error Found.... will be using Orbis-pub-gen for this pkg\n\n Simply Click Build and select the save folder", "Error with an alternative", System.Windows.Forms.MessageBoxButtons.OKCancel, System.Windows.Forms.MessageBoxIcon.Question);
                        if (dlr == System.Windows.Forms.DialogResult.OK)
                        {
                            //this will open up the GP4 Project inside the Utility
                            Orbis_Pub__GenCMD("", AppCommonPath() + @"\PS2Emu\" + "PS2Classics.gp4");

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

        private void MenuItem_Click_8(object sender, RoutedEventArgs e)
        {
            try
            {
                SoundClass.PlayPS4Sound(SoundClass.Sound.PS4_Info_Pannel_Sound);
                PS4_PS2_Classics_Gui__WPF_.HowToUse.HowToUse thisisatutorial = new PS4_PS2_Classics_Gui__WPF_.HowToUse.HowToUse();
                thisisatutorial.ShowDialog();
            }
            catch(Exception ex)
            {

            }
        }

        private void MenuItem_Click_9(object sender, RoutedEventArgs e)
        {
            //Play Sound For Sound Pannel
            SoundClass.PlayPS4Sound(SoundClass.Sound.PS4_Info_Pannel_Sound);
            //Open new Window
            config_emu_ps4 configeditor = new config_emu_ps4();
            configeditor.ShowDialog();
        }

        private void MenuItem_Click_10(object sender, RoutedEventArgs e)
        {
            var fileDialog = new System.Windows.Forms.OpenFileDialog();
            fileDialog.Filter = "Image files (*.jpg, *.jpeg, *.jpe, *.jfif, *.png) | *.jpg; *.jpeg; *.jpe; *.jfif; *.png";
            var result = fileDialog.ShowDialog();      
            switch (result)
            {
                case System.Windows.Forms.DialogResult.OK:
                    var file = fileDialog.FileName;
                    BitmapImage image = new BitmapImage();
                    image.BeginInit();
                    image.UriSource = new Uri(file);
                    image.EndInit();
                    Icon0.Source = image;
                    Icon0.Stretch = Stretch.Fill;
                    break;
                case System.Windows.Forms.DialogResult.Cancel:
                default:
                 
                    break;
            }

            var ps2item = PS2_Tools.PS2_Content.GetPS2Item(PS2ID.Replace("_", "-"));

          
        }

        private void MenuItem_Click_11(object sender, RoutedEventArgs e)
        {
            //

            CustomMessageBox(Properties.Resources.Release_Notes.ToString(), "Release Notes", PS4_MessageBoxButton.OK, MessageBoxImage.Asterisk);
        }
    }
}
