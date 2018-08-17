using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PARAM.SFO_Editor
{
    public partial class NewSFO : Form
    {
        public NewSFO()
        {
            InitializeComponent();
        }

        public enum SFOToMake
        {
            PS3 = 0,
            PSVita = 1,
            PS4 = 2,
        }

        public SFOToMake _SfoToMake { get; set; }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Currently Only PS4 is fully supported Vita might also work");
            _SfoToMake = SFOToMake.PS3;
            this.Close();
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            _SfoToMake = SFOToMake.PS4;
            this.Close();
        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Currently Only PS4 is fully supported Vita might also work");
            _SfoToMake = SFOToMake.PSVita;
            this.Close();
        }
    }
}
