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
    public partial class OptionsList : Window
    {
        public OptionsList()
        {
            InitializeComponent();
        }

        List<string> _config = new List<string>();
        string Options = "";
        public OptionsList(string[] config,string _Options)
        {
            InitializeComponent();
            _config = config.ToList(); // this isn't going to be fast.
            Options = _Options;
        }

        long TotalSizeOfPkg = 0;

        private void btnRemove_Click(object sender, RoutedEventArgs e)
        {
            if (Options == "GS Uprender")
            {
                Constants.Config_Emu_PS4.Gs_Uprender item = Constants.Config_Emu_PS4.Gs_Uprender.Disabled;
                Enum.TryParse(listBox.SelectedItem.ToString(), out item);

                config_emu_ps4.config.gs_uprender = item;
            }
            if(Options == "GS Upscale")
            {
                Constants.Config_Emu_PS4.Gs_Upscale item = Constants.Config_Emu_PS4.Gs_Upscale.Disabled;
                Enum.TryParse(listBox.SelectedItem.ToString(), out item);

                config_emu_ps4.config.gs_upscale = item;
            }
            Close();        
            
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            LoadConfig();
        }

        public void LoadConfig()
        {
            for (int i = 0; i < _config.Count; i++)
            {
                listBox.Items.Add(_config[i].ToString());
            }
        }

        private void listBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                
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
