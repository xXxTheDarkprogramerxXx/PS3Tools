using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PS4_PS2_Classis_GUI
{
    public partial class Busy : Form
    {

        #region <<< Global Variables >>>

        public BackgroundWorker myWorker;
        public static string INFO = "";

        #endregion < Global Variables >


        public Busy(BackgroundWorker myback)
        {
            InitializeComponent();


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
    }
}
