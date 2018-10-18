using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PARAM.SFO_Editor
{
    public partial class RawView : Form
    {
        Param_SFO.PARAM_SFO pSFO;
        Param_SFO.PARAM_SFO.Playstation version;

        public RawView()
        {
            InitializeComponent();
        }

        public RawView(Param_SFO.PARAM_SFO _sfo,Param_SFO.PARAM_SFO.Playstation _version)
        {
            pSFO = _sfo;
            version = _version;
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
                if (item.Name == "CATEGORY")
                {
                    var test = item.Value;
                }

                listBox1.Items.Add("Index : " + item.index + " Name : " + item.Name + " Value : " + item.Value);
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            CreateSFX(pSFO);
        }

        #region << Create SFX >>

        public void CreateSFX(Param_SFO.PARAM_SFO psfo)
        {
            string FileHeader;
            if (version == Param_SFO.PARAM_SFO.Playstation.ps4)
            {
                //table items
                FileHeader = CreateSFXHeader();
                string XMLItem = FileHeader + "\n<paramsfo>";//begin the tag
                foreach (var item in psfo.Tables)
                {
                    if(item.Name == "CATEGORY")
                    {
                       var test = item.Value;
                    }
                    XMLItem += "\n\t<param key=\"" + item.Name + "\">" + item.Value + "</param>";
                }
                XMLItem += "\n</paramsfo>";//close the tag
                SaveFileDialog dlg = new SaveFileDialog();
                dlg.Filter = "PARAM.SFX | PARAM.SFX";
                dlg.DefaultExt = "SFX";
                dlg.AddExtension = true;
                dlg.FileName = "PARAM.SFX";
                if (dlg.ShowDialog() == DialogResult.OK)
                {
                    //user wants to save in a new location or whatever
                    System.IO.File.WriteAllText(dlg.FileName, XMLItem,Encoding.UTF8);
                    MessageBox.Show("File Saved");
                    System.Diagnostics.Process.Start("explorer.exe", System.IO.Path.GetDirectoryName(dlg.FileName));
                }
            }
            else
            {
                MessageBox.Show("SFX For PS3 may be a bit buggy","SFO Extarct",MessageBoxButtons.OK,MessageBoxIcon.Warning);
                FileHeader = CreateSFXHeader();
                string XMLItem = FileHeader + "\n<paramsfo>";//begin the tag
                foreach (var item in psfo.Tables)
                {
                    #region << Get Format >>
                    var frmt = "";
                    if(item.Indextable.param_data_fmt == Param_SFO.PARAM_SFO.FMT.ASCII)
                    {
                        frmt = "utf8-S";
                    }
                    if (item.Indextable.param_data_fmt == Param_SFO.PARAM_SFO.FMT.UINT32)
                    {
                        frmt = "int32";
                    }
                    if (item.Indextable.param_data_fmt == Param_SFO.PARAM_SFO.FMT.UTF_8)
                    {
                        frmt = "utf8";
                    }
                    if (item.Indextable.param_data_fmt == Param_SFO.PARAM_SFO.FMT.Utf8Null)
                    {
                        frmt = "utf8";
                    }
                    #endregion << Get Format >>


                    XMLItem += "\n\t<param key=\"" + item.Name + "\"" + "fmt=\""+ frmt + "\" max_len=\""+item.Indextable.param_data_max_len+"\"" + ">" + item.Value + "</param>";
                }
                XMLItem += "\n</paramsfo>";//close the tag
                SaveFileDialog dlg = new SaveFileDialog();
                dlg.Filter = "PARAM.SFX | PARAM.SFX";
                dlg.DefaultExt = "SFX";
                dlg.AddExtension = true;
                dlg.FileName = "PARAM.SFX";
                if (dlg.ShowDialog() == DialogResult.OK)
                {
                    //user wants to save in a new location or whatever
                    System.IO.File.WriteAllText(dlg.FileName, XMLItem, Encoding.UTF8);
                    MessageBox.Show("File Saved");
                    System.Diagnostics.Process.Start("explorer.exe", System.IO.Path.GetDirectoryName(dlg.FileName));
                }
            }
           

        }

        public string CreateSFXHeader()
        {
            return "<?xml version=\"1.0\" encoding=\"utf-8\" standalone=\"yes\"?>";
        }

        #endregion << Create SFX >>


        public static void WriteToXmlFile<T>(string filePath, T objectToWrite, bool append = false) where T : new()
        {
            TextWriter writer = null;
            try
            {
                var serializer = new System.Xml.Serialization.XmlSerializer(typeof(T));
                writer = new StreamWriter(filePath, append);
                serializer.Serialize(writer, objectToWrite);
            }
            finally
            {
                if (writer != null)
                    writer.Close();
            }
        }

        private void btnSaveAdvanced_Click(object sender, EventArgs e)
        {
            SaveFileDialog dlg = new SaveFileDialog();
            dlg.Filter = "PARAM.TXT | PARAM.TXT";
            dlg.DefaultExt = "TXT";
            dlg.AddExtension = true;
            dlg.FileName = "PARAM.TXT";
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                //user wants to save in a new location or whatever
                WriteToXmlFile(dlg.FileName, pSFO, true);//write the whole sfo as readable text extra feature for some friends
                MessageBox.Show("File Saved");
                System.Diagnostics.Process.Start("explorer.exe", System.IO.Path.GetDirectoryName(dlg.FileName));
            }
        }
    }
}
