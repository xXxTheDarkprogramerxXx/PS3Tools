using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PS4_PS2_Classis_GUI
{
    public partial class Settings : Form
    {
        bool FinsihedLoading = false;
        public Settings()
        {
            InitializeComponent();
        }

        private void cbxTemp_CheckedChanged(object sender, EventArgs e)
        {
            if (FinsihedLoading == true)
            {
                Properties.Settings.Default.OverwriteTemp = cbxTemp.Checked;
                Properties.Settings.Default.Save();
                if(cbxTemp.Checked == true)
                {
                    textBox1.Enabled = true;
                    btnBroswe.Enabled = true;
                }
            }
        }

        private void Settings_Load(object sender, EventArgs e)
        {
            FinsihedLoading = false;
            if(Properties.Settings.Default.OverwriteTemp == true)
            {
                cbxTemp.Checked = true;
            }
            if(Properties.Settings.Default.TempPath != string.Empty)
            {
                textBox1.Text = Properties.Settings.Default.TempPath;
                textBox1.Enabled = true;
                btnBroswe.Enabled = true;
            }
            //set the values
            checkBox3.Checked = Properties.Settings.Default.EnableAdvancedMode;


            checkBox1.Checked = Properties.Settings.Default.EnablePS2IDReplace ;

            checkBox2.Checked = Properties.Settings.Default.EnableBootScreen;

            checkBox4.Checked = Properties.Settings.Default.EnableGuiMusic;

            FinsihedLoading = true;
        }

        private void checkBox3_CheckedChanged(object sender, EventArgs e)
        {
            if (FinsihedLoading == true)
            {
                Properties.Settings.Default.EnableAdvancedMode = checkBox3.Checked;
                Properties.Settings.Default.Save();
            }
        }

        private void btnBroswe_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog saveFileDialog1 = new FolderBrowserDialog();
            //saveFileDialog1.Filter = "PS4 PKG|*.pkg";
            //saveFileDialog1.Title = "Save an PS4 PKG File";
            //saveFileDialog1.ov
            if (DialogResult.OK == saveFileDialog1.ShowDialog())
            {
                textBox1.Text = saveFileDialog1.SelectedPath.ToString();
                Properties.Settings.Default.TempPath = textBox1.Text.Trim();
                Properties.Settings.Default.Save();
            }
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (FinsihedLoading == true)
            {
                Properties.Settings.Default.EnablePS2IDReplace = checkBox1.Checked;
                Properties.Settings.Default.Save();
            }
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            if (FinsihedLoading == true)
            {
                Properties.Settings.Default.EnableBootScreen = checkBox2.Checked;
                Properties.Settings.Default.Save();
            }
        }

        private void checkBox4_CheckedChanged(object sender, EventArgs e)
        {
            if (FinsihedLoading == true)
            {
                if(checkBox4.Checked == false)
                {
                    if (Form1.waveOutDevice != null)
                    {
                        Form1.waveOutDevice.Stop();
                        Form1.waveOutDevice.Dispose();
                    }
                  
                }
                else
                {
                    AudioFileReader audioFileReader = new AudioFileReader(Form1.AppCommonPath() + "PS4.mp3");
                    Form1.waveOutDevice.Init(audioFileReader);
                    Form1.waveOutDevice.Play();
                    Form1.waveOutDevice.Play();
                }
                Properties.Settings.Default.EnableGuiMusic = checkBox4.Checked;
                Properties.Settings.Default.Save();
            }
        }
    }
}
