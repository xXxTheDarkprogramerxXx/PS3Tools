using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PS4_PS2_Classis_GUI.Resources
{
    public partial class editcontentid : Form
    {
        Form1 orgif;
        public editcontentid(Form1 mainform)
        {
            orgif = mainform;
            InitializeComponent();
        }

        private void editcontentid_Load(object sender, EventArgs e)
        {
            var test = orgif.txtTitleId.Text;
        }
    }
}
