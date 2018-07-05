using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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
using System.Windows.Shapes;

namespace PS4_PS2_Classics_Gui__WPF_
{
    /// <summary>
    /// Interaction logic for SettingsWindow.xaml
    /// </summary>
    public partial class SettingsWindow : Window
    {
        public SettingsWindow()
        {
            InitializeComponent();
        }
        bool LoadCompleted = false;
        bool SkipSettings = false;

        int CurrentIdx = 0;
        MouseButtonEventArgs ButtonLick;


        public class SettingValue
        {
            public string SettingName { get; set; }

            public string SValue { get; set; }

            public Control type { get; set; }

        }

        List<SettingValue> Values = new List<SettingValue>();


        public void AddEsttingValues(string SettingName,string DefaultValue, Control TypeOfContorl)
        {
            SettingValue set = new SettingValue();
            set.SettingName = SettingName;
            switch (SettingName)
            {
                case "Overwrite Temp Folder":
                    if(Properties.Settings.Default.OverwriteTemp == true)
                    {
                        DefaultValue = "On";
                    }
                    else
                    {
                        DefaultValue = "Off";
                    }
                    break;
                case "Temp Path":
                    if(Properties.Settings.Default.TempPath != "")
                    {
                        DefaultValue = Properties.Settings.Default.TempPath;
                    }
                    else
                    {
                        DefaultValue = "";
                    }
                    break;
                case "Replace NP Title ID With PS2 Title ID":
                    if (Properties.Settings.Default.EnablePS2IDReplace == true)
                    {
                        DefaultValue = "On";
                    }
                    else
                    {
                        DefaultValue = "Off";
                    }
                    break;
                case "Enable Boot Logo":
                    if (Properties.Settings.Default.EnableBootScreen == true)
                    {
                        DefaultValue = "On";
                    }
                    else
                    {
                        DefaultValue = "Off";
                    }
                    break;

                case "Enable PS4 Ambient Music":
                    if (Properties.Settings.Default.EnableGuiMusic == true)
                    {
                        DefaultValue = "On";
                    }
                    else
                    {
                        DefaultValue = "Off";
                    }
                    break;
                case "Enable kozarovv Patches":
                    if(Properties.Settings.Default.EnableCustomConfigFetching == true)
                    {
                        DefaultValue = "On";
                    }
                    else
                    {
                        DefaultValue = "Off";
                    }
                    break;

            }
            set.SValue = DefaultValue;
            set.type = TypeOfContorl;

            Values.Add(set);
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Random r = new Random();
            InitializeComponent();
            AddEsttingValues("Overwrite Temp Folder", "Off", new ListView());
            AddEsttingValues("Temp Path", "", new Control());
            AddEsttingValues("Replace NP Title ID With PS2 Title ID", "On", new ListView());
            //AddEsttingValues("Replace NP Title ID With PS2 Title ID", "On", new ListView());
            AddEsttingValues("Enable Boot Logo", "On", new ListView());
            AddEsttingValues("Enable PS4 Ambient Music", "On", new ListView());

            AddEsttingValues("Enable kozarovv Patches", "Off", new ListView());

            for (int i = 0; i < Values.Count; i++)
            {
                listBox.Items.Add(new { Name = Values[i].SettingName, ValueForNameValue = Values[i].SValue });
            }
            //Margin="0,200,0,10"
            //listView.Items.Add(new { SettingValue = "On" });
            //ListViewItem lvi = listView.Items[0] as ListViewItem;
            //lvi.Margin.Top = 200;


            //listView.Items.Add(new { SettingValue = "Off" });



            LoadCompleted = true;
        }


        public void ClearAndAddList()
        {
            listBox.Items.Clear();
            for (int i = 0; i < Values.Count; i++)
            {
                listBox.Items.Add(new { Name = Values[i].SettingName, ValueForNameValue = Values[i].SValue });
            }

            listBox.SelectedIndex = -1;
            SkipSettings = true;
            //i dont know why but the dam view keeps comming up

            OptionsView.Visibility = Visibility.Hidden;
        }

