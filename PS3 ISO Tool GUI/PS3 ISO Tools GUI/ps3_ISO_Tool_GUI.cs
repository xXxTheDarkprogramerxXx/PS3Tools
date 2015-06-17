using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;
using System.IO;

namespace PS3_ISO_Tools_GUI
{
    public partial class ps3_ISO_Tool_GUI : Form
    {
        public ps3_ISO_Tool_GUI()
        {
            InitializeComponent();
        }

        #region <<< Var's >>>

        string version = "1.03";

        #endregion <<< Var's >>>

        private void Form1_Load(object sender, EventArgs e)
        {
            lblVersion.Text = version;
            comboBox1.SelectedIndex = 0;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog thedialog = new OpenFileDialog();
            thedialog.Title = "Select ISO";
            thedialog.Filter = "Image File|*.iso";
            thedialog.InitialDirectory = Environment.SpecialFolder.MyComputer.ToString();
            if (thedialog.ShowDialog() == DialogResult.OK)
            {
                string isopath = thedialog.FileName;
                txtPath.Text = isopath;
                ProcessStartInfo start = new ProcessStartInfo();
                start.FileName =Application.StartupPath +  @"\\psiso_tool.exe";
                start.Arguments = "--" + comboBox1.Text.ToLower() + " --verbose " + "\"" + isopath + "\"";
                start.UseShellExecute = false;
                start.RedirectStandardOutput = true;
                start.CreateNoWindow = true;
                using (Process process = Process.Start(start))
                {
                    using (StreamReader reader = process.StandardOutput)
                    {
                        string result = reader.ReadToEnd();
                        textBox1.Text = result;
                    }
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog thedialog = new FolderBrowserDialog();
            if (thedialog.ShowDialog() == DialogResult.OK)
            {
                if (comboBox1.SelectedIndex == 1)
                {
                    string isopath = thedialog.SelectedPath;
                    txtPath.Text = isopath;
                    ProcessStartInfo start = new ProcessStartInfo();
                    start.FileName = Application.StartupPath + @"\\psiso_tool.exe";
                    start.Arguments = "--mkps3iso " + "\"" + isopath + "\"";
                    start.UseShellExecute = false;
                    start.RedirectStandardOutput = true;
                    start.CreateNoWindow = true;
                    using (Process process = Process.Start(start))
                    {
                        using (StreamReader reader = process.StandardOutput)
                        {
                            string result = reader.ReadToEnd();
                            textBox1.Text = result;
                        }
                    }
                }
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
         
        }
    }
}
