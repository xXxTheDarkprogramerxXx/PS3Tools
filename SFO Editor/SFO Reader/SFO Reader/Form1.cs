//▒██   ██▒▒██   ██▒▒██   ██▒   ▄▄▄█████▓ ██░ ██ ▓█████    ▓█████▄  ▄▄▄       ██▀███   ██ ▄█▀ ██▓███   ██▀███   ▒█████    ▄████  ██▀███   ▄▄▄       ███▄ ▄███▓▓█████  ██▀███     ▒██   ██▒▒██   ██▒▒██   ██▒
//▒▒ █ █ ▒░▒▒ █ █ ▒░▒▒ █ █ ▒░   ▓  ██▒ ▓▒▓██░ ██▒▓█   ▀    ▒██▀ ██▌▒████▄    ▓██ ▒ ██▒ ██▄█▒ ▓██░  ██▒▓██ ▒ ██▒▒██▒  ██▒ ██▒ ▀█▒▓██ ▒ ██▒▒████▄    ▓██▒▀█▀ ██▒▓█   ▀ ▓██ ▒ ██▒   ▒▒ █ █ ▒░▒▒ █ █ ▒░▒▒ █ █ ▒░
//░░  █   ░░░  █   ░░░  █   ░   ▒ ▓██░ ▒░▒██▀▀██░▒███      ░██   █▌▒██  ▀█▄  ▓██ ░▄█ ▒▓███▄░ ▓██░ ██▓▒▓██ ░▄█ ▒▒██░  ██▒▒██░▄▄▄░▓██ ░▄█ ▒▒██  ▀█▄  ▓██    ▓██░▒███   ▓██ ░▄█ ▒   ░░  █   ░░░  █   ░░░  █   ░
// ░ █ █ ▒  ░ █ █ ▒  ░ █ █ ▒    ░ ▓██▓ ░ ░▓█ ░██ ▒▓█  ▄    ░▓█▄   ▌░██▄▄▄▄██ ▒██▀▀█▄  ▓██ █▄ ▒██▄█▓▒ ▒▒██▀▀█▄  ▒██   ██░░▓█  ██▓▒██▀▀█▄  ░██▄▄▄▄██ ▒██    ▒██ ▒▓█  ▄ ▒██▀▀█▄      ░ █ █ ▒  ░ █ █ ▒  ░ █ █ ▒ 
//▒██▒ ▒██▒▒██▒ ▒██▒▒██▒ ▒██▒     ▒██▒ ░ ░▓█▒░██▓░▒████▒   ░▒████▓  ▓█   ▓██▒░██▓ ▒██▒▒██▒ █▄▒██▒ ░  ░░██▓ ▒██▒░ ████▓▒░░▒▓███▀▒░██▓ ▒██▒ ▓█   ▓██▒▒██▒   ░██▒░▒████▒░██▓ ▒██▒   ▒██▒ ▒██▒▒██▒ ▒██▒▒██▒ ▒██▒
//▒▒ ░ ░▓ ░▒▒ ░ ░▓ ░▒▒ ░ ░▓ ░     ▒ ░░    ▒ ░░▒░▒░░ ▒░ ░    ▒▒▓  ▒  ▒▒   ▓▒█░░ ▒▓ ░▒▓░▒ ▒▒ ▓▒▒▓▒░ ░  ░░ ▒▓ ░▒▓░░ ▒░▒░▒░  ░▒   ▒ ░ ▒▓ ░▒▓░ ▒▒   ▓▒█░░ ▒░   ░  ░░░ ▒░ ░░ ▒▓ ░▒▓░   ▒▒ ░ ░▓ ░▒▒ ░ ░▓ ░▒▒ ░ ░▓ ░
//░░   ░▒ ░░░   ░▒ ░░░   ░▒ ░       ░     ▒ ░▒░ ░ ░ ░  ░    ░ ▒  ▒   ▒   ▒▒ ░  ░▒ ░ ▒░░ ░▒ ▒░░▒ ░       ░▒ ░ ▒░  ░ ▒ ▒░   ░   ░   ░▒ ░ ▒░  ▒   ▒▒ ░░  ░      ░ ░ ░  ░  ░▒ ░ ▒░   ░░   ░▒ ░░░   ░▒ ░░░   ░▒ ░
// ░    ░   ░    ░   ░    ░       ░       ░  ░░ ░   ░       ░ ░  ░   ░   ▒     ░░   ░ ░ ░░ ░ ░░         ░░   ░ ░ ░ ░ ▒  ░ ░   ░   ░░   ░   ░   ▒   ░      ░      ░     ░░   ░     ░    ░   ░    ░   ░    ░  
// ░    ░   ░    ░   ░    ░               ░  ░  ░   ░  ░      ░          ░  ░   ░     ░  ░               ░         ░ ░        ░    ░           ░  ░       ░      ░  ░   ░         ░    ░   ░    ░   ░    ░  
//                                                          ░                                                        