        private void listBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Left || e.Key == Key.Right || e.Key == Key.Up || e.Key == Key.Down)
            {
                SoundClass.PlayPS4Sound(SoundClass.Sound.Navigation);
            }
        }


        private void OptionsView_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.Key == Key.Escape)
            {
                MainWindowView.Opacity = 1.0f;
                OptionsView.Visibility = Visibility.Hidden;
                listBox.Background = System.Windows.Media.Brushes.Transparent;
            }
            if(e.Key == Key.Enter)
            {
                //save the current item
            }

           
        }

        private void listBox_MouseDown(object sender, MouseButtonEventArgs e)
        {
            
        }

        private void listBox_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
         

        }

        private void listBox_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                ButtonLick = e;
            }
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.Key == Key.Escape)
            {
                if (OptionsView.Visibility == Visibility.Visible)
                {
                    //hide this 
                    OptionsView.Visibility = Visibility.Hidden;
                }
                else
                {
                    this.Close();
                }
            }
            if(OptionsView.Visibility == Visibility.Visible)
            {
                if(e.Key == Key.Up)
                {

                }
            }
        }

        private void ListView_SettingItemDown(object sender, MouseButtonEventArgs e)
        {
            //if(e.LeftButton == MouseButtonState.Pressed)
            //{
            //    DoSettings();
            //}
        }

        public void DoSettings()
        {
            //check control type
            if (Values[listBox.SelectedIndex].type != new Control())
            {
                ListViewItem lvi = listView.SelectedValue as ListViewItem;

                Values[listBox.SelectedIndex].SValue = lvi.Content.ToString();
                if (Values[listBox.SelectedIndex].SValue == "On")
                {
                    switch (Values[listBox.SelectedIndex].SettingName)
                    {
                        case "Overwrite Temp Folder":
                            Properties.Settings.Default.OverwriteTemp = true;
                            break;
                        case "Replace NP Title ID With PS2 Title ID":
                            Properties.Settings.Default.EnablePS2IDReplace = true;
                            break;
                        case "Enable Boot Logo":
                            Properties.Settings.Default.EnableBootScreen = true;
                            break;
                        case "Enable PS4 Ambient Music":
                            Properties.Settings.Default.EnableGuiMusic = true;

                            SoundClass.PlayPS4Sound(SoundClass.Sound.PS4_Music);

                            break;
                        case "Enable kozarovv Patches":
                            Properties.Settings.Default.EnableCustomConfigFetching = true;
                            break;


                    }
                    Properties.Settings.Default.Save();//save the settings
                }
                else
                {
                    switch (Values[listBox.SelectedIndex].SettingName)
                    {
                        case "Overwrite Temp Folder":
                            Properties.Settings.Default.OverwriteTemp = false;
                            break;
                        case "Replace NP Title ID With PS2 Title ID":
                            Properties.Settings.Default.EnablePS2IDReplace = false;
                            break;
                        case "Enable Boot Logo":
                            Properties.Settings.Default.EnableBootScreen = false;
                            
                            break;
                        case "Enable PS4 Ambient Music":
                            Properties.Settings.Default.EnableGuiMusic = false;
                            if (SoundClass.PS4BGMDevice != null)
                            {
                                SoundClass.PS4BGMDevice.Stop();
                            }
                            break;

                        case "Enable kozarovv Patches":
                            Properties.Settings.Default.EnableCustomConfigFetching = false;
                            break;

                    }
                    Properties.Settings.Default.Save();//save the settings
                }
            }
            else
            {
                using (var dialog = new System.Windows.Forms.FolderBrowserDialog())
                {
                    System.Windows.Forms.DialogResult result = dialog.ShowDialog();
                    if (result == System.Windows.Forms.DialogResult.OK)
                    {
                        Properties.Settings.Default.TempPath = dialog.SelectedPath.ToString();
                    }
                    Values[listBox.SelectedIndex].SValue = Properties.Settings.Default.TempPath;
                }
                Properties.Settings.Default.Save();//save the settings
            }

            //close the options pannel
            MainWindowView.Opacity = 1.0f;
            OptionsView.Visibility = Visibility.Hidden;
            listBox.Background = System.Windows.Media.Brushes.Transparent;

            ClearAndAddList();
        }

        private void ListView_MouseDown(object sender, MouseButtonEventArgs e)
        {
            //if (e.LeftButton == MouseButtonState.Pressed)
            //{
            //    DoSettings();
            //}
        }

        private void ListView_MouseEnter(object sender, MouseEventArgs e)
        {

        }

        private void ListView_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            //if (e.LeftButton == MouseButtonState.Pressed)
            //{
            //    DoSettings();
            //}
        }

        private void listView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (listBox.SelectedIndex != -1)
                {
                    DoSettings();
                }
            }
            catch(Exception ex)
            {
                MainWindowView.Opacity = 1.0f;
                OptionsView.Visibility = Visibility.Hidden;
                listBox.Background = System.Windows.Media.Brushes.Transparent;
            }
        }

        private void listBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            CurrentIdx = listBox.SelectedIndex;

            if (ButtonLick.LeftButton == MouseButtonState.Pressed)
            {
                if (listBox.SelectedIndex != 1)
                {


                    MainWindowView.Opacity = 0.7f;
                    OptionsView.Visibility = Visibility.Hidden;
                    OptionsView.Visibility = Visibility.Visible;
                    listBox.Background = System.Windows.Media.Brushes.Transparent;
                }
                else
                {
                    using (var dialog = new System.Windows.Forms.FolderBrowserDialog())
                    {
                        System.Windows.Forms.DialogResult result = dialog.ShowDialog();
                        if (result == System.Windows.Forms.DialogResult.OK)
                        {
                            Properties.Settings.Default.TempPath = dialog.SelectedPath.ToString();
                        }
                        Values[listBox.SelectedIndex].SValue = Properties.Settings.Default.TempPath;
                    }
                    Properties.Settings.Default.Save();//save the settings
                    ClearAndAddList();
                }
            }
        }
    }
}
