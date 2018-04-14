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
    public partial class Advanced : Form
    {

        public string LabelText
        {
            get
            {
                return this.txtAllInfo.Text;
            }
            set
            {
                this.txtAllInfo.Text = value;
            }
        }

        public Advanced()
        {
            InitializeComponent();
        }
    }
}
