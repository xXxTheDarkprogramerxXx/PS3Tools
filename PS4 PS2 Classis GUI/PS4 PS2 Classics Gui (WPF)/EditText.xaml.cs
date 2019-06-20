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

    public partial class EditText : Window
    {
       

        public EditText()
        {
            InitializeComponent();
        }

        public string _Value { get; set; }
        public EditText(string Value)
        {
            InitializeComponent();
            _Value = Value;
            txtErrorMessage.Text = _Value;
        }


        private void BtnNegative_Click(object sender, RoutedEventArgs e)
        {
           

            this.Close();
        }

        private void BtnPositive_Click(object sender, RoutedEventArgs e)
        {
            _Value = txtErrorMessage.Text.ToString();
            this.Close();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

        }

        private void btnPositive_Click_1(object sender, RoutedEventArgs e)
        {
            _Value = txtErrorMessage.Text.ToString();
            this.Close();
        }

        private void btnNegative_Click_1(object sender, RoutedEventArgs e)
        {
            _Value = txtErrorMessage.Text.ToString();
            this.Close();
        }
    }
}
