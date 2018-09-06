using DiscUtils.Iso9660;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Deployment.Application;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Threading;

namespace PS4_PSP_Classics_GUI
{
    /// <summary>
    /// Interaction logic for PSPLayoutIdea.xaml
    /// </summary>
    public partial class PSPLayoutIdea : Window
    {
        #region << Vars >>

        //Dialogs
        System.Windows.Forms.FolderBrowserDialog tempkeeper = null;

        //advanced window
        Advanced advanced = new Advanced();

        //VLC controls
        Vlc.DotNet.Forms.VlcControl tempvlc;

        //Background Workers
        private readonly BackgroundWorker bgWorkerVLC = new BackgroundWorker();
        private readonly BackgroundWorker bgWorkerSS = new BackgroundWorker();

        //Lists 
        public static List<string> pspfiles = new List<string>();

        //Strings
        public string PSPID = "";

        //Bools
        public static bool bgwClose;

        //WPF Controls
        TextBox txtContentID = new TextBox();

        #region <<File Types and Instancase >>

        PSP_Tools.Pbp pbp = new PSP_Tools.Pbp();

        #endregion << File Tyoes and Instances >>

        #endregion << Vars >>

        public PSPLayoutIdea()
        {
            InitializeComponent();
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
            return returnstring;
        }

