using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PKG_Viewer
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        public enum PlaystationSystem
        {
            PSP = 0,
            PSVita = 1,
            PS3 = 2,
            PS4 = 3,
            Unknonw = 99
        }

        /* ps4 pkg types */
        private enum PS4_PKGType
        {
            Game,
            App,
            Addon_Theme,
            Patch,
            Unknown
        }

        public PlaystationSystem CheckPKGSystemType(string PKGLocation)
        {
            PlaystationSystem pstype = PlaystationSystem.Unknonw;

            //read first few bytes magic

            using (FileStream fsSourceDDS = new FileStream(PKGLocation, FileMode.Open, FileAccess.Read))
            using (BinaryReader binaryReader = new BinaryReader(fsSourceDDS))
            {
                fsSourceDDS.Seek(0x0c, SeekOrigin.Begin);
                var with = binaryReader.ReadUInt32();
                fsSourceDDS.Seek(0x10, SeekOrigin.Begin);
                var height = binaryReader.ReadUInt16();
            }

            return pstype;
        }


        #region << PS4 Method (LMAN's Method) >>

        #region << Vars >>

        private string m_pkgfile;

        private bool m_loaded;

        private byte[] sfo_byte;

        private byte[] icon_byte;

        private byte[] pic_byte;

        private byte[] trp_byte;

        private Thread m_thread;

        private bool m_error;

        #endregion << Vars >>
        //private void PkgLoader(string filename)
        //{
        //    try
        //    {
        //        this.m_loaded = false;
        //        this.sfo_byte = null;
        //        this.icon_byte = null;
        //        this.pic_byte = null;
        //        this.trp_byte = null;
        //        this.LabelName.Text = "";
        //        this.pictureBox1.Image = Properties.Resources.images;
        //        try
        //        {
        //            IEnumerator enumerator = this.DataGridViewParams.Columns.GetEnumerator();
        //            while (enumerator.MoveNext())
        //            {
        //                DataGridViewColumn dataGridViewColumn = (DataGridViewColumn)enumerator.Current;
        //                dataGridViewColumn.SortMode = DataGridViewColumnSortMode.NotSortable;
        //                dataGridViewColumn.SortMode = DataGridViewColumnSortMode.Automatic;
        //            }
        //        }
        //        finally
        //        {
        //            IEnumerator enumerator;
        //            if (enumerator is IDisposable)
        //            {
        //                (enumerator as IDisposable).Dispose();
        //            }
        //        }
        //        this.ClearToolStripMenuItem.Enabled = false;
        //        this.OpenLocationToolStripMenuItem.Enabled = false;
        //        this.ToolsToolStripMenuItem.Enabled = false;
        //        if (!File.Exists(filename))
        //        {
        //            throw new Exception("File not found!");
        //        }
        //        List<Utils.Names> list = new List<Utils.Names>();
        //        byte[] array = new byte[]
        //        {
        //            0,
        //            112,
        //            97,
        //            114,
        //            97,
        //            109,
        //            46,
        //            115,
        //            102,
        //            111,
        //            0
        //        };
        //        StringBuilder stringBuilder = new StringBuilder();
        //        using (BinaryReader binaryReader = new BinaryReader(File.OpenRead(filename)))
        //        {
        //            if ((ulong)Utils.ReadUInt32(binaryReader) != 2135117396uL)
        //            {
        //                throw new Exception("Not a valid package! ( Make sure that this is a base PS4 package, not the splited one! )");
        //            }
        //            binaryReader.BaseStream.Seek(24L, SeekOrigin.Begin);
        //            uint num = Utils.ReadUInt32(binaryReader);
        //            uint num2 = Utils.ReadUInt32(binaryReader);
        //            binaryReader.BaseStream.Seek(44L, SeekOrigin.Begin);
        //            uint num3 = Utils.ReadUInt32(binaryReader);
        //            binaryReader.BaseStream.Seek(64L, SeekOrigin.Begin);
        //            string text = Utils.ReadASCIIString(binaryReader, 36);
        //            binaryReader.BaseStream.Seek(119L, SeekOrigin.Begin);
        //            ushort num4 = Utils.ReadUInt16(binaryReader);
        //            Dictionary<long, long> dictionary;
        //            uint num5;
        //            uint num6;
        //            checked
        //            {
        //                binaryReader.BaseStream.Seek((long)(unchecked((ulong)num) + 176uL), SeekOrigin.Begin);
        //                dictionary = new Dictionary<long, long>();
        //                num5 = Utils.ReadUInt32(binaryReader);
        //                num6 = Utils.ReadUInt32(binaryReader);
        //                binaryReader.BaseStream.Seek(binaryReader.BaseStream.Position + 24L, SeekOrigin.Begin);
        //            }
        //            do
        //            {
        //                dictionary.Add((long)((ulong)num5), (long)((ulong)num6));
        //                num5 = Utils.ReadUInt32(binaryReader);
        //                num6 = Utils.ReadUInt32(binaryReader);
        //                binaryReader.BaseStream.Seek(checked(binaryReader.BaseStream.Position + 24L), SeekOrigin.Begin);
        //            }
        //            while ((ulong)(checked(num5 + num6)) > 0uL);
        //            checked
        //            {
        //                int num8;
        //                try
        //                {
        //                    Dictionary<long, long>.Enumerator enumerator2 = dictionary.GetEnumerator();
        //                    while (enumerator2.MoveNext())
        //                    {
        //                        KeyValuePair<long, long> current = enumerator2.Current;
        //                        try
        //                        {
        //                            binaryReader.BaseStream.Seek(current.Key, SeekOrigin.Begin);
        //                            uint num7 = binaryReader.ReadUInt32();
        //                            binaryReader.BaseStream.Seek(current.Key, SeekOrigin.Begin);
        //                            if (unchecked((ulong)num7) == 1179865088uL && this.sfo_byte == null)
        //                            {
        //                                this.sfo_byte = Utils.ReadByte(binaryReader, (int)current.Value);
        //                                break;
        //                            }
        //                        }
        //                        catch (Exception expr_304)
        //                        {
        //                            ProjectData.SetProjectError(expr_304);
        //                            ProjectData.ClearProjectError();
        //                        }
        //                        num8++;
        //                    }
        //                }
        //                finally
        //                {
        //                    Dictionary<long, long>.Enumerator enumerator2;
        //                    ((IDisposable)enumerator2).Dispose();
        //                }
        //                try
        //                {
        //                    try
        //                    {
        //                        Dictionary<long, long>.Enumerator enumerator3 = dictionary.GetEnumerator();
        //                        while (enumerator3.MoveNext())
        //                        {
        //                            KeyValuePair<long, long> current2 = enumerator3.Current;
        //                            binaryReader.BaseStream.Seek(current2.Key, SeekOrigin.Begin);
        //                            if (Utils.Contain(binaryReader.ReadBytes(array.Length), array))
        //                            {
        //                                binaryReader.BaseStream.Seek(current2.Key, SeekOrigin.Begin);
        //                                byte[] array2 = binaryReader.ReadBytes((int)current2.Value);
        //                                int arg_3A5_0 = 1;
        //                                int num9 = array2.Length - 1;
        //                                for (int i = arg_3A5_0; i <= num9; i++)
        //                                {
        //                                    if (array2[i] == 0)
        //                                    {
        //                                        list.Add(new Utils.Names(list.Count, (ulong)dictionary.Keys.ElementAtOrDefault(num8), (ulong)dictionary.Values.ElementAtOrDefault(num8), stringBuilder.ToString()));
        //                                        num8++;
        //                                        stringBuilder.Clear();
        //                                    }
        //                                    stringBuilder.Append(Utils.HexToString(Conversion.Hex(array2[i])));
        //                                }
        //                                break;
        //                            }
        //                        }
        //                    }
        //                    finally
        //                    {
        //                        Dictionary<long, long>.Enumerator enumerator3;
        //                        ((IDisposable)enumerator3).Dispose();
        //                    }
        //                }
        //                catch (Exception expr_435)
        //                {
        //                    ProjectData.SetProjectError(expr_435);
        //                    ProjectData.ClearProjectError();
        //                }
        //                try
        //                {
        //                    List<Utils.Names>.Enumerator enumerator4 = list.GetEnumerator();
        //                    while (enumerator4.MoveNext())
        //                    {
        //                        Utils.Names current3 = enumerator4.Current;
        //                        ToolStripMenuItem toolStripMenuItem = new ToolStripMenuItem(Path.GetFileName(current3.Name));
        //                        toolStripMenuItem.Tag = current3;
        //                        toolStripMenuItem.Click += new EventHandler(this.ExportItem_Click);
        //                        this.ExportFilesToolStripMenuItem.DropDownItems.Add(toolStripMenuItem);
        //                    }
        //                }
        //                finally
        //                {
        //                    List<Utils.Names>.Enumerator enumerator4;
        //                    ((IDisposable)enumerator4).Dispose();
        //                }
        //                Utils.Names names = list.Find((Utils.Names b) => Operators.CompareString(b.Name, "param.sfo", false) == 0);
        //                if (names != null)
        //                {
        //                    binaryReader.BaseStream.Seek((long)names.Offset, SeekOrigin.Begin);
        //                    this.sfo_byte = Utils.ReadByte(binaryReader, (int)names.Size);
        //                }
        //                names = list.Find((Utils.Names b) => Operators.CompareString(b.Name, "icon0.png", false) == 0);
        //                if (names != null)
        //                {
        //                    binaryReader.BaseStream.Seek((long)names.Offset, SeekOrigin.Begin);
        //                    this.icon_byte = Utils.ReadByte(binaryReader, (int)names.Size);
        //                }
        //                else
        //                {
        //                    names = list.Find((Utils.Names b) => Operators.CompareString(b.Name, "icon1.png", false) == 0);
        //                    if (names != null)
        //                    {
        //                        binaryReader.BaseStream.Seek((long)names.Offset, SeekOrigin.Begin);
        //                        this.icon_byte = Utils.ReadByte(binaryReader, (int)names.Size);
        //                    }
        //                }
        //                names = list.Find((Utils.Names b) => Operators.CompareString(b.Name, "trophy/trophy00.trp", false) == 0);
        //                if (names != null)
        //                {
        //                    binaryReader.BaseStream.Seek((long)names.Offset, SeekOrigin.Begin);
        //                    this.trp_byte = Utils.ReadByte(binaryReader, (int)names.Size);
        //                }
        //                else
        //                {
        //                    names = list.Find((Utils.Names b) => Operators.CompareString(b.Name, "trophy/trophy01.trp", false) == 0);
        //                    if (names != null)
        //                    {
        //                        binaryReader.BaseStream.Seek((long)names.Offset, SeekOrigin.Begin);
        //                        this.trp_byte = Utils.ReadByte(binaryReader, (int)names.Size);
        //                    }
        //                }
        //                if (this.sfo_byte != null && this.sfo_byte.Length > 0)
        //                {
        //                    SfoLoader sfoLoader = new SfoLoader();
        //                    sfoLoader.Loadsfo(new MemoryStream(this.sfo_byte));
        //                    try
        //                    {
        //                        List<SfoLoader.Param>.Enumerator enumerator5 = sfoLoader.paramList.GetEnumerator();
        //                        while (enumerator5.MoveNext())
        //                        {
        //                            SfoLoader.Param current4 = enumerator5.Current;
        //                            this.DataGridViewParams.Rows.Add(new object[]
        //                            {
        //                                current4.name,
        //                                RuntimeHelpers.GetObjectValue(current4.data)
        //                            });
        //                        }
        //                    }
        //                    finally
        //                    {
        //                        List<SfoLoader.Param>.Enumerator enumerator5;
        //                        ((IDisposable)enumerator5).Dispose();
        //                    }
        //                    this.LabelName.Text = Conversions.ToString(sfoLoader.GetParam("TITLE").data);
        //                    this.Text = "PS4 Package Viewer - " + this.LabelName.Text;
        //                    string str = Conversions.ToString(sfoLoader.GetParam("CATEGORY").data);
        //                    string text2 = this.GetPkgType(str).ToString().Replace("_", "\\");
        //                    string text3 = Conversions.ToString(sfoLoader.ContainsParam("VERSION") ? Operators.ConcatenateObject("- v", sfoLoader.GetParam("VERSION").data) : "");
        //                    string text4 = sfoLoader.ContainsParam("SYSTEM_VER") ? ("- System v" + Conversion.Hex(RuntimeHelpers.GetObjectValue(sfoLoader.GetParam("SYSTEM_VER").data)).Insert(1, ".").Substring(0, 4)) : "";
        //                    string text5 = (num4 == 6666) ? "( Fake )" : ((num4 == 7747) ? "( Official DP )" : "( Official )");
        //                    this.ToolStripStatusLabelStatus.Text = string.Concat(new string[]
        //                    {
        //                        text2,
        //                        " ",
        //                        text5,
        //                        " ",
        //                        text3,
        //                        " ",
        //                        text4
        //                    });
        //                }
        //                if (this.icon_byte != null && this.icon_byte.Length > 0)
        //                {
        //                    this.PictureBoxIcon.Image = Utils.BytesToImage(this.icon_byte);
        //                }
        //                else if (this.trp_byte != null && this.trp_byte.Length > 0)
        //                {
        //                    TRPReader tRPReader = new TRPReader();
        //                    tRPReader.Load(this.trp_byte);
        //                    this.icon_byte = tRPReader.ExtractFileToMemory("ICON0.PNG");
        //                    if (this.icon_byte != null && this.icon_byte.Length > 0)
        //                    {
        //                        this.PictureBoxIcon.Image = Utils.BytesToImage(this.icon_byte);
        //                    }
        //                }
        //            }
        //        }
        //        this.m_loaded = true;
        //        this.DataGridViewParams.Update();
        //        this.ClearToolStripMenuItem.Enabled = true;
        //        this.OpenLocationToolStripMenuItem.Enabled = true;
        //        this.ToolsToolStripMenuItem.Enabled = true;
        //        this.ExportFilesToolStripMenuItem.Enabled = (list.Count > 0);
        //    }
        //    catch (Exception expr_8F7)
        //    {
        //        ProjectData.SetProjectError(expr_8F7);
        //        Exception ex = expr_8F7;
        //        this.m_loaded = false;
        //        this.ClearToolStripMenuItem.Enabled = true;
        //        this.OpenLocationToolStripMenuItem.Enabled = true;
        //        this.ToolsToolStripMenuItem.Enabled = true;
        //        this.ToolStripStatusLabelStatus.Text = ex.Message;
        //        ProjectData.ClearProjectError();
        //    }
        //}

        #endregion << PS4 Method >>

        private void button1_Click(object sender, EventArgs e)
        {
            //Open File Dialog For ISO Files
            OpenFileDialog thedialog = new OpenFileDialog();
            thedialog.Title = "Select PKG File";
            thedialog.Filter = "PS4 PKG File|*.pkg";
            thedialog.Multiselect = true;
            thedialog.InitialDirectory = Environment.SpecialFolder.MyComputer.ToString();
            if (thedialog.ShowDialog() == DialogResult.OK)
            {
                //load the pkg info
                //check if its ps3 or ps4 or psv
                CheckPKGSystemType(thedialog.FileName.ToString());
            }
        }

        private string AppCommonPath()
        {
            string returnstring = "";

            returnstring = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\Ps4Tools\";

            return returnstring;
        }

        public void ExtractAllResources()
        {
            if (!Directory.Exists(AppCommonPath()))
            {
                Directory.CreateDirectory(AppCommonPath());
            }
            //copy byte files
            System.IO.File.WriteAllBytes(AppCommonPath() + "orbis-pub-cmd.exe", Properties.Resources.orbis_pub_cmd);
        }


        private void Form1_Load(object sender, EventArgs e)
        {
            MessageBox.Show("Untill a key is found to unpack PS4's pkg's we can't decrypt any ps4 pkg on the fly for now this tool only supports my ps4 pkg's made with my tools as well as some other dev pkg's that use passcode zero\n\nThis can however read non encrypted files from the PKG itself");
            ExtractAllResources();
        }

        private void button2_Click(object sender, EventArgs e)
        {

        }
    }
}
