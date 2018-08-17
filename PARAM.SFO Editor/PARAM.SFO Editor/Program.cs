using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PARAM.SFO_Editor
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        //static void Main()
        //{
        //    Application.EnableVisualStyles();
        //    Application.SetCompatibleTextRenderingDefault(false);
        //    Application.Run(new Form1());
        //}

        static void Main(string[] args)
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            if (AppDomain.CurrentDomain.SetupInformation.ActivationArguments.ActivationData != null)
            {
                try
                {
                    Application.Run(new Form1(AppDomain.CurrentDomain.SetupInformation.ActivationArguments.ActivationData[0].ToString()));
                }
                catch(Exception ex)
                {
                    Application.Run(new Form1());
                }
            }
            else
            {
                Application.Run(new Form1());
            }
        }
    }
}
