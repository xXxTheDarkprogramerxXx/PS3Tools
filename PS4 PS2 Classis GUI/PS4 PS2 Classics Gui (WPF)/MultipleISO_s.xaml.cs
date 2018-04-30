using DiscUtils.Iso9660;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace PS4_PS2_Classics_Gui__WPF_
{
    /// <summary>
    /// Interaction logic for MultipleISO_s.xaml
    /// </summary>
    public partial class MultipleISO_s : Window
    {
        public MultipleISO_s()
        {
            InitializeComponent();
        }

        long TotalSizeOfPkg = 0;

        private void btnRemove_Click(object sender, RoutedEventArgs e)
        {
            if (System.Windows.Forms.DialogResult.Yes == System.Windows.Forms.MessageBox.Show("You are about to delete an iso from your list. Are you sure you want to continue?", "Remove ISO", System.Windows.Forms.MessageBoxButtons.YesNo, System.Windows.Forms.MessageBoxIcon.Question))
            {
                //delete the iso
                MainWindow.isoFiles.RemoveAt(listBox.SelectedIndex);
            }
            LoadIsoFiles();
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            //add another iso here to the list
        }
        #region Calculation
        private string CalculateBytes(long count)
        {
            string[] sizeNames = { " B", " KB", " MB", " GB", " TB", " PB", " EB" };
            if (count == 0)
                return "0" + sizeNames[0];
            long bytes = Math.Abs(count);
            int log = Convert.ToInt32(Math.Floor(Math.Log(bytes, 1024)));
            double number = Math.Round(bytes / Math.Pow(1024, log), 1);
            return (Math.Sign(count) * number).ToString() + sizeNames[log];
        }

        #endregion Calcs

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            LoadIsoFiles();
        }

        public void LoadIsoFiles()
        {
            listBox.Items.Clear();
            TotalSizeOfPkg = 0;
           
            //foreach iso file
            for (int i = 0; i < MainWindow.isoFiles.Count; i++)
            {
                string File = MainWindow.isoFiles[i].ToString();
                listBox.Items.Add(System.IO.Path.GetFileName(File));
                CheckForCustomConfig(i);
                long length = new System.IO.FileInfo(MainWindow.isoFiles[i].ToString()).Length;
                TotalSizeOfPkg += length;
            }

            //after all the iso files are added add about 30mb
            lblSize.Content = CalculateBytes(TotalSizeOfPkg);

        }

        public void CheckForCustomConfig(int id)
        {
            bool FoundCustomConfig = false;
            string HTMLFile = string.Empty;
            string PS2Id = string.Empty;
            if (Properties.Settings.Default.EnableCustomConfigFetching == true)
            {
                using (FileStream isoStream = File.OpenRead(MainWindow.isoFiles[id].ToString()))
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
                        PS2Id = fullstring.ToString().Substring(start, end - start);
                        MainWindow.PS2TitleId.Add(PS2Id);
                    }
                }

                #region << Old Method >>
                //    //we need to read a site into a textfile 
                //    //we use the following 
                //    string Url = "https://github.com/Zarh/Get_CONFIG/tree/master/files";

                //HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Url);
                //ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;
                //HttpWebResponse response = (HttpWebResponse)request.GetResponse();

                //if (response.StatusCode == HttpStatusCode.OK)
                //{
                //    Stream receiveStream = response.GetResponseStream();
                //    StreamReader readStream = null;

                //    if (response.CharacterSet == null)
                //    {
                //        readStream = new StreamReader(receiveStream);
                //    }
                //    else
                //    {
                //        readStream = new StreamReader(receiveStream, Encoding.GetEncoding(response.CharacterSet));
                //    }

                //    HTMLFile = readStream.ReadToEnd();

                //    response.Close();
                //    readStream.Close();
                //}

                //foreach (var lineitem in HTMLFile.Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries))
                //{
                //    if(lineitem.Contains(PS2Id + ".CONFIG"))
                //    {
                //        string Download = "https://github.com/Zarh/Get_CONFIG/blob/master/files/" + PS2Id + ".CONFIG?raw=true";
                //        MainWindow.PS2CutomConfig.Add(Download);
                //    }
                //}
                #endregion << Old Method >>


                string forFile = @"--fpu-no-clamping=0
--fpu-clamp-operands=1
--fpu-clamp-results=1
--vu0-no-clamping=0
--vu1-no-clamping=0
--vu0-clamp-operands=1
--vu0-clamp-results=1
--vu1-clamp-operands=1
--vu1-clamp-results=1
--cop2-no-clamping=0
--cop2-clamp-operands=1
--cop2-clamp-results=1
--gs-use-clut-merge=1
--gs-kernel-cl=""clutmerge""
--gs-kernel-cl-up=""clutmerge2x2""";


                MainWindow.PS2CutomLua.Add(forFile);

            }
        }

        private void listBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                using (FileStream isoStream = File.OpenRead(MainWindow.isoFiles[listBox.SelectedIndex].ToString()))
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
                            lblTitleId.Content = PS2Id.Replace(".", "").Replace("_", "");
                        }
                        else
                        {
                            System.Windows.Forms.MessageBox.Show("Could not load PS2 ID");
                        }
                    }
                    else
                    {
                        System.Windows.Forms.DialogResult dlr = System.Windows.Forms.MessageBox.Show("Could not load PS2 ID\n\n wpuld you like to submit an issue ?", "Error Reporting", System.Windows.Forms.MessageBoxButtons.YesNo, System.Windows.Forms.MessageBoxIcon.Question, System.Windows.Forms.MessageBoxDefaultButton.Button2);
                        if (dlr == System.Windows.Forms.DialogResult.Yes)
                        {
                            //load github issue page
                            Process.Start(@"https://github.com/xXxTheDarkprogramerxXx/PS3Tools/issues");
                        }

                    }
                }
                SoundClass.PlayPS4Sound(SoundClass.Sound.Navigation);
            }
            catch (Exception ex)
            {

            }
        }

        private void listBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Up || e.Key == Key.Down || e.Key == Key.Right || e.Key == Key.Left)
            {
                SoundClass.PlayPS4Sound(SoundClass.Sound.Navigation);
            }
        }
    }
}
