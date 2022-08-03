using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace PS4_PSX_Classics_GUI.Views
{
    /// <summary>
    /// Interaction logic for VLC.xaml
    /// </summary>
    public partial class VLC : UserControl
    {
        public static System.Windows.Forms.Integration.WindowsFormsHost _WindowsFormsHost { get; private set; }
       
        public VLC()
        {
            InitializeComponent();
            _WindowsFormsHost = WindowsFormsHost;
        }

        public void UpdateVLCControl()
        {
            WindowsFormsHost = _WindowsFormsHost;
        }
    }
}
