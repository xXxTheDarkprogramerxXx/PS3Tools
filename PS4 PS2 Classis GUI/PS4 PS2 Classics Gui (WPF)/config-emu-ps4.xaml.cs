using System;
using System.Collections.Generic;
using System.Linq;
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
    /// Interaction logic for config_emu_ps4.xaml
    /// </summary>
    public partial class config_emu_ps4 : Window
    {
        public config_emu_ps4()
        {
            InitializeComponent();
        }

        public class SettingValue
        {
            public string SettingName { get; set; }

            public string[] SValue { get; set; }

            public Control type { get; set; }

        }

        List<SettingValue> Values = new List<SettingValue>();

        /// <summary>
        /// Adds Options menu item
        /// </summary>
        /// <param name="Value"></param>
        /// <returns></returns>
        public ListViewItem CreateItem(string Value)
        {
            //FontSize="20" FontFamily="Yu Gothic" Foreground="#FFFFFFFF"
            ListViewItem itemadd = new ListViewItem();
            var mar = itemadd.Margin;
            if (ListView.Items.Count == 0)
            {
                mar.Top = 200;
                mar.Bottom = 10;
                mar.Left = 0;
                mar.Right = 0;
            }
            else
            {
                mar.Top = 10;
                mar.Bottom = 10;
                mar.Left = 0;
                mar.Right = 0;
            }
            itemadd.FontSize = 20;
            itemadd.FontFamily = ListView.FontFamily;
            itemadd.HorizontalAlignment = HorizontalAlignment.Center;
            itemadd.VerticalAlignment = VerticalAlignment.Center;
            itemadd.Foreground = new SolidColorBrush(Colors.White);
            itemadd.Content = (Value);


            return itemadd;
        }

        /// <summary>
        /// Add Settings 
        /// </summary>
        /// <param name="SettingName"></param>
        /// <param name="DefaultValue"></param>
        /// <param name="TypeOfContorl"></param>
        public void AddEsttingValues(string SettingName, string[] DefaultValue, Control TypeOfContorl)
        {
            SettingValue set = new SettingValue();
            set.SettingName = SettingName;
            switch (SettingName)
            {
             
            }
            set.SValue = DefaultValue;
            set.type = TypeOfContorl;

            Values.Add(set);
        }
        public static Constants.ConfigEmuModel config = new Constants.ConfigEmuModel();

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            //Load all settings into view

            Random r = new Random();
            InitializeComponent();

            //Constants.ConfigEditorUsed
           

            AddEsttingValues("GS Uprender", new string[] { config.gs_uprender.ToString() }, new Control());

            AddEsttingValues("GS Upscale", new string[] { config.gs_upscale.ToString() }, new Control());

            AddEsttingValues("Config Local Lua", new string[] { config.config_local_lua },new TextBox());

            AddEsttingValues("Load Tooling Lua", new string[] { config.load_tooling_lua }, new ItemsControl());

            AddEsttingValues("Record Audio", new string[] { config.recod.audio }, new TextBox());

            AddEsttingValues("Record Audio Img", new string[] { config.recod.audio_img }, new TextBox());

            AddEsttingValues("Record Audio Image", new string[] { config.recod.audio_image }, new TextBox());

            AddEsttingValues("Record Audio Ext", new string[] { config.recod.audio_ext }, new TextBox());

            AddEsttingValues("Max Console Spam", new string[] { config.max_console_spam }, new TextBox());

            #region << Path >>

            AddEsttingValues("Path Snaps", new string[] { config.path.snaps }, new TextBox());

            AddEsttingValues("Path Recordings", new string[] { config.path.recordings }, new TextBox());

            AddEsttingValues("Path Audio Images", new string[] { config.path.audio_images }, new TextBox());

            AddEsttingValues("Path Memcards", new string[] { config.path.memcards }, new TextBox());

            AddEsttingValues("Path Vmc", new string[] { config.path.vmc }, new TextBox());

            AddEsttingValues("Path Emulog", new string[] { config.path.emulog }, new TextBox());

            AddEsttingValues("Path Manual", new string[] { config.path.manual }, new TextBox());

            AddEsttingValues("Path Patches", new string[] { config.path.patches }, new TextBox());

            AddEsttingValues("Path Trophydata", new string[] { config.path.trophydata }, new TextBox());

            AddEsttingValues("Path Featuredata", new string[] { config.path.featuredata }, new TextBox());

            AddEsttingValues("Path Postproc", new string[] { config.path.postproc }, new TextBox());

            AddEsttingValues("Path Toolingscript", new string[] { config.path.toolingscript }, new TextBox());

            AddEsttingValues("Snapshot Name", new string[] { config.snap.Name }, new TextBox());

            AddEsttingValues("Snapshot Datafile", new string[] { config.snap.DataFile }, new TextBox());

            AddEsttingValues("Snapshot Restore", new string[] { config.snap.Resotore }, new TextBox());

            AddEsttingValues("Snapshot Mcd Files	", new string[] { config.snap.Mcd_Files }, new TextBox());

            AddEsttingValues("Snapshot Repeat	", new string[] { config.snap.Repeat }, new TextBox());

            AddEsttingValues("Snapshot Modulo	", new string[] { config.snap.Modulo }, new TextBox());

            AddEsttingValues("DS4 Deadzone Adjust	", new string[] { config.DS4.deadzone_adjust }, new TextBox());

            AddEsttingValues("DS4 Diagonal Adjust	", new string[] { config.DS4.diagonal_adjust }, new TextBox());

            AddEsttingValues("Host Pad Loses Focus", new string[] { config.Host.pad_loses_focus }, new TextBox());

            AddEsttingValues("Host Gamepads", new string[] { config.Host.gamepads }, new TextBox());

            AddEsttingValues("Host Keyboard", new string[] { config.Host.keyboard }, new TextBox());

            AddEsttingValues("Host Audio", new string[] { config.Host.audio }, new TextBox());

            AddEsttingValues("Host Audio Latency", new string[] { config.Host.audio_latency }, new TextBox());

            AddEsttingValues("Host Window Scale	", new string[] { config.Host.window_scale }, new TextBox());

            AddEsttingValues("Host Window Pos	", new string[] { config.Host.window_pos }, new TextBox());

            AddEsttingValues("Host Display Mode	", new string[] { config.Host.display_mode }, new TextBox());

            AddEsttingValues("Host OSD", new string[] { config.Host.osd }, new TextBox());

            AddEsttingValues("Host vsync", new string[] { config.Host.vsync }, new TextBox());

            AddEsttingValues("Host Trophy Support", new string[] { config.Host.trophy_support }, new TextBox());

            AddEsttingValues("RTC Epoch", new string[] { config.rtc_epoch }, new TextBox());

            AddEsttingValues("Framelimiter", new string[] { config.framelimiter }, new TextBox());

            AddEsttingValues("Framelimit-fps", new string[] { config.framelimit_fps }, new TextBox());

            AddEsttingValues("Framelimit-scalar", new string[] { config.framelimit_scalar }, new TextBox());

            AddEsttingValues("Framelimit-mode", new string[] { config.framelimit_mode.ToString() }, new Control());

            AddEsttingValues("Audio-stretching", new string[] { config.audio_stretching.ToString() }, new TextBox());

            AddEsttingValues("Ps2-lang", new string[] { config.ps2_lang.ToString() }, new TextBox());

            AddEsttingValues("Pad-record", new string[] { config.pad_record.ToString() }, new TextBox());

            AddEsttingValues("Max Disc Num", new string[] { config.max_disc_num.ToString() }, new TextBox());

            AddEsttingValues("Ps2 Title ID", new string[] { config.ps2_title_id.ToString() }, new TextBox());

            AddEsttingValues("Boot Disc ID", new string[] { config.boot_disc_id.ToString() }, new TextBox());

            AddEsttingValues("Mute Audio", new string[] { config.mute_audio.ToString() }, new Control());

            AddEsttingValues("Mute Streaming Audio", new string[] { config.mute_streaming_audio.ToString() }, new Control());

            #endregion << Path >>
            for (int i = 0; i < Values.Count; i++)
            {
                ListView.Items.Add(new { Name = Values[i].SettingName, ValueForNameValue = Values[i].SValue[0] });
            }
            //Margin="0,200,0,10"
            //listView.Items.Add(new { SettingValue = "On" });
            //ListViewItem lvi = listView.Items[0] as ListViewItem;
            //lvi.Margin.Top = 200;


            //listView.Items.Add(new { SettingValue = "Off" });
        }

        private void btnRemove_Click(object sender, RoutedEventArgs e)
        {

        }

        private void listBox_KeyDown(object sender, KeyEventArgs e)
        {

        }

        public void ClearandReload()
        {
            Values.Clear();
            ListView.Items.Clear();

            AddEsttingValues("GS Uprender", new string[] { config.gs_uprender.ToString() }, new Control());

            AddEsttingValues("GS Upscale", new string[] { config.gs_upscale.ToString() }, new Control());

            AddEsttingValues("Config Local Lua", new string[] { config.config_local_lua }, new TextBox());

            AddEsttingValues("Load Tooling Lua", new string[] { config.load_tooling_lua }, new ItemsControl());

            AddEsttingValues("Record Audio", new string[] { config.recod.audio }, new TextBox());

            AddEsttingValues("Record Audio Img", new string[] { config.recod.audio_img }, new TextBox());

            AddEsttingValues("Record Audio Image", new string[] { config.recod.audio_image }, new TextBox());

            AddEsttingValues("Record Audio Ext", new string[] { config.recod.audio_ext }, new TextBox());

            AddEsttingValues("Max Console Spam", new string[] { config.max_console_spam }, new TextBox());

            #region << Path >>

            AddEsttingValues("Path Snaps", new string[] { config.path.snaps }, new TextBox());

            AddEsttingValues("Path Recordings", new string[] { config.path.recordings }, new TextBox());

            AddEsttingValues("Path Audio Images", new string[] { config.path.audio_images }, new TextBox());

            AddEsttingValues("Path Memcards", new string[] { config.path.memcards }, new TextBox());

            AddEsttingValues("Path Vmc", new string[] { config.path.vmc }, new TextBox());

            AddEsttingValues("Path Emulog", new string[] { config.path.emulog }, new TextBox());

            AddEsttingValues("Path Manual", new string[] { config.path.manual }, new TextBox());

            AddEsttingValues("Path Patches", new string[] { config.path.patches }, new TextBox());

            AddEsttingValues("Path Trophydata", new string[] { config.path.trophydata }, new TextBox());

            AddEsttingValues("Path Featuredata", new string[] { config.path.featuredata }, new TextBox());

            AddEsttingValues("Path Postproc", new string[] { config.path.postproc }, new TextBox());

            AddEsttingValues("Path Toolingscript", new string[] { config.path.toolingscript }, new TextBox());

            AddEsttingValues("Snapshot Name", new string[] { config.snap.Name }, new TextBox());

            AddEsttingValues("Snapshot Datafile", new string[] { config.snap.DataFile }, new TextBox());

            AddEsttingValues("Snapshot Restore", new string[] { config.snap.Resotore }, new TextBox());

            AddEsttingValues("Snapshot Mcd Files	", new string[] { config.snap.Mcd_Files }, new TextBox());

            AddEsttingValues("Snapshot Repeat	", new string[] { config.snap.Repeat }, new TextBox());

            AddEsttingValues("Snapshot Modulo	", new string[] { config.snap.Modulo }, new TextBox());

            AddEsttingValues("DS4 Deadzone Adjust	", new string[] { config.DS4.deadzone_adjust }, new TextBox());

            AddEsttingValues("DS4 Diagonal Adjust	", new string[] { config.DS4.diagonal_adjust }, new TextBox());

            AddEsttingValues("Host Pad Loses Focus", new string[] { config.Host.pad_loses_focus }, new TextBox());

            AddEsttingValues("Host Gamepads", new string[] { config.Host.gamepads }, new TextBox());

            AddEsttingValues("Host Keyboard", new string[] { config.Host.keyboard }, new TextBox());

            AddEsttingValues("Host Audio", new string[] { config.Host.audio }, new TextBox());

            AddEsttingValues("Host Audio Latency", new string[] { config.Host.audio_latency }, new TextBox());

            AddEsttingValues("Host Window Scale	", new string[] { config.Host.window_scale }, new TextBox());

            AddEsttingValues("Host Window Pos	", new string[] { config.Host.window_pos }, new TextBox());

            AddEsttingValues("Host Display Mode	", new string[] { config.Host.display_mode }, new TextBox());

            AddEsttingValues("Host OSD", new string[] { config.Host.osd }, new TextBox());

            AddEsttingValues("Host vsync", new string[] { config.Host.vsync }, new TextBox());

            AddEsttingValues("Host Trophy Support", new string[] { config.Host.trophy_support }, new TextBox());

            AddEsttingValues("RTC Epoch", new string[] { config.rtc_epoch }, new TextBox());

            AddEsttingValues("Framelimiter", new string[] { config.framelimiter }, new TextBox());

            AddEsttingValues("Framelimit-fps", new string[] { config.framelimit_fps }, new TextBox());

            AddEsttingValues("Framelimit-scalar", new string[] { config.framelimit_scalar }, new TextBox());

            AddEsttingValues("Framelimit-mode", new string[] { config.framelimit_mode.ToString() }, new TextBox());

            AddEsttingValues("Audio-stretching", new string[] { config.audio_stretching.ToString() }, new TextBox());

            AddEsttingValues("Ps2-lang", new string[] { config.ps2_lang.ToString() }, new TextBox());

            AddEsttingValues("Pad-record", new string[] { config.pad_record.ToString() }, new TextBox());

            AddEsttingValues("Max Disc Num", new string[] { config.max_disc_num.ToString() }, new TextBox());

            AddEsttingValues("Ps2 Title ID", new string[] { config.ps2_title_id.ToString() }, new TextBox());

            AddEsttingValues("Boot Disc ID", new string[] { config.boot_disc_id.ToString() }, new TextBox());

            AddEsttingValues("Mute Audio", new string[] { config.mute_audio.ToString() }, new TextBox());

            AddEsttingValues("Mute Streaming Audio", new string[] { config.mute_streaming_audio.ToString() }, new TextBox());

            #endregion << Path >>
            for (int i = 0; i < Values.Count; i++)
            {
                ListView.Items.Add(new { Name = Values[i].SettingName, ValueForNameValue = Values[i].SValue[0] });
            }
        }

        public void DoSettings()
        {
            //check control type
            if (Values[ListView.SelectedIndex].type != new Control())
            {
               
                if(Values[ListView.SelectedIndex].SettingName.ToString() == "GS Uprender")
                {
                    //Open Config Screen

                    OptionsList optionlist = new OptionsList(Enum.GetNames(typeof(Constants.Config_Emu_PS4.Gs_Uprender)), Values[ListView.SelectedIndex].SettingName.ToString());
                    optionlist.ShowDialog();
                  
               }

                if (Values[ListView.SelectedIndex].SettingName.ToString() == "GS Upscale")
                {
                    //Open Config Screen

                    OptionsList optionlist = new OptionsList(Enum.GetNames(typeof(Constants.Config_Emu_PS4.Gs_Upscale)), Values[ListView.SelectedIndex].SettingName.ToString());
                    optionlist.ShowDialog();

                }
            }
            if (Values[ListView.SelectedIndex].type != new TextBox())
            {
                EditText EditText = new EditText(Values[ListView.SelectedIndex].SValue[0]);
                EditText.ShowDialog();
                Values[ListView.SelectedIndex].SValue[0] = EditText._Value;
                switch(Values[ListView.SelectedIndex].SettingName.ToString())
                {
                    case "Config Local Lua":
                        config.config_local_lua = Values[ListView.SelectedIndex].SValue[0];
                        break;
                    case "Load Tooling Lua":
                        config.load_tooling_lua = Values[ListView.SelectedIndex].SValue[0];
                        break;
                }
            }
            else
            {
               
                Properties.Settings.Default.Save();//save the settings
            }
            ClearandReload();
        }

        private void listBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (ListView.SelectedIndex != -1)
                {
                    DoSettings();
                }
            }
            catch (Exception ex)
            {
               
            }
        }
    }
}
