using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Diagnostics;
using System.Threading;
using IsoCreator;
using BER.CDCat.Export;

namespace PS3_ISO_Creator
{
    public partial class Form1 : Form
    {

        public Form1()
        {
            InitializeComponent();

            m_creator = new IsoCreator.IsoCreator();
            m_creator.Progress += new ProgressDelegate(creator_Progress);
            m_creator.Finish += new FinishDelegate(creator_Finished);
            m_creator.Abort += new AbortDelegate(creator_Abort);

        }

        string version = "1.0";

        private Thread m_thread = null;
        private IsoCreator.IsoCreator m_creator = null;

        private delegate void SetLabelDelegate(string text);
        private delegate void SetNumericValueDelegate(int value);

        void creator_Abort(object sender, AbortEventArgs e)
        {
            MessageBox.Show(e.Message, "Abort", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            MessageBox.Show("The ISO creating process has been stopped.", "Abort", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            button2.Invoke(new Action(() => button2.Enabled = false)); 
            progressBar.Value = 0;
            progressBar.Maximum = 0;
            labelStatus.Text = "Process not started";
        }

        void creator_Finished(object sender, FinishEventArgs e)
        {
            MessageBox.Show(e.Message, "Finish", MessageBoxButtons.OK, MessageBoxIcon.Information);
            button2.Invoke(new Action(() => button2.Enabled = false)); 
            progressBar.Value = 0;
            labelStatus.Text = "Process not started";
        }

        private void SetLabelStatus(string text)
        {
            this.labelStatus.Text = text;
            this.labelStatus.Refresh();
        }

        private void SetProgressValue(int value)
        {
            this.progressBar.Value = value;
        }

        private void SetProgressMaximum(int maximum)
        {
            this.progressBar.Maximum = maximum;
        }

        void creator_Progress(object sender, ProgressEventArgs e)
        {
            if (e.Action != null)
            {
                if (!this.InvokeRequired)
                {
                    this.SetLabelStatus(e.Action);
                }
                else
                {
                    this.Invoke(new SetLabelDelegate(SetLabelStatus), e.Action);
                }
            }

            if (e.Maximum != -1)
            {
                if (!this.InvokeRequired)
                {
                    this.SetProgressMaximum(e.Maximum);
                }
                else
                {
                    this.Invoke(new SetNumericValueDelegate(SetProgressMaximum), e.Maximum);
                }
            }

            if (!this.InvokeRequired)
            {
                progressBar.Value = (e.Current <= progressBar.Maximum) ? e.Current : progressBar.Maximum;
            }
            else
            {
                int value = (e.Current <= progressBar.Maximum) ? e.Current : progressBar.Maximum;
                this.Invoke(new SetNumericValueDelegate(SetProgressValue), value);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog dialog = new FolderBrowserDialog();
            if (dialog.ShowDialog(this) == DialogResult.OK)
            {
                txtPath.Text = dialog.SelectedPath;
                textBoxVolumeName.Text = new DirectoryInfo(dialog.SelectedPath).Name + ".iso";
            }

            

            DialogResult result = MessageBox.Show("Do you want to create an ISO of this folder for ps3 ?", "PS3ISO", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                if (m_thread == null || !m_thread.IsAlive)
                {
                    if (textBoxVolumeName.Text.Trim() != "")
                    {
                        m_thread = new Thread(new ParameterizedThreadStart(m_creator.Folder2Iso));
                        m_thread.Start(new IsoCreator.IsoCreator.IsoCreatorFolderArgs(txtPath.Text, Application.StartupPath + "\\" + textBoxVolumeName.Text, textBoxVolumeName.Text));

                        button2.Enabled = true;
                    }
                    else
                    {
                        MessageBox.Show("Please insert a name for the volume", "No volume name", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                    }
                }
                else
                {
                    if (MessageBox.Show("Are you sure you want to abort the process?", "Abort", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        m_thread.Abort();
                    }
                }
            }
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (m_creator != null && m_thread != null && m_thread.IsAlive)
            {
                m_thread.Abort();
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            lblVersion.Text = version;
        }
    }
}