using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using PeXploit;
using System.IO;
using System.Threading;

namespace SFO_Reader
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        #region << Var's >>

        string sfopath = null;

        public int i = 0;

        #endregion << Var's >>

        #region << Private Events >>

        private void button1_Click(object sender, EventArgs e)
        {
            if (backgroundWorker1.IsBusy)
            {
                backgroundWorker1.CancelAsync();
            }
            OpenFileDialog theDialog = new OpenFileDialog();
            theDialog.Title = "Select PARAM.SFO File";
            theDialog.Filter = "PARAM.SFO|*.sfo";
            theDialog.InitialDirectory = System.Environment.SpecialFolder.MyComputer.ToString();
            if (theDialog.ShowDialog() == DialogResult.OK)
            {
                sfopath = theDialog.FileName;
                sfopath = sfopath.Replace("\\PARAM.SFO", "");
                if (File.Exists(sfopath + @"\C00\PARAM.SFO"))
                {
                    PARAM_SFO para = new PARAM_SFO(sfopath + @"\C00\PARAM.SFO");
                    if (backgroundWorker1.IsBusy == false)
                    {
                        backgroundWorker1.RunWorkerAsync();
                    }
                    textBox1.Text = para.TitleID;
                    textBox4.Text = para.Title;
                    textBox3.Text = para.DataType.ToString();
                    textBox2.Text = para.ContentID;
                    string[] lines = System.IO.File.ReadAllLines(Application.StartupPath + @"\PS3DB");
                    foreach (string line in lines)
                    {
                        if (line.Contains(textBox1.Text))
                        {
                            lblEdat.Text = "Edat Enabled";
                        }
                    }
                    this.Text = "SFO Reader : " + para.Title.Replace("???", "-");
                }
                else
                {
                    {
                        PARAM_SFO para = new PARAM_SFO(sfopath + @"\PARAM.SFO");
                        string[] lines = System.IO.File.ReadAllLines(Application.StartupPath + @"\PS3DB");
                        textBox1.Text = para.TitleID;
                        textBox4.Text = para.Title.Replace("???", "(TM)");
                        textBox3.Text = para.DataType.ToString();
                        textBox2.Text = para.ContentID;
                        this.Text = "SFO Reader : " + para.Title.Replace("???", "-");
                        foreach (string line in lines)
                        {
                            if (line.Contains(textBox1.Text) == true)
                            {
                                if (backgroundWorker1.IsBusy == false)
                                    backgroundWorker1.RunWorkerAsync();
                                lblEdat.Text = "Edat Enabled";
                                return;
                            }
                        }
                        if (backgroundWorker1.IsBusy == false)
                            backgroundWorker1.RunWorkerAsync();
                        textBox4.Visible = false;
                        label4.Visible = false;
                    }
                }
            }

        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                while (i == 0)
                {
                    if (Directory.Exists(sfopath + @"\C00\") && i == 0)
                    {
                        pictureBox2.ImageLocation = sfopath + @"\C00\ICON0.PNG";
                        Thread.Sleep(1500);
                        i = 1;
                    }
                    else
                    {
                        pictureBox2.ImageLocation = sfopath + @"\ICON0.PNG";
                        Thread.Sleep(1500);
                        i = 1;
                    }
                }
                while (i == 1)
                {
                    try
                    {
                        if (Directory.Exists(sfopath + @"\C00\") && i == 1)
                        {
                            pictureBox2.ImageLocation = sfopath + @"\C00\PIC1.PNG";
                            Thread.Sleep(1500);
                            i = 0;
                        }
                        else
                        {
                            pictureBox2.ImageLocation = sfopath + @"\PIC1.PNG";
                            Thread.Sleep(1500);
                            i = 0;
                        }
                    }
                    catch (Exception ee)
                    { }
                }
            }
            catch (Exception ex)
            {

            }
        }

        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            backgroundWorker1.RunWorkerAsync();
        }

        private void lblEdat_Click(object sender, EventArgs e)
        {
            MessageBox.Show("This Means That The Game Selected Is Usable Via PeXploit", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        #endregion << Private Events >>

        #region << Private Methods >>

        private Image getImageFromBytes(byte[] image)
        {
            MemoryStream ms = new MemoryStream(image, 0, image.Length);
            return Image.FromStream(ms, true);
        }
       
        #endregion << Rpiavte Methods >>
    }
}
