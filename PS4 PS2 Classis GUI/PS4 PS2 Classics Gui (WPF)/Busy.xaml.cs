using System;
using System.Collections.Generic;
using System.ComponentModel;
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
    /// Interaction logic for Busy.xaml
    /// </summary>
    public partial class Busy : Window
    {

        System.Windows.Forms.Timer tmrWait = new System.Windows.Forms.Timer();
        

        #region <<< Global Variables >>>

        public BackgroundWorker myWorker;
        public static string INFO = "";

        #endregion < Global Variables >


        public Busy(BackgroundWorker myback)
        {
            InitializeComponent();

            tmrWait.Interval = 10;
            tmrWait.Tick += tmrWait_Tick;

            myWorker = (BackgroundWorker)myback;
            tmrWait.Start();

            lblTask.Text = INFO;
        }

        private void tmrWait_Tick(object sender, EventArgs e)
        {
            if (myWorker.CancellationPending)
            {
                this.Close();
            }
            lblTask.Text = INFO;
        }

        private void Busy_Load(object sender, EventArgs e)
        {
            // this.Location = new Point(this.Location.X, 0);
        }


        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            string doestgisevenload = "";
             //this.Location = new Point(this.Location.X, 0);
        }
    }
}
