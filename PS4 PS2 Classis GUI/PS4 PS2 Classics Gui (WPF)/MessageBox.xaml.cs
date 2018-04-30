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
    //do not include this in a class to have it everywhere (use it everywhere)
    public enum PS4_MessageBoxResult
    {
        //
        // Summary:
        //     The message box returns no result.
        None = 0,
        //
        // Summary:
        //     The result value of the message box is OK.
        OK = 1,
        //
        // Summary:
        //     The result value of the message box is Cancel.
        Cancel = 2,
        //
        // Summary:
        //     The result value of the message box is Yes.
        Yes = 6,
        //
        // Summary:
        //     The result value of the message box is No.
        No = 7
    }

    public enum PS4_MessageBoxButton
    {
        //
        // Summary:
        //     The message box displays an OK button.
        OK = 0,
        //
        // Summary:
        //     The message box displays OK and Cancel buttons.
        OKCancel = 1,
        //
        // Summary:
        //     The message box displays Yes, No, and Cancel buttons.
        YesNoCancel = 3,
        //
        // Summary:
        //     The message box displays Yes and No buttons.
        YesNo = 4
    }

    /// <summary>
    /// Interaction logic for MessageBox.xaml
    /// </summary>
    public partial class MessageBox : Window
    {
        public static PS4_MessageBoxResult ReturnResult = PS4_MessageBoxResult.None;//defualt to none
        string Massage = "";
        string Caption = "";
        PS4_MessageBoxButton Buttons = PS4_MessageBoxButton.OK;

        public MessageBox()
        {
            InitializeComponent();
        }



        public MessageBox(string Text, string Caption, PS4_MessageBoxButton _Buttons,SoundClass.Sound SoundToPlay)
        {
            InitializeComponent();
            lblTitle.Content = Caption;
            Buttons = _Buttons;
            txtErrorMessage.Text = Text;

            switch(Buttons)
            {
                case PS4_MessageBoxButton.OK:
                    {
                        btnPositive.Visibility = Visibility.Visible;
                        btnPositive.Content = "OK";
                        btnPositive.Click += BtnPositive_Click;
                    }
                    break;
                case PS4_MessageBoxButton.YesNo:
                    {
                        btnPositive.Visibility = Visibility.Visible;
                        btnPositive.Content = "Yes";
                        btnPositive.Click += BtnPositive_Click;

                        btnNegative.Visibility = Visibility.Visible;
                        btnNegative.Content = "No";
                        btnNegative.Click += BtnNegative_Click; ;
                    }
                    break;

                case PS4_MessageBoxButton.OKCancel:
                    {
                        btnPositive.Visibility = Visibility.Visible;
                        btnPositive.Content = "OK";
                        btnPositive.Click += BtnPositive_Click;

                        btnNegative.Visibility = Visibility.Visible;
                        btnNegative.Content = "Cancel";
                        btnNegative.Click += BtnNegative_Click;
                    }
                    break;
                default:

                    break;
            }

            SoundClass.PlayPS4Sound(SoundToPlay);

        }

        private void BtnNegative_Click(object sender, RoutedEventArgs e)
        {
            switch (Buttons)
            {
                case PS4_MessageBoxButton.OKCancel:
                    ReturnResult = PS4_MessageBoxResult.Cancel;
                    break;
                case PS4_MessageBoxButton.YesNo:
                    ReturnResult = PS4_MessageBoxResult.No;
                    break;
            }


            this.Close();
        }

        private void BtnPositive_Click(object sender, RoutedEventArgs e)
        {
            switch (Buttons)
            {
                case PS4_MessageBoxButton.OK:
                    ReturnResult = PS4_MessageBoxResult.OK;
                    break;
                case PS4_MessageBoxButton.OKCancel:
                    ReturnResult = PS4_MessageBoxResult.OK;
                    break;
                case PS4_MessageBoxButton.YesNo:
                    ReturnResult = PS4_MessageBoxResult.Yes;
                    break;
            }
            this.Close();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

        }
    }
}