        private void MediaPlayer_MediaEnded(object sender, RoutedEventArgs e)
        {
            MediaPlayer.Visibility = Visibility.Hidden;

            //ask user if they want to open a menu to load a pbp cso or a game
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            #region << Background Workers >>

            bgWorkerVLC.DoWork += BgWorkerVLC_DoWork;
            bgWorkerVLC.RunWorkerCompleted += BgWorkerVLC_RunWorkerCompleted;

            bgWorkerSS.DoWork += bgWorkerSS_DoWork;
            bgWorkerSS.WorkerSupportsCancellation = true;

            #endregion << Background Workers >>

           

            #region << Quick Sound/Video Extract >>


            System.IO.File.WriteAllBytes(AppCommonPath() + "PS4.mp3", Properties.Resources.ps4BGM);
            System.IO.File.WriteAllBytes(AppCommonPath() + "PSP.mp4", Properties.Resources.PSP_Logo);

            if (!Directory.Exists(AppCommonPath() + @"\PSPEmu\"))
            {
                UpdateInfo("Created Directory" + AppCommonPath() + @"\PSPEmu\");
                Directory.CreateDirectory(AppCommonPath() + @"\PSPEmu\");
            }
            System.IO.File.WriteAllBytes(AppCommonPath() + @"\PSPEmu\" + "param.sfo", Properties.Resources.param);

            #endregion <<Quick Sound/Video Extract >>

            #region << Boot Screen Settings >>

            if (Properties.Settings.Default.EnableBootScreen == true)
            {
                MediaPlayer.Visibility = Visibility.Visible;
                MediaPlayer.Source = new Uri(AppCommonPath() + "PSP.mp4");
                MediaPlayer.Play();
                MediaPlayer.MediaEnded += MediaPlayer_MediaEnded;
            }

            #endregion << Boot Screen Settings >>

            #region << Gui Music >>

            if (Properties.Settings.Default.EnableGuiMusic == true)
            {
                //btnMutePlaySound.Background = ImageBrushFromBitmap(Properties.Resources.icon_sound_mute);
                SoundClass.PlayPS4Sound(SoundClass.Sound.PS4_Music);
            }

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
            this.Title = string.Format("PSPHD GUI Version : {0}", v);

            

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
            PS2ClassicsSfo.SFO sfo = new PS2ClassicsSfo.SFO(AppCommonPath() + @"\PSPEmu\" + "param.sfo");

            UpdateInfo("Setting Content ID");
            //all we want to change is the Content ID which will rename the package 
            txtContentID.Text = sfo.ContentID.ToString().Trim().Substring(7, 9);


            #endregion << Begin Maintaining the form and getting everything ready >>

            #region << PMF and AT3 >>
            bgWorkerVLC.RunWorkerAsync();
            #endregion << PMF and AT3 >>
        }

        private void BgWorkerVLC_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {

        }

        private void BgWorkerVLC_DoWork(object sender, DoWorkEventArgs e)
        {
            #region << PMF and AT3 >>

            try
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
        new string[] { "input-repeat=65535" };
        var mediaOptions = new string[] { "input-repeat=-1" };
        tempvlc.VlcMediaplayerOptions = options;
        Views.VLC._WindowsFormsHost.Child = tempvlc;



    //this.WindowsFormsHost.Child = tempvlc;

    tempvlc.EndInit();

        Util.SoundClass.atr3vlc = new Vlc.DotNet.Forms.VlcControl();
        Util.SoundClass.atr3vlc.BeginInit();
        Util.SoundClass.atr3vlc.VlcLibDirectory = vlcLibDirectory;// /*libvlc's directory*/;
                                                                  //this.MyControl.VlcMediaplayerOptions = new[] { "-vv" };
    Util.SoundClass.atr3vlc.EndInit();

    });

            }
            catch(Exception ex)
            {

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

        private void txtContentID_Copy_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void btnStart_Click(object sender, RoutedEventArgs e)
        {
            Icon0.Visibility = Visibility.Visible;
            if (Util.SoundClass.atr3vlc.IsPlaying)
            {
                Util.SoundClass.atr3vlc.Stop();
            }
            if (tempvlc.IsPlaying)
            {
                tempvlc.Stop();
            }
            if (Properties.Settings.Default.EnablePMF == false)
            {
               Views.VLC._WindowsFormsHost.Visibility = Visibility.Hidden;
            }
            else
            {
                Icon0.Visibility = Visibility.Hidden;
            }
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
            thedialog.Filter = "PSP Image File (*.iso,*.cso,*.pbp,*.ISO,*.CSO,*.PBP)|*.iso;*.cso;*.pbp;*.ISO;*.CSO;*.PBP";
            //"Plain text files (*.csv;*.txt)|*.csv;*.txt";
            thedialog.Multiselect = false;//psp emu only supports 1.8Gig so we might as well only allow one iso/pbp/cso file
            thedialog.InitialDirectory = Environment.SpecialFolder.MyComputer.ToString();

            if (thedialog.ShowDialog() == true)
            {

                //get the file type 
                FileType.FileTypes type = FileType.LoadFileInfo(thedialog.FileName);

                if (type == FileType.FileTypes.Unknown)
                {
                    MessageBox ps4messagebox = new MessageBox("Unknown File Type Selected", "File Type Unknown", PS4_MessageBoxButton.OK, SoundClass.Sound.Error);
                    ps4messagebox.ShowDialog();
                    return;
                }

                if (type == FileType.FileTypes.CSO)
                {
                    MessageBox errormes = new MessageBox("CSO Files are currently not fully working\n\nShould be working in next update\n\nYou can manually decrypt the cso to iso", "CSO", PS4_MessageBoxButton.OK, SoundClass.Sound.Error);
                    errormes.ShowDialog();
                    return;
                }

                if (type == FileType.FileTypes.PBP)
                {
                    pbp = new PSP_Tools.Pbp();
                    pbp.LoadPbp(thedialog.FileName);

                    string DiscID = pbp.GetPBPDiscID();

                    pspfiles = thedialog.FileNames.ToList<string>();

                    byte[] array = pbp.ReadFileFromPBP(PSP_Tools.Pbp.DataType.ParamSfo);
                    if (array != null)
                    {
                        PSP_Tools.PARAM_SFO sfo = new PSP_Tools.PARAM_SFO(array);
                        if (sfo.DISC_ID != "")
                        {
                            txtContentID.Text = sfo.DISC_ID;//disc id of PSP Game
                        }
                        txtTitle.Text = sfo.Title;
                        byte[] Icon0Png = pbp.ReadFileFromPBP(PSP_Tools.Pbp.DataType.Icon0Png);
                        if (Icon0Png.Length != 0)
                        {
                            Icon0.Source = ToImage(Icon0Png);
                        }

                        byte[] Pic1Png = pbp.ReadFileFromPBP(PSP_Tools.Pbp.DataType.Pic1Png);
                        if (Pic1Png.Length != 0)
                        {
                            ImageBrush imgB = new ImageBrush();

                            BitmapImage btpImg = new BitmapImage();
                            btpImg.BeginInit();
                            btpImg.StreamSource = new MemoryStream(Pic1Png);
                            btpImg.EndInit();
                            imgB.ImageSource = btpImg;

                            xmbOverlay.Source = btpImg;
                        }
                    }
                    /*Data.psar = boot.bin ;)
                      data.psp = eboot.bin*/

                    //pbp.WritePDPFiles(AppCommonPath() + @"\PSP\",pspdata:"EBOOT.BIN",psrdata:"BOOT.BIN");
                    //we should probably only write out the data when the item is completed 

                    if (Properties.Settings.Default.EnablePMF == true)
                    {
                        ////play at3
                        byte[] atrac3 = pbp.ReadFileFromPBP(PSP_Tools.Pbp.DataType.Snd0At3);
                        if (atrac3.Length != 0)
                        {
                            Util.SoundClass.atr3vlc.Play(new MemoryStream(atrac3));
                            // Util.SoundClass.Init_SoundPlayer(atrac3);
                        }

                        byte[] pmf = pbp.ReadFileFromPBP(PSP_Tools.Pbp.DataType.Icon1Pmf);
                        if (pmf.Length != 0)
                        {
                            Icon0.Visibility = Visibility.Hidden;
                            tempvlc.Play(new MemoryStream(pmf));
                            // Util.SoundClass.Init_SoundPlayer(atrac3);
                        }
                    }
                }

                else if (type == FileType.FileTypes.ISO)
                {
                    //UpdateInfo("adding iso files to list");
                    pspfiles = thedialog.FileNames.ToList<string>();//tada now we know how many iso's there is 
                    if (pspfiles.Count > 1)
                    {

                        System.Windows.MessageBox.Show("Maximum amount of ISO's allowed by the PS4 in a PSPHD is 1");
                        pspfiles.Clear();
                        return;
                    }
                    if (pspfiles.Count == 1)
                    {
                        //UpdateInfo("Standard ISO Method");
                        #region << For Single File >>
                        //set the path and the text on the gui
                        string isopath = thedialog.FileName;

                        //now using the file stream we can read the CNF file
                        using (FileStream isoStream = File.OpenRead(isopath))
                        {
                            //use disk utils to read iso quickly
                            CDReader cd = new CDReader(isoStream, true);
                            //look for the spesific file(I Wanted to use UMD_DATA.BIN bit i decided not to since homebrew doesnt have that we should use sfo)
                            Stream fileStream = cd.OpenFile(@"PSP_GAME\PARAM.SFO", FileMode.Open);
                            // Use fileStream...
                            // Read SFO
                            PSP_Tools.PARAM_SFO sfo = new PSP_Tools.PARAM_SFO(fileStream);

                            string DiscID = sfo.DISC_ID.ToString();//read string to end this will read all the info we need


                            string PSPId = DiscID;

                            if (PSPId != string.Empty)
                            {
                                PSPID = PSPId.Replace(".", "");

                                if (Properties.Settings.Default.EnablePSPIDReplace == true)
                                {
                                    txtContentID.Text = PSPID.Replace("_", "");
                                }

                                txtTitle.Text = sfo.Title;

                                try
                                {
                                    Stream iconstream = cd.OpenFile(@"PSP_GAME\ICON0.PNG", FileMode.Open);

                                    byte[] Icon0Png = ReadFully(iconstream);
                                    if (Icon0Png.Length != 0)
                                    {
                                        Icon0.Source = ToImage(Icon0Png);
                                    }
                                }
                                catch (Exception ex)
                                {

                                }
                                try
                                {
                                    Stream iconstream = cd.OpenFile(@"PSP_GAME\PIC1.PNG", FileMode.Open);


                                    byte[] Pic1Png = ReadFully(iconstream);
                                    if (Pic1Png.Length != 0)
                                    {
                                        ImageBrush imgB = new ImageBrush();

                                        BitmapImage btpImg = new BitmapImage();
                                        btpImg.BeginInit();
                                        btpImg.StreamSource = new MemoryStream(Pic1Png);
                                        btpImg.EndInit();
                                        imgB.ImageSource = btpImg;

                                        xmbOverlay.Source = btpImg;
                                    }
                                }
                                catch (Exception ex)
                                {

                                }
                                try
                                {
                                    if (Properties.Settings.Default.EnablePMF == true)
                                    {

                                        ////play at3
                                        Stream atrac3stream = cd.OpenFile(@"PSP_GAME\SND0.AT3", FileMode.Open);


                                        byte[] atrac3 = ReadFully(atrac3stream);
                                        if (atrac3.Length != 0)
                                        {
                                            var mediaOptions = new string[] { "input-repeat=65535" };
                                            Util.SoundClass.atr3vlc.Play(new MemoryStream(atrac3), mediaOptions);
                                            // Util.SoundClass.Init_SoundPlayer(atrac3);

                                        }
                                    }
                                }
                                catch (Exception ex)
                                {

                                }

                                try
                                {
                                    if (Properties.Settings.Default.EnablePMF == true)
                                    {
                                        Stream pmfstream = cd.OpenFile(@"PSP_GAME\ICON1.PMF", FileMode.Open);
                                        Icon0.Visibility = Visibility.Hidden;
                                        byte[] pmf = ReadFully(pmfstream);
                                        if (pmf.Length != 0)
                                        {
                                            Icon0.Visibility = Visibility.Hidden;
                                            var mediaOptions = new string[] { "input-repeat=65535" };
                                            tempvlc.Play(new MemoryStream(pmf), mediaOptions);
                                            //tempvlc.Stopped += delegate
                                            //{

                                            //    var mediaOptions = new string[] { "input-repeat=65535" };
                                            //    tempvlc.Play(new MemoryStream(pmf),mediaOptions);
                                            //    tempvlc.Stopped += delegate
                                            //    {
                                            //        tempvlc.Play(new MemoryStream(pmf), mediaOptions);
                                            //    };
                                            //};
                                        }
                                    }
                                }
                                catch
                                {

                                }
                            }
                            else
                            {
                                MessageBoxResult msrlt = System.Windows.MessageBox.Show("Could not load PSP ID\n\n would you like to submit an issue ?", "Error Reporting", MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.Yes);
                                if (msrlt == MessageBoxResult.Yes)
                                {
                                    //load github issue page
                                    Process.Start(@"https://github.com/xXxTheDarkprogramerxXx/PS3Tools/issues");
                                }

                            }
                        }

                        #endregion << For Single File >>
                    }
                    else if (pspfiles.Count > 1)
                    {
                        //UpdateInfo("Multi ISO Method");
                        //UpdateInfo("Opening Mutli ISO Screen");
                        multi.Show();
                        //multi.SetDesktopLocation(this.Location.X + this.Size.Width, this.Location.Y);
                    }
                }
            }
        }


        #region << Methods >>

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

        /// <summary>
        /// Converts stream to byte array
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
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

        #endregion << Methods >>

        private void Icon_Drop(object sender, DragEventArgs e)
        {

        }

        private void psBUtton_Click(object sender, RoutedEventArgs e)
        {
            Environment.Exit(0);
            this.Close();
        }

        private void btnSelect_Click(object sender, RoutedEventArgs e)
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
    }
}
