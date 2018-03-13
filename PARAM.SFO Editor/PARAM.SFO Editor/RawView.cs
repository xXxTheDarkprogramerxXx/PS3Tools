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
    public partial class RawView : Form
    {
        PeXploit.PARAM_SFO pSFO;

        public RawView()
        {
            InitializeComponent();
        }

        public RawView(PeXploit.PARAM_SFO _sfo)
        {
            pSFO = _sfo;
            InitializeComponent();
        }
        private void RawView_Load(object sender, EventArgs e)
        {
            //show Content ID
            listBox1.Items.Add("Content ID : "+pSFO.ContentID );
            listBox1.Items.Add("Title : " + pSFO.Title );
            listBox1.Items.Add("TITLEID : " + pSFO.TITLEID );
            listBox1.Items.Add("TitleID : " + pSFO.TitleID);
            listBox1.Items.Add("Detail : " + pSFO.Detail);
            

            foreach (var item in pSFO.Tables)
            {
                listBox1.Items.Add("Index : " + item.index + " Name : " + item.Name + " Value : " + item.Value);
            }
        }
    }
}
