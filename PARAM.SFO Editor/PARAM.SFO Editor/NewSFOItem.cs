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
    public partial class NewSFOItem : Form
    {
        public NewSFOItem()
        {
            InitializeComponent();
        }

        public Param_SFO.PARAM_SFO.Table tableItemAdded { get; set; }

        public Param_SFO.PARAM_SFO.index_table Indextableadded { get; set; }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            Param_SFO.PARAM_SFO.index_table indextable = new Param_SFO.PARAM_SFO.index_table();

            Param_SFO.PARAM_SFO.Table tableitem = new Param_SFO.PARAM_SFO.Table();

            if(comboBox1.SelectedIndex == 0)
            {

                indextable.param_data_fmt = Param_SFO.PARAM_SFO.FMT.Utf8Null;
            }
            else
            {

                indextable.param_data_fmt = Param_SFO.PARAM_SFO.FMT.UINT32;
            }

            indextable.param_data_len = Convert.ToUInt32(txtVal.Text.Length);
            indextable.param_data_max_len = Convert.ToUInt32(txtMaxLen.Text);
            tableitem.Indextable = indextable;
            tableitem.Name = txtName.Text.Trim(); 
            tableitem.Value = txtVal.Text;
            tableItemAdded = tableitem;
            Indextableadded = indextable;
            this.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.Close();
        }

        private void txtVal_TextChanged(object sender, EventArgs e)
        {
            txtLen.Text = txtVal.Text.Length.ToString();
        }
    }
}
