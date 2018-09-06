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
    public partial class CustomLookup : Form
    {
        private IEnumerable<Param_SFO.PARAM_SFO.DataTypes> temp;

        public CustomLookup()
        {
            InitializeComponent();
        }

        public CustomLookup(List<Enum> tempholder)
        {
            InitializeComponent();

            comboBox1.Items.AddRange(tempholder.ToArray());
        }

        public CustomLookup(string usercurrenttype)
        {
            // TODO: Complete member initialization
            InitializeComponent();
            this.temp = temp;
            comboBox1.DataSource = Enum.GetValues(typeof(Param_SFO.PARAM_SFO.DataTypes));
            //comboBox1.SelectedItem = (Param_SFO.PARAM_SFO.DataTypes)(usercurrenttype);
            comboBox1.SelectedItem = Param_SFO.PARAM_SFO.DataTypes.PS4_Game_Application_Patch;
        }

        private void CustomLookup_Load(object sender, EventArgs e)
        {

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            Param_SFO.PARAM_SFO.DataTypes enums = (Param_SFO.PARAM_SFO.DataTypes)comboBox1.SelectedItem;

            uint enumval = (uint)enums;

            for (int i = 0; i < Form1.psfo.Tables.Count; i++)
            {
                if (Form1.psfo.Tables[i].Name == "CATEGORY")
                {
                    label2.Text = "Value Buffer : " + System.Text.Encoding.ASCII.GetString(Form1.psfo.Tables[i].ValueBuffer).Replace("\0","");
                    var hex = (BitConverter.ToString(Form1.psfo.Tables[i].ValueBuffer, 0, Convert.ToInt32(Form1.psfo.Tables[i].Indextable.param_data_max_len))).ToString().Replace("-", string.Empty);
                    var temp = Convert.ToInt32(hex).ToString("X4");
                    label3.Text = "Value Hex : " + hex;
                    label4.Text = "Value Byte Array : " + temp;
                }
            }

            //display all values
            //label2.Text = "Value Buffer :"+Form1.psfo.Category;

        }
    }
}
