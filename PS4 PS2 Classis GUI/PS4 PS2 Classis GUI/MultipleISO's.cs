using DiscUtils.Iso9660;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PS4_PS2_Classis_GUI
{
    public partial class MultipleISO_s : Form
    {
        public MultipleISO_s()
        {
            InitializeComponent();
        }

        private const int CP_NOCLOSE_BUTTON = 0x200;
        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams myCp = base.CreateParams;
                myCp.ClassStyle = myCp.ClassStyle | CP_NOCLOSE_BUTTON;
                return myCp;
            }
        }

        public void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            //calc the size of the file
            long length = new System.IO.FileInfo(Form1.isoFiles[listBox1.SelectedIndex].ToString()).Length;
            //now make it human readable

            lblSize.Text = CalculateBytes(length);//human readble
            using (FileStream isoStream = File.OpenRead(Form1.isoFiles[listBox1.SelectedIndex].ToString()))
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
                        lblTitleId.Text = "PS2 ID : " + PS2Id.Replace(".", "");
                    }
                    else
                    {
                        MessageBox.Show("Could not load PS2 ID");
                    }
                }
                else
                {
                    DialogResult dlr = MessageBox.Show("Could not load PS2 ID\n\n wpuld you like to submit an issue ?", "Error Reporting", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);
                    if (dlr == DialogResult.Yes)
                    {
                        //load github issue page
                        Process.Start(@"https://github.com/xXxTheDarkprogramerxXx/PS3Tools/issues");
                    }

                }
            }
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
        private void MultipleISO_s_Load(object sender, EventArgs e)
        {
            
            LoadIsoFiles();
        }

        public void LoadIsoFiles()
        {
            //foreach iso file
            for (int i = 0; i < Form1.isoFiles.Count; i++)
            {
                string File = Form1.isoFiles[i].ToString();
                listBox1.Items.Add(Path.GetFileName(File));
            }
        }

        private void btnRemove_Click(object sender, EventArgs e)
        {
            //foreach iso file
            if (DialogResult.Yes == MessageBox.Show("You are about to delete an iso from your list. Are you sure you want to continue?","Remove ISO",MessageBoxButtons.YesNo,MessageBoxIcon.Question))
            {
                //delete the iso
                Form1.isoFiles.RemoveAt(listBox1.SelectedIndex);
            }
            LoadIsoFiles();
            
        }
    }
}
