using System;
using System.Collections.Generic;
using System.Linq;
using System.Resources;
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
    /// Interaction logic for VideoScreen.xaml
    /// </summary>
    public partial class VideoScreen : Window
    {
        public VideoScreen()
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

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            MediaPlayer.Source = new Uri(AppCommonPath() + "PS2.mp4");
            MediaPlayer.Play();
            MediaPlayer.MediaEnded += MediaPlayer_MediaEnded;

        }

        private void MediaPlayer_MediaEnded(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
