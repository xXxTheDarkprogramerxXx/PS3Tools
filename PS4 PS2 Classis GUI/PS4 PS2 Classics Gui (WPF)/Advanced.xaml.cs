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
using System.Windows.Shapes;

namespace PS4_PS2_Classics_Gui__WPF_
{
    /// <summary>
    /// Interaction logic for Advanced.xaml
    /// </summary>
    public partial class Advanced : Window
    {
        public Advanced()
        {
            InitializeComponent();
        }

        public string LabelText
        {
            get
            {
                return this.txtAllInfo.Content.ToString();
            }
            set
            {
                this.txtAllInfo.Content = value;
            }
        }

        private void lblInfo_Loaded(object sender, RoutedEventArgs e)
        {

        }
    }
}
